using CoreRuntime.Manager;
using Hexed.Core;
using Hexed.Wrappers;
using VRC.Udon.Common.Interfaces;
using VRC.Udon.Wrapper.Modules;
using VRC.SDKBase;
using Hexed.Interfaces;
using Hexed.Modules.Standalone;

namespace Hexed.Hooking
{
    internal class ExternVRCSDKBaseVRCPlayerApi_SetManuelAvatarScalingAllowed : IHook
    {
        private delegate void _SetManuelAvatarScalingAllowedDelegate(nint instance, nint __0, nint __1);
        private static _SetManuelAvatarScalingAllowedDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_SetManuelAvatarScalingAllowedDelegate>(typeof(ExternVRCSDKBaseVRCPlayerApi).GetMethod(nameof(ExternVRCSDKBaseVRCPlayerApi.__SetManualAvatarScalingAllowed__SystemBoolean__SystemVoid)), Patch);
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
