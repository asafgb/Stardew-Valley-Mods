using System;
using StardewValley;

namespace StardewValleyMods.CategorizeChests.Framework
{
    internal class DiscoveredItem
    {
        public DiscoveredItem(ItemType type, int index, Item item)
        {
            this.ItemKey = new ItemKey(type, index);
            this.Item = item;
        }

        public readonly ItemKey ItemKey;

        public readonly Item Item;
    }
}
