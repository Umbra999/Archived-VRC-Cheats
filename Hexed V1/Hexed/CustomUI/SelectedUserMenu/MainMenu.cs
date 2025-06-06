using Hexed.UIApi;
using Hexed.Wrappers;
using UnityEngine;

namespace Hexed.CustomUI.SelectedUserMenu
{
    internal class MainMenu
    {
        public static QMMenuPage ClientPage;
        public static UITextMeshText InfoText;

        public static void Init()
        {
            ClientPage = new("Hexed");

            new QMIconButton(MenuHelper.selectedMenuTemplate, 1.25f, -0.8f, ClientPage.OpenMe, "Hexed Client", UnityUtils.GetSprite("moon"));

            UtilsMenu.Init();
            AvatarMenu.Init();
            ExploitMenu.Init();
            BotMenu.Init();

            InfoText = new UITextMeshText(ClientPage.MenuObject, "Informations", new Vector2(-440, 155), 29, false);
            InfoText.text.color = Color.gray;
        }
    }
}
