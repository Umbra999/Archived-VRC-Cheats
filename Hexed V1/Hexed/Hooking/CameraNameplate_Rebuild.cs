using CoreRuntime.Manager;
using Hexed.Core;
using Hexed.Interfaces;
using Hexed.Modules;
using Hexed.Wrappers;

namespace Hexed.Hooking
{
    internal class CameraNameplate_Rebuild
    {
        private delegate void _RebuildPlateDelegate(nint instance);
        private static _RebuildPlateDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_RebuildPlateDelegate>(typeof(CameraNameplate).GetMethod(nameof(CameraNameplate.Method_Private_Void_2)), Patch);
        }

        private static void Patch(nint instance)
        {
            CameraNameplate nameplate = instance == nint.Zero ? null : new(instance);
            if (nameplate != null)
            {
                IPlayerModule module = GameManager.GetPlayerModuleByClass(nameplate.field_Private_VRCPlayer_0.GetPhotonPlayer().ActorID(), typeof(PlayerNameplates));
                if (module != null)
                {
                    PlayerNameplates nameplateModule = (PlayerNameplates)module;
                    nameplateModule.UpdateCameraPlate(nameplate);
                }
            }

            originalMethod(instance);
        }
    }
}
