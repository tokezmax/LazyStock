using AutoMapper;
using Common.Tools;
using LazyStock.ScheduleServices.EFModel;
using LazyStock.ScheduleServices.Model;
using LazyStock.ScheduleServices.Model.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LazyStock.ScheduleServices.Services.DataProvide
{
    class StableStockServices
    {
        public static List<HighQualityListModel> QueryQuality()
        {
            List<QueryHighQualityList_Result> SpHighQualityList = new List<QueryHighQualityList_Result>();
            List<HighQualityListModel> HighQualityList = new List<HighQualityListModel>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<QueryHighQualityList_Result, HighQualityListModel>();
            });
            
            IMapper iMapper = config.CreateMapper();

            using (StockEntities db = new StockEntities())
            {
                db.Database.ExecuteSqlCommand(" GenStockInfo @StockNum ",new SqlParameter("StockNum", ""));
                SpHighQualityList = db.QueryHighQualityList(0.05).ToList();
            }

            SpHighQualityList.ForEach(t =>
            {
                HighQualityListModel r = iMapper.Map<QueryHighQualityList_Result, HighQualityListModel>(t);
                HighQualityList.Add(r);


            });

            LogHelper.doLog("StableStockServices_QueryQuality", JsonConvert.SerializeObject(HighQualityList));
            return HighQualityList;
        }


        public static List<HighQualityListModel> QueryHighQualityListForSlot()
        {
            List<QueryHighQualityListForSlot_Result> SpHighQualityList = new List<QueryHighQualityListForSlot_Result>();
            List<HighQualityListModel> HighQualityList = new List<HighQualityListModel>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<QueryHighQualityList_Result, HighQualityListModel>();
            });

            IMapper iMapper = config.CreateMapper();

            using (StockEntities db = new StockEntities())
            {
                SpHighQualityList = db.QueryHighQualityListForSlot(0.05).ToList();
            }

            SpHighQualityList.ForEach(t =>
            {
                HighQualityListModel r = iMapper.Map<QueryHighQualityListForSlot_Result, HighQualityListModel>(t);
                HighQualityList.Add(r);
            });


            for (int i = 0; i < HighQualityList.Count; i = i + 100)
            {
                var items = HighQualityList.Skip(i).Take(100);

                ReceiveByHighQualityListReqModel r = new ReceiveByHighQualityListReqModel();

                r.HighQualityStock = items.ToList<HighQualityListModel>();

                var jsonText = "{\"ReqParam\" : " + JsonConvert.SerializeObject(r) + @"}";
                LogHelper.doLog("StableStockServices_QueryHighQualityListForSlot", "==發送(" + r.HighQualityStock.Count().ToString() + "筆)==\r\n" + jsonText);
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.Headers.Add("PowerAdmin", Setting.AppSettings("PowerAdmin"));
                var response = client.UploadData(Common.Tools.Setting.AppSettings("ReceiveByHighQualityList"), "POST", Encoding.UTF8.GetBytes(jsonText));
                string resResult = Encoding.UTF8.GetString(response);
                LogHelper.doLog("StableStockServices_QueryHighQualityListForSlot", "==接收==\r\n" + resResult);
            }

            LogHelper.doLog("StableStockServices_QueryHighQualityListForSlot", JsonConvert.SerializeObject(HighQualityList));
            return HighQualityList;
        }
    }
}
