using LazyStock.Web.Services;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace LazyStock.Web
{
    public partial class _default : System.Web.UI.Page
    {
 
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack) {
                
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