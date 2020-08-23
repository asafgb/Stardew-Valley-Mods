using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Menu.Widgets;
namespace LockChest.Interface.Widgets
{
    public abstract class Button : Widget
    {
        public event Action OnPress;

        public override bool ReceiveLeftClick(Point point)
        {
            //Point point2 = new Point(point.X - this.Position.X, point.Y - this.Position.Y);
            if (this.LocalBounds.Contains(point))
            {
                this.OnPress?.Invoke();
                return true;
            }
            return false;
        }
    }
}
