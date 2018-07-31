using LazyStock.ScheduleServices.Model.Data;
using System.Collections.Generic;

namespace LazyStock.ScheduleServices.Model
{
    public class ReceiveByHighQualityListReqModel
    {
        public List<HighQualityListModel> HighQualityStock { get; set; }
    }
}