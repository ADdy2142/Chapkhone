using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.Utilities.Extensions
{
    public static class DateTimeExtestions
    {
        public static string ToShortPersianDate(this DateTime value)
        {
            var persianCalendar = new PersianCalendar();
            int dayOfMonth = persianCalendar.GetDayOfMonth(value);
            int monthOfYear = persianCalendar.GetMonth(value);
            int year = persianCalendar.GetYear(value);

            string month = string.Empty;
            switch (monthOfYear)
            {
                case 1:
                    month = "فروردین";
                    break;

                case 2:
                    month = "اردیبهشت";
                    break;

                case 3:
                    month = "خرداد";
                    break;

                case 4:
                    month = "تیر";
                    break;

                case 5:
                    month = "مرداد";
                    break;

                case 6:
                    month = "شهریور";
                    break;

                case 7:
                    month = "مهر";
                    break;

                case 8:
                    month = "آبان";
                    break;

                case 9:
                    month = "آذر";
                    break;

                case 10:
                    month = "دی";
                    break;

                case 11:
                    month = "بهمن";
                    break;

                case 12:
                    month = "اسفند";
                    break;
            }

            return $"{dayOfMonth} {month} {year}";
        }
    }
}