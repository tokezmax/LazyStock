using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyStock.ScheduleServices.Model.Line
{
    /// <summary>
    /// Text
    /// https://developers.line.me/en/docs/messaging-api/reference/#text
    /// </summary>
    public class TextMessageModel
    {
        public String Type { get; } = "text";

        /// <summary>
        /// Message text
        /// Max: 2000 characters
        /// </summary>
        public string Text { get; set; }
    }
}
