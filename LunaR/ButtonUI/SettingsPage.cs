using LunaR.Extensions;
using LunaR.Modules;
using LunaR.Patching;
using LunaR.UIApi;
using LunaR.Wrappers;
using System;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

namespace LunaR.Buttons
{
    internal class SettingsPage
    {
        public static VRCMenuPage ClientSettingsPage;
        public static MenuPanel PingSpoofPanel;
        public static MenuPanel FPSSpoofPanel;
        public static MenuPanel WorldSpoofPanel;
        public static MenuPanel AntiPortalPanel;
        public static MenuPanel LatencySpoofPanel;
        public static MenuPanel HideBotPanel;
        public static MenuPanel AntiblockPanel;
        public static MenuPanel LockPanel;
        public static MenuPanel HashtableSpoofPanel;

        public static void Init()
        {
            ClientSettingsPage = new VRCMenuPage("LunaR", GameObject.Find("UserInterface/MenuContent/Backdrop/Backdrop").transform, new Vector2(0f, 50f), () => { });
            ClientSettingsPage.MenuButton.Button.transform.localPosition = new Vector3(-695, 0, 0);
            ClientSettingsPage.MenuButton.Button.gameObject.GetComponent<RectTransform>().sizeDelta /= new Vector2(1.45f, 1);

            HashtableSpoofPanel = new MenuPanel(ClientSettingsPage.VRCUiPage.gameObject, "VR Spoof", "", "Disabled", -500, -150);
            HashtableSpoofPanel.AddPageAction("Disabled", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "VRSpoofMode", 0);
                PatchExtensions.VRSpoof = PatchExtensions.VRMode.None;
            });
            HashtableSpoofPanel.AddPageAction("VR", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "VRSpoofMode", 1);
                PatchExtensions.VRSpoof = PatchExtensions.VRMode.VR;
            });
            HashtableSpoofPanel.AddPageAction("Desktop", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "VRSpoofMode", 2);
                PatchExtensions.VRSpoof = PatchExtensions.VRMode.Desktop;
            });

            PingSpoofPanel = new MenuPanel(ClientSettingsPage.VRCUiPage.gameObject, "Ping Spoof", "", "Disabled", 0, -150);
            PingSpoofPanel.AddPageAction("Disabled", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "PingSpoofMode", 0);
                PatchExtensions.PingMode = PatchExtensions.FrameAndPingMode.None;
            });

            PingSpoofPanel.AddPageAction("Custom", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "PingSpoofMode", 1);
                PatchExtensions.PingMode = PatchExtensions.FrameAndPingMode.Custom;
            });

            PingSpoofPanel.AddPageAction("Realistic", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "PingSpoofMode", 2);
                PatchExtensions.PingMode = PatchExtensions.FrameAndPingMode.Realistic;
            });


            FPSSpoofPanel = new MenuPanel(ClientSettingsPage.VRCUiPage.gameObject, "Frame Spoof", "", "Disabled", 500, -150);
            FPSSpoofPanel.AddPageAction("Disabled", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "FrameSpoofMode", 0);
                PatchExtensions.FrameMode = PatchExtensions.FrameAndPingMode.None;
            });

            FPSSpoofPanel.AddPageAction("Custom", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "FrameSpoofMode", 1);
                PatchExtensions.FrameMode = PatchExtensions.FrameAndPingMode.Custom;
            });

            FPSSpoofPanel.AddPageAction("Realistic", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "FrameSpoofMode", 2);
                PatchExtensions.FrameMode = PatchExtensions.FrameAndPingMode.Realistic;
            });

            WorldSpoofPanel = new MenuPanel(ClientSettingsPage.VRCUiPage.gameObject, "World Spoof", "", "Disabled", 0, -410);
            WorldSpoofPanel.AddPageAction("Disabled", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "WorldSpoofMode", 0);
                PatchExtensions.WorldSpoof = PatchExtensions.WorldMode.None;
            });

            WorldSpoofPanel.AddPageAction("Offline", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "WorldSpoofMode", 1);
                PatchExtensions.WorldSpoof = PatchExtensions.WorldMode.Offline;
            });

            WorldSpoofPanel.AddPageAction("Custom", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "WorldSpoofMode", 2);
                PatchExtensions.WorldSpoof = PatchExtensions.WorldMode.Custom;
            });

            AntiPortalPanel = new MenuPanel(ClientSettingsPage.VRCUiPage.gameObject, "Anti Portal", "", "Disabled", 500, -410);
            AntiPortalPanel.AddPageAction("Disabled", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "AntiPortalMode", 0);
                PatchExtensions.AntiPortal = PatchExtensions.AntiPortalMode.None;
            });

            AntiPortalPanel.AddPageAction("All", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "AntiPortalMode", 1);
                PatchExtensions.AntiPortal = PatchExtensions.AntiPortalMode.All;
            });

            AntiPortalPanel.AddPageAction("Friends", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "AntiPortalMode", 2);
                PatchExtensions.AntiPortal = PatchExtensions.AntiPortalMode.Friends;
            });


            LatencySpoofPanel = new MenuPanel(ClientSettingsPage.VRCUiPage.gameObject, "Latency Spoof", "", "Disabled", 0, -670);
            LatencySpoofPanel.AddPageAction("Disabled", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "LatencyMode", 0);
                PatchExtensions.LatencySpoof = PatchExtensions.LatencyMode.None;
            });

            LatencySpoofPanel.AddPageAction("Custom", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "LatencyMode", 1);
                PatchExtensions.LatencySpoof = PatchExtensions.LatencyMode.Custom;
            });

            LatencySpoofPanel.AddPageAction("Low", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "LatencyMode", 2);
                PatchExtensions.LatencySpoof = PatchExtensions.LatencyMode.Low;
            });

            LatencySpoofPanel.AddPageAction("High", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "LatencyMode", 3);
                PatchExtensions.LatencySpoof = PatchExtensions.LatencyMode.High;
            });

            HideBotPanel = new MenuPanel(ClientSettingsPage.VRCUiPage.gameObject, "Hide Robot", "", "Disabled", 500, -670);
            HideBotPanel.AddPageAction("Disabled", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "HideBotSpoof", 0);
                PatchExtensions.HideSpoof = PatchExtensions.HideMode.None;

                PlayerExtensions.ReloadAvatar(APIUser.CurrentUser);
            });

            HideBotPanel.AddPageAction("Mini", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "HideBotSpoof", 1);
                PatchExtensions.HideSpoof = PatchExtensions.HideMode.Mini;

                PlayerExtensions.ReloadAvatar(APIUser.CurrentUser);
            });

            HideBotPanel.AddPageAction("Big", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "HideBotSpoof", 2);
                PatchExtensions.HideSpoof = PatchExtensions.HideMode.Big;

                PlayerExtensions.ReloadAvatar(APIUser.CurrentUser);
            });

            HideBotPanel.AddPageAction("Custom", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "HideBotSpoof", 3);
                PatchExtensions.HideSpoof = PatchExtensions.HideMode.Custom;

                PlayerExtensions.ReloadAvatar(APIUser.CurrentUser);
            });

            AntiblockPanel = new MenuPanel(ClientSettingsPage.VRCUiPage.gameObject, "Antiblock", "", "Disabled", -500, -410);
            AntiblockPanel.AddPageAction("Disabled", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "AntiBlockMode", 0);
                ModerationHandler.AntiBlock = ModerationHandler.BlockMode.None;
            });

            AntiblockPanel.AddPageAction("All", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "AntiBlockMode", 1);
                ModerationHandler.AntiBlock = ModerationHandler.BlockMode.All;
            });

            AntiblockPanel.AddPageAction("Friends", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "AntiBlockMode", 2);
                ModerationHandler.AntiBlock = ModerationHandler.BlockMode.NoFriends;
            });

            LockPanel = new MenuPanel(ClientSettingsPage.VRCUiPage.gameObject, "Instance Lock", "", "Disabled", -500, -670);
            LockPanel.AddPageAction("Disabled", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "LockInstanceMode", 0);
                PatchExtensions.MasterLock = PatchExtensions.InstanceLock.None;
            });

            LockPanel.AddPageAction("All", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "LockInstanceMode", 1);
                PatchExtensions.MasterLock = PatchExtensions.InstanceLock.All;
            });

            LockPanel.AddPageAction("Friends", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "LockInstanceMode", 2);
                PatchExtensions.MasterLock = PatchExtensions.InstanceLock.Friends;
            });

            LockPanel.AddPageAction("Blocked", delegate
            {
                Extensions.ConfigManager.Ini.SetInt("Toggles", "LockInstanceMode", 3);
                PatchExtensions.MasterLock = PatchExtensions.InstanceLock.Blocked;
            });

            new MenuButton(ClientSettingsPage.VRCUiPage.gameObject, MenuButtonType.PlaylistButton, "Custom Ping", 765f, -260f, delegate
            {
                Utils.VRCUiPopupManager.NumberInput("0 - 32767", "", InputField.InputType.Standard, true, "Ok", (s, k, t) =>
                {
                    Extensions.ConfigManager.Ini.SetInt("Toggles", "FakePing", Convert.ToInt32(s));
                    PatchExtensions.FakePingValue = Convert.ToInt16(Extensions.ConfigManager.Ini.GetInt("Toggles", "FakePing"));
                });
            });

            new MenuButton(ClientSettingsPage.VRCUiPage.gameObject, MenuButtonType.PlaylistButton, "Custom Frame", 1265f, -260f, delegate
            {
                Utils.VRCUiPopupManager.NumberInput("0 - 1000", "", InputField.InputType.Standard, true, "Ok", (s, k, t) =>
                {
                    Extensions.ConfigManager.Ini.SetInt("Toggles", "FakeFrames", Convert.ToInt32(s));
                    int FPS = Extensions.ConfigManager.Ini.GetInt("Toggles", "FakeFrames");
                    PatchExtensions.FakeFrameValue = (float)1 / FPS;
                });
            });

            new MenuButton(ClientSettingsPage.VRCUiPage.gameObject, MenuButtonType.PlaylistButton, "Custom Height", 1265f, -780f, delegate
            {
                Utils.VRCUiPopupManager.NumberInput("200 - 3000", "", InputField.InputType.Standard, true, "Ok", (s, k, t) =>
                {
                    Extensions.ConfigManager.Ini.SetInt("Toggles", "FakeHeight", Convert.ToInt32(s));
                    PatchExtensions.FakeHeightValue = Extensions.ConfigManager.Ini.GetInt("Toggles", "FakeHeight");
                });
            });

            new MenuButton(ClientSettingsPage.VRCUiPage.gameObject, MenuButtonType.PlaylistButton, "Custom Latency", 765f, -780f, delegate
            {
                Utils.VRCUiPopupManager.NumberInput("0 - 255", "", InputField.InputType.Standard, true, "Ok", (s, k, t) =>
                {
                    Extensions.ConfigManager.Ini.SetInt("Toggles", "FakeLatency", Convert.ToInt32(s));
                    PatchExtensions.FakeLatencyValue = Extensions.ConfigManager.Ini.GetInt("Toggles", "FakeLatency");
                });
            });

            new MenuButton(ClientSettingsPage.VRCUiPage.gameObject, MenuButtonType.PlaylistButton, "Custom World", 765f, -520f, delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("WorldID", "Ok", delegate (string text)
                {
                    Extensions.ConfigManager.Ini.SetString("Toggles", "FakeWorldID", text);
                    PatchExtensions.FakeWorldID = Extensions.ConfigManager.Ini.GetString("Toggles", "FakeWorldID");
                });
            });
        }
    }
}