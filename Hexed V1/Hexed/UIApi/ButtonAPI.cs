using TMPro;
using UnityEngine;
using Hexed.HexedServer;
using Hexed.Wrappers;
using VRC.UI.Elements.Menus;
using VRC.UI.Elements;
using UnityEngine.UI;
using VRC.UI.Elements.Controls;
using VRC.Localization;

namespace Hexed.UIApi
{
    public class ButtonAPI
    {
        public enum ButtonSize
        {
            Default,
            Half,
            Square
        }

        public enum MMButtonType
        {
            Big,
            Medium,
            Small
        }
    }

    public class QMButtonBase
    {
        public GameObject baseObject;
        public Transform parentPage;
        private Action clickAction;
        private int[] initShift = { 0, 0 };

        public GameObject GetGameObject()
        {
            return baseObject;
        }

        public void SetActive(bool state)
        {
            baseObject.gameObject.SetActive(state);
        }

        public void SetLocation(float buttonXLoc, float buttonYLoc)
        {
            baseObject.GetComponent<RectTransform>().anchoredPosition += Vector2.right * (232 * (buttonXLoc + initShift[0]));
            baseObject.GetComponent<RectTransform>().anchoredPosition += Vector2.down * (210 * (buttonYLoc + initShift[1]));
        }

        public void SetIcon(Sprite icon)
        {
            foreach (var x in baseObject.GetComponentsInChildren<Image>())
            {
                if (x.gameObject.name != "Icon") continue;

                x.sprite = icon;
                x.overrideSprite = icon;
            }
        }

        public void SetToolTip(string buttonToolTip)
        {
            LocalizableString localizableString = LocalizableStringExtensions.Localize(buttonToolTip);

            foreach (var x in baseObject.GetComponentsInChildren<ToolTip>())
            {
                x._localizableString = localizableString;
            }
        }

        public void DestroyMe()
        {
            UnityEngine.Object.Destroy(baseObject);
        }

        public void ClickMe()
        {
            baseObject.GetComponent<Button>().onClick.Invoke();
        }

        public void SetBackgroundImage(Sprite newImg)
        {
            baseObject.transform.Find("Background").GetComponent<Image>().sprite = newImg;
            baseObject.transform.Find("Background").GetComponent<Image>().overrideSprite = newImg;
        }

        public void SetButtonText(string buttonText)
        {
            baseObject.GetComponentInChildren<TextMeshProUGUIEx>().m_isRichText = true;
            baseObject.GetComponentInChildren<TextMeshProUGUIEx>().text = buttonText;
        }

        public void SetAction(Action buttonAction)
        {
            clickAction = buttonAction;

            baseObject.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();

            if (clickAction != null) baseObject.GetComponent<Button>().onClick.AddListener(clickAction);
        }
    }

    public class MMButtonBase
    {
        public GameObject baseObject;
        public Transform parentPage;
        public int[] initShift = { 0, 0 };

        public GameObject GetGameObject()
        {
            return baseObject;
        }

        public void SetActive(bool state)
        {
            baseObject.gameObject.SetActive(state);
        }

        public void SetLocation(float buttonXLoc, float buttonYLoc)
        {
            baseObject.GetComponent<RectTransform>().anchoredPosition += Vector2.right * (232 * (buttonXLoc + initShift[0]));
            baseObject.GetComponent<RectTransform>().anchoredPosition += Vector2.down * (210 * (buttonYLoc + initShift[1]));
        }

        public void SetIcon(Sprite icon)
        {
            baseObject.transform.Find("Text_ButtonName/Icon").GetComponent<Image>().sprite = icon;
            baseObject.transform.Find("Text_ButtonName/Icon").GetComponent<Image>().overrideSprite = icon;
        }

        public void SetToolTip(string buttonToolTip)
        {
            LocalizableString localizableString = LocalizableStringExtensions.Localize(buttonToolTip);

            foreach (var x in baseObject.GetComponentsInChildren<ToolTip>())
            {
                x._localizableString = localizableString;
            }
        }

        public void DestroyMe()
        {
            UnityEngine.Object.Destroy(baseObject);
        }

        public void ClickMe()
        {
            baseObject.GetComponent<Button>().onClick.Invoke();
        }

        public void SetBackgroundImage(Sprite newImg)
        {
            baseObject.transform.Find("Background_Button").GetComponent<Image>().sprite = newImg;
            baseObject.transform.Find("Background_Button").GetComponent<Image>().overrideSprite = newImg;
        }

        public void SetButtonText(string buttonText)
        {
            baseObject.GetComponentInChildren<TextMeshProUGUIEx>().m_isRichText = true;
            baseObject.GetComponentInChildren<TextMeshProUGUIEx>().text = buttonText;
        }

