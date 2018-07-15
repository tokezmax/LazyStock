using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LazyStock.Web.Models
{
    public class DividendDataModel
    {
        public int Num { get; set; }
        public string Year { get; set; }
        public float CashDivi { get; set; }
        public float Divi { get; set; }
        public float AllDivi { get; set; }
    }
}