using LunaR.Extensions;
using LunaR.Patching;
using LunaR.Wrappers;
using System;
using UnityEngine;
using VRC;
using VRC.Core;

namespace LunaR.Modules
{
    internal class PlateChanger : MonoBehaviour
    {
        private static float UpdateDistance = 38f;
        private static float Delay = 0;

        public void Update()
        {
            Delay += Time.deltaTime;
            if (Delay < 2) return;
            Delay = 0;

            if (!GeneralWrappers.IsInWorld()) return;

            foreach (Player player in Utils.PlayerManager.GetAllPlayers())
            {
                try
                {
                    GameObject avatarObject = player.GetAvatarObject();
                    if (avatarObject == null) continue;
                    float Distance = Vector3.Distance(Utils.CurrentUser.transform.position, avatarObject.transform.position);
                    if (Distance < UpdateDistance) AdjustTag(player);
                }
                catch { }
            }
        }

        private static void AdjustTag(Player player)
        {
            NameplateHelper Helper = player.GetVRCPlayer().field_Public_PlayerNameplate_0.gameObject.GetComponent<NameplateHelper>();
            if (Helper == null) return;

            PlayerTags.TagToUpdate(Helper.uiQuickStats, Helper.uiContents, Color.white, string.Concat(new string[]
            {
                 $"F: {player.GetFramesColored()} ",
                 $"P: {player.GetPingColored()} ",
                 player.IsMaster() ? "<color=#7b00ff>M</color> " : "",
                 ModerationHandler.BlockList.Contains(player.GetPhotonPlayer().ActorID()) ? "<color=#ff0000>B</color> " : ModerationHandler.MuteList.Contains(player.GetPhotonPlayer().ActorID()) ? "<color=#ff0000>M</color> " : "",
                 PlayerExtensions.IsBlocked(player.UserID()) ? "<color=#424242>B</color> " : Helper.statusMute.activeSelf ? "<color=#424242>M</color> " : "",
                 PlayerExtensions.GetIsFrozen(player.GetPhotonPlayer().ActorID()) ? "<color=#fc0352>S</color>" : ""
            }));
        }

        public static void Selfhide(bool Toggle)
        {
            SelfHideToggle = Toggle;
            PlayerExtensions.ReloadAvatar(APIUser.CurrentUser);
        }

        public static bool SelfHideToggle = false;

        public PlateChanger(IntPtr ptr) : base(ptr)
        {
        }
    }
}