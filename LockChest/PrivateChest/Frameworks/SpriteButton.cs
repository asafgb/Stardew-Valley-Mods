using Menu.Interfaces;
using Menu.Widgets;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPrivateChest.Frameworks
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
