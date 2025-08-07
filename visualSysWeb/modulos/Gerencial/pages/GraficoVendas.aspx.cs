using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Gerencial.pages
{
    public partial class GraficoVendas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

           if(!IsPostBack)
            {
                carregarVendas();
            }
        }
        protected void imgBtnFiltro_Click(object send , EventArgs e)
        {

            
        }
        private void carregarVendas()
        {
            gridVendas.DataSource = Conexao.GetTable("sp_br_mercadoria_Acum '20356', 'MATRIZ'", null, false);
            gridVendas.DataBind();

            gridVendas2.DataSource = Conexao.GetTable("sp_br_mercadoria_Acum '20357', 'MATRIZ'", null, false);
            gridVendas2.DataBind();

            StringBuilder opt = new StringBuilder("");
            StringBuilder vlrs = new StringBuilder("");
            StringBuilder vlrs2 = new StringBuilder("");

            opt.Append("[");
            vlrs.Append("[[");
            vlrs2.Append("[[");


            for (int i = gridVendas.Rows.Count-1; i >= 0; i--)
            {
                GridViewRow rw = gridVendas.Rows[i];
                opt.Append("'" + rw.Cells[0].Text + "',");
                vlrs.Append("" + Funcoes.decimalPonto(rw.Cells[1].Text) + ",");
                vlrs2.Append("" + Funcoes.decimalPonto(rw.Cells[2].Text) + ",");
            }

            //foreach (GridViewRow rw in gridVendas.Rows)
            //{
            //    opt.Append("'" + rw.Cells[0].Text + "',");
            //    vlrs.Append("" + Funcoes.decimalPonto(rw.Cells[1].Text) + ",");
            //    vlrs2.Append("" + Funcoes.decimalPonto(rw.Cells[2].Text) + ",");
            //}
            opt.Append("]");
            vlrs.Append("],[");
            vlrs2.Append("],[");
            for (int i = gridVendas2.Rows.Count - 1; i >= 0; i--)
            {
                GridViewRow rw = gridVendas2.Rows[i];
                vlrs.Append("" + Funcoes.decimalPonto(rw.Cells[0].Text) + ",");
                vlrs2.Append("" + Funcoes.decimalPonto(rw.Cells[1].Text) + ",");
            }

            //    foreach (GridViewRow rw in gridVendas2.Rows)
            //{
            //    vlrs.Append("" + Funcoes.decimalPonto(rw.Cells[0].Text) + ",");
            //    vlrs2.Append("" + Funcoes.decimalPonto(rw.Cells[1].Text) + ",");
            //}

            vlrs.Append("]]");
            vlrs2.Append("]]");




            ScriptManager.RegisterStartupScript(Page, typeof(Page), "Grafico", "carregarGraficos('#grafico1'," + opt.ToString() + "," + vlrs.ToString() + ");", true);

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "Grafico2", "carregarGraficos('#grafico2'," + opt.ToString() + "," + vlrs2.ToString() + ");", true);
        }

    }
}