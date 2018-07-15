using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace LazyStock.Web
{
    public partial class CallbackFromLineLogin : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //���o��^��code
            var code = Request.QueryString["code"];
            if (code == null)
            {
                Response.Write("�S�����T�^��code");
                Response.End();
            }
            //��ܡA���ե�
            Response.Write("<br/> code : " + code);
            //�qCode���^toke
            var token = isRock.LineLoginV21.Utility.GetTokenFromCode(code,
                "1593644840",  //TODO:�Ч󥿬��A�ۤv�� client_id
                "760934e2ecec09b4e73af7f18a0abfcc", //TODO:�Ч󥿬��A�ۤv�� client_secret
                "http://localhost:2458/CallbackFromLineLogin.aspx");  //TODO:�Ч󥿬��A�ۤv�� callback url
                                                          //��ܡA���ե�
                                                          //(�`�N�o�O�d�ҡAtoken���ӥΩ��X�ǻ��A�]���ӥX�{�b�Τ�ݡA�A���Ӧۦ�O���b��Ʈw��ServerSite session��)
            Response.Write("<br/> token : " + token.access_token);
            //�Q��token������o�Τ��T
            var user = isRock.LineLoginV21.Utility.GetUserProfile(token.access_token);
            Response.Write("<br/> user: " + user.displayName);
        }
    }
}