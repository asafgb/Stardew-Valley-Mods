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
using Menu.Interfaces;
using LockChest.Frameworks;

namespace LockChest.Interface.Widgets
{
    public class ChestOverlay : Widget
    {
        private readonly ItemGrabMenu ItemGrabMenu;
        private readonly InventoryMenu InventoryMenu;
        private readonly InventoryMenu.highlightThisItem DefaultChestHighlighter;
        private readonly InventoryMenu.highlightThisItem DefaultInventoryHighlighter;
        private readonly ModConfig Config;
        private readonly Chest Chest;
        private TextButton LockButton;
        private bool IsLocked;
        private AllLockChests allLockChests;
        //private TextButton OpenButton;
        //private TextButton StashButton;
        //private CategoryMenu CategoryMenu;

        public ChestOverlay(ItemGrabMenu menu, Chest chest, AllLockChests allLockChests, ModConfig config)// IChestDataManager chestDataManager, IChestFiller chestFiller, IItemDataManager itemDataManager, ITooltipManager tooltipManager
        {
            this.allLockChests = allLockChests;

            IsLocked = this.allLockChests.GetChests.Any(cst => cst == chest);
            this.Config = config;
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

            //ChestMenu.Instance.WidgetHost.RootWidget.AddChild<TextButton>(new TextButton("test1", Sprites.LeftProtrudingTab));
            
            ((LeftMenu)ChestMenu.Instance.WidgetHost.RootWidget).PositionButtons(this.ItemGrabMenu);
        }

        private void LockButton_OnPress()
        {
            if (!IsLocked)
            {
                this.allLockChests.GetChests.Add(new ChestIdentity(Chest, Game1.player.currentLocation));

            }
            else
            {
                // check it possible to drop items to that chest

                // move Items
                this.allLockChests.GetChests.Remove(this.allLockChests.GetChests.Where(chst => chst == Chest).First());

            }
            allLockChests.SaveTemp();
            IsLocked = !IsLocked;

            this.LockButton.OnPress -= LockButton_OnPress;
            ((LeftMenu)ChestMenu.Instance.WidgetHost.RootWidget).PositionButtons(this.ItemGrabMenu);
            int IndexBeforeRemove = ChestMenu.Instance.WidgetHost.RootWidget.getChildIndex(this.LockButton);
            ChestMenu.Instance.WidgetHost.RootWidget.RemoveChild(this.LockButton);
            AddButtons(IndexBeforeRemove);
        }
    }
}
