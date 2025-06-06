using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Hexed.UIApi;
using Hexed.Wrappers;
using VRC.UI;
using VRC.UI.Client.HUD;

namespace Hexed.CustomUI
{
    internal class UIQuickChanges
    {
        public static void ApplyLateChanges()
        {
            ChangeHeader();
            RemoveAds();
            ColorActionMenu();
            ChangeDebugPanel();
            ChangeMainButtons();
            CreateConsole();
        }

        public static void ApplyEarlyChanges()
        {
            RemoveLoadingBackground();
        }

        private static void ChangeHeader()
        {
            Transform TitleText = MenuHelper.menuPageTemplate.transform.Find("Header_H1/LeftItemContainer/Text_Title");
            if (TitleText != null) TitleText.GetComponent<TextMeshProUGUI>().text = "                  愛 H E X E D 毒";
            else Wrappers.Logger.LogError("Failed to find Text_Title");

            //Transform WelcomeText = QuickMenuHelper.MainMenu.transform.Find("Container/MMParent/Menu_Dashboard/Header_MM_H1/HeaderBackground/Header Text");
            //if (WelcomeText != null) WelcomeText.GetComponent<TextMeshProUGUI>().text = "愛 H E X E D 毒";
            //else Wrappers.Logger.LogError("Failed to find Header Text");
        }

        public static TextMeshProUGUI CustomDebugPanel;
        private static void ChangeDebugPanel()
        {
            Transform DebugBackground = MenuHelper.QuickMenu.transform.Find("CanvasGroup/Container/Window/Panel_QM_Widget/Panel_QM_DebugInfo/Panel/Background");
            if (DebugBackground != null)
            {
                DebugBackground.localPosition = Vector3.zero;
                DebugBackground.gameObject.SetActive(false);
            }
            else Wrappers.Logger.LogError("Failed to find DebugInfoPanelBackground");

            Transform DebugPanel = MenuHelper.QuickMenu.transform.Find("CanvasGroup/Container/Window/Panel_QM_Widget/Panel_QM_DebugInfo/Panel/Text_Ping");
            if (DebugPanel != null)
            {
                Transform NewDebug = UnityEngine.Object.Instantiate(DebugPanel, MenuHelper.QuickMenu.transform.Find("CanvasGroup/Container/Window/Panel_QM_Widget/Panel_QM_DebugInfo/Panel"));
                NewDebug.name = "HexedDebugPanel";
                NewDebug.GetComponent<RectTransform>().localPosition += new Vector3(200, 0, 0);
                NewDebug.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 0);

                CustomDebugPanel = NewDebug.GetComponent<TextMeshProUGUI>();
            }
            else Wrappers.Logger.LogError("Failed to find DebugPanel");

        }

