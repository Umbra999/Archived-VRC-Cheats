using CoreRuntime.Manager;
using Hexed.Core;
using Hexed.Interfaces;
using VRC.Networking;
using VRC.SDKBase;

namespace Hexed.Hooking
{
    internal class UdonClientImageDownloader_LoadUrlInternal : IHook
    {
        private delegate nint _LoadURLInternalDelegate(nint __0, nint __1, nint __2, nint __3);
        private static _LoadURLInternalDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_LoadURLInternalDelegate>(typeof(UdonClientImageDownloader).GetMethod(nameof(UdonClientImageDownloader.Method_Public_Static_IVRCImageDownload_VRCUrl_Material_IUdonEventReceiver_TextureInfo_PDM_0)), Patch);
        }

        private static nint Patch(nint __0, nint __1, nint __2, nint __3)
        {
            VRCUrl url = __0 == nint.Zero ? null : new(__0);

            if (url != null)
            {
                if (InternalSettings.NoUdonDownload)
                {
                    Wrappers.Logger.Log($"Udon tried to download a image from {url.url}", Wrappers.Logger.LogsType.Protection);
                    return nint.Zero;
                }

                Wrappers.Logger.Log($"Udon downloaded a image from {url.url}", Wrappers.Logger.LogsType.Room);
            }

            return originalMethod(__0, __1, __2, __3);
        }
    }
}
