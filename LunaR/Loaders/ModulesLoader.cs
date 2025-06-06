using LunaR.Buttons;
using LunaR.Modules;
using LunaR.Wrappers;
using UnityEngine;
using VRC.Core;

namespace LunaR.Loaders
{
    internal class ModulesLoader
    {
        public static void Initialize()
        {
            LoadingMusic.LoadingSong.Initialize().Start();
            UIChanges.HudElements();
            UIChanges.ColorScreens();
            Nameplates.Init();
            RememberMe.Init();
            AssetChanger.Init();
            ESP.HighlightColor(new Color(25, 1, 97, 0.8f) / 3);
            ESP.CoreColor(Color.blue);
            UnityPlayerFix.ApplyPatches();
            AvatarFavs.Start();
            UIChanges.ChangeTrustColors();
            UIChanges.ChangePages();
            if (!GeneralWrappers.IsInVr())
            {
                VRC.Core.Logger.AddDebugLevel(DebugLevel.Always);
                VRC.Core.Logger.AddDebugLevel(DebugLevel.API);
                VRC.Core.Logger.AddDebugLevel(DebugLevel.AssetBundleDownloadManager);
                VRC.Core.Logger.AddDebugLevel(DebugLevel.ContentCreator);
                VRC.Core.Logger.AddDebugLevel(DebugLevel.Errors);
                VRC.Core.Logger.AddDebugLevel(DebugLevel.NetworkTransport);
                VRC.Core.Logger.AddDebugLevel(DebugLevel.NetworkProcessing);
                VRC.Core.Logger.AddDebugLevel(DebugLevel.Warnings);
            }
        }
    }
}