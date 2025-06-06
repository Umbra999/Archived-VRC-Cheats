using CoreRuntime.Manager;
using Hexed.Core;
using Hexed.Interfaces;
using VRC.UI.Elements.Controls;

namespace Hexed.Hooking
{
    internal class InstanceContentCell_SetDetails : IHook
    {
        private delegate void _SetDetailDelegate(IntPtr instance, IntPtr __0);
        private static _SetDetailDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_SetDetailDelegate>(typeof(InstanceContentCell).GetMethod(nameof(InstanceContentCell.Method_Private_Void_PDM_4)), Patch);
        }

        private static void Patch(IntPtr instance, IntPtr __0)
        {
            originalMethod(instance, __0);

            InstanceContentCell ContentCell = instance == IntPtr.Zero ? null : new(instance);

            if (ContentCell == null || ContentCell.UserCount == null) return;

            if (ContentCell.name != "Cell_MM_WorldInstance(Clone)") return;

            IWorld activeWorld = ContentCell.field_Protected_IWorld_0;
            if (activeWorld == null || activeWorld.prop_String_0 == null) return;

            if (InternalSettings.InstanceHistory.ContainsKey(activeWorld.prop_String_0)) ContentCell.UserCount.text += $" [{InternalSettings.InstanceHistory[activeWorld.prop_String_0].ToShortTimeString()}]";
        }
    }
}
