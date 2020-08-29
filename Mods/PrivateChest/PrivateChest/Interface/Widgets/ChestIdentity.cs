using ItemManager;
using ItemManager.interfaces;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StardewValley;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockChest.Interface.Widgets
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ChestIdentity
    {
        [JsonProperty]
        private List<ItemKeeper> Inventory;


        [JsonConstructor]
        public ChestIdentity()
        {
            this.Inventory = new List<ItemKeeper>();
        }

        [JsonIgnore]
        public Chest GetChest {
            get
            {
                List<Item> lsttemp=Inventory.Select(itemkeeper => ItemDataManager.Instance.GetItem(itemkeeper.itemKey, itemkeeper.QualityOrUpgradeLevel, itemkeeper.Stack)).ToList();
                Chest cst = new Chest(0, lsttemp, new Vector2(0, 0), false,0);
                return cst;
            }
        }
        public void SetInventory(List<ItemKeeper> Inventory)
        {
            this.Inventory = Inventory;
        }
    }
}
