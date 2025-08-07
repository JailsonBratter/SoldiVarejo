using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class Filial : visualSysWeb.code.PagePadrao     //inicio da classe 
    {
        static String sqlGrid = "select * from Filial";//colocar os campos no select que ser?o apresentados na tela
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pesquisa();

            }
            pesquisar(pnBtn);
            
        }

        

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/manutencao/pages/FilialDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
        }
        private void pesquisa()
        {
            String sql = "";
            String strSqltotalFiltro = "select count(*) from filial";
            String totalRegistro = Conexao.retornaUmValor(strSqltotalFiltro, null);

            if (!txtFilial.Text.Equals("")) //colocar nome do campo de pesquisa
            {
                sql = " filial like '%" + txtFilial.Text + "%'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
            }
            if (!txtRazaoSocial.Text.Equals("")) //colocar nome do campo de pesquisa2
            {
                if (!sql.Equals(""))
                {
                    sql += " and ";
                }
                sql += "(razao_social like '%" + txtRazaoSocial.Text + "%' or fantasia like '%"+txtRazaoSocial.Text+"%'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
            }
            if (!txtCnpj.Text.Equals(""))
            {
                if (!sql.Equals(""))
                {
                    sql += " and ";
                }
                sql += " replace(replace(replace(cnpj,'.',''),'-',''),'/','')=" + txtCnpj.Text.Replace(".", "").Replace("-", "").Replace("/", "") + "%'";
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