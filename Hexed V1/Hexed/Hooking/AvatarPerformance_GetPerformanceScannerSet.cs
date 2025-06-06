using CoreRuntime.Manager;
using Hexed.Core;
using Hexed.Interfaces;
using VRC.SDKBase.Validation.Performance;

namespace Hexed.Hooking
{
    internal class AvatarPerformance_GetPerformanceScannerSet : IHook
    {
        private delegate nint _GetPerformanceScannerSetDelegate(bool __0);
        private static _GetPerformanceScannerSetDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_GetPerformanceScannerSetDelegate>(typeof(AvatarPerformance).GetMethod(nameof(AvatarPerformance.GetPerformanceScannerSet)), Patch);
        }

        private static nint Patch(bool __0)
        {
            if (InternalSettings.ShowPerformanceStats) return originalMethod(__0);

            return nint.Zero;
        }
    }
}
