using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ChatCommands.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
namespace ChatCommands.ClassReplacements
{
    internal class CommandChatBox : ChatBox
    {
        public CommandChatBox(IModHelper helper, ICommandHandler handler, ChatCommandsConfig config)
        {
            this.handler = handler;
            this.bCheatHistoryPosition = helper.Reflection.GetField<int>(this, "cheatHistoryPosition", true);
            this.bFormatMessage = helper.Reflection.GetMethod(this, "formatMessage", true);
            this.bMessages = helper.Reflection.GetField<List<ChatMessage>>(this, "messages", true).GetValue();
            this.bEmojiMenuIcon = helper.Reflection.GetField<ClickableTextureComponent>(this, "emojiMenuIcon", true).GetValue();
            this.bChoosingEmoji = helper.Reflection.GetField<bool>(this, "choosingEmoji", true);
            Texture2D texture2D = Game1.content.Load<Texture2D>("LooseSprites\\chatBox");
            this.chatBox.OnEnterPressed -= helper.Reflection.GetField<TextBoxEvent>(this, "e", true).GetValue();
            this.chatBox = (this.commandChatTextBox = new CommandChatTextBox(texture2D, null, Game1.smallFont, Color.White));
            Game1.keyboardDispatcher.Subscriber = this.chatBox;
            this.chatBox.Selected = false;
            this.chatBox.OnEnterPressed += this.EnterPressed;
            this.multiplayer = helper.Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer", true).GetValue();
            this.inputState = helper.Reflection.GetField<InputState>(typeof(Game1), "input", true).GetValue();
            ConsoleChatMessage.Init(LocalizedContentManager.CurrentLanguageCode);
            this.emojiMenu = new CommandEmojiMenu(helper.Reflection, this, ChatBox.emojiTexture, texture2D);
            helper.Reflection.GetMethod(this, "updatePosition", true).Invoke(new object[0]);
            this.displayLineIndex = -1;
            this.config = config;
            this.config.UseMonospacedFontForCommandOutput = (this.config.UseMonospacedFontForCommandOutput && (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.ja && LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.zh) && LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.th);
            this.DetermineNumberOfMaxMessages();
        }

        public void EnterPressed(TextBox sender)
        {
            ChatTextBox chatTextBox = sender as ChatTextBox;
            bool flag = chatTextBox != null;
            if (flag)
            {
                bool flag2 = chatTextBox.finalText.Count > 0;
                if (flag2)
                {
                    string text = ChatMessage.makeMessagePlaintext(chatTextBox.finalText, Utils.ShouldIncludeColorInfo(chatTextBox.finalText));
                    bool flag3 = text.Length < 1;
                    if (flag3)
                    {
                        this.textBoxEnter(sender);
                        this.commandChatTextBox.Reset();
                        this.bCheatHistoryPosition.SetValue(-1);
                        return;
                    }
                    this.sentMessageHistory.Insert(0, this.commandChatTextBox.Save());
                    bool flag4 = this.sentMessageHistory.Count >= this.config.MaximumNumberOfHistoryMessages;
                    if (flag4)
                    {
                        this.sentMessageHistory.RemoveAt(this.sentMessageHistory.Count - 1);
                    }
                    string input = CommandChatBox.FilterMessagePlaintext(text);
                    bool flag5 = text[0] == '/' && this.handler.CanHandle(input);
                    if (flag5)
                    {
                        this.receiveChatMessage(Game1.player.UniqueMultiplayerID, 0, LocalizedContentManager.CurrentLanguageCode, text);
                        this.handler.Handle(input);
                    }
                    else
                    {
                        bool flag6 = this.commandChatTextBox.CurrentRecipientId != -1L;
                        if (flag6)
                        {
                            long key = Game1.player.UniqueMultiplayerID ^ this.commandChatTextBox.CurrentRecipientId;
                            string text2 = string.Format("{0}", this.commandChatTextBox.CurrentRecipientId);
                            Farmer farmer2 = Game1.getOnlineFarmers().FirstOrDefault((Farmer farmer) => farmer.UniqueMultiplayerID == this.commandChatTextBox.CurrentRecipientId);
                            string text3 = (farmer2 != null) ? farmer2.Name : null;
                            bool flag7 = text3 == null;
                            if (flag7)
                            {
                                this.addMessage(this.commandChatTextBox.CurrentRecipientName + " is offline now.", Color.Red);
                            }
                            else
                            {
                                this.receiveChatMessage(Game1.player.UniqueMultiplayerID, 0, LocalizedContentManager.CurrentLanguageCode, string.Format("{0}{1}{2}{3}", new object[]
                                {
                                    'û',
                                    text3,
                                    'ú',
                                    text
                                }));
                                this.multiplayer.sendChatMessage(LocalizedContentManager.CurrentLanguageCode, string.Format("{0}{1}{2}{3}", new object[]
                                {
                                    'ú',
                                    Utils.EncipherText(text2, key),
                                    'ú',
                                    Utils.EncipherText(text, key)
                                }), this.commandChatTextBox.CurrentRecipientId);
                            }
                        }
                        else
                        {
                            Match match;
                            bool success = (match = CommandChatTextBox.WhisperRegex.Match(input)).Success;
                            if (success)
                            {
                                string text4 = null;
                                bool flag8 = !Context.IsMultiplayer;
                                if (flag8)
                                {
                                    text4 = "You can't send whispers in singleplayer.";
                                }
                                bool flag9 = text4 == null;
                                if (flag9)
                                {
                                    text4 = ((match.Groups[1].Value == Game1.player.Name) ? "You can't whisper to yourself." : ("There isn't anyone named " + match.Groups[1].Value + " online."));
                                }
                                this.addMessage(text4, Color.Red);
                            }
                            else
                            {
                                bool success2 = CommandChatTextBox.WhisperReplyRegex.Match(input).Success;
                                if (!success2)
                                {
                                    this.textBoxEnter(sender);
                                    this.commandChatTextBox.Reset();
                                    this.bCheatHistoryPosition.SetValue(-1);
                                    return;
                                }
                                this.addMessage((!Context.IsMultiplayer) ? "You can't reply to whispers in singleplayer." : "You can't reply when you haven't received any whispers.", Color.Red);
                            }
                        }
                    }
                }
                this.commandChatTextBox.Reset();
                this.bCheatHistoryPosition.SetValue(-1);
            }
            sender.Text = "";
            this.clickAway();
        }

