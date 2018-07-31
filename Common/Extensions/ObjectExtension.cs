using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Extensions
{
    /// <summary>
    /// Object 的擴充方法
    ///
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// 取得值並轉型，若無法轉型則回泛型型別的 defaulValue
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="obj">所有物件</param>
        /// <returns>泛型回傳值</returns>
        public static T ToGetValue<T>(this object obj)
        {
            return ToGetValue<T>(obj, default(T));
        }

        /// <summary>
        /// 取得值並轉型，若無法轉型則回 defaulValue
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="obj">所有物件</param>
        /// <param name="defaultValue">預設值</param>
        /// <returns>泛型回傳值</returns>
        public static T ToGetValue<T>(this object obj, T defaultValue)
        {
            //如果是字串,為null時傳回defaultValue
            if (typeof(T) == typeof(string))
            {
                T sObj = obj.ToConvert(defaultValue);
                if (sObj == null)
                {
                    return defaultValue;
                }
                return sObj;
            }

            //傳回一般值
            return obj.ToConvert(defaultValue);
        }

        /// <summary>
        /// 轉換值型別為Enum
        /// </summary>
        /// <typeparam name="TEnum">泛型Enum</typeparam>
        /// <param name="item"></param>
        /// <returns>泛型Enum</returns>
        public static TEnum ToEnumOrDefault<TEnum>(this Object item)
        {
            try
            {
                var enumerate = Enum.Parse(typeof(TEnum), item.ToString());
                return (TEnum)enumerate;
            }
            catch { return Activator.CreateInstance<TEnum>(); }
        }

        /// <summary>
        /// Object to Hashtable
        /// </summary>
        /// <param name="Contents"></param>
        /// <returns></returns>
        public static Hashtable ToHashTable(this object Contents)
        {
            Dictionary<string, object> dicSource =
               Contents.GetType()
                .GetProperties(
                    BindingFlags.Instance |
                    BindingFlags.Public
                )
                .ToDictionary(
                    prop => prop.Name,
                    prop => prop.GetValue(Contents, null)
                );

            Hashtable ht = new Hashtable();

            foreach (KeyValuePair<string, object> kvp in dicSource)
            {
                ht.AddDefualtItem(kvp.Key, kvp.Value.ToString());
            }

            return ht;
        }

        /// <summary>
        /// Object to invoke parameter
        /// </summary>
        /// <param name="Contents"></param>
        /// <returns></returns>
        public static string ToInvokeParameter(this object Contents)
        {
            Dictionary<string, object> dicSource =
               Contents.GetType()
                .GetProperties(
                    BindingFlags.Instance |
                    BindingFlags.Public
                )
                .ToDictionary(
                    prop => prop.Name,
                    prop => prop.GetValue(Contents, null)
                );

            return string.Join("&", dicSource.Select(p => p.Key + "=" + p.Value));
        }

        #region 私有方法

        /// <summary>
        /// 轉型，當轉型失敗時回傳預設值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="obj">所有物件</param>
        /// <param name="defaultValue">預設值</param>
        /// <returns>泛型回傳值</returns>
        private static T ToConvert<T>(this object obj, T defaultValue)
        {
            try
            {
                var type = typeof(T);

                //是 ValueType 且 非基本型別 且 是泛型型別。例如int?、double? 等型別
                if (type.IsValueType && !type.IsPrimitive && type.IsGenericType)
                {
                    type = type.GetGenericArguments()[0];
                }

                return (T)Convert.ChangeType(obj, type);
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion 私有方法
    }
}