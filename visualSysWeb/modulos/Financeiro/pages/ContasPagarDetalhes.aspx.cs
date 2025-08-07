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
using visualSysWeb.modulos.NotaFiscal.dao;

namespace visualSysWeb.modulos.Financeiro.pages
{
    public partial class ContasPagarDetalhes : visualSysWeb.code.PagePadrao
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];


            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    conta_a_pagarDAO obj = new conta_a_pagarDAO(usr);
                    Session.Remove("objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);

                    status = "incluir";

                    txtentrada.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtemissao.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtQtdeParcelas.Text = "1";

                    txtVencimento.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    DllStatus.SelectedIndex = 1;
                    txtusuario.Text = usr.getUsuario();
                    BtnBaixa.Visible = false;
                    imgBtnBaixar.Visible = false;
                    divParcelas.Visible = true;

                  
                }
                habilitar(true);

            }
            else
            {
                divParcelas.Visible = false;
                if (Request.Params["documento"] != null && Request.Params["fornecedor"] != null && Request.Params["valor"] != null)  // colocar o campo index da tabela
                {
                    try
                    {
                        if (!IsPostBack)
                        {

                            String documento = Request.Params["documento"].ToString();// colocar o campo index da tabela
                            if (documento.Equals("------"))
                            {
                                Response.Redirect("ContasPagar.aspx");
                            }
                            String fornecedor = Request.Params["fornecedor"].ToString();// colocar o campo index da tabela
                            Decimal valor = Decimal.Parse(Request.Params["valor"].ToString());
                            status = "visualizar";
                            conta_a_pagarDAO obj = new conta_a_pagarDAO(documento, fornecedor, usr);
                            Session.Remove("objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                            Session.Add("objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);

                            carregarDados(obj);
                        }
                        if (status.Equals("visualizar"))
                        {
                            habilitar(false);
                        }
                        else
                        {
                            habilitar(true);
                        }
                        if (Request.Params["excluir"] != null)
                        {
                            modalPnConfirma.Show();
                        }
                    }
                    catch (Exception err)
                    {
                        lblError.Text = err.Message;
                    }
                }
                else
                {
                    Response.Redirect("ContasPagar.aspx");
                }
            }

            if (DllStatus.SelectedItem.Text.Equals("CONCLUIDO"))
            {
                carregabtn(pnBtn, null, null, null, null, "Estornar", null);
            }
            else
            {
                carregabtn(pnBtn, null, null, null, null, "Cancelar", null);
                //carregabtn(pnBtn);
            }

            formatarCampos();


        }

        private void formatarCampos()
        {
            txtentrada.Attributes.Add("OnKeyUp", "javascript:return formataData(this,event);");
            txtemissao.Attributes.Add("OnKeyUp", "javascript:return formataData(this,event);");
            txtVencimento.Attributes.Add("OnKeyUp", "javascript:return formataData(this,event);");
            txtPagamento.Attributes.Add("OnKeyUp", "javascript:return formataData(this,event);");

            camposnumericos();

        }

        private void habilitar(bool enable)
        {
            EnabledControls(conteudo, enable);
            EnabledControls(cabecalho, enable);

            if (status.Equals("editar"))
            {
                imgBtnFornecedor.Visible = false;
                divParcelas.Visible = false;
                txtQtdeParcelas.Text = "1";
            }

            divQtdeDias.Visible = ddlTipoParcelas.Text.Equals("Dia");

            DivCodBarras.Visible = false;
            if (status.Equals("visualizar") && DllStatus.SelectedValue.Equals("1"))
            {

                BtnBaixa.Visible = true;
                imgBtnBaixar.Visible = true;
            }
            else
            {
                BtnBaixa.Visible = false;
                imgBtnBaixar.Visible = false;
                
                if(status.Equals("incluir"))
                {
                    DivCodBarras.Visible = true;
                }

            }

            ocultaSimplificado();
        }
        private void camposnumericos()
        {
            String[] campos = { "txtValor",
                                    "txtValor_Pago",
                                    "txtDesconto",
                                    "txtParcial",
                                    "txtAcrescimo"

                                     };
            foreach (String item in campos)
            {
                TextBox txt = (TextBox)cabecalho.FindControl(item);
                if (txt == null)
                    txt = (TextBox)conteudo.FindControl(item);

                if (txt != null)
                {
                    txt.Attributes.Add("OnKeyPress", "javascript:return numeros(this,event);");
                    txt.AutoPostBack = true;
                    txt.TextChanged += txt_TextChanged;
                }
            }
        }

        private void limparCampos()
        {
            LimparCampos(cabecalho);
            LimparCampos(conteudo);
        }

        protected bool validaCamposObrigatorios()
        {

            txtemissao.BackColor = System.Drawing.Color.White;
            txtVencimento.BackColor = System.Drawing.Color.White;
            if (!txtemissao.Text.Trim().Equals(""))
            {
                if (DateTime.Parse(txtemissao.Text) > DateTime.Parse(txtentrada.Text))
                {
                    txtemissao.BackColor = System.Drawing.Color.Red;
                    throw new Exception("A data de Emissão não pode ser maior que a data de Entrada!");

                }
            }

            if (!txtVencimento.Text.Trim().Equals(""))
            {
                if (DateTime.Parse(txtVencimento.Text) < DateTime.Parse(txtemissao.Text))
                {
                    txtVencimento.BackColor = System.Drawing.Color.Red;
                    throw new Exception("A data de vencimento não pode ser Menor que a data de Emissão!");

                }
            }

            Decimal vValor;
            Decimal.TryParse(txtValor.Text, out vValor);
            if (vValor <= 0)
            {
                txtValor.BackColor = System.Drawing.Color.Red;
                throw new Exception("O valor do documento não pode estar zerado ou negativo!");
            }


            if (ddlLancamentoSimples.SelectedValue.Equals("1"))
            {
                txtValorPagar.Text = txtValor.Text;
            }

            Decimal vValorPagar;
            Decimal.TryParse(txtValorPagar.Text, out vValorPagar);

            if (vValorPagar <= 0)
            {
                txtValorPagar.BackColor = System.Drawing.Color.Red;
                throw new Exception("O valor a pagar não pode estar zerado ou negativo!");
            }

            if (chkBaixa_Automatica.Checked)
            {
                txtPagamento.BackColor = System.Drawing.Color.White;
                txtValor_Pago.BackColor = System.Drawing.Color.White;
                if (txtPagamento.Text.Equals(""))
                {
                    txtPagamento.BackColor = System.Drawing.Color.Red;
                    throw new Exception("O campo data de pagamento não foi preenchido!");
                }
                if (ddlLancamentoSimples.SelectedValue.Equals("1"))
                {
                    txtValor_Pago.Text = txtValor.Text;
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
                return true;
            else
                return false;
        }

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array

            String[] campos = { "txtCodigo_Centro_Custo",
                                    "txtFornecedor",
                                    "txtValor",
                                    "txtemissao.Text",
                                    "txtentrada",
                                    "txtVencimento"
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
                if (campo.ID.Equals("txtSerie"))
                {
                    return true;
                }
                if (campo.ID.Equals("txtFornecedor"))
                {
                    imgBtnFornecedor.Visible = false;
                    return true;
                }
                if (campo.ID.Equals("ddlLancamentoSimples"))
                {
                    return true;
                }


            }
            if (ddlLancamentoSimples.SelectedValue.Equals("1"))
            {

                if (campo.ID.Equals("txtAcrescimo"))
                {
                    return true;
                }
                if (campo.ID.Equals("txtDesconto"))
                {
                    return true;
                }
            }

            String[] campos = { "DllStatus",
                                    "txtusuario",
                                    "txtValorPagar",
                                    "txtFornecedor",
                                    "txtContabil_Eventos",
                                    "txtContabil_Eventos_Descricao"
                                     };

            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("ContasPagarDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            if (!DllStatus.SelectedItem.Text.Equals("CONCLUIDO") && !DllStatus.SelectedItem.Text.Equals("CANCELADO"))
            {
                lblError.Text = "";
                status = "editar";
                editar(pnBtn);
                habilitar(true);
                User usr = (User)Session["User"];
                txtusuario.Text = usr.getUsuario();
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
            Response.Redirect("ContasPagar.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            if (DllStatus.SelectedItem.Text.Equals("CONCLUIDO"))
            {
                Label1.Text = "Confirma Estorno?";
                modalPnConfirma.Show();
            }
            else if (DllStatus.SelectedItem.Text.Equals("ABERTO"))
            {

                Label1.Text = "Confirma Cancelamento?";
                modalPnConfirma.Show();
            }
            else
            {
                lblError.Text = "Status não Permite alterações";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!DllStatus.SelectedItem.Text.Equals("CONCLUIDO") && !DllStatus.SelectedItem.Text.Equals("CANCELADO"))
                {
                    if (validaCamposObrigatorios())
                    {

                        conta_a_pagarDAO obj = (conta_a_pagarDAO)Session["objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

                        carregarDadosObj(obj);
                        if (chkBaixa_Automatica.Checked)
                            obj.status = "2";

                        obj.salvar(status.Equals("incluir")); // se for incluir true se não falso;

                        txtDocumento.Text = obj.Documento;
                        lblError.Text = "Salvo com Sucesso";
                        lblError.ForeColor = System.Drawing.Color.Blue;

                        Session.Remove("objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                        Session.Add("objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
                        carregarDados(obj);


                        if (status.Equals("editar"))
                        {
                            if (obj.Grupo_pagamento.Count > 0 && obj.VlrAlterado)
                            {
                                Label5.Text = "Gostaria de replicar as alterações " + (obj.VencAlterado ? " de Vencimento " : " de Valores ") + " para os titulos relacionados  <br> que ainda estão abertos e com vencimento maior?";
                                modalConfirmaAlteracaoGrupo.Show();
                            }
                        }

                        visualizar(pnBtn);
                        habilitar(false);
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

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ContasPagar.aspx");//colocar endereco pagina de pesquisa
            Session.Remove("objcontaPagar");

        }

        private void carregarDados(conta_a_pagarDAO obj)
        {
            txtDocumento.Text = obj.Documento.ToString().Trim();
            txtFornecedor.Text = obj.Fornecedor.ToString();
            txtSerie.Text = obj.serie.ToString();
            txtCodigo_Centro_Custo.Text = obj.Codigo_Centro_Custo.ToString();
            txtDescCentroCusto.Text = obj.DescCentroCusto;
            txtValor.Text = string.Format("{0:0,0.00}", obj.Valor);
            txtDesconto.Text = string.Format("{0:0,0.00}", obj.Desconto);
            txtAcrescimo.Text = obj.acrescimo.ToString("N2");
            txtValorPagar.Text = ((obj.Valor - obj.Desconto) + obj.acrescimo).ToString("N2");
            txtobs.Text = obj.obs.ToString();
            txtemissao.Text = obj.emissaoBr();
            txtPagamento.Text = obj.PagamentoBr();
            txtTipo_Pagamento.Text = obj.Tipo_Pagamento.ToString();
            txtDuplicata.Text = obj.Duplicata ? "SIM" : "NAO";
            txtNumero_cheque.Text = obj.Numero_cheque.ToString();
            txtValor_Pago.Text = string.Format("{0:0,0.00}", obj.Valor_Pago);
            txtVencimento.Text = obj.VencimentoBr();
            txtid_cc.Text = obj.id_cc.ToString();
            txtBanco.Text = obj.banco.ToString();
            txtAgencia.Text = obj.agencia.ToString();
            txtConta.Text = obj.conta.ToString().Trim();
            chkBaixa_Automatica.Checked = obj.Baixa_Automatica;
            txtusuario.Text = obj.usuario.ToString();
            DllStatus.SelectedValue = obj.status;
            chkConferido.Checked = obj.conferido;
            txtCodBarras.Text = obj.cod_barras;
            if (obj.origemCodigoContabilEvento)
            {
                txtContabil_Eventos.Text = obj.eventoContabil.ToString();
                txtContabil_Eventos_Descricao.Text = obj.descEventoContabil;
            }

            if (obj.qtdeParcelas > 1)
            {
                lblparcelasCondicoes.Text = obj.qtdeParcelas.ToString() + " parcelas";


                if (obj.tipoParcela.Equals("Dia"))
                    lblparcelasCondicoes.Text += " a cada:" + obj.qtdeDias.ToString() + " dias";
                else
                    lblparcelasCondicoes.Text += " por " + obj.tipoParcela;


                if (obj.forcaDiaUtilVencimento)
                    lblparcelasCondicoes.Text += ", sempre em um dia útil;";
            }

            txtentrada.Text = obj.entradaBr();
            txtParcial.Text = string.Format("{0:0,0.00}", obj.Parcial);
            ddlLancamentoSimples.SelectedValue = (obj.lancamento_simples ? "1" : "0");
            if (status.Equals("visualizar") && DllStatus.SelectedValue.Equals("1"))
            {
                BtnBaixa.Visible = true;
                imgBtnBaixar.Visible = true;
            }
            else
            {
                BtnBaixa.Visible = false;

                imgBtnBaixar.Visible = false;
            }

            if (obj.Grupo_pagamento.Count > 0)
            {
                divTitulosDoMesmoGrupo.Visible = true;
                gridTitulosGrupo.DataSource = obj.Grupo_pagamento;
                gridTitulosGrupo.DataBind();

                foreach (GridViewRow item in gridTitulosGrupo.Rows)
                {
                    HyperLink link = (HyperLink)item.Cells[0].Controls[0];
                    if (link.Text.Equals(txtDocumento.Text))
                    {
                        item.RowState = DataControlRowState.Selected;
                        break;
                    }
                }

            }
            else
            {
                divTitulosDoMesmoGrupo.Visible = false;
            }
            User usr = (User)Session["User"];
            //Centro de custo 
            String sql = " Select nc.* , c.descricao_centro_custo from NF_CentroCusto as nc " +
                         " inner join centro_custo as c on nc.codigo_centro_custo = c.codigo_centro_custo " +
                         " where nc.filial ='" + usr.getFilial() + "'" +
                         "   and nc.codigo='";
            try
            {
                if (obj.codigo_nf != null)
                {
                    sql += obj.codigo_nf + "'";
                }
            }
            catch
            {
                sql += obj.Documento + "'";
            }

            sql +=  "   and nc.tipo_nf=" + 2 +
                         "   and nc.serie =" + obj.serie +
                         "   and nc.cliente_fornecedor ='" + obj.Fornecedor + "'" +

                         " order by nc.codigo_centro_custo ";

            SqlDataReader rs = null;
            List<Nf_CentroCustoDAO> lsCentroCusto = new List<Nf_CentroCustoDAO>();
            try
            {
                rs = Conexao.consulta(sql, usr, false);
                while (rs.Read())
                {
                    Nf_CentroCustoDAO cItem = new Nf_CentroCustoDAO
                    {
                        Filial = rs["Filial"].ToString(),
                        Codigo = rs["codigo"].ToString(),
                        Cliente_fornecedor = rs["cliente_fornecedor"].ToString(),
                        Tipo_nf = Funcoes.intTry(rs["tipo_nf"].ToString()),
                        serie = Funcoes.intTry(rs["serie"].ToString()),
                        Data = Funcoes.dtTry(rs["data"].ToString()),
                        Codigo_centro_custo = rs["codigo_centro_custo"].ToString(),
                        Descricao_centro_custo = rs["descricao_centro_custo"].ToString(),
                        porc = Funcoes.decTry(rs["porc"].ToString())
                    };

                    cItem.Valor = Decimal.Round(((obj.Valor * cItem.porc) / 100),2);
                    lsCentroCusto.Add(cItem);
                }
            }
            catch (Exception)
            {

                throw;
            }

            gridCentroCusto.DataSource = lsCentroCusto;
            gridCentroCusto.DataBind();

        }



        private void carregarDadosObj(conta_a_pagarDAO obj)
        {
            obj.Documento = txtDocumento.Text;
            obj.Fornecedor = txtFornecedor.Text;
            obj.serie = Funcoes.intTry(txtSerie.Text);
            obj.Codigo_Centro_Custo = txtCodigo_Centro_Custo.Text;
            obj.Valor = Decimal.Parse(txtValor.Text);
            obj.Desconto = Funcoes.decTry(txtDesconto.Text);

            obj.acrescimo = Funcoes.decTry(txtAcrescimo.Text);

            obj.obs = txtobs.Text;
            obj.emissao = DateTime.Parse(txtemissao.Text);
            obj.Pagamento = (txtPagamento.Text.Equals("") ? new DateTime() : DateTime.Parse(txtPagamento.Text));
            obj.Tipo_Pagamento = txtTipo_Pagamento.Text;
            obj.Duplicata = txtDuplicata.Text.Equals("SIM");//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            obj.Numero_cheque = txtNumero_cheque.Text;
            obj.Valor_Pago = Decimal.Parse((txtValor_Pago.Text.Equals("") ? "0" : txtValor_Pago.Text));
            obj.Vencimento = DateTime.Parse(txtVencimento.Text);
            obj.id_cc = txtid_cc.Text;
            obj.Baixa_Automatica = chkBaixa_Automatica.Checked;
            obj.usuario = txtusuario.Text;
            obj.status = DllStatus.SelectedValue;
            obj.banco = txtBanco.Text;
            obj.conferido = chkConferido.Checked;
            obj.cod_barras = txtCodBarras.Text;

            if (status.Equals("incluir"))
            {
                obj.qtdeParcelas = Funcoes.intTry(txtQtdeParcelas.Text);
                obj.tipoParcela = ddlTipoParcelas.Text;
                obj.qtdeDias = Funcoes.intTry(txtQtdeDias.Text);
                obj.forcaDiaUtilVencimento = chkConsiderarFimDeSemana.Checked;
            }


            obj.entrada = DateTime.Parse((txtentrada.Text.Equals("") ? "0" : txtentrada.Text));
            obj.Parcial = Decimal.Parse((txtParcial.Text.Equals("") ? "0" : txtParcial.Text));
            obj.lancamento_simples = ddlLancamentoSimples.SelectedValue.Equals("1");
            //Popular ID_CC de modo automático qdo for conta caixa e lcto simplificado.
            if (obj.lancamento_simples && obj.id_cc.Equals("")){
                User usr = (User)Session["User"];
                conta_correnteDAO contaCaixa = new conta_correnteDAO(usr);
                obj.id_cc = contaCaixa.ContaCaixaID(usr);
            }
            //Inclusão do código de eventos contabil.
            try
            {
                obj.eventoContabil = int.Parse(txtContabil_Eventos.Text);
            }
            catch
            {
                if (obj.origemCodigoContabilEvento)
                   obj.eventoContabil = 0;
            }

        }





        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                conta_a_pagarDAO obj = (conta_a_pagarDAO)Session["objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                if (DllStatus.SelectedItem.Text.Equals("CONCLUIDO"))
                {
                    obj.estornar();
                    modalPnConfirma.Hide();
                    lblError.Text = "Registro Estornado com sucesso";
                    Session.Remove("objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
                    carregarDados(obj);
                    carregabtn(pnBtn);
                }
                else if (DllStatus.SelectedItem.Text.Equals("ABERTO"))
                {
                    obj.excluir();
                    modalPnConfirma.Hide();
                    lblError.Text = "Registro Cancelado com sucesso";
                    limparCampos();
                    pesquisar(pnBtn);
                    if (obj.Grupo_pagamento.Count > 0)
                    {
                        Label5.Text = "Gostaria de Cancelar os outros titulos Relacionados?";
                        modalConfirmaAlteracaoGrupo.Show();
                    }
                    else
                    {
                        Session.Remove("objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));

                    }

                }


                habilitar(false);

            }
            catch (Exception err)
            {
                lblError.Text = "Não foi possivel Efetuar a Operação Error:" + err.Message;
            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            modalPnConfirma.Hide();

            habilitar(false);

        }

        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {
            String camporecebe = (String)Session["camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            TextBox txt = (TextBox)cabecalho.FindControl(camporecebe);
            if (txt == null)
                txt = (TextBox)conteudo.FindControl(camporecebe);



            modalPnFundo.Hide();
            if (camporecebe.Equals("txtCodigo_Centro_Custo"))
            {
                if (!txtCodigo_Centro_Custo.Text.Equals(""))
                {

                }
            }
            else if (camporecebe.Equals("txtid_cc"))
            {
                if (!txtid_cc.Text.Equals(""))
                {

                }
            }
        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalPnFundo.Hide();
        }

        protected void txtCodigo_Centro_Custo_TextChanged(object sender, EventArgs e)
        {
            if (!txtCodigo_Centro_Custo.Text.Equals(""))
            {
                conta_a_pagarDAO obj = (conta_a_pagarDAO)Session["objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                obj.Codigo_Centro_Custo = txtCodigo_Centro_Custo.Text;
                txtDescCentroCusto.Text = obj.DescCentroCusto;

                Session.Remove("objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);

            }
        }
        protected void exibeLista()
        {

            lblErroPesquisa.Text = "";
            User usr = (User)Session["User"];
            String or = (String)Session["camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            String sqlLista = "";


            switch (or)
            {
                case "txtFornecedor":
                    sqlLista = "Select fornecedor,Razao_social, ISnULL(CNPJ, '') AS CNPJ from fornecedor where (fornecedor like '%" + TxtPesquisaLista.Text + "%' or razao_social like '%" + TxtPesquisaLista.Text + "%')";
                    if (status.ToUpper().Equals("INCLUIR"))
                    {
                        sqlLista += " AND ISNULL(Inativo, 0) = 0";
                    }
                    lbllista.Text = "Fornecedores";

                    break;
                case "TxtTipo_Pagamento":

                    sqlLista = "select tipo_pagamento from tipo_pagamento where tipo_pagamento like '%" + TxtPesquisaLista.Text + "%'";
                    lbllista.Text = "Pagamentos";
                    break;
                case "txtid_cc":

                    sqlLista = "select id_cc  from conta_corrente where id_cc like '%" + TxtPesquisaLista.Text + "%'";
                    lbllista.Text = "Conta Corrente";
                    break;

                case "txtCodigo_Centro_Custo":

                    sqlLista = "select codigo_centro_custo, descricao_centro_Custo from centro_custo where codigo_centro_custo like '%" + TxtPesquisaLista.Text + "%' or descricao_centro_Custo like '%" + TxtPesquisaLista.Text + "%'";
                    lbllista.Text = "Centro de Custo";
                    break;

                case "txtContabil_Eventos":

                    sqlLista = "select Codigo, Descricao from Contabil_Eventos where codigo like '%" + TxtPesquisaLista.Text + "%' or descricao like '%" + TxtPesquisaLista.Text + "%'";
                    lbllista.Text = "Evento Contábil";
                    break;

            }
            GridLista.DataSource = Conexao.GetTable(sqlLista, usr, false);
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

            switch (btn.ID)
            {
                case "imgBtnFornecedor":
                    Session.Add("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "txtFornecedor");

                    break;
                case "imgBtnTxtTipo_Pagamento":
                    Session.Add("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "TxtTipo_Pagamento");
                    break;
                case "imgBtntxtId_cc":
                    Session.Add("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "txtid_cc");
                    break;

                case "imgBtnTxtCentroCusto":

                    Session.Add("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "txtCodigo_Centro_Custo");
                    break;

                case "imgbtnIDContabilEventos":

                    Session.Add("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "txtContabil_Eventos");
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
                        return item.Cells[campo].Text;
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
                conta_a_pagarDAO obj = (conta_a_pagarDAO)Session["objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                String listaAtual = (String)Session["camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                Session.Remove("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));

                switch (listaAtual)
                {
                    case "txtFornecedor":
                        txtFornecedor.Text = ListaSelecionada(1);
                        String strCentroCusto = Conexao.retornaUmValor("Select centro_custo from fornecedor where ltrim(rtrim(fornecedor))='" + txtFornecedor.Text.Trim() + "'", null);
                        if (!strCentroCusto.Equals(""))
                        {
                            txtCodigo_Centro_Custo.Text = strCentroCusto;
                            obj.Codigo_Centro_Custo = txtCodigo_Centro_Custo.Text;
                            txtDescCentroCusto.Text = obj.DescCentroCusto;
                        }

                        ////Caso o fornecedor tenha uma conta contábil declarada, o sistema vai utilizar esta informação para inserir nos lançamentos contábeis e não permite o evento contábil, caso contrário
                        ////o sistema libera a opção para inserção de um evento contábil.
                        //String strFornecedorContaContabil = Conexao.retornaUmValor("Select rtrim(ltrim(isnull(conta_contabil_credito, ''))) from fornecedor where ltrim(rtrim(fornecedor))='" + txtFornecedor.Text.Trim() + "'", null);
                        //if (!strFornecedorContaContabil.Equals(""))
                        //{
                        //    obj.origemCodigoContabilEvento = false;
                        //    obj.eventoContabil = int.Parse(strFornecedorContaContabil);
                        //    txtContabil_Eventos_Descricao.Text = "CONTA CONTABIL DO FORNECEDOR";
                        //    imgbtnIDContabilEventos.Enabled = false;
                        //}
                        //else
                        //{
                            imgbtnIDContabilEventos.Enabled = true;
                        //}


                        break;
                    case "TxtTipo_Pagamento":
                        txtTipo_Pagamento.Text = ListaSelecionada(1);
                        break;
                    case "txtid_cc":

                        txtid_cc.Text = ListaSelecionada(1);
                        obj.id_cc = txtid_cc.Text;
                        txtBanco.Text = obj.banco;
                        txtAgencia.Text = obj.agencia;

                        txtConta.Text = obj.conta;

                        break;

                    case "txtCodigo_Centro_Custo":
                        txtCodigo_Centro_Custo.Text = ListaSelecionada(1);

                        obj.Codigo_Centro_Custo = txtCodigo_Centro_Custo.Text;
                        txtDescCentroCusto.Text = obj.DescCentroCusto;

                        break;
                    case "txtContabil_Eventos":
                        txtContabil_Eventos.Text = ListaSelecionada(1);
                        //obj.eventoContabil = int.Parse(txtContabil_Eventos.Text);
                        //obj.descEventoContabil = ListaSelecionada(2);
                        txtContabil_Eventos_Descricao.Text = obj.descEventoContabil;
                        break;


                }
                Session.Remove("objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);

                modalPnFundo.Hide();
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



        protected void txt_TextChanged(object sender, EventArgs e)
        {
            try
            {

                lblError.Text = "";
                ((TextBox)sender).BackColor = System.Drawing.Color.White;

                conta_a_pagarDAO recb = (conta_a_pagarDAO)Session["objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

                switch (((TextBox)sender).ID)
                {
                    case "txtDesconto":
                        recb.Desconto = Funcoes.decTry(txtDesconto.Text);

                        txtDesconto.Text = recb.Desconto.ToString("N2");
                        txtAcrescimo.Focus();
                        break;
                    case "txtAcrescimo":
                        //recb.acrescimo = Decimal.Parse(txtAcrescimo.Text);

                        recb.acrescimo = Funcoes.decTry(txtAcrescimo.Text);

                        txtAcrescimo.Text = recb.acrescimo.ToString("N2");
                        txtPagamento.Focus();
                        break;
                    case "txtValor":
                        //recb.Valor = Decimal.Parse(txtValor.Text);

                        recb.Valor = Funcoes.decTry(txtValor.Text);
                        txtValor.Text = recb.Valor.ToString("N2");
                        txtDesconto.Focus();
                        break;
                }

                txtValorPagar.Text = ((recb.Valor - recb.Desconto) + recb.acrescimo).ToString("N2");
                if (ddlLancamentoSimples.SelectedValue.Equals("1"))
                {
                    txtValor_Pago.Text = txtValorPagar.Text;
                }


                Session.Remove("objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), recb);
            }
            catch (Exception)
            {
                lblError.Text = "Valor Inválido";
                lblError.ForeColor = System.Drawing.Color.Red;
                ((TextBox)sender).BackColor = System.Drawing.Color.Red;

            }


        }

        protected void txtid_cc_TextChanged(object sender, EventArgs e)
        {
            if (!txtid_cc.Text.Equals(""))
            {
                conta_a_pagarDAO obj = (conta_a_pagarDAO)Session["objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                obj.id_cc = txtid_cc.Text;
                txtBanco.Text = obj.banco;
                txtAgencia.Text = obj.agencia;

                txtConta.Text = obj.conta;

                Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
            }
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
                txtValor_Pago.Text = txtValorPagar.Text;
                //txtValor_Pago.BackColor = System.Drawing.Color.Red;
                //throw new Exception("O Campo Valor Pago não foi preenchido!");

            }

            if (!ddlLancamentoSimples.SelectedValue.Equals("1"))
            {
                if (txtid_cc.Text.Equals(""))
                {
                    txtid_cc.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Não foi informado uma Conta para baixa!");
                }
            }

            conta_a_pagarDAO obj = (conta_a_pagarDAO)Session["objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            if (Funcoes.valorParametro("EVENTOS_CONTABEIS", null).ToUpper().Equals("TRUE"))
            {
                if (obj.contabil_evento.conta_contabil == null || obj.contabil_evento.conta_contabil.Equals(""))
                {
                    txtContabil_Eventos.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Não foi informado EVENTO CONTÁBIL.");
                }
            }



            carregarDadosObj(obj);
            obj.status = "2";
            if (obj.ValorPagar > obj.Valor_Pago)
            {
                obj.Desconto += (obj.ValorPagar - obj.Valor_Pago);
            }
            else if (obj.ValorPagar < obj.Valor_Pago)
            {
                obj.acrescimo += (obj.Valor_Pago - obj.ValorPagar);
            }

            obj.salvar(false);
            Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
            carregarDados(obj);
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
        protected void ddlLancamentoSimples_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (status.Equals("incluir"))
            {
                if (ddlLancamentoSimples.SelectedValue.Equals("1"))
                {
                    User usr = (User)Session["user"];
                    txtFornecedor.Text = usr.getFilial();
                    txtemissao.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtVencimento.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtPagamento.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtCodigo_Centro_Custo.Text = Funcoes.valorParametro("CONT_PAGAR_CC_CAIXA", usr);
                    txtTipo_Pagamento.Text = "AVISTA";
                    txtValor.Focus();


                    SqlDataReader rsCC = null;
                    try
                    {
                        rsCC = Conexao.consulta("Select top 1 centro_custo.descricao_centro_custo," +
                                                                   " Conta_Corrente.id_cc," +
                                                                    " Conta_Corrente.Banco," +
                                                                    " Conta_Corrente.Agencia," +
                                                                    " Conta_Corrente.Conta" +
                                                                " from centro_custo left join Conta_Corrente on Centro_Custo.id_cc = Conta_Corrente.id_cc" +
                                                                " where centro_custo.Codigo_centro_custo ='" + txtCodigo_Centro_Custo.Text + "'", null, false);

                        if (rsCC.Read())
                        {
                            txtDescCentroCusto.Text = rsCC["descricao_centro_custo"].ToString();
                            txtid_cc.Text = rsCC["id_cc"].ToString();
                            txtBanco.Text = rsCC["Banco"].ToString();
                            txtAgencia.Text = rsCC["Agencia"].ToString();
                            txtConta.Text = rsCC["Conta"].ToString();
                        }

                    }
                    catch (Exception err)
                    {

                        lblError.Text = err.Message;
                    }
                    finally
                    {
                        if (rsCC != null)
                            rsCC.Close();
                    }
                    chkBaixa_Automatica.Checked = true;
                    chkConferido.Checked = true;
                    ocultaSimplificado();
                }
                else
                {

                    txtDesconto.Enabled = true;
                    txtAcrescimo.Enabled = true;
                    ocultaSimplificado();
                }
            }
        }

        private void ocultaSimplificado()
        {
            bool visible = true;
            if (ddlLancamentoSimples.SelectedValue.Equals("1"))
            {
                visible = false;
            }


            divDesconto.Visible = visible;
            divAcrescimo.Visible = visible;
            divValorPagar.Visible = visible;
            divValorPago.Visible = visible;
            divParcial.Visible = visible;
            divDuplicata.Visible = visible;
            divIdConta.Visible = visible;
            divBanco.Visible = visible;
            divAgencia.Visible = visible;
            divConta.Visible = visible;
            divNumeroCheque.Visible = visible;

        }

        protected void imgBtnConfirmaGrupo_Click(object sender, EventArgs e)
        {
            try
            {
                conta_a_pagarDAO cta = (conta_a_pagarDAO)Session["objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

                DateTime dtVenc = Funcoes.dtTry(txtVencimento.Text);
                Decimal vlr = Funcoes.decTry(txtValor.Text);
                Decimal acrescimo = Funcoes.decTry(txtAcrescimo.Text);
                Decimal desconto = Funcoes.decTry(txtDesconto.Text);
                User usr = (User)Session["User"];

                int nItem = Funcoes.intTry(cta.Documento.Substring(cta.Documento.IndexOf("-") + 1));
                foreach (Contas_a_pagar_grupoDAO item in cta.Grupo_pagamento)
                {

                    String nDoc = item.Documento;
                    int nItem2 = Funcoes.intTry(nDoc.Substring(nDoc.IndexOf("-") + 1));
                    if (nItem2 > nItem && item.Status.Equals("ABERTO"))
                    {
                        if (Label5.Text.Contains("Cancelar"))
                        {
                            conta_a_pagarDAO conta = new conta_a_pagarDAO(nDoc, cta.Fornecedor, usr);
                            conta.excluir();
                        }
                        else
                        {
                            DateTime nVenc = new DateTime();
                            if (cta.VencAlterado)
                            {
                                if (cta.tipoParcela.Equals("Mês"))
                                {
                                    nVenc = dtVenc.AddMonths(nItem2 - nItem);
                                }
                                else
                                {
                                    nVenc = dtVenc.AddDays((cta.qtdeDias * (nItem2 - nItem)));
                                }

                                if (cta.forcaDiaUtilVencimento)
                                    nVenc = Funcoes.DiaUtil(nVenc);
                            }

                            conta_a_pagarDAO.atualizarDoc(usr.getFilial()
                                                         , nDoc
                                                         , cta.Fornecedor
                                                         , vlr
                                                         , acrescimo
                                                         , desconto
                                                         , nVenc
                                                         , cta.Codigo_Centro_Custo
                                                         , cta.Tipo_Pagamento
                                                         );
                        }

                    }
                }

                if (!Label5.Text.Contains("Cancelar"))
                {
                    cta.VlrAlterado = false;
                    cta.carregarContasPagarGrupo();
                    Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), cta);
                    carregarDados(cta);
                }
                else
                {
                    lblError.Text = "Titulos Cancelados";
                    habilitar(false);
                }
            }
            catch (Exception err)
            {
                lblError.Text = err.Message;
            }


        }

        protected void imgBtnCancelaGrupo_Click(object obj, EventArgs e)
        {
            modalConfirmaAlteracaoGrupo.Hide();
            conta_a_pagarDAO cta = (conta_a_pagarDAO)Session["objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            cta.VlrAlterado = false;
            Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), cta);
            carregarDados();
            habilitar(false);
        }

        private void carregarDados()
        {
            conta_a_pagarDAO cta = (conta_a_pagarDAO)Session["objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            carregarDados(cta);
        }

        protected void ddlTipoParcelas_SelectedIndexChanged(object sender, EventArgs e)
        {
            divQtdeDias.Visible = ddlTipoParcelas.Text.Equals("Dia");
            if (txtQtdeDias.Text.Equals(""))
                txtQtdeDias.Text = "30";
        }

        protected void ImgbtnConfirmaCodBarra_Click(object sender, ImageClickEventArgs e)
        {
            String numeroCodBarra = txtlinhaDigitavel.Text.Replace(" ", "").Replace(".", "").Trim();
            int fatorVenc = 0;
            if (numeroCodBarra.Length == 44)
            {
                fatorVenc = Funcoes.intTry(numeroCodBarra.Substring(5, 4));
                
                txtValor.Text = numeroCodBarra.Substring(9, 8) + "," + numeroCodBarra.Substring(17, 2);


            }
            else if (numeroCodBarra.Length == 47)
            {
                fatorVenc = Funcoes.intTry(numeroCodBarra.Substring(33, 4));
                
                txtValor.Text = numeroCodBarra.Substring(37, 8) + "," + numeroCodBarra.Substring(46, 2);
            }
            else if (numeroCodBarra.Length == 48)
            {
                if (numeroCodBarra.Substring(0, 1).Equals("8"))
                {
                    String strVlr = numeroCodBarra.Substring(4, 7) 
                         + numeroCodBarra.Substring(12, 2)
                         + ","+numeroCodBarra.Substring(14, 2)
                         ;
                    txtValor.Text = strVlr;
                    //fatorVenc = Funcoes.intTry(numeroCodBarra.Substring(20, 4));
                }
               
            }
            if(fatorVenc>0)
                txtVencimento.Text = Funcoes.fatorVencimento(fatorVenc).ToString("dd/MM/yyyy");


            txtValor.Text = Funcoes.decTry(txtValor.Text).ToString();
            txtValorPagar.Text = txtValor.Text;
            modalCodBarras.Hide();
        }

        protected void ImgBtnCancelarCodBarra_Click(object sender, ImageClickEventArgs e)
        {
            modalCodBarras.Hide();
        }

        protected void ImgBtnIncluirPorCodBarra_Click(object sender, ImageClickEventArgs e)
        {
            modalCodBarras.Show();
            txtlinhaDigitavel.Focus();
        }

        protected void txtContabil_Eventos_TextChanged(object sender, EventArgs e)
        {
            if (!txtContabil_Eventos.Text.Equals(""))
            {
                conta_a_pagarDAO obj = (conta_a_pagarDAO)Session["objcontaPagar" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                obj.origemCodigoContabilEvento = true;
                obj.eventoContabil = int.Parse(txtContabil_Eventos.Text);
                txtContabil_Eventos_Descricao.Text = obj.descEventoContabil;

                Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
            }
        }

        protected void txtCodBarras_TextChanged(object sender, EventArgs e)
        {
            DateTime barraDataVencimento;
            Double barraValor = 0;
            int barraFatorData = 0;
            //if (Funcoes.isnumero(txtCodBarras.Text))
            //{
                if (txtCodBarras.Text.Trim().Length == 44) //Digitos quando a barra é lida a partir do leitor
                {
                    barraFatorData = int.Parse(txtCodBarras.Text.Trim().Substring(5, 4));
                    barraDataVencimento = (DateTime.Parse("1997-10-07").AddDays(barraFatorData));
                    barraValor = Double.Parse(txtCodBarras.Text.Trim().Substring(9, 10))/100;
                    txtVencimento.Text = barraDataVencimento.ToString("dd/MM/yyyy");
                    txtValor.Text = barraValor.ToString("n2");
                }
            //}
        }
    }
}