        public override void clickAway()
        {
            bool flag = this.inputState.GetKeyboardState().IsKeyDown(Keys.Escape) && (this.isEscapeDown || !this.sawChangeToTrue);
            if (flag)
            {
                bool flag2 = this.ignoreClickAway || !this.sawChangeToTrue;
                if (flag2)
                {
                    return;
                }
                this.ignoreClickAway = true;
            }
            this.DeactivateLayer();
        }

        public void EscapeStatusChanged(bool isDown)
        {
            bool flag = !this.isEscapeDown && isDown;
            if (flag)
            {
                this.sawChangeToTrue = true;
            }
            else
            {
                this.sawChangeToTrue = false;
            }
            this.isEscapeDown = isDown;
            bool flag2 = !isDown;
            if (flag2)
            {
                this.ignoreClickAway = false;
            }
        }

        private void DeactivateLayer()
        {
            bool value = this.bChoosingEmoji.GetValue();
            if (value)
            {
                this.bChoosingEmoji.SetValue(false);
            }
            else
            {
                bool flag = this.commandChatTextBox.CurrentRecipientId != -1L;
                if (flag)
                {
                    this.commandChatTextBox.UpdateForNewRecepient(-1L, null);
                }
                else
                {
                    bool flag2 = this.commandChatTextBox.finalText.Any<ChatSnippet>();
                    if (flag2)
                    {
                        this.commandChatTextBox.Reset();
                        while (this.bCheatHistoryPosition.GetValue() != -1)
                        {
                            this.receiveKeyPress(Keys.Down);
                        }
                    }
                    else
                    {
                        this.Reset();
                    }
                }
            }
        }

        private void Reset()
        {
            int value = this.bCheatHistoryPosition.GetValue();
            this.bCheatHistoryPosition.SetValue(-5);
            base.clickAway();
            bool flag = this.bCheatHistoryPosition.GetValue() != -5;
            if (flag)
            {
                this.displayLineIndex = this.bMessages.Count - 1;
                this.commandChatTextBox.Reset();
            }
            else
            {
                this.bCheatHistoryPosition.SetValue(value);
            }
        }

        public override bool isWithinBounds(int x, int y)
        {
            bool flag = x - this.xPositionOnScreen < this.width && x - this.xPositionOnScreen >= 0 && y - this.yPositionOnScreen < this.height && y - this.yPositionOnScreen >= -this.GetOldMessagesBoxHeight();
            return flag || (this.bChoosingEmoji.GetValue() && this.emojiMenu.isWithinBounds(x, y));
        }

