using LunaR.Api;
using LunaR.Buttons;
using LunaR.ButtonUI;
using LunaR.Extensions;
using LunaR.Modules;
using LunaR.Wrappers;
using System;
using System.Collections;
using UnityEngine;

namespace LunaR.Loaders
{
    internal class ButtonsLoader
    {
        public static void Initialize()
        {
            UIChanges.CreatePasteButton();
            UIChanges.CreateRestartButton();
            UIChanges.ChangeLoadingPicture();
            UIChanges.FixButtons();
            UIChanges.ChangeClipping();
            SettingsPage.Init();
            WorldMenu.SetupWorldPage();
            ConfigManager.SetupPageInfo();
            SocialMenu.Initialize();
            UIChanges.ChangeVRLaser();
            SafetyMenu.InitExtraLevels();
            OnQuickMenuCoro(delegate
            {
                if (GeneralWrappers.IsInVr()) APIStuff.GetQuickMenuInstance().transform.localScale = new Vector3(1.165f, 1.165f, 1.165f);
                else APIStuff.GetQuickMenuInstance().transform.localScale = new Vector3(1.04f, 1.04f, 1.04f);
                QMButtons.MainMenu.Init();
                QMButtons.SafetyMenu.Init();
                QMButtons.UtilsMenu.Init();
                QMButtons.MovementMenu.Init();
                QMButtons.ExploitMenu.Init();
                QMButtons.DebugMenu.Init();
                QMButtons.BotMenu.Init();
                QMButtons.ERPMenu.Init();
                QMButtons.TargetMenu.Init();
                QMButtons.DynamicMenu.Init();
                PlayerList.Init();
                QMRedesign.CreateMainQM();
            }, delegate
            {
                QMButtons.MainMenu.InitWings();
            }).Start();
        }

        private static IEnumerator OnQuickMenuCoro(Action Qm, Action Wing)
        {
            while (APIStuff.GetQuickMenuInstance() == null) yield return new WaitForEndOfFrame();
            Qm();
            while (APIStuff.GetLeftWing().field_Private_MenuStateController_0 == null || APIStuff.GetRightWing().field_Private_MenuStateController_0 == null) yield return new WaitForEndOfFrame();
            Wing();
        }
    }
}