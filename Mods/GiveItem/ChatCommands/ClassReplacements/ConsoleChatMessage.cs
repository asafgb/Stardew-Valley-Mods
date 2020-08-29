using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace ChatCommands.ClassReplacements
{
    internal class ConsoleChatMessage : ChatMessage
    {
        private static float widestCharacter = -1f;
        public void ConsoleDraw(SpriteBatch b, int x, int y)
        {
            SpriteFont spriteFont = ChatBox.messageFont(this.language);
            float num = 0f;
            float num2 = 0f;
            for (int i = 0; i < this.message.Count; i++)
            {
                bool flag = this.message[i].emojiIndex != -1;
                if (flag)
                {
                    b.Draw(ChatBox.emojiTexture, new Vector2((float)((double)x + (double)num + 1.0), (float)((double)y + (double)num2 - 4.0)), new Rectangle?(new Rectangle(this.message[i].emojiIndex * 9 % ChatBox.emojiTexture.Width, this.message[i].emojiIndex * 9 / ChatBox.emojiTexture.Width * 9, 9, 9)), Color.White * this.alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.99f);
                }
                else
                {
                    bool flag2 = this.message[i].message != null;
                    if (flag2)
                    {
                        bool flag3 = this.message[i].message.Equals(Environment.NewLine);
                        if (flag3)
                        {
                            num = 0f;
                            num2 += spriteFont.MeasureString("(").Y;
                        }
                        else
                        {
                            ConsoleChatMessage.DrawMonoSpacedString(b, spriteFont, this.message[i].message, new Vector2((float)x + num, (float)y + num2), this.color * this.alpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);
                        }
                    }
                }
                num += this.message[i].myLength;
                bool flag4 = (double)num >= 888.0;
                if (flag4)
                {
                    num = 0f;
                    num2 += spriteFont.MeasureString("(").Y;
                    bool flag5 = this.message.Count > i + 1 && this.message[i + 1].message != null && this.message[i + 1].message.Equals(Environment.NewLine);
                    if (flag5)
                    {
                        i++;
                    }
                }
            }
        }

        
        private static void DrawMonoSpacedString(SpriteBatch b, SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            foreach (char c in text)
            {
                b.DrawString(font, c.ToString() ?? "", position + new Vector2((ConsoleChatMessage.widestCharacter - font.MeasureString(c.ToString() ?? "").X) / 2f, 0f), color, rotation, origin, scale, effects, layerDepth);
                position.X += ConsoleChatMessage.widestCharacter;
            }
        }
        
        public static void Init(LocalizedContentManager.LanguageCode language)
        {
            SpriteFont spriteFont = ChatBox.messageFont(language);
            ConsoleChatMessage.widestCharacter = spriteFont.MeasureString("e").X;
        }

        
        public static float MeasureStringWidth(SpriteFont font, string text, bool isConsole)
        {
            bool flag = !isConsole;
            float result;
            if (flag)
            {
                result = font.MeasureString(text).X;
            }
            else
            {
                float num = 0f;
                foreach (string text2 in text.Split(new string[]
                {
                    "\n",
                    "\r\n"
                }, StringSplitOptions.None))
                {
                    bool flag2 = (float)text2.Length * ConsoleChatMessage.widestCharacter > num;
                    if (flag2)
                    {
                        num = (float)text2.Length * ConsoleChatMessage.widestCharacter;
                    }
                }
                result = num;
            }
            return result;
        }
    }
}
