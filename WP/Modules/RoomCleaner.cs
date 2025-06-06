using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC.SDKBase;
using Object = UnityEngine.Object;

namespace WengaPort.Modules
{
	class RoomCleaner
	{
		public static void CleanRoom()
		{
			try
			{
				GameObject gameObject = (from x in Object.FindObjectsOfType<GameObject>()
										 where x.name == "DrawingManager"
										 select x).First();
				Networking.RPC(0, gameObject, "CleanRoomRPC", null);
			}
			catch (Exception)
			{
				Extensions.Logger.WengaLogger("Failed to Clean Room");
			}
		}
		public static void ChangePedestals(string ID)
		{
			foreach (VRC_AvatarPedestal vrc_AvatarPedestal in Object.FindObjectsOfType<VRC_AvatarPedestal>())
			{
				Networking.RPC(0, vrc_AvatarPedestal.gameObject, "SwitchAvatar", new Il2CppSystem.Object[]
				{
					ID
				});
			}
		}

		public static bool MirrorSpam = false;
		public static IEnumerator SpamMirrors()
		{
			for (; ; )
            {
				if (!MirrorSpam || RoomManager.field_Internal_Static_ApiWorld_0 == null) yield break;
				try
				{
					List<VRC_Trigger> list = (from s in ItemHandler.World_Triggers where s.interactText.ToLower().Contains("mirror") || s.name.ToLower().Contains("mirror") select s).ToList<VRC_Trigger>();
					foreach (VRC_Trigger vrc_Trigger in list)
					{
						vrc_Trigger.TakesOwnershipIfNecessary.ToString();
						vrc_Trigger.Interact();
					}
				}
				catch { }
				yield return new WaitForSeconds(1);
			}
		}
	}
}

