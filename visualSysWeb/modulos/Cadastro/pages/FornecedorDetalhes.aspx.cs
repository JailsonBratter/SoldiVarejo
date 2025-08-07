using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using visualSysWeb.code;
using System.Collections;
using System.Data.SqlClient;
using visualSysWeb.modulos.Cadastro.code;

namespace visualSysWeb.Cadastro
{
    public partial class FornecedorDetalhes : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                User usr = (User)Session["User"];
                if (Request.Params["fornecedor"] != null)
                {

                    if (!IsPostBack)
                    {
                        String fornecedor = Request.Params["fornecedor"].ToString().Replace("|E", "&");

                        fornecedorDAO obj = new fornecedorDAO(fornecedor, usr);
                        status = "visualizar";
                        String strTeste = "fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", "").Replace(" ", "").Replace("+", "");
                        Session.Remove("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""));
                        Session.Add("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""), obj);
                        carregarDados();
                        habilitar(false);


                    }
                }
                else
                {
                    if (!IsPostBack)
                    {
                        fornecedorDAO obj = new fornecedorDAO(usr);
                        status = "incluir";
                        habilitar(true);
                        Session.Remove("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""));
                        Session.Add("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""), obj);
                    }
                }

                carregabtn(pnBtn);

            }
            catch (Exception err)
            {

                lblError.Text = err.Message;
            }

            formataCampos();

        }


        private void habilitar(bool enable)
        {

            EnabledControls(conteudo, enable);
            EnabledControls(cabecalho, enable);
            EnabledControls(pnEditarProduto, enable);
            User usr = (User)Session["User"];
            if (usr == null)
                return;


            if (!usr.adm())
            {
                tabCadastro.Visible = usr.telaPermissao("Cadastro");
                tabContato.Visible = usr.telaPermissao("Contato");
                tabMercadoria.Visible = usr.telaPermissao("Mercadoria");
                tabAdicionais.Visible = usr.telaPermissao("Adicionais");
                tabObservacoes.Visible = usr.telaPermissao("Observacao");
                tabDepartamentos.Visible = usr.telaPermissao("Departamentos");
            }

        }


        private void formataCampos()
        {

            txtCEP.Attributes.Add("OnKeyUp", "javascript:return formataCEP(this,event);");
            if (chkpessoa_fisica.Checked)
            {
                lblCnpj_Cpf.Text = "CPF";
                txtCNPJ.Attributes.Add("OnKeyUp", "javascript:return formataCPF(this,event);");
                txtCNPJ.MaxLength = 14;
            }
            else
            {
                lblCnpj_Cpf.Text = "CNPJ";
                txtCNPJ.Attributes.Add("OnKeyUp", "javascript:return formataCNPJ(this,event);");
                txtCNPJ.MaxLength = 18;
            }


            txtTelefone1.Attributes.Add("OnKeyUp", "javascript:return formataTelefone(this,event);");
            txtTelefone2.Attributes.Add("OnKeyUp", "javascript:return formataTelefone(this,event);");
            txtTelefone3.Attributes.Add("OnKeyUp", "javascript:return formataTelefone(this,event);");

            txtTelefone1_2.Attributes.Add("OnKeyUp", "javascript:return formataTelefone(this,event);");
            txtTelefone2_2.Attributes.Add("OnKeyUp", "javascript:return formataTelefone(this,event);");
            txtTelefone3_2.Attributes.Add("OnKeyUp", "javascript:return formataTelefone(this,event);");

            txtTelefone1_3.Attributes.Add("OnKeyUp", "javascript:return formataTelefone(this,event);");
            txtTelefone2_3.Attributes.Add("OnKeyUp", "javascript:return formataTelefone(this,event);");
            txtTelefone3_3.Attributes.Add("OnKeyUp", "javascript:return formataTelefone(this,event);");


            camposnumericos();
        }

        private void camposnumericos()
        {
            ArrayList campos = new ArrayList();
            campos.Add("txtContado");
            FormataCamposNumericos(campos, conteudo);
            FormataCamposNumericos(campos, cabecalho);

            ArrayList camposInteiros = new ArrayList();
            camposInteiros.Add("txtEndereco_nro");
            camposInteiros.Add("txtCodMunicipio");

            FormataCamposInteiros(camposInteiros, conteudo);
            FormataCamposInteiros(camposInteiros, cabecalho);

        }

        protected override bool campoDesabilitado(Control campo)
        {
            if (status != "incluir" && campo != null && campo.ID.Equals("txtFornecedor"))
            {
                return true;
            }



            String[] campos = { "txtCidade",
                           "txtDescricao_produto",
                           "txtUsuario",
                           "txtUsuarioAlteracao",
                           "ddStatus"
                          };

            for (int i = 0; i < campos.Length; i++)
            {
                if (campo.ID.Equals(campos[i]))
                {

                    return true;
                }
            }
            return false;
        }


        private void carregarDados()
        {

            fornecedorDAO obj = (fornecedorDAO)Session["fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", "")];
            txtFornecedor.Text = obj.Fornecedor.ToString();
            //txtFilial.Text = obj.Filial.ToString();
            txtUsuario.Text = obj.Usuario;
            txtUsuarioAlteracao.Text = obj.UsuarioAlteracao;
            txtRazao_social.Text = obj.Razao_social.ToString();
            txtNome_Fantasia.Text = obj.Nome_Fantasia.ToString();
            txtCNPJ.Text = obj.CNPJ.ToString();
            txtIE.Text = obj.IE.ToString();
            txtCidade.Text = obj.Cidade.ToString();
            txtUF.Text = obj.UF.ToString();
            txtCEP.Text = obj.CEP.ToString();
            txtEndereco.Text = obj.Endereco.ToString();
            txtBairro.Text = obj.Bairro.ToString();
            txtDesc_Coml.Text = string.Format("{0:0,0.00}", obj.Desc_Coml);
            txtDesc_Finan.Text = string.Format("{0:0,0.00}", obj.Desc_Finan);
            txtAdc_Finan.Text = string.Format("{0:0,0.00}", obj.Adc_Finan);
            txtAdc_Mkt.Text = string.Format("{0:0,0.00}", obj.Adc_Mkt);
            txtAdc_Perda.Text = string.Format("{0:0,0.00}", obj.Adc_Perda);
            txtAdc_Frete.Text = string.Format("{0:0,0.00}", obj.Adc_Frete);
            txtUltima_Contagem_do_Estoque.Text = obj.Ultima_Contagem_do_EstoqueBr();
            txtBonificacao.Text = string.Format("{0:0,0.00}", obj.Bonificacao);
            txtPrazo.Text = string.Format("{0:0,0.00}", obj.Prazo);
            txtcondicao_pagamento.Text = obj.condicao_pagamento.ToString();
            txtDesc_exp.Text = string.Format("{0:0,0.00}", obj.Desc_exp);
            txtobs.Text = obj.obs.ToString();
            chkpessoa_fisica.Checked = obj.pessoa_fisica;
            txtsenha.Text = obj.senha.ToString();
            txtCodMunicipio.Text = obj.codmun;
            ddlTipoIE.SelectedValue = obj.indIEDest.ToString();
            //txtCodigo_fornecedor.Text = obj.Codigo_fornecedor.ToString();
            txtCentroCusto.Text = obj.centro_custo;
            txtEndereco_nro.Text = obj.Endereco_nro.ToString();
            chkFormulario_proprio.Checked = obj.Formulario_proprio;
            chkProdutor_rural.Checked = obj.produtor_rural;
            ddlTipoFornecedor.Text = (obj.Tipo_fornecedor ? "ADMINISTRATIVO" : "COMERCIAL");

            ddlCRT.Text = obj.CRT.ToString();
            carregarGrids();

            if (chkpessoa_fisica.Checked)
            {
                lblCnpj_Cpf.Text = "CPF";

            }
            else
            {
                lblCnpj_Cpf.Text = "CNPJ";
                txtCNPJ.Attributes.Add("OnKeyUp", "javascript:return formataCNPJ(this,event);");
            }

            chkBaseIpi.Checked = obj.ipi_base;
            chkDespesasBase.Checked = obj.despesas_base;
            chkInativo.Checked = (obj.inativo > 0 ? true : false);

            txtEmail.Text = obj.email;
            txtSite.Text = obj.site;
            txtTelefone1.Text = obj.telefone1;
            txtContato1.Text = obj.contato1;
            txtTelefone2.Text = obj.telefone2;
            txtContato2.Text = obj.contato2;
            txtTelefone3.Text = obj.telefone3;
            txtContato3.Text = obj.contato3;

            txtCargo1.Text = obj.cargo1;
            txtTelefone1_2.Text = obj.telefone1_2;
            txtTelefone1_3.Text = obj.telefone1_3;
            txtEmail1.Text = obj.email1;

            txtCargo2.Text = obj.cargo2;
            txtTelefone2_2.Text = obj.telefone2_2;
            txtTelefone2_3.Text = obj.telefone2_3;
            txtEmail2.Text = obj.email2;

            txtCargo3.Text = obj.cargo3;
            txtTelefone3_2.Text = obj.telefone3_2;
            txtTelefone3_3.Text = obj.telefone3_3;
            txtEmail3.Text = obj.email3;

            txtContaContabilCredito.Text = obj.conta_contabil_credito;
            txtContaContabilDebito.Text = obj.conta_contabil_debito;


        }

