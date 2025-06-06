using System.Reflection;
using UnityEngine;

namespace Hexed.Wrappers
{
    internal static class UnityUtils
    {
        public static bool IsBadPosition(Vector3 v3)
        {
            return float.IsNaN(v3.x) || float.IsNaN(v3.y) || float.IsNaN(v3.z) || float.IsInfinity(v3.x) || float.IsInfinity(v3.y) || float.IsInfinity(v3.z);
        }

        public static bool IsBadRotation(Quaternion v3)
        {
            return float.IsNaN(v3.x) || float.IsNaN(v3.y) || float.IsNaN(v3.z) || float.IsInfinity(v3.x) || float.IsInfinity(v3.y) || float.IsInfinity(v3.z);
        }

        public static bool IsBadDistance(Vector3 current, Vector3 target)
        {
            return Vector3.Distance(current, target) > 500000;
        } 

        public static string ColorToHex(Color c)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}", ToByte(c.r), ToByte(c.g), ToByte(c.b));
        }

        private static byte ToByte(float f)
        {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }

        public static void DestroyChildren(this Transform transform)
        {
            transform.DestroyChildren(null);
        }

        public static void DestroyChildren(this Transform transform, Func<Transform, bool> exclude)
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                if (exclude == null || exclude(transform.GetChild(i))) UnityEngine.Object.DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        private static readonly Dictionary<string, Sprite> resourceSprites = new();
        public static Sprite GetSprite(string resourceName)
        {
            if (resourceSprites.ContainsKey(resourceName)) return resourceSprites[resourceName];

            var texture = GetTexture(resourceName);
            var rect = new Rect(0.0f, 0.0f, texture.width, texture.height);
            var pivot = new Vector2(0.5f, 0.5f);
            var border = Vector4.zero;
            var sprite = Sprite.CreateSprite(texture, rect, pivot, 100.0f, 0, SpriteMeshType.FullRect, border, false, null);
            sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            resourceSprites.Add(resourceName, sprite);
            return sprite;
        }

        private static readonly Dictionary<Sprite, Sprite> resourceSpritesOverride = new();
        public static Sprite GetSprite(string resourceName, Sprite existing)
        {
            if (existing == null) return null;

            if (resourceSpritesOverride.ContainsKey(existing)) return resourceSpritesOverride[existing];

            var texture = GetTexture(resourceName);
            var sprite = Sprite.CreateSprite(texture, existing.rect, existing.pivot, existing.pixelsPerUnit, 0, SpriteMeshType.FullRect, existing.border, false, null);
            sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            resourceSpritesOverride.Add(existing, sprite);
            return sprite;
        }

        private static Texture2D GetTexture(string resourceName)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Hexed.Resources.{resourceName}.png");
            if (stream == null)
            {
                Logger.LogError($"Failed to find texture {resourceName}");
                return null;
            }

            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            var texture = new Texture2D(1, 1);
            texture.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            ImageConversion.LoadImage(texture, ms.ToArray());
            texture.wrapMode = TextureWrapMode.Clamp;
            return texture;
        }

        public readonly struct InfPosition
        {
            public static readonly float InfValue = 8589934588;
            public static Vector3 INFBypass => new(InfValue, InfValue, InfValue);
            public static Vector3 NegINFBypass => new(-InfValue, -InfValue, -InfValue);
            public static Quaternion ROTBypass => new(InfValue, InfValue, InfValue, InfValue);
            public static Quaternion NegROTBypass => new(-InfValue, -InfValue, -InfValue, -InfValue);
        }

        private static readonly Dictionary<Texture2D, Texture2D> cachedReadableTexture2D = new();
        private static readonly Dictionary<Texture, Texture2D> cachedConvertedTexture2D = new();
        private static readonly Dictionary<Texture2D, Texture2D> cachedDesaturatedTexture2D = new();
        private static readonly Dictionary<Sprite, Texture2D> cachedUnpackedTexture2D = new();

        public static Texture2D ToTexture2D(this Texture texture)
        {
            if (cachedConvertedTexture2D.Any(a => a.Key != null && a.Key == texture)) return cachedConvertedTexture2D.FirstOrDefault(a => a.Key != null && a.Key == texture).Value;
            FilterMode currentFilter = texture.filterMode;
            texture.filterMode = FilterMode.Point;

            RenderTexture tmp = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.ARGB32);
            tmp.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            Graphics.Blit2(texture, tmp);
            RenderTexture.active = tmp;

            Texture2D myTexture2D = new(texture.width, texture.height);
            myTexture2D.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            myTexture2D.name = texture.name;
            myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            myTexture2D.Apply();
            myTexture2D.wrapMode = texture.wrapMode;
            myTexture2D.wrapModeU = texture.wrapModeU;
            myTexture2D.wrapModeV = texture.wrapModeV;
            myTexture2D.wrapModeW = texture.wrapModeW;

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(tmp);
            texture.filterMode = currentFilter;
            texture.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            cachedConvertedTexture2D.Add(texture, myTexture2D);
            return myTexture2D;
        }

        public static Texture2D CloneReadable(this Texture2D texture)
        {
            if (cachedReadableTexture2D.Any(a => a.Key != null && a.Key == texture)) return cachedReadableTexture2D.FirstOrDefault(a => a.Key != null && a.Key == texture).Value;
            FilterMode currentFilter = texture.filterMode;
            texture.filterMode = FilterMode.Point;
            RenderTexture tmp = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.ARGB32);
            tmp.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            Graphics.Blit2(texture, tmp);
            RenderTexture.active = tmp;

            Texture2D myTexture2D = new(texture.width, texture.height);
            myTexture2D.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            myTexture2D.name = texture.name;
            myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            myTexture2D.Apply();
            myTexture2D.wrapMode = texture.wrapMode;
            myTexture2D.wrapModeU = texture.wrapModeU;
            myTexture2D.wrapModeV = texture.wrapModeV;
            myTexture2D.wrapModeW = texture.wrapModeW;

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(tmp);
            texture.filterMode = currentFilter;

            cachedReadableTexture2D.Add(texture, myTexture2D);
            return myTexture2D;
        }

        public static Texture2D Desaturate(this Texture2D oldText)
        {
            if (cachedDesaturatedTexture2D.Any(a => a.Key != null && a.Key == oldText)) return cachedDesaturatedTexture2D.FirstOrDefault(a => a.Key != null && a.Key == oldText).Value;
            Texture2D tempTexture = (oldText.isReadable ? oldText : oldText.CloneReadable());
            tempTexture.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            Texture2D greyTexture = new(oldText.width, oldText.height);
            greyTexture.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            Color[] c = tempTexture.GetPixels();
            Color[] cDesat = new Color[c.Length];

            for (int i = 0; i < c.Length; i++)
            {
                float desat = (c[i].r + c[i].g + c[i].b) / 3;
                cDesat[i].r = desat;
                cDesat[i].g = desat;
                cDesat[i].b = desat;
                cDesat[i].a = c[i].a;
            }
            greyTexture.SetPixels(cDesat);
            greyTexture.Apply();

            cachedDesaturatedTexture2D.Add(oldText, greyTexture);
            return greyTexture;
        }
        public static Texture2D UnpackTexture(this Sprite sprite)
        {
            if (cachedUnpackedTexture2D.Any(a => a.Key != null && a.Key == sprite)) return cachedUnpackedTexture2D.FirstOrDefault(a => a.Key != null && a.Key == sprite).Value;
            Rect actualRect;
            if (!sprite.packed || sprite.packingMode != SpritePackingMode.Tight) actualRect = sprite.textureRect;
            else
            {
                float xMin = 1f;
                float yMax = 0f;
                float xMax = 0f;
                float yMin = 1f;

                foreach (Vector2 vector in sprite.uv)
                {
                    if (vector.x > xMax)
                        xMax = vector.x;
                    if (vector.x < xMin)
                        xMin = vector.x;
                    if (vector.y > yMax)
                        yMax = vector.y;
                    if (vector.y < yMin)
                        yMin = vector.y;
                }
                actualRect = new Rect
                {
                    m_XMin = xMin * sprite.texture.width,
                    m_YMin = yMin * sprite.texture.height,
                    m_Width = (xMax - xMin) * sprite.texture.width,
                    m_Height = (yMax - yMin) * sprite.texture.height
                };
            }

            Texture2D readableText = sprite.texture.isReadable ? sprite.texture : sprite.texture.CloneReadable();
            Color[] c = readableText.GetPixels((int)actualRect.x, (int)actualRect.y, (int)actualRect.width, (int)actualRect.height);
            var slicedText = new Texture2D((int)actualRect.width, (int)actualRect.height);
            slicedText.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            slicedText.SetPixels(c);
            slicedText.Apply();

            cachedUnpackedTexture2D.Add(sprite, slicedText);
            return slicedText;
        }

        public static Sprite ReplaceTexture(this Sprite sprite, Texture2D newTexture)
        {
            if (sprite == null) return sprite;
            var rect = new Rect(0, 0, newTexture.width, newTexture.height);
            var pivot = sprite.pivot / sprite.rect.size;
            var border = sprite.border;
            var newSprite = Sprite.CreateSprite(newTexture, rect, pivot, sprite.pixelsPerUnit, 0, SpriteMeshType.FullRect, border, false, null);
            newSprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return newSprite;
        }
    }
}
