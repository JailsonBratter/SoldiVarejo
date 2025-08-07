using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using visualSysWeb.modulos.Cadastro.code;
using visualSysWeb.code;
using System.Collections;


namespace visualSysWeb.Cadastro
{
    public partial class Mercadoria : visualSysWeb.code.PagePadrao
    {

        String sql = "Select a.plu as PLU,isnull(b.EAN,'---') EAN, isnull(a.Ref_Fornecedor,'---') AS Refer, REPLACE(a.descricao,' ','_')  AS Mercadoria, l.Preco_Custo," +
                                "l.preco, preco_promocao = case when l.promocao=1then l.preco_promocao else 0 end, convert(varchar,l.Data_Alteracao,103)Data_Alteracao,ICMS_SAIDA = right(replicate('0',3)+isnull(ltrim(rtrim(t.Indice_ST)),'00'),3)+'-'+RIGHT( REPLICATE('0',4) + CONVERT(VARCHAR(10),CONVERT(MONEY,isnull(t.ICMS_Efetivo,0))), 5)  , a.cst_saida as PISCofins, isnull(a.cf,'') ncm  " +
                                " ,Saldo_atual = case when isnull((Select permite_item from tipo where tipo=a.tipo ),0)=1 then 0 else l.Saldo_atual end " +
                                ",l.margem,a.peso_bruto, ISNULL(Convert(varchar, l.Data_Inventario, 103), '1900.01.01') AS DataInventario " +
                                "   from mercadoria a  LEFT join ean b on a.plu=b.plu  inner join mercadoria_loja l on l.plu=a.plu	inner join Tributacao t on a.Codigo_Tributacao = t.Codigo_Tributacao  " +
                                 "  left join W_BR_CADASTRO_DEPARTAMENTO d on (a.codigo_departamento= d.codigo_Departamento) left join Familia f on a.Codigo_familia = f.Codigo_familia";
        static String ultimaOrdem = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];

            pesquisar(pnBtn);
            if (!IsPostBack)
            {
                FiltrosMercadoria filtro = (FiltrosMercadoria)Session["filtroMercadoria"];
                if (filtro != null)
                {
                    txtPlu.Text = filtro.plu;
                    txtEan.Text = filtro.ean;
                    txtMercadoria.Text = filtro.mercadoria;
                    DllCampo1.Text = filtro.campo1;
                    DllOnde.Text = filtro.onde;
                    DllCampo2.Text = filtro.campo2;
                    txtRefForn.Text = filtro.Ref;
                    txtNcm.Text = filtro.Ncm;
                    chkPromocao.Checked = filtro.promocao;
                    chkInativo.Checked = filtro.inativo;
                    txtGrupo.Text = filtro.grupo;
                    txtSubGrupo.Text = filtro.subgrupo;
                    txtDepartamento.Text = filtro.departamento;
                    txtFamilia.Text = filtro.familia;
                    ddlTipoData.Text = filtro.data;
                    txtDe.Text = filtro.dtDe;
                    txtAte.Text = filtro.dtAte;
                    txtCSTPisCofins.Text = filtro.CstPisCofins;
                    chkPendente.Checked = filtro.pendente;
                    chkPrecoAtacado.Checked = filtro.atacado;
                    txtTipo.Text = filtro.tipo;
                }

                if (usr != null)
                {
                    pesquisar(true);
                    chkInativo.Visible = usr.adm(usr.tela);

                }
            }


            //txtEan.Attributes.Add("OnKeyPress", "javascript:return numeros(this,event);");
            //txtPlu.Attributes.Add("OnKeyPress", "javascript:return numeros(this,event);");
            //txtMercadoria.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            if (!IsPostBack)
            {
                txtAte.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
                txtAte.MaxLength = 10;
                txtDe.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
                txtDe.MaxLength = 10;
            }


