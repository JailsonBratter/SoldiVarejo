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
    public partial class CFOPDetalhes : visualSysWeb.code.PagePadrao
    {
        
        static String camporeceber = "";
        protected void Page_Load(object sender, EventArgs e)
        {


            User usr = (User)Session["User"];
            tabMenu.Items[MultiView1.ActiveViewIndex].Selected = true;
            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    CFOPDAO obj = new CFOPDAO();
                    status = "incluir";
                  
                    Session.Remove("objCFOP" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("objCFOP" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
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
                            CFOPDAO obj = new CFOPDAO(index, usr);
                            Session.Remove("objCFOP" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                            Session.Add("objCFOP" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
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
                EnabledControls(cabecalho, false);
            }
            else
            {
                EnabledControls(conteudo, true);
                EnabledControls(cabecalho, true);
            }
            carregabtn(pnBtn);
            camposnumericos();
        }

          private void camposnumericos()
        {
            txtCFOP.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
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
            String[] campos = { "txtCFOP", 
                                    "txtDESCRICAO", 
                                    "chkTIPO", 
                                    "" 
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
            Response.Redirect("CFOPDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            editar(pnBtn);
            EnabledControls(cabecalho, true);
            EnabledControls(conteudo, true);
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("CFOP.aspx"); //colocar o endereco da tela de pesquisa
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
                    CFOPDAO obj = (CFOPDAO)Session["objCFOP" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                    obj.salvar(status.Equals("incluir")); 
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;
                    EnabledControls(cabecalho, false);
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
            Response.Redirect("CFOP.aspx");//colocar endereco pagina de pesquisa
        }
        protected void tabMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            switch (e.Item.Value)
            {
                case "tab1":
                    MultiView1.ActiveViewIndex = 0;
                    break;
            }
        }
        //--Atualizar DaoForm 
        private void carregarDados()
        {
            CFOPDAO obj = (CFOPDAO)Session["objCFOP" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            txtCFOP.Text = string.Format("{0:0000}", obj.CFOP);
            txtDESCRICAO.Text = obj.DESCRICAO.ToString();
            ddlTipo.SelectedValue = obj.TIPO.ToString();
        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            CFOPDAO obj = (CFOPDAO)Session["objCFOP" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            obj.CFOP = Decimal.Parse(txtCFOP.Text);
            obj.DESCRICAO = txtDESCRICAO.Text;
            obj.TIPO = int.Parse(ddlTipo.SelectedValue);
            Session.Remove("objCFOP" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("objCFOP" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
        }


       

        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                CFOPDAO obj = (CFOPDAO)Session["objCFOP" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                obj.excluir();
                modalExluirCFOP.Hide();
                lblError.Text = "Registro Excluido com sucesso";
                limparCampos();
                Session.Remove("objCFOP" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                pesquisar(pnBtn);
            }
            catch (Exception err)
            {
                lblError.Text = "Não foi possivel Excluir o registro error:" + err.Message;
            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            modalExluirCFOP.Hide();
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
                  
    }
}