        public void SetAction(Action buttonAction)
        {
            baseObject.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            if (buttonAction != null) baseObject.GetComponent<Button>().onClick.AddListener(buttonAction);
        }
    }

    public class QMMenuPage
    {
        public GameObject MenuObject;
        public TextMeshProUGUI MenuTitleText;
        private GameObject BackButton;
        private UIPage MenuPage;

        public QMMenuPage(string menuTitle)
        {
            Initialize(menuTitle);
        }

        private void Initialize(string menuTitle)
        {
            MenuObject = UnityEngine.Object.Instantiate(MenuHelper.menuPageTemplate, MenuHelper.menuPageTemplate.transform.parent);

            MenuObject.SetActive(false);

            MenuObject.name = $"Hexed_Menu_{EncryptUtils.RandomString(12)}";
            UnityEngine.Object.DestroyImmediate(MenuObject.GetComponent<MainMenuContent>());
            MenuPage = MenuObject.AddComponent<UIPage>();
            MenuPage.field_Public_String_0 = MenuObject.name;
            MenuPage.field_Private_Boolean_1 = true;
            MenuPage.field_Protected_MenuStateController_0 = MenuHelper.menuStateController;
            MenuPage.field_Private_List_1_UIPage_0 = new();
            MenuPage.field_Private_List_1_UIPage_0.Add(MenuPage);

            MenuHelper.menuStateController.field_Private_Dictionary_2_String_UIPage_0.Add(MenuObject.name, MenuPage);

            MenuObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup").DestroyChildren();
            MenuTitleText = MenuObject.GetComponentInChildren<TextMeshProUGUI>(true);
            MenuTitleText.text = menuTitle;
            BackButton = MenuObject.transform.GetChild(0).Find("LeftItemContainer/Button_Back").gameObject;
            BackButton.SetActive(true);

            MenuObject.transform.GetChild(0).Find("RightItemContainer/Button_QM_Expand").gameObject.SetActive(false);

            for (int i = 0; i < MenuObject.transform.childCount; i++)
            {
                if (MenuObject.transform.GetChild(i).name != "Header_H1" && MenuObject.transform.GetChild(i).name != "ScrollRect")
                {
                    UnityEngine.Object.Destroy(MenuObject.transform.GetChild(i).gameObject);
                }
            }

            MenuObject.transform.Find("ScrollRect").GetComponent<ScrollRect>().enabled = false;
        }

        public void OpenMe()
        {
            MenuObject.SetActive(true);
            MenuHelper.menuStateController.Method_Public_Void_String_UIContext_Boolean_TransitionType_0(MenuPage.field_Public_String_0);
        }

        public void OpenMe(Action action)
        {
            MenuObject.SetActive(true);
            MenuHelper.menuStateController.Method_Public_Void_String_UIContext_Boolean_TransitionType_0(MenuPage.field_Public_String_0);
            action.Invoke();
        }

        public string GetMenuName()
        {
            return MenuObject.name;
        }

        public GameObject GetMenuObject()
        {
            return MenuObject;
        }

        public GameObject GetBackButton()
        {
            return BackButton;
        }
    }

    public class QMSingleButton : QMButtonBase
    {
        public QMSingleButton(QMMenuPage btnMenu, float btnXLocation, float btnYLocation, string btnText, Action btnAction, string btnToolTip, ButtonAPI.ButtonSize Size = ButtonAPI.ButtonSize.Default, Sprite Sprite = null)
        {
            parentPage = btnMenu.GetMenuObject().transform;
            InitButton(btnXLocation, btnYLocation, btnText, btnAction, btnToolTip, Size, Sprite);
        }

        public QMSingleButton(Transform btnMenu, float btnXLocation, float btnYLocation, string btnText, Action btnAction, string btnToolTip, ButtonAPI.ButtonSize Size = ButtonAPI.ButtonSize.Default, Sprite Sprite = null)
        {
            parentPage = btnMenu;
            InitButton(btnXLocation, btnYLocation, btnText, btnAction, btnToolTip, Size, Sprite);
        }

        private void InitButton(float btnXLocation, float btnYLocation, string btnText, Action btnAction, string btnToolTip, ButtonAPI.ButtonSize Size = ButtonAPI.ButtonSize.Default, Sprite Sprite = null)
        {
            if (Size != ButtonAPI.ButtonSize.Default) btnYLocation -= 0.26f;

            baseObject = UnityEngine.Object.Instantiate(MenuHelper.singleButtonTemplate, parentPage);

            SetActive(false);

            baseObject.name = $"Hexed_Button_{EncryptUtils.RandomString(12)}";
            baseObject.GetComponentInChildren<TextMeshProUGUI>().richText = true;
            baseObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 176);
            baseObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-68, -225);

