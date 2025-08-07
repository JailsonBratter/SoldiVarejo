using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.modulos.Sped.code;
using visualSysWeb.dao;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using visualSysWeb.modulos.Sped.dao;
using System.IO;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Sped.pages
{
    public partial class IcmsIpi : visualSysWeb.code.PagePadrao
    {
        private bool SPEDC170_Obrigatorio = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime hoje = DateTime.Today;
                DateTime primeiroDiaMesAnterior = new DateTime(hoje.Year, hoje.Month, 1).AddMonths(-1);
                DateTime ultimoDiaMesAnterior = new DateTime(hoje.Year, hoje.Month, 1).AddDays(-1);

                txtDe.Text = primeiroDiaMesAnterior.ToString("dd/MM/yyyy"); //   "01/" + DateTime.Now.ToString("MM/yyyy");
                txtAte.Text = ultimoDiaMesAnterior.ToString("dd/MM/yyyy"); // DateTime.Now.ToString("dd/MM/yyyy");
                carregarBlocos(true);
            }
            else
            {
                carregarBlocos(false);
            }

            Boolean.TryParse(Funcoes.valorParametro("SPED_C170_OBRIG_SAIDA", null), out SPEDC170_Obrigatorio);

            //status = "editar";
            sopesquisa(pnBtn);

            if (!IsPostBack)
            {
                txtAte.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
                txtAte.MaxLength = 10;
                txtDe.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
                txtDe.MaxLength = 10;
            }
        }


        protected void carregarBlocos(bool carregaDados)
        {
            //Hashtable tbBlocos0 = (Hashtable)Session["Blocos0" + urlSessao()];



            String sql = "Select id from sped_blocos where id_bloco_pai = 0 AND tipoArquivo='ICMSIPI' order by ordem";


            User usr = (User)Session["User"];




            ArrayList arqGerados = (ArrayList)Session["ArquivosGerar" + urlSessao()];


            if (arqGerados != null)
            {
                carregarchkTelas(carregaDados);
            }
            else
            {
                arqGerados = new ArrayList();
                Hashtable tbBlocos = (Hashtable)Session["BlocosCarregados" + urlSessao()];
                if (tbBlocos == null)
                {
                    tbBlocos = new Hashtable();
                }


                pnBlocos.Controls.Clear();
                SqlDataReader rs = null;
                try
                {


                    rs = Conexao.consulta(sql, usr, false);


                    while (rs.Read())
                    {
                        int id = 0;
                        int.TryParse(rs["id"].ToString(), out id);
                        Sped_blocosDAO bloco = new Sped_blocosDAO(id, usr);

                        addBloco(pnBlocos, bloco, 0, null, tbBlocos, carregaDados, arqGerados);
                    }

                    Session.Remove("ArquivosGerar" + urlSessao());
                    Session.Add("ArquivosGerar" + urlSessao(), arqGerados);

                    Session.Remove("BlocosCarregados" + urlSessao());
                    Session.Add("BlocosCarregados" + urlSessao(), tbBlocos);

                    carregarchkTelas(carregaDados);
                }
                catch (Exception err)
                {
                    showMessage(err.Message, true);                }
                finally
                {
                    if (rs != null)
                        rs.Close();
                }
            }
            verificaSubArquivos(pnBlocos);
        }
        private void addBloco(Panel pnBlocoPrincipal, Sped_blocosDAO bloco, int vMargem, SqlDataReader rsBlocoPai, Hashtable tbBlocos, bool carregarDados, ArrayList arqGerar)
        {


            if (!tbBlocos.Contains(bloco.id))
            {
                tbBlocos.Add(bloco.id, bloco);
            }

            User usr = (User)Session["User"];

            DateTime dtDe = new DateTime();
            DateTime dtAte = new DateTime();

            DateTime.TryParse(txtDe.Text, out dtDe);
            DateTime.TryParse(txtAte.Text, out dtAte);
            if (bloco.gerarArquivo)
            {
                String sqlBloco = "exec " + bloco.str_procedure;
                String sqlParametros = "";

                foreach (Sped_blocos_parametrosDAO parametro in bloco.arrParametros)
                {
                    if (!sqlParametros.Equals(""))
                    {
                        sqlParametros += ",";
                    }
                    sqlParametros += "@" + parametro.nome_parametro + "=";
                    if (parametro.tipo_dados.ToUpper().Equals("DATA"))
                    {
                        if (parametro.nome_parametro.ToUpper().Replace("_", "").IndexOf("DATAINI") >= 0)
                        {
                            sqlParametros += "'" + dtDe.ToString("yyyyMMdd") + "'";
                        }
                        else if (parametro.nome_parametro.ToUpper().Replace("_", "").IndexOf("DATAFIM") >= 0)
                        {
                            sqlParametros += "'" + dtAte.ToString("yyyyMMdd") + "'";
                        }
                        else if (!parametro.campo_pai.Equals(""))
                        {
                            DateTime dt = new DateTime();
                            DateTime.TryParse(rsBlocoPai[parametro.campo_pai].ToString(), out dt);
                            sqlParametros += "'" + dt.ToString("yyyyMMdd") + "'";
                        }


                    }
                    else if (parametro.nome_parametro.ToUpper().Equals("FILIAL"))
                    {
                        sqlParametros += "'" + usr.getFilial() + "'";
                    }
                    else if (parametro.nome_parametro.ToUpper().Equals("TIPO"))
                    {
                        sqlParametros += "1";
                    }
                    else if (!parametro.campo_pai.Equals(""))
                    {
                        if (parametro.tipo_dados.ToUpper().Equals("TEXTO"))
                        {
                            sqlParametros += "'" + rsBlocoPai[parametro.campo_pai].ToString() + "'";
                        }
                        else
                        {
                            sqlParametros += rsBlocoPai[parametro.campo_pai].ToString();
                        }
                    }


                }

                sqlBloco += " " + sqlParametros;
                SqlDataReader rsBloco = null;
                try
                {
                    if (sqlBloco.ToString().IndexOf("C100") >= 0)
                    {

                    }

                    rsBloco = Conexao.consulta(sqlBloco, usr, false);
                    while (rsBloco.Read())
                    {
                        Sped_blocosDAO sPai = null;
                        String strNomeArquivo = "";
                        if (bloco.id_bloco_pai > 0)
                        {
                            sPai = (Sped_blocosDAO)tbBlocos[bloco.id_bloco_pai];
                            strNomeArquivo = sPai.bloco + "." + rsBlocoPai[sPai.campo_arquivo].ToString() + ".";
                        }
                        strNomeArquivo += bloco.bloco;


                        String strTexto = bloco.bloco;
                        if (!bloco.campo_arquivo.Equals(""))
                        {
                            strNomeArquivo += "." + rsBloco[bloco.campo_arquivo].ToString();
                            strTexto += "." + rsBloco[bloco.campo_arquivo].ToString();
                        }
                        foreach (Sped_blocos_parametrosDAO parametro in bloco.arrParametros)
                        {
                            if (!parametro.campo_pai.Equals(""))
                            {
                                strNomeArquivo += "." + rsBlocoPai[parametro.campo_pai].ToString();
                                strTexto += "." + rsBlocoPai[parametro.campo_pai].ToString();
                            }
                        }


                        strNomeArquivo += "." + bloco.ordem.ToString();
                        strNomeArquivo = strNomeArquivo.Replace("/", "").Replace(" ", "").Replace("00:00:00", "");

                        strTexto = strTexto.Replace("/", "").Replace(" ", "").Replace("00:00:00", "");



                        ArquivosGerar arqGerarItem = new ArquivosGerar();
                        arqGerarItem.strNomeArquivo = strNomeArquivo;
                        arqGerarItem.strTexto = strTexto;
                        arqGerarItem.vMargem = vMargem;

                        arqGerar.Add(arqGerarItem);




                        foreach (Sped_blocosDAO item in bloco.arrBlocosFilhos)
                        {
                            if (item.gerarArquivo)
                            {
                                addBloco(pnBlocoPrincipal, item, vMargem + 20, rsBloco, tbBlocos, carregarDados, arqGerar);
                            }
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    if (rsBloco != null)
                    {
                        rsBloco.Close();
                    }
                }
            }
        }





        protected void carregarchkTelas(bool carregarDados)
        {
            User usr = (User)Session["User"];
            ArrayList chkArquivo = (ArrayList)Session["ArquivosGerar" + urlSessao()];
            if (chkArquivo != null)
            {

                DateTime dtDe = new DateTime();
                DateTime dtAte = new DateTime();
                String css = "celulaSpedPar";

                DateTime.TryParse(txtDe.Text, out dtDe);
                DateTime.TryParse(txtAte.Text, out dtAte);
                chkTodos.Checked = true;

                foreach (ArquivosGerar item in chkArquivo)
                {



                    Panel cPn = new Panel();
                    cPn.CssClass = "row";


                    Panel cPanel1 = new Panel();
                    cPanel1.CssClass = css;

                    // CheckBox Gerar Bloco



                    Panel cPnBtnGerar = new Panel();

                    CheckBox chkGerar = new CheckBox();
                    chkGerar.ID = "chkGerar_" + item.strNomeArquivo;

                    CheckBox chkExi = (CheckBox)pnBlocos.FindControl(chkGerar.ID);
                    if (chkExi == null)
                    {

                        chkGerar.Text = item.strTexto;
                        if (carregarDados)
                        {
                            ArquivosBloco arq = new ArquivosBloco(usr, dtDe, dtAte, "ICMSIPI");

                            chkGerar.Checked = !arq.arquivoGerado(item.strNomeArquivo);
                            if (!chkGerar.Checked)
                            {
                                chkTodos.Checked = false;
                                chkGerar.ForeColor = System.Drawing.Color.Green;
                            }

                        }
                        cPnBtnGerar.Controls.Add(chkGerar);
                        cPnBtnGerar.ID = "cPnGerar" + item.strNomeArquivo;
                        cPanel1.Controls.Add(cPnBtnGerar);

                        cPanel1.ID = "cPanel1" + item.strNomeArquivo;
                        cPn.Controls.Add(cPanel1);

                        cPn.Attributes.Add("style", "margin-left:" + item.vMargem + "px; margin-top:5px; width:80%; ");
                        cPn.Attributes.Add("onclick", "javascript:blocosfilhosSelecionados();");

                        cPn.ID = "cPn" + item.strNomeArquivo;

                        pnBlocos.Controls.Add(cPn);
                    }

                }
            }
        }

        public void verificaSubArquivos(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox chk = (CheckBox)c;
                    encontraChkPai(pnBlocos, chk);
                }

                verificaSubArquivos(c);
            }
        }
        public void encontraChkPai(Control parent, CheckBox chkPai)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox chk = (CheckBox)c;
                    if ((chk.Checked) && (chk.ID.IndexOf(chkPai.ID.Substring(0, chkPai.ID.Length - 2)) >= 0))
                    {
                        chkPai.Checked = true;
                        break;
                    }
                }

                encontraChkPai(c, chkPai);
            }
        }

        protected void btnCancelaProcesso_Click(object sender, EventArgs e)
        {
            TimerProcessa.Enabled = false;
            modalConfirmaCancelamento.Show();
        }

        protected void btnArquivoFinal_Click(object sender, EventArgs e)
        {
            try
            {
                User usr = (User)Session["User"];

                DateTime dtInicio = new DateTime();
                DateTime dtFim = new DateTime();

                DateTime.TryParse(txtDe.Text, out dtInicio);
                DateTime.TryParse(txtAte.Text, out dtFim);

                ArquivosBloco arq = new ArquivosBloco(usr, dtInicio, dtFim, "ICMSIPI");
                arq.unificarArquivos();
                String strPastaDowload = Server.MapPath("~/modulos/Sped/pages/DowLoad");
                String strArquivoDowload = arq.BaixarArquivoFinal(strPastaDowload);
                RedirectNovaAba("~\\modulos\\financeiro\\pages\\BaixarArquivo.aspx?endereco=" + strArquivoDowload.Replace("\\", "/") );
                showMessage("Arquivo Gerado com sucesso", false);


            }
            catch (Exception err)
            {
                showMessage(err.Message, true);
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
            if (validaDatas())
            {
                chkTodos.Checked = true;
                Session.Remove("ArquivosGerar" + urlSessao());
                carregarBlocos(true);
            }
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override bool campoDesabilitado(Control campo)
        {
            throw new NotImplementedException();
        }

        protected override bool campoObrigatorio(Control campo)
        {
            throw new NotImplementedException();
        }



        protected void txtDe_TextChanged(object sender, EventArgs e)
        {

        }

        private bool validaDatas()
        {
            txtDe.BackColor = System.Drawing.Color.White;
            txtAte.BackColor = System.Drawing.Color.White;
            lblError.Text = "";
            DateTime dtInicio;
            DateTime dtFim;
            DateTime.TryParse(txtDe.Text, out dtInicio);
            DateTime.TryParse(txtAte.Text, out dtFim);

            if (!dtInicio.Month.Equals(dtFim.Month))
            {
               showMessage("As Datas não são do mesmo mês !!",true);
                
                txtDe.BackColor = System.Drawing.Color.Red;
                txtAte.BackColor = System.Drawing.Color.Red;
                return false;
            }
            else
            {
                return true;
            }
        }

        protected void btnGerarBlocos_Click(object sender, EventArgs e)
        {
            if (validaDatas())
            {


                //   carregarBlocos();
                barraTotal.Attributes.Remove("style");
                barraTotal.Attributes.Add("style", "width: 0%;");
                lblProgressTotal.Text = "0%";

                barraItens.Attributes.Remove("style");
                barraItens.Attributes.Add("style", "width: 0%;");
                lblProgressItens.Text = "0%";
                // processarBlocos();

                Session.Remove("erroProcessamento");
                Session.Remove("cancelaProcesso");


                lblError.Text = "";

                lblInicio.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");


                habilita(false);
                int vTotalArquivos = 0;
                Session.Remove("totSelecionado");
                Session.Remove("ArquivosGerados");
                Session.Remove("ArquivoProcesso");
                Session.Remove("fimProcesso");
                Session.Remove("BlocosCarregados" + urlSessao());

                qtdChkSelecionados(pnBlocos);
                String strTotSele = (String)Session["totSelecionado"];
                int.TryParse(strTotSele, out vTotalArquivos);
                lblArqGerados.Text = " 0 ";
                lblArquivoSelecionados.Text = vTotalArquivos + " Arquivo(s) Selecionado(s)";

                System.Threading.Thread th = new System.Threading.Thread(processarBlocos);

                th.Start();


                TimerProcessa.Interval = 200;
                TimerProcessa.Enabled = true;
                divPrecessamento.Visible = true;
                btnCancelaProcesso.Visible = true;
            }

        }

        private void habilita(bool enable)
        {
            pnBlocos.Enabled = enable;
            chkTodos.Enabled = enable;
            txtDe.Enabled = enable;
            txtAte.Enabled = enable;
            btnGerarBlocos.Enabled = enable;
            btnArquivoFinal.Enabled = enable;
            ImgDtDe.Visible = enable;
            ImgDtAte.Visible = enable;

        }

        private void qtdChkSelecionados(Control pn)
        {

            String strTotSele = (String)Session["totSelecionado"];
            int vTotalArquivos = 0;

            int.TryParse(strTotSele, out vTotalArquivos);
            foreach (Control c in pn.Controls)
            {
                if (c is CheckBox)
                {
                    if (((CheckBox)(c)).Checked)
                    {
                        vTotalArquivos++;
                        Session.Remove("totSelecionado");
                        Session.Add("totSelecionado", vTotalArquivos.ToString());
                    }
                }
                qtdChkSelecionados(c);

            }

        }

        private void testeCarregarBarras()
        {



         
            for (int i = 0; i <= 100; i++)
            {
                System.Threading.Thread.Sleep(500);
                Session.Remove("barraTotal");
                Session.Add("barraTotal", i.ToString());
            }


        }



        private void processarBlocos()
        {

            //int vTotalArqGerar = qtdChkSelecionados();

            //Session.Remove("totArquivoSelecionados");
            //Session.Add("totArquivoSelecionados", vTotalArqGerar.ToString());

            String sql = "Select id from sped_blocos where id_bloco_pai = 0 AND tipoArquivo='ICMSIPI' order by ordem";
            Conexao.executarSql("delete from EFD_PisCofins_Bloco_9");
            User usr = (User)Session["User"];
            Hashtable tbBlocos = (Hashtable)Session["BlocosCarregados" + urlSessao()];
            if (tbBlocos == null)
            {
                tbBlocos = new Hashtable();
            }
            DateTime dtDe = new DateTime();
            DateTime dtAte = new DateTime();
            DateTime.TryParse(txtDe.Text, out dtDe);
            DateTime.TryParse(txtAte.Text, out dtAte);

            ArquivosBloco arq = new ArquivosBloco(usr, dtDe, dtAte, "ICMSIPI");


            SqlDataReader rs = null;
            try
            {
                bool b990 = true;

                rs = Conexao.consulta(sql, usr, false);

                StringBuilder ArqTxt = new StringBuilder();
                while (rs.Read())
                {

                    String cancela = (String)Session["cancelaProcesso"];
                    if (cancela != null)
                    {
                        b990 = false;
                        break;
                    }

                    int id = 0;
                    int.TryParse(rs["id"].ToString(), out id);


                    Sped_blocosDAO bloco;
                    if (tbBlocos.Contains(id))
                    {
                        bloco = (Sped_blocosDAO)tbBlocos[id];
                    }
                    else
                    {
                        bloco = new Sped_blocosDAO(id, usr);
                    }
                    //if (tbBlocos.Contains(bloco.blocoTotaliza))
                    //{
                    //    tbBlocos.Remove(bloco.blocoTotaliza);
                    //}

                    gerarArquivo(ArqTxt, bloco, null, tbBlocos);

                    if (tbBlocos.Contains(bloco.blocoTotaliza))
                    {
                        int tBlocp990 = (int)tbBlocos[bloco.blocoTotaliza];
                        StringBuilder strBloc990 = new StringBuilder();
                        if (tBlocp990 >= 1)
                        {
                            tBlocp990++;
                        }
                        strBloc990.Append("|" + bloco.blocoTotaliza + "|" + tBlocp990.ToString() + "|");
                        arq.gerarArquivo(bloco.blocoTotaliza, strBloc990);
                        int existBloco = 0;
                        int.TryParse(Conexao.retornaUmValor("Select COUNT(*) from EFD_PisCofins_Bloco_9 where  Bloco='" + bloco.blocoTotaliza + "'", null), out existBloco);
                        String sql990 = "";
                        if (existBloco > 0)
                        {
                            sql990 = "update EFD_PisCofins_Bloco_9 set Registros = (Registros+" + tBlocp990.ToString() + ") where  Bloco='" + bloco.blocoTotaliza + "'";
                        }
                        else
                        {
                            sql990 = "insert into EFD_PisCofins_Bloco_9 (Registros, Bloco) values ('" + tBlocp990.ToString() + "'  ,'" + bloco.blocoTotaliza + "')";
                        }

                        Conexao.executarSql(sql990);




                    }
                }

                //Gerar Bloco990

                SqlDataReader rs990 = null;
                try
                {
                    if (b990)
                    {
                        String sql990 = "";
                        foreach (DictionaryEntry item in tbBlocos)
                        {
                            try
                            {
                                if (!item.Key.ToString().Trim().Equals(""))
                                {
                                    if (item.Key.ToString().Substring(0, 1).Equals("T"))
                                    {
                                        int tBloc = (int)item.Value;
                                        sql990 = "insert into EFD_PisCofins_Bloco_9 (Registros, Bloco) values ('" + tBloc.ToString() + "','" + item.Key.ToString().Substring(1).Trim() + "') ";
                                        Conexao.executarSql(sql990);
                                    }
                                }
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }

                        sql990 = "insert into EFD_PisCofins_Bloco_9 (Bloco, Registros ) values ('1001','1')" +
                                                                                           ", ('1010','1') " +
                                                                                           ", ('1990','1')" +
                                                                                           ", ('9990','1')" +
                                                                                           ", ('9999','1')" +
                                                                                           ", ('D001','1')" +
                                                                                           ", ('D990','1')" +
                                                                                           ", ('E001','1')" +
                                                                                           ", ('E990','1')" +
                                                                                           ", ('G001','1')" +
                                                                                           ", ('G990','1')" +
                                                                                           //", ('H001','1')" +
                                                                                           //", ('H990','1')" +
                                                                                           //", ('K001','1')" +
                                                                                           //", ('K990','1')" +
                                                                                           ", ('9001','1')"
                                                                                           ;

                        Conexao.executarSql(sql990);



                        StringBuilder strArqBlocD001 = new StringBuilder();
                        strArqBlocD001.AppendLine("|D001|1|");
                        strArqBlocD001.AppendLine("|D990|2|");
                        arq.gerarArquivo("D001", strArqBlocD001);

                        StringBuilder strArqBlocE001 = new StringBuilder();
                        strArqBlocE001.AppendLine("|E001|1|");
                        strArqBlocE001.AppendLine("|E990|2|");
                        arq.gerarArquivo("E001", strArqBlocE001);

                        StringBuilder strArqBlocG001 = new StringBuilder();

                        strArqBlocG001.AppendLine("|G001|1|");
                        strArqBlocG001.AppendLine("|G990|2|");
                        arq.gerarArquivo("G001", strArqBlocG001);

                        //strArqBloc990.AppendLine("|H001|1|");
                        //strArqBloc990.AppendLine("|H990|2|");

                        //StringBuilder strArqBlocK001 = new StringBuilder();
                        //strArqBlocK001.AppendLine("|K001|1|");
                        //strArqBlocK001.AppendLine("|K990|2|");
                        //arq.gerarArquivo("K001", strArqBlocK001);

                        StringBuilder strArqBloc1001 = new StringBuilder();
                        strArqBloc1001.AppendLine("|1001|0|");
                        strArqBloc1001.AppendLine(Conexao.retornaUmValor("exec sp_EFD_ICMSIPI_1010",null));
                        //strArqBloc1001.AppendLine("|1010|N|N|N|N|N|N|N|N|N|N|N|N|N");
                        strArqBloc1001.AppendLine("|1990|3|");
                        arq.gerarArquivo("Z1001", strArqBloc1001); // colocado o Z para o arquivo ficar por ultimo na pasta;


                        StringBuilder strArqBloc990 = new StringBuilder();
                        strArqBloc990.AppendLine("|9001|0|");
                        int count990 = 4;
                        int count9900 = 1;
                        rs990 = Conexao.consulta("Select * from EFD_PisCofins_Bloco_9 order by bloco", usr, false);
                        while (rs990.Read())
                        {
                            String strQtd = "";
                            if (rs990["bloco"].ToString().Trim().Equals("0990") ||
                                rs990["bloco"].ToString().Trim().Equals("C990") ||
                                rs990["bloco"].ToString().Trim().Equals("B990") ||
                                rs990["bloco"].ToString().Trim().Equals("D990") ||
                                rs990["bloco"].ToString().Trim().Equals("H990") ||
                                rs990["bloco"].ToString().Trim().Equals("K990")
                                )
                            {
                                strQtd = "1";
                            }
                            else
                            {
                                strQtd = rs990["Registros"].ToString().Trim();
                            }
                            strArqBloc990.AppendLine("|9900|" + rs990["bloco"].ToString().Trim() + "|" + strQtd + "|");
                            count990++;
                            count9900++;
                        }
                        strArqBloc990.AppendLine("|9900|9900|" + count9900 + "|");

                        strArqBloc990.AppendLine("|9990|" + count990 + "|");

                        arq.gerarArquivo("Z9900", strArqBloc990); // colocado o Z para o arquivo ficar por ultimo na pasta;


                        Session.Add("fimProcesso", "true");
                        Session.Remove("BlocosCarregados" + urlSessao());
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    if (rs990 != null)
                        rs990.Close();
                }

            }
            catch (Exception err)
            {
                Session.Add("erroProcessamento", err.Message);
                showMessage(err.Message, true);
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }
        }




        private void gerarArquivo(StringBuilder ArqTxt, Sped_blocosDAO bloco, SqlDataReader rsBlocoPai, Hashtable tbBlocos)
        {

            String cancela = (String)Session["cancelaProcesso"];
            if (cancela != null)
            {
                return;

            }



            if (!tbBlocos.Contains(bloco.id))
            {
                tbBlocos.Add(bloco.id, bloco);
            }





            User usr = (User)Session["User"];

            DateTime dtDe = new DateTime();
            DateTime dtAte = new DateTime();

            DateTime.TryParse(txtDe.Text, out dtDe);
            DateTime.TryParse(txtAte.Text, out dtAte);

            
            String sqlBloco = "exec " + bloco.str_procedure;
            String sqlParametros = "";

            String strFormatoDt = "yyyyMMdd";
            if (bloco.bloco.Equals("0000") || bloco.bloco.Equals("K100") || bloco.bloco.Equals("K200"))
            {
                strFormatoDt = "ddMMyyyy";
            }

            foreach (Sped_blocos_parametrosDAO parametro in bloco.arrParametros)
            {
                if (bloco.bloco.Equals("C800"))
                {
                    String str = "";
                }
                if (!sqlParametros.Equals(""))
                {
                    sqlParametros += ",";
                }
                sqlParametros += "@" + parametro.nome_parametro + "=";
                if (parametro.tipo_dados.ToUpper().Equals("DATA"))
                {
                    if (parametro.nome_parametro.ToUpper().Replace("_", "").IndexOf("DATAINI") >= 0)
                    {
                        sqlParametros += "'" + dtDe.ToString(strFormatoDt) + "'";
                    }
                    else if (parametro.nome_parametro.ToUpper().Replace("_", "").IndexOf("DATAFIM") >= 0)
                    {
                        sqlParametros += "'" + dtAte.ToString(strFormatoDt) + "'";
                    }
                    else if (parametro.herda_pai)
                    {
                        DateTime dt = new DateTime();
                        DateTime.TryParse(parametro.valorParametro, out dt);
                        sqlParametros += "'" + dt.ToString(strFormatoDt) + "'";
                    }
                    else if (!parametro.campo_pai.Equals(""))
                    {


                        DateTime dt = Funcoes.dtTry(rsBlocoPai[parametro.campo_pai].ToString());
                        String strDt = rsBlocoPai[parametro.campo_pai].ToString();
                        if (strDt.Trim().Length > 0)
                        {
                            //
                            if(dt.Equals(DateTime.MinValue))
                            {
                                strDt = strDt.Substring(4, 4) + strDt.Substring(2, 2) + strDt.Substring(0, 2);
                            }
                            else
                            {
                                strDt = dt.ToString("yyyyMMdd");
                            }
                            
                        }//DateTime.TryParse(rsBlocoPai[parametro.campo_pai].ToString(), out dt);
                        sqlParametros += "'" + strDt + "'";

                    }
                    else if (parametro.nome_parametro.ToUpper().Contains("DATA"))
                    {
                        sqlParametros += "'" + dtDe.ToString("yyyyMMdd") + "'";
                    }


                }
                else if (parametro.nome_parametro.ToUpper().Equals("FILIAL"))
                {
                    sqlParametros += "'" + usr.getFilial() + "'";
                }
                else if (parametro.nome_parametro.ToUpper().Equals("TIPO"))
                {
                    sqlParametros += "1";
                }
                else if (parametro.herda_pai)
                {
                    if (parametro.tipo_dados.ToUpper().Equals("TEXTO"))
                    {
                        sqlParametros += "'" + parametro.valorParametro + "'";
                    }
                    else
                    {
                        sqlParametros += parametro.valorParametro;
                    }

                }
                else if (!parametro.campo_pai.Equals(""))
                {
                    if (parametro.tipo_dados.ToUpper().Equals("TEXTO"))
                    {
                        sqlParametros += "'" + rsBlocoPai[parametro.campo_pai].ToString() + "'";
                    }
                    else
                    {
                        sqlParametros += rsBlocoPai[parametro.campo_pai].ToString();
                    }
                }
            }


            


            sqlBloco += " " + sqlParametros;

            SqlDataReader rsBloco = null;
            try
            {

                //Torna a geração do bloco C170 obrigatório.
                if (bloco.bloco.Equals("C170"))
                {
                    if (rsBlocoPai["IND_EMIT"].ToString().Equals("0") && SPEDC170_Obrigatorio)
                    {
                        sqlBloco += ", @GerarObrigatorio=1";
                    }
                }

                if (bloco.bloco.Equals("C100"))
                {

                }

                int cTrib = 0;
                rsBloco = Conexao.consulta(sqlBloco, usr, false);
                bool criaBloco = true;

                //Blocos que podem estar vazios.
                if (!rsBloco.HasRows)
                {
                    if (bloco.bloco.Equals("H001") || bloco.bloco.Equals("K001"))
                    {
                        if (bloco.bloco.Equals("H001"))
                        {
                            Conexao.executarSql("INSERT INTO EFD_PisCofins_Bloco_9 (Bloco, Registros) VALUES ('H001', '1'), ('H990','1')");
                            ArqTxt.AppendLine("|H001|1|");
                            ArqTxt.AppendLine("|H990|2|");
                        }
                        else if (bloco.bloco.Equals("K001"))
                        {
                            Conexao.executarSql("INSERT INTO EFD_PisCofins_Bloco_9 (Bloco, Registros) VALUES ('K001', '1'), ('K990','1')");
                            ArqTxt.AppendLine("|K001|1|");
                            ArqTxt.AppendLine("|K990|2|");
                        }
                    }

                }
                else
                {

                    while (rsBloco.Read())
                    {
                        natureza_operacaoDAO ntOP = null;
                        Sped_blocosDAO sPai = null;
                        String strNomeArquivo = "";

                        if (bloco.bloco.Equals("C170"))
                        {
                            int tipNf = 0;
                            int.TryParse(rsBlocoPai["IND_OPER"].ToString(), out tipNf);
                            if (tipNf == 1)
                            {
                                criaBloco = false;


                                Decimal vNatOp = 0;
                                Decimal.TryParse(rsBloco["COD_NAT"].ToString(), out vNatOp);
                                ntOP = new natureza_operacaoDAO(vNatOp.ToString(), usr);


                                if (vNatOp >= 6000)
                                {
                                    criaBloco = true;
                                }
                                if (ntOP.NF_devolucao)
                                {
                                    criaBloco = true;
                                }

                                if (SPEDC170_Obrigatorio)
                                {
                                    criaBloco = true;
                                }

                            }
                        }
                        if (bloco.bloco.Equals("C176"))
                        {
                            if (ntOP == null)
                            {
                                Decimal vNatOp = 0;
                                Decimal.TryParse(rsBloco["COD_NAT"].ToString(), out vNatOp);
                                ntOP = new natureza_operacaoDAO(vNatOp.ToString(), usr);

                            }
                            if (!ntOP.NF_devolucao)
                            {
                                criaBloco = false;
                            }
                        }
                        if (bloco.bloco.Equals("C490"))
                        {
                            Decimal vTot = 0;
                            Decimal.TryParse(rsBlocoPai["VL_BRT"].ToString(), out vTot);
                            if (vTot <= 0)
                            {
                                criaBloco = false;
                            }

                        }

                        if (criaBloco)
                        {

                            if (bloco.id_bloco_pai > 0)
                            {
                                sPai = (Sped_blocosDAO)tbBlocos[bloco.id_bloco_pai];
                                if (!sPai.campo_arquivo.Equals(""))
                                {
                                    strNomeArquivo = sPai.bloco + "." + rsBlocoPai[sPai.campo_arquivo].ToString() + ".";
                                }
                            }
                            strNomeArquivo += bloco.bloco;


                            String strTexto = bloco.bloco;
                            if (!bloco.campo_arquivo.Equals(""))
                            {
                                strNomeArquivo += "." + rsBloco[bloco.campo_arquivo].ToString();
                                strTexto += "." + rsBloco[bloco.campo_arquivo].ToString();
                            }
                            foreach (Sped_blocos_parametrosDAO parametro in bloco.arrParametros)
                            {
                                if (!parametro.campo_pai.Equals(""))
                                {
                                    if (parametro.herda_pai)
                                    {
                                        strNomeArquivo += "." + parametro.valorParametro.Trim();
                                        strTexto += "." + parametro.valorParametro.Trim();
                                    }
                                    else
                                    {
                                        strNomeArquivo += "." + rsBlocoPai[parametro.campo_pai].ToString();
                                        strTexto += "." + rsBlocoPai[parametro.campo_pai].ToString();
                                    }

                                }
                            }


                            strNomeArquivo += "." + bloco.ordem.ToString();
                            strNomeArquivo = strNomeArquivo.Replace("/", "").Replace(" ", "").Replace("00:00:00", "");


                            bool bGerar = true;
                            if (bloco.gerarArquivo)
                            {
                                CheckBox chkGerar = (CheckBox)pnBlocos.FindControl("chkGerar_" + strNomeArquivo);
                                bGerar = chkGerar.Checked;
                                if (bGerar)
                                {
                                    lblProgressItens.Text = strNomeArquivo;
                                    Session.Remove("ArquivoProcesso");
                                    Session.Add("ArquivoProcesso", strNomeArquivo);

                                }
                            }
                            if (bGerar)
                            {

                                StringBuilder strArqBloco = new StringBuilder();
                                bool separador = true;

                                if (!bloco.arquivoGrupo)
                                {

                                    if (rsBloco[0].ToString().IndexOf("|") == 0)
                                    {
                                        separador = false;
                                    }
                                    else
                                    {
                                        strArqBloco.Append("|");
                                    }

                                    if (!bloco.arquivoGrupo)
                                    {
                                        //Identificar se trata-se de uma NFCe
                                        bool NFCe = false;
                                        for (int i = 0; i < rsBloco.FieldCount; i++)
                                        {
                                            //NFCe 
                                            bool iColuna = true;
                                            String strCampo = "";

                                            //Checa se o BLOCO é igual a C100 e o modelo é igual a 65 - NFCe
                                            if (bloco.bloco.Equals("C100") && rsBloco.GetName(i).ToUpper().Trim().Equals("COD_MOD"))
                                            {
                                                if (rsBloco[i].ToString().Equals("65"))
                                                {
                                                    NFCe = true;
                                                }
                                            }

                                            if (bloco.bloco.Equals("C100") && rsBloco.GetName(i).ToUpper().Equals("COD_PART"))
                                            {
                                                if (rsBloco[i].ToString().Length >= 44)
                                                {
                                                    NFCe = true;
                                                    strCampo = "";
                                                }
                                                else
                                                {
                                                    strCampo = rsBloco[i].ToString();
                                                }
                                            }
                                            else
                                            {
                                                strCampo = rsBloco[i].ToString();
                                            }


                                            if (bloco.bloco.Equals("0200") && i == 12)
                                            {
                                                DateTime ini2017 = new DateTime(2017, 01, 01);
                                                if (dtAte.CompareTo(ini2017) < 0)
                                                {
                                                    iColuna = false;
                                                }
                                            }

                                            if (bloco.bloco.Equals("C170"))
                                            {
                                                if (rsBloco.GetName(i).Equals("VLR_ABAT_NT"))
                                                {
                                                    DateTime ini2019 = new DateTime(2019, 01, 01);
                                                    if (dtAte.CompareTo(ini2019) < 0)
                                                    {
                                                        iColuna = false;
                                                    }
                                                }
                                            }

                                            if (bloco.bloco.Equals("C405") && i > 6)
                                            {
                                                iColuna = false;
                                            }

                                            if (bloco.bloco.Equals("C420"))
                                            {
                                                if (i == 0)
                                                {
                                                    strArqBloco.Append("C420|");
                                                    if (strCampo.Substring(0, 1).Equals("T"))
                                                    {
                                                        cTrib++;
                                                    }
                                                }


                                            }

                                            if (iColuna)
                                            {
                                                if (strCampo.IndexOf("/") >= 0)
                                                {
                                                    DateTime dt = new DateTime();

                                                    bool bDt = DateTime.TryParse(rsBloco[i].ToString(), out dt);
                                                    if (bDt)
                                                    {
                                                        strArqBloco.Append(dt.ToString("ddMMyyyy"));
                                                    }
                                                    else
                                                    {
                                                        strArqBloco.Append(rsBloco[i].ToString().Replace("/", "").Trim());
                                                    }
                                                }
                                                else
                                                {
                                                    if (strCampo.IndexOf(",") >= 0 && !(NFCe && (rsBloco.GetName(i).Equals("VL_PIS") || rsBloco.GetName(i).Equals("VL_COFINS"))))
                                                    {
                                                        Decimal vVlr = 0;
                                                        bool bVlr = Decimal.TryParse(strCampo, out vVlr);
                                                        if (bVlr)
                                                        {
                                                            strArqBloco.Append(vVlr.ToString("N2").Replace(".", ""));
                                                        }
                                                        else
                                                        {
                                                            strArqBloco.Append(strCampo.Trim());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //NFCe
                                                        System.Diagnostics.Debug.WriteLine(rsBloco.GetName(i));
                                                        if (bloco.bloco.Equals("C100") && rsBloco.GetName(i).ToUpper().Equals("COD_PART"))
                                                        {
                                                            if (rsBloco[i].ToString().Length >= 44)
                                                            {
                                                                strArqBloco.Append("");
                                                            }
                                                            else
                                                            {
                                                                strArqBloco.Append(rsBloco[i].ToString().Trim());
                                                            }
                                                        }
                                                        else if (NFCe && bloco.bloco.Equals("C100") &&
                                                            (rsBloco.GetName(i).ToUpper().Equals("VL_BC_ICMS_ST")
                                                            || rsBloco.GetName(i).ToUpper().Equals("VL_ICMS_ST")
                                                            || rsBloco.GetName(i).ToUpper().Equals("VL_IPI")
                                                            || rsBloco.GetName(i).ToUpper().Equals("VL_PIS")
                                                            || rsBloco.GetName(i).ToUpper().Equals("VL_COFINS")
                                                            || rsBloco.GetName(i).ToUpper().Equals("VL_PIS_ST")
                                                            || rsBloco.GetName(i).ToUpper().Equals("VL_COFINS_ST"))
                                                            )
                                                        {
                                                            strArqBloco.Append("");
                                                        }
                                                        else
                                                        {
                                                            strArqBloco.Append(rsBloco[i].ToString().Trim());
                                                        }
                                                    }
                                                }
                                                if (separador)
                                                {
                                                    strArqBloco.Append("|");
                                                }

                                                if (bloco.bloco.Equals("C420") && i == 1)
                                                {
                                                    if (cTrib > 0)
                                                    {
                                                        strArqBloco.Append(cTrib.ToString().Trim() + "||");
                                                    }
                                                    else
                                                    {
                                                        strArqBloco.Append("||");
                                                    }

                                                }
                                            }
                                        }

                                    }
                                }


                                if (bloco.arrBlocosFilhos.Count > 0)
                                {
                                    bool gerarArquivoLinha = false;

                                    foreach (Sped_blocosDAO item in bloco.arrBlocosFilhos)
                                    {
                                        if (item.gerarArquivo)
                                        {
                                            gerarArquivoLinha = true;
                                            break;

                                        }
                                    }
                                    if (strArqBloco.Length > 0)
                                    {

                                        if (gerarArquivoLinha)
                                        {
                                            ArqTxt.AppendLine(strArqBloco.ToString());
                                            ArquivosBloco arq = new ArquivosBloco(usr, dtDe, dtAte, "ICMSIPI");
                                            arq.gerarArquivo(strNomeArquivo, ArqTxt);
                                            String strArq = (String)Session["ArquivosGerados"];
                                            int vArq = 0;
                                            int.TryParse(strArq, out vArq);
                                            vArq++;
                                            Session.Remove("ArquivosGerados");
                                            Session.Add("ArquivosGerados", vArq.ToString());
                                            strArqBloco.Clear();
                                            ArqTxt.Clear();
                                            bGerar = false;


                                        }
                                        else
                                        {
                                            strArqBloco.AppendLine();
                                        }
                                    }
                                }

                                foreach (Sped_blocosDAO item in bloco.arrBlocosFilhos)
                                {
                                    foreach (Sped_blocos_parametrosDAO pitem in item.arrParametros)
                                    {
                                        if (pitem.herda_pai)
                                        {
                                            pitem.valorParametro = rsBlocoPai[pitem.campo_pai].ToString().Trim();
                                        }
                                    }

                                    gerarArquivo(strArqBloco, item, rsBloco, tbBlocos);
                                }

                                if (!bloco.arquivoGrupo)
                                {

                                    if (!tbBlocos.Contains("T" + bloco.bloco))
                                    {
                                        tbBlocos.Add("T" + bloco.bloco, 0);
                                    }

                                    if (!tbBlocos.Contains(bloco.blocoTotaliza))
                                    {
                                        tbBlocos.Add(bloco.blocoTotaliza, 0);
                                    }

                                    int TqtdeB = (int)tbBlocos["T" + bloco.bloco];

                                    int qtd = (int)tbBlocos[bloco.blocoTotaliza];
                                    qtd++;
                                    TqtdeB++;
                                    tbBlocos[bloco.blocoTotaliza] = qtd;
                                    tbBlocos["T" + bloco.bloco] = TqtdeB;
                                }
                                if (strArqBloco.Length > 0)
                                {

                                    ArqTxt.AppendLine(strArqBloco.ToString().Trim());

                                    if (bGerar)
                                    {
                                        if (bloco.gerarArquivo)
                                        {

                                            ArquivosBloco arq = new ArquivosBloco(usr, dtDe, dtAte, "ICMSIPI");
                                            arq.gerarArquivo(strNomeArquivo, ArqTxt);
                                            String strArq = (String)Session["ArquivosGerados"];
                                            int vArq = 0;
                                            int.TryParse(strArq, out vArq);
                                            vArq++;
                                            Session.Remove("ArquivosGerados");
                                            Session.Add("ArquivosGerados", vArq.ToString());
                                            ArqTxt.Clear();
                                        }
                                    }


                                }
                            }
                        }
                    }
                }

            }
            catch (Exception err)
            {

                Session.Remove("erroProcessamento");
                Session.Add("erroProcessamento", err.Message + " Bloco:" + bloco.bloco);
                showMessage( err.Message,true);

                throw err;
            }
            finally
            {
                if (rsBloco != null)
                {
                    rsBloco.Close();
                }
            }




        }





        protected void TimerProcessa_Tick(object sender, EventArgs e)
        {


            String erro = (String)Session["erroProcessamento"];
            if (erro != null)
            {
                showMessage( erro,true);
               

                TimerProcessa.Enabled = false;

                lblProgressTotal.Text = "ERRO!!!! ";
                habilita(true);
                barraItens.Attributes.Remove("style");
                barraItens.Attributes.Add("style", "width: 100%;");


                barraTotal.Attributes.Remove("style");
                barraTotal.Attributes.Add("style", "width: 100%;");

            }

            else
            {



                String strArquivoSelecionados = (String)Session["totSelecionado"];
                String strArquivoProcesso = (String)Session["ArquivoProcesso"];
                String strArquivosGerados = (String)Session["ArquivosGerados"];
                String strPorcItem = (String)Session["PorcItem"];

                Decimal vArqSelecionados;
                Decimal.TryParse(strArquivoSelecionados, out vArqSelecionados);

                Decimal vArquivosGerados;
                Decimal.TryParse(strArquivosGerados, out vArquivosGerados);

                Decimal vPorcItem;
                Decimal.TryParse(strPorcItem, out vPorcItem);

                Decimal porc = 0;
                String fim = (String)Session["fimProcesso"];
                if (fim != null)
                {
                    vArquivosGerados = vArqSelecionados;
                }


                if (vArquivosGerados > 0)
                {
                    lblArqGerados.Text = vArquivosGerados.ToString();
                    porc = vArquivosGerados / vArqSelecionados * 100;
                }

                if (strArquivoProcesso != null && !strArquivoProcesso.Trim().Equals(""))
                {
                    lblProgressItens.Text = strArquivoProcesso;


                    if (vPorcItem < 100)
                    {
                        vPorcItem += 5;
                    }
                    else
                    {
                        vPorcItem = 0;
                    }
                    Session.Remove("PorcItem");
                    Session.Add("PorcItem", vPorcItem.ToString());
                }
                else
                {
                    lblProgressItens.Text = "Aguarde.";
                }

                DateTime t = new DateTime();
                DateTime.TryParse(lblInicio.Text, out t);

                TimeSpan mDt = DateTime.Now.Subtract(t);
                lblTempo.Text = mDt.ToString(@"hh\:mm\:ss");

                barraItens.Attributes.Remove("style");
                barraItens.Attributes.Add("style", "width: " + vPorcItem.ToString("N0") + "%;");


                barraTotal.Attributes.Remove("style");
                barraTotal.Attributes.Add("style", "width: " + porc.ToString("N0") + "%;");
                lblProgressTotal.Text = porc.ToString("N0") + "%";



                if (porc >= 100)
                {
                    TimerProcessa.Enabled = false;
                    lblProgressTotal.Text = "TODOS ARQUIVOS GERADOS";
                    barraItens.Attributes.Remove("style");
                    barraItens.Attributes.Add("style", "width: 100%;");
                    lblProgressItens.Text = "";
                    habilita(true);
                    //carregarchkTelas(true);
                }
                else
                {
                    String cancela = (String)Session["cancelaProcesso"];
                    if (cancela != null)
                    {
                        TimerProcessa.Enabled = false;
                        habilita(true);

                        lblProgressTotal.Text = "PROCESSAMENTO CANCELADO!!";

                        barraItens.Attributes.Remove("style");
                        barraItens.Attributes.Add("style", "width: 100%;");


                        barraTotal.Attributes.Remove("style");
                        barraTotal.Attributes.Add("style", "width: 100%;");
                    }


                    if (fim != null)
                    {
                        TimerProcessa.Enabled = false;
                        habilita(true);

                        lblProgressTotal.Text = "PROCESSAMENTO TERMINADO!!";

                        barraItens.Attributes.Remove("style");
                        barraItens.Attributes.Add("style", "width: 100%;");


                        barraTotal.Attributes.Remove("style");
                        barraTotal.Attributes.Add("style", "width: 100%;");
                    }

                }

            }

        }

        protected void imgBtnLimpar_Click(object sender, ImageClickEventArgs e)
        {
            ModalConfirma.Show();

        }


        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                User usr = (User)Session["User"];

                DateTime dtDe = new DateTime();
                DateTime dtAte = new DateTime();

                DateTime.TryParse(txtDe.Text, out dtDe);
                DateTime.TryParse(txtAte.Text, out dtAte);
                ArquivosBloco arq = new ArquivosBloco(usr, dtDe, dtAte, "ICMSIPI");
                arq.limparArquivos();
                carregarchkTelas(true);

            }
            catch (Exception err)
            {
                showMessage(err.Message, true);
            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            ModalConfirma.Hide();
        }

        protected void imgBtnConfirmaCancelar_Click(object sender, ImageClickEventArgs e)
        {
            btnCancelaProcesso.Visible = false;
            Session.Add("cancelaProcesso", "true");
            modalConfirmaCancelamento.Hide();
            TimerProcessa.Enabled = true;
        }

        protected void imgBtnBtnNaoCancelar_Click(object sender, ImageClickEventArgs e)
        {
            modalConfirmaCancelamento.Hide();
            TimerProcessa.Enabled = true;

        }

        protected void btnOkError_Click(object sender, EventArgs e)
        {
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
    }
}