using Hexed.Core;
using Hexed.Modules.Standalone;
using Hexed.Wrappers;
using VRC.Management;

namespace Hexed.Modules.EventManagement
{
    internal class ModerationHandler
    {
        public static List<int> BlockList = new();
        public static List<int> MuteList = new();

        public static void ClearModerations()
        {
            MuteList.Clear();
            BlockList.Clear();
        }

        public static bool ReceivedModerationEvent(ref Dictionary<byte, object> Event33Data)
        {
            switch (Convert.ToByte(Event33Data[0]))
            {
                case 2: // Alert
                    switch (Event33Data[2].ToString())
                    {
                        case "You have been warned for your behavior. If you continue, you may be kicked out of the instance":
                            Logger.Log($"Server tried to Warn you", Logger.LogsType.Room);
                            VRConsole.Log($"Server --> Warn", VRConsole.LogsType.Moderation);
                            return false;
                    }
                    break;

                case 8: // RoomOwner MicOff
                    Logger.Log($"Server tried to Mic Off you", Logger.LogsType.Room);
                    VRConsole.Log($"Server --> Mic Off", VRConsole.LogsType.Moderation);
                    return false;

                case 9: // Mod Mic Gain change
                    Logger.Log($"Server tried to change your Mic Volume", Logger.LogsType.Room);
                    VRConsole.Log($"Server --> Voice Volume Change", VRConsole.LogsType.Moderation);
                    return false;

                case 10: // Friend Player, still used or is that even a receive?
                    Logger.Log($"Server tried to move you to a new World", Logger.LogsType.Room);
                    VRConsole.Log($"Server --> Force World Change", VRConsole.LogsType.Moderation);
                    return false;

                case 11: // Mod Warp To Instance 
                    Logger.Log($"Server tried to move you to a new World", Logger.LogsType.Room);
                    VRConsole.Log($"Server --> Force World Change", VRConsole.LogsType.Moderation);
                    return false;

                case 12: // Mod Teleport User
                    Logger.Log($"Server tried to Teleport you", Logger.LogsType.Room);
                    VRConsole.Log($"Server --> Force Teleport", VRConsole.LogsType.Moderation);
                    return false;

                case 13: // Votekick
                    string Target = Event33Data[2].ToString().Replace("A vote kick has been initiated against ", "").Replace(", do you agree?", "");
                    Logger.Log($"Votekick started on {Target} [ID: {Event33Data[3]}]", Logger.LogsType.Room);
                    VRConsole.Log($"Votekick --> {Target}", VRConsole.LogsType.Moderation);
                    break;

                case 24: // Moderation Response
                    //Logger.Log($"Moderation response is {Event33Data[2]}", Logger.LogsType.Room);
                    //VRConsole.Log($"Server --> Moderation response", VRConsole.LogsType.Moderation);
                    break;
                case 25: // Votekick Response
                    Logger.Log($"Votekick response is {Event33Data[2]} [ID: {Event33Data[3]}]", Logger.LogsType.Room);
                    VRConsole.Log($"Server --> Votekick response", VRConsole.LogsType.Moderation);
                    break;

                case 20: // Request Moderations
                    break;

                case 21: // Reply Moderations (Block / Mute)     
                    bool? MuteCheck = CheckMute(Event33Data);
                    if (MuteCheck == true)
                    {
                        if (!MuteList.Contains((int)Event33Data[1]))
                        {
                            MuteList.Add((int)Event33Data[1]);
                            Photon.Realtime.Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer((int)Event33Data[1]);
                            if (PhotonPlayer != null)
                            {
                                string SenderName = PhotonPlayer.DisplayName() ?? "NO NAME";
                                Logger.Log($"{SenderName} muted you", Logger.LogsType.Room);
                                VRConsole.Log($"{SenderName} --> Mute", VRConsole.LogsType.Mute);
                            }
                        }
                    }

                    else if (MuteCheck == false)
                    {
                        if (MuteList.Contains((int)Event33Data[1]))
                        {
                            MuteList.Remove((int)Event33Data[1]);
                            Photon.Realtime.Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer((int)Event33Data[1]);
                            if (PhotonPlayer != null)
                            {
                                string SenderName = PhotonPlayer.DisplayName() ?? "NO NAME";
                                Logger.Log($"{SenderName} unmuted you", Logger.LogsType.Room);
                                VRConsole.Log($"{SenderName} --> Unmute", VRConsole.LogsType.Mute);
                            }
                        }
                    }

                    bool? BlockCheck = CheckBlock(Event33Data);
                    if (BlockCheck == true)
                    {
                        if (!BlockList.Contains((int)Event33Data[1]))
                        {
                            BlockList.Add((int)Event33Data[1]);
                            Photon.Realtime.Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer((int)Event33Data[1]);
                            if (PhotonPlayer != null)
                            {
                                string SenderName = PhotonPlayer.DisplayName() ?? "NO NAME";
                                Logger.Log($"{SenderName} blocked you", Logger.LogsType.Room);
                                VRConsole.Log($"{SenderName} --> Block", VRConsole.LogsType.Block);
                            }
                        }

                        if (InternalSettings.AntiBlock) return false;
                    }

                    else if (BlockCheck == false)
                    {
                        if (BlockList.Contains((int)Event33Data[1]))
                        {
                            BlockList.Remove((int)Event33Data[1]);
                            Photon.Realtime.Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer((int)Event33Data[1]);
                            if (PhotonPlayer != null)
                            {
                                string SenderName = PhotonPlayer.DisplayName() ?? "NO NAME";
                                Logger.Log($"{SenderName} unblocked you", Logger.LogsType.Room);
                                VRConsole.Log($"{SenderName} --> Unblock", VRConsole.LogsType.Block);
                            }
                        }
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
                                    Photon.Realtime.Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer(Actor);

                                    if (PhotonPlayer != null)
                                    {
                                        string SenderName = PhotonPlayer.DisplayName() ?? "NO NAME";
                                        Logger.Log($"{SenderName} muted you", Logger.LogsType.Room);
                                        VRConsole.Log($"{SenderName} --> Mute", VRConsole.LogsType.Mute);
                                    }
                                }
                            }
                        }

