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
    public partial class Mobile_Grafico : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
            {
                Response.Redirect("~/modulos/mobile/mobile_login.aspx");
            }

            if (!IsPostBack) 
            {
                lblData.Text = DateTime.Today.ToString("dd/MMM/yyy");
                DataTable tb = new DataTable();
                DataTable TbG = new DataTable();
                String sqlGrid = "sp_Rel_Venda_Filial '" + DateTime.Today.ToString("yyyyMMdd") + "', 1";
                User usr = (User)Session["User"];
                tb = Conexao.GetTable(sqlGrid, usr, false);
                Grafico1.DataSource = tb;
                Grafico1.DataBind();
                Grafico1.ChartAreas[0].Area3DStyle.Enable3D = true;

                Grafico02.DataSource = tb;
                Grafico02.DataBind();
                Grafico02.ChartAreas[0].Area3DStyle.Enable3D = true;

                sqlGrid = "sp_Rel_Posicao_Financeiro '" + DateTime.Today.ToString("yyyyMMdd") + "'";
                TbG = Conexao.GetTable(sqlGrid, usr, false);
                Grafico03.DataSource = TbG;
                Grafico03.DataBind();
            }
        }

        protected void Chart1_Load(object sender, EventArgs e)
        {

        }
    }
}