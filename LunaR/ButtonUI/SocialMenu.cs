using LunaR.Buttons.Bots;
using LunaR.Extensions;
using LunaR.Modules;
using LunaR.UIApi;
using LunaR.Wrappers;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using UnityEngine;
using VRC.Core;
using VRC.UI;

namespace LunaR.Buttons
{
    internal class SocialMenu
    {
        public static void Initialize()
        {
            new MenuButton(GameObject.Find("UserInterface/MenuContent/Screens/UserInfo"), MenuButtonType.PlaylistButton, "Avatar Author", 1389f, -394f, new Action(() =>
            {
                Transform UserScreen = GameObject.Find("Screens").transform.Find("UserInfo");
                PageUserInfo Infos = UserScreen.transform.GetComponentInChildren<PageUserInfo>();
                APIUser User = Infos.field_Private_APIUser_0;
                RippingHandler.GetAuthorFromPicture(User).Start();
            }));

            new MenuButton(GameObject.Find("UserInterface/MenuContent/Screens/UserInfo"), MenuButtonType.PlaylistButton, "Copy Bio", 1389f, -473f, new Action(() =>
            {
                Transform UserScreen = GameObject.Find("Screens").transform.Find("UserInfo");
                PageUserInfo Infos = UserScreen.transform.GetComponentInChildren<PageUserInfo>();
                APIUser User = Infos.field_Private_APIUser_0;

                Clipboard.SetText(User.bio);
                Extensions.Logger.Log(User.bio, Extensions.Logger.LogsType.Info);
            }));

            new MenuButton(GameObject.Find("UserInterface/MenuContent/Screens/UserInfo"), MenuButtonType.PlaylistButton, "Copy Status", 1389f, -552f, new Action(() =>
            {
                Transform UserScreen = GameObject.Find("Screens").transform.Find("UserInfo");
                PageUserInfo Infos = UserScreen.transform.GetComponentInChildren<PageUserInfo>();
                APIUser User = Infos.field_Private_APIUser_0;

                Clipboard.SetText(User.statusDescription);
                Extensions.Logger.Log(User.statusDescription, Extensions.Logger.LogsType.Info);
            }));

            new MenuButton(GameObject.Find("UserInterface/MenuContent/Screens/UserInfo"), MenuButtonType.PlaylistButton, "Copy UserID", 1389f, -631f, new Action(() =>
            {
                Transform UserScreen = GameObject.Find("Screens").transform.Find("UserInfo");
                PageUserInfo Infos = UserScreen.transform.GetComponentInChildren<PageUserInfo>();
                APIUser User = Infos.field_Private_APIUser_0;

                Clipboard.SetText(User.UserID());
                Extensions.Logger.Log(User.UserID(), Extensions.Logger.LogsType.Info);
            }));

            new MenuButton(GameObject.Find("UserInterface/MenuContent/Screens/UserInfo"), MenuButtonType.PlaylistButton, "Open Steam", 119f, -747f, new Action(() =>
            {
                Transform UserScreen = GameObject.Find("Screens").transform.Find("UserInfo");
                PageUserInfo Infos = UserScreen.transform.GetComponentInChildren<PageUserInfo>();

                if (Infos.field_Private_APIUser_0.username.Contains("steam_"))
                {
                    string[] array = Infos.field_Private_APIUser_0.username.Split('_');
                    Process.Start($"https://steamcommunity.com/profiles/{array[1]}");
                }
            }));

            new MenuButton(GameObject.Find("UserInterface/MenuContent/Screens/UserInfo"), MenuButtonType.PlaylistButton, "Open VRChat", 339f, -747f, new Action(() =>
            {
                Transform UserScreen = GameObject.Find("Screens").transform.Find("UserInfo");
                PageUserInfo Infos = UserScreen.transform.GetComponentInChildren<PageUserInfo>();
                Process.Start($"https://vrchat.com/home/user/{Infos.field_Private_APIUser_0.UserID()}");
            }));

            new MenuButton(GameObject.Find("UserInterface/MenuContent/Screens/UserInfo"), MenuButtonType.PlaylistButton, "Drop Portal", 339f, -667f, new Action(() =>
            {
                Transform UserScreen = GameObject.Find("Screens").transform.Find("UserInfo");
                PageUserInfo Infos = UserScreen.transform.GetComponentInChildren<PageUserInfo>();

                APIUser APIUser = Infos.field_Private_APIUser_0;
                if (APIUser.location.StartsWith("wrld_")) PortalHandler.DropPortalToWorld(Utils.CurrentUser, APIUser.location);
            }));

            new MenuButton(GameObject.Find("UserInterface/MenuContent/Screens/UserInfo"), MenuButtonType.PlaylistButton, "Log", 119f, -667f, new Action(() =>
            {
                Transform UserScreen = GameObject.Find("Screens").transform.Find("UserInfo");
                PageUserInfo Infos = UserScreen.transform.GetComponentInChildren<PageUserInfo>();

                 string Log = string.Concat(new object[]
                    {
                        Environment.NewLine,
                        $"======= Player Informations =======", Environment.NewLine,
                        $"Displayname: {Infos.field_Private_APIUser_0.DisplayName()}", Environment.NewLine,
                        $"Username: {Infos.field_Private_APIUser_0.username}", Environment.NewLine,
                        $"UserID: {Infos.field_Private_APIUser_0.UserID()}", Environment.NewLine,
                        $"Platform: {Infos.field_Private_APIUser_0.GetPlatform()}", Environment.NewLine,
                        $"Status: {Infos.field_Private_APIUser_0.status}", Environment.NewLine,
                        $"State: {Infos.field_Private_APIUser_0.state}", Environment.NewLine,
                        $"Last Login: {Infos.field_Private_APIUser_0.last_login}", Environment.NewLine,
                        $"Rank: {Infos.field_Private_APIUser_0.GetRank()}", Environment.NewLine,
                        $"Creation: {Infos.field_Private_APIUser_0.date_joined}", Environment.NewLine,
                        $"Avatar Copy: {Infos.field_Private_APIUser_0.allowAvatarCopying}", Environment.NewLine,
                        $"Current VRC+: {Infos.field_Private_APIUser_0.isSupporter}", Environment.NewLine,
                        $"Early VRC+: {Infos.field_Private_APIUser_0.isEarlyAdopter}", Environment.NewLine,
                        $"User Icon: {Infos.field_Private_APIUser_0.userIcon}", Environment.NewLine,
                        $"User Picture: {Infos.field_Private_APIUser_0.profilePicImageUrl}", Environment.NewLine,
                        $"Links: {string.Join(" ", Infos.field_Private_APIUser_0.bioLinks.ToArray())}", Environment.NewLine,
                        $"Languages: {string.Join(" ", Infos.field_Private_APIUser_0.languagesDisplayNames.ToArray())}", Environment.NewLine,
                        $"Tags: {string.Join(" ", Infos.field_Private_APIUser_0.tags.ToArray())}", Environment.NewLine,
                         "====================================",
                    });

                    Extensions.Logger.Log(Log, Extensions.Logger.LogsType.Clean);
                    Utils.VRCUiPopupManager.AlertNotification(Infos.field_Private_APIUser_0.DisplayName(), Log, "Copy", new Action(() => 
                    {
                        Clipboard.SetText(Log);
                        Utils.VRCUiPopupManager.HideCurrentPopUp(); 
                    }));
            }));
        }
    }
}