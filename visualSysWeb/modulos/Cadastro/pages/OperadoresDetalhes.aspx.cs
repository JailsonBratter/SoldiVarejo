using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using System.Data.SqlClient;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class OperadoresDetalhes : visualSysWeb.code.PagePadrao
    {
        static String camporeceber = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    OperadoresDAO obj = new OperadoresDAO(usr);
                    Session.Remove("operador" + urlSessao());
                    Session.Add("operador" + urlSessao(), obj);
                    status = "incluir";
                    habilitar(true);
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
                            OperadoresDAO obj = new OperadoresDAO(index, usr);
                            Session.Remove("operador" + urlSessao());
                            Session.Add("operador" + urlSessao(), obj);
                            carregarDados();
                        }
                        if (status.Equals("visualizar"))
                        {
                            habilitar(false);
                        }
                        else
                        {
                            habilitar(true);
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

        private void limparCampos()
        {
            LimparCampos(cabecalho);
            LimparCampos(conteudo);
        }
        private void habilitar(bool enable)
        {
            EnabledControls(conteudo, enable);
            EnabledControls(cabecalho, enable);
        }
        protected bool validaCamposObrigatorios()
        {

            if (status.Equals("incluir") && !txtID_Operador.Text.Equals(""))
            {   int count = 0;
                int.TryParse(Conexao.retornaUmValor("Select count(id_operador) from operadores  where id_operador ="+txtID_Operador.Text, null), out count);
                if (count > 0)
                {
                    txtID_Operador.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Codigo de Operador já Existe");
                }
            }



            if (validaCampos(cabecalho) && validaCampos(conteudo))
                return true;
            else
                return false;
        }

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array
            String[] campos = {     "txtNome", 
                                    "txtCargo",
                                    (status.Equals("incluir")?"txtSenha":"")
                                     };
            

            return existeNoArray(campos, campo.ID + "");


        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array

            if (!status.Equals("incluir") && (campo.ID.Equals("ImgOperador")))
            {
                return true;
            }
            String[] campos = { "txtID_Operador", 
                                    "", 
                                    "", 
                                    "" 
                                     };

            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("OperadoresDetalhes.aspx?novo=true"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            editar(pnBtn);
            EnabledControls(cabecalho, true);
            EnabledControls(conteudo, true);
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Operadores.aspx"); //colocar o endereco da tela de pesquisa
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

                    OperadoresDAO obj = (OperadoresDAO)Session["operador" + urlSessao()];
                    obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;
                    habilitar(false);

                    visualizar(pnBtn);
                    carregarDados();
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
            Response.Redirect("Operadores.aspx");//colocar endereco pagina de pesquisa
        }

        //--Atualizar DaoForm 
        private void carregarDados()
        {
            OperadoresDAO obj = (OperadoresDAO)Session["operador" + urlSessao()];
            txtID_Operador.Text = obj.ID_Operador.ToString();
            txtNome.Text = obj.Nome.ToString();
            txtSenha.Text = obj.Senha.ToString();
            ddlNivel.SelectedValue = obj.ID_NivelAcesso.ToString();
            //txtID_NivelAcesso.Text = obj.ID_NivelAcesso.ToString();
            txtCargo.Text = obj.Cargo.ToString();
            chkOpCaixa.Checked = obj.OpCaixa;

        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            OperadoresDAO obj = (OperadoresDAO)Session["operador" + urlSessao()];

            int.TryParse(txtID_Operador.Text,out obj.ID_Operador);
            
            
            obj.Nome = txtNome.Text;
            obj.Senha = txtSenha.Text;
            obj.ID_NivelAcesso = Funcoes.intTry(ddlNivel.SelectedItem.Value);
            obj.Cargo = txtCargo.Text;
            obj.OpCaixa = chkOpCaixa.Checked;
            Session.Remove("operador" + urlSessao());
            Session.Add("operador" + urlSessao(), obj);

        }




        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                OperadoresDAO obj = (OperadoresDAO)Session["operador" + urlSessao()];
                obj.excluir();
                lblError.Text = "Registro Excluido com sucesso";
                limparCampos();
                pesquisar(pnBtn);
                modalPnConfirma.Hide();
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

        protected void ImgOperador_Click(object sender, ImageClickEventArgs e)
        {
            txtID_Operador.Text = OperadoresDAO.proximoCodigo().ToString();
        }
    }
}