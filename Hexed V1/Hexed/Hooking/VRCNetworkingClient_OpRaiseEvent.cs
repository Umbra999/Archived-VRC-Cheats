using ExitGames.Client.Photon;
using Hexed.Core;
using Hexed.HexedServer;
using Hexed.Modules.EventManagement;
using Hexed.Modules;
using Hexed.Wrappers;
using Photon.Realtime;
using VRC.Core;
using CoreRuntime.Manager;
using Hexed.Interfaces;
using Hexed.Modules.Standalone;
using static RootMotion.FinalIK.Grounding;

namespace Hexed.Hooking
{
    internal class VRCNetworkingClient_OpRaiseEvent : IHook
    {
        private delegate bool _OpRaiseEventDelegate(IntPtr instance, byte __0, IntPtr __1, IntPtr __2, SendOptions __3);
        private static _OpRaiseEventDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_OpRaiseEventDelegate>(typeof(VRCNetworkingClient).GetMethod(nameof(VRCNetworkingClient.Method_Public_Virtual_New_Boolean_Byte_Object_RaiseEventOptions_SendOptions_0)), Patch);
        }

        private static bool Patch(IntPtr instance, byte __0, IntPtr __1, IntPtr __2, SendOptions __3)
        {
            Il2CppSystem.Object eventData = __1 == nint.Zero ? null : new(__1);
            RaiseEventOptions raiseOptions = __2 == nint.Zero ? null : new(__2);

            if (InternalSettings.OpRaiseLog) Wrappers.Logger.LogOpRaise(__0, eventData, raiseOptions, __3);

            switch (__0)
            {
                case 1: // USpeak
                    {
                        byte[] Data = (byte[])CPP2IL.TypeSerializer.ILToManaged(eventData);
                        EventSanitizer.BlacklistedBytes.Add(Convert.ToBase64String(Data.Skip(8).ToArray()));
                        if (FakeSerialize.BotSerialize)
                        {
                            BotConnection.SelfbotVoice(Convert.ToBase64String(Data));
                            return false;
                        }
                    }
                    break;

                case 3: // RequestPastEvents
                    break;

                case 4: // SendPastEvents
                    break;

                case 5: // InitialSyncFinished 
                    break;

                case 6: // RPC
                    break;

                case 7: // Unknown, some Position related stuff
                    break;

                case 8: // Interests
                    {
                        switch (InternalSettings.CustomInterest)
                        {
                            case InternalSettings.InterestMode.Reversed:
                                {
                                    int[] InterestedActors = InterestManager.field_Private_Static_InterestManager_0.field_Private_Il2CppStructArray_1_Int32_0;

                                    byte[] customEvent = new byte[InterestedActors.Length * 12];
                                    int index = 0;

                                    foreach (int Actor in InterestedActors)
                                    {
                                        if (Actor == GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID()) continue;

                                        Array.Copy(BitConverter.GetBytes(int.Parse(Actor + "00001")), 0, customEvent, index, 4);
                                        index += 4;
                                        customEvent[index++] = 1;
                                        customEvent[index++] = 255;

                                        Array.Copy(BitConverter.GetBytes(int.Parse(Actor + "00003")), 0, customEvent, index, 4);
                                        index += 4;
                                        customEvent[index++] = 1;
                                        customEvent[index++] = 255;
                                    }

                                    eventData = CPP2IL.TypeSerializer.ManagedToIL(customEvent);
                                    __1 = eventData.Pointer;
                                }
                                break;

                            case InternalSettings.InterestMode.All:
                                {
                                    byte[] customEvent = new byte[(GameHelper.VRCNetworkingClient.GetActorCount() - 1) * 12];
                                    int index = 0;

                                    foreach (var Player in GameHelper.VRCNetworkingClient.GetAllPhotonPlayers())
                                    {
                                        if (Player.ActorID() == GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID()) continue;

                                        Array.Copy(BitConverter.GetBytes(int.Parse(Player.ActorID() + "00001")), 0, customEvent, index, 4);
                                        index += 4;
                                        customEvent[index++] = 1;
                                        customEvent[index++] = 255;

                                        Array.Copy(BitConverter.GetBytes(int.Parse(Player.ActorID() + "00003")), 0, customEvent, index, 4);
                                        index += 4;
                                        customEvent[index++] = 1;
                                        customEvent[index++] = 255;
                                    }

                                    eventData = CPP2IL.TypeSerializer.ManagedToIL(customEvent);
                                    __1 = eventData.Pointer;
                                }
                                break;
                        }
                    }
                    break;

                case 10: // Unknown, some udon stuff
                    break;

                case 11: // Udon Sync Objects
                    {
                        if (ExploitHandler.isMemoryViolation)
                        {
                            byte[] OriginalEvent = (byte[])CPP2IL.TypeSerializer.ILToManaged(eventData);

                            int typeIndex = GeneralUtils.lastIndexOfByteArray(OriginalEvent, new byte[] { 4, 0, 0, 0 });

                            if (typeIndex != -1 && OriginalEvent.Length > typeIndex + 7)
                            {
                                BitConverter.GetBytes(EncryptUtils.Random.Next(int.MaxValue - 2000, int.MaxValue)).CopyTo(OriginalEvent, typeIndex + 4);

                                eventData = CPP2IL.TypeSerializer.ManagedToIL(OriginalEvent);
                                __1 = eventData.Pointer;

                                Wrappers.Logger.LogDebug($"Raised Memory Violation");

                                break;
                            }
                        }
                    }
                    break;

                case 12: // Unreliable Player Serialization
                    {
                        byte[] OriginalEvent = (byte[])CPP2IL.TypeSerializer.ILToManaged(eventData);

                        int PlayerSyncOffset = OriginalEvent[19] + 19;
                        if (OriginalEvent.Length > PlayerSyncOffset)
                        {
                            if (InternalSettings.InfinityPosition)
                            {
                                int offsetInStruct = 1;
                                int lenghtInStruct = 12;
                                int offsetToOverride = PlayerSyncOffset + offsetInStruct;

                                if (OriginalEvent.Length >= offsetToOverride + lenghtInStruct)
                                {
                                    Buffer.BlockCopy(BitConverter.GetBytes(UnityUtils.InfPosition.InfValue), 0, OriginalEvent, offsetToOverride, 4);
                                    Buffer.BlockCopy(BitConverter.GetBytes(UnityUtils.InfPosition.InfValue), 0, OriginalEvent, offsetToOverride + 4, 4);
                                    Buffer.BlockCopy(BitConverter.GetBytes(UnityUtils.InfPosition.InfValue), 0, OriginalEvent, offsetToOverride + 8, 4);
                                }
                            }
                        }

                        int PlayerNetOffset = OriginalEvent[18] + 18;
                        if (OriginalEvent.Length > PlayerNetOffset)
                        {
                            switch (InternalSettings.PingMode)
                            {
                                case InternalSettings.FrameAndPingMode.Custom:
                                    {
                                        int offsetInStruct = 0;
                                        int lenghtInStruct = 2;
                                        int offsetToOverride = PlayerNetOffset + offsetInStruct;

                                        if (OriginalEvent.Length >= offsetToOverride + lenghtInStruct) Buffer.BlockCopy(BitConverter.GetBytes(InternalSettings.FakePingValue), 0, OriginalEvent, offsetToOverride, lenghtInStruct);
                                    }
                                    break;

                                case InternalSettings.FrameAndPingMode.Realistic:
                                    {
                                        int offsetInStruct = 0;
                                        int lenghtInStruct = 2;
                                        int offsetToOverride = PlayerNetOffset + offsetInStruct;

                                        if (OriginalEvent.Length >= offsetToOverride + lenghtInStruct) Buffer.BlockCopy(BitConverter.GetBytes(Convert.ToInt16(EncryptUtils.Random.Next(4, 16))), 0, OriginalEvent, offsetToOverride, lenghtInStruct);
                                    }
                                    break;
                            }

                            switch (InternalSettings.LatencySpoof)
                            {
                                case InternalSettings.LatencyMode.Custom:
                                    {
                                        int offsetInStruct = 2;
                                        int lenghtInStruct = 1;
                                        int offsetToOverride = PlayerNetOffset + offsetInStruct;

                                        if (OriginalEvent.Length >= offsetToOverride + lenghtInStruct) OriginalEvent[offsetToOverride] = InternalSettings.FakeLatencyValue;
                                    }
                                    break;

                                case InternalSettings.LatencyMode.Low:
                                    {
                                        int offsetInStruct = 2;
                                        int lenghtInStruct = 1;
                                        int offsetToOverride = PlayerNetOffset + offsetInStruct;

                                        if (OriginalEvent.Length >= offsetToOverride + lenghtInStruct) OriginalEvent[offsetToOverride] = byte.MinValue;
                                    }
                                    break;

                                case InternalSettings.LatencyMode.High:
                                    {
                                        int offsetInStruct = 2;
                                        int lenghtInStruct = 1;
                                        int offsetToOverride = PlayerNetOffset + offsetInStruct;

                                        if (OriginalEvent.Length >= offsetToOverride + lenghtInStruct) OriginalEvent[offsetToOverride] = byte.MaxValue;
                                    }
                                    break;
                            }

                            switch (InternalSettings.FrameMode)
                            {
                                case InternalSettings.FrameAndPingMode.Custom:
                                    {
                                        int offsetInStruct = 3;
                                        int lenghtInStruct = 1;
                                        int offsetToOverride = PlayerNetOffset + offsetInStruct;

                                        if (OriginalEvent.Length >= offsetToOverride + lenghtInStruct) OriginalEvent[offsetToOverride] = InternalSettings.FakeFrameValue == 0 ? (byte)0 : Convert.ToByte(Math.Min(255, Math.Max(0, 1000 / InternalSettings.FakeFrameValue)));
                                    }
                                    break;

                                case InternalSettings.FrameAndPingMode.Realistic:
                                    {
                                        int offsetInStruct = 3;
                                        int lenghtInStruct = 1;
                                        int offsetToOverride = PlayerNetOffset + offsetInStruct;

                                        if (OriginalEvent.Length >= offsetToOverride + lenghtInStruct) OriginalEvent[offsetToOverride] = Convert.ToByte(EncryptUtils.Random.Next(5, 9));
                                    }
                                    break;
                            }
                        }

                        int PoseRecorderOffset = OriginalEvent[17] + 17;
                        if (OriginalEvent.Length > PoseRecorderOffset)
                        {
                            switch (InternalSettings.MicSpoof)
                            {
                                case InternalSettings.MicStateMode.Muted:
                                    {
                                        int offsetInStruct = 7;
                                        int lenghtInStruct = 1;
                                        int offsetToOverride = PoseRecorderOffset + offsetInStruct;

                                        if (OriginalEvent.Length >= offsetToOverride + lenghtInStruct) OriginalEvent[offsetToOverride] |= 1 << 6;
                                    }
                                    break;

                                case InternalSettings.MicStateMode.Unmuted:
                                    {
                                        int offsetInStruct = 7;
                                        int lenghtInStruct = 1;
                                        int offsetToOverride = PoseRecorderOffset + offsetInStruct;

                                        if (OriginalEvent.Length >= offsetToOverride + lenghtInStruct) OriginalEvent[offsetToOverride] &= (~(1 << 6) & 0xFF);
                                    }
                                    break;
                            }

                            switch (InternalSettings.EarmuffSpoof)
                            {
                                case InternalSettings.EarmuffStateMode.Enabled:
                                    {
                                        int offsetInStruct = 7;
                                        int lenghtInStruct = 1;
                                        int offsetToOverride = PoseRecorderOffset + offsetInStruct;

                                        if (OriginalEvent.Length >= offsetToOverride + lenghtInStruct) OriginalEvent[offsetToOverride] |= 1 << 5;
                                    }
                                    break;

                                case InternalSettings.EarmuffStateMode.Disabled:
                                    {
                                        int offsetInStruct = 7;
                                        int lenghtInStruct = 1;
                                        int offsetToOverride = PoseRecorderOffset + offsetInStruct;

                                        if (OriginalEvent.Length >= offsetToOverride + lenghtInStruct) OriginalEvent[offsetToOverride] &= (~(1 << 5) & 0xFF);
                                    }
                                    break;
                            }
                        }

                        if (OriginalEvent.Length > PlayerSyncOffset && OriginalEvent.Length > PlayerNetOffset)
                        {
                            if (FakeSerialize.NoSerialize)
                            {
                                if (FakeSerialize.CachedMovement == null) FakeSerialize.CachedMovement = OriginalEvent;
                                else
                                {
                                    if (FakeSerialize.BotSerialize) BotConnection.SelfbotMovement(Convert.ToBase64String(OriginalEvent));

                                    int viewID = BitConverter.ToInt32(OriginalEvent, 0);
                                    int ServerTime = BitConverter.ToInt32(OriginalEvent, 4);

                                    int ogPlayerNetOffset = PlayerNetOffset;

                                    byte PingByte1 = OriginalEvent[ogPlayerNetOffset++];
                                    byte PingByte2 = OriginalEvent[ogPlayerNetOffset++];
                                    byte Latency = OriginalEvent[ogPlayerNetOffset++];
                                    byte Frame = OriginalEvent[ogPlayerNetOffset++];
                                    byte Quality = OriginalEvent[ogPlayerNetOffset++];

                                    OriginalEvent = FakeSerialize.CachedMovement;

                                    Buffer.BlockCopy(BitConverter.GetBytes(viewID), 0, OriginalEvent, 0, 4);
                                    Buffer.BlockCopy(BitConverter.GetBytes(ServerTime), 0, OriginalEvent, 4, 4);

                                    int Offset = OriginalEvent[18] + 18;
                                    OriginalEvent[Offset++] = PingByte1;
                                    OriginalEvent[Offset++] = PingByte2;
                                    OriginalEvent[Offset++] = Latency;
                                    OriginalEvent[Offset++] = Frame;
                                    OriginalEvent[Offset++] = Quality;
                                }
                            }

                            else if (InternalSettings.ObfuscateMovement)
                            {
                                int Adjustedindex = EncryptUtils.Random.Next(1, 9);

                                OriginalEvent[17] += (byte)Adjustedindex;
                                OriginalEvent[18] += (byte)Adjustedindex;
                                OriginalEvent[19] += (byte)Adjustedindex;

                                byte DoubleByte = OriginalEvent[20];
                                OriginalEvent[20] = (byte)EncryptUtils.Random.Next(byte.MinValue, byte.MaxValue);

                                List<byte> ByteList = OriginalEvent.ToList();

                                List<byte> randomBytes = new();
                                for (int i = 0; i < Adjustedindex - 1; i++)
                                {
                                    randomBytes.Add((byte)EncryptUtils.Random.Next(byte.MinValue, byte.MaxValue));
                                }
                                randomBytes.Add(DoubleByte);

                                ByteList.InsertRange(21, randomBytes);

                                OriginalEvent = ByteList.ToArray();
                            }
                        }

                        if (InternalSettings.MovementRedirect)
                        {
                            __0 = 7;
                        }

                        eventData = CPP2IL.TypeSerializer.ManagedToIL(OriginalEvent);
                        __1 = eventData.Pointer;
                        EventSanitizer.BlacklistedBytes.Add(Convert.ToBase64String(OriginalEvent.Skip(8).ToArray()));
                    }
                    break;

                case 13: // Reliable Serialization
                    {
                        if (FakeSerialize.BotSerialize)
                        {
                            byte[] Data = (byte[])CPP2IL.TypeSerializer.ILToManaged(eventData);
                            BotConnection.SelfbotAvatarSync(Convert.ToBase64String(Data));
                            return false;
                        }
                    }
                    break;

                case 16: // Unreliable Item Serialization
                    break;

                case 17: // Unknown, maybe unreliable object sync or something?
                    break;

                case 21: // Unknown, related to Udon panel
                    break;

                case 22: // Unknown, related to Udon panel
                    break;

                case 23: // Unknown, related to Udon panel
                    break;

                case 25: // Unknown, Probably udon world related sync data
                    break;

                case 33: // Moderations
                    if (!ModerationHandler.RaisedModerationEvent((Dictionary<byte, object>)CPP2IL.TypeSerializer.ILToManaged(eventData))) return false;
                    break;

                case 40: // UserRecordUpdate 
                    break;

                case 42: // Update Custom Props
                    if (!UserPropHandler.RaisedPropEvent(eventData.TryCast<Il2CppSystem.Collections.Hashtable>())) return false;
                    break;

                case 43: // Chatbox message
                    if (!ChatHandler.RaisedChatEvent(CPP2IL.TypeSerializer.ILToManaged(eventData).ToString())) return false;
                    break;

                case 44: // Typing Indicator
                    if (FakeSerialize.BotSerialize) return false;
                    break;

                case 53: // Player Product Sync
                    break;

                case 60: // Physbone interaction
                    break;

                case 66: // EAC Ping
                    break;

                case 70: // Old Portals, now unknown
                    break;

                case 71: // Emojis
                    if (!EmojiHandler.RaisedEmojiEvent((Dictionary<byte, object>)CPP2IL.TypeSerializer.ILToManaged(eventData))) return false;
                    break;

                case 73: // File Encryption
                    break;

                case 74: // Content share
                    if (!SharingHandler.RaisedContentShare((Dictionary<byte, object>)CPP2IL.TypeSerializer.ILToManaged(eventData))) return false;
                    break;

                case 202: // Instantiate
                    if (InternalSettings.InvisibleConnect)
                    {
                        raiseOptions.field_Public_ReceiverGroup_0 = (ReceiverGroup)byte.MaxValue;
                        bool res = originalMethod(instance, __0, __1, raiseOptions.Pointer, __3);
                        raiseOptions.field_Public_ReceiverGroup_0 = ReceiverGroup.Others;
                        return res;
                    }
                    break;

                case 210: // Transfer Ownership
                    break;

                default:
                    Wrappers.Logger.LogWarning($"Not implemented OpRaiseEvent");
                    Wrappers.Logger.LogOpRaise(__0, eventData, raiseOptions, __3);
                    break;
            }

            return originalMethod(instance, __0, __1, __2, __3);
        }
    }
}
