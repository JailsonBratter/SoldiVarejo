using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using System.Data.SqlClient;

namespace visualSysWeb.modulos.NotaFiscal.pages
{
    public partial class NFEntradaDoca : visualSysWeb.code.PagePadrao
    {
        static DataTable tb;
        static String ultimaOrdem = "";
        static String sqlGrid = "SELECT CONVERT(VARCHAR, MN.Emissao, 103) AS Emissao, SUBSTRING(MN.CHAVE, 26, 9) AS NroNFe, mn.CNPJ , mn.RazaoSocial  as Fornecedor,"
        + "MN.Vnf AS total, ISNULL(nfr.Usuario, '') AS Usuario, [Status] = CASE WHEN (SELECT COUNT(*) FROM NF WHERE NF.ID = MN.CHAVE) > 0 AND ISNULL(nfr.status, 0) = 0 THEN 'ENT.MANUAL'"
        + "WHEN ISNULL(NFR.Status, 0) = 1 THEN 'PENDENTE LCTO' WHEN ISNULL(NFR.Status, 0) = 2 THEN 'LANÇADA' ELSE 'AGUARDANDO' END,"
        + "Chave = mn.chave, StatusManifesto = mn.status, XMLDisponivel = CASE WHEN ISNULL(MN.nfeXML,'') <> '' THEN 'SIM' ELSE 'NÃO' END"
        + ", EntManual = CASE WHEN (SELECT COUNT(*) FROM NF WHERE NF.ID = MN.CHAVE) > 0 THEN 'SIM' ELSE 'NAO' END"
        +" FROM Nf_manifestar MN LEFT OUTER JOIN nf_recebimento nfr on mn.chave = nfr.ID WHERE mn.chave is not null";

        static String sqlGridOrigem = "SELECT CONVERT(VARCHAR, MN.Emissao, 103) AS Emissao, SUBSTRING(MN.CHAVE, 26, 9) AS NroNFe, mn.CNPJ , mn.RazaoSocial  as Fornecedor,"
        + "MN.Vnf AS total, ISNULL(nfr.Usuario, '') AS Usuario, [Status] = CASE WHEN (SELECT COUNT(*) FROM NF WHERE NF.ID = MN.CHAVE) > 0 AND ISNULL(nfr.status, 0) = 0 THEN 'ENT.MANUAL'"
        + "WHEN ISNULL(NFR.Status, 0) = 1 THEN 'PENDENTE LCTO' WHEN ISNULL(NFR.Status, 0) = 2 THEN 'LANÇADA' ELSE 'AGUARDANDO' END,"
        + "Chave = mn.chave, StatusManifesto = mn.status, XMLDisponivel = CASE WHEN ISNULL(MN.nfeXML,'') <> '' THEN 'SIM' ELSE 'NÃO' END"
        + ", EntManual = CASE WHEN (SELECT COUNT(*) FROM NF WHERE NF.ID = MN.CHAVE) > 0 THEN 'SIM' ELSE 'NAO' END"
        + " FROM Nf_manifestar MN LEFT OUTER JOIN nf_recebimento nfr on mn.chave = nfr.ID WHERE mn.chave is not null";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pesquisar("mn.Emissao");
            }
            pesquisar(pnBtn);
            Formatarcampos();

        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?novo=true");
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
                String strSqltotalFiltro = "select count(*) from NF_Manifestar MN LEFT OUTER JOIN NF_Recebimento NFR ON MN.Chave = NFr.ID WHERE mn.Filial='" + usr.getFilial() + "'";
                String totalRegistro = Conexao.retornaUmValor(strSqltotalFiltro, null);

                if (ordem.Equals(""))
                {
                    strOrdem = " order by convert(varchar, mn.Emissao,102) DESC";
                }
                else
                {
                    if (ordem.Equals("NroNFe"))
                    {
                        strOrdem = " order by convert(int,Codigo) ";
                    }
                    else if (ordem.Equals("Emissao"))
                    {
                        strOrdem = " order by convert(varchar,mn.emissao,102) ";
                    }
                    else
                    {
                        strOrdem = " order By " + ordem;
                    }
                }

                if (ordem.Equals(ultimaOrdem))
                {
                    //strOrdem += " Desc ";
                    ultimaOrdem = "";
                }
                else
                {
                    ultimaOrdem = ordem;
                }

