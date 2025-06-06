using System;
using UnityEngine;
using UnityEngine.UI;
using UnhollowerBaseLib;
using Object = UnityEngine.Object;

namespace WengaPort.Utility
{
    class MenuButton
    {
        public MenuButton(MenuType type, MenuButtonType buttontype, string text, float x_pos, float y_pos, Action listener)
        {
            try
            {
                SettingsPage = GameObject.Find("UserInterface/MenuContent/Screens/Settings");
                SocialPage = GameObject.Find("UserInterface/MenuContent/Screens/Social");
                UserInfoPage = GameObject.Find("UserInterface/MenuContent/Screens/UserInfo");
                AvatarPage = GameObject.Find("UserInterface/MenuContent/Screens/Avatar");
                WorldsPage = GameObject.Find("UserInterface/MenuContent/Screens/Worlds");
                WorldsInfoPage = GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo");

            }
            catch (Exception)
            {
                throw;
            }
            try
            {
                switch (buttontype)
                {
                    case MenuButtonType.PlaylistButton:
                        GameObject original = GameObject.Find("MenuContent/Screens/UserInfo/User Panel/Playlists/PlaylistsButton");
                        Button = Object.Instantiate(original, original.transform);
                        break;

                    case MenuButtonType.AvatarFavButton:
                        GameObject gameObject = GameObject.Find("/UserInterface/MenuContent/Screens/Avatar/Favorite Button");
                        Button = Object.Instantiate(gameObject, gameObject.transform.parent);
                        break;
                    case MenuButtonType.ReportButton:
                        GameObject bigbutton = GameObject.Find("/UserInterface/MenuContent/Screens/WorldInfo/FavoriteButton");
                        Button = Object.Instantiate(bigbutton, bigbutton.transform.parent);
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            try
            {
                switch (type)
                {
                    case MenuType.UserInfo:
                        Button.transform.SetParent(UserInfoPage.transform);
                        break;

                    case MenuType.AvatarMenu:
                        Button.transform.SetParent(AvatarPage.transform);
                        break;

                    case MenuType.SettingsMenu:
                        Button.transform.SetParent(SettingsPage.transform);
                        break;

                    case MenuType.SocialMenu:
                        Button.transform.SetParent(SocialPage.transform);
                        break;

                    case MenuType.WorldMenu:
                        Button.transform.SetParent(WorldsPage.transform);
                        break;
                    case MenuType.WorldInfoMenu:
                        Button.transform.SetParent(WorldsInfoPage.transform);
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }

            // this.Button.GetComponentInChildren<Image>().gameObject.active = false;
            Button.GetComponentInChildren<Text>().text = text;
            Button.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            Button.GetComponentInChildren<Button>().onClick.AddListener(listener);
            Button.GetComponentInChildren<Button>().m_Interactable = true;
            Button.GetComponent<RectTransform>().anchoredPosition = new Vector2(x_pos, y_pos);
            Il2CppReferenceArray<Component> componentsInChildren = Button.GetComponentsInChildren(Image.Il2CppType);
            foreach (Component component in componentsInChildren)
            {
                bool flag2 = component.name == "Icon_New";

                if (flag2)
                    Object.DestroyImmediate(component);
            }
        }

        public GameObject Button;
        public GameObject UserInfoPage;
        public GameObject AvatarPage;
        public GameObject SettingsPage;
        public GameObject SocialPage;
        public GameObject WorldsPage;
        public GameObject WorldsInfoPage;
    }
}