                        else if (MassMuteCheck == false)
                        {
                            foreach (int Actor in MuteList)
                            {
                                Photon.Realtime.Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer(Actor);
                                if (PhotonPlayer != null)
                                {
                                    string SenderName = PhotonPlayer.DisplayName() ?? "NO NAME";
                                    Logger.Log($"{SenderName} unmuted you", Logger.LogsType.Room);
                                    VRConsole.Log($"{SenderName} --> Unmute", VRConsole.LogsType.Mute);
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
                                    Photon.Realtime.Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer(Actor);
                                    if (PhotonPlayer != null)
                                    {
                                        string SenderName = PhotonPlayer.DisplayName() ?? "NO NAME";
                                        Logger.Log($"{SenderName} blocked you", Logger.LogsType.Room);
                                        VRConsole.Log($"{SenderName} --> Block", VRConsole.LogsType.Block);
                                    }
                                }
                            }

                            if (InternalSettings.AntiBlock)
                            {
                                Event33Data[10] = new int[0]; // add all to the normal one ig
                            }
                        }

                        else if (MassBlockCheck == false)
                        {
                            foreach (int Actor in BlockList)
                            {
                                Photon.Realtime.Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer(Actor);
                                if (PhotonPlayer != null)
                                {
                                    string SenderName = PhotonPlayer.DisplayName() ?? "NO NAME";
                                    Logger.Log($"{SenderName} unblocked you", Logger.LogsType.Room);
                                    VRConsole.Log($"{SenderName} --> Unblock", VRConsole.LogsType.Block);
                                }
                            }
                            BlockList.Clear();
                        }
                    }
                    break;

                default:
                    Logger.LogWarning($"Not implemented Moderation Event received with code {Event33Data[0]}");
                    break;
            }
            return true;
        }

        public static bool RaisedModerationEvent(Dictionary<byte, object> Event33Data)
        {
            switch (Convert.ToByte(Event33Data[0]))
            {
                case 23: // Mute
                    if (InternalSettings.SilentMute && Event33Data[3] is bool mutebool) return !mutebool;
                    break;

                case 22: // Block
                    break;

                case 20: // Request Moderations
                    break;

                default:
                    Logger.LogWarning($"Not implemented Moderation Event send with code {Event33Data[0]}");
                    break;
            }

            return true;
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
    }
}
