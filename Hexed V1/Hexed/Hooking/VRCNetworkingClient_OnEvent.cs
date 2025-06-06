using Hexed.Core;
using Hexed.Modules.EventManagement;
using Hexed.Modules;
using Hexed.Wrappers;
using VRC.Core;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Il2CppSystem.Collections;
using CoreRuntime.Manager;
using System.Text.Json;
using Hexed.Interfaces;
using Hexed.Modules.Standalone;
using Hexed.Extensions;

namespace Hexed.Hooking
{
    internal class VRCNetworkingClient_OnEvent : IHook
    {
        private delegate void _OnEventDelegate(nint instance, nint __0);
        private static _OnEventDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_OnEventDelegate>(typeof(VRCNetworkingClient).GetMethod(nameof(VRCNetworkingClient.OnEvent)), Patch);
        }

        private static void Patch(nint instance, nint __0)
        {
            EventData eventData = __0 == nint.Zero ? null : new(__0);

            if (eventData == null) return;

            if (InternalSettings.OnEventLog) Wrappers.Logger.LogEventData(eventData);

            switch (eventData.Code)
            {
                case 1: // USpeak
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code)) return;

                        byte[] Data = (byte[])CPP2IL.TypeSerializer.ILToManaged(eventData.CustomData);
                        if (!EventSanitizer.SanitizeEvent1(eventData.Sender, Data, eventData.Code)) return;

