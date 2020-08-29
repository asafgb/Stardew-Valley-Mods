using System;
using System.Linq;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.Objects;
using StardewValleyMods.CategorizeChests.Framework.Persistence;

namespace StardewValleyMods.CategorizeChests.Framework
{
		internal class ChestFinder : IChestFinder
	{
				public Chest GetChestByAddress(ChestAddress address)
		{
			if (address.LocationType == ChestLocationType.Refrigerator)
			{
				FarmHouse farmHouse = (FarmHouse)Game1.locations.First((GameLocation l) => l is FarmHouse);
				if (Game1.player.HouseUpgradeLevel < 1)
				{
					throw new InvalidSaveDataException("Chest save data contains refrigerator data but no refrigerator exists");
				}
				return farmHouse.fridge;
			}
			else
			{
				GameLocation locationFromAddress = this.GetLocationFromAddress(address);
				if (locationFromAddress.objects.ContainsKey(address.Tile) && locationFromAddress.objects[address.Tile] is Chest)
				{
					return locationFromAddress.objects[address.Tile] as Chest;
				}
				throw new InvalidSaveDataException(string.Format("Can't find chest in {0} at {1}", locationFromAddress.Name, address.Tile));
			}
		}

				private GameLocation GetLocationFromAddress(ChestAddress address)
		{
			GameLocation gameLocation = Game1.locations.FirstOrDefault((GameLocation l) => l.Name == address.LocationName);
			if (gameLocation == null)
			{
				throw new InvalidSaveDataException(string.Format("Can't find location named {0}", address.LocationName));
			}
			if (address.LocationType != ChestLocationType.Building)
			{
				return gameLocation;
			}
			BuildableGameLocation buildableGameLocation;
			if ((buildableGameLocation = (gameLocation as BuildableGameLocation)) == null)
			{
				throw new InvalidSaveDataException(string.Format("Can't find any buildings in location named {0}", gameLocation.Name));
			}
			Building building = buildableGameLocation.buildings.FirstOrDefault((Building b) => b.nameOfIndoors == address.BuildingName);
			if (building == null)
			{
				throw new InvalidSaveDataException(string.Format("Can't find building named {0} in location named {1}", address.BuildingName, gameLocation.Name));
			}
			return building.indoors;
		}
	}
}
