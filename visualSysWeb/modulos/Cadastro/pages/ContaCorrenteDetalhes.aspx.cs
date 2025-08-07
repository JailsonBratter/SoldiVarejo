using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class ContaCorrenteDetalhes : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            User usr = (User)Session["User"];

            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    conta_correnteDAO obj = new conta_correnteDAO(usr);
                    status = "incluir";

                    bool contaCaixa = Conexao.retornaUmValor("Select COUNT(*) from Conta_Corrente where conta_caixa = 1 and filial ='" + usr.getFilial() + "'", usr).Equals("1");
                    
                    Session.Remove("ContaCorrente" + urlSessao());
                    Session.Add("ContaCorrente" + urlSessao(), obj);

                    EnabledControls(conteudo, true);
                    EnabledControls(cabecalho, true);
                    

                    if (!contaCaixa)
                    {
                        ModalConfirma.Show();
                    }
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
                            conta_correnteDAO obj = new conta_correnteDAO(index, usr);

                            Session.Remove("ContaCorrente" + urlSessao());
                            Session.Add("ContaCorrente" + urlSessao(), obj);
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
            if (status.Equals("visualizar"))
            {
                //btnTornarContaCaixa.Visible = true;
            }
            else
            {
                //btnTornarContaCaixa.Visible = false;
            }
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
            String[] campos = { "txtid_cc", 
                                    "txtBanco", 
                                    "txtAgencia", 
                                    "txtConta" 
                                     };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array
            if (!status.Equals("incluir"))
            {
                if (campo.ID.Equals("txtid_cc"))
                {
                    return true;
                }
            }
            else
            {
                if (campo.ID.Equals("txtsaldo"))
                {
                    conta_correnteDAO obj = (conta_correnteDAO)Session["ContaCorrente" + urlSessao()];
                    if (obj.conta_caixa)
                        return false;

                }
            }

            String[] campos = { "txtsaldo", 
                                    "txtNomeBanco", 
                                    "", 
                                    "" 
                                     };
            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("ContaCorrenteDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            status = "editar";
            editar(pnBtn);
            EnabledControls(cabecalho, true);
            EnabledControls(conteudo, true);
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ContaCorrente.aspx");
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            //pnConfima.Visible = true;
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
                String listaAtual = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                Session.Remove("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));

                if (listaAtual.Equals("txtBanco"))
                {
                    txtBanco.Text = selecionado;
                    txtNomeBanco.Text = ListaSelecionada(2);

                }
                else if (listaAtual.Equals("txtCentroDeCusto"))
                {
                    txtCentroDeCusto.Text = selecionado;
                    txtNomeCentro.Text = ListaSelecionada(4);
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
        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaCamposObrigatorios())
                {

                    carregarDadosObj();
                    conta_correnteDAO obj = (conta_correnteDAO)Session["ContaCorrente" + urlSessao()];
                    obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;
                    carregarDados();

                    EnabledControls(cabecalho, false);
                    EnabledControls(conteudo, false);
                    visualizar(pnBtn);

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
            Response.Redirect("ContaCorrente.aspx");//colocar endereco pagina de pesquisa
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
        //--Atualizar DaoForm 
        private void carregarDados()
        {
            conta_correnteDAO obj = (conta_correnteDAO)Session["ContaCorrente" + urlSessao()];
            txtid_cc.Text = obj.id_cc.ToString();
            txtBanco.Text = obj.Banco.ToString();
            txtAgencia.Text = obj.Agencia.ToString();
            txtConta.Text = obj.Conta.ToString();
            txtNomeBanco.Text = Conexao.retornaUmValor("Select nome_banco from banco where numero_banco='" + obj.Banco + "'", null);
            if (txtNomeBanco.Text.Equals(""))
            {
                txtNomeBanco.Text = obj.filial;
            }

            txtsaldo.Text = string.Format("{0:0,0.00}", obj.saldo);

            if (obj.conta_caixa)
            {
                lblContaCaixa.Text = "CONTA CAIXA";
            }
            else
            {
                lblContaCaixa.Text = "";
            }
            txtCentroDeCusto.Text = obj.codigo_centro_custo;
            txtNomeCentro.Text = obj.descricao_centro_custo;

        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            conta_correnteDAO obj = (conta_correnteDAO)Session["ContaCorrente" + urlSessao()];

            obj.id_cc = txtid_cc.Text;
            obj.Banco = txtBanco.Text;
            obj.Agencia = txtAgencia.Text;
            obj.Conta = txtConta.Text;
            obj.saldo = (txtsaldo.Text.Equals("") ? 0 : Decimal.Parse(txtsaldo.Text));
            obj.codigo_centro_custo = txtCentroDeCusto.Text;
            obj.descricao_centro_custo = txtNomeCentro.Text;


            Session.Remove("ContaCorrente" + urlSessao());
            Session.Add("ContaCorrente" + urlSessao(), obj);


        }


        protected void lista_click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            String panel = btn.Parent.ID;

            TxtPesquisaLista.Text = "";

            String or = btn.ID.Substring(7);

            Session.Remove("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), or);
            exibeLista();
        }

        private void exibeLista()
        {
            divPagPesquisa.Visible = true;
            divAddDetalhesBanco.Visible = false;
            divAddBanco.Visible = false;
            lblErroPesquisa.Text = "";
            String sqlLista = "";
            String or = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            switch (or)
            {
                case "txtBanco":
                    lbllista.Text = "Escolha um Banco";
                    sqlLista = "Select * from banco where nome_banco like '%" + TxtPesquisaLista.Text + "%'";
                    divAddBanco.Visible = true;

                    break;
                case "txtCentroDeCusto":
                    lbllista.Text = "Escolha um Centro de Custo";
                    sqlLista = "Select Codigo =codigo_centro_custo" +
                                     ",Grupo=descricao_grupo " +
                                     ",SubGrupo = descricao_subgrupo" +
                                     ",Centro_custo = descricao_centro_custo " +
                                " from centro_custo " +
                                "   inner join Subgrupo_CC on Centro_Custo.Codigo_subgrupo = Subgrupo_CC.Codigo_Subgrupo" +
                                "   inner join Grupo_CC on Subgrupo_CC.codigo_grupo = Grupo_CC.codigo_grupo " +
                                " where codigo_centro_custo like '%" + TxtPesquisaLista.Text + "%'" +
                                "    or descricao_grupo like '%" + TxtPesquisaLista.Text + "%'" +
                                "    or descricao_subgrupo like '%" + TxtPesquisaLista.Text + "%'" +
                                "    or descricao_centro_custo like '%" + TxtPesquisaLista.Text + "%'";
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
        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                conta_correnteDAO obj = (conta_correnteDAO)Session["ContaCorrente" + urlSessao()];
                obj.excluir();
                //pnConfima.Visible = false;
                lblError.Text = "Registro Excluido com sucesso";
                limparCampos();
                pesquisar(pnBtn);
                Session.Remove("ContaCorrente" + urlSessao());
            }
            catch (Exception err)
            {
                lblError.Text = "N?o foi possivel Excluir o registro error:" + err.Message;
            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            //pnConfima.Visible = false;
        }

        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {
            //TextBox txt = (TextBox)conteudo.FindControl(camporeceber);
            //txt.Text = "";
            //for (int i = 0; i < chkLista.Items.Count; i++)
            //{
            //    if (chkLista.Items[i].Selected)
            //    {
            //        txt.Text += chkLista.Items[i].Value;
            //    }
            //}
            //pnFundo.Visible = false;
        }


        protected void txtBanco_TextChanged(object sender, EventArgs e)
        {
            if (!txtBanco.Text.Equals(""))
            {
                txtNomeBanco.Text = Conexao.retornaUmValor("Select nome_banco from banco where numero_banco='" + txtBanco.Text + "'", null);
            }
            else
            {
                txtNomeBanco.Text = "";
            }
        }

        protected void btnTornarContaCaixa_Click(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            Conexao.executarSql("Update conta_corrente set conta_caixa=0 where filial='" + usr.getFilial() + "'");
            conta_correnteDAO obj = (conta_correnteDAO)Session["ContaCorrente" + urlSessao()];
            obj.conta_caixa = true;
            obj.salvar(false);
            Session.Remove("ContaCorrente" + urlSessao());
            Session.Add("ContaCorrente" + urlSessao(), obj);
            carregarDados();
        }
        protected void btnImgAddNovoBanco_Click(object sender, ImageClickEventArgs e)
        {
            txtAddNumeroBanco.Text = "";
            txtAddNomeBanco.Text = "";
            divAddBanco.Visible = false;
            divPagPesquisa.Visible = false;
            divAddDetalhesBanco.Visible = true;
            pnFundo.Height = 200;
            pnFundoFrame.Height = 180;
            pnFundo.DefaultButton = "btnImgConfirmaNovoBanco";
            lbllista.Text = "Adicionar novo banco";
            ModalFundo.Show();
        }

        protected void btnImgConfirmaNovoBanco_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                if (txtAddNumeroBanco.Text.Equals("") && !isnumero(txtAddNumeroBanco.Text))
                    throw new Exception("Numero Não informado ou invalido");
                if (txtAddNomeBanco.Text.Trim().Equals(""))
                    throw new Exception("Nome do Banco não preenchido");

                String strSql = " insert into Banco (Numero_Banco,Nome_Banco)values('" + txtAddNumeroBanco.Text.Trim() + "','" + txtAddNomeBanco.Text + "');";
                Conexao.executarSql(strSql);
                divAddBanco.Visible = true;
                divPagPesquisa.Visible = true;
                divAddDetalhesBanco.Visible = false;
                exibeLista();
                pnFundo.Height = 500;
                pnFundoFrame.Height = 480;
                pnFundo.DefaultButton = "ImgPesquisaLista";
                lbllista.Text = "Escolha um Banco";
            }
            catch (Exception err)
            {
                lblErroPesquisa.Text = err.Message;

            }
            ModalFundo.Show();
        }

        protected void btnImgCancelaNovoBanco_Click(object sender, ImageClickEventArgs e)
        {
            divAddBanco.Visible = true;
            divPagPesquisa.Visible = true;
            divAddDetalhesBanco.Visible = false;
            exibeLista();
            pnFundo.Height = 500;
            pnFundo.DefaultButton = "ImgPesquisaLista";
            pnFundoFrame.Height = 480;
            ModalFundo.Show();
        }

        //btnConfirmaContaCaixa_Click
        protected void btnConfirmaContaCaixa_Click(object sender, ImageClickEventArgs e)
        {
            User usr = (User)Session["User"];
            if(usr!=null)
            {
                conta_correnteDAO obj = (conta_correnteDAO)Session["ContaCorrente" + urlSessao()];
                obj.id_cc = "CAIXA-" + usr.getFilial();
                obj.Banco = "00";
                obj.Agencia = "00";
                obj.Conta = "0000";
                obj.conta_caixa = true;
                Session.Remove("ContaCorrente" + urlSessao());
                Session.Add("ContaCorrente" + urlSessao(),obj);
                carregarDados();
                EnabledControls(conteudo, true);
                txtsaldo.Focus();
            }
        }
        protected void btnCancelaContaCaixa_Click(object sender, ImageClickEventArgs e)
        {
        }
    }
}