using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Mobile.pages
{
    public partial class SmartPhone_Resultado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
                Response.Redirect("Mobile_Login.aspx");

            string tela = Request["tela"];

            if (!IsPostBack)
            {

                ddlDias.Items.Clear();
                for (int i = 0; i < 5; i++)
                {
                    ddlDias.Items.Add(DateTime.Now.AddDays(-i).ToString("dd/MM/yyyy"));

                }

                lblTitulo.Text = (tela.Equals("0") ? "FATURAMENTO" : (tela.Equals("1") ? "CONTAS A PAGAR" : "CONTAS A RECEBER")) + " - " + DateTime.Today.ToString("MMM/yyy").ToUpper();
                switch (tela)
                {
                    case "0":
                        lblTitulo.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0080FF"); 
                        break;
                    case "1":
                        lblTitulo.ForeColor = System.Drawing.ColorTranslator.FromHtml("#e04040");
                        break;
                    case "2":
                        lblTitulo.ForeColor = System.Drawing.ColorTranslator.FromHtml("#67cc3e");
                        break;
                }
                atualizar();
           }

        }

        protected void ddlDias_SelectedIndexChanged(object sender, EventArgs e)
        {
            atualizar();
        }


        protected void atualizar()
        {
            string tela = Request["tela"];
            DataTable tb = new DataTable();
            User usr = (User)Session["User"];
            String sqlGrid = "sp_Cons_SmartPhone '" + usr.getFilial() + "'," + tela+ ", '" + Funcoes.dtTry(ddlDias.SelectedItem.Text).ToString("yyyy-MM-dd") + "'";

            tb = Conexao.GetTable(sqlGrid, usr, false);

            DataTable Grafico = new DataTable();

            Grafico.Columns.Add("Coluna", typeof(string));

            Grafico.Columns.Add("Valor", typeof(double));

            DataRow[] rowGrafico = tb.Select("VisualizaGrafico = 1");

            foreach (DataRow row in rowGrafico)

            {

                Grafico.Rows.Add(row[0], row[1]);

            }



            Grafico1.DataSource = Grafico;
            Grafico1.DataBind();
            Grafico1.ChartAreas[0].Area3DStyle.Enable3D = true;

            gridPesquisa.DataSource = tb;
            gridPesquisa.DataBind();
        }
    }
}