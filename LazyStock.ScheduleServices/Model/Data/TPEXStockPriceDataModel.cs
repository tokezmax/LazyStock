using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyStock.ScheduleServices.Model
{
    class TPEXStockPriceDataModel
    {
        public string reportDate { get; set; }
        public int iTotalRecords { get; set; }
        public string[][] aaData { get; set; }
    }
}
