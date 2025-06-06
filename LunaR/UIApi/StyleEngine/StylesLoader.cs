using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using VRC.UI.Core.Styles;

namespace LunaR.StyleAPI
{
    public class StylesLoader
    {
        public static IEnumerator WaitForStyleInit()
        {
            while (StyleEngineWrapper.StyleEngine == null)
            {
                yield return null;
                var qmHolder = StyleExtensions.FindInactiveObjectInActiveRoot("UserInterface/Canvas_QuickMenu(Clone)");
                if (qmHolder == null) continue;
                var styleEngine = qmHolder.GetComponent<StyleEngine>();
                if (styleEngine != null) StyleEngineWrapper.StyleEngine = styleEngine;
            }

            while (StyleEngineWrapper.StyleEngine.field_Private_List_1_ElementStyle_0.Count == 0) yield return null;

            StyleEngineWrapper.BackupDefaultStyle();
            ReloadStyles();
            OverrideStyle.WarmUpGrayscaleImages();
            ApplyStyle();
        }

        private static void DoLoadStyle(string styleRawName, System.IO.Stream Stream)
        {
            try
            {
                OverrideStyle.LoadFromZip(Stream!);
            }
            catch (Exception ex)
            {
                Extensions.Logger.LogError($"Can't load style {styleRawName}: {ex}");
            }
        }

        private static void ReloadStyles()
        {
            var resourcePath = $"LunaR.Resources.LunaRStyle.zip";
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath);
            DoLoadStyle(resourcePath, stream);
        }

        private static void ApplyStyle()
        {
            ColorizerManager.UpdateColors();
            OverrideStyle.ApplyOverrides();
            StyleEngineWrapper.UpdateStylesForSpriteOverrides();

            foreach (var styleElement in StyleEngineWrapper.StyleEngine.GetComponentsInChildren<StyleElement>(true))
                styleElement.Method_Protected_Void_0();

            foreach (var styleElement in GameObject.Find("UserInterface/MenuContent").GetComponentsInChildren<StyleElement>(true))
                styleElement.Method_Protected_Void_0();
        }
    }
}