using Hexed.HexedServer;
using Hexed.Modules.Standalone;
using Hexed.UIApi;
using Hexed.Wrappers;
using System.IO;

namespace Hexed.CustomUI.QM.BotMenus
{
    internal class AudioMenu
    {
        private static QMMenuPage AudioPage;
        public static void Init()
        {
            AudioPage = new("Bot Audio");

            QMSingleButton OpenMenu = new(BotMenu.BotsPage, 1, 1.5f, "Audio", AudioPage.OpenMe, "Bot Audio Options", ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Options"));

            QMScrollButton AudioScroll = new(AudioPage, 1, 0, "Audio \nFiles", "Audio Files", ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Options"));
            QMScrollButton DirScroll = new(AudioPage, 1, 0, "Audio Options", "Audio File Options");
            DirScroll.MainButton.GetGameObject().SetActive(false);

            AudioScroll.SetAction(delegate
            {
                foreach (string Dir in Directory.GetDirectories("Hexed\\Bots\\Audios"))
                {
                    string FolderName = Dir.Replace("Hexed\\Bots\\Audios\\", "");
                    AudioScroll.Add(FolderName, "Open the Subfolder", delegate
                    {
                        DirScroll.SetAction(delegate
                        {
                            foreach (string AudioFile in Directory.GetFiles($"Hexed\\Bots\\Audios\\{FolderName}"))
                            {
                                string AudioName = AudioFile.Replace($"Hexed\\Bots\\Audios\\{FolderName}\\", "").Split('.')[0];

                                DirScroll.Add(AudioName, "Play the Audio", delegate
                                {
                                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), AudioFile);
                                    BotConnection.PlayAudioFile(EncryptUtils.ToBase64(fullPath));
                                });
                            }
                        });
                        DirScroll.MainButton.ClickMe();
                    });
                }

                foreach (string AudioFile in Directory.GetFiles("Hexed\\Bots\\Audios"))
                {
                    string AudioName = AudioFile.Replace("Hexed\\Bots\\Audios\\", "").Split('.')[0];

                    AudioScroll.Add(AudioName, "Play the Audio", delegate
                    {
                        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), AudioFile);
                        BotConnection.PlayAudioFile(EncryptUtils.ToBase64(fullPath));
                    });
                }
            });

            new QMSingleButton(AudioPage, 2, 0, "TTS", delegate
            {
                GameHelper.VRCUiPopupManager.AskInGameInput("Text Message", "Ok", delegate (string text)
                {
                    BotConnection.PlayTextToVoice(text);
                });
            }, "Send a Text to Speech message", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(AudioPage, 2, 0.5f, "Youtube", delegate
            {
                GameHelper.VRCUiPopupManager.AskInGameInput("Youtube URL", "Ok", delegate (string text)
                {
                    BotConnection.PlayYoutube(text);
                });
            }, "Send a Text to Speech message", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(AudioPage, 3, 0, "Stop \nAudio", BotConnection.StopAudioReplay, "Stop the currently played Audio", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(AudioPage, 3.75f, 0, "+", delegate
            {
                BotConnection.ChangeAudioVolume(true);
            }, "Increase the Volume", ButtonAPI.ButtonSize.Square);

            new QMSingleButton(AudioPage, 4.25f, 0, "-", delegate
            {
                BotConnection.ChangeAudioVolume(false);
            }, "Decrease the Volume", ButtonAPI.ButtonSize.Square);

            new QMSingleButton(AudioPage, 4, 0.5f, "Volume", BotConnection.ResetAudioVolume, "Reset the Volume", ButtonAPI.ButtonSize.Half);
        }
    }
}
