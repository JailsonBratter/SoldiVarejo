using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.code
{
    public partial class ConfiguracaoColetor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            if (usr == null)
                return;


            if (!IsPostBack)
            {
                if(Request.Params["nf"]!=null)
                {
                    divPreco.Visible = true;
                }
                else
                {
                    divPreco.Visible = false;
                }
                Coletor coletor = new Coletor();
                RdoTipoDeArquivo.SelectedValue = (coletor.coletorFixo ? "0" : "1");
                txtDel.Text = coletor.delimitador;
                txtinicioPlu.Text = coletor.pluInicio.ToString();
                txtFimPlu.Text = coletor.pluFim.ToString();
                txtDel.Text = coletor.delimitador;
                txtInicioContado.Text = coletor.contadoInicio.ToString();
                txtFimContado.Text = coletor.contadoFim.ToString();
                txtDecimaisContado.Text = coletor.contadoDecimal.ToString();

                txtPrecoInicio.Text = coletor.precoInicio.ToString();
                txtPrecoFim.Text = coletor.precoFim.ToString();
                txtPrecoCasasDecimais.Text = coletor.precoDecimal.ToString();

                tela();
            }
        }


        protected void RdoTipoDeArquivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            tela();
            
        }

        protected void tela()
        {
            if (RdoTipoDeArquivo.SelectedValue.Equals("0"))
            {
                //txtDel.Text = "";
                txtDel.Visible = false;
                lblDel.Visible = false;
                lblFimPlu.Visible = true;
                txtFimPlu.Visible = true;
                lblFimContado.Visible = true;
                txtFimContado.Visible = true;
                if (divPreco.Visible)
                {
                    lblPrecoFim.Visible = true;
                    txtPrecoFim.Visible = true;
                }
                lblInicioPlu.Text = "Inicio";
                lblInicioContado.Text = "Inicio";
                lblInicioPreco.Text = "Inicio";

                txtinicioPlu.Focus();
            }
            else
            {
                lblDel.Visible = true;
                txtDel.Visible = true;
                txtDel.Focus();
                lblFimPlu.Visible = false;
                txtFimPlu.Visible = false;
                lblFimContado.Visible = false;
                txtFimContado.Visible = false;
                if (divPreco.Visible)
                {
                    lblPrecoFim.Visible = false;
                    txtPrecoFim.Visible = false;
                }

                lblInicioPlu.Text = "Posição";
                lblInicioContado.Text = "Posição";
                lblInicioPreco.Text = "Posição";

            }
        }
        protected void btnSalvarConfigColetor_Click(object sender, EventArgs e)
        {
            
            Coletor coletor = new Coletor();
            coletor.coletorFixo = RdoTipoDeArquivo.SelectedItem.Value.Equals("0");
            int.TryParse(txtinicioPlu.Text, out coletor.pluInicio);
            int.TryParse(txtFimPlu.Text, out coletor.pluFim);
            coletor.delimitador = txtDel.Text;
            int.TryParse(txtInicioContado.Text , out  coletor.contadoInicio);
            int.TryParse(txtFimContado.Text , out  coletor.contadoFim);
            int.TryParse(txtDecimaisContado.Text ,out coletor.contadoDecimal);

            coletor.precoInicio = Funcoes.intTry(txtPrecoInicio.Text);
            coletor.precoFim = Funcoes.intTry(txtPrecoFim.Text);
            coletor.precoDecimal = Funcoes.intTry(txtPrecoCasasDecimais.Text);

            coletor.salvar();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "window.close()", true);


        }
    }
}