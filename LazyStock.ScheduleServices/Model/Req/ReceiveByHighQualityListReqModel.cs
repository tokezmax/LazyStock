using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LazyStock.ScheduleServices.Model.Data;
using Newtonsoft.Json;

namespace LazyStock.ScheduleServices.Model
{
    public class ReceiveByHighQualityListReqModel
    {
        public List<HighQualityListModel> HighQualityStock { get; set; }
    }
}