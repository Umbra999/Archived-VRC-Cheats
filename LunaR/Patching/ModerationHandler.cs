using LunaR.ConsoleUtils;
using LunaR.Extensions;
using LunaR.Modules;
using LunaR.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRC.Core;

namespace LunaR.Patching
{
    internal class ModerationHandler
    {
        public static List<int> BlockList = new();
        public static List<int> MuteList = new();
        public static bool SeeBlocked = false;
        public static BlockMode AntiBlock;

        public enum BlockMode
        {
            None = 0,
            All = 1,
            NoFriends = 2
        }

        public static void ClearModerations()
        {
            MuteList.Clear();
            BlockList.Clear();
        }

        public static bool CheckEvent33(Dictionary<byte, object> Event33Data)
        {

            switch (int.Parse(Event33Data[0].ToString()))
            {
                case 2: // Show Message Alert
                    switch (Event33Data[0].ToString())
                    {
                        case "You have been warned for your behavior. If you continue, you may be kicked out of the instance":
                            if (RoomManager.field_Internal_Static_ApiWorldInstance_0?.ownerId == null)
                            {
                                Extensions.Logger.Log($"Server tried to Warn you", Extensions.Logger.LogsType.Moderation);
                                VRConsole.Log(VRConsole.LogsType.Moderation, $"Server --> Warn");
                            }
                            else
                            {
                                Player user = Utils.PlayerManager.GetPlayer(RoomManager.field_Internal_Static_ApiWorldInstance_0.ownerId);
                                Extensions.Logger.Log($"{user.DisplayName()} tried to Warn you", Extensions.Logger.LogsType.Moderation);
                                VRConsole.Log(VRConsole.LogsType.Moderation, $"{user.DisplayName()} --> Warn");
                            }
                            return false;

                        case "Unable to start a vote to kick":
                            Extensions.Logger.Log($"Failed to send Votekick", Extensions.Logger.LogsType.Moderation);
                            VRConsole.Log(VRConsole.LogsType.Moderation, "Failed to send Votekick");
                            break;

                        case "Not enough players to start a vote to kick":
                            Extensions.Logger.Log($"Failed to send Votekick", Extensions.Logger.LogsType.Moderation);
                            VRConsole.Log(VRConsole.LogsType.Moderation, "Failed to send Votekick");
                            break;
                    }
                    break;

                case 8: // RoomOwner MicOff
                    if (RoomManager.field_Internal_Static_ApiWorldInstance_0?.ownerId == null)
                    {
                        Extensions.Logger.Log($"Server tried to Force Mute You", Extensions.Logger.LogsType.Moderation);
                        VRConsole.Log(VRConsole.LogsType.Moderation, $"Server --> Force Mute");
                    }
                    else
                    {
                        Player user = Utils.PlayerManager.GetPlayer(RoomManager.field_Internal_Static_ApiWorldInstance_0.ownerId);
                        Extensions.Logger.Log($"{user.DisplayName()} tried to ForceMicOff You", Extensions.Logger.LogsType.Moderation);
                        VRConsole.Log(VRConsole.LogsType.Moderation, $"{user.DisplayName()} --> Force Mute");
                    }
                    return false;

                case 9: // Mod Mic Gain change
                    Extensions.Logger.Log($"Server tried to change your Mic Volume", Extensions.Logger.LogsType.Moderation);
                    VRConsole.Log(VRConsole.LogsType.Moderation, $"Server --> Voice Volume Change");
                    return false;

                case 10: // Mod Changeworld with delay?
                    Extensions.Logger.Log($"Server tried to move you to a new World", Extensions.Logger.LogsType.Moderation);
                    VRConsole.Log(VRConsole.LogsType.Moderation, $"Server --> Force World Change");
                    return false;

                case 11: // Mod Worldchange
                    Extensions.Logger.Log($"Server tried to move you to a new World", Extensions.Logger.LogsType.Moderation);
                    VRConsole.Log(VRConsole.LogsType.Moderation, $"Server --> Force World Change");
                    return false;

                case 12: // Mod Teleport
                    Extensions.Logger.Log($"Server tried to Teleport you", Extensions.Logger.LogsType.Moderation);
                    VRConsole.Log(VRConsole.LogsType.Moderation, $"Server --> Force Teleport");
                    return false;

                case 13: // Votekick
                    string Target = Event33Data[2].ToString().Replace("A vote kick has been initiated against ", "").Replace(", do you agree?", "");
                    Extensions.Logger.Log($"Votekick started on {Target} [ID: {Event33Data[3]}]", Extensions.Logger.LogsType.Moderation);
                    VRConsole.Log(VRConsole.LogsType.Moderation, $"Votekick --> {Target}");
                    break;

                case 20: // Init Moderations 
                    //if (BotDetection.MissingModeration.Contains((int)Event33Data[1])) BotDetection.MissingModeration.Remove((int)Event33Data[1]); 
                    //Event33Data[3] has some shit in need to reverse and move from 21 check to 20 check            
                    break;

                case 21: // Mute and Block        
                    bool? MuteCheck = CheckMute(Event33Data);
                    if (MuteCheck == true)
                    {
                        if (BotDetection.MissingModeration.Contains((int)Event33Data[1])) BotDetection.MissingModeration.Remove((int)Event33Data[1]);
                        if (!MuteList.Contains((int)Event33Data[1]))
                        {
                            MuteList.Add((int)Event33Data[1]);
                            Photon.Realtime.Player PhotonPlayer = PhotonExtensions.GetPhotonPlayer((int)Event33Data[1]);
                            if (PhotonPlayer != null)
                            {
                                Player VRCPlayer = PhotonPlayer.GetPlayer();
                                string SenderName = VRCPlayer == null ? PhotonPlayer.GetDisplayName() : VRCPlayer.DisplayName();
                                Extensions.Logger.Log($"{SenderName} muted you", Extensions.Logger.LogsType.Moderation);
                                VRConsole.Log(VRConsole.LogsType.Voice, $"{SenderName} --> Mute");
                            }
                        }
                    }

                    else if (MuteCheck == false)
                    {
                        if (BotDetection.MissingModeration.Contains((int)Event33Data[1])) BotDetection.MissingModeration.Remove((int)Event33Data[1]);
                        if (MuteList.Contains((int)Event33Data[1]))
                        {
                            MuteList.Remove((int)Event33Data[1]);
                            Photon.Realtime.Player PhotonPlayer = PhotonExtensions.GetPhotonPlayer((int)Event33Data[1]);
                            if (PhotonPlayer != null)
                            {
                                Player VRCPlayer = PhotonPlayer.GetPlayer();
                                string SenderName = VRCPlayer == null ? PhotonPlayer.GetDisplayName() : VRCPlayer.DisplayName();
                                Extensions.Logger.Log($"{SenderName} unmuted you", Extensions.Logger.LogsType.Moderation);
                                VRConsole.Log(VRConsole.LogsType.Voice, $"{SenderName} --> Unmute");
                            }
                        }
                    }

                    bool? BlockCheck = CheckBlock(Event33Data);
                    if (BlockCheck == true)
                    {
                        if (BotDetection.MissingModeration.Contains((int)Event33Data[1])) BotDetection.MissingModeration.Remove((int)Event33Data[1]);
                        if (!BlockList.Contains((int)Event33Data[1]))
                        {
                            BlockList.Add((int)Event33Data[1]);
                            Photon.Realtime.Player PhotonPlayer = PhotonExtensions.GetPhotonPlayer((int)Event33Data[1]);
                            if (PhotonPlayer != null)
                            {
                                Player VRCPlayer = PhotonPlayer.GetPlayer();
                                string SenderName = VRCPlayer == null ? PhotonPlayer.GetDisplayName() : VRCPlayer.DisplayName();
                                Extensions.Logger.Log($"{SenderName} blocked you", Extensions.Logger.LogsType.Moderation);
                                VRConsole.Log(VRConsole.LogsType.Block, $"{SenderName} --> Block");
                            }
                        }

                        switch (AntiBlock)
                        {
                            case BlockMode.All:
                                Antiblock((int)Event33Data[1]).Start();
                                return false;

                            case BlockMode.NoFriends:
                                Photon.Realtime.Player PhotonPlayer = PhotonExtensions.GetPhotonPlayer((int)Event33Data[1]);
                                if (PhotonPlayer != null)
                                {
                                    Player VRCPlayer = PhotonPlayer.GetPlayer();
                                    string UserID = VRCPlayer == null ? PhotonPlayer.UserID() : VRCPlayer.UserID();
                                    if (!PlayerExtensions.IsFriend(UserID))
                                    {
                                        Antiblock((int)Event33Data[1]).Start();
                                        return false;
                                    }
                                }
                                break;
                        }
                    }

                    else if (BlockCheck == false)
                    {
                        if (BotDetection.MissingModeration.Contains((int)Event33Data[1])) BotDetection.MissingModeration.Remove((int)Event33Data[1]);
                        if (BlockList.Contains((int)Event33Data[1]))
                        {
                            BlockList.Remove((int)Event33Data[1]);
                            Photon.Realtime.Player PhotonPlayer = PhotonExtensions.GetPhotonPlayer((int)Event33Data[1]);
                            if (PhotonPlayer != null)
                            {
                                Player VRCPlayer = PhotonPlayer.GetPlayer();
                                string SenderName = VRCPlayer == null ? PhotonPlayer.GetDisplayName() : VRCPlayer.DisplayName();
                                Extensions.Logger.Log($"{SenderName} unblocked you", Extensions.Logger.LogsType.Moderation); ;
                                VRConsole.Log(VRConsole.LogsType.Block, $"{SenderName} --> Unblock");
                            }
                        }

                        if (SeeBlocked) SeeBlock((int)Event33Data[1]).Start();
                    }

                    else
                    {
                        bool? MassMuteCheck = CheckMassMute(Event33Data);
                        if (MassMuteCheck == true)
                        {
                            foreach (int Actor in (int[])Event33Data[11])
                            {
                                if (!MuteList.Contains(Actor))
                                {
                                    MuteList.Add(Actor);
                                    Photon.Realtime.Player PhotonPlayer = PhotonExtensions.GetPhotonPlayer(Actor);

                                    if (PhotonPlayer != null)
                                    {
                                        Player VRCPlayer = PhotonPlayer.GetPlayer();
                                        string SenderName = VRCPlayer == null ? PhotonPlayer.GetDisplayName() : VRCPlayer.DisplayName();
                                        Extensions.Logger.Log($"{SenderName} muted you", Extensions.Logger.LogsType.Moderation);
                                        VRConsole.Log(VRConsole.LogsType.Voice, $"{SenderName} --> Mute");
                                    }
                                }
                            }
                        }

                        else if (MassMuteCheck == false)
                        {
                            foreach (int Actor in MuteList)
                            {
                                Photon.Realtime.Player PhotonPlayer = PhotonExtensions.GetPhotonPlayer(Actor);
                                if (PhotonPlayer != null)
                                {
                                    Player VRCPlayer = PhotonPlayer.GetPlayer();
                                    string SenderName = VRCPlayer == null ? PhotonPlayer.GetDisplayName() : VRCPlayer.DisplayName();
                                    Extensions.Logger.Log($"{SenderName} unmuted you", Extensions.Logger.LogsType.Moderation);
                                    VRConsole.Log(VRConsole.LogsType.Voice, $"{SenderName} --> Unmute");
                                }
                            }
                            MuteList.Clear();
                        }

                        bool? MassBlockCheck = CheckMassBlock(Event33Data);
                        if (MassBlockCheck == true)
                        {
                            foreach (int Actor in (int[])Event33Data[10])
                            {
                                if (!BlockList.Contains(Actor))
                                {
                                    BlockList.Add(Actor);
                                    Photon.Realtime.Player PhotonPlayer = PhotonExtensions.GetPhotonPlayer(Actor);
                                    if (PhotonPlayer != null)
                                    {
                                        Player VRCPlayer = PhotonPlayer.GetPlayer();
                                        string SenderName = VRCPlayer == null ? PhotonPlayer.GetDisplayName() : VRCPlayer.DisplayName();
                                        Extensions.Logger.Log($"{SenderName} blocked you", Extensions.Logger.LogsType.Moderation);
                                        VRConsole.Log(VRConsole.LogsType.Block, $"{SenderName} --> Block");
                                    }
                                }

                                switch (AntiBlock)
                                {
                                    case BlockMode.All:
                                        Antiblock(Actor).Start();
                                        return false;

                                    case BlockMode.NoFriends:
                                        Photon.Realtime.Player PhotonPlayer = PhotonExtensions.GetPhotonPlayer(Actor);
                                        if (PhotonPlayer != null)
                                        {
                                            Player VRCPlayer = PhotonPlayer.GetPlayer();
                                            string UserID = VRCPlayer == null ? PhotonPlayer.UserID() : VRCPlayer.UserID();
                                            if (!PlayerExtensions.IsFriend(UserID)) Antiblock(Actor).Start();
                                        }
                                        break;
                                }
                            }
                        }

                        else if (MassBlockCheck == false)
                        {
                            foreach (int Actor in BlockList)
                            {
                                Photon.Realtime.Player PhotonPlayer = PhotonExtensions.GetPhotonPlayer(Actor);
                                if (PhotonPlayer != null)
                                {
                                    Player VRCPlayer = PhotonPlayer.GetPlayer();
                                    string SenderName = VRCPlayer == null ? PhotonPlayer.GetDisplayName() : VRCPlayer.DisplayName();
                                    Extensions.Logger.Log($"{SenderName} unblocked you", Extensions.Logger.LogsType.Moderation);
                                    VRConsole.Log(VRConsole.LogsType.Block, $"{SenderName} --> Unblock");
                                }
                            }
                            BlockList.Clear();
                        }
                    }
                    break;
            }
            return true;
        }

