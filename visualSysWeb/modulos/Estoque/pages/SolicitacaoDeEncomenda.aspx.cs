using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Estoque.pages
{
    public partial class SolicitacaoDeEncomenda : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pesquisa(true);
            }
            pesquisar(pnBtn);
        }

        private void pesquisa(bool limitar)
        {
            User usr = (User)Session["User"];
            String sql = "Select Codigo,Descricao,convert(varchar,Data_cadastro,103) as data_cadastro,usuario_cadastro,status" +
                ",STUFF ( " +
                   " ( " +
                   " Select  ', ' + pedido from solicitacao_producao_Pedidos where tipo_producao = 1 and filial = solicitacao_producao.filial and codigo = solicitacao_producao.codigo " +
                   " FOR XML PATH('') " +
                   " ) " +
                    ",1,2,''" +
               " ) as pedido " +
               " from solicitacao_producao";
            String sqlWhere = " isnull(tipo_producao,0) = 1";
            if (!txtCodigo.Text.Equals(""))
            {
                sqlWhere = " and  codigo ='" + txtCodigo.Text + "'";
            }
            if (!txtDescricao.Text.Equals(""))
            {
                if (sqlWhere.Length > 0)
                {
                    sqlWhere += " and ";
                }

                sqlWhere += " Descricao like '%" + txtDescricao.Text + "%'";
            }

            if (!ddlStatus.Text.Equals(""))
            {
                if (sqlWhere.Length > 0)
                {
                    sqlWhere += " and ";
                }

                sqlWhere += " status='" + ddlStatus.Text + "'";
            }

            if (sqlWhere.Length > 0)
            {
                sql += " where " + sqlWhere;
            }


            gridPesquisa.DataSource = Conexao.GetTable(sql + " order by convert(int,Codigo) desc", usr, limitar);
            gridPesquisa.DataBind();


        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("SolicitacaoDeEncomendaDetalhes.aspx?novo=true");
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