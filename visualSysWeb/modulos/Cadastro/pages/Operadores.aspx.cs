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
    public partial class Operadores : visualSysWeb.code.PagePadrao     //inicio da classe 
    {

        static String sqlGrid = "select * from Operadores";//colocar os campos no select que ser?o apresentados na tela
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
            Response.Redirect("~/modulos/cadastro/pages/OperadoresDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
        }
        private void pesquisar()
        {

            gridPesquisa.Columns[0].Visible = chkInativo.Checked;
            String sql = "";

            
            if (chkInativo.Checked)
            {
                sql += " isnull(inativo,0) = 1";
            }
            else
            {
                sql += " isnull(inativo,0) <> 1";
            }


            if (!txtId_operador.Text.Equals("")) //colocar nome do campo de pesquisa
            {
                sql = " and id_operador = '" + txtId_operador.Text + "'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
            }
            if (!txtNome.Text.Equals("")) //colocar nome do campo de pesquisa2
            {
                sql += " and nome like '%" + txtNome.Text + "%'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
            }



            try
            {
                DataTable tb;
                User usr = (User)Session["User"];

                tb = Conexao.GetTable(sqlGrid + " where " + sql, usr, false);
                
                String strSqltotalFiltro = "select count(*) from Operadores where isnull(inativo,0)"+(chkInativo.Checked?" =1 ":" <> 1 ");
                 String totalRegistro = Conexao.retornaUmValor(strSqltotalFiltro, null);

                strSqltotalFiltro += " and"+sql;

                gridPesquisa.DataSource = tb;
                gridPesquisa.DataBind();
                lblPesquisaErro.Text = "";
                String totalFiltro = Conexao.retornaUmValor(strSqltotalFiltro, usr);
                lblRegistros.Text = totalFiltro + " Registros de  " + totalRegistro + " Cadastrados ";
                divSalvaInativos.Visible = chkInativo.Checked ;
                if (chkInativo.Checked)
                {
                    
                    foreach (GridViewRow item in gridPesquisa.Rows)
                    {
                        item.CssClass = "linhaInativo";
                    }
                }
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

        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            pesquisar();
        }

        protected void imgBtnSalvarInativos_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow item in gridPesquisa.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        HyperLink meuLink = (HyperLink)item.Cells[1].Controls[0];
                        Conexao.executarSql("update operadores set inativo=0 where id_operador = '" + meuLink.Text + "'");
                    }
                }
            }
            pesquisar();
        }
    }
}