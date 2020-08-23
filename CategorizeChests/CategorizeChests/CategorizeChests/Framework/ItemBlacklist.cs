using System;
using System.Collections.Generic;

namespace StardewValleyMods.CategorizeChests.Framework
{
    internal static class ItemBlacklist
    {
        public static bool Includes(ItemKey itemKey)
        {
            return ItemBlacklist.BlacklistedItemKeys.Contains(itemKey);
        }

        private static readonly HashSet<ItemKey> BlacklistedItemKeys = new HashSet<ItemKey>
        {
            new ItemKey(ItemType.Object, 2),
            new ItemKey(ItemType.Object, 4),
            new ItemKey(ItemType.Object, 75),
            new ItemKey(ItemType.Object, 76),
            new ItemKey(ItemType.Object, 77),
            new ItemKey(ItemType.Object, 290),
            new ItemKey(ItemType.Object, 343),
            new ItemKey(ItemType.Object, 450),
            new ItemKey(ItemType.Object, 668),
            new ItemKey(ItemType.Object, 670),
            new ItemKey(ItemType.Object, 751),
            new ItemKey(ItemType.Object, 760),
            new ItemKey(ItemType.Object, 762),
            new ItemKey(ItemType.Object, 764),
            new ItemKey(ItemType.Object, 765),
            new ItemKey(ItemType.Object, 0),
            new ItemKey(ItemType.Object, 313),
            new ItemKey(ItemType.Object, 314),
            new ItemKey(ItemType.Object, 315),
            new ItemKey(ItemType.Object, 316),
            new ItemKey(ItemType.Object, 317),
            new ItemKey(ItemType.Object, 318),
            new ItemKey(ItemType.Object, 319),
            new ItemKey(ItemType.Object, 320),
            new ItemKey(ItemType.Object, 321),
            new ItemKey(ItemType.Object, 452),
            new ItemKey(ItemType.Object, 674),
            new ItemKey(ItemType.Object, 675),
            new ItemKey(ItemType.Object, 676),
            new ItemKey(ItemType.Object, 677),
            new ItemKey(ItemType.Object, 678),
            new ItemKey(ItemType.Object, 679),
            new ItemKey(ItemType.Object, 750),
            new ItemKey(ItemType.Object, 784),
            new ItemKey(ItemType.Object, 785),
            new ItemKey(ItemType.Object, 786),
            new ItemKey(ItemType.Object, 792),
            new ItemKey(ItemType.Object, 793),
            new ItemKey(ItemType.Object, 794),
            new ItemKey(ItemType.Object, 294),
            new ItemKey(ItemType.Object, 295),
            new ItemKey(ItemType.Object, 30),
            new ItemKey(ItemType.Object, 94),
            new ItemKey(ItemType.Object, 102),
            new ItemKey(ItemType.Object, 449),
            new ItemKey(ItemType.Object, 461),
            new ItemKey(ItemType.Object, 590),
            new ItemKey(ItemType.Object, 788),
            new ItemKey(ItemType.Object, 789),
            new ItemKey(ItemType.Object, 790),
            new ItemKey(ItemType.Weapon, 25),
            new ItemKey(ItemType.Weapon, 30),
            new ItemKey(ItemType.Weapon, 35),
            new ItemKey(ItemType.Weapon, 36),
            new ItemKey(ItemType.Weapon, 37),
            new ItemKey(ItemType.Weapon, 38),
            new ItemKey(ItemType.Weapon, 39),
            new ItemKey(ItemType.Weapon, 40),
            new ItemKey(ItemType.Weapon, 41),
            new ItemKey(ItemType.Weapon, 42),
            new ItemKey(ItemType.Weapon, 20),
            new ItemKey(ItemType.Weapon, 34),
            new ItemKey(ItemType.Weapon, 46),
            new ItemKey(ItemType.Weapon, 49),
            new ItemKey(ItemType.Weapon, 19),
            new ItemKey(ItemType.Weapon, 48),
            new ItemKey(ItemType.Boots, 515)
        };
    }
}
