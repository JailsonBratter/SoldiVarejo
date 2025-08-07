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
    public partial class TransportadorasDetalhes : visualSysWeb.code.PagePadrao
    {
       
        static String camporeceber = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            TransportadoraDAO obj = new TransportadoraDAO();
            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    status = "incluir";
                    Session.Remove("transportadora" + urlSessao());
                    Session.Add("transportadora" + urlSessao(), obj);
                    EnabledControls(conteudo, true);
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
                            obj = new TransportadoraDAO(index, usr);
                            Session.Remove("transportadora" + urlSessao());
                            Session.Add("transportadora" + urlSessao(), obj);
                            
                            carregarDados();
                        }
                        if (status.Equals("visualizar"))
                        {
                            EnabledControls(conteudo, false);
                          
                        }
                        else
                        {
                            EnabledControls(conteudo, true);
                         
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
            LimparCampos(conteudo);
        }

        protected bool validaCamposObrigatorios()
        {
            if (validaCampos(conteudo))
                return true;
            else
                return false;
        }

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array
            String[] campos = { "txtNome_transportadora", 
                                    "", 
                                    "", 
                                    "" 
                                     };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array
           
            if(status.Equals("editar") && campo.ID.Equals("txtNome_transportadora"))
                return true;

            String[] campos = { "", 
                                    "", 
                                    "", 
                                    "" 
                                     };
            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("TransportadorasDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            editar(pnBtn);
            EnabledControls(conteudo, true);
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Transportadoras.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaCamposObrigatorios())
                {
                     TransportadoraDAO obj = (TransportadoraDAO)Session["transportadora" + urlSessao()];
                     if (obj != null)
                     {

                         carregarDadosObj();
                         obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                         lblError.Text = "Salvo com Sucesso";
                         lblError.ForeColor = System.Drawing.Color.Blue;
                         EnabledControls(conteudo, false);
                         visualizar(pnBtn);
                     }
                     else
                     {

                         lblError.Text = "Ocorreu um Erro e não foi possivel Salvar o Registro;";
                         lblError.ForeColor = System.Drawing.Color.Red;
                
                     }

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
            Response.Redirect("Transportadoras.aspx");//colocar endereco pagina de pesquisa
        }
        private void carregarDados()
        {
            TransportadoraDAO obj = (TransportadoraDAO)Session["transportadora" + urlSessao()];
            if (obj != null)
            {
                txtNome_transportadora.Text = obj.Nome_transportadora.ToString();
                txtRazao_social.Text = obj.Razao_social.ToString();
                txtendereco.Text = obj.endereco.ToString();
                txtcidade.Text = obj.cidade.ToString();
                txtestado.Text = obj.estado.ToString();
                txtcnpj.Text = obj.cnpj.ToString();
                txtie.Text = obj.ie.ToString();
                txtPlaca.Text = obj.Placa.ToString();
                txtTelefones.Text = obj.Telefones.ToString();
                chkPadrao.Checked = obj.padrao;
            }
        }

        private void carregarDadosObj()
        {
             TransportadoraDAO obj = (TransportadoraDAO)Session["transportadora" + urlSessao()];
             if (obj != null)
             {

                 obj.Nome_transportadora = Funcoes.RemoverAcentos( txtNome_transportadora.Text);
                 obj.Razao_social = Funcoes.RemoverAcentos(txtRazao_social.Text);
                 obj.endereco = Funcoes.RemoverAcentos(txtendereco.Text);
                 obj.cidade = Funcoes.RemoverAcentos(txtcidade.Text);
                 obj.estado = txtestado.Text;
                 obj.cnpj = txtcnpj.Text;
                 obj.ie = txtie.Text;
                 obj.Placa = Funcoes.RemoverAcentos(txtPlaca.Text);
                 obj.Telefones = txtTelefones.Text;
                obj.padrao = chkPadrao.Checked;
                 Session.Remove("transportadora" + urlSessao());
                 Session.Add("transportadora" + urlSessao(), obj);
             }

        }


        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                 TransportadoraDAO obj = (TransportadoraDAO)Session["transportadora" + urlSessao()];
                 if (obj != null)
                 {
                     obj.excluir();
                     modalExcluir.Hide();
                     lblError.Text = "Registro Excluido com sucesso";
                     limparCampos();
                     pesquisar(pnBtn);
                 }
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




        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            modalExcluir.Show();
        }
      

    }
}