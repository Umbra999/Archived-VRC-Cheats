using LunaR.Api;
using LunaR.Extensions;
using LunaR.Modules;
using LunaR.UIApi;
using LunaR.Wrappers;
using UnityEngine;

namespace LunaR.QMButtons
{
    internal class ERPMenu
    {
        private static Lovense.Toy RemoteToy;
        private static Lovense.Toy LocalToy;

        public static void Init()
        {
            QMNestedButton ThisMenu = new(MainMenu.ClientMenu, 3.5f, 1, "ERP", "ERP Menu", "ERP Options", QMButtonAPI.ButtonSize.Default, GeneralWrappers.GetSprite("ERP"));

            UIMenuText RemoteText = new(ThisMenu.GetMenuObject(), "Remote Options", new Vector2(0, 360), 32, false, TextAnchor.UpperCenter);
            RemoteText.text.fontStyle = FontStyle.Bold;
            UIMenuText LocalText = new(ThisMenu.GetMenuObject(), "Local Options", new Vector2(0, -150), 32, false, TextAnchor.UpperCenter);
            LocalText.text.fontStyle = FontStyle.Bold;

            new QMToggleButton(ThisMenu, 1, 0, "Connect", delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("Link", "Ok", delegate (string text)
                {
                    RemoteToy = Lovense.CreateToy(text, false);
                });
            }, delegate
            {
                if (RemoteToy != null)
                {
                    Lovense.DisconnectToy(RemoteToy);
                    RemoteToy = null;
                }
            }, "Connect to Lovense");

            new QMToggleButton(ThisMenu, 2, 0, "Trigger \nIntensity", delegate
            {
                if (RemoteToy != null) RemoteToy.TriggerControl = true;
            }, delegate
            {
                if (RemoteToy != null) RemoteToy.TriggerControl = false;
            }, "Control the Intensity with your VR Trigger");

            new QMToggleButton(ThisMenu, 3, 0, "Touch \nIntensity", delegate
            {
                if (RemoteToy != null) RemoteToy.TouchControl = true;
            }, delegate
            {
                if (RemoteToy != null) RemoteToy.TouchControl = false;
            }, "Control the Intensity with Touching");

            new QMToggleButton(ThisMenu, 4, 0, "Gesture \nIntensity", delegate
            {
                if (RemoteToy != null) RemoteToy.GestureContol = true;
            }, delegate
            {
                if (RemoteToy != null) RemoteToy.GestureContol = false;
            }, "Control the Intensity with your Gestures");

            new QMToggleButton(ThisMenu, 1, 1, "Friends \nOnly", delegate
            {
                if (RemoteToy != null) RemoteToy.FriendsOnly = true;
            }, delegate
            {
                if (RemoteToy != null) RemoteToy.FriendsOnly = false;
            }, "Only Friends can control the Lovense");

            new QMToggleButton(ThisMenu, 1, 2.5f, "Connect", delegate
            {
                Utils.VRCUiPopupManager.AskInGameInput("Link", "Ok", delegate (string text)
                {
                    LocalToy = Lovense.CreateToy(text, true);
                });
            }, delegate
            {
                if (LocalToy != null)
                {
                    Lovense.DisconnectToy(LocalToy);
                    LocalToy = null;
                }
            }, "Connect to Lovense");

            new QMToggleButton(ThisMenu, 2, 2.5f, "Gesture \nIntensity", delegate
            {
                if (LocalToy != null) LocalToy.GestureContol = true;
            }, delegate
            {
                if (LocalToy != null) LocalToy.GestureContol = false;
            }, "Control the Intensity with your Gestures");

            new QMToggleButton(ThisMenu, 3, 2.5f, "Touch \nIntensity", delegate
            {
                if (LocalToy != null) LocalToy.TouchControl = true;
            }, delegate
            {
                if (LocalToy != null) LocalToy.TouchControl = false;
            }, "Control the Intensity with Touching");

            new QMToggleButton(ThisMenu, 4, 2.5f, "Friends \nOnly", delegate
            {
                if (LocalToy != null) LocalToy.FriendsOnly = true;
            }, delegate
            {
                if (LocalToy != null) LocalToy.FriendsOnly = false;
            }, "Only Friends can control the Lovense");
        }
    }
}