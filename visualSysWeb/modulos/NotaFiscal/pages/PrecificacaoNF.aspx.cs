using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using System.Data.SqlClient;
using visualSysWeb.code;

namespace visualSysWeb.modulos.NotaFiscal.pages
{
    public partial class PrecificacaoNF : visualSysWeb.code.PagePadrao
    {
        static DataTable tb;
        static String sqlGrid = "select ltrim(rtrim(nf.Codigo)) as Codigo, nf.cliente_fornecedor,fornecedor=replace(nf.cliente_fornecedor,'&','|E')  ,convert(varchar,nf.data,103) as Data,convert(varchar,nf.emissao,103) as Emissao,nf.total, Case When isNull(Nf.Estado, 0) = 0 THEN 'PENDENTE' ELSE 'PRECIFICADA' END As Estado, " +
            "ISNULL(NF.Serie, 0) As Serie FROM NF  INNER JOIN Natureza_Operacao N ON NF.Codigo_Operacao = N.Codigo_Operacao WHERE tipo_nf=2 and nf_canc<>1 AND ISNULL(N.Precificacao, 0) = 1 ";//colocar os campos no select que ser?o apresentados na tela
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                pesquisar();
            }
            sopesquisa(pnBtn);

        }

        private void camposnumericos()
        {
            txtCodigo.Attributes.Add("OnKeyPress", "javascript:return numero(this,event);");

        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/NotaFiscal/pages/PrecificacaoNFDetalhes.aspx?novo=true");
        }


        protected void pesquisar()
        {
            try
            {
                User usr = (User)Session["User"];

                bool PrecificacaoDesativada = (Funcoes.valorParametro("PRECIFIC_NF_OFF", usr).Equals("TRUE") ? true : false);
                if (PrecificacaoDesativada)
                {
                    txtAviso.Text = "Precificação de NFe INDISPONÍVEL.";
                    txtAviso.Visible = true;
                    filtrosPesq.Visible = false;
                    return;
                }
                if (!PrecificacaoDesativada)
                {
                    txtAviso.Visible = false;
                    String sql = " and isNull(Nf.Estado, 0)  =" + ddlEstado.SelectedValue + " and nf.filial ='" + usr.getFilial() + "' ";
                    if (!txtCodigo.Text.Equals(""))
                    {
                        sql += " and NF.Codigo like '" + txtCodigo.Text + "%'";
                    }
                    if (!txtSerie.Text.Equals(""))
                    {
                        sql += " and NF.Serie  = " + txtSerie.Text;
                    }
                    if (!txtFornecedor.Text.Equals(""))
                    {

                        sql += " and NF.cliente_Fornecedor Like '" + txtFornecedor.Text + "%'";
                    }
                    if (!DllTipoPesquisa.SelectedValue.Equals(""))
                    {
                        if (!txtDe.Text.Equals(""))
                        {
                            sql += " and " + DllTipoPesquisa.SelectedValue + " between '" + DateTime.Parse(txtDe.Text).ToString("yyyy-MM-dd") + "' and '" + DateTime.Parse(txtAte.Text).ToString("yyyy-MM-dd") + "'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
                        }
                    }

                    tb = Conexao.GetTable(sqlGrid + sql, null, false);

                    gridPesquisa.DataSource = tb;
                    gridPesquisa.DataBind();
                    lblPesquisaErro.Text = "";


                }
                else
                {
                    filtrosPesq.Visible = false;

                }
            }
            catch (FormatException)
            {
                lblPesquisaErro.Text = "Digite uma Data Valida";
            }

            catch (Exception err)
            {
                lblPesquisaErro.Text = err.Message;
            }
        }
        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisar();
        }
        protected override void btnEditar_Click(object sender, EventArgs e) { }
        protected override void btnExcluir_Click(object sender, EventArgs e) { }
        protected override void btnConfirmar_Click(object sender, EventArgs e) { }
        protected override void btnCancelar_Click(object sender, EventArgs e) { }


        protected void gridPesquisa_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridPesquisa.DataSource = tb;
            gridPesquisa.PageIndex = e.NewPageIndex;
            gridPesquisa.DataBind();
        }
        protected override bool campoObrigatorio(Control campo)
        {
            return false;
        }

        protected override bool campoDesabilitado(Control campo)
        {
            return false;//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
        }

        protected void imgFornecedor_Click(object sender, ImageClickEventArgs e)
        {
            TxtPesquisaLista.Text = "";
            carregaLista();
            
        }

        protected void carregaLista()
        {
            lblErroPesquisa.Text = "";

            lbllista.Text = "ESCOLHA UM FORNECEDOR";
            String sqlLista = "select fornecedor, Razao_social from fornecedor ";
            if (!TxtPesquisaLista.Text.Equals(""))
            {
                sqlLista += " where (fornecedor like '%" + TxtPesquisaLista.Text + "%') or razao_social like '%"+TxtPesquisaLista.Text+"%'";
            }
            User usr = (User)Session["User"];
            GridLista.DataSource = Conexao.GetTable(sqlLista, usr, false);
            GridLista.DataBind();
            if (GridLista.Rows.Count == 1)
            {
                if (!GridLista.Rows[0].Cells[1].Text.Equals("------"))
                {
                    RadioButton rdo = (RadioButton)GridLista.Rows[0].FindControl("RdoListaItem");
                    rdo.Checked = true;
                }
            }

            TxtPesquisaLista.Focus();
            modalFornecedor.Show();
        }

        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {
            String selecionado = ListaSelecionada(1);

            if (!selecionado.Equals("") && !selecionado.Equals("------"))
            {
                txtFornecedor.Text = ListaSelecionada(1);
            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                modalFornecedor.Show();
            }

        }

        protected void GridLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoListaItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('GridLista.*GrlistaItem',this)";
            rdo.Attributes.Add("onclick", script);
        }

        protected void GridLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void btnCancelaLista_Click(object sender, EventArgs e)
        {
            modalFornecedor.Hide();
        }

        protected void ImgPesquisaLista_Click(object sender, EventArgs e)
        {
            carregaLista();
        }

        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            pesquisar();
        }
        protected void txtSerie_TextChanged(object sender, EventArgs e)
        {
            pesquisar();
        }
        protected String ListaSelecionada(int index)
        {
            foreach (GridViewRow item in GridLista.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoListaItem");

                if (rdo != null)
                {
                    if (rdo.Checked)
                    {
                        return item.Cells[index].Text;
                    }
                }
            }

            return "";
        }

    }
}