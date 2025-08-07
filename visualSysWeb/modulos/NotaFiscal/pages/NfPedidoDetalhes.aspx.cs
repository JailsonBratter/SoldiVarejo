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

namespace visualSysWeb.modulos.NotaFiscal.pages
{
    public partial class NfPedidoDetalhes : visualSysWeb.code.PagePadrao
    {


        protected void Page_Load(object sender, EventArgs e)
        {

            User usr = (User)Session["User"];
            if (usr == null)
            {
                return;
            }
            nfDAO obj = null;
            try
            {
                obj = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            }
            catch (Exception)
            {

                Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
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
                        obj = new nfDAO(usr, "3");
                        obj.Emissao = DateTime.Now;
                        obj.Data = DateTime.Now;
                        if (usr != null)
                        {
                            txtusuario.Text = usr.getNome();
                            obj.usuario = usr.getNome();
                        }
                        obj.Codigo_operacao = 5102;
                        if (Request.Params["pedidoImporta"] != null)
                        {
                            String nPedido = Request.Params["pedidoImporta"].ToString();
                            obj.importarPedido(nPedido, 1);

                        }

                        obj.centro_custo = Funcoes.valorParametro("CENTRO_CUSTO_NF_SAIDA", usr);

                        Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                        Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
                        carregarDados();
                        txtDtDeImp.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtDtAteImp.Text = DateTime.Now.ToString("dd/MM/yyyy");


                        carregarPedidosImportacao();


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
                            obj = new nfDAO(codigo, "3", cliente, usr);
                            if (obj.status.Equals("CANCELADA"))
                            {
                                status = "pesquisar";

                            }
                            else
                            {
                                status = "visualizar";
                            }

                            Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                            Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
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
            if (txtStatus.Text.Equals("AUTORIZADO")|| txtStatus.Text.Equals("CANCELADA"))
            {
                btnXml.Visible = false;
            }
            else
            {
                btnXml.Visible = !enable;
            }

            ImgDtEmissao.Visible = enable;
            ImgDeCalendario.Visible = enable;
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
            String[] campos = {
                                    "txtCliente_CNPJ",
                                    "txtEmissao",
                                    "txtData",
                                    "txtcentro_custo",
                                    "txtCodigo_operacao",
                                    "txtTipoFrete",
                                    "txtTransportadora",

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
                                   "txtDesconto",
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
                                   "txtCompItemNf"

                                     };
            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("NfPedidoDetalhes.aspx?novo=true");

        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            nfDAO obj = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            if (obj.status.Equals("TRANSMITIDO") || obj.status.Equals("AUTORIZADO") || obj.status.Equals("CANCELADA") || obj.status.Equals("INUTILIZADO"))
            {
                lblError.Text = "Não é permitido alterações na Nota Fiscal";
                status = "pesquisar";
                carregarDados();
                carregabtn(pnBtn, true);
                habilitarCampos(false);
            }
            else
            {
                habilitarCampos(true);
                status = "editar";
                carregarDados();
                carregabtn(pnBtn, true);
            }

        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("NfPedido.aspx"); //colocar o endereco da tela de pesquisa
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


                    nfDAO obj = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];


                    obj.status = "DIGITACAO";
                    carregarDadosObj();
                    Decimal tPG = obj.TotalPag();
                    Decimal dif = tPG - obj.Total;
                    if (obj.finNFe != 2 && (tPG == 0 || !dif.ToString("N2").Equals("0,00")))
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
            nfDAO obj = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            int pos = obj.Observacao.IndexOf("Valor do Imposto Aproximado");
            int tam = (obj.Observacao.IndexOf("IBPT") + 4) - pos;
            if (pos >= 0)
            {
                String imp = obj.Observacao.Substring(pos, tam);
                obj.Observacao = obj.Observacao.Replace(imp, "");
            }

            if (obj.vTotTrib > 0)
            {
                obj.Observacao += " Valor do Imposto Aproximado: R$ " + obj.vTotTrib.ToString("N2") + " Conforme Lei.12.741/12 fonte IBPT";
            }


            obj.salvar(status.Equals("incluir"));
            status = "visualizar";



            lblTotalPagamentos.Text = obj.TotalPag().ToString("N2");
            lblError.Text = "Salvo com Sucesso";
            lblError.ForeColor = System.Drawing.Color.Blue;




            Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
            carregabtn(pnBtn, true);
            carregarDados();
            habilitarCampos(false);
        }
        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("NfPedido.aspx");//colocar endereco pagina de pesquisa
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
        }
        //--Atualizar DaoForm 
        private void carregarDados()
        {
            String nomeObj = "obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "");
            nfDAO obj = (nfDAO)Session[nomeObj];
            txtCodigo.Text = obj.Codigo.ToString() + "";
            txtCliente_Fornecedor.Text = (obj.Cliente_Fornecedor != null ? obj.Cliente_Fornecedor.ToString() : "");
            TxtNomeCliente.Text = obj.Nome_cliente.ToString() + "";
            txtUfCliente.Text = obj.UfCliente.ToString();
            txtData.Text = obj.DataBr();
            txtCodigo_operacao.Text = (obj.Codigo_operacao.ToString().Equals("0") ? "" : obj.Codigo_operacao.ToString());
            txtEmissao.Text = obj.EmissaoBr();
            ddlFinalidade.SelectedValue = obj.finNFe.ToString();
            obj.calculaTotalItens();

            txtTotal.Text = string.Format("{0:0,0.00}", obj.Total);
            txtTotalProdutos.Text = obj.valorTotalProdutos.ToString("N2");
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
            txtQtde.Text = obj.qtde.ToString();
            txtEspecie.Text = obj.especie;
            txtMarca.Text = obj.marca;
            txtNumero.Text = obj.numero.ToString();
            txtPesoBrutoTransporte.Text = obj.peso_bruto.ToString();
            txtPesoLiquidoTransporte.Text = obj.peso_liquido.ToString();
            ddlTipoFrete.SelectedValue = obj.tipo_frete;


            txtTransportadora.Text = obj.nome_transportadora;
            txtQuantidade.Text = obj.qtde.ToString("N2");
            txtEspecie.Text = obj.especie;
            txtMarca.Text = obj.marca;
            txtNumero.Text = obj.numero.ToString("N2");
            txtPeso_Bruto.Text = obj.peso_bruto.ToString("N2");
            txtPeso_liquido.Text = obj.peso_liquido.ToString("N2");
            txtPlaca.Text = obj.Placa;
            ddlindPres.SelectedValue = obj.indPres.ToString();
            ddlindFinal.SelectedValue = obj.indFinal.ToString();
            DDlRefECF.Text = (obj.Ref_ECF ? "ECF" : "NFE");

            //txtReferenciaNota.Text = obj.nota_referencia.ToString();
            Session.Remove(nomeObj);
            Session.Add(nomeObj, obj);

            carregarGrids();
            btnXml.Visible = status.Equals("visualizar");
            BotaoXml();
        }


