using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Common.Tools;

namespace LazyStock.Web.Models
{
    public class BaseResModel<T>
    {
        private string _Message;

        [JsonProperty(Order = 0)]
        public ResponseCodeEnum Code { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        [JsonProperty(Order = 1)]
        public string Message
        {
            get
            {
                if (_Message == null)
                {
                    _Message = EnumInfoHelper.GetDisplayValue(this.Code);
                }
                return _Message;
            }
            set
            {
                _Message = value;
            }
        }
        [JsonProperty(Order = 3, NullValueHandling = NullValueHandling.Ignore)]
        public T Result { get; set; }
    }
}