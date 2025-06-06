using Hexed.CustomUI;

namespace Hexed.Modules.Standalone
{
    internal class VRConsole
    {
        public enum LogsType
        {
            Notification,
            Emoji,
            Emote,
            Mute,
            Block,
            Avatar,
            Connect,
            Disconnect,
            Portal,
            Sticker,
            Moderation,
            Online,
            Offline,
            Friend,
            Group,
            World,
            Protection,
            Camera,
            Room,
            Pedestal,
            Content,
            Queue
        }

        private static readonly List<string> CachedLogs = new();

        public static void Log(object logObj, LogsType Type)
        {
            if (logObj == null) return;

            string logMsg = logObj.ToString();

            switch (Type)
            {
                case LogsType.Notification:
                    logMsg = "<color=#dd00ff>[" + DateTime.Now.ToShortTimeString() + "] " + "[Notification]:</color>  " + logMsg;
                    break;

                case LogsType.Emoji:
                    logMsg = "<color=#475400>[" + DateTime.Now.ToShortTimeString() + "] " + "[Emoji]:</color>  " + logMsg;
                    break;

                case LogsType.Emote:
                    logMsg = "<color=#540038>[" + DateTime.Now.ToShortTimeString() + "] " + "[Emote]:</color>  " + logMsg;
                    break;

                case LogsType.Mute:
                    logMsg = "<color=#00ffea>[" + DateTime.Now.ToShortTimeString() + "] " + "[Mute]:</color>  " + logMsg;
                    break;

                case LogsType.Block:
                    logMsg = "<color=#ad0000>[" + DateTime.Now.ToShortTimeString() + "] " + "[Block]:</color>  " + logMsg;
                    break;

                case LogsType.Avatar:
                    logMsg = "<color=#4800ff>[" + DateTime.Now.ToShortTimeString() + "] " + "[Avatar]:</color>  " + logMsg;
                    break;

                case LogsType.Connect:
                    logMsg = "<color=#00ff04>[" + DateTime.Now.ToShortTimeString() + "] " + "[Connected]:</color>  " + logMsg;
                    break;

                case LogsType.Disconnect:
                    logMsg = "<color=#ff1a1a>[" + DateTime.Now.ToShortTimeString() + "] " + "[Disconnected]:</color>  " + logMsg;
                    break;

                case LogsType.Portal:
                    logMsg = "<color=#030b82>[" + DateTime.Now.ToShortTimeString() + "] " + "[Portal]:</color>  " + logMsg;
                    break;

                case LogsType.Sticker:
                    logMsg = "<color=#17e2c4>[" + DateTime.Now.ToShortTimeString() + "] " + "[Sticker]:</color>  " + logMsg;
                    break;

                case LogsType.Moderation:
                    logMsg = "<color=#426ff5>[" + DateTime.Now.ToShortTimeString() + "] " + "[Moderation]:</color>  " + logMsg;
                    break;

                case LogsType.Online:
                    logMsg = "<color=#77eb34>[" + DateTime.Now.ToShortTimeString() + "] " + "[Online]:</color>  " + logMsg;
                    break;

                case LogsType.Offline:
                    logMsg = "<color=#eb4634>[" + DateTime.Now.ToShortTimeString() + "] " + "[Offline]:</color>  " + logMsg;
                    break;

                case LogsType.Friend:
                    logMsg = "<color=#ccff00>[" + DateTime.Now.ToShortTimeString() + "] " + "[Friend]:</color>  " + logMsg;
                    break;

                case LogsType.Group:
                    logMsg = "<color=#008f8c>[" + DateTime.Now.ToShortTimeString() + "] " + "[Group]:</color>  " + logMsg;
                    break;

                case LogsType.World:
                    logMsg = "<color=#db5400>[" + DateTime.Now.ToShortTimeString() + "] " + "[World]:</color>  " + logMsg;
                    break;

                case LogsType.Protection:
                    logMsg = "<color=#c42525>[" + DateTime.Now.ToShortTimeString() + "] " + "[Protection]:</color>  " + logMsg;
                    break;

                case LogsType.Camera:
                    logMsg = "<color=#ff0073>[" + DateTime.Now.ToShortTimeString() + "] " + "[Camera]:</color>  " + logMsg;
                    break;

                case LogsType.Room:
                    logMsg = "<color=#266f70>[" + DateTime.Now.ToShortTimeString() + "] " + "[Room]:</color>  " + logMsg;
                    break;

                case LogsType.Pedestal:
                    logMsg = "<color=#ffffff>[" + DateTime.Now.ToShortTimeString() + "] " + "[Pedestal]:</color>  " + logMsg;
                    break;

                case LogsType.Content:
                    logMsg = "<color=#ff63e8>[" + DateTime.Now.ToShortTimeString() + "] " + "[Content]:</color>  " + logMsg;
                    break;

                case LogsType.Queue:
                    logMsg = "<color=#ff63a1>[" + DateTime.Now.ToShortTimeString() + "] " + "[Queue]:</color>  " + logMsg;
                    break;
            }

            CachedLogs.Insert(0, logMsg);
            if (CachedLogs.Count > 21) CachedLogs.RemoveAt(21);
            UIQuickChanges.ConsoleText?.SetText(string.Join("\n", CachedLogs));
        }
    }
}
