using System;
using Menu.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Menu.Interfaces
{
    internal class TooltipManager : ITooltipManager
    {
        private Widget Tooltip;

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

    }
}
