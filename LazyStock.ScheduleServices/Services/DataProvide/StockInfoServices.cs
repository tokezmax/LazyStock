using AutoMapper;
using Common.Tools;
using LazyStock.ScheduleServices.EFModel;
using LazyStock.ScheduleServices.Interface;
using LazyStock.ScheduleServices.Model;
using LazyStock.ScheduleServices.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;

namespace LazyStock.ScheduleServices.Services
{
    internal class StockInfoServices
    {
        //private static IRepository<CalStockEachQAvgEPS> CalStockEachQAvgEPSRepo;
        private static IRepository<CalStockEPS_Divi> CalStockEPS_DiviRepo;

        private static IRepository<CalStockInfo> CalStockInfoRepo;

        static StockInfoServices()
        {
            //CalStockEachQAvgEPSRepo = new GenericRepository<CalStockEachQAvgEPS>();
            CalStockEPS_DiviRepo = new GenericRepository<CalStockEPS_Divi>();
            CalStockInfoRepo = new GenericRepository<CalStockInfo>();
        }

        public static void UploadData()
        {
            try
            {
                List<CalStockInfo> CalStockInfos = CalStockInfoRepo.GetAll().OrderBy(x => x.StockNum).ToList();
                List<CalStockEPS_Divi> CalStockEPS_Divis = CalStockEPS_DiviRepo.GetAll().OrderBy(x => x.StockNum).ThenByDescending(n => n.Year).ToList();
                //StockEntities _context = new StockEntities();
                //List<CalStockInfo> CalStockInfos = _context.CalStockInfo.ToList<CalStockInfo>();
                //List<CalStockEPS_Divi> CalStockEPS_Divis = _context.CalStockEPS_Divi.ToList<CalStockEPS_Divi>();

                ReceiveByStockInfoReqModel StockInfos = new ReceiveByStockInfoReqModel();
                StockInfos.StockInfos = new List<StockInfoResModel>();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<CalStockInfo, StockInfoResModel>().ForMember(x => x.EPS_Divi, opt => opt.Ignore());
                });
                IMapper iMapper = config.CreateMapper();

                CalStockInfos.ForEach(t =>
                {
                    StockInfoResModel r = iMapper.Map<CalStockInfo, StockInfoResModel>(t);

                    r.EPS_Divi = (from StockEPS_Divi in CalStockEPS_Divis
                                  where StockEPS_Divi.StockNum == t.StockNum
                                  select new EPS_DiviDataModel()
                                  {
                                      Year = StockEPS_Divi.Year,
                                      LastQ = StockEPS_Divi.LastQ,
                                      StockNum = StockEPS_Divi.StockNum,
                                      TotalEPS = StockEPS_Divi.TotalEPS,
                                      TotalDivi = StockEPS_Divi.TotalDivi,
                                      EachDiviFromEPS = StockEPS_Divi.EachDiviFromEPS
                                  }).ToList();

                    StockInfos.StockInfos.Add(r);
                });

                for (int i = 0; i < StockInfos.StockInfos.Count; i = i + 2)
                {
                    var items = StockInfos.StockInfos.Skip(i).Take(2);

                    ReceiveByStockInfoReqModel r = new ReceiveByStockInfoReqModel();

                    r.StockInfos = items.ToList<StockInfoResModel>();

                    var jsonText = "{\"ReqParam\" : " + JsonConvert.SerializeObject(r) + @"}";
                    LogHelper.doLog("StockInfoServices", "==發送(" + r.StockInfos.Count().ToString() + "筆)==\r\n" + jsonText);
                    WebClient client = new WebClient();
                    client.Encoding = Encoding.UTF8;
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    client.Headers.Add("PowerAdmin", Setting.AppSettings("PowerAdmin"));
                    var response = client.UploadData(Common.Tools.Setting.AppSettings("ReceiveByStockInfoURL"), "POST", Encoding.UTF8.GetBytes(jsonText));
                    string resResult = Encoding.UTF8.GetString(response);
                    LogHelper.doLog("StockInfoServices", "==接收==\r\n" + resResult);
                }
            }
            catch (Exception e)
            {
                LogHelper.doLog("StockInfoServices", "[Exception]" + e.Message);
            }
        }
    }
}