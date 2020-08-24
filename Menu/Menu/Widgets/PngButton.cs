using Menu.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Widgets
{
    public class PngButton : Button
    {
        private readonly Background Background;
        private readonly ImageLabel imageLabel;
        private int LeftPadding
        {
            get
            {
                return this.Background.Graphic.LeftBorderThickness;
            }
        }

        private int RightPadding
        {
            get
            {
                return this.Background.Graphic.RightBorderThickness;
            }
        }

        private int TopPadding
        {
            get
            {
                return this.Background.Graphic.TopBorderThickness;
            }
        }

        private int BottomPadding
        {
            get
            {
                return this.Background.Graphic.BottomBorderThickness;
            }
        }


        public PngButton(NineSlice backgroundTexture)
        {
            //this.Label = new Label(text, Color.Black);
            this.Background = new Background(backgroundTexture);
            this.imageLabel = new ImageLabel();
            base.Width = (this.Background.Width = this.imageLabel.Width + this.LeftPadding + this.RightPadding);
            base.Height = (this.Background.Height = this.imageLabel.Height + this.TopPadding + this.BottomPadding);
            base.AddChild<Background>(this.Background);
            base.AddChild<ImageLabel>(this.imageLabel);
            this.CenterLabel();
        }

        protected override void OnDimensionsChanged()
        {
            base.OnDimensionsChanged();
            if (this.Background != null)
            {
                this.Background.Width = base.Width;
                this.Background.Height = base.Height;
            }
            if (this.imageLabel != null)
            {
                this.CenterLabel();
            }
        }

        private void CenterLabel()
        {
            this.imageLabel.Position = new Point(this.LeftPadding + (base.Width - this.RightPadding - this.LeftPadding) / 2 - this.imageLabel.Width / 2, this.TopPadding + (base.Height - this.BottomPadding - this.TopPadding) / 2 - this.imageLabel.Height / 2);
        }
    }
}
