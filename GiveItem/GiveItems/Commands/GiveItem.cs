using ChatCommands;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Commands
{
    class GiveItem : Command
    {
        public GiveItem(IModHelper helper, string command, string Desc , IMonitor monitor = null) : base(helper, command, Desc, monitor) { }

        public override void InvokedCommand(string arg1, string[] arg2)
        {
            this.monitor.Log($"{null} asaf", LogLevel.Debug);
            //base.InvokedCommand(arg1, arg2);
        }
    }
}
