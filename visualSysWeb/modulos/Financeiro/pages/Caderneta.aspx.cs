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
    public partial class Caderneta : visualSysWeb.code.PagePadrao     //inicio da classe 
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pesquisaCaderneta();
            }
            sopesquisa(pnBtn);
        }

        protected void pesquisaCaderneta()
        {
            String SqlWhere = " Where isnull(conta_assinada,0) = 1 and ISNULL(inativo,0) = 0 ";
            String sqlGrid = "select cliente.Codigo_Cliente, " +
                             "  cliente.cnpj , "+
                             "  cliente.Nome_Cliente, " +
                             "  cliente.Limite_Credito, " +
                             "  Utilizado = cliente.Utilizado, " +
                             "  cliente.situacao "+
                             " From cliente ";

            
            if (!txtCliente.Text.Equals(""))
            {
                SqlWhere += " and (cliente.Codigo_Cliente='" + txtCliente.Text + "' or cliente.nome_cliente like '%" + txtCliente.Text + "%')";

            }
            if (!ddlStatus.SelectedValue.Equals(""))
            {
                
                SqlWhere += " and cliente.Situacao ='" + ddlStatus.SelectedValue + "'";
            }

            if(!txtCNPJ.Text.Equals(""))
            {
                SqlWhere += " and replace(replace(replace(cnpj,'.',''),'-',''),'/','') like '%" + txtCNPJ.Text.Replace(".", "").Replace("-", "").Replace("/", "") + "%'";
            }


            sqlGrid +=  SqlWhere;

            User usr = (User)Session["User"];
            DataTable tb = Conexao.GetTable(sqlGrid, usr, true);
            gridPesquisa.DataSource = tb;
            gridPesquisa.DataBind();

        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/financeiro/pages/CadernetaDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisaCaderneta();
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