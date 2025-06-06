using UnityEngine;

namespace WengaPort.Modules
{
    class Headlight
	{
		public static void Toggle(bool state)
		{
			Transform boneTransform = Utils.CurrentUser.field_Internal_Animator_0.GetBoneTransform((HumanBodyBones)10);
			bool flag = boneTransform == null;
			if (!flag)
			{
				Light component = boneTransform.GetComponent<Light>();
				if (state)
				{
					Light light = boneTransform.gameObject.AddComponent<Light>();
					light.color = Color;
					light.type = (LightType)2;
					light.shadows = 0;
					light.range = Range;
					light.spotAngle = float.MaxValue;
					light.intensity = Intensity;
				}
				else
				{
                    Object.Destroy(component);
				}
			}
		}

		public static float Range = 20f;

		public static float Intensity = 1f;

		public static float Point = 2f;

		public static Color Color = Color.white;
	}
}
