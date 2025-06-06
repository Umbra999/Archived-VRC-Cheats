using AmplitudeSDKWrapper;
using BestHTTP.JSON;
using ExitGames.Client.Photon;
using HarmonyLib;
using LunaR.Buttons.Bots;
using LunaR.ButtonUI;
using LunaR.ConsoleUtils;
using LunaR.Modules;
using LunaR.Patching;
using LunaR.QMButtons;
using LunaR.Wrappers;
using MelonLoader;
using Newtonsoft.Json;
using Photon.Realtime;
using RootMotion.FinalIK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;
using VRC.Core;
using VRC.DataModel;
using VRC.Management;
using VRC.SDKBase;
using VRC.SDKBase.Validation.Performance;
using VRC.UI;
using VRC.UI.Elements.Controls;
using VRC.UI.Elements.Menus;

namespace LunaR.Extensions
{
    internal class Patching
    {
        private static readonly HarmonyLib.Harmony Instance = new("LunaR");

        private static HarmonyMethod GetPatch(string name)
        {
            return new HarmonyMethod(typeof(Patching).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic));
        }

        private static void CreatePatch(MethodInfo TargetMethod, HarmonyMethod Before = null, HarmonyMethod After = null)
        {
            try
            {
                Instance.Patch(TargetMethod, Before, After);
            }
            catch (Exception e)
            {
                Logger.LogError($"Failed to Patch {TargetMethod.Name} \n{e}");
            }
        }

        public static unsafe void InitEarlyPatch()
        {
            CreatePatch(AccessTools.Property(typeof(PhotonPeer), "RoundTripTime").GetMethod, null, GetPatch(nameof(FakePing)));
            CreatePatch(AccessTools.Property(typeof(PhotonPeer), "RoundTripTimeVariance").GetMethod, null, GetPatch(nameof(FakeLatency)));
            CreatePatch(AccessTools.Property(typeof(Time), "smoothDeltaTime").GetMethod, null, GetPatch(nameof(FakeFrames)));
            CreatePatch(AccessTools.Property(typeof(Networking), "LocalPlayer").GetMethod, null, GetPatch(nameof(LocalPlayerPatch)));
            CreatePatch(typeof(ApiWorldUpdate).GetMethod(nameof(ApiWorldUpdate.Update)), GetPatch(nameof(APIUpdate)));
            CreatePatch(typeof(ApiWorldUpdate).GetMethod(nameof(ApiWorldUpdate.Method_Private_Void_String_0)), GetPatch(nameof(APIStringUpdate)));
            CreatePatch(typeof(WebSocketSharp.WebSocket).GetMethod(nameof(WebSocketSharp.WebSocket.messagec)), GetPatch(nameof(SocketReceive)));
            CreatePatch(typeof(NetworkManager).GetMethod(nameof(NetworkManager.Method_Public_Virtual_Final_New_Void_Player_0)), GetPatch(nameof(OnPhotonJoin)));
            CreatePatch(typeof(NetworkManager).GetMethod(nameof(NetworkManager.Method_Public_Virtual_Final_New_Void_Player_1)), GetPatch(nameof(OnPhotonLeft)));
            CreatePatch(typeof(VRCNetworkingClient).GetMethod(nameof(VRCNetworkingClient.OnEvent)), GetPatch(nameof(OnEvent)));
            CreatePatch(typeof(NetworkManager).GetMethod(nameof(NetworkManager.OnJoinedRoom)), GetPatch(nameof(OnJoinedRoom)));
            CreatePatch(typeof(NetworkManager).GetMethod(nameof(NetworkManager.OnLeftRoom)), GetPatch(nameof(OnLeftRoom)));
            CreatePatch(typeof(PhotonPeer).GetMethods().Where(m => m.Name == "SendOperation").First(), GetPatch(nameof(OperationPatch)));
            CreatePatch(typeof(VRCNetworkingClient).GetMethod(nameof(VRCNetworkingClient.Method_Public_Virtual_New_Boolean_Byte_Object_RaiseEventOptions_SendOptions_0)), GetPatch(nameof(OpRaiseEventPrefix)));
            CreatePatch(typeof(NetworkManager).GetMethod(nameof(NetworkManager.OnMasterClientSwitched)), GetPatch(nameof(MasterSwitched)));
            CreatePatch(typeof(VRC_EventHandler).GetMethod(nameof(VRC_EventHandler.InternalTriggerEvent)), GetPatch(nameof(TriggerEvent)));
            CreatePatch(typeof(VRCAvatarManager).GetMethod(nameof(VRCAvatarManager.Method_Private_Boolean_ApiAvatar_GameObject_0)), GetPatch(nameof(AvatarInitialized)));
            CreatePatch(typeof(AssetManagement).GetMethod(nameof(AssetManagement.Method_Public_Static_Object_Object_Boolean_Boolean_Boolean_1)), GetPatch(nameof(AssetsLoaded)));
            CreatePatch(typeof(PortalInternal).GetMethod(nameof(PortalInternal.SetTimerRPC)), GetPatch(nameof(PortalTimer)));
            CreatePatch(typeof(VRC.Udon.VM.UdonVM).GetMethod(nameof(VRC.Udon.VM.UdonVM.LoadProgram)), GetPatch(nameof(InitializeUdonProgram)));
            CreatePatch(typeof(VRC_StationInternal).GetMethod(nameof(VRC_StationInternal.Method_Public_Boolean_Player_Boolean_0)), GetPatch(nameof(ChairPatch)));
            CreatePatch(typeof(AvatarPerformance).GetMethod(nameof(AvatarPerformance.GetPerformanceScannerSet)), GetPatch(nameof(CalculatePerformance)));
            CreatePatch(typeof(NetworkManager).GetMethod(nameof(NetworkManager.Method_Public_Void_Player_1)), GetPatch(nameof(PlayerInstantiated)));
            CreatePatch(typeof(VRCGameObjectExtensions).GetMethod(nameof(VRCGameObjectExtensions.Method_Public_Static_Void_Object_Color_0)), GetPatch(nameof(PatchDebugLevel)));
            CreatePatch(typeof(PortalInternal).GetMethod(nameof(PortalInternal.Method_Public_Static_Boolean_ApiWorld_ApiWorldInstance_Vector3_Vector3_Action_1_String_0)), GetPatch(nameof(CreatePortal)));

            foreach (MethodInfo method in typeof(SelectedUserMenuQM).GetMethods().Where(x => x.Name.StartsWith("Method_Private_Void_IUser_") && x.Name.Contains("PDM"))) 
            {
                CreatePatch(method, GetPatch(nameof(SetUserPatch)));
            }

            CreatePatch(typeof(IKSolverHeuristic).GetMethods().First(m => m.Name.Equals("IsValid") && m.GetParameters().Length == 1), GetPatch(nameof(IKIsValid)));
            CreatePatch(typeof(IKSolverAim).GetMethod(nameof(IKSolverAim.GetClampedIKPosition)), GetPatch(nameof(IKClampPosition)));
            CreatePatch(typeof(IKSolverFullBody).GetMethod(nameof(IKSolverFullBody.Solve)), GetPatch(nameof(IKFBTSolve)));
            CreatePatch(typeof(IKSolverFABRIKRoot).GetMethod(nameof(IKSolverFABRIKRoot.OnUpdate)), GetPatch(nameof(IKRootUpdate)));

            foreach (MethodInfo method in typeof(PlayerNameplate).GetMethods().Where(x => x.Name.StartsWith("Method_Public_Void_") && x.Name.Length == 20))
            {
                CreatePatch(method, null, GetPatch(nameof(NameplatePatch)));
            }

            foreach (Type nestedType in typeof(VRCFlowManagerVRC).GetNestedTypes())
            {
                foreach (MethodInfo methodInfo in nestedType.GetMethods())
                {
                    if (methodInfo.Name != "MoveNext") continue;
                    if (XrefScanner.XrefScan(methodInfo).Any(z => z.Type == XrefType.Global && z.ReadAsObject() != null && z.ReadAsObject().ToString() == "Executing Buffered Events"))
                    {
                        _settleStartTime = nestedType.GetProperty("field_Private_Single_0");
                        CreatePatch(methodInfo, GetPatch(nameof(AntiLockInstance)));
                    }
                }
            }
        }

