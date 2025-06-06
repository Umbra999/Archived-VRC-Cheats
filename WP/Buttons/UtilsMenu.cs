using WengaPort.Api;
using WengaPort.Wrappers;
using UnityEngine;
using WengaPort.Modules;
using VRC;
using VRC.SDKBase;
using VRCSDK2;
using WengaPort.Extensions;
using System.Windows.Forms;

namespace WengaPort.Buttons
{
    class UtilsMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMSingleButton HalfButton;   

        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(MainMenu.ThisMenu, 2.5f, 1f, "Utils", "Util Menu", null, null, null, Color.yellow);

            new QMToggleButton(ThisMenu, 1, 0, "Selfhide", () =>
            {
                SelfHide.Initialize(true);
            }, "Disabled", () =>
            {
                SelfHide.Initialize(false);
            }, "Hide yourself");

            new QMToggleButton(ThisMenu, 2, 0, "Headlight", () =>
            {
                Headlight.Toggle(true);
            }, "Disabled", () =>
            {
                Headlight.Toggle(false);
            }, "Toggle Headlight");

            new QMToggleButton(ThisMenu, 3, 0, "Hide \nCamera", () =>
            {
                PatchManager.HideCamera = true;
            }, "Disabled", () =>
            {
                PatchManager.HideCamera = false;
            }, "Hide your Camera from other People");

            new QMToggleButton(ThisMenu, 4, 0, "Serialize", () =>
            {
                PhotonModule.CustomSerialize(true);
            }, "Disabled", () =>
            {
                PhotonModule.CustomSerialize(false);
            }, "Dont send any Movement");

            new QMToggleButton(ThisMenu, 3, 1, "Lock \nInstance", () =>
            {
                PhotonModule.LockInstance = true;
            }, "Disabled", () =>
            {
                PhotonModule.LockInstance = false;
            }, "Lock the Instance as Master");

