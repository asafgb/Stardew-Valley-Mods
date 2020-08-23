using System;
using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Tools;

namespace StardewValleyMods.CategorizeChests.Framework
{
    internal class ItemDataManager : IItemDataManager
    {
        public IDictionary<string, IEnumerable<ItemKey>> Categories
        {
            get
            {
                return this._Categories;
            }
        }

        public ItemDataManager(IMonitor monitor)
        {
            this.Monitor = monitor;
            Dictionary<string, IEnumerable<ItemKey>> dictionary = new Dictionary<string, IEnumerable<ItemKey>>();
            foreach (DiscoveredItem discoveredItem in this.DiscoverItems())
            {
                if (!ItemBlacklist.Includes(discoveredItem.ItemKey))
                {
                    this.PrototypeMap[discoveredItem.ItemKey] = discoveredItem.Item;
                    string key = this.ChooseCategoryName(discoveredItem.ItemKey);
                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary[key] = new List<ItemKey>();
                    }
                    (dictionary[key] as List<ItemKey>).Add(discoveredItem.ItemKey);
                }
            }
            this._Categories = dictionary;
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
                throw new ItemNotImplementedException(item);
            }
            return source.First<KeyValuePair<ItemKey, Item>>().Key;
        }

        public Item GetItem(ItemKey itemKey)
        {
            return this.PrototypeMap[itemKey];
        }

        public bool HasItem(ItemKey itemKey)
        {
            return this.PrototypeMap.ContainsKey(itemKey);
        }

        private IEnumerable<DiscoveredItem> DiscoverItems()
        {
            yield return new DiscoveredItem(ItemType.Tool, 0, ToolFactory.getToolFromDescription(0, 0));
            yield return new DiscoveredItem(ItemType.Tool, 1, ToolFactory.getToolFromDescription(1, 0));
            yield return new DiscoveredItem(ItemType.Tool, 3, ToolFactory.getToolFromDescription(3, 0));
            yield return new DiscoveredItem(ItemType.Tool, 4, ToolFactory.getToolFromDescription(4, 0));
            yield return new DiscoveredItem(ItemType.Tool, 2, ToolFactory.getToolFromDescription(2, 0));
            yield return new DiscoveredItem(ItemType.Tool, this.CustomIDOffset, new MilkPail());
            yield return new DiscoveredItem(ItemType.Tool, this.CustomIDOffset + 1, new Shears());
            yield return new DiscoveredItem(ItemType.Tool, this.CustomIDOffset + 2, new Pan());
            foreach (int num in Game1.content.Load<Dictionary<int, string>>("Data\\Boots").Keys)
            {
                yield return new DiscoveredItem(ItemType.Boots, num, new Boots(num));
            }
            Dictionary<int, string>.KeyCollection.Enumerator enumerator = default(Dictionary<int, string>.KeyCollection.Enumerator);
            foreach (int num2 in Game1.content.Load<Dictionary<int, string>>("Data\\hats").Keys)
            {
                yield return new DiscoveredItem(ItemType.Hat, num2, new Hat(num2));
            }
            enumerator = default(Dictionary<int, string>.KeyCollection.Enumerator);
            foreach (int num3 in Game1.objectInformation.Keys)
            {
                if (num3 >= 516 && num3 <= 534)
                {
                    yield return new DiscoveredItem(ItemType.Ring, num3, new Ring(num3));
                }
            }
            enumerator = default(Dictionary<int, string>.KeyCollection.Enumerator);
            foreach (int num4 in Game1.content.Load<Dictionary<int, string>>("Data\\weapons").Keys)
            {
                Item item = (num4 >= 32 && num4 <= 34) ? (Item)new Slingshot(num4) : (Item)new MeleeWeapon(num4);
                yield return new DiscoveredItem(ItemType.Weapon, num4, item);
            }
            enumerator = default(Dictionary<int, string>.KeyCollection.Enumerator);
            foreach (int num5 in Game1.objectInformation.Keys)
            {
                if (num5 < 516 || num5 > 534)
                {
                    StardewValley.Object item2 = new StardewValley.Object(num5, 1, false, -1, 0);
                    yield return new DiscoveredItem(ItemType.Object, num5, item2);
                }
            }
            enumerator = default(Dictionary<int, string>.KeyCollection.Enumerator);
            yield break;
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

        private readonly int CustomIDOffset = 1000;

        private IMonitor Monitor;

        private IDictionary<string, IEnumerable<ItemKey>> _Categories;

        private Dictionary<ItemKey, Item> PrototypeMap = new Dictionary<ItemKey, Item>();
    }
}
