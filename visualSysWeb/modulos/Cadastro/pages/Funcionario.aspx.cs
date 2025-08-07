using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using visualSysWeb.modulos.Cadastro.code;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class Funcionario : visualSysWeb.code.PagePadrao     //inicio da classe 
    {

        static String sqlGrid = "select * from funcionario";//colocar os campos no select que ser?o apresentados na tela
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                

                Conexao.preencherDDL(ddlFuncao, "Select funcao from funcao");
                FiltroFuncionario filtro = (FiltroFuncionario)Session["filtroFuncionario"];
                if (filtro != null)
                {
                    txtCodigo.Text = filtro.codigo;
                    txtNome.Text = filtro.nome;
                    ddlFuncao.Text = filtro.funcao;
                }
                pesquisa();
            }
            pesquisar(pnBtn);
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/cadastro/pages/funcionarioDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
        }
        private void pesquisa()
        {
            FiltroFuncionario filtro = new FiltroFuncionario();
            filtro.codigo = txtCodigo.Text;
            filtro.nome = txtNome.Text;
            filtro.funcao = ddlFuncao.Text;
            Session.Remove("filtroFuncionario");
            Session.Add("filtroFuncionario", filtro);

            String sql = "";
            String strSqltotalFiltro = "select count(*) from funcionario ";
            String totalRegistro = Conexao.retornaUmValor(strSqltotalFiltro, null);

            if (!txtCodigo.Text.Equals(""))
            {
                sql = " codigo ='" + txtCodigo.Text + "'";
            }
            if (!txtNome.Text.Equals(""))
            {
                if (!sql.Equals(""))
                {
                    sql += " and ";
                }
                sql += " (nome Like '%" + txtNome.Text + "%' or nome2 like '%" + txtNome.Text + "%' or sobrenome like '%" + txtNome.Text + "%')";
            }
            if (!ddlFuncao.Text.Equals(""))
            {
                if (!sql.Equals(""))
                {
                    sql += " and ";
                }
                sql += " funcao ='" + ddlFuncao.Text + "'";
            }
            try
            {
                DataTable tb;
                User usr = (User)Session["User"];
                if (!sql.Equals(""))
                {
                    tb = Conexao.GetTable(sqlGrid + " where " + sql, usr, false);
                    strSqltotalFiltro += " Where " + sql;
                }
                else
                {
                    tb = Conexao.GetTable(sqlGrid, usr, false);
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