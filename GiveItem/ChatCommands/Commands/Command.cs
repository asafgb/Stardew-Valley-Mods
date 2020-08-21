using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace ChatCommands
{
    public class Command : ICommand
    {
        public string command;
        public string Desc;
        public IModHelper helper;
        public IMonitor monitor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"> The Mods commands</param>
        /// <param name="command"> The commands that gonna run</param>
        /// <param name="Desc"> description of the Function</param>
        /// <param name="monitor">If needed for console print</param>
        public Command(IModHelper helper, string command, string Desc, IMonitor monitor)
        {
            this.command = command.ToLower();
            this.Desc = Desc;
            this.helper = helper;
            this.monitor = monitor;
            this.Register();
        }

        public void Register()
        {
            helper.ConsoleCommands.Add(this.command, this.Desc, this.InvokedCommand);
        }

        public virtual void InvokedCommand(string arg1, string[] arg2) { }

        public virtual void Handle(string input)
        {
        }

        public virtual bool CanHandle(string input)
        {
            return false;
        }
    }
}
