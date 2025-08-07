using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using System.Drawing;
using System.Data.SqlClient;

namespace visualSysWeb.modulos.usuarios.pages
{
    public partial class UsuariosDetalhes : visualSysWeb.code.PagePadrao
    {


        protected void Page_Load(object sender, EventArgs e)
        {

          
            
            User usr = (User)Session["User"];

            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    usuarios_webDAO obj = new usuarios_webDAO();
                    status = "incluir";
                    EnabledControls(conteudo, true);
                    txtusuario.Enabled = true;
                    txtusuario.BackColor = Color.White;
                    Session.Remove("usuario" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("usuario" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);

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
                            usuarios_webDAO obj = new usuarios_webDAO(index, usr);

                            Session.Remove("usuario" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                            Session.Add("usuario" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
                            carregarDados();
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
                EnabledControls(conteudo, false);
            }
            else
            {
                EnabledControls(conteudo, true);
            }

          

           
           
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
            String[] campos = { "", 
                                    "", 
                                    "", 
                                    "" 
                                     };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array
            if (status.Equals("incluir") && campo.ID.Equals("txtusuario"))
                return false;

            String[] campos = { "txtusuario", 
                                    "txtId", 
                                    "", 
                                    "" 
                                     };
            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("usuariosDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            status = "editar";
            editar(pnBtn);
            EnabledControls(conteudo, true);
            carregarDados();

        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("usuarios.aspx"); //colocar o endereco da tela de pesquisa
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
                    usuarios_webDAO obj = (usuarios_webDAO)Session["usuario" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                    obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                    lblError.Text = "Salvo com Sucesso";
                    txtId.Text = obj.id.ToString();
                    lblError.ForeColor = System.Drawing.Color.Blue;
                    EnabledControls(conteudo, false);
                    visualizar(pnBtn);

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
            }
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("usuarios.aspx");//colocar endereco pagina de pesquisa
        }
      

        private void carregarDados()
        {
            usuarios_webDAO obj = (usuarios_webDAO)Session["usuario" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            txtId.Text = obj.id.ToString();
            txtusuario.Text = obj.usuario.ToString();
            txtnome.Text = obj.nome.ToString();
            txtsenha.Text = obj.senha.ToString();//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            txtHost.Text = obj.host;
            txtPorta.Text = obj.porta;
            txtEmail.Text = obj.email;
            txtEmailSenha.Text = obj.emailSenha;
            txtCodigo_funcionario.Text = obj.codigo_funcionario;
            txtId_operador.Text = obj.id_operador.ToString();
            txtFilial.Text = obj.filial.ToString();
            txtGruupoEmpresa.Text = obj.grupo_empresa;
            chkadm.Checked = obj.adm;

        }



        private void carregarDadosObj()
        {
            usuarios_webDAO obj = (usuarios_webDAO)Session["usuario" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            obj.id = (txtId.Text.Trim().Equals("") ? 0 : int.Parse(txtId.Text));
            obj.usuario = txtusuario.Text;
            obj.nome = txtnome.Text;
            obj.senha = txtsenha.Text;
            obj.filial = txtFilial.Text;
            obj.adm = chkadm.Checked;
            obj.host = txtHost.Text;
            obj.porta = txtPorta.Text;
            obj.email = txtEmail.Text;
            obj.emailSenha = txtEmailSenha.Text;
            int.TryParse(txtId_operador.Text, out obj.id_operador);
            obj.codigo_funcionario = txtCodigo_funcionario.Text;
            obj.grupo_empresa = txtGruupoEmpresa.Text.Trim();
            Session.Remove("usuario" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("usuario" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
        }





        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                carregarDadosObj();
                usuarios_webDAO obj = (usuarios_webDAO)Session["usuario" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                obj.excluir();
                modalExcluir.Hide();
                lblError.Text = "Registro Excluido com sucesso";
                limparCampos();
                pesquisar(pnBtn);
            }
            catch (Exception err)
            {
                lblError.Text = "Não foi possivel Excluir o registro error:" + err.Message;
            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            modalExcluir.Hide();
        }

        protected void lista_click(object sender, ImageClickEventArgs e)
        {
          
            ImageButton btn = (ImageButton)sender;

            TxtPesquisaLista.Text = "";

            switch (btn.ID)
            {
                case "imgBtnCodigo_funcionario":
                    Session.Add("camporecebe" + urlSessao(), "txtCodigo_funcionario");
                    break;
                case "imgBtnId_operador":
                    Session.Add("camporecebe" + urlSessao(), "TxtId_operador");
                    break;
                case "btnFilial":
                    Session.Add("camporecebe" + urlSessao(), "txtFilial");
                    break;
                case "imgListaGrupoEmpresa":
                    Session.Add("camporecebe" + urlSessao(), "txtGruupoEmpresa");
                    break;

            }

            exibeLista();

            
        
        }
        private void exibeLista()
        {
            User usr = (User)Session["User"];
            String sqlLista = "";
            String campo = (String)Session["camporecebe" + urlSessao()];
            switch (campo)
            {
                case "txtCodigo_funcionario":
                    lbllista.Text = "Escolha o Funcionario";
                    
                    sqlLista = "Select codigo, nome, Funcao from Funcionario where codigo='" + TxtPesquisaLista.Text + "' or nome like '%" + TxtPesquisaLista.Text + "%' or funcao like '" + TxtPesquisaLista.Text + "%'";
                    break;
                case "TxtId_operador":
                    lbllista.Text = "Escolha o Operador";
                   
                    sqlLista = "Select ID_Operador,Nome from Operadores where nome like '%" + TxtPesquisaLista.Text + "%' "+ (!TxtPesquisaLista.Text.Equals("")? "or  id_operador = " + TxtPesquisaLista.Text :"")  ;
                  
                    break;
                case "txtFilial":
                     sqlLista = "select Filial from filial";
                    lbllista.Text = "Escolha a Filial";

                    break;
                case "txtGruupoEmpresa":
                    sqlLista = "select id, grupo FROM cliente_grupo ORDER BY id";
                    lbllista.Text = "Escolha o GRUPO";

                    break;



            }


            GridLista.DataSource = Conexao.GetTable(sqlLista, usr, false);
            GridLista.DataBind();
            modalPnFundo.Show();
        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalPnFundo.Hide();
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
            String campo = (String)Session["camporecebe" + urlSessao()];
            switch (campo)
            {
                case "txtCodigo_funcionario":
                    txtCodigo_funcionario.Text = ListaSelecionada(1);
                    break;
                case "TxtId_operador":
                    txtId_operador.Text = ListaSelecionada(1);
                    break;
                case "txtFilial":
                    txtFilial.Text = ListaSelecionada(1);
                    break;
                case "txtGruupoEmpresa":
                    txtGruupoEmpresa.Text = ListaSelecionada(1);
                    break;
            }

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
                return;
            }
            string script = "SetUniqueRadioButton('GridLista.*GrlistaItem',this)";
            rdo.Attributes.Add("onclick", script);
        }


    }
}