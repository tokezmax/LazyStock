using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LazyStock.Soa.Models;
using Common;

namespace LazyStock.Soa.Controllers
{
    public class StockBasicController : BaseController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            //StockBasicModel person = new StockBasicModel();
            List<StockBasicModel> StockBasics = new List<StockBasicModel>();
            try
            {

                System.Data.DataTable dt = Common.DataAccess.Dao.QueryDataTable(Sql, null, Common.Tools.Setting.ConnectionString("Stock"));
                StockBasics = dt.ToList<Rootobject>();

                dt = Common.DataAccess.Dao.QueryDataTable(SqlDivi, null, Common.Tools.Setting.ConnectionString("Stock"));
                List<Dividend> Divis = dt.ToList<Dividend>();

                dt = Common.DataAccess.Dao.QueryDataTable(SqlEPS, null, Common.Tools.Setting.ConnectionString("Stock"));
                List<EP> EPS = dt.ToList<EP>();

                StockBasics.ForEach(t =>
                {
                    t.Ver = Common.Tools.TokenGenerator.GetRandom(3);
                    var query = from StockDividends in Divis
                                where StockDividends.Num == t.Num
                                select new Dividend()
                                {
                                    Year = StockDividends.Year,
                                    Num = StockDividends.Num,
                                    CashDivi = StockDividends.CashDivi,
                                    Divi = StockDividends.Divi,
                                    AllDivi = StockDividends.AllDivi
                                };
                    t.Dividends = query.ToList();


                    var queryEPS = from StockEPS in EPS
                                   where StockEPS.Num == t.Num
                                   select new EP()
                                   {
                                       Index = StockEPS.Index,
                                       Year = StockEPS.Year,
                                       Num = StockEPS.Num,
                                       Q = StockEPS.Q,
                                       EPS = StockEPS.EPS,
                                       KeepEPS = StockEPS.KeepEPS,
                                       Quarter3EPSAvg = StockEPS.Quarter3EPSAvg
                                   };
                    t.EPS = queryEPS.ToList();
                });

                String FileName = AppDomain.CurrentDomain.BaseDirectory + @"\StockJson\\";
                StockBasics.ForEach(t =>
                {
                    string json = JsonConvert.SerializeObject(t);
                    LogHelper.FileOverWrite(FileName + t.Num + ".json", json, false);
                });
            }
            catch
            {

            }

            return Json(StockBasics, JsonRequestBehavior.AllowGet);
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}