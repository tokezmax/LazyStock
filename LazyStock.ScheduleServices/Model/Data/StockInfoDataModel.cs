using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyStock.ScheduleServices.Model
{
    public class StockInfoDataModel 
    {
        /// <summary>
        /// 股號
        /// </summary>
        public String StockNum { get; set; }
        /// <summary>
        /// 股價
        /// </summary>
        public float StockPrice { get; set; }
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
