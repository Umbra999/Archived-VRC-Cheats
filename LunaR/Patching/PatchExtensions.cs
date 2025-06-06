using ExitGames.Client.Photon;
using LunaR.Extensions;
using LunaR.Patching;
using Newtonsoft.Json;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaR.Wrappers
{
    internal class PatchExtensions
    {
        public static WorldMode WorldSpoof = 0;
        public enum WorldMode
        {
            None = 0,
            Offline = 1,
            Custom = 2
        }

        public static VRMode VRSpoof = 0;
        public enum VRMode
        {
            None = 0,
            VR = 1,
            Desktop = 2
        }

        public static InstanceLock MasterLock = 0;

        public enum InstanceLock
        {
            None = 0,
            All = 1,
            Friends = 2,
            Blocked = 3
        }

        public static HideMode HideSpoof = 0;

        public enum HideMode
        {
            None = 0,
            Mini = 1,
            Big = 2,
            Custom = 3
        }

        public static FrameAndPingMode FrameMode = 0;
        public static FrameAndPingMode PingMode = 0;

        public enum FrameAndPingMode
        {
            None = 0,
            Custom = 1,
            Realistic = 2
        }

        public static LatencyMode LatencySpoof = 0;
        public enum LatencyMode
        {
            None = 0,
            Custom = 1,
            Low = 2,
            High = 3
        }

        public static AntiPortalMode AntiPortal = 0;

        public enum AntiPortalMode
        {
            None = 0,
            All = 1,
            Friends = 2
        }

        public static bool BlockOPRaise = false;
        public static Dictionary<string, DateTime> InstanceHistory = new();
        public static bool HiddenCamera = false;
        public static bool AntiEmoji = false;
        public static bool AntiDestroy = false;
        public static bool RepeatUspeak = false;
        public static bool InfinityPortals = false;
        public static bool AntiUdonSync = false;
        public static string VoiceTarget = "";
        public static bool RepeatMovement = false;
        public static string MovementTarget = "";
        public static bool OpRaiseLogger = false;
        public static bool ShowStats = false;
        public static bool OperationLog = false;
        public static bool WorldTrigger = false;
        public static bool AntiWorldTrigger = false;
        public static bool AntiUdon = false;
        public static bool TeleportInfinity = false;
        public static bool FreezeItems = false;
        public static bool NoChairs = false;
        public static bool UdonSpoof = false;
        public static bool AntiTimer = false;

        public static bool EventLog = false;
        public static bool RPCLog = false;
        public static bool WebsocketLog = false;
        public static bool MuteQuest = false;
        public static bool AntiMimic = false;

        public static short FakePingValue = 69;
        public static float FakeFrameValue = 110;
        public static int FakeHeightValue = 1337;
        public static int FakeLatencyValue = 0;
        public static string FakeWorldID = "wrld_4cf554b4-430c-4f8f-b53e-1f294eed230b:999";

        public static int LastServerEvent = 0;

        public static Dictionary<int, int> SerializedActors = new();
        public static List<int> NonSerializedActors = new();

        public static int PatternScan(byte[] src, byte[] pattern, int IgnoreIndex = -1)
        {
            int maxFirstCharSlot = src.Length - pattern.Length + 1;
            for (int i = 0; i < maxFirstCharSlot; i++)
            {
                if (src[i] != pattern[0]) continue;
                for (int j = pattern.Length - 1; j >= 1; j--)
                {
                    if (src[i + j] != pattern[j]) break;
                    if (j == 1 && i != IgnoreIndex) return i;
                }
            }
            return -1;
        }

        public static int[] PatternScans(byte[] src, byte[] pattern)
        {
            List<int> Found = new();
            int maxFirstCharSlot = src.Length - pattern.Length + 1;
            for (int i = 0; i < maxFirstCharSlot; i++)
            {
                if (src[i] != pattern[0]) continue;
                for (int j = pattern.Length - 1; j >= 1; j--)
                {
                    if (src[i + j] != pattern[j]) break;
                    if (j == 1) Found.Add(i);
                }
            }
            return Found.ToArray();
        }

        public static void RemoveActorFromCache(int Actor)
        {
            if (SerializedActors.ContainsKey(Actor)) SerializedActors.Remove(Actor);
            if (NonSerializedActors.Contains(Actor)) NonSerializedActors.Remove(Actor);
            if (ModerationHandler.BlockList.Contains(Actor)) ModerationHandler.BlockList.Remove(Actor);
            if (ModerationHandler.MuteList.Contains(Actor)) ModerationHandler.MuteList.Remove(Actor);
            if (BotDetection.DetectedSerializer.Contains(Actor)) BotDetection.DetectedSerializer.Remove(Actor);
            if (BotDetection.DetectedBots.Contains(Actor)) BotDetection.DetectedBots.Remove(Actor);
            if (PhotonExtensions.CachedHashtables.ContainsKey(Actor)) PhotonExtensions.CachedHashtables.Remove(Actor);
        }

        public static List<byte> LogIgnoreCodes = new()
        {
            1,
            7,
            8,
            35,
        };

        public static void LogEvent(EventData EventData, bool IgnoreList = false)
        {
            try
            {
                if (LogIgnoreCodes.Contains(EventData.Code) && !IgnoreList) return;

                object CustomData = EventData.CustomData == null ? "NULL" : IL2CPPSerializer.IL2CPPToManaged.Serialize(EventData.CustomData);

                Player PhotonPlayer = null;
                string SenderNAME = "NULL";

                if (EventData.Sender > 0) PhotonPlayer = PhotonExtensions.GetPhotonPlayer(EventData.Sender);
                if (PhotonPlayer != null)
                {
                    SenderNAME = PhotonPlayer.GetPlayer() == null ? PhotonPlayer.GetDisplayName() : PhotonPlayer.GetPlayer().DisplayName();
                }

                string IfBytes = "";
                if (CustomData.GetType() == typeof(byte[]))
                {
                    IfBytes = "  |  ";
                    byte[] arr = (byte[])CustomData;
                    IfBytes += string.Join(", ", arr);
                    IfBytes += $" [L: {arr.Length}]";
                }

                Extensions.Logger.Log(string.Concat(new object[]
                {
                    Environment.NewLine,
                    $"======= ONEVENT {EventData.Code} =======", Environment.NewLine,
                    $"ACTOR NUMBER: {EventData.Sender}", Environment.NewLine,
                    $"SENDER NAME: {SenderNAME}", Environment.NewLine,
                    $"TYPE: {CustomData}", Environment.NewLine,
                    $"DATA SERIALIZED: {JsonConvert.SerializeObject(CustomData, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })}{IfBytes}", Environment.NewLine,
                    "======= END =======",
                }), Extensions.Logger.LogsType.Clean);
            }
            catch (Exception e)
            {
                Extensions.Logger.LogError($"Failed to Log Event with code {EventData.Code} with Exception: {e}");
            }
        }

        public static bool IsBadPosition(Vector3 v3)
        {
            if (IsInfinity(v3)) return true;
            if (IsNaN(v3)) return true;
            return false;
        }

        public static bool IsBadRotation(Quaternion v3)
        {
            if (IsInfinityRotation(v3)) return true;
            if (IsNaNRotation(v3)) return true;
            return false;
        }

        private static bool IsNaN(Vector3 v3)
        {
            return float.IsNaN(v3.x) || float.IsNaN(v3.y) || float.IsNaN(v3.z) || float.IsInfinity(v3.x) || float.IsInfinity(v3.y) || float.IsInfinity(v3.z);
        }

        private static bool IsInfinity(Vector3 v3)
        {
            return 998001 <= v3.x || 998001 <= v3.y || 998001 <= v3.z || -998001 >= v3.x || -998001 >= v3.y || -998001 >= v3.z;
        }

        private static bool IsNaNRotation(Quaternion v3)
        {
            return float.IsNaN(v3.x) || float.IsNaN(v3.y) || float.IsNaN(v3.z) || float.IsInfinity(v3.x) || float.IsInfinity(v3.y) || float.IsInfinity(v3.z);
        }

        private static bool IsInfinityRotation(Quaternion v3)
        {
            return 998001 <= v3.x || 998001 <= v3.y || 998001 <= v3.z || 998001 <= v3.w || -998001 >= v3.x || -998001 >= v3.y || -998001 >= v3.z || -998001 >= v3.w;
        }

        public static void LogOpRaise(byte Code, Il2CppSystem.Object Object, RaiseEventOptions RaiseOptions, SendOptions SendOptions)
        {
            try
            {
                if (LogIgnoreCodes.Contains(Code)) return;

                object CustomData = Object == null ? "NULL" : IL2CPPSerializer.IL2CPPToManaged.Serialize(Object);
                string TargetActors = RaiseOptions.field_Public_ArrayOf_Int32_0 != null ? string.Join(", ", RaiseOptions.field_Public_ArrayOf_Int32_0) : "NULL";

                string IfBytes = "";
                if (CustomData.GetType() == typeof(byte[]))
                {
                    IfBytes = "  |  ";
                    byte[] data = (byte[])CustomData;
                    if (Code == 8)
                    {
                        int index = 0;
                        while (index < data.Length)
                        {
                            int ViewID = BitConverter.ToInt32(data, index);
                            index += 4;
                            byte UpdateFreq = data[index++];
                            byte VoiceQuality = data[index++];
                            IfBytes += $"\nViewID:{ViewID} Freq:{UpdateFreq} Quality:{VoiceQuality}";
                        }
                    }
                    else
                    {
                        IfBytes += string.Join(", ", data);
                        IfBytes += $" [L: {data.Length}]";
                    }
                }

                Extensions.Logger.Log(string.Concat(new object[]
                {
                    Environment.NewLine,
                    $"======= OPRAISEEVENT {Code} =======", Environment.NewLine,
                    $"TYPE: {CustomData}", Environment.NewLine,
                    $"DATA SERIALIZED: {JsonConvert.SerializeObject(CustomData, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })}{IfBytes}", Environment.NewLine,
                    $"TARGET ACTORS: {TargetActors}", Environment.NewLine,
                    $"EVENT CACHING: {RaiseOptions.field_Public_EventCaching_0}", Environment.NewLine,
                    $"BYTE #1: {RaiseOptions.field_Public_Byte_0}", Environment.NewLine,
                    $"BYTE #2: {RaiseOptions.field_Public_Byte_1}", Environment.NewLine,
                    $"RECEIVER GROUP: {RaiseOptions.field_Public_ReceiverGroup_0}", Environment.NewLine,
                    $"CHANNEL: {SendOptions.Channel}",Environment.NewLine,
                    $"DELIVERY MODE: {SendOptions.DeliveryMode}",Environment.NewLine,
                    $"ENCRYPT: {SendOptions.Encrypt}",Environment.NewLine,
                    $"RELIABLE: {SendOptions.Reliability}",Environment.NewLine,
                    "======= END =======",
                }), Extensions.Logger.LogsType.Clean);
            }
            catch (Exception e)
            {
                Extensions.Logger.LogError($"Failed to Log Event with code {Code} with Exception: {e}");
            }
        }
    }
}