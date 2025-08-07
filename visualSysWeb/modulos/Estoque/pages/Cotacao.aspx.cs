using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Estoque.pages
{
    public partial class Cotacao : visualSysWeb.code.PagePadrao     //inicio da classe 
    {
        static DataTable tb;
        static String sqlGrid = "select Cotacao,descricao,Data = CONVERT(varchar,data,103),Status from cotacao ";//colocar os campos no select que ser?o apresentados na tela
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                User usr = (User)Session["User"];
                tb = Conexao.GetTable(sqlGrid + "order by cotacao desc", usr, true);
                gridPesquisa.DataSource = tb;
                gridPesquisa.DataBind();
            }
            pesquisar(pnBtn);
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/Estoque/pages/cotacaoDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            String sql = "";
            if (!txtCotacao.Text.Equals("")) //colocar nome do campo de pesquisa
            {
                sql = " cotacao = " + txtCotacao.Text; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
            }
            if (!txtDescricao.Text.Equals("")) //colocar nome do campo de pesquisa2
            {
                if (!sql.Equals(""))
                {
                    sql += " and ";
                }
                sql += " descricao like '" + txtDescricao.Text + "%'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
            }
            if (!txtDataDe.Text.Equals(""))
            {
                if (!sql.Equals(""))
                {
                    sql += " and ";
                }
                sql += " Data between '" + DateTime.Parse(txtDataDe.Text).ToString("yyyy-MM-dd") + "' and '" + DateTime.Parse(txtDataAte.Text).ToString("yyyy-MM-dd") + "' ";
             
            }

            if (!ddlStatus.Text.Equals(""))
            {
                if (!sql.Equals(""))
                {
                    sql += " and ";
                }
                sql += " Status ='" + ddlStatus.Text + "'";
            }



            try
            {
                User usr = (User)Session["User"];
                if (!sql.Equals(""))
                {
                    tb = Conexao.GetTable(sqlGrid + " where " + sql +" order by cotacao desc " , usr, false);
                }
                else
                {
                    tb = Conexao.GetTable(sqlGrid+ " order by cotacao desc ", usr, false);
                }
                gridPesquisa.DataSource = tb;
                gridPesquisa.DataBind();
                lblPesquisaErro.Text = "";
            }
            catch (Exception err)
            {
                lblPesquisaErro.Text = err.Message;
            }
        }
        protected override void btnEditar_Click(object sender, EventArgs e) { }
        protected override void btnExcluir_Click(object sender, EventArgs e) { }
        protected override void btnConfirmar_Click(object sender, EventArgs e) { }
        protected override void btnCancelar_Click(object sender, EventArgs e) { }


        protected override bool campoObrigatorio(Control campo)
        {
            return false;
        }

        protected override bool campoDesabilitado(Control campo)
        {
            return false;
        }


        protected void txtDataDe_TextChanged(object sender, EventArgs e)
        {
            if (IsDate(txtDataDe.Text))
            {
                txtDataAte.Text = txtDataDe.Text;
            }
        }

    }
}