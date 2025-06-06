using LunaR.ConsoleUtils;
using LunaR.Extensions;
using LunaR.Modules;
using LunaR.Wrappers;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase;

namespace LunaR.Patching
{
    internal class EventValidation
    {
        public static List<int> ActorRPCBlock = new();
        public static List<int> ActorUSpeakBlock = new();
        public static List<int> ActorParameterBlock = new();
        public static List<int> ActorMovementBlock = new();
        public static List<int> ActorOwnershipBlock = new();

        public static List<string> RPCBlockedUsers = new();

        public static List<string> BlacklistedBytes = new();

        public static void ClearEventBlocks()
        {
            ActorRPCBlock.Clear();
            ActorUSpeakBlock.Clear();
            ActorParameterBlock.Clear();
            ActorMovementBlock.Clear();
            ActorOwnershipBlock.Clear();
        }

        public static void LimitActor(int Sender, byte Event, int Seconds = 20)
        {
            switch (Event)
            {
                case 1:
                    ActorUSpeakBlock.Add(Sender);
                    Utils.DelayAction(Seconds, delegate
                    {
                        ActorUSpeakBlock.Remove(Sender);
                    }).Start();
                    break;

                case 6:
                    ActorRPCBlock.Add(Sender);
                    Utils.DelayAction(Seconds, delegate
                    {
                        ActorRPCBlock.Remove(Sender);
                    }).Start();
                    break;

                case 7:
                    ActorMovementBlock.Add(Sender);
                    Utils.DelayAction(Seconds, delegate
                    {
                        ActorMovementBlock.Remove(Sender);
                    }).Start();
                    break;

                case 9:
                    ActorParameterBlock.Add(Sender);
                    Utils.DelayAction(Seconds, delegate
                    {
                        ActorParameterBlock.Remove(Sender);
                    }).Start();
                    break;

                case 209:
                    ActorOwnershipBlock.Add(Sender);
                    Utils.DelayAction(Seconds, delegate
                    {
                        ActorOwnershipBlock.Remove(Sender);
                    }).Start();
                    break;

                case 210:
                    ActorOwnershipBlock.Add(Sender);
                    Utils.DelayAction(Seconds, delegate
                    {
                        ActorOwnershipBlock.Remove(Sender);
                    }).Start();
                    break;
            }
            Player PhotonPlayer = PhotonExtensions.GetPhotonPlayer(Sender);
            if (PhotonPlayer != null)
            {
                VRC.Player player = PhotonPlayer.GetPlayer();
                string DisplayName = player == null ? PhotonPlayer.GetDisplayName() : player.DisplayName();
                Extensions.Logger.Log($"Limiting {DisplayName} for {Seconds} Seconds [Event {Event}]", Extensions.Logger.LogsType.Protection);
                VRConsole.Log(VRConsole.LogsType.Protection, $"{DisplayName} --> Invalid Event [{Event}]");
            }
        }

        public static bool IsValidServertime(int Time, int difference = 30000)
        {
            if (Time < Utils.VRCNetworkingClient.prop_LoadBalancingPeer_0.ServerTimeInMilliSeconds - difference || Time > Utils.VRCNetworkingClient.prop_LoadBalancingPeer_0.ServerTimeInMilliSeconds + difference) return false;
            return true;
        }

        public enum DiscardEventState
        {
            None,
            Once,
            Ratelimit
        }

        public static bool CheckOwnership(int[] ViewIDs, int Sender)
        {
            if (ViewIDs[0] < 0 || ViewIDs[0] > 999999999) return false;
            if (ViewIDs[1] < 0 || ViewIDs[1] > 999999999) return false;
            Player PhotonPlayer209 = PhotonExtensions.GetPhotonPlayer(Sender);
            if (PhotonPlayer209 == null || PhotonPlayer209.GetPlayer() == null) return false;
            return true;
        }

        public static bool CheckInstantiation(Il2CppSystem.Collections.Hashtable Data)
        {
            if (Data.ContainsKey("6") && !IsValidServertime(Data["6"].Unbox<int>(), 20000)) return false;
            return true;
        }

