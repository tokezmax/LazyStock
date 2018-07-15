using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LazyStock.Models
{
    public class EPS_DiviDataModel
    {
        public int StockNum { get; set; }
        public string Year { get; set; }
        public string LastQ { get; set; }
        public float TotalEPS { get; set; }
        public float TotalDivi { get; set; }
        public float EachDiviFromEPS { get; set; }
    }
  
}