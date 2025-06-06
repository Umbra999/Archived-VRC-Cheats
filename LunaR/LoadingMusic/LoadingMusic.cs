using System.Collections;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

namespace LunaR.LoadingMusic
{
    internal class LoadingSong
    {
        public static IEnumerator Initialize()
        {
            if (!File.Exists("LunaR\\LoadingMusic.ogg"))
            {
                using WebClient webClient = new();
                webClient.DownloadFile("https://cdn.discordapp.com/attachments/846666528626704415/846682057023029269/LoadingMusic.ogg", Path.Combine(System.Environment.CurrentDirectory, "LunaR/LoadingMusic.ogg"));
            }

            var audioLoader = UnityWebRequest.Get(string.Format("file://{0}", string.Concat(Directory.GetCurrentDirectory(), "/LunaR/LoadingMusic.ogg")).Replace("\\", "/"));
            audioLoader.SendWebRequest();
            while (!audioLoader.isDone) yield return new WaitForEndOfFrame();
            AudioClip myClip = WebRequestWWW.InternalCreateAudioClipUsingDH(audioLoader.downloadHandler, audioLoader.url, false, false, 0);
            VRCUiManager.prop_VRCUiManager_0.transform.Find("LoadingBackground_TealGradient_Music/LoadingSound").GetComponent<AudioSource>().clip = myClip;
            VRCUiManager.prop_VRCUiManager_0.transform.Find("MenuContent/Popups/LoadingPopup/LoadingSound").GetComponent<AudioSource>().clip = myClip;
            VRCUiManager.prop_VRCUiManager_0.transform.Find("LoadingBackground_TealGradient_Music/LoadingSound").GetComponent<AudioSource>().Play();
            VRCUiManager.prop_VRCUiManager_0.transform.Find("MenuContent/Popups/LoadingPopup/LoadingSound").GetComponent<AudioSource>().Play();
        }
    }
}