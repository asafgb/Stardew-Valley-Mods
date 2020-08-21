using System;
using System.Collections.Generic;
using System.Linq;
using ChatCommands;
using ChatCommands.Commands;
using ChatCommands.Util;
using Commands.Commands;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;



namespace Commands
{
    public class ModEntry : ChatCommandsMod
    { 

        public override void Entry(IModHelper helper)
        {
            base.Entry(helper);


            //helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            //helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;


            //this.Config = this.Helper.ReadConfig<ModConfig>();
            //bool exampleBool = this.Config.ExampleBoolean;
            
            List<ICommand> lstcommands = new List<ICommand>
            {
                //new GiveItem(helper,"giveItem","give specific item",this.Monitor)
                new GiveItem(),
            };
            foreach (ICommand command in lstcommands)
            {
                command.Register(helper.ConsoleCommands);
            }
        }



        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            Game1.chatBox.chatBox.OnEnterPressed += ChatBox_OnEnterPressed;
            var a = Game1.objectInformation;
            var b = Game1.clothingInformation;
        }

        private void ChatBox_OnEnterPressed(StardewValley.Menus.TextBox sender)
        {
            var a=sender.Text;
            //throw new NotImplementedException();
        }
    }
}
