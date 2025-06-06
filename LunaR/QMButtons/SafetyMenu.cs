using LunaR.Api;
using LunaR.Extensions;
using LunaR.Modules;
using LunaR.Patching;
using LunaR.Wrappers;
using System;

namespace LunaR.QMButtons
{
    internal class SafetyMenu
    {
        public static void Init()
        {
            QMNestedButton ThisMenu = new(MainMenu.ClientMenu, 1, 0, "Safety", "Safety Menu", "Safety Options", QMButtonAPI.ButtonSize.Default, GeneralWrappers.GetSprite("Safety"));

            new QMToggleButton(ThisMenu, 1, 0, "Anti \nDestroy", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "AntiDestroy", true);
                PatchExtensions.AntiDestroy = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "AntiDestroy", false);
                PatchExtensions.AntiDestroy = false;
            }, "Prevent Players from destroying Objects", ConfigManager.Ini.GetBool("Toggles", "AntiDestroy"));

            new QMToggleButton(ThisMenu, 2, 0, "Unload \nUdon", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "AntiUdonSync", true);
                PatchExtensions.AntiUdonSync = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "AntiUdonSync", false);
                PatchExtensions.AntiUdonSync = false;
            }, "Prevents Udon from syncing you", ConfigManager.Ini.GetBool("Toggles", "AntiUdonSync"));

            new QMToggleButton(ThisMenu, 4, 0, "See \nBlocked", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "SeeBlocked", true);
                ModerationHandler.SeeBlocked = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "SeeBlocked", false);
                ModerationHandler.SeeBlocked = false;
            }, "See Players which you blocked", ConfigManager.Ini.GetBool("Toggles", "SeeBlocked"));

            new QMToggleButton(ThisMenu, 1, 1, "Local \nTrigger", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "AntiTrigger", true);
                PatchExtensions.AntiWorldTrigger = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "AntiTrigger", false);
                PatchExtensions.AntiWorldTrigger = false;
            }, "Make all Global Triggers local", ConfigManager.Ini.GetBool("Toggles", "AntiTrigger"));

            new QMToggleButton(ThisMenu, 2, 1, "Anti \nSpawnsound", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "AntiAudio", true);
                AvatarAdjustment.AntiSpawnsound = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "AntiAudio", false);
                AvatarAdjustment.AntiSpawnsound = false;
            }, "Stop all Avatar Spawnsounds", ConfigManager.Ini.GetBool("Toggles", "AntiAudio"));

            new QMToggleButton(ThisMenu, 4, 1, "Anti \nUdon", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "AntiUdon", true);
                PatchExtensions.AntiUdon = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "AntiUdon", false);
                PatchExtensions.AntiUdon = false;
            }, "Prevent Udon Events", ConfigManager.Ini.GetBool("Toggles", "AntiUdon"));

            new QMToggleButton(ThisMenu, 2, 2, "Hidden \nCamera", delegate
            {
                PatchExtensions.HiddenCamera = true;
                ConfigManager.Ini.SetBool("Toggles", "AntiCamera", true);
            }, delegate
            {
                PatchExtensions.HiddenCamera = false;
                ConfigManager.Ini.SetBool("Toggles", "AntiCamera", false);
            }, "Hide your Camera from other Players", ConfigManager.Ini.GetBool("Toggles", "AntiCamera"));

            new QMToggleButton(ThisMenu, 3, 2, "Mute \nQuest", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "MuteQuest", true);
                PatchExtensions.MuteQuest = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "MuteQuest", false);
                PatchExtensions.MuteQuest = false;
            }, "Mute all Quest Users", ConfigManager.Ini.GetBool("Toggles", "MuteQuest"));

            new QMToggleButton(ThisMenu, 4, 2, "No Portal \nFollow", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "PortalFollow", true);
                PortalHandler.NoPortalFollow = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "PortalFollow", false);
                PortalHandler.NoPortalFollow = false;
            }, "Destroy the Portal after you entered it", ConfigManager.Ini.GetBool("Toggles", "PortalFollow"));

            new QMToggleButton(ThisMenu, 3, 3, "No \nChairs", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoChairs", true);
                PatchExtensions.NoChairs = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoChairs", false);
                PatchExtensions.NoChairs = false;
            }, "Block Chairs from being used", ConfigManager.Ini.GetBool("Toggles", "NoChairs"));

            new QMToggleButton(ThisMenu, 2, 3, "Anti \nTimer", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "AntiTimer", true);
                PatchExtensions.AntiTimer = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "AntiTimer", false);
                PatchExtensions.AntiTimer = true;
            }, "Block Camera Timer Sounds", ConfigManager.Ini.GetBool("Toggles", "AntiTimer"));

            new QMToggleButton(ThisMenu, 3, 1, "Udon \nSpoof", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "UdonSpoof", true);
                PatchExtensions.UdonSpoof = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "UdonSpoof", false);
                PatchExtensions.UdonSpoof = false;
            }, "Spoof yourself as World Author", ConfigManager.Ini.GetBool("Toggles", "UdonSpoof"));

            new QMToggleButton(ThisMenu, 1, 2, "Anti \nMimic", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "AntiMimic", true);
                PatchExtensions.AntiMimic = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "AntiMimic", false);
                PatchExtensions.AntiMimic = false;
            }, "Prevent People from copying your Movement", ConfigManager.Ini.GetBool("Toggles", "AntiMimic"));

            new QMToggleButton(ThisMenu, 3, 0, "Anti \nEmoji", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "AntiEmoji", true);
                PatchExtensions.AntiEmoji = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "AntiEmoji", false);
                PatchExtensions.AntiEmoji = false;
            }, "Toggle Player Emojis", ConfigManager.Ini.GetBool("Toggles", "AntiEmoji"));
        }
    }
}