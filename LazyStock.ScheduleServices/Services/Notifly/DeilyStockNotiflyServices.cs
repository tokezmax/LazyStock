using Common.Tools;
using isRock.LineBot;
using LazyStock.ScheduleServices.Model.Data;
using LazyStock.ScheduleServices.Services.DataProvide;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LazyStock.ScheduleServices.Services.Notifly
{
    internal class DeilyStockNotiflyServices
    {
        public String CheckDoneDirPath = "";
        public String CheckDoneFileName = "";
        public String CheckDoneFullPath = "";
        protected DateTime GetDate;

        public DeilyStockNotiflyServices()
        {
            int DaysDiff = -1;
            if (DateTime.Now.Hour >= 19)
                DaysDiff = 0;
            if (!String.IsNullOrEmpty(Setting.AppSettings("SpecifiedDate")))
                DaysDiff = Int32.Parse(Setting.AppSettings("SpecifiedDate"));
            GetDate = DateTime.Now.AddDays(DaysDiff);

            CheckDoneDirPath = AppDomain.CurrentDomain.BaseDirectory + @"\CheckDone\DeilyStockNotiflyServices\";
            CheckDoneFileName = GetDate.ToString("yyyyMMdd") + ".txt";
            CheckDoneFullPath = CheckDoneDirPath + CheckDoneFileName;
        }

        public bool CheckIsDone()
        {
            return (System.IO.File.Exists(CheckDoneFullPath));
        }

        public void DoNotifly()
        {
            string AdminUserId = Setting.AppSettings("LineAdminUserId");
            string channelAccessToken = Setting.AppSettings("LineChannelAccessToken");
            var bot = new Bot(channelAccessToken);
            try
            {
                if (CheckIsDone())
                {
                    LogHelper.doLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, "執行過了，do Nothing");
                    return;
                }

                List<HighQualityListModel> QueryQuality =
                   StableStockServices.QueryQuality();

                String StableTitle = $"==={GetDate.ToString("yyyy-MM-dd")}===\n保守型-到價提醒: \n ";
                String UnStableTitle = $"==={GetDate.ToString("yyyy-MM-dd")}===\n預估型-到價提醒: \n ";
                String StableContext = "";
                String UnStableContext = "";

                String LZUrl = "\nhttp://lazystock.azurewebsites.net";
                QueryQuality.OrderBy(x => x.StockNum).ToList().ForEach(t =>
                  {
                      if (t.StableIsBuy == "1")
                          StableContext += $"{t.StockNum}{t.StockName} \n ";
                      else if (t.UnStableIsBuy == "1")
                          UnStableContext += $"{t.StockNum}{t.StockName} \n ";
                  });

                //List<String> users = new List<string>();
                //string _userid = "";
                foreach (String userid in Setting.AppSettings("LineNotiflyUserIds").Split(','))
                {
                    if (String.IsNullOrEmpty(userid))
                        continue;
                    if (!String.IsNullOrEmpty(StableContext))
                        bot.PushMessage(userid, StableTitle + StableContext + LZUrl);
                    if (!String.IsNullOrEmpty(UnStableContext))
                        bot.PushMessage(userid, UnStableTitle + UnStableContext + LZUrl);
                }

                if (!System.IO.Directory.Exists(CheckDoneDirPath))
                    System.IO.Directory.CreateDirectory(CheckDoneDirPath);

                System.IO.File.WriteAllText(this.CheckDoneFullPath, "done");

                LogHelper.doLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, "發送成功!");
            }
            catch (Exception e)
            {
                LogHelper.doLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, "[Exception]" + e.Message);
                try
                {
                    bot.PushMessage(AdminUserId, $" {DateTime.Now.ToString()} ，群發發生錯誤");
                }
                catch { }
            }
        }
    }
}