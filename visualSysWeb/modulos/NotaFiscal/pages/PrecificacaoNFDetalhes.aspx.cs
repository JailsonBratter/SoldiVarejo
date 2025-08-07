using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using visualSysWeb.code;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;

namespace visualSysWeb.modulos.NotaFiscal.pages
{
    public partial class PrecificacaoNFDetalhes : visualSysWeb.code.PagePadrao
    {

        static DataTable tb;
        //static String sql = "";
        string strCodigo = "";
        string strFornecedor = "";
        int intSerie = 0;

        static String ultimaOrdem = "";
        Hashtable FamiliasAtualizadas = new Hashtable();
        User usr = new User();

        protected void Page_Load(object sender, EventArgs e)
        {
            usr = (User)Session["User"];
            try
            {
                //MercadoriaDAO merc;
                EnabledControls(cabecalho, false);
                if (Request.Params["codigo"] != null && Request.Params["fornecedor"] != null && Request.Params["serie"] != null)
                {
                    strCodigo = Request.Params["codigo"].ToString();
                    strFornecedor = Request.Params["fornecedor"].ToString().Replace("|E", "&");
                    intSerie = int.Parse(Request.Params["serie"].ToString());
                    if (!IsPostBack)
                    {
                        nfDAO nf = new nfDAO(strCodigo, "2", strFornecedor, intSerie, usr);
                        Session.Remove("nf" + urlSessao());
                        Session.Add("nf" + urlSessao(), nf);

                        carregardados();
                        chkTodasFilias.Enabled = !nf.Estado;
                        if (nf.Estado)
                        {
                            status = "sopesquisa";
                            btnMargem.Visible = false;
                        }   
                        else
                            status = "editar";
                    }
                    carregabtn(pnBtn);

                }


            }
            catch (Exception err)
            {

                lblError.Text = "Ocorreu um erro: " + err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
                carregabtn(pnBtn);

            }
        }

        private void carregardados()
        {

            carregardados("nf_item.num_item");
        }
        private void carregardados(String ordem)
        {

            String strOrdem = "";
            if (ordem.Equals(""))
            {
                strOrdem = " order by convert(int,nf_item.num_item) ";
            }
            else
            {
                if (ordem.Equals("mercadoria.PLU"))
                {
                    strOrdem = " order by convert(int,mercadoria.PLU) ";
                }
                else
                {
                    strOrdem = " order By " + ordem;
                }
            }

            if (ordem.Equals(ultimaOrdem))
            {
                strOrdem += " Desc ";
                ultimaOrdem = "";
            }
            else
            {
                ultimaOrdem = ordem;
            }


            nfDAO nf = (nfDAO)Session["nf" + urlSessao()];
            txtCodigo.Text = nf.Codigo;
            txtFornecedor_CNPJ.Text = nf.Fornecedor_CNPJ;
            TxtNomeFornecedor.Text = nf.Nome_Fornecedor;
            txtEmissao.Text = nf.EmissaoBr();
            txtData.Text = nf.DataBr();
            txtTotal.Text = nf.Total.ToString("N2");
            txtDataPrecificacao.Text = nf.dataprecificacaoBR();
            txtUsuario.Text = nf.usuario;
            txtUsuario_Alteracao.Text = nf.usuario_Alteracao;
            txtUsuarioPrecificacao.Text = nf.usuarioPrecificacao;
            chkTodasFilias.Checked = nf.precoTodasFiliais;
            txtSerie.Text = nf.serie.ToString();

            //String sql = "Select  mercadoria.plu, mercadoria.descricao, isnull(mercadoria_loja.preco_custo,0)preco_custo, ISNULL(mercadoria_loja.preco_custo_1,0)preco_custo_1,(isnull(mercadoria_loja.preco_custo,0) - isnull(Mercadoria_Loja.Preco_Custo_1,0)) dif_custo,isnull(mercadoria_loja.margem,0)margem , "
            //         + "isnull(mercadoria_loja.preco,0) preco, convert(decimal(10,4), isnull(mercadoria_loja.preco_custo,0) * (1+ (isnull(mercadoria_loja.margem,0) / 100))) as Sugestao, "
            //         + "mercadoria.codigo_Familia from  nf_item inner join mercadoria on nf_item.plu = mercadoria.plu "
            //         + " left join Mercadoria_Loja on nf_item.PLU =mercadoria_loja.PLU and nf_item.Filial = Mercadoria_Loja.Filial "
            //         + " where Convert(Float, Codigo) =" + strCodigo
            //         + " and tipo_nf = 2"
            //         + " and cliente_fornecedor = '" + strFornecedor + "' "
            //         + " and nf_Item.serie = " + intSerie.ToString() + " "
            //         + strOrdem;

            String sql = "Select  mercadoria.plu, mercadoria.descricao, isnull(mercadoria_loja.preco_custo,0)preco_custo, ISNULL(mercadoria_loja.preco_custo_1,0)preco_custo_1,(isnull(mercadoria_loja.preco_custo,0) - isnull(Mercadoria_Loja.Preco_Custo_1,0)) dif_custo,isnull(dbo.F_MargemReferencia('" + usr.getFilial() + "', mercadoria_loja.PLU),0)margem , "
                     + "isnull(mercadoria_loja.preco,0) preco, convert(decimal(10,4), isnull(mercadoria_loja.preco_custo,0) * (1+ (isnull(dbo.F_MargemReferencia('" + usr.getFilial() + "', mercadoria_loja.PLU),0) / 100))) as Sugestao, "
                     + "mercadoria.codigo_Familia from  nf_item inner join mercadoria on nf_item.plu = mercadoria.plu "
                     + " left join Mercadoria_Loja on nf_item.PLU =mercadoria_loja.PLU and nf_item.Filial = Mercadoria_Loja.Filial "
                     + " where Convert(Float, Codigo) =" + strCodigo
                     + " and tipo_nf = 2"
                     + " and cliente_fornecedor = '" + strFornecedor + "' "
                     + " and nf_Item.serie = " + intSerie.ToString() + " "
                     + strOrdem;

            tb = Conexao.GetTable(sql, usr, false);
            gridItens.DataSource = tb;
            gridItens.DataBind();
            if (nf.Estado)
            {
                for (int i = 0; gridItens.Rows.Count > i; i++)
                {
                    TextBox txtBox1 = (TextBox)gridItens.Rows[i].FindControl("txtSugestao");
                    TextBox txtBox2 = (TextBox)gridItens.Rows[i].FindControl("txtMargem");
                    CheckBox chk = (CheckBox)gridItens.Rows[i].FindControl("chkSelecionaItem");
                    CheckBox chkH = (CheckBox)gridItens.HeaderRow.FindControl("chkSeleciona");
                    txtBox1.Enabled = false;
                    txtBox2.Enabled = false;
                    chk.Visible = false;
                    chkH.Visible = false;

                }
                chkTodasFilias.Enabled = false;
            }
            else
            {
                atualizaFamilias();
                chkTodasFilias.Enabled = true;

            }





        }

