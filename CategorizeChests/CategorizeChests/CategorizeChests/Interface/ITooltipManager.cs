using System;
using Microsoft.Xna.Framework.Graphics;
using StardewValleyMods.CategorizeChests.Interface.Widgets;

namespace StardewValleyMods.CategorizeChests.Interface
{
		internal interface ITooltipManager
	{
				void ShowTooltipThisFrame(Widget tooltip);

				void Draw(SpriteBatch batch);
	}
}
