using System;
using System.Collections.Generic;
using WengaPort.Buttons;
using System.Linq;

namespace WengaPort.ConsoleUtils
{
    class VRConsole
    {
        public static List<string> AllLogsText = new List<string>();
        public enum LogsType
        {
            Info,
            Protection,
            Voice,
            Avatar,
            Block,
            Join,
            Left,
            Warn,
            Kick,
            Votekick,
            Logout,
            Ban,
            Friend,
            Portal,
            World,
            Online,
            Offline,
            Clear
        }
        public static void Log(LogsType Type, string Text)
        {
            if (string.IsNullOrEmpty(Text)) return;
            string Text2 = string.Empty;
            switch (Type)
            {
                case LogsType.Info:
                    Text2 = "<color=#ee00ff>[" + DateTime.Now.ToShortTimeString() + "] " + "[Info]:</color>  " + Text;
                    break;
                case LogsType.Voice:
                    Text2 = "<color=#00ffea>[" + DateTime.Now.ToShortTimeString() + "] " + "[Voice]:</color>  " + Text;
                    break;
                case LogsType.Avatar:
                    Text2 = "<color=#4800ff>[" + DateTime.Now.ToShortTimeString() + "] " + "[Avatar]:</color>  " + Text;
                    break;
                case LogsType.Block:
                    Text2 = "<color=#ad0000>[" + DateTime.Now.ToShortTimeString() + "] " + "[Block]:</color>  " + Text;
                    break;
                case LogsType.Join:
                    Text2 = "<color=#00ff04>[" + DateTime.Now.ToShortTimeString() + "] " + "[+]:</color>  " + Text;
                    break;
                case LogsType.Left:
                    Text2 = "<color=#ff1a1a>[" + DateTime.Now.ToShortTimeString() + "] " + "[-]:</color>  " + Text;
                    break;
                case LogsType.Portal:
                    Text2 = "<color=#030b82>[" + DateTime.Now.ToShortTimeString() + "] " + "[Portal]:</color>  " + Text;
                    break;
                case LogsType.Warn:
                    Text2 = "<color=#ffd900>[" + DateTime.Now.ToShortTimeString() + "] " + "[Warn]:</color>  " + Text;
                    break;
                case LogsType.Kick:
                    Text2 = "<color=#ff0000>[" + DateTime.Now.ToShortTimeString() + "] " + "[Kick]:</color>  " + Text;
                    break;
                case LogsType.Votekick:
                    Text2 = "<color=#ff0000>[" + DateTime.Now.ToShortTimeString() + "] " + "[Votekick]:</color>  " + Text;
                    break;
                case LogsType.Logout:
                    Text2 = "<color=#4800ff>[" + DateTime.Now.ToShortTimeString() + "] " + "[Logout]:</color>  " + Text;
                    break;
                case LogsType.Online:
                    Text2 = "<color=#77eb34>[" + DateTime.Now.ToShortTimeString() + "] " + "[Online]:</color>  " + Text;
                    break;
                case LogsType.Offline:
                    Text2 = "<color=#eb4634>[" + DateTime.Now.ToShortTimeString() + "] " + "[Offline]:</color>  " + Text;
                    break;
                case LogsType.Ban:
                    Text2 = "<color=#c20000>[" + DateTime.Now.ToShortTimeString() + "] " + "[Ban]:</color>  " + Text;
                    break;
                case LogsType.Friend:
                    Text2 = "<color=#ccff00>[" + DateTime.Now.ToShortTimeString() + "] " + "[Friend]:</color>  " + Text;
                    break;
                case LogsType.Clear:
                    Text2 = "";
                    AllLogsText.Clear();
                    ConsoleMenu.QMInfo.SetText("");
                    break;
                case LogsType.World:
                    Text2 = "<color=#db5400>[" + DateTime.Now.ToShortTimeString() + "] " + "[World]:</color>  " + Text;
                    break;
                case LogsType.Protection:
                    Text2 = "<color=#eb0000>[" + DateTime.Now.ToShortTimeString() + "] " + "[Protection]:</color>  " + Text;
                    break;

            }
            AllLogsText.Insert(0, Text2);
            ConsoleMenu.QMInfo.SetText(string.Join("\n", AllLogsText.Take(20)));
        }
    }
}
