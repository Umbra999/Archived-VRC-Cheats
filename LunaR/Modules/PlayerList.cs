using LunaR.Extensions;
using LunaR.Patching;
using LunaR.UIApi;
using LunaR.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC;

namespace LunaR.Modules
{
    internal class PlayerList : MonoBehaviour
    {
        public static UIMenuText PlayerCount;
        public static UIMenuText HearCount;
        private static float Delay = 0;

        public static void Init()
        {
            GameObject parent = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/Wing_Left/Button/Icon");
            PlayerCount = new UIMenuText(parent, "Playerlist", new Vector2(-685, 300f), 32, false, TextAnchor.UpperLeft);
            PlayerCount.text.color = Color.white;

            GameObject parent2 = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/Wing_Right/Button/Icon");
            HearCount = new UIMenuText(parent2, "Hearlist", new Vector2(400, 300f), 32, false, TextAnchor.UpperLeft);
            HearCount.text.color = Color.white;
        }

        public static void AddPlayerToList()
        {
            List<string> PlayerStrings = new();
            List<string> HearStrings = new();

            foreach (Photon.Realtime.Player PhotonPlayer in Utils.VRCNetworkingClient.GetAllPhotonPlayers().OrderBy(a => a.ActorID()))
            {
                Player player = PhotonPlayer.GetPlayer();
                if (player != null && player.GetAPIUser() != null)
                {
                    PlayerStrings.Add(string.Concat(new string[]
                    {
                            player.IsFriend() ? "<color=#ebc400>" : "<color=#ffffff>",
                            $"{PhotonPlayer.ActorID()}",
                            "</color> - ",
                            (player.GetIsBot() || BotDetection.DetectedBots.Contains(PhotonPlayer.ActorID())) ? "<color=#a33333>BOT</color> | " : "",
                            ModerationHandler.BlockList.Contains(PhotonPlayer.ActorID()) ? "<color=#ff0000>B</color> | " : ModerationHandler.MuteList.Contains(PhotonPlayer.ActorID()) ? "<color=#ff0000>M</color> | " : "",
                            PlayerExtensions.GetIsFrozen(PhotonPlayer.ActorID()) ? "<color=#fc0352>S</color> | " : "",
                            player.IsInstanceOwner() ? "<color=#3c1769>O</color> | " : "",
                            PhotonPlayer.IsMaster() ? "<color=#3c1769>M</color> | " : "",
                            player.GetAPIUser().isSupporter ? "<color=#b66b25>V</color> | " : "",
                            player.GetAPIUser().IsOnMobile ? "<color=#27b02d>" : "<color=#ffffff>",
                            player.GetVRCPlayerApi().IsUserInVR() ? (player.GetVRCPlayer().IsPlayerFBT() ? "FBT" : "VR") : "PC",
                            "</color> | ",
                            $"<color=#{ColorUtility.ToHtmlStringRGB(player.GetAPIUser().GetRankColor())}>",
                            player.DisplayName(),
                            "</color> | ",
                            $"P: {player.GetPingColored()} ",
                            $"F: {player.GetFramesColored()}",
                    }));

                    if (PhotonPlayer.CanHearMe()) HearStrings.Add($"<color=#{ColorUtility.ToHtmlStringRGB(player.GetVRCPlayer().GetQualityColor())}>{player.DisplayName()}</color>");
                }
                else
                {
                    PlayerStrings.Add(string.Concat(new string[]
                    {
                            PlayerExtensions.IsFriend(PhotonPlayer.UserID()) ? "<color=#ebc400>" : "<color=#ffffff>",
                            $"{PhotonPlayer.ActorID()} ",
                            "</color> - ",
                            PhotonPlayer.IsMod() ? "<color=#a33333>MOD</color> | " : "",
                            "<color=#a33333>BOT</color> | ",
                            ModerationHandler.BlockList.Contains(PhotonPlayer.ActorID()) ? "<color=#424242>B</color> | " :  ModerationHandler.MuteList.Contains(PhotonPlayer.ActorID()) ? "<color=#424242>M</color> | " : "",
                            PhotonPlayer.IsMaster() ? "<color=#3c1769>M</color> | " : "",
                            PhotonPlayer.IsVR() ? "VR | " : "PC | ",
                            $"<color=#a33333>{PhotonPlayer.GetDisplayName()}</color>"
                    }));

                    if (PhotonPlayer.CanHearMe()) HearStrings.Add($"<color=#a33333>BOT | {PhotonPlayer.GetDisplayName()}</color>");
                }
            }

            PlayerCount.SetText($"<b>In Room: {PlayerStrings.Count} / {RoomManager.field_Internal_Static_ApiWorld_0.capacity * 2}</b> \n" + string.Join("\n", PlayerStrings));
            HearCount.SetText($"<b>Hear You: {HearStrings.Count} / {PlayerStrings.Count - 1}</b> \n" + string.Join("\n", HearStrings));
        }

        public void Update()
        {
            Delay += Time.deltaTime;
            if (Delay < 2) return;
            Delay = 0;

            if (PlayerCount != null && PlayerCount.gameObject.active) AddPlayerToList();
        }

        public PlayerList(IntPtr ptr) : base(ptr)
        {
        }
    }
}