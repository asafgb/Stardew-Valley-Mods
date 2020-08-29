using System;
using StardewValley;

namespace StardewValleyMods.CategorizeChests.Framework
{
    internal class ItemNotImplementedException : Exception
    {
        public ItemNotImplementedException(Item item) : base(string.Format("Chest categorization for item named {0} is not implemented", item.Name))
        {
        }
    }
}
