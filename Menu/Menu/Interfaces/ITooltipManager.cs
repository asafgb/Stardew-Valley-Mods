using Menu.Widgets;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Interfaces
{
    public interface ITooltipManager
    {
        void ShowTooltipThisFrame(Widget tooltip);

        void Draw(SpriteBatch batch);
    }
}
