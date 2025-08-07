using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.mobile.pages
{
    public partial class Mobile_Vendas : System.Web.UI.Page
    {
        static DataTable tb;
        static String sqlGrid = "sp_Rel_Venda_Filial '" + DateTime.Today.ToString("yyyyMMdd") + "'";
        
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("~/Modulos/Mobile/Mobile_Login.aspx");
                }

                lblData.Text = DateTime.Today.ToString("dd/MMM/yyy");
                User usr = (User)Session["User"];
                tb = Conexao.GetTable(sqlGrid, usr, false);
                gridPesquisa.DataSource = tb;
                gridPesquisa.DataBind();
            }
            if (Session["User"] == null)
            {
                Response.Redirect("~/Modulos/Mobile/Mobile_Login.aspx");
            }
        }
    }
}