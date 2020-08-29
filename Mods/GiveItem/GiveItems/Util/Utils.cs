using ChatCommands.Commands;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Util
{
    public class Utils
    {
        public static void FindItems(string name)
        {
            List<KeyValuePair<int,string>> Items= Game1.objectInformation.Where(pair => pair.Value.Split('/')[0].Contains(name)).ToList();
        }

        public static Item FindItem(string name)
        {
            List<KeyValuePair<int, string>> items= Game1.objectInformation.Where(pair => pair.Value.Split('/')[0].ToLower() == name.ToLower()).ToList();
            if(items.Count ==1)
                return (Item)new StardewValley.Object(items[0].Key, 1, false, -1, 0);
            return null;
        }
        public static int FindItemIndex(string name)
        {
            List<KeyValuePair<int, string>> items = Game1.objectInformation.Where(pair => pair.Value.Split('/')[0].ToLower() == name.ToLower()).ToList();
            if (items.Count == 1)
                return items[0].Key;
            return -2;
        }

        public static List<string> GetAllEntities()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => typeof(ICommand).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                 .Select(x => x.FullName).ToList();
        }

    }
}