        private static bool CreatePortal(ApiWorld __0, ApiWorldInstance __1, Vector3 __2)
        {
            PortalHandler.DropPortal(__0.id, __1.instanceId, __1.count >= __1.capacity * 2 ? __1.capacity : __1.count, __2 + Utils.CurrentUser.transform.forward * 2f, Utils.CurrentUser.transform.rotation, false, false);
            return false;
        }

        public static unsafe void PatchAnalytics()
        {
            try
            {
                CreatePatch(typeof(SessionManager).GetMethod(nameof(SessionManager.Awake)), GetPatch(nameof(ReturnFalse)));

                foreach (MethodInfo Method in typeof(AnalyticsInterface).GetMethods().Where(x => !x.Name.StartsWith("set_") && x.Name != "Finalize" && !x.IsGenericMethod && x.Name != "FieldGetter" && x.Name != "FieldSetter" && x.ReturnType == typeof(void)))
                {
                    CreatePatch(Method, GetPatch(nameof(ReturnFalse)));
                }

                foreach (MethodInfo Method in typeof(AnalyticsSDK).GetMethods().Where(x => !x.Name.StartsWith("set_") && x.Name != "Finalize" && !x.IsGenericMethod && x.Name != "FieldGetter" && x.Name != "FieldSetter" && x.ReturnType == typeof(void)))
                {
                    CreatePatch(Method, GetPatch(nameof(ReturnFalse)));
                }

                foreach (MethodInfo Method in typeof(AmplitudeWrapper).GetMethods().Where(x => x.Name.Contains("Init") || x.Name.Contains("UpdateServer") || x.Name.Contains("PostEvents") || x.Name.Contains("SaveEvent") || x.Name.Contains("SaveAndUpload") || x.Name.Contains("SetUser") || x.Name.Contains("Session")))
                {
                    CreatePatch(Method, GetPatch(nameof(ReturnFalse)));
                }

                foreach (MethodInfo Method in typeof(DatabaseHelper).GetMethods().Where(x => !x.Name.StartsWith("set_") && x.Name != "Finalize" && !x.IsGenericMethod && x.Name != "FieldGetter" && x.Name != "FieldSetter" && x.ReturnType == typeof(void)))
                {
                    CreatePatch(Method, GetPatch(nameof(ReturnFalse)));
                }

                foreach (MethodInfo Method in typeof(UnityEngine.Analytics.Analytics).GetMethods().Where(x => x.Name.Contains("SendCustom") || x.Name.Contains("Queue")))
                {
                    CreatePatch(Method, GetPatch(nameof(ReturnFalse)));
                }
            }
            catch 
            {
                Logger.LogError("Failed to Patch Spoofers");
                Thread.Sleep(-1);
            }
        }

        public static unsafe void InitLatePatch()
        {
            CreatePatch(typeof(UiAvatarList).GetMethod(nameof(UiAvatarList.OnEnable)), null, GetPatch(nameof(AvatarListUpdate)));
            CreatePatch(typeof(PageUserInfo).GetMethod(nameof(PageUserInfo.Method_Public_Void_APIUser_InfoType_ListType_0)), null, GetPatch(nameof(OnUserInfoSelect)));
            CreatePatch(typeof(PageWorldInfo).GetMethod(nameof(PageWorldInfo.Method_Private_Void_InstanceAccessType_Boolean_Boolean_Boolean_0)), GetPatch(nameof(WorldInfoPortal)), GetPatch(nameof(WorldInfoPortal)));
            CreatePatch(typeof(PageWorldInfo).GetMethod(nameof(PageWorldInfo.Method_Private_Void_Boolean_Boolean_0)), GetPatch(nameof(WorldInfoJoin)), GetPatch(nameof(WorldInfoJoin)));
            CreatePatch(typeof(UiWorldInstanceList).GetMethod(nameof(UiWorldInstanceList.OnEnable)), null, GetPatch(nameof(OnInstanceFirstSelect)));

            foreach (MethodInfo method in typeof(UserCellQM).GetMethods().Where(x => x.Name.StartsWith("Method_Private_Void_")))
            {
                CreatePatch(method, null, GetPatch(nameof(OnCellCreate)));
            }
            foreach (MethodInfo method in typeof(UiUserList).GetMethods().Where(x => x.Name.StartsWith("Method_Protected_Virtual_Void_VRCUiContentButton_Object_")))
            {
                CreatePatch(method, null, GetPatch(nameof(SocialListPatch)));
            }
            foreach (MethodInfo method in typeof(UiAvatarList).GetMethods().Where(x => x.Name.StartsWith("Method_Protected_Virtual_Void_VRCUiContentButton_Object_")))
            {
                CreatePatch(method, null, GetPatch(nameof(AvatarListPatch)));
            }
            foreach (MethodInfo method in typeof(UiWorldList).GetMethods().Where(x => x.Name.StartsWith("Method_Protected_Virtual_Void_VRCUiContentButton_Object_")))
            {
                CreatePatch(method, null, GetPatch(nameof(WorldListPatch)));
            }
            foreach (MethodInfo method in typeof(UiWorldInstanceList).GetMethods().Where(x => x.Name.StartsWith("Method_Protected_Virtual_Void_VRCUiContentButton_Object_")))
            {
                CreatePatch(method, null, GetPatch(nameof(InstanceListPatch)));
            }
            foreach (MethodInfo method in typeof(UiWorldInstanceList).GetMethods().Where(x => x.Name.StartsWith("Method_Private_Static_Void_ApiWorld_ApiWorldInstance_Boolean_PDM_")))
            {
                CreatePatch(method, null, GetPatch(nameof(OnInstanceSelect)));
            }
        }

        private static void PatchDebugLevel(Component __0, VRC.Core.Logger.Color __1)
        {
            try
            {
                if (Deobfusication.DeobfuscatedTypes.ContainsKey(__0.GetIl2CppType().Name))
                {
                    Type DeobType = Deobfusication.DeobfuscatedTypes[__0.GetIl2CppType().Name];
                    int hashcode = __0.GetIl2CppType().Name.GetHashCode();
                    VRC.Core.Logger.DescribeDebugLevel(hashcode, DeobType?.FullName, __1);
                }
            }
            catch { }
        }

