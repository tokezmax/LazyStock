using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class LineBotWebHookController : isRock.LineBot.LineWebHookControllerBase
    {
        const string channelAccessToken = "R8HX6KfjnOQcuwhoLy58urMTvCa/JOJwKb/5Vutli6R4N8P6ODJdYrqHTlxacTdIrlRXnYfRaRrcW/dgLkJHNKuVsgFL9KoGE/6GsHaurpbuIPJg/afGoKrO0rL3p4edHMz7ujGfTy4X67yD95n8zQdB04t89/1O/w1cDnyilFU=";
        const string AdminUserId = "U3e9d23440ba5e125c44ebf8962cee353";

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
                    /*
                    if (LineEvent.message.type == "text") //收到文字
                        this.ReplyMessage(LineEvent.replyToken, "你說了("+ UserId + "):" + LineEvent.message.text);
                    if (LineEvent.message.type == "sticker") //收到貼圖
                        this.ReplyMessage(LineEvent.replyToken, 1, 2);
                    */
                    if (LineEvent.message.type == "text") {

                        if (LineEvent.message.text.IndexOf("點餐") > -1)
                        {
                            this.ReplyMessage(LineEvent.replyToken, "早安~"+_UserName+"("+ _UserId + ")[" + UserId + "]~是否和上次一樣？\n●拿鐵(L)-冰-無糖\n●歐姆蛋貝果\n\n2018/05/27 9:30 取餐呢？");
                        }
                        else if (LineEvent.message.text.IndexOf("是") > -1)
                        {
                            this.ReplyMessage(LineEvent.replyToken, "感謝您的訂購~ :)");
                        }
                        else if (LineEvent.message.text.ToUpper().IndexOf("YES") > -1)
                        {
                            this.ReplyMessage(LineEvent.replyToken, "取消完成~ T_T");
                        }
                        if (LineEvent.message.text.IndexOf("取消訂單") > -1)
                        {
                            this.ReplyMessage(LineEvent.replyToken, "陳sir~確定要取消下列訂單~\n●拿鐵(L)-冰-無糖\n●歐姆蛋貝果\n");
                        }
                        else if ((LineEvent.message.text.IndexOf("點餐") > -1) || LineEvent.message.text.ToUpper().IndexOf("MENU") > -1) {

                        }
                        else {
                            this.ReplyMessage(LineEvent.replyToken, "歡迎~請問需要什麼服務呢？\n");
                        }
                    }
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
