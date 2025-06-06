using DiscordRichPresence;
using System.Diagnostics;
using UnityEngine;
using WengaPort.Buttons;
using WengaPort.Extensions;
using WengaPort.Modules;

namespace WengaPort.Loaders
{
    class ModulesLoader
    {
        public static void Initialize()
        {
            LoadingMusic.LoadingSong.Initialize();
            ButtonsMainColor.Initialize();
            ButtonsMainColor.Initialize2();
            ButtonsMainColor.Initialize3();
            AntiMenuOverrender.UIInit();
            DiscordManager.Init();
            AvatarHider.AvatarSpoofInit();
            Nameplates.OnUI();
            if (!UnityEngine.XR.XRDevice.isPresent)
            {
                PatchManager.VRMode = false;
            } 
            PlayerList.Init();
            ESP.HighlightColor(Color.green);
            Movement.UIInit();
            MelonLoader.MelonCoroutines.Start(UIChanges.Initialize());
            Application.targetFrameRate = 144;
            MelonLoader.MelonCoroutines.Start(UIChanges.UpdateClock());
        }
    }
}
