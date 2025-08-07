using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using visualSysWeb.code;
using System.Drawing;
using visualSysWeb.dao;
using System.Data.SqlClient;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class PrecificacaoProdutosDetalhes : visualSysWeb.code.PagePadrao
    {
        static String ultimaOrdem = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            if (Request.Params["NOVO"] != null)
            {
                if (!IsPostBack)
                {
                    status = "incluir";
                    precificacaoDAO obj =new precificacaoDAO(usr);
                    obj.usuario= usr.getUsuario();
                    obj.data_cadastro = DateTime.Now;
                    obj.status="0";
                    Session.Remove("Preco"+urlSessao());
                    Session.Add("Preco"+urlSessao(),obj);
                    carregarDados();
                }
            }
            else
            {
                if (Request.Params["codigo"] != null)
                {
                    String strCodigo = Request.Params["codigo"].ToString();
                    if (strCodigo != null)
                    {
                        if (!IsPostBack)
                        {
                            status = "visualizar";
                            precificacaoDAO obj = new precificacaoDAO(strCodigo, usr);
                            Session.Remove("Preco" + urlSessao());
                            Session.Add("Preco" + urlSessao(), obj);
                            carregarDados();

                        }
                    }
                }

            }

            if (!IsPostBack)
            {
                carregarGrupos("", "", "");
                carregarLinhas();
            }

            if (status.Equals("visualizar"))
            {
                HabilitarCampos(false);
            }
            else
            {
                HabilitarCampos(true);
            }
            carregabtn(pnBtn);
        }
        protected void HabilitarCampos(bool enable)
        {
            btnAplicarPorc.Visible = enable;
            EnabledControls(cabecalho, enable);
            EnabledControls(gridItens, enable);

            imgBtnIncluirSelecionados.Visible = enable;

            if (txtStatus.Text.Equals("PRECIFICADO"))
            {
                btnEncerrar.Visible = false;
            }

        }

        protected void carregarDados()
        {
            

            precificacaoDAO obj = (precificacaoDAO)Session["Preco" + urlSessao()];
            txtCodigo.Text = obj.codigo;
            txtDescricao.Text = obj.descricao;
            txtData.Text = obj.data_cadastroBr();
            txtUsuarioPrecificacao.Text = obj.usuario;
            txtPorc.Text = obj.vlr_porc.ToString("N2");
            chkTodasFilias.Checked = obj.todas_filiais;
            lblRegistros.Text = (obj.gridInicio + 1) + " ate " + (obj.arrItens.Count > 100 ? obj.gridFim : obj.arrItens.Count) + " de " + obj.arrItens.Count.ToString() + " Registro(s) Adicionado(s)";
            switch (obj.status)
            {
                case "0":
                    txtStatus.Text = "PENDENTE";
                    break;
                case "1":
                    txtStatus.Text = "PRECIFICADO";
                    break;
            }
            //obj.carregarItens(ordem);
            gridItens.DataSource = obj.precificacaoItens();
            gridItens.DataBind();

            gridMercadoriasSelecionadas.DataSource = obj.precificacaoItens();
            gridMercadoriasSelecionadas.DataBind();

            atualizaFamilias();

        }

        private void carregarDadosObj()
        {
            precificacaoDAO obj = (precificacaoDAO)Session["Preco" + urlSessao()];

            obj.codigo = txtCodigo.Text;
            obj.descricao = txtDescricao.Text;

            DateTime.TryParse(txtData.Text, out obj.data_cadastro);
            obj.usuario = txtUsuarioPrecificacao.Text;
            obj.todas_filiais = chkTodasFilias.Checked;
            Decimal.TryParse(txtPorc.Text, out obj.vlr_porc);
            switch (txtStatus.Text )
            {
                case "PENDENTE":
                    obj.status = "0";
                    break;
                case "PRECIFICADO":
                    obj.status = "1";
                    break;
            }
            
            foreach (GridViewRow item in gridItens.Rows)
            {
                if (obj.arrItens.Count > 0)
                {
                    precificacao_itensDAO objItem = (precificacao_itensDAO)obj.arrItens[item.RowIndex];
                    TextBox txtmargem = (TextBox)item.FindControl("txtMargem");
                    Decimal.TryParse(txtmargem.Text, out objItem.margem);

                    TextBox txtpreco = (TextBox)item.FindControl("txtPreco_novo");
                    Decimal.TryParse(txtpreco.Text, out objItem.preco_novo);
                    obj.arrItens[item.RowIndex] = objItem;
                }
                
            }

            Session.Remove("Preco" + urlSessao());
            Session.Add("Preco" + urlSessao(), obj);
                        

        }

        protected void gridItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Excluir"))
            {
                precificacaoDAO obj = (precificacaoDAO)Session["Preco" + urlSessao()];
                int index = Convert.ToInt32(e.CommandArgument);//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                obj.removeItem(index);
                carregarDados();
            }
        }

        protected void gridItens_Sorting(object sender, GridViewSortEventArgs e)
        {
            carregarDadosObj();
            precificacaoDAO obj = (precificacaoDAO)Session["Preco" + urlSessao()];
            obj.ordernarItens(e.SortExpression);

            Session.Remove("Preco" + urlSessao());
            Session.Add("Preco" + urlSessao(), obj);
            
            carregarDados();

            if (status.Equals("visualizar"))
            {
                EnabledControls(gridItens, false);
            }
            atualizaFamilias();
        }

        protected void sugestao()
        {

            //Decimal dblCusto = 0;
            //Decimal dblMargem = 0;
            //Decimal dblPrecoNovo = 0;
            ////TextBox txtBox1 = (TextBox)sender;

            ////GridViewRow row = gridItens.SelectedRow;
            ////dblCusto = Convert.ToDouble(row.Cells[3].Text );



            //for (int i = 0; i < gridItens.Rows.Count; i++)
            //{
            //    TextBox txtBox1 = (TextBox)gridItens.Rows[i].FindControl("txtPreco_novo");
            //    if (txtBox1.Enabled)
            //    {
            //        dblPrecoNovo = Decimal.Parse((txtBox1.Text.Equals("") ? "0" : txtBox1.Text));
            //        TextBox txtCusto = (TextBox)gridItens.Rows[i].FindControl("txtCusto");
            //        dblCusto = Decimal.Parse(gridItens.Rows[i].Cells[2].Text);
            //        Decimal NovaMargem = 0;

            //        if (dblPrecoNovo >= 0 && dblCusto > 0)
            //        {
            //            //NovaMargem = ((dblPrecoNovo  - dblCusto) / dblCusto ) * 100;
            //            NovaMargem = Funcoes.verificamargem(dblCusto, dblPrecoNovo, 0, 0);
            //            TextBox txtMargem = (TextBox)gridItens.Rows[i].FindControl("txtMargem");
            //            Label lblMargem = (Label)gridItens.Rows[i].FindControl("lblMargem");
            //            txtMargem.Text = String.Format("{0:N4}", NovaMargem);
            //            lblMargem.Text = String.Format("{0:N4}", NovaMargem);
            //        }
            //        txtBox1.Text = String.Format("{0:N2}", dblPrecoNovo);
            //    }
            //}
            atualizaFamilias();
        }

        protected void txtMargem_textChanged(object sender, EventArgs e)
        {
            //margem();
        }
        protected void txtPreco_novo_textChanged(object sender, EventArgs e)
        {
            //sugestao();   
        }
        protected void margem()
        {
            Decimal dblCusto = 0;
            Decimal dblMargem = 0;
            Decimal dblPrecoNovo = 0;
          



            for (int i = 0; i < gridItens.Rows.Count; i++)
            {
                TextBox txtBox1 = (TextBox)gridItens.Rows[i].FindControl("txtMargem");

                dblMargem = Decimal.Parse((txtBox1.Text.Equals("") ? "0" : txtBox1.Text));
                dblCusto = Decimal.Parse(gridItens.Rows[i].Cells[2].Text);

                if (dblMargem >= 0 && dblCusto > 0)
                {
                    
                    dblPrecoNovo = Funcoes.verificapreco(dblMargem, dblCusto);
                    TextBox txtPrecoNovo = (TextBox)gridItens.Rows[i].FindControl("txtPreco_novo");
                    txtPrecoNovo.Text = String.Format("{0:N2}", dblPrecoNovo);
                }
            }
            atualizaFamilias();
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("PrecificacaoProdutosDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Equals("PENDENTE"))
            {

                status = "editar";
                HabilitarCampos(true);
                carregabtn(pnBtn);
                atualizaFamilias();
            }
            else
            {
                lblError.Text = "Não é permitido Fazer alterações";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("PrecificacaoProdutos.aspx");
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            lblError.Text = "Não é permitido Excluir!";
        }

        protected void btnPag_Click(object sender, EventArgs e)
        {
            if (!status.Equals("visualizar"))
            {
                carregarDadosObj();
            }
            precificacaoDAO obj = (precificacaoDAO)Session["Preco" + urlSessao()];
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
                if (obj.gridFim > obj.arrItens.Count)
                {
                    decimal pagInicio = (decimal.Truncate(obj.arrItens.Count / 100) * 100);
                    obj.gridInicio = Convert.ToInt32(pagInicio);
                    obj.gridFim = obj.arrItens.Count;
                }
            }
            else if (btn.ID.Equals("btnPagFim"))
            {
                decimal pagInicio = (decimal.Truncate(obj.arrItens.Count / 100) * 100);
                obj.gridInicio = Convert.ToInt32(pagInicio);
                obj.gridFim = obj.arrItens.Count;
            }

            Session.Remove("Preco" + urlSessao());
            Session.Add("Preco" + urlSessao(), obj);
            carregarDados();
            if (status.Equals("visualizar"))
            {
                EnabledControls(gridItens, false);
            }


        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {

            
            if (validaCamposObrigatorios())
            {
                carregarDadosObj();
                precificacaoDAO obj = (precificacaoDAO)Session["Preco"+urlSessao()];

                obj.salvar(status.Equals("incluir"));
                Session.Remove("Preco" + urlSessao());
                Session.Add("Preco" + urlSessao(), obj);

                lblError.Text = "Salvo Com sucesso!";
                lblError.ForeColor = System.Drawing.Color.Blue;
                status = "visualizar";
                HabilitarCampos(false);
                carregabtn(pnBtn);
            }

            }
            catch (Exception err)
            {

                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }

        }
        protected bool validaCamposObrigatorios()
        {
            if (validaCampos(cabecalho) )
                return true;
            else
                return false;
        }
        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("PrecificacaoProdutos.aspx");
        }

        protected override bool campoDesabilitado(Control campo)
        {
            if (campo.ID == null)
            {
                return false;
            }
            if (campo.ID.IndexOf("txtCusto") >= 0)
            {
                return true;
            }


            String[] campos = { "txtCodigo" , 
                                   "txtUsuarioPrecificacao" , 
                                   "txtData", 
                                    "txtStatus" 
                              };

            return existeNoArray(campos, campo.ID.Trim());
        }

        protected override bool campoObrigatorio(Control campo)
        {
            return false;
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
                        TextBox txtBox1 = (TextBox)gridItens.Rows[i].FindControl("txtPreco_novo");
                        txtBox1.Enabled = false;
                        TextBox txtMargem = (TextBox)gridItens.Rows[i].FindControl("txtMargem");
                        txtMargem.Enabled = false;

                        txtBox1.BackColor = Color.FromArgb(0xDCDCDC);
                        txtMargem.BackColor = Color.FromArgb(0xDCDCDC);

                        Decimal valor = Decimal.Parse(gridItens.Rows[i].Cells[4].Text);

                        txtBox1.Text = String.Format("{0:N2}", valor);
                        dblPrecoNovo = Decimal.Parse((txtBox1.Text.Equals("") ? "0" : txtBox1.Text));
                        dblCusto = Decimal.Parse(gridItens.Rows[i].Cells[2].Text);
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

                        TextBox txtPrecoNovo = (TextBox)gridItens.Rows[i].FindControl("txtPreco_novo");
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
                        dblCusto = Decimal.Parse(gridItens.Rows[i].Cells[2].Text);

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
        protected void atualizaFamilias()
        {

            Hashtable tbFamilia = new Hashtable();
            for (int i = 0; gridItens.Rows.Count > i; i++)
            {
                String strFamilia = gridItens.Rows[i].Cells[6].Text.Trim().Replace("&nbsp;", "");
                if (!strFamilia.Equals(""))
                {
                    TextBox txtBox1 = (TextBox)gridItens.Rows[i].FindControl("txtPreco_novo");

                    try
                    {
                        tbFamilia.Add(strFamilia, txtBox1.Text);
                    }
                    catch (Exception)
                    {
                        TextBox txtBox2 = (TextBox)gridItens.Rows[i].FindControl("txtMargem");
                        Decimal dblCusto = Decimal.Parse(gridItens.Rows[i].Cells[2].Text);
                        txtBox1.Text = tbFamilia[strFamilia].ToString();
                        Decimal novoValor = Decimal.Parse(txtBox1.Text);
                        Decimal margem = Funcoes.verificamargem(dblCusto, novoValor, 0, 0);
                        txtBox2.Text = margem.ToString("N4");
                        
                        txtBox1.Enabled = false;
                        txtBox2.Enabled = false;
                        
                    }

                }

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
        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarGrupos(ddlGrupo.Text, "", "");

        }

        protected void ddlSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarGrupos(ddlGrupo.Text, ddlSubGrupo.Text, "");

        }

        protected void ddlDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarGrupos(ddlGrupo.Text, ddlSubGrupo.Text, ddlDepartamento.Text);

        }
        protected void ddlLinha_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarMercadorias(false);
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
            if (grupo.Equals("") && subGrupo.Equals("") && departamento.Equals(""))
            {
                carregarMercadorias(true);
            }
            else
            {
                carregarMercadorias(false);
            }
        }
        private void carregarLinhas()
        {
            String sqlLinha = "Select codigo=convert(varchar(3),linha.codigo_linha)+convert(varchar(3),cor_linha.codigo_cor),  linha= linha.descricao_linha+'-'+cor_linha.descricao_cor from linha " +
                                                                " inner join cor_linha on linha.codigo_linha = cor_linha.codigo_linha";
            Conexao.preencherDDL1Branco(ddlLinha, sqlLinha, "linha", "codigo", null);
        }
        protected void carregarMercadorias(bool limitar)
        {
            if (IsPostBack)
            {
                verificaSelecionados();
            }
            lblMercadoriaLista.Text = "Inclusão de Produto";
            //lblMercadoriaLista.ForeColor = Label1.ForeColor;

            if (ddlGrupo.Text.Equals("") &&
                ddlSubGrupo.Text.Equals("") &&
                ddlDepartamento.Text.Equals("") &&
                ddlLinha.Text.Equals("") &&
                txtFornecedor.Text.Equals("") &&
                txtfiltromercadoria.Text.Equals(""))
            {
                limitar = true;
            }


            User usr = (User)Session["user"];

            if (usr != null)
            {
                String sqlMercadoria = "Select distinct mercadoria.plu PLU," +
                                                       " isnull(ean.ean,'---')EAN," +
                                                       " mercadoria.Ref_fornecedor REFERENCIA, " +
                                                       " mercadoria.descricao DESCRICAO, " +
                                                       " mercadoria_loja.preco_Custo as [PRC CUSTO]," +
                                                       " mercadoria_loja.preco as [PRC VENDA] " +
                                                 " from mercadoria inner join mercadoria_loja on mercadoria.plu = mercadoria_loja.plu " +
                                                   " left join ean on mercadoria.plu=ean.plu  " +
                                                   " inner join W_BR_CADASTRO_DEPARTAMENTO on mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento " +
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

                gridMercadoria1.DataSource = Conexao.GetTable(sqlMercadoria + " order by mercadoria.descricao", usr, limitar);
                gridMercadoria1.DataBind();


            }

        }
        private void verificaSelecionados()
        {
            carregarDadosObj();
            foreach (GridViewRow item in gridMercadoria1.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                if (chk.Checked)
                {
                    incluirMercadoria(chk);
                    chk.Checked = false;
                }
            }
            
            carregarDados();
        }
        private void incluirMercadoria(CheckBox ck1)
        {
            GridViewRow linha = (GridViewRow)ck1.NamingContainer;


            User usr = (User)Session["User"];
            precificacaoDAO obj = (precificacaoDAO)Session["Preco" + urlSessao()];
            precificacao_itensDAO item = new precificacao_itensDAO(usr);
            item.plu = linha.Cells[1].Text;
            Decimal.TryParse(linha.Cells[5].Text,out item.custo);
            Decimal.TryParse(linha.Cells[6].Text, out item.preco_anterior);
            if (obj.vlr_porc > 0)
            {
                Decimal vlr = item.preco_anterior + ((item.preco_anterior * obj.vlr_porc) / 100);
                item.preco_novo = vlr;
            }
            else
            {
                Decimal.TryParse(linha.Cells[6].Text, out item.preco_novo);
            }
            item.margem = Funcoes.verificamargem(item.custo, item.preco_novo, 0, 0);
            
            obj.addItem(item);

            Session.Remove("Preco" + urlSessao());
            Session.Add("Preco" + urlSessao(), obj);
            

        }
        protected void imgFornecedor_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("Fornecedor");
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
            }
            User usr = (User)Session["User"];
            GridLista.DataSource = Conexao.GetTable(sqlLista, null, true);
            GridLista.DataBind();

            modalLista.Show();
            TxtPesquisaLista.Focus();
        }
        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalLista.Hide();
        }
        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {

            String itemLista = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            switch (itemLista)
            {
               
                case "Fornecedor":
                    txtFornecedor.Text = ListaSelecionada(1);
                    carregarMercadorias(false);
                    break;
            }

            modalLista.Hide();
        }
        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            String itemLista = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            exibeLista(itemLista);
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
        protected void ImgPesquisaMercadoria_Click(object sender, ImageClickEventArgs e)
        {
            carregarMercadorias(false);
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
        protected void imgLimpar_Click(object sender, ImageClickEventArgs e)
        {
            limparSelecaoMercadoria();
            carregarMercadorias(true);
        }
        protected void limparSelecaoMercadoria()
        {
            ddlGrupo.Text = "";
            ddlSubGrupo.Text = "";
            ddlDepartamento.Text = "";
            ddlLinha.Text = "";
            txtFornecedor.Text = "";
            txtfiltromercadoria.Text = "";
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
        protected void GridMercadoriaSelecionado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument) + 1;
            ArrayList selecionados = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            selecionados.RemoveAt(index);
            Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), selecionados);


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

        protected void imgBtnIncluirSelecionados_Click(object sender, ImageClickEventArgs e)
        {
            verificaSelecionados();
        }

        protected void btnEncerrar_Click(object sender, EventArgs e)
        {
            modalEncerrar.Show();

        }
        protected void btnAplicarPorc_Click(object sender, EventArgs e)
        {

            if (!txtPorc.Text.Equals(""))
            {
                carregarDadosObj();

                precificacaoDAO obj = (precificacaoDAO)Session["Preco" + urlSessao()];
                Decimal vPorc = 0;
                Decimal.TryParse(txtPorc.Text, out vPorc);
                obj.aplicaPorc(vPorc);
                Session.Remove("Preco" + urlSessao());
                Session.Add("Preco" + urlSessao(), obj);
                carregarDados();
            }


        }

        protected void btnConfirmaEncerrar_Click(object sender, ImageClickEventArgs e)
        {
            Session.Remove("familiasAtualiadas" + urlSessao());
            
            try
            {
                carregarDadosObj();
                precificacaoDAO obj = (precificacaoDAO)Session["Preco" + urlSessao()];

                obj.status = "1";
                foreach (precificacao_itensDAO item in obj.arrItens)
                {
                    Update(item.plu, item.custo, item.preco_novo, item.margem, item.Codigo_Familia, true);
                }

                obj.salvar(status.Equals("incluir"));

                Session.Remove("Preco" + urlSessao());
                Session.Add("Preco" + urlSessao(), obj);
                carregarDados();

                lblError.Text = "Salvo Com sucesso!";
                lblError.ForeColor = System.Drawing.Color.Blue;
                status = "visualizar";
                HabilitarCampos(false);
                carregabtn(pnBtn);
            }
            catch (Exception err)
            {

                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }

            modalEncerrar.Hide();
        }


        protected void btnCancelarEncerrar_Click(object sender, ImageClickEventArgs e)
        {
            modalEncerrar.Hide();
        }


        private void Update(String plu, Decimal precocusto, Decimal precovenda, Decimal margem, String familia, bool atualizaPreco)
        {
            User usr = (User)Session["User"];
            Decimal precoVendaAnterior = 0;
            try
            {
                precoVendaAnterior = Funcoes.decTry(Conexao.retornaUmValor("SELECT L.Preco FROM Mercadoria_Loja L WHERE L.Filial = '" + usr.getFilial() + "' AND L.PLU = '" + plu + "'",usr));
            }
            catch
            {

            }
            try
            {

                String sql = "";




                if (familia.Trim().Length > 0 && isnumeroint(familia) && int.Parse(familia) > 0)
                {
                    bool familiaUpdate = false;
                    try
                    {

                        Hashtable FamiliasAtualizadas = (Hashtable)Session["familiasAtualiadas"+urlSessao()];
                        if (FamiliasAtualizadas == null)
                        {
                            FamiliasAtualizadas = new Hashtable();
                        }
                        FamiliasAtualizadas.Add(familia.Trim(), "atualizada");
                        Session.Remove("familiasAtualiadas" + urlSessao());
                        Session.Add("familiasAtualiadas" + urlSessao(), FamiliasAtualizadas);
                        familiaUpdate = true;
                    }
                    catch (Exception)
                    {
                        familiaUpdate = false;
                    }

                    if (familiaUpdate)
                    {


                        sql = "update Mercadoria set " +
                            " Margem =" + margem.ToString().Replace(",", ".");

                        if (atualizaPreco)
                        {
                            sql += ", Preco =" + precovenda.ToString().Replace(",", ".") +
                            ", imprime_etiqueta = 0" +
                            ", Etiqueta = 1" +
                            ", Estado_Mercadoria = 1" +
                            ", terceiro_preco = case when isnull(terceiro_preco,0) > 0 then " +
                            "   (preco_custo * (1 + (margem_terceiro_preco / 100)))" +
                            "   else " +
                            "       0 " +
                            "   end";
                        }

                        sql += ", Data_Alteracao = '" + DateTime.Today.ToString("yyyy-MM-dd") + "'" +
                                    "  where  plu='" + plu + "'";
                        Conexao.executarSql(sql);


                        if (atualizaPreco)
                        {
                            String sqlTb = "update Preco_Mercadoria " +
                                "set preco="+ Funcoes.decimalPonto(precovenda.ToString())+", preco_promocao =case when desconto=0 then " + Funcoes.decimalPonto(precovenda.ToString()) + "  else " + Funcoes.decimalPonto(precovenda.ToString()) + "- ((" + Funcoes.decimalPonto(precovenda.ToString()) + " * desconto)/100)  end " +
                                " where plu ='" + plu + "' and filial ='"+usr.getFilial()+"'";

                            Conexao.executarSql(sqlTb);

                        }


                        //Atualiza preço familia
                        String SqlUpFamilia = "update familia set preco=" + precovenda.ToString().Replace(",", ".") + " , imprime_etiqueta=1 where codigo_familia='" + familia.Trim() + "'";
                        Conexao.executarSql(SqlUpFamilia);


                        //Atualiza preço dos itens da familia
                        String sqlFamilia = "select PLU,isnull(preco_custo,0)preco_custo from mercadoria where Codigo_Familia = '" + familia + "'and isnull(inativo,0)=0";
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
                                    sqlExecutar.Add("update mercadoria set margem =" + fmMargem.ToString().Replace(",", ".") + ", preco=" + precovenda.ToString().Replace(",", ".") + ",Estado_Mercadoria = 1" +
                                        " , terceiro_preco = case when isnull(terceiro_preco, 0) > 0 then " +
                                        "   (preco_custo * (1 + (margem_terceiro_preco / 100)))" +
                                        "   else " +
                                        "       0 " +
                                        "   end"+
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
                                            " values ('" + rsFamilia["plu"].ToString() + "','Precif Produtos N " + txtCodigo.Text + " FAMILIA:" + familia.Trim() + "',GETDATE(),'" + usr.getNome() + "'," + vPrecoAnt.ToString().Replace(",", ".") + "," + precovenda.ToString().Replace(",", ".") + ",'" + (chkTodasFilias.Checked ? "TODAS" : usr.getFilial()) + "')");
                                        //Conexao.executarSql("insert into log_preco (plu,descricao,data,usuario,preco_old,preco_new,filial) " +
                                        //    " values ('" + rsFamilia["plu"].ToString() + "','Precif Nota FAMILIA:" + familia.Trim() + "',GETDATE(),'" + usr.getNome() + "'," + vPrecoAnt.ToString().Replace(",", ".") + "," + precovenda.ToString().Replace(",", ".") + ",'" + (chkTodasFilias.Checked ? "TODAS" : usr.getFilial()) + "')");
                                    }

                                }
                                sql += ", Data_Alteracao = '" + DateTime.Today.ToString("yyyy-MM-dd") + "'" +
                                         "  where  PLU = '" + rsFamilia["plu"].ToString() + "' and filial='" + usr.getFilial() + "'";

                               


                                sqlExecutar.Add(sql);
                                //Conexao.executarSql(sql);

                                if (atualizaPreco)
                                {
                                    String sqlTb = "update Preco_Mercadoria " +
                                        "set preco="+ Funcoes.decimalPonto(precovenda.ToString()) + ", preco_promocao =case when desconto=0 then " + Funcoes.decimalPonto(precovenda.ToString()) + "  else " + Funcoes.decimalPonto(precovenda.ToString()) + "- ((" + Funcoes.decimalPonto(precovenda.ToString()) + " * desconto)/100) end " +
                                        " where plu ='" + rsFamilia["plu"].ToString() + "' and filial='" + usr.getFilial() + "'  ";

                                    sqlExecutar.Add(sqlTb);

                                }

                                if (chkTodasFilias.Checked)
                                {
                                    SqlDataReader rsFiliais = null;
                                    try
                                    {

                                    
                                    rsFiliais =Conexao.consulta("Select preco_custo,filial from mercadoria_loja where plu='" + rsFamilia["plu"].ToString() + "' and filial <>'" + usr.getFilial() + "'", null, false);
                                    while (rsFiliais.Read())
                                    {
                                        Decimal pCusto = rsFiliais["preco_custo"].ToString().Equals("") ? 0 : Decimal.Parse(rsFiliais["preco_custo"].ToString());
                                        Decimal vMargem = Funcoes.verificamargem(pCusto, precovenda, 0, 0);

                                        sqlExecutar.Add("update mercadoria_loja set Preco =" + precovenda.ToString().Replace(",", ".") + ", margem=" + vMargem.ToString().Replace(",", ".") + " where plu='" + rsFamilia["plu"].ToString() + "' and Filial='" + rsFiliais["filial"].ToString() + "'");
                                            //Conexao.executarSql("update mercadoria_loja set Preco =" + precovenda.ToString().Replace(",", ".") +", margem=" + vMargem.ToString().Replace(",", ".") + " where plu='" + rsFamilia["plu"].ToString() + "' and Filial='" + rsFiliais["filial"].ToString() + "'");
                                            if (atualizaPreco)
                                            {

                                                String sqlTb = "update Preco_Mercadoria " +
                                                    "set preco= "+ Funcoes.decimalPonto(precovenda.ToString()) + ", preco_promocao = case when desconto=0 then " + Funcoes.decimalPonto(precovenda.ToString() )+ "  else " + Funcoes.decimalPonto(precovenda.ToString()) + "- ((" + Funcoes.decimalPonto(precovenda.ToString()) + " * desconto)/100) end" +
                                                    " where plu ='" + rsFamilia["plu"].ToString() + "' and filial='" + rsFiliais["filial"].ToString() + "' and desconto>0";

                                                sqlExecutar.Add(sqlTb);

                                            }
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
                    sql = "update  Mercadoria set " +
                                   " Margem =" + margem.ToString().Replace(",", ".");
                    if (atualizaPreco)
                    {
                        sql += ", Preco =" + precovenda.ToString().Replace(",", ".") +
                            ", Etiqueta = 1" +
                            ", imprime_etiqueta = 1" +
                            ", Estado_Mercadoria = 1"+
                            ",  terceiro_preco = case when isnull(terceiro_preco,0) > 0 then " +
                            "   (preco_custo * (1 + (margem_terceiro_preco / 100)))" +
                            "   else " +
                            "       0 " +
                            "   end";
                    }
                    sql += ", Data_Alteracao = '" + DateTime.Today.ToString("yyyy-MM-dd") + "'" +
                            "  where PLU = '" + plu + "'";

                    Conexao.executarSql(sql);

                    sql = "update  Mercadoria_loja set " +
                                   " Margem =" + margem.ToString().Replace(",", ".");

                    if (atualizaPreco)
                    {
                        sql += ", Preco =" + precovenda.ToString().Replace(",", ".");
                        String strVericaAlteracao = "select isnull(preco,0) from mercadoria_loja where PLU= '" + plu + "' and Filial='" + usr.getFilial() + "'";
                        Decimal vPrecoAnt = Decimal.Parse(Conexao.retornaUmValor(strVericaAlteracao, null));
                        if ((precovenda - vPrecoAnt) != 0)
                        {
                            Conexao.executarSql("insert into log_preco (plu,descricao,data,usuario,preco_old,preco_new,filial) " +
                                " values ('" + plu + "','Precificação Produtos N "+txtCodigo.Text+"',GETDATE(),'" + usr.getNome() + "'," + vPrecoAnt.ToString().Replace(",", ".") + "," + precovenda.ToString().Replace(",", ".") + ",'" + (chkTodasFilias.Checked ? "TODAS" : usr.getFilial()) + "')");
                        }

                    }
                    sql += ", Data_Alteracao = '" + DateTime.Today.ToString("yyyy-MM-dd") + "'" +
                         "  where PLU = '" + plu + "' and filial='" + usr.getFilial() + "'";

                    Conexao.executarSql(sql);

                    if (atualizaPreco)
                    {
                        String sqlTb = "update Preco_Mercadoria " +
                            "set preco = "+ Funcoes.decimalPonto(precovenda.ToString()) + ", preco_promocao =case when desconto=0 then " + Funcoes.decimalPonto(precovenda.ToString()) + "  else " + Funcoes.decimalPonto(precovenda.ToString()) + "- ((" + Funcoes.decimalPonto(precovenda.ToString()) + " * desconto)/100) end " +
                            " where plu ='" + plu + "' and filial='" + usr.getFilial() + "' and desconto>0";

                        Conexao.executarSql(sqlTb);

                    }


                    if (chkTodasFilias.Checked)
                    {
                        SqlDataReader rsFiliais = null;
                        try
                        {

                        
                        rsFiliais =Conexao.consulta("Select preco_custo,filial from mercadoria_loja where plu='" + plu + "' and filial <>'" + usr.getFilial() + "'", null, false);
                        while (rsFiliais.Read())
                        {
                            Decimal pCusto = rsFiliais["preco_custo"].ToString().Equals("") ? 0 : Decimal.Parse(rsFiliais["preco_custo"].ToString());
                            Decimal vMargem = Funcoes.verificamargem(pCusto, precovenda, 0, 0);

                            

                            Conexao.executarSql("update mercadoria_loja set  Preco =" + precovenda.ToString().Replace(",", ".") + ", margem=" + vMargem.ToString("N4").Replace(",", ".") + " where plu='" + plu + "' and Filial='" + rsFiliais["filial"].ToString() + "'");
                          
                                String sqlTb = "update Preco_Mercadoria " +
                                    "set preco="+ Funcoes.decimalPonto(precovenda.ToString()) + ", preco_promocao = case when desconto=0 then "+Funcoes.decimalPonto(precovenda.ToString())+"  else " + Funcoes.decimalPonto(precovenda.ToString()) + "- ((" + Funcoes.decimalPonto(precovenda.ToString()) + " * desconto)/100) end " +
                                    " where plu ='" + plu + "' and filial= '" + rsFiliais["filial"].ToString() + "' ";

                                Conexao.executarSql(sqlTb);

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
                        Decimal decRelNovoPreco = 0;
                        string strRelPLU = "";

                        Decimal decRelTerceiroPreco = 0;
                        Decimal decRelPrecoAtacado = 0;

                        Decimal decRelPercentualAlterado = 0;

                        rsVinculados = Conexao.consulta("sp_cons_PLUsVinculado '" + plu + "'", null, false);

                        while (rsVinculados.Read())
                        {
                            Decimal.TryParse(rsVinculados["preco_custo"].ToString(), out decRelCusto);
                            Decimal.TryParse(rsVinculados["margem"].ToString(), out decRelMargem);
                            Decimal.TryParse(rsVinculados["preco"].ToString(), out decRelPreco);
                            Decimal.TryParse(rsVinculados["fator"].ToString(), out decRelFator);
                            Decimal.TryParse(rsVinculados["QtdeBase"].ToString(), out decQtdeBase);
                            strRelPLU = rsVinculados["PLU"].ToString();

                            Decimal.TryParse(rsVinculados["preco_atacado"].ToString(), out decRelPrecoAtacado);
                            Decimal.TryParse(rsVinculados["terceiro_preco"].ToString(), out decRelTerceiroPreco);

                            decRelNovoPreco = (precovenda / (decQtdeBase <= 1 ? 1 :decQtdeBase)) * (decRelFator <= 0 ? 1 : decRelFator); //Define qual o preco do novo custo
                            decRelNovaMargem = Funcoes.verificamargem(decRelCusto, decRelNovoPreco, 0, 0);

                            if (precoVendaAnterior > 0)
                            {
                                decRelPercentualAlterado = ((precovenda - precoVendaAnterior ) / precoVendaAnterior);
                            }

                            sql = "update Mercadoria set Margem =" + decRelNovaMargem.ToString().Replace(",", ".");
                            sql += ", Preco =" + decRelNovoPreco.ToString().Replace(",", ".") +
                                ", Etiqueta = 1" +
                                ", imprime_etiqueta = 1" +
                                ", Estado_Mercadoria = 1";
                                //", preco_atacado = case when isnull(preco_atacado,0) > 0 then " +
                                //"       (preco_custo * (1 + (margem_atacado / 100)))" +
                                //"   else " +
                                //"       0 " +
                                //"   end" +
                                //",  terceiro_preco = case when isnull(terceiro_preco,0) > 0 then " +
                                //"   (preco_custo * (1 + (margem_terceiro_preco / 100)))" +
                                //"   else " +
                                //"       0 " +
                                //"   end";
                            sql += ", Data_Alteracao = '" + DateTime.Today.ToString("yyyy-MM-dd") + "'" +
                                    "  where PLU = '" + strRelPLU + "'";

                            Conexao.executarSql(sql, connRel, tranRel);


                            sql = "update Mercadoria_loja set Margem =" + decRelNovaMargem.ToString().Replace(",", ".");
                            sql += ", Preco =" + decRelNovoPreco.ToString().Replace(",", ".");

                            if (chkTodasFilias.Checked)
                            {
                                sql += " WHERE PLU = '" + strRelPLU + "'";
                            }
                            else
                            {
                                sql += " WHERE Mercadoria_Loja.Filial = '" + usr.getFilial() + "' AND Mercadoria_Loja.PLU = '" + strRelPLU + "'";
                            }

                            Conexao.executarSql(sql, connRel, tranRel);


                            //Checa se existe preço atacado e faz alteração
                            if (decRelPrecoAtacado > 0 && decRelPercentualAlterado != 0)
                            {
                                decRelPrecoAtacado = Decimal.Parse((decRelPrecoAtacado * (1 + decRelPercentualAlterado)).ToString("N2"));
                                decRelNovaMargem = Funcoes.verificamargem(decRelCusto, decRelPrecoAtacado, 0, 0);

                                sql = "update Mercadoria set margem_atacado =" + decRelNovaMargem.ToString().Replace(",", ".");
                                sql += ", Preco_Atacado =" + decRelPrecoAtacado.ToString().Replace(",", ".");

                                if (chkTodasFilias.Checked)
                                {
                                    sql += " WHERE PLU = '" + strRelPLU + "'";
                                }
                                else
                                {
                                    sql += " WHERE Mercadoria_Loja.Filial = '" + usr.getFilial() + "' AND Mercadoria_Loja.PLU = '" + strRelPLU + "'";
                                }

                                Conexao.executarSql(sql, connRel, tranRel);


                            }
                            //checa se existe terceiro preço e faz alteração
                            if (decRelTerceiroPreco > 0 && decRelPercentualAlterado != 0)
                            {
                                decRelTerceiroPreco = Decimal.Parse((decRelTerceiroPreco * (1 + decRelPercentualAlterado)).ToString("N2"));
                                decRelNovaMargem = Funcoes.verificamargem(decRelCusto, decRelTerceiroPreco, 0, 0);

                                sql = "update Mercadoria set margem_terceiro_preco =" + decRelNovaMargem.ToString().Replace(",", ".");
                                sql += ", Terceiro_preco =" + decRelPrecoAtacado.ToString().Replace(",", ".");

                                if (chkTodasFilias.Checked)
                                {
                                    sql += " WHERE PLU = '" + strRelPLU + "'";
                                }
                                else
                                {
                                    sql += " WHERE Mercadoria_Loja.Filial = '" + usr.getFilial() + "' AND Mercadoria_Loja.PLU = '" + strRelPLU + "'";
                                }

                                Conexao.executarSql(sql, connRel, tranRel);

                            }

                            //Grava informação na tabela LOG_PRECO

                            if ((decRelPreco - decRelNovoPreco) != 0)
                            {
                                Conexao.executarSql("insert into log_preco (plu,descricao,data,usuario,preco_old,preco_new,filial) " +
                                    " values ('" + strRelPLU + "','Precificação Nota PLU VINCULADO. PLU Origem: " + plu  + "',GETDATE(),'" + usr.getNome() + "'," + decRelPreco.ToString().Replace(",", ".") + "," + decRelNovoPreco.ToString().Replace(",", ".") + ",'" + (chkTodasFilias.Checked ? "TODAS" : usr.getFilial()) + "')");
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

    }
}