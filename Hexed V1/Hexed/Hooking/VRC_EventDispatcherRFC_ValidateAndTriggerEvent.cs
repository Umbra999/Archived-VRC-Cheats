using CoreRuntime.Manager;
using Hexed.Core;
using Hexed.Interfaces;
using Hexed.Modules.EventManagement;
using Hexed.Wrappers;
using VRC.Core;
using VRC.SDKBase;

namespace Hexed.Hooking
{
    internal class VRC_EventDispatcherRFC_ValidateAndTriggerEvent : IHook
    {
        private delegate void _ValidateAndTriggerEventDelegate(nint instance, nint __0, nint __1, VRC_EventHandler.VrcBroadcastType __2, int __3, float __4);
        private static _ValidateAndTriggerEventDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_ValidateAndTriggerEventDelegate>(typeof(VRC_EventDispatcherRFC).GetMethod(nameof(VRC_EventDispatcherRFC.Method_Public_Void_Player_VrcEvent_VrcBroadcastType_Int32_Single_0)), Patch);
        }

        private static void Patch(nint instance, nint __0, nint __1, VRC_EventHandler.VrcBroadcastType __2, int __3, float __4)
        {
            VRC.Player player = __0 == nint.Zero ? null : new(__0);
            VRC_EventHandler.VrcEvent vrcEvent = __1 == nint.Zero ? null : new(__1);

            if (player == null || vrcEvent == null || EventSanitizer.IsActorEventBlocked(player.GetPhotonPlayer().ActorID(), 6)) return;

            object[] DecodedParameterBytes = new object[0];
            if (vrcEvent.ParameterBytes != null)
            {
                DecodedParameterBytes = new object[vrcEvent.ParameterBytes.Length];
                Il2CppSystem.Object[] ParamBytes = ParameterSerialization.Method_Public_Static_Il2CppReferenceArray_1_Object_Il2CppStructArray_1_Byte_0(vrcEvent.ParameterBytes);
                for (int i = 0; i < ParamBytes.Length; i++)
                {
                    if (ParamBytes[i] == null) continue;

                    try
                    {
                        DecodedParameterBytes[i] = CPP2IL.TypeSerializer.ILToManaged(ParamBytes[i]);
                    }
                    catch (Exception e)
                    {
                        Wrappers.Logger.LogError($"Failed to Serialize RPC Parameter: {e}");
                        return;
                    }
                }
            }

            if (InternalSettings.RPCLog) Wrappers.Logger.LogRPC(player, vrcEvent, __2, __3, __4, DecodedParameterBytes);

            if (!EventSanitizer.CheckDecodedRPC(player, vrcEvent, __2, __3, __4, DecodedParameterBytes)) return;

            originalMethod(instance, __0, __1, __2, __3, __4);
        }
    }
}
