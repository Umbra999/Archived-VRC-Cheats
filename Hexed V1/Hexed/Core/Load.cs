using BestHTTP;
using CoreRuntime.Interfaces;
using Hexed.Core;
using Hexed.CustomUI;
using Hexed.Extensions;
using Hexed.HexedServer;
using Hexed.Modules.LeapMotion;
using Hexed.Modules.Standalone;
using Hexed.UIApi;
using Hexed.Wrappers;
using Sentry.Unity;
using System.Collections;
using System.Diagnostics;

namespace Hexed.Loader
{
    public class Load : HexedMod
    {
        public override void OnEntry(string[] args)
        {
            string UnityLoaderPath = args == null ? new FileInfo(FilePath).Directory.Parent.Parent.ToString() : new FileInfo(args[0]).Directory.Parent.Parent.ToString();

            if (!ServerHandler.Init(UnityLoaderPath))
            {
                Process.GetCurrentProcess().Kill();
                return;
            }

            CoreRuntime.Manager.MonoManager.PatchUpdate(typeof(VRCApplication).GetMethod(nameof(VRCApplication.Update)));
            CoreRuntime.Manager.MonoManager.PatchOnApplicationQuit(typeof(SentryMonoBehaviour).GetMethod(nameof(SentryMonoBehaviour.OnApplicationQuit)));

            LoadMenu(UnityLoaderPath).Start();
        }

        public override void OnApplicationQuit()
        {
            BotConnection.StopBot();
        }

        public override void OnUpdate()
        {
            GameManager.UpdateGlobalModules();
        }

        private static IEnumerator LoadMenu(string UnityLoaderPath)
        {
            while (VRCUiManager.prop_VRCUiManager_0 == null) yield return null;

            ConfigManager.Init(UnityLoaderPath);
            ExternalConsole.Init();

            Logger.Log($"VRChat running in {(GameUtils.IsInVr() ? "VR" : "DESKTOP")} mode", Logger.LogsType.Info);

            GameManager.CreateHooks();
            GameManager.CreateGlobalModules();

            LoadExtensions();

            while (MenuHelper.QuickMenu == null || MenuHelper.MainMenu == null || !MenuHelper.QuickMenu.isActiveAndEnabled) yield return null;

            CustomUI.QM.MainMenu.Init();
            UIQuickChanges.ApplyLateChanges();
        }

        private static void LoadExtensions()
        {
            HighlightHelper.Init();
            UIQuickChanges.ChangeTrustColors();
            AssetChanger.Initialize();
            UIQuickChanges.ApplyEarlyChanges();

            if (!GameUtils.IsInVr())
            {
                LeapMain.Init();
            }
        }
    }
}
