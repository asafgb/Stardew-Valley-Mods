using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace LockChest.Common
{
    internal class UpdateNotifier
    {
        IModHelper Helper;
        private readonly IMonitor Monitor;
        private string gitUrlmanifestJson = "https://raw.githubusercontent.com/asafgb/Stardew-Valley-Mods/master/GiveItem/GiveItems/manifest.json";

        public UpdateNotifier(IMonitor monitor, IModHelper Helper)
        {
            this.Monitor = monitor;
            this.Helper = Helper;
        }

        public async void Check(IManifest manifest)
        {
            try
            {
                if ((await this.GetCurrentVersion(manifest.UniqueID).ConfigureAwait(false)).IsNewerThan(manifest.Version))
                {
                    this.NotifyNewVersion(manifest.Name);
                }
                else
                {
                    this.NotifyUpToDate(manifest.Name);
                }
            }
            catch (Exception ex)
            {
                this.NotifyFailure(manifest.Name, ex.Message);
            }
        }

        private void NotifyNewVersion(string modName)
        {
            string message = string.Format("A new version of {0} is available!", modName);
            this.Monitor.Log(message, LogLevel.Alert);
            Helper.Events.GameLoop.SaveLoaded += (sender, e) => { this.ShowHudMessage(message); };

        }



        private void NotifyUpToDate(string modName)
        {
            this.Monitor.Log(string.Format("{0} is up to date.", modName), LogLevel.Info);
        }

        private void NotifyFailure(string modName, string reason)
        {
            this.Monitor.Log(string.Format("Update check failed for {0}. Please check for a new version manually. ({1})", modName, reason), LogLevel.Warn);
            Helper.Events.GameLoop.SaveLoaded += (sender, e) => { this.ShowHudMessage(string.Format("Update check failed for {0}", modName)); };
        }

        private void ShowHudMessage(string message)
        {
            Game1.addHUDMessage(new HUDMessage(message, Color.Red, 3500f)
            {
                noIcon = true,
                timeLeft = 3500f
            });
        }

        private async Task<ISemanticVersion> GetCurrentVersion(string uniqueId)
        {
            HttpWebRequest httpWebRequest = WebRequest.CreateHttp(this.GetManifestUrl(uniqueId));
            httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            AssemblyName name = typeof(UpdateNotifier).Assembly.GetName();
            httpWebRequest.UserAgent = string.Format("{0}/{1}", name.Name, name.Version);
            ISemanticVersion result;
            using (WebResponse webResponse = await httpWebRequest.GetResponseAsync())
            {
                using (Stream responseStream = webResponse.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream))
                    {
                        JToken jtoken = JObject.Parse(streamReader.ReadToEnd()).SelectToken("Version");
                        result = new SemanticVersion(jtoken.Value<int>("MajorVersion"), jtoken.Value<int>("MinorVersion"), jtoken.Value<int>("PatchVersion"), null);
                    }
                }
            }
            return result;
        }

        private string GetManifestUrl(string uniqueId)
        {
            return string.Format("");
        }

    }
}
