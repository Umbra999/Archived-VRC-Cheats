using System;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;

namespace LunaR.UIApi
{
    public class MenuButton
    {
        public MenuButton(GameObject Page, MenuButtonType buttontype, string text, float x_pos, float y_pos, Action listener)
        {
            switch (buttontype)
            {
                case MenuButtonType.PlaylistButton:
                    GameObject original = GameObject.Find("MenuContent/Screens/UserInfo/Buttons/RightSideButtons/RightUpperButtonColumn/PlaylistsButton");
                    Button = UnityEngine.Object.Instantiate(original, original.transform);
                    break;

                case MenuButtonType.AvatarFavButton:
                    GameObject gameObject = GameObject.Find("UserInterface/MenuContent/Screens/Avatar/Favorite Button");
                    Button = UnityEngine.Object.Instantiate(gameObject, gameObject.transform.parent);
                    break;

                case MenuButtonType.ReportButton:
                    GameObject bigbutton = GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo/FavoriteButton");
                    Button = UnityEngine.Object.Instantiate(bigbutton, bigbutton.transform.parent);
                    break;

                case MenuButtonType.UnfriendButton:
                    GameObject largebutton = GameObject.Find("MenuContent/Screens/UserInfo/Buttons/RightSideButtons/RightLowerButtonColumn/UnfriendButton");
                    Button = UnityEngine.Object.Instantiate(largebutton, largebutton.transform.parent);
                    break;
            }

            Button.transform.SetParent(Page.transform);
            Button.GetComponentInChildren<Text>().text = text;
            Button.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            Button.GetComponentInChildren<Button>().onClick.AddListener(listener);
            Button.GetComponentInChildren<Button>().m_Interactable = true;
            Button.GetComponent<RectTransform>().anchoredPosition = new Vector2(x_pos, y_pos);
            foreach (Component component in Button.GetComponentsInChildren(Il2CppType.Of<Image>()))
            {
                if (component.name == "Icon_New") UnityEngine.Object.DestroyImmediate(component);
            }
        }

        public GameObject Button;
    }

    public enum MenuButtonType
    {
        PlaylistButton,
        AvatarFavButton,
        ReportButton,
        UnfriendButton,
    }
}