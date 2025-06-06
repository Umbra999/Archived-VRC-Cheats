using Hexed.Extensions;
using Hexed.Modules.Standalone;
using Hexed.UIApi;
using Hexed.Wrappers;
using System.Text.Json;
using VRC.Udon;

namespace Hexed.CustomUI.SelectedUserMenu
{
    internal class UtilsMenu
    {
        private static QMMenuPage UtilsPage;
        public static void Init()
        {
            UtilsPage = new("Player Utils");

            QMSingleButton OpenMenu = new(MainMenu.ClientPage, 1, 0, "Utils", UtilsPage.OpenMe, "Utils Options", ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Utils"));

            new QMSingleButton(UtilsPage, 1, 0, "Teleport", delegate
            {
                GameHelper.CurrentPlayer.transform.position = PlayerSimplifier.GetSelectedPlayer().transform.position;
            }, "Teleport to the Player", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(UtilsPage, 1, 0.5f, "Copy \nUserID", delegate
            {
                GeneralUtils.CopyToClipboard(PlayerSimplifier.GetSelectedPlayer().UserID());
            }, "Copy the UserID", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(UtilsPage, 2, 0, "Dump \nProps", delegate
            {
                Logger.Log(JsonSerializer.Serialize(CPP2IL.TypeSerializer.ILToManaged(PlayerSimplifier.GetSelectedPlayer().GetPhotonPlayer().GetProperties()), new JsonSerializerOptions() { WriteIndented = true }), Logger.LogsType.Info);
            }, "Dump the Player properties", ButtonAPI.ButtonSize.Half);

            QMScrollButton UdonScroll = new(UtilsPage, 2, 0.5f, "Udon", "Udon Options", ButtonAPI.ButtonSize.Half);
            QMScrollButton UdonInteract = new(UtilsPage, 2, 2, "Udon Interact", "Udon Interact Options");
            UdonInteract.MainButton.GetGameObject().SetActive(false);

            UdonScroll.SetAction(delegate
            {
                UdonScroll.Add("Send \nCustom", "Send Custom Event", delegate
                {
                    GameHelper.VRCUiPopupManager.AskInGameInput("Custom Event", "Ok", delegate (string text)
                    {
                        foreach (UdonBehaviour Udon in UnityEngine.Object.FindObjectsOfType<UdonBehaviour>())
                        {
                            PhotonHelper.SendUdonRPC(Udon, text, PlayerSimplifier.GetSelectedPlayer());
                        }
                    });
                });

                foreach (UdonBehaviour Udon in UnityEngine.Object.FindObjectsOfType<UdonBehaviour>())
                {
                    UdonScroll.Add(Udon.name, "Open Event Menu", delegate
                    {
                        UdonInteract.SetAction(delegate
                        {
                            foreach (string UdonEvent in Udon._eventTable.Keys)
                            {
                                UdonInteract.Add(UdonEvent, "Trigger Event", delegate
                                {
                                    PhotonHelper.SendUdonRPC(Udon, UdonEvent, PlayerSimplifier.GetSelectedPlayer());
                                });
                            }
                        });
                        UdonInteract.MainButton.ClickMe();
                    });
                }
            });

            new QMSingleButton(UtilsPage, 3, 0, "Copy \nActorID", delegate
            {
                GeneralUtils.CopyToClipboard(PlayerSimplifier.GetSelectedPlayer().GetPhotonPlayer().ActorID().ToString());
            }, "Copy the ActorID", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(UtilsPage, 3, 0.5f, "Copy \nName", delegate
            {
                GeneralUtils.CopyToClipboard(PlayerSimplifier.GetSelectedPlayer().DisplayName());
            }, "Copy the Name", ButtonAPI.ButtonSize.Half);

            new QMToggleButton(UtilsPage, 4, 0, "Always \nHear", delegate
            {
                ExploitHandler.ListenPlayer(PlayerSimplifier.GetSelectedPlayer(), true);
            }, delegate
            {
                ExploitHandler.ListenPlayer(PlayerSimplifier.GetSelectedPlayer(), false);
            }, "Always listen to the Player");
        }
    }
}
