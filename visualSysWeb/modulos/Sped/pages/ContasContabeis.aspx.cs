using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using visualSysWeb.modulos.Sped.code;

namespace visualSysWeb.modulos.Sped.pages
{
    public partial class ContasContabeis : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                PlanoContasFiltros filtro = (PlanoContasFiltros)Session["filtroContasContabeis"];

                if(filtro !=null)
                {
                    txtDataDe.Text = filtro.dtDe;
                    txtDataAte.Text = filtro.dtAte;
                    txtCodigo.Text = filtro.cod_conta;
                    txtDescricao.Text = filtro.descricao;
                    txtCnpj.Text = filtro.cnpj_Estabelecimento;
                    ddlTipo.SelectedValue = filtro.tipo;
                    ddlNatureza.SelectedValue = filtro.natureza;
                    ddlEntradaSaida.SelectedValue = filtro.entrada;

                }
               


                pesquisar();

            }
            pesquisar(pnBtn);

        }

        protected void pesquisar()
        {

            User usr = (User)Session["User"];
            if (usr != null)
            {
                PlanoContasFiltros filtro = new PlanoContasFiltros();
                filtro.dtDe = txtDataDe.Text;
                filtro.dtAte = txtDataAte.Text;
                filtro.cod_conta = txtCodigo.Text;
                filtro.descricao = txtDescricao.Text;
                filtro.tipo = ddlTipo.SelectedItem.Value;
                filtro.natureza = ddlNatureza.SelectedItem.Value;
                filtro.cnpj_Estabelecimento = txtCnpj.Text;
                filtro.entrada = ddlEntradaSaida.SelectedItem.Value;

                Session.Remove("filtroContasContabeis");
                Session.Add("filtroContasContabeis", filtro);

               
                gridPesquisa.DataSource = filtro.pesquisa(usr);
                gridPesquisa.DataBind();

                lblQtdRegistros.Text = filtro.qtdFiltros+ " de "+ filtro.qtdeCadastro + " Cadastrados";
            }
        }
        protected void txtDataDe_TextChanged(object sender, EventArgs e)
        {
            if (IsDate(txtDataDe.Text))
            {
                txtDataAte.Text = txtDataDe.Text;
            }
        }
        protected void gridPesquisa_Sorting(object sender, GridViewSortEventArgs e)
        {

            //pesquisar(e.SortExpression);

        }
       

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("ContasContabeisDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisar();
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

        protected void imgBtnLimpar_Click(object sender, EventArgs e)
        {
            Session.Remove("filtroContasContabeis");
            Response.Redirect("ContasContabeis.aspx");


        }
    }
}