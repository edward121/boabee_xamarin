using System.Threading.Tasks;

using Plugin.Connectivity;

using System;
using System.Diagnostics;

namespace BoaBeePCL
{
	public static class Reachability
	{
		public static async Task<bool> isConnected()
		{
			Uri uri = null;
			try
			{
                uri = new Uri(ServerURLs.baseURL);
			}
			catch
			{
				return false;
			}

			bool isReachable = await CrossConnectivity.Current.IsRemoteReachable(uri.Host, uri.Port, 10000);
			return isReachable;
		}

		public static async Task<bool> isWebhookAvailable()
		{
			DBKioskSettings kioskSettings = DBLocalDataStore.GetInstance().GetKioskSettings();
			if (kioskSettings != null)
			{
				if (string.IsNullOrWhiteSpace(kioskSettings.badgePrintingWebhook))
				{
					return false;
				}
				else
				{
					Uri uri = null;
					try
					{
						uri = new Uri(kioskSettings.badgePrintingWebhook);
					}
					catch
					{
						return false;
					}
					DateTime beforeCheck = DateTime.Now;
					bool isAvailable = await CrossConnectivity.Current.IsRemoteReachable(uri.Host, uri.Port, 10000);
					TimeSpan checkDuration = DateTime.Now.Subtract(beforeCheck);
                    Debug.WriteLine("webhook availability check duration is {0}", checkDuration.ToString());
					//Console.WriteLine("webhook availability check duration is {0}", checkDuration.ToString());

					return isAvailable;
				}
			}
			else
			{
				return false;
			}
		}
	}
}