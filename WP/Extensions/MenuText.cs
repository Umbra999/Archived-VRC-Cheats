using UnityEngine;
using UnityEngine.UI;
using WengaPort.Modules;

namespace WengaPort.Api
{
    class MenuText
    {
        public MenuText(QMNestedButton menuBase, float posx, float poxy, string text)
        {
            menuTitle = UnityEngine.Object.Instantiate(Utils.QuickMenu.transform.Find("QuickMenu_NewElements/_InfoBar/EarlyAccessText").gameObject, menuBase.getBackButton().getGameObject().transform.parent);
            menuTitle.GetComponent<Text>().fontStyle = FontStyle.Normal;
            menuTitle.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            menuTitle.GetComponent<Text>().text = text;
            menuTitle.GetComponent<Text>().fontSize = 50;
            menuTitle.GetComponent<RectTransform>().anchoredPosition = new Vector2(posx, -poxy);
            menuTitle.GetComponent<Text>().color = Color.white;
            Posx = posx;
            Posy = -poxy;
            Text = text;
            menuTitle.name = string.Format("MenuText_{0}_{1}_{2}", text, posx, -Posy);
        }

        public MenuText(string MenuName, float posx, float poxy, string text)
        {
            menuTitle = UnityEngine.Object.Instantiate(Utils.QuickMenu.transform.Find("QuickMenu_NewElements/_InfoBar/EarlyAccessText").gameObject, Utils.QuickMenu.transform.Find(MenuName));
            menuTitle.GetComponent<Text>().fontStyle = FontStyle.Normal;
            menuTitle.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            menuTitle.GetComponent<Text>().text = text;
            menuTitle.GetComponent<Text>().fontSize = 50;
            menuTitle.GetComponent<RectTransform>().anchoredPosition = new Vector2(posx, -poxy);
            menuTitle.GetComponent<Text>().color = Color.white;
            Posx = posx;
            Posy = -poxy;
            Text = text;
            menuTitle.name = string.Format("MenuText_{0}_{1}_{2}", text, posx, -Posy);
        }

        public MenuText(Transform parent, float posx, float poxy, string text)
        {
            menuTitle = UnityEngine.Object.Instantiate(Utils.QuickMenu.transform.Find("QuickMenu_NewElements/_InfoBar/EarlyAccessText").gameObject, parent);
            menuTitle.GetComponent<Text>().fontStyle = FontStyle.Normal;
            menuTitle.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            menuTitle.GetComponent<Text>().text = text;
            menuTitle.GetComponent<Text>().fontSize = 50;
            menuTitle.GetComponent<RectTransform>().anchoredPosition = new Vector2(posx, -poxy);
            menuTitle.GetComponent<Text>().color = Color.white;
            Posx = posx;
            Posy = -poxy;
            Text = text;
            menuTitle.name = string.Format("MenuText_{0}_{1}_{2}", text, posx, -Posy);
        }

        public void setactive(bool value)
        {
            menuTitle.SetActive(value);
        }

        public void Delete()
        {
            UnityEngine.Object.Destroy(menuTitle);
        }

        public void SetText(string text)
        {
            menuTitle.GetComponent<Text>().text = text;
        }

        public void SetPos(float x, float y)
        {
            Posy = y;
            Posx = x;
            menuTitle.GetComponent<RectTransform>().anchoredPosition = new Vector2(Posx, -Posy);
        }

        public void SetColor(float r, float g, float b, float a)
        {
            menuTitle.GetComponent<Text>().color = new Color(r, g, b, a);
        }

        public void SetColor(Color color)
        {
            menuTitle.GetComponent<Text>().color = color;
        }

        public void SetFontSize(int size)
        {
            menuTitle.GetComponent<Text>().fontSize = size;
        }

        public GameObject menuTitle;

        public float Posx;

        public float Posy;

        public string Text;
    }
}
