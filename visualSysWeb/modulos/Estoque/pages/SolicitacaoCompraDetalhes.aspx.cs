using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using System.Data.SqlClient;
using System.Globalization;
using visualSysWeb.code;
using System.IO;

namespace visualSysWeb.modulos.Estoque.pages
{
    public partial class SolicitacaoCompraDetalhes : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            if (usr != null)
            {
                if (Request["novo"] != null)
                {
                    if (!IsPostBack)
                    {
                        solicitacao_compraDAO obj = new solicitacao_compraDAO(usr);
                        obj.usuario_cadastro = usr.getUsuario();
                        obj.data_cadastro = DateTime.Now;
                        obj.status = "INICIADO";
                        status = "incluir";
                        txtDescricao.Focus();
                        Session.Remove("objSolicita" + urlSessao());
                        Session.Add("objSolicita" + urlSessao(), obj);
                        carregarDados();

                    }

                }
                else
                {
                    if (Request["codigo"] != null)
                    {
                        if (!IsPostBack)
                        {
                            String cod = Request["codigo"].ToString();
                            status = "visualizar";
                            solicitacao_compraDAO obj = new solicitacao_compraDAO(cod, usr);
                            Session.Remove("objSolicita" + urlSessao());
                            Session.Add("objSolicita" + urlSessao(), obj);
                            carregarDados();

                        }
                    }
                }


