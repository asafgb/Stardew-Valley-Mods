using ChatCommands;
using ChatCommands.ClassReplacements;
using ChatCommands.Commands;
using Commands.Util;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Commands.Enums.GameEnums;

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
            int itemIndex = -1, itemQulity = 0, itemStack = 1, itemPrice = -1;
            int result;
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
                        case "i":
                            //item = Utils.FindItem(args[i + 1]);
                            itemIndex = Utils.FindItemIndex(args[i + 1]);
                            break;
                        case "qu":// Quality
                                Quality quality = (Quality)Enum.Parse(typeof(Quality), args[i + 1]);
                                itemQulity = (int)quality;
                            break;
                        case "st": // stack
                            if(int.TryParse(args[i + 1], out result))
                            {
                                itemStack = result;
                            }
                            break;
                        case "pr": //price
                            if (int.TryParse(args[i + 1], out result))
                            {
                                itemPrice = result;
                            }
                            break;
                        default:
                            break;
                    }
                }
                if(itemIndex != -1)
                {
                    item = (Item)new StardewValley.Object(itemIndex, itemStack, false, itemPrice, itemQulity);
                    if(farm.addItemToInventoryBool(item))
                    {
                        Game1.chatBox.addMessage($"item {item.DisplayName} has been added",Color.Aqua);
                        //Game1.chatBox.receiveChatMessage()
                    }
                }
                else
                {


                    Game1.chatBox.addMessage($"/{name} item <itemname> qu <4/Iridium> st 25 pr 3000", Color.Aqua);
                    Game1.chatBox.addMessage($"Example: /{name} item \"Cave Carrot\" qu Gold st 25 pr 3000", Color.Aqua);
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
