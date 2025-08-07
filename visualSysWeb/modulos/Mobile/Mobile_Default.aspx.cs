using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.mobile
{
    public partial class mobile_default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
            {
                Response.Redirect("~/modulos/mobile/mobile_login.aspx");
            }
        }

        protected void imgAtalho01_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/modulos/mobile/pages/mobile_posicao_financeiro.aspx");
        }

        protected void imgAtalho02_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/modulos/mobile/pages/mobile_Vendas.aspx");
        }

        protected void imgAtalho03_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/modulos/mobile/pages/mobile_Grafico.aspx");
        }
    }
}