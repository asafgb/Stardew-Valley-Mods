using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using Menu.Widgets;
namespace StardewValleyMods.CategorizeChests.Interface.Widgets
{
    internal class Label : Widget
    {
        public string Text
        {
            get
            {
                return this._Text;
            }
            set
            {
                this._Text = value;
                this.RecalculateDimensions();
            }
        }

        public Label(string text, Color color, SpriteFont font)
        {
            this.Font = font;
            this.Color = color;
            this.Text = text;
            this.RecalculateDimensions();
        }

        public Label(string text, Color color) : this(text, color, Game1.smallFont)
        {
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.DrawString(this.Font, this.Text, new Vector2((float)base.GlobalPosition.X, (float)base.GlobalPosition.Y), this.Color);
        }

        private void RecalculateDimensions()
        {
            Vector2 vector = this.Font.MeasureString(this.Text);
            base.Width = (int)vector.X;
            base.Height = (int)vector.Y;
        }

        private string _Text;

        public readonly SpriteFont Font;

        public readonly Color Color;
    }
}
