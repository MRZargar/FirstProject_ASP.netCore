using DataLayer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Mehr.Classes
{
    public static class Extention 
    {
        public static void SetViewMessage(this Controller controller, string message, WebMessageType type,
                bool WithCloseBtn = true)
        {
            controller.TempData["Message"] = WebMessage.Get(message, type, WithCloseBtn);
        }

        public static string getFirstSolarMonth(this DateTime value, int moveMonth = 0)
        {
            string today = DateTime.Today.ToSolar();
            string toMonth = today.Substring(0, today.Length - 2) + "01";

            if (moveMonth == 0)
            {
                return toMonth;
            }

            DateTime toMonthAD = Convert.ToDateTime(toMonth.ToAD());

            PersianCalendar pc = new PersianCalendar();
            return pc.AddMonths(toMonthAD, moveMonth).ToSolar();
        }

        public static string ToSolar(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00");
        }

        public static string ToSolar(this string value)
        {
            return Convert.ToDateTime(value).ToSolar();
        }

        public static string ToAD(this string value)
        {
            PersianCalendar pc = new PersianCalendar();
            value = value.PersianToEnglish();
            string date = value;
            string time = string.Empty;
            try
            {
                bool hasTime = value.Contains(':');
                if (hasTime)
                {
                    int idx = value.IndexOf(' ');
                    date = value.Substring(0, idx);
                    time = value.Substring(idx);
                }
                var dateSplit = date.Split('/');
                int year = Convert.ToInt32(dateSplit[0]);
                int month = Convert.ToInt32(dateSplit[1]);
                int day = Convert.ToInt32(dateSplit[2]);
                return pc.ToDateTime(year, month, day, 0, 0, 0, 0).ToShortDateString() + time;
            }
            catch (Exception)
            {
                throw new Exception("not valid date pattern ...");
            }
        }

        public static string PersianToEnglish(this string persianStr)
        {
            Dictionary<string, string> LettersDictionary = new Dictionary<string, string>
            {
                ["۰"] = "0",
                ["۱"] = "1",
                ["۲"] = "2",
                ["۳"] = "3",
                ["۴"] = "4",
                ["۵"] = "5",
                ["۶"] = "6",
                ["۷"] = "7",
                ["۸"] = "8",
                ["۹"] = "9"
            };
            return LettersDictionary.Aggregate(persianStr, (current, item) =>
                         current.Replace(item.Key, item.Value));
        }
    }
}
