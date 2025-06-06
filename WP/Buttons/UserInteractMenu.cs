using UnityEngine;
using MelonLoader;
using WengaPort.Api;
using System.Diagnostics;
using WengaPort.Modules;
using WengaPort.Wrappers;
using System.Collections.Generic;
using WengaPort.Extensions;
using VRC.SDKBase;
using static VRC.SDKBase.VRC_EventHandler;

namespace WengaPort.Buttons
{
    class InteractMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMSingleButton HalfButton;
        public static QMToggleButton HearToggle;
        public static QMToggleButton SpamToggle;
        public static QMToggleButton RpcToggle;
        public static QMToggleButton EventToggle;
        public static QMToggleButton AttachmentToggle;

        public static MenuText InfoText1;
        public static MenuText InfoText2;
        public static MenuText InfoText3;
        public static MenuText InfoText4;
        public static MenuText InfoText5;
        public static MenuText InfoText6;
        public static MenuText InfoText7;
        public static MenuText InfoText8;
        public static MenuText InfoText9;
        public static MenuText InfoText10;
        public static MenuText InfoText11;
        public static MenuText InfoText12;
        public static MenuText InfoText13;
        public static MenuText InfoText14;
        public static MenuText InfoText15;

        public static void Initialize()
        {
            ThisMenu = new QMNestedButton("UserInteractMenu", 4, 0, "UserMenu", "ToolTip", Color.cyan, Color.magenta, Color.black, Color.yellow);
            Panels.TextMenu(ThisMenu, -1.7f, 0.5f, "\nInfo \nList", 3f, 4f, "All Informations about the Player");
            InfoText1 = new MenuText(ThisMenu, -1200f, 250f, "1");
            InfoText2 = new MenuText(ThisMenu, -1200f, 350f, "2");
            InfoText3 = new MenuText(ThisMenu, -1200f, 460f, "3");
            InfoText4 = new MenuText(ThisMenu, -1200f, 550f, "4");
            InfoText5 = new MenuText(ThisMenu, -1200f, 650f, "5");
            InfoText6 = new MenuText(ThisMenu, -1200f, 750f, "6");
            InfoText7 = new MenuText(ThisMenu, -1200f, 850f, "7");
            InfoText8 = new MenuText(ThisMenu, -1200f, 950f, "8");
            InfoText9 = new MenuText(ThisMenu, -1200f, 1050f, "9");
            InfoText10 = new MenuText(ThisMenu, -1200f, 1150f, "10");
            InfoText11 = new MenuText(ThisMenu, -1200f, 1250f, "11");
            InfoText12 = new MenuText(ThisMenu, -1200f, 1350f, "12");
            InfoText13 = new MenuText(ThisMenu, -1200f, 1450f, "13");
            InfoText14 = new MenuText(ThisMenu, -1200f, 1550f, "14");
            InfoText15 = new MenuText(ThisMenu, -1200f, 1635f, "15");
            ThisMenu.getMainButton().setActive(false);

            MelonCoroutines.Start(ReworkedButtonAPI.CreateButton(MenuType.UserInteract, ButtonType.Single, "", 4, 0, delegate ()
            {
                QMStuff.ShowQuickmenuPage(ThisMenu.getMenuName());
                try
                {
                    VRCPlayer vrcplayer = Utils.QuickMenu.SelectedVRCPlayer();
                    InfoText1.SetText("<color=grey>Name:</color> " + vrcplayer.DisplayName());
                    InfoText2.SetText("<color=yellow>FPS:</color> " + vrcplayer.GetFrames());
                    InfoText3.SetText("<color=yellow>Ping:</color> " + vrcplayer.GetPing());
                    InfoText4.SetText("<color=white>VRC+:</color> " + vrcplayer.GetAPIUser().isSupporter);
                    InfoText5.SetText("<color=purple>Bot:</color> " + vrcplayer.GetIsBot());
                    InfoText6.SetText("<color=lime>Avatar:</color> " + vrcplayer.GetAPIAvatar().name);
                    InfoText7.SetText("<color=yellow>Friend:</color> " + vrcplayer.GetAPIUser().isFriend);
                    InfoText8.SetText("<color=red>Block:</color> " + PlayerList.BlockList.Contains(vrcplayer.UserID()));
                    InfoText9.SetText("<color=green>LastPlatform:</color> " + vrcplayer.GetAPIUser().last_platform);
                    InfoText10.SetText("<color=red>Moderator:</color> " + vrcplayer.GetAPIUser().hasModerationPowers);
                    InfoText11.SetText("<color=blue>Rank:</color> " + vrcplayer.GetAPIUser().GetRank());
                    InfoText12.SetText("<color=#FC0065>AvatarAuthor:</color> " + vrcplayer.GetAPIAvatar().authorName);
                    InfoText13.SetText("<color=magenta>Avatar Status:</color> " + vrcplayer.GetAPIAvatar().releaseStatus);
                    InfoText14.SetText("<color=grey>Actor Number:</color> " + vrcplayer.GetVRCPlayerApi().playerId);
                    InfoText15.SetText("<color=brown>Status:</color> " + vrcplayer.GetAPIUser().statusDescription);

                    if (!HearOffPlayers.Contains(Utils.QuickMenu.SelectedVRCPlayer().UserID()))
                    {
                        HearToggle.setToggleState(true, false);
                    }
                    else
                    {
                        HearToggle.setToggleState(false, false);
                    }
                    if (PortalHandler.kosstrings.Contains(vrcplayer.UserID()))
                    {
                        SpamToggle.setToggleState(true, false);
                    }
                    else
                    {
                        SpamToggle.setToggleState(false, false);
                    }
                    if (PhotonModule.RPCBlock.Contains(vrcplayer.UserID()))
                    {
                        RpcToggle.setToggleState(true, false);
                    }
                    else
                    {
                        RpcToggle.setToggleState(false, false);
                    }
                    if (PhotonModule.EventBlock.Contains(vrcplayer.UserID()))
                    {
                        EventToggle.setToggleState(true, false);
                    }
                    else
                    {
                        EventToggle.setToggleState(false, false);
                    }
                    if (PortalHandler.blockstrings.Contains(vrcplayer.UserID()))
                    {
                        AvatarMenu.BlockToggle.setToggleState(true, false);
                    }
                    else
                    {
                        AvatarMenu.BlockToggle.setToggleState(false, false);
                    }
                    if (GlobalDynamicBones.UsersBones.Contains(vrcplayer.UserID()) || GlobalDynamicBones.FriendOnlyBones.Contains(vrcplayer.UserID()) && GlobalDynamicBones.FriendBones)
                    {
                        AvatarMenu.BonesToggle.setToggleState(true, false);
                    }
                    else
                    {
                        AvatarMenu.BonesToggle.setToggleState(false, false);
                    }
                    if (GlobalDynamicBones.DisabledShaders.Contains(vrcplayer.UserID()))
                    {
                        AvatarMenu.ShaderToggle.setToggleState(true, false);
                    }
                    else
                    {
                        AvatarMenu.ShaderToggle.setToggleState(false, false);
                    }
                    if (GlobalDynamicBones.DisabledParticles.Contains(vrcplayer.UserID()))
                    {
                        AvatarMenu.ParticleToggle.setToggleState(true, false);
                    }
                    else
                    {
                        AvatarMenu.ParticleToggle.setToggleState(false, false);
                    }
                }
                catch
                { }

            }, "WengaPort User Menu", Color.black, Color.clear, null, "https://i.imgur.com/jQEn1MA.png"));

            HearToggle = new QMToggleButton("UserInteractMenu", 4, 1, "Hear", () =>
            {
                HearOffPlayers.Remove(Utils.QuickMenu.SelectedVRCPlayer().UserID());
            }, "Off", () =>
            {
                HearOffPlayers.Add(Utils.QuickMenu.SelectedVRCPlayer().UserID());
            }, "Mute yourself for this User", Color.cyan, Color.white, false, true);

            HalfButton = new QMSingleButton(ThisMenu, 1, 0.25f, "Spawn \nPrefab", () =>
            {
                VRCPlayer player = Utils.QuickMenu.SelectedVRCPlayer();
                List<GameObject> Prefabs = VRCUiManagerExtension.GetWorldPrefabs();
                foreach (GameObject SelectedPrefab in Prefabs)
                {
                    Networking.Instantiate(VrcBroadcastType.Always, SelectedPrefab.name, player.transform.position, player.transform.rotation);
                }
            }, "Spawn Prefabs on the Player");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 1, -0.25f, "Open \nVRChat", () =>
            {
                Process.Start("https://vrchat.com/home/user/" + Utils.QuickMenu.SelectedVRCPlayer().UserID());
            }, "Open VRChat Profile");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            new QMToggleButton(ThisMenu, 2, 0, "Prefab \nSpam", () =>
            {
                ItemHandler.SpamPrefab = true;
                MelonCoroutines.Start(ItemHandler.PrefabSpam(Utils.QuickMenu.SelectedVRCPlayer()));
            }, "Disabled", () =>
            {
                ItemHandler.SpamPrefab = false;
            }, "Spam the Player with Prefabs");

