using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.modulos.Cadastro.code;
using visualSysWeb.code;
using visualSysWeb.dao;
using System.Globalization;

namespace visualSysWeb.modulos.Estoque.pages
{
    public partial class SugestaoCompra : visualSysWeb.code.PagePadrao
    {

        static String ultimaOrdem = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            status = "pesquisar";
            carregabtn(pnBtn, "Gerar", null, null, null, null, null);
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

                }
                User usr = (User)Session["User"];
                if (usr != null)
                {
                    //pesquisar(true);
                }
            }

            txtEan.Attributes.Add("OnKeyPress", "javascript:return numeros(this,event);");
            txtPlu.Attributes.Add("OnKeyPress", "javascript:return numeros(this,event);");
            txtMercadoria.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");


            //GridViewRow row = gridMercadorias.Rows[-1];

            //row.Cells[8].Text = DateTime.Now.AddMonths(-4).ToString("MMMM", new CultureInfo("pt-BR")).ToUpper().Substring(0, 3);
            //row.Cells[9].Text = DateTime.Now.AddMonths(-3).ToString("MMMM", new CultureInfo("pt-BR")).ToUpper().Substring(0, 3);
            //row.Cells[10].Text = DateTime.Now.AddMonths(-2).ToString("MMMM", new CultureInfo("pt-BR")).ToUpper().Substring(0, 3);
            //row.Cells[11].Text = DateTime.Now.AddMonths(-1).ToString("MMMM", new CultureInfo("pt-BR")).ToUpper().Substring(0, 3); 

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
            modalEscolher.Show();
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisar(false);
        }

        protected void pesquisar(bool limitar)
        {
            pesquisar(limitar, ultimaOrdem);
        }


        protected void pesquisar(bool limitar, String ordem)
        {



            User usr = (User)Session["User"];
            String strSql = " execute sp_estoque_sugestao_compra '" + usr.getFilial() + "'" +
                                ",'" + txtPlu.Text + "'" +
                                ",'" + txtEan.Text + "'" +
                                ",'" + txtMercadoria.Text + "'" +
                                ",'" + txtNcm.Text + "'" +
                                ",'" + DllCampo1.Text + "'" +
                                ",'" + DllOnde.Text + "'" +
                                ",'" + DllCampo2.Text + "'" +
                                ",'" + txtRefForn.Text + "'" +
                                ",'" + txtGrupo.Text + "'" +
                                ",'" + txtSubGrupo.Text + "'" +
                                ",'" + txtDepartamento.Text + "'" +
                                ",'" + txtFamilia.Text + "'" +
                                ",'" + ddlTipoData.Text + "'" +
                                ",'" + txtDe.Text + "'" +
                                ",'" + txtAte.Text + "'" +
                                "," + (ordem.Equals("") ? "null" : ordem) +
                                "," + txtPrazo.Text +
                                "," + (chkEmbalagem.Checked ? "1" : "0") +
                                ",'" + txtFornecedor.Text + "'" + 
                                ",'" + txtMarca.Text + "'";



            gridMercadorias.DataSource = Conexao.GetTable(strSql, usr, false);
            gridMercadorias.DataBind();

            atualizarGrid();



        }

        private void atualizarGrid()
        {
            Decimal vltTotal = 0;
            foreach (GridViewRow row in gridMercadorias.Rows)
            {
                TextBox txtQtde = (TextBox)row.FindControl("txtQtde_Compra");
                if (txtQtde != null && !txtQtde.Text.Equals("------"))
                {

                    CheckBox chk = (CheckBox)row.FindControl("chkSelecionaItem");
                    if (txtQtde.Text.Equals("0") && chk != null)
                    {
                        txtQtde.BackColor = System.Drawing.Color.White;
                        txtQtde.ForeColor = System.Drawing.Color.Black;
                        chk.Checked = false;
                    }
                    else
                    {
                        Decimal prcCusto = Decimal.Parse(row.Cells[6].Text);
                        vltTotal += (Decimal.Parse(txtQtde.Text) * prcCusto);
                        chk.Checked = true;
                        Label lblQtde = (Label)row.FindControl("lblQtdeCompra");
                        if (!txtQtde.Text.Equals(lblQtde.Text))
                        {
                            txtQtde.BackColor = System.Drawing.Color.FromArgb(255, 128, 128);

                        }
                        else
                        {
                            txtQtde.BackColor = System.Drawing.Color.White;

                        }
                    }

                }
            }
            lblTotal.Text = vltTotal.ToString("N2");
        }
        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {

        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
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


                HyperLink meuLink = (HyperLink)gridMercadorias.Rows[i].Cells[7].Controls[0];
                if (!meuLink.Text.Equals("0,00") && !meuLink.Text.Equals("------"))
                {
                    gridMercadorias.Rows[i].BackColor = System.Drawing.Color.Tomato;

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
                    //foreach (TableCell valor in item.Cells)
                    //{
                    //    String str = valor.Text;

                    //}

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
                    if (!chk.Checked)
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
                case "imgFornecedor":
                    Session.Add("camporecebe" + urlSessao(), "Fornecedor");
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
                    sqlLista = " Select descricao_grupo from grupo  where descricao_grupo like '%" + TxtPesquisaLista.Text + "%' ";
                    lbllista.Text = "Grupo";
                    break;
                case "SubGrupo":
                    sqlLista = " Select descricao_subGrupo from W_BR_CADASTRO_DEPARTAMENTO where descricao_subGrupo  like '%" + TxtPesquisaLista.Text + "%'" + (txtGrupo.Text.Equals("") ? "" : " and descricao_grupo ='" + txtGrupo.Text + "' group by descricao_subGrupo"); ;
                    lbllista.Text = "Sub Grupo";
                    break;
                case "Departamento":
                    sqlLista = "select descricao_departamento  from W_BR_CADASTRO_DEPARTAMENTO where descricao_departamento like '%" + TxtPesquisaLista.Text + "%' " + (txtSubGrupo.Text.Trim().Equals("") ? "" : " and descricao_subgrupo = '" + txtSubGrupo.Text + "'"); ;
                    lbllista.Text = "Departamento";
                    break;
                case "Familia":
                    sqlLista = "Select descricao_familia from familia  where descricao_familia like '%" + TxtPesquisaLista.Text + "%'";
                    lbllista.Text = "Familia";
                    break;
                case "Fornecedor":
                    sqlLista = "Select Fornecedor = fornecedor, [Razao Social] =Razao_social,CNPJ from fornecedor  where (fornecedor like '%" + TxtPesquisaLista.Text + "%')"
                                                                            + " or razao_social like '%" + TxtPesquisaLista.Text + "%'"
                                                                            + " or replace(replace(replace(cnpj,'.',''),'-',''),'/','') like '%" + TxtPesquisaLista.Text.Replace(".", "").Replace("-", "").Replace("/", "") + "%'";
                    lbllista.Text = "Fornecedor";
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
                }
                else if (listaAtual.Equals("SubGrupo"))
                {
                    txtSubGrupo.Text = ListaSelecionada(1);
                }
                else if (listaAtual.Equals("Departamento"))
                {
                    txtDepartamento.Text = ListaSelecionada(1);
                }
                else if (listaAtual.Equals("Familia"))
                {
                    txtFamilia.Text = ListaSelecionada(1);
                }
                else if (listaAtual.Equals("Fornecedor"))
                {
                    txtFornecedor.Text = ListaSelecionada(1);
                    fornecedorDAO fornOcorrencia = new fornecedorDAO(txtFornecedor.Text, null);
                    if (fornOcorrencia.ocorrenciasPendentes > 0)
                    {
                        EnabledControls(pnOcorrencias, true);

                        gridOcorrencias.DataSource = fornOcorrencia.ocorrenciasPendentesDT;
                        gridOcorrencias.DataBind();

                        modalOcorrencia.Show();
                    }
                    else
                    {
                        lblError.Text = "";
                    }
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
            if (e.Row.RowType.Equals(DataControlRowType.Header))
            {
                int i = e.Row.RowIndex;
                e.Row.Cells[9].Text = DateTime.Now.AddMonths(-4).ToString("MMMM", new CultureInfo("pt-BR")).ToUpper().Substring(0, 3);
                e.Row.Cells[10].Text = DateTime.Now.AddMonths(-3).ToString("MMMM", new CultureInfo("pt-BR")).ToUpper().Substring(0, 3);
                e.Row.Cells[11].Text = DateTime.Now.AddMonths(-2).ToString("MMMM", new CultureInfo("pt-BR")).ToUpper().Substring(0, 3);
                e.Row.Cells[12].Text = DateTime.Now.AddMonths(-1).ToString("MMMM", new CultureInfo("pt-BR")).ToUpper().Substring(0, 3);

            }

        }

        protected void gridMercadorias_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //if (e.Row.RowType.Equals(DataControlRowType.Header))
            //{
            //    int i = e.Row.RowIndex;
            //      e.Row.Cells[8].Text= DateTime.Now.AddMonths(-4).ToString("MMMM", new CultureInfo("pt-BR")).ToUpper().Substring(0,3);
            //      e.Row.Cells[9].Text = DateTime.Now.AddMonths(-3).ToString("MMMM", new CultureInfo("pt-BR")).ToUpper().Substring(0, 3);
            //      e.Row.Cells[10].Text = DateTime.Now.AddMonths(-2).ToString("MMMM", new CultureInfo("pt-BR")).ToUpper().Substring(0, 3);
            //      e.Row.Cells[11].Text = DateTime.Now.AddMonths(-1).ToString("MMMM", new CultureInfo("pt-BR")).ToUpper().Substring(0, 3); 

            //}
        }

        protected void btnCotacao_Click(object sender, ImageClickEventArgs e)
        {
            User usr = (User)Session["User"];
            gridCotacaoAberta.DataSource = Conexao.GetTable("SELECT COTACAO,CONVERT(VARCHAR,DATA,103)DATA , DESCRICAO FROM Cotacao WHERE Status='ABERTO'", usr, false);
            gridCotacaoAberta.DataBind();
            pnSugestao.Visible = false;
            pnCotacaoEscolha.Visible = true;


        }
        protected void btnPedido_Click(object sender, ImageClickEventArgs e)
        {
            User usr = (User)Session["User"];
            pedidoDAO ped = new pedidoDAO(usr);
            if (!txtFornecedor.Text.Equals(""))
            {
                ped.Cliente_Fornec = txtFornecedor.Text;
            }
            ped.Obs = "Pedido Gerado por Sugestão de compra no dia: " + DateTime.Now.ToString("dd/MM/yyyy - hh:mm");
            foreach (GridViewRow rw in gridMercadorias.Rows)
            {
                CheckBox chk = (CheckBox)rw.FindControl("chkSelecionaItem");
                if (chk.Checked)
                {
                    pedido_itensDAO item = new pedido_itensDAO(usr);
                    item.PLU = rw.Cells[0].Text;
                    TextBox txtQtde = (TextBox)rw.FindControl("txtQtde_Compra");
                    item.Embalagem = Decimal.Parse(rw.Cells[4].Text);
                    if (item.Embalagem == 0)
                        item.Embalagem = 1;

                    item.Qtde = (Decimal.Parse(txtQtde.Text) / item.Embalagem);
                    item.unitario = Decimal.Parse(rw.Cells[6].Text);
                    ped.addItens(item);
                }
            }
            Session.Remove("PedidoSugestao");
            Session.Add("PedidoSugestao", ped);



            Response.Redirect("~/modulos/pedidos/pages/PedidoCompraDetalhes.aspx?novo=true");

        }

        protected void txtQtde_Compra_TextChanged(object sender, EventArgs e)
        {

            atualizarGrid();

            TextBox txt = (TextBox)sender;
            txt.Focus();

        }



        protected void btnAdicionaCotacao_Click(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            Button btn = (Button)sender;
            GridViewRow rowCt = (GridViewRow)btn.Parent.Parent;
            cotacaoDAO cotacao = new cotacaoDAO(rowCt.Cells[0].Text, usr);
            addItens(cotacao);
            Response.Redirect("~/modulos/Estoque/pages/cotacaoDetalhes.aspx?campoIndex=" + cotacao.Cotacao);

        }
        private void addItens(cotacaoDAO cotacao)
        {
            try
            {

            
            User usr = (User)Session["User"];
            foreach (GridViewRow rw in gridMercadorias.Rows)
            {
                CheckBox chk = (CheckBox)rw.FindControl("chkSelecionaItem");
                if (chk.Checked)
                {
                    cotacao_itemDAO item = new cotacao_itemDAO(usr);
                    item.Mercadoria = rw.Cells[0].Text;
                    TextBox txtQtde = (TextBox)rw.FindControl("txtQtde_Compra");
                    item.descricao = rw.Cells[3].Text;
                    item.embalagem = Decimal.Parse(rw.Cells[4].Text);
                    item.Quantidade = (Decimal.Parse(txtQtde.Text) / item.embalagem);
                    item.preco_compra = Decimal.Parse(rw.Cells[6].Text);
                    cotacao.addItem(item);
                }
            }
            }
            catch (Exception err)
            {

                lblErroPesquisa.Text = err.Message;
            }
            Session.Remove("CotacaoManter");
            Session.Add("CotacaoManter", cotacao);
        }

        protected void btnNovaCotacao_Click(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            cotacaoDAO cotacao = new cotacaoDAO(usr);
            addItens(cotacao);


            Response.Redirect("~/modulos/manutencao/pages/cotacaoDetalhes.aspx?novo=true");

        }

        protected void imgBtnFechar_Click(object sender, ImageClickEventArgs e)
        {
            pnSugestao.Visible = true;
            pnCotacaoEscolha.Visible = false;

        }

        protected void imgBtnCancelaOcorrencias_Click(object sender, ImageClickEventArgs e)
        {
            modalOcorrencia.Hide();
        }
    }
}
