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

namespace Commands.Util
{
    internal class UpdateNotifier
    {
        IModHelper _helper;
        // Token: 0x0600001B RID: 27 RVA: 0x00002638 File Offset: 0x00000838
        public UpdateNotifier(IMonitor monitor, IModHelper helper)
        {
            this.Monitor = monitor;
            _helper = helper;
        }

        // Token: 0x0600001C RID: 28 RVA: 0x00002648 File Offset: 0x00000848
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

        // Token: 0x0600001D RID: 29 RVA: 0x0000268C File Offset: 0x0000088C
        private void NotifyNewVersion(string modName)
        {
            string message = string.Format("A new version of {0} is available!", modName);
            this.Monitor.Log(message, LogLevel.Alert);
            _helper.Events.GameLoop.SaveLoaded += delegate (object sender, SaveLoadedEventArgs e)
            {
                this.ShowHudMessage(message);
            };
        }

        // Token: 0x0600001E RID: 30 RVA: 0x000026DA File Offset: 0x000008DA
        private void NotifyUpToDate(string modName)
        {
            this.Monitor.Log(string.Format("{0} is up to date.", modName), LogLevel.Info);
        }

        // Token: 0x0600001F RID: 31 RVA: 0x000026F4 File Offset: 0x000008F4
        private void NotifyFailure(string modName, string reason)
        {
            this.Monitor.Log(string.Format("Update check failed for {0}. Please check for a new version manually. ({1})", modName, reason), LogLevel.Warn);
            _helper.Events.GameLoop.SaveLoaded += delegate (object sender, SaveLoadedEventArgs e)
            {
                this.ShowHudMessage(string.Format("Update check failed for {0}", modName));
            };
        }

        // Token: 0x06000020 RID: 32 RVA: 0x00002743 File Offset: 0x00000943
        private void ShowHudMessage(string message)
        {
            Game1.addHUDMessage(new HUDMessage(message, Color.Red, 3500f)
            {
                noIcon = true,
                timeLeft = 3500f
            });
        }

        // Token: 0x06000021 RID: 33 RVA: 0x0000276C File Offset: 0x0000096C
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

        // Token: 0x06000022 RID: 34 RVA: 0x000027B9 File Offset: 0x000009B9
        private string GetManifestUrl(string uniqueId)
        {
            return string.Format("https://raw.githubusercontent.com/doncollins/StardewValleyMods/stable/{0}/manifest.json", uniqueId);
        }

        // Token: 0x0400000B RID: 11
        private readonly IMonitor Monitor;
    }
}