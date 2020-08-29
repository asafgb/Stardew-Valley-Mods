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
            GemCategory = -2,
            FishCategory = -4,
            EggCategory = -5,
            MilkCategory = -6,
            CookingCategory = -7,
            CraftingCategory = -8,
            BigCraftableCategory = -9,
            mineralsCategory = -12,
            meatCategory = -14,
            metalResources = -15,
            buildingResources = -16,
            sellAtPierres = -17,
            sellAtPierresAndMarnies = -18,
            fertilizerCategory = -19,
            junkCategory = -20,
            baitCategory = -21,
            tackleCategory = -22,
            ishShopCategory = -23,
            furnitureCategory = -24,
            ingredientsCategory = -25,
            artisanGoodsCategory = -26,
            syrupCategory = -27,
            monsterLootCategory = -28,
            equipmentCategory = -29,
            SeedsCategory = -74,
            VegetableCategory = -75,
            FruitsCategory = -79,
            flowersCategory = -80,
            GreensCategory = -81,
            hatCategory = -95,
            ringCategory = -96,
            weaponCategory = -98,
            toolCategory = -99,
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


        public enum Quality
        {
            Normal = 0,
            Silver = 1,
            Gold= 2,
            Iridium = 4
        }
    }
}
