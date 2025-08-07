using ExcelDataReader;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;
using visualSysWeb.modulos.Gerencial.code;
using visualSysWeb.modulos.Gerencial.dao;

namespace visualSysWeb.modulos.Gerencial.pages
{
    public partial class ProgramacaoPrecificacaoProdutosDetalhes : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];

            if (usr != null)
            {
                if (!IsPostBack)
                {
                    Programacao_PrecificacaoDAO lista = null;
                    if (Request["novo"] != null)
                    {

                        status = "incluir";
                        Programacao_PrecificacaoDAO listaManter = (Programacao_PrecificacaoDAO)Session["manterProgramacao"];
                        if (listaManter != null)
                        {
                            modalConfirmaManter.Show();
                        }
                        else
                        {

                            lista = new Programacao_PrecificacaoDAO();
                            Session.Remove("listaProduto" + urlSessao());
                            Session.Add("listaProduto" + urlSessao(), lista);
                            novaLista();

                        }
                    }
                    else if (Request["id"] != null)
                    {
                        int id = Funcoes.intTry(Request["id"].ToString());
                        lista = new Programacao_PrecificacaoDAO(id);
                        status = "visualizar";
                    }

                    Session.Remove("listaProduto" + urlSessao());
                    Session.Add("listaProduto" + urlSessao(), lista);
                    CarregarDados();
                    HabilitarCampos(!status.Equals("visualizar"));

                }
            }
            carregabtn(pnBtn);

        }

        private void novaLista()
        {
            User usr = (User)Session["User"];
            Programacao_PrecificacaoDAO lista = (Programacao_PrecificacaoDAO)Session["listaProduto" + urlSessao()];
            lista.data_cadastro = DateTime.Now;
            lista.Usuario_cadastro = usr.getUsuario();
            lista.filial = "TODAS";
            status = "incluir";
            CarregarDados();
            carregarMercadorias();
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Programacao_PrecificacaoDAO lista = (Programacao_PrecificacaoDAO)Session["listaProduto" + urlSessao()];
            Session.Remove("manterProgramacao");
            Session.Add("manterProgramacao", lista);
            Response.Redirect("ProgramacaoPrecificacaoDetalhes.aspx?novo=true");
        }


        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            status = "editar";
            HabilitarCampos(true);
            carregabtn(pnBtn);
            carregarMercadorias();
        }

        private void CarregarDados()
        {
            Programacao_PrecificacaoDAO lista = (Programacao_PrecificacaoDAO)Session["listaProduto" + urlSessao()];
            if (lista != null)
            {
                txtId.Text = lista.id.ToString();
                txtFilial.Text = lista.filial;
                txtDescricao.Text = lista.descricao;
                txtUsuario.Text = lista.Usuario_cadastro;
                txtDataCadastro.Text = Funcoes.dataBr(lista.data_cadastro);
                txtDataInicio.Text = Funcoes.dataBr(lista.data_inicio);
                carregarGrids();
            }

        }

        private void CarregarDadosObj()
        {
            Programacao_PrecificacaoDAO lista = (Programacao_PrecificacaoDAO)Session["listaProduto" + urlSessao()];
            lista.id = Funcoes.intTry(txtId.Text);
            lista.filial = txtFilial.Text;
            lista.descricao = txtDescricao.Text;
            lista.Usuario_cadastro = txtUsuario.Text;
            lista.data_cadastro = Funcoes.dtTry(txtDataCadastro.Text);
            lista.data_inicio = Funcoes.dtTry(txtDataInicio.Text);

            GridView grid = null;

            if (TabContainer1.ActiveTabIndex == 0)
            {
                grid = gridItens;
            }
            else
            {
                grid = gridItensSelecionados;
            }
            foreach (GridViewRow row in grid.Rows)
            {
                TextBox txtPreco = (TextBox)row.FindControl("txtPreco");
                if (txtPreco != null)
                {
                    Programacao_Precificacao_ItemDAO item = lista.Itens.Find(a => a.plu.Equals(row.Cells[1].Text));
                    item.preco = Funcoes.decTry(txtPreco.Text);
                }
            }

            Session.Remove("listaProduto" + urlSessao());
            Session.Add("listaProduto" + urlSessao(), lista);

        }

        private void carregarGrids()
        {
            Programacao_PrecificacaoDAO lista = (Programacao_PrecificacaoDAO)Session["listaProduto" + urlSessao()];
            if (lista.itensFim == 0)
                lista.itensFim = lista.qtdePorPagina;

            if (lista != null)
            {
                if (lista.Itens.Count > 0)
                {
                    divAvisoItens.Visible = false;
                    int nfim = (lista.itensFim > lista.Itens.Count ? lista.Itens.Count : lista.itensFim);

                    lblRegistros.Text = (lista.itensInicio + 1) + " ate " + nfim + " de " + lista.Itens.Count + " Incluido(s) ";
                    lblRegistrosSeleciona.Text = lblRegistros.Text;

                    gridItens.DataSource = lista.Itens;
                    gridItens.DataBind();

                    gridItensSelecionados.DataSource = lista.Itens;
                    gridItensSelecionados.DataBind();
                }
                else
                {
                    divAvisoItens.Visible = true;
                }



            }
        }

        protected void HabilitarCampos(bool enable)
        {
            //EnabledControls(conteudo, enable);
            EnabledControls(tabItens, enable);
            EnabledControls(TabSelecionarItens, enable);
            EnabledControls(cabecalho, enable);


        }
        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProgramacaoPrecificacaoProdutos.aspx");
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            modalConfirmaExcluir.Show();
        }


        protected bool validaCamposObrigatorios()
        {


            if (validaCampos(cabecalho) && validaCampos(conteudo))
                return true;
            else
                return false;
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                validaCamposObrigatorios();
                CarregarDadosObj();
                Programacao_PrecificacaoDAO obj = (Programacao_PrecificacaoDAO)Session["listaProduto" + urlSessao()];
                obj.salvar(status.Equals("incluir"));
                status = "visualizar";

                showMessage("SALVO COM SUCESSO", false);

                CarregarDados();
                carregabtn(pnBtn);
                HabilitarCampos(false);

            }
            catch (Exception err)
            {

                showMessage(err.Message, true);
            }
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProgramacaoPrecificacaoProdutos.aspx");
        }

        protected override bool campoDesabilitado(Control campo)
        {
            if (campo.ID != null)
            {
                String[] campos = { "txtId",
                                   "txtUsuario",
                                   "txtDataCadastro",
                                   "txtDescricaoItem",
                                    "txtstatus",
                                    "txtDescricaoListaPadrao"
                                    ,"txtGrupo"
                                    ,"txtSubGrupo"
                                    ,"txtDepartamento"
                                    ,"txtCozinha"
                                    ,"txtAgrupamento"
                                    ,"txtTipoProducao"

                              };

                return existeNoArray(campos, campo.ID.Trim());
            }
            else
                return false;
        }

        protected override bool campoObrigatorio(Control campo)
        {
            String[] campos = { "txtDescricao",
                                "txtDataInicio"
                                     };
            return existeNoArray(campos, campo.ID + "");
        }
        protected void btnOkError_Click(object sender, EventArgs e)
        {

            if (lblErroPanel.Text.Equals("Excluido com Sucesso"))
            {

                Response.Redirect("ProgramacaoPrecificacaoProdutos.aspx");
            }
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




        protected void imgPlu_Click(object sender, ImageClickEventArgs e)
        {
            if (!txtPlu.Text.Trim().Equals(""))
            {
                txtDescricaoItem.Text = Conexao.retornaUmValor("Select descricao from mercadoria where plu ='" + txtPlu.Text + "'", null);
                if (txtDescricaoItem.Text.Equals(""))
                {
                    showMessage("PRODUTO NÃO ENCONTRADO", true);
                }
                else
                {
                    addItensDig.DefaultButton = "ImgBtnAddItens";
                    ImgBtnAddItens.Focus();
                }
            }
            else
            {
                exibeLista("PLURapido");
            }
        }

        protected void ImgBtnAddItens_Click(object sender, EventArgs e)
        {

            try
            {
                if (!txtPlu.Text.Equals(""))
                {
                    Programacao_PrecificacaoDAO lista = (Programacao_PrecificacaoDAO)Session["listaProduto" + urlSessao()];
                    CarregarDadosObj();
                    Programacao_Precificacao_ItemDAO item = new Programacao_Precificacao_ItemDAO();
                    item.plu = txtPlu.Text;
                    item.descricao = txtDescricaoItem.Text;
                    lista.addItens(item);
                    txtPlu.Text = "";
                    txtDescricaoItem.Text = "";
                    txtPlu.Focus();
                    addItensDig.DefaultButton = "imgPlu";
                    carregarGrids();
                }
            }
            catch (Exception err)
            {

                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;

            }

        }



        protected void imgFornecedor_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("Fornecedor");
        }
        protected void imgBtnListaPadrao_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("ListaPadrao");
        }
        protected void imgBtnGrupo_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("Grupo");
        }
        protected void imgBtnSubGrupo_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("SubGrupo");
        }
        protected void imgBtnDepartamento_Click(object sender, ImageClickEventArgs e)
        {

            exibeLista("Departamento");
        }

        protected void Img_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            TxtPesquisaLista.Text = "";
            String campo = "";
            switch (btn.ID)
            {

                case "imgBtnFilial":
                    campo = "Filial";
                    break;

            }

            exibeLista(campo);


        }
        protected void exibeLista(String campo)
        {

            Session.Remove("campoLista" + urlSessao());
            Session.Add("campoLista" + urlSessao(), campo);
            String sqlLista = "";

            switch (campo)
            {
                case "PLU":
                case "PLURapido":
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
                case "Grupo":
                    lbltituloLista.Text = "Escolha um  Grupo";
                    sqlLista = "select Codigo_Grupo as codigo,Descricao_Grupo as Grupo from Grupo where descricao_grupo like '%" + TxtPesquisaLista.Text + "%' or codigo_grupo like '" + TxtPesquisaLista.Text + "%'";

                    break;
                case "SubGrupo":
                    lbltituloLista.Text = "Escolha um  SubGrupo";
                    sqlLista = "Select codigo_subgrupo as Codigo ,descricao_grupo as Grupo , descricao_subgrupo as SubGrupo  from  W_BR_CADASTRO_DEPARTAMENTO " +
                                    " Where (codigo_subgrupo like '" + TxtPesquisaLista.Text + "%' or descricao_subgrupo like '%" + TxtPesquisaLista.Text + "%' or codigo_subgrupo like '%" + TxtPesquisaLista.Text + "%' ) ";
                    if (!txtGrupo.Text.Equals(""))
                    {
                        sqlLista += " and descricao_grupo ='" + txtGrupo.Text + "'";

                    }
                    sqlLista += " group by codigo_subgrupo, descricao_subgrupo, descricao_grupo ";


                    break;
                case "Departamento":
                    lbltituloLista.Text = "Escolha um  Departamento";
                    sqlLista = "Select codigo_departamento as Codigo ,descricao_grupo as Grupo, descricao_subgrupo as SubGrupo,descricao_departamento as Departamento   from  W_BR_CADASTRO_DEPARTAMENTO " +
                                    " Where (codigo_departamento like '" + TxtPesquisaLista.Text + "%' or descricao_departamento like '%" + TxtPesquisaLista.Text + "%' or descricao_grupo like '%" + TxtPesquisaLista.Text + "%'  or  descricao_subgrupo like '%" + TxtPesquisaLista.Text + "%' ) ";
                    if (!txtGrupo.Text.Equals(""))
                    {
                        sqlLista += " and descricao_grupo ='" + txtGrupo.Text + "'";

                    }
                    if (!txtSubGrupo.Text.Equals(""))
                    {
                        sqlLista += " and descricao_subgrupo='" + txtSubGrupo.Text + "'";
                    }

                    sqlLista += " group by codigo_departamento, descricao_departamento,descricao_subgrupo, descricao_grupo ";

                    break;
                case "ListaPadrao":
                    lbltituloLista.Text = "Escolha a Lista de produtos";
                    sqlLista = "Select Id, Descricao , Qtde_Itens = (Select count(*) from LISTA_PADRAO_ITENS WHERE ID_LISTA = ID)from LISTA_PADRAO";
                    break;
                case "Filial":
                    lbltituloLista.Text = "Escolha uma  Filial";
                    sqlLista = " select Filial='TODAS' UNION ALL  select Filial from Filial where filial like '%" + TxtPesquisaLista.Text + "%' ";

                    break;
                case "Agrupamento":
                    lbltituloLista.Text = "Escolha um  Agrupamento";
                    sqlLista = "Select Agrupamento from agrupamento_producao " +
                                    " Where (Agrupamento like '%" + TxtPesquisaLista.Text + "%')";


                    break;
                case "TipoProducao":
                    lbltituloLista.Text = "Escolha um  Tipo";
                    sqlLista = "Select TIPO ='PRODUCAO' UNION SELECT 'ENCOMENDA' UNION SELECT 'MISTO'";

                    break;
            }
            User usr = (User)Session["User"];
            GridLista.DataSource = Conexao.GetTable(sqlLista, null, true);
            GridLista.DataBind();

            modalLista.Show();
            TxtPesquisaLista.Focus();
        }
        protected void ddlLinha_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarMercadorias();
        }

        protected void carregarMercadorias()
        {
            try
            {
                lblMercadoriaLista.Text = "Inclusão de Produto";

                User usr = (User)Session["user"];

                ListaPrecificacaoPesquisa pesq = (ListaPrecificacaoPesquisa)Session["pesq" + urlSessao()];
                if (pesq == null)
                    pesq = new ListaPrecificacaoPesquisa(usr);

                int nPorPagina = Funcoes.intTry(Funcoes.valorParametro("ITENS_POR_PAG", usr));
                pesq.grupo = txtGrupo.Text;
                pesq.subgrupo = txtSubGrupo.Text;
                pesq.departamento = txtDepartamento.Text;
                pesq.linha = ddlLinha.Text;
                pesq.plu_descricao = txtfiltromercadoria.Text;
                pesq.gridInicio = 0;
                pesq.gridFim = nPorPagina;
                pesq.codigo_lista = txtcodListaPadrao.Text;

                pesq.atualizarPesquia();
                Session.Remove("pesq" + urlSessao());
                Session.Add("pesq" + urlSessao(), pesq);

                atualizaGridPesquisa();

            }
            catch (Exception err)
            {

                showMessage(err.Message, true);
            }



        }

        private void atualizaGridPesquisa()
        {
            User usr = (User)Session["user"];
            int nPorPagina = Funcoes.intTry(Funcoes.valorParametro("ITENS_POR_PAG", usr));
            ListaPrecificacaoPesquisa pesq = (ListaPrecificacaoPesquisa)Session["pesq" + urlSessao()];
            gridMercadoria1.DataSource = pesq.Resultado();
            gridMercadoria1.DataBind();
            int nFim = (pesq.ItensEncontrados.Count > nPorPagina ? pesq.gridFim : pesq.ItensEncontrados.Count);

            lblResultadoPesquisaIncluir.Text = (pesq.gridInicio + 1) + " ate " + nFim + " de " + pesq.ItensEncontrados.Count + " Encontrado(s) ";
            if (pesq.ItensSelecionados.Count > 0)
            {
                foreach (GridViewRow row in gridMercadoria1.Rows)
                {
                    String plu = row.Cells[1].Text;

                    if (pesq.ItensSelecionados.Count(i => i.PLU.Equals(plu)) > 0)
                    {
                        CheckBox chk = (CheckBox)row.FindControl("chkSelecionaItem");
                        chk.Checked = true;
                    }
                }
            }

        }
        protected void txtfiltromercadoria_TextChanged(object sender, EventArgs e)
        {

            carregarMercadorias();

        }

        protected void txtcodListaPadrao_TextChanged(object sender, EventArgs e)
        {
            if (!txtcodListaPadrao.Text.Equals(""))
            {
                txtDescricaoListaPadrao.Text = Conexao.retornaUmValor("Select descricao from LISTA_PADRAO WHERE ID =" + txtcodListaPadrao.Text, null);
            }
            else
            {
                txtDescricaoListaPadrao.Text = "";
            }
            carregarMercadorias();
        }
        protected void ImgPesquisaMercadoria_Click(object sender, ImageClickEventArgs e)
        {
            carregarMercadorias();
        }
        protected void imgLimpar_Click(object sender, ImageClickEventArgs e)
        {
            limparSelecaoMercadoria();
            carregarMercadorias();
        }
        protected void limparSelecaoMercadoria()
        {
            txtGrupo.Text = "";
            txtSubGrupo.Text = "";
            txtDepartamento.Text = "";

            ddlLinha.Text = "";

            txtfiltromercadoria.Text = "";


        }
        protected void chkSelecionaMercadoria_CheckedChanged(object sender, EventArgs e)
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


        protected void imgBtnIncluirSelecionados_Click(object sender, ImageClickEventArgs e)
        {
            selecionadosPag();
            CheckBox chkTodos = (CheckBox)gridMercadoria1.HeaderRow.FindControl("chkSeleciona");

            if (chkTodos.Checked)
            {

                ListaPrecificacaoPesquisa pesq = (ListaPrecificacaoPesquisa)Session["pesq" + urlSessao()];

                btnTodosItensSelecionados.Text = pesq.ItensEncontrados.Count.ToString() + " Itens Encontrados";
                int qtdeSelec = pesq.ItensSelecionados.Count;


                btnApenasTela.Text = qtdeSelec.ToString() + " Itens Selecionados";

                modalConfirmaTodosSelecionado.Show();

            }
            else
            {
                verificaSelecionados(true);
            }
        }
        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {
            String itemLista = (String)Session["campoLista" + urlSessao()];
            switch (itemLista)
            {

                case "PLU":
                    txtfiltromercadoria.Text = ListaSelecionada(1);
                    break;
                case "PLURapido":
                    txtPlu.Text = ListaSelecionada(1);
                    txtDescricaoItem.Text = ListaSelecionada(3);
                    break;
                case "Grupo":
                    txtGrupo.Text = ListaSelecionada(2);
                    break;
                case "SubGrupo":
                    txtSubGrupo.Text = ListaSelecionada(3);
                    txtGrupo.Text = ListaSelecionada(2);
                    break;
                case "Departamento":
                    txtDepartamento.Text = ListaSelecionada(4);
                    txtSubGrupo.Text = ListaSelecionada(3);
                    txtGrupo.Text = ListaSelecionada(2);
                    break;
                case "ListaPadrao":
                    txtcodListaPadrao.Text = ListaSelecionada(1);
                    txtDescricaoListaPadrao.Text = ListaSelecionada(2);
                    break;
                case "Filial":
                    txtFilial.Text = ListaSelecionada(1);
                    break;

            }
            carregarMercadorias();

            modalLista.Hide();


        }
        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalLista.Hide();
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
            String itemLista = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];


            exibeLista(itemLista);
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
        protected void btnPag_Click(object sender, EventArgs e)
        {
            CarregarDadosObj();
            Programacao_PrecificacaoDAO obj = (Programacao_PrecificacaoDAO)Session["listaProduto" + urlSessao()];

            int nFim = obj.qtdeItens;
            int nPorPagina = obj.qtdePorPagina;


            Button btn = (Button)sender;
            if (btn.ID.Contains("btnPagInicio"))
            {
                obj.itensInicio = 0;
                obj.itensFim = nPorPagina;
            }
            else if (btn.ID.Contains("btnPagAnterio"))
            {
                obj.itensInicio -= nPorPagina;
                obj.itensFim = obj.itensInicio + nPorPagina;
                if (obj.itensFim < 0)
                {
                    obj.itensInicio = 0;
                    obj.itensFim = nPorPagina;
                }
            }
            else if (btn.ID.Contains("btnPagProximo"))
            {
                obj.itensInicio += nPorPagina;
                obj.itensFim = obj.itensInicio + nPorPagina;
                if (obj.itensFim > nFim)
                {
                    decimal pagInicio = (decimal.Truncate(nFim / nPorPagina) * nPorPagina);
                    obj.itensInicio = Convert.ToInt32(pagInicio);
                    if (obj.itensInicio == nFim)
                        obj.itensInicio = (nFim - obj.qtdePorPagina);
                    obj.itensFim = nFim;
                }
            }
            else if (btn.ID.Contains("btnPagFim"))
            {
                decimal pagInicio = (decimal.Truncate(nFim / nPorPagina) * nPorPagina);
                obj.itensInicio = Convert.ToInt32(pagInicio);
                if (obj.itensInicio == nFim)
                    obj.itensInicio = (nFim - obj.qtdePorPagina);

                obj.itensFim = nFim;
            }
            carregarGrids();
            if (status.Equals("visualizar"))
            {
                HabilitarCampos(false);
            }
        }



        protected void btnPagPesq_Click(object sender, EventArgs e)
        {
            selecionadosPag();
            Programacao_PrecificacaoDAO lista = (Programacao_PrecificacaoDAO)Session["listaProduto" + urlSessao()];
            ListaPrecificacaoPesquisa obj = (ListaPrecificacaoPesquisa)Session["pesq" + urlSessao()];
            int nFim = obj.ItensEncontrados.Count;
            int nPorPagina = 0; //lista.qtdePorPagina;


            Button btn = (Button)sender;
            if (btn.ID.Contains("btnPagInicio"))
            {
                obj.gridInicio = 0;
                obj.gridFim = nPorPagina;
            }
            else if (btn.ID.Contains("btnPagAnterio"))
            {
                obj.gridInicio -= nPorPagina;
                obj.gridFim = obj.gridInicio + nPorPagina;
                if (obj.gridInicio < 0)
                {
                    obj.gridInicio = 0;
                    obj.gridFim = nPorPagina;
                }


            }
            else if (btn.ID.Contains("btnPagProximo"))
            {

                obj.gridInicio += nPorPagina;
                obj.gridFim = obj.gridInicio + nPorPagina;
                if (obj.gridFim > nFim)
                {
                    decimal pagInicio = (decimal.Truncate(nFim / nPorPagina) * nPorPagina);
                    obj.gridInicio = Convert.ToInt32(pagInicio);
                    obj.gridFim = nFim;
                }
            }
            else if (btn.ID.Contains("btnPagFim"))
            {
                decimal pagInicio = (decimal.Truncate(nFim / nPorPagina) * nPorPagina);
                obj.gridInicio = Convert.ToInt32(pagInicio);
                obj.gridFim = nFim;
            }

            Session.Remove("pesq" + urlSessao());
            Session.Add("pesq" + urlSessao(), obj);
            atualizaGridPesquisa();

        }

        protected void imgBtnCancelar_Click(object sender, ImageClickEventArgs e)
        {
            visualizarIncluirItens(false);
        }
        private void visualizarIncluirItens(bool visualizar)
        {
            //divIncluirItens.Visible = !visualizar;
            //divSelecionarAdditens.Visible = visualizar;
            //divAvisoItens.Visible = !visualizar;
            if (gridItens.Rows.Count == 0)
            {
                divAvisoItens.Visible = true;
            }
            else
            {
                divAvisoItens.Visible = false;
            }
        }

        protected void btnTodosSelecionados_Click(object sender, EventArgs e)
        {
            verificaSelecionados(false);
        }

        protected void btnApenasTela_Click(object sender, EventArgs e)
        {
            verificaSelecionados(true);
        }

        private void verificaSelecionados(bool grid)
        {
            try
            {
                selecionadosPag();
                CarregarDadosObj();
                ListaPrecificacaoPesquisa pesq = (ListaPrecificacaoPesquisa)Session["pesq" + urlSessao()];
                Programacao_PrecificacaoDAO lista = (Programacao_PrecificacaoDAO)Session["listaProduto" + urlSessao()];


                if (grid)
                {

                    foreach (ListaPrecificacaoPesquisaItem itemSele in pesq.ItensSelecionados)
                    {
                        Programacao_Precificacao_ItemDAO item = new Programacao_Precificacao_ItemDAO
                        {
                            plu = itemSele.PLU,
                            descricao = itemSele.Descricao
                        };
                        lista.addItens(item);

                    }
                }
                else
                {

                    foreach (ListaPrecificacaoPesquisaItem item in pesq.ItensEncontrados)
                    {
                        Programacao_Precificacao_ItemDAO nitem = new Programacao_Precificacao_ItemDAO
                        {
                            plu = item.PLU,
                            descricao = item.Descricao
                        };
                        lista.addItens(nitem);
                    }

                }
                pesq.ItensSelecionados.Clear();
                atualizaGridPesquisa();
                carregarGrids();
            }
            catch (Exception err)
            {
                showMessage(err.Message, true);
            }



        }






        protected void gridItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            lblPluExcluir.Text = gridItens.Rows[index].Cells[1].Text;
            lblPluDescricao.Text = gridItens.Rows[index].Cells[2].Text;
            lblFornecedorExcluir.Text = "";
            lblFornecedorDescricao.Text = "";
            modalExcluirItem.Show();
        }

        protected void gridPesquisaFornecedor_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void imgBtnConfirmaExluirItem_Click(object sender, ImageClickEventArgs e)
        {
            Programacao_PrecificacaoDAO lista = (Programacao_PrecificacaoDAO)Session["listaProduto" + urlSessao()];
            if (!lblPluExcluir.Text.Equals(""))
            {
                lista.excluirItem(lblPluExcluir.Text);

            }
            carregarGrids();
        }

        protected void imgBtnCancelaExluirItem_Click(object sender, ImageClickEventArgs e)
        {
            modalExcluirItem.Hide();
        }






        protected void btnCancelarInativar_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void imgBtnConfirmaManter_Click(object sender, ImageClickEventArgs e)
        {
            Programacao_PrecificacaoDAO listaManter = (Programacao_PrecificacaoDAO)Session["manterLista"];
            Session.Remove("listaProduto" + urlSessao());
            Session.Add("listaProduto" + urlSessao(), listaManter);
            novaLista();
            Session.Remove("manterLista");
        }

        protected void imgBtnCancelarManter_Click(object sender, ImageClickEventArgs e)
        {
            Session.Remove("manterLista");
            User usr = (User)Session["User"];
            int tipo = Funcoes.intTry(Request["tipo"].ToString());
            Programacao_PrecificacaoDAO lista = new Programacao_PrecificacaoDAO(tipo);
            Session.Remove("listaProduto" + urlSessao());
            Session.Add("listaProduto" + urlSessao(), lista);

            novaLista();
        }



        protected void selecionadosPag()
        {
            ListaPrecificacaoPesquisa pesq = (ListaPrecificacaoPesquisa)Session["pesq" + urlSessao()];
            foreach (GridViewRow item in gridMercadoria1.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                if (chk.Checked)
                {
                    String plu = item.Cells[1].Text;
                    ListaPrecificacaoPesquisaItem itemPes = pesq.ItensEncontrados.Find(obj => obj.PLU.Equals(plu));
                    if (!pesq.ItensSelecionados.Contains(itemPes))
                        pesq.ItensSelecionados.Add(itemPes);
                }
            }
        }

        protected void imgBtnCozinha_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("Cozinha");
        }

        protected void imgBtnAGrupamento_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("Agrupamento");
        }

        protected void imgBtnTipoProducao_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("TipoProducao");
        }

        protected void imgBtnImp_Click(object sender, ImageClickEventArgs e)
        {
            //RedirectNovaAba("ListaPadraoPrint.aspx?codLista=" + txtCodigo.Text);
        }

        protected void linkExportaExcel_Click(object sender, EventArgs e)
        {

            if (gridItens.Rows.Count > 0)
            {
                exportarExcel();
                showMessage("Exportado com Sucesso!", false);

            }
            else
            {
                showMessage("Incluia os itens para fazer a Exportação!", true);
            }
        }

        protected void exportarExcel()
        {
            try
            {



                String path = Server.MapPath("~/modulos/Estoque/pages/download/");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);


                String enderecoCompleto = path + @"\Programacao-Preco-" + txtDescricao.Text + "-" + txtDataInicio.Text.Replace("/", "-") + ".xlsx";
                if (File.Exists(enderecoCompleto))
                    File.Delete(enderecoCompleto);

                ExcelPackage package = new ExcelPackage(new FileInfo(enderecoCompleto));
                ExcelWorkbook workbook = package.Workbook;
                ExcelWorksheet sheet = null;
                if (workbook.Worksheets.Count(i => i.Name.Equals("Planilha 1")) > 0)
                    sheet = workbook.Worksheets["Planilha 1"];
                else
                    sheet = workbook.Worksheets.Add("Planilha 1");

                sheet.Cells["A1"].Value = "PLU";
                sheet.Cells["B1"].Value = "DESCRICAO";
                sheet.Column(2).Width = 50;
                sheet.Cells["C1"].Value = "PRECO";
                sheet.Cells["D1"].Value = "(EXCLUIR)";


                Programacao_PrecificacaoDAO lista = (Programacao_PrecificacaoDAO)Session["listaProduto" + urlSessao()];

                int row = 2;

                foreach (Programacao_Precificacao_ItemDAO item in lista.Itens)
                {
                    sheet.Cells[row, 1].Value = Double.Parse(item.plu);
                    sheet.Cells[row, 2].Value = item.descricao;
                    sheet.Cells[row, 3].Value = item.preco;
                    row++;
                }


                package.Save();

                RedirectNovaAba("~/code/BaixarArquivo.aspx?endereco=" + enderecoCompleto.Replace("\\", "/"));
            }
            catch (Exception err)
            {

                showMessage(err.Message, true);
            }
        }

        private bool UploadArquivo(String path)
        {
            FileArquivo.SaveAs((path
                    + FileArquivo.FileName));
            return true;
        }
        protected void btnUpLoad_Click(object sender, EventArgs e)
        {
            try
            {



                if (status.Equals("visualizar"))
                    throw new Exception("Para importar Clique antes em Editar");

                //String pasta = Server
                String path = Server.MapPath("~/modulos/Estoque/pages/uploads/");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                UploadArquivo(path);

                String[] files = Directory.GetFiles(path);

                foreach (String arq in files)
                {
                    lerExcel(arq);
                }

                foreach (String arq in files)
                {
                    File.Delete(arq);
                }
            }
            catch (Exception err)
            {

                showMessage(err.Message, true);
            }
        }
        private void lerExcel(String caminhoExcel)
        {
            Programacao_PrecificacaoDAO obj = (Programacao_PrecificacaoDAO)Session["listaProduto" + urlSessao()];
            String plu = "";
            String erros = "";
            String preco = "";
            int row = 0;
            int linhasImportadas = 0;
            int itensExcluidos = 0;
            try
            {


                using (var stream = File.Open(caminhoExcel, FileMode.Open, FileAccess.Read))
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {

                        do
                        {
                            while (reader.Read())
                            {
                                if (row > 0)
                                {
                                    try
                                    {


                                        if (!reader.IsDBNull(0))
                                        {

                                            plu = reader.GetDouble(0).ToString();
                                            try
                                            {
                                                preco = reader.GetDouble(2).ToString();

                                            }
                                            catch (Exception err)
                                            {
                                                throw err;
                                            }
                                            bool exclui = false;
                                            if (reader.FieldCount >= 4 && !reader.IsDBNull(3))
                                            {

                                                string comando = reader.GetString(3);
                                                if (comando.ToUpper().Equals("D") || comando.ToUpper().Equals("E"))
                                                {
                                                    exclui = true;

                                                }

                                            }
                                            if (exclui)
                                            {
                                                if (obj.excluirItem(plu))
                                                    itensExcluidos++;
                                            }
                                            else
                                            {
                                                String descricao = Conexao.retornaUmValor("Select descricao from mercadoria where plu ='" + plu + "'", null);
                                                obj.addItens(new Programacao_Precificacao_ItemDAO() { plu = plu, descricao = descricao, preco = Funcoes.decTry(preco) });
                                                linhasImportadas++;
                                            }

                                        }
                                    }
                                    catch (Exception err)
                                    {
                                        if (err.Message.Contains("Relacione ao menos uma Filial"))
                                            throw err;
                                        erros += "Erro importacao linha:" + (row + 1).ToString() + " Erro :" + err.Message + "<br> ";
                                    }

                                }
                                row++;
                                //plusPrecos += ";" + reader.GetDecimal(2);
                                // reader.GetDouble(0);
                            }
                        } while (reader.NextResult());



                    }
                }
                CarregarDados();

                if (erros.Length > 0)
                    throw new Exception(erros);
                else
                {
                    String msg = linhasImportadas.ToString() + " Linhas Importadas";
                    if (itensExcluidos > 0)
                    {
                        msg += "<br> " + itensExcluidos + " Itens Excluidos";
                    }
                    showMessage(msg, false);

                }

            }
            catch (Exception err)
            {

                showMessage(err.Message, true);
            }
        }

        protected void ImgBtnConfirmaExcluir_Click(object sender, ImageClickEventArgs e)
        {
            Programacao_PrecificacaoDAO obj = (Programacao_PrecificacaoDAO)Session["listaProduto" + urlSessao()];
            try
            {
                User usr = (User)Session["User"];
                obj.excluir(usr);
                modalConfirmaExcluir.Hide();
                showMessage("Excluido com Sucesso", true);

            }
            catch (Exception err)
            {

                showMessage(err.Message, true);
            }


        }

        protected void ImgBtnCancelaExcluir_Click(object sender, ImageClickEventArgs e)
        {
            modalConfirmaExcluir.Hide();
        }
    }
}