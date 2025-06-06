using System;

namespace LunaR.Extensions
{
    internal class Logger
    {
        public enum LogsType
        {
            Clean,
            Info,
            Protection,
            Moderation,
            Avatar,
            API,
            Bot,
        }

        public static void Log(object obj, LogsType Type)
        {
            string log = obj.ToString().Replace("\a", "a").Replace("\u001B[", "u001B[");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"[{DateTime.Now.ToShortTimeString()}] [");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("LunaR");
            Console.ForegroundColor = ConsoleColor.Blue;
            if (Type != LogsType.Clean) Console.Write($"] [{Type}] {log}\n");
            else Console.Write($"] {log}\n");
        }

        public static void LogError(object obj)
        {
            string log = obj.ToString().Replace("\a", "");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"[{DateTime.Now.ToShortTimeString()}] [");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("LunaR");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"] ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{log}\n");
        }
    }
}