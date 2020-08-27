using LockChest.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockChest.Frameworks
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SDate
    {
        [JsonProperty]
        public int Year;
        [JsonProperty]
        public SeasonEnum season;
        [JsonProperty]
        public int DayofMonth;

        //public SDate(int day, string season) : this(day, season, 1)
        //{

        //}
        [JsonConstructor]
        public SDate(int day, string season, int year)
        {
            this.DayofMonth = day;
            this.season = (SeasonEnum)Enum.Parse(typeof(SeasonEnum), season);
            this.Year = year;
        }

        
        public SDate(int day, SeasonEnum season, int year)
        {
            this.DayofMonth = day;
            this.season = season;
            this.Year = year;
        }

        public int DaysSinceStart
        {
            get
            {
                return (this.Year - 1) * 4 * 28 + (int)season * 28 + this.DayofMonth;
            }
        }

        public void AddDays(int Offset)
        {
            int newDate = DaysSinceStart + Offset;
            // if add - Alot of days
            if (newDate > 0)
            {
                this.Year = (newDate / 28 / 4);
                newDate -= this.Year * 28 * 4;
                this.Year += 1;

                this.season = (SeasonEnum)(newDate / 28);
                newDate -= (int)this.season * 28;

                this.DayofMonth = newDate;
            }
        }

        public static SDate operator +(SDate b, SDate c)
        {
            SDate date = new SDate(b.DayofMonth, b.season, b.Year);
            date.AddDays(c.DaysSinceStart);
            return date;
        }

        public static bool operator >(SDate b, SDate c)
        {
            return b.DaysSinceStart > c.DaysSinceStart;
        }
        public static bool operator <(SDate b, SDate c)
        {
            return b.DaysSinceStart < c.DaysSinceStart;
        }
        public static bool operator ==(SDate b, SDate c)
        {
            return b.DaysSinceStart == c.DaysSinceStart;
        }
        public static bool operator !=(SDate b, SDate c)
        {
            return b.DaysSinceStart != c.DaysSinceStart;
        }
    }
}
