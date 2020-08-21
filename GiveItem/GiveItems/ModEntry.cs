using System;
using System.Collections.Generic;
using System.Linq;
using ChatCommands;
using ChatCommands.Commands;

using Commands.Commands;
using Commands.Util;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;



namespace Commands
{
    public class ModEntry : ChatCommandsMod
    {
        IModHelper _helper;
        public override void Entry(IModHelper helper)
        {
            this._helper = helper;
            base.Entry(helper);
            _helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
            List<string> AllCommandsClasses = Utils.GetAllEntities();

            foreach (string cls in AllCommandsClasses)
            {
                ICommand cm=null;
                try
                {
                    cm = (ICommand)System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(cls);
                }
                catch (Exception)
                {
                    Type type = Type.GetType(cls);
                     cm = (ICommand)Activator.CreateInstance(type, helper);
                }

                if (cm != null)
                    cm.Register(helper.ConsoleCommands);
               
            }
        }

        private void GameLoop_GameLaunched(object sender, GameLaunchedEventArgs e)
        {
            _helper.Content.AssetEditors.Add(new MiscEditor(Helper));
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
