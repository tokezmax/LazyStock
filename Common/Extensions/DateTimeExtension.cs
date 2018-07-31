using System;
using System.Globalization;

namespace Common.Extensions
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 取得yyyy/MM/dd HH:mm:ss格式字串
        /// </summary>
        /// <param name="datetime">日期</param>
        /// <returns>yyyy/MM/dd HH:mm:ss格式字串</returns>
        public static string GetFullDateTime(this DateTime datetime, String DateSplitChar = "/")
        {
            return datetime.ToString("yyyy" + DateSplitChar + "MM" + DateSplitChar + "dd HH:mm:ss");
        }

        /// <summary>
        /// 取得yyyy/MM/dd格式字串
        /// </summary>
        /// <param name="datetime">日期</param>
        /// <returns>yyyy/MM/dd格式字串</returns>
        public static string GetFullDate(this DateTime datetime, String DateSplitChar = "/")
        {
            return datetime.ToString("yyyy" + DateSplitChar + "MM" + DateSplitChar + "dd");
        }

        /// <summary>
        /// To the full taiwan date.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns></returns>
        public static string ToFullTaiwanDate(this DateTime datetime)
        {
            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            return string.Format("民國 {0} 年 {1} 月 {2} 日",
                taiwanCalendar.GetYear(datetime),
                datetime.Month,
                datetime.Day);
        }

        /// <summary>
        /// To the simple taiwan date.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns></returns>
        public static string ToSimpleTaiwanDate(this DateTime datetime)
        {
            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            return string.Format("{0}/{1}/{2}",
                taiwanCalendar.GetYear(datetime),
                datetime.ToString("MM"),
                datetime.ToString("dd"));
        }
    }
}