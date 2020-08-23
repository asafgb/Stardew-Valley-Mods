using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Menu.Widgets;
using Menu;

namespace LockChest.Interface.Widgets
{
    public class ChestOverlay : Widget
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

        private void AddButtons(int indexToPut =-1)
        {
            this.LockButton = new TextButton(IsLocked ? "Unlock" : "Lock", Sprites.LeftProtrudingTab);
            this.LockButton.OnPress += LockButton_OnPress;

            ChestMenu.Instance.WidgetHost.RootWidget.AddChild<TextButton>(this.LockButton, indexToPut);
            ChestMenu.Instance.WidgetHost.RootWidget.PositionButtons(this.ItemGrabMenu);
            // base.AddChild<TextButton>(this.LockButton);

            //base.PositionButtons(this.ItemGrabMenu);
        }

        private void LockButton_OnPress()
        {
            IsLocked = !IsLocked;
            this.LockButton.OnPress -= LockButton_OnPress;
            ChestMenu.Instance.WidgetHost.RootWidget.PositionButtons(this.ItemGrabMenu);
            int IndexBeforeRemove = ChestMenu.Instance.WidgetHost.RootWidget.getChildIndex(this.LockButton);
            ChestMenu.Instance.WidgetHost.RootWidget.RemoveChild(this.LockButton);
            AddButtons(IndexBeforeRemove);
        }
    }
}
