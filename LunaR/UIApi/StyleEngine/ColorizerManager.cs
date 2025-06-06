using UnityEngine;

namespace LunaR.StyleAPI
{
    public class ColorizerManager
    {
        public static Color BaseColor => ParseColor("16 16 16");
        public static Color AccentColor => ParseColor("0 0 255");
        public static Color TextColor => ParseColorWithFallback("", "0 0 255");
        public static Color AuxColor => ParseColorWithFallback("", "0 0 255");

        private static Color ParseColorWithFallback(string? mainValue, string fallbackValue)
        {
            return ParseColor(string.IsNullOrEmpty(mainValue?.Trim()) ? fallbackValue : mainValue!);
        }

        private static int ParseComponent(string[] split, int idx, int defaultValue = 255)
        {
            if (split.Length <= idx || !int.TryParse(split[idx], out var parsed)) parsed = defaultValue;
            if (parsed < 0) parsed = 0;
            else if (parsed > 255) parsed = 255;
            return parsed;
        }

        private static Color ParseColor(string str)
        {
            var split = str.Split(' ');
            var r = ParseComponent(split, 0, 200);
            var g = ParseComponent(split, 1, 200);
            var b = ParseComponent(split, 2, 200);
            var a = ParseComponent(split, 3, 255);

            return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }

        private static string MenuColorBase { get; set; } = "";
        private static string MenuColorHighlight { get; set; } = "";
        private static string MenuColorBackground { get; set; } = "";
        private static string MenuColorDarklight { get; set; } = "";

        private static string MenuColorText { get; set; } = "";
        private static string MenuColorTextHigh { get; set; } = "";

        private static string MenuColorAccent { get; set; } = "";
        private static string MenuColorAccentDarker { get; set; } = "";

        public static void UpdateColors()
        {
            UpdateColors(BaseColor, AccentColor, TextColor);
        }

        public static string ReplacePlaceholders(string input)
        {
            return input
                    .Replace("$BASE$", MenuColorBase)
                    .Replace("$HIGH$", MenuColorHighlight)
                    .Replace("$BG$", MenuColorBackground)
                    .Replace("$DARK$", MenuColorDarklight)
                    .Replace("$TEXT$", MenuColorText)
                    .Replace("$TEXTHI$", MenuColorTextHigh)
                    .Replace("$ACCT$", MenuColorAccent)
                    .Replace("$ACCDK$", MenuColorAccentDarker);
        }

        private static void UpdateColors(Color @base, Color accent, Color text)
        {
            var highlight = @base.RGBMultipliedClamped(1.1f);
            var background = @base.RGBMultipliedClamped(0.9f);
            var dark = @base.RGBMultipliedClamped(0.5f);

            MenuColorBase = ColorToHex(@base);
            MenuColorHighlight = ColorToHex(highlight);
            MenuColorBackground = ColorToHex(background);
            MenuColorDarklight = ColorToHex(dark);

            MenuColorText = ColorToHex(text.RGBMultipliedClamped(0.9f));
            MenuColorTextHigh = ColorToHex(text);

            MenuColorAccent = ColorToHex(accent);
            MenuColorAccentDarker = ColorToHex(accent.RGBMultipliedClamped(0.7f));
        }

        private static string PartToHex(float f) => ((int)(f * 255)).ToString("x2");

        private static string ColorToHex(Color c) => $"#{PartToHex(c.r)}{PartToHex(c.g)}{PartToHex(c.b)}";
    }
}