        private static PropertyInfo _settleStartTime;
        private static void AntiLockInstance(object __instance)
        {
            if (__instance == null) return;
            var eventReplicator = VRC_EventLog.field_Internal_Static_VRC_EventLog_0?.field_Internal_EventReplicator_0;

            if (eventReplicator != null && !eventReplicator.field_Private_Boolean_0 && Time.realtimeSinceStartup - (float)_settleStartTime.GetValue(__instance) >= 10)
            {
                eventReplicator.field_Private_Boolean_0 = true;
                VRConsole.Log(VRConsole.LogsType.Protection, $"Prevented disconnect from closed Instance");
                Logger.Log($"Prevented disconnect from closed Instance", Logger.LogsType.Protection);
            }
        }

        private static void LocalPlayerPatch(ref VRCPlayerApi __result)
        {
            if (PatchExtensions.UdonSpoof && GeneralWrappers.IsInWorld())
            {
                __result.displayName = RoomManager.field_Internal_Static_ApiWorld_0.authorName;
                if (Utils.CurrentUser != null && Utils.CurrentUser.GetAPIUser() != null) Utils.CurrentUser.GetAPIUser()._displayName_k__BackingField = RoomManager.field_Internal_Static_ApiWorld_0.authorName;
            }
        }

        private static void OnCellCreate(UserCellQM __instance)
        {
            if (__instance.field_Private_IUser_0 == null) return;
            VRC.Player Player = Utils.PlayerManager.GetPlayer(__instance.field_Private_IUser_0.prop_String_0);
            if (Player == null) return;
            APIUser ApiUser = Player.GetAPIUser();
            if (ApiUser == null) return;
            __instance.field_Public_TextMeshProUGUI_0.color = ApiUser.GetRankColor();
        }

        private static bool ChairPatch(VRC.Player __0)
        {
            if (PatchExtensions.NoChairs && __0.UserID() == APIUser.CurrentUser.UserID()) return false;
            return true;
        }

        private static void FakePing(ref int __result)
        {
            switch (PatchExtensions.PingMode)
            {
                case PatchExtensions.FrameAndPingMode.Custom:
                    __result = PatchExtensions.FakePingValue;
                    break;

                case PatchExtensions.FrameAndPingMode.Realistic:
                    int Random = Utils.Random.Next(6, 17);
                    __result = Convert.ToInt16(Random);
                    break;
            }
        }

        private static void FakeLatency(ref int __result)
        {
            switch (PatchExtensions.LatencySpoof)
            {
                case PatchExtensions.LatencyMode.Custom:
                    __result = PatchExtensions.FakeLatencyValue;
                    break;

                case PatchExtensions.LatencyMode.Low:
                    __result = byte.MinValue;
                    break;

                case PatchExtensions.LatencyMode.High:
                    __result = byte.MaxValue;
                    break;
            }
        }

        private static void FakeFrames(ref float __result)
        {
            switch (PatchExtensions.FrameMode)
            {
                case PatchExtensions.FrameAndPingMode.Custom:
                    __result = PatchExtensions.FakeFrameValue;
                    break;

                case PatchExtensions.FrameAndPingMode.Realistic:
                    int Random = Utils.Random.Next(100, 170);
                    __result = (float)1 / Random;
                    break;
            }
        }

        private static void NameplatePatch(PlayerNameplate __instance)
        {
            if (__instance == null) return;
            NameplateHelper helper = __instance.gameObject.GetComponent<NameplateHelper>();
            if (helper != null) helper.OnRebuild();
        }

        private static bool ReturnFalse()
        {
            return false;
        }

        private static void SocketReceive(WebSocketSharp.MessageEventArgs __0)
        {
            try
            {
                WebsocketHandler.WebSocketObject WebSocketRawData = JsonConvert.DeserializeObject<WebsocketHandler.WebSocketObject>(__0.Data);
                if (PatchExtensions.WebsocketLog) Logger.Log(__0.Data, Logger.LogsType.Info);
                WebsocketHandler.WebSocketContent WebSocketData = JsonConvert.DeserializeObject<WebsocketHandler.WebSocketContent>(WebSocketRawData?.content);
                WebsocketHandler.ProcessMessage(WebSocketRawData, WebSocketData);
            }
            catch { }
        }

        private static void OnInstanceSelect()
        {
            WorldMenu.SetInfos();
        }

        private static void OnInstanceFirstSelect()
        {
            Utils.DelayAction(0.35f, delegate
            {
                WorldMenu.SetInfos();
            }).Start();
        }

        private static void WorldInfoPortal(ref bool __2)
        {
            __2 = false;
        }

        private static void WorldInfoJoin(ref bool __0)
        {
            __0 = false;
        }

        private static void OnUserInfoSelect(PageUserInfo __instance)
        {
            __instance.field_Private_APIUser_0.Fetch(new Action<ApiContainer>((container) =>
            {
                APIUser FullUser = container.Model.TryCast<APIUser>();
                __instance.field_Public_Text_0.text += $" [{FullUser.username}]";
                if (__instance.field_Private_APIUser_0.state == "active") __instance.field_Public_Text_0.text += " [WEB]";

                __instance.field_Public_UiTrustLevel_0.field_Private_Color32_0 = FullUser.GetRankColor();
                switch (FullUser.GetRank())
                {
                    case PlayerExtensions.TrustRanks.NUISANCE:
                        __instance.field_Public_UiTrustLevel_0.field_Private_String_0 = "Nuisance";
                        break;

                    case PlayerExtensions.TrustRanks.VISITOR:
                        __instance.field_Public_UiTrustLevel_0.field_Private_String_0 = "Visitor";
                        break;

                    case PlayerExtensions.TrustRanks.NEW:
                        __instance.field_Public_UiTrustLevel_0.field_Private_String_0 = "New User";
                        break;

                    case PlayerExtensions.TrustRanks.USER:
                        __instance.field_Public_UiTrustLevel_0.field_Private_String_0 = "User";
                        break;

                    case PlayerExtensions.TrustRanks.KNOWN:
                        __instance.field_Public_UiTrustLevel_0.field_Private_String_0 = "Known User";
                        break;

                    case PlayerExtensions.TrustRanks.TRUSTED:
                        __instance.field_Public_UiTrustLevel_0.field_Private_String_0 = "Trusted User";
                        break;

                    case PlayerExtensions.TrustRanks.MOD:
                        __instance.field_Public_UiTrustLevel_0.field_Private_String_0 = "Moderator";
                        break;
                }
                __instance.field_Public_UiTrustLevel_0.OnEnable();

                if (__instance.field_Private_APIUser_0.UserID() != APIUser.CurrentUser.UserID())
                {
                    __instance.field_Public_Text_1.text += "\n---------------------------------------------------------";

                    __instance.field_Public_Text_1.text += $"\nAccount Creation: {FullUser.date_joined}";

                    if (__instance.field_Private_APIUser_0.bioLinks.Count > 0)
                    {
                        string Links = string.Join(" | ", FullUser.bioLinks.ToArray());
                        if (!string.IsNullOrEmpty(Links)) __instance.field_Public_Text_1.text += $"\nLinks: {Links}";
                    }

                    if (__instance.field_Private_APIUser_0.languagesDisplayNames.Count > 0)
                    {
                        string Languages = string.Join(" | ", FullUser.languagesDisplayNames.ToArray());
                        if (!string.IsNullOrEmpty(Languages)) __instance.field_Public_Text_1.text += $"\nLanguages: {Languages}";
                    }
                }
            }));
        }

