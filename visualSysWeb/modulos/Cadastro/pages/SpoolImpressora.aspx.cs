using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class SpoolImpressora : visualSysWeb.code.PagePadrao     //inicio da classe 
    {

        static String sqlGrid = "select * from Spool_impressoras";//colocar os campos no select que ser?o apresentados na tela
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pesquisar();
            }
            pesquisar(pnBtn);
        }
        private void pesquisar()
        {
            User usr = (User)Session["User"];
            DataTable tb = null;
            String sql = "";
            if (!txtDescricao.Text.Equals("")) //colocar nome do campo de pesquisa
            {
                sql = " descricao like '%" + txtDescricao.Text + "%'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
            }
            if (!txtIdTrm.Text.Equals("")) //colocar nome do campo de pesquisa2
            {
                if (!sql.Equals(""))
                {
                    sql += " and ";
                }
                sql += "Id_trm= '" + txtIdTrm.Text + "'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
            }
            try
            {
                if (!sql.Equals(""))
                {
                    tb = Conexao.GetTable(sqlGrid + " where " + sql, usr, false);
                }
                else
                {
                    tb = Conexao.GetTable(sqlGrid, usr, false);
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
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/cadastro/pages/SpoolImpressoraDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {

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
    }
}