using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreHelpers
{
    public class DateInfo
    {
        public DateInfo(int y, int m, int d)
        {
            Year = y;
            Month = m;
            Day = d;
        }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}