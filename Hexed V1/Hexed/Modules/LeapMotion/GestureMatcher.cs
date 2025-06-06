using System.Text.Json;
using UnityEngine;

namespace Hexed.Modules.LeapMotion
{
    internal class GestureMatcher
    {
        readonly static Vector2[] ms_fingerLimits =
        {
            new Vector2(0f, 15f),
            new Vector2(-20f, 20f),
            new Vector2(-50f, 50f),
            new Vector2(-7.5f, 7.5f),
            new Vector2(-20f, 20f)
        };

        public class GesturesData
        {
            readonly public static int ms_handsCount = 2;
            readonly public static int ms_fingersCount = 5;

            public bool[] m_handsPresenses = null;
            public Vector3[] m_handsPositons = null;
            public Quaternion[] m_handsRotations = null;
            public float[] m_leftFingersBends = null;
            public float[] m_leftFingersSpreads = null;
            public float[] m_rightFingersBends = null;
            public float[] m_rightFingersSpreads = null;

            public GesturesData()
            {
                m_handsPresenses = new bool[ms_handsCount];
                m_handsPositons = new Vector3[ms_handsCount];
                m_handsRotations = new Quaternion[ms_handsCount];
                m_leftFingersBends = new float[ms_fingersCount];
                m_leftFingersSpreads = new float[ms_fingersCount];
                m_rightFingersBends = new float[ms_fingersCount];
                m_rightFingersSpreads = new float[ms_fingersCount];
            }
        }

        public static void GetGestures(LeapObjects.HandObject[] p_frame, ref GesturesData p_data)
        {
            for (int i = 0; i < GesturesData.ms_handsCount; i++)
            {
                p_data.m_handsPresenses[i] = false;
            }

            for (int i = 0; i < GesturesData.ms_fingersCount; i++)
            {
                p_data.m_leftFingersBends[i] = 0f;
                p_data.m_leftFingersSpreads[i] = 0f;
                p_data.m_rightFingersBends[i] = 0f;
                p_data.m_leftFingersSpreads[i] = 0f;
            }

            foreach (LeapObjects.HandObject l_hand in p_frame)
            {
                int l_sideID = l_hand.IsLeft ? 0 : 1;

                if (!p_data.m_handsPresenses[l_sideID])
                {
                    p_data.m_handsPresenses[l_sideID] = true;
                    FillHandPosition(l_hand, ref p_data.m_handsPositons[l_sideID]);
                    FillHandRotation(l_hand, ref p_data.m_handsRotations[l_sideID]);
                    switch (l_sideID)
                    {
                        case 0:
                            {
                                FillFingerBends(l_hand, ref p_data.m_leftFingersBends);
                                FilFingerSpreads(l_hand, ref p_data.m_leftFingersSpreads);
                            }
                            break;
                        case 1:
                            {
                                FillFingerBends(l_hand, ref p_data.m_rightFingersBends);
                                FilFingerSpreads(l_hand, ref p_data.m_rightFingersSpreads);
                            }
                            break;
                    }
                }
            }
        }

        static void FillHandPosition(LeapObjects.HandObject p_hand, ref Vector3 p_pos)
        {
            p_pos.x = p_hand.Position.X;
            p_pos.y = p_hand.Position.Y;
            p_pos.z = p_hand.Position.Z;
        }

        static void FillHandRotation(LeapObjects.HandObject p_hand, ref Quaternion p_rot)
        {
            p_rot.x = p_hand.Rotation.X;
            p_rot.y = p_hand.Rotation.Y;
            p_rot.z = p_hand.Rotation.Z;
            p_rot.w = p_hand.Rotation.W;
        }

