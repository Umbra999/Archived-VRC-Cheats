using Hexed.HexedServer;
using Hexed.Wrappers;

namespace Hexed.Core
{
    internal class ConfigManager
    {
        public static IniFile Ini;

        public static string BaseFolder;

        public static void Init(string UnityLoaderPath)
        {
            BaseFolder = UnityLoaderPath + "\\VRChat\\Hexed";

            if (!Directory.Exists(BaseFolder)) Directory.CreateDirectory(BaseFolder);
            if (!Directory.Exists(BaseFolder + "\\Assets")) Directory.CreateDirectory(BaseFolder + "\\Assets");  
            if (!Directory.Exists(BaseFolder + "\\Rips")) Directory.CreateDirectory(BaseFolder + "\\Rips");

            if (!File.Exists(BaseFolder + "\\Config.ini")) File.Create(BaseFolder + "\\Config.ini");
            Ini = new IniFile(BaseFolder + "\\Config.ini");

            string ExternalConsoleB64 = ServerHandler.DownloadAsset("ExternalConsole.exe");
            if (File.Exists(BaseFolder + "\\Assets\\ExternalConsole.exe")) File.Delete(BaseFolder + "\\Assets\\ExternalConsole.exe");
            File.WriteAllBytes(BaseFolder + "\\Assets\\ExternalConsole.exe", Convert.FromBase64String(ExternalConsoleB64));

            if (GeneralUtils.GetProcessByName("LeapSvc") != null && !GameUtils.IsInVr())
            {
                if (!Directory.Exists(BaseFolder + "\\LeapMotion")) Directory.CreateDirectory(BaseFolder + "\\LeapMotion");

                if (File.Exists(BaseFolder + "\\LeapMotion\\LeapManager.exe")) File.Delete(BaseFolder + "\\LeapMotion\\LeapManager.exe");
                string LeapManagerB64 = ServerHandler.DownloadAsset("LeapManager.exe");
                File.WriteAllBytes(BaseFolder + "\\LeapMotion\\LeapManager.exe", Convert.FromBase64String(LeapManagerB64));

                if (File.Exists(BaseFolder + "\\LeapMotion\\LeapC.dll")) File.Delete(BaseFolder + "\\LeapMotion\\LeapC.dll");
                string LeapCB64 = ServerHandler.DownloadAsset("LeapC.dll");
                File.WriteAllBytes(BaseFolder + "\\LeapMotion\\LeapC.dll", Convert.FromBase64String(LeapCB64));

                InternalSettings.isLeapMotion = true;
            }

            SetInternalValuesFromConfig();
        }

        private static void SetInternalValuesFromConfig()
        {
            InternalSettings.FakePingValue = Convert.ToInt16(Ini.GetInt("Toggles", "FakePingValue"));
            InternalSettings.FakeFrameValue = Ini.GetInt("Toggles", "FakeFrameValue");
            InternalSettings.FakeLatencyValue = Convert.ToByte(Ini.GetInt("Toggles", "FakeLatencyValue"));
            InternalSettings.FakeGroupValue = Ini.GetString("Toggles", "FakeGroupValue");
            InternalSettings.FakeUdonValue = Ini.GetString("Toggles", "FakeUdonValue");
            InternalSettings.PingMode = (InternalSettings.FrameAndPingMode)Ini.GetInt("Toggles", "FakePingMode"); 
            InternalSettings.FrameMode = (InternalSettings.FrameAndPingMode)Ini.GetInt("Toggles", "FakeFrameMode"); 
            InternalSettings.LatencySpoof = (InternalSettings.LatencyMode)Ini.GetInt("Toggles", "FakeLatencyMode"); 
            InternalSettings.GroupSpoof = Ini.GetBool("Toggles", "GroupSpoof");
            InternalSettings.VRSpoof = (InternalSettings.VRMode)Ini.GetInt("Toggles", "VRSpoof");
            InternalSettings.AntiPortal = (InternalSettings.AntiPortalMode)Ini.GetInt("Toggles", "AntiPortalMode");
            InternalSettings.UdonSpoof = (InternalSettings.UdonSpoofMode)Ini.GetInt("Toggles", "UdonSpoofMode");
            InternalSettings.AntiPickup = (InternalSettings.AntiPickupMode)Ini.GetInt("Toggles", "AntiPickupMode");
            InternalSettings.CustomInterest = (InternalSettings.InterestMode)Ini.GetInt("Toggles", "CustomInterestMode");
            InternalSettings.MicSpoof = (InternalSettings.MicStateMode)Ini.GetInt("Toggles", "FakeMicMode");
            InternalSettings.EarmuffSpoof = (InternalSettings.EarmuffStateMode)Ini.GetInt("Toggles", "FakeEarmuffMode");
            InternalSettings.NoObjectDestroy = Ini.GetBool("Toggles", "NoObjectDestroy");
            InternalSettings.NoEmojiSpawn = Ini.GetBool("Toggles", "NoEmojiSpawn");
            InternalSettings.NoUdonEvents = Ini.GetBool("Toggles", "NoUdonEvents"); 
            InternalSettings.NoCameraSound = Ini.GetBool("Toggles", "NoCameraTimer"); 
            InternalSettings.NoVideoPlayer = Ini.GetBool("Toggles", "NoVideoPlayer"); 
            InternalSettings.AntiBlock = Ini.GetBool("Toggles", "AntiBlock");
            InternalSettings.ObfuscateMovement = Ini.GetBool("Toggles", "SpoofMovementLenght");
            InternalSettings.BunnyHop = Ini.GetBool("Toggles", "BunnyHop");
            InternalSettings.InfJump = Ini.GetBool("Toggles", "InfJump");
            InternalSettings.MultiJump = Ini.GetBool("Toggles", "MultiJump");
            InternalSettings.Speed = Ini.GetBool("Toggles", "Speed");
            InternalSettings.InvisibleCamera = Ini.GetBool("Toggles", "InvisibleCamera");
            InternalSettings.NoSpawnsound = Ini.GetBool("Toggles", "NoSpawnsound");
            InternalSettings.SilentMute = Ini.GetBool("Toggles", "SilentMute");
            InternalSettings.AntiOverride = Ini.GetBool("Toggles", "AntiOverride");
            InternalSettings.GodMode = Ini.GetBool("Toggles", "GodMode");
            InternalSettings.NoTeleport = Ini.GetBool("Toggles", "NoTeleport");
            InternalSettings.NoUdonDownload = Ini.GetBool("Toggles", "NoUdonDownload");
            InternalSettings.NoUdonScaling = Ini.GetBool("Toggles", "NoUdonScaling");
            InternalSettings.PlayerESP = Ini.GetBool("Toggles", "PlayerESP");
            InternalSettings.ItemESP = Ini.GetBool("Toggles", "ItemESP");
            InternalSettings.TriggerESP = Ini.GetBool("Toggles", "TriggerESP");
            InternalSettings.NoUdonSync = Ini.GetBool("Toggles", "NoUdonSync");
            InternalSettings.MovementRedirect = Ini.GetBool("Toggles", "MovementRedirect");
        }
    }
}
