using Hexed.Core;
using Hexed.Modules;
using Hexed.Modules.LeapMotion;
using Hexed.UIApi;
using Hexed.Wrappers;

namespace Hexed.CustomUI.QM
{
    internal class DebugMenu
    {
        private static QMMenuPage DebugPage;
        public static void Init()
        {
            DebugPage = new("Debug");

            QMSingleButton OpenMenu = new(MainMenu.ClientPage, 2.5f, 1, "Debug", DebugPage.OpenMe, "Debug Options", ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Debug"));

            new QMToggleButton(DebugPage, 1, 0, "OpRaise \nLog", delegate
            {
                InternalSettings.OpRaiseLog = true;
            }, delegate
            {
                InternalSettings.OpRaiseLog = false;
            }, "Log OpRaise Events");

            new QMToggleButton(DebugPage, 2, 0, "OnEvent \nLog", delegate
            {
                InternalSettings.OnEventLog = true;
            }, delegate
            {
                InternalSettings.OnEventLog = false;
            }, "Log Events");

            new QMToggleButton(DebugPage, 3, 0, "Operation \nLog", delegate
            {
                InternalSettings.OperationLog = true;
            }, delegate
            {
                InternalSettings.OperationLog = false;
            }, "Log Operations");

            new QMToggleButton(DebugPage, 4, 0, "RPC \nLog", delegate
            {
                InternalSettings.RPCLog = true;
            }, delegate
            {
                InternalSettings.RPCLog = false;
            }, "Log RPCs");

            new QMToggleButton(DebugPage, 1, 1, "API \nLog", delegate
            {
                InternalSettings.APILog = true;
            }, delegate
            {
                InternalSettings.APILog = false;
            }, "Log API requests");

            new QMToggleButton(DebugPage, 2, 1, "Socket \nLog", delegate
            {
                InternalSettings.SocketLog = true;
            }, delegate
            {
                InternalSettings.SocketLog = false;
            }, "Log Socket notifications");

            new QMToggleButton(DebugPage, 3, 1, "Show \nPerformance", delegate
            {
                InternalSettings.ShowPerformanceStats = true;
            }, delegate
            {
                InternalSettings.ShowPerformanceStats = false;
            }, "Show Avatar Performance");

            new QMToggleButton(DebugPage, 4, 1, "OP Response \nLog", delegate
            {
                InternalSettings.OperationResponseLog = true;
            }, delegate
            {
                InternalSettings.OperationResponseLog = false;
            }, "Log Operation Responses");
        }
    }
}
