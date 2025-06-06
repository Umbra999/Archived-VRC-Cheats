using System;
using System.Linq;
using System.Reflection;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;
using VRC.Core;

namespace LunaR.Modules
{
    internal class RememberMe
    {
        private static MethodInfo ValidateTextTargetMethod;
        private static readonly string UserKey = "CachedUser";
        private static readonly string PassKey = "CachedPass";

        public static void Init()
        {
            ValidateTextTargetMethod = typeof(InputFieldValidator).GetMethods(BindingFlags.Instance | BindingFlags.Public).Single(it => it.GetParameters().Length == 1 && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "^([\\w\\.\\-\\+]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$"));
            LoadPlayerPrefs();
            VRCUiManager uiManager = GetVRCUiManager();
            uiManager.field_Private_Action_1_VRCUiPage_0 = (uiManager.field_Private_Action_1_VRCUiPage_0 == null) ? new Action<VRCUiPage>(OnPageShown) : Il2CppSystem.Delegate.Combine(uiManager.field_Private_Action_1_VRCUiPage_0, (UnityEngine.Events.UnityAction<VRCUiPage>)new Action<VRCUiPage>(OnPageShown)).Cast<Il2CppSystem.Action<VRCUiPage>>();
        }

        private static void OnPageShown(VRCUiPage page)
        {
            if (page == null) return;
            else if (page.name.Equals("StoreLoginPrompt") || page.name.Equals("LoginUserPass") || page.name.Equals("Login2FA")) LoadPlayerPrefs();
            else if (page.name.Equals("StandardPopup"))
            {
                VRCUiPopupStandard popupStandard = page.TryCast<VRCUiPopupStandard>();
                if (popupStandard != null && popupStandard.field_Public_Text_0.text.Equals("LOGIN")) SavePlayerPrefs();
            }
        }

        private static void LoadPlayerPrefs()
        {
            VRCUiPageAuthentication authPage = GetAuthPage();
            if (authPage == null) return;
            if (SecurePlayerPrefs.HasKey(UserKey))
            {
                authPage.field_Public_UiInputField_0.field_Private_String_0 = SecurePlayerPrefs.GetString(UserKey, ApiCredentials.SECURE_PLAYER_PREFS_PW);
                authPage.field_Public_UiInputField_0.prop_String_0 = authPage.field_Public_UiInputField_0.field_Private_String_0;
                ValidateTextTargetMethod.Invoke(authPage.field_Public_UiInputField_0.GetComponent<InputFieldValidator>(), new object[] { authPage.field_Public_UiInputField_0.field_Private_String_0 });
            }
            if (SecurePlayerPrefs.HasKey(PassKey))
            {
                authPage.field_Public_UiInputField_1.field_Private_String_0 = SecurePlayerPrefs.GetString(PassKey, ApiCredentials.SECURE_PLAYER_PREFS_PW);
                authPage.field_Public_UiInputField_1.prop_String_0 = authPage.field_Public_UiInputField_1.field_Private_String_0;
                ValidateTextTargetMethod.Invoke(authPage.field_Public_UiInputField_1.GetComponent<InputFieldValidator>(), new object[] { authPage.field_Public_UiInputField_1.field_Private_String_0 });
            }
        }

        private static void SavePlayerPrefs()
        {
            VRCUiPageAuthentication authPage = GetAuthPage();
            if (authPage == null) return;
            if (!string.IsNullOrEmpty(authPage.field_Public_UiInputField_0.prop_String_0)) SecurePlayerPrefs.SetString(UserKey, authPage.field_Public_UiInputField_0.prop_String_0, ApiCredentials.SECURE_PLAYER_PREFS_PW);
            if (!string.IsNullOrEmpty(authPage.field_Public_UiInputField_1.prop_String_0)) SecurePlayerPrefs.SetString(PassKey, authPage.field_Public_UiInputField_1.prop_String_0, ApiCredentials.SECURE_PLAYER_PREFS_PW);
        }

        private static VRCUiPageAuthentication GetAuthPage()
        {
            return Resources.FindObjectsOfTypeAll<VRCUiPageAuthentication>().First((p) => p.gameObject.name == "LoginUserPass");
        }

        private static VRCUiManager GetVRCUiManager()
        {
            return VRCUiManager.field_Private_Static_VRCUiManager_0;
        }
    }
}