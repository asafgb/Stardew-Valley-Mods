using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryFavorite
{
    public class ObjectPatches
    {
        private static IMonitor Monitor;

        // call this method from your Entry class
        public static void Initialize(IMonitor monitor)
        {
            Monitor = monitor;
        }

        public static bool CanBePlacedHere_Prefix(SObject __instance, GameLocation location, Vector2 tile, ref bool __result)
        {
            try
            {
            ...; // your patch logic here
                return false; // don't run original logic
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(CanBePlacedHere_Prefix)}:\n{ex}", LogLevel.Error);
                return true; // run original logic
            }
        }
    }
}
