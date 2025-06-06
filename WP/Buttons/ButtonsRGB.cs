using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WengaPort.Buttons
{
    public class ButtonsRGB
    {
        static float timer = 0.5f;
        static float r = 0, g = 0, b = 1;

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
        static List<Button> quickmenuBtn = new List<Button>();
        static List<Renderer> Renderers = new List<Renderer>();

        public static void Initialize()
        {
            {
                try
                {
                    if (timer <= 0)
                    {
                        if (quickmenuStuff.Count == 0 || quickmenuBtn.Count == 0) Buttons();
                        if (b > 0 && r <= 0)
                        {
                            b -= 0.025f;
                            g += 0.025f;
                        }
                        else if (g > 0)
                        {
                            g -= 0.025f;
                            r += 0.025f;
                        }
                        else if (r > 0)
                        {
                            r -= 0.025f;
                            b += 0.025f;
                        }
                        Color rainbow = new Color(r, g, b);
                        Color rainbow2 = new Color(r, g, b, 0.6f);
                        foreach (Image btn in quickmenuStuff)
                        {
                            try
                            {
                                btn.color = rainbow2;
                            }
                            catch { }
                        }
                        foreach (Button btn in quickmenuBtn)
                        {
                            try
                            {
                                btn.colors = new ColorBlock()
                                {
                                    colorMultiplier = 1f,
                                    disabledColor = Color.grey,
                                    highlightedColor = rainbow * 1.5f,
                                    normalColor = rainbow / 1.5f,
                                    pressedColor = Color.grey * 1.5f
                                };
                            }
                            catch { }
                        }
                        timer = 0.025f;
                    }
                    timer -= Time.deltaTime;
                }
                catch { }
            }
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
                        if (btn.GetComponentInChildren<CanvasRenderer>())
                        {
                            foreach (Image img in btn.GetComponentsInChildren<Image>(true))
                            {
                                quickmenuStuff.Add(img);
                            }
                        }
                    }
                    catch { }
                }

                foreach (Button btn in UserInterface.GetComponentsInChildren<Button>(true))
                {
                    try
                    {
                        quickmenuBtn.Add(btn);
                        if (btn.GetComponentInChildren<CanvasRenderer>())
                        {
                            foreach (Image img in btn.GetComponentsInChildren<Image>(true))
                            {
                                quickmenuStuff.Add(img);
                            }
                        }
                    }
                    catch { }
                }
            }
            catch { }
        }
    }
}