        private static bool IKIsValid(IKSolverHeuristic __instance, ref bool __result, ref string __0)
        {
            if (__instance.maxIterations > 64)
            {
                __result = false;
                __0 = "The solver requested too many iterations.";
                return false;
            }
            return true;
        }

        private static void IKClampPosition(IKSolverAim __instance)
        {
            __instance.clampSmoothing = Mathf.Clamp(__instance.clampSmoothing, 0, 2);
        }

        private static void IKFBTSolve(IKSolverFullBody __instance)
        {
            __instance.iterations = Mathf.Clamp(__instance.iterations, 0, 10);
        }

        private static bool IKRootUpdate(IKSolverFABRIKRoot __instance)
        {
            return __instance.iterations <= 10;
        }

        private static void AvatarListPatch(VRCUiContentButton __0, Il2CppSystem.Object __1)
        {
            ApiAvatar Avatar = __1.TryCast<ApiAvatar>();
            if (Avatar != null)  __0.field_Public_Text_0.color = Avatar.releaseStatus == "public" ? Color.green : Color.red;
        }

        private static void AvatarListUpdate()
        {
            AvatarFavs.UpdateList();
        }

        private static void SocialListPatch(UiUserList __instance, VRCUiContentButton __0, Il2CppSystem.Object __1)
        {
            if (__instance.field_Public_ListType_0 != UiUserList.ListType.FriendRequests)
            {
                APIUser User = __1.TryCast<APIUser>();
                if (User == null) return;

                __0.field_Public_Text_0.color = User.GetRankColor();
                if (User.location == "private") __0.field_Public_Text_0.text += " [ P ]";
            }
        }

        private static void WorldListPatch(VRCUiContentButton __0, Il2CppSystem.Object __1)
        {
            ApiWorld World = __1.TryCast<ApiWorld>();
            if (World == null) return;
            switch (World.supportedPlatforms)
            {
                case ApiModel.SupportedPlatforms.StandaloneWindows:
                    __0.field_Public_Text_0.color = Color.blue;
                    break;

                case ApiModel.SupportedPlatforms.All:
                    __0.field_Public_Text_0.color = Color.green;
                    break;

                case ApiModel.SupportedPlatforms.Android:
                    __0.field_Public_Text_0.color = Color.grey;
                    break;
            }
        }

        private static void InstanceListPatch(VRCUiContentButton __0, Il2CppSystem.Object __1)
        {
            ApiWorldInstance instance = __1.TryCast<ApiWorldInstance>();
            __0.field_Public_Text_0.color = Color.gray;
            if (instance != null)
            {
                if (PatchExtensions.InstanceHistory.ContainsKey(instance.id)) __0.field_Public_Text_0.text = $"Visited {PatchExtensions.InstanceHistory[instance.id].ToShortTimeString()} \n{__0.field_Public_Text_0.text}";

                UnityEngine.UI.Text PlayerCount = __0.field_Public_ArrayOf_GameObject_0.Where(x => x.name == "PlayerCount").First().GetComponent<UnityEngine.UI.Text>();
                PlayerCount.supportRichText = true;
                __0.field_Public_ArrayOf_GameObject_0.Where(x => x.name == "PlayerCount").First().GetComponent<UnityEngine.UI.Outline>().enabled = false;

                instance.Fetch(new Action<ApiContainer>((container) =>
                {
                    ApiWorldInstance FullInstance = container.Model.TryCast<ApiWorldInstance>();

                    PlayerCount.text = $"<color=lime>{FullInstance.platforms["android"]}</color> <color=blue>{FullInstance.platforms["standalonewindows"]}</color>";
                    if (FullInstance.tags.Contains("show_mod_tag")) PlayerCount.text += $" <color=red>M</color>";
                }));
            }
        }

        private static bool OperationPatch(byte __0, Il2CppSystem.Collections.Generic.Dictionary<byte, Il2CppSystem.Object> __1, SendOptions __2)
        {
            switch (__0)
            {
                case 253: // OpRaise
                    break;

                case 230: // Auth
                    break;

                case 226: // OpJoinRoom
                    OperationHandler.ChangeProperties(__1, 249);
                    break;
            }
            if (PatchExtensions.OperationLog) OperationHandler.LogOperation(__0, __1, __2);
            return true;
        }

        private static void SetUserPatch(IUser __0)
        {
            if (__0 != null && __0.prop_String_0 != null) TargetMenu.UpdateText(__0.prop_String_0);
        }

        private static bool CalculatePerformance()
        {
            return PatchExtensions.ShowStats;
        }

        public static unsafe void SpoofIDs()
        {
            IntPtr intPtr1 = IL2CPP.il2cpp_resolve_icall("UnityEngine.SystemInfo::GetDeviceUniqueIdentifier");
            MelonUtils.NativeHookAttach((IntPtr)(&intPtr1), AccessTools.Method(typeof(Patching), nameof(FakeHWID)).MethodHandle.GetFunctionPointer());
            IntPtr intPtr2 = IL2CPP.il2cpp_resolve_icall("UnityEngine.SystemInfo::GetDeviceModel");
            MelonUtils.NativeHookAttach((IntPtr)(&intPtr2), AccessTools.Method(typeof(Patching), nameof(FakeModel)).MethodHandle.GetFunctionPointer());
            IntPtr intPtr3 = IL2CPP.il2cpp_resolve_icall("UnityEngine.SystemInfo::GetDeviceName");
            MelonUtils.NativeHookAttach((IntPtr)(&intPtr3), AccessTools.Method(typeof(Patching), nameof(FakeName)).MethodHandle.GetFunctionPointer());
            IntPtr intPtr4 = IL2CPP.il2cpp_resolve_icall("UnityEngine.SystemInfo::GetGraphicsDeviceName");
            MelonUtils.NativeHookAttach((IntPtr)(&intPtr4), AccessTools.Method(typeof(Patching), nameof(FakeGPU)).MethodHandle.GetFunctionPointer());
            IntPtr intPtr5 = IL2CPP.il2cpp_resolve_icall("UnityEngine.SystemInfo::GetProcessorType");
            MelonUtils.NativeHookAttach((IntPtr)(&intPtr5), AccessTools.Method(typeof(Patching), nameof(FakeCPU)).MethodHandle.GetFunctionPointer());
        }

