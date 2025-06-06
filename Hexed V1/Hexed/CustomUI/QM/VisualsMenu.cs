using Hexed.Core;
using Hexed.UIApi;
using Hexed.Wrappers;

namespace Hexed.CustomUI.QM
{
    internal class VisualsMenu
    {
        private static QMMenuPage VisualsPage;

        public static void Init()
        {
            VisualsPage = new("Visuals");

            QMSingleButton OpenMenu = new(MainMenu.ClientPage, 3.5f, 1, "Visuals", VisualsPage.OpenMe, "Visual Options", ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Visuals"));

            new QMToggleButton(VisualsPage, 1, 0, "Player \nESP", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "PlayerESP", true);
                InternalSettings.PlayerESP = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "PlayerESP", false);
                InternalSettings.PlayerESP = false;
            }, "Highlight Player Objects", ConfigManager.Ini.GetBool("Toggles", "PlayerESP"));

            new QMToggleButton(VisualsPage, 2, 0, "Pickup \nESP", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "ItemESP", true);
                InternalSettings.ItemESP = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "ItemESP", false);
                InternalSettings.ItemESP = false;
            }, "Highlight pickupable Objects", ConfigManager.Ini.GetBool("Toggles", "ItemESP"));

            new QMToggleButton(VisualsPage, 3, 0, "Trigger \nESP", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "TriggerESP", true);
                InternalSettings.TriggerESP = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "TriggerESP", false);
                InternalSettings.TriggerESP = false;
            }, "Highlight triggerable Objects", ConfigManager.Ini.GetBool("Toggles", "TriggerESP"));
        }
    }
}
