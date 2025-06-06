using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LunaR.StyleAPI
{
    public class OverrideStyle
    {
        private static readonly Dictionary<string, Sprite> myOverrideSprites = new();

        private static readonly List<string> Grayscale = new()
        {
            "images/background_layer_01",
            "images/background_layer_02_lines",
            "images/backgroundbottom",
            "images/backgroundoutline",
            "images/backgroundtop",
            "images/button_default",
            "images/button_hover",
            "images/dotted_panel",
            "images/notif_tab_marker",
            "images/notif_tab_marker_active",
            "images/notif_tab_marker_hover",
            "images/page_backdrop",
            "images/page_tab_backdrop",
            "images/page_tab_backdrop_hover",
            "images/panel_2_modal",
            "images/sliderfill",
            "images/tabbottom",
            "images/tableft",
            "images/tabright",
            "images/wing_item_backdrop",
            "images/wing_item_backdrop_hover"
        };

        public static void WarmUpGrayscaleImages()
        {
            foreach (var spriteName in Grayscale)
            {
                var originalSprite = StyleEngineWrapper.TryFindOriginalSprite(spriteName);
                if (originalSprite == null) continue;
                SpriteSnipperUtil.GetGrayscaledSprite(originalSprite, true);
            }
        }

        public static void ApplyOverrides()
        {
            foreach (var spriteName in Grayscale)
            {
                var originalSprite = StyleEngineWrapper.TryFindOriginalSprite(spriteName);
                if (originalSprite == null) continue;
                var grayscaled = SpriteSnipperUtil.GetGrayscaledSprite(originalSprite, true);
                StyleEngineWrapper.OverrideSprite(spriteName, grayscaled);
            }

            foreach (var keyValuePair in myOverrideSprites)
                StyleEngineWrapper.OverrideSprite(keyValuePair.Key, keyValuePair.Value);

            OverridesStyleSheet.ApplyOverrides();
        }

        public static void LoadFromStreams(Dictionary<string, Stream> streamMap, bool closeStreams = false)
        {
            if (streamMap.TryGetValue("overrides.vrcss", out var styleStream)) OverridesStyleSheet.ParseFrom(styleStream.ReadAllLines());

            foreach (var keyValuePair in streamMap)
            {
                if (!keyValuePair.Key.EndsWith(".png")) continue;

                var loadedTexture = StyleExtensions.LoadTexture(keyValuePair.Value);
                if (loadedTexture == null)
                {
                    Extensions.Logger.LogError($"Failed to load a texture from {keyValuePair.Key}");
                    continue;
                }

                loadedTexture.hideFlags |= HideFlags.DontUnloadUnusedAsset; ;

                var rect = new Rect(0, 0, loadedTexture.width, loadedTexture.height);
                var pivot = new Vector2(0.5f, 0.5f);
                var border = new Vector4(0, 0, 0, 0);

                var sprite = Sprite.CreateSprite_Injected(loadedTexture, ref rect, ref pivot, 100, 0, SpriteMeshType.FullRect, ref border, false);
                sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;

                var key = keyValuePair.Key;
                key = key.Substring(0, key.Length - 4).Replace('\\', '/');
                myOverrideSprites[key] = sprite;
            }

            if (closeStreams)
            {
                foreach (var streamMapValue in streamMap.Values)
                    streamMapValue.Dispose();
            }
        }

        public static void LoadFromZip(Stream stream, bool closeStream = false)
        {
            using var zipStream = new ZipArchive(stream, ZipArchiveMode.Read, !closeStream, Encoding.UTF8);
            var entriesDict = zipStream.Entries.ToDictionary(it => it.FullName.ToLower().Replace('\\', '/'), it => it.Open());

            LoadFromStreams(entriesDict, true);
        }
    }
}