using LunaR.QMButtons;
using LunaR.Wrappers;
using System;
using System.Collections;
using UnityEngine;
using VRC.SDKBase;

namespace LunaR.Modules
{
    internal class Movement : MonoBehaviour
    {
        private new static Transform transform;

        public static void NoClipEnable()
        {
            FlyEnable();
            Utils.CurrentUser.gameObject.GetComponent<CharacterController>().enabled = false;
            MainMenu.NoClipToggle.setToggleState(true, false);
            NoClipToggle = true;
        }

        public static void NoClipDisable()
        {
            FlyDisable();
            Utils.CurrentUser.gameObject.GetComponent<CharacterController>().enabled = true;
            NoClipToggle = false;
        }

        public static bool SpeedToggle = false;
        public static float WalkSpeed = 4;
        private static float OldWalkSpeed;
        public static void ToggleSpeed(bool State)
        {
            SpeedToggle = State;
            if (State)
            {
                OldWalkSpeed = Utils.CurrentUser.GetVRCPlayerApi().GetWalkSpeed();
                Utils.CurrentUser.GetVRCPlayerApi().SetWalkSpeed(WalkSpeed);
            }
            else Utils.CurrentUser.GetVRCPlayerApi().SetWalkSpeed(OldWalkSpeed);
        }

        public static bool JumpToggle = false;
        public static float JumpBoost = 4;
        private static float OldJumpBoost;
        public static void ToggleJump(bool State)
        {
            JumpToggle = State;
            if (State)
            {
                OldJumpBoost = Utils.CurrentUser.GetVRCPlayerApi().GetJumpImpulse();
                Utils.CurrentUser.GetVRCPlayerApi().SetJumpImpulse(JumpBoost);
            }
            else Utils.CurrentUser.GetVRCPlayerApi().SetJumpImpulse(OldJumpBoost);
        }

        public static bool FlyToggle = false;
        public static bool InfJump = true;
        public static bool DoubleJump = false;
        public static bool BunnyHop = false;
        public static bool NoClipToggle = false;
        public static bool SpinBot = false;
        private static Vector3 Gravity;

        public static void FlyEnable()
        {
            if (!FlyToggle) Gravity = Physics.gravity;
            if (transform == null) transform = Camera.main.transform;
            FlyToggle = true;
            MainMenu.FlyToggle.setToggleState(true, false);
        }

        public static void FlyDisable()
        {
            FlyToggle = false;
            Physics.gravity = Gravity;
            Utils.CurrentUser.gameObject.GetComponent<CharacterController>().enabled = true;
            MainMenu.FlyToggle.setToggleState(false, false);
            MainMenu.NoClipToggle.setToggleState(false, false);
            NoClipToggle = false;
            Attachment = false;
            TargetMenu.AttachToggle.SetToggleState(false, false);
        }

        public bool IsSpeedFly = false;

