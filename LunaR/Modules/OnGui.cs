using LunaR.Buttons.Bots;
using LunaR.Wrappers;
using System;
using System.Diagnostics;
using UnityEngine;

namespace LunaR.Modules
{
    internal class OnGui : MonoBehaviour
    {
        public OnGui(IntPtr ptr) : base(ptr)
        {
        }

        private void OnGUI()
        {
            if (!GeneralWrappers.IsInWorld() || Input.GetKey(KeyCode.Tab))
            {
                if (GUI.Button(new Rect(Screen.width - 200, 30f, 100f, 25f), $"<color=#152bb3>Close Game</color>", GUI.skin.box)) Application.Quit();
                if (GUI.Button(new Rect(Screen.width - 200, 60f, 100f, 25f), $"<color=#152bb3>Restart Game</color>", GUI.skin.box)) GeneralWrappers.RestartGame();
                GUI.Label(new Rect(10f, Screen.height / 3, Screen.width / 5, Screen.height / 2), string.Concat(new object[0]), GUI.skin.label);
            }
        }
    }
}