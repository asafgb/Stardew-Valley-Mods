using System;
using Microsoft.Xna.Framework.Input;

namespace StardewValleyMods.CategorizeChests
{
    internal class Config
    {
        public Keys StashKey { get; set; } = Keys.L;
        public bool CheckForUpdates { get; set; } = true;
        public string GithubUrlForProjectManifest { get; set; }
    }
}
