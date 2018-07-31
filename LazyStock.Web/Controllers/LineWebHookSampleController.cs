using System;
using System.Linq;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class LineBotWebHookController : isRock.LineBot.LineWebHookControllerBase
    {
        private const string channelAccessToken = "SALEdmTrZ001uP7nelpndyaRH5NPeTlEHd0QioPjNrNfOOzxfYj1QgDWZhPdePPYbrju9fe08TplcdB00qfArHWqfxs0Ob/B5jYwCmIIowTfjik14pb/EbjrdNqAdi2JIBLspjzBIYCuAdsUUsKPGwdB04t89/1O/w1cDnyilFU=";
        private const string AdminUserId = "U06bdebb439e5566c04d9a40bf48ecdd8";

        [Route("api/LineWebHookSample")]
        [HttpPost]
        public IHttpActionResult POST()
        {
            try
            {
                //設定ChannelAccessToken(或抓取Web.Config)
                this.ChannelAccessToken = channelAccessToken;
                //取得Line Event(範例，只取第一個)
                var LineEvent = this.ReceivedMessage.events.FirstOrDefault();
                //來源帳號
                var UserId = this.ReceivedMessage.events.FirstOrDefault().source.userId;
                //使用者名稱
                var _UserName = this.GetUserInfo(UserId).displayName;
                //使用者id
                var _UserId = this.GetUserInfo(UserId).userId;

                //配合Line verify
                if (LineEvent.replyToken == "00000000000000000000000000000000") return Ok();
                //回覆訊息
                if (LineEvent.type == "message")
                {
                    if (LineEvent.message.type == "text") //收到文字
                        this.ReplyMessage(LineEvent.replyToken, "你說了(" + UserId + "):" + LineEvent.message.text);
                    if (LineEvent.message.type == "sticker") //收到貼圖
                        this.ReplyMessage(LineEvent.replyToken, 1, 2);
                }
                //response OK
                return Ok();
            }
            catch (Exception ex)
            {
                //如果發生錯誤，傳訊息給Admin
                this.PushMessage(AdminUserId, "發生錯誤:\n" + ex.Message);
                //response OK
                return Ok();
            }
        }
    }
}