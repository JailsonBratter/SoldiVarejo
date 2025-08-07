using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class CFOPEntradaSaida : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pesquisa();
            }
            pesquisar(pnBtn);
            camposnumericos();
        }


        private void camposnumericos()
        {
            txtCFOP.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
        }

        protected void pesquisa()
        {
            User usr = (User)Session["User"];

            String strSql = "select id, CFOP_entrada,CFOP_Saida,DESCRICAO from CFOP_Entrada_Saida ";
            String strWhere = "";
            String strSqltotalFiltro = "Select count(*) from CFOP_Entrada_Saida  ";
            String totalRegistro = Conexao.retornaUmValor(strSqltotalFiltro, null);

            if (!txtCFOP.Text.Equals(""))
            {
                strWhere = " cfop_entrada LIKE '%" + txtCFOP.Text + "%' or cfop_saida like '%"+txtCFOP.Text+"%' ";
            }
            if (!txtDescricao.Text.Equals(""))
            {
                if (strWhere.Length > 0)
                {
                    strWhere += " AND ";
                }
                strWhere += " DESCRICAO LIKE '%" + txtDescricao.Text + "%'";

            }
          


            if (strWhere.Length > 0)
            {
                strSql += " where " + strWhere;
                strSqltotalFiltro += " where " + strWhere;
            }

            gridPesquisa.DataSource = Conexao.GetTable(strSql, usr, true);
            gridPesquisa.DataBind();
            String totalFiltro = Conexao.retornaUmValor(strSqltotalFiltro, usr);
            lblRegistros.Text = totalFiltro + " Registros de  " + totalRegistro + " Cadastrados ";


        }


        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("CFOPEntradaSaidaDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisa();
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override bool campoDesabilitado(Control campo)
        {
            throw new NotImplementedException();
        }

        protected override bool campoObrigatorio(Control campo)
        {
            throw new NotImplementedException();
        }
    }
}