        private int GetOldMessagesBoxHeight()
        {
            return (from item in this.GetDisplayedLines()
                    select item.verticalSize).Sum() + 20;
        }

        public override void receiveChatMessage(long sourceFarmer, int chatKind, LocalizedContentManager.LanguageCode language, string message)
        {
            bool flag = false;
            string text = "you";
            bool flag2 = message[0] == 'ú';
            if (flag2)
            {
                string[] array = message.Substring(1).Split(new char[]
                {
                    'ú'
                });
                string s = Utils.DecipherText(array[0], sourceFarmer ^ Game1.player.UniqueMultiplayerID);
                long num;
                bool flag3 = !long.TryParse(s, out num) || num != Game1.player.UniqueMultiplayerID;
                if (flag3)
                {
                    return;
                }
                message = Utils.DecipherText(array[1], sourceFarmer ^ Game1.player.UniqueMultiplayerID);
                flag = true;
                this.commandChatTextBox.LastWhisperId = sourceFarmer;
            }
            else
            {
                bool flag4 = message[0] == 'û';
                if (flag4)
                {
                    string[] array2 = message.Substring(1).Split(new char[]
                    {
                        'ú'
                    });
                    text = array2[0];
                    message = array2[1];
                    flag = true;
                }
            }
            string text2 = this.bFormatMessage.Invoke<string>(new object[]
            {
                sourceFarmer,
                chatKind,
                message
            });
            bool flag5 = flag;
            if (flag5)
            {
                text2 = string.Concat(new string[]
                {
                    text2.Substring(0, text2.IndexOf(":", StringComparison.InvariantCultureIgnoreCase)),
                    " (to ",
                    text,
                    " only):",
                    text2.Substring(text2.IndexOf(":", StringComparison.InvariantCultureIgnoreCase) + 1)
                });
            }
            string text3 = CommandChatBox.FixedParseText(text2, this.chatBox.Font, this.chatBox.Width - 16, false);
            foreach (string message2 in text3.Split(new string[]
            {
                "\r\n",
                "\n"
            }, StringSplitOptions.None))
            {
                this.AddNewMessage(message2, this.messageColor(chatKind), this.chatBox.Font, language, false);
            }
        }

        public void AddConsoleMessage(string message, Color color)
        {
            string text = CommandChatBox.FixedParseText(message, this.chatBox.Font, this.chatBox.Width - 8, this.config.UseMonospacedFontForCommandOutput);
            foreach (string message2 in text.Split(new string[]
            {
                "\r\n",
                "\n"
            }, StringSplitOptions.None))
            {
                this.AddNewMessage(message2, color, this.chatBox.Font, LocalizedContentManager.CurrentLanguageCode, true);
            }
        }

        public override void addMessage(string message, Color color)
        {
            string text = CommandChatBox.FixedParseText(message, this.chatBox.Font, this.chatBox.Width - 8, false);
            foreach (string message2 in text.Split(new string[]
            {
                "\r\n",
                "\n"
            }, StringSplitOptions.None))
            {
                this.AddNewMessage(message2, color, this.chatBox.Font, LocalizedContentManager.CurrentLanguageCode, false);
            }
        }

        private void AddNewMessage(string message, Color color, SpriteFont font, LocalizedContentManager.LanguageCode code, bool isConsoleMessage = false)
        {
            bool flag = string.IsNullOrEmpty(message);
            if (flag)
            {
                message = " ";
            }
            ChatMessage chatMessage = (isConsoleMessage && this.config.UseMonospacedFontForCommandOutput) ? new ConsoleChatMessage() : new ChatMessage();
            chatMessage.timeLeftToDisplay = 600;
            chatMessage.verticalSize = (int)font.MeasureString(message).Y + 4;
            chatMessage.color = color;
            chatMessage.language = code;
            chatMessage.parseMessageForEmoji(message);
            this.bMessages.Add(chatMessage);
            bool flag2 = this.config.MaximumNumberOfHistoryMessages > 0 && this.bMessages.Count >= this.config.MaximumNumberOfHistoryMessages;
            if (flag2)
            {
                this.bMessages.RemoveAt(0);
            }
            else
            {
                bool flag3 = this.displayLineIndex == this.bMessages.Count - 2;
                if (flag3)
                {
                    this.displayLineIndex++;
                }
            }
        }

