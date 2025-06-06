using UnityEngine;
using MelonLoader;
using WengaPort.Api;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using Logger = WengaPort.Extensions.Logger;
using WengaPort.Modules;

namespace WengaPort.Buttons
{
    class PhotonMenu
    {
        public static QMNestedButton ThisMenu;

        public static IniFile Ini = new IniFile("WengaPort\\Photon\\PhotonConfig.ini");
        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(MainMenu.ThisMenu, 1.5f, 2f, "Photon", "PhotonBot Utils", null, null, null, Color.yellow);
            ThisMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            Ini.SetBool("Toggles", "Bot", false);

            new QMToggleButton(ThisMenu, 1, 0, "Bot", () =>
            {
                Ini.SetBool("Toggles", "Bot", true);
                Ini.SetBool("Toggles", "Exit", false);
                Ini.SetBool("Toggles", "DesyncMaster", false);
                Ini.SetBool("Toggles", "Desync", false);
                Ini.SetBool("Toggles", "JoinMe", false);
                Ini.SetBool("Toggles", "WhoInInstance", false);
                Ini.SetBool("Toggles", "JoinClipboard", false);
                if (File.Exists("WengaPort\\Photon\\MultiBot.exe"))
                {
                    Process.Start("WengaPort\\Photon\\MultiBot.exe");
                }
                else
                {
                    Logger.WengaLogger("No Bot-Extension Found");
                    Ini.SetBool("Toggles", "Bot", false);
                }
            }, "Disabled", () =>
            {
                Ini.SetBool("Toggles", "Exit", true);
                Ini.SetBool("Toggles", "Bot", false);
            }, "Toggle the Bot state");

            new QMSingleButton(ThisMenu, 2, 0, "Join \nMe", () =>
            {
                Ini.SetBool("Toggles", "JoinMe", true);
            }, "Let the Bot join your World");

            new QMSingleButton(ThisMenu, 3, 0, "Join \nClipboard", () =>
            {
                string ID = Clipboard.GetText();
                if (ID.Contains("wrld_"))
                {
                    Ini.SetString("Toggles", "ClipboardWorld", ID);
                    Ini.SetBool("Toggles", "JoinClipboard", true);
                }
            }, "Let the Bot join the WorldID in your Clipboard");

            new QMSingleButton(ThisMenu, 4, 0, "Desync", () =>
            {
                Ini.SetBool("Toggles", "Desync", true);
                PhotonModule.EmojiRPC(29);
            }, "Desync the World");

            new QMSingleButton(ThisMenu, 4, 1, "Desync \nMaster", () =>
            {
                Ini.SetBool("Toggles", "DesyncMaster", true);
                PhotonModule.EmojiRPC(29);
            }, "Desync the Masterclient");
        }
    }
}
