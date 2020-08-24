using Menu.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Widgets
{
    public class ImageLabel : Widget
    {
        Double Index = 0;
       // Texture2D tex;
        public ImageLabel()
        {
            this.RecalculateDimensions();
        }

        public override void Draw(SpriteBatch batch)
        {
            if (this.IsHover)
            {
                Index = Index + 0.25 > 10 ? -4 : Index + 0.25;//(Index + 0.25) % 10;
                batch.Draw(Game1.mouseCursors, new Rectangle(base.GlobalPosition.X, base.GlobalPosition.Y+ (int)Index, base.Width, base.Height), new Rectangle(421, 472, 12, 12), Color.Red);
                //batch.Draw(Game1.mouseCursors, new Rectangle(Index*20, Index*30, base.Width, base.Height), new Rectangle(421, 472, 12, 12), Color.Red);
            }
            else
            {
                batch.Draw(Game1.mouseCursors, new Rectangle(base.GlobalPosition.X, base.GlobalPosition.Y, base.Width, base.Height), new Rectangle(421, 472, 12, 12), Color.White);
            }
        }

        private void RecalculateDimensions()
        {
            base.Width = (int)13*3;
            base.Height = (int)13*2;
        }

      

    }
}
