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
    public partial class CartaoDetalhes : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            cartaoDAO obj ;
            if (Request.Params["novo"] != null)
            {
                status = "incluir";
                EnabledControls(cabecalho, true);
                obj = new cartaoDAO(usr);
                Session.Remove("cartao" + urlSessao());
                Session.Add("cartao" + urlSessao(), obj);
            }
            else
            {
                if (Request.Params["id_cartao"] != null)  // colocar o campo index da tabela
                {
                    try
                    {
                        if (!IsPostBack)
                        {
                            String idCartao = Request.Params["id_cartao"].ToString();// colocar o campo index da tabela
                            String nroFinalizadora = Request.Params["nro_finalizadora"].ToString();
                            status = "visualizar";
                            obj = new cartaoDAO(nroFinalizadora,idCartao, usr);

                            Session.Remove("cartao" + urlSessao());
                            Session.Add("cartao" + urlSessao(), obj);
                            carregarDados();
                        }
                        if (status.Equals("visualizar"))
                        {
                            EnabledControls(cabecalho, false);
                        }
                        else
                        {
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
            if (validaCampos(cabecalho) )
                return true;
            else
                return false;
        }

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array
            String[] campos = { "txtid_cartao", 
                                    "txtnro_Finalizadora", 
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
            Response.Redirect("cartaoDetalhes.aspx?novo=true"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            editar(pnBtn);
            EnabledControls(cabecalho, true);
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("cartao.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            //pnConfima.Visible = true;
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaCamposObrigatorios())
                {

                    carregarDadosObj();
                    cartaoDAO obj = (cartaoDAO)Session["cartao" + urlSessao()];
                    obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;
                    EnabledControls(cabecalho, false);
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
            Response.Redirect("cartao.aspx");//colocar endereco pagina de pesquisa
        }
      
        //--Atualizar DaoForm 
        private void carregarDados()
        {
            cartaoDAO obj = (cartaoDAO)Session["cartao" + urlSessao()];
            txtnro_Finalizadora.Text = obj.nro_Finalizadora.ToString();
            txtid_cartao.Text = obj.id_cartao.ToString();
            txtdias.Text = obj.dias.ToString();
            txttaxa.Text = string.Format("{0:0,0.00}", obj.taxa);
            txtdata.Text = obj.data.ToString();
            txtcentro_custo.Text = obj.centro_custo.ToString();
            txtdespesa_cc.Text = obj.despesa_cc.ToString();
            txtfornecedor.Text = obj.fornecedor.ToString();
            //txtajuste_cc.Text = obj.ajuste_cc.ToString();
            txtbandeira.Text = obj.bandeira.ToString();
            txtacrecimo.Text = string.Format("{0:0,0.00}", obj.acrecimo);
            txtid_Bandeira.Text = obj.id_Bandeira.ToString();
            txtid_Rede.Text = obj.id_Rede.ToString();
        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            cartaoDAO obj = (cartaoDAO)Session["cartao" + urlSessao()];
            obj.nro_Finalizadora = int.Parse(txtnro_Finalizadora.Text);
            obj.id_cartao = txtid_cartao.Text;
            obj.dias = int.Parse(txtdias.Text);
            obj.taxa = Decimal.Parse(txttaxa.Text);
            obj.data = int.Parse(txtdata.Text);
            obj.centro_custo = txtcentro_custo.Text;
            obj.despesa_cc = txtdespesa_cc.Text;
            obj.fornecedor = txtfornecedor.Text;
            //obj.ajuste_cc = txtajuste_cc.Text;
            obj.bandeira = txtbandeira.Text;
            obj.acrecimo = (txtacrecimo.Text.Equals("") ? 0 : Decimal.Parse(txtacrecimo.Text));
            obj.id_Bandeira = txtid_Bandeira.Text;
            obj.id_Rede = txtid_Rede.Text;
            Session.Remove("cartao" + urlSessao());
            Session.Add("cartao" + urlSessao(), obj);
                            
        }


        protected void Img_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;

            TxtPesquisaLista.Text = "";
            String or = btn.ID;
            Session.Add("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), or);
            TxtPesquisaLista.Text = "";
            exibeLista();


        }

        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            //try
            //{
            //    obj.excluir();
            //    pnConfima.Visible = false;
            //    lblError.Text = "Registro Excluido com sucesso";
            //    limparCampos();
            //    pesquisar(pnBtn);
            //}
            //catch (Exception err)
            //{
                lblError.Text = "Não foi possivel Excluir o registro error:" ;
            //}
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            //pnConfima.Visible = false;
        }

        //protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        //{
        //    TextBox txt = (TextBox)conteudo.FindControl(camporeceber);
        //    txt.Text = "";
        //    for (int i = 0; i < chkLista.Items.Count; i++)
        //    {
        //        if (chkLista.Items[i].Selected)
        //        {
        //            txt.Text += chkLista.Items[i].Value;
        //        }
        //    }
        //    pnFundo.Visible = false;
        //}


        protected void btnFechar_Click(object sender, ImageClickEventArgs e)
        {
            String selecionado = ListaSelecionada(1);

            if (!selecionado.Equals("") && !selecionado.Equals("------"))
            {
            String listaAtual = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            switch(listaAtual)
            {
                case "btnCentroCusto":
                    txtcentro_custo.Text = ListaSelecionada(1);
                    break;
                case "btnDespesasCentroCusto":
                    txtdespesa_cc.Text = ListaSelecionada(1);
                    break;
            }
            Session.Remove("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));

          
            modalPnFundo.Hide();
            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                modalPnFundo.Show();
            }
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

        protected void exibeLista()
        {
            lblErroPesquisa.Text = "";
            User usr = (User)Session["User"];
            String or = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            String sqlLista = "";


            switch (or)
            {
                case "btnCentroCusto":
                case "btnDespesasCentroCusto":
                    lbllista.Text = "Escolha um Centro Custo";
                    sqlLista = "Select codigo_centro_custo codigo ,descricao_centro_custo Descrição from centro_custo where descricao_centro_custo  like '%" + TxtPesquisaLista.Text + "%'";

                    break;
              

            }



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
            modalPnFundo.Show();
            TxtPesquisaLista.Focus();

        }
        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalPnFundo.Hide();
            Session.Remove("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            

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

        protected void GridLista_SelectedIndexChanged(object sender, EventArgs e)
        {


        }
    }
}