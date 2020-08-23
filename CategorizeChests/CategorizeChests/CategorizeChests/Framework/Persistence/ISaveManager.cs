using System;

namespace StardewValleyMods.CategorizeChests.Framework.Persistence
{
		internal interface ISaveManager
	{
				void Save(string path);

				void Load(string path);
	}
}
