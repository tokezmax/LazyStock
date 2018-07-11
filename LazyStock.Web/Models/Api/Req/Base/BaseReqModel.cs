using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace LazyStock.Web.Models
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