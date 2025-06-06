using LunaR.ConsoleUtils;
using LunaR.Objects;
using LunaR.Wrappers;
using System;
using System.Collections.Generic;
using VRC.Core;

namespace LunaR.Modules
{
    internal class WebsocketHandler
    {

        public static void ProcessMessage(WebSocketObject WebsocketObject, WebSocketContent WebSocketData)
        {
            User apiuser = WebSocketData?.user;
            NotificationDetails details = WebSocketData?.details;
            World apiworld = WebSocketData?.world;

            switch (WebsocketObject.type)
            {
                case "friend-online":
                    string Platform = apiuser.last_platform == "standalonewindows" ? "PC" : "Quest";
                    Extensions.Logger.Log($"{apiuser.displayName} is now Online [{Platform}]", Extensions.Logger.LogsType.API);
                    VRConsole.Log(VRConsole.LogsType.Online, $"{apiuser.displayName} [{Platform}]");

                    break;

                case "friend-offline":
                    APIUser.FetchUser(WebSocketData.userId, new Action<APIUser>((user) =>   
                    {
                        Extensions.Logger.Log($"{user.DisplayName()} is now Offline", Extensions.Logger.LogsType.API);
                        VRConsole.Log(VRConsole.LogsType.Offline, user.displayName);
                    }), null);

                    break;  

                case "friend-location":
                    if (WebSocketData.location == "private")
                    {
                        Extensions.Logger.Log($"{apiuser.displayName} is now in PRIVATE", Extensions.Logger.LogsType.API);
                        VRConsole.Log(VRConsole.LogsType.World, $"{apiuser.displayName} --> PRIVATE");
                    }
                    else if (WebSocketData.location.StartsWith("wrld_"))
                    {
                        InstanceAccessType Type = GeneralWrappers.GetWorldType(WebSocketData.world.id);
                        Extensions.Logger.Log($"{apiuser.displayName} is now in {WebSocketData.world.name} [{Type}]", Extensions.Logger.LogsType.API);
                        VRConsole.Log(VRConsole.LogsType.World, $"{apiuser.displayName} --> {apiworld.name} [{Type}]");
                    }
                    break;

                case "friend-delete":
                    APIUser.FetchUser(WebSocketData.userId, new Action<APIUser>((user) =>
                    {
                        Extensions.Logger.Log($"You and {user.DisplayName()} unfriended", Extensions.Logger.LogsType.API);
                        VRConsole.Log(VRConsole.LogsType.Friend, $"{user.DisplayName()} --> Unfriend");
                    }), null);
                    break;

                case "friend-add":
                    Extensions.Logger.Log($"You and {apiuser.displayName} friended", Extensions.Logger.LogsType.API);
                    VRConsole.Log(VRConsole.LogsType.Friend, $"{apiuser.displayName} --> Friend");
                    break;

                case "friend-update":
                    break;

                case "friend-active":
                    Extensions.Logger.Log($"{apiuser.displayName} is now Online [Website]", Extensions.Logger.LogsType.API);
                    VRConsole.Log(VRConsole.LogsType.Active, apiuser.displayName);
                    break;

                case "notification":
                    switch (WebSocketData.type)
                    {
                        case "invite":
                            Extensions.Logger.Log($"{WebSocketData.senderUsername} invited you to {details.worldName} [{details.worldId}]", Extensions.Logger.LogsType.API);
                            VRConsole.Log(VRConsole.LogsType.Notification, $"{WebSocketData.senderUsername} --> Invite");
                            break;

                        case "friendRequest":
                            Extensions.Logger.Log($"{WebSocketData.senderUsername} sendet a Friend Request", Extensions.Logger.LogsType.API);
                            VRConsole.Log(VRConsole.LogsType.Friend, $"{WebSocketData.senderUsername} --> Request");
                            break;

                        case "requestInvite":
                            Extensions.Logger.Log($"{WebSocketData.senderUsername} sendet a Invite Request", Extensions.Logger.LogsType.API);
                            VRConsole.Log(VRConsole.LogsType.Notification, $"{WebSocketData.senderUsername} --> Invite Request");
                            break;

                        case "inviteResponse":
                            Extensions.Logger.Log($"{WebSocketData.senderUsername} declined your Invite", Extensions.Logger.LogsType.API);
                            VRConsole.Log(VRConsole.LogsType.Notification, $"{WebSocketData.senderUsername} --> Declined Invite");
                            break;

                        case "requestInviteResponse":
                            Extensions.Logger.Log($"{WebSocketData.senderUsername} declined your Invite Request", Extensions.Logger.LogsType.API);
                            VRConsole.Log(VRConsole.LogsType.Notification, $"{WebSocketData.senderUsername} --> Declined Invite Request");
                            break;
                    }
                    break;
            }
        }

        public class WebSocketObject
        {
            public string type { get; set; }
            public string content { get; set; }
        }

        public class WebSocketContent
        {
            public string userId { get; set; }
            public string senderUserId { get; set; } // Notifications
            public string senderUsername { get; set; } // Notifications
            public string type { get; set; } // Notifications
            public NotificationDetails details { get; set; } // Notifications
            public User user { get; set; }
            public string location { get; set; }
            public World world { get; set; }
        }
    }
}