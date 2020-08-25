using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Tools;

namespace LockChest.Interface
{
    public class CategoryToCreate
    {
        Dictionary<TypeEnum, Func<int, Item>> catetoItem;
        public CategoryToCreate Instance { get; } = new CategoryToCreate();
        public CategoryToCreate()
        {
            catetoItem.Add(TypeEnum.artisanGoodsCategory, (index) => new StardewValley.Object(index,1) ); // wine
            catetoItem.Add(TypeEnum.baitCategory, (index) => new StardewValley.Object(index, 1)); // bait
            catetoItem.Add(TypeEnum.BigCraftableCategory, (index) => new StardewValley.Object(index, 1)); // chest
            catetoItem.Add(TypeEnum.buildingResources, (index) => new StardewValley.Object(index, 1)); // wood
            catetoItem.Add(TypeEnum.CookingCategory, (index) => new StardewValley.Object(index, 1)); // wood
            catetoItem.Add(TypeEnum.EggCategory, (index) => new StardewValley.Object(index, 1)); // egg
            //catetoItem.Add(TypeEnum.equipmentCategory, (index) => ObjectFactory.getItemFromDescription((byte)noneUsable, index,1));
            catetoItem.Add(TypeEnum.fertilizerCategory, (index) => new StardewValley.Object(index, 1)); // fertilizer
            catetoItem.Add(TypeEnum.FishCategory, (index) => new StardewValley.Object(index, 1));// fishes
            catetoItem.Add(TypeEnum.flowersCategory, (index) => new StardewValley.Object(index, 1)); // flower
            catetoItem.Add(TypeEnum.FruitsCategory, (index) => new StardewValley.Object(index, 1)); // fruit
            catetoItem.Add(TypeEnum.furnitureCategory, (index) => new StardewValley.Object(index, 1)); //decor  X must check
            catetoItem.Add(TypeEnum.GemCategory, (index) => new StardewValley.Object(index, 1)); // gems
            catetoItem.Add(TypeEnum.GreensCategory, (index) => new StardewValley.Object(index, 1)); // Forage X must check
            catetoItem.Add(TypeEnum.hatCategory, (index) => new StardewValley.Objects.Hat(index));
            catetoItem.Add(TypeEnum.ingredientsCategory, (index) => new StardewValley.Object(index, 1)); // X must check
            catetoItem.Add(TypeEnum.junkCategory, (index) => new StardewValley.Object(index, 1));// Cola
            catetoItem.Add(TypeEnum.meatCategory, (index) => new StardewValley.Object(index, 1));
            catetoItem.Add(TypeEnum.metalResources, (index) => new StardewValley.Object(index, 1));
            catetoItem.Add(TypeEnum.MilkCategory, (index) => new StardewValley.Object(index, 1));
            catetoItem.Add(TypeEnum.mineralsCategory, (index) => new StardewValley.Object(index, 1));
            catetoItem.Add(TypeEnum.monsterLootCategory, (index) => new StardewValley.Object(index, 1));
            catetoItem.Add(TypeEnum.ringCategory, (index) => new StardewValley.Objects.Ring(index));
            catetoItem.Add(TypeEnum.SeedsCategory, (index) => new StardewValley.Object(index, 1)); // new StardewValley.Tools.Seeds()
            catetoItem.Add(TypeEnum.syrupCategory, (index) => new StardewValley.Object(index, 1));
            catetoItem.Add(TypeEnum.tackleCategory, (index) => new StardewValley.Object(index, 1));
            catetoItem.Add(TypeEnum.CraftingCategory, (index) => new StardewValley.Object(index, 1));
            //catetoItem.Add(TypeEnum.toolCategory, (index) => ToolFactory.getToolFromDescription((byte)index,0));
            catetoItem.Add(TypeEnum.VegetableCategory, (index) => new StardewValley.Object(index, 1));
            catetoItem.Add(TypeEnum.weaponCategory, (index) => ToolFactory.getToolFromDescription((byte)index, 0));

        }
    }
}
