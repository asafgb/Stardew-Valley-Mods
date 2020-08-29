using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace ChatCommands.ClassReplacements
{
    internal class CommandEmojiMenu : EmojiMenu
    {

        private readonly ClickableComponent bDownArrow;
        private readonly IReflectedMethod bDownArrowPressed;
        private readonly List<ClickableComponent> bEmojiSelectionButtons;
        private readonly IReflectedField<int> bPageStartIndex;
        private readonly ClickableComponent bSendArrow;
        private readonly ClickableComponent bUpArrow;
        private readonly IReflectedMethod bUpArrowPressed;

        public CommandEmojiMenu(IReflectionHelper reflection, ChatBox chatBox, Texture2D emojiTexture, Texture2D chatBoxTexture) : base(chatBox, emojiTexture, chatBoxTexture)
        {
            this.bUpArrow = reflection.GetField<ClickableComponent>(this, "upArrow", true).GetValue();
            this.bDownArrow = reflection.GetField<ClickableComponent>(this, "downArrow", true).GetValue();
            this.bSendArrow = reflection.GetField<ClickableComponent>(this, "sendArrow", true).GetValue();
            this.bEmojiSelectionButtons = reflection.GetField<List<ClickableComponent>>(this, "emojiSelectionButtons", true).GetValue();
            this.bPageStartIndex = reflection.GetField<int>(this, "pageStartIndex", true);
            this.bUpArrowPressed = reflection.GetMethod(this, "upArrowPressed", true);
            this.bDownArrowPressed = reflection.GetMethod(this, "downArrowPressed", true);
        }

        
        public void LeftClick(int x, int y, ChatBox cb)
        {
            bool flag = !this.isWithinBounds(x, y);
            if (!flag)
            {
                int x2 = x - this.xPositionOnScreen;
                int y2 = y - this.yPositionOnScreen;
                bool flag2 = this.bUpArrow.containsPoint(x2, y2);
                if (flag2)
                {
                    this.bUpArrowPressed.Invoke(new object[]
                    {
                        30
                    });
                }
                else
                {
                    bool flag3 = this.bDownArrow.containsPoint(x2, y2);
                    if (flag3)
                    {
                        this.bDownArrowPressed.Invoke(new object[]
                        {
                            30
                        });
                    }
                    else
                    {
                        bool flag4 = this.bSendArrow.containsPoint(x2, y2) && (double)cb.chatBox.currentWidth > 0.0;
                        if (flag4)
                        {
                            CommandChatBox commandChatBox = cb as CommandChatBox;
                            if (commandChatBox != null)
                            {
                                commandChatBox.EnterPressed(cb.chatBox);
                            }
                            this.bSendArrow.scale = 0.5f;
                            Game1.playSound("shwip");
                        }
                    }
                }
                foreach (ClickableComponent clickableComponent in this.bEmojiSelectionButtons)
                {
                    bool flag5 = !clickableComponent.containsPoint(x2, y2);
                    if (!flag5)
                    {
                        int emoji = this.bPageStartIndex.GetValue() + int.Parse(clickableComponent.name);
                        CommandChatTextBox commandChatTextBox = cb.chatBox as CommandChatTextBox;
                        if (commandChatTextBox != null)
                        {
                            commandChatTextBox.ReceiveEmoji(emoji);
                        }
                        Game1.playSound("coin");
                        break;
                    }
                }
            }
        }


    }
}
