using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.pedidos
{
    public partial class ImprimePedido : System.Web.UI.Page
    {
        static pedidoDAO obj;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
            {
              Response.Redirect("~/Account/Login.aspx");
            }
            if (Request.Params["pedido"] != null)
            {
                String pedido = Request.Params["pedido"].ToString();
                obj = new pedidoDAO(pedido);
                carregarDados();
            }
        }

        public void carregarDados()
        {

            if (obj.Tipo == 1)
            {
                lblTitulo.Text = "Pedido de Venda";
                txtFantasia.Text = obj.fantasia.ToString();
            }
            else {
                lblTitulo.Text = "Pedido de Compra";
            }
            txtFilial.Text = obj.Filial.ToString();
            txtPedido.Text = obj.Pedido.ToString();
            txtStatus.Text = obj.getStatus();

            txtCliente_Fornec.Text = obj.Cliente_Fornec.ToString();
            txtNome.Text = obj.nome;
            
            txtData_cadastro.Text = obj.Data_cadastroBr();
            txtData_entrega.Text = obj.Data_entregaBr();

            txtDesconto.Text = string.Format("{0:0,0.00}", obj.Desconto);
            txtTotal.Text = string.Format("{0:0,0.00}", obj.Total);
            txtUsuario.Text = obj.Usuario.ToString();
            txtObs.Text = obj.Obs.ToString();
            txtcfop.Text = obj.cfop.ToString();
            txtfuncionario.Text = obj.funcionario.ToString();
            txthora_fim.Text = obj.hora_fim.ToString();
            gridItens.DataSource = obj.itens;
            gridItens.DataBind();
            gridPagamentos.DataSource = obj.pagamentos;
            gridPagamentos.DataBind();

            if (txtFantasia.Text.Equals("")){
                nomeFantasia.Visible = false;

            }
            
        }
    }
}