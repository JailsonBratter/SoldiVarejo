using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;
using visualSysWeb.modulos.Cadastro.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class Departamentos : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divCategoriasAll.Visible = Funcoes.valorParametro("INCLUI_CATEGORIA", null).ToUpper().Equals("TRUE");
                if (divCategoria.Visible)
                    Div1.Attributes.Add("style", "height:2000px;");
                carregarGrupoPesquisa(true);
                EnabledControls(PnDetalhesGrupo, true);
                EnabledControls(pnSubGrupoDetalhes, true);
                EnabledControls(pnDepartamentoDetalhes, true);
                //EnabledControls(pnCategoria, true);
                //EnabledControls(pnSeguimento, true);
                //EnabledControls(pnSubSeguimento, true);
                //EnabledControls(pnGrupoCategoria, true);
                //EnabledControls(pnSubGrupoCategoria, true);
                txtDescricaoCategoriaDetalhes.Attributes.Add("OnChange", "javascript:this.value = this.value.toUpperCase();");
                txtDescSeguimentoDetalhes.Attributes.Add("OnChange", "javascript:this.value = this.value.toUpperCase();");
                txtDescSubSeguimentoDetalhes.Attributes.Add("OnChange", "javascript:this.value = this.value.toUpperCase();");
                txtDescGrupoCategoriaDetalhes.Attributes.Add("OnChange", "javascript:this.value = this.value.toUpperCase();");
                txtDescSubGrupoCategoria.Attributes.Add("OnChange", "javascript:this.value = this.value.toUpperCase();");
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
                    
                    imgDepartamentoEditar.Visible = usr.telaPermissao("Departamento");
                    imgDepartamentoAdd.Visible = usr.telaPermissao("Departamento");
                    imgBtnDepartamentoExcluir.Visible = usr.telaPermissao("Departamento");


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

        protected void focus(String select, GridView grid, String radioBtn)
        {
            String strSelect = (String)Session[select];
            foreach (GridViewRow item in grid.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl(radioBtn);
                if (rdo != null)
                {
                    if (strSelect != null && item.Cells[1].Text.Equals(strSelect))
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
                        return item.Cells[campo].Text.Replace("&nbsp;", "");
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

        protected String DepartamentoSelecionado(int campo)
        {
            foreach (GridViewRow item in GridDepartamento.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoDepartamentoItem");

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

        protected void DepartamentoFocus()
        {
            String strCentro = (String)Session["departamentoSelec"];
            foreach (GridViewRow item in GridDepartamento.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoDepartamentoItem");
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
         
            String sqlGrupo = "Select * from grupo where filial='MATRIZ'";

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
            String sqlSubGrupo = "Select * from Subgrupo where filial='MATRIZ' and codigo_grupo='" + Grupo + "'";

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
            CarregarDepartamentoPesquisa(true);

        }

        protected void CarregarDepartamentoPesquisa(bool primeiro)
        {

            lblError.Text = "";
            String subGrupo = SubGrupoSelecionado(1);
            String sqlDepartamento = "select *, strCardapio=case when isnull(cardapio,0)=1 then 'SIM' ELSE 'NAO' END from departamento where filial='MATRIZ' AND CODIGO_SUBGRUPO='" + subGrupo + "'";
            if (!txtCodigoDepartamento.Text.Trim().Equals(""))
            {
                sqlDepartamento += " AND CODIGO_DEPARTAMENTO ='" + txtCodigoDepartamento.Text + "' ";
            }

            if (!txtDescricaoDepartamento.Text.Trim().Equals(""))
            {
                sqlDepartamento += " AND DESCRICAO_DEPARTAMENTO LIKE '%" + txtDescricaoDepartamento.Text + "%'";
            }
            GridDepartamento.DataSource = Conexao.GetTable(sqlDepartamento + " order by convert(int ,codigo_Departamento) ", null, false);
            GridDepartamento.DataBind();

            if (GridDepartamento.Rows.Count > 0)
            {
                String strGrupo = (String)Session["departamentoSelec"];
                if (strGrupo == null)
                {
                    int index = 0;
                    if (!primeiro)
                        index = GridDepartamento.Rows.Count - 1;

                    RadioButton rdo = (RadioButton)GridDepartamento.Rows[index].FindControl("RdoDepartamentoItem");
                    if (rdo != null)
                    {
                        GridDepartamento.Rows[index].RowState = DataControlRowState.Selected;
                        rdo.Checked = true;
                    }

                }
                else
                {
                    DepartamentoFocus();
                    Session.Remove("departamentoSelec");
                }
            }

            if (divCategoriasAll.Visible)
            {
                carregarCategorias(true);
            }

        }

        private void carregarCategorias(bool primeiro)
        {
            String sql = "Select * from categorias " +
                         "where codigo_departamento='" + DepartamentoSelecionado(1) + "' order by codigo";

            gridCategorias.DataSource = Conexao.GetTable(sql, null, false);
            gridCategorias.DataBind();


            selecionarItem(gridCategorias, "selectCategoria", "RdoCategoriaItem", true);
            carregarSeguimento(true);

        }
        public bool selecionarItem(GridView grid, String select, String rdoItem, bool primeiro)
        {
            if (grid.Rows.Count > 0)
            {
                String strSelec = (String)Session[select];
                if (strSelec == null)
                {

                    int index = 0;
                    if (!primeiro)
                    {

                        index = grid.Rows.Count - 1;
                    }
                    RadioButton rdo = (RadioButton)grid.Rows[index].FindControl(rdoItem);
                    if (rdo != null)
                    {
                        grid.Rows[index].RowState = DataControlRowState.Selected;
                        rdo.Checked = true;
                    }
                    return true;
                }
                else
                {
                    focus(select, grid, rdoItem);
                    Session.Remove(select);
                }
            }
            return false;

        }

        private void carregarSeguimento(bool primeiro)
        {
            String sql = "Select * from categorias " +
             "where codigo_departamento='" + CategoriaSelecionado(1) + "' order by codigo";

            gridSeguimento.DataSource = Conexao.GetTable(sql, null, false);
            gridSeguimento.DataBind();

            selecionarItem(gridSeguimento, "selectSeguimento", "RdoSeguimentoItem", primeiro);
            carregarSubSeguimento(true);

        }

        private string CategoriaSelecionado(int campo)
        {
            foreach (GridViewRow item in gridCategorias.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoCategoriaItem");

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

        private string SeguimentoSelecionado(int campo)
        {
            foreach (GridViewRow item in gridSeguimento.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoSeguimentoItem");

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
        private string SubSeguimentoSelecionado(int campo)
        {
            foreach (GridViewRow item in gridSubSeguimento.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoSubSeguimentoItem");

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
        private string CategoriaGrupoSelecionado(int campo)
        {
            foreach (GridViewRow item in gridCategoriasGrupo.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoCategoriaGrupoItem");

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

        private string CategoriaSubGrupoSelecionado(int campo)
        {
            foreach (GridViewRow item in gridCategoriasSubGrupo.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoCategoriaSubGrupoItem");

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

        private void carregarSubSeguimento(bool primeiro)
        {
            String sql = "Select * from categorias " +
              "where codigo_departamento='" + SeguimentoSelecionado(1) + "' order by codigo";

            gridSubSeguimento.DataSource = Conexao.GetTable(sql, null, false);
            gridSubSeguimento.DataBind();
            selecionarItem(gridSubSeguimento, "selectSubSeguimento", "RdoSubSeguimentoItem", primeiro);
            carregarCategoriaGrupo(true);

        }

        private void carregarCategoriaGrupo(bool primeiro)
        {
            String sql = "Select * from categorias " +
             "where codigo_departamento='" + SubSeguimentoSelecionado(1) + "' order by codigo";

            gridCategoriasGrupo.DataSource = Conexao.GetTable(sql, null, false);
            gridCategoriasGrupo.DataBind();
            selecionarItem(gridCategoriasGrupo, "selectCategoriaGrupo", "RdoCategoriaGrupoItem", primeiro);
            carregarCategoriaSubGrupo(true);
        }

        private void carregarCategoriaSubGrupo(bool primeiro)
        {
            String sql = "Select * from categorias " +
            "where codigo_departamento='" + CategoriaGrupoSelecionado(1) + "' order by codigo";

            gridCategoriasSubGrupo.DataSource = Conexao.GetTable(sql, null, false);
            gridCategoriasSubGrupo.DataBind();
            selecionarItem(gridCategoriasSubGrupo, "selectCategoriaSubGrupo", "RdoCategoriaSubGrupoItem", primeiro);
        }




        protected override void btnIncluir_Click(object sender, EventArgs e)
        {

        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {

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
                               "txtCodSubGrupoDepartamento",
                               "txtDescricaoSubGrupoDepartamento",
                               "txtCodGrupoDepartamento",
                               "txtDescricaoGrupoDepartamento"

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

        protected void GridDepartamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoDepartamentoItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('GridDepartamento.*GrDepartamento',this)";
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

                txtDescricaoGrupoDetalhe.Text = GrupoSelecionado(2);
                txtCodigoGrupoDetalhe.Enabled = false;
                txtDescricaoGrupoDetalhe.Focus();
                modalGrupoDetalhe.Show();
            }

        }

        protected void ImgBtnGrupoAdd_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = "";
            String proximoGrupo = "1";
            if (GridGrupos.Rows.Count > 0)
            {
                String prx = GridGrupos.Rows[GridGrupos.Rows.Count - 1].Cells[1].Text;
                if (!prx.Equals("------") && !prx.Equals("999"))
                {
                    proximoGrupo = (int.Parse(prx) + 1).ToString();
                }
                else
                {
                    if (GridGrupos.Rows.Count > 1)
                    {
                        prx = GridGrupos.Rows[GridGrupos.Rows.Count - 2].Cells[1].Text;
                        proximoGrupo = (int.Parse(prx) + 1).ToString();
                    }

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
            if (!SubGrupoSelecionado(1).Equals("------"))
            {
                txtCodGrupoSubGrupo.Text = GrupoSelecionado(1);
                txtDescricaoGrupoSubGrupo.Text = GrupoSelecionado(2);
                txtCodigoSubGrupoDetalhes.Text = SubGrupoSelecionado(1);
                txtCodigoSubGrupoDetalhes.Enabled = false;
                txtDescricaoSubGrupoDetalhes.Text = SubGrupoSelecionado(2);
                

                txtDescricaoSubGrupoDetalhes.Focus();
                modalSubGrupoDetalhes.Show();
            }

        }

        protected void imgBtnSubGrupoAdd_Click(object sender, ImageClickEventArgs e)
        {

            lblError.Text = "";
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
                    String strprx = GridSubGrupos.Rows[GridSubGrupos.Rows.Count - 1].Cells[1].Text;
                    if (!strprx.Equals("------"))
                    {
                        proximoSubGrupo = (int.Parse(strprx.Substring(3)) + 1).ToString();
                    }
                }

                txtCodGrupoSubGrupo.Text = GrupoSelecionado(1);
                txtDescricaoGrupoSubGrupo.Text = GrupoSelecionado(2);
                txtCodigoSubGrupoDetalhes.Enabled = true;
                txtCodigoSubGrupoDetalhes.Text = txtCodGrupoSubGrupo.Text.PadLeft(3, '0') + proximoSubGrupo.PadLeft(3, '0');
                txtDescricaoSubGrupoDetalhes.Text = "";

                txtDescricaoSubGrupoDetalhes.Focus();
                modalSubGrupoDetalhes.Show();
            }
        }

        protected void imgDepartamentoPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            CarregarDepartamentoPesquisa(true);
        }

        protected void imgDepartamentoEditar_Click(object sender, ImageClickEventArgs e)
        {

            lblError.Text = "";
            if (!DepartamentoSelecionado(1).Equals("------"))
            {

                txtCodGrupoDepartamento.Text = GrupoSelecionado(1);

                txtDescricaoGrupoDepartamento.Text = GrupoSelecionado(2);
                txtCodSubGrupoDepartamento.Text = SubGrupoSelecionado(1);
                txtDescricaoSubGrupoDepartamento.Text = SubGrupoSelecionado(2);
                txtCodigoDepartamentoDetalhe.Text = DepartamentoSelecionado(1);
                txtDescricaoDepartamentoDetalhe.Text = DepartamentoSelecionado(2);
                DepartamentoDAO dep = new DepartamentoDAO(txtCodigoDepartamentoDetalhe.Text);
                //txtImpressoraRemota.Text = dep.impressora_remota;
                //txtId_trm.Text = dep.id_trm;
                chkCardapio.Checked = dep.cardapio; 
                txtDescricaoDepartamentoDetalhe.Focus();
                txtCodigoDepartamentoDetalhe.Enabled = false;
                imgAddFornecedor.Visible = true;
                lblIncluirFornecedor.Text = "Incluir Novo Fornecedor";
                carregarFornecedores();
                carregarImpressoras();
                modalDepartamentoDetalhes.Show();
            }

        }

        private void carregarFornecedores()
        {
            String sql = "Select f.Fornecedor," +
                               " f.CNPJ," +
                               " f.Razao_social," +
                               " Contrato='' " +
                          "from fornecedor_departamento as fd " +
                           " inner join fornecedor as f on ltrim(rtrim(fd.fornecedor))=ltrim(rtrim(f.Fornecedor)) " +
                          " Where codigo_departamento ='" + txtCodigoDepartamentoDetalhe.Text + "'";
            gridFornecedorDepartamento.DataSource = Conexao.GetTable(sql, null, false);
            gridFornecedorDepartamento.DataBind();



        }

        private void carregarImpressoras()
        {
            String sql = "Select filial.Loja, filial.Filial , si.impressora_remota, si.Descricao " +
                   " from Spool_Loja_Impressoras as sli " +
                       " inner join filial  on sli.Loja  = Filial.loja " +
                       " inner join Spool_impressoras as si on sli.Impressora_Remota = si.impressora_remota " +
                   " where sli.Codigo_Departamento ='" + txtCodigoDepartamentoDetalhe.Text + "'";

            gridImpressoras.DataSource = Conexao.GetTable(sql, null, false);
            gridImpressoras.DataBind();


        }


        protected void imgDepartamentoAdd_Click(object sender, ImageClickEventArgs e)
        {

            lblError.Text = "";
            if (SubGrupoSelecionado(1).Equals("------"))
            {
                lblError.Text = "Não é possivel incluir um Centro de Custo Sem um SubGrupo!";

                lblError.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                String proximoDepartamento = "1";
                if (GridDepartamento.Rows.Count > 0)
                {
                    String prx = GridDepartamento.Rows[GridDepartamento.Rows.Count - 1].Cells[1].Text;
                    if (!prx.Equals("------"))
                    {
                        proximoDepartamento = (int.Parse(prx.Substring(6)) + 1).ToString();
                    }
                }
                txtCodGrupoDepartamento.Text = GrupoSelecionado(1);
                txtDescricaoGrupoDepartamento.Text = GrupoSelecionado(2);
                txtCodSubGrupoDepartamento.Text = SubGrupoSelecionado(1);
                txtDescricaoSubGrupoDepartamento.Text = SubGrupoSelecionado(2);
                txtCodigoDepartamentoDetalhe.Enabled = true;
                txtDescricaoDepartamentoDetalhe.Text = "";
                txtCodigoDepartamentoDetalhe.Text = txtCodSubGrupoDepartamento.Text + proximoDepartamento.PadLeft(3, '0');
                txtDescricaoDepartamentoDetalhe.Focus();

                imgAddFornecedor.Visible = false;
                lblIncluirFornecedor.Text = "Para Incluir os Fornecedores confirme a inclusão do Departamento";
                carregarFornecedores();
                modalDepartamentoDetalhes.Show();
            }
        }

        protected void RdoGrupoItem_CheckedChanged(object sender, EventArgs e)
        {
            CarregarSubGrupoPesquisa(true);
            GrupoFocus();
            SubGrupoFocus();
            DepartamentoFocus();
        }
        protected void RdoSubGrupoItem_CheckedChanged(object sender, EventArgs e)
        {
            CarregarDepartamentoPesquisa(true);
            GrupoFocus();
            SubGrupoFocus();
            DepartamentoFocus();
        }
        protected void RdoDepartamentoItem_CheckedChanged(object sender, EventArgs e)
        {
            carregarCategorias(true);
            GrupoFocus();
            SubGrupoFocus();
            DepartamentoFocus();
        }

        protected void imgBtnGrupoExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                grupoDAO grupo = new grupoDAO();
                grupo.Codigo_Grupo = GrupoSelecionado(1);
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
                SubGrupoDAO subGrupo = new SubGrupoDAO();
                subGrupo.Codigo_SubGrupo = SubGrupoSelecionado(1);
                subGrupo.excluir();
                CarregarSubGrupoPesquisa(true);
            }
            catch (Exception err)
            {

                lblError.Text = err.Message;

                lblError.ForeColor = System.Drawing.Color.Red;
            }

        }

        protected void imgBtnDepartamentoExcluir_Click(object sender, ImageClickEventArgs e)
        {
            lblExcluirDepartamento.Text = DepartamentoSelecionado(1);


            modalConfirmaExcluirDepartamento.Show();
        }

        protected void btnConfirmaDetalhesGrupo_Click(object sender, ImageClickEventArgs e)
        {
            if (txtDescricaoGrupoDetalhe.Text.Trim().Equals("") || txtCodigoGrupoDetalhe.Text.Trim().Equals(""))
            {
                lblErroGrupoDetalhes.Text = "OS CAMPOS NÃO PODEM ESTAR EM BRANCO!";
                modalGrupoDetalhe.Show();
            }
            else
            {
                try
                {
                    grupoDAO grupo = new grupoDAO();
                    grupo.Codigo_Grupo = txtCodigoGrupoDetalhe.Text;
                    grupo.Descricao_Grupo = txtDescricaoGrupoDetalhe.Text;
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
                    SubGrupoDAO subGrupo = new SubGrupoDAO();
                    subGrupo.Codigo_Grupo = txtCodGrupoSubGrupo.Text;
                    subGrupo.Codigo_SubGrupo = txtCodigoSubGrupoDetalhes.Text;
                    subGrupo.Descricao_SubGrupo = txtDescricaoSubGrupoDetalhes.Text;

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
        protected void imgBtnConfirmaDepartamentoDetalhes_Click(object sender, ImageClickEventArgs e)
        {

            if (txtDescricaoDepartamentoDetalhe.Text.Trim().Equals("") || txtCodigoDepartamentoDetalhe.Text.Trim().Equals(""))
            {
                lblDepartamentoErro.Text = "OS CAMPOS NÃO PODEM ESTAR EM BRANCO!";
                modalDepartamentoDetalhes.Show();
            }
            else
            {
                try
                {
                    DepartamentoDAO departamento = new DepartamentoDAO();
                    departamento.Codigo_SubGrupo = txtCodSubGrupoDepartamento.Text;
                    departamento.Codigo_departamento = txtCodigoDepartamentoDetalhe.Text;
                    departamento.Descricao_departamento = txtDescricaoDepartamentoDetalhe.Text;
                    departamento.cardapio = chkCardapio.Checked;
                    //departamento.impressora_remota = txtImpressoraRemota.Text;
                    //departamento.id_trm = txtId_trm.Text;


                    departamento.salvar(txtCodigoDepartamentoDetalhe.Enabled);
                    Session.Remove("departamentoSelec");
                    Session.Add("departamentoSelec", txtCodigoDepartamentoDetalhe.Text);

                    CarregarDepartamentoPesquisa(false);


                }
                catch (Exception err)
                {

                    lblDepartamentoErro.Text = err.Message;
                    modalDepartamentoDetalhes.Show();
                }

            }

        }
        protected void imgBtnCancelaDepartamentoDetalhes_Click(object sender, ImageClickEventArgs e)
        {
            modalDepartamentoDetalhes.Hide();
        }

        protected void imgAddFornecedor_Click(object sender, ImageClickEventArgs e)
        {
            Session.Remove("camporecebe" + urlSessao());
            Session.Add("camporecebe" + urlSessao(), "Fornecedor");
            exibeLista();
        }

        protected void gridFornecedorDepartamento_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow rw = gridFornecedorDepartamento.Rows[index];

            lblCodFornecExcluir.Text = rw.Cells[1].Text;
            modalDepartamentoDetalhes.Show();
            modalPnConfirma.Show();

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
                        return item.Cells[campo].Text.Trim();
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
                try
                {

                    lbllista.Text = "";
                    String listaAtual = (String)Session["camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                    Session.Remove("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));

                    modalPnFundo.Hide();
                    if (listaAtual.Equals("Fornecedor"))
                    {
                        String strDepartamento = txtCodigoDepartamentoDetalhe.Text;
                        int nForn = 0;
                        int.TryParse(Conexao.retornaUmValor("Select count(*) from fornecedor_departamento where fornecedor ='" + selecionado.Trim() + "' and codigo_departamento ='" + strDepartamento.Trim() + "'", null), out nForn);
                        if (nForn <= 0)
                        {
                            Conexao.executarSql("insert into fornecedor_departamento values('" + selecionado.Trim() + "','" + strDepartamento.Trim() + "')");
                        }
                        carregarFornecedores();
                        modalDepartamentoDetalhes.Show();

                    }
                    else if (listaAtual.Equals("imgPesquisaLoja"))
                    {
                        txtLoja.Text = selecionado;
                        txtDescLoja.Text = ListaSelecionada(2);
                        modalDepartamentoDetalhes.Show();
                        modalAddImpressora.Show();
                    }
                    else if (listaAtual.Equals("imgPesquisaImpressora"))
                    {
                        txtImpressora.Text = selecionado;
                        txtDescImpressora.Text = ListaSelecionada(2);
                        modalDepartamentoDetalhes.Show();
                        modalAddImpressora.Show();
                    }



                }
                catch (Exception err)
                {

                    lbllista.Text = err.Message;

                    modalPnFundo.Show();
                }
            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                modalPnFundo.Show();
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

        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }
        protected void exibeLista()
        {

            lblErroPesquisa.Text = "";
            User usr = (User)Session["User"];
            String or = (String)Session["camporecebe" + urlSessao()];
            String sqlLista = "";


            switch (or)
            {
                case "Fornecedor":
                    sqlLista = "Select Fornecedor," +
                                   " CNPJ," +
                                   " Razao_social " +
                               " from fornecedor " +
                               " where (" +
                                    " fornecedor like '%" + TxtPesquisaLista.Text + "%'" +
                                    " or razao_social like '%" + TxtPesquisaLista.Text + "%'" +
                                    " OR REPLACE(replace(replace(cnpj,'.',''),'/',''),'-','')like '%" + TxtPesquisaLista.Text.Replace(".", "").Replace("/", "").Replace("-", "") + "%')";
                    lbllista.Text = "Escolha um FORNECEDOR";
                    break;
                case "imgPesquisaLoja":
                    sqlLista = "Select loja , filial from filial  where filial like '%" + TxtPesquisaLista.Text + "%'";

                    lbllista.Text = "Escolha um Loja";
                    break;
                case "imgPesquisaImpressora":
                    sqlLista = "Select Impressora= impressora_remota, Descricao from Spool_impressoras " +
                                               " where ativo = 1 and Descricao  like '%" + TxtPesquisaLista.Text + "%'";

                    lbllista.Text = "Escolha a Impressora";
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

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalPnFundo.Hide();
            String or = (String)Session["camporecebe" + urlSessao()];
            switch (or)
            {
                case "Fornecedor":
                    modalDepartamentoDetalhes.Show();
                    break;
                case "imgPesquisaLoja":
                case "imgPesquisaImpressora":
                    modalDepartamentoDetalhes.Show();
                    modalAddImpressora.Show();
                    break;
            }

        }

        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                String sql = "Delete from Fornecedor_departamento " +
                               " where fornecedor='" + lblCodFornecExcluir.Text.Trim() + "'" +
                               " and codigo_departamento='" + txtCodigoDepartamentoDetalhe.Text.Trim() + "'";
                Conexao.executarSql(sql);
                carregarFornecedores();
                modalDepartamentoDetalhes.Show();
            }
            catch (Exception err)
            {

                lblError.Text = "Não foi possivel Excluir o registro pelo error:" + err.Message;


            }
        }
        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            modalPnConfirma.Hide();
            modalDepartamentoDetalhes.Show();
        }


        protected void btnConfirmaExclusaoDepartamento_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                DepartamentoDAO centro = new DepartamentoDAO();
                centro.Codigo_departamento = DepartamentoSelecionado(1);
                centro.excluir();
                CarregarDepartamentoPesquisa(true);
                modalConfirmaExcluirDepartamento.Hide();
            }
            catch (Exception err)
            {

                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;

            }
        }
        protected void btnCancelaExclusaoDepartamento_Click(object sender, ImageClickEventArgs e)
        {
            modalConfirmaExcluirDepartamento.Hide();
        }


        protected void imgBtAddImpressora_Click(object sender, ImageClickEventArgs e)
        {
            txtImpressora.Text = "";
            txtDescImpressora.Text = "";
            txtLoja.Text = "";
            txtDescLoja.Text = "";
            modalDepartamentoDetalhes.Show();
            modalAddImpressora.Show();
        }



        protected void gridImpressoras_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow rw = gridImpressoras.Rows[index];

            lblConfirmaImpressoraExcluir.Text = rw.Cells[1].Text + "-" + rw.Cells[3].Text;
            modalDepartamentoDetalhes.Show();
            modalConfirmaExcluirImpressora.Show();

        }


        protected void imgBtnConfirmaAddImpressora_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                int exist = 0;

                txtLoja.BackColor = System.Drawing.Color.White;
                lblErrorImpressora.Text = "";
                if (txtLoja.Text.Equals(""))
                {
                    txtLoja.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Preencha a Loja");
                }
                else
                {
                    int.TryParse(Conexao.retornaUmValor("Select count (*) from filial where loja=" + txtLoja.Text, null), out exist);
                    if (exist <= 0)
                    {
                        txtLoja.BackColor = System.Drawing.Color.Red;
                        throw new Exception("Loja não Existe");
                    }

                }
                txtImpressora.BackColor = System.Drawing.Color.White;
                if (txtImpressora.Text.Equals(""))
                {
                    txtImpressora.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Preencha a Impressora");
                }
                else
                {
                    int.TryParse(Conexao.retornaUmValor("Select count (*) from Spool_impressoras where impressora_remota=" + txtImpressora.Text, null), out exist);
                    if (exist <= 0)
                    {
                        txtImpressora.BackColor = System.Drawing.Color.Red;
                        throw new Exception("Impressora não Existe");
                    }
                }

                String strDepartamento = txtCodigoDepartamentoDetalhe.Text;

                int.TryParse(Conexao.retornaUmValor("Select count (*) from Spool_loja_impressoras where loja=" + txtLoja.Text + " and codigo_departamento = '" + strDepartamento + "' and impressora_remota =" + txtImpressora.Text, null), out exist);
                if (exist == 0)
                {
                    String strSql = "insert into Spool_loja_impressoras (loja,codigo_departamento, impressora_remota)" +
                                    " values(" + txtLoja.Text + ",'" + strDepartamento + "'," + txtImpressora.Text + ")";

                    Conexao.executarSql(strSql);
                }
                else
                {
                    throw new Exception("Impressora já incluida");
                }


                carregarImpressoras();
                modalDepartamentoDetalhes.Show();

            }
            catch (Exception err)
            {
                lblErrorImpressora.Text = err.Message;
                lblErrorImpressora.ForeColor = System.Drawing.Color.Red;
                modalDepartamentoDetalhes.Show();
                modalAddImpressora.Show();
            }
        }

        protected void imgBtnCancelaAddImpressora_Click(object sender, ImageClickEventArgs e)
        {
            modalDepartamentoDetalhes.Show();
        }

        protected void imgPesquisaImpressora_Click(Object sender, ImageClickEventArgs e)
        {
            ImageButton img = (ImageButton)sender;
            Session.Remove("camporecebe" + urlSessao());
            Session.Add("camporecebe" + urlSessao(), img.ID);

            exibeLista();

        }

        protected void txtLoja_TextChange(Object sender, EventArgs e)
        {
            if (!txtLoja.Text.Trim().Equals(""))
            {
                txtDescLoja.Text = Conexao.retornaUmValor("Select Filial from filial where loja =" + txtLoja.Text + "", null);
            }
            else
            {
                txtDescLoja.Text = "";
            }
            modalDepartamentoDetalhes.Show();
            modalAddImpressora.Show();
        }

        protected void txtImpressora_TextChange(Object sender, EventArgs e)
        {
            if (!txtImpressora.Text.Trim().Equals(""))
            {
                txtDescImpressora.Text = Conexao.retornaUmValor("Select Descricao from Spool_impressoras where impressora_remota =" + txtImpressora.Text + "", null);
            }
            else
            {
                txtDescImpressora.Text = "";
            }
            modalDepartamentoDetalhes.Show();
            modalAddImpressora.Show();

        }
        protected void imgBtnConfirmaExclusaoImpressora_Click(Object sender, EventArgs e)
        {
            String strDepartamento = txtCodigoDepartamentoDetalhe.Text;
            String strloja = lblConfirmaImpressoraExcluir.Text.Substring(0, lblConfirmaImpressoraExcluir.Text.IndexOf("-"));
            String strImp = lblConfirmaImpressoraExcluir.Text.Substring(lblConfirmaImpressoraExcluir.Text.IndexOf("-") + 1);

            String sql = "Delete from Spool_Loja_Impressoras where loja=" + strloja + " and codigo_departamento = '" + strDepartamento + "' and impressora_remota =" + strImp;

            Conexao.executarSql(sql);
            carregarImpressoras();
            modalDepartamentoDetalhes.Show();


        }
        protected void imgBtnCancelaExclusaoImpressora_Click(Object sender, EventArgs e)
        {
            modalDepartamentoDetalhes.Show();
        }

        protected void ImgBtnCategoriaPesquisa_Click(object sender, ImageClickEventArgs e)
        {
            carregarCategorias(true);
        }

        protected void ImgBtnCategoriaEditar_Click(object sender, ImageClickEventArgs e)
        {
            txtCodigoGrupoCategoriaDetalhes.Text = GrupoSelecionado(1);
            txtDescricaoGrupoCategoriaDetalhes.Text = GrupoSelecionado(2);
            txtCodSubGrupoCategoriaDetalhes.Text = SubGrupoSelecionado(1);
            txtDescricaoSubGrupoCategoriaDetalhes.Text = SubGrupoSelecionado(2);
            txtCodDepartamentoCategoriaDetalhes.Text = DepartamentoSelecionado(1);
            txtDescricaoDepartamentoCategoriaDetalhes.Text = DepartamentoSelecionado(2);
            txtCodigoCategoriaDetalhes.Text = CategoriaSelecionado(1);
            txtDescricaoCategoriaDetalhes.Text = CategoriaSelecionado(2);
            lblIncluirCategoriaDetalhe.Text = "Editar";
            modalCategoriaDetalhes.Show();
        }

        protected void ImgBtnCategoriaAdd_Click(object sender, ImageClickEventArgs e)
        {
            txtCodigoGrupoCategoriaDetalhes.Text = GrupoSelecionado(1);
            txtDescricaoGrupoCategoriaDetalhes.Text = GrupoSelecionado(2);
            txtCodSubGrupoCategoriaDetalhes.Text = SubGrupoSelecionado(1);
            txtDescricaoSubGrupoCategoriaDetalhes.Text = SubGrupoSelecionado(2);
            txtCodDepartamentoCategoriaDetalhes.Text = DepartamentoSelecionado(1);
            txtDescricaoDepartamentoCategoriaDetalhes.Text = DepartamentoSelecionado(2);
            if (txtCodDepartamentoCategoriaDetalhes.Text.Contains("--"))
            {
                showMessage("Não é possivel incluir sem um Departamento;", true);
            }
            else
            {

                String cod = Conexao.retornaUmValor("Select max(codigo) from categorias where codigo_departamento = '" + txtCodDepartamentoCategoriaDetalhes.Text + "'", null);
                int proxCod = proximaSeguencia(cod, 1);
                txtCodigoCategoriaDetalhes.Text = txtCodDepartamentoCategoriaDetalhes.Text + "." + proxCod.ToString();
                txtDescricaoCategoriaDetalhes.Text = "";

                lblIncluirCategoriaDetalhe.Text = "Incluir";


                txtDescricaoCategoriaDetalhes.Focus();
                modalCategoriaDetalhes.Show();
            }

        }

        protected void gridCategorias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoCategoriaItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('gridCategorias.*GrCategoria',this)";
            rdo.Attributes.Add("onclick", script);
        }

        protected void RdoCategoriaItem_CheckedChanged(object sender, EventArgs e)
        {
            carregarSeguimento(true);
        }

        protected void ImgBtnSeguimentoPesquisa_Click(object sender, ImageClickEventArgs e)
        {
            carregarSeguimento(true);
        }

        protected void ImgBtnSeguimentoEditar_Click(object sender, ImageClickEventArgs e)
        {
            txtcodGrupoSeguimentoDetalhes.Text = GrupoSelecionado(1);
            txtDescGrupoSeguimentoDetalhes.Text = GrupoSelecionado(2);
            txtCodSubGrupoSeguimentoDetalhes.Text = SubGrupoSelecionado(1);
            txtDescSubGrupoSeguimentoDetalhes.Text = SubGrupoSelecionado(2);
            txtCodDepartamentoSeguimentoDetalhes.Text = DepartamentoSelecionado(1);
            txtDescDepartamentoSeguimentoDetalhes.Text = DepartamentoSelecionado(2);
            txtCodCategoriaSeguimentoDetalhes.Text = CategoriaSelecionado(1);
            txtDescCategoriaSeguimentoDetalhes.Text = CategoriaSelecionado(2);
            txtCodSeguimentoDetalhes.Text = SeguimentoSelecionado(1);
            txtDescSeguimentoDetalhes.Text = SeguimentoSelecionado(2);
            lblIncluirSeguimentoDetalhe.Text = "Editar";
            modalSeguiemntoDetalhes.Show();
        }

        protected void ImgBtnSeguimentoAdd_Click(object sender, ImageClickEventArgs e)
        {
            txtcodGrupoSeguimentoDetalhes.Text = GrupoSelecionado(1);
            txtDescGrupoSeguimentoDetalhes.Text = GrupoSelecionado(2);
            txtCodSubGrupoSeguimentoDetalhes.Text = SubGrupoSelecionado(1);
            txtDescSubGrupoSeguimentoDetalhes.Text = SubGrupoSelecionado(2);
            txtCodDepartamentoSeguimentoDetalhes.Text = DepartamentoSelecionado(1);
            txtDescDepartamentoSeguimentoDetalhes.Text = DepartamentoSelecionado(2);
            txtCodCategoriaSeguimentoDetalhes.Text = CategoriaSelecionado(1);
            txtDescCategoriaSeguimentoDetalhes.Text = CategoriaSelecionado(2);

            if (txtCodCategoriaSeguimentoDetalhes.Text.Contains("--"))
            {
                showMessage("Não é possivel incluir sem uma Categoria!", true);
            }
            else
            {
                String cod = Conexao.retornaUmValor("Select max(codigo) from categorias where codigo_departamento = '" + txtCodCategoriaSeguimentoDetalhes.Text + "'", null);
                int proxCod = proximaSeguencia(cod, 2);
                txtCodSeguimentoDetalhes.Text = txtCodCategoriaSeguimentoDetalhes.Text + "." + proxCod.ToString();
                txtDescSeguimentoDetalhes.Text = "";
                txtDescSeguimentoDetalhes.Focus();
                lblIncluirSeguimentoDetalhe.Text = "Incluir";
                modalSeguiemntoDetalhes.Show();
            }
        }

        protected void gridSeguimento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoSeguimentoItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('gridSeguimento.*GrSeguimento',this)";
            rdo.Attributes.Add("onclick", script);
        }

        protected void RdoSeguimentoItem_CheckedChanged(object sender, EventArgs e)
        {
            carregarSubSeguimento(true);
        }

        protected void ImgBtnSubseguimentoPesquisa_Click(object sender, ImageClickEventArgs e)
        {
            carregarSubSeguimento(true);
        }

        protected void ImgBtnSubseguimentoEditar_Click(object sender, ImageClickEventArgs e)
        {

            txtcodGrupoSubSeguimentoDetalhes.Text = GrupoSelecionado(1);
            txtDescGrupoSubSeguimentoDetalhes.Text = GrupoSelecionado(2);
            txtCodSubGrupoSubSeguimentoDetalhes.Text = SubGrupoSelecionado(1);
            txtDescSubGrupoSubSeguimentoDetalhes.Text = SubGrupoSelecionado(2);
            txtCodDepartamentoSubSeguimentoDetalhes.Text = DepartamentoSelecionado(1);
            txtDescDepartamentoSubSeguimentoDetalhes.Text = DepartamentoSelecionado(2);
            txtCodCategoriaSubSeguimentoDetalhes.Text = CategoriaSelecionado(1);
            txtDescCategoriaSubSeguimentoDetalhes.Text = CategoriaSelecionado(2);
            txtCodSeguimentoSubSeguimentoDetalhes.Text = SeguimentoSelecionado(1);
            txtDescSeguimentoSubSeguimentoDetalhes.Text = SeguimentoSelecionado(2);
            txtCodSubSeguimentoDetalhes.Text = SubSeguimentoSelecionado(1);
            txtDescSubSeguimentoDetalhes.Text = SubSeguimentoSelecionado(2);
            lblIncluirSubSeguimento.Text = "Editar";
            modalSubSeguimentoDetalhes.Show();
        }

        protected void ImgBtnSubseguimentoAdd_Click(object sender, ImageClickEventArgs e)
        {
            txtcodGrupoSubSeguimentoDetalhes.Text = GrupoSelecionado(1);
            txtDescGrupoSubSeguimentoDetalhes.Text = GrupoSelecionado(2);
            txtCodSubGrupoSubSeguimentoDetalhes.Text = SubGrupoSelecionado(1);
            txtDescSubGrupoSubSeguimentoDetalhes.Text = SubGrupoSelecionado(2);
            txtCodDepartamentoSubSeguimentoDetalhes.Text = DepartamentoSelecionado(1);
            txtDescDepartamentoSubSeguimentoDetalhes.Text = DepartamentoSelecionado(2);
            txtCodCategoriaSubSeguimentoDetalhes.Text = CategoriaSelecionado(1);
            txtDescCategoriaSubSeguimentoDetalhes.Text = CategoriaSelecionado(2);
            txtCodSeguimentoSubSeguimentoDetalhes.Text = SeguimentoSelecionado(1);
            txtDescSeguimentoSubSeguimentoDetalhes.Text = SeguimentoSelecionado(2);
            if (txtCodSeguimentoSubSeguimentoDetalhes.Text.Contains("--"))
                showMessage("Não é possivel incluir sem um Seguimento", true);
            else
            {
                String cod = Conexao.retornaUmValor("Select max(codigo) from categorias where codigo_departamento = '" + txtCodSeguimentoSubSeguimentoDetalhes.Text + "'", null);

                int proxCod = proximaSeguencia(cod, 3);
                txtCodSubSeguimentoDetalhes.Text = txtCodSeguimentoSubSeguimentoDetalhes.Text + "." + proxCod.ToString();
                txtDescSubSeguimentoDetalhes.Text = "";
                txtDescSubSeguimentoDetalhes.Focus();
                lblIncluirSubSeguimento.Text = "Incluir";
                modalSubSeguimentoDetalhes.Show();
            }
        }

        protected int proximaSeguencia(String nCod, int nPontos)
        {
            for (int i = 0; i < nPontos; i++)
            {
                nCod = nCod.Substring(nCod.IndexOf('.') + 1);
            }

            return Funcoes.intTry(nCod) + 1;

        }
        protected void gridSubSeguimento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoSubSeguimentoItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('gridSubSeguimento.*GrSubSeguimento',this)";
            rdo.Attributes.Add("onclick", script);
        }

        protected void ImgBtnCategoriaGrupoPesquisa_Click(object sender, ImageClickEventArgs e)
        {
            carregarCategoriaGrupo(true);
        }

        protected void ImgBtnCategoriaGrupoEditar_Click(object sender, ImageClickEventArgs e)
        {
            txtcodGrupoGrupoCategoriatoDetalhes.Text = GrupoSelecionado(1);
            txtDescGrupoGrupoCategoriaDetalhes.Text = GrupoSelecionado(2);
            txtCodSubGrupoGrupoCategoriaDetalhes.Text = SubGrupoSelecionado(1);
            txtDescSubGrupoGrupoCategoriaDetalhes.Text = SubGrupoSelecionado(2);
            txtCodDepartamentoGrupoCategoriaDetalhes.Text = DepartamentoSelecionado(1);
            txtDescDepartamentoGrupoCategoriaDetalhes.Text = DepartamentoSelecionado(2);
            txtCodCategoriaGrupoCategoriaDetalhes.Text = CategoriaSelecionado(1);
            txtDescCategoriaGrupoCategoriaDetalhes.Text = CategoriaSelecionado(2);
            txtCodSeguimentoGrupoCategoriaDetalhes.Text = SeguimentoSelecionado(1);
            txtDescSeguimentoGrupoCategoriaDetalhes.Text = SeguimentoSelecionado(2);
            txtCodSubSeguimentoGrupoCategoriaDetalhes.Text = SubSeguimentoSelecionado(1);
            txtDescSubSeguimentoGrupoCategoriaDetalhes.Text = SubSeguimentoSelecionado(2);
            txtCodGrupoCategoriaDetalhes.Text = CategoriaGrupoSelecionado(1);
            txtDescGrupoCategoriaDetalhes.Text = CategoriaGrupoSelecionado(2);
            lblIncluirGrupoCategoria.Text = "Editar";
            modalGrupoCategoriaDetalhes.Show();
        }



        protected void ImgBtnCategoriaGrupoAdd_Click(object sender, ImageClickEventArgs e)
        {
            txtcodGrupoGrupoCategoriatoDetalhes.Text = GrupoSelecionado(1);
            txtDescGrupoGrupoCategoriaDetalhes.Text = GrupoSelecionado(2);
            txtCodSubGrupoGrupoCategoriaDetalhes.Text = SubGrupoSelecionado(1);
            txtDescSubGrupoGrupoCategoriaDetalhes.Text = SubGrupoSelecionado(2);
            txtCodDepartamentoGrupoCategoriaDetalhes.Text = DepartamentoSelecionado(1);
            txtDescDepartamentoGrupoCategoriaDetalhes.Text = DepartamentoSelecionado(2);
            txtCodCategoriaGrupoCategoriaDetalhes.Text = CategoriaSelecionado(1);
            txtDescCategoriaGrupoCategoriaDetalhes.Text = CategoriaSelecionado(2);
            txtCodSeguimentoGrupoCategoriaDetalhes.Text = SeguimentoSelecionado(1);
            txtDescSeguimentoGrupoCategoriaDetalhes.Text = SeguimentoSelecionado(2);
            txtCodSubSeguimentoGrupoCategoriaDetalhes.Text = SubSeguimentoSelecionado(1);
            txtDescSubSeguimentoGrupoCategoriaDetalhes.Text = SubSeguimentoSelecionado(2);

            if (txtCodSubSeguimentoGrupoCategoriaDetalhes.Text.Contains("--"))
            {
                showMessage("Não é possivel incluir sem Sub Seguimento", true);
            }
            else
            {


                String cod = Conexao.retornaUmValor("Select max(codigo) from categorias where codigo_departamento = '" + txtCodSubSeguimentoGrupoCategoriaDetalhes.Text + "'", null);
                int proxCod = proximaSeguencia(cod, 4);

                txtCodGrupoCategoriaDetalhes.Text = txtCodSubSeguimentoGrupoCategoriaDetalhes.Text + "." + proxCod.ToString();
                txtDescGrupoCategoriaDetalhes.Text = "";
                txtDescGrupoCategoriaDetalhes.Focus();
                lblIncluirGrupoCategoria.Text = "Incluir";
                modalGrupoCategoriaDetalhes.Show();
            }
        }

        protected void gridCategoriasGrupo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoCategoriaGrupoItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('gridCategoriasGrupo.*GrCategoriaGrupo',this)";
            rdo.Attributes.Add("onclick", script);
        }

        protected void ImgBtnCategoriaSubGrupoPesquisa_Click(object sender, ImageClickEventArgs e)
        {
            carregarCategoriaSubGrupo(true);
        }

        protected void ImgBtnCategoriaSubGrupoEditar_Click(object sender, ImageClickEventArgs e)
        {
            txtcodGrupoSubGrupoCategoriatoDetalhes.Text = GrupoSelecionado(1);
            txtDescGrupoSubGrupoCategoriaDetalhes.Text = GrupoSelecionado(2);
            txtCodSubGrupoSubGrupoCategoriaDetalhes.Text = SubGrupoSelecionado(1);
            txtDescSubGrupoSubGrupoCategoriaDetalhes.Text = SubGrupoSelecionado(2);
            txtCodDepartamentoSubGrupoCategoriaDetalhes.Text = DepartamentoSelecionado(1);
            txtDescDepartamentoSubGrupoCategoriaDetalhes.Text = DepartamentoSelecionado(2);
            txtCodCategoriaSubGrupoCategoriaDetalhes.Text = CategoriaSelecionado(1);
            txtDescCategoriaSubGrupoCategoriaDetalhes.Text = CategoriaSelecionado(2);
            txtCodSeguimentoSubGrupoCategoriaDetalhes.Text = SeguimentoSelecionado(1);
            txtDescSeguimentoSubGrupoCategoriaDetalhes.Text = SeguimentoSelecionado(2);
            txtCodSubSeguimentoSubGrupoCategoriaDetalhes.Text = SubSeguimentoSelecionado(1);
            txtDescSubSeguimentoSubGrupoCategoriaDetalhes.Text = SubSeguimentoSelecionado(2);
            txtCodGrupoCategoriaSubGrupoCategoriaDetalhes.Text = CategoriaGrupoSelecionado(1);
            txtDescGrupoCategoriaSubGrupoCategoriaDetalhes.Text = CategoriaGrupoSelecionado(2);
            txtCodSubGrupoCategoria.Text = CategoriaSubGrupoSelecionado(1);
            txtDescSubGrupoCategoria.Text = CategoriaSubGrupoSelecionado(2);
            lblIncluirSubGrupoCategoria.Text = "Editar";
            modalSubGrupoCategoriaDetalhes.Show();
        }

        protected void ImgBtnCategoriaSubGrupoExcluir_Click(object sender, ImageClickEventArgs e)
        {
            excluirObjCategoria(CategoriaGrupoSelecionado(1), CategoriaSubGrupoSelecionado(1));
            carregarCategoriaSubGrupo(true);
        }

        protected void ImgBtnCategoriaSubGrupoAdd_Click(object sender, ImageClickEventArgs e)
        {
            txtcodGrupoSubGrupoCategoriatoDetalhes.Text = GrupoSelecionado(1);
            txtDescGrupoSubGrupoCategoriaDetalhes.Text = GrupoSelecionado(2);
            txtCodSubGrupoSubGrupoCategoriaDetalhes.Text = SubGrupoSelecionado(1);
            txtDescSubGrupoSubGrupoCategoriaDetalhes.Text = SubGrupoSelecionado(2);
            txtCodDepartamentoSubGrupoCategoriaDetalhes.Text = DepartamentoSelecionado(1);
            txtDescDepartamentoSubGrupoCategoriaDetalhes.Text = DepartamentoSelecionado(2);
            txtCodCategoriaSubGrupoCategoriaDetalhes.Text = CategoriaSelecionado(1);
            txtDescCategoriaSubGrupoCategoriaDetalhes.Text = CategoriaSelecionado(2);
            txtCodSeguimentoSubGrupoCategoriaDetalhes.Text = SeguimentoSelecionado(1);
            txtDescSeguimentoSubGrupoCategoriaDetalhes.Text = SeguimentoSelecionado(2);
            txtCodSubSeguimentoSubGrupoCategoriaDetalhes.Text = SubSeguimentoSelecionado(1);
            txtDescSubSeguimentoSubGrupoCategoriaDetalhes.Text = SubSeguimentoSelecionado(2);
            txtCodGrupoCategoriaSubGrupoCategoriaDetalhes.Text = CategoriaGrupoSelecionado(1);
            txtDescGrupoCategoriaSubGrupoCategoriaDetalhes.Text = CategoriaGrupoSelecionado(2);

            if (txtCodGrupoCategoriaSubGrupoCategoriaDetalhes.Text.Contains("--"))
            {
                showMessage("Não é possivel Incluir sem Grupo !", true);
            }
            else
            {
                String cod = Conexao.retornaUmValor("Select max(codigo) from categorias where codigo_departamento = '" + txtCodGrupoCategoriaSubGrupoCategoriaDetalhes.Text + "'", null);
                int proxCod = proximaSeguencia(cod, 5);

                txtCodSubGrupoCategoria.Text = txtCodGrupoCategoriaSubGrupoCategoriaDetalhes.Text + "." + proxCod.ToString();
                txtDescSubGrupoCategoria.Text = "";
                txtDescSubGrupoCategoria.Focus();
                lblIncluirSubGrupoCategoria.Text = "Incluir";
                modalSubGrupoCategoriaDetalhes.Show();
            }
        }

        protected void ImgBtnSeguimentoExcluir_Click(object sender, ImageClickEventArgs e)
        {

            excluirObjCategoria(CategoriaSelecionado(1), SeguimentoSelecionado(1));
        }

        private void excluirObjCategoria(String codPai, String codCat)
        {
            try
            {
                CategoriaDAO cat = new CategoriaDAO();
                cat.codigo = codCat;
                cat.codigo_departamento = codPai;
                cat.delete();
            }
            catch (Exception err)
            {

                showMessage(err.Message, true);
            }
        }

        private void salvarObjCategoria(String codPai, String codigo, String descricao, bool novo)
        {
            try
            {

                CategoriaDAO cat = new CategoriaDAO();
                cat.codigo = codigo;
                cat.codigo_departamento = codPai;
                cat.descricao = descricao;
                cat.salvar(novo);
            }
            catch (Exception err)
            {

                showMessage(err.Message, true);
            }
        }
        protected void ImgBtnSubseguimentoExcluir_Click(object sender, ImageClickEventArgs e)
        {
            excluirObjCategoria(SeguimentoSelecionado(1), SubSeguimentoSelecionado(1));
        }

        protected void ImgBtnCategoriaGrupoExcluir_Click1(object sender, ImageClickEventArgs e)
        {
            excluirObjCategoria(SubSeguimentoSelecionado(1), CategoriaGrupoSelecionado(1));

        }

        protected void RdoSubSeguimentoItem_CheckedChanged(object sender, EventArgs e)
        {
            carregarCategoriaGrupo(true);
        }

        protected void RdoCategoriaSubGrupoItem_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void gridCategoriasSubGrupo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoCategoriaSubGrupoItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('gridCategoriasSubGrupo.*GrCategoriaSubGrupo',this)";
            rdo.Attributes.Add("onclick", script);
        }

        protected void ImgBtnConfirmaSeguimentoDetalhes_Click(object sender, ImageClickEventArgs e)
        {
            btnconfirmar(CategoriaSelecionado(1)
                , txtDescSeguimentoDetalhes
                , txtCodSeguimentoDetalhes
                , "selectSeguimento"
                , modalSubSeguimentoDetalhes
                , lblErrorSeguimentoDetalhes
                , lblIncluirSeguimentoDetalhe.Text.Equals("Incluir"));
            carregarSeguimento(false);

        }


        protected void ImgBtnCancelaSeguimentoDetalhes_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void ImgBtnConfirmaCategoriaDetalhes_Click(object sender, ImageClickEventArgs e)
        {

            btnconfirmar(DepartamentoSelecionado(1)
                , txtDescricaoCategoriaDetalhes
                , txtCodigoCategoriaDetalhes
                , "selectCategoria"
                , modalCategoriaDetalhes
                , lblErrorCategoriaDetalhes
                , lblIncluirCategoriaDetalhe.Text.Equals("Incluir"));

            carregarCategorias(false);

        }

        protected void ImgBtnCancelaCategoriaDetalhes_Click(object sender, ImageClickEventArgs e)
        {
            modalCategoriaDetalhes.Hide();
        }

        protected void ImgBtnCancelaSeguimentoDetalhes_Click1(object sender, ImageClickEventArgs e)
        {
            modalSeguiemntoDetalhes.Hide();
        }

        protected void ImgBtnConfirmaSubSeguimentoDetalhes_Click(object sender, ImageClickEventArgs e)
        {
            btnconfirmar(SeguimentoSelecionado(1)
                , txtDescSubSeguimentoDetalhes
                , txtCodSubSeguimentoDetalhes
                , "selectSubSeguimento"
                , modalSubSeguimentoDetalhes
                , lblErrorSubSeguimentoDetalhes
                , lblIncluirSubSeguimento.Text.Equals("Incluir"));
            carregarSubSeguimento(false);
        }

        protected void ImgBtnCancelaSubSeguimentoDetalhes_Click(object sender, ImageClickEventArgs e)
        {
            modalSubSeguimentoDetalhes.Hide();
        }

        protected void btnconfirmar(String codPai, TextBox txtDescricao, TextBox txtcodigo, String Select, ModalPopupExtender modal, Label lblErro, bool incluir)
        {
            if (txtDescricao.Text.Trim().Equals("") || txtcodigo.Text.Trim().Equals(""))
            {
                lblErro.Text = "OS CAMPOS NÃO PODEM ESTAR EM BRANCO!";
                modal.Show();
            }
            else
            {
                try
                {
                    salvarObjCategoria(codPai
                        , txtcodigo.Text
                        , txtDescricao.Text
                        , incluir
                        );

                    Session.Remove(Select);
                    Session.Add(Select, txtcodigo.Text);



                }
                catch (Exception err)
                {

                    showMessage(err.Message, true);
                }

            }
        }

        protected void ImgBtnConfirmaGrupoCategoriaDetalhes_Click(object sender, ImageClickEventArgs e)
        {
            btnconfirmar(SubSeguimentoSelecionado(1)
                    , txtDescGrupoCategoriaDetalhes
                    , txtCodGrupoCategoriaDetalhes
                    , "selectCategoriaGrupo"
                    , modalGrupoCategoriaDetalhes
                    , lblErroGrupoCategoria
                    , lblIncluirGrupoCategoria.Text.Equals("Incluir"));
            carregarCategoriaGrupo(false);
        }

        protected void ImgBtnCancelaGrupoCategoriaDetalhes_Click(object sender, ImageClickEventArgs e)
        {
            modalGrupoCategoriaDetalhes.Hide();
        }

        protected void ImgBtnConfirmaSubGrupoCategoriaDetalhes_Click(object sender, ImageClickEventArgs e)
        {
            btnconfirmar(CategoriaGrupoSelecionado(1)
                , txtDescSubGrupoCategoria
                , txtCodSubGrupoCategoria
                , "selectCategoriaSubGrupo"
                , modalSubGrupoCategoriaDetalhes
                , lblErrorSubGrupoCategoria
                , lblIncluirSubGrupoCategoria.Text.Equals("Incluir"));

            carregarCategoriaSubGrupo(false);
        }

        protected void ImgBtnCancelaSubGrupoCategoriaDetalhes_Click(object sender, ImageClickEventArgs e)
        {
            modalSubGrupoCategoriaDetalhes.Hide();
        }


        protected void btnOkError_Click(object sender, EventArgs e)
        {



            modalError.Hide();


        }

        protected void showMessage(String mensagem, bool erro)
        {
            lblErroPanel.Text = mensagem;
            if (erro)
            {
                lblErroPanel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblErroPanel.ForeColor = System.Drawing.Color.Blue;
            }
            modalError.Show();
        }

        protected void ImgBtnCategoriaExcluir_Click(object sender, ImageClickEventArgs e)
        {
            excluirObjCategoria(DepartamentoSelecionado(1), CategoriaSelecionado(1));
        }

        protected void RdoCategoriaGrupoItem_CheckedChanged(object sender, EventArgs e)
        {
            carregarCategoriaGrupo(false);
        }
    }
}