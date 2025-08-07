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

namespace visualSysWeb.modulos.Financeiro.pages
{
    public partial class ChequeClienteDetalhes : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];


            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    status = "sopesquisa";
                    Session.Remove("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "Cliente");
                    exibeLista();
                    Habilitar(false);
                }

            }
            else
            {
                if (Request.Params["campoIndex"] != null)
                {
                    try
                    {
                        if (!IsPostBack)
                        {
                            txtCodigo_Cliente.Text = Request.Params["campoIndex"];
                            status = "sopesquisa";
                            Session.Remove("objCheques" + urlSessao());

                            Habilitar(false);

                        }
                        carregarDados();
                    }
                    catch (Exception err)
                    {
                        lblError.Text = err.Message;
                        lblError.ForeColor = System.Drawing.Color.Red;
                    }

                }

            }
            carregabtn(pnBtn);

        }


        protected void Habilitar(bool enable)
        {
            txtCodigo_Cliente.Enabled = false;
            txtNome_Cliente.Enabled = false;

            EnabledControls(pnItens, true);
        }
        protected void carregarDados()
        {
            User usr = (User)Session["User"];
            ArrayList cheques = new ArrayList();
            if (!txtCodigo_Cliente.Equals(""))
            {
                txtNome_Cliente.Text = Conexao.retornaUmValor("Select nome_cliente from cliente where codigo_cliente ='" + txtCodigo_Cliente.Text + "'", usr);


                String sql = "select * from cheque where codigo_cliente ='" + txtCodigo_Cliente.Text + "'";
                if (!txtDe.Text.Equals("") && !txtAte.Text.Equals(""))
                {
                    sql += " and " + ddlTipoPesquisa.SelectedValue + " between '" + DateTime.Parse(txtDe.Text).ToString("yyyy-MM-dd") + "' and '" + DateTime.Parse(txtAte.Text).ToString("yyyy-MM-dd") + "'";
                }

                if (!txtNumeroChequePesquisa.Text.Equals(""))
                {

                    sql += " and numero_cheque='" + txtNumeroChequePesquisa.Text + "'";
                }


                SqlDataReader rs = Conexao.consulta(sql + " order by data_cadastro desc ", usr, false);
                while (rs.Read())
                {
                    chequeDAO ch = new chequeDAO();
                    ch.Codigo_Cliente = txtCodigo_Cliente.Text;
                    ch.Nome_Cliente = txtNome_Cliente.Text;
                    ch.Lancamento_Cheque = rs["Lancamento_Cheque"].ToString();
                    ch.Emissao_Cheque = (rs["Emissao_Cheque"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Emissao_Cheque"].ToString()));
                    ch.Deposito_Cheque = (rs["Deposito_Cheque"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Deposito_Cheque"].ToString()));
                    ch.Banco_Cheque = rs["Banco_Cheque"].ToString();
                    ch.Agencia_Cheque = rs["Agencia_Cheque"].ToString();
                    ch.Numero_Cheque = rs["Numero_Cheque"].ToString();
                    ch.Documento_Cheque = rs["Documento_Cheque"].ToString();
                    ch.Total_Cheque = (Decimal)(rs["Total_Cheque"].ToString().Equals("") ? new Decimal() : rs["Total_Cheque"]);
                    ch.Devolvido_Cheque = rs["Devolvido_Cheque"].ToString();
                    ch.Compensado_Cheque = rs["Compensado_Cheque"].ToString();
                    ch.utilizado_cheque = (Decimal)(rs["utilizado_cheque"].ToString().Equals("") ? new Decimal() : rs["utilizado_cheque"]);
                    ch.Data_cadastro = (rs["Data_cadastro"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data_cadastro"].ToString()));
                    ch.Responsavel_Cheque = rs["Responsavel_Cheque"].ToString();
                    ch.Responsavel_Telefone = rs["Responsavel_Telefone"].ToString();
                    ch.Observacao = rs["Observacao"].ToString();

                    cheques.Add(ch);
                }

            }
            ArrayList cabecalho = new ArrayList();
            cabecalho.Add("CODIGO_CLIENTE");
            cabecalho.Add("NOME_CLIENTE");
            cabecalho.Add("LANCAMENTO_CHEQUE");
            cabecalho.Add("Emissao_Cheque");
            cabecalho.Add("Deposito_Cheque");
            cabecalho.Add("Banco_Cheque");
            cabecalho.Add("Agencia_Cheque");
            cabecalho.Add("Numero_Cheque");
            cabecalho.Add("Documento_Cheque");
            cabecalho.Add("Total_Cheque");
            cabecalho.Add("Devolvido_Cheque");
            cabecalho.Add("Compensado_Cheque");
            cabecalho.Add("utilizado_cheque");
            cabecalho.Add("Data_Cadastro");
            cabecalho.Add("Responsavel_Cheque");
            cabecalho.Add("Responsavel_Telefone");
            cabecalho.Add("Observacao");


            ArrayList TABELA = new ArrayList();
            TABELA.Add(cabecalho);

            foreach (chequeDAO cheq in cheques)
            {
                TABELA.Add(cheq.ArrToString());
            }
            gridCheque.DataSource = Conexao.GetArryTable(TABELA);
            gridCheque.DataBind();
            Session.Remove("objCheques" + urlSessao());
            Session.Add("objCheques" + urlSessao(), cheques);
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChequeClienteDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            status = "editar";
            Habilitar(true);
            carregarDados();

        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChequeCliente.aspx");
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                /*
                Conexao.executarSql("delete from cheque where codigo_cliente='" + txtCodigo_Cliente.Text + "'", conn, tran);
                ArrayList cheq = (ArrayList)Session["objCheques" + urlSessao()];

                foreach (chequeDAO cheque in cheq)
                {
                    cheque.salvar(true, conn, tran);

                }
                ArrayList arrCheqExcluidos = (ArrayList)Session["objChequesExcluidos" + urlSessao()];
                if (arrCheqExcluidos != null)
                {
                    foreach(chequeDAO chExcluido in arrCheqExcluidos)
                    {
                        chExcluido.excluir(conn,tran);
                    }
                }
                
                lblError.Text = "Salvo Com Sucesso";
                lblError.ForeColor = System.Drawing.Color.Blue;
                tran.Commit();
                carregarDados();
                Habilitar(false);
                 */
            }
            catch (Exception err)
            {
                tran.Rollback();

                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }


        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChequeCliente.aspx");
        }

        protected override bool campoDesabilitado(Control campo)
        {

            String[] campos = { "txtCodigo_Cliente", 
                                "txtNome_Cliente", 
                                    "",
                                    "",
                                    "", 
                                    "" 
                                     };

            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoObrigatorio(Control campo)
        {

            String[] campos = { "", 
                                    "", 
                                    "",
                                    "",
                                    "", 
                                    "" 
                                     };

            return existeNoArray(campos, campo.ID + "");

        }

        protected void gridCheque_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (isnumero(e.Row.Cells[i].Text))
                    {
                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                        // e.Row.Cells[i].Text = Decimal.Parse(e.Row.Cells[i].Text).ToString("N2");

                    }
                }
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

        }

        protected void gridCheque_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            LimparCampos(pnItens);

            lblErroItem.Text = "";
            chequeDAO objCheque = null;
            int index = 0;
            ArrayList cheq = (ArrayList)Session["objCheques" + urlSessao()];
            if (cheq != null)
            {
                index = Convert.ToInt32(e.CommandArgument);
                objCheque = (chequeDAO)cheq[index];
            }
            if (objCheque != null)
            {
                objCheque.index = index;
                txtEmissao.Text = objCheque.Emissao_ChequeBr();
                txtDeposito.Text = objCheque.Deposito_ChequeBr();
                txtBanco.Text = objCheque.Banco_Cheque;
                txtAgencia.Text = objCheque.Agencia_Cheque;
                txtNumeroCheque.Text = objCheque.Numero_Cheque;
                TxtTotal.Text = objCheque.Total_Cheque.ToString("N2");
                txtDocumento.Text = objCheque.Documento_Cheque;
                txtResponsavelCheque.Text = objCheque.Responsavel_Cheque;
                txtResponsavelTelefone.Text = objCheque.Responsavel_Telefone;
                txtObservacao.Text = objCheque.Observacao;
                Session.Remove("chequeItem" + urlSessao());
                Session.Add("chequeItem" + urlSessao(), objCheque);
                ModalItens.Show();

                ChkCompensado.Visible = true;
                if (objCheque.Devolvido_Cheque.Equals("S"))
                {
                    lblDevolvido.Visible = true;
                    BtnDevolvido.Visible = false;
                }
                else
                {
                    lblDevolvido.Visible = false;
                    BtnDevolvido.Visible = true;
                }
                if (!status.Equals("visualizar"))
                {
                    btnExcluirCheque.Visible = true;
                    lblExcluir.Visible = true;
                }
                btnCancelaItem.Visible = true;
                lblCancelaItem.Visible = true;
            }
            carregarDados();
        }

        protected void ImgBtnAddItens_Click(object sender, ImageClickEventArgs e)
        {
            EnabledControls(pnItensFrame, true);
            ModalItens.Show();
            Session.Remove("chequeItem" + urlSessao());
            lblErroItem.Text = "";
            LimparCampos(pnItens);
            ChkCompensado.Visible = false;
            BtnDevolvido.Visible = false;
            lblDevolvido.Visible = false;
            btnExcluirCheque.Visible = false;
            lblExcluir.Visible = false;
        }

        protected void btnFechar_Click(object sender, ImageClickEventArgs e)
        {

            String selecionado = ListaSelecionada(1);

            if (!selecionado.Equals("") && !selecionado.Equals("------"))
            {
                String listaAtual = (String)Session["camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                Session.Remove("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));

                if (listaAtual.Equals("Cliente"))
                {
                    txtCodigo_Cliente.Text = ListaSelecionada(1);
                    txtNome_Cliente.Text = ListaSelecionada(2);
                    carregarDados();
                }
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
                        return item.Cells[campo].Text.Trim();
                    }
                }
            }

            return "";
        }
        protected void Img_Click(object sender, ImageClickEventArgs e)
        {
            carregarDados();

        }

        protected void txt_TextChanged(object sender, EventArgs e)
        {

        }
        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {

            modalPnFundo.Hide();
            carregarDados();
        }
        protected void TxtPesquisaLista_TextChanged(object sender, EventArgs e)
        {
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
                case "Cliente":
                    sqlLista = "select codigo_cliente Codigo, Nome_Cliente Nome from Cliente where (codigo_cliente like '%" + TxtPesquisaLista.Text + "%' or nome_cliente like '%" + TxtPesquisaLista.Text + "%') AND ISNULL(inativo,0)=0"; ;
                    lbllista.Text = "Escolha um Cliente";
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
        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
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
        protected void btnConfirmaItens_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (TxtTotal.Text.Trim().Equals(""))
                    throw new Exception("Por favor informar um valor para o Total");

                lblErroItem.Text = "";
                ArrayList arrCheque = (ArrayList)Session["objCheques" + urlSessao()];
                chequeDAO cheque = (chequeDAO)Session["chequeItem" + urlSessao()];
                bool novo = false;
                if (cheque == null)
                {
                    cheque = new chequeDAO();
                    novo = true;
                }
                else
                {
                    novo = false;
                }
                cheque.Codigo_Cliente = txtCodigo_Cliente.Text;
                cheque.Nome_Cliente = txtNome_Cliente.Text;
                cheque.Emissao_Cheque = DateTime.Parse(txtEmissao.Text);
                cheque.Deposito_Cheque = DateTime.Parse(txtDeposito.Text);
                cheque.Banco_Cheque = txtBanco.Text;
                cheque.Agencia_Cheque = txtAgencia.Text;
                cheque.Numero_Cheque = txtNumeroCheque.Text;
                cheque.Total_Cheque = Decimal.Parse(TxtTotal.Text);
                cheque.Documento_Cheque = txtDocumento.Text;
                cheque.Compensado_Cheque = (ChkCompensado.Checked ? "S" : "N");
                cheque.Responsavel_Cheque = txtResponsavelCheque.Text;
                cheque.Responsavel_Telefone = txtResponsavelTelefone.Text;
                //Checa se foi incluso valor superior a 250 caracteres
                if (txtObservacao.Text.Trim().Length > 250)
                {
                    cheque.Observacao = txtObservacao.Text.Trim().Substring(0, 249);
                }
                else
                {
                    cheque.Observacao = txtObservacao.Text;
                }

                if (novo)
                {
                    cheque.Data_cadastro = DateTime.Now;
                    arrCheque.Add(cheque);
                }
                else
                {
                    arrCheque[cheque.index] = cheque;
                }
                cheque.salvar(novo);

                Session.Remove("chequeItem" + urlSessao());
                carregarDados();
            }
            catch (Exception err)
            {
                lblErroItem.Text = err.Message;
                lblErroItem.ForeColor = System.Drawing.Color.Red;
                ModalItens.Show();
            }


        }
        protected void btnCancelaItem_Click(object sender, ImageClickEventArgs e)
        {
            modalPnFundo.Hide();
            carregarDados();
        }

        protected void BtnDevolvido_Click(object sender, EventArgs e)
        {
            ArrayList arrCheque = (ArrayList)Session["objCheques" + urlSessao()];
            chequeDAO cheque = (chequeDAO)Session["chequeItem" + urlSessao()];
            if (cheque != null && arrCheque != null)
            {
                try
                {
                    cheque.Devolvido_Cheque = "S";
                    cheque.salvar(false);
                    arrCheque[cheque.index] = cheque;
                    Conexao.executarSql("update cliente set utilizado=(utilizado+" + cheque.Total_Cheque.ToString("N2").Replace(".", "").Replace(",", ".") + " ),situacao='ATRASO' where codigo_cliente='" + txtCodigo_Cliente.Text + "'");
                    lblErroItem.Text = "Cheque:" + cheque.Documento_Cheque + " Devolvido com sucesso ";
                    lblError.ForeColor = System.Drawing.Color.Blue;
                }
                catch (Exception err)
                {

                    lblError.Text = err.Message;
                    lblError.ForeColor = System.Drawing.Color.Red;

                }
            }
            ModalItens.Hide();
        }

        protected void btnExcluirCheque_Click(object sender, ImageClickEventArgs e)
        {
            ArrayList arrCheque = (ArrayList)Session["objCheques" + urlSessao()];
            chequeDAO cheque = (chequeDAO)Session["chequeItem" + urlSessao()];
            arrCheque.RemoveAt(cheque.index);

            ArrayList arrChequeExcluidos = (ArrayList)Session["objChequesExcluidos" + urlSessao()];
            if (arrChequeExcluidos == null)
                arrChequeExcluidos = new ArrayList();

            arrChequeExcluidos.Add(cheque);
            cheque.excluir();

            Session.Remove("objChequesExcluidos" + urlSessao());
            Session.Add("objChequesExcluidos" + urlSessao(), arrChequeExcluidos);
            ModalItens.Hide();
            lblError.Text = "Cheque Excluido com Sucesso!";
            carregarDados();
        }
    }
}