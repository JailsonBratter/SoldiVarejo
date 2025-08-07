using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.modulos.Mobile.code;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Mobile
{
    public partial class SmartPhone_EspelhoCego : System.Web.UI.Page
    {
        String chave = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            NotaEspelho nota = null;

            chave = Request.Params["chave"].ToString();// colocar o campo index da tabela

            try
            {
                nota = (NotaEspelho)Session["objNota" + chave];
            }
            catch (Exception)
            {

                Session.Remove("objNota" + chave);
                nota = null;
            }


            if (!IsPostBack)
            {
                txtcodigo.Attributes.Add("onkeypress", "return validaTecla(this, event)");
                txtQuantidade.Attributes.Add("OnChange", "calculaQtde();");
                txtEmbalagem.Attributes.Add("OnChange", "calculaQtde();");
                //btnFechar.Attributes.Add("onClick", "fechar()");

            }
            //imgAdd.Enabled = false;
            //calcula();
        }
        protected void buscaCodigo(object sender, EventArgs e)
        {
            txtEmbalagem.Enabled = false;
            txtQuantidade.Enabled = false;
            txtQuantidadeTotal.Enabled = false;
            //imgAdd.Enabled = false;

            ItensNFe item = new ItensNFe(txtcodigo.Text.Trim());
            if (item.plu == null)
            {
                lblDescricao.Text = "PRODUTO NÃO CADASTRADO\n\rFavor entrar em contato com responsável.";
                lblDescricao.ForeColor = System.Drawing.Color.Red;
                return;
            }
            //lblDescricao.Text = item.descricao;
            lblDescricao.ForeColor = System.Drawing.Color.Blue;
            txtEmbalagem.Enabled = true;
            txtQuantidade.Enabled = true;
            txtEmbalagem.Focus();
            //imgAdd.Enabled = true;
        }

        protected void calcula()
        {
            if (Funcoes.intTry(txtEmbalagem.Text) > 0 && Funcoes.intTry(txtQuantidade.Text) > 0)
            {
                txtQuantidadeTotal.Text = (Funcoes.intTry(txtEmbalagem.Text) * Funcoes.intTry(txtQuantidade.Text)).ToString();
            }
        }
    }
}