        private static void RemoveAds()
        {
            Transform PanelInfo = MenuHelper.QuickMenu.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_Camera/Scrollrect/Viewport/VerticalLayoutGroup/Buttons (1)/Panel_Info");
            if (PanelInfo != null)
            {
                PanelInfo.localScale = Vector3.zero;
                PanelInfo.gameObject.SetActive(false);
            }
            else Wrappers.Logger.LogError("Failed to find PanelInfo");

            Transform invertedSphere = MenuHelper.userInterface.Find("PlayerDisplay/BlackFade/inverted_sphere");
            if (invertedSphere != null)
            {
                invertedSphere.localScale = Vector3.zero;
                invertedSphere.gameObject.SetActive(false);
            }
            else Wrappers.Logger.LogError("Failed to find inverted_sphere");

            Transform ThankYouCat = MenuHelper.QuickMenu.transform.Find("CanvasGroup/Container/ThankYouCharacter/Character");
            if (ThankYouCat != null)
            {
                ThankYouCat.localScale = Vector3.zero;
                ThankYouCat.gameObject.SetActive(false);
            }
            else Wrappers.Logger.LogError("Failed to find Character");

            Transform ThankYouBubble = MenuHelper.QuickMenu.transform.Find("CanvasGroup/Container/ThankYouCharacter/Dialog Bubble");
            if (ThankYouBubble != null)
            {
                ThankYouBubble.localScale = Vector3.zero;
                ThankYouBubble.gameObject.SetActive(false);
            }
            else Wrappers.Logger.LogError("Failed to find Dialog Buble");

            Transform VRCPlusBanner = MenuHelper.menuPageTemplate.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup/VRC+_Banners");
            if (VRCPlusBanner != null)
            {
                VRCPlusBanner.localScale = Vector3.zero;
                VRCPlusBanner.gameObject.SetActive(false);
            }
            else Wrappers.Logger.LogError("Failed to find VRC+_Banners");

            Transform CarouselBanner = MenuHelper.menuPageTemplate.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup/Carousel_Banners");
            if (CarouselBanner != null)
            {
                CarouselBanner.localScale = Vector3.zero;
                CarouselBanner.gameObject.SetActive(false);
            }
            else Wrappers.Logger.LogError("Failed to find Carousel_Banners");

            Transform MainCarouselBanner = MenuHelper.MainMenu.transform.Find("Container/MMParent/Menu_Dashboard/ScrollRect_MM/Viewport/Content/Panel/Carousel_Banners");
            if (MainCarouselBanner != null)
            {
                MainCarouselBanner.localScale = Vector3.zero;
                MainCarouselBanner.gameObject.SetActive(false);
            }
            else Wrappers.Logger.LogError("Failed to find Main Carousel_Banners");

            Transform ProfileCredits = MenuHelper.MainMenu.transform.Find("Container/MMParent/Menu_MM_Profile/ScrollRect/Viewport/VerticalLayoutGroup/Row1/Profile/DetailsArea/UserIconAndCredits/Panel_MM_CreditsButton");
            if (ProfileCredits != null)
            {
                ProfileCredits.localScale = Vector3.zero;
                ProfileCredits.gameObject.SetActive(false);
            }
            else Wrappers.Logger.LogError("Failed to find Main Panel_MM_CreditsButton");

            Transform AvatarBannerUpsell = MenuHelper.MainMenu.transform.Find("Container/MMParent/Menu_Avatars/Menu_MM_DynamicSidePanel/Panel_SectionList/ScrollRect_Navigation_Container/ScrollRect_Content/Viewport/VerticalLayoutGroup/VRC+ Upsell");
            if (AvatarBannerUpsell != null)
            {
                AvatarBannerUpsell.localScale = Vector3.zero;
                AvatarBannerUpsell.gameObject.SetActive(false);
            }
            else Wrappers.Logger.LogError("Failed to find VRC+ Upsell");

            Transform InvMenuMessaging = MenuHelper.MainMenu.transform.Find("Container/MMParent/Modal_MM_InviteResponse/MenuPanel/ScrollRect/Viewport/VerticalLayoutGroup/Panel_AddPhotoPrompt/Photo_VRCPlus_Message");
            if (InvMenuMessaging != null)
            {
                InvMenuMessaging.localScale = Vector3.zero;
                InvMenuMessaging.gameObject.SetActive(false);
            }
            else Wrappers.Logger.LogError("Failed to find Photo_VRCPlus_Message");

            Transform WalletButton = MenuHelper.QuickMenu.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_QM_SocialIdentity/Panel_MM_Wallet");
            if (WalletButton != null)
            {
                WalletButton.localScale = Vector3.zero;
                WalletButton.gameObject.SetActive(false);
            }
            else Wrappers.Logger.LogError("Failed to find Panel_MM_Wallet");

            Transform WalletContents = MenuHelper.MainMenu.transform.Find("Container/Panel_MM_Wallet/Cell_Wallet_Contents");
            if (WalletContents != null)
            {
                WalletContents.localScale = Vector3.zero;
                WalletContents.gameObject.SetActive(false);
            }
            else Wrappers.Logger.LogError("Failed to find Cell_Wallet_Contents");

            //Transform QMVRCPlusPage = MenuHelper.QuickMenu.transform.Find("CanvasGroup/Container/Window/Page_Buttons_QM/HorizontalLayoutGroup/Page_VRCPlus");
            //if (QMVRCPlusPage != null)
            //{
            //    QMVRCPlusPage.localScale = Vector3.zero;
            //    QMVRCPlusPage.gameObject.SetActive(false);
            //}
            //else Wrappers.Logger.LogError("Failed to find Page_VRCPlus");
        }

