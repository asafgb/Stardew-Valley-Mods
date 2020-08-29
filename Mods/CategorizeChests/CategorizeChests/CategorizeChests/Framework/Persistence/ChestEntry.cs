using ItemManager.interfaces;
using System;
using System.Collections.Generic;

namespace StardewValleyMods.CategorizeChests.Framework.Persistence
{
		internal class ChestEntry
	{
				public ChestAddress Address;

				public IEnumerable<ItemKey> AcceptedItemKinds;
	}
}
