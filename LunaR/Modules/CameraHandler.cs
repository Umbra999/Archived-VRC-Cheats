using LunaR.Wrappers;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.XR;
using VRC;
using VRC.UserCamera;

namespace LunaR.Modules
{
    internal class CameraHandler
    {
        public static void TakePicture(int timer)
        {
            Utils.UserCameraController.prop_Int32_1 = 0;
            Utils.UserCameraController.StartCoroutine(Utils.UserCameraController.Method_Private_IEnumerator_Int32_PDM_0(timer));
        }

        public static void Reset()
        {
            Utils.UserCameraController.StopAllCoroutines();
        }

        public static void ToggleCamera()
        {
            if (Utils.UserCameraController.prop_UserCameraMode_0 == UserCameraMode.Off) EnableCamera();
            else DisablepCamera();
        }

        public static void EnableCamera()
        {
            Utils.UserCameraController.prop_UserCameraMode_0 = UserCameraMode.Photo;
        }

        public static void DisablepCamera()
        {
            Utils.UserCameraController.prop_UserCameraMode_0 = UserCameraMode.Off;
        }

        public static void TakePictureRPC(Player Target)
        {
            Utils.UserCameraController.field_Internal_UserCameraIndicator_0.PhotoCapture(Target);
        }

        public static bool _isZoomed = false;

        public static bool UserCamAnnoy = false;
        public static Player UserCamAnnoyTarget;

        public static bool CameraLag = false;
    }
}