            HalfButton = new QMSingleButton(ThisMenu, 3, 2.25f, "Portal \nby ID", () =>
            {
                string ID = Clipboard.GetText();
                if (ID.Contains("wrld_"))
                {
                    PortalHandler.DropPortal(ID);
                }
            }, "Drop Portal by ID");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 3, 1.75f, "Reupload \nAvatarID", () =>
            {
                string ID = Clipboard.GetText();
                if (ID.Contains("avtr_"))           
                {
                    Modules.Reupload.ReuploaderButtons.ReuploadAvatar(ID);
                }
            }, "Reupload a AvatarID from your Clipboard");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 1, 2.25f, "Clear \nDrawings", () =>
            {
                RoomCleaner.CleanRoom();
            }, "Clear all Drawings");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 2, 2.25f, "Respawn \nItems", () =>
            {
                foreach (VRC_ObjectSync vrc_ObjectSync in ItemHandler.World_ObjectSyncs)
                {
                    Networking.SetOwner(Utils.CurrentUser.field_Private_VRCPlayerApi_0, vrc_ObjectSync.gameObject);
                    vrc_ObjectSync.Respawn();
                }
            }, "Respawn all Items");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 2, 1.75f, "Force \nChange", () =>
            {
                string ID = Clipboard.GetText();
                if (ID.Contains("avtr_"))
                {
                    PlayerExtensions.ChangeAvatar(ID);
                }
            }, "Change into AvatarID");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 1, 1.75f, "Bring\nItems", () =>
            {
                ItemHandler.BringPickups();
            }, "Bring all Pickups to you");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            new QMToggleButton(ThisMenu, 1, 1, "ESP", () =>
            {
                ESP.ESPEnabled = true;
            }, "Disabled", () =>
            {
                ESP.ESPEnabled = false;
            }, "Toggle ESP bubbles");

            new QMToggleButton(ThisMenu, 4, 2, "Global \nDynbones", () =>
            {
                GlobalDynamicBones.GlobalBones = true;
                foreach (Player player in Utils.PlayerManager.GetAllPlayers())
                {
                    PlayerExtensions.ReloadAvatar(player);
                }
            }, "Disabled", () =>
            {
                GlobalDynamicBones.GlobalBones = false;
                foreach (Player player in Utils.PlayerManager.GetAllPlayers())
                {
                    PlayerExtensions.ReloadAvatar(player);
                }
            }, "Toggle Dynamic bones for everyone");

            new QMToggleButton(ThisMenu, 4, 1, "Friend \nDynbones", () =>
            {
                GlobalDynamicBones.FriendBones = true;
                foreach (string player in GlobalDynamicBones.FriendOnlyBones)
                {
                    var P = Utils.PlayerManager.GetPlayer(player);
                    PlayerExtensions.ReloadAvatar(P);
                }
            }, "Disabled", () =>
            {
                GlobalDynamicBones.FriendBones = false;
                foreach (string player in GlobalDynamicBones.FriendOnlyBones)
                {
                    var P = Utils.PlayerManager.GetPlayer(player);
                    PlayerExtensions.ReloadAvatar(P);
                }
            }, "Toggle Dynamic bones for Friends Only", Color.cyan, Color.white, false, true);

            new QMToggleButton(ThisMenu, 0, 0, "No \nPickup", () =>
            {
                ItemHandler.PickupToggle = true;
                foreach (var item in Resources.FindObjectsOfTypeAll<VRCSDK2.VRC_Pickup>())
                {
                    if (item.gameObject.active && item.pickupable)
                    {
                        item.pickupable = false;
                        ItemHandler.World_Pickups.Add(item);
                    }
                }
            }, "Disabled", () =>
            {
                ItemHandler.PickupToggle = false;
                foreach (VRCSDK2.VRC_Pickup pickup in ItemHandler.World_Pickups)
                {
                    pickup.pickupable = true;
                }
            }, "Toggle Item Pickup");

            new QMToggleButton(ThisMenu, 0, 1, "Auto Hold \nPickups", () =>
            {
                ItemHandler.AutoHoldToggle = true;
                foreach (var item in Resources.FindObjectsOfTypeAll<VRCSDK2.VRC_Pickup>())
                {
                    if (item.gameObject.active && item.pickupable)
                    {
                        item.AutoHold = (VRC.SDKBase.VRC_Pickup.AutoHoldMode)1;
                        ItemHandler.World_Pickups.Add(item);
                    }
                }
            }, "Disabled", () =>
            {
                ItemHandler.AutoHoldToggle = false;
                foreach (VRCSDK2.VRC_Pickup pickup in ItemHandler.World_Pickups)
                {
                    pickup.AutoHold = 0;
                    pickup.pickupable = true;
                }
            }, "Auto Hold Pickups");

            new QMToggleButton(ThisMenu, 2, 1, "Fast \nPickups", () =>
            {
                ItemHandler.FastPickupToggle = true;
                foreach (var item in Resources.FindObjectsOfTypeAll<VRCSDK2.VRC_Pickup>())
                {
                    if (item.gameObject.active && item.pickupable)
                    {
                        item.ThrowVelocityBoostMinSpeed = int.MaxValue;
                        ItemHandler.World_Pickups.Add(item);
                    }
                }
            }, "Disabled", () =>
            {
                ItemHandler.FastPickupToggle = false;
                foreach (VRCSDK2.VRC_Pickup pickup in ItemHandler.World_Pickups)
                {
                    pickup.ThrowVelocityBoostMinSpeed = 1;
                }
            }, "Thrown Pickups are very Fast");

            HalfButton = new QMSingleButton(ThisMenu, 5, 0.75f, "Reload \nAvatar", () =>
            {
                PlayerExtensions.ReloadAvatar(Utils.CurrentUser);
            }, "Reload your Avatar");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 5, 1.25f, "Infinit \nTimer", () =>
            {
                foreach (PortalInternal portalInternal in Object.FindObjectsOfType<PortalInternal>())
                {
                    portalInternal.SetTimerRPC(float.NegativeInfinity, Utils.CurrentUser.GetPlayer());
                }
            }, "Reset the Portal Timer to Infinit");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 5, 1.75f, "Interact \nAll", () =>
            {
                foreach (VRC.SDKBase.VRC_Trigger vrc_Trigger in Object.FindObjectsOfType<VRC.SDKBase.VRC_Trigger>())
                {
                    vrc_Trigger.TakesOwnershipIfNecessary.ToString();
                    vrc_Trigger.Interact();
                }
            }, "Interact with all Triggers");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
        }
    }
}
