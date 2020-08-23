using System;
using Microsoft.Xna.Framework.Input;

namespace StardewValleyMods.CategorizeChests
{
    internal class Config
    {
        public Keys StashKey { get; set; } = Keys.S;

        public bool CheckForUpdates { get; set; } = true;
    }
}
