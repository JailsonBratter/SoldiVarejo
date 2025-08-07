using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using System.Data.SqlClient;
using visualSysWeb.code;
using senha;
using System.Collections;
using System.IO;
using visualSysWeb.modulos.Cadastro.dao;
using System.Security.Cryptography.Xml;
using visualSysWeb.modulos.Cadastro.code;

namespace visualSysWeb.Cadastro
{
    public partial class ClienteDetalhes : visualSysWeb.code.PagePadrao
    {
        //private static bool campo = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            ClienteDAO obj;
            if (!IsPostBack)
            {
                Menus mn = (Menus)Session["menu"];
                try
                {


                    if (mn != null && mn.moduloAtivo("PetShop"))
                    {
                        tabPet.Visible = true;
                    }
                    else
                    {
                        tabPet.Visible = false;
                    }
                }
                catch (Exception err)
                {

                    lblError.Text = err.Message;
                }
            }
            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    obj = new ClienteDAO(usr);
                    Session.Remove("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
                    status = "incluir";
                }
                habilitarCampos(true);
                //EnabledControls(conteudo, true);
                //EnabledControls(cabecalho, true);


            }
            else
            {
                if (Request.Params["cod"] != null)
                {
                    try
                    {
                        if (!IsPostBack)
                        {
                            String clie = Request.Params["cod"].ToString();
                            status = "visualizar";
                            obj = new ClienteDAO(clie, usr);

                            //campo = false;
                            Session.Remove("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                            Session.Add("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);


                            carregarDados(obj);


                        }
                        if (status.Equals("visualizar"))
                        {
                            habilitarCampos(false);
                            //EnabledControls(conteudo, false);
                            //EnabledControls(cabecalho, false);
                        }
                        else
                        {
                            habilitarCampos(true);
                            //EnabledControls(conteudo, true);
                            //EnabledControls(cabecalho, true);
                        }

                    }
                    catch (Exception err)
                    {

                        lblError.Text = err.Message;
                    }
                }
            }

