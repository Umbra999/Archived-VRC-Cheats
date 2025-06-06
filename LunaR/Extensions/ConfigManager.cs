using LunaR.Buttons;
using LunaR.Modules;
using LunaR.Patching;
using LunaR.Wrappers;
using MelonLoader;
using System.IO;

namespace LunaR.Extensions
{
    internal class ConfigManager
    {
        public static IniFile Ini;

        public static void Init()
        {
            if (!File.Exists("LunaR\\Config.ini"))
            {
                File.Create("LunaR\\Config.ini");
                Ini = new IniFile("LunaR\\Config.ini");
            }
            else Ini = new IniFile("LunaR\\Config.ini");

            if (!Ini.GetString("Toggles", "HideRobot").Contains("https://")) Ini.SetString("Toggles", "HideRobot", "https://cdn.discordapp.com/attachments/855072998796296212/895341093447675964/Cute_Robot.vrca");

            ModerationHandler.AntiBlock = (ModerationHandler.BlockMode)Ini.GetInt("Toggles", "AntiBlockMode");
            PatchExtensions.VRSpoof = (PatchExtensions.VRMode)Ini.GetInt("Toggles", "VRSpoofMode");
            PatchExtensions.MasterLock = (PatchExtensions.InstanceLock)Ini.GetInt("Toggles", "LockInstanceMode");
            PatchExtensions.HideSpoof = (PatchExtensions.HideMode)Ini.GetInt("Toggles", "HideBotSpoof");
            PatchExtensions.FrameMode = (PatchExtensions.FrameAndPingMode)Ini.GetInt("Toggles", "FrameSpoofMode");
            PatchExtensions.PingMode = (PatchExtensions.FrameAndPingMode)Ini.GetInt("Toggles", "PingSpoofMode");
            PatchExtensions.LatencySpoof = (PatchExtensions.LatencyMode)Ini.GetInt("Toggles", "LatencyMode");
            PatchExtensions.AntiPortal = (PatchExtensions.AntiPortalMode)Ini.GetInt("Toggles", "AntiPortalMode");
            PatchExtensions.WorldSpoof = (PatchExtensions.WorldMode)Ini.GetInt("Toggles", "WorldSpoofMode");

            PatchExtensions.AntiDestroy = Ini.GetBool("Toggles", "AntiDestroy");
            PatchExtensions.AntiMimic = Ini.GetBool("Toggles", "AntiMimic");
            PatchExtensions.AntiWorldTrigger = Ini.GetBool("Toggles", "AntiTrigger");
            PatchExtensions.AntiUdon = Ini.GetBool("Toggles", "AntiUdon");
            Movement.DoubleJump = Ini.GetBool("Toggles", "DoubleJump");
            Movement.InfJump = Ini.GetBool("Toggles", "InfJump");
            PatchExtensions.WorldTrigger = Ini.GetBool("Toggles", "GlobalTrigger");
            PatchExtensions.AntiUdonSync = Ini.GetBool("Toggles", "AntiUdonSync");
            PatchExtensions.MuteQuest = Ini.GetBool("Toggles", "MuteQuest");
            Movement.BunnyHop = Ini.GetBool("Toggles", "BunnyJump");
            AvatarAdjustment.AntiSpawnsound = Ini.GetBool("Toggles", "AntiAudio");
            PortalHandler.NoPortalFollow = Ini.GetBool("Toggles", "PortalFollow");
            ModerationHandler.SeeBlocked = Ini.GetBool("Toggles", "SeeBlocked");
            PatchExtensions.FreezeItems = Ini.GetBool("Toggles", "FreezeItems");
            PatchExtensions.HiddenCamera = Ini.GetBool("Toggles", "AntiCamera");
            ItemHandler.NoPickup = Ini.GetBool("Toggles", "NoPickup");
            PatchExtensions.AntiEmoji = Ini.GetBool("Toggles", "AntiEmoji");
            PatchExtensions.InfinityPortals = Ini.GetBool("Toggles", "InfinityPortals");
            PatchExtensions.NoChairs = Ini.GetBool("Toggles", "NoChairs");
            PatchExtensions.AntiTimer = Ini.GetBool("Toggles", "AntiTimer");
            PatchExtensions.UdonSpoof = Ini.GetBool("Toggles", "UdonSpoof");

            int FPS = Ini.GetInt("Toggles", "FakeFrames");
            PatchExtensions.FakeFrameValue = (float)1 / FPS;
            PatchExtensions.FakePingValue = (short)Ini.GetInt("Toggles", "FakePing");
            PatchExtensions.FakeHeightValue = Ini.GetInt("Toggles", "FakeHeight");
            PatchExtensions.FakeLatencyValue = Ini.GetInt("Toggles", "FakeLatency");
            PatchExtensions.FakeWorldID = Ini.GetString("Toggles", "FakeWorldID");
        }

        public static void SetupPageInfo()
        {
            SettingsPage.PingSpoofPanel.SetPanelIndex((int)PatchExtensions.PingMode);
            SettingsPage.FPSSpoofPanel.SetPanelIndex((int)PatchExtensions.FrameMode);
            SettingsPage.WorldSpoofPanel.SetPanelIndex((int)PatchExtensions.WorldSpoof);
            SettingsPage.AntiPortalPanel.SetPanelIndex((int)PatchExtensions.AntiPortal);
            SettingsPage.LatencySpoofPanel.SetPanelIndex((int)PatchExtensions.LatencySpoof);
            SettingsPage.HideBotPanel.SetPanelIndex((int)PatchExtensions.HideSpoof);
            SettingsPage.AntiblockPanel.SetPanelIndex((int)ModerationHandler.AntiBlock);
            SettingsPage.LockPanel.SetPanelIndex((int)PatchExtensions.MasterLock);
            SettingsPage.HashtableSpoofPanel.SetPanelIndex((int)PatchExtensions.VRSpoof);
        }
    }
}