using System;
using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;

namespace StardewValleyMods.CategorizeChests.Framework
{
    internal class ChestFiller : IChestFiller
    {
        public ChestFiller(IChestDataManager chestDataManager, IMonitor monitor)
        {
            this.ChestDataManager = chestDataManager;
            this.Monitor = monitor;
        }

        public void DumpItemsToChest(Chest chest)
        {
            ChestData chestData = this.ChestDataManager.GetChestData(chest);
            bool flag = false;
            foreach (Item item in this.GetInventoryItems())
            {
                try
                {
                    if (chestData.Accepts(item) && this.TryPutItemInChest(chest, item))
                    {
                        flag = true;
                    }
                }
                catch (ItemNotImplementedException)
                {
                }
            }
            if (flag)
            {
                Game1.playSound("dwop");
            }
        }

        private bool TryPutItemInChest(Chest chest, Item item)
        {
            bool result = false;
            foreach (Item item2 in from i in chest.items
                                   where i != null
                                   where i.canStackWith(item)
                                   select i)
            {
                int num = item2.maximumStackSize() - item2.Stack;
                if (num >= item.Stack)
                {
                    chest.grabItemFromInventory(item, Game1.player);
                    return true;
                }
                if (num > 0)
                {
                    item.Stack -= num;
                    item2.addToStack(item);
                    result = true;
                }
            }
            if (this.ChestHasEmptySpaces(chest))
            {
                chest.grabItemFromInventory(item, Game1.player);
                return true;
            }
            return result;
        }

        private bool ChestHasEmptySpaces(Chest chest)
        {
            if (chest.items.Count >= 36)
            {
                return chest.items.Any((Item i) => i == null);
            }
            return true;
        }

        private IEnumerable<Item> GetInventoryItems()
        {
            return (from i in Game1.player.Items
                    where i != null
                    select i).ToList<Item>();
        }

        private readonly IChestDataManager ChestDataManager;

        private readonly IMonitor Monitor;
    }
}
