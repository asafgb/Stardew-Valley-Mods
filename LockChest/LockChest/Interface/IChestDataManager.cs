using System;
using LockChest.Frameworks;
using StardewValley.Objects;

namespace LockChest.Interface
{
    internal interface IChestDataManager
    {
        ChestData GetChestData(Chest chest);
    }
}