        protected void atualizaFamilias()
        {

            Hashtable tbFamilia = new Hashtable();
            for (int i = 0; gridItens.Rows.Count > i; i++)
            {
                String strFamilia = gridItens.Rows[i].Cells[8].Text.Trim().Replace("&nbsp;", "");
                if (!strFamilia.Equals(""))
                {
                    TextBox txtBox1 = (TextBox)gridItens.Rows[i].FindControl("txtSugestao");

                    try
                    {
                        tbFamilia.Add(strFamilia, txtBox1.Text);
                    }
                    catch (Exception)
                    {
                        TextBox txtBox2 = (TextBox)gridItens.Rows[i].FindControl("txtMargem");
                        Decimal dblCusto = Decimal.Parse(gridItens.Rows[i].Cells[3].Text);
                        Decimal novoValor = Decimal.Parse(txtBox1.Text);
                        Decimal margem = Funcoes.verificamargem(dblCusto, novoValor, 0, 0);
                        txtBox2.Text = margem.ToString("N4");
                        CheckBox chk = (CheckBox)gridItens.Rows[i].FindControl("chkSelecionaItem");
                        txtBox1.Text = tbFamilia[strFamilia].ToString();
                        txtBox1.Enabled = false;
                        txtBox2.Enabled = false;
                        chk.Visible = false;

                    }

                }

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
            Response.Redirect("PrecificacaoNF.aspx");
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            Decimal dblPrecoCusto = 0;
            Decimal dblMargem = 0;
            Decimal dblPrecoNovo = 0;
            String plu = "";
            String familia = "";

            FamiliasAtualizadas.Clear();
            //TextBox txtBox1 = (TextBox)sender;
            //GridViewRow row = gridItens.SelectedRow;
            //dblCusto = Convert.ToDouble(row.Cells[3].Text );

            int controleNumero = 0;

            try
            {
                for (int i = 0; i < gridItens.Rows.Count; i++)
                {
                    controleNumero = i;

                    plu = gridItens.Rows[i].Cells[0].Text;
                    familia = gridItens.Rows[i].Cells[8].Text.Trim().Replace("&nbsp;", "");
                    dblPrecoCusto = Decimal.Parse(gridItens.Rows[i].Cells[3].Text.Equals("") ? "0" : gridItens.Rows[i].Cells[3].Text);
                    TextBox txtBox2 = (TextBox)gridItens.Rows[i].FindControl("txtSugestao");
                    TextBox txtBox3 = (TextBox)gridItens.Rows[i].FindControl("txtMargem");
                    CheckBox chkAtualiza = (CheckBox)gridItens.Rows[i].FindControl("chkSelecionaItem");
                    dblPrecoNovo = Decimal.Parse((txtBox2.Text.Equals("") ? "0" : txtBox2.Text));
                    dblMargem = Decimal.Parse((txtBox3.Text.Equals("") ? "0" : txtBox3.Text));

                    if (plu.Equals("50937"))
                    {

                    }

                    Console.WriteLine(plu);

                    if (dblPrecoNovo >= 0 && (dblMargem >= 0 || dblMargem <= 0))
                    {
                        //CORRIGIR A MARGEM PARA SETA O VALOR CORRETO
                        dblMargem = (((dblPrecoNovo - dblPrecoCusto) / dblPrecoCusto) * 100);

                        Update(plu, dblPrecoCusto, dblPrecoNovo, dblMargem, familia, chkAtualiza.Checked);
                    }


                }

                //Atualiza o Estado da NF para Precificada.
                String sql = " UPDATE NF SET NF.Estado = 1,data_precificacao='" + DateTime.Now.ToString("yyyy-MM-dd") + "',usuario_precificacao='" + usr.getNome() + "', precoTodasFiliais=" + (chkTodasFilias.Checked ? "1" : "0") + " WHERE Filial = '" + usr.filial.Filial + "' " +
                      " AND cliente_fornecedor = '" + strFornecedor + "'" +
                      " AND Convert(Float, Codigo) =" + strCodigo +
                      " AND Serie = " + intSerie.ToString() + 
                      " AND tipo_nf = 2";

                Conexao.executarSql(sql);
                status = "visualizar";
                Response.Redirect("PrecificacaoNF.aspx", false);
                Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }


        protected void chkSelecionaItem_CheckedChanged(object sender, EventArgs e)
        {
            calculachk();
        }

        protected void chkSeleciona_CheckedChanged(object sender, EventArgs e)
        {

            foreach (GridViewRow item in gridItens.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                if (chk != null)
                {

                    chk.Checked = (sender as CheckBox).Checked;
                }
            }


            calculachk();
        }


        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("PrecificacaoNF.aspx");//colocar endereco pagina de pesquisa
            Session.Remove("obj" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
        }

        protected override bool campoDesabilitado(Control campo)
        {

            if (campo.ID.Equals("chkTodasFilias") && status.Equals("editar"))
                return false;
            else
                return true;
        }

        protected override bool campoObrigatorio(Control campo)
        {
            return false;
        }

        protected void gridItens_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridItens.EditIndex = e.NewEditIndex;
            carregardados(ultimaOrdem);
        }

        protected void gridItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }//Sinto Muito Me Perdoe Agradeço Eu Te Amo.