            carregabtn(pnBtn);
            formatarCampos();


        }


        private void habilitarCampos(bool enable)
        {
            EnabledControls(conteudo, enable);
            EnabledControls(cabecalho, enable);
            EnabledControls(PnPets, enable);
            User usr = (User)Session["User"];

            if (!usr.adm())
            {
                TabCadastro.Visible = usr.telaPermissao("Cadastro");
                TabAdicionais.Visible = usr.telaPermissao("Adicionais");
                TabBancarias.Visible = usr.telaPermissao("Bancarias");
                TabEntregas.Visible = usr.telaPermissao("Entregas");
                TabPagamentos.Visible = usr.telaPermissao("Pagamentos");

            }
            txtDiasPagamentos.Enabled = true;
            txtDiasPagamentos.BackColor = System.Drawing.Color.White;
            ddlStatus.Enabled = true;
            ddlStatus.BackColor = System.Drawing.Color.White;
            //** Comentando trecho abaixo para não mais gerar seguencia. Torna-se desnecessário esta rotina
            //ImgBtnProximaSequencia.Visible = status.Equals("incluir");
            ImgBtnProximaSequencia.Visible = false;
        }

        private void formatarCampos()
        {
            txtCEP.Attributes.Add("OnKeyUp", "javascript:return formataCEP(this,event);");
            txtCep_ent.Attributes.Add("OnKeyUp", "javascript:return formataCEP(this,event);");
            txtCepClienteEntrega.Attributes.Add("OnKeyUp", "javascript:return formataCEP(this,event);");
            txtCepEntrega.Attributes.Add("OnKeyUp", "javascript:return formataCEP(this,event);");

            if (chkPessoa_Juridica.Checked)
            {
                txtCNPJ.Attributes.Add("OnKeyUp", "javascript:return formataCNPJ(this,event);");
                txtCNPJ.MaxLength = 18;

            }
            else
            {
                txtCNPJ.Attributes.Add("OnKeyUp", "javascript:return formataCPF(this,event);");
                txtCNPJ.MaxLength = 14;

            }


            txtData_Nascimento.Attributes.Add("OnKeyUp", "javascript:return formataData(this,event);");
            txtDataVencimentoChave.Attributes.Add("OnKeyUp", "javascript:return formataData(this,event);");
            txtDataVencimentoChave.Enabled = true;
            txtDataVencimentoChave.BackColor = System.Drawing.Color.White;
            txtChaveAtual.Enabled = false;
            txtTelefone.Attributes.Add("OnKeyUp", "javascript:return formataTelefone(this,event);");
            imgDtVencimentoChave.Visible = true;
            User usr = (User)Session["User"];
            if (usr != null)
            {
                if (!IsPostBack)
                {

                    try
                    {

                        String kSenha = System.Web.Configuration.WebConfigurationManager.AppSettings["FLCRT"].ToString();
                        bool GeraSenha = kSenha.ToUpper().Equals("9.2");
                        if (GeraSenha)
                        {
                            try
                            {
                                txtDataVencimentoChave.Enabled = true;
                                imgDtVencimentoChave.Visible = true;
                                TabGerarSenhaAcesso.Visible = true;
                                String chvSenha = Conexao.retornaUmValor("select chave_senha from cliente where cnpj='" + txtCNPJ.Text + "'", null);
                                txtChaveAtual.Text = chvSenha;
                                Senha sn = new Senha(chvSenha);
                                txtDataVencimentoChave.Text = sn.dataValidade.ToString("dd/MM/yyyy");
                                chkHabilitaF9.Checked = sn.ModuloAcesso("F9");
                                Menus mn = new Menus(new User(), Server.MapPath(""));
                                List<ModuloSenha> modulos = mn.modulosSenha();
                                foreach (ModuloSenha m in modulos)
                                {
                                    m.Acesso = sn.ModuloAcesso(m.Nome);
                                }
                                gridModulos.DataSource = modulos;
                                gridModulos.DataBind();
                            }
                            catch (Exception)
                            { }
                        }
                        else
                        {
                            TabGerarSenhaAcesso.Visible = false;
                            txtChaveAtual.Text = "";
                            txtDataVencimentoChave.Text = "";
                        }


                    }
                    catch (Exception)
                    {
                        TabGerarSenhaAcesso.Visible = false;
                        txtChaveAtual.Text = "";
                        txtDataVencimentoChave.Text = "";
                    }

                }
            }


            camposnumericos();
        }


        private void camposnumericos()
        {
            ArrayList campos = new ArrayList();
            campos.Add("txtRenda_Mensal");
            campos.Add("txtLimite_Credito");

            FormataCamposNumericos(campos, conteudo);
            FormataCamposNumericos(campos, cabecalho);

            ArrayList camposInteiros = new ArrayList();
            camposInteiros.Add("txtendereco_nro");
            camposInteiros.Add("txtendereco_ent_nro");
            camposInteiros.Add("txtNumeroBanco");
            camposInteiros.Add("txtAgencia");
            camposInteiros.Add("txtConta");


            FormataCamposInteiros(camposInteiros, conteudo);
            FormataCamposInteiros(camposInteiros, cabecalho);

        }


        private void carregarDadosObj(ClienteDAO obj)
        {
            obj.Codigo_Cliente = "";
            obj.Codigo_Cliente = txtCodigo_Cliente.Text.ToString();
            obj.Nome_Cliente = Funcoes.RemoverAcentos(txtNome_Cliente.Text.ToString());

            obj.Situacao = txtSituacao.Text.ToString();
            obj.Endereco = Funcoes.RemoverAcentos(txtEndereco.Text);
            obj.Estado_civil = txtEstado_civil.Text;
            obj.CEP = txtCEP.Text;
            obj.Bairro = Funcoes.RemoverAcentos(txtBairro.Text);
            obj.Cidade = Funcoes.RemoverAcentos(txtCidade.Text);
            obj.UF = txtUF.Text;
            obj.CNPJ = txtCNPJ.Text;
            obj.IE = txtIE.Text;
            obj.Data_Nascimento = (txtData_Nascimento.Text.Equals("") ? new DateTime() : DateTime.Parse(txtData_Nascimento.Text));
            obj.Naturalidade = Funcoes.RemoverAcentos(txtNaturalidade.Text);
            obj.Nome_conjuge = Funcoes.RemoverAcentos(txtNome_conjuge.Text);
            obj.Contato = Funcoes.RemoverAcentos(txtContato.Text);
            obj.Renda_Mensal = Decimal.Parse(txtRenda_Mensal.Text.Equals("") ? "0.0" : txtRenda_Mensal.Text);
            obj.Pessoa_Juridica = chkPessoa_Juridica.Checked;
            obj.Limite_Credito = Decimal.Parse(txtLimite_Credito.Text.Equals("") ? "0.0" : txtLimite_Credito.Text);
            obj.Utilizado = Funcoes.decTry(txtTotalUtlizado.Text);
            obj.ICM_Isento = chkICM_Isento.Checked;
            obj.Historico = Funcoes.RemoverAcentos(txtHistorico.Text);

            int.TryParse(ddlTipoIE.SelectedValue, out obj.indIEDest);

            obj.ativa_terceiro_preco = chkTerceiroPreco.Checked;
            obj.Codigo_tabela = txtCodigo_tabela.Text;
            obj.vendedor = txtvendedor.Text;
            obj.nome_fantasia = Funcoes.RemoverAcentos(txtnome_fantasia.Text);
            obj.Endereco_ent = txtEndereco_ent.Text;
            obj.Cep_ent = txtCep_ent.Text;
            obj.Bairro_ent = Funcoes.RemoverAcentos(txtBairro_ent.Text);
            obj.Cidade_ent = Funcoes.RemoverAcentos(txtCidade_ent.Text);
            obj.Uf_ent = txtUf_ent.Text;
            obj.endereco_nro = txtendereco_nro.Text;
            obj.complemento_end = Funcoes.RemoverAcentos(txtcomplemento_end.Text);
            obj.endereco_ent_nro = txtendereco_ent_nro.Text;
            obj.habilita_f9 = chkHabilitaF9.Checked;
            obj.Iva_descricao = ChkIvaDescricao.Checked;
            obj.opt_Simples_Nac = chkOpt_Simples_nac.Checked;
            obj.complemento_ent = txtComplemento_ent.Text;
            obj.conta_assinada = chkContaAssinada.Checked;
            obj.grupoEmpresa = Funcoes.intTry(txtGrupoEmpresa.Text);
            obj.nomeGrupoEmpresa = txtNomeGrupoEmpresa.Text;
            obj.conta_contabil_credito = txtContaContabilCredito.Text;
            obj.conta_contabil_debito = txtContaContabilDebito.Text;

            Session.Remove("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);

        }
        private void carregarDados(ClienteDAO obj)
        {

            txtCodigo_Cliente.Text = obj.Codigo_Cliente.ToString();
            txtUsuario.Text = obj.Usuario;
            txtUsuarioAlteracao.Text = obj.UsuarioAlteracao;
            txtNome_Cliente.Text = obj.Nome_Cliente.ToString();
            txtSituacao.Text = obj.Situacao.ToString();
            txtEndereco.Text = obj.Endereco.ToString();
            txtEstado_civil.Text = obj.Estado_civil.ToString();
            txtCEP.Text = obj.CEP.ToString();
            txtBairro.Text = obj.Bairro.ToString();
            txtCidade.Text = obj.Cidade.ToString();
            txtUF.Text = obj.UF.ToString();
            txtCNPJ.Text = obj.CNPJ.ToString();
            txtIE.Text = obj.IE.ToString();
            txtData_Nascimento.Text = obj.Data_NascimentoBr();
            txtNaturalidade.Text = obj.Naturalidade.ToString();
            txtNome_conjuge.Text = obj.Nome_conjuge.ToString();
            txtContato.Text = obj.Contato.ToString();
            txtRenda_Mensal.Text = string.Format("{0:0,0.00}", obj.Renda_Mensal);
            chkPessoa_Juridica.Checked = obj.Pessoa_Juridica;
            if (chkPessoa_Juridica.Checked)
            {
                lblCpf.Text = "CNPJ";
                lblRg.Text = "IE";
            }
            else
            {
                lblCpf.Text = "CPF";
                lblRg.Text = "RG";

            }
            chkTerceiroPreco.Checked = obj.ativa_terceiro_preco;
            ddlTipoIE.SelectedValue = (obj.indIEDest == 0 ? "9" : obj.indIEDest.ToString());
            txtLimite_Credito.Text = obj.Limite_Credito.ToString("N2");
            txtMaiorCompra.Text = obj.maiorCompra().ToString("N2");
            txtDataUltimaCompra.Text = obj.UltimaCompra();
            Decimal tAtrasados = obj.totalAtrasados();
            txtAtrasados.Text = tAtrasados.ToString("N2");
            Decimal tAbertos = obj.totalAbertos();
            txtAberto.Text = tAbertos.ToString("N2");
            txtConcluido.Text = obj.totalConcluido();


            Decimal vUtilizado = (tAbertos + tAtrasados);
            if (vUtilizado > 0)
            {
                txtUtilizadoReceber.Text = (vUtilizado).ToString("N2");
            }
            else
            {
                txtUtilizadoReceber.Text = "0,00";
            }

            txtUtilizado.Text = obj.cadernetaSaldo().ToString("N2");
            txtTotalUtlizado.Text = (vUtilizado + obj.cadernetaSaldo()).ToString("N2");

            chkICM_Isento.Checked = obj.ICM_Isento;
            txtHistorico.Text = obj.Historico.ToString();
            txtdata_cadastro.Text = obj.data_cadastroBr();

            txtCodigo_tabela.Text = obj.Codigo_tabela.ToString();
            txtvendedor.Text = obj.vendedor.ToString();
            txtnome_fantasia.Text = obj.nome_fantasia.ToString();
            txtEndereco_ent.Text = obj.Endereco_ent.ToString();
            txtCep_ent.Text = obj.Cep_ent.ToString();
            txtBairro_ent.Text = obj.Bairro_ent.ToString();
            txtCidade_ent.Text = obj.Cidade_ent.ToString();
            txtUf_ent.Text = obj.Uf_ent.ToString();
            txtendereco_nro.Text = obj.endereco_nro.ToString();
            txtcomplemento_end.Text = obj.complemento_end.ToString();
            txtendereco_ent_nro.Text = obj.endereco_ent_nro.ToString();
            ChkIvaDescricao.Checked = obj.Iva_descricao;
            chkHabilitaF9.Checked = obj.habilita_f9;
            chkOpt_Simples_nac.Checked = obj.opt_Simples_Nac;
            txtComplemento_ent.Text = obj.complemento_ent;
            chkContaAssinada.Checked = obj.conta_assinada;

            txtGrupoEmpresa.Text = obj.grupoEmpresa.ToString();
            txtNomeGrupoEmpresa.Text = obj.nomeGrupoEmpresa;

            atualizaGrids();
            pessoaJurFisica();

            txtContaContabilCredito.Text = obj.conta_contabil_credito;
            txtContaContabilDebito.Text = obj.conta_contabil_debito;

            Session.Remove("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);

        }



        private void limparCampos()
        {
            LimparCampos(cabecalho);
            LimparCampos(conteudo);

        }

        protected void gridPagamenos_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gridPagamenos.Rows)
            {


                DateTime dtVenc = new DateTime();
                DateTime.TryParse(row.Cells[3].Text, out dtVenc);
                TimeSpan dias = DateTime.Now.Subtract(dtVenc);

                if (dias.Days > 0 && (row.Cells[5].Text.Equals("VENCIDO") || row.Cells[5].Text.Equals("ABERTO")))
                {
                    row.CssClass = "linhaInativo";
                    row.ForeColor = System.Drawing.Color.Red;
                }


            }
        }

        protected void txtDiasPagamentos_TextChanged(object sender, EventArgs e)
        {
            atualizaGrids();
        }

        private void atualizaGrids()
        {
            ClienteDAO obj = (ClienteDAO)Session["cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            gridbanco.DataSource = obj.clienteBanco;
            gridbanco.DataBind();
            gridEnderecosEntrega.DataSource = obj.entregacozinha;
            gridEnderecosEntrega.DataBind();
            gridMeioComunicacao.DataSource = obj.meiosComunicacao;
            gridMeioComunicacao.DataBind();


            gridPagamenos.DataSource = obj.getPagamentos(ddlStatus.Text, Funcoes.intTry(txtDiasPagamentos.Text));
            gridPagamenos.DataBind();

            gridCaderneta.DataSource = obj.getCaderneta(Funcoes.intTry(txtDiasPagamentos.Text));
            gridCaderneta.DataBind();



            gridLugarEntrega.DataSource = obj.localEntrega;
            gridLugarEntrega.DataBind();



            gridPet.DataSource = obj.tbPets();
            gridPet.DataBind();


            gridFidelidade.DataSource = obj.ArrFidelidade;
            gridFidelidade.DataBind();

            divNaoRegistrado.Visible = (obj.ArrFidelidade.Count == 0);

            txtTotalPontos.Text = obj.ArrFidelidade.Sum(p => p.Qtde_pontos).ToString("N0");

        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClienteDAO obj = (ClienteDAO)Session["cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            gridPagamenos.DataSource = obj.getPagamentos(ddlStatus.Text, Funcoes.intTry(txtDiasPagamentos.Text));
            gridPagamenos.DataBind();

        }
        protected override bool campoObrigatorio(Control campo)
        {
            String[] campos = { "txtEndereco",
                           "txtendereco_nro",
                           "txtBairro",
                           "txtCidade",
                           "txtUF",
                           "txtCEP",
                           "txtNome_Cliente"
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

        protected void chkPessoa_Juridica_CheckedChanged(object sender, EventArgs e)
        {
            pessoaJurFisica();
        }

        protected void pessoaJurFisica()
        {
            if (chkPessoa_Juridica.Checked)
            {
                lblCpf.Text = "CNPJ";
                lblRg.Text = "IE";
            }
            else
            {
                lblCpf.Text = "CPF";
                lblRg.Text = "RG";

            }
        }






        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("ClienteDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            editar(pnBtn);
            //campo = true;
            habilitarCampos(true);
            //EnabledControls(cabecalho, true);
            //EnabledControls(conteudo, true);


        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Clientes.aspx");
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            modalPnConfirma.Show();
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
            divAddTabela.Visible = false;
            divAddGrupoEmpresa.Visible = false;
            lblErroPesquisa.Text = "";
            String sqlLista = "";
            String or = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            switch (or)
            {
                case "txtCodigo_tabela":
                    lbllista.Text = "Escolha uma Tabela";
                    sqlLista = "select codigo_tabela from tabela_preco where codigo_tabela like '%" + TxtPesquisaLista.Text + "%'";
                    //divAddTabela.Visible = true;
                    break;
                case "txtvendedor":
                    lbllista.Text = "Escolha um Vendedor";
                    sqlLista = "select Nome from funcionario where funcao = 'VENDEDOR' and nome like '%" + TxtPesquisaLista.Text + "%';";

                    break;
                case "txtSituacao":
                    lbllista.Text = "Escolha a Situação do cliente";
                    sqlLista = "SELECT situacao FROM  Situacao_cliente;";

                    break;
                case "txtMeioComunicacao":
                    lbllista.Text = "Escolha um Meio de comunicação";
                    sqlLista = "SELECT Meio_comunicacao FROM  meio_comunicacao where meio_comunicacao like '%" + TxtPesquisaLista.Text + "%';";

                    break;
                case "txtNumeroBanco":
                    lbllista.Text = "Escolha um Banco";
                    sqlLista = "SELECT numero_banco,nome_banco FROM  banco where numero_banco like '%" + TxtPesquisaLista.Text + "%' or nome_banco like '%" + TxtPesquisaLista.Text + "%' ;";

                    break;
                case "txtCidade":
                    lbllista.Text = "Escolha uma Cidade ";
                    sqlLista = "Select munic Codigo , upper(nome_munic) municipio from unidade_federacao where nome_munic like '%" + TxtPesquisaLista.Text + "%' or munic like'%" + TxtPesquisaLista.Text + "%'";
                    break;
                case "txtCEP":
                    lbllista.Text = "Escolha o CEP";
                    sqlLista = "select top 500 * from CEP_Brasil where (cep+Logradouro+Bairro+Cidade+UF) like '%" + TxtPesquisaLista.Text.Replace(" ", "%") + "%'";
                    break;
                case "txtGrupoEmpresa":
                    lbllista.Text = "Escolha o Grupo";
                    sqlLista = " Select id, Grupo from cliente_grupo where grupo like '%" + TxtPesquisaLista.Text + "%'";
                    divAddGrupoEmpresa.Visible = true;
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

        protected void btnFechar_Click(object sender, ImageClickEventArgs e)
        {
            String selecionado = ListaSelecionada(1);

            if (!selecionado.Equals("") && !selecionado.Equals("------"))
            {
                ImageButton btn = (ImageButton)sender;
                String listaAtual = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                Session.Remove("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));

                if (listaAtual.Equals("txtCodigo_tabela"))
                {
                    txtCodigo_tabela.Text = ListaSelecionada(1);
                    if (!txtCodigo_tabela.Text.Trim().Equals(""))
                    {
                        chkTerceiroPreco.Checked = false;
                    }

                }
                else if (listaAtual.Equals("txtvendedor"))
                {
                    txtvendedor.Text = ListaSelecionada(1);
                }
                else if (listaAtual.Equals("txtSituacao"))
                {
                    txtSituacao.Text = ListaSelecionada(1);
                }
                else if (listaAtual.Equals("txtMeioComunicacao"))
                {
                    txtMeioComunicacao.Text = ListaSelecionada(1);
                }
                else if (listaAtual.Equals("txtNumeroBanco"))
                {
                    txtNumeroBanco.Text = ListaSelecionada(1);
                    txtNomeBanco.Text = ListaSelecionada(2);
                }
                else if (listaAtual.Equals("txtCidade"))
                {
                    txtCidade.Text = ListaSelecionada(2);
                }
                else if (listaAtual.Equals("txtCEP"))
                {
                    txtCEP.Text = ListaSelecionada(5);
                    txtEndereco.Text = ListaSelecionada(1);
                    txtBairro.Text = ListaSelecionada(2);
                    txtCidade.Text = ListaSelecionada(3);
                    txtUF.Text = ListaSelecionada(4);
                    txtendereco_nro.Focus();
                }
                else if (listaAtual.Equals("txtGrupoEmpresa"))
                {
                    txtGrupoEmpresa.Text = ListaSelecionada(1);
                    txtNomeGrupoEmpresa.Text = ListaSelecionada(2);

                }



                ModalFundo.Hide();
            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                ModalFundo.Show();
            }
        }
        protected void GridLista_SelectedIndexChanged(object sender, EventArgs e)
        {


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
        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            ModalFundo.Hide();
        }

        protected bool validaCamposObrigatorios()
        {
            //validarCnpjCpf();

            txtNome_Cliente.BackColor = System.Drawing.Color.White;
            txtCidade.BackColor = System.Drawing.Color.White;
            if (txtNome_Cliente.Text.Length > 50)
            {
                lblError.Text = "Nome do Cliente não pode conter mais de 50 Caracters!";
                txtNome_Cliente.BackColor = System.Drawing.Color.Red;
                return false;
            }
            String cidade = Conexao.retornaUmValor("Select nome_munic from unidade_federacao where nome_munic='" + txtCidade.Text + "'", new User());
            if (cidade.Equals(""))
            {
                lblError.Text = "Cidade não cadastrada! ";
                txtCidade.BackColor = System.Drawing.Color.Red;
                return false;

            }

            if (validaCampos(cabecalho) && validaCampos(conteudo))
                return true;
            else
                return false;

        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                if (validaCamposObrigatorios())
                {
                    ClienteDAO obj = (ClienteDAO)Session["cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                    carregarDadosObj(obj);
                    obj.salvar(status.Equals("incluir"));
                    carregarDados(obj);

                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;

                    EnabledControls(conteudo, false);
                    EnabledControls(cabecalho, false);


                    // carregarDados(obj); *verificar utilidade;

                    visualizar(pnBtn);
                }
                else
                {
                    lblError.Text += "Campo Obrigatorio não preenchido ou preenchido de forma incorreta";
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
            Response.Redirect("Clientes.aspx");
        }

        protected override bool campoDesabilitado(Control campo)
        {


            String[] campos = { (status.Equals("editar")? "txtCodigo_Cliente":""),
                           "txtdata_cadastro",
                           "txtUtilizado",
                           "txtCodigoPet",
                           "txtNomeGrupoEmpresa",
                           "txtDataUltimaCompra",
                           "txtMaiorCompra",
                           "txtConcluido",
                           "txtAberto",
                           "txtAtrasados",
                           "txtSaldoCredito",
                           "txtUtilizado",
                           "txtTotalUtlizado",
                           "txtUsuario",
                           "txtUsuarioAlteracao"

                          };

            for (int i = 0; i < campos.Length; i++)
            {
                if (campo.ClientID.IndexOf(campos[i]) > 0)
                {

                    return true;
                }
            }
            return false;
        }


        protected void btnDeleteComunicacao_click(object sender, ImageClickEventArgs e)
        {
            ClienteDAO obj = (ClienteDAO)Session["cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            ImageButton btn = (ImageButton)sender;
            obj.deleteMeioComunicacao(btn.ID.Substring(10));
            carregarDados(obj);

        }
        protected void btnAddComunicacao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                txtIdMeioComunicacao.BackColor = System.Drawing.Color.White;

                int exist = 0;
                int.TryParse(Conexao.retornaUmValor("Select COUNT(*) from meio_comunicacao where Meio_Comunicacao='" + txtMeioComunicacao.Text + "'", null), out exist);
                if (exist == 0)
                {
                    txtMeioComunicacao.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Meio de comunicação não existe! Escolha um meio válido");
                }


                ClienteDAO obj = (ClienteDAO)Session["cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                obj.addMeioComunicacao(txtMeioComunicacao.Text, txtIdMeioComunicacao.Text, txtContatoComunicacao.Text);

                txtMeioComunicacao.Text = "";
                txtIdMeioComunicacao.Text = "";
                txtContatoComunicacao.Text = "";
                gridMeioComunicacao.DataSource = obj.meiosComunicacao;
                gridMeioComunicacao.DataBind();

                Session.Remove("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
            }
            catch (Exception err)
            {

                lblError.Text = err.Message;
                if (err.Message.IndexOf("ID do Meio de Comunicação") >= 0)
                    txtIdMeioComunicacao.BackColor = System.Drawing.Color.Red;

            }

        }


        protected void gridMeioComunicacao_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void gridMeioComunicacao_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            ClienteDAO obj = (ClienteDAO)Session["cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gridMeioComunicacao.Rows[index];

            obj.deleteMeioComunicacao(row.Cells[2].Text.Replace("&nbsp;", ""));

            Session.Remove("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);

            gridMeioComunicacao.DataSource = obj.meiosComunicacao;
            gridMeioComunicacao.DataBind();
        }

        protected void gridbanco_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void gridbanco_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            ClienteDAO obj = (ClienteDAO)Session["cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gridbanco.Rows[index];

            obj.deleteClienteBanco(row.Cells[1].Text, row.Cells[3].Text, row.Cells[4].Text);
            gridbanco.DataSource = obj.clienteBanco;
            gridbanco.DataBind();
            Session.Remove("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);

        }



        protected void imgAddBanco_Click(object sender, ImageClickEventArgs e)
        {

            bool validado = true;
            txtNumeroBanco.BackColor = System.Drawing.Color.White;
            txtAgencia.BackColor = System.Drawing.Color.White;
            txtConta.BackColor = System.Drawing.Color.White;
            lblError.Text = "";
            if (txtNumeroBanco.Text.Trim().Equals(""))
            {
                txtNumeroBanco.BackColor = System.Drawing.Color.Red;
                validado = false;
            }
            if (txtAgencia.Text.Trim().Equals(""))
            {
                txtAgencia.BackColor = System.Drawing.Color.Red;
                validado = false;
            }
            if (txtConta.Text.Trim().Equals(""))
            {
                txtConta.BackColor = System.Drawing.Color.Red;
                validado = false;
            }

            if (validado)
            {

                ClienteDAO obj = (ClienteDAO)Session["cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                obj.addClienteBanco(txtNumeroBanco.Text, txtNomeBanco.Text, txtAgencia.Text, txtConta.Text, txtTelefone.Text, txtContatoBanco.Text);



                txtNumeroBanco.Text = "";
                txtNomeBanco.Text = "";
                txtAgencia.Text = "";
                txtConta.Text = "";
                txtTelefone.Text = "";
                txtContatoBanco.Text = "";

                Session.Remove("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                Session.Add("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
                atualizaGrids();

            }
            else
            {
                lblError.Text = "Não é possivel Incluir as Informações Bancarias Campos Obrigatorios não preenchidos";
            }



        }


        protected void gridLugarEntrega_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void gridLugarEntrega_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            ClienteDAO obj = (ClienteDAO)Session["cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gridbanco.Rows[index];

            obj.deletelocalEntrega(row.Cells[1].Text);
            gridbanco.DataSource = obj.clienteBanco;
            gridbanco.DataBind();
            Session.Remove("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);

        }
        protected void imgAddClienteEntrega_Click(object sender, ImageClickEventArgs e)
        {
            ClienteDAO obj = (ClienteDAO)Session["cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            obj.addLocalEntrega(txtLugarClienteEntrega.Text, txtEnderecoClienteEntrega.Text, txtUfClienteEntrega.Text, txtCepClienteEntrega.Text, txtCidadeClienteEntrega.Text);

            gridLugarEntrega.DataSource = obj.localEntrega;
            gridLugarEntrega.DataBind();

            txtLugarClienteEntrega.Text = "";
            txtEnderecoClienteEntrega.Text = "";
            txtUfClienteEntrega.Text = "";
            txtCepClienteEntrega.Text = "";
            txtCidadeClienteEntrega.Text = "";

            Session.Remove("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);

        }

        protected void gridEnderecosEntrega_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void gridEnderecosEntrega_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            ClienteDAO obj = (ClienteDAO)Session["cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gridEnderecosEntrega.Rows[index];

            obj.deleteEntregaCozinha(row.Cells[1].Text, row.Cells[2].Text);
            gridEnderecosEntrega.DataSource = obj.entregacozinha;
            gridEnderecosEntrega.DataBind();

            Session.Remove("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);

        }
        protected void imgAddEnderecosEntrega_Click(object sender, ImageClickEventArgs e)
        {
            ClienteDAO obj = (ClienteDAO)Session["cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            obj.addEntregaCozinha(txtEnderecoEntrega.Text, txtNumeroEntrega.Text, txtBairroEntrega.Text, txtCidadeEntrega.Text, txtUFEntrega.Text, txtCepEntrega.Text);

            gridEnderecosEntrega.DataSource = obj.entregacozinha;
            gridEnderecosEntrega.DataBind();

            txtEnderecoEntrega.Text = "";
            txtNumeroEntrega.Text = "";
            txtBairroEntrega.Text = "";
            txtCidadeEntrega.Text = "";
            txtUFEntrega.Text = "";
            txtCepEntrega.Text = "";

            Session.Remove("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);


        }

        protected void TxtPesquisaLista_TextChanged(object sender, EventArgs e)
        {

            exibeLista();

        }
        protected void txtCNPJ_TextChanged(object sender, EventArgs e)
        {
            lblError.Text = "";
            try
            {

                validarCnpjCpf();

            }
            catch (Exception err)
            {

                lblError.Text = err.Message;
            }
        }

        private void validarCnpjCpf()
        {
            bool validar = false;
            validar = (Funcoes.IsCpf(txtCNPJ.Text) || Funcoes.IsCnpj(txtCNPJ.Text));

            if (!validar)
            {
                txtCNPJ.BackColor = System.Drawing.Color.Red;
                throw new Exception("CNPJ / CPF invalido ");
            }

            if (int.Parse(Conexao.retornaUmValor("select COUNT(*) from cliente where replace(replace(replace(cnpj,'.',''),'-',''),'/','')='" + txtCNPJ.Text.Replace(".", "").Replace("-", "").Replace("/", "") + "'", null)) > 0)
            {
                txtCNPJ.BackColor = System.Drawing.Color.Red;
                throw new Exception("CNPJ /CPF JÁ ESTA CADASTRADO");
            }

        }

        protected void btnGerarNovaSenha_Click(object sender, EventArgs e)
        {

            lblError.Text = "";
            txtDataVencimentoChave.BackColor = System.Drawing.Color.White;

            txtCNPJ.BackColor = txtIE.BackColor;
            try
            {
                if (txtCNPJ.Text.Trim().Equals("") || !Funcoes.IsCnpj(txtCNPJ.Text))
                {
                    txtCNPJ.BackColor = System.Drawing.Color.Red;
                    throw new Exception("CNPJ INVÁLIDO");
                }


                DateTime dtVencimento;
                try
                {
                    dtVencimento = DateTime.Parse(txtDataVencimentoChave.Text);
                }
                catch (Exception)
                {
                    txtDataVencimentoChave.BackColor = System.Drawing.Color.Red;

                    throw new Exception("DATA DE VENCIMENTO INVÁLIDA");
                }

                if (dtVencimento < DateTime.Now)
                {
                    txtDataVencimentoChave.BackColor = System.Drawing.Color.Red;

                    throw new Exception("A DATA NÃO PODE SER MENOR QUE A DATA ATUAL!");
                }
                txtDataVencimentoChave.Text = dtVencimento.ToString("dd/MM/yyyy");
                List<ModuloSenha> modulos = new List<ModuloSenha>()
; foreach (GridViewRow row in gridModulos.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("chkSelecionaItem");
                    if (chk.Checked)
                    {
                        modulos.Add(new ModuloSenha()
                        {
                            Nome = row.Cells[0].Text,
                            Acesso = true
                        });
                    }
                }

                Senha sn = null;
                if (chkHabilitaF9.Checked)
                {
                    ModuloSenha modulo = new ModuloSenha()
                    {
                        Nome = "F9",
                        Acesso = true
                    };
                    modulos.Add(modulo);

                }

                if (modulos.Count > 0)
                    sn = new Senha(txtCNPJ.Text, dtVencimento, modulos);
                else
                    sn = new Senha(txtCNPJ.Text, dtVencimento);

                txtChaveAtual.Text = sn.chave;
                Conexao.executarSql("update cliente set chave_senha='" + sn.chave + "' where CNPJ='" + txtCNPJ.Text + "'");
            }
            catch (Exception err)
            {

                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;

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
                    txtendereco_nro.Focus();
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
                        txtendereco_nro.Focus();
                    }
                }
            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            modalPnConfirma.Hide();
            EnabledControls(cabecalho, false);
            EnabledControls(conteudo, false);
        }
        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {


                ClienteDAO obj = (ClienteDAO)Session["cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                obj.Exclui();

                limparCampos();
                modalPnConfirma.Hide();
                lblError.Text = "Inativado Com Sucesso";
                lblError.ForeColor = System.Drawing.Color.Red;
                sopesquisa(pnBtn);
                EnabledControls(cabecalho, false);
                EnabledControls(conteudo, false);

            }
            catch (Exception err)
            {
                lblError.Text = "Erro:" + err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

        //ImgBtnAddPet_Click
        protected void ImgBtnAddPet_Click(object sender, ImageClickEventArgs e)
        {
            User usr = (User)Session["User"];
            LimparCampos(PnPets);
            ddlSexo.Text = "";
            cliente_petDAO pet = new cliente_petDAO(usr);
            Session.Remove("itemPet" + urlSessao());
            Session.Add("itemPet" + urlSessao(), pet);
            carregarGridsPet();
            modalPet.Show();
            txtNomePet.Focus();
            txtNomePet.Enabled = true;
            txtNomePet.BackColor = System.Drawing.Color.White;
            atualizarTelaPet();




        }
        public void atualizarTelaPet()
        {
            User usr = (User)Session["User"];
            Conexao.preencherDDL1Branco(ddlCor, "Select cor from cor ", "cor", "cor", usr);
            Conexao.preencherDDL1Branco(ddlPelagem, "Select pelagem from pelagem ", "pelagem", "pelagem", usr);
            Conexao.preencherDDL1Branco(ddlPorte, "Select porte from porte ", "porte", "porte", usr);
            Conexao.preencherDDL1Branco(ddlRaca, "Select raca from raca ", "raca", "raca", usr);
            Conexao.preencherDDL1Branco(ddlEspecie, "Select especie from especie ", "especie", "especie", usr);
            Conexao.preencherDDL1Branco(ddlVacina, "Select vacina from vacina ", "vacina", "vacina", usr);
        }
        //gridPet_RowCommand
        protected void gridPet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            ClienteDAO obj = (ClienteDAO)Session["cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            cliente_petDAO pet = (cliente_petDAO)obj.arrPets[index];
            atualizarTelaPet();

            txtCodigoPet.Text = pet.codigo_pet;
            txtNomePet.Text = pet.Nome_Pet;
            txtNomePet.Enabled = false;
            txtNomePet.BackColor = txtCodigo_Cliente.BackColor;
            ddlSexo.Text = pet.Sexo;
            ddlCor.Text = pet.Cor;
            txtDtNascimentoPet.Text = pet.Data_NascimentoBr();
            ddlPelagem.Text = pet.Pelagem;
            ddlPorte.Text = pet.Porte;
            ddlRaca.Text = pet.Raca;
            txtUltimoCio.Text = pet.Ultimo_CioBr();
            ddlEspecie.Text = pet.Especie;
            txtPedigree.Text = pet.pedigree;

            TxtObservacaoVeterinario.Text = "";
            Session.Remove("itemPet" + urlSessao());
            Session.Add("itemPet" + urlSessao(), pet);
            carregarGridsPet();
            modalPet.Show();


        }
        //gridVacinas_RowCommand
        protected void gridVacinas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            cliente_petDAO pet = (cliente_petDAO)Session["itemPet" + urlSessao()];
            pet.arrVacinas.RemoveAt(index);
            Session.Remove("itemPet" + urlSessao());
            Session.Add("itemPet" + urlSessao(), pet);
            gridVacinas.DataSource = pet.tbVacinas();
            gridVacinas.DataBind();
            modalPet.Show();
        }


        private void carregarDadosPetObj()
        {
            cliente_petDAO pet = (cliente_petDAO)Session["itemPet" + urlSessao()];
            pet.codigo_pet = txtCodigoPet.Text;
            pet.Codigo_Cliente = txtCodigo_Cliente.Text;
            pet.Nome_Pet = txtNomePet.Text;
            pet.Sexo = ddlSexo.Text;
            pet.Cor = ddlCor.Text;
            DateTime.TryParse(txtDtNascimentoPet.Text, out pet.Data_Nascimento);
            pet.Pelagem = ddlPelagem.Text;
            pet.Porte = ddlPorte.Text;
            pet.Raca = ddlRaca.Text;
            DateTime.TryParse(txtUltimoCio.Text, out pet.Ultimo_Cio);
            pet.Especie = ddlEspecie.Text;
            pet.pedigree = txtPedigree.Text;
            gridVacinas.DataSource = pet.tbVacinas();
            gridVacinas.DataBind();
            Session.Remove("itemPet" + urlSessao());
            Session.Add("itemPet" + urlSessao(), pet);

        }
        //btnConfirmaPet_Click
        protected void btnConfirmaPet_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                txtNomePet.BackColor = System.Drawing.Color.White;
                if (txtNomePet.Text.Equals(""))
                {
                    txtNomePet.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Informe o Nome do Pet");
                }

                carregarDadosPetObj();
                ClienteDAO obj = (ClienteDAO)Session["cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                cliente_petDAO pet = (cliente_petDAO)Session["itemPet" + urlSessao()];

                if (txtCodigoPet.Text.Equals(""))
                {
                    obj.addPet(pet);
                }
                else
                {
                    obj.atualizarPet(pet);
                }
                carregarDados(obj);
                modalPet.Hide();
            }
            catch (Exception err)
            {

                lblErroPet.Text = err.Message;
                modalPet.Show();
            }

        }

        protected void btnCancelaPet_Click(object sender, ImageClickEventArgs e)
        {
            modalPet.Hide();
        }

        protected void btnExcluiPet_Click(object sender, ImageClickEventArgs e)
        {
            modalPet.Show();
            modalExcluirPEt.Show();
        }



        protected void imgBtnAddTabela_Click(object sender, ImageClickEventArgs e)
        {
            ModalFundo.Show();
        }


        protected void imgBtnAddVacina_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                bool valida = true;
                ddlVacina.BackColor = System.Drawing.Color.White;
                txtDtVacina.BackColor = System.Drawing.Color.White;


                if (ddlVacina.Text.Trim().Equals(""))
                {
                    ddlVacina.BackColor = System.Drawing.Color.Red;
                    valida = false;
                }

                if (!IsDate(txtDtVacina.Text))
                {

                    txtDtVacina.BackColor = System.Drawing.Color.Red;
                    valida = false;
                }
                else
                {
                    DateTime dt;
                    DateTime.TryParse(txtDtVacina.Text, out dt);
                    DateTime dthj;
                    DateTime.TryParse(DateTime.Now.ToString("dd/MM/yyyy"), out dthj);
                    DateTime dtIni = DateTime.Parse("01/01/1990");


                    if (dt > dthj || dt < dtIni)
                    {
                        txtDtVacina.BackColor = System.Drawing.Color.Red;

                        throw new Exception("Data Inválida ");
                    }


                }
                if (!valida)
                {
                    throw new Exception("Vacina ou data Inválida!");
                }


                Cliente_Pet_VacinaDAO vacina = new Cliente_Pet_VacinaDAO();
                vacina.Vacina = ddlVacina.Text;
                DateTime.TryParse(txtDtVacina.Text, out vacina.Data_ultima_vacinacao);
                cliente_petDAO pet = (cliente_petDAO)Session["itemPet" + urlSessao()];
                pet.addVacina(vacina);

                carregarGridsPet();

                ddlVacina.Text = "";
                txtDtVacina.Text = "";

            }
            catch (Exception err)
            {

                lblErroPet.Text = err.Message;
            }
            modalPet.Show();
        }


        protected void imgBtnAddGrupoEmpresa_Click(object sender, EventArgs e)
        {
            Label7.ForeColor = System.Drawing.Color.Black;
            Label7.Text = "Novo Grupo";
            txtNomeNovoGrupoEmpresa.Text = "";
            modalIncluirNovoGrupoEmpresa.Show();
        }


        protected void txtGrupoEmpresa_TextChanged(object sender, EventArgs e)
        {
            ClienteDAO obj = (ClienteDAO)Session["cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            obj.grupoEmpresa = Funcoes.intTry(txtGrupoEmpresa.Text);
            obj.nomeGrupoEmpresa = "";
            txtNomeGrupoEmpresa.Text = obj.nomeGrupoEmpresa;

        }
        protected void imgBtnConfirmaIncluirGrupo_Click(object sender, EventArgs e)
        {
            try
            {
                ClienteDAO.incluirNovoGrupoEmpresa(txtNomeNovoGrupoEmpresa.Text.ToUpper());
                exibeLista();
            }
            catch (Exception err)
            {
                Label7.Text = err.Message;
                Label7.ForeColor = System.Drawing.Color.Red;
                modalIncluirNovoGrupoEmpresa.Show();
            }
        }
        protected void imgBtnCancelarIncluirGrupo_Click(object sender, EventArgs e)
        {
            ModalFundo.Show();
        }

        protected void ImgBtnAddProntuario_Click(object sender, ImageClickEventArgs e)
        {
            cliente_petDAO pet = (cliente_petDAO)Session["itemPet" + urlSessao()];
            ClienteObservacao_veterinarioDAO obs = new ClienteObservacao_veterinarioDAO();
            User usr = (User)Session["User"];
            obs.Usuario = usr.getUsuario();
            obs.Data = DateTime.Now;

            pet.ObservacoesVet.Add(obs);
            pet.statusObs = "Incluir";
            carregarGridsPet();
            RadioButton rdo = (RadioButton)gridProntuarios.Rows[pet.ObservacoesVet.Count - 1].FindControl("RdoProntuarioSelecionado");
            rdo.Checked = true;
            txtHoraObservacaoVeterinario.Text = obs.Data.ToString("HH:mm");
            txtHoraObservacaoVeterinario.Enabled = true;
            txtHoraObservacaoVeterinario.BackColor = System.Drawing.Color.White;
            TxtObservacaoVeterinario.Enabled = true;
            TxtObservacaoVeterinario.BackColor = System.Drawing.Color.White;
            TxtObservacaoVeterinario.Text = "";
            TxtObservacaoVeterinario.Focus();

        }

        private void carregarGridsPet()
        {
            cliente_petDAO pet = (cliente_petDAO)Session["itemPet" + urlSessao()];
            gridVacinas.DataSource = pet.tbVacinas();
            gridVacinas.DataBind();

            gridProntuarios.DataSource = pet.ObservacoesVet;
            gridProntuarios.DataBind();

            List<ClientePetImagens> listImg = new List<ClientePetImagens>();
            if (pet.Imagens.Count == 0)
            {
                listImg.Add(new ClientePetImagens() { Imagem = "---", url = "---" });

            }
            else
            {
                listImg = pet.Imagens;
            }


            gridImagens.DataSource = listImg;
            gridImagens.DataBind();

            TxtObservacaoVeterinario.Enabled = false;
            txtHoraObservacaoVeterinario.Enabled = false;
            if (gridProntuarios.Rows.Count > 0)
            {

                RadioButton chk = (RadioButton)gridProntuarios.Rows[gridProntuarios.Rows.Count - 1].FindControl("RdoProntuarioSelecionado");
                if (chk != null)
                {
                    chk.Checked = true;
                    chk.Focus();
                    mostrarObservacao(chk);

                }


            }

            modalPet.Show();
        }

        protected void gridProntuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gridProntuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoProntuarioSelecionado");

            if (rdo == null)
            {
                return;//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            }
            string script = "SetUniqueRadioButton('gridProntuarios.*GrProntuario',this)";
            rdo.Attributes.Add("onclick", script);
        }

        protected void RdoProntuarioSelecionado_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            mostrarObservacao(rdo);
        }
        protected void mostrarObservacao(RadioButton rdo)
        {
            GridViewRow row = (GridViewRow)rdo.Parent.Parent;
            int i = row.RowIndex;

            cliente_petDAO pet = (cliente_petDAO)Session["itemPet" + urlSessao()];
            TxtObservacaoVeterinario.Enabled = false;
            txtHoraObservacaoVeterinario.Enabled = false;
            txtHoraObservacaoVeterinario.Text = pet.ObservacoesVet[i].Data.ToString("HH:mm");
            txtHoraObservacaoVeterinario.BackColor = txtdata_cadastro.BackColor;
            TxtObservacaoVeterinario.Text = pet.ObservacoesVet[i].Observacao;

            TxtObservacaoVeterinario.BackColor = txtdata_cadastro.BackColor;

            rdo.Focus();
            modalPet.Show();
        }

        protected void TxtObservacaoVeterinario_TextChanged(object sender, EventArgs e)
        {
            if (TxtObservacaoVeterinario.Enabled)
            {
                atualizarObservacaoVeterinario();
            }
        }

        private void atualizarObservacaoVeterinario()
        {

            cliente_petDAO pet = (cliente_petDAO)Session["itemPet" + urlSessao()];
            if (!pet.statusObs.Equals(""))
            {
                foreach (GridViewRow item in gridProntuarios.Rows)
                {
                    RadioButton rdo = (RadioButton)item.FindControl("RdoProntuarioSelecionado");
                    if (rdo.Checked)
                    {
                        pet.ObservacoesVet[item.RowIndex].Data = Funcoes.dtTry(pet.ObservacoesVet[item.RowIndex].Data.ToString("dd/MM/yyyy") + " " + txtHoraObservacaoVeterinario.Text);
                        pet.ObservacoesVet[item.RowIndex].Observacao = TxtObservacaoVeterinario.Text;
                        pet.statusObs = "";
                        break;
                    }
                }

            }
            modalPet.Show();

        }

        protected void ImgBtnExcluirProntuario_Click(object sender, ImageClickEventArgs e)
        {
            modalPet.Show();
            modalExcluirProntuario.Show();
        }

        protected void imgBtnCancelaExcluirProntuario_Click(object sender, ImageClickEventArgs e)
        {
            modalExcluirProntuario.Hide();
            modalPet.Show();
        }

        protected void imgBtnConfirmaExcluirProntuario_Click(object sender, ImageClickEventArgs e)
        {
            cliente_petDAO pet = (cliente_petDAO)Session["itemPet" + urlSessao()];
            foreach (GridViewRow item in gridProntuarios.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoProntuarioSelecionado");
                if (rdo.Checked)
                {
                    pet.ObservacoesVet.RemoveAt(item.RowIndex);
                    break;
                }
            }
            modalPet.Show();
            TxtObservacaoVeterinario.Text = "";
            carregarGridsPet();

        }

        protected void imgBtnExcluirPet_Click(object sender, ImageClickEventArgs e)
        {
            ClienteDAO obj = (ClienteDAO)Session["cliente" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            cliente_petDAO pet = (cliente_petDAO)Session["itemPet" + urlSessao()];
            obj.arrPets.RemoveAt(pet.index);
            carregarDados(obj);
            modalPet.Hide();
            modalExcluirPEt.Hide();
        }

        protected void imgBtnCancelaExcluirPet_Click(object sender, ImageClickEventArgs e)
        {
            modalExcluirPEt.Hide();
            modalPet.Show();
        }

        protected void txtHoraObservacaoVeterinario_TextChanged(object sender, EventArgs e)
        {
            atualizarObservacaoVeterinario();
            TxtObservacaoVeterinario.Focus();
        }

        protected void gridImagens_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            cliente_petDAO pet = (cliente_petDAO)Session["itemPet" + urlSessao()];
            ClientePetImagens img = pet.Imagens[index];
            FileInfo fi = new FileInfo(img.url);
            String caminhoPasta = Server.MapPath("~/modulos/Cadastro/pages/uploads/");
            String urlServer = caminhoPasta + img.Imagem.Replace(fi.Extension, "") + fi.Extension;
            String path = "~/modulos/Cadastro/pages/uploads/" + img.Imagem.Replace(fi.Extension, "") + fi.Extension;



            if (!Directory.Exists(caminhoPasta))
            {
                Directory.CreateDirectory(caminhoPasta);
            }

            File.Copy(img.url, urlServer, true);
            RedirectNovaAba(path);
            modalPet.Show();

        }

        protected void btnEnviarArquivo_Click(object sender, EventArgs e)
        {
            try
            {
                String endereco = "c:/Bratter/uploads_Soldi/Cliente/" + txtCodigo_Cliente.Text + "/" + txtCodigoPet.Text;


                if (!Directory.Exists(endereco))
                    Directory.CreateDirectory(endereco);

                FileInfo fi = new FileInfo(FileArquivo.FileName);
                endereco += "/" + txtNomeImagem.Text.Replace(fi.Extension, "") + fi.Extension;
                FileArquivo.SaveAs(endereco);

                cliente_petDAO pet = (cliente_petDAO)Session["itemPet" + urlSessao()];
                ClientePetImagens img = new ClientePetImagens()
                {
                    Codigo_Cliente = txtCodigo_Cliente.Text
                   ,
                    Codigo_pet = txtCodigoPet.Text
                   ,
                    Imagem = txtNomeImagem.Text
                   ,
                    url = endereco
                };
                pet.Imagens.Add(img);

                ModalIncluirImagem.Hide();
                carregarGridsPet();
            }
            catch (Exception err)
            {
                lblErroPet.Text = err.Message;
                modalPet.Show();
            }

        }

        protected void ImgBtnNovaImagem_Click(object sender, ImageClickEventArgs e)
        {
            modalPet.Show();
            ModalIncluirImagem.Show();
        }
        protected void btnFecharAddImagem_Click(object sender, EventArgs e)
        {
            ModalIncluirImagem.Hide();
            modalPet.Show();
        }

        protected void ImgBtnProximaSequnecia_Click(object sender, EventArgs e)
        {
            if (txtCodigo_Cliente.Text.Equals(""))
            {
                txtCodigo_Cliente.Text = Funcoes.sequencia("CLIENTE.CODIGO_CLIENTE", null);
            }
            bool existe = (Funcoes.intTry(Conexao.retornaUmValor("select count(*) from cliente where ltrim(rtrim(codigo_cliente))='" + txtCodigo_Cliente.Text.Trim() + "'", null)) > 0);
            while (existe)
            {
                txtCodigo_Cliente.Text = (Funcoes.intTry(txtCodigo_Cliente.Text) + 1).ToString();
                existe = (Funcoes.intTry(Conexao.retornaUmValor("select count(*) from cliente where ltrim(rtrim(codigo_cliente))='" + txtCodigo_Cliente.Text.Trim() + "'", null)) > 0);
            }

        }

        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void chkSelecionaItem_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void chkTerceiroPreco_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTerceiroPreco.Checked)
            {
                txtCodigo_tabela.Text = "";
            }
        }

        protected void txtCodigo_tabela_TextChanged(object sender, EventArgs e)
        {
            chkTerceiroPreco.Checked = false;
        }
    }
}