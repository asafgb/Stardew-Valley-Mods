using ChatCommands.ClassReplacements;
using ChatCommands.Commands;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Commands
{
    class SaidInColor : ICommand
    {
        IModHelper _helper;
        public SaidInColor(IModHelper helper)
        {
            this._helper = helper;
        }
        
        public void Handle(string name, string[] args)
        {
            string message = "heyyy everyone!";

            if(args.Count() >0)
            {
                message = args[0];
            }

            CommandChatBox commandChatBox = Game1.chatBox as CommandChatBox;

            commandChatBox.multiplayer.globalChatInfoMessage("Custom.Chat", new[] { message });

            commandChatBox.multiplayer.sendChatMessage(LocalizedContentManager.CurrentLanguageCode, message, Game1.player.UniqueMultiplayerID);


            Game1.chatBox.activate();
            Game1.chatBox.chatBox.finalText.Add(new ChatSnippet("/color red", LocalizedContentManager.LanguageCode.en));
            Game1.chatBox.chatBox.RecieveCommandInput('\r');

            Game1.chatBox.activate();
            Game1.chatBox.chatBox.reset();
            Game1.chatBox.chatBox.finalText.Add(new ChatSnippet(message, LocalizedContentManager.LanguageCode.en));
            Game1.chatBox.chatBox.updateWidth();
            Game1.chatBox.chatBox.RecieveCommandInput('\r');


            Game1.chatBox.activate();
            Game1.chatBox.chatBox.finalText.Add(new ChatSnippet("/color white", LocalizedContentManager.LanguageCode.en));
            Game1.chatBox.chatBox.RecieveCommandInput('\r');
        }

        public void Register(ICommandHelper helper)
        {
            helper.Add("Said", "Push Notify in Console", new Action<string, string[]>(this.Handle));
        }
    }
}
