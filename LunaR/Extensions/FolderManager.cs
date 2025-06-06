using LunaR.Wrappers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using VRC.Core;

namespace LunaR.Extensions
{
    internal class FolderManager
    {
        private static long GetDirectorySize(string folderPath)
        {
            DirectoryInfo di = new(folderPath);
            return di.EnumerateFiles("*", SearchOption.AllDirectories).Sum(fi => fi.Length);
        }

        public static void Initialize()
        {
            string filePath = System.Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\LocalLow\VRChat\VRChat\Cache-WindowsPlayer");
            bool NeedsRestart = false;

            using WebClient webClient = new();
            if (!Directory.Exists("LunaR")) Directory.CreateDirectory("LunaR");

            ConfigManager.Init();

            if (!File.Exists("BouncyCastle.Crypto.dll"))
            {
                webClient.DownloadFile("https://cdn.discordapp.com/attachments/855072998796296212/976553926000279642/BouncyCastle.Crypto.dll", Path.Combine(System.Environment.CurrentDirectory, "BouncyCastle.Crypto.dll"));
                NeedsRestart = true;
            }
            if (!File.Exists("LunaR\\LunaR.nmasset"))
            {
                webClient.DownloadFile("https://cdn.discordapp.com/attachments/846666528626704415/846682009866731550/FadeAway.nmasset", Path.Combine(System.Environment.CurrentDirectory, "LunaR/LunaR.nmasset"));
            }
            if (!File.Exists("LunaR\\LoadingPicture.jpg"))
            {
                webClient.DownloadFile("https://cdn.discordapp.com/attachments/855072998796296212/997352795973898300/LoadingPicture.jpg", Path.Combine(System.Environment.CurrentDirectory, "LunaR/LoadingPicture.jpg"));
            }
            if (!File.Exists("LunaR\\Background.jpg"))
            {
                webClient.DownloadFile("https://cdn.discordapp.com/attachments/855072998796296212/997352311905075310/Background.jpg", Path.Combine(System.Environment.CurrentDirectory, "LunaR/Background.jpg"));
            }
            if (!File.Exists("websocket-sharp2.dll"))
            {
                webClient.DownloadFile("https://cdn.discordapp.com/attachments/855072998796296212/895719711029993482/websocket-sharp2.dll", Path.Combine(System.Environment.CurrentDirectory, "websocket-sharp2.dll"));
                NeedsRestart = true;
            }
            if (!Directory.Exists("LunaR\\VRCA")) Directory.CreateDirectory("LunaR\\VRCA");
            if (!Directory.Exists("LunaR\\Photon"))
            {
                Directory.CreateDirectory("LunaR\\Photon");
                Directory.CreateDirectory("LunaR\\Photon\\Music");
            }
            if (Directory.Exists(filePath))
            {
                if (GetDirectorySize(filePath) >= 18547937170)
                {
                    DirectoryInfo fi = new(filePath);
                    foreach (FileInfo file in fi.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in fi.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                    Logger.Log("Cache Cleaned to improve Performance", Logger.LogsType.Info);
                }
            }
            if (!File.Exists("LunaR\\AvatarFavorites.json"))
            {
                string contents = JsonConvert.SerializeObject(new List<ApiAvatar>(), Formatting.Indented);
                File.WriteAllText("LunaR\\AvatarFavorites.json", contents);
            }
            if (!File.Exists("LunaR\\AvatarHistory.json"))
            {
                string contents = JsonConvert.SerializeObject(new List<ApiAvatar>(), Formatting.Indented);
                File.WriteAllText("LunaR\\AvatarHistory.json", contents);
            }

            if (NeedsRestart) GeneralWrappers.RestartGame();
        }
    }
}