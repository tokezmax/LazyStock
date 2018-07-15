using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LazyStock.Web.Models
{
    public class MemberInfo
    {
        public string displayName { get; set; }
        public string userId { get; set; }
        public string pictureUrl { get; set; }
        public string statusMessage { get; set; }
    }
  
}