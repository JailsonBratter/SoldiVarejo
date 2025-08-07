using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;
using visualSysWeb.modulos.Cadastro.code;
using visualSysWeb.modulos.Cadastro.dao;
using System.Data.SqlClient;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class Cardapio : PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                String strID = Request.Params["ID"];

                int ID = (strID != null ? Funcoes.intTry(Request.Params["ID"].ToString()) : 0);

                User usr = (User)Session["User"];
                if (usr == null)
                    return;

                CardapioDAO cardapio = (CardapioDAO)Session["obj" + urlSessao()];
                if (!IsPostBack)
                {
                    cardapio = new CardapioDAO(usr.getFilial(), usr, ID);
                    divSemCardapio.Visible = !cardapio.Existe;
                    divConteudo.Visible = cardapio.Existe;
                    Session.Remove("obj" + urlSessao());
                    Session.Add("obj" + urlSessao(), cardapio);

                    status = "visualizar";
                    carregarDados();
                    habilitar(false);
                }

                if (cardapio != null && cardapio.Existe)
                    carregabtn(pnBtn, "NAO", null, null, null, "NAO", "NAO");

                

                
            }
            catch (Exception err)
            {

                showMessage(err.Message, true);
            }

        }

        private void habilitar(bool v)
        {
            divIncluirNovaCategoria.Visible = v;
            divIncluirProdutos.Visible = v;
            divIncluirObs.Visible = v;
            divBotoesObservacoes.Visible = v;
            divFecharObs.Visible = !v;
            divCamposAddObs.Visible = v;



            EnabledControls(divConteudo, v);
            EnabledControls(gridCategoria, true);
            EnabledButtons(gridCategoria, v);
            EnabledButtons(gridObservacoesProduto, v);
            ImgBtnEnviarAPI.Visible = !v;
            divAtualizarApi.Visible = !v;
            btnImgToken.Visible = true;
        }

        private void carregarDados()
        {
            CardapioDAO cardapio = (CardapioDAO)Session["obj" + urlSessao()];
            txtFilial.Text = cardapio.Filial;
            txtDtCadastro.Text = Funcoes.dataBr(cardapio.DtCadastro);
            TxtDtAlteracao.Text = Funcoes.dataBr(cardapio.DtUltAlteracao);
            txtUsuarioCadastro.Text = cardapio.UsuarioCadastro;
            txtUsuarioUltAlteracao.Text = cardapio.UsuarioUltAlteracao;
            txtUrlPadrao.Text = cardapio.UrlPadrao;
            txtToken.Text = cardapio.Token;
            CarregarGrids();
        }

        private void CarregarGrids()
        {
            CardapioDAO cardapio = (CardapioDAO)Session["obj" + urlSessao()];

            if (cardapio.Categorias.Count > 0)
            {
                divSemCategorias.Visible = false;
                gridCategoria.DataSource = cardapio.Categorias;
                gridCategoria.DataBind();
                if (!IsPostBack)
                {
                    RadioButton rdo = (RadioButton)gridCategoria.Rows[0].FindControl("rdoCategoria");
                    rdo.Checked = true;

                }
                else
                {
                    SelecionarCategoria(Funcoes.intTry(txtIdCategoria.Text));
                }

                categoriaSelecionada();

            }
            else
            {
                divSemCategorias.Visible = true;
            }


        }
        private void carregardadosObj()
        {
            AtualizarCategoria();
            User usr = (User)Session["User"];
            CardapioDAO cardapio = (CardapioDAO)Session["obj" + urlSessao()];
            txtFilial.Text = cardapio.Filial;
            cardapio.DtUltAlteracao = DateTime.Now;
            cardapio.UsuarioUltAlteracao = usr.getUsuario();
            cardapio.UrlPadrao = txtUrlPadrao.Text;
            cardapio.Token = txtToken.Text;

        }

        protected void ddlPizza_SelectedIndexChanged(object sender, EventArgs e)
        {
            PizzaChange();
            AtualizarCategoria();
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                CardapioDAO cardapio = (CardapioDAO)Session["obj" + urlSessao()];
                cardapio.Insert();
                Response.Redirect("Cardapio.aspx");
            }
            catch (Exception err)
            {
                showMessage(err.Message, true);
            }
        }

        private void showMessage(string message, bool erro)
        {
            lblErroPanel.Text = message;
            if (erro)
                lblErroPanel.ForeColor = System.Drawing.Color.Red;
            else
                lblErroPanel.ForeColor = System.Drawing.Color.Blue;

            modalError.Show();
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            status = "editar";

            habilitar(true);
            carregabtn(pnBtn, "NAO", null, null, null, "NAO", "NAO");
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
            try
            {
                carregardadosObj();
                CardapioDAO cardapio = (CardapioDAO)Session["obj" + urlSessao()];
                cardapio.salvar();
                status = "visualizar";
                carregabtn(pnBtn, "NAO", null, null, null, "NAO", "NAO");
                habilitar(false);

                showMessage("Salvo Com sucesso!", false);
            }
            catch (Exception err)
            {
                showMessage(err.Message, true);

            }
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Cardapio.aspx?tela=C034");

        }

        protected override bool campoDesabilitado(Control campo)
        {
            if (campo == null || campo.ID == null)
                return false;

            switch (campo.ID)
            {
                case "txtFilial":
                case "txtUsuarioCadastro":
                case "txtUsuarioUltAlteracao":
                case "TxtDtAlteracao":
                case "txtDtCadastro":
                case "txtIdCategoria":
                case "txtAddObsPlu":
                case "txtAddObsPreco":
                    return true;
                default:
                    return false;
            }
        }

        protected override bool campoObrigatorio(Control campo)
        {
            return false;
        }

        protected void btnOkError_Click(object sender, EventArgs e)
        {

            modalError.Hide();
            if (lblErroPanel.Text.Contains("Id de Categoria já cadastrado"))
            {
                txtIdNovaCategoria.ForeColor = System.Drawing.Color.Red;
                modalNovaCategoria.Show();
            }
            else if (lblErroPanel.Text.Contains("Observação"))
            {
                modalIncluirObs.Show();
            }
            else if (lblErroPanel.Text.Contains("já foi incluido na Categoria"))
            {
                modalIncluirProdutos.Show();
            }
        }

        protected void ImgAddCategoria_Click(object sender, ImageClickEventArgs e)
        {

            EnabledControls(PnAddCategoria, true);
            LimparCampos(PnAddCategoria);
            txtIdNovaCategoria.Focus();
            modalNovaCategoria.Show();
        }

        protected void btnIncluirNovaCategoria_Click(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            CardapioDAO cardapio = (CardapioDAO)Session["obj" + urlSessao()];
            if (txtIdNovaCategoria.Text.Equals(""))
            {
                txtIdNovaCategoria.Text = (cardapio.Categorias.Count + 1).ToString();
            }
            int idCat = Funcoes.intTry(txtIdNovaCategoria.Text);
            if (cardapio.Categorias.Count(c => c.Id == idCat) == 0)
            {
                Cardapio_CategoriaDAO categoria = new Cardapio_CategoriaDAO(usr.getFilial(), cardapio.ID)
                {
                    Id = idCat,
                    Categoria = txtTituloNovaCategoria.Text.ToUpper(),
                    idCardapio = cardapio.ID,
                    Status = "ativa",
                    Pizza = Funcoes.intTry(DdlnovaCategoriaPizza.SelectedValue),
                    CategoriaMeia = Funcoes.intTry(txtNovaCategoriaMeia.Text),
                    CategoriaTerco = Funcoes.intTry(txtNovaCategoriaTerco.Text)


                };

                cardapio.Categorias.Add(categoria);
                CarregarGrids();

                modalNovaCategoria.Hide();

                SelecionarCategoria(idCat);
                categoriaSelecionada();
            }
            else
            {
                showMessage("Id de Categoria já cadastrado!", true);
            }



        }

        protected void btnCancelarNovaCategoria_Click(object sender, EventArgs e)
        {
            modalNovaCategoria.Hide();
        }

        protected void gridCategoria_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("rdoCategoria");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('gridCategoria.*GrCategoria',this)";
            rdo.Attributes.Add("onclick", script);
        }

        protected void rdoCategoria_CheckedChanged(object sender, EventArgs e)
        {
            categoriaSelecionada();
        }

        private void categoriaSelecionada()
        {
            if (status == "editar" && !txtIdCategoria.Text.Equals(""))
                AtualizarCategoria(false);

            CardapioDAO cardapio = (CardapioDAO)Session["obj" + urlSessao()];
            foreach (GridViewRow row in gridCategoria.Rows)
            {
                RadioButton rdo = (RadioButton)row.FindControl("rdoCategoria");
                if (rdo.Checked)
                {
                    Cardapio_CategoriaDAO cat = cardapio.Categorias[row.RowIndex];
                    txtCategoria.Text = cat.Categoria;
                    txtIdCategoria.Text = cat.Id.ToString();
                    ddlStatusCatgoria.SelectedValue = (cat.Status.Equals("") ? "ativa" : cat.Status);
                    ddlPizza.SelectedValue = cat.Pizza.ToString();
                    txtIdCategoriaMeia.Text = cat.CategoriaMeia.ToString();
                    txtIdCategoriaTerco.Text = cat.CategoriaTerco.ToString();
                    PizzaChange();
                    if (cat.Produtos.Count > 0)
                    {
                        divSemProdutos.Visible = false;
                        gridProdutos.DataSource = cat.Produtos;
                        gridProdutos.DataBind();
                    }
                    else
                    {
                        divSemProdutos.Visible = true;
                        gridProdutos.DataSource = null;
                        gridProdutos.DataBind();
                    }
                    EnabledControls(gridProdutos, status.Equals("editar"));
                    break;
                }
            }
        }

        protected void imgBtnPesquisa_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            Session.Remove("pesquisa" + urlSessao());
            Session.Add("pesquisa" + urlSessao(), btn.ID);
            exibirLista();
        }

        private void exibirLista()
        {
            string lista = (string)Session["pesquisa" + urlSessao()];

            CardapioDAO cardapio = (CardapioDAO)Session["obj" + urlSessao()];
            switch (lista)
            {
                case "imgBtnNovaCategoriaMeia":
                case "imgBtnNovaCategoriaTerco":
                case "imgBtnCategoriaMeia":
                case "imgBtnCategoriaTerco":
                    lbllista.Text = "Escolha a Categoria ";
                    GridLista.DataSource = cardapio.Categorias.FindAll(c => c.Status == "oculta");
                    break;
                case "ImgBtnAddObsPluAdd":
                    lbllista.Text = "Escolha o produto";
                    GridLista.DataSource = Conexao.GetTable("Select plu, descricao, preco from mercadoria " +
                        "where plu like '" + TxtPesquisaLista.Text.Trim() + "%' or descricao like '%" + TxtPesquisaLista.Text + "%'"
                        , null, true);
                    break;
            }
            GridLista.DataBind();
            modalPnFundo.Show();

        }

        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibirLista();
        }
        protected void btnFechar_Click(object sender, ImageClickEventArgs e)
        {
            string lista = (string)Session["pesquisa" + urlSessao()];

            modalPnFundo.Hide();
            switch (lista)
            {
                case "imgBtnNovaCategoriaMeia":
                    txtNovaCategoriaMeia.Text = ListaSelecionada(2);
                    modalNovaCategoria.Show();
                    break;
                case "imgBtnNovaCategoriaTerco":
                    txtNovaCategoriaTerco.Text = ListaSelecionada(2);
                    modalNovaCategoria.Show();
                    break;
                case "imgBtnCategoriaMeia":
                    txtIdCategoriaMeia.Text = ListaSelecionada(2);
                    break;
                case "imgBtnCategoriaTerco":
                    txtIdCategoriaTerco.Text = ListaSelecionada(2);
                    break;
                case "ImgBtnAddObsPluAdd":
                    txtAddObsPluAdd.Text = ListaSelecionada(1);
                    txtAddObsPreco.Text = ListaSelecionada(3);
                    modalIncluirObs.Show();
                    break;
            }
            AtualizarCategoria();

        }
        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalPnFundo.Hide();
            string lista = (string)Session["pesquisa" + urlSessao()];

            modalPnFundo.Hide();
            switch (lista)
            {
                case "imgBtnNovaCategoriaMeia":
                case "imgBtnNovaCategoriaTerco":
                    modalNovaCategoria.Show();
                    break;
                case "ImgBtnAddObsPluAdd":
                    modalIncluirObs.Show();
                    break;
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

        private void AtualizarCategoria(bool seleciona = true)
        {
            Cardapio_CategoriaDAO categoria = categoriaAtual();
            categoria.Categoria = txtCategoria.Text;
            categoria.Status = ddlStatusCatgoria.SelectedItem.Value;
            categoria.Pizza = Funcoes.intTry(ddlPizza.SelectedValue);
            categoria.CategoriaMeia = Funcoes.intTry(txtIdCategoriaMeia.Text);
            categoria.CategoriaTerco = Funcoes.intTry(txtIdCategoriaTerco.Text);
            PizzaChange();
            foreach (GridViewRow row in gridProdutos.Rows)
            {
                Cardapio_produtosDAO prod = categoria.Produtos.Find(p => p.Plu.Equals(row.Cells[0].Text));
                if (prod != null)
                {
                    CheckBox chkAtivo = (CheckBox)row.FindControl("chkAtivo");
                    prod.Ativo = chkAtivo.Checked;

                    DropDownList ddlPrecoPorObs = (DropDownList)row.FindControl("ddlPrecoPorObs");
                    prod.PrecoPorObs = Funcoes.intTry(ddlPrecoPorObs.SelectedValue);
                }
            }
            if (seleciona)
            {
                CarregarGrids();
                SelecionarCategoria(Funcoes.intTry(txtIdCategoria.Text));
            }

        }
        private void PizzaChange()
        {
            if (ddlPizza.SelectedValue.Equals("2"))
            {
                divCatMeia.Visible = true;
                divCatTerco.Visible = false;
                txtIdCategoriaTerco.Text = "0";
            }
            else if (ddlPizza.SelectedValue.Equals("3"))
            {
                divCatMeia.Visible = true;
                divCatTerco.Visible = true;
            }
            else
            {
                divCatMeia.Visible = false;
                divCatTerco.Visible = false;
                txtIdCategoriaMeia.Text = "0";
                txtIdCategoriaTerco.Text = "0";
            }
        }


        private void SelecionarCategoria(int idCat)
        {
            foreach (GridViewRow item in gridCategoria.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("rdoCategoria");

                if (item.Cells[1].Text.Equals(idCat.ToString()))
                {
                    rdo.Checked = true;
                    rdo.Focus();
                }
                else
                    rdo.Checked = false;

            }
        }

        protected void txtCategoria_TextChanged(object sender, EventArgs e)
        {
            AtualizarCategoria();
        }

        protected void ddlStatusCatgoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarCategoria();
        }

        protected void ImgBtnAddProduto_Click(object sender, ImageClickEventArgs e)
        {
            Session.Remove("selecionados" + urlSessao());
            carregarGrupos("", "", "");
        }


        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarGrupos(ddlGrupo.Text, "", "");

        }

        protected void ddlSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarGrupos(ddlGrupo.Text, ddlSubGrupo.Text, "");

        }

        protected void ddlDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarGrupos(ddlGrupo.Text, ddlSubGrupo.Text, ddlDepartamento.Text);

        }

        private void carregarGrupos(String grupo, String subGrupo, String departamento)
        {

            String sqlGrupo = "select Codigo_Grupo,Descricao_Grupo from Grupo";
            String sqlSubGrupo = "Select codigo_subgrupo, descricao_subgrupo from subgrupo " + (!ddlGrupo.Text.Equals("") ? " where codigo_grupo='" + ddlGrupo.SelectedValue + "'" : "");
            String sqlDepartamento = "Select codigo_departamento, descricao_departamento from W_BR_CADASTRO_DEPARTAMENTO ";
            String sqlWhereDep = "";
            if (!ddlGrupo.Text.Equals(""))
            {
                sqlWhereDep = " where codigo_grupo='" + ddlGrupo.SelectedValue + "'";
            }
            if (!ddlSubGrupo.Text.Equals(""))
            {
                if (sqlWhereDep.Length > 0)
                    sqlWhereDep += " and ";
                else
                    sqlWhereDep += " where ";

                sqlWhereDep += " codigo_subgrupo='" + ddlSubGrupo.SelectedValue + "'";
            }

            Conexao.preencherDDL1Branco(ddlGrupo, sqlGrupo, "Descricao_grupo", "codigo_grupo", null);
            Conexao.preencherDDL1Branco(ddlSubGrupo, sqlSubGrupo, "descricao_subgrupo", "codigo_subgrupo", null);
            Conexao.preencherDDL1Branco(ddlDepartamento, sqlDepartamento + sqlWhereDep, "descricao_departamento", "codigo_departamento", null);
            ddlGrupo.Text = grupo;
            ddlSubGrupo.Text = subGrupo;
            ddlDepartamento.Text = departamento;
            if (grupo.Equals("") && subGrupo.Equals("") && departamento.Equals(""))
            {
                carregarMercadorias(true);
            }
            else
            {
                carregarMercadorias(false);
            }
        }

        protected void carregarMercadorias(bool limitar)
        {
            if (IsPostBack)
            {
                verificaSelecionados();
            }
            lblMercadoriaLista.Text = "Inclusão de Produto";
            //lblMercadoriaLista.ForeColor = Label1.ForeColor;

            if (ddlGrupo.Text.Equals("") &&
                ddlSubGrupo.Text.Equals("") &&
                ddlDepartamento.Text.Equals("") &&
                txtfiltromercadoria.Text.Equals(""))
            {
                limitar = true;
            }


            User usr = (User)Session["user"];
            String sqlMercadoria = "Select distinct mercadoria.plu PLU," +
                                                   " mercadoria.descricao DESCRICAO, " +
                                                   " CASE WHEN ml.promocao =1 and convert(date,GETDATE()) between ml.Data_Inicio and ml.data_fim " +
                                                   " then ml.Preco_Promocao else  ml.preco  end as [PRC VENDA] " +
                                             " from mercadoria inner join mercadoria_loja as ml on mercadoria.plu = ml.plu " +
                                               " left join ean on mercadoria.plu=ean.plu  " +

                                    " where (ml.filial='" + usr.getFilial() + "') ";
            if (isnumero(txtfiltromercadoria.Text))
            {
                if (txtfiltromercadoria.Text.Length <= 6)
                {
                    sqlMercadoria += " and mercadoria.plu = '" + txtfiltromercadoria.Text + "' ";
                }
                else
                {
                    sqlMercadoria += " and (ean like '%" + txtfiltromercadoria.Text + "%')";
                }
            }
            else
            {
                if (txtfiltromercadoria.Text.Length > 0)
                {

                    sqlMercadoria += " and (mercadoria.descricao like '%" + txtfiltromercadoria.Text + "%' or mercadoria.Ref_fornecedor like '%" + txtfiltromercadoria.Text + "%')";
                }


                if (!ddlGrupo.Text.Equals(""))
                {
                    sqlMercadoria += " and substring(mercadoria.codigo_departamento,1,3)='" + ddlGrupo.SelectedValue.PadLeft(3, '0') + "' ";
                }
                if (!ddlSubGrupo.Text.Equals(""))
                {
                    sqlMercadoria += " and substring(mercadoria.codigo_departamento,1,6) ='" + ddlSubGrupo.SelectedValue + "' ";

                }
                if (!ddlDepartamento.Text.Equals(""))
                {
                    sqlMercadoria += " and mercadoria.codigo_departamento ='" + ddlDepartamento.SelectedValue + "' ";
                }
            }


            //if Funcoes.valorParametro("PEDIDO_SIMPLES", usr).ToUpper()
            //voltar aqui 22042015

            gridMercadoria1.DataSource = Conexao.GetTable(sqlMercadoria + " order by mercadoria.descricao", usr, limitar);
            gridMercadoria1.DataBind();

            modalIncluirProdutos.Show();


        }

        private void verificaSelecionados()
        {
            try
            {

                User usr = (User)Session["User"];

                CardapioDAO cardapio = (CardapioDAO)Session["obj" + urlSessao()];
                List<Cardapio_produtosDAO> selecionados = (List<Cardapio_produtosDAO>)Session["selecionados" + urlSessao()];
                if (selecionados == null)
                    selecionados = new List<Cardapio_produtosDAO>();

                foreach (GridViewRow row in gridMercadoria1.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("chkSelecionaItem");
                    if (chk.Checked)
                    {
                        Cardapio_CategoriaDAO CateJaCadastrada = cardapio.pluIncluido(row.Cells[1].Text);
                        if (CateJaCadastrada != null)
                        {
                            throw new Exception("Plu " + row.Cells[1].Text + " já foi incluido na Categoria :" + CateJaCadastrada.Id + " - " + CateJaCadastrada.Categoria);
                        }

                        if (selecionados.Count(e => e.Plu.Equals(row.Cells[1].Text)) == 0)
                        {
                            Cardapio_produtosDAO prod = new Cardapio_produtosDAO(usr.getFilial(), cardapio.ID)
                            {
                                Plu = row.Cells[1].Text,
                                Descricao = row.Cells[2].Text,
                                idCardapio = cardapio.ID,
                                DescricaoComercial = Conexao.retornaUmValor("Select Descricao_comercial from mercadoria where plu ='" + row.Cells[1].Text + "'", null),
                                Preco = Funcoes.decTry(row.Cells[3].Text),
                                PrecoPorObs = 1,
                                Ativo = true
                            };
                            selecionados.Add(prod);
                        }
                        chk.Checked = false;
                    }

                }

                Session.Remove("selecionados" + urlSessao());
                Session.Add("selecionados" + urlSessao(), selecionados);
                gridMercadoriasSelecionadas.DataSource = selecionados;
                gridMercadoriasSelecionadas.DataBind();
                modalIncluirProdutos.Show();
            }
            catch (Exception err)
            {
                showMessage(err.Message, true);
            }


        }


        protected void txtfiltromercadoria_TextChanged(object sender, EventArgs e)
        {
            if (txtfiltromercadoria.Text.Length > 0)
            {
                carregarMercadorias(false);
            }
            else
            {
                carregarMercadorias(true);
            }
        }
        protected void imgLimpar_Click(object sender, ImageClickEventArgs e)
        {
            limparSelecaoMercadoria();
            carregarMercadorias(true);
        }
        protected void limparSelecaoMercadoria()
        {
            ddlGrupo.Text = "";
            ddlSubGrupo.Text = "";
            ddlDepartamento.Text = "";

            txtfiltromercadoria.Text = "";
        }
        protected void chkSeleciona_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTodos = (CheckBox)sender;
            foreach (GridViewRow item in gridMercadoria1.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");

                if (chk != null)
                {
                    chk.Checked = chkTodos.Checked;
                    //incluirMercadoria(chk);
                }
            }
            modalIncluirProdutos.Show();


        }

        protected void GridMercadoriaSelecionado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            object o = ViewState["gridLinha"];
            if (o != null)
            {
                int indexSelecionado = (int)o;

                if (e.Row.RowIndex.Equals(indexSelecionado))
                {
                    e.Row.RowState = DataControlRowState.Selected;
                }

                if (e.Row.RowState == DataControlRowState.Selected)
                {
                    //e.Row.Cells[0].Focus();
                    e.Row.FindControl("txtQtd").Focus();

                }
            }
        }


        protected void imgBtnIncluirSelecionados_Click(object sender, ImageClickEventArgs e)
        {
            verificaSelecionados();
        }

        protected void ImgPesquisaMercadoria_Click(object sender, ImageClickEventArgs e)
        {
            carregarMercadorias(false);
        }
        private Cardapio_CategoriaDAO categoriaAtual()
        {
            CardapioDAO cardapio = (CardapioDAO)Session["obj" + urlSessao()];
            int idCat = Funcoes.intTry(txtIdCategoria.Text);
            Cardapio_CategoriaDAO categoria = cardapio.Categorias.Find(g => g.Id == idCat);
            return categoria;
        }
        protected void BtnConfirmarIncluirProdutos_Click(object sender, EventArgs e)
        {
            CardapioDAO cardapio = (CardapioDAO)Session["obj" + urlSessao()];
            Cardapio_CategoriaDAO categoria = categoriaAtual();
            List<Cardapio_produtosDAO> novosProdutos = (List<Cardapio_produtosDAO>)Session["selecionados" + urlSessao()];
            foreach (Cardapio_produtosDAO prod in novosProdutos)
            {
                if (categoria.Produtos.Count(p => p.Plu.Equals(prod.Plu)) == 0)
                {
                    prod.CarregarObs();
                    categoria.Produtos.Add(prod);
                }
                SelecionarCategoria(categoria.Id);
                CarregarGrids();

            }

            modalIncluirProdutos.Hide();
        }

        protected void BtnCancelarIncluirProdutos_Click(object sender, EventArgs e)
        {
            modalIncluirProdutos.Hide();
        }


        private Cardapio_produtosDAO produtoAtual(int index)
        {
            Cardapio_CategoriaDAO categoria = categoriaAtual();
            return categoria.Produtos[index];
        }

        private Cardapio_produtosDAO produtoAtual(String plu)
        {
            Cardapio_CategoriaDAO categoria = categoriaAtual();

            Cardapio_produtosDAO prod = categoria.Produtos.Find(p => p.Plu.Equals(plu));
            return prod;
        }


        protected void linkBtnObservacoes_Click(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            GridViewRow row = (GridViewRow)link.Parent.Parent;
            Cardapio_produtosDAO prod = produtoAtual(row.RowIndex);
            txtAddObsPlu.Text = prod.Plu;
            txtAddObsTitulo.Focus();
            if (prod.Observacoes.Count > 0)
            {
                gridObservacoesProduto.DataSource = prod.Observacoes;
            }
            else
            {
                gridObservacoesProduto.DataSource = null;
            }
            gridObservacoesProduto.DataBind();
            EnabledControls(pnObsProdutos, true);
            EnabledButtons(gridObservacoesProduto, status.Equals("editar"));

            modalIncluirObs.Show();
        }


        protected void btnAddObsCancela_Click(object sender, EventArgs e)
        {
            modalIncluirObs.Hide();
        }

        protected void ImgBtnAddObservacao_Click(object sender, ImageClickEventArgs e)
        {
            Cardapio_produtosDAO produto = produtoAtual(txtAddObsPlu.Text);
            User usr = (User)Session["User"];
            if (ddlAddObsTipoObservacao.Text.Equals(""))
            {
                showMessage("O tipo de Observação é obrigatoria", true);
                return;
            }

            if (produto.Observacoes.Count(o => o.Titulo.Equals(txtAddObsTitulo.Text)) == 0)
            {
                produto.Observacoes.Add(new Cardapio_Produtos_ObservacoesDAO(usr.getFilial())
                {
                    Plu = produto.Plu,
                    Titulo = txtAddObsTitulo.Text,
                    Tipo = ddlAddObsTipoObservacao.SelectedValue,
                    Obrigatorio = (ddlAddObsTipoObservacao.SelectedValue.Equals("selecao-unica") ? 1 : 0),
                    ObrigatorioOrdem = Funcoes.intTry(txtOrdemObrigatoria.Text),
                    PluAdd = txtAddObsPluAdd.Text,
                    Preco = Funcoes.decTry(txtAddObsPreco.Text)
                });
                limparAddObs();
                gridObservacoesProduto.DataSource = produto.Observacoes;
                gridObservacoesProduto.DataBind();
                modalIncluirObs.Show();
            }
            else
            {
                showMessage("Observação já cadastrada", true);
            }


        }

        private void limparAddObs()
        {
            txtAddObsPluAdd.Text = "";
            txtAddObsPreco.Text = "";
            txtAddObsTitulo.Text = "";
            ddlAddObsTipoObservacao.SelectedValue = "escolha-unica";
        }

        protected void ImgBtnEnviarAPI_Click(object sender, ImageClickEventArgs e)
        {
            String resp = "";
            String respProd = "";

            try
            {
                CardapioDAO cardapio = (CardapioDAO)Session["obj" + urlSessao()];

                if (cardapio.Titulo.ToString().IndexOf("FOOD") > 0)
                {
                    carregariFood(cardapio);
                    return;
                }

                //ApiCardapio apiCategoria = new ApiCardapio(txtUrlPadrao.Text+"/categorias/", txtToken.Text);
                //ApiCardapio apiProdutosInativar = new ApiCardapio(txtUrlPadrao.Text + "/produtos/inativar/", txtToken.Text);
                //ApiCardapio apiProdutos = new ApiCardapio(txtUrlPadrao.Text + "/produtos/", txtToken.Text);


                var jsonCategoria = new { categorias = new List<Object>() };
                var jsonProdutos = new { produtos = new List<Object>() };



                foreach (Cardapio_CategoriaDAO item in cardapio.Categorias)
                {
                    ApiCardapio apiCategoria = new ApiCardapio(txtUrlPadrao.Text + "/categorias/", txtToken.Text);
                    ApiCardapio apiProdutosInativar = new ApiCardapio(txtUrlPadrao.Text + "/produtos/inativar/", txtToken.Text);
                    ApiCardapio apiProdutos = new ApiCardapio(txtUrlPadrao.Text + "/produtos/", txtToken.Text);

                    jsonCategoria.categorias.Add(new
                    {
                        id = item.Id,
                        descricao = item.Categoria,
                        pizza = item.Pizza,
                        categoriaMeia = item.CategoriaMeia,
                        categoriaTerco = item.CategoriaTerco,
                        status = item.Status
                    });

                    int contadorEnvio = 0;
                    foreach (Cardapio_produtosDAO prod in item.Produtos)
                    {
                        var jProd = new
                        {
                            id = prod.Plu,
                            descricao = prod.DescricaoComercial,
                            titulo = prod.Descricao,
                            preco = prod.Preco,
                            ativo = prod.Ativo,
                            idCategoria = item.Id,
                            urlImagem = txtUrlPadrao.Text.Replace("/api/v1/", "") + "/static/img/" + prod.Plu + ".jpg",
                            precoPorObs = prod.PrecoPorObs,
                            observacoes = new List<Object>()
                        };
                        foreach (Cardapio_Produtos_ObservacoesDAO obs in prod.Observacoes)
                        {
                            var jObs = new
                            {
                                preco = obs.Preco,
                                tipo = obs.Tipo,
                                titulo = obs.Titulo,
                                pluAdd = (obs.PluAdd.Equals("") ? "0" : obs.PluAdd),
                                ordemObs = obs.ObrigatorioOrdem
                            };
                            jProd.observacoes.Add(jObs);
                        }
                        jsonProdutos.produtos.Add(jProd);
                    }
                    //Enviar sempre que mudar de categoria

                    //apiProdutosInativar.EnviarPost(null);

                    resp += jsonCategoria.categorias[0] + apiCategoria.EnviarPost(jsonCategoria);

                    respProd += apiProdutos.EnviarPost(jsonProdutos);
                    jsonProdutos.produtos.Clear();
                    jsonCategoria.categorias.Clear();


                }

                //apiProdutosInativar.EnviarPost(null);

                //String resp = apiCategoria.EnviarPost(jsonCategoria);

                //String respProd = apiProdutos.EnviarPost(jsonProdutos);

                showMessage("Enviado Com Sucesso <br> Cat:" + resp + " <br>Prods:" + respProd, false);
            }
            catch (Exception err)
            {
                showMessage("ERRO:" + err.Message, true);
            }

        }

        protected void ImgBtnConfirmaIncluirObservacoes_Click(object sender, ImageClickEventArgs e)
        {
            CarregarGrids();
            modalIncluirObs.Hide();
        }

        protected void txtAddObsPluAdd_TextChanged(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            txtAddObsPreco.Text = Conexao.retornaUmValor("Select preco from mercadoria_loja where plu =" + txtAddObsPluAdd.Text + "'", usr);
            modalIncluirObs.Show();
            ddlAddObsTipoObservacao.Focus();
        }

        protected void gridMercadoriasSelecionadas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            List<Cardapio_produtosDAO> selecionados = (List<Cardapio_produtosDAO>)Session["selecionados" + urlSessao()];
            selecionados.RemoveAt(index);
            Session.Remove("selecionados" + urlSessao());
            Session.Add("selecionados" + urlSessao(), selecionados);
            gridMercadoriasSelecionadas.DataSource = selecionados;
            gridMercadoriasSelecionadas.DataBind();
            modalIncluirProdutos.Show();
        }

        protected void gridProdutos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Excluir"))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                Cardapio_CategoriaDAO categoria = categoriaAtual();
                Cardapio_produtosDAO produto = categoria.Produtos[index];
                Session.Remove("produtoExcluir" + urlSessao());
                Session.Add("produtoExcluir" + urlSessao(), produto);
                lblTextConfirmaExcluirProduto.Text = " " + produto.Plu + " - " + produto.Descricao;
                modalConfirmarExcluirProduto.Show();

            }

        }

        protected void ImgBtnConfirmaExcluirBotoes_Click(object sender, EventArgs e)
        {
            Cardapio_CategoriaDAO categoria = categoriaAtual();
            Cardapio_produtosDAO produto = (Cardapio_produtosDAO)Session["produtoExcluir" + urlSessao()];
            categoria.addProdutosInativos(produto);
            categoria.Produtos.Remove(produto);

            modalConfirmarExcluirProduto.Hide();
            CarregarGrids();
        }

        protected void ImgBtnCancelaExcluirProdutos_Click(object sender, ImageClickEventArgs e)
        {
            modalConfirmarExcluirProduto.Hide();
        }

        protected void gridObservacoesProduto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Cardapio_produtosDAO prod = produtoAtual(txtAddObsPlu.Text);
            int index = Convert.ToInt32(e.CommandArgument);
            prod.Observacoes.RemoveAt(index);
            gridObservacoesProduto.DataSource = prod.Observacoes;
            gridObservacoesProduto.DataBind();
            modalIncluirObs.Show();
        }

        protected void ImgBtnToken_Click(object sender, ImageClickEventArgs e)
        {
            divToken.Visible = !divToken.Visible;
        }

        protected void carregariFood(CardapioDAO catalogo)
        {
            APIiFood ifood = new APIiFood();
            string path = Server.MapPath("~/modulos/Cadastro/imgs/uploads/");

            SqlDataReader dr;
            //dr = Conexao.consulta("sp_Cons_iFood_Produtos 2" + IDCardapio.ToString(), null, false);
            dr = Conexao.consulta("sp_Cons_iFood_Produtos " + catalogo.ID.ToString(), null, false);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Cardapio_iFood_Produto prod;
                    try
                    {
                        //System.Threading.Thread.Sleep(1000);
                        prod = new Cardapio_iFood_Produto(catalogo.ID, dr["PLU"].ToString(), path);
                        if (prod.PLU == "4239" || prod.PLU == "2222")
                        {
                            string x = "";
                        }
                        if (!ifood.PutProduto(catalogo.Catalogo, prod))
                        {
                            if (ifood.PostProduto(prod))
                            {
                                ifood.PostItem(prod.IDCategoria, prod);
                            }
                        }
                        else
                        {
                            ifood.PatchItem(prod.IDCategoria, prod);
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
                //Checa o SQLDATAREADER está fechado
                if (dr != null)
                {
                    if (!dr.IsClosed)
                    {
                        dr.Close();
                    }
                }
            }

            //APIiFood ifood = new APIiFood();
            //List<Cardapio_iFood_Produto> ifoodProdutos;
            //ifoodProdutos = ifood.produtosCardapio(catalogo.ID, path);
            //foreach(Cardapio_iFood_Produto prod in ifoodProdutos)
            //{
            //    if (!ifood.PutProduto(catalogo.Catalogo, prod))
            //    {
            //        if (ifood.PostProduto(prod))
            //        {
            //            ifood.PostItem(prod.IDCategoria, prod);
            //        }
            //    }
            //    else
            //    {
            //        ifood.PatchItem(prod.IDCategoria, prod);
            //    }
            //}
        }
    }
}