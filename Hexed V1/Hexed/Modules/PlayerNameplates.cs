using Hexed.Interfaces;
using Hexed.Wrappers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

namespace Hexed.Modules
{
    internal class PlayerNameplates : IPlayerModule
    {
        private VRCPlayer targetPlayer;

        public void Initialize(VRCPlayer player)
        {
            targetPlayer = player;

            PlayerNameplate nameplate = targetPlayer.field_Public_PlayerNameplate_0;
            if (nameplate == null) return;

            nameplate.field_Public_Sprite_4 = UnityUtils.GetSprite("Nameplate", nameplate.field_Public_Sprite_4);
            nameplate.field_Public_Sprite_3 = nameplate.field_Public_Sprite_4;

            if (targetPlayer.GetAPIUser() == null) return;

            Color trustColor = targetPlayer.GetAPIUser().GetRankColor();

            GameObject uiContents = nameplate.field_Public_GameObject_0;
            if (uiContents == null) return;

            GameObject FriendAnchor = nameplate.field_Public_GameObject_4.transform.Find("Friend Anchor Icon")?.gameObject;

            Image uiIconBackground = nameplate.field_Public_GameObject_4.transform.Find("Background")?.GetComponent<Image>();
            GameObject uiStatusIcons = nameplate.field_Public_HorizontalLayoutGroup_0.gameObject;
            Graphic uiQuickStatsBackground = nameplate.field_Public_Graphic_1;
            Graphic uiGlow = nameplate.field_Public_Graphic_4;
            Graphic uiPulse = nameplate.field_Public_Graphic_5;
            Graphic uiIconGlow = nameplate.field_Public_Graphic_6;
            Graphic uiIconPulse = nameplate.field_Public_Graphic_7;

            ImageThreeSlice avatarLoadingBackground = nameplate.transform.parent.Find("Avatar Progress/Back")?.GetComponent<ImageThreeSlice>();
            TextMeshProUGUI avatarLoadingText = nameplate.transform.parent.Find("Avatar Progress/Fill Container/Text")?.GetComponent<TextMeshProUGUI>();

            if (FriendAnchor != null) FriendAnchor.SetActive(false);
            if (uiStatusIcons != null) uiStatusIcons.SetActive(false);
            if (uiGlow != null) uiGlow.color = new Color(trustColor.r, trustColor.g, trustColor.b, uiGlow.color.a);
            if (uiIconGlow != null) uiIconGlow.color = new Color(trustColor.r, trustColor.g, trustColor.b, uiIconGlow.color.a);
            if (uiIconPulse != null) uiIconPulse.color = new Color(trustColor.r, trustColor.g, trustColor.b, uiIconPulse.color.a);
            if (uiPulse != null) uiPulse.color = new Color(trustColor.r, trustColor.g, trustColor.b, uiPulse.color.a);
            if (uiIconBackground != null) uiIconBackground.color = new Color(trustColor.r, trustColor.g, trustColor.b, uiIconBackground.color.a);
            if (uiQuickStatsBackground != null) uiQuickStatsBackground.color = new Color(trustColor.r, trustColor.g, trustColor.b, uiQuickStatsBackground.color.a);
            if (avatarLoadingBackground != null) avatarLoadingBackground.color = new Color(trustColor.r, trustColor.g, trustColor.b, avatarLoadingBackground.color.a);
            if (avatarLoadingText != null) avatarLoadingText.color = new Color(trustColor.r, trustColor.g, trustColor.b, avatarLoadingText.color.a);

            Transform Root = nameplate.transform.parent.parent.parent.parent;
            ImageThreeSlice BubbleBackground = Root.Find("ChatBubble/Canvas/Chat")?.GetComponent<ImageThreeSlice>();
            ImageThreeSlice BubbleTyping = Root.Find("ChatBubble/Canvas/TypingIndicator")?.GetComponent<ImageThreeSlice>();
            ImageThreeSlice BubbleMirrorBackground = Root.Find("ChatBubbleMirror/Canvas/Chat")?.GetComponent<ImageThreeSlice>();
            ImageThreeSlice BubbleMirrorTyping = Root.Find("ChatBubbleMirror/Canvas/TypingIndicator")?.GetComponent<ImageThreeSlice>();
            Image CameraIcon = Root.Find("CameraNameplate/Canvas/Container/Image")?.GetComponent<Image>();
            TextMeshProUGUI CameraText = Root.Find("CameraNameplate/Canvas/Container/Name")?.GetComponent<TextMeshProUGUI>();
            ImageThreeSlice uiCameraBackground = Root.Find("CameraNameplate/Canvas/Container")?.GetComponent<ImageThreeSlice>();

            if (BubbleBackground != null) BubbleBackground.color = new Color(trustColor.r, trustColor.g, trustColor.b, BubbleBackground.color.a);
            if (BubbleTyping != null) BubbleTyping.color = new Color(trustColor.r, trustColor.g, trustColor.b, BubbleTyping.color.a);
            if (BubbleMirrorBackground != null) BubbleMirrorBackground.color = new Color(trustColor.r, trustColor.g, trustColor.b, BubbleMirrorBackground.color.a);
            if (BubbleMirrorTyping != null) BubbleMirrorTyping.color = new Color(trustColor.r, trustColor.g, trustColor.b, BubbleMirrorTyping.color.a);
            if (CameraIcon != null) CameraIcon.color = new Color(trustColor.r, trustColor.g, trustColor.b, CameraIcon.color.a);
            if (CameraText != null) CameraText.color = new Color(trustColor.r, trustColor.g, trustColor.b, CameraText.color.a);
            if (uiCameraBackground != null) uiCameraBackground._sprite = UnityUtils.GetSprite("Nameplate", uiCameraBackground._sprite);
        }

