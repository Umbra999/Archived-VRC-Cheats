using LunaR.Objects;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using VRC.Core;
using VRC.UI;

namespace LunaR.Modules
{
    internal class RippingHandler
    {
        public static IEnumerator DownloadAvatar(ApiAvatar avatar)
        {
            if (avatar.assetUrl != null)
            {
                UnityWebRequest www = UnityWebRequest.Get(avatar.assetUrl);
                www.SendWebRequest();
                while (!www.isDone) yield return new WaitForEndOfFrame();
                File.WriteAllBytes("LunaR\\VRCA\\" + avatar.name + ".vrca", www.downloadHandler.data);
            }

            if (avatar.imageUrl != null || avatar.thumbnailImageUrl != null)
            {
                UnityWebRequest www = UnityWebRequest.Get(avatar.imageUrl == null ? avatar.thumbnailImageUrl : avatar.imageUrl);
                www.SendWebRequest();
                while (!www.isDone) yield return new WaitForEndOfFrame();
                File.WriteAllBytes("LunaR\\VRCA\\" + avatar.name + ".png", www.downloadHandler.data);
            }

            Extensions.Logger.Log($"Downloaded {avatar.name} by {avatar.authorName}", Extensions.Logger.LogsType.Info);
            Extensions.Logger.Log($"AssetURL: {avatar.assetUrl}", Extensions.Logger.LogsType.Info);
            Extensions.Logger.Log($"ImageURL: {avatar.imageUrl}", Extensions.Logger.LogsType.Info);
            Extensions.Logger.Log($"Thumbnail: {avatar.thumbnailImageUrl}", Extensions.Logger.LogsType.Info);
            Extensions.Logger.Log($"AvatarID: {avatar.id}", Extensions.Logger.LogsType.Info);
        }

        public static IEnumerator DownloadWorld(ApiWorld world)
        {
            if (world.assetUrl != null)
            {
                UnityWebRequest www = UnityWebRequest.Get(world.assetUrl);
                www.SendWebRequest();
                while (!www.isDone) yield return new WaitForEndOfFrame();
                File.WriteAllBytes("LunaR\\VRCA\\" + world.name + ".vrcw", www.downloadHandler.data);
            }

            if (world.imageUrl != null || world.thumbnailImageUrl != null)
            {
                UnityWebRequest www = UnityWebRequest.Get(world.imageUrl == null ? world.thumbnailImageUrl : world.imageUrl);
                www.SendWebRequest();
                while (!www.isDone) yield return new WaitForEndOfFrame();
                File.WriteAllBytes("LunaR\\VRCA\\" + world.name + ".png", www.downloadHandler.data);
            }
        }

        public static bool CheckStolenAvatar(string AvatarFile)
        {
            AssetBundleObject Bundle = JsonConvert.DeserializeObject<AssetBundleObject>(AvatarFile);

            string[] Samples = Bundle.name.Split(' ');
            if (Samples[0] != "Avatar") return true;
            if (Samples[1] != "-") return true;
            if (Samples[Samples.Length - 5] != "-") return true;
            if (Samples[Samples.Length - 4] != "Asset") return true;
            if (Samples[Samples.Length - 3] != "bundle") return true;
            if (Samples[Samples.Length - 2] != "-") return true;
            if (!Samples[Samples.Length - 1].EndsWith("_Release")) return true;

            if (Bundle.extension != ".vrca") return true;
            if (Bundle.mimeType != "application/x-avatar") return true;
            if (Bundle.id.Length != 41) return true;
            if (!Bundle.id.StartsWith("file_")) return true;
            if (string.IsNullOrEmpty(Bundle.ownerId)) return true;
            foreach (string Tag in Bundle.tags)
            {
                if (Tag != "author_quest_fallback" && !Tag.StartsWith("admin_")) return true;
            }

            return false;
        }