            if (!Funcoes.valorParametro("INTEGRA_WS", usr).Equals("RAKUTEN"))
            {
                btnTransmitir.Visible = false;
                gridMercadorias.Columns[0].Visible = chkInativo.Checked;
            }
        }







        protected void txtDataDe_TextChanged(object sender, EventArgs e)
        {
            if (IsDate(txtDe.Text))
            {
                txtAte.Text = txtDe.Text;
            }
        }



        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/Cadastro/pages/MercadoriaDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            FiltrosMercadoria filtro = (FiltrosMercadoria)Session["filtroMercadoria"];
            if (filtro != null)
            {
                filtro.ultimaOrdem = "";
                Session.Remove("filtroMercadoria");
                Session.Add("filtroMercadoria", filtro);
            }

            pesquisar(false, "ltrim(rtrim(a.descricao))");

            User usr = (User)Session["User"];
            bool pesqUnico = Funcoes.valorParametro("N_DETALHE_UM_PRODUTO", usr).Equals("TRUE");

            if (!pesqUnico)
            {
                HyperLink meuLink = (HyperLink)gridMercadorias.Rows[0].Cells[1].Controls[0];
                if (gridMercadorias.Rows.Count == 1 && !meuLink.Text.Equals("------"))
                {
                    Response.Redirect("MercadoriaDetalhes.aspx?plu=" + meuLink.Text);
                }
            }



        }

        protected void pesquisar(bool limitar)
        {
            pesquisar(limitar, ultimaOrdem);
        }


        protected void pesquisar(bool limitar, String ordem)
        {


            FiltrosMercadoria filtro = (FiltrosMercadoria)Session["filtroMercadoria"];
            if (filtro == null)
                filtro = new FiltrosMercadoria();

            filtro.plu = txtPlu.Text;
            filtro.ean = txtEan.Text;
            filtro.mercadoria = txtMercadoria.Text;
            filtro.campo1 = DllCampo1.Text;
            filtro.onde = DllOnde.Text;
            filtro.campo2 = DllCampo2.Text;
            filtro.Ref = txtRefForn.Text;
            filtro.Ncm = txtNcm.Text;
            filtro.promocao = chkPromocao.Checked;
            filtro.inativo = chkInativo.Checked;
            filtro.pendente = chkPendente.Checked;
            filtro.grupo = txtGrupo.Text;
            filtro.subgrupo = txtSubGrupo.Text;
            filtro.departamento = txtDepartamento.Text;
            filtro.familia = txtFamilia.Text;
            filtro.data = ddlTipoData.Text;
            filtro.dtDe = txtDe.Text;
            filtro.dtAte = txtAte.Text;
            filtro.tribSaida = txtTribSaida.Text;
            filtro.CstPisCofins = txtCSTPisCofins.Text;
            filtro.atacado = chkPrecoAtacado.Checked;
            filtro.tipo = txtTipo.Text;
            filtro.PLURelacionado = txtPLURelacionado.Text;


            if (IsPostBack)
            {
                filtro.iniGrid = 0;
                filtro.fimGrid = 100;
            }

            Session.Remove("filtroMercadoria");
            Session.Add("filtroMercadoria", filtro);



            //gridMercadorias.Columns[0].Visible = chkInativo.Checked;
            imgBtnSalvarInativos.Visible = chkInativo.Checked;
            lblSalvarInativos.Visible = chkInativo.Checked;

            User usr = (User)Session["User"];

            lblPesquisaErro.Text = "";
            try
            {
                carregarGrid(ordem);



                ddlPag.Items.Clear();
                ddlPag1.Items.Clear();
                for (int i = 0; i < filtro.qtdeFiltro; i += 100)
                {
                    int gridInicio = i;
                    int gridFim = i + 100;
                    if (gridFim > filtro.qtdeFiltro)
                    {
                        decimal pagInicio = (decimal.Truncate(filtro.qtdeFiltro / 100) * 100);
                        gridInicio = Convert.ToInt32(pagInicio);
                        gridFim = filtro.qtdeFiltro;
                    }
                    ddlPag.Items.Add((gridInicio + 1) + "-" + gridFim);
                    ddlPag1.Items.Add((gridInicio + 1) + "-" + gridFim);

                }

                if (filtro.fimGrid > filtro.qtdeFiltro)
                {
                    filtro.fimGrid = filtro.qtdeFiltro;
                }


            }
            catch (Exception err)
            {

                lblPesquisaErro.Text = err.Message;
            }
            if(gridMercadorias.HeaderRow !=null)
            {
                CheckBox chkTodos = (CheckBox)gridMercadorias.HeaderRow.FindControl("chkSeleciona");
                if (chkTodos != null)
                {
                    chkTodos.Attributes.Remove("OnClick");
                    chkTodos.Attributes.Add("OnClick", "javascript:selecionar(this);");

                }
            }
            
        }

        protected void carregarGrid(string ordem)
        {

            User usr = (User)Session["User"];
            FiltrosMercadoria filtro = (FiltrosMercadoria)Session["filtroMercadoria"];


            gridMercadorias.DataSource = Conexao.GetTable(filtro.SqlFinal(usr, ordem), usr, false);
            gridMercadorias.DataBind();

            verificaPromocao();
            if (visualSysWeb.code.Funcoes.valorParametro("INIBE_CUSTO_PRODUTO", usr) == "TRUE")
            {
                if (!usr.adm(usr.tela))
                {
                    gridMercadorias.Columns[5].Visible = false;
                }
            }
            if (filtro.atacado)
            {
                gridMercadorias.Columns[9].Visible = true;
                gridMercadorias.Columns[10].Visible = true;
            }
            else
            {
                gridMercadorias.Columns[9].Visible = false;
                gridMercadorias.Columns[10].Visible = false;
            }


            try
            {


                ddlPag.Text = (filtro.iniGrid + 1) + "-" + filtro.fimGrid;
                ddlPag1.Text = (filtro.iniGrid + 1) + "-" + filtro.fimGrid;
            }
            catch (Exception)
            {


            }




            if (filtro.qtdeFiltro > 100)
            {
                lblRegistros.Text = (filtro.iniGrid + 1) + " ate " + filtro.fimGrid + " de " + filtro.qtdeFiltro + " Pesquisados de " + filtro.qtdeCadastro + " Cadastrados";
            }
            else
            {
                lblRegistros.Text = filtro.qtdeFiltro + " Pesquisados de " + filtro.qtdeCadastro + " Cadastrados";

            }

            Session.Remove("filtroMercadoria");
            Session.Add("filtroMercadoria", filtro);

        }

        protected void btnPag_Click(object sender, EventArgs e)
        {


            Button btn = (Button)sender;
            FiltrosMercadoria pg = (FiltrosMercadoria)Session["filtroMercadoria"];


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
                if (pg.fimGrid > pg.qtdeFiltro)
                {
                    decimal pagInicio = (decimal.Truncate(pg.qtdeFiltro / 100) * 100);
                    pg.iniGrid = Convert.ToInt32(pagInicio);
                    pg.fimGrid = pg.qtdeFiltro;
                }
            }
            else if (btn.ID.Equals("btnPagFim") || btn.ID.Equals("btnPagFim1"))
            {
                decimal pagInicio = (decimal.Truncate(pg.qtdeFiltro / 100) * 100);
                pg.iniGrid = Convert.ToInt32(pagInicio);
                pg.fimGrid = pg.qtdeFiltro;
            }



            pg.ultimaOrdem = "";
            Session.Remove("filtroMercadoria");
            Session.Add("filtroMercadoria", pg);
            
            carregarGrid(pg.ordemFiltro);

        }



        protected void ddlPag_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltrosMercadoria pg = (FiltrosMercadoria)Session["filtroMercadoria"];
            DropDownList ddl = (DropDownList)sender;
            int gridInicio = int.Parse(ddlPag.Text.Substring(0, ddl.Text.IndexOf("-"))) - 1;
            int gridFim = int.Parse(ddlPag.Text.Substring(ddl.Text.IndexOf("-") + 1));

            pg.iniGrid = gridInicio;
            pg.fimGrid = gridFim;
            pg.ultimaOrdem = "";

            Session.Remove("filtroMercadoria");
            Session.Add("filtroMercadoria", pg);
            carregarGrid(pg.ordemFiltro);
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
        protected override bool campoObrigatorio(Control campo)
        {
            String[] campos = { "",
                           "",
                           "",
                           "",
                           ""
                          };

            for (int i = 0; i < campos.Length; i++)
            {
                if (campo.ClientID.Equals(campos[i]))
                {

                    return true;
                }
            }
            return false;

        }




        protected override bool campoDesabilitado(Control campo)
        {
            String[] campos = { "",
                           ""
                          };

            for (int i = 0; i < campos.Length; i++)
            {
                if (campo.ClientID.Equals(campos[i]))
                {

                    return true;
                }
            }
            return false;
        }

        protected void txtPlu_TextChanged(object sender, EventArgs e)
        {
            pesquisar(true);
        }

        protected void txtEan_TextChanged(object sender, EventArgs e)
        {
            pesquisar(true);
        }

        protected void txtMercadoria_TextChanged(object sender, EventArgs e)
        {
            pesquisar(false);

        }

        protected void txtRefForn_TextChanged(object sender, EventArgs e)
        {
            pesquisar(false);

        }


        protected void verificaPromocao()
        {

            for (int i = 0; i < gridMercadorias.Rows.Count; i++)
            {


                HyperLink meuLink = (HyperLink)gridMercadorias.Rows[i].Cells[8].Controls[0];
                if (!meuLink.Text.Equals("0,00") && !meuLink.Text.Equals("------"))
                {
                    gridMercadorias.Rows[i].CssClass = "linhaPromocao";
                    //gridMercadorias.Rows[i].ForeColor = System.Drawing.Color.Blue;
                    //gridMercadorias.Rows[i].Style.Add("font-weight", "bold");

                }

                if (chkInativo.Checked)
                {
                    CheckBox chkIna = (CheckBox)gridMercadorias.Rows[i].FindControl("chkSelecionaItem");
                    if (chkIna != null)
                        if (!chkIna.Checked)
                        {
                            gridMercadorias.Rows[i].CssClass = "linhaInativo";
                            //gridMercadorias.Rows[i].Style.Add("font-weight", "bold");

                        }
                }

                if (chkPendente.Checked)
                {
                    gridMercadorias.Rows[i].CssClass = "linhaPendente";
                }
                else
                {
                    Label lblInativo = (Label)gridMercadorias.Rows[i].FindControl("lblInativo");
                    if (lblInativo.Text.Equals("3"))
                    {
                        gridMercadorias.Rows[i].CssClass = "linhaPendente";
                    }
                }
            }

        }
        protected void gridMercadoria_Sorting(object sender, GridViewSortEventArgs e)
        {
            pesquisar(true, e.SortExpression);
        }

        protected void chkSeleciona_CheckedChanged(object sender, EventArgs e)
        {

            foreach (GridViewRow item in gridMercadorias.Rows)
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
        }

        protected void imgBtnSalvarInativos_Click(object sender, ImageClickEventArgs e)
        {
            for (int i = 0; i < gridMercadorias.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)gridMercadorias.Rows[i].FindControl("chkSelecionaItem");
                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        HyperLink meuLink = (HyperLink)gridMercadorias.Rows[i].Cells[1].Controls[0];
                        Conexao.executarSql("update mercadoria set inativo=0 where plu = '" + meuLink.Text + "'");
                    }
                }
            }
            pesquisar(false);
        }

        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            pesquisar(true);
        }



        protected void Img_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            TxtPesquisaLista.Text = "";
            switch (btn.ID)
            {
                case "imgBtnGrupo":
                    Session.Add("camporecebe" + urlSessao(), "Grupo");
                    break;
                case "imgBtnSubGrupo":
                    Session.Add("camporecebe" + urlSessao(), "SubGrupo");
                    break;
                case "imgBtnDepartamento":
                    Session.Add("camporecebe" + urlSessao(), "Departamento");
                    break;
                case "imgBtnFamilia":
                    Session.Add("camporecebe" + urlSessao(), "Familia");
                    break;

                case "imgBtnTribSaida":
                    Session.Add("camporecebe" + urlSessao(), "tribSaida");
                    break;
                case "imgBtnTipo":
                    Session.Add("camporecebe" + urlSessao(), "Tipo");
                    break;
            }

            exibeLista();


        }

        protected void exibeLista()
        {
            lblErroPesquisa.Text = "";
            User usr = (User)Session["User"];
            String or = (String)Session["camporecebe" + urlSessao()];
            String sqlLista = "";


            switch (or)
            {
                case "Grupo":
                    sqlLista = " Select descricao_grupo from grupo  where descricao_grupo like '%" + TxtPesquisaLista.Text + "%'  ";
                    lbllista.Text = "Grupo";
                    break;
                case "SubGrupo":
                    sqlLista = " Select descricao_subGrupo,descricao_grupo from W_BR_CADASTRO_DEPARTAMENTO where descricao_subGrupo  like '%" + TxtPesquisaLista.Text + "%'" + (txtGrupo.Text.Equals("") ? "" : " and descricao_grupo ='" + txtGrupo.Text + "'") + " group by descricao_subGrupo,descricao_grupo;";
                    lbllista.Text = "Sub Grupo";
                    break;
                case "Departamento":
                    sqlLista = "select descricao_departamento, descricao_subGrupo,descricao_grupo  from W_BR_CADASTRO_DEPARTAMENTO where descricao_departamento like '%" + TxtPesquisaLista.Text + "%' " + (txtSubGrupo.Text.Trim().Equals("") ? "" : " and descricao_subgrupo = '" + txtSubGrupo.Text + "'") + "group by descricao_departamento, descricao_subGrupo,descricao_grupo"; ;
                    lbllista.Text = "Departamento";
                    break;
                case "Familia":
                    sqlLista = "Select descricao_familia from familia  where descricao_familia like '%" + TxtPesquisaLista.Text + "%'";
                    lbllista.Text = "Familia";
                    break;
                case "tribSaida":
                    sqlLista = "Select Cod= Codigo_Tributacao " +
                                       ",Descricao = Descricao_Tributacao " +
                                       ",Icms = Saida_ICMS " +
                                       ",CST = Indice_ST " +
                                "from Tributacao";
                    lbllista.Text = "Tributação";

                    break;
                case "Tipo":
                    sqlLista = "select Tipo, " +
                    " CASE WHEN ISNULL(Gera_Carga, 0) = 1 THEN 'SIM' ELSE 'NAO' END AS GeraCarga, " +
                    " CASE WHEN ISNULL(Permite_item, 0) = 1 THEN 'SIM' ELSE 'NAO' END AS Itens, " +
                    " CASE WHEN ISNULL(Movimenta_Estoque_Item, 0) = 1 THEN 'SIM' ELSE 'NAO' END as MovEstoqueItem, " +
                    " CASE WHEN ISNULL(Compra, 0) = 1 THEN 'SIM' ELSE 'NAO' END AS Compra, " +
                    " CASE WHEN ISNULL(Estoque, 0) = 1 THEN 'SIM' ELSE 'NAO' END AS Estoque, " +
                    " CASE WHEN ISNULL(PLUAssociado, 0) = 1 THEN 'SIM' ELSE 'NAO' END AS PLUAssociado" +
                    " FROM Tipo";
                    lbllista.Text = "Tipo";
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

                String listaAtual = (String)Session["camporecebe" + urlSessao()];
                Session.Remove("camporecebe");

                if (listaAtual.Equals("Grupo"))
                {
                    txtGrupo.Text = ListaSelecionada(1);
                    txtSubGrupo.Text = "";
                    txtDepartamento.Text = "";

                }
                else if (listaAtual.Equals("SubGrupo"))
                {
                    txtSubGrupo.Text = ListaSelecionada(1);
                    txtGrupo.Text = ListaSelecionada(2);
                    txtDepartamento.Text = "";
                }
                else if (listaAtual.Equals("Departamento"))
                {
                    txtDepartamento.Text = ListaSelecionada(1);
                    txtSubGrupo.Text = ListaSelecionada(2);
                    txtGrupo.Text = ListaSelecionada(3);

                }
                else if (listaAtual.Equals("Familia"))
                {
                    txtFamilia.Text = ListaSelecionada(1);
                }
                else if (listaAtual.Equals("tribSaida"))
                {
                    txtTribSaida.Text = ListaSelecionada(1);
                }
                else if (listaAtual.Equals("Tipo"))
                {
                    txtTipo.Text = ListaSelecionada(1);
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

        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }

        protected void GridLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoListaItem");

            if (rdo == null)
            {
                return;//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            }
            string script = "SetUniqueRadioButton('GridLista.*GrlistaItem',this)";
            rdo.Attributes.Add("onclick", script);
        }

        protected void gridMercadorias_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gridMercadorias_RowCreated(object sender, GridViewRowEventArgs e)
        {
        }

        protected void gridMercadorias_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }


        protected void btnTransmitir_Click(object sender, EventArgs e)
        {
            ArrayList arrProdutos = new ArrayList();
            User usr = (User)Session["User"];
            foreach (GridViewRow row in gridMercadorias.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelecionaItem");

                if (chk.Checked)
                {
                    ////Exclusivo SAP
                    HyperLink meuLink = (HyperLink)row.Cells[1].Controls[0];
                    arrProdutos.Add(meuLink.Text);

                }
            }

            Session.Remove("arrProdutos");
            Session.Add("arrProdutos", arrProdutos);

            Session.Remove("detalhesTrans");
            Session.Remove("resultadoTrans");
            Session.Remove("erroTrans");
            Session.Remove("abortaTrans");

            System.Threading.Thread th = new System.Threading.Thread(trasmite);
            th.Start();

            TimerTrans.Interval = 450;
            TimerTrans.Enabled = true;
            lblDetalhesTrans.Text = "Transmitindo...";
            modalTransmitirWebServer.Show();


        }
        protected void imgBtnLimpar_Click(object sender, ImageClickEventArgs e)
        {
            txtPlu.Text = "";
            txtEan.Text = "";
            txtRefForn.Text = "";
            txtMercadoria.Text = "";
            txtNcm.Text = "";
            DllCampo1.Text = "";
            DllCampo2.Text = "";
            DllOnde.Text = "";
            chkPromocao.Checked = false;
            chkInativo.Checked = false;
            txtGrupo.Text = "";
            txtSubGrupo.Text = "";
            txtDepartamento.Text = "";
            txtFamilia.Text = "";
            ddlTipoData.Text = "";
            txtDe.Text = "";
            txtAte.Text = "";
            txtTribSaida.Text = "";
            txtTipo.Text = "";

        }
        protected void trasmite()
        {

            User usr = (User)Session["User"];
            ArrayList arrProdutos = (ArrayList)Session["arrProdutos"];
            String strErro = "";
            String strOK = "";
            string strNao = "";
            String retorno = "";

            int nAtualizados = 0;
            int nNaoAtualizados = 0;


            if (Funcoes.valorParametro("INTEGRA_WS", usr).Equals("RAKUTEN"))
            {
                if (arrProdutos != null)
                {




                    foreach (String produto in arrProdutos)
                    {

                        MercadoriaDAO merc = new MercadoriaDAO(produto, usr);
                        if (merc.IntegraWS)
                        {

                            try
                            {

                                KCWSFabricante fb = new KCWSFabricante(merc.Marca, "Salvar");
                                KCWSProduto prod = new KCWSProduto(merc, "Salvar", usr);
                                KCWSProdutoEstoque prodEst = new KCWSProdutoEstoque("Salvar", merc.PLU, (int)merc.saldo_atual, usr);
                                KCWSProdutoCategoria prodCat = new KCWSProdutoCategoria(usr);
                                prodCat.Salvar(merc.PLU, merc.Codigo_departamento);
                                prodCat.Salvar(merc.PLU, merc.Codigo_departamento.Substring(0, 6));
                                prodCat.Salvar(merc.PLU, merc.Codigo_departamento.Substring(0, 3));

                                nAtualizados++;
                                strOK = nAtualizados.ToString() + " Integrado(s) ao WebService com sucesso <br>";

                            }
                            catch (Exception err)
                            {

                                nNaoAtualizados++;
                                strNao = nNaoAtualizados.ToString() + " Erro(s) ";
                                strErro += "<span style=\"Color:red;\"> Erro na integração do PLU =" + merc.PLU + " com o WebService Detalhes:" + err + "</span><br>";

                            }



                        }
                        else
                        {
                            nNaoAtualizados++;
                            strNao = nNaoAtualizados.ToString() + " Erro(s) ";
                            strErro += "<span style=\"Color:red;\"> Não foi possivel a integração do PLU =" + merc.PLU + " <br> Item não selecionado para integração<BR> Selecione a opção Integra WS nos detalhes do item! </span><br>";
                        }

                        retorno = strOK + strNao + strErro;
                        Session.Remove("detalhesTrans");
                        Session.Add("detalhesTrans", retorno);
                    }


                    Session.Add("resultadoTrans", retorno);
                }
            }
            else if (Funcoes.valorParametro("INTEGRA_WS", usr).Equals("SAP"))
            {
                Session.Add("resultadoTrans", "Integração SAP Não Habilitada");


                //if (arrProdutos != null)
                //{

                //    foreach (String produto in arrProdutos)
                //    {
                //        MercadoriaDAO merc = new MercadoriaDAO(produto, usr);

                //        WSSAPProduto SAPProd = new WSSAPProduto();
                //        SAPProd.Codigo = int.Parse(merc.PLU.ToString());
                //        SAPProd.Descricao = merc.Descricao.ToString();
                //        SAPProd.Grupo_UM = 1;
                //        SAPProd.Grupo_Itens = 1;
                //        SAPProd.Item_Estoque = 1;
                //        SAPProd.Item_Venda = (merc.Tipo.ToString() == "MATERIA PRIMA" || merc.Tipo.ToString() == "COMPONENTE" ? 0 : 1);
                //        SAPProd.Item_Compra = (merc.Tipo.ToString() == "PRODUCAO" ? 0 : 1);
                //        SAPProd.UM_Estoque = merc.und;
                //        SAPProd.UM_Venda = merc.und;
                //        SAPProd.UM_Compra = (merc.Tipo.ToString() == "PRODUCAO" ? "" : merc.und);
                //        SAPProd.Ativo = (merc.Inativo ? 0 : 1);

                //        //Novos Campos
                //        SAPProd.NCMCod = merc.cf;
                //        SAPProd.MaterialType = 1;
                //        SAPProd.ProductSource = 1;
                //        SAPProd.Departamento = merc.Codigo_departamento + merc.Descricao_departamento;
                //        SAPProd.GrupoBratter = merc.codigo_Grupo + merc.descricao_Grupo;
                //        SAPProd.MarcaHom = merc.Marca;
                //        SAPProd.ItemClass = 1;




                //}

                //    Session.Add("resultadoTrans", retorno);


                //}

                //else
                //{
                //    showMessage("NÃO FOI SELECIONADO ITENS PARA A TRANSMISSAO", true);
                //}






            }
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

        protected void btnOkTransmissao_Click(object sender, EventArgs e)
        {

            modalTransmitirWebServer.Hide();
        }

        protected void TimerTrans_Tick(object sender, EventArgs e)
        {
            String detalhes = (String)Session["detalhesTrans"];
            String resultado = (String)Session["resultadoTrans"];
            String error = (String)Session["erroTrans"];
            String aborta = (String)Session["abortaTrans"];

            if (detalhes != null)
            {
                lblDetalhesTrans.Text = detalhes;
                String strAg = "Aguarde";
                if (btnOkTransmissao.Text.Contains(".."))
                    strAg += "...";
                else if (btnOkTransmissao.Text.Contains("..."))
                    strAg += ".";
                else
                    strAg += "..";

                btnOkTransmissao.Text = strAg;
            }

            if (resultado != null)
            {
                TimerTrans.Enabled = false;
                btnOkTransmissao.Enabled = true;
                btnOkTransmissao.Text = "OK";


            }

            if (aborta != null)
            {
                TimerTrans.Enabled = false;


            }

            modalTransmitirWebServer.Show();
        }

    }
}