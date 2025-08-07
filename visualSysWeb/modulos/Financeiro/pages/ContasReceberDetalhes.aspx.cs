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

namespace visualSysWeb.modulos.Financeiro.pages
{
    public partial class ContasReceberDetalhes : visualSysWeb.code.PagePadrao
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            if (usr != null)
            {
                conta_a_receberDAO obj;

                if (Request.Params["novo"] != null)
                {
                    if (!IsPostBack)
                    {
                        status = "incluir";
                        habilitar(true);
                        txtEntrada.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtVencimento.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtEmissao.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtPagamento.Text = DateTime.Now.ToString("dd/MM/yyyy");

                        txtCodigo_Centro_Custo.Text = Funcoes.valorParametro("CENTRO_CUSTO_REC", usr);
                        

                        txtid_cc.Text = Funcoes.valorParametro("CONTA_CC_REC", usr);

                        obj = new conta_a_receberDAO(usr);
                        obj.Codigo_Centro_Custo = txtCodigo_Centro_Custo.Text;
                        txtCentroDescricao.Text = obj.Centro_custo_Descricao;


                        obj.id_cc = txtid_cc.Text;
                       
                        txtbanco.Text = obj.banco;
                        txtagencia.Text = obj.agencia;
                        txtagencia.Text = obj.conta;

                        Session.Remove("objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                        Session.Add("objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
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
                                String cliente = Request.Params["codCliente"].ToString();// colocar o campo index da tabela
                                String dtEmissao = Request.Params["Emissao"].ToString();
                                if (index.Equals("------"))
                                {
                                    Response.Redirect("ContasReceber.aspx");
                                }
                                status = "visualizar";
                                obj = new conta_a_receberDAO(index, cliente, dtEmissao, usr);
                                Session.Remove("objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                                Session.Add("objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
                                carregarDados();

                            }
                            else
                            {
                                obj = (conta_a_receberDAO)Session["objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                            }


                            if (status.Equals("visualizar"))
                            {
                                habilitar(false);
                            }
                            else
                            {
                                habilitar(true);
                            }
                        }
                        catch (Exception err)
                        {
                            lblError.Text = err.Message;
                        }
                    }
                }
                if (DDlStatus.SelectedItem.Text.Equals("CONCLUIDO"))
                {
                    carregabtn(pnBtn, null, null, null, null, "Estornar", null);
                }
                else
                {
                    carregabtn(pnBtn, null, null, null, null, "Cancelar", null);
                    //carregabtn(pnBtn);
                }


            }
            formatarCampos();
        }


        private void habilitar(bool enable)
        {
            EnabledControls(conteudo, enable);
            EnabledControls(cabecalho, enable);

            if (status.Equals("editar"))
            {
                imgBtnCliente.Visible = false;
            }

            if (status.Equals("visualizar") && DDlStatus.SelectedValue.Equals("ABERTO"))
            {

                BtnBaixa.Visible = true;
                imgBtnBaixar.Visible = true;
            }
            else
            {
                BtnBaixa.Visible = false;
                imgBtnBaixar.Visible = false;
            }

        }

        private void formatarCampos()
        {
            //            txtEntrada.Attributes.Add("OnKeyUp", "javascript:return formataData(this,event);");
            //           txtEmissao.Attributes.Add("OnKeyUp", "javascript:return formataData(this,event);");
            //          txtVencimento.Attributes.Add("OnKeyUp", "javascript:return formataData(this,event);");


            camposnumericos();

        }


        private void camposnumericos()
        {
            String[] campos = { "txtValor", 
                                    "txtValor_Pago", 
                                    "txtDesconto",
                                    "txtAcrescimo"
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

        private void limparCampos()
        {
            LimparCampos(cabecalho);
            LimparCampos(conteudo);
        }

        protected bool validaCamposObrigatorios()
        {
            bool retorno = false;
            txtEmissao.BackColor = System.Drawing.Color.White;
            txtVencimento.BackColor = System.Drawing.Color.White;
            if (!txtEmissao.Text.Trim().Equals(""))
            {
                if (txtEntrada.Text.Length == 0)
                    txtEntrada.Text = txtEmissao.Text;

                if (DateTime.Parse(txtEmissao.Text) > DateTime.Parse(txtEntrada.Text))
                {
                    txtEmissao.BackColor = System.Drawing.Color.Red;
                    throw new Exception("A data de emissão não pode ser maior que a data de entrada!");

                }

            }

            if (!txtVencimento.Text.Trim().Equals(""))
            {
                if (DateTime.Parse(txtVencimento.Text) < DateTime.Parse(txtEmissao.Text))
                {
                    txtVencimento.BackColor = System.Drawing.Color.Red;
                    throw new Exception("A data de vencimento não pode ser menor que a data de emissão!");

                }
            }
            Decimal vValor;
            Decimal.TryParse(txtValor.Text, out vValor);
            if (vValor <= 0)
            {
                txtValor.BackColor = System.Drawing.Color.Red;
                throw new Exception("O Valor do documento não pode estar zerado ou negativo!");
            }

            Decimal vValorPagar;
            Decimal.TryParse(txtTotalPagar.Text, out vValorPagar);

            if (vValorPagar <= 0)
            {
                txtTotalPagar.BackColor = System.Drawing.Color.Red;
                throw new Exception("O valor a pagar não pode estar zerado ou negativo!");
            }


            if (chkBaixa_Automatica.Checked)
            {
                txtPagamento.BackColor = System.Drawing.Color.White;
                txtValor_Pago.BackColor = System.Drawing.Color.White;
                if (txtPagamento.Text.Equals(""))
                {
                    txtPagamento.BackColor = System.Drawing.Color.Red;
                    throw new Exception("O campo Data de pagamento não foi preenchido!");
                }
                Decimal vPago;
                Decimal.TryParse(txtValor_Pago.Text, out vPago);

                if (vPago <= 0)
                {
                    txtValor_Pago.BackColor = System.Drawing.Color.Red;
                    throw new Exception("O campo valor pago não foi preenchido ou esta negativo!");

                }
            }


            if (validaCampos(cabecalho) && validaCampos(conteudo))
                retorno = true;
            else
                retorno = false;

            if (retorno && !txtPagamento.Text.Trim().Equals(""))
            {
                if (DateTime.Parse(txtPagamento.Text) < DateTime.Parse(txtEmissao.Text))
                {
                    modalConfirmaData.Show();
                    retorno = false;
                }
            }
            return retorno;
        }

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array
            String[] campos = { "txtValor", 
                                    "txtDocumento", 
                                    "txtCodigo_Cliente", 
                                    "txtEmissao",
                                    "txtVencimento",
                                    "txtCodigo_Centro_Custo"
                                     };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array
            if (status.Equals("editar"))
            {
                if (campo.ID.Equals("txtDocumento"))
                {
                    return true;
                }
                if (campo.ID.Equals("txtCodigo_Cliente") || campo.ID.Equals("txtNome_Cliente"))
                {
                    imgBtnCliente.Visible = false;
                    return true;
                }



            }

            String[] campos = { "txtTotalPagar", 
                                    "DDlStatus", 
                                    "txtusuario",
                                    "txtpdv",
                                    "txtoperador",
                                    "txtEntrada" ,
                                    "txtfinalizadora",
                                    "txtid_finalizadora",
                                    "txtdocumento_emitido",
                                    "txtTipoCartao",
                                    "txtTaxa"
                                     };
            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("ContasReceberDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            if (!DDlStatus.SelectedItem.Text.Equals("CONCLUIDO") && !DDlStatus.SelectedItem.Text.Equals("CANCELADO"))
            {

                lblError.Text = "";
                status = "editar";
                editar(pnBtn);
                habilitar(true);
                carregarDados();
                User usr = (User)Session["User"];
                txtusuario.Text = usr.getNome();
                BtnBaixa.Visible = false;
                imgBtnBaixar.Visible = false;
            }
            else
            {
                lblError.Text = "Status não Permite alterações";
                status = "visualizar";
                lblError.ForeColor = System.Drawing.Color.Red;
                habilitar(false);

            }
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ContasReceber.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            if (DDlStatus.SelectedItem.Text.Equals("CONCLUIDO"))
            {
                Label1.Text = "Confirma Estorno?";
                ModalConfirma.Show();
            }
            else if (DDlStatus.SelectedItem.Text.Equals("ABERTO"))
            {

                Label1.Text = "Confirma Cancelamento?";
                ModalConfirma.Show();
            }
            else
            {
                User usr = (User)Session["User"];
                if (usr.adm())
                {
                    Label1.Text = "Confirma Reativação do Titulo?";
                    ModalConfirma.Show();
                }
                else
                {
                    lblError.Text = "Status não Permite alterações";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }
        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!DDlStatus.SelectedItem.Text.Equals("CONCLUIDO") && !DDlStatus.SelectedItem.Text.Equals("CANCELADO"))
                {

                    if (validaCamposObrigatorios())
                    {
                        salvartitulo();
                    }
                    else
                    {
                        lblError.Text = "Campo Obrigatorio não preenchido";
                        lblError.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception err)
            {
                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void salvartitulo()
        {



            carregarDadosObj();
            conta_a_receberDAO obj = (conta_a_receberDAO)Session["objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            if (chkBaixa_Automatica.Checked)
                obj.status = 2;
            obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
            lblError.Text = "Salvo com Sucesso";
            lblError.ForeColor = System.Drawing.Color.Blue;
            habilitar(false);
            visualizar(pnBtn);
            Session.Remove("objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
            carregarDados();
        }
        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ContasReceber.aspx");//colocar endereco pagina de pesquisa
        }


        private void carregarDados()
        {
            conta_a_receberDAO obj = (conta_a_receberDAO)Session["objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            txtDocumento.Text = obj.Documento.ToString();
            txtCodigo_Cliente.Text = obj.Codigo_Cliente.ToString();
            txtNome_Cliente.Text = obj.nomeCliente;
            txtCodigo_Centro_Custo.Text = obj.Codigo_Centro_Custo.ToString();

            txtCentroDescricao.Text = obj.Centro_custo_Descricao;
            txtValor.Text = string.Format("{0:0,0.00}", obj.Valor);
            txtDesconto.Text = string.Format("{0:0,0.00}", obj.Desconto);
            txtAcrescimo.Text = obj.acrescimo.ToString("N2");
            txtTotalPagar.Text = (((obj.Valor - obj.Desconto) + obj.acrescimo) - obj.taxa).ToString("N2");
            txtObs.Text = obj.Obs.ToString();
            txtEmissao.Text = obj.EmissaoBr();
            txtVencimento.Text = obj.VencimentoBr();
            txtEntrada.Text = obj.entradaBr();
            txtPagamento.Text = obj.PagamentoBr();
            txtValor_Pago.Text = string.Format("{0:0,0.00}", obj.Valor_Pago);
            txtid_cc.Text = obj.id_cc.ToString();
            txtcheque.Text = obj.cheque.ToString();
            txtagencia.Text = obj.agencia.ToString();
            txtconta.Text = obj.conta.ToString();
            txtbanco.Text = obj.banco.ToString();
            chkBaixa_Automatica.Checked = obj.Baixa_Automatica;
            txtusuario.Text = obj.usuario.ToString();//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            //Status na sequencia 4 é igual ao valor 7.
            if (obj.status > 4)
            {
                DDlStatus.SelectedIndex = 4;
            }
            else
            {
                DDlStatus.SelectedIndex = obj.status - 1;
            }
            txtoperador.Text = obj.operador.ToString();
            txtpdv.Text = obj.pdv.ToString();
            txtfinalizadora.Text = obj.id_finalizadora.ToString().Trim();
            txtid_finalizadora.Text = obj.finalizadora.ToString().Trim();
            txtdocumento_emitido.Text = obj.documento_emitido.ToString();
            txtTipoRecebimento.Text = obj.tipo_recebimento;
            chkNota_servico.Checked = obj.nota_servico;
            txtTipoCartao.Text = obj.tipoCartao;
            txtTaxa.Text = obj.taxa.ToString("N2");
            gridCupons.DataSource = obj.cupons;
            gridCupons.DataBind();

            if (status.Equals("visualizar") && DDlStatus.SelectedValue.Equals("ABERTO"))
            {
                BtnBaixa.Visible = true;
                imgBtnBaixar.Visible = true;
            }
            else
            {
                BtnBaixa.Visible = false;

                imgBtnBaixar.Visible = false;
            }


        }

        private void carregarDadosObj()
        {
            conta_a_receberDAO obj = (conta_a_receberDAO)Session["objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            obj.Documento = txtDocumento.Text;
            obj.Codigo_Cliente = txtCodigo_Cliente.Text;

            obj.Codigo_Centro_Custo = txtCodigo_Centro_Custo.Text;
            obj.Valor = Decimal.Parse(txtValor.Text);
            obj.Desconto = (txtDesconto.Text.Trim().Equals("") ? 0 : Decimal.Parse(txtDesconto.Text));
            obj.acrescimo = (txtAcrescimo.Text.Trim().Equals("") ? 0 : Decimal.Parse(txtAcrescimo.Text));
            obj.Obs = txtObs.Text;
            obj.Emissao = (txtEmissao.Text.Trim().Equals("") ? new DateTime() : DateTime.Parse(txtEmissao.Text));
            obj.Vencimento = (txtVencimento.Text.Trim().Equals("") ? new DateTime() : DateTime.Parse(txtVencimento.Text));
            obj.entrada = (txtEntrada.Text.Trim().Equals("") ? new DateTime() : DateTime.Parse(txtEntrada.Text));
            obj.Pagamento = (txtPagamento.Text.Trim().Equals("") ? new DateTime() : DateTime.Parse(txtPagamento.Text));
            obj.Valor_Pago = (txtValor_Pago.Text.Trim().Equals("") ? 0 : Decimal.Parse(txtValor_Pago.Text));
            obj.id_cc = txtid_cc.Text;
            obj.cheque = txtcheque.Text;
            obj.agencia = txtagencia.Text;
            obj.conta = txtconta.Text;
            obj.banco = txtbanco.Text;
            obj.Baixa_Automatica = chkBaixa_Automatica.Checked;
            obj.usuario = txtusuario.Text;
            //Status 7 é igual a SUSPENSO
            if (DDlStatus.Text == "SUSPENSO")
            {
                obj.status = 7;
            }
            else
            {
                obj.status = DDlStatus.SelectedIndex + 1;
            }
            obj.operador = txtoperador.Text;
            obj.pdv = (txtpdv.Text.Trim().Equals("") ? 0 : int.Parse(txtpdv.Text));
            obj.finalizadora = (txtfinalizadora.Text.Trim().Equals("") ? 0 : int.Parse(txtid_finalizadora.Text));
            obj.id_finalizadora = txtfinalizadora.Text;
            obj.documento_emitido = txtdocumento_emitido.Text;
            obj.tipo_recebimento = txtTipoRecebimento.Text;

            bool aVista = Conexao.retornaUmValor("Select a_Vista from tipo_pagamento where tipo_pagamento ='" + txtTipoRecebimento.Text + "'", null).Equals("1");
            obj.aVista = aVista;
            obj.nota_servico = chkNota_servico.Checked;
            Session.Remove("objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);

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

        protected void Img_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            TxtPesquisaLista.Text = "";
            String campo = "";

            switch (btn.ID)
            {
                case "imgBtnCliente":
                    campo = "txtCliente";

                    break;
                case "imgBtnCentroCusto":
                    campo = "txtCodigo_Centro_Custo";
                    break;
                case "imgBtntxtId_cc":
                    campo = "txtid_cc";
                    break;
                case "imgBtnTipoRecebimento":
                    campo = "txtTipoRecebimento";
                    break;

            }

            Session.Add("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), campo);


            exibeLista();
        }
        protected void exibeLista()
        {
            lblErroPesquisa.Text = "";
            User usr = (User)Session["User"];
            String or = (String)Session["camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            String sqlLista = "";


            switch (or)
            {
                case "txtCliente":
                    lbllista.Text = "Escolha um Cliente";
                    sqlLista = "select codigo_cliente Codigo,nome_cliente nome from cliente where codigo_cliente like '%" + TxtPesquisaLista.Text + "%' or nome_cliente like '%" + TxtPesquisaLista.Text + "%'";

                    break;
                case "txtCodigo_Centro_Custo":
                    lbllista.Text = "Escolha um Centro de Custo";
                    sqlLista = "select Codigo_centro_custo codigo, descricao_centro_custo Descricao, id_cc Conta from centro_custo where Codigo_centro_custo like '%" + TxtPesquisaLista.Text + "%' or descricao_centro_custo like '%" + TxtPesquisaLista.Text + "%' or id_cc like '%" + TxtPesquisaLista.Text + "%'";
                    break;
                case "txtid_cc":

                    sqlLista = "select id_cc  from conta_corrente where id_cc like '%" + TxtPesquisaLista.Text + "%'";
                    lbllista.Text = "Conta Corrente";
                    break;
                case "txtTipoRecebimento":
                    sqlLista = "select tipo_pagamento from tipo_pagamento where tipo_pagamento like '%" + TxtPesquisaLista.Text + "%'";
                    lbllista.Text = "Pagamentos";
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
        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                conta_a_receberDAO obj = (conta_a_receberDAO)Session["objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                if (DDlStatus.SelectedItem.Text.Equals("CONCLUIDO"))
                {
                    obj.Estornar();
                    ModalConfirma.Hide();
                    lblError.Text = "Registro Estornado com sucesso";
                    Session.Remove("objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
                    carregarDados();
                    carregabtn(pnBtn);
                }
                else if (DDlStatus.SelectedItem.Text.Equals("ABERTO"))
                {
                    obj.excluir();
                    ModalConfirma.Hide();
                    lblError.Text = "Registro Cancelado com sucesso";
                    limparCampos();
                    pesquisar(pnBtn);
                    Session.Remove("objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                }
                else if (DDlStatus.SelectedItem.Text.Equals("CANCELADO") || DDlStatus.SelectedItem.Text.Equals("SUSPENSO"))
                {
                    obj.reativaTitulo();
                    String url = Request.Url.ToString().Replace(obj.Documento, obj.Documento.Replace("C", ""));
                    Response.Redirect(url);
                }
                    habilitar(false);
            }
            catch (Exception err)
            {
                lblError.Text = "Não foi possivel Excluir o registro error:" + err.Message;
            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            ModalConfirma.Hide();
            habilitar(false);
        }


        protected void btnFechar_Click(object sender, ImageClickEventArgs e)
        {

            String selecionado = ListaSelecionada(1);

            if (!selecionado.Equals("") && !selecionado.Equals("------"))
            {

                conta_a_receberDAO recb = (conta_a_receberDAO)Session["objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                String listaAtual = (String)Session["camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                Session.Remove("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));

                if (listaAtual.Equals("txtCliente"))
                {

                    txtCodigo_Cliente.Text = ListaSelecionada(1);
                    txtNome_Cliente.Text = ListaSelecionada(2);

                }
                else if (listaAtual.Equals("txtCodigo_Centro_Custo"))
                {
                    txtCodigo_Centro_Custo.Text = ListaSelecionada(1);
                    txtCentroDescricao.Text = ListaSelecionada(2);
                }

                else if (listaAtual.Equals("txtid_cc"))
                {

                    txtid_cc.Text = ListaSelecionada(1);
                    recb.id_cc = txtid_cc.Text;
                    txtbanco.Text = recb.banco;
                    txtagencia.Text = recb.agencia;
                    txtconta.Text = recb.conta;

                }
                else if (listaAtual.Equals("txtTipoRecebimento"))
                {
                    txtTipoRecebimento.Text = ListaSelecionada(1);
                }
                Session.Remove("objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), recb);
                modalPnFundo.Hide();
            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                modalPnFundo.Show();
            }

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
                        return item.Cells[campo].Text;
                    }
                }
            }

            return "";
        }


        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalPnFundo.Hide();
        }

        protected void txtCalcula_TextChanged(object sender, EventArgs e)
        {
            try
            {

                lblError.Text = "";
                ((TextBox)sender).BackColor = System.Drawing.Color.White;

                conta_a_receberDAO recb = (conta_a_receberDAO)Session["objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

                switch (((TextBox)sender).ID)
                {
                    case "txtDesconto":
                        //recb.Desconto = Decimal.Parse(txtDesconto.Text);
                        Decimal.TryParse(txtDesconto.Text, out recb.Desconto);
                        txtDesconto.Text = recb.Desconto.ToString("N2");
                        txtAcrescimo.Focus();
                        break;
                    case "txtAcrescimo":
                        //recb.acrescimo = Decimal.Parse(txtAcrescimo.Text);
                        Decimal.TryParse(txtAcrescimo.Text, out recb.acrescimo);

                        txtAcrescimo.Text = recb.acrescimo.ToString("N2");
                        txtPagamento.Focus();
                        break;
                    case "txtValor":
                        //recb.Valor = Decimal.Parse(txtValor.Text);
                        Decimal.TryParse(txtValor.Text, out recb.Valor);
                        txtValor.Text = recb.Valor.ToString("N2");
                        txtDesconto.Focus();
                        break;
                }

                txtTotalPagar.Text = ((recb.Valor - recb.Desconto) + recb.acrescimo).ToString("N2");
                Session.Remove("objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), recb);
            }
            catch (Exception)
            {
                lblError.Text = "Valor Inválido";
                lblError.ForeColor = System.Drawing.Color.Red;
                ((TextBox)sender).BackColor = System.Drawing.Color.Red;

            }

        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetNomesClientes(string prefixText, int count)
        {
            String sql = "Select codigo_cliente +'|'+ nome_cliente from cliente where nome_cliente like '" + (prefixText.Length > 4 ? "%" : "") + prefixText + "%'";
            return Conexao.retornaArray(sql, prefixText.Length);
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetConta(string prefixText, int count)
        {
            String sql = "select id_cc  from conta_corrente where id_cc like '" + (prefixText.Length > 4 ? "%" : "") + prefixText + "%'";
            return Conexao.retornaArray(sql, prefixText.Length);
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetCentroCusto(string prefixText, int count)
        {
            String sql = "Select codigo_centro_custo +'|'+ descricao_centro_custo from centro_custo where descricao_centro_custo like '" + (prefixText.Length > 4 ? "%" : "") + prefixText + "%'";
            return Conexao.retornaArray(sql, prefixText.Length);
        }
        protected void txtCodigo_Centro_custo_TextChanged(object sender, EventArgs e)
        {

            txtCentroDescricao.Text = Conexao.retornaUmValor("select descricao_centro_custo from centro_custo where codigo_centro_custo ='" + txtCodigo_Centro_Custo.Text + "'", null);
            txtCentroDescricao.Focus();

        }

        protected void txtid_cc_TextChanged(object sender, EventArgs e)
        {
            if (!txtid_cc.Text.Equals(""))
            {
                conta_a_receberDAO recb = (conta_a_receberDAO)Session["objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

                recb.id_cc = txtid_cc.Text;
                txtbanco.Text = recb.banco;
                txtagencia.Text = recb.agencia;
                txtconta.Text = recb.conta;
            }

        }
        protected void txtCentroDescricao_TextChanged(object sender, EventArgs e)
        {
            if (txtCentroDescricao.Text.IndexOf("|") >= 0)
            {
                txtCodigo_Centro_Custo.Text = txtCentroDescricao.Text.Substring(0, txtCentroDescricao.Text.IndexOf("|"));
                txtCentroDescricao.Text = txtCentroDescricao.Text.Substring(txtCentroDescricao.Text.IndexOf("|") + 1);
                txtObs.Focus();
            }
        }
        protected void txtCodigo_Cliente_TextChanged(object sender, EventArgs e)
        {
            if (!txtCodigo_Cliente.Text.Trim().Equals(""))
            {
                txtNome_Cliente.Text = Conexao.retornaUmValor("select nome_cliente from cliente where codigo_cliente ='" + txtCodigo_Cliente.Text + "'", null);
            }
            else
            {
                txtNome_Cliente.Focus();
            }
        }

        protected void txtNomeCliente_TextChanged(object sender, EventArgs e)
        {
            if (txtNome_Cliente.Text.IndexOf("|") >= 0)
            {
                txtCodigo_Cliente.Text = txtNome_Cliente.Text.Substring(0, txtNome_Cliente.Text.IndexOf("|"));
                txtNome_Cliente.Text = txtNome_Cliente.Text.Substring(txtNome_Cliente.Text.IndexOf("|") + 1);
                txtEntrada.Focus();
            }

        }

        protected void btnConfirmaData_Click(object sender, ImageClickEventArgs e)
        {
            User usr = (User)Session["User"];
            txtObs.Text = "DATA DE PAGAMENTO ANTERIOR A EMISSÃO AUTORIZADA PELO CLIENTE , CONFIRMADO PELO USUARIO:" + usr.getUsuario();
            salvartitulo();
        }
        protected void btnCancelaData_Click(object sender, ImageClickEventArgs e)
        {
            modalConfirmaData.Hide();

        }


        protected void baixarTitulo()
        {

            txtPagamento.BackColor = System.Drawing.Color.White;
            txtValor_Pago.BackColor = System.Drawing.Color.White;
            if (txtPagamento.Text.Equals(""))
            {
                txtPagamento.BackColor = System.Drawing.Color.Red;
                throw new Exception("O Campo Data de Pagamento não foi preenchido!");
            }
            Decimal vPago;
            Decimal.TryParse(txtValor_Pago.Text, out vPago);

            if (vPago <= 0)
            {
                txtValor_Pago.Text = txtTotalPagar.Text;
                //txtValor_Pago.BackColor = System.Drawing.Color.Red;
                //throw new Exception("O Campo Valor Pago não foi preenchido!");

            }
            if (txtid_cc.Text.Equals(""))
            {
                txtid_cc.BackColor = System.Drawing.Color.Red;
                throw new Exception("Não foi informado uma Conta para baixa!");
            }


            carregarDadosObj();
            conta_a_receberDAO obj = (conta_a_receberDAO)Session["objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            obj.status = 2;
            if (obj.ValorPagar > obj.Valor_Pago)
            {
                obj.Desconto += (obj.ValorPagar - obj.Valor_Pago);
            }
            else if (obj.ValorPagar < obj.Valor_Pago)
            {
                obj.acrescimo += (obj.Valor_Pago - obj.ValorPagar);
            }


            obj.salvar(false);
            Session.Remove("objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("objcontaReceber" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
            carregarDados();
            status = "visualizar";
            lblError.Text = "Documento Baixado com Sucesso";
            lblError.ForeColor = System.Drawing.Color.Blue;

            habilitar(false);



        }
        protected void imgBtnBaixar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                baixarTitulo();

            }
            catch (Exception err)
            {

                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;

            }
        }
        protected void chkBaixa_Automatica_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBaixa_Automatica.Checked)
            {
                txtPagamento.CssClass = "campoObrigatorio";
                txtValor_Pago.CssClass = "campoObrigatorio";
            }
            else
            {
                txtPagamento.CssClass = "";
                txtValor_Pago.CssClass = "numero";
            }

        }



    }
}