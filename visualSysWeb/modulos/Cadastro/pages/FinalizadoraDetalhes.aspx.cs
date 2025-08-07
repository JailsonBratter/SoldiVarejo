using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class FinalizadoraDetalhes : visualSysWeb.code.PagePadrao
    {
        protected static finalizadoraDAO obj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
           
            
            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    obj = new finalizadoraDAO();
                    status = "incluir";

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
                            obj = new finalizadoraDAO(index, usr);
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
        }
        private void camposnumericos()
        {
            ArrayList campos = new ArrayList();
            campos.Add("txtTroco");
            FormataCamposNumericos(campos, conteudo);
            FormataCamposNumericos(campos, cabecalho);

            ArrayList camposInteiros = new ArrayList();
            camposInteiros.Add("txtNro_Finalizadora");
            camposInteiros.Add("txtCodigo_centro_custo");
            camposInteiros.Add("txtTecla");
            camposInteiros.Add("txtEcf");

            FormataCamposInteiros(camposInteiros, conteudo);
            FormataCamposInteiros(camposInteiros, cabecalho);

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
        {// TODOS OS CAMPOS SÃO OBRIGATORIOS
            if (!campo.ID.Equals("chkNaoComputa"))
                return true;
            else
                return false;
        }
        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array
            String[] campos = { "txtPagamento", 
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
            Response.Redirect("Finalizadoras.aspx"); //colocar o endereco da tela de pesquisa
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
                    obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;
                    EnabledControls(conteudo, false);
                    EnabledControls(cabecalho, false);
                    visualizar(pnBtn);
                    status = "visualizar";
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
            Response.Redirect("Finalizadoras.aspx");//colocar endereco pagina de pesquisa
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

        private void carregarDados()
        {
            txtNro_Finalizadora.Text = obj.Nro_Finalizadora.ToString();
            txtCodigo_centro_custo.Text = obj.Codigo_centro_custo.ToString();
            txtPagamento.Text = obj.Pagamento.ToString();
            txtTroco.Text = obj.Troco.ToString();
            txtFinalizadora.Text = obj.Finalizadora.ToString();
            txtTecla.Text = obj.Tecla.ToString();
            txtEcf.Text = obj.Ecf.ToString();
            Conexao.preencherDDL1Branco(ddlAutorizadora, "Select * from Autorizadora", "descricao", "id", null);
            ddlAutorizadora.SelectedValue = obj.id_Autorizadora;

        }

        private void carregarDadosObj()
        {//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            obj.Nro_Finalizadora = int.Parse(txtNro_Finalizadora.Text);
            User usr = (User)Session["User"];
            obj.filial = usr.getFilial();
            obj.Codigo_centro_custo = txtCodigo_centro_custo.Text;
            obj.Pagamento = txtPagamento.Text;
            obj.Troco = txtTroco.Text;
            obj.Finalizadora = txtFinalizadora.Text;
            obj.Tecla = int.Parse(txtTecla.Text);
            obj.Ecf = int.Parse(txtEcf.Text);
            obj.id_Autorizadora = ddlAutorizadora.SelectedValue;
        }

        protected void lista_click(object sender, ImageClickEventArgs e)
        {

            ImageButton btn = (ImageButton)sender;

            TxtPesquisaLista.Text = "";
            switch (btn.ID)
            {
                case "bntPagamento":
                    Session.Add("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "txtPagamento");
                    break;
                case "btnCentroCusto":
                    Session.Add("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "txtCodigo_centro_custo");
                    break;
            }
            exibeLista();
        }


        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                obj.excluir();
                modalPnConfirma.Hide();
                lblError.Text = "Registro Excluido com sucesso";
                limparCampos();
                pesquisar(pnBtn);
            }
            catch (Exception err)
            {

                lblError.Text = "Não foi possivel Excluir o registro pelo error:" + err.Message;
            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            modalPnConfirma.Hide();
        }



        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalPnFundo.Hide();
        }

        protected void exibeLista()
        {
            lblErroPesquisa.Text = "";
            User usr = (User)Session["User"];
            String or = (String)Session["camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            String sqlLista = "";


            switch (or)
            {
                case "txtPagamento":

                    sqlLista = "Select pagamento from pagamento where pagamento like '%" + TxtPesquisaLista.Text + "%'";
                    lbllista.Text = "Pagamentos";
                    break;
                case "txtCodigo_centro_custo":
                    sqlLista = "select codigo_centro_custo,descricao_centro_custo from centro_custo where (codigo_centro_custo like '" + TxtPesquisaLista.Text + "%' )or descricao_centro_custo like '%" + TxtPesquisaLista.Text + "%'";
                    lbllista.Text = "Centro de Custo";
                    break;

            }
            GridLista.DataSource = Conexao.GetTable(sqlLista, usr, false);
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
            modalPnFundo.Show();
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
                String listaAtual = (String)Session["camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                Session.Remove("camporecebe" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));

                if (listaAtual.Equals("txtPagamento"))
                {

                    txtPagamento.Text = ListaSelecionada(1);
                }
                else if (listaAtual.Equals("txtCodigo_centro_custo"))
                {
                    txtCodigo_centro_custo.Text = ListaSelecionada(1);
                }


                modalPnFundo.Hide();
            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                modalPnFundo.Show();
            }

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




        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }



    }

}