        private static string FilterMessagePlaintext(string input)
        {
            bool flag = Game1.player.defaultChatColor == null || ChatMessage.getColorFromName(Game1.player.defaultChatColor).Equals(Color.White);
            string result;
            if (flag)
            {
                result = input;
            }
            else
            {
                result = (input.EndsWith(" [" + Game1.player.defaultChatColor + "]") ? input.Substring(0, input.LastIndexOf(" [" + Game1.player.defaultChatColor + "]", StringComparison.InvariantCultureIgnoreCase)) : input);
            }
            return result;
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);
            this.DetermineNumberOfMaxMessages();
        }

        private void DetermineNumberOfMaxMessages()
        {
            this.maxMessages = 10;
            float num = ChatBox.messageFont(LocalizedContentManager.CurrentLanguageCode).MeasureString("(").Y + 4f;
            for (float num2 = (float)this.yPositionOnScreen - num * (float)this.maxMessages; num2 >= 128f; num2 -= num)
            {
                this.maxMessages++;
            }
            this.maxMessages--;
        }

        protected override void runCommand(string command)
        {
            base.runCommand(command);
            bool flag = this.bMessages.Count <= this.displayLineIndex;
            if (flag)
            {
                this.displayLineIndex = this.bMessages.Count - 1;
            }
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            bool flag = !this.chatBox.Selected;
            if (!flag)
            {
                bool flag2 = this.bEmojiMenuIcon.containsPoint(x, y);
                if (flag2)
                {
                    this.bChoosingEmoji.SetValue(!this.bChoosingEmoji.GetValue());
                    Game1.playSound("shwip");
                    this.bEmojiMenuIcon.scale = 4f;
                }
                else
                {
                    bool flag3 = this.bChoosingEmoji.GetValue() && this.emojiMenu.isWithinBounds(x, y);
                    if (flag3)
                    {
                        CommandEmojiMenu commandEmojiMenu = this.emojiMenu as CommandEmojiMenu;
                        if (commandEmojiMenu != null)
                        {
                            commandEmojiMenu.LeftClick(x, y, this);
                        }
                    }
                    else
                    {
                        this.chatBox.Update();
                        bool value = this.bChoosingEmoji.GetValue();
                        if (value)
                        {
                            this.bChoosingEmoji.SetValue(false);
                            this.bEmojiMenuIcon.scale = 4f;
                        }
                        bool flag4 = !this.isWithinBounds(x, y);
                        if (!flag4)
                        {
                            this.chatBox.Selected = true;
                        }
                    }
                }
            }
        }

        public override void receiveKeyPress(Keys key)
        {
            bool flag = !base.isActive();
            if (!flag)
            {
                bool flag2 = key == Keys.Up;
                if (flag2)
                {
                    bool flag3 = this.bCheatHistoryPosition.GetValue() >= this.sentMessageHistory.Count - 1;
                    if (!flag3)
                    {
                        bool flag4 = this.bCheatHistoryPosition.GetValue() == -1;
                        if (flag4)
                        {
                            this.currentTypedMessage = this.commandChatTextBox.Save();
                        }
                        this.bCheatHistoryPosition.SetValue(this.bCheatHistoryPosition.GetValue() + 1);
                        this.commandChatTextBox.Load(this.sentMessageHistory[this.bCheatHistoryPosition.GetValue()], false);
                    }
                }
                else
                {
                    bool flag5 = key == Keys.Down;
                    if (flag5)
                    {
                        bool flag6 = this.bCheatHistoryPosition.GetValue() <= 0;
                        if (flag6)
                        {
                            bool flag7 = this.bCheatHistoryPosition.GetValue() == -1;
                            if (!flag7)
                            {
                                this.bCheatHistoryPosition.SetValue(-1);
                                int num = this.displayLineIndex;
                                this.clickAway();
                                base.activate();
                                this.displayLineIndex = num;
                                this.commandChatTextBox.Load(this.currentTypedMessage, true);
                            }
                        }
                        else
                        {
                            this.bCheatHistoryPosition.SetValue(this.bCheatHistoryPosition.GetValue() - 1);
                            this.commandChatTextBox.Load(this.sentMessageHistory[this.bCheatHistoryPosition.GetValue()], false);
                        }
                    }
                    else
                    {
                        bool flag8 = key == Keys.Left;
                        if (flag8)
                        {
                            this.commandChatTextBox.OnLeftArrowPress();
                        }
                        else
                        {
                            bool flag9 = key == Keys.Right;
                            if (flag9)
                            {
                                this.commandChatTextBox.OnRightArrowPress();
                            }
                        }
                    }
                }
            }
        }

