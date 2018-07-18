using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Tools;
using LazyStock.ScheduleServices;
using LazyStock.ScheduleServices.Services;

using System.Net;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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
    }
}
