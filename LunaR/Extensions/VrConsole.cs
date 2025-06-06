using LunaR.Modules;
using LunaR.UIApi;
using LunaR.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace LunaR.ConsoleUtils
{
    internal class VRConsole
    {
        private static readonly List<string> AllLogsText = new();
        private static UIMenuText ConsoleText;

        public static IEnumerator LoadConsoleSprite()
        {
            GameObject Menu = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard");
            var Console = UnityEngine.Object.Instantiate(GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/BackgroundLayer01"), Menu.transform);
            var Request = UnityWebRequestTexture.GetTexture("https://i.imgur.com/D6ZsLzo.png");
            Request.SendWebRequest();
            while (!Request.isDone) yield return null;
            var Texture = DownloadHandlerTexture.GetContent(Request);
            Console.GetComponent<Image>().sprite = Sprite.CreateSprite(Texture, new Rect(0, 0, Texture.width, Texture.height), new Vector2(0, 0), 100 * 1000, 1000, SpriteMeshType.FullRect, Vector4.zero, false);
            Console.GetComponent<Image>().color = new Color(1, 1, 1, 0.68f);
            Console.transform.localScale = new Vector3(0.88f, 0.55f, 1);
            Console.transform.localPosition = new Vector3(0, -35, 0);
            Console.AddComponent<RectMask2D>();
            ConsoleText = new UIMenuText(Console, "", new Vector2(-298, -398), 28, false, TextAnchor.LowerLeft);
            ConsoleText.gameObject.transform.localScale = new Vector3(1, 1.46f, 1);
            ConsoleText.text.color = Color.grey;

            Menu.AddComponent<EnableDisableListener>().OnEnableEvent += () =>
            {
                PlayerList.PlayerCount.gameObject.SetActive(true);
                PlayerList.HearCount.gameObject.SetActive(true);
                PlayerList.AddPlayerToList();
            };
            Menu.AddComponent<EnableDisableListener>().OnDisableEvent += () =>
            {
                PlayerList.PlayerCount.gameObject.SetActive(false);
                PlayerList.HearCount.gameObject.SetActive(false);
            };
        }

        public enum LogsType
        {
            Info,
            Protection,
            Moderation,
            Voice,
            Block,
            Avatar,
            Join,
            Left,
            Friend,
            Portal,
            World,
            Online,
            Offline,
            Active,
            Object,
            Emote,
            Emoji,
            Camera,
            Pedestal,
            Notification
        }

        public static void Log(LogsType Type, string Text)
        {
            if (string.IsNullOrEmpty(Text)) return;
            switch (Type)
            {
                case LogsType.Info:
                    Text = "<color=#ee00ff>[" + DateTime.Now.ToShortTimeString() + "] " + "[Info]:</color>  " + Text;
                    break;

                case LogsType.Notification:
                    Text = "<color=#dd00ff>[" + DateTime.Now.ToShortTimeString() + "] " + "[Notification]:</color>  " + Text;
                    break;

                case LogsType.Emoji:
                    Text = "<color=#475400>[" + DateTime.Now.ToShortTimeString() + "] " + "[Emoji]:</color>  " + Text;
                    break;

                case LogsType.Emote:
                    Text = "<color=#540038>[" + DateTime.Now.ToShortTimeString() + "] " + "[Emote]:</color>  " + Text;
                    break;

                case LogsType.Voice:
                    Text = "<color=#00ffea>[" + DateTime.Now.ToShortTimeString() + "] " + "[Voice]:</color>  " + Text;
                    break;

                case LogsType.Avatar:
                    Text = "<color=#4800ff>[" + DateTime.Now.ToShortTimeString() + "] " + "[Avatar]:</color>  " + Text;
                    break;

                case LogsType.Block:
                    Text = "<color=#ad0000>[" + DateTime.Now.ToShortTimeString() + "] " + "[Block]:</color>  " + Text;
                    break;

                case LogsType.Object:
                    Text = "<color=#270054>[" + DateTime.Now.ToShortTimeString() + "] " + "[Object]:</color>  " + Text;
                    break;

                case LogsType.Join:
                    Text = "<color=#00ff04>[" + DateTime.Now.ToShortTimeString() + "] " + "[ + ]:</color>  " + Text;
                    break;

                case LogsType.Left:
                    Text = "<color=#ff1a1a>[" + DateTime.Now.ToShortTimeString() + "] " + "[ - ]:</color>  " + Text;
                    break;

                case LogsType.Portal:
                    Text = "<color=#030b82>[" + DateTime.Now.ToShortTimeString() + "] " + "[Portal]:</color>  " + Text;
                    break;

                case LogsType.Moderation:
                    Text = "<color=#426ff5>[" + DateTime.Now.ToShortTimeString() + "] " + "[Moderation]:</color>  " + Text;
                    break;

                case LogsType.Online:
                    Text = "<color=#77eb34>[" + DateTime.Now.ToShortTimeString() + "] " + "[Online]:</color>  " + Text;
                    break;

                case LogsType.Offline:
                    Text = "<color=#eb4634>[" + DateTime.Now.ToShortTimeString() + "] " + "[Offline]:</color>  " + Text;
                    break;

                case LogsType.Active:
                    Text = "<color=#ff810a>[" + DateTime.Now.ToShortTimeString() + "] " + "[Active]:</color>  " + Text;
                    break;

                case LogsType.Friend:
                    Text = "<color=#ccff00>[" + DateTime.Now.ToShortTimeString() + "] " + "[Friend]:</color>  " + Text;
                    break;

                case LogsType.World:
                    Text = "<color=#db5400>[" + DateTime.Now.ToShortTimeString() + "] " + "[World]:</color>  " + Text;
                    break;

                case LogsType.Protection:
                    Text = "<color=#c42525>[" + DateTime.Now.ToShortTimeString() + "] " + "[Protection]:</color>  " + Text;
                    break;

                case LogsType.Camera:
                    Text = "<color=#ff0073>[" + DateTime.Now.ToShortTimeString() + "] " + "[Camera]:</color>  " + Text;
                    break;

                case LogsType.Pedestal:
                    Text = "<color=#ffffff>[" + DateTime.Now.ToShortTimeString() + "] " + "[Pedestal]:</color>  " + Text;
                    break;
            }
            AllLogsText.Insert(0, Text);
            if (ConsoleText != null) ConsoleText.SetText(string.Join("\n", AllLogsText.Take(21)));
            if (AllLogsText.Count > 23) AllLogsText.RemoveAt(23);
        }
    }
}