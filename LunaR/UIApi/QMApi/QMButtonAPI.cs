using LunaR.UIApi;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.UI.Elements;
using VRC.UI.Elements.Menus;

namespace LunaR.Api
{
    public class QMButtonAPI
    {
        public const string Identifier = "LunaR";

        public enum ButtonSize
        {
            Default,
            Half,
            Square
        }

        public enum SliderSize
        {
            Default,
            Double,
            Tripple
        }
    }

    public class QMButtonBase
    {
        protected GameObject button;
        protected string btnQMLoc;
        protected int[] initShift = { 0, 0 };
        protected Color OrigBackground;
        protected Color OrigText;

        public GameObject GetGameObject()
        {
            return button;
        }

        public void SetActive(bool state)
        {
            button.gameObject.SetActive(state);
        }

        public void SetLocation(float buttonXLoc, float buttonYLoc)
        {
            button.GetComponent<RectTransform>().anchoredPosition += Vector2.right * (232 * (buttonXLoc + initShift[0]));
            button.GetComponent<RectTransform>().anchoredPosition += Vector2.down * (210 * (buttonYLoc + initShift[1]));
        }

        public void SetToolTip(string buttonToolTip)
        {
            button.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>().field_Public_String_0 = buttonToolTip;
            button.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>().field_Public_String_1 = buttonToolTip;
        }

        public void DestroyMe()
        {
            if (button != null) UnityEngine.Object.Destroy(button);
        }

        public virtual void SetTextColor(Color buttonTextColor, bool save = true)
        { }
    }

    public class QMSingleButton : QMButtonBase
    {
        public QMSingleButton(QMNestedButton btnMenu, float btnXLocation, float btnYLocation, string btnText, Action btnAction, string btnToolTip, Color? btnTextColor = null, QMButtonAPI.ButtonSize Size = QMButtonAPI.ButtonSize.Default, Sprite Sprite = null)
        {
            btnQMLoc = btnMenu.GetMenuName();
            InitButton(btnXLocation, btnYLocation, btnText, btnAction, btnToolTip, Size, btnTextColor, Sprite);
            if (Size != QMButtonAPI.ButtonSize.Default)
            {
                button.GetComponentInChildren<RectTransform>().sizeDelta /= new Vector2(1f, 2f);
                button.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0, 22);
                if (Size == QMButtonAPI.ButtonSize.Square) button.GetComponentInChildren<RectTransform>().sizeDelta /= new Vector2(2f, 1f);
            }
        }