                        if (eventData.Sender == InternalSettings.RepeatVoiceActor) ExploitHandler.RepeatVoiceEvents(Data);
                    }
                    break;

                case 2: // ExecutiveMessage
                    {
                        if (eventData.CustomData == null) return;

                        Wrappers.Logger.Log($"The Server disconnected you: {JsonSerializer.Serialize(CPP2IL.TypeSerializer.ILToManaged(eventData.CustomData))}", Wrappers.Logger.LogsType.Room);
                        VRConsole.Log($"Server --> Disconnect", VRConsole.LogsType.Moderation);
                    }
                    break;

                case 3: // RequestPastEvents 
                    {
                        Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer(eventData.Sender);
                        string Displayname = PhotonPlayer.DisplayName() ?? "NO NAME";

                        Wrappers.Logger.Log($"{Displayname} requested Cache", Wrappers.Logger.LogsType.Room);
                        VRConsole.Log($"{Displayname} --> Cache request", VRConsole.LogsType.Room);
                    }
                    break;

                case 4: // SendPastEvents
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code)) return;

                        byte[][] Data = (byte[][])CPP2IL.TypeSerializer.ILToManaged(eventData.CustomData);
                        if (!EventSanitizer.SanitizeEvent4(eventData.Sender, Data, eventData.Code)) return;

                        Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer(eventData.Sender);
                        string Displayname = PhotonPlayer.DisplayName() ?? "NO NAME";

                        Wrappers.Logger.Log($"{Displayname} sent Cache with {Data.Length} Events", Wrappers.Logger.LogsType.Room);
                        VRConsole.Log($"{Displayname} --> Cache [{Data.Length}]", VRConsole.LogsType.Room);
                    }
                    break;

                case 5: // InitialSyncFinished 
                    {
                        Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer(eventData.Sender);
                        string Displayname = PhotonPlayer.DisplayName() ?? "NO NAME";

                        Wrappers.Logger.Log($"{Displayname} confirmed join", Wrappers.Logger.LogsType.Room);
                        VRConsole.Log($"{Displayname} --> Join confirmation", VRConsole.LogsType.Room);
                    }
                    break;

                case 6: // RPC
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code)) return;

                        byte[] Data = (byte[])CPP2IL.TypeSerializer.ILToManaged(eventData.CustomData);
                        if (!EventSanitizer.SanitizeEvent6(eventData.Sender, Data, eventData.Code)) return;
                    }
                    break;

                case 7: // Unknown, some PlayerPosition related stuff
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code)) return;

                        byte[] Data = (byte[])CPP2IL.TypeSerializer.ILToManaged(eventData.CustomData);
                        if (!EventSanitizer.SanitizeEvent7(eventData.Sender, Data, eventData.Code)) return;
                    }
                    break;

                case 8: // Interests
                    break;

                case 10: // Unknown, some udon stuff
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code)) return;

                        byte[] Data = (byte[])CPP2IL.TypeSerializer.ILToManaged(eventData.CustomData);
                        if (!EventSanitizer.SanitizeEvent10(eventData.Sender, Data, eventData.Code)) return;
                    }
                    break;

                case 11: // Udon Sync Objects
                    {
                        if (InternalSettings.NoUdonSync || eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code)) return;

                        byte[] Data = (byte[])CPP2IL.TypeSerializer.ILToManaged(eventData.CustomData);
                        if (!EventSanitizer.SanitizeEvent11(eventData.Sender, Data, eventData.Code)) return;
                    }
                    break;

                case 12: // Unreliable Player Serialization
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code)) return;

                        byte[] Data = (byte[])CPP2IL.TypeSerializer.ILToManaged(eventData.CustomData);
                        if (!EventSanitizer.SanitizeEvent12(eventData.Sender, Data, eventData.Code)) return;

                        InternalSettings.ActorsWithLastActiveTime[eventData.Sender] = GeneralUtils.GetUnixTimeInMilliseconds();
                    }
                    break;

                case 13: // Reliable Serialization / SDK3 Avatar
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code)) return;

                        byte[] Data = (byte[])CPP2IL.TypeSerializer.ILToManaged(eventData.CustomData);
                        if (!EventSanitizer.SanitizeEvent13(eventData.Sender, Data, eventData.Code)) return;
                    }
                    break;

                case 16: // Unreliable Item Serialization
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code)) return;

                        byte[] Data = (byte[])CPP2IL.TypeSerializer.ILToManaged(eventData.CustomData);
                        if (!EventSanitizer.SanitizeEvent16(eventData.Sender, Data, eventData.Code)) return;
                    }
                    break;

                case 17: // Unknown, maybe unreliable object serialization or sync or something?
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code)) return;

                        byte[] Data = (byte[])CPP2IL.TypeSerializer.ILToManaged(eventData.CustomData);
                        if (!EventSanitizer.SanitizeEvent17(eventData.Sender, Data, eventData.Code)) return;
                    }
                    break;

                case 20: // Unknown related to Udon panel, customdata is null maybe a reset event?
                    break;

                case 21: // Udon Ownership (Unknown Object rn i guess its Sync)
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code)) return;

                        int[] OwnershipData = (int[])CPP2IL.TypeSerializer.ILToManaged(eventData.CustomData);

                        if (!EventSanitizer.SanitizeEvent21(eventData.Sender, OwnershipData, eventData.Code)) return;

                        switch (InternalSettings.AntiPickup) // idk if needed for this event tbh
                        {
                            case InternalSettings.AntiPickupMode.Self:
                                if (ItemHelper.IsViewIdPickupable(OwnershipData[0]) && OwnershipData[1] != GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID())
                                {
                                    PhotonHelper.RaiseSyncOwnership(OwnershipData[0], GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID());
                                    return;
                                }
                                break;

                            case InternalSettings.AntiPickupMode.Nobody:
                                if (ItemHelper.IsViewIdPickupable(OwnershipData[0]) && OwnershipData[1] != 0)
                                {
                                    PhotonHelper.RaiseSyncOwnership(OwnershipData[0], 0);
                                    return;
                                }
                                break;
                        }
                    }
                    break;

                case 22: // Udon Ownership (Items)
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code)) return;

                        int[] OwnershipData = (int[])CPP2IL.TypeSerializer.ILToManaged(eventData.CustomData);

                        if (!EventSanitizer.SanitizeEvent22(eventData.Sender, OwnershipData, eventData.Code)) return;

                        switch (InternalSettings.AntiPickup)
                        {
                            case InternalSettings.AntiPickupMode.Self:
                                if (ItemHelper.IsViewIdPickupable(OwnershipData[0]) && OwnershipData[1] != GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID())
                                {
                                    PhotonHelper.RaiseItemOwnership(OwnershipData[0], GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID());
                                    return;
                                }
                                break;

                            case InternalSettings.AntiPickupMode.Nobody:
                                if (ItemHelper.IsViewIdPickupable(OwnershipData[0]) && OwnershipData[1] != 0)
                                {
                                    PhotonHelper.RaiseItemOwnership(OwnershipData[0], 0);
                                    return;
                                }
                                break;
                        }
                    }
                    break;

                case 23: // Unknown, object[]
                    break;

                case 24: // Unknown, prolly some udon sync data
                    break;

                case 26: // Unknown, some boolean maybe related to serialize state aka crashes state?
                    break;

                case 28: // Unknown, prolly some udon sync data
                    break;

                case 33: // Moderations
                    {
                        Dictionary<byte, object> moderationData = (Dictionary<byte, object>)CPP2IL.TypeSerializer.ILToManaged(eventData.CustomData);

                        if (!ModerationHandler.ReceivedModerationEvent(ref moderationData)) return;

                        eventData.customData = CPP2IL.TypeSerializer.ManagedToIL(moderationData);
                        __0 = eventData.Pointer;
                    }
                    break;

                case 34: //Apply Ratelimits
                    {
                        Wrappers.Logger.Log($"Prevented Server from adjusting Ratelimits", Wrappers.Logger.LogsType.Protection);
                        VRConsole.Log($"Server --> Ratelimit Adjust", VRConsole.LogsType.Protection);

                        eventData.customData = null;
                        __0 = eventData.Pointer;
                    }
                    break;

                case 35: // Reset Ratelimits
                    {
                        EventSanitizer.BlacklistedBytes.Clear();
                    }
                    break;

                case 40: // Update own props from API
                    break;

                case 42: // CustomProp Update
                    {
                        if (eventData.CustomData == null || !UserPropHandler.ReceivedPropEvent(eventData.CustomData.TryCast<Hashtable>(), GameHelper.VRCNetworkingClient.GetPhotonPlayer(eventData.Sender))) return;
                    }
                    break;

                case 43: // Chatbox message
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code)) return;

                        Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer(eventData.Sender);

                        if (!ChatHandler.ReceivedChatEvent(eventData.CustomData.ToString(), PhotonPlayer, eventData.Code)) return;
                    }
                    break;

                case 44: // Typing Indicator
                    break;

                case 51: // Unknown, probably relate to products
                    break;

                case 53: // Player Product sync
                    break;

                case 60: // Physbone permissions
                    break;

                case 66: // EAC Ping
                    break;

                case 70: // Old Portals, now unknown
                    break;

                case 71: // Emojis
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code)) return;

                        Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer(eventData.Sender);

                        if (!EmojiHandler.ReceivedEmojiEvent((Dictionary<byte, object>)CPP2IL.TypeSerializer.ILToManaged(eventData.CustomData), PhotonPlayer, eventData.Code)) return;
                    }
                    break;

                case 72: // Unknown, Type is either 1 or 0 afaik, similar to event 44?
                    break;

                case 73: // File Encryption
                    break;

                case 74: // Content share
                    {
                        if (eventData.CustomData == null || !SharingHandler.RecievedContentShare((Dictionary<byte, object>)CPP2IL.TypeSerializer.ILToManaged(eventData.CustomData), eventData.Code)) return;
                    }
                    break;

                case 202: // Instantiation
                    {
                        if (eventData.CustomData == null || !EventSanitizer.SanitizeEvent202(eventData.Sender, eventData.CustomData.Cast<Hashtable>(), eventData.Code)) return;
                    }
                    break;

                case 210:
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code)) return;

                        int[] OwnershipData = (int[])CPP2IL.TypeSerializer.ILToManaged(eventData.CustomData);

                        if (!EventSanitizer.SanitizeEvent210(eventData.Sender, OwnershipData, eventData.Code)) return;

                        switch (InternalSettings.AntiPickup)
                        {
                            case InternalSettings.AntiPickupMode.Self:
                                if (ItemHelper.IsViewIdPickupable(OwnershipData[0]) && OwnershipData[1] != GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID())
                                {
                                    PhotonHelper.RaiseLegacyOwnership(OwnershipData[0], GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID());
                                    return;
                                }
                                break;

                            case InternalSettings.AntiPickupMode.Nobody:
                                if (ItemHelper.IsViewIdPickupable(OwnershipData[0]) && OwnershipData[1] != 0)
                                {
                                    PhotonHelper.RaiseLegacyOwnership(OwnershipData[0], 0);
                                    return;
                                }
                                break;
                        }
                    }
                    break;

                case 226: // AppStats 
                    break;

                case 223: // AuthEvent
                    break;

                case 253: // Set Properties
                    break;

                case 254: // Player Disconnect
                    {
                        Player player = GameHelper.VRCNetworkingClient.GetPhotonPlayer(eventData.Sender);

                        if (player != null)
                        {
                            string Displayname = player.DisplayName() ?? "NO NAME";
                            string UserID = player.UserID() ?? "NO USERID";

                            GameManager.DestroyPlayerModules(player.ActorID());

                            EventSanitizer.RemoveActorBlocks(eventData.Sender);
                            if (InternalSettings.ActorsWithLastActiveTime.ContainsKey(eventData.Sender)) InternalSettings.ActorsWithLastActiveTime.Remove(eventData.Sender);
                            if (PhotonUtils.networkedProperties.ContainsKey(eventData.Sender)) PhotonUtils.networkedProperties.Remove(eventData.Sender); // is that even still used, needed whatever??

                            Wrappers.Logger.Log($"[ - ] {Displayname} [{UserID}]", Wrappers.Logger.LogsType.Room);
                            VRConsole.Log($"{Displayname}", VRConsole.LogsType.Disconnect);
                        }

                        if (eventData.Parameters.ContainsKey(203))
                        {
                            int newMasterActor = eventData[203].Unbox<int>();

                            Player master = GameHelper.VRCNetworkingClient.GetPhotonPlayer(newMasterActor);
                            if (master == null)
                            {
                                Wrappers.Logger.Log($"Prevented Server from setting invalid Master", Wrappers.Logger.LogsType.Protection);
                                VRConsole.Log($"Server --> Invalid Master Client", VRConsole.LogsType.Protection);
                                return;
                            }
                            else
                            {
                                string Displayname = master.DisplayName() ?? "NO NAME";

                                Wrappers.Logger.Log($"{Displayname} is now Master", Wrappers.Logger.LogsType.Room);
                                VRConsole.Log($"{Displayname} --> Master Client", VRConsole.LogsType.Room);
                            }
                        }
                    }
                    break;

                case 255: // Player Connect
                    {
                        string Displayname = "NO NAME";
                        string UserID = "NO USERID";

                        Hashtable props = eventData[249].TryCast<Hashtable>();
                        if (props != null && props.ContainsKey("user"))
                        {
                            var Table = props["user"].TryCast<Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>>();
                            if (Table != null)
                            {
                                if (Table.ContainsKey("displayName")) Displayname = Table["displayName"]?.ToString();
                                if (Table.ContainsKey("id")) UserID = Table["id"]?.ToString();
                            }
                        }

                        Wrappers.Logger.Log($"[ + ] {Displayname} [{UserID}]", Wrappers.Logger.LogsType.Room);
                        VRConsole.Log($"{Displayname}", VRConsole.LogsType.Connect);
                    }
                    break;

                default:
                    Wrappers.Logger.LogWarning($"Not implemented OnEvent, blocked for Protection");
                    Wrappers.Logger.LogEventData(eventData);
                    return;
            }

            originalMethod(instance, __0);
        }
    }
}
