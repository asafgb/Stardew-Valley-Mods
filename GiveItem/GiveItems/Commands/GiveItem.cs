using ChatCommands;
using ChatCommands.Commands;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Commands
{
    class GiveItem : ICommand
    {

        public GiveItem() // IMonitor monitor, ChatCommandsConfig config, NotifyingTextWriter writer
        {
        }

        private void Handle(string name, string[] args)
        {
            //KeyValuePair<int,string> item = Game1.objectInformation.FirstOrDefault(pair => pair.Value.Split('/')[0].Contains(args[1]));
            //Game1.player.addItemToInventoryBool((Item)new StardewValley.Object(item.Key, 1, false, -1, 0), false);
            //this.monitor.Log("yep that worked", LogLevel.Info);
        }


        //public GiveItem(IModHelper helper, string command, string Desc , IMonitor monitor = null) : base(helper, command, Desc, monitor) { }

        //public override void InvokedCommand(string arg1, string[] arg2)
        //{
        //    this.monitor.Log($"{null} asaf", LogLevel.Debug);
        //    //base.InvokedCommand(arg1, arg2);
        //}
        public void Register(ICommandHelper helper)
        {
            helper.Add("soon", "Toggles displaying console output in the in game chat box.", new Action<string, string[]>(this.Handle));
        }


    }
}