        public static DiscardEventState CheckParameter(byte[] Data, int Sender)
        {
            if (Data.Length < 11 || Data.Length > 400) return DiscardEventState.Ratelimit;

            int Actor = BitConverter.ToInt32(Data, 0);

            if (Actor.ToString().EndsWith("00003"))
            {
                if (Actor != int.Parse(Sender + "00003")) return DiscardEventState.Ratelimit;
                if (BotDetection.MissingSync3.Contains(Sender)) BotDetection.MissingSync3.Remove(Sender);
                string Base64 = Convert.ToBase64String(Data);
                if (BlacklistedBytes.Contains(Base64)) return DiscardEventState.Ratelimit;
                BlacklistedBytes.Add(Base64);
            }
            else if (Actor.ToString().EndsWith("00001"))
            {
                if (Actor != int.Parse(Sender + "00001")) return DiscardEventState.Ratelimit;
                if (BotDetection.MissingSync1.Contains(Sender)) BotDetection.MissingSync1.Remove(Sender);
                string Base64 = Convert.ToBase64String(Data);
                if (BlacklistedBytes.Contains(Base64)) return DiscardEventState.Ratelimit;
                BlacklistedBytes.Add(Base64);
            }
            else
            {
                if (Actor < 0 || Actor > 999999999) return DiscardEventState.Ratelimit;
                string Base64 = Convert.ToBase64String(Data);
                if (BlacklistedBytes.Contains(Base64)) return DiscardEventState.Once;
                BlacklistedBytes.Add(Base64);
            }

            return DiscardEventState.None;
        }

        public static DiscardEventState CheckMovement(byte[] Data, int Sender)
        {
            if (Data.Length < 11 || Data.Length > 370) return DiscardEventState.Ratelimit;

            if (!IsValidServertime(BitConverter.ToInt32(Data, 4), 45000)) return DiscardEventState.Ratelimit;
            int Actor = BitConverter.ToInt32(Data, 0);
            if (Actor.ToString().EndsWith("00001"))
            {
                if (Actor != int.Parse(Sender + "00001")) return DiscardEventState.Ratelimit;

                if (BotDetection.MissingSync1.Contains(Sender) || BotDetection.MissingSync3.Contains(Sender) || BotDetection.MissingModeration.Contains(Sender)) return DiscardEventState.Once;

                PatchExtensions.SerializedActors[Sender] = Environment.TickCount;
            }
            else if (Actor < 0 || Actor > 999999999) return DiscardEventState.Ratelimit;

            string Base64 = Convert.ToBase64String(Data.Take(Data.Length - 5).Skip(8).ToArray());
            if (BlacklistedBytes.Contains(Base64)) return DiscardEventState.Ratelimit; // was once before might need rework tho
            BlacklistedBytes.Add(Base64);

            return DiscardEventState.None;
        }

