using System;
using System.Reflection;
using ChatCommands.Util;
using StardewModdingAPI;

namespace ChatCommands
{
    public class CommandValidator
    {
        private readonly object commandHelper;
        private readonly MethodInfo commandHelperGet;

        public CommandValidator(ICommandHelper helper)
        {
            FieldInfo field = helper.GetType().GetField("CommandManager", BindingFlags.Instance | BindingFlags.NonPublic);
            this.commandHelper = ((field != null) ? field.GetValue(helper) : null);
            object obj = this.commandHelper;
            this.commandHelperGet = ((obj != null) ? obj.GetType().GetMethod("Get", BindingFlags.Instance | BindingFlags.Public) : null);
        }

        public bool IsValidCommand(string input)
        {
            string text = Utils.ParseArgs(input)[0];
            string text2 = text;
            string text3 = text2;
            if (text3 != null)
            {
                if (text3 == "halp")
                {
                    return true;
                }
                if (text3 == "help")
                {
                    return false;
                }
                if (text3 == "w" || text3 == "r")
                {
                    return false;
                }
            }
            MethodInfo methodInfo = this.commandHelperGet;
            return ((methodInfo != null) ? methodInfo.Invoke(this.commandHelper, new object[]
            {
                text
            }) : null) != null;
        }


    }
}
