using VRC;
using VRC.Core;
using VRC.Management;

namespace Hexed.Wrappers
{
    internal class GameHelper
    {
        public static VRCPlayer CurrentVRCPlayer
        {
            get
            {
                return VRCPlayer.field_Internal_Static_VRCPlayer_0;
            }
        }

        public static Player CurrentPlayer
        {
            get
            {
                return Player.prop_Player_0;
            }
        }

        public static NetworkManager NetworkManager
        {
            get
            {
                return NetworkManager.field_Internal_Static_NetworkManager_0;
            }
        }

        public static PlayerManager PlayerManager
        {
            get
            {
                return PlayerManager.prop_PlayerManager_0;
            }
        }

        public static ModerationManager ModerationManager
        {
            get
            {
                return ModerationManager.prop_ModerationManager_0;
            }
        }

        public static VRCNetworkingClient VRCNetworkingClient
        {
            get
            {
                return VRCNetworkingClient.field_Internal_Static_VRCNetworkingClient_0;
            }
        }

        public static VRCUiManager VRCUiManager
        {
            get
            {
                return VRCUiManager.prop_VRCUiManager_0;
            }
        }

        public static VRCUiPopupManager VRCUiPopupManager
        {
            get
            {
                return VRCUiPopupManager.prop_VRCUiPopupManager_0;
            }
        }
    }
}
