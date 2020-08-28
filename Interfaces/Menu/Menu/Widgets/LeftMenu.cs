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
        private List<Widget> _UnMoveChildren = new List<Widget>();

        private int MaxWidgetDispay { get; set; } = 3;
        private int CurrentPrintIndex { get; set; } = 0;
        private ItemGrabMenu menu = null;
        private PngButton DownArrow = null;

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
            foreach (Widget widget in this._UnMoveChildren)
            {
                widget.Draw(batch);
            }

            PositionButtons(this.menu, MaxWidgetDispay);
            if (MaxWidgetDispay < 0 || this.Children.Count <= MaxWidgetDispay)
            {
                foreach (Widget widget in this.Children)
                {
                    widget.Draw(batch);
                }
            }
            else if (this.Children.Count > 0)
            {
                Widget lastChild = this.Children[CurrentPrintIndex % this.Children.Count];
                int i = CurrentPrintIndex;
                int CounterToDisplay = MaxWidgetDispay;

                do
                {
                    if (CounterToDisplay > 0)
                    {
                        CounterToDisplay--;
                        lastChild = this.Children[i % this.Children.Count];
                        lastChild.Draw(batch);
                    }
                    i++;
                } while (i < CurrentPrintIndex + this.Children.Count);

                CurrentPrintIndex %= this.Children.Count;
                PlaceTheButton(DownArrow, lastChild);
                DownArrow.Draw(batch);


            }
        }

        public void PlaceTheButton(Widget buttonwidget, Widget lastwidget)
        {
            buttonwidget.Position = new Point(lastwidget.Position.X + lastwidget.Width - buttonwidget.Width, lastwidget.Position.Y + lastwidget.Height);
        }

        public void PositionButtons(ItemGrabMenu ItemGrMenu, int WidgetDispay = 3)
        {
            this.menu = ItemGrMenu;

            /// if there less children then we wanna to print
            WidgetDispay = Math.Min(WidgetDispay, this.Children.Count);
            Widget lastchild = null;

            for (int i = CurrentPrintIndex; WidgetDispay > 0 && i < CurrentPrintIndex + this.Children.Count; i++)
            {
                Widget child = this.Children[i % this.Children.Count];
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


        public override bool ReceiveKeyPress(Keys input)
        {
            //return this.RootWidget.ReceiveKeyPress(input);
            return base.ReceiveKeyPress(input);//|| true;
        }

        public override bool ReceiveCursorHover(Point point)
        {
            Point point2 = new Point(point.X - DownArrow.Position.X, point.Y - DownArrow.Position.Y);
            bool isHover = DownArrow.ReceiveCursorHover(point2);
            isHover = base.ReceiveCursorHover(point) || isHover;
            return isHover;
        }

        public override bool ReceiveLeftClick(Point point)
        {
            Point point2 = new Point(point.X - DownArrow.Position.X, point.Y - DownArrow.Position.Y);

            return DownArrow.ReceiveLeftClick(point2) || base.ReceiveLeftClick(point);
            //return base.ReceiveLeftClick(new Point(x, y));
        }



        public override bool ReceiveScrollWheelAction(int amount)
        {
            return true;
            //return this.RootWidget.ReceiveScrollWheelAction(amount);
        }

        public List<Widget> UnMoveChildren
        {
            get
            {
                return this._UnMoveChildren;
            }
        }

        public int GetChildIndex(Widget child)
        {
            return this._UnMoveChildren.IndexOf(child);
        }

        public T AddUnMoveChild<T>(T child, int InsertIndex = -1) where T : Widget
        {
            child.Parent = this;
            if (InsertIndex >= 0)
                this._UnMoveChildren.Insert(InsertIndex, child);
            else
                this._UnMoveChildren.Add(child);
            this.OnContentsChanged();
            return child;
        }


        public void RemoveUnMoveChild(Widget child)
        {
            this._UnMoveChildren.Remove(child);
            child.Parent = null;
            this.OnContentsChanged();
        }

        public void RemoveUnMoveChildren()
        {
            this.RemoveChildren((Widget c) => true);
        }


        public void RemoveUnMoveChildren(Predicate<Widget> shouldRemove)
        {
            foreach (Widget widget in this.Children.Where<Widget>((Func<Widget, bool>)(c => shouldRemove(c))))
                widget.Parent = (Widget)null;
            this._UnMoveChildren.RemoveAll(shouldRemove);
            this.OnContentsChanged();
        }

    }
}
