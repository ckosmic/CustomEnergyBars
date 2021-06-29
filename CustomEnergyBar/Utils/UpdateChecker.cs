using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CustomEnergyBar.Utils
{
	internal class UpdateChecker
	{
		public static Version localVersion = new Version(Assembly.GetExecutingAssembly().GetName().Version.ToString(3));

		private static HttpClient client = new HttpClient();

		public static async Task<Release> GetNewestReleaseAsync(CancellationToken token) {
			try {
				client.DefaultRequestHeaders.UserAgent.ParseAdd("Chrome / 23.0.1271.95");
				HttpResponseMessage response = await client.GetAsync("https://api.github.com/repos/ckosmic/CustomEnergyBars/releases", token);
				if (response.IsSuccessStatusCode) {
					string body = await response.Content.ReadAsStringAsync();
					Release[] releases = JsonConvert.DeserializeObject<Release[]>(body);
					return releases[0];
				} else {
					Plugin.Log.Info("Failed to retrieve a response from GitHub.");
				}
				return null;
			} catch (Exception e) {
				Plugin.Log.Error(e);
				return null;
			}
		}

		public static int CompareRelease(Release release) {
			return new Version(release.tagName).CompareTo(localVersion);
		}

		internal class Release {
			[JsonProperty("tag_name")]
			public string tagName;
			[JsonProperty("html_url")]
			public string url;
		}
	}
}
