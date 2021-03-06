﻿using Newtonsoft.Json;

namespace LazyStock.ScheduleServices.Model
{
    public class BaseReqModel
    {
        public BaseReqModel()
        {
            Ver = 1;
        }

        public int Ver { get; set; }

        [JsonIgnore]
        public string TransactionSn { get; set; }

        [JsonIgnore]
        public string HostName { get; set; }
    }
}