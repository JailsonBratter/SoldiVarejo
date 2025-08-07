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
    public partial class LinhasDetalhes : visualSysWeb.code.PagePadrao
    {

        static String camporeceber = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            linhaDAO obj;

            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    obj = new linhaDAO();
                    status = "incluir";
                    EnabledControls(conteudo, true);
                    EnabledControls(cabecalho, true);
                    Session.Remove("linha" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("linha" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
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
                            obj = new linhaDAO(index, usr);
                            Session.Remove("linha" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                            Session.Add("linha" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);

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
            camposnumericos();
        }
        private void camposnumericos()
        {
            String[] campos = { "txtcodigo_linha", 
                                "txtcodigo_cor"
                              };

            foreach (String item in campos)
            {
                TextBox txt = (TextBox)cabecalho.FindControl(item);
                if (txt == null)
                    txt = (TextBox)conteudo.FindControl(item);

                if (txt != null)
                    txt.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");

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
            String[] campos = { "txtdescricao_linha", 
                                    "txtcodigo_linha", 
                                    "", 
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
            incluir(pnBtn);
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            editar(pnBtn);
            EnabledControls(cabecalho, true);
            EnabledControls(conteudo, true);
            carregarDados();
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Linhas.aspx"); //colocar o endereco da tela de pesquisa
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
                {//Sinto Muito Me Perdoe Agradeço Eu Te Amo.

                    carregarDadosObj();
                    linhaDAO obj = (linhaDAO)Session["linha" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                    obj.salvar(status.Equals("incluir")); ;
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;
                    EnabledControls(cabecalho, false);
                    EnabledControls(conteudo, false);
                    visualizar(pnBtn);
                    carregarDados();
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
            Response.Redirect("Linhas.aspx");
        }


        private void carregarDados()
        {
            linhaDAO obj = (linhaDAO)Session["linha" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            txtcodigo_linha.Text = obj.codigo_linha.ToString();
            txtdescricao_linha.Text = obj.descricao_linha.ToString();
            gridCores.DataSource = obj.CoresLinha();
            gridCores.DataBind();

        }


        private void carregarDadosObj()
        {
            linhaDAO obj = (linhaDAO)Session["linha" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            int.TryParse(txtcodigo_linha.Text, out obj.codigo_linha);
            obj.descricao_linha = txtdescricao_linha.Text;

            Session.Remove("linha");
            Session.Add("linha", obj);
        }




        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                linhaDAO obj = (linhaDAO)Session["linha" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
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

        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
        }

        protected void gridCores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            linhaDAO linha = (linhaDAO)Session["linha" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            linha.removeCor(index);
            carregarDados();
        }



        protected void btnAddCor_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                txtcodigo_linha.BackColor = System.Drawing.Color.White;
                txtdescricao_linha.BackColor = System.Drawing.Color.White;
                txtcodigo_cor.BackColor = System.Drawing.Color.White;


                if (txtcodigo_linha.Text.Equals(""))
                {
                    txtcodigo_linha.BackColor = System.Drawing.Color.Red;

                    throw new Exception("Informe o Codigo da Linha");
                }

                if (!txtcodigo_cor.Text.Trim().Equals("") && !txtdescricao_cor.Text.Trim().Equals(""))
                {
                    carregarDadosObj();
                    cor_linhaDAO cor = new cor_linhaDAO();
                    cor.codigo_cor = int.Parse(txtcodigo_cor.Text);
                    cor.descricao_cor = txtdescricao_cor.Text;
                    txtcodigo_cor.Text = "";
                    txtdescricao_cor.Text = "";
                    linhaDAO linha = (linhaDAO)Session["linha" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                    linha.addCor(cor);
                    Session.Remove("linha" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("linha" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), linha);
                    carregarDados();
                }
                else
                {
                    if (txtcodigo_cor.Text.Trim().Equals(""))
                    {
                        txtcodigo_cor.BackColor = System.Drawing.Color.Red;
                    }

                    if(txtdescricao_cor.Text.Trim().Equals(""))
                    {
                        txtdescricao_linha.BackColor = System.Drawing.Color.Red;
                    }

                    throw new Exception("Preencha o codito e a Descrição da cor para poder inserir");


                }

            }
            catch (Exception err)
            {

                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}