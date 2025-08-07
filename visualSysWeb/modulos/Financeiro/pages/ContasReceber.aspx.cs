using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using System.Data.SqlClient;
using visualSysWeb.modulos.Financeiro.code;
using System.Collections;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Financeiro.pages
{
    public partial class ContasReceber : visualSysWeb.code.PagePadrao
    {
        static DataTable tb;
        static String ultimaOrdem = "";
        protected void Page_Load(object sender, EventArgs e)
        {



            if (!IsPostBack)
            {
                Conexao.preencherDDL(ddlTipoCartao, "select id_cartao from cartao");
                if (Request["codigo_cliente"] != null)
                    txtcliente.Text = Request["codigo_cliente"].ToString();

                ContasReceberFiltro pg = (ContasReceberFiltro)Session["contasReceberFiltro"];
                if (pg != null)
                {
                    txtDocumento.Text = pg.documento;
                    txtcliente.Text = pg.codCliente;
                    txtNomeCliente.Text = pg.nomeCliente;
                    txtDe.Text = pg.de;
                    txtAte.Text = pg.ate;
                    DllTipoPesquisa.Text = pg.pesquisaPor;
                    DllStatus.Text = pg.status;
                    txtCentroCusto.Text = pg.centroCusto;
                    ddlTipoCartao.Text = pg.cartao;
                    txtCNPJ.Text = pg.cnpj;
                }

                pesquisar(true);
            }
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
            txtcliente.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
            //txtDocumento.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
            txtValor.Attributes.Add("OnKeyPress", "javascript:return formataDouble(this,event);");
        }


        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/Financeiro/pages/ContasReceberDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                ContasReceberFiltro pg = (ContasReceberFiltro)Session["contasReceberFiltro"];
                if (pg == null)
                {
                    pg = new ContasReceberFiltro();
                }
                pg.documento = txtDocumento.Text;
                pg.codCliente = txtcliente.Text;
                pg.nomeCliente = txtNomeCliente.Text;
                pg.de = txtDe.Text;
                pg.ate = txtAte.Text;
                pg.pesquisaPor = DllTipoPesquisa.Text;
                pg.status = DllStatus.Text;
                pg.centroCusto = txtCentroCusto.Text;
                pg.cartao = ddlTipoCartao.Text;
                pg.cnpj = txtCNPJ.Text;

                if (IsPostBack)
                {
                    pg.iniGrid = 0;
                    pg.fimGrid = 100;
                }

                Session.Remove("contasReceberFiltro");
                Session.Add("contasReceberFiltro", pg);

                String sql = " conta_a_receber.Filial='" + usr.getFilial() + "' ";
                if (!txtDocumento.Text.Equals("")) //colocar nome do campo de pesquisa
                {
                    sql = " ltrim(rtrim(Documento)) like '" + txtDocumento.Text + "%'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
                    limitar = false;
                }
                if (!txtcliente.Text.Equals("")) //colocar nome do campo de pesquisa2
                {
                    if (!sql.Equals(""))
                    {
                        sql += " and ";
                    }
                    sql += " cliente.Codigo_cliente = '" + txtcliente.Text + "'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
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
                        limitar = false;
                    }

                    // pg.orderBy += DllTipoPesquisa.SelectedValue + ", Valor";


                }

                if (txtCNPJ.Text.Trim().Length > 0)
                {
                    if (!sql.Equals(""))
                    {
                        sql += " and ";
                    }
                    sql += " replace(replace(replace(cliente.cnpj,'.',''),'-',''),'/','') = '" + txtCNPJ.Text.Replace(".", "").Replace("-", "").Replace("/", "") + "'";
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
                    sql += "( ((isnull(Valor,0)-isnull(desconto,0))+isnull(acrescimo,0)) = " + txtValor.Text.Replace(",", ".") + " OR (Valor=" + txtValor.Text.Replace(",", ".") + "))";

                }
                if (!txtCentroCusto.Text.Trim().Equals(""))
                {
                    if (!sql.Equals(""))
                    {
                        sql += " and ";
                    }
                    sql += "codigo_centro_custo='" + txtCentroCusto.Text + "'";

                }

                if (!ddlTipoCartao.Text.Equals(""))
                {
                    if (!sql.Equals(""))
                    {
                        sql += " and ";
                    }

                    sql += " cartao.id_cartao ='" + ddlTipoCartao.Text + "'";
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
                    else if (ordem.Equals("Codigo_cliente"))
                    {
                        pg.orderBy = " order by conta_a_receber.codigo_cliente ";
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
                String sqlGrid = "select Documento" +
                                        ",conta_a_receber.codigo_cliente" +
                                        ",Nome_cliente" +
                                        ", convert(varchar,entrada,103)As Entrada" +
                                        ",convert(varchar,pagamento,103)As Pagamento  " +
                                        ",((isnull(Valor,0)-isnull(desconto,0))+isnull(acrescimo,0))-isnull(conta_a_receber.taxa,0) Valor" +
                                        ",CONVERT (VARCHAR ,Emissao,103) as Emissao" +
                                        ",CONVERT (VARCHAR,vencimento,103) as vencimento" +
                                        ",Status = case Status when 1 then 'ABERTO'" +
                                        "                      when 2 then 'CONCLUIDO' " +
                                        "                      when 3 then 'CANCELADO' " +
                                        "                      when 4 then 'LANCADO' " +
                                        "         END " +
                                        ",codigo_centro_custo" +
                                        ",id_cartao=isnull(id_cartao,id_finalizadora)" +
                                        ",id_cc" +
                                         ", ROW_NUMBER() OVER(" + pg.orderBy + ") numeroLinha" + 
                                 " from conta_a_receber " +
                                 "     left join cliente on conta_a_receber.codigo_cliente = cliente.codigo_cliente " +
                                 "     left join Cartao  on convert(int,Conta_a_receber.id_Bandeira) = convert(int,Cartao.id_bandeira) " +
                                 "                      and Conta_a_receber.finalizadora= Cartao.nro_Finalizadora" +
                                 "                      and Conta_a_receber.Rede_Cartao =cartao.id_rede";

                String totalRegistro = Conexao.retornaUmValor("select count(*) from conta_a_receber where filial ='" + usr.getFilial() + "'", null);
                String strRegistro = "Select count(*) from (" + sqlGrid;
                int.TryParse(totalRegistro, out pg.totalCadastrado);

                pg.sqlFinal = " with conta_rcber as (" + sqlGrid;
                if (!sql.Equals(""))
                {
                    strRegistro += " where " + sql;
                    pg.sqlFinal += " where " + sql;
                }
                pg.sqlFinal += ") ";
                strRegistro += ")as a ";

                String totalFiltros = Conexao.retornaUmValor(strRegistro, null);
                int.TryParse(totalFiltros, out pg.totalfiltro);

                String strSqlTotValor = pg.sqlFinal + " Select sum(valor) from conta_rcber ";
                String strvlrTotal = Conexao.retornaUmValor(strSqlTotValor, usr);
                Decimal vlrTotal = 0;
                Decimal.TryParse(strvlrTotal, out vlrTotal);
                lblTotalValores.Text = " R$" + vlrTotal.ToString("N2");

                Session.Remove("contasReceberFiltro");
                Session.Add("contasReceberFiltro", pg);

                ddlPag.Items.Clear();
                ddlPag1.Items.Clear();
                for (int i = 0; i < pg.totalfiltro; i += 100)
                {
                    int gridInicio = i;
                    int gridFim = i + 100;
                    if (gridFim > pg.totalfiltro)
                    {
                        decimal pagInicio = (decimal.Truncate(pg.totalfiltro / 100) * 100);
                        gridInicio = Convert.ToInt32(pagInicio);
                        gridFim = pg.totalfiltro;
                    }
                    ddlPag.Items.Add((gridInicio + 1) + "-" + gridFim);
                    ddlPag1.Items.Add((gridInicio + 1) + "-" + gridFim);

                }

                if (pg.fimGrid > pg.totalfiltro)
                {
                    pg.fimGrid = pg.totalfiltro;
                }
                carregarGrid();

                lblPesquisaErro.Text = "";
                exibirBtnBaixa();
            }
            catch (Exception err)
            {
                lblPesquisaErro.Text = err.Message;
            }
        }
        protected void ddlPag_SelectedIndexChanged(object sender, EventArgs e)
        {
            ContasReceberFiltro pg = (ContasReceberFiltro)Session["contasReceberFiltro"];
            DropDownList ddl = (DropDownList)sender;
            int gridInicio = int.Parse(ddlPag.Text.Substring(0, ddl.Text.IndexOf("-"))) - 1;
            int gridFim = int.Parse(ddlPag.Text.Substring(ddl.Text.IndexOf("-") + 1));

            pg.iniGrid = gridInicio;
            pg.fimGrid = gridFim;



            Session.Remove("contasReceberFiltro");
            Session.Add("contasReceberFiltro", pg);
            carregarGrid();
        }
        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
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
                    campo = "CentroCusto";
                    break;

            }

            Session.Add("camporecebe", campo);


            exibeLista();
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisar(false);
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
                case "txtCliente":
                    lbllista.Text = "Escolha um Cliente";
                    sqlLista = "select codigo_cliente Codigo,nome_cliente nome from cliente where codigo_cliente like '%" + TxtPesquisaLista.Text + "%' or nome_cliente like '%" + TxtPesquisaLista.Text + "%'";

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

            modalPnFundo.Show();
            TxtPesquisaLista.Focus();
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
        protected void BtnBaixar_Click(object sender, EventArgs e)
        {
            // lblErroGrid.Visible = false;
            //lblGridtitulo.Visible = true;

            ArrayList arr = new ArrayList();
            ArrayList cab = new ArrayList();
            cab.Add("DOCUMENTO");
            cab.Add("CLIENTE");
            cab.Add("NomeCliente");
            cab.Add("VALOR");
            cab.Add("VENCIMENTO");
            cab.Add("VALOR PAGO");
            cab.Add("DATA PAGAMENTO");
            cab.Add("id_cc");
            cab.Add("emissao");


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

                                row.Add(gridPesquisa.DataKeys[i][0].ToString()); //DOCUMENTO
                                row.Add(gridPesquisa.DataKeys[i][1].ToString());//CLIENTE
                                row.Add(gridPesquisa.DataKeys[i][6].ToString());//NomeCliente
                                row.Add(gridPesquisa.DataKeys[i][2].ToString());//VALOR
                                row.Add(gridPesquisa.DataKeys[i][4].ToString());//VENCIMENTO
                                row.Add(gridPesquisa.DataKeys[i][2].ToString()); //VALOR PAGO
                                row.Add(gridPesquisa.DataKeys[i][5].ToString()); //DATA PAGAMENTO
                                row.Add(gridPesquisa.DataKeys[i][7].ToString());//id_cc
                                row.Add(gridPesquisa.DataKeys[i][3].ToString());//emissao

                                arr.Add(row);
                                preenche = false;
                            }

                        }//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
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
        protected void chkSeleciona_CheckedChanged(object sender, EventArgs e)
        {

            foreach (GridViewRow item in gridPesquisa.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");

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
        protected void chkSelecionaItem_CheckedChanged(object sender, EventArgs e)
        {
            exibirBtnBaixa();
        }

        protected void exibirBtnBaixa()
        {
            BtnBaixar.Visible = false;
            BtnCancelarBaixa.Visible = false;
            Decimal vlrSelecionado = 0;
            for (int i = 0; gridPesquisa.Rows.Count > i; i++)
            {

                CheckBox chk = (CheckBox)gridPesquisa.Rows[i].FindControl("chkSelecionaItem");

                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        if (gridPesquisa.DataKeys[i][5].ToString().Equals("ABERTO"))
                        {
                            vlrSelecionado += Funcoes.decTry(gridPesquisa.DataKeys[i][2].ToString());
                            BtnBaixar.Visible = true;

                        }
                        else if (gridPesquisa.DataKeys[i][5].ToString().Equals("CONCLUIDO"))
                        {
                            BtnCancelarBaixa.Visible = true;
                        }
                    }
                }
            }

            lblSelecionado.Text = " R$ " + vlrSelecionado.ToString("N2");
        }
        protected void btnConfirmaGrid_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lblErroGrid.Visible = false;
                lblGridtitulo.Visible = true;
                if (verificaDatas())
                {
                    SalvarBaixas();
                }
                else
                {
                    ModalGrid.Show();
                    ModalConfirma.Show();
                }


            }
            catch (Exception err)
            {

                lblErroGrid.Text = "Erro : " + err.Message;
                lblGridtitulo.Visible = true;
                lblErroGrid.Visible = true;
                ModalGrid.Show();
            }

        }
        protected void SalvarBaixas()
        {
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
            try
            {
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
                        throw new Exception("O Documento:" + item.Cells[0].Text + " esta com o valor zero ou negativo;");
                    }

                    DropDownList ddlConta = (DropDownList)item.FindControl("ddlContaCorrente");
                    ddlConta.BackColor = System.Drawing.Color.White;

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
                            throw new Exception("Data invalida do Documento:" + item.Cells[0].Text);
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

                        //if (!txtDtpgTodas.Text.Equals("")&& IsDate(txtDtpgTodas.Text))
                        //{
                        //    txtDtPg.Text = txtDtpgTodas.Text;
                        //}

                        User usr = (User)Session["User"];
                        String documento = GridBaixas.DataKeys[i][0].ToString().Trim();
                        String cliente = GridBaixas.DataKeys[i][1].ToString().Trim();
                        String emissao = GridBaixas.DataKeys[i][4].ToString().Trim();

                        conta_a_receberDAO obj = new conta_a_receberDAO(documento, cliente, emissao, usr);



                        if (strStatus.Equals("1"))
                        {
                            obj.Estornar();
                        }
                        else if (IsDate(txtDtPg.Text))
                        {
                            obj.status = int.Parse(strStatus);

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
                                obj.Obs += "\n" + txtObsBaixa.Text;
                            }
                            obj.salvar(false);
                        }

                        pesquisar(true);

                    }


                }
            }

            catch (Exception err)
            {
                lblErroGrid.Text = "Erro:" + err.Message;
                lblGridtitulo.Visible = false;
                lblErroGrid.Visible = true;
                fecha = false;


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

        protected bool verificaDatas()
        {

            for (int i = 0; i < GridBaixas.Rows.Count; i++)
            {
                TextBox txtDtPg = (TextBox)GridBaixas.Rows[i].FindControl("txtDataPago");
                User usr = (User)Session["User"];
                String documento = GridBaixas.DataKeys[i][0].ToString();
                String cliente = GridBaixas.DataKeys[i][1].ToString().Trim();
                String emissao = GridBaixas.DataKeys[i][4].ToString().Trim();

                txtDtPg.BackColor = System.Drawing.Color.White;

                if (!IsDate(txtDtPg.Text))
                {
                    txtDtPg.BackColor = System.Drawing.Color.Red;
                    txtDtPg.Focus();
                    throw new Exception("DATA INVALIDA!");
                }
                conta_a_receberDAO obj = new conta_a_receberDAO(documento, cliente, emissao, usr);
                if (DateTime.Parse(txtDtPg.Text) < obj.Emissao)
                {
                    txtDtPg.BackColor = System.Drawing.Color.Red;
                    txtDtPg.Focus();
                    return false;
                }


            }

            return true;
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

                if (listaAtual.Equals("txtCliente"))
                {

                    txtcliente.Text = ListaSelecionada(1);
                    txtNomeCliente.Text = ListaSelecionada(2);

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
        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalPnFundo.Hide();
        }



        protected void btnCancelaGrid_Click(object sender, ImageClickEventArgs e)
        {
            ModalGrid.Hide();
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
                txt.Text = e.Row.Cells[3].Text;


                TextBox txtdt = (TextBox)e.Row.FindControl("txtDataPago");
                txtdt.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtdt.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");


            }
        }

        protected void txtDataDe_TextChanged(object sender, EventArgs e)
        {
            if (IsDate(txtDe.Text))
            {
                txtAte.Text = txtDe.Text;
            }
        }
        protected void btnConfirmaData_Click(object sender, ImageClickEventArgs e)
        {
            SalvarBaixas();
        }
        protected void btnCancelaData_Click(object sender, ImageClickEventArgs e)
        {
            ModalConfirma.Hide();
            ModalGrid.Show();
        }
        protected void gridPesquisa_Sorting(object sender, GridViewSortEventArgs e)
        {
            pesquisar(true, e.SortExpression);
        }

        protected void imgBtnLimpar_Click(object sender, ImageClickEventArgs e)
        {
            txtDocumento.Text = "";
            txtcliente.Text = "";
            txtNomeCliente.Text = "";
            txtValor.Text = "";
            txtDe.Text = "";
            txtAte.Text = "";
            DllTipoPesquisa.SelectedIndex = 0;
            DllStatus.SelectedIndex = 0;
            txtCentroCusto.Text = "";
            ddlTipoCartao.Text = "";
            txtCNPJ.Text = "";

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

        protected void carregarGrid()
        {

            User usr = (User)Session["User"];
            ContasReceberFiltro pg = (ContasReceberFiltro)Session["contasReceberFiltro"];
            String sqlFinal = pg.sqlFinal + " Select * from conta_rcber where numeroLinha between " + (pg.iniGrid + 1) + " and " + pg.fimGrid + " order by numeroLinha";

            gridPesquisa.DataSource = Conexao.GetTable(sqlFinal, usr, false);
            gridPesquisa.DataBind();


            ddlPag.Text = (pg.iniGrid + 1) + "-" + pg.fimGrid;
            ddlPag1.Text = (pg.iniGrid + 1) + "-" + pg.fimGrid;


            if (pg.totalfiltro > 100)
            {
                lblRegistros.Text = (pg.iniGrid + 1) + " ate " + pg.fimGrid + " de " + pg.totalfiltro + " Pesquisados de " + pg.totalCadastrado + " Cadastrados";
            }
            else
            {
                lblRegistros.Text = pg.totalfiltro + " Pesquisados de " + pg.totalCadastrado + " Cadastrados";

            }

            for (int i = 0; gridPesquisa.Rows.Count > i; i++)
            {
                DateTime dtVenc = new DateTime();
                DateTime.TryParse(gridPesquisa.DataKeys[i][4].ToString(), out dtVenc);
                DateTime dtHoje = DateTime.Now.Date;
                if (gridPesquisa.DataKeys[i][5].ToString().Equals("ABERTO") && dtVenc < DateTime.Now.Date)
                {
                    gridPesquisa.Rows[i].CssClass = "linhaInativo";
                }
            }


            Session.Remove("contasReceberFiltro");
            Session.Add("contasReceberFiltro", pg);

        }

        protected void btnPag_Click(object sender, EventArgs e)
        {


            Button btn = (Button)sender;
            ContasReceberFiltro pg = (ContasReceberFiltro)Session["contasReceberFiltro"];


            if (btn.ID.Equals("btnPagInicio") || btn.ID.Equals("btnPagInicio1"))
            {
                pg.iniGrid = 0;
                pg.fimGrid = 100;
            }
            else if (btn.ID.Equals("btnPagAnterio") || btn.ID.Equals("btnPagAnterio1"))
            {
                pg.iniGrid -= 100;
                pg.fimGrid = pg.iniGrid + 100;
                if (pg.iniGrid < 0)
                {
                    pg.iniGrid = 0;
                    pg.fimGrid = 100;
                }


            }
            else if (btn.ID.Equals("btnPagProximo") || btn.ID.Equals("btnPagProximo1"))
            {

                pg.iniGrid += 100;
                pg.fimGrid = pg.iniGrid + 100;
                if (pg.fimGrid > pg.totalfiltro)
                {
                    decimal pagInicio = (decimal.Truncate(pg.totalfiltro / 100) * 100);
                    pg.iniGrid = Convert.ToInt32(pagInicio);
                    pg.fimGrid = pg.totalfiltro;
                }
            }
            else if (btn.ID.Equals("btnPagFim") || btn.ID.Equals("btnPagFim1"))
            {
                decimal pagInicio = (decimal.Truncate(pg.totalfiltro / 100) * 100);
                pg.iniGrid = Convert.ToInt32(pagInicio);
                pg.fimGrid = pg.totalfiltro;
            }




            Session.Remove("contasReceberFiltro");
            Session.Add("contasReceberFiltro", pg);
            carregarGrid();
            exibirBtnBaixa();

        }

    }
}