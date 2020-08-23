using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Common
{
    internal class Config
    {
        public bool CheckForUpdates { get; set; } = true;

        public string GithubUrlForProjectManifest { get; set; } = "";
    }
}
