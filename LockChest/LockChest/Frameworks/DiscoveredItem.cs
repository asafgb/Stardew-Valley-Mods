using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockChest.Frameworks
{
    internal class DiscoveredItem
    {
        public readonly Item Item;
        public readonly ItemKey ItemKey;
        public DiscoveredItem(ItemType type, int index, Item item)
        {
            this.ItemKey = new ItemKey(type, index);
            this.Item = item;
        }
    }
}
