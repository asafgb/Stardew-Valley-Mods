using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatCommands
{
    public class ChatCommandsConfig
    {
        public bool ListenToConsoleOnStartup { get; set; } = false;
        public int MaximumNumberOfHistoryMessages { get; set; } = 70;
        public bool UseMonospacedFontForCommandOutput { get; set; } = true;
    }
}
