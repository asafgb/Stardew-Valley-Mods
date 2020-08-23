using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockChest.Frameworks
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class ItemKey
    {
        [JsonProperty]
        public readonly ItemType ItemType;

        [JsonProperty]
        public readonly int ObjectIndex;

        [JsonConstructor]
        public ItemKey(ItemType itemType, int parentSheetIndex)
        {
            this.ItemType = itemType;
            this.ObjectIndex = parentSheetIndex;
        }

        public override int GetHashCode()
        {
            return (int)this.ItemType * 10000 + this.ObjectIndex;
        }

        public override bool Equals(object obj)
        {
            ItemKey itemKey;
            return (itemKey = (obj as ItemKey)) != null && itemKey.ItemType == this.ItemType && itemKey.ObjectIndex == this.ObjectIndex;
        }


    }
}

