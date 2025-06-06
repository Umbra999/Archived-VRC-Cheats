using ExitGames.Client.Photon;
using Hexed.Wrappers;
using Photon.Realtime;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using System.Collections;
using static VRC.SDKBase.VRC_EventHandler;

namespace Hexed.Extensions
{
    internal class PhotonHelper
    {
        public static void OpRaiseEvent(byte code, object customObject, RaiseEventOptions RaiseEventOptions, byte Channel, DeliveryMode deliveryMode)
        {
            SendOptions options = new()
            {
                Channel = Channel,
                DeliveryMode = deliveryMode,
                Encrypt = true,
                Reliability = deliveryMode == DeliveryMode.Reliable || deliveryMode == DeliveryMode.ReliableUnsequenced,
            };

            OpRaiseEvent(code, CPP2IL.TypeSerializer.ManagedToIL(customObject), RaiseEventOptions, options);
        }

        public static void OpRaiseEvent(byte code, Il2CppSystem.Object customObject, RaiseEventOptions RaiseEventOptions, SendOptions sendOptions)
        {
            GameHelper.VRCNetworkingClient.Method_Public_Virtual_New_Boolean_Byte_Object_RaiseEventOptions_SendOptions_0(code, customObject, RaiseEventOptions, sendOptions);
        }

        public static void SendRPC(VrcEventType EventType, string Name, bool ParameterBool, VrcBooleanOp BoolOP, GameObject ParamObject, GameObject[] ParamObjects, string ParamString, float Float, int Int, byte[] bytes, VrcBroadcastType BroadcastType, float Fastforward = 0)
        {
            VrcEvent a = new()
            {
                EventType = EventType,
                Name = Name,
                ParameterBool = ParameterBool,
                ParameterBoolOp = BoolOP,
                ParameterBytes = bytes,
                ParameterObject = ParamObject,
                ParameterObjects = ParamObjects,
                ParameterString = ParamString,
                ParameterFloat = Float,
                ParameterInt = Int,
            };
            Networking.SceneEventHandler.TriggerEvent(a, BroadcastType, ParamObject, Fastforward);
        }

        public static void SendUdonRPC(UdonBehaviour behaviour, string EventName, VRC.Player Target = null, bool Local = false)
        {
            // TODO: Replace with manual send like this? to send _ events
            // SendRPC(VrcEventType.SendRPC, "", false, VrcBooleanOp.Unused, behaviour.gameObject, null, "UdonSyncRunProgramAsRPC", 0, 2, Encoding.UTF8.GetBytes(EventName), VrcBroadcastType.AlwaysUnbuffered, 0);

            if (behaviour != null)
            {
                if (Target != null)
                {
                    if (!Networking.IsOwner(Target.GetVRCPlayerApi(), behaviour.gameObject)) Networking.SetOwner(Target.GetVRCPlayerApi(), behaviour.gameObject);
                    behaviour.SendCustomNetworkEvent(NetworkEventTarget.Owner, EventName);
                }
                else if (!Local) behaviour.SendCustomNetworkEvent(NetworkEventTarget.All, EventName);
                else behaviour.SendCustomEvent(EventName);
            }
            else
            {
                foreach (UdonBehaviour Behaviour in UnityEngine.Object.FindObjectsOfType<UdonBehaviour>())
                {
                    if (Behaviour._eventTable.ContainsKey(EventName)) Behaviour.SendCustomNetworkEvent(NetworkEventTarget.All, EventName);
                }
            }
        }

        public static void RaiseChatMessage(string Message)
        {
            if (Message == null || Message.Length > 144) return;

            SendOptions options = new()
            {
                Channel = 0,
                DeliveryMode = DeliveryMode.Reliable,
                Encrypt = true,
                Reliability = true
            };

            RaiseEventOptions raiseEventOptions = new()
            {
                field_Public_ReceiverGroup_0 = ReceiverGroup.All,
            };

            OpRaiseEvent(43, Message, raiseEventOptions, options);
        }

