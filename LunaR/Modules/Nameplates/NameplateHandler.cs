using LunaR.Wrappers;
using System.Collections;
using TMPro;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using VRC;

namespace LunaR.Modules
{
    public class Nameplates
    {

        private static Material npUIMaterial;
        private static Sprite nameplateOutline;

        public static void Init()
        {
            LoadAssets().Start();
        }

        public static void OnUpdatePlayer(Player VRCPlayer)
        {
            if (VRCPlayer == null) return;
            PlayerNameplate nameplate = VRCPlayer.GetVRCPlayer().field_Public_PlayerNameplate_0;
            if (nameplate == null) return;
            NameplateHelper helper = nameplate.GetComponent<NameplateHelper>();

            if (helper == null)
            {
                helper = nameplate.gameObject.AddComponent<NameplateHelper>();
                helper.SetNameplate(nameplate);

                helper.uiContents = nameplate.gameObject.transform.Find("Contents");
                helper.uiQuickStats = helper.uiContents.Find("Quick Stats");

                helper.uiIconBackground = helper.uiContents.Find("Icon/Background").GetComponent<Image>();
                helper.uiNameBackground = helper.uiContents.Find("Main/Background").GetComponent<ImageThreeSlice>();
                helper.uiStatusIcons = helper.uiContents.Find("Status Line").gameObject;
                helper.statusMute = helper.uiStatusIcons.transform.Find("Status Icons/User Muted").gameObject;
                helper.uiQuickStatsBackground = helper.uiQuickStats.GetComponent<ImageThreeSlice>();
                helper.uiName = helper.uiContents.Find("Main/Text Container/Name").GetComponent<TextMeshProUGUI>();
                helper.uiStatus = helper.uiContents.Find("Main/Text Container/Sub Text/Text").GetComponent<TextMeshProUGUI>();
                helper.uiGlow = helper.uiContents.Find("Main/Glow").GetComponent<ImageThreeSlice>();
                helper.uiPulse = helper.uiContents.Find("Main/Pulse").GetComponent<ImageThreeSlice>();
                helper.uiIconGlow = helper.uiContents.Find("Icon/Glow").GetComponent<Image>();
                helper.uiIconPulse = helper.uiContents.Find("Icon/Pulse").GetComponent<Image>();
            }

            ImageThreeSlice bgImage = helper.uiNameBackground.GetComponent<ImageThreeSlice>();
            if (bgImage != null) bgImage._sprite = nameplateOutline;

            ApplyNameplateColour(helper, PlayerExtensions.GetRankColor(VRCPlayer.GetAPIUser()));
        }

        private static void ApplyNameplateColour(NameplateHelper helper, Color Color)
        {
            if (helper == null) return;

            helper.uiIconBackground.material = npUIMaterial;
            helper.uiIconBackground.color = Color;

            helper.uiNameBackground.material = npUIMaterial;
            helper.uiNameBackground.color = Color;

            helper.uiQuickStatsBackground.material = npUIMaterial;
            helper.uiQuickStatsBackground.color = Color;

            helper.SetNameColour(Color);
            helper.OnRebuild();
        }

        private static IEnumerator LoadAssets()
        {
            AssetBundleCreateRequest dlBundle = AssetBundle.LoadFromFileAsync("LunaR\\LunaR.nmasset");
            while (!dlBundle.isDone) yield return new WaitForSeconds(0.1f);
            if (dlBundle.assetBundle != null)
            {
                npUIMaterial = dlBundle.assetBundle.LoadAsset_Internal("NameplateMat", Il2CppType.Of<Material>()).Cast<Material>();
                npUIMaterial.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                nameplateOutline = dlBundle.assetBundle.LoadAsset_Internal("NameplateOutline", Il2CppType.Of<Sprite>()).Cast<Sprite>();
                nameplateOutline.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            }
        }
    }
}