using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Dispositivos.pages
{
    public partial class CargaPDV : visualSysWeb.code.PagePadrao
    {
        bool bOperadores = false;
        bool bCargaTabelaPreco = false;
        protected void Page_Load(object sender, EventArgs e)
        {

            User usr = (User)Session["user"];
            bOperadores = Funcoes.valorParametro("CARGA_OPERADORES", usr).ToUpper().Equals("TRUE");

            if (!IsPostBack)
            {
                CargaDAO carga = null;
                // carga = new CargaDAO(usr);
                Session.Remove("carga");
                Session.Add("carga", carga);
                pesquisa();
                //ChecaRedeGrid();
                pnTipoZanthus.Visible = rdoZanthusTipoArquivo.SelectedValue.Equals("");
                pnPDV.Width = 300;
                chkOperadores.Visible = bOperadores;

            }
            bCargaTabelaPreco = Funcoes.valorParametro("CARGA_TABELA_PRECO", usr).ToUpper().Equals("TRUE");

            status = "editar";
            carregabtn(pnBtn);

        }

        protected void pesquisa()
        {
            User usr = (User)Session["User"];
            if (usr != null)
            {
                String strSql = "SELECT Filial, Modelo,FabEqu,Caixa" +
                                ", Diretorio_Carga= CASE WHEN isnull(Carga_Automatica,0) = 1 THEN 'CARGA AUTOMATICA'"+
                                       " WHEN ISNULL(ATIVA_LINK_SERVER,0)= 0  THEN Diretorio_carga "+
                                       " WHEN ISNULL(ATIVA_LINK_SERVER,0)= 1 THEN 'LINK:' + Link_server "+
                                       " WHEN ISNULL(ATIVA_LINK_SERVER,0)= 2 THEN 'SERVICO' "+
                                   "END" +
                                   ", Data_ult_atualizacao  " +
                               "FROM Controle_Filial_PDV WHERE Controle_Filial_PDV.Filial = '" + usr.filial.Filial + "' ORDER BY Caixa ";

                gridPesquisa.DataSource = Conexao.GetTable(strSql, usr, false);
                gridPesquisa.DataBind();
                atualizarHistorico();


            }
        }
        private void atualizarHistorico()
        {
            String sqlCargas = " Select top 100 id_carga," +
                                      "caixas = isnull((select STRING_AGG('*'+convert(varchar,status)+'*'+convert(varchar,caixa), '<br> ')  from carga_pdv where id_carga = c.id_carga),'---') " +
                                "from carga_pdv  as c " +
                                "group by id_carga " +
                                "order by id_carga desc";
            gridHistoricoCargas.DataSource = Conexao.GetTable(sqlCargas, null, false);
            gridHistoricoCargas.DataBind();

            foreach (GridViewRow Row in gridHistoricoCargas.Rows)
            {
                String htmlValues = Row.Cells[1].Text;
                htmlValues = htmlValues.Replace("*0*", "<img src=\"../imgs/carga_0.png\" width=\"10px\" />");
                htmlValues = htmlValues.Replace("*1*", "<img src=\"../imgs/carga_1.png\" width=\"10px\" />");
                htmlValues = htmlValues.Replace("*2*", "<img src=\"../imgs/carga_2.png\" width=\"10px\" />");
                Row.Cells[1].Text = htmlValues;
            }
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
            throw new NotImplementedException();
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {

            try
            {


                User usr = (User)Session["user"];

                if (rdoPDV.SelectedValue != "2" && !rdoZanthusTipoArquivo.SelectedValue.Equals("1"))
                {
                    if (!chkAlterados.Checked &&
                       !chkcargaTotal.Checked &&
                       !chkDesmarcarAlteracoes.Checked &&
                        (!bOperadores || !chkOperadores.Checked)
                       )
                    {
                        throw new Exception("Escolha um tipo de carga!");

                    }

                }
                else
                {
                    gerarCarga();
                }


                if (rdoPDV.SelectedValue == "0" &&
                        (
                            chkcargaTotal.Checked ||
                            chkAlterados.Checked ||
                            chkOperadores.Checked
                        )

                    )
                {
                    if (!verificaPDVSelecionado())
                    {
                        throw new Exception("Selecione um ou mais caixas para enviar a carga!");
                    }


                }


                if (!chkAlterados.Checked &&
                   !chkcargaTotal.Checked)
                {
                    if (chkDesmarcarAlteracoes.Checked)
                    {
                        CargaDAO.desmarcarAlterado();
                        erroShow("Alterados Desmarcados com sucesso", false);
                    }

                    if (chkOperadores.Checked)
                    {
                        CargaDAO.gerarArquivoOperadores(usr);
                        geraArquivos(usr.filial.diretorio_gera + "\\", "OP");
                        checaArquivosPendentes(true);
                        CargaDAO.limpaTemp();
                    }

                }
                else
                {
                    if (!chkcargaTotal.Checked)
                    {
                        gerarCarga();
                    }
                    else
                    {
                        modalPnConfirma.Show();
                    }
                }

            }
            catch (Exception err)
            {

                erroShow(err.Message, true);
            }
        }

        private void gerarCarga()
        {
            User usr = (User)Session["User"];
            int tipoArquivo = Convert.ToInt32(rdoPDV.SelectedValue);
            int alterados = (chkAlterados.Checked ? 1 : 0);
            if (tipoArquivo == 1 && rdoZanthusTipoArquivo.SelectedValue.Equals("1"))
            {
                tipoArquivo = 10;
                alterados = (chkGerarComCPF.Checked ? 1 : 0);
            }
            String strErro = "";
            String strMsg = "";
            string strNomeArquivoCarga = "";
            if (alterados == 1)
            {
                int qtde = 0;
                int.TryParse(Conexao.retornaUmValor("Select count(*) from mercadoria  WHERE ESTADO_MERCADORIA =1", usr), out qtde);
                if (qtde <= 0)
                {
                    throw new Exception("Não exite Itens Alterados para gerar a Carga!");
                }
            }
            if (rdoPDV.SelectedValue == "0")
            {
                if (existeLink())
                {
                    strErro = transmiteLinkServer(alterados);

                }
                if (existeServico())
                {
                    strErro += transmiteServico(alterados);
                }
            }

            if (rdoPDV.SelectedValue != "0" || existeArquivo())
            {
                try
                {


                    CargaDAO carga = new CargaDAO(usr, tipoArquivo, alterados, chkDesmarcarAlteracoes.Checked, chkGeraBuscaPreco.Checked, chkOperadores.Checked, bCargaTabelaPreco);

                    if (rdoPDV.SelectedValue == "0")
                    {
                        //Seta nome do arquivo para que todos tem o mesmo nome. Se o Check Box chkAlterados estiver clicado, o nome do arquivo
                        //inicia com A de alterado, senão, inicia com T de total. Somado a isso vem o restante contendo
                        //ano + mes + dia + hora + minuto que foi gerado a carga.
                        strNomeArquivoCarga = (chkAlterados.Checked ? "A" : "T") + DateTime.Now.ToString("yyyyMMddHHmmss");
                        geraArquivos(usr.filial.diretorio_gera + "\\", strNomeArquivoCarga);
                        checaArquivosPendentes(true);
                    }
                    else
                    {

                        strMsg = "Arquivo de Carga Gerado em :" + (tipoArquivo == 2 ? usr.filial.diretorio_busca_preco : usr.filial.diretorio_gera);
                        erroShow(strMsg, false);
                    }
                }
                catch (Exception err)
                {

                    strErro += err.Message;
                }
            }

            if (strErro.Equals(""))
            {
                erroShow("Carga efetuada com Sucesso! " + strMsg, false);
            }
            else
            {
                erroShow(strErro, true);
                return;
            }

        }
        protected String transmiteLinkServer(int alterados)
        {
            return transmite(alterados, "LINK:");
        }
        protected String transmiteServico(int alterados)
        {
            return transmite(alterados, "SERVICO");
        }

        protected String transmite(int alterados,string tipoCarga)
        {
            User usr = (User)Session["User"];
            String strErro = "";
            CargaDAO carga = new CargaDAO(usr);


            foreach (GridViewRow row in gridPesquisa.Rows)
            {
                if (row.Cells[2].Text.Contains(tipoCarga))
                {

                    CheckBox chk = (CheckBox)row.FindControl("chkSelecionaItem");
                    if (chk.Checked)
                    {
                        String caixa = row.Cells[1].Text;
                        Session.Remove("resultadoCarga");
                        Session.Add("resultadoCarga", "Enviando carga para o Caixa:" + caixa);
                        try
                        {
                            if (tipoCarga.Equals("LINK:"))
                            {
                                String endLink = row.Cells[2].Text.Replace(tipoCarga, "");
                                carga.insereLinkServer(endLink, alterados, caixa);
                            }
                            else if(tipoCarga.Equals("SERVICO"))
                            {
                                carga.insereServico(alterados, caixa);
                            }
                            row.ForeColor = System.Drawing.Color.Green;
                            row.Cells[3].Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        }
                        catch (Exception err)
                        {
                            strErro += "PDV:" + caixa + " erro:" + err.Message + "<br> ";
                            row.ForeColor = System.Drawing.Color.Red;
                        }

                    }
                }
            }
            if (!strErro.Equals(""))
            {
                Session.Remove("erroCarga");
                Session.Add("erroCarga", strErro);
            }
            return strErro;
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected override bool campoDesabilitado(Control campo)
        {
            throw new NotImplementedException();
        }

        protected override bool campoObrigatorio(Control campo)
        {
            throw new NotImplementedException();
        }
        protected void rdoZanthusTipoArquivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoZanthusTipoArquivo.SelectedValue.Equals("1"))
            {
                chkAlterados.Visible = false;
                chkcargaTotal.Visible = false;
                chkDesmarcarAlteracoes.Visible = false;
                chkAlterados.Checked = false;
                chkGerarComCPF.Visible = true;
                chkGeraBuscaPreco.Visible = false;
            }
            else
            {
                chkAlterados.Visible = true;
                chkcargaTotal.Visible = true;
                chkDesmarcarAlteracoes.Visible = true;
                chkGerarComCPF.Visible = false;
                chkGeraBuscaPreco.Visible = true;
            }
        }

        protected void rdoPDV_SelectedIndexChanged(object sender, EventArgs e)
        {
            divTipoCarga.Visible = true;
            divOpcoes.Visible = true;
            imgPDV.ImageUrl = "~\\modulos\\Dispositivos\\imgs\\pdv" + rdoPDV.SelectedValue + ".jpg";
            chkGerarComCPF.Visible = false;
            if (rdoPDV.SelectedValue.Equals("2"))
            {
                chkAlterados.Visible = false;
                chkDesmarcarAlteracoes.Visible = false;
                chkAlterados.Checked = false;
                chkDesmarcarAlteracoes.Checked = false;
                chkOperadores.Visible = false;
                chkOperadores.Checked = false;
                divTipoCarga.Visible = false;
                divOpcoes.Visible = false;
           

            }
            else
            {

                chkOperadores.Visible = bOperadores;

                chkAlterados.Visible = true;
                chkDesmarcarAlteracoes.Visible = true;
            }

            if (rdoPDV.SelectedValue != "0")
            {

                pnPDV.CssClass = "frame";
                pnPDV.Width = 700;

                divArquivos.Visible = false;
                divPdvsCadastrados.Visible = false;
                gridPesquisa.Visible = false;
                ltbArquivosPendentes.Visible = false;
                btnReenviar.Enabled = false;
                btnLimpar.Enabled = false;
                chkOperadores.Visible = false;
                gridHistoricoCargas.Visible = false;

                if (rdoPDV.SelectedValue == "1")
                {
                    pnTipoZanthus.Visible = true;
                    chkGeraBuscaPreco.Visible = true;
                }
                else
                {
                    pnTipoZanthus.Visible = false;
                    chkGeraBuscaPreco.Visible = false;
                }
            }
            else
            {

                pnTipoZanthus.Visible = false;
                pnPDV.CssClass = "cargaPDVDireita";
                pnPDV.Width = 300;
                chkOperadores.Visible = bOperadores;
                divArquivos.Visible = true;
                divPdvsCadastrados.Visible = true;
                gridPesquisa.Visible = true;
                gridHistoricoCargas.Visible = true;
                ltbArquivosPendentes.Visible = true;
                btnReenviar.Enabled = true;
                btnLimpar.Enabled = true;


            }
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
                    //
                    if (ChecaRede(item.Cells[2].Text))
                    {
                        chk.Checked = (sender as CheckBox).Checked;
                        item.ForeColor = System.Drawing.Color.MidnightBlue;
                    }
                    else
                    {
                        chk.Checked = false;
                        item.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }
        protected void chkSelecionaItem_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void ChecaRedeGrid()
        {
            foreach (GridViewRow item in gridPesquisa.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                //foreach (TableCell valor in item.Cells)
                //{
                //    String str = valor.Text;

                //}
                //
                chk.Visible = !item.Cells[2].Text.Contains("CARGA AUTOMATICA");
                
                if (ChecaRede(item.Cells[2].Text))
                {
                    chk.Checked = true;
                    item.ForeColor = System.Drawing.Color.MidnightBlue;
                }
                else
                {
                    item.ForeColor = System.Drawing.Color.Red;
                }
            }

        }

        private bool ChecaRede(String strRede)
        {
            if (strRede.Contains("LINK:"))
            {
                String end = strRede.Replace("LINK:", "").Trim();
                try
                {
                    String strcount = Conexao.retornaUmValor("Select count(*) from " + end + ".[SOLDI_PDV].[dbo].[Produtos_Carga] where ID_Produto =''", null);
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }
            else
            {
                if (strRede.Contains("CARGA AUTOMATICA"))
                {
                    return false;
                }
                else if (!Directory.Exists(strRede))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        protected void geraArquivos(string strDiretorioOrigem, string strNomeArquivo)
        {

            String strArquivoOrigem = (chkAlterados.Checked ? "ALTERA6.SDF" : "6.SDF");
            //Gera o arquivo local.
            for (int i = 0; i < gridPesquisa.Rows.Count; i++)
            {
                if (!gridPesquisa.Rows[i].Cells[2].Text.Contains("CARGA AUTOMATICA"))
                {
                    continue;
                }
                if (!gridPesquisa.Rows[i].Cells[2].Text.Contains("LINK:"))
                {
                    CheckBox chk = (CheckBox)gridPesquisa.Rows[i].FindControl("chkSelecionaItem");

                    if (chk != null && chk.Checked)
                    {
                        File.Copy(strDiretorioOrigem + strArquivoOrigem, strDiretorioOrigem + strNomeArquivo + "." + gridPesquisa.Rows[i].Cells[1].Text.PadLeft(3, '0'), true);
                        if (chkOperadores.Checked)
                        {
                            File.Copy(strDiretorioOrigem + "\\OP.TXT", strDiretorioOrigem + "OP." + gridPesquisa.Rows[i].Cells[1].Text.PadLeft(3, '0'), true);
                        }

                        if (bCargaTabelaPreco)
                        {
                            if (File.Exists(strDiretorioOrigem + "\\TBPRECO.TXT"))
                            {
                                File.Copy(strDiretorioOrigem + "\\TBPRECO.TXT", strDiretorioOrigem + "TBPRECO." + gridPesquisa.Rows[i].Cells[1].Text.PadLeft(3, '0'), true);
                            }
                        }

                    }
                }
            }
            //processo para efetuar cópia do arquivo para destino
            for (int i = 0; i < gridPesquisa.Rows.Count; i++)
            {
                if (!gridPesquisa.Rows[i].Cells[2].Text.Contains("CARGA AUTOMATICA"))
                {
                    continue;
                }

                if (!gridPesquisa.Rows[i].Cells[2].Text.Contains("LINK:"))
                {
                    CheckBox chk = (CheckBox)gridPesquisa.Rows[i].FindControl("chkSelecionaItem");
                    if (chk != null && chk.Checked)
                    {
                        if (!copiaArquivo(strDiretorioOrigem + strNomeArquivo + "." + gridPesquisa.Rows[i].Cells[1].Text.PadLeft(3, '0'), gridPesquisa.Rows[i].Cells[2].Text + strNomeArquivo + ".XXX"))
                        {
                            gridPesquisa.Rows[i].ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            gridPesquisa.Rows[i].ForeColor = System.Drawing.Color.Green;
                        }
                        if (chkOperadores.Checked)
                        {
                            if (!copiaArquivo(strDiretorioOrigem + "OP." + gridPesquisa.Rows[i].Cells[1].Text.PadLeft(3, '0'), gridPesquisa.Rows[i].Cells[2].Text + "OP.TXT"))
                            {
                                gridPesquisa.Rows[i].ForeColor = System.Drawing.Color.Red;
                            }
                            else
                            {
                                gridPesquisa.Rows[i].ForeColor = System.Drawing.Color.Green;
                            }
                        }
                        if (bCargaTabelaPreco)
                        {
                            if (!copiaArquivo(strDiretorioOrigem + "TBPRECO." + gridPesquisa.Rows[i].Cells[1].Text.PadLeft(3, '0'), gridPesquisa.Rows[i].Cells[2].Text + "TBPRECO.TXT"))
                            {
                                gridPesquisa.Rows[i].ForeColor = System.Drawing.Color.Red;
                            }
                            else
                            {
                                gridPesquisa.Rows[i].ForeColor = System.Drawing.Color.Green;
                            }
                        }
                    }
                }
            }

        }

        protected bool copiaArquivo(string strOrigem, string strDestino)
        {
            string strDestinoAlterado = "";
            strDestinoAlterado = strDestino.Substring(0, strDestino.Length - 4) + ".SDF";
            try
            {
                File.Copy(strOrigem, strDestino, true);
                if (File.Exists(strDestino))
                {
                    if (File.Exists(strDestinoAlterado))
                    {
                        File.Delete(strDestinoAlterado);
                    }
                    File.Move(strDestino, strDestinoAlterado);
                    File.Delete(strOrigem);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected void checaArquivosPendentes(bool gerado)
        {
            User usr = (User)Session["User"];
            if (usr != null)
            {
                //Limpar conteúdo do ListBox ltbArquivosPendentes
                ltbArquivosPendentes.Items.Clear();
                //Preenche ltbArquivosPendentes com conteudo do diretorio.
                string strDirPendentes = @usr.filial.diretorio_gera;
                string[] txtList = Directory.GetFiles(strDirPendentes, "*.0*");
                foreach (string f in txtList)
                {
                    string strNomeArquivo = f.Substring(strDirPendentes.Length);
                    ltbArquivosPendentes.Items.Add(strNomeArquivo);
                }
                if (gerado)
                {
                    erroShow("Arquivo de carga gerado com sucesso!", false);
                }
            }
        }

        protected void btnReenviar_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ltbArquivosPendentes.Items.Count; i++)
            {
                string strFileReenvio = "";
                string strArquivoDestino = "";
                User usr = (User)Session["user"];
                if (usr != null)
                {
                    strFileReenvio = usr.filial.diretorio_gera + ltbArquivosPendentes.Items[i].Text;
                    for (int j = 0; j < gridPesquisa.Rows.Count; j++)
                    {
                        if (int.Parse(Right(ltbArquivosPendentes.Items[i].Text, 3)) == int.Parse(gridPesquisa.Rows[j].Cells[1].Text))
                        {
                            strArquivoDestino = gridPesquisa.Rows[j].Cells[2].Text + ltbArquivosPendentes.Items[i].Text;
                            if (!copiaArquivo(strFileReenvio, strArquivoDestino))
                            {
                                gridPesquisa.Rows[j].ForeColor = System.Drawing.Color.Red;
                            }
                        }
                    }
                }
            }
            checaArquivosPendentes(true);
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ltbArquivosPendentes.Items.Count; i++)
            {
                string strFileExcluir = "";
                User usr = (User)Session["user"];
                if (usr != null)
                {
                    strFileExcluir = usr.filial.diretorio_gera + ltbArquivosPendentes.Items[i].Text;
                    File.Delete(strFileExcluir);
                }
            }
            checaArquivosPendentes(false);

        }

        static string Right(string original, int numbercharacters)
        {
            return original.Substring(original.Length - numbercharacters);
        }

        protected void chkAlterados_CheckedChanged(object sender, EventArgs e)
        {
            chkGeraBuscaPreco.Enabled = !chkAlterados.Checked;
            if (chkAlterados.Checked)
            {
                chkcargaTotal.Checked = false;
                chkGeraBuscaPreco.Checked = false;

                ChecaRedeGrid();
            }
        }

        protected void chkTotal_CheckedChanged(object sender, EventArgs e)
        {
            if (chkcargaTotal.Checked)
            {
                chkAlterados.Checked = false;
                chkGeraBuscaPreco.Enabled = true;
                chkGeraBuscaPreco.Checked = true;
                foreach (GridViewRow item in gridPesquisa.Rows)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                    chk.Checked = false;
                }
            }
        }


        private bool verificaPDVSelecionado()
        {
            foreach (GridViewRow item in gridPesquisa.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                if (chk.Checked)
                {
                    return true;
                }

            }
            return false;
        }

        protected void btnConfirmaGerarTotal_Click(object sender, EventArgs e)
        {
            gerarCarga();
            modalPnConfirma.Hide();
        }

        protected void btnCancelaGerarTotal_Click(object sender, EventArgs e)
        {
            modalPnConfirma.Hide();
        }

        protected void btnOkError_Click(object sender, EventArgs e)
        {
            modalError.Hide();
            atualizarHistorico();
        }

        protected void erroShow(String mensagem, bool erro)
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
        protected bool existeLink()
        {
            return existeGrid("LINK:");
        }
        protected bool existeServico()
        {
            return existeGrid("SERVICO");
        }
        protected bool existeGrid(String procura)
        {

            foreach (GridViewRow row in gridPesquisa.Rows)
            {
                if (row.Cells[2].Text.Contains(procura))
                {
                    return true;
                }
            }
            return false;

        }
        protected bool existeArquivo()
        {
            foreach (GridViewRow row in gridPesquisa.Rows)
            {
                if (!row.Cells[2].Text.Contains("LINK:"))
                {
                    return true;
                }
            }
            return false;

        }


        protected void TimerCarga_Tick(object sender, EventArgs e)
        {
            String resultado = (String)Session["resultadoCarga"];
            String error = (String)Session["erroCarga"];
            String aborta = (String)Session["abortaCarga"];

            if (aborta != null)
            {
                TimerCarga.Enabled = false;
                Session.Remove("abortaImportaColetor");
            }
            else if (resultado != null)
            {

                if (resultado.Contains("Carga Efeturada com Sucesso!"))
                {
                    TimerCarga.Enabled = false;
                   
                    erroShow(resultado, false);
                }
                else
                {
                    lblDetalhesAguarde.Text = resultado;
                    lblDetalhesAguarde.ForeColor = System.Drawing.Color.Blue;
                }

            }



            if (error != null)
            {
                Session.Remove("erroCarga");
                TimerCarga.Enabled = false;
                erroShow(error, true);
            }

            if (TimerCarga.Enabled)
            {
                modalaguarde.Show();
            }
            else
            {
                modalaguarde.Hide();
            }


        }

        protected void ImgBtnAtulizaHistorico_Click(object sender, ImageClickEventArgs e)
        {
            atualizarHistorico();
            erroShow("Historico Atualizado com sucesso", false);
        }
    }
}




//Sinto Muito Me Perdoe Agradeço Eu Te Amo.-- Hoponopono
//Sinto Muito Me Perdoe Agradeço Eu Te Amo.