using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using WengaPort.Extensions;
using WengaPort.Modules;

namespace WengaPort.Buttons
{
    class ConsoleMenu
    {
        internal static QMInfo QMInfo;

        public static string ConsoleUrl = "https://i.imgur.com/el2Lwa8.png"; //This is a 1650 x 900 png

        public static void Initialize()
        {
            QMInfo = new QMInfo(Utils.QuickMenu.transform.Find("ShortcutMenu"), "", 0, 380, 1660, 930, false);
            QMInfo.Text.color = Color.white;
            QMInfo.Text.alignment = TextAnchor.LowerLeft;
            QMInfo.Text.supportRichText = true;
            QMInfo.Text.fontSize = 42;
            QMInfo.Text.fontStyle = FontStyle.Bold;
            QMInfo.Text.resizeTextForBestFit = false;
            LoadSprite(QMInfo.Image, ConsoleUrl);
        }

        internal static void LoadSprite(Image Instance, string url)
        {
            MelonLoader.MelonCoroutines.Start(LoadSpriteEnum(Instance, url));
        }
        private static IEnumerator LoadSpriteEnum(Image Instance, string url)
        {
            while (VRCPlayer.field_Internal_Static_VRCPlayer_0 != true) yield return null;
            var Sprite = new Sprite();
            WWW www = new WWW(url);
            yield return www;
            {
                Sprite = Sprite.CreateSprite(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0), 100 * 1000, 1000, SpriteMeshType.FullRect, Vector4.zero, false);
            }
            Instance.sprite = Sprite;
            Instance.color = Color.white;
            yield break;
        }
    }
}
