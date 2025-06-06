using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;

namespace WengaPort.LoadingMusic
{
    class LoadingSong
    {
        public static void Initialize()
        {
            if (!File.Exists("WengaPort\\LoadingMusic.ogg"))
            {
                using WebClient webClient = new WebClient();
                webClient.DownloadFile("https://cdn.discordapp.com/attachments/797668950883565638/804088968475312168/LoadingMusic.ogg", Path.Combine(System.Environment.CurrentDirectory, "WengaPort/LoadingMusic.ogg"));
            }

            var audioLoader = new WWW(string.Format("file://{0}", string.Concat(Directory.GetCurrentDirectory(), "/WengaPort/LoadingMusic.ogg")).Replace("\\", "/"));
            while (audioLoader.progress < 1)
                Thread.Sleep(1);
            var newMenuClip = audioLoader.GetAudioClip(false, false, AudioType.OGGVORBIS);
            newMenuClip.name = "New Music";
            VRCUiManager.prop_VRCUiManager_0.transform.Find("LoadingBackground_TealGradient_Music/LoadingSound")
                .GetComponent<AudioSource>().clip = newMenuClip;
            VRCUiManager.prop_VRCUiManager_0.transform.Find("MenuContent/Popups/LoadingPopup/LoadingSound")
                .GetComponent<AudioSource>().clip = newMenuClip;
            VRCUiManager.prop_VRCUiManager_0.transform.Find("LoadingBackground_TealGradient_Music/LoadingSound")
                .GetComponent<AudioSource>().Play();
            VRCUiManager.prop_VRCUiManager_0.transform.Find("MenuContent/Popups/LoadingPopup/LoadingSound")
                .GetComponent<AudioSource>().Play();
        }
    }
}
