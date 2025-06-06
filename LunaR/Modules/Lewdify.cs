using System.Collections.Generic;
using UnityEngine;
using VRC;

namespace LunaR.Modules
{
    internal class Lewdify
    {
        private static readonly List<string> KeywordsToDisable = new()
        {
            "top",
            "bra",
            "strap",
            "cloth",
            "skirt",
            "trouser",
            "bottom",
            "boxers",
            "hoodie",
            "dress",
            "stock",
            "cover",
            "sfw",
            "biki",
            "bikini",
            "tiddy",
            "titty",
            "underwear",
            "under",
            "swimsuit",
            "tanktop",
            "jacket",
            "shirt",
            "tshirt",
            "t-shirt",
            "fishnets",
            "jacket_",
            "jacket_1",
            "jacket_2",
            "corset",
            "straps",
            "harness",
            "lingerie",
            "outfit",
            "bodysuit",
            "pasties",
            "asphalttop",
            "undie",
            "short",
            "jacketpack",
            "garter",
            "fish nets",
            "sweater",
            "chest harness",
            "maid dress",
            "maid lingerie",
            "one piece",
            "pastie",
            "bottoms",
            "thong",
            "shoe",
            "bura",
            "body.002",
            "nippleband",
            "bra",
            "eyecover",
            "harness1",
            "tearsweater",
            "nets",
            "pastes",
            "nsfw",
            "lewd",
            "parker",
            "pant",
        };

        private static readonly List<string> KeywordsToEnable = new()
        {
            "penis",
            "dick",
            "futa",
            "dildo",
            "strap",
            "shlong",
            "dong",
            "vibrator",
            "lovense",
            "cock",
            "sex",
            "toy",
            "butt",
            "plug",
            "whip",
            "cum",
            "sperm",
            "facial",
            "nude",
            "naked",
            "erp",
            "unclothed",
            "nora",
        };

        public static void Lewd(Player player)
        {
            foreach (SkinnedMeshRenderer renderer in player.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                if (KeywordsToDisable.Contains(renderer.name.ToLower())) renderer.enabled = false;
                if (KeywordsToEnable.Contains(renderer.name.ToLower()))
                {
                    renderer.gameObject.SetActive(true);
                    renderer.enabled = true;
                }
            }
        }
    }
}