using VRC.Core;
using VRC.SDKBase;
using VRC.UI.Elements.Menus;
using VRC;
using Hexed.UIApi;

namespace Hexed.Wrappers
{
    internal static class PlayerSimplifier
    {
        public static APIUser GetAPIUser(this Player Instance)
        {
            return Instance.prop_APIUser_0;
        }

        public static APIUser GetAPIUser(this VRCPlayer Instance)
        {
            return Instance.GetPlayer()?.GetAPIUser();
        }

        public static APIUser GetAPIUser(this PlayerNet Instance)
        {
            return Instance.GetPlayer()?.GetAPIUser();
        }

        // Player
        public static Player GetPlayer(this VRCPlayer Instance)
        {
            return Instance._player;
        }

        public static Player GetPlayer(this PlayerNet Instance)
        {
            return Instance.prop_Player_0;
        }

        public static Player GetPlayer(this Photon.Realtime.Player Instance)
        {
            return Instance.field_Public_Player_0;
        }

        public static Player GetPlayer(this PlayerManager instance, string UserID)
        {
            if (UserID == null) return null;

            foreach (Player player in instance.GetAllPlayers())
            {
                if (player.UserID() == UserID) return player;
            }

            return null;
        }

        public static Player[] GetAllPlayers(this PlayerManager instance)
        {
            return instance.field_Private_List_1_Player_0.ToArray();
        }

        //VRCPlayer
        public static VRCPlayer GetVRCPlayer(this PlayerNet Instance)
        {
            return Instance._vrcPlayer;
        }

        public static VRCPlayer GetVRCPlayer(this Player player)
        {
            return player._vrcplayer;
        }

        //IUser
        public static IUser GetIUser(this VRCPlayer Instance)
        {
            return Instance.prop_IUser_0;
        }

        public static IUser GetIUser(this Player Instance)
        {
            return Instance.GetVRCPlayer()?.GetIUser();
        }

        //ApiAvatar
        public static ApiAvatar GetAvatar(this VRCPlayer player)
        {
            return player.prop_ApiAvatar_0;
        }

        public static ApiAvatar GetFallbackAvatar(this VRCPlayer player)
        {
            return player.prop_ApiAvatar_1;
        }

        public static ApiAvatar GetAvatar(this Player Instance)
        {
            return Instance.GetVRCPlayer()?.GetAvatar();
        }

        public static ApiAvatar GetFallbackAvatar(this Player Instance)
        {
            return Instance.GetVRCPlayer()?.GetFallbackAvatar();
        }

        //PlayerApi
        public static VRCPlayerApi GetVRCPlayerApi(this Player Instance)
        {
            return Instance.prop_VRCPlayerApi_0;
        }

        public static VRCPlayerApi GetVRCPlayerApi(this VRCPlayer Instance)
        {
            return Instance.prop_VRCPlayerApi_0;
        }

        public static VRCPlayerApi GetVRCPlayerApi(this PlayerNet Instance)
        {
            return Instance.GetPlayer()?.GetVRCPlayerApi();
        }

        //PhotonPlayer
        public static Photon.Realtime.Player GetPhotonPlayer(this PlayerNet Instance)
        {
            return Instance.GetPlayer()?.GetPhotonPlayer();
        }

        public static Photon.Realtime.Player GetPhotonPlayer(this Player Instance)
        {
            return Instance.prop_Player_1;
        }

        public static Photon.Realtime.Player GetPhotonPlayer(this VRCPlayer Instance)
        {
            return Instance.GetPlayer()?.GetPhotonPlayer();
        }

        //PlayerNet
        public static PlayerNet GetPlayerNet(this Player Instance)
        {
            return Instance._playerNet;
        }

        public static PlayerNet GetPlayerNet(this VRCPlayer Instance)
        {
            return Instance._playerNet;
        }

        //UserID
        public static string UserID(this Player Instance)
        {
            return Instance.prop_String_0;
        }

        public static string UserID(this VRCPlayer Instance)
        {
            try
            {
                return Instance.GetPlayer()?.UserID();
            }
            catch
            {
                return "No UserID";
            }
        }

        public static string UserID(this PlayerNet Instance)
        {
            try
            {
                return Instance.GetPlayer()?.UserID();
            }
            catch
            {
                return "No UserID";
            }
        }

        public static string UserID(this APIUser Instance)
        {
            return Instance.id;
        }

        //Displayname
        public static string DisplayName(this Player Instance)
        {
            try
            {
                return Instance.GetAPIUser()?.DisplayName();
            }
            catch
            {
                return "No DisplayName";
            }
        }

        public static string DisplayName(this VRCPlayer Instance)
        {
            try
            {
                return Instance.GetAPIUser()?.DisplayName();
            }
            catch
            {
                return "No DisplayName";
            }
        }

        public static string DisplayName(this PlayerNet Instance)
        {
            try
            {
                return Instance.GetAPIUser()?.DisplayName();
            }
            catch
            {
                return "No DisplayName";
            }
        }

        public static string DisplayName(this APIUser Instance)
        {
            return Instance.displayName;
        }

        // Avatar Manager
        public static VRCAvatarManager GetVRCAvatarManager(this Player player)
        {
            return player.GetVRCPlayer()?.prop_VRCAvatarManager_0;
        }


        // USpeaker
        public static USpeaker GetUSpeaker(this Player player)
        {
            return player.prop_USpeaker_0;
        }

        public static USpeaker GetUSpeaker(this VRCPlayer player)
        {
            return player.prop_USpeaker_0;
        }

        // QM Selection
        public static Player GetSelectedPlayer()
        {
            return MenuHelper.selectedUserMenu.SelectedPlayer();
        }

        private static Player SelectedPlayer(this SelectedUserMenuQM QMInstance)
        {
            if (QMInstance.field_Private_IUser_0 == null) return null;
            return GameHelper.PlayerManager.GetPlayer(QMInstance.field_Private_IUser_0.prop_String_0);
        }
    }
}
