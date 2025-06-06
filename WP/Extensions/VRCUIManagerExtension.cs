using System;
using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase;
using VRC.UI;
using WengaPort.Api;
using UnityEngine.UI;
using WengaPort.Modules;
using Object = UnityEngine.Object;

namespace WengaPort.Extensions
{
	public static class VRCUiManagerExtension
	{
		public static APIUser SelectedAPIUser(this VRCUiManager Instance)
		{
			return Instance.prop_VRCUiPopupManager_0.GetComponentInChildren<PageUserInfo>().field_Public_APIUser_0;
		}

		public static void QueHudMessage(this VRCUiManager instance, string Message)
		{
			if (RoomManager.field_Internal_Static_ApiWorld_0 == null) return;
			else if (HudMessage1 == null)
            {
				HudMessage1 = CreateTextNear(CreateImage("WengaPortHud", -300f), 55f, TextAnchor.LowerLeft);
			}
			MelonCoroutines.Start(ShowMessage(HudMessage1, MessagesList, Message));
		}

		public static Image CreateImage(string name, float offset)				
		{
			var hudRoot = GameObject.Find("UserInterface/UnscaledUI/HudContent/Hud");
			var requestedParent = hudRoot.transform.Find("NotificationDotParent");
			var indicator = Object.Instantiate(hudRoot.transform.Find("NotificationDotParent/NotificationDot").gameObject, requestedParent, false).Cast<GameObject>();
			indicator.name = "NotifyDot-" + name;
			indicator.SetActive(true);
			indicator.transform.localPosition += Vector3.right * offset;
			var image = indicator.GetComponent<Image>();
			image.enabled = false;
			return image;
		}

		public static List<string> MessagesList = new List<string>();
		public static Text CreateTextNear(Image image, float offset, TextAnchor alignment)
		{
			var gameObject = new GameObject(image.gameObject.name + "-text");
			gameObject.AddComponent<Text>();
			gameObject.transform.SetParent(image.transform, false);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.up * offset + Vector3.right * 300f;
			var text = gameObject.GetComponent<Text>();
			text.color = Color.white;
			text.fontStyle = FontStyle.Bold;
			text.horizontalOverflow = HorizontalWrapMode.Overflow;
			text.verticalOverflow = VerticalWrapMode.Overflow;
			text.alignment = alignment;
			text.fontSize = 25;
			text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
			text.supportRichText = true;

			gameObject.SetActive(true);
			return text;
		}
		public static Text HudMessage1;
		public static System.Collections.IEnumerator ShowMessage(Text text, List<string> MessagesList, string message)
		{
			MessagesList.Add(message);
			text.text = string.Join("\n", MessagesList);
			yield return new WaitForSeconds(5.5f);
			MessagesList.Remove(message);
			text.text = string.Join("\n", MessagesList);
		}

		public static void CloseUI(this VRCUiManager Instance, bool withFade = false)
		{
			try
			{
				Instance.Method_Public_Void_Boolean_0(withFade);
			}
			catch { }
		}
		public static VRCUiPage GetPage(this VRCUiManager Instance, string screenPath)
		{
			GameObject gameObject = GameObject.Find(screenPath);
			VRCUiPage vrcuiPage = null;
			if (gameObject != null)
			{
				vrcuiPage = gameObject.GetComponent<VRCUiPage>();
				if (vrcuiPage == null)
				{
                    Logger.WengaLogger("Screen Not Found - " + screenPath);
				}
			}
			else
			{
                Logger.WengaLogger("Screen Not Found - " + screenPath);
			}
			return vrcuiPage;
		}
		public static void HideCurrentPopUp(this VRCUiPopupManager instance)
		{
			Utils.VRCUiManager.HideScreen("POPUP");
		}

		public static void Alert(this VRCUiPopupManager instance, string title, string Content, string buttonname, Action action, string button2, Action action2)
		{
			instance.Method_Public_Void_String_String_String_Action_String_Action_Action_1_VRCUiPopup_1(title, Content, buttonname, action, button2, action2, null);
		}

		public static List<GameObject> GetWorldPrefabs()
		{
			return VRC_SceneDescriptor.Instance.DynamicPrefabs.ToArray().ToList();
		}

		public static List<Material> GetWorldMaterials()
		{
			return VRC_SceneDescriptor.Instance.DynamicMaterials.ToArray().ToList();
		}

		public static void SelectPrefab(Action<GameObject> PrefabAction)
		{
			List<GameObject> Prefabs = GetWorldPrefabs();
			QMNestedButton PrefabsPage = new QMNestedButton("ShortcutMenu", -100, -100, "", "");
			ScrollMenu PrefabsScroll = new ScrollMenu(PrefabsPage);
			PrefabsScroll.SetAction(delegate
			{
				foreach (var prefab in Prefabs)
				{
					PrefabsScroll.Add(new QMSingleButton(PrefabsPage, 0, 0, prefab.name, delegate
					{
						PrefabAction.Invoke(prefab);
					}, prefab.name));
				}
			});
			PrefabsPage.getMainButton().getGameObject().GetComponent<Button>().onClick.Invoke();
			PrefabsPage.DestroyMe();
		}
	}
}
