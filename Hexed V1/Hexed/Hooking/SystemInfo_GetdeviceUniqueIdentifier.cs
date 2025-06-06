using CoreRuntime.Manager;
using Hexed.HexedServer;
using Hexed.Interfaces;
using Il2CppInterop.Runtime;
using UnityEngine;
using VRC.Core;

namespace Hexed.Hooking
{
    internal class SystemInfo_GetdeviceUniqueIdentifier : IHook
    {
        private delegate nint _deviceUniqueIdentifierDelegate();

        public unsafe void Initialize()
        {
            OriginalLenght = SystemInfo.deviceUniqueIdentifier.Length;
            if (OriginalLenght == 0)
            {
                Wrappers.Logger.LogError("Failed to get HWID lenght");
                return;
            }

            if (APIUser.CurrentUser != null)
            {
                Wrappers.Logger.LogWarning($"Already logged in, skipping HWID Spoof");
                return;
            }

            nint mainmethod = IL2CPP.il2cpp_resolve_icall("UnityEngine.SystemInfo::GetDeviceUniqueIdentifier");

            HookManager.Detour<_deviceUniqueIdentifierDelegate>(mainmethod, Patch);
        }

        private static nint Patch()
        {
            if (!APIUser.IsLoggedIn)
            {
                if (IsTokenExisting())
                {
                    if (Core.ConfigManager.Ini.GetString("Spoof", "HWID").Length != OriginalLenght || Core.ConfigManager.Ini.GetString("Spoof", "Token") != ApiCredentials.GetString("authToken"))
                    {
                        GenerateAndSetHWID();
                        SetToken();
                    }

                    else if (FakeHWID == null) SetHWIDFromConfig();
                }

                else if (!SetFreshHWID)
                {
                    SetFreshHWID = true;
                    GenerateAndSetHWID();
                }
            }

            else if (SetFreshHWID)
            {
                SetFreshHWID = false;
                SetToken();
            }

            if (FakeHWID == null || OriginalLenght != FakeHWID.ToString().Length || OriginalLenght == 0)
            {
                Wrappers.Logger.LogError("Error generating HWID, restart your game");
                Thread.Sleep(-1);
            }

            return FakeHWID.Pointer;
        }

        private static Il2CppSystem.Object FakeHWID;
        private static bool SetFreshHWID = false;
        private static int OriginalLenght = 0;

        private static string GenerateHWID()
        {
            byte[] bytes = new byte[OriginalLenght / 2];
            EncryptUtils.Random.NextBytes(bytes);
            return string.Join("", bytes.Select(it => it.ToString("x2")));
        }

        private static bool IsTokenExisting()
        {
            return ApiCredentials.GetString("authToken").StartsWith("authcookie_");
        }

        private static void GenerateAndSetHWID()
        {
            Core.ConfigManager.Ini.SetString("Spoof", "HWID", GenerateHWID());
            FakeHWID = new(IL2CPP.ManagedStringToIl2Cpp(Core.ConfigManager.Ini.GetString("Spoof", "HWID")));
            Wrappers.Logger.Log($"Generated and Spoofed HWID: {FakeHWID.ToString()}", Wrappers.Logger.LogsType.Protection);
        }

        private static void SetToken()
        {
            Core.ConfigManager.Ini.SetString("Spoof", "Token", ApiCredentials.GetString("authToken"));
            VRC.Tools.ClearCookies();
            VRC.Tools.ClearExpiredBestHTTPCache();
            ApiCache.Clear();
            ApiCache.Cleanup();
        }

        private static void SetHWIDFromConfig()
        {
            FakeHWID = new(IL2CPP.ManagedStringToIl2Cpp(Core.ConfigManager.Ini.GetString("Spoof", "HWID")));
            Wrappers.Logger.Log($"Spoofed HWID from Config: {FakeHWID.ToString()}", Wrappers.Logger.LogsType.Protection);
        }
    }
}