        public void Update()
        {
            if (GeneralWrappers.IsInWorld())
            {
                try
                {
                    if (!GeneralWrappers.IsInVr())
                    {
                        if (Input.GetKeyDown(KeyCode.F) & Input.GetKey(KeyCode.LeftControl))
                        {
                            if (FlyToggle) FlyDisable();
                            else FlyEnable();
                        }
                        else if (Input.GetKeyDown(KeyCode.G) & Input.GetKey(KeyCode.LeftControl))
                        {
                            if (NoClipToggle)
                            {
                                FlyDisable();
                                NoClipDisable();
                            }
                            else
                            {
                                NoClipEnable();
                                FlyEnable();
                            }
                        }
                    }

                    if (FlyToggle)
                    {
                        Networking.LocalPlayer.SetVelocity(Vector3.zero);
                        Physics.gravity = Vector3.zero;

                        if (GeneralWrappers.IsInVr())
                        {
                            if (Math.Abs(Input.GetAxis("Vertical")) != 0f) Utils.CurrentUser.transform.position += transform.transform.forward * FlySpeed * Time.deltaTime * Input.GetAxis("Vertical");
                            if (Math.Abs(Input.GetAxis("Horizontal")) != 0f) Utils.CurrentUser.transform.position += transform.transform.right * FlySpeed * Time.deltaTime * Input.GetAxis("Horizontal");
                            if (Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") < 0f) Utils.CurrentUser.transform.position += transform.transform.up * FlySpeed * Time.deltaTime * Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryThumbstickVertical");
                            if (Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") > 0f) Utils.CurrentUser.transform.position += transform.transform.up * FlySpeed * Time.deltaTime * Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryThumbstickVertical");
                        }

                        else
                        {
                            if (Input.GetKeyDown(KeyCode.LeftShift))
                            {
                                if (!IsSpeedFly)
                                {
                                    FlySpeed *= 2f;
                                    IsSpeedFly = true;
                                }
                            }
                            else if (Input.GetKeyUp(KeyCode.LeftShift))
                            {
                                if (IsSpeedFly)
                                {
                                    FlySpeed /= 2f;
                                    IsSpeedFly = false;
                                }
                            }
                            if (Input.GetKey(KeyCode.E)) Utils.CurrentUser.transform.position += transform.transform.up * FlySpeed * Time.deltaTime;
                            else if (Input.GetKey(KeyCode.Q)) Utils.CurrentUser.transform.position += transform.transform.up * -1f * FlySpeed * Time.deltaTime;
                            if (Input.GetKey(KeyCode.W)) Utils.CurrentUser.transform.position += transform.transform.forward * FlySpeed * Time.deltaTime;
                            else if (Input.GetKey(KeyCode.S)) Utils.CurrentUser.transform.position += transform.transform.forward * -1f * FlySpeed * Time.deltaTime;
                            if (Input.GetKey(KeyCode.A)) Utils.CurrentUser.transform.position += transform.transform.right * -1f * FlySpeed * Time.deltaTime;
                            else if (Input.GetKey(KeyCode.D)) Utils.CurrentUser.transform.position += transform.transform.right * FlySpeed * Time.deltaTime;
                        }
                    }
                    else if (InfJump)
                    {
                        if (VRCInputManager.Method_Public_Static_VRCInput_String_0("Jump").prop_Single_0 == 1)
                        {
                            Vector3 Jump = Networking.LocalPlayer.GetVelocity();
                            Jump.y = Networking.LocalPlayer.GetJumpImpulse();
                            Networking.LocalPlayer.SetVelocity(Jump);
                        }
                    }
                    else if (DoubleJump)
                    {
                        if (VRCInputManager.Method_Public_Static_VRCInput_String_0("Jump").prop_Boolean_0 && !Networking.LocalPlayer.IsPlayerGrounded())
                        {
                            Vector3 Jump = Networking.LocalPlayer.GetVelocity();
                            Jump.y = Networking.LocalPlayer.GetJumpImpulse();
                            Jump.y += 1f;
                            Networking.LocalPlayer.SetVelocity(Jump);
                            Jump.y -= 1f;
                        }
                        if (BunnyHop)
                        {
                            if (Jumping() && Networking.LocalPlayer.IsPlayerGrounded())
                            {
                                Vector3 jump = Networking.LocalPlayer.GetVelocity();
                                jump.y = Networking.LocalPlayer.GetJumpImpulse();
                                Networking.LocalPlayer.SetVelocity(jump);
                            }
                        }
                    }
                    else if (BunnyHop)
                    {
                        if (Jumping() && Networking.LocalPlayer.IsPlayerGrounded())
                        {
                            var jump = Networking.LocalPlayer.GetVelocity();
                            jump.y = Networking.LocalPlayer.GetJumpImpulse();
                            Networking.LocalPlayer.SetVelocity(jump);
                        }
                    }
                    if (SpinBot) Utils.CurrentUser.transform.Rotate(0f, 5, 0f);
                    if (Rotate)
                    {
                        if (Input.GetKey(KeyCode.UpArrow)) Utils.CurrentUser.transform.Rotate(Vector3.right, RotateSpeed * Time.deltaTime);
                        else if (Input.GetKey(KeyCode.DownArrow)) Utils.CurrentUser.transform.Rotate(Vector3.left, RotateSpeed * Time.deltaTime);
                        else if (Input.GetKey(KeyCode.RightArrow)) Utils.CurrentUser.transform.Rotate(Vector3.back, RotateSpeed * Time.deltaTime);
                        else if (Input.GetKey(KeyCode.LeftArrow)) Utils.CurrentUser.transform.Rotate(Vector3.forward, RotateSpeed * Time.deltaTime);
                        else if (Input.GetKey(KeyCode.X))
                        {
                            ToggleRotate(false);
                            ToggleRotate(true);
                        }
                        Utils.CurrentUser.Method_Public_Void_1();
                    }
                }
                catch { }        
            }

            else if (Rotate)
            {
                MovementMenu.RotationToggle.SetToggleState(false, false);
                ToggleRotate(false);
            }
        }

        private static bool Jumping()
        {
            return VRCInputManager.Method_Public_Static_VRCInput_String_0("Jump").field_Public_Single_0 != 0;
        }

        public static void ToggleRotate(bool state)
        {
            if (state)
            {
                Rotate = true;
                Utils.CurrentUser.Method_Public_Void_1();
            }
            else
            {
                Utils.CurrentUser.gameObject.GetComponent<CharacterController>().enabled = false;
                Rotate = false;
                Quaternion localRotation = Utils.CurrentUser.transform.localRotation;
                Utils.CurrentUser.transform.localRotation = new Quaternion(0f, localRotation.y, 0f, localRotation.w);
                Utils.CurrentUser.Method_Public_Void_1();
                Utils.CurrentUser.gameObject.GetComponent<CharacterController>().enabled = true;
            }
        }

        public static bool Rotate = false;
        public static float FlySpeed = 4.2f;
        public static float RotateSpeed = 180f;
        public static bool Attachment = false;

        public static IEnumerator AttachToPlayer(VRCPlayer Player, HumanBodyBones bone)
        {
            Attachment = true;
            Utils.CurrentUser.transform.position = Player.transform.position;
            Transform Transform = Player.field_Internal_Animator_0.GetBoneTransform(bone);
            while (Attachment && Transform != null && GeneralWrappers.IsInWorld() && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.Space))
            {
                Utils.CurrentUser.transform.position = Transform.position;
                yield return new WaitForEndOfFrame();
            }
            Attachment = false;
            TargetMenu.AttachToggle.SetToggleState(false, false);
        }

        public Movement(IntPtr ptr) : base(ptr)
        {
        }
    }
}