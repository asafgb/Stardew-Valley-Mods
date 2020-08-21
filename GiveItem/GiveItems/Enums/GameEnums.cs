using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Enums
{
    public class GameEnums
    {
        public enum Categories
        {
            None = 0,
            Mineral = -2,
            Fish = -4,
            Cooking = -7,
            Animal_Product = -5,
            Crafting = -8,
            Resource = -16,
            Fertilizer = -19,
            Trash = -20,
            Bait = -21,
            Artisan_Goods = -26,
            MonsterLoot = -28,
            Seed = -74,
            Vegetable = -75,
            Fruit = -79,
            Flower = -80,
            Forage = -81,
        }

        public enum TypeOfCategories
        {
            Basic,
            Minerals,
            Quest,
            asdf,
            Crafting,
            Arch,
            Fish,
            Cooking,
            Seeds,
            Ring,
        }
    }
}
