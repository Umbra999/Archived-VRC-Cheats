using Hexed.Modules.EventManagement;
using UnityEngine;
using UnityEngine.XR;
using VRC.Core;
using VRC.SDKBase;

namespace Hexed.Wrappers
{
    internal static class GameUtils
    {
        public static bool IsInVr()
        {
            var xrDisplaySubsystems = new Il2CppSystem.Collections.Generic.List<XRDisplaySubsystem>();
            SubsystemManager.GetInstances(xrDisplaySubsystems);
            foreach (XRDisplaySubsystem xrDisplay in xrDisplaySubsystems)
            {
                if (xrDisplay.running) return true;
            }
            return false;
        }

        public static bool IsInWorld()
        {
            return GameHelper.VRCNetworkingClient.prop_Room_0 != null && GameHelper.CurrentPlayer != null;
        }

        public static string GetCurrentWorldID()
        {
            return GameHelper.VRCNetworkingClient.prop_Room_0?.field_Protected_String_0;
        }

        public static bool Forcejoin(string id)
        {
            string SanitizedID = id.Trim('\r', '\n', ' ');
            if (SanitizedID.Contains("worldId=") && SanitizedID.Contains("&instanceId="))
            {
                int worldIdIndex = SanitizedID.IndexOf("worldId=");
                int instanceIdIndex = SanitizedID.IndexOf("&instanceId=");
                string worldId = SanitizedID.Substring(worldIdIndex + "worldId=".Length, instanceIdIndex - (worldIdIndex + "worldId=".Length));
                string instanceId = SanitizedID.Substring(instanceIdIndex + "&instanceId=".Length);
                return Forcejoin($"{worldId}:{instanceId}");
            }

            if (EventSanitizer.CheckWorldID(SanitizedID)) return Networking.GoToRoom(SanitizedID);
            return false;
        }

        public static InstanceAccessType GetWorldType(string WorldID)
        {
            if (WorldID.Contains("~canRequestInvite")) return InstanceAccessType.InvitePlus;
            if (WorldID.Contains("~private")) return InstanceAccessType.InviteOnly;
            if (WorldID.Contains("~friends")) return InstanceAccessType.FriendsOnly;
            if (WorldID.Contains("~hidden")) return InstanceAccessType.FriendsOfGuests;
            if (WorldID.Contains("~group")) return InstanceAccessType.Group;
            return InstanceAccessType.Public;
        }

        public static string GetLocalizedWorldType(InstanceAccessType Type)
        {
            switch (Type)
            {
                case InstanceAccessType.Public:
                    return "Public";

                case InstanceAccessType.FriendsOfGuests:
                    return "Friends+";

                case InstanceAccessType.FriendsOnly:
                    return "Friends";

                case InstanceAccessType.InviteOnly:
                    return "Invite";

                case InstanceAccessType.InvitePlus:
                    return "Invite+";

                case InstanceAccessType.Group:
                    return "Group";
            }

            return null;
        }

        public static GameObject[] GetWorldPrefabs()
        {
            return VRC_SceneDescriptor.Instance.DynamicPrefabs.ToArray();
        }

        public static Material[] GetWorldMaterials()
        {
            return VRC_SceneDescriptor.Instance.DynamicMaterials.ToArray();
        }
    }
}
