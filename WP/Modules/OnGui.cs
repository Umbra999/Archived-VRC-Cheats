using System;
using System.Diagnostics;
using UnityEngine;
using VRC.SDKBase;
using WengaPort.Extensions;

namespace WengaPort.Modules
{
    class OnGui : MonoBehaviour
    {
		public OnGui(IntPtr ptr) : base(ptr) { }
		void OnGUI()
        {
			if (!UnityEngine.XR.XRDevice.isPresent)
            {
				if (RoomManager.field_Internal_Static_ApiWorldInstance_0 == null)
				{
					Color Rainbow = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time / 2, 1), 1, 1));
					var RainbowHandler = Nameplates.ConvertRGBtoHEX(Rainbow);
					bool flag2 = GUI.Button(new Rect(Screen.width - 200, 30f, 100f, 25f), $"<color=#{RainbowHandler}>Close Game</color>", GUI.skin.box);
					if (flag2)
					{
						Process.GetCurrentProcess().Kill();
					}
					bool flag3 = GUI.Button(new Rect(Screen.width - 200, 60f, 100f, 25f), $"<color=#{RainbowHandler}>Restart Game</color>", GUI.skin.box);
					if (flag3)
					{
						try
						{
							Process.Start(Environment.CurrentDirectory + "\\VRChat.exe", Environment.CommandLine.ToString());
						}
						catch (Exception)
						{
							new Exception();
						}
						Process.GetCurrentProcess().Kill();
					}
					GUI.Label(new Rect(10f, Screen.height / 3, (Screen.width / 5), Screen.height / 2), string.Concat(new object[0]), GUI.skin.label);
				}
				else
				{
					try
					{
						Color Rainbow = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time / 2, 1), 1, 1));
						var RainbowHandler = Nameplates.ConvertRGBtoHEX(Rainbow);
						GUI.Label(new Rect(Screen.width - 130f, 10f, 500f, 500f), string.Concat(new object[]
						{
						string.Format($"<b><color=#{RainbowHandler}>WengaPort</color></b>"),
						string.Format("\n<b>In Room: {0}</b>", PlayerList.Players.Count),
						string.Format("\n<b>FPS: {0}</b>", Mathf.Round(1f / Time.deltaTime)),
						string.Format("\n<b>Actor Number: {0}</b>", Networking.LocalPlayer.playerId),
						string.Format("\n<b>Master: {0}</b>", Networking.LocalPlayer.isMaster),
						string.Format("\n<b>USpeak: {0}</b>", Utils.CurrentUser.prop_USpeaker_0.field_Public_BitRate_0),
						string.Format("\n<b>X: {0}</b>", Utils.CurrentUser.transform.position.x),
						string.Format("\n<b>Y: {0}</b>", Utils.CurrentUser.transform.position.y),
						string.Format("\n<b>Z: {0}</b>", Utils.CurrentUser.transform.position.z),
						}), GUI.skin.label);
					}
					catch { }
				}
			}
		}
    }
}
