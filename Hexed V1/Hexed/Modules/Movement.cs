using Hexed.Core;
using Hexed.CustomUI.QM;
using Hexed.Extensions;
using Hexed.Interfaces;
using Hexed.Wrappers;
using UnityEngine;

namespace Hexed.Modules
{
    internal class Movement : IGlobalModule
    {
        public void Initialize()
        {

        }

        public void OnUpdate()
        {
            if (!GameUtils.IsInWorld() || GameHelper.CurrentPlayer?.GetVRCPlayerApi() == null)
            {
                if (isFlyEnabled) DisableFly();

                if (isRotateEnabled) DisableRotate();
            }
            else
            {
                if (isFlyEnabled)
                {
                    GameHelper.CurrentPlayer.GetVRCPlayerApi().SetVelocity(Vector3.zero);
                    float FlySpeed = flySpeed;

                    if (GameUtils.IsInVr())
                    {
                        GameHelper.CurrentPlayer.transform.position += playerTransform.transform.forward * FlySpeed * Time.deltaTime * InputHelper.GetVRCInput("Vertical").GetAxis();
                        GameHelper.CurrentPlayer.transform.position += playerTransform.transform.right * FlySpeed * Time.deltaTime * InputHelper.GetVRCInput("Horizontal").GetAxis();
                        GameHelper.CurrentPlayer.transform.position += playerTransform.transform.up * FlySpeed * Time.deltaTime * InputHelper.GetVRCInput("LookVertical").GetAxis();
                    }

                    else
                    {
                        if (Input.GetKeyInt(KeyCode.LeftShift)) FlySpeed *= 2f;

                        if (Input.GetKeyInt(KeyCode.E)) GameHelper.CurrentPlayer.transform.position += playerTransform.transform.up * FlySpeed * Time.deltaTime;
                        else if (Input.GetKeyInt(KeyCode.Q)) GameHelper.CurrentPlayer.transform.position += playerTransform.transform.up * -1f * FlySpeed * Time.deltaTime;
                        if (Input.GetKeyInt(KeyCode.W)) GameHelper.CurrentPlayer.transform.position += playerTransform.transform.forward * FlySpeed * Time.deltaTime;
                        else if (Input.GetKeyInt(KeyCode.S)) GameHelper.CurrentPlayer.transform.position += playerTransform.transform.forward * -1f * FlySpeed * Time.deltaTime;
                        if (Input.GetKeyInt(KeyCode.A)) GameHelper.CurrentPlayer.transform.position += playerTransform.transform.right * -1f * FlySpeed * Time.deltaTime;
                        else if (Input.GetKeyInt(KeyCode.D)) GameHelper.CurrentPlayer.transform.position += playerTransform.transform.right * FlySpeed * Time.deltaTime;
                    }
                }

                else if (InternalSettings.InfJump)
                {
                    if (InputHelper.GetVRCInput("Jump").GetHeldTime() > 0)
                    {
                        Vector3 Jump = GameHelper.CurrentPlayer.GetVRCPlayerApi().GetVelocity();
                        Jump.y = GameHelper.CurrentPlayer.GetVRCPlayerApi().GetJumpImpulse();
                        GameHelper.CurrentPlayer.GetVRCPlayerApi().SetVelocity(Jump);
                    }
                }

                else
                {
                    if (InternalSettings.MultiJump)
                    {
                        if (InputHelper.GetVRCInput("Jump").IsPressed() && !GameHelper.CurrentPlayer.GetVRCPlayerApi().IsPlayerGrounded())
                        {
                            Vector3 Jump = GameHelper.CurrentPlayer.GetVRCPlayerApi().GetVelocity();
                            Jump.y = GameHelper.CurrentPlayer.GetVRCPlayerApi().GetJumpImpulse();
                            Jump.y += 1f;
                            GameHelper.CurrentPlayer.GetVRCPlayerApi().SetVelocity(Jump);
                            Jump.y -= 1f;
                        }
                    }

                    if (InternalSettings.BunnyHop)
                    {
                        if (InputHelper.GetVRCInput("Jump").GetHeldTime() > 0 && GameHelper.CurrentPlayer.GetVRCPlayerApi().IsPlayerGrounded())
                        {
                            Vector3 jump = GameHelper.CurrentPlayer.GetVRCPlayerApi().GetVelocity();
                            jump.y = GameHelper.CurrentPlayer.GetVRCPlayerApi().GetJumpImpulse();
                            GameHelper.CurrentPlayer.GetVRCPlayerApi().SetVelocity(jump);
                        }
                    }

                    if (InternalSettings.HighJump) try { GameHelper.CurrentPlayer.GetVRCPlayerApi().SetJumpImpulse(JumpImpulse); } catch { }
                }

                if (InternalSettings.Speed) try { GameHelper.CurrentPlayer.GetVRCPlayerApi().SetRunSpeed(Speed); } catch { }

                if (isRotateEnabled)
                {
                    if (Input.GetKey(KeyCode.UpArrow))
                    {
                        GameHelper.CurrentPlayer.transform.Rotate(Vector3.right, RotateSpeed * Time.deltaTime);
                    }
                    else if (Input.GetKey(KeyCode.DownArrow))
                    {
                        GameHelper.CurrentPlayer.transform.Rotate(Vector3.left, RotateSpeed * Time.deltaTime);
                    }
                    else if (Input.GetKey(KeyCode.RightArrow))
                    {
                        GameHelper.CurrentPlayer.transform.Rotate(Vector3.back, RotateSpeed * Time.deltaTime);
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        GameHelper.CurrentPlayer.transform.Rotate(Vector3.forward, RotateSpeed * Time.deltaTime);
                    }
                    else if (Input.GetKey(KeyCode.X))
                    {
                        DisableRotate();
                        EnableRotate();
                    }

                    AlignTrackingToPlayer();
                }
            }
        }

