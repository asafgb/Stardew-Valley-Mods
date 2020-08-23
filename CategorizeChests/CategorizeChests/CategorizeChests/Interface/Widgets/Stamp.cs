using System;
using Microsoft.Xna.Framework.Graphics;
using Menu.Widgets;
namespace StardewValleyMods.CategorizeChests.Interface.Widgets
{
		internal class Stamp : Widget
	{
				public Stamp(TextureRegion textureRegion)
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
