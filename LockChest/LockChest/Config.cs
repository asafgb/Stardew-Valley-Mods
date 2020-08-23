using System;
using Microsoft.Xna.Framework.Input;

namespace LockChest
{
    internal class Config
    {
        public Keys StashKey { get; set; } = Keys.L;

        public bool CheckForUpdates { get; set; } = true;
    }
}
