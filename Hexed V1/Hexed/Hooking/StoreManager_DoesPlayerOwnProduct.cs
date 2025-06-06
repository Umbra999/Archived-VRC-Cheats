using CoreRuntime.Manager;
using Hexed.Interfaces;
using VRC.SDKBase;

namespace Hexed.Hooking
{
    internal class StoreManager_DoesPlayerOwnProduct : IHook
    {
        private delegate bool _DoesPlayerOwnProductDelegate(IntPtr instance, IntPtr __0, IntPtr __1);
        private static _DoesPlayerOwnProductDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_DoesPlayerOwnProductDelegate>(typeof(VRC.Economy.Internal.Store).GetMethod(nameof(VRC.Economy.Internal.Store.Method_Private_Boolean_VRCPlayerApi_IProduct_PDM_0)), Patch);
        }

        private static bool Patch(IntPtr instance, IntPtr __0, IntPtr __1)
        {
            VRCPlayerApi player = __0 == IntPtr.Zero ? null : new(__0);

            if (player != null && player.isLocal) return true;

            return originalMethod(instance, __0, __1);
        }
    }
}
