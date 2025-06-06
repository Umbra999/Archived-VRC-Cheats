using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WengaPort.ConsoleUtils;

namespace WengaPort.Extensions
{
    class Logger
    {
        public static void WengaLogger(object obj) 
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] [WengaPort] {obj}");
        }

        public static IEnumerator WebsocketLogger(VRConsole.LogsType LogType, object obj)
        {
            if (obj == null)
            {
                yield break;
            }
            while (ConvertWebsocket) yield return new WaitForSeconds(1);
            ConvertWebsocket = true;
            yield return new WaitForSeconds(1);
            ConvertWebsocket = false;
            WebsocketOutput.Add(obj);
            foreach (object message in WebsocketOutput)
            {
                VRConsole.Log(LogType, message.ToString());
            }
            WebsocketOutput.Clear();
        }
        public static List<object> WebsocketOutput = new List<object>();

        public static bool ConvertWebsocket = false;
    }
}
