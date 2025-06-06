using LunaR.Api;
using LunaR.Buttons.Bots;
using LunaR.Extensions;
using LunaR.Modules;
using LunaR.Patching;
using LunaR.UIApi;
using LunaR.Wrappers;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;
using VRC.SDKBase;
using VRC.Udon;
using static VRC.SDKBase.VRC_EventHandler;

namespace LunaR.QMButtons
{
    internal class TargetMenu
    {
        public static UIMenuText PlayerInfos;

        public static QMToggleButton HideObjectToggle;
        public static QMToggleButton RPCBlockToggle;
        public static QMToggleButton AttachToggle;

        public static QMToggleButton PortalKOSToggle;

        public static QMToggleButton ShaderToggle;
        public static QMToggleButton ParticleToggle;
        public static QMToggleButton AudioToggle;
        public static QMToggleButton ColliderToggle;

        public static void Init()
        {
            GameObject parent = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/Wing_Left/Button/Icon");
            PlayerInfos = new UIMenuText(parent, "Player Details", new Vector2(-590, 300f), 32, false, TextAnchor.UpperLeft);
            PlayerInfos.text.color = Color.cyan;
            PlayerInfos.gameObject.SetActive(false);

            APIStuff.GetQuickMenuInstance().field_Private_UIPage_1.transform.Find("ScrollRect").GetComponent<ScrollRect>().content.gameObject.AddComponent<EnableDisableListener>().OnEnableEvent += () =>
            {
                PlayerInfos.gameObject.SetActive(true);
            };
            APIStuff.GetQuickMenuInstance().field_Private_UIPage_1.transform.Find("ScrollRect").GetComponent<ScrollRect>().content.gameObject.AddComponent<EnableDisableListener>().OnDisableEvent += () =>
            {
                PlayerInfos.SetText("");
                PlayerInfos.gameObject.SetActive(false);
            };

            QMNestedButton UtilsMenu = new("Menu_SelectedUser_Local", 1, 1, "Player Utils", "Player Utils Menu", "Player Utils");
            UtilsMenu.GetMainButton().SetActive(false);
            new QMIconButton("Menu_SelectedUser_Local", 1.16f, -0.8f, delegate
            {
                if (PortalHandler.HiddenPlayers.Contains(PlayerWrappers.GetSelectedAPIUser().UserID())) HideObjectToggle.SetToggleState(true, false);
                else HideObjectToggle.SetToggleState(false, false);

                if (EventValidation.RPCBlockedUsers.Contains(PlayerWrappers.GetSelectedAPIUser().UserID())) RPCBlockToggle.SetToggleState(true, false);
                else RPCBlockToggle.SetToggleState(false, false);

                UtilsMenu.OpenMe();
            }, "Open Player Utils", GeneralWrappers.GetSprite("Utils"));

            QMNestedButton AvatarMenu = new("Menu_SelectedUser_Local", 1, 1, "Player Avatar", "Player Avatar Menu", "Player Avatar");
            AvatarMenu.GetMainButton().SetActive(false);
            new QMIconButton("Menu_SelectedUser_Local", 1.61f, -0.8f, delegate
            {
                if (AvatarAdjustment.DisabledShaders.Contains(PlayerWrappers.GetSelectedAPIUser().UserID())) ShaderToggle.SetToggleState(true, false);
                else ShaderToggle.SetToggleState(false, false);

                if (AvatarAdjustment.DisabledParticles.Contains(PlayerWrappers.GetSelectedAPIUser().UserID())) ParticleToggle.SetToggleState(true, false);
                else ParticleToggle.SetToggleState(false, false);

                if (AvatarAdjustment.DisabledAudios.Contains(PlayerWrappers.GetSelectedAPIUser().UserID())) AudioToggle.SetToggleState(true, false);
                else AudioToggle.SetToggleState(false, false);

                if (AvatarAdjustment.DisabledColliders.Contains(PlayerWrappers.GetSelectedAPIUser().UserID())) ColliderToggle.SetToggleState(true, false);
                else ColliderToggle.SetToggleState(false, false);

                AvatarMenu.OpenMe();
            }, "Open Player Avatars", GeneralWrappers.GetSprite("Avatar"));

            QMNestedButton ExploitMenu = new("Menu_SelectedUser_Local", 1, 1, "Player Exploits", "Player Exploit Menu", "Player Exploits");
            ExploitMenu.GetMainButton().SetActive(false);
            new QMIconButton("Menu_SelectedUser_Local", 2.06f, -0.8f, delegate
            {
                if (PortalHandler.PortalTargets.Contains(PlayerWrappers.GetSelectedAPIUser().UserID())) PortalKOSToggle.SetToggleState(true, false);
                else PortalKOSToggle.SetToggleState(false, false);

                ExploitMenu.OpenMe();
            }, "Open Player Exploits", GeneralWrappers.GetSprite("Exploits"));

            QMNestedButton BotMenu = new("Menu_SelectedUser_Local", 1, 1, "Player Bots", "Player Bot Menu", "Player Bots");
            BotMenu.GetMainButton().SetActive(false);
            new QMIconButton("Menu_SelectedUser_Local", 2.51f, -0.8f, delegate
            {
                BotMenu.OpenMe();
            }, "Open Player Bots", GeneralWrappers.GetSprite("Bots"));

            QMNestedButton SafetyMenu = new("Menu_SelectedUser_Local", 1, 1, "Player Safety", "Player Safety Menu", "Player Safety");
            SafetyMenu.GetMainButton().SetActive(false);
            new QMIconButton("Menu_SelectedUser_Local", 2.95f, -0.8f, delegate
            {
                SafetyMenu.OpenMe();
            }, "Open Player Bots", GeneralWrappers.GetSprite("Safety"));

            new QMSingleButton(UtilsMenu, 1, 0, "Open \nVRChat", delegate
            {
                Process.Start("https://vrchat.com/home/user/" + PlayerWrappers.GetSelectedAPIUser().UserID());
            }, "Open the VRChat Page", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(UtilsMenu, 2, 0, "Open \nSteam", delegate
            {
                APIUser.FetchUser(PlayerWrappers.GetSelectedAPIUser().UserID(), new Action<APIUser>(user =>
                {
                    if (user.username.Contains("steam_"))
                    {
                        string[] array = user.username.Split('_');
                        Process.Start($"https://steamcommunity.com/profiles/{array[1]}");
                    }
                }), null);
            }, "Open the Steam Page", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(UtilsMenu, 3, 0, "Dump \nProps", delegate
            {
                Extensions.Logger.Log(JsonConvert.SerializeObject( IL2CPPSerializer.IL2CPPToManaged.Serialize(PlayerWrappers.GetSelectedPlayer().prop_Player_1.GetHashtable()), Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), Extensions.Logger.LogsType.Clean);
            }, "Dump the Photon Props", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(UtilsMenu, 4, 0, "Teleport \nItems", delegate
            {
                ItemHandler.ItemsToPlayer(PlayerWrappers.GetSelectedPlayer().GetVRCPlayer());
            }, "Teleport all Items to the Player", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(UtilsMenu, 1, 0.5f, "Draw \nCircles", delegate
            {
                ItemHandler.DrawCircle(PlayerWrappers.GetSelectedPlayer()).Start();
            }, "Draw Circles with Pens", null, QMButtonAPI.ButtonSize.Half);

            QMScrollMenu UdonScroll = new(UtilsMenu, 2, 0.5f, "Udon", null, "Target Udon Events", "Target Udon Options", QMButtonAPI.ButtonSize.Half);
            QMScrollMenu UdonInteract = new(UtilsMenu, 2, 0.5f, "Udon Interact", null, "Udon Interact Menu", "Udon Interact Options");
            UdonInteract.BaseMenu.GetMainButton().SetActive(false);

            UdonScroll.SetAction(delegate
            {
                UdonScroll.Add("Send \nCustom", "Send Custom Event", delegate
                {
                    Utils.VRCUiPopupManager.AskInGameInput("Custom Event", "Ok", delegate (string text)
                    {
                        foreach (UdonBehaviour Udon in UnityEngine.Object.FindObjectsOfType<UdonBehaviour>())
                        {
                            RPCHandler.SendUdonRPC(Udon.gameObject, text, PlayerWrappers.GetSelectedPlayer());
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
                                    RPCHandler.SendUdonRPC(Udon.gameObject, UdonEvent, PlayerWrappers.GetSelectedPlayer());
                                });
                            }
                        });
                        UdonInteract.BaseMenu.GetMainButton().ClickMe();
                    });
                }
            });

            new QMSingleButton(UtilsMenu, 3, 0.5f, "Spawn \nPrefabs", delegate
            {
                foreach (GameObject SelectedPrefab in GeneralWrappers.GetWorldPrefabs())
                {
                    Networking.Instantiate(VrcBroadcastType.Always, SelectedPrefab.name, PlayerWrappers.GetSelectedPlayer().transform.position, PlayerWrappers.GetSelectedPlayer().transform.rotation);
                    ItemHandler.SpawnedPrefabs.Add(SelectedPrefab);
                }
            }, "Spawn Prefabs on the Player", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(UtilsMenu, 4, 0.5f, "Write \nPens", delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("Message", "Ok", delegate (string text)
                {
                    foreach (VRC_Pickup Pickup in UnityEngine.Object.FindObjectsOfType<VRC_Pickup>())
                    {
                        if (!Pickup.transform.parent.name.ToLower().Contains("eraser") && (Pickup.name.ToLower().Contains("pen") || Pickup.name.ToLower().Contains("marker") || Pickup.name.ToLower().Contains("grip")))
                        {
                            PenWriter.DrawWord(Pickup, PlayerWrappers.GetSelectedPlayer().transform.position, text).Start();
                            break;
                        }
                    }
                });
            }, "Write a Text in front of the Player", null, QMButtonAPI.ButtonSize.Half);

            AttachToggle = new QMToggleButton(UtilsMenu, 1, 1, "Attach", delegate
            {
                Movement.NoClipEnable();
                Movement.AttachToPlayer(PlayerWrappers.GetSelectedPlayer().GetVRCPlayer(), HumanBodyBones.Head).Start();
            }, delegate
            {
                Movement.Attachment = false; ;
            }, "Attach to the Player");

            new QMSingleButton(UtilsMenu, 2, 1, "Portal \nMessage", delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("Portal Message", "Ok", delegate (string text)
                {
                    PortalHandler.DropMessagePortal(PlayerWrappers.GetSelectedPlayer().GetVRCPlayer(), text);
                });
            }, "Spawn a Portal with a Message", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(UtilsMenu, 3, 1, "Teleport", delegate
            {
                Utils.CurrentUser.transform.position = PlayerWrappers.GetSelectedPlayer().transform.position;
            }, "Teleport to the Player", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(UtilsMenu, 4, 1, "Copy \nUserID", delegate
            {
                Clipboard.SetText(PlayerWrappers.GetSelectedAPIUser().UserID());
            }, "Copy the Players UserID", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(UtilsMenu, 2, 1.5f, "Drop \nPortal", delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("WorldID", "Ok", delegate (string text)
                {
                    PortalHandler.DropPortalToWorld(PlayerWrappers.GetSelectedPlayer().GetVRCPlayer(), text);
                });
            }, "Drop a Portal to a World", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(UtilsMenu, 3, 1.5f, "Log", delegate
            {
                APIUser.FetchUser(PlayerWrappers.GetSelectedPlayer().UserID(), new Action<APIUser>(User =>
                {
                    string Log = string.Concat(new object[]
                    {
                       Environment.NewLine,
                        $"======= Player Informations =======", Environment.NewLine,
                        $"Displayname: {User.DisplayName()}", Environment.NewLine,
                        $"Username: {User.username}", Environment.NewLine,
                        $"UserID: {User.UserID()}", Environment.NewLine,
                        $"Platform: {User.GetPlatform()}", Environment.NewLine,
                        $"Status: {User.status}", Environment.NewLine,
                        $"State: {User.state}", Environment.NewLine,
                        $"Last Login: {User.last_login}", Environment.NewLine,
                        $"Rank: {User.GetRank()}", Environment.NewLine,
                        $"Creation: {User.date_joined}", Environment.NewLine,
                        $"Avatar Copy: {User.allowAvatarCopying}", Environment.NewLine,
                        $"Current VRC+: {User.isSupporter}", Environment.NewLine,
                        $"Early VRC+: {User.isEarlyAdopter}", Environment.NewLine,
                        $"User Icon: {User.userIcon}", Environment.NewLine,
                        $"User Picture: {User.profilePicImageUrl}", Environment.NewLine,
                        $"Links: {string.Join(" ", User.bioLinks.ToArray())}", Environment.NewLine,
                        $"Languages: {string.Join(" ", User.languagesDisplayNames.ToArray())}", Environment.NewLine,
                        $"Tags: {string.Join(" ", User.tags.ToArray())}", Environment.NewLine,
                         "====================================",
                    });

                    Extensions.Logger.Log(Log, Extensions.Logger.LogsType.Clean);
                    Utils.VRCUiPopupManager.AlertNotification(User.DisplayName(), Log, "Copy", new Action(() =>
                    {
                        Clipboard.SetText(Log);
                        Utils.VRCUiPopupManager.HideCurrentPopUp();
                    }));

                }), null);
            }, "Log the Players Informations", null, QMButtonAPI.ButtonSize.Half);

            ShaderToggle = new QMToggleButton(AvatarMenu, 1, 0, "Disable \nShaders", delegate
            {
                AvatarAdjustment.DisabledShaders.Add(PlayerWrappers.GetSelectedAPIUser().UserID());
                PlayerExtensions.ReloadAvatar(PlayerWrappers.GetSelectedAPIUser());
            }, delegate
            {
                AvatarAdjustment.DisabledShaders.Remove(PlayerWrappers.GetSelectedAPIUser().UserID());
                PlayerExtensions.ReloadAvatar(PlayerWrappers.GetSelectedAPIUser());
            }, "Disable the Players Shaders");

            ParticleToggle = new QMToggleButton(AvatarMenu, 2, 0, "Disable \nParticles", delegate
            {
                AvatarAdjustment.DisabledParticles.Add(PlayerWrappers.GetSelectedAPIUser().UserID());
                PlayerExtensions.ReloadAvatar(PlayerWrappers.GetSelectedAPIUser());
            }, delegate
            {
                AvatarAdjustment.DisabledParticles.Remove(PlayerWrappers.GetSelectedAPIUser().UserID());
                PlayerExtensions.ReloadAvatar(PlayerWrappers.GetSelectedAPIUser());
            }, "Disable the Players Particles");

            AudioToggle = new QMToggleButton(AvatarMenu, 3, 0, "Disable \nAudio", delegate
            {
                AvatarAdjustment.DisabledAudios.Add(PlayerWrappers.GetSelectedAPIUser().UserID());
                PlayerExtensions.ReloadAvatar(PlayerWrappers.GetSelectedAPIUser());
            }, delegate 
            {
                AvatarAdjustment.DisabledAudios.Remove(PlayerWrappers.GetSelectedAPIUser().UserID());
                PlayerExtensions.ReloadAvatar(PlayerWrappers.GetSelectedAPIUser());
            }, "Disable the Players Audios");

            ColliderToggle = new QMToggleButton(AvatarMenu, 4, 0, "Disable \nCollider", delegate
            {
                AvatarAdjustment.DisabledColliders.Add(PlayerWrappers.GetSelectedAPIUser().UserID());
                PlayerExtensions.ReloadAvatar(PlayerWrappers.GetSelectedAPIUser());
            }, delegate
            {
                AvatarAdjustment.DisabledColliders.Remove(PlayerWrappers.GetSelectedAPIUser().UserID());
                PlayerExtensions.ReloadAvatar(PlayerWrappers.GetSelectedAPIUser());
            }, "Disable the Players Colliders");

            new QMSingleButton(AvatarMenu, 2, 1, "Favorite \nAvatar", delegate
            {
                if (!AvatarFavs.AvatarObjects.Exists(m => m.id == PlayerWrappers.GetSelectedPlayer().GetApiAvatar().id)) AvatarFavs.FavoriteAvatar(PlayerWrappers.GetSelectedPlayer().GetApiAvatar());
                else AvatarFavs.UnfavoriteAvatar(PlayerWrappers.GetSelectedPlayer().GetApiAvatar());
            }, "Add/Remove to Avatar to Favorites", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(AvatarMenu, 3, 1, "Save \nVRCA", delegate
            {
                RippingHandler.DownloadAvatar(PlayerWrappers.GetSelectedPlayer().GetApiAvatar()).Start();
            }, "Download the VRCA File", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(AvatarMenu, 2, 1.5f, "Lewd \nAvatar", delegate
            {
                Lewdify.Lewd(PlayerWrappers.GetSelectedPlayer());
            }, "Make the Avatar Lewd", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(AvatarMenu, 3, 1.5f, "Copy \nAvatarID", delegate
            {
                Clipboard.SetText(PlayerWrappers.GetSelectedPlayer().GetApiAvatar().id);
            }, "Copy the AvatarID to Clipboard", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(AvatarMenu, 4, 1, "Reload \nAvatar", delegate
            {
                PlayerExtensions.ReloadAvatar(PlayerWrappers.GetSelectedAPIUser());
            }, "Reload the Avatar", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(AvatarMenu, 4, 1.5f, "Forceclone", delegate
            {
                VRC.Player SelectedPlayer = PlayerWrappers.GetSelectedPlayer();
                if (SelectedPlayer.GetApiAvatar().releaseStatus == "public") PlayerExtensions.ChangeAvatar(SelectedPlayer.GetApiAvatar().id);
            }, "Clone the Avatar", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(AvatarMenu, 1, 1f, "Copy \nAssetURL", delegate
            {
                VRC.Player SelectedPlayer = PlayerWrappers.GetSelectedPlayer();
                Clipboard.SetText(SelectedPlayer.GetApiAvatar().assetUrl);
                Extensions.Logger.Log(SelectedPlayer.GetApiAvatar().assetUrl, Extensions.Logger.LogsType.Info);
            }, "Clone the AssetURL", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(AvatarMenu, 1, 1.5f, "Copy \nImageURL", delegate
            {
                VRC.Player SelectedPlayer = PlayerWrappers.GetSelectedPlayer();
                Clipboard.SetText(SelectedPlayer.GetApiAvatar().assetUrl);
                Extensions.Logger.Log(SelectedPlayer.GetApiAvatar().imageUrl, Extensions.Logger.LogsType.Info);
            }, "Copy the ImageURL", null, QMButtonAPI.ButtonSize.Half);

            QMScrollMenu AvatarScroll = new(AvatarMenu, 1, 2, "Mesh \nEditor", null, "Edit the Avatar Mesh", "Avatar Options", QMButtonAPI.ButtonSize.Half);
            AvatarScroll.SetAction(delegate
            {
                foreach (SkinnedMeshRenderer renderer in PlayerWrappers.GetSelectedPlayer().GetComponentsInChildren<SkinnedMeshRenderer>(true))
                {
                    AvatarScroll.Add(renderer.name, "Toggle Mesh", delegate
                    {
                        if (!renderer.enabled || !renderer.gameObject.active)
                        {
                            renderer.enabled = true;
                            renderer.gameObject.SetActive(true);        
                        }
                        else renderer.enabled = false;
                    });
                }
            });

            new QMToggleButton(ExploitMenu, 1, 0, "Repeat \nVoice", delegate
            {
                PatchExtensions.VoiceTarget = PlayerWrappers.GetSelectedAPIUser().UserID();
                PatchExtensions.RepeatUspeak = true;
            }, delegate
            {
                PatchExtensions.RepeatUspeak = false;
                PatchExtensions.VoiceTarget = "";
            }, "Repeat the Players Voice");

            new QMToggleButton(ExploitMenu, 2, 0, "Repeat \nMotion", delegate
            {
                PatchExtensions.MovementTarget = PlayerWrappers.GetSelectedAPIUser().UserID();
                PatchExtensions.RepeatMovement = true;
            }, delegate
            {
                PatchExtensions.RepeatMovement = false;
                PatchExtensions.MovementTarget = "";
                FakeSerialize.MovementToCopy = null;
            }, "Repeat the Players Movement");

            PortalKOSToggle = new QMToggleButton(ExploitMenu, 3, 0, "Portal \nSpam", delegate
            {
                bool ShouldStart = false;
                if (PortalHandler.PortalTargets.Count == 0) ShouldStart = true;
                if (!PortalHandler.PortalTargets.Contains(PlayerWrappers.GetSelectedAPIUser().UserID()))
                {
                    PortalHandler.PortalTargets.Add(PlayerWrappers.GetSelectedAPIUser().UserID());
                    if (ShouldStart) Exploits.PortalSpam().Start();
                }
            }, delegate
            {
                PortalHandler.PortalTargets.Remove(PlayerWrappers.GetSelectedAPIUser().UserID());
            }, "Spam the Player with Portals");

            new QMToggleButton(ExploitMenu, 4, 0, "Item \nOrbit", delegate
            {
                ItemHandler.ItemRotate(PlayerWrappers.GetSelectedPlayer()).Start();
            }, delegate
            {
                ItemHandler.ItemOrbitEnabled = false;
            }, "Orbit the Player with Items");

            new QMToggleButton(ExploitMenu, 1, 1, "Annoying \nCamera", delegate
            {
                CameraHandler.UserCamAnnoy = true;
                CameraHandler.UserCamAnnoyTarget = PlayerWrappers.GetSelectedPlayer();
                CameraHandler.TakePicture(int.MaxValue);
            }, delegate
            {
                Utils.UserCameraController.StopAllCoroutines();
                CameraHandler.UserCamAnnoy = false;
                CameraHandler.UserCamAnnoyTarget = null;
            }, "Play the Camera sound at the Player");

            new QMToggleButton(ExploitMenu, 2, 1, "Pickup \nSpam", delegate
            {
                ItemHandler.PickupSpam(PlayerWrappers.GetSelectedPlayer().GetVRCPlayer()).Start();
            }, delegate
            {
                ItemHandler.PickupSpamToggle = false;
            }, "Spam the Player with Pickups");

            new QMSingleButton(ExploitMenu, 4, 1, "Invalid \nPortal", delegate
            {
                PortalHandler.DropCrashPortal(PlayerWrappers.GetSelectedPlayer().GetVRCPlayer());
            }, "Drop a Invalid Portal on the Player", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ExploitMenu, 4, 1.5f, "Infinit \nPortal", delegate
            {
                PortalHandler.DropInfinitePortal(PlayerWrappers.GetSelectedPlayer().GetVRCPlayer());
            }, "Drop a Infinit Portal on the Player", null, QMButtonAPI.ButtonSize.Half);

            new QMToggleButton(ExploitMenu, 1, 2, "EV4 \nSpam", delegate
            {
                int[] Actor = new int[] { PlayerWrappers.GetSelectedPlayer().field_Private_Player_0.ActorID() };
                PhotonModule.Event4Spam(Actor).Start();
            }, delegate
            {
                PhotonModule.Ev4Crash = false;
            }, "Spam the Player with Pickups");

            new QMSingleButton(ExploitMenu, 3, 1, "Avatar \nCrash", delegate
            {
                Exploits.TargetAvatarCrash(PlayerWrappers.GetSelectedPlayer(), ServerRequester.PCCrash).Start();
            }, "Crash the Player with Avatars", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ExploitMenu, 3, 1.5f, "Quest \nCrash", delegate
            {
                Exploits.TargetAvatarCrash(PlayerWrappers.GetSelectedPlayer(), ServerRequester.QuestCrash).Start();
            }, "Crash the Player with Quest Avatars", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(BotMenu, 1, 0, "Avatar \nCrash", delegate
            {
                Websocket.Server.SendMessage($"HandleExploit/Avatar/{PlayerWrappers.GetSelectedAPIUser().UserID()}/{ServerRequester.PCCrash}");
            }, "Crash the Player with the Bots Avatar", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(BotMenu, 1, 0.5f, "Quest \nCrash", delegate
            {
                Websocket.Server.SendMessage($"HandleExploit/Avatar/{PlayerWrappers.GetSelectedAPIUser().UserID()}/{ServerRequester.QuestCrash}");
            }, "Crash the Player with the Bots Avatar", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(BotMenu, 2, 0, "Forcekick", delegate
            {
                Websocket.Server.SendMessage($"HandleExploit/Forcekick/{PlayerWrappers.GetSelectedAPIUser().UserID()}");
            }, "Forcekick the Player with Bots", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(BotMenu, 2, 0.5f, "Friend", delegate
            {
                Websocket.Server.SendMessage($"Friend/{PlayerWrappers.GetSelectedAPIUser().UserID()}");
            }, "Friend the Player with Bots", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(BotMenu, 3, 0, "Mute", delegate
            {
                Websocket.Server.SendMessage($"PhotonModeration/Mute/{PlayerWrappers.GetSelectedAPIUser().UserID()}/true");
            }, "Mute the Player with Bots", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(BotMenu, 3, 0.5f, "Unmute", delegate
            {
                Websocket.Server.SendMessage($"PhotonModeration/Mute/{PlayerWrappers.GetSelectedAPIUser().UserID()}/false");
            }, "Unmute the Player with Bots", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(BotMenu, 4, 0, "Block", delegate
            {
                Websocket.Server.SendMessage($"PhotonModeration/Block/{PlayerWrappers.GetSelectedAPIUser().UserID()}/true");
            }, "Block the Player with Bots", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(BotMenu, 4, 0.5f, "Unblock", delegate
            {
                Websocket.Server.SendMessage($"PhotonModeration/Block/{PlayerWrappers.GetSelectedAPIUser().UserID()}/false");
            }, "Unblock the Player with Bots", null, QMButtonAPI.ButtonSize.Half);

            new QMToggleButton(BotMenu, 1, 1, "Copy \nRPC", delegate
            {
                Websocket.Server.SendMessage($"CopyEvent/6/{PlayerWrappers.GetSelectedPlayer().prop_Player_1.ActorID()}");
            }, delegate
            {
                Websocket.Server.SendMessage($"CopyEvent/6/-2");
            }, "Copy the Players RPCs");

            new QMToggleButton(BotMenu, 2, 1, "Copy \nMotion", delegate
            {
                Websocket.Server.SendMessage($"CopyEvent/7/{PlayerWrappers.GetSelectedPlayer().prop_Player_1.ActorID()}");
            }, delegate
            {
                Websocket.Server.SendMessage($"CopyEvent/7/-2");
            }, "Copy the Players Movement");

            new QMToggleButton(BotMenu, 3, 1, "Copy \nVoice", delegate
            {
                Websocket.Server.SendMessage($"CopyEvent/1/{PlayerWrappers.GetSelectedPlayer().prop_Player_1.ActorID()}");
            }, delegate
            {
                Websocket.Server.SendMessage($"CopyEvent/1/-2");
            }, "Copy the Players Vocie");

            new QMToggleButton(BotMenu, 4, 1, "Copy \nSync", delegate
            {
                Websocket.Server.SendMessage($"CopyEvent/9/{PlayerWrappers.GetSelectedPlayer().prop_Player_1.ActorID()}");
            }, delegate
            {
                Websocket.Server.SendMessage($"CopyEvent/9/-2");
            }, "Copy the Players Vocie");

            new QMToggleButton(BotMenu, 1, 2, "Record \nMotion", delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("File Name", "Ok", delegate (string text)
                {
                    Websocket.Server.SendMessage($"RecordMovement/{PlayerWrappers.GetSelectedPlayer().prop_Player_1.ActorID()}/{text}/true");
                });
            }, delegate
            {
                Websocket.Server.SendMessage($"RecordMovement/null/null/false");
            }, "Record the Players Movement");

            new QMSingleButton(BotMenu, 2, 2, "Derank", delegate
            {
                Websocket.Server.SendMessage($"HandleExploit/Derank/{PlayerWrappers.GetSelectedAPIUser().UserID()}");
            }, "Massblock the Player", null, QMButtonAPI.ButtonSize.Half);

            HideObjectToggle = new QMToggleButton(SafetyMenu, 1, 0, "Hide \nObject", delegate
            {
                PlayerWrappers.GetSelectedPlayer().gameObject.SetActive(false);
                PortalHandler.HiddenPlayers.Add(PlayerWrappers.GetSelectedAPIUser().UserID());
            }, delegate
            {
                PlayerWrappers.GetSelectedPlayer().gameObject.SetActive(true);
                PortalHandler.HiddenPlayers.Remove(PlayerWrappers.GetSelectedAPIUser().UserID());
            }, "Hide the Player Object");

            RPCBlockToggle = new QMToggleButton(SafetyMenu, 2, 0, "RPC \nBlock", delegate
            {
                EventValidation.RPCBlockedUsers.Add(PlayerWrappers.GetSelectedAPIUser().UserID());
            }, delegate
            {
                EventValidation.RPCBlockedUsers.Remove(PlayerWrappers.GetSelectedAPIUser().UserID());
            }, "Discard RPCs from the Player");

            new QMSingleButton(SafetyMenu, 3, 0, "Ratelimit \n30s", delegate
            {
                int Actor = PlayerWrappers.GetSelectedPlayer().prop_Player_1.ActorID();
                EventValidation.LimitActor(Actor, 1, 30);
                EventValidation.LimitActor(Actor, 6, 30);
                EventValidation.LimitActor(Actor, 7, 30);
                EventValidation.LimitActor(Actor, 9, 30);
                EventValidation.LimitActor(Actor, 209, 30);
                EventValidation.LimitActor(Actor, 210, 30);
            }, "Ratelimit the Players Events", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(SafetyMenu, 3, 0.5f, "Ratelimit \n60s", delegate
            {
                int Actor = PlayerWrappers.GetSelectedPlayer().prop_Player_1.ActorID();
                EventValidation.LimitActor(Actor, 1, 60);
                EventValidation.LimitActor(Actor, 6, 60);
                EventValidation.LimitActor(Actor, 7, 60);
                EventValidation.LimitActor(Actor, 9, 60);
                EventValidation.LimitActor(Actor, 209, 60);
                EventValidation.LimitActor(Actor, 210, 60);
            }, "Ratelimit the Players Events", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(SafetyMenu, 4, 0, "Ratelimit \n120s", delegate
            {
                int Actor = PlayerWrappers.GetSelectedPlayer().prop_Player_1.ActorID();
                EventValidation.LimitActor(Actor, 1, 120);
                EventValidation.LimitActor(Actor, 6, 120);
                EventValidation.LimitActor(Actor, 7, 120);
                EventValidation.LimitActor(Actor, 9, 120);
                EventValidation.LimitActor(Actor, 209, 120);
                EventValidation.LimitActor(Actor, 210, 120);
            }, "Ratelimit the Players Events", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(SafetyMenu, 4, 0.5f, "Ratelimit \n240s", delegate
            {
                int Actor = PlayerWrappers.GetSelectedPlayer().prop_Player_1.ActorID();
                EventValidation.LimitActor(Actor, 1, 240);
                EventValidation.LimitActor(Actor, 6, 240);
                EventValidation.LimitActor(Actor, 7, 240);
                EventValidation.LimitActor(Actor, 9, 240);
                EventValidation.LimitActor(Actor, 209, 240);
                EventValidation.LimitActor(Actor, 210, 240);
            }, "Ratelimit the Players Events", null, QMButtonAPI.ButtonSize.Half);
        }

        public static void UpdateText(string UserID)
        {
            VRC.Player SelectedPlayer = Utils.PlayerManager.GetPlayer(UserID);
            if (SelectedPlayer != null)
            {
                string item = string.Concat(new string[]
                {
                        "<color=white><b>Player Informations</b></color>",
                        "\n<color=grey>Displayname:</color> " + SelectedPlayer.DisplayName(),
                        "\n<color=grey>Actor Number:</color> " + SelectedPlayer.prop_Player_1.ActorID(),
                        "\n<color=grey>Rank:</color> " + SelectedPlayer.GetAPIUser().GetRank(),
                        "\n<color=grey>VRMode:</color> " + (SelectedPlayer.GetVRCPlayerApi().IsUserInVR() ? (SelectedPlayer.GetVRCPlayer().IsPlayerFBT() ? "FBT" : "VR") : "PC"),
                        "\n<color=grey>Cloning:</color> " + (SelectedPlayer.GetAPIUser().allowAvatarCopying ? "Yes" : "No"),
                        //"\n<color=grey>Show Rank:</color> " + (SelectedPlayer.prop_Player_1.GetHashtable()["show_social_rank"] ? "Yes" : "No"),
                        //"\n<color=grey>Show Mod:</color> " + (SelectedPlayer.GetAPIUser().showModTag ? "Yes" : "No"),
                        "\n<color=grey>Early VRC+:</color> " + (SelectedPlayer.GetAPIUser().isEarlyAdopter ? "Yes" : "No"),
                        "\n<color=grey>VRC+:</color> " + (SelectedPlayer.GetAPIUser().isSupporter ? "Yes" : "No"),
                        "\n<color=grey>Block:</color> " + (ModerationHandler.BlockList.Contains(SelectedPlayer.GetVRCPlayerApi().playerId) ? "Yes" : "No"),
                        "\n<color=grey>Mute:</color> " + (ModerationHandler.MuteList.Contains(SelectedPlayer.GetVRCPlayerApi().playerId) ? "Yes" : "No"),
                        "\n<color=grey>Friend:</color> " + (SelectedPlayer.IsFriend() ? "Yes" : "No"),
                        "\n<color=grey>Bot:</color> " + (SelectedPlayer.GetIsBot() ? "Yes" : "No"),
                        "\n<color=grey>Platform:</color> " + SelectedPlayer.GetAPIUser().GetPlatform(),        
                        "\n<color=grey>Avatar Name:</color> " + SelectedPlayer.GetApiAvatar().name,
                        "\n<color=grey>Avatar Status:</color> " + SelectedPlayer.GetApiAvatar().releaseStatus,
                        "\n<color=grey>Avatar Author:</color> " + SelectedPlayer.GetApiAvatar().authorName,
                        "\n<color=grey>Avatar Version:</color> " + SelectedPlayer.GetApiAvatar().version,
                        "\n<color=grey>Avatar Platform:</color> " + SelectedPlayer.GetApiAvatar().supportedPlatforms,
                        "\n<color=grey>Avatar Upload:</color> " + SelectedPlayer.GetApiAvatar().unityVersion
                });
                PlayerInfos.SetText(item);
            }
        }
    }
}