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
        private SpriteButton NextButton;
        Background test;


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
            if (Game1.player.currentLocation.ToString() == "StardewValley.Locations.FarmHouse" && ChestMenu.Instance.chest as PrivateMenu == null)
            {
                this.OpenMyPrivateChest = new TextButton("Private Chests", Sprites.LeftProtrudingTab);
                this.OpenMyPrivateChest.OnPress += OpenMenu;
                ChestMenu.Instance.WidgetHost.RootWidget.AddChild<TextButton>(this.OpenMyPrivateChest, indexToPut);
                ((LeftMenu)ChestMenu.Instance.WidgetHost.RootWidget).PositionButtons(this.ItemGrabMenu);
            }else if (ChestMenu.Instance.chest as PrivateMenu != null)
            {
                test = ChestMenu.Instance.WidgetHost.RootWidget.AddChild<Background>(new Background(Sprites.MenuBackground));
                test.ShouldMove = false;
                Body = ChestMenu.Instance.WidgetHost.RootWidget.AddChild<Widget>(new Widget());
                TopRow = Body.AddChild<Widget>(new Widget());
                NextButton = TopRow.AddChild<SpriteButton>(new SpriteButton(Sprites.RightArrow));
                PrevButton = TopRow.AddChild<SpriteButton>(new SpriteButton(Sprites.LeftArrow));
                NextButton.OnPress += delegate ()
                {
                    this.AllPrivateChests.AddIndex(1);
                };
                PrevButton.OnPress += delegate ()
                {
                    this.AllPrivateChests.AddIndex(-1);
                };
                Body.ShouldMove = false;
                //TopRow.ShouldMove = false;
                //NextButton.ShouldMove = false;
                //PrevButton.ShouldMove = false;
                this.PositionElements();
            }
        }

        private void PositionElements()
        {
            //this.Background.Position =new Point(this.ItemGrabMenu.xPositionOnScreen, this.ItemGrabMenu.yPositionOnScreen - 100);
            this.Body.Position = new Point(this.ItemGrabMenu.xPositionOnScreen+128, this.ItemGrabMenu.yPositionOnScreen - 45);
            this.Body.Width = 12*48;
            this.TopRow.Width = this.Body.Width;
            base.Width = this.Body.Width + this.Body.Width + this.Body.Width + this.Padding * 2;
            int num = 840;
            this.NextButton.X = this.TopRow.Width / 2 + num / 2;
            this.PrevButton.X = this.TopRow.Width / 2 - num / 2 - this.PrevButton.Width;
            //this.CategoryLabel.CenterHorizontally();
            this.TopRow.Height = this.TopRow.Children.Max((Widget c) => c.Height);
            foreach (Widget widget in this.TopRow.Children)
            {
                widget.Y = this.TopRow.Height / 2 - widget.Height / 2;
                widget.Height /= 2;
                widget.Width /= 2;
            }
            //Amount.Position = new Point((this.NextButton.X + this.PrevButton.X) / 2, this.NextButton.Y);

            this.Body.Height = 64;
            //base.Height = this.Body.Height + this.Background.Graphic.TopBorderThickness + this.Background.Graphic.BottomBorderThickness + this.Padding * 2;
        }


        private void OpenMenu()
        {
            new PrivateMenu(helper.Reflection).OpenMenu(this.AllPrivateChests, 1);
        }


        private int Padding
        {
            get
            {
                return 2 * Game1.pixelZoom;
            }
        }

    }
}
