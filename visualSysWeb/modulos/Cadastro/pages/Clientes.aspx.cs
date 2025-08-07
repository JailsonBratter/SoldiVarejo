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
using senha;
using visualSysWeb.modulos.Cadastro.code;
using visualSysWeb.code;

namespace visualSysWeb.Cadastro
{
    public partial class Clientes : visualSysWeb.code.PagePadrao
    {
        static DataTable tb;
        static String sql = "select codigo_cliente, nome_cliente,CNPJ,convert(varchar,data_cadastro,103)Data_cadastro,CIDADE, CEP,NRO=endereco_nro, '' as DtVencimento, '' as Dias, convert(varchar,data_cadastro,102)DataCadOrdem " +
            " from cliente";
        static String UltimaOrdem = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //UltimaOrdem = "DataCadOrdem Desc";

                FiltroClientes filtro = (FiltroClientes)Session["filtroCliente"];

                if (filtro != null)
                {
                    txtCod_cliente.Text = filtro.codigo;
                    txtcliente.Value = filtro.cliente;
                    txtCnpj.Value = filtro.cnpj;
                    txtDataCadastro.Text = filtro.dtCadastro;
                    txtCidade.Text = filtro.cidade;
                    txtCEP.Text = filtro.cep;
                    chkInativo.Checked = filtro.inativo;
                    txtNomePet.Text = filtro.nomePet;
                    chkContaAssinada.Checked = filtro.contaAssinada;
                    txtTabPreco.Text = filtro.tabPreco;

                }
                Menus mn = (Menus)Session["menu"];
                try
                {


                    if (mn != null && mn.moduloAtivo("PetShop"))
                    {
                        DivPet.Visible = true;
                    }
                    else
                    {
                        DivPet.Visible = false;
                    }
                }
                catch (Exception)
                {
                }
            }

            if (!IsPostBack)
            {
                User usr = (User)Session["User"];
                if (usr != null)
                {
                    pesquisar("DataCadOrdem Desc", true);
                    chkInativo.Visible = usr.adm(usr.tela);

                }
            }


