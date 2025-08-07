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
    public partial class Familia : visualSysWeb.code.PagePadrao     //inicio da classe 
    {
        static String sqlGrid = "select * from familia";//colocar os campos no select que ser?o apresentados na tela
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
            txtCodigo.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/cadastro/pages/familiaDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
        }
        private void pesquisa()
        {
            String sql = "";
            String strSqltotalFiltro = "select count(*) from familia ";
            String totalRegistro = Conexao.retornaUmValor(strSqltotalFiltro, null);

            if (!txtCodigo.Text.Equals("")) //colocar nome do campo de pesquisa
            {
                sql = " codigo_familia like '%" + txtCodigo.Text + "%'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
            }
            if (!txtDescricao.Text.Equals("")) //colocar nome do campo de pesquisa2
            {
                if (!sql.Equals(""))
                {
                    sql += " and ";
                }
                sql += "descricao_familia like '%" + txtDescricao.Text + "%'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
            }
            try
            {
                User usr = (User)Session["User"];
                if (!sql.Equals(""))//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                {
                    gridPesquisa.DataSource = Conexao.GetTable(sqlGrid + " where " + sql, null, false);
                    strSqltotalFiltro += " where " + sql;
                }
                else
                {
                    gridPesquisa.DataSource = Conexao.GetTable(sqlGrid, null, true);
                }
                gridPesquisa.DataBind();
                lblPesquisaErro.Text = "";
                String totalFiltro = Conexao.retornaUmValor(strSqltotalFiltro, null);
                lblRegistros.Text = totalFiltro + " Registros de  " + totalRegistro + " Cadastrados ";

            }
            catch (Exception err)
            {
                lblPesquisaErro.Text = err.Message;
            }
        }
        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisa();
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