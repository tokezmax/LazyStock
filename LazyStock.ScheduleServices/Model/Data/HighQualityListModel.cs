using System;

namespace LazyStock.ScheduleServices.Model.Data
{
    public class HighQualityListModel
    {
        public string StockNum { get; set; }
        public string StockName { get; set; }
        public Nullable<double> Price { get; set; }
        public double EstCurrPrice { get; set; }
        public double EstFuturePrice { get; set; }
        public string StableIsBuy { get; set; }
        public string UnStableIsBuy { get; set; }
        public Nullable<int> IsPocket { get; set; }
        public double DiffCurrPrice { get; set; }
        public double DiffFuturePrice { get; set; }
        public string RevenueYYYYMM { get; set; }
        public Nullable<double> RevenueGrowthRatio { get; set; }
    }
}