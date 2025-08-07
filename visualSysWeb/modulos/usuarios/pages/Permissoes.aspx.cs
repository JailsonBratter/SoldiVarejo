using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using System.Data.SqlClient;


namespace visualSysWeb.modulos.usuarios.pages
{
    public partial class Permissoes : visualSysWeb.code.PagePadrao     //inicio da classe 
    {
        static DataTable tb;
        static String sqlGrid = "select * from usuarios_web";//colocar os campos no select que ser?o apresentados na tela
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                User usr = (User)Session["User"];
                if (usr != null)
                {
                    usr.consultaTodasFiliais = true;
                }
                tb = Conexao.GetTable(sqlGrid, usr,true);

                gridPesquisa.DataSource = tb;
                gridPesquisa.DataBind();
                Lblindex.Text = "1/" + gridPesquisa.PageCount;
            }
            sopesquisa(pnBtn);
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {}

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            String sql = "";
            if (!txtPESQ1.Text.Equals("")) //colocar nome do campo de pesquisa
            {
                sql = " usuario like '%" + txtPESQ1.Text + "%'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
            }
            if (!txtPESQ2.Text.Equals("")) //colocar nome do campo de pesquisa2
            {
                if (!sql.Equals(""))
                {
                    sql += " and ";
                }
                sql += "nome like '%" + txtPESQ2.Text + "%'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
            }
            try
            {
                User usr = (User)Session["User"];
                if (usr != null)
                {
                    usr.consultaTodasFiliais = true;
                }

                if (!sql.Equals(""))
                {
                    tb = Conexao.GetTable(sqlGrid + " where " + sql, usr,false);
                }
                else
                {
                    tb = Conexao.GetTable(sqlGrid, usr,true);
                }
                gridPesquisa.DataSource = tb;
                gridPesquisa.DataBind();//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                lblPesquisaErro.Text = "";
                Lblindex.Text = "1/" + gridPesquisa.PageCount;
            }
            catch (Exception err)
            {
                lblPesquisaErro.Text = err.Message;
            }
        }
        protected override void btnEditar_Click(object sender, EventArgs e) { }
        protected override void btnExcluir_Click(object sender, EventArgs e) { }
        protected override void btnConfirmar_Click(object sender, EventArgs e) { }
        protected override void btnCancelar_Click(object sender, EventArgs e) { }


        protected void gridPesquisa_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridPesquisa.DataSource = tb;
            gridPesquisa.PageIndex = e.NewPageIndex;
            Lblindex.Text = (e.NewPageIndex + 1) + "/" + gridPesquisa.PageCount;
            gridPesquisa.DataBind();
        }
        protected override bool campoObrigatorio(Control campo)
        {
            return false;
        }

        protected override bool campoDesabilitado(Control campo)
        {
            return false;
        }
    }
}