using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using System.Data.SqlClient;
using System.Collections;
using visualSysWeb.modulos.Cadastro.code;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class Cardapios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            carregarDados();
        }
        public void carregarDados()
        {

            User usr = (User)Session["User"];
            DataTable dt = new DataTable();
            string sql = "SELECT ID, Titulo, data_ultima_alteracao, usuario_alteracao FROM Cardapio";
            dt = Conexao.GetTable(sql, usr, false);

            gridCardapios.DataSource = dt;
            gridCardapios.DataBind();

            String totalRegistros = Conexao.retornaUmValor("SELECT COUNT(*) FROM Cardapio", usr);
            lblRegistros.Text = totalRegistros + " Registros";
        }
    }
}