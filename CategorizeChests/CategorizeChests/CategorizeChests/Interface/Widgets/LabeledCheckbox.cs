using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Menu.Widgets;
namespace StardewValleyMods.CategorizeChests.Interface.Widgets
{
		internal class LabeledCheckbox : Widget
	{
								public event Action<bool> OnChange;

								public bool Checked { get; set; }

				public LabeledCheckbox(string labelText)
		{
			this.CheckedBox = base.AddChild<Stamp>(new Stamp(Sprites.FilledCheckbox));
			this.UncheckedBox = base.AddChild<Stamp>(new Stamp(Sprites.EmptyCheckbox));
			this.Label = base.AddChild<Label>(new Label(labelText, Color.Black));
			int num = (int)this.Label.Font.MeasureString(" ").X;
			base.Height = Math.Max(this.CheckedBox.Height, this.Label.Height);
			this.CheckedBox.CenterVertically();
			this.UncheckedBox.CenterVertically();
			this.Label.CenterVertically();
			this.Label.X = this.CheckedBox.X + this.CheckedBox.Width + num;
			base.Width = this.Label.X + this.Label.Width;
		}

				public override bool ReceiveLeftClick(Point point)
		{
			this.Checked = !this.Checked;
			Action<bool> onChange = this.OnChange;
			if (onChange != null)
			{
				onChange(this.Checked);
			}
			return true;
		}

				public override void Draw(SpriteBatch batch)
		{
			(this.Checked ? this.CheckedBox : this.UncheckedBox).Draw(batch);
			this.Label.Draw(batch);
		}

				private readonly Widget CheckedBox;

				private readonly Widget UncheckedBox;

				private readonly Label Label;
	}
}
