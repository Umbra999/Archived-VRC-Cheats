using Hexed.Core;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Hexed.Modules.Standalone
{
    internal class ExternalConsole
    {
        static NetworkStream stream;

        public static void Init()
        {
            if (!File.Exists(ConfigManager.BaseFolder + "\\Assets\\ExternalConsole.exe")) return;

            var customWriter = new CustomTextWriter();
            Console.SetOut(customWriter);

            Process.Start(ConfigManager.BaseFolder + "\\Assets\\ExternalConsole.exe");

            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];

            TcpClient client = new();

            client.Connect(ipAddress, 666);

            stream = client.GetStream();
        }

        static public void writeMessage(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            if (stream != null) stream.Write(data, 0, data.Length);
        }

        public class CustomTextWriter : TextWriter
        {
            public override Encoding Encoding => Encoding.ASCII;

            public override void Write(string value)
            {
                writeMessage(value);
            }
        }
    }
}
