using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.NotaFiscal.pages
{
    public partial class CTeEntrada : visualSysWeb.code.PagePadrao
    {
        static DataTable tb;
        static String ultimaOrdem = "";
        static String sqlGrid = "SELECT * FROM NF_CTe";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pesquisar("");
            }
            status = "visualizar";
            sopesquisa(pnBtn);
            Formatarcampos();

        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/NotaFiscal/pages/CTeEntradaDetalhes.aspx?novo=true");
        }


        private void Formatarcampos()
        {
            txtCodigo.Attributes.Add("OnKeyPress", "javascript:return numeros(this,event);");
            txtDe.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
            txtAte.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
        }

        protected void pesquisar(String ordem)
        {
            try
            {

                User usr = (User)Session["User"];
                String strOrdem = "";
                String strSqltotalFiltro = "select count(*) from NF_CTe  WHERE  filial='" + usr.getFilial() + "'";
                String totalRegistro = Conexao.retornaUmValor(strSqltotalFiltro, null);

                if (ordem.Equals(""))
                {
                    strOrdem = " order by convert(varchar,aquisicao,102) ";
                }
                else
                {
                    if (ordem.Equals("numero_documento"))
                    {
                        strOrdem = " order by convert(int,Codigo) ";
                    }
                    else if (ordem.Equals("aquisicao"))
                    {
                        strOrdem = " order by convert(varchar, aquisicao,102) ";
                    }
                    else if (ordem.Equals("Emissao"))
                    {
                        strOrdem = " order by convert(varchar,emissao,102) ";
                    }
                    else
                    {
                        strOrdem = " order By " + ordem;
                    }
                }

                if (ordem.Equals(ultimaOrdem))
                {
                    strOrdem += " Desc ";
                    ultimaOrdem = "";
                }
                else
                {
                    ultimaOrdem = ordem;
                }

                String sql = "";
                if (!txtCodigo.Text.Equals(""))
                {
                    sql = " and numero_documento like '" + txtCodigo.Text + "%'";
                }
                if (!txtFornecedor.Text.Equals(""))
                {

                    sql += " and cliente_Fornecedor Like '" + txtFornecedor.Text + "%'";
                }
                if (!DllTipoPesquisa.SelectedValue.Equals(""))
                {
                    if (!txtDe.Text.Equals(""))
                    {

                        sql += " and " + DllTipoPesquisa.SelectedValue + " between '" + DateTime.Parse(txtDe.Text).ToString("yyyy-MM-dd") + "' and '" + DateTime.Parse(txtAte.Text).ToString("yyyy-MM-dd") + "'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
                    }
                }



                if (!sql.Equals(""))
                {
                    tb = Conexao.GetTable(sqlGrid + sql + strOrdem, usr, false);
                    strSqltotalFiltro += sql;
                }
                else
                {
                    tb = Conexao.GetTable(sqlGrid + strOrdem, usr, true);
                }

                String totalFiltro = Conexao.retornaUmValor(strSqltotalFiltro, usr);
                lblRegistros.Text = totalFiltro + " Registros de  " + totalRegistro + " Cadastrados ";

                try
                {
                    gridPesquisa.DataSource = tb;
                    gridPesquisa.DataBind();
                    lblPesquisaErro.Text = "";
                }
                catch (Exception e)
                {

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
            pesquisar("");
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

        protected void gridPesquisa_Sorting(object sender, GridViewSortEventArgs e)
        {
            pesquisar(e.SortExpression);
        }

        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            carregaLista();
        }


        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            pesquisar(ultimaOrdem);
        }
        protected void imgFornecedor_Click(object sender, ImageClickEventArgs e)
        {
            carregaLista();
        }
        protected void carregaLista()
        {
            lblPesquisaErro.Text = "";

            String sqlLista = "";
            lbllista.Text = "ESCOLHA UM FORNECEDOR";
            sqlLista = "select Fornecedor, Razao_Social , CNPJ from fornecedor where CNPJ like '%" + TxtPesquisaLista.Text + "%' or fornecedor like '%" + TxtPesquisaLista.Text + "%' or razao_social like '%" + TxtPesquisaLista.Text + "%' group by Fornecedor, Razao_Social , CNPJ ";
            User usr = (User)Session["User"];

            GridLista.DataSource = Conexao.GetTable(sqlLista, usr, true);
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

        protected void btnFechar_Click(object sender, ImageClickEventArgs e)
        {
            String selecionado = ListaSelecionada();

            if (!selecionado.Equals("") && !selecionado.Equals("------"))
            {
                txtFornecedor.Text = ListaSelecionada();
            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                modalFornecedor.Show();
            }

        }

        protected void TxtPesquisaLista_TextChanged(object sender, EventArgs e)
        {
            carregaLista();
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

        protected String ListaSelecionada()
        {
            foreach (GridViewRow item in GridLista.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoListaItem");

                if (rdo != null)
                {
                    if (rdo.Checked)
                    {
                        return item.Cells[1].Text;
                    }
                }
            }

            return "";
        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalFornecedor.Hide();
        }
    }
}