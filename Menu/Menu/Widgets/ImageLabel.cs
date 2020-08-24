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
       // Texture2D tex;
        public ImageLabel()
        {
            this.RecalculateDimensions();
        }

        public override void Draw(SpriteBatch batch)
        {
            //batch.DrawString(this.Font, this.Text, new Vector2((float)base.GlobalPosition.X, (float)base.GlobalPosition.Y), this.Color);
            //batch.Draw(tex, new Vector2((float)base.GlobalPosition.X, (float)base.GlobalPosition.Y), Color.White);
            //batch.Draw(Game1.mouseCursors, new Rectangle(421, 459, 12, 12), new Rectangle(base.GlobalPosition.X, base.GlobalPosition.Y, base.Width, base.Height), Color.White);
            batch.Draw(Game1.mouseCursors, new Rectangle(base.GlobalPosition.X, base.GlobalPosition.Y, base.Width, base.Height), new Rectangle(421, 472, 12, 12),  Color.White);
            //throw new Exception("asaf");
        }

        private void RecalculateDimensions()
        {
            base.Width = (int)13*3;
            base.Height = (int)13*2;
        }

       
    }
}
