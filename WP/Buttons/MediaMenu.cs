using WengaPort.Api;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using VRC.SDKBase;
using System;
using System.Windows.Forms;

namespace WengaPort.Buttons
{
    class MediaMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMSingleButton HalfButton;

        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(MainMenu.ThisMenu, 1.5f, 0f, "Media", "Videoplayer/Media Menu", null, null, null, Color.yellow);
            ThisMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 1, 0, "Play\nVideo", () =>
            {
                var videoPlayers = Resources.FindObjectsOfTypeAll<VRCSDK2.VRC_SyncVideoPlayer>();

                if (videoPlayers.Count() > 0)
                    Networking.RPC(RPC.Destination.Owner, videoPlayers[0].gameObject, "Play", new Il2CppSystem.Object[0]);
            }, "Play the current Video in the Videoplayer");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 2, 0, "Clear\nVideo", () =>
            {
                var videoPlayers = Resources.FindObjectsOfTypeAll<VRCSDK2.VRC_SyncVideoPlayer>();

                if (videoPlayers.Count() > 0)
                    Networking.RPC(RPC.Destination.Owner, videoPlayers[0].gameObject, "Clear", new Il2CppSystem.Object[0]);
            }, "Clear the current Video in the Videoplayer");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 3, 0, "Stop\nVideo", () =>
            {
                var videoPlayers = Resources.FindObjectsOfTypeAll<VRCSDK2.VRC_SyncVideoPlayer>();

                if (videoPlayers.Count() > 0)
                    Networking.RPC(RPC.Destination.Owner, videoPlayers[0].gameObject, "Stop", new Il2CppSystem.Object[0]);
            }, "Stop the current Video in the Videoplayer");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 4, 0, "Pause\nVideo", () =>
            {
                var videoPlayers = Resources.FindObjectsOfTypeAll<VRCSDK2.VRC_SyncVideoPlayer>();

                if (videoPlayers.Count() > 0)
                    Networking.RPC(RPC.Destination.Owner, videoPlayers[0].gameObject, "Pause", new Il2CppSystem.Object[0]);
            }, "Pause the current Video in the Videoplayer");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 1, 0.5f, "Speed Up\nVideo", () =>
            {
                var videoPlayers = Resources.FindObjectsOfTypeAll<VRCSDK2.VRC_SyncVideoPlayer>();

                if (videoPlayers.Count() > 0)
                    Networking.RPC(RPC.Destination.Owner, videoPlayers[0].gameObject, "SpeedUp", new Il2CppSystem.Object[0]);
            }, "Speed Up the current Video in the Videoplayer");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 2, 0.5f, "Speed Down\nVideo", () =>
            {
                var videoPlayers = Resources.FindObjectsOfTypeAll<VRCSDK2.VRC_SyncVideoPlayer>();

                if (videoPlayers.Count() > 0)
                    Networking.RPC(RPC.Destination.Owner, videoPlayers[0].gameObject, "SpeedDown", new Il2CppSystem.Object[0]);
            }, "Pause the current Video in the Videoplayer");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 3, 0.5f, "Next\nVideo", () =>
            {
                var videoPlayers = Resources.FindObjectsOfTypeAll<VRCSDK2.VRC_SyncVideoPlayer>();

                if (videoPlayers.Count() > 0)
                    Networking.RPC(RPC.Destination.Owner, videoPlayers[0].gameObject, "Next", new Il2CppSystem.Object[0]);
            }, "Play the next Video in the Videoplayer");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 4, 0.5f, "Previous\nVideo", () =>
            {
                var videoPlayers = Resources.FindObjectsOfTypeAll<VRCSDK2.VRC_SyncVideoPlayer>();

                if (videoPlayers.Count() > 0)
                    Networking.RPC(RPC.Destination.Owner, videoPlayers[0].gameObject, "Previous", new Il2CppSystem.Object[0]);
            }, "Play the previous Video in the Videoplayer");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 1, 1, "Add\nVideo", () =>
            {
                string url = Clipboard.GetText();
                var videoPlayers = Resources.FindObjectsOfTypeAll<VRCSDK2.VRC_SyncVideoPlayer>();

                if (videoPlayers.Count() > 0)
                {
                    Networking.RPC(RPC.Destination.Owner, videoPlayers[0].gameObject, "AddURL", new Il2CppSystem.Object[]
                    {
                        (Il2CppSystem.String)url
                    });
                }
            }, "Add a Video to the Videoplayer");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 2, 1, "Kill\nPlayers", () =>
            {
                var videoPlayers = Resources.FindObjectsOfTypeAll<VRCSDK2.VRC_SyncVideoPlayer>();
                if (videoPlayers.Count() > 0)
                {
                    videoPlayers[0].gameObject.SetActive(false);
                }
            }, "Disable all current Videoplayers");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 2.5f, 1.75f, "► ||", () =>
            {
                MainMenu.keybd_event(179, 0, 1U, IntPtr.Zero);
            }, "Play / Pause the current Song");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);
            HalfButton.getGameObject().GetComponentInChildren<Text>().fontSize /= 2;

            HalfButton = new QMSingleButton(ThisMenu, 3f, 1.75f, "▲", () =>
            {
                MainMenu.keybd_event(175, 0, 1U, IntPtr.Zero);
            }, "Increase System Volume incrementally up to 100%");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);
            HalfButton.getGameObject().GetComponentInChildren<Text>().fontSize /= 2;

            HalfButton = new QMSingleButton(ThisMenu, 2f, 1.75f, "▼", () =>
            {
                MainMenu.keybd_event(174, 0, 1U, IntPtr.Zero);
            }, "Decrease System Volume incrementally down to 0%");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);
            HalfButton.getGameObject().GetComponentInChildren<Text>().fontSize /= 2;

            HalfButton = new QMSingleButton(ThisMenu, 3.5f, 1.75f, "► ►", () =>
            {
                MainMenu.keybd_event(176, 0, 1U, IntPtr.Zero);
            }, "Play Next Media");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);
            HalfButton.getGameObject().GetComponentInChildren<Text>().fontSize /= 2;

            HalfButton = new QMSingleButton(ThisMenu, 1.5f, 1.75f, "◄ ◄", () =>
            {
                MainMenu.keybd_event(177, 0, 1U, IntPtr.Zero);
            }, "Play Previous Song"); ;
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 2);
            HalfButton.getGameObject().GetComponentInChildren<Text>().fontSize /= 2;
        }
    }
}
