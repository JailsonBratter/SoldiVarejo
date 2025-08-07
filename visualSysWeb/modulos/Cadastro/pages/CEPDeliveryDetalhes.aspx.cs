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
    public partial class CEPDeliveryDetalhes : visualSysWeb.code.PagePadrao
    {

        //static String camporeceber = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];


            if (Request.Params["novo"] != null)
            {



                if (!IsPostBack)
                {
                    status = "incluir";
                    CEP_Brasil_DeliveryDAO obj = new CEP_Brasil_DeliveryDAO();
                    Session.Remove("obj" + urlSessao());
                    Session.Add("obj" + urlSessao(), obj);
                    EnabledControls(cabecalho, true);
                }

            }
            else
            {
                if (Request.Params["cep"] != null)  // colocar o campo index da tabela
                {
                    try
                    {
                        if (!IsPostBack)
                        {
                            String index = Request.Params["cep"].ToString();// colocar o campo index da tabela
                            status = "visualizar";
                            CEP_Brasil_DeliveryDAO obj = new CEP_Brasil_DeliveryDAO(index, usr);
                            Session.Remove("obj" + urlSessao());
                            Session.Add("obj" + urlSessao(), obj);
                            carregarDados();
                        }
                        if (status.Equals("visualizar"))
                        {
                            //EnabledControls(conteudo, false);
                            EnabledControls(cabecalho, false);
                        }
                        else
                        {
                            //EnabledControls(conteudo, true);
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
        }

        private void limparCampos()
        {
            LimparCampos(cabecalho);

        }

        protected bool validaCamposObrigatorios()
        {
            if (validaCampos(cabecalho))
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
            String[] campos = { "", 
                                    "", 
                                    "", 
                                    "" 
                                     };
            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("CEPDeliveryDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            editar(pnBtn);
            EnabledControls(cabecalho, true);

        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("CEPDelivery.aspx"); //colocar o endereco da tela de pesquisa
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
                    CEP_Brasil_DeliveryDAO obj = (CEP_Brasil_DeliveryDAO)Session["obj" + urlSessao()];
                    obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;
                    EnabledControls(cabecalho, false);
                    //EnabledControls(conteudo, false);
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
            Response.Redirect("CEPDelivery.aspx");//colocar endereco pagina de pesquisa
        }

        //--Atualizar DaoForm 
        private void carregarDados()
        {
            CEP_Brasil_DeliveryDAO obj = (CEP_Brasil_DeliveryDAO)Session["obj" + urlSessao()];
            txtLogradouro.Text = obj.Logradouro.ToString();
            txtBairro.Text = obj.Bairro.ToString();
            txtCidade.Text = obj.Cidade.ToString();
            txtUF.Text = obj.UF.ToString();
            txtCEP.Text = obj.CEP.ToString();
            txtnum_inicio.Text = obj.num_inicio.ToString();
            txtnum_fim.Text = obj.num_fim.ToString();
        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            CEP_Brasil_DeliveryDAO obj = (CEP_Brasil_DeliveryDAO)Session["obj" + urlSessao()];
            obj.Logradouro = txtLogradouro.Text;
            obj.Bairro = txtBairro.Text;
            obj.Cidade = txtCidade.Text;
            obj.UF = txtUF.Text;
            obj.CEP = txtCEP.Text;
            obj.num_inicio = int.Parse(txtnum_inicio.Text);
            obj.num_fim = int.Parse(txtnum_fim.Text);
            Session.Remove("obj" + urlSessao());
            Session.Add("obj" + urlSessao(), obj);
        }


       

        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                CEP_Brasil_DeliveryDAO obj = (CEP_Brasil_DeliveryDAO)Session["obj" + urlSessao()];
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
        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            modalExcluir.Hide();
        }


        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            pnFundo.Visible = false;
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
            
            lblErroPesquisa.Text = "";
            String sqlLista = "";
            String or = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            switch (or)
            {
                
                case "txtCEP":
                    lbllista.Text = "Escolha o CEP";
                    sqlLista = "select top 500 * from CEP_Brasil where (cep+Logradouro+Bairro+Cidade+UF) like '%" + TxtPesquisaLista.Text.Replace(" ", "%") + "%'";
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
                String listaAtual = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                Session.Remove("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));

              
                 if (listaAtual.Equals("txtCEP"))
                {
                    txtCEP.Text = ListaSelecionada(5);
                    txtLogradouro.Text = ListaSelecionada(1);
                    txtBairro.Text = ListaSelecionada(2);
                    txtCidade.Text = ListaSelecionada(3);
                    txtUF.Text = ListaSelecionada(4);
                    txtnum_inicio.Focus();
                }




                ModalFundo.Hide();
            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                ModalFundo.Show();
            }
        }
        protected void txtCEP_TextChanged(object sender, EventArgs e)
        {
            if (!txtCEP.Text.Equals(""))
            {

                String strSql = "Select * from CEP_Brasil where CEP='"+txtCEP.Text+"'";
                User usr = (User)Session["User"];
                SqlDataReader rs = null;
                try
                {
                    rs = Conexao.consulta(strSql, usr, false);
                    if (rs.Read())
                    {
                        txtLogradouro.Text = rs["Logradouro"].ToString();
                        txtBairro.Text = rs["bairro"].ToString();
                        txtCidade.Text = rs["cidade"].ToString();
                        txtUF.Text = rs["uf"].ToString();
                        txtnum_inicio.Focus();
                    }
                }
                catch (Exception err)
                {

                    lblError.Text = err.Message;
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
                finally
                {
                    if (rs != null)
                        rs.Close();
                }


            }
        }

    }
}