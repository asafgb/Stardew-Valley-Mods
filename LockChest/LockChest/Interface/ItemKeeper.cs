using ItemManager.interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockChest.Interface
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class ItemKeeper
    {
        [JsonProperty]
        public ItemKey itemKey;
        [JsonProperty]
        public int QualityOrUpgradeLevel;
        [JsonProperty]
        public int Stack;

        public ItemKeeper(ItemKey itemKey, int QualityOrUpgradeLevel,int Stack =1)
        {
            this.itemKey = itemKey;
            this.QualityOrUpgradeLevel = QualityOrUpgradeLevel;
            this.Stack = Stack;

            
        }
         
    }
}
