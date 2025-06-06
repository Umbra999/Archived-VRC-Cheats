using Hexed.Core;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.Management;
using VRC.SDK3.Components;

namespace Hexed.Wrappers
{
    internal static class PlayerUtils
    {
        public static void ChangeAvatar(string avatarID)
        {
            if (avatarID.StartsWith("avtr_") && GameHelper.CurrentPlayer.GetAPIUser().avatarId != avatarID)
            {
                SystemController controller = Resources.FindObjectsOfTypeAll<SystemController>().FirstOrDefault();
                controller.Method_Public_Void_ApiAvatar_String_PDM_0(new ApiAvatar() { id = avatarID }, "AvatarPedestal");
            }
        }

        public static bool IsPlayerFBT(this VRCPlayer Player)
        {
            if (Player.prop_VRC_AnimationController_0?.field_Private_IkController_0?.prop_EnumNPublicSealedvaInNoLiThFoFiSi8vUnique_0 == IkController.EnumNPublicSealedvaInNoLiThFoFiSi8vUnique.SixPoint) return true;
            return false;
        }

        public static bool IsPlayerAFK(this VRCPlayer player)
        {
            if (player.field_Internal_Animator_0 == null) return false;

            return player.field_Internal_Animator_0.GetBool("AFK");
        }

        public static bool IsPlayerEarmuff(this VRCPlayer player)
        {
            if (player.field_Internal_Animator_0 == null) return false;

            return player.field_Internal_Animator_0.GetBool("Earmuffs"); // Doesnt work properly, replace
        }

        public static bool IsPlayerMicDisabled(this VRCPlayer player)
        {
            if (player.field_Internal_Animator_0 == null) return false;

            return player.field_Internal_Animator_0.GetBool("MuteSelf"); // Doesnt work properly, replace
        }

        public static bool IsPlayerSeated(this VRCPlayer player)
        {
            if (player.field_Internal_Animator_0 == null) return false;

            return player.field_Internal_Animator_0.GetBool("InStation");
        }

        public static APIDevice GetAPIDevice(string device)
        {
            switch (device)
            {
                case "android":
                    return APIDevice.Android;

                case "standalonewindows":
                    return APIDevice.Windows;

                case "ios":
                    return APIDevice.IOS;
            }
            return APIDevice.Unknown;
        }

        public static APIDevice GetAPIDevice(this APIUser Player)
        {
            string Platform = Player.last_platform;

            return Platform == null ? APIDevice.Unknown : GetAPIDevice(Platform);
        }

        public static APIDevice GetAPIDevice(this Player Player)
        {
            APIUser apiUser = Player.GetAPIUser();
            if (apiUser == null) return APIDevice.Unknown;

            return GetAPIDevice(apiUser);
        }

        public static APIDevice GetAPIDevice(this VRCPlayer Player)
        {
            APIUser apiUser = Player.GetAPIUser();
            if (apiUser == null) return APIDevice.Unknown;

            return GetAPIDevice(apiUser);
        }

        public static APIDevice GetAPIDevice(this Photon.Realtime.Player Player)
        {
            string Platform = Player.GetPlatform();

            return Platform == null ? APIDevice.Unknown : GetAPIDevice(Platform);
        }


        public static Device GetDevice(this VRCPlayer Player)
        {
            APIDevice APIDevice = Player.GetAPIDevice();

            switch (APIDevice)
            {
                case APIDevice.Unknown:
                    return Device.Unknown;

                case APIDevice.Android:
                    if (Player.GetVRCPlayerApi().IsUserInVR()) return Device.Quest;
                    return Device.Mobile;

                case APIDevice.Windows:
                    if (Player.GetVRCPlayerApi().IsUserInVR()) return Device.VR;
                    return Device.Desktop;

                case APIDevice.IOS:
                    return Device.Mobile; // TODO: Add IOS handling
            }

            return Device.Unknown;
        }

        public static Device GetDevice(this Player Player)
        {
            APIDevice APIDevice = Player.GetAPIDevice();

            switch (APIDevice)
            {
                case APIDevice.Android:
                    if (Player.GetVRCPlayerApi().IsUserInVR()) return Device.Quest;
                    return Device.Mobile;

                case APIDevice.Windows:
                    if (Player.GetVRCPlayerApi().IsUserInVR()) return Device.VR;
                    return Device.Desktop;

                case APIDevice.IOS:
                    return Device.Mobile; // TODO: Add IOS handling
            }

            return Device.Unknown;
        }

