using UnityEngine;
using WengaPort.Api;
using WengaPort.Wrappers;
using WengaPort.Modules;
using VRC;
using System.Linq;
using WengaPort.Extensions;

namespace WengaPort.Buttons
{
    class SafetyMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMToggleButton VRToggle;
        public static QMToggleButton AntiMenuToggle;
        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(MainMenu.ThisMenu, 1.5f, 1f, "Safety", "Safety Utils", null, null, null, Color.yellow);

            new QMToggleButton(ThisMenu, 1, 0, "Forcemute", () =>
            {
                try
                {
                    PhotonModule.Forcemute = true;
                    foreach (Player instance in Utils.PlayerManager.GetAllPlayers().ToArray())
                    {
                        InteractMenu.HearOffPlayers.Add(instance.GetVRCPlayer().UserID());
                    }
                }
                catch
                {
                }
            }, "Disabled", () =>
            {
                try
                {
                    PhotonModule.Forcemute = false;
                    foreach (Player instance in Utils.PlayerManager.GetAllPlayers().ToArray())
                    {
                        InteractMenu.HearOffPlayers.Remove(instance.GetVRCPlayer().UserID());
                    }
                }
                catch
                { }
            }, "Forcemute Everyone");

            AntiMenuToggle = new QMToggleButton(ThisMenu, 2, 0, "Anti \nOverrender", () =>
            {
                AntiMenuOverrender.AntiOverrender(true);
            }, "Disabled", () =>
            {
                AntiMenuOverrender.AntiOverrender(false);
            }, "Prevents your Menu from getting Overrendered");

            new QMToggleButton(ThisMenu, 0, 0, "World \nSpoof", () =>
            {
                PatchManager.WorldSpoof = true;
            }, "Disabled", () =>
            {
                PatchManager.WorldSpoof = false;
            }, "Spoof yourself to private World");

            new QMToggleButton(ThisMenu, 0, 1, "Offline", () =>
            {
                PatchManager.OfflineMode = true;
            }, "Online", () =>
            {
                PatchManager.OfflineMode = true;
            }, "Spoof yourself Offline");

            new QMToggleButton(ThisMenu, 1, 2, "Optimize \nDynbones", () =>
            {
                GlobalDynamicBones.OptimizeBones = true;
                foreach (Player p in Utils.PlayerManager.GetAllPlayers())
                {
                    PlayerExtensions.ReloadAvatar(p);
                }
            }, "Disabled", () =>
            {
                GlobalDynamicBones.OptimizeBones = false;
                foreach (Player p in Utils.PlayerManager.GetAllPlayers())
                {
                    PlayerExtensions.ReloadAvatar(p);
                }
            }, "Optimize Dynbones instead smooth Bones");

            new QMToggleButton(ThisMenu, 2, 1, "Antiblock", () =>
            {
                PatchManager.AntiBlock = true;
            }, "Disabled", () =>
            {
                PatchManager.AntiBlock = false;
            }, "See blocked People", Color.cyan, Color.white, false, true);

            VRToggle = new QMToggleButton(ThisMenu, 4, 1, "VR", () =>
            {
                PatchManager.VRMode = true;
            }, "Desktop", () =>
            {
                PatchManager.VRMode = false;
            }, "Spoof yourself VR/Desktop");

            new QMToggleButton(ThisMenu, 4, 2, "Spoof \nFrames", () =>
            {
                PatchManager.FrameSpoof = true;
            }, "Disabled", () =>
            {
                PatchManager.FrameSpoof = false;
            }, "Spoof your FPS", Color.cyan, Color.white, false, true);

            new QMToggleButton(ThisMenu, 3, 2, "Spoof \nPing", () =>
            {
                PatchManager.PingSpoof = true;
            }, "Disabled", () =>
            {
                PatchManager.PingSpoof = false;
            }, "Spoof your Ping", Color.cyan, Color.white, false, true);

            new QMToggleButton(ThisMenu, 3, 0, "Anti \nPortal", () =>
            {
                PortalHandler.AntiPortal = true;
            }, "Disabled", () =>
            {
                PortalHandler.AntiPortal = false;
            }, "Delete all spawned Portals");

            new QMToggleButton(ThisMenu, 4, 0, "Anti \nTrigger", () =>
            {
                PatchManager.AntiWorldTrigger = true;
            }, "Disabled", () =>
            {
                PatchManager.AntiWorldTrigger = false;
            }, "Make all global Triggers local");

            new QMToggleButton(ThisMenu, 5, 0, "Friend \nPortal", () =>
            {
                PortalHandler.FriendOnlyPortal = true;
            }, "Disabled", () =>
            {
                PortalHandler.FriendOnlyPortal = false;
            }, "Delete all Portals from non friends");

            new QMToggleButton(ThisMenu, 5, 1, "Anti \nSpawnsound", () =>
            {
                AvatarProcesser.SpawnSound = false;
            }, "Disabled", () =>
            {
                AvatarProcesser.SpawnSound = true;
            }, "Disable Avatar spawn Sounds");

            new QMToggleButton(ThisMenu, 1, 1, "Anti \nDisconnect", () =>
            {
                PatchManager.AntiMasterDC = true;
            }, "Disabled", () =>
            {
                PatchManager.AntiMasterDC = false;
            }, "Disable all Always Events");

            new QMToggleButton(ThisMenu, 3, 1, "Anti \nVideoplayer", () =>
            {
                PatchManager.BlockPlayer = true;
            }, "Disabled", () =>
            {
                PatchManager.BlockPlayer = false;
            }, "Prevents you from IP Logging via Players");

            new QMToggleButton(ThisMenu, 2, 2, "Anti \nUdon", () =>
            {
                PatchManager.AntiUdon = true;
            }, "Disabled", () =>
            {
                PatchManager.AntiUdon = false;
            }, "Prevents you from Udon Exploits/Events");
        }
    }
}
