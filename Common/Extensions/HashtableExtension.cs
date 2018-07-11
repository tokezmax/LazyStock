using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Common.Extensions
{
    public static partial class HashtableExtension
    {
        #region 加入元素方法
        /// <summary>
        /// 加入元素(強制取代)
        /// </summary>
        /// <param name="Self">Original Hashtable</param>
        /// <param name="Key">Key</param>
        /// <param name="Value">Value</param>
        public static void AddItem(this Hashtable Self, String Key, Object Value)
        {
            if (Self.ContainsKey(Key))
                Self.Remove(Key);
            Self.Add(Key, Value);
        }

        /// <summary>
        /// 加入多組元素(強制取代)
        /// </summary>
        /// <param name="Self">Original Hashtable</param>
        /// <param name="Keys">Key</param>
        /// <param name="Value">Value</param>
        public static void AddItems(this Hashtable Self, string[] Keys, Object Value)
        {
            foreach (String Key in Keys)
                Self.AddItem(Key, Value);
        }

        /// <summary>
        /// 加入元素(不強制取代)
        /// </summary>
        /// <param name="Self">Original Hashtable</param>
        /// <param name="Key">Key</param>
        /// <param name="Value">Value</param>
        public static void AddDefualtItem(this Hashtable Self, String Key, Object Value)
        {
            if (Self.ContainsKey(Key))
                return;
            Self.Add(Key, Value);
        }
        /// <summary>
        /// 加入多組元素(不強制取代)
        /// </summary>
        /// <param name="Self">Original Hashtable</param>
        /// <param name="Keys">Key</param>
        /// <param name="Value">Value</param>
        public static void AddDefualtItems(this Hashtable Self, string[] Keys, Object Value)
        {
            foreach (String Key in Keys)
                Self.AddDefualtItem(Key, Value);
        }

        /// <summary>
        /// Dictionary To Hashtable
        /// </summary>
        /// <param name="Self">Original Hashtable</param>
        /// <param name="Dictionary">Dictionary</param>
        public static void AddDictionary(this Hashtable Self,Dictionary<String,String> dirction)
        {
            foreach (String Key in dirction.Keys)
                Self.AddItem(Key, dirction[Key]);
        }

        #endregion

        #region 刪除元素方法
        public static void ClearEmpty(this Hashtable Self)
        {
            if(Self.Keys.Count<=0)
                return ;

            string[] Keys = new string[Self.Keys.Count];
            Self.Keys.CopyTo(Keys, 0);

            foreach (String Key in Keys)
                if (Self.ContainsKey(Key))
                    if (String.IsNullOrEmpty(Self.GetString(Key)))
                        Self.Remove(Key);
        }

        #endregion

        #region 取值方法
        /// <summary>
        /// Get Parameter and convert to String . if the exception occurred will return empty string.
        /// </summary>
        /// <param name="Self">Original Hashtable</param>
        /// <param name="Key">Key</param>
        /// <returns>String</returns>
        public static string GetString(this Hashtable Self, String Key)
        {
            try{
                if (Self.ContainsKey(Key))
                    return Self[Key].ToString();
            }catch {}
            return "";
        }

        /// <summary>
        /// Get Parameter and convert to Int32 . if the exception occurred will return 0.
        /// </summary>
        /// <param name="Self">Original Hashtable</param>
        /// <param name="Key">Key</param>
        /// <returns>Int32</returns>
        public static int GetInt(this Hashtable Self, String Key)
        {
            try{
                if (Self.ContainsKey(Key))
                    return Int32.Parse(Self[Key].ToString());
            }catch {}
            return 0;
        }
        /// <summary>
        /// Get Parameter and convert to Double . if the exception occurred will return 0.
        /// </summary>
        /// <param name="Self">Original Hashtable</param>
        /// <param name="Key">Key</param>
        /// <returns>Double</returns>
        public static Double GetDouble(this Hashtable Self, String Key)
        {
            try
            {
                if (Self.ContainsKey(Key))
                    return Double.Parse(Self[Key].ToString());
            }
            catch { }
            return 0;
        }

        /// <summary>
        /// Get Parameter and convert to long(Int64) . if the exception occurred will return 0.
        /// </summary>
        /// <param name="Self">Original Hashtable</param>
        /// <param name="Key">Key</param>
        /// <returns>long(Int64)</returns>
        public static long GetLong(this Hashtable Self, String Key)
        {
            try
            {
                if (Self.ContainsKey(Key))
                    return long.Parse(Self[Key].ToString());
            }
            catch { }
            return 0;
        }

        /// <summary>
        /// Get Parameter and convert to String[] . if the exception occurred will return null.
        /// </summary>
        /// <param name="Self">Original Hashtable</param>
        /// <param name="Key">Key</param>
        /// <returns>String[]</returns>
        public static String[] GetStrings(this Hashtable Self, String Key)
        {
            try
            {
                if (Self.ContainsKey(Key))
                    return Self[Key] as String[];
            }
            catch { }
            return (new String[] { });
        }

        /// <summary>
        /// Get Parameter and convert to Hashtable . if the exception occurred will return null.
        /// </summary>
        /// <param name="Self">Original Hashtable</param>
        /// <param name="Key">Key</param>
        /// <returns>Hashtable</returns>
        public static Hashtable GetHashtable(this Hashtable Self, String Key){
            try
            {
                if (Self.ContainsKey(Key))
                    return Self[Key] as Hashtable;
            }
            catch { }
            return (new Hashtable());
        }
        #endregion

        #region 檢核方法

        #endregion

        #region 顯示方法
        /// <summary>
        /// Print Key & Value.
        /// </summary>
        /// <param name="Self">Original Hashtable</param>
        /// <returns>[key1]value1,[key2]value2</returns>
        public static String PrintKeyAndValue(this Hashtable Self) {
            string ReturnString = "";
            foreach (String Key in Self.Keys) {
                ReturnString += "[" + Key + "]" + Self[Key].ToString() + ",";
            }
            return ReturnString.TrimEnd(',');
        }
        #endregion
    }
}
