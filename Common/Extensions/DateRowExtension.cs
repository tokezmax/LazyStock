using System;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Newtonsoft.Json;

namespace Common.Extensions
{
    public static class DateRowExtension
    {
        #region 取值方法
        /// <summary>
        /// Get Parameter and convert to String . if the exception occurred will return empty string.
        /// </summary>
        /// <param name="Self">Original Hashtable</param>
        /// <param name="Key">Key</param>
        /// <returns>String</returns>
        public static string GetString(this DataRow Self, String FieldName)
        {
            try{
                if (!DBNull.Value.Equals(Self[FieldName]))
                    return Self[FieldName].ToString();
                return String.Empty;
            }
            catch {
                return "";
            }
        }

        /// <summary>
        /// Get Parameter and convert to Int32 . if the exception occurred will return 0.
        /// </summary>
        /// <param name="Self">Original Hashtable</param>
        /// <param name="Key">Key</param>
        /// <returns>Int32</returns>
        public static int GetInt32(this DataRow Self, String FieldName)
        {
            try
            {
                if (!DBNull.Value.Equals(Self[FieldName]))
                    return Convert.ToInt32(Self[FieldName].ToString());
                return 0;
            }
            catch {
                return 0;
            }
        }
        /// <summary>
        /// Get Parameter and convert to Double . if the exception occurred will return 0.
        /// </summary>
        /// <param name="Self">Original Hashtable</param>
        /// <param name="Key">Key</param>
        /// <returns>Double</returns>
        public static Double GetDouble(this DataRow Self, String FieldName)
        {
            try
            {
                if (!DBNull.Value.Equals(Self[FieldName]))
                    return Convert.ToDouble(Self[FieldName].ToString());
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Get Parameter and convert to long(Int64) . if the exception occurred will return 0.
        /// </summary>
        /// <param name="Self">Original Hashtable</param>
        /// <param name="Key">Key</param>
        /// <returns>long(Int64)</returns>
        public static Double GetLong(this DataRow Self, String FieldName)
        {
            try
            {
                if (!DBNull.Value.Equals(Self[FieldName]))
                    return long.Parse(Self[FieldName].ToString());
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Get Parameter and convert to DateTime . if the exception occurred will return null.
        /// </summary>
        /// <param name="Self">Original Hashtable</param>
        /// <param name="Key">Key</param>
        /// <returns>String</returns>
        public static DateTime? GetDateTime(this DataRow Self, String FieldName)
        {
            try
            {
                if (!DBNull.Value.Equals(Self[FieldName]))
                    return DateTime.Parse(Self[FieldName].ToString());

                return null;
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