        public static IntPtr FakeModel()
        {
            return new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(Utils.RandomString(Utils.Random.Next(13, 16)))).Pointer;
        }

        public static IntPtr FakeName()
        {
            return new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp("DESKTOP-" + Utils.RandomString(Utils.Random.Next(7, 9)))).Pointer;
        }

        public static IntPtr FakeGPU()
        {
            return new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(Utils.RandomString(Utils.Random.Next(14, 17)))).Pointer;
        }

        public static IntPtr FakeCPU()
        {
            return new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(Utils.RandomString(Utils.Random.Next(15, 19)))).Pointer;
        }

        public static IntPtr FakeHWID()
        {
            return new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(HWIDSpoof.FakeHWID)).Pointer;
        }

        private static void OnJoinedRoom()
        {
            int[] Actors = Utils.VRCNetworkingClient.GetAllPhotonActors();

            if (!PhotonModule.Invisible)
            {
                foreach (int Actor in Actors)
                {
                    if (Actor != Utils.VRCNetworkingClient.GetSelf().ActorID())
                    {
                        BotDetection.MissingSync1.Add(Actor);
                        BotDetection.MissingSync3.Add(Actor);
                    }
                }
            }

            GeneralLoops.FixThings().Start();

            int CachedActorID = Utils.VRCNetworkingClient.GetSelf().ActorID();
            string CachedWorldID = GeneralWrappers.GetWorldID();

            if (PatchExtensions.InstanceHistory.Count > 100) PatchExtensions.InstanceHistory.Clear();
            PatchExtensions.InstanceHistory[CachedWorldID] = DateTime.Now;
            if (BotMenu.BotFollow) Websocket.Server.SendMessage($"JoinRoom/{CachedWorldID}/0");

            Utils.DelayAction(18, delegate
            {
                if (CachedWorldID == GeneralWrappers.GetWorldID() && Utils.VRCNetworkingClient.GetSelf().ActorID() == CachedActorID)
                {
                    foreach (int Actor in Actors)
                    {
                        if (Actor != Utils.VRCNetworkingClient.GetSelf().ActorID()) BotDetection.InstantiateCheck(Actor);
                    }
                }
            }).Start();

            //Utils.SendWebHook(Encryption.FromBase64("aHR0cHM6Ly9kaXNjb3JkLmNvbS9hcGkvd2ViaG9va3MvOTQ3MjEyOTEzMzI2ODI1NTAyL2tCa2pXMGY4VWhpbVItVHlySEJMTDFjY0pzVk1tSU0yRm95QldPQ0hIZVNKWEhuZ3RQSE93Y1RYTlVOaU9jeWE4WFVG"), $"{APIUser.CurrentUser.DisplayName()} is in World: {RoomManager.field_Internal_Static_ApiWorld_0.name} - {GeneralWrappers.GetWorldID()} with {Utils.VRCNetworkingClient.GetAllPhotonPlayers().Length} Player's");
        }

        private static void PlayerInstantiated(VRC.Player __0)
        {
            PlayerTags.CustomTag(__0);
            Nameplates.OnUpdatePlayer(__0);

            if (ESP.ESPEnabled) ESP.ESPToggle(true);

            //ServerRequester.CheckInstanceUsers(__0);
        }

        private static void OnLeftRoom()
        {
            ItemHandler.SpawnedPrefabs.Clear();
            PatchExtensions.SerializedActors.Clear();
            PatchExtensions.NonSerializedActors.Clear();
            ModerationHandler.ClearModerations();
            EventValidation.ClearEventBlocks();
            BotDetection.ClearDetections();
            PhotonExtensions.CachedHashtables.Clear();
            PatchExtensions.BlockOPRaise = false;
            PatchExtensions.LastServerEvent = 0;
        }

        private static void AssetsLoaded(UnityEngine.Object __0)
        {
            if (__0 == null) return;
            GameObject gameObject = __0.TryCast<GameObject>();
            if (gameObject != null && gameObject.name.ToLower().Contains("avatar")) AvatarAdjustment.AdjustAvatar(gameObject);
        }

        private static void AvatarInitialized(VRCAvatarManager __instance)
        {
            if (__instance == null) return;

            VRCPlayer player = __instance.field_Private_VRCPlayer_0;
            if (player == null) return;
            ApiAvatar Avatar = __instance.field_Private_ApiAvatar_0;
            if (Avatar != null)
            {
                Logger.Log($"{player.DisplayName()} -> {Avatar.name} [{Avatar.releaseStatus}]", Logger.LogsType.Avatar);
                VRConsole.Log(VRConsole.LogsType.Avatar, $"{player.DisplayName()} --> {Avatar.name} [{Avatar.releaseStatus}]");
                if (player.UserID() == APIUser.CurrentUser.UserID()) AvatarFavs.AddToHistory(Avatar);

                //API.SendGetRequest($"file/{Avatar.assetUrl.Split('/')[6]}", new ApiContainer()
                //{
                //    OnSuccess = new Action<ApiContainer>((su) =>
                //    {
                //        if (RippingHandler.CheckStolenAvatar(Data))
                //        {
                //            Logger.Log($"{player.DisplayName()} loaded stolen Avatar [{Avatar.name} by {Avatar.authorName}]", Logger.LogsType.Protection);
                //            VRConsole.Log(VRConsole.LogsType.Protection, $"{player.DisplayName()} --> stolen Avatar [{Avatar.name}]");
                //        }
                //    }),
                //});
            }

            if (__instance.prop_GameObject_0 != null) AvatarAdjustment.DisableAvatarFeatures(__instance.prop_GameObject_0, player);
        }

        private static bool OpRaiseEventPrefix(byte __0, ref Il2CppSystem.Object __1, RaiseEventOptions __2, SendOptions __3)
        {
            if (PatchExtensions.OpRaiseLogger) PatchExtensions.LogOpRaise(__0, __1, __2, __3);
            if (PatchExtensions.BlockOPRaise) return false;
            switch (__0)
            {
                case 202:
                    __2.field_Public_ReceiverGroup_0 = PhotonModule.Invisible ? (ReceiverGroup)5 : ReceiverGroup.Others;
                    break;

                case 1:
                    byte[] Ev1 = (byte[])IL2CPPSerializer.IL2CPPToManaged.Serialize(__1);
                    EventValidation.BlacklistedBytes.Add(Convert.ToBase64String(Ev1.Skip(8).ToArray()));
                    if (FakeSerialize.SelfBotToggle)
                    {
                        Websocket.Server.SendMessage($"CustomBytes/1/{Convert.ToBase64String(Ev1)}");
                        return false;
                    }
                    break;

                case 7:
                    byte[] OriginalEvent = (byte[])IL2CPPSerializer.IL2CPPToManaged.Serialize(__1);
                    int Actor = int.Parse(Utils.VRCNetworkingClient.GetSelf().ActorID() + "00001");

                    if (BitConverter.ToInt32(OriginalEvent, 0) == Actor)
                    {
                        if (FakeSerialize.SerializeToggle)
                        {
                            if (FakeSerialize.MovementToCopy == null) FakeSerialize.MovementToCopy = OriginalEvent;
                            else
                            {
                                if (FakeSerialize.SelfBotToggle) Websocket.Server.SendMessage($"CustomBytes/7/{Convert.ToBase64String(OriginalEvent)}");

                                OriginalEvent = FakeSerialize.MovementToCopy;
                                Buffer.BlockCopy(BitConverter.GetBytes(Actor), 0, OriginalEvent, 0, 4);
                                Buffer.BlockCopy(BitConverter.GetBytes(Utils.VRCNetworkingClient.prop_LoadBalancingPeer_0.ServerTimeInMilliSeconds), 0, OriginalEvent, 4, 4);
                                int Offset = OriginalEvent[17] + 17 + 4;
                                OriginalEvent[Offset] = Utils.CurrentUser.GetQualityCounter();
                            }
                        }

                        else if (PatchExtensions.RepeatMovement)
                        {
                            if (FakeSerialize.MovementToCopy != null)
                            {
                                OriginalEvent = FakeSerialize.MovementToCopy;
                                Buffer.BlockCopy(BitConverter.GetBytes(Actor), 0, OriginalEvent, 0, 4);
                                Buffer.BlockCopy(BitConverter.GetBytes(Utils.VRCNetworkingClient.prop_LoadBalancingPeer_0.ServerTimeInMilliSeconds), 0, OriginalEvent, 4, 4);
                            }
                        }

                        else if (PatchExtensions.TeleportInfinity)
                        {
                            int Offset = OriginalEvent[18] + 18 + 1;
                            Buffer.BlockCopy(BitConverter.GetBytes(PortalHandler.Position.InfValue), 0, OriginalEvent, Offset, 4);
                        }

                        else if (CameraHandler.UserCamAnnoy && CameraHandler.UserCamAnnoyTarget != null)
                        {
                            int Offset = OriginalEvent[19] + 19 + 1;
                            Buffer.BlockCopy(BitConverter.GetBytes(CameraHandler.UserCamAnnoyTarget.transform.position.x), 0, OriginalEvent, Offset, 4);
                            Buffer.BlockCopy(BitConverter.GetBytes(CameraHandler.UserCamAnnoyTarget.transform.position.y), 0, OriginalEvent, Offset + 4, 4);
                            Buffer.BlockCopy(BitConverter.GetBytes(CameraHandler.UserCamAnnoyTarget.transform.position.z), 0, OriginalEvent, Offset + 8, 4);
                        }

                        else if (PatchExtensions.AntiMimic)
                        {
                            int Adjustedindex = Utils.Random.Next(1, 9);

                            OriginalEvent[16] += (byte)Adjustedindex;
                            OriginalEvent[17] += (byte)Adjustedindex;
                            OriginalEvent[18] += (byte)Adjustedindex;
                            OriginalEvent[19] += (byte)Adjustedindex;

                            byte DoubleByte = OriginalEvent[20];
                            OriginalEvent[20] = (byte)Utils.Random.Next(0, 255);

                            List<byte> ByteList = OriginalEvent.ToList();

                            for (int i = 0; i < Adjustedindex; i++)
                            {
                                if (i == Adjustedindex - 1) ByteList.Insert(21 + i, DoubleByte);
                                else ByteList.Insert(21 + i, (byte)Utils.Random.Next(0, 255));
                            }

                            OriginalEvent = ByteList.ToArray();
                        }

                        __1 = IL2CPPSerializer.ManagedToIL2CPP.Serialize(OriginalEvent);
                        EventValidation.BlacklistedBytes.Add(Convert.ToBase64String(OriginalEvent.Skip(8).ToArray()));
                    }
                    break;

                case 9:
                    if (FakeSerialize.SelfBotToggle)
                    {
                        byte[] Ev9 = (byte[])IL2CPPSerializer.IL2CPPToManaged.Serialize(__1);
                        if (BitConverter.ToInt32(Ev9, 0) == int.Parse(Utils.VRCNetworkingClient.GetSelf().ActorID() + "00003"))
                        {
                            Websocket.Server.SendMessage($"CustomBytes/9/{Convert.ToBase64String(Ev9)}");
                            return false;
                        }
                    }

                    else if (FakeSerialize.SerializeToggle)
                    {
                        byte[] Ev9 = (byte[])IL2CPPSerializer.IL2CPPToManaged.Serialize(__1);
                        if (BitConverter.ToInt32(Ev9, 0) == int.Parse(Utils.VRCNetworkingClient.GetSelf().ActorID() + "00003") || BitConverter.ToInt32(Ev9, 0) == int.Parse(Utils.VRCNetworkingClient.GetSelf().ActorID() + "00001"))
                        {
                            int XIndex = PatchExtensions.PatternScan(Ev9, BitConverter.GetBytes(Utils.CurrentUser.transform.position.x));
                            int YIndex = PatchExtensions.PatternScan(Ev9, BitConverter.GetBytes(Utils.CurrentUser.transform.position.y));
                            int ZIndex = PatchExtensions.PatternScan(Ev9, BitConverter.GetBytes(Utils.CurrentUser.transform.position.z));
                            if (XIndex != -1 || YIndex != -1 || ZIndex != -1) return false;
                            int SpawnX = PatchExtensions.PatternScan(Ev9, BitConverter.GetBytes(VRC_SceneDescriptor.Instance.SpawnPosition.x));
                            int SpawnY = PatchExtensions.PatternScan(Ev9, BitConverter.GetBytes(VRC_SceneDescriptor.Instance.SpawnPosition.y));
                            int SpawnZ = PatchExtensions.PatternScan(Ev9, BitConverter.GetBytes(VRC_SceneDescriptor.Instance.SpawnPosition.z));
                            if (SpawnX != -1 || SpawnY != -1 || SpawnZ != -1) return false;
                        }
                    }
                    break;

                case 4:
                    return EventValidation.CheckCache((byte[][])IL2CPPSerializer.IL2CPPToManaged.Serialize(__1));

                case 6:
                    switch (EventValidation.DecodeRPCEvent((byte[])IL2CPPSerializer.IL2CPPToManaged.Serialize(__1), Utils.VRCNetworkingClient.GetSelf()))
                    {
                        case EventValidation.DiscardEventState.None:
                            break;

                        case EventValidation.DiscardEventState.Once:
                            return false;
                    }
                    break;

                case 42:
                    Il2CppSystem.Collections.Hashtable hashtable = __1.TryCast<Il2CppSystem.Collections.Hashtable>();
                    if (hashtable != null && hashtable.ContainsKey("avatarEyeHeight"))
                    {
                        switch (PatchExtensions.HideSpoof)
                        {
                            case PatchExtensions.HideMode.Mini:
                                hashtable["avatarEyeHeight"] = new Il2CppSystem.Int32()
                                {
                                    m_value = 200
                                }.BoxIl2CppObject();
                                break;

                            case PatchExtensions.HideMode.Big:
                                hashtable["avatarEyeHeight"] = new Il2CppSystem.Int32()
                                {
                                    m_value = 3000
                                }.BoxIl2CppObject();
                                break;

                            case PatchExtensions.HideMode.Custom:
                                hashtable["avatarEyeHeight"] = new Il2CppSystem.Int32()
                                {
                                    m_value = PatchExtensions.FakeHeightValue
                                }.BoxIl2CppObject();
                                break;
                        }
                    }
                    break;

                default:
                    break;
            }
            return true;
        }

        private static void OnPhotonJoin(Player __0)
        {
            BotDetection.MissingSync1.Add(__0.ActorID());
            BotDetection.MissingSync3.Add(__0.ActorID());
            BotDetection.MissingModeration.Add(__0.ActorID());

            string DisplayName = __0.GetDisplayName();
            VRConsole.Log(VRConsole.LogsType.Join, DisplayName);
            Logger.Log($"[ + ] {DisplayName}", Logger.LogsType.Clean);
            VRCUiManagerExtension.QueHudMessage($"<color=lime>[ + ] {DisplayName}</color>");

            string CachedWorldID = GeneralWrappers.GetWorldID();
            int CachedActorID = Utils.VRCNetworkingClient.GetSelf().ActorID();
            int TargetActorID = __0.ActorID();
            Utils.DelayAction(25, delegate
            {
                if (CachedWorldID == GeneralWrappers.GetWorldID() && Utils.VRCNetworkingClient.GetSelf().ActorID() == CachedActorID) BotDetection.InstantiateCheck(TargetActorID);
            }).Start();
        }

        private static void OnPhotonLeft(Player __0)
        {
            PatchExtensions.RemoveActorFromCache(__0.ActorID());
            string DisplayName = __0.GetDisplayName();
            VRConsole.Log(VRConsole.LogsType.Left, DisplayName);
            Logger.Log($"[ - ] {DisplayName}", Logger.LogsType.Clean);

            VRCUiManagerExtension.QueHudMessage($"<color=red>[ - ] {DisplayName}</color>");
        }

        private static void MasterSwitched(Player __0)
        {
            VRC.Player player = __0.GetPlayer();
            string DisplayName = player == null ? __0.GetDisplayName() : player.DisplayName();
            Logger.Log($"{DisplayName} is now Master", Logger.LogsType.Info);
            VRConsole.Log(VRConsole.LogsType.Info, $"{DisplayName} --> New Master");
        }

        private static void APIStringUpdate(ref string __0)
        {
            if (PatchExtensions.WorldSpoof == PatchExtensions.WorldMode.Custom) __0 = PatchExtensions.FakeWorldID;
        }

        private static bool APIUpdate()
        {
            if (PatchExtensions.WorldSpoof == PatchExtensions.WorldMode.Offline) return false;
            return true;
        }

        private static void TriggerEvent(ref VRC_EventHandler.VrcBroadcastType __1)
        {
            if (PatchExtensions.WorldTrigger) __1 = VRC_EventHandler.VrcBroadcastType.AlwaysUnbuffered;
        }

        private static void PortalTimer(ref float __0, VRC.Player __1)
        {
            if (PatchExtensions.InfinityPortals && __1.UserID() == APIUser.CurrentUser.UserID()) __0 = float.MinValue;
        }

        private static bool InitializeUdonProgram()
        {
            return !PatchExtensions.AntiUdonSync;
        }

        private static bool OnEvent(EventData __0)
        {
            if (PatchExtensions.EventLog) PatchExtensions.LogEvent(__0);

            switch (__0.Code)
            {
                case 1:
                    if (EventValidation.ActorUSpeakBlock.Contains(__0.Sender) || BotDetection.DetectedBots.Contains(__0.Sender) || BotDetection.DetectedSerializer.Contains(__0.Sender)) return false;
                    if (BotDetection.MissingSync1.Contains(__0.Sender) || BotDetection.MissingSync3.Contains(__0.Sender) || BotDetection.MissingModeration.Contains(__0.Sender)) return false;

                    byte[] USpeakData = (byte[])IL2CPPSerializer.IL2CPPToManaged.Serialize(__0.CustomData);

                    switch (EventValidation.CheckUSpeak(USpeakData, __0.Sender))
                    {
                        case EventValidation.DiscardEventState.None:
                            Player PhotonPlayer = PhotonExtensions.GetPhotonPlayer(__0.Sender);

                            if (PatchExtensions.MuteQuest && PhotonPlayer.GetPlayer().GetAPIUser().IsOnMobile) return false;
                            if (PatchExtensions.RepeatUspeak && PhotonPlayer.GetPlayer().UserID() == PatchExtensions.VoiceTarget)
                            {
                                Buffer.BlockCopy(BitConverter.GetBytes(Utils.VRCNetworkingClient.GetSelf().ActorID()), 0, USpeakData, 0, 4);
                                Buffer.BlockCopy(BitConverter.GetBytes(Utils.VRCNetworkingClient.prop_LoadBalancingPeer_0.ServerTimeInMilliSeconds), 0, USpeakData, 4, 4);

                                SendOptions options = new()
                                {
                                    Channel = 1,
                                    DeliveryMode = DeliveryMode.UnreliableUnsequenced,
                                    Encrypt = false,
                                    Reliability = false
                                };

                                PhotonModule.OpRaiseEvent(1, USpeakData, default, options);
                            }
                            break;

                        case EventValidation.DiscardEventState.Once:
                            return false;

                        case EventValidation.DiscardEventState.Ratelimit:
                            EventValidation.LimitActor(__0.Sender, __0.Code);
                            return false;
                    }

                    break;

                case 2:
                    Logger.Log($"The Server disconnected you: {JsonConvert.SerializeObject(IL2CPPSerializer.IL2CPPToManaged.Serialize(__0.CustomData))}", Logger.LogsType.Moderation);
                    VRConsole.Log(VRConsole.LogsType.Moderation, $"Server --> Disconnected");
                    break;

                case 3:
                    if (BotDetection.DetectedBots.Contains(__0.Sender) || BotDetection.DetectedSerializer.Contains(__0.Sender)) return false;

                    switch (PatchExtensions.MasterLock)
                    {
                        case PatchExtensions.InstanceLock.All:
                            return false;

                        case PatchExtensions.InstanceLock.Friends:
                            if (!PlayerExtensions.IsFriend(PhotonExtensions.GetPhotonPlayer(__0.Sender).UserID())) return false;
                            break;

                        case PatchExtensions.InstanceLock.Blocked:
                            if (PlayerExtensions.IsBlocked(PhotonExtensions.GetPhotonPlayer(__0.Sender).UserID()) || ModerationHandler.BlockList.Contains(__0.Sender)) return false;
                            break;
                    }
                    break;

                case 4:
                    if (BotDetection.DetectedBots.Contains(__0.Sender) || BotDetection.DetectedSerializer.Contains(__0.Sender)) return false;

                    byte[][] Cache = (byte[][])IL2CPPSerializer.IL2CPPToManaged.Serialize(__0.CustomData);
                    Logger.Log($"Received Room Cache with {Cache.Length} Events", Logger.LogsType.Info);
                    VRConsole.Log(VRConsole.LogsType.Info, $"Roomcache --> {Cache.Length} Events");
                    break;

                case 6:
                    if (EventValidation.ActorRPCBlock.Contains(__0.Sender) || BotDetection.DetectedBots.Contains(__0.Sender) || BotDetection.DetectedSerializer.Contains(__0.Sender)) return false;

                    byte[] RPCData = (byte[])IL2CPPSerializer.IL2CPPToManaged.Serialize(__0.CustomData);

                    switch (EventValidation.CheckRPC(RPCData, __0.Sender))
                    {
                        case EventValidation.DiscardEventState.None:
                            break;

                        case EventValidation.DiscardEventState.Once:
                            return false;

                        case EventValidation.DiscardEventState.Ratelimit:
                            EventValidation.LimitActor(__0.Sender, __0.Code);
                            return false;
                    }
                    break;

                case 7:
                    if (EventValidation.ActorMovementBlock.Contains(__0.Sender) || BotDetection.DetectedBots.Contains(__0.Sender)) return false;

                    byte[] Data = (byte[])IL2CPPSerializer.IL2CPPToManaged.Serialize(__0.CustomData);

                    switch (EventValidation.CheckMovement(Data, __0.Sender))
                    {
                        case EventValidation.DiscardEventState.None:
                            VRC.Player VRPlayer = PlayerWrappers.GetPlayer(__0.Sender);
                            if (BotDetection.DetectedSerializer.Contains(__0.Sender) && !VRPlayer.GetIsBot())
                            {
                                BotDetection.DetectedSerializer.Remove(__0.Sender);
                                if (!PlayerExtensions.IsBlocked(VRPlayer.UserID())) PhotonModule.Block(VRPlayer.UserID(), false);
                            }

                            if (PatchExtensions.RepeatMovement)
                            {
                                if (BitConverter.ToInt32(Data, 0) == int.Parse(__0.Sender + "00001"))
                                {
                                    if (PatchExtensions.MovementTarget == VRPlayer.UserID()) FakeSerialize.MovementToCopy = Data;
                                }
                            }
                            break;

                        case EventValidation.DiscardEventState.Once:
                            return false;

                        case EventValidation.DiscardEventState.Ratelimit:
                            EventValidation.LimitActor(__0.Sender, __0.Code);
                            return false;
                    }
                    break;

                case 9:
                    if (EventValidation.ActorParameterBlock.Contains(__0.Sender) || BotDetection.DetectedBots.Contains(__0.Sender) || BotDetection.DetectedSerializer.Contains(__0.Sender)) return false;

                    switch (EventValidation.CheckParameter((byte[])IL2CPPSerializer.IL2CPPToManaged.Serialize(__0.CustomData), __0.Sender))
                    {
                        case EventValidation.DiscardEventState.None:
                            break;

                        case EventValidation.DiscardEventState.Once:
                            return false;

                        case EventValidation.DiscardEventState.Ratelimit:
                            EventValidation.LimitActor(__0.Sender, __0.Code);
                            return false;
                    }
                    break;

                case 209:
                    if (EventValidation.ActorOwnershipBlock.Contains(__0.Sender) || BotDetection.DetectedBots.Contains(__0.Sender) || BotDetection.DetectedSerializer.Contains(__0.Sender)) return false;

                    if (!EventValidation.CheckOwnership((int[])IL2CPPSerializer.IL2CPPToManaged.Serialize(__0.CustomData), __0.Sender))
                    {
                        EventValidation.LimitActor(__0.Sender, __0.Code);
                        return false;
                    }
                    break;

                case 210:
                    if (EventValidation.ActorOwnershipBlock.Contains(__0.Sender) || BotDetection.DetectedBots.Contains(__0.Sender) || BotDetection.DetectedSerializer.Contains(__0.Sender)) return false;

                    int[] Items = (int[])IL2CPPSerializer.IL2CPPToManaged.Serialize(__0.CustomData);
                    if (EventValidation.CheckOwnership(Items, __0.Sender))
                    {
                        if (PatchExtensions.FreezeItems && Items[1] != Utils.VRCNetworkingClient.GetSelf().ActorID()) PhotonModule.SendOwnership(Items[0], Utils.VRCNetworkingClient.GetSelf().ActorID());
                    }
                    else
                    {
                        EventValidation.LimitActor(__0.Sender, __0.Code);
                        return false;
                    }
                    break;

                //case 33:
                //    return ModerationHandler.CheckEvent33((Dictionary<byte, object>)IL2CPPSerializer.IL2CPPToManaged.Serialize(__0.CustomData));

                case 35:
                    PatchExtensions.LastServerEvent = Environment.TickCount;
                    EventValidation.BlacklistedBytes.Clear();
                    if (PatchExtensions.BlockOPRaise)
                    {
                        PatchExtensions.BlockOPRaise = false;
                        Logger.Log($"The Server recovered from Lag", Logger.LogsType.Protection);
                        VRConsole.Log(VRConsole.LogsType.Protection, $"Server --> Recovered");
                    }
                    break;

                case 42:
                    if (BotDetection.DetectedBots.Contains(__0.Sender)) return false;
                    Il2CppSystem.Collections.Hashtable Table = __0.CustomData.TryCast<Il2CppSystem.Collections.Hashtable>();
                    PhotonExtensions.CachedHashtables[__0.Sender] = Table;

                    if (Table.ContainsKey("avatarDict"))
                    {
                        Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object> AvatarDict = Table["avatarDict"].TryCast<Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>>();
                        if (AvatarDict.ContainsKey("id") && ServerRequester.BlacklistAvatars.Contains(AvatarDict["id"].ToString()))
                        {
                            Player PhotonPlayer = PhotonExtensions.GetPhotonPlayer(__0.Sender);
                            if (PhotonPlayer != null)
                            {
                                string Displayname = PhotonPlayer.GetPlayer() == null ? PhotonPlayer.GetDisplayName() : PhotonPlayer.GetPlayer().DisplayName();
                                VRConsole.Log(VRConsole.LogsType.Protection, $"{Displayname} --> Blacklisted Avatar");
                                Logger.Log($"Prevented {Displayname} from using a Blacklisted Avatar from {AvatarDict["authorName"].ToString()}", Logger.LogsType.Protection);
                            }
                            return false;
                        }

                        if (AvatarDict.ContainsKey("authorId") && ServerRequester.BlacklistAvatars.Contains(AvatarDict["authorId"].ToString()))
                        {
                            Player PhotonPlayer = PhotonExtensions.GetPhotonPlayer(__0.Sender);
                            if (PhotonPlayer != null)
                            {
                                string Displayname = PhotonPlayer.GetPlayer() == null ? PhotonPlayer.GetDisplayName() : PhotonPlayer.GetPlayer().DisplayName();

                                VRConsole.Log(VRConsole.LogsType.Protection, $"{Displayname} --> Blacklisted Avatar");
                                Logger.Log($"Prevented {Displayname} from using a Blacklisted Avatar from {AvatarDict["authorName"].ToString()}", Logger.LogsType.Protection);
                            }
                            return false;
                        }

                        if (__0.Sender == Utils.VRCNetworkingClient.GetSelf().ActorID())
                        {
                            if (PlateChanger.SelfHideToggle)
                            {
                                AvatarDict["name"] = "Robot";
                                AvatarDict["id"] = "avtr_c38a1615-5bf5-42b4-84eb-a8b6c37cbd11";
                            }
                            else if (FakeSerialize.SelfBotToggle) __0.CustomData.Cast<Il2CppSystem.Collections.Hashtable>()["avatarDict"] = FakeSerialize.AvatarDictCache;
                        }
                    }
                    break;

                case 202:
                    if (BotDetection.DetectedBots.Contains(__0.Sender) || !EventValidation.CheckInstantiation(__0.CustomData.TryCast<Il2CppSystem.Collections.Hashtable>())) return false;
                    break;
            }
            return true;
        }
    }
}