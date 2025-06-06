using CoreRuntime.Manager;
using Hexed.Core;
using Hexed.HexedServer;
using Hexed.Interfaces;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using VRC.Udon.Wrapper.Modules;

namespace Hexed.Hooking
{
    internal class ExternVRCSDKBaseVRCPlayerApi_get_displayname : IHook
    {
        private delegate void _get_displaynameDelegate(nint instance, nint __0, nint __1);
        private static _get_displaynameDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_get_displaynameDelegate>(typeof(ExternVRCSDKBaseVRCPlayerApi).GetMethod(nameof(ExternVRCSDKBaseVRCPlayerApi.__get_displayName__SystemString)), Patch);
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

                switch (InternalSettings.UdonSpoof)
                {
                    case InternalSettings.UdonSpoofMode.Owner:
                        if (RoomManager.field_Internal_Static_ApiWorld_0 != null) playerApi.displayName = RoomManager.field_Internal_Static_ApiWorld_0.authorName;
                        break;

                    case InternalSettings.UdonSpoofMode.Random:
                        playerApi.displayName = EncryptUtils.RandomString(13);
                        break;

                    case InternalSettings.UdonSpoofMode.Custom:
                        playerApi.displayName = InternalSettings.FakeUdonValue;
                        break;
                }

                heap.SetHeapVariable(address[i], playerApi);
            }

            originalMethod(instance, __0, __1);
        }
    }
}
