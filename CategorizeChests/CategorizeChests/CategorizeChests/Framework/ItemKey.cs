using System;
using Newtonsoft.Json;

namespace StardewValleyMods.CategorizeChests.Framework
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class ItemKey
    {
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

        [JsonProperty]
        public readonly ItemType ItemType;

        [JsonProperty]
        public readonly int ObjectIndex;
    }
}
