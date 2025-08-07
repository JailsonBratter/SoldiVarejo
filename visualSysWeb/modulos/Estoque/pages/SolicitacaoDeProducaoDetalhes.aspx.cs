using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;
using visualSysWeb.modulos.Estoque.dao;

namespace visualSysWeb.modulos.Estoque.pages
{
    public partial class SolicitacaoDeProducaoDetalhes : visualSysWeb.code.PagePadrao
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
                        solicitacao_producaoDAO obj = new solicitacao_producaoDAO(usr, 0);
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
                            solicitacao_producaoDAO obj = new solicitacao_producaoDAO(cod, 0, usr);
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


        private void carregarDados()
        {
            solicitacao_producaoDAO obj = (solicitacao_producaoDAO)Session["objSolicita" + urlSessao()];
            txtCodigo.Text = obj.codigo;
            txtDescricao.Text = obj.descricao;
            txtDataCadastro.Text = obj.data_cadastroBr();
            txtUsuarioCadastro.Text = obj.usuario_cadastro;
            txtstatus.Text = obj.status;
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


        }

        private void carregarDadosObj()
        {
            carregarDadosObj(true);
        }

        private void carregarDadosObj(bool atualizaGrid)
        {
            solicitacao_producaoDAO obj = (solicitacao_producaoDAO)Session["objSolicita" + urlSessao()];
            obj.codigo = txtCodigo.Text;
            obj.descricao = txtDescricao.Text;
            DateTime.TryParse(txtDataCadastro.Text, out obj.data_cadastro);
            obj.usuario_cadastro = txtUsuarioCadastro.Text;
            obj.status = txtstatus.Text;
            Session.Remove("objSolicita" + urlSessao());
            Session.Add("objSolicita" + urlSessao(), obj);

            if (atualizaGrid)
            {
                if (obj.arrItens.Count > 0)
                {
                    GridView grid = (TabContainer1.ActiveTabIndex == 0 ? gridItens : gridMercadoriasSelecionadas);
                    atualizaItens(grid);

                }
            }

        }

