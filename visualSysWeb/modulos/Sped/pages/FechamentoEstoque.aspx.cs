using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using visualSysWeb.dao;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Sped.pages
{
    public partial class FechamentoEstoque : visualSysWeb.code.PagePadrao
    {

        User usr;

        protected void Page_Load(object sender, EventArgs e)
        {
             usr = (User)Session["User"];
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

        protected void btnFecharMes_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dataFechamento = DateTime.Parse(txtDataFechamento.Text);
                if (dataFechamento.Month >= DateTime.Today.Month)
                {
                    lblErro.Text = "Data de fechamento não pode conter o mês atual.";
                    return;
                }
                else if (dataFechamento <= usr.filial.dtFechamentoEstoque)
                {
                    lblErro.Text = "Data solicitada já consta como fechada no sistema.";
                    return;
                }

                string sql = "sp_Ins_EstoqueNaData '" + usr.filial.Filial + "', '" + dataFechamento.ToString("yyyyMMdd") + "'";
                Conexao.executarSqlCmd(sql);

                txtDataFechamento.Enabled = false;
                btnFecharMes.Enabled = false;
                clnDataFechamento.Enabled = false;
                lblErro.Text = "Fechamento Executado com sucesso.";
                lblErro.ForeColor = System.Drawing.Color.Blue;
            }
            catch (Exception ex)
            {
                lblErro.ForeColor = System.Drawing.Color.Red;
                lblErro.Text = ex.Message;
            }


        }
    }
}