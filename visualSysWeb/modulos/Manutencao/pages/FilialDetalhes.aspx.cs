using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class FilialDetalhes : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];

            if (usr != null)
            {
                if (Request.Params["novo"] != null)
                {
                    if (!IsPostBack)
                    {
                        filialDAO obj = new filialDAO();
                        status = "inccluir";
                        Session.Remove("filial" + urlSessao());
                        Session.Add("filial" + urlSessao(), obj);
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
                                String index = Request.Params["campoIndex"].ToString();
                                status = "visualizar";
                                filialDAO obj = new filialDAO(index);
                                Session.Remove("filial" + urlSessao());
                                Session.Add("filial" + urlSessao(), obj);
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
        }

        private void carregarDados()
        {
            filialDAO obj = (filialDAO)Session["filial" + urlSessao()];
            txtFilial.Text = obj.Filial;
            txtRazaoSocial.Text = obj.Razao_Social;
            txtFantasia.Text = obj.Fantasia;
            txtCnpj.Text = obj.CNPJ;
            txtIe.Text = obj.IE;
            txtPluinicial.Text = obj.PLU_inicial;
            txtConversor.Text = obj.conversor;
            txtLoja.Text = obj.loja.ToString();
            txtCstPisCofins.Text = obj.cst_pis_cofins;
            txtPis.Text = obj.pis.ToString("N2");
            txtCofins.Text = obj.cofins.ToString("N2");
            txtEndereco.Text = obj.Endereco;
            txtEndereco_Nro.Text = obj.endereco_nro;
            
            txtCidade.Text = obj.Cidade;
            txtBairro.Text = obj.bairro;
            txtUF.Text = obj.UF;
            txtCEP.Text = obj.CEP;
            chkBaixaCaderneta.Checked = obj.baixa_caderneta;
            chkDesmarcarAlteracoes.Checked = obj.dismarca_alteracoes;
            chkGeraVendedor.Checked = obj.gera_vendedor;
            chkProduta.Checked = obj.produtora;
            txtRegEstadual.Text = obj.Reg_Estadual;
            txtRegFederal.Text = obj.Reg_Federal;
            txtRPA.Text = obj.RPA;
            txtIr.Text = obj.IR.ToString();
            txtCsll.Text = obj.CSLL.ToString();
            txtAliquota_est.Text = obj.Aliquota_est;
            txtAliquota_fed.Text = obj.Aliquota_fed;
            //txtTelefone.Text = obj.telefone;
            txtSerie_nfe.Text = obj.serie_nfe.ToString();
            txtFone.Text = obj.fone;
            txtNumero.Text = obj.numero;
            txtIcmsSN.Text = obj.ICMSSN;
            txtCsoSn.Text = obj.CSOSN;
            txtCRT.Text = obj.CRT;
            txtDataFechamentoEstoque.Text = obj.dtFechamentoEstoqueBr;
            txtDataFechamentoFinanceiro.Text = obj.dtFechamentoFinanceiroBr;
            txtDiretorioGera.Text = obj.diretorio_gera;
            txtDiretorioBuscaPreco.Text = obj.diretorio_busca_preco;
            txtDiretorio_Exporta.Text = obj.diretorio_exporta;
            txtDiretorio_multiloja.Text = obj.diretorio_multiloja;
            txtDiretorioBalanca.Text = obj.diretorio_balanca;
            txtInicioPeriodo.Text = obj.inicio_periodo;
            txtFimPeriodo.Text = obj.fim_periodo;
            ddlDiasPeriodo.SelectedValue = obj.dias_periodo.ToString();
            ddlPDV.SelectedValue = obj.pdv.ToString();


        }

        private void carregarDadosObj()
        {
            filialDAO obj = (filialDAO)Session["filial" + urlSessao()];

            obj.Filial = txtFilial.Text  ;
            obj.Razao_Social = txtRazaoSocial.Text  ;
            obj.Fantasia = txtFantasia.Text  ;
            obj.CNPJ = txtCnpj.Text  ;
            obj.IE = txtIe.Text  ;
            obj.PLU_inicial = txtPluinicial.Text ;
            obj.conversor = txtConversor.Text  ;
            
            int.TryParse(txtLoja.Text, out obj.loja);

            obj.cst_pis_cofins = txtCstPisCofins.Text;
            Decimal.TryParse(txtPis.Text, out obj.pis);
            Decimal.TryParse(txtCofins.Text, out obj.cofins);

            obj.Endereco = txtEndereco.Text;
            obj.Cidade = txtCidade.Text;
            obj.numero = txtNumero.Text ;
            obj.bairro =txtBairro.Text ;
            obj.UF = txtUF.Text  ;
            obj.CEP = txtCEP.Text  ;
            obj.baixa_caderneta = chkBaixaCaderneta.Checked;
            obj.dismarca_alteracoes = chkDesmarcarAlteracoes.Checked;
            obj.gera_vendedor = chkGeraVendedor.Checked;
            obj.produtora = chkProduta.Checked;
            obj.Reg_Estadual= txtRegEstadual.Text ;
            obj.Reg_Federal = txtRegFederal.Text  ;
            obj.RPA = txtRPA.Text  ;
            
            Decimal.TryParse(txtIr.Text, out obj.IR);
            Decimal.TryParse(txtCsll.Text, out obj.CSLL);

            obj.Aliquota_est = txtAliquota_est.Text;
            obj.Aliquota_fed = txtAliquota_fed.Text  ;
            //obj.telefone = txtTelefone.Text  ;
            
            int.TryParse(txtSerie_nfe.Text, out obj.serie_nfe);
            obj.fone = txtFone.Text;
            obj.numero = txtNumero.Text ;
            obj.ICMSSN = txtIcmsSN.Text  ;
            obj.CSOSN = txtCsoSn.Text  ;
            obj.CRT = txtCRT.Text  ;
            DateTime.TryParse(txtDataFechamentoEstoque.Text, out obj.dtFechamentoEstoque);
            DateTime.TryParse(txtDataFechamentoFinanceiro.Text, out obj.dtFechamentoFinanceiro);
            
             obj.diretorio_gera= txtDiretorioGera.Text ;
            
            obj.diretorio_busca_preco = txtDiretorioBuscaPreco.Text ;
            obj.diretorio_exporta= txtDiretorio_Exporta.Text ;
            obj.diretorio_multiloja=txtDiretorio_multiloja.Text  ;
            obj.diretorio_balanca = txtDiretorioBalanca.Text  ;

            obj.inicio_periodo = txtInicioPeriodo.Text;
            obj.fim_periodo = txtFimPeriodo.Text;

            int.TryParse(ddlDiasPeriodo.SelectedValue, out obj.dias_periodo);
            obj.pdv = Funcoes.intTry(ddlPDV.SelectedItem.Value);

            Session.Remove("filial" + urlSessao());
            Session.Add("filial" + urlSessao(), obj);
        }


        private void habilitar(bool enable)
        {
            EnabledControls(conteudo, enable);
            EnabledControls(cabecalho, enable);
                        
        }


        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FilialDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            status="editar";
            habilitar(true);
            carregabtn(pnBtn);
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Filial.aspx");
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        protected bool validaCamposObrigatorios()
        {

            if (validaCampos(cabecalho) && validaCampos(conteudo))
            {
                    return true;
                
            }
            else
                return false;
        }
        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaCamposObrigatorios())
                {

                    carregarDadosObj();
                    filialDAO obj = (filialDAO)Session["filial" + urlSessao()];

                    obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                    status = "visualizar";
                    visualizar(pnBtn);
                    carregarDados();
                    habilitar(false);



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
                carregarDados();

            }
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Filial.aspx");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array
            String[] campos = { (status=="incluir"?"":"txtFilial"), 
                                    "", 
                                    "", 
                                    "" 
                                     };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array
            String[] campos = {     "txtFilial", 
                                    "txtRazaoSocial",
                                    ""
                                     };
            return existeNoArray(campos, campo.ID + "");

        }
    }
}