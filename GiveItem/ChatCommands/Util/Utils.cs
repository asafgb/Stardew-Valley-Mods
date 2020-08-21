using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Menus;

namespace ChatCommands.Util
{
    public static class Utils
    {
        public static string StripSMAPIPrefix(string input)
        {
            bool flag = input.Length == 0;
            string result;
            if (flag)
            {
                result = input;
            }
            else
            {
                result = ((input[0] != '[') ? input : string.Join("", new string[]
                {
                    input.Substring(input.IndexOf(']') + 1)
                }).TrimStart(new char[0]));
            }
            return result;
        }

        public static Color ConvertConsoleColorToColor(ConsoleColor color)
        {
            bool flag = color == ConsoleColor.White || color == ConsoleColor.Black;
            Color result;
            if (flag)
            {
                result = Utils.DefaultCommandColor;
            }
            else
            {
                try
                {
                    string name = Enum.GetName(typeof(ConsoleColor), color);
                    PropertyInfo property = typeof(Color).GetProperty(name, BindingFlags.Static | BindingFlags.Public);
                    result = (Color)property.GetValue(typeof(Color));
                }
                catch
                {
                    result = Utils.DefaultCommandColor;
                }
            }
            return result;
        }

        public static bool ShouldIgnore(string input)
        {
            return Utils.SuppressConsolePatterns.Any((Regex p) => p.IsMatch(input));
        }

        public static string[] ParseArgs(string input)
        {
            bool flag = false;
            IList<string> list = new List<string>();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char c in input)
            {
                bool flag2 = c == '"';
                if (flag2)
                {
                    flag = !flag;
                }
                else
                {
                    bool flag3 = !flag && char.IsWhiteSpace(c);
                    if (flag3)
                    {
                        list.Add(stringBuilder.ToString());
                        stringBuilder.Clear();
                    }
                    else
                    {
                        stringBuilder.Append(c);
                    }
                }
            }
            list.Add(stringBuilder.ToString());
            return (from item in list
                    where !string.IsNullOrWhiteSpace(item)
                    select item).ToArray<string>();
        }

        public static ChatSnippet CopyChatSnippet(ChatSnippet snippet)
        {
            return (snippet.message == null) ? new ChatSnippet(snippet.emojiIndex) : new ChatSnippet(snippet.message, LocalizedContentManager.CurrentLanguageCode);
        }

        public static string EncipherText(string text, long key)
        {
            Random random = new Random((int)key);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char c in text)
            {
                stringBuilder.Append(c + (char)random.Next(-32, 32));
            }
            return string.Concat<char>(stringBuilder.ToString().Reverse<char>());
        }

        public static string DecipherText(string text, long key)
        {
            Random random = new Random((int)key);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char c in text.Reverse<char>())
            {
                stringBuilder.Append(c - (char)random.Next(-32, 32));
            }
            return stringBuilder.ToString();
        }

        public static bool ShouldIncludeColorInfo(List<ChatSnippet> finalText)
        {
            bool flag = !string.IsNullOrEmpty(finalText[0].message) && finalText[0].message[0] == '/';
            if (flag)
            {
                bool flag2 = finalText[0].message.Split(new char[]
                {
                    ' '
                })[0].Length > 1;
                if (flag2)
                {
                    return false;
                }
            }
            return true;
        }

        private static readonly Color DefaultCommandColor = new Color(104, 214, 255);

        private static readonly Regex[] SuppressConsolePatterns = new Regex[]
        {
            new Regex("^TextBox\\.Selected is now '(?:True|False)'\\.$", RegexOptions.Compiled | RegexOptions.CultureInvariant),
            new Regex("^(?:FRUIT )?TREE: IsClient:(?:True|False) randomOutput: \\d+$", RegexOptions.Compiled | RegexOptions.CultureInvariant),
            new Regex("^loadPreferences\\(\\); begin", RegexOptions.Compiled | RegexOptions.CultureInvariant),
            new Regex("^savePreferences\\(\\); async=", RegexOptions.Compiled | RegexOptions.CultureInvariant),
            new Regex("^Multiplayer auth success$", RegexOptions.Compiled | RegexOptions.CultureInvariant),
            new Regex("^DebugOutput: added CLOUD", RegexOptions.Compiled | RegexOptions.CultureInvariant)
        };
    }
}