        public static Device GetDevice(this Photon.Realtime.Player Player)
        {
            APIDevice APIDevice = Player.GetAPIDevice();

            switch (APIDevice)
            {
                case APIDevice.Android:
                    if (Player.IsVR()) return Device.Quest;
                    return Device.Mobile;

                case APIDevice.Windows:
                    if (Player.IsVR()) return Device.VR;
                    return Device.Desktop;

                case APIDevice.IOS:
                    return Device.Mobile; // TODO: Add IOS handling
            }

            return Device.Unknown;
        }

        public static Color GetPlatformColor(string Platform)
        {
            switch (Platform)
            {
                case "android":
                    return new Color(0f, 0.83f, 0.27f);

                case "standalonewindows":
                    return new Color(0.34f, 0.46f, 0.89f);
            }

            return Color.gray;
        }

        public static string GetDeviceTag(this Device device)
        {
            switch (device)
            {
                case Device.Quest:
                    return "QS";

                case Device.Mobile:
                    return "MB";

                case Device.Desktop:
                    return "PC";

                case Device.VR:
                    return "VR";
            }

            return "UKN";
        }

        public static bool IsBlocked(this ModerationManager instance, string userId)
        {
            if (userId == null) return false;

            var moderationlist = instance.field_Private_Dictionary_2_String_List_1_ApiPlayerModeration_0;
            if (!moderationlist.ContainsKey(userId)) return false;

            var playerModerationList = moderationlist[userId];
            if (playerModerationList == null || playerModerationList._items == null) return false;

            var matchingModerations = playerModerationList._items.Where(x => x?.moderationType == ApiPlayerModeration.ModerationType.Block);

            return matchingModerations != null && matchingModerations.ToArray().Length > 0;
        }

        public static bool IsMuted(this ModerationManager instance, string userId)
        {
            if (userId == null) return false;

            var moderationlist = instance.field_Private_Dictionary_2_String_List_1_ApiPlayerModeration_0;
            if (!moderationlist.ContainsKey(userId)) return false;

            var playerModerationList = moderationlist[userId];
            if (playerModerationList == null || playerModerationList._items == null) return false;

            var matchingModerations = playerModerationList._items.Where(x => x?.moderationType == ApiPlayerModeration.ModerationType.Mute);

            return matchingModerations != null && matchingModerations.ToArray().Length > 0;
        }

        public static short GetPing(this PlayerNet instance)
        {
            return instance.field_Private_Int16_0;
        }

        public static short GetPing(this Player instance)
        {
            return instance.GetPlayerNet().GetPing();
        }

        public static short GetPing(this VRCPlayer instance)
        {
            return instance.GetPlayerNet().GetPing();
        }

        public static Color GetPingColor(this VRCPlayer Instance)
        {
            if (Instance.GetPing() < 1) return Color.red;

            float t = Instance.GetPing() / 200f;
            return Color.Lerp(Color.green, Color.red, t);
        }

        public static Color GetPingColor(this Player Instance)
        {
            if (Instance.GetPing() < 1) return Color.red;

            float t = Instance.GetPing() / 200f;
            return Color.Lerp(Color.green, Color.red, t);
        }

        public static int GetFrames(this PlayerNet Instance)
        {
            return Instance.prop_Byte_0 == 0 ? 0 : 1000 / Instance.prop_Byte_0;
        }

        public static int GetFrames(this VRCPlayer Instance)
        {
            return Instance.GetPlayerNet().GetFrames();
        }

        public static int GetFrames(this Player Instance)
        {
            return Instance.GetPlayerNet().GetFrames();
        }

        public static Color GetFramesColor(this VRCPlayer Instance)
        {
            float t = Instance.GetFrames() / 140f;
            return Color.Lerp(Color.green, Color.red, 1 - t);
        }

        public static Color GetFramesColor(this Player Instance)
        {
            float t = Instance.GetFrames() / 140f;
            return Color.Lerp(Color.green, Color.red, 1 - t);
        }

        public static float GetQuality(this PlayerNet Instance)
        {
            return Instance.prop_Single_0;
        }

        public static float GetQuality(this VRCPlayer Instance)
        {
            return Instance.GetPlayerNet().GetQuality();
        }

        public static float GetQuality(this Player Instance)
        {
            return Instance.GetPlayerNet().GetQuality();
        }

        public static int GetQualityPercentage(this VRCPlayer Instance)
        {
            return (int)(Instance.GetPlayerNet().GetQuality() * 100);
        }

        public static int GetQualityPercentage(this Player Instance)
        {
            return (int)(Instance.GetPlayerNet().GetQuality() * 100);
        }

