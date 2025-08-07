using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;
using visualSysWeb.modulos.Cadastro.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class PromocaoJornalDetalhes : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    PromocaoJornalDAO obj = new PromocaoJornalDAO(usr);
                    status = "incluir";
                    obj.Inicio = DateTime.Now;
                    obj.Fim = DateTime.Now;
                    obj.Filial = usr.getFilial();
                    obj.status = "INICIADO";

                    Session.Remove("objPromocao" + urlSessao());
                    Session.Add("objPromocao" + urlSessao(), obj);
                    carregarDados();
                    incluir(pnBtn);
                    HabilitarCampos(true);

                    btnEncerrar.Visible = false;
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
                            int index = Funcoes.intTry(Request.Params["campoIndex"].ToString());// colocar o campo index da tabela
                            status = "visualizar";
                            PromocaoJornalDAO obj = new PromocaoJornalDAO(index, usr);
                            obj.gridInicio = 0;
                            obj.gridFim = 100;
                            Session.Remove("objPromocao" + urlSessao());
                            Session.Add("objPromocao" + urlSessao(), obj);

                            carregarDados();
                            if (obj.status.Equals("ATIVO") || obj.Fim < Funcoes.dtTry(DateTime.Now.ToString("dd/MM/yyyy")))
                            {
                                btnEncerrar.Visible = false;
                            }
                        }
                        if (status.Equals("visualizar"))
                        {
                            HabilitarCampos(false);
                        }
                        else
                        {
                            HabilitarCampos(true);
                        }

                    }
                    catch (Exception err)
                    {
                        msgShow(err.Message, true);
                    }
                }
            }
            carregabtn(pnBtn);

        }

        private void HabilitarCampos(bool v)
        {
            EnabledControls(cabecalho, v);
            EnabledControls(conteudo, v);
            EnabledButtons(gridItens, v);
            EnabledButtons(gridMercadoriasSelecionadas,v);
            
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
        protected void txtPrecoItem_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            atualizarGrid(gridItens);
            txt.Focus();
        } 
        protected void txtPrecoItemSelec_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            atualizarGrid(gridMercadoriasSelecionadas);
            txt.Focus();
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
        protected void ImgPesquisaMercadoria_Click(object sender, ImageClickEventArgs e)
        {
            carregarMercadorias(false);
        }

        private void exibeLista(string campo)
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
            TxtPesquisaLista.Text = "";
            TxtPesquisaLista.Focus(); ;
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
        protected void imgBtnIncluirSelecionados_Click(object sender, ImageClickEventArgs e)
        {
            verificaSelecionados();
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

        protected void imgBtnConfirmaExcluirItem_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
               

                PromocaoJornalDAO obj = (PromocaoJornalDAO)Session["objPromocao" + urlSessao()];
                obj.Itens.RemoveAll(p => (p.Ordem +p.Plu).ToString().Equals(lblPluExcluirItem.Text));
                modalExcluir.Hide();
                carregarGrid();
                msgShow("Item Excluido Com Sucesso", false);
            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
            }

        }

        protected void imgBntnExcluirPromocao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                PromocaoJornalDAO obj = (PromocaoJornalDAO)Session["objPromocao" + urlSessao()];
                obj.excluir();
            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
            }
        }

        protected void imgBtnCancelarExcluir_Click(object sender, ImageClickEventArgs e)
        {
            modalExcluir.Hide();
        }

        protected void imgBtnCancelaExcluirItem_Click(object sender, ImageClickEventArgs e)
        {

            modalExcluirItem.Hide();
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
        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalLista.Hide();
        }
        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {

            String itemLista = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
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
            }

            modalLista.Hide();
        }

        protected void chkSeleciona_CheckedChanged(object sender, EventArgs e)
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
        private void carregarPlu()
        {
            User usr = (User)Session["User"];
            SqlDataReader rsplu = null;
            try
            {
                rsplu = Conexao.consulta("Select top 1 mercadoria.PLU,mercadoria.descricao, mercadoria_loja.preco from mercadoria inner join mercadoria_loja on mercadoria.plu=mercadoria_loja.plu left join ean on mercadoria.plu=ean.plu where mercadoria_loja.filial='" + usr.getFilial() + "' and ( mercadoria.plu='" + txtPlu.Text + "' or ean.EAN='" + txtPlu.Text + "')", null, false);
                if (rsplu.Read())
                {
                    txtPlu.Text = rsplu["plu"].ToString();
                    txtDescricaoItem.Text = rsplu["descricao"].ToString();
                    txtPrecoItem.Text = Funcoes.decTry(rsplu["preco"].ToString()).ToString();
                    txtPrecoPromocaoItem.Text = Funcoes.decTry(rsplu["preco"].ToString()).ToString();
                    txtPrecoPromocaoItem.Focus();
                    addItensDig.DefaultButton = "ImgBtnAddItens";
                    
                }
                else
                {
                    msgShow("Produto não encontrado!!", true);


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

        protected bool validaCamposObrigatorios()
        {
            //Checagem de data.
            if (Funcoes.dtTry(txtDtFim.Text) < Funcoes.dtTry(DateTime.Now.ToString("dd/MM/yyyy")))
            {
                lblError.Text = "DATA FINAL não pode ser inferior a DATA ATUAL.";
                lblError.ForeColor = System.Drawing.Color.Red;
                return false;
            }

            if (Funcoes.dtTry(txtDtFim.Text) < Funcoes.dtTry(txtDtInicio.Text))
            {
                lblError.Text = "DATA FINAL não pode ser inferior a DATA INICIAL.";
                lblError.ForeColor = System.Drawing.Color.Red;
                return false;
            }

            if (validaCampos(conteudo))
                return true;
            else
                return false;
        }

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array
            String[] campos = { "txtNome_objPromocao",
                                    "",
                                    "",
                                    ""
                                     };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array

            if (campo.ID == null)
                return true;

            if (status.Equals("editar") && campo.ID.Equals("txtNome_objPromocao"))
                return true;

            String[] campos = { "txtCodigo",
                                    "txtStatus",
                                    "txtDescricaoItem",
                                    "txtPrecoItem"
                                     };
            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("PromocaoJornalDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            
            if (Funcoes.dtTry(txtDtInicio.Text) < DateTime.Now && Funcoes.dtTry(txtDtFim.Text) > DateTime.Now && txtStatus.Text.Equals("ATIVO"))
            {
                lblError.Text = " NÃO É POSSIVEL FAZER ALTERAÇÕES! Este jornal está ativo com promoções em andamento.";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
            else if (txtStatus.Text.Equals("ATIVO") && Funcoes.dtTry(txtDtFim.Text) < DateTime.Now)
            {
                lblError.Text = " NÃO É POSSIVEL FAZER MAIS ALTERAÇÕES. Este jornal está com seu prazo finalizado.";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                editar(pnBtn);
                HabilitarCampos(true);
            }
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("PromocaoJornal.aspx"); //colocar o endereco da tela de pesquisa
        }
        protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
        {
            int tab = TabContainer1.ActiveTabIndex;

            if (tab == 1)
            {
                atualizarGrid(gridItens);
            }
            else
            {
                atualizarGrid(gridMercadoriasSelecionadas);
            }
            carregarGrid();
            if (tab == 1)
            {
                carregarGrupos("", "", "");
                carregarLinhas();
            }


            if (status.Equals("visualizar"))
                HabilitarCampos(false);
        }
        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaCamposObrigatorios())
                {
                    PromocaoJornalDAO obj = (PromocaoJornalDAO)Session["objPromocao" + urlSessao()];
                    if (obj != null)
                    {
                        carregarDadosObj();
                        obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                        msgShow("Salvo com Sucesso", false);
                        EnabledControls(conteudo, false);
                        visualizar(pnBtn);
                        txtDescricao.Enabled = false;
                        txtDtInicio.Enabled = false;
                        txtDtFim.Enabled = false;
                    }
                    else
                    {
                        msgShow("Ocorreu um Erro e não foi possivel Salvar o Registro;", true);
                    }
                }
                else
                {
                    msgShow("Campo Obrigatorio não preenchido ou Data Inválida.", true);
                }
            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
            }
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("PromocaoJornal.aspx");//colocar endereco pagina de pesquisa
        }
        private void carregarDados()
        {
            PromocaoJornalDAO obj = (PromocaoJornalDAO)Session["objPromocao" + urlSessao()];
            if (obj != null)
            {
                txtCodigo.Text = obj.Codigo.ToString();
                txtDescricao.Text = obj.Descricao;
                txtDtInicio.Text = Funcoes.dataBr(obj.Inicio);
                txtDtFim.Text = Funcoes.dataBr(obj.Fim);
                txtStatus.Text = obj.status.ToString();
                carregarGrid();

            }
        }

        protected void carregarGrid()
        {
            PromocaoJornalDAO obj = (PromocaoJornalDAO)Session["objPromocao" + urlSessao()];
            gridItens.DataSource = obj.Itens;
            gridItens.DataBind();

            gridMercadoriasSelecionadas.DataSource = obj.Itens;
            gridMercadoriasSelecionadas.DataBind();

        }

        private void carregarDadosObj()
        {
            PromocaoJornalDAO obj = (PromocaoJornalDAO)Session["objPromocao" + urlSessao()];
            if (obj != null)
            {

                obj.Codigo = Funcoes.intTry(txtCodigo.Text);
                obj.Descricao = Funcoes.RemoverAcentos(txtDescricao.Text);
                obj.Inicio = Funcoes.dtTry(txtDtInicio.Text);
                obj.Fim = Funcoes.dtTry(txtDtFim.Text);
                obj.status = txtStatus.Text;

                if (TabContainer1.ActiveTabIndex == 0)
                    atualizarGrid(gridItens);
                else
                    atualizarGrid(gridMercadoriasSelecionadas);



                Session.Remove("objPromocao" + urlSessao());
                Session.Add("objPromocao" + urlSessao(), obj);
            }

        }
        protected void atualizarGrid(GridView  grid)
        {
            PromocaoJornalDAO obj = (PromocaoJornalDAO)Session["objPromocao" + urlSessao()];
            foreach (GridViewRow row in grid.Rows)
            {
                TextBox txt = (TextBox)row.FindControl("txtPrecoPromocao");
                PromocaoJornalItensDAO item = obj.Itens.Find(p => p.Ordem.ToString().Equals(row.Cells[0].Text) && p.Plu.Equals(row.Cells[1].Text));
                item.PrecoPromocao = Funcoes.decTry(txt.Text);
            }
            Session.Remove("objPromocao" + urlSessao());
            Session.Add("objPromocao" + urlSessao(),obj);
        }

        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                PromocaoJornalDAO obj = (PromocaoJornalDAO)Session["objPromocao" + urlSessao()];
                if (obj != null)
                {
                    obj.excluir();
                    modalExcluir.Hide();
                    msgShow("Registro Excluido com sucesso", false);
                    pesquisar(pnBtn);
                }
            }
            catch (Exception err)
            {
                msgShow("Não foi possivel Excluir o registro error:" + err.Message, true);
            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            modalExcluir.Hide();
        }




        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            modalExcluir.Show();
        }


        protected void btnOkError_Click(object sender, EventArgs e)
        {
            if (lblErroPanel.Text.Equals("Produto não encontrado!!"))
            {
                txtPlu.Focus();
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
        private void carregarLinhas()
        {
            String sqlLinha = "Select codigo=convert(varchar(3),linha.codigo_linha)+convert(varchar(3),cor_linha.codigo_cor),  linha= linha.descricao_linha+'-'+cor_linha.descricao_cor from linha " +
                                                                " inner join cor_linha on linha.codigo_linha = cor_linha.codigo_linha";
            Conexao.preencherDDL1Branco(ddlLinha, sqlLinha, "linha", "codigo", null);
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
            String sqlMercadoria = "Select distinct mercadoria.plu PLU," +
                                                   " isnull(ean.ean,'---')EAN," +
                                                   " mercadoria.Ref_fornecedor REFERENCIA, " +
                                                   " mercadoria.descricao DESCRICAO, " +
                                                   " mercadoria_loja.preco_Custo as [PRC COMPRA]," +
                                                   " mercadoria_loja.saldo_atual SALDO, " +
                                                   " mercadoria_loja.preco as [PRC VENDA] " +
                                             " from mercadoria inner join mercadoria_loja on mercadoria.plu = mercadoria_loja.plu " +
                                               " left join ean on mercadoria.plu=ean.plu  " +
                                               //" inner join W_BR_CADASTRO_DEPARTAMENTO on mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento "+
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
                    sqlMercadoria += " and substring(mercadoria.codigo_departamento,1,3)='" + ddlGrupo.SelectedValue.PadLeft(3, '0') + "' ";
                }
                if (!ddlSubGrupo.Text.Equals(""))
                {
                    sqlMercadoria += " and substring(mercadoria.codigo_departamento,1,6) ='" + ddlSubGrupo.SelectedValue + "' ";

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
            PromocaoJornalDAO obj = (PromocaoJornalDAO)Session["objPromocao" + urlSessao()];
            PromocaoJornalItensDAO item = new PromocaoJornalItensDAO(usr);
            item.Plu = linha.Cells[1].Text;
            item.Descricao = linha.Cells[4].Text;
            item.Preco = Decimal.Parse(linha.Cells[7].Text);
            item.PrecoPromocao = item.Preco;

            obj.addItens(item);

            Session.Remove("objInventario" + urlSessao());
            Session.Add("objInventario" + urlSessao(), obj);



        }
        protected void ImgBtnAddItens_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtPlu.Text.Equals("") && !txtPrecoPromocaoItem.Text.Equals(""))
                {
                    carregarDadosObj();
                    User usr = (User)Session["User"];
                    PromocaoJornalDAO obj = (PromocaoJornalDAO)Session["objPromocao" + urlSessao()];
                    PromocaoJornalItensDAO item = new PromocaoJornalItensDAO(usr);
                    item.Plu = txtPlu.Text;

                    //Incluso linha abaixo para trazer dados do produtos (ean,  REF)
                    MercadoriaDAO produto = new MercadoriaDAO(txtPlu.Text, usr);


                    item.Preco = Funcoes.decTry(txtPrecoItem.Text);
                    item.PrecoPromocao = Funcoes.decTry(txtPrecoPromocaoItem.Text);
                    item.Descricao = produto.Descricao;

                    obj.addItens(item);

                    Session.Remove("objPromocao" + urlSessao());
                    Session.Add("objPromocao" + urlSessao(), obj);
                    carregarDados();

                    txtPlu.Text = "";
                    txtDescricaoItem.Text = "";
                    txtPrecoItem.Text = "";
                    txtPrecoPromocaoItem.Text = "";


                    addItensDig.DefaultButton = "imgPlu";
                    txtPlu.Focus();
                }
                else
                {
                    txtPlu.Focus();
                    throw new Exception("Qtde invalida");
                }

            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
            }
        }
        protected void imgFornecedor_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("Fornecedor");
        }
        protected void gridItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            PromocaoJornalDAO obj = (PromocaoJornalDAO)Session["objPromocao" + urlSessao()];
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row =  gridItens.Rows[index];
            lblPluExcluirItem.Text = row.Cells[0].Text + row.Cells[1].Text;
            modalExcluirItem.Show();
            
        }
        protected void btnEncerrar_Click(object sender, EventArgs e)
        {
            modalEncerrar.Show();

        }
        protected void btnConfirmaEncerrar_Click(object sender, ImageClickEventArgs e)
        {
            salvarMovimentacao("ATIVO");
            modalEncerrar.Hide();
        }

        protected void btnCancelarEncerrar_Click(object sender, ImageClickEventArgs e)
        {
            modalEncerrar.Hide();
        }
        private void salvarMovimentacao(String strStatus)
        {
            txtStatus.Text = strStatus;
            carregarDadosObj();
            System.Threading.Thread th = new System.Threading.Thread(salvarMovimento);
            th.Start();
        }

        protected void salvarMovimento()
        {
            try
            {

                if (validaCamposObrigatorios())
                {

                    //carregarDadosObj();
                    PromocaoJornalDAO obj = (PromocaoJornalDAO)Session["objPromocao" + urlSessao()];

                    obj.salvar(status.Equals("incluir"));
                    //obj.CarregarItens();

                    //Session.Remove("objInventario" + urlSessao());
                    //Session.Add("objInventario" + urlSessao(), obj);
                    Session.Add("resultadoMovimento", "Salvo com Sucesso");

                }
                else
                {

                    Session.Add("erroMovimento", "Campo Obrigatorio não preenchido");

                }
            }
            catch (Exception err)
            {

                Session.Add("erroMovimento", err.Message);

            }
        }
    }
}