using LockChest.Frameworks;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockChest.Interface
{
    internal interface IItemDataManager
    {
        IDictionary<string, IEnumerable<ItemKey>> Categories { get; }
        ItemKey GetKey(Item item);
        Item GetItem(ItemKey itemKey);
        bool HasItem(ItemKey itemKey);
    }
}
