using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValleyMods.CategorizeChests.Interface.Widgets;

namespace StardewValleyMods.CategorizeChests.Interface
{
    internal class TooltipManager : ITooltipManager
    {
        public void ShowTooltipThisFrame(Widget tooltip)
        {
            this.Tooltip = tooltip;
        }

        public void Draw(SpriteBatch batch)
        {
            if (this.Tooltip != null)
            {
                Point mousePosition = Game1.getMousePosition();
                this.Tooltip.Position = new Point(mousePosition.X + 8 * Game1.pixelZoom, mousePosition.Y + 8 * Game1.pixelZoom);
                this.Tooltip.Draw(batch);
                this.Tooltip = null;
            }
        }

        private Widget Tooltip;
    }
}