        public static DiscardEventState CheckRPC(byte[] Data, int Sender)   
        {
            if (Data.Length < 25 || Data.Length > 370) return DiscardEventState.Ratelimit;

            if (!IsValidServertime(BitConverter.ToInt32(Data, 1), 45000)) return DiscardEventState.Ratelimit;

            Il2CppSystem.Object obj;
            try
            {
                BinarySerializer.Method_Public_Static_Boolean_ArrayOf_Byte_byref_Object_0(Data, out obj);
                if (obj == null) return DiscardEventState.Ratelimit;
            }
            catch 
            {
                return DiscardEventState.Ratelimit;
            }

            VRC_EventLog.EventLogEntry evtLogEntry = obj.TryCast<VRC_EventLog.EventLogEntry>();
            if (evtLogEntry == null) return DiscardEventState.Ratelimit;

            if (evtLogEntry.field_Private_Int32_1 != Sender) return DiscardEventState.Ratelimit;
            if (evtLogEntry.prop_VrcEvent_0 == null) return DiscardEventState.Ratelimit;
            if (evtLogEntry.prop_VrcEvent_0.EventType > VRC_EventHandler.VrcEventType.CallUdonMethod || evtLogEntry.prop_VrcEvent_0.EventType < VRC_EventHandler.VrcEventType.MeshVisibility) return DiscardEventState.Ratelimit;
            if (evtLogEntry.prop_VrcEvent_0.ParameterString == null) return DiscardEventState.Ratelimit;

            GameObject DecodedObject;
            try
            {
                DecodedObject = Network.Method_Public_Static_GameObject_String_Boolean_0(evtLogEntry.prop_String_0);
            }
            catch 
            { 
                return DiscardEventState.Ratelimit; 
            }
            if (DecodedObject != null && (PatchExtensions.IsBadPosition(DecodedObject.transform.position) || PatchExtensions.IsBadRotation(DecodedObject.transform.rotation))) return DiscardEventState.Ratelimit;

            try
            {
                if (ParameterSerialization.Method_Public_Static_ArrayOf_Object_ArrayOf_Byte_0(evtLogEntry.prop_VrcEvent_0.ParameterBytes) == null) return DiscardEventState.Ratelimit;
            }
            catch 
            {
                return DiscardEventState.Ratelimit;
            }

            string Base64 = Convert.ToBase64String(Data);
            if (BlacklistedBytes.Contains(Base64))
            {
                if (evtLogEntry.prop_VrcEvent_0.ParameterString == "UdonSyncRunProgramAsRPC") return DiscardEventState.Once;
                return DiscardEventState.Ratelimit;
            }
            BlacklistedBytes.Add(Base64);

            return DecodeRPCEvent(Data, PhotonExtensions.GetPhotonPlayer(Sender));
        }

        public static DiscardEventState CheckUSpeak(byte[] Data, int Sender)
        {
            if (Data.Length < 25 || Data.Length > 1300) return DiscardEventState.Ratelimit;

            if (BitConverter.ToInt32(Data, 0) != Sender) return DiscardEventState.Ratelimit;

            if (!IsValidServertime(BitConverter.ToInt32(Data, 4), 45000)) return DiscardEventState.Ratelimit;

            string Base64 = Convert.ToBase64String(Data.Skip(8).ToArray());
            if (BlacklistedBytes.Contains(Base64)) return DiscardEventState.Ratelimit;
            BlacklistedBytes.Add(Base64);

            return DiscardEventState.None;
        }

        public static bool CheckCache(byte[][] Data)
        {
            if (Data.Length > 50)
            {
                VRConsole.Log(VRConsole.LogsType.Protection, $"Cache Disconnect [{Data.Length}]");
                Extensions.Logger.Log($"Prevented Disconnect with Cache Amount [{Data.Length}]", Extensions.Logger.LogsType.Protection);
                return false;
            }
            foreach (byte[] Array in Data)
            {
                if (Array.Length > 220)
                {
                    VRConsole.Log(VRConsole.LogsType.Protection, $"Cache Disconnect [{Array.Length}]");
                    Extensions.Logger.Log($"Prevented Disconnect with Cache Bytes [{Array.Length}]", Extensions.Logger.LogsType.Protection);
                    return false;
                }
            }
            return true;
        }

