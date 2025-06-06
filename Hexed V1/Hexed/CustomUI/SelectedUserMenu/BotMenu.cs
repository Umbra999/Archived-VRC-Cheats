using Hexed.Modules.Standalone;
using Hexed.UIApi;
using Hexed.Wrappers;

namespace Hexed.CustomUI.SelectedUserMenu
{
    internal class BotMenu
    {
        private static QMMenuPage BotPage;
        public static void Init()
        {
            BotPage = new("Player Bots");

            QMSingleButton OpenMenu = new(MainMenu.ClientPage, 4, 0, "Bots", BotPage.OpenMe, "Bot Options", ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Bots"));

            new QMToggleButton(BotPage, 1, 0, "Repeat \nMovement", delegate
            {
                BotConnection.MimicMovement(PlayerSimplifier.GetSelectedPlayer().GetPhotonPlayer().ActorID());
            }, delegate
            {
                BotConnection.MimicMovement(-2);
            }, "Copy the Players Movement Events");

            new QMToggleButton(BotPage, 2, 0, "Repeat \nAvatarSync", delegate
            {
                BotConnection.MimicAvatarSync(PlayerSimplifier.GetSelectedPlayer().GetPhotonPlayer().ActorID());
            }, delegate
            {
                BotConnection.MimicAvatarSync(-2);
            }, "Copy the Players Avatar Sync Events");

            new QMToggleButton(BotPage, 3, 0, "Repeat \nVoice", delegate
            {
                BotConnection.MimicVoice(PlayerSimplifier.GetSelectedPlayer().GetPhotonPlayer().ActorID());
            }, delegate
            {
                BotConnection.MimicVoice(-2);
            }, "Copy the Players Voice Events");

            new QMToggleButton(BotPage, 4, 0, "Repeat \nRPC", delegate
            {
                BotConnection.MimicRPC(PlayerSimplifier.GetSelectedPlayer().GetPhotonPlayer().ActorID());
            }, delegate
            {
                BotConnection.MimicRPC(-2);
            }, "Copy the Players RPC Events");

            new QMSingleButton(BotPage, 1, 1, "Block", delegate
            {
                BotConnection.Block(PlayerSimplifier.GetSelectedPlayer().UserID(), true);
            }, "Block the Player", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(BotPage, 1, 1.5f, "Unblock", delegate
            {
                BotConnection.Block(PlayerSimplifier.GetSelectedPlayer().UserID(), false);
            }, "Unblock the Player", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(BotPage, 2, 1, "Mute", delegate
            {
                BotConnection.Mute(PlayerSimplifier.GetSelectedPlayer().UserID(), true);
            }, "Mute the Player", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(BotPage, 2, 1.5f, "Unmute", delegate
            {
                BotConnection.Mute(PlayerSimplifier.GetSelectedPlayer().UserID(), false);
            }, "Unmute the Player", ButtonAPI.ButtonSize.Half);

            new QMToggleButton(BotPage, 3, 1, "Repeat \nChat", delegate
            {
                BotConnection.MimicChat(PlayerSimplifier.GetSelectedPlayer().GetPhotonPlayer().ActorID());
            }, delegate
            {
                BotConnection.MimicChat(-2);
            }, "Copy the Players Chat Events");

            new QMToggleButton(BotPage, 4, 1, "Record \nMovement", delegate
            {
                GameHelper.VRCUiPopupManager.AskInGameInput("File Name", "Ok", delegate (string text)
                {
                    BotConnection.StartRecordMotion(PlayerSimplifier.GetSelectedPlayer().GetPhotonPlayer().ActorID(), text);
                });
            }, BotConnection.StopRecordMotion, "Record the Players Movement Events");

            new QMSingleButton(BotPage, 1, 2, "Add \nFriend", delegate
            {
                BotConnection.AddFriend(PlayerSimplifier.GetSelectedPlayer().UserID());
            }, "Add the Player as Friend", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(BotPage, 2, 2, "Forcekick", delegate
            {
                BotConnection.Forcekick(PlayerSimplifier.GetSelectedPlayer().UserID());
            }, "Votekick the Player", ButtonAPI.ButtonSize.Half);
        }
    }
}
