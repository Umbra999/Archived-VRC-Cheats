using LunaR.Modules;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using VRC.Core;

namespace LunaR.Extensions
{
    public static class PhotonExtensions
    {
        public static Dictionary<int, Il2CppSystem.Collections.Hashtable> CachedHashtables = new();
        public static string GetDisplayName(this Player player)
        {
            var Table = player.GetHashtable()["user"].TryCast<Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>>();
            if (Table != null) return Table["displayName"].ToString();
            return $"No Name [{player.ActorID()}]";
        }

        public static int ActorID(this Player player) => player.field_Private_Int32_0;

        public static VRC.Player GetPlayer(this Player player) => player.field_Public_Player_0;

        public static Il2CppSystem.Collections.Hashtable GetHashtable(this Player player)
        {
            if (CachedHashtables.ContainsKey(player.ActorID())) return CachedHashtables[player.ActorID()];
            else return player.prop_Hashtable_0;
        }

        public static Player[] GetAllPhotonPlayers(this VRCNetworkingClient Instance)
        {
            Player[] result = new Player[Instance.prop_Room_0.field_Private_Dictionary_2_Int32_Player_0.Count];
            int index = 0;
            foreach (var player in Instance.prop_Room_0.field_Private_Dictionary_2_Int32_Player_0)
            {
                result[index++] = player.Value;
            }
            return result;
        }

        public static int[] GetAllPhotonActors(this VRCNetworkingClient Instance)
        {
            int[] result = new int[Instance.prop_Room_0.field_Private_Dictionary_2_Int32_Player_0.Count];
            int index = 0;
            foreach (var player in Instance.prop_Room_0.field_Private_Dictionary_2_Int32_Player_0)
            {
                result[index++] = player.Key;
            }
            return result;
        }

        public static bool IsVR(this Player player)
        {
            return player.GetHashtable()["inVRMode"].Unbox<bool>();
        }

        public static bool IsMaster(this Player player)
        {
            return player.prop_Boolean_1;
        }

        public static bool IsMod(this Player player)
        {
            if (player.GetHashtable().ContainsKey("modTag") && player.GetHashtable()["modTag"].ToString() != "") return true;
            return false;
        }

        public static int AvatarHeight(this Player player)
        {
            if (player.GetHashtable().ContainsKey("avatarEyeHeight")) return player.GetHashtable()["avatarEyeHeight"].Unbox<int>();
            return 0;
        }

        public static Player GetPhotonPlayer(int photonID)
        {
            return Utils.VRCNetworkingClient.prop_Room_0.Method_Public_Virtual_New_Player_Int32_Boolean_0(photonID);
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

        public static Player GetSelf(this LoadBalancingClient Instance)
        {
            return Instance.prop_Player_0;
        }

        public static Player GetSelf(this VRCNetworkingClient Instance)
        {
            return Instance.prop_Player_0;
        }

        public static string UserID(this Player player)
        {
            var Table = player.GetHashtable()["user"].TryCast<Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>>();
            if (Table != null) return Table["id"].ToString();
            return $"No UserID [{player.ActorID()}]";
        }

        public static string AvatarID(this Player player)
        {
            var Table = player.GetHashtable()["avatarDict"].TryCast<Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>>();
            if (Table != null) return Table["id"].ToString();
            return "No AvatarID";
        }

        public static int GetActorPosition(int[] arr, int offset)
        {
            if (arr.Length >= offset)
            {
                Array.Sort(arr);
                return arr[offset - 1];
            }
            return arr[0];
        }
    }
}