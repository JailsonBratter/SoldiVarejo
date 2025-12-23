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
using visualSysWeb.modulos.NotaFiscal.code;
using visualSysWeb.code;
using System.IO;
using System.Collections;
using System.Text;
using System.Dynamic;
using visualSysWeb.modulos.NotaFiscal.NFeRT;

namespace visualSysWeb.modulos.NotaFiscal.pages
{
    public partial class NfSaidaDetalhes : visualSysWeb.code.PagePadrao
    {
        static bool mostraBotoesTeste = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            mostraBotoesTeste = Funcoes.valorParametro("EXIBE_BOTOES_TESTE", null).ToUpper().Equals("TRUE");
            User usr = (User)Session["User"];
            nfDAO obj = null;
            try
            {

                obj = (nfDAO)Session["obj" + urlSessao()];
            }
            catch (Exception)
            {

                Session.Remove("obj" + urlSessao());
                obj = null;
            }


            if (Request.Params["novo"] != null)
            {
                try
                {


                    if (!IsPostBack)
                    {
                        status = "incluir";
                        habilitarCampos(true);
                        obj = new nfDAO(usr, "1");

                        if (usr != null)
                        {
                            txtusuario.Text = usr.getNome();
                            obj.usuario = usr.getNome();
                        }

                        if (Request.Params["pedidoImporta"] != null)
                        {
                            String nPedido = Request.Params["pedidoImporta"].ToString();
                            obj.importarPedido(nPedido, 1);

                        }

                        if (Request.Params["DevolucaoImporta"] != null)
                        {
                            obj.OrigemDevolucao = true;
                            String nDevolucaoNFe = Request.Params["DevolucaoImporta"].ToString();
                            obj.devolucaoNFeCodigo = int.Parse(nDevolucaoNFe);
                            obj.importarDevolucaoNFe(nDevolucaoNFe);
                        }

                        bool comNatureza = false;
                        if (obj.Codigo_operacao > 0)
                        {
                            comNatureza = true;
                        }
                        obj.centro_custo = Funcoes.valorParametro("CENTRO_CUSTO_NF_SAIDA", usr);

                        Session.Remove("obj" + urlSessao());
                        Session.Add("obj" + urlSessao(), obj);

                        if (!comNatureza)
                        {
                            Session.Remove("campoLista" + urlSessao());
                            Session.Add("campoLista" + urlSessao(), "txtCodigo_operacao");
                            exibeLista();
                        }
                        else
                        {
                            carregarDados();
                        }


                    }
                    carregarGrids();
                }
                catch (Exception err)
                {
                    showMessage(err.Message, true);


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

                            String codigo = Request.Params["codigo"].ToString();
                            String cliente = Request.Params["cliente"].ToString();
                            if (Request.Params["serie"] != null)
                            {
                                string serie = Request.Params["serie"].ToString();
                                int serieNFe = 0;
                                int.TryParse(serie, out serieNFe);
                                obj = new nfDAO(codigo, "1", cliente, serieNFe, usr);
                            }
                            else
                            {
                                obj = new nfDAO(codigo, "1", cliente, usr);

                            }
                            if (obj.status.Equals("TRANSMITIDO") || obj.status.Equals("AUTORIZADO") || obj.status.Equals("CANCELADA"))
                            {
                                status = "pesquisar";

                            }
                            else
                            {
                                status = "visualizar";
                            }

                            Session.Remove("obj" + urlSessao());
                            Session.Add("obj" + urlSessao(), obj);
                            carregarDados();

                        }
                        if (!status.Equals("incluir") && !status.Equals("editar"))
                        {
                            habilitarCampos(false);
                        }
                        else
                        {
                            habilitarCampos(true);
                        }
                    }
                    catch (Exception err)
                    {
                        lblError.Text = err.Message;
                    }
                }
            }
            carregabtn(pnBtn);
            camposnumericos();
            formataCampos();


        }

        private void formataCampos()
        {
            TxtPesquisaLista.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            txtCodigo.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            txtCliente_CNPJ.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
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
            txtDataCupom.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
        }



        private void limparCampos()
        {
            LimparCampos(cabecalho);
            LimparCampos(conteudo);
            LimparCampos(rodape);
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
                                "txtValorPg"

                                     };
            foreach (String item in campos)
            {
                TextBox txt = (TextBox)cabecalho.FindControl(item);
                if (txt == null)
                    txt = (TextBox)conteudo.FindControl(item);

                if (txt != null)
                {
                    txt.Attributes.Add("OnKeyPress", "javascript:return numeros(this,event);");

                }
            }
        }

        private void habilitarCampos(bool enable)
        {
            EnabledControls(conteudo, enable);
            EnabledControls(cabecalho, enable);
            EnabledControls(rodape, enable);
            EnabledControls(pnItens, enable);
            EnabledControls(gridPagamentos, enable);
            addItens.Visible = enable;
            AddPg.Visible = enable;
            btnXml.Visible = !enable;
            ImgDtEmissao.Visible = enable;
            ImgDeCalendario.Visible = enable;
            if (enable && txtCodigo_operacao.Text.Trim().Equals(""))
                btnimg_txtCodigo_operacao.Visible = true;
            else
                btnimg_txtCodigo_operacao.Visible = false;

            if (!txtCliente_Fornecedor.Text.Equals(""))
            {
                btnimg_txtCliente_CNPJ.Visible = false;
                txtCliente_CNPJ.Enabled = false;
            }
            else
            {
                btnimg_txtCliente_CNPJ.Visible = true;
                txtCliente_CNPJ.Enabled = true;

            }
            if (!txtStatus.Text.Equals("AUTORIZADO") || !status.Equals("visualizar"))
            {
                btnFaturaPedido.Visible = mostraBotoesTeste;
            }

        }

        protected void btnFaturaPedido_Click(object sender, EventArgs e)
        {
            try
            {
                nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
                nf.status = "AUTORIZADO";
                nf.Emissao = DateTime.Now;
                nf.AtualizarStatus();
                carregarDados();
                habilitarCampos(false);
                showMessage("Pedido Faturado com Sucesso", false);


            }
            catch (Exception err)
            {
                showMessage(err.Message, true);
            }
        }

        protected bool validaCamposObrigatorios()
        {
            try
            {
                User usr = (User)Session["user"];
                natureza_operacaoDAO op = new natureza_operacaoDAO(txtCodigo_operacao.Text, usr);

                if (ddltPag.SelectedValue.Equals(""))
                {
                    ddltPag.BackColor = System.Drawing.Color.Red;
                    TabContainer1.ActiveTabIndex = 1;
                    throw new Exception("O Campo Forma de Pagamento é obrigatorio");
                }

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
            String[] campos = {
                                    "txtCliente_CNPJ",
                                    "txtEmissao",
                                    "txtData",
                                    "txtcentro_custo",
                                    "txtCodigo_operacao",
                                    "txtTipoFrete",
                                    "txtTransportadora",
                                    "ddltPag",
                                     };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array
            String[] campos = { "txtusuario",
                                  "txtCliente_Fornecedor",
                                   "TxtNomeCliente",
                                   "txtPedido",
                                   "txtid",
                                   "txtIPI_Nota",
                                   "txtBase_Calculo",
                                   "txtICMS_Nota",
                                   "txtICMS_ST",
                                   //"txtDesconto",
                                   "txtTotal",
                                   "txtBase_Calc_Subst",
                                   "txtNum_item",
                                   "txtStatus",
                                   "txtiva",
                                   "TxtTotalItem",
                                   "txtCodigo",
                                   "txtTotalProdutos",
                                   "txtaliquota_icms",
                                   "txtredutor_base",
                                   "txtPisItem",
                                   "txtCofinsItem",
                                   "txtCodigo_operacao",
                                   "txtindice_st",
                                   "txtAliquota_iva",
                                   "txtUfCliente",
                                   "txtCompItemNf",
                                   "txtVlrPisItem",
                                   "txtVlrCofinsItem",
                                   "txtTotal_vFCP",
                                   "txtTotal_vFCPST",
                                   "txtcentro_custo",
                                     };
            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("NfSaidaDetalhes.aspx?novo=true");

        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            nfDAO obj = (nfDAO)Session["obj" + urlSessao()];
            if (obj.status.Equals("TRANSMITIDO") || obj.status.Equals("AUTORIZADO") || obj.status.Equals("CANCELADA") || obj.status.Equals("INUTILIZADO"))
            {
                lblError.Text = "Não é permitido alterações na Nota Fiscal";
                status = "pesquisar";
                carregarDados();
                carregabtn(pnBtn, true);
                habilitarCampos(false);
            }
            else if ((obj.status.Equals("VALIDADO") || obj.status.Equals("VALIDADA")) && !obj.Tipo_NF.Equals("3"))
            {
                if (obj.status == "VALIDADO" || obj.status == "VALIDADA")
                {
                    //if (!obj.StatusConsultadoAntesDaExclusao)
                    //{
                    //    lblError.Text = "Antes de executar esta operação o usuário deverá checar o STATUS da NFe no SEFAZ. Clicar na opção CONSULTA SITUAÇÃO através do BOTÃO XML.";
                    //    carregarDados();
                    //    carregabtn(pnBtn, true);
                    //    habilitarCampos(false);
                    //}
                    //else
                    //{
                        habilitarCampos(true);
                        status = "editar";
                        carregarDados();
                        User usr = (User)Session["User"];
                        txtusuario.Text = usr.getNome();
                        carregabtn(pnBtn, true);
                    //}
                }
            }
            else
            {
                habilitarCampos(true);
                status = "editar";
                carregarDados();
                User usr = (User)Session["User"];
                txtusuario.Text = usr.getNome();
                carregabtn(pnBtn, true);
            }

        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("NfSaida.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {

            modalPnConfirma.Show();
            //pnConfima.Visible = true;
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaCamposObrigatorios())
                {


                    nfDAO obj = (nfDAO)Session["obj" + urlSessao()];

                    obj.status = "DIGITACAO";
                    carregarDadosObj();
                    Decimal tPG = obj.TotalPag();
                    Decimal dif = tPG - obj.Total;
                    String tipoPg = ddltPag.SelectedValue;
                    if (!tipoPg.Equals("90") && obj.finNFe != 2 && (tPG == 0 || !dif.ToString("N2").Equals("0,00")))
                    {
                        modalConfirmaPgAdd.Show();
                    }
                    else
                    {
                        salvarNota();
                    }

                }
                else
                {
                    lblError.Text = "Campo Obrigatorio não preenchido";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception err)
            {
                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void salvarNota()
        {
            nfDAO obj = (nfDAO)Session["obj" + urlSessao()];
            User usr = (User)Session["User"];

            int pos = obj.Observacao.IndexOf("Valor do Imposto Aproximado");
            int tam = (obj.Observacao.IndexOf("IBPT") + 4) - pos;
            //Para atribuir a informação de valores dos impostos.
            if (pos >= 0)
            {
                String imp = obj.Observacao.Substring(pos, tam);
                obj.Observacao = obj.Observacao.Replace(imp, "");
            }

            if (obj.vTotTrib > 0)
            {
                obj.Observacao += " Valor do Imposto Aproximado: R$ " + obj.vTotTrib.ToString("N2") + " Conforme Lei.12.741/12 fonte IBPT";
            }
            //Para atribuir valores do DIFAL e FUNDO DE COMBATE A POBREZA
            pos = obj.Observacao.IndexOf("Valor DIFAL:");
            tam = (obj.Observacao.IndexOf("D.F.") + 4) - pos;
            if (pos >= 0)
            {
                string imp = obj.Observacao.Substring(pos, tam);
                obj.Observacao = obj.Observacao.Replace(imp, "");
            }
            if (obj.vFCP > 0 || obj.vValorDifal > 0)
            {
                obj.Observacao += "Valor DIFAL: " + obj.vValorDifal.ToString("n2") + " Valor FCP: " + obj.vFCP.ToString("N2") + " Total: " + (obj.vFCP + obj.vValorDifal).ToString("N2") + " D.F.";
            }
            //Se possui valor de difal e ou FCP
            if (obj.vFCP > 0 || obj.vValorDifal > 0)
            {
                var filialIEDestino = usr.filial.IEs.Find(x => x.UF == obj.UfCliente);
                if (filialIEDestino != null)
                {
                    obj.Observacao += "Nao foi recolhido guia GNRE pois contem Inscricao Estadual dentro do estado. IE numero: " + filialIEDestino.IE;
                }
            }



            obj.salvar(status.Equals("incluir"));
            status = "visualizar";



            lblTotalPagamentos.Text = obj.TotalPag().ToString("N2");
            lblError.Text = "Salvo com Sucesso";
            lblError.ForeColor = System.Drawing.Color.Blue;




            Session.Remove("obj" + urlSessao());
            Session.Add("obj" + urlSessao(), obj);
            carregabtn(pnBtn, true);
            carregarDados();
            habilitarCampos(false);
        }
        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("NfSaida.aspx");//colocar endereco pagina de pesquisa
        }
        protected void btnConfirmaPgAdd_Click(object sender, ImageClickEventArgs e)
        {
            try
            {


                salvarNota();
            }
            catch (Exception err)
            {
                modalConfirmaPgAdd.Hide();
                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;

            }
        }

        protected void btnCancelaPgAdd_Click(object sender, ImageClickEventArgs e)
        {
            modalConfirmaPgAdd.Hide();
            carregarGrids();
        }
        //--Atualizar DaoForm 
        private void carregarDados()
        {
            carregarDados(true);
        }
        private void carregarDados(bool calcular)
        {
            String nomeObj = "obj" + urlSessao();
            nfDAO obj = (nfDAO)Session[nomeObj];
            txtCodigo.Text = obj.Codigo.ToString() + "";
            txtOrdemCompra.Text = obj.Ordem_compra;
            txtCliente_Fornecedor.Text = (obj.Cliente_Fornecedor != null ? obj.Cliente_Fornecedor.ToString() : "");
            TxtNomeCliente.Text = obj.Nome_cliente.ToString() + "";
            txtUfCliente.Text = obj.UfCliente.ToString();
            txtData.Text = obj.DataBr();
            txtCodigo_operacao.Text = (obj.Codigo_operacao.ToString().Equals("0") ? "" : obj.Codigo_operacao.ToString());
            txtEmissao.Text = obj.EmissaoBr();
            ddlFinalidade.SelectedValue = obj.finNFe.ToString();
            if (calcular)
                obj.calculaTotalItens();

            txtTotal.Text = string.Format("{0:0,0.00}", obj.Total);
            //txtTotalProdutos.Text = obj.valorTotalProdutos.ToString("N2");
            txtTotalProdutos.Text = obj.TotalProdutos.ToString("N2");
            txtDesconto.Text = obj.Desconto.ToString("N2");
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
            txtStatus.Text = obj.status;
            //chknf_Canc.Checked = obj.nf_Canc;


            txtcentro_custo.Text = obj.centro_custo.ToString();

            txtICMS_ST.Text = string.Format("{0:0,0.00}", obj.ICMS_ST);

            txtCliente_CNPJ.Text = obj.Fornecedor_CNPJ.ToString();

            txtDesconto_geral.Text = string.Format("{0:0,0.00}", obj.Desconto_geral);
            txtusuario.Text = obj.usuario.ToString();
            txtEntrega.Text = obj.Endereco_Entrega;

            lblTotalPagamentos.Text = obj.TotalPag().ToString("N2");

            txtTransportadora.Text = obj.nome_transportadora;
            txtQuantidade.Text = obj.qtde.ToString("N2");
            txtEspecie.Text = obj.especie;
            txtMarca.Text = obj.marca;
            txtNumero.Text = obj.numero.ToString();
            txtPesoBrutoTransporte.Text = obj.peso_bruto.ToString();
            txtPesoLiquidoTransporte.Text = obj.peso_liquido.ToString();
            ddlTipoFrete.SelectedValue = obj.tipo_frete;


            txtPlaca.Text = obj.Placa;
            ddlindPres.SelectedValue = obj.indPres.ToString();
            ddlindFinal.SelectedValue = obj.indFinal.ToString();
            DDlRefECF.Text = (obj.Ref_ECF ? "ECF" : "NFE");

            ddlIntermediador.SelectedValue = obj.indIntermed.ToString();
            txtIntermedCnpj.Text = obj.intermedCnpj;
            txtIdCadIntTran.Text = obj.idCadIntTran;
            txtCnpjPagamento.Text = obj.CNPJPagamento;


            //nfe 4.0
            ddltPag.SelectedValue = obj.tPag;
            txtTotal_vFCP.Text = obj.vFCP.ToString("N2");
            txtTotal_vFCPST.Text = obj.vFCPST.ToString("N2");

            //txtReferenciaNota.Text = obj.nota_referencia.ToString();
            Session.Remove(nomeObj);
            Session.Add(nomeObj, obj);

            carregarGrids();
            btnXml.Visible = status.Equals("visualizar");
            //BotaoXml();
        }


        public void carregarGrids()
        {
            try
            {



                nfDAO obj = (nfDAO)Session["obj" + urlSessao()];
                gridItens.DataSource = obj.nfItens();
                gridItens.DataBind();

                gridPagamentos.DataSource = obj.nfPagamento();
                gridPagamentos.DataBind();


                if (DDlRefECF.Text.Equals("NFE"))
                {
                    obj.Ref_ECF = false;
                    DivECF.Visible = false;
                    DivReferenciaNota.Visible = true;
                    gridNfReferencia.DataSource = obj.NotasReferencias();
                    gridNfReferencia.DataBind();
                }
                else
                {
                    obj.Ref_ECF = true;
                    DivECF.Visible = true;
                    DivReferenciaNota.Visible = false;
                    gridNfReferencia.DataSource = obj.EcfReferencias();
                    gridNfReferencia.DataBind();

                }
                GridPedidos.DataSource = obj.PedidosImportados();
                GridPedidos.DataBind();

                if (!status.Equals("visualizar") && txtCodigo_operacao.Text.Trim().Equals(""))
                    btnimg_txtCodigo_operacao.Visible = true;
                else
                    btnimg_txtCodigo_operacao.Visible = false;


                if (!status.Equals("visualizar") && txtCliente_CNPJ.Text.Trim().Equals(""))
                {
                    txtCliente_CNPJ.Enabled = true;
                    btnimg_txtCliente_CNPJ.Visible = true;
                }
                else
                {
                    txtCliente_CNPJ.Enabled = false;
                    txtCliente_CNPJ.BackColor = txtCliente_Fornecedor.BackColor;
                    btnimg_txtCliente_CNPJ.Visible = false;
                }


                Session.Remove("obj" + urlSessao());
                Session.Add("obj" + urlSessao(), obj);
            }
            catch (Exception err)
            {

                lblError.Text = err.Message;
            }
        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            try
            {


                nfDAO obj = (nfDAO)Session["obj" + urlSessao()];
                obj.Codigo = txtCodigo.Text;
                obj.Ordem_compra = txtOrdemCompra.Text;
                obj.Cliente_Fornecedor = txtCliente_Fornecedor.Text;
                obj.Tipo_NF = "1";
                obj.Data = DateTime.Parse(txtData.Text);
                obj.Codigo_operacao = Funcoes.decTry((txtCodigo_operacao.Text.Equals("") ? "0" : txtCodigo_operacao.Text));
                obj.Emissao = DateTime.Parse(txtEmissao.Text);
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
                obj.nota_referencia = txtReferenciaNota.Text;

                //obj.nf_Canc = chknf_Canc.Checked;
                obj.centro_custo = txtcentro_custo.Text;
                obj.ICMS_ST = Funcoes.decTry((txtICMS_ST.Text.Equals("") ? "0" : txtICMS_ST.Text));
                obj.Fornecedor_CNPJ = txtCliente_CNPJ.Text;
                obj.Desconto_geral = Funcoes.decTry((txtDesconto_geral.Text.Equals("") ? "0" : txtDesconto_geral.Text));
                obj.usuario = txtusuario.Text;
                //============================================
                //transporte
                //============================================
                obj.nome_transportadora = txtTransportadora.Text;
                obj.qtde = (txtQuantidade.Text.Trim().Equals("") ? 1 : Funcoes.decTry(txtQuantidade.Text));
                obj.especie = (txtEspecie.Text.Trim().Equals("") ? "0" : txtEspecie.Text);
                obj.marca = (txtMarca.Text.Trim().Equals("") ? "0" : txtMarca.Text);
                obj.numero = (txtNumero.Text.Trim().Equals("") ? 0 : Funcoes.decTry(txtNumero.Text));
                obj.peso_bruto = (txtPesoBrutoTransporte.Text.Trim().Equals("") ? 0 : Funcoes.decTry(txtPesoBrutoTransporte.Text));
                obj.peso_liquido = (txtPesoLiquidoTransporte.Text.Trim().Equals("") ? 0 : Funcoes.decTry(txtPesoLiquidoTransporte.Text));
                obj.tipo_frete = ddlTipoFrete.SelectedValue;
                obj.Placa = txtPlaca.Text;

                //=======informacoes Adicionais Cliente 
                obj.indFinal = Funcoes.intTry(ddlindFinal.SelectedValue);
                obj.indPres = Funcoes.intTry(ddlindPres.SelectedValue);
                obj.finNFe = Funcoes.intTry(ddlFinalidade.SelectedValue);
                obj.indIntermed = Funcoes.intTry(ddlIntermediador.SelectedValue);
                obj.intermedCnpj = txtIntermedCnpj.Text;
                obj.idCadIntTran = txtIdCadIntTran.Text;
                obj.CNPJPagamento = txtCnpjPagamento.Text;
                obj.tPag = ddltPag.SelectedItem.Value;

                int.TryParse(ddlFinalidade.SelectedValue, out obj.finNFe);

                Session.Remove("obj" + urlSessao());

                Session.Add("obj" + urlSessao(), obj);
            }
            catch (Exception err)
            {

                lblError.Text = err.Message;
            }
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
                nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
                if (nf != null && nf.NfItens.Count > 0)
                {
                    nf_itemDAO itemnf = nf.item(e.Row.RowIndex);
                    if (itemnf.inativo)
                    {
                        e.Row.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }

        }
        protected void ItemInativo(bool inativo)
        {
            if (inativo)
            {
                lblInativo.Text = "inativo";
                LblErroItem.Text = "ATENÇÃO:Produto esta Inativo!";
            }
            else
            {

                lblInativo.Text = "";
                LblErroItem.Text = "";
            }

        }

        protected void gridItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
            int index = Convert.ToInt32(e.CommandArgument);
            nf_itemDAO itemnf = nf.item(index);
            ViewState["gridLinha"] = index;
            carregabtn(pnBtn, true);
            carregarDadosObj();

            if (!ddlFinalidade.SelectedValue.Equals("2"))
            {
                LimparCampos(pnItens);

                carregarDadosItens(itemnf);
                PnExcluirItem.Visible = true;
                // TabContainer1.ScrollBars= 
            }
            else
            {
                EnabledControls(pnItemComplementar, true);
                LimparCampos(pnItemComplementar);
                carregarDadosItensComplementar(itemnf);
                pnCompExcluirItem.Visible = true;
            }

            Session.Remove("item" + urlSessao());
            Session.Add("item" + urlSessao(), itemnf);
        }

        protected void ImgAddNovoCupom_Click(object sender, ImageClickEventArgs e)
        {
            if (!txtCodigo_operacao.Text.Equals("") && !txtCliente_CNPJ.Text.Equals(""))
            {
                lblErrorPedido.Text = "";
                //lblImportacao.Text = "INFORMAR O NUMERO DO CUPOM";
                // txtNumeroPedido.Visible = false;
                // lblNumeroPedido.Visible = false;
                txtNumeroPedido.Text = "";
                txtCupom.Text = "";
                txtCaixa.Text = "";
                txtDataCupom.Text = DateTime.Now.ToString("dd/MM/yyyy");
                carregarDadosObj();
                ModalImportar.Show();

            }
            else
            {
                showMessage("Inclua as Informações de Cliente e Natureza de Operação", true);
            }


        }

        protected void ImgBtnAddItens_Click(object sender, ImageClickEventArgs e)
        {
            if (!txtCodigo_operacao.Text.Equals("") && !txtCliente_Fornecedor.Text.Equals(""))
            {
                if (ddlFinalidade.SelectedValue != "2")
                {
                    lblError.Text = "";
                    String or = "txtPLU";

                    TxtPesquisaLista.Text = "";
                    carregarDadosObj();
                    Session.Remove("campoLista" + urlSessao());
                    Session.Add("campoLista" + urlSessao(), or);
                    LimparCampos(pnItens);
                    exibeLista();
                    PnExcluirItem.Visible = false;
                }
                else
                {

                    carregarDadosObj();
                    LimparCampos(pnItemComplementar);
                    EnabledControls(pnItemComplementar, true);
                    carregarGrids();
                    modalComplementar.Show();
                }
            }
            else
            {
                showMessage("Inclua as Informações de Cliente e Natureza de Operação", true);
            }



        }

        protected nf_itemDAO carregaItem()
        {
            User usr = (User)Session["user"];
            nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
            nf_itemDAO itemnf = (nf_itemDAO)Session["item" + urlSessao()];
            if (itemnf == null)
                itemnf = new nf_itemDAO(usr);

            itemnf.vtotal = 0; //Zerado para não calcular no momento errado.
            itemnf.vtotal_produto = 0;
            itemnf.vBASEPisCofins = 0;

            itemnf.Tipo_NF = nf.Tipo_NF;
            itemnf.Codigo = txtCodigo.Text;
            itemnf.Cliente_Fornecedor = txtCliente_Fornecedor.Text;
            itemnf.PLU = txtPLU.Text;
            itemnf.CODIGO_REFERENCIA = txtCODIGO_REFERENCIA.Text;
            itemnf.Descricao = txtDescricao.Text;
            itemnf.naturezaOperacao = nf.NtOperacao;
            itemnf.codigo_operacao = Funcoes.decTry((txtCodigo_operacao_item.Text.Equals("") ? "0,00" : txtCodigo_operacao_item.Text));

            itemnf.Unitario = Funcoes.decTry(txtUnitario.Text);

            itemnf.Embalagem = Funcoes.decTry(txtEmbalagem.Text);

            itemnf.indFinal = Funcoes.intTry(ddlindFinal.SelectedItem.Value);


            // itemnf.Total = (TxtTotalItem.Text.Equals("")?0: Funcoes.decTry(TxtTotalItem.Text));
            itemnf.Qtde = Funcoes.decTry(txtQtde.Text);



            itemnf.Codigo_Tributacao = Funcoes.decTry(txtCodigo_Tributacao.Text);
            itemnf.Desconto = Funcoes.decTry(txtDescontoItem.Text);
            itemnf.despesas = Funcoes.decTry((txtdespesas.Text.Equals("") ? "0,00" : txtdespesas.Text));
            itemnf.aliquota_icms = Funcoes.decTry((txtaliquota_icms.Text.Equals("") ? "0" : txtaliquota_icms.Text));
            itemnf.IPI = Funcoes.decTry((txtIPI.Text.Equals("") ? "0,00" : txtIPI.Text));

            itemnf.IPIV = Funcoes.decTry((txtIPIV.Text.Equals("") ? "0,00" : txtIPIV.Text));
            itemnf.vmargemIva = Funcoes.decTry((txtmargem_iva.Text.Equals("") ? "0,00" : txtmargem_iva.Text));


            itemnf.vAliquota_iva = Funcoes.decTry((txtAliquota_iva.Text.Equals("") ? "0,00" : txtAliquota_iva.Text));
            itemnf.indice_St = txtindice_st.Text;
            itemnf.vIva = Funcoes.decTry((txtiva.Text.Equals("") ? "0,00" : txtiva.Text));
            itemnf.redutor_base = Funcoes.decTry((txtredutor_base.Text).Equals("") ? "0,00" : txtredutor_base.Text);
            //itemnf.PISV = Funcoes.decTry((txtPisItem.Text.Equals("")?"0,00":txtPisItem.Text));
            //itemnf.COFINSV = Funcoes.decTry((txtCofinsItem.Text.Equals("")?"0,00":txtCofinsItem.Text));
            itemnf.Num_item = int.Parse((txtNum_item.Text.Equals("") ? "0" : txtNum_item.Text));



            itemnf.NCM = (txtNCM.Text.Equals("") ? "0" : txtNCM.Text);
            itemnf.CEST = txtCEST.Text;
            itemnf.Und = txtUnd.Text;
            itemnf.Peso_liquido = Funcoes.decTry((txtPeso_liquido.Text.Equals("") ? "0,00" : txtPeso_liquido.Text));
            itemnf.Peso_Bruto = Funcoes.decTry((txtPeso_Bruto.Text.Equals("") ? "0,00" : txtPeso_Bruto.Text));

            itemnf.CSTPIS = txtCSTPIS.Text;
            itemnf.CSTCOFINS = txtCSTPIS.Text;
            Decimal.TryParse(txtPisItem.Text, out itemnf.PISp);
            Decimal.TryParse(txtCofinsItem.Text, out itemnf.COFINSp);


            itemnf.calculaPisCofins();

            itemnf.inativo = lblInativo.Text.Equals("inativo");


            itemnf.pCredSN = Funcoes.decTry((txtpCredSN.Text.Equals("") ? "0,00" : txtpCredSN.Text));
            itemnf.vCredicmssn = Funcoes.decTry((txtvCredicmssn.Text.Equals("") ? "0,00" : txtvCredicmssn.Text));

            itemnf.vOrigem = int.Parse(ddlOrigem.SelectedValue);


            //NFE 4.0
            itemnf.indEscala = ddlEscalaRelevante.SelectedItem.Value.Equals("S");
            itemnf.cnpj_Fabricante = txtCNPJFab.Text;
            itemnf.cBenef = txt_cBenef.Text;

            Decimal.TryParse(txtItem_BaseFCP.Text, out itemnf.vBCFCP);
            Decimal.TryParse(txtItem_pFCP.Text, out itemnf.pFCP);
            Decimal.TryParse(txtItem_VlrFCP.Text, out itemnf.vFCP);

            Decimal.TryParse(txtItem_BaseFCPST.Text, out itemnf.vBCFCPST);
            Decimal.TryParse(txtItem_pFCPST.Text, out itemnf.pFCPST);
            Decimal.TryParse(txtItem_VlrFCPST.Text, out itemnf.vFCPST);

            Decimal.TryParse(txtItem_pDevolv.Text, out itemnf.pDevol);
            Decimal.TryParse(txtItem_vIPIDevol.Text, out itemnf.vIPIDevol);

            itemnf.Frete = Funcoes.decTry(txtFreteItem.Text);

            itemnf.codigo_produto_ANVISA = txtCodigoProdutoAnvisa.Text;
            itemnf.motivo_isencao_ANVISA = txtMotivoIsencaoAnvisa.Text;
            Decimal.TryParse(txtPrecoMaximoAnvisa.Text, out itemnf.preco_Maximo_ANVISA);
            itemnf.codigoEmissaoNFe = txtCodigoEmisaoNFe.Text;

            itemnf.pedidoItemNumero = txtPedidoItemNumero.Text;
            itemnf.pedidoItemSequencia = txtPedidoItemSequencia.Text;

            Decimal.TryParse(txtICMSDestino.Text, out itemnf.aliquota_ICMS_Destino);

            Session.Remove("item" + urlSessao());
            Session.Add("item" + urlSessao(), itemnf);
            return itemnf;

        }
        protected void btnConfirmaItens_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                String strPlu = Conexao.retornaUmValor("Select plu from mercadoria where plu ='" + txtPLU.Text + "'", null);
                if (strPlu.Equals(""))
                {
                    txtPLU.BackColor = System.Drawing.Color.Red;
                    throw new Exception("PLU Não Cadastrado");
                }


                LblErroItem.Text = "";
                nfDAO nf = (nfDAO)Session["obj" + urlSessao()];



                nf_itemDAO itemnf = carregaItem();
                itemnf.Tipo_NF = nf.Tipo_NF;



                if (itemnf.Unitario <= 0)
                    throw new Exception("O Valor Unitario não pode Ser 0");

                if (itemnf.Embalagem <= 0)
                    throw new Exception("A Embalagem não pode Ser 0");

                if (itemnf.Qtde <= 0)
                    throw new Exception("A Qtde não pode Ser 0");


                if (nf.Tipo_NF.Equals("2"))
                {

                    bool existe = (int.Parse(Conexao.retornaUmValor("select COUNT(*) from CFOP where CFOP ='" + itemnf.codigo_operacao.ToString().Replace(".", "").Replace(",", ".") + "' and TIPO=2", null)) >= 1);
                    if (!existe)
                        throw new Exception("CFOP de Entrada não existe");


                }
                else
                {
                    bool existe = (int.Parse(Conexao.retornaUmValor("select COUNT(*) from CFOP where CFOP ='" + itemnf.codigo_operacao.ToString().Replace(".", "").Replace(",", ".") + "' and TIPO=1", null)) >= 1);
                    if (!existe)
                        throw new Exception("CFOP de Saida não existe");
                }

                if (nf.Tipo_NF.Equals("2"))
                {
                    bool existe = (int.Parse(Conexao.retornaUmValor("Select COUNT(*) from PIS_CST_entrada where PIS_CST_entrada='" + itemnf.CSTPIS + "'", null)) >= 1);
                    if (!existe)
                        throw new Exception("CST de PIS de Entrada não existe");

                    existe = (int.Parse(Conexao.retornaUmValor("Select COUNT(*) from PIS_CST_entrada where PIS_CST_entrada='" + itemnf.CSTCOFINS + "'", null)) >= 1);
                    if (!existe)
                        throw new Exception("CST de COFINS de Entrada não existe");

                }
                else
                {
                    bool existe = (int.Parse(Conexao.retornaUmValor("Select COUNT(*) from PIS_CST_saida where PIS_CST_saida='" + itemnf.CSTPIS + "'", null)) >= 1);
                    if (!existe)
                        throw new Exception("CST de PIS de Saida não existe");


                    existe = (int.Parse(Conexao.retornaUmValor("Select COUNT(*) from PIS_CST_SAIDA where PIS_CST_SAIDA='" + itemnf.CSTCOFINS + "'", null)) >= 1);
                    if (!existe)
                        throw new Exception("CST de COFINS de Saida não existe");


                }

                if (nf.NtOperacao.Saida && nf.NtOperacao.Preco_Venda && !nf.objCliente.Codigo_tabela.Equals("") && !nf.objCliente.Codigo_tabela.Equals("0"))
                {
                    decimal valorOriginalVenda = 0;
                    Decimal.TryParse(txtUnitarioOriginal.Text, out valorOriginalVenda);
                    if (itemnf.Unitario < valorOriginalVenda)
                    {
                       // throw new Exception("Preço não pode ser inferior ao preço de venda praticado na loja. Preço loja: R$ " + txtUnitarioOriginal.Text);
                       // Voltar aqui.
                    }
                }

                int numItem = int.Parse(txtNum_item.Text);
                itemnf.CalculaImpostos();
                if (numItem > nf.qtdItens())
                {
                    nf.addItem(itemnf);
                }
                else
                {
                    nf.atualizaItem(itemnf);
                }
                nf.calculaTotalItens();

                //nf.confirmaItens();

                Session.Remove("obj" + urlSessao());
                Session.Add("obj" + urlSessao(), nf);

                Session.Remove("item" + urlSessao());
                carregarDados();
                habilitarCampos(true);

                ModalItens.Hide();

            }
            catch (Exception erro)
            {

                LblErroItem.Text = erro.Message;
                LblErroItem.ForeColor = System.Drawing.Color.Red;
                ModalItens.Show();
            }

        }


        protected void exibeLista()
        {
            lblErroPesquisa.Text = "";

            bool limitar = true;
            nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
            modalLista.Show();
            String campo = (String)Session["campoLista" + urlSessao()];
            String sqlLista = "";
            User usr = (User)Session["User"];

            ddlTipoDestinatario.Visible = false;
            switch (campo)
            {
                case "txtPLU":
                case "txtCompPlu":
                    lbltituloLista.Text = "Escolha um Produto";
                    if (!nf.objCliente.Codigo_tabela.Equals("") && !nf.objCliente.Codigo_tabela.Equals("0") && nf.NtOperacao.Preco_Venda)
                    {
                        sqlLista = "SELECT mercadoria.PLU,mercadoria.DESCRICAO,isnull(ean.ean,'') EAN,Referencia =ISNULL(mercadoria.ref_fornecedor, ''),"
                            + " (isnull(preco_mercadoria.preco_promocao,"
                              + "CASE WHEN(convert(date, getdate()) between  Mercadoria_loja.Data_Inicio "
                              + " AND Mercadoria_loja.Data_Fim) AND ISNULL(Mercadoria_Loja.Preco_Promocao, 0) > 0 THEN Mercadoria_loja.Preco_Promocao ELSE Mercadoria_Loja.Preco END "
                              + "))as Preco"
                              + " ,cf as NCM ,isnull(inativo,0) as Inativo from mercadoria  INNER JOIN Mercadoria_Loja ON Mercadoria.plu = mercadoria_LOJA.plu "
                              + " LEFT JOIN Preco_Mercadoria ON Preco_Mercadoria.PLU = Mercadoria.plu AND Preco_Mercadoria.Filial = Mercadoria_Loja.Filial AND Preco_Mercadoria.Codigo_Tabela = '" + nf.objCliente.Codigo_tabela + "'"
                              + "  LEFT JOIN EAN ON EAN.plu=mercadoria.plu where Mercadoria_loja.Filial = '" + usr.getFilial() + "'"
                              + " AND ("
                              + " (mercadoria.plu =  '" + TxtPesquisaLista.Text + "') or (mercadoria.ref_fornecedor like '%" + TxtPesquisaLista.Text + "%')  "
                              + " OR (mercadoria.descricao like '%" + TxtPesquisaLista.Text + "%')"
                              + " OR (EAN.ean like '%" + TxtPesquisaLista.Text + "%') "
                              + ")";
                    }
                    else
                    {
                        sqlLista = "select  mercadoria.PLU,mercadoria.DESCRICAO,isnull(ean.ean,'') EAN,Referencia =mercadoria.ref_fornecedor, mercadoria_loja.preco, mercadoria.cf as NCM ,isnull(mercadoria.inativo,0) as Inativo from "
                            + " mercadoria inner join mercadoria_loja on mercadoria.plu = mercadoria_loja.plu and mercadoria_loja.filial = '" + usr.getFilial() + "' left join ean on ean.plu=mercadoria.plu WHERE "
                            + "((mercadoria.plu =  '" + TxtPesquisaLista.Text + "') or(mercadoria.ref_fornecedor like '%" + TxtPesquisaLista.Text + "%')  "
                            + " OR (mercadoria.descricao like '%" + TxtPesquisaLista.Text + "%')"
                            + " OR  (ean.ean like '%" + TxtPesquisaLista.Text + "%') "
                            + ")";
                    }
                    break;
                case "txtCliente_CNPJ":
                case "txtFiltroCupomVarios":
                case "txtFiltroPedidoVarios":
                case "txtCodigoClienteImporta":
                    ddlTipoDestinatario.Visible = true;

                    if (ddlTipoDestinatario.Text.Equals("FORNECEDOR"))
                    {
                        lbltituloLista.Text = "Escolha um Fornecedor";
                        sqlLista = "select replace(replace(replace(cnpj,'.',''),'-',''),'/','') as [CNPJ/CPF],Fornecedor,Nome_fantasia as Fantasia from fornecedor where rtrim(ltrim(isnull(cnpj,''))) <> '' and replace(replace(replace(cnpj,'.',''),'-',''),'/','') like '%" + TxtPesquisaLista.Text + "%' or FORNECEDOR like '%" + TxtPesquisaLista.Text + "%' or nome_fantasia like '%" + TxtPesquisaLista.Text + "%'  group by CNPJ,fornecedor,nome_fantasia";
                    }
                    else
                    {
                        lbltituloLista.Text = "Escolha um Cliente";
                        sqlLista = "select CNPJ as [CNPJ/CPF],codigo_cliente Codigo,Nome_cliente Cliente,nome_fantasia as Fantasia  from cliente where rtrim(ltrim(isnull(cnpj,''))) <> '' and (CNPJ like '%" + TxtPesquisaLista.Text + "%' or codigo_cliente like '%" + TxtPesquisaLista.Text + "%' or nome_cliente like '%" + TxtPesquisaLista.Text + "%' or nome_fantasia like '%" + TxtPesquisaLista.Text + "%') and isnull(inativo,0)=0";
                    }
                    break;
                case "txtcentro_custo":
                    lbltituloLista.Text = "Escolha um Centro de custo";
                    sqlLista = "SELECT  codigo_centro_custo as Codigo , descricao_centro_custo as Descricao  from centro_custo where codigo_centro_custo like'%" + TxtPesquisaLista.Text + "%' and descricao_centro_custo like '%" + TxtPesquisaLista.Text + "%'";
                    break;
                case "txtCodigo_operacao":
                case "txtCompCfop":
                    lbltituloLista.Text = "Escolha um Codigo de operação";
                    if (status.Equals("incluir"))
                    {
                        sqlLista = "Select  Codigo_operacao AS Codigo,Descricao,CASE WHEN Gera_apagar_receber=1 THEN 'SIM'ELSE'NAO' END AS Financeiro,CASE WHEN Baixa_estoque=1 THEN 'SIM'ELSE'NAO' END AS Estoque, CASE WHEN gera_custo=1 THEN 'SIM'ELSE'NAO' END AS custo , CASE WHEN Imprime_NF=1 THEN 'SIM'ELSE'NAO' END AS ImprimeNF,CASE WHEN Saida=1 THEN 'SIM'ELSE'NAO' END AS SAIDA, CASE WHEN nf_devolucao=1 THEN 'SIM'ELSE'NAO' END AS DEVOLUÇÃO,CASE WHEN PRECO_VENDA=1 THEN 'VENDA'ELSE'CUSTO' END AS PRECO from natureza_operacao  where ISNULL(Inativa, 0) = 0 AND (codigo_operacao like '%" + TxtPesquisaLista.Text + "%' or descricao like '%" + TxtPesquisaLista.Text + "%') ";
                    }
                    else
                    {
                        sqlLista = "Select  Codigo_operacao AS Codigo,Descricao,CASE WHEN Gera_apagar_receber=1 THEN 'SIM'ELSE'NAO' END AS Financeiro,CASE WHEN Baixa_estoque=1 THEN 'SIM'ELSE'NAO' END AS Estoque, CASE WHEN gera_custo=1 THEN 'SIM'ELSE'NAO' END AS custo , CASE WHEN Imprime_NF=1 THEN 'SIM'ELSE'NAO' END AS ImprimeNF,CASE WHEN Saida=1 THEN 'SIM'ELSE'NAO' END AS SAIDA, CASE WHEN nf_devolucao=1 THEN 'SIM'ELSE'NAO' END AS DEVOLUÇÃO,CASE WHEN PRECO_VENDA=1 THEN 'VENDA'ELSE'CUSTO' END AS PRECO from natureza_operacao  where (codigo_operacao like '%" + TxtPesquisaLista.Text + "%' or descricao like '%" + TxtPesquisaLista.Text + "%') ";
                    }

                    if (txtCodigo_operacao.Text.Equals(""))
                    {
                        sqlLista += " and (saida=1 or Imprime_NF =1) ";
                    }
                    else
                    {
                        try
                        {
                            natureza_operacaoDAO OP;
                            if (status.Equals("incluir"))
                            {
                                OP = new natureza_operacaoDAO(txtCodigo_operacao.Text, null, true);
                            }
                            else
                            {
                                OP = new natureza_operacaoDAO(txtCodigo_operacao.Text, null);
                            }
                            sqlLista += " and (saida=" + (OP.Saida ? 1 : 0) + " and Imprime_NF =" + (OP.Imprime_NF ? 1 : 0) + ") "; ;
                        }
                        catch (Exception)
                        {
                            sqlLista += " and (saida=1 or Imprime_NF =1) ";
                        }
                    }

                    sqlLista += " and filial ='MATRIZ' order by Codigo_operacao ";

                    break;
                case "txtCodigo_operacao_item":

                    lbltituloLista.Text = "Escolha um Codigo de operação";
                    if (nf.NtOperacao != null && !nf.NtOperacao.Saida && nf.NtOperacao.Imprime_NF)
                    {
                        sqlLista = "Select * from cfop where cfop like '%" + TxtPesquisaLista.Text + "%' or Descricao like '%" + TxtPesquisaLista.Text + "%' and tipo =2";
                    }
                    else
                    {

                        sqlLista = "Select  * from cfop where (cfop like '%" + TxtPesquisaLista.Text + "%' or Descricao like '%" + TxtPesquisaLista.Text + "%') and tipo =1";
                    }
                    break;
                case "txtCodigo_Tributacao":
                    lbltituloLista.Text = "Escolha a Tributação";
                    sqlLista = "select  codigo_tributacao Codigo, descricao_tributacao Descricao from tributacao where codigo_tributacao like '%" + TxtPesquisaLista.Text + "%' or Descricao_tributacao like '%" + TxtPesquisaLista.Text + "%'";

                    break;
                case "txtTipoPg":
                    lbltituloLista.Text = "Escolha um Tipo de Pagamento";
                    sqlLista = "select  tipo_pagamento Pagamento, Prazo from tipo_pagamento where tipo_pagamento like '%" + TxtPesquisaLista.Text + "%'";
                    break;
                case "txtNCM":
                case "txtCompNCM":
                    lbltituloLista.Text = "Escolha um NCM";
                    sqlLista = "SELECT  cf,descricao from cf where cf like '%" + TxtPesquisaLista.Text + "%' or descricao like '%" + TxtPesquisaLista.Text + "%'";
                    break;
                case "txtTransportadora":
                    lbltituloLista.Text = "Escolha uma Transportadora";
                    sqlLista = "Select nome_transportadora from transportadora where nome_transportadora like'%" + TxtPesquisaLista.Text + "%'";
                    break;
                case "txtCSTCOFINS":
                case "txtCSTPIS":

                case "txtCompCstPis":
                case "txtCompCstCofins":
                    if (nf.NtOperacao != null && !nf.NtOperacao.Saida)
                    {
                        sqlLista = "Select CST=PIS_CST_entrada , Descricao from pis_cst_entrada where descricao like '%" + TxtPesquisaLista.Text + "%' or PIS_CST_entrada like '%" + TxtPesquisaLista.Text + "%' order by cst";
                    }
                    else
                    {

                        lbltituloLista.Text = "Escolha o CST";
                        sqlLista = "Select CST=PIS_CST_Saida , Descricao from pis_cst_saida where descricao like '%" + TxtPesquisaLista.Text + "%' or PIS_CST_Saida like '%" + TxtPesquisaLista.Text + "%' order by cst";

                    }
                    break;
                case "txtReferenciaNota":
                    lbltituloLista.Text = "Escolha a nota de Entrada ";
                    sqlLista = "select Codigo,Emissao= CONVERT(varchar,Emissao,103),Data= CONVERT(varchar,Data,103) ,Total,id  from nf where Tipo_NF=2 and Cliente_Fornecedor ='" + txtCliente_Fornecedor.Text.Trim() + "' and  codigo like'%" + TxtPesquisaLista.Text + "%' ORDER BY CONVERT(varchar,Data,102) DESC";
                    limitar = false;
                    break;
                case "txtCEST":
                    lbltituloLista.Text = "Escolha a CEST";
                    sqlLista = "EXEC SP_CEST_NCM '" + txtNCM.Text.Trim() + "'";
                    break;


            }

            GridLista.DataSource = Conexao.GetTable(sqlLista, usr, limitar);
            GridLista.DataBind();

            if (lbltituloLista.Text.Equals("Escolha um Produto"))
            {
                foreach (GridViewRow row in GridLista.Rows)
                {
                    if (row.Cells[7].Text.Equals("1"))
                        row.ForeColor = System.Drawing.Color.Red;
                }
                //GridLista.Columns[6].Visible = false;
            }
            else
            {
                if (GridLista.Columns.Count >= 6)
                    GridLista.Columns[6].Visible = true;

            }



            if (GridLista.Rows.Count == 1)
            {
                if (!GridLista.Rows[0].Cells[1].Text.Equals("------"))
                {
                    RadioButton rdo = (RadioButton)GridLista.Rows[0].FindControl("RdoListaItem");
                    rdo.Checked = true;
                }
            }
            else
            {
                if (campo.Equals("txtCodigo_operacao"))
                {
                    String nat = Funcoes.valorParametro("NF_EMISSAO_NATUREZA", usr);
                    if (!nat.Equals(""))
                    {
                        foreach (GridViewRow row in GridLista.Rows)
                        {
                            if (row.Cells[1].Text.Equals(nat))
                            {
                                RadioButton rdo = (RadioButton)row.FindControl("RdoListaItem");
                                rdo.Checked = true;
                                break;
                            }
                        }
                    }
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

            modalLista.Show();
        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            carregarGrids();
            modalLista.Hide();

            String itemLista = (String)Session["campoLista" + urlSessao()];
            TextBox txt = (TextBox)pnItens.FindControl(itemLista);
            if (txt != null)
            {
                if (txt.Parent.ID.Equals("pnItensFrame"))
                {
                    ModalItens.Show();
                }
                if (txt.ID.Equals("txtCodigo_operacao"))
                {
                    Session.Remove("campoLista" + urlSessao());
                    Session.Add("campoLista" + urlSessao(), "txtCliente_CNPJ");
                    TxtPesquisaLista.Text = "";
                    exibeLista();

                }
                else if (txt.ID.Equals("txtCompPlu") ||
                              txt.ID.Equals("txtCompNCM") ||
                              txt.ID.Equals("txtCompCfop") ||
                              txt.ID.Equals("txtCompCstCofins") ||
                              txt.ID.Equals("txtCompCstPis")
                     )
                {
                    modalComplementar.Show();

                }
            }
        }

        protected void GridLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }



        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
            modalLista.Show();
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

        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {
            try
            {


                String selecionado = ListaSelecionada(1);

                if (!selecionado.Equals("") && !selecionado.Equals("------"))
                {

                    User usr = (User)Session["User"];
                    String itemLista = (String)Session["campoLista" + urlSessao()];
                    TextBox txt = (TextBox)cabecalho.FindControl(itemLista);
                    nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
                    if (itemLista.Equals("txtTransportadora"))
                        txt = txtTransportadora;

                    if (itemLista.Equals("txtCodigo_operacao"))
                    {
                        txt = txtCodigo_operacao;
                        txt.Text = ListaSelecionada(1);

                        nf.Codigo_operacao = Funcoes.decTry((txt.Text.Equals("") ? "0,00" : txt.Text));
                        //if (nf.qtdItens() > 0)
                        //    nf.atribuirCodOperacaoItem();
                        if (nf.NtOperacao != null && ((!nf.NtOperacao.Saida && nf.NtOperacao.Imprime_NF) || nf.NtOperacao.NF_devolucao))
                        {
                            ddlTipoDestinatario.Text = "FORNECEDOR";
                        }
                        if (nf.NtOperacao.NF_devolucao)
                        {
                            ddlFinalidade.SelectedValue = "4";
                        }
                        else
                        {
                            ddlFinalidade.SelectedValue = "1";
                        }

                        Session.Remove("obj" + urlSessao());
                        Session.Add("obj" + urlSessao(), nf);

                        Session.Remove("campoLista" + urlSessao());
                        Session.Add("campoLista" + urlSessao(), "txtCliente_CNPJ");
                        TxtPesquisaLista.Text = "";
                        exibeLista();
                        return;



                    }
                    else if (itemLista.Equals("txtReferenciaNota"))
                    {
                        String referencia = ListaSelecionada(5);
                        if (!referencia.Equals(""))
                        {
                            nf.NfReferencias.Add(referencia);
                            Session.Remove("obj" + urlSessao());
                            Session.Add("obj" + urlSessao(), nf);

                        }
                    }
                    else
                    {
                        if (txt != null)
                            txt.Text = ListaSelecionada(1);
                    }

                    if (txt != null)
                    {
                        if (txt.ID.Equals("txtCompPlu"))
                        {
                            txtCompPlu.Text = ListaSelecionada(1);
                            txtCompDescricao.Text = ListaSelecionada(2);

                            txtCompNCM.Text = ListaSelecionada(6);

                            modalComplementar.Show();
                        }
                        else if (txt.ID.Equals("txtCompNCM") ||
                                  txt.ID.Equals("txtCompCfop") ||
                           txt.ID.Equals("txtCompCstCofins") ||
                           txt.ID.Equals("txtCompCstPis")
                           )
                        {
                            modalComplementar.Show();

                        }
                        else if (txt.ID.Equals("txtPLU"))
                        {

                            MercadoriaDAO merc = new MercadoriaDAO(txt.Text, usr);

                            preco_mercadoriaDAO precoMercadoria = null;

                            txtDescricao.Text = merc.Descricao;
                            txtEmbalagem.Text = merc.Embalagem.ToString("N2");
                            txtPisItem.Text = merc.pis.ToString("N2");
                            txtDescontoItem.Text = "0,00";
                            txtQtde.Text = "1,00";
                            txtCodigoEmisaoNFe.Text = merc.codigoEmissaoNFe;
                            if (nf.NtOperacao != null)
                            {

                                if (nf.NtOperacao.Saida && nf.NtOperacao.NF_devolucao)
                                {
                                    txtUnitario.Text = (nf.NtOperacao.Preco_Venda ? merc.Preco.ToString("N4") : merc.preco_compra.ToString("N4"));




                                    if (!nf.NtOperacao.Tributacao_padrao.Equals(""))
                                    {
                                        txtCodigo_Tributacao.Text = nf.NtOperacao.Tributacao_padrao.ToString();
                                    }
                                    else
                                    {
                                        txtCodigo_Tributacao.Text = merc.Codigo_Tributacao_ent.ToString();

                                    }
                                    txtCSTPIS.Text = merc.cst_saida;

                                    //txtredutor_base.Text = merc.;
                                    CalculatotalItem(merc);

                                    txtIPI.Text = merc.IPI.ToString("N2");
                                    txtIPIV.Text = ((Funcoes.decTry(TxtTotalItem.Text) * merc.IPI) / 100).ToString("N2");

                                }
                                else if (!nf.NtOperacao.Saida)
                                {
                                    txtUnitario.Text = (nf.NtOperacao.Preco_Venda ? merc.Preco.ToString("N4") : merc.preco_compra.ToString("N4"));
                                    txtCodigo_Tributacao.Text = merc.Codigo_Tributacao_ent.ToString();
                                    txtCSTPIS.Text = merc.cst_entrada;
                                    //txtredutor_base.Text = merc.;
                                    CalculatotalItem(merc);

                                    txtIPI.Text = merc.IPI.ToString("N2");
                                    txtIPIV.Text = ((Funcoes.decTry(TxtTotalItem.Text) * merc.IPI) / 100).ToString("N2");
                                }
                                else
                                {
                                    if (nf.NtOperacao.Saida && nf.NtOperacao.Preco_Venda && !nf.objCliente.Codigo_tabela.Equals("") && !nf.objCliente.Codigo_tabela.Equals("0"))
                                    {
                                        //Guardando o valor origem para validar
                                        txtUnitarioOriginal.Text = (nf.NtOperacao.Preco_Venda ? merc.Preco.ToString("N4") : merc.preco_compra.ToString("N4"));

                                        precoMercadoria = new preco_mercadoriaDAO(nf.objCliente.Codigo_tabela, merc.PLU, usr);

                                        if (precoMercadoria != null)
                                        {
                                            if (precoMercadoria.Preco_promocao > 0)
                                            {
                                                txtUnitario.Text = precoMercadoria.Preco_promocao.ToString("N4");
                                                txtUnitarioOriginal.Text = precoMercadoria.Preco_promocao.ToString("N4");
                                            }
                                            else
                                            {
                                                txtUnitario.Text = (nf.NtOperacao.Preco_Venda ? merc.Preco.ToString("N4") : merc.preco_compra.ToString("N4"));
                                            }
                                        }
                                        else
                                        {
                                            txtUnitario.Text = (nf.NtOperacao.Preco_Venda ? merc.Preco.ToString("N4") : merc.preco_compra.ToString("N4"));
                                        }
                                    }
                                    else
                                    {
                                        txtUnitario.Text = (nf.NtOperacao.Preco_Venda ? merc.Preco.ToString("N4") : merc.preco_compra.ToString("N4"));
                                        txtUnitarioOriginal.Text = (nf.NtOperacao.Preco_Venda ? merc.Preco.ToString("N4") : merc.preco_compra.ToString("N4"));
                                    }


                                    if (!nf.NtOperacao.Tributacao_padrao.Equals(""))
                                    {
                                        txtCodigo_Tributacao.Text = nf.NtOperacao.Tributacao_padrao.ToString();
                                    }
                                    else
                                    {
                                        txtCodigo_Tributacao.Text = merc.Codigo_Tributacao.ToString();
                                    }
                                    //incidencia de PIS e COFINS
                                    if (!nf.NtOperacao.incide_PisCofins)
                                    {
                                        //Não incidência de PIS E COFINS utilizar CST 08 OPERAÇÃO SEM INCIDÊNCIA DA CONTRIBUIÇÃO
                                        txtCSTPIS.Text = "08";
                                    }
                                    else
                                    {
                                        txtCSTPIS.Text = merc.cst_saida;
                                    }

                                    txtredutor_base.Text = "0,00";

                                    if (precoMercadoria != null)
                                    {
                                        if (precoMercadoria.Preco_promocao > 0)
                                        {
                                            CalculatotalItem(merc, precoMercadoria);
                                        }
                                        else
                                        {
                                            CalculatotalItem(merc);
                                        }
                                    }
                                    else
                                    {
                                        CalculatotalItem(merc);
                                    }

                                    txtIPI.Text = "0,00"; //merc.IPI.ToString("N2");
                                    txtIPIV.Text = "0,00";//((Funcoes.decTry(TxtTotalItem.Text) * merc.IPI) / 100).ToString("N2");

                                }
                            }



                            txtUnd.Text = merc.und;
                            txtPeso_liquido.Text = merc.peso_liquido.ToString();
                            txtPeso_Bruto.Text = merc.peso_bruto.ToString();


                            txtCODIGO_REFERENCIA.Text = "0";
                            //txtaliquota_icms.Text = "0,00";

                            txtdespesas.Text = "0,00";
                            // txtCofinsItem.Text = "0,00";
                            txtNum_item.Text = (nf.qtdItens() + 1).ToString();
                            txtNCM.Text = merc.cf.Replace(".", "");
                            txtCEST.Text = merc.CEST.ToString();

                            ddlEscalaRelevante.SelectedValue = (merc.indEscala ? "S" : "N");
                            txtCNPJFab.Text = merc.cnpjFabricante.ToString();
                            txt_cBenef.Text = merc.cBenef;


                            ddlOrigem.SelectedValue = merc.origem.ToString();

                            decimal vBaseIva = Funcoes.decTry(txtBase_Calc_Subst.Text);



                            Decimal pFCP = 0;

                            Decimal.TryParse(Conexao.retornaUmValor("Select porc from uf_pobreza where uf ='" + nf.UfCliente + "'", null), out pFCP);

                            if (pFCP > 0)
                            {
                                if (vBaseIva > 0)
                                {
                                    txtItem_BaseFCP.Text = vBaseIva.ToString("N2");
                                    txtItem_pFCP.Text = pFCP.ToString("N2");
                                    txtItem_VlrFCP.Text = ((vBaseIva * pFCP) / 100).ToString("N2");
                                    //txtItem_BaseFCPST.Text = vBaseIva.ToString("N2");
                                    //txtItem_pFCPST.Text = pFCP.ToString("N2");
                                    //txtItem_VlrFCPST.Text = ((vBaseIva * pFCP) / 100).ToString("N2");
                                }
                                else
                                {
                                    decimal vBaseIcms = Funcoes.decTry(txtBase_Calculo.Text);
                                    txtItem_BaseFCP.Text = vBaseIcms.ToString("N2");
                                    txtItem_pFCP.Text = pFCP.ToString("N2");
                                    txtItem_VlrFCP.Text = ((vBaseIcms * pFCP) / 100).ToString("N2");
                                }
                            }


                            //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                            ItemInativo(merc.Inativo == 1);

                            //Caso já exista uma ordem de compra preenchida no cabeçalho, o sistema preenche o número do pedido do item com o valor existente.
                            if (txtPedidoItemNumero.Text.Trim().Equals("") && !txtOrdemCompra.Text.Trim().Equals(""))
                            {
                                txtPedidoItemNumero.Text = txtOrdemCompra.Text.Trim();
                            }


                        }

                        if (txt.ID.Equals("txtNCM"))
                        {
                            txt.Text = txt.Text.Trim().PadLeft(8, '0').Replace(".", "");
                        }

                        if (txt.ID.Equals("txtCliente_CNPJ"))
                        {


                            //if (nf.NtOperacao != null && ((!nf.NtOperacao.Saida && nf.NtOperacao.Imprime_NF) || nf.NtOperacao.NF_devolucao))
                            bool fantasiaObs = Funcoes.valorParametro("NOME_FANTASIA_ENF", usr).ToUpper().Equals("TRUE");

                            if (ddlTipoDestinatario.Text.Equals("FORNECEDOR"))
                            {
                                fornecedorDAO forn = new fornecedorDAO(ListaSelecionada(2), usr);
                                txtCliente_Fornecedor.Text = forn.Fornecedor;
                                TxtNomeCliente.Text = forn.Razao_social;
                                txtUfCliente.Text = forn.UF;
                                nf.DestFornecedor = true;
                                if (forn.indIEDest == 9)
                                {
                                    ddlindFinal.SelectedValue = "1";
                                }

                                //Atribuir o centro de custo quando se tratar de fornecdor e tipo de NF = 2 (Entrada)
                                if ((nf.Tipo_NF.Equals("2") && !nf.centro_custo.Trim().Equals("")) || nf.DestFornecedor )
                                {
                                    txtcentro_custo.Text = forn.centro_custo;
                                }

                            }
                            else
                            {
                                ClienteDAO cliente = new ClienteDAO(ListaSelecionada(2), usr);
                                
                                if ((nf.NtOperacao != null) && nf.NtOperacao.CNPJDestOrigem && 
                                    usr.filial.CNPJ.Replace(".","").Replace("-","").Replace("/","") != cliente.CNPJ.Replace(".", "").Replace("-", "").Replace("/", ""))
                                {
                                    throw new Exception("Quando trata-se de " + nf.NtOperacao.Descricao + " o CNPJ do destinatário deverá ser igual ao da loja.");
                                }

                                txtCliente_Fornecedor.Text = cliente.Codigo_Cliente;
                                TxtNomeCliente.Text = cliente.Nome_Cliente;
                                txtUfCliente.Text = cliente.UF;
                                txtcentro_custo.Text = visualSysWeb.code.Funcoes.valorParametro("CENTRO_CUSTO_NF_SAIDA", new User());
                                if (fantasiaObs)
                                {
                                    txtObservacao.Text += cliente.Nome_Cliente + "\n";
                                }

                                if (cliente.indIEDest == 9)
                                {
                                    ddlindFinal.SelectedValue = "1";
                                }
                            }
                            if (!txtCliente_Fornecedor.Text.Equals(""))
                            {
                                btnimg_txtCliente_CNPJ.Visible = false;
                                txtCliente_CNPJ.Enabled = false;
                            }
                            else
                            {
                                btnimg_txtCliente_CNPJ.Visible = true;
                                txtCliente_CNPJ.Enabled = true;

                            }
                            nf.DestFornecedor = ddlTipoDestinatario.Text.Equals("FORNECEDOR");
                            nf.Cliente_Fornecedor = txtCliente_Fornecedor.Text;
                            //nf.Nome_cliente = TxtNomeCliente.Text;
                            nf.vUfcliente = txtUfCliente.Text;

                            nf.centro_custo = txtcentro_custo.Text;

                            if (nf.NfItens.Count > 0 && !nf.Codigo_operacao.ToString().Equals("0"))
                            {
                                nf.atribuirCodOperacaoItem();
                            }

                            txtDataCupom.Text = DateTime.Now.ToString("dd/MM/yyyy");
                            txtDtDeMov.Text = DateTime.Now.ToString("dd/MM/yyyy");
                            txtDtAteMov.Text = DateTime.Now.ToString("dd/MM/yyyy");
                            divXmlDevolucaoImporta.Visible = nf.NtOperacao.NF_devolucao;

                            ModalImportar.Show();
                        }




                        if (txt.ID.Equals("txtCodigo_Tributacao") || txt.ID.Equals("txtCSTCOFINS") || txt.ID.Equals("txtCSTPIS"))
                        {
                            MercadoriaDAO merc = new MercadoriaDAO(txtPLU.Text, usr);
                            CalculatotalItem(merc);
                            ModalItens.Show();
                        }


                        if (txt.Parent.ID != null && txt.Parent.ID.Equals("pnItensFrame"))
                        {
                            ModalItens.Show();
                        }

                        if (txt.Parent.ID != null && txt.Parent.ID.Equals("PnPagamentoFrame"))
                        {
                            try
                            {
                                DateTime emissao = DateTime.Parse(txtEmissao.Text);
                                int d = int.Parse(Conexao.retornaUmValor("Select prazo from tipo_pagamento where tipo_pagamento ='" + txt.Text + "'", usr));
                                txtVencimentoPg.Text = emissao.AddDays(d).ToString("dd/MM/yyyy");
                                if (txt.Text.ToUpper().Contains("BOLETO"))
                                {
                                    ddltPag.SelectedValue = "15";
                                }
                                else if (txt.Text.ToUpper().Contains("CHEQUE"))
                                {
                                    ddltPag.SelectedValue = "02";
                                }
                                else
                                {
                                    ddltPag.SelectedValue = "99";
                                }

                                ModalPagamentos.Show();
                            }
                            catch (Exception)
                            {

                            }
                        }

                    }
                    modalLista.Hide();
                    carregarGrids();

                    if (txt != null && txt.ID.Equals("txtCliente_CNPJ"))
                    {
                        if (nf.Codigo_operacao.ToString().Equals("0"))
                        {
                            Session.Remove("campoLista" + urlSessao());
                            Session.Add("campoLista" + urlSessao(), "txtCodigo_operacao");
                            TxtPesquisaLista.Text = "";
                            exibeLista();
                        }

                    }

                    if (itemLista.Equals("txtFiltroCupomVarios"))
                    {
                        txtFiltroCupomVarios.Text = ListaSelecionada(1).Replace("&nbsp;", "");
                        if (txtFiltroCupomVarios.Text.Equals(""))
                            txtFiltroCupomVarios.Text = ListaSelecionada(2);

                        pesquisaVariosCupons();
                    }
                    if (itemLista.Equals("txtFiltroPedidoVarios"))
                    {
                        txtClienteVariosPedidos.Text = ListaSelecionada(1).Replace("&nbsp;", "");
                        if (txtClienteVariosPedidos.Text.Equals(""))
                            txtClienteVariosPedidos.Text = ListaSelecionada(2);
                        pesquisaVariosPedidos();
                    }
                }
                else
                {
                    lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                    modalLista.Show();
                }
            }
            catch (Exception err)
            {
                showMessage(err.Message, true);
            }
        }

        private void CalculatotalItem(MercadoriaDAO merc, preco_mercadoriaDAO precoMercadoria = null)
        {
            User usr = (User)Session["User"];
            tributacaoDAO trib = new tributacaoDAO(txtCodigo_Tributacao.Text, usr);
            nfDAO nf = (nfDAO)Session["obj" + urlSessao()];

            EnabledControls(pnItensFrame, true);
            txtUnitarioOriginal.Visible = false;

            if (precoMercadoria != null)
            {
                txtUnitario.Enabled = false;
                txtUnitario.BackColor = Color.FromArgb(0xDCDCDC);
                txtEmbalagem.Enabled = false;
                txtEmbalagem.BackColor = Color.FromArgb(0xDCDCDC);
                txtDescontoItem.Enabled = false;
                txtDescontoItem.BackColor = Color.FromArgb(0xDCDCDC);
                txtDescValorItem.Enabled = false;
                txtDescValorItem.BackColor = Color.FromArgb(0xDCDCDC);
                txtDescricao.Enabled = false;
                txtDescricao.BackColor = txtDescValorItem.BackColor = Color.FromArgb(0xDCDCDC);
            }


            decimal aliquotaicms = 0;
            decimal aliquotaIva = 0;
            decimal porcIva = 0;
            decimal aliquotaicmsdestino = 0;
            String indiceSt = "";
            bool Simples = usr.filial.Reg_Federal.ToUpper().Equals("SIMPLES NACIONAL");
            // Sempre bloqueado
            txtICMSDestino.Text = "0,00";
            txtICMSDestino.Enabled = false;
            if (nf.NtOperacao != null && ((!nf.NtOperacao.Saida && nf.NtOperacao.Imprime_NF) || nf.NtOperacao.NF_devolucao))
            {
                aliquotaicms = trib.Entrada_ICMS;

                if (Simples)
                {
                    indiceSt = trib.csosn;
                }
                else
                {
                    indiceSt = trib.Indice_ST;
                }
                porcIva = merc.margem_iva;
                if (porcIva > 0)
                {
                    String strAliquotaIva = Conexao.retornaUmValor("Select top 1 aliquota_iva " +
                                                                             " from nf_item " +
                                                                                 " inner join nf on NF_Item.codigo = nf.Codigo and nf.Tipo_NF = NF_Item.Tipo_NF " +
                                                                             " where plu='" + merc.PLU.Trim() + "' and nf.tipo_nf=2 order by convert(varchar,nf.data,102) desc;", usr);
                    if (strAliquotaIva.Equals("0") || strAliquotaIva.Equals("0,00") || strAliquotaIva.Equals(""))
                    {
                        aliquotaIva = trib.Entrada_ICMS;
                    }
                    else
                    {
                        Decimal.TryParse(strAliquotaIva, out aliquotaIva);
                    }

                }
                else
                {
                    aliquotaIva = trib.Entrada_ICMS;
                }

            }
            else
            {

                if (nf.UfCliente.Equals(usr.filial.UF.ToUpper()))
                {
                    aliquotaicms = trib.Saida_ICMS;
                    aliquotaIva = trib.Saida_ICMS;
                    porcIva = merc.margem_iva;
                    if (Simples)
                    {
                        indiceSt = trib.csosn;
                    }
                    else
                    {
                        indiceSt = trib.Indice_ST;
                    }
                }
                else
                {
                    SqlDataReader rsAliquotaEstado = null;
                    try
                    {
                        //Caso trata-se de produto importado ou com matéria prima importada maior ou igual a 40%
                        if (merc.origem == 1 || merc.origem == 2 || merc.origem == 3 || merc.origem == 8)
                        {
                            aliquotaicms = 4;
                            
                        }
                        else
                        {
                            switch (nf.UfCliente)
                            {
                                case "MG":
                                case "PR":
                                case "RS":
                                case "RJ":
                                case "SC":
                                    aliquotaicms = 12;
                                    break;
                                default:
                                    aliquotaicms = 7;
                                    break;
                            }
                        }
                        porcIva = 0;
                        aliquotaIva = 0;
                        indiceSt = "00";

                        //Checa se é necessário validar o NCM Difal
                        if (!usr.filial.CRT.Equals("1"))
                        {
                            rsAliquotaEstado = Conexao.consulta("select * from aliquota_imp_estado where uf='" + nf.UfCliente + "' and ncm='" + merc.cf + "'", null, false);
                            if (rsAliquotaEstado.Read())
                            {
                                aliquotaicmsdestino = Funcoes.decTry(rsAliquotaEstado["icms_interestadual"].ToString());
                                //Não pode haver alíquota de destino menor ou igual a zero.
                                if (aliquotaicmsdestino <= 0)
                                {
                                    throw new Exception("PLU: " + txtPLU.Text + " o NCM:" + merc.cf + " configurado com valor 0 (zero).");
                                }

                                txtICMSDestino.Text = aliquotaicmsdestino.ToString("N2");
                                //porcIva = Funcoes.decTry(rsAliquotaEstado["iva_ajustado"].ToString());
                                //aliquotaIva = Funcoes.decTry(rsAliquotaEstado["icms_estado"].ToString());
                                //if (aliquotaicms > 0)
                                //    indiceSt = "00";
                                //else
                                //    indiceSt = rsAliquotaEstado["CST"].ToString();
                            }
                            else
                            {
                                throw new Exception("PLU: " + txtPLU.Text + " o NCM:" + merc.cf + " não configurado.");
                            }
                        }
                        else
                        {
                            aliquotaicmsdestino = 0;
                            txtICMSDestino.Text = aliquotaicmsdestino.ToString("N2");
                        }
                    }
                    catch (Exception e)
                    {

                        throw e;
                    }
                    finally
                    {
                        if (rsAliquotaEstado != null)
                          rsAliquotaEstado.Close();
                    }

                    //Checa tributação interestadual
                    if (Simples && nf.indFinal == 0 && nf.Tipo_NF.Equals("1"))
                    {
                        aliquotaicms = 0;
                        indiceSt = trib.csosn;
                    }
                    else if (nf.indFinal == 1 && nf.Tipo_NF.Equals("1"))
                    {
                        switch (merc.origem)
                        {
                            case 1:
                            case 2:
                            case 3:
                            case 8:
                                aliquotaicms = 4;
                                aliquotaIva = 0;
                                porcIva = 0;
                                 if (Simples)
                                {
                                    indiceSt = trib.csosn;
                                }
                                else
                                {
                                    indiceSt = "00";
                                }
                                break;
                            default:
                                if (Simples)
                                {
                                    aliquotaicms = 0;
                                    indiceSt = trib.csosn;
                                }
                                break;
                        }
                    }

                }


            }

            //Caso a NFe não seja uma NFe de devolução. 2022.01.25 Jailson
            //Comentado o bloqueio da edição da devolução em 2023.11.09 14:33 a pedido do Leonardo. (responsável pela alteração: Jailson.)
            
            if (indiceSt != null) // && !nf.NtOperacao.NF_devolucao)
            {
                txtindice_st.Text = indiceSt.Trim();
                switch (indiceSt.Trim())
                {
                    case "00":
                        txtaliquota_icms.Text = aliquotaicms.ToString("N2");
                        txtredutor_base.Enabled = false; 
                        txtredutor_base.Text = "0,00";
                        txtredutor_base.BackColor = Color.FromArgb(0xDCDCDC);
                        txtiva.Enabled = false;
                        txtiva.Text = "0,00";
                        txtiva.BackColor = Color.FromArgb(0xDCDCDC);
                        txtmargem_iva.Enabled = false;
                        txtmargem_iva.Text = "0,00";
                        txtmargem_iva.BackColor = Color.FromArgb(0xDCDCDC);
                        txtAliquota_iva.Enabled = false;
                        txtAliquota_iva.Text = "0,00";
                        txtAliquota_iva.BackColor = Color.FromArgb(0xDCDCDC);


                        break;

                    case "30":
                        txtredutor_base.Enabled = false;
                        txtredutor_base.Text = "0,00";
                        txtredutor_base.BackColor = Color.FromArgb(0xDCDCDC);

                        break;
                    case "20":
                        txtaliquota_icms.Text = aliquotaicms.ToString("N2");
                        txtiva.Enabled = false;
                        txtiva.Text = "0,00";
                        txtiva.BackColor = Color.FromArgb(0xDCDCDC);
                        txtmargem_iva.Enabled = false;
                        txtmargem_iva.Text = "0,00";
                        txtmargem_iva.BackColor = Color.FromArgb(0xDCDCDC);
                        txtAliquota_iva.Enabled = false;
                        txtAliquota_iva.Text = "0,00";
                        txtAliquota_iva.BackColor = Color.FromArgb(0xDCDCDC);
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
                        txtAliquota_iva.Enabled = false;
                        txtAliquota_iva.Text = "0,00";
                        txtAliquota_iva.BackColor = Color.FromArgb(0xDCDCDC);

                        break;
                    case "70":
                    case "10":
                        txtaliquota_icms.Text = aliquotaicms.ToString("N2");
                        txtredutor_base.Text = trib.Redutor.ToString("N2");
                        txtmargem_iva.Text = porcIva.ToString("N2");
                        txtAliquota_iva.Text = aliquotaIva.ToString("N2");

                        break;
                    case "101": // Tributação do ICMS pelo SIMPLES NACIONAL e CSOSN=101
                        txtaliquota_icms.Text = "0,00";
                        txtredutor_base.Text = "0,00";
                        txtiva.Enabled = false;
                        txtiva.Text = "0,00";
                        txtiva.BackColor = Color.FromArgb(0xDCDCDC);
                        txtpCredSN.Text = "0,00";
                        break;
                    case "102":
                    case "103":
                    case "300":
                    case "400":
                    case "500":
                        txtaliquota_icms.Text = "0,00";
                        txtredutor_base.Text = "0,00";
                        txtpCredSN.Text = "0,00";
                        txtvCredicmssn.Text = "0,00";
                        txtmargem_iva.Text = "0,00";
                        txtAliquota_iva.Text = "0,00";
                        break;
                    case "201":
                    case "203":
                        txtredutor_base.Text = trib.Redutor.ToString("N2");
                        txtmargem_iva.Text = porcIva.ToString("N2");
                        txtAliquota_iva.Text = aliquotaIva.ToString("N2");
                        txtpCredSN.Text = trib.Saida_ICMS.ToString("N2");
                        break;
                    case "900":
                        txtaliquota_icms.Text = aliquotaicms.ToString("N2");
                        txtredutor_base.Text = trib.Redutor.ToString("N2");
                        txtAliquota_iva.Text = aliquotaIva.ToString("N2");
                        txtpCredSN.Text = "0,00";
                        break;
                }
            }
            else if (nf.NtOperacao.NF_devolucao)
            {
                //Caso trata-se de uma NFe de devolução
                txtaliquota_icms.Enabled = true;
                txtAliquota_iva.Enabled = true;
                txtiva.Enabled = true;
                txtmargem_iva.Enabled = true;
                txtredutor_base.Enabled = true;
                txtindice_st.Enabled = true;

            }


            String strcfop = "";
            if (txtCodigo_operacao_item.Text.Equals(""))
            {
                if (nf.NtOperacao.Saida)
                {
                    if (txtUfCliente.Text.Equals(usr.filial.UF))
                    {
                        strcfop = "5";
                    }
                    else
                    {
                        strcfop = "6";
                    }
                }
                else
                {
                    if (txtUfCliente.Text.Equals(usr.filial.UF))
                    {
                        strcfop = "1";
                    }
                    else
                    {
                        strcfop = "2";
                    }

                }
                //Checa se o destinatário não possui IE, caso não possua, o sistema deverá atribuir 
                //CFOP 6108
                if (nf.objCliente.UF != usr.filial.UF 
                    &&  (nf.objCliente.indIEDest.ToString().Equals("2") || nf.objCliente.indIEDest.ToString().Equals("9")) 
                    && nf.indFinal.ToString().Equals("1"))
                {
                    strcfop = "6108"; //Venda de mercadoria adquirida ou recebida de terceiros, destinada a não contribuinte
                }
                else
                {
                    if (nf.NtOperacao.utilizaCFOP)
                    {
                        bool st = false;
                        switch (indiceSt)
                        {
                            case "00":
                            case "20":
                            case "40":
                            case "41":
                            case "50":
                            case "51":
                            case "101":
                            case "102":
                            case "103":
                                st = false;
                                break;
                            default:
                                st = true;
                                break;
                        }
                        if (nf.NtOperacao.cfop_st.Trim().Length > 0 && st)
                        {
                            strcfop += nf.NtOperacao.cfop_st.Substring(1);
                        }
                        else if (nf.NtOperacao.cfop.Trim().Length > 0)
                        {
                            strcfop += nf.NtOperacao.cfop.Substring(1);
                        }
                        else
                        {
                            strcfop += nf.NtOperacao.Codigo_operacao.ToString().Substring(1);
                        }

                    }
                    else if (nf.NtOperacao.Saida)
                    {
                        if (nf.NtOperacao.Saida && merc.CFOP.Length > 0)
                        {
                            strcfop += merc.CFOP.Substring(1);
                        }
                        else if (nf.NtOperacao.NF_devolucao)
                        {
                            switch (indiceSt.Trim())
                            {

                                case "30":
                                case "10":
                                case "70":
                                case "60":
                                case "201":
                                case "202":
                                case "203":
                                case "500":
                                    strcfop += "411";
                                    break;
                                default:
                                    strcfop += "202";
                                    break;
                            }
                        }
                        else
                        {
                            if (trib.cfop > 0)
                            {
                                strcfop += trib.cfop.ToString();
                            }
                            else
                            {
                                strcfop += nf.NtOperacao.Codigo_operacao.ToString().Substring(1);
                            }
                        }

                    }
                    else
                    {
                        strcfop += nf.NtOperacao.Codigo_operacao.ToString().Substring(1);
                    }
                }

                txtCodigo_operacao_item.Text = strcfop;
            }

            nf_itemDAO item = carregaItem();
            if (nf.objCliente.UF != usr.filial.UF
                && (nf.objCliente.indIEDest.ToString().Equals("2") || nf.objCliente.indIEDest.ToString().Equals("9"))
                && nf.indFinal.ToString().Equals("1"))
            {
                item.difalContribuinte = true; //Venda de mercadoria adquirida ou recebida de terceiros, destinada a não contribuinte
            }

            item.CalculaImpostos();

            TxtTotalItem.Text = item.vtotal_produto.ToString("N2");
            txtPisItem.Text = item.PISp.ToString("N2");
            txtCofinsItem.Text = item.COFINSp.ToString("N2");
            txtVlrCofinsItem.Text = item.COFINSV.ToString("N2");
            txtVlrPisItem.Text = item.PISV.ToString("N2");
            txtCSTPIS.Text = item.CSTPIS;

            if (trib != null && trib.Incide_ICM_Subistituicao)
            {
                if (item.vmargemIva > 0)
                    txtiva.Text = item.CalculoIva().ToString("N2");
                else
                    txtiva.Text = "0,00";

            }
            else
            {
                txtiva.Text = "0,00";
                txtmargem_iva.Enabled = false;
                txtmargem_iva.Text = "0,00";
                txtmargem_iva.BackColor = Color.FromArgb(0xDCDCDC);

            }

            if (item.IPI > 0)
            {
                txtIPI.Text = item.porcIPI.ToString("N2");
                txtIPIV.Text = item.IPIV.ToString("N2");
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

        protected void btnCancelaItem_Click(object sender, ImageClickEventArgs e)
        {
            habilitarCampos(true);
            carregarDados();
            Session.Remove("item" + urlSessao());
        }

        protected void ImgExcluiItem_Click(object sender, ImageClickEventArgs e)
        {
            nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
            nf.removeItem(nf.item(int.Parse(txtNum_item.Text) - 1));
            Session.Remove("obj" + urlSessao());
            Session.Add("obj" + urlSessao(), nf);
            PnExcluirItem.Visible = false;
            habilitarCampos(true);
            carregarDados();
        }

        protected void gridPagamentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
            nf.removePagamento(index);
            habilitarCampos(true);
            carregarDados();
        }



        protected void AddPagamento_Click(object sender, ImageClickEventArgs e)
        {
            if (!txtCliente_Fornecedor.Text.Equals(""))
            {
                lblError.Text = "";
                LimparCampos(PnAddPagamento);

                ModalPagamentos.Show();
                nfDAO nf = (nfDAO)Session["obj" + urlSessao()];

                SqlDataReader rsPgPadrao = null;
                try
                {


                    rsPgPadrao = Conexao.consulta("Select * from tipo_pagamento where padrao = 1", null, false);
                    if (rsPgPadrao.Read())
                    {

                        int dias = Funcoes.intTry(rsPgPadrao["prazo"].ToString());
                        txtVencimentoPg.Text = DateTime.Now.AddDays(dias).ToString("dd/MM/yyyy");
                        txtTipoPg.Text = rsPgPadrao["tipo_pagamento"].ToString();

                    }

                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    if (rsPgPadrao != null)
                        rsPgPadrao.Close();
                }
                txtValorPg.Text = (nf.Total - nf.TotalPag()).ToString("N2");
            }
            else
            {
                lblError.Text = "Inclua as Informações do Codigo e Fornecedor";
            }
        }



        protected void btnConfirmaPagamentos_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                txtTipoPg.BackColor = System.Drawing.Color.White;
                if (txtTipoPg.Text.Equals(""))
                {
                    txtTipoPg.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Tipo de Pagamento Não informado");
                }

                nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
                User usr = (User)Session["user"];
                nf_pagamentoDAO pg = new nf_pagamentoDAO(usr);
                pg.Codigo = txtCodigo.Text;
                pg.Cliente_Fornecedor = txtCliente_Fornecedor.Text;
                pg.Tipo_NF = "2";
                pg.Tipo_pagamento = txtTipoPg.Text;
                pg.Vencimento = DateTime.Parse(txtVencimentoPg.Text);
                pg.Valor = Funcoes.decTry(txtValorPg.Text);
                bool SemPag = Conexao.retornaUmValor("Select sem_pagamento from tipo_pagamento where tipo_pagamento ='" + pg.Tipo_pagamento + "'", null).Equals("1");
                if (SemPag || ddlFinalidade.SelectedItem.Value.Equals("4"))
                {
                    ddltPag.SelectedValue = "90";
                }
                else if (pg.Tipo_pagamento.ToUpper().Contains("BOLETO"))
                {
                    ddltPag.SelectedValue = "15";
                }
                else
                {
                    ddltPag.SelectedValue = "99";
                }
                nf.addPagamento(pg);

                lblTotalPagamentos.Text = nf.TotalPag().ToString("N2");
                Session.Remove("obj" + urlSessao());
                Session.Add("obj" + urlSessao(), nf);
                habilitarCampos(true);
                carregarGrids();
            }
            catch (Exception err)
            {
                lblErroPg.Text = "Pagamento Invalido:" + err.Message;
                ModalPagamentos.Show();
            }


        }

        protected void btnCancelaPagamentos_Click(object sender, ImageClickEventArgs e)
        {
            ModalPagamentos.Hide();
            carregarGrids();
        }

        protected void btnConfirmaImportar_Click(object sender, ImageClickEventArgs e)
        {
            bool importa = true;
            try
            {
                User usr = (User)Session["User"];
                if (txtNumeroPedido.Text.Equals("") &&
                    txtCupom.Text.Trim().Equals("") &&
                    txtChaveXml.Text.Trim().Equals("") &&
                    txtDescOutrasMovimentacoes.Text.Trim().Equals("")
                    )
                {
                    lblErrorPedido.Text = "PEDIDO,CUPOM,CHAVE OU OUTRAS MOVIMENTAÇÃO INVÁLIDO!! FAVOR INFORMAR NOVAMENTE.";
                    ModalImportar.Show();
                }
                else
                {
                    //status = "incluir";
                    nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
                    //nf.Tipo_NF = "1";
                    //nf.centro_custo = visualSysWeb.code.Funcoes.valorParametro("CENTRO_CUSTO_NF_SAIDA", new User());
                    ModalImportar.Hide();

                    if (!txtNumeroPedido.Text.Equals(""))
                    {

                        String statusPedido = Conexao.retornaUmValor("Select status from pedido where pedido='" + txtNumeroPedido.Text + "' and tipo=" + ddlTipoPedido.SelectedValue + " and filial ='" + usr.getFilial() + "'", usr);
                        if (statusPedido.Equals("2"))
                        {
                            bool liberaPedidoFechado = Funcoes.valorParametro("LIBERA_PEDIDO_FECHADO", usr).ToUpper().Equals("TRUE");
                            if (liberaPedidoFechado)
                            {
                                importa = false;
                                lblConfirmaPedidoFechado.Text = txtNumeroPedido.Text;
                                modalConfirmaPedidoFechado.Show();
                            }
                            else
                            {
                                throw new Exception("Pedido Cancelado ou Concluido");

                            }
                        }
                        else
                        {
                            nf.importarPedido(txtNumeroPedido.Text, int.Parse(ddlTipoPedido.SelectedValue));

                            // AddNovoCupom.Visible = false;


                        }
                    }
                    else if (!txtCupom.Text.Equals(""))
                    {
                        if (txtCaixa.Text.Equals("") || txtDataCupom.Text.Equals(""))
                        {
                            throw new Exception("PREENCA AS INFORMAÇÕES DE CAIXA E DATA DO CUPOM");

                        }
                        else
                        {
                            DateTime dt = DateTime.Parse(txtDataCupom.Text);
                            nf.importarCupom(txtCupom.Text, txtCaixa.Text, dt);


                            //AddNovoCupom.Visible = true;

                        }
                    }
                    else if (!txtChaveXml.Text.Trim().Equals(""))
                    {
                        nf.Tipo_NF = "2";
                        nf.importarNFeProdutor(txtChaveXml.Text);

                    }
                    else if (!txtDescOutrasMovimentacoes.Text.Trim().Equals(""))
                    {
                        if (!IsDate(txtDtDeMov.Text) || !IsDate(txtDtAteMov.Text))
                        {
                            txtDtAteMov.BackColor = System.Drawing.Color.Red;
                            txtDtDeMov.BackColor = System.Drawing.Color.Red;
                            throw new Exception("PREENCA O PERIODO CORRETAMENTE");
                        }
                        importa = false;
                        lblErroMovimentacao.Text = "";
                        divMsgImpMovimento.Visible = true;
                        lblFiltro.Text = "De:" + txtDtDeMov.Text + " ate " + txtDtAteMov.Text + " Descricao:" + txtDescOutrasMovimentacoes.Text;
                        String strSqlOut = "Select Codigo_inventario,tipoMovimentacao,Descricao_inventario, CONVERT(varchar, data,103) as data, status " +
                                           " from Inventario" +
                                            " Where data between '" + DateTime.Parse(txtDtDeMov.Text).ToString("yyyy-MM-dd") + "' and '" + DateTime.Parse(txtDtAteMov.Text).ToString("yyyy-MM-dd") + "'" +
                                                    " and (" +
                                                    " Descricao_inventario like '%" + txtDescOutrasMovimentacoes.Text + "%'" +
                                                    " or codigo_inventario like '%" + txtDescOutrasMovimentacoes.Text + "%'" +
                                                    ")" +
                                                    " and importado_Nota is null" +
                                                    " and status='ENCERRADO'";

                        gridMovimentacoes.DataSource = Conexao.GetTable(strSqlOut, usr, false);
                        gridMovimentacoes.DataBind();


                        modalOutrasMovimentacoes.Show();
                    }


                    if (importa)
                    {
                        Session.Remove("obj" + urlSessao());
                        Session.Add("obj" + urlSessao(), nf);
                        carregarDados();
                        if (nf.Cliente_Fornecedor.Equals(""))
                        {
                            Session.Remove("campoLista" + urlSessao());
                            Session.Add("campoLista" + urlSessao(), "txtCliente_CNPJ");
                            TxtPesquisaLista.Text = "";
                            exibeLista();
                        }
                        else if (nf.Codigo_operacao.ToString().Equals("0"))
                        {
                            Session.Remove("campoLista" + urlSessao());
                            Session.Add("campoLista" + urlSessao(), "txtCodigo_operacao");
                            TxtPesquisaLista.Text = "";
                            exibeLista();
                        }
                    }

                    if (!txtCliente_CNPJ.Text.Equals(""))
                    {
                        btnimg_txtCliente_CNPJ.Visible = false;
                    }


                }
            }
            catch (Exception err)
            {

                lblErrorPedido.Text = err.Message;
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
            // AddNovoCupom.Visible = false;
            if (txtTransportadora.Text.Equals(""))
                txtTransportadora.Text = Conexao.retornaUmValor("Select Nome_transportadora from Transportadora where padrao = 1", null);
            //txtcentro_custo.Text = visualSysWeb.code.Funcoes.valorParametro("CENTRO_CUSTO_NF_SAIDA", new User());
            //txtCodigo_operacao.Text = "0";
            nfDAO nf = (nfDAO)Session["obj" + urlSessao()];

            if (nf.Codigo_operacao.ToString().Equals("0"))
            {
                Session.Remove("campoLista" + urlSessao());
                Session.Add("campoLista" + urlSessao(), "txtCodigo_operacao");
                TxtPesquisaLista.Text = "";
                exibeLista();
            }

        }
        protected void carregarDadosItens(nf_itemDAO itemnf)
        {
            User usr = (User)Session["user"];
            nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
            txtPLU.Text = itemnf.PLU;
            txtCODIGO_REFERENCIA.Text = itemnf.CODIGO_REFERENCIA;
            txtDescricao.Text = itemnf.Descricao;
            txtQtde.Text = itemnf.Qtde.ToString("N3");
            txtEmbalagem.Text = itemnf.Embalagem.ToString();
            txtUnitario.Text = itemnf.Unitario.ToString();
            txtCodigo_Tributacao.Text = itemnf.Codigo_Tributacao.ToString();


            txtDescontoItem.Text = itemnf.Desconto.ToString();
            txtDescValorItem.Text = itemnf.DescontoValor.ToString("N2");
            txtdespesas.Text = itemnf.despesas.ToString(); 
            txtIPI.Text = itemnf.IPI.ToString("N2");
            txtIPIV.Text = itemnf.IPIV.ToString();
            txtaliquota_icms.Text = itemnf.aliquota_icms.ToString("N2");
            txtAliquota_iva.Text = itemnf.aliquota_iva.ToString("N2");
            txtindice_st.Text = itemnf.vIndiceSt.ToString();
            txtredutor_base.Text = itemnf.redutor_base.ToString("N2");
            txtPisItem.Text = itemnf.PISp.ToString("N2");
            txtCofinsItem.Text = itemnf.COFINSp.ToString("N2");
            txtVlrPisItem.Text = itemnf.PISV.ToString("N2");
            txtVlrCofinsItem.Text = itemnf.COFINSV.ToString("N2");
            txtNum_item.Text = itemnf.Num_item.ToString();
            txtCodigo_operacao_item.Text = itemnf.codigo_operacao.ToString();
            txtNCM.Text = itemnf.NCM.ToString();
            txtCEST.Text = itemnf.CEST;
            txtUnd.Text = itemnf.Und;
            txtPeso_liquido.Text = itemnf.Peso_liquido.ToString("N2");
            txtPeso_Bruto.Text = itemnf.Peso_Bruto.ToString("N2");
            txtCSTPIS.Text = itemnf.CSTPIS;
            txtPisItem.Text = itemnf.PISp.ToString("N2");
            txtVlrPisItem.Text = itemnf.PISV.ToString("N2");


            txtvCredicmssn.Text = itemnf.vCredicmssn.ToString("N2");
            txtpCredSN.Text = itemnf.pCredSN.ToString("N2");
            ModalItens.Show();
            if (!nf.NtOperacao.NF_devolucao)
            {
                MercadoriaDAO merc = new MercadoriaDAO(itemnf.PLU, usr);
                //Inclusão para trabalhar bloqueando cmapos qdo o cliente é tabelado.
                merc.margem_iva = itemnf.vmargemIva;
                preco_mercadoriaDAO precoMercadoria = null;
                if (nf.NtOperacao.Saida && nf.NtOperacao.Preco_Venda && !nf.objCliente.Codigo_tabela.Equals("") && !nf.objCliente.Codigo_tabela.Equals("0"))
                {
                    txtUnitarioOriginal.Text = itemnf.Unitario.ToString("N4");
                    precoMercadoria = new preco_mercadoriaDAO(nf.objCliente.Codigo_tabela, itemnf.PLU, usr);
                    if (precoMercadoria != null)
                    {
                        if (precoMercadoria.Preco_promocao <= 0)
                        {
                            precoMercadoria = null;
                        }
                        else
                        {
                            txtUnitarioOriginal.Text = precoMercadoria.Preco_promocao.ToString("N4");
                        }
                    }
                }
                CalculatotalItem(merc, precoMercadoria);
            }

            txtiva.Text = itemnf.vIva.ToString("N2");

            ddlOrigem.SelectedValue = itemnf.origem;
            txtmargem_iva.Text = itemnf.vmargemIva.ToString("N2");

            TxtTotalItem.Text = itemnf.Total.ToString("N2");
            lblInativo.Text = (itemnf.inativo ? "inativo" : "");
            ItemInativo(itemnf.inativo);

            //Nfe 4.0
            ddlEscalaRelevante.SelectedValue = (itemnf.indEscala ? "S" : "N");
            txtCNPJFab.Text = itemnf.cnpj_Fabricante;
            txt_cBenef.Text = itemnf.cBenef;

            txtItem_BaseFCP.Text = itemnf.vBCFCP.ToString("N2");
            txtItem_pFCP.Text = itemnf.pFCP.ToString("N2");
            txtItem_VlrFCP.Text = itemnf.vFCP.ToString("N2");

            txtItem_BaseFCPST.Text = itemnf.vBCFCPST.ToString("N2");
            txtItem_pFCPST.Text = itemnf.pFCPST.ToString("N2");
            txtItem_VlrFCPST.Text = itemnf.vFCPST.ToString("N2");
            txtItem_pDevolv.Text = itemnf.pDevol.ToString("N2");
            txtItem_vIPIDevol.Text = itemnf.vIPIDevol.ToString("N2");
            txtFreteItem.Text = itemnf.Frete.ToString("N2");

            txtCodigoProdutoAnvisa.Text = itemnf.codigo_produto_ANVISA;
            txtMotivoIsencaoAnvisa.Text = itemnf.motivo_isencao_ANVISA;
            txtPrecoMaximoAnvisa.Text = itemnf.preco_Maximo_ANVISA.ToString("N2");
            txtCodigoEmisaoNFe.Text = itemnf.codigoEmissaoNFe;

            txtPedidoItemNumero.Text = itemnf.pedidoItemNumero;
            txtPedidoItemSequencia.Text = itemnf.pedidoItemSequencia;

            ItemInativo(itemnf.inativo);
        }
        protected void txt_TextChanged(object sender, EventArgs e)
        { // carregarDadosItens(carregaItem());
            LblErroItem.Text = "";
            ((TextBox)sender).BackColor = System.Drawing.Color.White;

            try
            {

                String id = ((TextBox)sender).ID;
                nf_itemDAO itemnf = (nf_itemDAO)Session["item" + urlSessao()];
                switch (id)
                {
                    case "txtQtde":
                        itemnf.vQtde = Funcoes.decTry(txtQtde.Text);
                        txtUnitario.Text = itemnf.Unitario.ToString();
                        itemnf.CalculaImpostos();
                        txtEmbalagem.Focus();
                        txtIPIV.Text = itemnf.IPIV.ToString("N2");
                        txtiva.Text = itemnf.CalculoIva().ToString("N2");
                        txtVlrPisItem.Text = itemnf.PISV.ToString("N2");
                        txtVlrCofinsItem.Text = itemnf.COFINSV.ToString("N2");
                        break;
                    case "txtEmbalagem":
                        itemnf.Embalagem = Funcoes.decTry(txtEmbalagem.Text);
                        txtUnitario.Text = itemnf.Unitario.ToString();
                        itemnf.CalculaImpostos();
                        txtUnitario.Focus();
                        txtIPIV.Text = itemnf.IPIV.ToString("N2");
                        txtiva.Text = itemnf.CalculoIva().ToString("N2");
                        txtVlrPisItem.Text = itemnf.PISV.ToString("N2");
                        txtVlrCofinsItem.Text = itemnf.COFINSV.ToString("N2");
                        break;
                    case "txtUnitario":
                        itemnf.Unitario = Funcoes.decTry(txtUnitario.Text);
                        itemnf.CalculaImpostos();
                        txtIPIV.Text = itemnf.vIpiv.ToString("N2");
                        txtiva.Text = itemnf.vIva.ToString("N2");
                        txtVlrPisItem.Text = itemnf.PISV.ToString("N2");
                        txtVlrCofinsItem.Text = itemnf.COFINSV.ToString("N2");
                        txtCodigo_Tributacao.Focus();
                        break;
                    case "txtDescontoItem":
                        itemnf.Desconto = Funcoes.decTry(txtDescontoItem.Text);
                        itemnf.CalculaImpostos();
                        txtDescValorItem.Text = itemnf.DescontoValor.ToString("N2");
                        txtIPIV.Text = itemnf.IPIV.ToString("N2");
                        txtiva.Text = itemnf.CalculoIva().ToString("N2");
                        txtVlrPisItem.Text = itemnf.PISV.ToString("N2");
                        txtVlrCofinsItem.Text = itemnf.COFINSV.ToString("N2");
                        txtIPI.Focus();
                        break;
                    case "txtDescValorItem":
                        itemnf.DescontoValor = Funcoes.decTry(txtDescValorItem.Text);
                        txtDescontoItem.Text = itemnf.Desconto.ToString("N2");
                        itemnf.CalculaImpostos();
                        txtIPI.Focus();
                        break;
                    case "txtdespesas":
                        itemnf.despesas = Funcoes.decTry(txtdespesas.Text);
                        itemnf.CalculaImpostos();
                        txtIPI.Focus();
                        break;
                    case "txtIPI":
                        itemnf.IPI = Funcoes.decTry(txtIPI.Text);
                        itemnf.CalculaImpostos();
                        txtIPI.Text = itemnf.IPI.ToString("N2");
                        txtIPIV.Text = itemnf.vIpiv.ToString("N2");
                        txtIPIV.Focus();
                        txtiva.Text = itemnf.vIva.ToString("N2");
                        break;
                    case "txtIPIV":
                        itemnf.IPIV = Funcoes.decTry(txtIPIV.Text);
                        itemnf.CalculaImpostos();
                        txtIPIV.Text = itemnf.IPIV.ToString("N2");
                        txtIPI.Text = itemnf.IPI.ToString("N2");
                        txtmargem_iva.Focus();

                        txtiva.Text = itemnf.vIva.ToString("N2");
                        break;
                    case "txtmargem_iva":
                        itemnf.vmargemIva = Funcoes.decTry(txtmargem_iva.Text);
                        itemnf.CalculaImpostos();
                        txtmargem_iva.Text = itemnf.vmargemIva.ToString("N2");
                        txtiva.Text = itemnf.vIva.ToString("N2");
                        if (txtiva.Enabled)
                            txtiva.Focus();
                        else
                            txtaliquota_icms.Focus();
                        break;
                    case "txtiva":
                        itemnf.vIva = Funcoes.decTry(txtiva.Text);
                        itemnf.CalculaImpostos();
                        txtmargem_iva.Text = itemnf.CalculoMargem_iva.ToString("N2");
                        txtaliquota_icms.Focus();
                        break;
                    case "txtaliquota_icms":
                        if (txtredutor_base.Enabled)
                            txtredutor_base.Focus();
                        else
                            txtPisItem.Focus();
                        break;
                    case "txtredutor_base":
                        itemnf.redutor_base = Funcoes.decTry(txtredutor_base.Text);
                        itemnf.CalculaImpostos();
                        txtPisItem.Focus();
                        break;
                    case "txtPisItem":
                        txtCofinsItem.Focus();
                        break;
                    case "txtCofinsItem":
                        txtCodigo_operacao_item.Focus();
                        break;
                    case "txtCodigo_Tributacao":
                        txtCodigo_Tributacao.BackColor = System.Drawing.Color.White;

                        if (!txtCodigo_Tributacao.Text.Trim().Equals(""))
                        {
                            User usr = (User)Session["User"];
                            //itemnf.Codigo_Tributacao = Funcoes.decTry(txtCodigo_Tributacao.Text);
                            MercadoriaDAO merc = new MercadoriaDAO(txtPLU.Text, usr);
                            CalculatotalItem(merc);
                            ModalItens.Show();
                            txtDescontoItem.Focus();
                        }
                        else
                        {
                            txtCodigo_Tributacao.BackColor = System.Drawing.Color.Red;
                            txtCodigo_Tributacao.Focus();
                        }

                        break;
                    case "txtpCredSN":
                        Decimal.TryParse(txtpCredSN.Text, out itemnf.pCredSN);
                        txtvCredicmssn.Text = itemnf.vlrcredIcmssn.ToString("N2");
                        break;



                }


                TxtTotalItem.Text = itemnf.vtotal_produto.ToString("N2");


                Session.Remove("item" + urlSessao());
                Session.Add("item" + urlSessao(), itemnf);
                //   carregarDadosItens(itemnf);
            }
            catch (Exception)
            {

                LblErroItem.Text = "Valor Inválido";
                LblErroItem.ForeColor = System.Drawing.Color.Red;
                ((TextBox)sender).BackColor = System.Drawing.Color.Red;
                ((TextBox)sender).Focus();
            }
            ModalItens.Show();

        }
        protected void BotaoXml()
        {
            btnProximo.Visible = true;
            lblCancelarNota.Visible = false;
            ImgBtnCancelarNota.Visible = false;
            ImgBtnCartaDeCorrecao.Visible = false;
            lblCartaCorrecaoText.Visible = false;
            divSituacao.Visible = false; //Não será mais utilizado.
            lblProximoXml.Visible = true;
            divVisualizaXML.Visible = false;
            visualizarConfirmacao(false);
            visualizarCorrecao(false);
            btnVisualizarCorrecao.Visible = false;
            nfDAO nf = (nfDAO)Session["obj" + urlSessao()];


            if (nf != null)
            {
                xmlNFE xml = new xmlNFE(nf);
                switch (nf.status)
                {
                    case "DIGITACAO":
                        lblProximoXml.Text = "VALIDAR";
                        break;
                    case "VALIDADO":
                        lblProximoXml.Text = "TRANSMITIR";
                        divVisualizaXML.Visible = true;
                        break;
                    case "TRANSMITIDO":
                        lblProximoXml.Text = "SITUACAO";
                        divVisualizaXML.Visible = true;
                        break;
                    case "AUTORIZADO":
                        lblProximoXml.Text = "AUTORIZADO";
                        btnProximo.Visible = false;
                        lblCancelarNota.Visible = true;
                        ImgBtnCancelarNota.Visible = true;
                        ImgBtnCartaDeCorrecao.Visible = true;
                        lblCartaCorrecaoText.Visible = true;
                        String ultimaCorrecao = nf.UltimaCorrecaoRegistrada;
                        lblResultadoXML.Text = "Nota fiscal Autorizada com o ID= " + nf.id + "<br/>" + ultimaCorrecao;
                        if (ultimaCorrecao.Length > 0)
                            btnVisualizarCorrecao.Visible = true;


                        lblResultadoXML.ForeColor = System.Drawing.Color.Blue;
                        divSituacao.Visible = false;
                        divVisualizaXML.Visible = true;
                        break;
                    case "CANCELADA":
                        lblProximoXml.Text = "CANCELADA";
                        btnProximo.Visible = false;
                        lblCancelarNota.Visible = false;
                        ImgBtnCancelarNota.Visible = false;
                        ImgBtnCartaDeCorrecao.Visible = false;
                        lblCartaCorrecaoText.Visible = false;
                        divSituacao.Visible = false;
                        xmlNFE xml1 = new xmlNFE(nf);
                        try
                        {
                            lblResultadoXML.Text = xml1.respostaCancelamento();
                            lblResultadoXML.ForeColor = System.Drawing.Color.Blue;
                        }
                        catch (Exception err)
                        {

                            lblResultadoXML.Text = err.Message;
                            lblResultadoXML.ForeColor = System.Drawing.Color.Red;
                        }

                        break;
                }


            }
        }
        protected void gravarXml()
        {
            try
            {

                //Objeto para emissão da NFe
                nfDAO nf = (nfDAO)Session["obj"];
                xmlNFE xml = new xmlNFE(nf);
                String ResultadoXML = "";

                User usr = (User)Session["user"];
                NFCeOperacoes nNFe = new NFCeOperacoes(usr);

                bool soValidar = nf.status.ToUpper().Equals("DIGITACAO");
                var notaEmitida = nNFe.criarNFCe(nf, soValidar: soValidar);

                if (soValidar)
                {
                    nf.status = "VALIDADO";
                    nf.AtualizarStatus();

                    Session.Remove("obj");
                    Session.Add("obj", nf);

                    TimerXml.Enabled = false;
                    ResultadoXML = "NFe validada com sucesso!";
                    lblResultadoXML.Text = ResultadoXML;
                    lblResultadoXML.Visible = true;

                    Session.Remove("resultadoXml");
                    Session.Add("resultadoXml", ResultadoXML);

                    //Retorno.
                    String resultado = (String)Session["resultadoXml"];
                    modalXml.Hide();
                    modalXml.Show();
                    
                    return;
                }

                if (notaEmitida != null)
                {

                    ResultadoXML = "NFe " + notaEmitida.NFe.infNFe.Id + "\r\nAutorizada:" + notaEmitida.protNFe.infProt.nProt.ToString();

                    Session.Remove("resultadoXml");
                    Session.Add("resultadoXml", ResultadoXML);

                    //Retorno.
                    String resultado = (String)Session["resultadoXml"];


                    if (resultado != null)
                    {
                        if (resultado.Contains("Carta Correcao - "))
                            ExibirCorrecao();
                        lblResultadoXML.Visible = true;
                        lblResultadoXML.Text = resultado;
                        TimerXml.Enabled = true;
                        lblResultadoXML.ForeColor = System.Drawing.Color.Blue;

                        if (nf.status.Equals("AUTORIZADO"))
                        {
                            //Gravar NFe na tabela documentos eletronicos.

                            RedirectNovaAba("~/modulos/notafiscal/pages/DanfeReport.aspx?cliente_Fornecedor=" + nf.Cliente_Fornecedor + "&" +
                                                            "numero=" + nf.Codigo + "&" +
                                                            "tipoNf=" + nf.Tipo_NF + " &" +
                                                            "tipoOrigem=N&" +
                                                            "enviaEmail=true");
                        }
                        Session.Remove("resultadoXml");

                        carregarDados();

                    }
                }


                return;



                String aborta = (String)Session["aborta"];
                if (aborta != null)
                {
                    xml.abortarTransmissaoA3();
                    Session.Remove("aborta");

                }
                else
                {


                    switch (nf.status)
                    {
                        case "DIGITACAO":


                            bool validarArquivo = !nf.usr.filial.tipo_certificado.Equals("A3");
                            //xml.gravarArquivo(validarArquivo);
                            xml.gravarArquivo(true);
                            nf.status = "VALIDADO";

                            nf.AtualizarStatus();

                            Session.Remove("obj");
                            Session.Add("obj", nf);

                            lblResultadoXML.ForeColor = System.Drawing.Color.Blue;

                            Session.Remove("resultadoXml");
                            Session.Add("resultadoXml", "Validado com Sucesso");
                            break;
                        case "VALIDADO":

                            if (nf.usr.filial.tipo_certificado.Equals("A1"))
                            {
                                xml.transmitir();
                                ResultadoXML = xml.respostaEnvio();
                                nf.status = "TRANSMITIDO";
                                nf.AtualizarStatus();
                                Session.Remove("resultadoXml");
                                Session.Add("resultadoXml", ResultadoXML);
                            }
                            else
                            {
                                xml.limparErros();
                                xml.transmitir();
                                int tentativas = 0;
                                while (tentativas < 50)
                                {
                                    System.Threading.Thread.Sleep(1000);
                                    ResultadoXML = xml.respostaEnvio();
                                    if (ResultadoXML.Equals("Sem Resposta"))
                                        tentativas++;
                                    else
                                        break;
                                }
                                if (ResultadoXML.Contains("Sem Resposta"))
                                {
                                    xml.abortarTransmissaoA3();
                                    throw new Exception("Sem Respota");
                                }
                                //Checa se hão houve a criação do protocolo.
                                if (ResultadoXML.IndexOf("Autorizado") >= 0 && ResultadoXML.IndexOf("Protocolo") >= 0)
                                {
                                    nf.status = "AUTORIZADO";
                                    nf.Emissao = DateTime.Today;

                                    nf.numeroProtocolo = ResultadoXML.Substring(ResultadoXML.IndexOf("Protocolo :") + 11).Replace("<br/>", "");
                                    if (nf.Data <= nf.Emissao)
                                    {
                                        nf.Data = nf.Emissao;
                                    }
                                }
                                else
                                {
                                    nf.status = "TRANSMITIDO";
                                }
                                nf.AtualizarStatus();
                                Session.Remove("resultadoXml");
                                Session.Add("resultadoXml", ResultadoXML);

                            }
                            break;
                        case "TRANSMITIDO":
                            xml.SituacaoTransmicao();
                            ResultadoXML = xml.respostaConsulta();
                            if (nf.numeroProtocolo.Equals(""))
                            {
                                nf.numeroProtocolo = ResultadoXML.Substring(ResultadoXML.IndexOf("Protocolo :") + 11);
                            }
                            nf.status = "AUTORIZADO";
                            nf.Emissao = xml.dtAutorizacao;
                            if (nf.Data <= nf.Emissao)
                            {
                                nf.Data = nf.Emissao;
                            }

                            nf.AtualizarStatus();
                            Session.Remove("resultadoXml");
                            Session.Add("resultadoXml", ResultadoXML);
                            break;
                    }



                }

            }
            catch (Exception err)
            {
                TimerXml.Enabled = false;
                Session.Remove("erroXml");
                Session.Add("erroXml", err.Message);
                erroBtnXml();
                modalXml.Show();
            }

        }

        protected void situacaonotafiscal()
        {
            try
            {
                Session.Remove("resultadoXml");

                String nomeObj = "obj" + urlSessao();
                nfDAO nf = (nfDAO)Session[nomeObj];

                xmlNFE xml = new xmlNFE(nf);

                String ResultadoXML = "";

                ResultadoXML = "CONSULTA_" + xml.situacaoNotafiscal();
                lblResultadoXML.Text = ResultadoXML;
                if (ResultadoXML.IndexOf("Status:217") >= 0)
                {
                    btnProximo.Visible = true;
                    throw new Exception(ResultadoXML);
                }
                else if (ResultadoXML.IndexOf("Status:100") >= 0 || ResultadoXML.IndexOf("Status:150") >= 0) //Uso autorizado ou Uso autorizado fora de prazo.
                {
                    if (!nf.status.Equals("AUTORIZADO"))
                    {
                        nf.status = "AUTORIZADO";
                        nf.numeroProtocolo = ResultadoXML.Substring(ResultadoXML.IndexOf("Protocolo:") + 10);
                        nf.Emissao = xml.dtAutorizacao;
                        nf.AtualizarStatus();
                    }
                    else
                    {
                        if (nf.numeroProtocolo.Equals(""))
                        {
                            nf.numeroProtocolo = ResultadoXML.Substring(ResultadoXML.IndexOf("Protocolo:") + 10);
                            Conexao.executarSql("update nf set numero_protocolo = '" + nf.numeroProtocolo + "' where nf.codigo='" + nf.Codigo + "' and tipo_nf=" + nf.Tipo_NF + " and nf.cliente_fornecedor='" + nf.Cliente_Fornecedor + "' and nf.filial='" + nf.Filial + "'");
                        }
                    }

                }
                else if (ResultadoXML.IndexOf("Status:101") >= 0)
                {
                    switch (nf.status)
                    {
                        case "AUTORIZADO":
                            nf.status = "CANCELADA";
                            nf.nf_Canc = true;
                            nf.numeroProtocolo = ResultadoXML.Substring(ResultadoXML.IndexOf("Protocolo:") + 10);
                            nf.salvar(false);
                            break;
                        default:
                            nf.status = "CANCELADA";
                            nf.nf_Canc = true;
                            nf.AtualizarStatus();
                            break;
                    }
                }
                Session.Add("resultadoXml", ResultadoXML);
            }
            catch (Exception err)
            {
                Session.Remove("erroXml");
                Session.Add("erroXml", err.Message);

            }
        }


        protected void cancelarxmlnota()
        {
            try
            {

                String nomeObj = "obj" + urlSessao();
                nfDAO nf = (nfDAO)Session[nomeObj];
                xmlNFE xml = new xmlNFE(nf);
                xml.CancelarNFE(txtJustificativa.Text);
                String ResultadoXML = "";
                int cont = 0;
                while (cont < 50)
                {
                    try
                    {

                        System.Threading.Thread.Sleep(2000);
                        ResultadoXML = xml.respostaCancelamento();

                        break;
                    }
                    catch (Exception err)
                    {
                        int i = err.Message.IndexOf("Erro-Cancelar");
                        if (err.Message.IndexOf("Erro-Cancelar") >= 0)
                            throw err;

                        cont++;

                        if (cont >= 50)
                            throw err;
                        else
                            lblResultadoXML.Text = "Aguarde....";
                    }
                }



                nf.status = "CANCELADA";
                nf.nf_Canc = true;
                nf.salvar(false);
                lblResultadoXML.Text = ResultadoXML;
                Session.Remove("resultadoXml");
                Session.Add("resultadoXml", ResultadoXML);

                Session.Remove(nomeObj);
                Session.Add(nomeObj, nf);
                carregarDados();
                BotaoXml();



            }
            catch (Exception err)
            {

                Session.Remove("erroXml");
                Session.Add("erroXml", err.Message);
                erroBtnXml();
            }

        }

        protected void correcaoxmlnota()
        {
            try
            {

                String nomeObj = "obj" + urlSessao();
                nfDAO nf = (nfDAO)Session[nomeObj];
                String nCorrecao = Conexao.retornaUmValor("select max(seq) from nfe_correcao where codigo ='" + nf.Codigo + "' and filial='" + nf.usr.getFilial() + "'", null);
                if (nCorrecao.Equals("") || nCorrecao.Length > 10)
                    nCorrecao = "1";
                else
                    nCorrecao = (int.Parse(nCorrecao) + 1).ToString();

                User usr = (User)Session["user"];
                NFCeOperacoes nNFe = new NFCeOperacoes(usr);

                var retorno = nNFe.CartaCorrecaoNFe(1, int.Parse(nCorrecao), nf.id, txtCorrecao.Text, usr.filial.CNPJ);

                if (retorno.Retorno.cStat == 128)
                {
                    lblResultadoXML.Text = "Efetuado com sucesso. " + retorno.Retorno.xMotivo;
                    lblResultadoXML.Visible = true;
                }
                else
                {
                    lblResultadoXML.Text = "Falha. " + retorno.Retorno.xMotivo;
                    lblResultadoXML.Visible = true;
                }

                return;

                xmlNFE xml = new xmlNFE(nf);

                xml.CorrecaoNFE(txtCorrecao.Text, nCorrecao);
                String ResultadoXML = "";
                int cont = 0;
                while (cont < 50)
                {
                    try
                    {

                        System.Threading.Thread.Sleep(2000);

                        ResultadoXML = "Carta Correcao - " + xml.respostaCartaCorrecao(nCorrecao, txtCorrecao.Text);

                        break;
                    }
                    catch (Exception err)
                    {
                        int i = err.Message.IndexOf("Erro-Correcao");
                        if (err.Message.IndexOf("Erro-Correcao") >= 0)
                            throw err;

                        cont++;

                        if (cont >= 50)
                            throw err;
                        else
                            lblResultadoXML.Text = "Aguarde....";
                    }
                }



                lblResultadoXML.Text = ResultadoXML;
                lblResultadoXML.Visible = true;

                Session.Remove("resultadoXml");
                Session.Add("resultadoXml", ResultadoXML);

                Session.Remove(nomeObj);
                Session.Add(nomeObj, nf);

                // carregarDados();
                //BotaoXml();



            }
            catch (Exception err)
            {
                Session.Remove("erroXml");
                Session.Add("erroXml", err.Message);
                erroBtnXml();
            }

        }


        protected void btnXmlprocessar_Click(object sender, ImageClickEventArgs e)
        {
            nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
            //bool atualiar = false;
            try
            {
                Session.Remove("aborta");
                // xmlNFE xml = new xmlNFE(nf);
                TimerXml.Interval = 450;
                TimerXml.Enabled = true;
                divSituacao.Visible = false;
                Session.Remove("obj");
                Session.Add("obj", nf);
                //gravarXml();
                System.Threading.Thread th = new System.Threading.Thread(gravarXml);
                th.Start();
                switch (nf.status)
                {
                    case "DIGITACAO":
                        // xml.gravarArquivo(true);
                        lblProximoXml.Text = "Validando!!";
                        btnProximo.Visible = false;
                        lblResultadoXML.Text = "Aguarde a Validação da Nota";
                        lblResultadoXML.ForeColor = System.Drawing.Color.Green;
                        break;
                    case "VALIDADO":
                        lblProximoXml.Text = "Transmitindo!!";
                        btnProximo.Visible = false;
                        lblResultadoXML.Text = "Aguarde a Transmissão da Nota";
                        lblResultadoXML.ForeColor = System.Drawing.Color.Green;


                        break;
                    case "TRANSMITIDO":
                        lblProximoXml.Text = "Verificando!!";
                        btnProximo.Visible = false;
                        lblResultadoXML.Text = "Aguarde a Consulta da Nota";
                        lblResultadoXML.ForeColor = System.Drawing.Color.Green;
                        break;


                }
                /*
                if (atualiar)
                {
                    nf.AtualizarStatus();
                    Session.Remove("obj" + urlSessao());
                    Session.Add("obj" + urlSessao(), nf);
                    carregarDados();
                    lblResultadoXML.ForeColor = System.Drawing.Color.Blue;
                    BotaoXml();
                }
                 */
                modalXml.Show();
            }
            catch (Exception err)
            {
                if (!nf.usr.filial.tipo_certificado.Equals("A3"))
                {
                    switch (nf.status)
                    {
                        case "TRANSMITIDO":
                            nf.status = "DIGITACAO";
                            nf.AtualizarStatus();
                            Session.Remove("obj" + urlSessao());
                            Session.Add("obj" + urlSessao(), nf);
                            carregarDados();
                            BotaoXml();
                            break;
                    }
                }
                TimerXml.Enabled = false;
                lblResultadoXML.Text = err.Message;
                lblResultadoXML.ForeColor = System.Drawing.Color.Red;

                carregarDados();
                modalXml.Hide();

                modalXml.Show();
            }
        }


        protected void erroBtnXml()
        {
            btnProximo.Visible = false;
            lblProximoXml.Visible = false;
            ImgBtnCancelarNota.Visible = false;
            lblCancelarNota.Visible = false;
            ImgBtnCartaDeCorrecao.Visible = false;
            lblCartaCorrecaoText.Visible = false;
            imgBtnConfirmaCancelamento.Visible = false;
            lblConfirmaCancelamento.Visible = false;
            msgCorrecao.Visible = false;
            imgBtnConfirmaCorrecao.Visible = false;
            lblConfirmaCorrecao.Visible = false;
            divSituacao.Visible = true;
        }



        protected void btnXml_Click(object sender, EventArgs e)
        {
            lblResultadoXML.Text = "";
            lblResultadoXML.Visible = true;
            nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
            if (nf != null)
            {
                xmlNFE xml = new xmlNFE(nf);

                if (nf.status.Equals("VALIDADO"))
                {
                    try
                    {
                        //String situacao = xml.respostaEnvio();
                        //if (!situacao.Equals("Sem Resposta"))
                        //{
                        //    nf.status = "TRANSMITIDO";
                        //    nf.AtualizarStatus();
                        //    Session.Remove("obj" + urlSessao());
                        //    Session.Add("obj" + urlSessao(), nf);
                        //    lblResultadoXML.Text = situacao;

                        //}

                        BotaoXml();
                    }
                    catch (Exception err)
                    {

                        lblResultadoXML.Text = err.Message;
                        lblResultadoXML.ForeColor = System.Drawing.Color.Red;
                        erroBtnXml();
                    }
                }
                else
                {
                    BotaoXml();
                }

            }
            modalXml.Show();
        }


        protected void btnCancelarXml_Click(object sender, ImageClickEventArgs e)
        {
            modalXml.Hide();
            carregarDados();
            habilitarCampos(false);
            Session.Add("aborta", "");
            //TimerXml.Enabled = false;
        }


        protected void btnCancelarNota_Click(object sender, ImageClickEventArgs e)
        {
            User usr = (User)Session["user"];
            nfDAO obj = (nfDAO)Session["obj" + urlSessao()];
            //carregarDadosObj();
            if ((obj.status.ToString().Equals("AUTORIZADO") || obj.status.ToString().Equals("AUTORIZADA")) && obj.Emissao <= usr.filial.dtFechamentoEstoque)
            {
                showMessage("Data de emissão da NFe não permite o cancelamento. Data de emissão [" + obj.Emissao.ToString("dd/MM/yyyy") + "] não pode ser menor ou igual a data de fechamento do estoque [" + usr.filial.dtFechamentoEstoque.ToString("dd-MM-yyyy") + "]", true);
                return;
            }
            else
            {
                obj = null;
            }

            visualizarConfirmacao(true);
            ImgBtnCancelarNota.Visible = false;
            lblCancelarNota.Visible = false;
            modalXml.Show();
        }

        protected void visualizarConfirmacao(bool visivel)
        {
            lblResultadoXML.Visible = !visivel;
            lblJustificativa.Visible = visivel;
            txtJustificativa.Visible = visivel;
            lblConfirmaCancelamento.Visible = visivel;
            imgBtnConfirmaCancelamento.Visible = visivel;
            lblCorrecao.Visible = false;
            txtCorrecao.Visible = false;
            divSituacao.Visible = !visivel;
            ImgBtnCartaDeCorrecao.Visible = false;
            msgCorrecao.Visible = false;
            if (visivel)
            {
                imgBtnConfirmaCorrecao.Visible = false;
                lblConfirmaCorrecao.Visible = false;
                lblCartaCorrecaoText.Visible = false;
            }
        }

        protected void visualizarCorrecao(bool visivel)
        {
            lblResultadoXML.Visible = !visivel;
            lblJustificativa.Visible = false;
            txtJustificativa.Visible = false;
            lblConfirmaCancelamento.Visible = false;
            imgBtnConfirmaCancelamento.Visible = false;
            lblCorrecao.Visible = visivel;
            divSituacao.Visible = !visivel;
            txtCorrecao.Visible = visivel;
            msgCorrecao.Visible = visivel;
            imgBtnConfirmaCorrecao.Visible = visivel;
            lblConfirmaCorrecao.Visible = visivel;
            if (visivel)
            {
                ImgBtnCancelarNota.Visible = false;
                lblCancelarNota.Visible = false;

            }

        }

        protected void imgBtnConfirmaCancelamento_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                if (!txtJustificativa.Text.Trim().Equals("") && txtJustificativa.Text.Trim().Length >= 15 && txtJustificativa.Text.Trim().Length <= 255)
                {
                    visualizarConfirmacao(false);
                    visualizarCorrecao(false);

                    //Novo cancelamento.
                    //Objeto para emissão da NFe
                    nfDAO nf = (nfDAO)Session["obj" +urlSessao()];
                    String ResultadoXML = "";

                    User usr = (User)Session["user"];
                    NFCeOperacoes nNFe = new NFCeOperacoes(usr);

                    string numeroProtocolo = nf.numeroProtocolo.Split(' ')[0];

                    var retornoCancelamento = nNFe.CancelarNFe(usr.filial.CNPJ, 1, 1, nf.id, numeroProtocolo, txtJustificativa.Text);

                    if (retornoCancelamento.Retorno.retEvento.Count > 0)
                    {
                        if (retornoCancelamento.Retorno.retEvento[0].infEvento.cStat.ToString() == "135")
                        {
                            ResultadoXML = retornoCancelamento.Retorno.retEvento[0].infEvento.xEvento.ToString() + "\r\n";
                            ResultadoXML += retornoCancelamento.Retorno.retEvento[0].infEvento.xMotivo.ToString() ;
                            //Cancelamento OK.  
                            nf.status = "CANCELADA";
                            nf.nf_Canc = true;
                            nf.salvar(false);
                            lblResultadoXML.Text = ResultadoXML;
                        }
                        else
                        {
                            throw new Exception("Erro. " + retornoCancelamento.Retorno.retEvento[0].infEvento.cStat.ToString());
                        }

                    }
                    Session.Remove("resultadoXml");
                    Session.Add("resultadoXml", ResultadoXML);

                    //Session.Remove(nomeObj);
                    //Session.Add(nomeObj, nf);
                    carregarDados();

                    //Fim do novo


                    //Session.Remove("aborta");
                    //TimerXml.Interval = 450;
                    //TimerXml.Enabled = true;

                    //System.Threading.Thread th = new System.Threading.Thread(cancelarxmlnota);
                    //th.Start();
                    imgBtnConfirmaCancelamento.Visible = false;

                }
                else
                {
                    lblResultadoXML.Text = "Informe uma Justificativa Valida de no minino 15 caracters e no Maximo 255!";
                    lblResultadoXML.ForeColor = System.Drawing.Color.Red;
                    lblResultadoXML.Visible = true;
                }
            }
            catch (Exception err)
            {
                lblResultadoXML.Visible = true;
                lblResultadoXML.Text = err.Message;
                lblResultadoXML.ForeColor = System.Drawing.Color.Red;

                imgBtnConfirmaCancelamento.Visible = true;
            }

            modalXml.Show();
        }

        protected void TxtPesquisaLista_TextChanged(object sender, EventArgs e)
        {
            exibeLista();
            modalLista.Show();
        }

        protected void TimerXml_Tick(object sender, EventArgs e)
        {
            String resultado = (String)Session["resultadoXml"];
            String error = (String)Session["erroXml"];
            String aborta = (String)Session["aborta"];


            if (resultado != null)
            {
                if (resultado.Contains("Carta Correcao - "))
                    ExibirCorrecao();
                lblResultadoXML.Visible = true;
                lblResultadoXML.Text = resultado;
                TimerXml.Enabled = false;
                lblResultadoXML.ForeColor = System.Drawing.Color.Blue;
                //BotaoXml();
                nfDAO nf = (nfDAO)Session["obj"];
                if (nf != null)
                {
                    if (nf.status.Equals("TRANSMITIDO"))
                    {
                        System.Threading.Thread.Sleep(2000);
                        TimerXml.Interval = 450;
                        TimerXml.Enabled = true;
                        //System.Threading.Thread th = new System.Threading.Thread(gravarXml);
                        //th.Start();

                    }
                    else
                    {
                        btnProximo.Visible = true;
                        Session.Remove("obj" + urlSessao());
                        Session.Add("obj" + urlSessao(), nf);
                        Session.Remove("obj");
                    }

                    if (nf.status.Equals("AUTORIZADO"))
                    {
                        //Gravar NFe na tabela documentos eletronicos.

                        RedirectNovaAba("~/modulos/notafiscal/pages/DanfeReport.aspx?cliente_Fornecedor=" + nf.Cliente_Fornecedor + "&" +
                                                       "numero=" + nf.Codigo + "&" +
                                                       "tipoNf=" + nf.Tipo_NF + " &" +
                                                       "tipoOrigem=N&" +
                                                       "enviaEmail=true");
                    }
                }
                Session.Remove("resultadoXml");

                carregarDados();

            }
            else if (error != null)
            {

                erroBtnXml();
                lblResultadoXML.Visible = true;

                if (error.Contains("http://www.portalfiscal.inf.br/nfe:"))
                {
                    error = error.Substring(error.IndexOf("http://www.portalfiscal.inf.br/nfe:") + 35);
                    error = error.Substring(0, error.IndexOf("...Final"));
                    error = error.Replace("http://www.portalfiscal.inf.br/nfe", "");
                    error = error.Replace("Ã©", "é")
                                 .Replace("Ã¡", "á")
                                 .Replace("Ã§Ã£", "çã");
                }

                lblResultadoXML.Text = error;
                TimerXml.Enabled = false;

                lblResultadoXML.ForeColor = System.Drawing.Color.Red;
                Session.Remove("erroXml");
                nfDAO nf = (nfDAO)Session["obj"];
                if (nf != null)
                {
                    switch (nf.status)
                    {
                        case "TRANSMITIDO":
                            nf.status = "DIGITACAO";
                            nf.AtualizarStatus();
                            Session.Remove("obj" + urlSessao());
                            Session.Add("obj" + urlSessao(), nf);
                            Session.Remove("obj");
                            carregarDados();

                            break;
                    }
                }

            }
            else
            {
                lblResultadoXML.Visible = true;
                lblResultadoXML.Text = " Aguarde ....";
                lblResultadoXML.ForeColor = System.Drawing.Color.Green;

                btnProximo.Visible = false;
                imgBtnConfirmaCancelamento.Visible = false;

            }
            modalXml.Show();

            //if (aborta != null)
            //{
            //    TimerXml.Enabled = false;
            //    System.Threading.Thread th = new System.Threading.Thread(gravarXml);
            //    th.Start();
            //    modalXml.Hide();

            //}
        }

        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                nfDAO obj = (nfDAO)Session["obj" + urlSessao()];

                if (!obj.Tipo_NF.Equals("3"))
                {
                    if (obj.status == "VALIDADO" || obj.status == "VALIDADA")
                    {
                        if (!obj.StatusConsultadoAntesDaExclusao)
                        {
                            lblError.Text = "Antes de executar esta operação o usuário deverá checar o STATUS da NFe no SEFAZ. Clicar na opção CONSULTA SITUAÇÃO através do BOTÃO XML.";
                            carregabtn(pnBtn);
                            carregarDados();

                            return;
                        }
                    }
                }



                obj.excluir();
                modalPnConfirma.Hide();
                lblError.Text = "Registro Excluido com sucesso";
                limparCampos();
                status = "pesquisar";
                carregabtn(pnBtn);
                Session.Remove("obj" + urlSessao());
            }
            catch (Exception err)
            {

                lblError.Text = "Não foi possivel Excluir a Nota pelo error:" + err.Message;

                carregabtn(pnBtn);
                carregarDados();
            }
        }
        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            modalPnConfirma.Hide();
            carregarDados();
        }

        protected void txtCalcula_TextChanged(object sender, EventArgs e)
        {
            carregarDadosObj();
            carregarDados();
        }


        protected void imgBtnObservacoesPadrao_Click(object sender, ImageClickEventArgs e)
        {
            modalObservacoes.Show();
            carregarObservacoes();
        }
        protected void carregarObservacoes()
        {
            GridObservacao.DataSource = Conexao.GetTable("select cod,Observacao=SUBSTRING(descricao,0,50) from Obs_nota", null, false);
            GridObservacao.DataBind();
        }

        protected void imgBtnConfirmaObservacoes_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow linha in GridObservacao.Rows)
            {
                CheckBox rdo = (CheckBox)linha.FindControl("chkSelecionado");
                if (rdo.Checked)
                {
                    txtObservacao.Text += Conexao.retornaUmValor("select descricao from obs_nota where cod='" + linha.Cells[1].Text + "'", null) + "\n";
                }

            }

            imgBtnAddObservacoes.Visible = true;
            lblObservacao.Visible = true;
            pnNovaObservacao.Visible = false;
            modalObservacoes.Hide();
        }

        protected void imgBtnCancelarObservacoes_Click(object sender, ImageClickEventArgs e)
        {
            imgBtnAddObservacoes.Visible = true;
            lblObservacao.Visible = true;
            pnNovaObservacao.Visible = false;
            modalObservacoes.Hide();
        }

        protected void imgBtnAddObservacoes_Click(object sender, ImageClickEventArgs e)
        {
            pnNovaObservacao.Visible = true;
            lblcod.Text = "";
            txtObservacaoAdd.Text = "";

            txtObservacaoAdd.Focus();
            imgBtnAddObservacoes.Visible = false;
            lblObservacao.Visible = false;
            modalObservacoes.Show();
        }
        protected void imgConfirmaAddObservacao_Click(object sender, ImageClickEventArgs e)
        {
            if (!txtObservacaoAdd.Text.Equals(""))
            {
                if (lblcod.Text.Equals(""))
                {
                    Conexao.executarSql("insert into Obs_nota (cod,descricao)values((select isnull(MAX(cod)+1,1) from Obs_nota),'" + txtObservacaoAdd.Text + "')");
                }
                else
                {
                    Conexao.executarSql("update obs_nota set descricao='" + txtObservacaoAdd.Text + "' where cod='" + lblcod.Text + "'");
                }
            }

            pnNovaObservacao.Visible = false;
            imgBtnAddObservacoes.Visible = true;
            lblObservacao.Visible = true;
            modalObservacoes.Show();
            carregarObservacoes();
        }

        protected void GridObservacao_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            String cod = GridObservacao.Rows[index].Cells[1].Text;
            txtObservacaoAdd.Text = Conexao.retornaUmValor("select descricao from obs_nota where cod='" + cod + "'", null);
            lblcod.Text = cod;
            pnNovaObservacao.Visible = true;
            modalObservacoes.Show();

        }

        protected void imgAddReferencia_Click(object sender, ImageClickEventArgs e)
        {
            nfDAO obj = (nfDAO)Session["obj" + urlSessao()];

            if (DDlRefECF.Text.Equals("NFE"))
            {
                if (!txtReferenciaNota.Text.Trim().Equals(""))
                {
                    obj.NfReferencias.Add(txtReferenciaNota.Text);
                    txtReferenciaNota.Text = "";
                    txtReferenciaNota.Focus();
                    Session.Remove("obj" + urlSessao());
                    Session.Add("obj" + urlSessao(), obj);
                    carregarGrids();
                }
            }
            else
            {
                txtECF.BackColor = System.Drawing.Color.White;
                txtCOO.BackColor = System.Drawing.Color.White;
                txtDataDocumento.BackColor = System.Drawing.Color.White;
                if (txtECF.Text.Trim().Equals(""))
                {
                    txtECF.BackColor = System.Drawing.Color.Red;
                }
                if (txtCOO.Text.Trim().Equals(""))
                {
                    txtCOO.BackColor = System.Drawing.Color.Red;
                }
                if (txtDataDocumento.Text.Trim().Equals("") || !IsDate(txtDataDocumento.Text))
                {
                    txtDataDocumento.BackColor = System.Drawing.Color.Red;
                }
                if (!txtECF.Text.Trim().Equals("") && !txtCOO.Text.Trim().Equals("") && !txtDataDocumento.Text.Equals("") && IsDate(txtDataDocumento.Text))
                {
                    obj.addECF(txtECF.Text, txtCOO.Text, DateTime.Parse(txtDataDocumento.Text));
                    txtECF.Text = "";
                    txtCOO.Text = "";
                    txtDataDocumento.Text = "";

                    Session.Remove("obj" + urlSessao());
                    Session.Add("obj" + urlSessao(), obj);
                    carregarGrids();
                }


            }
        }

        protected void gridNfReferencia_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            nfDAO obj = (nfDAO)Session["obj" + urlSessao()];

            if (DDlRefECF.Text.Equals("NFE"))
                obj.NfReferencias.RemoveAt(index);
            else
                obj.ECFReferencias.RemoveAt(index);

            Session.Remove("obj" + urlSessao());
            Session.Add("obj" + urlSessao(), obj);

            carregarGrids();
        }

        protected void ddlTipoDestinatario_TextChanged(object sender, EventArgs e)
        {
            exibeLista();
        }

        protected void DDlRefECF_SelectedIndexChanged(object sender, EventArgs e)
        {

            nfDAO obj = (nfDAO)Session["obj" + urlSessao()];

            if (obj != null)
            {
                Session.Remove("obj" + urlSessao());
                Session.Add("obj" + urlSessao(), obj);

                carregarGrids();

                if (DDlRefECF.Text.Equals("NFE"))
                {
                    obj.Ref_ECF = false;
                    DivECF.Visible = false;
                    DivReferenciaNota.Visible = true;
                    gridNfReferencia.DataSource = obj.NotasReferencias();
                    gridNfReferencia.DataBind();
                }
                else
                {
                    obj.Ref_ECF = true;
                    DivECF.Visible = true;
                    DivReferenciaNota.Visible = false;
                    gridNfReferencia.DataSource = obj.EcfReferencias();
                    gridNfReferencia.DataBind();

                }

            }
        }

        protected void imgBtnConfirmaCorrecao_Click(object sender, ImageClickEventArgs e)
        {

            try
            {

                if (!txtCorrecao.Text.Trim().Equals("") && txtCorrecao.Text.Trim().Length >= 15 && txtCorrecao.Text.Trim().Length <= 255)
                {
                    visualizarConfirmacao(false);
                    visualizarCorrecao(false);

                    Session.Remove("aborta");
                    TimerXml.Interval = 450;
                    TimerXml.Enabled = true;

                    System.Threading.Thread th = new System.Threading.Thread(correcaoxmlnota);

                    th.Start();
                    imgBtnConfirmaCancelamento.Visible = false;
                    imgBtnConfirmaCorrecao.Visible = false;

                }
                else
                {
                    msgCorrecao.Visible = false;
                    lblResultadoXML.Text = "Informe uma Correção Valida de no minino 15 caracters e no Maximo 255!";
                    lblResultadoXML.ForeColor = System.Drawing.Color.Red;
                    lblResultadoXML.Visible = true;
                }
            }
            catch (Exception err)
            {

                msgCorrecao.Visible = false;
                lblResultadoXML.Visible = true;
                lblResultadoXML.Text = err.Message;
                lblResultadoXML.ForeColor = System.Drawing.Color.Red;

                imgBtnConfirmaCancelamento.Visible = true;
                imgBtnConfirmaCorrecao.Visible = true;

            }

            modalXml.Show();
        }
        protected void ImgBtnCartaDeCorrecao_Click(object sender, ImageClickEventArgs e)
        {
            ImgBtnCartaDeCorrecao.Visible = false;
            lblCartaCorrecaoText.Visible = false;
            visualizarCorrecao(true);
            modalXml.Show();
        }

        protected void imgBtnConsultaSituacao_Click(object sender, ImageClickEventArgs e)
        {

            return;
            Session.Remove("aborta");
            Session.Remove("resultadoXml");

            //Grava no objeto se já foi efetuado a CONSULTA SITUAÇÃO da NFe
            nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
            nf.StatusConsultadoAntesDaExclusao = true;
            Session.Remove("obj" + urlSessao());
            Session.Add("obj" + urlSessao(), nf);

            TimerXml.Interval = 450;
            TimerXml.Enabled = true;

            System.Threading.Thread th = new System.Threading.Thread(situacaonotafiscal);

            th.Start();
            modalXml.Show();
        }

        protected void imgBtnConfirmaPedidoFechado_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                status = "incluir";
                nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
                nf.Tipo_NF = "1";
                nf.importarPedido(lblConfirmaPedidoFechado.Text, int.Parse(ddlTipoPedido.SelectedValue));
                nf.centro_custo = visualSysWeb.code.Funcoes.valorParametro("CENTRO_CUSTO_NF_SAIDA", new User());


                Session.Remove("obj" + urlSessao());
                Session.Add("obj" + urlSessao(), nf);
                carregarDados();
                if (nf.Cliente_Fornecedor.Equals(""))
                {
                    Session.Remove("campoLista" + urlSessao());
                    Session.Add("campoLista" + urlSessao(), "txtCliente_CNPJ");
                    TxtPesquisaLista.Text = "";
                    exibeLista();
                }
                else if (nf.Codigo_operacao.ToString().Equals("0"))
                {
                    Session.Remove("campoLista" + urlSessao());
                    Session.Add("campoLista" + urlSessao(), "txtCodigo_operacao");
                    TxtPesquisaLista.Text = "";
                    exibeLista();
                }


            }
            catch (Exception ERR)
            {

                Label19.Text = ERR.Message;
                modalConfirmaPedidoFechado.Show();
            }

        }
        protected void imgCancelaPedidoFechado_Click(object sender, ImageClickEventArgs e)
        {
            modalConfirmaPedidoFechado.Hide();
            ModalImportar.Show();
        }

        protected void btnConfirmaMov_Click(object sender, ImageClickEventArgs e)
        {
            try
            {


                modalOutrasMovimentacoes.Hide();
                status = "incluir";
                nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
                nf.Tipo_NF = "1";
                nf.Emissao = DateTime.Now;
                nf.Data = DateTime.Now;
                txtcentro_custo.Text = visualSysWeb.code.Funcoes.valorParametro("CENTRO_CUSTO_NF_SAIDA", new User());
                bool vSelecionado = false;
                foreach (GridViewRow linha in gridMovimentacoes.Rows)
                {
                    CheckBox chk = (CheckBox)linha.FindControl("chkItemMov");
                    if (chk.Checked)
                    {
                        vSelecionado = true;

                        nf.importarMovimentacao(linha.Cells[1].Text);
                    }

                }
                if (!vSelecionado)
                {
                    throw new Exception("Selecione uma Movimentação");
                }


                Session.Remove("obj" + urlSessao());
                Session.Add("obj" + urlSessao(), nf);
                carregarDados();
                if (nf.Cliente_Fornecedor.Equals(""))
                {
                    Session.Remove("campoLista" + urlSessao());
                    Session.Add("campoLista" + urlSessao(), "txtCliente_CNPJ");
                    TxtPesquisaLista.Text = "";
                    exibeLista();
                }
                else if (nf.Codigo_operacao.ToString().Equals("0"))
                {
                    Session.Remove("campoLista" + urlSessao());
                    Session.Add("campoLista" + urlSessao(), "txtCodigo_operacao");
                    TxtPesquisaLista.Text = "";
                    exibeLista();
                }
            }
            catch (Exception err)
            {
                divMsgImpMovimento.Visible = false;
                lblErroMovimentacao.Text = err.Message;
                modalOutrasMovimentacoes.Show();
            }

        }
        protected void btnCancelaMov_Click(object sender, ImageClickEventArgs e)
        {
            lblErroMovimentacao.Text = "";
            divMsgImpMovimento.Visible = true;
            modalOutrasMovimentacoes.Hide();
            ModalImportar.Show();
        }

        protected void chkSeleciona_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTd = (CheckBox)sender;
            foreach (GridViewRow linha in gridMovimentacoes.Rows)
            {
                CheckBox chk = (CheckBox)linha.FindControl("chkItemMov");
                chk.Checked = chkTd.Checked;

            }
            modalOutrasMovimentacoes.Show();
        }

        protected void imgBuscaArquivo_Click(object sender, ImageClickEventArgs e)
        {
            checaArquivosImportar();
            ModalImportar.Show();
        }

        protected void btnConfirmaColetor_Click(object sender, ImageClickEventArgs e)
        { }

        protected void btnCancelaColetor_Click(object sender, ImageClickEventArgs e)
        {
            ModalImportar.Show();
            modalImportColetor.Hide();
        }

        protected void btnAbrirColetor_Click(object sender, ImageClickEventArgs e)
        {
            ModalImportar.Hide();
            modalImportColetor.Show();
        }


        protected void checaArquivosImportar()
        {
            User usr = (User)Session["User"];
            if (usr != null)
            {
                //Limpar conteúdo do ListBox ltbArquivosParaImportacao
                ddlArquivos.Items.Clear();
                //Preenche ltbArquivosParaImportacao com conteudo do diretorio.
                String endereco = (usr.filial.ip.Equals("::1") ? "c:" : @"\\" + usr.filial.ip) + "\\Coletor";
                if (Funcoes.existePasta(endereco))
                {
                    string[] txtList = Directory.GetFiles(@endereco, "*.txt");
                    foreach (string f in txtList)
                    {
                        string strNomeArquivo = f.Substring(@endereco.Length + 1);
                        ddlArquivos.Items.Add(strNomeArquivo);
                    }

                    if (ddlArquivos.Items.Count <= 0)
                    {
                        ddlArquivos.Visible = false;

                        lblIdentificarArquivo.Visible = true;
                        lblIdentificarArquivo.Text = "Sem Arquivos Identificados na pasta " + endereco;

                    }
                    else
                    {
                        ddlArquivos.Visible = true;
                        lblIdentificarArquivo.Visible = false;
                    }

                }
                else
                {
                    ddlArquivos.Visible = false;

                    lblIdentificarArquivo.Visible = true;
                    lblIdentificarArquivo.Text = "Não Encontrado o diretorio:" + endereco;

                }

                modalImportColetor.Show();
            }
        }

        protected void imgLeArquivo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                status = "incluir";
                nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
                nf.Tipo_NF = "1";
                nf.Emissao = DateTime.Now;
                nf.Data = DateTime.Now;
                nf.centro_custo = visualSysWeb.code.Funcoes.valorParametro("CENTRO_CUSTO_NF_SAIDA", new User());

                Ler();
                modalImportColetor.Hide();

                Session.Remove("obj" + urlSessao());
                Session.Add("obj" + urlSessao(), nf);
                carregarDados();
                if (nf.Cliente_Fornecedor.Equals(""))
                {
                    Session.Remove("campoLista" + urlSessao());
                    Session.Add("campoLista" + urlSessao(), "txtCliente_CNPJ");
                    TxtPesquisaLista.Text = "";
                    exibeLista();
                }
                else if (nf.Codigo_operacao.ToString().Equals("0"))
                {
                    Session.Remove("campoLista" + urlSessao());
                    Session.Add("campoLista" + urlSessao(), "txtCodigo_operacao");
                    TxtPesquisaLista.Text = "";
                    exibeLista();
                }

            }
            catch (Exception err)
            {

                lblErroColetor.Text = "Erro na importacao do arquivo :" + err.Message;
                modalImportColetor.Show();
            }
        }
        private void Ler()
        {


            Coletor coletor = new Coletor();

            User usr = (User)Session["User"];
            String endereco = (usr.filial.ip.Equals("::1") ? "c:" : @"\\" + usr.filial.ip) + "\\Coletor";
            String arquivo = ddlArquivos.SelectedValue;

            String strSQL = "";
            arquivo = endereco + "\\" + arquivo;
            string[] lines = System.IO.File.ReadAllLines(arquivo);
            nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
            Hashtable tbPlus = new Hashtable();
            //int vdecimal = int.Parse(txtDecimaisContado.Text) + 1;
            String strPlus = "";



            foreach (string line in lines)
            {

                if (line.Length > 0)
                {
                    dynamic ObjItem = new ExpandoObject();

                    if (coletor.coletorFixo)
                    {
                        ObjItem.plu = line.Substring(coletor.pluInicio - 1, coletor.tamPlu);
                        ObjItem.contado = line.Substring(coletor.contadoInicio - 1, coletor.tamContado);
                        try
                        {
                            ObjItem.preco = line.Substring(coletor.precoInicio - 1, coletor.tamPreco);
                        }
                        catch (Exception)
                        {
                            ObjItem.preco = "0";

                        }
                    }
                    else
                    {
                        string[] arrLinha = line.Split(coletor.delimitador[0]);
                        ObjItem.plu = arrLinha[coletor.pluInicio - 1];
                        ObjItem.contado = arrLinha[coletor.contadoInicio - 1];
                        try
                        {
                            ObjItem.preco = arrLinha[coletor.precoInicio - 1];
                        }
                        catch (Exception)
                        {
                            ObjItem.preco = "0";
                        }
                    }
                    ObjItem.contado = (Funcoes.decTry(ObjItem.contado) / int.Parse("1".PadRight(coletor.contadoDecimal, '0'))).ToString();
                    ObjItem.preco = (Funcoes.decTry(ObjItem.preco) / int.Parse("1".PadRight(coletor.precoDecimal, '0'))).ToString();


                    if (!tbPlus.ContainsKey(ObjItem.plu.Trim()))
                    {
                        tbPlus.Add(ObjItem.plu.Trim(), ObjItem);
                    }
                    else
                    {
                        dynamic contPlu = tbPlus[ObjItem.plu.Trim()].ToString();
                        ObjItem.contado = (Funcoes.decTry(contPlu.contado) + Funcoes.decTry(ObjItem.contado)).ToString();
                        tbPlus[ObjItem.plu.Trim()] = ObjItem;
                    }

                    if (strPlus.Length > 0)
                        strPlus += ",";

                    strPlus += "'" + ObjItem.plu.Trim() + "'";
                }
            }

            nf.importaColetor(tbPlus, strPlus);

            if (!Funcoes.existePasta(endereco + "//importado"))
            {
                System.IO.Directory.CreateDirectory(endereco + "//importado");
            }

            System.IO.File.Move(arquivo, endereco + "//importado//" + DateTime.Now.ToString("yyyy.MM.dd.hh.mm") + ddlArquivos.SelectedValue);
            checaArquivosImportar();
        }
        protected void carregarDadosItensComplementar(nf_itemDAO item)
        {
            txtCompPlu.Text = item.PLU;
            txtCompReferencia.Text = item.CODIGO_REFERENCIA;
            txtCompDescricao.Text = item.Descricao;
            txtCompNCM.Text = item.NCM;
            txtCompBaseIpi.Text = item.baseIpi.ToString("N2");
            txtCompIpi.Text = item.porcIPI.ToString("N2");
            txtCompVlrIp.Text = item.vIpiv.ToString("N2");
            txtCompBaseIva.Text = item.vBaseIva.ToString("N2");
            txtCompIcmsIvaPorc.Text = item.vAliquota_iva.ToString("N2");
            txtCompMargemIva.Text = item.vmargemIva.ToString("N2");
            txtCompIva.Text = item.vIva.ToString("N2");
            txtCompBaseIcms.Text = item.vBaseICMS.ToString("N2");
            txtCompIcmsPorc.Text = item.vAliquota_icms.ToString("N2");
            txtCompValorIcms.Text = item.vicms.ToString("N2");
            txtCompCSTIcms.Text = item.vIndiceSt;
            txtCompRedutor.Text = item.redutor_base.ToString("N2");
            txtCompCfop.Text = item.codigo_operacao.ToString();
            txtCompPis.Text = item.PISV.ToString("N2");
            txtCompCstPis.Text = item.CSTPIS;
            txtCompCofins.Text = item.COFINSV.ToString();
            txtCompCstCofins.Text = item.CSTCOFINS;
            txtCompTotal.Text = item.vtotal.ToString("N2");
            txtCompCredSN.Text = item.pCredSN.ToString("N2");
            txtCompValorCredIcmsSN.Text = item.vCredicmssn.ToString("N2");
            ddlCompOrigemMercadoria.SelectedValue = item.vOrigem.ToString();
            txtCompItemNf.Text = item.Num_item.ToString();
            modalComplementar.Show();
        }
        protected void btnCancelaItemComplementar_Click(object sender, ImageClickEventArgs e)
        {
            carregarGrids();
            modalComplementar.Hide();
        }
        protected void ImgExcluiItemComplemento_Click(object sender, ImageClickEventArgs e)
        {
            nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
            nf.removeItem(nf.item(int.Parse(txtCompItemNf.Text) - 1));
            Session.Remove("obj" + urlSessao());
            Session.Add("obj" + urlSessao(), nf);
            PnExcluirItem.Visible = false;
            habilitarCampos(true);
            carregarDados();
        }
        protected void btnConfirmaItensComplementar_Click(object sender, ImageClickEventArgs e)
        {
            bool novo = false;
            User usr = (User)Session["User"];
            nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
            nf_itemDAO item = null;
            if (txtCompItemNf.Text.Equals(""))
            {
                item = new nf_itemDAO(usr);
                novo = true;
            }
            else
            {
                item = nf.item(int.Parse(txtCompItemNf.Text) - 1);
                novo = false;
            }
            item.Tipo_NF = nf.Tipo_NF;
            item.PLU = txtCompPlu.Text;

            item.CODIGO_REFERENCIA = txtCompReferencia.Text;
            item.Descricao = txtCompDescricao.Text;
            item.NCM = txtCompNCM.Text;
            Decimal.TryParse(txtCompBaseIpi.Text, out item.baseIpi);
            Decimal.TryParse(txtCompIpi.Text, out item.porcIPI);
            Decimal.TryParse(txtCompVlrIp.Text, out item.vIpiv);
            Decimal.TryParse(txtCompBaseIva.Text, out item.vBaseIva);
            Decimal.TryParse(txtCompIcmsIvaPorc.Text, out item.vAliquota_iva);
            Decimal.TryParse(txtCompMargemIva.Text, out item.vmargemIva);
            Decimal.TryParse(txtCompIva.Text, out item.vIva);
            Decimal.TryParse(txtCompBaseIcms.Text, out item.vBaseICMS);
            Decimal.TryParse(txtCompIcmsPorc.Text, out item.vAliquota_icms);
            Decimal.TryParse(txtCompValorIcms.Text, out item.vicms);
            item.vIndiceSt = txtCompCSTIcms.Text;
            Decimal.TryParse(txtCompRedutor.Text, out item.redutor_base);
            Decimal.TryParse(txtCompCfop.Text, out item.codigo_operacao);
            Decimal.TryParse(txtCompPis.Text, out item.PISV);
            item.CSTPIS = txtCompCstPis.Text;
            Decimal.TryParse(txtCompCofins.Text, out item.COFINSV);
            item.CSTCOFINS = txtCompCstCofins.Text;
            Decimal.TryParse(txtCompTotal.Text, out item.vtotal);
            Decimal.TryParse(txtCompCredSN.Text, out item.pCredSN);
            Decimal.TryParse(txtCompValorCredIcmsSN.Text, out item.vCredicmssn);
            int.TryParse(ddlCompOrigemMercadoria.SelectedValue, out item.vOrigem);
            item.Und = Conexao.retornaUmValor("Select Und from mercadoria where plu ='" + item.PLU + "'", null);


            if (novo)
            {
                nf.addItem(item);
            }
            else
            {
                nf.atualizaItem(item);
            }
            modalComplementar.Hide();
            //Session.Remove("obj" + urlSessao());
            //Session.Add("obj" + urlSessao(), nf);

            carregarDados();
        }

        protected void btnOkError_Click(object sender, EventArgs e)
        {
            if (lblErroPanel.Text.Contains("Não é permitido importar mais que 50 cupons"))
            {
                modalCupomVarios.Show();
            }
            if (lblErroPanel.Text.Contains("ItensImportaXmls erro"))
                modalXmlImportaDevolucao.Show();

            modalError.Hide();
            carregarGrids();
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

        protected void btnVisualizaXML_Click(object sender, EventArgs e)
        {
            nfDAO obj = (nfDAO)Session["obj" + urlSessao()];

            String urlXml = "";
            if (!obj.status.Equals("AUTORIZADO"))
            {
                //urlXml = obj.usr.filial.diretorio_exporta + "/SOLDI_XMLS/" + obj.id + ".xml";
                urlXml = obj.usr.filial.diretorio_exporta + "/NFe" + obj.id + ".xml";
            }
            else
            {
                //urlXml = obj.usr.filial.diretorio_exporta + "/Enviado/Autorizados/" + obj.Emissao.ToString("yyyyMM") + "/" + obj.id + "-procNFe.xml";
                urlXml = obj.usr.filial.diretorio_exporta + "/" + obj.Emissao.ToString("yyyy") + "/" + obj.Emissao.ToString("MM") + "/NFe" + obj.id + "-procNFe.xml";
            }

            String caminhoPasta = Server.MapPath("~/modulos/notafiscal/pages/uploads/" + obj.Emissao.ToString("yyyyMM"));
            String urlServer = caminhoPasta + "/" + obj.id + ".xml";
            String pathXml = "~/modulos/notafiscal/pages/uploads/" + obj.Emissao.ToString("yyyyMM") + "/" + obj.id + ".xml";

            if (File.Exists(urlServer))
            {
                File.Delete(urlServer);
            }
            if (!Directory.Exists(caminhoPasta))
            {
                Directory.CreateDirectory(caminhoPasta);
            }


            File.Copy(urlXml, urlServer);


            RedirectNovaAba(pathXml);
            modalXml.Show();
        }

        protected void btnAbrirXmsDevolucao_Click(object sender, EventArgs e)
        {

            divSemItens.Visible = true;
            gridItensXmlImportaDev.Visible = false;
            modalXmlImportaDevolucao.Show();
        }
        protected void imgBtnPesquisaImportaXmlDev_Click(object sender, EventArgs e)
        {
            try
            {
                User usr = (User)Session["User"];
                List<NfItemImportaDev> list = NfItemImportaDev.pesquisa(txtChaveDevImporta.Text, usr);
                gridItensXmlImportaDev.DataSource = list;
                gridItensXmlImportaDev.DataBind();
                Session.Remove("itemDevImp" + urlSessao());
                Session.Add("itemDevImp" + urlSessao(), list);


                divSemItens.Visible = false;
                gridItensXmlImportaDev.Visible = true;
                modalXmlImportaDevolucao.Show();
            }
            catch (Exception err)
            {
                if (err.Message.Contains("Sem Itens"))
                {
                    divSemItens.Visible = true;

                    gridItensXmlImportaDev.Visible = false;
                    modalXmlImportaDevolucao.Show();
                }

                else
                    showMessage(err.Message, true);

            }


        }
        protected void imgBtnConfirmaXmlImportaDev_Click(object sender, EventArgs e)
        {
            try
            {


                User usr = (User)Session["User"];
                nfDAO obj = (nfDAO)Session["obj" + urlSessao()];
                List<NfItemImportaDev> list = (List<NfItemImportaDev>)Session["itemDevImp" + urlSessao()];

                foreach (GridViewRow item in gridItensXmlImportaDev.Rows)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkItemDev");
                    if (!chk.Checked)
                    {
                        int nitem = Funcoes.intTry(item.Cells[6].Text);

                        var l = list.SingleOrDefault(x => x.item.Equals(nitem));
                        list.Remove(l);
                    }

                }

                obj.importarNFeDev(txtChaveDevImporta.Text, list);
                Session.Remove("obj" + urlSessao());
                Session.Add("obj" + urlSessao(), obj);
                carregarDados(false);
                modalXmlImportaDevolucao.Hide();

            }
            catch (Exception err)
            {

                showMessage("ItensImportaXmls erro:" + err.Message, true);
            }
        }
        protected void imgBtnCancelaXmlImportaDev_Click(object sender, EventArgs e)
        {
            modalXmlImportaDevolucao.Hide();
            ModalImportar.Show();
        }
        protected void chkSelTodosItensDev_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTd = (CheckBox)sender;
            foreach (GridViewRow linha in gridItensXmlImportaDev.Rows)
            {
                CheckBox chk = (CheckBox)linha.FindControl("chkItemDev");
                chk.Checked = chkTd.Checked;

            }
            modalXmlImportaDevolucao.Show();
        }


        protected void imgBtnPesquisaCupomVarios_Click(object sender, EventArgs e)
        {
            pesquisaVariosCupons();
        }

        protected void imgBtnConfirmaCupomVarios_Click(object sender, EventArgs e)
        {
            try
            {


                nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
                List<CupomImportar> imp = new List<CupomImportar>();

                foreach (GridViewRow item in gridCuponsVarios.Rows)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkItemCupom");
                    if (chk != null && chk.Checked)
                    {
                        CupomImportar cpItem = new CupomImportar();
                        cpItem.numero = item.Cells[2].Text;
                        cpItem.caixa = item.Cells[3].Text;
                        cpItem.dt = Funcoes.dtTry(item.Cells[4].Text);

                        var codigoCli = item.Cells[1].Text.ToString().Split('-')[0];

                        cpItem.cliente = codigoCli; // txtFiltroCupomVarios.Text.Replace(".","").Replace("/","").Replace("-","");
                        imp.Add(cpItem);
                    }

                }
                nf.importarCuponsVarios(imp);

                Session.Remove("obj" + urlSessao());
                Session.Add("obj" + urlSessao(), nf);
                carregarDados();
            }
            catch (Exception err)
            {

                showMessage(err.Message, true);
            }
            modalCupomVarios.Hide();
        }

        protected void imgBtnCancelaCupomVarios_Click(object sender, EventArgs e)
        {
            modalCupomVarios.Hide();
            ModalImportar.Show();
        }

        protected void chkSelTodosCupomVarios_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTd = (CheckBox)sender;
            foreach (GridViewRow item in gridCuponsVarios.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkItemCupom");
                if (chk != null)
                    chk.Checked = chkTd.Checked;
            }
            modalCupomVarios.Show();
            qtdCuponsSelecionados();
        }

        protected void qtdCuponsSelecionados()
        {
            int qtde = 0;
            foreach (GridViewRow item in gridCuponsVarios.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkItemCupom");
                if (chk.Checked)
                    qtde++;
            }
            lblQtdeSelecionados.Text = qtde.ToString() + " Selecionados";
            modalCupomVarios.Show();
        }

        protected void imgBtnImportarVariosCupons_Click(object sender, EventArgs e)
        {
            txtDataDeCupomVarios.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDataAteCupomVarios.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtFiltroCupomVarios.Text = "";
            pesquisaVariosCupons();
        }

        protected void pesquisaVariosCupons()
        {
            User usr = (User)Session["User"];

            Session.Remove("clienteVarios" + urlSessao());
            Session.Add("clienteVarios" + urlSessao(), txtFiltroCupomVarios.Text);

            DateTime dtDe = Funcoes.dtTry(txtDataDeCupomVarios.Text);
            DateTime dtAte = Funcoes.dtTry(txtDataAteCupomVarios.Text);
            String filtro = txtFiltroCupomVarios.Text;
            String sql = " select   documento  " +
                         "   , cliente = isnull(s.codigo_cliente + '-' + c.Nome_Cliente, '') " +
                         "   , caixa = s.caixa_saida " +
                         "   , dtMovimento = convert(varchar, s.data_movimento, 103) " +
                         "   , s.Hora_venda " +
                         "   ,valor = sum(s.vlr) " +
                         " from saida_estoque s WITH (INDEX=ix_saida_estoque)" +
                         "     LEFT OUTER JOIN tributacao d ON S.nro_ECF = d.Nro_ECF  and d.Saida_ICMS = s.Aliquota_ICMS " +
                         "     left join Cliente as c on s.Codigo_Cliente = c.Codigo_Cliente " +
                         " where s.Filial = '" + usr.filial.Filial + "'" +
                         " and s.Data_movimento between  '" + dtDe.ToString("yyyy-MM-dd") + "' and '" + dtAte.ToString("yyyy-MM-dd") + "' " +
            " and s.data_cancelamento is null " +
            " AND d.Indice_ST in ('00','40','41','60','101','102','500') ";
            if (filtro.Trim().Length > 0)
            {
                sql += " and (s.codigo_cliente = '" + filtro + "'" +
                              " or c.nome_cliente like '%" + filtro + "%'" +
                              " or replace(replace(replace(c.cnpj,'.',''),'-',''), '/','') like '" + filtro.Replace(".", "").Replace("-", "").Replace("/", "") + "'" +

                    ")";

            }
            sql += " GROUP BY Documento , s.Codigo_Cliente,Caixa_Saida ,s.Data_movimento,s.Hora_venda,c.Nome_Cliente " +
                  " order by convert(varchar, s.data_movimento,102),s.Hora_venda,s.caixa_saida,documento ";
            gridCuponsVarios.DataSource = Conexao.GetTable(sql, usr, false);
            gridCuponsVarios.DataBind();
            modalCupomVarios.Show();
            qtdCuponsSelecionados();

        }


        protected void imgBtnPesqClienteCupomVarios_Click(object sender, EventArgs e)
        {
            Session.Remove("campoLista" + urlSessao());
            Session.Add("campoLista" + urlSessao(), "txtFiltroCupomVarios");
            exibeLista();
        }
        protected void btnCorrecao_Click(object sender, EventArgs e)
        {
            ExibirCorrecao();
        }

        protected void imgBtnPesqClientePedidosVarios_Click(object sender, EventArgs e)
        {
            Session.Remove("campoLista" + urlSessao());
            Session.Add("campoLista" + urlSessao(), "txtFiltroPedidoVarios");
            exibeLista();
        }
        protected void imgBtnPesquisaPedidoVarios_Click(object sender, EventArgs e)
        {
            pesquisaVariosPedidos();
        }

        protected void imgBtnConfirmaPedidosVarios_Click(object sender, EventArgs e)
        {
            try
            {


                nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
                List<PedidoImporta> imp = new List<PedidoImporta>();

                foreach (GridViewRow item in gridVariosPedidos.Rows)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkItemPedido");
                    if (chk != null && chk.Checked)
                    {
                        PedidoImporta pdItem = new PedidoImporta();
                        pdItem.numeroPedido = item.Cells[2].Text;
                        pdItem.tipo = 1;
                        imp.Add(pdItem);
                    }
                }
                nf.importarVariosPedido(imp);
                Session.Remove("obj" + urlSessao());
                Session.Add("obj" + urlSessao(), nf);
                carregarDados();
            }
            catch (Exception err)
            {

                showMessage(err.Message, true);
            }


            modalImportarVariosPedidos.Hide();
        }
        protected void imgBtnCancelaPedidoVarios_Click(object sender, EventArgs e)
        {
            modalImportarVariosPedidos.Hide();
            ModalImportar.Show();
        }
        protected void chkSelTodosPedidosVarios_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTd = (CheckBox)sender;
            foreach (GridViewRow item in gridVariosPedidos.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkItemPedido");
                if (chk != null)
                    chk.Checked = chkTd.Checked;
            }
            modalImportarVariosPedidos.Show();
        }
        protected void imgBtnImportarVariosPedidos_Click(object sender, EventArgs e)
        {
            txtDataDePedidosVarios.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDataAtePedidosVarios.Text = DateTime.Now.ToString("dd/MM/yyyy");

            txtClienteVariosPedidos.Text = txtCliente_Fornecedor.Text;

            pesquisaVariosPedidos();
        }

        protected void pesquisaVariosPedidos()
        {
            User usr = (User)Session["User"];

            DateTime dtDe = Funcoes.dtTry(txtDataDePedidosVarios.Text);
            DateTime dtAte = Funcoes.dtTry(txtDataAtePedidosVarios.Text);

            String filtro = txtClienteVariosPedidos.Text;
            String sql = " Select  cliente = isnull(ltrim(rtrim(c.codigo_cliente)) + '-' + c.Nome_Cliente, '') " +
                            ",p.pedido " +
                            ",Data_cadastro  = convert(varchar,p.Data_cadastro,103) " +
                            ",p.Total " +
                    " from pedido as p inner join cliente as c on p.Cliente_Fornec = c.Codigo_Cliente " +
                    " where p.Tipo = 1 ";
            bool liberaPedidoFechado = Funcoes.valorParametro("LIBERA_PEDIDO_FECHADO", usr).ToUpper().Equals("TRUE");
            if (!liberaPedidoFechado)
                sql += "   and p.status = 1 ";
            else
                sql += " and p.status <> 3";

            sql += "   and " + DllDataPesquisa.SelectedValue.ToString() + " between  '" + dtDe.ToString("yyyy-MM-dd") + "' and '" + dtAte.ToString("yyyy-MM-dd") + "' ";
            if (filtro.Trim().Length > 0)
            {
                sql += " and (c.codigo_cliente = '" + filtro + "'" +
                              " or c.nome_cliente like '%" + filtro + "%'" +
                              " or replace(replace(replace(c.cnpj,'.',''),'-',''), '/','') like '" + filtro.Replace(".", "").Replace("-", "").Replace("/", "") + "'" +

                    ")";

            }
            sql += " order by convert(varchar, p.Data_cadastro,102), p.pedido desc ";

            gridVariosPedidos.DataSource = Conexao.GetTable(sql, usr, false);
            gridVariosPedidos.DataBind();
            modalImportarVariosPedidos.Show();

        }

        protected void chkSelItemCupomVarios_CheckedChanged(object sender, EventArgs e)
        {
            qtdCuponsSelecionados();
        }

        private void ExibirCorrecao()
        {
            try
            {
                nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
                StringBuilder Parametros = new StringBuilder();
                Parametros.Append("cliente_Fornecedor=" + nf.Cliente_Fornecedor);
                Parametros.Append("&numero=" + nf.Codigo);
                Parametros.Append("&NomePDF=CartaCorrecaoHtmlPdf");
                Parametros.Append("&Filial=" + nf.Filial);
                RedirectNovaAba("DanfeHtmlPdfSalvar.aspx?" + Parametros.ToString());
                modalXml.Show();
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void txtDesconto_TextChanged(object sender, EventArgs e)
        {
            try
            {


                if (txtDesconto.Text.Contains("%"))
                {
                    Decimal porc = Funcoes.decTry(txtDesconto.Text.Replace("%", ""));
                    if (porc > 99.99m)
                    {
                        txtDesconto.Text = "0";
                        throw new Exception("O Valor do Desconto não pode ser maior que 99,99%");
                    }
                    Decimal vTotal = Funcoes.decTry(txtTotalProdutos.Text);
                    Decimal valoDesc = (vTotal * porc) / 100;
                    txtDesconto.Text = valoDesc.ToString("N2");

                }
                carregarDadosObj();
                nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
                nf.rateiaDesconto(Funcoes.decTry(txtDesconto.Text));
                carregarDados();
            }
            catch (Exception err)
            {

                showMessage(err.Message, true);
                carregarDados();
            }
        }

        protected void ddlindFinal_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarDadosObj();
            carregarDados();
        }

        protected void ddlTipoImportacaoDev_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblTipoImportacaoDev.Text = ddlTipoImportacaoDev.SelectedItem.Value;
            modalXmlImportaDevolucao.Show();
        }
        protected void txtFrete_TextChanged(object sender, EventArgs e)
        {
            Decimal vlr = Funcoes.decTry(txtFrete.Text);
            nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
            nf.rateiaFrete(vlr);
            nf.calculaTotalItens();
            carregarDados();

        }

        protected void ddlIntermediador_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlIntermediador.SelectedItem.Value.Equals("1"))
            {
                ddlindPres.SelectedValue = "2";
            }
        }

        protected void imgBtnClienteImporta_Click(object sender, ImageClickEventArgs e)
        {
            Session.Remove("campoLista" + urlSessao());
            Session.Add("campoLista" + urlSessao(), "txtCodigoClienteImporta");
            exibeLista();

        }
    }
}