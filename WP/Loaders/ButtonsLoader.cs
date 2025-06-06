using WengaPort.Buttons;

namespace WengaPort.Loaders
{
    class ButtonsLoader
    {
        //Load each menues we created
        public static void Initialize()
        {
            MainMenu.Initialize();
            UtilsMenu.Initialize();
            ExploitMenu.Initialize();
            InteractMenu.Initialize();
            AvatarMenu.Initialize();
            ConsoleMenu.Initialize();
            SafetyMenu.Initialize();
            SocialMenu.Initialize();
            MediaMenu.Initialize();
            MovementMenu.Initialize();
            PhotonMenu.Initialize();
            InviteButtons.InviteButtonInit();
            DebugMenu.Initialize();
        }
    }
}
