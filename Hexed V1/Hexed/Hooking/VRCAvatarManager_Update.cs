using CoreRuntime.Manager;
using Hexed.Core;
using Hexed.Interfaces;
using Hexed.Wrappers;

namespace Hexed.Hooking
{
    internal class VRCAvatarManager_Update : IHook
    {
        private delegate void _UpdateDelegate(nint instance);
        private static _UpdateDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_UpdateDelegate>(typeof(VRCAvatarManager).GetMethod(nameof(VRCAvatarManager.Update)), Patch);
        }

        private static void Patch(IntPtr instance)
        {
            originalMethod(instance);

            VRCAvatarManager avatarManager = instance == IntPtr.Zero ? null : new VRCAvatarManager(instance);
            if (avatarManager == null) return;

            VRCPlayer player = avatarManager.field_Private_VRCPlayer_0;
            if (player == null) return;

            GameManager.UpdatePlayerModules(player.GetPhotonPlayer().ActorID());
        }
    }
}
