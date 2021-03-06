﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValleyMods.CategorizeChests.Framework;
using Menu.Widgets;
using Menu.Interfaces;
using ItemManager.interfaces;
using StardewValley.Objects;

namespace StardewValleyMods.CategorizeChests.Interface.Widgets
{
    internal class ItemToggle : Widget
    {
        public event Action OnToggle;

        public ItemToggle(IItemDataManager itemDataManager, ITooltipManager tooltipManager, ItemKey itemKey, bool active)
        {
            this.ItemDataManager = itemDataManager;
            this.TooltipManager = tooltipManager;
            this.ItemKey = itemKey;
            this.Active = active;
            base.Width = Game1.tileSize;
            base.Height = Game1.tileSize;
            string name = this.ItemDataManager.GetItem(this.ItemKey).Name;
            this.Tooltip = new ItemTooltip(name);
        }

        public override void Draw(SpriteBatch batch)
        {
            Item item = this.ItemDataManager.GetItem(this.ItemKey);
            float num = this.Active ? 1f : 0.25f;
            if(item as Wallpaper != null)
            {
                item.drawInMenu(batch, new Vector2((float)base.GlobalPosition.X, (float)base.GlobalPosition.Y), 1f, num, 0f, StackDrawType.Hide);
            }
            else
            item.drawInMenu(batch, new Vector2((float)base.GlobalPosition.X, (float)base.GlobalPosition.Y), 1f, num, 1f, StackDrawType.Hide);
            
            //item.GetFieldValue
            if (base.GlobalBounds.Contains(Game1.getMousePosition()))
            {
                this.TooltipManager.ShowTooltipThisFrame(this.Tooltip);
            }
        }

        public void Toggle()
        {
            this.Active = !this.Active;
            Action onToggle = this.OnToggle;
            if (onToggle == null)
            {
                return;
            }
            onToggle();
        }

        public override bool ReceiveLeftClick(Point point)
        {
            if (this.LocalBounds.Contains(point))
            {
                this.Toggle();
                return true;
            }
            return false;
        }

        private readonly IItemDataManager ItemDataManager;

        private readonly ITooltipManager TooltipManager;

        private readonly ItemKey ItemKey;

        public bool Active;

        private Widget Tooltip;
    }
}