        private static IEnumerator Antiblock(int Actor)
        {
            yield return new WaitForEndOfFrame();
            Player Instance = PlayerWrappers.GetPlayer(Actor);
            if (Instance == null)
            {
                yield return new WaitForSeconds(2);
                Instance = PlayerWrappers.GetPlayer(Actor);
            }
            if (Instance != null)
            {
                if (!Instance.prop_Boolean_15) yield return new WaitForSeconds(1);
                if (Instance != null && Instance.prop_Boolean_15)
                {
                    Instance.prop_Boolean_15 = false;
                    PlayerExtensions.ReloadAvatar(Instance.GetAPIUser());
                }
            }
        }

        private static IEnumerator SeeBlock(int Actor)
        {
            yield return new WaitForEndOfFrame();
            Player Instance = PlayerWrappers.GetPlayer(Actor);
            if (Instance == null)
            {
                yield return new WaitForSeconds(2);
                Instance = PlayerWrappers.GetPlayer(Actor);
            }
            if (Instance != null)
            {
                if (!Instance.prop_Boolean_14) yield return new WaitForSeconds(1);
                if (Instance != null && Instance.prop_Boolean_14)
                {
                    Instance.prop_Boolean_14 = false;
                    PlayerExtensions.ReloadAvatar(Instance.GetAPIUser());
                }
            }
        }


