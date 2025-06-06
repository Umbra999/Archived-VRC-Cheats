using UnityEngine;
using UnityEngine.UI;
using WengaPort.Modules;

namespace WengaPort.Extensions
{
    public class QMInfo
    {
        public QMInfo(Transform Parent, string text, float Pos_X, float Pos_Y, float Scale_X, float Scale_Y, bool infoIcon = true)
        {
            InfoGameObject = Object.Instantiate(Utils.QuickMenu.transform.Find("/UserInterface/QuickMenu/UserIconMenu/Info").gameObject, Parent);
            InfoGameObject.name = $"QMInfo_{Pos_X}_{Pos_Y}";
            InfoIconObject = InfoGameObject.transform.Find("InfoIcon").gameObject;
            TextObject = InfoGameObject.transform.Find("Text").gameObject;
            Image = InfoGameObject.GetComponent<Image>();
            InfoGameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(Pos_X, Pos_Y);
            InfoGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Scale_X, Scale_Y);
            TextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(TextObject.GetComponent<RectTransform>().sizeDelta.x, Scale_Y);
            Text = TextObject.GetComponent<Text>();
            Text.text = text;
            if (!infoIcon)
            {
                InfoIconObject.SetActive(false);
            }
        }
        public void SetText(string text)
        {
            Text.text = text;
        }
        public GameObject TextObject;
        public GameObject InfoIconObject;
        public GameObject InfoGameObject;
        public Text Text;
        public Image Image;
    }
}