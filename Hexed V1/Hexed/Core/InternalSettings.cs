namespace Hexed.Core
{
    internal class InternalSettings
    {
        public enum FrameAndPingMode
        {
            None = 0,
            Custom = 1,
            Realistic = 2
        }

        public enum LatencyMode
        {
            None = 0,
            Custom = 1,
            Low = 2,
            High = 3
        }

        public enum AntiPortalMode
        {
            None = 0,
            All = 1,
            Friends = 2
        }

        public enum ChatboxMode
        {
            None = 0,
            Static = 1,
            Spotify = 2
        }

        public enum VRMode
        {
            None = 0,
            VR = 1,
            Desktop = 2
        }

        public enum UdonSpoofMode
        {
            None = 0,
            Owner = 1,
            Random = 2,
            Custom = 3
        }

        public enum AntiPickupMode
        {
            None = 0,
            Self = 1,
            Nobody = 2,
        }

        public enum InterestMode
        {
            None = 0,
            Reversed = 1,
            All = 2,
        }

        public enum MicStateMode
        {
            None = 0,
            Muted = 1,
            Unmuted = 2,
        }

        public enum EarmuffStateMode
        {
            None = 0,
            Enabled = 1,
            Disabled = 2,
        }

        // Config Multiple
        public static FrameAndPingMode FrameMode = FrameAndPingMode.None;
        public static FrameAndPingMode PingMode = FrameAndPingMode.None;
        public static LatencyMode LatencySpoof = LatencyMode.None;
        public static AntiPortalMode AntiPortal = AntiPortalMode.None;
        public static ChatboxMode ChatBox = ChatboxMode.None;
        public static VRMode VRSpoof = VRMode.None;
        public static UdonSpoofMode UdonSpoof = UdonSpoofMode.None;
        public static AntiPickupMode AntiPickup = AntiPickupMode.None;
        public static InterestMode CustomInterest = InterestMode.None;
        public static MicStateMode MicSpoof = MicStateMode.None;
        public static EarmuffStateMode EarmuffSpoof = EarmuffStateMode.None;

        public static int FakeFrameValue = 1000;
        public static short FakePingValue = -999;
        public static byte FakeLatencyValue = 0;
        public static string FakeUdonValue = "";

        // Logs
        public static bool OnEventLog = false;
        public static bool OpRaiseLog = false;
        public static bool RPCLog = false;
        public static bool OperationLog = false;
        public static bool OperationResponseLog = false;
        public static bool APILog = false;
        public static bool SocketLog = false;

        // Internal
        public static bool isCameraZoomed = false;
        public static long joinedRoomTime = 0;
        public static Dictionary<int, long> ActorsWithLastActiveTime = new();
        public static Dictionary<string, DateTime> InstanceHistory = new();
        public static bool isLeapMotion = false;

        // Config
        public static bool PlayerESP = false;
        public static bool ItemESP = false;
        public static bool TriggerESP = false;
        public static bool NoObjectDestroy = false;
        public static bool NoEmojiSpawn = false;
        public static bool NoUdonEvents = false;
        public static bool NoUdonDownload = false;
        public static bool NoUdonScaling = false;
        public static bool NoCameraSound = false;
        public static bool NoVideoPlayer = false;
        public static bool AntiBlock = false;
        public static bool ObfuscateMovement = false;
        public static bool BunnyHop = false;
        public static bool InfJump = false;
        public static bool MultiJump = false;
        public static bool HighJump = false;
        public static bool Speed = false;
        public static bool InvisibleCamera = false;
        public static bool NoSpawnsound = false;
        public static bool SilentMute = false;
        public static bool AntiOverride = false;
        public static bool GodMode = false;
        public static bool NoTeleport = false;
        public static bool GroupSpoof = false;
        public static string FakeGroupValue = "";
        public static bool NoUdonSync = false;
        public static bool MovementRedirect = false;

        // Session
        public static bool ShowPerformanceStats = false;
        public static bool InfinityPosition = false;
        public static bool VoiceDistortion = false;
        public static bool BigAvatar = false;
        public static bool InvisibleConnect = false;
        public static int RepeatVoiceActor = -2;
        public static int RepeatChatActor = -2;
    }
}
