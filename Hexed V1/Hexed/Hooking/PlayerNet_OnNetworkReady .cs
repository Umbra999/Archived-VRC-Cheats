using CoreRuntime.Manager;
using Hexed.Core;
using Hexed.Interfaces;
using Hexed.Wrappers;

namespace Hexed.Hooking
{
    internal class PlayerNet_OnNetworkReady : IHook
    {
        private delegate void _OnNetworkReadyDelegate(nint instance);
        private static _OnNetworkReadyDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_OnNetworkReadyDelegate>(typeof(PlayerNet).GetMethod(nameof(PlayerNet.OnNetworkReady)), Patch);
        }

        private static void Patch(nint instance)
        {
            originalMethod(instance);

            PlayerNet player = instance == nint.Zero ? null : new(instance);

            if (player == null) return;

            GameManager.CreatePlayerModules(player.GetVRCPlayer());
        }
    }
}
