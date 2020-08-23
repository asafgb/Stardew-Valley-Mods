using System;
using Microsoft.Xna.Framework;
using Menu.Widgets;
namespace StardewValleyMods.CategorizeChests.Interface.Widgets
{
		internal class ItemTooltip : Widget
	{
				public ItemTooltip(string name)
		{
			Background background = base.AddChild<Background>(new Background(Sprites.TooltipBackground));
			Label label = base.AddChild<Label>(new Label(name, Color.Black));
			base.Width = (background.Width = label.Width + background.Graphic.LeftBorderThickness + background.Graphic.RightBorderThickness);
			base.Height = (background.Height = label.Height + background.Graphic.TopBorderThickness + background.Graphic.BottomBorderThickness);
			label.Position = new Point(background.Width / 2 - label.Width / 2, background.Height / 2 - label.Height / 2);
		}
	}
}
