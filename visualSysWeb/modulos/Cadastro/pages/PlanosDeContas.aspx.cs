using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class PlanosDeContas : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                carregarGrupoPesquisa(true);
                EnabledControls(PnDetalhesGrupo, true);
                EnabledControls(pnSubGrupoDetalhes, true);
                EnabledControls(pnCentroCustoDetalhes, true);

                User usr = (User)Session["User"];
                if (usr == null)
                    return;

                if (Request.Params["tela"] != null)
                {
                    usr.tela = Request.Params["tela"].ToString();
                }
                if (!usr.adm())
                {
                    ImgBtnGrupoEditar.Visible = usr.telaPermissao("Grupo");
                    ImgBtnGrupoAdd.Visible = usr.telaPermissao("Grupo");
                    imgBtnGrupoExcluir.Visible = usr.telaPermissao("Grupo");

                    imgBtnSubGrupoEditar.Visible = usr.telaPermissao("SubGrupo");
                    imgBtnSubGrupoAdd.Visible = usr.telaPermissao("SubGrupo");
                    imgBtnSubGrupoExcluir.Visible = usr.telaPermissao("SubGrupo");

                    imgCentroCustoEditar.Visible = usr.telaPermissao("Centro de Custo");
                    imgCentroCustoAdd.Visible = usr.telaPermissao("Centro de Custo");
                    imgBtnCentroCustoExcluir.Visible = usr.telaPermissao("Centro de Custo");


                }

            }



        }


        protected String GrupoSelecionado(int campo)
        {
            foreach (GridViewRow item in GridGrupos.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoGrupoItem");

                if (rdo != null)
                {
                    if (rdo.Checked)
                    {

                        rdo.Focus();
                        return item.Cells[campo].Text;
                    }
                }
            }

            return "";
        }
        protected void GrupoFocus()
        {
            String strGrupo = (String)Session["grupoSelec"];
            foreach (GridViewRow item in GridGrupos.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoGrupoItem");
                if (rdo != null)
                {
                    if (strGrupo != null && item.Cells[1].Text.Equals(strGrupo))
                    {

                        item.RowState = DataControlRowState.Selected;
                        rdo.Checked = true;
                        rdo.Focus();
                        break;
                    }
                    else
                    {

                        if (rdo.Checked)
                        {
                            item.RowState = DataControlRowState.Selected;
                            rdo.Focus();
                            break;
                        }
                    }
                }
            }

        }

        protected String SubGrupoSelecionado(int campo)
        {

            foreach (GridViewRow item in GridSubGrupos.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoSubGrupoItem");


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
        protected void SubGrupoFocus()
        {
            String strSubGrupo = (String)Session["subGrupoSelec"];
            foreach (GridViewRow item in GridSubGrupos.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoSubGrupoItem");
                if (rdo != null)
                {
                    if (strSubGrupo != null && item.Cells[1].Text.Equals(strSubGrupo))
                    {

                        item.RowState = DataControlRowState.Selected;
                        rdo.Checked = true;
                        break;
                    }
                    else
                    {

                        if (rdo.Checked)
                        {
                            item.RowState = DataControlRowState.Selected;
                            break;
                        }
                    }
                }
            }

        }

        protected String CentroCustoSelecionado(int campo)
        {
            foreach (GridViewRow item in GridCentroCusto.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoCentroCustoItem");

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

        protected void CentroCustoFocus()
        {
            String strCentro = (String)Session["centroSelec"];
            foreach (GridViewRow item in GridCentroCusto.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoCentroCustoItem");
                if (rdo != null)
                {
                    if (strCentro != null && item.Cells[1].Text.Equals(strCentro))
                    {

                        item.RowState = DataControlRowState.Selected;
                        rdo.Checked = true;
                        break;
                    }
                    else
                    {

                        if (rdo.Checked)
                        {
                            item.RowState = DataControlRowState.Selected;
                            break;
                        }
                    }
                }
            }
        }
        protected void carregarGrupoPesquisa(bool primeiro)
        {
            String sqlGrupo = "Select * from grupo_cc where filial='MATRIZ'";

            if (!txtCodigoGrupo.Text.Trim().Equals(""))
            {
                sqlGrupo += " AND CODIGO_GRUPO ='" + txtCodigoGrupo.Text + "' ";
            }

            if (!txtDescricaoGrupo.Text.Trim().Equals(""))
            {
                sqlGrupo += " AND DESCRICAO_GRUPO LIKE '%" + txtDescricaoGrupo.Text + "%'";
            }
            GridGrupos.DataSource = Conexao.GetTable(sqlGrupo + " order by convert(int ,codigo_grupo) ", null, false);
            GridGrupos.DataBind();

            if (GridGrupos.Rows.Count > 0)
            {
                String strGrupo = (String)Session["grupoSelec"];
                if (strGrupo == null)
                {

                    int index = 0;
                    if (!primeiro)
                    {

                        index = GridGrupos.Rows.Count - 1;
                    }
                    RadioButton rdo = (RadioButton)GridGrupos.Rows[index].FindControl("RdoGrupoItem");
                    if (rdo != null)
                    {
                        GridGrupos.Rows[index].RowState = DataControlRowState.Selected;
                        rdo.Checked = true;
                    }
                }
                else
                {
                    GrupoFocus();
                    Session.Remove("grupoSelec");
                }
            }
            CarregarSubGrupoPesquisa(true);
        }

        protected void CarregarSubGrupoPesquisa(bool primeiro)
        {
            String Grupo = GrupoSelecionado(1);
            String sqlSubGrupo = "Select * from Subgrupo_cc where filial='MATRIZ' and codigo_grupo='" + Grupo + "'";

            if (!txtCodigoSubGrupo.Text.Trim().Equals(""))
            {
                sqlSubGrupo += " AND CODIGO_SUBGRUPO ='" + txtCodigoSubGrupo.Text + "' ";
            }

            if (!txtDescricaoGrupo.Text.Trim().Equals(""))
            {
                sqlSubGrupo += " AND DESCRICAO_SUBGRUPO LIKE '%" + txtDescricaoSubGrupo.Text + "%'";
            }
            GridSubGrupos.DataSource = Conexao.GetTable(sqlSubGrupo + " order by convert(int ,codigo_SubGrupo) ", null, false);
            GridSubGrupos.DataBind();

            if (GridSubGrupos.Rows.Count > 0)
            {
                String strsubGrupo = (String)Session["subGrupoSelec"];
                if (strsubGrupo == null)
                {

                    int index = 0;
                    if (!primeiro)
                        index = GridSubGrupos.Rows.Count - 1;

                    RadioButton rdo = (RadioButton)GridSubGrupos.Rows[index].FindControl("RdoSubGrupoItem");
                    if (rdo != null)
                    {
                        GridSubGrupos.Rows[index].RowState = DataControlRowState.Selected;
                        rdo.Checked = true;
                    }
                }
                else
                {
                    SubGrupoFocus();
                    Session.Remove("subGrupoSelec");
                }
            }
            CarregarCentroCustoPesquisa(true);

        }

        protected void CarregarCentroCustoPesquisa(bool primeiro)
        {

            lblError.Text = "";
            String subGrupo = SubGrupoSelecionado(1);
            String sqlCentroCusto = "select * from centro_custo where filial='MATRIZ' AND CODIGO_SUBGRUPO='" + subGrupo + "'";
            if (!txtCodigoCentroCusto.Text.Trim().Equals(""))
            {
                sqlCentroCusto += " AND CODIGO_CENTRO_CUSTO ='" + txtCodigoCentroCusto.Text + "' ";
            }

            if (!txtDescricaoCentroCusto.Text.Trim().Equals(""))
            {
                sqlCentroCusto += " AND DESCRICAO_CENTRO_CUSTO LIKE '%" + txtDescricaoCentroCusto.Text + "%'";
            }
            GridCentroCusto.DataSource = Conexao.GetTable(sqlCentroCusto + " order by convert(int ,codigo_centro_custo) ", null, false);
            GridCentroCusto.DataBind();

            if (GridCentroCusto.Rows.Count > 0)
            {
                String strGrupo = (String)Session["centroSelec"];
                if (strGrupo == null)
                {
                    int index = 0;
                    if (!primeiro)
                        index = GridCentroCusto.Rows.Count - 1;

                    RadioButton rdo = (RadioButton)GridCentroCusto.Rows[index].FindControl("RdoCentroCustoItem");
                    if (rdo != null)
                    {
                        GridCentroCusto.Rows[index].RowState = DataControlRowState.Selected;
                        rdo.Checked = true;
                    }

                }
                else
                {
                    CentroCustoFocus();
                    Session.Remove("centroSelec");
                }
            }

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
            String[] campos = {"txtCodGrupoSubGrupo", 
                               "txtDescricaoGrupoSubGrupo", 
                               "txtCodSubGrupoCentroCusto", 
                               "txtDescricaoSubGrupoCentroCusto",
                               "txtCodGrupoCentroCusto",
                               "txtDescricaoGrupoCentroCusto"
                                     };


            return existeNoArray(campos, campo.ID + "");

        }

        protected override bool campoObrigatorio(Control campo)
        {
            return false;
        }

        protected void GridGrupos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoGrupoItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('GridGrupos.*GrGrupo',this)";
            rdo.Attributes.Add("onclick", script);

        }

        protected void GridSubGrupos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoSubGrupoItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('GridSubGrupos.*GrSubGrupo',this)";
            rdo.Attributes.Add("onclick", script);

        }

        protected void GridCentroCusto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoCentroCustoItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('GridCentroCusto.*GrCentroCusto',this)";
            rdo.Attributes.Add("onclick", script);

        }

        protected void ImgBtnGrupoPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            carregarGrupoPesquisa(true);
        }

        protected void ImgBtnGrupoEditar_Click(object sender, ImageClickEventArgs e)
        {
            if (!GrupoSelecionado(1).Equals("------"))
            {
                txtCodigoGrupoDetalhe.Text = GrupoSelecionado(1);

                txtDescricaoGrupoDetalhe.Text = GrupoSelecionado(2).Replace("&nbsp;", "");
                txtCodigoGrupoDetalhe.Enabled = false;
                String mod = GrupoSelecionado(3).Replace("&nbsp;", "");
                ddlModalidade.Text = mod;
                txtDescricaoGrupoDetalhe.Focus();
                modalGrupoDetalhe.Show();
            }

        }

        protected void ImgBtnGrupoAdd_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = "";
            lblErroGrupoDetalhes.Text = "";
            String proximoGrupo = "1";
            if (GridGrupos.Rows.Count > 0)
            {
                String prx = GridGrupos.Rows[GridGrupos.Rows.Count - 1].Cells[1].Text;
                if (!prx.Equals("------"))
                {
                    proximoGrupo = (int.Parse(prx) + 1).ToString();
                }
            }

            txtCodigoGrupoDetalhe.Text = proximoGrupo;
            txtDescricaoGrupoDetalhe.Text = "";
            txtCodigoGrupoDetalhe.Enabled = true;
            txtDescricaoGrupoDetalhe.Focus();
            modalGrupoDetalhe.Show();
        }

        protected void imgBtnSubGrupoPesquisa_Click(object sender, ImageClickEventArgs e)
        {
            CarregarSubGrupoPesquisa(true);
        }

        protected void imgBtnSubGrupoEditar_Click(object sender, ImageClickEventArgs e)
        {

            lblError.Text = "";
            lblErrorSubGrupoDetalhes.Text = "";
            if (!SubGrupoSelecionado(1).Equals("------"))
            {
                txtCodGrupoSubGrupo.Text = GrupoSelecionado(1).Replace("&nbsp;", "");
                txtDescricaoGrupoSubGrupo.Text = GrupoSelecionado(2).Replace("&nbsp;", "");
                txtCodigoSubGrupoDetalhes.Text = SubGrupoSelecionado(1).Replace("&nbsp;", "");
                txtCodigoSubGrupoDetalhes.Enabled = false;
                txtDescricaoSubGrupoDetalhes.Text = SubGrupoSelecionado(2).Replace("&nbsp;", "");
                txtDescricaoSubGrupoDetalhes.Focus();
                modalSubGrupoDetalhes.Show();
            }

        }

        protected void imgBtnSubGrupoAdd_Click(object sender, ImageClickEventArgs e)
        {

            lblError.Text = "";
            lblErrorSubGrupoDetalhes.Text = "";
            if (GrupoSelecionado(1).Equals("------"))
            {
                lblError.Text = " Não é possibel incluir um SubGrupo sem um Grupo ";

                lblError.ForeColor = System.Drawing.Color.Red;
            }
            else
            {

                String proximoSubGrupo = "1";
                if (GridSubGrupos.Rows.Count > 0)
                {
                    String strprx = GridSubGrupos.Rows[GridSubGrupos.Rows.Count - 1].Cells[1].Text.Replace("&nbsp;", "");
                    if (!strprx.Equals("------"))
                    {
                        proximoSubGrupo = (int.Parse(strprx.Substring(3)) + 1).ToString().Replace("&nbsp;", "");
                    }
                }

                txtCodGrupoSubGrupo.Text = GrupoSelecionado(1).Replace("&nbsp;", "");
                txtDescricaoGrupoSubGrupo.Text = GrupoSelecionado(2).Replace("&nbsp;", "");
                txtCodigoSubGrupoDetalhes.Enabled = true;
                txtCodigoSubGrupoDetalhes.Text = txtCodGrupoSubGrupo.Text.Trim().PadLeft(3, '0').Replace("&nbsp;", "") + proximoSubGrupo.PadLeft(3, '0').Replace("&nbsp;", "");
                txtDescricaoSubGrupoDetalhes.Text = "";
                txtDescricaoSubGrupoDetalhes.Focus();
                modalSubGrupoDetalhes.Show();
            }
        }

        protected void imgCentroCustoPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            CarregarCentroCustoPesquisa(true);
        }

        protected void imgCentroCustoEditar_Click(object sender, ImageClickEventArgs e)
        {

            lblError.Text = "";
            lblCentroCustoErro.Text = "";
            if (!CentroCustoSelecionado(1).Equals("------"))
            {

                txtCodGrupoCentroCusto.Text = GrupoSelecionado(1).Replace("&nbsp;", "");

                txtDescricaoGrupoCentroCusto.Text = GrupoSelecionado(2).Replace("&nbsp;", "");
                txtCodSubGrupoCentroCusto.Text = SubGrupoSelecionado(1).Replace("&nbsp;", "");
                txtDescricaoSubGrupoCentroCusto.Text = SubGrupoSelecionado(2).Replace("&nbsp;", "");
                centro_custoDAO centro  =new centro_custoDAO(CentroCustoSelecionado(1).Replace("&nbsp;", ""));
                txtCodigoCentroCustoDetalhe.Text = centro.Codigo_centro_custo;
                txtDescricaoCentroCustoDetalhe.Text = centro.descricao_centro_custo;

                txtDescricaoCentroCustoDetalhe.Focus();
                
                txtConta.Text = CentroCustoSelecionado(3).Replace("&nbsp;", "");
                
                
                txtCodigoCentroCustoDetalhe.Enabled = false;
                txtAgrupamento.Text = CentroCustoSelecionado(4).Replace("&nbsp;", "");
                txtContaContabilCredito.Text = centro.conta_contabil_credito;
                txtContaContabilDebito.Text = centro.conta_contabil_debito;
                
                modalCentroCustoDetalhes.Show();
            }

        }

        protected void imgCentroCustoAdd_Click(object sender, ImageClickEventArgs e)
        {

            lblError.Text = "";
            lblCentroCustoErro.Text = "";
            if (SubGrupoSelecionado(1).Equals("------"))
            {
                lblError.Text = "Não é possivel incluir um Centro de Custo Sem um SubGrupo!";

                lblError.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                String proximoCentroCusto = "1";
                if (GridCentroCusto.Rows.Count > 0)
                {
                    String prx = GridCentroCusto.Rows[GridCentroCusto.Rows.Count - 1].Cells[1].Text;
                    if (!prx.Equals("------"))
                    {
                        proximoCentroCusto = (int.Parse(prx.Substring(6)) + 1).ToString();
                    }
                }
                txtCodGrupoCentroCusto.Text = GrupoSelecionado(1);
                txtDescricaoGrupoCentroCusto.Text = GrupoSelecionado(2);
                txtCodSubGrupoCentroCusto.Text = SubGrupoSelecionado(1);
                txtDescricaoSubGrupoCentroCusto.Text = SubGrupoSelecionado(2);
                txtCodigoCentroCustoDetalhe.Enabled = true;
                txtDescricaoCentroCustoDetalhe.Text = "";
                
                txtCodigoCentroCustoDetalhe.Text = txtCodSubGrupoCentroCusto.Text.Trim() + proximoCentroCusto.PadLeft(3, '0');
                txtConta.Text = "";
                txtAgrupamento.Text = "";
                txtContaContabilCredito.Text = "";
                txtContaContabilDebito.Text = "";
                
                txtDescricaoCentroCustoDetalhe.Focus();
                modalCentroCustoDetalhes.Show();
            }
        }

        protected void RdoGrupoItem_CheckedChanged(object sender, EventArgs e)
        {
            CarregarSubGrupoPesquisa(true);
            GrupoFocus();
            SubGrupoFocus();
            CentroCustoFocus();
        }
        protected void RdoSubGrupoItem_CheckedChanged(object sender, EventArgs e)
        {
            CarregarCentroCustoPesquisa(true);
            GrupoFocus();
            SubGrupoFocus();
            CentroCustoFocus();
        }
        protected void RdoCentroCustoItem_CheckedChanged(object sender, EventArgs e)
        {
            GrupoFocus();
            SubGrupoFocus();
            CentroCustoFocus();
        }

        protected void imgBtnGrupoExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                grupo_ccDAO grupo = new grupo_ccDAO();
                grupo.codigo_grupo = GrupoSelecionado(1);
                grupo.excluir();
                carregarGrupoPesquisa(true);
            }
            catch (Exception err)
            {

                lblError.Text = err.Message;

                lblError.ForeColor = System.Drawing.Color.Red;
            }

        }

        protected void imgBtnSubGrupoExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                subgrupo_ccDAO subGrupo = new subgrupo_ccDAO();
                subGrupo.Codigo_Subgrupo = SubGrupoSelecionado(1);
                subGrupo.excluir();
                CarregarSubGrupoPesquisa(true);
            }
            catch (Exception err)
            {

                lblError.Text = err.Message;

                lblError.ForeColor = System.Drawing.Color.Red;
            }

        }

        protected void imgBtnCentroCustoExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {


                centro_custoDAO centro = new centro_custoDAO();
                centro.Codigo_centro_custo = CentroCustoSelecionado(1);
                centro.excluir();
                CarregarCentroCustoPesquisa(true);
            }
            catch (Exception err)
            {

                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;

            }
        }

        protected void btnConfirmaDetalhesGrupo_Click(object sender, ImageClickEventArgs e)
        {
            if (txtDescricaoGrupoDetalhe.Text.Trim().Equals("") || txtCodigoGrupoDetalhe.Text.Trim().Equals("") || ddlModalidade.SelectedValue.Equals(""))
            {
                lblErroGrupoDetalhes.Text = "OS CAMPOS NÃO PODEM ESTAR EM BRANCO!";
                modalGrupoDetalhe.Show();
            }
            else
            {
                try
                {
                    grupo_ccDAO grupo = new grupo_ccDAO();
                    grupo.codigo_grupo = txtCodigoGrupoDetalhe.Text;
                    grupo.Descricao_grupo = txtDescricaoGrupoDetalhe.Text;
                    grupo.modalidade = ddlModalidade.Text;
                    grupo.salvar(txtCodigoGrupoDetalhe.Enabled);
                    Session.Remove("grupoSelec");
                    Session.Add("grupoSelec", txtCodigoGrupoDetalhe.Text);
                    carregarGrupoPesquisa(false);


                }
                catch (Exception err)
                {

                    lblErroGrupoDetalhes.Text = err.Message;
                    modalGrupoDetalhe.Show();
                }

            }
        }

        protected void btnCancelaDetalhesGrupo_Click(object sender, ImageClickEventArgs e)
        {
            modalGrupoDetalhe.Hide();
        }

        protected void imgBtnConfirmaSubGrupoDetalhes_Click(object sender, ImageClickEventArgs e)
        {

            if (txtDescricaoSubGrupoDetalhes.Text.Trim().Equals("") || txtCodigoSubGrupoDetalhes.Text.Trim().Equals(""))
            {
                lblErrorSubGrupoDetalhes.Text = "OS CAMPOS NÃO PODEM ESTAR EM BRANCO!";
                modalSubGrupoDetalhes.Show();
            }
            else
            {
                try
                {
                    subgrupo_ccDAO subGrupo = new subgrupo_ccDAO();
                    subGrupo.codigo_grupo = txtCodGrupoSubGrupo.Text;
                    subGrupo.Codigo_Subgrupo = txtCodigoSubGrupoDetalhes.Text;
                    subGrupo.Descricao_Subgrupo = txtDescricaoSubGrupoDetalhes.Text;
                    subGrupo.salvar(txtCodigoSubGrupoDetalhes.Enabled);
                    Session.Remove("subGrupoSelec");
                    Session.Add("subGrupoSelec", txtCodigoSubGrupoDetalhes.Text);

                    CarregarSubGrupoPesquisa(false);


                }
                catch (Exception err)
                {

                    lblErrorSubGrupoDetalhes.Text = err.Message;
                    modalSubGrupoDetalhes.Show();
                }

            }

        }

        protected void imgBtnCancelaSubGrupoDetalhes_Click(object sender, ImageClickEventArgs e)
        {

        }
        protected void imgBtnConfirmaCentroCustoDetalhes_Click(object sender, ImageClickEventArgs e)
        {
            String modalidade = GrupoSelecionado(3).Replace("&nbsp;", "");
            if (modalidade.Equals(""))
            {
                lblCentroCustoErro.Text = "O GRUPO SELECIONADO NÃO ESTA COM O CAMPO MODALIDADE PREENCHIDO!! ";

                modalCentroCustoDetalhes.Show();

            }
            else if (txtDescricaoCentroCustoDetalhe.Text.Trim().Equals("") || txtCodigoCentroCustoDetalhe.Text.Trim().Equals(""))
            {
                lblCentroCustoErro.Text = "OS CAMPOS NÃO PODEM ESTAR EM BRANCO!";
                modalCentroCustoDetalhes.Show();
            }
            else
            {
                try
                {
                    centro_custoDAO centro = new centro_custoDAO();
                    centro.Codigo_subgrupo = txtCodSubGrupoCentroCusto.Text;
                    centro.Codigo_centro_custo = txtCodigoCentroCustoDetalhe.Text;
                    centro.descricao_centro_custo = txtDescricaoCentroCustoDetalhe.Text;
                    centro.modalidade = modalidade;
                    centro.Agrupamento = txtAgrupamento.Text;
                    centro.id_cc = txtConta.Text;
                    centro.conta_contabil_credito = txtContaContabilCredito.Text;
                    centro.conta_contabil_debito = txtContaContabilDebito.Text;
                    centro.salvar(txtCodigoCentroCustoDetalhe.Enabled);
                    Session.Remove("centroSelec");
                    Session.Add("centroSelec", txtCodigoCentroCustoDetalhe.Text);

                    CarregarCentroCustoPesquisa(false);


                }
                catch (Exception err)
                {

                    lblCentroCustoErro.Text = err.Message;

                    modalCentroCustoDetalhes.Show();
                }

            }

        }
        protected void imgBtnCancelaCentroCustoDetalhes_Click(object sender, ImageClickEventArgs e)
        {
            modalCentroCustoDetalhes.Hide();
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetContas(string prefixText, int count)
        {
            String sql = "Select id_cc from conta_corrente where id_cc like '" + (prefixText.Length > 4 ? "%" : "") + prefixText + "%'";
            return Conexao.retornaArray(sql, prefixText.Length);
        }
    }
}