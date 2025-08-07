using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Manutencao.pages
{
    public partial class ContratoFornecedor : visualSysWeb.code.PagePadrao     //inicio da classe 
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                carregarPesquisa("");
            }
            pesquisar(pnBtn);
        }

        private void carregarPesquisa(String Ordem)
        {
            User usr = (User)Session["User"];
            String strWhere = "";
            String sqlGrid = "select id_contrato,Fornecedor, descricao,Data_cadastro=convert(varchar,Data_cadastro,103),Data_validade=convert(varchar,Data_validade,103),prazo from Contrato_fornecedor";//colocar os campos no select que ser?o apresentados na tela

            String strTotalRegistro = Conexao.retornaUmValor("Select count(*) from (" + sqlGrid + ") as a ", null);




            if (!txtCodigo.Text.Equals(""))
            {
                strWhere += " id_contrato Like '" + txtCodigo.Text + "%'";
            }
            if (!txtFornecedor.Text.Equals(""))
            {
                if (strWhere.Length > 0)
                {
                    strWhere += " and ";
                }
                strWhere += " fornecedor like '%" + txtFornecedor.Text + "%' ";

            }
            if (!txtDataDe.Text.Equals(""))
            {
                if (strWhere.Length > 0)
                {
                    strWhere += " and ";
                }
                String tipoDt = "";
                if (ddlTipo.Text.Equals("Cadastro"))
                {
                    tipoDt = "data_Cadastro";
                }
                else
                {
                    tipoDt = "data_validade";
                }
                DateTime dtDe = new DateTime();
                DateTime dtAte = new DateTime();
                DateTime.TryParse(txtDataDe.Text, out dtDe);
                DateTime.TryParse(txtDataAte.Text, out dtAte);



                strWhere += "(" + tipoDt + " Between '" + dtDe.ToString("yyyy-MM-dd") + "' and '" + dtAte.ToString("yyyy-MM-dd") + "')";
            }

            if (strWhere.Length > 0)
            {
                sqlGrid += " Where " + strWhere;
            }
            String strTotalFiltro = Conexao.retornaUmValor("Select count(*) from (" + sqlGrid + ") as a ", null);

            if (strWhere.Equals(""))
            {
                lblRegistros.Text = strTotalRegistro + " Registros";
            }
            else
            {
                lblRegistros.Text = strTotalFiltro + " de " + strTotalRegistro + " Registros";
            }
            gridPesquisa.DataSource = Conexao.GetTable(sqlGrid, usr, false); ;
            gridPesquisa.DataBind();
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("ContratoFornecedorDetalhe.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
        }
        protected void txtDataDe_TextChanged(object sender, EventArgs e)
        {
            if (IsDate(txtDataDe.Text))
            {
                txtDataAte.Text = txtDataDe.Text;
            }
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            carregarPesquisa("");
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