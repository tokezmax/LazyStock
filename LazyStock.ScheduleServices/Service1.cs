using LazyStock.ScheduleServices.Services;
using LazyStock.ScheduleServices.Services.DataArchive;
using LazyStock.ScheduleServices.Services.Notifly;
using System;
using System.ServiceProcess;
using System.Timers;

namespace LazyStock.ScheduleServices
{
    public partial class Service1 : ServiceBase
    {
        public static bool IsBusy = false;

        public Service1()
        {
            InitializeComponent();
            IsBusy = false;
            Common.Tools.Setting.ReLoad();
        }

        protected override void OnStart(string[] args)
        {
            //建立一個timer
            System.Timers.Timer MyTimer = new System.Timers.Timer();
            //Elapsed代表，timer設定的時間到後要做什麼事情
            MyTimer.Elapsed += new ElapsedEventHandler(doSomething);
            int RunInterval = (60 * 60 * 1000); //預設1小時
            try
            {
                RunInterval = Int32.Parse(Common.Tools.Setting.AppSettings("RunSec")) * 1000;
            }
            catch { }
            //做什麼事情可以寫成method丟進去，sample為寫log
            MyTimer.Interval = RunInterval; //代表時間間隔，單位為毫秒，設定2000代表2秒
            MyTimer.Start(); //啟動timer
        }

        private void doSomething(object sender, ElapsedEventArgs e) //寫log
        {
            Common.Tools.Setting.ReLoad();
            String TraSn = Common.Tools.TokenGenerator.GetTimeStamp(3);

            try
            {
                if (IsBusy)
                {
                    Common.Tools.LogHelper.doLog("Common", "[" + TraSn + "]IsBusy");
                    return;
                }

                string[] typenames = "DeilyPriceTWSECrawlerServcies,DeilyPriceTPEXCrawlerServcies,DeilyPriceGTSMCrawlerServcies".Split(',');
                foreach (String typeFullName in typenames)
                {
                    Common.Tools.LogHelper.doLog(typeFullName, "[" + TraSn + "]Go");

                    try
                    {
                        BaseCrawlerServcies DataCrawler = (BaseCrawlerServcies)Activator.CreateInstance(Type.GetType("LazyStock.ScheduleServices.Services." + typeFullName));
                        DataCrawler.Init();
                        DataCrawler.CheckIsDone();
                        DataCrawler.Download();
                        DataCrawler.ImportDate();
                    }
                    catch (Exception eex)
                    {
                        Common.Tools.LogHelper.doLog(typeFullName, "[Exception][" + TraSn + "]" + eex.Message);
                    }

                    Common.Tools.LogHelper.doLog(typeFullName, "[" + TraSn + "]Done");
                }

                //CalStockInfo資料備份
                (new CalStockInfoArchiveServices()).GenCalStockInfoArchive();
                (new DeilyStockNotiflyServices()).DoNotifly();
            }
            catch (Exception ex)
            {
                Common.Tools.LogHelper.doLog("Common", "[Exception][" + TraSn + "]" + ex.Message);
            }
            finally
            {
                Common.Tools.LogHelper.doLog("Common", "[" + TraSn + "]UnBusy");
                IsBusy = false;
            }
        }

        protected override void OnStop()
        {
        }
    }
}