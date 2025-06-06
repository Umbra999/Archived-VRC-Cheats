using Hexed.HexedServer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hexed.UIApi
{
    public class UIMenuText
    {
        public UIMenuText(GameObject parent, string Message, Vector2 position, int size, bool wordWrap = false, TextAnchor textAnchor = TextAnchor.UpperLeft)
        {
            CreateText(parent, Message, position, size, wordWrap, textAnchor);
        }

        public Text text;
        public GameObject TextObject;

        public void SetText(string Message)
        {
            if (text != null) text.text = Message;
        }

        private void CreateText(GameObject parent, string Message, Vector2 position, int textsize, bool wordWrap, TextAnchor textAnchor)
        {
            TextObject = UnityEngine.Object.Instantiate(Resources.FindObjectsOfTypeAll<Text>().FirstOrDefault().gameObject, parent.transform); // set object to 0, 0 to prevent switched objects needing adjusments all the time one day
            foreach (var x in TextObject.GetComponents<Behaviour>())
            {
                if (x != TextObject.GetComponent<Text>()) UnityEngine.Object.DestroyImmediate(x);
            }
            TextObject.name = $"Hexed_UIText_{EncryptUtils.RandomString(8)}";
            text = TextObject.GetComponent<Text>();
            text.text = Message;
            text.resizeTextForBestFit = wordWrap;
            text.horizontalOverflow = wordWrap ? HorizontalWrapMode.Wrap : HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.fontSize = textsize;
            text.alignment = textAnchor;
            text.supportRichText = true;
            text.font = Font.GetDefault();

            TextObject.transform.localPosition = position;
        }
    }

    public class UITextMeshText
    {
        public UITextMeshText(GameObject parent, string Message, Vector2 position, int size, bool wordWrap = false, TextAlignmentOptions textAnchor = TextAlignmentOptions.TopLeft)
        {
            CreateText(parent, Message, position, size, wordWrap, textAnchor);
        }

        public TextMeshProUGUI text;
        public GameObject TextObject;

        public void SetText(string Message)
        {
            text.text = Message;
        }

        private void CreateText(GameObject parent, string Message, Vector2 position, int textsize, bool wordWrap, TextAlignmentOptions textAnchor)
        {
            TextObject = UnityEngine.Object.Instantiate(MenuHelper.QuickMenu.transform.Find("CanvasGroup/Container/Window/Panel_QM_Widget/Panel_QM_DebugInfo/Panel/Text_Ping").gameObject, parent.transform);
            foreach (var x in TextObject.GetComponents<Behaviour>())
            {
                if (x != TextObject.GetComponent<TextMeshProUGUI>()) UnityEngine.Object.DestroyImmediate(x);
            }
            TextObject.name = $"Hexed_UIText_{EncryptUtils.RandomString(12)}";
            text = TextObject.GetComponent<TextMeshProUGUI>();
            text.name = TextObject.name;
            text.text = Message;
            text.richText = true;
            text.fontStyle = FontStyles.Bold;
            text.enableWordWrapping = wordWrap;
            text.autoSizeTextContainer = false;
            text.fontSize = textsize;
            text.fontSizeMax = textsize;
            text.fontSizeMin = textsize;
            text.alignment = textAnchor;
            text.enableKerning = false;

            TextObject.GetComponent<RectTransform>().localPosition = position;
            TextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 0);
        }
    }
}
