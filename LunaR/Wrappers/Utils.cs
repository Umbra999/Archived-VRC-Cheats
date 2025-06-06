using LunaR.Patching;
using MelonLoader;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.Management;
using VRC.UserCamera;

namespace LunaR.Modules
{
    internal static class Utils
    {
        public static IEnumerator DelayAction(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action();
        }

        public static object Start(this IEnumerator e)
        {
            return MelonCoroutines.Start(e);
        }

        public static VRCUiPopupManager VRCUiPopupManager
        {
            get
            {
                return VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0;
            }
        }

        public static VRCUiManager VRCUiManager
        {
            get
            {
                return VRCUiManager.field_Private_Static_VRCUiManager_0;
            }
        }

        public static NotificationManager NotificationManager
        {
            get
            {
                return NotificationManager.field_Private_Static_NotificationManager_0;
            }
        }

        public static NetworkManager NetworkManager
        {
            get
            {
                return NetworkManager.field_Internal_Static_NetworkManager_0;
            }
        }

        public static PlayerManager PlayerManager
        {
            get
            {
                return PlayerManager.prop_PlayerManager_0;
            }
        }

        public static ModerationManager ModerationManager
        {
            get
            {
                return ModerationManager.prop_ModerationManager_0;
            }
        }

        public static VRCPlayer CurrentUser
        {
            get
            {
                return VRCPlayer.field_Internal_Static_VRCPlayer_0;
            }
        }

        public static VRCNetworkingClient VRCNetworkingClient
        {
            get
            {
                return VRCNetworkingClient.field_Internal_Static_VRCNetworkingClient_0;
            }
        }

        public static UserCameraController UserCameraController
        {
            get
            {
                return UserCameraController.field_Internal_Static_UserCameraController_0;
            }
        }

        public static System.Random Random = new(Environment.TickCount);

        public static string RandomString(int length)
        {
            char[] array = "abcdefghlijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToArray();
            string text = string.Empty;
            for (int i = 0; i < length; i++)
            {
                text += array[Random.Next(array.Length)].ToString();
            }
            return text;
        }

        public static string RandomNumberString(int length)
        {
            string text = string.Empty;
            for (int i = 0; i < length; i++)
            {
                text += Random.Next(0, 9).ToString("X8");
            }
            return text;
        }

        public static void Forcejoin(string id)
        {
            id = id.Trim('\r', '\n', ' ');
            if (id.Contains("worldId=") && id.Contains("&instanceId="))
            {
                int worldIdIndex = id.IndexOf("worldId=");
                int instanceIdIndex = id.IndexOf("&instanceId=");
                string worldId = id.Substring(worldIdIndex + "worldId=".Length, instanceIdIndex - (worldIdIndex + "worldId=".Length));
                string instanceId = id.Substring(instanceIdIndex + "&instanceId=".Length);
                Forcejoin($"{worldId}:{instanceId}");
                return;
            }

            if (!EventValidation.CheckWorldID(id)) return;
            string[] array = id.Split(':');
            if (array.Length == 2) new PortalInternal().Method_Private_Void_String_String_PDM_0(array[0], array[1]);
        }

        public static void SendWebHook(string URL, string MSG)
        {
            Task.Run(async delegate
            {
                var req = new
                {
                    content = MSG
                };

                HttpClient CurrentClient = new(new HttpClientHandler { UseCookies = false });
                HttpRequestMessage Payload = new(HttpMethod.Post, URL);
                string joinWorldBody = JsonConvert.SerializeObject(req);
                Payload.Content = new StringContent(joinWorldBody, Encoding.UTF8, "application/json");
                Payload.Headers.Add("User-Agent", "LunaR");
                HttpResponseMessage Response = await CurrentClient.SendAsync(Payload);
            });
        }

        public static void SendEmbedWebHook(string URL, object[] MSG)
        {
            Task.Run(async delegate
            {
                var req = new
                {
                    embeds = MSG
                };

                HttpClient CurrentClient = new(new HttpClientHandler { UseCookies = false });
                HttpRequestMessage Payload = new(HttpMethod.Post, URL);
                string joinWorldBody = JsonConvert.SerializeObject(req);
                Payload.Content = new StringContent(joinWorldBody, Encoding.UTF8, "application/json");
                Payload.Headers.Add("User-Agent", "LunaR");
                HttpResponseMessage Response = await CurrentClient.SendAsync(Payload);
            });
        }
    }
}