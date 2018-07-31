using System.ComponentModel.DataAnnotations;

namespace LazyStock.ScheduleServices.Model
{
    public enum ResponseCodeEnum
    {
        ///<summary>成功取得</summary>
        [Display(Name = "成功取得。")]
        Success = 0,

        ///<summary>資料不足或格式錯誤</summary>
        [Display(Name = "查無資料。")]
        DataNotFound = 997,

        ///<summary>資料不足或格式錯誤</summary>
        [Display(Name = "資料不足或格式錯誤。")]
        FieldsThatAreRequired = 998,

        ///<summary>API程式例外錯誤</summary>
        [Display(Name = "API程式例外錯誤。")]
        Failed = 999
    }
}