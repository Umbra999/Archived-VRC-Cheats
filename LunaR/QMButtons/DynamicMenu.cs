using LunaR.Api;
using LunaR.Extensions;
using LunaR.Modules;
using LunaR.Wrappers;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using static VRC.SDKBase.VRC_EventHandler;

namespace LunaR.QMButtons
{
    internal class DynamicMenu
    {
        private static GameObject SelectedPrefab;

        public static void Init()
        {
            QMNestedButton PrefabInteractPage = new(MainMenu.ClientMenu, 0, 0, "Prefab Interact", "Prefab Interact Menu", "Prefab Options");
            PrefabInteractPage.GetMainButton().SetActive(false);

            new QMSingleButton(PrefabInteractPage, 2, 0, "Spawn \nLocal", delegate
            {
                GameObject Item = Networking.Instantiate(VrcBroadcastType.Local, SelectedPrefab.name, Utils.CurrentUser.transform.position + Utils.CurrentUser.transform.forward * 2f, Utils.CurrentUser.transform.rotation);
                if (Item != null) ItemHandler.SpawnedPrefabs.Add(Item);
            }, "Spawn the Prefab Local", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(PrefabInteractPage, 3, 0, "Spawn \nGlobal", delegate
            {
                GameObject Item = Networking.Instantiate(VrcBroadcastType.Always, SelectedPrefab.name, Utils.CurrentUser.transform.position + Utils.CurrentUser.transform.forward * 2f, Utils.CurrentUser.transform.rotation);
                if (Item != null) ItemHandler.SpawnedPrefabs.Add(Item);
            }, "Spawn the Prefab Global", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(PrefabInteractPage, 1, 0.5f, "Spawn \n+ Infinity", delegate
            {
                GameObject Item = Networking.Instantiate(VrcBroadcastType.Always, SelectedPrefab.name, PortalHandler.Position.INFBypass, Utils.CurrentUser.transform.rotation);
                if (Item != null)
                {
                    ItemHandler.SpawnedPrefabs.Add(Item);
                    if (Item.GetComponent<BoxCollider>() != null) Item.GetComponent<BoxCollider>().size = Vector3.positiveInfinity;
                    if (Item.GetComponent<CapsuleCollider>() != null) Item.GetComponent<CapsuleCollider>().center = Vector3.positiveInfinity;
                    Item.SetActive(false);
                }
            }, "Spawn the Prefab at Positive Infinity", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(PrefabInteractPage, 2, 0.5f, "Spawn \n- Infinity", delegate
            {
                GameObject Item = Networking.Instantiate(VrcBroadcastType.Always, SelectedPrefab.name, PortalHandler.Position.NegINFBypass, Utils.CurrentUser.transform.rotation);
                if (Item != null)
                {
                    ItemHandler.SpawnedPrefabs.Add(Item);
                    if (Item.GetComponent<BoxCollider>() != null) Item.GetComponent<BoxCollider>().size = Vector3.negativeInfinity;
                    if (Item.GetComponent<CapsuleCollider>() != null) Item.GetComponent<CapsuleCollider>().center = Vector3.negativeInfinity;
                    Item.SetActive(false);
                }
            }, "Spawn the Prefab at Negative Infinity", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(PrefabInteractPage, 3, 0.5f, "Spawn \n+ Rotation", delegate
            {
                GameObject Item = Networking.Instantiate(VrcBroadcastType.Always, SelectedPrefab.name, Utils.CurrentUser.transform.position + Utils.CurrentUser.transform.forward * 2f, PortalHandler.Position.ROTBypass);
                if (Item != null)
                {
                    ItemHandler.SpawnedPrefabs.Add(Item);
                    Item.SetActive(false);
                }
            }, "Spawn the Prefab at Infinity Rotation", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(PrefabInteractPage, 4, 0.5f, "Spawn \n- Rotation", delegate
            {
                GameObject Item = Networking.Instantiate(VrcBroadcastType.Always, SelectedPrefab.name, Utils.CurrentUser.transform.position + Utils.CurrentUser.transform.forward * 2f, PortalHandler.Position.NegROTBypass);
                if (Item != null)
                {
                    ItemHandler.SpawnedPrefabs.Add(Item);
                    Item.SetActive(false);
                }
            }, "Spawn the Prefab at Infinity Rotation", null, QMButtonAPI.ButtonSize.Half);

            QMScrollMenu PrefabScroll = new(MainMenu.ClientMenu, 1.5f, 2, "Prefab", null, "Prefab Menu", "Prefab Options", QMButtonAPI.ButtonSize.Default, GeneralWrappers.GetSprite("Options"));
            PrefabScroll.SetAction(delegate
            {
                PrefabScroll.Add("Delete \nPrefabs", "Delete all spawned Prefabs", delegate
                {
                    foreach (GameObject Item in ItemHandler.SpawnedPrefabs)
                    {
                        if (Item != null) Object.Destroy(Item);
                    }
                    ItemHandler.SpawnedPrefabs.Clear();
                });

                foreach (GameObject Prefab in GeneralWrappers.GetWorldPrefabs())
                {
                    if (Prefab != null)
                    {
                        PrefabScroll.Add(Prefab.name, "Spawn Prefab", delegate
                        {
                            SelectedPrefab = Prefab;
                            PrefabInteractPage.OpenMe();
                        });
                    }
                }
            });

            QMScrollMenu UdonScroll = new(MainMenu.ClientMenu, 2.5f, 2, "Udon", null, "Udon Menu", "Udon Options", QMButtonAPI.ButtonSize.Default, GeneralWrappers.GetSprite("Options"));
            QMScrollMenu UdonInteract = new(MainMenu.ClientMenu, 2, 2, "Udon Interact", null, "Udon Interact Menu", "Udon Interact Options");
            UdonInteract.BaseMenu.GetMainButton().SetActive(false);

            UdonScroll.SetAction(delegate
            {
                UdonScroll.Add("Send \nCustom", "Send Custom Event", delegate
                {
                    Utils.VRCUiPopupManager.AskInGameInput("Custom Event", "Ok", delegate (string text)
                    {
                        foreach (UdonBehaviour Udon in Object.FindObjectsOfType<UdonBehaviour>())
                        {
                            RPCHandler.SendUdonRPC(Udon.gameObject, text);
                        }
                    });
                });

                foreach (UdonBehaviour Udon in Object.FindObjectsOfType<UdonBehaviour>())
                {
                    UdonScroll.Add(Udon.name, "Open Event Menu", delegate
                    {
                        UdonInteract.SetAction(delegate
                        {
                            foreach (string UdonEvent in Udon._eventTable.Keys)
                            {
                                UdonInteract.Add(UdonEvent, "Trigger Event", delegate
                                {
                                    RPCHandler.SendUdonRPC(Udon.gameObject, UdonEvent);
                                });
                            }
                        });
                        UdonInteract.BaseMenu.GetMainButton().ClickMe();
                    });
                }
            });

            QMScrollMenu TriggerScroll = new(MainMenu.ClientMenu, 3.5f, 2, "Trigger", null, "Trigger Menu", "Trigger Options", QMButtonAPI.ButtonSize.Default, GeneralWrappers.GetSprite("Options"));
            TriggerScroll.SetAction(delegate
            {
                foreach (VRC_Trigger Trigger in Object.FindObjectsOfType<VRC_Trigger>())
                {
                    TriggerScroll.Add(Trigger.name, "Interact", delegate
                    {
                        Trigger.Interact();
                    });
                }
            });
        }
    }
}