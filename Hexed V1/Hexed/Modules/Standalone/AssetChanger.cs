using UnityEngine.Networking;
using UnityEngine;
using System.Collections;
using Hexed.Wrappers;
using Hexed.UIApi;
using Hexed.Core;

namespace Hexed.Modules.Standalone
{
    internal class AssetChanger
    {
        public static void Initialize()
        {
            if (File.Exists(ConfigManager.BaseFolder + "\\Assets\\LoadingMusic.ogg")) LoadCustomMusic().Start();
            if (File.Exists(ConfigManager.BaseFolder + "\\Assets\\HideRobot.vrca")) LoadCustomHidebot().Start();
        }

        private static IEnumerator LoadCustomHidebot()
        {
            Wrappers.Logger.Log("Found custom Hide Robot", Wrappers.Logger.LogsType.Info);
            UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(string.Format("file://{0}", string.Concat(Directory.GetCurrentDirectory(), ConfigManager.BaseFolder + "\\Assets\\HideRobot.vrca")).Replace("\\", "/"));
            www.SendWebRequest();
            while (!www.isDone) yield return new WaitForEndOfFrame();

            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            string prefab_path = bundle.GetAllAssetNames().Where(x => x.EndsWith(".prefab")).FirstOrDefault();

            if (prefab_path == null)
            {
                Wrappers.Logger.LogError("Failed to find prefab in Assetbundle");
                yield break;
            }

            GameObject NewHideObject = bundle.LoadAsset<GameObject>(prefab_path);
            NewHideObject.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            VRCAvatarManager[] avatarManager = Resources.FindObjectsOfTypeAll<VRCAvatarManager>();
            foreach (VRCAvatarManager manager in avatarManager)
            {
                manager.field_Public_GameObject_1 = NewHideObject;  // Error
                manager.field_Public_GameObject_2 = NewHideObject;  // Hide
            }
        }

        public static IEnumerator LoadCustomMusic()
        {
            UnityWebRequest audioLoader = UnityWebRequest.Get(string.Format("file://{0}", string.Concat(Directory.GetCurrentDirectory(), ConfigManager.BaseFolder + "\\Assets\\LoadingMusic.ogg")).Replace("\\", "/"));
            audioLoader.SendWebRequest();

            while (!audioLoader.isDone) yield return new WaitForEndOfFrame();

            if (!audioLoader.isHttpError)
            {
                AudioClip t = WebRequestWWW.InternalCreateAudioClipUsingDH(audioLoader.downloadHandler, audioLoader.url, false, false, 0);
                t.hideFlags |= HideFlags.DontUnloadUnusedAsset;

                while (MenuHelper.userInterface == null) yield return new WaitForEndOfFrame();

                while (MenuHelper.userInterface.Find("LoadingBackground_TealGradient_Music/LoadingSound") == null) yield return new WaitForEndOfFrame();

                AudioSource _audioSource = MenuHelper.userInterface.Find("LoadingBackground_TealGradient_Music/LoadingSound").GetComponent<AudioSource>();
                ReplaceClip(_audioSource, t);

                while (MenuHelper.userInterface.Find("MenuContent/Popups/LoadingPopup/LoadingSound")?.gameObject?.active == false) yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();

                AudioSource _audioSource1 = MenuHelper.userInterface.Find("MenuContent/Popups/LoadingPopup/LoadingSound").GetComponent<AudioSource>();
                ReplaceClip(_audioSource1, t);

                Wrappers.Logger.Log("Initialized custom Loading Music", Wrappers.Logger.LogsType.Info);
            }
        }

        private static void ReplaceClip(AudioSource src, AudioClip t)
        {
            UnityEngine.Object.Destroy(src.GetComponent<VRCUiPageLoadingMusicController>());
            src.Stop();
            src.clip = t;
            src.loop = true;
            src.playOnAwake = true;
            src.Play();
        }
    }
}
