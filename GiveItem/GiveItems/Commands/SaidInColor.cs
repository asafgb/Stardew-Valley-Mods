﻿using ChatCommands.Commands;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Commands
{
    class SaidInColor : ICommand
    {
        public void Handle(string name, string[] args)
        {
            throw new NotImplementedException();
        }

        public void Register(ICommandHelper helper)
        {
            helper.Add("search", "Search By Categories", new Action<string, string[]>(this.Handle));
        }
    }
}
