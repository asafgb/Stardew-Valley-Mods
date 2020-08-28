using ItemManager;
using ItemManager.interfaces;
using LockChest.Interface;
using LockChest.Interface.Widgets;
using Newtonsoft.Json;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockChest.Frameworks
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AllPrivateChests
    {
        [JsonProperty]
        private List<ChestIdentity> lstChest;
        [JsonProperty]
        private List<ItemKeeper> playerInventory;
        [JsonProperty]
        private GameTimeHandler timeHandler;

        [JsonProperty]
        private int currChestIndex;

        [JsonProperty]
        private string CharacterName;

        [JsonIgnore]
        IModHelper helper;




        private readonly string TempSave;
        private readonly string AllDay;

        public AllPrivateChests(IModHelper helper, int AmountOfChest)
        {
            // if run temp
            if (helper != null)
            {
                this.helper = helper;
                this.currChestIndex = 0;
                lstChest = new List<ChestIdentity>();
                for (int i = 0; i < AmountOfChest; i++)
                {
                    lstChest.Add(new ChestIdentity());
                }
                CharacterName = Game1.player.Name;
                TempSave = Path.Combine(helper.DirectoryPath, $"{CharacterName}_Temp.json");
                AllDay = Path.Combine(helper.DirectoryPath, $"{CharacterName}_EndDay.json");
                timeHandler = new GameTimeHandler(Game1.year, Game1.currentSeason, Game1.dayOfMonth, Game1.timeOfDay);
                LoadSave();

            }
        }


        //public List<ChestIdentity> GetChests { get { return this.lstChest; } }

        public ChestIdentity getCurrentChestId { get { return this.lstChest[this.currChestIndex]; } }

        public Chest GetCurrentChest { get { return this.lstChest[this.currChestIndex].GetChest; } }

        public int CurrChestIndex { get { return this.currChestIndex; } }

        private void LoadSave()
        {
            if (File.Exists(TempSave) && IsValidLoad())
                LoadFile(TempSave);
            else if (File.Exists(AllDay))
                LoadFile(AllDay);
        }

        private bool IsValidLoad()
        {
            bool Isvalid = true;
            string json = File.ReadAllText(TempSave);
            AllPrivateChests temp = JsonConvert.DeserializeObject<AllPrivateChests>(json);

            // if the player crush and not the host 
            // so time In game will be later from the temp file
            // as well the inventory, it should be the same as before crush
            // if the host is crush so the Game shouldn't load
            Isvalid = Isvalid && this.timeHandler.isThisTimeIsLater(temp.timeHandler) && IsvalidInventory(temp.playerInventory);

            return Isvalid;
        }

        private bool IsvalidInventory(List<ItemKeeper> playerInventory)
        {
            if (playerInventory.Count != Game1.player.Items.Count)
                return false;

            bool Isvalid = true;


            for (int i = 0; i < Game1.player.Items.Count; i++)
            {
                Item item = Game1.player.Items[i];
                if (item != null)
                {
                    Item fromList = ItemDataManager.Instance.GetItem(playerInventory[i].itemKey);

                    Isvalid = Isvalid &&
                               item.ParentSheetIndex == fromList.ParentSheetIndex &&
                               item.GetType() == fromList.GetType() &&
                               item.Stack == playerInventory[i].Stack;
                }
                else
                    Isvalid = Isvalid && playerInventory[i] == null;
            }

            return Isvalid;
        }

        public void AddIndex(int offset)
        {
            this.currChestIndex = Mod(this.currChestIndex + offset, this.lstChest.Count);
        }
        private int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }


        private void SaveChest(string gameName)
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(gameName, json);
        }

        // Save any change in any Change inventory
        public void SaveTemp()
        {
            this.playerInventory = Game1.player.items.Select(item => item == null ? null : item as Tool != null ?
                                  new ItemKeeper(ItemDataManager.Instance.GetKey(item), ((Tool)item).UpgradeLevel) :
                                  new ItemKeeper(ItemDataManager.Instance.GetKey(item), ((StardewValley.Object)item).Quality, item.Stack)).ToList();
            this.timeHandler.TimeOfDay = Game1.timeOfDay;
            this.SaveChest(TempSave);
        }

        // Save at end day
        public void SaveEndDay()
        {
            this.SaveChest(AllDay);
            this.timeHandler.Date.AddDays(1);
            this.timeHandler.TimeOfDay = 600;
            if (File.Exists(TempSave))
                File.Delete(TempSave);
        }


        private void LoadFile(string gameName)
        {
            //string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            string json = File.ReadAllText(gameName);
            AllPrivateChests temp = JsonConvert.DeserializeObject<AllPrivateChests>(json);
            this.lstChest = temp.lstChest;
            this.playerInventory = temp.playerInventory;
        }



    }
}
