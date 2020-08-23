using System;
using System.Runtime.CompilerServices;
using LockChest.Interface;
using StardewModdingAPI;
using StardewValley.Objects;


namespace LockChest.Frameworks
{
    internal class ChestDataManager : IChestDataManager
    {
        private readonly IMonitor Monitor;
        private ConditionalWeakTable<Chest, ChestData> Table = new ConditionalWeakTable<Chest, ChestData>();



        public ChestDataManager( IMonitor monitor)
        {
            this.Monitor = monitor;
        }

        public ChestData GetChestData(Chest chest)
        {
            return null;
            //return this.Table.GetValue(chest, (Chest c) => new ChestData(c, this.ItemDataManager));
        }



    }
}
