using LunaR.Api;
using LunaR.Extensions;
using LunaR.Modules;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.DataModel.Core;
using VRC.SDKBase;
using VRC.UI.Elements.Menus;

namespace LunaR.Wrappers
{
    public static class PlayerWrappers
    {
        // API User
        public static APIUser GetAPIUser(this Player Instance)
        {
            return Instance.prop_APIUser_0;
        }

        public static APIUser GetAPIUser(this VRCPlayer Instance)
        {
            return Instance.GetPlayer().GetAPIUser();
        }

        public static APIUser GetAPIUser(this PlayerNet Instance)
        {
            return Instance.GetPlayer().GetAPIUser();
        }

        public static APIUser GetSelectedAPIUser()
        {
            return APIStuff.GetTargetMenuInstance().SelectedAPIUser();
        }

        // Player
        public static Player[] GetAllPlayers(this PlayerManager instance)
        {
            return instance.field_Private_List_1_Player_0.ToArray();
        }

        public static Player GetPlayer(this VRCPlayer Instance)
        {
            return Instance.prop_Player_0;
        }

        public static Player GetPlayer(this PlayerNet Instance)
        {
            return Instance.prop_Player_0;
        }

        private static Player SelectedPlayer(this SelectedUserMenuQM QMInstance)
        {
            if (QMInstance.field_Private_IUser_0 == null) return null;
            return Utils.PlayerManager.GetPlayer(QMInstance.field_Private_IUser_0.prop_String_0);
        }

        private static APIUser SelectedAPIUser(this SelectedUserMenuQM QMInstance)
        {
            if (QMInstance.field_Private_IUser_0 == null) return null;
            DataModel<APIUser> user = QMInstance.field_Private_IUser_0.Cast<DataModel<APIUser>>();
            return user.field_Protected_TYPE_0;
        }

        public static Player GetPlayer(this PlayerManager instance, string UserID)
        {
            if (UserID == null) return null;
            Player[] Players = instance.GetAllPlayers();
            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i].UserID() == UserID) return Players[i];
            }
            return null;
        }

        public static Player GetPlayer(int PhotonID)
        {
            Photon.Realtime.Player PhotonPlayer = PhotonExtensions.GetPhotonPlayer(PhotonID);
            if (PhotonPlayer != null) return PhotonPlayer.GetPlayer();
            return null;
        }

        public static Player GetPlayerByRayCast(this RaycastHit RayCast)
        {
            var gameObject = RayCast.transform.gameObject;
            return GetPlayer(VRCPlayerApi.GetPlayerByGameObject(gameObject).playerId);
        }

        public static Player GetSelectedPlayer()
        {
            return APIStuff.GetTargetMenuInstance().SelectedPlayer();
        }

        //VRCPlayer
        public static VRCPlayer GetVRCPlayer(this PlayerNet Instance)
        {
            return Instance.prop_VRCPlayer_0;
        }

        public static VRCPlayer GetVRCPlayer(this Player player)
        {
            return player.prop_VRCPlayer_0;
        }

        //ApiAvatar
        public static ApiAvatar GetAPIAvatar(this VRCPlayer player)
        {
            return player.prop_ApiAvatar_0;
        }

        public static ApiAvatar GetApiAvatar(this Player Instance)
        {
            return Instance.prop_ApiAvatar_0;
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
            return Instance.GetVRCPlayer().GetVRCPlayerApi();
        }


        //PhotonPlayer
        public static Photon.Realtime.Player GetPhotonPlayer(this PlayerNet Instance)
        {
            return Instance.GetPlayer().GetPhotonPlayer();
        }

        public static Photon.Realtime.Player GetPhotonPlayer(this Player Instance)
        {
            return Instance.field_Private_Player_0;
        }

        public static Photon.Realtime.Player GetPhotonPlayer(this VRCPlayer Instance)
        {
            return Instance.GetPlayer().GetPhotonPlayer();
        }


        //UserID
        public static string UserID(this Player Instance)
        {
            try
            {
                return Instance.GetAPIUser().id;
            }
            catch
            {
                return "No UserID";
            }
        }

        public static string UserID(this VRCPlayer Instance)
        {
            try
            {
                return Instance.GetAPIUser().id;
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
                return Instance.GetAPIUser().id;
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
                return Instance.GetAPIUser().displayName;
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
                return Instance.GetAPIUser().displayName;
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
                return Instance.GetAPIUser().displayName;
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

        // Misc
        public static VRCAvatarManager GetVRCAvatarManager(this Player player)
        {
            return player.prop_VRCPlayer_0.prop_VRCAvatarManager_0;
        }

        public static USpeaker GetUSpeaker(this VRCPlayer player)
        {
            return player.prop_USpeaker_0;
        }

        public static bool CanHearMe(int Actor)
        {
            return InterestManager.Method_Public_Static_Boolean_Int32_0(Actor);
        }

        public static bool CanHearMe(this Photon.Realtime.Player Player)
        {
            return CanHearMe(Player.ActorID());
        }
    }
}