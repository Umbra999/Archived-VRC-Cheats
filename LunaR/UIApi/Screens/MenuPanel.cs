using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LunaR.UIApi
{
    internal class MenuPanel
    {
        private readonly Dictionary<int, Action> PageActions = new();
        private readonly Dictionary<int, string> PageTitles = new();
        private int CurrentPageIndex;

        public GameObject Panel;
        public Text Title;
        public Text Description;
        public Text State;

        public MenuPanel(GameObject Page, string Name, string Desc, string Value, float x_pos, float y_pos)
        {
            Panel = UnityEngine.Object.Instantiate(GameObject.Find("UserInterface/MenuContent/Screens/Settings/AudioDevicePanel"), Page.transform);
            Panel.name = Name;
            Panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(x_pos, y_pos);

            foreach (Il2CppSystem.Object obj in Panel.transform)
            {
                Transform btnEnum = obj.Cast<Transform>();
                if (btnEnum != null && (btnEnum.gameObject.name.Contains("VolumeDisplay") || btnEnum.gameObject.name.Contains("VolumeSlider"))) UnityEngine.Object.Destroy(btnEnum.gameObject);
            }

            Title = Panel.transform.Find("TitleText").GetComponent<Text>();
            Title.text = Name;
            Panel.transform.Find("SelectNextMic").GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            Action NextAction = delegate 
            {
                if (CurrentPageIndex < PageActions.Count - 1)
                {
                    CurrentPageIndex++;
                    State.text = PageTitles[CurrentPageIndex];
                    InvokePageAction(CurrentPageIndex);
                }
            }; 
            Panel.transform.Find("SelectNextMic").GetComponent<Button>().onClick.AddListener(NextAction);

            Panel.transform.Find("SelectPrevMic").GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            Action PrevAction = delegate 
            {
                if (CurrentPageIndex > 0)
                {
                    CurrentPageIndex--;
                    State.text = PageTitles[CurrentPageIndex];
                    InvokePageAction(CurrentPageIndex);
                }
            };
            Panel.transform.Find("SelectPrevMic").GetComponent<Button>().onClick.AddListener(PrevAction);

            Description = Panel.transform.Find("LevelText").GetComponent<Text>();
            Description.text = Desc;

            Panel.GetComponent<MonoBehaviourPublicBuTeBuIEVoBoOnVoBoIEUnique>().enabled = false;
            State = Panel.transform.Find("MicDeviceText").GetComponent<Text>();
            State.text = Value;

            CurrentPageIndex = 0;
            State.text = "Disabled";
        }

        public void SetPanelIndex(int Index, bool Invoke = false)
        {
            CurrentPageIndex = Index;
            State.text = PageTitles[Index];
            if (Invoke) InvokePageAction(CurrentPageIndex);
        }

        public void AddPageAction(string Title, Action Action)
        {
            int Index = PageActions.Count;
            State.text = Title;
            PageActions.Add(Index, Action);
            PageTitles.Add(Index, Title);
        }

        private void InvokePageAction(int ActionAtIndex)
        {
            PageActions[ActionAtIndex]?.Invoke();
        }
    }
}