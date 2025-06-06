using System.Linq;
using UnityEngine;
using VRC.UI.Elements;
using VRC.UI.Elements.Controls;
using VRC.UI.Elements.Menus;

namespace Hexed.UIApi
{
    public static class MenuHelper
    {
        private static GameObject _menuPageTemplate;
        public static GameObject menuPageTemplate
        {
            get
            {
                if (_menuPageTemplate == null) _menuPageTemplate = QuickMenu.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_Dashboard").gameObject;
                return _menuPageTemplate;
            }
        }

        private static GameObject _iconButtonTemplate;
        public static GameObject iconButtonTemplate
        {
            get
            {
                if (_iconButtonTemplate == null) _iconButtonTemplate = QuickMenu.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_Dashboard/Header_H1/RightItemContainer/Button_QM_Expand").gameObject;
                return _iconButtonTemplate;
            }
        }

        private static GameObject _singleButtonTemplate;
        public static GameObject singleButtonTemplate
        {
            get
            {
                if (_singleButtonTemplate == null) _singleButtonTemplate = QuickMenu.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickLinks/Button_Worlds").gameObject;
                return _singleButtonTemplate;
            }
        }

        private static GameObject _mainBigSingleButtonTemplate;
        public static GameObject mainBigSingleButtonTemplate
        {
            get
            {
                if (_mainBigSingleButtonTemplate == null) _mainBigSingleButtonTemplate = MainMenu.transform.Find("Container/MMParent/Menu_WorldDetail/ScrollRect/Viewport/VerticalLayoutGroup/Actions/ViewOnWebsite").gameObject;
                return _mainBigSingleButtonTemplate;
            }
        }

        private static GameObject _mainMediumSingleButtonTemplate;
        public static GameObject mainMediumSingleButtonTemplate
        {
            get
            {
                if (_mainMediumSingleButtonTemplate == null) _mainMediumSingleButtonTemplate = MainMenu.transform.Find("Container/MMParent/Menu_UserDetail/ScrollRect/Viewport/VerticalLayoutGroup/Row3/CellGrid_MM_Content/AddANote").gameObject;
                return _mainMediumSingleButtonTemplate;
            }
        }

        private static GameObject _mainSmallSingleButtonTemplate;
        public static GameObject mainSmallSingleButtonTemplate
        {
            get
            {
                if (_mainSmallSingleButtonTemplate == null) _mainSmallSingleButtonTemplate = MainMenu.transform.Find("Container/MMParent/Menu_Avatars/Menu_MM_DynamicSidePanel/Panel_SectionList/ScrollRect_Navigation_Container/ScrollRect_Content/Panel_SelectedAvatar/ScrollRect/Viewport/VerticalLayoutGroup/Button_AvatarDetails").gameObject;
                return _mainSmallSingleButtonTemplate;
            }
        }

        public static MenuStateController _menuStateController;
        public static MenuStateController menuStateController
        {
            get
            {
                if (_menuStateController == null) _menuStateController = QuickMenu.GetComponent<MenuStateController>();
                return _menuStateController;
            }
        }
        private static VRC.UI.Elements.QuickMenu _QuickMenu;
        public static VRC.UI.Elements.QuickMenu QuickMenu
        {
            get
            {
                if (_QuickMenu == null) _QuickMenu = Resources.FindObjectsOfTypeAll<VRC.UI.Elements.QuickMenu>()?.FirstOrDefault(x => x.gameObject?.name == "Canvas_QuickMenu(Clone)");
                return _QuickMenu;
            }
        }

        private static MainMenu _MainMenu;
        public static MainMenu MainMenu
        {
            get
            {
                if (_MainMenu == null) _MainMenu = Resources.FindObjectsOfTypeAll<MainMenu>()?.FirstOrDefault(x => x.gameObject?.name == "Canvas_MainMenu(Clone)");
                return _MainMenu;
            }
        }

        private static Transform _userInterface;
        public static Transform userInterface
        {
            get
            {
                if (_userInterface == null) _userInterface = QuickMenu?.transform?.parent;
                return _userInterface;
            }
        }

        private static Transform _selectedMenuTemplate;
        public static Transform selectedMenuTemplate
        {
            get
            {
                if (_selectedMenuTemplate == null) _selectedMenuTemplate = QuickMenu.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_SelectedUser_Local");
                return _selectedMenuTemplate;
            }
        }

        private static SelectedUserMenuQM _selectedUserMenu;
        public static SelectedUserMenuQM selectedUserMenu
        {
            get
            {
                if (_selectedUserMenu == null) _selectedUserMenu = selectedMenuTemplate.GetComponent<SelectedUserMenuQM>();
                return _selectedUserMenu;
            }
        }

        private static GameObject _sliderButtonTemplate;
        public static GameObject sliderButtonTemplate
        {
            get
            {
                if (_sliderButtonTemplate == null) _sliderButtonTemplate = QuickMenu.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_AudioSettings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/Audio/VolumeSlider_Master").gameObject;
                return _sliderButtonTemplate;
            }
        }

        private static Transform _wingMenuTemplate;
        public static Transform wingMenuTemplate
        {
            get
            {
                if (_wingMenuTemplate == null) _wingMenuTemplate = leftWing.transform.Find("Container/InnerContainer/WingMenu");
                return _wingMenuTemplate;
            }
        }

        private static Transform _wingButtonTemplate;
        public static Transform wingButtonTemplate
        {
            get
            {
                if (_wingButtonTemplate == null) _wingButtonTemplate = leftWing.transform.Find("Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup/Button_Profile");
                return _wingButtonTemplate;
            }
        }

        private static WingMenu[] _wings;
        public static WingMenu[] wings
        {
            get
            {
                if (_wings == null || _wings.Length == 0) _wings = QuickMenu.transform.Find("CanvasGroup/Container").GetComponentsInChildren<WingMenu>(true);
                return _wings;
            }
        }

        private static WingMenu _leftWing;
        public static WingMenu leftWing
        {
            get
            {
                if (_leftWing == null) _leftWing = wings.FirstOrDefault(w => w._wingType == WingType.Left);
                return _leftWing;
            }
        }

        private static WingMenu _rightWing;
        public static WingMenu rightWing
        {
            get
            {
                if (_rightWing == null) _rightWing = wings.FirstOrDefault(w => w._wingType == WingType.Right);
                return _rightWing;
            }
        }
    }
}
