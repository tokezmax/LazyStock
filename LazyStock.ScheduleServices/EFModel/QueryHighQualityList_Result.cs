//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazyStock.ScheduleServices.EFModel
{
    using System;
    
    public partial class QueryHighQualityList_Result
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
