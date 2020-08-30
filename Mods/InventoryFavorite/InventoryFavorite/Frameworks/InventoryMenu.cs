using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryFavorite.Frameworks
{
    internal class InventoryMenu : ItemGrabMenu
    {
        public InventoryMenu(IList<Item> inventory, object context = null) : base(inventory, context)
        {
        }
    }
}
