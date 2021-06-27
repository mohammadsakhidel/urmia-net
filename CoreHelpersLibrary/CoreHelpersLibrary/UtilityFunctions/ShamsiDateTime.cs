using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Security;
using System.Collections.Generic;

/// <summary>
/// Summary description for MyDateTime
/// </summary>
/// 

namespace CoreHelpers
{
    public class ShamsiDateTime
    {
        public ShamsiDateTime()
        {
            Year = 0;
            Month = 0;
            Day = 0;
        }
        public ShamsiDateTime(int year, int month, int day)
        {
            //
            // TODO: Add constructor logic here
            //
            Year = year;
            Month = month;
            Day = day;
        }
        //*************** Properties :
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string DayName
        {
            get
            {
                return GetDayOfWeek(Persia.Calendar.ConvertToPersian(MiladyDate).DayOfWeek);
            }
        }
        public DateTime MiladyDate
        {
            get
            {
                return Persia.Calendar.ConvertToGregorian(Year, Month, Day);
            }
        }
        //************** Methods :
        public static ShamsiDateTime FromText(string text)
        {
            List<string> digits = MyHelper.SplitString(text, "/");
            ShamsiDateTime shamsi = new ShamsiDateTime(Convert.ToInt32(digits[0]), Convert.ToInt32(digits[1]), Convert.ToInt32(digits[2]));
            return shamsi;
        }

        public override string ToString()
        {
            return Year.ToString("D4") + "/" + Month.ToString("D2") + "/" + Day.ToString("D2");
        }
        public string ToLongString()
        {
            return DayName + " " + Day.ToString() + " " + MonthName + " " + Year.ToString();
        }
        public string ToLongString(bool ShowTime)
        {
            return DayName + " " + Day.ToString() + " " + MonthName + " " + Year.ToString() + (ShowTime ? "" : "");
        }
        public string ToMediumString()
        {
            return Day.ToString() + " " + MonthName + " " + Year.ToString();
        }
        public string MonthName
        {
            get
            {
                string name = "";
                switch (Month)
                {
                    case 1:
                        name = "فروردین";
                        break;
                    case 2:
                        name = "اردیبهشت";
                        break;
                    case 3:
                        name = "خرداد";
                        break;
                    case 4:
                        name = "تیر";
                        break;
                    case 5:
                        name = "مرداد";
                        break;
                    case 6:
                        name = "شهریور";
                        break;
                    case 7:
                        name = "مهر";
                        break;
                    case 8:
                        name = "آبان";
                        break;
                    case 9:
                        name = "آذر";
                        break;
                    case 10:
                        name = "دی";
                        break;
                    case 11:
                        name = "بهمن";
                        break;
                    case 12:
                        name = "اسفند";
                        break;
                }
                return name;
            }
        }
        public static ShamsiDateTime MiladyToShamsi(DateTime miladyDate)
        {
            int[] dateString = Persia.Calendar.ConvertToPersian(miladyDate).ArrayType;
            int year = dateString[0];
            int month = dateString[1];
            int day = dateString[2];
            return new ShamsiDateTime(year, month, day);
        }
        public string GetDayOfWeek(int day)
        {
            string dayName = "";
            if (day == 6)
            {
                dayName = "جمعه";
            }
            else if (day == 2)
            {
                dayName = "دوشنبه";
            }
            else if (day == 0)
            {
                dayName = "شنبه";
            }
            else if (day == 1)
            {
                dayName = "یکشنبه";
            }
            else if (day == 5)
            {
                dayName = "پنجشنبه";
            }
            else if (day == 3)
            {
                dayName = "سه شنبه";
            }
            else if (day == 4)
            {
                dayName = "چهارشنبه";
            }
            return dayName;
        }
    }
}
