using Hexed.Core;
using Hexed.UIApi;
using Hexed.Wrappers;

namespace Hexed.CustomUI.QM
{
    internal class SafetyMenu
    {
        private static QMMenuPage SafetyPage;
        public static void Init()
        {
            SafetyPage = new("Safety");

            QMSingleButton OpenMenu = new(MainMenu.ClientPage, 1, 0, "Safety", SafetyPage.OpenMe, "Safety Options", ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Safety"));

            new QMToggleButton(SafetyPage, 1, 0, "No \nDestroy", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoObjectDestroy", true);
                InternalSettings.NoObjectDestroy = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoObjectDestroy", false);
                InternalSettings.NoObjectDestroy = false;
            }, "Prevent Players from destroying Objects", ConfigManager.Ini.GetBool("Toggles", "NoObjectDestroy"));

            new QMToggleButton(SafetyPage, 2, 0, "No \nEmojis", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoEmojiSpawn", true);
                InternalSettings.NoEmojiSpawn = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoEmojiSpawn", false);
                InternalSettings.NoEmojiSpawn = false;
            }, "Prevent Emoji spawns", ConfigManager.Ini.GetBool("Toggles", "NoEmojiSpawn"));

            new QMToggleButton(SafetyPage, 3, 0, "Anti \nMimic", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "SpoofMovementLenght", true);
                InternalSettings.ObfuscateMovement = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "SpoofMovementLenght", false);
                InternalSettings.ObfuscateMovement = false;
            }, "Obfuscate your Movement Events", ConfigManager.Ini.GetBool("Toggles", "SpoofMovementLenght"));

            new QMToggleButton(SafetyPage, 4, 0, "Mute \nCamera", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoCameraTimer", true);
                InternalSettings.NoCameraSound = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoCameraTimer", false);
                InternalSettings.NoCameraSound = false;
            }, "Prevent Camera Sounds", ConfigManager.Ini.GetBool("Toggles", "NoCameraTimer"));

            new QMToggleButton(SafetyPage, 1, 1, "No Udon\nEvents", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoUdonEvents", true);
                InternalSettings.NoUdonEvents = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoUdonEvents", false);
                InternalSettings.NoUdonEvents = false;
            }, "Prevent Udon Events", ConfigManager.Ini.GetBool("Toggles", "NoUdonEvents"));

            new QMToggleButton(SafetyPage, 2, 1, "No \nVideo", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoVideoPlayer", true);
                InternalSettings.NoVideoPlayer = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoVideoPlayer", false);
                InternalSettings.NoVideoPlayer = false;
            }, "Prevent Videoplayers from loading", ConfigManager.Ini.GetBool("Toggles", "NoVideoPlayer"));

            new QMToggleButton(SafetyPage, 3, 1, "No Udon\nSync", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoUdonSync", true);
                InternalSettings.NoUdonSync = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoUdonSync", false);
                InternalSettings.NoUdonSync = false;
            }, "Prevent Udon Sync", ConfigManager.Ini.GetBool("Toggles", "NoUdonSync"));

            new QMToggleButton(SafetyPage, 4, 1, "Anti \nBlock", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "AntiBlock", true);
                InternalSettings.AntiBlock = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "AntiBlock", false);
                InternalSettings.AntiBlock = false;
            }, "See Players which blocked you", ConfigManager.Ini.GetBool("Toggles", "AntiBlock"));

            QMSelectButton AntiPortalMenu = new(SafetyPage, 1, 2, "Anti \nPortal", "Prevent Portals being dropped", InternalSettings.AntiPortal, ConfigManager.Ini.GetInt("Toggles", "AntiPortalMode"));
            AntiPortalMenu.AddAction(0, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "AntiPortalMode", 0);
                InternalSettings.AntiPortal = InternalSettings.AntiPortalMode.None;
            });
            AntiPortalMenu.AddAction(1, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "AntiPortalMode", 1);
                InternalSettings.AntiPortal = InternalSettings.AntiPortalMode.All;
            });
            AntiPortalMenu.AddAction(2, delegate
            {
                ConfigManager.Ini.SetInt("Toggles", "AntiPortalMode", 2);
                InternalSettings.AntiPortal = InternalSettings.AntiPortalMode.Friends;
            });

            new QMToggleButton(SafetyPage, 2, 2, "No \nScaling", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoUdonScaling", true);
                InternalSettings.NoUdonScaling = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoUdonScaling", false);
                InternalSettings.NoUdonScaling = false;
            }, "Disable Udon scaling", ConfigManager.Ini.GetBool("Toggles", "NoUdonScaling"));

            new QMToggleButton(SafetyPage, 3, 2, "Silent \nMute", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "SilentMute", true);
                InternalSettings.SilentMute = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "SilentMute", false);
                InternalSettings.SilentMute = false;
            }, "Mute Player's without them knowing", ConfigManager.Ini.GetBool("Toggles", "SilentMute"));

            new QMToggleButton(SafetyPage, 4, 2, "Anti \nOverride", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "AntiOverride", true);
                InternalSettings.AntiOverride = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "AntiOverride", false);
                InternalSettings.AntiOverride = false;
            }, "Prevent overrides on your Audio Controller", ConfigManager.Ini.GetBool("Toggles", "AntiOverride"));

            new QMToggleButton(SafetyPage, 1, 3, "God \nMode", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "GodMode", true);
                InternalSettings.GodMode = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "GodMode", false);
                InternalSettings.GodMode = false;
            }, "Prevent taking Damage", ConfigManager.Ini.GetBool("Toggles", "GodMode"));

            new QMToggleButton(SafetyPage, 2, 3, "No \nTeleport", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoTeleport", true);
                InternalSettings.NoTeleport = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoTeleport", false);
                InternalSettings.NoTeleport = false;
            }, "Prevent getting Teleported", ConfigManager.Ini.GetBool("Toggles", "NoTeleport"));

            new QMToggleButton(SafetyPage, 3, 3, "No Udon \nDownload", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoUdonDownload", true);
                InternalSettings.NoUdonDownload = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "NoUdonDownload", false);
                InternalSettings.NoUdonDownload = false;
            }, "Prevent Udon downloads", ConfigManager.Ini.GetBool("Toggles", "NoUdonDownload"));

            new QMToggleButton(SafetyPage, 4, 3, "Movement \nRedirect", delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "MovementRedirect", true);
                InternalSettings.MovementRedirect = true;
            }, delegate
            {
                ConfigManager.Ini.SetBool("Toggles", "MovementRedirect", false);
                InternalSettings.MovementRedirect = false;
            }, "Redirect own Movement to a different Event", ConfigManager.Ini.GetBool("Toggles", "MovementRedirect"));
        }
    }
}
