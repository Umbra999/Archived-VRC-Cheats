using CoreRuntime.Manager;
using Hexed.Core;
using Hexed.Interfaces;
using Hexed.Modules;
using Hexed.Wrappers;

namespace Hexed.Hooking
{
    internal class PlayerNameplate_Rebuild : IHook
    {
        private delegate void _RebuildPlateDelegate(IntPtr instance, float __0, bool __1);
        private static _RebuildPlateDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_RebuildPlateDelegate>(typeof(PlayerNameplate).GetMethod(nameof(PlayerNameplate.Method_Private_Void_Single_Boolean_0)), Patch);
        }

        private static void Patch(IntPtr instance, float __0, bool __1)
        {
            PlayerNameplate nameplate = instance == IntPtr.Zero ? null : new(instance);
            if (nameplate != null)
            {
                VRCPlayer player = nameplate.field_Private_VRCPlayer_0;
                if (player != null)
                {
                    IPlayerModule module = GameManager.GetPlayerModuleByClass(player.GetPhotonPlayer().ActorID(), typeof(PlayerNameplates));
                    if (module != null)
                    {
                        PlayerNameplates nameplateModule = (PlayerNameplates)module;
                        nameplateModule.UpdatePlayerPlate(nameplate);
                    }
                }
            }

            originalMethod(instance, __0, __1);
        }
    }
}
