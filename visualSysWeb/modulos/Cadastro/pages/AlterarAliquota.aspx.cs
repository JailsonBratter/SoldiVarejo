using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class AlterarAliquota : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Conexao.preencherDDL1Branco(ddlTributacao, "Select  Codigo_Tributacao, Descricao_Tributacao from Tributacao ", "Descricao_tributacao", "codigo_tributacao", null);
                pesquisa(true);
            }
            sopesquisa(pnBtn);
        }
        protected void pesquisa(bool limitar)
        {
            String strWhere = "";
            if (!txtPlu.Text.Equals(""))
            {
                strWhere = " m.plu like '" + txtPlu.Text + "%'";
            }
            if (!txtDescricao.Text.Equals(""))
            {
                if (strWhere.Length > 0)
                {
                    strWhere += " and ";
                }
                strWhere += " m.descricao like '%" + txtDescricao.Text + "%'";
            }
            if (!txtNcmDe.Text.Equals(""))
            {
                if (strWhere.Length > 0)
                {
                    strWhere += " and ";
                }
                if (!txtNcmAte.Text.Equals(""))
                {
                    txtNcmDe.Text = txtNcmDe.Text.PadRight(8, '0');
                    txtNcmAte.Text = txtNcmAte.Text.PadRight(8, '9');
                    strWhere += " (m.cf between '" + txtNcmDe.Text + "' and '" + txtNcmAte.Text + "')";
                }
                else
                {
                    strWhere += " m.cf like '" + txtNcmDe.Text + "%'";
                }
            }
            if (!txtGrupo.Text.Equals(""))
            {
                if (strWhere.Length > 0)
                {
                    strWhere += " and ";
                }
                strWhere += " d.descricao_grupo ='" + txtGrupo.Text + "'";
            }
            if (!txtSubGrupo.Text.Equals(""))
            {
                if (strWhere.Length > 0)
                {
                    strWhere += " and ";
                }
                strWhere += " d.descricao_subgrupo ='" + txtSubGrupo.Text + "'";
            }
            if (!txtDepartamento.Text.Equals(""))
            {
                if (strWhere.Length > 0)
                {
                    strWhere += " and ";
                }
                strWhere += " d.descricao_Departamento ='" + txtDepartamento.Text + "'";
            }

            if (!txtFamilia.Text.Equals(""))
            {
                if (strWhere.Length > 0)
                {
                    strWhere += " and ";
                }
                strWhere += " f.descricao_Familia ='" + txtFamilia.Text + "'";
            }


            String sql = "Select	PLU," +
                           " Descricao," +
                           " cf as ncm, " +
                           " t.Descricao_Tributacao as tributacao, " +
                           " m.CEST," +
                           " m.cst_saida as [CST Pis/Cofins]," +
                           " m.pis_perc_saida as [Pis%], " +
                           " m.cofins_perc_saida as [Cofins%]," +
                           " m.IPI, " +
                           " d.Descricao_grupo as Grupo," +
                           " d.descricao_subgrupo as SubGrupo," +
                           " d.descricao_departamento as Departamento," +
                           " f.Descricao_familia," +
                           " m.margem_iva as iva,"+
                           " m.origem"+

                   " from mercadoria as m " +
                   " left join Tributacao as t on m.Codigo_Tributacao = t.Codigo_Tributacao " +
                   " inner join W_BR_CADASTRO_DEPARTAMENTO as d on m.Codigo_departamento = d.codigo_departamento " +
                   " left join Familia as f on m.Codigo_familia =f.Codigo_familia ";
            if (strWhere.Length > 0)
            {
                sql += " Where " + strWhere;
            }

            sql += " order by Grupo , SubGrupo, Departamento";
            User usr = (User)Session["User"];
            if (strWhere.Length > 0)
            {
                limitar = false;
            }

            GridItens.DataSource = Conexao.GetTable(sql, usr, limitar);
            GridItens.DataBind();
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
            pesquisa(false);
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

        protected void exibeLista()
        {
            lblErroPesquisa.Text = "";
            User usr = (User)Session["User"];
            String or = (String)Session["camporecebe" + urlSessao()];
            String sqlLista = "";


            switch (or)
            {
                case "Grupo":
                    sqlLista = " Select descricao_grupo from grupo  where descricao_grupo like '%" + TxtPesquisaLista.Text + "%'  ";
                    lbllista.Text = "Grupo";
                    break;
                case "SubGrupo":
                    sqlLista = " Select descricao_subGrupo,descricao_grupo from W_BR_CADASTRO_DEPARTAMENTO where descricao_subGrupo  like '%" + TxtPesquisaLista.Text + "%'" + (txtGrupo.Text.Equals("") ? "" : " and descricao_grupo ='" + txtGrupo.Text + "'") + " group by descricao_subGrupo,descricao_grupo;";
                    lbllista.Text = "Sub Grupo";
                    break;
                case "Departamento":
                    sqlLista = "select descricao_departamento, descricao_subGrupo,descricao_grupo  from W_BR_CADASTRO_DEPARTAMENTO where descricao_departamento like '%" + TxtPesquisaLista.Text + "%' " + (txtSubGrupo.Text.Trim().Equals("") ? "" : " and descricao_subgrupo = '" + txtSubGrupo.Text + "'") + "group by descricao_departamento, descricao_subGrupo,descricao_grupo"; ;
                    lbllista.Text = "Departamento";
                    break;
                case "Familia":
                    sqlLista = "Select descricao_familia from familia  where descricao_familia like '%" + TxtPesquisaLista.Text + "%'";
                    lbllista.Text = "Familia";
                    break;
                case "cest":
                    sqlLista = "EXEC SP_CEST_NCM '" + txtNcm.Text.Trim() + "'";
                    lbllista.Text = "Escolha uma CEST";
                    break;
                case "ncm":
                    lbllista.Text = "Escolha a Classificação Fiscal (NCM)";
                    sqlLista = "select  cf as NCM,descricao from cf where cf like '%" + TxtPesquisaLista.Text + "%' or descricao like '%" + TxtPesquisaLista.Text + "%'";
                    break;
                case "cst":
                    lbllista.Text = "Escolha o CST";
                    sqlLista = "Select CST=pis_cst_Saida   , Descricao from pis_cst_Saida where descricao like '%" + TxtPesquisaLista.Text + "%' or pis_cst_Saida like '%" + TxtPesquisaLista.Text + "%'";
                    break;


            }
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

            modalPnFundo.Show();
        }
        protected String ListaSelecionada(int campo)
        {
            foreach (GridViewRow item in GridLista.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoListaItem");

                if (rdo != null)
                {
                    if (rdo.Checked)
                    {
                        return item.Cells[campo].Text;
                    }
                }
            }

            return "";
        }
        protected void btnFechar_Click(object sender, ImageClickEventArgs e)
        {
            String selecionado = ListaSelecionada(1);

            if (!selecionado.Equals("") && !selecionado.Equals("------"))
            {

                String listaAtual = (String)Session["camporecebe" + urlSessao()];
                Session.Remove("camporecebe");

                if (listaAtual.Equals("Grupo"))
                {
                    txtGrupo.Text = ListaSelecionada(1);
                    txtSubGrupo.Text = "";
                    txtDepartamento.Text = "";

                }
                else if (listaAtual.Equals("SubGrupo"))
                {
                    txtSubGrupo.Text = ListaSelecionada(1);
                    txtGrupo.Text = ListaSelecionada(2);
                    txtDepartamento.Text = "";
                }
                else if (listaAtual.Equals("Departamento"))
                {
                    txtDepartamento.Text = ListaSelecionada(1);
                    txtSubGrupo.Text = ListaSelecionada(2);
                    txtGrupo.Text = ListaSelecionada(3);

                }
                else if (listaAtual.Equals("Familia"))
                {
                    txtFamilia.Text = ListaSelecionada(1);
                }
                else if (listaAtual.Equals("ncm"))
                {
                    txtNcm.Text = ListaSelecionada(1);
                }
                else if (listaAtual.Equals("cest"))
                {
                    txtCest.Text = ListaSelecionada(1);
                }
                else if (listaAtual.Equals("cst"))
                {
                    txtCstPisConfins.Text = ListaSelecionada(1);
                }
                modalPnFundo.Hide();
            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                modalPnFundo.Show();
            }
        }
        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalPnFundo.Hide();
        }

        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }

        protected void GridLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoListaItem");

            if (rdo == null)
            {
                return;//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            }
            string script = "SetUniqueRadioButton('GridLista.*GrlistaItem',this)";
            rdo.Attributes.Add("onclick", script);
        }

        protected void Img_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            TxtPesquisaLista.Text = "";
            String campo = "";
            switch (btn.ID)
            {
                case "imgBtnGrupo":
                    campo = "Grupo";
                    break;
                case "imgBtnSubGrupo":
                    campo = "SubGrupo";
                    break;
                case "imgBtnDepartamento":
                    campo = "Departamento";
                    break;
                case "imgBtnFamilia":
                    campo = "Familia";
                    break;
                case "imgBtnNcm":
                    campo = "ncm";
                    break;
                case "imgBtnCest":
                    campo = "cest";
                    break;
                case "imgBtnCstPisConfins":
                    campo = "cst";
                    break;

            }
            Session.Add("camporecebe" + urlSessao(), campo);

            exibeLista();


        }

        protected void chkSeleciona_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTodos = (CheckBox)sender;
            foreach (GridViewRow item in GridItens.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");

                if (chk != null)
                {
                    chk.Checked = chkTodos.Checked;

                }
            }


        }

        protected void btnConfirmaEncerrar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {


                String strSql = "";
                if (!ddlTributacao.Text.Equals(""))
                {
                    strSql = " codigo_tributacao='" + ddlTributacao.SelectedValue + "'";
                }
                if (!txtNcm.Text.Trim().Equals(""))
                {
                    if (strSql.Length > 0)
                    {
                        strSql += ",";
                    }
                    strSql += " cf='" + txtNcm.Text.Trim() + "'";
                }
                if (!txtCstPisConfins.Text.Trim().Equals(""))
                {
                    if (strSql.Length > 0)
                    {
                        strSql += ",";
                    }
                    strSql += " cst_saida='" + txtCstPisConfins.Text.Trim() + "'";
                }
                if (!txtPis.Text.Trim().Equals(""))
                {
                    if (strSql.Length > 0)
                    {
                        strSql += ",";
                    }
                    strSql += " pis_perc_saida=" + Funcoes.decimalPonto(txtCstPisConfins.Text.Trim());
                }
                if (!txtCofins.Text.Trim().Equals(""))
                {
                    if (strSql.Length > 0)
                    {
                        strSql += ",";
                    }
                    strSql += " cofins_perc_saida=" + Funcoes.decimalPonto(txtCofins.Text.Trim());
                }
                if (!txtCest.Text.Trim().Equals(""))
                {
                    if (strSql.Length > 0)
                    {
                        strSql += ",";
                    }
                    strSql += " CEST='" + txtCest.Text.Trim() + "'";
                }
                if (!txtIpi.Text.Trim().Equals(""))
                {
                    if (strSql.Length > 0)
                    {
                        strSql += ",";
                    }
                    strSql += " ipi=" + Funcoes.decimalPonto(txtCofins.Text.Trim());
                }
                if (!txtIva.Text.Equals(""))
                {
                    if (strSql.Length > 0)
                    {
                        strSql += ",";
                    }
                    strSql += " margem_iva=" + Funcoes.decimalPonto(txtIva.Text.Trim());
                }
                if (!ddlOrigem.SelectedValue.Equals(""))
                {
                    if (strSql.Length > 0)
                    {
                        strSql += ",";
                    }
                    strSql += " origem=" + ddlOrigem.SelectedValue;
                }

                if (strSql.Length <= 0)
                {
                    throw new Exception("Não foi selecionada nenhuma alteração!");
                }
                String strItens = "";
                foreach (GridViewRow item in GridItens.Rows)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                    if (chk != null && chk.Checked)
                    {
                        if (strItens.Length > 0)
                        {
                            strItens += ",";
                        }
                        strItens += "'" + item.Cells[1].Text + "'";
                    }

                }
                if (strItens.Trim().Length <= 0)
                {
                    throw new Exception("Nenhum item foi selecioando!");
                }

                Conexao.executarSql("update mercadoria set " + strSql + " where plu in(" + strItens + ")");

                lblError.Text = "Itens Alterados com Sucesso!!";
                lblError.ForeColor = System.Drawing.Color.Blue;
                pesquisa(false);
            }
            catch (Exception err)
            {

                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
            modalEncerrar.Hide();
        }

        protected void btnCancelarEncerrar_Click(object sender, ImageClickEventArgs e)
        {
            modalEncerrar.Hide();
        }

        protected void btnAplicarAlteracoes_Click(object sender, EventArgs e)
        {


            modalEncerrar.Show();
        }
    }
}