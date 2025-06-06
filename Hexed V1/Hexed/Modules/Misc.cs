using Hexed.Core;
using Hexed.CustomUI;
using Hexed.UIApi;
using Hexed.Wrappers;
using VRC;
using VRC.Core;
using UnityEngine;
using Hexed.Interfaces;

namespace Hexed.Modules
{
    internal class Misc : IGlobalModule
    {
        public void Initialize()
        {
            AddDebugLevels();
            DestroyAnalytics();
        }

        public void OnUpdate()
        {
            if (!GameUtils.IsInWorld()) return;

            if (MenuHelper.QuickMenu != null && MenuHelper.QuickMenu.isActiveAndEnabled)
            {
                TimeSpan span = TimeSpan.FromMilliseconds(GeneralUtils.GetUnixTimeInMilliseconds() - InternalSettings.joinedRoomTime);
                string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}", span.Hours, span.Minutes, span.Seconds);
                if (UIQuickChanges.CustomDebugPanel != null) UIQuickChanges.CustomDebugPanel.text = $"Room: {elapsedTime}";
            }

            if (VRCUiCursorManager.field_Private_Static_VRCUiCursorManager_0.field_Private_EnumNPublicSealedvaNoMoGaHa5vUnique_0 == VRCUiCursorManager.EnumNPublicSealedvaNoMoGaHa5vUnique.None) Camera.main.nearClipPlane = 0.001f;
            else Camera.main.nearClipPlane = 0.01f;
        }

        private static void AddDebugLevels()
        {
            if (GameUtils.IsInVr()) return;

            VRC.Core.Logger.AddDebugLevel(DebugLevel.Always);
            VRC.Core.Logger.AddDebugLevel(DebugLevel.API);
            VRC.Core.Logger.AddDebugLevel(DebugLevel.AssetBundleDownloadManager);
            VRC.Core.Logger.AddDebugLevel(DebugLevel.ContentCreator);
            VRC.Core.Logger.AddDebugLevel(DebugLevel.Errors);
            VRC.Core.Logger.AddDebugLevel(DebugLevel.NetworkTransport);
            VRC.Core.Logger.AddDebugLevel(DebugLevel.NetworkProcessing);
            VRC.Core.Logger.AddDebugLevel(DebugLevel.Warnings);
            VRC.Core.Logger.AddDebugLevel(DebugLevel.All);
        }

        private static void DestroyAnalytics()
        {
            GameObject AnalyticsManager = VRCApplication.prop_VRCApplication_0?.transform.Find("AnalyticsManager")?.gameObject;

            if (AnalyticsManager == null)
            {
                Wrappers.Logger.LogError("AnalyticsManager not found");
                return;
            }

            UnityEngine.Object.DestroyImmediate(AnalyticsManager);
            Wrappers.Logger.Log("Removed AnalyticsManager from Scene", Wrappers.Logger.LogsType.Protection);
        }

        public static void ReloadAvatars()
        {
            foreach (Player player in GameHelper.PlayerManager.GetAllPlayers())
            {
                player.GetAPIUser().ReloadAvatar();
            }
        }
    }
}
