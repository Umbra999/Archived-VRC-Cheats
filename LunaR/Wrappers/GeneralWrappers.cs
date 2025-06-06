using LunaR.Buttons.Bots;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.XR;
using VRC.Core;
using VRC.SDK3.Components;
using VRC.SDKBase;

namespace LunaR.Wrappers
{
    public static class GeneralWrappers
    {

        public static List<GameObject> GetWorldPrefabs()
        {
            return VRC_SceneDescriptor.Instance.DynamicPrefabs.ToArray().ToList();
        }

        public static List<Material> GetWorldMaterials()
        {
            return VRC_SceneDescriptor.Instance.DynamicMaterials.ToArray().ToList();
        }

        public static bool IsInVr()
        {
            return XRDevice.isPresent;
        }

        public static bool IsInWorld()
        {
            return RoomManager.field_Internal_Static_ApiWorldInstance_0 != null;
        }

        public static string GetWorldID()
        {
            if (!IsInWorld()) return null;
            return $"{RoomManager.field_Internal_Static_ApiWorld_0.id}:{RoomManager.field_Internal_Static_ApiWorldInstance_0.instanceId}";
        }

        public static InstanceAccessType GetWorldType(string WorldID)
        {
            if (WorldID.Contains("~canRequestInvite")) return InstanceAccessType.InvitePlus;
            if (WorldID.Contains("~private")) return InstanceAccessType.InviteOnly;
            if (WorldID.Contains("~friends")) return InstanceAccessType.FriendsOnly;
            if (WorldID.Contains("~hidden")) return InstanceAccessType.FriendsOfGuests;
            return InstanceAccessType.Public;
        }

        public static bool IsIPLogger(string url)
        {
            foreach (string text in new List<string>
            {
                "lovebird.guru",
                "trulove.guru",
                "dateing.club",
                "shrekis.life",
                "headshot.monster",
                "gaming-at-my.best",
                "progaming.monster",
                "yourmy.monster",
                "imageshare.best",
                "screenshot.best",
                "gamingfun.me",
                "catsnthing.com",
                "catsnthings.fun",
                "curiouscat.club",
                "joinmy.site",
                "freegiftcards.co",
                "stopify.co",
                "leancoding.co",
                "grabify.link",
            })
            {
                if (url.ToLower().Contains(text.ToLower())) return true;
            }
            return false;
        }

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null) return gameObject.AddComponent<T>();
            return component;
        }

        public static Sprite GetSprite(string resourceName)
        {
            var texture = GetTexture(resourceName);
            var rect = new Rect(0.0f, 0.0f, texture.width, texture.height);
            var pivot = new Vector2(0.5f, 0.5f);
            var border = Vector4.zero;
            var sprite = Sprite.CreateSprite_Injected(texture, ref rect, ref pivot, 100.0f, 0, SpriteMeshType.Tight, ref border, false);
            sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return sprite;
        }

        private static Texture2D GetTexture(string resourceName)
        {
            var resourcePath = $"LunaR.Resources.{resourceName}.png";
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath);
            if (stream == null)
            {
                Extensions.Logger.LogError($"Failed to find texture {resourceName}");
                return null;
            }

            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            var texture = new Texture2D(1, 1);
            ImageConversion.LoadImage(texture, ms.ToArray());
            texture.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            texture.wrapMode = TextureWrapMode.Clamp;
            return texture;
        }

        internal static bool IsUdonWorld()
        {
            if (GetSDK3Descriptor() != null) return true;
            return false;
        }

        internal static VRCSDK2.VRC_SceneDescriptor GetSDK2Descriptor()
        {
            return UnityEngine.Object.FindObjectOfType<VRCSDK2.VRC_SceneDescriptor>();
        }

        internal static VRCSceneDescriptor GetSDK3Descriptor()
        {
            return UnityEngine.Object.FindObjectOfType<VRCSceneDescriptor>();
        }

        public static void RestartGame()
        {
            Process.Start("VRChat.exe", Environment.CommandLine.ToString());
            Application.Quit();
        }

        public static string ToRGBHex(Color c)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}", ToByte(c.r), ToByte(c.g), ToByte(c.b));
        }

        public static Color GetColor(string Hex)
        {
            ColorUtility.TryParseHtmlString(Hex, out Color C);
            return C;
        }

        private static byte ToByte(float f)
        {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }
    }
}