            pesquisar(pnBtns);
            txtDataCadastro.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
        }

        private void camposnumericos()
        {
            txtCod_cliente.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
            txtCnpj.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
        }
        protected void gridClientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridClientes.DataSource = tb;
            gridClientes.PageIndex = e.NewPageIndex;
            gridClientes.DataBind();
        }


        protected void imgPesquisar_Click(object sender, ImageClickEventArgs e)
        {

        }


        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/Cadastro/pages/ClienteDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void pesquisar(String ordem, bool limitar)
        {

            if (ordem.Equals(UltimaOrdem))
            {
                if (ordem.IndexOf("Desc") < 0)
                    ordem += " Desc";

                UltimaOrdem = "";
            }
            else
            {
                UltimaOrdem = ordem;
            }
            FiltroClientes filtro = new FiltroClientes();
            filtro.codigo = txtCod_cliente.Text;
            filtro.cliente = txtcliente.Value;
            filtro.cnpj = txtCnpj.Value;
            filtro.dtCadastro = txtDataCadastro.Text;
            filtro.cidade = txtCidade.Text;
            filtro.cep = txtCEP.Text;
            filtro.inativo = chkInativo.Checked;
            filtro.nomePet = txtNomePet.Text;
            filtro.contaAssinada = chkContaAssinada.Checked;
            filtro.tabPreco = txtTabPreco.Text;
            Session.Remove("filtroCliente");
            Session.Add("filtroCliente", filtro);
            try
            {
                String sqlWhere = "";
                String strSqltotalFiltro = "select count(*) from cliente";
                String totalRegistro = Conexao.retornaUmValor(strSqltotalFiltro, null);

                if (chkInativo.Checked)
                {
                    sqlWhere += " isnull(inativo,0) = 1";
                }
                else
                {
                    sqlWhere += " isnull(inativo,0) <> 1";
                }


                if (!txtCod_cliente.Text.Equals(""))
                {
                    if (!sqlWhere.Equals(""))
                    {
                        sqlWhere += " and ";
                    }
                    sqlWhere += " codigo_cliente = '" + txtCod_cliente.Text + "'";

                }

                if (!txtcliente.Value.Equals(""))
                {
                    if (!sqlWhere.Equals(""))
                    {
                        sqlWhere += " and ";
                    }

                    sqlWhere += " nome_cliente like '%" + txtcliente.Value + "%'";
                }
                if (!txtCnpj.Value.Equals(""))
                {
                    if (!sqlWhere.Equals(""))
                    {
                        sqlWhere += " and ";
                    }

                    sqlWhere += " replace(replace(replace(cnpj,'.',''),'-',''),'/','')like'" + txtCnpj.Value.Replace(".", "").Replace("-", "").Replace("/", "") + "%'";
                }
                if (!txtDataCadastro.Text.Equals(""))
                {
                    if (!sqlWhere.Equals(""))
                    {
                        sqlWhere += " and ";
                    }

                    sqlWhere += " Data_cadastro = '" + DateTime.Parse(txtDataCadastro.Text).ToString("yyyy-MM-dd") + "'";

                }
                if (!txtCidade.Text.Equals(""))
                {
                    if (!sqlWhere.Equals(""))
                    {
                        sqlWhere += " and ";
                    }

                    sqlWhere += " Cidade= '" + txtCidade.Text + "'";

                }
                if (!txtCEP.Text.Equals(""))
                {
                    if (!sqlWhere.Equals(""))
                    {
                        sqlWhere += " and ";
                    }

                    sqlWhere += " replace(replace(replace(CEP,'.',''),'-',''),'/','') = '" + txtCEP.Text.Replace("-", "") + "'";

                }

                if (!txtNomePet.Text.Equals(""))
                {
                    if (!sqlWhere.Equals(""))
                    {
                        sqlWhere += " and ";
                    }
                    sqlWhere += " codigo_cliente in (Select codigo_cliente from cliente_pet where Nome_Pet like '%" + txtNomePet.Text + "%') ";

                }
                if (!txtTabPreco.Text.Equals(""))
                {
                    if (!sqlWhere.Equals(""))
                    {
                        sqlWhere += " and ";
                    }
                    sqlWhere += " codigo_tabela = '" + txtTabPreco.Text + "' ";

                }
                if (chkContaAssinada.Checked)
                {
                    if (!sqlWhere.Equals(""))
                    {
                        sqlWhere += " and ";
                    }
                    sqlWhere += " conta_assinada = 1 ";
                }


                User usr = (User)Session["User"];
                //Sinto Muito Me Perdoe Agradeço Eu Te Amo.


                String sqlFinal = "";



                if (sqlWhere.Equals(""))
                {
                    sqlFinal = sql;

                }
                else
                {
                    sqlFinal = sql + " where " + sqlWhere;
                    strSqltotalFiltro += " where " + sqlWhere;
                }

                bool vSenha = false;

                try
                {


                    String kSenha = System.Web.Configuration.WebConfigurationManager.AppSettings["FLCRT"].ToString();
                    vSenha = kSenha.ToUpper().Equals("9.2");


                }
                catch (Exception)
                {
                    vSenha = false;
                }




                if (vSenha)
                {
                    ArrayList arrTb = new ArrayList();
                    SqlDataReader rs = Conexao.consulta(sqlFinal.Replace("'' as Dias", "chave_senha"), usr, false);
                    ArrayList cabecalho = new ArrayList();
                    cabecalho.Add("codigo_cliente");
                    cabecalho.Add("nome_cliente");
                    cabecalho.Add("CNPJ");
                    cabecalho.Add("Data_cadastro");
                    cabecalho.Add("DataCadOrdem");
                    cabecalho.Add("CIDADE");
                    cabecalho.Add("CEP");
                    cabecalho.Add("NRO");
                    cabecalho.Add("DtVencimento");
                    cabecalho.Add("DtVencimentoOrdem");
                    cabecalho.Add("Dias");
                    cabecalho.Add("DiasOrdem");
                    arrTb.Add(cabecalho);
                    while (rs.Read())
                    {
                        ArrayList item = new ArrayList();
                        item.Add(rs["codigo_cliente"].ToString());
                        item.Add(rs["nome_cliente"].ToString());
                        item.Add(rs["CNPJ"].ToString());
                        item.Add(rs["Data_cadastro"].ToString());
                        item.Add(rs["DataCadOrdem"].ToString());
                        item.Add(rs["CIDADE"].ToString());
                        item.Add(rs["CEP"].ToString());
                        item.Add(rs["NRO"].ToString());

                        try
                        {
                            String chvSenha = rs["chave_senha"].ToString();
                            Senha sn = new Senha(chvSenha);
                            item.Add(sn.dataValidade.ToString("dd/MM/yyyy"));
                            item.Add(sn.dataValidade.ToString("yyyyMMdd"));
                            TimeSpan qtdDias = sn.dataValidade - DateTime.Now.AddDays(-1);
                            item.Add(qtdDias.Days.ToString());
                            item.Add(qtdDias.Days.ToString().PadLeft(5, '0'));

                        }
                        catch (Exception)
                        {
                            item.Add("");
                            item.Add("");
                            item.Add("");
                            item.Add("");

                        }

                        arrTb.Add(item);
                    }
                    tb = Conexao.GetArryTable(arrTb);
                }
                else
                {
                    if (!IsPostBack)
                    {
                        tb = Conexao.GetTable(sqlFinal + " ORDER BY DataCadOrdem DESC" , null, limitar);
                    }
                    else
                    {
                        tb = Conexao.GetTable(sqlFinal + UltimaOrdem, null, limitar);
                    }
                    gridClientes.Columns[8].Visible = false;
                    gridClientes.Columns[9].Visible = false;
                }
                if (!ordem.Equals(" Desc"))
                {
                    DataView dv = tb.DefaultView;
                    dv.Sort = ordem;
                    tb = dv.ToTable();
                }
                gridClientes.DataSource = tb;
                gridClientes.DataBind();




                lblPesquisaErro.Text = "";

                String totalFiltro = Conexao.retornaUmValor(strSqltotalFiltro, usr);
                lblRegistros.Text = totalFiltro + " Registros de  " + totalRegistro + " Cadastrados ";

                gridClientes.Columns[0].Visible = chkInativo.Checked;
                imgBtnSalvarInativos.Visible = chkInativo.Checked;
                lblSalvarInativos.Visible = chkInativo.Checked;


                if (chkInativo.Checked)
                {
                    foreach (GridViewRow item in gridClientes.Rows)
                    {
                        item.CssClass = "linhaInativo";
                    }
                }

            }

            catch (Exception err)
            {

                lblPesquisaErro.Text = err.Message;
            }
        }

        protected void gridPesquisa_Sorting(object sender, GridViewSortEventArgs e)
        {
            //   pesquisar(e.SortExpression);
            String ordem = e.SortExpression;
            if (ordem.Equals(UltimaOrdem))
            {
                if (ordem.IndexOf("Desc") < 0)
                    ordem += " Desc";

                UltimaOrdem = "";
            }
            else
            {
                UltimaOrdem = ordem;
            }

            if (!ordem.Equals(" Desc"))
            {
                DataView dv = tb.DefaultView;
                dv.Sort = ordem;
                tb = dv.ToTable();
            }
            gridClientes.DataSource = tb;
            gridClientes.DataBind();
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisar(UltimaOrdem, false);
            HyperLink meuLink = (HyperLink)gridClientes.Rows[0].Cells[1].Controls[0];
            if (gridClientes.Rows.Count == 1 && !meuLink.Text.Equals("------"))
            {
                Response.Redirect("ClienteDetalhes.aspx?cod=" + meuLink.Text);
            }

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


        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            pesquisar(UltimaOrdem, false);
        }
        protected void imgBtnSalvarInativos_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow item in gridClientes.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        HyperLink meuLink = (HyperLink)item.Cells[1].Controls[0];
                        Conexao.executarSql("update cliente set inativo=0 where codigo_cliente = '" + meuLink.Text + "'");
                    }
                }
            }
            pesquisar(UltimaOrdem, false);
        }

        protected void ImgBtnTabPreco_Click(object sender, ImageClickEventArgs e)
        {
            Session.Remove("lista" + urlSessao());
            Session.Add("lista" + urlSessao(), "tabPreco");
            exibeLista();
        }
        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }
        private void exibeLista()
        {

            lblErroPesquisa.Text = "";
            String sqlLista = "";
            String or = (String)Session["lista" + urlSessao()];

            switch (or)
            {
                case "tabPreco":
                    lbllista.Text = "Escolha uma Tabela";
                    sqlLista = "select codigo_tabela from tabela_preco where codigo_tabela like '%" + TxtPesquisaLista.Text + "%'";
                    //divAddTabela.Visible = true;
                    break;


            }
            User usr = (User)Session["User"];
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
            ModalFundo.Show();

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
                ImageButton btn = (ImageButton)sender;
                String listaAtual = (String)Session["lista" + urlSessao()];
                Session.Remove("lista" + urlSessao());

                if (listaAtual.Equals("tabPreco"))
                {
                    txtTabPreco.Text = ListaSelecionada(1);

                }

                ModalFundo.Hide();
            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                ModalFundo.Show();
            }
        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            ModalFundo.Hide();
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
    }
    
}