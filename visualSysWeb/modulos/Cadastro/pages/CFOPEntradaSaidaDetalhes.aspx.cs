using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using System.Data.SqlClient;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class CFOPEntradaSaidaDetalhes : visualSysWeb.code.PagePadrao
    {
        
        static String camporeceber = "";
        protected void Page_Load(object sender, EventArgs e)
        {


            User usr = (User)Session["User"];

            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    CFOPEntradaSaidaDAO obj = new CFOPEntradaSaidaDAO();
                    status = "incluir";

                    Session.Remove("objCFOPEntradaSaida" + urlSessao());
                    Session.Add("objCFOPEntradaSaida" + urlSessao(), obj);

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
                            CFOPEntradaSaidaDAO obj = new CFOPEntradaSaidaDAO(index, usr);
                            Session.Remove("objCFOPEntradaSaida" + urlSessao());
                            Session.Add("objCFOPEntradaSaida" + urlSessao(), obj);
                            carregarDados();
                        }

                    }
                    catch (Exception err)
                    {
                        lblError.Text = err.Message;
                    }
                }
            }

            if (status.Equals("visualizar"))
            {
                EnabledControls(conteudo, false);
            }
            else
            {
                EnabledControls(conteudo, true);
            }
            carregabtn(pnBtn);
            camposnumericos();
        }

          private void camposnumericos()
        {
            txtCFOPEntrada.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
            txtCFOPSaida.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
        }

        private void limparCampos()
        {
            LimparCampos(conteudo);
        }

        protected bool validaCamposObrigatorios()
        {
            if ( validaCampos(conteudo))
                return true;
            else
                return false;
        }

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array
            String[] campos = {
                                "txtCFOPEntrada", 
                                "txtCFOPSaida",
                                "txtDESCRICAO"
                              };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array
            String[] campos = { "", 
                                    "", 
                                    "", 
                                    "" 
                                     };
            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("CFOPEntradaSaidaDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            editar(pnBtn);
            EnabledControls(conteudo, true);
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("CFOPEntradaSaida.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            modalExluirCFOP.Show();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaCamposObrigatorios())
                {
                   
                    carregarDadosObj();
                    CFOPEntradaSaidaDAO obj = (CFOPEntradaSaidaDAO)Session["objCFOPEntradaSaida" + urlSessao()];
                    obj.salvar(status.Equals("incluir")); 
                    EnabledControls(conteudo, false);
                    visualizar(pnBtn);
                    msgShow("Salvo com sucesso!", false);
                  
                }
                else
                {
                    msgShow("Campo Obrigatorio não preenchido",true);
                }
            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
            }
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("CFOPEntradaSaida.aspx");//colocar endereco pagina de pesquisa
        }
        
        //--Atualizar DaoForm 
        private void carregarDados()
        {
            CFOPEntradaSaidaDAO obj = (CFOPEntradaSaidaDAO)Session["objCFOPEntradaSaida" + urlSessao()];
            txtCFOPEntrada.Text = obj.CFOPEntrada;
            txtCFOPSaida.Text =  obj.CFOPSaida;
            txtDESCRICAO.Text = obj.DESCRICAO.ToString();
            
        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            CFOPEntradaSaidaDAO obj = (CFOPEntradaSaidaDAO)Session["objCFOPEntradaSaida" + urlSessao()];
            obj.CFOPEntrada = txtCFOPEntrada.Text;
            obj.CFOPSaida = txtCFOPSaida.Text;
            obj.DESCRICAO = txtDESCRICAO.Text;
            Session.Remove("objCFOPEntradaSaida" + urlSessao());
            Session.Add("objCFOPEntradaSaida" + urlSessao(), obj);
        }


        protected void lista_click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            exibeLista(btn.ID);
           
        }
        protected void exibeLista(String id)
        {
            String sqlLista = "Select * from cfop ";

            switch (id)
            {
                case "txtCFOPEntrada":
                    sqlLista += " Where tipo=2 ";
                    lbllista.Text = "Escolha a CFOP Entrada" ;
                    break;
                case "txtCFOPSaida":
                    sqlLista += " Where tipo=1 ";
                    lbllista.Text = "Escolha a CFOP Saida";
                    break;
            }
            sqlLista += " and(Cfop like '%" + TxtPesquisaLista.Text + "%' or Descricao like '%" + TxtPesquisaLista.Text + "%')";
            Session.Remove("lista" + urlSessao());
            Session.Add("lista" + urlSessao(), id);
            TxtPesquisaLista.Focus();
            GridLista.DataSource = Conexao.GetTable(sqlLista, null, false);
            GridLista.DataBind();
            ModalFundo.Show();
        }
        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            String id = (String)Session["lista" + urlSessao()];
            exibeLista(id);
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
        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            ModalFundo.Hide();
        }
        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                CFOPEntradaSaidaDAO obj = (CFOPEntradaSaidaDAO)Session["objCFOPEntradaSaida" + urlSessao()]; obj.excluir();
                modalExluirCFOP.Hide();

                msgShow("Registro Excluido com sucesso", false);
                limparCampos();
                Session.Remove("objCFOPEntradaSaida" + urlSessao());
                Session.Add("objCFOPEntradaSaida" + urlSessao(), obj);
                pesquisar(pnBtn);
            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
                
            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            modalExluirCFOP.Hide();
        }

        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {
            String id = (String)Session["lista" + urlSessao()];
            TextBox txt = (TextBox) conteudo.FindControl(id);
            txt.Text = ListaSelecionada(1);
        }


        protected void btnOkError_Click(object sender, EventArgs e)
        {
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
        protected void btnFechar_Click(object sender, ImageClickEventArgs e)
        {
            String selecionado = ListaSelecionada(1);

            if (!selecionado.Equals("") && !selecionado.Equals("------"))
            {
                String listaAtual = (String)Session["lista" + urlSessao()];

                if (listaAtual.Equals("txtCFOPEntrada"))
                {
                    txtCFOPEntrada.Text = selecionado;
                  
                }
                else if (listaAtual.Equals("txtCFOPSaida"))
                {
                    txtCFOPSaida.Text = selecionado;
                }

                ModalFundo.Hide();
            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                ModalFundo.Show();
            }
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