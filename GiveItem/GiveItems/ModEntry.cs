using System;
using System.Collections.Generic;
using System.Linq;
using ChatCommands;
using ChatCommands.Util;
using Commands.Commands;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;



namespace Commands
{
    public class ModEntry : Mod, ICommand
    {
        /// <summary>The mod configuration from the player.</summary>
        private ModConfig Config;
        private NotifyingTextWriter consoleNotifier;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;


            List<ICommand> lstcommands = new List<ICommand>
            {
                new GiveItem(helper,"giveItem","give specific item",this.Monitor)
            };


            //this.Config = this.Helper.ReadConfig<ModConfig>();
            //bool exampleBool = this.Config.ExampleBoolean;
        }

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            Game1.chatBox.chatBox.OnEnterPressed += ChatBox_OnEnterPressed;
        }

        private void ChatBox_OnEnterPressed(StardewValley.Menus.TextBox sender)
        {
            var a=sender.Text;
            //throw new NotImplementedException();
        }



        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            // print button presses to the console window
           // this.Monitor.Log($"{Game1.player.Name} pressed {e.Button}.", LogLevel.Debug);
        }

        public void Handle(string input)
        {
            throw new NotImplementedException();
        }

        public bool CanHandle(string input)
        {
            throw new NotImplementedException();
        }

        public void InvokedCommand(string arg1, string[] arg2)
        {
            throw new NotImplementedException();
        }

        public void Register()
        {
            throw new NotImplementedException();
        }
    }
}
