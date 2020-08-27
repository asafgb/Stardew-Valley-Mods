using System;
using Microsoft.Xna.Framework;
using Menu.Widgets;
namespace StardewValleyMods.CategorizeChests.Interface.Widgets
{
    internal class WrapBag : Widget
    {
        public WrapBag(int width)
        {
            base.Width = width;
        }

        protected override void OnContentsChanged()
        {
            base.OnContentsChanged();
            int num = 0;
            int y = 0;
            int num2 = 0;
            foreach (Widget widget in base.Children)
            {
                if (num + widget.Width > base.Width && num > 0)
                {
                    num = 0;
                    y = num2;
                }
                widget.Position = new Point(num, y);
                num += widget.Width;
                num2 = Math.Max(num2, widget.Position.Y + widget.Height);
            }
            base.Height = num2;
        }
    }
}
