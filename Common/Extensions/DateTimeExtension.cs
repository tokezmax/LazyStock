using System;

namespace Common.Extensions
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 取得yyyy/MM/dd HH:mm:ss格式字串
        /// </summary>
        /// <param name="datetime">日期</param>
        /// <returns>yyyy/MM/dd HH:mm:ss格式字串</returns>
        public static string GetFullDateTime(this DateTime datetime,String DateSplitChar = "/")
        {
            return datetime.ToString("yyyy" + DateSplitChar + "MM" + DateSplitChar + "dd HH:mm:ss");
        }
        /// <summary>
        /// 取得yyyy/MM/dd格式字串
        /// </summary>
        /// <param name="datetime">日期</param>
        /// <returns>yyyy/MM/dd格式字串</returns>
        public static string GetFullDate(this DateTime datetime,String DateSplitChar = "/")
        {
            return datetime.ToString("yyyy" + DateSplitChar + "MM" + DateSplitChar + "dd");
        }


    }
}
