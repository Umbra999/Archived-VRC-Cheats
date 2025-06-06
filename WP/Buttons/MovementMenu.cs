using WengaPort.Api;
using UnityEngine;
using WengaPort.Modules;

namespace WengaPort.Buttons
{
    class MovementMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMToggleButton RotateToggle;

        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(MainMenu.ThisMenu, 3.5f, 0f, "Movement", "Movement Menu", null, null, null, Color.yellow);
            ThisMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            new QMToggleButton(ThisMenu, 1, 0, "Speed", () =>
            {
                Movement.IncreaseSpeed();
            }, "Disabled", () =>
            {
                Movement.DecreaseSpeed();
            }, "Toggle Speed");

            new QMToggleButton(ThisMenu, 2, 0, "Inf \nJump", () =>
            {
                Movement.InfJump = true;
            }, "Disabled", () =>
            {
                Movement.InfJump = false;
            }, "Jump infinite High", Color.cyan, Color.white, false, true);

            new QMToggleButton(ThisMenu, 3, 0, "Double \nJump", () =>
            {
                Movement.DoubleJump = true;
            }, "Disabled", () =>
            {
                Movement.DoubleJump = false;
            }, "Jump in the Air");

            RotateToggle = new QMToggleButton(ThisMenu, 4, 0, "Rotate", () =>
            {
                Movement.ToggleRotate(true);
            }, "Disabled", () =>
            {
                Movement.ToggleRotate(false);
            }, "Rotate with Arrow Keys");
        }
    }
}
