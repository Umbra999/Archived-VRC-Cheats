using Hexed.UIApi;
using Hexed.Wrappers;
using VRC.Core;

namespace Hexed.CustomUI.SelectedUserMenu
{
    internal class AvatarMenu
    {
        private static QMMenuPage AvatarPage;
        public static void Init()
        {
            AvatarPage = new("Player Avatar");

            QMSingleButton OpenMenu = new(MainMenu.ClientPage, 2, 0, "Avatar", AvatarPage.OpenMe, "Avatar Options", ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Avatar"));

            new QMSingleButton(AvatarPage, 1, 0, "Reload \nAvatar", delegate
            {
                PlayerSimplifier.GetSelectedPlayer().GetAPIUser().ReloadAvatar();
            }, "Reload the Avatar", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(AvatarPage, 1, 0.5f, "Force \nClone", delegate
            {
                VRC.Player SelectedPlayer = PlayerSimplifier.GetSelectedPlayer();
                ApiAvatar Avatar = SelectedPlayer.GetAvatar();

                if (Avatar.releaseStatus == "public" || Avatar.releaseStatus == "hidden") PlayerUtils.ChangeAvatar(Avatar.id);
            }, "Clone the Main Avatar", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(AvatarPage, 2, 0, "Dump \nMain", delegate
            {
                ApiAvatar avatar = PlayerSimplifier.GetSelectedPlayer().GetAvatar();

                //ApiFile.DownloadFile(avatar.assetUrl, new Action<Il2CppStructArray<byte>>((avatarFile) =>
                //{
                //    File.WriteAllBytes($"{Core.ConfigManager.BaseFolder}\\Rips\\{avatar.name}.vrca", avatarFile);
                //    Wrappers.Logger.Log($"Downloaded Avatar {avatar.name}", Wrappers.Logger.LogsType.Info);
                //}), new Action<string>((error) =>
                //{
                //    Wrappers.Logger.LogError($"Failed to download Main Avatar");
                //}), null);
            }, "Dump the Main Avatar file", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(AvatarPage, 2, 0.5f, "Dump \nFallback", delegate
            {
                ApiAvatar avatar = PlayerSimplifier.GetSelectedPlayer().GetFallbackAvatar();

                //ApiFile.DownloadFile(avatar.assetUrl, new Action<Il2CppStructArray<byte>>((avatarFile) =>
                //{
                //    File.WriteAllBytes($"{Core.ConfigManager.BaseFolder}\\Rips\\{avatar.name}.vrca", avatarFile);
                //    Wrappers.Logger.Log($"Downloaded Avatar {avatar.name}", Wrappers.Logger.LogsType.Info);
                //}), new Action<string>((error) =>
                //{
                //    Wrappers.Logger.LogError($"Failed to download Fallback Avatar");
                //}), null);
            }, "Dump the Fallback Avatar file", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(AvatarPage, 3, 0, "Copy Main \nAssetURL", delegate
            {
                GeneralUtils.CopyToClipboard(PlayerSimplifier.GetSelectedPlayer().GetAvatar().assetUrl);
            }, "Copy the Main Asset URL", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(AvatarPage, 3, 0.5f, "Copy Fallback \nAssetURL", delegate
            {
                GeneralUtils.CopyToClipboard(PlayerSimplifier.GetSelectedPlayer().GetFallbackAvatar().assetUrl);
            }, "Copy the Fallback Asset URL", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(AvatarPage, 4, 0, "Copy Main \nAvatarID", delegate
            {
                GeneralUtils.CopyToClipboard(PlayerSimplifier.GetSelectedPlayer().GetAvatar().id);
            }, "Copy the Main AvatarID", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(AvatarPage, 4, 0.5f, "Copy Fallback \nAvatarID", delegate
            {
                GeneralUtils.CopyToClipboard(PlayerSimplifier.GetSelectedPlayer().GetFallbackAvatar().id);
            }, "Copy the Main AvatarID", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(AvatarPage, 1, 1, "Copy Main \nImageURL", delegate
            {
                GeneralUtils.CopyToClipboard(PlayerSimplifier.GetSelectedPlayer().GetAvatar().imageUrl);
            }, "Copy the Main Image URL", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(AvatarPage, 1, 1.5f, "Copy Fallback \nImageURL", delegate
            {
                GeneralUtils.CopyToClipboard(PlayerSimplifier.GetSelectedPlayer().GetFallbackAvatar().imageUrl);
            }, "Copy the Fallback Image URL", ButtonAPI.ButtonSize.Half);
        }
    }
}
