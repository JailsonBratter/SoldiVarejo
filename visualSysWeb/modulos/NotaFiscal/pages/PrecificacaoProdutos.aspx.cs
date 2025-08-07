using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class PrecificacaoProdutos : visualSysWeb.code.PagePadrao   
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pesquisar(true);
            }
            pesquisar(pnBtn);
        }

        private void pesquisar(bool limitar)
        {
            String sql = "Select codigo,descricao,convert(varchar,data_cadastro,103) as data_cadastro,usuario, status = case status WHEN 0 then  'PENDENTE'  when 1 then 'PRECIFICADO' END   from precificacao ";
            String sqlwhere = "";
            if (!txtCodigo.Text.Equals(""))
            {
                sqlwhere = " codigo = '" + txtCodigo.Text.Trim() + "'";
            }

            if (!txtDescricao.Text.Equals(""))
            {
                if(sqlwhere.Length>0)
                {
                    sqlwhere+=" and ";
                }
                sqlwhere += " descricao like '%"+txtDescricao.Text.Trim()+"%'";
            }
            if (!txtDe.Text.Equals(""))
            {
                if (sqlwhere.Length > 0)
                {
                    sqlwhere += " and ";
                }

                DateTime dtDe=new DateTime();
                DateTime dtAte= new DateTime();

                DateTime.TryParse(txtDe.Text, out dtDe);
                DateTime.TryParse(txtAte.Text, out dtAte);
                sqlwhere += " ( data_cadastro between '" + dtDe.ToString("yyyy-MM-dd") + "' and '"+dtAte.ToString("yyyy-MM-dd")+"')";
            }

            if (!ddlStatus.Text.Equals(""))
            {
                if (sqlwhere.Length > 0)
                {
                    sqlwhere += " and ";
                }

                sqlwhere += " status = '" + ddlStatus.SelectedValue + "'";
            }

            if (sqlwhere.Length > 0)
            {
                sql += " where " + sqlwhere;
            }
            User usr = (User)Session["User"];
            gridPesquisa.DataSource = Conexao.GetTable(sql, usr, limitar);
            gridPesquisa.DataBind();


        }
        protected void ImgPesquisaLista_Click(object sender, EventArgs e)
        {
        }
        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {
        }
        protected void btnCancelaLista_Click(object sender, EventArgs e)
        {
        }
        protected void GridLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoListaItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('GridLista.*GrlistaItem',this)";
            rdo.Attributes.Add("onclick", script);
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("PrecificacaoProdutosDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisar(false);
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            
        }

        protected override bool campoDesabilitado(Control campo)
        {
            return false;
        }

        protected override bool campoObrigatorio(Control campo)
        {
            return false;
        }

        protected void txtDe_TextChanged(object sender, EventArgs e)
        {
            txtAte.Text = txtDe.Text;

        }
    }
}