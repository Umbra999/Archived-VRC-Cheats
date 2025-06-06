using Photon.Realtime;
using VRC.Core;

namespace Hexed.Wrappers
{
    internal static class PhotonUtils
    {
        public static Dictionary<int, Il2CppSystem.Collections.Hashtable> networkedProperties = new();
       
        public static int ActorID(this Player player)
        {
            return player.prop_Int32_0;
        }

        public static Il2CppSystem.Collections.Hashtable GetProperties(this Player player)
        {
            var NetworkedProps = player?.GetNetworkedProperties();
            if (NetworkedProps != null) return NetworkedProps;
            return player?.prop_Hashtable_0;
        }

        private static Il2CppSystem.Collections.Hashtable GetNetworkedProperties(this Player player)
        {
            if (networkedProperties.ContainsKey(player.ActorID())) return networkedProperties[player.ActorID()];
            return null;
        }

        public static Player[] GetAllPhotonPlayers(this VRCNetworkingClient Instance)
        {
            var RawDictionary = Instance.prop_Room_0?.prop_ConcurrentDictionary_2_Int32_Player_0?.ToArray();

            Player[] result = new Player[RawDictionary.Count];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = RawDictionary[i]?.Value;
            }

            return result;
        }

        public static int GetActorCount(this VRCNetworkingClient Instance)
        {
            return Instance.prop_Room_0.prop_ConcurrentDictionary_2_Int32_Player_0.Count;
        }

        public static bool IsMaster(this Player player)
        {
            return player.prop_Boolean_1;
        }

        public static Player GetPhotonPlayer(this VRCNetworkingClient Instance, int photonID)
        {
            Player[] Players = Instance.GetAllPhotonPlayers();
            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i].ActorID() == photonID) return Players[i];
            }

            return null;
        }

        public static Player GetPhotonPlayer(this VRCNetworkingClient Instance, string UserID)
        {
            Player[] Players = Instance.GetAllPhotonPlayers();
            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i].UserID() == UserID) return Players[i];
            }

            return null;
        }

        public static Player GetCurrentPlayer(this VRCNetworkingClient Instance)
        {
            return Instance.prop_Player_0;
        }

        public static string DisplayName(this Player player)
        {
            var props = player?.GetProperties();

            if (props != null && props.ContainsKey("user"))
            {
                var Table = player.GetProperties()["user"].TryCast<Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>>();
                if (Table != null && Table.ContainsKey("displayName")) return Table["displayName"]?.ToString();
            }

            return null;
        }

        public static string GetPlatform(this Player player)
        {
            var props = player?.GetProperties();

            if (props != null && props.ContainsKey("user"))
            {
                var Table = player.GetProperties()["user"].TryCast<Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>>();
                if (Table != null && Table.ContainsKey("last_platform")) return Table["last_platform"]?.ToString();
            }

            return null;
        }

        public static string UserID(this Player player)
        {
            var props = player?.GetProperties();

            if (props != null && props.ContainsKey("user"))
            {
                var Table = player.GetProperties()["user"].TryCast<Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>>();
                if (Table != null && Table.ContainsKey("id")) return Table["id"]?.ToString();
            }

            return null;
        }

        public static string AvatarID(this Player player)
        {
            var props = player?.GetProperties();

            if (props != null && props.ContainsKey("avatarDict"))
            {
                var Table = player.GetProperties()["avatarDict"].TryCast<Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>>();
                if (Table != null && Table.ContainsKey("id")) return Table["id"]?.ToString();
            }

            return null;
        }

        public static string AvatarName(this Player player)
        {
            var props = player?.GetProperties();

            if (props != null && props.ContainsKey("avatarDict"))
            {
                var Table = player.GetProperties()["avatarDict"].TryCast<Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>>();
                if (Table != null && Table.ContainsKey("name")) return Table["name"]?.ToString();
            }

            return null;
        }

        public static string AvatarAuthorName(this Player player)
        {
            var props = player?.GetProperties();

            if (props != null && props.ContainsKey("avatarDict"))
            {
                var Table = player.GetProperties()["avatarDict"].TryCast<Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>>();
                if (Table != null && Table.ContainsKey("authorName")) return Table["authorName"]?.ToString();
            }

            return null;
        }

        public static string AvatarReleaseStatus(this Player player)
        {
            var props = player?.GetProperties();

            if (props != null && props.ContainsKey("avatarDict"))
            {
                var Table = player.GetProperties()["avatarDict"].TryCast<Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>>();
                if (Table != null && Table.ContainsKey("releaseStatus")) return Table["releaseStatus"]?.ToString();
            }

            return null;
        }

        public static bool IsVR(this Player player)
        {
            if (player.GetProperties().ContainsKey("inVRMode")) return player.GetProperties()["inVRMode"].Unbox<bool>();
            return false;
        }

        public static bool IsInstanceModerator(this Player player)
        {
            if (player.GetProperties().ContainsKey("canModerateInstance")) return player.GetProperties()["canModerateInstance"].Unbox<bool>();
            return false;
        }

        public static int AvatarHeight(this Player player)
        {
            if (player.GetProperties().ContainsKey("avatarEyeHeight")) return player.GetProperties()["avatarEyeHeight"].Unbox<int>();
            return 0;
        }

        public static string RepresentedGroupID(this Player player)
        {
            if (player.GetProperties().ContainsKey("groupOnNameplate")) return player.GetProperties()["groupOnNameplate"].ToString();
            return null;
        }

        public static bool ShowGroupToOthers(this Player player)
        {
            if (player.GetProperties().ContainsKey("showGroupBadgeToOthers")) return player.GetProperties()["showGroupBadgeToOthers"].Unbox<bool>();
            return false;
        }

        public static bool ShowSocialRankToOthers(this Player player)
        {
            if (player.GetProperties().ContainsKey("showSocialRank")) return player.GetProperties()["showSocialRank"].Unbox<bool>();
            return false;
        }

        public static bool UseImpostorAsFallback(this Player player)
        {
            if (player.GetProperties().ContainsKey("useImpostorAsFallback")) return player.GetProperties()["useImpostorAsFallback"].Unbox<bool>();
            return false;
        }
    }
}
