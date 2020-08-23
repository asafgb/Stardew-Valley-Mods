using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.Objects;

namespace StardewValleyMods.CategorizeChests.Framework.Persistence
{
    internal class Saver
    {
        public Saver(ISemanticVersion version, IChestDataManager chestDataManager)
        {
            this.Version = version;
            this.ChestDataManager = chestDataManager;
        }

        public string DumpData()
        {
            return JsonConvert.SerializeObject(this.GetSerializableData(), new JsonConverter[]
            {
                new StringEnumConverter()
            });
        }

        private SaveData GetSerializableData()
        {
            return new SaveData
            {
                Version = this.Version.ToString(),
                ChestEntries = this.BuildChestEntries()
            };
        }

        private IEnumerable<ChestEntry> BuildChestEntries()
        {
            foreach (GameLocation location in Game1.locations)
            {
                foreach (KeyValuePair<Vector2, Chest> keyValuePair in this.GetLocationChests(location))
                {
                    yield return new ChestEntry
                    {
                        Address = new ChestAddress
                        {
                            LocationType = ChestLocationType.Normal,
                            LocationName = location.Name,
                            Tile = keyValuePair.Key
                        },
                        AcceptedItemKinds = this.ChestDataManager.GetChestData(keyValuePair.Value).AcceptedItemKinds
                    };
                }
                //IEnumerator<KeyValuePair<Vector2, Chest>> enumerator2 = null;
                BuildableGameLocation buildableGameLocation;
                if ((buildableGameLocation = (location as BuildableGameLocation)) != null)
                {
                    IEnumerable<Building> enumerable = from b in buildableGameLocation.buildings
                                                       where b.indoors != null
                                                       select b;
                    foreach (Building building in enumerable)
                    {
                        foreach (KeyValuePair<Vector2, Chest> keyValuePair2 in this.GetLocationChests(building.indoors))
                        {
                            yield return new ChestEntry
                            {
                                Address = new ChestAddress
                                {
                                    LocationType = ChestLocationType.Building,
                                    LocationName = location.Name,
                                    BuildingName = building.nameOfIndoors,
                                    Tile = keyValuePair2.Key
                                },
                                AcceptedItemKinds = this.ChestDataManager.GetChestData(keyValuePair2.Value).AcceptedItemKinds
                            };
                        }
                        //enumerator2 = null;
                        //building = null;
                    }
                    //IEnumerator<Building> enumerator3 = null;
                }
                FarmHouse farmHouse;
                if ((farmHouse = (location as FarmHouse)) != null && Game1.player.HouseUpgradeLevel >= 1)
                {
                    Chest fridge = farmHouse.fridge;
                    yield return new ChestEntry
                    {
                        Address = new ChestAddress
                        {
                            LocationType = ChestLocationType.Refrigerator
                        },
                        AcceptedItemKinds = this.ChestDataManager.GetChestData(fridge).AcceptedItemKinds
                    };
                }
                //location = null;
            }
            List<GameLocation>.Enumerator enumerator = default(List<GameLocation>.Enumerator);
            yield break;
            yield break;
        }

        //private IDictionary<Vector2, Chest> GetLocationChests(GameLocation location)
        //{
        //    return (from pair in location.Objects
        //            where pair.Value is Chest && ((Chest)pair.Value).playerChest
        //            select pair).ToDictionary((KeyValuePair<Vector2, StardewValley.Object> pair) => pair.Key, (KeyValuePair<Vector2, StardewValley.Object> pair) => (Chest)pair.Value);
        //}

        private IDictionary<Vector2, Chest> GetLocationChests(GameLocation location)
        {
            Dictionary<Vector2, Chest> chests = new Dictionary<Vector2, Chest>();
            foreach (var directories in location.Objects)
            {
                foreach (var item in directories)
                {
                    if ((item.Value as Chest) != null && ((Chest)item.Value).playerChest.Value)
                    {
                        chests.Add(item.Key, (Chest)item.Value);
                    }
                }
            }
            return chests;
        }

        private readonly ISemanticVersion Version;

        private readonly IChestDataManager ChestDataManager;
    }
}
