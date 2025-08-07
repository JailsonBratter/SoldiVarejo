using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;
using visualSysWeb.modulos.Cadastro.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class EmbaladoraDetalhes : visualSysWeb.code.PagePadrao
    {

        static String camporeceber = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            User usr = (User)Session["User"];

            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    EmbaladoraDAO obj = new EmbaladoraDAO(usr);
                    status = "incluir";

                    habitilitar(true);
                    Session.Remove("ObjEmbaladora" + urlSessao());
                    Session.Add("ObjEmbaladora" + urlSessao(), obj);
                }
            }
            else
            {
                if (Request.Params["id"] != null)  // colocar o campo index da tabela
                {
                    try
                    {
                        if (!IsPostBack)
                        {
                            int index = Funcoes.intTry(Request.Params["id"].ToString());// colocar o campo index da tabela
                            status = "visualizar";
                            EmbaladoraDAO obj = new EmbaladoraDAO(index, usr);
                            Session.Remove("ObjEmbaladora" + urlSessao());
                            Session.Add("ObjEmbaladora" + urlSessao(), obj);
                            carregarDados();
                        }
                        if (status.Equals("visualizar"))
                        {
                            habitilitar(false);

                        }
                        else
                        {
                            habitilitar(true);

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



        private void limparCampos()
        {

            LimparCampos(divConteudo);
        }

        protected bool validaCamposObrigatorios()
        {
            if (validaCampos(divConteudo))
                return true;
            else
                return false;
        }

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array
            return !campo.ID.Equals("txtID");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array
            String[] campos = { "txtID",
                                    "",
                                    "",
                                    ""
                                     };
            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("EmbaladoraDetalhes.aspx?novo=true");
        }
        protected void habitilitar(bool enable)
        {

            EnabledControls(divConteudo, enable);

        }
        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            editar(pnBtn);
            carregarDados();
            habitilitar(true);
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Embaladora.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            modalExcluir.Show();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaCamposObrigatorios())
                {

                    carregarDadosObj();
                    EmbaladoraDAO obj = (EmbaladoraDAO)Session["ObjEmbaladora" + urlSessao()];
                    obj.salvar(status.Equals("incluir"));
                    msgShow("Salvo Com Sucesso", false);
                    EnabledControls(divConteudo, false);
                    visualizar(pnBtn);

                }
                else
                {
                    msgShow("Campo Obrigatorio não preenchido", true);

                }
            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
            }
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Embaladora.aspx");//colocar endereco pagina de pesquisa
        }

        //--Atualizar DaoForm 
        private void carregarDados()
        {
            EmbaladoraDAO obj = (EmbaladoraDAO)Session["ObjEmbaladora" + urlSessao()];
            txtID.Text = obj.ID.ToString();
            txtDescricao.Text = obj.Descricao;
            txtUsuarioFtp.Text = obj.Usuario;
            txtSenhaFtp.Text = obj.Senha;
            txtEnderecoFtp.Text = obj.End_FTP;

        }


        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            EmbaladoraDAO obj = (EmbaladoraDAO)Session["ObjEmbaladora" + urlSessao()];
            obj.ID = Funcoes.intTry(txtID.Text);
            obj.Descricao = txtDescricao.Text;
            obj.Usuario = txtUsuarioFtp.Text;
            obj.Senha = txtSenhaFtp.Text;
            obj.End_FTP = txtEnderecoFtp.Text;
            Session.Remove("ObjEmbaladora" + urlSessao());
            Session.Add("ObjEmbaladora" + urlSessao(), obj);
        }




        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                EmbaladoraDAO obj = (EmbaladoraDAO)Session["ObjEmbaladora" + urlSessao()];
                obj.excluir();
                modalExcluir.Hide();
                msgShow("Registro Excluido com sucesso", false);
                limparCampos();
                Session.Remove("ObjEmbaladora");
                pesquisar(pnBtn);
            }
            catch (Exception err)
            {
                msgShow("Não foi possivel Excluiro registro error:" + err.Message, true);
            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            modalExcluir.Hide();
        }


        protected void exibeLista()
        {

            //User usr = (User)Session["User"];
            //String or = (String)Session["camporecebe" + urlSessao()];
            //String sqlLista = "";


            //switch (or)
            //{
            //    case "Link":
            //        sqlLista = "Select Link= '['+name+']' from sys.servers where Name like '%" + TxtPesquisaLista.Text + "%'  ";
            //        lbllista.Text = "Linked Servers";
            //        break;

            //}


            //GridLista.DataSource = Conexao.GetTable(sqlLista, null, false);
            //GridLista.DataBind();
            //if (GridLista.Rows.Count == 1)
            //{
            //    if (!GridLista.Rows[0].Cells[1].Text.Equals("------"))
            //    {
            //        RadioButton rdo = (RadioButton)GridLista.Rows[0].FindControl("RdoListaItem");
            //        rdo.Checked = true;
            //    }
            //}
            //TxtPesquisaLista.Focus();

            //modalPnFundo.Show();



        }

        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }

        protected void btnFechar_Click(object sender, ImageClickEventArgs e)
        {
            //    String selecionado = ListaSelecionada(1);

            //    if (!selecionado.Equals("") && !selecionado.Equals("------"))
            //    {

            //        String listaAtual = (String)Session["camporecebe" + urlSessao()];
            //        Session.Remove("camporecebe");

            //        if (listaAtual.Equals("Link"))
            //        {
            //            txtLinkServer.Text = ListaSelecionada(1);

            //        }

            //        modalPnFundo.Hide();
            //    }
            //    else
            //    {
            //        lblErroPesquisa.Text = "<br>Selecione Uma Opção";
            //        modalPnFundo.Show();
            //    }
        }
        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            //modalPnFundo.Hide();
        }

        protected String ListaSelecionada(int campo)
        {
            //foreach (GridViewRow item in GridLista.Rows)
            //{
            //    RadioButton rdo = (RadioButton)item.FindControl("RdoListaItem");

            //    if (rdo != null)
            //    {
            //        if (rdo.Checked)
            //        {
            //            return item.Cells[campo].Text;
            //        }
            //    }
            //}

            return "";
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
    }
}