using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Sped.pages
{
    public partial class ContasContabeisDetalhes : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            bool novo = (Request.Params["Novo"] != null);
            if (novo)
            {
                if (!IsPostBack)
                {
                    status = "incluir";
                    ContaContabilDAO conta = new ContaContabilDAO(usr);
                    conta.data = DateTime.Now;
                    conta.cnpj_estabelecimento = usr.filial.CNPJ;
                   
                    Session.Remove("ContaContabil" + urlSessao());
                    Session.Add("ContaContabil" + urlSessao(), conta);
                    carregarDados();
                    habilitarCampos(true);

                }
            }
            else
            {
                if (!IsPostBack)
                {
                    status = "visualizar";
                    String codigo = Request.Params["codigo"];
                    String cnpj = Request.Params["cnpj"];
                    if (codigo != null && cnpj != null)
                    {
                        ContaContabilDAO conta = new ContaContabilDAO(cnpj, codigo, usr);

                        Session.Remove("ContaContabil" + urlSessao());
                        Session.Add("ContaContabil" + urlSessao(), conta);
                        carregarDados();
                        habilitarCampos(false);
                    }
                }

            }

            carregabtn(pnBtn);

        }

        protected void habilitarCampos(bool enable)
        {
            EnabledControls(conteudo, enable);
        }


        protected void carregarDados()
        {
            ContaContabilDAO conta = (ContaContabilDAO)Session["ContaContabil" + urlSessao()];

            txtCod_conta.Text = conta.cod_conta;
            txtData.Text = conta.data.ToString("dd/MM/yyyy");
            txtDescricao.Text = conta.descricao;
            txtCNPJ.Text = conta.cnpj_estabelecimento;
            txtNivel.Text = conta.nivel;
            ddlTipo.SelectedValue = conta.tipo;
            ddlNatureza.SelectedValue = conta.natureza;
            ddlEntradaSaida.SelectedValue = (conta.entrada ? "1" : "0");
            txtContaRelacionada.Text = conta.conta_relacionada;
        }
        protected void carregarDadosObj()
        {
            ContaContabilDAO conta = (ContaContabilDAO)Session["ContaContabil" + urlSessao()];

            conta.cod_conta = txtCod_conta.Text;
            conta.data = Funcoes.dtTry(txtData.Text);
            conta.descricao = txtDescricao.Text;
            conta.cnpj_estabelecimento = txtCNPJ.Text;
            conta.nivel = txtNivel.Text;
            conta.tipo = ddlTipo.SelectedValue;
            conta.natureza = ddlNatureza.SelectedValue;
            conta.entrada = ddlEntradaSaida.SelectedValue.Equals("1");
            conta.conta_relacionada = txtContaRelacionada.Text;

            Session.Remove("ContaContabil" + urlSessao());
            Session.Add("ContaContabil" + urlSessao(), conta);
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("ContasContabeisDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            status = "editar";
            carregarDados();
            habilitarCampos(true);
            editar(pnBtn);
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ContasContabeis.aspx");
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            modalExcluir.Show();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                validaCampos(conteudo);
                carregarDadosObj();
                ContaContabilDAO conta = (ContaContabilDAO)Session["ContaContabil" + urlSessao()];
                conta.salvar(status.Equals("incluir"));
                status = "visualizar";
                habilitarCampos(false);
                visualizar(pnBtn);
                showMsg("Salvo Com Sucesso", false);
            }
            catch (Exception err)
            {

                showMsg(err.Message, true);
            }
            
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ContasContabeis.aspx");
        }

        protected override bool campoDesabilitado(Control campo)
        {

            if (status.Equals("editar"))
            {
                switch (campo.ID)
                {
                    case "txtCod_conta":
                    case "txtCNPJ":
                        return true;
              
                }
              
                
            }
            switch (campo.ID)
            {
                case "txtData":
                    return true;
            }
            return false;
        }

        protected override bool campoObrigatorio(Control campo)
        {
            switch (campo.ID)
            {
                case "txtCod_conta":
                case "txtCNPJ":
                case "txtDescricao":
                    return true;
                default:
                    return false;

            }

        }

      


        protected void showMsg(String msg,bool erro)
        {
            lblMsgPanel.Text = msg;
            lblMsgPanel.ForeColor = (erro ? System.Drawing.Color.Red : System.Drawing.Color.Blue);
            modalMsg.Show();
        }

        protected void btnOkError_Click(Object sender, EventArgs e)
        {
            modalMsg.Hide();
        }
         
        protected void btnConfirmaExcluir_Click(object sender, EventArgs e)
        {
            ContaContabilDAO conta = (ContaContabilDAO)Session["ContaContabil" + urlSessao()];
            try
            {
                conta.excluir();
                Session.Remove("ContaContabil" + urlSessao());
                LimparCampos(conteudo);
                showMsg("Excluido com Sucesso!", false);
                pesquisar(pnBtn);
            }
            catch (Exception err)
            {

                showMsg(err.Message, true);
            }

            modalExcluir.Hide();
        }
        protected void btnCancelarExcluir_Click(object sender,EventArgs e)
        {
            modalExcluir.Hide();
        }
    }
}