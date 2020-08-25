using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockChest.Interface
{
    public class DiscoverItem
    {
        public TypeEnum Category;
        //public ItemType type;
        public int ItemIndex;
        public string  ItemName;


        public DiscoverItem(int ItemIndex, TypeEnum Category, string ItemName)//, ItemType type
        {
            this.Category = Category;
            this.ItemIndex = ItemIndex;
            this.ItemName = ItemName;
            //this.type = type;
        }

        public DiscoverItem(int ItemIndex, Item item)//, ItemType type
        {
            this.Category = (TypeEnum)item.Category;
            this.ItemIndex = ItemIndex;
            this.ItemName = item.Name;
            //this.type = type;
        }

    }
}
