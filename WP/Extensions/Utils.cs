using UnityEngine;
using VRC;
using VRC.UserCamera;
using ModerationManager = VRC.Management.ModerationManager;
using System.Collections.Generic;
using UnhollowerRuntimeLib.XrefScans;
using System.Reflection;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WengaPort.Modules
{
	static class Utils
	{
		public static VRCUiPopupManager VRCUiPopupManager
		{
			get
			{
				return VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0;
			}
		}

		public static VRCUiManager VRCUiManager
		{
			get
			{
				return VRCUiManager.prop_VRCUiManager_0;
			}
		}

		public static ModerationManager ModerationManager
		{
			get
			{
				return ModerationManager.prop_ModerationManager_0;
			}
		}

		public static NotificationManager NotificationManager
		{
			get
			{
				return NotificationManager.field_Private_Static_NotificationManager_0;
			}
		}

		public static VRCWebSocketsManager VRCWebSocketsManager
		{
			get
			{
				return VRCWebSocketsManager.field_Private_Static_VRCWebSocketsManager_0;
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
				return PlayerManager.field_Private_Static_PlayerManager_0;
			}
		}

		public static VRCPlayer CurrentUser
		{
			get
			{
				return VRCPlayer.field_Internal_Static_VRCPlayer_0;
			}
			set
			{
                CurrentUser = CurrentUser;
			}
		}

		public static UserInteractMenu UserInteractMenu
		{
			get
			{
				return Resources.FindObjectsOfTypeAll<UserInteractMenu>()[0];
			}
		}

		public static GameObject GetAvatarObject(this Player p) => p.prop_VRCPlayer_0.prop_VRCAvatarManager_0.prop_GameObject_0;
		public static GameObject GetAvatarObject(this VRCPlayer p) => p.prop_VRCAvatarManager_0.prop_GameObject_0;

		public static Camera Camera
		{
			get
			{
				return VRCVrCamera.field_Private_Static_VRCVrCamera_0.field_Public_Camera_0;
			}
		}

		public static VRCVrCamera VRCVrCamera
		{
			get
			{
				return VRCVrCamera.field_Private_Static_VRCVrCamera_0;
			}
		}

		public static UserCameraController UserCameraController
		{
			get
			{
				return UserCameraController.field_Internal_Static_UserCameraController_0;
			}
		}

		public static VRCTrackingManager VRCTrackingManager
		{
			get
			{
				return VRCTrackingManager.field_Private_Static_VRCTrackingManager_0;
			}
		}
		public static QuickMenu QuickMenu
		{
			get
			{
				return QuickMenu.prop_QuickMenu_0;
			}
		}
        public static object GeneralWrappers { get; internal set; }

		public static readonly List<string> EmojiType = new List<string>
		{
			":D",
			"Like",
			"Heart",
			"D:",
			"Dislike",
			"!!!",
			"xD",
			"=O",
			"???",
			":*",
			"*-*",
			">:{",
			"=|",
			"^P",
			"O_O",
			"=.=",
			"Fire",
			"M-moneys",
			"</3",
			"Present",
			"Beer",
			"Tomato",
			"Zzz...",
			"Thinking...",
			"Pizza",
			"Sunglasses",
			"Music Note",
			"GO!",
			"Wave",
			"Stop",
			"Clouds",
			"Pumpkin",
			"Spooky Ghost",
			"Skull",
			"Sweet-Candy",
			"Candy-Corn",
			"Boo!!",
			"Scary-bats",
			"Spider-Web",
			"",
			"Mistletoe",
			"Snowball",
			"Snowflake",
			"Coal",
			"Candy Cane",
			"Gingerbread",
			"Confetti",
			"Champagne",
			"Presents",
			"Beach-Ball",
			"Drink",
			"Hang Ten",
			"Ice Cream",
			"Life Ring",
			"Neon Shades",
			"Pineapple",
			"Splash",
			"Sun Lotion"
		};

		public static readonly List<string> EmoteType = new List<string>
		{
			"",
			"wave",
			"clap",
			"point",
			"cheer",
			"dance",
			"backflip",
			"die",
			"sadness"
		};
		internal static bool XRefScanForMethod(this MethodBase methodBase, string methodName = null, string reflectedType = null)
		{
			bool flag = false;
			foreach (XrefInstance xrefInstance in XrefScanner.XrefScan(methodBase))
			{
				if (xrefInstance.Type == XrefType.Method)
				{
					MethodBase methodBase2 = xrefInstance.TryResolve();
					if (!(methodBase2 == null))
					{
						if (!string.IsNullOrEmpty(methodName))
						{
							flag = (!string.IsNullOrEmpty(methodBase2.Name) && methodBase2.Name.IndexOf(methodName, StringComparison.OrdinalIgnoreCase) >= 0);
						}
						if (!string.IsNullOrEmpty(reflectedType))
						{
							Type reflectedType2 = methodBase2.ReflectedType;
							flag = (!string.IsNullOrEmpty((reflectedType2 != null) ? reflectedType2.Name : null) && methodBase2.ReflectedType.Name.IndexOf(reflectedType, StringComparison.OrdinalIgnoreCase) >= 0);
						}
						if (flag)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
		public class Serialization
		{
			public static byte[] ToByteArray(Il2CppSystem.Object obj)
			{
				if (obj == null) return null;
				var bf = new Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				var ms = new Il2CppSystem.IO.MemoryStream();
				bf.Serialize(ms, obj);
				return ms.ToArray();
			}
			public static byte[] ToByteArray(object obj)
			{
				if (obj == null) return null;
				var bf = new BinaryFormatter();
				var ms = new MemoryStream();
				bf.Serialize(ms, obj);
				return ms.ToArray();
			}

			public static T FromByteArray<T>(byte[] data)
			{
				if (data == null) return default(T);
				BinaryFormatter bf = new BinaryFormatter();
				using (MemoryStream ms = new MemoryStream(data))
				{
					object obj = bf.Deserialize(ms);
					return (T)obj;
				}
			}
			public static T IL2CPPFromByteArray<T>(byte[] data)
			{
				if (data == null) return default(T);
				var bf = new Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				var ms = new Il2CppSystem.IO.MemoryStream(data);
				object obj = bf.Deserialize(ms);
				return (T)obj;
			}

			public static T FromIL2CPPToManaged<T>(Il2CppSystem.Object obj)
			{
				return FromByteArray<T>(ToByteArray(obj));
			}

			public static T FromManagedToIL2CPP<T>(object obj)
			{
				return IL2CPPFromByteArray<T>(ToByteArray(obj));
			}

			public static object[] FromIL2CPPArrayToManagedArray(Il2CppSystem.Object[] obj)
			{
				object[] Parameters = new object[obj.Length];
				for (int i = 0; i < obj.Length; i++)
					Parameters[i] = FromIL2CPPToManaged<object>(obj[i]);
				return Parameters;
			}
			public static Il2CppSystem.Object[] FromManagedArrayToIL2CPPArray(object[] obj)
			{
				Il2CppSystem.Object[] Parameters = new Il2CppSystem.Object[obj.Length];
				for (int i = 0; i < obj.Length; i++)
					Parameters[i] = FromManagedToIL2CPP<Il2CppSystem.Object>(obj[i]);
				return Parameters;
			}
		}
	}
}
