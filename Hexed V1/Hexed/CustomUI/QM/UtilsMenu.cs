using Hexed.Core;
using Hexed.Extensions;
using Hexed.Modules;
using Hexed.Modules.Standalone;
using Hexed.UIApi;
using Hexed.Wrappers;
using UnityEngine;

namespace Hexed.CustomUI.QM
{
    internal class UtilsMenu
    {
        private static QMMenuPage UtilsPage;
        public static QMToggleButton FakeSerializeToggle;

        public static void Init()
        {
            UtilsPage = new("Utils");

            QMSingleButton OpenMenu = new(MainMenu.ClientPage, 2, 0, "Utils", UtilsPage.OpenMe, "Utils Options", ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Utils"));

            new QMToggleButton(UtilsPage, 1, 0, "No \nSpawnsound", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoSpawnsound", true);
                InternalSettings.NoSpawnsound = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoSpawnsound", false);
                InternalSettings.NoSpawnsound = false;
            }, "Disable Avatar spawn Audios", ConfigManager.Ini.GetBool("Toggles", "NoSpawnsound"));

            new QMToggleButton(UtilsPage, 2, 0, "Headlight", delegate
            {
                ItemHelper.ToggleHeadlight(true);
            }, delegate
            {
                ItemHelper.ToggleHeadlight(false);
            }, "Light up the Map");

            new QMToggleButton(UtilsPage, 3, 0, "Invisible \nCamera", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "InvisibleCamera", true);
                InternalSettings.InvisibleCamera = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "InvisibleCamera", false);
                InternalSettings.InvisibleCamera = false;
            }, "Hide your Camera from other Players", ConfigManager.Ini.GetBool("Toggles", "InvisibleCamera"));

            new QMSingleButton(UtilsPage, 4, 0, "Reload \nAvatars", Misc.ReloadAvatars, "Reload all Avatars", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(UtilsPage, 4, 0.5f, "Change \nAvatar", delegate
            {
                GameHelper.VRCUiPopupManager.AskInGameInput("Avatar ID", "Ok", delegate (string text)
                {
                    PlayerUtils.ChangeAvatar(text);
                }, "avtr_");
            }, "Change Avatar by ID", ButtonAPI.ButtonSize.Half);

            FakeSerializeToggle = new QMToggleButton(UtilsPage, 1, 1, "Fake \nSerialize", delegate
            {
                FakeSerialize.CustomSerialize(true);
            }, delegate
            {
                FakeSerialize.CustomSerialize(false);
            }, "Stop moving for other Players");

            new QMSingleButton(UtilsPage, 2, 1, "Delete \nPortals", SharingHandler.DeletePortals, "Delete all existing Portals", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(UtilsPage, 3, 1, "Drop \nPortal", delegate
            {
                GameHelper.VRCUiPopupManager.AskInGameInput("World ID", "Ok", delegate (string text)
                {
                    new VRC.Core.ApiWorldInstance() { id = text, instanceId = text.Split(':')[1], worldId = text.Split(':')[0], world = new VRC.Core.ApiWorld() { id = text.Split(':')[0] } }.Fetch(new Action<VRC.Core.ApiContainer>((container) =>
                    {
                        VRC.Core.ApiModelContainer<VRC.Core.ApiWorldInstance> apiWorldInstance = new();
                        apiWorldInstance.setFromContainer(container);
                        VRC.Core.ApiWorldInstance World = container.Model.Cast<VRC.Core.ApiWorldInstance>();

                        PhotonHelper.RaisePortalCreate(World.shortName, GameHelper.CurrentPlayer.transform.position + Vector3.forward * 1.505f, 0);
                    }));
                }, "wrld_");
            }, "Drop a Portal by ID", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(UtilsPage, 3, 1.5f, "Copy \nWorldID", delegate
            {
                GeneralUtils.CopyToClipboard($"{GameUtils.GetCurrentWorldID()}");
            }, "Copy the current WorldID", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(UtilsPage, 4, 1, "Copy \nServer IP", delegate
            {
                GeneralUtils.CopyToClipboard($"{GameHelper.VRCNetworkingClient.field_Private_String_3}");
            }, "Copy the current Photon IP", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(UtilsPage, 4, 1.5f, "Join \nWorldID", delegate
            {
                GameHelper.VRCUiPopupManager.AskInGameInput("World ID", "Ok", delegate (string text)
                {
                    GameUtils.Forcejoin(text);
                }, "wrld_");
            }, "Join the World by ID", ButtonAPI.ButtonSize.Half);
        }
    }
}
