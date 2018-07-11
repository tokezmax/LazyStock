using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LazyStock.Web.Models
{
    public class StockPriceDataModel
    {
        public String StockNum { get; set; }
        public float StockPrice { get; set; }
        public float PER { get; set; }
        public float PBR { get; set; }
    }
}