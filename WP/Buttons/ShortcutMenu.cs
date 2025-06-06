using UnityEngine;
using MelonLoader;
using WengaPort.Api;
using WengaPort.Wrappers;
using WengaPort.Modules;
using System;
using System.Runtime.InteropServices;
using System.Linq;
using VRC;
using System.Windows.Forms;
using VRC.SDKBase;
using WengaPort.Extensions;
using static VRC.SDKBase.VRC_EventHandler;
using VRC.Udon;
using Object = UnityEngine.Object;
using VRC.UI;

namespace WengaPort.Buttons
{
    class MainMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMSingleButton HalfButton;
        public static QMToggleButton FlyButton;
        public static QMToggleButton NoClipButton;
        public static QMSingleButton HalfToggleButton;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        public static void Initialize()
        {
            ThisMenu = new QMNestedButton("ShortcutMenu", 0, 0, "MainButton", "Main Menu", Color.cyan, Color.magenta, Color.black, Color.yellow);
            ThisMenu.getMainButton().setActive(false); //Hidding this button cause we gonna create a new one with a custom sprite on it -->
            MelonCoroutines.Start(ReworkedButtonAPI.CreateButton(MenuType.ShortCut, ButtonType.Single, "", 0, -1.5f, delegate ()
            {
                QMStuff.ShowQuickmenuPage(ThisMenu.getMenuName()); //Open the menu we created above ^

            }, "Open the Client", Color.black, Color.clear, null, "https://i.imgur.com/tfavyxX.png")); // 292 x 281 png


            Panels.TextMenu(ThisMenu, 2.5f, 1, "\nMenu\nSelector", 4, 3, "Menu Selector");

            QMNestedButton PrefabsPage = new QMNestedButton(ThisMenu, 2.5f, 2f, "World", "World Menu");
            QMNestedButton PrefabsSubPage = new QMNestedButton(PrefabsPage, 2, 1, "Prefab", "Prefab Menu");
            PrefabsPage.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            GameObject SelectedPrefab = null;
            QMNestedButton Options = new QMNestedButton(PrefabsPage, -1, -1, "", "Options about the prefabs");
            Options.getMainButton().setActive(false);

            QMNestedButton MaterialMenu = new QMNestedButton(PrefabsPage, 1, 1, "Material", "VRC Material Menu");
            ScrollMenu MaterialObjects = new ScrollMenu(MaterialMenu);
            MaterialObjects.SetAction(delegate
            {
                foreach (var Material in VRCUiManagerExtension.GetWorldMaterials())
                {
                    MaterialObjects.Add(new QMSingleButton(MaterialMenu, 0, 0, Material.name, delegate
                    {
                        Material.shader = Shader.Find("Diffuse");
                    }, $"{Material.name}"));
                }
            });

            new QMSingleButton(Options, 1, 0, "Instatiate\nMe\nGlobal", delegate
            {
                Networking.Instantiate(VrcBroadcastType.Always, SelectedPrefab.name, Utils.CurrentUser.transform.position + Utils.CurrentUser.transform.forward * 2f, Utils.CurrentUser.transform.rotation);
            }, "Instatiate Prefab at yourself Global");

            new QMSingleButton(Options, 2, 0, "Instatiate\nMe\nLocal", delegate
            {
                Networking.Instantiate(VrcBroadcastType.Local, SelectedPrefab.name, Utils.CurrentUser.transform.position + Utils.CurrentUser.transform.forward * 2f, Utils.CurrentUser.transform.rotation);
            }, "Instatiate Prefab at yourself Local");

            new QMSingleButton(Options, 3, 0, "Instatiate\nMax-Value\nGlobal", delegate
            {
                GameObject Item = Networking.Instantiate(VrcBroadcastType.Always, SelectedPrefab.name, new Vector3(int.MaxValue, int.MaxValue, int.MaxValue) * 268, Utils.CurrentUser.transform.rotation);
                Item.SetActive(false);
            }, "Instatiate Prefab at Positive-Infinity Global");

            new QMSingleButton(Options, 4, 0, "Instatiate\nMax-Value\nLocal", delegate
            {
                GameObject Item = Networking.Instantiate(VrcBroadcastType.Local, SelectedPrefab.name, new Vector3(int.MaxValue, int.MaxValue, int.MaxValue) * 268, Utils.CurrentUser.transform.rotation);
                Item.SetActive(false);
            }, "Instatiate Prefab at Positive-Infinity Local");

            new QMSingleButton(Options, 3, 1, "Instatiate\nMin-Value\nGlobal", delegate
            {
                GameObject Item = Networking.Instantiate(VrcBroadcastType.Always, SelectedPrefab.name, new Vector3(int.MinValue, int.MinValue, int.MinValue) * 268, Utils.CurrentUser.transform.rotation);
                Item.SetActive(false);
            }, "Instatiate Prefab at Negative-Infinity Global");

            new QMSingleButton(Options, 4, 1, "Instatiate\nMin-Value\nLocal", delegate
            {
                GameObject Item = Networking.Instantiate(VrcBroadcastType.Local, SelectedPrefab.name, new Vector3(int.MinValue, int.MinValue, int.MinValue) * 268, Utils.CurrentUser.transform.rotation);
                Item.SetActive(false);
            }, "Instatiate Prefab at Negative-Infinity Global");
            
            ScrollMenu PrefabsScroll = new ScrollMenu(PrefabsSubPage);
            PrefabsScroll.SetAction(delegate
            {
                foreach (var prefab in VRCUiManagerExtension.GetWorldPrefabs())
                {
                    PrefabsScroll.Add(new QMSingleButton(PrefabsSubPage, 0, 0, prefab.name, delegate
                    {
                        SelectedPrefab = prefab;
                        QMStuff.ShowQuickmenuPage(Options.getMenuName());
                    }, prefab.name));
                }
            });

            QMNestedButton UdonExploits = new QMNestedButton(PrefabsPage, 4, 1, "Udon", "Udon Trigger/Event Menu");
            QMNestedButton UdonTriggers = new QMNestedButton(UdonExploits, -5, -5, "", "");
            ScrollMenu UdonEvents = new ScrollMenu(UdonTriggers);
            ScrollMenu UdonObjects = new ScrollMenu(UdonExploits);
            UdonTriggers.getMainButton().setActive(false);
            UdonObjects.SetAction(delegate
            {
                foreach (var Udon in Object.FindObjectsOfType<UdonBehaviour>())
                {
                    UdonObjects.Add(new QMSingleButton(UdonExploits, 0, 0, Udon.gameObject.name, delegate
                    {
                        UdonEvents.SetAction(delegate
                        {
                            foreach (var UdonEvent in Udon._eventTable)
                            {
                                UdonEvents.Add(new QMSingleButton(UdonEvents.BaseMenu, 0, 0, UdonEvent.Key, delegate
                                {
                                    Udon.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, UdonEvent.Key);
                                }, Udon.gameObject + " Execute " + UdonEvent.Key));
                            }
                        });
                        UdonEvents.BaseMenu.getMainButton().getGameObject().GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
                    }, Udon.interactText));
                }
            });

