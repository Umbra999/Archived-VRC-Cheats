using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WengaPort.Buttons;

namespace WengaPort.Api
{
    public static class ReworkedButtonAPI
    {
        public static List<object> ButtonOutput = new List<object>();
        public static Image Img;
        public static List<Button> SpriteButton = new List<Button>();
        public static List<Image> SpritebtnImage = new List<Image>();
        public static IEnumerator CreateButton(MenuType Menu, ButtonType type, string ButtonText, float X, float Y, System.Action action, string buttonTooltip, Color color, Color BtnText, QMNestedButton MainButton = null, string SpriteImage = null, string OnText = null, string OffText = null, UnityAction OffAction = null, Color? BackButtonColor = null, Color? BackButtonTextColor = null)
        {
            var shortcutmenu = GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu");
            while (shortcutmenu.active != true) yield return null;
            string MenuString = "ShortcutMenu";

            switch (Menu)
            {
                default: MenuString = "ShortcutMenu"; break;
                case MenuType.ShortCut: MenuString = "ShortcutMenu"; break;
                case MenuType.UserInteract: MenuString = "UserInteractMenu"; break;
                case MenuType.UserInfo: MenuString = "UserInfo"; break;
            }

            switch (type)
            {
                default: break;
                case ButtonType.Single:
                    QMSingleButton x = null;
                    if (MainButton != null)
                    {
                        x = new QMSingleButton(MainButton, X, Y, ButtonText, action, buttonTooltip, color, BtnText);
                    }
                    else
                    {
                        x = new QMSingleButton(MenuString, X, Y, ButtonText, action, buttonTooltip, color, BtnText);
                    }

                    if (SpriteImage != null)
                    {
                        var image = new Sprite();
                        WWW www = new WWW(SpriteImage);
                        yield return www;
                        {
                            image = Sprite.CreateSprite(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0), 100 * 1000, 1000, SpriteMeshType.FullRect, Vector4.zero, false);
                            x.getGameObject().GetComponent<Image>().sprite = image;
                            SpriteButton.Add(x.getGameObject().GetComponent<Button>());
                            SpritebtnImage.Add(x.getGameObject().GetComponent<Image>());
                            foreach (Image img in SpritebtnImage)
                            {
                                try
                                {
                                    img.color = Color.white;
                                }
                                catch { }
                            }

                            foreach (Button btn in SpriteButton)
                            {
                                btn.colors = new ColorBlock()
                                {
                                    colorMultiplier = 2,
                                    disabledColor = Color.white,
                                    highlightedColor = Color.white,
                                    normalColor = Color.white,
                                    pressedColor = Color.white
                                };
                            }
                        }
                    }
                    ButtonOutput.Add(x);
                    break;
            }
        }
    }
}
