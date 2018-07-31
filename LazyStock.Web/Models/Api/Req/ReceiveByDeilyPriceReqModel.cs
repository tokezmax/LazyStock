using System.Collections.Generic;

namespace LazyStock.Web.Models
{
    public class ReceiveByDeilyPriceReqModel
    {
        public List<StockPriceDataModel> StockPrices { get; set; }
    }

    /*
    public class StockPrice
    {
        public String StockNum { get; set; }
        public float Price { get; set; }
    }
    */
}