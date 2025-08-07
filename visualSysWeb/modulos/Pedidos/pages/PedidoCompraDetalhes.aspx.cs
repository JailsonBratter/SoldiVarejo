using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using System.Data.SqlClient;
using System.Collections;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Pedidos.pages
{
    public partial class PedidoCompraDetalhes : visualSysWeb.code.PagePadrao
    {


        protected void Page_Load(object sender, EventArgs e)
        {

            User usr = (User)Session["User"];
            pedidoDAO obj = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    pedidoDAO PedManter = (pedidoDAO)Session["PedidoManter"];
                    pedidoDAO PedSugestao = (pedidoDAO)Session["PedidoSugestao"];
                    if (PedSugestao != null)
                    {
                        Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                        Session.Add("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), PedSugestao);
                        novoPedido();
                        Session.Remove("PedidoSugestao");

                    }
                    else if (PedManter != null)
                    {
                        Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                        Session.Add("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), PedManter);
                        modalManterDados.Show();

                    }
                    else
                    {
                        obj = new pedidoDAO(usr);
                        Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                        Session.Add("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
                        novoPedido();
                    }
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
                            obj = new pedidoDAO(index, 2, usr);
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
            formataCampos();
        }

        protected void novoPedido()
        {


            pedidoDAO obj = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            User usr = (User)Session["User"];
            status = "incluir";
            habilitarCampos(true);
            txtData_cadastro.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtData_entrega.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtUsuario.Text = usr.getUsuario();
            txtfuncionario.Text = usr.getUsuario();
            txthora.Text = "00:00";
            txtCFOP.Text = Funcoes.valorParametro("NATUREZA_OP_PEDIDO_COMPRA", usr);
            obj.Tipo = 2;
            txtTotal.Text = obj.Total.ToString("N2");
            txtCliente_Fornec.Text = obj.Cliente_Fornec;
            txtObs.Text = obj.Obs;
            Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
            carregarGrids();
        }
        protected void btnConfirmaManter_Click(object sender, ImageClickEventArgs e)
        {
            User usr = (User)Session["User"];
            pedidoDAO ped = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            ped.Pedido = "";
            ped.Status = 1;
            ped.Obs = "";
            status = "incluir";
            habilitarCampos(true);
            txtData_cadastro.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtData_entrega.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtUsuario.Text = usr.getUsuario();
            txtfuncionario.Text = usr.getUsuario();
            txthora.Text = "00:00";
            txtCFOP.Text = Funcoes.valorParametro("NATUREZA_OP_PEDIDO_COMPRA", usr);

            Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), ped);
            carregarGrids();
        }
        protected void btnCancelaManter_Click(object sender, ImageClickEventArgs e)
        {
            User usr = (User)Session["User"];
            pedidoDAO obj = new pedidoDAO(usr);
            Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
                       
            modalManterDados.Hide();
            novoPedido();
            Session.Remove("PedidoManter");
        }
        private void formataCampos()
        {
            txtData_entrega.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
            txtData_cadastro.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
            txtVencimentoPg.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
            txthora.Attributes.Add("OnKeyPress", "javascript:return formataHora(this,event);");
            camposnumericos();
        }

        private void camposnumericos()
        {
            ArrayList campos = new ArrayList();
            campos.Add("txtValorPg");
            campos.Add("txtDesconto");
            campos.Add("txtQtde");

            FormataCamposNumericos(campos, conteudo);
            FormataCamposNumericos(campos, cabecalho);

            ArrayList camposInteiros = new ArrayList();
            camposInteiros.Add("txtPedido");
            camposInteiros.Add("txtPLU");

            FormataCamposInteiros(camposInteiros, conteudo);
            FormataCamposInteiros(camposInteiros, cabecalho);

        }


        private void limparCampos()
        {
            LimparCampos(cabecalho);
            LimparCampos(conteudo);
        }

        protected bool validaCamposObrigatorios()
        {
            if (validaCampos(cabecalho) && validaCampos(conteudo))
                return true;
            else
                return false;
        }

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array
            String[] campos = { "txtCliente_Fornec", 
                                    "", 
                                    "", 
                                    "" 
                                     };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array
            String[] campos = { "txtPedido", 
                                    "ddlStatus", 
                                    "txtUsuario", 
                                    "txtTotal",
                                    "TxtTotalItem"
                                     };


            return existeNoArray(campos, campo.ID + "");
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            pedidoDAO obj = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            if (obj != null)
            {
                Session.Remove("PedidoManter");
                Session.Add("PedidoManter", obj);
                Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            }
            Response.Redirect("~/modulos/pedidos/pages/pedidoCompraDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            if (ddlStatus.SelectedValue.Equals("2"))
            {
                lblError.Text = "NÃO É POSSIVEL FAZER ALTERAÇÕES EM PEDIDOS FECHADOS";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                status = "editar";
                editar(pnBtn);
                habilitarCampos(true);
                carregarDados();
                lblError.Text = "";
            }
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("PedidoCompra.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            modalPnConfirma.Show();

        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaCamposObrigatorios())
                {

                    carregarDadosObj();
                    pedidoDAO obj = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                    obj.salvar(status.Equals("incluir")); // se for incluir true se não falso;
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;


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

        protected void habilitarCampos(bool hab)
        {
            EnabledControls(cabecalho, hab);
            EnabledControls(conteudo, hab);
            gridItens.Enabled = hab;
            gridPagamentos.Enabled = hab;
            if (status.Equals("visualizar"))
            {
                btnImpressao.Visible = true;
                pedidoDAO obj = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                Session.Remove("PedidoPrintCompra");
                Session.Add("PedidoPrintCompra", obj);
            }
            else
            {
                btnImpressao.Visible = false;
            }

        }


        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("pedidoCompra.aspx");//colocar endereco pagina de pesquisa
        }

        protected void txt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TxtTotalItem.Text = ((Decimal.Parse(txtQtde.Text) * Decimal.Parse(txtEmbalagem.Text)) * Decimal.Parse(txtUnitario.Text)).ToString("N2");

            }
            catch (Exception)
            {

            }

            ModalItens.Show();

        }

        private void carregarDados()
        {
            pedidoDAO obj = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            txtPedido.Text = (obj.Pedido == null ? "" : obj.Pedido.ToString());
            ddlStatus.SelectedValue = obj.Status.ToString();
            txtCliente_Fornec.Text = obj.Cliente_Fornec.ToString();
            txtData_cadastro.Text = obj.Data_cadastroBr();
            txtData_entrega.Text = obj.Data_entregaBr();
            txthora.Text = obj.hora.ToString();
            txtDesconto.Text = string.Format("{0:0,0.00}", obj.Desconto);
            txtTotal.Text = string.Format("{0:0,0.00}", obj.Total);
            txtUsuario.Text = obj.Usuario.ToString();
            txtObs.Text = obj.Obs.ToString();
            txtCFOP.Text = obj.CFOP.ToString();
            txtfuncionario.Text = obj.funcionario.ToString();
            carregarGrids();
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

        protected void btnConfirmaItens_Click(object sender, ImageClickEventArgs e)
        {
            User usr = (User)Session["user"];
            pedidoDAO ped = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            pedido_itensDAO item = new pedido_itensDAO(usr);
            item.Filial = ped.Filial;
            item.Pedido = ped.Pedido;//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            item.Tipo = ped.Tipo;
            item.PLU = txtPLU.Text;
            item.Qtde = Decimal.Parse(txtQtde.Text);
            item.Embalagem = Decimal.Parse(txtEmbalagem.Text);
            item.unitario = Decimal.Parse(txtUnitario.Text);
            item.index = int.Parse(lblIndex.Text);
            ped.atualizaitem(item);


            ModalItens.Hide();
            carregarDados();
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
                User usr = (User)Session["user"];
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
            itempd.Tipo = 2;
            itempd.index = int.Parse(lblIndex.Text);
            ped.removeItem(itempd);

            txtTotal.Text = ped.Total.ToString("N2");
            Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), ped);

            carregarGrids();
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
            carregarDadosObj();
            Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            carregarMercadorias();
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
            User usr = (User)Session["user"];
            String sqlMercadoria = "Select mercadoria.plu PLU,isnull(ean.ean,'---')EAN,mercadoria.Ref_fornecedor REFERENCIA, rtrim(ltrim(mercadoria.descricao)) DESCRICAO from mercadoria left join ean on mercadoria.plu=ean.plu " +
                                    " where mercadoria.plu = '" + txtfiltromercadoria.Text + "' or ean like '%" + txtfiltromercadoria.Text + "%' or descricao like '%" + txtfiltromercadoria.Text + "%' ORDER BY mercadoria.Descricao";

            gridMercadoria1.DataSource = Conexao.GetTable(sqlMercadoria, usr, true);
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
                cabecalho.Add("embalagem");
                cabecalho.Add("Preco");
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
            Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), todosSel);
            GridMercadoriaSelecionado.DataSource = Conexao.GetArryTable(todosSel);
            GridMercadoriaSelecionado.DataBind();

            modalMercadorialista.Show();

        }
        protected void gridItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            LimparCampos(pnItens);
            pedidoDAO ped = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            int index = Convert.ToInt32(e.CommandArgument);
            pedido_itensDAO itemPd = ped.item(index);
            carregarDadosObj();
            carregarDadosItens(itemPd);
        }

        protected void gridPagamentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            pedidoDAO ped = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            int index = Convert.ToInt32(e.CommandArgument);
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
            obj.Desconto = (txtDesconto.Text.Equals("") ? 0 : Decimal.Parse(txtDesconto.Text));
            obj.Total = (txtTotal.Text.Equals("") ? 0 : Decimal.Parse(txtTotal.Text));
            obj.Usuario = txtUsuario.Text;
            obj.Obs = txtObs.Text;
            obj.CFOP = (txtCFOP.Text.Equals("") ? 0 : Decimal.Parse(txtCFOP.Text));
            obj.funcionario = txtfuncionario.Text;
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
                case "Fornecedor":
                    sqlLista = "select Fornecedor, razao_social from fornecedor where Fornecedor like '%" + TxtPesquisaLista.Text + "%' or razao_social like '%" + TxtPesquisaLista.Text + "%'"; ;
                    lbllista.Text = "Escolha um Fornecedor";
                    break;
                case "PLU":
                    sqlLista = "select Plu, Descricao from mercadoria where plu like '%" + TxtPesquisaLista.Text + "%' or desricao like '%" + TxtPesquisaLista.Text + "%'"; ;
                    lbllista.Text = "Escolha um Produto";
                    break;
                case "Funcionario":
                    sqlLista = "select Nome from usuarios_web where nome like '%" + TxtPesquisaLista.Text + "%'";
                    lbllista.Text = "Escolha um Funcionario";
                    break;
                case "CFOP":
                    sqlLista = "select codigo_operacao, descricao from natureza_operacao where saida=0 and codigo_operacao like '%" + TxtPesquisaLista.Text + "%' and descricao like '%" + TxtPesquisaLista.Text + "%'";
                    lbllista.Text = "Escolha a Natureza de Operação";
                    break;
                case "TipoPagamento":
                    sqlLista = "select tipo_pagamento Tipo ,Prazo from tipo_pagamento where tipo_pagamento like '%" + TxtPesquisaLista.Text + "%' ";
                    lbllista.Text = "Escolha a Tipo de pagamento";
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
            modalPnFundo.Show();
            TxtPesquisaLista.Focus();
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
                case "imgBtnFornecedor":
                    Session.Add("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "Fornecedor");
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

                String listaAtual = (String)Session["camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                Session.Remove("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));

                if (listaAtual.Equals("Fornecedor"))
                {

                    txtCliente_Fornec.Text = ListaSelecionada(1);
                    fornecedorDAO fornOcorrencia = new fornecedorDAO(txtCliente_Fornec.Text, null);
                    if (fornOcorrencia.ocorrenciasPendentes > 0)
                    {
                        lblError.Text = "Existem pendências com este fornecedor.";
                    }
                    else
                    {
                        lblError.Text = "";
                    }
                    fornOcorrencia = null;
                }
                else if (listaAtual.Equals("PLU"))
                {
                    User usr = (User)Session["User"];
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
                    txtVencimentoPg.Text = DateTime.Now.AddDays(int.Parse(ListaSelecionada(2))).ToString("dd/MM/yyyy");
                    ModalPagamentos.Show();

                    
                }


                modalPnFundo.Hide();
                carregarGrids();
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
            User usr = (User)Session["User"];
            ArrayList selecionados = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            if (selecionados != null && selecionados.Count > 1)
            {
                pedidoDAO obj = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                foreach (GridViewRow sl in GridMercadoriaSelecionado.Rows)
                {

                    pedido_itensDAO item = new pedido_itensDAO(usr);

                    item.PLU = sl.Cells[1].Text;
                    TextBox txtQtdItem = (TextBox)sl.FindControl("txtQtd");
                    if (!txtQtdItem.Text.Equals("------"))
                    {
                        item.Qtde = (txtQtdItem.Text.Equals("") ? 1 : Decimal.Parse(txtQtdItem.Text));
                        TextBox txtPrecoItem = (TextBox)sl.FindControl("txtPreco");
                        item.Embalagem = decimal.Parse(sl.Cells[5].Text);
                        item.unitario = (txtPrecoItem.Text.Equals("") ? 1 : Decimal.Parse(txtPrecoItem.Text));
                        item.inserido = true;
                        obj.addItens(item);
                        Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                        Session.Add("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
                        carregarDados();

                        Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                        modalMercadorialista.Hide();


                    }
                    else
                    {
                        modalMercadorialista.Show();

                    }
                }


            }
            else
            {
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
            ArrayList todosSel = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
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
                    sel.Add(merc.Descricao_resumida);
                    sel.Add("1");
                    sel.Add(merc.Embalagem.ToString());
                    sel.Add(merc.preco_compra.ToString("N2"));
                    todosSel.Add(sel);
                }

            }
            ViewState["gridLinha"] = todosSel.Count - 2;
            Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), todosSel);
            carregarMercadorias();
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
            carregarMercadorias();
        }
        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                pedidoDAO obj = (pedidoDAO)Session["pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                obj.excluir();
                modalPnConfirma.Hide();
                lblError.Text = "Registro Excluido com sucesso";
                limparCampos();
                status = "pesquisar";
                carregabtn(pnBtn);
                Session.Remove("pedido" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            }
            catch (Exception err)
            {

                lblError.Text = "Não foi possivel Excluir o registro pelo error:" + err.Message;

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
        public static string[] GetNomesFornecedor(string prefixText, int count)
        {
            String sql = "Select fornecedor from fornecedor where fornecedor like '" + (prefixText.Length > 4 ? "%" : "") + prefixText + "%'";
            return Conexao.retornaArray(sql, prefixText.Length);
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

    }
}