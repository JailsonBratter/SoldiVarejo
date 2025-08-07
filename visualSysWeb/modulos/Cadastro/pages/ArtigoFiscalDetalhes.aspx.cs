using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class ArtigoFiscalDetalhes : visualSysWeb.code.PagePadrao
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
                    ArtigoFiscalDAO obj = new ArtigoFiscalDAO();
                    status = "incluir";

                    Session.Remove("objArtigoFiscal" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("objArtigoFiscal" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
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
                            ArtigoFiscalDAO obj = new ArtigoFiscalDAO(index, usr);
                            Session.Remove("objArtigoFiscal" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                            Session.Add("objArtigoFiscal" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
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
            txtArtigoFiscal.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
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
            String[] campos = { "txtArtigoFiscal",
                                    "txtDescricao",
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
            Response.Redirect("ArtigoFiscalDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            editar(pnBtn);
            EnabledControls(cabecalho, true);
            EnabledControls(conteudo, true);
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ArtigoFiscal.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            modalExluirArtigoFiscal.Show();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaCamposObrigatorios())
                {

                    carregarDadosObj();
                    ArtigoFiscalDAO obj = (ArtigoFiscalDAO)Session["objArtigoFiscal" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
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
            Response.Redirect("ArtigoFiscal.aspx");//colocar endereco pagina de pesquisa
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
            ArtigoFiscalDAO obj = (ArtigoFiscalDAO)Session["objArtigoFiscal" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            txtArtigoFiscal.Text = string.Format("{0:0000}", obj.ArtigoFiscal);
            txtDescricao.Text = obj.Descricao.ToString();
        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            ArtigoFiscalDAO obj = (ArtigoFiscalDAO)Session["objArtigoFiscal" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            obj.ArtigoFiscal = Decimal.Parse(txtArtigoFiscal.Text);
            obj.Descricao = txtDescricao.Text;
            Session.Remove("objArtigoFiscal" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("objArtigoFiscal" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
        }




        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ArtigoFiscalDAO obj = (ArtigoFiscalDAO)Session["objArtigoFiscal" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                obj.excluir();
                modalExluirArtigoFiscal.Hide();
                lblError.Text = "Registro Excluido com sucesso";
                limparCampos();
                Session.Remove("objArtigoFiscal" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                pesquisar(pnBtn);
            }
            catch (Exception err)
            {
                lblError.Text = "Não foi possivel Excluir o registro error:" + err.Message;
            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            modalExluirArtigoFiscal.Hide();
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