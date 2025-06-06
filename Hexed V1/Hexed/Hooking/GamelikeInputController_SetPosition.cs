using CoreRuntime.Manager;
using Hexed.Interfaces;
using Hexed.Modules;
using Hexed.Wrappers;

namespace Hexed.Hooking
{
    internal class GamelikeInputController_SetPosition : IHook, IDesktopOnly
    {
        private delegate void _SetPositionDelegate(nint instance);
        private static _SetPositionDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_SetPositionDelegate>(typeof(GamelikeInputController).GetMethod(nameof(GamelikeInputController.Method_Private_Void_0)), Patch);
        }

        private static void Patch(nint instance)
        {
            GamelikeInputController controller = instance == nint.Zero ? null : new(instance);

            if (Movement.isRotateEnabled) return;

            originalMethod(instance);
        }
    }
}
