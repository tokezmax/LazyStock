using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LazyStock.Web.Models
{
    public class EPS_DiviDataModel
    {
        public string StockNum { get; set; }
        public int Year { get; set; }
        public string LastQ { get; set; }
        public Nullable<double> TotalEPS { get; set; }
        public Nullable<double> TotalDivi { get; set; }
        public Nullable<double> EachDiviFromEPS { get; set; }
    }

}