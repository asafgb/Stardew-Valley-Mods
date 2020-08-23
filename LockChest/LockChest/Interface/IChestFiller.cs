using System;
using StardewValley.Objects;


namespace LockChest.Interface
{
    public interface IChestFiller
    {
        void DumpItemsToChest(Chest chest);
    }
}
