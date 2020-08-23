using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using StardewValley.Objects;

namespace StardewValleyMods.CategorizeChests.Framework.Persistence
{
		internal class Loader
	{
				public Loader(IChestDataManager chestDataManager, IChestFinder chestFinder, IItemDataManager itemDataManager)
		{
			this.ChestDataManager = chestDataManager;
			this.ChestFinder = chestFinder;
			this.ItemDataManager = itemDataManager;
		}

				public void LoadData(JToken token)
		{
			foreach (ChestEntry chestEntry in token.ToObject<SaveData>(new JsonSerializer
			{
				Converters = 
				{
					new StringEnumConverter()
				}
			}).ChestEntries)
			{
				Chest chestByAddress = this.ChestFinder.GetChestByAddress(chestEntry.Address);
				this.ChestDataManager.GetChestData(chestByAddress).AcceptedItemKinds = from itemKey in chestEntry.AcceptedItemKinds
				where this.ItemDataManager.HasItem(itemKey)
				select itemKey;
			}
		}

				private readonly IChestDataManager ChestDataManager;

				private readonly IChestFinder ChestFinder;

				private readonly IItemDataManager ItemDataManager;
	}
}
