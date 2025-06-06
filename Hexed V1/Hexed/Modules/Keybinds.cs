using Hexed.Core;
using Hexed.Interfaces;
using Hexed.Modules.Standalone;
using Hexed.Wrappers;
using UnityEngine;

namespace Hexed.Modules
{
    internal class Keybinds : IGlobalModule, IDesktopOnly
    {
        public void Initialize()
        {

        }

        public void OnUpdate()
        {
            if (!GameUtils.IsInWorld()) return;

            if (Input.GetKeyInt(KeyCode.LeftControl) && Input.GetKeyDownInt(KeyCode.Mouse0))
            {
                if (Physics.Raycast(Camera.current.ScreenPointToRay(Input.mousePosition), out RaycastHit hit)) GameHelper.CurrentPlayer.transform.position = hit.point;
            }

            else if (Input.GetKeyDownInt(KeyCode.F) & Input.GetKeyInt(KeyCode.LeftControl))
            {
                Movement.ToggleFly();
            }

            else if (Input.GetKeyDownInt(KeyCode.G) & Input.GetKeyInt(KeyCode.LeftControl))
            {
                Movement.ToggleNoClip();
            }

            else if (Input.GetKeyInt(KeyCode.LeftAlt) && !InternalSettings.isCameraZoomed && !Input.GetKeyInt(KeyCode.Tab))
            {
                InternalSettings.isCameraZoomed = true;
                Camera.main.fieldOfView = 10;
            }

            else if (Input.GetKeyUpInt(KeyCode.LeftAlt) && InternalSettings.isCameraZoomed)
            {
                InternalSettings.isCameraZoomed = false;
                Camera.main.fieldOfView = VRCInputManager.Method_Public_Static_Single_EnumNPublicSealedvaCoHeToTaThShPeVoViUnique_0(VRCInputManager.EnumNPublicSealedvaCoHeToTaThShPeVoViUnique.DesktopFOV);
            }

            else if (Input.GetKeyInt(KeyCode.LeftControl) && Input.GetKeyDownInt(KeyCode.Alpha5))
            {
                ThirdPerson.SwitchCamSetup();
            }

            else if (Input.GetKeyDownInt(KeyCode.Mouse2))
            {
                FakeSerialize.CustomSerialize(!FakeSerialize.NoSerialize, true);
            }

            else if (Input.GetKeyInt(KeyCode.LeftControl) && Input.GetKeyDownInt(KeyCode.Alpha1))
            {
                PlayerUtils.ChangeAvatar("avtr_c38a1615-5bf5-42b4-84eb-a8b6c37cbd11");
            }

            else if (Input.GetKeyInt(KeyCode.LeftControl) && Input.GetKeyDownInt(KeyCode.Alpha0))
            {
                DebugLogGui gui = Resources.FindObjectsOfTypeAll<DebugLogGui>().FirstOrDefault();

                gui.field_Public_Boolean_0 = !gui.field_Public_Boolean_0;
            }

            else if (Input.GetKeyInt(KeyCode.LeftControl) && Input.GetKeyDownInt(KeyCode.Alpha9))
            {
                DebugTextGUI gui = Resources.FindObjectsOfTypeAll<DebugTextGUI>().FirstOrDefault();

                gui.field_Public_Boolean_0 = !gui.field_Public_Boolean_0;
            }

            else if (Input.GetKeyInt(KeyCode.LeftControl) && Input.GetKeyDownInt(KeyCode.Alpha8))
            {
                VRCNetworkInfoGUI gui = Resources.FindObjectsOfTypeAll<VRCNetworkInfoGUI>().FirstOrDefault();

                gui.field_Private_Boolean_0 = !gui.field_Private_Boolean_0;
            }

            else if (Input.GetKeyInt(KeyCode.LeftControl) && Input.GetKeyDownInt(KeyCode.Alpha7))
            {
                DebugNetworkObjectInfo gui = Resources.FindObjectsOfTypeAll<DebugNetworkObjectInfo>().FirstOrDefault();

                gui.field_Private_Boolean_0 = !gui.field_Private_Boolean_0;
                gui.Method_Private_Void_0();
            }

            else if (Input.GetKeyInt(KeyCode.LeftControl) && Input.GetKeyDownInt(KeyCode.Alpha6))
            {
                GameHelper.CurrentVRCPlayer.GetVRCPlayerApi().Respawn();
            }
        }
    }
}
