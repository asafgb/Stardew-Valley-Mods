using ItemManager.interfaces;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StardewValley;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockChest.Interface.Widgets
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ChestIdentity
    {
        [JsonProperty]
        private Point TileLocation;

        [JsonProperty]
        private string Room;

        [JsonProperty]
        private List<ItemKeeper> Inventory;

        public ChestIdentity(Chest chest, GameLocation gameLocation) : this(new Point((int)chest.TileLocation.X, (int)chest.TileLocation.Y), gameLocation.ToString())
        {
        }

        [JsonConstructor]
        public ChestIdentity(Point point, string Room)
        {
            this.TileLocation = point;
            this.Room = Room;
            this.Inventory = new List<ItemKeeper>();
        }

        public static bool operator == (ChestIdentity b, ChestIdentity c)
        {
            return b.Room == c.Room && b.TileLocation == c.TileLocation;
        }
        public static bool operator !=(ChestIdentity b, ChestIdentity c)
        {
            return !(b==c);
        }

        public static bool operator ==(ChestIdentity b, Chest c)
        {
            return  b.TileLocation == new Point ((int)c.TileLocation.X, (int)c.TileLocation.Y) && b.Room == Game1.player.currentLocation.ToString();
        }
        public static bool operator !=(ChestIdentity b, Chest c)
        {
            return !(b == c);
        }
    }
}
