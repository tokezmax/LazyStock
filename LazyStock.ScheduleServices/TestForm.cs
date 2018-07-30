using Common.Tools;
using isRock.LineBot;
using LazyStock.ScheduleServices.Model.Data;
using LazyStock.ScheduleServices.Services;
using LazyStock.ScheduleServices.Services.DataArchive;
using LazyStock.ScheduleServices.Services.DataProvide;
using LazyStock.ScheduleServices.Services.Notifly;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LazyStock.ScheduleServices
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                BaseCrawlerServcies DataCrawler = (BaseCrawlerServcies)Activator.CreateInstance(Type.GetType("LazyStock.ScheduleServices.Services.DeilyPriceTPEXCrawlerServcies"));
                DataCrawler.Init();
                DataCrawler.CheckIsDone();
                DataCrawler.Download();
                DataCrawler.ImportDate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Service1.UploadDeilyPriceByTWSE();
            try
            {
                BaseCrawlerServcies DataCrawler = (BaseCrawlerServcies)Activator.CreateInstance(Type.GetType("LazyStock.ScheduleServices.Services.DeilyPriceTWSECrawlerServcies"));
                DataCrawler.Init();
                DataCrawler.CheckIsDone();
                DataCrawler.Download();
                DataCrawler.ImportDate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String ErrorStr = "";
            try
            {
                string[] typenames = "DeilyPriceTWSECrawlerServcies,DeilyPriceTPEXCrawlerServcies".Split(',');
                foreach (String typeFullName in typenames)
                {
                    try
                    {
                   
                            BaseCrawlerServcies DataCrawler = (BaseCrawlerServcies)Activator.CreateInstance(Type.GetType("LazyStock.ScheduleServices.Services." + typeFullName));
                            DataCrawler.Init();
                            DataCrawler.CheckIsDone();
                            DataCrawler.Download();
                            DataCrawler.ImportDate();
                    }
                    catch (Exception eex){
                        ErrorStr += "["+ typeFullName + "]" + eex.Message+"\r\n";
                    }
                }
            }
            catch (Exception ex){
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show(ErrorStr);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Setting.ReLoad();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                BaseCrawlerServcies DataCrawler = (BaseCrawlerServcies)Activator.CreateInstance(Type.GetType("LazyStock.ScheduleServices.Services.DeilyPriceGTSMCrawlerServcies"));
                DataCrawler.Init();
                DataCrawler.CheckIsDone();
                DataCrawler.Download();
                DataCrawler.ImportDate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            StockInfoServices.UploadData();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            

            DeilyStockNotiflyServices a = new DeilyStockNotiflyServices();
            var tg = Task.Run(() => a.DoNotifly());
            tg.Wait();

            return;

            List<HighQualityListModel> QueryQuality = 
                StableStockServices.QueryQuality();

            String Context = "";
            
            int i = 0;

            String temp = DateTime.Now.ToString("yyyyMMdd")+ "(預測型)價提醒: \r\n";
            QueryQuality.ForEach(t =>
            {
                temp += $"{t.StockNum}{t.StockName} \r\n";
            });

            string channelAccessToken = "SALEdmTrZ001uP7nelpndyaRH5NPeTlEHd0QioPjNrNfOOzxfYj1QgDWZhPdePPYbrju9fe08TplcdB00qfArHWqfxs0Ob/B5jYwCmIIowTfjik14pb/EbjrdNqAdi2JIBLspjzBIYCuAdsUUsKPGwdB04t89/1O/w1cDnyilFU=";
            string AdminUserId = "U06bdebb439e5566c04d9a40bf48ecdd8";
            var bot = new Bot(channelAccessToken);
            try
            {
                List<String> users = new List<string>();
                users.Add("U06bdebb439e5566c04d9a40bf48ecdd8");
                //users.Add("U3ba8b8ce8d3c64bc338bcaddfde466f4");
                //users.Add("Uc4e3e555d10de47b5a45c64b615a1e30");
                List<String> msg = new List<string>();
                msg.Add(temp);

                //bot.PushMessage(AdminUserId, temp);
                bot.PushMulticast(users, msg);
                //MultiCastMessageAsync(users, msg);

                //https://api.line.me/v2/bot/message/multicast
            }
            catch {
                bot.PushMessage(AdminUserId, $" {DateTime.Now.ToString()} ，群發發生錯誤");
            }
        }

        /// <summary>
        /// Send push messages to multiple users at any time.
        /// Only available for plans which support push messages. Messages cannot be sent to groups or rooms
        /// https://developers.line.me/en/docs/messaging-api/reference/#send-multicast-messages
        /// </summary>
        /// <param name="to">IDs of the receivers. Max: 150 users</param>
        /// <param name="messages">Reply messages. Up to 5 messages.</param>
        public void MultiCastMessageAsync(IList<string> to, IList<string> messages)
        {
            string channelAccessToken = "SALEdmTrZ001uP7nelpndyaRH5NPeTlEHd0QioPjNrNfOOzxfYj1QgDWZhPdePPYbrju9fe08TplcdB00qfArHWqfxs0Ob/B5jYwCmIIowTfjik14pb/EbjrdNqAdi2JIBLspjzBIYCuAdsUUsKPGwdB04t89/1O/w1cDnyilFU=";
            //string AdminUserId = "U06bdebb439e5566c04d9a40bf48ecdd8";
            //HttpClient _client = new HttpClient();
            string _uri = @"https://api.line.me/v2/bot/message/multicast";
            //var request = new HttpRequestMessage(HttpMethod.Post, $"{_uri}/bot/message/multicast");
            var content = JsonConvert.SerializeObject(new { to, messages });

            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            client.Headers.Add("Authorization", channelAccessToken);
            
            var response = client.UploadString(_uri, "POST", content);
            //string resResult = Encoding.UTF8.GetString(response);
            


            /*
            //request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            //            var response =  _client.SendSync(request).ConfigureAwait(false);
            //var response = _client..UploadData(_uri, "POST", Encoding.UTF8.GetBytes(jsonText));
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                MessageBox.Show("OK");
            }else
                MessageBox.Show($"{response.StatusCode}/失敗");
            */


            //await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Common.Tools.Setting.ReLoad();
            String TraSn = Common.Tools.TokenGenerator.GetTimeStamp(3);

            try
            {
               

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
            }
        }


        private void button9_Click_1(object sender, EventArgs e)
        {
            StableStockServices.QueryHighQualityListForSlot();
        }
    }
}
