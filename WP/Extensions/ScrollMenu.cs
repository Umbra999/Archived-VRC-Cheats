using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WengaPort.Api;
using Object = UnityEngine.Object;

namespace WengaPort.Extensions
{
    public class ScrollMenu
    {
        public class ScrollObject
        {
            public QMButtonBase ButtonBase;
            public int Index;
        }
        public QMNestedButton BaseMenu;
        public QMSingleButton NextButton;
        public QMSingleButton BackButton;
        public QMSingleButton IndexButton;
        public List<ScrollObject> QMButtons = new List<ScrollObject>();
        private int Posx = 1;
        private int Posy = 0;
        private int Pos = 0;
        private int Index = 0;
        public int currentMenuIndex = 0;


        public bool ShouldChangePos = false;
        public bool AllowOverStepping = false;
        public ScrollMenu(QMNestedButton Base)
        {
            BaseMenu = Base;
            IndexButton = new QMSingleButton(BaseMenu, 5, 0.5f, "Page:\n" + (currentMenuIndex + 1).ToString() + " of " + (Index + 1).ToString(), delegate { }, "");
            IndexButton.getGameObject().GetComponentInChildren<Button>().enabled = false;
            IndexButton.getGameObject().GetComponentInChildren<Image>().enabled = false;
            BackButton = new QMSingleButton(BaseMenu, 5, 1f, "Back", () =>
            {
                ShowMenu(currentMenuIndex - 1);
            }, "Go Back");
            BackButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            NextButton = new QMSingleButton(BaseMenu, 5, 0, "Next", () =>
            {
                ShowMenu(currentMenuIndex + 1);
            }, "Go Next");
            NextButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
        }
        public void ShowMenu(int MenuIndex)
        {
            if (!AllowOverStepping && (MenuIndex < 0 || MenuIndex > Index))
            {
                return;
            }
            foreach (var item in QMButtons)
            {
                if (item.Index == MenuIndex)
                    item.ButtonBase?.setActive(true);
                else
                    item.ButtonBase?.setActive(false);
            }
            currentMenuIndex = MenuIndex;
            IndexButton.setButtonText("Page:\n" + (currentMenuIndex + 1).ToString() + " of " + (Index + 1).ToString());
        }
        public void SetAction(Action Open)
        {
            BaseMenu.getMainButton().setAction(new Action(() =>
            {
                Clear();
                Open();
                ShowMenu(0);
                QMStuff.ShowQuickmenuPage(BaseMenu.getMenuName());
            }));
        }
        private void Clear()
        {
            try
            {
                if (QMButtons != null)
                {
                    foreach (var item in QMButtons)
                        Object.Destroy(item.ButtonBase.getGameObject());
                    QMButtons.Clear();
                }
                Posx = 1;
                Posy = 0;
                Pos = 0;
                Index = 0;
                currentMenuIndex = 0;
            }
            catch {}
        }
        public void Add(QMButtonBase Button)
        {
            if (Posx < 6)
            {
                Posx++;
            }
            if (Posx > 5 && Posy < 3)
            {
                Posx = 2;
                Posy++;
            }
            if (Pos == 12)
            {
                Posx = 2;
                Posy = 0;
                Pos = 0;
                Index++;
            }
            Button.getGameObject().transform.SetParent(QMStuff.GetQuickMenuInstance().transform.Find(BaseMenu.getMenuName()));
            if(!ShouldChangePos)
                Button.setLocation(Posx, Posy);
            Button.setActive(false);
            QMButtons.Add(new ScrollObject()
            {
                ButtonBase = Button,
                Index = Index
            });
            Pos++;
        }

    }
}
