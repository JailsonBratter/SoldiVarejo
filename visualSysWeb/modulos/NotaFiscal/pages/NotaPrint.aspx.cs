using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.NotaFiscal.pages
{
    public partial class NotaPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            

            carregarDados();
        }

        protected void carregarDados()
        {
            nfDAO obj = (nfDAO)Session["nfImprime"];
            txtCodigo.Text = obj.Codigo.ToString() + "";
            TxtNomeFornecedor.Text = obj.Nome_Fornecedor.ToString() + "";
            txtData.Text = obj.DataBr();
            txtCodigo_operacao.Text = (obj.Codigo_operacao.ToString().Equals("0") ? "" : obj.Codigo_operacao.ToString());
            txtEmissao.Text = obj.EmissaoBr();
            obj.calculaTotalItens();

            txtTotal.Text = obj.Total.ToString("N2");
            txtTotalProdutos.Text = obj.valorTotalProdutos.ToString("N2");
            txtDesconto.Text = string.Format("{0:0,0.00}", obj.Desconto);
            txtFrete.Text = string.Format("{0:0,0.00}", obj.Frete);
            txtSeguro.Text = string.Format("{0:0,0.00}", obj.Seguro);
            txtIPI_Nota.Text = string.Format("{0:0,0.00}", obj.IPI_Nota);
            txtOutras.Text = string.Format("{0:0,0.00}", obj.Outras);
            txtICMS_Nota.Text = string.Format("{0:0,0.00}", obj.ICMS_Nota);

            txtBase_Calculo.Text = string.Format("{0:0,0.00}", obj.Base_Calculo);
            txtDespesas_financeiras.Text = string.Format("{0:0,0.00}", obj.Despesas_financeiras);
            txtPedido.Text = obj.Pedido.ToString() + "";
            txtBase_Calc_Subst.Text = string.Format("{0:0,0.00}", obj.Base_Calc_Subst);
            txtObservacao.Text = obj.Observacao.ToString() + "";
            chknf_Canc.Checked = obj.nf_Canc;


            txtcentro_custo.Text = obj.centro_custo.ToString();

            txtICMS_ST.Text = string.Format("{0:0,0.00}", obj.ICMS_ST);

            txtFornecedor_CNPJ.Text = obj.Fornecedor_CNPJ.ToString();

            txtDesconto_geral.Text = string.Format("{0:0,0.00}", obj.Desconto_geral);
            txtusuario.Text = obj.usuario.ToString();
            txtid.Text = obj.id.ToString();
            gridItens.DataSource = obj.nfItens();
            gridItens.DataBind();

            gridPagamentos.DataSource = obj.nfPagamento();
            gridPagamentos.DataBind();


        }

    }
}