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

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class FamiliaDetalhes : visualSysWeb.code.PagePadrao
    {

        static String camporeceber = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];


            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    familiaDAO obj = new familiaDAO(usr);
                    status = "incluir";
                    EnabledControls(conteudo, true);
                    EnabledControls(cabecalho, true);
                    Session.Remove("familia" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("familia" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
                    txtQtdEtiquetas.Text = "1";
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
                            String index = Request.Params["campoIndex"].ToString();// colocar o campo index da tabela
                            status = "visualizar";
                            familiaDAO obj = new familiaDAO(index, usr);
                            Session.Remove("familia" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                            Session.Add("familia" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
                            carregarDados();
                        }
                        if (status.Equals("visualizar"))
                        {
                            EnabledControls(conteudo, false);
                            EnabledControls(cabecalho, false);
                        }
                        else
                        {
                            EnabledControls(conteudo, true);
                            EnabledControls(cabecalho, true);
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


        private void camposnumericos()
        {
            ArrayList campos = new ArrayList();
            campos.Add("txtPrecoFamilia");
            FormataCamposNumericos(campos, conteudo);
            FormataCamposNumericos(campos, cabecalho);

            ArrayList camposInteiros = new ArrayList();
            camposInteiros.Add("txtQtdEtiquetas");
            FormataCamposInteiros(camposInteiros, conteudo);
            FormataCamposInteiros(camposInteiros, cabecalho);

        }

        private void limparCampos()
        {
            LimparCampos(cabecalho);
            LimparCampos(conteudo);
        }

        protected bool validaCamposObrigatorios()
        {

            if (validaCampos(cabecalho) && validaCampos(conteudo))
            {
                if (Decimal.Parse(txtPrecoFamilia.Text) > 0)
                    return true;
                else
                {
                    TabContainer1.ActiveTabIndex = 1;
                    txtPrecoFamilia.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Preço zerado");
                }

            }
            else
                return false;
        }

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array
            String[] campos = {     "txtDescricao_Familia", 
                                    "txtPrecoFamilia",
                                    "txtQtdEtiquetas", 
                                    "txtDescricaoEtiqueta",
                                "txtPrecoFamilia"
                                     };
            return existeNoArray(campos, campo.ID + "");

        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array
            String[] campos = { "txtCodigo_familia", 
                                    "", 
                                    "", 
                                    "" 
                                     };
            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FamiliaDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            editar(pnBtn);

            EnabledControls(cabecalho, true);
            EnabledControls(conteudo, true);
            carregarDados();
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("familia.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            modalPnConfirma.Show();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaCamposObrigatorios())
                {

                    carregarDadosObj();
                    familiaDAO obj = (familiaDAO)Session["familia" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

                    obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                    status = "visualizar";
                    visualizar(pnBtn);
                    carregarDados();
                    EnabledControls(conteudo, false);
                    EnabledControls(cabecalho, false);

                  

                }
                else
                {
                    lblError.Text = "Campo Obrigatorio não preenchido";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception err)
            {
                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
                carregarDados();

            }
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("familia.aspx");//colocar endereco pagina de pesquisa
        }

        //--Atualizar DaoForm 
        private void carregarDados()
        {
            familiaDAO obj = (familiaDAO)Session["familia" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            txtCodigo_familia.Text = obj.Codigo_familia.ToString();
            txtDescricao_Familia.Text = obj.Descricao_Familia.ToString();

            txtQtdEtiquetas.Text = obj.qtd_Etiqueta.ToString();
            txtPrecoFamilia.Text = obj.preco.ToString("N2");
            chkImprimeEtiquetaItens.Checked = obj.imprimeEtiquetaItens;
            gridItens.DataSource = obj.itens;
            gridItens.DataBind();


        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            familiaDAO obj = (familiaDAO)Session["familia" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            obj.Codigo_familia = txtCodigo_familia.Text;
            obj.Descricao_Familia = txtDescricao_Familia.Text;
            obj.preco = (txtPrecoFamilia.Text.Equals("") ? 0 : Decimal.Parse(txtPrecoFamilia.Text));

            obj.qtd_Etiqueta = (txtQtdEtiquetas.Text.Equals("") ? 0 : int.Parse(txtQtdEtiquetas.Text));
            obj.aplicarTodasFiliais = chkAplicarTodasFilias.Checked;
            obj.imprimeEtiquetaItens = chkImprimeEtiquetaItens.Checked;

            Session.Remove("familia" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("familia" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
        }


        protected void lista_click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            pnFundo.Visible = true;
            chkLista.Items.Clear();
            String sqlLista = "";

            switch (btn.ID)
            {
                case "idBotao":
                    sqlLista = "Query de pesquisa com no minimo 2campos";
                    lbllista.Text = "Pagamentos";
                    camporeceber = "txtPagamento";
                    break;
            }
            User usr = (User)Session["User"];
            SqlDataReader lista = Conexao.consulta(sqlLista, usr, true);

            while (lista.Read())
            {
                ListItem item = new ListItem();
                item.Value = lista[0].ToString();
                item.Text = lista[1].ToString();
                chkLista.Items.Add(item);
            }
            if (lista != null)
                lista.Close();

        }

        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                familiaDAO obj = (familiaDAO)Session["familia" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

                obj.excluir();
                modalPnConfirma.Hide();
                lblError.Text = "Registro Excluido com sucesso";
                limparCampos();
                Session.Remove("familia" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                pesquisar(pnBtn);
            }
            catch (Exception err)
            {
                lblError.Text = "Não foi possivel Excluir o registro error:" + err.Message;
            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            modalPnConfirma.Hide();
        }

        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {
            TextBox txt = (TextBox)conteudo.FindControl(camporeceber);
            txt.Text = "";
            for (int i = 0; i < chkLista.Items.Count; i++)
            {
                if (chkLista.Items[i].Selected)
                {
                    txt.Text += chkLista.Items[i].Value;
                }
            }
            pnFundo.Visible = false;
        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            pnFundo.Visible = false;
        }

        protected void ImgBtnAddItens_Click(object sender, ImageClickEventArgs e)
        {
            carregarDadosObj();
            Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            carregarMercadorias();
        }
        protected void gridItens_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //for (int i = 0; i < e.Row.Cells.Count; i++)
                //{
                //    if (isnumero(e.Row.Cells[i].Text))
                //    {
                //        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                //        e.Row.Cells[i].Text = Decimal.Parse(e.Row.Cells[i].Text).ToString("N2");

                //    }
                //}
            }

        }

        protected void carregarMercadorias()
        {
            lblErroPesquisa.Text = "";
            User usr = (User)Session["user"];
            String sqlMercadoria = "Select top 300 mercadoria.plu,isnull(ean.ean,'---')EAN,mercadoria.descricao from mercadoria left join ean on mercadoria.plu=ean.plu " +
                                    " where ISNULL(Inativo,0)=0 AND   ISNULL(MERCADORIA.Codigo_familia,'') = '' AND ( mercadoria.plu = '" + txtfiltromercadoria.Text + "' or ean like '%" + txtfiltromercadoria.Text + "%' or descricao like '%" + txtfiltromercadoria.Text + "%')";

            gridMercadoria1.DataSource = Conexao.GetTable(sqlMercadoria, null, false);
            gridMercadoria1.DataBind();
            ArrayList todosSel = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            if (todosSel == null)
            {
                todosSel = new ArrayList();
                ArrayList cabecalho = new ArrayList();
                cabecalho.Add("PLU");
                cabecalho.Add("Descricao");
                cabecalho.Add("custo");
                cabecalho.Add("margem");
                cabecalho.Add("Preco");
                cabecalho.Add("Promocao");

                todosSel.Add(cabecalho);
            }
            Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), todosSel);
            GridMercadoriaSelecionado.DataSource = Conexao.GetArryTable(todosSel);
            GridMercadoriaSelecionado.DataBind();

            modalMercadorialista.Show();

        }
        protected void gridItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!status.Equals("visualizar"))
            {

                int index = Convert.ToInt32(e.CommandArgument);
                familiaDAO obj = (familiaDAO)Session["familia" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                obj.excluirItem(index);
            }
            carregarDados();

        }
        protected void btnFecharMecadoria_Click(object sender, ImageClickEventArgs e)
        {
            User usr = (User)Session["User"];
            ArrayList selecionados = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            if (selecionados != null && selecionados.Count > 1)
            {
                familiaDAO obj = (familiaDAO)Session["familia" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                foreach (GridViewRow sl in GridMercadoriaSelecionado.Rows)
                {

                    String plu = sl.Cells[1].Text;
                    String Descricao = sl.Cells[2].Text;
                    String custo = sl.Cells[3].Text;
                    String margem = sl.Cells[4].Text;
                    String preco = sl.Cells[5].Text;
                    String promocao = sl.Cells[6].Text;


                    obj.addItens(plu, Descricao, custo, margem, preco,promocao);

                    Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    modalMercadorialista.Hide();


                }


            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione os Produtos";
                modalMercadorialista.Show();
            }
            carregarDados();


        }
        protected void btnCancelaMercadoria_Click(object sender, ImageClickEventArgs e)
        {
            modalMercadorialista.Hide();
            carregarDados();
        }
        protected void txtfiltromercadoria_TextChanged(object sender, EventArgs e)
        {
            carregarMercadorias();
        }
        protected void ImgPesquisaMercadoria_Click(object sender, ImageClickEventArgs e)
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
                    chk.Checked = (sender as CheckBox).Checked;
                }
            }
            modalMercadorialista.Show();
        }

        protected void ImgBtnAddSelecionado_Click(object sender, ImageClickEventArgs e)
        {
            ArrayList todosSel = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            User usr = (User)Session["User"];

            if (todosSel != null)
            {
                foreach (GridViewRow linha in gridMercadoria1.Rows)
                {
                    CheckBox rdo = (CheckBox)linha.FindControl("chkSelecionaItem");
                    if (rdo.Checked)
                    {
                        ArrayList sel = new ArrayList();
                        MercadoriaDAO merc = new MercadoriaDAO(linha.Cells[1].Text, usr);
                        sel.Add(merc.PLU);
                        sel.Add(merc.Descricao_resumida);
                        sel.Add(merc.Preco_Custo.ToString("N2"));
                        sel.Add(merc.Margem.ToString("N4"));
                        sel.Add(merc.Preco.ToString("N2"));
                        sel.Add(merc.Preco_promocao.ToString("N2"));
                        todosSel.Add(sel);
                    }

                }

                Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), todosSel);
            }
            carregarMercadorias();
        }
        protected void GridMercadoriaSelecionado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument) + 1;
            ArrayList selecionados = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            selecionados.RemoveAt(index);
            Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), selecionados);
            carregarMercadorias();
        }
    }
}