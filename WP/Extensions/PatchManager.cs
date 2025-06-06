using ExitGames.Client.Photon;
using Harmony;
using Il2CppSystem.Collections;
using MelonLoader;
using Newtonsoft.Json.Linq;
using Photon.Realtime;
using RootMotion.FinalIK;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Transmtn;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.Core;
using VRC.Networking;
using VRC.SDKBase;
using VRC.UI;
using VRCSDK2.Validation.Performance;
using WengaPort.Buttons;
using WengaPort.ConsoleUtils;
using WengaPort.FoldersManager;
using WengaPort.Modules;
using WengaPort.Wrappers;
using static VRC.SDKBase.VRC_EventHandler;

namespace WengaPort.Extensions
{
    internal class PatchManager
    {
        public static HarmonyInstance Instance = HarmonyInstance.Create("WengaPortPatches");

        private static HarmonyMethod GetPatch(string name)
        {
            return new HarmonyMethod(typeof(PatchManager).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic));
        }

        public unsafe static void InitPatch()
        {
            try
            {
                Instance.Patch(typeof(NetworkManager).GetMethod("Method_Public_Void_Player_1"), GetPatch("OnPlayerLeft"), null);
                Instance.Patch(typeof(NetworkManager).GetMethod("Method_Public_Void_Player_0"), GetPatch("OnPlayerJoin"), null);
                Instance.Patch(AccessTools.Method(typeof(VRC_EventHandler), "InternalTriggerEvent", null, null), GetPatch("TriggerEvent"), null, null);
                Instance.Patch(AccessTools.Property(typeof(PhotonPeer), "RoundTripTime").GetMethod, null, GetPatch("FakePing"), null);
                Instance.Patch(AccessTools.Property(typeof(PhotonPeer), "RoundTripTimeVariance").GetMethod, null, GetPatch("FakePing"), null);
                Instance.Patch(typeof(PhotonPeer).GetMethod("SendOperation"), GetPatch("OperationPatch"), null);
                Instance.Patch(typeof(LoadBalancingClient).GetMethod("OnEvent"), GetPatch("OnEvent"), null);
                Instance.Patch(typeof(LoadBalancingClient).GetMethod(nameof(LoadBalancingClient.Method_Public_Boolean_EnterRoomParams_0)), null, GetPatch("OpJoinRoomPatch"));
                Instance.Patch(AccessTools.Method(typeof(API), "SendPutRequest", null, null), GetPatch("RequestPatch"), null, null);
                Instance.Patch(AccessTools.Method(typeof(MenuController), "Method_Public_Void_APIUser_0"), postfix: new HarmonyMethod(typeof(PatchManager).GetMethod("OnUserInfoOpen", BindingFlags.Static | BindingFlags.Public)));
                Instance.Patch(typeof(PortalTrigger).GetMethod(nameof(PortalTrigger.OnTriggerEnter), BindingFlags.Public | BindingFlags.Instance), GetPatch("EnterPortalPrefix"), null, null);
                Instance.Patch(typeof(LoadBalancingPeer).GetMethod("Method_Public_Virtual_New_Boolean_Byte_Object_RaiseEventOptions_SendOptions_0"), GetPatch("OpRaiseEventPrefix"), null, null);
                Instance.Patch(AccessTools.Method(typeof(VRC_EventDispatcherRFC), "Method_Public_Void_Player_VrcEvent_VrcBroadcastType_Int32_Single_0", null, null), GetPatch("CaughtEventPatch"), null, null);
                Instance.Patch(AccessTools.Property(typeof(Time), "smoothDeltaTime").GetMethod, null, GetPatch("FakeFrames"), null);
                Instance.Patch(typeof(MonoBehaviour2PrivateUdOb1BoObObObObObUnique).GetMethod(nameof(MonoBehaviour2PrivateUdOb1BoObObObObObUnique.UdonSyncRunProgramAsRPC)), GetPatch("UdonSyncPatch"), null);
                Instance.Patch(AccessTools.Property(typeof(Text), "text").GetMethod, null, GetPatch("TextPatch"));
                Instance.Patch(AccessTools.Property(typeof(Tools), "Platform").GetMethod, null, GetPatch("ModelSpoof"));
                Instance.Patch(typeof(IKSolverHeuristic).GetMethods().Where(m => m.Name.Equals("IsValid") && m.GetParameters().Length == 1).First(), prefix: new HarmonyMethod(typeof(PatchManager).GetMethod("IsValid", BindingFlags.NonPublic | BindingFlags.Static)));
                MethodInfo SteamPatch = typeof(PhotonPeer).Assembly.GetType("ExitGames.Client.Photon.EnetPeer").GetMethod("EnqueueOperation", (BindingFlags)(-1));
                Instance.Patch(SteamPatch, GetPatch("EnqueueOperationPatched"), null, null);
                Instance.Patch(typeof(NetworkManager).GetMethod("OnJoinedRoom"), GetPatch("OnJoinedRoom"), null);
                Instance.Patch(typeof(NetworkManager).GetMethod("OnLeftRoom"), GetPatch("OnLeftRoom"), null);
                Instance.Patch(typeof(PageWorldInfo).GetMethod("Method_Public_Void_ApiWorld_ApiWorldInstance_Boolean_Boolean_0"), GetPatch("SetupWorldPage"), null);
                typeof(VRCPlayer).GetMethods().Where(m => m.Name.StartsWith("Method_Private_Void_GameObject_VRC_AvatarDescriptor_Boolean_") && !m.checkXref("Avatar is Ready, Initializing")).ToList()
                    .ForEach(m => Instance.Patch(m, postfix: new HarmonyMethod(typeof(PatchManager).GetMethod("AvatarFinishedLoadingPostfix", BindingFlags.NonPublic | BindingFlags.Static))));
                AntiCrashHelper.Hook();
                Logger.WengaLogger("[Patches] Trigger");
                Logger.WengaLogger("[Patches] PingSpoof");
                Logger.WengaLogger("[Patches] Events");
                Logger.WengaLogger("[Patches] Moderations");
                Logger.WengaLogger("[Patches] Notification");
                Logger.WengaLogger("[Patches] Portals");
                Logger.WengaLogger("[Patches] FrameSpoof");
                Logger.WengaLogger("[Patches] Udon");
                Logger.WengaLogger("[Patches] Safety");
                Logger.WengaLogger("[Patches] Anticrash");
                Logger.WengaLogger("[Patches] Network Hooks");
                Logger.WengaLogger("[Patches] HWID Spoof");
            }
            catch (Exception arg)
            {
                Logger.WengaLogger(string.Format("[Patches] Failed Patching \n{0}", arg));
            }
            try
            {
                MethodInfo[] methods = typeof(AvatarPerformance).GetMethods(BindingFlags.Public | BindingFlags.Static);
                for (int i = 0; i < methods.Length; i++)
                    if (methods[i].Name == "Method_Public_Static_IEnumerator_String_GameObject_AvatarPerformanceStats_0" || methods[i].Name == "Method_Public_Static_IEnumerator_GameObject_AvatarPerformanceStats_EnumPublicSealedvaNoExGoMePoVe7vUnique_MulticastDelegateNPublicSealedVoUnique_0" || methods[i].Name == "Method_Public_Static_Void_String_GameObject_AvatarPerformanceStats_0")
                        Instance.Patch(methods[i], new HarmonyMethod(typeof(PatchManager).GetMethod("CalculatePerformance", BindingFlags.Static | BindingFlags.NonPublic)), null, null);
            }
            catch (Exception e)
            {
                Logger.WengaLogger("Failed to patch Performance Scanners: " + e);
            }
        }

