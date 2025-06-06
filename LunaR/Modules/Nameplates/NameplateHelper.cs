using System;
using TMPro;
using UnhollowerBaseLib.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace LunaR.Modules
{
    public class NameplateHelper : MonoBehaviour
    {
        public Transform uiContents;
        public Transform uiQuickStats;
        public GameObject statusMute;

        public Graphic uiIconBackground;
        public ImageThreeSlice uiNameBackground;
        public ImageThreeSlice uiQuickStatsBackground;
        public GameObject uiStatusIcons;
        public TextMeshProUGUI uiName;
        public TextMeshProUGUI uiStatus;
        public ImageThreeSlice uiGlow;
        public ImageThreeSlice uiPulse;
        public Image uiIconGlow;
        public Image uiIconPulse;

        private PlayerNameplate nameplate;
        private Color nameColour;

        public NameplateHelper(IntPtr ptr) : base(ptr)
        {
        }

        [HideFromIl2Cpp]
        public void SetNameplate(PlayerNameplate nameplate)
        {
            this.nameplate = nameplate;
        }

        [HideFromIl2Cpp]
        public void SetNameColour(Color color)
        {
            nameColour = color;
        }

        [HideFromIl2Cpp]
        public void OnRebuild()
        {
            if (nameplate != null)
            {
                uiName.color = nameColour;
                uiStatus.color = nameColour;
                uiGlow.color = nameColour;
                uiIconGlow.color = nameColour;
                uiIconPulse.color = nameColour;
                uiPulse.color = nameColour;
                uiStatusIcons.SetActive(false);
            }
        }
    }
}