        public void carregarGrids()
        {
            try
            {


                nfDAO obj = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
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

                Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
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


                nfDAO obj = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                obj.Codigo = txtCodigo.Text;
                obj.Cliente_Fornecedor = txtCliente_Fornecedor.Text;
                obj.Tipo_NF = "3";
                obj.Data = DateTime.Parse(txtData.Text);
                obj.Codigo_operacao = Decimal.Parse((txtCodigo_operacao.Text.Equals("") ? "0" : txtCodigo_operacao.Text));
                obj.Emissao = DateTime.Parse(txtEmissao.Text);
                obj.Total = Decimal.Parse((txtTotal.Text.Equals("") ? "0" : txtTotal.Text));
                obj.Desconto = Decimal.Parse((txtDesconto.Text.Equals("") ? "0" : txtDesconto.Text));
                obj.Frete = Decimal.Parse((txtFrete.Text.Equals("") ? "0" : txtFrete.Text));
                obj.Seguro = Decimal.Parse((txtSeguro.Text.Equals("") ? "0" : txtSeguro.Text));
                obj.IPI_Nota = Decimal.Parse((txtIPI_Nota.Text.Equals("") ? "0" : txtIPI_Nota.Text));
                obj.Outras = Decimal.Parse((txtOutras.Text.Equals("") ? "0" : txtOutras.Text));
                obj.ICMS_Nota = Decimal.Parse((txtICMS_Nota.Text.Equals("") ? "0" : txtICMS_Nota.Text));
                obj.Base_Calculo = Decimal.Parse((txtBase_Calculo.Text.Equals("") ? "0" : txtBase_Calculo.Text));
                obj.Despesas_financeiras = Decimal.Parse((txtDespesas_financeiras.Text.Equals("") ? "0" : txtDespesas_financeiras.Text));
                obj.Pedido = txtPedido.Text;
                obj.Base_Calc_Subst = Decimal.Parse((txtBase_Calc_Subst.Text.Equals("") ? "0" : txtBase_Calc_Subst.Text));
                obj.Observacao = txtObservacao.Text;
                obj.nota_referencia = txtReferenciaNota.Text;

                //obj.nf_Canc = chknf_Canc.Checked;
                obj.centro_custo = txtcentro_custo.Text;
                obj.ICMS_ST = Decimal.Parse((txtICMS_ST.Text.Equals("") ? "0" : txtICMS_ST.Text));
                obj.Fornecedor_CNPJ = txtCliente_CNPJ.Text;
                obj.Desconto_geral = Decimal.Parse((txtDesconto_geral.Text.Equals("") ? "0" : txtDesconto_geral.Text));
                obj.usuario = txtusuario.Text;
                //============================================
                //transporte
                //============================================
                obj.nome_transportadora = txtTransportadora.Text;
                obj.qtde = (txtQuantidade.Text.Trim().Equals("") ? 1 : decimal.Parse(txtQuantidade.Text));
                obj.especie = (txtEspecie.Text.Trim().Equals("") ? "0" : txtEspecie.Text);
                obj.marca = (txtMarca.Text.Trim().Equals("") ? "0" : txtMarca.Text);
                obj.numero = (txtNumero.Text.Trim().Equals("") ? 0 : decimal.Parse(txtNumero.Text));
                obj.peso_bruto = (txtPesoBrutoTransporte.Text.Trim().Equals("") ? 0 : decimal.Parse(txtPesoBrutoTransporte.Text));
                obj.peso_liquido = (txtPesoLiquidoTransporte.Text.Trim().Equals("") ? 0 : decimal.Parse(txtPesoLiquidoTransporte.Text));
                obj.tipo_frete = ddlTipoFrete.SelectedValue;
                obj.Placa = txtPlaca.Text;

                //=======informacoes Adicionais Cliente 
                obj.indFinal = int.Parse(ddlindFinal.SelectedValue);
                obj.indPres = int.Parse(ddlindPres.SelectedValue);

                int.TryParse(ddlFinalidade.SelectedValue, out obj.finNFe);

                Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));

                Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
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
                        e.Row.Cells[i].Text = Decimal.Parse(e.Row.Cells[i].Text).ToString("N2");

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
                nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
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

            nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
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


        }

        protected void ImgAddNovoCupom_Click(object sender, ImageClickEventArgs e)
        {
            lblErrorPedido.Text = "";
            txtNumeroPedido.Text = "";

            carregarDadosObj();
            ModalImportar.Show();

        }

        protected void ImgBtnAddItens_Click(object sender, ImageClickEventArgs e)
        {
            if (!txtCliente_Fornecedor.Text.Equals(""))
            {
                if (ddlFinalidade.SelectedValue != "2")
                {
                    lblError.Text = "";
                    String or = "txtPLU";

                    TxtPesquisaLista.Text = "";
                    carregarDadosObj();
                    Session.Remove("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), or);
                    LimparCampos(pnItens);
                    exibeLista();
                    PnExcluirItem.Visible = false;
                }
                else
                {

                    carregarDadosObj();
                    LimparCampos(pnItemComplementar);
                    EnabledControls(pnItemComplementar, true);
                    modalComplementar.Show();
                }
            }
            else
            {
                lblError.Text = "Inclua as Informações de Cliente";
            }



        }

        protected nf_itemDAO carregaItem()
        {
            User usr = (User)Session["user"];
            nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            nf_itemDAO itemnf = (nf_itemDAO)Session["item" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            if (itemnf == null)
                itemnf = new nf_itemDAO(usr);


            itemnf.Tipo_NF = nf.Tipo_NF;
            itemnf.Codigo = txtCodigo.Text;
            itemnf.Cliente_Fornecedor = txtCliente_Fornecedor.Text;
            itemnf.PLU = txtPLU.Text;
            itemnf.CODIGO_REFERENCIA = txtCODIGO_REFERENCIA.Text;
            itemnf.Descricao = txtDescricao.Text;
            itemnf.naturezaOperacao = nf.NtOperacao;
            itemnf.codigo_operacao = Decimal.Parse((txtCodigo_operacao_item.Text.Equals("") ? "0,00" : txtCodigo_operacao_item.Text));

            itemnf.Unitario = Decimal.Parse(txtUnitario.Text);

            itemnf.Embalagem = Decimal.Parse(txtEmbalagem.Text);




            // itemnf.Total = (TxtTotalItem.Text.Equals("")?0: Decimal.Parse(TxtTotalItem.Text));
            itemnf.Qtde = Decimal.Parse(txtQtde.Text);



            itemnf.Codigo_Tributacao = Decimal.Parse(txtCodigo_Tributacao.Text);
            itemnf.Desconto = Decimal.Parse(txtDescontoItem.Text);
            itemnf.despesas = Decimal.Parse((txtdespesas.Text.Equals("") ? "0,00" : txtdespesas.Text));
            itemnf.aliquota_icms = Decimal.Parse((txtaliquota_icms.Text.Equals("") ? "0" : txtaliquota_icms.Text));
            itemnf.IPI = Decimal.Parse((txtIPI.Text.Equals("") ? "0,00" : txtIPI.Text));

            itemnf.IPIV = Decimal.Parse((txtIPIV.Text.Equals("") ? "0,00" : txtIPIV.Text));
            itemnf.vmargemIva = Decimal.Parse((txtmargem_iva.Text.Equals("") ? "0,00" : txtmargem_iva.Text));
            itemnf.vAliquota_iva = Decimal.Parse((txtAliquota_iva.Text.Equals("") ? "0,00" : txtAliquota_iva.Text));
            itemnf.indice_St = txtindice_st.Text;
            itemnf.vIva = Decimal.Parse((txtiva.Text.Equals("") ? "0,00" : txtiva.Text));
            itemnf.redutor_base = Decimal.Parse((txtredutor_base.Text).Equals("") ? "0,00" : txtredutor_base.Text);
            //itemnf.PISV = Decimal.Parse((txtPisItem.Text.Equals("")?"0,00":txtPisItem.Text));
            //itemnf.COFINSV = Decimal.Parse((txtCofinsItem.Text.Equals("")?"0,00":txtCofinsItem.Text));
            itemnf.Num_item = int.Parse((txtNum_item.Text.Equals("") ? "0" : txtNum_item.Text));



            itemnf.NCM = (txtNCM.Text.Equals("") ? "0" : txtNCM.Text);
            itemnf.CEST = txtCEST.Text;
            itemnf.Und = txtUnd.Text;
            itemnf.Peso_liquido = Decimal.Parse((txtPeso_liquido.Text.Equals("") ? "0,00" : txtPeso_liquido.Text));
            itemnf.Peso_Bruto = Decimal.Parse((txtPeso_Bruto.Text.Equals("") ? "0,00" : txtPeso_Bruto.Text));

            itemnf.CSTPIS = txtCSTPIS.Text;
            itemnf.CSTCOFINS = txtCSTCOFINS.Text;
            itemnf.inativo = lblInativo.Text.Equals("inativo");


            itemnf.pCredSN = Decimal.Parse((txtpCredSN.Text.Equals("") ? "0,00" : txtpCredSN.Text));
            itemnf.vCredicmssn = Decimal.Parse((txtvCredicmssn.Text.Equals("") ? "0,00" : txtvCredicmssn.Text));

            itemnf.vOrigem = int.Parse(ddlOrigem.SelectedValue);
            Session.Remove("item" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("item" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), itemnf);
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
                nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];



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

                int numItem = int.Parse(txtNum_item.Text);
                if (numItem > nf.qtdItens())
                {

                    nf.addItem(itemnf);
                }
                else
                {
                    nf.atualizaItem(itemnf);
                }


                //nf.confirmaItens();

                Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), nf);

                Session.Remove("item" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
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
            nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            modalLista.Show();
            String campo = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            String sqlLista = "";
            User usr = (User)Session["User"];

            ddlTipoDestinatario.Visible = false;
            switch (campo)
            {
                case "txtPLU":
                case "txtCompPlu":
                    lbltituloLista.Text = "Escolha uma mercadoria";
                    sqlLista = "select  mercadoria.PLU,DESCRICAO,isnull(ean.ean,'') EAN,Referencia =mercadoria.ref_fornecedor,(select preco from mercadoria_loja as ml  where ml.plu=mercadoria.plu and filial='" + usr.getFilial() + "' )as Preco,cf as NCM  from mercadoria left join ean on ean.plu=mercadoria.plu where (mercadoria.plu =  '" + TxtPesquisaLista.Text + "') or(mercadoria.ref_fornecedor like '%" + TxtPesquisaLista.Text + "%')  " +
                               " union all " +
                               "select  mercadoria.PLU,DESCRICAO,isnull(ean.ean,'') EAN,Referencia =mercadoria.ref_fornecedor, (select preco from mercadoria_loja as ml  where ml.plu=mercadoria.plu and filial='" + usr.getFilial() + "' )as Preco,cf as NCM from mercadoria left join ean on ean.plu=mercadoria.plu where (descricao like '%" + TxtPesquisaLista.Text + "%')" +
                               " union all " +
                                "select  mercadoria.PLU,DESCRICAO,isnull(ean.ean,'') EAN ,Referencia =mercadoria.ref_fornecedor, (select preco from mercadoria_loja as ml  where ml.plu=mercadoria.plu and filial='" + usr.getFilial() + "' )as Preco ,cf as NCM from mercadoria left join ean on ean.plu=mercadoria.plu where  (ean like '%" + TxtPesquisaLista.Text + "%') ";

                    break;
                case "txtCliente_CNPJ":
                    ddlTipoDestinatario.Visible = true;

                    if (ddlTipoDestinatario.Text.Equals("FORNECEDOR"))
                    {
                        lbltituloLista.Text = "Escolha um Fornecedor";
                        sqlLista = "select replace(replace(replace(cnpj,'.',''),'-',''),'/','') as CNPJ,Fornecedor,Nome_fantasia as Fantasia from fornecedor where replace(replace(replace(cnpj,'.',''),'-',''),'/','') like '%" + TxtPesquisaLista.Text + "%' or FORNECEDOR like '%" + TxtPesquisaLista.Text + "%' or nome_fantasia like '%" + TxtPesquisaLista.Text + "%'  group by CNPJ,fornecedor,nome_fantasia";
                    }
                    else
                    {
                        lbltituloLista.Text = "Escolha um Cliente";
                        sqlLista = "select CNPJ,codigo_cliente Codigo,Nome_cliente Cliente,nome_fantasia as Fantasia  from cliente where (CNPJ like '%" + TxtPesquisaLista.Text + "%' or codigo_cliente like '%" + TxtPesquisaLista.Text + "%' or nome_cliente like '%" + TxtPesquisaLista.Text + "%' or nome_fantasia like '%" + TxtPesquisaLista.Text + "%') and isnull(inativo,0)=0";
                    }
                    break;
                case "txtcentro_custo":
                    lbltituloLista.Text = "Escolha um Centro de custo";
                    sqlLista = "SELECT  codigo_centro_custo as Codigo , descricao_centro_custo as Descricao  from centro_custo where codigo_centro_custo like'%" + TxtPesquisaLista.Text + "%' and descricao_centro_custo like '%" + TxtPesquisaLista.Text + "%'";
                    break;
                case "txtCodigo_operacao":
                case "txtCompCfop":
                    lbltituloLista.Text = "Escolha um Codigo de operação";
                    sqlLista = "Select  Codigo_operacao AS Codigo,Descricao,CASE WHEN Gera_apagar_receber=1 THEN 'SIM'ELSE'NAO' END AS Financeiro,CASE WHEN Baixa_estoque=1 THEN 'SIM'ELSE'NAO' END AS Estoque, CASE WHEN gera_custo=1 THEN 'SIM'ELSE'NAO' END AS custo , CASE WHEN Imprime_NF=1 THEN 'SIM'ELSE'NAO' END AS ImprimeNF,CASE WHEN Saida=1 THEN 'SIM'ELSE'NAO' END AS SAIDA, CASE WHEN nf_devolucao=1 THEN 'SIM'ELSE'NAO' END AS DEVOLUÇÃO,CASE WHEN PRECO_VENDA=1 THEN 'VENDA'ELSE'CUSTO' END AS PRECO from natureza_operacao  where (codigo_operacao like '%" + TxtPesquisaLista.Text + "%' or descricao like '%" + TxtPesquisaLista.Text + "%') ";

                    if (txtCodigo_operacao.Text.Equals(""))
                    {
                        sqlLista += " and (saida=1 or Imprime_NF =1) ";
                    }
                    else
                    {
                        try
                        {
                            natureza_operacaoDAO OP = new natureza_operacaoDAO(txtCodigo_operacao.Text, null);
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

                case "txtCompNCM":
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

            Session.Remove("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), or);
            TxtPesquisaLista.Text = "";
            exibeLista();

            modalLista.Show();
        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            String itemLista = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            TextBox txt = (TextBox)pnItens.FindControl(itemLista);
            if (txt != null)
            {
                if (txt.Parent.ID.Equals("pnItensFrame"))
                {
                    ModalItens.Show();
                }
                if (txt.ID.Equals("txtCodigo_operacao"))
                {


                    ModalImportar.Show();
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
            carregarGrids();
            modalLista.Hide();
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
            String selecionado = ListaSelecionada(1);

            if (!selecionado.Equals("") && !selecionado.Equals("------"))
            {

                User usr = (User)Session["User"];
                String itemLista = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                TextBox txt = (TextBox)cabecalho.FindControl(itemLista);
                nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                if (itemLista.Equals("txtTransportadora"))
                    txt = txtTransportadora;

                if (itemLista.Equals("txtCodigo_operacao"))
                {
                    txt = txtCodigo_operacao;
                    txt.Text = ListaSelecionada(1);

                    nf.Codigo_operacao = Decimal.Parse((txt.Text.Equals("") ? "0,00" : txt.Text));
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

                    Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), nf);


                    ModalImportar.Show();



                }
                else if (itemLista.Equals("txtReferenciaNota"))
                {
                    String referencia = ListaSelecionada(5);
                    if (!referencia.Equals(""))
                    {
                        nf.NfReferencias.Add(referencia);
                        Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                        Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), nf);

                    }
                }
                else
                {

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

                        txtDescricao.Text = merc.Descricao;
                        txtEmbalagem.Text = merc.Embalagem.ToString("N2");
                        txtPisItem.Text = merc.pis.ToString("N2");
                        txtDescontoItem.Text = "0,00";
                        txtQtde.Text = "1,00";
                        if (nf.NtOperacao != null)
                        {
                            //if (nf.NtOperacao.Saida)
                            //{
                            //    if (txtUfCliente.Text.Equals(usr.filial.UF))
                            //    {
                            //        txtCodigo_operacao_item.Text = "5" + txtCodigo_operacao.Text.Substring(1);
                            //    }
                            //    else
                            //    {
                            //        txtCodigo_operacao_item.Text = "6" + txtCodigo_operacao.Text.Substring(1);
                            //    }
                            //}
                            //else
                            //{
                            //    if (txtUfCliente.Text.Equals(usr.filial.UF))
                            //    {
                            //        txtCodigo_operacao_item.Text = "1" + txtCodigo_operacao.Text.Substring(1);
                            //    }
                            //    else
                            //    {
                            //        txtCodigo_operacao_item.Text = "2" + txtCodigo_operacao.Text.Substring(1);
                            //    }
                            //}
                            if (nf.NtOperacao.Saida && nf.NtOperacao.NF_devolucao)
                            {
                                txtUnitario.Text = (nf.NtOperacao.Preco_Venda ? merc.Preco.ToString("N4") : merc.preco_compra.ToString("N4"));
                                txtCodigo_Tributacao.Text = merc.Codigo_Tributacao_ent.ToString();
                                txtCSTPIS.Text = merc.cst_saida;
                                txtCSTCOFINS.Text = merc.cst_saida;
                                //txtredutor_base.Text = merc.;
                                CalculatotalItem(merc);

                                txtIPI.Text = merc.IPI.ToString("N2");
                                txtIPIV.Text = ((Decimal.Parse(TxtTotalItem.Text) * merc.IPI) / 100).ToString("N2");

                            }
                            else if (!nf.NtOperacao.Saida)
                            {
                                txtUnitario.Text = (nf.NtOperacao.Preco_Venda ? merc.Preco.ToString("N4") : merc.preco_compra.ToString("N4"));
                                txtCodigo_Tributacao.Text = merc.Codigo_Tributacao_ent.ToString();
                                txtCSTPIS.Text = merc.cst_entrada;
                                txtCSTCOFINS.Text = merc.cst_entrada;
                                //txtredutor_base.Text = merc.;
                                CalculatotalItem(merc);

                                txtIPI.Text = merc.IPI.ToString("N2");
                                txtIPIV.Text = ((Decimal.Parse(TxtTotalItem.Text) * merc.IPI) / 100).ToString("N2");
                            }
                            else
                            {
                                txtUnitario.Text = (nf.NtOperacao.Preco_Venda ? merc.Preco.ToString("N4") : merc.preco_compra.ToString("N4"));
                                txtCodigo_Tributacao.Text = merc.Codigo_Tributacao.ToString();
                                txtCSTPIS.Text = merc.cst_saida;
                                txtCSTCOFINS.Text = merc.cst_saida;
                                txtredutor_base.Text = "0,00";
                                CalculatotalItem(merc);
                                txtIPI.Text = "0,00"; //merc.IPI.ToString("N2");
                                txtIPIV.Text = "0,00";//((Decimal.Parse(TxtTotalItem.Text) * merc.IPI) / 100).ToString("N2");

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

                        ddlOrigem.SelectedValue = merc.origem.ToString();
                        //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                        ItemInativo(merc.Inativo == 1);
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
                            if (forn.indIEDest == 9)
                            {
                                ddlindFinal.SelectedValue = "1";
                            }
                        }
                        else
                        {
                            ClienteDAO cliente = new ClienteDAO(ListaSelecionada(2), usr);
                            txtCliente_Fornecedor.Text = cliente.Codigo_Cliente;
                            TxtNomeCliente.Text = cliente.Nome_Cliente;
                            txtUfCliente.Text = cliente.UF;
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
                        if (nf.NfItens.Count > 0 && !nf.Codigo_operacao.ToString().Equals("0"))
                        {
                            nf.atribuirCodOperacaoItem();
                        }

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
                        Session.Remove("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                        Session.Add("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "txtCodigo_operacao");
                        TxtPesquisaLista.Text = "";
                        exibeLista();
                    }

                }
            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                modalLista.Show();
            }
        }

        private void CalculatotalItem(MercadoriaDAO merc)
        {
            User usr = (User)Session["User"];
            tributacaoDAO trib = new tributacaoDAO(txtCodigo_Tributacao.Text, usr);
            nfDAO nf = (nfDAO)Session["obj" + urlSessao()];
            EnabledControls(pnItensFrame, true);


            decimal aliquotaicms = 0;
            decimal aliquotaIva = 0;
            decimal porcIva = 0;
            String indiceSt = "";
            bool Simples = usr.filial.Reg_Federal.ToUpper().Equals("SIMPLES NACIONAL");
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
                    if (strAliquotaIva.Equals("0") || strAliquotaIva.Equals(""))
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

                        rsAliquotaEstado = Conexao.consulta("select * from aliquota_imp_estado where uf='" + nf.UfCliente + "' and ncm='" + merc.cf + "'", null, false);
                        if (rsAliquotaEstado.Read())
                        {
                            aliquotaicms = Decimal.Parse(rsAliquotaEstado["icms_interestadual"].ToString());
                            porcIva = Decimal.Parse(rsAliquotaEstado["iva_ajustado"].ToString());
                            aliquotaIva = Decimal.Parse(rsAliquotaEstado["icms_estado"].ToString());
                            indiceSt = rsAliquotaEstado["CST"].ToString();
                        }
                        else
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
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {
                        if (rsAliquotaEstado != null)
                            rsAliquotaEstado.Close();


                    }
                }


            }


            if (indiceSt != null)
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
                    if (nf.NtOperacao.NF_devolucao)
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
                        strcfop += nf.NtOperacao.Codigo_operacao.ToString().Substring(1);
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

                    strcfop += nf.NtOperacao.Codigo_operacao.ToString().Substring(1);
                }
                txtCodigo_operacao_item.Text = strcfop;
            }

            nf_itemDAO item = carregaItem();

            TxtTotalItem.Text = item.vtotal_produto.ToString("N2");
            txtCofinsItem.Text = item.COFINSV.ToString("N2");
            txtPisItem.Text = item.PISV.ToString("N2");
            txtCSTPIS.Text = item.CSTPIS;
            txtCSTCOFINS.Text = item.CSTCOFINS;

            if (trib != null && trib.Incide_ICM_Subistituicao)
            {
                if (item.vmargemIva > 0 && (txtiva.Text.Equals("") || txtiva.Text.Equals("0,00")))
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
                txtIPIV.Text = item.IPIV.ToString("N2");


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
            Session.Remove("item" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
        }

        protected void ImgExcluiItem_Click(object sender, ImageClickEventArgs e)
        {
            nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            nf.removeItem(nf.item(int.Parse(txtNum_item.Text) - 1));
            Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), nf);
            PnExcluirItem.Visible = false;
            habilitarCampos(true);
            carregarDados();
        }

        protected void gridPagamentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
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
                nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
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

                nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                User usr = (User)Session["user"];
                nf_pagamentoDAO pg = new nf_pagamentoDAO(usr);
                pg.Codigo = txtCodigo.Text;
                pg.Cliente_Fornecedor = txtCliente_Fornecedor.Text;
                pg.Tipo_NF = "3";
                pg.Tipo_pagamento = txtTipoPg.Text;
                pg.Vencimento = DateTime.Parse(txtVencimentoPg.Text);
                pg.Valor = Decimal.Parse(txtValorPg.Text);
                nf.addPagamento(pg);

                lblTotalPagamentos.Text = nf.TotalPag().ToString("N2");
                Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), nf);
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
        }

        protected void btnConfirmaImportar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                User usr = (User)Session["User"];

                status = "incluir";
                nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                nf.Tipo_NF = "3";
                nf.centro_custo = visualSysWeb.code.Funcoes.valorParametro("CENTRO_CUSTO_NF_SAIDA", new User());
                ModalImportar.Hide();
                String strCnpjSelecionado = "";
                ArrayList arrPedidosSel = new ArrayList();
                foreach (GridViewRow row in gridPedidosImportar.Rows)
                {
                    CheckBox ckItem = (CheckBox)row.FindControl("chkItem");


                    if (ckItem.Checked)
                    {
                        if (strCnpjSelecionado.Equals(""))
                        {
                            strCnpjSelecionado = row.Cells[4].Text;
                        }

                        if (!row.Cells[4].Text.Equals(strCnpjSelecionado))
                        {
                            throw new Exception("Não é permitido selecionar Pedidos com CNPJ diferentes!");
                        }
                        arrPedidosSel.Add(row.Cells[1].Text);

                    }
                }

                if (arrPedidosSel.Count == 0)
                {
                    throw new Exception("Selecione no minimo um Pedido para importação!");

                }
                
                String pedidos = "";
                foreach (String pedido in arrPedidosSel)
                {
                    nf.importarPedido(pedido, 1);
                    if (pedidos.Length > 0)
                    {
                        pedidos += ",";
                    }
                    pedidos += " " + pedido.Trim();
                }
                nf.Observacao += "Pedidos Importados:" +pedidos;
                carregarDados();
                ModalImportar.Hide();
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

            txtcentro_custo.Text = visualSysWeb.code.Funcoes.valorParametro("CENTRO_CUSTO_NF_SAIDA", new User());
            //txtCodigo_operacao.Text = "0";
            nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            if (nf.Codigo_operacao.ToString().Equals("0"))
            {
                Session.Remove("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "txtCodigo_operacao");
                TxtPesquisaLista.Text = "";
                exibeLista();
            }

        }
        protected void carregarDadosItens(nf_itemDAO itemnf)
        {
            User usr = (User)Session["user"];

            txtPLU.Text = itemnf.PLU;
            txtCODIGO_REFERENCIA.Text = itemnf.CODIGO_REFERENCIA;
            txtDescricao.Text = itemnf.Descricao;
            txtQtde.Text = itemnf.Qtde.ToString("N3");
            txtEmbalagem.Text = itemnf.Embalagem.ToString();
            txtUnitario.Text = itemnf.Unitario.ToString("N2");
            txtCodigo_Tributacao.Text = itemnf.Codigo_Tributacao.ToString();


            txtDescontoItem.Text = itemnf.Desconto.ToString("N2");
            txtdespesas.Text = itemnf.despesas.ToString("N2"); ;
            txtIPI.Text = itemnf.IPI.ToString("N2");
            txtIPIV.Text = itemnf.IPIV.ToString("N2");
            txtaliquota_icms.Text = itemnf.aliquota_icms.ToString("N2");
            txtAliquota_iva.Text = itemnf.aliquota_iva.ToString("N2");

            txtredutor_base.Text = itemnf.redutor_base.ToString("N2");
            txtPisItem.Text = itemnf.PISV.ToString("N2");
            txtCofinsItem.Text = itemnf.COFINSV.ToString("N2");
            txtNum_item.Text = itemnf.Num_item.ToString();
            txtCodigo_operacao_item.Text = itemnf.codigo_operacao.ToString();
            txtNCM.Text = itemnf.NCM.ToString();
            txtCEST.Text = itemnf.CEST;
            txtUnd.Text = itemnf.Und;
            txtPeso_liquido.Text = itemnf.Peso_liquido.ToString("N2");
            txtPeso_Bruto.Text = itemnf.Peso_Bruto.ToString("N2");
            txtCSTPIS.Text = itemnf.CSTPIS;
            txtCSTCOFINS.Text = itemnf.CSTCOFINS;
            txtvCredicmssn.Text = itemnf.vCredicmssn.ToString("N2");
            txtpCredSN.Text = itemnf.pCredSN.ToString("N2");
            ModalItens.Show();
            MercadoriaDAO merc = new MercadoriaDAO(itemnf.PLU, usr);
            merc.margem_iva = itemnf.vmargemIva;
            CalculatotalItem(merc);
            ddlOrigem.SelectedValue = itemnf.origem;
            txtmargem_iva.Text = itemnf.vmargemIva.ToString("N2");
            txtiva.Text = itemnf.vIva.ToString("N2");
            TxtTotalItem.Text = itemnf.Total.ToString("N2");
            lblInativo.Text = (itemnf.inativo ? "inativo" : "");
            ItemInativo(itemnf.inativo);
        }
        protected void txt_TextChanged(object sender, EventArgs e)
        { // carregarDadosItens(carregaItem());
            LblErroItem.Text = "";
            ((TextBox)sender).BackColor = System.Drawing.Color.White;

            try
            {

                String id = ((TextBox)sender).ID;
                nf_itemDAO itemnf = (nf_itemDAO)Session["item" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                switch (id)
                {
                    case "txtQtde":
                        itemnf.Qtde = Decimal.Parse(txtQtde.Text);
                        txtUnitario.Text = itemnf.Unitario.ToString();
                        txtEmbalagem.Focus();
                        txtIPIV.Text = itemnf.IPIV.ToString("N2");
                        txtiva.Text = itemnf.CalculoIva().ToString("N2");
                        break;
                    case "txtEmbalagem":
                        itemnf.Embalagem = Decimal.Parse(txtEmbalagem.Text);
                        txtUnitario.Text = itemnf.Unitario.ToString();
                        txtUnitario.Focus();
                        txtIPIV.Text = itemnf.IPIV.ToString("N2");
                        txtiva.Text = itemnf.CalculoIva().ToString("N2");
                        break;
                    case "txtUnitario":
                        itemnf.Unitario = Decimal.Parse(txtUnitario.Text);
                        txtIPIV.Text = itemnf.IPIV.ToString("N2");
                        txtiva.Text = itemnf.CalculoIva().ToString("N2");
                        txtCodigo_Tributacao.Focus();
                        break;
                    case "txtDescontoItem":
                        itemnf.Desconto = Decimal.Parse(txtDescontoItem.Text);
                        txtIPIV.Text = itemnf.IPIV.ToString("N2");
                        txtiva.Text = itemnf.CalculoIva().ToString("N2");
                        txtdespesas.Focus();
                        break;
                    case "txtdespesas":
                        itemnf.despesas = Decimal.Parse(txtdespesas.Text);
                        txtIPI.Focus();
                        break;
                    case "txtIPI":
                        itemnf.IPI = Decimal.Parse(txtIPI.Text);
                        txtIPI.Text = itemnf.IPI.ToString("N2");
                        txtIPIV.Text = itemnf.IPIV.ToString("N2");
                        txtIPIV.Focus();
                        txtiva.Text = itemnf.CalculoIva().ToString("N2");
                        break;
                    case "txtIPIV":
                        itemnf.IPIV = Decimal.Parse(txtIPIV.Text);
                        txtIPIV.Text = itemnf.IPIV.ToString("N2");
                        txtIPI.Text = itemnf.IPI.ToString("N2");
                        txtmargem_iva.Focus();
                        txtiva.Text = itemnf.CalculoIva().ToString("N2");
                        break;
                    case "txtmargem_iva":
                        itemnf.vmargemIva = Decimal.Parse(txtmargem_iva.Text);
                        txtmargem_iva.Text = itemnf.vmargemIva.ToString("N2");
                        txtiva.Text = itemnf.CalculoIva().ToString("N2");
                        if (txtiva.Enabled)
                            txtiva.Focus();
                        else
                            txtaliquota_icms.Focus();
                        break;
                    case "txtiva":
                        itemnf.vIva = Decimal.Parse(txtiva.Text);
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
                        itemnf.redutor_base = Decimal.Parse(txtredutor_base.Text);
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
                            //itemnf.Codigo_Tributacao = decimal.Parse(txtCodigo_Tributacao.Text);
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


                Session.Remove("item" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("item" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), itemnf);
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
            //lblCancelarNota.Visible = false;
            //ImgBtnCancelarNota.Visible = false;
            //ImgBtnCartaDeCorrecao.Visible = false;
            //lblCartaCorrecaoText.Visible = false;
            //divSituacao.Visible = true;
            lblProximoXml.Visible = true;
            visualizarConfirmacao(false);
            visualizarCorrecao(false);
            nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];


            if (nf != null)
            {
                xmlNFE xml = new xmlNFE(nf);
                switch (nf.status)
                {
                    case "DIGITACAO":
                        lblProximoXml.Text = "AUTORIZAR";
                        break;
                    //case "VALIDADO":
                    //    lblProximoXml.Text = "TRANSMITIR";
                    //    break;
                    //case "TRANSMITIDO":
                    //    lblProximoXml.Text = "SITUACAO";
                    //    break;
                    //case "AUTORIZADO":
                    //    lblProximoXml.Text = "AUTORIZADO";
                    //    btnProximo.Visible = false;
                    //    //lblCancelarNota.Visible = true;
                    //    //ImgBtnCancelarNota.Visible = true;
                    //    //ImgBtnCartaDeCorrecao.Visible = true;
                    //    //lblCartaCorrecaoText.Visible = true;
                    //    //lblResultadoXML.Text = "Nota fiscal Autorizada com o ID= " + xml.funChave() + "<br/>" + nf.UltimaCorrecaoRegistrada;

                    //    //lblResultadoXML.ForeColor = System.Drawing.Color.Blue;
                    //    //divSituacao.Visible = true;
                    //    break;
                    case "CANCELADA":
                        lblProximoXml.Text = "CANCELADA";
                        btnProximo.Visible = false;
                        //lblCancelarNota.Visible = false;
                        //ImgBtnCancelarNota.Visible = false;
                        //ImgBtnCartaDeCorrecao.Visible = false;
                        //lblCartaCorrecaoText.Visible = false;
                        //divSituacao.Visible = false;
                        //xmlNFE xml1 = new xmlNFE(nf);
                        //try
                        //{
                        //    lblResultadoXML.Text = xml1.respostaCancelamento();
                        //    lblResultadoXML.ForeColor = System.Drawing.Color.Blue;
                        //}
                        //catch (Exception err)
                        //{

                        //    lblResultadoXML.Text = err.Message;
                        //    lblResultadoXML.ForeColor = System.Drawing.Color.Red;
                        //}

                        break;
                }


            }
        }
        protected void gravarXml()
        {
            try
            {


                nfDAO nf = (nfDAO)Session["obj"];
                xmlNFE xml = new xmlNFE(nf);
                String ResultadoXML = "";

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
                            xml.gravarArquivo(validarArquivo);
                            nf.status = "VALIDADO";

                            nf.AtualizarStatus();

                            Session.Remove("obj");
                            Session.Add("obj", nf);

                            //lblResultadoXML.ForeColor = System.Drawing.Color.Blue;

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
                                ResultadoXML = xml.respostaEnvio();

                                if (ResultadoXML.Equals("Sem Resposta"))
                                {

                                    xml.transmitir();
                                    int tentativas = 0;
                                    while (tentativas < 50)
                                    {
                                        System.Threading.Thread.Sleep(2000);
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
                                    nf.status = "TRANSMITIDO";
                                    nf.AtualizarStatus();
                                    Session.Remove("resultadoXml");
                                    Session.Add("resultadoXml", ResultadoXML);
                                }
                                else
                                {
                                    nf.status = "TRANSMITIDO";
                                    nf.AtualizarStatus();
                                    Session.Remove("resultadoXml");
                                    Session.Add("resultadoXml", ResultadoXML);
                                }
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
                            nf.Emissao = DateTime.Now;
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
                Session.Remove("erroXml");
                Session.Add("erroXml", err.Message);
                erroBtnXml();
            }

        }

        protected void situacaonotafiscal()
        {
            try
            {
                Session.Remove("resultadoXml");

                String nomeObj = "obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "");
                nfDAO nf = (nfDAO)Session[nomeObj];

                xmlNFE xml = new xmlNFE(nf);
                String ResultadoXML = "";

                ResultadoXML = "CONSULTA_" + xml.situacaoNotafiscal();
                //lblResultadoXML.Text = ResultadoXML;
                if (ResultadoXML.IndexOf("Status:217") >= 0)
                {
                    btnProximo.Visible = true;
                    throw new Exception(ResultadoXML);
                }
                else if (ResultadoXML.IndexOf("Status:100") >= 0)
                {
                    if (!nf.status.Equals("AUTORIZADO"))
                    {
                        nf.status = "AUTORIZADO";
                        nf.AtualizarStatus();

                    }

                }
                else if (ResultadoXML.IndexOf("Status:101") >= 0)
                {
                    switch (nf.status)
                    {
                        case "AUTORIZADO":
                            nf.status = "CANCELADA";
                            nf.nf_Canc = true;
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

                String nomeObj = "obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "");
                nfDAO nf = (nfDAO)Session[nomeObj];
                xmlNFE xml = new xmlNFE(nf);
                //xml.CancelarNFE(txtJustificativa.Text);
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

                        //if (cont >= 50)
                        //    throw err;
                        //else
                        //lblResultadoXML.Text = "Aguarde....";
                    }
                }



                nf.status = "CANCELADA";
                nf.nf_Canc = true;
                nf.salvar(false);
                //lblResultadoXML.Text = ResultadoXML;
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

                String nomeObj = "obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "");
                nfDAO nf = (nfDAO)Session[nomeObj];
                String nCorrecao = Conexao.retornaUmValor("select max(seq) from nfe_correcao where codigo ='" + nf.Codigo + "' and filial='" + nf.usr.getFilial() + "'", null);
                if (nCorrecao.Equals("") || nCorrecao.Length > 10)
                    nCorrecao = "1";
                else
                    nCorrecao = (int.Parse(nCorrecao) + 1).ToString();

                xmlNFE xml = new xmlNFE(nf);
                //xml.CorrecaoNFE(txtCorrecao.Text, nCorrecao);
                String ResultadoXML = "";
                int cont = 0;
                while (cont < 50)
                {
                    try
                    {

                        System.Threading.Thread.Sleep(2000);
                        //ResultadoXML = xml.respostaCartaCorrecao(nCorrecao);

                        break;
                    }
                    catch (Exception err)
                    {
                        int i = err.Message.IndexOf("Erro-Correcao");
                        if (err.Message.IndexOf("Erro-Correcao") >= 0)
                            throw err;

                        cont++;

                        //if (cont >= 50)
                        //    throw err;
                        //else
                        //    //lblResultadoXML.Text = "Aguarde....";
                    }
                }


                nf.nota_referencia = nCorrecao;
                //nf.salvarCorrecao(ResultadoXML.Substring(ResultadoXML.IndexOf("Protocolo :") + 11, 15), txtCorrecao.Text);

                //lblResultadoXML.Text = ResultadoXML;
                //lblResultadoXML.Visible = true;
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
            String strProcessa = (String)Session["processaNotaPedido"];
            if (strProcessa != null)
                return;
            else
                Session.Add("processaNotaPedido","processando");

            nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            //bool atualiar = false;
            try
            {

                //divSituacao.Visible = false;
                nf.status = "AUTORIZADO";
                nf.Data = DateTime.Now;
                nf.Emissao = DateTime.Now;
                nf.AtualizarStatus();
                Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), nf);
                carregarDados();
                //lblResultadoXML.ForeColor = System.Drawing.Color.Blue;

                btnXml.Visible = false;
                modalXml.Hide();
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
                            Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                            Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), nf);
                            carregarDados();
                            BotaoXml();
                            break;
                    }
                }
                //lblResultadoXML.Text = err.Message;
                //lblResultadoXML.ForeColor = System.Drawing.Color.Red;

                carregarDados();
                modalXml.Show();
            }
        }


        protected void erroBtnXml()
        {
            btnProximo.Visible = false;
            lblProximoXml.Visible = false;
            //ImgBtnCancelarNota.Visible = false;
            //lblCancelarNota.Visible = false;
            //ImgBtnCartaDeCorrecao.Visible = false;
            //lblCartaCorrecaoText.Visible = false;
            //imgBtnConfirmaCancelamento.Visible = false;
            //lblConfirmaCancelamento.Visible = false;
            //msgCorrecao.Visible = false;
            //imgBtnConfirmaCorrecao.Visible = false;
            //lblConfirmaCorrecao.Visible = false;
            //divSituacao.Visible = true;
        }



        protected void btnXml_Click(object sender, EventArgs e)
        {
            Session.Remove("processaNotaPedido");
            //lblResultadoXML.Text = "";
            //lblResultadoXML.Visible = true;
            carregarDados();
            nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
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
                        //    Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                        //    Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), nf);
                        //    lblResultadoXML.Text = situacao;

                        //}

                        BotaoXml();
                    }
                    catch (Exception err)
                    {

                        //lblResultadoXML.Text = err.Message;
                        //lblResultadoXML.ForeColor = System.Drawing.Color.Red;
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
            visualizarConfirmacao(true);
            //ImgBtnCancelarNota.Visible = false;
            //lblCancelarNota.Visible = false;
            modalXml.Show();
        }

        protected void visualizarConfirmacao(bool visivel)
        {
            //lblResultadoXML.Visible = !visivel;
            //lblJustificativa.Visible = visivel;
            //txtJustificativa.Visible = visivel;
            //lblConfirmaCancelamento.Visible = visivel;
            //imgBtnConfirmaCancelamento.Visible = visivel;
            //lblCorrecao.Visible = false;
            //txtCorrecao.Visible = false;
            //divSituacao.Visible = !visivel;
            //ImgBtnCartaDeCorrecao.Visible = false;
            //msgCorrecao.Visible = false;
            //if (visivel)
            //{
            //    imgBtnConfirmaCorrecao.Visible = false;
            //    lblConfirmaCorrecao.Visible = false;
            //    lblCartaCorrecaoText.Visible = false;
            //}
        }

        protected void visualizarCorrecao(bool visivel)
        {
            //lblResultadoXML.Visible = !visivel;
            //lblJustificativa.Visible = false;
            //txtJustificativa.Visible = false;
            //lblConfirmaCancelamento.Visible = false;
            //imgBtnConfirmaCancelamento.Visible = false;
            //lblCorrecao.Visible = visivel;
            //divSituacao.Visible = !visivel;
            //txtCorrecao.Visible = visivel;
            //msgCorrecao.Visible = visivel;
            //imgBtnConfirmaCorrecao.Visible = visivel;
            //lblConfirmaCorrecao.Visible = visivel;
            //if (visivel)
            //{
            //    ImgBtnCancelarNota.Visible = false;
            //    lblCancelarNota.Visible = false;

            //}

        }

        protected void imgBtnConfirmaCancelamento_Click(object sender, ImageClickEventArgs e)
        {
            //try
            //{

            //    if (!txtJustificativa.Text.Trim().Equals("") && txtJustificativa.Text.Trim().Length >= 15 && txtJustificativa.Text.Trim().Length <= 255)
            //    {
            //        visualizarConfirmacao(false);
            //        visualizarCorrecao(false);

            //        Session.Remove("aborta");
            //        TimerXml.Interval = 450;
            //        TimerXml.Enabled = true;

            //        System.Threading.Thread th = new System.Threading.Thread(cancelarxmlnota);

            //        th.Start();
            //        imgBtnConfirmaCancelamento.Visible = false;

            //    }
            //    else
            //    {
            //        lblResultadoXML.Text = "Informe uma Justificativa Valida de no minino 15 caracters e no Maximo 255!";
            //        lblResultadoXML.ForeColor = System.Drawing.Color.Red;
            //        lblResultadoXML.Visible = true;
            //    }
            //}
            //catch (Exception err)
            //{
            //    lblResultadoXML.Visible = true;
            //    lblResultadoXML.Text = err.Message;
            //    lblResultadoXML.ForeColor = System.Drawing.Color.Red;

            //    imgBtnConfirmaCancelamento.Visible = true;
            //}

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
                //lblResultadoXML.Visible = true;
                //lblResultadoXML.Text = resultado;
                //TimerXml.Enabled = false;
                //lblResultadoXML.ForeColor = System.Drawing.Color.Blue;
                BotaoXml();
                nfDAO nf = (nfDAO)Session["obj"];
                if (nf != null)
                {
                    if (nf.status.Equals("TRANSMITIDO"))
                    {
                        System.Threading.Thread.Sleep(2000);
                        //TimerXml.Interval = 450;
                        //TimerXml.Enabled = true;
                        System.Threading.Thread th = new System.Threading.Thread(gravarXml);
                        th.Start();

                    }
                    else
                    {
                        btnProximo.Visible = true;
                        Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                        Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), nf);
                        Session.Remove("obj");
                    }

                    if (nf.status.Equals("AUTORIZADO"))
                    {
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
                //lblResultadoXML.Visible = true;
                //lblResultadoXML.Text = error;
                //TimerXml.Enabled = false;

                //lblResultadoXML.ForeColor = System.Drawing.Color.Red;
                Session.Remove("erroXml");
                nfDAO nf = (nfDAO)Session["obj"];
                if (nf != null)
                {
                    switch (nf.status)
                    {
                        case "TRANSMITIDO":
                            nf.status = "DIGITACAO";
                            nf.AtualizarStatus();
                            Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                            Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), nf);
                            Session.Remove("obj");
                            carregarDados();

                            break;
                    }
                }

            }
            else
            {
                //lblResultadoXML.Visible = true;
                //lblResultadoXML.Text = " Aguarde ....";
                //lblResultadoXML.ForeColor = System.Drawing.Color.Green;

                btnProximo.Visible = false;
                //imgBtnConfirmaCancelamento.Visible = false;

            }
            modalXml.Show();

            if (aborta != null)
            {
                //TimerXml.Enabled = false;
                //System.Threading.Thread th = new System.Threading.Thread(gravarXml);
                //th.Start();
                //modalXml.Hide();

            }
        }

        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                nfDAO obj = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

                obj.status = "CANCELADA";
                obj.nf_Canc = true;
                obj.salvar(false);

                modalPnConfirma.Hide();
                lblError.Text = "Registro Excluido com sucesso";
                limparCampos();
                status = "pesquisar";
                carregabtn(pnBtn);
                Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
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
            nfDAO obj = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            if (DDlRefECF.Text.Equals("NFE"))
            {
                if (!txtReferenciaNota.Text.Trim().Equals(""))
                {
                    obj.NfReferencias.Add(txtReferenciaNota.Text);
                    txtReferenciaNota.Text = "";
                    txtReferenciaNota.Focus();
                    Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
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

                    Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
                    carregarGrids();
                }


            }
        }

        protected void gridNfReferencia_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            nfDAO obj = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            if (DDlRefECF.Text.Equals("NFE"))
                obj.NfReferencias.RemoveAt(index);
            else
                obj.ECFReferencias.RemoveAt(index);

            Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);

            carregarGrids();
        }

        protected void ddlTipoDestinatario_TextChanged(object sender, EventArgs e)
        {
            exibeLista();
        }

        protected void DDlRefECF_SelectedIndexChanged(object sender, EventArgs e)
        {

            nfDAO obj = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            if (obj != null)
            {
                Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);

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

                //if (!txtCorrecao.Text.Trim().Equals("") && txtCorrecao.Text.Trim().Length >= 15 && txtCorrecao.Text.Trim().Length <= 255)
                //{
                //    visualizarConfirmacao(false);
                //    visualizarCorrecao(false);

                //    Session.Remove("aborta");
                //    //TimerXml.Interval = 450;
                //    //TimerXml.Enabled = true;

                //    System.Threading.Thread th = new System.Threading.Thread(correcaoxmlnota);

                //    th.Start();
                //    //imgBtnConfirmaCancelamento.Visible = false;
                //    //imgBtnConfirmaCorrecao.Visible = false;

                //}
                //else
                //{
                //    //msgCorrecao.Visible = false;
                //    //lblResultadoXML.Text = "Informe uma Correção Valida de no minino 15 caracters e no Maximo 255!";
                //    //lblResultadoXML.ForeColor = System.Drawing.Color.Red;
                //    //lblResultadoXML.Visible = true;
                //}
            }
            catch (Exception err)
            {

                //msgCorrecao.Visible = false;
                //lblResultadoXML.Visible = true;
                //lblResultadoXML.Text = err.Message;
                //lblResultadoXML.ForeColor = System.Drawing.Color.Red;

                //imgBtnConfirmaCancelamento.Visible = true;
                //imgBtnConfirmaCorrecao.Visible = true;

            }

            modalXml.Show();
        }
        protected void ImgBtnCartaDeCorrecao_Click(object sender, ImageClickEventArgs e)
        {
            //ImgBtnCartaDeCorrecao.Visible = false;
            //lblCartaCorrecaoText.Visible = false;
            visualizarCorrecao(true);
            modalXml.Show();
        }

        protected void imgBtnConsultaSituacao_Click(object sender, ImageClickEventArgs e)
        {
            Session.Remove("aborta");
            Session.Remove("resultadoXml");
            //TimerXml.Interval = 450;
            //TimerXml.Enabled = true;

            System.Threading.Thread th = new System.Threading.Thread(situacaonotafiscal);

            th.Start();
            modalXml.Show();
        }

        protected void imgBtnConfirmaPedidoFechado_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                status = "incluir";
                nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                //nf.Tipo_NF = "1";
                nf.importarPedido(lblConfirmaPedidoFechado.Text, 1);
                nf.centro_custo = visualSysWeb.code.Funcoes.valorParametro("CENTRO_CUSTO_NF_SAIDA", new User());


                Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), nf);
                carregarDados();
                if (nf.Cliente_Fornecedor.Equals(""))
                {
                    Session.Remove("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "txtCliente_CNPJ");
                    TxtPesquisaLista.Text = "";
                    exibeLista();
                }
                else if (nf.Codigo_operacao.ToString().Equals("0"))
                {
                    Session.Remove("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "txtCodigo_operacao");
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
                nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                //nf.Tipo_NF = "1";
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


                Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), nf);
                carregarDados();
                if (nf.Cliente_Fornecedor.Equals(""))
                {
                    Session.Remove("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "txtCliente_CNPJ");
                    TxtPesquisaLista.Text = "";
                    exibeLista();
                }
                else if (nf.Codigo_operacao.ToString().Equals("0"))
                {
                    Session.Remove("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "txtCodigo_operacao");
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

        protected void chktodos_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTd = (CheckBox)sender;
            foreach (GridViewRow linha in gridPedidosImportar.Rows)
            {
                CheckBox chk = (CheckBox)linha.FindControl("chkItem");
                chk.Checked = chkTd.Checked;

            }
            ModalImportar.Show();
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
                nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                //nf.Tipo_NF = "1";
                nf.Emissao = DateTime.Now;
                nf.Data = DateTime.Now;
                nf.centro_custo = visualSysWeb.code.Funcoes.valorParametro("CENTRO_CUSTO_NF_SAIDA", new User());

                Ler();
                modalImportColetor.Hide();

                Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), nf);
                carregarDados();
                if (nf.Cliente_Fornecedor.Equals(""))
                {
                    Session.Remove("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "txtCliente_CNPJ");
                    TxtPesquisaLista.Text = "";
                    exibeLista();
                }
                else if (nf.Codigo_operacao.ToString().Equals("0"))
                {
                    Session.Remove("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "txtCodigo_operacao");
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
            nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            Hashtable tbPlus = new Hashtable();
            //int vdecimal = int.Parse(txtDecimaisContado.Text) + 1;
            String strPlus = "";

            String contado = "";
            String plu = "";

            foreach (string line in lines)
            {
                if (line.Length > 0)
                {

                    if (coletor.coletorFixo)
                    {
                        plu = line.Substring(coletor.pluInicio, coletor.tamPlu);
                        contado = line.Substring(coletor.contadoInicio, coletor.tamContado);
                    }
                    else
                    {
                        string[] arrLinha = line.Split(coletor.delimitador[0]);
                        plu = arrLinha[coletor.pluInicio];
                        contado = arrLinha[coletor.contadoInicio];
                    }
                    if (!tbPlus.ContainsKey(plu.Trim()))
                    {
                        tbPlus.Add(plu.Trim(), contado.Trim());
                    }
                    else
                    {
                        String contPlu = tbPlus[plu.Trim()].ToString();
                        contado = (Decimal.Parse(contPlu) + Decimal.Parse(contado)).ToString();
                        tbPlus[plu.Trim()] = contado;
                    }

                    if (strPlus.Length > 0)
                        strPlus += ",";

                    strPlus += "'" + plu.Trim() + "'";
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
            nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            nf.removeItem(nf.item(int.Parse(txtCompItemNf.Text) - 1));
            Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), nf);
            PnExcluirItem.Visible = false;
            habilitarCampos(true);
            carregarDados();
        }
        protected void btnConfirmaItensComplementar_Click(object sender, ImageClickEventArgs e)
        {
            bool novo = false;
            User usr = (User)Session["User"];
            nfDAO nf = (nfDAO)Session["obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
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
            //Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            //Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), nf);

            carregarDados();
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
        protected void imgPesquisaImporta_Click(object sender, EventArgs e)
        {
            carregarPedidosImportacao();
        }
        private void carregarPedidosImportacao()
        {
            User usr = (User)Session["User"];
            String sql = "Select pedido" +
                          " , Data = convert(varchar, pedido.Data_cadastro, 103)" +
                          " , Cliente = cliente_fornec + '-' + cliente.nome_cliente" +
                          " , CNPJ" +
                          " , Total" +
                   " from pedido " +
                   " inner join cliente on pedido.cliente_fornec = cliente.codigo_cliente" +
                   " where tipo = 1 and status = 1";

            DateTime dtDe = new DateTime();
            DateTime dtAte = new DateTime();
            DateTime.TryParse(txtDtDeImp.Text, out dtDe);
            DateTime.TryParse(txtDtAteImp.Text, out dtAte);

            sql += " and ( pedido.data_cadastro between '" + dtDe.ToString("yyyy-MM-dd") + "' and '" + dtAte.ToString("yyyy-MM-dd") + "') ";
            if (!txtImportaCliente.Text.Equals(""))
            {
                sql += " and (" +
                        "(cliente.codigo_cliente ='" + txtImportaCliente.Text + "' )" +
                        " or (cliente.nome_cliente like '%" + txtImportaCliente.Text + "%')" +
                        " or (cliente.nome_fantasia like '%" + txtImportaCliente.Text + "%')" +
                        " or (replace(replace(replace(cliente.cnpj,'.',''),'-',''),'/','') = '" + txtImportaCliente.Text.Replace(".", "").Replace("-", "").Replace("/", "") + "')" +
                    ")";
            }
            sql += " order by convert(int,pedido) desc ";

            gridPedidosImportar.DataSource = Conexao.GetTable(sql, usr, false);
            gridPedidosImportar.DataBind();



            ModalImportar.Show();


        }

    }
}