        public static void ToggleFly()
        {
            if (isFlyEnabled) DisableFly();
            else EnableFly();
        }

        public static void ToggleNoClip()
        {
            if (IsNoClipEnabled) DisableFly();
            else EnableNoClip();
        }

        private static void EnableFly()
        {
            if (playerTransform == null) playerTransform = Camera.main.transform;
            Gravity = Physics.gravity;
            Physics.gravity = Vector3.zero;
            PhysicsMenu.FlyToggle?.SetToggleState(true);
            isFlyEnabled = true;
        }

        private static void DisableFly()
        {
            isFlyEnabled = false;
            IsNoClipEnabled = false;
            PhysicsMenu.FlyToggle?.SetToggleState(false);
            PhysicsMenu.NoClipToggle?.SetToggleState(false);
            Physics.gravity = Gravity;
            if (GameHelper.CurrentPlayer != null) GameHelper.CurrentPlayer.GetComponent<CharacterController>().enabled = true;
        }

        private static void EnableNoClip()
        {
            if (!isFlyEnabled) EnableFly();

            GameHelper.CurrentPlayer.GetComponent<CharacterController>().enabled = false;
            PhysicsMenu.NoClipToggle?.SetToggleState(true);
            IsNoClipEnabled = true;
        }

        public static void ToggleHighJump()
        {
            if (InternalSettings.HighJump) DisableHighJump();
            else EnableHighJump();
        }

        private static void EnableHighJump()
        {
            oldJumpImpulse = GameHelper.CurrentPlayer.GetVRCPlayerApi().GetJumpImpulse();
            InternalSettings.HighJump = true;
        }

        private static void DisableHighJump()
        {
            InternalSettings.HighJump = false;
            GameHelper.CurrentPlayer.GetVRCPlayerApi().SetJumpImpulse(oldJumpImpulse);
        }

        public static void ToggleSpeed()
        {
            if (InternalSettings.Speed) DisableSpeed();
            else EnableSpeed();
        }

        private static void EnableSpeed()
        {
            InternalSettings.Speed = true;
        }

        private static void DisableSpeed()
        {
            InternalSettings.Speed = false;
            GameHelper.CurrentPlayer.GetVRCPlayerApi().SetRunSpeed();
        }

        public static void ToggleRotate()
        {
            if (isRotateEnabled) DisableRotate();
            else EnableRotate();
        }

        private static void EnableRotate()
        {
            isRotateEnabled = true;
            PhysicsMenu.RotateToggle?.SetToggleState(true);
        }

        private static void DisableRotate()
        {
            isRotateEnabled = false;
            Quaternion localRotation = GameHelper.CurrentPlayer.transform.localRotation;
            GameHelper.CurrentVRCPlayer.transform.localRotation = new Quaternion(0f, localRotation.y, 0f, localRotation.w);
            AlignTrackingToPlayer();
            PhysicsMenu.RotateToggle?.SetToggleState(false);
        }

        private static void AlignTrackingToPlayer()
        {
            GameHelper.CurrentVRCPlayer.Method_Public_Void_0();
        }

        private static Vector3 Gravity;
        private static float oldJumpImpulse;
        private static Transform playerTransform;

        private static float flySpeed = 4.4f;
        private static float JumpImpulse = 10;
        private static float Speed = 10;
        private static float RotateSpeed = 180;

        private static bool isFlyEnabled = false;
        private static bool IsNoClipEnabled = false;
        public static bool isRotateEnabled = false;
    }
}
