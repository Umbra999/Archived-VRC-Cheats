using CoreRuntime.Manager;
using Hexed.Interfaces;
using VRC.Core;

namespace Hexed.Hooking
{
    internal class RoomManager_SetupWorld : IHook // get rid of the hook and fix scaling properly
    {
        private delegate bool _SetupWorldDelegate(nint __0, nint __1, nint __2, int __3);
        private static _SetupWorldDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_SetupWorldDelegate>(typeof(RoomManager).GetMethod(nameof(RoomManager.Method_Public_Static_Boolean_ApiWorld_ApiWorldInstance_String_Int32_0)), Patch);
        }

        private static bool Patch(nint __0, nint __1, nint __2, int __3)
        {
            ApiWorld world = __0 == nint.Zero ? null : new(__0);

            if (world != null && world.tags.Contains("feature_avatar_scaling_disabled")) world.tags.Remove("feature_avatar_scaling_disabled");

            return originalMethod(__0, __1, __2, __3);
        }
    }
}
