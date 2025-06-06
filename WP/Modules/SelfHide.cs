using UnityEngine;

namespace WengaPort.Modules
{
    class SelfHide
    {
		public static void Initialize(bool State)
		{
			Utils.CurrentUser.transform.Find("ForwardDirection").gameObject.active = !State;
		}

		public static void Invisible()
		{
			for (int i = 0; i < 20; i++)
			{
				VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = new Vector3(999999999999 * 2, 999999999999 * 2, 999999999999 * 2);
				VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = new Vector3(999999999999 * 2, 999999999999 * 2, 999999999999 * 2);
				VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = new Vector3(999999999999 * 2, 999999999999 * 2, 999999999999 * 2);
				VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = new Vector3(999999999999 * 2, 999999999999 * 2, 999999999999 * 2);
				VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = new Vector3(999999999999 * 2, 999999999999 * 2, 999999999999 * 2);
			}
		}
	}
}
