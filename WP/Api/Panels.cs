using UnityEngine;
using UnityEngine.UI;

namespace WengaPort.Api
{
    class Panels
    {

        //Couldve use the default panels that vrchat have behind the shoretcutmenu buttons but hah they just a ugly square
        public static QMSingleButton TextMenu(QMNestedButton parent ,float x, float y, string Txt, float rectx, float recty, string tooltip)
        {
            var NewText = new QMSingleButton(parent, x, y + 0.05f, Txt, null, tooltip);
            NewText.setIntractable(false);
            NewText.getGameObject().GetComponent<RectTransform>().sizeDelta *= new Vector2(rectx, recty);
            var TextComp = NewText.getGameObject().GetComponentInChildren<Text>();
            TextComp.fontSize = 50;
            TextComp.alignment = TextAnchor.UpperCenter;
            return NewText;
        }
    }
}