        public static bool CheckStolenAvatarImage(string ImageFile)
        {
            AssetBundleObject Image = JsonConvert.DeserializeObject<AssetBundleObject>(ImageFile);

            string[] ImageSamples = Image.name.Split(' ');
            if (ImageSamples[0] != "Avatar") return true;
            if (ImageSamples[1] != "-") return true;
            if (ImageSamples[ImageSamples.Length - 4] != "-") return true;
            if (ImageSamples[ImageSamples.Length - 3] != "Image") return true;
            if (ImageSamples[ImageSamples.Length - 2] != "-") return true;
            if (!ImageSamples[ImageSamples.Length - 1].EndsWith("_Release")) return true;

            if (Image.extension != ".png") return true;
            if (Image.mimeType != "image/png") return true;
            if (Image.id.Length != 41) return true;
            if (!Image.id.StartsWith("file_")) return true;
            if (string.IsNullOrEmpty(Image.ownerId)) return true;
            if (Image.tags.Length > 0) return true;

            return false;
        }

        public static bool CheckStolenWorld(string WorldFile)
        {
            AssetBundleObject Bundle = JsonConvert.DeserializeObject<AssetBundleObject>(WorldFile);

            string[] Samples = Bundle.name.Split(' ');
            if (Samples[0] != "World") return true;
            if (Samples[1] != "-") return true;
            if (Samples[Samples.Length - 5] != "-") return true;
            if (Samples[Samples.Length - 4] != "Asset") return true;
            if (Samples[Samples.Length - 3] != "bundle") return true;
            if (Samples[Samples.Length - 2] != "-") return true;
            if (!Samples[Samples.Length - 1].EndsWith("_Release")) return true;

            if (Bundle.extension != ".vrcw") return true;
            if (Bundle.mimeType != "application/x-world") return true;
            if (Bundle.id.Length != 41) return true;
            if (!Bundle.id.StartsWith("file_")) return true;
            if (string.IsNullOrEmpty(Bundle.ownerId)) return true;

            return false;
        }

        public static bool CheckStolenWorldImage(string ImageFile)
        {
            AssetBundleObject Image = JsonConvert.DeserializeObject<AssetBundleObject>(ImageFile);

            string[] ImageSamples = Image.name.Split(' ');
            if (ImageSamples[0] != "World") return true;
            if (ImageSamples[1] != "-") return true;
            if (ImageSamples[ImageSamples.Length - 4] != "-") return true;
            if (ImageSamples[ImageSamples.Length - 3] != "Image") return true;
            if (ImageSamples[ImageSamples.Length - 2] != "-") return true;
            if (!ImageSamples[ImageSamples.Length - 1].EndsWith("_Release")) return true;

            if (Image.extension != ".png") return true;
            if (Image.mimeType != "image/png") return true;
            if (Image.id.Length != 41) return true;
            if (!Image.id.StartsWith("file_")) return true;
            if (string.IsNullOrEmpty(Image.ownerId)) return true;
            if (Image.tags.Length > 0) return true;

            return false;
        }

        public static IEnumerator GetAuthorFromPicture(APIUser APIUser)
        {
            string URL = "https://api.vrchat.cloud/api/1/file/";
            string[] Split = APIUser.currentAvatarImageUrl.Split('/');
            URL = $"{URL}{Split[6]}";

            UnityWebRequest www = UnityWebRequest.Get(URL);
            www.SendWebRequest();
            while (!www.isDone) yield return new WaitForEndOfFrame();

            AssetBundleObject Bundle = JsonConvert.DeserializeObject<AssetBundleObject>(www.downloadHandler.text);

            APIUser.FetchUser(Bundle.ownerId, new Action<APIUser>(user =>
            {
                VRCUiPage vrcUiPage = GameObject.Find("UserInterface/MenuContent/Screens/UserInfo").GetComponent<VRCUiPage>();
                Utils.VRCUiManager.Method_Public_VRCUiPage_VRCUiPage_0(vrcUiPage);
                vrcUiPage.Cast<PageUserInfo>().Method_Public_Void_APIUser_PDM_0(user);
            }), null);
        }
    }
}