using System;
using Menu.Interfaces;
using Menu.Widgets;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValleyMods.CategorizeChests.Interface.Widgets
{
    internal class SpriteButton : Button
    {
        public SpriteButton(TextureRegion textureRegion)
        {
            this.TextureRegion = textureRegion;
            base.Width = this.TextureRegion.Width;
            base.Height = this.TextureRegion.Height;
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(this.TextureRegion.Texture, this.TextureRegion.Region, base.GlobalPosition.X, base.GlobalPosition.Y, this.TextureRegion.Width, this.TextureRegion.Height, null);
        }

        private readonly TextureRegion TextureRegion;
    }
}
