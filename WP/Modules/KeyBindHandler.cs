using System;
using System.Linq;
using UnityEngine;
using VRC.UI;

namespace WengaPort.Modules
{
    class KeyBindHandler : MonoBehaviour
    {
        public KeyBindHandler(IntPtr ptr) : base(ptr) { }
        public void Update()
        {
            if (!UnityEngine.XR.XRDevice.isPresent && RoomManager.field_Internal_Static_ApiWorld_0 != null)
            {
                CameraHandler.Zoom();
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.K))
                {
                    if (!CameraHandler.DesktopCam)
                    {
                        CameraHandler.EnableDesktopCam();
                    }
                    else
                    {
                        CameraHandler.DisableDesktopCam();
                    }
                }

                else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    var r = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                    if (Physics.Raycast(r, out RaycastHit raycastHit))
                    {
                        VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = raycastHit.point;
                    }
                }

                else if (Input.GetKeyDown(KeyCode.Mouse2))
                {
                    PhotonModule.EmojiRPC(29);
                    PhotonModule.EmoteRPC(3);
                }

                else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha0))
                {
                    Resources.FindObjectsOfTypeAll<DebugLogGui>().First().field_Public_Boolean_0 = !Resources.FindObjectsOfTypeAll<DebugLogGui>().First().field_Public_Boolean_0;
                }

                else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha9))
                {
                    Resources.FindObjectsOfTypeAll<VRCNetworkInfoGUI>().First().field_Private_Boolean_0 = !Resources.FindObjectsOfTypeAll<VRCNetworkInfoGUI>().First().field_Private_Boolean_0;
                }

                else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha8))
                {
                    Resources.FindObjectsOfTypeAll<VRCNetworkGraphGUI>().First().field_Private_Boolean_0 = !Resources.FindObjectsOfTypeAll<VRCNetworkGraphGUI>().First().field_Private_Boolean_0;
                }

                else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.X))
                {
                    if (AntiMenuOverrender.AntiOverrenderToggle)
                    {
                        AntiMenuOverrender.AntiOverrender(false);
                    }
                    else
                    {
                        AntiMenuOverrender.AntiOverrender(true);
                    }
                }
            }
        }
    }
}
