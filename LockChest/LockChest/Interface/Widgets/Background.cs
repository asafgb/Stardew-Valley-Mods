using System;
using Menu.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LockChest.Interface.Widgets
{
    internal class Background : Widget
    {
        public Background(NineSlice nineSlice)
        {
            this.Graphic = nineSlice;
        }

        public Background(NineSlice nineSlice, int width, int height)
        {
            this.Graphic = nineSlice;
            base.Width = width;
            base.Height = height;
        }

        public override void Draw(SpriteBatch batch)
        {
            this.Graphic.Draw(batch, new Rectangle(base.GlobalPosition.X, base.GlobalPosition.Y, base.Width, base.Height));
        }

        public readonly NineSlice Graphic;
    }
}
