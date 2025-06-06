using WebSocketSharpManaged;
using WebSocketSharpManaged.Server;

namespace LunaR.Buttons.Bots
{
    internal class Websocket
    {
        public class Client : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs Message)
            {
                string Data = Message.Data;
                Extensions.Logger.Log($"{Data}", Extensions.Logger.LogsType.Bot);
            }
        }

        internal class Server
        {
            public static WebSocketServer WSServer;

            public static void Initialize()
            {
                WSServer = new WebSocketServer("ws://localhost:6666");
                WSServer.AddWebSocketService<Client>("/Bot");
                WSServer.Log.Level = LogLevel.Fatal;
                WSServer.Start();
                Extensions.Logger.Log("Starting Bot Server", Extensions.Logger.LogsType.Bot);
            }

            public static void SendMessage(string Message)
            {
                if (WSServer != null) WSServer.WebSocketServices.Broadcast(Message);
            }
        }
    }
}