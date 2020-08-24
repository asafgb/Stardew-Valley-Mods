using Menu.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Widgets
{
    public class LeftMenu : Widget
    {

        private int MaxWidgetDispay { get; set; } = 3;
        private int CurrentPrintIndex { get; set; } = 0;
        private ItemGrabMenu menu = null;
        private PngButton DownArrow =null;

        public LeftMenu()
        {
            //DownArrow = new TextButton("down", Sprites.LeftProtrudingTab);
            DownArrow = new PngButton(Sprites.LeftProtrudingTab);
            DownArrow.OnPress += DownArrow_OnPress;
        }

        private void DownArrow_OnPress()
        {
            CurrentPrintIndex++;
        }

        public override void Draw(SpriteBatch batch)
        {
            this.DrawChildren(batch);
        }

        protected override void DrawChildren(SpriteBatch batch)
        {
            if (MaxWidgetDispay < 0 || this.Children.Count <= MaxWidgetDispay)
            {
                foreach (Widget widget in this.Children)
                {
                    widget.Draw(batch);
                }
            }
            else
            {
                PositionButtons(this.menu, MaxWidgetDispay);
                Widget lastChild = this.Children[CurrentPrintIndex % this.Children.Count];
                int Counter =  MaxWidgetDispay;
                for (int i = CurrentPrintIndex; Counter > 0; i++)
                {
                    if (this.Children[i % this.Children.Count].ShouldMove)
                    {
                        Counter--;
                        lastChild = this.Children[i % this.Children.Count];
                        lastChild.Draw(batch);

                    }
                    else
                    {
                        this.Children[i % this.Children.Count].Draw(batch);
                    }
                }
                CurrentPrintIndex %= this.Children.Count;

                PlaceTheButton(DownArrow, lastChild);
                DownArrow.Draw(batch);
            }
        }


        public void PlaceTheButton(Widget buttonwidget, Widget lastwidget)
        {
            buttonwidget.Position = new Point(lastwidget.Position.X + lastwidget.Width - buttonwidget.Width, lastwidget.Position.Y + lastwidget.Height);
        }

        // this will help for width one size
        //private int maxWidthOfChildren()
        //{
        //    int MaxWidth = 0;
        //    foreach (Widget child in _Children)
        //    {
        //        if(child.ShouldMove)
        //        MaxWidth = Math.Max(child.Width, MaxWidth);
        //    }
        //    return MaxWidth;
        //}

        public void PositionButtons(ItemGrabMenu ItemGrMenu,int WidgetDispay = 3)
        {
            this.menu = ItemGrMenu;

            /// if there less children then we wanna to print
            WidgetDispay = Math.Min(WidgetDispay, this.Children.Count);
            Widget lastchild = null;

            for (int i = CurrentPrintIndex; WidgetDispay > 0; i++)
            {
                Widget child = this.Children[i % this.Children.Count];
                if (child.ShouldMove)
                {
                    if (lastchild == null)
                    {
                        child.Position = new Point(ItemGrMenu.xPositionOnScreen + ItemGrMenu.width / 2 - child.Width - 112 * Game1.pixelZoom, ItemGrMenu.yPositionOnScreen + 22 * Game1.pixelZoom);
                    }
                    else
                    {
                        child.Position = new Point(lastchild.Position.X + lastchild.Width - child.Width, lastchild.Position.Y + lastchild.Height);
                    }
                    lastchild = child;
                    WidgetDispay--;
                }
            }
            //this.StashButton.Width = (this.OpenButton.Width = Math.Max(this.StashButton.Width, this.OpenButton.Width));
            //this.OpenButton.Position = new Point(this.ItemGrabMenu.xPositionOnScreen + this.ItemGrabMenu.width / 2 - this.OpenButton.Width - 112 * Game1.pixelZoom, this.ItemGrabMenu.yPositionOnScreen + 22 * Game1.pixelZoom);
            //this.StashButton.Position = new Point(this.OpenButton.Position.X + this.OpenButton.Width - this.StashButton.Width, this.OpenButton.Position.Y + this.OpenButton.Height);
        }


        public override bool ReceiveKeyPress(Keys input)
        {
            //return this.RootWidget.ReceiveKeyPress(input);
            return base.ReceiveKeyPress(input);//|| true;
        }

        public override bool ReceiveCursorHover(Point point)
        {
            Point point2 = new Point(point.X - DownArrow.Position.X, point.Y - DownArrow.Position.Y);

            return DownArrow.ReceiveCursorHover(point2) || base.ReceiveCursorHover(point);
        }

        public override bool ReceiveLeftClick(Point point)
        {
            Point point2 = new Point(point.X - DownArrow.Position.X, point.Y - DownArrow.Position.Y);
           
            return DownArrow.ReceiveLeftClick(point2) || base.ReceiveLeftClick(point) ;
            //return base.ReceiveLeftClick(new Point(x, y));
        }

     

        public override bool ReceiveScrollWheelAction(int amount)
        {
            return true;
            //return this.RootWidget.ReceiveScrollWheelAction(amount);
        }


    }
}
