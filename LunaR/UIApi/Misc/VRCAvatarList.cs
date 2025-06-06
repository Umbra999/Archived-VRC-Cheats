using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

namespace LunaR.Api
{
    public class VRCAvatarList
    {
        public VRCAvatarList(Transform parent, string name, int Position = 0)
        {
            GameObject List = GameObject.Find("/UserInterface/MenuContent/Screens/Avatar/Vertical Scroll View/Viewport/Content/Public Avatar List");
            GameObject = Object.Instantiate(List.gameObject, parent);
            GameObject.GetComponent<UiAvatarList>().field_Public_Category_0 = UiAvatarList.Category.SpecificList;
            UiVRCList = GameObject.GetComponent<UiVRCList>();
            Text = GameObject.transform.Find("Button").GetComponentInChildren<Text>();
            GameObject.transform.SetSiblingIndex(Position);

            UiVRCList.clearUnseenListOnCollapse = false;
            UiVRCList.usePagination = false;
            UiVRCList.hideElementsWhenContracted = false;
            UiVRCList.hideWhenEmpty = false;
            UiVRCList.field_Protected_Dictionary_2_Int32_List_1_ApiModel_0.Clear();

            GameObject.SetActive(true);
            GameObject.name = name;
            Text.text = name;
        }

        public GameObject GameObject;
        public UiVRCList UiVRCList;
        public Text Text;

        public void RenderElement(Il2CppSystem.Collections.Generic.List<ApiAvatar> AvatarList)
        {
            UiVRCList.Method_Protected_Void_List_1_T_Int32_Boolean_VRCUiContentButton_0(AvatarList, 0, true);
        }
    }
}