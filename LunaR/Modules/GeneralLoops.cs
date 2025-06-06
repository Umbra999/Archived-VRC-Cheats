using LunaR.Api;
using LunaR.Buttons.Bots;
using LunaR.ButtonUI;
using LunaR.ConsoleUtils;
using LunaR.Extensions;
using LunaR.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC;

namespace LunaR.Modules
{
    internal class GeneralLoops : MonoBehaviour
    {
        public static float RoomTime = 0;
        private static float Delay = 0;
        private static VRCUiManagerExtension.UIHudText UIText;

        public void Start()
        {
            DestroyAnalytics().Start();

            UIText = new("", -65, -95, TextAnchor.UpperCenter);
            UIText.text.color = Color.blue;
        }

        public void Update()
        {
            if (GeneralWrappers.IsInWorld())
            {
                RoomTime += Time.deltaTime;

                Delay += Time.deltaTime;
                if (Delay < 1) return;
                Delay = 0;

                if (APIStuff.GetQuickMenuInstance().isActiveAndEnabled)
                {
                    TimeSpan span = TimeSpan.FromSeconds((double)new decimal(RoomTime));
                    string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}", span.Hours, span.Minutes, span.Seconds);
                    QMRedesign.LowerPannel.text = $"T: {DateTime.Now.ToLongTimeString()}   R: {elapsedTime}   Actor: {Utils.VRCNetworkingClient.GetSelf().ActorID()}";
                }

                int FPS = (int)(1.0f / Time.deltaTime);
                UIText.text.text = $"<b>FPS: {FPS}</b>";

                if (PatchExtensions.LastServerEvent < Environment.TickCount - 3500 && PatchExtensions.LastServerEvent != 0)
                {
                    PatchExtensions.BlockOPRaise = true;
                    Extensions.Logger.Log($"The Server is lagging", Extensions.Logger.LogsType.Protection);
                    VRConsole.Log(VRConsole.LogsType.Protection, $"Server --> Lagging [{Environment.TickCount - PatchExtensions.LastServerEvent} ms]");
                }

                foreach (int Actor in PatchExtensions.SerializedActors.Keys)
                {
                    if (PatchExtensions.SerializedActors[Actor] < Environment.TickCount - 1700)
                    {
                        if (!PatchExtensions.NonSerializedActors.Contains(Actor)) PatchExtensions.NonSerializedActors.Add(Actor);
                    }
                    else if (PatchExtensions.NonSerializedActors.Contains(Actor)) PatchExtensions.NonSerializedActors.Remove(Actor);
                }

                if (Utils.CurrentUser != null && PatchExtensions.IsBadPosition(Utils.CurrentUser.transform.position))
                {
                    Extensions.Logger.Log($"Respawned because of Bad Position", Extensions.Logger.LogsType.Protection);
                    VRConsole.Log(VRConsole.LogsType.Protection, $"Position --> Out of bounds");
                    SpawnManager.prop_SpawnManager_0.Method_Public_Void_VRCPlayer_0(Utils.CurrentUser);
                }
            }
            else
            {
                GameObject LoadingSkybox = GameObject.Find("SkyCube_Baked");
                if (LoadingSkybox != null) DestroyImmediate(LoadingSkybox);
            }
        }

        private static IEnumerator DestroyAnalytics()
        {
            while (GameObject.Find("_Application/AnalyticsManager") == null) yield return null;

            DestroyImmediate(GameObject.Find("_Application/AnalyticsManager"), true);
            Extensions.Logger.Log("Analytics Disabled", Extensions.Logger.LogsType.Protection);
        }

        public static IEnumerator FixThings()
        {
            RoomTime = 0f;
            while (Utils.CurrentUser == null || Utils.CurrentUser.GetVRCPlayerApi() == null) yield return new WaitForSeconds(1);
            if (ItemHandler.InfinitRangeToggle) ItemHandler.InfinitRange(true);
            if (ItemHandler.NoPickup) Utils.CurrentUser.GetVRCPlayerApi().EnablePickups(false);
            if (ESP.PickupESP) ESP.ItemESP().Start();
            if (ESP.TriggerESPToggle) ESP.TriggerESP().Start();
            if (Movement.SpeedToggle) Movement.ToggleSpeed(true);
        }

        public GeneralLoops(IntPtr ptr) : base(ptr)
        {
        }
    }
}