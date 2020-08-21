using System;
using System.Collections.Generic;
using System.Linq;
using ChatCommands.ClassReplacements;
using ChatCommands.Commands;
using ChatCommands.Util;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace ChatCommands
{ 
    public class ChatCommandsMod : Mod, ICommandHandler
    {
        
        public virtual bool CanHandle(string input)
        {
            return input.Length > 1 && this.commandValidator.IsValidCommand(input.Substring(1));
        }

        
        public virtual void Handle(string input)
        {
            input = input.Substring(1);
            string[] array = Utils.ParseArgs(input);
            bool flag = array[0] == "halp";
            if (flag)
            {
                array[0] = "help";
            }
            //this.consoleNotifier.Notify(true);
            base.Helper.ConsoleCommands.Trigger(array[0], array.Skip(1).ToArray<string>());
            //this.consoleNotifier.Notify(false);
        }

        
        public override void Entry(IModHelper helper)
        {
            this.commandValidator = new CommandValidator(helper.ConsoleCommands);
            this.consoleNotifier = new NotifyingTextWriter(Console.Out, new NotifyingTextWriter.OnLineWritten(this.OnLineWritten));
            this.inputState = helper.Reflection.GetField<InputState>(typeof(Game1), "input", true).GetValue();
            Console.SetOut(this.consoleNotifier);
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.GameLoop.ReturnedToTitle += this.OnReturnedToTitle;
            this.modConfig = helper.ReadConfig<ChatCommandsConfig>();
            //IEnumerable<ICommand> enumerable = new ICommand[]
            //{
            //    new ListenCommand(base.Monitor, this.modConfig, this.consoleNotifier)
            //};
            //foreach (ICommand command in enumerable)
            //{
            //    command.Register(helper.ConsoleCommands);
            //}
        }

        
        private void OnReturnedToTitle(object sender, ReturnedToTitleEventArgs e)
        {
            CommandChatBox commandChatBox = Game1.chatBox as CommandChatBox;
            if (commandChatBox != null)
            {
                commandChatBox.ClearHistory();
            }
        }

        
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            bool flag = base.Helper.Input.IsDown(SButton.Escape) && !this.wasEscapeDown;
            if (flag)
            {
                this.wasEscapeDown = !this.wasEscapeDown;
                CommandChatBox commandChatBox = Game1.chatBox as CommandChatBox;
                if (commandChatBox != null)
                {
                    commandChatBox.EscapeStatusChanged(this.wasEscapeDown);
                }
            }
            bool flag2 = e.IsMultipleOf(2U) && Game1.chatBox != null && Game1.chatBox.isActive();
            if (flag2)
            {
                bool flag3 = base.Helper.Input.IsDown(SButton.Left);
                bool flag4 = base.Helper.Input.IsDown(SButton.Right);
                bool flag5 = base.Helper.Input.IsDown(SButton.Up);
                bool flag6 = base.Helper.Input.IsDown(SButton.Down);
                bool flag7 = flag3 ^ flag4 ^ flag5 ^ flag6;
                if (flag7)
                {
                    SButton key = flag3 ? SButton.Left : (flag4 ? SButton.Right : (flag5 ? SButton.Up : SButton.Down));
                    bool flag8 = this.repeatWaitPeriod != 0;
                    if (flag8)
                    {
                        this.repeatWaitPeriod--;
                    }
                    bool flag9 = this.repeatWaitPeriod == 0;
                    if (flag9)
                    {
                        Game1.chatBox.receiveKeyPress((Keys)key);
                        bool flag10 = flag5 || flag6;
                        if (flag10)
                        {
                            this.repeatWaitPeriod = 15;
                        }
                    }
                }
                else
                {
                    this.repeatWaitPeriod = 15;
                }
            }
        }

        
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            bool flag = Game1.chatBox != null && Game1.chatBox is CommandChatBox;
            if (!flag)
            {
                bool flag2 = Game1.chatBox != null;
                if (flag2)
                {
                    Game1.onScreenMenus.Remove(Game1.chatBox);
                }
                Game1.chatBox = new CommandChatBox(base.Helper, this, this.modConfig);
                Game1.onScreenMenus.Add(Game1.chatBox);
                base.Monitor.Log("Replaced Chatbox", LogLevel.Trace);
            }
        }

        
        private void OnLineWritten(char[] buffer, int index, int count)
        {
            string text = string.Join<char>("", buffer.Skip(index).Take(count)).Trim();
            string text2 = Utils.StripSMAPIPrefix(text).Trim();
            bool flag = Utils.ShouldIgnore(text2);
            if (!flag)
            {
                bool flag2 = this.consoleNotifier.IsNotifying();
                if (flag2)
                {
                    text = text2;
                }
                bool flag3 = !string.IsNullOrWhiteSpace(text);
                if (flag3)
                {
                    CommandChatBox commandChatBox = Game1.chatBox as CommandChatBox;
                    if (commandChatBox != null)
                    {
                        commandChatBox.AddConsoleMessage(text, Utils.ConvertConsoleColorToColor(Console.ForegroundColor));
                    }
                }
            }
        }

        
        private const int BaseWaitPeriod = 15;

        
        private CommandValidator commandValidator;

        
        private NotifyingTextWriter consoleNotifier;

        
        private InputState inputState;

        
        private ChatCommandsConfig modConfig;

        
        private int repeatWaitPeriod = 15;

        
        private bool wasEscapeDown;
    }
}
