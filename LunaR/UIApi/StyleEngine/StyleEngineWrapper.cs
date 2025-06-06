using System.Collections.Generic;
using System.IO;
using UnhollowerRuntimeLib;
using UnityEngine;
using VRC.UI.Core.Styles;

namespace LunaR.StyleAPI
{
    public class StyleEngineWrapper
    {
        public static StyleEngine StyleEngine;
        private static readonly Dictionary<string, Sprite> myOriginalSpritesByLowercaseFullKey = new();
        private static readonly Dictionary<string, List<ElementStyle>> myStylesCache = new();
        private static readonly Dictionary<string, string> myNormalizedToActualSpriteNames = new();
        private static readonly Il2CppSystem.Collections.Generic.Dictionary<Sprite, Sprite> mySpriteOverrideDict = new();

        public static List<ElementStyle> TryGetBySelector(string normalizedSelector)
        {
            return myStylesCache.TryGetValue(normalizedSelector, out var result) ? result : null;
        }

        public static Sprite TryFindOriginalSprite(string key)
        {
            return myOriginalSpritesByLowercaseFullKey.TryGetValue(key, out var result) ? result : null;
        }

        public static void OverrideSprite(string key, Sprite sprite)
        {
            var keyLastPart = Path.GetFileName(key);
            var actualKey = myNormalizedToActualSpriteNames.TryGetValue(keyLastPart, out var normalized) ? normalized : keyLastPart;

            var originalSprite = TryFindOriginalSprite(key);
            if (originalSprite != null) mySpriteOverrideDict[originalSprite] = sprite;

            StyleEngine.field_Private_Dictionary_2_String_Sprite_0[actualKey] = sprite;
            StyleEngine.field_Private_Dictionary_2_Tuple_2_String_Type_Object_0[new Il2CppSystem.Tuple<string, Il2CppSystem.Type>(key.ToLower(), Il2CppType.Of<Sprite>())] = sprite;
        }

        public static void UpdateStylesForSpriteOverrides()
        {
            var writeAccumulator = new List<(int, StyleElement.PropertyValue)>();

            foreach (var elementStyle in StyleEngine.field_Private_List_1_ElementStyle_0)
            {
                foreach (var keyValuePair in elementStyle.field_Public_Dictionary_2_Int32_PropertyValue_0)
                {
                    var styleProperty = keyValuePair.Value;
                    var maybeSprite = styleProperty.field_Public_Object_0?.TryCast<Sprite>();
                    if (maybeSprite == null || !mySpriteOverrideDict.ContainsKey(maybeSprite)) continue;

                    styleProperty.field_Public_Object_0 = mySpriteOverrideDict[maybeSprite];
                    writeAccumulator.Add((keyValuePair.Key, styleProperty));
                }

                foreach (var (k, v) in writeAccumulator)
                    elementStyle.field_Public_Dictionary_2_Int32_PropertyValue_0[k] = v;

                writeAccumulator.Clear();
            }
        }

        public static void BackupDefaultStyle()
        {
            foreach (var elementStyle in StyleEngine.field_Private_List_1_ElementStyle_0)
            {
                var innerList = new List<(int, StyleElement.PropertyValue)>();
                foreach (var keyValuePair in elementStyle.field_Public_Dictionary_2_Int32_PropertyValue_0)
                    innerList.Add((keyValuePair.Key, keyValuePair.Value));

                var normalizedSelector = elementStyle.field_Public_Selector_0.ToStringNormalized();

                if (myStylesCache.TryGetValue(normalizedSelector, out var existing)) existing.Add(elementStyle);
                else myStylesCache[normalizedSelector] = new List<ElementStyle>() { elementStyle };
            }

            foreach (var keyValuePair in StyleEngine.field_Private_Dictionary_2_String_Sprite_0)
            {
                var key = keyValuePair.Key;
                var normalizedKey = key.ToLower();

                myNormalizedToActualSpriteNames[normalizedKey] = key;
            }

            foreach (var keyValuePair in StyleEngine.field_Private_Dictionary_2_Tuple_2_String_Type_Object_0)
            {
                if (keyValuePair.Key.Item2 == Il2CppType.Of<Sprite>()) myOriginalSpritesByLowercaseFullKey[keyValuePair.Key.Item1] = keyValuePair.Value.Cast<Sprite>();
            }
        }
    }
}