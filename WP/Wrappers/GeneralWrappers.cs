using System;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using VRC;
using VRC.Core;
using WengaPort.Modules;

namespace WengaPort.Wrappers
{
    public static class GeneralWrappers
    {
        public static PlayerManager GetPlayerManager() { return PlayerManager.field_Private_Static_PlayerManager_0; }

        public static QuickMenu GetQuickMenu() { return QuickMenu.prop_QuickMenu_0; }

        public static VRCUiManager GetVRCUiPageManager() { return VRCUiManager.prop_VRCUiManager_0; }

        public static UserInteractMenu GetUserInteractMenu() { return Resources.FindObjectsOfTypeAll<UserInteractMenu>()[0]; }

        public static GameObject GetPlayerCamera() { return GameObject.Find("Camera (eye)"); }

        public static VRCVrCamera GetVRCVrCamera() { return VRCVrCamera.field_Private_Static_VRCVrCamera_0; }

        public static string GetRoomId() { return APIUser.CurrentUser.location; }

        public static VRCUiManager GetVRCUiManager() { return VRCUiManager.prop_VRCUiManager_0; }

        public static HighlightsFX GetHighlightsFX() { return HighlightsFX.prop_HighlightsFX_0; }

        public static void EnableOutline(this HighlightsFX instance, Renderer renderer, bool state) => instance.Method_Public_Void_Renderer_Boolean_0(renderer, state); //First method to take renderer, bool parameters

        public static VRCUiPopupManager GetVRCUiPopupManager() { return VRCUiPopupManager.prop_VRCUiPopupManager_0; }

        public static void AlertPopup(this VRCUiPopupManager manager, string title, string text) => manager.Method_Public_Void_String_String_Single_0(title, text, 10f);

        public static void SelectPlayer(this QuickMenu instance, Player player) => instance.Method_Public_Void_Player_0(player);

        public static void PopupYesNoSquare(string title, string Content, string buttonname, Action action, string button2, Action action2) => VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.Method_Public_Void_String_String_String_Action_String_Action_Action_1_VRCUiPopup_0(title, Content, buttonname, action, button2, action2, null);

        public static bool IsInVr() { return XRDevice.isPresent; }

        public static short GetPing(this Player instance)
        {
            return instance.prop_PlayerNet_0.field_Private_Int16_0;
        }

        public static short GetPing(this VRCPlayer instance)
        {
            return instance.prop_PlayerNet_0.field_Private_Int16_0;
        }

        public static string GetPingColored(this VRCPlayer Instance)
        {
            string arg;
            if (Instance.GetPing() <= 75)
            {
                arg = "<color=#59D365>";
            }
            else if(Instance.GetPing() <= 130)
            {
                arg = "<color=#FF7000>";
            }
            else
            {
                arg = "<color=red>";
            }
            return string.Format("{0}{1}</color>", arg, Instance.GetPing());
        }

        public static string GetPingColored(this Player Instance)
        {
            string arg;
            if (Instance.GetPing() <= 75)
            {
                arg = "<color=#59D365>";
            }
            else if(Instance.GetPing() <= 130)
            {
                arg = "<color=#FF7000>";
            }
            else
            {
                arg = "<color=red>";
            }
            return string.Format("{0}{1}</color>", arg, Instance.GetPing());
        }

        public static string GetFramesColored(this VRCPlayer Instance)
        {
            string arg;
            if (Instance.GetFrames() >= 80)
            {
                arg = "<color=#59D365>";
            }
            else if(Instance.GetFrames() >= 30)
            {
                arg = "<color=#FF7000>";
            }
            else
            {
                arg = "<color=red>";
            }
            return string.Format("{0}{1}</color>", arg, Instance.GetFrames());
        }

        public static string GetFramesColored(this Player Instance)
        {
            string arg;
            if (Instance.GetFrames() >= 80)
            {
                arg = "<color=#59D365>";
            }
            else if (Instance.GetFrames() >= 30)
            {
                arg = "<color=#FF7000>";
            }
            else
            {
                arg = "<color=red>";
            }
            return string.Format("{0}{1}</color>", arg, Instance.GetFrames());
        }

        public static int GetFrames(this VRCPlayer Instance)
        {
            return (Instance.prop_PlayerNet_0.prop_Byte_0 != 0) ? ((int)(1000f / (float)Instance.prop_PlayerNet_0.prop_Byte_0)) : 0;
        }

        public static int GetFrames(this Player Instance)
        {
            return (Instance.prop_PlayerNet_0.prop_Byte_0 != 0) ? ((int)(1000f / (float)Instance.prop_PlayerNet_0.prop_Byte_0)) : 0;
        }

        public static bool GetIsBot(this VRCPlayer Instance)
        {
            return Instance.GetPing() == 0 && Instance.transform.position.x == 0 && Instance.GetFrames() == 0 && Instance.UserID() != APIUser.CurrentUser.id;
        }
        public static GameObject GetAvatarObject(this Player p) => p.prop_VRCPlayer_0.prop_VRCAvatarManager_0.prop_GameObject_0;

        public static bool GetIsBot(this Player Instance)
        {
            return Instance.GetPing() == 0 && Instance.transform.position.x == 0 && Instance.GetFrames() == 0 && Instance.UserID() != APIUser.CurrentUser.id;
        }

        public static void ShowInputKeyBoard(Il2CppSystem.Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, Text> InputAction)
        {
            VRCUiPopupManager vrcpopup = GetVRCUiPopupManager();
            vrcpopup.field_Public_VRCUiPopupInput_1.gameObject.SetActive(true);
            vrcpopup.field_Public_VRCUiPopupInput_1.Method_Public_Void_String_InputType_String_Action_3_String_List_1_KeyCode_Text_Boolean_PDM_0("Enter Input", InputField.InputType.Standard, "Enter text", InputAction, true);
            GameObject.Find("UserInterface/MenuContent/Popups/InputKeypadPopup").SetActive(true);
        }

        public static void ClosePopup()
        {
            try
            {
                VRCUiPopupManager vrcpopup = GetVRCUiPopupManager();
                vrcpopup.Method_Private_Void_PDM_0();
            }
            catch { }
        }

        public static void SetToolTipBasedOnToggle(this UiTooltip tooltip)
        {
            UiToggleButton componentInChildren = tooltip.gameObject.GetComponentInChildren<UiToggleButton>();

            if (componentInChildren != null && !string.IsNullOrEmpty(tooltip.field_Public_Text_1.text))
            {
                string displayText = (!componentInChildren.field_Public_Boolean_0) ? tooltip.field_Public_Text_1.text : tooltip.field_Public_Text_0.text;
                if (TooltipManager.field_Private_Static_Text_0 != null) //Only return type field of text
                {
                    TooltipManager.Method_Public_Static_Void_String_1(displayText); //Last function to take string parameter
                }
                else if (tooltip != null) tooltip.field_Public_Text_0.text = displayText;
            }
        }

        public static string convert(WebResponse res)
        {
            string strResponse = "";
            using (var stream = res.GetResponseStream())
            using (var reader = new StreamReader(stream)) strResponse = reader.ReadToEnd();
            res.Dispose();
            return strResponse;
        }
    }
}
