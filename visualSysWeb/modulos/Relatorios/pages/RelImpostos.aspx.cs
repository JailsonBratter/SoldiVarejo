using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using System.Data.SqlClient;
using System.Data;
using visualSysWeb.code;
using System.Collections;
using System.Web.Services;

namespace visualSysWeb.modulos.Relatorios.pages
{
    public partial class RelImpostos : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                User usr = (User)Session["User"];
                status = "pesquisar";
                carregarDados();
            }
            //status = "pesquisar";

            carregabtn(pnBtn);

        }
        protected void carregarDados() 
        {
            DataTable Tb01 = new DataTable();

            Tb01 = Conexao.GetTable("exec sp_REL_Impostos 'MATRIZ'", null, false);

            GridImpostos.DataSource = Tb01;
            GridImpostos.DataBind();
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {

            User usr = (User)Session["User"];
            string sql = "exec sp_REL_Impostos '" + usr.filial.Filial  + "'";
            DataTable Tb01 = new DataTable();

            if (!txtDe.Text.Equals(""))
            {

                sql += ", " + "'" + DateTime.Parse(txtDe.Text).ToString("yyyy-MM-dd").Replace("-","") + "', '" + DateTime.Parse(txtAte.Text).ToString("yyyy-MM-dd").Replace("-","") + "'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
            }

            Tb01 = Conexao.GetTable(sql, null, false);

            GridImpostos.DataSource = Tb01;
            GridImpostos.DataBind();
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override bool campoDesabilitado(Control campo)
        {
            throw new NotImplementedException();
        }

        protected override bool campoObrigatorio(Control campo)
        {
            throw new NotImplementedException();
        }
    }
}