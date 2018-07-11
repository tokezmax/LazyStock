using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LazyStock.Web.Models
{
    public class SetStockPriceReqModel
    {
        public List<StockPrice> StockPrices { get; set; }
    }

    public class StockPrice
    {
        public String StockNum { get; set; }
        public float Price { get; set; }
    }
}