//using Hexed.UIApi;
//using Hexed.Wrappers;
//using Il2CppInterop.Runtime.InteropTypes.Arrays;
//using UnityEngine;
//using VRC.Core;
//using VRC.UI.Elements.Controls;
//using VRC.UI.Elements.Menus;

//namespace Hexed.CustomUI.MainMenu
//{
//    internal class WorldMenu
//    {
//        private static Transform WorldPage;
//        private static Transform ActionMenu;

//        public static void Init()
//        {
//            WorldPage = MenuHelper.MainMenu.transform.Find("Container/MMParent/Menu_WorldDetail/ScrollRect/Viewport/VerticalLayoutGroup/Actions");

//            Transform PlayerTextPanel = UnityEngine.Object.Instantiate(MenuHelper.MainMenu.transform.Find("Container/MMParent/Menu_WorldDetail/ScrollRect/Viewport/VerticalLayoutGroup/Header_Actions"), MenuHelper.MainMenu.transform.Find("Container/MMParent/Menu_WorldDetail/ScrollRect/Viewport/VerticalLayoutGroup"));
//            PlayerTextPanel.Find("LeftItemContainer/Text_Title").GetComponent<TextMeshProUGUIEx>().text = "Players";

//            ActionMenu = UnityEngine.Object.Instantiate(MenuHelper.MainMenu.transform.Find("Container/MMParent/Menu_WorldDetail/ScrollRect/Viewport/VerticalLayoutGroup/Actions"), MenuHelper.MainMenu.transform.Find("Container/MMParent/Menu_WorldDetail/ScrollRect/Viewport/VerticalLayoutGroup"));
//            ActionMenu.DestroyChildren();

//            new MMSingleButton(WorldPage, 0, 0, "Copy ID", delegate
//            {
//                IWorld SelectedWorld = MenuHelper.MainMenu.transform.Find("Container/MMParent/Menu_WorldDetail").GetComponent<MainMenuWorldDetailsPage>().prop_IWorld_0;
//                if (SelectedWorld == null) return;
//                GeneralUtils.CopyToClipboard(SelectedWorld.prop_String_0);

//            }, "Copy the WorldID", ButtonAPI.MMButtonType.Big, UnityUtils.GetSprite("History"));

//            new MMSingleButton(WorldPage, 0, 0, "Download File", delegate
//            {
//                IWorld SelectedWorld = MenuHelper.MainMenu.transform.Find("Container/MMParent/Menu_WorldDetail").GetComponent<MainMenuWorldDetailsPage>().prop_IWorld_0;

//                Object1Public1ObInObIL1BoLiObApUnique FullIWorld = SelectedWorld.TryCast<Object1Public1ObInObIL1BoLiObApUnique>();
//                if (FullIWorld == null || FullIWorld.prop_TYPE_0 == null) return;

//                ApiFile.DownloadFile(FullIWorld.prop_TYPE_0.world.assetUrl, new Action<Il2CppStructArray<byte>>((worldFile) =>
//                {
//                    File.WriteAllBytes(Core.ConfigManager.BaseFolder + $"\\Rips\\{FullIWorld.prop_TYPE_0.world.name}.vrcw", worldFile);
//                    Wrappers.Logger.Log($"Downloaded World {FullIWorld.prop_TYPE_0.world.name}", Wrappers.Logger.LogsType.Info);
//                }), new Action<string>((error) =>
//                {
//                    Wrappers.Logger.LogError($"Failed to download World");
//                }), null);
//            }, "Download the World file", ButtonAPI.MMButtonType.Big, UnityUtils.GetSprite("Download"));
//        }
//    }
//}