        protected void gridMeioComunicacao_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void gridMeioComunicacao_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            fornecedorDAO obj = (fornecedorDAO)Session["fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", "")];

            int index = Convert.ToInt32(e.CommandArgument);
            gridMeioComunicacao.DataSource = obj.meiosComunicacao;
            gridMeioComunicacao.DataBind();

            GridViewRow row = gridMeioComunicacao.Rows[index];


            obj.deleteMeioComunicacao(row.Cells[2].Text);


            Session.Remove("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""));
            Session.Add("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""), obj);
            carregarGrids();
        }


        private void carregarDadosObj()
        {

            fornecedorDAO obj = (fornecedorDAO)Session["fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", "")];
            obj.Fornecedor = Funcoes.RemoverAcentos(txtFornecedor.Text);
            // obj.Filial = txtFilial.Text;
            obj.Razao_social = Funcoes.RemoverAcentos(txtRazao_social.Text);
            obj.Nome_Fantasia = Funcoes.RemoverAcentos(txtNome_Fantasia.Text);
            obj.CNPJ = txtCNPJ.Text;
            obj.IE = txtIE.Text;
            obj.Cidade = Funcoes.RemoverAcentos(txtCidade.Text);
            obj.UF = txtUF.Text;
            obj.CEP = txtCEP.Text;
            obj.Endereco = Funcoes.RemoverAcentos(txtEndereco.Text);
            obj.Bairro = Funcoes.RemoverAcentos(txtBairro.Text);
            obj.Desc_Coml = Decimal.Parse(txtDesc_Coml.Text.Equals("") ? "0.0" : txtDesc_Coml.Text);
            obj.Desc_Finan = Decimal.Parse(txtDesc_Finan.Text.Equals("") ? "0.0" : txtDesc_Finan.Text);
            obj.Adc_Finan = Decimal.Parse(txtAdc_Finan.Text.Equals("") ? "0.0" : txtAdc_Finan.Text);
            obj.Adc_Mkt = Decimal.Parse(txtAdc_Mkt.Text.Equals("") ? "0.0" : txtAdc_Mkt.Text);
            obj.Adc_Perda = Decimal.Parse(txtAdc_Perda.Text.Equals("") ? "0.0" : txtAdc_Perda.Text);
            obj.Adc_Frete = Decimal.Parse(txtAdc_Frete.Text.Equals("") ? "0.0" : txtAdc_Frete.Text);
            obj.Ultima_Contagem_do_Estoque = (txtUltima_Contagem_do_Estoque.Text.Equals("") ? new DateTime() : DateTime.Parse(txtUltima_Contagem_do_Estoque.Text));
            obj.Bonificacao = Decimal.Parse(txtBonificacao.Text.Equals("") ? "0.0" : txtBonificacao.Text);
            obj.Prazo = Decimal.Parse(txtPrazo.Text.Equals("") ? "0.0" : txtPrazo.Text);
            obj.condicao_pagamento = txtcondicao_pagamento.Text;
            obj.Desc_exp = Decimal.Parse(txtDesc_exp.Text.Equals("") ? "0.0" : txtDesc_exp.Text);
            obj.obs = txtobs.Text;
            obj.pessoa_fisica = chkpessoa_fisica.Checked;
            obj.senha = txtsenha.Text;
            obj.Endereco_nro = txtEndereco_nro.Text;
            obj.Formulario_proprio = chkFormulario_proprio.Checked;
            obj.email = txtEmail.Text;
            obj.site = txtSite.Text;
            obj.telefone1 = txtTelefone1.Text;
            obj.contato1 = Funcoes.RemoverAcentos(txtContato1.Text);
            obj.telefone2 = txtTelefone2.Text;
            obj.contato2 = Funcoes.RemoverAcentos(txtContato2.Text);
            obj.telefone3 = txtTelefone3.Text;
            obj.contato3 = Funcoes.RemoverAcentos(txtContato3.Text);
            obj.codmun = txtCodMunicipio.Text;
            obj.produtor_rural = chkProdutor_rural.Checked;
            obj.centro_custo = txtCentroCusto.Text;

            obj.cargo1 = txtCargo1.Text;
            obj.telefone1_2 = txtTelefone1_2.Text;
            obj.telefone1_3 = txtTelefone1_3.Text;
            obj.email1 = txtEmail1.Text;

            obj.cargo2 = txtCargo2.Text;
            obj.telefone2_2 = txtTelefone2_2.Text;
            obj.telefone2_3 = txtTelefone2_3.Text;
            obj.email2 = txtEmail2.Text;

            obj.cargo3 = txtCargo3.Text;
            obj.telefone3_2 = txtTelefone3_2.Text;
            obj.telefone3_3 = txtTelefone3_3.Text;
            obj.email3 = txtEmail3.Text;
            obj.Tipo_fornecedor = ddlTipoFornecedor.Text.Equals("ADMINISTRATIVO");
            obj.Usuario = txtUsuario.Text;
            User usr = (User)Session["User"];
            obj.UsuarioAlteracao = usr.getUsuario();
            obj.conta_contabil_credito = txtContaContabilCredito.Text;
            obj.conta_contabil_debito = txtContaContabilDebito.Text;
            obj.ipi_base = chkBaseIpi.Checked;
            obj.despesas_base = chkDespesasBase.Checked;
            obj.inativo = (chkInativo.Checked ? 1 : 0);
            obj.CRT = Funcoes.intTry(ddlCRT.SelectedValue.ToString());

            int.TryParse(ddlTipoIE.SelectedValue, out obj.indIEDest);
            Session.Remove("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""));
            Session.Add("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""), obj);

        }


        protected void GridMercadoria_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[10].Text.Equals("True"))
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }


            }
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FornecedorDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            fornecedorDAO obj = new fornecedorDAO(txtFornecedor.Text, usr);
            status = "editar";
            Session.Remove("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""));
            Session.Add("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""), obj);

            habilitar(true);
            lblError.Text = "";
            editar(pnBtn);
            carregarDados();
            //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {

            Response.Redirect("Fornecedor.aspx");
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            modalExcluirFornecedor.Show();
        }

        protected void btnCancelarExclusao_Click(object sender, EventArgs e)
        {
            modalExcluirFornecedor.Hide();
        }

        protected void btnConfirmarExclusao_Click(object sender, EventArgs e)
        {
            try
            {
                fornecedorDAO obj = (fornecedorDAO)Session["fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", "")];
                obj.excluir();
                Session.Remove("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""));
                status = "pesquisar";
                carregabtn(pnBtn);
                lblError.Text = "Fornecedor Excluido Com sucesso!";
                lblError.ForeColor = System.Drawing.Color.Blue;
                modalExcluirFornecedor.Hide();
                LimparCampos(conteudo);
                LimparCampos(cabecalho);

            }
            catch (Exception erro)
            {
                lblError.Text = "Não é possivel excluir o registro erro :" + erro.Message;
                lblError.ForeColor = System.Drawing.Color.Red;

            }
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            carregarDadosObj();
            if (validarCnpjCpf() && validarIE())
            {
                salvar();

            }
            else
            {
                modalConfirmaSalvar.Show();
            }


        }
        private void salvar()
        {
            try
            {

                validar();

                //carregarDadosObj();
                fornecedorDAO obj = (fornecedorDAO)Session["fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", "")];
                obj.salvar(status.Equals("incluir")); // se for incluir true se não falso;
                carregarDados();
                lblError.Text = "Salvo com Sucesso";
                lblError.ForeColor = System.Drawing.Color.Blue;
                habilitar(false);
                visualizar(pnBtn);

                //Integração
                User usr = (User)Session["User"];
                if (Funcoes.valorParametro("INTEGRA_WS", usr).Equals("RAKUTEN"))
                {
                    try
                    {
                        //KCWSFabricante KCForn = new KCWSFabricante(obj, "Salvar",usr);
                    }
                    catch
                    {

                    }
                }
            }//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            catch (Exception err)
            {

                lblError.Text = "Error:" + err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;

            }
        }
        private bool validarCnpjCpf()
        {
            bool validar = false;

            try
            {


                if (chkpessoa_fisica.Checked)
                    validar = Funcoes.IsCpf(txtCNPJ.Text);
                else
                    validar = Funcoes.IsCnpj(txtCNPJ.Text);

            }
            catch (Exception)
            {


            }
            return validar;

        }



        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Fornecedor.aspx");
        }
        protected override bool campoObrigatorio(Control campo)
        {
            String[] campos = { "txtFornecedor",
                           "txtCidade",
                           "txtCodMunicipio",
                           "txtUF",
                           "",
                           "",
                           ""
                          };

            for (int i = 0; i < campos.Length; i++)
            {
                if (campo.ID.Equals(campos[i]))
                {

                    return true;
                }
            }
            return false;

        }

        protected void validar()
        {
            //validarCnpjCpf();


            txtFornecedor.BackColor = System.Drawing.Color.White;

            validaCampos(cabecalho);
            validaCampos(conteudo);
            if (txtFornecedor.Text.Length > 20)
            {

                txtFornecedor.BackColor = System.Drawing.Color.Red;
                throw new Exception("o Campo fornecedor não pode ter mais do que 20 Caracteres");

            }
            if (txtEmail.Text.Length > 0)
            {
                if (txtEmail.Text.IndexOf("@") <= 0 || txtEmail.Text.IndexOf(".") <= 0)
                {
                    txtEmail.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Emai-l Inválido");
                }
            }

            String codMunic = Conexao.retornaUmValor("select munic from unidade_federacao where upper(nome_munic)='" + txtCidade.Text.ToUpper().Trim() + "' and sigla_uf='" + txtUF.Text + "'", null);
            if (!codMunic.Equals(txtCodMunicipio.Text))
            {
                txtCodMunicipio.BackColor = System.Drawing.Color.Red;
                txtCidade.BackColor = System.Drawing.Color.Red;
                throw new Exception("O Codigo do Municipio não corresponde a cidade informada");

            }


        }

        protected void lista_click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;


            String or = btn.ID;

            Session.Remove("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""));
            Session.Add("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""), or);
            TxtPesquisaLista.Text = "";
            exibeLista();

        }

        protected void exibeLista()
        {
            lblErroPesquisa.Text = "";
            String campo = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", "")];
            String sqlLista = "";

            switch (campo)
            {
                case "imgBtnCidade":
                    lbllista.Text = "Escolha um Municipio";
                    sqlLista = "select upper(nome_munic) as Cidade , munic as 'Cod Municipio' , Sigla_uf as UF from unidade_federacao  where" + (txtUF.Text.Equals("") ? "" : "(Sigla_uf='" + txtUF.Text.ToUpper() + "')  and") + "( nome_munic like '%" + TxtPesquisaLista.Text + "%' or munic like '%" + TxtPesquisaLista.Text + "%' or Sigla_uf like '%" + TxtPesquisaLista.Text + "%')  order by sigla_uf, nome_munic  ";

                    break;
                case "imgMeioComunicacao":
                    lbllista.Text = "Escolha um Meio de comunicação";
                    sqlLista = "SELECT Meio_comunicacao FROM  meio_comunicacao where meio_comunicacao like '%" + TxtPesquisaLista.Text + "%';";

                    break;
                case "imgBtn_txtCEP":
                    lbllista.Text = "Escolha o CEP";
                    sqlLista = "select top 500 * from CEP_Brasil where (cep+Logradouro+Bairro+Cidade+UF) like '%" + TxtPesquisaLista.Text.Replace(" ", "%") + "%'";
                    break;
                case "imgBtnCentroCusto":
                    lbllista.Text = "Escolha o Centro de Custo";
                    sqlLista = "select Codigo_centro_custo,descricao_centro_custo,modalidade from Centro_Custo where (descricao_centro_custo) like '%" + TxtPesquisaLista.Text.Replace(" ", "%") + "%' or codigo_centro_custo like '" + TxtPesquisaLista.Text + "%'";
                    break;
                case "Departamento":
                    lbllista.Text = "Escolha o Departamento";
                    sqlLista = "Select   cod= codigo_departamento," +
                                       " Grupo= Descricao_grupo," +
                                       " SubGrupo = descricao_subgrupo," +
                                       " Departamento=descricao_departamento " +

                                   " from W_BR_CADASTRO_DEPARTAMENTO " +
                                   " where  " +
                                       " codigo_departamento Like '%" + TxtPesquisaLista.Text.Trim() + "%'" +
                                       " or descricao_grupo like '%" + TxtPesquisaLista.Text.Trim() + "%'" +
                                       " or descricao_subgrupo like '%" + TxtPesquisaLista.Text.Trim() + "%'" +
                                       " or descricao_departamento like '%" + TxtPesquisaLista.Text.Trim() + "%'";
                    break;
                case "imgTxtPLu_produto":
                    lbllista.Text = "Escolha o Produto";
                    sqlLista = "Select Plu , Descricao from mercadoria " +
                                "where " +
                                "     plu like'%" + TxtPesquisaLista.Text + "%'" +
                                " OR descricao like '%" + TxtPesquisaLista.Text + "%'";

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

            modalLista.Show();
            TxtPesquisaLista.Focus();
        }
        protected void btnFechar_Click(object sender, ImageClickEventArgs e)
        {
            String selecionado = ListaSelecionada(1);

            if (!selecionado.Equals("") && !selecionado.Equals("------"))
            {

                modalLista.Hide();
                User usr = (User)Session["User"];
                String itemLista = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", "")];

                if (itemLista.Equals("imgBtnCidade"))
                {
                    txtCidade.Text = ListaSelecionada(1);
                    txtCodMunicipio.Text = ListaSelecionada(2);
                    txtUF.Text = ListaSelecionada(3);
                }
                if (itemLista.Equals("imgMeioComunicacao"))
                {
                    txtMeioComunicacao.Text = ListaSelecionada(1);

                }
                else if (itemLista.Equals("imgBtn_txtCEP"))
                {
                    txtCEP.Text = ListaSelecionada(5);
                    txtEndereco.Text = ListaSelecionada(1);
                    txtBairro.Text = ListaSelecionada(2);
                    txtCidade.Text = ListaSelecionada(3);
                    txtUF.Text = ListaSelecionada(4);
                    txtEndereco_nro.Focus();
                }
                else if (itemLista.Equals("imgBtnCentroCusto"))
                {
                    txtCentroCusto.Text = ListaSelecionada(1);
                }
                else if (itemLista.Equals("Departamento"))
                {
                    fornecedorDAO obj = (fornecedorDAO)Session["fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", "")];

                    String strDepartamento = selecionado;
                    String strFornecedor = txtFornecedor.Text;


                    if (!obj.departamentoExiste(strDepartamento))
                    {
                        obj.addDepartamento(strDepartamento);
                    }
                    Session.Remove("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""));
                    Session.Add("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""), obj);
                }
                else if (itemLista.Equals("imgTxtPLu_produto"))
                {
                    txtPLU_produto.Text = selecionado;
                    txtDescricao_produto.Text = ListaSelecionada(2);
                    modalEditarProduto.Show();

                }
                carregarGrids();

            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                modalLista.Show();
            }
        }

        protected void carregarGrids()
        {
            fornecedorDAO obj = (fornecedorDAO)Session["fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", "")];

            gridMeioComunicacao.DataSource = obj.meiosComunicacao;
            gridMeioComunicacao.DataBind();

            gridMercadorias.DataSource = obj.Mercadorias;
            gridMercadorias.DataBind();

            gridMercadorias.Columns[10].Visible = false;

            gridFornecedorDepartamento.DataSource = obj.departamentos;
            gridFornecedorDepartamento.DataBind();

            gridOcorrencia.DataSource = obj.ocorrencia;
            gridOcorrencia.DataBind();



        }
        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalLista.Hide();
            carregarGrids();
            String itemLista = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", "")];
            if (itemLista.Equals("imgTxtPLu_produto"))
            {
                modalEditarProduto.Show();
            }
        }

        protected void TxtPesquisaLista_TextChanged(object sender, EventArgs e)
        {
            exibeLista();
        }
        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
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

        protected void btnAddComunicacao_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                txtMeioComunicacao.BackColor = System.Drawing.Color.White;
                int exist = 0;
                int.TryParse(Conexao.retornaUmValor("Select COUNT(*) from meio_comunicacao where Meio_Comunicacao='" + txtMeioComunicacao.Text + "'", null), out exist);
                if (exist == 0)
                {
                    txtMeioComunicacao.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Meio de comunicação não existe! Escolha um meio válido");
                }

                fornecedorDAO obj = (fornecedorDAO)Session["fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", "")];
                obj.addMeioComunicacao(txtMeioComunicacao.Text, txtIdMeioComunicacao.Text, txtContatoComunicacao.Text);

                txtMeioComunicacao.Text = "";
                txtIdMeioComunicacao.Text = "";
                txtContatoComunicacao.Text = "";
                gridMeioComunicacao.DataSource = obj.meiosComunicacao;
                gridMeioComunicacao.DataBind();

                Session.Remove("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""));
                Session.Add("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""), obj);
            }
            catch (Exception err)
            {
                lblError.Text = err.Message;
                carregarGrids();
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

        protected void chkpessoa_fisica_CheckedChanged(object sender, EventArgs e)
        {
            if (chkpessoa_fisica.Checked)
            {
                lblCnpj_Cpf.Text = "CPF";
                ddlTipoIE.SelectedValue = "9";
                ddlCRT.SelectedValue = "0";
            }
            else
            {
                lblCnpj_Cpf.Text = "CNPJ";
            }
        }

        protected void txtEmail_TextChanged(object sender, EventArgs e)
        {

            TextBox txt = (TextBox)sender;
            txt.BackColor = System.Drawing.Color.White;
            if (!txt.Text.Equals(""))
            {
                if (txt.Text.IndexOf("@") <= 0 || txt.Text.IndexOf(".") <= 0)
                {
                    txt.BackColor = System.Drawing.Color.Red;
                    lblError.Text = "E-mail Inválido";
                }
            }


        }

        protected void txtCidade_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtCodMunicipio.Text = Conexao.retornaUmValor("select munic from unidade_federacao where upper(nome_munic)='" + txtCidade.Text.ToUpper().Trim() + "' and sigla_uf='" + txtUF.Text + "'", null);
            }
            catch
            {
                txtCodMunicipio.Text = "";
            }

        }

        protected void txtCEP_TextChanged(object sender, EventArgs e)
        {
            if (txtEndereco.Text.Trim().Length == 0 && txtCidade.Text.Trim().Length == 0 && txtBairro.Text.Trim().Length == 0 && txtCEP.Text.Replace("-", "").Length == 8)
            {
                WSCEP wscep = new WSCEP(txtCEP.Text.Replace("-", ""));
                if (!wscep.logradouro.Equals(""))
                {
                    txtEndereco.Text = wscep.logradouro;
                    txtBairro.Text = wscep.bairro;
                    txtCidade.Text = wscep.localidade;
                    txtUF.Text = wscep.uf;
                    txtEndereco_nro.Focus();
                }
                else
                {
                    User usr = (User)Session["User"];
                    SqlDataReader rd = Conexao.consulta("EXEC sp_Cons_CEP '" + txtCEP.Text.Replace("-", "") + "'", usr, true);
                    if (rd.Read())
                    {
                        txtEndereco.Text = rd["Logradouro"].ToString();
                        txtBairro.Text = rd["Bairro"].ToString();
                        txtCidade.Text = rd["Cidade"].ToString();
                        txtUF.Text = rd["UF"].ToString();
                        txtEndereco_nro.Focus();
                    }
                }
            }
        }

        protected void txtIE_TextChanged(object sender, EventArgs e)
        {
            lblError.Text = "";
            try
            {
                validarIE();
            }
            catch (Exception err)
            {
                lblError.Text = err.Message;
            }
        }

        private bool validarIE()
        {
            bool validar = false;
            if (!chkpessoa_fisica.Checked)
            {
                validar = !(Funcoes.isIE(txtUF.Text, txtIE.Text));
            }
            if (validar)
            {
                txtIE.BackColor = System.Drawing.Color.Red;
                lblError.Text = "Incrição Estadual invalida ";
                return false;
            }
            return true;

        }

        protected void btnConfirmarSalvar_Click(object sender, EventArgs e)
        {
            salvar();
        }
        protected void btnCancelaSalvar_Click(object sender, EventArgs e)
        {
            modalConfirmaSalvar.Hide();
        }

        protected void gridFornecedorDepartamento_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            int index = Convert.ToInt32(e.CommandArgument);
            carregarGrids();
            GridViewRow rw = gridFornecedorDepartamento.Rows[index];

            lblCodFornecExcluir.Text = rw.Cells[1].Text;

            modalPnConfirmaDepartamento.Show();

        }
        protected void btnConfirmaExclusaoDepartamento_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                fornecedorDAO obj = (fornecedorDAO)Session["fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", "")];



                //String strFornecedor = txtFornecedor.Text;
                String strDepartamento = lblCodFornecExcluir.Text;

                obj.excluirDepartamento(strDepartamento);

                //String sql = "Delete from Fornecedor_departamento " +
                //               " where fornecedor='" + strFornecedor.Trim()  + "'" +
                //               " and codigo_departamento='" + strDepartamento.Trim() + "'";


                //Conexao.executarSql(sql);
                carregarGrids();
            }
            catch (Exception err)
            {

                lblError.Text = "Não foi possivel Excluir o registro pelo error:" + err.Message;


            }
        }
        protected void btnCancelaExclusaoDepartamento_Click(object sender, ImageClickEventArgs e)
        {
            modalPnConfirmaDepartamento.Hide();

        }

        //imgAddDepartamento_Click
        protected void imgAddDepartamento_Click(object sender, ImageClickEventArgs e)
        {
            Session.Remove("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""));
            Session.Add("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""), "Departamento");
            exibeLista();
        }


        protected void gridMercadorias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            fornecedorDAO obj = (fornecedorDAO)Session["fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", "")];
            FornecedorMercadoria prod = obj.mercadoria(index);
            if (e.CommandName.Equals("Excluir"))
            {
                Label6.Text = "Tem Certeza que gostaria de Excluir o Produto ? <br>" +
                    "PLU:" + prod.plu + " REF:" + prod.codigo_referencia + " <br> DESC:" + prod.Descricao;

                Session.Remove("produtoExcluir" + urlSessao());
                Session.Add("produtoExcluir" + urlSessao(), prod);
                modalExcluirProduto.Show();
            }
            else if (e.CommandName.Equals("Editar"))
            {
                lblErroDetalhesProduto.Text = "";
                txtPLU_produto.Text = prod.plu;
                txtEAN_produto.Text = prod.ean;
                txtReferencia_produto.Text = prod.codigo_referencia;
                txtDescricao_produto.Text = prod.Descricao;
                txtDescricao_NF_produto.Text = prod.Descricao_NF;
                txtEmbalagem_produto.Text = prod.embalagem.ToString();
                txtPrecoCompra_produto.Text = prod.preco_compra.ToString("N2");
                txtPrecoCusto_produto.Text = prod.preco_Custo.ToString("N2");

                txtDescricao_produto.Enabled = false;
                Session.Remove("produtoEditar" + urlSessao());
                Session.Add("produtoEditar" + urlSessao(), prod);

                modalEditarProduto.Show();
                EnabledControls(pnEditarProduto, true);
                txtPLU_produto.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            }
            carregarGrids();
        }


        protected void ImgBtnConfirmaExcluirProduto_Click(object sender, EventArgs e)
        {
            try
            {


                fornecedorDAO obj = (fornecedorDAO)Session["fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", "")];

                FornecedorMercadoria prod = (FornecedorMercadoria)Session["produtoExcluir" + urlSessao()];

                obj.excluiProduto(prod);
                modalExcluirProduto.Hide();
                Session.Remove("produtoExcluir" + urlSessao());
                carregarGrids();
                lblError.Text = "Produto Excluido com Sucesso!";
                lblError.ForeColor = System.Drawing.Color.Blue;
            }
            catch (Exception err)
            {

                lblError.Text = "Erro ao Excluir o produto:" + err.Message;

                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }
        protected void ImgBtnCancelaExcluirProduto_Click(object sender, EventArgs e)
        {
            Session.Remove("produtoExcluir" + urlSessao());
            modalExcluirProduto.Hide();
            carregarGrids();
        }
        protected void imgBtnSalvarEditarProduto_Click(object sender, EventArgs e)
        {
            try
            {



                fornecedorDAO obj = (fornecedorDAO)Session["fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", "")];
                FornecedorMercadoria prod = (FornecedorMercadoria)Session["produtoEditar" + urlSessao()];


                if (prod != null)
                {
                    prod.plu = txtPLU_produto.Text;
                    prod.ean = txtEAN_produto.Text;
                    prod.codigo_referencia = txtReferencia_produto.Text;
                    prod.Descricao = txtDescricao_produto.Text;
                    prod.Descricao_NF = txtDescricao_NF_produto.Text;
                    prod.embalagem = Funcoes.intTry(txtEmbalagem_produto.Text);
                    prod.preco_compra = Funcoes.decTry(txtPrecoCompra_produto.Text);
                    prod.preco_Custo = Funcoes.decTry(txtPrecoCusto_produto.Text);
                }
                obj.AddProduto(prod);
                modalEditarProduto.Hide();
                Session.Remove("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""));
                Session.Add("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""), obj);
                carregarGrids();
            }
            catch (Exception err)
            {

                modalEditarProduto.Show();
                lblErroDetalhesProduto.Text = err.Message;
            }

        }

        protected void imgBtnCancelaEditarProduto_Click(object sender, EventArgs e)
        {
            modalEditarProduto.Hide();
            carregarGrids();
        }

        protected void txtPLU_produto_TextChanged(object sender, EventArgs e)
        {
            txtDescricao_produto.Text = Conexao.retornaUmValor("Select Descricao from mercadoria where plu ='" + txtPLU_produto.Text + "'", null);
            modalEditarProduto.Show();
            txtDescricao_produto.Enabled = false;
            txtEAN_produto.Focus();
            carregarGrids();
        }

        protected void imgPOG_click(object s, EventArgs e)
        {
            modalEditarProduto.Show();
            carregarGrids();

        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void produtoDetalhes(String plu)
        {


        }
        protected void btnAddOcorrencia_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                fornecedorDAO obj = (fornecedorDAO)Session["fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", "")];

                if (obj.editadoOcorrencia)
                {
                    obj.atualizarOcorrenciaDT(txtOcorrencia.Text, ddStatus.Text);
                    btnAddOcorrencia.ImageUrl = "~/img/add.png";
                    obj.editadoOcorrencia = false;
                }
                else
                {
                    obj.addOcorrencia(txtOcorrencia.Text);

                    txtOcorrencia.Text = "";
                }
                gridOcorrencia.DataSource = obj.ocorrencia;
                gridOcorrencia.DataBind();

                Session.Remove("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""));
                Session.Add("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""), obj);

            }
            catch (Exception err)
            {
                lblError.Text = err.Message;
                carregarGrids();
            }

        }

        protected void gridOcorrencia_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);

            fornecedorDAO obj = (fornecedorDAO)Session["fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", "")];
            gridOcorrencia.DataSource = obj.ocorrencia;
            gridOcorrencia.DataBind();


            if (e.CommandName.Equals("Alterar"))
            {
                if (gridOcorrencia.Rows[index].Cells[3].Text.Equals("FECHADO"))
                {
                    return;
                }

                btnAddOcorrencia.ImageUrl = "~/img/confirm.png";
                obj.editadoOcorrencia = true;

                obj.dataAlteracaoOcorrencia = gridOcorrencia.Rows[index].Cells[1].Text;
                txtOcorrencia.Text = gridOcorrencia.Rows[index].Cells[2].Text;
                ddStatus.Text = gridOcorrencia.Rows[index].Cells[3].Text;

                Session.Remove("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""));
                Session.Add("fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", ""), obj);

            }
            else
            {
                //fornecedorDAO obj = (fornecedorDAO)Session["fornecedor" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", "")];
                //obj. removeItens(index);
                carregarDados();
            }

        }

        protected void gridOcorrencia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells[3].Text.Equals("FECHADO"))
            {
                e.Row.Cells[0].Enabled = false;
            }
        }
    }
}