using LunaR.Extensions;
using LunaR.Wrappers;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.Management;
using VRC.UI;

namespace LunaR.Modules
{
    public static class PlayerExtensions
    {
        public enum Pronouns
        {
            None,
            Female,
            Male,
            Diverse
        }

        public static bool IsPlayerFBT(this VRCPlayer Player)
        {
            if (Player.prop_VRCAvatarManager_0.field_Internal_IkController_0?.prop_IkType_0 == IkController.IkType.SixPoint) return true;
            return false;
        }

        public static string GetPlatform(this APIUser Player)
        {
            switch (Player.last_platform)
            {
                case "android":
                    return "Quest";

                case "standalonewindows":
                    return "PC";
            }
            return Player.last_platform;
        }

        public static void ReloadAvatar(this APIUser Instance)
        {
            VRCPlayer.Method_Public_Static_Void_APIUser_0(Instance);
        }

        public static bool IsFriend(this VRCPlayer player)
        {
            return IsFriend(player.UserID());
        }

        public static bool IsFriend(this Player player)
        {
            return IsFriend(player.UserID());
        }

        public static bool IsFriend(this APIUser player)
        {
            if (player.isFriend || APIUser.IsFriendsWith(player.UserID())) return true;
            return false;
        }

        public static bool IsFriend(string UserID)
        {
            return APIUser.IsFriendsWith(UserID);
        }

        public static Pronouns GetGender(this APIUser User)
        {
            string Status = User.statusDescription.ToLower();
            string Bio = User.bio.ToLower();
            string Text = Status + "\n" + Bio;

            if (Text.Contains("she⁄her")) return Pronouns.Female;
            if (Text.Contains("she⁄they")) return Pronouns.Female;
            if (Text.Contains("she⁄them")) return Pronouns.Female;
            if (Text.Contains("them⁄her")) return Pronouns.Female;
            if (Text.Contains("they⁄her")) return Pronouns.Female;
            if (Text.Contains("i'm female")) return Pronouns.Female;
            if (Text.Contains("im female")) return Pronouns.Female;
            if (Text.Contains("i am female")) return Pronouns.Female;
            if (Text.Contains("gender˸ female")) return Pronouns.Female;

            if (Text.Contains("they⁄them")) return Pronouns.Diverse;
            if (Text.Contains("she⁄he")) return Pronouns.Diverse;
            if (Text.Contains("she⁄him")) return Pronouns.Diverse;
            if (Text.Contains("he⁄she")) return Pronouns.Diverse;
            if (Text.Contains("he⁄her")) return Pronouns.Diverse;
            if (Text.Contains("i'm trans")) return Pronouns.Diverse;
            if (Text.Contains("im trans")) return Pronouns.Diverse;
            if (Text.Contains("i am trans")) return Pronouns.Diverse;
            if (Text.Contains("i am trans")) return Pronouns.Diverse;
            if (Text.Contains("gender˸ trans")) return Pronouns.Diverse;
            if (Text.Contains("any pronouns")) return Pronouns.Diverse;

            if (Text.Contains("he⁄him")) return Pronouns.Male;
            if (Text.Contains("he⁄them")) return Pronouns.Male;
            if (Text.Contains("he⁄they")) return Pronouns.Male;
            if (Text.Contains("them⁄him")) return Pronouns.Male;
            if (Text.Contains("they⁄him")) return Pronouns.Male;
            if (Text.Contains("i'm male")) return Pronouns.Male;
            if (Text.Contains("im male")) return Pronouns.Male;
            if (Text.Contains("i am male")) return Pronouns.Male;
            if (Text.Contains("gender˸ male")) return Pronouns.Male;

            return Pronouns.None;
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
            return RoomManager.field_Internal_Static_ApiWorldInstance_0.ownerId == UserID;
        }

        public static void ChangeAvatar(string avatarID)
        {
            if (avatarID.StartsWith("avtr_") && Utils.CurrentUser.GetAPIAvatar().id != avatarID)
            {
                var AviMenu = GameObject.Find("Screens").transform.Find("Avatar").GetComponent<PageAvatar>();
                AviMenu.field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0 = new ApiAvatar
                {
                    id = avatarID
                };
                AviMenu.ChangeToSelectedAvatar();
            }
        }

        public static bool IsBlocked(string userId)
        {
            if (!ModerationManager.prop_ModerationManager_0.field_Private_Dictionary_2_String_List_1_ApiPlayerModeration_0.ContainsKey(userId)) return false;
            foreach (ApiPlayerModeration Moderation in ModerationManager.prop_ModerationManager_0.field_Private_Dictionary_2_String_List_1_ApiPlayerModeration_0[userId])
            {
                if (Moderation.moderationType == ApiPlayerModeration.ModerationType.Block) return true;
            }
            return false;
        }

