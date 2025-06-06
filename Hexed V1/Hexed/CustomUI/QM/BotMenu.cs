using Hexed.Modules.Standalone;
using Hexed.UIApi;
using Hexed.Wrappers;

namespace Hexed.CustomUI.QM
{
    internal class BotMenu
    {
        public static QMMenuPage BotsPage;
        public static QMToggleButton BotSerializeToggle;

        public static void Init()
        {
            BotsPage = new("Bots");

            QMSingleButton OpenMenu = new(MainMenu.ClientPage, 4, 0, "Bots", BotsPage.OpenMe, "Bot Options", ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Bots"));

            new QMToggleButton(BotsPage, 1, 0, "Start \nHost", BotConnection.StartBot, BotConnection.StopBot, "Start/Stop the Photonbot Server");

            new QMSingleButton(BotsPage, 2, 0, "Change \nAvatar", delegate
            {
                GameHelper.VRCUiPopupManager.AskInGameInput("AvatarID", "Ok", delegate (string text)
                {
                    if (text.StartsWith("avtr_")) BotConnection.ChangeAvatar(text);
                }, "avtr_");

            }, "Change the Bots Avatar", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(BotsPage, 2, 0.5f, "Change \nFallback", delegate
            {
                GameHelper.VRCUiPopupManager.AskInGameInput("AvatarID", "Ok", delegate (string text)
                {
                    if (text.StartsWith("avtr_")) BotConnection.ChangeFallbackAvatar(text);
                }, "avtr_");

            }, "Change the Bots fallback Avatar", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(BotsPage, 3, 0, "Change \nStatus", delegate
            {
                GameHelper.VRCUiPopupManager.AskInGameInput("Text", "Ok", delegate (string text)
                {
                    BotConnection.ChangeStatus(text);
                });
            }, "Change the Bots Status", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(BotsPage, 3, 0.5f, "Change \nBio", delegate
            {
                GameHelper.VRCUiPopupManager.AskInGameInput("Text", "Ok", delegate (string text)
                {
                    BotConnection.ChangeBio(text);
                });
            }, "Change the Bots Bio", ButtonAPI.ButtonSize.Half);

            BotSerializeToggle = new QMToggleButton(BotsPage, 4, 0, "Self \nBot", delegate
            {
                FakeSerialize.CustomSerialize(true);
                BotConnection.SelfbotExpose();
                GeneralUtils.DelayAction(1, delegate
                {
                    VRC.Player Bot = GameHelper.PlayerManager.GetPlayer(BotConnection.LatestResponse);
                    if (Bot != null) FakeSerialize.AddBotSerialize(Bot);
                }).Start();
            }, delegate
            {
                FakeSerialize.CustomSerialize(false);
            }, "Control the Bot");

            new QMSingleButton(BotsPage, 1.5f, 3.5f, "Join", delegate
            {
                BotConnection.JoinRoom(GameUtils.GetCurrentWorldID());
            }, "Connect the bot to your Room", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(BotsPage, 2.5f, 3.5f, "Join \nID", delegate
            {
                GameHelper.VRCUiPopupManager.AskInGameInput("World ID", "Ok", delegate (string text)
                {
                    if (text.StartsWith("wrld_")) BotConnection.JoinRoom(text);
                }, "wrld_");
            }, "Connect the bot to a Room", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(BotsPage, 3.5f, 3.5f, "Leave", BotConnection.LeaveRoom, "Disconnect the Bot from the Room", ButtonAPI.ButtonSize.Half);

            BotMenus.AudioMenu.Init();
            BotMenus.PhysicsMenu.Init();
            BotMenus.ExploitMenu.Init();
            BotMenus.MiscMenu.Init();
        }
    }
}
