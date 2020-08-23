using LockChest.Interface;
using StardewValley;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockChest.Frameworks
{
    internal class ChestData
    {
        public IEnumerable<ItemKey> AcceptedItemKinds
        {
            get
            {
                return this.ItemsEnabled;
            }
            set
            {
                this.ItemsEnabled.Clear();
                foreach (ItemKey item in value)
                {
                    this.ItemsEnabled.Add(item);
                }
            }
        }

        public ChestData(Chest chest, IItemDataManager itemDataManager)
        {
            this.Chest = chest;
            this.ItemDataManager = itemDataManager;
        }

        public void Accept(ItemKey itemKey)
        {
            if (!this.ItemsEnabled.Contains(itemKey))
            {
                this.ItemsEnabled.Add(itemKey);
            }
        }

        public void Reject(ItemKey itemKey)
        {
            if (this.ItemsEnabled.Contains(itemKey))
            {
                this.ItemsEnabled.Remove(itemKey);
            }
        }

        public void Toggle(ItemKey itemKey)
        {
            if (this.Accepts(itemKey))
            {
                this.Reject(itemKey);
                return;
            }
            this.Accept(itemKey);
        }

        public bool Accepts(Item item)
        {
            return this.Accepts(this.ItemDataManager.GetKey(item));
        }

        public bool Accepts(ItemKey itemKey)
        {
            return this.ItemsEnabled.Contains(itemKey);
        }

        private readonly IItemDataManager ItemDataManager;

        private readonly Chest Chest;

        private HashSet<ItemKey> ItemsEnabled = new HashSet<ItemKey>();
    }
}
