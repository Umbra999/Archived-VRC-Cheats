using ExitGames.Client.Photon;
using Photon.Pun;
using UnhollowerBaseLib;
using UnityEngine;
using VRC;
using VRC.SDKBase;
using RootMotion.FinalIK;
using RealisticEyeMovements;
using System.Linq;
using System.Collections;
using System.Threading;
using WengaPort.Wrappers;
using static VRC.SDKBase.VRC_EventHandler;
using System.IO;
using VRC.Core;
using System.Collections.Generic;

namespace WengaPort.Modules
{
    internal static class PhotonModule
    {
		public static bool EmojiSpam = false;
		public static void EmojiRPC(int i)
		{
			try
			{
                Il2CppSystem.Int32 @int = default;
				@int.m_value = i;
                Il2CppSystem.Object @object = @int.BoxIl2CppObject();
				Networking.RPC(0, Utils.CurrentUser.gameObject, "SpawnEmojiRPC", new Il2CppSystem.Object[]
				{
					@object
				});
			}
			catch { }
		}

		public static IEnumerator EmojiSpammer()
        {
			for (; ; )
            {
				if (!EmojiSpam || RoomManager.field_Internal_Static_ApiWorld_0 == null) yield break;
				EmojiRPC(29);
				yield return new WaitForEndOfFrame();
            }
        }

		public static List<string> RPCBlock = new List<string>();
		public static List<string> EventBlock = new List<string>();
		public static bool EmoteLagToggle = false;

		public static IEnumerator EmoteLag()
        {
			for (; ; )
            {
				if (!EmoteLagToggle | RoomManager.field_Internal_Static_ApiWorld_0 == null)
                {
					yield break;
                }
				for (int i = 0; i < 2; i++)
				{
					EmojiRPC(int.MinValue);
					EmojiRPC(int.MinValue);
					EmojiRPC(int.MinValue);
				}
                yield return new WaitForEndOfFrame();
			}
		}

		public static string[] WorldArray;
		public static bool WorldTravel = false;
		public static IEnumerator RankUp()
		{
			if (WorldArray == null)
			{
				WorldArray = File.ReadAllLines("WengaPort\\Photon\\Worlds.txt");
			}
			for (; ; )
			{
				if (!WorldTravel) yield break;
				string World = WorldArray[new System.Random().Next(0, WorldArray.Length - 1)];
				int RandString = new System.Random().Next(0, 10000);
				Join(World, $"{RandString}");
				yield return new WaitForSeconds(1);
			}
			yield break;
		}

		public static void Join(string WorldID, string InstanceID)
		{
			Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object> DickTionary = new Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>();
			DickTionary.Add("userId", APIUser.CurrentUser.id);
			DickTionary.Add("worldId", $"{WorldID}:{InstanceID}");
            API.SendPutRequest("joins", null, DickTionary, null);
            API.SendPutRequest("visits", null, DickTionary, null);
		}

		public static void EmoteRPC(int i)
		{
			try
			{
				Il2CppSystem.Int32 @int = default;
				@int.m_value = i;
				Il2CppSystem.Object @object = @int.BoxIl2CppObject();
				Networking.RPC(0, Utils.CurrentUser.gameObject, "PlayEmoteRPC", new Il2CppSystem.Object[]
				{
					@object
				});
			}
			catch { }
		}

		public static GameObject Capsule = new GameObject();
		public static bool Serialize = false;
		public static bool Invisible = false;
		public static bool LockInstance = false;
		public static bool Forcemute = false;
		public static void CustomSerialize(bool Toggle)
		{
			try
			{
				if (Toggle)
				{
					Serialize = true;
					Capsule = Object.Instantiate(Utils.CurrentUser.prop_VRCAvatarManager_0.prop_GameObject_0, null, true);
					Animator component = Capsule.GetComponent<Animator>();
					if (component != null && component.isHuman)
					{
						Transform boneTransform = component.GetBoneTransform((HumanBodyBones)10);
						if (boneTransform != null)
						{
							boneTransform.localScale = Vector3.one;
						}
					}
					Capsule.name = "Serialize Capsule";
					component.enabled = false;
					Capsule.GetComponent<FullBodyBipedIK>().enabled = false;
					Capsule.GetComponent<LimbIK>().enabled = false;
					Capsule.GetComponent<VRIK>().enabled = false;
					Capsule.GetComponent<LookTargetController>().enabled = false;
					Capsule.transform.position = Utils.CurrentUser.transform.position;
					Capsule.transform.rotation = Utils.CurrentUser.transform.rotation;
				}
				else
				{
                    Object.Destroy(Capsule);
					Serialize = false;
				}
			}
			catch { }
		}

