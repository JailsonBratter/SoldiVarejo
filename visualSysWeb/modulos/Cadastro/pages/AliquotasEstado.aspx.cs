using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using System.Collections;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class AliquotasEstado : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pesquisa(true);
            }
            pesquisar(pnBtn);
            camposnumericos();
        }

        private void camposnumericos()
        {
            txtNCM.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
        }
        protected void pesquisa(bool limitar)
        {
            User usr = (User)Session["User"];

            if (usr == null)
                return;

            String strSql = "Select * from cf ";
            String strWhere = "";
            String totalRegistro = Conexao.retornaUmValor("select count(*) from cf ", usr);

            if (!txtDescricao.Text.Equals(""))
            {
                strWhere = " descricao like '%" + txtDescricao.Text + "%' ";
            }
            if (!txtNCM.Text.Equals(""))
            {
                if (strWhere.Length > 0)
                {
                    strWhere += " AND ";
                }
                strWhere += " cf LIKE '%" + txtNCM.Text + "%'";

            }


            String totalFiltro = "";
            String strSqltotalFiltro = "Select count(*) from cf where filial='" + usr.getFilial() + "'";
            if (strWhere.Length > 0)
            {
                strSql += " where " + strWhere;
                strSqltotalFiltro += " and " + strWhere;

            }







            gridPesquisa.DataSource = Conexao.GetTable(strSql, usr, limitar);
            gridPesquisa.DataBind();

            if (strWhere.Length > 0)
            {
                totalFiltro = Conexao.retornaUmValor(strSqltotalFiltro, usr);
            }
            else
            {
                totalFiltro = gridPesquisa.Rows.Count.ToString();
            }

            lblRegistros.Text = totalFiltro + " Registros de  " + totalRegistro + " Cadastrados ";

        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("AliquotasEstadoDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisa(false);
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