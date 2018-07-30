using Common.Tools;
using LazyStock.ScheduleServices.EFModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyStock.ScheduleServices.Services.DataArchive
{
    class CalStockInfoArchiveServices
    {
        public  String CheckDoneDirPath = "";
        public  String CheckDoneFileName = "";
        public  String CheckDoneFullPath = "";
        protected  DateTime GetDate;

        public CalStockInfoArchiveServices()
        {
            int DaysDiff = -1;
            if (DateTime.Now.Hour >= 19)
                DaysDiff = 0;
            if (!String.IsNullOrEmpty(Setting.AppSettings("SpecifiedDate")))
                DaysDiff = Int32.Parse(Setting.AppSettings("SpecifiedDate"));
            GetDate = DateTime.Now.AddDays(DaysDiff);

            CheckDoneDirPath = AppDomain.CurrentDomain.BaseDirectory + @"\CheckDone\StockInfoArchive\";
            CheckDoneFileName = GetDate.ToString("yyyyMMdd") + ".txt";
            CheckDoneFullPath = CheckDoneDirPath + CheckDoneFileName;
        }

        public  void CheckIsDone()
        {
            if (System.IO.File.Exists(CheckDoneFullPath))
                throw new Exception("[" + GetDate.ToString("yyyyMMdd") + "]Before Done, do nothing!");
        }

        public void GenCalStockInfoArchive()
        {
            try
            {
                CheckIsDone();

                LogHelper.doLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, "Start Done " + GetDate.ToString("yyyy-MM-dd"));
                using (StockEntities db = new StockEntities())
                {
                    db.Database.ExecuteSqlCommand("GenCalStockInfoArchive @BackupDate ", new SqlParameter("BackupDate", GetDate.ToString("yyyy-MM-dd")));
                }

                if (!System.IO.Directory.Exists(CheckDoneDirPath))
                    System.IO.Directory.CreateDirectory(CheckDoneDirPath);

                System.IO.File.WriteAllText(this.CheckDoneFullPath, "done");

                LogHelper.doLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, "Archive Done");
            }
            catch (Exception e)
            {
                LogHelper.doLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, "[Exception]" + e.Message);
            }
        }
    }
}
