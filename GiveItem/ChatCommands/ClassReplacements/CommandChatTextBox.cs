using System;
using System.Linq;
using System.Text.RegularExpressions;
using ChatCommands.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace ChatCommands.ClassReplacements
{
    internal class CommandChatTextBox : ChatTextBox
    {

        internal static readonly Regex WhisperRegex = new Regex("^/w (\\w+) ", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        internal static readonly Regex WhisperReplyRegex = new Regex("^/r ", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private int currentInsertPosition;
        private int currentSnippetIndex;
        private string displayTo;
        private int lastUpdatecurrentSnippetIndex;
        private int lastUpdateInsertPosition;
        internal long LastWhisperId = -1L;
        private int toOffset;


        public CommandChatTextBox(Texture2D textBoxTexture, Texture2D caretTexture, SpriteFont font, Color textColor) : base(textBoxTexture, caretTexture, font, textColor)
        {
            this.UpdateForNewRecepient(this.CurrentRecipientId, null);
        }

        internal long CurrentRecipientId { get; private set; } = -1L;
        
        internal string CurrentRecipientName { get; private set; }
        
        public void Reset()
        {
            this.currentWidth = 0f;
            this.finalText.Clear();
            this.UpdateForNewRecepient(-1L, null);
            this.currentSnippetIndex = (this.currentInsertPosition = 0);
        }
        public override void RecieveCommandInput(char command)
        {
            if (command == '\b')
            {
                if (base.Selected)
                {
                    this.Backspace();
                    return;
                }
            }
            base.RecieveCommandInput(command);
        }
        public override void RecieveTextInput(string text)
        {
            bool flag = string.IsNullOrEmpty(text);
            if (!flag)
            {
                bool flag2 = this.finalText.Count == 0;
                if (flag2)
                {
                    this.finalText.Add(new ChatSnippet("", LocalizedContentManager.CurrentLanguageCode));
                }
                bool flag3 = (double)this.currentWidth + (double)ChatBox.messageFont(LocalizedContentManager.CurrentLanguageCode).MeasureString(text).X >= (double)(base.Width - 16);
                if (!flag3)
                {
                    bool flag4 = this.finalText[this.currentSnippetIndex].message == null;
                    if (flag4)
                    {
                        bool flag5 = this.currentInsertPosition == 0;
                        if (flag5)
                        {
                            this.finalText.Insert(this.currentSnippetIndex, new ChatSnippet(text, LocalizedContentManager.CurrentLanguageCode));
                        }
                        else
                        {
                            this.finalText.Insert(this.currentSnippetIndex + 1, new ChatSnippet(text, LocalizedContentManager.CurrentLanguageCode));
                            this.currentSnippetIndex++;
                        }
                        this.currentInsertPosition = this.GetLastIndexOfCurrentSnippet();
                    }
                    else
                    {
                        ChatSnippet chatSnippet = this.finalText[this.currentSnippetIndex];
                        chatSnippet.message = chatSnippet.message.Substring(0, this.currentInsertPosition) + text + chatSnippet.message.Substring(this.currentInsertPosition);
                        this.currentInsertPosition += text.Length;
                        CommandChatTextBox.ReMeasureSnippetLength(chatSnippet);
                    }
                    bool flag6 = !this.CheckForWhisper();
                    if (flag6)
                    {
                        this.CheckForWhisperReply();
                    }
                    base.updateWidth();
                }
            }
        }
        private bool CheckForWhisper()
        {
            Match match = CommandChatTextBox.WhisperRegex.Match(ChatMessage.makeMessagePlaintext(this.finalText, Utils.ShouldIncludeColorInfo(this.finalText)));
            bool flag = !match.Success;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                long id;
                bool flag2 = this.TryGetUniqueIdFromName(match.Groups[1].Value, out id);
                if (flag2)
                {
                    bool flag3 = this.UpdateForNewRecepient(id, null);
                    if (flag3)
                    {
                        this.finalText[0].message = this.finalText[0].message.Substring(match.Groups[0].Value.Length);
                        this.currentInsertPosition = this.GetLastIndexOfCurrentSnippet();
                        return true;
                    }
                }
                result = false;
            }
            return result;
        }

        
        private void CheckForWhisperReply()
        {
            Match match = CommandChatTextBox.WhisperReplyRegex.Match(ChatMessage.makeMessagePlaintext(this.finalText, Utils.ShouldIncludeColorInfo(this.finalText)));
            bool flag = !match.Success || this.LastWhisperId == -1L;
            if (!flag)
            {
                this.UpdateForNewRecepient(this.LastWhisperId, null);
                this.finalText[0].message = this.finalText[0].message.Substring(match.Groups[0].Value.Length);
                this.currentInsertPosition = this.GetLastIndexOfCurrentSnippet();
            }
        }

        
        public void Backspace()
        {
            bool flag = this.finalText.Any<ChatSnippet>();
            if (flag)
            {
                bool flag2 = this.currentInsertPosition == 0;
                if (flag2)
                {
                    bool flag3 = this.currentSnippetIndex == 0;
                    if (flag3)
                    {
                        return;
                    }
                    bool flag4 = this.finalText[this.currentSnippetIndex].message == null;
                    if (flag4)
                    {
                        ChatSnippet chatSnippet = this.finalText[this.currentSnippetIndex - 1];
                        bool flag5 = chatSnippet.message == null;
                        if (flag5)
                        {
                            this.finalText.RemoveAt(this.currentSnippetIndex - 1);
                            this.currentSnippetIndex--;
                        }
                        else
                        {
                            chatSnippet.message = chatSnippet.message.Substring(0, chatSnippet.message.Length - 1);
                            bool flag6 = chatSnippet.message.Length == 0;
                            if (flag6)
                            {
                                this.finalText.Remove(chatSnippet);
                                this.currentSnippetIndex--;
                            }
                            else
                            {
                                CommandChatTextBox.ReMeasureSnippetLength(chatSnippet);
                            }
                        }
                    }
                    else
                    {
                        this.finalText.RemoveAt(this.currentSnippetIndex - 1);
                        this.currentSnippetIndex--;
                        bool flag7 = this.currentSnippetIndex == 0;
                        if (flag7)
                        {
                            return;
                        }
                        bool flag8 = this.finalText[this.currentSnippetIndex - 1].message == null;
                        if (flag8)
                        {
                            return;
                        }
                        ChatSnippet chatSnippet2 = this.finalText[this.currentSnippetIndex - 1];
                        this.currentInsertPosition = chatSnippet2.message.Length;
                        ChatSnippet chatSnippet3 = chatSnippet2;
                        chatSnippet3.message += this.finalText[this.currentSnippetIndex].message;
                        CommandChatTextBox.ReMeasureSnippetLength(chatSnippet2);
                        this.finalText.RemoveAt(this.currentSnippetIndex);
                        this.currentSnippetIndex--;
                    }
                }
                else
                {
                    bool flag9 = this.finalText[this.currentSnippetIndex].message == null;
                    if (flag9)
                    {
                        this.finalText.RemoveAt(this.currentSnippetIndex);
                        bool flag10 = this.currentSnippetIndex != 0;
                        if (flag10)
                        {
                            this.currentSnippetIndex--;
                            this.currentInsertPosition = this.GetLastIndexOfCurrentSnippet();
                            bool flag11 = this.currentSnippetIndex != this.finalText.Count - 1 && this.finalText[this.currentSnippetIndex + 1].message != null;
                            if (flag11)
                            {
                                ChatSnippet chatSnippet4 = this.finalText[this.currentSnippetIndex + 1];
                                ChatSnippet chatSnippet5 = this.finalText[this.currentSnippetIndex];
                                this.currentInsertPosition = chatSnippet5.message.Length;
                                ChatSnippet chatSnippet6 = chatSnippet5;
                                chatSnippet6.message += chatSnippet4.message;
                                CommandChatTextBox.ReMeasureSnippetLength(chatSnippet5);
                                this.finalText.Remove(chatSnippet4);
                            }
                        }
                        else
                        {
                            this.currentInsertPosition = 0;
                        }
                    }
                    else
                    {
                        ChatSnippet chatSnippet7 = this.finalText[this.currentSnippetIndex];
                        chatSnippet7.message = chatSnippet7.message.Remove(this.currentInsertPosition - 1, 1);
                        bool flag12 = chatSnippet7.message.Length == 0;
                        if (flag12)
                        {
                            this.finalText.Remove(chatSnippet7);
                            bool flag13 = this.currentSnippetIndex != 0;
                            if (flag13)
                            {
                                this.currentSnippetIndex--;
                                this.currentInsertPosition = this.GetLastIndexOfCurrentSnippet();
                            }
                            else
                            {
                                this.currentInsertPosition = 0;
                            }
                        }
                        else
                        {
                            CommandChatTextBox.ReMeasureSnippetLength(chatSnippet7);
                            this.currentInsertPosition--;
                        }
                    }
                }
            }
            base.updateWidth();
        }

        
        private static void ReMeasureSnippetLength(ChatSnippet snippet)
        {
            bool flag = snippet.message == null;
            if (flag)
            {
                snippet.myLength = 40f;
            }
            else
            {
                snippet.myLength = ChatBox.messageFont(LocalizedContentManager.CurrentLanguageCode).MeasureString(snippet.message).X;
            }
        }

        
        private static int GetLastIndexOfMessage(ChatSnippet snippet)
        {
            string message = snippet.message;
            return (message != null) ? message.Length : 1;
        }

        
        private int GetLastIndexOfCurrentSnippet()
        {
            return CommandChatTextBox.GetLastIndexOfMessage(this.finalText[this.currentSnippetIndex]);
        }

        
        public void ReceiveEmoji(int emoji)
        {
            bool flag = (double)this.currentWidth + 40.0 > (double)(base.Width - 16);
            if (!flag)
            {
                bool flag2 = this.currentInsertPosition == 0;
                if (flag2)
                {
                    this.finalText.Insert(this.currentSnippetIndex, new ChatSnippet(emoji));
                    bool flag3 = this.finalText.Count != 1;
                    if (flag3)
                    {
                        this.currentSnippetIndex++;
                    }
                    else
                    {
                        this.currentInsertPosition = this.GetLastIndexOfCurrentSnippet();
                    }
                }
                else
                {
                    bool flag4 = this.currentInsertPosition == this.GetLastIndexOfCurrentSnippet();
                    if (flag4)
                    {
                        ChatSnippet chatSnippet = new ChatSnippet(emoji);
                        this.finalText.Insert(this.currentSnippetIndex + 1, chatSnippet);
                        this.currentSnippetIndex++;
                        this.currentInsertPosition = CommandChatTextBox.GetLastIndexOfMessage(chatSnippet);
                    }
                    else
                    {
                        ChatSnippet chatSnippet2 = this.finalText[this.currentSnippetIndex];
                        string message = chatSnippet2.message.Substring(0, this.currentInsertPosition);
                        string message2 = chatSnippet2.message.Substring(this.currentInsertPosition);
                        this.finalText.RemoveAt(this.currentSnippetIndex);
                        this.finalText.Insert(this.currentSnippetIndex, new ChatSnippet(message2, LocalizedContentManager.CurrentLanguageCode));
                        this.finalText.Insert(this.currentSnippetIndex, new ChatSnippet(emoji));
                        this.finalText.Insert(this.currentSnippetIndex, new ChatSnippet(message, LocalizedContentManager.CurrentLanguageCode));
                        this.currentSnippetIndex += 2;
                        this.currentInsertPosition = 0;
                    }
                }
                base.updateWidth();
            }
        }

        
        public override void Draw(SpriteBatch spriteBatch, bool drawShadow = true)
        {
            bool selected = base.Selected;
            if (selected)
            {
                base.Selected = false;
                this.DrawAtOffset(this.toOffset, spriteBatch, drawShadow);
                base.Selected = true;
                bool flag = this.lastUpdateInsertPosition != this.currentInsertPosition || this.lastUpdatecurrentSnippetIndex != this.currentSnippetIndex || DateTime.Now.Millisecond % 1000 >= 500;
                if (flag)
                {
                    spriteBatch.Draw(Game1.staminaRect, new Rectangle(base.X + this.toOffset + 16 + (int)this.GetCursorOffset() - 3, base.Y + 8, 4, 32), this._textColor);
                }
                this.lastUpdatecurrentSnippetIndex = this.currentSnippetIndex;
                this.lastUpdateInsertPosition = this.currentInsertPosition;
            }
            else
            {
                this.DrawAtOffset(this.toOffset, spriteBatch, drawShadow);
            }
            bool flag2 = this.toOffset != 0;
            if (flag2)
            {
                spriteBatch.Draw(this._textBoxTexture, new Rectangle(base.X, base.Y, 16, base.Height), new Rectangle?(new Rectangle(0, 0, 16, base.Height)), Color.White);
                spriteBatch.Draw(this._textBoxTexture, new Rectangle(base.X + 16, base.Y, this.toOffset - 32, base.Height), new Rectangle?(new Rectangle(16, 0, 4, base.Height)), Color.White);
                spriteBatch.Draw(this._textBoxTexture, new Rectangle(base.X + this.toOffset - 16, base.Y, 16, base.Height), new Rectangle?(new Rectangle(this._textBoxTexture.Bounds.Width - 16, 0, 16, base.Height)), Color.White);
                spriteBatch.DrawString(ChatBox.messageFont(LocalizedContentManager.CurrentLanguageCode), this.displayTo, new Vector2((float)(base.X + 12), (float)(base.Y + 14)), Color.White);
            }
        }

        
        private void DrawAtOffset(int offset, SpriteBatch spriteBatch, bool drawShadow = true)
        {
            base.X += offset;
            base.Width -= offset;
            base.Draw(spriteBatch, drawShadow);
            base.X -= offset;
            base.Width += offset;
        }

        
        public bool UpdateForNewRecepient(long id, string to = null)
        {
            bool flag = !Context.IsMultiplayer;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                bool flag2 = to == null;
                if (flag2)
                {
                    to = this.GetOtherPlayerNameFromUniqueId(id);
                }
                bool flag3 = to == null;
                if (flag3)
                {
                    this.toOffset = 0;
                    this.CurrentRecipientId = -1L;
                    this.CurrentRecipientName = null;
                    result = false;
                }
                else
                {
                    this.CurrentRecipientId = id;
                    this.CurrentRecipientName = to;
                    this.displayTo = "To: " + to;
                    this.toOffset = (int)ChatBox.messageFont(LocalizedContentManager.CurrentLanguageCode).MeasureString(this.displayTo).X;
                    this.toOffset += 32;
                    result = true;
                }
            }
            return result;
        }

        
        private string GetOtherPlayerNameFromUniqueId(long id)
        {
            Farmer farmer2 = Game1.getOnlineFarmers().FirstOrDefault((Farmer farmer) => farmer.UniqueMultiplayerID == id && farmer.UniqueMultiplayerID != Game1.player.UniqueMultiplayerID);
            return (farmer2 != null) ? farmer2.Name : null;
        }

        
        private bool TryGetUniqueIdFromName(string name, out long id)
        {
            Farmer farmer2 = Game1.getOnlineFarmers().FirstOrDefault((Farmer farmer) => farmer.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            id = ((farmer2 != null) ? farmer2.UniqueMultiplayerID : -1L);
            return id != -1L;
        }

        
        private float GetCursorOffset()
        {
            float num = 0f;
            bool flag = !this.finalText.Any<ChatSnippet>();
            float result;
            if (flag)
            {
                result = num;
            }
            else
            {
                for (int i = 0; i < this.currentSnippetIndex; i++)
                {
                    num += this.finalText[i].myLength;
                }
                bool flag2 = this.finalText[this.currentSnippetIndex].message == null;
                if (flag2)
                {
                    num += (float)((this.currentInsertPosition == 1) ? 40 : 0);
                }
                else
                {
                    num += ChatBox.messageFont(LocalizedContentManager.CurrentLanguageCode).MeasureString(this.finalText[this.currentSnippetIndex].message.Substring(0, this.currentInsertPosition)).X;
                }
                result = num;
            }
            return result;
        }

        
        public void OnLeftArrowPress()
        {
            bool flag = !this.finalText.Any<ChatSnippet>();
            if (!flag)
            {
                bool flag2 = this.currentInsertPosition == 0;
                if (flag2)
                {
                    bool flag3 = this.currentSnippetIndex == 0;
                    if (!flag3)
                    {
                        this.currentSnippetIndex--;
                        this.currentInsertPosition = ((this.finalText[this.currentSnippetIndex].message == null) ? 0 : (this.GetLastIndexOfCurrentSnippet() - 1));
                    }
                }
                else
                {
                    this.currentInsertPosition--;
                }
            }
        }

        
        public void OnRightArrowPress()
        {
            bool flag = !this.finalText.Any<ChatSnippet>();
            if (!flag)
            {
                int num = this.currentInsertPosition;
                string message = this.finalText[this.currentSnippetIndex].message;
                bool flag2 = num == ((message != null) ? message.Length : 1);
                if (flag2)
                {
                    bool flag3 = this.currentSnippetIndex == this.finalText.Count - 1;
                    if (!flag3)
                    {
                        this.currentSnippetIndex++;
                        this.currentInsertPosition = 1;
                    }
                }
                else
                {
                    this.currentInsertPosition++;
                }
            }
        }

        
        private void MoveCursorAllTheWayRight()
        {
            bool flag = !this.finalText.Any<ChatSnippet>();
            if (!flag)
            {
                this.currentSnippetIndex = this.finalText.Count - 1;
                this.currentInsertPosition = this.GetLastIndexOfCurrentSnippet();
            }
        }

        
        public CommandChatTextBoxState Save()
        {
            return new CommandChatTextBoxState(this.currentInsertPosition, this.currentSnippetIndex, this.CurrentRecipientId, this.CurrentRecipientName, this.finalText);
        }

        
        public void Load(CommandChatTextBoxState state, bool useSavedPosition = false)
        {
            this.finalText.Clear();
            foreach (ChatSnippet snippet in state.FinalText)
            {
                this.finalText.Add(Utils.CopyChatSnippet(snippet));
            }
            if (useSavedPosition)
            {
                this.currentInsertPosition = state.CurrentInsertPosition;
                this.currentSnippetIndex = state.CurrentSnippetIndex;
            }
            else
            {
                this.MoveCursorAllTheWayRight();
            }
            this.UpdateForNewRecepient(state.CurrentRecipientId, state.CurrentRecipientName);
            base.updateWidth();
        }

        

    }
}