        public static bool CheckWorldID(string ID, int? Playercount = null)
        {
            if (!ID.Contains(":")) return false;
            string WorldID = ID.Split(':')[0];
            string InstanceID = ID.Split(':')[1];

            if (WorldID.Length != 41 || InstanceID.Length < 1) return false;
            if (!WorldID.StartsWith("wrld_")) return false;
            if (WorldID == "wrld_5b89c79e-c340-4510-be1b-476e9fcdedcc") return false;
            if (!WorldID.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '-')) return false;
            if (!InstanceID.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '-' || c == '(' || c == ')' || c == '~')) return false;
            if (InstanceID.Contains("~private") || InstanceID.Contains("~hidden") || InstanceID.Contains("~friends") || InstanceID.Contains("~canRequestInvite"))
            {
                if (InstanceID.Contains("~canRequestInvite") && !InstanceID.Contains("~private")) return false;
                if (!InstanceID.Contains("~nonce(")) return false;
                if (!InstanceID.Contains("(usr_")) return false;
            }
            else
            {
                int ExtraCount = InstanceID.Count(c => c == '~');
                if (InstanceID.Contains("~region"))
                {
                    if (ExtraCount > 1) return false;
                }
                else if (ExtraCount > 0) return false;
                if (InstanceID.Length > 120) return false;
            }
            if (InstanceID.Contains("~region") && !InstanceID.Contains("~region(use)") && !InstanceID.Contains("~region(eu)") && !InstanceID.Contains("~region(jp)") && !InstanceID.Contains("~region(us)")) return false;
            if (Playercount != null && (Playercount < 0 || Playercount > 80)) return false;
            return true;
        }


        public static DiscardEventState DecodeRPCEvent(byte[] EventData, Player PhotonSender)
        {
            try
            {
                BinarySerializer.Method_Public_Static_Boolean_ArrayOf_Byte_byref_Object_0(EventData, out Il2CppSystem.Object obj);
                VRC_EventLog.EventLogEntry evtLogEntry = obj.TryCast<VRC_EventLog.EventLogEntry>();

                GameObject TargetObject = Network.Method_Public_Static_GameObject_String_Boolean_0(evtLogEntry.prop_String_0);
                VRC.Player VRCPlayer = PhotonSender.GetPlayer();

                string[] DecodedParameterBytes = new string[evtLogEntry.prop_VrcEvent_0.ParameterBytes.Count];
                if (evtLogEntry.prop_VrcEvent_0.ParameterBytes != null)
                {
                    Il2CppSystem.Object[] ParamBytes = ParameterSerialization.Method_Public_Static_ArrayOf_Object_ArrayOf_Byte_0(evtLogEntry.prop_VrcEvent_0.ParameterBytes);
                    for (int i = 0; i < ParamBytes.Length; i++)
                    {
                        DecodedParameterBytes[i] = Il2CppSystem.Convert.ToString(ParamBytes[i]);
                    }
                }

                if (PatchExtensions.RPCLog)
                {
                    string DecodedBytes = DecodedParameterBytes.Length > 0 ? string.Join(" | ", DecodedParameterBytes.Where(b => b != null)) : "NULL";
                    string TargetObjects = TargetObject != null ? TargetObject.name : "NULL";
                    string ObjectPosition = TargetObject != null ? TargetObject.transform.position.ToString() : "NULL";

                    Extensions.Logger.Log(string.Concat(new object[]
                    {
                            Environment.NewLine,
                            $"======= RPC EVENT =======", Environment.NewLine,
                            $"PHOTON SENDER: {VRCPlayer.DisplayName()} | Actor: {PhotonSender.ActorID()}", Environment.NewLine,
                            $"ACTOR: {evtLogEntry.field_Private_Int32_1}", Environment.NewLine,
                            $"PARAMETER BOOL: {evtLogEntry.prop_VrcEvent_0.ParameterBool}", Environment.NewLine,
                            $"BOOL OP: {evtLogEntry.prop_VrcEvent_0.ParameterBoolOp}", Environment.NewLine,
                            $"BYTES DECODED: {DecodedBytes}", Environment.NewLine,
                            $"BYTES VERSION: {evtLogEntry.prop_VrcEvent_0.ParameterBytesVersion.Unbox<int>()}", Environment.NewLine,
                            $"OBJECT PATH: {evtLogEntry.prop_String_0}", Environment.NewLine,
                            $"OBJECT TARGET: {TargetObjects}", Environment.NewLine,
                            $"OBJECT POSITION: {ObjectPosition}", Environment.NewLine,
                            $"PARAMETER STRING: {evtLogEntry.prop_VrcEvent_0.ParameterString}", Environment.NewLine,
                            $"PARAMETER FLOAT: {evtLogEntry.prop_VrcEvent_0.ParameterFloat}", Environment.NewLine,
                            $"PARAMETER INT: {evtLogEntry.prop_VrcEvent_0.ParameterInt}", Environment.NewLine,
                            $"BROADCAST TYPE: {evtLogEntry.field_Public_VrcBroadcastType_0}", Environment.NewLine,
                            $"EVENT TYPE: {evtLogEntry.prop_VrcEvent_0.EventType}", Environment.NewLine,
                            $"TAKE OWNERSHIP: {evtLogEntry.prop_VrcEvent_0.TakeOwnershipOfTarget}", Environment.NewLine,
                            $"========= END =========",
                    }), Extensions.Logger.LogsType.Clean);
                }

                if (RPCBlockedUsers.Contains(VRCPlayer.UserID())) return DiscardEventState.Once;

                switch (evtLogEntry.prop_VrcEvent_0.ParameterString)
                {
                    case "ConfigurePortal":
                        if (PhotonSender.ActorID() != Utils.VRCNetworkingClient.GetSelf().ActorID())
                        {
                            int PlayerCount = Convert.ToInt32(DecodedParameterBytes[2]);
                            if (DecodedParameterBytes[2] == null || !CheckWorldID(DecodedParameterBytes[0] + ":" + DecodedParameterBytes[1], PlayerCount))
                            {
                                Extensions.Logger.Log($"{VRCPlayer.DisplayName()} spawned Invalid Portal [{DecodedParameterBytes[0]}:{DecodedParameterBytes[1]}] [{DecodedParameterBytes[2]}]", Extensions.Logger.LogsType.Protection);
                                VRConsole.Log(VRConsole.LogsType.Protection, $"{VRCPlayer.DisplayName()} --> Invalid Portal");
                                return DiscardEventState.Ratelimit;
                            }
                            if (PatchExtensions.AntiPortal == PatchExtensions.AntiPortalMode.All || PatchExtensions.AntiPortal == PatchExtensions.AntiPortalMode.Friends && !VRCPlayer.IsFriend()) return DiscardEventState.Once;
                        }

                        VRConsole.Log(VRConsole.LogsType.Portal, $"{VRCPlayer.DisplayName()} --> Portaldrop");
                        Extensions.Logger.Log($"{VRCPlayer.DisplayName()} spawned a Portal [{DecodedParameterBytes[0]}:{DecodedParameterBytes[1]}] [{DecodedParameterBytes[2]}]", Extensions.Logger.LogsType.Info);
                        break;

                    case "_DestroyObject":
                        if (PhotonSender.ActorID() != Utils.VRCNetworkingClient.GetSelf().ActorID() && PatchExtensions.AntiDestroy)
                        {
                            VRConsole.Log(VRConsole.LogsType.Protection, $"{VRCPlayer.DisplayName()} --> Destroy [ID: {DecodedParameterBytes[0]}]");
                            Extensions.Logger.Log($"Prevented {VRCPlayer.DisplayName()} from destroying an Object [ID: {DecodedParameterBytes[0]}]", Extensions.Logger.LogsType.Protection);
                            return DiscardEventState.Once;
                        }

                        VRConsole.Log(VRConsole.LogsType.Object, $"{VRCPlayer.DisplayName()} --> Destroy [ID: {DecodedParameterBytes[0]}]");
                        Extensions.Logger.Log($"{VRCPlayer.DisplayName()} destroyed an Object [ID: {DecodedParameterBytes[0]}]", Extensions.Logger.LogsType.Info);
                        break;

                    case "SpawnEmojiRPC":
                        int Emoji = Convert.ToInt32(DecodedParameterBytes[0]);
                        if (PhotonSender.ActorID() != Utils.VRCNetworkingClient.GetSelf().ActorID())
                        {
                            if (Emoji < 0 || Emoji > 57)
                            {
                                Extensions.Logger.Log($"{VRCPlayer.DisplayName()} used Invalid Emoji [{Emoji}]", Extensions.Logger.LogsType.Protection);
                                VRConsole.Log(VRConsole.LogsType.Protection, $"{VRCPlayer.DisplayName()} --> Invalid Emoji [{Emoji}]");
                                return DiscardEventState.Ratelimit;
                            }
                            if (PatchExtensions.AntiEmoji) return DiscardEventState.Once;
                        }


                        Il2CppSystem.Collections.Generic.List<VRC.UI.Client.Emoji.EmojiData> emojis = VRC.DataModel.VRCData.field_Public_Static_IEmojis_0.prop_Observable_1_IList_0.field_Protected_TYPE_0.Cast<Il2CppSystem.Collections.Generic.List<VRC.UI.Client.Emoji.EmojiData>>();
                        VRConsole.Log(VRConsole.LogsType.Emoji, $"{VRCPlayer.DisplayName()} --> {emojis[Emoji].Name} [{Emoji}]");
                        Extensions.Logger.Log($"{VRCPlayer.DisplayName()} used Emoji {emojis[Emoji].Name} [{Emoji}]", Extensions.Logger.LogsType.Info);
                        break;

                    case "PlayEmoteRPC":
                        int Emote = Convert.ToInt32(DecodedParameterBytes[0]);
                        if (PhotonSender.ActorID() != Utils.VRCNetworkingClient.GetSelf().ActorID() && (Emote < 1 || Emote > 8))
                        {
                            Extensions.Logger.Log($"{VRCPlayer.DisplayName()} used Invalid Emote [{Emote}]", Extensions.Logger.LogsType.Protection);
                            VRConsole.Log(VRConsole.LogsType.Protection, $"{VRCPlayer.DisplayName()} --> Invalid Emote [{Emote}]");
                            return DiscardEventState.Ratelimit;
                        }

                        VRConsole.Log(VRConsole.LogsType.Emote, $"{VRCPlayer.DisplayName()} --> {(RPCHandler.Emotes)Emote} [{Emote}]");
                        Extensions.Logger.Log($"{VRCPlayer.DisplayName()} used Emote {(RPCHandler.Emotes)Emote} [{Emote}]", Extensions.Logger.LogsType.Info);
                        break;

                    case "CancelRPC":
                        VRConsole.Log(VRConsole.LogsType.Emote, $"{VRCPlayer.DisplayName()} --> Cancel Emote");
                        Extensions.Logger.Log($"{VRCPlayer.DisplayName()} canceled Emote", Extensions.Logger.LogsType.Info);
                        break;

                    case "ReloadAvatarNetworkedRPC":
                        VRConsole.Log(VRConsole.LogsType.Avatar, $"{VRCPlayer.DisplayName()} --> Reload Avatar");
                        Extensions.Logger.Log($"{VRCPlayer.DisplayName()} used Avatar Reload", Extensions.Logger.LogsType.Avatar);
                        break;

                    case "SwitchAvatar":
                        if (PhotonSender.ActorID() != Utils.VRCNetworkingClient.GetSelf().ActorID())
                        {
                            VRConsole.Log(VRConsole.LogsType.Protection, $"{VRCPlayer.DisplayName()} --> Pedestal Switch");
                            Extensions.Logger.Log($"Prevented {VRCPlayer.DisplayName()} from switching Pedestal [{DecodedParameterBytes[0]}]", Extensions.Logger.LogsType.Protection);
                            return DiscardEventState.Ratelimit;
                        }

                        VRConsole.Log(VRConsole.LogsType.Pedestal, $"{VRCPlayer.DisplayName()} --> Avatar Switch");
                        Extensions.Logger.Log($"{VRCPlayer.DisplayName()} switched a Pedestal", Extensions.Logger.LogsType.Info);
                        break;

                    case "RefreshAvatar":
                        VRConsole.Log(VRConsole.LogsType.Pedestal, $"{VRCPlayer.DisplayName()} --> Avatar Refresh");
                        Extensions.Logger.Log($"{VRCPlayer.DisplayName()} refreshed a Pedestal", Extensions.Logger.LogsType.Info);
                        break;

                    case "SetAvatarUse":
                        if (PhotonSender.ActorID() != Utils.VRCNetworkingClient.GetSelf().ActorID())
                        {
                            VRConsole.Log(VRConsole.LogsType.Protection, $"{VRCPlayer.DisplayName()} --> Avatar Use");
                            Extensions.Logger.Log($"Prevented {VRCPlayer.DisplayName()} from using a Pedestal", Extensions.Logger.LogsType.Protection);
                            return DiscardEventState.Ratelimit;
                        }

                        VRConsole.Log(VRConsole.LogsType.Pedestal, $"{VRCPlayer.DisplayName()} --> Avatar Use");
                        Extensions.Logger.Log($"{VRCPlayer.DisplayName()} used a Pedestal", Extensions.Logger.LogsType.Info);
                        break;

                    case "PlayEffect":
                        VRConsole.Log(VRConsole.LogsType.Portal, $"{VRCPlayer.DisplayName()} --> Portal Enter");
                        Extensions.Logger.Log($"{VRCPlayer.DisplayName()} entered a Portal", Extensions.Logger.LogsType.Info);
                        if (PhotonSender.ActorID() == Utils.VRCNetworkingClient.GetSelf().ActorID()) return DiscardEventState.Once;
                        break;

                    case "_InstantiateObject":
                        if (PhotonSender.ActorID() != Utils.VRCNetworkingClient.GetSelf().ActorID() && !DecodedParameterBytes[3].StartsWith(VRCPlayer.GetPhotonPlayer().ActorID().ToString()))
                        {
                            Extensions.Logger.Log($"{VRCPlayer.DisplayName()} instantiated Invalid Object [ID: {DecodedParameterBytes[3]}]", Extensions.Logger.LogsType.Protection);
                            VRConsole.Log(VRConsole.LogsType.Protection, $"{VRCPlayer.DisplayName()} --> Invalid Object [ID: {DecodedParameterBytes[3]}]");
                            return DiscardEventState.Ratelimit;
                        }

                        VRConsole.Log(VRConsole.LogsType.Object, $"{VRCPlayer.DisplayName()} --> Instantiate [ID: {DecodedParameterBytes[3]}]");
                        Extensions.Logger.Log($"{VRCPlayer.DisplayName()} instantiated Object [ID: {DecodedParameterBytes[3]}]", Extensions.Logger.LogsType.Info);
                        break;

                    case "ChangeVisibility":
                        if (Convert.ToBoolean(DecodedParameterBytes[0]) == true)
                        {
                            VRConsole.Log(VRConsole.LogsType.Camera, $"{VRCPlayer.DisplayName()} --> Show");
                            Extensions.Logger.Log($"{VRCPlayer.DisplayName()} showed the Camera", Extensions.Logger.LogsType.Info);
                        }
                        else
                        {
                            VRConsole.Log(VRConsole.LogsType.Camera, $"{VRCPlayer.DisplayName()} --> Hide");
                            Extensions.Logger.Log($"{VRCPlayer.DisplayName()} hide the Camera", Extensions.Logger.LogsType.Info);
                        }
                        break;

                    case "PhotoCapture":
                        VRConsole.Log(VRConsole.LogsType.Camera, $"{VRCPlayer.DisplayName()} --> Photo");
                        Extensions.Logger.Log($"{VRCPlayer.DisplayName()} took a Photo", Extensions.Logger.LogsType.Info);
                        break;

                    case "UdonSyncRunProgramAsRPC":
                        if (PatchExtensions.AntiUdon)
                        {
                            Extensions.Logger.Log($"{VRCPlayer.DisplayName()} used Udon Event [{DecodedParameterBytes[0]}]", Extensions.Logger.LogsType.Protection);
                            VRConsole.Log(VRConsole.LogsType.Protection, $"{VRCPlayer.DisplayName()} --> Udon [{DecodedParameterBytes[0]}]");
                            return DiscardEventState.Once;
                        }
                        break;

                    case "EnableMeshRPC":
                        if (PhotonSender.ActorID() != Utils.VRCNetworkingClient.GetSelf().ActorID())
                        {
                            VRConsole.Log(VRConsole.LogsType.Protection, $"{VRCPlayer.DisplayName()} --> Admin RPC [SetPresetRPC]");
                            Extensions.Logger.Log($"Prevented {VRCPlayer.DisplayName()} from using an Admin RPC [SetPresetRPC]", Extensions.Logger.LogsType.Protection);
                            return DiscardEventState.Ratelimit;
                        }
                        break;

                    case "SetPresetRPC":
                        if (PhotonSender.ActorID() != Utils.VRCNetworkingClient.GetSelf().ActorID())
                        {
                            VRConsole.Log(VRConsole.LogsType.Protection, $"{VRCPlayer.DisplayName()} --> Admin RPC [SetPresetRPC]");
                            Extensions.Logger.Log($"Prevented {VRCPlayer.DisplayName()} from using an Admin RPC [SetPresetRPC]", Extensions.Logger.LogsType.Protection);
                            return DiscardEventState.Ratelimit;
                        }
                        break;

                    case "TimerBloop":
                        if (PhotonSender.ActorID() != Utils.VRCNetworkingClient.GetSelf().ActorID() && PatchExtensions.AntiTimer) return DiscardEventState.Once;
                        break;

                    case "SyncWorldInstanceIdRPC":
                        if (PhotonSender.ActorID() != Utils.VRCNetworkingClient.GetSelf().ActorID() && !CheckWorldID($"{RoomManager.field_Internal_Static_ApiWorld_0.id}:{DecodedParameterBytes[0]}"))
                        {
                            VRConsole.Log(VRConsole.LogsType.Protection, $"{VRCPlayer.DisplayName()} --> Invalid Portal Sync [ID: {DecodedParameterBytes[0]}]");
                            Extensions.Logger.Log($"Prevented {VRCPlayer.DisplayName()} from using Invalid Portal Sync [ID: {DecodedParameterBytes[0]}]", Extensions.Logger.LogsType.Protection);
                            return DiscardEventState.Ratelimit;
                        }

                        VRConsole.Log(VRConsole.LogsType.Portal, $"{VRCPlayer.DisplayName()} --> Portal Sync");
                        Extensions.Logger.Log($"{VRCPlayer.DisplayName()} synced Portal Instance [{DecodedParameterBytes[0]}]", Extensions.Logger.LogsType.Info);
                        break;

                    case "SetTimerRPC":
                        if (PhotonSender.ActorID() != Utils.VRCNetworkingClient.GetSelf().ActorID())
                        {
                            float Time = Convert.ToSingle(DecodedParameterBytes[0]);
                            if (Time < 0 || Time > 31)
                            {
                                VRConsole.Log(VRConsole.LogsType.Protection, $"{VRCPlayer.DisplayName()} --> Invalid Timer [{Time}]");
                                Extensions.Logger.Log($"{VRCPlayer.DisplayName()} used a Invalid Portal Timer [{Time}]", Extensions.Logger.LogsType.Info);
                                return DiscardEventState.Ratelimit;
                            }
                        }
                        break;
                }

                if (PatchExtensions.AntiWorldTrigger && PhotonSender.ActorID() != Utils.VRCNetworkingClient.GetSelf().ActorID())
                {
                    if (evtLogEntry.field_Public_VrcBroadcastType_0 == VRC_EventHandler.VrcBroadcastType.Always || evtLogEntry.field_Public_VrcBroadcastType_0 == VRC_EventHandler.VrcBroadcastType.AlwaysUnbuffered || evtLogEntry.field_Public_VrcBroadcastType_0 == VRC_EventHandler.VrcBroadcastType.AlwaysBufferOne)
                    {
                        if (evtLogEntry.prop_VrcEvent_0.EventType != VRC_EventHandler.VrcEventType.SendRPC)
                        {
                            VRConsole.Log(VRConsole.LogsType.Protection, $"{VRCPlayer.DisplayName()} --> WorldTrigger");
                            Extensions.Logger.Log($"Prevented {VRCPlayer.DisplayName()} from using Worldtrigger", Extensions.Logger.LogsType.Protection);
                            return DiscardEventState.Once;
                        }
                    }
                }
            }
            catch 
            {
                Extensions.Logger.Log($"Exception RPC from {PhotonSender.GetDisplayName()} [{PhotonSender.ActorID()}]", Extensions.Logger.LogsType.Protection);
                return DiscardEventState.Ratelimit;
            }

            return DiscardEventState.None;
        }
    }
}