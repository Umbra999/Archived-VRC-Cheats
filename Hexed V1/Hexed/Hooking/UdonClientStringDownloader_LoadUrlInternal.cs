using CoreRuntime.Manager;
using Hexed.Core;
using Hexed.Interfaces;
using VRC.Networking;
using VRC.SDKBase;

namespace Hexed.Hooking
{
    internal class UdonClientStringDownloader_LoadUrlInternal : IHook
    {
        private delegate void _LoadURLInternalDelegate(nint __0, nint __1);
        private static _LoadURLInternalDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_LoadURLInternalDelegate>(typeof(UdonClientStringDownloader).GetMethod(nameof(UdonClientStringDownloader.Method_Private_Static_Void_VRCUrl_IUdonEventReceiver_PDM_0)), Patch);
        }

        private static void Patch(nint __0, nint __1)
        {
            VRCUrl url = __0 == nint.Zero ? null : new(__0);

            if (url != null)
            {
                if (InternalSettings.NoUdonDownload)
                {
                    Wrappers.Logger.Log($"Udon tried to download a string from {url.url}", Wrappers.Logger.LogsType.Protection);
                    return;
                }

                Wrappers.Logger.Log($"Udon downloaded a string from {url.url}", Wrappers.Logger.LogsType.Room);
            }

            originalMethod(__0, __1);
        }
    }
}