        private static bool IsValid(ref IKSolverHeuristic __instance, ref bool __result)
        {
            if (__instance.maxIterations > 64)
            {
                __result = false;
                Logger.WengaLogger($"[Room] [Protection] Prevented Malicious IK Interaction");
                VRConsole.Log(VRConsole.LogsType.Protection, "Prevented Malicious IK Interaction");
                return false;
            }
            return true;
        }

        public static Uri avatarLink;
        public static APIUser currentUserSocial;
        public static bool canGet = true;
        public static void OnUserInfoOpen(MenuController __instance)
        {
            currentUserSocial = __instance.activeUser;
            avatarLink = new Uri(currentUserSocial.currentAvatarImageUrl);

            string adjustedLink = string.Format("https://{0}", avatarLink.Authority);

            for (int i = 0; i < avatarLink.Segments.Length - 2; i++)
            {
                adjustedLink += avatarLink.Segments[i];
            }

            avatarLink = new Uri(adjustedLink.Trim("/".ToCharArray()));
        }

        public static bool JoinRoomLog = false;
        private static void OpJoinRoomPatch(ref EnterRoomParams __0, ref bool __result)
        {
            if (JoinRoomLog)
            {
                try
                {
                    Console.WriteLine("OpJoinRoom: " + __result);
                    Console.WriteLine("field_Public_String_0: " + __0.field_Public_String_0);
                    Console.WriteLine("field_Public_Boolean_1: " + __0.field_Public_Boolean_0);
                    Console.WriteLine("field_Public_Boolean_1: " + __0.field_Public_Boolean_1);
                    Console.WriteLine("field_FamOrAssem_Boolean_0: " + __0.field_FamOrAssem_Boolean_0);
                    Console.WriteLine("string[]_0: " + __0.field_Public_ArrayOf_0?.ToArray().Join());
                    Console.WriteLine("ObjectPublicStObBoObUnique.field_Public_LobbyType_0: " + __0.field_Public_ObjectPublicStObBoObUnique_0?.field_Public_LobbyType_0.ToString());
                    Console.WriteLine("ObjectPublicStObBoObUnique.field_Public_String_0: " + __0.field_Public_ObjectPublicStObBoObUnique_0?.field_Public_String_0);
                    Console.WriteLine("ObjectPublicStObBoObUnique.prop_Boolean_0: " + __0.field_Public_ObjectPublicStObBoObUnique_0?.prop_Boolean_0);
                    Console.WriteLine("RoomOptions.field_Public_Byte_0: " + __0.field_Public_RoomOptions_0.field_Public_Byte_0);
                    Console.WriteLine("RoomOptions.prop_Boolean_0: " + __0.field_Public_RoomOptions_0.prop_Boolean_0);
                    Console.WriteLine("RoomOptions.prop_Boolean_1: " + __0.field_Public_RoomOptions_0.prop_Boolean_1);
                    Console.WriteLine("RoomOptions.prop_Boolean_2: " + __0.field_Public_RoomOptions_0.prop_Boolean_2);
                    Console.WriteLine("RoomOptions.prop_Boolean_3: " + __0.field_Public_RoomOptions_0.prop_Boolean_3);
                    Console.WriteLine("RoomOptions.prop_Boolean_4: " + __0.field_Public_RoomOptions_0.prop_Boolean_4);
                    Console.WriteLine("RoomOptions.prop_Boolean_5: " + __0.field_Public_RoomOptions_0.prop_Boolean_5);
                    Console.WriteLine("RoomOptions.prop_Boolean_6: " + __0.field_Public_RoomOptions_0.prop_Boolean_6);
                    Console.WriteLine("RoomOptions.field_Private_Boolean_0: " + __0.field_Public_RoomOptions_0.field_Private_Boolean_0);
                    Console.WriteLine("RoomOptions.field_Private_Boolean_1: " + __0.field_Public_RoomOptions_0.field_Private_Boolean_1);
                    Console.WriteLine("RoomOptions.field_Private_Boolean_2: " + __0.field_Public_RoomOptions_0.field_Private_Boolean_2);
                    Console.WriteLine("RoomOptions.field_Private_Boolean_3: " + __0.field_Public_RoomOptions_0.field_Private_Boolean_3);
                    Console.WriteLine("RoomOptions.field_Public_Int32_0: " + __0.field_Public_RoomOptions_0.field_Public_Int32_0);
                    Console.WriteLine("RoomOptions.field_Public_Int32_1: " + __0.field_Public_RoomOptions_0.field_Public_Int32_1);
                    Console.WriteLine("RoomOptions.String[]_0: " + __0.field_Public_RoomOptions_0.field_Public_ArrayOf_0?.ToArray().Join());
                    Console.WriteLine("RoomOptions.String[]_1: " + __0.field_Public_RoomOptions_0.field_Public_ArrayOf_1?.ToArray().Join());
                    object RoomOptions = Utils.Serialization.FromIL2CPPToManaged<object>(__0.field_Public_RoomOptions_0.field_Public_Hashtable_0);
                    Console.WriteLine("RoomOptions.field_Public_Hashtable_0: " + Newtonsoft.Json.JsonConvert.SerializeObject(RoomOptions));
                    object Hashtable = Utils.Serialization.FromIL2CPPToManaged<object>(__0.field_Public_Hashtable_0);
                    Console.WriteLine("field_Public_Hashtable_0: " + Newtonsoft.Json.JsonConvert.SerializeObject(Hashtable));


                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static void SetupWorldPage(ref ApiWorld __0)
        {
           
        }

        public static bool NeedRankPatch = true;
        private static void TextPatch(ref string __result, ref Text __instance)
        {
            if (__instance == null) return;
            else if (!__instance.gameObject.activeSelf) return;
            try
            {
                if (__instance.GetComponentInParent<UiVRCList>() != null && __instance.gameObject.name == "TitleText" && __instance.transform.parent.name == "Button" && __instance.GetComponentInParent<UiVRCList>().field_Private_GridLayoutGroup_0.rectChildren != null)
                {
                    var list = __instance.GetComponentInParent<UiVRCList>();
                    __result = $"{__result} [{list.field_Private_GridLayoutGroup_0.rectChildren.Count}]";
                }
                if (NeedRankPatch && (__result.Contains("Known User") || __result.Contains("Trusted User")))
                {
                    var Shortcut = GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu");
                    if (Shortcut.gameObject.active == true)
                    {
                        NeedRankPatch = false;
                        GameObject QMRankTextOn = Utils.QuickMenu.transform.Find("ShortcutMenu/Toggle_States_ShowTrustRank_Colors/TRUSTED/ON/Text_ShowTrustRank (1)").gameObject;
                        GameObject QMRankTextOff = Utils.QuickMenu.transform.Find("ShortcutMenu/Toggle_States_ShowTrustRank_Colors/TRUSTED/OFF/Text_ShowTrustRank (2)").gameObject;
                        switch (Utils.CurrentUser.GetAPIUser().GetRank().ToLower())
                        {
                            case "veteran":
                                QMRankTextOn.GetComponent<Text>().color = Color.yellow;
                                QMRankTextOff.GetComponent<Text>().color = Color.yellow;
                                __result = __result.Replace("Known User", "Veteran User");
                                __result = __result.Replace("Trusted User", "Veteran User");
                                break;
                            case "legend":
                                QMRankTextOn.GetComponent<Text>().color = Color.white;
                                QMRankTextOff.GetComponent<Text>().color = Color.white;
                                __result = __result.Replace("Known User", "Legend User");
                                __result = __result.Replace("Trusted User", "Legend User");
                                break;
                            case "known":
                                GameObject QMRankTextOn2 = Utils.QuickMenu.transform.Find("ShortcutMenu/Toggle_States_ShowTrustRank_Colors/KNOWN/ON/Text_ShowTrustRank (1)").gameObject;
                                GameObject QMRankTextOff2 = Utils.QuickMenu.transform.Find("ShortcutMenu/Toggle_States_ShowTrustRank_Colors/KNOWN/OFF/Text_ShowTrustRank (3)").gameObject;
                                QMRankTextOn2.GetComponent<Text>().color = new Color(0.92f, 0.37f, 0f);
                                QMRankTextOff2.GetComponent<Text>().color = new Color(0.92f, 0.37f, 0f);
                                break;
                            case "trusted":
                                QMRankTextOn.GetComponent<Text>().color = new Color(0.87f, 0f, 0.5f);
                                QMRankTextOff.GetComponent<Text>().color = new Color(0.87f, 0f, 0.5f);
                                break;
                        }
                    } 
                }
            }
            catch { }
            return;
        }

        public static bool OperationLog = false;
        private static bool OperationPatch(ref byte __0, ref Il2CppSystem.Collections.Generic.Dictionary<byte, Il2CppSystem.Object> __1, ref SendOptions __2)
        {
            if (OperationLog)
            {
                try
                {
                    Logger.WengaLogger($"[Operation] {__0}");
                    object Data = Utils.Serialization.FromIL2CPPToManaged<object>(__1.Values);
                    Logger.WengaLogger($"[Dictionary] \n{Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented)}");
                    Logger.WengaLogger($"[SendOptions] Channel: {__2.Channel} Mode: {__2.DeliveryMode} Encrypt: {__2.Encrypt}");
                }
                catch { }
            }
            return true;
        }

        private static bool EnterPortalPrefix(PortalTrigger __instance, MethodInfo __originalMethod)
        {
            try
            {
                var portalInternal = __instance.field_Private_PortalInternal_0;
                string dropper;
                using (StringReader sr = new StringReader(__instance.transform.Find("NameTag").GetComponentInChildren<TMPro.TextMeshPro>().text))
                {
                    sr.ReadLine();
                    dropper = sr.ReadLine();
                    dropper = dropper == "" ? "No Player" : dropper;
                }
                if (Vector3.Distance(Utils.CurrentUser.transform.position, __instance.transform.position) > 0.8f)
                {
                    return false;
                }
                {
                    Utils.VRCUiPopupManager.Alert("Enter Portal", $"{portalInternal.field_Private_ApiWorld_0.name} \n by {dropper}", "Yes", new Action(() =>
                    {
                        Networking.GoToRoom(portalInternal.field_Private_ApiWorld_0.id + ":" + portalInternal.field_Private_String_1);
                        Utils.VRCUiPopupManager.HideCurrentPopUp();
                    }), "No", new Action(() =>
                    {
                        Utils.VRCUiPopupManager.HideCurrentPopUp();
                    }));
                    return false;
                }
            }
            catch { }
            return true;
        }

        public static void QuestIni()
        {
            if (Create.Ini.GetBool("Toggles", "QuestSpoof", true))
            {
                QuestSpoof = true;
            }
            else
            {
                QuestSpoof = false;
            }
        }

        public static bool QuestSpoof = false;

        private static void ModelSpoof(ref string __result)
        {
            if (QuestSpoof)
            {
                if (RoomManager.field_Internal_Static_ApiWorldInstance_0 == null)
                {
                    __result = "android";
                }
            }
        }

        public static System.Collections.IEnumerator QuestSpoofDelay()
        {
            yield return new WaitForSeconds(4);
            QuestSpoof = false;
            yield break;
        }

        private static bool CalculatePerformance() => false;

        private static void FakePing(ref short __result)
        {
            if (PingSpoof)
            {
                System.Random r = new System.Random();
                var values = new[] { -69, -666 };
                int result = values[r.Next(values.Length)];
                __result = (short)result;
            }
        }

        private static void FakeFrames(ref float __result)
        {
            if (FrameSpoof)
            {
                System.Random r = new System.Random();
                var values = new[] { int.MinValue,int.MaxValue };
                int result = values[r.Next(values.Length)];
                __result = (float)1 / result;
                return;
            }
        }

        private static void OnJoinedRoom()
        {
            MelonCoroutines.Start(NoClipping.SetNearClipPlane(0.001f));
            if (LoginDelay)
            {
                LoginDelay = false;
                MelonCoroutines.Start(QuestSpoofDelay());
                ExploitMenu.ButtonToggles();
                Api.ApiExtension.Start();
            }
        }

        private static void OnLeftRoom()
        {
            
        }

        private static void AvatarFinishedLoadingPostfix(VRCPlayer __instance, GameObject __0, bool __2)
        {
            try
            {
                if (__instance == null || __0 == null || !__2) return;
                var player = __instance;
                var Avatar = __instance.GetAPIAvatar();
                var AvatarID = __instance.GetAPIAvatar().id;

                APIUser apiUser = __instance.field_Private_Player_0.field_Private_APIUser_0;
                var AvatarGameobject = __0;

                Logger.WengaLogger($"[Room] [Avatar] {player.DisplayName()} -> {Avatar.name} [{Avatar.releaseStatus}]");
                VRConsole.Log(VRConsole.LogsType.Avatar, $"{player.DisplayName()} --> {Avatar.name} [{Avatar.releaseStatus}]");
                GlobalDynamicBones.ProcessDynamicBones(AvatarGameobject, player);
                GlobalDynamicBones.OptimizeBone(AvatarGameobject);
                GlobalDynamicBones.DisableAvatarFeatures(AvatarGameobject, player);
            }
            catch { }
        }

        public unsafe static void PatchSafety()
        {
            Logger.WengaLogger("[Patches] HWID Spoofing. . .");
            MainHWID = SystemInfo.deviceUniqueIdentifier;
            System.Random random = new System.Random(System.Environment.TickCount);
            byte[] array = new byte[SystemInfo.deviceUniqueIdentifier.Length / 2];
            random.NextBytes(array);
            SpoofHWID = string.Join("", from it in array select it.ToString("x2"));
            var mainmethod = IL2CPP.il2cpp_resolve_icall("UnityEngine.SystemInfo::GetDeviceUniqueIdentifier");
            MelonUtils.NativeHookAttach((IntPtr)(&mainmethod), AccessTools.Method(typeof(PatchManager), "FakeDeviceID").MethodHandle.GetFunctionPointer());
            Logger.WengaLogger($"[HWID] Before {MainHWID}");
            Logger.WengaLogger($"[HWID] Spoofing {SpoofHWID}");
            Logger.WengaLogger($"[HWID] After {SystemInfo.deviceUniqueIdentifier}");
            if (SpoofHWID == null || SpoofHWID == MainHWID)
            {
                Logger.WengaLogger("Failed to spoof HWID");
                Process.GetCurrentProcess().Kill();
            }
        }
        static string SpoofHWID;
        static string MainHWID;

        public static IntPtr FakeDeviceID()
        {
            return new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(SpoofHWID)).Pointer;
        }

        private static bool OpRaiseEventPrefix(ref byte __0, ref Il2CppSystem.Object __1, ref ObjectPublicStObBoObUnique __2, ref SendOptions __3)
        {
            try
            {
                switch (__0)
                {
                    case 202:
                        return !PhotonModule.Invisible;
                    case 254:
                        return !PhotonModule.Invisible;
                    case 7:
                        return !PhotonModule.Serialize;
                    case 206:
                        return !PhotonModule.Serialize;
                    case 201:
                        return !PhotonModule.Serialize;
                    case 4:
                        return !PhotonModule.LockInstance;
                    case 5:
                        return !PhotonModule.LockInstance;
                    case 1:
                        return !PhotonModule.Forcemute;
                    default:
                        break;
                }
            }
            catch { }
            return true;
        }

        private static void OnPlayerLeft(VRC.Player __0)
        {
            try
            {
                VRConsole.Log(VRConsole.LogsType.Left, __0.DisplayName());
                Logger.WengaLogger($"[-] {__0.DisplayName()}");
                Utils.VRCUiManager.QueHudMessage($"<color=red>[-] {__0.DisplayName()}</color>");
            }
            catch { }
        }
        public static bool LoginDelay = true;

        private static void OnPlayerJoin(VRC.Player __0)
        {
            try
            {
                //PlayerList.IsAllowedClient();
                VRConsole.Log(VRConsole.LogsType.Join, __0.DisplayName());
                Logger.WengaLogger($"[+] {__0.DisplayName()}");
                Utils.VRCUiManager.QueHudMessage($"<color=lime>[+] {__0.DisplayName()}</color>");
                if (PlayerList.CheckWenga(__0.UserID()))
                {
                    MelonCoroutines.Start(PlayerList.AdminPlateChanger(__0));
                }
                MelonCoroutines.Start(PlayerList.CustomTag(__0));
                if (GlobalDynamicBones.FriendBones)
                {
                    PlayerList.DynBoneAdder(__0);
                }
            }
            catch { }
        }

        private static bool RequestPatch(ref string __0, ref Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object> __2)
        {
            if (__2 != null && WorldSpoof)
            {
                if (__0 == "visits" || __0 == "joins")
                {
                    __2.Clear();
                    __2.Add("userId", APIUser.CurrentUser.id);
                    __2.Add("worldId", $"wrld_ffe773ba-c31f-4f56-928b-0da8f256e435:666~private({APIUser.CurrentUser.id})~nonce(4825472E3261BA1D30C6FA4C375DC86FB67AA6627890F365286C67EFAF081D03)");
                }
            }
            bool flag3 = OfflineMode && (__0 == "visits" || __0 == "joins");
            return !flag3;
        }

        public static bool WorldTrigger = false;
        public static bool AntiWorldTrigger = false;
        public static bool AntiUdon = false;
        public static bool AntiMasterDC = false;
        public static bool OfflineMode = false;
        public static bool WorldSpoof = false;
        public static bool FrameSpoof = true;
        public static bool PingSpoof = true;
        public static bool EventLog = false;
        public static bool DictLog = false;
        public static bool RPCLog = true;

        private static bool UdonSyncPatch(string __0)
        {
            if (AntiUdon)
            {
                Logger.WengaLogger($"[Room] [Protection] Prevented Udon Event [{__0}]");
                VRConsole.Log(VRConsole.LogsType.Protection, $"Prevented Udon Event [{__0}]");
                return false;
            }
            return true;
        }

        private static bool TriggerEvent(ref VrcEvent __0, ref VrcBroadcastType __1, ref int __2)
        {
            try
            {
                if (WorldTrigger)
                {
                    __1 = (VrcBroadcastType)4;
                }
            }
            catch { }
            return true;
        }

        private static bool EnqueueOperationPatched(Il2CppSystem.Collections.Generic.Dictionary<byte, Il2CppSystem.Object> __0, byte __1)
        {
            try
            {
                if (__1 == 252)
                {
                    maskProperties(__0, 251);
                }
            }
            catch (Exception value)
            {
                Logger.WengaLogger(value);
            }
            return true;
        }

        public static bool VRMode = true;
        public static bool InvisibleHide = false;
        public static bool MiniHide = false;
        public static bool AntiBlock = true;
        private static void maskProperties(Il2CppSystem.Collections.Generic.Dictionary<byte, Il2CppSystem.Object> param, byte propertyIndex)
        {
            try
            {
                Hashtable hashtable = param[propertyIndex].Cast<Hashtable>();
                //if (hashtable.ContainsKey("steamUserID"))
                //{
                //    hashtable["steamUserID"] = "666";
                //}
                if (hashtable.ContainsKey("inVRMode"))
                {
                    hashtable["inVRMode"] = new Il2CppSystem.Boolean()
                    {
                        m_value = VRMode
                    }.BoxIl2CppObject();
                }
                if (hashtable.ContainsKey("avatarEyeHeight"))
                {
                    if (InvisibleHide)
                    {
                        hashtable["avatarEyeHeight"] = new Il2CppSystem.Int32()
                        {
                            m_value = int.MinValue
                        }.BoxIl2CppObject();
                    }
                    else if (MiniHide)
                    {
                        hashtable["avatarEyeHeight"] = new Il2CppSystem.Int32()
                        {
                            m_value = 1
                        }.BoxIl2CppObject();
                    }
                }
            }
            catch (Exception value)
            {
                Logger.WengaLogger(value);
            }
        }

        public static bool BlockPlayer = false;
        public static bool EventDelay = false;
        public static bool HideCamera = false;

        public static System.Collections.IEnumerator EventDelayer()
        {
            EventDelay = true;
            yield return new WaitForSeconds(10);
            EventDelay = false;
            yield break;
        }

        private static readonly byte[] BotIgnoreCodes = new byte[]
        {
            7,206,201,226,202,254,4,5,1,33,230,255,3,253
        };

        private static readonly byte[] IgnoreCodes = new byte[]
        {
            1,7,8,9,206,201,226
        };

        private static bool OnEvent(ref EventData __0)
        {
            try
            {
                if (!IgnoreCodes.Contains(__0.Code) && DictLog)
                {
                    object Data = Utils.Serialization.FromIL2CPPToManaged<object>(__0.Parameters);
                    Logger.WengaLogger($"[Event {__0.Code}] Event \n{Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented)}");
                }
                else if (EventLog)
                {
                    Logger.WengaLogger($"[PhotonEvent] {__0.Code}");
                }
                if (!BotIgnoreCodes.Contains(__0.Code) && Utils.PlayerManager.GetPlayerWithPlayerID(__0.Sender).GetVRCPlayer().GetIsBot() && !EventDelay)
                {
                    Logger.WengaLogger($"[Room] [Protection] Prevented PhotonEvent {__0.Code} from {Utils.PlayerManager.GetPlayer(__0.Sender).DisplayName()}");
                    VRConsole.Log(VRConsole.LogsType.Protection, $"{Utils.PlayerManager.GetPlayer(__0.Sender).DisplayName()} --> Bot Event [{__0.Code}]");
                    return false;
                }
                else
                {
                    switch (__0.Code)
                    {
                        case 33:
                            object Data = Utils.Serialization.FromIL2CPPToManaged<object>(__0.Parameters);
                            if (Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented).Contains("\"10\": false,"))
                            {
                                JObject jObject = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented));
                                var jo = JObject.Parse(jObject.ToString());
                                var id = jo["245"]["1"].ToString();

                                var ParsedID = Utils.PlayerManager.GetPlayerWithPlayerID(int.Parse(id)).GetAPIUser().DisplayName();
                                if (ParsedID == null)
                                {
                                    ParsedID = "?";
                                }

                                Logger.WengaLogger($"[Moderation] {ParsedID} unblocked you");
                                VRConsole.Log(VRConsole.LogsType.Block, $"{ParsedID} --> Unblocked You");
                                PlayerList.BlockList.Remove(Utils.PlayerManager.GetPlayerWithPlayerID(int.Parse(id)).UserID());
                                if (Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented).Contains("\"11\": false"))
                                {
                                    Logger.WengaLogger($"[Moderation] {ParsedID} unmuted you");
                                    VRConsole.Log(VRConsole.LogsType.Voice, $"{ParsedID} --> Unmuted You");
                                }
                                else if (Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented).Contains("\"11\": true"))
                                {
                                    Logger.WengaLogger($"[Moderation] {ParsedID} muted you");
                                    VRConsole.Log(VRConsole.LogsType.Voice, $"{ParsedID} --> Muted You");
                                }
                            }
                            else if (Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented).Contains("\"10\": true,"))
                            {
                                JObject jObject = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented));
                                var jo = JObject.Parse(jObject.ToString());
                                var id = jo["245"]["1"].ToString();

                                var ParsedID = Utils.PlayerManager.GetPlayerWithPlayerID(int.Parse(id)).GetAPIUser().DisplayName();
                                if (ParsedID == null)
                                {
                                    ParsedID = "?";
                                }

                                Logger.WengaLogger($"[Moderation] {ParsedID} blocked you");
                                VRConsole.Log(VRConsole.LogsType.Block, $"{ParsedID} --> Blocked You");
                                PlayerList.BlockList.Add(Utils.PlayerManager.GetPlayerWithPlayerID(int.Parse(id)).UserID());
                                if (Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented).Contains("\"11\": false"))
                                {
                                    Logger.WengaLogger($"[Moderation] {ParsedID} unmuted you");
                                    VRConsole.Log(VRConsole.LogsType.Voice, $"{ParsedID} --> Unmuted You");
                                }
                                else if (Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented).Contains("\"11\": true"))
                                {
                                    Logger.WengaLogger($"[Moderation] {ParsedID} muted you");
                                    VRConsole.Log(VRConsole.LogsType.Voice, $"{ParsedID} --> Muted You");
                                }
                                return !AntiBlock;
                            }
                            else if (Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented).Contains("You have been warned"))
                            {
                                Logger.WengaLogger($"[Moderation] The Room Owner warned you");
                                VRConsole.Log(VRConsole.LogsType.Warn, $"RoomOwner --> Warn You");
                                return false;
                            }
                            else if (Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented).Contains("A vote kick has been"))
                            {
                                Logger.WengaLogger("[Moderation] Someone votekick Someone");
                                VRConsole.Log(VRConsole.LogsType.Votekick, $"[?] --> [?]");
                            }
                            else if (Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented).Contains("Unable to start a vote to kick"))
                            {
                                Logger.WengaLogger("[Moderation] Failed at Votekick");
                                VRConsole.Log(VRConsole.LogsType.Votekick, $"Failed at Votekick");
                                return false;
                            }
                            else if (Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented).Contains("\"0\": 8"))
                            {
                                Logger.WengaLogger("[Moderation] The Room Owner ForceMicOff You");
                                VRConsole.Log(VRConsole.LogsType.Voice, $"RoomOwner --> MicOff You");
                                return false;
                            }
                            break;
                        case 2:
                            object Data2 = Utils.Serialization.FromIL2CPPToManaged<object>(__0.Parameters);
                            if (Newtonsoft.Json.JsonConvert.SerializeObject(Data2, Newtonsoft.Json.Formatting.Indented).Contains("You have been kicked"))
                            {
                                Logger.WengaLogger("[Moderation] RoomOwner kicked you");
                                VRConsole.Log(VRConsole.LogsType.Kick, $"RoomOwner --> You");
                                return false;
                            }
                            break;
                        case 1:
                            Il2CppStructArray<byte> il2CppStructArray2 = new Il2CppStructArray<byte>(__0.CustomData.Pointer);
                            bool flag4 = il2CppStructArray2.Length > 1700;
                            if (flag4)
                            {
                                Logger.WengaLogger($"[Room] [Protection] Prevented USpeak Event {il2CppStructArray2.Length}");
                                VRConsole.Log(VRConsole.LogsType.Protection, $"Prevented USpeak Event {il2CppStructArray2.Length}");
                                return false;
                            }
                            break;
                        default:
                            break;
                    }
                }
                if (PhotonModule.EventBlock.Contains(Utils.PlayerManager.GetPlayerWithPlayerID(__0.Sender).UserID()))
                {
                    return false;
                }
            }
            catch { }
            return true;
        }

        private static bool IsNaN(Vector3 v3)
        {
            return float.IsNaN(v3.x) || float.IsNaN(v3.y) || float.IsNaN(v3.z);
        }

        private static bool IsNanPos(Vector3 v3)
        {
            return int.MaxValue < v3.x || int.MaxValue < v3.y || int.MaxValue < v3.z || int.MinValue > v3.x || int.MinValue > v3.y || int.MinValue > v3.z;
        }

        private static bool IsMaxPos(Vector3 v3)
        {
            return int.MaxValue == v3.x || int.MaxValue == v3.y || int.MaxValue == v3.z || int.MinValue == v3.x || int.MinValue == v3.y || int.MinValue == v3.z;
        }

        private static bool CaughtEventPatch(ref VRC.Player __0, ref VrcEvent __1, ref VrcBroadcastType __2, ref int __3, ref float __4)
        {
            try
            {
                VRC.Player instance = __0;
                string name = __1.ParameterObject.name;
                string text = instance.DisplayName();
                string senderID = instance.UserID();
                Il2CppSystem.Object[] array = Networking.DecodeParameters(__1.ParameterBytes);
                if (array == null)
                {
                    array = new Il2CppSystem.Object[0];
                }
                VRC.Player player = null;
                string a2 = string.Empty;
                string text3 = string.Empty;
                try
                {
                    player = Utils.PlayerManager.GetPlayer(Il2CppSystem.Convert.ToString(array[0]));
                    text3 += player.DisplayName();
                    for (int i = 1; i < array.Length - 1; i++)
                    {
                        text3 = text3 + " [" + Il2CppSystem.Convert.ToString(array[i]) + "]";
                    }
                    a2 = player.UserID();
                }
                catch { }
                string text4 = string.Empty;
                string text4clean = string.Empty;
                foreach (Il2CppSystem.Object value in array)
                {
                    text4clean = Il2CppSystem.Convert.ToString(value);
                    text4 = "[" + Il2CppSystem.Convert.ToString(value) + "]";
                }
                if (instance.GetVRCPlayer().GetIsBot() || PhotonModule.RPCBlock.Contains(instance.UserID()))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(string.Concat(new string[]
                    {
                        "\n[BLOCKED RPC] \nPLAYER: ",text," \nEXECUTED:", __1.ParameterString, " \nOBJECT: ", __1.ParameterObject.name," [", __2.ToString(),"](",__3.ToString(),"/",__4.ToString(),")"
                    }));
                    Console.ResetColor();
                    return false;
                }
                else if (__1.ParameterObject.name != "USpeak" && __1.ParameterString != "SetTimerRPC" && RPCLog)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;

                    Console.WriteLine(string.Format("\n[RPC] \nPLAYER: {0} \nOBJECT: {1}  \nEXECUTED: {2} \nFOR: {3} \nType: {4} [{5}] L: {6}", new object[]
                    {
                        text,__1.ParameterObject.name,__1.ParameterString,(player == null) ? text4 : text3,__1.EventType,__2,array.Length
                    }));
                    Console.ResetColor();
                }
                
                if (__1.ParameterObject != null)
                {
                    if (__0.field_Private_APIUser_0.id != APIUser.CurrentUser.id && IsNaN(__1.ParameterObject.transform.position))
                    {
                        VRConsole.Log(VRConsole.LogsType.Protection, text + " --> used Infinity-Events");
                        Logger.WengaLogger("[Room] [Protection] Prevented " + text + " from using Infinity-Events");
                        return false;
                    }
                    else if (__0.field_Private_APIUser_0.id != APIUser.CurrentUser.id && IsNanPos(__1.ParameterObject.transform.position))
                    {
                        VRConsole.Log(VRConsole.LogsType.Protection, text + " --> spawned Infinity-Objects");
                        Logger.WengaLogger("[Room] [Protection] Prevented " + text + " from spawning Objects at Infinity");
                        return false;
                    }
                    else if (__0.field_Private_APIUser_0.id != APIUser.CurrentUser.id && IsMaxPos(__1.ParameterObject.transform.position))
                    {
                        VRConsole.Log(VRConsole.LogsType.Protection, text + " --> spawned MaxValue-Objects");
                        Logger.WengaLogger("[Room] [Protection] Prevented " + text + " from spawning Objects at MaxValue");
                        return false;
                    }
                    else if (__1.ParameterObject.name == "VRCVideoSync" && BlockPlayer)
                    {
                        Logger.WengaLogger("[Room] [Protection] Video not loading - Sync disabled");
                        VRConsole.Log(VRConsole.LogsType.Protection, "Video Sync --> Disabled");
                        return false;
                    }
                }

                else if(__0.field_Private_APIUser_0.id != APIUser.CurrentUser.id)
                {
                    if (__1.EventType == VrcEventType.SetGameObjectActive || __1.EventType == VrcEventType.AnimationTrigger || __1.EventType == VrcEventType.AudioTrigger || __1.EventType == VrcEventType.SetComponentActive)
                    {
                        Logger.WengaLogger($"[Room] [Protection] Prevented {text} from using null Trigger");
                        VRConsole.Log(VRConsole.LogsType.Protection, $"{text} --> null Trigger");
                        return false;
                    }
                }

                if (__1.ParameterString != null)
                {
                    switch (__1.ParameterString)
                    {
                        case "ConfigurePortal":
                            VRConsole.Log(VRConsole.LogsType.Portal, text + " --> Portaldrop");
                            Logger.WengaLogger($"[Room] [Portal] {text} spawned a Portal");
                            if (__0.field_Private_APIUser_0.id != APIUser.CurrentUser.id && (PortalHandler.AntiPortal || !__0.field_Private_APIUser_0.isFriend && PortalHandler.FriendOnlyPortal))
                            {
                                return false;
                            }
                            break;

                        case "PlayEffect":
                            VRConsole.Log(VRConsole.LogsType.Portal, text + " --> Portal entered");
                            Logger.WengaLogger("[Room] [Portal]" + text + " entered a Portal");
                            break;

                        case "_DestroyObject":
                            VRConsole.Log(VRConsole.LogsType.Portal, text + " --> Destroyed Object");
                            Logger.WengaLogger($"[Room] [Portal] {text} destroyed an Object");
                            break;

                        case "CleanRoomRPC":
                            VRConsole.Log(VRConsole.LogsType.Info, text + " --> Cleaned Room");
                            Logger.WengaLogger($"[Room] [Info] {text} cleaned the Room");
                            break;

                        case "PlayEmoteRPC":
                            if (__0.field_Private_APIUser_0.id != APIUser.CurrentUser.id && (Convert.ToInt32(text4clean) < 0 || Convert.ToInt32(text4clean) > 8))
                            {
                                Logger.WengaLogger($"[Room] [Protection] {text} played out of Range Emote");
                                VRConsole.Log(VRConsole.LogsType.Protection, $"{text} --> out of Range Emote");
                                return false;
                            }
                            //VRConsole.Log(VRConsole.LogsType.Info,$" {text} --> Emote");
                            Logger.WengaLogger($"[Room] [Info] {text} played Emote");
                            break;

                        case "SpawnEmojiRPC":
                            if (__0.field_Private_APIUser_0.id != APIUser.CurrentUser.id && (Convert.ToInt32(text4clean) < 0 || Convert.ToInt32(text4clean) > 57))
                            {
                                Logger.WengaLogger($"[Room] [Protection] {text} played out of Range Emoji");
                                VRConsole.Log(VRConsole.LogsType.Protection, $"{text} --> out of Range Emoji");
                                return false;
                            }
                            VRConsole.Log(VRConsole.LogsType.Info, $"{text} --> Emoji");
                            Logger.WengaLogger($"[Room] [Info] {text} played Emoji");
                            break;

                        case "_InstantiateObject":
                            if (__0.field_Private_APIUser_0.id != APIUser.CurrentUser.id && text4.Contains("Infinity"))
                            {
                                VRConsole.Log(VRConsole.LogsType.Protection, $"{text} --> Instantiate Infinity-Objects");
                                Logger.WengaLogger($"[Room] [Protection] Prevented {text} from Instantiating Objects at Infinity");
                                return false;
                            }
                            VRConsole.Log(VRConsole.LogsType.Info, $"{text} --> Instantiate Object");
                            Logger.WengaLogger($"[Room] [Info] {text} instantiated Object");
                            break;

                        case "ChangeVisibility":
                            if (text4.Contains("True"))
                            {
                                VRConsole.Log(VRConsole.LogsType.Info, $"{text} --> Camera Show");
                                Logger.WengaLogger($"[Room] [Info] {text} showed the Camera");
                                if (__0.field_Private_APIUser_0.id == APIUser.CurrentUser.id && HideCamera)
                                {
                                    return false;
                                }
                            }
                            else if (text4.Contains("False"))
                            {
                                VRConsole.Log(VRConsole.LogsType.Info, $"{text} --> Camera Hide");
                                Logger.WengaLogger($"[Room] [Info] {text} hide the Camera");
                            }
                            else if (text4 == null)
                            {
                                VRConsole.Log(VRConsole.LogsType.Info, $"{text} --> null Camera");
                                Logger.WengaLogger($"[Room] [Info] {text} used a null Camera");
                            }
                            break;
                        case "InteractWithStationRPC":
                            if (__0.field_Private_APIUser_0.id == APIUser.CurrentUser.id && ItemHandler.ChairToggle)
                            {
                                return false;
                            }
                            else if (text4.Contains("True"))
                            {
                                VRConsole.Log(VRConsole.LogsType.Info, $"{text} --> Chair used");
                                Logger.WengaLogger($"[Room] [Info] {text} used a Chair");
                            }
                            else if (text4.Contains("False"))
                            {
                                VRConsole.Log(VRConsole.LogsType.Info, $"{text} --> Chair unused");
                                Logger.WengaLogger($"[Room] [Info] {text} unused a Chair");
                            }
                            else if (text4 == null)
                            {
                                VRConsole.Log(VRConsole.LogsType.Protection, $"{text} --> null Chair");
                                Logger.WengaLogger($"[Room] [Info] {text} used a null Chair");
                                return false;
                            }
                            break;
                        default:
                            break;
                    }
                }
                if (__2 == 0 && senderID != APIUser.CurrentUser.id)
                {
                    if (AntiMasterDC)
                    {
                        VRConsole.Log(VRConsole.LogsType.Protection, $"{text} --> Always Event [{__1.EventType}]");
                        Logger.WengaLogger($"[Room] [Protection] Prevented {text} from using Always Event [{__1.EventType}]");
                        return false;
                    }
                    else if (__1.ParameterString.Length >= 100)
                    {
                        VRConsole.Log(VRConsole.LogsType.Protection, $"{text} --> Always Event [{__1.EventType}]");
                        Logger.WengaLogger($"[Room] [Protection] Prevented {text} from using Event Disconnect Exploit [{__1.EventType}]");
                        return false;
                    }
                }
                if (senderID != APIUser.CurrentUser.id && AntiWorldTrigger && (__1.EventType == VrcEventType.SetGameObjectActive || __1.EventType == VrcEventType.AnimationBool))
                {
                    if (__2 == 0 || __2 == VrcBroadcastType.AlwaysUnbuffered || __2 == VrcBroadcastType.AlwaysBufferOne)
                    {
                        VRConsole.Log(VRConsole.LogsType.Protection, $"{text} --> WorldTrigger");
                        Logger.WengaLogger($"[Room] [Protection] Prevented {text} from using Worldtrigger");
                        return false;
                    }
                }
            }
            catch { }
            return true;
        }
    }
}