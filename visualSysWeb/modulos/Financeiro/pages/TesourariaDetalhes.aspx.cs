using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using System.Data.SqlClient;
using System.Collections;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Financeiro.pages
{

    public partial class TesourariaDetalhes : visualSysWeb.code.PagePadrao
    {
        private bool emEncerramento = false;

        static String ultimaOrdem = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["id"] != null)
            {
                User usr = (User)Session["user"];
                if (usr != null)
                {
                    if (!IsPostBack)
                    {
                        Session.Remove("tesourariaEncerrar");
                        Session.Add("tesourariaEncerrar" + urlSessao(), emEncerramento);
                        carregarOperacoes(usr, "");
                    }
                    else
                    {
                        emEncerramento = (bool)Session["tesourariaEncerrar" + urlSessao()];
                    }
                }
            }
            carregabtn(pnBtn);
        }





        protected void carregarOperacoes(User usr, String ordem)
        {

            if (ordem.Equals(""))
            {
                ordem = "1";
            }
            else
            {
                if (ordem.Equals(ultimaOrdem))
                {
                    ordem += " Desc ";
                    ultimaOrdem = "";
                }
                else
                {
                    ultimaOrdem = ordem;
                }
            }

            String id = Request.Params["id"];
            String emissao = Request.Params["emissao"];
            String pdv = Request.Params["pdv"];
            String idFechamento = Request.Params["idfecha"];

            status_pdvDAO stsPdv = (status_pdvDAO)Session["tesouraria" + urlSessao()];
            if (!IsPostBack)
            {
                int id_fecha = Funcoes.intTry(idFechamento);
                stsPdv = new status_pdvDAO(int.Parse(id), int.Parse(pdv), id_fecha, usr);
                lblInicioPeriodo.Text = stsPdv.Data_Abertura.ToString("dd/MM/yyyy HH:mm");
                if (!stsPdv.data_fechamento_movimento.Equals(DateTime.MinValue))
                    lblFimPeriodo.Text = stsPdv.data_fechamento_movimento.ToString("dd/MM/yyyy HH:mm");
                else
                    lblFimPeriodo.Text = "---";

                lblOperador.Text = id + '-' + Conexao.retornaUmValor("Select Nome from operadores where ID_Operador =" + id, usr);
                lblcaixa.Text = pdv;

                lblIdMovimento.Text = id_fecha.ToString();

                if (stsPdv.Status.Equals("OPERANDO"))
                {
                    stsPdv.limparDetalhes();
                    stsPdv.inserirDetalhes();
                }


                Session.Remove("tesouraria" + urlSessao());
                Session.Add("tesouraria" + urlSessao(), stsPdv);

                if (stsPdv.Status.Equals("FECHADO") && usr.adm() == true)
                {
                    BtnEstorno.Visible = true;
                }
                else
                {
                    BtnEstorno.Visible = false;
                }
            }

            String sql = "";
            if (usr.adm())
            {



                sql = "Select finalizadora AS COD_FINALIZADORA , id_finalizadora  AS FINALIZADORA," +
                            " convert( decimal(18,2),Sum(Valor)) as REGISTRADO ," +
                            " convert(decimal(18,2),SUM(digitado)) as VALOR," +
                            "convert(decimal(18,2),(SUM(digitado)- Sum(Valor) ))AS DIFERENCA" +
                            ", convert(decimal(18, 2), sum(Deposito)) AS Deposito"+
                      " from (" +
                           " Select CR.finalizadora,  cr.id_finalizadora, 	Sum(isnull(cr.Total,0)) AS Valor  , 0 AS Digitado, 0 as Deposito " +
                           "     from Tesouraria_detalhes cr " +
                           " where cr.id_fechamento =" + stsPdv.id_fechamento + " and  cr.Emissao >='" + stsPdv.Data_Abertura.ToString("yyyyMMdd") + "' and cr.pdv=" + pdv + " and cr.operador = " + id +
                           " group by CR.finalizadora,  cr.id_finalizadora " +
                           " UNION " +
                           " SELECT	F.Nro_Finalizadora AS COD_FINALIZADORA," +
                                   " F.FINALIZADORA," +
                                   " 0 as REGISTRADO, " +
                                   " convert(decimal(18,2),ISNULL((SELECT SUM(ISNULL(T.Total_Entregue,0)) " +
                                   "     FROM  Tesour" +
                                   "aria AS T  " +
                                   "     WHERE  F.Nro_Finalizadora = T.FINALIZADORA " +
                                             "  AND T.ID_OPERADOR =  " + id +
                                             "  AND T.DATA_ABERTURA= '" + stsPdv.Data_Abertura.ToString("yyyyMMdd") + "' " +
                                             "  AND T.PDV= " + pdv +
                                             "  AND T.ID_FECHAMENTO=" + stsPdv.id_fechamento + "),0)) AS VALOR " +
                                   ", convert(decimal(18,2),ISNULL((SELECT SUM(ISNULL(T2.Deposito,0)) " +
                                   "     FROM  Tesour" +
                                   "aria AS T2  " +
                                   "     WHERE  F.Nro_Finalizadora = T2.FINALIZADORA " +
                                             "  AND T2.ID_OPERADOR =  " + id +
                                             "  AND T2.DATA_ABERTURA= '" + stsPdv.Data_Abertura.ToString("yyyyMMdd") + "' " +
                                             "  AND T2.PDV= " + pdv +
                                             "  AND T2.ID_FECHAMENTO=" + stsPdv.id_fechamento + "),0)) AS Deposito " +

                           " FROM FINALIZADORA AS F " +


                       //" SELECT Finalizadora, id_finalizadora,0,T.Total_Entregue FROM  Tesouraria t " +
                       //" WHERE T.ID_OPERADOR = " + id + " AND T.DATA_ABERTURA= '" + Dt.ToString("yyyyMMdd") + "' AND T.PDV= " + pdv +
                       " )as a " +
                       " GROUP BY finalizadora, id_finalizadora " +
                       " order by " + ordem;
                gridItens.Columns[2].Visible = true; //Registrado
                gridItens.Columns[4].Visible = true; //Diferenca
                gridItens.Columns[5].Visible = true;//Deposito
                btnEncerrar.Visible = true;
                //divReprocessa.Visible = true;

            }
            else
            {
                //if (ordem.IndexOf("id_finalizadora") >= 0)
                //{
                //    ordem = ordem.Replace("id_finalizadora", "FINALIZADORA");
                //} 
                //else if (ordem.IndexOf("finalizadora") >= 0)
                //{
                //    ordem = ordem.Replace("finalizadora", "Nro_Finalizadora");
                //} 

                sql = "SELECT	F.Nro_Finalizadora AS COD_FINALIZADORA," +
                               " F.FINALIZADORA," +
                               " 0 as REGISTRADO, " +
                               " convert(decimal(18,2),ISNULL((SELECT SUM(ISNULL(T.Total_Entregue,0)) " +
                               "     FROM  Tesouraria AS T  " +
                               "     WHERE  F.Nro_Finalizadora = T.FINALIZADORA " +
                                         "  AND T.ID_OPERADOR =  " + id +
                                         "  AND T.DATA_ABERTURA= '" + stsPdv.Data_Abertura.ToString("yyyyMMdd") + "' " +
                                         "  AND T.PDV= " + pdv + "),0)) AS VALOR ," +
                               " 0 AS DIFERENCA " +
                               ", 0 AS Deposito" +
                       " FROM FINALIZADORA AS F " +
                       " ORDER BY " + ordem;

                gridItens.Columns[2].Visible = false; //Registrado
                gridItens.Columns[4].Visible = false; //Diferenca
                btnEncerrar.Visible = false;
                //divReprocessa.Visible = false;
            }
            gridItens.DataSource = Conexao.GetTable(sql, usr, false);
            gridItens.DataBind();

            gridCancelados.DataSource = stsPdv.tbCancelados();
            gridCancelados.DataBind();

            bool bBloqueiaOperando = Funcoes.valorParametro("PDV_BLOQ_OPERACAO", usr).ToUpper().Equals("TRUE");
            bool bOp = false;
            if (bBloqueiaOperando)
            {
                bOp = true;
            }
            else
            {
                bOp = !stsPdv.Status.ToUpper().Equals("OPERANDO");
            }
            if (!stsPdv.Status.ToUpper().Equals("ABERTO") && bOp)
            {
                EnabledControls(gridItens, false);
                btnEncerrar.Visible = false;
                GridViewRow footer = gridItens.FooterRow;
                Label lblTotalDiferenca = (Label)footer.FindControl("lblDiferencaTotal");
                Decimal vDif = 0;
                Decimal.TryParse(lblTotalDiferenca.Text, out vDif);
                if (vDif != 0)
                {
                    divImprimir.Visible = true;

                }
                else
                {
                    divImprimir.Visible = false;
                }
                status = "sopesquisa";
                divReprocessa.Visible = false;
            }
            else
            {
                status = "editar";
            }
            carregabtn(pnBtn);
        }
        protected void imgBtnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            String id = Request.Params["id"];
            String emissao = Request.Params["emissao"];
            String pdv = Request.Params["pdv"];
            String idFecha = Request.Params["idfecha"].ToString();
            DateTime Dt;
            DateTime.TryParse(emissao, out Dt);
            RedirectNovaAba("TesourariaReciboPrint.aspx?emissao=" + Dt.ToString("dd/MM/yyyy") + "&id=" + id + "&pdv=" + pdv + "&idFechamento=" + idFecha);
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            //String emissao = Request.Params["emissao"];
            Response.Redirect("Tesouraria.aspx");
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            salvar();
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {

            status_pdvDAO stsPdv = (status_pdvDAO)Session["tesouraria" + urlSessao()];
            if (stsPdv.Status.Equals("OPERANDO"))
            {
                stsPdv.limparDetalhes();
            }

            Response.Redirect("Tesouraria.aspx");
        }

        protected override bool campoDesabilitado(Control campo)
        {
            throw new NotImplementedException();
        }

        protected override bool campoObrigatorio(Control campo)
        {
            return false;
            //throw new NotImplementedException();
        }



        protected void gridItens_DataBound(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            if (usr.adm())
            {
                GridViewRow footer = gridItens.FooterRow;
                Decimal vTotalFinalizadora = 0;
                Decimal vTotalDigitado = 0;

                foreach (GridViewRow item in gridItens.Rows)
                {
                    Decimal vFinalizadora;
                    TextBox txtRegistrado = (TextBox)item.FindControl("txtRegistrado");
                    Decimal.TryParse(txtRegistrado.Text, out vFinalizadora);
                    txtRegistrado.Text = vFinalizadora.ToString("N2");


                    Decimal vDigitado;
                    TextBox txtDig = (TextBox)item.FindControl("txtValor");
                    Decimal.TryParse(txtDig.Text, out vDigitado);
                    txtDig.Text = vDigitado.ToString("N2");
                    vTotalFinalizadora += vFinalizadora;
                    vTotalDigitado += vDigitado;

                    Label lblDif = (Label)item.FindControl("lblDiferenca");
                    Decimal vDif;
                    Decimal.TryParse(lblDif.Text, out vDif);
                    lblDif.Text = vDif.ToString("N2");
                    if (vDif > 0)
                    {
                        lblDif.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (vDif < 0)
                    {
                        lblDif.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lblDif.ForeColor = System.Drawing.Color.Black;
                    }

                    TextBox txtDep = (TextBox)item.FindControl("txtDeposito");
                    if (int.Parse(item.Cells[0].Text) > 1)
                    {
                        txtDep.Text = "0";
                        txtDep.Visible = false;
                    }

                }

                if (footer != null)
                {


                    Label lblTotalFinalizadora = (Label)footer.FindControl("lblRegistradoTotal");
                    lblTotalFinalizadora.Text = vTotalFinalizadora.ToString("N2");
                    footer.Cells[2].HorizontalAlign = HorizontalAlign.Center;

                    Label lblTotalDigitado = (Label)footer.FindControl("lblValorTotal");
                    lblTotalDigitado.Text = vTotalDigitado.ToString("N2");
                    footer.Cells[3].HorizontalAlign = HorizontalAlign.Center;

                    Label lblTotalDiferenca = (Label)footer.FindControl("lblDiferencaTotal");
                    lblTotalDiferenca.Text = (vTotalDigitado - vTotalFinalizadora).ToString("N2");
                    footer.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                }

            }

        }
        private void salvar()
        {
            try
            {
                User usr = (User)Session["User"];
                carregarObj();
                status_pdvDAO stsPdv = (status_pdvDAO)Session["tesouraria" + urlSessao()];
                stsPdv.Status = "ABERTO";


                stsPdv.salvar(true);
                Session.Remove("tesouraria" + urlSessao());
                Session.Add("tesouraria" + urlSessao(), stsPdv);

                lblerror.Text = "Salvo Com Sucesso";
                lblerror.ForeColor = System.Drawing.Color.Blue;
                carregarOperacoes(usr, ultimaOrdem);


            }
            catch (Exception err)
            {

                lblerror.Text = err.Message;
                lblerror.ForeColor = System.Drawing.Color.Red;
            }

        }

        private void carregarObj()
        {
            User usr = (User)Session["User"];
            status_pdvDAO stsPdv = (status_pdvDAO)Session["tesouraria" + urlSessao()];

            int.TryParse(Request.Params["id"], out stsPdv.Id_Operador);
            int.TryParse(lblcaixa.Text, out stsPdv.Pdv);
            stsPdv.vDiferenca = 0;
            stsPdv.arrItensTesouraria.Clear();
            foreach (GridViewRow row in gridItens.Rows)
            {

                Decimal vDigitado = 0;
                Decimal vReg = 0;
                TextBox txtDigitado = (TextBox)row.FindControl("txtValor");
                TextBox txtRegistrado = (TextBox)row.FindControl("txtRegistrado");
                Decimal.TryParse(txtDigitado.Text, out vDigitado);
                Decimal.TryParse(txtRegistrado.Text, out vReg);

                if ((vDigitado > 0) || (vReg > 0))
                {
                    tesourariaDAO ts = new tesourariaDAO(usr);
                    int.TryParse(row.Cells[0].Text, out ts.FINALIZADORA);
                    ts.id_finalizadora = row.Cells[1].Text;

                    Decimal.TryParse(txtRegistrado.Text, out ts.Total_Sistema);
                    Decimal.TryParse(txtDigitado.Text, out ts.Total_Entregue);
                    stsPdv.addItem(ts);

                }
            }


            Session.Remove("tesouraria" + urlSessao());
            Session.Add("tesouraria" + urlSessao(), stsPdv);
        }

        protected void btnEncerrar_Click(object sender, EventArgs e)
        {
            Label14.Text = "";

            if (emEncerramento )
            {
                return;
            }
            else
            {
                emEncerramento = true;

                Session.Remove("tesourariaEncerrar" + urlSessao());
                Session.Add("tesourariaEncerrar" + urlSessao(), emEncerramento);
            }

            salvar();
            lblerror.Text = "";
            GridViewRow footer = gridItens.FooterRow;
            Label lblTotalDiferenca = (Label)footer.FindControl("lblDiferencaTotal");
            Decimal vDif = 0;
            Decimal.TryParse(lblTotalDiferenca.Text, out vDif);
            status_pdvDAO stsPDv = (status_pdvDAO)Session["tesouraria" + urlSessao()];
            if (vDif > 0)
            {
                Label14.Text = "Existe uma Sobra entre Valor Registrado e o Valor Digitado, Será gerado um Documento com o Numero:" + stsPDv.Data_Abertura.ToString("ddMMyy") + stsPDv.Pdv.ToString().PadLeft(2, '0') + stsPDv.Id_Operador.ToString().PadLeft(2, '0') + "<br>";
            }
            else if (vDif < 0)
            {
                Label14.Text = "Existe uma Quebra entre Valor Registrado e o Valor Digitado, Será gerado um Documento com o Numero:" + stsPDv.Data_Abertura.ToString("ddMMyy") + stsPDv.Pdv.ToString().PadLeft(2, '0') + stsPDv.Id_Operador.ToString().PadLeft(2, '0') + "<br>";
            }
            Label14.Text += "Tem Certeza que gostaria de Fechar o Caixa?";

            modalEncerrar.Show();


        }

        protected void btnConfirmaEncerrar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                User usr = (User)Session["User"];
                status_pdvDAO stsPDv = (status_pdvDAO)Session["tesouraria" + urlSessao()];
                stsPDv.encerrar(usr);
                lblerror.Text = "Salvo Com Sucesso";
                lblerror.ForeColor = System.Drawing.Color.Blue;
                carregarOperacoes(usr, ultimaOrdem);
            }
            catch (Exception err)
            {
                lblerror.Text = err.Message;
            }
            modalEncerrar.Hide();
        }

        protected void btnCancelarEncerrar_Click(object sender, ImageClickEventArgs e)
        {
            modalEncerrar.Hide();
        }


        protected void btnFechar_Click(object sender, ImageClickEventArgs e)
        {
            SqlConnection cnn = Conexao.novaConexao();
            SqlTransaction trans = cnn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {


                status_pdvDAO stsPdv = (status_pdvDAO)Session["tesouraria" + urlSessao()];
                User usr = (User)Session["User"];

                foreach (GridViewRow item in GridLista.Rows)
                {
                    DropDownList ddlFinalizadora = (DropDownList)item.FindControl("ddlFinalizadora");
                    Label lblFinalizadora = (Label)item.FindControl("lblFinalizadora");

                    DropDownList ddlAutorizadora = (DropDownList)item.FindControl("ddlAutorizadora");
                    Label lblAutorizadora = (Label)item.FindControl("lblAutorizadora");

                    DropDownList ddlCartao = (DropDownList)item.FindControl("ddlCartao");
                    Label lblCartao = (Label)item.FindControl("lblCartao");

                    String CodigoAutorizacao = "";
                    TextBox txtCodAutorizacao = (TextBox)item.FindControl("txtCodAutorizacao");
                    CodigoAutorizacao = txtCodAutorizacao.Text;

                    if (ddlFinalizadora.Visible && (!ddlFinalizadora.Text.Equals(lblFinalizadora.Text)
                        || !ddlCartao.Text.Equals(lblCartao.Text)
                        || !ddlAutorizadora.SelectedValue.Equals(lblAutorizadora.Text)
                        ))
                    {
                        if (ddlFinalizadora.Text.Equals(""))
                        {
                            ddlFinalizadora.BackColor = System.Drawing.Color.Red;
                            throw new Exception("Obrigatorio preencher a finalizadora");
                        }
                        else if(ddlCartao.Visible && ddlCartao.Text.Equals(""))
                        {
                            ddlCartao.BackColor = System.Drawing.Color.Red;
                            throw new Exception("Obrigatorio preencher o cartão");
                        }
                        else if (!CodigoAutorizacao.Equals("") && ddlCartao.Text.Equals(""))
                        {
                            ddlCartao.BackColor = System.Drawing.Color.Red;
                            throw new Exception("Código de autorização deve estar relacionado ao cartão.");
                        }
                        else
                        {

                            Decimal vValor = Funcoes.decTry(item.Cells[1].Text);
                            String horaVenda = item.Cells[2].Text;
                            String dtVencimento = Funcoes.dateSql(DateTime.Now);
                            String id_bandeira = "";

                            decimal taxa = 0;
                            int Sequencia = 0;
                            int.TryParse(item.Cells[6].Text, out Sequencia); // Converte o valor para inteiro e grava na varíavel sequencia.
                            SqlDataReader rs = null;
                            try
                            {
                                String sqlCartao = "Select * from cartao where cartao.id_cartao ='" + ddlCartao.SelectedValue + "' and cartao.id_rede='" + ddlAutorizadora.SelectedValue + "'";
                                rs = Conexao.consulta(sqlCartao, null, false);
                                if (rs.Read())
                                {
                                    int corte = Funcoes.intTry(rs["corte"].ToString());
                                    string hora_corte = rs["hora_corte"].ToString();
                                    id_bandeira = rs["id_bandeira"].ToString();
                                    taxa = Funcoes.decTry(rs["taxa"].ToString());
                                    if (corte > 0)
                                    {
                                        dtVencimento = Funcoes.dateSql(Funcoes.proximoDiaSemana(stsPdv.Data_Abertura, horaVenda, corte, hora_corte));
                                    }
                                    else
                                    {
                                        dtVencimento = "master.dbo.F_BR_PROX_DIA_UTIL(dateadd(day,Isnull(" + rs["dias"].ToString() + ",0),emissao))";
                                    }
                                }

                                String sqlDetalhe = "update tesouraria_detalhes " +
                                               " set finalizadora = '" + ddlFinalizadora.SelectedItem.Value + "' , " +
                                                   " ID_FINALIZADORA = '" + ddlFinalizadora.SelectedItem.Text + "'," +
                                                   " id_cartao ='" + ddlCartao.SelectedItem.Value + "'," +
                                                   " id_bandeira = '" + id_bandeira + "'," +
                                                   " rede_cartao = '" + ddlAutorizadora.SelectedItem.Value + "'," +
                                                   " vencimento = " + dtVencimento + " ," +
                                                   " taxa =((" + Funcoes.decimalPonto(taxa.ToString()) + "/100)* total), " +
                                                   " Autorizacao = '" + CodigoAutorizacao + "'" +
                                               " where cupom ='" + item.Cells[0].Text + "'" +
                                                        " and pdv=" + stsPdv.Pdv +
                                                        " and operador=" + stsPdv.Id_Operador +
                                                        " and emissao ='" + stsPdv.Data_Abertura.ToString("yyyyMMdd") + "' " +
                                                        " and tesouraria_detalhes.filial= '" + usr.getFilial() + "'" +
                                                        " and total = " + vValor.ToString().Replace(",", ".") +
                                                        " and id_fechamento =" + stsPdv.id_fechamento.ToString()+
                                                        " and Sequencia = " + Sequencia.ToString();

                                Conexao.executarSql(sqlDetalhe, cnn, trans);


                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            finally
                            {
                                if (rs != null)
                                    rs.Close();
                            }

                        }
                    }
                }
                trans.Commit();

                salvar();

                carregarOperacoes(usr, ultimaOrdem);

            }
            catch (Exception err)
            {
                trans.Rollback();
                lblErroPesquisa.Text = err.Message;
                modalPnFundo.Show();
            }
            finally
            {
                if (cnn != null)
                    cnn.Close();
            }


        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
        }

        protected void GridLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            User usr = (User)Session["User"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //DropDownList ddl = (DropDownList)e.Row.FindControl("ddlFinalizadora");
                //Conexao.preencherDDL1Branco(ddl, "Select finalizadora from Finalizadora", "finalizadora", "finalizadora", usr);
                //Label lblFinalizadora = (Label)e.Row.FindControl("lblFinalizadora");
                //ddl.Text = lblFinalizadora.Text;

                //DropDownList ddlCartao = (DropDownList)e.Row.FindControl("ddlCartao");
                //Conexao.preencherDDL1Branco(ddlCartao, "Select id_cartao  from Cartao inner join Finalizadora on Cartao.nro_Finalizadora = Finalizadora.Nro_Finalizadora where Finalizadora = '" + lblFinalizadora.Text+"'", "id_cartao", "id_cartao", usr);

                Label lblCartao = (Label)e.Row.FindControl("lblAutorizacao");
                //ddlCartao.Text = lblCartao.Text;

                if (!lblCartao.Text.Equals("0") && !lblCartao.Text.Trim().Equals(""))
                {
                    ImageButton btn = (ImageButton)e.Row.FindControl("imgBtnCartao");
                    btn.Visible = false;
                }
            }
        }

        protected void imgEditarFinalizadora_Click(object sender, ImageClickEventArgs e)
        {

            carregarObj();


            ImageButton btn = (ImageButton)sender;
            GridViewRow linha = (GridViewRow)btn.NamingContainer;

            lblFiltroFinalizadora.Text = linha.Cells[0].Text;
            exibeDetalhes();



        }
        private void exibeDetalhes()
        {
            User usr = (User)Session["User"];

            String id_op = Request.Params["id"];

            status_pdvDAO stsPdv = (status_pdvDAO)Session["tesouraria" + urlSessao()];

            String sqlFinalizadora = " Select  cupom, total,lf.finalizadora,lf.id_finalizadora, lf.id_cartao,lf.autorizacao,lf.hora_venda as Hora,cartao.taxa,cartao.dias,lf.rede_cartao,a.Descricao Autorizadora    " +
                                        ", Sequencia " +
                                      " from tesouraria_detalhes lf  " +
                                           " left join  Cartao on cartao.nro_Finalizadora=lf.finalizadora and  convert( int ,lf.rede_cartao)= convert(int,Cartao.id_rede) and convert(int,lf.id_Bandeira)= convert(int ,Cartao.id_bandeira)" +
                                           " left join Autorizadora as a on lf.rede_cartao = a.id " +
                                        " where lf.filial='" + usr.getFilial() + "' and  lf.Emissao >='" + stsPdv.Data_Abertura.ToString("yyyyMMdd") + "' and lf.pdv=" + lblcaixa.Text + " and lf.operador = " + id_op + " and finalizadora = " + lblFiltroFinalizadora.Text +
                                        " and (id_fechamento =" + stsPdv.id_fechamento + ")" +
                                        (txtValorFiltro.Text.Trim().Equals("") ? "" : " and  total=" + txtValorFiltro.Text.Replace(".", "").Replace(",", ".")) +
                                        " order by cartao.id_cartao ";

            GridLista.DataSource = Conexao.GetTable(sqlFinalizadora, usr, false);
            GridLista.DataBind();

            if (GridLista.Rows[0].Cells[0].Text.Equals("------"))
            {
                lblTotalReg.Text = "0";
            }
            else
            {
                lblTotalReg.Text = GridLista.Rows.Count.ToString();
            }

            Decimal vlrTotalReg = 0;
            foreach (GridViewRow linha in GridLista.Rows)
            {
                Decimal vlrLInha = 0;
                Decimal.TryParse(linha.Cells[1].Text, out vlrLInha);
                vlrTotalReg += vlrLInha;

            }
            lblValorReg.Text = vlrTotalReg.ToString("N2");
            lblErroPesquisa.Text = "";
            modalPnFundo.Show();
        }

        protected void imgBtnCartao_Click(object sender, ImageClickEventArgs e)
        {
            User usr = (User)Session["User"];
            ImageButton btn = (ImageButton)sender;
            GridViewRow linha = (GridViewRow)btn.NamingContainer;
            Label lblFinalizadora = (Label)linha.FindControl("lblFinalizadora");
            Label lblautorizadora = (Label)linha.FindControl("lblAutorizadora");
            TextBox txtCodAutorizacao = (TextBox)linha.FindControl("txtCodAutorizacao");
            txtCodAutorizacao.Enabled = true;

            carregarDadosCartao(linha, lblFinalizadora.Text, lblautorizadora.Text);

            lblFinalizadora.Visible = false;
            lblautorizadora.Visible = false;
            btn.Visible = false;


        }

        private void carregarDadosCartao(GridViewRow linha, string finalizadora, String autorizadora)
        {
            User usr = (User)Session["User"];
            DropDownList ddlFinalizadora = (DropDownList)linha.FindControl("ddlFinalizadora");
            if (ddlFinalizadora.Items.Count == 0)
                Conexao.preencherDDL1Branco(ddlFinalizadora, "Select finalizadora,Nro_finalizadora from Finalizadora", "finalizadora", "Nro_finalizadora", usr);

            Funcoes.SetDDLText(ddlFinalizadora, finalizadora);
            ddlFinalizadora.Visible = true;
            String sqlCartao = "Select id_cartao  " +
                               "from Cartao inner join Finalizadora on Cartao.nro_Finalizadora = Finalizadora.Nro_Finalizadora " +
                               "where cartao.nro_finalizadora = '" + ddlFinalizadora.SelectedItem.Value + "'";

            DropDownList ddlAutorizadora = (DropDownList)linha.FindControl("ddlAutorizadora");

            Conexao.preencherDDL1Branco(ddlAutorizadora, "Select * from Autorizadora", "Descricao", "id", null);
            if (autorizadora.Equals(""))
            {
                autorizadora = Conexao.retornaUmValor("Select descricao from autorizadora where padrao =1", null);
            }
            Funcoes.SetDDLText(ddlAutorizadora, autorizadora);
            ddlAutorizadora.Visible = true;

            if (!autorizadora.Equals(""))
                sqlCartao += " and id_rede=" + ddlAutorizadora.SelectedItem.Value;

            DropDownList ddlCartao = (DropDownList)linha.FindControl("ddlCartao");
            Conexao.preencherDDL1Branco(ddlCartao, sqlCartao + " order by id_cartao ", "id_cartao", "id_cartao", null);
            Label lblCartao = (Label)linha.FindControl("lblCartao");

            if (ddlCartao.Items.Count > 1)
            {
                Funcoes.SetDDLValue(ddlCartao, lblCartao.Text);
                ddlCartao.Visible = true;
                lblCartao.Visible = false;
                ddlAutorizadora.Visible = true;

            }
            else
            {
                ddlAutorizadora.Visible = false;
                ddlAutorizadora.Text = "";
                ddlCartao.Visible = false;
                lblCartao.Visible = false;

            }

            modalPnFundo.Show();
            ddlFinalizadora.Focus();
        }

        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeDetalhes();
        }
        //
        protected void imgBtnReprocessa_Click(object sender, ImageClickEventArgs e)
        {
            modalProcessamento.Show();
        }

        //imgBtnConfirmaProcessar_Click
        protected void imgBtnConfirmaProcessar_Click(object sender, ImageClickEventArgs e)
        {
            atualizarRegistros();
            modalProcessamento.Hide();

        }

        private void atualizarRegistros()
        {

            status_pdvDAO stsPdv = (status_pdvDAO)Session["tesouraria" + urlSessao()];
            User usr = (User)Session["User"];

            stsPdv.limparDetalhes();
            stsPdv.inserirDetalhes();
            Session.Remove("tesouraria" + urlSessao());
            Session.Add("tesouraria" + urlSessao(), stsPdv);
            carregarOperacoes(usr, "");
        }

        protected void imgBtnCancelaProcessar_Click(object sender, ImageClickEventArgs e)
        {
            modalProcessamento.Hide();
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string[] GetCartoes(string id_cartao)
        {
            String sql = "Select id_cartao  from Cartao inner join Finalizadora on Cartao.nro_Finalizadora = Finalizadora.Nro_Finalizadora where Finalizadora = '" + id_cartao.Trim() + "'";
            string[] str = Conexao.retornaArray(sql, 100);
            return str;
        }

        protected void ddlFinalizadora_SelectedIndexChanged(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            DropDownList ddlFinalizadora = (DropDownList)sender;
            GridViewRow linha = (GridViewRow)ddlFinalizadora.Parent.Parent;
            DropDownList ddlAutorizadora = (DropDownList)linha.FindControl("ddlAutorizadora");
            carregarDadosCartao(linha, ddlFinalizadora.SelectedItem.Text, ddlAutorizadora.SelectedItem.Text);
        }



        protected void gridItens_Sorting(object sender, GridViewSortEventArgs e)
        {
            User usr = (User)Session["User"];
            carregarOperacoes(usr, e.SortExpression);
        }

        protected void ddlAutorizadora_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlAutorizadora = (DropDownList)sender;
            GridViewRow linha = (GridViewRow)ddlAutorizadora.Parent.Parent;
            DropDownList ddlFinalizadora = (DropDownList)linha.FindControl("ddlFinalizadora");
            carregarDadosCartao(linha, ddlFinalizadora.SelectedItem.Text, ddlAutorizadora.SelectedItem.Text);
        }

        protected void ddlCartao_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DropDownList ddlCartao = (DropDownList)sender;
            //GridViewRow linha = (GridViewRow)ddlCartao.Parent.Parent;
            //DropDownList ddlFinalizadora = (DropDownList)linha.FindControl("ddlFinalizadora");
            //DropDownList ddlAutorizadora = (DropDownList)linha.FindControl("ddlAutorizadora");
            //string idFinalizadora = Conexao.retornaUmValor("Select nro_Finalizadora from Cartao where id_cartao = '" + ddlCartao.SelectedItem.Text + "' AND id_Rede =" + ddlAutorizadora.SelectedItem.Value,null);
            //Funcoes.SetDDLValue(ddlFinalizadora, idFinalizadora);

            //modalPnFundo.Show();
            //ddlFinalizadora.Focus();
        }

        protected void imgBtnConfirmaEstorno_Click(object sender, ImageClickEventArgs e)
        {
            int valorAdicional = 0;
            int.TryParse(Funcoes.valorParametro("SOMAR_A_SENHA", null), out valorAdicional);

            //Pega os dados de hora minuto e dia atual
            string horaDia = DateTime.Now.ToString("HHmmdd");
            int valorHoraDia = 0;
            //Soma todos os digitos e armazena em valoHoraDia
            for (int i = 0; i < horaDia.Length; i++)
            {
                valorHoraDia += int.Parse(horaDia.Substring(i, 1));
            }
            //Soma o valor adicional do parametro ao cálculo dos digitos da hora
            valorHoraDia += valorAdicional;
            //Guarda o valor digitado e compara com o valor calculado.
            int valorHoraDiaDigitado = 0;
            int.TryParse(txtSAutorizacao.Text, out valorHoraDiaDigitado);

            if (valorHoraDia != valorHoraDiaDigitado)
            {
                lblErroEstorno.Text = "Senha digitada não confere.";
                lblErroEstorno.ForeColor = System.Drawing.Color.Red;
                throw new Exception("Senha digitada não confere.");
            }
            else
            {
                modalSenha.Hide();
                estornar();
                BtnEstorno.Visible = false;
            }
        }

        protected void imgBtnCancelaEstorno_Click(object sender, ImageClickEventArgs e)
        {
            modalSenha.Hide();
        }

        protected void imgBtnEstorno_Click(object sender, ImageClickEventArgs e)
        {
            modalSenha.Show();
        }

        private void estornar()
        {
            try
            {
                status_pdvDAO stsPdv = (status_pdvDAO)Session["tesouraria" + urlSessao()];
                User usr = (User)Session["User"];
                stsPdv.estornarFechamento(usr);
            }
            catch (Exception err)
            {
                lblerror.Text = err.Message;
                lblerror.ForeColor = System.Drawing.Color.Red;
            }
        }
    }

}