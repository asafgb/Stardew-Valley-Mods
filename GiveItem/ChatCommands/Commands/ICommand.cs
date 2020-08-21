using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatCommands
{
    public interface ICommand
    {
        void Handle(string input);

        bool CanHandle(string input);

        void InvokedCommand(string arg1, string[] arg2);
        void Register();
    }
}
