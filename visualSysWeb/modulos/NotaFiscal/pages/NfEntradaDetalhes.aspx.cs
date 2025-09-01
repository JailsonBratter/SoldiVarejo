using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using System.Data.SqlClient;
using System.Drawing;
using visualSysWeb.code;
using visualSysWeb.modulos.NotaFiscal.dao;
using visualSysWeb.modulos.NotaFiscal.code;

namespace visualSysWeb.modulos.NotaFiscal.pages
{
    public partial class NfEntradaDetalhes : visualSysWeb.code.PagePadrao
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            User usr = (User)Session["User"];

            //Sempre atualiza o objeto da DATA DE FECHAMENTO na sessão.
            usr.filial.dtFechamentoEstoque = usr.filial.GetDataFechamentoEstoque(usr.filial.Filial);

            Session.Remove("User");
            Session.Add("User", usr);

            nfDAO obj = null;
            try
            {
                obj = (nfDAO)Session["objNfEntrada" + urlSessao()];
            }
            catch (Exception)
            {

                Session.Remove("objNfEntrada" + urlSessao());
                obj = null;
            }

            // txtNFE.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnConfirmaImportar.UniqueID + "').click();return false;}} else {return true}; ");

            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    status = "incluir";
                    habilitarCampos(true);
                    obj = new nfDAO(usr, "2");
                    obj.DestFornecedor = true;
                    obj.serie = Funcoes.intTry(usr.filial.serie_nfe.ToString());

                    //Especifico para DOCA
                    try
                    {
                        string chave = Request.Params["chave"].ToString();
                        bool entradaDoca = (Request.Params["doca"].ToString().ToUpper().Equals("TRUE") ? true : false);
                        if (!chave.Equals("") && entradaDoca)
                        {
                            obj.nfRecebimentoDoca = new Nf_RecebimentoDAO(usr, chave);
                            obj.entradaDoca = true;
                            txtNFE.Text = chave;
                        }
                    }
                    catch
                    {

                    }

                    Session.Remove("objNfEntrada" + urlSessao());
                    Session.Add("objNfEntrada" + urlSessao(), obj);

                    //Alterar para que primeiro solicite a NATUREZA de operação
                    Session.Remove("campoLista" + urlSessao());
                    Session.Add("campoLista" + urlSessao(), "txtCodigo_operacao");