		private static string RandomString(int length)
		{
			char[] array = "abcdefghlijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789".ToArray();
			string text = string.Empty;
			Il2CppSystem.Random random = new Il2CppSystem.Random(new Il2CppSystem.Random().Next(length));
			for (int i = 0; i < length; i++)
			{
				text += array[random.Next(array.Length)].ToString();
			}
			return text;
		}

		public static string RandomNumberString(int length)
		{
			string text = string.Empty;
			for (int i = 0; i < length; i++)
			{
				text += new System.Random().Next(0, int.MaxValue).ToString("X8");
			}
			return text;
		}

		public static bool DisconnectToggle = false;
		public static bool DebugSpamToggle = false;

		private static VRC_EventHandler handler;

		public static IEnumerator DisconnectLobby()
		{
			for (; ; )
            {
				if (!DisconnectToggle || RoomManager.field_Internal_Static_ApiWorld_0 == null) yield break;
				else if (handler == null)
				{
					handler = Resources.FindObjectsOfTypeAll<VRC_EventHandler>()[0];
				}
				VrcEvent vrcEvent = new VrcEvent
				{
					EventType = (VrcEventType)14,
					ParameterObject = handler.gameObject,
					ParameterInt = 1,
					ParameterFloat = 0f,
					ParameterString = "<3 WengaPort <3 | " + RandomString(820) + " | <3 WengaPort <3",
					ParameterBoolOp = (VrcBooleanOp)(-1),
					ParameterBytes = new Il2CppStructArray<byte>(0L)
				};
				int Type = 0;
				Player player = PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.ToArray()[new Il2CppSystem.Random().Next(0, PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.Count)];
				handler.TriggerEvent(vrcEvent, (VrcBroadcastType)Type, player.gameObject, 0f);
				Player player2 = PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.ToArray()[new Il2CppSystem.Random().Next(0, PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.Count)];
				handler.TriggerEvent(vrcEvent, (VrcBroadcastType)Type, player2.gameObject, 0f);
				yield return new WaitForSeconds(0.2f);
			}
		}

		public static void DebugSpam()
		{
			for (int i = 0; i < 3; i++)
			{
				SendRPC(VrcEventType.SendRPC, "SendRPC", GameObject.Find("Camera (eye)").gameObject, 1, 0, "Get Fucked Russian Debug -Wenga#0666 L̛̛̾̈́̈̋͛͊̍͛̆̑̐̉̒̈̀̋̉̇̄͐͆͛͆́́̐͆̃̉̿́̀̐͋͐̃̎̅̊̀̌̾̎̓̽͛̑̃̿̈́͐̀̉̍͐̀͋̆̑̌̑̓̆̍̏͆̔̍͗̇́͋̓̍́̾͊̅̍̃̆͌̃͑͐̀̿̈́́̕͘̕͘̕̕͘͝͠͠͞͝͞͝͝͞͞͞͞͝͞L̛̛̾̈́̈̋͛͊̍͛̆̑̐̉̒̈̀̋̉̇̄͐͆͛͆́́̐͆̃̉̿́̀̐͋͐̃̎̅̊̀̌̾̎̓̽͛̑̃̿̈́͐̀̉̍͐̀͋̆̑̌̑̓̆̍̏͆̔̍͗̇́͋̓̍́̾͊̅̍̃̆͌̃͑͐̀̿̈́́̕͘̕͘̕̕͘͝͠͠͞͝͞͝͝͞͞͞͞͝͞", VrcBooleanOp.Unused, VrcBroadcastType.AlwaysUnbuffered);
			}
		}

		public static void EventSpammer(this int count, int amount, System.Action action, int? sleep = null)
		{
			for (int ii = 0; ii < count; ii++)
			{
				for (int i = 0; i < amount; i++)
					action();
				if (sleep != null)
					Thread.Sleep(sleep.Value);
				else
					Thread.Sleep(25);
			}
		}

		public static void SendRPC(VrcEventType EventType, string Name, GameObject ParamObject, int Int, float Float, string String, VrcBooleanOp Bool, VrcBroadcastType BroadcastType)
		{
			if (handler == null)
			{
				handler = Resources.FindObjectsOfTypeAll<VRC_EventHandler>()[0];
			}
			VrcEvent a = new VrcEvent
			{
				EventType = EventType,
				Name = Name,
				ParameterObject = ParamObject,
				ParameterInt = Int,
				ParameterFloat = Float,
				ParameterString = String,
				ParameterBoolOp = Bool,
			};
			foreach (var Player in Utils.PlayerManager.GetAllPlayers())
			{
				handler.TriggerEvent(a, BroadcastType, Player.gameObject, 0f);
			}
		}
	}
}
