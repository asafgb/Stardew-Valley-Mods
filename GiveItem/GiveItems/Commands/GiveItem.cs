using ChatCommands;
using ChatCommands.Commands;
using Commands.Util;
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

        public void Handle(string name, string[] args)
        {
            Farmer farm = Game1.player;
            Item item = null;
            //KeyValuePair<int,string> item = Game1.objectInformation.FirstOrDefault(pair => pair.Value.Split('/')[0].Contains(args[1]));
            //Game1.player.addItemToInventoryBool((Item)new StardewValley.Object(item.Key, 1, false, -1, 0), false);

            if (args.Count() == 1)
            {
                item = Utils.FindItem(args[0]);
                if(item!=null)
                    Game1.player.addItemToInventoryBool(item);
            }
            else if (args.Count() %2==0)
            {
                for (int i = 0; i < args.Length; i += 2)
                {
                    switch (args[i])
                    {
                        case "player":
                            if(Game1.getAllFarmers().Any(farmer => farmer.Name == args[i + 1]))
                            {
                                farm = Game1.getAllFarmers().First(farmer => farmer.Name == args[i + 1]);
                            }
                            break;
                        case "item":
                            item = Utils.FindItem(args[0]);
                            break;
                        default:
                            break;
                    }
                }
            }


            //if(args.Count()==1)
            //{
            //    Game1.player.addItemToInventoryBool(Utils.FindItem(args[0]));
            //}
        }


        public void Register(ICommandHelper helper)
        {
            helper.Add("give", "Toggles displaying console output in the in game chat box.", new Action<string, string[]>(this.Handle));
        }
    }
}