            QMNestedButton TriggerMenu = new QMNestedButton(PrefabsPage, 3, 1, "Trigger", "VRC Trigger Menu");
            ScrollMenu TriggerObjects = new ScrollMenu(TriggerMenu);
            TriggerObjects.SetAction(delegate
            {
                foreach (var Trigger in Object.FindObjectsOfType<VRC_Trigger>())
                {
                    TriggerObjects.Add(new QMSingleButton(TriggerMenu, 0, 0, Trigger.gameObject.name, delegate
                    {
                       Trigger.Interact();
                    }, Trigger.interactText));
                }
            });

            new QMToggleButton(PrefabsPage, 1, 0, "Disable \nPostProcess", () =>
            {
                ItemHandler.PostProcessToggle = true;
                CameraHandler.DisablePostProcess(true);
            }, "Disabled", () =>
            {
                ItemHandler.PostProcessToggle = true;
                CameraHandler.DisablePostProcess(false);
            }, "Toggle Post Processing");

            new QMToggleButton(PrefabsPage, 2, 0, "Disable \nChairs", () =>
            {
                ItemHandler.ChairToggle = true;
            }, "Disabled", () =>
            {
                ItemHandler.ChairToggle = false;
            }, "Toggle Chairs", Color.cyan, Color.white, false, true);

            new QMToggleButton(PrefabsPage, 3, 0, "Disable \nMirror", () =>
            {
                ItemHandler.MirrorToggle = true;
                var objects = Resources.FindObjectsOfTypeAll<VRC_MirrorReflection>();
                foreach (var item in objects)
                {
                    if (item.gameObject.active)
                    {
                        item.gameObject.SetActive(false);
                        ItemHandler.World_Mirrors.Add(item);
                    }
                }
            }, "Disabled", () =>
            {
                ItemHandler.MirrorToggle = false;
                foreach (var item in ItemHandler.World_Mirrors)
                {
                    item.gameObject.SetActive(true);
                }
            }, "Toggle Mirrors");

            new QMToggleButton(PrefabsPage, 4, 0, "Disable \nItems", () =>
            {
                ItemHandler.ItemToggle = true;
                foreach (VRCSDK2.VRC_Pickup item in Object.FindObjectsOfType<VRCSDK2.VRC_Pickup>())
                {
                    ItemHandler.World_Pickups.Add(item);
                    item.gameObject.SetActive(false);
                }
            }, "Disabled", () =>
            {
                ItemHandler.ItemToggle = false;
                foreach (VRC_Pickup item in ItemHandler.World_Pickups)
                {
                    item.gameObject.SetActive(true);
                }
            }, "Toggle Items");