        private static bool? CheckBlock(Dictionary<byte, object> event33data)
        {
            if (event33data.ContainsKey(1) && event33data[1] is int && event33data.ContainsKey(10) && event33data[10] is bool blockbool && event33data.ContainsKey(11) && event33data[11] is bool) return blockbool;
            return null;
        }

        private static bool? CheckMute(Dictionary<byte, object> event33data)
        {
            if (event33data.ContainsKey(1) && event33data[1] is int && event33data.ContainsKey(10) && event33data[10] is bool && event33data.ContainsKey(11) && event33data[11] is bool mutebool) return mutebool;
            return null;
        }

        private static bool? CheckMassBlock(Dictionary<byte, object> event33data)
        {
            if (event33data.ContainsKey(10) && event33data[10] is int[] blockactors && event33data.ContainsKey(11) && event33data[11] is int[])
            {
                if (blockactors.Length > 0) return true;
                else return false;
            }
            return null;
        }

        private static bool? CheckMassMute(Dictionary<byte, object> event33data)
        {
            if (event33data.ContainsKey(10) && event33data[10] is int[] && event33data.ContainsKey(11) && event33data[11] is int[] muteactors)
            {
                if (muteactors.Length > 0) return true;
                else return false;
            }
            return null;
        }

        public static void SendAPIKick(APIUser User, string Message)
        {
            Utils.ModerationManager.Method_Private_Void_APIUser_String_0(User, Message);
        }

        public static void SendAPIKick(Photon.Realtime.Player User, string Message)
        {
            APIUser ApiUser = new()
            {
                displayName = User.GetDisplayName(),
                id = User.UserID()
            };
            SendAPIKick(ApiUser, Message);
        }
    }
}