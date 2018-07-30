using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace LazyStock.Web.Models
{
    public class ReceiveByHighQualityListReqModel
    {
        public List<HighQualityResModel> HighQualityStock { get; set; }
    }
}