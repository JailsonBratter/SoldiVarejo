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
    public partial class PedidoDevolucaoDetalhes : visualSysWeb.code.PagePadrao
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            User usr = (User)Session["User"];
            if (!IsPostBack)
            {
                pedidoDAO obj = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            }
            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    EnabledControls(pnMercadoriaLista, true);
                    Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    novoPedido();
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
                            pedidoDAO obj = new pedidoDAO(index, 3, usr);
                            Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                            Session.Add("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
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
                        lblError.Text = err.Message;
                    }
                }
            }
            carregabtn(pnBtn, null, null, null, null, "Cancelar Pedido", null);

            if (!IsPostBack)
            {
                if (txtCliente_Fornec.Enabled)
                    txtCliente_Fornec.Focus();
            }
            verificaStatusCliente();

        }
        private void novoPedido()
        {
            User usr = (User)Session["User"];

            status = "incluir";
            habilitarCampos(true);
            txtData_cadastro.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtData_entrega.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtUsLogado.Text = usr.getUsuario();

            txthora.Text = "00:00";
            txtCFOP.Text = Funcoes.valorParametro("NAT_OP_DEVOLUCAO_VENDA", usr);
            txtCentroCusto.Text = Funcoes.valorParametro("CC_PEDIDO_DEVOLUCAO", usr);
            pedidoDAO obj = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            obj = new pedidoDAO(usr);
            obj.Tipo = 3;
            obj.Status = 1;
            obj.Data_cadastro = DateTime.Now;
            obj.Data_entrega = DateTime.Now;
            obj.CFOP = (txtCFOP.Text.Equals("") ? 0 : Decimal.Parse(txtCFOP.Text));
            obj.centro_custo = txtCentroCusto.Text;
            if (Funcoes.valorParametro("PEDIDO_VDA_VEND_OBRIG", usr) != "TRUE")
            {
                txtfuncionario.Text = usr.getUsuario();
                obj.funcionario = usr.getUsuario();
            }
            else
            {
                txtfuncionario.Text = "";
            }

            Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
            txtDataCupom.Text = DateTime.Now.ToString("dd/MM/yyyy");

            carregarMercadorias();

            //carregarGrids();
            carregabtn(pnBtn, null, null, null, null, "Cancelar Pedido", null);

        }
        private void limparCampos()
        {
            LimparCampos(cabecalho);
            LimparCampos(conteudo);
        }

        protected void btnConfirmaManter_Click(object sender, ImageClickEventArgs e)
        {

            //User usr = (User)Session["User"];
            //pedidoDAO ped = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            //ped.Pedido = "";
            //ped.Status = 1;
            //ped.Obs = "";
            //ped.aproveitaItens();
            //Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            //Session.Add("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), ped);


            //carregarDados();

            //status = "incluir";
            //habilitarCampos(true);
            //txtData_cadastro.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //txtData_entrega.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //txtUsLogado.Text = usr.getUsuario();
            //txtfuncionario.Text = usr.getUsuario();
            //txthora.Text = "00:00";
            //txtCFOP.Text = Funcoes.valorParametro("NATUREZA_OP_PEDIDO_VENDA", usr);
            ////txtCodTbPreco.Text = Funcoes.valorParametro("TABELA_DESCONTO_VENDA", usr);
            //txtCentroCusto.Text = Funcoes.valorParametro("CENTRO_CUSTO_PEDIDO_VENDA", usr);


            //modalManterDados.Hide();
            //carregabtn(pnBtn, null, null, null, null, "Cancelar Pedido", null);
            //Session.Remove("PedidoManter");


        }

        protected void btnCancelaManter_Click(object sender, ImageClickEventArgs e)
        {
            modalManterDados.Hide();
            novoPedido();
            Session.Remove("PedidoManter");
        }

        protected bool validaCamposObrigatorios()
        {
            string vPar = Funcoes.valorParametro("BLOQ_CLIENTE_FINANCEIRO", null);
            if (vPar.ToUpper().Equals("TRUE"))
            {
                if (!txtCliente_Fornec.Text.Trim().Equals(""))
                {
                    String statusCliente = Conexao.retornaUmValor("Select Situacao from cliente where codigo_cliente ='" + txtCliente_Fornec.Text + "'", null);
                    if (!statusCliente.ToUpper().Trim().Equals("OK"))
                    {
                        pedidoDAO ped = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                        Hashtable tbTipo = Conexao.TbValores("select tipo_pagamento,a_vista from tipo_pagamento", null);
                        foreach (pedido_pagamentoDAO pg in ped.PedPg)
                        {
                            if (!pg.excluido)
                            {
                                String aVista = tbTipo[pg.Tipo_pagamento].ToString();
                                if (!aVista.Equals("1"))
                                    throw new Exception("A Situação do Cliente não permite pagamentos a Prazo!!");
                            }
                        }
                    }
                }
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
            String[] campos = { "txtPedido", 
                                    "ddlStatus", 
                                    "txtUsLogado", 
                                    "txtTotal",
                                    "TxtTotalItem",
                                    "txtTotalBruto"
                                     };


            return existeNoArray(campos, campo.ID + "");
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/pedidos/pages/pedidoDevolucaoDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            if (!ddlStatus.SelectedValue.Equals("1"))
            {
                User usr = (User)Session["user"];
                if (usr.adm(usr.tela))
                {
                    status = "editar";
                    txtCliente_Fornec.Enabled = true;
                    txtCliente_Fornec.BackColor = System.Drawing.Color.White;
                    txtNomeCliente.Enabled = true;
                    txtNomeCliente.BackColor = System.Drawing.Color.White;
                    //imgBtnCliente.Visible = true;
                }
                else
                {
                    lblError.Text = "SÓ É POSSIVEL FAZER ALTERAÇÕES EM PEDIDOS ABERTOS";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                status = "editar";
                carregabtn(pnBtn, null, null, null, null, "Cancelar Pedido", null);
                habilitarCampos(true);
                carregarDados();
                lblError.Text = "";
            }
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("PedidoDevolucao.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            if (!ddlStatus.SelectedValue.Equals("3"))
            {
                modalPnConfirma.Show();
            }
            else
            {
                lblError.Text = "PEDIDO JÁ ESTA CANCELADO";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Salvar()
        {
            try
            {
                if (validaCamposObrigatorios())
                {

                    carregarDadosObj();
                    pedidoDAO obj = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                    String baixaEstoque = Funcoes.valorParametro("BAIXA_ESTOQUE_PED_VENDA", null);
                    if ((baixaEstoque.Equals("TRUE")))
                    {
                        if (obj.PedPg.Count <= 0)
                        {
                            //throw new Exception("Não foi incluído nenhum pagamento");
                        }

                        ddlStatus.SelectedValue = "2";
                        obj.Status = 2;
                        SqlConnection conn = Conexao.novaConexao();
                        SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

                        try
                        {
                            obj.salvar(status.Equals("incluir"), conn, tran);
                            User usr = (User)Session["User"];
                            natureza_operacaoDAO op = new natureza_operacaoDAO(obj.CFOP.ToString(), null);


                            Hashtable pluQtd = new Hashtable();

                            foreach (pedido_itensDAO item in obj.PedItens)
                            {
                                if (!item.excluido)
                                {
                                    if (pluQtd.Contains(item.PLU))
                                    {
                                        Decimal vqtd = (Decimal)pluQtd[item.PLU];
                                        pluQtd[item.PLU] = vqtd + (item.Qtde * item.Embalagem);
                                    }
                                    else
                                        pluQtd.Add(item.PLU, (item.Qtde * item.Embalagem));


                                    //if (Funcoes.valorParametro("PEDIDO_SOCOMESTOQUE", usr).ToUpper() == "TRUE")
                                    //{
                                    //    Decimal estoqueAtual = Decimal.Parse(Conexao.retornaUmValor("select isnull(Saldo_Atual,0) from mercadoria_loja where PLU='" + item.PLU + "'", usr, conn, tran));
                                    //    if (estoqueAtual < (Decimal)pluQtd[item.PLU])
                                    //        throw new Exception("O Estoque do produto " + item.PLU + "-" + item.Descricao + " não é suficiente para cadastrar o pedido");
                                    //}
                                    //item.naturezaOperacao = op;
                                    item.AtualizaSaidaEstoque(conn, tran);
                                }
                                //}




                            }
                            tran.Commit();
                        }
                        catch (Exception err)
                        {
                            tran.Rollback();
                            throw err;
                        }
                        finally
                        {
                            if (conn != null)
                                conn.Close();
                        }

                    }
                    else
                    {
                        obj.salvar(status.Equals("incluir")); // se for incluir true se não falso;
                    }
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;
                    status = "visualizar";
                    visualizar(pnBtn);
                    habilitarCampos(false);
                    Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
                    carregarDados();

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

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            //if (!txtDesconto.Text.Equals("") && decimal.Parse(txtDesconto.Text) > 0)
            //{
            //    txtSAutorizacao.Focus();
            //    modalSenha.Show();

            //}
            //else
            //{
            Salvar();
            //}


        }

        protected void habilitarCampos(bool hab)
        {
            EnabledControls(cabecalho, hab);
            EnabledControls(conteudo, hab);
            EnabledControls(PnAddPagamento, hab);
            EnabledControls(pnItens, hab);

            gridItens.Enabled = hab;
            gridPagamentos.Enabled = hab;

            User usr = (User)Session["User"];
            btnImprimirDevolucao.Visible = status.Equals("visualizar");



        }


        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("pedidoDevolucao.aspx");//colocar endereco pagina de pesquisa
        }

        protected void txt_TextChanged(object sender, EventArgs e)
        {

            ModalItens.Show();

        }

        private void carregarDados()
        {
            pedidoDAO obj = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            txtPedido.Text = (obj.Pedido == null ? "" : obj.Pedido.ToString());
            ddlStatus.SelectedValue = obj.Status.ToString();
            txtCliente_Fornec.Text = obj.Cliente_Fornec.ToString();
            txtNomeCliente.Text = obj.NomeCliente;
            txtData_cadastro.Text = obj.Data_cadastroBr();
            txtData_entrega.Text = obj.Data_entregaBr();
            txthora.Text = obj.hora.ToString();
            txtUsLogado.Text = obj.Usuario.ToString();
            txtObs.Text = obj.Obs.ToString();
            txtCFOP.Text = obj.CFOP.ToString();
            txtfuncionario.Text = obj.funcionario.ToString();
            txtCentroCusto.Text = obj.centro_custo;
            carregarGrids();

            txtTotal.Text = string.Format("{0:0,0.00}", obj.Total);
            txtTotalBruto.Text = obj.totalBruto.ToString("N2");
            //txtDesconto.Text = string.Format("{0:0,0.00}", obj.Desconto);

        }

        protected void gridItens_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


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
            pedidoDAO ped = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            User usr = (User)Session["User"];
            pedido_itensDAO item = new pedido_itensDAO(usr);

            item.Filial = ped.Filial;
            item.Pedido = ped.Pedido;
            item.Tipo = ped.Tipo;
            item.PLU = txtPLU.Text;
            item.Qtde = Decimal.Parse(txtQtde.Text);
            item.Embalagem = Decimal.Parse(txtEmbalagem.Text);
            item.unitario = Decimal.Parse(txtUnitario.Text);
            item.Desconto = Decimal.Parse(txtDescontoItem.Text);
            item.index = int.Parse(lblIndex.Text);
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
        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {

            String itemLista = (String)Session["camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
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
                pedidoDAO ped = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                User usr = (User)Session["User"];
                pedido_pagamentoDAO pg = new pedido_pagamentoDAO(usr);
                pg.Tipo_pagamento = txtTipoPg.Text;
                pg.Vencimento = DateTime.Parse(txtVencimentoPg.Text);
                pg.Valor = Decimal.Parse(txtValorPg.Text);
                ped.addPagamentos(pg);

                Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), ped);
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
            Session.Remove("item" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
        }

        protected void ImgExcluiItem_Click(object sender, ImageClickEventArgs e)
        {
            pedidoDAO ped = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            User usr = (User)Session["User"];
            pedido_itensDAO itempd = new pedido_itensDAO(usr);
            itempd.Filial = ped.Filial;
            itempd.Pedido = ped.Pedido;
            itempd.PLU = txtPLU.Text;
            itempd.index = int.Parse(lblIndex.Text);
            //Decimal valor = (txtDesconto.Text.Trim().Equals("") ? 0 : Decimal.Parse(txtDesconto.Text));
            ped.removeItem(itempd);

            Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), ped);
            carregarDados();
            //carregarGrids();

            //PnExcluirItem.Visible = false;
        }

        public void carregarGrids()
        {
            try
            {


                pedidoDAO obj = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                gridItens.DataSource = obj.PdItens();
                gridItens.DataBind();

                gridPagamentos.DataSource = obj.PdPag();
                gridPagamentos.DataBind();


            }
            catch (Exception err)
            {

                lblError.Text = err.Message;
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


                // PnExcluirItem.Visible = false;
                carregarDadosObj();
                Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                carregarMercadorias();
            }
            catch (Exception err)
            {
                lblError.Text = err.Message;
            }
        }


        protected void ImgBtnAddPagamentos_Click(object sender, ImageClickEventArgs e)
        {
            carregarDadosObj();
            pedidoDAO ped = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            txtValorPg.Text = (ped.Total - ped.totalPagamentos()).ToString("N2");
            ModalPagamentos.Show();
        }

        protected void carregarMercadorias()
        {
            lblMercadoriaLista.Text = "Inclusão de Produto";
            lblMercadoriaLista.ForeColor = Label1.ForeColor;

            User usr = (User)Session["user"];

            if (txtDataCupom.Text.Equals(""))
            {
                txtDataCupom.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }

            String sqlMercadoria = " select  s.PLU, Ref = m.Ref_fornecedor, m.Descricao, SUM(Qtde)Qtde,(Select SUM(qtde) from pedido_itens inner join pedido as pd on pd.pedido=pedido_itens.pedido and pd.filial = pedido_itens.filial and pd.tipo=pedido_itens.tipo  where pd.filial ='" + usr.getFilial() + "' and pd.status<>3 and  documento =s.documento and data_documento=s.data_movimento and caixa_documento = s.caixa_saida and pedido_itens.plu=s.plu  and pedido_itens.unitario=convert(numeric(12,2), (s.vlr/s.Qtde)) )as Qtde_Dev,'1' as Embalagem, convert(numeric(12,2), (vlr/Qtde)) Vlr,s.Documento as Cupom , s.Caixa_saida as Pdv, convert(varchar,s.Data_movimento,103) as Data     " +
                                               " from saida_estoque s inner join mercadoria m on s.PLU=m.PLU   " +
                                               " where Documento =" + (txtCupom.Text.Equals("") ? "0" : txtCupom.Text) + " AND  s.filial='" + usr.getFilial() + "'  and s.caixa_saida=" + (txtPdv.Text.Equals("") ? "0" : txtPdv.Text) + " and s.data_movimento='" + DateTime.Parse(txtDataCupom.Text).ToString("yyyy-MM-dd") + "' " +
                                               " and m.descricao like '%" + txtDescricaoMercadoriaCupom.Text + "%' and data_cancelamento is null " +
                                               " GROUP BY s.plu, convert(numeric(12,2), (vlr/Qtde)) ,m.embalagem ,m.descricao,m.Ref_fornecedor,s.Documento , s.Caixa_saida, s.Data_movimento " +
                                             " ORDER BY RTRIM(LTRIM(m.descricao))";



            gridMercadoria1.DataSource = Conexao.GetTable(sqlMercadoria, usr, !(txtCupom.Text.Length > 0));
            gridMercadoria1.DataBind();
            ArrayList todosSel = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            if (todosSel == null)
            {
                todosSel = new ArrayList();
                ArrayList cabecalho = new ArrayList();
                cabecalho.Add("PLU");
                cabecalho.Add("Referencia");
                cabecalho.Add("Descricao");
                cabecalho.Add("QTD");
                cabecalho.Add("QTDPadrao");
                cabecalho.Add("embalagem");
                cabecalho.Add("Preco");
                cabecalho.Add("PrecoPadrao");
                cabecalho.Add("Cupom");
                cabecalho.Add("Pdv");
                cabecalho.Add("Data");
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

                        //TextBox txtEmb = (TextBox)GridMercadoriaSelecionado.Rows[i].FindControl("txtEmbItem");

                        ls[3] = txtQtdItem.Text;
                        //ls[5] = txtEmb.Text;


                    }
                }
            }
            Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), todosSel);
            GridMercadoriaSelecionado.DataSource = Conexao.GetArryTable(todosSel);
            GridMercadoriaSelecionado.DataBind();

            modalMercadorialista.Show();
            carregarGrids();

        }
        protected void gridItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            lblerroItem.Text = "";
            LimparCampos(pnItens);
            pedidoDAO ped = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            int index = Convert.ToInt32(e.CommandArgument);
            pedido_itensDAO itemPd = ped.item(index);
            carregarDadosObj();
            carregarDadosItens(itemPd);
            EnabledControls(conteudoItem, false);

        }

        protected void gridPagamentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            pedidoDAO ped = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            int index = Convert.ToInt32(e.CommandArgument);//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            pedido_pagamentoDAO pgPd = ped.pagamento(index);
            ped.removePG(pgPd);
            carregarGrids();
        }

        protected void carregarDadosItens(pedido_itensDAO itemPd)
        {
            txtPLU.Text = itemPd.PLU;
            txtDescricao.Text = itemPd.Descricao;
            txtQtde.Text = itemPd.Qtde.ToString("N2");
            txtEmbalagem.Text = itemPd.Embalagem.ToString("N2");
            txtUnitario.Text = itemPd.unitario.ToString("N2");
            txtDescontoItem.Text = itemPd.Desconto.ToString("N2");
            TxtTotalItem.Text = itemPd.total.ToString("N2");
            lblIndex.Text = itemPd.index.ToString();
            ModalItens.Show();
        }
        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            pedidoDAO obj = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            obj.Pedido = txtPedido.Text;
            obj.Status = int.Parse(ddlStatus.SelectedValue);
            obj.Cliente_Fornec = txtCliente_Fornec.Text;
            obj.Data_cadastro = DateTime.Parse(txtData_cadastro.Text);
            obj.Data_entrega = DateTime.Parse(txtData_entrega.Text);
            obj.hora = txthora.Text;
            //obj.Desconto = (txtDesconto.Text.Equals("") ? 0 : Decimal.Parse(txtDesconto.Text));
            obj.Total = (txtTotal.Text.Equals("") ? 0 : Decimal.Parse(txtTotal.Text));
            obj.Usuario = txtUsLogado.Text;
            obj.Obs = txtObs.Text;
            obj.CFOP = (txtCFOP.Text.Equals("") ? 0 : Decimal.Parse(txtCFOP.Text));
            obj.funcionario = txtfuncionario.Text;
            //obj.TabelaPreco = txtCodTbPreco.Text;
            //obj.pedido_simples = (ddlPedidoSimples.SelectedValue.Equals("1"));
            obj.centro_custo = txtCentroCusto.Text;
            Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
        }

        protected void exibeLista()
        {

            lblErroPesquisa.Text = "";
            User usr = (User)Session["User"];
            String or = (String)Session["camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            String sqlLista = "";


            switch (or)
            {
                case "Cliente":
                    sqlLista = "select codigo_cliente Codigo, Nome_Cliente Nome from Cliente where (codigo_cliente like '%" + TxtPesquisaLista.Text + "%' or nome_cliente like '%" + TxtPesquisaLista.Text + "%') and isnull(inativo,0)=0 "; ;
                    lbllista.Text = "Escolha um Cliente";
                    break;
                case "PLU":
                    sqlLista = "select Plu, Descricao from mercadoria where plu like '%" + TxtPesquisaLista.Text + "%' or desricao like '%" + TxtPesquisaLista.Text + "%'"; ;
                    lbllista.Text = "Escolha um Produto";
                    break;
                case "Funcionario":
                    sqlLista = "select Nome from funcionario where nome like '%" + TxtPesquisaLista.Text + "%'";
                    lbllista.Text = "Escolha um Funcionario";
                    break;
                case "CFOP":
                    sqlLista = "select codigo_operacao, descricao from natureza_operacao where nf_devolucao=1 and codigo_operacao like '%" + TxtPesquisaLista.Text + "%' and descricao like '%" + TxtPesquisaLista.Text + "%'";
                    lbllista.Text = "Escolha a Natureza de Operação";
                    break;
                case "TipoPagamento":
                    sqlLista = "select tipo_pagamento Tipo ,Prazo from tipo_pagamento where tipo_pagamento like '%" + TxtPesquisaLista.Text + "%'  order by a_vista desc ,Tipo";
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
            Session.Remove("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            switch (btn.ID)
            {
                case "PLU":
                    Session.Add("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "PLU");
                    break;
                case "imgBtnCliente":
                    Session.Add("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "Cliente");
                    break;
                case "imgBtnFuncionario":
                    Session.Add("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "Funcionario");
                    break;
                case "btnimg_txtPLU":
                    Session.Add("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "PLU");
                    break;
                case "imgBtnCfop":
                    Session.Add("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "CFOP");
                    break;
                case "btnimg_txtTipoPg":
                    Session.Add("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "TipoPagamento");
                    break;
                case "imgCodTbPreco":
                    Session.Add("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "txtCodTbPreco");
                    break;
                case "btnimg_txtcentro_custo":
                    Session.Add("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "txtCentroCusto");
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

                    lbllista.Text = "";
                    String listaAtual = (String)Session["camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                    Session.Remove("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));

                    if (listaAtual.Equals("Cliente"))
                    {

                        txtCliente_Fornec.Text = ListaSelecionada(1);
                        txtNomeCliente.Text = ListaSelecionada(2);
                        pedidoDAO ped = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                        ped.Cliente_Fornec = txtCliente_Fornec.Text;
                        Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                        Session.Add("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), ped);

                    }
                    else if (listaAtual.Equals("PLU"))
                    {
                        User usr = (User)Session["User"];
                        MercadoriaDAO merc = new MercadoriaDAO(ListaSelecionada(1), usr);
                        txtPLU.Text = merc.PLU;
                        txtDescricao.Text = merc.Descricao;
                        txtEmbalagem.Text = "1";
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
                        txtVencimentoPg.Text = DateTime.Now.AddDays(int.Parse(ListaSelecionada(2))).ToString("dd/MM/yyyy");
                        ModalPagamentos.Show();


                    }
                    else if (listaAtual.Equals("txtCodTbPreco"))
                    {
                        pedidoDAO ped = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                        //txtCodTbPreco.Text = ListaSelecionada(1);
                        //ped.TabelaPreco = txtCodTbPreco.Text;
                        txtTotal.Text = ped.Total.ToString("N2");
                        //txtDesconto.Text = ped.Desconto.ToString("N2");
                        Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                        Session.Add("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), ped);

                    }
                    else if (listaAtual.Equals("txtCentroCusto"))
                    {
                        txtCentroCusto.Text = ListaSelecionada(1);
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



                User usr = (User)Session["User"];
                ArrayList selecionados = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                if (selecionados != null && selecionados.Count > 1)
                {

                    pedidoDAO obj = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                    ArrayList arrAdds = new ArrayList();
                    foreach (GridViewRow sl in GridMercadoriaSelecionado.Rows)
                    {
                        pedido_itensDAO item = new pedido_itensDAO(usr);

                        item.PLU = sl.Cells[1].Text;
                        TextBox txtQtdItem = (TextBox)sl.FindControl("txtQtd");
                        if (!txtQtdItem.Text.Equals("------"))
                        {
                            item.Qtde = (txtQtdItem.Text.Equals("") ? 1 : Decimal.Parse(txtQtdItem.Text));
                            Label lblQtde = (Label)sl.FindControl("lblQTDPadrao");
                            Decimal tQtde = Decimal.Parse(txtQtdItem.Text);
                            Decimal lQtde = Decimal.Parse(lblQtde.Text);
                            Decimal vPreco = 0;
                            Decimal.TryParse(sl.Cells[6].Text, out vPreco);
                            Decimal pdItem = obj.qtdeAdicionadaItem(sl.Cells[1].Text, vPreco);
                            String strItem = "";
                            if (pdItem > 0 )
                            {
                                strItem = " Já foi incluido a quantidade de " + pdItem + " no Pedido e";
                                tQtde += pdItem;
                            }
                            txtQtdItem.BackColor = System.Drawing.Color.White;

                            if (tQtde > lQtde)
                            {
                                txtQtdItem.Focus();
                                txtQtdItem.BackColor = System.Drawing.Color.Red;
                                throw new Exception("O Item " + sl.Cells[1].Text + " com Vlr:"+vPreco.ToString("N4")+  strItem + " não pode ter a quantidade maior que " + lQtde.ToString("N4"));
                            }
                            else if (tQtde <= 0)
                            {
                                txtQtdItem.Focus();
                                txtQtdItem.BackColor = System.Drawing.Color.Red;
                                throw new Exception("O Item " + sl.Cells[1].Text + " com Vlr:" + vPreco.ToString("N4") + " não pode ter a quantidade menor ou igual a Zero");
                            }


                            item.Embalagem = 1;
                            item.unitario = (sl.Cells[6].Text.Trim().Equals("") ? 0 : Decimal.Parse(sl.Cells[6].Text));
                            item.inserido = true;
                            item.Desconto = 0;
                            item.vPrecoMinimo = (sl.Cells[6].Text.Trim().Equals("") ? 0 : Decimal.Parse(sl.Cells[6].Text));
                            item.documento = sl.Cells[7].Text;
                            item.caixa_documento = int.Parse(sl.Cells[8].Text);
                            item.data_documento = DateTime.Parse(sl.Cells[9].Text);
                            arrAdds.Add(item);
                            

                            //Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                            


                        }
                        else
                        {
                            modalMercadorialista.Show();
                            break;
                        }
                    }

                    foreach (pedido_itensDAO item in arrAdds)
                    {
                        obj.addItens(item);
                    }

                    Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
                    carregarDados();
                    modalMercadorialista.Hide();

                    //obj.aplicarDesconto(valorDesconto);


                }
                else
                {
                    modalMercadorialista.Show();
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
            try
            {
                lblMercadoriaLista.Text = "Inclusão de Produto";
                lblMercadoriaLista.ForeColor = System.Drawing.Color.Black;

                ArrayList todosSel = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                User usr = (User)Session["User"];
                ArrayList novosSel = new ArrayList();
                Hashtable pluQtd = new Hashtable();
                foreach (ArrayList item in todosSel)
                {
                    pluQtd.Add(item[0].ToString() + item[5].ToString(), item[3]);
                }
                


                foreach (GridViewRow linha in gridMercadoria1.Rows)
                {
                    CheckBox rdo = (CheckBox)linha.FindControl("chkSelecionaItem");
                    if (rdo.Checked)
                    {
                        ArrayList sel = new ArrayList();
                     
                        Decimal vQtdeC = 0;
                        Decimal vQtdeP = 0;
                        Decimal.TryParse(linha.Cells[4].Text, out vQtdeC);
                        Decimal.TryParse(linha.Cells[5].Text, out vQtdeP);
                        if (pluQtd.Contains(linha.Cells[1].Text + linha.Cells[7].Text))
                        {
                            Decimal vQtdIncluido =0;
                            Decimal.TryParse((String)pluQtd[linha.Cells[1].Text+ linha.Cells[7].Text], out vQtdIncluido);
                            vQtdeP += vQtdIncluido;
                        }
                        Decimal vDev = vQtdeC - vQtdeP;

                        if (vDev <= 0)
                        {
                            novosSel.Clear();
                            throw new Exception("Todas as Unidades Vendidas do item " + linha.Cells[1].Text +"  Com o Vlr:"+ linha.Cells[7].Text+ " já foram devolvidas !");

                        }
                      

                        MercadoriaDAO merc = new MercadoriaDAO(linha.Cells[1].Text, usr);
                        sel.Add(merc.PLU);
                        sel.Add(merc.Ref_fornecedor);
                        sel.Add(merc.Descricao);
                        sel.Add(vDev.ToString("N4"));//Qtde
                        sel.Add(vDev.ToString("N4"));//Limite qtde
                        sel.Add(linha.Cells[6].Text);//Embalagem
                        sel.Add(linha.Cells[7].Text);//Vlr
                        sel.Add(linha.Cells[7].Text);
                        sel.Add(linha.Cells[8].Text);//Cupom
                        sel.Add(linha.Cells[9].Text);//Pdv
                        sel.Add(linha.Cells[10].Text);//Data

                        novosSel.Add(sel);
                    }

                }

                if (novosSel.Count > 0)
                {
                    foreach (ArrayList item in novosSel)
                    {
                        todosSel.Add(item);
                    }
                }
                else
                {
                    throw new Exception("Nenhum Item foi selecionado!");
                }

                ViewState["gridLinha"] = todosSel.Count - 2;
                Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), todosSel);
                carregarMercadorias();
            }
            catch (Exception err)
            {

                lblMercadoriaLista.Text = err.Message;
                lblMercadoriaLista.ForeColor = System.Drawing.Color.Red;
                modalMercadorialista.Show();
            }
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
            ArrayList selecionados = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            selecionados.RemoveAt(index);
            Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), selecionados);
            GridMercadoriaSelecionado.DataSource = Conexao.GetArryTable(selecionados);
            GridMercadoriaSelecionado.DataBind();

            modalMercadorialista.Show();
        }

        protected void txtCliente_Fornec_TextChanged(object sender, EventArgs e)
        {
            if (!txtCliente_Fornec.Text.Trim().Equals(""))
            {
                //pedidoDAO ped = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                //ped.Cliente_Fornec = txtCliente_Fornec.Text;
                txtNomeCliente.Text = Conexao.retornaUmValor("select Nome_cliente from cliente where codigo_cliente='" + txtCliente_Fornec.Text + "' and isnull(inativo,0)=0", new User());
                txtNomeCliente.Focus();
                //Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                //Session.Add("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), ped);
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
                pedidoDAO obj = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                obj.excluir();
                modalPnConfirma.Hide();
                lblError.Text = "Registro Cancelado com sucesso";
                status = "pesquisar";
                carregabtn(pnBtn, null, null, null, null, "Cancelar Pedido", null);
                carregarDados();
                Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            }
            catch (Exception err)
            {

                lblError.Text = "Não foi possivel Cancelar o registro pelo error:" + err.Message;

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
            String sql = "	Select codigo_cliente +'|'+ rtrim(nome_cliente) +'| CNPJ: '+isnull(CNPJ,'')+'| FONE: '+isnull((Select top 1 id_meio_comunicacao from Cliente_contato where Codigo_Cliente=cliente.Contato and Meio_Comunicacao like 'FONE%' OR Meio_Comunicacao like 'CELULAR%' ),'') from cliente where (nome_cliente like '" + (prefixText.Length > 4 ? "%" : "") + prefixText + "%') and isnull(inativo,0)=0";
            return Conexao.retornaArray(sql, prefixText.Length);
        }


        protected void txtNomeCliente_TextChanged(object sender, EventArgs e)
        {
            if (txtNomeCliente.Text.IndexOf("|") >= 0)
            {
                String nome = txtNomeCliente.Text.Substring(txtNomeCliente.Text.IndexOf("|") + 1);
                txtCliente_Fornec.Text = txtNomeCliente.Text.Substring(0, txtNomeCliente.Text.IndexOf("|"));
                txtNomeCliente.Text = nome.Substring(0, nome.IndexOf("|"));
                //txtCodTbPreco.Focus();
                txtCentroCusto.Focus();
                verificaStatusCliente();
            }

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

            //lblError.Text = "";
            //try
            //{
            //    pedidoDAO ped = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            //    Decimal valor = Decimal.Parse(txtDesconto.Text);
            //    ped.aplicarDesconto(valor);
            //    txtTotal.Text = ped.Total.ToString("N2");
            //    carregarGrids();
            //}
            //catch (Exception)
            //{
            //    lblError.Text = "Valor de Desconto inválido";
            //    lblError.ForeColor = System.Drawing.Color.Red;
            //}
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
                            txtObs.Text += " \n Desconto Autorizado por : " + rsSenha["nome"].ToString() + " Data :" + DateTime.Now.ToString("dd/MM/yyyy"); ;
                            Salvar();

                        }

                        else
                        {

                            txtSAutorizacao.BackColor = System.Drawing.Color.Red;
                            txtSAutorizacao.Focus();
                            modalSenha.Show();
                        }
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


        protected void verificaStatusCliente()
        {
            string vPar = Funcoes.valorParametro("BLOQ_CLIENTE_FINANCEIRO", null);
            if (status.Equals("incluir") || status.Equals("editar"))
            {
                TabContainer1.Tabs[2].BackColor = TabContainer1.Tabs[1].BackColor;
                txtCliente_Fornec.BackColor = System.Drawing.Color.White;
                txtNomeCliente.BackColor = System.Drawing.Color.White;

                //imgBtnVerificaStatus.Visible = false;
                //btnTitulosAbertos.Visible = false;
                if (!txtCliente_Fornec.Text.Trim().Equals(""))
                {

                    if (vPar.ToUpper().Equals("TRUE"))
                    {
                        String statusCliente = Conexao.retornaUmValor("Select Situacao from cliente where codigo_cliente ='" + txtCliente_Fornec.Text + "'", null);
                        if (!statusCliente.Trim().ToUpper().Equals("OK"))
                        {
                            txtCliente_Fornec.BackColor = System.Drawing.Color.Red;
                            txtNomeCliente.BackColor = System.Drawing.Color.Red;
                            TabContainer1.Tabs[2].BackColor = System.Drawing.Color.Red;

                            //imgBtnVerificaStatus.Visible = true;

                        }
                        //btnTitulosAbertos.Visible = true;
                    }
                }
            }
            else
            {
                if (vPar.ToUpper().Equals("TRUE"))
                {
                    //btnTitulosAbertos.Visible = !txtCliente_Fornec.Text.Trim().Equals("");
                }
            }


        }
        protected void ImgVerifica_Click(object sender, ImageClickEventArgs e)
        {
            verificaStatusCliente();
        }

        protected void btnImprimirDevolucao_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refdrts", "window.open('PedidoDevolucaoPrint.aspx?codigo=" + txtPedido.Text + "','_blank');", true);
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
                titulos.Add(arrItem);
            }

            Session.Remove("titulosImprimir");
            Session.Add("titulosImprimir", titulos);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refdrts", "window.open('PedidoPrint.aspx','_blank');", true);
        }
    }
}