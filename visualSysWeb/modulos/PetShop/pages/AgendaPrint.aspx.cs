using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.PetShop.pages
{
    public partial class AgendaPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            if (usr == null)
            {
                Response.Redirect("~");
            }
            if (Request.Params["pedido"] != null)
            {
                String codigo = Request.Params["pedido"].ToString();

                if (!codigo.Equals(""))
                {
                    AgendaDAO ag = new AgendaDAO(codigo, usr);
                    lblPedido.Text = ag.Pedido;
                    lblUsuario.Text = ag.usuario_cadastro;
                    lblFuncionario.Text = ag.Nome;
                    lblCliente.Text = ag.Codigo_Cliente + "-" + ag.NomeCliente;
                    lblPet.Text = ag.Nome_Pet;
                    lblData.Text = ag.DataBr();
                    lblInicio.Text = ag.Inicio;
                    lblFim.Text = ag.Fim;
                    lblDelivery.Text = (ag.delivery ? "SIM" : "NÃO");
                    lblRetirada.Text = ag.Hora_retirada;
                    lblFuncionarioRetira.Text = ag.Funcionario_retira;
                    lblHoraPrevista.Text = ag.Hora_entrega_prevista;
                    lblHoraReal.Text = ag.Hora_entrega_real;
                    lblFuncionarioEntrega.Text = ag.Funcionario_entrega;
                    string[] arrLinha = ag.Sigla.ToString().Split(';');
                    lblServicos.Text ="";
                    foreach (string item in arrLinha)
                    {
                        if (!item.Trim().Equals(""))
                        {
                            lblServicos.Text += item+"<br>";
                        }
                    }
                    lblObservacoes.Text = ag.Obs;
                    lblObsVeterinario.Text = ag.Obs_Veterinario;
                    lblKmSaida.Text = ag.Saida_KM.ToString();
                    lblKmChegada.Text = ag.Chegada_KM.ToString();

                }
            }

        }
    }
}