using LunaR.Api;
using LunaR.ConsoleUtils;
using LunaR.Modules;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LunaR.ButtonUI
{
    internal class QMRedesign
    {
        public static TextMeshProUGUI LowerPannel;

        public static void CreateMainQM()
        {
            RemoveVRCPlusAds();
            ChangeHeaderBar();
            ChangeMainDesign();
            ChangeMainButtons();
            VRConsole.LoadConsoleSprite().Start();
        }

        public static void ChangeHeaderBar()
        {
            GameObject Window = APIStuff.GetQuickMenuInstance().field_Public_GameObject_0;
            Transform Panel = Window.transform.Find("QMNotificationsArea/DebugInfoPanel/Panel/");
            var BackGround = Panel.Find("Background");
            var BackGroundRect = BackGround.GetComponent<RectTransform>();
            BackGroundRect.sizeDelta = new Vector2(620, 0);
            BackGroundRect.anchoredPosition = new Vector2(310, 0);
            var Ping = Panel.Find("Text_Ping");

            Transform Panel1 = Object.Instantiate(Ping, Panel);
            Panel1.name = $"LowerPanel";
            Panel1.GetComponent<RectTransform>().localPosition += new Vector3(200, 0, 0);
            Panel1.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 0);
            LowerPannel = Panel1.GetComponent<TextMeshProUGUI>();
            LowerPannel.alignment = TextAlignmentOptions.Left;

            GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/ThankYouCharacter").transform.localScale = new Vector3(0, 0, 0);
            GameObject Text = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/Header_H1/LeftItemContainer/Text_Title");
            Text.GetComponent<TextMeshProUGUI>().text = "死 ＬＵＮＡＲ 愛";
            Text.GetComponent<TextMeshProUGUI>().color = Color.blue;
            Text.transform.localPosition = new Vector3(225, -15, 0);
        }

        private static void RemoveVRCPlusAds()
        {
            GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Carousel_Banners").SetActive(false);
            GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/VRC+_Banners").transform.localScale = Vector3.zero;
            GameObject.Find("UserInterface/MenuContent/Screens/Avatar/Vertical Scroll View/Viewport/FavoriteListTemplate/GetMoreFavorites/MoreFavoritesButton").transform.localScale = Vector3.zero;
            GameObject.Find("UserInterface/MenuContent/Screens/Avatar/Vertical Scroll View/Viewport/FavoriteListTemplate/GetMoreFavorites/MoreFavoritesText").transform.localScale = Vector3.zero;
            GameObject.Find("UserInterface/MenuContent/Backdrop/Header/Tabs/ViewPort/Content/VRC+PageTab").transform.localScale = Vector3.zero;
            GameObject.Find("UserInterface/MenuContent/Screens/UserInfo/Buttons/RightSideButtons/RightUpperButtonColumn/GiftVRChatPlusButton").transform.localScale = Vector3.zero;
            GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_SelectedUser_Remote/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UserActions/Button_GiftVRChatPlus").transform.localScale = Vector3.zero;
            GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_SelectedUser_Local/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UserActions/Button_GiftVRCPlus").transform.localScale = Vector3.zero;
            GameObject.Find("UserInterface/MenuContent/Screens/UserInfo/Buttons/RightSideButtons/RightUpperButtonColumn/Supporter").transform.localScale = Vector3.zero;
            GameObject.Find("UserInterface/MenuContent/Screens/Avatar/Favorite Button/Icon_VRC+").transform.localScale = Vector3.zero;
        }

        public static void ChangeMainDesign()
        {
            GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/Header_H1/LeftItemContainer").GetComponent<HorizontalLayoutGroup>().enabled = false;
            GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup").GetComponent<VerticalLayoutGroup>().enabled = false;
            GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMNotificationsArea/DebugInfoPanel/Panel/Background").SetActive(false);
            GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect").GetComponent<ScrollRect>().enabled = false;
            GameObject QMBackground = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/BackgroundLayer01");
            QMBackground.GetComponent<Image>().color = QMBackground.GetComponent<Image>().color / 1.018f;
        }

        public static void ChangeMainButtons()
        {
            GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Header_QuickLinks").SetActive(false);

            GameObject QuickLinkButtons = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickLinks");
            QuickLinkButtons.transform.localPosition = new Vector3(0, -110, 0);
            QuickLinkButtons.GetComponent<GridLayoutGroup>().cellSize = new Vector2(200, 86);

            foreach (Transform Object in QuickLinkButtons.GetComponentsInChildren<Transform>())
            {
                if (Object.name.Contains("Button_"))
                {
                    Transform Icon = Object.gameObject.transform.Find("Icon");
                    if (Icon != null) Icon.localScale = Vector3.zero;
                    Transform IconON = Object.gameObject.transform.Find("Icon_On");
                    if (IconON != null) IconON.localScale = Vector3.zero;
                    Transform IconOFF = Object.gameObject.transform.Find("Icon_Off");
                    if (IconOFF != null) IconOFF.localScale = Vector3.zero;
                    Object.gameObject.transform.Find("Badge_MMJump").transform.localScale = Vector3.zero;
                    Object.gameObject.transform.Find("Text_H4").GetComponent<TextMeshProUGUI>().transform.localScale += new Vector3(0.15f, 0.16f, 0.15f);
                    Object.gameObject.transform.Find("Text_H4").GetComponent<TextMeshProUGUI>().transform.localPosition += new Vector3(0, 5f, 0);
                }
            }

            GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Header_QuickActions").SetActive(false);

            GameObject QuickActionButtons = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions");
            QuickActionButtons.transform.localPosition = new Vector3(0, -830, 0);
            QuickActionButtons.GetComponent<GridLayoutGroup>().cellSize = new Vector2(200, 86);

            foreach (Transform Object in QuickActionButtons.GetComponentsInChildren<Transform>())
            {
                if (Object.name.Contains("Button_"))
                {
                    Transform Icon = Object.gameObject.transform.Find("Icon");
                    if (Icon != null) Icon.localScale = Vector3.zero;
                    Transform IconON = Object.gameObject.transform.Find("Icon_On");
                    if (IconON != null) IconON.localScale = Vector3.zero;
                    Transform IconOFF = Object.gameObject.transform.Find("Icon_Off");
                    if (IconOFF != null) IconOFF.localScale = Vector3.zero;
                    Object.gameObject.transform.Find("Text_H4").GetComponent<TextMeshProUGUI>().transform.localScale += new Vector3(0.15f, 0.16f, 0.15f);
                    Object.gameObject.transform.Find("Text_H4").GetComponent<TextMeshProUGUI>().transform.localPosition += new Vector3(0, 5f, 0);
                }
            }            

            GameObject VRButtons = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/SitStandCalibrateButton");
            VRButtons.GetComponent<GridLayoutGroup>().cellSize = new Vector2(200, 86);

            foreach (Transform Object in VRButtons.GetComponentsInChildren<Transform>(true))
            {
                Transform Icon = Object.gameObject.transform.Find("Icon");
                if (Icon != null) Icon.localScale = Vector3.zero;
                Transform IconON = Object.gameObject.transform.Find("Icon_On");
                if (IconON != null) IconON.localScale = Vector3.zero;
                Transform IconOFF = Object.gameObject.transform.Find("Icon_Off");
                if (IconOFF != null) IconOFF.localScale = Vector3.zero;
            }
        }
    }
}