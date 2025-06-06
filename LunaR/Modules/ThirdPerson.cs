using LunaR.Wrappers;
using System;
using UnityEngine;

namespace LunaR.Modules
{
    internal class ThirdPerson : MonoBehaviour
    {
        public static bool Enabled = false;

        public void Start()
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(gameObject.GetComponent<MeshRenderer>());
            GameObject referenceCamera = GameObject.Find("Camera (eye)");
            if (referenceCamera != null)
            {
                gameObject.transform.localScale = referenceCamera.transform.localScale;
                Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
                rigidbody.isKinematic = true;
                rigidbody.useGravity = false;
                if (gameObject.GetComponent<Collider>()) gameObject.GetComponent<Collider>().enabled = false;
                gameObject.GetComponent<Renderer>().enabled = false;
                gameObject.AddComponent<Camera>();
                GameObject gameObject2 = referenceCamera;
                gameObject.transform.parent = gameObject2.transform;
                gameObject.transform.rotation = gameObject2.transform.rotation;
                gameObject.transform.position = gameObject2.transform.position;
                gameObject.transform.position -= gameObject.transform.forward * 2f;
                gameObject2.GetComponent<Camera>().enabled = false;
                gameObject.GetComponent<Camera>().fieldOfView = 75f;
                TPCameraBack = gameObject;
                GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Destroy(gameObject3.GetComponent<MeshRenderer>());
                gameObject3.transform.localScale = referenceCamera.transform.localScale;
                Rigidbody rigidbody2 = gameObject3.AddComponent<Rigidbody>();
                rigidbody2.isKinematic = true;
                rigidbody2.useGravity = false;
                if (gameObject3.GetComponent<Collider>()) gameObject3.GetComponent<Collider>().enabled = false;
                gameObject3.GetComponent<Renderer>().enabled = false;
                gameObject3.AddComponent<Camera>();
                gameObject3.transform.parent = gameObject2.transform;
                gameObject3.transform.rotation = gameObject2.transform.rotation;
                gameObject3.transform.Rotate(0f, 180f, 0f);
                gameObject3.transform.position = gameObject2.transform.position;
                gameObject3.transform.position += -gameObject3.transform.forward * 2f;
                gameObject2.GetComponent<Camera>().enabled = false;
                gameObject3.GetComponent<Camera>().fieldOfView = 75f;
                TPCameraFront = gameObject3;
                TPCameraBack.GetComponent<Camera>().enabled = false;
                TPCameraFront.GetComponent<Camera>().enabled = false;
                GameObject.Find("Camera (eye)").GetComponent<Camera>().enabled = true;
            }
        }

        public static void Trigger()
        {
            switch (CameraSetup)
            {
                case 0:
                    CameraSetup = 1;
                    Enabled = true;
                    break;

                case 1:
                    CameraSetup = 2;
                    break;

                case 2:
                    CameraSetup = 0;
                    GameObject.Find("Camera (eye)").GetComponent<Camera>().enabled = true;
                    TPCameraBack.GetComponent<Camera>().enabled = false;
                    TPCameraFront.GetComponent<Camera>().enabled = false;
                    Enabled = false;
                    break;
            }
        }

        public void Update()
        {
            if (GeneralWrappers.IsInWorld())
            {
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha5)) Trigger();
                if (TPCameraBack != null && TPCameraFront != null && Enabled)
                {
                    switch (CameraSetup)
                    {
                        case 0:
                            GameObject.Find("Camera (eye)").GetComponent<Camera>().enabled = true;
                            TPCameraBack.GetComponent<Camera>().enabled = false;
                            TPCameraFront.GetComponent<Camera>().enabled = false;
                            break;

                        case 1:
                            GameObject.Find("Camera (eye)").GetComponent<Camera>().enabled = false;
                            TPCameraBack.GetComponent<Camera>().enabled = true;
                            TPCameraFront.GetComponent<Camera>().enabled = false;
                            break;

                        case 2:
                            GameObject.Find("Camera (eye)").GetComponent<Camera>().enabled = false;
                            TPCameraBack.GetComponent<Camera>().enabled = false;
                            TPCameraFront.GetComponent<Camera>().enabled = true;
                            break;
                    }
                    if (CameraSetup != 0)
                    {
                        float axis = Input.GetAxis("Mouse ScrollWheel");
                        if (axis > 0f)
                        {
                            TPCameraBack.transform.position += TPCameraBack.transform.forward * 0.1f;
                            TPCameraFront.transform.position -= TPCameraBack.transform.forward * 0.1f;
                        }
                        else if (axis < 0f)
                        {
                            TPCameraBack.transform.position -= TPCameraBack.transform.forward * 0.1f;
                            TPCameraFront.transform.position += TPCameraBack.transform.forward * 0.1f;
                        }
                    }
                }
            }
        }

        private static GameObject TPCameraBack;
        private static GameObject TPCameraFront;
        private static int CameraSetup;

        public ThirdPerson(IntPtr ptr) : base(ptr)
        {
        }
    }
}