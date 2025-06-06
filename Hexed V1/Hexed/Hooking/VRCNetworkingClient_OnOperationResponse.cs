using Hexed.Core;
using VRC.Core;
using ExitGames.Client.Photon;
using Hexed.Modules.EventManagement;
using Hexed.Wrappers;
using CoreRuntime.Manager;
using Hexed.Interfaces;
using Hexed.Modules.Standalone;

namespace Hexed.Hooking
{
    internal class VRCNetworkingClient_OnOperationResponse : IHook
    {
        private delegate void _OnOperationResponseDelegate(IntPtr instance, IntPtr __0);
        private static _OnOperationResponseDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_OnOperationResponseDelegate>(typeof(VRCNetworkingClient).GetMethod(nameof(VRCNetworkingClient.OnOperationResponse)), Patch);
        }

        private static void Patch(IntPtr instance, IntPtr __0)
        {
            VRCNetworkingClient client = instance == nint.Zero ? null : new(instance);
            OperationResponse response = __0 == nint.Zero ? null : new(__0);

            if (response == null || client == null) return;

            if (InternalSettings.OperationResponseLog) Wrappers.Logger.LogOperationResponse(response);

            if (response.ReturnCode != 0) return;

            switch (response.OperationCode)
            {
                case 226:
                    {
                        if (client.prop_ServerConnection_0 == Photon.Realtime.ServerConnection.GameServer)
                        {
                            InternalSettings.joinedRoomTime = GeneralUtils.GetUnixTimeInMilliseconds();

                            string WorldID = GameUtils.GetCurrentWorldID();
                            if (WorldID != null)
                            {
                                if (InternalSettings.InstanceHistory.Count > 150) InternalSettings.InstanceHistory.Clear();
                                InternalSettings.InstanceHistory[WorldID] = DateTime.Now;
                            }

                            Wrappers.Logger.Log("Connected to Room", Wrappers.Logger.LogsType.Room);
                            VRConsole.Log("Connected to Room", VRConsole.LogsType.Room);

                            GameManager.DestroyAllPlayerModules();
                            EventSanitizer.ClearEventBlocks();
                            ModerationHandler.ClearModerations();
                            InternalSettings.ActorsWithLastActiveTime.Clear();
                            PhotonUtils.networkedProperties.Clear();
                        }
                    }
                    break;

                case 254:
                    {
                        Wrappers.Logger.Log("Disconnected from Room", Wrappers.Logger.LogsType.Room);
                        VRConsole.Log("Disconnected from Room", VRConsole.LogsType.Room);
                    }
                    break;
            }

            originalMethod(instance, __0);
        }
    }
}
