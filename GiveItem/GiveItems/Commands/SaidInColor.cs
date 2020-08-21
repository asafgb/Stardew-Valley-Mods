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
            //SendPublicChat("test1", true);
            //SendDirectMessage(Game1.player.UniqueMultiplayerID, "test2");

            //var mp = _helper.Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer").GetValue();
            //mp.broadcastGlobalMessage("Custom.Chat", false, new string[] { "test" });

            //var mp = _helper.Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer").GetValue();
            //mp.globalChatInfoMessage("StardewAquarium.FishDonated", new[] { Game1.player.Name, "test" });

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

            //CommandChatBox commandChatBox = Game1.chatBox as CommandChatBox;
            ////commandChatBox.multiplayer.broadcastGlobalMessage()
            //foreach (Farmer farmer in Game1.getAllFarmers())
            //{
            //    commandChatBox.multiplayer.sendChatMessage(LocalizedContentManager.CurrentLanguageCode, "heyyyy" , farmer.UniqueMultiplayerID);
            //}
            ////Game1.multiplayer.sendChatMessage(LocalizedContentManager.CurrentLanguageCode, "testing", Multiplayer.AllPlayers);

            //var mp = _helper.Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer").GetValue();
            //mp.globalChatInfoMessage("StardewAquarium.FishDonated", new[] { Game1.player.Name, "yo" });

            //mp.sendChatMessage(LocalizedContentManager.CurrentLanguageCode, "testing", Multiplayer.AllPlayers);


            //commandChatBox.multiplayer.broadcastGlobalMessage(LocalizedContentManager.CurrentLanguageCode.ToString(), false, new string[] { "asaf" });
        }


        //private void SendPublicChat(string text, bool error = false)
        //{
        //    // format text
        //    if (error)
        //    {
        //        Game1.chatBox.activate();
        //        Game1.chatBox.setText("/color red");
        //        Game1.chatBox.chatBox.RecieveCommandInput('\r');
        //    }

        //    // send chat message
        //    // (Bypass Game1.chatBox.setText which doesn't handle long text well)
        //    Game1.chatBox.activate();
        //    Game1.chatBox.chatBox.reset();
        //    Game1.chatBox.chatBox.finalText.Add(new ChatSnippet(text, LocalizedContentManager.LanguageCode.en));
        //    Game1.chatBox.chatBox.updateWidth();
        //    Game1.chatBox.chatBox.RecieveCommandInput('\r');
        //}

        ///// <summary>Send a private message to a specified player.</summary>
        ///// <param name="playerID">The player ID.</param>
        ///// <param name="text">The text to send.</param>
        //private void SendDirectMessage(long playerID, string text)
        //{
        //    Game1.server.sendMessage(playerID, Multiplayer.chatMessage, Game1.player, this._helper.Content.CurrentLocaleConstant, text);
        //}


        public void Register(ICommandHelper helper)
        {
            helper.Add("Said", "Push Notify in Console", new Action<string, string[]>(this.Handle));
        }
    }
}
