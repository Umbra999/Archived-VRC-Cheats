using LunaR.Buttons.Bots;
using LunaR.Extensions;
using LunaR.Modules;
using LunaR.Wrappers;
using MelonLoader;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace LunaR.Loaders
{
    internal class LoaderClasses
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleOutputCP(uint wCodePageID);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCP(uint wCodePageID);

        [DllImport("user32.dll", EntryPoint = "SetWindowText")]
        public static extern bool SetWindowText(IntPtr hwnd, string lpString);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string className, string windowName);

        public static void OnStart()
        {
            if (!Console.Title.Contains("LunaRLoader") || !File.Exists("LunaR\\Key.LunaR")) ServerRequester.Kill();
            HWIDSpoof.Init();
            Extensions.Patching.PatchAnalytics();
            Extensions.Patching.InitEarlyPatch();
            FolderManager.Initialize();
            UIInit().Start();
            Console.Title = $"LunaR by Umbra | CONSOLE | {Utils.RandomString(20)}";
            IntPtr VRChat = FindWindow(null, "VRChat");
            SetWindowText(VRChat, $"Lunar by Umbra | VRCHAT | {Utils.RandomString(20)}");
            ClassInjector.RegisterTypeInIl2Cpp<EnableDisableListener>();
            ClassInjector.RegisterTypeInIl2Cpp<Movement>();
            ClassInjector.RegisterTypeInIl2Cpp<PlayerList>();
            if (!GeneralWrappers.IsInVr()) ClassInjector.RegisterTypeInIl2Cpp<ThirdPerson>();
            ClassInjector.RegisterTypeInIl2Cpp<NameplateHelper>();
            ClassInjector.RegisterTypeInIl2Cpp<ESP>();
            ClassInjector.RegisterTypeInIl2Cpp<PlateChanger>();
            ClassInjector.RegisterTypeInIl2Cpp<Lovense>();
            if (!GeneralWrappers.IsInVr()) ClassInjector.RegisterTypeInIl2Cpp<KeyBindHandler>();
            if (!GeneralWrappers.IsInVr()) ClassInjector.RegisterTypeInIl2Cpp<OnGui>();
            ClassInjector.RegisterTypeInIl2Cpp<GeneralLoops>();
            ServerRequester.Init();
            Deobfusication.Load();
            StyleAPI.StylesLoader.WaitForStyleInit().Start();
            SetConsoleOutputCP(65001);
            SetConsoleCP(65001);
            Console.OutputEncoding = Encoding.GetEncoding(65001);
            Console.InputEncoding = Encoding.GetEncoding(65001);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(@"
                                                      .....                                        
                                                         .';:;'                                     
                                                            .:do:.                                  
                                                              'cxkl.                                
                                                               .:oOO:                               
                                                                .:ok0o.                             
                                                                 'coOKo.                            
                                                                 .;cd0Kc                            
          :xc.          .ld:.    .cx:     .cxdl.    .cd:.       .ok00OKk.     ;ddddddddo:.          
         .kM0,          ,KMk.    '0Mx.    'OWWW0:.  .OMk.      .xWNNWNXK:     oMNkcccclKWx.         
         .kM0,          ,KMk.    '0Mx.    '0MKxKNkl''OMk.     .dWXooXWWXc     dMNOooooxKNo.         
         .kM0,          '0MO.    ;KMx.    '0Mk.'kNMXkKMk.    .oNM0odKWMWc     oMNkcoKWWO;           
         .xMNxllllll'    cKNOl:cl0N0;     '0Mk. .:d0WWMk.    lNW0dxO0XWWx.    dMX:  .dXXx'          
          ;ooooooooo'     .:oxxxdl;.      .:o:.    .coo:    .coc. ':oOX0o.    ,ol.    'lo;.         
                                                                 .;cx00:                            
                                                                 'cdO0l                             
                                                                .:oOOc                              
                                                               .:dOx,                               
                                                             .,lxxc.                                
                                                            'lol;.                                  
                                                         .';;,.                                     
                                                        ...                                         
      _
     |u|
   __|m|__
   \+-b-+/       Beautiful girls, dead feelings...
    ~|r|~        one day the sun is gonna explode and all this was for nothing.
     |a|
      \|
");
        }

        public static void OnExit()
        {
            Websocket.Server.SendMessage($"Shutdown");
            MelonPreferences.Save();
            if (File.Exists($"{Path.GetTempPath()}\\VRChat\\VRChat\\amplitude.cache")) File.Delete($"{Path.GetTempPath()}\\VRChat\\VRChat\\amplitude.cache");
            Process.GetCurrentProcess().Kill();
        }

        public static void LevelLoaded(int level)
        {
            switch (level)
            {
                case 0:
                    break;

                case 1:
                    break;
            }
        }

        public static void LevelInit(int level)
        {
            switch (level)
            {
                case 0:
                    break;

                case 1:
                    break;

                default:
                    break;
            }
        }

        public static void OnUpdate()
        {
        }

        public static IEnumerator UIInit()
        {
            while (VRCUiManager.prop_VRCUiManager_0 == null) yield return null;
            Extensions.Patching.InitLatePatch();
            ButtonsLoader.Initialize();
            ModulesLoader.Initialize();
            GameObject LunaR = new()
            {
                name = "LunaR",
            };
            UnityEngine.Object.DontDestroyOnLoad(LunaR);
            LunaR.AddComponent<Movement>();
            LunaR.AddComponent<PlayerList>();
            if (!GeneralWrappers.IsInVr()) LunaR.AddComponent<ThirdPerson>();
            LunaR.AddComponent<ESP>();
            LunaR.AddComponent<PlateChanger>();
            LunaR.AddComponent<Lovense>();
            if (!GeneralWrappers.IsInVr()) LunaR.AddComponent<KeyBindHandler>();
            if (!GeneralWrappers.IsInVr()) LunaR.AddComponent<OnGui>();
            LunaR.AddComponent<GeneralLoops>();
        }
    }
}