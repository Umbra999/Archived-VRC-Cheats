using System;
using VRC.Core;
using UnityEngine;

namespace DiscordRichPresence
{
    internal class DiscordManager : MonoBehaviour
    {
        private static DiscordRpc.RichPresence presence;
        private static DiscordRpc.EventHandlers eventHandlers;
        private static bool running = false;
        private static string roomId = "";
        public static void Init()
        {
            eventHandlers = default;
            eventHandlers.errorCallback = delegate (int code, string message)
            {
                WengaPort.Extensions.Logger.WengaLogger(string.Concat(new object[]
                {
                    "[WengaPort Presence] (E",
                    code,
                    ") ",
                    message
                }));
            };
            presence.state = "Loading Screen";
            presence.details = "Not logged in (" + (UnityEngine.XR.XRDevice.isPresent ? "VR" : "PC") + ")";
            presence.largeImageKey = "logo";
            presence.smallImageKey = "small";
            presence.smallImageText = "Get Logout :<";
            presence.partySize = 0;
            presence.partyMax = 0;
            presence.startTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            presence.partyId = "";
            presence.largeImageText = "WengaPort";

            try
            {
                string optionalSteamId = null;
                ApiServerEnvironment serverEnvironment = VRCApplicationSetup.prop_VRCApplicationSetup_0.field_Public_ApiServerEnvironment_0;
                DiscordRpc.Initialize("770056934236880897", ref eventHandlers, true, optionalSteamId);
                DiscordRpc.UpdatePresence(ref presence);
                running = true;
                WengaPort.Extensions.Logger.WengaLogger("Discord Presence Init");
            }
            catch (Exception arg)
            {
                WengaPort.Extensions.Logger.WengaLogger("[Discord] Unable to init discord RichPresence:");
                WengaPort.Extensions.Logger.WengaLogger("[Discord] " + arg);
            }
        }

        public static string RoomChanged(string worldName, string worldAndRoomId, string roomIdWithTags, ApiWorldInstance.AccessType accessType, int maxPlayers)
        {
            if (!running)
            {
                return null;
            }
            if (!worldAndRoomId.Equals(""))
            {
                presence.state = "[World Hidden]";
                presence.partyId = "";
                presence.startTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            }
            else
            {
                presence.state = "[Loading Screen]";
                presence.partyId = "";
                presence.partyMax = 0;
                presence.startTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                presence.joinSecret = "";
            }
            DiscordRpc.UpdatePresence(ref presence);
            return presence.joinSecret;
        }


        public static void UserChanged(string displayName)
        {
            if (!running)
            {
                return;
            }
            if (!displayName.Equals(""))
            {
            presence.details = "Logged in (" + (UnityEngine.XR.XRDevice.isPresent ? "VR" : "PC") + ")";
            DiscordRpc.UpdatePresence(ref presence);
            return;
            }
            presence.details = "Not logged in (" + (UnityEngine.XR.XRDevice.isPresent ? "VR" : "PC") + ")";
            RoomChanged("", "", "", 0, 0);
        }

        public void Update()
        {
            Delay += Time.deltaTime;
            if (!running || Delay < 4f)
            {
                return;
            }
            Delay = 0f;
            APIUser currentUser2 = APIUser.CurrentUser;
            UserChanged(((currentUser2 != null) ? currentUser2.displayName : null) ?? "");
            string text = "";
            ApiWorld currentRoom = RoomManager.field_Internal_Static_ApiWorld_0;
            if (((currentRoom != null) ? currentRoom.currentInstanceIdOnly : null) != null)
            {
                text = currentRoom.id + ":" + currentRoom.currentInstanceIdWithTags;
            }
            if (roomId != text)
            {
                roomId = text;
                if (roomId != "")
                {
                    RoomChanged(currentRoom.name, currentRoom.id + ":" + currentRoom.currentInstanceIdOnly, currentRoom.currentInstanceIdWithTags, currentRoom.currentInstanceAccess, currentRoom.capacity);
                }
                else
                {
                    RoomChanged("", "", "", ApiWorldInstance.AccessType.InviteOnly, 0);
                }
            }
        }
        public float Delay = 0f;

        public static void OnApplicationQuit()
        {
            if (!running)
            {
                return;
            }
            DiscordRpc.Shutdown();
        }
        public DiscordManager(IntPtr ptr) : base(ptr) { }
    }
}