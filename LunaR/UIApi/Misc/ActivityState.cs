using UnityEngine;

namespace LunaR.UIApi.Misc
{
    internal class ActivityState
    {
        public GameObject gameObject;
        public ActivityState(string State, Color color, string description, float Offset)
        {
            gameObject = Object.Instantiate(GameObject.Find("UserInterface/MenuContent/Popups/UpdateStatusPopup/Popup/StatusSettings/AskMeStatus"), GameObject.Find("UserInterface/MenuContent/Popups/UpdateStatusPopup/Popup/StatusSettings").transform);
            gameObject.name = $"{State}Status";
            gameObject.transform.localPosition += new Vector3(0, Offset, 0);

            UiStatusIcon Icon = gameObject.transform.Find("StatusIcon").GetComponent<UiStatusIcon>();
            Icon.field_Public_Color32_2 = color;

            I2.Loc.Localize Localize = gameObject.transform.Find("Description").GetComponent<I2.Loc.Localize>();
            Localize.prop_String_0 = description;

            UnityEngine.UI.Text Text = gameObject.transform.Find("Description").GetComponent<UnityEngine.UI.Text>();
            Text.text = description;
        }
    }
}
