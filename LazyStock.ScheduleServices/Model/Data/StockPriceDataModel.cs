using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LazyStock.ScheduleServices.Model
{
    public class StockPriceDataModel
    {
        /// <summary>
        /// 股號
        /// </summary>
        public String StockNum { get; set; }
        /// <summary>
        /// 股價
        /// </summary>
        public double StockPrice { get; set; }
        /// <summary>
        /// 本益比
        /// </summary>
        public float PER { get; set; }
        /// <summary>
        /// 股價淨值比
        /// </summary>
        public float PBR { get; set; }
    }
}