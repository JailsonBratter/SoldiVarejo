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
using System.Data;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Sped.pages
{
    public partial class PisCofins : visualSysWeb.code.PagePadrao
    {
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



            String sql = "Select id from sped_blocos where id_bloco_pai = 0 AND tipoArquivo='PISCOFINS'  order by ordem";


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
                    showMessage(err.Message, true);
                }
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
                            ArquivosBloco arq = new ArquivosBloco(usr, dtDe, dtAte, "PISCOFINS");

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


        protected void btnArquivoFinal_Click(object sender, EventArgs e)
        {
            try
            {
                User usr = (User)Session["User"];

                DateTime dtInicio = new DateTime();
                DateTime dtFim = new DateTime();

                DateTime.TryParse(txtDe.Text, out dtInicio);
                DateTime.TryParse(txtAte.Text, out dtFim);

                ArquivosBloco arq = new ArquivosBloco(usr, dtInicio, dtFim, "PISCOFINS");
                arq.unificarArquivos();
                String strPastaDowload = Server.MapPath("~/modulos/Sped/pages/DowLoad");
                String strArquivoDowload = arq.BaixarArquivoFinal(strPastaDowload);
                RedirectNovaAba("~\\modulos\\financeiro\\pages\\BaixarArquivo.aspx?endereco=" + strArquivoDowload.Replace("\\", "/"));
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

        protected void btnGerarBlogcos_Click(object sender, EventArgs e)
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

        protected void imgBtnLimpar_Click(object sender, ImageClickEventArgs e)
        {
            ModalConfirma.Show();

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



            int v = 1;
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
            String sql = "Select id from sped_blocos where id_bloco_pai = 0 AND tipoArquivo='PISCOFINS' order by ordem";
            
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

            ArquivosBloco arq = new ArquivosBloco(usr, dtDe, dtAte, "PISCOFINS");


            SqlDataReader rs = null;
            try
            {

                Conexao.executarSql("delete from EFD_PisCofins_Bloco_9");
                
                Conexao.executarSql("delete from EFD_M400 ");
                Conexao.executarSql("delete from EFD_M410 ");


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

                    gerarArquivo(ArqTxt, bloco, null, tbBlocos);

                    if (tbBlocos.Contains(bloco.blocoTotaliza))
                    {
                        int tBlocp990 = (int)tbBlocos[bloco.blocoTotaliza];
                        StringBuilder strBloc990 = new StringBuilder();
                       
                        if (tBlocp990 > 1)
                        {
                            tBlocp990++;
                        }
                        if (bloco.blocoTotaliza.Equals("A990"))
                        {
                            strBloc990.Append("|" + bloco.blocoTotaliza + "|2|");
                        }
                        else
                        {
                            strBloc990.Append("|" + bloco.blocoTotaliza + "|" + tBlocp990.ToString() + "|");
                        
                        }
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
                            if (item.Key.ToString().Substring(0, 1).Equals("T"))
                            {
                                int tBloc = (int)item.Value;
                                sql990 = "insert into EFD_PisCofins_Bloco_9 (Registros, Bloco) values ('" + tBloc.ToString() + "','" + item.Key.ToString().Substring(1).Trim() + "') ";
                                Conexao.executarSql(sql990);
                            }
                        }

                        sql990 = "insert into EFD_PisCofins_Bloco_9 (Bloco, Registros ) values ('1001','1')" +
                                                                                           ", ('1990','1')" +
                                                                                           ", ('9990','1')" +
                                                                                           ", ('9999','1')" +
                                                                                           ", ('D001','1')" +
                                                                                           ", ('D990','1')" +
                                                                                           ", ('F001','1')" +
                                                                                           ", ('F990','1')" +
                                                                                           ", ('I001','1')" +
                                                                                           ", ('I990','1')" +
                                                                                           ", ('9001','1')" +
                                                                                           ", ('M990','1')"+
                                                                                           ", ('P001','1')" +
                                                                                           ", ('P990','1')" 
                                                                                           
                                                                                           ;

                        Conexao.executarSql(sql990);



                        StringBuilder strArqBloc990 = new StringBuilder();

                        strArqBloc990.AppendLine("|D001|1|");
                        strArqBloc990.AppendLine("|D990|2|");
                        strArqBloc990.AppendLine("|F001|1|");
                        strArqBloc990.AppendLine("|F990|2|");
                        strArqBloc990.AppendLine("|I001|1|");
                        strArqBloc990.AppendLine("|I990|2|");

                        //Blocos M001
                        int M990 = 1;
                        

                        SqlDataReader rsM001 = null;
                        
                        try
                        {

                            rsM001 = Conexao.consulta("exec sp_EFD_PisCofins_M001", null, false);
                            if (rsM001.Read())
                            {
                                rsLinhaTxt(strArqBloc990, rsM001);
                                M990++;

                                sql990 = "insert into EFD_PisCofins_Bloco_9 (Registros, Bloco) values ('1','M001') ";
                                Conexao.executarSql(sql990);

                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        finally
                        {
                            if (rsM001 != null)
                                rsM001.Close();
                        }

                        //Bloco M400 
                        SqlDataReader rsM400 = null;
                        int m400 = 0;
                        int m410 = 0;
                        try
                        {

                            rsM400 = Conexao.consulta("Select REG, CST, sum(VL_TOT_REC) AS VL_TOT_REC, COD_CTA = (Select top 1  Cod_Conta from Conta_Contabil where entrada = 0), DESC_COMPL ='' From EFD_M400 Group By REG, CST", null, false);
                            while (rsM400.Read())
                            {
                                rsLinhaTxt(strArqBloc990, rsM400);
                                m400++;
                                M990++;
                                //Bloco M410
                                SqlDataReader rsM410 = null;
                                
                                try
                                {
                                    rsM410 = Conexao.consulta("Select REG, NAT_REC, sum(VL_TOT_REC) AS VL_TOT_REC, COD_CTA = (Select top 1  Cod_Conta from Conta_Contabil where entrada = 0), DESC_COMPL ='' From EFD_M410 WHERE CST = '" + rsM400["CST"].ToString() + "' Group By REG, NAT_REC, CST", null, false);
                                    while (rsM410.Read())
                                    {
                                        rsLinhaTxt(strArqBloc990, rsM410);
                                        M990++;
                                        m410++;
                                    }
                                }
                                catch (Exception)
                                {

                                    throw;
                                }
                                finally
                                {
                                    if (rsM410 != null)
                                        rsM410.Close();

                                  
                                }
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        finally
                        {
                            if (rsM400 != null)
                                rsM400.Close();

                            
                        }
                        sql990 = "insert into EFD_PisCofins_Bloco_9 (Registros, Bloco) values ('" + m400.ToString() + "','M400') ";
                        Conexao.executarSql(sql990);
                        sql990 = "insert into EFD_PisCofins_Bloco_9 (Registros, Bloco) values ('" + m410.ToString() + "','M410') ";
                        Conexao.executarSql(sql990);

                        //Bloco M800 
                        SqlDataReader rsM800 = null;
                        int m800 = 0;
                        int m810 = 0;
                        try
                        {

                            rsM800 = Conexao.consulta("Select 'M800', CST, sum(VL_TOT_REC) AS VL_TOT_REC, COD_CTA = (Select top 1  Cod_Conta from Conta_Contabil where entrada = 0), DESC_COMPL ='' From EFD_M400 Group By REG, CST", null, false);
                            while (rsM800.Read())
                            {
                                rsLinhaTxt(strArqBloc990, rsM800);
                                M990++;
                                m800++;
                                //Bloco M810
                                SqlDataReader rsM810 = null;
                                
                                try
                                {
                                    rsM810 = Conexao.consulta("Select 'M810', NAT_REC, sum(VL_TOT_REC) AS VL_TOT_REC, COD_CTA = (Select top 1  Cod_Conta from Conta_Contabil where entrada = 0), DESC_COMPL ='' From EFD_M410 WHERE CST = '" + rsM800["CST"].ToString() + "' Group By REG, NAT_REC, CST", null, false);
                                    while (rsM810.Read())
                                    {
                                        rsLinhaTxt(strArqBloc990, rsM810);
                                        m810++;
                                        M990++;
                                    }
                                }
                                catch (Exception)
                                {

                                    throw;
                                }
                                finally
                                {
                                    if (rsM810 != null)
                                        rsM810.Close();
                                   
                                }
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        finally
                        {
                            if (rsM800 != null)
                                rsM800.Close();

                            
                        }

                        sql990 = "insert into EFD_PisCofins_Bloco_9 (Registros, Bloco) values ('" + m800.ToString() + "','M800') ";
                        Conexao.executarSql(sql990);

                        sql990 = "insert into EFD_PisCofins_Bloco_9 (Registros, Bloco) values ('" + m810.ToString() + "','M810') ";
                        Conexao.executarSql(sql990);


                        strArqBloc990.AppendLine("|M990|" + M990.ToString() + "|");

                        strArqBloc990.AppendLine("|P001|1|");
                        strArqBloc990.AppendLine("|P990|2|");
                        strArqBloc990.AppendLine("|1001|1|");
                        strArqBloc990.AppendLine("|1990|2|");

                        strArqBloc990.AppendLine("|9001|0|");
                        int count990 = 4;
                        int count9900 = 1;

                        rs990 = Conexao.consulta("Select * from EFD_PisCofins_Bloco_9 order by bloco", usr, false);
                        while (rs990.Read())
                        {
                            String strQtd = "";
                            if (rs990["bloco"].ToString().Trim().Equals("0990") ||
                                rs990["bloco"].ToString().Trim().Equals("C990")
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


        private void AddBlocoM(StringBuilder arq)
        {

        }


        private void rsLinhaTxt(StringBuilder strArqBloco, SqlDataReader rsBloco)
        {
            bool iColuna = true;
            
            bool separador = true;

            if (rsBloco[0].ToString().IndexOf("|") == 0)
            {
                separador = false;
            }
            else
            {
                strArqBloco.Append("|");
            }
            for (int i = 0; i < rsBloco.FieldCount; i++)
            {

                String strCampo = rsBloco[i].ToString();

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
                    if (strCampo.IndexOf(",") >= 0)
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
                        strArqBloco.Append(rsBloco[i].ToString().Trim());
                    }
                }
                if (separador)
                {
                    strArqBloco.Append("|");
                }

            }
            strArqBloco.AppendLine();
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


            if (bloco.bloco.Equals("C870"))
            {
                String str = "só para parar no bloco";
            }



            User usr = (User)Session["User"];

            DateTime dtDe = new DateTime();
            DateTime dtAte = new DateTime();

            DateTime.TryParse(txtDe.Text, out dtDe);
            DateTime.TryParse(txtAte.Text, out dtAte);

            String sqlBloco = "exec " + bloco.str_procedure;
            String sqlParametros = "";

            String strFormatoDt = "yyyyMMdd";

            String ultimoDiaMes = "";
            if (bloco.bloco.Equals("0000"))
            {
                DateTime dtFimMes = dtAte;;
                dtFimMes = dtFimMes.AddMonths(1);

                DateTime.TryParse("01/" + dtFimMes.ToString("MM/yyyy"), out dtFimMes);
                dtFimMes = dtFimMes.AddDays(-1);
                
                strFormatoDt = "ddMMyyyy";
                ultimoDiaMes = dtFimMes.ToString(strFormatoDt);
            }

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
                        sqlParametros += "'" + dtDe.ToString(strFormatoDt) + "'";
                    }
                    else if (parametro.nome_parametro.ToUpper().Replace("_", "").IndexOf("DATAFIM") >= 0)
                    {
                        if (bloco.bloco.Equals("0000"))
                        {
                            sqlParametros += "'" + ultimoDiaMes + "'";
                        }
                        else
                        {
                            sqlParametros += "'" + dtAte.ToString(strFormatoDt) + "'";
                        }
                    }
                    else if (parametro.herda_pai)
                    {
                        DateTime dt = new DateTime();
                        DateTime.TryParse(parametro.valorParametro, out dt);
                        sqlParametros += "'" + dt.ToString(strFormatoDt) + "'";
                    }
                    else if (!parametro.campo_pai.Equals(""))
                    {
                        String dt = rsBlocoPai[parametro.campo_pai].ToString();
                        
                        
                        sqlParametros += "'" + Funcoes.dtTry(dt).ToString(strFormatoDt) + "'";
                    }


                }
                else if (parametro.nome_parametro.ToUpper().Equals("FILIAL"))
                {
                    sqlParametros += "'" + usr.getFilial() + "'";
                }
                else if (parametro.nome_parametro.ToUpper().Equals("TIPO"))
                {
                    sqlParametros += "0";
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
                //TESTE
                if (sqlBloco.IndexOf("C170") >= 0)
                {

                }


                int cTrib = 0;
                rsBloco = Conexao.consulta(sqlBloco, usr, false);

                

                bool criaBloco = true;
                while (rsBloco.Read())
                {
                    natureza_operacaoDAO ntOP = null;
                    Sped_blocosDAO sPai = null;
                    String strNomeArquivo = "";

                    if (bloco.bloco.Equals("C170"))
                    {
                        int tipNf = 0;
                        int.TryParse(rsBlocoPai["IND_OPER"].ToString(), out tipNf);
                        //if (tipNf == 1)
                        //{
                        //    criaBloco = false;


                        //    Decimal vNatOp = 0;
                        //    Decimal.TryParse(rsBloco["COD_NAT"].ToString(), out vNatOp);
                        //    ntOP = new natureza_operacaoDAO(vNatOp.ToString(), usr);


                        //    if (vNatOp >= 6000)
                        //    {
                        //        criaBloco = true;
                        //    }
                        //    //if (ntOP.NF_devolucao)
                        //    //{
                        //    //    criaBloco = true;
                        //    //}

                        //}
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
                    //if (bloco.bloco.Equals("C490"))
                    //{
                    //    Decimal vTot = 0;
                    //    Decimal.TryParse(rsBlocoPai["VL_BRT"].ToString(), out vTot);
                    //    if (vTot <= 0)
                    //    {
                    //        criaBloco = false;
                    //    }

                    //}

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

                                //Identificar se trata-se de uma NFCe
                                bool NFCe = false;
                                for (int i = 0; i < rsBloco.FieldCount; i++)
                                {

                                    bool iColuna = true;
                                    String strCampo = rsBloco[i].ToString();
                                    if (bloco.bloco.Equals("C405") && i > 6)
                                    {


                                        iColuna = false;
                                    }
                                    
                                    //Checa se o BLOCO é igual a C100 e o modelo é igual a 65 - NFCe
                                    if (bloco.bloco.Equals("C100") && rsBloco.GetName(i).ToUpper().Trim().Equals("COD_MOD"))
                                    {
                                        if (rsBloco[i].ToString().Equals("65"))
                                        {
                                            NFCe = true;
                                        }
                                    }

                                    if (NFCe)
                                    {
                                        if (NFCe && bloco.bloco.Equals("C100") &&
                                            (rsBloco.GetName(i).ToUpper().Equals("VL_BC_ICMS_ST")
                                            || rsBloco.GetName(i).ToUpper().Equals("VL_ICMS_ST")
                                            || rsBloco.GetName(i).ToUpper().Equals("VL_IPI")
                                            || rsBloco.GetName(i).ToUpper().Equals("VL_PIS_ST")
                                            || rsBloco.GetName(i).ToUpper().Equals("VL_COFINS_ST"))
                                            )
                                        {
                                            strCampo = "";
                                        }
                                    }


                                    if (bloco.bloco.Equals("C481") || bloco.bloco.Equals("C870"))
                                    {
                                        if (rsBloco.GetName(i).Equals("NATUREZA_RECEITA"))
                                        {
                                            iColuna = false;
                                        }
                                        if (i == 0)
                                        {
                                            if (!rsBloco["CST_PIS"].Equals("01") && !rsBloco["CST_PIS"].Equals("02"))
                                            {
                                                Decimal vlr;
                                                Decimal.TryParse(rsBloco["VL_ITEM"].ToString(), out vlr);

                                                natezaReceita(rsBloco["CST_PIS"].ToString(), vlr, rsBloco["NATUREZA_RECEITA"].ToString());
                                            }
                                        }
                                    }
                                   

                                    if (bloco.bloco.Equals("0200")) //não inclui a coluna 12 CEST
                                    {
                                        if (rsBloco.GetName(i).Equals("CEST"))
                                            iColuna = false;
                                    }

                                    if (bloco.bloco.Equals("C170"))
                                    {
                                        if (rsBloco.GetName(i).Equals("VLR_ABAT_NT"))
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
                                            if (strCampo.IndexOf(",") >= 0)
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
                                if (gerarArquivoLinha)
                                {
                                    ArqTxt.AppendLine(strArqBloco.ToString());
                                    ArquivosBloco arq = new ArquivosBloco(usr, dtDe, dtAte, "PISCOFINS");
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
                            ArqTxt.AppendLine(strArqBloco.ToString().Trim());

                            if (bGerar)
                            {
                                if (bloco.gerarArquivo)
                                {

                                    ArquivosBloco arq = new ArquivosBloco(usr, dtDe, dtAte, "PISCOFINS");
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
            catch (Exception err)
            {

                Session.Remove("erroProcessamento");
                Session.Add("erroProcessamento", err.Message + " Bloco:" + bloco.bloco);
                showMessage(err.Message, true);
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


        protected void natezaReceita(String cstSaida, Decimal vlr, String codNatureza)
        {
            String strSql = "INSERT INTO EFD_M400 (REG, CST, VL_TOT_REC) VALUES ('M400', '" + cstSaida + "', " + vlr.ToString().Replace(".", "").Replace(",", ".") + ")";
            Conexao.executarSql(strSql);

            strSql = "INSERT INTO EFD_M410 (REG, NAT_REC, VL_TOT_REC, CST) VALUES ('M410','" + codNatureza + "'," + vlr.ToString().Replace(".", "").Replace(",", ".") + ", '" + cstSaida + "')";
            Conexao.executarSql(strSql);

        }


        protected void TimerProcessa_Tick(object sender, EventArgs e)
        {


            String erro = (String)Session["erroProcessamento"];
            if (erro != null)
            {
                showMessage(erro, true);
                
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
                Decimal.TryParse(strPorcItem, out  vPorcItem);

                Decimal porc = 0;
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
                    //TimerProcessa.Enabled = false;
                    lblProgressItens.Text = "Calculando Totais";
                    lblProgressTotal.Text = "99%";
                    barraItens.Attributes.Remove("style");
                    barraItens.Attributes.Add("style", "width: 99%;");
                    //lblProgressItens.Text = "";
                    //habilita(true);
                    ////carregarchkTelas(true);
                }


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

                String fim = (String)Session["fimProcesso"];
                if (fim != null)
                {
                    TimerProcessa.Enabled = false;
                    habilita(true);

                    lblProgressTotal.Text = "TODOS ARQUIVOS GERADOS";

                    barraItens.Attributes.Remove("style");
                    barraItens.Attributes.Add("style", "width: 100%;");


                    barraTotal.Attributes.Remove("style");
                    barraTotal.Attributes.Add("style", "width: 100%;");
                    btnCancelaProcesso.Visible = false;
                }



            }

        }

        protected void btnCancelaProcesso_Click(object sender, EventArgs e)
        {
            TimerProcessa.Enabled = false;
            modalConfirmaCancelamento.Show();
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

        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                User usr = (User)Session["User"];

                DateTime dtDe = new DateTime();
                DateTime dtAte = new DateTime();

                DateTime.TryParse(txtDe.Text, out dtDe);
                DateTime.TryParse(txtAte.Text, out dtAte);
                ArquivosBloco arq = new ArquivosBloco(usr, dtDe, dtAte, "PISCOFINS");
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