using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LazyStock.Web.Models
{
    public class MemberInfoModel
    {

        public string Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string PicUrl { get; set; }

        public string LineId { get; set; }

        public string Permission { get; set; }
        public string HashCode { get; set; }
        public string LastloginDate { get; set; }

        /*
        public string displayName { get; set; }
        public string userId { get; set; }
        public string pictureUrl { get; set; }
        public string statusMessage { get; set; }
        */
    }
  
}