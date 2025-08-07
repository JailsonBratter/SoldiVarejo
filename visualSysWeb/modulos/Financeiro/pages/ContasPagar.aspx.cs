using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using visualSysWeb.modulos.Financeiro.code;
using System.Data.SqlClient;
using System.Collections;

namespace visualSysWeb.modulos.Financeiro.pages
{
    public partial class ContasPagar : visualSysWeb.code.PagePadrao     //inicio da classe 
    {
        static DataTable tb;
        static String sqlGrid = "select ltrim(rtrim(Documento)) as Documento,Fornecedor,convert(varchar,emissao,103) as Emissao,convert(varchar,vencimento,103)As Vencimento,convert(varchar,entrada,103)As Entrada,convert(varchar,pagamento,103)As Pagamento,((isnull(Valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) Valor,isnull(tipo_pagamento,'-')Tipo_Pagamento, Status = case Status when 1 then 'ABERTO' when 2 then 'CONCLUIDO' when 3 then 'CANCELADO' when 4 then 'LANCADO' END ,codigo_centro_custo,numero_cheque,CONFERIDO =CASE conferido WHEN 1 then 'CONFERIDO' ELSE null END ,P_SIMPLES =CASE ISNULL(lancamento_simples,0) WHEN 1 then 'SIM' ELSE 'NAO' END,id_cc from conta_a_pagar";//colocar os campos no select que ser?o apresentados na tela
        static String ultimaOrdem = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            ddlTipoPagamento.Items.Add("");
            SqlDataReader lista = Conexao.consulta("select tipo_pagamento from tipo_pagamento", null, false);
            while (lista.Read())
            {
                ddlTipoPagamento.Items.Add(lista[0].ToString());
            }
            if (lista != null)
                lista.Close();

            if (!IsPostBack)
            {
                ddlTipoPagamento.Text = "";
                ContasPagarFiltro pg = (ContasPagarFiltro)Session["contasPagarFiltro"];
                if (pg != null)
                {
                    txtDocumento.Text = pg.Documento;
                    txtFornecedor.Text = pg.fornecedor;
                    txtDe.Text = pg.de;
                    txtAte.Text = pg.ate;
                    DllTipoPesquisa.Text = pg.PesquisaPor;
                    DllStatus.Text = pg.Status;
                    ddlTipoPagamento.Text = pg.tipoPagamento;
                    chkConferido.Checked = pg.conferido;
                    txtCentroCusto.Text = pg.codigo_centro_custo;
                }
                pesquisar(true);

            }
            EnabledControls(filtrosPesq, true);
            pesquisar(pnBtn);
            camposnumericos();

            if (!IsPostBack)
            {
                txtAte.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
                txtAte.MaxLength = 10;
                txtDe.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
                txtDe.MaxLength = 10;
            }
        }


