using LunaR.Wrappers;
using System;
using System.Linq;
using UnityEngine;

namespace LunaR.Modules
{
    internal class KeyBindHandler : MonoBehaviour
    {
        public KeyBindHandler(IntPtr ptr) : base(ptr)
        {
        }

        public void Update()
        {
            if (!GeneralWrappers.IsInWorld()) return;

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.K)) CameraHandler.ToggleCamera();
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.O))
            {
                Movement.SpinBot = !Movement.SpinBot;
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Backspace)) SpawnManager.prop_SpawnManager_0.Method_Public_Void_VRCPlayer_0(Utils.CurrentUser);
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray r = new(Camera.main.transform.position, Camera.main.transform.forward);
                if (Physics.Raycast(r, out RaycastHit raycastHit)) Utils.CurrentUser.transform.position = raycastHit.point;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                RPCHandler.EmojiRPC(55);
                RPCHandler.EmoteRPC(3);
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha0))
            {
                Resources.FindObjectsOfTypeAll<DebugLogGui>().First().field_Public_Boolean_0 = !Resources.FindObjectsOfTypeAll<DebugLogGui>().First().field_Public_Boolean_0;
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha1))
            {
                PlayerExtensions.ChangeAvatar("avtr_c38a1615-5bf5-42b4-84eb-a8b6c37cbd11");
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha9))
            {
                Resources.FindObjectsOfTypeAll<DebugTextGUI>().First().field_Public_Boolean_0 = !Resources.FindObjectsOfTypeAll<DebugTextGUI>().First().field_Public_Boolean_0;
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha8))
            {
                Resources.FindObjectsOfTypeAll<VRCNetworkInfoGUI>().First().field_Private_Boolean_0 = !Resources.FindObjectsOfTypeAll<VRCNetworkInfoGUI>().First().field_Private_Boolean_0;
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha7))
            {
                Resources.FindObjectsOfTypeAll<MonoBehaviour2PublicObVeBoReUnique>().First().field_Private_Boolean_0 = !Resources.FindObjectsOfTypeAll<MonoBehaviour2PublicObVeBoReUnique>().First().field_Private_Boolean_0;
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha6))
            {
                Resources.FindObjectsOfTypeAll<MonoBehaviour2PublicVeBoVeReDi2InTeCoObUnique>().First().field_Private_Boolean_0 = !Resources.FindObjectsOfTypeAll<MonoBehaviour2PublicVeBoVeReDi2InTeCoObUnique>().First().field_Private_Boolean_0;
            }
            else if (Input.GetKey(KeyCode.LeftAlt) && !CameraHandler._isZoomed && !Input.GetKey(KeyCode.Tab))
            {
                CameraHandler._isZoomed = true;
                Camera.main.fieldOfView = 10;
            }
            else if (Input.GetKeyUp(KeyCode.LeftAlt) && CameraHandler._isZoomed || !Application.isFocused)
            {
                CameraHandler._isZoomed = false;
                Camera.main.fieldOfView = 60;
            }
        }
    }
}