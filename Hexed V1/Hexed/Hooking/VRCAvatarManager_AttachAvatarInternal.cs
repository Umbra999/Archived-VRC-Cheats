using CoreRuntime.Manager;
using Hexed.Interfaces;
using Hexed.Modules.Standalone;
using UnityEngine;

namespace Hexed.Hooking
{
    internal class VRCAvatarManager_AttachAvatarInternal : IHook
    {
        private delegate bool _AttachAvatarInternalDelegate(IntPtr instance, IntPtr __0, IntPtr __1, float __2, IntPtr __3, bool __4);
        private static _AttachAvatarInternalDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_AttachAvatarInternalDelegate>(typeof(VRCAvatarManager).GetMethod(nameof(VRCAvatarManager.Method_Private_Boolean_GameObject_String_Single_ApiAvatar_Boolean_0)), Patch);
        }

        private static bool Patch(IntPtr instance, IntPtr __0, IntPtr __1, float __2, IntPtr __3, bool __4)
        {
            VRCAvatarManager manager = instance == nint.Zero ? null : new(instance);
            GameObject Avatar = __0 == nint.Zero ? null : new(__0);
            string name = __1 == IntPtr.Zero ? null : new Il2CppSystem.Object(__1).ToString();

            if (Avatar != null && name.StartsWith("https://")) AvatarSanitizer.SanitizeAvatarObject(Avatar, manager.field_Private_VRCPlayer_0);

            bool returnValue = originalMethod(instance, __0, __1, __2, __3, __4);

            return returnValue;
        }
    }
}
