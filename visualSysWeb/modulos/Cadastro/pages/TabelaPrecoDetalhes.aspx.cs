using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class TabelaPrecoDetalhes : visualSysWeb.code.PagePadrao     //inicio da classe 
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                User usr = (User)Session["User"];
                if (usr == null)
                    return;

                bool tabMargem = Funcoes.valorParametro("TAB_PRECO_MARG", usr).ToUpper().Equals("TRUE");

                if (tabMargem)
                {
                    divPrecoMargem.Visible = true;
                    divPrecoDesconto.Visible = false;
                    pnAcrescimo.Visible = false;
                    gridProdutos.Visible = false;
                    gridMercadoriasSelecionadas.Visible = false;
                    gridTabelaPrecoMargem.Visible = true;
                    gridMercadoriasSelecionadoMargem.Visible = true;
                }
                else
                {
                    divPrecoMargem.Visible = false;
                    divPrecoDesconto.Visible = true;
                    gridProdutos.Visible = true;
                    gridMercadoriasSelecionadas.Visible = true;
                    pnAcrescimo.Visible = true;
                    gridTabelaPrecoMargem.Visible = false;
                    gridMercadoriasSelecionadoMargem.Visible = false;
                }
                TabelaPrecoDAO obj = null;
                if (Request.Params["novo"] != null)
                {
                    status = "incluir";
                    obj = new TabelaPrecoDAO(usr);
                    Session.Remove("tabelaPreco" + urlSessao());
                    Session.Add("tabelaPreco" + urlSessao(), obj);
                    habilitarCampos(true);
                    txtcodigo_tabela.Focus();
                }
                else if (Request["codigo_tabela"] != null)
                {
                    status = "visualizar";
                    string codigo = Request["codigo_tabela"].ToString();
                    obj = new TabelaPrecoDAO(codigo, usr);
                    carregarDados(obj);
                    habilitarCampos(false);
                }
            }

           
            carregabtn(pnBtn);
            btnHelpArredondamento.Visible = true;

        }
        private void carregarDados()
        {
            TabelaPrecoDAO obj = (TabelaPrecoDAO)Session["tabelaPreco" + urlSessao()];
            carregarDados(obj);
        }

        private void carregarDados(TabelaPrecoDAO obj)
        {
            txtcodigo_tabela.Text = obj.codigo_tabela;
            txtNro_Tabela.Text = obj.nro_tabela.ToString();
            txtPorc.Text = obj.porcPositivo.ToString();
            chkAcrescimo.Checked = obj.Acrescimo;
            lblDesconto.Text = chkAcrescimo.Checked ? "Acrescimo" : "Desconto";
            Session.Remove("tabelaPreco" + urlSessao());
            Session.Add("tabelaPreco" + urlSessao(), obj);
            carregarGrid();
        }
        private void carregarGrid()
        {
            TabelaPrecoDAO obj = (TabelaPrecoDAO)Session["tabelaPreco" + urlSessao()];
            User usr = (User)Session["User"];
            bool tabMargem = Funcoes.valorParametro("TAB_PRECO_MARG", usr).ToUpper().Equals("TRUE");

            if (tabMargem)
            {
                divPrecoMargem.Visible = true;
                divPrecoDesconto.Visible = false;
                pnAcrescimo.Visible = false;
                gridProdutos.Visible = false;
                gridMercadoriasSelecionadas.Visible = false;
                gridTabelaPrecoMargem.Visible = true;
                gridMercadoriasSelecionadoMargem.Visible = true;

                gridTabelaPrecoMargem.DataSource = obj.produtos;
                gridTabelaPrecoMargem.DataBind();
                gridMercadoriasSelecionadoMargem.DataSource = obj.produtos;
                gridMercadoriasSelecionadoMargem.DataBind();
                carregarDadosTipoArredondamento(gridMercadoriasSelecionadoMargem);
                carregarDadosTipoArredondamento(gridTabelaPrecoMargem);
            }
            else
            {
                divPrecoMargem.Visible = false;
                divPrecoDesconto.Visible = true;

                gridProdutos.Visible = true;
                gridMercadoriasSelecionadas.Visible = true;
                pnAcrescimo.Visible = true;
                gridTabelaPrecoMargem.Visible = false;
                gridMercadoriasSelecionadoMargem.Visible = false;
                gridProdutos.DataSource = obj.produtos;
                gridProdutos.DataBind();
                carregarDadosTipoArredondamento(gridProdutos);

                gridMercadoriasSelecionadas.DataSource = obj.produtos;
                gridMercadoriasSelecionadas.DataBind();
                carregarDadosTipoArredondamento(gridMercadoriasSelecionadas);
                

            }
        }
        private void carregarDadosTipoArredondamento(GridView grid)
        {
            foreach (GridViewRow row in grid.Rows)
            {
                Label lblTipo = (Label)row.FindControl("lblTipoConta");
                DropDownList ddl = (DropDownList)row.FindControl("ddlTipoArredondamento");
                ddl.SelectedValue = lblTipo.Text;
            }
        }
        private void carregarDadosObj()
        {
            User usr = (User)Session["User"];
            bool tabMargem = Funcoes.valorParametro("TAB_PRECO_MARG", usr).ToUpper().Equals("TRUE");
            if (TabContainer1.ActiveTabIndex == 1)
            {
                if (tabMargem)
                {
                    atualizarGrid(gridMercadoriasSelecionadoMargem);
                }
                else
                {
                    atualizarGrid(gridMercadoriasSelecionadas);
                }
                
            }
              
            else{
                if (tabMargem)
                {
                    atualizarGrid(gridTabelaPrecoMargem);
                }
                else
                {
                    atualizarGrid(gridProdutos);
                }
                
            }
               

            TabelaPrecoDAO obj = (TabelaPrecoDAO)Session["tabelaPreco" + urlSessao()];
            obj.codigo_tabela = txtcodigo_tabela.Text;
            obj.nro_tabela = Funcoes.decTry(txtNro_Tabela.Text);
            obj.porc = Funcoes.decTry(txtPorc.Text);
            if (chkAcrescimo.Checked)
            {
                obj.porc = obj.porc * -1;
            }


            Session.Remove("tabelaPreco" + urlSessao());
            Session.Add("tabelaPreco" + urlSessao(), obj);


        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("TabelaPrecoDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("TabelaPreco.aspx");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            editar(pnBtn);
            TabelaPrecoDAO obj = (TabelaPrecoDAO)Session["tabelaPreco" + urlSessao()];
            carregarDados(obj);
            habilitarCampos(true);


        }

        private void habilitarCampos(bool enable)
        {
            EnabledControls(cabecalho, enable);
            EnabledControls(addItens, enable);

            habilitarGrid(gridProdutos, enable);
            habilitarGrid(gridMercadoriasSelecionadas, enable);

            habilitarGrid(gridTabelaPrecoMargem, enable);
            habilitarGrid(gridMercadoriasSelecionadoMargem, enable);

            pnAcrescimo.Enabled = enable;
            chkAcrescimo.Enabled = true;
            divAdd.Visible = enable;
            divIncluirSelecionado.Visible = enable;
            txtDescricao.Enabled = false;
            txtPreco.Enabled = false;
            txtPrecoCusto.Enabled = false;
            //EnabledControls(conteudo, enable);
            //EnabledButtons(gridProdutos, enable);
        }

        private void habilitarGrid(GridView grid, bool enable)
        {
            foreach (GridViewRow row in grid.Rows)
            {

                TextBox txtDesconto = (TextBox)row.FindControl("txtDescontoItem");
                TextBox txtPrecoPromocao = (TextBox)row.FindControl("txtPrecoPromocaoItem");
                TextBox txtDescontoPromocao = (TextBox)row.FindControl("txtDescontoPromocaoItem");
                DropDownList ddlTipo = (DropDownList)row.FindControl("ddlTipoArredondamento");
                Panel pnAcrescimo = (Panel)row.FindControl("pnAcrescimo");
                TextBox txtMargemItem = (TextBox)row.FindControl("txtMargemItem");


                if (txtDesconto != null)
                {
                    txtDesconto.Enabled = enable;
                }
                if (txtPrecoPromocao != null)
                {
                    txtPrecoPromocao.Enabled = enable;
                }
                if (pnAcrescimo != null)
                {
                    pnAcrescimo.Enabled = enable;
                }
                if (ddlTipo != null)
                {
                    ddlTipo.Enabled = enable;
                }
                if(txtMargemItem != null)
                {
                    txtMargemItem.Enabled = enable;
                }


            }
            EnabledButtons(grid, enable);
        }
        private bool validarCampos()
        {
            validaCampos(cabecalho);
            validaCampos(conteudo);
            return true;
        }
        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            modalExcluir.Show();
        }
        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                carregarDadosObj();
                if (validarCampos())
                {
                    TabelaPrecoDAO obj = (TabelaPrecoDAO)Session["tabelaPreco" + urlSessao()];
                    obj.salvar(status.Equals("incluir"));
                    msgShow("Salvo com sucesso", false);
                    status = "visualizar";
                    visualizar(pnBtn);
                    carregarDados(obj);
                    habilitarCampos(false);
                    txtPlu.Text = "";
                    txtDescricao.Text = "";
                    txtDesconto.Text = "";
                    txtPreco.Text = "";
                    txtPrecoPromocao.Text = "";
                    TxtMargem.Text = "";
                    txtPrecoTabela.Text = "";
                    txtPrecoCusto.Text = "";
                    ddlTipoArredondamento.SelectedValue = "1";
                }
            }
            catch (Exception erro)
            {
                msgShow(erro.Message, true);
            }

        }
        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("TabelaPreco.aspx");

        }


        protected override bool campoObrigatorio(Control campo)
        {
            switch (campo.ID)
            {
                case "txtcodigo_tabela":
                    return true;
                default:
                    return false;
            };
        }

        protected override bool campoDesabilitado(Control campo)
        {
            if (status.Equals("editar"))
            {
                switch (campo.ID)
                {
                    case "txtcodigo_tabela":
                        return true;
                    default:
                        return false;
                };
            }
            switch (campo.ID)
            {
                case "txtDescricao":
                case "txtPreco":
                case "txtPrecoCusto":
                    return true;
                default:
                    return false;
            };


        }

        protected void btnOkError_Click(object sender, EventArgs e)
        {
            if (lblErroPanel.Text.Equals("Excluido com Sucesso"))
            {
                Response.Redirect("TabelaPreco.aspx");
            }
            modalError.Hide();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!txtPlu.Text.Trim().Equals(""))
            {
                atualizarGrid(gridProdutos);
                TabelaPrecoDAO obj = (TabelaPrecoDAO)Session["tabelaPreco" + urlSessao()];
                if (!obj.produtos.Exists(p => p.PLU.Equals(txtPlu.Text)))
                {
                    User usr = (User)Session["User"];
                    bool tabMargem = Funcoes.valorParametro("TAB_PRECO_MARG", usr).ToUpper().Equals("TRUE");
                    preco_mercadoriaDAO item = null;
                    if (tabMargem)
                    {
                        item = new preco_mercadoriaDAO()
                        {
                            PLU = txtPlu.Text,
                            Descricao = txtDescricao.Text,
                            PrecoCusto = Funcoes.decTry(txtPrecoCusto.Text),
                            Preco = Funcoes.decTry(txtPreco.Text),
                            Preco_promocao = Funcoes.decTry(txtPrecoTabela.Text),
                            tipo_arredondamento = Funcoes.intTry(ddlTipoArredondamento.SelectedValue)
                            
                        };

                    }
                    else
                    {
                        item = new preco_mercadoriaDAO()
                        {
                            PLU = txtPlu.Text,
                            Descricao = txtDescricao.Text,
                            Preco = Funcoes.decTry(txtPreco.Text),
                            Desconto = Funcoes.decTry(txtDesconto.Text),
                            tipo_arredondamento = Funcoes.intTry(ddlTipoArredondamento.SelectedValue)
                        };
                        item.Preco_promocao = getCalculoPrecoPromocao(item.Preco, item.Desconto, chkAcrescimo.Checked);
                        if (chkAcrescimo.Checked)
                        {
                            if (item.Desconto > 0)
                                item.Desconto = item.Desconto * -1;

                        }
                    }
                    
                    obj.produtos.Add(item);
                }

                Session.Remove("tabelaPreco" + urlSessao());
                Session.Add("tabelaPreco" + urlSessao(), obj);

                carregarGrid();

                txtPlu.Text = "";
                txtDescricao.Text = "";
                txtDesconto.Text = "";
                txtPreco.Text = "";
                txtPrecoPromocao.Text = "";
                TxtMargem.Text = "";
                txtPrecoTabela.Text = "";
                txtPrecoCusto.Text = "";
                ddlTipoArredondamento.SelectedValue = "1";
                txtPlu.Focus();
                addItens.DefaultButton = "ImgBtn_txtPluAddRapido";
            }
            else
            {
                msgShow("Selecione um item para incluir", true);
            }
        }
        protected void gridProdutos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            TabelaPrecoDAO obj = (TabelaPrecoDAO)Session["tabelaPreco" + urlSessao()];
            obj.produtos.RemoveAt(index);

            carregarGrid();
        }
        protected void btnConfirmaExclusao_Click(object sender, EventArgs e)
        {
            try
            {
                TabelaPrecoDAO obj = (TabelaPrecoDAO)Session["tabelaPreco" + urlSessao()];
                obj.excluir();
                msgShow("Excluido com Sucesso", false);
            }
            catch (Exception err)
            {

                msgShow(err.Message, true);
            }

        }
        protected void btnCancelaExclusao_Click(object sender, EventArgs e)
        {

        }
       

        protected void ImgBtnAddItens_Click(object sender, EventArgs e)
        {
            carregarPlu();
        }
        protected void atualizarGrid(GridView grid)
        {
            User usr = (User)Session["User"];
            bool tabMargem = Funcoes.valorParametro("TAB_PRECO_MARG", usr).ToUpper().Equals("TRUE");
            TabelaPrecoDAO obj = (TabelaPrecoDAO)Session["tabelaPreco" + urlSessao()];
            foreach (GridViewRow row in grid.Rows)
            {
                if (tabMargem)
                {
                    TextBox txtPrecoPromocaoItem = (TextBox)row.FindControl("txtPrecoPromocaoItem");
                    DropDownList ddlTipoArredondamento = (DropDownList)row.FindControl("ddlTipoArredondamento");
                    preco_mercadoriaDAO prod = obj.produtos.Find(e => e.PLU.Equals(row.Cells[1].Text));
                    prod.Preco_promocao = Funcoes.decTry(txtPrecoPromocaoItem.Text);
                    prod.tipo_arredondamento = Funcoes.intTry(ddlTipoArredondamento.SelectedValue);
                    prod.Desconto = Funcoes.porcDesconto(prod.Preco, prod.Preco_promocao);
                }
                else
                {
                    TextBox txtDesconto = (TextBox)row.FindControl("txtDescontoItem");
                    TextBox txtPrecoPromocaoItem = (TextBox)row.FindControl("txtPrecoPromocaoItem");
                    CheckBox chkAcrescimoDesconto = (CheckBox)row.FindControl("chkAcrescimo");
                    DropDownList ddlTipoArredondamento = (DropDownList)row.FindControl("ddlTipoArredondamento");
                    preco_mercadoriaDAO prod = obj.produtos.Find(e => e.PLU.Equals(row.Cells[1].Text));
                    prod.Desconto = Funcoes.decTry(txtDesconto.Text);
                    prod.Preco_promocao = Funcoes.decTry(txtPrecoPromocaoItem.Text);
                    prod.tipo_arredondamento = Funcoes.intTry(ddlTipoArredondamento.SelectedValue);
                    if (chkAcrescimoDesconto.Checked)
                    {
                        if (prod.Desconto > 0)
                            prod.Desconto = prod.Desconto * -1;

                    }
                }
               


            }
        }

        protected void gridItens_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
        protected void msgShow(String mensagem, bool erro)
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
            btnOkError.Focus();
            modalError.Show();
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
        protected void ddlLinha_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarMercadorias(false);
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
        private void carregarLinhas()
        {
            String sqlLinha = "Select codigo=convert(varchar(3),linha.codigo_linha)+convert(varchar(3),cor_linha.codigo_cor),  linha= linha.descricao_linha+'-'+cor_linha.descricao_cor from linha " +
                                                                " inner join cor_linha on linha.codigo_linha = cor_linha.codigo_linha";
            Conexao.preencherDDL1Branco(ddlLinha, sqlLinha, "linha", "codigo", null);
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
                ddlLinha.Text.Equals("") &&
                txtFornecedor.Text.Equals("") &&
                txtfiltromercadoria.Text.Equals(""))
            {
                limitar = true;
            }


            User usr = (User)Session["user"];
            String sqlMercadoria = "Select distinct mercadoria.plu PLU," +
                                                   " isnull(ean.ean,'---')EAN," +
                                                   " mercadoria.Ref_fornecedor REFERENCIA, " +
                                                   " mercadoria.descricao DESCRICAO, " +
                                                   " mercadoria_loja.preco as [PRC VENDA], " +
                                                   " mercadoria_loja.preco_custo as [PRC CUSTO]"+
                                             " from mercadoria inner join mercadoria_loja on mercadoria.plu = mercadoria_loja.plu " +
                                               " left join ean on mercadoria.plu=ean.plu  " +
                                               //" inner join W_BR_CADASTRO_DEPARTAMENTO on mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento "+
                                               " left join Fornecedor_Mercadoria on mercadoria.PLU = Fornecedor_Mercadoria.PLU  AND Mercadoria_Loja.Filial = Fornecedor_Mercadoria.Filial " +
                                    " where (mercadoria_loja.filial='" + usr.getFilial() + "') ";
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

            if (!ddlLinha.Text.Equals(""))
            {
                sqlMercadoria += " and convert(varchar(3),isnull(Cod_Linha,''))+CONVERT(varchar(3),isnull(Cod_Cor_Linha,'')) ='" + ddlLinha.SelectedValue + "'";
            }

            if (!txtFornecedor.Text.Trim().Equals(""))
            {
                sqlMercadoria += " and (Fornecedor_Mercadoria.Fornecedor ='" + txtFornecedor.Text + "') ";
            }
            //if Funcoes.valorParametro("PEDIDO_SIMPLES", usr).ToUpper()
            //voltar aqui 22042015

            gridMercadoria1.DataSource = Conexao.GetTable(sqlMercadoria + " order by mercadoria.descricao", usr, limitar);
            gridMercadoria1.DataBind();




        }
        private void verificaSelecionados()
        {
            carregarDadosObj();
            foreach (GridViewRow item in gridMercadoria1.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                if (chk.Checked)
                {
                    incluirMercadoria(chk);
                    chk.Checked = false;
                }
            }
            carregarGrid();
        }
        private void incluirMercadoria(CheckBox ck1)
        {
            GridViewRow linha = (GridViewRow)ck1.NamingContainer;


            User usr = (User)Session["User"];
            TabelaPrecoDAO obj = (TabelaPrecoDAO)Session["tabelaPreco" + urlSessao()];
            if (!obj.produtos.Exists(p => p.PLU.Equals(linha.Cells[1].Text)))
            {
                preco_mercadoriaDAO item = new preco_mercadoriaDAO();
                item.PLU = linha.Cells[1].Text;
                item.Descricao = linha.Cells[4].Text;

                item.Preco = Funcoes.decTry(linha.Cells[5].Text);
                item.PrecoCusto = Funcoes.decTry(linha.Cells[6].Text);
                item.Desconto = Funcoes.decTry(txtPorc.Text);
                item.Preco_promocao = getCalculoPrecoPromocao(item.Preco, item.Desconto, chkAcrescimo.Checked);
                obj.produtos.Add(item);

                Session.Remove("tabelaPreco" + urlSessao());
                Session.Add("tabelaPreco" + urlSessao(), obj);
            }
        }
        protected void ImgPesquisaMercadoria_Click(object sender, ImageClickEventArgs e)
        {
            carregarMercadorias(false);
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
            ddlLinha.Text = "";
            txtFornecedor.Text = "";
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


        }
        protected void GridMercadoriaSelecionado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            TabelaPrecoDAO obj = (TabelaPrecoDAO)Session["tabelaPreco" + urlSessao()];
            obj.produtos.RemoveAt(index);

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

        protected void imgFornecedor_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("Fornecedor");
        }
        protected void exibeLista(String campo)
        {
            Session.Remove("campoLista" + urlSessao());
            Session.Add("campoLista" + urlSessao(), campo);

            String sqlLista = "";

            switch (campo)
            {
                case "PLU":
                    lbltituloLista.Text = "Escolha uma mercadoria";
                    sqlLista = "select mercadoria.PLU, ISNULL(mercadoria.Ref_Fornecedor,'') as REFERENCIA, DESCRICAO,isnull(ean.ean,'') EAN from mercadoria left join ean on ean.plu=mercadoria.plu where ";
                    sqlLista = sqlLista + " mercadoria.plu like '%" + TxtPesquisaLista.Text + "%' or descricao like '%" + TxtPesquisaLista.Text + "%'  or mercadoria.Ref_fornecedor like '%" + TxtPesquisaLista.Text + "%' or (ean like '%" + TxtPesquisaLista.Text + "%') ORDER BY DESCRICAO";

                    break;
                case "Fornecedor":
                    lbltituloLista.Text = "Escolha um  Fornecedor";
                    sqlLista = "select Fornecedor,CNPJ,Razao_social from Fornecedor " +
                               " where Fornecedor like '%" + TxtPesquisaLista.Text + "%'  or " +
                               " Razao_social like '%" + TxtPesquisaLista.Text + "%' or  " +
                               "  Nome_Fantasia = '%" + TxtPesquisaLista.Text + "%' or  " +
                               " replace(replace(REPLACE(cnpj,'.',''),'/',''),'-','') like '" + TxtPesquisaLista.Text.Replace(".", "").Replace("-", "").Replace("/", "") + "%' order by fornecedor ";

                    break;
            }
            User usr = (User)Session["User"];
            GridLista.DataSource = Conexao.GetTable(sqlLista, null, true);
            GridLista.DataBind();

            modalLista.Show();
            TxtPesquisaLista.Text = "";
            TxtPesquisaLista.Focus();
        }
        protected void gridItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {


        }

        protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            int tab = TabContainer1.ActiveTabIndex;
            bool tabMargem = Funcoes.valorParametro("TAB_PRECO_MARG", usr).ToUpper().Equals("TRUE");
            if (tab == 1)
            {

                atualizarGrid(tabMargem ? gridTabelaPrecoMargem: gridProdutos);
                carregarGrid();
                carregarGrupos("", "", "");
                carregarLinhas();


            }
            else
            {
                atualizarGrid(tabMargem ? gridMercadoriasSelecionadoMargem :  gridMercadoriasSelecionadas);
                carregarGrid();
            }
           
            habilitarCampos(!status.Equals("visualizar"));
        }
        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {

            String itemLista = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            switch (itemLista)
            {
                case "PLU":
                    txtPlu.Text = ListaSelecionada(1);
                    carregarPlu();
                    break;

                case "Fornecedor":
                    txtFornecedor.Text = ListaSelecionada(1);
                    carregarMercadorias(false);
                    break;
            }

            modalLista.Hide();
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
        protected void carregarPlu()
        {
            if (!txtPlu.Text.Trim().Equals(""))
            {
                User usr = (User)Session["User"];
                bool tabMargem = Funcoes.valorParametro("TAB_PRECO_MARG", usr).ToUpper().Equals("TRUE");
                MercadoriaDAO merc = new MercadoriaDAO(txtPlu.Text, usr);
                txtDescricao.Text = merc.Descricao;
                if (tabMargem)
                {
                    txtPrecoCusto.Text = merc.Preco_Custo.ToString("N2");
                    txtPreco.Text = merc.Preco.ToString("N2");
                    TxtMargem.Text = merc.Margem.ToString("N2");
                    txtPrecoTabela.Text = merc.Preco.ToString("N2");
                    txtPrecoTabela.Focus();
                }
                else
                {
                    txtPreco.Text = merc.Preco.ToString("N2");
                    if (!txtPorc.Text.Equals(""))
                        txtDesconto.Text = txtPorc.Text;

                    decimal porc = Funcoes.decTry(txtPorc.Text);

                    txtPrecoPromocao.Text = getCalculoPrecoPromocao(merc.Preco, porc, chkAcrescimo.Checked).ToString();
                    txtDesconto.Focus();

                }

                addItens.DefaultButton = "btnAdd";

            }
            else
            {
                exibeLista("PLU");
            }

        }
        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalLista.Hide();
        }
        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            String itemLista = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            exibeLista(itemLista);
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

        protected void txtPorc_TextChanged(object sender, EventArgs e)
        {
            atribuirItens();
        }

        protected void txtGrid_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.Parent.Parent;
            this.calculaValor(row);

        }
        protected void txtGridPreco_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.Parent.Parent;
            this.calculaDesconto(row);
        }
        protected void txtGridDesconto_TextChanged(object sender , EventArgs e)
        {
            TextBox txtGridDesconto = (TextBox)sender;
            GridViewRow row = (GridViewRow)txtGridDesconto.Parent.Parent;
            this.calculaValor(row);


        }
        protected void txtMarg_TextChanged(object sender,EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.Parent.Parent;
            this.calculaMargemPreco(row);
        }
        protected void txtPreco_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.Parent.Parent;
            this.calcularPrecoMargem(row);
        }
        protected void chkAcrescimo_CheckedChanged(object sender, EventArgs e)
        {
            atribuirItens();
            lblDesconto.Text = chkAcrescimo.Checked ? "Acrescimo" : "Desconto";
            if (!txtPrecoPromocao.Text.Trim().Equals(""))
            {
                Decimal valorPromocao = getCalculoPrecoPromocao(Funcoes.decTry(txtPreco.Text), Funcoes.decTry(txtDesconto.Text), chkAcrescimo.Checked);
                txtPrecoPromocao.Text = Funcoes.arredondar(valorPromocao, 1, Funcoes.intTry(ddlTipoArredondamento.SelectedValue)).ToString("N2");
            }
        }
        protected void atribuirItens()
        {
            TabelaPrecoDAO obj = (TabelaPrecoDAO)Session["tabelaPreco" + urlSessao()];
            foreach (preco_mercadoriaDAO item in obj.produtos)
            {

                item.Desconto = Funcoes.decTry(txtPorc.Text);
                if (chkAcrescimo.Checked)
                {
                    item.Desconto = item.Desconto * -1;
                }
                Decimal valor = getCalculoPrecoPromocao(item.Preco, item.PositivoDesconto, item.Acrescimo);
                item.Preco_promocao = Funcoes.arredondar(valor, 1, item.tipo_arredondamento);
            }
            carregarGrid();
        }

        protected void chkAcrescimo_CheckedChanged1(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chk.Parent.Parent.Parent;
            this.calculaValor(row);
        }

        protected void ddlTipoArredondamento_SelectedIndexChanged(object sender, EventArgs e)
        {

            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            this.calculaValor(row);
            ddl.Focus();
        }

        private void calculaMargemPreco(GridViewRow row)
        {
            TextBox txtMarg = (TextBox)row.FindControl("txtMargemItem");
            TextBox txtPrecoPromo = (TextBox)row.FindControl("txtPrecoPromocaoItem");
            DropDownList ddl = (DropDownList)row.FindControl("ddlTipoArredondamento");
            bool tabMargem = Funcoes.valorParametro("TAB_PRECO_MARG", null).ToUpper().Equals("TRUE");

            Decimal preco = 0;

            if (tabMargem)
            {
                preco = Funcoes.decTry(row.Cells[4].Text);
            }
            else
            {
                preco = Funcoes.decTry(row.Cells[3].Text);
            }
            Decimal marg = Funcoes.decTry(txtMarg.Text);
            Decimal precoFinal =  Funcoes.precoMargem(preco, marg);
            precoFinal = Funcoes.arredondar(precoFinal, 1, Funcoes.intTry(ddl.SelectedValue));
            txtPrecoPromo.Text = precoFinal.ToString("N2");
            calcularPrecoMargem(row);
            txtPrecoPromo.Focus();
        }
        private void calcularPrecoMargem(GridViewRow row)
        {
            TextBox txtMarg = (TextBox)row.FindControl("txtMargemItem");
            TextBox txtPrecoPromo = (TextBox)row.FindControl("txtPrecoPromocaoItem");
            bool tabMargem = Funcoes.valorParametro("TAB_PRECO_MARG", null).ToUpper().Equals("TRUE");

            Decimal preco = 0;

            if (tabMargem)
            {
                preco = Funcoes.decTry(row.Cells[4].Text);
            }
            else
            {
                preco = Funcoes.decTry(row.Cells[3].Text);
            }
            Decimal precoPromo = Funcoes.decTry(txtPrecoPromo.Text);
            txtMarg.Text = Funcoes.valorMargem(preco,precoPromo).ToString("N2");
            txtMarg.Focus();
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static decimal getValorMargem(decimal preco, decimal precoPromo)
        {
            return  Funcoes.valorMargem(preco,precoPromo);
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static decimal getValorArredondar(decimal preco, int vdec,int tipo)
        {
            decimal vlr = Funcoes.arredondar(preco, vdec, tipo);
            return vlr;

        }
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static decimal getPrecoMargem(decimal vlrCusto,decimal vlrMargem)
        {
            return Funcoes.precoMargem(vlrCusto,vlrMargem);
        }
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static bool getTabPrecoMarg()
        {
            return Funcoes.valorParametro("TAB_PRECO_MARG", null).ToUpper().Equals("TRUE") ;
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static decimal getCalculoPrecoPromocao(decimal valor, decimal porc, bool acrescimo)
        {
            decimal vlr_porc = (valor * porc) / 100;
            decimal vlr_preco = valor + (acrescimo ? vlr_porc : -vlr_porc);
            return vlr_preco;
        }

        
        private void calculaValor(GridViewRow row)
        {
            User usr = (User)Session["User"];
            bool tabMargem = Funcoes.valorParametro("TAB_PRECO_MARG", usr).ToUpper().Equals("TRUE");
            DropDownList ddl = (DropDownList)row.FindControl("ddlTipoArredondamento");

            if (ddl != null)
            {
                if (tabMargem)
                {
                    TextBox txtMargemItem = (TextBox)row.FindControl("txtMargemItem");
                    TextBox txtPrecoPromocaoItem = (TextBox)row.FindControl("txtPrecoPromocaoItem");

                    Decimal vlrMargem = Funcoes.decTry(txtMargemItem.Text);
                    Decimal vlrCusto = Funcoes.decTry(row.Cells[4].Text);
                    Decimal preco = Funcoes.precoMargem(vlrCusto, vlrMargem);
                    txtPrecoPromocaoItem.Text = Funcoes.arredondar(preco, 1, Funcoes.intTry(ddl.SelectedValue)).ToString("N2");
                    calcularPrecoMargem(row);
                    txtPrecoPromocaoItem.Focus();
                }
                else { 
                    CheckBox chk = (CheckBox)row.FindControl("chkAcrescimo");
                    TextBox txtPrecoPromocaoItem = (TextBox)row.FindControl("txtPrecoPromocaoItem");
                    TextBox textDesc = (TextBox)row.FindControl("txtDescontoItem");

                    int tipo = Funcoes.intTry(ddl.SelectedValue);
                    Decimal preco = Funcoes.decTry(row.Cells[3].Text);

                    Decimal desconto = Funcoes.decTry(textDesc.Text);
                    bool acrescimo = (chk != null ? chk.Checked : false);
                    Decimal valor = getCalculoPrecoPromocao(preco, desconto, acrescimo);
                    valor = Funcoes.arredondar(valor, 1, Funcoes.intTry(ddl.SelectedValue));
                    Decimal nValorDesc = (preco - valor);
                    if (chk != null && chk.Checked)
                    {
                        nValorDesc = nValorDesc * -1;
                    }
                    txtPrecoPromocaoItem.Text = valor.ToString("N2");
                    if (textDesc != null && nValorDesc != 0)
                    {
                        nValorDesc = (nValorDesc / preco) * 100;
                        textDesc.Text = nValorDesc.ToString("N2");
                    }
                }
               
            }

        }

        private void calculaDesconto(GridViewRow row)
        {
            DropDownList ddl = (DropDownList)row.FindControl("ddlTipoArredondamento");
            CheckBox chk = (CheckBox)row.FindControl("chkAcrescimo");
            TextBox txtPrecoPromocaoItem = (TextBox)row.FindControl("txtPrecoPromocaoItem");
            TextBox textDesc = (TextBox)row.FindControl("txtDescontoItem");

            int tipo = Funcoes.intTry(ddl.SelectedValue);
            Decimal preco = Funcoes.decTry(row.Cells[3].Text);
            
            bool acrescimo = (chk != null ? chk.Checked : false);
            Decimal valor =  Funcoes.decTry(txtPrecoPromocaoItem.Text);
            valor = Funcoes.arredondar(valor, 2, Funcoes.intTry(ddl.SelectedValue));
            Decimal nValorDesc = (preco - valor);
            

            txtPrecoPromocaoItem.Text = valor.ToString("N2");
            if (textDesc != null && nValorDesc != 0)
            {
                nValorDesc = (nValorDesc / preco) * 100;
                if (nValorDesc < 0)
                {
                    nValorDesc = nValorDesc * -1;
                    chk.Checked = true;
                }
                else
                {
                    chk.Checked = false;
                }
                    textDesc.Text = nValorDesc.ToString("N2");
                
            }
        }
        protected void gridTabelaPrecoMargem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            TabelaPrecoDAO obj = (TabelaPrecoDAO)Session["tabelaPreco" + urlSessao()];
            obj.produtos.RemoveAt(index);

            carregarGrid();
        }

        protected void gridMercadoriasSelecionadoMargem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            TabelaPrecoDAO obj = (TabelaPrecoDAO)Session["tabelaPreco" + urlSessao()];
            obj.produtos.RemoveAt(index);

            carregarGrid();
        }

        protected void TxtMargem_TextChanged(object sender, EventArgs e)
        {
            Decimal vlrMargem = Funcoes.decTry(TxtMargem.Text);
            Decimal vlrCusto = Funcoes.decTry(txtPrecoCusto.Text);
            Decimal preco = Funcoes.precoMargem(vlrCusto, vlrMargem);
            txtPrecoTabela.Text = Funcoes.arredondar(preco, 1, Funcoes.intTry(ddlTipoArredondamento.SelectedValue)).ToString("N2");
            txtPrecoTabela.Focus();
        }

        protected void txtPrecoTabela_TextChanged(object sender, EventArgs e)
        {
            Decimal vlrPreco = Funcoes.decTry(txtPrecoTabela.Text);
            Decimal vlrCusto = Funcoes.decTry(txtPrecoCusto.Text);
            TxtMargem.Text = Funcoes.valorMargem(vlrCusto, vlrPreco).ToString("N2");
            btnAdd.Focus();
        }

        protected void ddlTipoArredondamento_SelectedIndexChanged1(object sender, EventArgs e)
        {

            Decimal vlrMargem = Funcoes.decTry(TxtMargem.Text);
            Decimal vlrCusto = Funcoes.decTry(txtPrecoCusto.Text);
            Decimal preco = Funcoes.precoMargem(vlrCusto, vlrMargem);
            txtPrecoTabela.Text = Funcoes.arredondar(preco, 1, Funcoes.intTry(ddlTipoArredondamento.SelectedValue)).ToString("N2");
            txtPrecoTabela.Focus();

        }

        protected void txtDesconto_TextChanged(object sender, EventArgs e)
        {
            Decimal preco = Funcoes.decTry(txtPreco.Text);

            Decimal desconto = Funcoes.decTry(txtDesconto.Text);
            bool acrescimo = chkAcrescimo.Checked;
            Decimal valor = getCalculoPrecoPromocao(preco, desconto, acrescimo);
            valor = Funcoes.arredondar(valor, 1, Funcoes.intTry(ddlTipoArredondamento.SelectedValue));
            Decimal nValorDesc = (preco - valor);
            if (acrescimo)
            {
                nValorDesc = nValorDesc * -1;
            }
            txtPrecoPromocao.Text = valor.ToString("N2");
        }

        protected void ImgBtnFecharHelp_Click(object sender, ImageClickEventArgs e)
        {
            modalHelpArredondar.Hide();
        }

        protected void btnHelpArredondamento_Click(object sender, ImageClickEventArgs e)
        {
            modalHelpArredondar.Show();
        }

        protected void txtPrecoPromocao_TextChanged(object sender, EventArgs e)
        {
            Decimal preco = Funcoes.decTry(txtPreco.Text);
            Decimal precoPromocao = Funcoes.decTry(txtPrecoPromocao.Text);
            txtPreco.Text = preco.ToString("N2");
            Decimal desconto = (preco - precoPromocao);
            if(desconto != 0)
            {
                Decimal nValorDesc = (desconto / preco) * 100;
                txtDesconto.Text = nValorDesc.ToString("N2");
            }
            else
            {
                txtDesconto.Text = "0,00";
            }
                
          
        }
    }
}