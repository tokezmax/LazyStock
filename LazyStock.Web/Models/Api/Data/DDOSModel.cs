using System;

namespace LazyStock.Web.Models
{
    public class DDOSModel
    {
        public DDOSModel()
        {
            this.Min = DateTime.Now.Minute;
        }

        public string userid { get; set; }
        public string ip { get; set; }
        public int Min { get; set; }
        public int QueryCount { get; set; }
    }
}