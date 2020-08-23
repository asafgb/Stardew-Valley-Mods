
using Menu.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;

namespace Menu.Interfaces
{
    public class WidgetHost : InterfaceHost
    {
        public readonly ITooltipManager TooltipManager;
        public readonly Widget RootWidget;

        public WidgetHost(IModHelper helper) : base(helper, null)
        {
            this.RootWidget = new Widget
            {
                Width = Game1.viewport.Width,
                Height = Game1.viewport.Height
            };
              this.TooltipManager = new TooltipManager();
        }

        protected override void Draw(SpriteBatch batch)
        {
            this.RootWidget.Draw(batch);
            base.DrawCursor();
             this.TooltipManager.Draw(batch);
        }

        protected override bool ReceiveKeyPress(Keys input)
        {
            return this.RootWidget.ReceiveKeyPress(input);
        }

        protected override bool ReceiveLeftClick(int x, int y)
        {
            return this.RootWidget.ReceiveLeftClick(new Point(x, y));
        }

        protected override bool ReceiveCursorHover(int x, int y)
        {
            return this.RootWidget.ReceiveCursorHover(new Point(x, y));
        }

        protected override bool ReceiveScrollWheelAction(int amount)
        {
            return this.RootWidget.ReceiveScrollWheelAction(amount);
        }
    }
}
