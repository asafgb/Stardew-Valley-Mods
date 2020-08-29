using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Widgets
{
    public abstract class Button : Widget
    {
        public event Action OnPress;
        public event Action<Widget> OnHover;

        public override bool ReceiveLeftClick(Point point)
        {
            // Check 
            if ( this.LocalBounds.Contains(point) )
            {
                this.OnPress?.Invoke();
                return true;
            }
            return false;
        }


        public override bool ReceiveCursorHover(Point point)
        {
            this.IsHover = this.LocalBounds.Contains(point);
            this.OnHover?.Invoke(this);
            return this.LocalBounds.Contains(point);
            
        }
    }
}