        protected void gridItens_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void calculachk()
        {
            Decimal dblCusto = 0;
            Decimal dblMargem = 0;
            Decimal dblPrecoNovo = 0;
            for (int i = 0; i < gridItens.Rows.Count; i++)
            {

                CheckBox chk = (CheckBox)gridItens.Rows[i].FindControl("chkSelecionaItem");
                if (chk != null)
                    if (!chk.Checked)
                    {
                        TextBox txtBox1 = (TextBox)gridItens.Rows[i].FindControl("txtSugestao");
                        txtBox1.Enabled = false;
                        TextBox txtMargem = (TextBox)gridItens.Rows[i].FindControl("txtMargem");
                        txtMargem.Enabled = false;

                        txtBox1.BackColor = Color.FromArgb(0xDCDCDC);
                        txtMargem.BackColor = Color.FromArgb(0xDCDCDC);

                        Decimal valor = Decimal.Parse(gridItens.Rows[i].Cells[6].Text);

                        txtBox1.Text = String.Format("{0:N2}", valor);
                        dblPrecoNovo = Decimal.Parse((txtBox1.Text.Equals("") ? "0" : txtBox1.Text));
                        dblCusto = Decimal.Parse(gridItens.Rows[i].Cells[3].Text);
                        Decimal NovaMargem = 0;

                        if (dblPrecoNovo >= 0 && dblCusto > 0)
                        {
                            //NovaMargem = ((dblPrecoNovo  - dblCusto) / dblCusto ) * 100;
                            NovaMargem = Funcoes.verificamargem(dblCusto, dblPrecoNovo, 0, 0);

                            txtMargem.Text = String.Format("{0:N4}", NovaMargem);
                            if (NovaMargem < 0)
                            {
                                txtMargem.ForeColor = System.Drawing.Color.Red;
                            }
                            else
                            {
                                txtMargem.ForeColor = System.Drawing.Color.Black;
                            }
                        }

                    }
                    else
                    {
                        TextBox txtBox1 = (TextBox)gridItens.Rows[i].FindControl("txtMargem");

                        txtBox1.BackColor = Color.White;

                        TextBox txtPrecoNovo = (TextBox)gridItens.Rows[i].FindControl("txtSugestao");
                        txtPrecoNovo.Enabled = true;
                        txtPrecoNovo.BackColor = Color.White;

                        //txtBox1.BackColor = Color.White;
                        Label lbl = (Label)gridItens.Rows[i].FindControl("lblMargem");
                        if (!txtBox1.Enabled && lbl != null)
                        {
                            txtBox1.Text = lbl.Text;
                        }

                        txtBox1.Enabled = true;
                        dblMargem = Decimal.Parse(txtBox1.Text.Equals("") ? "0" : txtBox1.Text);
                        dblCusto = Decimal.Parse(gridItens.Rows[i].Cells[3].Text);

                        if (dblMargem >= 0 && dblCusto > 0)
                        {
                            //dblPrecoNovo = (dblCusto * (1 + (dblMargem/100)));
                            dblPrecoNovo = Funcoes.verificapreco(dblMargem, dblCusto);
                            txtPrecoNovo.Text = String.Format("{0:N2}", dblPrecoNovo);
                        }
                        txtBox1.ForeColor = System.Drawing.Color.Black;
                    }
            }
            atualizaFamilias();


        }

