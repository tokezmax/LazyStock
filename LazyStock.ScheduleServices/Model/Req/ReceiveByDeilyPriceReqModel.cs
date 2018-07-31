using System.Collections.Generic;

namespace LazyStock.ScheduleServices.Model
{
    public class ReceiveByDeilyPriceReqModel
    {
        public List<StockPriceDataModel> StockPrices { get; set; }
    }
}