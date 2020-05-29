using DataLayer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;

namespace Mehr.Classes
{
    public static class Extention 
    {
        public static void SetViewMessage(this Controller controller, string message, WebMessageType type,
                bool WithCloseBtn = true)
        {
            controller.TempData["Message"] = WebMessage.Get(message, type, WithCloseBtn);
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
            var dateSplit = value.Split('/');
            int year = Convert.ToInt32(dateSplit[0]);
            int month = Convert.ToInt32(dateSplit[1]);
            int day = Convert.ToInt32(dateSplit[2]);
            return pc.ToDateTime(year, month, day, 0, 0, 0, 0).ToShortDateString();
        }
    }
}
