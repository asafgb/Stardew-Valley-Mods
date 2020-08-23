﻿using System;
using System.Collections.Generic;
using StardewValley;

namespace StardewValleyMods.CategorizeChests.Framework
{
    internal interface IItemDataManager
    {
        IDictionary<string, IEnumerable<ItemKey>> Categories { get; }

        ItemKey GetKey(Item item);

        Item GetItem(ItemKey itemKey);

        bool HasItem(ItemKey itemKey);
    }
}
