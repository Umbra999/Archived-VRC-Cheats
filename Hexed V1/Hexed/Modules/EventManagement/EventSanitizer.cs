using Hexed.Core;
using Hexed.Modules.Standalone;
using Hexed.Wrappers;
using VRC;
using VRC.SDKBase;

namespace Hexed.Modules.EventManagement
{
    internal class EventSanitizer
    {
        private static readonly Dictionary<int, List<int>> EventBlocks = new();
        public static List<string> BlacklistedBytes = new();

        public static bool IsActorEventBlocked(int Actor, int Code)
        {
            if (!EventBlocks.ContainsKey(Actor)) EventBlocks[Actor] = new List<int>();
            if (EventBlocks[Actor].Contains(Code)) return true;
            return false;
        }

        public static void RemoveActorBlocks(int Actor)
        {
            if (EventBlocks.ContainsKey(Actor)) EventBlocks.Remove(Actor);
        }

        public static void ClearEventBlocks()
        {
            EventBlocks.Clear();
            BlacklistedBytes.Clear();
        }

        public static void LimitActor(int Actor, byte EventCode, string Reason, int Seconds = 20)
        {
            if (IsActorEventBlocked(Actor, EventCode)) return;

            EventBlocks[Actor].Add(EventCode);
            GeneralUtils.DelayAction(Seconds, delegate
            {
                if (EventBlocks.ContainsKey(Actor) && EventBlocks[Actor].Contains(EventCode)) EventBlocks[Actor].Remove(EventCode);
            }).Start();

            Photon.Realtime.Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer(Actor);
            if (PhotonPlayer != null)
            {
                string DisplayName = PhotonPlayer.DisplayName() ?? "NO NAME";
                Logger.Log($"Limiting {DisplayName} Event {EventCode} for {Seconds} Seconds [{Reason}]", Logger.LogsType.Protection);
                VRConsole.Log($"{DisplayName} --> Invalid Event [{EventCode}]", VRConsole.LogsType.Protection);
            }
        }


        public static bool IsValidServertime(int Time, int difference = 60000)
        {
            if (Time < GameHelper.VRCNetworkingClient.prop_LoadBalancingPeer_0.ServerTimeInMilliSeconds - difference || Time > GameHelper.VRCNetworkingClient.prop_LoadBalancingPeer_0.ServerTimeInMilliSeconds + difference) return false;
            return true;
        }


