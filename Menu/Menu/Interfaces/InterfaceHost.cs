using System;
using Menu.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Menu.Interfaces
{
    public abstract class InterfaceHost : IDisposable
    {
        //IModHelper helper;
        public virtual void Dispose()
        {

            Stat.helper.Events.Display.RenderedActiveMenu -= Display_RenderedActiveMenu;
            Stat.helper.Events.GameLoop.UpdateTicked -= GameLoop_UpdateTicked;
            Stat.helper.Events.Input.ButtonPressed -= Input_ButtonPressed;
            Stat.helper.Events.Input.CursorMoved -= Input_CursorMoved;
            Stat.helper.Events.Input.MouseWheelScrolled -= Input_MouseWheelScrolled;
        }

        protected InterfaceHost(Func<bool> keepAlive = null) // IModHelper helper, 
        {
            //this.helper = helper;
            this.KeepAliveCheck = keepAlive;
            this.LastViewport = new xTile.Dimensions.Rectangle(Game1.viewport.X, Game1.viewport.Y, Game1.viewport.Width, Game1.viewport.Height);

            Stat.helper.Events.Display.RenderedActiveMenu += Display_RenderedActiveMenu;
            Stat.helper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked;
            Stat.helper.Events.Input.ButtonPressed += Input_ButtonPressed;
            Stat.helper.Events.Input.CursorMoved += Input_CursorMoved;
            Stat.helper.Events.Input.MouseWheelScrolled += Input_MouseWheelScrolled;

        }

        private void Input_MouseWheelScrolled(object sender, MouseWheelScrolledEventArgs e)
        {
            bool flag3 = e.OldValue != e.NewValue && this.ReceiveScrollWheelAction(e.NewValue - e.OldValue);
            if (flag3)
            {
                MouseState oldMouseState = Game1.oldMouseState;
                Game1.oldMouseState = new MouseState(oldMouseState.X, oldMouseState.Y, e.NewValue, oldMouseState.LeftButton, oldMouseState.MiddleButton, oldMouseState.RightButton, oldMouseState.XButton1, oldMouseState.XButton2);
            }
        }

        private void Input_CursorMoved(object sender, CursorMovedEventArgs e)
        {
            Point newPosition = new Point((int)e.NewPosition.AbsolutePixels.X, (int)e.NewPosition.AbsolutePixels.Y);
            bool flag = this.ReceiveCursorHover(newPosition.X, newPosition.Y);
            if (flag)
            {
                MouseState oldMouseState = Game1.oldMouseState;
                Game1.oldMouseState = new MouseState(oldMouseState.X, oldMouseState.Y, oldMouseState.ScrollWheelValue, oldMouseState.LeftButton, oldMouseState.MiddleButton, oldMouseState.RightButton, oldMouseState.XButton1, oldMouseState.XButton2);
            }
        }

        private void Input_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) && !Game1.oldPadState.IsButtonDown(Buttons.X))
            {
                if (this.ReceiveLeftClick(Game1.getMouseX(), Game1.getMouseY()))
                {
                    Game1.oldPadState = GamePad.GetState(PlayerIndex.One);
                    return;
                }
            }
            bool flag2 = e.IsDown(SButton.MouseLeft) && this.ReceiveLeftClick((int)e.Cursor.ScreenPixels.X, (int)e.Cursor.ScreenPixels.Y);
            if (flag2)
            {
                MouseState oldMouseState = Game1.oldMouseState;

                Game1.oldMouseState = new MouseState(oldMouseState.X, oldMouseState.Y, oldMouseState.ScrollWheelValue, ButtonState.Pressed, oldMouseState.MiddleButton, oldMouseState.RightButton, oldMouseState.XButton1, oldMouseState.XButton2);
            }

        }

        private void GameLoop_UpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (this.KeepAliveCheck != null && !this.KeepAliveCheck())
            {
                this.Dispose();
                return;
            }
            xTile.Dimensions.Rectangle viewport = Game1.viewport;
            if (this.LastViewport.Width != viewport.Width || this.LastViewport.Height != viewport.Height)
            {
                viewport = new xTile.Dimensions.Rectangle(viewport.X, viewport.Y, viewport.Width, viewport.Height);
                this.ReceiveGameWindowResized(this.LastViewport, viewport);
                this.LastViewport = viewport;
            }
        }

        private void Display_RenderedActiveMenu(object sender, RenderedActiveMenuEventArgs e)
        {
            this.Draw(Game1.spriteBatch);
        }

        protected virtual void Draw(SpriteBatch batch)
        {
        }

        protected virtual bool ReceiveLeftClick(int x, int y)
        {
            return false;
        }

        protected virtual bool ReceiveKeyPress(Keys input)
        {
            return false;
        }

        protected virtual bool ReceiveButtonPress(Buttons input)
        {
            return false;
        }

        protected virtual bool ReceiveTriggerPress(Buttons input)
        {
            return false;
        }

        protected virtual bool ReceiveScrollWheelAction(int amount)
        {
            return false;
        }

        protected virtual bool ReceiveCursorHover(int x, int y)
        {
            return false;
        }

        protected virtual void ReceiveGameWindowResized(xTile.Dimensions.Rectangle oldBounds, xTile.Dimensions.Rectangle newBounds)
        {
        }

        protected void DrawCursor()
        {
            if (Game1.options.hardwareCursor)
            {
                return;
            }
            Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2((float)Game1.getOldMouseX(), (float)Game1.getOldMouseY()), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 0f);
        }

        private void OnPostRenderGuiEvent(object sender, EventArgs e)
        {

        }

        private void OnUpdateTick(object sender, EventArgs e)
        {

        }



        private xTile.Dimensions.Rectangle LastViewport;

        private readonly Func<bool> KeepAliveCheck;
    }
}
