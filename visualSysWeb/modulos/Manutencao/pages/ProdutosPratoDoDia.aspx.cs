using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class ProdutosPratoDoDia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                int dia = (int)DateTime.Now.DayOfWeek;
                ddlDiaSemana.SelectedValue = dia.ToString();
                pesquisa();

            }

        }

        private void pesquisa()
        {
            User usr = (User)Session["User"];
            if (usr == null)
                return;
            String sql = " Select mercadoria.plu " +
                            ", ean=(Select top 1 ean from ean where plu = mercadoria.plu)" +
                            ",ref=ref_fornecedor" +
                            ",mercadoria.descricao" +
                            ",preco = mercadoria_loja.preco " +
                        " from mercadoria " +
                            " inner join mercadoria_loja on mercadoria.plu = mercadoria_loja.plu " +
                        " Where isnull(mercadoria.inativo,0) =0 and prato_dia=1  AND mercadoria_loja.filial='" + usr.getFilial() + "'";
            String where1 = "";
            String where2 = " and prato_dia_" + ddlDiaSemana.SelectedItem.Value + "=1";

            if (!txtDescricao.Text.Trim().Equals(""))
            {
                where1 += " and mercadoria.descricao like '%" + txtDescricao.Text + "%'";
            }

            //quando nao tiver nenhum filtro fazer a consulta em branco;
            //if (where1.Length == 0)
            //    where1 = " and 1=2";

            //if (where2.Length == 0)
            //    where2 = " and 1=2";

            gridProdutos1.DataSource = Conexao.GetTable(sql + where1, null, false);
            gridProdutos1.DataBind();

            gridProdutos2.DataSource = Conexao.GetTable(sql + where2, null, false);
            gridProdutos2.DataBind();
        }
        protected void chkSeleciona1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTodos = (CheckBox)sender;

            foreach (GridViewRow item in gridProdutos1.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");

                if (chk != null)
                {
                    chk.Checked = chkTodos.Checked;
                    //incluirMercadoria(chk);
                }
            }


        }
        protected void chkSeleciona2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTodos = (CheckBox)sender;

            foreach (GridViewRow item in gridProdutos2.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");

                if (chk != null)
                {
                    chk.Checked = chkTodos.Checked;
                    //incluirMercadoria(chk);
                }
            }


        }

        protected void imgLista_Click(object sender, EventArgs e)
        {
            String idNome = ((ImageButton)sender).ID;

            Session.Remove("listaManu");
            Session.Add("listaManu", idNome);
            TxtPesquisaLista.Text = "";
            exibirLista();

        }

        protected void exibirLista()
        {


            lblErroPesquisa.Text = "";
            String idNome = (String)Session["listaManu"];
            String sql = "";
            switch (idNome)
            {
                case "imgBtnGrupo1":
                case "imgBtnGrupo2":
                    lbltituloLista.Text = "Escolha o Grupo";
                    sql = "Select Codigo =codigo_grupo, Descricao= descricao_grupo " +
                        "from grupo " +
                        "where codigo_grupo like '" + TxtPesquisaLista.Text + "%' or descricao_grupo like '%" + TxtPesquisaLista.Text + "%'";

                    break;
                case "imgBtnSubGrupo1":
                case "imgBtnSubGrupo2":
                    lbltituloLista.Text = "Escolha o SubGrupo";
                    sql = "Select Codigo =codigo_subgrupo, Descricao=descricao_subgrupo, Grupo = descricao_grupo " +
                        "from subgrupo inner join grupo on subgrupo.codigo_grupo = grupo.codigo_grupo " +
                        "where (codigo_subgrupo like '" + TxtPesquisaLista.Text + "%' " +
                        "or descricao_subgrupo like '%" + TxtPesquisaLista.Text + "%')";



                    break;
                case "imgBtnDepartamento1":
                case "imgBtnDepartamento2":
                    lbltituloLista.Text = "Escolha o Departamento";
                    sql = "Select Codigo =codigo_departamento" +
                                ", Descricao= Descricao_departamento" +
                                ", SubGrupo=subGrupo.descricao_subgrupo " +
                                ", Grupo = grupo.descricao_grupo" +
                          " from departamento inner join subgrupo on departamento.codigo_subgrupo = subgrupo.codigo_subgrupo" +
                                            " inner join grupo on subgrupo.codigo_grupo = grupo.codigo_grupo" +
                          " where (codigo_departamento like '" + TxtPesquisaLista.Text + "%' or descricao_departamento like '%" + TxtPesquisaLista.Text + "%')";



                    break;

            }

            GridLista.DataSource = Conexao.GetTable(sql, null, false);
            GridLista.DataBind();
            modalLista.Show();
        }

        protected void btnConfirmaLista_Click(object sender, EventArgs e)
        {
            String idNome = (String)Session["listaManu"];

            if (ListaSelecionada(1).Equals(""))
            {

                lblErroPesquisa.ForeColor = System.Drawing.Color.Red;
                lblErroPesquisa.Text = "Selecione uma Opção";
                modalLista.Show();
            }
            else
            {


                switch (idNome)
                {
                    //case "imgBtnGrupo1":
                    //    txtCodGrupo1.Text = ListaSelecionada(1);
                    //    txtDescricaoGrupo1.Text = ListaSelecionada(2);
                    //    txtCodSubGrupo1.Text = "";
                    //    txtDescricaoSubGrupo1.Text = "";
                    //    txtCodDepartamento1.Text = "";
                    //    txtDescricaoDepartamento1.Text = "";
                    //    break;
                    //case "imgBtnSubGrupo1":
                    //    txtCodGrupo1.Text = ListaSelecionada(1).Substring(0, 3);
                    //    txtDescricaoGrupo1.Text = ListaSelecionada(3);
                    //    txtCodSubGrupo1.Text = ListaSelecionada(1);
                    //    txtDescricaoSubGrupo1.Text = ListaSelecionada(2);
                    //    txtCodDepartamento1.Text = "";
                    //    txtDescricaoDepartamento1.Text = "";
                    //    break;
                    //case "imgBtnDepartamento1":
                    //    txtCodGrupo1.Text = ListaSelecionada(1).Substring(0, 3);
                    //    txtDescricaoGrupo1.Text = ListaSelecionada(4);
                    //    txtCodSubGrupo1.Text = ListaSelecionada(1).Substring(0, 6); ;
                    //    txtDescricaoSubGrupo1.Text = ListaSelecionada(3);
                    //    txtCodDepartamento1.Text = ListaSelecionada(1);
                    //    txtDescricaoDepartamento1.Text = ListaSelecionada(2);
                    //    break;
                    //case "imgBtnGrupo2":
                    //    txtCodGrupo2.Text = ListaSelecionada(1);
                    //    txtDescricaoGrupo2.Text = ListaSelecionada(2);
                    //    txtCodSubGrupo2.Text = "";
                    //    txtDescricaoSubGrupo2.Text = "";
                    //    txtCodDepartamento2.Text = "";
                    //    txtDescricaoDepartamento2.Text = "";
                    //    break;
                    //case "imgBtnSubGrupo2":
                    //    txtCodGrupo2.Text = ListaSelecionada(1).Substring(0, 3);
                    //    txtDescricaoGrupo2.Text = ListaSelecionada(3);
                    //    txtCodSubGrupo2.Text = ListaSelecionada(1);
                    //    txtDescricaoSubGrupo2.Text = ListaSelecionada(2);
                    //    txtCodDepartamento2.Text = "";
                    //    txtDescricaoDepartamento2.Text = "";
                    //    break;
                    //case "imgBtnDepartamento2":
                    //    txtCodGrupo2.Text = ListaSelecionada(1).Substring(0, 3);
                    //    txtDescricaoGrupo2.Text = ListaSelecionada(4);
                    //    txtCodSubGrupo2.Text = ListaSelecionada(1).Substring(0, 6); ;
                    //    txtDescricaoSubGrupo2.Text = ListaSelecionada(3);
                    //    txtCodDepartamento2.Text = ListaSelecionada(1);
                    //    txtDescricaoDepartamento2.Text = ListaSelecionada(2);
                    //    break;

                }

                modalLista.Hide();
                pesquisa();
            }

        }
        protected void btnCancelaLista_Click(object sender, EventArgs e)
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
        protected void ImgPesquisaLista_Click(object sender, EventArgs e)
        {
            exibirLista();
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

        protected void btn_Click(object sender, EventArgs e)
        {
            try
            {
                bool esq = ((Button)sender).ID.Equals("btnEsquerda");
                StringBuilder sql = new StringBuilder();


                GridView grid = null;
                if (esq)
                {
                    grid = gridProdutos2;
                }
                else
                {
                    grid = gridProdutos1;
                }

                foreach (GridViewRow item in grid.Rows)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                    if (chk.Checked)
                    {
                        sql.AppendLine("update mercadoria set " +
                                    " prato_dia_" + ddlDiaSemana.SelectedItem.Value + " =  " + (esq ? "0" : "1") +
                                    "where plu ='" + item.Cells[1].Text + "';");
                    }
                }
                if (sql.Length == 0)
                    throw new Exception("Escolha os itens a serem transferidos!");

                Conexao.executarSql(sql.ToString());
                pesquisa();
            }
            catch (Exception err)
            {

                showMessagem(err.Message, true);
            }


        }



        private void showMessagem(String msg, bool erro)
        {
            lblErroPanel.Text = msg;
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
        protected void btnOkError_Click(object sender, EventArgs e)
        {
            modalError.Hide();

            if (lblErroPanel.Text.Contains("Escolha itens para incluir"))
                modalIncluiItens.Show();
            //pesquisa();
        }

        protected void txtDescricao2_TextChanged(object sender, EventArgs e)
        {
            pesquisa();
        }

        protected void txtDescricao1_TextChanged(object sender, EventArgs e)
        {
            pesquisa();
        }


        protected void txt_TextChanged(object sender, EventArgs e)
        {



            TextBox txt = (TextBox)sender;
            String vChangeAtual = txt.ID + txt.Text;
            String txtChange = (String)Session["txtChange"];
            if (!txt.Text.Trim().Equals(""))
            {
                if (txtChange != null && vChangeAtual.Equals(txtChange))
                    return;
            }
            Session.Remove("txtChange");
            Session.Add("txtChange", vChangeAtual);

            //if (txt.Text.Trim().Equals(""))
            //{
            //    switch (txt.ID)
            //    {
            //        case "txtCodGrupo1":
            //            txtDescricaoGrupo1.Text = "";
            //            txtCodSubGrupo1.Text = "";
            //            txtDescricaoSubGrupo1.Text = "";
            //            txtCodDepartamento1.Text = "";
            //            txtDescricaoDepartamento1.Text = "";
            //            break;
            //        case "txtCodSubGrupo1":

            //            txtDescricaoSubGrupo1.Text = "";
            //            txtCodDepartamento1.Text = "";
            //            txtDescricaoDepartamento1.Text = "";
            //            break;
            //        case "txtCodDepartamento1":
            //            txtDescricaoDepartamento1.Text = "";
            //            break;
            //        case "txtCodGrupo2":
            //            txtDescricaoGrupo2.Text = "";
            //            txtCodSubGrupo2.Text = "";
            //            txtDescricaoSubGrupo2.Text = "";
            //            txtCodDepartamento2.Text = "";
            //            txtDescricaoDepartamento2.Text = "";
            //            break;
            //        case "txtCodSubGrupo2":
            //            txtDescricaoSubGrupo2.Text = "";
            //            txtCodDepartamento2.Text = "";
            //            txtDescricaoDepartamento1.Text = "";
            //            break;
            //        case "txtCodDepartamento2":
            //            txtDescricaoDepartamento2.Text = "";

            //            break;

            //    }
            //}
            //else
            //{
            //    switch (txt.ID)
            //    {
            //        case "txtCodGrupo1":
            //            txtDescricaoGrupo1.Text = Conexao.retornaUmValor("Select Descricao_grupo from grupo where codigo_grupo=" + txt.Text, null);
            //            txtCodSubGrupo1.Text = "";
            //            txtDescricaoSubGrupo1.Text = "";
            //            txtCodDepartamento1.Text = "";
            //            txtDescricaoDepartamento1.Text = "";
            //            txtCodSubGrupo1.Focus();
            //            break;
            //        case "txtCodSubGrupo1":
            //            txtCodGrupo1.Text = txt.Text.Substring(0, 3);
            //            txtDescricaoGrupo1.Text = Conexao.retornaUmValor("Select Descricao_grupo from grupo where codigo_grupo=" + txt.Text.Substring(0, 3), null);
            //            txtDescricaoSubGrupo1.Text = Conexao.retornaUmValor("Select Descricao_subgrupo from subgrupo where codigo_subgrupo='" + txt.Text + "'", null);
            //            txtCodDepartamento1.Text = "";
            //            txtDescricaoDepartamento1.Text = "";
            //            txtCodDepartamento1.Focus();
            //            break;
            //        case "txtCodDepartamento1":
            //            txtCodGrupo1.Text = txt.Text.Substring(0, 3);
            //            txtDescricaoGrupo1.Text = Conexao.retornaUmValor("Select Descricao_grupo from grupo where codigo_grupo=" + txt.Text.Substring(0, 3), null);
            //            txtCodSubGrupo1.Text = txt.Text.Substring(0, 6);
            //            txtDescricaoSubGrupo1.Text = Conexao.retornaUmValor("Select Descricao_subgrupo from subgrupo where codigo_subgrupo='" + txt.Text.Substring(0, 6) + "'", null);
            //            txtDescricaoDepartamento1.Text = Conexao.retornaUmValor("Select Descricao_departamento from departamento where codigo_departamento='" + txt.Text + "'", null);
            //            txtDescricao1.Focus();
            //            break;

            //        case "txtCodGrupo2":
            //            txtDescricaoGrupo2.Text = Conexao.retornaUmValor("Select Descricao_grupo from grupo where codigo_grupo=" + txt.Text, null);
            //            txtCodSubGrupo2.Text = "";
            //            txtDescricaoSubGrupo2.Text = "";
            //            txtCodDepartamento2.Text = "";
            //            txtDescricaoDepartamento2.Text = "";
            //            txtCodSubGrupo2.Focus();
            //            break;
            //        case "txtCodSubGrupo2":
            //            txtCodGrupo2.Text = txt.Text.Substring(0, 3);
            //            txtDescricaoGrupo2.Text = Conexao.retornaUmValor("Select Descricao_grupo from grupo where codigo_grupo=" + txt.Text.Substring(0, 3), null);
            //            txtDescricaoSubGrupo2.Text = Conexao.retornaUmValor("Select Descricao_subgrupo from subgrupo where codigo_subgrupo='" + txt.Text + "'", null);
            //            txtCodDepartamento2.Text = "";
            //            txtDescricaoDepartamento2.Text = "";
            //            txtCodDepartamento2.Focus();
            //            break;
            //        case "txtCodDepartamento2":
            //            txtCodGrupo2.Text = txt.Text.Substring(0, 3);
            //            txtDescricaoGrupo2.Text = Conexao.retornaUmValor("Select Descricao_grupo from grupo where codigo_grupo=" + txt.Text.Substring(0, 3), null);
            //            txtCodSubGrupo2.Text = txt.Text.Substring(0, 6);
            //            txtDescricaoSubGrupo2.Text = Conexao.retornaUmValor("Select Descricao_subgrupo from subgrupo where codigo_subgrupo='" + txt.Text.Substring(0, 6) + "'", null);
            //            txtDescricaoDepartamento2.Text = Conexao.retornaUmValor("Select Descricao_departamento from departamento where codigo_departamento='" + txt.Text + "'", null);
            //            txtDescricao2.Focus();
            //            break;

            //    }

            //}
            pesquisa();
        }

        protected void ddlDiaSemana_TextChanged(object sender, EventArgs e)
        {
            pesquisa();
        }

        protected void imgBtnPesquisa_Click(object sender, ImageClickEventArgs e)
        {
            pesquisa();
        }

        protected void imgBtnAddItens_Click(object sender, ImageClickEventArgs e)
        {
            Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            carregarGrupos("", "", "");
            carregarLinhas();
            carregarMercadorias(true);
        }

        protected void imgBtnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            modalConfirmaExclusao.Show();
        }

        protected void ImgBtnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {


                String sql = "update mercadoria set prato_dia =0 " +
                                                  ",prato_dia_1 = 0" +
                                                  ",prato_dia_2 = 0" +
                                                  ",prato_dia_3 = 0" +
                                                  ",prato_dia_4 = 0" +
                                                  ",prato_dia_5 = 0" +
                                                  ",prato_dia_6 = 0" +
                                                  ",prato_dia_7 = 0" +
                              " where plu in (";
                String plus = "";
                foreach (GridViewRow row in gridProdutos1.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("chkSelecionaItem");
                    if (chk.Checked)
                    {
                        if (plus.Length > 0)
                            plus += ",";
                        plus += "'" + row.Cells[1].Text + "'";
                    }

                }

                if (plus.Length == 0)
                    throw new Exception("Escolha os itens a serem desmarcados");
                sql += plus + ")";
                Conexao.executarSql(sql);
                pesquisa();
            }
            catch (Exception err)
            {
                showMessagem(err.Message, true);
            }
        }
        protected void ImgBtnCancelarExclusao_Click(object sender, ImageClickEventArgs e)
        {
            modalConfirmaExclusao.Hide();
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
            modalIncluiItens.Show();

            //if (grupo.Equals("") && subGrupo.Equals("") && departamento.Equals(""))
            //{
            //    carregarMercadorias(true);
            //}
            //else
            //{
            //    carregarMercadorias(false);
            //}
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
                txtfiltromercadoria.Text.Equals(""))
            {
                limitar = true;
            }


            User usr = (User)Session["user"];
            String sqlMercadoria = "Select distinct mercadoria.plu PLU," +
                                                   " isnull(ean.ean,'---')EAN," +
                                                   " mercadoria.Ref_fornecedor REFERENCIA, " +
                                                   " mercadoria.descricao DESCRICAO, " +
                                                   " mercadoria_loja.preco as [PRC VENDA] " +
                                             " from mercadoria inner join mercadoria_loja on mercadoria.plu = mercadoria_loja.plu " +
                                               " left join ean on mercadoria.plu=ean.plu  " +
                                               //" inner join W_BR_CADASTRO_DEPARTAMENTO on mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento "+
                                               " left join Fornecedor_Mercadoria on mercadoria.PLU = Fornecedor_Mercadoria.PLU  AND Mercadoria_Loja.Filial = Fornecedor_Mercadoria.Filial " +
                                    " where (mercadoria_loja.filial='" + usr.getFilial() + "') ";
            if (Funcoes.isnumero(txtfiltromercadoria.Text))
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



            gridMercadoria1.DataSource = Conexao.GetTable(sqlMercadoria + " order by mercadoria.descricao", usr, limitar);
            gridMercadoria1.DataBind();

            modalIncluiItens.Show();


        }
        private void verificaSelecionados()
        {



            foreach (GridViewRow item in gridMercadoria1.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                if (chk.Checked)
                {
                    incluirMercadoria(chk);
                    chk.Checked = false;
                }
            }
            carregarSelecionados();
            modalIncluiItens.Show();


        }
        private void incluirMercadoria(CheckBox ck1)
        {
            GridViewRow linha = (GridViewRow)ck1.NamingContainer;


            ArrayList itens = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            if (itens == null)
                itens = new ArrayList();

            String PLU = linha.Cells[1].Text;//PLU

            foreach (String item in itens)
            {
                if (item.Equals(PLU))
                    return;
            }

            itens.Add(PLU);

            Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), itens);



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
            txtfiltromercadoria.Text = "";
            modalIncluiItens.Show();
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
            modalIncluiItens.Show();

        }
        protected void GridMercadoriaSelecionado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);

            String PLU = gridMercadoriasSelecionadas.Rows[index].Cells[0].Text;
            ArrayList itens = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            foreach (String item in itens)
            {

                if (item.Equals(PLU))
                {
                    itens.Remove(item);
                    break;
                }

            }
            Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), itens);

            carregarSelecionados();
            modalIncluiItens.Show();

        }

        private void carregarSelecionados()
        {

            ArrayList itens = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];


            if (itens == null)
                itens = new ArrayList();

            String plus = "''";

            foreach (string item in itens)
            {


                plus += ",'" + item + "'";
            }


            String sql = "Select plu, Ean=(Select top 1 ean from ean where plu = mercadoria.plu),Referencia=Ref_fornecedor,Descricao from mercadoria where plu in (" + plus + ");";
            gridMercadoriasSelecionadas.DataSource = Conexao.GetTable(sql, null, false);
            gridMercadoriasSelecionadas.DataBind();

        }

        protected void gridItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int index = Convert.ToInt32(e.CommandArgument);


        }

        protected void imgBtnIncluirSelecionados_Click(object sender, ImageClickEventArgs e)
        {
            verificaSelecionados();
        }
        protected void ImgBtnConfirmar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                String sql = "update mercadoria set prato_dia = 1 where plu in(";
                String plus = "";
                foreach (GridViewRow item in gridMercadoriasSelecionadas.Rows)
                {
                    if(!item.Cells[0].Text.Equals("------"))
                    {

                    if (plus.Length > 0)
                        plus += ",";

                    plus += item.Cells[0].Text;

                    }
                }
                if (plus.Length == 0)
                    throw new Exception("Escolha itens para incluir!");
                sql += plus + ");";
                Conexao.executarSql(sql);
            }
            catch (Exception err)
            {

                showMessagem(err.Message, true);
            }
            modalIncluiItens.Hide();

            pesquisa();
        }
        protected void ImgBtnCancelar_Click(object sender, ImageClickEventArgs e)
        {
            modalIncluiItens.Hide();
        }
    }
}