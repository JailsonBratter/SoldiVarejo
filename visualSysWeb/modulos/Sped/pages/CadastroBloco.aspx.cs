using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using System.Data.SqlClient;

namespace visualSysWeb.modulos.Sped.pages
{
    public partial class CadastroBloco : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String sql = "Select id from sped_blocos where id_bloco_pai = 0 order by ordem";
            User usr = (User)Session["User"];
            SqlDataReader rs = null;
            try
            {


                rs = Conexao.consulta(sql, usr, false);


                while (rs.Read())
                {
                    int id = 0;
                    int.TryParse(rs["id"].ToString(), out id);
                    Sped_blocosDAO bloco = new Sped_blocosDAO(id, usr);
                    AddBloco(pnBlocos, bloco, 0);
                }
            }
            catch (Exception err)
            {

                lblPesquisaErro.Text = err.Message;
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }
        }

        protected void ImgEditarBloco_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void ImgAddBloco_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void ImgExcluirBloco_Click(object sender, ImageClickEventArgs e)
        {

        }



        private void AddBloco(Panel pnBlocoPrincipal, Sped_blocosDAO bloco, int vMargem)
        {
            String css = "";
            if (bloco.ordem % 2 == 0)
            {
                css = "celulaSpedPar";
            }
            else
            {
                css = "celulaSpedImpar ";
            }
 
            Panel cPn = new Panel();
            cPn.CssClass = "row";
            Panel cPnBloco = new Panel();
            cPnBloco.CssClass = css;
            Label lblBloco = new Label();
            lblBloco.Text = "<p>Bloco</p>" + bloco.bloco;
            cPnBloco.Controls.Add(lblBloco);

            cPn.Controls.Add(cPnBloco);

         


            Panel cPnProcedure = new Panel();
            cPnProcedure.CssClass = css;
            Label lblprocedure = new Label();
            lblprocedure.Text = "<p>Procedure</p>" + bloco.str_procedure;
            cPnProcedure.Controls.Add(lblprocedure);


            cPn.Controls.Add(cPnProcedure);

            Panel cPanel1 = new Panel();
            //cPanel1.CssClass = css;

            // Botão de Editar Bloco
            Panel cPnBtnEditar = new Panel();
            ImageButton imgEditarBloco = new ImageButton();
            imgEditarBloco.ImageUrl = "~/img/Edit.png";
            imgEditarBloco.Width = 30;
            imgEditarBloco.Click += ImgEditarBloco_Click;
            imgEditarBloco.ID = "Editar_" + bloco.id;
            cPnBtnEditar.Controls.Add(imgEditarBloco);
            cPanel1.Controls.Add(cPnBtnEditar);

            cPn.Controls.Add(cPanel1);
            cPn.Attributes.Add("style", "margin-left:" + vMargem + "px; margin-top:5px; width:80%; ");
            pnBlocoPrincipal.Controls.Add(cPn);
            foreach (Sped_blocosDAO item in bloco.arrBlocosFilhos)
            {
                AddBloco(pnBlocoPrincipal,item,vMargem+20);               
            }



        }
    }
}