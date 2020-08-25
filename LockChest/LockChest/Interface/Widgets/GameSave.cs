using Netcode;
using Newtonsoft.Json;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockChest.Interface.Widgets
{
    public class GameSave
    {
        public NetObjectList<Item> PlayerInventory;
        
        private string CharacterName;

        List<ChestIdentity> Chests;

        public GameSave()
        {
            PlayerInventory=Game1.player.items;
            CharacterName = Game1.player.name;
            Chests = new List<ChestIdentity>();
            SaveGame("test"); 
        }

        private void SaveGame(string gameName)
        {
            //string json = JsonConvert.SerializeObject(this,Formatting.Indented);
            File.WriteAllText(gameName,json);
        }

        public void SaveTemp()
        {
            this.SaveGame($"{CharacterName}_Temp");
        }

        public void SaveEndDay()
        {
            this.SaveGame($"{CharacterName}_EndDay");
        }


    }
}
