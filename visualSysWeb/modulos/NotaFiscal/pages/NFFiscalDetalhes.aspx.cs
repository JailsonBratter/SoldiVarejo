using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.NotaFiscal.pages
{
    public partial class NFFiscalDetalhes : visualSysWeb.code.PagePadrao
    {
        string nota = "", fornecedor = "";
        int serie = 0;
        decimal dectotbcicms = 0, dectoticms = 0, dectotbcpiscofins = 0, dectotpis = 0, dectotcofins = 0;
        decimal dectotBCST = 0, dectotValorST = 0, dectotValorIPI = 0;
        natureza_operacaoDAO nat;

        protected void Page_Load(object sender, EventArgs e)
        {
            nota = Request.Params["codigo"];
            fornecedor = Request.Params["fornecedor"];
            int.TryParse(Request.Params["serie"], out serie);

            User usr = (User)Session["User"];
            nfDAO nf = new nfDAO(nota, "2", fornecedor, serie, usr);
            nat = new natureza_operacaoDAO(nf.Codigo_operacao.ToString(), null);

            lblValidacaoFiscal.Text = (nf.validacaoFiscal.Equals("") ? "NFe NÃO CONFERIDA" : nf.validacaoFiscal);

            if (!IsPostBack)
            {
                lblSN.Visible = false;
                pesquisar();

                status = "editar";
            }
            carregabtn(pnBtn);

            txtTBCICMS.Enabled = false;
            txtTValorICMS.Enabled = false;
            txtTBCPISCofins.Enabled = false;
            txtTValorCofins.Enabled = false;
            txtTValorPPIS.Enabled = false;
            txtBCST.Enabled = false;
            txtValorST.Enabled = false;
            txtValorIPI.Enabled = false;
        }

        protected void pesquisar()
        {

            User usr = (User)Session["User"];
            if (usr != null)
            {
                string sql = "sp_br_Cons_NFe_DadosFiscais '" + usr.getFilial() + "', '" + fornecedor + "', '" + nota + "', " + serie.ToString();

                //DataTable dt = Conexao.GetTable(sql, usr, false);

                gridPesquisa.DataSource = Conexao.GetTable(sql, usr, false);
                gridPesquisa.DataBind();
            }
        }
        protected void txtDataDe_TextChanged(object sender, EventArgs e)
        {
            //if (IsDate(txtDataDe.Text))
            //{
            //    txtDataAte.Text = txtDataDe.Text;
            //}
        }
        protected void gridPesquisa_Sorting(object sender, GridViewSortEventArgs e)
        {

            //pesquisar(e.SortExpression);

        }
        protected void gridPesquisa_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //int index = Convert.ToInt32(e.CommandArgument);

            //LinkButton lkNota = (LinkButton)gridPesquisa.Rows[index].Cells[0].Controls[0];
            //String nota = lkNota.Text;

            //LinkButton lkSerie = (LinkButton)gridPesquisa.Rows[index].Cells[1].Controls[0];
            //String serie = lkSerie.Text.Trim();

            //LinkButton lkFornecedor = (LinkButton)gridPesquisa.Rows[index].Cells[2].Controls[0];
            //String fornecedor = lkFornecedor.Text.Trim();

            //RedirectNovaAba("~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?codigo=" + nota + "&fornecedor=" + fornecedor + "&serie=" + serie);
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            editar(pnBtn);
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisar();
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void btnCorrigir_Click(object sender, EventArgs e)
        {

        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            string csticms = "", cfop = "", cstpiscofins = "";
            decimal decaliqicms = 0, decbcicms = 0, decvlricms = 0, decredutor = 0, decbcpiscofins = 0,
                decaliqpis = 0, decaliqcofins = 0, decvlrpis = 0, decvlrcofins = 0, decaliqsn = 0, decvalorsn = 0;

            dectotbcicms = 0;
            dectoticms = 0;
            dectotbcpiscofins = 0;
            dectotpis = 0;
            dectotcofins = 0;
            dectotBCST = 0;
            dectotValorST = 0;
            dectotValorIPI = 0;

            string plu = "", sequencia = "";
            User usr = (User)Session["User"];

            for (int i = 0; i < gridPesquisa.Rows.Count; i++)
            {
                //Atribundo valores às variáveis com o conteudo dos textbox
                csticms = "";
                cfop = "";
                cstpiscofins = "";
                //Convertendo valores em variáveis
                decaliqicms = 0;
                decbcicms = 0;
                decredutor = 0;
                decvlricms = 0;
                decbcpiscofins = 0;
                decaliqpis = 0;
                decvlrpis = 0;
                decaliqcofins = 0;
                decvlrcofins = 0;
                decaliqsn = 0;
                decvalorsn = 0;

                sequencia = gridPesquisa.Rows[i].Cells[0].Text;
                plu = gridPesquisa.Rows[i].Cells[2].Text;

                //Criando TEXT Box
                TextBox TXTCSTICMS = (TextBox)gridPesquisa.Rows[i].FindControl("txtCSTICMS");
                TextBox TXTCFOP = (TextBox)gridPesquisa.Rows[i].FindControl("txtCFOP");
                TextBox TXTALIQICMS = (TextBox)gridPesquisa.Rows[i].FindControl("txtAliqICMS");
                TextBox TXTBCICMS = (TextBox)gridPesquisa.Rows[i].FindControl("txtBCICMS");
                TextBox TXTREDUTORBC = (TextBox)gridPesquisa.Rows[i].FindControl("txtRedutorBC");
                TextBox TXTVLRICMS = (TextBox)gridPesquisa.Rows[i].FindControl("txtVlrICMs");
                TextBox TXTCSTPISCOFINS = (TextBox)gridPesquisa.Rows[i].FindControl("txtCSTPisCofins");
                TextBox TXTBCPISCOFINS = (TextBox)gridPesquisa.Rows[i].FindControl("txtBCPisCofins");
                TextBox TXTALIQPIS = (TextBox)gridPesquisa.Rows[i].FindControl("txtAliqPIS");
                TextBox TXTVLRPIS = (TextBox)gridPesquisa.Rows[i].FindControl("txtVlrPIS");
                TextBox TXT1ALIQCOFINS = (TextBox)gridPesquisa.Rows[i].FindControl("txt1AliqCofins");
                TextBox TXTVLRCOFINS = (TextBox)gridPesquisa.Rows[i].FindControl("txtVlrcofins");
                TextBox TXTALIQSN = (TextBox)gridPesquisa.Rows[i].FindControl("txtAliqSN");
                TextBox TXTVALORSN = (TextBox)gridPesquisa.Rows[i].FindControl("txtValorSN");

                //Atribundo valores às variáveis com o conteudo dos textbox
                csticms = TXTCSTICMS.Text;
                cfop = TXTCFOP.Text;
                cstpiscofins = TXTCSTPISCOFINS.Text;
                //Convertendo valores em variáveis
                decaliqicms = Decimal.Parse((TXTALIQICMS.Text.Equals("") ? "0" : TXTALIQICMS.Text));
                decbcicms = Decimal.Parse((TXTBCICMS.Text.Equals("") ? "0" : TXTBCICMS.Text));
                decredutor = Decimal.Parse((TXTREDUTORBC.Text.Equals("") ? "0" : TXTREDUTORBC.Text));
                decvlricms = Decimal.Parse((TXTVLRICMS.Text.Equals("") ? "0" : TXTVLRICMS.Text));
                decbcpiscofins = Decimal.Parse((TXTBCPISCOFINS.Text.Equals("") ? "0" : TXTBCPISCOFINS.Text));
                decaliqpis = Decimal.Parse((TXTALIQPIS.Text.Equals("") ? "0" : TXTALIQPIS.Text));
                decvlrpis = Decimal.Parse((TXTVLRPIS.Text.Equals("") ? "0" : TXTVLRPIS.Text));
                decaliqcofins = Decimal.Parse((TXT1ALIQCOFINS.Text.Equals("") ? "0" : TXT1ALIQCOFINS.Text));
                decvlrcofins = Decimal.Parse((TXTVLRCOFINS.Text.Equals("") ? "0" : TXTVLRCOFINS.Text));
                decaliqsn = Decimal.Parse((TXTALIQSN.Text.Equals("") ? "0" : TXTALIQSN.Text));
                decvalorsn = Decimal.Parse((TXTVALORSN.Text.Equals("") ? "0" : TXTVALORSN.Text));

                //Atribuindo valores aos Principais.
                dectotbcicms += decbcicms;
                dectoticms += (decvlricms + decvalorsn); 
                dectotbcpiscofins += decbcpiscofins;
                dectotpis += decvlrpis;
                dectotcofins += decvlrcofins;


                //Exceuta a alteração
                update(sequencia, plu, csticms, cfop, cstpiscofins, decaliqicms, decbcicms, decredutor, decvlricms, decbcpiscofins, decaliqpis, decvlrpis, decaliqcofins, decvlrcofins, decaliqsn, decvalorsn);
            }

            string sql = "UPDATE NF SET ";
            sql += " Base_Calculo = " + dectotbcicms.ToString().Replace(",", ".");
            sql += ", ICMS_Nota = " + dectoticms.ToString().Replace(",", ".");
            sql += ", base_pis = " + dectotbcpiscofins.ToString().Replace(",", ".");
            sql += ", base_cofins = " + dectotbcpiscofins.ToString().Replace(",", ".");
            sql += ", pisv = " + dectotpis.ToString().Replace(",", ".");
            sql += ", cofinsv = " + dectotcofins.ToString().Replace(",", ".");
            sql += ", Validacao_Fiscal = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " Usuário: " + usr.getNome() + "'";
            sql += " WHERE Filial = '" + usr.getFilial() + "' AND Cliente_Fornecedor = '" + fornecedor + "' AND Codigo = '" + nota
                + "' AND Serie = " + serie.ToString();
            Conexao.executarSqlCmd(sql);

            //status = "visualizar";
            //Response.Redirect("NfEntrada.aspx", false);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "fechar", "fecharJanela();", true);
        }

        private void update(string _sequencia, string _plu, string _CSTICMS, string _CFOP, string _CSTPISC, decimal _aliquotaICMS,
            decimal _bcICMS, decimal _redutorBC, decimal _vlrICMS, decimal _bcPISCFOINS,
            decimal _aliquotaPIS, decimal _vlrPIS, decimal _aliquotaCofins, decimal _vlrCofins,
            decimal _aliqsn, decimal _valorsn)
        {
            User usr = (User)Session["User"];

            try
            {
                string sql = "UPDATE NF_Item SET ";
                sql += "Aliquota_Pis = " + _aliquotaPIS.ToString().Replace(",", ".");
                sql += ", Aliquota_Cofins = " + _aliquotaCofins.ToString().Replace(",", ".");
                sql += ", PISV = " + _vlrPIS.ToString().Replace(",", ".");
                sql += ", CofinsV = " + _vlrCofins.ToString().Replace(",", ".");
                sql += ", cstPIS = '" + _CSTPISC + "'";
                sql += ", CSTCofins = '" + _CSTPISC + "'";
                sql += ", Aliquota_ICMS = " + _aliquotaICMS.ToString().Replace(",", ".");
                sql += ", redutor_Base = " + _redutorBC.ToString().Replace(",", ".");
                sql += ", Codigo_Operacao = " + _CFOP;
                sql += ", CFOP = " + _CFOP;
                sql += ", base_pis = " + _bcPISCFOINS.ToString().Replace(",", ".");
                sql += ", base_cofins = " + _bcPISCFOINS.ToString().Replace(",", ".");
                sql += ", cst_ICMS = '" + _CSTICMS.Substring(1, _CSTICMS.Length - 1) + "'";
                sql += ", base_icms = " + _bcICMS.ToString().Replace(",", ".");
                sql += ", icmsv = " + _vlrICMS.ToString().Replace(",", ".");
                sql += ", pCredSN = " + _aliqsn.ToString().Replace(",", ".");
                sql += ", vCredICMSSN = " + _valorsn.ToString().Replace(",", ".");
                sql += " WHERE ";
                sql += " Filial = '" + usr.getFilial() + "' AND Cliente_Fornecedor = '" + fornecedor + "' AND Codigo = '" + nota + "' AND Serie = " + serie.ToString();
                sql += " AND PLU = '" + _plu + "' AND Num_Item = '" + _sequencia + "'";
                Conexao.executarSqlCmd(sql);
            }
            catch
            {
                throw;
            }
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "fechar", "fecharJanela();", true);
        }

        protected override bool campoDesabilitado(Control campo)
        {
            throw new NotImplementedException();
        }

        protected override bool campoObrigatorio(Control campo)
        {
            throw new NotImplementedException();
        }

        protected void gridPesquisa_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal decaliqicms = 0, decbcicms = 0, decvlricms = 0, decredutor = 0, decbcpiscofins = 0,
                decaliqpis = 0, decaliqcofins = 0, decvlrpis = 0, decvlrcofins = 0, decaliqsn = 0, decvalorsn = 0;

            decimal decBCST = 0, decValorST = 0, decValorIPI = 0;
            //Pegado dados do cadastro
            string plu = "";
            bool divergencia = false;

            //Convertendo valores em variáveis
            decaliqicms = 0;
            decbcicms = 0;
            decredutor = 0;
            decvlricms = 0;
            decbcpiscofins = 0;
            decaliqpis = 0;
            decvlrpis = 0;
            decaliqcofins = 0;
            decvlrcofins = 0;
            decaliqsn = 0;
            decvalorsn = 0;

            //Criando TEXT Box
            if (e.Row.RowIndex >= 0)
            {
                plu = e.Row.Cells[2].Text;
                MercadoriaTributario tributario = new MercadoriaTributario(plu);

                TextBox TXTCSTICMS = (TextBox)e.Row.FindControl("txtCSTICMS");
                TextBox TXTCFOP = (TextBox)e.Row.FindControl("txtCFOP");
                TextBox TXTALIQICMS = (TextBox)e.Row.FindControl("txtAliqICMS");
                TextBox TXTBCICMS = (TextBox)e.Row.FindControl("txtBCICMS");
                TextBox TXTREDUTORBC = (TextBox)e.Row.FindControl("txtRedutorBC");
                TextBox TXTVLRICMS = (TextBox)e.Row.FindControl("txtVlrICMs");
                TextBox TXTCSTPISCOFINS = (TextBox)e.Row.FindControl("txtCSTPisCofins");
                TextBox TXTBCPISCOFINS = (TextBox)e.Row.FindControl("txtBCPisCofins");
                TextBox TXTALIQPIS = (TextBox)e.Row.FindControl("txtAliqPIS");
                TextBox TXTVLRPIS = (TextBox)e.Row.FindControl("txtVlrPIS");
                TextBox TXT1ALIQCOFINS = (TextBox)e.Row.FindControl("txt1AliqCofins");
                TextBox TXTVLRCOFINS = (TextBox)e.Row.FindControl("txtVlrcofins");
                TextBox TXTALIQSN = (TextBox)e.Row.FindControl("txtAliqSN");
                TextBox TXTVALORSN = (TextBox)e.Row.FindControl("txtValorSN");
                Button btnCorrigir = (Button)e.Row.FindControl("btnCorrigir");

                //Convertendo valores em variáveis
                decaliqicms = Decimal.Parse((TXTALIQICMS.Text.Equals("") ? "0" : TXTALIQICMS.Text));
                decbcicms = Decimal.Parse((TXTBCICMS.Text.Equals("") ? "0" : TXTBCICMS.Text));
                decredutor = Decimal.Parse((TXTREDUTORBC.Text.Equals("") ? "0" : TXTREDUTORBC.Text));
                decvlricms = Decimal.Parse((TXTVLRICMS.Text.Equals("") ? "0" : TXTVLRICMS.Text));
                decbcpiscofins = Decimal.Parse((TXTBCPISCOFINS.Text.Equals("") ? "0" : TXTBCPISCOFINS.Text));
                decaliqpis = Decimal.Parse((TXTALIQPIS.Text.Equals("") ? "0" : TXTALIQPIS.Text));
                decvlrpis = Decimal.Parse((TXTVLRPIS.Text.Equals("") ? "0" : TXTVLRPIS.Text));
                decaliqcofins = Decimal.Parse((TXT1ALIQCOFINS.Text.Equals("") ? "0" : TXT1ALIQCOFINS.Text));
                decvlrcofins = Decimal.Parse((TXTVLRCOFINS.Text.Equals("") ? "0" : TXTVLRCOFINS.Text));
                decaliqsn = Decimal.Parse((TXTALIQSN.Text.Equals("") ? "0" : TXTALIQSN.Text));
                decvalorsn = Decimal.Parse((TXTVALORSN.Text.Equals("") ? "0" : TXTVALORSN.Text));
                //ICMS
                if (!nat.Incide_ICMS)
                {
                    if (TXTCSTICMS.Text.Substring(1,(TXTCSTICMS.Text.Length - 1)) != nat.cst_ICMS)
                    {
                        TXTCSTICMS.BackColor = System.Drawing.Color.Orange;
                        divergencia = true;
                    }
                }
                else
                {
                    if (TXTCSTICMS.Text != tributario.CST)
                    {
                        TXTCSTICMS.BackColor = System.Drawing.Color.Orange;
                        divergencia = true;
                    }
                }

                //Se o CFOP existe uma pré-configuração o sistema compara e apresenta erro.
                if (!nat.cfop.Equals("") && TXTCFOP.Text != nat.cfop)
                {
                    TXTCFOP.BackColor = System.Drawing.Color.Orange;
                    divergencia = true;
                }



                //PIS e Cofins
                if (!nat.incide_PisCofins)
                {
                    //Se o CST for diferente do cadastrado na Natureza de operação
                    if (TXTCSTPISCOFINS.Text != nat.cst_pis_cofins)
                    {
                        TXTCSTPISCOFINS.BackColor = System.Drawing.Color.Orange;
                        divergencia = true;
                    }
                    if (!TXTBCPISCOFINS.Text.Equals("0,00"))
                    {
                        TXTBCPISCOFINS.BackColor = System.Drawing.Color.Orange;
                        divergencia = true;
                    }
                }
                else
                {
                    if (TXTCSTPISCOFINS.Text != tributario.CSTPIS)
                    {
                        TXTCSTPISCOFINS.BackColor = System.Drawing.Color.Orange;
                        divergencia = true;
                    }
                    //Checa o valor está de acordo
                    if (TXTCSTPISCOFINS.Text.Equals("50") && decbcpiscofins <= 0)
                    {
                        TXTBCPISCOFINS.BackColor = System.Drawing.Color.Orange;
                        divergencia = true;
                    }
                    else if (!TXTCSTPISCOFINS.Text.Equals("50") && decbcpiscofins > 0)
                    {
                        TXTBCPISCOFINS.BackColor = System.Drawing.Color.Orange;
                        divergencia = true;
                    }
                }

                btnCorrigir.Visible = divergencia;

                decimal.TryParse(e.Row.Cells[12].Text, out decBCST);
                decimal.TryParse(e.Row.Cells[13].Text, out decValorST);
                decimal.TryParse(e.Row.Cells[15].Text, out decValorIPI);

                //Atribuindo valores aos Principais.
                dectotbcicms += decbcicms;
                dectoticms += (decvlricms + decvalorsn);
                dectotbcpiscofins += decbcpiscofins;
                dectotpis += decvlrpis;
                dectotcofins += decvlrcofins;

                dectotBCST += decBCST;
                dectotValorST += decValorST;
                dectotValorIPI += decValorIPI;

                txtTBCICMS.Text = dectotbcicms.ToString();
                txtTValorICMS.Text = dectoticms.ToString();
                txtTBCPISCofins.Text = dectotbcpiscofins.ToString();
                txtTValorPPIS.Text = dectotpis.ToString();
                txtTValorCofins.Text = dectotcofins.ToString();

                txtBCST.Text = dectotBCST.ToString();
                txtValorST.Text = dectotValorST.ToString();
                txtValorIPI.Text = dectotValorIPI.ToString();

                if (decvalorsn > 0)
                {
                    lblSN.Visible = true;
                }

            }
        }

        protected void ImgBtnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("RegistrosFiscais.aspx");
        }
    }
}