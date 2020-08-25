using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockChest.Interface
{

    public class SpcecificItemToCreate
    {
        Dictionary<TypeEnum, Func<int, Item>> catetoItem;
        public SpcecificItemToCreate Instance { get; } = new SpcecificItemToCreate();

        public SpcecificItemToCreate()
        {
            DiscoverItems();
            catetoItem = new Dictionary<TypeEnum, Func<int, Item>>();
        }

        private void DiscoverItems()
        {
            dirItemType.Add(ItemType.Tool, new DiscoverItem(0, ToolFactory.getToolFromDescription(0, 0)));
            dirItemType.Add(ItemType.Tool, new DiscoverItem(1, ToolFactory.getToolFromDescription(1, 0)));
            dirItemType.Add(ItemType.Tool, new DiscoverItem(2, ToolFactory.getToolFromDescription(2, 0)));
            dirItemType.Add(ItemType.Tool, new DiscoverItem(3, ToolFactory.getToolFromDescription(3, 0)));
            dirItemType.Add(ItemType.Tool, new DiscoverItem(4, ToolFactory.getToolFromDescription(4, 0)));

            dirItemType.Add(ItemType.Tool, new DiscoverItem(this.CustomIDOffset, new MilkPail()));
            dirItemType.Add(ItemType.Tool, new DiscoverItem(4, ToolFactory.getToolFromDescription(4, 0)));
            dirItemType.Add(ItemType.Tool, new DiscoverItem(4, ToolFactory.getToolFromDescription(4, 0)));
            dirItemType.Add(ItemType.Tool, new DiscoverItem(4, ToolFactory.getToolFromDescription(4, 0)));
            dirItemType.Add(ItemType.Tool, new DiscoverItem(4, ToolFactory.getToolFromDescription(4, 0)));
            dirItemType.Add(ItemType.Tool, new DiscoverItem(4, ToolFactory.getToolFromDescription(4, 0)));
            dirItemType.Add(ItemType.Tool, new DiscoverItem(4, ToolFactory.getToolFromDescription(4, 0)));
            dirItemType.Add(ItemType.Tool, new DiscoverItem(4, ToolFactory.getToolFromDescription(4, 0)));
            dirItemType.Add(ItemType.Tool, new DiscoverItem(4, ToolFactory.getToolFromDescription(4, 0)));


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
        }

    }
}
