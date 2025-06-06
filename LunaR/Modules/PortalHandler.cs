using Il2CppSystem;
using System.Collections.Generic;
using UnityEngine;
using VRC.SDKBase;

namespace LunaR.Modules
{
    internal class PortalHandler
    {
        public static bool NoPortalFollow = false;

        public static void DeletePortals()
        {
            PortalTrigger[] array = Resources.FindObjectsOfTypeAll<PortalTrigger>();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].gameObject.activeInHierarchy && array[i].gameObject.GetComponentInParent<VRC_PortalMarker>() == null) UnityEngine.Object.Destroy(array[i].gameObject);
            }
        }

        public static void DropCrashPortal(VRCPlayer Target)
        {
            int[] values = new[] { -69, -666, -999 };
            DropPortal("wrld_5b89c79e-c340-4510-be1b-476e9fcdedcc", "999~private(8JoV9XEdpo)~region(eu)", values[Utils.Random.Next(values.Length)], Target.transform.position + Target.transform.forward * 1.505f, Target.transform.rotation, false, false);
        }

        public static void DropInfinitePortal(VRCPlayer Target)
        {
            DropPortal("wrld_5b89c79e-c340-4510-be1b-476e9fcdedcc", "\nLunaR\n", int.MinValue, Target.transform.position + Target.transform.forward * 1.505f, Target.transform.rotation, true, false);
        }

        public static void DropMessagePortal(VRCPlayer Target, string Message)
        {
            string instance = $"\n{Message}\n";
            DropPortal("wrld_5b89c79e-c340-4510-be1b-476e9fcdedcc", instance, -666, Target.transform.position + Target.transform.forward * 1.505f, Target.transform.rotation, false, false);
        }

        public static GameObject DropPortalToWorld(VRCPlayer Target, string RoomId)
        {
            string[] WorldID = RoomId.Split(':');
            return DropPortal(WorldID[0], WorldID[1], 0, Target.transform.position + Target.transform.forward * 2f, Target.transform.rotation, false, false);
        }

        public static GameObject DropPortal(string WorldID, string InstanceID, int players, Vector3 Position, Quaternion Rotation, bool Infinit, bool Hide)
        {
            GameObject PortalObject = Networking.Instantiate(VRC_EventHandler.VrcBroadcastType.Always, "Portals/PortalInternalDynamic", Position, Rotation);
            if (PortalObject == null) return null;

            Il2CppSystem.Object[] Metadata = new Il2CppSystem.Object[]
            {
                WorldID,
                InstanceID,
                new Int32() { m_value = players }.BoxIl2CppObject()
            };

            Networking.RPC(RPC.Destination.AllBufferOne, PortalObject, "ConfigurePortal", Metadata);

            PortalObject.SetActive(!Hide);
            if (Infinit) Utils.DelayAction(1, delegate
            {
                UnityEngine.Object.Destroy(PortalObject.GetComponent<PortalInternal>());
            }).Start();
            return PortalObject;
        }

        public static List<string> PortalTargets = new();
        public static List<string> HiddenPlayers = new();

        public struct Position
        {
            public static readonly float InfValue = 2.14748365E+09f;
            public static Vector3 INFBypass => new(InfValue, InfValue, InfValue);
            public static Vector3 NegINFBypass => new(-InfValue, -InfValue, -InfValue);
            public static Quaternion ROTBypass => new(InfValue, InfValue, InfValue, InfValue);
            public static Quaternion NegROTBypass => new(-InfValue, -InfValue, -InfValue, -InfValue);
        }
    }
}