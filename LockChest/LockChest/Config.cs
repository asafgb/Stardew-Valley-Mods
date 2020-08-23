using System;
using Microsoft.Xna.Framework.Input;

namespace LockChest
{
    public class Config
    {
        public Keys StashKey { get; set; } = Keys.L;
        public bool CheckForUpdates { get; set; } = true;
    }
}
