using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    class ModConfig
    {
        //public bool ExampleBoolean { get; set; } = true;
        //public int ExampleNumber { get; set; } = 5;
        //public ModConfig()
        //{
        //    this.ExampleBoolean = true;
        //    this.ExampleNumber = 5;
        //}
        public bool ListenToConsoleOnStartup { get; set; } = false;
        public int MaximumNumberOfHistoryMessages { get; set; } = 70;
        public bool UseMonospacedFontForCommandOutput { get; set; } = true;
    }
}
