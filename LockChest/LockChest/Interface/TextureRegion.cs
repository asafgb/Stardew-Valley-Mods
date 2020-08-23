using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace LockChest.Interface
{
    internal class TextureRegion
    {
        public readonly Texture2D Texture;
        public readonly Rectangle Region;
        public readonly bool Zoom;

        public TextureRegion(Texture2D texture, Rectangle region) : this(texture, region, false)
        {
        }

        public TextureRegion(Texture2D texture, Rectangle region, bool zoom)
        {
            this.Texture = texture;
            this.Region = region;
            this.Zoom = zoom;
        }

        public int Width
        {
            get
            {
                return this.Region.Width * (this.Zoom ? Game1.pixelZoom : 1);
            }
        }

        public int Height
        {
            get
            {
                return this.Region.Height * (this.Zoom ? Game1.pixelZoom : 1);
            }
        }

       
    }
}
