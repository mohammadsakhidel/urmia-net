using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreHelpers
{
    public static class DateHelper
    {
        public static DateInfo GetDateInfo(DateTime dt)
        {
            if (Configs.DateType == DateType.Shamsi)
            {
                ShamsiDateTime shamsi = ShamsiDateTime.MiladyToShamsi(dt);
                return new DateInfo(shamsi.Year, shamsi.Month, shamsi.Day);
            }
            else
            {
                return new DateInfo(dt.Year, dt.Month, dt.Day);
            }
        }

        public static DateTime? GetMiladyDateFromInfo(int day, int month, int year)
        {
            try
            {
                if (Configs.DateType == DateType.Shamsi)
                {
                    ShamsiDateTime shamsi = new ShamsiDateTime(year, month, day);
                    return shamsi.MiladyDate;
                }
                else
                {
                    return new DateTime(year, month, day);
                }
            }
            catch
            {
                return (DateTime?)null;
            }
        }

        public static DateTime? GetMiladyDateFromInfo(string day, string month, string year)
        {
            try
            {
                return GetMiladyDateFromInfo(Convert.ToInt32(day), Convert.ToInt32(month), Convert.ToInt32(year));
            }
            catch
            {
                return (DateTime?)null;
            }
        }

        public static string DateToText(DateTime dt)
        {
            if (Configs.DateType == DateType.Shamsi)
            {
                ShamsiDateTime shamsi = ShamsiDateTime.MiladyToShamsi(dt);
                var txt = String.Format("{0} {1} {2}",
                    shamsi.Day,
                    shamsi.MonthName,
                    shamsi.Year.ToString());
                return txt;
            }
            else
            {
                return String.Format("{0} {1}, {2}", 
                    HttpContext.GetGlobalResourceObject("Words", "Month" + dt.Month), 
                    dt.Day, 
                    dt.Year);
            }
        }

        public static string DateTimeToText(DateTime dt)
        {
            if (Configs.DateType == DateType.Shamsi)
            {
                ShamsiDateTime shamsi = ShamsiDateTime.MiladyToShamsi(dt);
                var txt = String.Format("{0} {1} {2} - {3}:{4}",
                    shamsi.Day,
                    shamsi.MonthName,
                    shamsi.Year.ToString(),
                    dt.Hour.ToString("D2"),
                    dt.Minute.ToString("D2"));
                return txt;
            }
            else
            {
                return String.Format("{0} {1}, {2} - {3}:{4}", 
                    HttpContext.GetGlobalResourceObject("Words", "Month" + dt.Month), 
                    dt.Day, 
                    dt.Year, 
                    dt.Hour.ToString("D2"),
                    dt.Minute.ToString("D2"));
            }
        }

        public static string DateToShortText(DateTime dt)
        {
            if (Configs.DateType == DateType.Shamsi)
            {
                ShamsiDateTime shamsi = ShamsiDateTime.MiladyToShamsi(dt);
                var txt = String.Format("{0}/{1}/{2}",
                    shamsi.Year,
                    shamsi.Month.ToString("D2"),
                    shamsi.Day.ToString("D2"));
                return txt;
            }
            else
            {
                return dt.ToShortDateString();
            }
        }

        public static string DateTimeToShortText(DateTime dt)
        {
            if (Configs.DateType == DateType.Shamsi)
            {
                ShamsiDateTime shamsi = ShamsiDateTime.MiladyToShamsi(dt);
                var txt = String.Format("{0}/{1}/{2} - {3}:{4}", 
                    shamsi.Year, 
                    shamsi.Month.ToString("D2"), 
                    shamsi.Day.ToString("D2"),
                    dt.Hour.ToString("D2"),
                    dt.Minute.ToString("D2"));
                return txt;
            }
            else
            {
                return String.Format("{0} - {1}:{2}",
                    dt.ToShortDateString(),
                    dt.Hour.ToString("D2"),
                    dt.Minute.ToString("D2"));
            }
        }

        public static bool IsValidDateTimeText(string text)
        {
            bool res = false;
            DateTime outDate = new DateTime();
            res = DateTime.TryParse(text, out outDate);
            return res;
        }
    }
}