using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Interfaces
{
    public class NineSlice
    {
        public TextureRegion Center;
        public TextureRegion Top;
        public TextureRegion TopRight;
        public TextureRegion Right;
        public TextureRegion BottomRight;
        public TextureRegion Bottom;
        public TextureRegion BottomLeft;
        public TextureRegion Left;
        public TextureRegion TopLeft;

        public int RightBorderThickness
        {
            get
            {
                return this.Right.Width;
            }
        }

        public int LeftBorderThickness
        {
            get
            {
                return this.Left.Width;
            }
        }

        public int TopBorderThickness
        {
            get
            {
                return this.Top.Height;
            }
        }

        public int BottomBorderThickness
        {
            get
            {
                return this.Bottom.Height;
            }
        }

        public void Draw(SpriteBatch batch, Rectangle bounds)
        {
            batch.Draw(this.Center, bounds.X + this.Left.Width, bounds.Y + this.Top.Height, bounds.Width - this.Left.Width - this.Right.Width, bounds.Height - this.Top.Height - this.Bottom.Height, null);
            batch.Draw(this.Top, bounds.X + this.TopLeft.Width, bounds.Y, bounds.Width - this.TopLeft.Width - this.TopRight.Width, this.Top.Height, null);
            batch.Draw(this.Left, bounds.X, bounds.Y + this.TopLeft.Height, this.Left.Width, bounds.Height - this.TopLeft.Height - this.BottomLeft.Height, null);
            batch.Draw(this.Right, bounds.X + bounds.Width - this.Right.Width, bounds.Y + this.TopRight.Height, this.Right.Width, bounds.Height - this.TopRight.Height - this.BottomRight.Height, null);
            batch.Draw(this.Bottom, bounds.X + this.BottomLeft.Width, bounds.Y + bounds.Height - this.Bottom.Height, bounds.Width - this.BottomLeft.Width - this.BottomRight.Width, this.Bottom.Height, null);
            batch.Draw(this.TopLeft, bounds.X, bounds.Y, this.TopLeft.Width, this.TopLeft.Height, null);
            batch.Draw(this.TopRight, bounds.X + bounds.Width - this.TopRight.Width, bounds.Y, this.TopRight.Width, this.TopRight.Height, null);
            batch.Draw(this.BottomLeft, bounds.X, bounds.Y + bounds.Height - this.BottomLeft.Height, this.BottomLeft.Width, this.BottomLeft.Height, null);
            batch.Draw(this.BottomRight, bounds.X + bounds.Width - this.BottomRight.Width, bounds.Y + bounds.Height - this.BottomRight.Height, this.BottomRight.Width, this.BottomRight.Height, null);
        }
    }
}
