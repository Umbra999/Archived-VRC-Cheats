using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WengaPort.Buttons
{
    class ButtonsMainColor
    {

        public static List<Color> colors = new List<Color>()
        {
            Color.red,
            Color.magenta,
            Color.blue,
            Color.black,
            Color.green,
            Color.yellow,
            Color.white,
            Color.cyan,
            Color.gray,
        };

        static List<Image> quickmenuStuff = new List<Image>();
        static List<Image> ScreenPage = new List<Image>();
        static List<Text> ScreenText = new List<Text>();
        static List<Button> ScreenButton = new List<Button>();
        static List<Button> quickmenuBtn = new List<Button>();
        static List<Text> quickmenuTxt = new List<Text>();
        static List<Renderer> Renderers = new List<Renderer>();

        public static void Initialize() //Buttons color
        {
            {
                try
                {
                    if (quickmenuStuff.Count == 0 || quickmenuBtn.Count == 0) Buttons();

                    Color BaseColor;
                    ColorUtility.TryParseHtmlString("#2EFFE1", out BaseColor);

                    Color PressedColor;
                    ColorUtility.TryParseHtmlString("#000000", out PressedColor);

                    Color Higlightedcolor;
                    ColorUtility.TryParseHtmlString("#000000", out Higlightedcolor);

                    Color TextColor;
                    ColorUtility.TryParseHtmlString("#00fff7", out TextColor);


                    foreach (Image btn in quickmenuStuff)
                    {
                        try
                        {
                            btn.color = BaseColor;
                        }
                        catch { }
                    }
                    foreach (Button btn in quickmenuBtn)
                    {
                        try
                        {
                            btn.colors = new ColorBlock()
                            {
                                colorMultiplier = 2,
                                disabledColor = Color.black,
                                highlightedColor = Higlightedcolor * 2,
                                normalColor = BaseColor,
                                pressedColor = PressedColor,
                                fadeDuration = 0.4f
                            };
                        }
                        catch { }
                    }

                    foreach (Text btn in quickmenuTxt)
                    {
                        try
                        {
                            btn.color = TextColor;
                        }
                        catch { }
                    }
                }
                catch { }
            }
        }

        public static void Initialize2() //Screen colors (social page and stuff)
        {
            {
                try
                {
                    if (ScreenPage.Count == 0) Screen();

                    Color SecColor;
                    ColorUtility.TryParseHtmlString("#4a4a4a", out SecColor);

                    Color ScreenTextColor;
                    ColorUtility.TryParseHtmlString("#00fff7", out ScreenTextColor);

                    Color ScreenButtonColor;
                    ColorUtility.TryParseHtmlString("#00fff7", out ScreenButtonColor);

                    Color Higlightedcolor;
                    ColorUtility.TryParseHtmlString("#00f57f", out Higlightedcolor);

                    Color PressedColor;
                    ColorUtility.TryParseHtmlString("#000000", out PressedColor);

                    Color BaseColor;
                    ColorUtility.TryParseHtmlString("#00fff7", out BaseColor);

                    foreach (Image btn in ScreenPage)
                    {
                        try
                        {
                            btn.color = SecColor / 2f;
                        }
                        catch { }
                    }
                    foreach (Text btn in ScreenText)
                    {
                        try
                        {
                            btn.color = ScreenTextColor;
                        }
                        catch { }
                    }

                    //Screen buttons
                    foreach (Button btn in ScreenButton)
                    {
                        try
                        {
                            btn.colors = new ColorBlock()
                            {
                                colorMultiplier = 5,
                                disabledColor = PressedColor / 3,
                                highlightedColor = Higlightedcolor * 4,
                                normalColor = BaseColor,
                                pressedColor = PressedColor
                            };
                        }
                        catch { }
                    }
                }
                catch { }
            }
        }

        public static void Initialize3()
        {
            try
            {
                GameObject LoadingSkybox = GameObject.Find("SkyCube_Baked");
                if (LoadingSkybox != null)
                {
                    MeshRenderer Mat = LoadingSkybox.GetComponent<MeshRenderer>();
                    Mat.enabled = false;
                }
            }
            catch { }
        }

        private static void Screen()
        {
            try
            {
                GameObject UserInterface = GameObject.Find("/UserInterface/MenuContent");
                foreach (CanvasRenderer btn in UserInterface.GetComponentsInChildren<CanvasRenderer>(true))
                {
                    try
                    {
                        if (btn.GetComponent<Image>())
                        {
                            ScreenPage.Add(btn.GetComponent<Image>());
                        }

                        if (btn.GetComponent<Text>())
                        {
                            ScreenText.Add(btn.GetComponent<Text>());
                        }

                        if (btn.GetComponent<Button>())
                        {
                            ScreenButton.Add(btn.GetComponent<Button>());
                        }
                    }
                    catch { }
                }

            }
            catch { }
        }


        private static void Buttons()
        {
            try
            {
                GameObject UserInterface = GameObject.Find("/UserInterface/MenuContent");
                GameObject VoiceDot = GameObject.Find("/UserInterface/UnscaledUI/HudContent/Hud/VoiceDotParent/VoiceDot");
                GameObject VoiceDotDisabled = GameObject.Find("/UserInterface/UnscaledUI/HudContent/Hud/VoiceDotParent/VoiceDotDisabled");

                foreach (Button btn in QuickMenu.prop_QuickMenu_0.GetComponentsInChildren<Button>(true))
                {
                    try
                    {
                        quickmenuBtn.Add(btn);
                        if (btn.GetComponentInChildren<RectTransform>())
                        {
                            foreach (Image img in btn.GetComponentsInChildren<Image>(true))
                            {
                                quickmenuStuff.Add(img);
                            }
                        }

                    }
                    catch { }
                }

                foreach (Text btn in QuickMenu.prop_QuickMenu_0.GetComponentsInChildren<Text>(true))
                {
                    try
                    {
                        quickmenuTxt.Add(btn);
                        if (btn.GetComponentInChildren<RectTransform>())
                        {
                            foreach (Image img in btn.GetComponentsInChildren<Image>(true))
                            {
                                quickmenuStuff.Add(img);
                            }
                        }

                    }
                    catch { }
                }

                foreach (CanvasRenderer btn in VoiceDot.GetComponentsInChildren<CanvasRenderer>(true))
                {
                    try
                    {
                        if (btn.GetComponent<Image>())
                        {
                            quickmenuStuff.Add(btn.GetComponent<Image>());
                        }
                    }
                    catch { }
                }
                foreach (CanvasRenderer btn in VoiceDotDisabled.GetComponentsInChildren<CanvasRenderer>(true))
                {
                    try
                    {
                        if (btn.GetComponent<Image>())
                        {
                            quickmenuStuff.Add(btn.GetComponent<Image>());
                        }
                    }
                    catch { }
                }
            }
            catch { }
        }
    }
}
