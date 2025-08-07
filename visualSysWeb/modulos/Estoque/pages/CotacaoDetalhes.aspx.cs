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
using visualSysWeb.code;
using System.IO;

namespace visualSysWeb.modulos.Estoque.pages
{
    public partial class CotacaoDetalhes : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];

            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    cotacaoDAO obj = null;
                    status = "incluir";
                    cotacaoDAO manterCotacao = (cotacaoDAO)Session["CotacaoManter"];
                    if (manterCotacao != null)
                    {
                        obj = manterCotacao;

                    }
                    else
                    {
                        obj = new cotacaoDAO(usr);
                    }

                    obj.Usuario = usr.getUsuario();
                    obj.Status = "ABERTO";
                    obj.Data = DateTime.Now;
                    Session.Remove("cotacao" + urlSessao());
                    Session.Add("cotacao" + urlSessao(), obj);
                    carregarDados();
                    habilitar(true);
                }
            }
            else
            {
                if (Request.Params["campoIndex"] != null)  // colocar o campo index da tabela
                {
                    try
                    {
                        if (!IsPostBack)
                        {
                            cotacaoDAO obj = null;
                            cotacaoDAO manterCotacao = (cotacaoDAO)Session["CotacaoManter"];
                            if (manterCotacao != null)
                            {
                                obj = manterCotacao;
                                status = "editar";

                            }
                            else
                            {

                                String index = Request.Params["campoIndex"].ToString();// colocar o campo index da tabela
                                status = "visualizar";
                                obj = new cotacaoDAO(index, usr);
                            }
                            Session.Remove("cotacao" + urlSessao());
                            Session.Add("cotacao" + urlSessao(), obj);

                            carregarDados();
                        }
                        if (status.Equals("visualizar"))
                        {
                            habilitar(false);
                        }
                        else
                        {
                            habilitar(true);
                        }

                    }
                    catch (Exception err)
                    {
                        lblError.Text = err.Message;
                    }
                }
            }
            carregabtn(pnBtn);
        }

        private void habilitar(bool enable)
        {
            EnabledControls(conteudo, enable);
            EnabledControls(cabecalho, enable);
            EnabledControls(gridItens, enable);
            EnabledControls(GridItensMercadoria, true);
            btnFinalizar.Visible = (!ddlStatus.Text.Equals("FINALIZADA") && !enable);

            User usr = (User)Session["user"];
            if (!usr.host.Equals(""))
            {
                if (!ddlStatus.Text.Equals("FINALIZADA"))
                {
                    EnabledButtons(gridPedido, false);
                    
                    divEnviarTodosEmails.Visible = false;
                }
                else
                {
                    imgBtnEnviarTodosEmails.Visible = true;
                    divEnviarTodosEmails.Visible = true;
                    EnabledButtons(gridPedido, true);
                }
            }
            else
            {
                divEnviarTodosEmails.Visible = false;
                gridPedido.Columns[3].Visible = false;
            }

            divBtnImprimirNaoCotados.Visible = !enable;
            imgBtnImpNaoCotados.Visible = !enable;
        }

        private void limparCampos()
        {
            LimparCampos(cabecalho);
            LimparCampos(conteudo);
        }

        protected bool validaCamposObrigatorios()
        {
            if (validaCampos(cabecalho) && validaCampos(conteudo))
                return true;
            else
                return false;
        }

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array
            String[] campos = { "", 
                                    "", 
                                    "", 
                                    "" 
                                     };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array
            String[] campos = { "txtCotacao", 
                                    "txtUsuario", 
                                    "ddlStatus", 
                                    "" 
                                     };
            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Session.Remove("CotacaoManter");
            Response.Redirect("CotacaoDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            if (!ddlStatus.Text.Equals("FINALIZADA"))
            {
                status = "editar";
                editar(pnBtn);
                habilitar(true);
                carregarDados();

                lblError.Text = "";
            }
            else
            {
                lblError.Text = "Não é permitido Alterar Cotações já finalizada!";
            }
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Session.Remove("CotacaoManter");
            Response.Redirect("Cotacao.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            //pnConfima.Visible = true;
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            salvar();
        }

        protected void salvar()
        {
            try
            {
                if (validaCamposObrigatorios())
                {

                    carregarDadosObj();
                    cotacaoDAO obj = (cotacaoDAO)Session["cotacao" + urlSessao()];
                    obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;
                    Session.Remove("cotacao" + urlSessao());
                    Session.Add("cotacao" + urlSessao(), obj);
                    carregarDados();
                    habilitar(false);
                    visualizar(pnBtn);
                    Session.Remove("CotacaoManter");
                }
                else
                {
                    lblError.Text = "Campo Obrigatorio n?o preenchido";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception err)
            {
                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Session.Remove("CotacaoManter");
            Response.Redirect("Cotacao.aspx"); //colocar o endereco da tela de pesquisa
        }

        //--Atualizar DaoForm 
        private void carregarDados()
        {

            cotacaoDAO obj = (cotacaoDAO)Session["cotacao" + urlSessao()];
            if (obj != null)
            {
                User usr = (User)Session["user"];
                txtCotacao.Text = (obj.Cotacao.ToString().Equals("0") ? "" : obj.Cotacao.ToString());
                txtUsuario.Text = obj.Usuario.ToString();
                txtData.Text = obj.DataBr();
                txtdescricao.Text = obj.descricao.ToString();
                ddlStatus.Text = obj.Status.Trim();
                carregarGrids();
            }
        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            cotacaoDAO obj = (cotacaoDAO)Session["cotacao" + urlSessao()];

            obj.Cotacao = (txtCotacao.Text.Equals("") ? 0 : int.Parse(txtCotacao.Text));
            obj.Usuario = txtUsuario.Text;
            obj.Data = (txtData.Text.Equals("") ? new DateTime() : DateTime.Parse(txtData.Text));
            obj.Status = ddlStatus.Text;
            obj.descricao = txtdescricao.Text;

            foreach (GridViewRow rw in gridItens.Rows)
            {
                TextBox txtQtdeDig = (TextBox)rw.FindControl("txtQtdItem");
                TextBox txtEmbDig = (TextBox)rw.FindControl("txtEmbalagem");
                
                if(!txtQtdeDig.Text.Equals("------"))
                {
                    cotacao_itemDAO item = obj.item(rw.RowIndex);
                    item.Quantidade = (txtQtdeDig.Text.Equals("") ? 1 : Decimal.Parse(txtQtdeDig.Text));
                    item.embalagem = (txtEmbDig.Text.Equals("") ? 1 : Decimal.Parse(txtEmbDig.Text));
                    //obj.atualizaitem(item);
                }

            }


            Session.Remove("cotacao" + urlSessao());
            Session.Add("cotacao" + urlSessao(), obj);

        }


        protected void lista_click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                cotacaoDAO obj = (cotacaoDAO)Session["cotacao" + urlSessao()];

                obj.excluir();

                lblError.Text = "Registro Excluido com sucesso";
                limparCampos();
                pesquisar(pnBtn);
            }
            catch (Exception err)
            {
                lblError.Text = "Não foi possivel Excluir o registro error:" + err.Message;
            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {

        }




        protected void gridItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            cotacaoDAO obj = (cotacaoDAO)Session["cotacao" + urlSessao()];
            obj.removeItem(obj.item(index));
            carregarDados();
            EnabledButtons(gridItens, true);

        }

        protected void ImgBtnAddItens_Click(object sender, ImageClickEventArgs e)
        {
            carregarDadosObj();
            carregarGrupos("", "", "");
            limparSelecaoMercadoria();
            
            carregarMercadorias();
            modalMercadorialista.Show();
        }
        protected void RdoItem_CheckedChanged(object sender, EventArgs e)
        {
            CarregarFornecedor();
        }

        protected void CarregarFornecedor()
        {
            User usr = (User)Session["user"];
            String strplu = MercadoriaSelecionado(1);
            String strSql = "SELECT Cotacao_digitacao.Mercadoria, Cotacao_digitacao.Preco, Cotacao_digitacao.Qtde,Cotacao_digitacao.Embalagem, Cotacao_digitacao.Prazo_pgto,  Cotacao_digitacao.Prazo_entrega, Cotacao_digitacao.Usuario, Mercadoria.Descricao, Mercadoria.preco_compra, Cotacao_digitacao.Fornecedor  " +
                                ",OBS= (SELECT substring(OBS,1,10)+'...' FROM COTACAO_OBS_FORNECEDOR WHERE COTACAO = '" + txtCotacao.Text + "' AND FILIAL = '"+usr.getFilial()+ "'  and fornecedor =Cotacao_digitacao.Fornecedor )" +
                                "FROM  dbo.Cotacao_digitacao Cotacao_digitacao   " +
                                "	INNER JOIN dbo.Mercadoria Mercadoria  ON  Cotacao_digitacao.Filial = Mercadoria.Filial AND  Cotacao_digitacao.Mercadoria = Mercadoria.PLU  " +
                                "WHERE (  Cotacao_digitacao.Cotacao = '" + txtCotacao.Text + "'  AND  Cotacao_digitacao.Filial = '" + usr.getFilial() + "'  AND  Cotacao_digitacao.Mercadoria = '" + strplu + "' )  " +
                                "ORDER BY Cotacao_digitacao.Preco";
            gridFornecedor.DataSource = Conexao.GetTable(strSql, null, false);
            gridFornecedor.DataBind();
        }
        protected String MercadoriaSelecionado(int campo)
        {
            foreach (GridViewRow item in GridItensMercadoria.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoItem");

                if (rdo != null)
                {
                    if (rdo.Checked)
                    {
                        item.RowState = DataControlRowState.Selected;
                        rdo.Focus();
                        return item.Cells[campo].Text;
                    }
                }
            }

            return "";
        }


        protected void GridItensMercadoria_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('GridItensMercadoria.*Gritem',this)";
            rdo.Attributes.Add("onclick", script);

        }
        protected void GridMercadoria1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void btnFecharMecadoria_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                // verificaSelecionados();
                User usr = (User)Session["User"];
                ArrayList selecionados = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

                if (selecionados != null && selecionados.Count > 1)
                {
                    //GridMercadoriaSelecionado.DataSource = Conexao.GetArryTable(selecionados);
                    //GridMercadoriaSelecionado.DataBind();

                    cotacaoDAO obj = (cotacaoDAO)Session["cotacao" + urlSessao()];



                    foreach (GridViewRow sl in GridMercadoriaSelecionado.Rows)
                    {
                        cotacao_itemDAO item = new cotacao_itemDAO(usr);

                        item.Mercadoria = sl.Cells[1].Text;
                        TextBox txtQtdItem = (TextBox)sl.FindControl("txtQtd");
                        TextBox txtEmbItem = (TextBox)sl.FindControl("txtEmb");
                        if (!txtQtdItem.Text.Equals("------"))
                        {
                            item.EAN = sl.Cells[2].Text;
                            item.Quantidade = Funcoes.decTry(txtQtdItem.Text);
                            item.descricao = sl.Cells[4].Text;
                            item.embalagem = Funcoes.decTry(txtEmbItem.Text);
                            item.preco_compra = Funcoes.decTry(sl.Cells[7].Text);
                            item.inserido = true;
                            if (GridMercadoriaSelecionado.Rows.Count == 1)
                                obj.addItem(item);
                            else
                            {
                                try
                                {
                                    obj.addItem(item);
                                }
                                catch (Exception)
                                {
                                }
                            }

                        }
                        else
                        {
                            break;
                        }


                    }

                    obj.ordernarItens();
                    Session.Remove("cotacao" + urlSessao());
                    Session.Add("cotacao" + urlSessao(), obj);

                    carregarDados();

                    modalMercadorialista.Hide();


                }
                else
                {
                    modalMercadorialista.Show();
                }
            }
            catch (Exception err)
            {
                lblMercadoriaLista.Text = err.Message;
                lblMercadoriaLista.ForeColor = System.Drawing.Color.Red;
                modalMercadorialista.Show();

            }

        }
        protected void btnCancelaMercadoria_Click(object sender, ImageClickEventArgs e)
        {
            modalMercadorialista.Hide();
            carregarDados();
        }
        protected void ImgPesquisaMercadoria_Click(object sender, ImageClickEventArgs e)
        {
            carregarMercadorias();
        }
        protected void imgLimpar_Click(object sender, ImageClickEventArgs e)
        {
            limparSelecaoMercadoria();
            carregarMercadorias();
        }
        protected void limparSelecaoMercadoria()
        {

            ddlGrupo.Text = "";
            ddlSubGrupo.Text = "";
            ddlDepartamento.Text = "";
            txtfiltromercadoria.Text = "";
            

            ArrayList todosSel = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            if (todosSel != null)
            {
                todosSel.Clear();
                ArrayList cabecalho = new ArrayList();
                cabecalho.Add("PLU");
                cabecalho.Add("EAN");
                cabecalho.Add("Referencia");
                cabecalho.Add("Descricao");
                cabecalho.Add("QTD");
                cabecalho.Add("embalagem");
                cabecalho.Add("Preco");
                cabecalho.Add("PrecoPadrao");
                todosSel.Add(cabecalho);
                Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), todosSel);

            }
            Session.Remove("plusAdd" + urlSessao());
        }
        protected void carregarMercadorias()
        {
            //verificaSelecionados();
            lblMercadoriaLista.Text = "Inclusão de Produto";
            //lblMercadoriaLista.ForeColor = Label1.ForeColor;



            User usr = (User)Session["user"];
            String sqlMercadoria = "Select mercadoria.plu PLU,isnull(ean.ean,'---')EAN,mercadoria.Ref_fornecedor REFERENCIA,mercadoria.descricao DESCRICAO, mercadoria_loja.preco as [PRC COMPRA], mercadoria_loja.saldo_atual as SALDO,MERCADORIA.EMBALAGEM  from mercadoria inner join mercadoria_loja on mercadoria.plu = mercadoria_loja.plu left join ean on mercadoria.plu=ean.plu  inner join W_BR_CADASTRO_DEPARTAMENTO on mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento " +
                                    " where isnull(mercadoria.inativo,0)=0 and  (mercadoria_loja.filial='" + usr.getFilial() + "') and ((mercadoria.plu = '" + txtfiltromercadoria.Text + "') or (ean like '%" + txtfiltromercadoria.Text + "%') or (mercadoria.descricao like '%" + txtfiltromercadoria.Text + "%') or mercadoria.Ref_fornecedor like '%" + txtfiltromercadoria.Text + "%')";


            if (!ddlGrupo.Text.Equals(""))
            {
                sqlMercadoria += " and codigo_grupo='" + ddlGrupo.SelectedValue + "' ";
            }
            if (!ddlSubGrupo.Text.Equals(""))
            {
                sqlMercadoria += " and codigo_subgrupo ='" + ddlSubGrupo.SelectedValue + "' ";

            }
            if (!ddlDepartamento.Text.Equals(""))
            {
                sqlMercadoria += " and mercadoria.codigo_departamento ='" + ddlDepartamento.SelectedValue + "' ";
            }


            //if Funcoes.valorParametro("PEDIDO_SIMPLES", usr).ToUpper()
            //voltar aqui 22042015

            gridMercadoria1.DataSource = Conexao.GetTable(sqlMercadoria + " order by mercadoria.descricao ", usr, !(txtfiltromercadoria.Text.Length > 0));
            gridMercadoria1.DataBind();
            
            ArrayList todosSel = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            if (todosSel == null)
            {
                todosSel = new ArrayList();
                ArrayList cabecalho = new ArrayList();
                cabecalho.Add("PLU");
                cabecalho.Add("EAN");
                cabecalho.Add("Referencia");
                cabecalho.Add("Descricao");
                cabecalho.Add("QTD");
                cabecalho.Add("embalagem");
                cabecalho.Add("Preco");
                cabecalho.Add("PrecoPadrao");
                todosSel.Add(cabecalho);
            }

            if (todosSel.Count > 1)
            {

                for (int i = 0; i < GridMercadoriaSelecionado.Rows.Count; i++)
                {
                    TextBox txtQtdItem = (TextBox)GridMercadoriaSelecionado.Rows[i].FindControl("txtQtd");
                    if (!txtQtdItem.Text.Equals("------"))
                    {
                        ArrayList ls = (ArrayList)todosSel[i + 1];
                        ls[4] = txtQtdItem.Text;



                    }
                }
            }
            Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), todosSel);
            GridMercadoriaSelecionado.DataSource = Conexao.GetArryTable(todosSel);
            GridMercadoriaSelecionado.DataBind();

            modalMercadorialista.Show();
            carregarGrids();

        }
        private void carregarGrids()
        {
            User usr = (User)Session["User"];
            cotacaoDAO obj = (cotacaoDAO)Session["cotacao" + urlSessao()];
            gridItens.DataSource = obj.cotacaoItens();
            gridItens.DataBind();

            GridItensMercadoria.DataSource = obj.cotacaoItens();
            GridItensMercadoria.DataBind();

            GridViewRow item = (GridViewRow)GridItensMercadoria.Rows[0];
            CheckBox chk = (CheckBox)item.FindControl("RdoItem");
            chk.Checked = true;
            CarregarFornecedor();

            if(obj.Status.Equals("FINALIZADA"))
            {
                tabItensNaoCotados.Visible = true;
                gridItensNaoCotados.DataSource = obj.itensNaoCotados();
                gridItensNaoCotados.DataBind();
            }
            else
            {
                tabItensNaoCotados.Visible = false;

            }


            gridPedido.DataSource = Conexao.GetTable("Select pedido , Fornecedor= cliente_fornec , total,cotacao from pedido where cotacao ='" + (obj.Cotacao.ToString().Equals("0") ? "-1" : obj.Cotacao.ToString()) + "' ", usr, false);
            gridPedido.DataBind();
        }

        protected void txtfiltromercadoria_TextChanged(object sender, EventArgs e)
        {
            carregarMercadorias();
        }

        protected void chkSeleciona_CheckedChanged(object sender, EventArgs e)
        {

            foreach (GridViewRow item in gridMercadoria1.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");

                if (chk != null)
                {
                    chk.Checked = true;
                    //incluirMercadoria(chk);
                }
            }

            ArrayList todosSel = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            GridMercadoriaSelecionado.DataSource = Conexao.GetArryTable(todosSel);
            GridMercadoriaSelecionado.DataBind();
            modalMercadorialista.Show();
        }

        private void verificaSelecionados()
        {
            foreach (GridViewRow item in gridMercadoria1.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                if (chk.Checked)
                {
                    incluirMercadoria(chk);
                }
            }
        }

        //protected void chkSelecionaItem_CheckedChanged(object sender, EventArgs e)
        //{
        //    CheckBox ck1 = (CheckBox)sender;
        //    if (ck1.Checked)
        //    {

        //        incluirMercadoria(ck1);
        //        ArrayList todosSel = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

        //        GridMercadoriaSelecionado.DataSource = Conexao.GetArryTable(todosSel);
        //        GridMercadoriaSelecionado.DataBind();
        //        ck1.Focus();
        //        modalMercadorialista.Show();
        //    }
        //    else
        //    {
        //        ck1.Checked = true;
        //        modalMercadorialista.Show();
        //    }

        // }

        private void incluirMercadoria(CheckBox ck1)
        {
            GridViewRow linha = (GridViewRow)ck1.NamingContainer;


            ArrayList todosSel = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            User usr = (User)Session["User"];
            Hashtable tb = (Hashtable)Session["plusAdd" + urlSessao()];
            bool addItem = true;
            if (tb == null)
            {
                tb = new Hashtable();
                tb.Add(linha.Cells[1].Text, linha.Cells[1].Text);
                addItem = true;
            }
            else
            {
                if (tb.ContainsKey(linha.Cells[1].Text))
                {
                    addItem = false;
                }
                else
                {
                    tb.Add(linha.Cells[1].Text, linha.Cells[1].Text);
                    addItem = true;
                }
            }
            Session.Remove("plusAdd" + urlSessao());
            Session.Add("plusAdd" + urlSessao(), tb);
            if (addItem)
            {
                ArrayList sel = new ArrayList();
                
                //MercadoriaDAO merc = new MercadoriaDAO(linha.Cells[1].Text, usr);

                sel.Add(linha.Cells[1].Text.Replace("&nbsp;","")); // plu
                sel.Add(linha.Cells[2].Text.Replace("&nbsp;", "")); //EAN

                sel.Add(linha.Cells[3].Text.Replace("&nbsp;", "")); //Referencia
                sel.Add(linha.Cells[4].Text.Replace("&nbsp;", "")); //Descricao
                sel.Add("1"); // QTDE
                sel.Add(linha.Cells[7].Text.Replace("&nbsp;", "")); // embalagem
                sel.Add(linha.Cells[5].Text.Replace("&nbsp;", "")); // preco_compra
                sel.Add(linha.Cells[5].Text.Replace("&nbsp;", "")); // preco_compra
                todosSel.Add(sel);

                ViewState["gridLinha"] = todosSel.Count - 2;
                Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), todosSel);
            }
            //carregarMercadorias();
            //modalMercadorialista.Show();

        }

        protected void GridMercadoriaSelecionado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
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
                    //e.Row.Cells[0].Focus();
                    e.Row.FindControl("txtQtd").Focus();

                }
            }
        }
        protected void GridMercadoriaSelecionado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument) + 1;
            ArrayList selecionados = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            selecionados.RemoveAt(index);
            Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), selecionados);
            GridMercadoriaSelecionado.DataSource = Conexao.GetArryTable(selecionados);
            GridMercadoriaSelecionado.DataBind();

            modalMercadorialista.Show();
        }

        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarGrupos(ddlGrupo.Text, "", "");
            carregarMercadorias();
        }
        protected void ddlSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarGrupos(ddlGrupo.Text, ddlSubGrupo.Text, "");
            carregarMercadorias();
        }

        protected void ddlDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarGrupos(ddlGrupo.Text, ddlSubGrupo.Text, ddlDepartamento.Text);
            carregarMercadorias();
        }


        private void carregarGrupos(String grupo, String subGrupo, String departamento)
        {

            String sqlGrupo = "select Codigo_Grupo,Descricao_Grupo from Grupo";
            String sqlSubGrupo = "Select codigo_subgrupo, descricao_subgrupo from subgrupo " + (!ddlGrupo.Text.Equals("") ? " where codigo_grupo='" + ddlGrupo.SelectedValue + "'" : "");
            String sqlDepartamento = "Select codigo_departamento, descricao_departamento from W_BR_CADASTRO_DEPARTAMENTO ";
            String sqlWhereDep = "";
            if (!ddlGrupo.Text.Equals(""))
            {
                sqlWhereDep = " where codigo_grupo='" + ddlGrupo.SelectedValue + "'";
            }
            if (!ddlSubGrupo.Text.Equals(""))
            {
                if (sqlWhereDep.Length > 0)
                    sqlWhereDep += " and ";
                else
                    sqlWhereDep += " where ";

                sqlWhereDep += " codigo_subgrupo='" + ddlSubGrupo.SelectedValue + "'";
            }

            Conexao.preencherDDL1Branco(ddlGrupo, sqlGrupo, "Descricao_grupo", "codigo_grupo", null);
            Conexao.preencherDDL1Branco(ddlSubGrupo, sqlSubGrupo, "descricao_subgrupo", "codigo_subgrupo", null);
            Conexao.preencherDDL1Branco(ddlDepartamento, sqlDepartamento + sqlWhereDep, "descricao_departamento", "codigo_departamento", null);
            ddlGrupo.Text = grupo;
            ddlSubGrupo.Text = subGrupo;
            ddlDepartamento.Text = departamento;

            modalMercadorialista.Show();

        }

        protected void txtEmbalagemItem_TextChanged(object sender, EventArgs e)
        {
            cotacaoDAO obj = (cotacaoDAO)Session["cotacao" + urlSessao()];
            TextBox txtEmbalagem = (TextBox)sender;
            txtEmbalagem.BackColor = System.Drawing.Color.White;

            GridViewRow linha = (GridViewRow)txtEmbalagem.NamingContainer;

            cotacao_itemDAO item = obj.item(linha.RowIndex);
            try
            {


                item.embalagem = Decimal.Parse(txtEmbalagem.Text);
                txtEmbalagem.Text = item.embalagem.ToString("N2");
                obj.atualizaitem(item);
                Session.Remove("cotacao" + urlSessao());
                Session.Add("cotacao" + urlSessao(), obj);
                EnabledButtons(gridItens, true);
            }
            catch (Exception)
            {
                txtEmbalagem.BackColor = System.Drawing.Color.Red;

            }
            txtEmbalagem.Focus();
            linha.RowState = DataControlRowState.Selected;
        }

        protected void txtQtdItem_TextChanged(object sender, EventArgs e)
        {

            cotacaoDAO obj = (cotacaoDAO)Session["cotacao" + urlSessao()];
            TextBox txtQtde = (TextBox)sender;
            txtQtde.BackColor = System.Drawing.Color.White;

            GridViewRow linha = (GridViewRow)txtQtde.NamingContainer;

            cotacao_itemDAO item = obj.item(linha.RowIndex);
            try
            {


                item.Quantidade = Decimal.Parse(txtQtde.Text);
                txtQtde.Text = item.Quantidade.ToString("N2");
                obj.atualizaitem(item);
                Session.Remove("cotacao" + urlSessao());
                Session.Add("cotacao" + urlSessao(), obj);
                EnabledButtons(gridItens, true);
            }
            catch (Exception)
            {
                txtQtde.BackColor = System.Drawing.Color.Red;

            }
            txtQtde.Focus();
            linha.RowState = DataControlRowState.Selected;

        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            User usr = (User)Session["user"];
            int dig = int.Parse(Conexao.retornaUmValor("Select count(*) from cotacao_digitacao where cotacao='" + txtCotacao.Text + "' and filial='" + usr.getFilial() + "'", null));
            if (dig > 0)
            {
                modalEncerrar.Show();
            }
            else
            {

                lblError.Text = "Impossivel Finalizar a cotação ainda não tem registros de Digitação!";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }
        protected void btnGrandeVencedor_Click(object sender, EventArgs e)
        {
            finalizarCotacao(true);
        }

        private void finalizarCotacao(bool GV)
        {
            try
            {
                cotacaoDAO obj = (cotacaoDAO)Session["cotacao" + urlSessao()];
                User usr = (User)Session["user"];
                obj.Status = "FINALIZADA";
                obj.salvar(false);
                Conexao.executarSql("exec sp_br_finaliza_cotacao " + txtCotacao.Text.Trim() + " , '" + usr.getFilial() + "'," + (GV ? "1" : "0"));
                Session.Remove("cotacao" + urlSessao());
                Session.Add("cotacao" + urlSessao(), obj);
                carregarDados();
                habilitar(false);
                visualizar(pnBtn);

            }
            catch (Exception err)
            {

                lblError.Text = "falha ao finalizar cotacao erro:" + err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }
        protected void btnMelhoresPrecos_Click(object sender, EventArgs e)
        {
            finalizarCotacao(false);
        }

        protected void gridPedido_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int index = Convert.ToInt32(e.CommandArgument);
            HyperLink meuLink = (HyperLink)gridPedido.Rows[index].Cells[0].Controls[0];
            lblPedidoEmail.Text = meuLink.Text;
            lblMensagemEmail.Text = "Tem Certeza que gostaria de Enviar o email";
            modalConfirmaEmail.Show();

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
                lblError.Text = "Email Enviado com Sucesso!";
                lblError.ForeColor = System.Drawing.Color.Blue;
            }
            catch (Exception err)
            {
                throw err;
                
            }

        }

        protected void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            try
            {

            
            if(lblPedidoEmail.Text.Equals(""))
            {
                foreach (GridViewRow item in gridPedido.Rows)
                {
                    HyperLink meuLink = (HyperLink)item.Cells[0].Controls[0];
                    String numPedido = meuLink.Text;
                    enviarEmailPedido(numPedido);
                }
            }
            else
            {
                enviarEmailPedido(lblPedidoEmail.Text);
            }
            
            modalConfirmaEmail.Hide();
            }
            catch (Exception err )
            {

                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }
        protected void btnCancela_Click(object sender, EventArgs e)
        {
            modalConfirmaEmail.Hide();
        }

        protected void imgBtnIncluirSelecionados_Click(object sender, ImageClickEventArgs e)
        {
            verificaSelecionados();
            carregarMercadorias();
            modalMercadorialista.Show();
        }

        protected void linkObs_Click(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent;
            String fornecedor = row.Cells[0].Text;
            cotacaoDAO obj = (cotacaoDAO)Session["cotacao" + urlSessao()];
            lblObsFornecedor.Text = obj.obsFornecedor(fornecedor);
            modalObsFornecedor.Show();
            
        }
        

        protected void imgBtnEnviarTodosEmails_Click(object sender, ImageClickEventArgs e)
        {
            lblPedidoEmail.Text = "";
            lblMensagemEmail.Text = "Tem Certeza Que Gostaria de Enviar Todos os E-mails?";
            modalConfirmaEmail.Show();

        }

        protected void imgBtnImpNaoCotados_Click(object sender, ImageClickEventArgs e)
        {
            RedirectNovaAba("CotacaoNaoCotadosPrint.aspx?cotacao=" + txtCotacao.Text);
        }
    }
}