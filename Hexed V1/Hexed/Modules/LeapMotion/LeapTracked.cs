using RootMotion.FinalIK;
using UnityEngine;
using VRC.FingerTracking;
using VRC.Networking.Pose;

namespace Hexed.Modules.LeapMotion
{
    internal class LeapTracked
    {
        private static void ReorientateLeapToUnity(ref Vector3 p_pos, ref Quaternion p_rot)
        {
            p_pos *= 0.001f;
            p_pos.z *= -1f;
            p_rot.x *= -1f;
            p_rot.y *= -1f;
        }

        public static void UpdateFingers(HandProcessor fingerController, bool LeftHand, bool RightHand)
        {
            if (fingerController.field_Private_VRC_AnimationController_0 == null) return;

            fingerController.field_Private_Boolean_0 = true;
            //PoseRecorder.field_Internal_Static_Int32_0 |= (int)0x200u;
            //fingerController.field_Private_VRC_AnimationController_0.field_Private_VRCGestureController_0.field_Private_EnumNPublicSealedvaKeMoCoGaViOcViDaWaUnique_0 = VRCInputManager.EnumNPublicSealedvaKeMoCoGaViOcViDaWaUnique.Index; // not needed?

            for (int j = 0; j < 5; j++)
            {
                if (LeftHand)
                {
                    float currentSpread = (((LeapMain.GestureData.m_leftFingersSpreads[j] * 2.0f) - 1.0f) / 5.0f * 0.5f) + 0.5f;
                    float currentBend = (((LeapMain.GestureData.m_leftFingersBends[j] * 2.0f) - 1.0f) / 5.0f * 0.5f) + 0.5f;

                    fingerController.field_Private_Il2CppStructArray_1_Single_0[j] = 1f - currentBend;
                    fingerController.field_Private_Il2CppStructArray_1_Single_1[j] = currentSpread;
                }

                if (RightHand)
                {
                    float currentSpread = (((LeapMain.GestureData.m_rightFingersSpreads[j] * 2.0f) - 1.0f) / 5.0f * 0.5f) + 0.5f;
                    float currentBend = (((LeapMain.GestureData.m_rightFingersBends[j] * 2.0f) - 1.0f) / 5.0f * 0.5f) + 0.5f;

                    fingerController.field_Private_Il2CppStructArray_1_Single_0[j + 5] = 1f - currentBend;
                    fingerController.field_Private_Il2CppStructArray_1_Single_1[j + 5] = currentSpread;
                }
            }
        }

        public static void LateUpdateIK(IKSolverVR p_solver, bool LeftHand, bool RightHand, Transform p_left, Transform p_right)
        {
            if (LeapMain.LeapFrame == null) return;

            GestureMatcher.GetGestures(LeapMain.LeapFrame, ref LeapMain.GestureData);

            for (int i = 0; i < GestureMatcher.GesturesData.ms_handsCount; i++)
            {
                if (LeapMain.GestureData.m_handsPresenses[i] && LeapMain.LeapHands[i] != null)
                {
                    Vector3 l_pos = LeapMain.GestureData.m_handsPositons[i];
                    Quaternion l_rot = LeapMain.GestureData.m_handsRotations[i];
                    ReorientateLeapToUnity(ref l_pos, ref l_rot);

                    LeapMain.LeapHands[i].transform.localPosition = l_pos;
                    LeapMain.LeapHands[i].transform.localRotation = l_rot;
                }
            }

            if (LeftHand && p_solver.leftArm != null)
            {
                p_solver.leftArm.positionWeight = 1f;
                p_solver.leftArm.rotationWeight = 1f;

                if (p_solver.leftArm.target != null)
                {
                    p_solver.leftArm.target.position = p_left.position;
                    p_solver.leftArm.target.rotation = p_left.rotation;
                }
            }

            if (RightHand && p_solver.rightArm != null)
            {
                p_solver.rightArm.positionWeight = 1f;
                p_solver.rightArm.rotationWeight = 1f;

                if (p_solver.rightArm.target != null)
                {
                    p_solver.rightArm.target.position = p_right.position;
                    p_solver.rightArm.target.rotation = p_right.rotation;
                }
            }
        }
    }
}
