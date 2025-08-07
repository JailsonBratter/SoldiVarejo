using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Estoque.pages
{
    public partial class SolicitacaoCompra : visualSysWeb.code.PagePadrao
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
            String sql = "Select Codigo,Descricao,convert(varchar,Data_cadastro,103) as data_cadastro,usuario_cadastro,status " +
                " , STUFF ( " +
                   " ( " +
                   " Select  ', ' + pedido from solicitacao_pedidos where filial = solicitacao_compra.filial and codigo_solicitacao = solicitacao_compra.codigo " +
                   " FOR XML PATH('') " +
                   " ) " +
                    ",1,2,''" +
               " ) as pedido" +
               " from solicitacao_compra";
            String sqlWhere = "";
            if (!txtCodigo.Text.Equals(""))
            {
                sqlWhere = " codigo ='" + txtCodigo.Text + "'";
            }
            if(!txtDescricao.Text.Equals(""))
            {
                if(sqlWhere.Length>0)
                {
                    sqlWhere+= " and " ;
                }

                sqlWhere+= " Descricao like '%"+txtDescricao.Text +"%'";
            }

            if(!ddlStatus.Text.Equals(""))
            {
                if(sqlWhere.Length>0)
                {
                    sqlWhere+= " and " ;
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
            Response.Redirect("SolicitacaoCompraDetalhes.aspx?novo=true");
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