                String sql = "";
                if (!txtCodigo.Text.Equals(""))
                {
                    sql = " and convert(int, substring(mn.Chave, 26, 9)) like '" + txtCodigo.Text + "%'";
                }
                if (!txtFornecedor.Text.Equals(""))
                {

                    sql += " and mn.cnpj Like '" + txtFornecedor.Text + "%'";
                }

                if (!DllTipoPesquisa.SelectedValue.Equals(""))
                {
                    if (DllTipoPesquisa.SelectedValue.ToString().Equals("0"))
                    {
                        sqlGrid = "SELECT CONVERT(VARCHAR, MN.Emissao, 103) AS Emissao, SUBSTRING(MN.CHAVE, 26, 9) AS NroNFe, mn.CNPJ , mn.RazaoSocial  as Fornecedor,"
                                + "MN.Vnf AS total, '' AS Usuario, [Status] =  'AGUARDANDO', "
                                + " Chave = mn.chave, StatusManifesto = mn.status, XMLDisponivel = CASE WHEN ISNULL(MN.nfeXML,'') <> '' THEN 'SIM' ELSE 'NÃO' END"
                                + ", EntManual = 'NAO'"
                                + " FROM Nf_manifestar MN WHERE mn.chave is not null AND (SELECT COUNT(*) FROM NF WHERE NF.ID = MN.CHAVE) = 0 ";
                    }
                    else if (DllTipoPesquisa.SelectedValue.ToString().Equals("1"))
                    {
                        sqlGrid = sqlGridOrigem;
                        sqlGrid = sqlGrid.Replace(" MN LEFT OUTER JOIN nf_re", " MN INNER JOIN nf_re");
                        sql += " AND ISNULL(nfr.Status, 0) = 1";
                    }
                    else if (DllTipoPesquisa.SelectedValue.ToString().Equals("2"))
                    {
                        sqlGrid = sqlGridOrigem;
                        sql += " AND MN.chave in (SELECT NF.ID FROM NF WHERE NF.EMISSAO >= MN.EMISSAO)";
                    }
                }

                if (!txtDe.Text.Equals("") && !txtAte.Text.Equals(""))
                {
                    sql += " AND mN.Emissao BETWEEN '" + DateTime.Parse(txtDe.Text).ToString("yyyy-MM-dd") + "' AND '" + DateTime.Parse(txtAte.Text).ToString("yyyy-MM-dd") + "'";
                }

                if (!sql.Equals(""))
                {
                    if (sqlGrid.IndexOf("WHERE") < 0)
                    {
                        sqlGrid += " WHERE MN.CHAVE IS NOT NULL ";
                    }

                    tb = Conexao.GetTable(sqlGrid + sql + strOrdem, usr, false);
                    strSqltotalFiltro += sql;
                }
                else
                {
                    tb = Conexao.GetTable(sqlGrid + strOrdem, usr, true);
                }

                String totalFiltro = Conexao.retornaUmValor(strSqltotalFiltro, usr);
                lblRegistros.Text = totalFiltro + " Registros de  " + totalRegistro + " Cadastrados ";

                gridPesquisa.DataSource = tb;
                gridPesquisa.DataBind();
                lblPesquisaErro.Text = "";

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
        //Sinto Muito Me Perdoe Agradeço Eu Te Amo.


        protected override bool campoObrigatorio(Control campo)
        {
            return false;
        }

        protected override bool campoDesabilitado(Control campo)
        {
            return false;
        }

        protected void imgFornecedor_Click(object sender, ImageClickEventArgs e)
        {

            carregaLista();



        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalFornecedor.Hide();
        }

        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
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

        protected void gridPesquisa_Sorting(object sender, GridViewSortEventArgs e)
        {
            pesquisar(e.SortExpression);
        }
        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            pesquisar(ultimaOrdem);
        }

        protected void gridPesquisa_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Entrada")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string chave = gridPesquisa.Rows[index].Cells[8].Text;
                if (chave.Trim().Length == 44)
                {
                    RedirectNovaAba("~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?novo=true&chave=" + chave + "&doca=true");
                }
            }
        }

        protected void gridPesquisa_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            User usr = (User)Session["User"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string statusNFeDoca = e.Row.Cells[6].Text;
                string entradaManual = e.Row.Cells[11].Text;
                //ddlCartao.Text = lblCartao.Text;

                if (statusNFeDoca.IndexOf("PENDENTE") < 0 || entradaManual.Equals("SIM"))
                {
                    ImageButton btn = (ImageButton)e.Row.Cells[7].Controls[0];
                    btn.Visible = false;
                }
            }

        }
    }
}