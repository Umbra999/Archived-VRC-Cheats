using LunaR.Api;
using LunaR.Extensions;
using LunaR.Modules;
using LunaR.Wrappers;
using System;
using System.Runtime.InteropServices;

namespace LunaR.QMButtons
{
    internal class DebugMenu
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void Init()
        {
            QMNestedButton ThisMenu = new(MainMenu.ClientMenu, 2.5f, 1, "Debug", "Debug Menu", "Debug Options", QMButtonAPI.ButtonSize.Default, GeneralWrappers.GetSprite("Debug"));

            new QMToggleButton(ThisMenu, 1, 0, "RPC \nLog", delegate
            {
                PatchExtensions.RPCLog = true;
            }, delegate
            {
                PatchExtensions.RPCLog = false;
            }, "Log Decoded RPC Events");

            new QMToggleButton(ThisMenu, 2, 0, "Event \nLog", delegate
            {
                PatchExtensions.EventLog = true;
            }, delegate
            {
                PatchExtensions.EventLog = false;
            }, "Log Photon Events");

            new QMToggleButton(ThisMenu, 3, 0, "Operaion \nLog", delegate
            {
                PatchExtensions.OperationLog = true;
            }, delegate
            {
                PatchExtensions.OperationLog = false;
            }, "Log Photon Operations");

            new QMToggleButton(ThisMenu, 4, 0, "No \nConsole", delegate
            {
                ShowWindow(GetConsoleWindow(), 0);
            }, delegate
            {
                ShowWindow(GetConsoleWindow(), 5);
            }, "Disable the Windows Console");

            new QMToggleButton(ThisMenu, 1, 1, "Enable \nStats", delegate
            {
                PatchExtensions.ShowStats = true;
            }, delegate
            {
                PatchExtensions.ShowStats = false;
            }, "Toggle Performance Scanners");

            new QMToggleButton(ThisMenu, 2, 1, "OpRaise \nLog", delegate
            {
                PatchExtensions.OpRaiseLogger = true;
            }, delegate
            {
                PatchExtensions.OpRaiseLogger = false;
            }, "Log OpRaise Events");

            new QMToggleButton(ThisMenu, 3, 1, "Websocket \nLog", delegate
            {
                PatchExtensions.WebsocketLog = true;
            }, delegate
            {
                PatchExtensions.WebsocketLog = false;
            }, "Log Websocket Data");

            new QMSingleButton(ThisMenu, 4, 1, "Set \nTag", delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("Tag", "Ok", delegate (string text)
                {
                    ServerRequester.SetUserTag(text);
                });
            }, "Set your Client Tag", null, QMButtonAPI.ButtonSize.Half);

            new QMToggleButton(ThisMenu, 1, 2.2f, "Event 1", delegate
            {
                PatchExtensions.LogIgnoreCodes.Add(1);
            }, delegate
            {
                PatchExtensions.LogIgnoreCodes.Remove(1);
            }, "Log the Event Code", true);

            new QMToggleButton(ThisMenu, 2, 2.2f, "Event 6", delegate
            {
                PatchExtensions.LogIgnoreCodes.Add(6);
            }, delegate
            {
                PatchExtensions.LogIgnoreCodes.Remove(6);
            }, "Log the Event Code");

            new QMToggleButton(ThisMenu, 3, 2.2f, "Event 7", delegate
            {
                PatchExtensions.LogIgnoreCodes.Add(7);
            }, delegate
            {
                PatchExtensions.LogIgnoreCodes.Remove(7);
            }, "Log the Event Code", true);

            new QMToggleButton(ThisMenu, 4, 2.2f, "Event 8", delegate
            {
                PatchExtensions.LogIgnoreCodes.Add(8);
            }, delegate
            {
                PatchExtensions.LogIgnoreCodes.Remove(8);
            }, "Log the Event Code", true);

            new QMToggleButton(ThisMenu, 1, 3.15f, "Event 9", delegate
            {
                PatchExtensions.LogIgnoreCodes.Add(9);
            }, delegate
            {
                PatchExtensions.LogIgnoreCodes.Remove(9);
            }, "Log the Event Code");

            new QMToggleButton(ThisMenu, 2, 3.15f, "Event 209", delegate
            {
                PatchExtensions.LogIgnoreCodes.Add(209);
            }, delegate
            {
                PatchExtensions.LogIgnoreCodes.Remove(209);
            }, "Log the Event Code");

            new QMToggleButton(ThisMenu, 3, 3.15f, "Event 210", delegate
            {
                PatchExtensions.LogIgnoreCodes.Add(210);
            }, delegate
            {
                PatchExtensions.LogIgnoreCodes.Remove(210);
            }, "Log the Event Code");

            new QMToggleButton(ThisMenu, 4, 3.15f, "Event 35", delegate
            {
                PatchExtensions.LogIgnoreCodes.Add(35);
            }, delegate
            {
                PatchExtensions.LogIgnoreCodes.Remove(35);
            }, "Log the Event Code", true);
        }
    }
}