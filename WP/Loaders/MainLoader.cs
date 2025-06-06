using MelonLoader;
using System.Diagnostics;
using System;
using WengaPort.Loaders;
using WengaPort.Buttons;
using WengaPort.Modules;
using UnityEngine;
using WengaPort.Wrappers;
using WengaPort.Extensions;
using VRC;
using VRCSDK2;
using System.Linq;
using DiscordRichPresence;
using UnityEngine.UI;
using WengaPort.ConsoleUtils;
using UnhollowerRuntimeLib;
using UnityEngine.Rendering.PostProcessing;

namespace WengaPort.MainLoader
{
    public class MainLoader : MelonMod
    {
        public static string Version = "2.0.0";
        public unsafe override void OnApplicationStart() // Runs after Game Initialization.
        {
            FoldersManager.Create.Initialize();
            OnStart();
            Console.Title = "WengaPort Loading...";
            PatchManager.QuestIni();
            PatchManager.PatchSafety();
            ClassInjector.RegisterTypeInIl2Cpp<Modules.Reupload.ReuploaderButtons>();
            ClassInjector.RegisterTypeInIl2Cpp<Modules.Reupload.ApiFileHelper>();
            ClassInjector.RegisterTypeInIl2Cpp<Movement>();
            ClassInjector.RegisterTypeInIl2Cpp<LovenseRemote>();
            ClassInjector.RegisterTypeInIl2Cpp<PlayerList>();
            ClassInjector.RegisterTypeInIl2Cpp<ThirdPerson>();
            ClassInjector.RegisterTypeInIl2Cpp<AvatarFavs>();
            ClassInjector.RegisterTypeInIl2Cpp<AntiMenuOverrender>();
            ClassInjector.RegisterTypeInIl2Cpp<AttachmentManager>();
            ClassInjector.RegisterTypeInIl2Cpp<NameplateHelper>();
            ClassInjector.RegisterTypeInIl2Cpp<ESP>();
            ClassInjector.RegisterTypeInIl2Cpp<AvatarHider>();
            ClassInjector.RegisterTypeInIl2Cpp<KeyBindHandler>();
            ClassInjector.RegisterTypeInIl2Cpp<GlobalDynamicBones>();
            ClassInjector.RegisterTypeInIl2Cpp<CanHearList>();
            ClassInjector.RegisterTypeInIl2Cpp<OnGui>();
            ClassInjector.RegisterTypeInIl2Cpp<Api.ApiExtension>();
            ClassInjector.RegisterTypeInIl2Cpp<DiscordManager>();
        }

        public override void OnLevelWasLoaded(int level) // Runs when a Scene has Loaded.
        {
            if (level == -1) GameObject.Find("UserInterface/MenuContent/Screens/Settings/OtherOptionsPanel/AllowAvatarCopyingToggle").GetComponent<UiSettingConfig>().SetEnable(false);
            VRConsole.AllLogsText.Clear();
            GlobalDynamicBones.currentWorldDynamicBoneColliders.Clear();
            GlobalDynamicBones.currentWorldDynamicBones.Clear();
            PlayerList.BlockList.Clear();
            MelonCoroutines.Start(PatchManager.EventDelayer());
        }

        public override void OnLevelWasInitialized(int level) // Runs when a Scene has Initialized.
        {
            switch (level)
            {
                case 0:
                case 1:
                    break;
                default:
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    break;
            }

            RoomTime = 0f;
            ItemHandler.World_Pickups.Clear();
            ItemHandler.World_ObjectSyncs.Clear();
            ItemHandler.World_Triggers.Clear();
            ItemHandler.World_Mirrors.Clear();
            ItemHandler.PostProcess.Clear();
            ItemHandler.Pedestals.Clear();
            ItemHandler.World_Panorama.Clear();

            ItemHandler.World_ObjectSyncs = Resources.FindObjectsOfTypeAll<VRC_ObjectSync>().ToArray().ToList();
            ItemHandler.World_Triggers = UnityEngine.Object.FindObjectsOfType<VRC_Trigger>().ToArray().ToList();
            
            if (ItemHandler.MirrorToggle)
            {
                foreach (var item in Resources.FindObjectsOfTypeAll<VRC.SDKBase.VRC_MirrorReflection>())
                {
                    if (item.gameObject.active)
                    {
                        item.gameObject.SetActive(false);
                        ItemHandler.World_Mirrors.Add(item);
                    }
                }
            }
            if (ItemHandler.ItemToggle)
            {
                foreach (VRC_Pickup item in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                {
                    item.gameObject.SetActive(false);
                    ItemHandler.World_Pickups.Add(item);
                }
            }
            if (ItemHandler.PickupToggle)
            {
                foreach (VRC_Pickup item in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                {
                    if (item.pickupable)
                    {
                        item.pickupable = false;
                        ItemHandler.World_Pickups.Add(item);
                    }
                }
            }

            if (ItemHandler.PostProcessToggle)
            {
                foreach (var volume in Resources.FindObjectsOfTypeAll<PostProcessVolume>())
                {
                    if (volume.isActiveAndEnabled)
                    {
                        volume.enabled = false;
                        ItemHandler.PostProcess.Add(volume);
                    }
                }
            }

            if (ItemHandler.PedestalToggle)
            {
                foreach (VRC_AvatarPedestal vrc_AvatarPedestal in Resources.FindObjectsOfTypeAll<VRC_AvatarPedestal>())
                {
                    if (vrc_AvatarPedestal.enabled)
                    {
                        vrc_AvatarPedestal.enabled = false;
                        ItemHandler.Pedestals.Add(vrc_AvatarPedestal);
                    }
                }
            }

            if (ItemHandler.PanoramaToggle)
            {
                foreach (VRC.SDKBase.VRC_Panorama panorama in Resources.FindObjectsOfTypeAll<VRC.SDKBase.VRC_Panorama>())
                {
                    if (panorama.enabled)
                    {
                        ItemHandler.World_Panorama.Add(panorama);
                        panorama.enabled = false;
                    }
                }
            }

            if (ItemHandler.AutoHoldToggle)
            {
                foreach (var item in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                {
                    if (item.gameObject.active && item.pickupable)
                    {
                        item.AutoHold = (VRC.SDKBase.VRC_Pickup.AutoHoldMode)1;
                        ItemHandler.World_Pickups.Add(item);
                    }
                }
            }
        }

        public override void OnUpdate() // Runs once per frame.
        {
            if (VRCPlayer.field_Internal_Static_VRCPlayer_0 != true) ButtonsMainColor.Initialize3(); //Really scuffed but this is to disable the blue loading screen
            if (RoomManager.field_Internal_Static_ApiWorld_0 != null)
            {
                try
                {
                    RoomTime += Time.deltaTime;
                    inviteDot.GetComponent<Image>().color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time / 2f, 1f), 1f, 1f));
                    reqDot.GetComponent<Image>().color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time / 2f, 1f), 1f, 1f));

