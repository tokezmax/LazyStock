using isRock.LineBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LazyStock.Web
{
    public partial class _default : System.Web.UI.Page
    {
        //const string channelAccessToken = "!!!!! 改成自己的ChannelAccessToken !!!!!";
        //const string AdminUserId= "!!!改成你的AdminUserId!!!";

        const string channelAccessToken = "R8HX6KfjnOQcuwhoLy58urMTvCa/JOJwKb/5Vutli6R4N8P6ODJdYrqHTlxacTdIrlRXnYfRaRrcW/dgLkJHNKuVsgFL9KoGE/6GsHaurpbuIPJg/afGoKrO0rL3p4edHMz7ujGfTy4X67yD95n8zQdB04t89/1O/w1cDnyilFU=";
        const string AdminUserId = "U3e9d23440ba5e125c44ebf8962cee353";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var bot = new Bot(channelAccessToken);
            bot.PushMessage(AdminUserId, $"測試 {DateTime.Now.ToString()} ! ");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            var bot = new Bot(channelAccessToken);
            bot.PushMessage(AdminUserId, 1,2);
        }
    }
}