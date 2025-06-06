using Hexed.Interfaces;
using Hexed.Modules.EventManagement;
using Hexed.Wrappers;
using TMPro;
using UnityEngine;

namespace Hexed.Modules
{
    internal class PlayerTags : IPlayerModule, IDelayModule
    {
        private VRCPlayer targetPlayer;

        public void Initialize(VRCPlayer player)
        {
            targetPlayer = player;
        }

        public void OnUpdate()
        {
            if (targetPlayer == null) return;

            PlayerNameplate nameplate = targetPlayer.field_Public_PlayerNameplate_0;
            if (nameplate == null) return;

            SetTag(nameplate, new Vector3(0, 42, 0), "State", Color.white, string.Concat(new string[]
           {
                 targetPlayer.IsInstanceOwner() ? "<color=#7b00ff>O</color> " :  targetPlayer.GetPhotonPlayer().IsInstanceModerator() ? "<color=#3c1769>P</color> " : "",
                 targetPlayer.GetPhotonPlayer().IsMaster() ? "<color=#8719fc>M</color> " : "",
                 ModerationHandler.BlockList.Contains(targetPlayer.GetPhotonPlayer().ActorID()) ? "<color=#ff0000>B</color> " : ModerationHandler.MuteList.Contains(targetPlayer.GetPhotonPlayer().ActorID()) ? "<color=#ff0000>M</color> " : "",
                 GameHelper.ModerationManager.IsBlocked(targetPlayer.UserID()) ? "<color=#424242>B</color> " : GameHelper.ModerationManager.IsMuted(targetPlayer.UserID()) ? "<color=#424242>M</color> " : "",
                 $"<color={UnityUtils.ColorToHex(PlayerUtils.GetPlatformColor(targetPlayer.GetPhotonPlayer().GetPlatform()))}>{(targetPlayer.IsPlayerFBT() ? "FBT" : targetPlayer.GetDevice().GetDeviceTag())}</color> ",
           }));

            SetTag(nameplate, new Vector3(0, -42, 0), "Network", Color.white, string.Concat(new string[]
            {
                 $"P: <color={UnityUtils.ColorToHex(targetPlayer.GetPingColor())}>{targetPlayer.GetPing()}</color> ",
                 $"F: <color={UnityUtils.ColorToHex(targetPlayer.GetFramesColor())}>{targetPlayer.GetFrames()}</color> ",
                 $"Q: <color={UnityUtils.ColorToHex(targetPlayer.GetQualityColor())}>{targetPlayer.GetQualityPercentage()}%</color> ",
            }));

            SetTag(nameplate, new Vector3(0, -66, 0), "Pose", Color.white, string.Concat(new string[]
            {
                 PlayerUtils.IsPlayerAFK(targetPlayer) ? "<color=#ff03e2>AFK</color>\n" : "",
                 PlayerUtils.IsPlayerMicDisabled(targetPlayer) ? "<color=#2fc47c>MUTED</color>\n" : "",
                 PlayerUtils.IsPlayerSeated(targetPlayer) ? "<color=#ebe831>SEATED</color>\n" : "",
                 //PlayerUtils.IsPlayerEarmuff(targetPlayer) ? "<color=#9d31eb>EARMUFF</color>\n" : "",
                 PlayerUtils.IsFrozen(targetPlayer.GetPhotonPlayer().ActorID()) ? "<color=#fc0352>FROZEN</color>\n" : "",
            }));
        }

        private static TextMeshProUGUI SetTag(PlayerNameplate nameplate, Vector3 position, string identifier, Color color, string content)
        {
            GameObject contents = nameplate?.field_Public_GameObject_0;
            if (contents == null) return null;

            Transform tag = contents.transform.Find($"HexedTag-{identifier}");
            if (tag == null) tag = MakeTag(nameplate, identifier);

            TextMeshProUGUI text = tag.GetComponent<TextMeshProUGUI>();
            if (text == null) return null;

            text.color = color;
            text.text = content;
            text.richText = true;
            tag.localPosition = position;

            return text;
        }

        private static Transform MakeTag(PlayerNameplate nameplate, string identifier)
        {
            GameObject contents = nameplate?.field_Public_GameObject_0;
            if (contents == null) return null;

            Transform trustText = nameplate?.field_Public_TextMeshProUGUIEx_4?.transform;
            if (trustText == null) return null;

            Transform rank = UnityEngine.Object.Instantiate(trustText, contents.transform);
            if (rank == null) return null;

            rank.name = $"HexedTag-{identifier}";
            rank.gameObject.SetActive(true);

            return rank;
        }
    }
}
