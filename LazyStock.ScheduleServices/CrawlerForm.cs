using Common.Tools;
using LazyStock.ScheduleServices.EFModel;
using LazyStock.ScheduleServices.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
using System.Data.SqlClient;

namespace LazyStock.ScheduleServices
{
    public partial class CrawlerForm : Form
    {
        public CrawlerForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String DownloadDirPath = AppDomain.CurrentDomain.BaseDirectory + @"\DataSource\TWSEStockPriceEveryDay\";
            String DownloadFileName = TokenGenerator.GetTimeStamp(3) + ".csv";
            String DownloadFullPath = DownloadDirPath + DownloadFileName;

            if (!System.IO.Directory.Exists(DownloadDirPath))
                System.IO.Directory.CreateDirectory(DownloadDirPath);

            for (int i=1095; i>0; i--) {
                String YYYYMMDD = DateTime.Now.AddDays( (0 - i)).ToString("yyyyMMdd");

                DownloadDirPath = AppDomain.CurrentDomain.BaseDirectory + @"\DataSource\TWSEStockPriceEveryDay\";
                DownloadFileName = YYYYMMDD + ".csv";
                DownloadFullPath = DownloadDirPath + DownloadFileName;
                if (System.IO.File.Exists(DownloadFullPath))
                    continue;

                String DownloadUrl = Setting.AppSettings("TWSEStockPriceURL").Replace("@yyyyMMdd", YYYYMMDD);
                String DownloadData = "";
                using (WebClient wClient = new WebClient())
                    DownloadData = wClient.DownloadString(DownloadUrl);

                //if (DownloadData.IndexOf(@"證券代號") < 0)
                    //throw new Exception("錯誤的資料格式!");

                //DownloadData = DownloadData.Substring(DownloadData.IndexOf(@"證券代號") - 1).Replace("=\"", "\"");
                System.IO.File.WriteAllText(DownloadFullPath, DownloadData);

                System.Threading.Thread.Sleep(3000);
                listBox1.Items.Add(YYYYMMDD);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String DownloadDirPath2 = AppDomain.CurrentDomain.BaseDirectory + @"\DataSource\TWSEStockPriceEveryDay\";

            String DownloadDirPath = AppDomain.CurrentDomain.BaseDirectory + @"\DataSource\TWSEStockPriceEveryDay\OKData\";
            String DownloadFileName = TokenGenerator.GetTimeStamp(3) + ".csv";
            String DownloadFullPath = DownloadDirPath + DownloadFileName;

            if (!System.IO.Directory.Exists(DownloadDirPath))
                System.IO.Directory.CreateDirectory(DownloadDirPath);

            for (int i = 1095; i > 0; i--)
            {
                String YYYYMMDD = DateTime.Now.AddDays((0 - i)).ToString("yyyyMMdd");
                DownloadFileName = YYYYMMDD + ".csv";
                DownloadFullPath = DownloadDirPath2 + DownloadFileName;
                if (!System.IO.File.Exists(DownloadFullPath))
                    continue;

                String DownloadData = System.IO.File.ReadAllText(DownloadFullPath);
                if (!String.IsNullOrEmpty(DownloadData))
                    DownloadData = DownloadData.Substring(DownloadData.IndexOf(@"證券代號") - 1).Replace("=\"", "\"");

                DownloadFullPath = DownloadDirPath + DownloadFileName;
                if (System.IO.File.Exists(DownloadFullPath))
                    continue;

                System.IO.File.WriteAllText(DownloadFullPath, DownloadData);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String DownloadDirPath = AppDomain.CurrentDomain.BaseDirectory + @"\DataSource\TWSEStockPriceEveryDay\OKData\";
            //String DownloadFileName = TokenGenerator.GetTimeStamp(3) + ".csv";
            //String DownloadFullPath = DownloadDirPath + DownloadFileName;

            foreach (String DownloadFullPath in System.IO.Directory.GetFiles(DownloadDirPath, "*.csv")) {
                List<StockPriceDay> StockPriceDayList = new List<StockPriceDay>();
                String YYYYMMDD = System.IO.Path.GetFileNameWithoutExtension(DownloadFullPath);
                var csv = new CsvHelper.CsvReader(System.IO.File.OpenText(DownloadFullPath));

                if (DataExists(YYYYMMDD))
                    continue;

                while (csv.Read())
                {
                    StockPriceDay Additem = new StockPriceDay();
                    try
                    {
                        Additem.StockNum = csv.GetField<string>(0).Replace(" ", "");
                        if (Additem.StockNum == "證券代號") continue;
                        if (Additem.StockNum.Length != 4)
                            continue;
                    }
                    catch{
                        continue;
                    }

                    //交易天數(filename)
                    Additem.TraDate = YYYYMMDD;

                    //成交筆數(3) m
                    try
                    {
                        long M = 0;
                        long.TryParse(csv.GetField<string>(3).Replace(",",""), out M);
                        Additem.M = M;
                    }
                    catch
                    {
                        continue;
                    }

                    //開盤價(5) o
                    try
                    {
                        double O = 0;
                        double.TryParse(csv.GetField<string>(5).Replace(",", ""), out O);
                        Additem.O = Math.Round(O, 2);
                        Additem.O = O;
                    }
                    catch
                    {
                        continue;
                    }

                    //最高價6 h
                    try
                    {
                        double H = 0;
                        double.TryParse(csv.GetField<string>(6).Replace(",", ""), out H);
                        Additem.H = Math.Round(H, 2);
                        Additem.H = H;
                    }
                    catch
                    {
                        continue;
                    }

                    //最低價7 l
                    try
                    {
                        double L = 0;
                        double.TryParse(csv.GetField<string>(7).Replace(",", ""), out L);
                        Additem.L = Math.Round(L, 2);
                        Additem.L = L;
                    }
                    catch
                    {
                        continue;
                    }

                    //收盤價8 C
                    try
                    {
                        double C = 0;
                        double.TryParse(csv.GetField<string>(8).Replace(",", ""), out C);
                        Additem.C = Math.Round(C, 2);
                        Additem.C = C;
                    }
                    catch
                    {
                        continue;
                    }

                    if (Additem.StockNum.StartsWith("0"))
                        continue;

                    StockPriceDayList.Add(Additem);
                }

                String sql = "INSERT INTO [dbo].[StockPriceDay] ([TraDate],[StockNum],[M],[O],[H],[L],[C])  " +
                         "values (@TraDate, @StockNum, @M, @O,@H,@L,@C)";

                using (var cn = new SqlConnection(Setting.ConnectionString("Stock")))
                {
                    cn.Open();
                    int rowsChanged = cn.Execute(sql, StockPriceDayList);
                    Console.WriteLine($"{YYYYMMDD}-完成筆數：{ rowsChanged}");
                }
            }
        }

        public bool DataExists(String TraDate)
        {
            using (var cn = new SqlConnection(Setting.ConnectionString("Stock")))
            {
                cn.Open();
                var exists = cn.ExecuteScalar<bool>("select count(1) from StockPriceDay where TraDate=@TraDate", new { TraDate });
                return exists;
            }
        }
    }
}