using Common.Tools;
using Newtonsoft.Json;

namespace LazyStock.ScheduleServices.Model
{
    public class BaseResModel
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
        public object Result { get; set; }
    }
}