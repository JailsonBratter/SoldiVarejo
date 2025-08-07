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
    public partial class Mobile_Venda_SubGrupo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["Codigo_Grupo"] != null)
            {
                if (!IsPostBack)
                {
                    //Se não tem nenhum usuário definido o sistema chama a tela de login.
                    if (Session["User"] == null)
                    {
                        Response.Redirect("~/Modulos/Mobile/Mobile_Login.aspx");
                    }
                    //Atribui à variável strFilial o valor do parametro enviado na tela MOBILE_vENDAS
                    String strFilial = "";
                    String strGrupo = Request.Params["codigo_Grupo"];
                    lblFilial.Text = (String)Session["strFilialPar"];
                    strFilial = lblFilial.Text;
                    lblData.Text = DateTime.Today.ToString("dd/MMM/yyy") + " | Vendas SUBGRUPO";
                    Session.Remove("strFilialParSub");
                    Session.Add("strFilialParSub", strFilial);
                    DataTable tb;
                    String sqlGrid = "sp_Rel_Venda_SubGrupo '" + strFilial + "', '" + DateTime.Today.ToString("yyyyMMdd") + "', '" + DateTime.Today.ToString("yyyyMMdd") + "', '" + strGrupo + "'";
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

        protected void gridPesquisa_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex != -1)
            {
                double dblMargem = 0;
                double dblVlr = Convert.ToDouble(e.Row.Cells[2].Text);
                double dblCusto = Convert.ToDouble(e.Row.Cells[4].Text);
                if (dblVlr != 0 & dblCusto != 0)
                {
                    dblMargem = ((dblVlr - dblCusto) / dblVlr) * 100;
                }
                e.Row.Cells[e.Row.Cells.Count - 1].Text = dblMargem.ToString("N");
            }
        }
    }
}