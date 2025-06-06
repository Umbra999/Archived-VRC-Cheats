using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC.SDKBase;

namespace LunaR.Modules
{
    internal class PenWriter
    {
        public static List<Vector3> LowerA(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1.15f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1.15f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerB(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z+ -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1.2f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerC(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space, player.y+ 0.8f, player.z+ -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.8f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerD(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1.2f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerE(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.8f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerF(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space + -0.1f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 1.2f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1.2f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 1.2f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.15f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.05f, player.y + 1f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerG(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.9f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerH(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1.2f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.8f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerI(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space + -0.1f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.8f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerJ(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.15f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.15f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.075f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.225f, player.y + 1f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerK(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 0.8f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerL(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x +Space, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.8f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerM(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.8f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerN(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerO(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerP(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1.1f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1.2f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 1.2f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1.2f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1.1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.8f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerQ(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space + -0.25f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.05f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.05f, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space + -0.05f, player.y + 0.85f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 0.85f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.85f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerR(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 1f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerS(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerT(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space + -0.1f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerU(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerV(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerW(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerX(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.8f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerY(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space + -0.1f, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f)
            };
        }

        public static List<Vector3> LowerZ(Vector3 player)
        {
            return new List<Vector3>
            {
                new Vector3(player.x + Space, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 1f, player.z + -2f),
                new Vector3(player.x + Space + -0.1f, player.y + 0.9f, player.z + -2f),
                new Vector3(player.x + Space, player.y + 0.8f, player.z + -2f),
                new Vector3(player.x + Space + -0.2f, player.y + 0.8f, player.z + -2f)
            };
        }

        public static IEnumerator DrawWord(VRC_Pickup Pen, Vector3 Position, string Text)
        {
            ItemHandler.TakeOwnershipIfNecessary(Pen.gameObject);
            Text = Text.ToLower();
            foreach (char ch in Text.ToCharArray())
            {
                if (ch == ' ') Space += -0.6f;
                else
                {
                    List<Vector3> positions = GetList(Position, ch);
                    Vector3 originalPosition = Pen.transform.position;
                    Quaternion originalRotation = Pen.transform.rotation;
                    yield return new WaitForSeconds(0.1f);
                    Pen.transform.rotation = new Quaternion(-0.7f, 0f, 0f, 0.7f);
                    Pen.transform.position = positions[0];
                    yield return new WaitForSeconds(0.1f);

                    VRC_Trigger BaseTrigger = Pen.gameObject.GetComponent<VRC_Trigger>();
                    if (BaseTrigger == null)
                    {
                        Networking.RPC(RPC.Destination.All, Pen.gameObject, "Dropped", new Il2CppSystem.Object[0]);
                        Networking.RPC(RPC.Destination.All, Pen.gameObject, "PickedUp", new Il2CppSystem.Object[0]);
                    }
                    else
                    {
                        BaseTrigger.OnDrop();
                        BaseTrigger.OnPickup();
                    }
                    yield return new WaitForSeconds(0.2f);
                    if (BaseTrigger == null) Networking.RPC(RPC.Destination.All, Pen.gameObject, "PenDown", new Il2CppSystem.Object[0]);
                    else BaseTrigger.OnPickupUseDown();
                    yield return new WaitForSeconds(0.1f);
                    foreach (Vector3 position in positions)
                    {
                        Pen.transform.position = position;
                        yield return new WaitForSeconds(0.1f);
                    }
                    if (BaseTrigger == null)
                    {
                        Networking.RPC(RPC.Destination.All, Pen.gameObject, "PenUp", new Il2CppSystem.Object[0]);
                        Networking.RPC(RPC.Destination.All, Pen.gameObject, "Dropped", new Il2CppSystem.Object[0]);
                    }
                    else
                    {
                        BaseTrigger.OnPickupUseUp();
                        BaseTrigger.OnDrop();
                    }
                    Pen.transform.position = originalPosition;
                    Pen.transform.rotation = originalRotation;
                    Space += -0.3f;
                }
            }
            Space = 0f;
            yield break;
        }

        public static List<Vector3> GetList(Vector3 player, char chr)
        {
            switch (chr)
            {
                case 'a':
                    return LowerA(player);

                case 'b':
                    return LowerB(player);

                case 'c':
                    return LowerC(player);

                case 'd':
                    return LowerD(player);

                case 'e':
                    return LowerE(player);

                case 'f':
                    return LowerF(player);

                case 'g':
                    return LowerG(player);

                case 'h':
                    return LowerH(player);

                case 'i':
                    return LowerI(player);

                case 'j':
                    return LowerJ(player);

                case 'k':
                    return LowerK(player);

                case 'l':
                    return LowerL(player);

                case 'm':
                    return LowerM(player);

                case 'n':
                    return LowerN(player);

                case 'o':
                    return LowerO(player);

                case 'p':
                    return LowerP(player);

                case 'q':
                    return LowerQ(player);

                case 'r':
                    return LowerR(player);

                case 's':
                    return LowerS(player);

                case 't':
                    return LowerT(player);

                case 'u':
                    return LowerU(player);

                case 'v':
                    return LowerV(player);

                case 'w':
                    return LowerW(player);

                case 'x':
                    return LowerX(player);

                case 'y':
                    return LowerY(player);

                case 'z':
                    return LowerZ(player);

                default:
                    break;
            }
            return null;
        }

        private static float Space = 0f;
    }
}