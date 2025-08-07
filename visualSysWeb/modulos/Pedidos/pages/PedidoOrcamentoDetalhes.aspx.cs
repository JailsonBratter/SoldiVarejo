using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

using visualSysWeb.dao;
using System.Data.SqlClient;
using visualSysWeb.code;


namespace visualSysWeb.modulos.Pedidos.pages
{
    public partial class PedidoOrcamentoDetalhes : visualSysWeb.code.PagePadrao
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            User usr = (User)Session["User"];


            if (!IsPostBack)
            {

                pedidoDAO obj = (pedidoDAO)Session["pedido" + urlSessao()];

                Conexao.preencherDDL1Branco(ddlAgrupamento, "Select * from agrupamento_producao ORDER BY Agrupamento", "agrupamento", "agrupamento", usr);

            }
            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    status = "incluir";
                    pedidoDAO PedManter = (pedidoDAO)Session["PedidoManter"];

                    if (PedManter != null)
                    {
                        Session.Remove("pedido" + urlSessao());
                        Session.Add("pedido" + urlSessao(), PedManter);
                        modalManterDados.Show();

                    }
                    else
                    {
                        novoPedido();
                    }
                }
                if (status.Equals("visualizar"))
                {
                    habilitarCampos(false);
                }
                else
                {
                    habilitarCampos(true);
                }
            }
            else
            {
                if (Request.Params["campoIndex"] != null)  // colocar o campo index da tabela
                {
                    try
                    {
                        if (!IsPostBack)
                        {
                            String index = Request.Params["campoIndex"].ToString();// colocar o campo index da tabela
                            status = "visualizar";
                            pedidoDAO obj = new pedidoDAO(index, 8, usr);
                            Session.Remove("pedido" + urlSessao());
                            Session.Add("pedido" + urlSessao(), obj);
                            carregarDados();
                        }
                        if (status.Equals("visualizar"))
                        {
                            habilitarCampos(false);
                        }
                        else
                        {
                            if (ddlStatus.SelectedValue.Equals("1"))
                            {
                                habilitarCampos(true);
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        msgShow(err.Message, true);
                    }
                }
            }
            carregabtn(pnBtn, null, null, null, null, "Cancelar Pedido", null);

            if (!IsPostBack)
            {
                if (txtCliente_Fornec.Enabled)
                    txtCliente_Fornec.Focus();
            }

        }
        private void novoPedido()
        {
            User usr = (User)Session["User"];
            status = "incluir";
            habilitarCampos(true);
            txtData_cadastro.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtData_entrega.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtUsLogado.Text = usr.getUsuario();
            if (Funcoes.valorParametro("PEDIDO_VDA_VEND_OBRIG", usr) != "TRUE")
            {
                txtfuncionario.Text = usr.getUsuario();
            }
            else
            {
                txtfuncionario.Text = "";
            }

            bool horaBranco = Funcoes.valorParametro("HORA_PEDIDO_VAZIA", usr).Equals("TRUE");
            if (!horaBranco)
                txthora.Text = DateTime.Now.ToString("HH:mm");
            else
                txthora.Text = "";

            txtCFOP.Text = Funcoes.valorParametro("NATUREZA_OP_PEDIDO_VENDA", usr);
            txtCodTbPreco.Text = Funcoes.valorParametro("TABELA_DESCONTO_VENDA", usr);
            txtCentroCusto.Text = Funcoes.valorParametro("CENTRO_CUSTO_PEDIDO_VENDA", usr);
            pedidoDAO obj = (pedidoDAO)Session["pedido" + urlSessao()];

            obj = new pedidoDAO(usr)
            {
                Tipo = 8,
                Status = 1
            };
            Session.Remove("pedido" + urlSessao());
            Session.Add("pedido" + urlSessao(), obj);
            carregarGrids();
            carregabtn(pnBtn, null, null, null, null, "Cancelar Pedido", null);

        }
        private void limparCampos()
        {
            LimparCampos(cabecalho);
            LimparCampos(conteudo);
        }

        protected void btnConfirmaManter_Click(object sender, ImageClickEventArgs e)
        {

            User usr = (User)Session["User"];
            pedidoDAO ped = (pedidoDAO)Session["pedido" + urlSessao()];

            ped.Pedido = "";
            ped.Status = 1;
            ped.Obs = "";
            ped.aproveitaItens();
            Session.Remove("pedido" + urlSessao());
            Session.Add("pedido" + urlSessao(), ped);


            carregarDados();

            status = "incluir";
            txtData_cadastro.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtData_entrega.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtUsLogado.Text = usr.getUsuario();
            if (Funcoes.valorParametro("PEDIDO_VDA_VEND_OBRIG", usr) != "TRUE")
            {
                txtfuncionario.Text = usr.getUsuario();
            }
            else
            {
                txtfuncionario.Text = "";
            }
            txthora.Text = "00:00";
            txtCFOP.Text = Funcoes.valorParametro("NATUREZA_OP_PEDIDO_VENDA", usr);
            txtCodTbPreco.Text = Funcoes.valorParametro("TABELA_DESCONTO_VENDA", usr);
            txtCentroCusto.Text = Funcoes.valorParametro("CENTRO_CUSTO_PEDIDO_VENDA", usr);


            modalManterDados.Hide();
            carregabtn(pnBtn, null, null, null, null, "Cancelar Pedido", null);
            Session.Remove("PedidoManter");
            habilitarCampos(true);

            // EnabledButtons(gridItens, true);
        }

        protected void btnCancelaManter_Click(object sender, ImageClickEventArgs e)
        {
            modalManterDados.Hide();
            novoPedido();
            Session.Remove("PedidoManter");
        }

        protected bool validaCamposObrigatorios()
        {
            pedidoDAO ped = (pedidoDAO)Session["pedido" + urlSessao()];

            if (!IsDate(txtData_entrega.Text))
            {
                txtData_entrega.BackColor = System.Drawing.Color.Red;
                throw new Exception("Data de entrega Inválida");

            }

            if (txthora.Text.Equals(""))
            {
                txthora.BackColor = System.Drawing.Color.Red;
                throw new Exception("Hora de entrega não foi preenchido");
            }


            if (validaCampos(cabecalho) && validaCampos(conteudo))
                return true;
            else
                return false;



        }

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array
            String[] campos = { "txtCFOP",
                                    "txtCliente_Fornec",
                                    "txtfuncionario",
                                    ""
                                     };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array
            User usr = (User)Session["User"];
            bool bDesconto = Funcoes.valorParametro("PED_HAB_DESC", usr).ToUpper().Equals("TRUE");
            bool bStatus = Funcoes.valorParametro("PED_HAB_STATUS", usr).ToUpper().Equals("TRUE");

            String[] campos = { "txtPedido",
                                    (!bStatus?"ddlStatus":""),
                                    "txtUsLogado",
                                    "txtTotal",
                                    "TxtTotalItem",
                                    "txtTotalBruto",
                                    "txtData_cadastro",
                                    (bDesconto?"txtDesconto":""),
                                    "txtHoraCadastro",
                                    "txtDescricaoAddRapito",
                                    "txtEmbAddRapito",
                                    "txtPrecoAddRapito",
                                    //"txtFrete",
                                    "txtDespesas",
                                    "txtLimiteCredito",
                                    "txtLimiteDisponivel",
                                    "txtLimiteUtilizado",
                                    "txtEndereco",
                                    "txtEnderecoNumero",
                                    "txtBairro",
                                    "txtCidade",
                                    "txtUf",
                                    "txtEnderecoComplemento"
                                     };


            return existeNoArray(campos, campo.ID + "");
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            pedidoDAO obj = (pedidoDAO)Session["pedido" + urlSessao()];

            if (obj != null)
            {
                Session.Remove("PedidoManter");
                Session.Add("PedidoManter", obj);
                Session.Remove("pedido" + urlSessao());
            }
            Response.Redirect("~/modulos/pedidos/pages/pedidoOrcamentoDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            if (ddlStatus.SelectedValue.Equals("2") || ddlStatus.SelectedValue.Equals("3"))
            {
                User usr = (User)Session["user"];
                if (usr.adm(usr.tela))
                {
                    status = "editar";
                    txtCliente_Fornec.Enabled = true;
                    txtCliente_Fornec.BackColor = System.Drawing.Color.White;
                    txtNomeCliente.Enabled = true;
                    txtNomeCliente.BackColor = System.Drawing.Color.White;
                    imgBtnCliente.Visible = true;
                }
                else
                {
                    msgShow("SÓ É POSSIVEL FAZER ALTERAÇÕES EM PEDIDOS ABERTOS", true);

                }
            }
            else
            {
                status = "editar";

                habilitarCampos(true);
                carregarDados();

            }
            carregabtn(pnBtn, null, null, null, null, "Cancelar Pedido", null);
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("PedidoOrcamento.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            if (!ddlStatus.SelectedValue.Equals("3"))
            {
                modalPnConfirma.Show();
            }
            else
            {
                msgShow("PEDIDO JÁ ESTA CANCELADO", true);
            }
        }

        protected void Salvar(bool limitAutorizado)
        {
            try
            {



                if (validaCamposObrigatorios())
                {
                    pedidoDAO obj = (pedidoDAO)Session["pedido" + urlSessao()];
                 
                    if (status.Equals("incluir"))
                        txtHoraCadastro.Text = DateTime.Now.ToString("hh:mm");


                    carregarDadosObj();
                    obj.salvar(status.Equals("incluir")); ;
                    msgShow("Salvo com Sucesso", false);
                    status = "visualizar";
                    visualizar(pnBtn);
                    Session.Remove("pedido" + urlSessao());
                    Session.Add("pedido" + urlSessao(), obj);
                    carregarDados();
                    habilitarCampos(false);

                }
                else
                {
                    msgShow("Campo Obrigatorio não preenchido", true);
                    lblError.ForeColor = System.Drawing.Color.Red;

                }
            }
            catch (Exception err)
            {
                msgShow(err.Message, true);

            }

        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            Salvar(false);
        }

        protected void habilitarCampos(bool hab)
        {
            EnabledControls(cabecalho, hab);
            EnabledControls(conteudo, hab);
            //EnabledControls(pnFundo, hab);
            EnabledControls(PnAddPagamento, hab);
            EnabledControls(pnItens, hab);

            EnabledButtons(gridItens, hab);
            EnabledButtons(gridPagamentos, hab);

            if (status.Equals("visualizar"))
            {
                btnImpressao.Visible = true;
                pedidoDAO obj = (pedidoDAO)Session["pedido" + urlSessao()];
                Session.Remove("PedidoPrint");
                Session.Add("PedidoPrint", obj);
            }
            else
            {
                btnImpressao.Visible = false;
                
            }
            User usr = (User)Session["User"];
            bool HabDesconto = Funcoes.valorParametro("PEDIDO_TAB_DESCONTO", usr).ToUpper().Equals("TRUE");
            if (!HabDesconto)
            {
                txtCodTbPreco.Enabled = false;
                imgCodTbPreco.Visible = false;
            }
            txtDiasParcelas.Enabled = true;
            txtDiasParcelas.BackColor = System.Drawing.Color.White;

        }


        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("pedidoOrcamento.aspx");//colocar endereco pagina de pesquisa
        }

        protected void txt_TextChanged(object sender, EventArgs e)
        {

            ModalItens.Show();

        }

        private void carregarDados()
        {
            pedidoDAO obj = (pedidoDAO)Session["pedido" + urlSessao()];
            txtPedido.Text = (obj.Pedido == null ? "" : obj.Pedido.ToString());
            ddlStatus.SelectedValue = obj.Status.ToString();
            txtCliente_Fornec.Text = obj.Cliente_Fornec.ToString();
            txtNomeCliente.Text = obj.NomeCliente;
            txtEndereco.Text = obj.cliente.Endereco;
            txtEnderecoNumero.Text = obj.cliente.endereco_nro;
            txtEnderecoComplemento.Text = obj.cliente.complemento_end;
            txtBairro.Text = obj.cliente.Bairro;
            txtCidade.Text = obj.cliente.Cidade;
            txtUf.Text = obj.cliente.UF;

            txtData_cadastro.Text = obj.Data_cadastroBr();
            txtData_entrega.Text = obj.Data_entregaBr();
            txthora.Text = obj.hora.ToString();
            txtCodTbPreco.Text = obj.TabelaPreco;
            txtUsLogado.Text = obj.Usuario.ToString();
            txtObs.Text = obj.Obs.ToString();
            txtCFOP.Text = obj.CFOP.ToString();
            txtfuncionario.Text = obj.funcionario.ToString();
            txtCentroCusto.Text = obj.centro_custo;
            ddlEntrega.Text = (obj.entrega ? "ENTREGA" : "RETIRA");
            txtHoraCadastro.Text = obj.hora_cadastro;
            ddlIntermediador.SelectedValue = obj.indIntermed.ToString();
            txtIntermedCnpj.Text = obj.intermedCnpj;
            txtIdCadIntTran.Text = obj.idCadIntTran;
            txtCnpjPagamento.Text = obj.CNPJPagamento;


            carregarGrids();

            txtTotal.Text = string.Format("{0:0,0.00}", obj.Total);
            txtTotalBruto.Text = obj.totalBruto.ToString("N2");
            txtDesconto.Text = string.Format("{0:0,0.00}", obj.Desconto);
            txtFrete.Text = string.Format("{0:0,0.00}", obj.Frete);
            txtDespesas.Text = obj.Despesas.ToString("N2");
            calculaLimiteCliente();

        }

        protected void gridItens_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //for (int i = 0; i < e.Row.Cells.Count; i++)
                //{
                //    if (isnumero(e.Row.Cells[i].Text))
                //    {
                //        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                //        e.Row.Cells[i].Text = Decimal.Parse(e.Row.Cells[i].Text).ToString("N2");

                //    }
                //}
            }
        }

        protected void gridPagamentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (isnumero(e.Row.Cells[i].Text))
                    {
                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                        e.Row.Cells[i].Text = Decimal.Parse(e.Row.Cells[i].Text).ToString("N2");

                    }
                }
            }
        }

        protected void btnConfirmaItens_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                txtObsItem.BackColor = System.Drawing.Color.White;
                if (txtObsItem.Text.Length > 255)
                {
                    txtObsItem.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Observação pode conter até 255 caracteres");

                }
                if (chkProduzirItem.Checked)
                {
                    DateTime dtEntrega = Funcoes.dtTry(txtData_entrega.Text + " " + txthora.Text);
                    DateTime dtProducao = Funcoes.dtTry(txtDataProducaoItem.Text + " " + txtHoraProducaoItem.Text);

                    if (dtProducao > dtEntrega)
                    {
                        throw new Exception("A Data e hora de produção não pode ser maior que a data de entrega");
                    }
                }


                pedidoDAO ped = (pedidoDAO)Session["pedido" + urlSessao()];
                User usr = (User)Session["User"];
                pedido_itensDAO item = new pedido_itensDAO(usr);

                Decimal vlrTotal = item.total;

                item.Filial = ped.Filial;
                item.Pedido = ped.Pedido;
                item.Tipo = ped.Tipo;
                item.PLU = txtPLU.Text;
                item.Qtde = Decimal.Parse(txtQtde.Text);
                item.Embalagem = Decimal.Parse(txtEmbalagem.Text);
                item.unitario = Decimal.Parse(txtUnitario.Text);
                item.Desconto = Decimal.Parse(txtDescontoItem.Text);
                item.index = int.Parse(lblIndex.Text);
                item.num_item = item.index;
                item.obs = txtObsItem.Text;
                item.produzir = chkProduzirItem.Checked;
                item.data_hora_produzir = Funcoes.dtTry(txtDataProducaoItem.Text + " " + txtHoraProducaoItem.Text);

                if (vlrTotal != item.total)
                {
                    ped.PedPg.Clear();

                }

                bool precoMenor = Funcoes.valorParametro("PED_N_PRECO_MENOR", usr).ToUpper().Equals("TRUE");
                if (precoMenor)
                {
                    if (item.unitario < item.precoMinimo)
                    {
                        lblerroItem.Text = "O Item não pode ter o preco menor que " + item.precoMinimo.ToString("N2");
                        ModalItens.Show();
                        txtUnitario.Focus();
                    }
                    else
                    {
                        ped.atualizaitem(item);
                        ModalItens.Hide();
                        carregarDados();

                    }
                }
                else
                {
                    ped.atualizaitem(item);
                    ModalItens.Hide();
                    carregarDados();
                }

                //Validação para 
                item.agrupamento = ddlAgrupamento.Text;

                //Validação para marcar se o item é novo
                if (chkInserido.Checked)
                {
                    item.inserido = true;
                }

                //Parametro para obrigar agrupamento no pedido de venda produção.
                bool obrigaAgrupamento = Funcoes.valorParametro("PED_PRODUZIR_OBRIGA_AGRUP", usr).ToUpper().Equals("TRUE");

                if (obrigaAgrupamento)
                {
                    if (item.produzir && item.agrupamento.Equals(""))
                    {
                        lblerroItem.Text = "Para item a ser produzido, o agrupamento é obrigatório";
                        ModalItens.Show();
                        ddlAgrupamento.Focus();
                    }
                }





            }
            catch (Exception err)
            {
                lblerroItem.Text = err.Message;
                ModalItens.Show();
            }
        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {

            String itemLista = (String)Session["camporecebe" + urlSessao()];
            if (itemLista != null && itemLista.Equals("PLU"))
            {
                ModalItens.Show();
            }

            modalPnFundo.Hide();

        }

        protected void btnConfirmaPagamentos_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                pedidoDAO ped = (pedidoDAO)Session["pedido" + urlSessao()];
                User usr = (User)Session["User"];

                int qtdePag = Funcoes.intTry(txtParcelas.Text);
                DateTime dtVencimento = Funcoes.dtTry(txtVencimentoPg.Text);
                if (qtdePag > 1)
                {
                    int diasParc = Funcoes.intTry(txtDiasParcelas.Text);
                    Decimal totalPedido = Funcoes.decTry(txtTotal.Text);
                    Decimal vlrParc = Decimal.Round((totalPedido / qtdePag), 2);

                    int diasStringParc = diasParc;
                    Decimal vltTotalParcelas = 0;
                    for (int i = 0; i < qtdePag; i++)
                    {
                        vltTotalParcelas += Decimal.Round(vlrParc, 2);
                        if (i == (qtdePag - 1))
                        {
                            if (vltTotalParcelas != ped.Total)
                            {
                                vlrParc += (ped.Total - vltTotalParcelas);
                            }
                        }

                        pedido_pagamentoDAO pg = new pedido_pagamentoDAO(usr);
                        pg.Tipo_pagamento = txtTipoPg.Text;
                        pg.Vencimento = dtVencimento;
                        pg.Valor = vlrParc;
                        ped.addPagamentos(pg);
                        dtVencimento = dtVencimento.AddDays(diasParc);
                        diasStringParc += diasParc;

                    }
                }
                else
                {
                    pedido_pagamentoDAO pg = new pedido_pagamentoDAO(usr);
                    pg.Tipo_pagamento = txtTipoPg.Text;
                    pg.Vencimento = dtVencimento;
                    pg.Valor = Funcoes.decTry(txtValorPg.Text);
                    ped.addPagamentos(pg);
                }

                Session.Remove("pedido" + urlSessao());
                Session.Add("pedido" + urlSessao(), ped);
                carregarGrids();
                ModalPagamentos.Hide();
            }
            catch (Exception)
            {
                lblErroPg.Text = "Pagamento Invalido";
                ModalPagamentos.Show();
            }


        }

        protected void btnCancelaItem_Click(object sender, ImageClickEventArgs e)
        {
            ModalItens.Hide();
            carregarGrids();
            Session.Remove("item" + urlSessao());
        }

        protected void ImgExcluiItem_Click(object sender, ImageClickEventArgs e)
        {
            pedidoDAO ped = (pedidoDAO)Session["pedido" + urlSessao()];
            User usr = (User)Session["User"];
            pedido_itensDAO itempd = new pedido_itensDAO(usr);
            itempd.Filial = ped.Filial;
            itempd.Pedido = ped.Pedido;
            itempd.PLU = txtPLU.Text;
            itempd.index = int.Parse(lblIndex.Text);
            Decimal valor = (txtDesconto.Text.Trim().Equals("") ? 0 : Decimal.Parse(txtDesconto.Text));
            ped.removeItem(itempd);

            ped.aplicarDesconto(valor);
            ped.PedPg.Clear();

            Session.Remove("pedido" + urlSessao());
            Session.Add("pedido" + urlSessao(), ped);
            carregarDados();

            //carregarGrids();
            //PnExcluirItem.Visible = false;
        }

        public void carregarGrids()
        {
            try
            {
                pedidoDAO obj = (pedidoDAO)Session["pedido" + urlSessao()];
                gridItens.DataSource = obj.PdItens();
                gridItens.DataBind();

                gridPagamentos.DataSource = obj.PdPag();
                gridPagamentos.DataBind();
            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
            }
        }
        protected void btnCancelaPagamentos_Click(object sender, ImageClickEventArgs e)
        {
            ModalPagamentos.Hide();
            carregarGrids();
        }

        protected void ImgBtnAddItens_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                carregarDadosObj();
                if (txtPluAddRapido.Text.Trim().Equals("") && txtRefAddRapido.Text.Trim().Equals(""))
                {
                    Session.Remove("btnFechaMercadoria");
                    // PnExcluirItem.Visible = false;

                    Session.Remove("selecionados" + urlSessao());
                    carregarMercadorias();
                    txtfiltromercadoria.Focus();
                }
                else if (!txtQtdeAddRapito.Text.Equals(""))
                {
                    pedidoDAO obj = (pedidoDAO)Session["pedido" + urlSessao()];

                    User usr = (User)Session["User"];
                    pedido_itensDAO item = new pedido_itensDAO(usr);

                    item.PLU = txtPluAddRapido.Text;
                    item.Qtde = (txtQtdeAddRapito.Text.Equals("") ? 1 : Decimal.Parse(txtQtdeAddRapito.Text));

                    item.Embalagem = Funcoes.decTry(txtEmbAddRapito.Text);
                    if (item.Embalagem == 0)
                        item.Embalagem = 1;
                    item.unitario = Funcoes.decTry(txtPrecoAddRapito.Text);
                    item.inserido = true;
                    item.Desconto = 0;
                    item.vPrecoMinimo = Funcoes.decTry(txtPrecoAddRapito.Text);
                    obj.addItens(item);
                    obj.PedPg.Clear();
                    Session.Remove("pedido" + urlSessao());
                    Session.Add("pedido" + urlSessao(), obj);
                    carregarDados();


                    txtPluAddRapido.Text = "";
                    txtRefAddRapido.Text = "";
                    txtDescricaoAddRapito.Text = "";
                    txtEmbAddRapito.Text = "";
                    txtPrecoAddRapito.Text = "";
                    txtQtdeAddRapito.Text = "";
                    txtPluAddRapido.Focus();
                }
                else
                {

                    txtQtdeAddRapito.Focus();
                    carregarPlu();
                }



            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
            }
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static decimal[] getValorUnitarioPlu(string plu, decimal qtde, string cod_cliente, decimal precoEditado)
        {
            User usr = (User)HttpContext.Current.Session["User"];
            //decimal precoEditado = 0;
            precoEditado = 0;
            decimal unitario = 0;
            if (precoEditado > 0)
            {
                unitario = precoEditado;
            }
            else
            {
                unitario = Funcoes.valorProduto(plu, qtde, cod_cliente, usr);
            }

            return new decimal[1] { unitario };
        }

        protected void carregarPlu()
        {
            User usr = (User)Session["User"];
            SqlDataReader rsplu = null;
            try
            {



                rsplu = Conexao.consulta("Select top 1 mercadoria.PLU,mercadoria.descricao,mercadoria.embalagem," +
                    " CASE WHEN (convert(date,getdate()) between  Mercadoria_loja.Data_Inicio AND Mercadoria_loja.Data_Fim ) AND ISNULL(Mercadoria_Loja.Preco_Promocao,0) > 0 THEN Mercadoria_loja.Preco_Promocao ELSE Mercadoria_Loja.Preco END as [Preco]" +
                    ",mercadoria.ref_fornecedor " +
                    "from mercadoria inner join mercadoria_loja on mercadoria.plu=mercadoria_loja.plu left join ean on mercadoria.plu=ean.plu where mercadoria_loja.filial='" + usr.getFilial() + "' and ( mercadoria.plu='" + txtPluAddRapido.Text + "' or ean.EAN='" + txtPluAddRapido.Text + "')", null, false);
                if (rsplu.Read())
                {
                    txtPluAddRapido.Text = rsplu["plu"].ToString();
                    txtRefAddRapido.Text = rsplu["ref_fornecedor"].ToString();

                    txtDescricaoAddRapito.Text = rsplu["descricao"].ToString();
                    txtEmbAddRapito.Text = Funcoes.decTry(rsplu["embalagem"].ToString()).ToString();
                    txtPrecoAddRapito.Text = Funcoes.decTry(rsplu["preco"].ToString()).ToString("N2");
                }
                else
                {
                    if (rsplu != null)
                        rsplu.Close();
                    rsplu = Conexao.consulta("Select top 1 mercadoria.PLU,mercadoria.descricao,mercadoria.embalagem, " +
                        " CASE WHEN (convert(date,getdate()) between  Mercadoria_loja.Data_Inicio AND Mercadoria_loja.Data_Fim ) AND ISNULL(Mercadoria_Loja.Preco_Promocao,0) > 0 THEN Mercadoria_loja.Preco_Promocao ELSE Mercadoria_Loja.Preco END as [Preco]" +
                        ",mercadoria.ref_fornecedor " +
                    "from mercadoria inner join mercadoria_loja on mercadoria.plu=mercadoria_loja.plu left join ean on mercadoria.plu=ean.plu where mercadoria_loja.filial='" + usr.getFilial() + "' and ( mercadoria.Ref_fornecedor='" + txtRefAddRapido.Text + "')", null, false);

                    if (rsplu.Read())
                    {
                        txtPluAddRapido.Text = rsplu["plu"].ToString();
                        txtRefAddRapido.Text = rsplu["ref_fornecedor"].ToString();
                        txtDescricaoAddRapito.Text = rsplu["descricao"].ToString();
                        txtEmbAddRapito.Text = Funcoes.decTry(rsplu["embalagem"].ToString()).ToString();
                        txtPrecoAddRapito.Text = Funcoes.decTry(rsplu["preco"].ToString()).ToString("N2");
                    }
                    else
                    {
                        txtPluAddRapido.Text = "";
                        txtRefAddRapido.Text = "";
                        txtDescricaoAddRapito.Text = "";
                        txtQtde.Text = "";
                        txtEmbAddRapito.Text = "";
                        txtPrecoAddRapito.Text = "";

                        msgShow("Produto não encontrado!!", true);

                        txtPluAddRapido.Focus();

                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rsplu != null)
                    rsplu.Close();
            }

        }
        protected void ImgBtnAddPagamentos_Click(object sender, ImageClickEventArgs e)
        {
            carregarDadosObj();
            pedidoDAO ped = (pedidoDAO)Session["pedido" + urlSessao()];
            txtValorPg.Text = ((ped.vTotal + ped.Frete) - ped.totalPagamentos()).ToString("N2");
            txtTipoPg.Text = "";
            txtVencimentoPg.Text = "";
            txtParcelas.Text = "1";
            txtDiasParcelas.Text = "30";
            txtParcelas.Enabled = false;
            txtDiasParcelas.Enabled = false;
            ModalPagamentos.Show();
        }

        protected void carregarMercadorias()
        {
            lblMercadoriaLista.Text = "Inclusão de Produto";
            lblMercadoriaLista.ForeColor = Label1.ForeColor;

            User usr = (User)Session["user"];
            String sqlMercadoria = "Select mercadoria.plu PLU,isnull(ean.ean,'---')EAN,mercadoria.Ref_fornecedor REFERENCIA, Replace(mercadoria.descricao,' ','&nbsp')  DESCRICAO, " +
                                    " CASE WHEN (convert(date,getdate()) between  Mercadoria_loja.Data_Inicio AND Mercadoria_loja.Data_Fim ) AND ISNULL(Mercadoria_Loja.Preco_Promocao,0) > 0 THEN Mercadoria_loja.Preco_Promocao ELSE Mercadoria_Loja.Preco END as [PRC VENDA], " +
                                    " mercadoria_loja.saldo_atual SALDO,mercadoria.peso_bruto as [PESO BRUTO] from mercadoria inner join mercadoria_loja on mercadoria.plu = mercadoria_loja.plu left join ean on mercadoria.plu=ean.plu " +
                                    " where (mercadoria_loja.filial='" + usr.getFilial() + "')  AND (ISNULL(Mercadoria.Inativo, 0) = 0) ";
            if (ddlTipoPesquisaMercadoria.Text.Equals("PLU"))
                sqlMercadoria += "and (mercadoria.plu like '" + txtfiltromercadoria.Text + "%')";
            else
                sqlMercadoria += " and ((ean like '%" + txtfiltromercadoria.Text + "%') or (mercadoria.descricao like '%" + txtfiltromercadoria.Text + "%') or mercadoria.Ref_fornecedor like '%" + txtfiltromercadoria.Text + "%')";

            if (Funcoes.valorParametro("PEDIDO_SOCOMESTOQUE", usr).ToUpper() == "TRUE")
            {
                sqlMercadoria = sqlMercadoria + " AND Mercadoria_Loja.Saldo_Atual > 0";
            }
            if (ddlTipoPesquisaMercadoria.Text.Equals("PLU"))
                sqlMercadoria += " ORDER BY convert(numeric(20),mercadoria.plu) ";
            else

                sqlMercadoria += " ORDER BY RTRIM(LTRIM(mercadoria.descricao)) ";


            //if Funcoes.valorParametro("PEDIDO_SIMPLES", usr).ToUpper()
            //voltar aqui 22042015

            gridMercadoria1.DataSource = Conexao.GetTable(sqlMercadoria, usr, !(txtfiltromercadoria.Text.Length > 0));
            gridMercadoria1.DataBind();
            ArrayList todosSel = (ArrayList)Session["selecionados" + urlSessao()];
            if (todosSel == null)
            {
                todosSel = new ArrayList();
                ArrayList cabecalho = new ArrayList();
                cabecalho.Add("PLU");
                cabecalho.Add("Referencia");
                cabecalho.Add("Descricao");
                cabecalho.Add("QTD");
                cabecalho.Add("embalagem");
                cabecalho.Add("Preco");
                cabecalho.Add("PrecoPadrao");
                cabecalho.Add("Peso_Bruto");
                todosSel.Add(cabecalho);
            }

            if (todosSel.Count > 1)
            {

                for (int i = 0; i < GridMercadoriaSelecionado.Rows.Count; i++)
                {
                    TextBox txtQtdItem = (TextBox)GridMercadoriaSelecionado.Rows[i].FindControl("txtQtd");
                    if (!txtQtdItem.Text.Equals("------"))
                    {
                        ArrayList ls = (ArrayList)todosSel[i + 1];

                        TextBox txtPreco = (TextBox)GridMercadoriaSelecionado.Rows[i].FindControl("txtPreco");


                        ls[3] = txtQtdItem.Text;
                        ls[5] = txtPreco.Text;


                    }
                }
            }
            Session.Remove("selecionados" + urlSessao());
            Session.Add("selecionados" + urlSessao(), todosSel);
            GridMercadoriaSelecionado.DataSource = Conexao.GetArryTable(todosSel);
            GridMercadoriaSelecionado.DataBind();

            modalMercadorialista.Show();
            carregarGrids();

        }
        protected void gridItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            lblerroItem.Text = "";
            LimparCampos(pnItens);
            pedidoDAO ped = (pedidoDAO)Session["pedido" + urlSessao()];
            int index = Convert.ToInt32(e.CommandArgument);
            pedido_itensDAO itemPd = ped.item(index);
            carregarDadosObj();
            carregarDadosItens(itemPd);
            //PnExcluirItem.Visible = true;
        }

        protected void gridPagamentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            pedidoDAO ped = (pedidoDAO)Session["pedido" + urlSessao()];
            int index = Convert.ToInt32(e.CommandArgument);//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            pedido_pagamentoDAO pgPd = ped.pagamento(index);
            ped.removePG(pgPd);
            carregarGrids();
        }

        protected void carregarDadosItens(pedido_itensDAO itemPd)
        {
            txtPLU.Text = itemPd.PLU;
            txtDescricao.Text = itemPd.Descricao;
            txtQtde.Text = itemPd.Qtde.ToString("N3");
            txtEmbalagem.Text = itemPd.Embalagem.ToString("N2");
            txtUnitario.Text = itemPd.unitario.ToString("N2");
            txtObsItem.Text = itemPd.obs;
            txtDescontoItem.Text = itemPd.Desconto.ToString("N2");
            TxtTotalItem.Text = itemPd.total.ToString("N2");
            lblIndex.Text = itemPd.index.ToString();
            chkProduzirItem.Checked = itemPd.produzir;
            txtDataProducaoItem.Enabled = itemPd.produzir;
            txtHoraProducaoItem.Enabled = itemPd.produzir;

            //Tratamento para controlar qdo o item é novo
            //Estre tratamento deve-se ao fato de caso o usuario insirar um novo item e ao ser inserido na grid vc o edite, o sistema deixa o INCLUIDO=false, por
            //isso foi incluído um check que fara o tratamento.
            if (itemPd.inserido)
            {
                chkInserido.Checked = true;
            }
            else
            {
                chkInserido.Checked = false;
            }

            ddlAgrupamento.Enabled = itemPd.produzir;
            if (itemPd.produzir)
            {
                txtDataProducaoItem.Text = itemPd.data_hora_produzir.ToString("dd/MM/yyyy");
                txtHoraProducaoItem.Text = itemPd.data_hora_produzir.ToString("HH:mm:ss");
                txtDataProducaoItem.BackColor = txtDescricao.BackColor;
                txtHoraProducaoItem.BackColor = txtDescricao.BackColor;
                ddlAgrupamento.Text = itemPd.agrupamento;
            }
            else
            {
                txtDataProducaoItem.Text = "";
                txtHoraProducaoItem.Text = "00:00:00";
                txtDataProducaoItem.BackColor = TxtTotalItem.BackColor;
                txtHoraProducaoItem.BackColor = TxtTotalItem.BackColor;
                ddlAgrupamento.Text = itemPd.agrupamento;
            }
            ScriptManager.RegisterClientScriptBlock(
                    Page,
                    Page.GetType(),
                    "contaCaracters",
                    "limite_textarea('" + itemPd.obs + "')",
                    true);
            ModalItens.Show();
        }
        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            pedidoDAO obj = (pedidoDAO)Session["pedido" + urlSessao()];
            obj.Pedido = txtPedido.Text;
            obj.Status = int.Parse(ddlStatus.SelectedValue);
            obj.Cliente_Fornec = txtCliente_Fornec.Text;

            obj.Data_cadastro = DateTime.Parse(txtData_cadastro.Text);
            obj.Data_entrega = DateTime.Parse(txtData_entrega.Text);
            obj.hora = txthora.Text;
            obj.Desconto = (txtDesconto.Text.Equals("") ? 0 : Decimal.Parse(txtDesconto.Text));
            obj.Total = (txtTotal.Text.Equals("") ? 0 : Decimal.Parse(txtTotal.Text));
            obj.Usuario = txtUsLogado.Text;
            obj.Obs = txtObs.Text;
            obj.CFOP = (txtCFOP.Text.Equals("") ? 0 : Decimal.Parse(txtCFOP.Text));
            obj.funcionario = txtfuncionario.Text;
            obj.TabelaPreco = txtCodTbPreco.Text;
            obj.centro_custo = txtCentroCusto.Text;
            obj.entrega = ddlEntrega.Text.Equals("ENTREGA");
            obj.hora_cadastro = txtHoraCadastro.Text;
            obj.Frete = (txtFrete.Text.Equals("") ? 0 : Decimal.Parse(txtFrete.Text));
            obj.Despesas = Funcoes.decTry(txtDespesas.Text);
            obj.indIntermed = Funcoes.intTry(ddlIntermediador.SelectedValue);
            obj.intermedCnpj = txtIntermedCnpj.Text;
            obj.idCadIntTran = txtIdCadIntTran.Text;
            obj.CNPJPagamento = txtCnpjPagamento.Text;


            Session.Remove("pedido" + urlSessao());
            Session.Add("pedido" + urlSessao(), obj);
        }

        protected void exibeLista()
        {

            lblErroPesquisa.Text = "";
            User usr = (User)Session["User"];
            String or = (String)Session["camporecebe" + urlSessao()];
            String sqlLista = "";

            ddlTipoPesquisa.Visible = false;
            switch (or)
            {
                case "Cliente":
                    sqlLista = "select codigo_cliente Codigo, Nome_Cliente Nome from Cliente where (codigo_cliente like '%" + TxtPesquisaLista.Text + "%' or nome_cliente like '%" + TxtPesquisaLista.Text + "%') and isnull(inativo,0)=0"; ;
                    lbllista.Text = "Escolha um Cliente";
                    break;
                case "PLU":
                case "PLURapido":
                    ddlTipoPesquisa.Visible = true;
                    sqlLista = "select Plu, Descricao from mercadoria where plu like '%" + TxtPesquisaLista.Text + "%' or descricao like '%" + TxtPesquisaLista.Text + "%' ORDER BY " + (ddlTipoPesquisa.Text.Equals("PLU") ? "CONVERT(INT,PLU)" : " DESCRICAO") + " DESC";
                    lbllista.Text = "Escolha um Produto";
                    break;
                case "Funcionario":
                    sqlLista = "select Nome from funcionario where nome like '%" + TxtPesquisaLista.Text + "%'";
                    lbllista.Text = "Escolha um Funcionario";
                    break;
                case "CFOP":
                    sqlLista = "select codigo_operacao, descricao from natureza_operacao where saida=1 and codigo_operacao like '%" + TxtPesquisaLista.Text + "%' and descricao like '%" + TxtPesquisaLista.Text + "%'";
                    lbllista.Text = "Escolha a Natureza de Operação";
                    break;
                case "TipoPagamento":
                    sqlLista = "select tipo_pagamento Tipo ,Prazo,Parcelado = case when isnull(parcelamento,0)=1 then 'SIM' ELSE 'NAO' END from tipo_pagamento where tipo_pagamento like '%" + TxtPesquisaLista.Text + "%'  order by a_vista desc ,Tipo";
                    lbllista.Text = "Escolha a Tipo de pagamento";
                    break;
                case "txtCodTbPreco":
                    lbllista.Text = "Escolha uma Tabela de Preços";
                    sqlLista = "select Codigo_tabela Codigo,porc Desconto from tabela_preco  where codigo_tabela like '%" + TxtPesquisaLista.Text + "%' or porc like '%" + TxtPesquisaLista.Text + "%'";

                    break;
                case "txtCentroCusto":
                    lbllista.Text = "Escolha um Centro de custo";
                    sqlLista = "SELECT  codigo_centro_custo as Codigo , descricao_centro_custo as Descricao  from centro_custo where (codigo_centro_custo like'" + TxtPesquisaLista.Text + "%') or (descricao_centro_custo like '%" + TxtPesquisaLista.Text + "%') and( modalidade='RECEITAS')";
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

        protected void Img_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            TxtPesquisaLista.Text = "";
            Session.Remove("camporecebe" + urlSessao());
            switch (btn.ID)
            {
                case "PLU":
                case "btnimg_txtPLU":
                    Session.Add("camporecebe" + urlSessao(), "PLU");
                    break;
                case "ImgBtn_txtPluAddRapido":
                    Session.Add("camporecebe" + urlSessao(), "PLURapido");
                    break;
                case "imgBtnCliente":
                    Session.Add("camporecebe" + urlSessao(), "Cliente");
                    break;
                case "imgBtnFuncionario":
                    Session.Add("camporecebe" + urlSessao(), "Funcionario");
                    break;
                case "imgBtnCfop":
                    Session.Add("camporecebe" + urlSessao(), "CFOP");
                    break;
                case "btnimg_txtTipoPg":
                    Session.Add("camporecebe" + urlSessao(), "TipoPagamento");
                    break;
                case "imgCodTbPreco":
                    Session.Add("camporecebe" + urlSessao(), "txtCodTbPreco");
                    break;
                case "btnimg_txtcentro_custo":
                    Session.Add("camporecebe" + urlSessao(), "txtCentroCusto");
                    break;
            }

            exibeLista();


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
                    User usr = (User)Session["User"];

                    lbllista.Text = "";
                    String listaAtual = (String)Session["camporecebe" + urlSessao()];
                    Session.Remove("camporecebe" + urlSessao());

                    if (listaAtual.Equals("Cliente"))
                    {

                        txtCliente_Fornec.Text = ListaSelecionada(1);
                        txtNomeCliente.Text = ListaSelecionada(2);
                        pedidoDAO obj = (pedidoDAO)Session["pedido" + urlSessao()];
                        obj.Cliente_Fornec = txtCliente_Fornec.Text;
                        obj.cliente = new ClienteDAO(obj.Cliente_Fornec, usr);
                        txtNomeCliente.Text = obj.NomeCliente;
                        txtEndereco.Text = obj.cliente.Endereco;
                        txtEnderecoNumero.Text = obj.cliente.endereco_nro;
                        txtEnderecoComplemento.Text = obj.cliente.complemento_end;
                        txtBairro.Text = obj.cliente.Bairro;
                        txtCidade.Text = obj.cliente.Cidade;
                        txtUf.Text = obj.cliente.UF;

                        Session.Remove("pedido" + urlSessao());
                        Session.Add("pedido" + urlSessao(), obj);
                        calculaLimiteCliente();
                    }
                    else if (listaAtual.Equals("PLU"))
                    {

                        MercadoriaDAO merc = new MercadoriaDAO(ListaSelecionada(1), usr);
                        txtPLU.Text = merc.PLU;
                        txtDescricao.Text = merc.Descricao;
                        txtEmbalagem.Text = merc.Embalagem.ToString("N2");
                        txtUnitario.Text = merc.preco_compra.ToString("N2");

                        TxtTotalItem.Text = ((Decimal.Parse(txtQtde.Text) * Decimal.Parse(txtEmbalagem.Text)) * Decimal.Parse(txtUnitario.Text)).ToString("N2");
                        ModalItens.Show();

                    }
                    else if (listaAtual.Equals("Funcionario"))
                    {

                        txtfuncionario.Text = ListaSelecionada(1);
                    }
                    else if (listaAtual.Equals("CFOP"))
                    {

                        txtCFOP.Text = ListaSelecionada(1);
                    }
                    else if (listaAtual.Equals("TipoPagamento"))
                    {

                        txtTipoPg.Text = ListaSelecionada(1);

                        txtDiasParcelas.Text = ListaSelecionada(2);
                        bool parcelamento = ListaSelecionada(3).Equals("SIM");


                        txtDiasParcelas.Enabled = parcelamento;
                        txtParcelas.Enabled = parcelamento;
                        if (!parcelamento)
                        {
                            txtParcelas.Text = "1";

                        }



                        txtVencimentoPg.Text = DateTime.Now.AddDays(int.Parse(ListaSelecionada(2))).ToString("dd/MM/yyyy");
                        ModalPagamentos.Show();


                    }
                    else if (listaAtual.Equals("txtCodTbPreco"))
                    {
                        pedidoDAO ped = (pedidoDAO)Session["pedido" + urlSessao()];
                        txtCodTbPreco.Text = ListaSelecionada(1);
                        ped.TabelaPreco = txtCodTbPreco.Text;
                        txtTotal.Text = ped.Total.ToString("N2");
                        txtDesconto.Text = ped.Desconto.ToString("N2");
                        txtFrete.Text = ped.Frete.ToString("N2");
                        Session.Remove("pedido" + urlSessao());
                        Session.Add("pedido" + urlSessao(), ped);

                    }
                    else if (listaAtual.Equals("txtCentroCusto"))
                    {
                        txtCentroCusto.Text = ListaSelecionada(1);
                    }
                    else if (listaAtual.Equals("PLURapido"))
                    {
                        txtPluAddRapido.Text = ListaSelecionada(1);
                        carregarPlu();
                        txtQtdeAddRapito.Focus();
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

        private void calculaLimiteCliente()
        {
            pedidoDAO ped = (pedidoDAO)Session["pedido" + urlSessao()];
            txtLimiteCredito.Text = ped.cliente.Limite_Credito.ToString("N2");
            Decimal UtilizadoTotal = ped.cliente.cadernetaSaldo() + ped.cliente.totalUtilizado();
            Decimal Disp = (ped.cliente.Limite_Credito - UtilizadoTotal);
            txtLimiteUtilizado.Text = UtilizadoTotal.ToString("N2");
            txtLimiteDisponivel.Text = Disp.ToString("N2");
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

        protected void btnFecharMecadoria_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                String str = (String)Session["btnFechaMercadoria"];
                if (str == null)
                {
                    Session.Add("btnFechaMercadoria", "true");

                    User usr = (User)Session["User"];
                    ArrayList selecionados = (ArrayList)Session["selecionados" + urlSessao()];
                    if (selecionados != null && selecionados.Count > 1)
                    {

                        pedidoDAO obj = (pedidoDAO)Session["pedido" + urlSessao()];
                        bool precoMenor = Funcoes.valorParametro("PED_N_PRECO_MENOR", usr).ToUpper().Equals("TRUE");
                        if (precoMenor)
                        {
                            foreach (GridViewRow sl in GridMercadoriaSelecionado.Rows)
                            {
                                TextBox txtPrecoItem = (TextBox)sl.FindControl("txtPreco");
                                Label lblPreco = (Label)sl.FindControl("lblPreco");
                                Decimal tPreco = Decimal.Parse(txtPrecoItem.Text);
                                Decimal lPreco = Decimal.Parse(lblPreco.Text);
                                if (tPreco < lPreco)
                                {
                                    txtPrecoItem.Focus();
                                    txtPrecoItem.BackColor = System.Drawing.Color.Red;
                                    throw new Exception("O Item " + sl.Cells[1].Text + " não pode ter o preço menor que " + lPreco.ToString("N2"));
                                }

                            }
                        }
                        Decimal valorDesconto = (txtDesconto.Text.Trim().Equals("") ? 0 : Decimal.Parse(txtDesconto.Text));

                        foreach (GridViewRow sl in GridMercadoriaSelecionado.Rows)
                        {
                            pedido_itensDAO item = new pedido_itensDAO(usr);

                            item.PLU = sl.Cells[1].Text;
                            TextBox txtQtdItem = (TextBox)sl.FindControl("txtQtd");
                            if (!txtQtdItem.Text.Equals("------"))
                            {
                                item.Qtde = (txtQtdItem.Text.Equals("") ? 1 : Decimal.Parse(txtQtdItem.Text));
                                TextBox txtPrecoItem = (TextBox)sl.FindControl("txtPreco");
                                Label lblPreco = (Label)sl.FindControl("lblPreco");
                                item.Embalagem = decimal.Parse(sl.Cells[5].Text);
                                item.unitario = (txtPrecoItem.Text.Equals("") ? 1 : Decimal.Parse(txtPrecoItem.Text));
                                item.inserido = true;
                                item.Desconto = 0;
                                item.vPrecoMinimo = Decimal.Parse(lblPreco.Text);
                                obj.addItens(item);
                                obj.PedPg.Clear();
                                Session.Remove("pedido" + urlSessao());
                                Session.Add("pedido" + urlSessao(), obj);
                                carregarDados();

                                //Session.Remove("selecionados" + urlSessao());
                                modalMercadorialista.Hide();


                            }
                            else
                            {
                                modalMercadorialista.Show();
                                break;
                            }
                        }
                        obj.aplicarDesconto(valorDesconto);


                    }
                    else
                    {
                        modalMercadorialista.Show();
                    }
                }
                else
                {
                    carregarDados();
                }
            }
            catch (Exception err)
            {
                lblMercadoriaLista.Text = err.Message;
                lblMercadoriaLista.ForeColor = System.Drawing.Color.Red;
                modalMercadorialista.Show();

            }

        }
        protected void btnCancelaMercadoria_Click(object sender, ImageClickEventArgs e)
        {
            modalMercadorialista.Hide();
            carregarDados();
        }
        protected void ImgPesquisaMercadoria_Click(object sender, ImageClickEventArgs e)
        {
            carregarMercadorias();
        }
        protected void ImgBtnAddSelecionado_Click(object sender, ImageClickEventArgs e)
        {
            ArrayList todosSel = (ArrayList)Session["selecionados" + urlSessao()];
            User usr = (User)Session["User"];


            foreach (GridViewRow linha in gridMercadoria1.Rows)
            {
                CheckBox rdo = (CheckBox)linha.FindControl("chkSelecionaItem");
                if (rdo.Checked)
                {
                    ArrayList sel = new ArrayList();
                    MercadoriaDAO merc = new MercadoriaDAO(linha.Cells[1].Text, usr);
                    sel.Add(merc.PLU);
                    sel.Add(merc.Ref_fornecedor);
                    sel.Add(merc.Descricao.Replace(" ", "&nbsp"));
                    sel.Add("1");
                    sel.Add("1");
                    sel.Add(merc.Preco.ToString("N2"));
                    sel.Add(merc.Preco.ToString("N2"));
                    sel.Add(merc.peso_bruto.ToString());
                    todosSel.Add(sel);
                }

            }

            ViewState["gridLinha"] = todosSel.Count - 2;
            Session.Remove("selecionados" + urlSessao());
            Session.Add("selecionados" + urlSessao(), todosSel);
            carregarMercadorias();
            Session.Remove("btnFechaMercadoria");

        }
        protected void GridMercadoria1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void TxtPesquisaLista_TextChanged(object sender, EventArgs e)
        {
            exibeLista();
        }

        protected void txtfiltromercadoria_TextChanged(object sender, EventArgs e)
        {
            carregarMercadorias();
        }
        protected void chkSeleciona_CheckedChanged(object sender, EventArgs e)
        {

            foreach (GridViewRow item in gridMercadoria1.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");

                if (chk != null)
                {
                    chk.Checked = (sender as CheckBox).Checked;
                }
            }
            modalMercadorialista.Show();
        }

        protected void GridMercadoriaSelecionado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument) + 1;
            ArrayList selecionados = (ArrayList)Session["selecionados" + urlSessao()];
            selecionados.RemoveAt(index);
            Session.Remove("selecionados" + urlSessao());
            Session.Add("selecionados" + urlSessao(), selecionados);
            GridMercadoriaSelecionado.DataSource = Conexao.GetArryTable(selecionados);
            GridMercadoriaSelecionado.DataBind();

            modalMercadorialista.Show();
        }

        protected void txtCliente_Fornec_TextChanged(object sender, EventArgs e)
        {
            if (!txtCliente_Fornec.Text.Trim().Equals(""))
            {
                pedidoDAO obj = (pedidoDAO)Session["pedido" + urlSessao()];
                obj.Cliente_Fornec = txtCliente_Fornec.Text;
                clienteDados();

                calculaLimiteCliente();
            }
            else
            {
                txtNomeCliente.Focus();
            }
        }


        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                pedidoDAO obj = (pedidoDAO)Session["pedido" + urlSessao()];
                obj.excluir();
                String baixaEstoque = Funcoes.valorParametro("BAIXA_ESTOQUE_PED_VENDA", null);

              

                modalPnConfirma.Hide();
                msgShow("Registro Cancelado com sucesso", false);
                status = "pesquisar";
                carregabtn(pnBtn, null, null, null, null, "Cancelar Pedido", null);
                carregarDados();
                Session.Remove("pedido" + urlSessao());
            }
            catch (Exception err)
            {
                msgShow("Não foi possivel Cancelar o registro pelo error:" + err.Message, true);

                carregabtn(pnBtn);
            }
        }
        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            modalPnConfirma.Hide();
            carregarDados();
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetNomesClientes(string prefixText, int count)
        {
            String sql = "	Select  codigo_cliente " +
                            "+'|'+ rtrim(nome_cliente) " +
                            "+'| CNPJ: '+isnull(CNPJ,'')" +
                            "+'| FONE: '+isnull((Select top 1 id_meio_comunicacao from Cliente_contato where Codigo_Cliente=cliente.codigo_cliente and (Meio_Comunicacao like 'FONE%' OR Meio_Comunicacao like 'CELULAR%') ),'')" +
                            " from cliente " +
                            "where (nome_cliente like '" + (prefixText.Length > 4 ? "%" : "") + prefixText + "%' ) " +
                                 " or(" +
                                        "(isnull(" +
                                        "   (Select top 1 id_meio_comunicacao from Cliente_contato where Codigo_Cliente=cliente.codigo_cliente and (Meio_Comunicacao like 'FONE%' OR Meio_Comunicacao like 'CELULAR%') ),'')" +
                                        ") like '%" + prefixText + "%') " +

                                "and isnull(inativo,0)=0";
            return Conexao.retornaArray(sql, prefixText.Length);
        }


        protected void txtNomeCliente_TextChanged(object sender, EventArgs e)
        {
            if (txtNomeCliente.Text.IndexOf("|") >= 0)
            {
                pedidoDAO ped = (pedidoDAO)Session["pedido" + urlSessao()];
                txtCliente_Fornec.Text = txtNomeCliente.Text.Substring(0, txtNomeCliente.Text.IndexOf("|"));
                ped.Cliente_Fornec = txtCliente_Fornec.Text;
                clienteDados();

                txtCodTbPreco.Focus();
                calculaLimiteCliente();
            }

        }
        private void clienteDados()
        {
            pedidoDAO obj = (pedidoDAO)Session["pedido" + urlSessao()];
            txtNomeCliente.Text = obj.NomeCliente;
            txtNomeCliente.Text = obj.NomeCliente;
            txtEndereco.Text = obj.cliente.Endereco;
            txtEnderecoNumero.Text = obj.cliente.endereco_nro;
            txtEnderecoComplemento.Text = obj.cliente.complemento_end;
            txtBairro.Text = obj.cliente.Bairro;
            txtCidade.Text = obj.cliente.Cidade;
            txtUf.Text = obj.cliente.UF;
            txtNomeCliente.Focus();

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

        protected void ddlPedidoSimples_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnabledControls(cabecalho, true);
        }

        protected void txtDesconto_TextChanged(object sender, EventArgs e)
        {


            try
            {
                pedidoDAO ped = (pedidoDAO)Session["pedido" + urlSessao()];
                Decimal valor = Decimal.Parse(txtDesconto.Text);
                ped.aplicarDesconto(valor);
                ped.PedPg.Clear();
                txtTotal.Text = ped.Total.ToString("N2");
                carregarGrids();
            }
            catch (Exception)
            {
                msgShow("Valor de Desconto inválido", true);

            }
        }

        protected void imgBtnConfirmaDesconto_Click(object sender, ImageClickEventArgs e)
        {
            txtSAutorizacao.BackColor = System.Drawing.Color.White;
            SqlDataReader rsSenha = null;
            try
            {
                if (!txtSAutorizacao.Text.Equals(""))
                {
                    rsSenha = Conexao.consulta("select senha,nome from funcionario where senha ='" + txtSAutorizacao.Text + "'", null, false);
                    if (rsSenha.Read())
                    {
                        if (rsSenha["senha"].ToString().Equals(txtSAutorizacao.Text))
                        {
                            if (lblTituloSenha.Text.Contains("Desconto"))
                            {
                                txtObs.Text += " \n Desconto Autorizado por : " + rsSenha["nome"].ToString() + " Data :" + DateTime.Now.ToString("dd/MM/yyyy"); ;
                                Salvar(false);

                            }
                            else if (lblTituloSenha.Text.Contains("não tem Limite"))
                            {
                                txtObs.Text += " \n Venda acima do limite do Cliente Autorizado por : " + rsSenha["nome"].ToString() + " Data :" + DateTime.Now.ToString("dd/MM/yyyy");
                                Salvar(true);
                            }

                        }

                        else
                        {

                            txtSAutorizacao.BackColor = System.Drawing.Color.Red;
                            txtSAutorizacao.Focus();
                            modalSenha.Show();
                        }
                    }
                    else
                    {

                        txtSAutorizacao.BackColor = System.Drawing.Color.Red;
                        txtSAutorizacao.Focus();
                        modalSenha.Show();
                    }

                }
            }
            catch (Exception)
            {

                txtSAutorizacao.BackColor = System.Drawing.Color.Red;
                txtSAutorizacao.Focus();
                modalSenha.Show();
            }
            finally
            {
                if (rsSenha != null)
                    rsSenha.Close();

            }
        }
        protected void imgBtnCancelaDesconto_Click(object sender, ImageClickEventArgs e)
        {
            modalSenha.Hide();
        }

        
        protected void btnTitulosAbertos_Click(object sender, ImageClickEventArgs e)
        {
            User usr = (User)Session["user"];
            gridTitulos.DataSource = Conexao.GetTable("select Documento,tipo_recebimento as tipo,convert(varchar,Emissao,103) Emissao ,convert(varchar,Vencimento,103) Vencimento,Valor = Case when Valor_pago>Valor then 0 else (Valor-Valor_Pago) end,Dias = (DATEDIFF(DAY,Vencimento,GETDATE())) " +
                                                       " from Conta_a_receber where Codigo_Cliente = '" + txtCliente_Fornec.Text + "'and status ='1'  " +
                                                       " order by convert(varchar,Vencimento,102)", usr, false);
            gridTitulos.DataBind();
            Decimal vTotal = 0;
            foreach (GridViewRow item in gridTitulos.Rows)
            {
                vTotal += Decimal.Parse(item.Cells[4].Text);
            }

            lblTotalTitulos.Text = vTotal.ToString("N2");
            modalTitulos.Show();
        }

        protected void btnImprimirTitulos_Click(object sender, ImageClickEventArgs e)
        {
            ArrayList titulos = new ArrayList();
            ArrayList cliente = new ArrayList();
            cliente.Add("CLIENTE");
            cliente.Add(txtCliente_Fornec.Text);
            cliente.Add(txtNomeCliente.Text);
            titulos.Add(cliente);
            foreach (GridViewRow item in gridTitulos.Rows)
            {
                ArrayList arrItem = new ArrayList();
                arrItem.Add(item.Cells[0].Text.Replace("&nbsp;", ""));
                arrItem.Add(item.Cells[1].Text);
                arrItem.Add(item.Cells[2].Text);
                arrItem.Add(item.Cells[3].Text);
                arrItem.Add(item.Cells[4].Text);
                arrItem.Add(item.Cells[5].Text);
                titulos.Add(arrItem);
            }

            Session.Remove("titulosImprimir");
            Session.Add("titulosImprimir", titulos);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refdrts", "window.open('PedidoPrint.aspx?simples=true','_blank');", true);
        }

        protected void chkProduzirItem_change(object sender, EventArgs e)
        {
            txtDataProducaoItem.Enabled = chkProduzirItem.Checked;
            txtHoraProducaoItem.Enabled = chkProduzirItem.Checked;
            if (chkProduzirItem.Checked)
            {
                //Parametro para obrigar agrupamento no pedido de venda produção.
                User usr = (User)Session["User"];
                bool obrigaAgrupamento = Funcoes.valorParametro("PED_PRODUZIR_OBRIGA_AGRUP", usr).ToUpper().Equals("TRUE");

                txtDataProducaoItem.BackColor = txtDescricao.BackColor;
                txtHoraProducaoItem.BackColor = txtDescricao.BackColor;

                if (obrigaAgrupamento)
                {
                    txtDataProducaoItem.Text = txtData_entrega.Text;
                    txtHoraProducaoItem.Text = txthora.Text;
                }
                else
                {
                    txtDataProducaoItem.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtHoraProducaoItem.Text = DateTime.Now.ToString("HH:mm");
                }
            }
            else
            {
                txtDataProducaoItem.Text = "";
                txtHoraProducaoItem.Text = "00:00:00";
                txtDataProducaoItem.BackColor = TxtTotalItem.BackColor;
                txtHoraProducaoItem.BackColor = TxtTotalItem.BackColor;
            }
            ModalItens.Show();
        }

        protected void txtPluAddRapido_TextChanged(object sender, EventArgs e)
        {

        }


        protected void btnOkError_Click(object sender, EventArgs e)
        {

            if (lblErroPanel.Text.Contains("INATIVO"))
            {
                txtCliente_Fornec.BackColor = System.Drawing.Color.Red;
                txtNomeCliente.BackColor = System.Drawing.Color.Red;
                TabContainer1.Tabs[2].BackColor = System.Drawing.Color.Red;
            }

            modalError.Hide();
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

        protected void ImgBtnConfirmaClientes_Click(object sender, EventArgs e)
        {
            if (txtNomeClienteNovo.Text.Equals(""))
            {
                lblErrorCliente.Text = "Preencha o Nome ";
                modalNovocliente.Show();
            }
            else
            {
                try
                {
                    User usr = (User)Session["User"];
                    ClienteDAO obj = new ClienteDAO(usr);
                    obj.Codigo_Cliente = txtCodigoClienteNovo.Text;
                    obj.Nome_Cliente = txtNomeClienteNovo.Text;
                    String tipo = Conexao.retornaUmValor("Select top 1 Meio_comunicacao from meio_comunicacao where Meio_comunicacao like '%FONE%' OR Meio_comunicacao like '%CELULAR%'", null);
                    obj.addMeioComunicacao(tipo, txtTelefoneClienteNovo.Text, "");
                    obj.Endereco = txtEnderecoClienteNovo.Text;
                    obj.endereco_nro = txtNumeroClienteNovo.Text;
                    obj.complemento_end = txtComplementoClienteNovo.Text;
                    obj.Bairro = txtBairroClienteNovo.Text;
                    obj.Cidade = txtCidadeClienteNovo.Text;
                    obj.UF = txtUFClienteNovo.Text;
                    obj.Usuario = usr.getUsuario();
                    obj.UsuarioAlteracao = usr.getUsuario();

                    obj.salvar(true);
                    modalNovocliente.Hide();
                }
                catch (Exception err)
                {
                    lblErrorCliente.Text = err.Message;
                    modalNovocliente.Show();
                }



            }
        }

        protected void ImgBtnCancelaClientes_Click(object sender, EventArgs e)
        {
            modalNovocliente.Hide();
        }

        protected void ImgbtnAddNovocliente_Click(object sender, EventArgs e)
        {
            LimparCampos(pnNovoCliente);
            EnabledControls(pnNovoCliente, true);
            modalNovocliente.Show();
        }


    }
}