        public static bool IsMute(string userId)
        {
            if (!ModerationManager.prop_ModerationManager_0.field_Private_Dictionary_2_String_List_1_ApiPlayerModeration_0.ContainsKey(userId)) return false;
            foreach (ApiPlayerModeration Moderation in ModerationManager.prop_ModerationManager_0.field_Private_Dictionary_2_String_List_1_ApiPlayerModeration_0[userId])
            {
                if (Moderation.moderationType == ApiPlayerModeration.ModerationType.Mute) return true;
            }
            return false;
        }

        public static bool IsMaster(this Player Instance)
        {
            return Instance.GetVRCPlayerApi().isMaster;
        }

        public static bool GetIsBot(this Player Instance)
        {
            return Instance.transform.position == Vector3.zero;
        }

        public static bool GetIsFrozen(int Instance)
        {
            return PatchExtensions.NonSerializedActors.Contains(Instance);
        }

        public static short GetPing(this PlayerNet instance)
        {
            return instance.field_Private_Int16_0;
        }

        public static short GetPing(this Player instance)
        {
            return instance._playerNet.GetPing();
        }

        public static short GetPing(this VRCPlayer instance)
        {
            return instance._playerNet.GetPing();
        }

        public static string GetPingColored(this Player Instance)
        {
            string arg;
            if (Instance.GetPing() < 70) arg = "<color=#59D365>";
            else if (Instance.GetPing() < 140) arg = "<color=#FF7000>";
            else arg = "<color=red>";
            return string.Format("{0}{1}</color>", arg, Instance.GetPing());
        }

        public static string GetFramesColored(this Player Instance)
        {
            string arg;
            if (Instance.GetFrames() > 80) arg = "<color=#59D365>";
            else if (Instance.GetFrames() > 30) arg = "<color=#FF7000>";
            else arg = "<color=red>";
            return string.Format("{0}{1}</color>", arg, Instance.GetFrames());
        }

        public static byte GetQualityCounter(this PlayerNet instance)
        {
            return instance.field_Private_Byte_1;
        }

        public static byte GetQualityCounter(this VRCPlayer Instance)
        {
            return Instance._playerNet.GetQualityCounter();
        }

        public static byte GetQualityCounter(this Player Instance)
        {
            return Instance._playerNet.GetQualityCounter();
        }

        public static int GetFrames(this PlayerNet Instance)
        {
            return Instance.prop_Byte_0 == 0 ? 0 : 1000 / Instance.prop_Byte_0;
        }

        public static int GetFrames(this VRCPlayer Instance)
        {
            return Instance._playerNet.GetFrames();
        }

        public static int GetFrames(this Player Instance)
        {
            return Instance._playerNet.GetFrames();
        }

        public static GameObject GetAvatarObject(this Player Instance)
        {
            return Instance.GetVRCPlayer().GetAvatarObject();
        }

        public static GameObject GetAvatarObject(this VRCPlayer Instance)
        {
            return Instance.prop_VRCAvatarManager_0.prop_GameObject_0;
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

                case TrustRanks.MOD:
                    return VRCPlayer.field_Internal_Static_Color_8;

                case TrustRanks.NUISANCE:
                    return VRCPlayer.field_Internal_Static_Color_0;

                default:
                    return Color.black;
            }
        }

        public static float GetQuality(this PlayerNet Instance)
        {
            return Instance.prop_Single_0;
        }

        public static float GetQuality(this VRCPlayer Instance)
        {
            return Instance._playerNet.GetQuality();
        }

        public static Color GetQualityColor(this VRCPlayer Instance)
        {
            return Color.Lerp(Color.red, Color.green, Instance.GetQuality());
        }

        public static TrustRanks GetRank(this APIUser Instance)
        {
            if (Instance.hasModerationPowers || Instance.hasSuperPowers || Instance.hasVIPAccess || Instance.tags.ToString().Contains("admin_") || Instance.developerType != APIUser.DeveloperType.None) return TrustRanks.MOD;
            else if (Instance.hasVeteranTrustLevel) return TrustRanks.TRUSTED;
            else if (Instance.hasTrustedTrustLevel) return TrustRanks.KNOWN;
            else if (Instance.hasKnownTrustLevel) return TrustRanks.USER;
            else if (Instance.hasBasicTrustLevel || Instance.isNewUser) return TrustRanks.NEW;
            else if (Instance.hasNegativeTrustLevel) return TrustRanks.NUISANCE;
            return TrustRanks.VISITOR;
        }

        public enum TrustRanks
        {
            TRUSTED,
            KNOWN,
            USER,
            NEW,
            NUISANCE,
            VISITOR,
            MOD,
        }
    }
}