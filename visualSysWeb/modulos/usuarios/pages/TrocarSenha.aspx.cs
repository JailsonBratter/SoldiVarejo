using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.usuarios.pages
{
    public partial class TrocarSenha : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            User usr = (User)Session["user"];

            try
            {
                usr.trocarSenha(txtSenha.Text, txtNovaSenha.Text, txtConfirmaNova.Text);
                lblerros.Text = "Senha Alterada com sucesso";
                lblerros.ForeColor = System.Drawing.Color.Blue;
            }
            catch (Exception err)
            {

                lblerros.Text = err.Message;
                lblerros.ForeColor = System.Drawing.Color.Red;
            }

        }//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
    }
}