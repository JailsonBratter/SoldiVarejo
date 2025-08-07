using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class ContaCorrente : visualSysWeb.code.PagePadrao     //inicio da classe 
    {
        
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
            Response.Redirect("~/modulos/cadastro/pages/ContaCorrenteDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
        }
        private void pesquisar()
        {
            String sqlGrid = "Select Conta_Corrente.*,case when isnull(conta_caixa,0)=1 then 'SIM' ELSE 'NAO' END AS cCaixa, isnull(banco.Nome_Banco,conta_corrente.filial) as Nome_banco from Conta_Corrente left join Banco  on Conta_Corrente.banco = banco.Numero_banco";//colocar os campos no select que ser?o apresentados na tela
            String sql = "";
            String strSqltotalFiltro = "select count(*) from Conta_Corrente left join Banco  on Conta_Corrente.banco = banco.Numero_banco";
            String totalRegistro = Conexao.retornaUmValor(strSqltotalFiltro, null);

            if (!txtId_Conta.Text.Equals("")) //colocar nome do campo de pesquisa
            {
                sql = " id_cc like '%" + txtId_Conta.Text + "%'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
            }
            if (!txtBanco.Text.Equals("")) //colocar nome do campo de pesquisa2
            {
                if (!sql.Equals(""))
                {
                    sql += " and ";
                }
                sql += " (isnull(banco.Nome_Banco,conta_corrente.filial) like '%" + txtBanco.Text + "%' or conta_corrente.banco ='" + txtBanco.Text + "') "; 
            }
            try
            {
                User usr = (User)Session["User"];
                if (!sql.Equals(""))
                {
                    gridPesquisa.DataSource = Conexao.GetTable(sqlGrid + " where " + sql, usr, false);
                    strSqltotalFiltro += " Where " + sql;
                }
                else
                {
                    gridPesquisa.DataSource  = Conexao.GetTable(sqlGrid, usr, false);
                }
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