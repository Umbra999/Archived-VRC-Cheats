using System;
using System.Net;
using VRC.Core;

namespace WengaPort.Modules
{
    class RippingHandler
    {
		public static void DownloadAvatar(ApiAvatar avatar)
		{
			try
			{
				string assetUrl = avatar.assetUrl;
				string imageUrl = avatar.imageUrl;
				using (WebClient webClient = new WebClient())
				{
					webClient.DownloadFileAsync(new Uri(assetUrl), "WengaPort\\VRCA\\" + avatar.name + ".vrca");
				}
				using (WebClient webClient2 = new WebClient())
				{
					webClient2.DownloadFileAsync(new Uri(imageUrl), "WengaPort\\VRCA\\" + avatar.name + ".png");
				}
				Extensions.Logger.WengaLogger("[VRCA] Downloaded " + avatar.name);
			}
			catch (Exception)
			{
				Extensions.Logger.WengaLogger("[VRCA]Download Error");
			}
		}

		public static void DownloadWorld(ApiWorld world)
		{
			try
			{
				string assetUrl = world.assetUrl;
				string imageUrl = world.imageUrl;
				using (WebClient webClient = new WebClient())
				{
					webClient.DownloadFileAsync(new Uri(assetUrl), "WengaPort\\VRCA\\" + world.name + ".vrca");
				}
				using (WebClient webClient2 = new WebClient())
				{
					webClient2.DownloadFileAsync(new Uri(imageUrl), "WengaPort\\VRCA\\" + world.name + ".png");
				}
				Extensions.Logger.WengaLogger("[VRCA] Downloaded " + world.name);
			}
			catch (Exception)
			{
				Extensions.Logger.WengaLogger("[VRCA]Download Error");
			}
		}
	}
}
