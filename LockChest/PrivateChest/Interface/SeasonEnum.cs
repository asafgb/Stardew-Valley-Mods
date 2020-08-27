using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockChest.Interface
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SeasonEnum
    {
        spring=0,
        summer=1,
        fall=2,
        winter=3
    }
}
