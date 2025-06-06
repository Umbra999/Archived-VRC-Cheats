using Hexed.Interfaces;
using Hexed.Wrappers;
using UnityEngine;

namespace Hexed.Modules
{
    internal class ThirdPerson : IGlobalModule, IDesktopOnly
    {
        public void Initialize()
        {
            GameObject referenceCamera = Camera.main.gameObject;

            BackCamera = GameObject.CreatePrimitive(PrimitiveType.Cube);
            BackCamera.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            BackCamera.name = "BackCamera";
            UnityEngine.Object.Destroy(BackCamera.GetComponent<BoxCollider>());
            UnityEngine.Object.Destroy(BackCamera.GetComponent<MeshRenderer>());
            UnityEngine.Object.Destroy(BackCamera.GetComponent<MeshFilter>());
            BackCamera.transform.localScale = referenceCamera.transform.localScale;
            Rigidbody BackRigidbody = BackCamera.AddComponent<Rigidbody>();
            BackRigidbody.isKinematic = true;
            BackRigidbody.useGravity = false;
            BackCamera.GetComponent<Renderer>().enabled = false;
            BackCamera.AddComponent<Camera>();
            BackCamera.transform.parent = referenceCamera.transform;
            BackCamera.transform.rotation = referenceCamera.transform.rotation;
            BackCamera.transform.position = referenceCamera.transform.position;
            BackCamera.transform.position -= BackCamera.transform.forward * 2f;
            BackCamera.GetComponent<Camera>().fieldOfView = VRCInputManager.Method_Public_Static_Single_EnumNPublicSealedvaCoHeToTaThShPeVoViUnique_0(VRCInputManager.EnumNPublicSealedvaCoHeToTaThShPeVoViUnique.DesktopFOV);

            FrontCamera = GameObject.CreatePrimitive(PrimitiveType.Cube);
            FrontCamera.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            FrontCamera.name = "FrontCamera";
            UnityEngine.Object.Destroy(FrontCamera.GetComponent<BoxCollider>());
            UnityEngine.Object.Destroy(FrontCamera.GetComponent<MeshRenderer>());
            UnityEngine.Object.Destroy(FrontCamera.GetComponent<MeshFilter>());
            FrontCamera.transform.localScale = referenceCamera.transform.localScale;
            Rigidbody FrontRigidbody = FrontCamera.AddComponent<Rigidbody>();
            FrontRigidbody.isKinematic = true;
            FrontRigidbody.useGravity = false;
            FrontCamera.GetComponent<Renderer>().enabled = false;
            FrontCamera.AddComponent<Camera>();
            BackCamera.GetComponent<BoxCollider>().enabled = false;
            FrontCamera.transform.parent = referenceCamera.transform;
            FrontCamera.transform.rotation = referenceCamera.transform.rotation;
            FrontCamera.transform.Rotate(0f, 180f, 0f);
            FrontCamera.transform.position = referenceCamera.transform.position;
            FrontCamera.transform.position += -FrontCamera.transform.forward * 2f;

            FrontCamera.GetComponent<Camera>().fieldOfView = VRCInputManager.Method_Public_Static_Single_EnumNPublicSealedvaCoHeToTaThShPeVoViUnique_0(VRCInputManager.EnumNPublicSealedvaCoHeToTaThShPeVoViUnique.DesktopFOV);

            FrontCamera.GetComponent<Camera>().enabled = false;
            BackCamera.GetComponent<Camera>().enabled = false;
        }

        public void OnUpdate()
        {
            if (!GameUtils.IsInWorld() || BackCamera == null || FrontCamera == null) return;

            switch (CameraSetup)
            {
                case 0:
                    BackCamera.GetComponent<Camera>().enabled = false;
                    FrontCamera.GetComponent<Camera>().enabled = false;
                    break;

                case 1:
                    BackCamera.GetComponent<Camera>().enabled = true;
                    FrontCamera.GetComponent<Camera>().enabled = false;
                    break;

                case 2:
                    BackCamera.GetComponent<Camera>().enabled = false;
                    FrontCamera.GetComponent<Camera>().enabled = true;
                    break;
            }

            if (CameraSetup != 0)
            {
                float axis = Input.GetAxis("Mouse ScrollWheel");
                if (axis > 0)
                {
                    BackCamera.transform.position += BackCamera.transform.forward * 0.1f;
                    FrontCamera.transform.position -= BackCamera.transform.forward * 0.1f;
                }
                else if (axis < 0)
                {
                    BackCamera.transform.position -= BackCamera.transform.forward * 0.1f;
                    FrontCamera.transform.position += BackCamera.transform.forward * 0.1f;
                }
            }
        }

        public static void SwitchCamSetup()
        {
            switch (CameraSetup)
            {
                case 0:
                    CameraSetup = 1;
                    break;

                case 1:
                    CameraSetup = 2;
                    break;

                case 2:
                    CameraSetup = 0;
                    break;
            }
        }

        private static GameObject BackCamera;
        private static GameObject FrontCamera;
        private static int CameraSetup = 0;
    }
}
