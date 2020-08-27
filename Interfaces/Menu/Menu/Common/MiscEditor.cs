using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Common
{
    class MiscEditor : IAssetEditor
    {
        //private const string LocationsPath = "Data\\Locations";
        private const string UIPath = "Modded\\UI";
        private readonly IModHelper _helper;

        public MiscEditor(IModHelper helper)
        {
            _helper = helper;
            //throw new Exception("Asa");
        }

        public bool CanEdit<T>(IAssetInfo asset)
        {
            return /*asset.AssetNameEquals(LocationsPath) ||*/ asset.AssetNameEquals(UIPath);
        }

        public void Edit<T>(IAssetData asset)
        {
            //else
            if (asset.AssetNameEquals(UIPath))
            {
                var data = asset.AsDictionary<string, string>().Data;
                data.Add("Chat_Custom.Chat", "{0}");
            }
        }
    }
}