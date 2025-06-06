using LunaR.Wrappers;
using System.Collections;
using UnityEngine;
using VRC;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace LunaR.Modules
{
    internal class RPCHandler
    {
        public enum Emotes
        {
            Wave = 1,
            Clap = 2,
            Point = 3,
            Cheer = 4,
            Dance = 5,
            Backflip = 6,
            Die = 7,
            Sadness = 8
        }

        public static void EmojiRPC(int i)
        {
            Il2CppSystem.Int32 @int = default;
            @int.m_value = i;
            Il2CppSystem.Object @object = @int.BoxIl2CppObject();
            Networking.RPC(0, Utils.CurrentUser.gameObject, "SpawnEmojiRPC", new Il2CppSystem.Object[]
            {
                @object
            });
        }

        public static void SetTimerRPC(PortalInternal portalInternal, float Timer)
        {
            Il2CppSystem.Single single = default;
            single.m_value = 30f - Timer;
            Il2CppSystem.Object @object = single.BoxIl2CppObject();
            Networking.RPC((RPC.Destination)7, portalInternal.gameObject, "SetTimerRPC", new Il2CppSystem.Object[]
            {
                @object
            });
        }

        public static void EmoteRPC(int i)
        {
            Il2CppSystem.Int32 @int = default;
            @int.m_value = i;
            Il2CppSystem.Object @object = @int.BoxIl2CppObject();
            Networking.RPC(0, Utils.CurrentUser.gameObject, "PlayEmoteRPC", new Il2CppSystem.Object[]
            {
                    @object
            });
        }

        public static void ChangePedestals(string ID)
        {
            foreach (VRC_AvatarPedestal vrc_AvatarPedestal in Object.FindObjectsOfType<VRC_AvatarPedestal>())
            {
                Networking.RPC(0, vrc_AvatarPedestal.gameObject, "SwitchAvatar", new Il2CppSystem.Object[]
                {
                    ID
                });
            }
        }

        public static void SendUdonRPC(GameObject Object, string EventName, Player Target = null, bool Local = false)
        {
            if (Object != null)
            {
                UdonBehaviour Behaviour = Object.GetComponent<UdonBehaviour>();
                if (Target != null)
                {
                    Networking.SetOwner(Target.field_Private_VRCPlayerApi_0, Object);
                    Behaviour.SendCustomNetworkEvent(NetworkEventTarget.Owner, EventName);
                }
                else
                {
                    if (!Local) Behaviour.SendCustomNetworkEvent(NetworkEventTarget.All, EventName);
                    else Behaviour.SendCustomEvent(EventName);
                }
            }
            else
            {
                foreach (UdonBehaviour Behaviour in UnityEngine.Object.FindObjectsOfType<UdonBehaviour>())
                {
                    if (Behaviour._eventTable.ContainsKey(EventName)) Behaviour.SendCustomNetworkEvent(NetworkEventTarget.All, EventName);
                }
            }
        }

        public static bool EmojiSpam = false;

        public static IEnumerator EmojiSpammer()
        {
            EmojiSpam = true;
            while (EmojiSpam && GeneralWrappers.IsInWorld())  
            {
                EmojiRPC(55);
                yield return new WaitForEndOfFrame();
            }
            EmojiSpam = false;
        }
    }
}