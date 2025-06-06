using LunaR.Api;
using LunaR.Extensions;
using LunaR.Modules;
using LunaR.Wrappers;

namespace LunaR.QMButtons
{
    internal class MovementMenu
    {
        public static QMToggleButton RotationToggle;

        public static void Init()
        {
            QMNestedButton ThisMenu = new(MainMenu.ClientMenu, 3, 0, "Motion", "Motion Menu", "Motion Options", QMButtonAPI.ButtonSize.Default, GeneralWrappers.GetSprite("Movement"));

            new QMToggleButton(ThisMenu, 1, 0, "Speed", delegate
            {
                Movement.ToggleSpeed(true);
            }, delegate
            {
                Movement.ToggleSpeed(false);
            }, "Increase your Speed");

            new QMToggleButton(ThisMenu, 2, 0, "Inf \nJump", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "InfJump", true);
                Movement.InfJump = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "InfJump", false);
                Movement.InfJump = false;
            }, "Jump without Cooldown", ConfigManager.Ini.GetBool("Toggles", "InfJump"));

            new QMToggleButton(ThisMenu, 3, 0, "Double \nJump", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "DoubleJump", true);
                Movement.DoubleJump = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "DoubleJump", false);
                Movement.DoubleJump = false;
            }, "Jump in the Air", ConfigManager.Ini.GetBool("Toggles", "DoubleJump"));

            RotationToggle = new QMToggleButton(ThisMenu, 1, 1, "Rotation", delegate
            {
                Movement.ToggleRotate(true);
            }, delegate
            {
                Movement.ToggleRotate(false);
            }, "Rotate with Arrow Keys");

            new QMToggleButton(ThisMenu, 2, 1, "Jump \nBoost", delegate
            {
                Movement.ToggleJump(true);
            }, delegate
            {
                Movement.ToggleJump(false);
            }, "Jump higher");

            new QMToggleButton(ThisMenu, 4, 0, "Bunny \nJump", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "BunnyJump", true);
                Movement.BunnyHop = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "BunnyJump", false);
                Movement.BunnyHop = false;
            }, "Jump on Ground", ConfigManager.Ini.GetBool("Toggles", "BunnyJump"));

            new QMSlider(ThisMenu, 1, 2.25f, "Fly Speed", "Change the Fly Speed", delegate (float f)
            {
                Movement.FlySpeed = f;
            }, 4.2f, 0.1f, 10, QMButtonAPI.SliderSize.Double);

            new QMSlider(ThisMenu, 3, 2.25f, "Walk Speed", "Change the Walk Speed", delegate (float f)
            {
                Movement.WalkSpeed = f;
            }, 4, 0.1f, 20, QMButtonAPI.SliderSize.Double);

            new QMSlider(ThisMenu, 1, 3.25f, "Rotation Speed", "Change the Rotation Speed", delegate (float f)
            {
                Movement.RotateSpeed = f;
            }, 180f, 50, 400f, QMButtonAPI.SliderSize.Double);

            new QMSlider(ThisMenu, 3, 3.25f, "Jump Boost", "Change the Jump Boost", delegate (float f)
            {
                Movement.JumpBoost = f;
            }, 4, 0.1f, 20, QMButtonAPI.SliderSize.Double);

        }
    }
}