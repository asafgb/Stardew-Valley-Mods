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
using MyPrivateChest.Frameworks;
using StardewModdingAPI;

namespace LockChest.Interface.Widgets
{
    public class ChestOverlay : Widget
    {
        private readonly ItemGrabMenu ItemGrabMenu;
        private readonly InventoryMenu InventoryMenu;
        private readonly InventoryMenu.highlightThisItem DefaultChestHighlighter;
        private readonly InventoryMenu.highlightThisItem DefaultInventoryHighlighter;
        private readonly ModConfig Config;
        private TextButton OpenMyPrivateChest;
        private AllPrivateChests AllPrivateChests;
        private PrivateMenu privateMenu;
        private IModHelper helper;


        public ChestOverlay(ItemGrabMenu menu, AllPrivateChests allPrivateChests, ModConfig config,IModHelper helper)// IChestDataManager chestDataManager, IChestFiller chestFiller, IItemDataManager itemDataManager, ITooltipManager tooltipManager
        {
            this.helper = helper;
            this.AllPrivateChests = allPrivateChests;
            this.Config = config;
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

        private void AddButtons(int indexToPut = -1)
        {
            if (Game1.player.currentLocation.ToString() == "StardewValley.Locations.FarmHouse")
            {
                this.OpenMyPrivateChest = new TextButton("Open Chests", Sprites.LeftProtrudingTab);
                this.OpenMyPrivateChest.OnPress += ToggleMenu;
                ChestMenu.Instance.WidgetHost.RootWidget.AddChild<TextButton>(this.OpenMyPrivateChest, indexToPut);
                ((LeftMenu)ChestMenu.Instance.WidgetHost.RootWidget).PositionButtons(this.ItemGrabMenu);
            }
        }

        private void ToggleMenu()
        {

            if (this.privateMenu == null)
            {
                this.OpenPrivateChest();
                return;
            }
            this.ClosePrivateChest();
        }

        private void OpenPrivateChest()
        {
            this.privateMenu = new PrivateMenu(this.AllPrivateChests,this.helper.Reflection);
            this.privateMenu.Position = new Point(this.ItemGrabMenu.xPositionOnScreen + this.ItemGrabMenu.width / 2 - this.privateMenu.Width / 2 - 6 * Game1.pixelZoom, this.ItemGrabMenu.yPositionOnScreen - 10 * Game1.pixelZoom);
            this.privateMenu.OnClose += this.ClosePrivateChest;
            this.privateMenu.ShouldMove = false;
            ChestMenu.Instance.WidgetHost.RootWidget.AddChild<PrivateMenu>(this.privateMenu);
            //base.AddChild<CategoryMenu>(this.CategoryMenu);
        }
        private void ClosePrivateChest()
        {
            //base.RemoveChild(this.CategoryMenu);
            ChestMenu.Instance.WidgetHost.RootWidget.RemoveChild(this.privateMenu);
            this.privateMenu = null;
        }

       

    }
}
