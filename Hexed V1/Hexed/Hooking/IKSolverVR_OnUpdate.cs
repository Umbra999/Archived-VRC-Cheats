using CoreRuntime.Manager;
using Hexed.Core;
using Hexed.Interfaces;
using Hexed.Modules.LeapMotion;
using Hexed.Wrappers;
using RootMotion.FinalIK;

namespace Hexed.Hooking
{
    internal class IKSolverVR_OnUpdate : IHook, IDesktopOnly
    {
        private delegate void _OnUpdateDelegate(nint instance);
        private static _OnUpdateDelegate originalMethod;

        public void Initialize()
        {
            if (InternalSettings.isLeapMotion) originalMethod = HookManager.Detour<_OnUpdateDelegate>(typeof(IKSolverVR).GetMethod(nameof(IKSolverVR.OnUpdate)), Patch);
        }

        private static void Patch(nint instance)
        {
            IKSolverVR IkSolver = instance == nint.Zero ? null : new(instance);

            if (IkSolver != null && GameHelper.CurrentVRCPlayer != null) LeapTracked.LateUpdateIK(IkSolver, LeapMain.GestureData.m_handsPresenses[0], LeapMain.GestureData.m_handsPresenses[1], LeapMain.LeapHands[0].transform, LeapMain.LeapHands[1].transform);

            originalMethod(instance);
        }
    }
}
