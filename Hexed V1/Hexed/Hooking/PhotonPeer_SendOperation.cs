using CoreRuntime.Manager;
using ExitGames.Client.Photon;
using Hexed.Core;
using Hexed.Interfaces;
using Hexed.Modules.EventManagement;
using System.Runtime.InteropServices;

namespace Hexed.Hooking
{
    internal class PhotonPeer_SendOperation : IHook
    {
        private delegate bool _SendOperationDelegate(nint instance, byte __0, nint __1, SendOptions __2);
        private static _SendOperationDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_SendOperationDelegate>(typeof(PhotonPeer).GetMethods().Where(x => x.Name == "SendOperation").ToArray()[0], Patch);
        }

        private static bool Patch(nint instance, byte __0, nint __1, SendOptions __2)
        {
            Il2CppSystem.Collections.Generic.Dictionary<byte, Il2CppSystem.Object> operationParameters = __1 == nint.Zero ? null : new(__1);

            switch (__0)
            {
                case 253: // OpRaise
                    break;

                case 230: // Auth?
                    break;

                case 231: // Auth Websocket?

                case 226: // OpJoinRoom
                    OperationHandler.ChangeOperation226(operationParameters);
                    break;

                case 252: // SetProperties
                    break;
            }

            if (InternalSettings.OperationLog) Wrappers.Logger.LogOperation(__0, operationParameters, __2);

            return originalMethod(instance, __0, __1, __2);
        }
    }
}
