using Transmtn.DTO;
using Transmtn.DTO.Notifications;
using WengaPort.Modules;

namespace WengaPort.Extensions
{
    static class NotificationHandler
    {
		public static void DismissNotification(this NotificationManager Instance, Notification notification)
		{
			Utils.VRCWebSocketsManager.DismissNotification(notification);
		}

		public static void DismissNotification(this VRCWebSocketsManager Instance, Notification notification)
		{
			Instance.prop_Api_0.PostOffice.MarkAsSeen(notification);
			Instance.prop_Api_0.PostOffice.Hide(notification);
		}

		public static void SendNotification(this VRCWebSocketsManager Instance, Notification notification)
		{
			Instance.prop_Api_0.PostOffice.Send(notification);
		}

		public static void SendInvite(this VRCWebSocketsManager Instance, string Message, string UserID)
		{
			Instance.prop_Api_0.PostOffice.Send(Invite.Create(UserID, "", new Location("", new Instance("", UserID, "", "", "", false)), Message));
		}
		public static Notification Notification(this QuickMenu Instance)
		{
			return Instance.field_Private_Notification_0;
		}
	}
}
