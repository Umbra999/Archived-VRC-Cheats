using LunaR.Api;
using LunaR.Buttons.Bots;
using LunaR.Extensions;
using LunaR.Modules;
using LunaR.Wrappers;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace LunaR.QMButtons
{
    internal class BotMenu
    {
        public static bool BotFollow = false;

        public static QMSlider XSlider;
        public static QMSlider YSlider;
        public static QMSlider ZSlider;

        public static QMSlider XRSlider;
        public static QMSlider YRSlider;
        public static QMSlider ZRSlider;

        public static QMSlider OSpeedSlider;
        public static QMSlider ODistanceSlider;

        public static QMSlider VolumeSlider;

        public static QMToggleButton SelfbotToggle;

        private static float XPosSlider = 0;
        private static float YPosSlider = 0;
        private static float ZPosSlider = 0;

        private static float XRotSlider = 0;
        private static float YRotSlider = 0;
        private static float ZRotSlider = 0;

        private static float DistanceSlider = 0;
        private static float SpeedSlider = 0;
        private static float MusicSlider = 0;

        public static void Init()
        {
            QMNestedButton ThisMenu = new(MainMenu.ClientMenu, 4, 0, "Bots", "Bot Menu", "Bot Options", QMButtonAPI.ButtonSize.Default, GeneralWrappers.GetSprite("Bots"));

            QMNestedButton AudioMenu = new(ThisMenu, 1.5f, -0.1f, "Audio", "Bot Audio Menu", "Bot Audio Options", QMButtonAPI.ButtonSize.Half);
            QMNestedButton ExploitMenu = new(ThisMenu, 2.5f, -0.1f, "Exploits", "Bot Exploit Menu", "Bot Exploit Options", QMButtonAPI.ButtonSize.Half);
            QMNestedButton MotionMenu = new(ThisMenu, 3.5f, -0.1f, "Motion", "Bot Motion Menu", "Bot Motion Options", QMButtonAPI.ButtonSize.Half);

            new QMToggleButton(ThisMenu, 2.5f, 0.7f, "Start", delegate
            {
                Utils.VRCUiPopupManager.NumberInput("Bot Amount", "", InputField.InputType.Standard, true, "Ok", (s, k, t) =>
                {
                    if (Websocket.Server.WSServer == null) Websocket.Server.Initialize();
                    Process.Start("LunaR\\Photon\\LunaRBots.exe", s);
                }, null);
            }, delegate
            {
                Websocket.Server.SendMessage($"Shutdown");
            }, "Start/Stop the Photonbot");

            new QMToggleButton(ThisMenu, 1, 1.75f, "Follow \nWorld", delegate
            {
                BotFollow = true;
            }, delegate
            {
                BotFollow = false;
            }, "Follow the Bot in your World");

            new QMSingleButton(ThisMenu, 2, 1.7125f, "Check \nWorld", delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("Text", "Ok", delegate (string text)
                {
                    if (text.StartsWith("wrld_")) ServerRequester.SendCheckInstance(text);
                });
            }, "Check the InstanceID", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ThisMenu, 2, 2.2125f, "Join ID\nInvis", delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("World ID", "Ok", delegate (string text)
                {
                    if (text.StartsWith("wrld_")) Websocket.Server.SendMessage($"JoinRoom/{text}/1");
                });
            }, "Join the Bot the WorldID Invisible", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ThisMenu, 3, 1.7125f, "Join", delegate
            {
                Websocket.Server.SendMessage($"JoinRoom/{GeneralWrappers.GetWorldID()}/0");
            }, "Join the Bot in your World", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ThisMenu, 3, 2.2125f, "Join \nID", delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("World ID", "Ok", delegate (string text)
                {
                    if (text.StartsWith("wrld_")) Websocket.Server.SendMessage($"JoinRoom/{text}/0");
                });
            }, "Join the Bot the WorldID", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ThisMenu, 4, 1.7125f, "Join \nInvis", delegate
            {
                Websocket.Server.SendMessage($"JoinRoom/{GeneralWrappers.GetWorldID()}/1");
            }, "Join the Bot in your World Invisible", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ThisMenu, 4, 2.2125f, "Leave", delegate
            {
                Websocket.Server.SendMessage($"LeaveRoom");
            }, "Leave the World", null, QMButtonAPI.ButtonSize.Half);


            SelfbotToggle = new QMToggleButton(ThisMenu, 1, 2.75f, "Self \nBot", delegate
            {
                if (FakeSerialize.BotID == null)
                {
                    Utils.VRCUiPopupManager.AskInGameInput("User ID", "Ok", delegate (string text)
                    {
                        if (text.StartsWith("usr_"))
                        {
                            FakeSerialize.BotID = text;
                            FakeSerialize.FakeBotSerialize(true).Start();
                        }
                    });
                }
                else FakeSerialize.FakeBotSerialize(true).Start();
            }, delegate
            {
                FakeSerialize.FakeBotSerialize(false).Start();
            }, "Control the Bot");

            new QMSingleButton(ThisMenu, 3, 2.7125f, "Change \nAvatar", delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("Avatar ID", "Ok", delegate (string text)
                {
                    if (text.StartsWith("avtr_")) Websocket.Server.SendMessage($"ChangeAvatar/{text}");
                });
            }, "Change the Bots Avatar", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ThisMenu, 3, 3.2125f, "Change \nStatus", delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("Text", "Ok", delegate (string text)
                {
                    Websocket.Server.SendMessage($"ChangeStatus/{text}");
                });
            }, "Change the Bots Statu", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ThisMenu, 2, 2.7125f, "Friend \nID", delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("User ID", "Ok", delegate (string text)
                {
                    if (text.StartsWith("usr_")) Websocket.Server.SendMessage($"Friend/{text}");
                });
            }, "Friend the UserID", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ThisMenu, 2, 3.2125f, "Change \nBio", delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("Text", "Ok", delegate (string text)
                {
                    Websocket.Server.SendMessage($"ChangeBio/{text}");
                });
            }, "Change the Bots Bio", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ExploitMenu, 1, 0, "RPC Crash \nMaster", delegate
            {
                Websocket.Server.SendMessage($"HandleExploit/RPC/true");
            }, "Crash the Master", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ExploitMenu, 2, 0, "Earrape", delegate
            {
                Websocket.Server.SendMessage($"HandleExploit/USpeak");
            }, "Play Earrape Sounds", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ExploitMenu, 4, 0, "RPC \nCrash", delegate
            {
                Websocket.Server.SendMessage($"HandleExploit/RPC/false");
            }, "Crash the World", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ExploitMenu, 2, 0.5f, "Ownership \nCrash", delegate
            {
                Websocket.Server.SendMessage($"HandleExploit/Ownership/false");
            }, "Crash the World", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ExploitMenu, 3, 0, "Desync \nWorld", delegate
            {
                Websocket.Server.SendMessage($"HandleExploit/Cache/false");
            }, "Disconnect the World", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ExploitMenu, 1, 0.5f, "Desync \nMaster", delegate
            {
                Websocket.Server.SendMessage($"HandleExploit/Cache/true");
            }, "Disconnect the Master", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(AudioMenu, 1, 0, "Play \nYoutube", delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("URL", "Ok", delegate (string text)
                {
                    if (text.Contains("watch?v=")) text = text.Split('=', '&')[1];
                    Websocket.Server.SendMessage($"HandleAudio/PlayYoutube/{text}");
                });
            }, "Play Audio from Youtube", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(AudioMenu, 1, 0.5f, "Stop \nAudio", delegate
            {
                Websocket.Server.SendMessage($"HandleAudio/StopAudio");
            }, "Stop the Audio", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(AudioMenu, 2, 0, "Text to \nSpeech", delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("Text", "Ok", delegate (string text)
                {
                    Websocket.Server.SendMessage($"HandleAudio/TextToVoice/{text}");
                });
            }, "Play Audio from Text", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(AudioMenu, 3, 0, "Reset \nVolume", delegate
            {
                VolumeSlider.Slide(0.5f, false);
                Websocket.Server.SendMessage($"HandleAudio/ChangeVolume/0.5");
            }, "Reset the Audio Volume", null, QMButtonAPI.ButtonSize.Half);

            QMScrollMenu AudioScroll = new(AudioMenu, 4, 0, "Audio \nFiles", null, "Show Audio Files", "Audio Files", QMButtonAPI.ButtonSize.Half);
            AudioScroll.SetAction(delegate
            {
                foreach (string Dir in Directory.GetDirectories("LunaR\\Photon\\Music"))
                {
                    QMScrollMenu DirScroll = new(AudioScroll.BaseMenu, 0, 0, "Audio \nFiles", null, "Show Audio Files", "Audio Files", QMButtonAPI.ButtonSize.Half);
                    DirScroll.BaseMenu.GetMainButton().GetGameObject().transform.localScale = Vector3.zero;

                    DirScroll.SetAction(delegate
                    {
                        foreach (string AudioFile in Directory.GetFiles(Dir))
                        {
                            string Audio = AudioFile.Replace(Dir + "\\", "").Split('.')[0];
                            DirScroll.Add(Audio, "Play the Audio", delegate
                            {
                                Websocket.Server.SendMessage($"HandleAudio/PlayAudio/{AudioFile}");
                            });
                        }
                    });

                    AudioScroll.Add(Dir.Replace("LunaR\\Photon\\Music\\", ""), "Open the Subfolder", delegate
                    {
                        DirScroll.BaseMenu.GetMainButton().ClickMe();
                    });
                }

                foreach (string AudioFile in Directory.GetFiles("LunaR\\Photon\\Music"))
                {
                    string Audio = AudioFile.Replace("LunaR\\Photon\\Music\\", "").Split('.')[0];
                    AudioScroll.Add(Audio, "Play the Audio", delegate
                    {
                        Websocket.Server.SendMessage($"HandleAudio/PlayAudio/{AudioFile}");
                    });
                }
            });

            VolumeSlider = new QMSlider(AudioMenu, 3, 0.5f, "Volume", "Change the Audio Volume", delegate (float f)
            {
                f = (float)(int)(f * 1000) / 1000;
                if (MusicSlider != f)
                {
                    MusicSlider = f;
                    Websocket.Server.SendMessage($"HandleAudio/ChangeVolume/{f}");
                }
            }, 0.5f, 0.1f, 1, QMButtonAPI.SliderSize.Double);


            XSlider = new QMSlider(MotionMenu, 1, 0, "X Position", "Change the Bot Offset", delegate (float f)
            {
                f = (float)(int)(f * 100) / 100;
                if (XPosSlider != f)
                {
                    XPosSlider = f;
                    Websocket.Server.SendMessage($"ChangePosition/X/{f}");
                }
            }, 0, -2.25f, 2.25f);

            YSlider = new QMSlider(MotionMenu, 2, 0, "Y Position", "Change the Bot Offset", delegate (float f)
            {
                f = (float)(int)(f * 100) / 100;
                if (YPosSlider != f)
                {
                    YPosSlider = f;
                    Websocket.Server.SendMessage($"ChangePosition/Y/{f}");
                }
            }, 0, -2.25f, 2.25f);

            ZSlider = new QMSlider(MotionMenu, 3, 0, "Z Position", "Change the Bot Offset", delegate (float f)
            {
                f = (float)(int)(f * 100) / 100;
                if (ZPosSlider != f)
                {
                    ZPosSlider = f;
                    Websocket.Server.SendMessage($"ChangePosition/Z/{f}");
                }
            }, 0, -2.25f, 2.25f);

            OSpeedSlider = new QMSlider(MotionMenu, 4, 0, "Speed", "Change the Bot Orbit Speed", delegate (float f)
            {
                f = (float)(int)(f * 100) / 100;
                if (SpeedSlider != f)
                {
                    SpeedSlider = f;
                    Websocket.Server.SendMessage($"ChangeCopySettings/Speed/{f}");
                }
            }, 0.25f, 0, 2);

            XRSlider = new QMSlider(MotionMenu, 1, 1, "X Rotation", "Change the Bot Offset", delegate (float f)
            {
                f = (float)(int)(f * 100) / 100;
                if (XRotSlider != f)
                {
                    XRotSlider = f;
                    Websocket.Server.SendMessage($"ChangeRotation/X/{f}");
                }
            }, 0, 0, 7.1f);

            YRSlider = new QMSlider(MotionMenu, 2, 1, "Y Rotation", "Change the Bot Offset", delegate (float f)
            {
                f = (float)(int)(f * 100) / 100;
                if (YRotSlider != f)
                {
                    YRotSlider = f;
                    Websocket.Server.SendMessage($"ChangeRotation/Y/{f}");
                }
            }, 0, 0, 7.1f);

            ZRSlider = new QMSlider(MotionMenu, 3, 1, "Z Rotation", "Change the Bot Offset", delegate (float f)
            {
                f = (float)(int)(f * 100) / 100;
                if (ZRotSlider != f)
                {
                    ZRotSlider = f;
                    Websocket.Server.SendMessage($"ChangeRotation/Z/{f}");
                }
            }, 0, 0, 7.1f);

            ODistanceSlider = new QMSlider(MotionMenu, 4, 1, "Distance", "Change the Bot Orbit Distance", delegate (float f)
            {
                f = (float)(int)(f * 100) / 100;
                if (DistanceSlider != f)
                {
                    DistanceSlider = f;
                    Websocket.Server.SendMessage($"ChangeCopySettings/Distance/{f}");
                }
            }, 0, 0, 5);

            new QMToggleButton(MotionMenu, 1, 2, "Freeze \nPosition", delegate
            {
                Websocket.Server.SendMessage($"ChangeCopySettings/FreezePosition/true");
            }, delegate
            {
                Websocket.Server.SendMessage($"ChangeCopySettings/FreezePosition/false");
            }, "Freeze the Bot Position");

            new QMToggleButton(MotionMenu, 2, 2, "Freeze \nRotation", delegate
            {
                Websocket.Server.SendMessage($"ChangeCopySettings/FreezeRotation/true");
            }, delegate
            {
                Websocket.Server.SendMessage($"ChangeCopySettings/FreezeRotation/false");
            }, "Freeze the Bot Rotation");

            new QMToggleButton(MotionMenu, 3, 2, "Follow \nCamera", delegate
            {
                Websocket.Server.SendMessage($"ChangeCopySettings/FollowCamera/true");
            }, delegate
            {
                Websocket.Server.SendMessage($"ChangeCopySettings/FollowCamera/false");
            }, "Follow the Camera");

            new QMToggleButton(MotionMenu, 4, 2, "Loop \nReplay", delegate
            {
                Websocket.Server.SendMessage($"LoopAnimations/true");
            }, delegate
            {
                Websocket.Server.SendMessage($"LoopAnimations/false");
            }, "Loop the Motion");

            new QMSingleButton(MotionMenu, 1, 3.1f, "Reset \nPosition", delegate
            {
                XSlider.Slide(0, false);
                YSlider.Slide(0, false);
                ZSlider.Slide(0, false);
                Websocket.Server.SendMessage($"ChangePosition/Reset");
            }, "Reset the Offset", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(MotionMenu, 2, 3.1f, "Reset \nRotation", delegate
            {
                XRSlider.Slide(0, false);
                YRSlider.Slide(0, false);
                ZRSlider.Slide(0, false);
                Websocket.Server.SendMessage($"ChangeRotation/Reset");
            }, "Reset the Offset", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(MotionMenu, 3, 3.1f, "Reset \nSpeed", delegate
            {
                OSpeedSlider.Slide(0.25f, false);
                Websocket.Server.SendMessage($"ChangeCopySettings/Speed/0.25");
            }, "Reset the Offset", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(MotionMenu, 3, 3.6f, "Reset \nDistance", delegate
            {
                ODistanceSlider.Slide(0, false);
                Websocket.Server.SendMessage($"ChangeCopySettings/Distance/0");
            }, "Reset the Offset", null, QMButtonAPI.ButtonSize.Half);

            new QMToggleButton(MotionMenu, 4, 3.1f, "AFK \nOverride", delegate
            {
                Websocket.Server.SendMessage($"ChangeCopySettings/OverrideAFK/true");
            }, delegate
            {
                Websocket.Server.SendMessage($"ChangeCopySettings/OverrideAFK/false");
            }, "Set the AFK Mode");

            new QMSingleButton(MotionMenu, 1, 3.6f, "Attach \nActor", delegate
            {
                Utils.VRCUiPopupManager.NumberInput("Actor Number", "", InputField.InputType.Standard, true, "Ok", (s, k, t) =>
                {
                    Websocket.Server.SendMessage($"CopyEvent/7/{s}");
                }, null);
            }, "Attach to Actor Number", null, QMButtonAPI.ButtonSize.Half);

            QMScrollMenu MotionScroll = new(MotionMenu, 2, 3.6f, "Motion \nFiles", null, "Show Motion Files", "Motion Files", QMButtonAPI.ButtonSize.Half);
            MotionScroll.SetAction(delegate
            {
                MotionScroll.Add("Stop \nReplay", "Stop the Replay", delegate
                {
                    Websocket.Server.SendMessage($"HandleReplay/false");
                });

                foreach (string Dir in Directory.GetDirectories("LunaR\\Photon\\MotionRecords"))
                {
                    QMScrollMenu DirScroll = new(MotionScroll.BaseMenu, 0, 0, "Motion \nFiles", null, "Show Motion Files", "Motion Files", QMButtonAPI.ButtonSize.Half);
                    DirScroll.BaseMenu.GetMainButton().GetGameObject().transform.localScale = Vector3.zero;

                    DirScroll.SetAction(delegate
                    {
                        foreach (string MotionFile in Directory.GetFiles(Dir))
                        {
                            string Audio = MotionFile.Replace(Dir + "\\", "").Split('.')[0];
                            DirScroll.Add(Audio, "Play the Motion", delegate
                            {
                                Websocket.Server.SendMessage($"HandleReplay/true/{MotionFile}");
                            });
                        }
                    });

                    MotionScroll.Add(Dir.Replace("LunaR\\Photon\\MotionRecords\\", ""), "Open the Subfolder", delegate
                    {
                        DirScroll.BaseMenu.GetMainButton().ClickMe();
                    });
                }

                foreach (string MotionFile in Directory.GetFiles("LunaR\\Photon\\MotionRecords"))
                {
                    string Motion = MotionFile.Replace("LunaR\\Photon\\MotionRecords\\", "").Split('.')[0];
                    MotionScroll.Add(Motion, "Play the Motion", delegate
                    {
                        Websocket.Server.SendMessage($"HandleReplay/true/{MotionFile}");
                    });
                }
            });
        }
    }
}