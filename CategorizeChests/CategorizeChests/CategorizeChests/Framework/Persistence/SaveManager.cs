using System;
using System.IO;
using Newtonsoft.Json.Linq;
using StardewModdingAPI;
using StardewValleyMods.CategorizeChests.Framework.Persistence.Legacy;

namespace StardewValleyMods.CategorizeChests.Framework.Persistence
{
		internal class SaveManager : ISaveManager
	{
				public SaveManager(ISemanticVersion version, IChestDataManager chestDataManager, IChestFinder chestFinder, IItemDataManager itemDataManager)
		{
			this.Version = version;
			this.ChestDataManager = chestDataManager;
			this.ChestFinder = chestFinder;
			this.ItemDataManager = itemDataManager;
		}

				public void Save(string path)
		{
			string contents = new Saver(this.Version, this.ChestDataManager).DumpData();
			File.WriteAllText(path, contents);
		}

				public void Load(string path)
		{
			JToken jtoken = JToken.Parse(File.ReadAllText(path));
			jtoken = this.ConvertVersion(jtoken);
			new Loader(this.ChestDataManager, this.ChestFinder, this.ItemDataManager).LoadData(jtoken);
		}

				private JToken ConvertVersion(JToken data)
		{
			if (this.ReadVersionNumber(data).IsOlderThan("1.1.0"))
			{
				data = new Version102Converter().Convert(data);
			}
			return data;
		}

				private ISemanticVersion ReadVersionNumber(JToken token)
		{
			JObject jobject;
			if ((jobject = (token as JObject)) != null)
			{
				return new SemanticVersion(jobject.Value<string>("Version"));
			}
			if (token is JArray)
			{
				return new SemanticVersion("1.0.2");
			}
			throw new InvalidSaveDataException("Cannot detect save data version");
		}

				private readonly ISemanticVersion Version;

				private readonly IChestDataManager ChestDataManager;

				private readonly IChestFinder ChestFinder;

				private readonly IItemDataManager ItemDataManager;
	}
}
