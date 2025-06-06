using System;
using UnityEngine;

namespace LunaR.UIApi
{
    public class VRCMenuPage
    {
        public VRCMenuPage(string name, Transform menuPage, Vector2 pos, Action OnPageShown = null, Action OnPageClose = null)
        {
            GameObject SettingsPage = GameObject.Find("UserInterface/MenuContent/Screens/Settings");
            Page = UnityEngine.Object.Instantiate(SettingsPage, SettingsPage.transform.parent);
            Page.name = $"VRCMenuPage={name}";
            VRCUiPage = Page.GetComponent<VRCUiPageSettings>();
            VRCUiPage.enabled = true;
            VRCUiPage.field_Public_Action_0 = new Action(() =>
            {
                Page.active = true;
                Page.SetActive(true);
                VRCUiPage.enabled = true;
                VRCUiPage.field_Protected_Boolean_0 = true;
                OnPageShown?.Invoke();
            });

            VRCUiPage.field_Public_Action_1 = new Action(() =>
            {
                OnPageClose?.Invoke();
            });

            Il2CppSystem.Collections.IEnumerator enumerator = Page.transform.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Il2CppSystem.Object obj = enumerator.Current;
                Transform btnEnum = obj.Cast<Transform>();
                if (!btnEnum.name.ToLower().Contains("depth")) UnityEngine.Object.Destroy(btnEnum.gameObject);
            }

            MenuButton = new MenuButton(GameObject.Find("UserInterface/MenuContent/Backdrop/Header/Tabs/ViewPort"), MenuButtonType.PlaylistButton, name, pos.x, pos.y, () =>
            {
                VRCUiManager.prop_VRCUiManager_0.Method_Public_VRCUiPage_VRCUiPage_0(VRCUiPage);
            });
        }

        public VRCMenuPage(string name, string menuPage, Vector2 pos, Action OnPageShown = null, Action OnPageClose = null) : this(name, GameObject.Find(menuPage).transform, pos, OnPageShown, OnPageClose)
        {
        }

        public string ScreenPath;
        public MenuButton MenuButton;
        public GameObject Page;
        public VRCUiPage VRCUiPage;
    }
}