        private static void ChangeMainButtons()
        {
            MenuHelper.menuPageTemplate.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup").GetComponent<VerticalLayoutGroup>().enabled = false;
            MenuHelper.menuPageTemplate.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup/Header_QuickLinks").gameObject.SetActive(false);
            MenuHelper.menuPageTemplate.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup/Header_QuickActions").gameObject.SetActive(false);

            Transform UpperQuickLinks = MenuHelper.menuPageTemplate.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickLinks");
            UpperQuickLinks.transform.localPosition = new Vector3(0, -110, 0);
            UpperQuickLinks.GetComponent<GridLayoutGroup>().cellSize = new Vector2(200, 86);

            foreach (Transform Object in UpperQuickLinks.GetComponentsInChildren<Transform>())
            {
                if (Object.name.Contains("Button_"))
                {
                    Transform Icons = Object.transform.Find("Icons");
                    if (Icons != null) Icons.gameObject.SetActive(false);

                    Transform Badge = Object.transform.Find("Badge_MMJump");
                    if (Badge != null) Badge.gameObject.SetActive(false);
                }
            }

            Transform LowerQuickLinks = MenuHelper.menuPageTemplate.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions");
            LowerQuickLinks.transform.localPosition = new Vector3(0, -860, 0);
            LowerQuickLinks.GetComponent<GridLayoutGroup>().cellSize = new Vector2(200, 86);

            foreach (Transform Object in LowerQuickLinks.GetComponentsInChildren<Transform>())
            {
                if (Object.name.Contains("Button_"))
                {
                    Transform Icons = Object.transform.Find("Icons");
                    if (Icons != null) Icons.gameObject.SetActive(false);

                    Transform Badge = Object.transform.Find("Badge_MMJump");
                    if (Badge != null) Badge.gameObject.SetActive(false);
                }
            }

            Transform VRQuickLinks = MenuHelper.menuPageTemplate.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/SitStandCalibrateButton");
            VRQuickLinks.GetComponent<GridLayoutGroup>().cellSize = new Vector2(200, 86);

            foreach (Transform Object in VRQuickLinks.GetComponentsInChildren<Transform>(true))
            {
                Transform Icons = Object.transform.Find("Icons");
                if (Icons != null) Icons.gameObject.SetActive(false);

                Transform Badge = Object.transform.Find("Badge_MMJump");
                if (Badge != null) Badge.gameObject.SetActive(false);
            }
        }

        private static void ColorActionMenu()
        {
            foreach (PedalGraphic grph in Resources.FindObjectsOfTypeAll<PedalGraphic>())
            {
                if (grph._texture != null)
                {
                    grph._texture = grph._texture.ToTexture2D().Desaturate();
                    grph.color = new Color(0.35f, 0.35f, 0.35f);
                }
            }
            foreach (ActionMenu menu in Resources.FindObjectsOfTypeAll<ActionMenu>())
            {
                Image baseImage = menu.transform.Find("Main/Cursor").GetComponentInChildren<Image>();
                if (baseImage == null) return;

                baseImage.sprite = baseImage.sprite.ReplaceTexture(baseImage.sprite.UnpackTexture().Desaturate());
            }
        }

        public static UIMenuText ConsoleText;
        private static void CreateConsole()
        {
            GameObject Console = UnityEngine.Object.Instantiate(MenuHelper.singleButtonTemplate.transform.Find("Background").gameObject, MenuHelper.menuPageTemplate.transform);

            Console.name = "HexedConsole";
            Console.GetComponent<Image>().sprite = UnityUtils.GetSprite("Console");
            Console.GetComponent<Image>().overrideSprite = UnityUtils.GetSprite("Console");
            Console.GetComponent<Image>().color = Color.white;

            Console.transform.localScale = new Vector3(0.88f, 0.55f, 1);
            Console.transform.localPosition = new Vector3(0, 460, 0);
            Console.AddComponent<RectMask2D>();

            ConsoleText = new UIMenuText(Console.gameObject, "", new Vector2(-310, -370), 26, false, TextAnchor.LowerLeft);
            ConsoleText.TextObject.transform.localScale = new Vector3(1, 1.55f, 1);
            ConsoleText.text.color = Color.grey;
        }

        public static void ChangeTrustColors()
        {
            VRCPlayer.field_Internal_Static_Color_0 = new Color(0.72f, 0.13f, 0.11f); // Nuisance
            VRCPlayer.field_Internal_Static_Color_1 = Color.yellow; // Friends
            VRCPlayer.field_Internal_Static_Color_2 = new Color(0.35f, 0.35f, 0.35f); // Visitor
            VRCPlayer.field_Internal_Static_Color_3 = new Color(0f, 1f, 1f); // New User
            VRCPlayer.field_Internal_Static_Color_4 = new Color(0f, 1f, 0f); // User
            VRCPlayer.field_Internal_Static_Color_5 = new Color(1f, 0.42f, 0f); // Known
            VRCPlayer.field_Internal_Static_Color_6 = new Color(0.7f, 0, 0.51f); // Trusted
            VRCPlayer.field_Internal_Static_Color_8 = Color.red; // MOD
        }

        private static void RemoveLoadingBackground()
        {
            Transform LoadingBackground = VRCApplication.prop_VRCApplication_0.transform.Find("TrackingVolume/VRLoadingOverlay");
            if (LoadingBackground != null)
            {
                LoadingBackground.localScale = Vector3.zero;
                LoadingBackground.gameObject.SetActive(false);
            }
            else Wrappers.Logger.LogError("Failed to find LoadingBackground");
        }
    }
}