                    exibeLista("txtCodigo_operacao");
                    //ModalImportar.Show();
                    if (usr != null)
                    {
                        txtusuario.Text = usr.getNome();
                        txtusuario_Alteracao.Text = usr.getNome();
                    }
                }
                carregarGrids();
            }
            else
            {
                if (Request.Params["codigo"] != null)  // colocar o campo index da tabela
                {
                    try
                    {
                        if (!IsPostBack)
                        {

                            String codigo = Request.Params["codigo"].ToString();// colocar o campo index da tabela
                            String fornecedor = Request.Params["fornecedor"].ToString();
                            String serie = Request.Params["serie"].ToString();
                            obj = new nfDAO(codigo, "2", fornecedor, Funcoes.intTry(serie), usr);
                            if (obj.Data < usr.filial.dtFechamentoEstoque || obj.nf_Canc)
                            {
                                status = "pesquisar";

                            }
                            else
                            {
                                status = "visualizar";
                            }
                            if (obj.nf_Canc)
                            {
                                showMessage("NOTA FISCAL CANCELADA", true);
                            }

                            Session.Remove("objNfEntrada" + urlSessao());
                            Session.Add("objNfEntrada" + urlSessao(), obj);
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
            camposnumericos();
            formataCampos();
            if (!status.Equals("incluir") && !status.Equals("editar"))
            {
                habilitarCampos(false);
            }
            else
            {
                habilitarCampos(true);
            }

        }

        private void formataCampos()
        {
            txtNFE.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            TxtPesquisaLista.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            //txtCodigo.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            txtFornecedor_CNPJ.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            txtEmissao.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            txtData.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            txtcentro_custo.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            txtCodigo_operacao.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            txtiva.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            txtmargem_iva.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            txtQtde.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            txtUnitario.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            txtPLU.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            txtCODIGO_REFERENCIA.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            txtDescricao.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            txtEmbalagem.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            txtCodigo_Tributacao.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            txtDescontoItem.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
        }

        private void limparCampos()
        {
            LimparCampos(cabecalho);
            LimparCampos(conteudo);
            LimparCampos(rodape);
            gridItens.DataSource = new DataTable();
            gridItens.DataBind();
            gridPagamentos.DataSource = new DataTable();
            gridPagamentos.DataBind();
            EnabledButtons(conteudo, false);
        }

        private void camposnumericos()
        {
            String[] campos = { "txtFrete",
                                "txtSeguro",
                                "txtIPI_Nota",
                                "txtBase_Calculo",
                                "txtBase_Calc_Subst",
                                "txtDesconto",
                                "txtDespesas_financeiras",
                                "txtOutras",
                                "txtDesconto_geral",
                                "txtICMS_Nota",
                                "txtICMS_ST",
                                "txtTotal",
                                "txtTotalProdutos",
                                "txtQtde",
                                "txtEmbalagem",
                                "txtUnitario",
                                "txtCodigo_Tributacao",
                                "txtDescontoItem",
                                "txtdespesas",
                                "TxtTotalItem",
                                "txtIPI",
                                "txtIPIV",
                                "txtmargem_iva",
                                "txtiva",
                                "txtaliquota_icms",
                                "txtredutor_base",
                                "txtPisItem",
                                "txtCofinsItem",
                                "txtNum_item",
                                "txtCodigo_operacao_item",
                                "txtNCM",
                                "txtPeso_liquido",
                                "txtPeso_Bruto",
                                "txtCSTPIS",
                                "txtCSTCOFINS",
                                "txtValorPg",
                                "txtCodigo",
                                "txtcentro_custo",
                                "txtCodigo_operacao",
                                "txtCodigo"
                                     };
            foreach (String item in campos)
            {
                TextBox txt = (TextBox)cabecalho.FindControl(item);
                if (txt == null)
                    txt = (TextBox)conteudo.FindControl(item);

                if (txt != null)
                    txt.Attributes.Add("OnKeyPress", "javascript:return numeros(this,event);");

            }
        }

        private void habilitarCampos(bool enable)
        {
            EnabledControls(conteudo, enable);
            EnabledControls(cabecalho, enable);
            EnabledControls(rodape, enable);
            EnabledButtons(gridItens, enable);
            EnabledControls(pnConfirma, true);
            EnabledControls(pnConfirmaPg, true);
            EnabledControls(pnItens, true);
            EnabledControls(pnItensFrame, true);
            EnabledControls(PnPagamentoFrame, true);
            EnabledControls(PnExcluirItem, true);
            EnabledControls(PnImportarFrame, true);

            User usr = (User)Session["User"];
            bool editarPlu = Funcoes.valorParametro("NF_ENT_NAO_PLU", usr).ToUpper().Equals("TRUE");
            if (editarPlu && !txtid.Text.Equals(""))
            {
                txtPLU.Enabled = false;
                btnimg_txtPLU.Visible = false;
            }


            addItens.Visible = enable;
            AddPg.Visible = enable;
            ImgDtEmissao.Visible = enable;
            ImgDtCalendario.Visible = enable;
            if (status.Equals("editar"))
            {
                btnimg_txtFornecedor_CNPJ.Visible = false;
                txtCodigo_operacao.Enabled = false;
                txtCodigo_operacao.BackColor = Color.FromArgb(0xDCDCDC);
                btnimg_txtCodigo_operacao.Visible = false;
                //TabPanel2.Enabled = false;
            }
            if (status.Equals("visualizar"))
            {
                btnImpressao.Visible = true;
                nfDAO obj = (nfDAO)Session["objNfEntrada" + urlSessao()];
                Session.Remove("nfImprime");
                Session.Add("nfImprime", obj);
            }
            else
            {
                btnImpressao.Visible = false;
            }
            camposnumericos();


            if (gridHistorico.Rows.Count > 0)
            {
                tabHistoricoEdicao.Visible = true;
            }
            else
            {
                tabHistoricoEdicao.Visible = false;
            }
        }

        protected bool validaCamposObrigatorios()
        {
            try
            {
                User usr = (User)Session["user"];
                natureza_operacaoDAO op = new natureza_operacaoDAO(txtCodigo_operacao.Text, usr);

                if (validaCampos(cabecalho) && validaCampos(conteudo) && validaCampos(rodape))
                    return true;
                else
                    return false;
            }
            catch (Exception err)
            {

                throw new Exception("erro:" + err.Message);
            }


        }

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array
            String[] campos = { "txtCodigo",
                                    "txtFornecedor_CNPJ",
                                    "txtEmissao",
                                    "txtData",
                                    "txtcentro_custo",
                                    "txtCodigo_operacao",
                                    "txtSerie"
                                     };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {
            if (status.Equals("editar"))
            {
                if (campo.ID != null && (campo.ID.Equals("txtCodigo")
                    || campo.ID.Equals("txtFornecedor_CNPJ")
                    || campo.ID.Equals("txtEmissao")
                    || campo.ID.Equals("txtData")
                    || campo.ID.Equals("txtSerie")
                    ))
                {

                    return true;

                }


            }

            if (status.Equals("incluir") && !txtid.Text.Equals(""))
            {
                txtEmissao.Enabled = false;
            }

            String[] campos = { "txtusuario",
                                  "txtCliente_Fornecedor",
                                    "TxtNomeFornecedor",
                                    "txtPedido",
                                    "txtid",
                                    "txtIPI_Nota",
                                    "txtBase_Calculo",
                                    "txtICMS_Nota",
                                    "txtICMS_ST",
                                    "txtDesconto",
                                    "txtTotal",
                                    "txtBase_Calc_Subst",
                                    "txtNum_item",
                                    "chknf_Canc",
                                    "txtaliquota_icms",
                                    "txtredutor_base",
                                    "TxtTotalItem",
                                    //"txtPisItem",
                                    //"txtCofinsItem",
                                    "txtTotalProdutos",
                                    "txtTotalCustoItem",
                                    "txtVlrPisItem",
                                    "txtVlrCofinsItem",
                                    "txtIndice_st",
                                    "txtDespesas_financeiras",
                                    "txtTotal_vFCP",
                                    "txtTotal_vFCPST",
                                    "txtusuario_Alteracao",
                                    "txtNovoEan",
                                    "txtNovoRef",
                                    "txtNovoNCM",
                                    "txtNovoDescricaoTributacao",
                                    "txtNovoCodGrupo",
                                    "txtNovoDescGrupo",
                                    "txtNovoCodSubGrupo",
                                    "txtNovoDescSubGrupo",
                                    "txtNovoCodDepartamento",
                                    "txtNovoDescDepartamento"
                                };
            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("NfEntradaDetalhes.aspx?novo=true");

        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            nfDAO obj = (nfDAO)Session["objNfEntrada" + urlSessao()];
            User usr = (User)Session["User"];

            //Checa se a NFe contém algum inventário posterior a data de entrada desta NFe
            //ou se não há informações em mercadoria_estoque_dia anterior a data de entrada da NFe
            if (obj.NtOperacao.Baixa_estoque)
            {
                foreach (var item in obj.NfItens)
                {
                    //Checa primeiro o inventario
                    if (obj.bloqueioInventario(usr, item.PLU, obj.Data))
                    {
                        showMessage("O Item: " + item.PLU + " foi inventariado após a data de entrada da NFe. Não será permitido a edição.", true);
                        status = "pesquisar";
                        carregarDados();
                        carregabtn(pnBtn, true);
                        habilitarCampos(false);
                        return;
                    }
                    //Caso não haja bloqueio pelo inventário, checa se há bloqueio pelo controle de estoque na MERCADORIA_ESTOQUE_DIA
                    if (obj.Data < DateTime.Today)
                    {
                        //Checa se a data é inferior a data de controle de estoque do item.
                        if (obj.bloqueioControleEstoqueDia(usr, item.PLU, obj.Data))
                        {
                            showMessage("O Item: " + item.PLU + " iniciou o controle de estoque posterior a data desta NFE. Não será permitido a edição.", true);
                            status = "pesquisar";
                            carregarDados();
                            carregabtn(pnBtn, true);
                            habilitarCampos(false);
                            return;
                        }
                    }
                }
            }


            if ((obj.Data < usr.filial.dtFechamentoEstoque) || obj.nf_Canc)
            {
                showMessage("Não é permitido alterações na Nota Fiscal", true);
                status = "pesquisar";
                carregarDados();
                carregabtn(pnBtn, true);
                habilitarCampos(false);
            }
            else
            {

                status = "editar";
                habilitarCampos(true);
                carregarDados();
                carregabtn(pnBtn, true);
            }


        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("NfEntrada.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            User usr = (User)Session["user"];
            nfDAO obj = (nfDAO)Session["objNfEntrada" + urlSessao()];
            carregarDadosObj();
            //Checa se a NFe contém algum inventário posterior a data de entrada desta NFe
            if (obj.NtOperacao.Baixa_estoque)
            {
                foreach (var item in obj.NfItens)
                {
                    if (obj.bloqueioInventario(usr, item.PLU, obj.Data))
                    {
                        showMessage("O Item: " + item.PLU + " foi inventariado após a data da NFe. Não será permitido a exclusão.", true);
                        status = "pesquisar";
                        carregarDados();
                        carregabtn(pnBtn, true);
                        habilitarCampos(false);
                        return;
                    }
                    //Caso não haja bloqueio pelo inventário, checa se há bloqueio pelo controle de estoque na MERCADORIA_ESTOQUE_DIA
                    if (obj.Data < DateTime.Today)
                    {
                        //Checa se a data é inferior a data de controle de estoque do item.
                        if (obj.bloqueioControleEstoqueDia(usr, item.PLU, obj.Data))
                        {
                            showMessage("O Item: " + item.PLU + " iniciou o controle de estoque posterior a data desta NFE. Não será permitido a edição.", true);
                            status = "pesquisar";
                            carregarDados();
                            carregabtn(pnBtn, true);
                            habilitarCampos(false);
                            return;
                        }
                    }
                }
            }

            if (obj.Data <= usr.filial.dtFechamentoEstoque)
            {
                showMessage("Data de entrada não pode ser menor ou igual a data de fechamento do estoque [" + usr.filial.dtFechamentoEstoque.ToString("dd-MM-yyyy") + "]", true);
                return;
            }
            else
            {
                obj = null;
            }

            modalPnConfirma.Show();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                User usr = (User)Session["user"];
                nfDAO obj = (nfDAO)Session["objNfEntrada" + urlSessao()];
                carregarDadosObj();
                if (obj.Data <= usr.filial.dtFechamentoEstoque)
                {
                    showMessage("Data de entrada não pode ser menor ou igual a data de fechamento do estoque [" + usr.filial.dtFechamentoEstoque.ToString("dd-MM-yyyy") + "]", true);
                    return;
                }
                else
                {
                    obj = null;
                }

                Session.Remove("execSalvar" + urlSessao());
                if (validaCamposObrigatorios())
                {
                    carregarDadosObj();
                    obj = (nfDAO)Session["objNfEntrada" + urlSessao()];
                    Decimal tPG = obj.TotalPag();
                    Decimal dif = tPG - obj.Total;
                    //Valida parâmetro apra bloquear.
                    if (Funcoes.valorParametro("NF_SEM_PAGAMENTO", usr).ToUpper().Equals("TRUE"))
                    {
                        if (obj.NtOperacao.Gera_apagar_receber && tPG == 0)
                        {
                            showMessage("Obrigatório incluir um título para pagamento.", true);
                        }
                        else
                        {

                            if (tPG == 0 || !dif.ToString("N2").Equals("0,00"))
                            {
                                modalConfirmaPgAdd.Show();
                            }
                            else
                            {
                                SalvarNota(false);
                            }
                        }

                    }
                    else
                    {

                        if (tPG == 0 || !dif.ToString("N2").Equals("0,00"))
                        {
                            modalConfirmaPgAdd.Show();
                        }
                        else
                        {
                            SalvarNota(false);
                        }

                    }
                }
                else
                {
                    showMessage("Campo Obrigatorio não preenchido", true);

                }
            }
            catch (Exception err)
            {
                showMessage(err.Message, true);

            }
        }

        protected void SalvarNota(bool confirmado)
        {
           
            try
            {

                if (!status.Equals("incluir") && !confirmado)
                {
                    modalConfirmaEdicao.Show();
                }
                else
                {
                    // Evitar Duplo click
                    String exec = (String)Session["execSalvar" + urlSessao()];
                    if (exec != null)
                        return;
                    else
                        Session.Add("execSalvar" + urlSessao(), "executando");



                    nfDAO obj = (nfDAO)Session["objNfEntrada" + urlSessao()];
                    //Grava data e hora do lançamento
                    obj.dataHoraLancamento = DateTime.Now;
                    obj.salvar(status.Equals("incluir"));

                    lblTotalPagamentos.Text = obj.TotalPag().ToString("N2");
                    showMessage("Salvo com Sucesso", false);

                    status = "visualizar";

                    Session.Remove("objNfEntrada" + urlSessao());
                    Session.Add("objNfEntrada" + urlSessao(), obj);
                    carregabtn(pnBtn);
                    carregarDados();
                    habilitarCampos(false);
                }
            }
            catch (Exception err)
            {
                throw err;
            }

        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("NfEntrada.aspx");//colocar endereco pagina de pesquisa
        }

        //--Atualizar DaoForm 
        private void carregarDados()
        {
            nfDAO obj = (nfDAO)Session["objNfEntrada" + urlSessao()];
            txtCodigo.Text = obj.Codigo.ToString() + "";
            txtSerie.Text = obj.serie.ToString();
            txtCliente_Fornecedor.Text = obj.Cliente_Fornecedor.ToString() + "";
            TxtNomeFornecedor.Text = obj.Nome_Fornecedor.ToString() + "";
            txtData.Text = obj.DataBr();
            txtCodigo_operacao.Text = (obj.Codigo_operacao.ToString().Equals("0") ? "" : obj.Codigo_operacao.ToString());
            txtEmissao.Text = obj.EmissaoBr();
            txtTotal.Text = obj.Total.ToString("N2");
            txtTotalProdutos.Text = obj.TotalProdutos.ToString("N2");
            txtDesconto.Text = string.Format("{0:0,0.00}", obj.Desconto);
            txtFrete.Text = string.Format("{0:0,0.00}", obj.Frete);
            txtSeguro.Text = string.Format("{0:0,0.00}", obj.Seguro);
            txtIPI_Nota.Text = string.Format("{0:0,0.00}", obj.IPI_Nota);
            txtOutras.Text = string.Format("{0:0,0.00}", obj.Outras);
            txtICMS_Nota.Text = string.Format("{0:0,0.00}", obj.ICMS_Nota);
            txtBase_Calculo.Text = string.Format("{0:0,0.00}", obj.Base_Calculo);
            txtDespesas_financeiras.Text = string.Format("{0:0,0.00}", obj.Despesas_financeiras);
            txtPedido.Text = obj.Pedido.ToString() + "";
            txtBase_Calc_Subst.Text = string.Format("{0:0,0.00}", obj.Base_Calc_Subst);
            txtObservacao.Text = obj.Observacao.ToString() + "";
            chknf_Canc.Checked = obj.nf_Canc;


            txtcentro_custo.Text = obj.centro_custo.ToString();

            txtICMS_ST.Text = string.Format("{0:0,0.00}", obj.ICMS_ST);

            txtFornecedor_CNPJ.Text = obj.Fornecedor_CNPJ.ToString();

            txtDesconto_geral.Text = string.Format("{0:0,0.00}", obj.Desconto_geral);
            txtusuario.Text = obj.usuario.ToString();
            txtusuario_Alteracao.Text = obj.usuario_Alteracao;
            txtid.Text = obj.id.ToString();

            if (obj.crt.Equals("1")) //SE A NOTA FOI EMITIDA COMO SIMPLES
            {
                divCorrigirPorc.Visible = true;
            }
            else
            {
                divCorrigirPorc.Visible = false;
            }


            chkBoletoRecebido.Checked = obj.boleto_recebido;
            lblTotalPagamentos.Text = obj.TotalPag().ToString("N2");

            txtTotal_vFCP.Text = obj.vFCP.ToString("N2");
            txtTotal_vFCPST.Text = obj.vFCPST.ToString("N2");


            Session.Remove("objNfEntrada" + urlSessao());
            Session.Add("objNfEntrada" + urlSessao(), obj);
            carregarGrids();
        }


        public void carregarGrids()
        {
            try
            {


                nfDAO obj = (nfDAO)Session["objNfEntrada" + urlSessao()];
                if (obj != null)
                {
                    gridItens.DataSource = obj.nfItens();
                    gridItens.DataBind();

                    gridCentroCusto.DataSource = obj.LsCentrosCustos;
                    gridCentroCusto.DataBind();

                    gridPagamentos.DataSource = obj.nfPagamento();
                    gridPagamentos.DataBind();

                    ////gridHistorico.Visible = true;
                    gridHistorico.DataSource = obj.histEdicao;
                    gridHistorico.DataBind();
                }
            }
            catch (Exception err)
            {
                showMessage(err.Message, true);
                throw err;
            }
        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            User usr = (User)Session["User"];
            nfDAO obj = (nfDAO)Session["objNfEntrada" + urlSessao()];
            obj.Codigo = txtCodigo.Text;
            obj.serie = Funcoes.intTry(txtSerie.Text);
            obj.Cliente_Fornecedor = txtCliente_Fornecedor.Text;
            obj.objForne = obj.objFornecedor;
            obj.Tipo_NF = "2";
            if (txtData.Text.Equals(""))
            {
                obj.Data = DateTime.Today;
                obj.Emissao = DateTime.Today;
            }
            else
            {
                obj.Data =  DateTime.Parse(txtData.Text);
                obj.Emissao = DateTime.Parse(txtEmissao.Text);
            }
            obj.Codigo_operacao = Funcoes.decTry((txtCodigo_operacao.Text.Equals("") ? "0" : txtCodigo_operacao.Text));
            obj.Total = Funcoes.decTry((txtTotal.Text.Equals("") ? "0" : txtTotal.Text));
            obj.Desconto = Funcoes.decTry((txtDesconto.Text.Equals("") ? "0" : txtDesconto.Text));
            obj.Frete = Funcoes.decTry((txtFrete.Text.Equals("") ? "0" : txtFrete.Text));
            obj.Seguro = Funcoes.decTry((txtSeguro.Text.Equals("") ? "0" : txtSeguro.Text));
            obj.IPI_Nota = Funcoes.decTry((txtIPI_Nota.Text.Equals("") ? "0" : txtIPI_Nota.Text));
            obj.Outras = Funcoes.decTry((txtOutras.Text.Equals("") ? "0" : txtOutras.Text));
            obj.ICMS_Nota = Funcoes.decTry((txtICMS_Nota.Text.Equals("") ? "0" : txtICMS_Nota.Text));
            obj.Base_Calculo = Funcoes.decTry((txtBase_Calculo.Text.Equals("") ? "0" : txtBase_Calculo.Text));
            obj.Despesas_financeiras = Funcoes.decTry((txtDespesas_financeiras.Text.Equals("") ? "0" : txtDespesas_financeiras.Text));
            obj.Pedido = txtPedido.Text;
            obj.Base_Calc_Subst = Funcoes.decTry((txtBase_Calc_Subst.Text.Equals("") ? "0" : txtBase_Calc_Subst.Text));
            obj.Observacao = txtObservacao.Text;
            obj.nf_Canc = chknf_Canc.Checked;
            obj.centro_custo = txtcentro_custo.Text;
            obj.ICMS_ST = Funcoes.decTry((txtICMS_ST.Text.Equals("") ? "0" : txtICMS_ST.Text));
            obj.Fornecedor_CNPJ = txtFornecedor_CNPJ.Text;
            obj.Desconto_geral = Funcoes.decTry((txtDesconto_geral.Text.Equals("") ? "0" : txtDesconto_geral.Text));
            obj.usuario = txtusuario.Text;
            obj.usuario_Alteracao = usr.getNome();
            obj.id = txtid.Text;
            obj.boleto_recebido = chkBoletoRecebido.Checked;

            //Carregar dados CTe
            obj.NFCTe = new NF_CTeDAO();
            obj.NFCTe.Filial = obj.Filial;
            obj.NFCTe.Chave_NFe = obj.id;
            obj.NFCTe.Situacao = ddlCTeSitDOC.SelectedValue;
            obj.NFCTe.Fornecedor = txtCTeCodigoFornecedor.Text;
            obj.NFCTe.Serie = Int32.Parse(txtCTeSerie.Text.Equals("") ? "0" : txtCTeSerie.Text) ;
            obj.NFCTe.Emissao = DateTime.Parse("1900-01-01"); //  txtCTeEmissao.Text);
            obj.NFCTe.Aquisicao = DateTime.Parse("1900-01-01"); // txtCTeAquisicao.Text);
            obj.NFCTe.Chave = txtCTeChave.Text;
            obj.NFCTe.Tipo_CTe = Funcoes.intTry(ddlTipoCTeBPe.SelectedValue);
            obj.NFCTe.Chave_Substituicao = txtCTeChaveSubstituicao.Text;
            obj.NFCTe.Tipo_Frete = Funcoes.intTry(ddlTipoCTeFrete.SelectedValue);
            obj.NFCTe.ICMS_Base = Funcoes.decTry(txtCTeBCICMS.Text);
            obj.NFCTe.ICMS_Reducao = Funcoes.decTry(txtCTeReducao.Text);
            obj.NFCTe.ICMS_Aliquota = Funcoes.decTry(txtCTeAliquota.Text);
            obj.NFCTe.ICMS_Valor = Funcoes.decTry(txtCTeValorICMS.Text);
            obj.NFCTe.IBGE_Origem = Funcoes.intTry(txtCTeMunicipioOrigem.Text);
            obj.NFCTe.IBGE_Destino = Funcoes.intTry(txtCTeMunicipioDestino.Text);
            obj.NFCTe.Boleto_Vencimento = DateTime.Parse("1900-01-01"); // txtCTeVencimento.Text);

            Session.Remove("objNfEntrada" + urlSessao());
            Session.Add("objNfEntrada" + urlSessao(), obj);



        }






        protected void gridItens_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                /*
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (isnumero(e.Row.Cells[i].Text))
                    {
                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                        e.Row.Cells[i].Text = Funcoes.decTry(e.Row.Cells[i].Text).ToString("N2");

                    }
                }
                 */
            }


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
                    e.Row.Cells[0].Focus();

                }
            }
            if (e.Row.RowIndex >= 0)
            {
                nfDAO nf = (nfDAO)Session["objNfEntrada" + urlSessao()];
                if (nf != null && nf.NfItens.Count > 0)
                {
                    nf_itemDAO itemnf = nf.item(e.Row.RowIndex);
                    if (itemnf.inativo)
                    {
                        e.Row.ForeColor = System.Drawing.Color.Red;
                    }

                    if (itemnf.Qtde_Devolver > 0)
                    {
                        e.Row.ForeColor = System.Drawing.Color.Orange;
                    }
                }
            }

        }

        protected void gridItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LimparCampos(pnItens);
                nfDAO nf = (nfDAO)Session["objNfEntrada" + urlSessao()];
                Session.Remove("itemEditado" + urlSessao());
                int index = Convert.ToInt32(e.CommandArgument);
                nf_itemDAO itemnf = nf.item(index);
                itemnf.objFornecedor = nf.objFornecedor;
                Session.Remove("item" + urlSessao());
                Session.Add("item" + urlSessao(), itemnf);
                ViewState["gridLinha"] = index;
                carregarDadosObj();
                carregarDadosItens(itemnf);
                habilitarItensTributacao();

                PnExcluirItem.Visible = true;
                ItemInativo(itemnf.inativo);

                User usr = (User)Session["User"];
                bool editarPlu = Funcoes.valorParametro("NF_ENT_NAO_PLU", usr).ToUpper().Equals("TRUE");
                if (editarPlu && !txtid.Text.Equals(""))
                {
                    txtPLU.Enabled = false;
                    btnimg_txtPLU.Visible = false;
                }


            }
            catch (Exception err)
            {
                showMessage(err.Message, true);

            }

        }

        protected void carregarDadosItens(nf_itemDAO itemnf)
        {
            txtPLU.Text = itemnf.PLU;
            txtCODIGO_REFERENCIA.Text = itemnf.CODIGO_REFERENCIA;

            txtDescricao.Text = itemnf.Descricao;
            txtCodigo_Tributacao.Text = itemnf.Codigo_Tributacao.ToString();
            txtIndice_st.Text = itemnf.indice_St.ToString();
            txtDescontoItem.Text = itemnf.Desconto.ToString("N4");
            txtDescValorItem.Text = itemnf.DescontoValor.ToString("N4");
            txtdespesas.Text = itemnf.despesas.ToString("N2"); ;
            txtIPI.Text = itemnf.porcIPI.ToString("N2");
            txtIPIV.Text = itemnf.vIpiv.ToString("N2");
            txtaliquota_icms.Text = itemnf.aliquota_icms.ToString("N2");

            txtmargem_iva.Text = itemnf.vmargemIva.ToString("N2");
            txtiva.Text = itemnf.vIva.ToString("N2");

            txtredutor_base.Text = itemnf.redutor_base.ToString("N2");
            txtredutor_base_iva.Text = itemnf.redutor_base_ST.ToString("N2");

            txtNum_item.Text = itemnf.Num_item.ToString();
            txtCodigo_operacao_item.Text = itemnf.codigo_operacao.ToString();
            txtNCM.Text = itemnf.NCM.ToString();
            txtUnd.Text = itemnf.Und;
            txtPeso_liquido.Text = itemnf.Peso_liquido.ToString("N2");
            txtPeso_Bruto.Text = itemnf.Peso_Bruto.ToString("N2");

            txtCSTPIS.Text = itemnf.CSTPIS;
            txtPisItem.Text = itemnf.PISp.ToString("N2");
            txtCofinsItem.Text = itemnf.COFINSp.ToString("N2");
            txtVlrPisItem.Text = itemnf.PISV.ToString("N2");
            txtVlrCofinsItem.Text = itemnf.COFINSV.ToString("N2");

            txtQtde.Text = itemnf.Qtde.ToString("N4");
            txtEmbalagem.Text = itemnf.Embalagem.ToString();
            txtUnitario.Text = itemnf.Unitario.ToString("N5");
            txtpCredSN.Text = itemnf.pCredSN.ToString("N2");
            txtvCredicmssn.Text = itemnf.vCredicmssn.ToString("N2");
            lblInativo.Text = (itemnf.inativo ? "inativo" : "");

            txtItem_BaseFCP.Text = itemnf.vBCFCP.ToString("N2");
            txtItem_pFCP.Text = itemnf.pFCP.ToString("N2");
            txtItem_VlrFCP.Text = itemnf.vFCP.ToString("N2");
            txtItem_BaseFCPST.Text = itemnf.vBCFCPST.ToString("N2");
            txtItem_pFCPST.Text = itemnf.pFCPST.ToString("N2");
            txtItem_VlrFCPST.Text = itemnf.vFCPST.ToString("N2");
            txtDtValidade.Text = itemnf.Data_validadeBr;
            txtValorFreteItem.Text = itemnf.Frete.ToString("N2");
            //CalculatotalItem();
            TxtTotalItem.Text = itemnf.vtotal_produto.ToString("N2");
            txtTotalCustoItem.Text = itemnf.TotalCustoUnitario.ToString("N2");
            ModalItens.Show();
            ItemInativo(itemnf.inativo);


        }

        protected void ItemInativo(bool inativo)
        {
            if (inativo)
            {
                lblInativo.Text = "inativo";
                lblErroItem.Text = "ATENÇÃO:Produto esta Inativo!";
            }
            else
            {

                lblInativo.Text = "";
                lblErroItem.Text = "";
            }

        }
        protected void ImgBtnAddReferencia_Click(object sender, ImageClickEventArgs e)
        {
            if (!txtCodigo.Text.Equals("") && !txtCliente_Fornecedor.Text.Equals(""))
            {
                try
                {


                    lblError.Text = "";
                    lblErroItem.Text = "";
                    String or = "txtCODIGO_REFERENCIA";

                    carregarDadosObj();
                    Session.Remove("campoLista" + urlSessao());
                    Session.Add("campoLista" + urlSessao(), or);
                    LimparCampos(pnItens);
                    exibeLista();
                    PnExcluirItem.Visible = false;
                }
                catch (Exception err)
                {
                    showMessage(err.Message, true);

                }
            }
            else
            {
                showMessage("Inclua as Informações do Codigo e Fornecedor", true);
            }

        }

        protected void ImgBtnAddItens_Click(object sender, ImageClickEventArgs e)
        {
            txtCodigo_operacao.BackColor = System.Drawing.Color.White;

            if (txtCodigo_operacao.Text.Trim().Equals(""))
            {
                txtCodigo_operacao.BackColor = System.Drawing.Color.Red;
                showMessage("Escolha a Natureza de operação!", true);
                return;
            }

            if (!txtCodigo.Text.Equals("") && !txtCliente_Fornecedor.Text.Equals(""))
            {
                try
                {
                    lblError.Text = "";
                    lblErroItem.Text = "";
                    String or = "txtPLU";

                    carregarDadosObj();
                    Session.Remove("campoLista" + urlSessao());
                    Session.Add("campoLista" + urlSessao(), or);
                    TxtPesquisaLista.Text = "";
                    LimparCampos(pnItens);
                    exibeLista();
                    PnExcluirItem.Visible = false;
                }
                catch (Exception err)
                {
                    showMessage(err.Message, true);

                }
            }
            else
            {
                showMessage("Inclua as Informações do Codigo e Fornecedor", true);
            }



        }

        protected nf_itemDAO carregaItem()
        {
            User usr = (User)Session["user"];
            nfDAO nf = (nfDAO)Session["objNfEntrada" + urlSessao()];
            nf_itemDAO itemnf = (nf_itemDAO)Session["item" + urlSessao()];
            if (itemnf == null)
                itemnf = new nf_itemDAO(usr);
            itemnf.crt = nf.crt;
            itemnf.objFornecedor = nf.objFornecedor;
            itemnf.Tipo_NF = "2";
            itemnf.Codigo = txtCodigo.Text;
            itemnf.Cliente_Fornecedor = txtCliente_Fornecedor.Text;
            itemnf.PLU = txtPLU.Text;
            itemnf.CODIGO_REFERENCIA = txtCODIGO_REFERENCIA.Text;
            itemnf.Descricao = txtDescricao.Text;
            itemnf.naturezaOperacao = new natureza_operacaoDAO(txtCodigo_operacao.Text, usr);
            itemnf.Unitario = Funcoes.decTry(txtUnitario.Text);
            itemnf.Embalagem = Funcoes.decTry(txtEmbalagem.Text);
            itemnf.Qtde = Funcoes.decTry(txtQtde.Text);

            itemnf.Codigo_Tributacao = Funcoes.decTry(txtCodigo_Tributacao.Text);

            itemnf.Desconto = Funcoes.decTry(txtDescontoItem.Text);
            itemnf.vDesconto_valor = Funcoes.decTry(txtDescValorItem.Text);

            itemnf.aliquota_icms = Funcoes.decTry((txtaliquota_icms.Text.Equals("") ? "0" : txtaliquota_icms.Text));

            itemnf.porcIPI = Funcoes.decTry((txtIPI.Text.Equals("") ? "0,00" : txtIPI.Text));
            itemnf.vIpiv = Funcoes.decTry((txtIPIV.Text.Equals("") ? "0,00" : txtIPIV.Text));

            itemnf.despesas = Funcoes.decTry((txtdespesas.Text.Equals("") ? "0,00" : txtdespesas.Text));

            itemnf.vmargemIva = Funcoes.decTry((txtmargem_iva.Text.Equals("") ? "0,00" : txtmargem_iva.Text));
            itemnf.vIva = Funcoes.decTry((txtiva.Text.Equals("") ? "0,00" : txtiva.Text));
            itemnf.redutor_base = Funcoes.decTry((txtredutor_base.Text).Equals("") ? "0,00" : txtredutor_base.Text);
            itemnf.redutor_base_ST = Funcoes.decTry((txtredutor_base_iva.Text).Equals("") ? "0,00" : txtredutor_base_iva.Text);

            itemnf.Num_item = int.Parse((txtNum_item.Text.Equals("") ? "0" : txtNum_item.Text));
            itemnf.codigo_operacao = Funcoes.decTry((txtCodigo_operacao_item.Text.Equals("") ? "0,00" : txtCodigo_operacao_item.Text));
            itemnf.NCM = (txtNCM.Text.Equals("") ? "0" : txtNCM.Text);
            itemnf.Und = txtUnd.Text;
            itemnf.Peso_liquido = Funcoes.decTry((txtPeso_liquido.Text.Equals("") ? "0,00" : txtPeso_liquido.Text));
            itemnf.Peso_Bruto = Funcoes.decTry((txtPeso_Bruto.Text.Equals("") ? "0,00" : txtPeso_Bruto.Text));

            itemnf.CSTPIS = txtCSTPIS.Text;
            itemnf.CSTCOFINS = txtCSTPIS.Text;

            itemnf.PISp = Funcoes.decTry((txtPisItem.Text.Equals("") ? "0,00" : txtPisItem.Text));
            itemnf.COFINSp = Funcoes.decTry((txtCofinsItem.Text.Equals("") ? "0,00" : txtCofinsItem.Text));
            //itemnf.calculaPisCofins();

            itemnf.pCredSN = Funcoes.decTry((txtpCredSN.Text.Equals("") ? "0,00" : txtpCredSN.Text));
            itemnf.vCredicmssn = Funcoes.decTry((txtvCredicmssn.Text.Equals("") ? "0,00" : txtvCredicmssn.Text));
            itemnf.vOrigem = int.Parse(itemnf.origem);
            itemnf.inativo = lblInativo.Text.Equals("inativo");

            Decimal.TryParse(txtItem_BaseFCP.Text, out itemnf.vBCFCP);
            Decimal.TryParse(txtItem_pFCP.Text, out itemnf.vFCP);
            Decimal.TryParse(txtItem_VlrFCP.Text, out itemnf.pFCP);

            Decimal.TryParse(txtItem_BaseFCPST.Text, out itemnf.vBCFCPST);
            Decimal.TryParse(txtItem_pFCPST.Text, out itemnf.pFCPST);
            Decimal.TryParse(txtItem_VlrFCPST.Text, out itemnf.vFCPST);
            itemnf.Data_validade = Funcoes.dtTry(txtDtValidade.Text);
            itemnf.Frete = Funcoes.decTry(txtValorFreteItem.Text);
            itemnf.buscaCentroCusto();
            //Sinto Muito Me Perdoe Agradeço Eu Te Amo.

            Session.Remove("item" + urlSessao());
            Session.Add("item" + urlSessao(), itemnf);
            return itemnf;

        }
        protected void btnConfirmaItens_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                nf_itemDAO itemnf = carregaItem();
                nfDAO nf = (nfDAO)Session["objNfEntrada" + urlSessao()];
                String itemEditado = (String)Session["itemEditado" + urlSessao()];
                if (itemEditado == null || !itemEditado.Equals("true"))
                {
                    nf.atualizarSemCalculo(itemnf);
                    nf.recalcularCentroCusto();
                    ModalItens.Hide();
                    carregarGrids();
                    return;
                }

                if (Funcoes.decTry(txtUnitario.Text) <= 0)
                {
                    txtUnitario.BackColor = System.Drawing.Color.Red;
                    throw new Exception("O Valor Unitario não pode Ser 0");
                }


                if (Funcoes.decTry(txtEmbalagem.Text) <= 0)
                {
                    txtEmbalagem.BackColor = System.Drawing.Color.Red;
                    throw new Exception("A Embalagem não pode Ser 0");
                }

                if (Funcoes.decTry(txtQtde.Text) <= 0)
                {
                    txtQtde.BackColor = System.Drawing.Color.Red;
                    throw new Exception("A Qtde não pode Ser 0");
                }
               

                

                itemnf.Tipo_NF = nf.Tipo_NF;

                int numItem = int.Parse(txtNum_item.Text);
                String msg = "";
                if (numItem > nf.qtdItens())
                {

                    nf.addItem(itemnf);
                    msg = "PLU: " + itemnf.PLU + " Adicionado Com Sucesso";
                }
                else
                {
                    nf.atualizaItem(itemnf);
                    msg = "PLU: " + itemnf.PLU + " Alterado Com Sucesso";
                }
                nf.calculaTotalItens();
                Session.Remove("objNfEntrada" + urlSessao());
                Session.Add("objNfEntrada" + urlSessao(), nf);
                Session.Remove("item" + urlSessao());

                carregarDados();

                showMessage(msg, false);

                ModalItens.Hide();

            }
            catch (Exception err)
            {

                showMessage("ERRO :" + err.Message, true);
                ModalItens.Show();
            }
        }


        protected void exibeLista(string campoLista = "")
        {
            lblErroPesquisa.Text = "";
            carregarDadosObj();
            String campo = "";

            if (campoLista.Equals(""))
            {
                campo = (String)Session["campoLista" + urlSessao()];
            }
            else
            {
                campo = campoLista;
            }


            String sqlLista = "";
            User usr = (User)Session["User"];

            switch (campo)
            {
                case "txtPLU":
                case "txtNovoPlu":
                    lbllista.Text = "Escolha um Produto";
                    //  sqlLista = "select mercadoria.PLU,DESCRICAO,isnull(ean.ean,'') EAN from mercadoria left join ean on ean.plu=mercadoria.plu where mercadoria.plu like '%" + TxtPesquisaLista.Text + "%' or descricao like '%" + TxtPesquisaLista.Text + "%' or (ean like '%" + TxtPesquisaLista.Text + "%') ORDER BY DESCRICAO";
                    sqlLista = "select  mercadoria.PLU,DESCRICAO,isnull(ean.ean,'') EAN,Referencia =mercadoria.ref_fornecedor,isnull(inativo,0) as Inativo from mercadoria left join ean on ean.plu=mercadoria.plu where (mercadoria.plu =  '" + TxtPesquisaLista.Text + "') or(mercadoria.ref_fornecedor like '%" + TxtPesquisaLista.Text + "%')  " +
                               " union all " +
                             "select  mercadoria.PLU,DESCRICAO,isnull(ean.ean,'') EAN,Referencia =mercadoria.ref_fornecedor ,isnull(inativo,0) as Inativo from mercadoria left join ean on ean.plu=mercadoria.plu where (descricao like '%" + TxtPesquisaLista.Text + "%')" +
                             " union all " +
                              "select  mercadoria.PLU,DESCRICAO,isnull(ean.ean,'') EAN ,Referencia =mercadoria.ref_fornecedor ,isnull(inativo,0) as Inativo from mercadoria left join ean on ean.plu=mercadoria.plu where  (ean like '%" + TxtPesquisaLista.Text + "%') ";

                    break;
                case "txtCODIGO_REFERENCIA":
                    lbllista.Text = "Escolha uma mercadoria";
                    sqlLista = "select PLU,DESCRICAO,codigo_referencia [codigo Cliente]  from Fornecedor_Mercadoria where fornecedor='" + txtCliente_Fornecedor.Text + "' and  (plu like '%" + TxtPesquisaLista.Text + "%' or descricao like '%" + TxtPesquisaLista.Text + "%' or (codigo_referencia like '%" + TxtPesquisaLista.Text + "%'))";

                    break;

                case "txtFornecedor_CNPJ":
                case "txtCTeCodigoFornecedor":
                    lbllista.Text = "Escolha um Fornecedor";
                    sqlLista = "select replace(replace(replace(cnpj,'.',''),'-',''),'/','') as CNPJ,Fornecedor from fornecedor where replace(replace(replace(cnpj,'.',''),'-',''),'/','') like '%" + TxtPesquisaLista.Text + "%' or FORNECEDOR like '%" + TxtPesquisaLista.Text + "%'  group by CNPJ,fornecedor";

                    break;
                case "txtcentro_custo":
                    lbllista.Text = "Escolha um Centro de custo";
                    sqlLista = "SELECT  codigo_centro_custo as Codigo , descricao_centro_custo as Descricao  from centro_custo where (codigo_centro_custo like'" + TxtPesquisaLista.Text + "%') or (descricao_centro_custo like '%" + TxtPesquisaLista.Text + "%') order by descricao_centro_custo";
                    break;
                case "txtCodigo_operacao":
                    lbllista.Text = "Escolha um Codigo de operação";
                    sqlLista = "Select Codigo_operacao AS Codigo,Descricao,CASE WHEN Gera_apagar_receber=1 THEN 'SIM'ELSE'NAO' END AS Finaceiro,CASE WHEN Baixa_estoque=1 THEN 'SIM'ELSE'NAO' END AS Estoque, CASE WHEN gera_custo=1 THEN 'SIM'ELSE'NAO' END AS custo , CASE WHEN Imprime_NF=1 THEN 'SIM'ELSE'NAO' END AS ImprimeNF,CASE WHEN Saida=1 THEN 'SIM'ELSE'NAO' END AS SAIDA from natureza_operacao where codigo_operacao like '%" + TxtPesquisaLista.Text + "%' and descricao like '%" + TxtPesquisaLista.Text + "%'  and saida<>1 and imprime_nf=0 AND ISNULL(Inativa, 0) = 0";
                    break;
                case "txtCodigo_operacao_item":
                    lbllista.Text = "Escolha um Codigo de operação";
                    sqlLista = "Select * from cfop where cfop like '%" + TxtPesquisaLista.Text + "%' or Descricao like '%" + TxtPesquisaLista.Text + "%' and tipo =2";
                    break;
                case "txtCodigo_Tributacao":
                    lbllista.Text = "Escolha a Tributação";
                    sqlLista = "select codigo_tributacao Codigo, descricao_tributacao Descrição ,Entrada_ICMS as ICMS, indice_ST as CST, Redutor from tributacao where codigo_tributacao like '%" + TxtPesquisaLista.Text + "%' or Descricao_tributacao like '%" + TxtPesquisaLista.Text + "%'";

                    break;
                case "txtTipoPg":
                    lbllista.Text = "Escolha um Tipo de Pagamento";
                    sqlLista = "select tipo_pagamento Pagamento, Prazo from tipo_pagamento where tipo_pagamento like '%" + TxtPesquisaLista.Text + "%'";
                    break;
                case "txtNCM":
                    lbllista.Text = "Escolha um NCM";
                    sqlLista = "SELECT cf,descricao from cf where cf like '%" + TxtPesquisaLista.Text + "%' or descricao like '%" + TxtPesquisaLista.Text + "%'";
                    break;
                case "txtCSTCOFINS":
                case "txtCSTPIS":
                    lbllista.Text = "Escolha o CST";
                    sqlLista = "Select CST=PIS_CST_entrada , Descricao from pis_cst_entrada where";
                    if (usr.filial.CRT.Equals("2"))
                    {
                        sqlLista += " (PIS_CST_entrada between '70' and '99') and";
                    }
                    sqlLista += " descricao like '%" + TxtPesquisaLista.Text + "%' or PIS_CST_entrada like '%" + TxtPesquisaLista.Text + "%' order by cst";
                    break;
                case "txtCTeMunicipioOrigem":
                case "txtCTeMunicipioDestino":
                    lbllista.Text = "Escolha a cidade";
                    sqlLista = "SELECT munic AS CodigoIBGE, nome_munic AS Municipio from Unidade_Federacao ";
                    break;
            }       

            GridLista.DataSource = Conexao.GetTable(sqlLista, null, true);
            GridLista.DataBind();
            if (lbllista.Text.Equals("Escolha um Produto"))
            {
                foreach (GridViewRow row in GridLista.Rows)
                {
                    if (row.Cells[5].Text.Equals("1"))
                        row.ForeColor = System.Drawing.Color.Red;
                }
                //GridLista.Columns[6].Visible = false;
            }
            if (GridLista.Rows.Count == 1)
            {
                if (!GridLista.Rows[0].Cells[1].Text.Equals("------"))
                {
                    RadioButton rdo = (RadioButton)GridLista.Rows[0].FindControl("RdoListaItem");
                    rdo.Checked = true;
                }
            }
            modalLista.Show();
            TxtPesquisaLista.Focus();
        }
        protected void imgLista_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;


            String or = btn.ID.Substring(7);

            Session.Remove("campoLista" + urlSessao());
            Session.Add("campoLista" + urlSessao(), or);
            TxtPesquisaLista.Text = "";
            exibeLista();
        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            String itemLista = (String)Session["campoLista" + urlSessao()];
            TextBox txt = (TextBox)pnItens.FindControl(itemLista);
            if (txt.Parent.ID.Equals("pnItensFrame"))
            {
                ModalItens.Show();
            }
            carregarGrids();
        }

        protected void GridLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }



        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
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

        protected void btnFechar_Click(object sender, ImageClickEventArgs e)
        {
            String selecionado = ListaSelecionada(1);

            if (!selecionado.Equals("") && !selecionado.Equals("------"))
            {
                User usr = (User)Session["User"];
                String itemLista = (String)Session["campoLista" + urlSessao()];
                nfDAO nf = (nfDAO)Session["objNfEntrada" + urlSessao()];
                if (itemLista.Equals("txtCODIGO_REFERENCIA"))
                {
                    String plu = ListaSelecionada(1);
                    MercadoriaDAO merc = new MercadoriaDAO(plu, usr);
                    txtPLU.Text = plu;
                    txtDescricao.Text = merc.Descricao_resumida;
                    txtEmbalagem.Text = merc.Embalagem.ToString("N2");
                    txtUnitario.Text = merc.preco_compra.ToString("N5");
                    txtCodigo_Tributacao.Text = merc.Codigo_Tributacao_ent.ToString();
                    txtDescontoItem.Text = "0,00";
                    txtQtde.Text = "1,00";
                    txtredutor_base.Text = "0,00";
                    txtdespesas.Text = "0,00";


                    //Pis/Cofins
                    if (nf.NtOperacao.cst_pis_cofins.Trim().Equals(""))
                    {
                        txtCSTPIS.Text = merc.cst_entrada;
                    }
                    else
                    {
                        txtCSTPIS.Text = nf.NtOperacao.cst_pis_cofins;
                    }
                    txtCofinsItem.Text = merc.cofins_perc_entrada.ToString("N2");
                    txtPisItem.Text = merc.pis_perc_entrada.ToString("N2");



                    txtCODIGO_REFERENCIA.Text = ListaSelecionada(3);

                    txtUnd.Text = merc.und;
                    txtPeso_liquido.Text = merc.peso_liquido.ToString();
                    txtPeso_Bruto.Text = merc.peso_bruto.ToString();
                    txtIPI.Text = merc.IPI.ToString("N2");
                    //txtIPIV.Text = ((Funcoes.decTry(TxtTotalItem.Text) * merc.IPI) / 100).ToString("N2");
                    txtmargem_iva.Text = merc.margem_iva.ToString("N2");
                    CalculatotalItem();
                    // txtiva.Text = ((Funcoes.decTry(TxtTotalItem.Text) * merc.margem_iva) / 100).ToString("N2");

                    String strCfop = Conexao.retornaUmValor("SELECT CFOP FROM TRIBUTACAO WHERE CODIGO_TRIBUTACAO =" + txtCodigo_Tributacao.Text, usr);
                    String iniCfop = "";
                    if (nf.UfCliente.Equals(usr.filial.UF))
                    {
                        iniCfop = "1";
                    }
                    else
                    {
                        iniCfop = "2";
                    }

                    txtCodigo_operacao_item.Text = iniCfop + strCfop;

                    txtNum_item.Text = (nf.qtdItens() + 1).ToString();
                    txtNCM.Text = merc.cf;
                    ItemInativo(merc.Inativo == 1);
                    Session.Add("itemEditado" + urlSessao(), "true");
                    ModalItens.Show();
                }
                else if (itemLista.Equals("txtNovoPlu"))
                {
                    txtNovoPlu.Text = ListaSelecionada(1);
                    MercadoriaDAO merc = new MercadoriaDAO(txtNovoPlu.Text, usr);
                    txtNovoDescricao.Text = merc.Descricao;
                    txtNovoCodTributacao.Text = merc.Codigo_Tributacao.ToString();
                    txtNovoDescricaoTributacao.Text = merc.descricaoTributacao;
                    txtNovoCodGrupo.Text = merc.codigo_Grupo;
                    txtNovoDescGrupo.Text = merc.descricao_Grupo;
                    txtNovoCodSubGrupo.Text = merc.codigo_subGrupo;
                    txtNovoDescSubGrupo.Text = merc.descricao_subGrupo;
                    txtNovoCodDepartamento.Text = merc.Codigo_departamento;
                    txtNovoDescDepartamento.Text = merc.Descricao_departamento;
                    modalCadastroProdutoDetalhes.Show();
                }
                else
                {
                    TextBox txt = (TextBox)cabecalho.FindControl(itemLista);

                    txt.Text = ListaSelecionada(1);


                    if (txt.ID.Equals("txtCodigo_operacao"))
                    {
                        
                        Decimal codOpe = 0;
                        Decimal.TryParse(txt.Text, out codOpe);
                        nf.Codigo_operacao = codOpe;
                        nf.atribuirCodOperacaoEntrada();
                        //nf.calculaTotalItens();
                        if (txtFornecedor_CNPJ.Text.Equals(""))
                        {
                            ModalImportar.Show();
                        }

                        carregarGrids();
                        carregarDados();
                    }


                    if (txt.ID.Equals("txtPLU"))
                    {
                        MercadoriaDAO merc = null;
                        try
                        {
                            merc = new MercadoriaDAO(txt.Text, usr);
                        }
                        catch (Exception err)
                        {
                            showMessage(err.Message, true);
                            return;
                        }

                        if (txtCODIGO_REFERENCIA.Text.Equals("") || txtCODIGO_REFERENCIA.Text.Equals("0"))
                        {
                            txtDescricao.Text = merc.Descricao_resumida;
                            txtEmbalagem.Text = merc.Embalagem.ToString("N2");
                            txtUnitario.Text = merc.preco_compra.ToString("N5");
                            txtCodigo_Tributacao.Text = merc.Codigo_Tributacao_ent.ToString();
                            txtDescontoItem.Text = "0,00";
                            txtQtde.Text = "1,00";
                            txtredutor_base.Text = "0,00";
                            txtdespesas.Text = "0,00";

                            if (nf.NtOperacao.cst_pis_cofins.Trim().Equals(""))
                            {
                                txtCSTPIS.Text = merc.cst_entrada;
                            }
                            else
                            {
                                txtCSTPIS.Text = nf.NtOperacao.cst_pis_cofins;
                            }
                            txtCofinsItem.Text = merc.cofins_perc_entrada.ToString("N2");
                            txtPisItem.Text = merc.pis_perc_entrada.ToString("N2");



                            txtCODIGO_REFERENCIA.Text = "0";

                            txtUnd.Text = merc.und;
                            txtPeso_liquido.Text = merc.peso_liquido.ToString();
                            txtPeso_Bruto.Text = merc.peso_bruto.ToString();
                            txtIPI.Text = merc.IPI.ToString("N2");

                            txtmargem_iva.Text = merc.margem_iva.ToString("N2");

                            CalculatotalItem();


                            //txtIPIV.Text = ((Funcoes.decTry(TxtTotalItem.Text) * merc.IPI) / 100).ToString("N2");
                            // txtiva.Text = ((Funcoes.decTry(TxtTotalItem.Text) * merc.margem_iva) / 100).ToString("N2");

                            String strCfop = Conexao.retornaUmValor("SELECT CFOP FROM TRIBUTACAO WHERE CODIGO_TRIBUTACAO =" + txtCodigo_Tributacao.Text, usr);
                            String iniCfop = "";
                            if (nf.UfCliente.Equals(usr.filial.UF))
                            {
                                iniCfop = "1";
                            }
                            else
                            {
                                iniCfop = "2";
                            }

                            txtCodigo_operacao_item.Text = iniCfop + strCfop;
                            if (txtNum_item.Text.Equals(""))
                            {
                                txtNum_item.Text = (nf.qtdItens() + 1).ToString();
                            }
                            txtNCM.Text = merc.cf;

                        }
                        else
                        {
                            txtDescricao.Text = merc.Descricao_resumida;

                        }
                        Session.Add("itemEditado" + urlSessao(), "true");
                        ItemInativo(merc.Inativo == 1 );
                    }

                    if (txt.ID.Equals("txtFornecedor_CNPJ"))
                    {
                        fornecedorDAO forn = new fornecedorDAO(txt.Text, usr);
                        txtCliente_Fornecedor.Text = forn.Fornecedor;
                        TxtNomeFornecedor.Text = forn.Razao_social;
                        if (forn.centro_custo.Equals(""))
                        {
                            txtcentro_custo.Text = Funcoes.valorParametro("CENTRO_CUSTO_NF_ENTRADA", usr);
                        }
                        else
                        {
                            txtcentro_custo.Text = forn.centro_custo;
                        }
                    }

                    if (txt.ID.Equals("txtCodigo_Tributacao"))
                    {
                        String strCfop = Conexao.retornaUmValor("SELECT CFOP FROM TRIBUTACAO WHERE CODIGO_TRIBUTACAO =" + txtCodigo_Tributacao.Text, usr);
                        String iniCfop = "";
                        if (nf.UfCliente.Equals(usr.filial.UF))
                        {
                            iniCfop = "1";
                        }
                        else
                        {
                            iniCfop = "2";
                        }

                        txtCodigo_operacao_item.Text = iniCfop + strCfop;
                    }

                    if (txt.ID.Equals("txtCodigo_Tributacao") || txt.ID.Equals("txtCSTCOFINS") || txt.ID.Equals("txtCSTPIS"))
                    {
                        CalculatotalItem();
                        Session.Add("itemEditado" + urlSessao(), "true");
                        ModalItens.Show();

                    }


                    if (txt.Parent.ID.Equals("pnItensFrame"))
                    {
                        Session.Add("itemEditado" + urlSessao(), "true");
                        ModalItens.Show();
                    }

                    if (txt.Parent.ID.Equals("PnPagamentoFrame"))
                    {
                        try
                        {
                            DateTime emissao = DateTime.Parse(txtEmissao.Text);
                            int d = int.Parse(Conexao.retornaUmValor("Select prazo from tipo_pagamento where tipo_pagamento ='" + txt.Text + "'", usr));
                            txtVencimentoPg.Text = emissao.AddDays(d).ToString("dd/MM/yyyy");
                            ModalPagamentos.Show();
                        }
                        catch (Exception)
                        {

                        }
                    }
                }

                modalLista.Hide();
                try
                {
                    carregarGrids();

                }
                catch (Exception err)
                {
                    showMessage(err.Message, true);
                }

            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                modalLista.Show();
            }

        }

        private void habilitarItensTributacao()
        {
            User usr = (User)Session["User"];
            tributacaoDAO trib = new tributacaoDAO(txtCodigo_Tributacao.Text, usr);
            EnabledControls(pnItensFrame, true);
            txtaliquota_icms.Text = trib.Entrada_ICMS.ToString("N2");

            txtIndice_st.Text = trib.Indice_ST;


            if (trib.Indice_ST != null)
            {
                switch (trib.Indice_ST.Trim())
                {
                    case "00":

                        txtredutor_base.Enabled = false;
                        txtredutor_base.Text = "0,00";
                        txtredutor_base.BackColor = Color.FromArgb(0xDCDCDC);
                        txtiva.Enabled = false;
                        txtiva.Text = "0,00";
                        txtiva.BackColor = Color.FromArgb(0xDCDCDC);
                        txtmargem_iva.Enabled = false;
                        txtmargem_iva.Text = "0,00";
                        txtmargem_iva.BackColor = Color.FromArgb(0xDCDCDC);

                        txtaliquota_icms.Text = trib.Entrada_ICMS.ToString("N2");
                        break;

                    case "30":
                        txtredutor_base.Enabled = false;
                        txtredutor_base.Text = "0,00";
                        txtredutor_base.BackColor = Color.FromArgb(0xDCDCDC);

                        break;
                    case "20":
                    case "101":
                    case "102":
                    case "300":
                    case "400":
                    case "500":
                    case "900":
                        txtiva.Enabled = false;
                        txtiva.Text = "0,00";
                        txtiva.BackColor = Color.FromArgb(0xDCDCDC);
                        txtmargem_iva.Enabled = false;
                        txtmargem_iva.Text = "0,00";
                        txtmargem_iva.BackColor = Color.FromArgb(0xDCDCDC);

                        txtaliquota_icms.Text = trib.Entrada_ICMS.ToString("N2");
                        txtredutor_base.Text = trib.Redutor.ToString("N2");

                        break;

                    case "40":
                    case "41":
                    case "50":
                    case "51":
                    case "60":
                        txtaliquota_icms.Enabled = false;
                        txtaliquota_icms.Text = "0.00";
                        txtaliquota_icms.BackColor = Color.FromArgb(0xDCDCDC);
                        txtredutor_base.Enabled = false;
                        txtredutor_base.Text = "0,00";
                        txtredutor_base.BackColor = Color.FromArgb(0xDCDCDC);
                        txtiva.Enabled = false;
                        txtiva.Text = "0,00";
                        txtiva.BackColor = Color.FromArgb(0xDCDCDC);
                        txtmargem_iva.Enabled = false;
                        txtmargem_iva.Text = "0,00";
                        txtmargem_iva.BackColor = Color.FromArgb(0xDCDCDC);


                        break;
                    case "70":
                    case "10":
                        txtaliquota_icms.Text = trib.Entrada_ICMS.ToString("N2");
                        txtredutor_base.Text = trib.Redutor.ToString("N2");
                        break;

                }
            }
        }    
        private void CalculatotalItem()
        {
           

            habilitarItensTributacao();

            nf_itemDAO item = carregaItem();
            item.CalculaImpostos();

            TxtTotalItem.Text = item.vtotal_produto.ToString("N2");
            txtTotalCustoItem.Text = item.TotalCustoUnitario.ToString("N2");

            txtVlrCofinsItem.Text = item.COFINSV.ToString("N2");
            txtVlrPisItem.Text = item.PISV.ToString("N2");

            if (item.vmargemIva > 0 && txtiva.Text.Equals(""))
                txtiva.Text = item.CalculoIva().ToString("N2");
            else
                if (txtiva.Text.Equals(""))
                txtiva.Text = item.vIva.ToString("N2");

            if (item.IPI > 0)
                txtIPIV.Text = item.IPIV.ToString("N2");
            else
                txtIPIV.Text = "0,00";

            if (item.Desconto > 0)
                txtDescValorItem.Text = item.DescontoValor.ToString("N2");
            else
                txtDescValorItem.Text = "0,00";

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

        protected void btnCancelaItem_Click(object sender, ImageClickEventArgs e)
        {
            carregarGrids();
            Session.Remove("item" + urlSessao());
        }

        protected void ImgExcluiItem_Click(object sender, ImageClickEventArgs e)
        {
            nfDAO nf = (nfDAO)Session["objNfEntrada" + urlSessao()];
            nf.removeItem(nf.item(int.Parse(txtNum_item.Text) - 1));
            carregarDados();
            Session.Remove("objNfEntrada" + urlSessao());
            Session.Add("objNfEntrada" + urlSessao(), nf);
            PnExcluirItem.Visible = false;
        }

        protected void gridPagamentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            nfDAO nf = (nfDAO)Session["objNfEntrada" + urlSessao()];
            nf.removePagamento(index);
            carregarGrids();
        }



        protected void AddPagamento_Click(object sender, ImageClickEventArgs e)
        {
            if (!txtCodigo.Text.Equals("") && !txtCliente_Fornecedor.Text.Equals(""))
            {
                lblError.Text = "";
                LimparCampos(PnAddPagamento);
                ModalPagamentos.Show();
                nfDAO nf = (nfDAO)Session["objNfEntrada" + urlSessao()];
                txtValorPg.Text = (nf.Total - nf.TotalPag()).ToString("N2");
            }
            else
            {
                showMessage("Inclua as Informações do Codigo e Fornecedor", true);
            }
        }



        protected void btnConfirmaPagamentos_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                nfDAO nf = (nfDAO)Session["objNfEntrada" + urlSessao()];
                User usr = (User)Session["user"];
                nf_pagamentoDAO pg = new nf_pagamentoDAO(usr);
                pg.Codigo = txtCodigo.Text;
                pg.Cliente_Fornecedor = txtCliente_Fornecedor.Text;
                pg.Tipo_NF = "2";
                pg.Tipo_pagamento = txtTipoPg.Text;
                pg.Vencimento = DateTime.Parse(txtVencimentoPg.Text);
                pg.Valor = Funcoes.decTry(txtValorPg.Text);
                pg.cod_barras = txtCodBarra.Text;
                nf.addPagamento(pg);
                lblTotalPagamentos.Text = nf.TotalPag().ToString("N2");
                Session.Remove("objNfEntrada" + urlSessao());
                Session.Add("objNfEntrada" + urlSessao(), nf);
                carregarGrids();
            }
            catch (Exception)
            {
                lblErroPg.Text = "Pagamento Invalido";
                ModalPagamentos.Show();
            }


        }

        protected void btnCancelaPagamentos_Click(object sender, ImageClickEventArgs e)
        {
            ModalPagamentos.Hide();
        }

        protected void btnConfirmaImportar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                bool cadAutomatico = Funcoes.valorParametro("NF_CADASTRO_AUTO", null).ToUpper().Equals("TRUE");
                txtEmissao.Enabled = false;

                if (txtNFE.Text.Equals("") && txtNumeroPedido.Text.Equals(""))
                {
                    lblErroImportacao.Text = "Informe a Chave da Nota Fiscal ou o Pedido de Compra";
                    ModalImportar.Show();
                }
                else if (txtNumeroPedido.Text.Equals("") && txtNFE.Text.Length != 44)
                {
                    lblErroImportacao.Text = "Chave INVÁLIDA!! Favor informar chave com 44 numeros.";
                    ModalImportar.Show();
                }
                else
                {
                    nfDAO nf = (nfDAO)Session["objNfEntrada" + urlSessao()];
                    ModalImportar.Hide();
                    if (txtNumeroPedido.Text.Equals(""))
                    {
                        try
                        {
                            if (!txtNFE.Text.Equals(""))
                            {
                                nf.notaEntradaManual = false;
                            }
                            nf.importarNFe(txtNFE.Text);
                        }
                        catch (Exception err)
                        {
                            
                            if( err.Message.Contains("Fornecedor não Cadastrado"))
                            {
                                
                                if(cadAutomatico)
                                    showNovoFornecedor();
                                else
                                    throw err;
                            }
                            if (err.Message.Contains("NFe não Existe na Base de Dados. Favor Checar arquivo XML."))
                            {
                                String nfeExist = Conexao.retornaUmValor("Select  nfeXML from nf_manifestar where chave ='" + txtNFE.Text + "'", null);
                                if (nfeExist.Length > 0)
                                {
                                    XML_NFe.XML_NFE.Salva_XML_NFE(nfeExist, true);
                                    nf.importarNFe(txtNFE.Text);
                                }
                                //else
                                //{
                                //    lblChaveConsultarSefaz.Text = txtNFE.Text;
                                //    modalConsultarSefaz.Show();
                                //}
                            }
                            else
                            {
                                throw err;
                            }

                        }
                    }
                    else
                    {
                        nf.importarPedido(txtNumeroPedido.Text, 2);
                    }
                    User usr = (User)Session["User"];

                    if (nf.DestFornecedor && !nf.Cliente_Fornecedor.Equals(""))
                    {
                        String strCentroCusto = Conexao.retornaUmValor("Select centro_custo from fornecedor where ltrim(rtrim(fornecedor))='" + nf.Cliente_Fornecedor.Trim() + "'", usr);

                        if (strCentroCusto.Equals(""))
                        {
                            nf.centro_custo = Funcoes.valorParametro("CENTRO_CUSTO_NF_ENTRADA", usr);
                        }
                        else
                        {
                            nf.centro_custo = strCentroCusto;
                        }
                    }
                    else
                    {
                        nf.centro_custo = Funcoes.valorParametro("CENTRO_CUSTO_NF_ENTRADA", usr);
                    }
                    Session.Remove("objNfEntrada" + urlSessao());
                    Session.Add("objNfEntrada" + urlSessao(), nf);
                    //Colocar aqui a mensagem de aviso.
                    int diasEmissaoEntrada = 0;
                    int.TryParse(Funcoes.valorParametro("CRITICA_DIAS_EMI_ENT", usr), out diasEmissaoEntrada);
                    double diasDiferenca =  nf.Emissao.Subtract(DateTime.Today).TotalDays; //DateTime.Today.Subtract(nf.Emissao).TotalDays; 

                    diasDiferenca = (diasDiferenca < 0 ? diasDiferenca * -1 : diasDiferenca);
                    if (diasEmissaoEntrada > 0 && diasDiferenca > diasEmissaoEntrada)
                    {
                        msgShow("Atenção! O documento fiscal está acima do prazo de " + diasEmissaoEntrada.ToString() + " dias entre a data de emissão " + nf.Emissao.ToString("yyyy-MM-dd") + " e a data atual " + DateTime.Today.ToString("yyyy-MM-dd") + " para entrada no Sistema.");
                    }
                    txtEmissao.Enabled = false;
                    carregarDados();
                    if(cadAutomatico)
                        this.verificarProdutosNovos();

                }
            }
            catch (Exception err)
            {

                lblErroImportacao.Text = err.Message;
                ModalImportar.Show();
            }


        }

       

        protected void btnCancelaImportar_Click(object sender, ImageClickEventArgs e)
        {
            txtData.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtEmissao.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtFrete.Text = "0,00";
            txtDespesas_financeiras.Text = "0,00";
            txtSeguro.Text = "0,00";
            txtOutras.Text = "0,00";
            txtIPI_Nota.Text = "0,00";
            txtDesconto_geral.Text = "0,00";
            txtICMS_Nota.Text = "0,00";
            txtICMS_ST.Text = "0,00";
            txtBase_Calculo.Text = "0,00";
            txtBase_Calc_Subst.Text = "0,00";
            txtDesconto.Text = "0,00";
            txtTotal.Text = "0,00";
            //txtCodigo_operacao.Text = "";
            User usr = (User)Session["User"];
            txtSerie.Text = usr.filial.serie_nfe.ToString();
            txtcentro_custo.Text = Funcoes.valorParametro("CENTRO_CUSTO_NF_ENTRADA", usr);
            habilitarCampos(true);
        }

        protected void itemEditado()
        {
            Session.Remove("itemEditado" + urlSessao());
            Session.Add("itemEditado" + urlSessao(), "true");

        }
        protected void txt_TextChanged(object sender, EventArgs e)
        {
            // carregarDadosItens(carregaItem());
            lblErroItem.Text = "";
            ((TextBox)sender).BackColor = System.Drawing.Color.White;


            try
            {

                String id = ((TextBox)sender).ID;
                nf_itemDAO itemnf = (nf_itemDAO)Session["item" + urlSessao()];
                switch (id)
                {
                   
                    case "txtQtde":

                        if (itemnf.Qtde != Funcoes.decTry(txtQtde.Text))
                        {   
                            itemEditado();
                            itemnf.Qtde = Funcoes.decTry(txtQtde.Text);
                            txtUnitario.Text = itemnf.Unitario.ToString("N5");

                            if (txtid.Text.Trim().Equals(""))
                            {
                                itemnf.vIpiv = 0;
                                txtIPIV.Text = itemnf.IPIV.ToString("N2");
                                txtiva.Text = itemnf.CalculoIva().ToString("N2");
                                itemnf.CalculaImpostos();
                                txtVlrPisItem.Text = itemnf.PISV.ToString("N2");
                                txtVlrCofinsItem.Text = itemnf.COFINSV.ToString("N2");
                            }
                          
                            
                        }
                        txtEmbalagem.Focus();
                        break;
                    case "txtEmbalagem":
                        if (itemnf.Embalagem != Funcoes.decTry(txtEmbalagem.Text))
                        {
                            itemEditado();
                            itemnf.Embalagem = Funcoes.decTry(txtEmbalagem.Text);
                            txtUnitario.Text = itemnf.Unitario.ToString("N5");

                            if (txtid.Text.Trim().Equals(""))
                            {
                                itemnf.vIpiv = 0;
                                txtIPIV.Text = itemnf.IPIV.ToString("N2");
                                txtiva.Text = itemnf.CalculoIva().ToString("N2");
                                itemnf.CalculaImpostos();
                                txtVlrPisItem.Text = itemnf.PISV.ToString("N2");
                                txtVlrCofinsItem.Text = itemnf.COFINSV.ToString("N2");
                            }                            
                        }
                        txtUnitario.Focus();
                        break;
                    case "txtUnitario":
                        if (itemnf.Unitario != Funcoes.decTry(txtUnitario.Text))
                        {
                            itemEditado();
                            itemnf.Unitario = Funcoes.decTry(txtUnitario.Text);
                            itemnf.vIpiv = 0;
                            txtIPIV.Text = itemnf.IPIV.ToString("N2");
                            txtiva.Text = itemnf.CalculoIva().ToString("N2");
                            txtCodigo_Tributacao.Focus();
                            itemnf.CalculaImpostos();
                            txtVlrPisItem.Text = itemnf.PISV.ToString("N2");
                            txtVlrCofinsItem.Text = itemnf.COFINSV.ToString("N2");
                        }
                        txtCodigo_Tributacao.Focus();
                        break;
                    case "txtDescontoItem":
                        if (itemnf.Desconto != Funcoes.decTry(txtDescontoItem.Text))
                        {
                            itemEditado();
                            itemnf.Desconto = Funcoes.decTry(txtDescontoItem.Text);
                            txtDescValorItem.Text = itemnf.Desconto.ToString("N2");
                            itemnf.vIpiv = 0;
                            txtIPIV.Text = itemnf.IPIV.ToString("N2");
                            txtiva.Text = itemnf.CalculoIva().ToString("N2");
                            txtIPI.Focus();
                            itemnf.CalculaImpostos();
                            txtVlrPisItem.Text = itemnf.PISV.ToString("N2");
                            txtVlrCofinsItem.Text = itemnf.COFINSV.ToString("N2");
                        }
                        break;
                    case "txtDescValorItem":

                        if (itemnf.DescontoValor != Funcoes.decTry(txtDescValorItem.Text))
                        {
                            itemEditado();
                            itemnf.DescontoValor = Funcoes.decTry(txtDescValorItem.Text);
                            txtDescontoItem.Text = itemnf.Desconto.ToString("N4");
                            itemnf.vIpiv = 0;
                            txtIPIV.Text = itemnf.IPIV.ToString("N2");
                            txtiva.Text = itemnf.CalculoIva().ToString("N2");
                            itemnf.CalculaImpostos();
                            txtVlrPisItem.Text = itemnf.PISV.ToString("N2");
                            txtVlrCofinsItem.Text = itemnf.COFINSV.ToString("N2");
                        }
                        txtIPI.Focus();

                        break;
                    case "txtdespesas":
                        if (itemnf.despesas != Funcoes.decTry(txtdespesas.Text))
                        {
                            itemEditado();
                            itemnf.despesas = Funcoes.decTry(txtdespesas.Text);
                            itemnf.CalculaImpostos();
                        }
                        txtIPI.Focus();
                        break;
                    case "txtIPI":
                        if (itemnf.IPI != Funcoes.decTry(txtIPI.Text))
                        {
                            itemEditado();
                            itemnf.IPI = Funcoes.decTry(txtIPI.Text);
                            txtIPI.Text = itemnf.IPI.ToString("N2");
                            txtIPIV.Text = itemnf.IPIV.ToString("N2");
                            itemnf.CalculaImpostos();
                            txtiva.Text = itemnf.CalculoIva().ToString("N2");
                        }
                        txtIPIV.Focus();
                        break;
                    case "txtIPIV":
                        if (itemnf.IPIV != Funcoes.decTry(txtIPIV.Text))
                        {
                            itemEditado();
                            itemnf.IPIV = Funcoes.decTry(txtIPIV.Text);
                            txtIPIV.Text = itemnf.IPIV.ToString("N2");
                            txtIPI.Text = itemnf.IPI.ToString("N2");
                            itemnf.CalculaImpostos();
                            txtiva.Text = itemnf.CalculoIva().ToString("N2");
                        }
                        txtmargem_iva.Focus();
                        break;
                    case "txtmargem_iva":
                        if (itemnf.vmargemIva != Funcoes.decTry(txtmargem_iva.Text))
                        {
                            itemEditado();
                            itemnf.vmargemIva = Funcoes.decTry(txtmargem_iva.Text);
                            txtmargem_iva.Text = itemnf.vmargemIva.ToString("N2");
                            txtiva.Text = itemnf.CalculoIva().ToString("N2");
                        }
                        if (txtiva.Enabled)
                            txtiva.Focus();
                        else
                            txtaliquota_icms.Focus();
                        break;
                    case "txtiva":
                        if(itemnf.vIva != Funcoes.decTry(txtiva.Text))
                        {
                            itemEditado();
                            itemnf.vIva = Funcoes.decTry(txtiva.Text);
                            txtmargem_iva.Text = itemnf.CalculoMargem_iva.ToString("N2");
                        }
                        txtaliquota_icms.Focus();
                        break;
                    case "txtaliquota_icms":
                        if (txtredutor_base.Enabled)
                            txtredutor_base.Focus();
                        else
                            txtPisItem.Focus();
                        break;
                    case "txtredutor_base":
                        if (itemnf.redutor_base != Funcoes.decTry(txtredutor_base.Text))
                        {
                            itemEditado();
                            itemnf.redutor_base = Funcoes.decTry(txtredutor_base.Text);
                            itemnf.CalculaImpostos();
                        }
                        txtPisItem.Focus();
                        break;
                    case "txtPisItem":
                        if (itemnf.PISp != Funcoes.decTry(txtPisItem.Text))
                        {
                            itemEditado();
                            itemnf.PISp = Funcoes.decTry(txtPisItem.Text);
                            itemnf.calculaPisCofins();
                            txtVlrPisItem.Text = itemnf.PISV.ToString("N2");
                        }
                        txtCofinsItem.Focus();

                        break;
                    case "txtCofinsItem":
                        if (itemnf.COFINSp != Funcoes.decTry(txtCofinsItem.Text))
                        {
                            itemEditado();
                            itemnf.COFINSp = Funcoes.decTry(txtCofinsItem.Text);
                            itemnf.calculaPisCofins();
                            txtVlrCofinsItem.Text = itemnf.COFINSV.ToString("N2");
                        }
                        txtCSTPIS.Focus();

                        break;
                    case "txtCodigo_Tributacao":
                        txtiva.Text = itemnf.CalculoIva().ToString("N2"); ;
                        break;
                    case "txtPLU":
                        itemnf.PLU = txtPLU.Text;
                        MercadoriaDAO merc = null;
                        try
                        {
                            merc = new MercadoriaDAO(txtPLU.Text, null);
                            txtDescricao.Text = merc.Descricao;
                            itemnf.Descricao = merc.Descricao;
                        }
                        catch (Exception err)
                        {
                            showMessage(err.Message, true);
                            return;
                        }
                        txtCODIGO_REFERENCIA.Focus();
                        break;
                    case "txtCODIGO_REFERENCIA":
                        txtDescricao.Focus();
                        break;

                    case "txtDescricao":
                        txtUnd.Focus();
                        break;

                }


                TxtTotalItem.Text = itemnf.vtotal_produto.ToString("N2");
                txtTotalCustoItem.Text = itemnf.TotalCustoUnitario.ToString("N2");


                Session.Remove("item" + urlSessao());
                Session.Add("item" + urlSessao(), itemnf);
            }
            catch (Exception)
            {

                lblErroItem.Text = "Valor Inválido";
                lblErroItem.ForeColor = System.Drawing.Color.Red;
                ((TextBox)sender).BackColor = System.Drawing.Color.Red;
                ((TextBox)sender).Focus();
            }
            ModalItens.Show();
        }
        protected void TxtPesquisaLista_TextChanged(object sender, EventArgs e)
        {
            exibeLista();
            modalLista.Show();
        }

        protected void btnConfirmaPgAdd_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                SalvarNota(false);

            }
            catch (Exception err)
            {

                showMessage(err.Message, true);
            }

        }

        protected void btnCancelaPgAdd_Click(object sender, ImageClickEventArgs e)
        {
            modalConfirmaPgAdd.Hide();
        }

        protected void txtCalcula_TextChanged(object sender, EventArgs e)
        {
            carregarDadosObj();
            carregarDados();
        }

        protected void txtFornecedor_CNPJ_TextChanged(object sender, EventArgs e)
        {
            if (!txtFornecedor_CNPJ.Text.Equals(""))
            {
                fornecedorDAO forn = new fornecedorDAO(txtFornecedor_CNPJ.Text, new User());
                txtFornecedor_CNPJ.Text = forn.CNPJ;
                txtCliente_Fornecedor.Text = forn.Fornecedor;
                TxtNomeFornecedor.Text = forn.Razao_social;
            }
        }

        protected void txtiva_TextChanged(object sender, EventArgs e)
        {
            nf_itemDAO item = carregaItem();
            if (!txtiva.Text.Equals(""))
            {
                item.vIva = Funcoes.decTry(txtiva.Text);
                Session.Remove("item" + urlSessao());
                Session.Add("item" + urlSessao(), item);
            }
            ModalItens.Show();
        }

        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                nfDAO obj = (nfDAO)Session["objNfEntrada" + urlSessao()];

                User usr = (User)Session["user"];
                if (obj.Data <= usr.filial.dtFechamentoEstoque)
                {
                    showMessage("Data de entrada não pode ser menor ou igual a data de fechamento do estoque [" + usr.filial.dtFechamentoEstoque.ToString("dd-MM-yyyy") + "]", true);
                    return;
                }
                //else
                //{
                //    obj = null;
                //}

                obj.excluir();
                modalPnConfirma.Hide();
                showMessage("Registro Cancelado com sucesso", true);
                limparCampos();
                status = "pesquisar";
                carregabtn(pnBtn);
                Session.Remove("objNfEntrada" + urlSessao());
            }
            catch (Exception err)
            {
                showMessage("Não foi possivel Cancelar a Nota pelo error:" + err.Message, true);
                carregabtn(pnBtn);
            }
        }
        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            modalPnConfirma.Hide();
            carregarDados();
        }



        protected void imgBtnMostraPorcCredito_Click(object sender, ImageClickEventArgs e)
        {
            modalCorrigiPorc.Show();
            txtValorPorcCredito.Focus();

        }

        protected void imgBtnConfirmaPorcCredito_Click(object sender, ImageClickEventArgs e)
        {
            Label2.ForeColor = System.Drawing.Color.Black;
            Label2.Text = "Infome o Valor do Credito";
            if (txtValorPorcCredito.Text.Trim().Equals(""))
            {
                Label2.Text = "PREENCHA O VALOR !";
                Label2.ForeColor = System.Drawing.Color.Red;
                modalCorrigiPorc.Show();
            }
            else
            {

                nfDAO nf = (nfDAO)Session["objNfEntrada" + urlSessao()];
                nf.vCredicmssn = 0;
                foreach (nf_itemDAO item in nf.NfItens)
                {
                    if (item.indice_St.Equals("101") || item.indice_St.Equals("102"))
                    {
                        Decimal.TryParse(txtValorPorcCredito.Text, out item.pCredSN);
                        Decimal vCredSN = Decimal.Round(((item.vtotal_produto + item.Frete) * item.pCredSN) / 100, 2);
                        item.vCredicmssn = vCredSN;
                        nf.vCredicmssn += vCredSN;
                        String strSQlTrib = "SELECT TOP 1 Codigo_Tributacao FROM Tributacao WHERE Indice_ST = '" + item.indice_St + "' AND Entrada_ICMS=" + item.pCredSN.ToString("N2").Replace(",", ".");

                        String codTrib = Conexao.retornaUmValor(strSQlTrib, null);
                        if (codTrib.Equals(""))
                        {
                            String strInsertTrib = "INSERT INTO TRIBUTACAO (Codigo_Tributacao,Descricao_Tributacao,Filial,Saida_ICMS,Nro_ECF,Gera_Mapa,Indice_ST,Entrada_ICMS,Redutor,Incide_ICMS,Incide_ICM_Subistituicao,ICMS_Efetivo,csosn,cst_sped)" +
                            "VALUES((SELECT MAX(Codigo_Tributacao)+1 FROM Tributacao)," +
                            "'CSOSN " + item.indice_St + " " + txtValorPorcCredito.Text.Replace(",", ".") + "%'," +
                            "'MATRIZ'," +
                            txtValorPorcCredito.Text.Replace(",", ".") + "," +
                            "4," +
                            "0," +
                            "'" + item.indice_St + "'," +
                            txtValorPorcCredito.Text.Replace(",", ".") + "," +
                            "0," +
                            "1," +
                            "0," +
                            "0," +
                            "'" + item.indice_St + "'" +
                            ",'00'" +
                            ")";
                            Conexao.executarSql(strInsertTrib);
                            codTrib = Conexao.retornaUmValor(strSQlTrib, null);
                        }
                        item.Codigo_Tributacao = Funcoes.decTry(codTrib);
                    }


                }
                Session.Remove("objNfEntrada" + urlSessao());
                Session.Add("objNfEntrada" + urlSessao(), nf);

                carregarDados();
                modalCorrigiPorc.Hide();
            }
        }

        protected void imgBtnCancelaPorcCredito_Click(object sender, ImageClickEventArgs e)
        {
            modalCorrigiPorc.Hide();
        }

        protected void imgBtnConfirmaEdicao_Click(object sender, ImageClickEventArgs e)
        {
            if (txtJustificativaEdicao.Text.Trim().Length > 3)
            {
                try
                {
                    SalvarNota(true);
                    User usr = (User)Session["User"];
                    nfDAO nf = (nfDAO)Session["objNfEntrada" + urlSessao()];
                    nf_justificativa_edicaoDAO just = new nf_justificativa_edicaoDAO()
                    {
                        filial = nf.Filial,
                        tipo_nf = Funcoes.intTry(nf.Tipo_NF),
                        codigo_nota = nf.Codigo,
                        cliente_fornecedor = nf.Cliente_Fornecedor,
                        serie = nf.serie,
                        usuario = usr.getUsuario(),
                        data_alteracao = DateTime.Now,
                        justificativa = txtJustificativaEdicao.Text
                    };
                    just.salvar();
                    nf.histEdicao.Add(just);
                    carregarDados();
                    habilitarCampos(false);
                }
                catch (Exception err)
                {
                    showMessage(err.Message, true);
                }


                modalConfirmaEdicao.Hide();
            }
            else
            {
                lblErroConfirmaEdicao.Text = "Inclua uma Justificativa Valida! ";
                modalConfirmaEdicao.Show();
            }

        }

        protected void imgBtnCancelaEdicao_Click(object sender, ImageClickEventArgs e)
        {
            modalConfirmaEdicao.Hide();
        }

        protected void btnOkError_Click(object sender, EventArgs e)
        {
            modalError.Hide();
            switch (lblErroPanel.Text)
            {
                case "ERRO :A Embalagem não pode Ser 0":
                    txtEmbalagem.BackColor = System.Drawing.Color.Red;
                    ModalItens.Show();
                    break;
                case "ERRO :O Valor Unitario não pode Ser 0":
                    txtUnitario.BackColor = System.Drawing.Color.Red;
                    ModalItens.Show();
                    break;
                case "ERRO :A Qtde não pode Ser 0":
                    txtQtde.BackColor = System.Drawing.Color.Red;
                    ModalItens.Show();
                    break;
            }
            if (lblErroPanel.Text.Contains("NOVO-FORNECEDOR"))
            {
                modalNovoFornecedor.Show();
            }
            if (lblErroPanel.Text.Contains("NOVO-PRODUTO:"))
            {
                modalCadastroNovosProdutos.Show();
            }
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

        protected void txtFrete_TextChanged(object sender, EventArgs e)
        {
            nfDAO nf = (nfDAO)Session["objNfEntrada" + urlSessao()];

            Decimal vlr = Funcoes.decTry(txtFrete.Text);
            nf.rateiaFrete(vlr);
            nf.calculaTotalItens();
            carregarDados();
        }
        protected void showNovoFornecedor()
        {
            nfDAO nf = (nfDAO)Session["objNfEntrada" + urlSessao()];
            fornecedorDAO fornec = nf.novoFornecedorNfe(txtNFE.Text);
            txtNovoFornecedor.Text = fornec.Fornecedor;
            txtNovoRazaoSocial.Text = fornec.Razao_social;
            txtNovoFantasia.Text = fornec.Nome_Fantasia;
            chknovoPessoaFisica.Checked = fornec.pessoa_fisica;
            if (chknovoPessoaFisica.Checked)
            {
                lblNovoCnpjCpf.Text = "CPF";
                lblNovoIeRg.Text = "RG";
            }
            else
            {
                lblNovoCnpjCpf.Text = "CNPJ";
                lblNovoIeRg.Text = "IE";
            }
            txtNovoCnpjCpf.Text = fornec.CNPJ;
            txtNovoIeRg.Text = fornec.IE;
            txtNovoCep.Text = fornec.CEP;
            txtNovoEndereco.Text = fornec.Endereco;
            txtNovoEnderecoNro.Text = fornec.Endereco_nro;
            txtNovoBairro.Text = fornec.Bairro;
            txtNovoCidade.Text = fornec.Cidade;
            txtNovoUf.Text = fornec.UF;
            txtNovoCodMunicipio.Text = fornec.codmun;
            txtNovoTelefone.Text = fornec.telefone1;
            Session.Remove("novoFornecedor" + urlSessao());
            Session.Add("novoFornecedor" + urlSessao(), fornec);
            modalNovoFornecedor.Show();
        }

        protected void ImgBtnConfirmaNovoFornecedor_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                fornecedorDAO fornec = (fornecedorDAO)Session["novoFornecedor" + urlSessao()];
                fornec.Fornecedor = txtNovoFornecedor.Text;
                fornec.Fornecedor = txtNovoFornecedor.Text ;
                fornec.Razao_social = txtNovoRazaoSocial.Text;
                fornec.Nome_Fantasia = txtNovoFantasia.Text;
                fornec.pessoa_fisica = chknovoPessoaFisica.Checked;
                fornec.CNPJ = txtNovoCnpjCpf.Text;
                fornec.IE = txtNovoIeRg.Text;
                fornec.CEP = txtNovoCep.Text;
                fornec.Endereco = txtNovoEndereco.Text ;
                fornec.Endereco_nro =txtNovoEnderecoNro.Text ;
                fornec.Bairro = txtNovoBairro.Text ;
                fornec.Cidade = txtNovoCidade.Text ;
                fornec.UF = txtNovoUf.Text ;
                fornec.codmun = txtNovoCodMunicipio.Text;
                fornec.telefone1 = txtNovoTelefone.Text ;
                fornec.salvar(true);
                modalNovoFornecedor.Hide();
                btnConfirmaImportar_Click(sender,e);
            }
            catch (Exception err)
            {
                showMessage("NOVO-FORNECEDOR:"+err.Message, true);

            }
        }

        protected void ImgBtnCancelaNovoFornecedor_Click(object sender, ImageClickEventArgs e)
        {
            modalNovoFornecedor.Hide();
            ModalImportar.Show();
        }

        protected void chknovoPessoaFisica_CheckedChanged(object sender, EventArgs e)
        {
            if (chknovoPessoaFisica.Checked)
            {
                lblNovoCnpjCpf.Text = "CPF";
                lblNovoIeRg.Text = "RG";
            }
            else
            {
                lblNovoCnpjCpf.Text = "CNPJ";
                lblNovoIeRg.Text = "IE";
            }
            modalNovoFornecedor.Show();
        }

        protected void imgBtnConfirmaNovoProduto_Click(object sender, ImageClickEventArgs e)
        {
            int index = Funcoes.intTry(lblIndexProdutosNovo.Text);
            List<nf_itemDAO> itensNovos = (List<nf_itemDAO>)Session["novosItens" + urlSessao()];
            nf_itemDAO item = itensNovos[index];

            if(item.PLU != txtNovoPlu.Text)
            {
                Funcoes.cancelaPluUsado(item.PLU);
            }
            item.PLU = txtNovoPlu.Text;
            if (isnumero(txtNovoEan.Text))
            {
                item.ean = txtNovoEan.Text;
            }
            else
            {
                item.ean = "";
            }
            item.CODIGO_REFERENCIA = txtNovoRef.Text;
            item.Descricao = txtNovoDescricao.Text;
            item.NCM = txtNovoNCM.Text;
            item.Codigo_tributacao_novo = txtNovoCodTributacao.Text;
            item.Descricao_tributacao_novo = txtNovoDescricaoTributacao.Text ;
            item.cstPisCofins_novo = txtNovoCSTPisCofins.Text ;
            item.Codigo_grupo_novo = txtNovoCodGrupo.Text;
            item.Descricao_grupo_novo = txtNovoDescGrupo.Text;
            item.Codigo_departamento_novo = txtNovoCodSubGrupo.Text ;
            item.Descricao_subGrupo_novo = txtNovoDescSubGrupo.Text ;
            item.Codigo_departamento_novo = txtNovoCodDepartamento.Text;
            item.Descricao_departamento_novo  = txtNovoDescDepartamento.Text ;

            modalCadastroProdutoDetalhes.Hide();
            carregarNovosItens();
        }

        protected void imgBtnCancelarNovoProduto_Click(object sender, ImageClickEventArgs e)
        {
            modalCadastroProdutoDetalhes.Hide();
            carregarNovosItens();
        }
        private void verificarProdutosNovos()
        {
            User usr = (User)Session["User"];
            nfDAO nf = (nfDAO)Session["objNfEntrada" + urlSessao()];
            List<nf_itemDAO> itensNovos = nf.NfItens.FindAll(p => p.PLU.Equals(""));
            foreach (nf_itemDAO item in itensNovos)
            {
                
                item.PLU = Funcoes.getNovoPlu(usr, "");

                SqlDataReader rsTrib = null;
                try
                {
                    rsTrib = Conexao.consulta(@"
                        Select top 1 t.Codigo_Tributacao, 
                            Descricao_Tributacao, 
                            m.CST_Saida,
                            m.Pis_Perc_Saida,
                            m.Cofins_Perc_Saida, 
                            m.codigo_departamento,
                            m.tipo,
	                        d.Descricao_departamento,
                            d.codigo_subGrupo,
	                        sg.Descricao_SubGrupo,
	                        g.codigo_grupo,
	                        g.Descricao_Grupo
                        from Tributacao as t
                            inner join mercadoria as m on m.Codigo_Tributacao = t.Codigo_Tributacao
                            inner join departamento as d on m.Codigo_departamento = d.Codigo_departamento
                            inner join SubGrupo as sg on d.Codigo_SubGrupo = sg.Codigo_SubGrupo
                            inner join grupo as g on sg.Codigo_Grupo = g.Codigo_Grupo
                        where m.cf = '"+item.NCM+"';", usr, false);
                    if (rsTrib.Read())
                    {
                        item.Codigo_tributacao_novo = rsTrib["Codigo_Tributacao"].ToString();
                        item.Descricao_tributacao_novo = rsTrib["Descricao_tributacao"].ToString();
                        item.cstPisCofins_novo = rsTrib["CST_Saida"].ToString();
                        item.pis_novo = rsTrib["Pis_Perc_Saida"].ToString();
                        item.cofins_novo = rsTrib["Cofins_Perc_Saida"].ToString();
                        item.Codigo_departamento_novo = rsTrib["codigo_departamento"].ToString();
                        item.Descricao_departamento_novo = rsTrib["descricao_departamento"].ToString();
                        item.Codigo_subGrupo_novo = rsTrib["codigo_subgrupo"].ToString();
                        item.Descricao_subGrupo_novo = rsTrib["descricao_subgrupo"].ToString();
                        item.Codigo_grupo_novo = rsTrib["codigo_grupo"].ToString();
                        item.Descricao_grupo_novo = rsTrib["descricao_grupo"].ToString();
                        item.tipo_novo = rsTrib["tipo"].ToString();

                    }
                    else
                    {
                        tributacaoDAO trib = new tributacaoDAO("1", usr);
                        item.Codigo_tributacao_novo = trib.Codigo_Tributacao.ToString();
                        item.Descricao_tributacao_novo = trib.Descricao_Tributacao;
                        item.cstPisCofins_novo = "00";
                        item.pis_novo = "0";
                        item.cofins_novo = "0";
                        item.Codigo_departamento_novo = "001001001";
                        item.Descricao_departamento_novo = Conexao.retornaUmValor("Select descricao_departamento from departamento where codigo_departamento ='001001001'", null);
                        item.Codigo_subGrupo_novo = "001001";
                        item.Descricao_subGrupo_novo = Conexao.retornaUmValor("Select descricao_SubGrupo from subGrupo where codigo_subGrupo ='001001001'", null);
                        item.Codigo_grupo_novo = "001";
                        item.Descricao_grupo_novo = Conexao.retornaUmValor("Select descricao_Grupo from grupo where codigo_grupo ='001001001'", null);
                        item.Tipo = "PRINCIPAL";
                    }
                    
                }
                catch (Exception err)
                {

                    showMessage("NOVO-PRODUTO:" + err.Message, true);
                }
                finally
                {
                    if (rsTrib != null)
                        rsTrib.Close();
                }

                
            }
            if (itensNovos.Count > 0)
            {
                Session.Remove("novosItens" + urlSessao());
                Session.Add("novosItens" + urlSessao(), itensNovos);
                carregarNovosItens();
            }
            
        }
        private void carregarNovosItens()
        {
            List<nf_itemDAO> itensNovos = (List<nf_itemDAO>) Session["novosItens" + urlSessao()];
            gridNovosProdutosCadastrar.DataSource = itensNovos;
            gridNovosProdutosCadastrar.DataBind();
            modalCadastroNovosProdutos.Show();

        }

        protected void gridNovosProdutosCadastrar_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            EnabledControls(PnCadastroProdutoDetalhes, true);
            int index = Convert.ToInt32(e.CommandArgument);
            lblIndexProdutosNovo.Text = index.ToString();
            List<nf_itemDAO> itensNovos = (List<nf_itemDAO>)Session["novosItens" + urlSessao()];
            nf_itemDAO item = itensNovos[index];
            txtNovoPlu.Text = item.PLU;
            txtNovoEan.Text = item.ean;
            txtNovoRef.Text = item.CODIGO_REFERENCIA;
            txtNovoDescricao.Text = item.Descricao;
            txtNovoNCM.Text = item.NCM;
            txtNovoCodTributacao.Text = item.Codigo_tributacao_novo;
            txtNovoDescricaoTributacao.Text = item.Descricao_tributacao_novo;
            txtNovoCSTPisCofins.Text = item.cstPisCofins_novo;
            txtNovoCodGrupo.Text = item.Codigo_grupo_novo;
            txtNovoDescGrupo.Text = item.Descricao_grupo_novo;
            txtNovoCodSubGrupo.Text = item.Codigo_departamento_novo;
            txtNovoDescSubGrupo.Text = item.Descricao_subGrupo_novo;
            txtNovoCodDepartamento.Text = item.Codigo_departamento_novo;
            txtNovoDescDepartamento.Text = item.Descricao_departamento_novo;
            modalCadastroProdutoDetalhes.Show();

        }

        protected void imgBtnConfirmaNovosProdutos_Click(object sender, ImageClickEventArgs e)
        {
            User usr = (User)Session["User"];
            List<nf_itemDAO> itensNovos = (List<nf_itemDAO>)Session["novosItens" + urlSessao()];
            try
            {
                foreach (var item in itensNovos)
                {
                    if (!Funcoes.existePLU(item.PLU))
                    {
                        MercadoriaDAO merc = new MercadoriaDAO(usr);
                        merc.PLU = item.PLU;
                        if (!item.ean.ToString().Equals(""))
                        {
                            merc.addEan(item.ean);
                        }
                        merc.Ref_fornecedor = item.CODIGO_REFERENCIA;
                        merc.Descricao = item.Descricao;
                        merc.cf = item.NCM;
                        merc.Codigo_Tributacao = Funcoes.decTry(item.Codigo_tributacao_novo);
                        merc.cst_saida = item.cstPisCofins_novo;
                        merc.pis_perc_saida = Funcoes.decTry(item.pis_novo);
                        merc.cofins_perc_saida = Funcoes.decTry(item.cofins_novo);
                        merc.Codigo_departamento = item.Codigo_departamento_novo;
                        merc.Inativo = 3;
                        merc.Tipo = item.tipo_novo;
                        merc.salvar(true);
             
                    }
                }
            }
            catch (Exception err)
            {
                showMessage("NOVO-PRODUTO:" + err.Message, true);
            }
            modalCadastroNovosProdutos.Hide();
        }

        protected void imgBtnCancelarNovosProdutos_Click(object sender, ImageClickEventArgs e)
        {
            List<nf_itemDAO> itensNovos = (List<nf_itemDAO>)Session["novosItens" + urlSessao()];
            foreach (nf_itemDAO item in itensNovos)
            {
                Funcoes.cancelaPluUsado(item.PLU);
            }
            btnCancelar_Click(sender, e);
        }

        protected void ImgBntCancelaChaveSefaz_Click(object sender, ImageClickEventArgs e)
        {
            modalConsultarSefaz.Hide();
            ModalImportar.Show();
        }

        protected void ImgBtnConsultaChaveSefaz_Click(object sender, ImageClickEventArgs e)
        {
            User usr = (User)Session["User"];
            xmlNFE nfeXml = new xmlNFE(usr);
            nfeXml.consultaNotaDFE(lblChaveConsultarSefaz.Text);


        }

        protected void buscarRespostaXML()
        {
            xmlNFE nfeXml = (xmlNFE)Session["xml" + urlSessao()];
        }

        protected void txtCodigo_operacao_TextChanged(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            String itemLista = (String)Session["campoLista" + urlSessao()];
            nfDAO nf = (nfDAO)Session["objNfEntrada" + urlSessao()];

            if (Funcoes.decTry(txtCodigo_operacao.Text) > 0)
            {
                natureza_operacaoDAO natOp = new natureza_operacaoDAO(txtCodigo_operacao.Text, usr);
                if (natOp.Descricao.Equals(""))
                {
                    return;
                }
                else
                {
                    natOp = null;
                }
            }

            Decimal codOpe = 0;
            Decimal.TryParse(txtCodigo_operacao.Text , out codOpe);
            nf.Codigo_operacao = codOpe;
            nf.atribuirCodOperacaoEntrada();
            //nf.calculaTotalItens();
            carregarDados();
        }
        protected void msgShow(String mensagem)
        {
            lblMensagemAviso.Text = mensagem;
            lblErroPanel.ForeColor = System.Drawing.Color.Red;
            btnOkMensagemAviso.Focus();
            modalMensagemAviso.Show();
        }

        protected void btnOkMensagemAviso_Click(object sender, EventArgs e)
        {
            modalError.Hide();
        }
    }
}