        public void OnUpdate()
        {
           
        }

        public void UpdatePlayerPlate(PlayerNameplate nameplate)
        {
            if (targetPlayer == null) return;

            APIUser apiUser = targetPlayer.GetAPIUser();
            if (apiUser == null) return;

            bool isFriend = apiUser.IsFriend();
            Color trustColor = apiUser.GetRankColor();

            nameplate.field_Public_Color_0 = new Color(trustColor.r, trustColor.g, trustColor.b, nameplate.field_Public_Color_0.a);
            nameplate.field_Public_Color_1 = new Color(trustColor.r, trustColor.g, trustColor.b, nameplate.field_Public_Color_1.a);

            if (nameplate.field_Public_TextMeshProUGUIEx_0 != null) nameplate.field_Public_TextMeshProUGUIEx_0.color = isFriend ? new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, nameplate.field_Public_TextMeshProUGUIEx_0.color.a) : new Color(trustColor.r, trustColor.g, trustColor.b, nameplate.field_Public_TextMeshProUGUIEx_0.color.a);
            if (nameplate.field_Public_TextMeshProUGUIEx_1 != null) nameplate.field_Public_TextMeshProUGUIEx_1.color = isFriend ? new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, nameplate.field_Public_TextMeshProUGUIEx_1.color.a) : new Color(trustColor.r, trustColor.g, trustColor.b, nameplate.field_Public_TextMeshProUGUIEx_1.color.a);
            if (nameplate.field_Public_TextMeshProUGUIEx_2 != null) nameplate.field_Public_TextMeshProUGUIEx_2.color = isFriend ? new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, nameplate.field_Public_TextMeshProUGUIEx_2.color.a) : new Color(trustColor.r, trustColor.g, trustColor.b, nameplate.field_Public_TextMeshProUGUIEx_2.color.a);
        }

        public void UpdateCameraPlate(CameraNameplate nameplate)
        {
            if (targetPlayer == null) return;

            APIUser apiUser = targetPlayer.GetAPIUser();
            if (apiUser == null) return;

            bool isFriend = apiUser.IsFriend();
            Color trustColor = apiUser.GetRankColor();

            if (nameplate.field_Public_Graphic_0 != null) nameplate.field_Public_Graphic_0.color = new Color(trustColor.r, trustColor.g, trustColor.b, nameplate.field_Public_Graphic_0.color.a);
            if (nameplate.field_Public_TextMeshProUGUI_0 != null) nameplate.field_Public_TextMeshProUGUI_0.color = isFriend ? new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, nameplate.field_Public_TextMeshProUGUI_0.color.a) : new Color(trustColor.r, trustColor.g, trustColor.b, nameplate.field_Public_TextMeshProUGUI_0.color.a);
        }
    }
}
