using System;
using System.Text.RegularExpressions;

namespace Common.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// 簡化 String.IsNullOrEmpty 的使用方式
        /// </summary>
        /// <param name="value">字串</param>
        /// <returns>True：為null或空值</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 簡化 String.IsNullOrWhiteSpace 的使用方式
        /// </summary>
        /// <param name="value">字串</param>
        /// <returns>True：為null或空值或空白</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 替換字串。使用方式：將要替換的字串用[[ xxx ]] 包住，並傳入對應屬性之物件。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="data">有對應屬性之物件</param>
        /// <returns>替代後的文字</returns>
        public static string TemplateSubstitute(this string input, object data)
        {
            var type = data.GetType();
            return Regex.Replace(input, @"\[\[(\w+)\]\]", m =>
            {
                var name = m.Groups[1].Value;
                var prop = type.GetProperty(name);

                return prop != null ? prop.GetValue(data, null).ToString() : m.Value;
            });
        }

        #region 日期相關

        /// <summary>
        /// 試轉型為DateTime後取得yyyy/MM/dd格式字串, 若失敗則傳回String.Empty
        /// </summary>
        /// <param name="dateTimeString">日期格式的字串</param>
        /// <returns>yyyy/MM/dd格式字串</returns>
        public static string ToFullDateString(this string dateTimeString)
        {
            return dateTimeString.ToFullDateString(string.Empty);
        }

        /// <summary>
        /// 試轉型為DateTime後取得yyyy/MM/dd格式字串, 若失敗則傳回defaultValue
        /// </summary>
        /// <param name="dateTimeString">日期格式的字串</param>
        /// <param name="defaultValue">預設值</param>
        /// <returns>yyyy/MM/dd格式字串</returns>
        public static string ToFullDateString(this string dateTimeString, string defaultValue)
        {
            try
            {
                var dt = DateTime.Parse(dateTimeString);
                return dt.GetFullDate();
            }
            catch { return defaultValue; }
        }

        /// <summary>
        /// 試轉型為DateTime後取得yyyy/MM/dd HH:mm:ss格式字串, 若失敗則傳回String.Empty
        /// </summary>
        /// <param name="dateTimeString">日期格式的字串</param>
        /// <returns>yyyy/MM/dd HH:mm:ss格式字串</returns>
        public static string ToFullDateTimeString(this string dateTimeString)
        {
            return dateTimeString.ToFullDateTimeString(string.Empty);
        }

        /// <summary>
        /// 試轉型為DateTime後取得yyyy/MM/dd HH:mm:ss格式字串, 若失敗則傳回defaultValue
        /// </summary>
        /// <param name="dateTimeString">日期格式的字串</param>
        /// <param name="defaultValue">預設值</param>
        /// <returns>yyyy/MM/dd HH:mm:ss格式字串</returns>
        public static string ToFullDateTimeString(this string dateTimeString, string defaultValue)
        {
            try
            {
                var dt = DateTime.Parse(dateTimeString);
                return dt.GetFullDateTime();
            }
            catch { return defaultValue; }
        }

        #endregion 日期相關

        private const RegexOptions RegexOptions = System.Text.RegularExpressions.RegexOptions.ECMAScript | System.Text.RegularExpressions.RegexOptions.Compiled;

        private static Regex _regexUserName = new Regex(@"(\w+\\)?(?<name>(\w|\W)+)", RegexOptions);

        /// <summary>取"\"之後文字，無則傳原本文字
        /// <para>適用於User.Identity.Name 只取 name</para></summary>
        /// <param name="str">字串</param>
        /// <param name="pattern"></param>
        /// <returns>取"\"之後文字，無則傳原本文字</returns>
        public static string GetUserName(this string str, string pattern = null)
        {
            if (!string.IsNullOrWhiteSpace(pattern))
                _regexUserName = new Regex(pattern, RegexOptions);

            return string.IsNullOrWhiteSpace(str) ? str : _regexUserName.Match(str).Groups["name"].Value;
        }

        public static T ToEnum<T>(this string value, bool ignoreCase = true)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }
    }
}