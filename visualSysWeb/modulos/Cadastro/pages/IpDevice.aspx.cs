using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class IpDevice : visualSysWeb.code.PagePadrao     //inicio da classe 
    {
        static DataTable tb;
        static String sqlGrid = "select * from ipDevice";//colocar os campos no select que ser?o apresentados na tela
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pesquisa();

            }
            pesquisar(pnBtn);
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/Cadastro/pages/ipDeviceDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
        }

        private void pesquisa()
        {
            User usr = (User)Session["User"];
            String sql = "";
            if (!txtId.Text.Equals("")) 
            {
                sql = " id =" + txtId.Text ; 
            }
            if (!txtIp.Text.Equals("")) //colocar nome do campo de pesquisa2
            {
                if (!sql.Equals(""))
                {
                    sql += " and ";
                }
                sql += "ip like '%" + txtIp.Text + "%'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
            }
            try
            {
                if (!sql.Equals(""))
                {
                    tb = Conexao.GetTable(sqlGrid + " where " + sql, usr,false);
                }
                else
                {
                    tb = Conexao.GetTable(sqlGrid, usr,false);
                }
                gridPesquisa.DataSource = tb;
                gridPesquisa.DataBind();
                lblPesquisaErro.Text = "";
            }
            catch (Exception err)
            {
                lblPesquisaErro.Text = err.Message;
            }

            tb = Conexao.GetTable(sqlGrid, usr, false);
            gridPesquisa.DataSource = tb;
            gridPesquisa.DataBind();
        }
        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisa();
        }
        protected override void btnEditar_Click(object sender, EventArgs e) { }
        protected override void btnExcluir_Click(object sender, EventArgs e) { }
        protected override void btnConfirmar_Click(object sender, EventArgs e) { }
        protected override void btnCancelar_Click(object sender, EventArgs e) { }


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