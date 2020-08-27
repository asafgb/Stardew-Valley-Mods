using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemManager.interfaces
{
    public class DiscoveredItem
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
