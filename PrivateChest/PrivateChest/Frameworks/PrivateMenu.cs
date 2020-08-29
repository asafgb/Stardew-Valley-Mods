using ItemManager;
using LockChest.Frameworks;
using LockChest.Interface;
using Menu.Interfaces;
using Menu.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Background = Menu.Widgets.Background;

namespace MyPrivateChest.Frameworks
{
    internal class PrivateMenu : Chest
    {
        //public event Action OnClose;
        private Chest CurrChest;
        AllPrivateChests allPrivateChests;
        private readonly IReflectionHelper Reflection;

        //private Widget Body;

        //private Widget TopRow;

        //private SpriteButton CloseButton;

        //private Background Background;

        //private Label CategoryLabel;

        //private SpriteButton PrevButton;

        //private SpriteButton NextButton;

        //


        //private string SelectedCategory;


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
        public PrivateMenu(IReflectionHelper Reflection)
        {
            this.Reflection = Reflection;
        }


        public void OpenMenu(AllPrivateChests allPrivateChests)
        {
            this.allPrivateChests = allPrivateChests;
            //Game1.exitActiveMenu();
            CurrChest = allPrivateChests.GetCurrentChest;
            IClickableMenu menu;
            switch (Constants.TargetPlatform)
            {
                case GamePlatform.Android:
                    menu = new ItemGrabMenu(
                    inventory: CurrChest.items,
                    reverseGrab: true,
                    showReceivingMenu: true,
                    highlightFunction: this.CanAcceptItem,
                    behaviorOnItemSelectFunction: null,
                    message: null,
                    behaviorOnItemGrab: null,
                    canBeExitedWithKey: true,
                    showOrganizeButton: true,
                    source: ItemGrabMenu.source_chest,
                    sourceItem: CurrChest,
                    context: CurrChest);
                    break;

                default:
                    menu = new ItemGrabMenu(
                    inventory: CurrChest.items,
                    reverseGrab: false,
                    showReceivingMenu: true,
                    highlightFunction: this.CanAcceptItem,
                    behaviorOnItemSelectFunction: this.GrabItemFromPlayer,
                    message: null,
                    behaviorOnItemGrab: this.GrabItemFromContainer,
                    canBeExitedWithKey: true,
                    showOrganizeButton: true,
                    source: ItemGrabMenu.source_chest,
                    sourceItem: CurrChest,
                    context: CurrChest);
                    break;

            }
            Game1.activeClickableMenu = menu;

        }

        private void GrabItemFromPlayer(Item item, Farmer player)
        {
            CurrChest.grabItemFromInventory(item, player);
            
            this.OnChanged();
        }


        private void GrabItemFromContainer(Item item, Farmer player)
        {
            CurrChest.grabItemFromChest(item, player);
            this.OnChanged();
        }

        protected virtual void OnChanged()
        {
            if (Game1.activeClickableMenu is ItemGrabMenu)
            {
                ((ItemGrabMenu)Game1.activeClickableMenu).behaviorOnItemGrab = this.GrabItemFromContainer;
                this.Reflection.GetField<ItemGrabMenu.behaviorOnItemSelect>(Game1.activeClickableMenu, "behaviorFunction").SetValue(this.GrabItemFromPlayer);
                List<ItemKeeper> keepers= this.CurrChest.items.Select(itm => new ItemKeeper(ItemDataManager.Instance.GetKey(itm),
                    itm as Tool != null ? ((Tool)itm).UpgradeLevel : ((StardewValley.Object)itm).Quality,itm.Stack)).ToList();
                this.allPrivateChests.getCurrentChestId.SetInventory(keepers);
                //allPrivateChests.SaveTemp();
            }
        }

        public bool CanAcceptItem(Item item)
        {
            return InventoryMenu.highlightAllItems(item);
        }

        private void BuildWidgets()
        {
            //this.Background = base.AddChild<Background>(new Background(Sprites.MenuBackground));
            //this.Body = base.AddChild<Widget>(new Widget());
            //this.TopRow = this.Body.AddChild<Widget>(new Widget());
            //this.NextButton = this.TopRow.AddChild<SpriteButton>(new SpriteButton(Sprites.RightArrow));
            //this.PrevButton = this.TopRow.AddChild<SpriteButton>(new SpriteButton(Sprites.LeftArrow));
            //this.NextButton.OnPress += delegate ()
            //{
            //    this.CycleCategory(1);
            //};
            //this.PrevButton.OnPress += delegate ()
            //{
            //    this.CycleCategory(-1);
            //};
            //this.CloseButton = base.AddChild<SpriteButton>(new SpriteButton(Sprites.ExitButton));
            //this.CloseButton.OnPress += delegate ()
            //{
            //    Action onClose = this.OnClose;
            //    if (onClose == null)
            //    {
            //        return;
            //    }
            //    onClose();
            //};
            //this.CategoryLabel = this.TopRow.AddChild<Label>(new Label("", Color.Black, this.HeaderFont));
        }

        private void PositionElements()
        {
            //this.Body.Position = new Point(this.Background.Graphic.LeftBorderThickness, this.Background.Graphic.RightBorderThickness);
            //this.TopRow.Width = this.Body.Width;
            //base.Width = this.Body.Width + this.Background.Graphic.LeftBorderThickness + this.Background.Graphic.RightBorderThickness + this.Padding * 2;
            //int num = (int)this.HeaderFont.MeasureString(" Animal Product ").X;
            //this.NextButton.X = this.TopRow.Width / 2 + num / 2;
            //this.PrevButton.X = this.TopRow.Width / 2 - num / 2 - this.PrevButton.Width;
            //this.CategoryLabel.CenterHorizontally();
            //this.TopRow.Height = this.TopRow.Children.Max((Widget c) => c.Height);
            //foreach (Widget widget in this.TopRow.Children)
            //{
            //    widget.Y = this.TopRow.Height / 2 - widget.Height / 2;
            //}
            //base.Height = this.Body.Height + this.Background.Graphic.TopBorderThickness + this.Background.Graphic.BottomBorderThickness + this.Padding * 2;
            //this.Background.Width = base.Width;
            //this.Background.Height = base.Height;
            //this.CloseButton.Position = new Point(base.Width - this.CloseButton.Width, 0);
        }





        //private void CycleCategory(int offset)
        //{
        //    ChestIndex = Mod(ChestIndex + offset, AllPrivateChests.GetChests.Count);
        //}




        //public override bool ReceiveLeftClick(Point point)
        //{
        //    base.PropagateLeftClick(point);
        //    return true;
        //}

        //public override bool ReceiveScrollWheelAction(int amount)
        //{
        //    this.CycleCategory((amount > 1) ? -1 : 1);
        //    return true;
        //}



    }
}
