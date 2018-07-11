using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LazyStock.Soa.Models
{
    public class StockBasicModel
    {
        public String Ver { get; set; }
        public int Num { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Industry { get; set; }
        public float PERatio { get; set; }
        public float InvestorRatio { get; set; }
        public float DebtRatio { get; set; }
        public int Value { get; set; }
        public List<DividendModel> Dividends { get; set; }
        public List<EPSModel> EPS { get; set; }
    }
    public class DividendModel
    {

        public int Num { get; set; }
        public string Year { get; set; }
        public float CashDivi { get; set; }
        public float Divi { get; set; }
        public float AllDivi { get; set; }
    }
    public class EPSModel
    {
        public string Index { get; set; }
        public int Num { get; set; }
        public int Year { get; set; }
        public string Q { get; set; }
        public float EPS { get; set; }
        public float KeepEPS { get; set; }
        public float Quarter3EPSAvg { get; set; }
    }
}