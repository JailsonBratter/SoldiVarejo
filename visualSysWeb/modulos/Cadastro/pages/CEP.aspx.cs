using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class CEP  : visualSysWeb.code.PagePadrao
    
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pesquisar();
            }
            sopesquisa(pnBtns);
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
            pesquisar();
                

        }
        protected void pesquisar()
        {
            String strSql = "select top 1000 * from CEP_Brasil where (cep+Logradouro+Bairro+Cidade+UF) like '%" + txtPesquisa.Text.Replace(" ", "%") + "%'";

           
            try
            {

                gridCEP.DataSource = Conexao.GetTable(strSql, null, false);
                gridCEP.DataBind();
             

            }
            catch (Exception err)
            {

                lblErro.Text = err.Message;
            }
            finally
            {
              
            }

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