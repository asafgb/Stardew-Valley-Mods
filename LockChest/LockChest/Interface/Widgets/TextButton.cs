

using Microsoft.Xna.Framework;

namespace LockChest.Interface.Widgets
{
    internal class TextButton : Button
    {
        private readonly Background Background;

        private readonly Label Label;

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


        public TextButton(string text, NineSlice backgroundTexture)
        {
            this.Label = new Label(text, Color.Black);
            this.Background = new Background(backgroundTexture);
            base.Width = (this.Background.Width = this.Label.Width + this.LeftPadding + this.RightPadding);
            base.Height = (this.Background.Height = this.Label.Height + this.TopPadding + this.BottomPadding);
            base.AddChild<Background>(this.Background);
            base.AddChild<Label>(this.Label);
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
            if (this.Label != null)
            {
                this.CenterLabel();
            }
        }

        private void CenterLabel()
        {
            this.Label.Position = new Point(this.LeftPadding + (base.Width - this.RightPadding - this.LeftPadding) / 2 - this.Label.Width / 2, this.TopPadding + (base.Height - this.BottomPadding - this.TopPadding) / 2 - this.Label.Height / 2);
        }

    }
}
