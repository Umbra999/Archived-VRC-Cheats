using UnityEngine;
using UnityEngine.UI;

namespace LunaR.UIApi
{
    public class UIMenuText
    {
        public UIMenuText(GameObject parent, string Message, Vector2 position, int size, bool wordWrap = false, TextAnchor textAnchor = TextAnchor.UpperLeft)
        {
            CreateText(parent, Message, position, size, wordWrap, parent.name == "Parent_Loading_Progress", textAnchor);
        }

        public Text text;
        public GameObject gameObject;

        public bool SetText(string Message)
        {
            if (text == null) return false;
            text.text = Message;
            return true;
        }

        private void CreateText(GameObject parent, string Message, Vector2 position, int textsize, bool wordWrap, bool isLoadingScreen, TextAnchor textAnchor)
        {
            gameObject = GetText(parent, isLoadingScreen);
            text = gameObject.GetComponent<Text>();
            text.text = Message;
            text.resizeTextForBestFit = wordWrap;
            text.horizontalOverflow = wordWrap ? HorizontalWrapMode.Wrap : HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.fontSize = textsize;
            text.alignment = textAnchor;
            text.font = Font.GetDefault();
            gameObject.GetComponent<RectTransform>().anchoredPosition = position;
        }

        private GameObject GetText(GameObject parent, bool isLoadingScreen) => Object.Instantiate(GameObject.Find(isLoadingScreen ? "UserInterface/MenuContent/Popups/LoadingPopup/ButtonMiddle/Text" : "UserInterface/MenuContent/Screens/Settings/TitlePanel/TitleText"), parent.transform);
    }
}