using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using System.Data.SqlClient;
using System.Collections;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class PDVDetalhes : visualSysWeb.code.PagePadrao
    {

        static String camporeceber = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            User usr = (User)Session["User"];
           
            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    PDVDAO obj = new PDVDAO(usr);
                    status = "incluir";
                    
                    habitilitar(true);
                    Session.Remove("objPDV");
                    Session.Add("objPDV", obj);
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
                            int index = int.Parse(Request.Params["campoIndex"].ToString());// colocar o campo index da tabela
                            status = "visualizar";
                            PDVDAO obj = new PDVDAO(index, usr);
                            Session.Remove("objPDV");
                            Session.Add("objPDV", obj);
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
                        lblError.Text = err.Message;
                    }
                }
            }
            carregabtn(pnBtn);
            camposnumericos();
        }
        private void camposnumericos()
        {
            ArrayList campos = new ArrayList();
            campos.Add("");
            FormataCamposNumericos(campos, conteudo);
            FormataCamposNumericos(campos, cabecalho);

            ArrayList camposInteiros = new ArrayList();
            camposInteiros.Add("txtPDV");
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
        {// colocar os nomes dos campos obrigarios no Array
            String[] campos = { "txtPDV"};
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array
            String[] campos = { "txtUltimaAtualizacao", 
                                    "txtUltimaIntegracaoVendas", 
                                    "", 
                                    "" 
                                     };
            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("PDVDetalhes.aspx?novo=true");
        }
        protected  void habitilitar(bool enable)
        {
            EnabledControls(cabecalho, enable);
            EnabledControls(conteudo, enable);

            if (enable)
            {
                if (rdoLinkServer.Checked)
                {
                    txtLinkServer.Enabled = true;
                    txtDiretorio_Carga.Enabled = false;
                    imgBtnLink.Visible = true;
                    txtDiretorio_Carga.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                }
                else if (rdoDiretorioCarga.Checked)
                {
                    txtDiretorio_Carga.Enabled = true;
                    txtLinkServer.Enabled = false;
                    imgBtnLink.Visible = false;
                    txtLinkServer.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                }
                else if (rdoCargaServico.Checked)
                {
                    txtDiretorio_Carga.Enabled = false;
                    txtLinkServer.Enabled = false;
                    imgBtnLink.Visible = false;
                    txtLinkServer.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                    txtDiretorio_Carga.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                }
            }

        }
        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            editar(pnBtn);
            carregarDados();
            habitilitar(true);
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("PDV.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            modalExluirPDV.Show();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaCamposObrigatorios())
                {

                    carregarDadosObj();
                    PDVDAO obj = (PDVDAO)Session["objPDV"];
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
            Response.Redirect("PDV.aspx");//colocar endereco pagina de pesquisa
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
            PDVDAO obj = (PDVDAO)Session["objPDV"];
            txtPDV.Text = string.Format("{0:0}", obj.PDV);
            txtModelo.Text = obj.Modelo.ToString();
            txtNumeroSerie.Text = obj.NumeroSerie.ToString();
            txtDiretorio_Carga.Text = obj.Diretorio_Carga.ToString();
            chkSat.Checked = obj.sat;
            txtLinkServer.Text = obj.Link_server;
            txtUltimaAtualizacao.Text = obj.getDataBrDataUltAtualizacao;
            txtConnectionString.Text = obj.ConnectionString;
            chkCargaAutomatica.Checked = obj.Carga_Automatica;
            chkIntegracaoVendasAutomatica.Checked = obj.Integracao_Vendas_automatica;
            txtUltimaIntegracaoVendas.Text = Funcoes.dataBr(obj.Data_ultima_Integracao_Vendas);

            if (obj.ativa_link_server == 0)
            {
                rdoDiretorioCarga.Checked = true;

            }
            else
            if (obj.ativa_link_server == 1)
            {
                rdoLinkServer.Checked = true;
            }
            else if(obj.ativa_link_server == 2)
            {
                rdoCargaServico.Checked = true;
            }
            
        }

        protected void rdoTipoCarga_Change(object obj, EventArgs e)
        {
            if (rdoLinkServer.Checked)
            {
                txtLinkServer.Enabled = true;
                txtDiretorio_Carga.Enabled = false;
                imgBtnLink.Visible = true;
                txtDiretorio_Carga.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
            }
            else if(rdoDiretorioCarga.Checked)
            {
                txtDiretorio_Carga.Enabled = true;
                txtLinkServer.Enabled = false;
                imgBtnLink.Visible = false;
                txtLinkServer.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
            }
            else if (rdoCargaServico.Checked)
            {
                txtDiretorio_Carga.Enabled = false;
                txtLinkServer.Enabled = false;
                imgBtnLink.Visible = false;
                txtLinkServer.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                txtDiretorio_Carga.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
            }
        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            PDVDAO obj = (PDVDAO)Session["objPDV"];
            obj.PDV = int.Parse(txtPDV.Text);
            obj.Modelo = txtModelo.Text;
            obj.NumeroSerie = txtNumeroSerie.Text ;
            obj.Diretorio_Carga = txtDiretorio_Carga.Text;
            obj.sat = chkSat.Checked;
            obj.Link_server = txtLinkServer.Text;
            if (rdoDiretorioCarga.Checked)
            {
                obj.ativa_link_server = 0;
            }
            else if (rdoLinkServer.Checked)
            {
                obj.ativa_link_server = 1;
            }
            else if( rdoCargaServico.Checked)
            {
                obj.ativa_link_server = 2;
            }
            obj.Carga_Automatica = chkCargaAutomatica.Checked;
            obj.Integracao_Vendas_automatica = chkIntegracaoVendasAutomatica.Checked;
            Session.Remove("objPDV");
            Session.Add("objPDV", obj);
        }


       

        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                PDVDAO obj = (PDVDAO)Session["objPDV"];
                obj.excluir();
                modalExluirPDV.Hide();
                lblError.Text = "Registro Excluido com sucesso";
                limparCampos();
                Session.Remove("objPDV");
                pesquisar(pnBtn);
            }
            catch (Exception err)
            {
                lblError.Text = "Não foi possivel Excluir o registro error:" + err.Message;
            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            modalExluirPDV.Hide();
        }

       

        
        protected void Img_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            TxtPesquisaLista.Text = "";
            
            switch (btn.ID)
            {
                case "imgBtnLink":
                    Session.Add("camporecebe" + urlSessao(), "Link");
                    

                    break;
                

            }

            exibeLista();
         
        }

        protected void exibeLista()
        {
            lblErroPesquisa.Text = "";
            User usr = (User)Session["User"];
            String or = (String)Session["camporecebe" + urlSessao()];
            String sqlLista = "";


            switch (or)
            {
                case "Link":
                    sqlLista = "Select Link= '['+name+']' from sys.servers where Name like '%" + TxtPesquisaLista.Text + "%'  ";
                    lbllista.Text = "Linked Servers";
                    break;

            }


            GridLista.DataSource = Conexao.GetTable(sqlLista, null, false);
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

        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }

        protected void btnFechar_Click(object sender, ImageClickEventArgs e)
        {
            String selecionado = ListaSelecionada(1);

            if (!selecionado.Equals("") && !selecionado.Equals("------"))
            {

                String listaAtual = (String)Session["camporecebe" + urlSessao()];
                Session.Remove("camporecebe");

                if (listaAtual.Equals("Link"))
                {
                    txtLinkServer.Text = ListaSelecionada(1);
                    
                }
                
                modalPnFundo.Hide();
            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                modalPnFundo.Show();
            }
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
    }
}