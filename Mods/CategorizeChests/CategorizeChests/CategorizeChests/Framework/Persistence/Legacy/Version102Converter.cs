using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StardewValleyMods.CategorizeChests.Framework.Persistence.Legacy
{
		internal class Version102Converter
	{
				public JToken Convert(JToken data)
		{
			JArray entryList;
			if ((entryList = (data as JArray)) == null)
			{
				throw new InvalidSaveDataException("Expected version <1.1.0 save data to be an array");
			}
			JToken result;
			try
			{
				result = JObject.FromObject(new
				{
					Version = "1.0.2",
					ChestEntries = this.TranslateEntries(entryList)
				});
			}
			catch (Exception ex) when (ex is JsonException || ex is InvalidCastException)
			{
				throw new InvalidSaveDataException("Malformed save data structure", ex);
			}
			return result;
		}

				private JArray TranslateEntries(JArray entryList)
		{
			if (!entryList.Any<JToken>())
			{
				return entryList;
			}
			return JArray.FromObject(from child in entryList.Children()
			select this.TranslateEntry((JObject)child));
		}

				private JObject TranslateEntry(JObject entry)
		{
			return JObject.FromObject(new
			{
				Address = JObject.FromObject(new
				{
					LocationType = entry.SelectToken("LocationType").ToObject<ChestLocationType>(),
					LocationName = entry.Value<string>("LocationName"),
					BuildingName = entry.Value<string>("BuildingName"),
					Tile = entry.SelectToken("Tile").ToObject<Vector2>()
				}),
				AcceptedItemKinds = this.TranslateItemKeys((JArray)entry.SelectToken("AcceptedItemKinds"))
			});
		}

				private JArray TranslateItemKeys(JArray kindList)
		{
			if (!kindList.Any<JToken>())
			{
				return kindList;
			}
			return JArray.FromObject(from child in kindList.Children()
			select this.TranslateItemKey((JObject)child));
		}

				private JObject TranslateItemKey(JObject itemKey)
		{
			return JObject.FromObject(new
			{
				ItemType = "Object",
				ObjectIndex = itemKey.Value<string>("ObjectIndex")
			});
		}
	}
}
