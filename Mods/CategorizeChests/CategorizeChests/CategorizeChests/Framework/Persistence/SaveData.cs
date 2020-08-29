using System;
using System.Collections.Generic;

namespace StardewValleyMods.CategorizeChests.Framework.Persistence
{
		internal class SaveData
	{
				public string Version;

				public IEnumerable<ChestEntry> ChestEntries;
	}
}
