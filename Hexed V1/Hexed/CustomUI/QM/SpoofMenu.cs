using Hexed.Core;
using Hexed.Extensions;
using Hexed.UIApi;
using Hexed.Wrappers;
using System;

namespace Hexed.CustomUI.QM
{
    internal class SpoofMenu
    {
        private static QMMenuPage SpoofPage;

        public static void Init()
        {
            SpoofPage = new("Spoof");

            QMSingleButton OpenMenu = new(MainMenu.ClientPage, 2, 2, "Spoof", SpoofPage.OpenMe, "Spoof Options", ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Spoof"));

            QMSelectButton UdonSPoofMenu = new(SpoofPage, 1, 0, "Udon \nSpoof", "Spoof your Udon name", InternalSettings.UdonSpoof, ConfigManager.Ini.GetInt("Toggles", "UdonSpoofMode"));
            UdonSPoofMenu.AddAction(0, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "UdonSpoofMode", 0);
                InternalSettings.UdonSpoof = InternalSettings.UdonSpoofMode.None;
            });
            UdonSPoofMenu.AddAction(1, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "UdonSpoofMode", 1);
                InternalSettings.UdonSpoof = InternalSettings.UdonSpoofMode.Owner;
            });
            UdonSPoofMenu.AddAction(2, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "UdonSpoofMode", 2);
                InternalSettings.UdonSpoof = InternalSettings.UdonSpoofMode.Random;
            });
            UdonSPoofMenu.AddAction(3, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "UdonSpoofMode", 3);
                InternalSettings.UdonSpoof = InternalSettings.UdonSpoofMode.Custom;
                GameHelper.VRCUiPopupManager.AskInGameInput("Fake Name", "Ok", delegate (string text)
                {
                    InternalSettings.FakeUdonValue = text;
                    ConfigManager.Ini.SetString("Toggles", "FakeUdonValue", InternalSettings.FakeUdonValue);
                }, "Name");
            });

            new QMToggleButton(SpoofPage, 2, 0, "Group \nSpoof", delegate
            {
                GameHelper.VRCUiPopupManager.AskInGameInput("Custom Text", "Ok", delegate (string text)
                {
                    InternalSettings.FakeGroupValue = text;
                    ConfigManager.Ini.SetString("Toggles", "FakeGroupValue", InternalSettings.FakeGroupValue);
                    ConfigManager.Ini.SetBool("Toggles", "GroupSpoof", true);
                    InternalSettings.GroupSpoof = true;
                    PhotonHelper.RaiseGroupOnNameplate(InternalSettings.FakeGroupValue);
                });
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "GroupSpoof", false);
                InternalSettings.GroupSpoof = false;
            }, "Spoof your represented Group", ConfigManager.Ini.GetBool("Toggles", "GroupSpoof"));

            QMSelectButton FakeVRMenu = new(SpoofPage, 3, 0, "VR \nSpoof", "Spoof your VR state", InternalSettings.VRSpoof, ConfigManager.Ini.GetInt("Toggles", "VRMode"));
            FakeVRMenu.AddAction(0, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "VRMode", 0);
                InternalSettings.VRSpoof = InternalSettings.VRMode.None;
                PhotonHelper.RaiseVRMode(GameUtils.IsInVr());
            });
            FakeVRMenu.AddAction(1, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "VRMode", 1);
                InternalSettings.VRSpoof = InternalSettings.VRMode.VR;
                PhotonHelper.RaiseVRMode(true);
            });
            FakeVRMenu.AddAction(2, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "VRMode", 2);
                InternalSettings.VRSpoof = InternalSettings.VRMode.Desktop;
                PhotonHelper.RaiseVRMode(false);
            });

            QMSelectButton FakeLatencyMenu = new(SpoofPage, 4, 0, "Latency \nSpoof", "Spoof your Latency", InternalSettings.LatencySpoof, ConfigManager.Ini.GetInt("Toggles", "FakeLatencyMode"));
            FakeLatencyMenu.AddAction(0, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "FakeLatencyMode", 0);
                InternalSettings.LatencySpoof = InternalSettings.LatencyMode.None;
            });
            FakeLatencyMenu.AddAction(1, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "FakeLatencyMode", 1);
                InternalSettings.LatencySpoof = InternalSettings.LatencyMode.Custom;
                GameHelper.VRCUiPopupManager.AskInGameInput("Latency Value", "Ok", delegate (string text)
                {
                    InternalSettings.FakeLatencyValue = Convert.ToByte(text);
                    ConfigManager.Ini.SetInt("Toggles", "FakeLatencyValue", InternalSettings.FakeLatencyValue);
                }, "0 - 255");
            });
            FakeLatencyMenu.AddAction(2, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "FakeLatencyMode", 2);
                InternalSettings.LatencySpoof = InternalSettings.LatencyMode.Low;
            });
            FakeLatencyMenu.AddAction(3, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "FakeLatencyMode", 3);
                InternalSettings.LatencySpoof = InternalSettings.LatencyMode.High;
            });

            QMSelectButton FakePingMenu = new(SpoofPage, 1, 1, "Ping \nSpoof", "Spoof your Ping", InternalSettings.PingMode, ConfigManager.Ini.GetInt("Toggles", "FakePingMode"));
            FakePingMenu.AddAction(0, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "FakePingMode", 0);
                InternalSettings.PingMode = InternalSettings.FrameAndPingMode.None;
            });
            FakePingMenu.AddAction(1, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "FakePingMode", 1);
                InternalSettings.PingMode = InternalSettings.FrameAndPingMode.Custom;
                GameHelper.VRCUiPopupManager.AskInGameInput("Ping Value", "Ok", delegate (string text)
                {
                    InternalSettings.FakePingValue = Convert.ToInt16(text);
                    ConfigManager.Ini.SetInt("Toggles", "FakePingValue", InternalSettings.FakePingValue);
                }, "-32767 - 32767");
            });
            FakePingMenu.AddAction(2, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "FakePingMode", 2);
                InternalSettings.PingMode = InternalSettings.FrameAndPingMode.Realistic;
            });

            QMSelectButton FakeFrameMenu = new(SpoofPage, 2, 1, "Frame \nSpoof", "Spoof your Frames", InternalSettings.FrameMode, ConfigManager.Ini.GetInt("Toggles", "FakeFrameMode"));
            FakeFrameMenu.AddAction(0, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "FakeFrameMode", 0);
                InternalSettings.FrameMode = InternalSettings.FrameAndPingMode.None;
            });
            FakeFrameMenu.AddAction(1, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "FakeFrameMode", 1);
                InternalSettings.FrameMode = InternalSettings.FrameAndPingMode.Custom;
                GameHelper.VRCUiPopupManager.AskInGameInput("Frame Value", "Ok", delegate (string text)
                {
                    InternalSettings.FakeFrameValue = Convert.ToInt32(text);
                    ConfigManager.Ini.SetInt("Toggles", "FakeFrameValue", InternalSettings.FakeFrameValue);
                }, "0 - 1000");
            });
            FakeFrameMenu.AddAction(2, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "FakeFrameMode", 2);
                InternalSettings.FrameMode = InternalSettings.FrameAndPingMode.Realistic;
            });

            QMSelectButton FakeMicMenu = new(SpoofPage, 3, 1, "Mic \nSpoof", "Spoof your Mic state", InternalSettings.MicSpoof, ConfigManager.Ini.GetInt("Toggles", "FakeMicMode"));
            FakeMicMenu.AddAction(0, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "FakeMicMode", 0);
                InternalSettings.MicSpoof = InternalSettings.MicStateMode.None;
            });
            FakeMicMenu.AddAction(1, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "FakeMicMode", 1);
                InternalSettings.MicSpoof = InternalSettings.MicStateMode.Muted;
            });
            FakeMicMenu.AddAction(2, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "FakeMicMode", 2);
                InternalSettings.MicSpoof = InternalSettings.MicStateMode.Unmuted;
            });

            QMSelectButton FakeEarmuffMenu = new(SpoofPage, 4, 1, "Earmuff \nSpoof", "Spoof your Earmuff state", InternalSettings.EarmuffSpoof, ConfigManager.Ini.GetInt("Toggles", "FakeEarmuffMode"));
            FakeEarmuffMenu.AddAction(0, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "FakeEarmuffMode", 0);
                InternalSettings.EarmuffSpoof = InternalSettings.EarmuffStateMode.None;
            });
            FakeEarmuffMenu.AddAction(1, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "FakeEarmuffMode", 1);
                InternalSettings.EarmuffSpoof = InternalSettings.EarmuffStateMode.Enabled;
            });
            FakeEarmuffMenu.AddAction(2, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "FakeEarmuffMode", 2);
                InternalSettings.EarmuffSpoof = InternalSettings.EarmuffStateMode.Disabled;
            });
        }
    }
}
