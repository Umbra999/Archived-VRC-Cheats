using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.SDKBase;
using VRC.UI;
using WengaPort.Modules;
using WengaPort.Wrappers;

namespace WengaPort.Modules
{
	internal static class PlayerExtensions
	{
		public static APIUser GetAPIUser(this Player Instance)
		{
			return Instance.field_Private_APIUser_0;
		}

		public static Player GetPlayerWithPlayerID(this PlayerManager Instance, int playerID)
		{
			List<Player> list = Instance.GetAllPlayers().ToList();
			foreach (Player player in list.ToArray())
			{
				if (player.GetVRCPlayerApi().playerId == playerID)
				{
					return player;
				}
			}
			return null;
		}

		public static APIUser GetAPIUser(this VRCPlayer Instance)
		{
			return Instance.field_Private_Player_0.field_Private_APIUser_0;
		}

		public static APIUser GetAPIUser(this PlayerNet Instance)
		{
			return Instance.field_Internal_VRCPlayer_0.field_Private_Player_0.field_Private_APIUser_0;
		}

		public static Player GetPlayer(this VRCPlayer Instance)
		{
			return Instance.field_Private_Player_0;
		}

		public static VRCPlayer GetVRCPlayer(this PlayerNet Instance)
		{
			return Instance.field_Internal_VRCPlayer_0;
		}

		public static string UserID(this Player Instance)
		{
			return Instance.GetAPIUser().id;
		}

		public static string UserID(this VRCPlayer Instance)
		{
			return Instance.GetAPIUser().id;
		}

		public static string UserID(this PlayerNet Instance)
		{
			return Instance.GetAPIUser().id;
		}

		public static string UserID(this APIUser Instance)
		{
			return Instance.id;
		}

		public static string DisplayName(this Player Instance)
		{
			return Instance.GetAPIUser().displayName;
		}

		public static string DisplayName(this VRCPlayer Instance)
		{
			return Instance.GetAPIUser().displayName;
		}

		public static string DisplayName(this PlayerNet Instance)
		{
			return Instance.GetAPIUser().displayName;
		}

		public static string DisplayName(this APIUser Instance)
		{
			return Instance.displayName;
		}

		public static void ReloadAvatar(this VRCPlayer Instance)
		{
			VRCPlayer.Method_Public_Static_Void_APIUser_0(Instance.GetAPIUser());
		}

		public static void ReloadAvatar(this Player Instance)
		{
			VRCPlayer.Method_Public_Static_Void_APIUser_0(Instance.GetAPIUser());
		}

		public static bool IsFriend(this Player player)
		{
			return APIUser.CurrentUser.friendIDs.Contains(player.field_Private_APIUser_0.id);
		}

		public static bool IsLocal(this Player player)
		{
			return player.field_Private_APIUser_0.id == APIUser.CurrentUser.id;
		}

		public static VRCPlayerApi GetVRCPlayerApi(this Player Instance)
		{
			return Instance.prop_VRCPlayerApi_0;
		}

		public static void ChangeAvatar(string avatarID)
		{
            new PageAvatar
            {
                field_Public_SimpleAvatarPedestal_0 = new SimpleAvatarPedestal
                {
                    field_Internal_ApiAvatar_0 = new ApiAvatar
                    {
                        id = avatarID
                    }
                }
            }.ChangeToSelectedAvatar();
        }


		public static VRCPlayerApi GetVRCPlayerApi(this VRCPlayer Instance)
		{
			return Instance.prop_VRCPlayerApi_0;
		}

		public static VRCPlayerApi GetVRCPlayerApi(this PlayerNet Instance)
		{
			return Instance.GetVRCPlayer().GetVRCPlayerApi();
		}

		public static bool GetIsMaster(this VRCPlayer Instance)
		{
			return Instance.GetVRCPlayerApi().isMaster;
		}
		public static bool GetIsMaster(this Player Instance)
		{
			return Instance.GetVRCPlayerApi().isMaster;
		}

		public static bool GetIsMaster(this PlayerNet Instance)
		{
			return Instance.GetVRCPlayerApi().isMaster;
		}

