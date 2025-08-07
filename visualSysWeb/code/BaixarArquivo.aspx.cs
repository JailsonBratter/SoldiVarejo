using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace visualSysWeb.code
{
    public partial class BaixarArquivo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["endereco"] != null)
            {
                String endereco = Request.Params["endereco"].ToString();


                FileInfo fInfo = new FileInfo(endereco);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fInfo.Name + "\"");
                HttpContext.Current.Response.AddHeader("Content-Length", fInfo.Length.ToString());
                HttpContext.Current.Response.WriteFile(fInfo.FullName);

                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Close();

            }
            javascript("window.close();", "Fechar");
        }

        public void javascript(String comando, String strChave)
        {
            HttpContext context = HttpContext.Current;

            var page = (Page)context.Handler;
            ScriptManager.RegisterStartupScript(page, typeof(Page), strChave, comando, true);
        }
    }
}