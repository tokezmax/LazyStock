using System;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using Newtonsoft.Json;

namespace Common.Extensions
{
    public static class DateTableExtension
    {
        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List with generic objects</returns>
        public static List<T> ToList<T>(this DataTable table) where T : class, new(){
            try{
                List<T> list = new List<T>();
                foreach (var row in table.AsEnumerable()){
                    T obj = new T();
                    foreach (var prop in obj.GetType().GetProperties()){
                        try{
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }catch{
                            continue;
                        }
                    }
                    list.Add(obj);
                }
                return list;
            }catch{
                return null;
            }
        }
        /// <summary>
        /// Converts a DataTable to Object ,Only ColName and Value.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static Object ToObject(this DataTable table) 
        {
            try
            {
                return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(table));
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Converts a DataTable to Object ,Only ColName and Value.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static String[] GetColNames(this DataTable table)
        {
            try
            {
                return table.Columns.Cast<DataColumn>()
                                     .Select(x => x.ColumnName)
                                     .ToArray();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Paging Datatable
        /// </summary>
        /// <param name="table"></param>
        /// <param name="SkipCount"></param>
        /// <param name="TakeCount"></param>
        /// <returns></returns>
        public static DataTable GetPaging(this DataTable table, int SkipCount=0, int TakeCount=0) { 
            
            if(TakeCount <=0 || table.Rows.Count<=0)
                return table;

            if(table.Rows.Count <= SkipCount)
                return table;

            return table.AsEnumerable().Skip(SkipCount).Take(TakeCount).CopyToDataTable();
        }
    }
}
