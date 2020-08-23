using System;
using System.Runtime.CompilerServices;
using StardewModdingAPI;
using StardewValley.Objects;

namespace StardewValleyMods.CategorizeChests.Framework
{
		internal class ChestDataManager : IChestDataManager
	{
				public ChestDataManager(IItemDataManager itemDataManager, IMonitor monitor)
		{
			this.ItemDataManager = itemDataManager;
			this.Monitor = monitor;
		}

				public ChestData GetChestData(Chest chest)
		{
			return this.Table.GetValue(chest, (Chest c) => new ChestData(c, this.ItemDataManager));
		}

				private readonly IItemDataManager ItemDataManager;

				private readonly IMonitor Monitor;

				private ConditionalWeakTable<Chest, ChestData> Table = new ConditionalWeakTable<Chest, ChestData>();
	}
}
