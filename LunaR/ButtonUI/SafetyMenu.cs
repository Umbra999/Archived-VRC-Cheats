using LunaR.UIApi;
using LunaR.UIApi.Misc;
using UnityEngine;
using UnityEngine.UI;
using VRC;

namespace LunaR.ButtonUI
{
    internal class SafetyMenu
    {
        public static void InitExtraLevels()
        {
            var horizontalLayoutGroup = GameObject.Find("UserInterface/MenuContent/Screens/Settings_Safety/_Buttons_UserLevel").GetComponent<HorizontalLayoutGroup>();
            horizontalLayoutGroup.childControlWidth = true;
            horizontalLayoutGroup.childForceExpandWidth = true;
            new SafetyLevel("Nuisance", Color.grey, UserSocialClass.NegativeTrustLevel1, "Trolls which got blocked for their behavior very often").gameObject.transform.SetAsFirstSibling();
            new SafetyLevel("Staff", Color.red, UserSocialClass.DeveloperTrustLevel, "VRChat Team Members");
        }
    }
}
