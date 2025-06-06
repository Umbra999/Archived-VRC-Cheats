using LunaR.Modules;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LunaR.Wrappers;

namespace LunaR.Extensions
{
    public static class VRCUiManagerExtension
    {

        public static void AskInGameInput(this VRCUiPopupManager instance, string title, string okButtonName, Action<string> onSuccess, string def = null)
        {
            instance.InputPopUp(title, okButtonName, new Action<string>((g) =>
            {
                onSuccess(g);
            }), def);
        }

        public static void QueHudMessage(string Message)
        {
            if (StaticHud == null) StaticHud = new("", 54, 0, TextAnchor.LowerLeft);
            ShowMessage(StaticHud.text, MessagesList, Message).Start();
        }

        public static System.Collections.IEnumerator ShowMessage(Text text, List<string> MessagesList, string message)
        {
            MessagesList.Add(message);
            text.text = string.Join("\n", MessagesList);
            yield return new WaitForSeconds(5.5f);
            MessagesList.Remove(message);
            text.text = string.Join("\n", MessagesList);
        }

        private static readonly List<string> MessagesList = new();
        private static UIHudText StaticHud;

        private static void InputPopUp(this VRCUiPopupManager instance, string title, string okButtonName, Action<string> onSuccess, string def = null)
        {
            Utils.CurrentUser.GetVRCPlayerApi().Immobilize(true);
            instance.Method_Public_Void_String_String_InputType_Boolean_String_Action_3_String_List_1_KeyCode_Text_Action_String_Boolean_Action_1_VRCUiPopup_Boolean_Int32_0(title, "", InputField.InputType.Standard, false, okButtonName, new Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, Text>((g, l, t) =>
            {
                if (string.Empty == g) g = def;
                onSuccess(g);
                Utils.CurrentUser.GetVRCPlayerApi().Immobilize(false);
                instance.HideCurrentPopUp();
            }), new Action(() =>
            {
                Utils.CurrentUser.GetVRCPlayerApi().Immobilize(false);
                instance.HideCurrentPopUp();
            }), def ?? "");
        }

        public static void CloseUI(this VRCUiManager Instance, bool withFade = false)
        {
            Instance.Method_Public_Void_Boolean_0(withFade);
        }

        public static void HideCurrentPopUp(this VRCUiPopupManager instance)
        {
            Utils.VRCUiManager.HideScreen("POPUP");
        }

        //public static void HudNotification(string Text, Sprite Image)
        //{
        //    MonoBehaviourPublicObnoObmousStObBoHuCaUnique.field_Public_Static_MonoBehaviourPublicObnoObmousStObBoHuCaUnique_0.userEventCarousel.Method_Private_Void_String_Sprite_0(Text, Image);
        //}

        public static void AlertNotification(this VRCUiPopupManager instance, string title, string Content, string buttonname, Action action)
        {
            instance.Method_Public_Void_String_String_String_Action_Action_1_VRCUiPopup_0(title, Content, buttonname, action, null);
        }

        public static void Alert(this VRCUiPopupManager instance, string title, string Content, string buttonname, Action action)
        {
            instance.Method_Public_Void_String_String_String_Action_String_Action_Action_1_VRCUiPopup_0(title, Content, buttonname, action, "Cancel", new Action(() =>
            {
                instance.HideCurrentPopUp();
            }), null);
        }

        public static void NumberInput(this VRCUiPopupManager instance, string title, string preFilledText, InputField.InputType inputType, bool useNumericKeypad, string submitButtonText, Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, Text> submitButtonAction, string placeholderText = "Enter text....", bool hidePopupOnSubmit = true, Action<VRCUiPopup> additionalSetup = null)
        {
            instance.Method_Public_Void_String_String_InputType_Boolean_String_Action_3_String_List_1_KeyCode_Text_Action_String_Boolean_Action_1_VRCUiPopup_Boolean_Int32_0(title, preFilledText, inputType, useNumericKeypad, submitButtonText, submitButtonAction, new Action(() =>
            {
                instance.HideCurrentPopUp();
            }), placeholderText, hidePopupOnSubmit, additionalSetup);
        }

        public class UIHudText
        {
            public UIHudText(string Text, float rightOffset, float upOffset, TextAnchor textAnchor)
            {
                text = CreateTextNear(GameObject.Find("UserInterface/UnscaledUI/HudContent_Old/Hud/NotificationDotParent").transform, Text, upOffset - 20, rightOffset - 55, textAnchor);

                // For new HUD
                //text = CreateTextNear(GameObject.Find("UserInterface/UnscaledUI/HudContent/HUD_UI 2/VR Canvas/Container/Left/Icons").transform, Text, upOffset, rightOffset, textAnchor);
            }

            public Text text;
            public GameObject indicator;

            public void SetPosition(Vector3 xyz)
            {
                indicator.transform.localPosition = xyz;
            }

            public void SetPosition(Vector2 xyz)
            {
                indicator.transform.localPosition = xyz;
            }

            private Text CreateTextNear(Transform transform, string Text, float upoffset, float rightoffset, TextAnchor alignment)
            {

                GameObject gameObject = new("LunarHud");
                gameObject.AddComponent<Text>();
                gameObject.transform.SetParent(transform, false);
                gameObject.transform.localScale = Vector3.one;
                gameObject.transform.localPosition = Vector3.up * upoffset + Vector3.right * rightoffset;
                Text text = gameObject.GetComponent<Text>();
                text.text = Text;
                text.color = Color.white;
                text.fontStyle = FontStyle.Bold;
                text.horizontalOverflow = HorizontalWrapMode.Overflow;
                text.verticalOverflow = VerticalWrapMode.Overflow;
                text.alignment = alignment;
                text.fontSize = 28;
                text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text.supportRichText = true;
                gameObject.SetActive(true);
                return text;

                // For new HUD

                //GameObject gameObject = new("LunarHud");
                //gameObject.AddComponent<Text>();
                //gameObject.transform.SetParent(transform, false);
                //gameObject.transform.localScale = Vector3.one;
                //gameObject.transform.localPosition = Vector3.up * upoffset + Vector3.right * rightoffset;
                //Text text = gameObject.GetComponent<Text>();
                //text.text = Text;
                //text.color = Color.white;
                //text.fontStyle = FontStyle.Bold;
                //text.horizontalOverflow = HorizontalWrapMode.Overflow;
                //text.verticalOverflow = VerticalWrapMode.Overflow;
                //text.alignment = alignment;
                //text.fontSize = 20;
                //text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                //text.supportRichText = true;
                //gameObject.SetActive(true);
                //return text;
            }
        }
    }
}