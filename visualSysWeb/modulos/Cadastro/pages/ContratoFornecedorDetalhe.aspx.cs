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

namespace visualSysWeb.modulos.Manutencao.pages
{
    public partial class ContratoFornecedorDetalhe : visualSysWeb.code.PagePadrao
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    status = "incluir";
                    Contrato_fornecedorDAO obj = new Contrato_fornecedorDAO();
                    txtdata_Cadastro.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    Session.Remove("cFornecedor" + urlSessao());
                    Session.Add("cFornecedor" + urlSessao(), obj);
                    carregarGrids();
                }
            }
            else
            {
                if (Request.Params["codigo"] != null)
                {
                    try
                    {
                        if (!IsPostBack)
                        {
                            String index = Request.Params["codigo"].ToString();
                            status = "visualizar";
                            Contrato_fornecedorDAO obj = new Contrato_fornecedorDAO(index, usr);
                            Session.Remove("cFornecedor" + urlSessao());
                            Session.Add("cFornecedor" + urlSessao(), obj);
                            carregarDados();
                        }

                    }
                    catch (Exception err)
                    {
                        showMessage(err.Message, true);
                    }
                }
            }
            carregabtn(pnBtn);
            if (status.Equals("visualizar"))
            {

                habilitarCampos(false);
            }
            else
            {
                habilitarCampos(true);

            }
        }

        private void habilitarCampos(bool enable)
        {

            if (ddlTipoReajuste.Text.ToUpper().Equals("ANO"))
            {
                lblDiaMesReajuste.Text = "Mes Reajuste";
            }
            else if (ddlTipoReajuste.Text.ToUpper().Equals("MES"))
            {
                lblDiaMesReajuste.Text = "Dia Reajuste";
            }

            EnabledControls(cabecalho, enable);
            EnabledControls(conteudo, enable);
            addItensDig.Visible = enable;
            pnAddFilial.Visible = enable;


        }


        private void limparCampos()
        {
            LimparCampos(cabecalho);

        }

        protected bool validaCamposObrigatorios()
        {
            Contrato_fornecedorDAO obj = (Contrato_fornecedorDAO)Session["cFornecedor" + urlSessao()];

            if (ddlTipoReajuste.Text.ToUpper().Equals("MES"))
            {
                int dia = 0;
                int.TryParse(txtDiaMesReajuste.Text, out dia);

                if (dia <= 0 || dia > 31)
                {
                    txtDiaMesReajuste.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Dia Invalido");
                }

            }
            else if (ddlTipoReajuste.Text.ToUpper().Equals("ANO"))
            {
                int mes = 0;
                int.TryParse(txtDiaMesReajuste.Text, out mes);

                if (mes <= 0 || mes > 12)
                {
                    txtDiaMesReajuste.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Mes Invalido");
                }
            }

            if (obj.arrFilias.Count == 0)
            {
                throw new Exception(" O Contrato Não foi Relacionado com uma Filial!");
            }

            if (obj.arrItens.Count == 0)
            {
                throw new Exception(" O Contrato Não foi Relacionado com nenhum item!");
            }
            verificaPrazos();
            bool itensZerados = false;
            foreach (GridViewRow item in gridItens.Rows)
            {
                TextBox txtItem = (TextBox)item.FindControl("txtValorItem");
                Decimal vlrQtde = 0;
                Decimal.TryParse(txtItem.Text, out vlrQtde);
                if (vlrQtde <= 0)
                {
                    txtItem.BackColor = System.Drawing.Color.Red;
                    itensZerados = true;
                }

            }
            if (itensZerados)
            {
                throw new Exception("Existe itens com valor inválido!!");
            }
          

            if (validaCampos(cabecalho))
                return true;
            else
                return false;
        }

        private void verificaPrazos()
        {
            txtprazo.BackColor = System.Drawing.Color.White;


            string[] arrLinha = txtprazo.Text.Split('/');

            int vParAnt = 0;
            foreach (String item in arrLinha)
            {
                if (item.Equals(""))
                {
                    txtprazo.BackColor = System.Drawing.Color.Red;
                    throw new Exception("prazo informado de forma incorreta, Separe as parcelas com Barra(/) Ex 30/60/90  ");
                }
                int vI;
                if (!int.TryParse(item, out vI))
                {
                    txtprazo.BackColor = System.Drawing.Color.Red;
                    throw new Exception("prazo informado de forma incorreta, Digite apenas numeros e separe as parcelas com Barra(/) Ex 30/60/90  ");
                }
                if (vI <= vParAnt)
                {
                    txtprazo.BackColor = System.Drawing.Color.Red;
                    throw new Exception("parcela informada de forma incorreta, Quantidade de dias menor que a parcela anterior ");
                }
                vParAnt = vI;
            }


        }

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array
            String[] campos = { "txtfornecedor",
                                    "txtdata_validade",
                                    "txtprazo",
                                    "txtData_inicio",
                                    "txtDescricao",
                                    "txtPrazoEntrega"
                                     };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array
            String[] campos = { "txtid_contrato",
                                    "txtdata_Cadastro",
                                    "txtDescricaoItem",
                                    "txtUndItem",
                                    "",
                                     };

            if (status.Equals("editar"))
            {
                campos[2] = "txtfornecedor";
                campos[3] = "imgFornecedor";
            }



            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("ContratoFornecedorDetalhe.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            status = "editar";
            editar(pnBtn);
            habilitarCampos(true);
            carregarGrids();
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ContratoFornecedor.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            modalConfirmaExcluirContrato.Show();
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
                return;
            }
            string script = "SetUniqueRadioButton('GridLista.*GrlistaItem',this)";
            rdo.Attributes.Add("onclick", script);
        }
        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaCamposObrigatorios())
                {

                    carregarDadosObj();
                    Contrato_fornecedorDAO obj = (Contrato_fornecedorDAO)Session["cFornecedor" + urlSessao()];
                    obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                    showMessage("Salvo com Sucesso", false);
                    status = "visualizar";
                    carregarDados();
                    habilitarCampos(false);
                    visualizar(pnBtn);
                }
                else
                {
                    showMessage("Campo Obrigatorio não preenchido",true);
                  
                }
            }
            catch (Exception err)
            {
                showMessage(err.Message, true);
            }
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ContratoFornecedor.aspx");//colocar endereco pagina de pesquisa
        }
        //--Atualizar DaoForm 
        private void carregarDados()
        {
            Contrato_fornecedorDAO obj = (Contrato_fornecedorDAO)Session["cFornecedor" + urlSessao()];
            txtid_contrato.Text = obj.id_contrato.ToString();
            txtfornecedor.Text = obj.fornecedor.ToString();
            txtdata_Cadastro.Text = obj.data_CadastroBr();
            txtdata_validade.Text = obj.data_validadeBr();
            txtprazo.Text = obj.prazo.ToString();
            txtQtdeminima.Text = obj.qtde_minima.ToString();
            txtDescricao.Text = obj.descricao;
            txtData_inicio.Text = obj.data_inicioBr();
            ddlTipoReajuste.Text = obj.tipo_reajuste;
            txtDiaMesReajuste.Text = obj.dia_mes_reajuste.ToString();

            txtFormaDeReajuste.Text = obj.forma_reajuste;
            txtPrazoEntrega.Text = obj.prazo_entrega.ToString();

            int nInicio = obj.gridInicio;
            int nFim = (obj.arrItens.Count > 100 ? obj.gridFim : obj.arrItens.Count);
            int total = obj.arrItens.Count;
            int qtdeDig = 0;
            bool selecionados = false;
            if (obj.itensDigitados)
            {
                qtdeDig = obj.qtdeItensDigitados;
                nFim = (qtdeDig > 100 ? obj.gridFim : qtdeDig);
                selecionados = true;
            }

            lblRegistros.Text = (nInicio + 1) + " ate " + nFim + " de " + (selecionados ? qtdeDig + " Selecionados de " : "") + total + " Registro(s) Adicionado(s)";

            gridItens.DataSource = obj.tbItens();
            gridItens.DataBind();

            gridMercadoriasSelecionadas.DataSource = obj.tbItens();
            gridMercadoriasSelecionadas.DataBind();


            gridFilial.DataSource = obj.tbFiliais();
            gridFilial.DataBind();


            if (TabContainer1.ActiveTabIndex == 2)
            {

                carregarItensFornecedor();
            }
        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            Contrato_fornecedorDAO obj = (Contrato_fornecedorDAO)Session["cFornecedor" + urlSessao()];
            obj.id_contrato = txtid_contrato.Text;
            obj.fornecedor = txtfornecedor.Text;
            obj.data_Cadastro = (txtdata_Cadastro.Text.Equals("") ? new DateTime() : DateTime.Parse(txtdata_Cadastro.Text));
            obj.data_validade = (txtdata_validade.Text.Equals("") ? new DateTime() : DateTime.Parse(txtdata_validade.Text));
            obj.prazo = txtprazo.Text;
            Decimal.TryParse(txtQtdeminima.Text, out obj.qtde_minima);
            obj.descricao = txtDescricao.Text;
            DateTime.TryParse(txtData_inicio.Text, out obj.data_inicio);

            obj.tipo_reajuste = ddlTipoReajuste.Text;
            int.TryParse(txtDiaMesReajuste.Text, out obj.dia_mes_reajuste);
            obj.forma_reajuste = txtFormaDeReajuste.Text;
            int.TryParse(txtPrazoEntrega.Text, out obj.prazo_entrega);

            if (obj.arrItens.Count > 0)
            {
                foreach (GridViewRow rw in gridItens.Rows)
                {
                    TextBox txt = (TextBox)rw.FindControl("txtValorItem");
                    Contrato_fornecedor_itemDAO item = (Contrato_fornecedor_itemDAO)obj.arrItens[rw.RowIndex];
                    Decimal.TryParse(txt.Text, out item.vlr);
                }
            }
            Session.Remove("cFornecedor" + urlSessao());
            Session.Add("cFornecedor" + urlSessao(), obj);
        }


        protected void lista_click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            String sqlLista = "";

            switch (btn.ID)
            {
                case "idBotao":
                    sqlLista = "Query de pesquisa com no minimo 2campos";
                    //lbllista.Text = "Pagamentos";
                    //camporeceber = "txtPagamento";
                    break;
            }
            User usr = (User)Session["User"];

        }

        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                showMessage("Registro não pode ser Excluido",true);
                limparCampos();
                pesquisar(pnBtn);
            }
            catch (Exception err)
            {
               
            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {

        }


        protected void Img_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            TxtPesquisaLista.Text = "";
            Session.Remove("camporecebe" + urlSessao());
            switch (btn.ID)
            {
                case "imgFornecedor":
                    Session.Add("camporecebe" + urlSessao(), "Fornecedor");
                    break;
                case "imgPlu":
                    Session.Add("camporecebe" + urlSessao(), "plu");
                    break;
                    
            }

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
                    sqlLista = "Select fornecedor,Razao_social,CNPJ,Cidade,UF  from fornecedor " +
                                  " Where (isnull(tipo_fornecedor,0) =0 ) " +
                                  " and ( (fornecedor like '%" + TxtPesquisaLista.Text + "%')" +
                                   " or(Razao_social like '%" + TxtPesquisaLista.Text + "%') " +
                                   " or (REPLACE(replace(replace(cnpj,'.',''),'/',''),'-','') like '%" + TxtPesquisaLista.Text.Replace(".", "").Replace("/", "").Replace("-", "") + "%')" +
                                   " or (Cidade like '%" + TxtPesquisaLista.Text + "%') " +
                                   " or (UF='" + TxtPesquisaLista.Text + "'))";
                    lbllista.Text = "Escolha um Fornecedor";
                    break;
                case "plu":
                    sqlLista = "Select plu, descricao, und from mercadoria where plu ='" + TxtPesquisaLista.Text + "'" +
                        " or descricao like '%" + txtPesquisaItem.Text + "%'";
                        
                    lbllista.Text = "Escolha um Produto";
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
                    String listaAtual = (String)Session["camporecebe" + urlSessao()];
                    Session.Remove("camporecebe" + urlSessao());

                    if (listaAtual.Equals("Fornecedor"))
                    {

                        txtfornecedor.Text = ListaSelecionada(1);
                        carregarItensFornecedor();

                    }
                    if(listaAtual.Equals("plu"))
                    {
                        txtPlu.Text = listaAtual;
                        txtDescricao.Text = ListaSelecionada(2);
                        txtUndItem.Text = ListaSelecionada(3);
                        txtValor.Focus();
                        addItensDig.DefaultButton = "imgBtnAddItemRapido";
                    }
                    modalPnFundo.Hide();
                    carregarGrids();

                }
                catch (Exception err)
                {

                    lbllista.Text = err.Message;
                }
            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                modalPnFundo.Show();
            }
        }
        private void carregarGrids()
        {
            Contrato_fornecedorDAO obj = (Contrato_fornecedorDAO)Session["cFornecedor" + urlSessao()];

            gridItens.DataSource = obj.tbItens();
            gridItens.DataBind();

            gridMercadoriasSelecionadas.DataSource = obj.tbItens();
            gridMercadoriasSelecionadas.DataBind();

            gridFilial.DataSource = obj.tbFiliais();
            gridFilial.DataBind();

        }
        protected void ImgBtnAddItens_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
              
                lblErroIncluirItem.Text = "";
                txtfornecedor.BackColor = System.Drawing.Color.White;
                if (txtfornecedor.Text.Equals(""))
                {
                    txtfornecedor.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Escolha um Fornecedor!");
                }

                Contrato_fornecedorDAO obj = (Contrato_fornecedorDAO)Session["cFornecedor" + urlSessao()];
                if (obj.arrFilias.Count == 0)
                {
                    throw new Exception("Informe as Filiais que o contrato vai atender!");
                }
                carregarDadosObj();
                TabContainer1.ActiveTabIndex = 2;

            }
            catch (Exception err)
            {
                showMessage(err.Message, true);
            }
        }
        protected void ImgBtnExcluirItens_Click(object sender, ImageClickEventArgs e)
        {
            lblItemExcluir.Text = pluSelecionado();
            modalConfirmaExcluir.Show();

        }

        private string pluSelecionado()
        {
            String plu = "";
            foreach (GridViewRow row in gridItens.Rows)
            {
                RadioButton rdo = (RadioButton)row.FindControl("RdoItem");
                if (rdo.Checked)
                {
                    plu = row.Cells[1].Text;

                    break;
                }
            }
            return plu;
        }

        protected void btnConfirmarExcluir_Click(object sender, ImageClickEventArgs e)
        {
            Contrato_fornecedorDAO obj = (Contrato_fornecedorDAO)Session["cFornecedor" + urlSessao()];
            if (lblFilialExcluir.Text.Equals(""))
            {
                obj.removeItem(lblItemExcluir.Text);
            }
            else
            {
                obj.removeFilial(lblFilialExcluir.Text);

            }
            Session.Remove("cFornecedor" + urlSessao());
            Session.Add("cFornecedor" + urlSessao(), obj);
            carregarGrids();
            lblItemExcluir.Text = "";
            lblFilialExcluir.Text = "";
        }

        protected void btnCancelarExcluir_Click(object sender, ImageClickEventArgs e)
        {

            lblItemExcluir.Text = "";
            lblFilialExcluir.Text = "";
            modalConfirmaExcluir.Hide();
        }

        protected void gridMercadoriasSelecionadas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow rw = gridMercadoriasSelecionadas.Rows[index];

            lblFilialExcluir.Text = "";
            lblItemExcluir.Text = rw.Cells[0].Text;

            modalConfirmaExcluir.Show();
        }

        protected void gridFilial_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow rw = gridFilial.Rows[index];

            lblFilialExcluir.Text = rw.Cells[1].Text;
            lblItemExcluir.Text = pluSelecionado();

            modalConfirmaExcluir.Show();

        }

        private void exibeListaIncluir()
        {
            String strInclui = (String)Session["campoInclui" + urlSessao()];
            String sql = "";
            Contrato_fornecedorDAO obj = (Contrato_fornecedorDAO)Session["cFornecedor" + urlSessao()];

            if (strInclui.Equals("itens"))
            {


                sql = " Select m.PLU,m.Descricao,m.Preco, m.UND, d.descricao_departamento from mercadoria as m " +
                        "    inner join W_BR_CADASTRO_DEPARTAMENTO as d on m.Codigo_departamento = d.codigo_departamento " +
                        "    inner join fornecedor_departamento as fd on fd.codigo_departamento =d.codigo_departamento " +
                        " where fd.fornecedor ='" + txtfornecedor.Text + "'  and " +
                           " ( " +
                             " (m.plu like '%" + txtPesquisaItem.Text + "%')" +
                             " or(m.descricao like '%" + txtPesquisaItem.Text + "%' )" +
                             " or (d.descricao_departamento like '%" + txtPesquisaItem.Text + "%')" +
                             ")" +

                       "  order by d.descricao_departamento, m.descricao ";
            }
            else if (strInclui.Equals("filiais"))
            {
                sql = " Select Filial, CNPJ, Razao_social  from filial where filial like '%" + txtPesquisaItem.Text + "%'";
            }

            gridAddItens.DataSource = Conexao.GetTable(sql, null, false);
            gridAddItens.DataBind();
            modalAddItem.Show();


        }

        protected void GridItens_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('gridItens.*GrItem',this)";
            rdo.Attributes.Add("onclick", script);

        }

        protected void RdoItem_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            GridViewRow rw = (GridViewRow)rdo.Parent.Parent;
            rw.RowState = DataControlRowState.Selected;
            rdo.Focus();


        }

        private int itemSelecionado()
        {
            foreach (GridViewRow item in gridItens.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoItem");

                if (rdo != null)
                {
                    if (rdo.Checked)
                    {
                        return item.RowIndex;
                    }
                }

            }
            return 0;
        }

        protected void ImgBtnAddFilias_Click(object sender, ImageClickEventArgs e)
        {
            try
            {


                carregarDadosObj();
                lblError.Text = "";

                Session.Remove("campoInclui" + urlSessao());
                Session.Add("campoInclui" + urlSessao(), "filiais");

                exibeListaIncluir();

            }
            catch (Exception err)
            {
                showMessage(err.Message, true);
            }
        }

        protected void imgBtnPesquisaItem_Click(object sender, ImageClickEventArgs e)
        {
            exibeListaIncluir();
        }
        //

        protected void imgBtnConfirmaAddItens_Click(object sender, ImageClickEventArgs e)
        {
            try
            {


                String strCampo = (String)Session["campoInclui" + urlSessao()];
                Contrato_fornecedorDAO obj = (Contrato_fornecedorDAO)Session["cFornecedor" + urlSessao()];

                if (strCampo.Equals("itens"))
                {
                }
                else if (strCampo.Equals("filiais"))
                {
                    int nItem = 0;
                    int.TryParse(lblCodItem.Text, out nItem);

                    foreach (GridViewRow ritem in gridAddItens.Rows)
                    {
                        CheckBox chk = (CheckBox)ritem.FindControl("chkSelecionaItem");
                        if (chk.Checked)
                        {
                            String filial = ritem.Cells[1].Text;
                            String cnpj = ritem.Cells[2].Text;
                            String razaoSocial = ritem.Cells[3].Text;
                            obj.addFilia(filial, cnpj, razaoSocial);
                        }

                    }
                }

                Session.Remove("cFornecedor" + urlSessao());
                Session.Add("cFornecedor" + urlSessao(), obj);
                carregarGrids();


                modalAddItem.Hide();


            }
            catch (Exception err)
            {
              showMessage("<br>" + err.Message,true);

            }
        }
        protected void imgBtnCancelarAddItens_Click(object sender, ImageClickEventArgs e)
        {
            carregarGrids();
            modalAddItem.Hide();

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

                }
            }
        }
        protected void chkSeleciona_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTodos = (CheckBox)sender;
            foreach (GridViewRow item in gridAddItens.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");

                if (chk != null)
                {
                    chk.Checked = chkTodos.Checked;
                    //incluirMercadoria(chk);
                }
            }

            modalAddItem.Show();
        }

        protected void imgBtnConfirmaExcluirContrato_Click(object sender, ImageClickEventArgs e)
        {
            Contrato_fornecedorDAO obj = (Contrato_fornecedorDAO)Session["cFornecedor" + urlSessao()];
            try
            {
                obj.excluir();
                showMessage("Excluido Com sucesso",false);
            }
            catch (Exception err)
            {
                showMessage(err.Message, true);
            }

        }
        protected void imgBtnCancelarExcluirContrato_Click(object sender, ImageClickEventArgs e)
        {

            modalConfirmaExcluirContrato.Hide();
        }

        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarGrupos(ddlGrupo.Text, "", "");

        }
        private void carregarGrupos(String grupo, String subGrupo, String departamento)
        {

            String sqlGrupo = "Select wd.codigo_grupo,wd.Descricao_grupo " +
                                       " from  W_BR_CADASTRO_DEPARTAMENTO  as wd " +
                                           " inner join fornecedor_departamento as fd on wd.codigo_departamento = fd.codigo_departamento " +
                                       " where fd.fornecedor = '" + txtfornecedor.Text + "'" +
                                       " group by wd.codigo_grupo,wd.Descricao_grupo";
            String sqlSubGrupo = "Select wd.codigo_subgrupo, wd.descricao_subgrupo  " +
                                   " from  W_BR_CADASTRO_DEPARTAMENTO  as wd " +
                                       " inner join fornecedor_departamento as fd on wd.codigo_departamento = fd.codigo_departamento " +
                                    " where fd.fornecedor = '" + txtfornecedor.Text + "'" +
                                   (!ddlGrupo.Text.Equals("") ? " and codigo_grupo='" + ddlGrupo.SelectedValue + "'" : "") +
                                   " group by wd.codigo_subgrupo, wd.descricao_subgrupo";

            String sqlDepartamento = "Select wd.codigo_departamento, descricao_departamento  from W_BR_CADASTRO_DEPARTAMENTO as wd " +
                                          " inner join fornecedor_departamento as fd on wd.codigo_departamento = fd.codigo_departamento " +
                                    " where fd.fornecedor = '" + txtfornecedor.Text + "'";

            String sqlWhereDep = "";
            if (!ddlGrupo.Text.Equals(""))
            {
                sqlWhereDep = " and wd.codigo_grupo='" + ddlGrupo.SelectedValue + "'";
            }
            if (!ddlSubGrupo.Text.Equals(""))
            {
                sqlWhereDep += " and wd.codigo_subgrupo='" + ddlSubGrupo.SelectedValue + "'";
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

        protected void imgFornecedor_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
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

        protected void carregarMercadorias(bool limitar)
        {
            if (IsPostBack)
            {
                //verificaSelecionados();
            }
            lblMercadoriaLista.Text = "Inclusão de Produto";
            //lblMercadoriaLista.ForeColor = Label1.ForeColor;

            if (ddlGrupo.Text.Equals("") &&
                ddlSubGrupo.Text.Equals("") &&
                ddlDepartamento.Text.Equals("") &&
                ddlLinha.Text.Equals("") &&
                txtfiltromercadoria.Text.Equals(""))
            {
                limitar = true;
            }


            User usr = (User)Session["user"];
            String sqlMercadoria = "Select distinct m.PLU,m.Descricao, m.UND,Departamento= wd.descricao_departamento  " +
                                             " from mercadoria as m inner join mercadoria_loja on m.plu = mercadoria_loja.plu " +
                                               " left join ean on m.plu=ean.plu  " +
                                               " inner join DEPARTAMENTO as wd on m.Codigo_departamento = wd.codigo_departamento " +
                                               " inner join fornecedor_departamento as fd on wd.codigo_departamento = fd.codigo_departamento and fd.fornecedor = '" + txtfornecedor.Text + "'" +
                                               " left join Fornecedor_Mercadoria on m.PLU = Fornecedor_Mercadoria.PLU  AND Mercadoria_Loja.Filial = Fornecedor_Mercadoria.Filial " +
                                    " where (mercadoria_loja.filial='" + usr.getFilial() + "')" +
                                    " and m.tipo not in(('MATERIA PRIMA', 'PRODUCAO'))  ";
            if (isnumero(txtfiltromercadoria.Text))
            {
                if (txtfiltromercadoria.Text.Length <= 6)
                {
                    sqlMercadoria += " and m.plu = '" + txtfiltromercadoria.Text + "' ";
                }
                else
                {
                    sqlMercadoria += " and (ean.ean like '%" + txtfiltromercadoria.Text + "%')";
                }
            }
            else
            {
                if (txtfiltromercadoria.Text.Length > 0)
                {

                    sqlMercadoria += " and (m.descricao like '%" + txtfiltromercadoria.Text + "%' or m.Ref_fornecedor like '%" + txtfiltromercadoria.Text + "%')";
                }


                if (!ddlGrupo.Text.Equals(""))
                {
                    sqlMercadoria += " and wd.codigo_grupo='" + ddlGrupo.SelectedValue + "' ";
                }
                if (!ddlSubGrupo.Text.Equals(""))
                {
                    sqlMercadoria += " and wd.codigo_subgrupo ='" + ddlSubGrupo.SelectedValue + "' ";

                }
                if (!ddlDepartamento.Text.Equals(""))
                {
                    sqlMercadoria += " and m.codigo_departamento ='" + ddlDepartamento.SelectedValue + "' ";
                }
            }

            if (!ddlLinha.Text.Equals(""))
            {
                sqlMercadoria += " and convert(varchar(3),isnull(Cod_Linha,''))+CONVERT(varchar(3),isnull(Cod_Cor_Linha,'')) ='" + ddlLinha.SelectedValue + "'";
            }


            //if Funcoes.valorParametro("PEDIDO_SIMPLES", usr).ToUpper()
            //voltar aqui 22042015

            gridMercadoria1.DataSource = Conexao.GetTable(sqlMercadoria + " order by m.descricao", usr, limitar);
            gridMercadoria1.DataBind();




        }
        protected void ImgPesquisaMercadoria_Click(object sender, ImageClickEventArgs e)
        {
            carregarMercadorias(false);
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
            txtfiltromercadoria.Text = "";
        }
        protected void imgBtnIncluirSelecionados_Click(object sender, ImageClickEventArgs e)
        {
            verificaSelecionados();
        }

        private void verificaSelecionados()
        {
            try
            {



                String strPlus = "";
                carregarDadosObj();
                foreach (GridViewRow item in gridMercadoria1.Rows)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                    if (chk.Checked)
                    {
                        if (!strPlus.Equals(""))
                        {
                            strPlus += ",";
                        }
                        strPlus += "'" + item.Cells[1].Text + "'";
                    }
                }
                incluirItens(strPlus);




                carregarDados();
            }
            catch (Exception err)
            {
                showMessage(err.Message, true);
            }



        }

        private void incluirItens(String strPlus)
        {
            User usr = (User)Session["User"];
            Contrato_fornecedorDAO obj = (Contrato_fornecedorDAO)Session["cFornecedor" + urlSessao()];
        


            String strFiliais = "";


            foreach (Contrato_Fornecedor_FilialDAO filial in obj.arrFilias)
            {
                if (strFiliais.Length > 0)
                    strFiliais += ",";

                strFiliais += "'" + filial.Filial + "'";
            }
            if (strFiliais.Length == 0)
                throw new Exception("Relacione ao menos uma Filial para poder incluir itens!");

            String sqlVerifica = "Select plu, Filial, id_contrato from Mercadoria_Loja " +
                                  "  where PLU in (" + strPlus + ") and Filial in  (" + strFiliais + ") and isnull(id_contrato,'0')<> '0' and isnull(id_contrato,'')<> '" + txtid_contrato.Text.Trim() + "'  ";

            SqlDataReader rsFilial = null;
            try
            {


                rsFilial = Conexao.consulta(sqlVerifica, null, false);
                String erro = "";
                while (rsFilial.Read())
                {
                    erro += "<br> Plu:" + rsFilial["plu"].ToString() + " Filial =" + rsFilial["Filial"].ToString() + " Contrato: " + rsFilial["id_contrato"].ToString();
                }



                if (erro.Length > 0)
                {

                    throw new Exception("O Contratos Já Cadastrados para " + erro);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rsFilial != null)
                    rsFilial.Close();
            }




            foreach (GridViewRow item in gridMercadoria1.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                if (chk.Checked)
                {
                    String plu = item.Cells[1].Text;
                    String strDescricao = item.Cells[2].Text;
                    String strUnd = item.Cells[3].Text;
                    Decimal vlr = 0;

                    if (!obj.itemIncluido(plu))
                    {
                        obj.addItem(plu, strDescricao, vlr, strUnd);
                    }
                    chk.Checked = false;
                }

            }
            //carregarDados();


        }

        protected void txtfornecedor_TextChanged(object sender, EventArgs e)
        {
            carregarItensFornecedor();
        }
        private void carregarItensFornecedor()
        {
                lblError.Text = "";
                carregarGrupos("", "", "");
                carregarMercadorias(true);
                if (gridMercadoria1.Rows.Count == 1 && gridMercadoria1.Rows[0].Cells[1].Text.Equals("------"))
                {
                
                showMessage( "Fornecedor Sem Departamentos Cadastrados!",true);
                    
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

        protected void ddlTipoReajuste_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoReajuste.Text.ToUpper().Equals("ANO"))
            {
                lblDiaMesReajuste.Text = "Mes Reajuste";
            }
            else if (ddlTipoReajuste.Text.ToUpper().Equals("MES"))
            {
                lblDiaMesReajuste.Text = "Dia Reajuste";
            }
        }

        protected void btnDigitado_Click(object sender, EventArgs e)
        {
            carregarDadosObj();

            Contrato_fornecedorDAO obj = (Contrato_fornecedorDAO)Session["cFornecedor" + urlSessao()];
            obj.gridInicio = 0;
            obj.gridFim = 100;
            if (obj.itensDigitados)
            {
                obj.itensDigitados = false;
                btnItensDigitados.Text = "DIGITADOS";
            }
            else
            {
                obj.itensDigitados = true;
                btnItensDigitados.Text = "TODOS";
            }


            Session.Remove("cFornecedor" + urlSessao());
            Session.Add("cFornecedor" + urlSessao(), obj);
            carregarDados();
            if (status.Equals("visualizar"))
            {
                EnabledControls(gridItens, false);
                EnabledControls(gridMercadoriasSelecionadas, false);
            }



        }
        protected void btnPag_Click(object sender, EventArgs e)
        {

            if (!status.Equals("visualizar"))
            {
                carregarDadosObj();
            }
            Contrato_fornecedorDAO obj = (Contrato_fornecedorDAO)Session["cFornecedor" + urlSessao()];
            int nFim = obj.arrItens.Count;
            if (obj.itensDigitados)
            {
                nFim = obj.qtdeItensDigitados;
            }





            Button btn = (Button)sender;
            if (btn.ID.Equals("btnPagInicio"))
            {
                obj.gridInicio = 0;
                obj.gridFim = 100;
            }
            else if (btn.ID.Equals("btnPagAnterio"))
            {
                obj.gridInicio -= 100;
                obj.gridFim = obj.gridInicio + 100;
                if (obj.gridInicio < 0)
                {
                    obj.gridInicio = 0;
                    obj.gridFim = 100;
                }


            }
            else if (btn.ID.Equals("btnPagProximo"))
            {

                obj.gridInicio += 100;
                obj.gridFim = obj.gridInicio + 100;
                if (obj.gridFim > nFim)
                {
                    decimal pagInicio = (decimal.Truncate(nFim / 100) * 100);
                    obj.gridInicio = Convert.ToInt32(pagInicio);
                    obj.gridFim = nFim;
                }
            }
            else if (btn.ID.Equals("btnPagFim"))
            {
                decimal pagInicio = (decimal.Truncate(nFim / 100) * 100);
                obj.gridInicio = Convert.ToInt32(pagInicio);
                obj.gridFim = nFim;
            }

            Session.Remove("cFornecedor" + urlSessao());
            Session.Add("cFornecedor" + urlSessao(), obj);

            carregarDados();
            if (status.Equals("visualizar"))
            {
                EnabledControls(gridItens, false);
            }



        }

        protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
        {
            if(TabContainer1.ActiveTabIndex == 1)
                carregarItensFornecedor();
        }
        protected void imgBtnAddItemRapido_Click(object sender,EventArgs e)
        {
            Contrato_fornecedorDAO obj = (Contrato_fornecedorDAO)Session["cFornecedor" + urlSessao()];
            obj.addItem(txtPlu.Text, txtDescricaoItem.Text, Funcoes.decTry(txtValor.Text), txtUndItem.Text);
            Session.Remove("cFornecedor" + urlSessao());
            Session.Add("cFornecedor" + urlSessao(), obj);
            carregarGrids();
            addItensDig.DefaultButton = "imgPlu";
            txtPlu.Text = "";
            txtDescricaoItem.Text = "";
            txtValor.Text = "";
            txtUndItem.Text = "";
            txtPlu.Focus();

        }
        protected void imgPlu_Click(object sender, EventArgs e)
        {
            if(txtPlu.Text.Trim().Length>0)
            {
              
                String sql = "Select m.plu, m.descricao , m.und from mercadoria  as m" +
                     " inner join DEPARTAMENTO as wd on m.Codigo_departamento = wd.codigo_departamento " +
                     " inner join fornecedor_departamento as fd on wd.codigo_departamento = fd.codigo_departamento and fd.fornecedor = '" + txtfornecedor.Text + "'" +
                    " where plu = '" + txtPlu.Text + "' or plu  =(Select plu from ean where ean ='" + txtPlu.Text + "')";
                SqlDataReader rs = null;

                try
                {
                    incluirItens(txtPlu.Text);
                    rs = Conexao.consulta(sql, null, false);
                    if(rs.Read())
                    {
                        txtPlu.Text = rs["plu"].ToString();
                        txtDescricaoItem.Text = rs["descricao"].ToString();
                        txtUndItem.Text = rs["und"].ToString();
                        txtValor.Focus();
                        addItensDig.DefaultButton = "imgBtnAddItemRapido";
                    }
                    else
                    {
                        showMessage("Item não encontrado ou não relacionado com o Fornecedor", true);
                    }
                }
                catch (Exception err)
                {

                    showMessage(err.Message, true);
                }
                finally
                {
                    if (rs != null)
                        rs.Close();
                }


            }
            else
            {
                Session.Add("camporecebe" + urlSessao(), "plu");
                exibeLista();
            }


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

    }

}