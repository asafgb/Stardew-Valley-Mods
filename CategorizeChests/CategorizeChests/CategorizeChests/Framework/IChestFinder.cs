using System;
using StardewValley.Objects;
using StardewValleyMods.CategorizeChests.Framework.Persistence;

namespace StardewValleyMods.CategorizeChests.Framework
{
		internal interface IChestFinder
	{
				Chest GetChestByAddress(ChestAddress address);
	}
}