                    if (menu != null && menu.active && !button.interactable)
                    {
                        button.interactable = true;
                        button.m_Interactable = true;
                    }
                    Delay += Time.deltaTime;
                    if (Delay > 4f)
                    {
                        if (PortalHandler.kosstrings.Count > 0)
                        {
                            foreach (Player player in PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.ToArray())
                            {
                                if (PortalHandler.kosstrings.Contains(player.UserID()))
                                {
                                    PortalHandler.DropInfinitePortal(player.GetVRCPlayer());
                                }
                            }
                        }
                        Delay = 0f;
                    }
                }
                catch { }
            }
        }
        public float Delay = 0f;
        public float PlateDelay = 0f;
        public static float RoomTime = 0f;

        public override void OnFixedUpdate() // Can run multiple times per frame. Mostly used for Physics.
        {

        }

        public virtual void OnStart()
        {

        } 

        public override void OnLateUpdate() // Runs once per frame after OnUpdate and OnFixedUpdate have finished.
        {

        }

        public override void OnGUI()
        {
            
        }

        public override void OnApplicationQuit() // Runs when the Game is told to Close.
        {
            PhotonMenu.Ini.SetBool("Toggles", "Exit", true);
            Api.ApiExtension.CloseWebsocket();
            DiscordManager.OnApplicationQuit();
            Process.GetCurrentProcess().Kill();
        }

        public override void OnModSettingsApplied() // Runs when Mod Preferences get saved to UserData/modprefs.ini.
        {

        }

        public override void VRChat_OnUiManagerInit() // Runs upon VRChat's UiManager Initialization. Only runs if the Mod is used in VRChat.
        {
            ButtonsLoader.Initialize();
            ModulesLoader.Initialize();
            PatchManager.InitPatch();
            VRCPlayer.field_Internal_Static_Color_0 = new Color(255, 0, 0);
            VRCPlayer.field_Internal_Static_Color_1 = new Color(255, 255, 0);
            VRCPlayer.field_Internal_Static_Color_2 = new Color(0, 0, 0);
            VRCPlayer.field_Internal_Static_Color_3 = new Color(0, 255, 255);
            VRCPlayer.field_Internal_Static_Color_4 = new Color(0, 255, 0);
            VRCPlayer.field_Internal_Static_Color_5 = new Color(0.92f, 0.37f, 0f);
            VRCPlayer.field_Internal_Static_Color_6 = new Color(222, 0, 128);
            VRCPlayer.field_Internal_Static_Color_7 = new Color(184, 86, 0);
            VRCPlayer.field_Internal_Static_Color_8 = new Color(255, 255, 255);
            VRCPlayer.field_Internal_Static_Color_9 = new Color(255, 255, 255);
            menu = GameObject.Find("/UserInterface/MenuContent/Screens/UserInfo/User Panel/OnlineFriend");
            button = GameObject.Find("/UserInterface/MenuContent/Screens/UserInfo/User Panel/OnlineFriend/Actions/Invite").GetComponent<Button>();
            inviteDot = GameObject.Find("/UserInterface/UnscaledUI/HudContent/Hud/NotificationDotParent/InviteDot");
            reqDot = GameObject.Find("/UserInterface/UnscaledUI/HudContent/Hud/NotificationDotParent/InviteRequestDot");
            var Client = new GameObject();
            UnityEngine.Object.DontDestroyOnLoad(Client);
            Client.AddComponent<Movement>();
            Client.AddComponent<LovenseRemote>();
            Client.AddComponent<PlayerList>();
            Client.AddComponent<CanHearList>();
            Client.AddComponent<ThirdPerson>();
            Client.AddComponent<AvatarFavs>();
            Client.AddComponent<AttachmentManager>();
            Client.AddComponent<NameplateHelper>();
            Client.AddComponent<AntiMenuOverrender>();
            Client.AddComponent<ESP>();
            Client.AddComponent<AvatarHider>();
            Client.AddComponent<KeyBindHandler>();
            Client.AddComponent<GlobalDynamicBones>();
            Client.AddComponent<OnGui>();
            Client.AddComponent<Modules.Reupload.ReuploaderButtons>();
            Client.AddComponent<Modules.Reupload.ApiFileHelper>();
            Client.AddComponent<Api.ApiExtension>();
            Client.AddComponent<DiscordManager>();
        }
        GameObject menu;
        GameObject inviteDot;
        GameObject reqDot;
        Button button;
    }
}