        private void camposnumericos()
        {

            //txtDocumento.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
            txtValor.Attributes.Add("OnKeyPress", "javascript:return formataDouble(this,event);");
        }


        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/Financeiro/pages/contasPagarDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
        }

        protected void pesquisar(bool limitar)
        {
            pesquisar(limitar, ultimaOrdem);
        }

        protected void pesquisar(bool limitar, String ordem)
        {


            try
            {
                User usr = (User)Session["User"];

                ContasPagarFiltro pg = new ContasPagarFiltro();

                pg.Documento = txtDocumento.Text;
                pg.fornecedor = txtFornecedor.Text;
                pg.de = txtDe.Text;
                pg.ate = txtAte.Text;
                pg.PesquisaPor = DllTipoPesquisa.Text;
                pg.Status = DllStatus.Text;
                pg.conferido = chkConferido.Checked;
                pg.codigo_centro_custo = txtCentroCusto.Text;

                Session.Remove("contasPagarFiltro");
                Session.Add("contasPagarFiltro", pg);



                String sql = " Filial='" + usr.getFilial() + "' ";
                if (!txtDocumento.Text.Equals("")) //colocar nome do campo de pesquisa
                {
                    if (!sql.Equals(""))
                    {
                        sql += " and ";
                    }
                    sql += " ltrim(rtrim(Documento)) like '" + txtDocumento.Text + "%'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
                    limitar = false;
                }
                if (!txtFornecedor.Text.Equals("")) //colocar nome do campo de pesquisa2
                {
                    if (!sql.Equals(""))
                    {
                        sql += " and ";
                    }
                    sql += " fornecedor = '" + txtFornecedor.Text + "'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
                    limitar = false;
                }
                if (!DllTipoPesquisa.SelectedValue.Equals(""))
                {
                    if (!txtDe.Text.Equals("")) //colocar nome do campo de pesquisa2
                    {
                        if (!sql.Equals(""))
                        {
                            sql += " and ";
                        }
                        sql += DllTipoPesquisa.SelectedValue + " between '" + DateTime.Parse(txtDe.Text).ToString("yyyy-MM-dd") + "' and '" + DateTime.Parse(txtAte.Text).ToString("yyyy-MM-dd") + "'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
                    }

                    //ordem += DllTipoPesquisa.SelectedValue + ", Valor";

                    limitar = false;
                }
                if (ddlTipoPagamento.SelectedIndex != 0) //colocar nome do campo de pesquisa2
                {
                    if (!sql.Equals(""))
                    {
                        sql += " and ";
                    }
                    sql += " tipo_pagamento  = '" + ddlTipoPagamento.Text + "'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 

                }
                if (DllStatus.SelectedIndex != 0) //colocar nome do campo de pesquisa2
                {
                    if (!sql.Equals(""))
                    {
                        sql += " and ";
                    }
                    sql += " Status = '" + DllStatus.SelectedValue + "'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 

                }
                if (!txtValor.Text.Trim().Equals(""))
                {
                    if (!sql.Equals(""))
                    {
                        sql += " and ";
                    }
                    sql += " (((isnull(Valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) = " + txtValor.Text.Replace(",", ".") +" or (Valor =" + txtValor.Text.Replace(",", ".")+")) " ;

                }

                if (!txtCentroCusto.Text.Trim().Equals(""))
                {
                    if (!sql.Equals(""))
                    {
                        sql += " and ";
                    }
                    sql += "codigo_centro_custo='" + txtCentroCusto.Text + "'";

                }

                if (!txtCheque.Text.Trim().Equals(""))
                {
                    if (!sql.Equals(""))
                    {
                        sql += " and ";
                    }
                    sql += " Numero_cheque='" + txtCheque.Text + "'";

                }

                if (chkConferido.Checked)
                {
                    if (!sql.Equals(""))
                    {
                        sql += " and ";
                    }
                    sql += " Conferido=1";

                }

                if (ddlSimples.SelectedIndex != 0)
                {
                    if (!sql.Equals(""))
                    {
                        sql += " and ";
                    }
                    if (ddlSimples.SelectedValue.Equals("1"))
                    {
                        sql += " lancamento_simples = 1";
                    }
                    else if (ddlSimples.SelectedValue.Equals("0"))
                    {
                        sql += " isnull(lancamento_simples,0) = 0";
                    }
                }



                if (ordem.Equals(""))
                {
                    pg.orderBy = " order by convert(varchar,emissao,102) ";
                }
                else
                {
                    if (ordem.Equals("Emissao"))
                    {
                        pg.orderBy = " order by convert(varchar,emissao,102) ";
                    }
                    else if (ordem.Equals("vencimento"))
                    {
                        pg.orderBy = " order by convert(varchar,vencimento,102) ";
                    }
                    else
                    {
                        pg.orderBy = " order By " + ordem;
                    }
                }

                if (ordem.Equals(ultimaOrdem))
                {
                    pg.orderBy += " Desc ";
                    ultimaOrdem = "";
                }
                else
                {
                    ultimaOrdem = ordem;
                }




                if (!sql.Equals(""))
                {

                    String totalItens = Conexao.retornaUmValor("select COUNT(documento) from Conta_a_pagar where " + sql, usr);
                    if (int.Parse(totalItens) > 100)
                    {
                        lblRegistros.Text = "100 titulos de " + totalItens + " Encontrados";

                    }
                    else
                    {
                        lblRegistros.Text = totalItens + " Titulos Encontrados";

                    }
                    tb = Conexao.GetTable(sqlGrid + " where " + sql + pg.orderBy, usr, true);
                }
                else
                {
                    tb = Conexao.GetTable(sqlGrid + pg.orderBy, usr, true);
                }
                gridPesquisa.DataSource = tb;
                gridPesquisa.DataBind();
                exibirBtnBaixa();
            }
            catch (Exception err)
            {
                lblPesquisaErro.Text = err.Message;
                lblPesquisaErro.ForeColor = System.Drawing.Color.Red;
            }
        }
        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            //String 
            pesquisar(true);
        }
        protected override void btnEditar_Click(object sender, EventArgs e) { }
        protected override void btnExcluir_Click(object sender, EventArgs e) { }
        protected override void btnConfirmar_Click(object sender, EventArgs e) { }
        protected override void btnCancelar_Click(object sender, EventArgs e) { }


        protected void gridPesquisa_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridPesquisa.DataSource = tb;
            gridPesquisa.PageIndex = e.NewPageIndex;
            gridPesquisa.DataBind();
        }
        protected override bool campoObrigatorio(Control campo)
        {
            return false;
        }

        protected override bool campoDesabilitado(Control campo)
        {
            return false;
        }

        protected void exibeLista()
        {
            lblErroPesquisa.Text = "";
            User usr = (User)Session["User"];
            String or = (String)Session["camporecebe"];
            String sqlLista = "";


            switch (or)
            {
                case "Fornecedor":
                    sqlLista = "select Fornecedor, razao_social from fornecedor where Fornecedor like '%" + TxtPesquisaLista.Text + "%' or razao_social like '%" + TxtPesquisaLista.Text + "%'"; ;
                    lbllista.Text = "Fornecedor";
                    break;
                case "CentroCusto":
                    sqlLista = "select [CODIGO]=Codigo_Centro_Custo, [CENTRO DE CUSTO]=descricao_centro_custo from centro_custo where codigo_centro_custo like '%" + TxtPesquisaLista.Text + "%' or descricao_centro_custo like '%" + TxtPesquisaLista.Text + "%'"; ;
                    lbllista.Text = "Centro de Custo";
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
            Session.Remove("camporecebe");

            switch (btn.ID)
            {
                case "imgBtnFornecedor":
                    Session.Add("camporecebe", "Fornecedor");
                    break;
                case "imgBtnCentroCusto":
                    Session.Add("camporecebe", "CentroCusto");
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
                String listaAtual = (String)Session["camporecebe"];
                Session.Remove("camporecebe");

                if (listaAtual.Equals("Fornecedor"))
                {

                    txtFornecedor.Text = ListaSelecionada(1);


                }
                if (listaAtual.Equals("CentroCusto"))
                {
                    txtCentroCusto.Text = ListaSelecionada(1);
                }


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



        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalPnFundo.Hide();
        }
        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }

        protected void chkSeleciona_CheckedChanged(object sender, EventArgs e)
        {

            foreach (GridViewRow item in gridPesquisa.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                if (chk != null)
                {
                    foreach (TableCell valor in item.Cells)
                    {
                        String str = valor.Text;

                    }

                    chk.Checked = (sender as CheckBox).Checked;
                }
            }
            exibirBtnBaixa();
        }

        protected void BtnBaixar_Click(object sender, EventArgs e)
        {
            lblErroGrid.Visible = false;
            lblGridtitulo.Visible = true;
            PnGrid.Visible = true;
            ArrayList arr = new ArrayList();
            ArrayList cab = new ArrayList();
            cab.Add("DOCUMENTO");
            cab.Add("FORNECEDOR");
            cab.Add("VALOR");
            cab.Add("VENCIMENTO");
            cab.Add("VALOR PAGO");
            cab.Add("DATA PAGAMENTO");
            cab.Add("id_cc");

            arr.Add(cab);
            bool preenche = false;
            User usr = (User)Session["User"];
            Conexao.preencherDDL1Branco(ddlContaCorrenteTodas, "Select id_cc from Conta_Corrente", "id_cc", "id_cc", usr);
            txtDtpgTodas.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblBanco.Text = "";

            try
            {

                for (int i = 0; gridPesquisa.Rows.Count > i; i++)
                {

                    CheckBox chk = (CheckBox)gridPesquisa.Rows[i].FindControl("chkSelecionaItem");
                    Button btn = (Button)sender;
                    if (chk != null)
                    {
                        if (chk.Checked)
                        {
                            String Status = gridPesquisa.DataKeys[i][5].ToString();
                            if (Status.Equals("ABERTO") && btn.ID.Equals("BtnBaixar"))
                            {
                                preenche = true;
                                lblGridtitulo.Text = "CONFIRMA BAIXA";
                            }
                            else if (Status.Equals("CONCLUIDO") && btn.ID.Equals("BtnCancelarBaixa"))
                            {
                                preenche = true;
                                lblGridtitulo.Text = "CONFIRMA ESTORNO DE BAIXA";
                            }

                            if (preenche)
                            {
                                ArrayList row = new ArrayList();

                                row.Add(gridPesquisa.DataKeys[i][0].ToString());
                                row.Add(gridPesquisa.DataKeys[i][1].ToString());
                                row.Add(gridPesquisa.DataKeys[i][2].ToString());
                                row.Add(gridPesquisa.DataKeys[i][4].ToString());
                                row.Add(gridPesquisa.DataKeys[i][2].ToString());
                                row.Add(gridPesquisa.DataKeys[i][5].ToString());
                                row.Add(gridPesquisa.DataKeys[i][6].ToString());
                                arr.Add(row);
                                preenche = false;
                            }

                        }
                    }
                }
            }
            catch (Exception err)
            {

                lblPesquisaErro.Text = "Não foi possivel Baixar os Documentos erro: " + err.Message;
            }

            GridBaixas.DataSource = Conexao.GetArryTable(arr);
            GridBaixas.DataBind();
            ModalGrid.Show();

        }



        protected void exibirBtnBaixa()
        {
            BtnBaixar.Visible = false;
            BtnCancelarBaixa.Visible = false;
            for (int i = 0; gridPesquisa.Rows.Count > i; i++)
            {

                CheckBox chk = (CheckBox)gridPesquisa.Rows[i].FindControl("chkSelecionaItem");

                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        if (gridPesquisa.DataKeys[i][5].ToString().Equals("ABERTO"))
                        {
                            BtnBaixar.Visible = true;

                        }
                        else if (gridPesquisa.DataKeys[i][5].ToString().Equals("CONCLUIDO"))
                        {
                            BtnCancelarBaixa.Visible = true;
                        }
                    }
                }
            }
        }
        protected void chkSelecionaItem_CheckedChanged(object sender, EventArgs e)
        {
            exibirBtnBaixa();
        }
        protected void btnConfirmaGrid_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lblErroGrid.Visible = false;
                lblGridtitulo.Visible = true;
                bool fecha = true;
                String strStatus = "";
                bool atualizar = false;
                if (lblGridtitulo.Text.Equals("CONFIRMA ESTORNO DE BAIXA"))
                {

                    strStatus = "1";
                    atualizar = true;
                }
                else if (lblGridtitulo.Text.Equals("CONFIRMA BAIXA"))
                {
                    strStatus = "2";
                    atualizar = true;
                }

              
                foreach (GridViewRow item in GridBaixas.Rows)
                {
                    TextBox txtValorPg = (TextBox)item.FindControl("txtValorPago");
                    Decimal vPg = 0;
                    Decimal.TryParse(txtValorPg.Text, out vPg);

                    txtValorPg.BackColor = System.Drawing.Color.White;
                    if (vPg <= 0)
                    {
                        txtValorPg.BackColor = System.Drawing.Color.Red;
                        txtValorPg.Focus();
                        throw new Exception("O documento:" + item.Cells[0].Text + " esta com o valor zero ou negativo;");
                    }

                    DropDownList ddlConta = (DropDownList)item.FindControl("ddlContaCorrente");
                    if (ddlConta.Text.Equals(""))
                    {
                        ddlConta.BackColor = System.Drawing.Color.Red;
                        ddlConta.Focus();      
                        throw new Exception("Não foi informado uma conta para baixa no titulo " + item.Cells[0].Text + "!");
                        
                    }

                    if (!strStatus.Equals("1"))
                    {
                        TextBox txtDtPg = (TextBox)item.FindControl("txtDataPago");
                        if (!IsDate(txtDtPg.Text))
                        {
                            txtDtPg.Focus();
                            throw new Exception("Data invalida do documento:" + item.Cells[0].Text);
                        }
                    }


                }
                    for (int i = 0; i < GridBaixas.Rows.Count; i++)
                    {

                        if (atualizar)
                        {
                            TextBox txtValorPg = (TextBox)GridBaixas.Rows[i].FindControl("txtValorPago");
                            TextBox txtDtPg = (TextBox)GridBaixas.Rows[i].FindControl("txtDataPago");

                            DropDownList ddlConta = (DropDownList)GridBaixas.Rows[i].FindControl("ddlContaCorrente");
                            //if (!ddlContaCorrenteTodas.Text.Equals(""))
                            //{
                            //    ddlConta.Text = ddlContaCorrenteTodas.Text;
                            //}

                            //if (!txtDtpgTodas.Text.Equals("") && IsDate(txtDtpgTodas.Text))
                            //{
                            //    txtDtPg.Text = txtDtpgTodas.Text;
                            //}


                            User usr = (User)Session["User"];
                            String documento = GridBaixas.DataKeys[i][0].ToString();
                            String fornecedor = GridBaixas.DataKeys[i][1].ToString();
                            Decimal valor = Decimal.Parse(GridBaixas.DataKeys[i][2].ToString());

                            conta_a_pagarDAO obj = new conta_a_pagarDAO(documento, fornecedor, usr);
                            if (strStatus.Equals("1"))
                            {
                                obj.estornar();
                                pesquisar(true);
                            }
                            else if (IsDate(txtDtPg.Text) && DateTime.Parse(txtDtPg.Text) >= obj.emissao)
                            {
                                obj.status = strStatus;
                                
                                 
                                Decimal.TryParse(txtValorPg.Text, out obj.Valor_Pago);

                              

                                if (obj.ValorPagar > obj.Valor_Pago)
                                {
                                    obj.Desconto += (obj.ValorPagar - obj.Valor_Pago);
                                }
                                else if (obj.ValorPagar < obj.Valor_Pago)
                                {
                                    obj.acrescimo += (obj.Valor_Pago - obj.ValorPagar);
                                }

                                obj.Pagamento = DateTime.Parse(txtDtPg.Text);
                                obj.id_cc = ddlConta.Text;
                                if (!txtObsBaixa.Text.Trim().Equals(""))
                                {
                                    obj.obs += "\n" + txtObsBaixa.Text;
                                }
                                obj.salvar(false);
                                pesquisar(true);

                            }
                            else
                            {
                                lblPesquisaErro.Text = "Data Invalida do Documento:" + obj.Documento + " A Data do pagamento não pode ser Menor que a Data de Emissão";
                                lblGridtitulo.Visible = false;
                                lblErroGrid.Visible = true;
                                fecha = false;
                                txtDtPg.Focus();
                                break;
                            }
                        }
                    
                }
                if (fecha)
                {
                    ModalGrid.Hide();
                }
                else
                {
                    ModalGrid.Show();
                }

            }
            catch (Exception err)
            {
                ModalGrid.Show();
                lblErroGrid.Text = "Erro : " + err.Message;
                lblGridtitulo.Visible = false;
                lblErroGrid.Visible = true;
                
            }

        }
        protected void btnCancelaGrid_Click(object sender, ImageClickEventArgs e)
        {
            PnGrid.Visible = false;
        }

        protected void GridBaixas_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlContaCorrente");
                User usr = (User)Session["User"];
                Conexao.preencherDDL1Branco(ddl, "Select id_cc from Conta_Corrente", "id_cc", "id_cc", usr);
                Label lblConta = (Label)e.Row.FindControl("lblConta");

                ddl.Text = lblConta.Text;
                TextBox txt = (TextBox)e.Row.FindControl("txtValorPago");
                txt.Text = e.Row.Cells[2].Text;

                TextBox txtdt = (TextBox)e.Row.FindControl("txtDataPago");
                txtdt.Text = DateTime.Now.ToString("dd/MM/yyyy");

            }
        }

        protected void txtDataDe_TextChanged(object sender, EventArgs e)
        {
            if (IsDate(txtDe.Text))
            {
                txtAte.Text = txtDe.Text;
            }
        }

        protected void gridPesquisa_Sorting(object sender, GridViewSortEventArgs e)
        {
            pesquisar(true, e.SortExpression);
        }

        protected void imgBtnLimpar_Click(object sender, ImageClickEventArgs e)
        {
            txtDocumento.Text = "";
            txtFornecedor.Text = "";
            txtValor.Text = "";
            txtDe.Text = "";
            txtAte.Text = "";
            ddlTipoPagamento.SelectedIndex = 0;
            DllTipoPesquisa.SelectedIndex = 0;
            DllStatus.SelectedIndex = 0;
            txtCentroCusto.Text = "";
            txtCheque.Text = "";
            chkConferido.Checked = false;
        }
        protected void ddlContaCorrenteTodas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ddlContaCorrenteTodas.Text.Equals(""))
            {
                foreach (GridViewRow rw in GridBaixas.Rows)
                {
                    DropDownList ddlConta = (DropDownList)rw.FindControl("ddlContaCorrente");
                    ddlConta.Text = ddlContaCorrenteTodas.Text;
                }
                User usr = (User)Session["User"];
                lblBanco.Text = Conexao.retornaUmValor("Select nome_banco from Banco inner join Conta_Corrente on banco.Numero_Banco= Conta_Corrente.Banco where Conta_Corrente.id_cc='" + ddlContaCorrenteTodas.Text + "'", usr);
            }
            ModalGrid.Show();
        }

        protected void txtDtpgTodas_TextChanged(object sender, EventArgs e)
        {
            if (!txtDtpgTodas.Text.Equals(""))
            {
                if (IsDate(txtDtpgTodas.Text))
                {
                    txtDtpgTodas.BackColor = System.Drawing.Color.White;

                    foreach (GridViewRow rw in GridBaixas.Rows)
                    {
                        TextBox txtDt = (TextBox)rw.FindControl("txtDataPago");
                        txtDt.Text = txtDtpgTodas.Text;
                    }
                }
                else
                {
                    txtDtpgTodas.BackColor = System.Drawing.Color.Red;

                }
            }
            ModalGrid.Show();
        }


    }
}