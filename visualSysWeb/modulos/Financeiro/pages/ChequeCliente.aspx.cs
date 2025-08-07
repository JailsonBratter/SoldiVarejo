using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Financeiro.pages
{
    public partial class ChequeCliente : visualSysWeb.code.PagePadrao     //inicio da classe 
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pesquisaCheque();
            }
            EnabledControls(filtrosPesq, true);
            pesquisar(pnBtn);
        }

        protected void pesquisaCheque()
        {
            String SqlWhere = "";
            String sqlGrid = "select cliente.Codigo_Cliente, " +
                             "  cliente.Nome_Cliente, " +
                             "  cliente.CNPJ," +
                             "  QtdCheques=COUNT(a.Numero_Cheque), ";

            sqlGrid += "  UltimoCheque=(select top 1 convert(varchar,c.Numero_Cheque,103) from Cheque c where c.Codigo_Cliente =a.Codigo_Cliente ";
            if (!txtDe.Text.Equals("") && !txtAte.Text.Equals(""))
            {
                sqlGrid += " and c.emissao_cheque between '" + DateTime.Parse(txtDe.Text).ToString("yyyy-MM-dd") + "' and '" + DateTime.Parse(txtAte.Text).ToString("yyyy-MM-dd") + "' ";
            }
            sqlGrid += " order by convert(varchar,c.Emissao_Cheque,102) desc), ";
            sqlGrid += "  Emissao=(select top 1 convert(varchar,c.Emissao_Cheque,103) from Cheque c where c.Codigo_Cliente =a.Codigo_Cliente  ";
            if (!txtDe.Text.Equals("") && !txtAte.Text.Equals(""))
            {
                sqlGrid += " and " + ddlTipoPesquisa.SelectedValue + " between '" + DateTime.Parse(txtDe.Text).ToString("yyyy-MM-dd") + "' and '" + DateTime.Parse(txtAte.Text).ToString("yyyy-MM-dd") + "' ";
            }

            sqlGrid += "order by convert(varchar,c.Emissao_Cheque,102) desc) ";

            sqlGrid += " from Cheque a inner join cliente on a.Codigo_Cliente = cliente.Codigo_Cliente ";
            if (!txtDe.Text.Equals("") && !txtAte.Text.Equals(""))
            {

                SqlWhere = "  a." + ddlTipoPesquisa.SelectedValue + "  between '" + DateTime.Parse(txtDe.Text).ToString("yyyy-MM-dd") + "' and '" + DateTime.Parse(txtAte.Text).ToString("yyyy-MM-dd") + "' ";
            }
            if (!txtCliente.Text.Equals(""))
            {
                if (!SqlWhere.Trim().Equals(""))
                {
                    SqlWhere += " and ";
                }

                SqlWhere += " (cliente.Codigo_Cliente='" + txtCliente.Text + "' or cliente.nome_cliente like '%" + txtCliente.Text + "%')";

            }

            if (!txtCnpjCpf.Text.Equals(""))
            {
                if (!SqlWhere.Trim().Equals(""))
                {
                    SqlWhere += " and ";
                }

                SqlWhere += " (replace(replace(replace(cliente.cnpj,'.',''),'-',''),'/','') like '%" + txtCnpjCpf.Text.Replace(".", "").Replace("-", "").Replace("/", "") + "%')";

            }




            if (!SqlWhere.Equals(""))
                sqlGrid += " Where " + SqlWhere;

            sqlGrid += " group by cliente.Codigo_Cliente,cliente.Nome_Cliente,cliente.CNPJ,a.Codigo_Cliente";


            User usr = (User)Session["User"];
            DataTable tb = Conexao.GetTable(sqlGrid, usr, false);
            gridPesquisa.DataSource = tb;
            gridPesquisa.DataBind();

        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/financeiro/pages/ChequeClienteDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisaCheque();
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

        protected void txtDe_TextChanged(object sender, EventArgs e)
        {
            txtAte.Text = txtDe.Text;
        }
    }
}