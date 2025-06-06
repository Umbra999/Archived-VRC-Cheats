using System.Collections;
using UnityEngine;
using VRC.UserCamera;

namespace WengaPort.Modules
{
    class NoClipping
    {
        public static void ChangeNearClipPlane(float value)
        {
            VRCVrCamera vrCamera = VRCVrCamera.field_Private_Static_VRCVrCamera_0;
            if (!vrCamera)
                return;
            Camera screenCamera = vrCamera.field_Public_Camera_0;
            if (!screenCamera)
                return;
            screenCamera.nearClipPlane = value;
            ChangePhotoCameraNearField(value); ;
        }
        public static void ChangePhotoCameraNearField(float value)
        {
            var cameraController = UserCameraController.field_Internal_Static_UserCameraController_0;
            if (cameraController == null)
                return;
            Camera cam = cameraController.field_Public_GameObject_1.GetComponent<Camera>();
            if (cam != null)
                cam.nearClipPlane = value;
        }

        public static IEnumerator SetNearClipPlane(float znear)
        {
            yield return new WaitForSecondsRealtime(6);
            Extensions.Logger.WengaLogger("[Utils] Clipping Adjusted");
            ChangeNearClipPlane(znear);
        }
    }
}
