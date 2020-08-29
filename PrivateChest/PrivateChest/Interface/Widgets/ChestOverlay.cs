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
using Background = Menu.Widgets.Background;

namespace LockChest.Interface.Widgets
{
    public class ChestOverlay : Widget
    {
        private readonly ItemGrabMenu ItemGrabMenu;
        private TextButton OpenMyPrivateChest;
        private AllPrivateChests AllPrivateChests;
        private IModHelper helper;

        private Widget Body;
        private Widget TopRow;
        private SpriteButton PrevButton;
        private Label ChestIndex;
        private SpriteButton NextButton;


        public ChestOverlay(ItemGrabMenu menu, AllPrivateChests allPrivateChests,IModHelper helper)// IChestDataManager chestDataManager, IChestFiller chestFiller, IItemDataManager itemDataManager, ITooltipManager tooltipManager
        {
            this.helper = helper;
            this.AllPrivateChests = allPrivateChests;
            this.ItemGrabMenu = menu;
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
            LeftMenu temp = ChestMenu.Instance.WidgetHost.RootWidget as LeftMenu;
            if (Game1.player.currentLocation.ToString() == "StardewValley.Locations.FarmHouse" && ChestMenu.Instance.chest as PrivateMenu == null)
            {
                this.OpenMyPrivateChest = new TextButton("Private Chests", Sprites.LeftProtrudingTab);
                this.OpenMyPrivateChest.OnPress += OpenMenu;
                ChestMenu.Instance.WidgetHost.RootWidget.AddChild<TextButton>(this.OpenMyPrivateChest, indexToPut);
                ((LeftMenu)ChestMenu.Instance.WidgetHost.RootWidget).PositionButtons(this.ItemGrabMenu);
            }else if (ChestMenu.Instance.chest as PrivateMenu != null)
            {
                NextButton = temp.AddUnMoveChild<SpriteButton>(new SpriteButton(Sprites.RightArrow));
                ChestIndex = temp.AddUnMoveChild<Label>(new Label($"{this.AllPrivateChests.CurrChestIndex+1}", Color.Black));
                PrevButton = temp.AddUnMoveChild<SpriteButton>(new SpriteButton(Sprites.LeftArrow));
                NextButton.OnPress += delegate ()
                {
                    this.AllPrivateChests.AddIndex(1);
                };
                PrevButton.OnPress += delegate ()
                {
                    this.AllPrivateChests.AddIndex(-1);
                };
                this.PositionElements();
            }
        }

        private void PositionElements()
        {
            this.NextButton.Position = new Point(this.ItemGrabMenu.xPositionOnScreen + 836, this.ItemGrabMenu.yPositionOnScreen - 45);
            this.PrevButton.Position = new Point(this.ItemGrabMenu.xPositionOnScreen -48, this.ItemGrabMenu.yPositionOnScreen - 45);
            this.ChestIndex.Position = new Point((this.NextButton.X + this.PrevButton.X) / 2, (this.NextButton.Y+5));
        }


        private void OpenMenu()
        {
            new PrivateMenu(helper.Reflection).OpenMenu(this.AllPrivateChests);
        }
    }
}
