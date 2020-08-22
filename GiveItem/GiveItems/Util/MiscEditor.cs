using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Util
{
    /// <summary>
    /// When the Strings\\UI is loaded
    /// It gonna put more data asset to it from the code
    /// </summary>
    class MiscEditor : IAssetEditor
    {
        //private const string LocationsPath = "Data\\Locations";
        private const string UIPath = "Strings\\UI";
        private readonly IModHelper _helper;

        public MiscEditor(IModHelper helper)
        {
            _helper = helper;
        }

        public bool CanEdit<T>(IAssetInfo asset)
        {
            return /*asset.AssetNameEquals(LocationsPath) ||*/ asset.AssetNameEquals(UIPath);
        }

        public void Edit<T>(IAssetData asset)
        {
            //if (asset.AssetNameEquals(LocationsPath))
            //{
            //    var data = asset.AsDictionary<string, string>().Data;
            //    data["newarea"] = data["Beach"];
            //}
            //else
            if (asset.AssetNameEquals(UIPath))
            {
                var data = asset.AsDictionary<string, string>().Data;
                data.Add("Chat_Custom.Chat", "{0}");
                //data.Add("Chat_StardewAquarium.FishDonated", "aaaaaa {0} {1}");
                //data.Add("Chat_StardewAquarium.AchievementUnlocked", "bbbb {0} {1} ");
            }
        }
    }
}