using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace AmirCalendar
{
    public enum SectionOfDate
    {
        Year,
        Month,
        Day
    }

    public enum DateFormat
    {
        Long,
        Short
    }

    public enum CalendarTheme
    {
        Blue,
        Gold,
        Green,
        WhiteSmoke
    }

    public enum ControlType
    {
        DatePicker,
        MonthCalendar
    }

    public static class FarsiDateHelper
    {
        internal static List<DayEvent> CalendarEvents;
        internal static Dictionary<int, int> CalendarHijriAdjustment;
        internal static List<string> tarikhKhasHoliday;
        private static readonly List<string> DayName;
        private static readonly List<string> MonthName;

        static FarsiDateHelper()
        {
            CalendarEvents = GetAllEvents();
            CalendarHijriAdjustment = GetHijriAdjustment();
            tarikhKhasHoliday = GetTarikhKhasHoliday();
            DayName = new List<string> { "یکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه" };
            MonthName = new List<string> { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
        }
        /// <summary>
        /// Get persian name of day in week.
        /// </summary>
        /// <param name="date">Gregorian date</param>
        /// <example>شنبه</example>
        public static string GetFarsiDayOfWeekName(DateTime date)
        {
            var pc = new PersianCalendar();
            return DayName[(int)pc.GetDayOfWeek(date)];
        }

        /// <summary>
        /// Get persian name of month in year.
        /// </summary>
        /// <param name="date">Gregorian date</param>
        /// <example>فروردین</example>
        public static string GetFarsiMonthName(DateTime date)
        {
            var pc = new PersianCalendar();
            return MonthName[pc.GetMonth(date) - 1];
        }

        /// <summary>
        /// Get persian date in long format
        /// </summary>
        /// <param name="date">Gregorian date</param>
        /// <example>شنبه 23 آبان سال 1400</example>
        public static string GetLongFarsiDate(DateTime date)
        {
            var pc = new PersianCalendar();
            return string.Format("{0} {1:D2} {2} سال {3:D4}", GetFarsiDayOfWeekName(date), pc.GetDayOfMonth(date),
                                 GetFarsiMonthName(date), pc.GetYear(date));
        }

        /// <summary>
        /// Get persian date in short format
        /// </summary>
        /// <param name="date">Gregorian date</param>
        /// <example>1400/08/23</example>
        public static string GetShortFarsiDate(DateTime date)
        {
            var pc = new PersianCalendar();
            return String.Format("{0}/{1}/{2}", pc.GetYear(date).ToString("0000"),
                                 pc.GetMonth(date).ToString("00"), pc.GetDayOfMonth(date).ToString("00"));
        }

        /// <summary>
        /// Get part of persian or hijri date. part is year number or month number in year or day number in month
        /// </summary>
        /// <param name="date">Gregorian date</param>
        /// <param name="getFarsi">get part of persian or hijri date</param>
        /// <param name="section">get year number or month number in year or day number in month</param>
        /// <param name="hijriAdjustment">number of days to add or subtract from hijri calendar</param>
        public static int GetSectionOfDate(DateTime date, bool getFarsi, SectionOfDate section, int hijriAdjustment = 0)
        {
            var c = getFarsi ? new PersianCalendar() : (Calendar)new HijriCalendar { HijriAdjustment = hijriAdjustment };
            int result;
            switch (section)
            {
                case SectionOfDate.Year:
                    result = c.GetYear(date);
                    break;
                case SectionOfDate.Month:
                    result = c.GetMonth(date);
                    break;
                default:
                    result = c.GetDayOfMonth(date);
                    break;
            }
            return result;
        }

        /// <summary>
        /// Input persian date is holiday or not
        /// </summary>
        /// <param name="farsiDate">Persian date. example: 1392/01/05</param>
        public static bool IsHolidayFarsiDate(string farsiDate)
        {
            if (!ValidateFarsiDate(farsiDate))
                throw new Exception("Incorrect Persian Date.");
            if (tarikhKhasHoliday.Contains( farsiDate)) return true;

            var gDate = GetGregorianDate(farsiDate);
            if (gDate.DayOfWeek == DayOfWeek.Friday) return true;
            return CalendarEvents.Any(c => c.IsHoliday &&
                                                         c.Month == GetSectionOfDate(gDate, c.IsPersian, SectionOfDate.Month, (c.IsPersian || CalendarHijriAdjustment == null ? 0 : CalendarHijriAdjustment[c.Month])) &&
                                                         c.Day == GetSectionOfDate(gDate, c.IsPersian, SectionOfDate.Day, (c.IsPersian || CalendarHijriAdjustment == null ? 0 : CalendarHijriAdjustment[c.Month])));
        }

        /// <summary>
        /// Get gregorian date from persian date
        /// </summary>
        /// <param name="farsiDate">Persian date. example: 1392/01/05</param>
        public static DateTime GetGregorianDate(string farsiDate)
        {
            if (!ValidateFarsiDate(farsiDate))
                throw new Exception("Incorrect Persian Date.");
            var pc = new PersianCalendar();
            var dateSection = farsiDate.Split(new[] { '/' });
            return pc.ToDateTime(Convert.ToInt32(dateSection[0]), Convert.ToInt32(dateSection[1]),
                                 Convert.ToInt32(dateSection[2]), 0, 0, 0, 0);
        }

        /// <summary>
        /// Gets the number of days in the specified month and year of the input persian date.
        /// </summary>
        /// <param name="farsiDate">Persian date. example: 1392/01/05</param>
        public static int GetNumberOfDaysInFarsiMonth(string farsiDate)
        {
            if (!ValidateFarsiDate(farsiDate))
                throw new Exception("Incorrect Persian Date.");
            var pc = new PersianCalendar();
            var dateSection = farsiDate.Split(new[] { '/' });
            return pc.GetDaysInMonth(Convert.ToInt32(dateSection[0]), Convert.ToInt32(dateSection[1]));
        }

        /// <summary>
        /// Convert all english digit in input string to persian digit
        /// </summary>
        /// <param name="value">string value for convert english digit. example: 1000 pen</param>
        /// <example>١٠٠٠ pen</example>
        public static string ToFarsiDigit(string value)
        {
            var dic = new Dictionary<char, char>
                          {
                              {'0','٠'},
                              {'1','١'},
                              {'2','٢'},
                              {'3','٣'},
                              {'4','٤'},
                              {'5','٥'},
                              {'6','٦'},
                              {'7','٧'},
                              {'8','٨'},
                              {'9','٩'}
                          };

            return value.Aggregate(string.Empty, (current, chr) => current + (char.IsDigit(chr) ? dic[chr] : chr));
        }

        /// <summary>
        /// Check for validate persian date
        /// </summary>
        /// <param name="farsiDate">Persian date. example: 1392/01/05 (return true) 1392/01/32 (return false)</param>
        /// <returns></returns>
        public static bool ValidateFarsiDate(string farsiDate)
        {
            if (string.IsNullOrEmpty(farsiDate) || farsiDate.All(c => c != '/'))
                return false;
            var pc = new PersianCalendar();
            var dateSection = farsiDate.Split(new[] { '/' });
            try
            {
                pc.ToDateTime(Convert.ToInt32(dateSection[0]), Convert.ToInt32(dateSection[1]),
                                 Convert.ToInt32(dateSection[2]), 0, 0, 0, 0);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private static List<DayEvent> GetAllEvents()
        {
            using (var res = Assembly.GetAssembly(typeof(FarsiDateHelper)).GetManifestResourceStream("AmirCalendar.Resources.DayEvents.xml"))
            {
                if (res == null) return null;
                var xReader = new XmlTextReader(res);
                var xDoc = XDocument.Load(xReader);
                var root = xDoc.Elements("calendars").First();
                var dayEvents = root.Elements("calendar").First().Elements().Select(c =>
                {
                    var attrMonth = c.Attribute("month");
                    var attrDay = c.Attribute("day");
                    var attrIsHoliday = c.Attribute("isHoliday");
                    var attrEvent = c.Attribute("event");
                    return (attrMonth != null && attrDay != null && attrIsHoliday != null && attrEvent != null) ? new DayEvent
                    {
                        Month = int.Parse(attrMonth.Value),
                        Day = int.Parse(attrDay.Value),
                        IsHoliday = int.Parse(attrIsHoliday.Value) == 1,
                        Event = attrEvent.Value,
                        IsPersian = true,
                    } : null;
                }).ToList();
                dayEvents.AddRange(root.Elements("calendar").Last().Elements().Select(c =>
                {
                    var attrMonth = c.Attribute("month");
                    var attrDay = c.Attribute("day");
                    var attrIsHoliday = c.Attribute("isHoliday");
                    var attrEvent = c.Attribute("event");
                    return (attrMonth != null && attrDay != null && attrIsHoliday != null && attrEvent != null) ? new DayEvent
                    {
                        Month = int.Parse(attrMonth.Value),
                        Day = int.Parse(attrDay.Value),
                        IsHoliday = int.Parse(attrIsHoliday.Value) == 1,
                        Event = attrEvent.Value,
                        IsPersian = false,
                    } : null;
                }).ToList());
                return dayEvents;
            }
        }

        private static Dictionary<int, int> GetHijriAdjustment()
        {
            if (!File.Exists(Application.StartupPath + "/HijriCalendarPatch.xml")) return null;
            using (var xReader = new XmlTextReader(Application.StartupPath + "/HijriCalendarPatch.xml"))
            {
                var xDoc = XDocument.Load(xReader);
                var root = xDoc.Elements("months").FirstOrDefault();
                if (root == null) return null;
                var monthElements = root.Elements("month").ToList();
                if (monthElements.Count != 12) return null;
                var hijriAdjustment = new Dictionary<int, int>();
                foreach (var element in monthElements)
                {
                    var attrkey = element.Attribute("key");
                    var attrvalue = element.Attribute("value");
                    if (attrkey == null || attrvalue == null)
                        return null;
                    hijriAdjustment.Add(int.Parse(attrkey.Value), int.Parse(attrvalue.Value));
                }
                return hijriAdjustment;
            }
        }
        private static List<string> GetTarikhKhasHoliday()
        {
            if (!File.Exists(Application.StartupPath + "/HijriCalendarPatch.xml")) return null;
            using (var xReader = new XmlTextReader(Application.StartupPath + "/HijriCalendarPatch.xml"))
            {
                var xDoc = XDocument.Load(xReader);
                var root = xDoc.Elements("months").FirstOrDefault();
                if (root == null) return null;
                var dateElements = root.Elements("date").ToList();
                if (dateElements.Count == 0) return null;
                var hijriAdjustment = new List<string>();
                foreach (var element in dateElements)
                {
                    var attrkey = element.Attribute("key");
                    var attrvalue = element.Attribute("value");
                    if (attrkey == null || attrvalue == null)
                        return null;
                    hijriAdjustment.Add(attrkey.Value);
                }
                return hijriAdjustment;
            }
        }
    }
}