		public static Color GetRankColor(this APIUser Instance)
		{
			string playerRank = Instance.GetRank();
			switch (playerRank.ToLower())
			{
				case "legend":
					return new Color(1f, 1f, 1f);
				case "veteran":
					return new Color(1f, 0.81f, 0.03f);
				case "trusted":
					return new Color(0.87f, 0f, 0.5f);
				case "known":
					return new Color(0.92f, 0.37f, 0f);
				case "user":
					return new Color(0f, 1f, 0f);
				case "new user":
					return new Color(0, 1f, 1f);
				case "visitor":
					return new Color(0f, 0f, 0f);
			}
			return Color.red;
		}

		public static float GetQuality(this VRCPlayer Instance)
		{
			return ((!(Instance.prop_PlayerNet_0 == null)) ? Instance.prop_PlayerNet_0.prop_Single_0 : 0f) * 100;
		}
		public static string GetQualityColored(this VRCPlayer Instance)
		{
            string color;
            if (Instance.GetQuality() >= 85)
				color = "<color=#59D365>";
			else if (Instance.GetQuality() >= 50)
				color = "<color=#FF7000>";
			else
				color = "<color=red>";
			string Percent = Instance.GetQuality().ToString().Split('.')[0];
			return $"{color}{Percent}%</color>";
		}

		public static string GetQualitySymbols(this VRCPlayer Instance)
		{
			string color;
			string Percent;
			if (Instance.GetQuality() >= 85)
            {
				color = "<color=#59D365>";
				Percent = "+";
			}
			else if (Instance.GetQuality() >= 50)
            {
				color = "<color=#FF7000>";
				Percent = "/";
			}	
			else
            {
				color = "<color=red>";
				Percent = "-";
			}
			return $"{color}{Percent}</color>";
		}

		public static string GetRank(this APIUser Instance)
		{
			string result;
			if (Instance.hasModerationPowers || Instance.tags.Contains("admin_moderator"))
			{
				result = "Moderation User";
			}
			else if (Instance.hasSuperPowers || Instance.tags.Contains("admin_"))
			{
				result = "Admin User";
			}
			else if (Instance.hasVIPAccess || Instance.tags.Contains("system_legend") || (Instance.tags.Contains("system_legend") && Instance.tags.Contains("system_trust_legend") && Instance.tags.Contains("system_trust_trusted")))
			{
				result = "Legend";
			}
			else if (Instance.hasLegendTrustLevel || (Instance.tags.Contains("system_trust_legend") && Instance.tags.Contains("system_trust_veteran")))
			{
				result = "Veteran";
			}
			else if (Instance.hasVeteranTrustLevel || Instance.tags.Contains("system_trust_veteran"))
			{
				result = "Trusted";
			}
			else if (Instance.hasTrustedTrustLevel)
			{
				result = "Known";
			}
			else if (Instance.hasKnownTrustLevel)
			{
				result = "User";
			}
			else if (Instance.hasBasicTrustLevel || Instance.isNewUser)
			{
				result = "New User";
			}
			else if (Instance.hasNegativeTrustLevel)
			{
				result = "NegativeTrust";
			}
			else if (Instance.hasVeryNegativeTrustLevel)
			{
				result = "VeryNegativeTrust";
			}
			else
            {
				result = "Visitor";
			}
			return result;
		}

		public static bool GetIsFriend(this APIUser Instance)
		{
			return Instance.isFriend || APIUser.IsFriendsWith(Instance.id);
		}

		public static ApiAvatar GetApiAvatar(this Player Instance)
		{
			return Instance.field_Internal_VRCPlayer_0.prop_VRCAvatarManager_0.field_Private_ApiAvatar_0;
		}

		public static APIUser SelectedAPIUser(this QuickMenu instance)
		{
			return instance.field_Private_APIUser_0;
		}

		public static VRCPlayer SelectedVRCPlayer(this QuickMenu instance)
		{
			return instance.field_Private_Player_0.GetVRCPlayer();
		}

		public static Player SelectedPlayer(this QuickMenu instance)
		{
			return instance.field_Private_Player_0.prop_VRCPlayer_0.GetVRC_Player();
		}
	}
}
