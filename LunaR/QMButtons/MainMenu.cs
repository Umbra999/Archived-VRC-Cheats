using LunaR.Api;
using LunaR.Modules;
using LunaR.Wrappers;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LunaR.QMButtons
{
    internal class MainMenu
    {
        public static QMWingToggle FlyToggle;
        public static QMWingToggle NoClipToggle;
        public static QMNestedButton ClientMenu;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        public static void Init()
        {
            ClientMenu = new("Menu_Dashboard", 2, 1, "Main Client", "Open the Client Menu", "LunaR Client");
            ClientMenu.GetMainButton().SetActive(false);
            new QMIconButton("Menu_Dashboard", 0.75f, -0.75f, delegate
            {
                ClientMenu.OpenMe();
            }, "LunaR Menu", GeneralWrappers.GetSprite("moon"));

            new QMIconButton(ClientMenu, 1.5f, 3.25f, delegate
            {
                keybd_event(177, 0, 1U, IntPtr.Zero);
            }, "Previous Media", GeneralWrappers.GetSprite("MediaPrevious"));

            new QMIconButton(ClientMenu, 2f, 3.25f, delegate
            {
                keybd_event(174, 0, 1U, IntPtr.Zero);
            }, "Volume Down Media", GeneralWrappers.GetSprite("MediaDown"));

            new QMIconButton(ClientMenu, 2.5f, 3.25f, delegate
            {
                keybd_event(179, 0, 1U, IntPtr.Zero);
            }, "Play / Pause Media", GeneralWrappers.GetSprite("MediaPlay"));

            new QMIconButton(ClientMenu, 3f, 3.25f, delegate
            {
                keybd_event(175, 0, 1U, IntPtr.Zero);
            }, "Volume Up Media", GeneralWrappers.GetSprite("MediaUp"));

            new QMIconButton(ClientMenu, 3.5f, 3.25f, delegate
            {
                keybd_event(176, 0, 1U, IntPtr.Zero);
            }, "Next Media", GeneralWrappers.GetSprite("MediaNext"));
        }

        public static void InitWings()
        {
            QMWingMenu LeftClientWing = new("LunaR", QMWingMenu.WingState.Left);
            QMWingButton LeftMainWing = new(APIStuff.GetLeftWing().field_Public_RectTransform_0.Find("WingMenu/ScrollRect/Viewport/VerticalLayoutGroup"), "LunaR", "LunaR Client", delegate { LeftClientWing.Open(); }, GeneralWrappers.GetSprite("moon"));

            new QMWingButton(LeftClientWing, "Copy \nWorldID", "Copy the current WorldID", delegate
            {
                Clipboard.SetText($"{GeneralWrappers.GetWorldID()}");
                Extensions.Logger.Log(GeneralWrappers.GetWorldID(), Extensions.Logger.LogsType.Info);
            }, GeneralWrappers.GetSprite("blur"));

            new QMWingButton(LeftClientWing, "Join \nWorldID", "Join the WorldID", delegate
            {
                if (Clipboard.ContainsText()) Utils.Forcejoin(Clipboard.GetText());
            }, GeneralWrappers.GetSprite("blur"));

            new QMWingButton(LeftClientWing, "Delete \nPortals", "Delete all Portals", delegate
            {
                PortalHandler.DeletePortals();
            }, GeneralWrappers.GetSprite("blur"));

            FlyToggle = new QMWingToggle(LeftClientWing, "Fly", "Toggle Fly", delegate (bool toggle)
            {
                if (toggle) Movement.FlyEnable();
                else Movement.FlyDisable();
            });

            NoClipToggle = new QMWingToggle(LeftClientWing, "NoClip", "Toggle NoClip", delegate (bool toggle)
            {
                if (toggle) Movement.NoClipEnable();
                else Movement.NoClipDisable();
            });
        }
    }
}