        protected void sugestao()
        {

            Decimal dblCusto = 0;
            Decimal dblMargem = 0;
            Decimal dblPrecoNovo = 0;
            //TextBox txtBox1 = (TextBox)sender;

            //GridViewRow row = gridItens.SelectedRow;
            //dblCusto = Convert.ToDouble(row.Cells[3].Text );



            for (int i = 0; i < gridItens.Rows.Count; i++)
            {
                TextBox txtBox1 = (TextBox)gridItens.Rows[i].FindControl("txtSugestao");
                if (txtBox1.Enabled)
                {
                    dblPrecoNovo = Decimal.Parse((txtBox1.Text.Equals("") ? "0" : txtBox1.Text));
                    dblCusto = Decimal.Parse(gridItens.Rows[i].Cells[3].Text);
                    Decimal NovaMargem = 0;

                    if (dblPrecoNovo >= 0 && dblCusto > 0)
                    {
                        //NovaMargem = ((dblPrecoNovo  - dblCusto) / dblCusto ) * 100;
                        NovaMargem = Funcoes.verificamargem(dblCusto, dblPrecoNovo, 0, 0);
                        TextBox txtMargem = (TextBox)gridItens.Rows[i].FindControl("txtMargem");
                        Label lblMargem = (Label)gridItens.Rows[i].FindControl("lblMargem");
                        txtMargem.Text = String.Format("{0:N4}", NovaMargem);
                        lblMargem.Text = String.Format("{0:N4}", NovaMargem);
                    }
                    txtBox1.Text = String.Format("{0:N2}", dblPrecoNovo);
                }
            }
            atualizaFamilias();
        }
        protected void txtSugestao_textChanged(object sender, EventArgs e)
        {
            sugestao();
        }

        protected void margem()
        {
            Decimal dblCusto = 0;
            Decimal dblMargem = 0;
            Decimal dblPrecoNovo = 0;
            //TextBox txtBox1 = (TextBox)sender;

            //GridViewRow row = gridItens.SelectedRow;
            //dblCusto = Convert.ToDouble(row.Cells[3].Text );



            for (int i = 0; i < gridItens.Rows.Count; i++)
            {
                TextBox txtBox1 = (TextBox)gridItens.Rows[i].FindControl("txtMargem");

                dblMargem = Decimal.Parse((txtBox1.Text.Equals("") ? "0" : txtBox1.Text));
                dblCusto = Decimal.Parse(gridItens.Rows[i].Cells[3].Text);

                if (dblMargem >= 0 && dblCusto > 0)
                {
                    //dblPrecoNovo = (dblCusto * (1 + (dblMargem/100)));
                    dblPrecoNovo = Funcoes.verificapreco(dblMargem, dblCusto);
                    TextBox txtPrecoNovo = (TextBox)gridItens.Rows[i].FindControl("txtSugestao");
                    txtPrecoNovo.Text = String.Format("{0:N2}", dblPrecoNovo);
                }
            }
            atualizaFamilias();
        }

        protected void txtMargem_textChanged(object sender, EventArgs e)
        {
            margem();
        }


        protected void gridItens_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {

        }

