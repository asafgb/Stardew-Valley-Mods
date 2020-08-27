
using LockChest.Frameworks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockChest.Interface
{
    [JsonObject(MemberSerialization.OptIn)]
    class GameTimeHandler
    {
        [JsonProperty]
        public SDate Date;
        [JsonProperty]
        public int TimeOfDay;

        public GameTimeHandler(int Year, string seasons, int DayofMonth, int TimeOfDay)
        {
            this.Date = new SDate(DayofMonth, seasons, Year);
            this.TimeOfDay = TimeOfDay;
        }

        [JsonConstructor]
        public GameTimeHandler(SDate Date, int TimeOfDay) : this(Date.Year,Date.season.ToString(),Date.DayofMonth,TimeOfDay)
        {

        }

        public bool isThisTimeIsLater(GameTimeHandler gameTime)
        {
            return this.isThisTimeIsLater(gameTime.Date, gameTime.TimeOfDay);
        }

        public bool isThisTimeIsLater(SDate Date, int TimeOfDay)
        {
            return  this.Date > Date || this.TimeOfDay >= TimeOfDay;
        }

    }
}
