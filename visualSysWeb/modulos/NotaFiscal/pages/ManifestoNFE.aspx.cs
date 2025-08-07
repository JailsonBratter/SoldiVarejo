using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;
using visualSysWeb.modulos.NotaFiscal.code;
using visualSysWeb.modulos.NotaFiscal.dao;

namespace visualSysWeb.modulos.NotaFiscal
{
    public partial class ManifestoNFE : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {


                User usr = (User)Session["User"];
                divImportarNotasPasta.Visible = Funcoes.valorParametro("MANIFESTO_IMPORTAR", usr).ToUpper().Equals("TRUE");
                if (usr == null)
                    return;

                ddlData.Text = "EMISSAO";
                txtDe.Text = "01/" + DateTime.Now.ToString("MM/yyyy");
                txtAte.Text = DateTime.Now.ToString("dd/MM/yyyy");
                xmlNFE nfeXml = new xmlNFE(usr);
                Session.Remove("xml" + urlSessao());
                Session.Add("xml" + urlSessao(), nfeXml);
                carregarNotas();
                atualizarCalendario();
                EnabledControls(divFiltro, true);
            }
            sopesquisa(pnBtn);

            //Campo desabilitado para não interferir no SOLDINFe
            imgBtnConsultarNotas.Enabled = false;
            imgBtnConsultarNotas.Visible = false;

        }

        private void carregarNotas()
        {
            try
            {


                User usr = (User)Session["User"];
                DateTime de = Funcoes.dtTry(txtDe.Text);
                DateTime ate = Funcoes.dtTry(txtAte.Text);
                String tipoData = ddlData.Text;
                List<NfManifestoDAO> lista = NfManifestoDAO.notasManifestos(txtChave.Text, txtRazaoSocial.Text, tipoData, de, ate, ddlStatus.Text,ddlLancado.Text, usr);




                Session.Remove("lista" + urlSessao());
                Session.Add("lista" + urlSessao(), lista);

                gridNfs.DataSource = lista;
                gridNfs.DataBind();

                int qtde = lista.Count;
                if (qtde == 1)
                {
                    if (gridNfs.Rows[0].Cells[1].Text.Replace("&nbsp;", "").Trim().Equals(""))
                        qtde = 0;
                }



                lblRegistros.Text = qtde + " ENCONTRADO(S) ";


                exibirBtnBaixa();

                GridViewRow row = (GridViewRow)Session["rowItens"];
                if(row != null)
                {
                    int index = row.RowIndex;
                    gridNfs.Rows[index].RowState = DataControlRowState.Selected;
                    gridNfs.Rows[index].Cells[0].Focus();

                }


                //foreach (GridViewRow row in gridNfs.Rows)
                //{
                //    Button button1 = (Button)row.Cells[6].Controls[0];

                //    if (row.Cells[5].Text.Equals("SIM"))
                //    {
                //        button1.Text = "Download";
                //        button1.CssClass = "btnDownload";
                //    }
                //    else
                //    {
                //        button1.Text = "Manifestar";

                //        button1.CssClass = "btnManifestar";
                //    }

                //}
            }
            catch (Exception err)
            {

                showMessage(err.Message, true);
            }

        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            carregarNotas();
            TabContainer1.ActiveTabIndex = 1;

        }
        protected override void btnEditar_Click(object sender, EventArgs e) { }
        protected override void btnExcluir_Click(object sender, EventArgs e) { }
        protected override void btnConfirmar_Click(object sender, EventArgs e) { }
        protected override void btnCancelar_Click(object sender, EventArgs e) { }
        //Sinto Muito Me Perdoe Agradeço Eu Te Amo.


        protected override bool campoObrigatorio(Control campo)
        {
            return false;
        }

        protected override bool campoDesabilitado(Control campo)
        {
            return false;
        }

        protected void gridNfs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //GridViewRow row = (GridViewRow)sender;
            //if (row.Cells[7].Text.Equals("SIM"))
            //{

            //}


        }

        protected void imgBtnConsultarNotas_Click(object sender, ImageClickEventArgs e)
        {
            Session.Remove("erroXml" + urlSessao());
            Session.Remove("retornoXml" + urlSessao());
            Session.Remove("itensManifestar" + urlSessao());
            consultaNotas();
        }




        protected void consultaNotas()
        {
            try
            {

                User usr = (User)Session["User"];
                NfManifestoDAO nota = new NfManifestoDAO(usr);

                xmlNFE nfeXml = (xmlNFE)Session["xml" + urlSessao()];
                if (nfeXml.permitirNovaConsultaManifesto())
                {
                    nota.NSU = nfeXml.ultimoNSU();

                    System.Threading.Thread th = new System.Threading.Thread(buscarRespostaXML);
                    th.Start();
                    TimerXml.Interval = 3000;
                    TimerXml.Enabled = true;
                    nfeXml.consultaNotasManifestadas(nota);
                    lblResponstaXml.Text = "AGUARDE !";
                    btnRespostaXml.Enabled = false;
                    modalXml.Show();
                }
            }
            catch (Exception err)
            {
                if(err.Message.Contains("Nenhum documento para baixar"))
                {
                    lblResponstaXml.Text = "AGUARDE !";
                    btnRespostaXml.Enabled = false;
                    modalXml.Show();
                    TimerXml.Interval = 3000;
                    TimerXml.Enabled = true;
                    Session.Add("retornoXml" + urlSessao(), err.Message);

                }
                else
                {
                    showMessage(err.Message, true);
                }
                

            }
        }

     
        protected void btnOkError_Click(object sender, EventArgs e)
        {
            if (lblErroPanel.Text.Equals("Informe a chave da nota Fiscal para fazer a consulta"))
            {
                txtChave.BackColor = System.Drawing.Color.Red;
            }

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
        protected void btnRespostaXml_Click(object sender, EventArgs e)
        {

            if (lblResponstaXml.Text.Contains("FIM MANIFESTADO"))
            {
                lblResponstaXml.Text = "AGUARDE !";
                btnRespostaXml.Enabled = false;
                Session.Remove("erroXml" + urlSessao());
                Session.Remove("retornoXml" + urlSessao());
                Session.Remove("itensManifestar" + urlSessao());
                consultaNotas();
            }
            else
            {
                avancaMes(0);
                carregarNotas();
                modalXml.Hide();
            }

        }

        protected void TimerXml_Tick(object sender, EventArgs e)
        {
            String strErro = (String)Session["erroXml" + urlSessao()];
            String strRetorno = (String)Session["retornoXml" + urlSessao()];
            String strTentativas = (String)Session["tentativas" + urlSessao()];

            btnRespostaXml.Enabled = true;
            if (strErro != null)
            {
                lblResponstaXml.Text = strErro;
                lblResponstaXml.ForeColor = System.Drawing.Color.Red;
                TimerXml.Enabled = false;
            }
            else if (strRetorno != null)
            {
                lblResponstaXml.Text = strRetorno;
                lblResponstaXml.ForeColor = System.Drawing.Color.Blue;
                if (strRetorno.Equals("FIM MANIFESTADO"))
                {
                    List<NfManifestoDAO> itens = (List<NfManifestoDAO>)Session["itensManifestar" + urlSessao()];
                    foreach (NfManifestoDAO item in itens)
                    {

                        lblResponstaXml.Text += "<br>Razao Social:" + item.RazaoSocial + " <br> Vlr:" + item.vNF + "<br> Retorno:" + item.retornoManifestacao;
                    }


                }
                if (!strRetorno.Contains("Nova Consulta"))
                {
                    TimerXml.Enabled = false;
                }



            }
            else
            {
                lblResponstaXml.Text = "AGUARDE CONSULTADO SEFAZ.<br> " + strTentativas.ToString() + " Tentativa(s)";
                lblResponstaXml.ForeColor = System.Drawing.Color.Green;
                btnRespostaXml.Enabled = false;

            }
            modalXml.Show();
        }

        protected void buscarRespostaXML()
        {
            try
            {
                xmlNFE nfeXml = (xmlNFE)Session["xml" + urlSessao()];
                String strRetorno = "";
                for (int i = 0; i < 20; i++)
                {
                    Session.Remove("tentativas" + urlSessao());
                    Session.Add("tentativas" + urlSessao(), " NSU:" + Funcoes.intTry(nfeXml.ultimoNSU()) + " N." + (i + 1).ToString());
                    System.Threading.Thread.Sleep(5000);

                    List<NfManifestoDAO> itens = (List<NfManifestoDAO>)Session["itensManifestar" + urlSessao()];
                    String operacao = (String)Session["operacaoManifestar" + urlSessao()];
                    if (itens != null)
                    {
                        List<NfManifestoDAO> itensConsultar = new List<NfManifestoDAO>();
                        foreach (NfManifestoDAO nf in itens)
                        {
                            if (nf.retornoManifestacao.Equals("") || nf.retornoManifestacao.Equals("SEM RETORNO"))
                            {
                                itensConsultar.Add(nf);
                            }
                        }
                        if (itensConsultar.Count > 0)
                            nfeXml.retornoEventoManifestados(itensConsultar, operacao);
                        else
                        {
                            Session.Remove("itensManifestar" + urlSessao());
                            Session.Add("itensManifestar" + urlSessao(), itens);

                            strRetorno = "FIM MANIFESTADO";
                            Session.Remove("retornoXml" + urlSessao());
                            Session.Add("retornoXml" + urlSessao(), strRetorno);
                            break;
                        }



                    }
                    else
                    {
                        strRetorno = nfeXml.retornoConsultaNotaManifestada();

                        if (strRetorno.Trim().Length > 0)
                        {

                            if (!nfeXml.strMaxNSU.Equals(nfeXml.strUltNSU))
                            {
                                strRetorno += " Nova Consulta ULT NSU: " + nfeXml.strUltNSU;
                                consultaNotas();
                                break;
                            }
                            Session.Remove("retornoXml" + urlSessao());
                            Session.Add("retornoXml" + urlSessao(), strRetorno);
                            break;
                        }


                    }
                }
                if (strRetorno.Length == 0)
                {
                    Session.Remove("erroXml" + urlSessao());
                    Session.Add("erroXml" + urlSessao(), "SEM RESPOSTA!");
                }
            }
            catch (Exception err)
            {
                Session.Remove("erroXml" + urlSessao());
                Session.Add("erroXml" + urlSessao(), err.Message);
            }
        }

        protected void atualizarCalendario()
        {
            String dtAtual = (String)Session["dtAtual" + urlSessao()];
            DateTime DataAtual = new DateTime();
            if (dtAtual == null)
                DataAtual = Funcoes.dtTry("01/" + DateTime.Now.ToString("MM/yyyy"));
            else
                DataAtual = Funcoes.dtTry(dtAtual);

            Session.Remove("dtAtual" + urlSessao());
            Session.Add("dtAtual" + urlSessao(), DataAtual.ToString());

            lblMesAtual.Text = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(DataAtual.Month);
            lblMesAtual.Text += "/" + DataAtual.Year;
            int primeiroDiaSemana = (int)DataAtual.DayOfWeek;
            if (primeiroDiaSemana == 7)
                primeiroDiaSemana = 1;
            else
                primeiroDiaSemana++;

            DateTime dtUltimoDiaMes = DataAtual.AddMonths(1).AddDays(-1);

            int ultimoDiaMes = (int)dtUltimoDiaMes.DayOfWeek;
            if (ultimoDiaMes == 7)
                ultimoDiaMes = 1;
            else
                ultimoDiaMes++;



            for (int s = 1; s <= 6; s++)
            {
                for (int d = 1; d <= 7; d++)
                {

                    if (s < 06 || d <= 2)
                    {

                        Label lblDia = (Label)divResumo.FindControl("lblS" + s + "D" + d);
                        HtmlGenericControl div = (HtmlGenericControl)divResumo.FindControl("divS" + s + "D" + d);
                        //div.Attributes.Remove("class");
                        div.Attributes.Add("class", "divDia");


                        Button btnTodasAsNotas = (Button)divResumo.FindControl("btnTodasNotasS" + s + "D" + d);
                        Button btnConfirmaOperacao = (Button)divResumo.FindControl("btnConfirmadoOperacaoS" + s + "D" + d);
                        Button btnCienciaOperacao = (Button)divResumo.FindControl("btnCienciaOperacaoS" + s + "D" + d);
                        Button btnDesconhecimentoOperacao = (Button)divResumo.FindControl("btnDesconhecimentoOperacaoS" + s + "D" + d);
                        Button btnOperacaoNaoRealizada = (Button)divResumo.FindControl("btnOperacaoNaoRealizadaS" + s + "D" + d);
                        btnConfirmaOperacao.Visible = true;
                        btnCienciaOperacao.Visible = true;
                        btnDesconhecimentoOperacao.Visible = true;
                        btnOperacaoNaoRealizada.Visible = true;
                        btnTodasAsNotas.Visible = true;


                        if ((s == 1 && d < primeiroDiaSemana) || DataAtual > dtUltimoDiaMes)
                        {
                            lblDia.Text = "";
                            btnTodasAsNotas.Visible = false;
                            btnConfirmaOperacao.Visible = false;
                            btnCienciaOperacao.Visible = false;
                            btnDesconhecimentoOperacao.Visible = false;
                            btnOperacaoNaoRealizada.Visible = false;
                            div.Attributes.Add("class", "divDia divDiaSemFundo");
                        }
                        else
                        {
                            lblDia.Text = DataAtual.Day.ToString();


                            int qtdeTodas = qtdeNotas(DataAtual, "TODOS");

                            if (qtdeTodas > 0)
                            {

                                btnTodasAsNotas.Text = qtdeTodas.ToString() + " Notas";

                                int qtdeOper = qtdeNotas(DataAtual, "CONFIRMADO OPERACAO");
                                if (qtdeOper > 0)
                                {
                                    btnConfirmaOperacao.Text = qtdeOper.ToString() + " CONF. OPERACAO";
                                }
                                else
                                {
                                    btnConfirmaOperacao.Visible = false;
                                }

                                int qtdeCiencia = qtdeNotas(DataAtual, "CIENCIA OPERACAO");
                                if (qtdeCiencia > 0)
                                {
                                    btnCienciaOperacao.Text = qtdeCiencia.ToString() + " CIENCIA OPERACAO";
                                }
                                else
                                {
                                    btnCienciaOperacao.Visible = false;
                                }

                                int qtdeDesc = qtdeNotas(DataAtual, "DESCONHECIMENTO");
                                if (qtdeDesc > 0)
                                {
                                    btnDesconhecimentoOperacao.Text = qtdeDesc.ToString() + " DESCONHECIMENTO";
                                }
                                else
                                {
                                    btnDesconhecimentoOperacao.Visible = false;
                                }

                                int qtdeOpRealizada = qtdeNotas(DataAtual, "OPERACAO REALIZADA");
                                if (qtdeOpRealizada > 0)
                                {
                                    btnOperacaoNaoRealizada.Text = qtdeOpRealizada.ToString() + " OPERACAO REALIZADA";
                                }
                                else
                                {
                                    btnOperacaoNaoRealizada.Visible = false;
                                }


                            }
                            else
                            {
                                btnTodasAsNotas.Visible = false;
                                btnConfirmaOperacao.Visible = false;
                                btnCienciaOperacao.Visible = false;
                                btnDesconhecimentoOperacao.Visible = false;
                                btnOperacaoNaoRealizada.Visible = false;

                            }

                            DataAtual = DataAtual.AddDays(1);

                        }

                    }

                }
            }


        }

        protected void btnProximoMes_Click(object sender, EventArgs e)
        {
            avancaMes(1);
        }

        protected void BtnMesAnterior_Click(object sender, EventArgs e)
        {
            avancaMes(-1);

        }

        protected void avancaMes(int mes)
        {
            String dtAtual = (String)Session["dtAtual" + urlSessao()];
            DateTime DataAtual = new DateTime();
            if (dtAtual == null)
                DataAtual = Funcoes.dtTry("01/" + DateTime.Now.ToString("MM/yyyy"));
            else
                DataAtual = Funcoes.dtTry(dtAtual);


            DataAtual = DataAtual.AddMonths(mes);

            txtDe.Text = DataAtual.ToString("dd/MM/yyyy");

            DateTime dtUltimoDiaMes = DataAtual.AddMonths(1).AddDays(-1);
            txtAte.Text = dtUltimoDiaMes.ToString("dd/MM/yyyy");

            ddlStatus.Text = "TODOS";


            Session.Remove("dtAtual" + urlSessao());
            Session.Add("dtAtual" + urlSessao(), DataAtual.ToString());
            carregarNotas();
            atualizarCalendario();
        }
        private int qtdeNotas(DateTime data, String status)
        {
            int qtde = 0;
            List<NfManifestoDAO> lista = (List<NfManifestoDAO>)Session["lista" + urlSessao()];
            foreach (NfManifestoDAO item in lista)
            {
                if (item.Emissao.Equals(data) && (status.Equals("TODOS") || item.Status.Equals(status)))
                {
                    qtde++;
                }
            }
            return qtde;
        }
        protected void chkSeleciona_CheckedChanged(object sender, EventArgs e)
        {

            foreach (GridViewRow item in gridNfs.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                if (chk != null)
                {

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
            btnConfirmacaoOperacaoManifestar.Visible = false;
            btnCienciaOperacaoManifestar.Visible = false;
            btnDesconhecimentoOperacaoManifestar.Visible = false;
            btnOperacaoNaoRealizadaManifestar.Visible = false;
            btnBaixarXmls.Visible = false;

            for (int i = 0; gridNfs.Rows.Count > i; i++)
            {
                Button btnItens = (Button)gridNfs.Rows[i].FindControl("btnItens");
                if (gridNfs.Rows[i].Cells[7].Text.Equals("SIM"))
                {
                    btnItens.Visible = true;

                }
                else
                {
                    btnItens.Visible = false;

                }

                if (gridNfs.Rows[i].Cells[8].Text.Equals("SIM"))
                {
                    gridNfs.Rows[i].ForeColor = System.Drawing.Color.Green;
                }
                CheckBox chk = (CheckBox)gridNfs.Rows[i].FindControl("chkSelecionaItem");

                if (chk != null)
                {
                    if (chk.Checked)
                    {


                        if (gridNfs.DataKeys[i][0].ToString().Equals("CONFIRMADO OPERACAO"))
                        {
                            btnBaixarXmls.Visible = true;

                        }
                        else if (gridNfs.DataKeys[i][0].ToString().Equals("CIENCIA OPERACAO"))
                        {
                            btnConfirmacaoOperacaoManifestar.Visible = true;
                            btnOperacaoNaoRealizadaManifestar.Visible = true;
                            btnDesconhecimentoOperacaoManifestar.Visible = true;
                            btnBaixarXmls.Visible = true;
                        }
                        else if (gridNfs.DataKeys[i][0].ToString().Equals("NAO MANIFESTADO"))
                        {
                            btnConfirmacaoOperacaoManifestar.Visible = true;
                            btnCienciaOperacaoManifestar.Visible = true;
                            btnDesconhecimentoOperacaoManifestar.Visible = true;
                            btnOperacaoNaoRealizadaManifestar.Visible = true;

                        }

                    }
                }
            }
        }

        protected void btnDiaStatus_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            String dtAtual = (String)Session["dtAtual" + urlSessao()];
            DateTime Mes = Funcoes.dtTry(dtAtual);

            Label lbldia = null;
            String nDia = "";
            if (btn.ID.Contains("btnTodasNotas"))
            {
                nDia = btn.ID.Replace("btnTodasNotas", "");
                ddlStatus.Text = "TODOS";
            }
            else if (btn.ID.Contains("btnConfirmadoOperacao"))
            {
                nDia = btn.ID.Replace("btnConfirmadoOperacao", "");
                ddlStatus.Text = "CONFIRMADO OPERACAO";
            }
            else if (btn.ID.Contains("btnCienciaOperacao"))
            {
                nDia = btn.ID.Replace("btnCienciaOperacao", "");
                ddlStatus.Text = "CIENCIA OPERACAO";
            }
            else if (btn.ID.Contains("btnDesconhecimentoOperacao"))
            {
                nDia = btn.ID.Replace("btnDesconhecimentoOperacao", "");
                ddlStatus.Text = "DESCONHECIMENTO DA OPERACAOO";
            }
            else if (btn.ID.Contains("btnOperacaoNaoRealizada"))
            {
                nDia = btn.ID.Replace("btnOperacaoNaoRealizada", "");
                ddlStatus.Text = "OPERACAO NAO REALIZADA";
            }

            lbldia = (Label)divResumo.FindControl("lbl" + nDia);
            DateTime dia = Funcoes.dtTry(lbldia.Text + "/" + Mes.ToString("MM/yyyy"));
            txtDe.Text = dia.ToString("dd/MM/yyyy");
            txtAte.Text = txtDe.Text;
            carregarNotas();
            TabContainer1.ActiveTabIndex = 1;

        }


        protected void btnCienciaOperacaoManifestar_Click(object sender, EventArgs e)
        {
            exibirConfirmacao("CIENCIA OPERACAO");
        }

        protected void btnConfirmacaoOperacaoManifestar_Click(object sender, EventArgs e)
        {
            exibirConfirmacao("CONFIRMACAO OPERACAO");
        }

        protected void btnDesconhecimentoOperacaoManifestar_Click(object sender, EventArgs e)
        {
            exibirConfirmacao("DESCONHECIMENTO OPERACAO");
        }

        protected void btnOperacaoNaoRealizadaManifestar_Click(object sender, EventArgs e)
        {
            exibirConfirmacao("OPERACAO NAO REALIZADA");
        }

        private void exibirConfirmacao(String operacao)
        {
            List<NfManifestoDAO> novaLista = manifestar(operacao);

            Session.Remove("itensManifestar" + urlSessao());
            Session.Add("itensManifestar" + urlSessao(), novaLista);
            gridNFManifestar.DataSource = novaLista;
            gridNFManifestar.DataBind();
            lblConfirmaOperacao.Text = operacao;
            modalConfirmarManifestar.Show();

        }
        protected List<NfManifestoDAO> manifestar(String operacao)
        {
            List<NfManifestoDAO> novaLista = new List<NfManifestoDAO>();
            List<NfManifestoDAO> lista = (List<NfManifestoDAO>)Session["lista" + urlSessao()];
            foreach (GridViewRow row in gridNfs.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelecionaItem");
                if (chk.Checked)
                {

                    NfManifestoDAO item = lista[row.RowIndex];
                    if ((operacao.Equals("CIENCIA OPERACAO") || operacao.Equals("DESCONHECIMENTO OPERACAO"))
                        && (item.Status.Equals("NAO MANIFESTADO") || item.Status.Equals("CIENCIA OPERACAO"))
                        )
                    {
                        novaLista.Add(item);
                    }
                    else if ((operacao.Equals("CONFIRMACAO OPERACAO") || operacao.Equals("OPERACAO NAO REALIZADA"))
                        && (item.Status.Equals("NAO MANIFESTADO") || item.Status.Equals("CIENCIA OPERACAO"))
                        )
                    {
                        novaLista.Add(item);
                    }


                }
            }
            return novaLista;
        }

        protected void btnConfirmaOperacao_Click(object sender, EventArgs e)
        {
            Session.Remove("retornoXml" + urlSessao());
            Session.Remove("operacaoManifestar" + urlSessao());
            Session.Add("operacaoManifestar" + urlSessao(), lblConfirmaOperacao.Text);
            manifestarOpercao(lblConfirmaOperacao.Text);
            modalConfirmarManifestar.Hide();

        }

        protected void btnCancelaOperacao_Click(object sender, EventArgs e)
        {
            modalConfirmarManifestar.Hide();
        }

        private void manifestarOpercao(String operacao)
        {
            xmlNFE nfeXml = (xmlNFE)Session["xml" + urlSessao()];
            List<NfManifestoDAO> item = (List<NfManifestoDAO>)Session["itensManifestar" + urlSessao()];
            nfeXml.manifestarNfe(item, operacao);
            Session.Remove("retornoXml" + urlSessao());

            Session.Remove("erroXml" + urlSessao());

            System.Threading.Thread th = new System.Threading.Thread(buscarRespostaXML);
            th.Start();
            TimerXml.Interval = 3000;
            TimerXml.Enabled = true;

            lblResponstaXml.Text = "AGUARDE !";
            lblResponstaXml.ForeColor = System.Drawing.Color.Green;
            btnRespostaXml.Enabled = false;
            modalXml.Show();


        }

        protected void btnBaixarXmls_Click(object sender, EventArgs e)
        {
            List<NfManifestoDAO> lista = (List<NfManifestoDAO>)Session["lista" + urlSessao()];
            List<NfManifestoDAO> listaDowloan = new List<NfManifestoDAO>();
            foreach (GridViewRow row in gridNfs.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelecionaItem");
                if (chk.Checked)
                {
                    NfManifestoDAO item = lista[row.RowIndex];
                    listaDowloan.Add(item);
                }
            }

            User usr = (User)Session["User"];
            String strPastaZip = Server.MapPath("~/modulos/NotaFiscal/pages/" + DateTime.Now.ToString("yyyyMMdd") + "-" + usr.getFilial());


            xmlNFE.criarPastaDowload(listaDowloan, strPastaZip);

            RedirectNovaAba("~\\modulos\\financeiro\\pages\\BaixarArquivo.aspx?endereco=" + strPastaZip.Replace("\\", "/") + ".zip");
        }

        protected void btnImportarNFE_Click(object sender, EventArgs e)
        {
            try
            {


                xmlNFE nfeXml = (xmlNFE)Session["xml" + urlSessao()];

                nfeXml.importarNFe();
                showMessage("Importação concluida", false);
            }
            catch (Exception err)
            {

                showMessage(err.Message, true);
            }

        }

        protected void imgBtnConsultaChave_Click(object sender, ImageClickEventArgs e)
        {
            txtChave.BackColor = System.Drawing.Color.White;
            if (txtChave.Text.Trim().Length == 0)
            {
                showMessage("Informe a chave da nota Fiscal para fazer a consulta", true);
            }
            else
            {
                try
                {
                    xmlNFE nfeXml = (xmlNFE)Session["xml" + urlSessao()];
                   
                    nfeXml.consultaNotaDFE(txtChave.Text);
                    System.Threading.Thread th = new System.Threading.Thread(buscarRespostaXML);
                    th.Start();
                    TimerXml.Interval = 3000;
                    TimerXml.Enabled = true;

                    lblResponstaXml.Text = "AGUARDE !";
                    lblResponstaXml.ForeColor = System.Drawing.Color.Green;
                    btnRespostaXml.Enabled = false;
                    modalXml.Show();
                   
                }
                catch (Exception err)
                {

                    showMessage(err.Message, true);
                }

            }
        }

        protected void btnItens_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.Parent.Parent;
            Session.Remove("rowItens");
            Session.Add("rowItens", row);
            if (row.Cells[7].Text.Equals("SIM"))
            {
                row.RowState = DataControlRowState.Selected;

                String chave = row.Cells[1].Text;
                lblCnpjItens.Text = row.Cells[2].Text;
                lblRazaoSocialItens.Text = row.Cells[3].Text;
                lblEmissaoItens.Text = row.Cells[4].Text;
                lblTotalItens.Text = row.Cells[5].Text;
                lblChaveItens.Text = chave.Trim();
                String strSqlItens = "Exec sp_NFe_det 'NFE" + chave + "'";
                gridItensDetalhes.DataSource = Conexao.GetTable(strSqlItens, null, false);
                gridItensDetalhes.DataBind();
                modalItens.Show();
                row.Cells[0].Focus();
            }

        }

        protected void Unnamed_Click(object sender, ImageClickEventArgs e)
        {
           
            modalItens.Hide();
            carregarNotas();
        }
    }
}