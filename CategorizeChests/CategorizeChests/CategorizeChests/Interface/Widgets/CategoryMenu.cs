using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValleyMods.CategorizeChests.Framework;

namespace StardewValleyMods.CategorizeChests.Interface.Widgets
{
		internal class CategoryMenu : Widget
	{
								public event Action OnClose;

						private SpriteFont HeaderFont
		{
			get
			{
				return Game1.dialogueFont;
			}
		}

						private int Padding
		{
			get
			{
				return 2 * Game1.pixelZoom;
			}
		}

						private IEnumerable<ItemToggle> ItemToggles
		{
			get
			{
				return from child in this.ToggleBag.Children
				where child is ItemToggle
				select child as ItemToggle;
			}
		}

				public CategoryMenu(ChestData chestData, IItemDataManager itemDataManager, ITooltipManager tooltipManager)
		{
			this.ItemDataManager = itemDataManager;
			this.TooltipManager = tooltipManager;
			this.ChestData = chestData;
			this.AvailableCategories = this.ItemDataManager.Categories.Keys.ToList<string>();
			this.AvailableCategories.Sort();
			this.BuildWidgets();
			this.SetCategory(this.AvailableCategories.First<string>());
		}

				private void BuildWidgets()
		{
			this.Background = base.AddChild<Background>(new Background(Sprites.MenuBackground));
			this.Body = base.AddChild<Widget>(new Widget());
			this.TopRow = this.Body.AddChild<Widget>(new Widget());
			this.ToggleBag = this.Body.AddChild<WrapBag>(new WrapBag(12 * Game1.tileSize));
			this.NextButton = this.TopRow.AddChild<SpriteButton>(new SpriteButton(Sprites.RightArrow));
			this.PrevButton = this.TopRow.AddChild<SpriteButton>(new SpriteButton(Sprites.LeftArrow));
			this.NextButton.OnPress += delegate()
			{
				this.CycleCategory(1);
			};
			this.PrevButton.OnPress += delegate()
			{
				this.CycleCategory(-1);
			};
			this.SelectAllButton = this.TopRow.AddChild<LabeledCheckbox>(new LabeledCheckbox("All"));
			this.SelectAllButton.OnChange += this.OnToggleSelectAll;
			this.CloseButton = base.AddChild<SpriteButton>(new SpriteButton(Sprites.ExitButton));
			this.CloseButton.OnPress += delegate()
			{
				Action onClose = this.OnClose;
				if (onClose == null)
				{
					return;
				}
				onClose();
			};
			this.CategoryLabel = this.TopRow.AddChild<Label>(new Label("", Color.Black, this.HeaderFont));
		}

				private void PositionElements()
		{
			this.Body.Position = new Point(this.Background.Graphic.LeftBorderThickness, this.Background.Graphic.RightBorderThickness);
			this.Body.Width = this.ToggleBag.Width;
			this.TopRow.Width = this.Body.Width;
			base.Width = this.Body.Width + this.Background.Graphic.LeftBorderThickness + this.Background.Graphic.RightBorderThickness + this.Padding * 2;
			int num = (int)this.HeaderFont.MeasureString(" Animal Product ").X;
			this.NextButton.X = this.TopRow.Width / 2 + num / 2;
			this.PrevButton.X = this.TopRow.Width / 2 - num / 2 - this.PrevButton.Width;
			this.SelectAllButton.X = this.Padding;
			this.CategoryLabel.CenterHorizontally();
			this.TopRow.Height = this.TopRow.Children.Max((Widget c) => c.Height);
			foreach (Widget widget in this.TopRow.Children)
			{
				widget.Y = this.TopRow.Height / 2 - widget.Height / 2;
			}
			this.ToggleBag.Y = this.TopRow.Y + this.TopRow.Height + this.Padding;
			this.Body.Height = this.ToggleBag.Y + this.ToggleBag.Height;
			base.Height = this.Body.Height + this.Background.Graphic.TopBorderThickness + this.Background.Graphic.BottomBorderThickness + this.Padding * 2;
			this.Background.Width = base.Width;
			this.Background.Height = base.Height;
			this.CloseButton.Position = new Point(base.Width - this.CloseButton.Width, 0);
		}

				private void OnToggleSelectAll(bool on)
		{
			if (on)
			{
				this.SelectAll();
				return;
			}
			this.SelectNone();
		}

				private void SelectAll()
		{
			foreach (ItemToggle itemToggle in this.ItemToggles)
			{
				if (!itemToggle.Active)
				{
					itemToggle.Toggle();
				}
			}
		}

				private void SelectNone()
		{
			foreach (ItemToggle itemToggle in this.ItemToggles)
			{
				if (itemToggle.Active)
				{
					itemToggle.Toggle();
				}
			}
		}

				private void CycleCategory(int offset)
		{
			int num = this.AvailableCategories.FindIndex((string c) => c == this.SelectedCategory);
			string category = this.AvailableCategories[Utility.Mod(num + offset, this.AvailableCategories.Count)];
			this.SetCategory(category);
		}

				private void SetCategory(string category)
		{
			this.SelectedCategory = category;
			this.CategoryLabel.Text = category;
			this.RecreateItemToggles();
			this.SelectAllButton.Checked = this.AreAllSelected();
			this.PositionElements();
		}

				private void RecreateItemToggles()
		{
			this.ToggleBag.RemoveChildren();
			using (IEnumerator<ItemKey> enumerator = this.ItemDataManager.Categories[this.SelectedCategory].GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ItemKey itemKey = enumerator.Current;
					this.ToggleBag.AddChild<ItemToggle>(new ItemToggle(this.ItemDataManager, this.TooltipManager, itemKey, this.ChestData.Accepts(itemKey))).OnToggle += delegate()
					{
						this.ToggleItem(itemKey);
					};
				}
			}
		}

				private void ToggleItem(ItemKey itemKey)
		{
			this.ChestData.Toggle(itemKey);
			this.SelectAllButton.Checked = this.AreAllSelected();
		}

				private bool AreAllSelected()
		{
			return this.ItemToggles.Count((ItemToggle t) => !t.Active) == 0;
		}

				public override bool ReceiveLeftClick(Point point)
		{
			base.PropagateLeftClick(point);
			return true;
		}

				public override bool ReceiveScrollWheelAction(int amount)
		{
			this.CycleCategory((amount > 1) ? -1 : 1);
			return true;
		}

				private readonly IItemDataManager ItemDataManager;

				private readonly ITooltipManager TooltipManager;

				private readonly ChestData ChestData;

				private const int MaxItemColumns = 12;

				private Widget Body;

				private Widget TopRow;

				private LabeledCheckbox SelectAllButton;

				private SpriteButton CloseButton;

				private Background Background;

				private Label CategoryLabel;

				private WrapBag ToggleBag;

				private SpriteButton PrevButton;

				private SpriteButton NextButton;

				private List<string> AvailableCategories;

				private string SelectedCategory;
	}
}
