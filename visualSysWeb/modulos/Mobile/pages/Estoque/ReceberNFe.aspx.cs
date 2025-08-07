using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace visualSysWeb.modulos.Mobile.pages.Estoque
{
    public partial class ReceberNFe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Timer1.Enabled = false;
            }
        }

        protected void btnImportar_Click(object sender, EventArgs e)
        {
            //Response.Redirect("lerBarra.html");
            Timer1.Interval = 5000;
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "Mensagem", "startScanner()", true);
            Timer1.Enabled = true;

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "Timer", "setarValor()", true);
            Timer1.Enabled = false;
            
        }
    }
}