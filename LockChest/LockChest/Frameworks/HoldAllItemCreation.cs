using LockChest.Interface;
using StardewValley;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockChest.Frameworks
{
    public class HoldAllItemCreation
    {
        public static HoldAllItemCreation Instance {get;set;} = new HoldAllItemCreation();
        public Dictionary<ItemType, DiscoverItem> dirItemType;
        private readonly int CustomIDOffset = 1000;



        public HoldAllItemCreation()
        {
            dirItemType = new Dictionary<ItemType, DiscoverItem>();
            
        }


  
}
}
