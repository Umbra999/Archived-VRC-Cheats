using Hexed.Core;
using Hexed.Modules;
using Hexed.UIApi;
using Hexed.Wrappers;

namespace Hexed.CustomUI.QM
{
    internal class PhysicsMenu
    {
        private static QMMenuPage PhysicsPage;
        public static QMToggleButton FlyToggle;
        public static QMToggleButton NoClipToggle;
        public static QMToggleButton RotateToggle;

        public static void Init()
        {
            PhysicsPage = new("Physics");

            QMSingleButton OpenMenu = new(MainMenu.ClientPage, 3, 0, "Physics", PhysicsPage.OpenMe, "Physics Options", ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Physics"));

            new QMToggleButton(PhysicsPage, 1, 0, "Bunny \nHop", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "BunnyHop", true);
                InternalSettings.BunnyHop = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "BunnyHop", false);
                InternalSettings.BunnyHop = false;
            }, "Automatically Jump", ConfigManager.Ini.GetBool("Toggles", "BunnyHop"));

            new QMToggleButton(PhysicsPage, 2, 0, "Inf \nJump", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "InfJump", true);
                InternalSettings.InfJump = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "InfJump", false);
                InternalSettings.InfJump = false;
            }, "Jump without Cooldown", ConfigManager.Ini.GetBool("Toggles", "InfJump"));

            new QMToggleButton(PhysicsPage, 3, 0, "Multi \nJump", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "MultiJump", true);
                InternalSettings.MultiJump = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "MultiJump", false);
                InternalSettings.MultiJump = false;
            }, "Jump in the Air", ConfigManager.Ini.GetBool("Toggles", "MultiJump"));

            new QMToggleButton(PhysicsPage, 4, 0, "High \nJump", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "HighJump", true);
                Movement.ToggleHighJump();
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "HighJump", false);
                Movement.ToggleHighJump();
            }, "Jump very High", ConfigManager.Ini.GetBool("Toggles", "HighJump"));

            new QMToggleButton(PhysicsPage, 1, 1, "Speed", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "Speed", true);
                Movement.ToggleSpeed();
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "Speed", false);
                Movement.ToggleSpeed();
            }, "Increase run Speed", ConfigManager.Ini.GetBool("Toggles", "Speed"));

            FlyToggle = new QMToggleButton(PhysicsPage, 2, 1, "Fly", Movement.ToggleFly, Movement.ToggleFly, "Toggle Fly mode");

            NoClipToggle = new QMToggleButton(PhysicsPage, 3, 1, "No \nClip", Movement.ToggleNoClip, Movement.ToggleNoClip, "Toggle NoClip mode");

            RotateToggle = new QMToggleButton(PhysicsPage, 4, 1, "Desktop \nRotate", Movement.ToggleRotate, Movement.ToggleRotate, "Toggle Rotation mode");
        }
    }
}
