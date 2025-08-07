using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class TabelaPreco : visualSysWeb.code.PagePadrao     //inicio da classe 
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
            Response.Redirect("~/modulos/cadastro/pages/TabelaPrecoDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
        }
        private void pesquisar()
        {
            String sqlGrid = "select Codigo_tabela,Nro_tabela, porc = case when porc < 0 then (porc * -1) else porc end, tipo =case when porc < 0 then 'ACRESCIMO' else 'DESCONTO' end from Tabela_Preco ";
            String sql = "";

            if (!TxtCodigo_Tabela.Text.Equals("")) 
            {
                sql = " codigo_tabela = '" + TxtCodigo_Tabela.Text + "'"; 
            }
            if (!txtNroTabela.Text.Equals("")) 
            {
                if (sql.Length > 0)
                    sql += " and ";

                sql += " nro_tabela like '%" + txtNroTabela.Text + "%'";
            }

            if (!ddlTipo.Text.Equals("TODOS"))
            {
                if (sql.Length > 0)
                    sql += " and ";

                sql += "case when porc < 0 then 'ACRESCIMO' else 'DESCONTO' end = '" + ddlTipo.Text + "'";
            }


            try
            {
                
                User usr = (User)Session["User"];

                String strSqltotalFiltro = "select count(*) from Tabela_Preco";
                String totalRegistro = Conexao.retornaUmValor(strSqltotalFiltro, null);

                //Grupo Empresa no cadastro de u´suário
                if (!usr.grupoClientes.Equals(""))
                {
                    if (sql.Length > 0)
                    {
                        sql += " AND";
                    }
                    sql += " Tabela_Preco.Codigo_tabela IN(SELECT Cliente.Codigo_Tabela FROM Cliente WHERE grupo_empresa IN('" + usr.grupoClientes + "'))";
                }

                strSqltotalFiltro +=  (sql.Length >0 ?" Where" + sql:"");


                gridPesquisa.DataSource = Conexao.GetTable(sqlGrid + (sql.Length > 0 ? " Where" + sql : ""), usr, false); 
                gridPesquisa.DataBind();

                String totalFiltro = Conexao.retornaUmValor(strSqltotalFiltro, usr);
                lblRegistros.Text = totalFiltro + " Registros de  " + totalRegistro + " Cadastrados ";
               
            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
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

        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            pesquisar();
        }

       
        protected void btnOkError_Click(object sender, EventArgs e)
        {
            modalError.Hide();
        }

        protected void msgShow(String mensagem, bool erro)
        {
            lblErroPanel.Text = mensagem;
            if (erro)
            {
                lblErroPanel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblErroPanel.ForeColor = System.Drawing.Color.Blue;
            }
            btnOkError.Focus();
            modalError.Show();
        }
    }
}