using Hexed.UIApi;
using Hexed.Wrappers;

namespace Hexed.CustomUI.QM
{
    internal class MainMenu
    {
        public static QMMenuPage ClientPage;

        public static void Init()
        {
            ClientPage = new("Hexed");

            new QMIconButton(MenuHelper.menuPageTemplate.transform, 0.75f, -0.80f, ClientPage.OpenMe, "Hexed Client", UnityUtils.GetSprite("moon"));

            new QMIconButton(ClientPage, 1.5f, 3.25f, delegate
            {
                NativeMethods.keybd_event(177, 0, 1U, IntPtr.Zero);
            }, "Previous Media", UnityUtils.GetSprite("MediaPrevious"));

            new QMIconButton(ClientPage, 2f, 3.25f, delegate
            {
                NativeMethods.keybd_event(174, 0, 1U, IntPtr.Zero);
            }, "Volume Down Media", UnityUtils.GetSprite("MediaDown"));

            new QMIconButton(ClientPage, 2.5f, 3.25f, delegate
            {
                NativeMethods.keybd_event(179, 0, 1U, IntPtr.Zero);
            }, "Play / Pause Media", UnityUtils.GetSprite("MediaPlay"));

            new QMIconButton(ClientPage, 3f, 3.25f, delegate
            {
                NativeMethods.keybd_event(175, 0, 1U, IntPtr.Zero);
            }, "Volume Up Media", UnityUtils.GetSprite("MediaUp"));

            new QMIconButton(ClientPage, 3.5f, 3.25f, delegate
            {
                NativeMethods.keybd_event(176, 0, 1U, IntPtr.Zero);
            }, "Next Media", UnityUtils.GetSprite("MediaNext"));

            SafetyMenu.Init();
            UtilsMenu.Init();
            PhysicsMenu.Init();
            BotMenu.Init();
            ExploitMenu.Init();
            DebugMenu.Init();
            VisualsMenu.Init();
            SpoofMenu.Init();
            UdonMenu.Init();

            SelectedUserMenu.MainMenu.Init();

            //CustomUI.MainMenu.WorldMenu.Init();
            CustomUI.MainMenu.AvatarMenu.Init();
            CustomUI.MainMenu.UserMenu.Init();
        }
    }
}
