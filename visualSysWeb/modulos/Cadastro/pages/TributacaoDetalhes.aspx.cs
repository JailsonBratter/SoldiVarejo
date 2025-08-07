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
    public partial class TributacaoDetalhes : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            
            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    status = "incluir";
                    tributacaoDAO obj = new tributacaoDAO();
                    Session.Remove("tributacao" + urlSessao());
                    Session.Add("tributacao" + urlSessao(), obj);
                    habilitarCampos(true);
                }
            }
            else //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            {
                if (Request.Params["campoIndex"] != null)  // colocar o campo index da tabela
                {
                    try
                    {
                        if (!IsPostBack)
                        {
                            String index = Request.Params["campoIndex"].ToString();// colocar o campo index da tabela
                            status = "visualizar";
                            tributacaoDAO obj = new tributacaoDAO(index, usr);
                            Session.Remove("tributacao" + urlSessao());
                            Session.Add("tributacao" + urlSessao(), obj);

                            carregarDados();
                        }
                        if (status.Equals("visualizar"))
                        {
                            habilitarCampos(false);
                        }
                        else
                        {
                            habilitarCampos(true);
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


        private void habilitarCampos(bool enable)
        {
            EnabledControls(conteudo, enable);
            EnabledControls(cabecalho, enable);
            
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
            String[] campos = { "txtEntrada_ICMS", 
                                    "txtSaida_ICMS", 
                                    "txtIndice_ST", 
                                    "txtRedutor",
                                    "txtICMS_Efetivo",
                                    "txtNro_ECF",
                                    "txtCsosn",
                                    "txtDescricao_Tributacao"
                                     };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array
            String[] campos = { "txtCodigo_Tributacao", 
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
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Tributacao.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            tributacaoDAO obj = (tributacaoDAO)Session["tributacao" + urlSessao()];
            if (obj.tributacaoUtilizada())
            {
                lblError.Text = "Não é Possivel Excluir essa tributação já esta sendo utilizada";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                modalExcluir.Show();
            }

            // pnConfima.Visible = true;
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaCamposObrigatorios())
                {
                    carregarDadosObj();
                    tributacaoDAO obj = (tributacaoDAO)Session["tributacao" + urlSessao()];
                    obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;
                    habilitarCampos(false);
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
            Response.Redirect("Tributacao.aspx");//colocar endereco pagina de pesquisa
        }
     
        //--Atualizar DaoForm 
        private void carregarDados()
        {
            tributacaoDAO obj = (tributacaoDAO)Session["tributacao" + urlSessao()];
            txtCodigo_Tributacao.Text = obj.Codigo_Tributacao.ToString("N0");
            txtDescricao_Tributacao.Text = obj.Descricao_Tributacao.ToString();
            txtSaida_ICMS.Text = string.Format("{0:0,0.00}", obj.Saida_ICMS);
            txtNro_ECF.Text = string.Format("{0:0,0.00}", obj.Nro_ECF);
            chkGera_Mapa.Checked = obj.Gera_Mapa;
            txtIndice_ST.Text = obj.Indice_ST.ToString();
            txtEntrada_ICMS.Text = string.Format("{0:0,0.00}", obj.Entrada_ICMS);
            txtRedutor.Text = string.Format("{0:0,0.00}", obj.Redutor);
            chkIncide_ICMS.Checked = obj.Incide_ICMS;
            chkIncide_ICM_Subistituicao.Checked = obj.Incide_ICM_Subistituicao;
            txtICMS_Efetivo.Text = string.Format("{0:0,0.00}", obj.ICMS_Efetivo);
            txtCsosn.Text = obj.csosn;
            txtCfop.Text = obj.cfop.ToString();
            txtCstSped.Text = obj.cst_sped;
            chkIpiEmOutrasDespesas.Checked = obj.ipi_EmOutrasDespesas;
            chkIcmsStEmOutrasDespesas.Checked = obj.icmsst_emOutrasDespesas;
            txtCFOPEntrada.Text = obj.cfop_entrada.ToString();

           
        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            tributacaoDAO obj = (tributacaoDAO)Session["tributacao" + urlSessao()];
            
            //obj.Codigo_Tributacao = Decimal.Parse(txtCodigo_Tributacao.Text);
            obj.Descricao_Tributacao = txtDescricao_Tributacao.Text;
            
            obj.Saida_ICMS = Decimal.Parse(txtSaida_ICMS.Text);
            obj.Nro_ECF = Decimal.Parse(txtNro_ECF.Text);
            obj.Gera_Mapa = chkGera_Mapa.Checked;
            obj.Indice_ST = txtIndice_ST.Text;
            obj.Entrada_ICMS = Decimal.Parse(txtEntrada_ICMS.Text);
            obj.Redutor = Decimal.Parse(txtRedutor.Text);
            obj.Incide_ICMS = chkIncide_ICMS.Checked;
            obj.Incide_ICM_Subistituicao = chkIncide_ICM_Subistituicao.Checked;
            obj.ICMS_Efetivo = Decimal.Parse(txtICMS_Efetivo.Text);
            obj.csosn = txtCsosn.Text;
            obj.cfop = Funcoes.decTry(txtCfop.Text);
            obj.cst_sped = txtCstSped.Text;
            obj.ipi_EmOutrasDespesas = chkIpiEmOutrasDespesas.Checked;
            obj.icmsst_emOutrasDespesas = chkIcmsStEmOutrasDespesas.Checked;
            obj.cfop_entrada = Funcoes.decTry(txtCFOPEntrada.Text);
            
            Session.Remove("tributacao" + urlSessao());
            Session.Add("tributacao" + urlSessao(), obj);
        }



        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                tributacaoDAO obj = (tributacaoDAO)Session["tributacao" + urlSessao()];
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

        

      
                  
    }
}