using System.Collections.Generic;
using System.Linq;
using Transmtn.DTO.Notifications;
using VRC.Core;
using ModerationManager = VRC.Management.ModerationManager;
using Logger = WengaPort.Extensions.Logger;

namespace WengaPort.Modules
{
    static class Moderations
    {     
        public static bool GetIsBlocked(this ModerationManager instance, string UserID)
        {
            return ModerationFromMe != null &&
                ModerationFromMe.Exists((ApiPlayerModeration m)
                => m != null && m.moderationType == ApiPlayerModeration.ModerationType.Block && m.targetUserId == UserID);
        }
        public static bool GetIsBlockedByUser(this ModerationManager instance, string UserID)
        {
            return ModerationAgainstMe != null &&
                ModerationAgainstMe.Exists((ApiPlayerModeration m)
                => m != null && m.moderationType == ApiPlayerModeration.ModerationType.Block && m.sourceUserId == UserID);
        }
        public static bool IsBannedFromPublicOnly(this ModerationManager instance, string userId)
        {
            return APIUser.CurrentUser.id == userId || GlobalModerations != null
                && GlobalModerations.Exists((ApiModeration m)
                => m != null && m.moderationType == ApiModeration.ModerationType.BanPublicOnly && m.targetUserId == userId);
        }
        public static bool GetIsKickedFromWorld(this ModerationManager instance, string userId, string worldId, string instanceId)
        {
            return GlobalModerations.Exists((ApiModeration m) => m.moderationType == ApiModeration.ModerationType.Kick && m.worldId == worldId && m.instanceId == instanceId && m.targetUserId == userId);
        }

        public static bool IsBlockedEitherWay(string userId)
        {
            if (userId == null) return false;

            var moderationManager = ModerationManager.prop_ModerationManager_0;
            if (moderationManager == null) return false;
            if (APIUser.CurrentUser?.id == userId)
                return false;

            foreach (var playerModeration in moderationManager.field_Private_List_1_ApiPlayerModeration_0)
            {
                if (playerModeration != null && playerModeration.moderationType == ApiPlayerModeration.ModerationType.Block && playerModeration.targetUserId == userId)
                    return true;
            }

            return false;
        }

        public static bool GetIsMuted(this ModerationManager instance, string UserID)
        {
            if (UserID == APIUser.CurrentUser.id)
                return false;
            bool flag = ModerationAgainstMe.Exists((ApiPlayerModeration m)
                => m.moderationType == ApiPlayerModeration.ModerationType.Mute && m.sourceUserId == UserID);
            if (flag)
            {
                return true;
            }
            bool flag2 = ModerationAgainstMe.Exists((ApiPlayerModeration m)
                => m.moderationType == ApiPlayerModeration.ModerationType.Unmute && m.sourceUserId == UserID);
            if (flag2)
            {
                return false;
            }
            return false;
        }
        public static bool GetIsMutedByUser(this ModerationManager instance, string userId)
        {
            if (userId == APIUser.CurrentUser.id)
            {
                return false;
            }
            bool flag = ModerationFromMe.Exists((ApiPlayerModeration m) => m.moderationType == ApiPlayerModeration.ModerationType.Mute && m.sourceUserId == userId);
            if (flag)
            {
                return true;
            }
            bool flag2 = ModerationFromMe.Exists((ApiPlayerModeration m) => m.moderationType == ApiPlayerModeration.ModerationType.Unmute && m.sourceUserId == userId);
            if (flag2)
            {
                return false;
            }
            return false;
        }
        public static int GetModerationAgainstYOU(this ModerationManager instance, ApiPlayerModeration.ModerationType type)
        {
            return ModerationAgainstMe.Where((ApiPlayerModeration m) => m.moderationType == type).Count();
        }
        public static int GetModerationFromYOU(this ModerationManager instance, ApiPlayerModeration.ModerationType type)
        {
            return ModerationFromMe.Where((ApiPlayerModeration m) => m.moderationType == type).Count();
        }

        private static List<ApiPlayerModeration> ModerationAgainstMe
        {
            get
            {
                return Utils.ModerationManager.field_Private_List_1_ApiPlayerModeration_0.ToArray().ToList();
            }
        }
        private static List<ApiPlayerModeration> ModerationFromMe
        {
            get
            {
                return Utils.ModerationManager.field_Private_List_1_ApiPlayerModeration_0.ToArray().ToList();
            }
        }
        private static List<ApiModeration> GlobalModerations
        {
            get
            {
                return Utils.ModerationManager.field_Private_List_1_ApiModeration_0.ToArray().ToList();
            }
        }

        public static void SendInvite(string WorldName, string WorldID, string playerID)
        {
            try
            {
                NotificationDetails notificationDetails = new NotificationDetails();
                notificationDetails.Add("worldId", WorldID);
                notificationDetails.Add("worldName", WorldName);
                Notification notification = Notification.Create(playerID, Notification.NotificationType.Invite, WorldName, notificationDetails);
                VRCWebSocketsManager.field_Private_Static_VRCWebSocketsManager_0.prop_Api_0.PostOffice.Send(notification);
                Logger.WengaLogger("Send Invite to:" + playerID);
            }
            catch
            {
                Logger.WengaLogger("Invite Failed");
            }
        }
    }
}
