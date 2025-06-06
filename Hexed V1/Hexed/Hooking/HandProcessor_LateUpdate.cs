using CoreRuntime.Manager;
using Hexed.Core;
using Hexed.Interfaces;
using Hexed.Modules.LeapMotion;
using Hexed.Wrappers;
using VRC.FingerTracking;

namespace Hexed.Hooking
{
    internal class HandProcessor_LateUpdate : IHook, IDesktopOnly
    {
        private delegate void _LateUpdateDelegate(nint instance);
        private static _LateUpdateDelegate originalMethod;

        public void Initialize()
        {
            if (InternalSettings.isLeapMotion) originalMethod = HookManager.Detour<_LateUpdateDelegate>(typeof(HandProcessor).GetMethod(nameof(HandProcessor.LateUpdate)), Patch);
        }

        private static void Patch(nint instance)
        {
            HandProcessor handProcessor = instance == nint.Zero ? null : new HandProcessor(instance);

            if (handProcessor != null && GameHelper.CurrentVRCPlayer != null) LeapTracked.UpdateFingers(handProcessor, LeapMain.GestureData.m_handsPresenses[0], LeapMain.GestureData.m_handsPresenses[1]);
            else originalMethod(instance);
        }
    }
}