        public QMSingleButton(string btnMenu, float btnXLocation, float btnYLocation, string btnText, Action btnAction, string btnToolTip, Color? btnTextColor = null, QMButtonAPI.ButtonSize Size = QMButtonAPI.ButtonSize.Default, Sprite Sprite = null)
        {
            btnQMLoc = btnMenu;
            InitButton(btnXLocation, btnYLocation, btnText, btnAction, btnToolTip, Size, btnTextColor, Sprite);
            if (Size != QMButtonAPI.ButtonSize.Default)
            {
                button.GetComponentInChildren<RectTransform>().sizeDelta /= new Vector2(1f, 2f);
                button.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0, 22);
                if (Size == QMButtonAPI.ButtonSize.Square) button.GetComponentInChildren<RectTransform>().sizeDelta /= new Vector2(2f, 1f);
            }
        }

        protected void InitButton(float btnXLocation, float btnYLocation, string btnText, Action btnAction, string btnToolTip, QMButtonAPI.ButtonSize Size = QMButtonAPI.ButtonSize.Default, Color? btnTextColor = null, Sprite Sprite = null)
        {
            if (Size != QMButtonAPI.ButtonSize.Default) btnYLocation -= 0.21f;

            button = UnityEngine.Object.Instantiate(APIStuff.SingleButtonTemplate(), GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/" + btnQMLoc).transform, true);
            button.name = $"{QMButtonAPI.Identifier}-SingleButton-{APIStuff.RandomNextNumber()}";
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 176);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(-68, -225);

            if (Sprite != null)
            {
                Image iconImage = button.transform.Find("Icon").GetComponent<Image>();
                iconImage.sprite = Sprite;
                iconImage.overrideSprite = Sprite;
            }
            else button.transform.Find("Icon").GetComponentInChildren<Image>().gameObject.SetActive(false);

            initShift[0] = 0;
            initShift[1] = 0;
            SetLocation(btnXLocation, btnYLocation);
            SetButtonText(btnText);
            SetToolTip(btnToolTip);
            SetAction(btnAction);

            if (btnTextColor != null) SetTextColor((Color)btnTextColor);
            else OrigText = button.GetComponentInChildren<TextMeshProUGUI>().color;

            SetActive(true);
        }

        public void SetBackgroundImage(Sprite newImg)
        {
            button.transform.Find("Background").GetComponent<Image>().sprite = newImg;
            button.transform.Find("Background").GetComponent<Image>().overrideSprite = newImg;
        }

        public void SetButtonText(string buttonText)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        }

        public void SetAction(Action buttonAction)
        {
            button.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            if (buttonAction != null) button.GetComponent<Button>().onClick.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction>(buttonAction));
        }

        public void ClickMe()
        {
            button.GetComponent<Button>().onClick.Invoke();
        }

        public override void SetTextColor(Color buttonTextColor, bool save = true)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().SetOutlineColor(buttonTextColor);
            if (save) OrigText = buttonTextColor;
        }
    }

    public class QMIconButton : QMButtonBase
    {
        public QMIconButton(QMNestedButton btnMenu, float btnXLocation, float btnYLocation, Action btnAction, string btnToolTip, Sprite Sprite = null)
        {
            btnQMLoc = btnMenu.GetMenuName();
            InitButton(btnXLocation, btnYLocation, btnAction, btnToolTip, Sprite);
        }

        public QMIconButton(string btnMenu, float btnXLocation, float btnYLocation, Action btnAction, string btnToolTip, Sprite Sprite = null)
        {
            btnQMLoc = btnMenu;
            InitButton(btnXLocation, btnYLocation, btnAction, btnToolTip, Sprite);
        }

        protected void InitButton(float btnXLocation, float btnYLocation, Action btnAction, string btnToolTip, Sprite Sprite = null)
        {
            button = UnityEngine.Object.Instantiate(APIStuff.GetQMIconButtonTemplate(), GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/" + btnQMLoc).transform, true);
            button.name = $"{QMButtonAPI.Identifier}-IconButton-{APIStuff.RandomNextNumber()}";
            button.GetComponent<RectTransform>().sizeDelta += new Vector2(10, 10);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(-68, -228);

            if (Sprite != null)
            {
                Image iconImage = button.transform.Find("Icon").GetComponent<Image>();
                iconImage.sprite = Sprite;
                iconImage.overrideSprite = Sprite;
            }

            initShift[0] = 0;
            initShift[1] = 0;
            SetLocation(btnXLocation, btnYLocation);
            SetToolTip(btnToolTip);
            SetAction(btnAction);

            SetActive(true);
        }

        public void SetBackgroundImage(Sprite newImg)
        {
            button.transform.Find("Background").GetComponent<Image>().sprite = newImg;
            button.transform.Find("Background").GetComponent<Image>().overrideSprite = newImg;
        }

        public void SetButtonText(string buttonText)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        }

        public void SetAction(Action buttonAction)
        {
            button.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            if (buttonAction != null) button.GetComponent<Button>().onClick.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction>(buttonAction));
        }

        public void ClickMe()
        {
            button.GetComponent<Button>().onClick.Invoke();
        }

        public override void SetTextColor(Color buttonTextColor, bool save = true)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().SetOutlineColor(buttonTextColor);
            if (save) OrigText = buttonTextColor;
        }
    }

    public class QMToggleButton : QMButtonBase
    {
        protected TextMeshProUGUI btnTextComp;
        protected Button btnComp;
        protected Image btnImageComp;
        protected bool currentState;
        protected Action OnAction;
        protected Action OffAction;

        public QMToggleButton(QMNestedButton location, float btnXPos, float btnYPos, string btnText, Action onAction, Action offAction, string btnToolTip, bool defaultState = false)
        {
            btnQMLoc = location.GetMenuName();
            Initialize(btnXPos, btnYPos, btnText, onAction, offAction, btnToolTip, defaultState);
        }

        public QMToggleButton(string location, float btnXPos, float btnYPos, string btnText, Action onAction, Action offAction, string btnToolTip, bool defaultState = false)
        {
            btnQMLoc = location;
            Initialize(btnXPos, btnYPos, btnText, onAction, offAction, btnToolTip, defaultState);
        }

        private void Initialize(float btnXLocation, float btnYLocation, string btnText, Action onAction, Action offAction, string btnToolTip, bool defaultState)
        {
            button = UnityEngine.Object.Instantiate(APIStuff.SingleButtonTemplate(), GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/" + btnQMLoc).transform, true);
            button.name = $"{QMButtonAPI.Identifier}-ToggleButton-{APIStuff.RandomNextNumber()}";
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 176);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(-68, -225);
            btnTextComp = button.GetComponentInChildren<TextMeshProUGUI>(true);
            btnComp = button.GetComponentInChildren<Button>(true);
            btnComp.onClick = new Button.ButtonClickedEvent();
            btnComp.onClick.AddListener(new Action(HandleClick));
            btnImageComp = button.transform.Find("Icon").GetComponentInChildren<Image>(true);

            initShift[0] = 0;
            initShift[1] = 0;
            SetLocation(btnXLocation, btnYLocation);
            SetButtonText(btnText);
            SetButtonActions(onAction, offAction);
            SetToolTip(btnToolTip);
            SetActive(true);

            currentState = defaultState;
            var tmpIcon = currentState ? APIStuff.GetOnIconSprite() : APIStuff.GetOffIconSprite();
            btnImageComp.sprite = tmpIcon;
            btnImageComp.overrideSprite = tmpIcon;
        }

        private void HandleClick()
        {
            currentState = !currentState;
            var stateIcon = currentState ? APIStuff.GetOnIconSprite() : APIStuff.GetOffIconSprite();
            btnImageComp.sprite = stateIcon;
            btnImageComp.overrideSprite = stateIcon;
            if (currentState) OnAction.Invoke();
            else OffAction.Invoke();
        }

        public void SetButtonText(string buttonText)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        }

        public void SetButtonActions(Action onAction, Action offAction)
        {
            OnAction = onAction;
            OffAction = offAction;
        }

        public void SetToggleState(bool newState, bool shouldInvoke = false)
        {
            if (currentState == newState) return;
            currentState = newState;
            Sprite newIcon = currentState ? APIStuff.GetOnIconSprite() : APIStuff.GetOffIconSprite();
            btnImageComp.sprite = newIcon;
            btnImageComp.overrideSprite = newIcon;

            if (shouldInvoke)
            {
                if (newState) OnAction.Invoke();
                else OffAction.Invoke();
            }
        }

        public void ClickMe()
        {
            HandleClick();
        }

        public bool GetCurrentState()
        {
            return currentState;
        }
    }

    public class QMNestedButton
    {
        protected string btnQMLoc;
        protected GameObject MenuObject;
        protected TextMeshProUGUI MenuTitleText;
        protected UIPage MenuPage;
        protected bool IsMenuRoot;
        protected GameObject BackButton;
        protected QMSingleButton MainButton;
        protected string MenuName;

        public QMNestedButton(QMNestedButton location, float posX, float posY, string btnText, string toolTipText, string menuTitle, QMButtonAPI.ButtonSize Size = QMButtonAPI.ButtonSize.Default, Sprite Sprite = null)
        {
            btnQMLoc = location.GetMenuName();
            Initialize(false, btnText, posX, posY, toolTipText, menuTitle, Size, Sprite);
        }

        public QMNestedButton(string location, float posX, float posY, string btnText, string toolTipText, string menuTitle, QMButtonAPI.ButtonSize Size = QMButtonAPI.ButtonSize.Default, Sprite Sprite = null)
        {
            btnQMLoc = location;
            Initialize(btnQMLoc.StartsWith("Menu_"), btnText, posX, posY, toolTipText, menuTitle, Size, Sprite);
        }

        private void Initialize(bool isRoot, string btnText, float btnPosX, float btnPosY, string btnToolTipText, string menuTitle, QMButtonAPI.ButtonSize Size = QMButtonAPI.ButtonSize.Default, Sprite Sprite = null)
        {
            MenuName = $"{QMButtonAPI.Identifier}-Menu-{APIStuff.RandomNextNumber()}";
            MenuObject = UnityEngine.Object.Instantiate(APIStuff.GetMenuPageTemplate(), APIStuff.GetMenuPageTemplate().transform.parent);
            MenuObject.name = MenuName;
            MenuObject.SetActive(false);
            UnityEngine.Object.DestroyImmediate(MenuObject.GetComponent<LaunchPadQMMenu>());
            MenuPage = MenuObject.AddComponent<UIPage>();
            MenuPage.field_Public_String_0 = MenuName;
            MenuPage.field_Private_Boolean_1 = true;
            MenuPage.field_Protected_MenuStateController_0 = APIStuff.GetQuickMenuInstance().prop_MenuStateController_0;
            MenuPage.field_Private_List_1_UIPage_0 = new Il2CppSystem.Collections.Generic.List<UIPage>();
            MenuPage.field_Private_List_1_UIPage_0.Add(MenuPage);
            APIStuff.GetQuickMenuInstance().prop_MenuStateController_0.field_Private_Dictionary_2_String_UIPage_0.Add(MenuName, MenuPage);
            if (isRoot)
            {
                var list = APIStuff.GetQuickMenuInstance().prop_MenuStateController_0.field_Public_ArrayOf_UIPage_0.ToList();
                list.Add(MenuPage);
                APIStuff.GetQuickMenuInstance().prop_MenuStateController_0.field_Public_ArrayOf_UIPage_0 = list.ToArray();
            }

            MenuObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup").DestroyChildren();
            MenuTitleText = MenuObject.GetComponentInChildren<TextMeshProUGUI>(true);
            MenuTitleText.text = menuTitle;
            IsMenuRoot = isRoot;
            BackButton = MenuObject.transform.GetChild(0).Find("LeftItemContainer/Button_Back").gameObject;
            BackButton.SetActive(true);
            BackButton.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            BackButton.GetComponentInChildren<Button>().onClick.AddListener(new Action(() =>
            {
                if (isRoot)
                {
                    if (btnQMLoc.StartsWith("Menu_"))
                    {
                        APIStuff.GetQuickMenuInstance().prop_MenuStateController_0.Method_Public_Void_String_Boolean_Boolean_0("QuickMenu" + btnQMLoc.Remove(0, 5));
                        return;
                    }
                    APIStuff.GetQuickMenuInstance().prop_MenuStateController_0.Method_Public_Void_String_Boolean_Boolean_0(btnQMLoc);
                    return;
                }
                MenuPage.Method_Protected_Virtual_New_Void_0();
            }));
            MenuObject.transform.GetChild(0).Find("RightItemContainer/Button_QM_Expand").gameObject.SetActive(false);
            MainButton = new QMSingleButton(btnQMLoc, btnPosX, btnPosY, btnText, OpenMe, btnToolTipText, null, Size, Sprite);

            for (int i = 0; i < MenuObject.transform.childCount; i++)
            {
                if (MenuObject.transform.GetChild(i).name != "Header_H1" && MenuObject.transform.GetChild(i).name != "ScrollRect")
                {
                    UnityEngine.Object.Destroy(MenuObject.transform.GetChild(i).gameObject);
                }
            }
        }

        public void OpenMe()
        {
            APIStuff.GetQuickMenuInstance().prop_MenuStateController_0.Method_Public_Void_String_UIContext_Boolean_TransitionType_0(MenuPage.field_Public_String_0);
        }

        public void CloseMe()
        {
            MenuPage.Method_Public_Virtual_New_Void_0();
        }

        public string GetMenuName()
        {
            return MenuName;
        }

        public GameObject GetMenuObject()
        {
            return MenuObject;
        }

        public QMSingleButton GetMainButton()
        {
            return MainButton;
        }

        public GameObject GetBackButton()
        {
            return BackButton;
        }
    }

    public class QMScrollMenu
    {
        public QMNestedButton BaseMenu;
        public QMSingleButton NextButton;
        public QMSingleButton BackButton;
        public UIMenuText IndexText;

        public List<ScrollObject> QMButtons = new();
        private float Posx = 1;
        private float Posy = 0;
        private int Index = 0;
        private Action<QMScrollMenu> OpenAction;
        public int currentMenuIndex = 0;

        public class ScrollObject
        {
            public QMButtonBase ButtonBase;
            public int Index;
        }

        public QMScrollMenu(QMNestedButton btnMenu, float btnXLocation, float btnYLocation, string btnText, Action<QMScrollMenu> MenuOpenAction, string btnToolTip, string MenuTitle, QMButtonAPI.ButtonSize Size = QMButtonAPI.ButtonSize.Default, Sprite Sprite = null)
        {
            BaseMenu = new QMNestedButton(btnMenu, btnXLocation, btnYLocation, btnText, btnToolTip, MenuTitle, Size, Sprite);
            SetAction(MenuOpenAction, true);

            IndexText = new(BaseMenu.GetMenuObject(), $"{currentMenuIndex + 1}/{Index + 1}", new Vector2(175, -435), 39);
            IndexText.text.fontStyle = FontStyle.Bold;

            BackButton = new QMSingleButton(BaseMenu, 1.5f, 3.5f, "<---", () =>
            {
                ShowMenu(currentMenuIndex - 1);
            }, "Go Back", null, QMButtonAPI.ButtonSize.Half);
            NextButton = new QMSingleButton(BaseMenu, 3.5f, 3.5f, "--->", () =>
            {
                ShowMenu(currentMenuIndex + 1);
            }, "Go Next", null, QMButtonAPI.ButtonSize.Half);
        }

        public QMScrollMenu(string btnMenu, float btnXLocation, float btnYLocation, string btnText, Action<QMScrollMenu> MenuOpenAction, string btnToolTip, string MenuTitle, QMButtonAPI.ButtonSize Size = QMButtonAPI.ButtonSize.Default, Sprite Sprite = null)
        {
            BaseMenu = new QMNestedButton(btnMenu, btnXLocation, btnYLocation, btnText, btnToolTip, MenuTitle, Size, Sprite);
            SetAction(MenuOpenAction, true);
            IndexText = new(BaseMenu.GetMenuObject(), $"{currentMenuIndex + 1}/{Index + 1}", new Vector2(175, -435), 39);
            IndexText.text.fontStyle = FontStyle.Bold;

            BackButton = new QMSingleButton(BaseMenu, 1.5f, 3.5f, "<---", () =>
            {
                ShowMenu(currentMenuIndex - 1);
            }, "Go Back", null, QMButtonAPI.ButtonSize.Half);
            NextButton = new QMSingleButton(BaseMenu, 3.5f, 3.5f, "--->", () =>
            {
                ShowMenu(currentMenuIndex + 1);
            }, "Go Next", null, QMButtonAPI.ButtonSize.Half);
        }

        public void ShowMenu(int MenuIndex)
        {
            if (MenuIndex < 0 || MenuIndex > Index) return;
            foreach (ScrollObject scrollObject in QMButtons)
            {
                if (scrollObject.Index == MenuIndex)
                {
                    QMButtonBase buttonBase = scrollObject.ButtonBase;
                    if (buttonBase != null) buttonBase.SetActive(true);
                }
                else
                {
                    QMButtonBase buttonBase2 = scrollObject.ButtonBase;
                    if (buttonBase2 != null) buttonBase2.SetActive(false);
                }
            }
            currentMenuIndex = MenuIndex;
            IndexText.text.text = $"{currentMenuIndex + 1}/{Index + 1}";
        }

        public void SetAction(Action<QMScrollMenu> Open, bool shouldClear = true)
        {
            OpenAction = Open;
            BaseMenu.GetMainButton().SetAction(delegate
            {
                if (shouldClear) Clear();
                OpenAction(this);
                BaseMenu.OpenMe();
                ShowMenu(0);
            });
        }

        public void Refresh()
        {
            Clear();
            OpenAction?.Invoke(this);
            BaseMenu.OpenMe();
            ShowMenu(0);
        }

        public void DestroyMe()
        {
            foreach (ScrollObject scrollObject in QMButtons)
            {
                UnityEngine.Object.Destroy(scrollObject.ButtonBase.GetGameObject());
            }
            QMButtons.Clear();

            if (BaseMenu.GetBackButton() != null) UnityEngine.Object.Destroy(BaseMenu.GetBackButton());
            if (IndexText != null) UnityEngine.Object.Destroy(IndexText.gameObject);
            if (BackButton != null) BackButton.DestroyMe();
            if (NextButton != null) NextButton.DestroyMe();
        }

        public void Clear()
        {
            try
            {
                foreach (ScrollObject scrollObject in QMButtons)
                {
                    UnityEngine.Object.Destroy(scrollObject.ButtonBase.GetGameObject());
                }
                QMButtons.Clear();
                Posx = 1;
                Posy = 0;
                Index = 0;
                currentMenuIndex = 0;
            }
            catch { }
        }

        public void Add(string Text, string Description, Action Action)
        {
            if (Posy == 3)
            {
                Posy = 0;
                Index++;
            }

            QMSingleButton Button = new(BaseMenu, Posx, Posy, Text, Action, Description, null, QMButtonAPI.ButtonSize.Half);

            Button.SetActive(false);
            QMButtons.Add(new ScrollObject
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
    }

    public class QMSlider : QMButtonBase
    {
        private Slider Slider;

        public QMSlider(QMNestedButton location, float posX, float posY, string btnText, string toolTipText, Action<float> Action, float defaultValue = 0, float minValue = 0, float maxValue = 10, QMButtonAPI.SliderSize Size = QMButtonAPI.SliderSize.Default)
        {
            btnQMLoc = location.GetMenuName();
            Initialize(btnText, posX, posY, toolTipText, Action, defaultValue, minValue, maxValue, Size);
        }

        public QMSlider(string location, float posX, float posY, string btnText, string toolTipText, Action<float> Action, float defaultValue = 0, float minValue = 0, float maxValue = 10, QMButtonAPI.SliderSize Size = QMButtonAPI.SliderSize.Default)
        {
            btnQMLoc = location;
            Initialize(btnText, posX, posY, toolTipText, Action, defaultValue, minValue, maxValue, Size);
        }

        private void Initialize(string Title, float posX, float posY, string tooltip, Action<float> onSlide, float defaultValue = 0, float minValue = 0, float maxValue = 10, QMButtonAPI.SliderSize Size = QMButtonAPI.SliderSize.Default)
        {
            button = UnityEngine.Object.Instantiate(APIStuff.SliderButtonTemplate(), GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/" + btnQMLoc).transform, true);
            button.name = $"{QMButtonAPI.Identifier}-Slider-{APIStuff.RandomNextNumber()}";
            switch (Size)
            {
                case QMButtonAPI.SliderSize.Default:
                    button.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 176);
                    break;

                case QMButtonAPI.SliderSize.Double:
                    button.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 176);
                    posX += 0.375f;
                    break;

                case QMButtonAPI.SliderSize.Tripple:
                    button.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 176);
                    posX += 0.75f;
                    break;
            }
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(-68, -228);

            UnityEngine.Object.DestroyImmediate(button.GetComponent<UIInvisibleGraphic>());

            var name = button.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            name.text = Title;

            Slider = button.GetComponentInChildren<Slider>();
            Slider.onValueChanged = new Slider.SliderEvent();
            Slider.onValueChanged.AddListener(new Action<float>(onSlide));
            Slider.minValue = minValue;
            Slider.maxValue = maxValue;
            Slider.value = defaultValue;

            var uiTooltip = button.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>();
            uiTooltip.field_Public_String_0 = tooltip;
            uiTooltip.field_Public_String_1 = tooltip;

            SetLocation(posX, posY);
            Slide(defaultValue, false);
        }

        public void Slide(float value, bool callback = true)
        {
            Slider.Set(value, callback);
        }
    }

    public class QMWingMenu
    {
        public Transform Container;
        public Wing AttachedWing;
        private string MenuName;

        public enum WingState
        {
            Left,
            Right
        }

        public QMWingMenu(string MenuTitle, WingState State = WingState.Left)
        {
            Initialize(MenuTitle, State);
        }

        private void Initialize(string text, WingState State = WingState.Left)
        {
            GameObject GameObject = UnityEngine.Object.Instantiate(APIStuff.GetWingMenuPrefab(), State == WingState.Left ? APIStuff.GetLeftWing().field_Public_RectTransform_0 : APIStuff.GetRightWing().field_Public_RectTransform_0);
            GameObject.name = $"{QMButtonAPI.Identifier}-WingMenu-{APIStuff.RandomNextNumber()}";
            GameObject.SetActive(false);
            RectTransform RectTransform = GameObject.GetComponent<RectTransform>();

            MenuName = GameObject.name;
            AttachedWing = State == WingState.Left ? APIStuff.GetLeftWing() : APIStuff.GetRightWing();

            var headerTransform = RectTransform.GetChild(0);
            var titleText = headerTransform.GetComponentInChildren<TextMeshProUGUI>();
            titleText.text = text;
            titleText.richText = true;

            var backButton = headerTransform.GetComponentInChildren<Button>(true);
            backButton.gameObject.SetActive(true);

            var backIcon = backButton.transform.Find("Icon");
            backIcon.gameObject.SetActive(true);
            var components = new Il2CppSystem.Collections.Generic.List<Behaviour>();
            backButton.GetComponents(components);

            foreach (var comp in components)
            {
                comp.enabled = true;
            }

            var content = RectTransform.GetComponentInChildren<ScrollRect>().content;
            foreach (var obj in content)
            {
                var control = obj.Cast<Transform>();
                if (control == null) continue;

                UnityEngine.Object.Destroy(control.gameObject);
            }

            Container = content;

            var menuStateCtrl = AttachedWing.GetComponent<MenuStateController>();

            var uiPage = GameObject.GetComponent<UIPage>();
            uiPage.field_Public_String_0 = MenuName;
            uiPage.field_Private_Boolean_1 = true;
            uiPage.field_Protected_MenuStateController_0 = menuStateCtrl;
            uiPage.field_Private_List_1_UIPage_0 = new Il2CppSystem.Collections.Generic.List<UIPage>();
            uiPage.field_Private_List_1_UIPage_0.Add(uiPage);

            menuStateCtrl.field_Private_Dictionary_2_String_UIPage_0.Add(uiPage.field_Public_String_0, uiPage);
        }

        public void Open()
        {
            AttachedWing.field_Private_MenuStateController_0.Method_Public_Void_String_UIContext_Boolean_TransitionType_0(MenuName);
        }
    }

    public class QMWingButton
    {
        private Image IconImage;

        public Sprite Sprite
        {
            get => IconImage.sprite;
            set
            {
                if (value != null)
                {
                    IconImage.sprite = value;
                    IconImage.overrideSprite = value;
                }
                IconImage.gameObject.SetActive(value != null);
            }
        }

        public QMWingButton(QMWingMenu Menu, string Text, string Tooltip, Action action, Sprite sprite = null, bool arrow = true, bool background = true, bool separator = false)
        {
            Initialize(Menu.Container, Text, Tooltip, action, sprite, arrow, background, separator);
        }

        public QMWingButton(Transform Parent, string Text, string Tooltip, Action action, Sprite sprite = null, bool arrow = true, bool background = true, bool separator = false)
        {
            Initialize(Parent, Text, Tooltip, action, sprite, arrow, background, separator);
        }

        private void Initialize(Transform Parent, string text, string tooltip, Action onClick, Sprite sprite = null, bool arrow = true, bool background = true, bool separator = false)
        {
            GameObject GameObject = UnityEngine.Object.Instantiate(APIStuff.WingButtonTemplate(), Parent);
            GameObject.name = $"{QMButtonAPI.Identifier}-WingButton-{APIStuff.RandomNextNumber()}";
            RectTransform RectTransform = GameObject.GetComponent<RectTransform>();
            GameObject.SetActive(true);

            var container = RectTransform.Find("Container").transform;
            container.Find("Background").gameObject.SetActive(background);
            container.Find("Icon_Arrow").gameObject.SetActive(arrow);
            RectTransform.Find("Separator").gameObject.SetActive(separator);

            IconImage = container.Find("Icon").GetComponent<Image>();
            Sprite = sprite;

            var tmp = container.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = text;
            tmp.richText = true;

            var button = GameObject.GetComponent<Button>();
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(new Action(onClick));

            var uiTooltip = GameObject.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>();
            uiTooltip.field_Public_String_0 = tooltip;
            uiTooltip.field_Public_String_1 = tooltip;
        }
    }

    public class QMWingToggle
    {
        private QMWingButton Button;
        private Action<bool> ToggleAction;
        private bool State;

        public QMWingToggle(QMWingMenu Menu, string text, string tooltip, Action<bool> onToggle, bool defaultValue = false)
        {
            Initialize(Menu.Container, text, tooltip, onToggle, defaultValue);
        }

        public QMWingToggle(Transform Parent, string text, string tooltip, Action<bool> onToggle, bool defaultValue = false)
        {
            Initialize(Parent, text, tooltip, onToggle, defaultValue);
        }

        private void Initialize(Transform parent, string text, string tooltip, Action<bool> onToggle, bool defaultValue = false)
        {
            ToggleAction = onToggle;
            Button = new QMWingButton(parent, text, tooltip, delegate
            {
                setToggleState(!State);
            }, GetCurrentIcon(), false);
            setToggleState(defaultValue);
        }

        private Sprite GetCurrentIcon()
        {
            return State ? APIStuff.GetOnIconSprite() : APIStuff.GetOffIconSprite();
        }

        public void setToggleState(bool toggle, bool callback = true)
        {
            if (State == toggle) return;
            State = toggle;
            Button.Sprite = GetCurrentIcon();
            if (callback) ToggleAction(State);
        }
    }

    public static class APIStuff
    {
        private static VRC.UI.Elements.QuickMenu QuickMenuInstance;
        private static SelectedUserMenuQM TargetMenuInstance;
        private static GameObject SingleButtonReference;
        private static GameObject TabButtonReference;
        private static GameObject MenuPageReference;
        private static GameObject QMIconButtonReference;
        private static GameObject SliderButtonReference;
        private static GameObject WingButtonReference;
        private static GameObject WingMenuReference;
        private static Sprite OnIconReference;
        private static Sprite OffIconReference;
        private static Wing[] AllWings;
        private static Wing LeftWing;
        private static Wing RightWing;
        private static int ButtonIndex = 0;


        public static VRC.UI.Elements.QuickMenu GetQuickMenuInstance()
        {
            if (QuickMenuInstance == null) QuickMenuInstance = GameObject.Find("UserInterface").GetComponentInChildren<VRC.UI.Elements.QuickMenu>();
            return QuickMenuInstance;
        }

        public static SelectedUserMenuQM GetTargetMenuInstance()
        {
            if (TargetMenuInstance == null) TargetMenuInstance = GetQuickMenuInstance().field_Public_Transform_0.Find("Window/QMParent/Menu_SelectedUser_Local").GetComponent<SelectedUserMenuQM>();
            return TargetMenuInstance;
        }

        public static GameObject SingleButtonTemplate()
        {
            if (SingleButtonReference == null) SingleButtonReference = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_DevTools/Scrollrect/Viewport/VerticalLayoutGroup/Buttons/Button_WarpAllToNewInstance").gameObject;
            return SingleButtonReference;
        }

        public static GameObject WingButtonTemplate()
        {
            if (WingButtonReference == null) WingButtonReference = GetLeftWing().transform.Find("Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup/Button_Profile").gameObject;
            return WingButtonReference;
        }

        public static GameObject GetWingMenuPrefab()
        {
            if (WingMenuReference == null) WingMenuReference = GetLeftWing().field_Public_RectTransform_0.Find("WingMenu").gameObject;
            return WingMenuReference;
        }

        public static GameObject GetQMIconButtonTemplate()
        {
            if (QMIconButtonReference == null) QMIconButtonReference = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/Header_H1/RightItemContainer/Button_QM_Expand").gameObject;
            return QMIconButtonReference;
        }

        public static GameObject GetMenuPageTemplate()
        {
            if (MenuPageReference == null) MenuPageReference = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard").gameObject;
            return MenuPageReference;
        }

        public static GameObject GetTabButtonTemplate()
        {
            if (TabButtonReference == null) TabButtonReference = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/Page_Buttons_QM/HorizontalLayoutGroup/Page_Settings").gameObject;
            return TabButtonReference;
        }

        public static Sprite GetOnIconSprite()
        {
            if (OnIconReference == null) OnIconReference = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Notifications/Panel_NoNotifications_Message/Icon").GetComponent<Image>().sprite;
            return OnIconReference;
        }

        public static Sprite GetOffIconSprite()
        {
            if (OffIconReference == null) OffIconReference = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Settings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UI_Elements_Row_1/Button_ToggleQMInfo/Icon_Off").GetComponent<Image>().sprite;
            return OffIconReference;
        }

        public static GameObject SliderButtonTemplate()
        {
            if (SliderButtonReference == null) SliderButtonReference = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_AudioSettings/Content/Audio/VolumeSlider_Master").gameObject;
            return SliderButtonReference;
        }

        public static int RandomNextNumber()
        {
            return ButtonIndex++;
        }

        public static void DestroyChildren(this Transform transform)
        {
            transform.DestroyChildren(null);
        }

        public static void DestroyChildren(this Transform transform, Func<Transform, bool> exclude)
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                if (exclude == null || exclude(transform.GetChild(i))) UnityEngine.Object.DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        public static Wing[] GetWings()
        {
            if (AllWings == null || AllWings.Length == 0) AllWings = GameObject.Find("UserInterface").GetComponentsInChildren<Wing>(true);
            return AllWings;
        }

        public static Wing GetLeftWing()
        {
            if (LeftWing == null) LeftWing = GetWings().FirstOrDefault(w => w._wingType == VRC.UI.Shared.WingType.Left);
            return LeftWing;
        }

        public static Wing GetRightWing()
        {
            if (RightWing == null) RightWing = GetWings().FirstOrDefault(w => w._wingType == VRC.UI.Shared.WingType.Right);
            return RightWing;
        }
    }
}