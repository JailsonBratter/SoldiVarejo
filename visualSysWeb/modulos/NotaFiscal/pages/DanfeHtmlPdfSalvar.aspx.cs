using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;



namespace visualSysWeb.modulos.NotaFiscal.pages
{
    public partial class DanfeHtmlPdfSalvar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            if (usr == null)
            {
                Response.Redirect("~");
                return;
            }
            String url = "";
            try
            {
                if (!IsPostBack)
                {
                    var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();

                    // String url = Request.Url.ToString().Replace("DanfeHtmlPdfSalvar", "DanfeHtmlPdf");// "~/modulos/notaFiscal/pages/DanfeHtmlPdf.aspx";
                    if (Request.Params["NomePDF"] != null)
                    {
                        url = Request.Url.ToString().Replace("DanfeHtmlPdfSalvar", Request.Params["NomePDF"]);
                        String strPasta = Server.MapPath("~/modulos/notafiscal/pages/");

                        //throw  new Exception();
                        htmlToPdf.GeneratePdfFromFile(url, null, strPasta + "/export2.pdf");
                        //FileStream fl = new FileStream("D:/TesteDeDamfe.pdf", FileMode.Create);

                        //fl.Write(pdfBytes, 0, pdfBytes.Length);
                        //fl.Close();
                    }
                    Response.Redirect("export2.pdf");
                }
                
            }
            catch (Exception err )
            {

                Response.Redirect(url+"&imprimir=true");
            }

        }


    }
}