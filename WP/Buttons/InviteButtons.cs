using System;
using UnityEngine;
using WengaPort.Api;
using WengaPort.Modules;
using WengaPort.Extensions;
using System.Windows.Forms;

namespace WengaPort.Buttons
{
    class InviteButtons
    {
        public static QMSingleButton InviteButton;
        public static Transform NotificationMenu = Utils.QuickMenu.transform.Find("QuickModeMenus/QuickModeInviteResponseMoreOptionsMenu");

        public static QMSingleButton NotificationButton;
        public static Transform NotificationPage = Utils.QuickMenu.transform.Find("QuickModeMenus/QuickModeNotificationsMenu");

        public static void InviteButtonInit()
        {
            NotificationButton = new QMSingleButton("ShortcutMenu", 4, 1, "⇄", delegate
            {
                UnityEngine.UI.Button NotifTab = GameObject.Find("/UserInterface/QuickMenu/QuickModeTabs/HomeTab").GetComponent<UnityEngine.UI.Button>();
                NotifTab.Press();
            }, "Go to the Home Menu");
            NotificationButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 1.75f);
            NotificationButton.getGameObject().transform.localPosition = UIChanges.GetButtonPosition(-0.25f, 2.53f);
            NotificationButton.SetParent(NotificationPage);

            InviteButton = new QMSingleButton("ShortcutMenu", 4, 0, "Drop\nPortal", new Action(() =>
            {
                string ID = string.Empty;
                Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>.Enumerator enumerator = Utils.QuickMenu.Notification().details.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string a = enumerator.current.Key;
                    if (a.ToLower() == "worldid")
                    {
                        Il2CppSystem.Object b = enumerator.current.value;
                        ID = b.ToString();
                    }
                }
                PortalHandler.DropPortal(ID);
            }), "Drop a Portal to the Invite");
            InviteButton.SetParent(NotificationMenu);

            InviteButton = new QMSingleButton("ShortcutMenu", 2, 0, "Check\nInstance", new Action(() =>
            {
                if (PhotonMenu.Ini.GetBool("Toggles", "Bot", true))
                {
                    string ID = string.Empty;
                    Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>.Enumerator enumerator = Utils.QuickMenu.Notification().details.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        string a = enumerator.current.Key;
                        if (a.ToLower() == "worldid")
                        {
                            Il2CppSystem.Object b = enumerator.current.value;
                            ID = b.ToString();
                        }
                    }
                    PhotonMenu.Ini.SetString("Toggles", "SocialInstance", ID);
                    PhotonMenu.Ini.SetBool("Toggles", "WhoInInstance", true);
                }
                else
                {
                    Extensions.Logger.WengaLogger("Start the Photonbots before using this feature");
                }
            }), "Check the Instance for Players");
            InviteButton.SetParent(NotificationMenu);

            InviteButton = new QMSingleButton("ShortcutMenu", 3, 0, "Copy\nWorldID", new Action(() =>
            {
                string ID = string.Empty;
                Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>.Enumerator enumerator = Utils.QuickMenu.Notification().details.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string a = enumerator.current.Key;
                    if (a.ToLower() == "worldid")
                    {
                        Il2CppSystem.Object b = enumerator.current.value;
                        ID = b.ToString();
                    }
                }
                Clipboard.SetText(ID);
            }), "Check the Instance for Players");
            InviteButton.SetParent(NotificationMenu);
        }
    }
}