        public static Color GetQualityColor(this VRCPlayer Instance)
        {
            return Color.Lerp(Color.red, Color.green, Instance.GetQuality());
        }

        public static Color GetQualityColor(this Player Instance)
        {
            return Color.Lerp(Color.red, Color.green, Instance.GetQuality());
        }

        public static float GetHeight(this VRCPlayer Instance)
        {
            return Instance.field_Private_Single_9;
        }

        public static TrustRanks GetRank(this APIUser Instance)
        {
            if (Instance.developerType != APIUser.DeveloperType.None || Instance.hasModerationPowers || Instance.hasScriptingAccess || Instance.hasVIPAccess || Instance.hasSuperPowers) return TrustRanks.MODERATOR;
            else if (Instance.hasVeteranTrustLevel) return TrustRanks.TRUSTED;
            else if (Instance.hasTrustedTrustLevel) return TrustRanks.KNOWN;
            else if (Instance.hasKnownTrustLevel) return TrustRanks.USER;
            else if (Instance.hasBasicTrustLevel) return TrustRanks.NEW;
            else if (Instance.hasNegativeTrustLevel || Instance.hasVeryNegativeTrustLevel) return TrustRanks.NUISANCE;
            return TrustRanks.VISITOR;
        }

        public static Color GetRankColor(this APIUser Instance)
        {
            return Instance.GetRank().GetRankColor();
        }

        public static Color GetRankColor(this TrustRanks Instance)
        {
            switch (Instance)
            {
                case TrustRanks.TRUSTED:
                    return VRCPlayer.field_Internal_Static_Color_6;

                case TrustRanks.KNOWN:
                    return VRCPlayer.field_Internal_Static_Color_5;

                case TrustRanks.USER:
                    return VRCPlayer.field_Internal_Static_Color_4;

                case TrustRanks.NEW:
                    return VRCPlayer.field_Internal_Static_Color_3;

                case TrustRanks.VISITOR:
                    return VRCPlayer.field_Internal_Static_Color_2;

                case TrustRanks.MODERATOR:
                    return VRCPlayer.field_Internal_Static_Color_8;

                case TrustRanks.NUISANCE:
                    return VRCPlayer.field_Internal_Static_Color_0;

                default:
                    return Color.black;
            }
        }

        public static string GetRankString(this TrustRanks Instance)
        {
            switch (Instance)
            {
                case TrustRanks.TRUSTED:
                    return "Trusted";

                case TrustRanks.KNOWN:
                    return "Known";

                case TrustRanks.USER:
                    return "User";

                case TrustRanks.NEW:
                    return "New";

                case TrustRanks.VISITOR:
                    return "Visitor";

                case TrustRanks.MODERATOR:
                    return "Moderator";

                case TrustRanks.NUISANCE:
                    return "Nuisance";

                default:
                    return "NO RANK";
            }
        }

        public enum TrustRanks
        {
            TRUSTED,
            KNOWN,
            USER,
            NEW,
            NUISANCE,
            VISITOR,
            MODERATOR,
        }

        public enum Device
        {
            Unknown,
            Quest,
            Mobile,
            Desktop,
            VR,
        }

        public enum APIDevice
        {
            Unknown,
            Android,
            Windows,
            IOS
        }

        public static bool IsInstanceOwner(this Player player)
        {
            return IsInstanceOwner(player.UserID());
        }

        public static bool IsInstanceOwner(this VRCPlayer player)
        {
            return IsInstanceOwner(player.UserID());
        }

        public static bool IsInstanceOwner(string UserID)
        {
            return RoomManager.prop_ApiWorldInstance_1?.ownerId == UserID;
        }

        public static bool IsFriend(this APIUser player)
        {
            return IsFriend(player.UserID());
        }

        public static bool IsFriend(string UserID)
        {
            return APIUser.IsFriendsWith(UserID);
        }

        public static bool IsFrozen(int Actor)
        {
            if (!InternalSettings.ActorsWithLastActiveTime.ContainsKey(Actor)) return false;

            return InternalSettings.ActorsWithLastActiveTime[Actor] < GeneralUtils.GetUnixTimeInMilliseconds() - 2500;
        }

        public static bool CanHearMe(int Actor)
        {
            return InterestManager.Method_Public_Static_Boolean_Int32_PDM_0(Actor);
        }

        public static bool CanHearMe(this Photon.Realtime.Player Player)
        {
            return CanHearMe(Player.ActorID());
        }

        public static void ReloadAvatar(this APIUser Instance)
        {
            VRCPlayer.Method_Public_Static_Void_APIUser_0(Instance);
        }
    }
}
