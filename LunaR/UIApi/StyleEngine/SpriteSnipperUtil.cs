using System;
using UnhollowerBaseLib;
using UnityEngine;

namespace LunaR.StyleAPI
{
    public class SpriteSnipperUtil
    {
        private static void ProcessPixelsToGrayscaleNormalized(Il2CppStructArray<Color> pixels)
        {
            var scaleFactor = 0f;
            for (var i = 0; i < pixels.Count; i++)
            {
                var pixel = pixels[i];
                if (pixel.a == 0) continue;

                var g = pixel.Grayscale();
                if (g > scaleFactor) scaleFactor = g;
            }

            for (var i = 0; i < pixels.Count; i++)
            {
                var pixel = pixels[i];
                var g = pixel.Grayscale() / scaleFactor;
                pixels[i] = new Color { r = g, g = g, b = g, a = pixel.a };
            }
        }

        private static void ProcessPixelsToGrayscale(Il2CppStructArray<Color> pixels)
        {
            for (var i = 0; i < pixels.Count; i++)
            {
                var pixel = pixels[i];
                var g = pixel.Grayscale();
                pixels[i] = new Color { r = g, g = g, b = g, a = pixel.a };
            }
        }

        public static Sprite GetGrayscaledSprite(Sprite original, bool normalizeWhite)
        {
            var newTexture = Copy(EnsureReadable(original.texture), original.textureRect, normalizeWhite ? ProcessPixelsToGrayscaleNormalized : ProcessPixelsToGrayscale);

            newTexture.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            var rect = new Rect(0, 0, newTexture.width, newTexture.height);
            var pivot = original.pivot / original.rect.size;
            var border = original.border;

            var newSprite = Sprite.CreateSprite_Injected(newTexture, ref rect, ref pivot, original.pixelsPerUnit, 0, SpriteMeshType.FullRect, ref border, false);

            newSprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            return newSprite;
        }

        private static Texture2D EnsureReadable(Texture2D source)
        {
            if (source.isReadable) return source;
            var newTexture = ForceReadTexture(source);
            newTexture.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return newTexture;
        }

        public static Texture2D Copy(Texture2D orig, Rect rect, Action<Il2CppStructArray<Color>>? processPixels = null)
        {
            if (!orig.isReadable) orig = ForceReadTexture(orig);

            var pixels = orig.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
            var newTex = new Texture2D((int)rect.width, (int)rect.height);
            processPixels?.Invoke(pixels);
            newTex.SetPixels(pixels);
            newTex.Apply();
            return newTex;
        }

        public static Texture2D ForceReadTexture(Texture2D tex)
        {
            var origFilter = tex.filterMode;
            tex.filterMode = FilterMode.Point;

            var rt = RenderTexture.GetTemporary(tex.width, tex.height, 0, RenderTextureFormat.ARGB32);
            rt.filterMode = FilterMode.Point;
            RenderTexture.active = rt;

            Graphics.Blit2(tex, rt);

            var newTex = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);

            newTex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
            newTex.Apply(false, false);

            RenderTexture.active = null;
            tex.filterMode = origFilter;

            RenderTexture.ReleaseTemporary(rt);

            return newTex;
        }
    }
}