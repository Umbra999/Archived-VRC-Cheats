using CoreRuntime.Manager;
using Hexed.Core;
using Hexed.Interfaces;
using VRC.SDK3.Video.Components;
using VRC.SDKBase;

namespace Hexed.Hooking
{
    internal class VRCUnityVideoPlayer_LoadURL : IHook
    {
        private delegate void _LoadURLDelegate(nint instance, nint __0);
        private static _LoadURLDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_LoadURLDelegate>(typeof(VRCUnityVideoPlayer).GetMethod(nameof(VRCUnityVideoPlayer.LoadURL)), Patch);
        }

        private static void Patch(nint instance, nint __0)
        {
            VRCUrl url = __0 == nint.Zero ? null : new(__0);

            if (url == null) return;

            if (InternalSettings.NoVideoPlayer)
            {
                Wrappers.Logger.Log($"Video tried loading from {url.url}", Wrappers.Logger.LogsType.Protection);
                return;
            }

            Wrappers.Logger.Log($"Video loading from {url.url}", Wrappers.Logger.LogsType.Room);

            originalMethod(instance, __0);
        }
    }
}