        public static void RaiseTypingIndicator(byte Type)
        {
            RaiseEventOptions raiseEventOptions = new()
            {
                field_Public_ReceiverGroup_0 = ReceiverGroup.All,
            };

            OpRaiseEvent(44, Type, raiseEventOptions, 0, DeliveryMode.Reliable);
        }

        public static void RaiseVRMode(bool VR)
        {
            OpRaiseEvent(42, new Hashtable { { "inVRMode", VR }, }, new RaiseEventOptions(), 0, DeliveryMode.Reliable);
        }

        public static void RaiseAvatarHeight(int Height)
        {
            OpRaiseEvent(42, new Hashtable { { "avatarEyeHeight", Height }, }, new RaiseEventOptions(), 0, DeliveryMode.Reliable);
        }

        public static void RaiseGroupOnNameplate(string GroupID)
        {
            if (GroupID == null) return;

            OpRaiseEvent(42, new Hashtable { { "groupOnNameplate", GroupID }, }, new RaiseEventOptions(), 0, DeliveryMode.Reliable);
        }

        public static void RaiseBlock(string userid, bool Block)
        {
            if (!userid.StartsWith("usr_")) return;

            OpRaiseEvent(33, new Dictionary<byte, object>
            {
                { 3, Block },
                { 0, (byte)22 },
                { 1, userid },
            }, new RaiseEventOptions(), 0, DeliveryMode.Reliable);
        }

        public static void RaisePortalCreate(string InstanceID, Vector3 Position, float Rotation)
        {
            if (InstanceID == null) return;

            byte[] bytes = new byte[12];

            Buffer.BlockCopy(BitConverter.GetBytes(Position.x), 0, bytes, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(Position.y), 0, bytes, 4, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(Position.z), 0, bytes, 8, 4);

            // TODO: FIX THIS!!!

            //OpRaiseEvent(70, new Dictionary<byte, object>
            //{
            //    { 0, EnumPublicSealedvaPoShSt4vUnique.Portal },
            //    { 132, InstanceID },
            //    { 1, EnumPublicSealedvaCrUpDePoReCr7vUnique.Create },
               
            //}, new RaiseEventOptions(), 0, DeliveryMode.Reliable);
        }

        public static void RaiseEmojiCreate(int ID)
        {
            OpRaiseEvent(71, new Dictionary<byte, object>
            {
                { 0, (byte)EnumPublicSealedvaLeCu3vUnique.Legacy },
                { 2, ID },
            }, new RaiseEventOptions(), 0, DeliveryMode.Reliable);
        }

        public static void RaiseEmojiCreate(string ID)
        {
            if (ID == null) return;

            OpRaiseEvent(71, new Dictionary<byte, object>
            {
                { 0, (byte)EnumPublicSealedvaLeCu3vUnique.Custom },
                { 1, ID },
            }, new RaiseEventOptions(), 0, DeliveryMode.Reliable);
        }

        public static void RaiseSyncOwnership(int viewId, int Owner)
        {
            OpRaiseEvent(21, new int[2] { viewId, Owner }, new RaiseEventOptions() { field_Public_ReceiverGroup_0 = ReceiverGroup.All }, 0, DeliveryMode.Reliable);
        }

        public static void RaiseItemOwnership(int viewId, int Owner)
        {
            OpRaiseEvent(22, new int[2] { viewId, Owner }, new RaiseEventOptions() { field_Public_ReceiverGroup_0 = ReceiverGroup.All }, 0, DeliveryMode.Reliable);
        }

        public static void RaiseLegacyOwnership(int viewId, int Owner)
        {
            OpRaiseEvent(210, new int[2] { viewId, Owner }, new RaiseEventOptions() { field_Public_ReceiverGroup_0 = ReceiverGroup.All }, 0, DeliveryMode.Reliable);
        }
    }
}
