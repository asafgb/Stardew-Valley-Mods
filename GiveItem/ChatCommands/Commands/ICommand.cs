using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatCommands.Commands
{
    public interface ICommand
    {
        void Register(ICommandHelper helper);
        void Handle(string name, string[] args);
    }
}