        public static bool SanitizeEvent1(int Actor, byte[] Data, byte Code)
        {
            try 
            {
                if (BitConverter.ToInt32(Data, 0) != Actor)
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                if (!IsValidServertime(BitConverter.ToInt32(Data, 4)))
                {
                    LimitActor(Actor, Code, "Invalid Time");
                    return false;
                }

                Photon.Realtime.Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer(Actor);
                if (PhotonPlayer == null)
                {
                    LimitActor(Actor, Code, "Invalid PhotonPlayer");
                    return false;
                }

                Player p = PhotonPlayer.GetPlayer();
                if (p == null)
                {
                    LimitActor(Actor, Code, "Invalid VRCPlayer");
                    return false;
                }

                if (p._USpeaker.field_Private_Single_22 < 0.05f)
                {
                    p._USpeaker.field_Private_Single_22 = 0.5f; // normalize to fix uspeak being broke after, ghetto way needs to be fixed
                    LimitActor(Actor, Code, "Invalid Scale");
                    return false;
                }

                string Base64 = Convert.ToBase64String(Data.Skip(8).ToArray());
                if (BlacklistedBytes.Contains(Base64))
                {
                    LimitActor(Actor, Code, "Blacklisted");
                    return false;
                }
                BlacklistedBytes.Add(Base64);

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent4(int Actor, byte[][] Data, byte Code)
        {
            try
            {
                foreach (byte[] Array in Data)
                {
                    Il2CppSystem.Object obj;
                    try
                    {
                        BinarySerializer.Method_Public_Static_Boolean_Il2CppStructArray_1_Byte_byref_Object_0(Array, out obj);
                        if (obj == null)
                        {
                            LimitActor(Actor, Code, "Invalid EventLog");
                            return false;
                        }
                    }
                    catch
                    {
                        LimitActor(Actor, Code, "Invalid EventLog");
                        return false;
                    }

                    VRC_EventLog.ObjectNPublicInVrInStSiInObSiByVrUnique evtLogEntry = obj.TryCast<VRC_EventLog.ObjectNPublicInVrInStSiInObSiByVrUnique>();
                    if (evtLogEntry == null)
                    {
                        LimitActor(Actor, Code, "Invalid RPC Entry");
                        return false;
                    }

                    if (evtLogEntry.prop_VrcEvent_0 == null)
                    {
                        LimitActor(Actor, Code, "Invalid VRCEvent");
                        return false;
                    }

                    if (evtLogEntry.prop_VrcEvent_0.EventType > VRC_EventHandler.VrcEventType.CallUdonMethod || evtLogEntry.prop_VrcEvent_0.EventType < VRC_EventHandler.VrcEventType.MeshVisibility)
                    {
                        LimitActor(Actor, Code, "Invalid Type");
                        return false;
                    }

                    if (evtLogEntry.prop_String_0 == null)
                    {
                        LimitActor(Actor, Code, "Invalid Path");
                        return false;
                    }

                    if (!evtLogEntry.prop_String_0.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == ':' || c == '/' || c is ' ' or '(' || c == ')' || c == '-' || c == '_'))
                    {
                        LimitActor(Actor, Code, "Invalid Path");
                        return false;
                    }

                    //UnityEngine.GameObject DecodedObject;
                    //try
                    //{
                    //    DecodedObject = Network.Method_Public_Static_GameObject_String_Boolean_0(evtLogEntry.prop_String_0);
                    //}
                    //catch
                    //{
                    //    LimitActor(Actor, Code, "Invalid GameObject");
                    //    return false;
                    //}

                    //if (DecodedObject != null && (UnityUtils.IsBadPosition(DecodedObject.transform.position) || UnityUtils.IsBadDistance(GameHelper.CurrentPlayer.transform.position, DecodedObject.transform.position) || UnityUtils.IsBadRotation(DecodedObject.transform.rotation)))
                    //{
                    //    LimitActor(Actor, Code, "Invalid Position");
                    //    return false;
                    //}

                    try
                    {
                        if (VRC.Core.ParameterSerialization.Method_Public_Static_Il2CppReferenceArray_1_Object_Il2CppStructArray_1_Byte_0(evtLogEntry.prop_VrcEvent_0.ParameterBytes) == null)
                        {
                            LimitActor(Actor, Code, "Invalid Parameters");
                            return false;
                        }
                    }
                    catch
                    {
                        LimitActor(Actor, Code, "Invalid Parameters");
                        return false;
                    }
                }

                return true;
            }
            catch 
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent6(int Actor, byte[] Data, byte Code)
        {
            try
            {
                if (!IsValidServertime(BitConverter.ToInt32(Data, 1)))
                {
                    LimitActor(Actor, Code, "Invalid Time");
                    return false;
                }

                if (BitConverter.ToInt32(Data, 5) != Actor) // on object instantiation its something like actor + 0000 whatever?
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                Il2CppSystem.Object obj;
                try
                {
                    BinarySerializer.Method_Public_Static_Boolean_Il2CppStructArray_1_Byte_byref_Object_0(Data, out obj);
                    if (obj == null)
                    {
                        LimitActor(Actor, Code, "Invalid EventLog");
                        return false;
                    }
                }
                catch
                {
                    LimitActor(Actor, Code, "Invalid EventLog");
                    return false;
                }

                VRC_EventLog.ObjectNPublicInVrInStSiInObSiByVrUnique evtLogEntry = obj.TryCast<VRC_EventLog.ObjectNPublicInVrInStSiInObSiByVrUnique>();
                if (evtLogEntry == null)
                {
                    LimitActor(Actor, Code, "Invalid RPC Entry");
                    return false;
                }

                if (evtLogEntry.prop_VrcEvent_0 == null)
                {
                    LimitActor(Actor, Code, "Invalid VRCEvent");
                    return false;
                }

                if (evtLogEntry.prop_VrcEvent_0.EventType > VRC_EventHandler.VrcEventType.CallUdonMethod || evtLogEntry.prop_VrcEvent_0.EventType < VRC_EventHandler.VrcEventType.MeshVisibility) // add check if its sendrpc cuz things can be different there? atleast requi made this in network sanity lol
                {
                    LimitActor(Actor, Code, "Invalid Type");
                    return false;
                }

                if (evtLogEntry.prop_String_0 == null)
                {
                    LimitActor(Actor, Code, "Invalid Path");
                    return false;
                }

                if (!evtLogEntry.prop_String_0.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == ':' || c == '/' || c is ' ' or '(' || c == ')' || c == '-' || c == '_'))
                {
                    LimitActor(Actor, Code, "Invalid Path");
                    return false;
                }

                //UnityEngine.GameObject DecodedObject;
                //try
                //{
                //    DecodedObject = Network.Method_Public_Static_GameObject_String_Boolean_0(evtLogEntry.prop_String_0);
                //}
                //catch
                //{
                //    LimitActor(Actor, Code, "Invalid GameObject");
                //    return false;
                //}

                //if (DecodedObject != null && (UnityUtils.IsBadPosition(DecodedObject.transform.position) || UnityUtils.IsBadDistance(GameHelper.CurrentPlayer.transform.position, DecodedObject.transform.position) || UnityUtils.IsBadRotation(DecodedObject.transform.rotation)))
                //{
                //    LimitActor(Actor, Code, "Invalid Position");
                //    return false;
                //}

                try
                {
                    if (VRC.Core.ParameterSerialization.Method_Public_Static_Il2CppReferenceArray_1_Object_Il2CppStructArray_1_Byte_0(evtLogEntry.prop_VrcEvent_0.ParameterBytes) == null)
                    {
                        LimitActor(Actor, Code, "Invalid Parameters");
                        return false;
                    }
                }
                catch
                {
                    LimitActor(Actor, Code, "Invalid Parameters");
                    return false;
                }

                string Base64 = Convert.ToBase64String(Data);
                if (BlacklistedBytes.Contains(Base64))
                {
                    if (evtLogEntry.prop_VrcEvent_0.ParameterString != "UdonSyncRunProgramAsRPC") LimitActor(Actor, Code, "Blacklisted"); // fix udon one day
                    return false;
                }
                BlacklistedBytes.Add(Base64);

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent7(int Actor, byte[] Data, byte Code)
        {
            try
            {
                int viewID = BitConverter.ToInt32(Data, 0);
                if (viewID.ToString().EndsWith("00001")) return SanitizeEvent12(Actor, Data, Code);
                else return SanitizeEvent17(Actor, Data, Code); // idfk if this is right 
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent10(int Actor, byte[] Data, byte Code)
        {
            try
            {
                if (BitConverter.ToInt32(Data, 0) < 0)
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                if (!IsValidServertime(BitConverter.ToInt32(Data, 4)))
                {
                    LimitActor(Actor, Code, "Invalid Time");
                    return false;
                }

                string Base64 = Convert.ToBase64String(Data);
                if (BlacklistedBytes.Contains(Base64))
                {
                    LimitActor(Actor, Code, "Blacklisted");
                    return false;
                }
                BlacklistedBytes.Add(Base64);

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent11(int Actor, byte[] Data, byte Code)
        {
            try
            {
                int EventActor = BitConverter.ToInt32(Data, 0); // ive only seen this at 1000 yet, might be the type what it syncs?
                if (EventActor < 0)
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                if (!IsValidServertime(BitConverter.ToInt32(Data, 4)))
                {
                    LimitActor(Actor, Code, "Invalid Time");
                    return false;
                }

                int FoundIndex = GeneralUtils.lastIndexOfByteArray(Data, new byte[] { 4, 0, 0, 0 });
                if (FoundIndex != -1)
                {
                    int Offset = FoundIndex + 4;
                    int typeFromIndex = BitConverter.ToInt32(Data, Offset);

                    if (typeFromIndex > 1500 || typeFromIndex < 0)
                    {
                        LimitActor(Actor, Code, "Invalid Type");
                        return false;
                    }
                }

                string Base64 = Convert.ToBase64String(Data);
                if (BlacklistedBytes.Contains(Base64))
                {
                    LimitActor(Actor, Code, "Blacklisted");
                    return false;
                }
                BlacklistedBytes.Add(Base64);

                return true;
            }
            catch 
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent12(int Actor, byte[] Data, byte Code)
        {
            try
            {
                int viewID = BitConverter.ToInt32(Data, 0);
                if (viewID != int.Parse(Actor + "00001"))
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                if (!IsValidServertime(BitConverter.ToInt32(Data, 4)))
                {
                    LimitActor(Actor, Code, "Invalid Time");
                    return false;
                }

                int PoseRecorderOffset = 17 + Data[17];
                int PlayerNetOffset = 18 + Data[18];
                int PlayerSyncOffset = 19 + Data[19];

                if (Data.Length < PoseRecorderOffset || Data.Length < PlayerNetOffset || Data.Length < PlayerSyncOffset)
                {
                    LimitActor(Actor, Code, "Invalid Offsets");
                    return false;
                }

                int syncOffset = PlayerSyncOffset + 1;
                float playerX = BitConverter.ToSingle(Data, syncOffset);
                float playerY = BitConverter.ToSingle(Data, syncOffset + 4);
                float playerZ = BitConverter.ToSingle(Data, syncOffset + 8);
                UnityEngine.Vector3 playerPos = new(playerX, playerY, playerZ);
                if (UnityUtils.IsBadPosition(playerPos) || UnityUtils.IsBadDistance(GameHelper.CurrentPlayer.transform.position, playerPos))
                {
                    LimitActor(Actor, Code, "Invalid Player Position");
                    return false;
                }
                UnityEngine.Quaternion playerRot = new();
                VRC.Core.QuantizedSerialization.Method_Private_Static_Void_Il2CppStructArray_1_Byte_byref_Quaternion_PDM_2(Data.Skip(syncOffset + 12).Take(5).ToArray(), ref playerRot);
                if (UnityUtils.IsBadRotation(playerRot))
                {
                    LimitActor(Actor, Code, "Invalid Player Rotation");
                    return false;
                }

                //byte Quality = Data[PlayerNetOffset + 4];
                //byte[] QualityActor = new byte[5];
                //Buffer.BlockCopy(new byte[] { Quality }, 0, QualityActor, 0, 1);
                //Buffer.BlockCopy(BitConverter.GetBytes(viewID), 0, QualityActor, 1, 4);
                //string QualityBase64 = Convert.ToBase64String(QualityActor);
                //if (BlacklistedBytes.Contains(QualityBase64))
                //{
                //    LimitActor(Actor, Code, "Blacklisted");
                //    return false;
                //}
                //BlacklistedBytes.Add(QualityBase64);

                string Base64 = Convert.ToBase64String(Data.Skip(8).ToArray());
                if (BlacklistedBytes.Contains(Base64))
                {
                    LimitActor(Actor, Code, "Blacklisted");
                    return false;
                }
                BlacklistedBytes.Add(Base64);

                return true;
            }
            catch 
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent13(int Actor, byte[] Data, byte Code)
        {
            try
            {
                if (BitConverter.ToInt32(Data, 0) != int.Parse(Actor + "00003"))
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                if (!IsValidServertime(BitConverter.ToInt32(Data, 4)))
                {
                    LimitActor(Actor, Code, "Invalid Time");
                    return false;
                }

                string Base64 = Convert.ToBase64String(Data);
                if (BlacklistedBytes.Contains(Base64))
                {
                    LimitActor(Actor, Code, "Blacklisted");
                    return false;
                }
                BlacklistedBytes.Add(Base64);

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent16(int Actor, byte[] Data, byte Code)
        {
            try
            {
                if (BitConverter.ToInt32(Data, 0) < 0)
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                if (!IsValidServertime(BitConverter.ToInt32(Data, 4)))
                {
                    LimitActor(Actor, Code, "Invalid Time");
                    return false;
                }

                int posOffset = Data[16] + 16 + 1;
                float x = BitConverter.ToSingle(Data, posOffset);
                float y = BitConverter.ToSingle(Data, posOffset + 4);
                float z = BitConverter.ToSingle(Data, posOffset + 8);
                UnityEngine.Vector3 Position = new(x, y, z);
                if (UnityUtils.IsBadPosition(Position) || UnityUtils.IsBadDistance(GameHelper.CurrentPlayer.transform.position, Position))
                {
                    LimitActor(Actor, Code, "Invalid Position");
                    return false;
                }

                UnityEngine.Quaternion Rotation = new();
                VRC.Core.QuantizedSerialization.Method_Private_Static_Void_Il2CppStructArray_1_Byte_byref_Quaternion_PDM_2(Data.Skip(posOffset + 12).Take(5).ToArray(), ref Rotation);
                if (UnityUtils.IsBadRotation(Rotation))
                {
                    LimitActor(Actor, Code, "Invalid Rotation");
                    return false;
                }

                string Base64 = Convert.ToBase64String(Data);
                if (BlacklistedBytes.Contains(Base64))
                {
                    LimitActor(Actor, Code, "Blacklisted");
                    return false;
                }
                BlacklistedBytes.Add(Base64);

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent17(int Actor, byte[] Data, byte Code)
        {
            try
            {
                if (BitConverter.ToInt32(Data, 0) < 0)
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                if (!IsValidServertime(BitConverter.ToInt32(Data, 4)))
                {
                    LimitActor(Actor, Code, "Invalid Time");
                    return false;
                }

                string Base64 = Convert.ToBase64String(Data);
                if (BlacklistedBytes.Contains(Base64))
                {
                    LimitActor(Actor, Code, "Blacklisted");
                    return false;
                }
                BlacklistedBytes.Add(Base64);

                return true;
            }
            catch 
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent202(int Actor, Il2CppSystem.Collections.Hashtable Data, byte Code) 
        {
            try
            {
                if (Data.ContainsKey("6") && !IsValidServertime(Data["6"].Unbox<int>()))
                {
                    LimitActor(Actor, Code, "Invalid Time");
                    return false;
                }

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent21(int Actor, int[] ViewIDs, byte Code)
        {
            try
            {
                if (ViewIDs.Length != 2)
                {
                    LimitActor(Actor, Code, "Invalid Lenght");
                    return false;
                }

                if (ViewIDs[0] < 0)
                {
                    LimitActor(Actor, Code, "Invalid ViewID");
                    return false;
                }

                if (ViewIDs[1] < 0) // maybe add check if actor is existing but i remember it can set to 0 or maybe it was a viewid idk tbh
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent22(int Actor, int[] ViewIDs, byte Code)
        {
            try
            {
                if (ViewIDs.Length != 2)
                {
                    LimitActor(Actor, Code, "Invalid Lenght");
                    return false;
                }

                if (ViewIDs[0] < 0)
                {
                    LimitActor(Actor, Code, "Invalid ViewID");
                    return false;
                }

                if (ViewIDs[1] < 0) // maybe add check if actor is existing but i remember it can set to 0 or maybe it was a viewid idk tbh
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent210(int Actor, int[] ViewIDs, byte Code)
        {
            try
            {
                if (ViewIDs.Length != 2)
                {
                    LimitActor(Actor, Code, "Invalid Lenght");
                    return false;
                }

                if (ViewIDs[0] < 0)
                {
                    LimitActor(Actor, Code, "Invalid ViewID");
                    return false;
                }

                if (ViewIDs[1] < 0) // maybe add check if actor is existing but i remember it can set to 0 or maybe it was a viewid idk tbh
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool CheckDecodedRPC(Player Player, VRC_EventHandler.VrcEvent Event, VRC_EventHandler.VrcBroadcastType BroadcastType, int instagatorId, float fastForward, object[] DecodedBytes)
        {
            try
            {
                switch (Event.EventType)
                {
                    // TODO: Add other event types to log and check whats even used

                    case VRC_EventHandler.VrcEventType.AddDamage:
                        {
                            if (InternalSettings.GodMode && Player.GetPhotonPlayer().ActorID() == GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID()) return false;
                        }
                        break;

                    case VRC_EventHandler.VrcEventType.TeleportPlayer:
                        {
                            if (InternalSettings.NoTeleport && Player.GetPhotonPlayer().ActorID() == GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID()) return false;

                            Logger.Log($"{Player.DisplayName()} used Teleport", Logger.LogsType.Room);
                            VRConsole.Log($"{Player.DisplayName()} --> Teleport", VRConsole.LogsType.Room);
                        }
                        break;

                    case VRC_EventHandler.VrcEventType.SendRPC:
                        {
                            switch (Event.ParameterString)
                            {
                                case "_DestroyObject":
                                    {
                                        if (InternalSettings.NoObjectDestroy) return false;

                                        Logger.Log($"{Player.DisplayName()} destroyed an Object [ID: {DecodedBytes[0]}]", Logger.LogsType.Room);
                                        VRConsole.Log($"{Player.DisplayName()} --> Object Destroy [ID: {DecodedBytes[0]}]", VRConsole.LogsType.Room);
                                    }
                                    break;

                                case "InternalApplyOverrideRPC": // i think thats unused
                                    {
                                        if (InternalSettings.AntiOverride && Player.GetPhotonPlayer().ActorID() == GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID()) return false;
                                    }
                                    break;

                                case "PlayEmoteRPC":
                                    {
                                        int Emote = (int)DecodedBytes[0];
                                        if (Player.GetPhotonPlayer().ActorID() != GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID() && (Emote < 1 || Emote > 8))
                                        {
                                            Logger.Log($"{Player.DisplayName()} used Invalid Emote [{Emote}]", Logger.LogsType.Protection);
                                            VRConsole.Log($"{Player.DisplayName()} --> Invalid Emote [{Emote}]", VRConsole.LogsType.Protection);

                                            LimitActor(Player.GetPhotonPlayer().ActorID(), 6, "Invalid RPC");
                                            return false;
                                        }

                                        Logger.Log($"{Player.DisplayName()} used a Emote [{Emote}]", Logger.LogsType.Room);
                                        VRConsole.Log($"{Player.DisplayName()} --> {Emote}", VRConsole.LogsType.Emote);
                                    }
                                    break;

                                case "CancelRPC":
                                    {
                                        Logger.Log($"{Player.DisplayName()} canceled Emote", Logger.LogsType.Room);
                                        VRConsole.Log($"{Player.DisplayName()} --> Cancel Emote", VRConsole.LogsType.Emote);
                                    }
                                    break;

                                case "ReloadAvatarNetworkedRPC":
                                    {
                                        Logger.Log($"{Player.DisplayName()} used Avatar Reload", Logger.LogsType.Room);
                                        VRConsole.Log($"{Player.DisplayName()} --> Reload Avatar", VRConsole.LogsType.Avatar);
                                    }
                                    break;

                                case "SwitchAvatar":
                                    {
                                        if (Player.GetPhotonPlayer().ActorID() != GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID())
                                        {
                                            Logger.Log($"Prevented {Player.DisplayName()} from switching Pedestal [{DecodedBytes[0]}]", Logger.LogsType.Protection);
                                            VRConsole.Log($"{Player.DisplayName()} --> Pedestal Switch", VRConsole.LogsType.Protection);

                                            LimitActor(Player.GetPhotonPlayer().ActorID(), 6, "Invalid RPC");
                                            return false;
                                        }

                                        Logger.Log($"{Player.DisplayName()} switched a Pedestal", Logger.LogsType.Room);
                                        VRConsole.Log($"{Player.DisplayName()} --> Avatar Switch", VRConsole.LogsType.Pedestal);
                                    }
                                    break;

                                case "RefreshAvatar":
                                    {
                                        Logger.Log($"{Player.DisplayName()} refreshed a Pedestal", Logger.LogsType.Room);
                                        VRConsole.Log($"{Player.DisplayName()} --> Avatar Refresh", VRConsole.LogsType.Pedestal);
                                    }
                                    break;

                                case "SetAvatarUse":
                                    {
                                        if (Player.GetPhotonPlayer().ActorID() != GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID())
                                        {
                                            Logger.Log($"Prevented {Player.DisplayName()} from using a Pedestal", Logger.LogsType.Protection);
                                            VRConsole.Log($"{Player.DisplayName()} --> Avatar Use", VRConsole.LogsType.Protection);

                                            LimitActor(Player.GetPhotonPlayer().ActorID(), 6, "Invalid RPC");
                                            return false;
                                        }

                                        Logger.Log($"{Player.DisplayName()} used a Pedestal", Logger.LogsType.Room);
                                        VRConsole.Log($"{Player.DisplayName()} --> Avatar Use", VRConsole.LogsType.Pedestal);
                                    }
                                    break;

                                case "_InstantiateObject":
                                    {
                                        if (Player.GetPhotonPlayer().ActorID() != GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID())
                                        {
                                            string RawPosition = DecodedBytes[1].ToString().TrimStart('(').TrimEnd(')');
                                            string[] PositionFloats = RawPosition.Split(',');
                                            UnityEngine.Vector3 Position = new(float.Parse(PositionFloats[0]), float.Parse(PositionFloats[1]), float.Parse(PositionFloats[2]));
                                            if (UnityUtils.IsBadPosition(Position) || UnityUtils.IsBadDistance(GameHelper.CurrentPlayer.transform.position, Position))
                                            {
                                                Logger.Log($"Prevented {Player.DisplayName()} from instantiating a invalid Object", Logger.LogsType.Protection);
                                                VRConsole.Log($"{Player.DisplayName()} --> Invalid Object", VRConsole.LogsType.Protection);

                                                LimitActor(Player.GetPhotonPlayer().ActorID(), 6, "Invalid Position");
                                                return false;
                                            }

                                            string RawRotation = DecodedBytes[2].ToString().TrimStart('(').TrimEnd(')');
                                            string[] RotationFloats = RawRotation.Split(',');
                                            UnityEngine.Quaternion Rotation = new(float.Parse(RotationFloats[0]), float.Parse(RotationFloats[1]), float.Parse(RotationFloats[2]), float.Parse(RotationFloats[3]));
                                            if (UnityUtils.IsBadRotation(Rotation))
                                            {
                                                Logger.Log($"Prevented {Player.DisplayName()} from instantiating a invalid Object", Logger.LogsType.Protection);
                                                VRConsole.Log($"{Player.DisplayName()} --> Invalid Object", VRConsole.LogsType.Protection);

                                                LimitActor(Player.GetPhotonPlayer().ActorID(), 6, "Invalid Rotation");
                                                return false;
                                            }

                                            string Actor = DecodedBytes[3].ToString();
                                            if (!Actor.EndsWith("00004"))
                                            {
                                                Logger.Log($"Prevented {Player.DisplayName()} from instantiating a invalid Object", Logger.LogsType.Protection);
                                                VRConsole.Log($"{Player.DisplayName()} --> Invalid Object", VRConsole.LogsType.Protection);

                                                LimitActor(Player.GetPhotonPlayer().ActorID(), 6, "Invalid Actor");
                                                return false;
                                            }
                                        }

                                        Logger.Log($"{Player.DisplayName()} instantiated Object {DecodedBytes[0]} [ID: {DecodedBytes[3]}]", Logger.LogsType.Room);
                                        VRConsole.Log($"{Player.DisplayName()} --> Object Instantiate [ID: {DecodedBytes[3]}]", VRConsole.LogsType.Room);
                                    }
                                    break;

                                case "_SendOnSpawn":
                                    {
                                        string Actor = DecodedBytes[0].ToString();
                                        if (!Actor.EndsWith("00004"))
                                        {
                                            Logger.Log($"Prevented {Player.DisplayName()} from instantiating a invalid Object", Logger.LogsType.Protection);
                                            VRConsole.Log($"{Player.DisplayName()} --> Invalid Object", VRConsole.LogsType.Protection);

                                            LimitActor(Player.GetPhotonPlayer().ActorID(), 6, "Invalid Actor");
                                            return false;
                                        }
                                    }
                                    break;

                                case "ChangeVisibility":
                                    {
                                        if ((bool)DecodedBytes[0] == true)
                                        {
                                            if (Player.GetPhotonPlayer().ActorID() == GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID()) return false;

                                            Logger.Log($"{Player.DisplayName()} showed the Camera", Logger.LogsType.Room);
                                            VRConsole.Log($"{Player.DisplayName()} --> Show", VRConsole.LogsType.Camera);
                                        }
                                        else
                                        {
                                            Logger.Log($"{Player.DisplayName()} hide the Camera", Logger.LogsType.Room);
                                            VRConsole.Log($"{Player.DisplayName()} --> Hide", VRConsole.LogsType.Camera);
                                        }
                                    }
                                    break;

                                case "PhotoCapture":
                                    {
                                        if (Player.GetPhotonPlayer().ActorID() != GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID())
                                        {
                                            if (!Player.GetVRCPlayer().field_Public_UserCameraIndicator_0.field_Private_Boolean_3)
                                            {
                                                Logger.Log($"Prevented {Player.DisplayName()} from using invisible Camera", Logger.LogsType.Protection);
                                                VRConsole.Log($"{Player.DisplayName()} --> Invisible Camera", VRConsole.LogsType.Protection);

                                                LimitActor(Player.GetPhotonPlayer().ActorID(), 6, "Invisible Camera");
                                                return false;
                                            }

                                            if (InternalSettings.NoCameraSound)
                                            {
                                                Logger.Log($"{Player.DisplayName()} took a Photo", Logger.LogsType.Room);
                                                VRConsole.Log($"{Player.DisplayName()} --> Photo", VRConsole.LogsType.Camera);
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            Logger.Log($"{Player.DisplayName()} took a Photo", Logger.LogsType.Room);
                                            VRConsole.Log($"{Player.DisplayName()} --> Photo", VRConsole.LogsType.Camera);
                                        }
                                    }
                                    break;

                                case "TimerBloop":
                                    {
                                        if (Player.GetPhotonPlayer().ActorID() != GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID())
                                        {
                                            if (!Player.GetVRCPlayer().field_Public_UserCameraIndicator_0.field_Private_Boolean_3)
                                            {
                                                Logger.Log($"Prevented {Player.DisplayName()} from using invisible Camera", Logger.LogsType.Protection);
                                                VRConsole.Log($"{Player.DisplayName()} --> Invisible Camera", VRConsole.LogsType.Protection);

                                                LimitActor(Player.GetPhotonPlayer().ActorID(), 6, "Invisible Camera");
                                                return false;
                                            }

                                            if (InternalSettings.NoCameraSound)
                                            {
                                                Logger.Log($"{Player.DisplayName()} used a Timer", Logger.LogsType.Room);
                                                VRConsole.Log($"{Player.DisplayName()} --> Timer", VRConsole.LogsType.Camera);
                                                return false;
                                            }
                                        }
                                    }
                                    break;

                                case "UdonSyncRunProgramAsRPC":
                                    {
                                        if (InternalSettings.NoUdonEvents && Player.GetPhotonPlayer().ActorID() != GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID()) return false;
                                    }
                                    break;

                                case "EnableMeshRPC": // dev prop
                                    {
                                        if (Player.GetPhotonPlayer().ActorID() != GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID())
                                        {
                                            Logger.Log($"Prevented {Player.DisplayName()} from using an Admin RPC [{Event.ParameterString}]", Logger.LogsType.Protection);
                                            VRConsole.Log($"{Player.DisplayName()} --> Admin RPC [{Event.ParameterString}]", VRConsole.LogsType.Protection);

                                            LimitActor(Player.GetPhotonPlayer().ActorID(), 6, "Admin RPC");
                                            return false;
                                        }
                                    }
                                    break;

                                case "SetPresetRPC": // dev prop
                                    {
                                        if (Player.GetPhotonPlayer().ActorID() != GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID())
                                        {
                                            Logger.Log($"Prevented {Player.DisplayName()} from using an Admin RPC [{Event.ParameterString}]", Logger.LogsType.Protection);
                                            VRConsole.Log($"{Player.DisplayName()} --> Admin RPC [{Event.ParameterString}]", VRConsole.LogsType.Protection);

                                            LimitActor(Player.GetPhotonPlayer().ActorID(), 6, "Admin RPC");
                                            return false;
                                        }
                                    }
                                    break;

                                case "ReleaseBones":
                                    {
                                        Logger.Log($"{Player.DisplayName()} released Physbone positions", Logger.LogsType.Room);
                                        VRConsole.Log($"{Player.DisplayName()} --> Physbone Release", VRConsole.LogsType.Avatar);
                                    }
                                    break;

                                case "SanityCheck":
                                    {
                                        if (Player.GetPhotonPlayer().ActorID() != GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID())
                                        {
                                            int Actor = (int)DecodedBytes[2];
                                            if (GameHelper.VRCNetworkingClient.GetAllPhotonPlayers().FirstOrDefault(x => x.ActorID() == Actor) == null)
                                            {
                                                Logger.Log($"Prevented {Player.DisplayName()} from checking a Invalid Actor", Logger.LogsType.Protection);
                                                VRConsole.Log($"{Player.DisplayName()} --> Invalid Actor", VRConsole.LogsType.Protection);

                                                LimitActor(Player.GetPhotonPlayer().ActorID(), 6, "Invalid Actor");
                                                return false;
                                            }

                                            if (Actor == Player.GetPhotonPlayer().ActorID())
                                            {
                                                Logger.Log($"Prevented {Player.DisplayName()} from checking your Sanity", Logger.LogsType.Protection);
                                                VRConsole.Log($"{Player.DisplayName()} --> Sanity Check", VRConsole.LogsType.Protection);
                                                return false;
                                            }
                                        }
                                    }
                                    break;

                                case "DisableKinematic":
                                    break;

                                case "EnableGravity":
                                    break;

                                default:
                                    Logger.LogWarning($"Not implemented RPC, blocked for Protection");
                                    Logger.LogRPC(Player, Event, BroadcastType, instagatorId, fastForward, DecodedBytes);
                                    return false;
                            }
                            break;
                        }
                }
            }
            catch
            {
                LimitActor(Player.GetPhotonPlayer().ActorID(), 6, "RPC Exception");
                return false;
            }

            return true;
        }

        public static bool CheckWorldID(string ID)
        {
            if (!ID.Contains(':')) return false;
            string WorldID = ID.Split(':')[0];
            string InstanceID = ID.Split(':')[1];

            if (WorldID.Length != 41 || InstanceID.Length < 1) return false;
            if (!WorldID.StartsWith("wrld_")) return false;
            if (!WorldID.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '-')) return false;
            if (!InstanceID.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '-' || c == '(' || c == ')' || c == '~' || c == '&')) return false;

            return true;
        }

        public static bool CheckGroupID(string ID)
        {
            if (ID.Length != 40) return false;
            if (!ID.StartsWith("grp_")) return false;
            if (!ID.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '-')) return false;

            return true;
        }

        public static bool CheckFileID(string ID)
        {
            if (ID.Length != 41) return false;
            if (!ID.StartsWith("file_")) return false;
            if (!ID.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '-')) return false;

            return true;
        }
    }
}