        private void Update(String plu, Decimal precocusto, Decimal precovenda, Decimal margem, String familia, bool atualizaPreco)
        {
            try
            {

                String sql = "";

                if (familia.Trim().Length > 0 && isnumeroint(familia) && int.Parse(familia) > 0)
                {
                    bool familiaUpdate = false;
                    try
                    {
                        FamiliasAtualizadas.Add(familia.Trim(), "atualizada");
                        familiaUpdate = true;
                    }
                    catch (Exception)
                    {
                        familiaUpdate = false;
                    }

                    if (familiaUpdate)
                    {


                        sql = "update  Mercadoria set " +
                            " Margem =" + margem.ToString().Replace(",", ".");

                        if (atualizaPreco)
                        {
                            sql += ", Preco =" + precovenda.ToString().Replace(",", ".") +
                            ", imprime_etiqueta = 0" +
                            ", Etiqueta = 1" +
                            ", Estado_Mercadoria = 1" +
                            ", preco_atacado = case when isnull(preco_atacado,0) > 0 or (isnull(margem_atacado, 0) > 0 and isnull(qtde_atacado, 0) > 0)   then " +
                            "   (preco_custo * (1 + (margem_atacado / 100)))" +
                            "   else " +
                            "       0 " +
                            "   end" +
                            ", terceiro_preco = case when isnull(terceiro_preco,0) > 0 or isnull(margem_terceiro_preco, 0) > 0  then " +
                            "   (preco_custo * (1 + (margem_terceiro_preco / 100)))" +
                            "   else " +
                            "       0 " +
                            "   end";
                        }

                        sql += ", Data_Alteracao = '" + DateTime.Today.ToString("yyyy-MM-dd  HH:mm:ss") + "'" +
                                    "  where  plu='" + plu + "'";
                        Conexao.executarSqlCmd(sql);

                        if (atualizaPreco)
                        {
                            String sqlTb = "update Preco_Mercadoria " +
                                "set preco="+ Funcoes.decimalPonto(precovenda.ToString())+", Preco_promocao = case when desconto=0 then " + Funcoes.decimalPonto(precovenda.ToString()) + "  else " + Funcoes.decimalPonto(precovenda.ToString()) + "- ((" + Funcoes.decimalPonto(precovenda.ToString()) + " * desconto)/100) end" +
                                " where plu ='" + plu + "'";

                            Conexao.executarSqlCmd(sqlTb);

                        }


                        //Atualiza preço familia
                        String SqlUpFamilia = "update familia set " +
                                                        "preco=" + precovenda.ToString().Replace(",", ".") + " , " +
                                                        "imprime_etiqueta=1 " +
                                               "where codigo_familia='" + familia.Trim() + "'";
                        Conexao.executarSqlCmd(SqlUpFamilia);


                        //Atualiza preço dos itens da familia
                        String sqlFamilia = "select PLU,isnull(preco_custo,0)preco_custo from mercadoria where Codigo_Familia = '" + familia + "' ";
                        ArrayList sqlExecutar = new ArrayList();
                        SqlDataReader rsFamilia = null;
                        try
                        {


                            rsFamilia = Conexao.consulta(sqlFamilia, usr, false);
                            while (rsFamilia.Read())
                            {
                                if (!plu.Equals(rsFamilia["plu"].ToString()))
                                {
                                    Decimal fmcusto = Decimal.Parse(rsFamilia["preco_custo"].ToString());
                                    Decimal fmMargem = Funcoes.verificamargem(fmcusto, precovenda, 0, 0);
                                    sqlExecutar.Add(
                                        "update mercadoria set " +
                                        "   margem =" + fmMargem.ToString().Replace(",", ".") + ", " +
                                        "   preco=" + precovenda.ToString().Replace(",", ".") + "," +
                                        "   Estado_Mercadoria = 1 " +
                                        "   ,preco_atacado  = case when isnull(preco_atacado,0) > 0 or (isnull(margem_atacado, 0) > 0 and isnull(qtde_atacado, 0) > 0)  then " +
                                        "       ("+ fmcusto.ToString().Replace(",", ".") + " * (1 + (margem_atacado / 100))) " +
                                        "   else" +
                                        "       0 " +
                                        "   end" +
                                        " , terceiro_preco = case when isnull(terceiro_preco, 0) > 0 or isnull(margem_terceiro_preco, 0) > 0  then " +
                                        "   (" + fmcusto.ToString().Replace(",", ".") + " * (1 + (margem_terceiro_preco / 100)))" +
                                        "   else " +
                                        "       0 " +
                                        "   end" +
                                        " where plu='" + rsFamilia["plu"].ToString() + "'");
                                    //Conexao.executarSql("update mercadoria set margem =" + fmMargem.ToString().Replace(",", ".") + ", preco=" + precovenda.ToString().Replace(",", ".") + " where plu='" + rsFamilia["plu"].ToString() + "'");
                                }
                                Decimal fmCustoLoja = Decimal.Parse(Conexao.retornaUmValor("select isnull(preco_custo,0) from mercadoria_loja   where  PLU = '" + rsFamilia["plu"].ToString() + "' and filial='" + usr.getFilial() + "'", null));
                                Decimal fmMargemLoja = Funcoes.verificamargem(fmCustoLoja, precovenda, 0, 0);

                                sql = "update  Mercadoria_loja set " +
                                         " Margem = " + fmMargemLoja.ToString().Replace(",", ".");

                                if (atualizaPreco)
                                {
                                    sql += ", Preco =" + precovenda.ToString().Replace(",", ".");
                                    String strVericaAlteracao = "select isnull(preco,0) from mercadoria_loja where PLU= '" + rsFamilia["plu"].ToString() + "' and Filial='" + usr.getFilial() + "'";
                                    Decimal vPrecoAnt = Decimal.Parse(Conexao.retornaUmValor(strVericaAlteracao, null));
                                    if ((precovenda - vPrecoAnt) != 0)
                                    {
                                        sqlExecutar.Add("insert into log_preco (plu,descricao,data,usuario,preco_old,preco_new,filial) " +
                                            " values ('" + rsFamilia["plu"].ToString() + "','Precif Nota FAMILIA:" + familia.Trim() + "',GETDATE(),'" + usr.getNome() + "'," + vPrecoAnt.ToString().Replace(",", ".") + "," + precovenda.ToString().Replace(",", ".") + ",'" + (chkTodasFilias.Checked ? "TODAS" : usr.getFilial()) + "')");
                                        //Conexao.executarSql("insert into log_preco (plu,descricao,data,usuario,preco_old,preco_new,filial) " +
                                        //    " values ('" + rsFamilia["plu"].ToString() + "','Precif Nota FAMILIA:" + familia.Trim() + "',GETDATE(),'" + usr.getNome() + "'," + vPrecoAnt.ToString().Replace(",", ".") + "," + precovenda.ToString().Replace(",", ".") + ",'" + (chkTodasFilias.Checked ? "TODAS" : usr.getFilial()) + "')");
                                    }

                                }
                                sql += ", preco_atacado = case when (isnull(margem_atacado, 0) > 0 and isnull(qtde_atacado, 0) > 0)  then " +
                                        "       (preco_custo * (1 + (margem_atacado / 100)))" +
                                        "   else " +
                                        "       0 " +
                                        "   end" +
                                    ", Data_Alteracao = '" + DateTime.Today.ToString("yyyy-MM-dd") + "'" +
                                         "  where  PLU = '" + rsFamilia["plu"].ToString() + "' and filial='" + usr.getFilial() + "'";



                                sqlExecutar.Add(sql);



                                String sqlTb = "update Preco_Mercadoria " +
                                    "set preco="+ Funcoes.decimalPonto(precovenda.ToString())+", Preco_promocao = case when desconto=0 then " + Funcoes.decimalPonto(precovenda.ToString()) + "  else " + Funcoes.decimalPonto(precovenda.ToString()) + "- ((" + Funcoes.decimalPonto(precovenda.ToString()) + " * desconto)/100) end " +
                                    " where plu ='" + rsFamilia["plu"].ToString() + "' and filial ='" + usr.getFilial().ToString() + "'";

                                sqlExecutar.Add(sqlTb);



                                if (chkTodasFilias.Checked)
                                {
                                    SqlDataReader rsFiliais = null;
                                    try
                                    {


                                        rsFiliais = Conexao.consulta("Select preco_custo,filial from mercadoria_loja where plu='" + rsFamilia["plu"].ToString() + "' and filial <>'" + usr.getFilial() + "'", null, false);
                                        while (rsFiliais.Read())
                                        {
                                            Decimal pCusto = rsFiliais["preco_custo"].ToString().Equals("") ? 0 : Decimal.Parse(rsFiliais["preco_custo"].ToString());
                                            Decimal vMargem = Funcoes.verificamargem(pCusto, precovenda, 0, 0);

                                            sqlExecutar.Add("update mercadoria_loja set Preco =" + precovenda.ToString().Replace(",", ".") + ", margem=" + vMargem.ToString().Replace(",", ".") +
                                                            ",preco_atacado = case when (isnull(margem_atacado, 0) > 0 and isnull(qtde_atacado, 0) > 0)  then " +
                                                            "       (" + pCusto.ToString().Replace(",", ".") + " * (1 + (margem_atacado / 100))) " +
                                                            "   else " +
                                                            "       0 " +
                                                            "   end" +
                                                            "  where plu='" + rsFamilia["plu"].ToString() + "' and Filial='" + rsFiliais["filial"].ToString() + "'");
                                            //Conexao.executarSql("update mercadoria_loja set Preco =" + precovenda.ToString().Replace(",", ".") +", margem=" + vMargem.ToString().Replace(",", ".") + " where plu='" + rsFamilia["plu"].ToString() + "' and Filial='" + rsFiliais["filial"].ToString() + "'");

                                            sqlTb = "update Preco_Mercadoria " +
                                                "set preco="+Funcoes.decimalPonto(precovenda.ToString())+", Preco_promocao = case when desconto=0 then " + Funcoes.decimalPonto(precovenda.ToString()) + "  else " + Funcoes.decimalPonto(precovenda.ToString()) + "- ((" + Funcoes.decimalPonto(precovenda.ToString()) + " * desconto)/100)  end " +
                                                " where plu ='" + rsFamilia["plu"].ToString() + "' and filial ='" + rsFiliais["filial"].ToString() + "'";

                                            sqlExecutar.Add(sqlTb);



                                        }
                                    }
                                    catch (Exception)
                                    {

                                        throw;
                                    }
                                    finally
                                    {
                                        if (rsFiliais != null)
                                            rsFiliais.Close();

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
                            if (rsFamilia != null)
                                rsFamilia.Close();

                        }
                        if (sqlExecutar.Count > 0)
                        {
                            SqlConnection conn = Conexao.novaConexao();
                            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                            try
                            {
                                foreach (String item in sqlExecutar)
                                {
                                    Conexao.executarSql(item, conn, tran);
                                }
                                tran.Commit();
                            }
                            catch (Exception err)
                            {
                                tran.Rollback();
                                throw err;
                            }
                            finally
                            {

                                if (conn != null)
                                    conn.Close();
                            }
                        }

                    }
                }


                else
                {


                    // Atualiza as informações na tabela de Mercadoria. [Margem, Preço, Etiqueta, Data de Alteração]
                    sql = "update Mercadoria set " +
                                   " Margem =" + margem.ToString().Replace(",", ".");
                    if (atualizaPreco)
                    {
                        sql += ", Preco =" + precovenda.ToString().Replace(",", ".") +
                            ", Etiqueta = 1" +
                            ", imprime_etiqueta = 1" +
                            ", Estado_Mercadoria = 1" +
                            ", preco_atacado = case when isnull(preco_atacado,0) > 0 or (isnull(margem_atacado, 0) > 0 and isnull(qtde_atacado, 0) > 0) then " +
                            "       ("+ precocusto.ToString().Replace(",", ".") + " * (1 + (margem_atacado / 100)))" +
                            "   else " +
                            "       0 " +
                            "   end" +
                            ",  terceiro_preco = case when isnull(terceiro_preco,0) > 0 or isnull(terceiro_preco, 0) > 0  then " +
                            "   ("+ precocusto.ToString().Replace(",", ".") + " * (1 + (margem_terceiro_preco / 100)))" +
                            "   else " +
                            "       0 " +
                            "   end";
                    }
                    sql += ", Data_Alteracao = '" + DateTime.Today.ToString("yyyy-MM-dd") + "'" +
                            "  where PLU = '" + plu + "'";

                    Conexao.executarSqlCmd(sql);

                    sql = "update  Mercadoria_loja set " +
                                   " Margem =" + margem.ToString().Replace(",", ".");

                    if (atualizaPreco)
                    {
                        sql += ", Preco =" + precovenda.ToString().Replace(",", ".");

                        Decimal vPrecoAnt = 0m;
                        Decimal vCustoAnt = 0m;
                        Decimal vCustoAtu = 0m;
                        try
                        {
                            DataTable dtAnteriores = Conexao.GetTable("SELECT TOP 1 ISNULL(PRECO, 0) AS Preco, ISNULL(Preco_Custo, 0) As Custo, ISNULL(Preco_Custo_1, 0) AS CustoAnt FROM  mercadoria_loja WHERE Filial='" + usr.getFilial() + "' AND PLU= '" + plu + "'", usr, false);
                            foreach (DataRow row in dtAnteriores.Rows)
                            {
                                Decimal.TryParse(row["Preco"].ToString(), out vPrecoAnt);
                                Decimal.TryParse(row["Custo"].ToString(), out vCustoAtu);
                                Decimal.TryParse(row["CustoAnt"].ToString(), out vCustoAnt);
                            }
                        }
                        catch
                        {

                        }

                        //String strVericaAlteracao = "select isnull(preco,0) from mercadoria_loja where PLU= '" + plu + "' and Filial='" + usr.getFilial() + "'";
                        //Decimal vPrecoAnt = Decimal.Parse(Conexao.retornaUmValor(strVericaAlteracao, null));
                        if ((precovenda - vPrecoAnt) != 0)
                        {
                            Conexao.executarSql("insert into log_preco (plu,descricao,data,usuario,preco_old,preco_new,filial, custo_old, custo_new) " +
                                " values ('" + plu + "','Precificação Nota',GETDATE(),'" + usr.getNome() + "'," + vPrecoAnt.ToString().Replace(",", ".") + "," + precovenda.ToString().Replace(",", ".") + ",'" + (chkTodasFilias.Checked ? "TODAS" : usr.getFilial()) + "'," +
                                vCustoAnt.ToString().Replace(",",".") + ", " + vCustoAtu.ToString().Replace(",",".") + ")");
                        }

                    }

                    sql += ", Data_Alteracao = '" + DateTime.Today.ToString("yyyy-MM-dd") + "'" +
                           ", preco_atacado = case when (isnull(margem_atacado, 0) > 0 and isnull(qtde_atacado, 0) > 0)  then " +
                           "   (preco_custo * (1 + (margem_atacado / 100))) " +
                           "   else " +
                            "       0 " +
                            "   end" +
                    "  where PLU = '" + plu + "' and filial='" + usr.getFilial() + "'";

                    Conexao.executarSql(sql);

                    if (chkTodasFilias.Checked)
                    {

                        SqlDataReader rsFiliais = null;
                        try
                        {


                            rsFiliais = Conexao.consulta("Select preco_custo,filial from mercadoria_loja where plu='" + plu + "' and filial <>'" + usr.getFilial() + "'", null, false);

                            while (rsFiliais.Read())
                            {
                                Decimal pCusto = rsFiliais["preco_custo"].ToString().Equals("") ? 0 : Decimal.Parse(rsFiliais["preco_custo"].ToString());
                                Decimal vMargem = Funcoes.verificamargem(pCusto, precovenda, 0, 0);

                                Conexao.executarSql("update mercadoria_loja set Preco =" + precovenda.ToString().Replace(",", ".") + ", margem=" + vMargem.ToString().Replace(",", ".") +
                                     ", preco_atacado = case when (isnull(margem_atacado, 0) > 0 and isnull(qtde_atacado, 0) > 0)  then " +
                                    "   (preco_custo * (1 + (margem_atacado / 100))) " +
                                    "   else " +
                                    "       0 " +
                                    "   end" +
                                    " where plu='" + plu + "' and Filial='" + rsFiliais["filial"].ToString() + "'");

                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        finally
                        {
                            if (rsFiliais != null)
                                rsFiliais.Close();

                        }

                        if (atualizaPreco)
                        {
                            String sqlTb = "update Preco_Mercadoria " +
                                "set preco="+ Funcoes.decimalPonto(precovenda.ToString())+", Preco_promocao =case when desconto=0 then " + Funcoes.decimalPonto(precovenda.ToString()) + "  else " + Funcoes.decimalPonto(precovenda.ToString()) + "- ((" + Funcoes.decimalPonto(precovenda.ToString()) + " * desconto)/100) end " +
                                " where plu ='" + plu + "'";

                            Conexao.executarSql(sqlTb);

                        }
                    }
                    else
                    {

                        if (atualizaPreco)
                        {
                            String sqlTb = "update Preco_Mercadoria " +
                                "set preco="+ Funcoes.decimalPonto(precovenda.ToString()) + ", Preco_promocao =case when desconto=0 then " + Funcoes.decimalPonto(precovenda.ToString()) + "  else " + Funcoes.decimalPonto(precovenda.ToString()) + "- ((" + Funcoes.decimalPonto(precovenda.ToString()) + " * desconto)/100) end " +
                                " where plu ='" + plu + "' and filial='" + usr.getFilial() + "'";

                            Conexao.executarSql(sqlTb);

                        }
                    }



                }


                //Atualização de produtos RELACIONADOS.
                //** Esta rotina tem como finalidade atualizar os preços de acordo com os custo contidos em suas margens
                if (atualizaPreco)
                {
                    SqlConnection connRel = Conexao.novaConexao();
                    SqlTransaction tranRel = connRel.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

                    try
                    {

                        SqlDataReader rsVinculados = null;
                        Decimal decQtdeBase = 0;
                        Decimal decRelCusto = 0;
                        Decimal decRelMargem = 0;
                        Decimal decRelPreco = 0;
                        Decimal decRelFator = 0;
                        Decimal decRelNovaMargem = 0;
                        Decimal decRelNovoCusto = 0;
                        Decimal decRelNovoPreco = 0;
                        string strRelPLU = "";

                        rsVinculados = Conexao.consulta("sp_cons_PLUsVinculado '" + plu + "'", null, false);

                        while (rsVinculados.Read())
                        {
                            Decimal.TryParse(rsVinculados["preco_custo"].ToString(), out decRelCusto);
                            Decimal.TryParse(rsVinculados["margem"].ToString(), out decRelMargem);
                            Decimal.TryParse(rsVinculados["preco"].ToString(), out decRelPreco);
                            Decimal.TryParse(rsVinculados["fator"].ToString(), out decRelFator);
                            Decimal.TryParse(rsVinculados["QtdeBase"].ToString(), out decQtdeBase);
                            strRelPLU = rsVinculados["PLU"].ToString();

                            decRelNovoCusto = (precocusto / (decQtdeBase <= 1 ? 1 : decQtdeBase)) * (decRelFator <= 0 ? 1 : decRelFator); //Define qual o preco do novo custo
                            //decRelNovoPreco = Decimal.Parse(((precovenda / (decQtdeBase <= 1 ? 1 : decQtdeBase)) * (decRelFator <= 0 ? 1 : decRelFator)).ToString("N2"));
                            decRelNovoPreco = Decimal.Parse((decRelNovoCusto * (1 + (decRelMargem / 100))).ToString("N2"));
                            decRelNovaMargem = Funcoes.verificamargem(decRelNovoCusto, decRelNovoPreco, 0, 0);

                            sql = "update Mercadoria set preco_custo = " + decRelNovoCusto.ToString().Replace(",", ".") +
                                           ", Margem =" + decRelNovaMargem.ToString().Replace(",", ".");
                            sql += ", Preco =" + decRelNovoPreco.ToString().Replace(",", ".") +
                                ", Etiqueta = 1" +
                                ", imprime_etiqueta = 1" +
                                ", Estado_Mercadoria = 1" +
                                ", preco_atacado = case when isnull(preco_atacado,0) > 0 or (isnull(margem_atacado, 0) > 0 and isnull(qtde_atacado, 0) > 0)  then " +
                                "       (" + decRelNovoCusto.ToString().Replace(",", ".") + " * (1 + (margem_atacado / 100)))" +
                                "   else " +
                                "       0 " +
                                "   end" +
                                ",  terceiro_preco = case when isnull(terceiro_preco,0) > 0 or isnull(margem_terceiro_preco, 0) > 0 then " +
                                "   (" + decRelNovoCusto.ToString().Replace(",", ".") + " * (1 + (margem_terceiro_preco / 100)))" +
                                "   else " +
                                "       0 " +
                                "   end";
                            sql += ", Data_Alteracao = '" + DateTime.Today.ToString("yyyy-MM-dd") + "'" +
                                    "  where PLU = '" + strRelPLU + "'";

                            Conexao.executarSql(sql, connRel, tranRel);


                            sql = "update Mercadoria_loja set preco_custo_2 = preco_custo_1, preco_custo_1 = preco_custo, preco_custo = " + decRelNovoCusto.ToString().Replace(",", ".") +
                                           ", Margem =" + decRelNovaMargem.ToString().Replace(",", ".");
                            sql += ", Preco =" + decRelNovoPreco.ToString().Replace(",", ".");
                            sql += ", preco_atacado = case when isnull(preco_atacado,0) > 0 or (isnull(margem_atacado, 0) > 0 and isnull(qtde_atacado, 0) > 0)  then " +
                                "       (" + decRelNovoCusto.ToString().Replace(",", ".") + " * (1 + (margem_atacado / 100))) " + "   else " + "       0 " + "   end";


                            if (chkTodasFilias.Checked)
                            {
                                sql += " WHERE PLU = '" + strRelPLU + "'";
                            }
                            else
                            {
                                sql += " WHERE Mercadoria_Loja.Filial = '" + usr.getFilial() + "' AND Mercadoria_Loja.PLU = '" + strRelPLU + "'";
                            }

                            Conexao.executarSql(sql, connRel, tranRel);

                            //Grava informação na tabela LOG_PRECO

                            if ((decRelPreco - decRelNovoPreco) != 0)
                            {
                                Conexao.executarSql("insert into log_preco (plu,descricao,data,usuario,preco_old,preco_new,filial) " +
                                    " values ('" + plu + "','Precificação Nota PLU VINCULADO',GETDATE(),'" + usr.getNome() + "'," + decRelPreco.ToString().Replace(",", ".") + "," + decRelNovoPreco.ToString().Replace(",", ".") + ",'" + (chkTodasFilias.Checked ? "TODAS" : usr.getFilial()) + "')");
                            }
                        }



                        tranRel.Commit();
                    }
                    catch (Exception err)
                    {
                        tranRel.Rollback();
                        throw err;
                    }
                    finally
                    {
                        if (connRel != null)
                        {
                            connRel.Close();
                            connRel.Dispose();
                        }

                    }

                }
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }

        }

        protected void gridItens_Sorting(object sender, GridViewSortEventArgs e)
        {
            carregardados(e.SortExpression);
        }

        protected void btnMargem_Click(object sender, EventArgs e)
        {
            txtMargemPadrao.Text = "0,0000";
            txtMargemPadrao.Focus();

            modalMargem.Show();
        }

        protected void btnCancelaMargem_Click(object sender, ImageClickEventArgs e)
        {
            modalMargem.Hide();
        }

        protected void btnConfirmaMargem_Click(object sender, ImageClickEventArgs e)
        {
            Decimal vlor = Funcoes.decTry(txtMargemPadrao.Text);
            foreach (GridViewRow row in gridItens.Rows)
            {
                TextBox txt = (TextBox)row.FindControl("txtMargem");
                txt.Text = vlor.ToString("N4");
            }
            margem();
            modalMargem.Hide();
        }
    }
}