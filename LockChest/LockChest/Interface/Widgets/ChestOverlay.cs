using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockChest.Interface.Widgets
{
    internal class ChestOverlay : Widget
    {
        private readonly ItemGrabMenu ItemGrabMenu;
        private readonly InventoryMenu InventoryMenu;
        private readonly InventoryMenu.highlightThisItem DefaultChestHighlighter;
        private readonly InventoryMenu.highlightThisItem DefaultInventoryHighlighter;
        private readonly Config Config;
        //private readonly IChestDataManager ChestDataManager;
        //private readonly IChestFiller ChestFiller;
        //private readonly IItemDataManager ItemDataManager;
        //private readonly ITooltipManager TooltipManager;
        private readonly Chest Chest;
        private TextButton LockButton;
        private bool IsLocked;
        //private TextButton OpenButton;
        //private TextButton StashButton;
        //private CategoryMenu CategoryMenu;

        public ChestOverlay(ItemGrabMenu menu, Chest chest, Config config)// IChestDataManager chestDataManager, IChestFiller chestFiller, IItemDataManager itemDataManager, ITooltipManager tooltipManager
        {
            this.Config = config;
            //this.ItemDataManager = itemDataManager;
            //this.ChestDataManager = chestDataManager;
            //this.ChestFiller = chestFiller;
            //this.TooltipManager = tooltipManager;
            this.Chest = chest;
            this.ItemGrabMenu = menu;
            this.InventoryMenu = menu.ItemsToGrabMenu;
            this.DefaultChestHighlighter = this.ItemGrabMenu.inventory.highlightMethod;
            this.DefaultInventoryHighlighter = this.InventoryMenu.highlightMethod;
            this.AddButtons();
        }

        protected override void OnParent(Widget parent)
        {
            base.OnParent(parent);
            if (parent != null)
            {
                base.Width = parent.Width;
                base.Height = parent.Height;
            }
        }

        private void AddButtons()
        {
            this.LockButton = new TextButton(IsLocked ? "Unlock" : "Lock", Sprites.LeftProtrudingTab);
            this.LockButton.OnPress += LockButton_OnPress;
            base.AddChild<TextButton>(this.LockButton);

            this.PositionButtons();
        }

        private void LockButton_OnPress()
        {
            IsLocked = !IsLocked;
            this.LockButton.OnPress -= LockButton_OnPress;
            base.RemoveChild(this.LockButton);
            AddButtons();
        }

        private void PositionButtons()
        {
            this.LockButton.Width += 12;
            this.LockButton.Position = new Point(this.ItemGrabMenu.xPositionOnScreen + this.ItemGrabMenu.width / 2 - this.LockButton.Width - 112 * Game1.pixelZoom, this.ItemGrabMenu.yPositionOnScreen + 22 * Game1.pixelZoom);
            //this.StashButton.Position = new Point(this.OpenButton.Position.X + this.OpenButton.Width - this.StashButton.Width, this.OpenButton.Position.Y + this.OpenButton.Height);
        }
    }
}
