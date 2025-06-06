using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using VRC.SDKBase;
using WengaPort.Buttons;
using WengaPort.Wrappers;

namespace WengaPort.Modules
{
    internal class Movement : MonoBehaviour
    {
        public static void NoClipEnable()
        {
            if (Utils.CurrentUser == null)
            {
                Utils.CurrentUser = VRCPlayer.field_Internal_Static_VRCPlayer_0;
            }
            Utils.CurrentUser.gameObject.GetComponent<CharacterController>().enabled = false;
            FlyEnable();
            MainMenu.FlyButton.setToggleState(true, false);
            MainMenu.NoClipButton.setToggleState(true, false);
            NoClipToggle = true;
        }

        public static void NoClipDisable()
        {
            Utils.CurrentUser.gameObject.GetComponent<CharacterController>().enabled = true;
            FlyDisable();
            MainMenu.FlyButton.setToggleState(false, false);
            MainMenu.NoClipButton.setToggleState(false, false);
            NoClipToggle = false;
        }

        public static void IncreaseSpeed()
        {
            FindObjectOfType<LocomotionInputController>().field_Public_Single_2 = 4;
            FindObjectOfType<LocomotionInputController>().field_Public_Single_0 = 8;
            FindObjectOfType<LocomotionInputController>().field_Public_Single_1 = 4;
        }

        public static void DecreaseSpeed()
        {
            FindObjectOfType<LocomotionInputController>().field_Public_Single_2 = 2;
            FindObjectOfType<LocomotionInputController>().field_Public_Single_0 = 4;
            FindObjectOfType<LocomotionInputController>().field_Public_Single_1 = 2;
        }

        public static bool FlyToggle = false;
        public static bool InfJump = true;
        public static bool DoubleJump = false;
        public static bool Attachment = false;
        public static bool NoClipToggle = false;
        public static bool ShortcutActive = false;
        public static bool ShortcutInactive = true;

        public static void FlyEnable()
        {
            FlyToggle = true;
        }

        public static void FlyDisable()
        {
            FlyToggle = false;
            Physics.gravity = gravity;
            Utils.CurrentUser.gameObject.GetComponent<CharacterController>().enabled = true;
            MainMenu.NoClipButton.setToggleState(false, false);
            NoClipToggle = false;
            AttachmentManager.Reset();
            Attachment = false;
            InteractMenu.AttachmentToggle.setToggleState(false, false);
        }

        public static void UIInit()
        {
            isInVR = GeneralWrappers.IsInVr();
        }

