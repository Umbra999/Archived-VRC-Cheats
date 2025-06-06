using Hexed.UIApi;
using Hexed.Wrappers;
using UnityEngine;

namespace Hexed.CustomUI.MainMenu
{
    internal class UserMenu
    {
        private static Transform UserPage;
        public static void Init()
        {
            UserPage = MenuHelper.MainMenu.transform.Find("Container/MMParent/Menu_UserDetail/ScrollRect/Viewport/VerticalLayoutGroup/Row3/CellGrid_MM_Content");

            new MMSingleButton(UserPage, 0, 0, "Copy ID", delegate
            {
                IUser SelectedUser = MenuHelper.MainMenu.transform.Find("Container/MMParent/Menu_UserDetail").GetComponent<VRC.UI.Elements.Menus.MainMenuSelectedUser>().field_Private_IUser_0;
                if (SelectedUser == null) return;
                GeneralUtils.CopyToClipboard($"{SelectedUser.prop_String_0}");
            }, "Copy the UserID", ButtonAPI.MMButtonType.Medium, UnityUtils.GetSprite("History"));

            new MMSingleButton(UserPage, 0, 0, "Copy Name", delegate
            {
                IUser SelectedUser = MenuHelper.MainMenu.transform.Find("Container/MMParent/Menu_UserDetail").GetComponent<VRC.UI.Elements.Menus.MainMenuSelectedUser>().field_Private_IUser_0;
                if (SelectedUser == null) return;
                GeneralUtils.CopyToClipboard($"{SelectedUser.prop_String_1}");
            }, "Copy the Name", ButtonAPI.MMButtonType.Medium, UnityUtils.GetSprite("History"));
        }
    }
}
