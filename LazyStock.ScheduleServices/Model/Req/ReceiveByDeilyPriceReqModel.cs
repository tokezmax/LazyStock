using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LazyStock.ScheduleServices.Model
{
    public class ReceiveByDeilyPriceReqModel
    {
        public List<StockPriceDataModel> StockPrices { get; set; }
    }
}