        static void FillFingerBends(LeapObjects.HandObject p_hand, ref float[] p_bends)
        {
            foreach (LeapObjects.FingerObject l_finger in p_hand.Fingers)
            {
                Quaternion l_prevSegment = Quaternion.identity;

                float l_angle = 0f;
                foreach (LeapObjects.BoneObject l_bone in l_finger.bones)
                {
                    if (l_bone.Type == LeapObjects.BoneType.TYPE_METACARPAL)
                    {
                        l_prevSegment = new Quaternion(l_bone.Rotation.X, l_bone.Rotation.Y, l_bone.Rotation.Z, l_bone.Rotation.W);
                        continue;
                    }

                    Quaternion l_curSegment = new Quaternion(l_bone.Rotation.X, l_bone.Rotation.Y, l_bone.Rotation.Z, l_bone.Rotation.W);
                    Quaternion l_diff = Quaternion.Inverse(l_prevSegment) * l_curSegment;
                    l_prevSegment = l_curSegment;

                    // Bend - local X rotation
                    float l_curAngle = 360f - l_diff.eulerAngles.x;
                    if (l_curAngle > 180f) l_curAngle -= 360f;
                    l_angle += l_curAngle;
                }

                p_bends[(int)l_finger.Type] = Mathf.InverseLerp(0f, (l_finger.Type == LeapObjects.FingerType.TYPE_THUMB) ? 90f : 180f, l_angle);
            }
        }

        static void FilFingerSpreads(LeapObjects.HandObject p_hand, ref float[] p_spreads)
        {

            foreach (LeapObjects.FingerObject l_finger in p_hand.Fingers)
            {
                LeapObjects.BoneObject l_parent = l_finger.bones[(int)LeapObjects.BoneType.TYPE_METACARPAL];
                LeapObjects.BoneObject l_child = l_finger.bones[(int)LeapObjects.BoneType.TYPE_PROXIMAL];

                Quaternion l_parentRot = new(l_parent.Rotation.X, l_parent.Rotation.Y, l_parent.Rotation.Z, l_parent.Rotation.W);
                Quaternion l_childRot = new(l_child.Rotation.X, l_child.Rotation.Y, l_child.Rotation.Z, l_child.Rotation.W);

                Quaternion l_diff = Quaternion.Inverse(l_parentRot) * l_childRot;

                float l_angle = l_diff.eulerAngles.y;
                if (l_angle > 180f) l_angle -= 360f;

                switch (l_finger.Type)
                {
                    case LeapObjects.FingerType.TYPE_THUMB:
                        {
                            if (p_hand.IsRight) l_angle *= -1f;
                            l_angle += ms_fingerLimits[(int)LeapObjects.FingerType.TYPE_INDEX].y * 2f;
                            l_angle *= 0.5f;
                        }
                        break;

                    case LeapObjects.FingerType.TYPE_INDEX:
                        {
                            if (p_hand.IsLeft) l_angle *= -1f;
                            l_angle += ms_fingerLimits[(int)LeapObjects.FingerType.TYPE_INDEX].y;
                            l_angle *= 0.5f;
                        }
                        break;

                    case LeapObjects.FingerType.TYPE_MIDDLE:
                        {
                            l_angle += (ms_fingerLimits[(int)LeapObjects.FingerType.TYPE_MIDDLE].y * (p_hand.IsRight ? 0.125f : -0.125f));
                            l_angle *= (p_hand.IsLeft ? -4f : 4f);
                        }
                        break;

                    case LeapObjects.FingerType.TYPE_RING:
                        {
                            if (p_hand.IsRight) l_angle *= -1f;
                            l_angle += ms_fingerLimits[(int)LeapObjects.FingerType.TYPE_RING].y;
                            l_angle *= 0.5f;
                        }
                        break;

                    case LeapObjects.FingerType.TYPE_PINKY:
                        {
                            l_angle += p_hand.IsRight ? ms_fingerLimits[(int)LeapObjects.FingerType.TYPE_PINKY].x : ms_fingerLimits[(int)LeapObjects.FingerType.TYPE_PINKY].y;
                            l_angle *= p_hand.IsRight ? -0.5f : 0.5f;
                        }
                        break;

                }

                p_spreads[(int)l_finger.Type] = Mathf.InverseLerp(ms_fingerLimits[(int)l_finger.Type].x, ms_fingerLimits[(int)l_finger.Type].y, l_angle);
                if (l_finger.Type != LeapObjects.FingerType.TYPE_THUMB)
                {
                    p_spreads[(int)l_finger.Type] *= 2f;
                    p_spreads[(int)l_finger.Type] -= 1f;
                }
            }
        }
    }
}
