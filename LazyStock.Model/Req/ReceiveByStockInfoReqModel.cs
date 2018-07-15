using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace LazyStock.Models
{
    public class ReceiveByStockInfoReqModel
    {
        public List<StockInfoResModel> StockInfos { get; set; }
    }
}