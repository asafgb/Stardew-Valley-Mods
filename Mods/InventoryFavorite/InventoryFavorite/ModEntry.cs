using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryFavorite
{
    public class ModEntry : Mod
    {
        public ModEntry()
        {
        }

        public override void Entry(IModHelper helper)
        {
            var harmony = HarmonyInstance.Create(this.ModManifest.UniqueID);

            // example patch, you'll need to edit this for your patch
            harmony.Patch(
               original: AccessTools.Method(typeof(StardewValley.Object), nameof(StardewValley.Object.canBePlacedHere)),
               prefix: new HarmonyMethod(typeof(ObjectPatches), nameof(ObjectPatches.CanBePlacedHere_Prefix))
            );
        }
    }
}
