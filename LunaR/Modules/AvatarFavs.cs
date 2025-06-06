using LunaR.Api;
using LunaR.Extensions;
using LunaR.Objects;
using LunaR.UIApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using UnityEngine;
using VRC.Core;
using VRC.UI;

namespace LunaR.Modules
{
    internal class AvatarFavs
    {
        public static void Start()
        {
            avatarPage = GameObject.Find("UserInterface/MenuContent/Screens/Avatar");
            PublicAvatarList = GameObject.Find("/UserInterface/MenuContent/Screens/Avatar/Vertical Scroll View/Viewport/Content/Public Avatar List");
            currPageAvatar = avatarPage.GetComponent<PageAvatar>();
            AvatarList = new VRCAvatarList(PublicAvatarList.transform.parent, "LunaR Favorites", 0);
            AvatarHistory = new VRCAvatarList(PublicAvatarList.transform.parent, "Avatar History", 1);
            AvatarSearch = new VRCAvatarList(PublicAvatarList.transform.parent, "Avatar Search", 2);
            AvatarObjects = JsonConvert.DeserializeObject<List<AvatarObject>>(File.ReadAllText("LunaR\\AvatarFavorites.json"));
            HistoryObjects = JsonConvert.DeserializeObject<List<AvatarObject>>(File.ReadAllText("LunaR\\AvatarHistory.json"));

            new MenuButton(GameObject.Find("UserInterface/MenuContent/Screens/Avatar"), MenuButtonType.PlaylistButton, "Fav/Unfav", 120f, -140, delegate
            {
                if (!AvatarObjects.Exists(m => m.id == currPageAvatar.field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0.id)) FavoriteAvatar(currPageAvatar.field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0);
                else UnfavoriteAvatar(currPageAvatar.field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0);
            });

            new MenuButton(GameObject.Find("UserInterface/MenuContent/Screens/Avatar"), MenuButtonType.PlaylistButton, "Show \nAuthor", 330f, -140, delegate
            {
                APIUser.FetchUser(currPageAvatar.field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0.authorId, new Action<APIUser>(user =>
                {
                    VRCUiPage vrcUiPage = GameObject.Find("UserInterface/MenuContent/Screens/UserInfo").GetComponent<VRCUiPage>();
                    VRCUiManager.prop_VRCUiManager_0.Method_Public_VRCUiPage_VRCUiPage_0(vrcUiPage);
                    vrcUiPage.Cast<PageUserInfo>().Method_Public_Void_APIUser_PDM_0(user);
                }), new Action<string>(error =>
                {
                    Extensions.Logger.LogError("Failed to get Avatar Author");
                }));
            });

            new MenuButton(GameObject.Find("UserInterface/MenuContent/Screens/Avatar"), MenuButtonType.PlaylistButton, "Copy ID", 540, -140, delegate
            {
                Clipboard.SetText(currPageAvatar.field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0.id);
                Extensions.Logger.Log(currPageAvatar.field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0.id, Extensions.Logger.LogsType.Info);
            });

            new MenuButton(GameObject.Find("UserInterface/MenuContent/Screens/Avatar"), MenuButtonType.PlaylistButton, "Download", 1000f, -140, delegate
            {
                RippingHandler.DownloadAvatar(currPageAvatar.field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0).Start();
            });

            new MenuButton(GameObject.Find("UserInterface/MenuContent/Screens/Avatar"), MenuButtonType.PlaylistButton, "Search \nName", 1210f, -140, delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("Search", "Ok", delegate (string text)
                {
                    SearchAvatar(text, false);
                });
            });

            new MenuButton(GameObject.Find("UserInterface/MenuContent/Screens/Avatar"), MenuButtonType.PlaylistButton, "Search \nAuthor", 1420f, -140, delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("UserID", "Ok", delegate (string text)
                {
                    SearchAvatar(text, true);
                });
            });
        }

        public static void UpdateList()
        {
            Il2CppSystem.Collections.Generic.List<ApiAvatar> avilist = new();
            AvatarObjects.ForEach(avi => avilist.Add(AvatarObject.ToApiAvatar(avi)));
            AvatarList.RenderElement(avilist);
            avilist.Clear();
            HistoryObjects.ForEach(avi => avilist.Add(AvatarObject.ToApiAvatar(avi)));
            AvatarHistory.RenderElement(avilist);
            avilist.Clear();
        }

        public static void SearchAvatar(string Name, bool Author)
        {
            using WebClient httpClient = new();
            httpClient.Headers.Add(HttpRequestHeader.UserAgent, "LunaR");
            string response;
            if (Author) response = httpClient.DownloadString($"https://requi.dev/vrcx_search.php?authorId={Name}&limit=900");
            else response = httpClient.DownloadString($"https://requi.dev/vrcx_search.php?searchTerm={Name}&limit=900");
            List<ApiAvatar> avatars = JsonConvert.DeserializeObject<List<ApiAvatar>>(response);
            Extensions.Logger.Log($"Found {avatars.Count} Avatars in Network", Extensions.Logger.LogsType.Info);
            var avilist = new Il2CppSystem.Collections.Generic.List<ApiAvatar>();
            foreach (ApiAvatar Avi in avatars)
            {
                if (Avi.releaseStatus == "public") avilist.Add(Avi);
            }
            AvatarSearch.RenderElement(avilist);
        }

        internal static void AddToHistory(ApiAvatar avatar)
        {
            if (!HistoryObjects.Exists(avi => avi.id == avatar.id)) HistoryObjects.Insert(0, AvatarObject.ToAvatarObject(avatar));
            else
            {
                HistoryObjects.Remove(HistoryObjects.Where(avi => avi.id == avatar.id).First());
                HistoryObjects.Insert(0, AvatarObject.ToAvatarObject(avatar));
            }
            if (HistoryObjects.Count > 20) HistoryObjects.RemoveAt(20);
            string contents = JsonConvert.SerializeObject(HistoryObjects, Formatting.Indented);
            File.WriteAllText("LunaR\\AvatarHistory.json", contents);
        }

        internal static void FavoriteAvatar(ApiAvatar avatar)
        {
            if (!AvatarObjects.Exists(avi => avi.id == avatar.id)) AvatarObjects.Insert(0, AvatarObject.ToAvatarObject(avatar));
            string contents = JsonConvert.SerializeObject(AvatarObjects, Formatting.Indented);
            File.WriteAllText("LunaR\\AvatarFavorites.json", contents);
            UpdateList();
        }

        internal static void UnfavoriteAvatar(ApiAvatar avatar)
        {
            if (AvatarObjects.Exists(avi => avi.id == avatar.id)) AvatarObjects.Remove(AvatarObjects.Where(avi => avi.id == avatar.id).FirstOrDefault());
            string contents = JsonConvert.SerializeObject(AvatarObjects, Formatting.Indented);
            File.WriteAllText("LunaR\\AvatarFavorites.json", contents);
            UpdateList();
        }

        private static VRCAvatarList AvatarList;
        private static VRCAvatarList AvatarHistory;
        private static VRCAvatarList AvatarSearch;

        public static List<AvatarObject> AvatarObjects = new();
        public static List<AvatarObject> HistoryObjects = new();
        private static GameObject avatarPage;
        private static PageAvatar currPageAvatar;
        private static GameObject PublicAvatarList;
    }
}