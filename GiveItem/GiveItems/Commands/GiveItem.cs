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
        private readonly IMonitor monitor;

        public GiveItem(IMonitor monitor) // , ChatCommandsConfig config, NotifyingTextWriter writer
        {
            this.monitor = monitor;
            
        }


        private void Handle(string name, string[] args)
        {
            //this.monitor.Log("yep that worked", LogLevel.Info);
            Game1.
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
