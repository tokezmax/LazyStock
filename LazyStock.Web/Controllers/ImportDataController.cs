using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.Extensions;
using Common.Tools;
using Newtonsoft.Json;
using LazyStock.Web.Models;
using System.Collections;
using Newtonsoft.Json.Linq;
using LiteDB;
using System.Threading.Tasks;
using System.Text;
using System.IO;

namespace LazyStock.Web.Controllers
{
    public class ImportDataController : Controller
    {
        #region
        /// <summary>
        /// 資料庫存放位置
        /// </summary>
        public static String LazyStockDBPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\LazyStockDB.db";

        public static String SqlStockPrice = "SELECT [StockNum],[StockPrice],[PER],[PBR] FROM [Stock].[dbo].[StockPrice] ";

        #endregion




        public ActionResult UploadDeilyPrice(String StockNum = "")
        {
            BaseResModel result = new BaseResModel();
            try
            {
                System.Data.DataTable dt = Common.DataAccess.Dao.QueryDataTable(SqlStockPrice, null, Common.Tools.Setting.ConnectionString("Stock"));

                List<StockPriceDataModel> StockPrices = new List<StockPriceDataModel>();
                StockPrices = dt.ToList<StockPriceDataModel>();

                SetStockPriceReqModel StockPricesReq = new SetStockPriceReqModel();
                StockPricesReq.StockPrices = (from a in StockPrices
                                              select new StockPrice()
                                              {
                                                  StockNum = a.StockNum,
                                                  Price = a.StockPrice
                                              }).ToList<StockPrice>();
                LogHelper.doLog("a", "==準備發送==\r\n" + StockPricesReq.StockPrices.Count.ToString() + "筆");

                for (int i = 0; i < StockPricesReq.StockPrices.Count; i = i + 100)
                {
                    var items = StockPricesReq.StockPrices.Skip(i).Take(100);

                    SetStockPriceReqModel StockPricesReq2 = new SetStockPriceReqModel();
                    StockPricesReq2.StockPrices = items.ToList<StockPrice>();
                    var jsonText = "{\"ReqParam\" : " + JsonConvert.SerializeObject(StockPricesReq2) + @"}";
                    LogHelper.doLog("a", "==發送(" + StockPricesReq2.StockPrices.Count().ToString() + "筆)==\r\n" + jsonText);
                    WebClient client = new WebClient();
                    client.Encoding = Encoding.UTF8;
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    var response = client.UploadData(Common.Tools.Setting.AppSettings("UploadByDeilyURL"), "POST", Encoding.UTF8.GetBytes(jsonText));
                    string resResult = Encoding.UTF8.GetString(response);
                    result = JsonConvert.DeserializeObject<BaseResModel>(resResult, new JsonSerializerSettings { MetadataPropertyHandling = MetadataPropertyHandling.Ignore });
                    LogHelper.doLog("a", "==接收==\r\n" + resResult);
                }

                result.Code = ResponseCodeEnum.Success;
                result.Message = "";//"[Key]" + Key + "[Val]" + Val;
            }
            catch (Exception e)
            {
                result.Code = ResponseCodeEnum.Failed;
                result.Message = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Getdate(String StockNum = "")
        {
            BaseResModel result = new BaseResModel();
            try
            {
                WebClient client = new WebClient();
                client.Headers.Add(HttpRequestHeader.Accept, "*/*");
                //client.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                //client.Encoding = Encoding.UTF8;

                
                client.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded;");
                client.Headers.Add(HttpRequestHeader.Cookie, "CLIENT%5FID=20180524212902554%5F123%2E0%2E41%2E147; _ga=GA1.2.896448925.1527168533; __gads=ID=445441048ff5fa66:T=1527168539:S=ALNI_MZYoUT6OWFQ_RItzuGYgFpPUJgWBg; _gid=GA1.2.164475403.1531229152; SCREEN_SIZE=WIDTH=1920&HEIGHT=1200; OTHER=FROM%5FURL=https%3A%2F%2Fgoodinfo%2Etw%2FStockInfo%2FStockList%2Easp%3FSHEET%3D%25E5%25B9%25B4%25E7%258D%25B2%25E5%2588%25A9%25E8%2583%25BD%25E5%258A%259B%26MARKET%5FCAT%3D%25E7%2586%25B1%25E9%2596%2580%25E6%258E%2592%25E8%25A1%258C%26INDUSTRY%5FCAT%3D%25E5%25B9%25B4%25E5%25BA%25A6EPS%25E6%259C%2580%25E9%25AB%2598; LOGIN=EMAIL=%E3%80%80&USER%5FNM=Wen+Zhong+Chen&ACCOUNT%5FID=1828404013838099&ACCOUNT%5FVENDOR=Facebook&NO%5FEXPIRE=T; _gat=1");
                client.Headers.Add("origin", "https://goodinfo.tw");
                client.Headers.Add("referer", "https://goodinfo.tw/StockInfo/StockList.asp?SHEET=%E5%B9%B4%E7%8D%B2%E5%88%A9%E8%83%BD%E5%8A%9B&MARKET_CAT=%E7%86%B1%E9%96%80%E6%8E%92%E8%A1%8C&INDUSTRY_CAT=%E5%B9%B4%E5%BA%A6EPS%E6%9C%80%E9%AB%98");
                client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36");
                String aaaaapath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\test.html";

                string url = "https://goodinfo.tw/StockInfo/StockList.asp?SEARCH_WORD=&SHEET=%E7%87%9F%E6%94%B6%E7%8B%80%E6%B3%81&SHEET2=%E6%9C%88%E7%87%9F%E6%94%B6%E7%8B%80%E6%B3%81&MARKET_CAT=%E7%86%B1%E9%96%80%E6%8E%92%E8%A1%8C&INDUSTRY_CAT=%E5%B9%B4%E5%BA%A6EPS%E6%9C%80%E9%AB%98@@%E6%AF%8F%E8%82%A1%E7%A8%85%E5%BE%8C%E7%9B%88%E9%A4%98+(EPS)@@%E5%B9%B4%E5%BA%A6EPS%E6%9C%80%E9%AB%98";
                string urlb = "http://localhost:2458/default.aspx";
                client.DownloadFile(url, aaaaapath);
                //LogHelper.doLog("b", a);

                //DownloadByTwse("2317", "20180120");
                //http://www.twse.com.tw/exchangeReport/MI_INDEX?response=csv&date=20180710&type=ALLBUT0999

            }
            catch (Exception e)
            {
                result.Code = ResponseCodeEnum.Failed;
                result.Message = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }



        #region 即時股價取得
        public void DownloadByTwse(string STOCK_CODE, string date)
        {
            Console.WriteLine("更新股價：" + STOCK_CODE);

            string download_url = "http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=csv&date=" + date + "&stockNo=" + STOCK_CODE;

            // 系統睡10秒, 避免快速呼叫而被證交所擋ip
            System.Threading.Thread.Sleep(10000);
            string downloadedData = "";
            using (WebClient wClient = new WebClient())
            {
                try
                {
                    downloadedData = wClient.DownloadString(download_url);
                }
                catch (WebException ex)
                {
                    Console.WriteLine("更新股價失敗：" + STOCK_CODE + " " + ex.Message);
                }
            }
            if (downloadedData.Trim().Length == 0)
            {
                return;
            }
            // 證交所一次是回應一整個月的資料
            CSVReader csv = new CSVReader();
            string[] lineStrs = downloadedData.Split('\n');
            for (int i = 0; i < lineStrs.Length; i++)
            {
                string strline = lineStrs[i];
                if (i == 0 || i == 1 || strline.Trim().Length == 0)
                {
                    continue;
                }
                if (strline.IndexOf("說明:") > -1 || strline.IndexOf("符號說明") > -1 || strline.IndexOf("當日統計資訊含一般") > -1 || strline.IndexOf("ETF證券代號") > -1)
                {
                    continue;
                }

                ArrayList result = new ArrayList();
                csv.ParseCSVData(result, strline);
                string[] datas = (string[])result.ToArray(typeof(string));

                //檢查資料內容
                if (Convert.ToInt32(datas[1].Replace(",", "")) == 0 || datas[3] == "--" || datas[4] == "--" || datas[5] == "--" || datas[6] == "--")
                {
                    continue;
                }

                string code = STOCK_CODE; //代號
                string date2 = datas[0]; //日期
                string open_price = datas[3];//開盤價
                string high_price = datas[4]; //最高價
                string low_price = datas[5]; //最低價
                string close_price = datas[6]; //收盤價
                string volume = datas[1]; //成交股數

                // 以下應用請自行處理
            }
        }
        #endregion
    }
}
public class CSVReader
{
    private Stream objStream;
    private StreamReader objReader;

    //add name space System.IO.Stream
    public CSVReader()
    {

    }
    public CSVReader(Stream filestream) : this(filestream, null) { }
    public CSVReader(StreamReader strReader)
    {
        this.objReader = strReader;
    }
    public CSVReader(Stream filestream, Encoding enc)
    {
        this.objStream = filestream;
        //check the Pass Stream whether it is readable or not
        if (!filestream.CanRead)
        {
            return;
        }
        objReader = (enc != null) ? new StreamReader(filestream, enc) : new StreamReader(filestream);
    }
    //parse the Line
    public string[] GetCSVLine()
    {
        string data = objReader.ReadLine();
        if (data == null) return null;
        if (data.Length == 0) return new string[0];
        //System.Collection.Generic
        ArrayList result = new ArrayList();
        //parsing CSV Data
        ParseCSVData(result, data);
        return (string[])result.ToArray(typeof(string));
    }

    public void ParseCSVData(ArrayList result, string data)
    {
        int position = -1;
        while (position < data.Length)
            result.Add(ParseCSVField(ref data, ref position));
    }

    private string ParseCSVField(ref string data, ref int StartSeperatorPos)
    {
        if (StartSeperatorPos == data.Length - 1)
        {
            StartSeperatorPos++;
            return "";
        }

        int fromPos = StartSeperatorPos + 1;
        if (data[fromPos] == '"')
        {
            int nextSingleQuote = GetSingleQuote(data, fromPos + 1);
            int lines = 1;
            while (nextSingleQuote == -1)
            {
                data = data + "\n" + objReader.ReadLine();
                nextSingleQuote = GetSingleQuote(data, fromPos + 1);
                lines++;
                if (lines > 20)
                    throw new Exception("lines overflow: " + data);
            }
            StartSeperatorPos = nextSingleQuote + 1;
            string tempString = data.Substring(fromPos + 1, nextSingleQuote - fromPos - 1);
            tempString = tempString.Replace("'", "''");
            return tempString.Replace("\"\"", "\"");
        }

        int nextComma = data.IndexOf(',', fromPos);
        if (nextComma == -1)
        {
            StartSeperatorPos = data.Length;
            return data.Substring(fromPos);
        }
        else
        {
            StartSeperatorPos = nextComma;
            return data.Substring(fromPos, nextComma - fromPos);
        }
    }

    private int GetSingleQuote(string data, int SFrom)
    {
        int i = SFrom - 1;
        while (++i < data.Length)
            if (data[i] == '"')
            {
                if (i < data.Length - 1 && data[i + 1] == '"')
                {
                    i++;
                    continue;
                }
                else
                    return i;
            }
        return -1;
    }
}