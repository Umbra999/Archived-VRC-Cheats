using LunaR.Extensions;
using LunaR.Wrappers;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VRC;

namespace LunaR.Modules
{
    internal class PlayerTags
    {
        public static void CustomTag(Player player)
        {
            try
            {
                Transform contents = player.transform.Find("Player Nameplate/Canvas/Nameplate/Contents");
                Transform stats = contents.Find("Quick Stats");
                int stack = 0;

                string CustomTag = null;
                foreach (KeyValuePair<string, string> Pair in ServerRequester.CustomUserTags)
                {
                    if (Pair.Key == player.UserID()) CustomTag = Pair.Value;
                }

                PlayerExtensions.TrustRanks Rank = player.GetAPIUser().GetRank();
                Color RankColor = player.GetAPIUser().GetRankColor();
                switch (Rank)
                {
                    case PlayerExtensions.TrustRanks.USER:
                        SetTag(ref stack, stats, contents, RankColor, CustomTag ?? "User");
                        break;

                    case PlayerExtensions.TrustRanks.KNOWN:
                        SetTag(ref stack, stats, contents, RankColor, CustomTag ?? "Known");
                        break;

                    case PlayerExtensions.TrustRanks.NUISANCE:
                        SetTag(ref stack, stats, contents, RankColor, CustomTag ?? "Nuisance");
                        break;

                    case PlayerExtensions.TrustRanks.NEW:
                        SetTag(ref stack, stats, contents, RankColor, CustomTag ?? "New");
                        break;

                    case PlayerExtensions.TrustRanks.VISITOR:
                        SetTag(ref stack, stats, contents, RankColor, CustomTag ?? "Visitor");
                        break;

                    case PlayerExtensions.TrustRanks.TRUSTED:
                        SetTag(ref stack, stats, contents, RankColor, CustomTag ?? "Trusted");
                        break;

                    case PlayerExtensions.TrustRanks.MOD:
                        SetTag(ref stack, stats, contents, RankColor, CustomTag ?? "Moderator");
                        break;

                    default:
                        break;
                }

                if (player.IsFriend()) SetTag(ref stack, stats, contents, Color.yellow, "Friend");
                if (player.GetAPIUser().IsOnMobile) SetTag(ref stack, stats, contents, new Color(0.1f, 0.4f, 0.17f), "Quest");

                PlayerExtensions.Pronouns Pronouns = player.GetAPIUser().GetGender();
                switch (Pronouns)
                {
                    case PlayerExtensions.Pronouns.Female:
                        SetTag(ref stack, stats, contents, new Color(0.65f, 0, 1), "Female");
                        break;

                    case PlayerExtensions.Pronouns.Male:
                        SetTag(ref stack, stats, contents, new Color(0, 0, 1), "Male");
                        break;

                    case PlayerExtensions.Pronouns.Diverse:
                        SetTag(ref stack, stats, contents, new Color(0, 0.83f, 1), "Diverse");
                        break;
                }

                stats.localPosition = new Vector3(0, (stack + 1) * 30, 0);
            }
            catch { }
        }

        public static void TagToUpdate(Transform stats, Transform contents, Color color, string content)
        {
            Transform tag = contents.Find($"LunaRTag-3");
            Transform label;
            if (tag == null) label = MakeTag(stats, -3, color);
            else label = tag.Find("Trust Text");
            var text = label.GetComponent<TextMeshProUGUI>();
            text.color = color;
            text.text = content;
            text.richText = true;
        }

        private static Transform MakeTag(Transform stats, int index, Color color)
        {
            Transform rank = Object.Instantiate(stats, stats.parent, false);
            rank.name = $"LunaRTag{index}";
            rank.localPosition = new Vector3(0, 30 * (index + 1), 0);
            rank.gameObject.active = true;
            var Background = rank.GetComponent<ImageThreeSlice>();
            Background.enabled = false;
            Transform textGO = null;
            for (int i = rank.childCount; i > 0; i--)
            {
                Transform child = rank.GetChild(i - 1);
                if (child.name == "Trust Text")
                {
                    textGO = child;
                    continue;
                }
                Object.Destroy(child.gameObject);
            }
            return textGO;
        }

        public static void SetTag(ref int stack, Transform stats, Transform contents, Color color, string content)
        {
            Transform tag = contents.Find($"LunaRTag{stack}");
            Transform label;
            if (tag == null) label = MakeTag(stats, stack, color);
            else
            {
                tag.gameObject.SetActive(true);
                label = tag.Find("Trust Text");
            }
            var text = label.GetComponent<TextMeshProUGUI>();
            text.color = color;
            text.text = content;
            text.richText = true;
            stack++;
        }
    }
}