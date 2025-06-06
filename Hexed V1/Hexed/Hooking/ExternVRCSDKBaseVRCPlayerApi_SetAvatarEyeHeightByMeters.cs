using CoreRuntime.Manager;
using Hexed.Core;
using Hexed.Interfaces;
using Hexed.Modules.Standalone;
using Hexed.Wrappers;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using VRC.Udon.Wrapper.Modules;

namespace Hexed.Hooking
{
    internal class ExternVRCSDKBaseVRCPlayerApi_SetAvatarEyeHeightByMeters : IHook
    {
        private delegate void _SetAvatarEyeHeightByMetersDelegate(nint instance, nint __0, nint __1);
        private static _SetAvatarEyeHeightByMetersDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_SetAvatarEyeHeightByMetersDelegate>(typeof(ExternVRCSDKBaseVRCPlayerApi).GetMethod(nameof(ExternVRCSDKBaseVRCPlayerApi.__SetAvatarEyeHeightByMeters__SystemSingle__SystemVoid)), Patch);
        }

        private static void Patch(nint instance, nint __0, nint __1)
        {
            IUdonHeap heap = __0 == nint.Zero ? null : new IUdonHeap(__0);
            var address = __1 == nint.Zero ? null : new Il2CppSystem.Span<uint>(__1).ToArray();

            for (int i = 0; i < address.Length; i++)
            {
                Il2CppSystem.Object heapObject = heap.GetHeapVariable(address[i]);
                if (heapObject == null) continue;

                VRCPlayerApi playerApi = heapObject.TryCast<VRCPlayerApi>();
                if (playerApi == null) continue;

                if (!playerApi.isLocal) continue;

                if (InternalSettings.NoUdonScaling)
                {
                    Logger.Log($"Prevented Server from scaling your Avatar", Logger.LogsType.Protection);
                    VRConsole.Log($"Server --> Avatar Scaling", VRConsole.LogsType.Protection);
                    return;
                }
            }

            originalMethod(instance, __0, __1);
        }
    }
}
