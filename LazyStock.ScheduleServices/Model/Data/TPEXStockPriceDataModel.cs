namespace LazyStock.ScheduleServices.Model
{
    internal class TPEXStockPriceDataModel
    {
        public string reportDate { get; set; }
        public int iTotalRecords { get; set; }
        public string[][] aaData { get; set; }
    }
}