            if (Sprite != null) SetIcon(Sprite);
            else baseObject.transform.Find("Icons").gameObject.SetActive(false);

            UnityEngine.Object.DestroyImmediate(baseObject.transform.Find("Badge_MMJump").gameObject);
            UnityEngine.Object.DestroyImmediate(baseObject.transform.Find("Badge_Close").gameObject);

            SetLocation(btnXLocation, btnYLocation);
            SetButtonText(btnText);
            SetToolTip(btnToolTip);
            SetAction(btnAction);

            SetActive(true);

            switch (Size)
            {
                case ButtonAPI.ButtonSize.Half:
                    {
                        Vector2 sizeDelta = baseObject.GetComponentInChildren<RectTransform>().sizeDelta;
                        sizeDelta.y /= 2;
                        baseObject.GetComponentInChildren<RectTransform>().sizeDelta = sizeDelta;
                    }
                    break;

                case ButtonAPI.ButtonSize.Square:
                    {
                        Vector2 sizeDelta = baseObject.GetComponentInChildren<RectTransform>().sizeDelta;
                        sizeDelta.y /= 2;
                        sizeDelta.x /= 2;
                        baseObject.GetComponentInChildren<RectTransform>().sizeDelta = sizeDelta;
                    }
                    break;
            }
        }
    }

    public class MMSingleButton : MMButtonBase
    {
        // add support for MMMenuPage here

        public MMSingleButton(Transform btnMenu, float btnXLocation, float btnYLocation, string btnText, Action btnAction, string btnToolTip, ButtonAPI.MMButtonType Size = ButtonAPI.MMButtonType.Big, Sprite Sprite = null)
        {
            parentPage = btnMenu;
            InitButton(btnXLocation, btnYLocation, btnText, btnAction, btnToolTip, Size, Sprite);
        }

        private void InitButton(float btnXLocation, float btnYLocation, string btnText, Action btnAction, string btnToolTip, ButtonAPI.MMButtonType Size = ButtonAPI.MMButtonType.Big, Sprite Sprite = null)
        {
            switch (Size)
            {
                case ButtonAPI.MMButtonType.Big:
                    baseObject = UnityEngine.Object.Instantiate(MenuHelper.mainBigSingleButtonTemplate, parentPage);
                    break;

                case ButtonAPI.MMButtonType.Medium:
                    baseObject = UnityEngine.Object.Instantiate(MenuHelper.mainMediumSingleButtonTemplate, parentPage);
                    break;

                case ButtonAPI.MMButtonType.Small:
                    baseObject = UnityEngine.Object.Instantiate(MenuHelper.mainSmallSingleButtonTemplate, parentPage);
                    break;
            }

            SetActive(false);

            baseObject.name = $"Hexed_Button_{EncryptUtils.RandomString(12)}";
            baseObject.GetComponentInChildren<TextMeshProUGUI>().richText = true;
            //baseObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 176);
            //baseObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-68, -225);

            if (Sprite != null) SetIcon(Sprite);
            else baseObject.transform.Find("Text_ButtonName/Icon").GetComponentInChildren<Image>().gameObject.SetActive(false);

            //initShift[0] = 0;
            //initShift[1] = 0;
            //SetLocation(btnXLocation, btnYLocation);
            SetButtonText(btnText);
            SetToolTip(btnToolTip);
            SetAction(btnAction);

            SetActive(true);
        }
    }

    public class QMIconButton : QMButtonBase
    {
        public QMIconButton(QMMenuPage btnMenu, float btnXLocation, float btnYLocation, Action btnAction, string btnToolTip, Sprite Sprite = null)
        {
            parentPage = btnMenu.GetMenuObject().transform;
            InitButton(btnXLocation, btnYLocation, btnAction, btnToolTip, Sprite);
        }

        public QMIconButton(Transform btnMenu, float btnXLocation, float btnYLocation, Action btnAction, string btnToolTip, Sprite Sprite = null)
        {
            parentPage = btnMenu;
            InitButton(btnXLocation, btnYLocation, btnAction, btnToolTip, Sprite);
        }

        private void InitButton(float btnXLocation, float btnYLocation, Action btnAction, string btnToolTip, Sprite Sprite = null)
        {
            baseObject = UnityEngine.Object.Instantiate(MenuHelper.iconButtonTemplate, parentPage);

            SetActive(false);

            baseObject.name = $"Hexed-IconButton-{EncryptUtils.RandomString(12)}";
            baseObject.GetComponent<RectTransform>().sizeDelta += new Vector2(10, 10);
            baseObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-68, -228);

            if (Sprite != null) SetIcon(Sprite);
            SetLocation(btnXLocation, btnYLocation);
            SetToolTip(btnToolTip);
            SetAction(btnAction);

            SetActive(true);
        }
    }

    public class QMToggleButton : QMButtonBase
    {
        private QMSingleButton Button;
        private bool currentState;
        private Action OnAction;
        private Action OffAction;

        public QMToggleButton(QMMenuPage location, float btnXPos, float btnYPos, string btnText, Action onAction, Action offAction, string btnToolTip, bool defaultState = false)
        {
            parentPage = location.GetMenuObject().transform;
            Initialize(btnXPos, btnYPos, btnText, onAction, offAction, btnToolTip, defaultState);
        }

        public QMToggleButton(Transform location, float btnXPos, float btnYPos, string btnText, Action onAction, Action offAction, string btnToolTip, bool defaultState = false)
        {
            parentPage = location;
            Initialize(btnXPos, btnYPos, btnText, onAction, offAction, btnToolTip, defaultState);
        }

        private void Initialize(float btnXLocation, float btnYLocation, string btnText, Action onAction, Action offAction, string btnToolTip, bool defaultState = false)
        {
            OnAction = onAction;
            OffAction = offAction;
            currentState= defaultState;

            Button = new QMSingleButton(parentPage, btnXLocation, btnYLocation, btnText, HandleClick, btnToolTip, ButtonAPI.ButtonSize.Default, currentState ? UnityUtils.GetSprite("Enabled") : UnityUtils.GetSprite("Disabled"));
        }

        public void SetToggleState(bool newState)
        {
            currentState = newState;
            Button.SetIcon(currentState ? UnityUtils.GetSprite("Enabled") : UnityUtils.GetSprite("Disabled"));
        }

        private void HandleClick()
        {
            currentState = !currentState;
            SetToggleState(currentState);
            if (currentState) OnAction.Invoke();
            else OffAction.Invoke();
        }

        public bool GetCurrentState()
        {
            return currentState;
        }
    }

    public class QMSelectButton : QMButtonBase
    {
        public QMSingleButton Button;
        private QMMenuPage Page;

        public QMSingleButton[] SelectButtons;

        public QMSelectButton(QMMenuPage location, float btnXPos, float btnYPos, string btnText, string btnToolTip, object enumObject, int ActiveIndex)
        {
            parentPage = location.GetMenuObject().transform;
            Initialize(btnXPos, btnYPos, btnText, btnToolTip, enumObject, ActiveIndex);
        }

        public QMSelectButton(Transform location, float btnXPos, float btnYPos, string btnText, string btnToolTip, object enumObject, int ActiveIndex)
        {
            parentPage = location;
            Initialize(btnXPos, btnYPos, btnText, btnToolTip, enumObject, ActiveIndex);
        }

        private void Initialize(float btnXLocation, float btnYLocation, string btnText, string btnToolTip, object enumObject, int ActiveIndex)
        {
            Page = new("Select");
            Button = new QMSingleButton(parentPage, btnXLocation, btnYLocation, btnText, Page.OpenMe, btnToolTip, ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Options"));

            string[] Names = Enum.GetNames(enumObject.GetType());

            if (Names.Length == 4)
            {
                SelectButtons = new QMSingleButton[4];

                SelectButtons[0] = new QMSingleButton(Page, 2.5f, 0, Names[0], delegate { HandleClick(0); }, btnToolTip, ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Disabled"));
                SelectButtons[1] = new QMSingleButton(Page, 2.5f, 1, Names[1], delegate { HandleClick(1); }, btnToolTip, ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Disabled"));
                SelectButtons[2] = new QMSingleButton(Page, 2.5f, 2, Names[2], delegate { HandleClick(2); }, btnToolTip, ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Disabled"));
                SelectButtons[3] = new QMSingleButton(Page, 2.5f, 3, Names[3], delegate { HandleClick(3); }, btnToolTip, ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Disabled"));
            }

            else if (Names.Length == 3)
            {
                SelectButtons = new QMSingleButton[3];

                SelectButtons[0] = new QMSingleButton(Page, 2.5f, 0.5f, Names[0], delegate { HandleClick(0); }, btnToolTip, ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Disabled"));
                SelectButtons[1] = new QMSingleButton(Page, 2.5f, 1.5f, Names[1], delegate { HandleClick(1); }, btnToolTip, ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Disabled"));
                SelectButtons[2] = new QMSingleButton(Page, 2.5f, 2.5f, Names[2], delegate { HandleClick(2); }, btnToolTip, ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Disabled"));
            }

            // Add support for only 2 states, make more dynamic tho

            SetToggleState(ActiveIndex, UnityUtils.GetSprite("Enabled"));
        }

        private void HandleClick(int Index)
        {
            for (int i = 0; i < SelectButtons.Length; i++)
            {
                SetToggleState(i, false);
            }

            SetToggleState(Index, true);
        }

        public void AddAction(int Index, Action action)
        {
            SelectButtons[Index].baseObject.GetComponent<Button>().onClick.AddListener(action);
        }

        public void SetToggleState(int Index, bool active)
        {
            SelectButtons[Index].SetIcon(active ? UnityUtils.GetSprite("Enabled") : UnityUtils.GetSprite("Disabled"));
        }
    }

    public class QMScrollButton : QMButtonBase
    {
        public QMSingleButton MainButton;
        private QMSingleButton BackButton;
        private QMSingleButton NextButton;
        private QMMenuPage Page;

        public List<ScrollObject> ScrollButtons = new();

        private float Posx = 1;
        private float Posy = 0;
        private int Index = 0;
        private int currentMenuIndex = 0;

        public class ScrollObject
        {
            public QMButtonBase ButtonBase;
            public int Index;
        }

        public QMScrollButton(QMMenuPage location, float btnXPos, float btnYPos, string btnText, string btnToolTip, ButtonAPI.ButtonSize Size = ButtonAPI.ButtonSize.Default, Sprite Sprite = null)
        {
            parentPage = location.GetMenuObject().transform;
            Initialize(btnXPos, btnYPos, btnText, btnToolTip, Size, Sprite);
        }

        public QMScrollButton(Transform location, float btnXPos, float btnYPos, string btnText, string btnToolTip, ButtonAPI.ButtonSize Size = ButtonAPI.ButtonSize.Default, Sprite Sprite = null)
        {
            parentPage = location;
            Initialize(btnXPos, btnYPos, btnText, btnToolTip, Size, Sprite);
        }

        private void Initialize(float btnXLocation, float btnYLocation, string btnText, string btnToolTip, ButtonAPI.ButtonSize Size = ButtonAPI.ButtonSize.Default, Sprite Sprite = null)
        {
            Page = new("Scroll");

            BackButton = new QMSingleButton(Page, 1.5f, 3.5f, "<---", () =>
            {
                ShowMenu(currentMenuIndex - 1);
            }, "Go Back", ButtonAPI.ButtonSize.Half);

            NextButton = new QMSingleButton(Page, 3.5f, 3.5f, "--->", () =>
            {
                ShowMenu(currentMenuIndex + 1);
            }, "Go Next", ButtonAPI.ButtonSize.Half);

            MainButton = new QMSingleButton(parentPage, btnXLocation, btnYLocation, btnText, null, btnToolTip, Size, Sprite);
        }


        private void ShowMenu(int MenuIndex)
        {
            if (MenuIndex < 0 || MenuIndex > Index) return;

            foreach (ScrollObject scrollObject in ScrollButtons)
            {
                QMButtonBase buttonBase = scrollObject.ButtonBase;
                buttonBase?.SetActive(scrollObject.Index == MenuIndex);
            }

            currentMenuIndex = MenuIndex;
            //IndexText.text.text = $"{currentMenuIndex + 1}/{Index + 1}";
        }

        private void Clear()
        {
            foreach (ScrollObject scrollObject in ScrollButtons)
            {
                if (scrollObject != null) UnityEngine.Object.Destroy(scrollObject.ButtonBase.GetGameObject());
            }

            ScrollButtons.Clear();
            Posx = 1;
            Posy = 0;
            Index = 0;
            currentMenuIndex = 0;
        }

        public void Add(string Text, string Description, Action Action)
        {
            if (Posy == 3)
            {
                Posy = 0;
                Index++;
            }

            QMSingleButton Button = new(Page, Posx, Posy, Text, Action, Description, ButtonAPI.ButtonSize.Half);

            Button.SetActive(false);
            ScrollButtons.Add(new ScrollObject
            {
                ButtonBase = Button,
                Index = Index
            });

            Posx++;
            if (Posx == 5)
            {
                Posx = 1;
                Posy += 0.5f;
            }
        }

        public new void SetAction(Action buttonAction)
        {
            MainButton.SetAction(delegate
            {
                Clear();
                buttonAction.Invoke();
                Page.OpenMe();
                ShowMenu(0);
            });
        }
    }
}