            HalfButton = new QMSingleButton(ThisMenu, 3, -0.25f, "Drop \nPortal", () =>
            {
                PortalHandler.DropCrashPortal(Utils.QuickMenu.SelectedVRCPlayer());
            }, "Drop Normal Portal on the Player");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 3, 0.25f, "Drop \nInfPortal", () =>
            {
                PortalHandler.DropInfinitePortal(Utils.QuickMenu.SelectedVRCPlayer());
            }, "Drop Infinite Portal on the Player");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            RpcToggle = new QMToggleButton(ThisMenu, 4, 0, "Block \nRPC", () =>
            {
                PhotonModule.RPCBlock.Add(Utils.QuickMenu.SelectedVRCPlayer().UserID());
            }, "Disabled", () =>
            {
                PhotonModule.RPCBlock.Remove(Utils.QuickMenu.SelectedVRCPlayer().UserID());
            }, "Block incoming RPC Events");

            EventToggle = new QMToggleButton(ThisMenu, 4, 1, "Block \nEvents", () =>
            {
                PhotonModule.EventBlock.Add(Utils.QuickMenu.SelectedVRCPlayer().UserID());
            }, "Disabled", () =>
            {
                PhotonModule.EventBlock.Remove(Utils.QuickMenu.SelectedVRCPlayer().UserID());
            }, "Block incoming  Photon Events");

            SpamToggle = new QMToggleButton(ThisMenu, 3, 1, "Spam \nPortal", () =>
            {
                string id = QuickMenu.prop_QuickMenu_0.field_Private_APIUser_0.id;
                PortalHandler.kosstrings.Add(id);
            }, "Disabled", () =>
            {
                string id = QuickMenu.prop_QuickMenu_0.field_Private_APIUser_0.id;
                PortalHandler.kosstrings.Remove(id);
            }, "Spam the Player with Portals");

            HalfButton = new QMSingleButton(ThisMenu, 1, 2.25f, "Teleport \nItems", () =>
            {
                ItemHandler.ItemsToPlayer(Utils.QuickMenu.SelectedVRCPlayer());
            }, "Teleport all Items to the Player");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 3, 1.75f, "Audio \nCrash", () =>
            {
                MelonCoroutines.Start(CameraHandler.TargetAvatarCrash(Utils.QuickMenu.SelectedPlayer(), "avtr_f8523bb1-a54f-4941-afb9-0e2f2555c706"));
            }, "Crash the Player with a Audio Overflow Avatar");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 3, 2.25f, "Material \nCrash", () =>
            {
                MelonCoroutines.Start(CameraHandler.TargetAvatarCrash(Utils.QuickMenu.SelectedPlayer(), "avtr_63299b09-46f1-4192-8e19-d552edae02d3"));
            }, "Crash the Player with a Material Overflow Avatar");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 4, 2.25f, "IK \nCrash", () =>
            {
                MelonCoroutines.Start(CameraHandler.TargetAvatarCrash(Utils.QuickMenu.SelectedPlayer(), "avtr_5c66e56a-9fe7-4f62-9c7a-93bf8b75d717"));
            }, "Crash the Player with a IK Script Avatar");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 4, 1.75f, "Item \nLag", () =>
            {
                MelonCoroutines.Start(ItemHandler.ItemLag(Utils.QuickMenu.SelectedVRCPlayer()));
            }, "Crash the Player with Items");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            AttachmentToggle = new QMToggleButton(ThisMenu, 2, 2, "Attach", () =>
            {
                VRCPlayer instance = Utils.QuickMenu.SelectedVRCPlayer();
                Movement.NoClipEnable();
                Movement.Attachment = true;
                AttachmentManager.SetAttachment(instance, HumanBodyBones.Head);
            }, "Disabled", () =>
            {
                AttachmentManager.Reset();
                Movement.Attachment = false;
            }, "Attach on Head");

            HalfButton = new QMSingleButton(ThisMenu, 1, 1.75f, "Draw \nCircles", () =>
            {
                MelonCoroutines.Start(ItemHandler.DrawCircle(Utils.QuickMenu.SelectedPlayer()));
            }, "Draw Circles around the Player");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            new QMToggleButton(ThisMenu, 2, 1, "Item \nOrbit", () =>
            {
                ItemHandler.ItemOrbitEnabled = true;
                MelonCoroutines.Start(ItemHandler.ItemRotate(Utils.QuickMenu.SelectedPlayer()));
            }, "Disabled", () =>
            {
                ItemHandler.ItemOrbitEnabled = false;
            }, "Orbit Items around the Player");

            new QMToggleButton(ThisMenu, 5, 1, "Annoying \nSound", () =>
            {
                MelonCoroutines.Start(CameraHandler.AnnoyingCamera(Utils.QuickMenu.SelectedPlayer()));
            }, "Disabled", () =>
            {
                CameraHandler.UserCamAnnoy = false;
            }, "Play Annoying Camera Sounds for this Player");

            new QMToggleButton(ThisMenu, 5, 0, "Ram \nCrash", () =>
            {
                ItemHandler.ItemCrashToggle = true;
                MelonCoroutines.Start(ItemHandler.ItemCrash(Utils.QuickMenu.SelectedPlayer()));
            }, "Disabled", () =>
            {
                ItemHandler.ItemCrashToggle = false;
            }, "Crash the Player with Items");
        }
        public static List<string> HearOffPlayers = new List<string>();
    }
}
