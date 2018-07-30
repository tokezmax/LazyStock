using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LazyStock.Web
{
    public partial class LazySlot : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            //if (String.IsNullOrEmpty(Request.Form["Num"])) { 
            if (!String.IsNullOrEmpty(Request.QueryString["Num"]))
            {
                Response.Write("<input id='Num' type='hidden' value='" + Request.QueryString["Num"] + "'>");
                //Response.Write("<input id='Num' type='hidden' value='" + Request.Form["Num"] + "'>");
            }

            //if (String.IsNullOrEmpty(Request.Form["StockName"]))
            if (!String.IsNullOrEmpty(Request.QueryString["StockName"]))
            {
                Response.Write("<input id='StockName' type='hidden' value='" + Request.QueryString["StockName"] + "'>");
                //Response.Write("<input id='StockName' type='hidden' value='" + Request.Form["StockName"] + "'>");
            }
            
        }
        protected override void Render(HtmlTextWriter writer)
        {
            StringWriter html = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(html);
            base.Render(tw);

            string outhtml = html.ToString();

            outhtml = Regex.Replace(outhtml, @"(?<=>)\s|\n|\t(?=<)", string.Empty);
            outhtml = outhtml.Trim();

            writer.Write(outhtml);
        }
    }
}