            new QMToggleButton(PrefabsPage, 1, 2, "Disable \nPedestals", () =>
            {
                CameraHandler.DisableAvatarPedestals(true);
            }, "Disabled", () =>
            {
                CameraHandler.DisableAvatarPedestals(false);
            }, "Toggle Avatar Pedestals");

            new QMToggleButton(PrefabsPage, 2, 2, "Nightmode", () =>
            {
                CameraHandler.NightMode(true);
            }, "Disabled", () =>
            {
                CameraHandler.NightMode(false);
            }, "Toggle Nightmode");

            new QMToggleButton(PrefabsPage, 3, 2, "Disable \nPanorama", () =>
            {
                ItemHandler.PanoramaToggle = true;
                foreach (VRC_Panorama panorama in Resources.FindObjectsOfTypeAll<VRC_Panorama>())
                {
                    if (panorama.enabled)
                    {
                        ItemHandler.World_Panorama.Add(panorama);
                        panorama.enabled = false;
                    }
                }
            }, "Disabled", () =>
            {
                ItemHandler.PanoramaToggle = false;
                foreach (var panorama in ItemHandler.World_Panorama)
                {
                    panorama.enabled = true;
                }
            }, "Toggle VRC_Panorama");

            new QMToggleButton(PrefabsPage, 4, 2, "World \nTrigger", () =>
            {
                PatchManager.WorldTrigger = true;
            }, "Disabled", () =>
            {
                PatchManager.WorldTrigger = false;
            }, "Make all Triggers Global");

            HalfButton = new QMSingleButton("ShortcutMenu", 5, 1.25f, "Delete \nPortals", () =>
            {
                PortalHandler.DeletePortals();
            }, "Delete all Portals");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton("ShortcutMenu", 5, 0.25f, "Copy \nWorldID", () =>
            {
                Clipboard.SetText($"{RoomManager.field_Internal_Static_ApiWorld_0.id}:{RoomManager.field_Internal_Static_ApiWorldInstance_0.idWithTags}");
                Extensions.Logger.WengaLogger("World ID copied to Clipboard");
            }, "Copy RoomID to Clipboard");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton("ShortcutMenu", 5, 0.75f, "Forcejoin", () =>
            {
                if (Clipboard.GetText().Contains("wrld_"))
                {
                    Networking.GoToRoom(Clipboard.GetText());
                }
            }, "Join World by Clipboard ID");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            FlyButton = new QMToggleButton("ShortcutMenu", 0, 0, "Fly", () =>
            {
                Movement.FlyEnable();
            }, "Disabled", () =>
            {
                Movement.FlyDisable();
            }, "Toggle Fly Mode");

            QMNestedButton TargetMenu = new QMNestedButton("ShortcutMenu", 5, -0.25f, "Players",
                "All current Players in the World");
            TargetMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            ScrollMenu menu = new ScrollMenu(TargetMenu);
            QMNestedButton SelectedMenu = new QMNestedButton(TargetMenu, 0, -1, "Recent", "Select the Recent Player",
               null, null, null, null);
            new QMSingleButton(TargetMenu, 0, 0, "Self Select", () =>
            {
                GeneralWrappers.GetQuickMenu().SelectPlayer(Utils.CurrentUser.prop_Player_0);
            }, "Select Yourself");
            Player SelectedPlayer = null;
            new QMSingleButton(SelectedMenu, 1, 0, "Select",delegate 
            {
                Utils.QuickMenu.SelectPlayer(SelectedPlayer); 
            },"Select the Player");

            new QMSingleButton(SelectedMenu, 2, 0, "Teleport", delegate
            {
                Utils.CurrentUser.transform.position = SelectedPlayer.transform.position;
            }, "Teleport to Player");
            menu.SetAction(delegate
            {
                foreach (Player player in Utils.PlayerManager.GetAllPlayers().ToArray())
                {
                    menu.Add(new QMSingleButton(menu.BaseMenu, 0, 0, player.DisplayName(), new Action(() =>
                    {
                        SelectedPlayer = player;
                        QMStuff.ShowQuickmenuPage(SelectedMenu.getMenuName());
                    }), "Select Player", null, player.GetAPIUser().GetRankColor()));
                }
            });

            NoClipButton = new QMToggleButton("ShortcutMenu", 0, 1, "NoClip", () =>
            {
                Movement.NoClipEnable();
            }, "Disabled", () =>
            {
                Movement.NoClipDisable();
            }, "Toggle NoClip");


            HalfToggleButton = new QMSingleButton("ShortcutMenu", 4, 1, "⇄", delegate
            {
                UnityEngine.UI.Button NotifTab = GameObject.Find("/UserInterface/QuickMenu/QuickModeTabs/NotificationsTab").GetComponent<UnityEngine.UI.Button>();
                NotifTab.Press();
            }, "Go to the Notification Menu");
            HalfToggleButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 1.75f); 
            HalfToggleButton.getGameObject().transform.localPosition = UIChanges.GetButtonPosition(-0.25f, 2.53f);
        }
    }
}
