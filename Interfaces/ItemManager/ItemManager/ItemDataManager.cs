using ItemManager.interfaces;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemManager
{
    public class ItemDataManager : IItemDataManager
    {
        public static ItemDataManager Instance { get; private set; } = new ItemDataManager();

        private readonly int CustomIDOffset = 1000;

        private IDictionary<string, IEnumerable<ItemKey>> _Categories;

        private Dictionary<ItemKey, Item> PrototypeMap = new Dictionary<ItemKey, Item>();
        public IDictionary<string, IEnumerable<ItemKey>> Categories
        {
            get
            {
                return this._Categories;
            }
        }

        private ItemDataManager()
        {
            Dictionary<string, IEnumerable<ItemKey>> dictionary = new Dictionary<string, IEnumerable<ItemKey>>();
            foreach (DiscoveredItem discoveredItem in this.DiscoverItems())
            {
                //if (!ItemBlacklist.Includes(discoveredItem.ItemKey))
                //{
                this.PrototypeMap[discoveredItem.ItemKey] = discoveredItem.Item;
                string key = this.ChooseCategoryName(discoveredItem.ItemKey);
                if (!dictionary.ContainsKey(key))
                {
                    dictionary[key] = new List<ItemKey>();
                }
                    (dictionary[key] as List<ItemKey>).Add(discoveredItem.ItemKey);
                //   }
            }
            this._Categories = dictionary;
            //CheckNoForgotItems();
        }

        //private void CheckNoForgotItems()
        //{
        //    List<Item> notexist = new List<Item>() ;
        //    for (int i = 0; i < 3000; i++)
        //    {
        //        try
        //        {
        //            //IsExistInList(GetIfExist((index) => new StardewValley.Object(index, 0), i), notexist);
        //            //IsExistInList(GetIfExist((index) => new Boots(index), i), notexist);
        //            //IsExistInList(GetIfExist((index) => new Clothing(index), i), notexist);
        //            //IsExistInList(GetIfExist((index) => new Hat(index), i), notexist);
        //            //IsExistInList(GetIfExist((index) => new Ring(index), i), notexist);
        //            ////IsExistInList(GetIfExist((index) => new SpecialItem(index, "Asaf"), i)); // SKull key|magic inc|club card


        //            //IsExistInList(ObjectFactory.getItemFromDescription(ObjectFactory.regularObject, i, 0), notexist);
        //            //IsExistInList(ObjectFactory.getItemFromDescription(ObjectFactory.bigCraftable, i, 0), notexist);

        //        }
        //        catch (Exception)
        //        {
        //            Console.WriteLine("");
        //        }
        //    }
            
        //}

        /// <summary>
        /// Check if that Item exist in the List of items
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool IsExistInList(Item item, List<Item> notexist)
        {
            List<string> BlackList = new List<string> { "Error Item" };
            if (item == null || BlackList.Contains(item.DisplayName))
                return true;
            Item temp=getItemByName(item.Name);
            // Item no found
            if (temp == null)
            {
                notexist.Add(item);
                return false;
            }
            else
                // Found
                return true;
        }

        /// <summary>
        /// Put random number and check if that CTOR can restore Item
        /// </summary>
        /// <param name="action"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        private Item GetIfExist(Func<int,Item> action,int num)
        {
            try
            {
                Item temp = action(num);
                return temp;
            }
            catch (Exception)
            {
                // error Ctor that Num
                return null;
            }
        }

        public Item getItemByName(string Name)
        {
            var lstItems = this.PrototypeMap.Values.Where(item => item.Name == Name);
            return lstItems.Count() > 0  ? lstItems.First() : null;
        }

        public List<KeyValuePair<ItemKey, Item>> GetListKeyAndValue(string Name)
        {
            return this.PrototypeMap.Where(item => item.Value.Name == Name).ToList();
        }

        private string ChooseCategoryName(ItemKey itemKey)
        {
            if (itemKey.ItemType != ItemType.Object)
            {
                return Enum.GetName(typeof(ItemType), itemKey.ItemType);
            }
            string categoryName = this.GetItem(itemKey).getCategoryName();
            if (!string.IsNullOrEmpty(categoryName))
            {
                return categoryName;
            }
            return "Miscellaneous";
        }

        public ItemKey GetKey(Item item)
        {
            IEnumerable<KeyValuePair<ItemKey, Item>> source = from p in this.PrototypeMap
                                                              where ItemDataManager.MatchesPrototype(item, p.Value)
                                                              select p;
            if (!source.Any<KeyValuePair<ItemKey, Item>>())
            {
                DiscoveredItem discoveredItem = new DiscoveredItem(ItemType.Object, item.ParentSheetIndex, item);
                this.PrototypeMap[discoveredItem.ItemKey] = discoveredItem.Item;
                string key = this.ChooseCategoryName(discoveredItem.ItemKey);
                if (!this._Categories.ContainsKey(key))
                {
                    this._Categories[key] = new List<ItemKey>();
                }
                    (this._Categories[key] as List<ItemKey>).Add(discoveredItem.ItemKey);

                source = from p in this.PrototypeMap
                         where ItemDataManager.MatchesPrototype(item, p.Value)
                         select p;
            }
            return source.First<KeyValuePair<ItemKey, Item>>().Key;
        }

        public Item GetItem(ItemKey itemKey)
        {
            return this.PrototypeMap[itemKey];
        }

        public Item GetItem(ItemKey itemKey,int QualityOrUpgradeLevel,int stack)
        {
            Item temp = this.PrototypeMap[itemKey];
            if (temp as Tool != null) {
                ((Tool)temp).UpgradeLevel = QualityOrUpgradeLevel;
            }
            else
            {
                ((StardewValley.Object)temp).Quality = QualityOrUpgradeLevel;
            }
            temp.Stack = stack;
            return temp;
        }

        public bool HasItem(ItemKey itemKey)
        {
            return this.PrototypeMap.ContainsKey(itemKey);
        }

        private IEnumerable<DiscoveredItem> DiscoverItems()
        {
            yield return new DiscoveredItem(ItemType.Tool, 0, ToolFactory.getToolFromDescription(0, 0));
            yield return new DiscoveredItem(ItemType.Tool, 1, ToolFactory.getToolFromDescription(1, 0));
            yield return new DiscoveredItem(ItemType.Tool, 2, ToolFactory.getToolFromDescription(2, 0));
            yield return new DiscoveredItem(ItemType.Tool, 3, ToolFactory.getToolFromDescription(3, 0));
            yield return new DiscoveredItem(ItemType.Tool, 4, ToolFactory.getToolFromDescription(4, 0));
            yield return new DiscoveredItem(ItemType.Tool, 5, ToolFactory.getToolFromDescription(5, 0));
            yield return new DiscoveredItem(ItemType.Tool, 6, ToolFactory.getToolFromDescription(6, 0));

            yield return new DiscoveredItem(ItemType.Tool, this.CustomIDOffset, new MilkPail());
            yield return new DiscoveredItem(ItemType.Tool, this.CustomIDOffset + 1, new Shears());
            yield return new DiscoveredItem(ItemType.Tool, this.CustomIDOffset + 2, new Pan());
            yield return new DiscoveredItem(ItemType.Tool, this.CustomIDOffset + 3, new Wand());
           
            foreach (int num in Game1.content.Load<Dictionary<int, string>>("Data\\Boots").Keys)
            {
                yield return new DiscoveredItem(ItemType.Boots, num, new Boots(num));
            }
            foreach (int num2 in Game1.content.Load<Dictionary<int, string>>("Data\\hats").Keys)
            {
                yield return new DiscoveredItem(ItemType.Hat, num2, new Hat(num2));
            }
         

            foreach (int num3 in Game1.objectInformation.Keys)
            {
                if (num3 >= 516 && num3 <= 534)
                {
                    yield return new DiscoveredItem(ItemType.Ring, num3, new Ring(num3));
                }
            }
            foreach (int num4 in Game1.content.Load<Dictionary<int, string>>("Data\\weapons").Keys)
            {
                Item item = (num4 >= 32 && num4 <= 34) ? (Item)new Slingshot(num4) : (Item)new MeleeWeapon(num4);
                yield return new DiscoveredItem(ItemType.Weapon, num4, item);
            }
            foreach (int num5 in Game1.objectInformation.Keys)
            {
                if (num5 < 516 || num5 > 534)
                {
                    StardewValley.Object item2 = new StardewValley.Object(num5, 1, false, -1, 0);
                    yield return new DiscoveredItem(ItemType.Object, num5, item2);
                }
            }
            foreach (int num6 in Game1.content.Load<Dictionary<int, string>>("Data\\ClothingInformation").Keys)
            {
                yield return new DiscoveredItem(ItemType.Cloth, num6, new Clothing(num6));
            }
            foreach (int num7 in Game1.content.Load<Dictionary<int, string>>("Data\\BigCraftablesInformation").Keys)
            {
                yield return new DiscoveredItem(ItemType.BigCraftable, num7, ObjectFactory.getItemFromDescription(ObjectFactory.bigCraftable, num7, 1));
            }
            for (int num8 = 0; num8 < 56; num8++)
            {
                Wallpaper Floor = new Wallpaper(num8, true);
                Floor.sourceRect.Value = new Rectangle(num8 % 8 * 32, 336 + num8 / 8 * 32, 28, 26);
                yield return new DiscoveredItem(ItemType.Flooring, num8, Floor);
            }
            for (int num9 = 0; num9 < 112; num9++)
            {
                Wallpaper wallpaper = new Wallpaper(num9, false);
                wallpaper.sourceRect.Value =  new Rectangle(num9 % 16 * 16, num9 / 16 * 48 + 8, 16, 28);
                yield return new DiscoveredItem(ItemType.Wallpaper, num9, wallpaper);
            }



            yield break;
        }

        public static bool MatchesPrototype(Item item, Item prototype)
        {
            if ((item.GetType() == prototype.GetType() || (prototype.GetType() == typeof(StardewValley.Object) && item.GetType() == typeof(ColoredObject))) && item.category == prototype.category && item.parentSheetIndex == prototype.parentSheetIndex)
            {
                Boots boots = item as Boots;
                int? num = (boots != null) ? new int?(boots.indexInTileSheet) : null;
                Boots boots2 = prototype as Boots;
                if (num == ((boots2 != null) ? new int?(boots2.indexInTileSheet) : null))
                {
                    BreakableContainer breakableContainer = item as BreakableContainer;
                    string a = (breakableContainer != null) ? breakableContainer.type : null;
                    BreakableContainer breakableContainer2 = prototype as BreakableContainer;
                    if (a == ((breakableContainer2 != null) ? breakableContainer2.type : null))
                    {
                        Fence fence = item as Fence;
                        bool? flag = (fence != null) ? new bool?(fence.isGate) : null;
                        Fence fence2 = prototype as Fence;
                        if (flag == ((fence2 != null) ? new bool?(fence2.isGate) : null))
                        {
                            Fence fence3 = item as Fence;
                            int? num2 = (fence3 != null) ? new int?(fence3.whichType) : null;
                            Fence fence4 = prototype as Fence;
                            if (num2 == ((fence4 != null) ? new int?(fence4.whichType) : null))
                            {
                                Hat hat = item as Hat;
                                num = ((hat != null) ? new int?(hat.which) : null);
                                Hat hat2 = prototype as Hat;
                                if (num == ((hat2 != null) ? new int?(hat2.which) : null))
                                {
                                    Ring ring = item as Ring;
                                    num2 = ((ring != null) ? new int?(ring.indexInTileSheet) : null);
                                    Ring ring2 = prototype as Ring;
                                    if (num2 == ((ring2 != null) ? new int?(ring2.indexInTileSheet) : null))
                                    {
                                        MeleeWeapon meleeWeapon = item as MeleeWeapon;
                                        num = ((meleeWeapon != null) ? new int?(meleeWeapon.type) : null);
                                        MeleeWeapon meleeWeapon2 = prototype as MeleeWeapon;
                                        if (num == ((meleeWeapon2 != null) ? new int?(meleeWeapon2.type) : null))
                                        {
                                            MeleeWeapon meleeWeapon3 = item as MeleeWeapon;
                                            num2 = ((meleeWeapon3 != null) ? new int?(meleeWeapon3.initialParentTileIndex) : null);
                                            MeleeWeapon meleeWeapon4 = prototype as MeleeWeapon;
                                            return num2 == ((meleeWeapon4 != null) ? new int?(meleeWeapon4.initialParentTileIndex) : null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}