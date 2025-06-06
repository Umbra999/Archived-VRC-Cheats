using System;
using System.Linq;
using UnhollowerBaseLib;
using UnityEngine;
using VRC.SDKBase;

namespace WengaPort.Modules
{
    class ESP : MonoBehaviour
    {
		public static void HighlightColor(Color highlightcolor)
		{
			bool flag = Resources.FindObjectsOfTypeAll<HighlightsFXStandalone>().Count != 0;
			if (flag)
			{
				Resources.FindObjectsOfTypeAll<HighlightsFXStandalone>().FirstOrDefault().highlightColor = highlightcolor;
			}
		}

		public static void PickupESP(bool state)
		{
			Il2CppArrayBase<VRC_Pickup> il2CppArrayBase = Resources.FindObjectsOfTypeAll<VRC_Pickup>();
			foreach (VRC_Pickup vrc_Pickup in il2CppArrayBase)
			{
				bool flag = !(vrc_Pickup == null) && !(vrc_Pickup.gameObject == null) && vrc_Pickup.gameObject.active && vrc_Pickup.enabled && vrc_Pickup.pickupable && !vrc_Pickup.name.Contains("ViewFinder") && !(HighlightsFX.prop_HighlightsFX_0 == null);
				if (flag)
				{
					HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(vrc_Pickup.GetComponentInChildren<MeshRenderer>(), state);
				}
			}
		}

		public void Update()
		{
			if (ESPEnabled)
			{
				foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
				{
					if (gameObject.transform.Find("SelectRegion"))
					{
						HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(gameObject.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
					}
				}
				wasEnabled = true;
			}
			else if (!ESPEnabled && wasEnabled)
			{
				foreach (GameObject gameObject2 in GameObject.FindGameObjectsWithTag("Player"))
				{
					if (gameObject2.transform.Find("SelectRegion"))
					{
						HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(gameObject2.transform.Find("SelectRegion").GetComponent<Renderer>(), false);
					}
				}
				wasEnabled = false;
			}
		}
		public static bool ESPEnabled = false;
		private static bool wasEnabled;

		public ESP(IntPtr ptr) : base(ptr) { }
	}
}

