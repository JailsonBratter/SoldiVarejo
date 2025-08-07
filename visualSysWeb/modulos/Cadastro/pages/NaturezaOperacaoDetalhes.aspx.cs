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
    public partial class NaturezaOperacaoDetalhes : visualSysWeb.code.PagePadrao
    {
        static String camporeceber = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    status = "incluir";
                    natureza_operacaoDAO obj = new natureza_operacaoDAO();
                    Session.Remove("NaturezaOperacao" + urlSessao());
                    Session.Add("NaturezaOperacao" + urlSessao(), obj);
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
                            String codigo = Request.Params["campoIndex"].ToString();// colocar o campo index da tabela
                            natureza_operacaoDAO obj = new natureza_operacaoDAO(codigo,usr);
                    
                            status = "visualizar";
                            Session.Remove("NaturezaOperacao" + urlSessao());
                            Session.Add("NaturezaOperacao" + urlSessao(), obj);
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
        private void habilitar(bool enable)
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

            rdoPreco.BackColor = System.Drawing.Color.White;
            rdoPreco.ForeColor = chkBaixa_estoque.ForeColor;
            rdoSaida.BackColor = System.Drawing.Color.White;
            rdoSaida.ForeColor = chkBaixa_estoque.ForeColor;

            if (rdoSaida.SelectedItem == null)
            {
                rdoSaida.BackColor = System.Drawing.Color.Red;
                rdoSaida.ForeColor = System.Drawing.Color.White;
                throw new Exception("Escolha se a natureza será de SAIDA OU ENTRADA");
            }

            if (rdoPreco.SelectedItem == null)
            {
                rdoPreco.BackColor = System.Drawing.Color.Red;
                rdoPreco.ForeColor = System.Drawing.Color.White;
                throw new Exception("Escolha o tipo de preço");
            }

            if (validaCampos(cabecalho) && validaCampos(conteudo))
                return true;
            else
                return false;
        }

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array
            String[] campos = { "txtCodigo_operacao", 
                                    "txtDescricao", 
                                    "", 
                                    "" 
                                     };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array

            if (!status.Equals("incluir") && campo.ID.Equals("txtCodigo_operacao"))
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
            Response.Redirect("NaturezaOperacaoDetalhes.aspx?novo=true"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            editar(pnBtn);
            EnabledControls(cabecalho, true);
            EnabledControls(conteudo, true);
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("NaturezaOperacao.aspx"); //colocar o endereco da tela de pesquisa
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
                    natureza_operacaoDAO obj = (natureza_operacaoDAO)Session["NaturezaOperacao" + urlSessao()];

                    obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;
                    habilitar(false);
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
            Response.Redirect("NaturezaOperacao.aspx");//colocar endereco pagina de pesquisa
        }
        //--Atualizar DaoForm 
        private void carregarDados()
        {
            natureza_operacaoDAO obj = (natureza_operacaoDAO)Session["NaturezaOperacao" + urlSessao()];

            txtCodigo_operacao.Text = obj.Codigo_operacao.ToString();
            txtDescricao.Text = obj.Descricao.ToString();
            chkGera_apagar_receber.Checked = obj.Gera_apagar_receber;
            chkGera_venda.Checked = obj.Gera_venda;
            chkBaixa_estoque.Checked = obj.Baixa_estoque;
            chkIncide_ICMS.Checked = obj.Incide_ICMS;
            chkIncide_IPI.Checked = obj.Incide_IPI;
            chkImprime_NF.Checked = obj.Imprime_NF;
            chkPermite_Desconto.Checked = obj.Permite_Desconto;
            chkGera_caderneta.Checked = obj.Gera_caderneta;
            chkNF_devolucao.Checked = obj.NF_devolucao;
            chkGera_custo.Checked = obj.Gera_custo;
            chkIpi_base.Checked = obj.ipi_base;
            rdoSaida.SelectedValue = (obj.Saida ? "NF Saida" : "NF Entrada");
            rdoPreco.SelectedValue = (obj.Preco_Venda ? "Preco Venda" : "Preco Compra");
            
            chkTipo_movimentacao.Checked = obj.Tipo_movimentacao;
            chkincide_ST.Checked = obj.incide_ST;
            chkincide_PisCofins.Checked = obj.incide_PisCofins;
            txtCstPisCofins.Text = obj.cst_pis_cofins;
            chkdespesas_Base.Checked = obj.despesas_base;

            chkUtilizaCfop.Checked = obj.utilizaCFOP;
            txtCStIcmsSaida.Text = obj.Tributacao_padrao;
            txtCFOP.Text = obj.cfop;
            txtCfopSt.Text = obj.cfop_st;
            ChkGera_Custo_Medio.Checked = obj.IncideCustoMedio;
            ChkGera_Precificacao.Checked = obj.Precificacao;
            chkInativa.Checked = obj.Inativa;
            txtCSTICMS.Text = obj.cst_ICMS;
            chkDestOrigem.Checked = obj.CNPJDestOrigem;
            chkDifal.Checked = obj.Difal;
        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            natureza_operacaoDAO obj = (natureza_operacaoDAO)Session["NaturezaOperacao" + urlSessao()];

            obj.Codigo_operacao = Decimal.Parse(txtCodigo_operacao.Text);
            obj.Descricao = txtDescricao.Text;
            obj.Gera_apagar_receber = chkGera_apagar_receber.Checked;
            obj.Gera_venda = chkGera_venda.Checked;
            obj.Baixa_estoque = chkBaixa_estoque.Checked;
            obj.Incide_ICMS = chkIncide_ICMS.Checked;
            obj.Incide_IPI = chkIncide_IPI.Checked;
            obj.Imprime_NF = chkImprime_NF.Checked;
            obj.Permite_Desconto = chkPermite_Desconto.Checked;
            obj.Gera_caderneta = chkGera_caderneta.Checked;
            obj.NF_devolucao = chkNF_devolucao.Checked;
            obj.Gera_custo = chkGera_custo.Checked;
            obj.Saida = rdoSaida.SelectedItem.Text.Equals("NF Saida");
            obj.Tipo_movimentacao = chkTipo_movimentacao.Checked;
            obj.incide_ST = chkincide_ST.Checked;
            obj.incide_PisCofins = chkincide_PisCofins.Checked;
            obj.Preco_Venda = rdoPreco.SelectedItem.Text.Equals("Preco Venda");
            obj.cst_pis_cofins = txtCstPisCofins.Text;
            obj.ipi_base = chkIpi_base.Checked;
            obj.despesas_base = chkdespesas_Base.Checked;
            obj.Tributacao_padrao = txtCStIcmsSaida.Text;
            obj.cfop = txtCFOP.Text;
            obj.cfop_st = txtCfopSt.Text;
            obj.utilizaCFOP = chkUtilizaCfop.Checked;
            obj.IncideCustoMedio = ChkGera_Custo_Medio.Checked;
            obj.Precificacao = ChkGera_Precificacao.Checked;
            obj.Inativa = chkInativa.Checked;
            obj.cst_ICMS = txtCSTICMS.Text;
            obj.CNPJDestOrigem = chkDestOrigem.Checked;
            obj.Difal = chkDifal.Checked;
        }


        
        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            /*
            try
            {
                obj.excluir();
                pnConfima.Visible = false;
                lblError.Text = "Registro Excluido com sucesso";
                limparCampos();
                pesquisar(pnBtn);
            }
            catch (Exception err)
            {
                lblError.Text = "N?o foi possivel Excluir o registro error:" + err.Message;
            }
             */
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            //pnConfima.Visible = false;
        }

                  
    }
}