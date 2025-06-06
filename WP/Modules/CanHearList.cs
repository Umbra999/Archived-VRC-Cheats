using WengaPort.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC;
using WengaPort.Wrappers;

namespace WengaPort.Modules
{
    internal class CanHearList : MonoBehaviour
    {
        public void Start()
        {
            float num = posy;
            Transform parent = Utils.QuickMenu.transform.Find("ShortcutMenu");
            PlayerCount = new MenuText(parent, posx, -500f, "<b>Hearlist: </b>");
            for (int i = 0; i <= 38; i++)
            {
                MenuText item = new MenuText(parent, posx, posy, "");
                Logs.Add(item);
                posy += 70f;
            }
            posy = num;
        }

        public static void AddPlayerToList()
        {
            try
            {
                Players.Clear();
                foreach (Player Player in Utils.PlayerManager.GetAllPlayers().ToArray())
                {
                    float Distance = Vector3.Distance(Utils.CurrentUser.transform.position, Player.transform.position);
                    if (Distance < 26.5f)
                    {
                        if (Player.UserID() != Utils.CurrentUser.UserID())
                        {
                            string Text;
                            Text = $"<color=#00d4f0>{Player.DisplayName()}</color> [Q] {PlayerExtensions.GetQualityColored(Player.GetVRCPlayer())} [R] {Distance}";
                            Players.Insert(0, Text);
                        }
                    }
                }
                PlayerCount.SetText(string.Format("<b>Hear you: {0}</b>", Players.Count));
                UpdateText();
            }
            catch { }
        }
        public float PlayerDelay = 0f;
        public void Update()
        {
            PlayerDelay += Time.deltaTime;
            if (PlayerDelay > 2f)
            {
                if (Utils.QuickMenu.transform.Find("ShortcutMenu").gameObject.active == true)
                {
                    AddPlayerToList();
                }
                PlayerDelay = 0f;
            }
        }

        public static void UpdateText()
        {
            try
            {
                for (int i = 0; i <= Logs.Count; i++)
                {
                    try
                    {
                        if (Players[i] != null)
                        {
                            Logs[i].SetText(Players[i]);
                        }
                    }
                    catch
                    {
                        Logs[i].SetText("");
                    }
                }
            }
            catch { }
        }

        public static List<string> Players = new List<string>();

        public static List<MenuText> Logs = new List<MenuText>();

        public static MenuText PlayerCount;

        public static float posx = 2600;

        public static float posy = -400f;

        public CanHearList(IntPtr ptr) : base(ptr) { }
    }
}
