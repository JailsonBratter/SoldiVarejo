using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using System.Text;

namespace visualSysWeb.modulos.Pedidos.pages
{
    public partial class PedidoDevolucaoPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {


                User usr = (User)Session["User"];
                if (usr == null)
                {
                    throw new Exception("NÃO FOI POSSIVEL IDENTIFICAR O USUARIO LOGADO");

                }

                if (Request.Params["codigo"] == null)
                {
                    throw new Exception("NÃO FOI POSSIVEL IDENTIFICAR O PEDIDO");

                }

                pedidoDAO ped = new pedidoDAO(Request.Params["codigo"].ToString(), 3, usr);
                lblPedido.Text = ped.Pedido;
                lblVendedor.Text = ped.funcionario;
                switch (ped.Status)
                {
                    case 1:
                        lblStatus.Text = "ABERTO";
                        break;
                    case 2:
                        lblStatus.Text = "FECHADO";
                        break;
                    case 3:
                        lblStatus.Text = "CANCELADO";
                        break;
                }
                lblData.Text = ped.Data_cadastroBr();
                lblDataEntrega.Text = ped.Data_entregaBr();
                lblHora.Text = ped.hora;
                lblUsuario.Text = ped.Usuario;
                lblNaturezaOp.Text = ped.CFOP.ToString("N0");
                lblCodigoCliente.Text = ped.Cliente_Fornec;
                lblNomeCliente.Text = ped.NomeCliente;
                lblCnpj.Text = Conexao.retornaUmValor("select CNPJ from cliente where codigo_cliente='" + ped.Cliente_Fornec + "'", null);

                lblValor.Text = ped.Total.ToString("N2");
                gridItens.DataSource = ped.PdItens();
                gridItens.DataBind();
                if (ped.Obs.Trim().Length > 0)
                {
                    divObs.Visible = true;
                    lblObservacao.Text = ped.Obs.Replace("\n", "<br>");
                }
                else
                {
                    divObs.Visible = false;
                 
                }

            }
            catch (Exception err)
            {

                divPage.InnerHtml = err.Message;

            }

        }
    }
}