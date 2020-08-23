using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValleyMods.CategorizeChests.Framework;
using Menu.Widgets;
using Menu.Interfaces;
using Menu;

namespace StardewValleyMods.CategorizeChests.Interface.Widgets
{
    internal class ChestOverlay : Widget
    {
        private readonly ItemGrabMenu ItemGrabMenu;
        private readonly InventoryMenu InventoryMenu;
        private readonly InventoryMenu.highlightThisItem DefaultChestHighlighter;
        private readonly InventoryMenu.highlightThisItem DefaultInventoryHighlighter;
        private readonly Config Config;
        private readonly IChestDataManager ChestDataManager;
        private readonly IChestFiller ChestFiller;
        private readonly IItemDataManager ItemDataManager;
        private readonly ITooltipManager TooltipManager;
        private readonly Chest Chest;
        private TextButton OpenButton;
        private TextButton StashButton;
        private CategoryMenu CategoryMenu;

        public ChestOverlay(ItemGrabMenu menu, Chest chest, Config config, IChestDataManager chestDataManager, IChestFiller chestFiller, IItemDataManager itemDataManager, ITooltipManager tooltipManager)
        {
            this.Config = config;
            this.ItemDataManager = itemDataManager;
            this.ChestDataManager = chestDataManager;
            this.ChestFiller = chestFiller;
            this.TooltipManager = tooltipManager;
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
            this.OpenButton = new TextButton("Categorize", Sprites.LeftProtrudingTab);
            this.OpenButton.OnPress += this.ToggleMenu;
            ChestMenu.Instance.WidgetHost.RootWidget.AddChild<TextButton>(this.OpenButton);
            //base.AddChild<TextButton>(this.OpenButton);
            this.StashButton = new TextButton(this.ChooseStashButtonLabel(), Sprites.LeftProtrudingTab);
            this.StashButton.OnPress += this.StashItems;
            ChestMenu.Instance.WidgetHost.RootWidget.AddChild<TextButton>(this.StashButton);
            ChestMenu.Instance.WidgetHost.RootWidget.PositionButtons(this.ItemGrabMenu);
            //base.AddChild<TextButton>(this.StashButton);
            //base.PositionButtons(this.ItemGrabMenu);
        }

        

        private string ChooseStashButtonLabel()
        {
            Keys stashKey = this.Config.StashKey;
            if (stashKey == Keys.None)
            {
                return "Stash";
            }
            string name = Enum.GetName(typeof(Keys), stashKey);
            return string.Format("Stash ({0})", name);
        }

        private void ToggleMenu()
        {
            if (this.CategoryMenu == null)
            {
                this.OpenCategoryMenu();
                return;
            }
            this.CloseCategoryMenu();
        }

        private void OpenCategoryMenu()
        {
            ChestData chestData = this.ChestDataManager.GetChestData(this.Chest);
            this.CategoryMenu = new CategoryMenu(chestData, this.ItemDataManager, this.TooltipManager);
            this.CategoryMenu.Position = new Point(this.ItemGrabMenu.xPositionOnScreen + this.ItemGrabMenu.width / 2 - this.CategoryMenu.Width / 2 - 6 * Game1.pixelZoom, this.ItemGrabMenu.yPositionOnScreen - 10 * Game1.pixelZoom);
            this.CategoryMenu.OnClose += this.CloseCategoryMenu;
            this.CategoryMenu.ShouldMove = false;
            ChestMenu.Instance.WidgetHost.RootWidget.AddChild<CategoryMenu>(this.CategoryMenu);
            //base.AddChild<CategoryMenu>(this.CategoryMenu);
            this.SetItemsClickable(false);
        }

        private void CloseCategoryMenu()
        {
            //base.RemoveChild(this.CategoryMenu);
            ChestMenu.Instance.WidgetHost.RootWidget.RemoveChild(this.CategoryMenu);
            this.CategoryMenu = null;
            this.SetItemsClickable(true);
        }

        private void StashItems()
        {
            if (!this.GoodTimeToStash())
            {
                return;
            }
            this.ChestFiller.DumpItemsToChest(this.Chest);
        }

        public override bool ReceiveKeyPress(Keys input)
        {
            if (input == this.Config.StashKey)
            {
                this.StashItems();
                return true;
            }
            return base.PropagateKeyPress(input);
        }

        public override bool ReceiveLeftClick(Point point)
        {
            bool flag = base.PropagateLeftClick(point);
            if (!flag && this.CategoryMenu != null)
            {
                this.CloseCategoryMenu();
            }
            return flag;
        }

        private void SetItemsClickable(bool clickable)
        {
            if (clickable)
            {
                this.ItemGrabMenu.inventory.highlightMethod = this.DefaultChestHighlighter;
                this.InventoryMenu.highlightMethod = this.DefaultInventoryHighlighter;
                return;
            }
            this.ItemGrabMenu.inventory.highlightMethod = ((Item item) => false);
            this.InventoryMenu.highlightMethod = ((Item item) => false);
        }

        private bool GoodTimeToStash()
        {
            return this.ItemsAreClickable() && this.ItemGrabMenu.heldItem == null;
        }

        private bool ItemsAreClickable()
        {
            IEnumerable<Item> actualInventory = this.ItemGrabMenu.inventory.actualInventory;
            InventoryMenu.highlightThisItem highlighter = this.ItemGrabMenu.inventory.highlightMethod;
            return actualInventory.Any((Item item) => highlighter(item));
        }

        
    }
}