                if (!IsPostBack)
                {
                    if (status.Equals("visualizar"))
                    {
                        HabilitarCampos(false);
                    }
                    else
                    {
                        HabilitarCampos(true);

                    }
                    carregarMercadorias(true);
                }
                carregabtn(pnBtn);
            }

        }

        protected void chkSelecionaItem_CheckedChanged(object sender, EventArgs e)
        {

            CheckBox chk = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chk.Parent.Parent;
            TextBox txt = (TextBox)row.FindControl("txtGridQtde");
            if (chk.Checked)
            {
                txt.Text = row.Cells[10].Text;
                txt.Enabled = false;
                txt.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                chk.Focus();
            }
            else
            {
                txt.Enabled = true;
                txt.BackColor = System.Drawing.Color.White;
                txt.Focus();
            }

            //calculachk();

        }

        protected void gridItens_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridItens.EditIndex = e.NewEditIndex;
            //carregardados();
        }
        private void calculachk()
        {

            for (int i = 0; i < gridItens.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)gridItens.Rows[i].FindControl("chkSelecionaItem");
                TextBox txt = (TextBox)gridItens.Rows[i].FindControl("txtGridQtde");

                if (chk.Checked)
                {
                    txt.Text = gridItens.Rows[i].Cells[10].Text;
                    txt.Enabled = false;
                    txt.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                }
                else
                {
                    if (!status.Equals("visualizar"))
                    {
                        txt.Enabled = true;
                        txt.BackColor = System.Drawing.Color.White;
                    }
                }

            }

        }

        private void carregarDados()
        {
            solicitacao_compraDAO obj = (solicitacao_compraDAO)Session["objSolicita" + urlSessao()];
            txtCodigo.Text = obj.codigo;
            txtDescricao.Text = obj.descricao;
            txtDataCadastro.Text = obj.data_cadastroBr();
            txtUsuarioCadastro.Text = obj.usuario_cadastro;
            txtstatus.Text = obj.status;
            ddlTipoSolicitacao.Text = obj.tipo_solicitacao;
            int nInicio = obj.gridInicio;
            int nFim = (obj.arrItens.Count > 100 ? obj.gridFim : obj.arrItens.Count);
            int total = obj.arrItens.Count;
            int qtdeDig = 0;
            bool selecionados = false;
            if (obj.itensDigitados)
            {
                qtdeDig = obj.qtdeItensDigitados;
                nFim = (qtdeDig > 100 ? obj.gridFim : qtdeDig);
                selecionados = true;
            }
           

            lblRegistros.Text = (nInicio + 1) + " ate " + nFim + " de " + (selecionados ? qtdeDig + " Selecionados de " : "") + total + " Registro(s) Adicionado(s)";

            gridItens.DataSource = obj.Itens;
            gridItens.DataBind();

            gridMercadoriasSelecionadas.DataSource = obj.Itens;
            gridMercadoriasSelecionadas.DataBind();


            gridPedidos.DataSource = obj.tbPedidos();
            gridPedidos.DataBind();

            gridCotacoes.DataSource = obj.tbCotacao();
            gridCotacoes.DataBind();


            foreach (GridViewRow item in gridItens.Rows)
            {
                TextBox txt = (TextBox)item.FindControl("txtGridQtde");
                Label lblAceita = (Label)item.FindControl("lblaceitaSolicitacao");
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                chk.Checked = lblAceita.Text.Equals("1");
                if (!status.Equals("visualizar"))
                {
                    txt.Enabled = !chk.Checked;
                }
            }


            //DateTime tMes1 = obj.data_cadastro.AddMonths(-4);
            //DateTime tMes2 = obj.data_cadastro.AddMonths(-3);
            //DateTime tMes3 = obj.data_cadastro.AddMonths(-2);
            //DateTime tMes4 = obj.data_cadastro.AddMonths(-1);

            //gridItens.HeaderRow.Cells[9].Text = new CultureInfo("pt-BR").DateTimeFormat.GetAbbreviatedMonthName(tMes1.Month).ToUpper();
            //gridItens.HeaderRow.Cells[10].Text = new CultureInfo("pt-BR").DateTimeFormat.GetAbbreviatedMonthName(tMes2.Month).ToUpper();
            //gridItens.HeaderRow.Cells[11].Text = new CultureInfo("pt-BR").DateTimeFormat.GetAbbreviatedMonthName(tMes3.Month).ToUpper();
            //gridItens.HeaderRow.Cells[12].Text = new CultureInfo("pt-BR").DateTimeFormat.GetAbbreviatedMonthName(tMes4.Month).ToUpper();
        }

        private void carregarDadosObj()
        {
            solicitacao_compraDAO obj = (solicitacao_compraDAO)Session["objSolicita" + urlSessao()];
            obj.codigo = txtCodigo.Text;
            obj.tipo_solicitacao = ddlTipoSolicitacao.Text;
            obj.descricao = txtDescricao.Text;
            DateTime.TryParse(txtDataCadastro.Text, out obj.data_cadastro);
            obj.usuario_cadastro = txtUsuarioCadastro.Text;
            obj.status = txtstatus.Text;
            for (int i = 0; i < gridItens.Rows.Count; i++)
            {
                TextBox txtGridQtde = (TextBox)gridItens.Rows[i].FindControl("txtGridQtde");
                if (!txtGridQtde.Text.Equals("------"))
                {
                    solicitacao_compra_itensDAO item = obj.item(gridItens.Rows[i].Cells[1].Text);
                    Decimal.TryParse(txtGridQtde.Text, out item.qtde_comprar);

                    Label lblAceita = (Label)gridItens.Rows[i].FindControl("lblaceitaSolicitacao");
                    CheckBox chk = (CheckBox)gridItens.Rows[i].FindControl("chkSelecionaItem");
                    lblAceita.Text = (chk.Checked ? "1" : "0");
                    item.aceita_sug = chk.Checked;
                }

            }
            Session.Remove("objSolicita" + urlSessao());
            Session.Add("objSolicita" + urlSessao(), obj);


        }
        protected void btnEncerrar_Click(object sender, EventArgs e)
        {
            modalEncerrar.Show();

        }
        protected void imgPlu_Click(object sender, ImageClickEventArgs e)
        {
            if (!txtPlu.Text.Trim().Equals(""))
            {
                carregarPlu();
            }
            else
            {
                exibeLista("PLU");
            }
        }

        protected void ImgBtnAddItens_Click(object sender, EventArgs e)
        {

            try
            {
                if (!txtPlu.Text.Equals(""))
                {
                    carregarDadosObj();
                    incluirItens(txtPlu.Text, txtContado.Text);


                }
            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
               //lblError.Text = err.Message;
               // lblError.ForeColor = System.Drawing.Color.Red;

            }




        }
        protected void btnPag_Click(object sender, EventArgs e)
        {

            if (!status.Equals("visualizar"))
            {
                carregarDadosObj();
            }
            solicitacao_compraDAO obj = (solicitacao_compraDAO)Session["objSolicita" + urlSessao()];
            int nFim = obj.arrItens.Count;
            if (obj.itensDigitados)
            {
                nFim = obj.qtdeItensDigitados;
            }





            Button btn = (Button)sender;
            if (btn.ID.Equals("btnPagInicio"))
            {
                obj.gridInicio = 0;
                obj.gridFim = 100;
            }
            else if (btn.ID.Equals("btnPagAnterio"))
            {
                obj.gridInicio -= 100;
                obj.gridFim = obj.gridInicio + 100;
                if (obj.gridInicio < 0)
                {
                    obj.gridInicio = 0;
                    obj.gridFim = 100;
                }


            }
            else if (btn.ID.Equals("btnPagProximo"))
            {

                obj.gridInicio += 100;
                obj.gridFim = obj.gridInicio + 100;
                if (obj.gridFim > nFim)
                {
                    decimal pagInicio = (decimal.Truncate(nFim / 100) * 100);
                    obj.gridInicio = Convert.ToInt32(pagInicio);
                    obj.gridFim = nFim;
                }
            }
            else if (btn.ID.Equals("btnPagFim"))
            {
                decimal pagInicio = (decimal.Truncate(nFim / 100) * 100);
                obj.gridInicio = Convert.ToInt32(pagInicio);
                obj.gridFim = nFim;
            }

            Session.Remove("objSolicita" + urlSessao());
            Session.Add("objSolicita" + urlSessao(), obj);

            carregarDados();
            if (status.Equals("visualizar"))
            {
                EnabledControls(gridItens, false);
            }



        }

        protected void ddlLinha_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarMercadorias(false);
        }

        private void carregarLinhas()
        {
            String sqlLinha = "Select codigo=convert(varchar(3),linha.codigo_linha)+convert(varchar(3),cor_linha.codigo_cor),  linha= linha.descricao_linha+'-'+cor_linha.descricao_cor from linha " +
                                                                " inner join cor_linha on linha.codigo_linha = cor_linha.codigo_linha";
            Conexao.preencherDDL1Branco(ddlLinha, sqlLinha, "linha", "codigo", null);
        }

        protected void imgFornecedor_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("Fornecedor");
        }
        protected void imgBtnListaPadrao_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("ListaPadrao");
        }
        protected void imgBtnGrupo_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("Grupo");
        }
        protected void imgBtnSubGrupo_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("SubGrupo");
        }
        protected void imgBtnDepartamento_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("Departamento");
        }



        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalLista.Hide();
        }
        protected String ListaSelecionada(int index)
        {
            foreach (GridViewRow item in GridLista.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoListaItem");

                if (rdo != null)
                {
                    if (rdo.Checked)
                    {
                        return item.Cells[index].Text;
                    }
                }
            }

            return "";
        }
        protected void carregarPlu()
        {
            User usr = (User)Session["User"];
            SqlDataReader rsplu = null;
            try
            {



                rsplu = Conexao.consulta("Select top 1 mercadoria.PLU,mercadoria.descricao, isnull(mercadoria_loja.saldo_atual,0)saldo_atual,isnull(mercadoria_loja.preco_custo,0)custo from mercadoria inner join mercadoria_loja on mercadoria.plu=mercadoria_loja.plu left join ean on mercadoria.plu=ean.plu where mercadoria_loja.filial='" + usr.getFilial() + "' and ( mercadoria.plu='" + txtPlu.Text + "' or ean.EAN='" + txtPlu.Text + "')", null, false);
                if (rsplu.Read())
                {
                    txtPlu.Text = rsplu["plu"].ToString();
                    txtDescricaoItem.Text = rsplu["descricao"].ToString();
                    TxtSaldoAtual.Text = Decimal.Parse(rsplu["saldo_atual"].ToString()).ToString();
                    txtCusto.Text = Decimal.Parse(rsplu["custo"].ToString()).ToString();
                    txtContado.Focus();
                    addItensDig.DefaultButton = "ImgBtnAddItens";
                }
                else
                {
                    msgShow("Produto não encontrado!!", true);
                    txtPlu.Focus();

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rsplu != null)
                    rsplu.Close();
            }
        }
        protected void ImgPesquisaMercadoria_Click(object sender, ImageClickEventArgs e)
        {
            carregarMercadorias(false);
        }
        protected void imgLimpar_Click(object sender, ImageClickEventArgs e)
        {
            limparSelecaoMercadoria();
            carregarMercadorias(true);
        }
        protected void imgBtnIncluirSelecionados_Click(object sender, ImageClickEventArgs e)
        {
            verificaSelecionados();
        }

        private void verificaSelecionados()
        {
            try
            {



                String strPlus = "";
                carregarDadosObj();
                foreach (GridViewRow item in gridMercadoria1.Rows)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                    if (chk.Checked)
                    {

                        strPlus = item.Cells[1].Text;
                        incluirItens(strPlus, "");
                        chk.Checked = false;
                    }
                }





                carregarDados();
            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
                
            }



        }

        private void incluirItens(String strPlus, String qtde)
        {
            User usr = (User)Session["User"];
            SqlDataReader rs = null;
            solicitacao_compraDAO obj = (solicitacao_compraDAO)Session["objSolicita" + urlSessao()];
            try
            {
                rs = Conexao.consulta("execute sp_estoque_solicitacao_compra '" + usr.getFilial() + "','" + strPlus + "','','','','','','','','','','','','','','',null,5,'',''", null, false);
                while (rs.Read())
                {
                    if (!obj.itemIncluido(rs["PLU"].ToString()))
                    {

                        solicitacao_compra_itensDAO objItem = new solicitacao_compra_itensDAO(usr);
                        objItem.codigo = obj.codigo;
                        objItem.filial = obj.filial;

                        objItem.plu = rs["PLU"].ToString();
                        objItem.ean = rs["EAN"].ToString();
                        objItem.descricao = rs["DESCRICAO"].ToString();
                        objItem.und = rs["UN"].ToString();
                        Decimal.TryParse(rs["EMB"].ToString(), out objItem.embalagem);
                        Decimal.TryParse(rs["SALDO"].ToString(), out objItem.saldo);
                        Decimal.TryParse(rs["PRC_CUSTO"].ToString(), out objItem.preco_custo);
                        Decimal.TryParse(rs["PRC_VENDA"].ToString(), out objItem.preco_venda);
                        Decimal.TryParse(rs["COB_CAD"].ToString(), out objItem.cob_cad);
                        Decimal.TryParse(rs["MES5"].ToString(), out  objItem.mes_5);
                        Decimal.TryParse(rs["MES4"].ToString(), out objItem.mes_4);
                        Decimal.TryParse(rs["MES3"].ToString(), out objItem.mes_3);
                        Decimal.TryParse(rs["MES2"].ToString(), out objItem.mes_2);
                        Decimal.TryParse(rs["ULT_30D"].ToString(), out objItem.ult_30);
                        Decimal.TryParse(rs["VDA_MED"].ToString(), out objItem.vda_med);
                        Decimal.TryParse(rs["COB_DIAS"].ToString(), out objItem.cob_dias);
                        Decimal.TryParse(rs["SUG_UNID"].ToString(), out objItem.sugestao);
                        objItem.CTR = rs["CTR"].ToString();
                        if (qtde.Equals(""))
                        {
                            Decimal.TryParse(rs["QTDE_COMPRA"].ToString(), out objItem.qtde_comprar);
                        }
                        else
                        {
                            Decimal.TryParse(qtde, out objItem.qtde_comprar);
                        }
                        if (objItem.sugestao < 0)
                        {
                            objItem.sugestao = 0;
                        }
                        if (objItem.qtde_comprar < 0)
                        {
                            objItem.qtde_comprar = 0;
                        }

                        if (objItem.embalagem == 0)
                            objItem.embalagem = 1;

                        obj.arrItens.Add(objItem);

                    }


                }

                obj.ordemItens();

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

            carregarDados();


        }
        protected void chkSelecionaItens_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTodos = (CheckBox)sender;
            foreach (GridViewRow item in gridItens.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");

                if (chk != null)
                {
                    chk.Checked = chkTodos.Checked;
                    //incluirMercadoria(chk);
                }
            }

            calculachk();
        }


        protected void limparSelecaoMercadoria()
        {
            lblGrupo.Text = "";
            txtGrupo.Text = "";
            lblSubGrupo.Text = "";
            txtSubGrupo.Text = "";
            lblDepartamento.Text = "";
            txtDepartamento.Text = "";

            ddlLinha.Text = "";
            txtFornecedor.Text = "";
            txtfiltromercadoria.Text = "";
            txtcodListaPadrao.Text = "";
            txtDescricaoListaPadrao.Text = "";
        }

        protected void txtfiltromercadoria_TextChanged(object sender, EventArgs e)
        {
            if (txtfiltromercadoria.Text.Length > 0)
            {
                carregarMercadorias(false);
            }
            else
            {
                carregarMercadorias(true);
            }
        }
        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {

            String itemLista = (String)Session["campoLista" + urlSessao()];
            switch (itemLista)
            {
                case "PLU":
                    txtPlu.Text = ListaSelecionada(1);
                    carregarPlu();
                    break;
                case "Fornecedor":
                    txtFornecedor.Text = ListaSelecionada(1);
                    carregarMercadorias(false);
                    break;
                case "Grupo":
                    lblGrupo.Text = ListaSelecionada(1);
                    txtGrupo.Text = ListaSelecionada(2);

                    lblSubGrupo.Text = "";
                    txtSubGrupo.Text = "";
                    lblDepartamento.Text = "";
                    txtDepartamento.Text = "";



                    carregarMercadorias(false);
                    break;
                case "SubGrupo":
                    lblSubGrupo.Text = ListaSelecionada(1);
                    txtGrupo.Text = ListaSelecionada(2);
                    txtSubGrupo.Text = ListaSelecionada(3);
                    lblDepartamento.Text = "";
                    txtDepartamento.Text = "";

                    carregarMercadorias(false);
                    break;
                case "Departamento":
                    lblDepartamento.Text = ListaSelecionada(1);
                    txtGrupo.Text = ListaSelecionada(2);
                    txtSubGrupo.Text = ListaSelecionada(3);
                    txtDepartamento.Text = ListaSelecionada(4);
                    carregarMercadorias(false);
                    break;
                case "ListaPadrao":
                    txtcodListaPadrao.Text = ListaSelecionada(1);
                    txtDescricaoListaPadrao.Text = ListaSelecionada(2);
                    carregarMercadorias(false);
                    break;
            }

            modalLista.Hide();
        }

        protected void exibeLista(String campo)
        {
            Session.Remove("campoLista" + urlSessao());
            Session.Add("campoLista" + urlSessao(), campo);

            String sqlLista = "";

            switch (campo)
            {
                case "PLU":
                    lbltituloLista.Text = "Escolha uma mercadoria";
                    sqlLista = "select mercadoria.PLU, ISNULL(mercadoria.Ref_Fornecedor,'') as REFERENCIA, DESCRICAO,isnull(ean.ean,'') EAN from mercadoria left join ean on ean.plu=mercadoria.plu where ";
                    sqlLista = sqlLista + " mercadoria.plu like '%" + TxtPesquisaLista.Text + "%' or descricao like '%" + TxtPesquisaLista.Text + "%'  or mercadoria.Ref_fornecedor like '%" + TxtPesquisaLista.Text + "%' or (ean like '%" + TxtPesquisaLista.Text + "%') ORDER BY DESCRICAO";

                    break;
                case "Fornecedor":
                    lbltituloLista.Text = "Escolha um  Fornecedor";
                    sqlLista = "select Fornecedor,CNPJ,Razao_social from Fornecedor " +
                               " where Fornecedor like '%" + TxtPesquisaLista.Text + "%'  or " +
                               " Razao_social like '%" + TxtPesquisaLista.Text + "%' or  " +
                               "  Nome_Fantasia = '%" + TxtPesquisaLista.Text + "%' or  " +
                               " replace(replace(REPLACE(cnpj,'.',''),'/',''),'-','') like '" + TxtPesquisaLista.Text.Replace(".", "").Replace("-", "").Replace("/", "") + "%' order by fornecedor ";


                    break;
                case "Grupo":
                    lbltituloLista.Text = "Escolha um  Grupo";
                    sqlLista = "select Codigo_Grupo as codigo,Descricao_Grupo as Grupo from Grupo where descricao_grupo like '%" + TxtPesquisaLista.Text + "%' or codigo_grupo like '" + TxtPesquisaLista.Text + "%'";

                    break;
                case "SubGrupo":
                    lbltituloLista.Text = "Escolha um  SubGrupo";
                    sqlLista = "Select codigo_subgrupo as Codigo ,descricao_grupo as Grupo , descricao_subgrupo as SubGrupo  from  W_BR_CADASTRO_DEPARTAMENTO " +
                                    " Where (codigo_subgrupo like '" + TxtPesquisaLista.Text + "%' or descricao_subgrupo like '%" + TxtPesquisaLista.Text + "%' or codigo_subgrupo like '%" + TxtPesquisaLista.Text + "%' ) ";
                    if (!lblGrupo.Text.Equals(""))
                    {
                        sqlLista += " and codigo_grupo ='" + lblGrupo.Text + "'";

                    }
                    sqlLista += " group by codigo_subgrupo, descricao_subgrupo, descricao_grupo ";


                    break;
                case "Departamento":
                    lbltituloLista.Text = "Escolha um  Departamento";
                    sqlLista = "Select codigo_departamento as Codigo ,descricao_grupo as Grupo, descricao_subgrupo as SubGrupo,descricao_departamento as Departamento   from  W_BR_CADASTRO_DEPARTAMENTO " +
                                    " Where (codigo_departamento like '" + TxtPesquisaLista.Text + "%' or descricao_departamento like '%" + TxtPesquisaLista.Text + "%' or descricao_grupo like '%" + TxtPesquisaLista.Text + "%'  or  descricao_subgrupo like '%" + TxtPesquisaLista.Text + "%' ) ";
                    if (!lblGrupo.Text.Equals(""))
                    {
                        sqlLista += " and codigo_grupo ='" + lblGrupo.Text + "'";

                    }
                    if (!lblSubGrupo.Text.Equals(""))
                    {
                        sqlLista += " and codigo_subgrupo='" + lblSubGrupo.Text + "'";
                    }

                    sqlLista += " group by codigo_departamento, descricao_departamento,descricao_subgrupo, descricao_grupo ";

                    break;
                case "ListaPadrao":
                    lbltituloLista.Text = "Escolha a Lista de produtos";
                    sqlLista = "Select Id, Descricao , Qtde_Itens = (Select count(*) from LISTA_PADRAO_ITENS WHERE ID_LISTA = ID)from LISTA_PADRAO";
                    break;
            }
            User usr = (User)Session["User"];
            GridLista.DataSource = Conexao.GetTable(sqlLista, null, true);
            GridLista.DataBind();

            modalLista.Show();
            TxtPesquisaLista.Focus();
        }
        protected bool validaCamposObrigatorios()
        {

            solicitacao_compraDAO obj = (solicitacao_compraDAO)Session["objSolicita" + urlSessao()];
            if (obj.arrItens.Count == 0)
            {
                throw new Exception("Não foi adicionado nenhum Item");
            }

            if (validaCampos(cabecalho) && validaCampos(conteudo))
                return true;
            else
                return false;
        }


        protected void carregarMercadorias(bool limitar)
        {
            if (IsPostBack)
            {
                //verificaSelecionados();
            }
            lblMercadoriaLista.Text = "Inclusão de Produto";
            //lblMercadoriaLista.ForeColor = Label1.ForeColor;

            if (lblGrupo.Text.Equals("") &&
                lblSubGrupo.Text.Equals("") &&
                lblDepartamento.Text.Equals("") &&
                ddlLinha.Text.Equals("") &&
                txtFornecedor.Text.Equals("") &&
                txtfiltromercadoria.Text.Equals("") &&
                txtcodListaPadrao.Text.Equals(""))
            {
                limitar = true;
            }


            User usr = (User)Session["user"];
            String sqlMercadoria = "  Select * into #cadDepart from W_BR_CADASTRO_DEPARTAMENTO;  " +
                                             "Select  mercadoria.plu PLU," +
                                                   " isnull(ean.ean,'---')EAN," +
                                                   " mercadoria.Ref_fornecedor REFERENCIA, " +
                                                   " mercadoria.descricao DESCRICAO, " +
                                                   " mercadoria_loja.preco_Custo as [PRC COMPRA]," +
                                                   " mercadoria_loja.saldo_atual SALDO, " +
                                                   " mercadoria_loja.preco as [PRC VENDA] " +
                                             " from mercadoria inner join mercadoria_loja on mercadoria.plu = mercadoria_loja.plu " +
                                               " left join ean on mercadoria.plu=ean.plu  " +
                                               " inner join #cadDepart on mercadoria.Codigo_departamento = #cadDepart.codigo_departamento " +
                                               " left join Fornecedor_Mercadoria on mercadoria.PLU = Fornecedor_Mercadoria.PLU  AND Mercadoria_Loja.Filial = Fornecedor_Mercadoria.Filial " +
                                    " where (mercadoria_loja.filial='" + usr.getFilial() + "') ";
            if (isnumero(txtfiltromercadoria.Text))
            {
                if (txtfiltromercadoria.Text.Length <= 6)
                {
                    sqlMercadoria += " and mercadoria.plu = '" + txtfiltromercadoria.Text + "' ";
                }
                else
                {
                    sqlMercadoria += " and (ean like '%" + txtfiltromercadoria.Text + "%')";
                }
            }
            else
            {
                if (txtfiltromercadoria.Text.Length > 0)
                {

                    sqlMercadoria += " and (mercadoria.descricao like '%" + txtfiltromercadoria.Text + "%' or mercadoria.Ref_fornecedor like '%" + txtfiltromercadoria.Text + "%')";
                }


                if (!lblGrupo.Text.Equals(""))
                {
                    sqlMercadoria += " and codigo_grupo='" + lblGrupo.Text + "' ";
                }
                if (!lblSubGrupo.Text.Equals(""))
                {
                    sqlMercadoria += " and codigo_subgrupo ='" + lblSubGrupo.Text + "' ";

                }
                if (!lblDepartamento.Text.Equals(""))
                {
                    sqlMercadoria += " and mercadoria.codigo_departamento ='" + lblDepartamento.Text + "' ";
                }
            }

            if (!ddlLinha.Text.Equals(""))
            {
                sqlMercadoria += " and convert(varchar(3),isnull(Cod_Linha,''))+CONVERT(varchar(3),isnull(Cod_Cor_Linha,'')) ='" + ddlLinha.SelectedValue + "'";
            }

            if (!txtFornecedor.Text.Trim().Equals(""))
            {
                sqlMercadoria += " and (Fornecedor_Mercadoria.Fornecedor ='" + txtFornecedor.Text + "') ";
            }
            //if Funcoes.valorParametro("PEDIDO_SIMPLES", usr).ToUpper()
            //voltar aqui 22042015

            if(txtcodListaPadrao.Text.Length>0)
            {
                sqlMercadoria += " and mercadoria.plu in (select plu from lista_padrao_itens where id_lista =" + txtcodListaPadrao.Text + ") "; 
            }

            gridMercadoria1.DataSource = Conexao.GetTable(sqlMercadoria + " order by mercadoria.descricao", usr, limitar);
            gridMercadoria1.DataBind();




        }
        protected void btnConfirmaEncerrar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {




                solicitacao_compraDAO obj = (solicitacao_compraDAO)Session["objSolicita" + urlSessao()];

                //if (obj.existItemZerado())
                //{
                //    throw new Exception("Não é possivel Encerra a solicitação contem itens com a quantidade zerada !");
                //}

                User usr = (User)Session["User"];

                if (obj.existItemContrato())
                {
                    gridContratos.DataSource = obj.ItensContratos;
                    gridContratos.DataBind();
                    //modalContratosAtivos.Show();
                    divSemItensContrato.Visible = false;
                }
                else
                {
                    divSemItensContrato.Visible = true;
                }


                gridCotacaoAberta.DataSource = Conexao.GetTable("SELECT COTACAO='',DATA='',DESCRICAO='NOVA COTACAO'" +
                                                                  "  UNION ALL " +
                                                                  "  SELECT CONVERT(VARCHAR,COTACAO),CONVERT(VARCHAR,DATA,103)DATA , DESCRICAO FROM Cotacao WHERE Status='ABERTO'", usr, false);
                gridCotacaoAberta.DataBind();


                RadioButton rdo = (RadioButton)gridCotacaoAberta.Rows[0].FindControl("RdoListaItem");
                rdo.Checked = true;

                divPage.Visible = false;
                pnCotacaoEscolha.Visible = true;

                modalEncerrar.Hide();
                HabilitarCampos(false);
            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
            }
        }



        protected void btnCancelarEncerrar_Click(object sender, ImageClickEventArgs e)
        {
            modalEncerrar.Hide();
        }


        protected void imgBtnConfirmaContratosAtivos_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                User usr = (User)Session["User"];
                solicitacao_compraDAO obj = (solicitacao_compraDAO)Session["objSolicita" + urlSessao()];
                foreach (GridViewRow item in gridContratos.Rows)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                    if (chk.Checked)
                    {
                        Decimal qtde = 0;
                        Decimal.TryParse(item.Cells[6].Text, out qtde);
                        gerarPedidoContrato(item.Cells[2].Text, qtde);
                        int index = 0;
                        int.TryParse(item.Cells[1].Text, out index);


                    }

                }

                foreach (pedidoDAO Pd in obj.arrPedidos)
                {
                    Pd.calcularParcelaPG();
                    Pd.salvar(true);
                }


                msgShow("Pedidos Gerados com Sucesso !!",false);

             
                telaCotacao();

            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
            }
        }


        protected void gerarPedidoContrato(String plu, Decimal qtde)
        {
            if (qtde > 0)
            {
                User usr = (User)Session["User"];
                String sql = "select cf.id_contrato, cf.fornecedor,cfi.vlr,m.Embalagem,cf.prazo,cf.prazo_entrega  from Contrato_fornecedor_item as cfi " +
                               " inner join Contrato_fornecedor as cf  on cf.id_contrato = cfi.id_contrato " +
                               " inner join Contrato_Fornecedor_Filial as cff on cf.id_contrato=cff.id_contrato " +
                               " inner join mercadoria as m on m.PLU=cfi.plu " +
                            " where cfi.plu='" + plu + "' and cff.Filial='" + usr.getFilial() + "' AND CF.data_validade > '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";

                SqlDataReader rs = null;
                bool pedidoIncluido = false;
                try
                {

                    rs = Conexao.consulta(sql, usr, false);

                    solicitacao_compraDAO obj = (solicitacao_compraDAO)Session["objSolicita" + urlSessao()];

                    if (rs.Read())
                    {
                        String fornecedor = rs["Fornecedor"].ToString().Trim();
                        int emb = 1;
                        Decimal vUnitario = 0;

                        pedido_itensDAO pdItem = new pedido_itensDAO(usr);
                        pdItem.PLU = plu;
                        pdItem.Qtde = qtde;

                        int.TryParse(rs["embalagem"].ToString(), out emb);
                        pdItem.Embalagem = emb;
                        if (pdItem.Embalagem == 0)
                            pdItem.Embalagem = 1;

                        Decimal.TryParse(rs["vlr"].ToString(), out vUnitario);
                        pdItem.unitario = vUnitario;


                        foreach (pedidoDAO Pd in obj.arrPedidos)
                        {
                            if (Pd.Cliente_Fornec.Equals(fornecedor))
                            {
                                pedidoIncluido = true;
                                if (Pd.Obs.IndexOf("Valores Definidos no Contrato N°:" + rs["Id_contrato"].ToString()) < 0)
                                {
                                    Pd.Obs += "\n Valores Definidos no Contrato N°:" + rs["Id_contrato"].ToString();
                                }
                                Pd.addItens(pdItem);
                                break;
                            }
                        }

                        if (!pedidoIncluido)
                        {
                            pedidoDAO pd = new pedidoDAO(usr);
                            pd.Cliente_Fornec = fornecedor;
                            pd.Tipo = 2;
                            pd.Data_cadastro = DateTime.Now;
                            pd.Status = 1;
                            pd.Data_entrega = DateTime.Now;
                            pd.CFOP = 1101;
                            pd.Usuario = usr.getUsuario();
                            pd.funcionario = usr.getNome();
                            pd.hora = "00:00";
                            pd.orcamento = obj.codigo;
                            int dias = 0;
                            int.TryParse(rs["prazo_entrega"].ToString(), out dias);

                            pd.Obs = "Pedido Gerado por Solicitacao de compra N°:" + obj.codigo +
                                      "\n Valores Definidos no Contrato N°:" + rs["Id_contrato"].ToString() +
                                      "\n Prazo Entrega:" + rs["prazo_entrega"].ToString() + " Dias Previsto:" + DateTime.Now.AddDays(dias).ToString("dd/MM/yyyy");

                            string[] arrLinha = rs["prazo"].ToString().Split('/');
                            foreach (String item in arrLinha)
                            {
                                pedido_pagamentoDAO pg = new pedido_pagamentoDAO(usr);
                                pg.Tipo_pagamento = "BOLETO " + item + " DIAS";
                                int diaPg = 0;
                                int.TryParse(item, out diaPg);
                                pg.Vencimento = DateTime.Now.AddDays(diaPg);
                                pd.addPagamentos(pg);

                            }

                            pd.addItens(pdItem);
                            obj.arrPedidos.Add(pd);
                        }
                    }
                    obj.itemgerado(plu);
                    Session.Remove("objSolicita" + urlSessao());
                    Session.Add("objSolicita" + urlSessao(), obj);

                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    if (rs != null)
                    {
                        rs.Close();
                    }
                }
            }
        }

        protected void imgBtnCancelaContratosAtivos_Click(object sender, ImageClickEventArgs e)
        {

            modalEncerrar.Hide();
            telaCotacao();
        }
        protected void chkSelecionaMercadoria_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTodos = (CheckBox)sender;
            foreach (GridViewRow item in gridMercadoria1.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");

                if (chk != null)
                {
                    chk.Checked = chkTodos.Checked;
                    //incluirMercadoria(chk);
                }
            }
        }

        protected void chkSeleciona_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTodos = (CheckBox)sender;
            foreach (GridViewRow item in gridContratos.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");

                if (chk != null)
                {
                    chk.Checked = chkTodos.Checked;
                    //incluirMercadoria(chk);
                }
            }

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
        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            String itemLista = (String)Session["campoLista" + urlSessao()];

            exibeLista(itemLista);
        }

        protected void gridItensSelecao_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                int index = Convert.ToInt32(e.CommandArgument);
                String plu = gridMercadoriasSelecionadas.Rows[index].Cells[1].Text;
                lblPluExcluir.Text = plu;
                lbllinhaGrid.Text = index.ToString();
                modalExcluirItem.Show();
            }
            catch (Exception err)
            {
                msgShow(err.Message, true);

            }
        }

        private void ExcluirItem(String plu)
        {
            try
            {


                solicitacao_compraDAO obj = (solicitacao_compraDAO)Session["objSolicita" + urlSessao()];
                obj.excluirItem(plu);

                Session.Remove("objSolicita" + urlSessao());
                Session.Add("objSolicita" + urlSessao(), obj);
                carregarDados();
            }
            catch (Exception)
            {

                throw;
            }
        }


        protected void gridItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                int index = Convert.ToInt32(e.CommandArgument);
                String plu = gridMercadoriasSelecionadas.Rows[index].Cells[1].Text;

                carregarDadosObj();

                lbllinhaGrid.Text = index.ToString();
                lblPluExcluir.Text = plu;
                modalExcluirItem.Show();
            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
                
            }
        }

        protected void HabilitarCampos(bool enable)
        {
            EnabledControls(conteudo, enable);
            EnabledControls(cabecalho, enable);
            calculachk();


            if (status.Equals("visualizar"))
            {
                btnEncerrar.Visible = !txtstatus.Text.Equals("ENCERRADO");
            }
            else
            {
                btnEncerrar.Visible = false;
            }
            
            if (txtstatus.Text.Equals("ENCERRADO"))
            {
                EnabledControls(gridPedidos, true);
            }
            else
            {
                EnabledControls(gridPedidos, false);
            }


        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("SolicitacaoCompraDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {

            if (txtstatus.Text.Equals("ENCERRADO"))
            {
                msgShow( "Não é Permitido fazer Alterações!",true);
                

            }
            else
            {
                status = "editar";
                HabilitarCampos(true);
                carregabtn(pnBtn);
            }
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("SolicitacaoCompra.aspx");
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            msgShow("Não é possivel Excluir",true);
        }



        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                validaCamposObrigatorios();
                carregarDadosObj();
                solicitacao_compraDAO obj = (solicitacao_compraDAO)Session["objSolicita" + urlSessao()];
                obj.salvar(status.Equals("incluir"));
                status = "visualizar";

                msgShow("Salvo Com Sucesso!!",false);

                Session.Remove("objSolicita" + urlSessao());
                Session.Add("objSolicita" + urlSessao(), obj);

                carregarDados();
                carregabtn(pnBtn);
                HabilitarCampos(false);

            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
            }
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("SolicitacaoCompra.aspx");
        }

        protected override bool campoDesabilitado(Control campo)
        {
            if (campo.ID != null)
            {
                String[] campos = { "txtCodigo", 
                                   "txtUsuarioCadastro", 
                                   "txtDataCadastro", 
                                    "TxtSaldoAtual",
                                    "txtstatus",
                                    "ddlTipoSolicitacao",
                                    "txtGrupo",
                                    "txtSubGrupo",
                                    "txtDepartamento"
                                    ,"txtDescricaoListaPadrao"
                              };

                return existeNoArray(campos, campo.ID.Trim());
            }
            else
                return false;
        }

        protected override bool campoObrigatorio(Control campo)
        {
            String[] campos = { "txtDescricao"
                                     };
            return existeNoArray(campos, campo.ID + "");
        }

        protected void imgBtnFechar_Click(object sender, ImageClickEventArgs e)
        {
            divPage.Visible = true;
            pnCotacaoEscolha.Visible = false;

        }

        private void encerrarSolicitacao()
        {
            try
            {


                solicitacao_compraDAO obj = (solicitacao_compraDAO)Session["objSolicita" + urlSessao()];
                obj.status = "ENCERRADO";
                obj.salvar(false);
                foreach (pedidoDAO ped in obj.arrPedidos)
                {
                    enviarEmailPedido(ped.Pedido);
                }
            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
                divPage.Visible = true;

                throw err;
            }
        }

        private void addItens(cotacaoDAO cotacao)
        {
            try
            {
                solicitacao_compraDAO obj = (solicitacao_compraDAO)Session["objSolicita" + urlSessao()];


                User usr = (User)Session["User"];
                foreach (solicitacao_compra_itensDAO objItem in obj.arrItens)
                {

                    if (!objItem.gerado)
                    {

                        if (objItem.qtde_comprar > 0)
                        {
                            cotacao_itemDAO item = new cotacao_itemDAO(usr);
                            item.Mercadoria = objItem.plu;
                            item.descricao = objItem.descricao;
                            item.embalagem = objItem.embalagem;
                            item.Quantidade = (objItem.qtde_comprar / (item.embalagem==0?1: item.embalagem));
                            item.preco_compra = objItem.preco_custo;
                            cotacao.addItem(item);
                        }
                    }


                }
            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
            }
            Session.Remove("CotacaoManter");
            Session.Add("CotacaoManter", cotacao);
        }

        protected void imgBtnConfirmaExluirItem_Click(object sender, EventArgs e)
        {
            ExcluirItem(lblPluExcluir.Text);
            modalExcluirItem.Hide();
            int linha = 0;
            int.TryParse(lbllinhaGrid.Text, out linha);

            if (TabContainer1.ActiveTabIndex == 0)
            {
                CheckBox chk = new CheckBox();

                if (gridItens.Rows.Count > linha)
                {
                    chk = (CheckBox)gridItens.Rows[linha].FindControl("chkSelecionaItem");

                }
                else
                {
                    chk = (CheckBox)gridItens.Rows[gridItens.Rows.Count - 1].FindControl("chkSelecionaItem");
                }
                chk.Focus();


            }
            else
            {

                if (gridMercadoriasSelecionadas.Rows.Count > linha)
                {

                    gridMercadoriasSelecionadas.Rows[linha].Focus();
                }
                else
                {
                    gridMercadoriasSelecionadas.Rows[gridMercadoriasSelecionadas.Rows.Count - 1].Focus();

                }
            }


        }

        protected void imgBtnCancelaExluirItem_Click(object sender, EventArgs e)
        {
            modalExcluirItem.Hide();
        }


        protected void gridPedido_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int index = Convert.ToInt32(e.CommandArgument);
            HyperLink meuLink = (HyperLink)gridPedidos.Rows[index].Cells[0].Controls[0];
            lblPedidoEmail.Text = meuLink.Text;

            modalConfirmaEmail.Show();

        }

        protected void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            enviarEmailPedido(lblPedidoEmail.Text);
            modalConfirmaEmail.Hide();
        }

        private void enviarEmailPedido(String NumeroPedido)
        {
            try
            {


                User usr = (User)Session["user"];
                pedidoDAO ped = new pedidoDAO(NumeroPedido, 2, usr);
                fornecedorDAO fornec = new fornecedorDAO(ped.Cliente_Fornec, usr);
                String arquivo = Server.MapPath("") + "\\EmailPedido.html";
                StreamReader objStreamReader = File.OpenText(arquivo);
                String conteudo = objStreamReader.ReadToEnd();
                objStreamReader.Close();

                conteudo = conteudo.Replace("<|RazaoSocial|>", usr.filial.Razao_Social);
                conteudo = conteudo.Replace("<|cnpj|>", usr.filial.CNPJ);
                conteudo = conteudo.Replace("<|Ie|>", usr.filial.IE);
                conteudo = conteudo.Replace("<|endereco|>", usr.filial.Endereco);
                conteudo = conteudo.Replace("<|numero|>", usr.filial.endereco_nro);
                conteudo = conteudo.Replace("<|complemento|>", "");
                conteudo = conteudo.Replace("<|cep|>", usr.filial.CEP);
                conteudo = conteudo.Replace("<|bairro|>", usr.filial.bairro);
                conteudo = conteudo.Replace("<|cidade|>", usr.filial.Cidade);
                conteudo = conteudo.Replace("<|uf|>", usr.filial.UF);
                conteudo = conteudo.Replace("<|Telefone|>", usr.filial.fone);

                conteudo = conteudo.Replace("<|FornecedorRazaoSocial|>", fornec.Razao_social);
                conteudo = conteudo.Replace("<|fornecedorcnpj|>", fornec.CNPJ);
                conteudo = conteudo.Replace("<|fornecedorIe|>", fornec.IE);
                conteudo = conteudo.Replace("<|Fornecedorendereco|>", fornec.Endereco);
                conteudo = conteudo.Replace("<|Fornecedornumero|>", fornec.Endereco_nro);
                conteudo = conteudo.Replace("<|Fornecedorcomplemento|>", "");
                conteudo = conteudo.Replace("<|Fornecedorcep|>", fornec.CEP);
                conteudo = conteudo.Replace("<|Fornecedorbairro|>", fornec.Bairro);
                conteudo = conteudo.Replace("<|Fornecedorcidade|>", fornec.Cidade);
                conteudo = conteudo.Replace("<|Fornecedoruf|>", fornec.UF);

                conteudo = conteudo.Replace("<|NumeroPedido|>", ped.Pedido);
                conteudo = conteudo.Replace("<|Data|>", ped.Data_cadastroBr());
                conteudo = conteudo.Replace("<|Total|>", ped.Total.ToString("N2"));

                String strItens = "";
                foreach (pedido_itensDAO item in ped.PedItens)
                {
                    String sqlPlu = "Select top 1 m.PLU, ean.EAN, ISNULL((select top 1 codigo_referencia from Fornecedor_Mercadoria where Fornecedor='" + fornec.Fornecedor + "' AND PLU=M.PLU),'') AS CODIGO_REFERENCIA " +
                                " from mercadoria m left join EAN on m.plu = ean.PLU  " +
                                " where m.plu = '" + item.PLU + "' ";
                    SqlDataReader rsPlu = Conexao.consulta(sqlPlu, null, false);
                    String ean = "";
                    String refFornec = "";
                    if (rsPlu.Read())
                    {
                        ean = rsPlu["ean"].ToString();
                        refFornec = rsPlu["codigo_referencia"].ToString();
                    }

                    if (rsPlu != null)
                        rsPlu.Close();


                    strItens += "<tr>";
                    strItens += "<td style=\"text-align:left; \">" + item.PLU + "</td>";
                    strItens += "<td style=\"text-align:left;\">" + ean + "</td>";
                    strItens += "<td style=\"text-align:left;\">" + refFornec + "</td>";
                    strItens += "<td style=\"text-align:left;\">" + item.Descricao + "</td>";
                    strItens += "<td style=\"text-align:right; \">" + item.Qtde + "</td>";
                    strItens += "<td style=\"text-align:right; \">" + item.Embalagem + "</td>";
                    strItens += "<td style=\"text-align:right; \">" + item.unitario.ToString("N2") + "</td>";
                    strItens += "<td style=\"text-align:right; \">" + item.total.ToString("N2") + "</td>";
                    strItens += "</tr>";

                }
                conteudo = conteudo.Replace("<|ITENS|>", strItens);
                String strPagamentos = "";
                foreach (pedido_pagamentoDAO item in ped.PedPg)
                {

                    strPagamentos += "<tr>";
                    strPagamentos += "<td style=\"text-align:left; \">" + item.Tipo_pagamento + "</td>";
                    strPagamentos += "<td style=\"text-align:right;\">" + item.Valor.ToString("N2") + "</td>";
                    strPagamentos += "</tr>";
                }
                conteudo = conteudo.Replace("<|PAGAMENTOS|>", strPagamentos);

                Funcoes.enviarEmail(usr, fornec.email, "", "PEDIDO DE COMPRA", conteudo);
                
                msgShow("Email Enviado com Sucesso!", false);
                lblError.ForeColor = System.Drawing.Color.Blue;
            }
            catch (Exception err)
            {

                msgShow(err.Message,true);
             
            }

        }
        protected void btnCancela_Click(object sender, EventArgs e)
        {
            modalConfirmaEmail.Hide();
        }

        protected void gridCotacaoAberta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoListaItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('gridCotacaoAberta.*GrCotacaoItem',this)";
            rdo.Attributes.Add("onclick", script);
        }


        private string cotacaoSelecionada()
        {
            foreach (GridViewRow item in gridCotacaoAberta.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoListaItem");
                if (rdo.Checked)
                {
                    return item.Cells[0].Text.Replace("&nbsp;","");
                }

            }
            return "";
        }

        private void telaCotacao()
        {
            try
            {

                solicitacao_compraDAO obj = (solicitacao_compraDAO)Session["objSolicita" + urlSessao()];
                User usr = (User)Session["User"];
                if (obj.itensCotacao())
                {
                    String strCotacao = cotacaoSelecionada();
                    cotacaoDAO cotacao = null;
                    if (strCotacao.Equals(""))
                    {
                        cotacao = new cotacaoDAO(usr);
                        addItens(cotacao);
                        cotacao.Usuario = usr.getUsuario();
                        cotacao.Status = "ABERTO";
                        cotacao.Data = DateTime.Now;
                        cotacao.descricao = obj.descricao;
                        cotacao.salvar(true);
                    }
                    else
                    {
                        cotacao = new cotacaoDAO(strCotacao, usr);
                        addItens(cotacao);
                        cotacao.salvar(false);
                    }

                    obj.arrCotacoes.Add(cotacao);

                    Session.Remove("objSolicita" + urlSessao());
                    Session.Add("objSolicita" + urlSessao(), obj);

                    

                    RedirectNovaAba("~/modulos/estoque/pages/cotacaoDetalhes.aspx?campoIndex=" + cotacao.Cotacao);
                }
                divPage.Visible = true;
                pnCotacaoEscolha.Visible = false;
                encerrarSolicitacao();
                carregarDados();

            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
                divPage.Visible = true;
                pnCotacaoEscolha.Visible = false;
            }
        }

        protected void btnSalvarListaPadrao_Click(object sender, EventArgs e)
        {
            modalSalvarListaPadrao.Show();
            txtNomeNovaListaPadrao.Text = "";
            txtNomeNovaListaPadrao.Focus();
        }

        protected void imgBtnSalvarListaPadrao_Click(object sender, EventArgs e)
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            User usr = (User)Session["User"];
            try
            {
                String sql = "insert into LISTA_PADRAO values('" + txtNomeNovaListaPadrao.Text.Trim() + "');";
                String sqlItens = "";

                solicitacao_compraDAO obj = (solicitacao_compraDAO)Session["objSolicita" + urlSessao()];
                Conexao.executarSql(sql,conn,tran);
                usr.consultaTodasFiliais = true;
                string codLista = Conexao.retornaUmValor("select IDENT_CURRENT( 'LISTA_PADRAO' ) ", usr, conn, tran);
                usr.consultaTodasFiliais = false;
                foreach (solicitacao_compra_itensDAO item in obj.arrItens)
                {
                    if (sqlItens.Length > 0)
                        sqlItens += ",";

                    sqlItens += "(" + codLista + ",'" + item.plu + "')";
                }

                Conexao.executarSql("insert into LISTA_PADRAO_ITENS values " + sqlItens,conn,tran);

                
                msgShow("Salvo Lista padrao!", false);
                tran.Commit();
            }
            catch (Exception err)
            {
                tran.Rollback();
                msgShow( err.Message,true);
                
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
           

        }

        protected void btnDigitado_Click(object sender, EventArgs e)
        {
            carregarDadosObj();

            solicitacao_compraDAO obj = (solicitacao_compraDAO)Session["objSolicita" + urlSessao()];
            if (obj.itensDigitados)
            {
                obj.itensDigitados = false;
                btnItensDigitados.Text = "DIGITADOS";
            }
            else
            {
                obj.itensDigitados = true;
                btnItensDigitados.Text = "TODOS";
            }


            Session.Remove("objSolicita" + urlSessao());
            Session.Add("objSolicita" + urlSessao(), obj);
            carregarDados();

            if (status.Equals("visualizar"))
            {
                EnabledControls(gridItens, false);
                EnabledControls(gridMercadoriasSelecionadas, false);

            }


        }

        protected void txtcodListaPadrao_TextChanged(object sender, EventArgs e)
        {
            if (!txtcodListaPadrao.Text.Equals(""))
            {
                txtDescricaoListaPadrao.Text = Conexao.retornaUmValor("Select descricao from LISTA_PADRAO WHERE ID =" + txtcodListaPadrao.Text, null);

                carregarMercadorias(false);

            }
            else
            {
                carregarMercadorias(true);
                txtDescricaoListaPadrao.Text = "";
            }

        }

        protected void btnOkError_Click(object sender, EventArgs e)
        {
            if (status.Equals("visualizar"))
            {
                HabilitarCampos(false);
            }
            else
            {
                HabilitarCampos(true);

            }
            modalError.Hide();
        }

        protected void msgShow(String mensagem, bool erro)
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
            btnOkError.Focus();
            modalError.Show();
        }
    }
}