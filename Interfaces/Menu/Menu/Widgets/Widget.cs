using System;
using System.Collections.Generic;
using System.Linq;
using Menu.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;

namespace Menu.Widgets
{
    public class Widget
    {
        private Widget _Parent;
        private List<Widget> _Children = new List<Widget>();
        private int _Width;
        private int _Height;
        



        public Widget Parent
        {
            get
            {
                return this._Parent;
            }
            set
            {
                this._Parent = value;
                this.OnParent(value);
            }
        }

        public bool IsHover { get; set; } = false;

        public List<Widget> Children
        {
            get
            {
                return this._Children;
            }
        }

        public Point Position { get; set; }

        public int X
        {
            get
            {
                return this.Position.X;
            }
            set
            {
                this.Position = new Point(value, this.Position.Y);
            }
        }

        public int Y
        {
            get
            {
                return this.Position.Y;
            }
            set
            {
                this.Position = new Point(this.Position.X, value);
            }
        }

        public int Width
        {
            get
            {
                return this._Width;
            }
            set
            {
                this._Width = value;
                this.OnDimensionsChanged();
            }
        }

        public int Height
        {
            get
            {
                return this._Height;
            }
            set
            {
                this._Height = value;
                this.OnDimensionsChanged();
            }
        }

        public Widget()
        {
            this.Position = Point.Zero;
            this.Width = 1;
            this.Height = 1;
        }

        protected virtual void OnParent(Widget parent)
        {
        }

        public int getChildIndex(Widget child)
        {
            return this._Children.IndexOf(child);
        }


        public virtual void Draw(SpriteBatch batch)
        {
            this.DrawChildren(batch);
        }

        protected virtual void DrawChildren(SpriteBatch batch)
        {
            foreach (Widget widget in this.Children)
            {
                widget.Draw(batch);
            }
        }

        public Rectangle LocalBounds
        {
            get
            {
                return new Rectangle(0, 0, this.Width, this.Height);
            }
        }

        public Rectangle GlobalBounds
        {
            get
            {
                return new Rectangle(this.GlobalPosition.X, this.GlobalPosition.Y, this.Width, this.Height);
            }
        }

        public Point GlobalPosition
        {
            get
            {
                return this.Globalize(Point.Zero);
            }
        }

        public bool Contains(Point point)
        {
            return point.X >= this.Position.X && point.X <= this.Position.X + this.Width && point.Y >= this.Position.Y && point.Y <= this.Position.Y + this.Height;
        }

        public Point Globalize(Point point)
        {
            Point point2 = new Point(point.X + this.Position.X, point.Y + this.Position.Y);
            if (this.Parent == null)
            {
                return point2;
            }
            return this.Parent.Globalize(point2);
        }

        public virtual bool ReceiveKeyPress(Keys input)
        {
            return this.PropagateKeyPress(input);
        }

        public virtual bool ReceiveLeftClick(Point point)
        {
            return this.PropagateLeftClick(point);
        }

        public virtual bool ReceiveCursorHover(Point point)
        {
            return this.PropagateCursorHover(point);
        }

        public virtual bool ReceiveScrollWheelAction(int amount)
        {
            return this.PropagateScrollWheelAction(amount);
        }

        //public bool ShouldMove { get; set; } = true;

        protected bool PropagateKeyPress(Keys input)
        {
            using (IEnumerator<Widget> enumerator = this.Children.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.ReceiveKeyPress(input))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected bool PropagateScrollWheelAction(int amount)
        {
            using (IEnumerator<Widget> enumerator = this.Children.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.ReceiveScrollWheelAction(amount))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected bool PropagateLeftClick(Point point)
        {
            foreach (Widget widget in this.Children)
            {

                Point point2 = new Point(point.X - widget.Position.X, point.Y - widget.Position.Y);
                if (widget.LocalBounds.Contains(point2) && widget.ReceiveLeftClick(point2))
                {
                    return true;
                }
            }
            return false;
        }

        protected bool PropagateCursorHover(Point point)
        {
            bool isHover = false;
            foreach (Widget widget in this.Children)
            {
                Point point2 = new Point(point.X - widget.Position.X, point.Y - widget.Position.Y);
                if (widget.ReceiveCursorHover(point2))
                {
                    isHover = true;
                }
            }
            return isHover;
        }

        public T AddChild<T>(T child,int InsertIndex=-1) where T : Widget
        {
            child.Parent = this;
            if (InsertIndex >= 0)
                this._Children.Insert(InsertIndex, child);
            else
                this._Children.Add(child);
            this.OnContentsChanged();
            return child;
        }


        public void RemoveChild(Widget child)
        {
            this._Children.Remove(child);
            child.Parent = null;
            this.OnContentsChanged();
        }

        public void RemoveChildren()
        {
            this.RemoveChildren((Widget c) => true);
        }

        public void RemoveChildren(Predicate<Widget> shouldRemove)
        {
            foreach (Widget widget in this.Children.Where<Widget>((Func<Widget, bool>)(c => shouldRemove(c))))
                widget.Parent = (Widget)null;
            this._Children.RemoveAll(shouldRemove);
            this.OnContentsChanged();
        }

        protected virtual void OnContentsChanged()
        {
        }

        protected virtual void OnDimensionsChanged()
        {
        }

        public void CenterHorizontally()
        {
            int num = (this.Parent != null) ? this.Parent.Width : Game1.viewport.Width;
            this.X = num / 2 - this.Width / 2;
        }

        public void CenterVertically()
        {
            int num = (this.Parent != null) ? this.Parent.Height : Game1.viewport.Height;
            this.Y = num / 2 - this.Height / 2;
        }

    }
}
