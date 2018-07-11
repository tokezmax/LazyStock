using System;
using System.Collections;
using System.Xml;
using Common.Extensions;
using Common.DataAccess;
using System.Data;
using System.Configuration;

namespace Common.Tools
{
    /// <summary>
    /// 參數設定檔(XML)
    /// </summary>
    public class Setting
    {
        private static String XmlContext = "<Config><SystemConfig><ConnectionString></ConnectionString><AppSettings></AppSettings><Package><Tools.Config><ConfigDbConnection></ConfigDbConnection></Tools.Config></Package></SystemConfig></Config>";
        private static String XmlPath = AppDomain.CurrentDomain.BaseDirectory + @"Setting.xml";

        public static Hashtable Settings = new Hashtable();

        static Setting()
        {
            ReLoad();
        }

        #region "載入方法"
        public static void ReLoad()
        {
            Settings.Clear();
            LoadByXML();
            //if (Setting.GetConfig("Package", "Tools.Config", "ConfigDbConnection") == "")
            //{
            //    return;
            //}
            //LoadByDB();
        }

        public static void LoadByXML()
        {
            lock (Settings)
            {
                CreateSettingFileInNotExist();

                #region 取得config裡的DB連線
                string _SmsPlatform_DB = (ConfigurationManager.ConnectionStrings["SmsPlatform"] == null || string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["SmsPlatform"].ConnectionString))
       ? "" : ConfigurationManager.ConnectionStrings["SmsPlatform"].ConnectionString.TrimEnd(';');

                string _MuchNewDb_DB = (ConfigurationManager.ConnectionStrings["MuchNewDb"] == null || string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["MuchNewDb"].ConnectionString))
? "" : ConfigurationManager.ConnectionStrings["MuchNewDb"].ConnectionString.TrimEnd(';');
                #endregion

                Settings.Clear();
                XmlDocument doc = new XmlDocument();
                doc.Load(XmlPath);
                XmlNode root = doc.DocumentElement;
                foreach (XmlNode nod in root.SelectNodes("/Config/*"))
                {
                    if (!nod.HasChildNodes)
                        continue;

                    Hashtable vs = new Hashtable();
                    foreach (XmlNode nods in nod.ChildNodes)
                    {

                        Hashtable vss = new Hashtable();
                        foreach (XmlNode nodss in nods.ChildNodes)
                        {
                            if (!nodss.HasChildNodes)
                                continue;
                            vss.AddItem(nodss.Name, nodss.InnerText);
                        }

                        vs.AddItem(nods.Name, vss);

                        #region 取得config裡的DB連線
                        /*
                        if (nod.Name == "SystemConfig")
                        {
                            Hashtable vss1 = new Hashtable();

                            vss1.AddItem("SmsPlatform", _SmsPlatform_DB);
                            vss1.AddItem("MuchNewDb", _MuchNewDb_DB);
                            vs.AddItem("ConnectionString", vss1);
                        }
                        */
                        #endregion
                    }

                    Settings.AddItem(nod.Name, vs);
                }

                #region 取得config裡的DB連線
                /*
                if (Settings.Count == 0)
                {
                    Hashtable vs1 = new Hashtable();
                    Hashtable vss1 = new Hashtable();

                    vss1.AddItem("SmsPlatform", _SmsPlatform_DB);
                    vss1.AddItem("MuchNewDb", _MuchNewDb_DB);

                    vs1.AddItem("ConnectionString", vss1);
                    Settings.AddItem("SystemConfig", vs1);
                }
                */
                #endregion
            }
        }

        /// <summary>
        /// 載入參數
        /// </summary>
        public static void LoadByDB()
        {
            lock (Settings)
            {
                Hashtable values = new Hashtable();
                values.Add("Groups", "");
                values.Add("Category", "");
                values.Add("Keys", "");
                values.Add("Value", "");
                values.Add("ShowYN", "");
                DataTable dt = Dao.SelectUSP("dbo.uspGetConfigsList", values, null, Setting.ConnectionString("SmsPlatform"));
                //DataTable dt = Dao.SelectUSP("dbo.uspGetConfigsList", values, null, Setting.GetConfig("Package", "Tools.Config", "ConfigDbConnection"));
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow Row in dt.Rows)
                    {
                        String Groups = Row["Groups"].ToString();
                        String Category = Row["Category"].ToString();
                        String Keys = Row["Keys"].ToString();
                        String Value = Row["Value"].ToString();

                        Settings.AddDefualtItem(Groups, new Hashtable());
                        Settings.GetHashtable(Groups).AddDefualtItem(Category, new Hashtable());
                        Settings.GetHashtable(Groups).GetHashtable(Category).AddItem(Keys, Value);

                    }
                }
                dt.Dispose();
            }
        }
        #endregion

        #region "參數讀寫方法"
        /// <summary>
        /// 取得(Groups)參數集合
        /// </summary>
        /// <param name="Groups">Groups Name</param>
        /// <returns>Hashtable<string,Hashtable></returns>
        public static Hashtable GetGroups(String Groups)
        {
            return Settings.GetHashtable(Groups);
        }
        /// <summary>
        /// 取得(Groups)->(Category)參數集合
        /// </summary>
        /// <param name="Groups">Groups Name</param>
        /// <param name="Category">Category Name</param>
        /// <returns>Hashtable<string,string></returns>
        public static Hashtable GetCategory(String Groups, String Category)
        {
            return Settings.GetHashtable(Groups).GetHashtable(Category);
        }
        /// <summary>
        /// 取得參數
        /// </summary>
        /// <param name="Groups">Groups Name</param>
        /// <param name="Category">Category Name</param>
        /// <param name="Keys">Config Key</param>
        /// <returns>String</returns>
        public static String GetConfig(String Groups, String Category, String Keys, bool GetNull = false)
        {
            var rVal = Settings.GetHashtable(Groups).GetHashtable(Category).GetString(Keys);
            if (GetNull && rVal == "")
                return null;
            else
                return rVal;
        }

        /// <summary>
        /// 取得ConnectionString
        /// </summary>
        /// <param name="Key">ConnectionStrings</param>
        /// <returns>連線字串</returns>
        public static String ConnectionString(String Key)
        {
            return Settings.GetHashtable("SystemConfig").GetHashtable("ConnectionString").GetString(Key);
        }
        /// <summary>
        /// 取得 AppSettings
        /// </summary>
        /// <param name="Key">AppSettings</param>
        /// <returns>指定參數之值</returns>
        public static String AppSettings(String Key)
        {
            return Settings.GetHashtable("SystemConfig").GetHashtable("AppSettings").GetString(Key);
        }

        /// <summary>
        /// 取得參數
        /// </summary>
        /// <param name="Groups">Groups Name</param>
        /// <param name="Category">Category Name</param>
        /// <param name="Keys">Config Key</param>
        /// <param name="Value">Value</param>
        /// <returns>String</returns>
        public static void SetConfig(String Groups, String Category, String Keys, String Value)
        {
            lock (GetGroups(Groups))
                GetGroups(Groups).GetHashtable(Category).AddItem(Keys, Value);
        }
        /// <summary>
        /// 若查無檔案，即自動產生
        /// </summary>
        private static void CreateSettingFileInNotExist()
        {
            if (System.IO.File.Exists(XmlPath))
                return;
            System.IO.File.WriteAllText(XmlPath, XmlContext, System.Text.Encoding.UTF8);
        }
        #endregion
    }
}