        private void ScrollDown()
        {
            bool flag = this.displayLineIndex >= this.bMessages.Count - 1;
            if (!flag)
            {
                this.displayLineIndex++;
            }
        }

        private void ScrollUp()
        {
            bool flag = this.displayLineIndex <= Math.Min(this.maxMessages, this.bMessages.Count) - 1;
            if (!flag)
            {
                this.displayLineIndex--;
            }
        }

        public override void receiveScrollWheelAction(int direction)
        {
            bool flag = !this.bChoosingEmoji.GetValue();
            if (flag)
            {
                bool flag2 = direction < 0;
                if (flag2)
                {
                    this.ScrollDown();
                }
                else
                {
                    this.ScrollUp();
                }
            }
            else
            {
                this.emojiMenu.receiveScrollWheelAction(direction);
            }
        }

        public override void draw(SpriteBatch b)
        {
            bool flag = this.GetDisplayedLines().Any<ChatMessage>();
            if (flag)
            {
                int num = this.GetOldMessagesBoxHeight() - 20;
                IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(301, 288, 15, 15), this.xPositionOnScreen, this.yPositionOnScreen - num - 20 + (this.chatBox.Selected ? 0 : this.chatBox.Height), this.chatBox.Width, num + 20, Color.White, 4f, false);
                bool flag2 = this.bMessages.Count > this.maxMessages && base.isActive();
                if (flag2)
                {
                    int num2 = num - 51;
                    int num3 = this.bMessages.Count - this.maxMessages + 1;
                    int height = num2 / num3;
                    int num4 = 14 + num2 - (this.bMessages.Count - this.displayLineIndex) * num2 / num3;
                    int num5 = -13 + num - (this.bMessages.Count - this.maxMessages + 1) * num / num3;
                    int num6 = -13 + num - (this.bMessages.Count - this.bMessages.Count) * num / num3;
                    b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + this.width - 32 - 3), (float)(this.yPositionOnScreen - num + (this.chatBox.Selected ? 0 : this.chatBox.Height) + num6 - 24)), new Rectangle?(new Rectangle(421, 472, 11, 12)), Color.White, 0f, Vector2.Zero, Vector2.One * 2f, SpriteEffects.None, 0f);
                    b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + this.width - 32 - 3), (float)(this.yPositionOnScreen - num + (this.chatBox.Selected ? 0 : this.chatBox.Height) + num5 + 5)), new Rectangle?(new Rectangle(421, 459, 11, 12)), Color.White, 0f, Vector2.Zero, Vector2.One * 2f, SpriteEffects.None, 0f);
                    IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(325, 448, 5, 17), this.xPositionOnScreen + this.width - 32, this.yPositionOnScreen - num + (this.chatBox.Selected ? 0 : this.chatBox.Height) + num4, 16, height, Color.White, 4f, false);
                }
            }
            int num7 = 0;
            foreach (ChatMessage chatMessage in this.GetDisplayedLines())
            {
                num7 += chatMessage.verticalSize;
                ConsoleChatMessage consoleChatMessage = chatMessage as ConsoleChatMessage;
                bool flag3 = consoleChatMessage != null;
                if (flag3)
                {
                    consoleChatMessage.ConsoleDraw(b, 12, this.yPositionOnScreen - num7 - 8 + (this.chatBox.Selected ? 0 : this.chatBox.Height));
                }
                else
                {
                    chatMessage.draw(b, 12, this.yPositionOnScreen - num7 - 8 + (this.chatBox.Selected ? 0 : this.chatBox.Height));
                }
            }
            bool flag4 = !base.isActive();
            if (!flag4)
            {
                this.chatBox.Draw(b, false);
                bool flag5 = this.bCheatHistoryPosition.GetValue() != -1;
                if (flag5)
                {
                    float num8 = (float)this.chatBox.X;
                    b.DrawString(Game1.tinyFont, "#", new Vector2(num8, (float)this.chatBox.Y), Color.White, 0f, Vector2.Zero, Vector2.One * 0.5f, SpriteEffects.None, 0f);
                    num8 += Game1.tinyFont.MeasureString("#").X * 0.5f;
                    b.DrawString(Game1.tinyFont, string.Format("{0}", this.bCheatHistoryPosition.GetValue() + 1), new Vector2(num8, (float)(this.chatBox.Y - 12)), Color.White);
                    num8 += Game1.tinyFont.MeasureString(string.Format("{0}", this.bCheatHistoryPosition.GetValue() + 1)).X;
                    b.DrawString(Game1.tinyFont, "/", new Vector2(num8, (float)this.chatBox.Y), Color.White, 0f, Vector2.Zero, Vector2.One * 0.5f, SpriteEffects.None, 0f);
                    num8 += Game1.tinyFont.MeasureString("/").X * 0.5f;
                    b.DrawString(Game1.tinyFont, string.Format("{0}", this.sentMessageHistory.Count), new Vector2(num8, (float)(this.chatBox.Y - 12)), Color.White);
                }
                this.bEmojiMenuIcon.draw(b, Color.White, 0.99f, 0);
                bool flag6 = !this.bChoosingEmoji.GetValue();
                if (!flag6)
                {
                    this.emojiMenu.draw(b);
                }
            }
        }

        internal void ClearHistory()
        {
            this.sentMessageHistory.Clear();
            this.bCheatHistoryPosition.SetValue(-1);
            bool flag = this.currentTypedMessage == null;
            if (flag)
            {
                this.currentTypedMessage = this.commandChatTextBox.Save();
            }
            this.commandChatTextBox.Load(this.currentTypedMessage, true);
        }

        private IEnumerable<ChatMessage> GetDisplayedLines()
        {
            int num;
            for (int i = this.displayLineIndex; i >= this.GetEndDisplayIndex(); i = num - 1)
            {
                ChatMessage message = this.bMessages[i];
                bool flag = this.chatBox.Selected || (double)message.alpha > 0.00999999977648258;
                if (flag)
                {
                    yield return message;
                }
                message = null;
                num = i;
            }
            yield break;
        }

        private int GetEndDisplayIndex()
        {
            bool flag = this.displayLineIndex < this.maxMessages;
            int result;
            if (flag)
            {
                result = 0;
            }
            else
            {
                result = this.displayLineIndex - this.maxMessages + 1;
            }
            return result;
        }

        private static string FixedParseText(string text, SpriteFont whichFont, int width, bool isConsole = false)
        {
            bool flag = text == null;
            string result;
            if (flag)
            {
                result = "";
            }
            else
            {
                string text2 = string.Empty;
                string str = string.Empty;
                bool flag2 = LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ja || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.th;
                if (flag2)
                {
                    foreach (char c in text)
                    {
                        bool flag3 = (double)ConsoleChatMessage.MeasureStringWidth(whichFont, text2 + c.ToString(), isConsole) > (double)width;
                        if (flag3)
                        {
                            str = str + text2 + Environment.NewLine;
                            text2 = string.Empty;
                        }
                        text2 += c.ToString();
                    }
                    result = str + text2;
                }
                else
                {
                    char[] separator = new char[]
                    {
                        ' '
                    };
                    foreach (string text3 in text.Split(separator))
                    {
                        try
                        {
                            bool flag4 = (double)ConsoleChatMessage.MeasureStringWidth(whichFont, text2 + text3, isConsole) > (double)width || text3.Equals(Environment.NewLine);
                            if (flag4)
                            {
                                str = str + text2 + Environment.NewLine;
                                text2 = string.Empty;
                            }
                            bool flag5 = !text3.Equals(Environment.NewLine);
                            if (flag5)
                            {
                                text2 = text2 + text3 + " ";
                            }
                        }
                        catch (Exception ex)
                        {
                            string str2 = "Exception measuring string: ";
                            Exception ex2 = ex;
                            Console.WriteLine(str2 + ((ex2 != null) ? ex2.ToString() : null));
                        }
                    }
                    result = str + text2;
                }
            }
            return result;
        }

        internal const char WhisperSeparator = 'ú';

        private readonly IReflectedField<int> bCheatHistoryPosition;

        private readonly IReflectedField<bool> bChoosingEmoji;

        private readonly ClickableTextureComponent bEmojiMenuIcon;

        private readonly IReflectedMethod bFormatMessage;

        private readonly List<ChatMessage> bMessages;

        private readonly CommandChatTextBox commandChatTextBox;

        private readonly ChatCommandsConfig config;

        private readonly ICommandHandler handler;

        private readonly InputState inputState;

        private readonly Multiplayer multiplayer;

        private readonly List<CommandChatTextBoxState> sentMessageHistory = new List<CommandChatTextBoxState>();

        private CommandChatTextBoxState currentTypedMessage;

        private int displayLineIndex;

        private bool ignoreClickAway;

        private bool isEscapeDown;

        private bool sawChangeToTrue;
    }
}

