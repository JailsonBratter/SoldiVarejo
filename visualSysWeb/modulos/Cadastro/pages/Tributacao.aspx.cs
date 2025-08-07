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
    public partial class Tributacao : visualSysWeb.code.PagePadrao     //inicio da classe 
    {
        static String sqlGrid = "select * from tributacao";//colocar os campos no select que ser?o apresentados na tela
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pesquisar();
            }
            pesquisar(pnBtn);
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/cadastro/pages/TributacaoDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
        }

        private void pesquisar()
        {
            String sql = "";
            String strSqltotalFiltro = "select count(*) from tributacao ";
            String totalRegistro = Conexao.retornaUmValor(strSqltotalFiltro, null);

            if (!txtCodigo.Text.Equals("")) //colocar nome do campo de pesquisa
            {
                sql = " codigo_tributacao = '" + txtCodigo.Text + "'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
            }
            if (!txtDescricao.Text.Equals("")) //colocar nome do campo de pesquisa2
            {
                if (!sql.Equals(""))
                {
                    sql += " and ";
                }
                sql += " Descricao_tributacao like '%" + txtDescricao.Text + "%'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
            }
            try
            {
                User usr = (User)Session["User"];
                DataTable tb = new DataTable();
                if (!sql.Equals(""))
                {
                    tb = Conexao.GetTable(sqlGrid + " where " + sql, usr, false);
                    strSqltotalFiltro += " where " + sql;

                }
                else
                {
                    tb = Conexao.GetTable(sqlGrid, usr, true);
                }
                gridPesquisa.DataSource = tb;
                gridPesquisa.DataBind();
                lblPesquisaErro.Text = "";
                String totalFiltro = Conexao.retornaUmValor(strSqltotalFiltro, usr);
                lblRegistros.Text = totalFiltro + " Registros de  " + totalRegistro + " Cadastrados ";

            }
            catch (Exception err)
            {
                lblPesquisaErro.Text = err.Message;
            }
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisar();
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