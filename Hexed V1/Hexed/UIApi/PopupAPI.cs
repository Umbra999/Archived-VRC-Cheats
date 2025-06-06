using Hexed.Wrappers;
using TMPro;
using UnityEngine;
using VRC.Localization;
using VRC.UI.Client.HUD;
using VRC.UI.Elements.Controls;

namespace Hexed.UIApi
{
    internal static class PopupAPI
    {
        private static Hud_UserEventCarousel _activeCarousel;
        private static Hud_UserEventCarousel activeCarousel
        {
            get
            {
                if (_activeCarousel == null) _activeCarousel = Resources.FindObjectsOfTypeAll<Hud_UserEventCarousel>()?[0];
                return _activeCarousel;
            }
        }

        public static void HudMessage(string Text, Sprite icon = null)
        {
            if (activeCarousel == null) return;

            LocalizableString localizableString = LocalizableStringExtensions.Localize(Text);

            activeCarousel.Method_Private_Void_LocalizableString_Sprite_0(localizableString, icon);
        }

        public static void HudToast(string content, string description = null, Sprite icon = null, float duration = 5f)
        {
            LocalizableString message = LocalizableStringExtensions.Localize(content);
            LocalizableString desc = LocalizableStringExtensions.Localize(description);

            VRCUiManager.field_Private_Static_VRCUiManager_0.field_Private_HudController_0.notification.Method_Public_Void_Sprite_LocalizableString_LocalizableString_Single_Object1PublicTYBoTYUnique_1_Boolean_0(icon, message, desc, duration);
        }

        public static void AskInGameInput(this VRCUiPopupManager instance, string title, string okButtonName, Action<string> onSuccess, string placeholder = null)
        {
            instance.InputPopUp(title, okButtonName, new Action<string>((g) =>
            {
                onSuccess(g);
            }), placeholder);
        }

        private static void InputPopUp(this VRCUiPopupManager instance, string title, string okButtonName, Action<string> onSuccess, string placeholder = null)
        {
            LocalizableString titleString = LocalizableStringExtensions.Localize(title);
            LocalizableString emptyString = LocalizableStringExtensions.Localize("");
            LocalizableString okString = LocalizableStringExtensions.Localize(okButtonName);
            LocalizableString placeholderString = LocalizableStringExtensions.Localize(placeholder);

            instance.Method_Public_Void_LocalizableString_LocalizableString_InputType_Boolean_LocalizableString_Action_3_String_List_1_KeyCode_TextMeshProUGUIEx_Action_LocalizableString_Boolean_Action_1_VRCUiPopup_Boolean_Int32_0(titleString, emptyString, TMP_InputField.InputType.Standard, false, okString, new Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, TextMeshProUGUIEx>((g, l, t) =>
            {
                if (string.Empty == g) g = placeholder;
                onSuccess(g);
            }), new Action(() =>
            {
                GameHelper.VRCUiManager.HideScreen("POPUP");
            }), placeholderString ?? emptyString);
        }

        public static void AlertNotification(this VRCUiPopupManager instance, string title, string Content, string okButtonName)
        {
            LocalizableString titleString = LocalizableStringExtensions.Localize(title);
            LocalizableString contentString = LocalizableStringExtensions.Localize(Content);
            LocalizableString okString = LocalizableStringExtensions.Localize(okButtonName);

            instance.Method_Public_Void_LocalizableString_LocalizableString_LocalizableString_Action_Action_1_VRCUiPopup_0(titleString, contentString, okString, new Action(() =>
            {
                GameHelper.VRCUiManager.HideScreen("POPUP");
            }), null);
        }

        public static void ConfirmNotification(this VRCUiPopupManager instance, string title, string Content, string okButtonName, Action onSuccess)
        {
            LocalizableString titleString = LocalizableStringExtensions.Localize(title);
            LocalizableString contentString = LocalizableStringExtensions.Localize(Content);
            LocalizableString okString = LocalizableStringExtensions.Localize(okButtonName);
            LocalizableString cancelString = LocalizableStringExtensions.Localize(okButtonName);

            instance.Method_Public_Void_LocalizableString_LocalizableString_LocalizableString_Action_LocalizableString_Action_Action_1_VRCUiPopup_0(titleString, contentString, okString, new Action(() =>
            {
                onSuccess();
                GameHelper.VRCUiManager.HideScreen("POPUP");
            }), cancelString, new Action(() =>
            {
                GameHelper.VRCUiManager.HideScreen("POPUP");
            }));
        }
    }
}