        private void atualizaItens(GridView grid)
        {

            solicitacao_producaoDAO obj = (solicitacao_producaoDAO)Session["objSolicita" + urlSessao()];
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                TextBox txtGridQtde = (TextBox)grid.Rows[i].FindControl("txtGridQtde");
                if (!txtGridQtde.Text.Equals("------"))
                {
                    solicitacao_producao_itensDAO item = obj.item(grid.Rows[i].Cells[1].Text);
                    if (item != null)
                        item.qtde = Funcoes.decTry(txtGridQtde.Text);


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
                    incluirItens(txtPlu.Text, txtQtde.Text);


                }
            }
            catch (Exception err)
            {
                showMessage(err.Message, true);

            }




        }
        protected void btnPag_Click(object sender, EventArgs e)
        {

            if (!status.Equals("visualizar"))
            {
                carregarDadosObj();
            }
            solicitacao_producaoDAO obj = (solicitacao_producaoDAO)Session["objSolicita" + urlSessao()];
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

        private String sqlDiaSemana()
        {

            int diaSemana = (int)DateTime.Now.DayOfWeek;
            String strDia = " and (select m2.prog_";

            switch (diaSemana)
            {
                case 0:
                    strDia += "Dom";
                    break;
                case 1:
                    strDia += "Seg";
                    break;
                case 2:
                    strDia += "Ter";
                    break;
                case 3:
                    strDia += "Qua";
                    break;
                case 4:
                    strDia += "Qui";
                    break;
                case 5:
                    strDia += "Sex";
                    break;
                case 6:
                    strDia += "Sab";
                    break;

            }
            return strDia + " from mercadoria as m2 where m2.plu = mercadoria.plu_receita ) = 1 ";

        }
        protected void carregarPlu()
        {
            User usr = (User)Session["User"];
            SqlDataReader rsplu = null;
            try
            {



                rsplu = Conexao.consulta("Select top 1 mercadoria.PLU,mercadoria.descricao, und=mercadoria.und_producao,isnull(mercadoria_loja.saldo_atual,0)saldo_atual,isnull(mercadoria_loja.preco_custo,0)custo " +
                    "from mercadoria inner join mercadoria_loja on mercadoria.plu=mercadoria_loja.plu left join ean on mercadoria.plu=ean.plu " +
                    "where mercadoria_loja.filial='" + usr.getFilial() + "' and ( mercadoria.plu='" + txtPlu.Text + "' or ean.EAN='" + txtPlu.Text + "')" +
                    sqlDiaSemana(), null, false);
                if (rsplu.Read())
                {
                    txtPlu.Text = rsplu["plu"].ToString();
                    txtDescricaoItem.Text = rsplu["descricao"].ToString();
                    txtUnidadeItem.Text = rsplu["und"].ToString();
                    txtQtde.Focus();
                    addItensDig.DefaultButton = "ImgBtnAddItens";
                }
                else
                {
                    showMessage("Produto não encontrado!!", true);
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
                showMessage(err.Message, true);
            }



        }

        private void incluirItens(String strPlus, String qtde)
        {
            User usr = (User)Session["User"];
            SqlDataReader rs = null;
            solicitacao_producaoDAO obj = (solicitacao_producaoDAO)Session["objSolicita" + urlSessao()];
            try
            {
                rs = Conexao.consulta("Select  mercadoria.plu PLU, isnull(ean.ean, '---')EAN, mercadoria.Ref_fornecedor REFERENCIA, mercadoria.descricao DESCRICAO ,UND = MERCADORIA.UND_PRODUCAO " +
                                          " from mercadoria left join ean on ean.plu = mercadoria.plu " +
                                          " where mercadoria.plu in (" + strPlus + ") "
                                          , null, false);
                while (rs.Read())
                {
                    if (!obj.itemIncluido(rs["PLU"].ToString()))
                    {

                        solicitacao_producao_itensDAO objItem = new solicitacao_producao_itensDAO(usr, 0);
                        objItem.codigo = obj.codigo;
                        objItem.filial = obj.filial;

                        objItem.plu = rs["PLU"].ToString();
                        objItem.ean = rs["EAN"].ToString();
                        objItem.descricao = rs["DESCRICAO"].ToString();
                        objItem.und = rs["UND"].ToString();
                        objItem.qtde = 1;

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

            //calculachk();
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
                    sqlLista = sqlLista + "( mercadoria.plu like '%" + TxtPesquisaLista.Text + "%' or descricao like '%" + TxtPesquisaLista.Text + "%'  or mercadoria.Ref_fornecedor like '%" + TxtPesquisaLista.Text + "%' or (ean like '%" + TxtPesquisaLista.Text + "%') )" +
                        sqlDiaSemana() +
                        " ORDER BY DESCRICAO";

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

            solicitacao_producaoDAO obj = (solicitacao_producaoDAO)Session["objSolicita" + urlSessao()];
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
                txtfiltromercadoria.Text.Equals("") &&
                txtcodListaPadrao.Text.Equals(""))
            {
                limitar = true;
            }
            int diaSemana = (int)DateTime.Now.DayOfWeek;
            String strDia = " and (select m2.prog_";

            switch (diaSemana)
            {
                case 0:
                    strDia += "Dom";
                    lblProgramação.Text = "Domingo ";
                    break;
                case 1:
                    strDia += "Seg";
                    lblProgramação.Text = "Segunda-Feira ";
                    break;
                case 2:
                    strDia += "Ter";
                    lblProgramação.Text = "Terça-Feira ";
                    break;
                case 3:
                    strDia += "Qua";
                    lblProgramação.Text = "Quarta-Feira ";
                    break;
                case 4:
                    strDia += "Qui";
                    lblProgramação.Text = "Quinta-Feira ";
                    break;
                case 5:
                    strDia += "Sex";
                    lblProgramação.Text = "Sexta-Feira ";
                    break;
                case 6:
                    strDia += "Sab";
                    lblProgramação.Text = "Sabado ";
                    break;

            }
            lblProgramação.Text += ": " + DateTime.Now.ToString("dd/MM/yyyy");

            User usr = (User)Session["user"];
            String sqlMercadoria = "  Select * into #cadDepart from W_BR_CADASTRO_DEPARTAMENTO;  " +
                                             "Select " + (limitar ? "Top 100" : "") + " mercadoria.plu PLU," +
                                                   " isnull(ean.ean,'---')EAN," +
                                                   " mercadoria.Ref_fornecedor REFERENCIA, " +
                                                   " mercadoria.descricao DESCRICAO, " +

                                                   " mercadoria_loja.saldo_atual SALDO, " +
                                                   " mercadoria_loja.preco as [PRC VENDA] " +
                                             " from mercadoria inner join mercadoria_loja on mercadoria.plu = mercadoria_loja.plu " +
                                               " left join ean on mercadoria.plu=ean.plu  " +
                                               " inner join #cadDepart on mercadoria.Codigo_departamento = #cadDepart.codigo_departamento " +
                                               " left join Fornecedor_Mercadoria on mercadoria.PLU = Fornecedor_Mercadoria.PLU  AND Mercadoria_Loja.Filial = Fornecedor_Mercadoria.Filial " +
                                    " where (mercadoria_loja.filial='" + usr.getFilial() + "') " +
                                    strDia + " from mercadoria as m2 where m2.plu = mercadoria.plu_receita ) =1 ";
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


            if (txtcodListaPadrao.Text.Length > 0)
            {
                sqlMercadoria += " and mercadoria.plu in (select plu from lista_padrao_itens where id_lista =" + txtcodListaPadrao.Text + ") ";
            }

            gridMercadoria1.DataSource = Conexao.GetTable(sqlMercadoria + " order by mercadoria.descricao", null, false);
            gridMercadoria1.DataBind();




        }
        protected void btnConfirmaEncerrar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                solicitacao_producaoDAO obj = (solicitacao_producaoDAO)Session["objSolicita" + urlSessao()];

                User usr = (User)Session["User"];
                txtstatus.Text = "ENCERRADO";
                obj.status = "ENCERRADO";
                obj.salvar(false);

                modalEncerrar.Hide();
            }
            catch (Exception err)
            {
                showMessage(err.Message, true);
            }
        }



        protected void btnCancelarEncerrar_Click(object sender, ImageClickEventArgs e)
        {
            modalEncerrar.Hide();
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
                showMessage(err.Message, true);

            }
        }

        private void ExcluirItem(String plu)
        {
            try
            {


                solicitacao_producaoDAO obj = (solicitacao_producaoDAO)Session["objSolicita" + urlSessao()];
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
                showMessage(err.Message, true);

            }
        }

        protected void HabilitarCampos(bool enable)
        {
            EnabledControls(conteudo, enable);
            EnabledControls(cabecalho, enable);


            if (status.Equals("visualizar"))
            {
                btnEncerrar.Visible = !txtstatus.Text.Equals("ENCERRADO");
            }
            else
            {
                btnEncerrar.Visible = false;
            }



        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("SolicitacaoDeProducaoDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {


            if (txtstatus.Text.Equals("ENCERRADO"))
            {
                showMessage("Não é Permitido fazer Alterações!", true);

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
            Response.Redirect("SolicitacaoDeProducao.aspx");
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            showMessage("Não é possivel Excluir", true);

        }



        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                validaCamposObrigatorios();
                carregarDadosObj();
                solicitacao_producaoDAO obj = (solicitacao_producaoDAO)Session["objSolicita" + urlSessao()];
                obj.salvar(status.Equals("incluir"));
                status = "visualizar";

                showMessage("Salvo Com Sucesso!!", false);


                Session.Remove("objSolicita" + urlSessao());
                Session.Add("objSolicita" + urlSessao(), obj);

                carregarDados();
                carregabtn(pnBtn);
                HabilitarCampos(false);

            }
            catch (Exception err)
            {
                showMessage(err.Message, true);

            }
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("SolicitacaoDeProducao.aspx");
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

        }

        private void encerrarSolicitacao()
        {
            try
            {


                solicitacao_producaoDAO obj = (solicitacao_producaoDAO)Session["objSolicita" + urlSessao()];
                obj.status = "ENCERRADO";
                obj.salvar(false);

            }
            catch (Exception err)
            {
                showMessage(err.Message, true);

                throw err;
            }
        }



        protected void imgBtnConfirmaExluirItem_Click(object sender, EventArgs e)
        {
            ExcluirItem(lblPluExcluir.Text);
            modalExcluirItem.Hide();
            int linha = 0;
            int.TryParse(lbllinhaGrid.Text, out linha);


            if (gridMercadoriasSelecionadas.Rows.Count > linha)
            {

                gridMercadoriasSelecionadas.Rows[linha].Focus();
            }
            else
            {
                gridMercadoriasSelecionadas.Rows[gridMercadoriasSelecionadas.Rows.Count - 1].Focus();

            }



        }

        protected void imgBtnCancelaExluirItem_Click(object sender, EventArgs e)
        {
            modalExcluirItem.Hide();
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

                solicitacao_producaoDAO obj = (solicitacao_producaoDAO)Session["objSolicita" + urlSessao()];
                Conexao.executarSql(sql, conn, tran);
                usr.consultaTodasFiliais = true;
                string codLista = Conexao.retornaUmValor("select IDENT_CURRENT( 'LISTA_PADRAO' ) ", usr, conn, tran);
                usr.consultaTodasFiliais = false;
                foreach (solicitacao_producao_itensDAO item in obj.arrItens)
                {
                    if (sqlItens.Length > 0)
                        sqlItens += ",";

                    sqlItens += "(" + codLista + ",'" + item.plu + "')";
                }

                Conexao.executarSql("insert into LISTA_PADRAO_ITENS values " + sqlItens, conn, tran);



                tran.Commit();
                showMessage("Salvo Lista padrao!", false);
            }
            catch (Exception err)
            {
                tran.Rollback();
                showMessage(err.Message, true);

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

            solicitacao_producaoDAO obj = (solicitacao_producaoDAO)Session["objSolicita" + urlSessao()];
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

        protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
        {
            if (!status.Equals("visualizar"))
            {

                carregarDadosObj(false);
                if (TabContainer1.ActiveTabIndex == 0)
                {
                    atualizaItens(gridMercadoriasSelecionadas);
                }
                else
                {
                    atualizaItens(gridItens);
                }
                carregarDados();

            }
        }
    }
}