        public void Update()
        {
            try
            {
                if (isInVR && !NoClipToggle)
                {
                    var Shortcut = GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu");
                    var ShortCutActive = Shortcut.gameObject.active;
                    if (ShortCutActive == false)
                    {
                        Utils.CurrentUser.gameObject.GetComponent<CharacterController>().enabled = true;
                    }
                    else if (ShortCutActive == true && !NoClipToggle)
                    {
                        Utils.CurrentUser.gameObject.GetComponent<CharacterController>().enabled = false;
                    }
                }
                if (Input.GetKeyDown(KeyCode.F) & Input.GetKey(KeyCode.LeftControl))
                {
                    if (FlyToggle)
                    {
                        MainMenu.NoClipButton.setToggleState(false, false);
                        MainMenu.FlyButton.setToggleState(false, false);
                        FlyDisable();
                    }
                    else
                    {
                        FlyEnable();
                        MainMenu.FlyButton.setToggleState(true, false);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.G) & Input.GetKey(KeyCode.LeftControl))
                {
                    if (NoClipToggle)
                    {
                        MainMenu.NoClipButton.setToggleState(false, false);
                        MainMenu.FlyButton.setToggleState(false, false);
                        FlyDisable();
                        NoClipDisable();
                    }
                    else
                    {
                        NoClipEnable();
                        FlyEnable();
                        MainMenu.FlyButton.setToggleState(true, false);
                        MainMenu.NoClipButton.setToggleState(true, false);
                    }
                }
                if (currentPlayer == null || transform == null)
                {
                    currentPlayer = Utils.CurrentUser;
                    transform = Camera.main.transform;
                    isInVR = UnityEngine.XR.XRDevice.isPresent;
                }
                if (FlyToggle)
                {
                    if (RoomManager.field_Internal_Static_ApiWorldInstance_0 == null)
                    {
                        return;
                    }
                    if (isInVR)
                    {
                        if (Math.Abs(Input.GetAxis("Vertical")) != 0f)
                        {
                            currentPlayer.transform.position += transform.transform.forward * FlySpeed * Time.deltaTime * Input.GetAxis("Vertical");
                        }
                        if (Math.Abs(Input.GetAxis("Horizontal")) != 0f)
                        {
                            currentPlayer.transform.position += transform.transform.right * FlySpeed * Time.deltaTime * Input.GetAxis("Horizontal");
                        }
                        if (Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") < 0f)
                        {
                            currentPlayer.transform.position += transform.transform.up * FlySpeed * Time.deltaTime * Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryThumbstickVertical");
                        }
                        if (Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") > 0f)
                        {
                            currentPlayer.transform.position += transform.transform.up * FlySpeed * Time.deltaTime * Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryThumbstickVertical");
                        }
                    }
                    else
                    {
                        if (Input.GetKeyDown((KeyCode)304))
                        {
                            FlySpeed *= 2f;
                        }
                        else if (Input.GetKeyUp((KeyCode)304))
                        {
                            FlySpeed /= 2f;
                        }
                        if (Input.GetKey((KeyCode)101))
                        {
                            currentPlayer.transform.position += transform.transform.up * FlySpeed * Time.deltaTime;
                        }
                        else if (Input.GetKey((KeyCode)113))
                        {
                            currentPlayer.transform.position += transform.transform.up * -1f * FlySpeed * Time.deltaTime;
                        }
                        if (Input.GetKey((KeyCode)119)) // W
                        {
                            currentPlayer.transform.position += transform.transform.forward * FlySpeed * Time.deltaTime;
                        }
                        else if (Input.GetKey((KeyCode)115)) // S
                        {
                            currentPlayer.transform.position += transform.transform.forward * -1f * FlySpeed * Time.deltaTime;
                        }
                        if (Input.GetKey((KeyCode)97)) // A
                        {
                            currentPlayer.transform.position += transform.transform.right * -1f * FlySpeed * Time.deltaTime;
                        }
                        else if (Input.GetKey((KeyCode)100)) // D
                        {
                            currentPlayer.transform.position += transform.transform.right * FlySpeed * Time.deltaTime;
                        }
                    }
                    Physics.gravity = Vector3.zero;
                }
                else if (InfJump)
                {
                    if (VRCInputManager.Method_Public_Static_VRCInput_String_0("Jump").prop_Single_0 == 1)
                    {
                        var Jump = Networking.LocalPlayer.GetVelocity();
                        Jump.y = Networking.LocalPlayer.GetJumpImpulse();
                        Networking.LocalPlayer.SetVelocity(Jump);
                    }
                }
                else if (DoubleJump)
                {
                    if (VRCInputManager.Method_Public_Static_VRCInput_String_0("Jump").prop_Boolean_0 && !Networking.LocalPlayer.IsPlayerGrounded())
                    {
                        var Jump = Networking.LocalPlayer.GetVelocity();
                        Jump.y = Networking.LocalPlayer.GetJumpImpulse();
                        Jump.y += 1f;
                        Networking.LocalPlayer.SetVelocity(Jump);
                        Jump.y -= 1f;
                    }
                }
                if (Rotate)
                {       
                    if (RoomManager.field_Internal_Static_ApiWorld_0 == null)
                    {
                        MovementMenu.RotateToggle.setToggleState(false, false);
                        ToggleRotate(false);
                    }
                    if (Input.GetKey(KeyCode.UpArrow))
                    {
                        currentPlayer.transform.Rotate(Vector3.right, RotateSpeed * Time.deltaTime);
                    }
                    else if (Input.GetKey(KeyCode.DownArrow))
                    {
                        currentPlayer.transform.Rotate(Vector3.left, RotateSpeed * Time.deltaTime);
                    }
                    else if (Input.GetKey(KeyCode.RightArrow))
                    {
                        currentPlayer.transform.Rotate(Vector3.back, RotateSpeed * Time.deltaTime);
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        currentPlayer.transform.Rotate(Vector3.forward, RotateSpeed * Time.deltaTime);
                    }
                    else if (Input.GetKey(KeyCode.X))
                    {
                        ToggleRotate(false);
                        ToggleRotate(true);
                    }
                    alignTrackingToPlayer?.Invoke();
                }
                if (Attachment)
                {
                    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space) || RoomManager.field_Internal_Static_ApiWorld_0 == null)
                    {
                        InteractMenu.AttachmentToggle.setToggleState(false, false);
                        Attachment = false;
                        AttachmentManager.Reset();
                    }
                }
            }
            catch { }
        }

        internal static AlignTrackingToPlayerDelegate GetAlignTrackingToPlayerDelegate
        {
            get
            {
                if (alignTrackingToPlayerMethod == null)
                {
                    alignTrackingToPlayerMethod = typeof(VRCPlayer).GetMethods(BindingFlags.Instance | BindingFlags.Public).
                        First((MethodInfo m) => m.ReturnType == typeof(void)
                        && m.GetParameters().Length == 0
                        && m.XRefScanForMethod("get_Transform", null)
                        && m.XRefScanForMethod(null, "Player")
                        && m.XRefScanForMethod("Vector3_Quaternion", "VRCPlayer")
                        && m.XRefScanForMethod(null, "VRCTrackingManager")
                        && m.XRefScanForMethod(null, "InputStateController"));
                }
                return (AlignTrackingToPlayerDelegate)Delegate.CreateDelegate(typeof(AlignTrackingToPlayerDelegate), Utils.CurrentUser, alignTrackingToPlayerMethod);
            }
        }

        private static AlignTrackingToPlayerDelegate alignTrackingToPlayer;
        private static MethodInfo alignTrackingToPlayerMethod;

        internal static void ToggleRotate(bool state)
        {
            if (state)
            {
                Rotate = true;
                alignTrackingToPlayer = GetAlignTrackingToPlayerDelegate;
            }
            else
            {
                Utils.CurrentUser.gameObject.GetComponent<CharacterController>().enabled = false;
                Rotate = false;
                Quaternion localRotation = Utils.CurrentUser.transform.localRotation;
                Utils.CurrentUser.transform.localRotation = new Quaternion(0f, localRotation.y, 0f, localRotation.w);
                alignTrackingToPlayer();
                Utils.CurrentUser.gameObject.GetComponent<CharacterController>().enabled = true;
            }
        }

        internal delegate void AlignTrackingToPlayerDelegate();

        public static Vector3 gravity = Physics.gravity;
        private static VRCPlayer currentPlayer;
        private static new Transform transform;
        public static bool isInVR;
        public static bool Rotate = false;
        public static float FlySpeed = 4.2f;
        public static float RotateSpeed = 180f;
        public Movement(IntPtr ptr) : base(ptr) { }
    }
}