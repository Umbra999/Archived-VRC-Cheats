using LunaR.Api;
using LunaR.Extensions;
using LunaR.Modules;
using LunaR.Wrappers;
using System;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase;

namespace LunaR.QMButtons
{
    internal class UtilsMenu
    {
        public static QMToggleButton SerializeToggle;

        public static void Init()
        {
            QMNestedButton ThisMenu = new(MainMenu.ClientMenu, 2, 0, "Utils", "Utils Menu", "Utils Options", QMButtonAPI.ButtonSize.Default, GeneralWrappers.GetSprite("Utils"));

            new QMToggleButton(ThisMenu, 1, 0, "Selfhide", delegate
            {
                PlateChanger.Selfhide(true);
            }, delegate
            {
                PlateChanger.Selfhide(false);
            }, "Hide yourself");

            new QMToggleButton(ThisMenu, 2, 0, "Headlight", delegate
            {
                ItemHandler.Headlight(true);
            }, delegate
            {
                ItemHandler.Headlight(false);
            }, "Spawn a little Light");

            SerializeToggle = new QMToggleButton(ThisMenu, 4, 0, "Serialize", delegate
            {
                FakeSerialize.CustomSerialize(true);
            }, delegate
            {
                FakeSerialize.CustomSerialize(false);
            }, "Send fake Movement");

            new QMToggleButton(ThisMenu, 1, 1, "Player \nESP", delegate
            {
                ESP.ESPToggle(true);
            }, delegate
            {
                ESP.ESPToggle(false);
            }, "Player ESP bubbles");

            new QMToggleButton(ThisMenu, 2, 1, "Item \nESP", delegate
            {
                ESP.ItemESP().Start();
            }, delegate
            {
                ESP.PickupESP = false;
            }, "Item ESP bubbles");

            new QMToggleButton(ThisMenu, 3, 1, "Trigger \nESP", delegate
            {
                ESP.TriggerESP().Start();
            }, delegate
            {
                ESP.TriggerESPToggle = false;
            }, "Trigger ESP bubbles");

            new QMToggleButton(ThisMenu, 4, 1, "Force \nSync", delegate
            {
                Extensions.ConfigManager.Ini.SetBool("Toggles", "FreezeItems", true);
                PatchExtensions.FreezeItems = true;
            }, delegate
            {
                Extensions.ConfigManager.Ini.SetBool("Toggles", "FreezeItems", false);
                PatchExtensions.FreezeItems = false;
            }, "Force all Object Syncs to be yours", Extensions.ConfigManager.Ini.GetBool("Toggles", "FreezeItems"));

            new QMToggleButton(ThisMenu, 1, 2, "Infinity \nPortals", delegate
            {
                Extensions.ConfigManager.Ini.SetBool("Toggles", "InfinityPortals", true);
                PatchExtensions.InfinityPortals = true;
            }, delegate
            {
                Extensions.ConfigManager.Ini.SetBool("Toggles", "InfinityPortals", false);
                PatchExtensions.InfinityPortals = false;
            }, "Set all Portal timers to Infinity", Extensions.ConfigManager.Ini.GetBool("Toggles", "InfinityPortals"));

            new QMToggleButton(ThisMenu, 2, 2, "No \nPickup", delegate
            {
                Extensions.ConfigManager.Ini.SetBool("Toggles", "NoPickup", true);
                ItemHandler.NoPickup = true;
                Utils.CurrentUser.GetVRCPlayerApi().EnablePickups(false);
            }, delegate
            {
                Extensions.ConfigManager.Ini.SetBool("Toggles", "NoPickup", false);
                ItemHandler.NoPickup = false;
                Utils.CurrentUser.GetVRCPlayerApi().EnablePickups(true);
            }, "Prevent Pickup grabbing", Extensions.ConfigManager.Ini.GetBool("Toggles", "NoPickup"));

            new QMToggleButton(ThisMenu, 3, 2, "Global \nTrigger", delegate
            {
                Extensions.ConfigManager.Ini.SetBool("Toggles", "GlobalTrigger", true);
                PatchExtensions.WorldTrigger = true;
            }, delegate
            {
                Extensions.ConfigManager.Ini.SetBool("Toggles", "GlobalTrigger", false);
                PatchExtensions.WorldTrigger = false;
            }, "Make all Triggers Global", Extensions.ConfigManager.Ini.GetBool("Toggles", "GlobalTrigger"));

            new QMToggleButton(ThisMenu, 4, 2, "Pickup \nRange", delegate
            {
                ItemHandler.InfinitRange(true);
            }, delegate
            {
                ItemHandler.InfinitRange(false);
            }, "Pickups are pickupable from everywhere");

            new QMToggleButton(ThisMenu, 4, 3, "T-Pose", delegate
            {
                Utils.CurrentUser.field_Internal_Animator_0.enabled = false;
            }, delegate
            {
                Utils.CurrentUser.field_Internal_Animator_0.enabled = true;
            }, "Override your Animator with TPose");

            new QMSingleButton(ThisMenu, 1, 3f, "Portal \nby ID", delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("World ID", "Ok", delegate (string text)
                {
                    if (text.Contains("wrld_")) PortalHandler.DropPortalToWorld(Utils.CurrentUser, text);
                });
            }, "Drop a Portal by WorldID", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ThisMenu, 1, 3.5f, "Reset \nPortals", delegate
            {
                foreach (PortalInternal portalInternal in UnityEngine.Object.FindObjectsOfType<PortalInternal>())
                {
                    portalInternal.SetTimerRPC(float.MinValue, Utils.CurrentUser.GetPlayer());
                }
            }, "Reset all Portal Timers", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ThisMenu, 2, 3f, "Check \nUserID", delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("User ID", "Ok", delegate (string text)
                {
                    if (text.Contains("usr_"))
                    {
                        APIUser.FetchUser(text, new Action<APIUser>(User =>
                        {
                            Extensions.Logger.Log(string.Concat(new object[]
                            {
                                Environment.NewLine,
                                $"======= Player Informations =======", Environment.NewLine,
                                $"Displayname: {User.DisplayName()}", Environment.NewLine,
                                $"Username: {User.username}", Environment.NewLine,
                                $"UserID: {User.UserID()}", Environment.NewLine,
                                $"Platform: {User.last_platform}", Environment.NewLine,
                                $"Last Login: {User.last_platform}", Environment.NewLine,
                                $"Rank: {User.GetRank()}", Environment.NewLine,
                                $"Creation: {User.date_joined}", Environment.NewLine,
                                $"Avatar Copy: {User.allowAvatarCopying}", Environment.NewLine,
                                $"VRC+: {User.isSupporter}", Environment.NewLine,
                                $"User Icon: {User.userIcon}", Environment.NewLine,
                                $"User Picture: {User.profilePicImageUrl}", Environment.NewLine,
                                "====================================",
                            }), Extensions.Logger.LogsType.Clean);
                        }), new Action<string>(error => { }));
                    }
                });
            }, "Check User by UserID", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ThisMenu, 2, 3.5f, "Respawn \nItems", delegate
            {
                foreach (VRC_Pickup vrc_ObjectSync in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                {
                    ItemHandler.TakeOwnershipIfNecessary(vrc_ObjectSync.gameObject);
                    vrc_ObjectSync.transform.position = default;
                }
            }, "Respawn all Items in the World", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ThisMenu, 3, 3f, "Change \nAvatar", delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("Avatar ID", "Ok", delegate (string text)
                {
                    PlayerExtensions.ChangeAvatar(text);
                });
            }, "Change into Avatar by ID", null, QMButtonAPI.ButtonSize.Half);

            new QMSingleButton(ThisMenu, 3, 3.5f, "Override \nVideo", delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("URL", "Ok", delegate (string text)
                {
                    Exploits.OverrideVideoPlayers(text);
                });
            }, "Override all Videoplayers", null, QMButtonAPI.ButtonSize.Half);
        }
    }
}