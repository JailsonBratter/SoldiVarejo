using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using System.Data.SqlClient;
using System.Collections;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class AliquotasEstadoDetalhes : visualSysWeb.code.PagePadrao
    {

        static String camporeceber = "";
        protected void Page_Load(object sender, EventArgs e)
        {


            User usr = (User)Session["User"];
            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    cfDAO obj = new cfDAO(usr);
                    status = "incluir";

                    Session.Remove("objCf" + urlSessao());
                    Session.Add("objCf" + urlSessao(), obj);
                }
            }
            else
            {
                if (Request.Params["ncm"] != null)  // colocar o campo index da tabela
                {
                    try
                    {
                        if (!IsPostBack)
                        {
                            String ncm = Request.Params["ncm"].ToString();// colocar o campo index da tabela

                            status = "visualizar";
                            cfDAO obj = new cfDAO(ncm, usr);
                            Session.Remove("objCf" + urlSessao());
                            Session.Add("objCf" + urlSessao(), obj);
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
                habilitar(false);
            }
            else
            {
                habilitar(true);
            }
            carregabtn(pnBtn);
            camposnumericos();
        }

        private void habilitar(bool enable)
        {
            EnabledControls(conteudo, enable);
            EnabledControls(cabecalho, enable);
            EnabledControls(PnDetalhesUF, enable);
            EnabledButtons(gridItens, enable);
        }

        private void camposnumericos()
        {
            ArrayList campos = new ArrayList();

            campos.Add("txtICMSEstado");
            campos.Add("txtICMSInterEstadual");
            campos.Add("txtIvaAjustado");

            FormataCamposNumericos(campos, conteudo);
            FormataCamposNumericos(campos, cabecalho);

            ArrayList camposInteiros = new ArrayList();
            camposInteiros.Add("txtNcm");
            camposInteiros.Add("txtCST");

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
            String[] campos = { "txtNcm",
                                "txtDescricao",

                                    ""

                                     };


            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {
            String[] campos = { status.Equals("incluir")?"":"txtNcm",
                                    "txtUF",
                                     "txtDescTributacao",
                                    ""

                                     };


            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("AliquotasEstadoDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            editar(pnBtn);
            habilitar(true);
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("AliquotasEstado.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            modalExluirCFOP.Show();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaCamposObrigatorios())
                {

                    carregarDadosObj();
                    cfDAO obj = (cfDAO)Session["objCf" + urlSessao()];

                    obj.salvar(status.Equals("incluir"));
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;
                    visualizar(pnBtn);
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
            }
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("AliquotasEstado.aspx");//colocar endereco pagina de pesquisa
        }
        //--Atualizar DaoForm 
        private void carregarDados()
        {

            cfDAO obj = (cfDAO)Session["objCf" + urlSessao()];

            txtNcm.Text = obj.cf;
            txtDescricao.Text = obj.descricao;
            txtMargemIva.Text = obj.margem_iva.ToString("N2");
            txtMargemIvaAjustado.Text = obj.margem_iva_ajustado.ToString("N2");
            txtImpEstadual.Text = obj.perc_imp_estadual.ToString("N2");
            txtImpFedNacional.Text = obj.perc_imp_fed_nac.ToString("N2");
            txtImpFedImportacao.Text = obj.perc_imp_fed_importado.ToString("N2");
            txtPis.Text = obj.perc_pis.ToString("N2");
            txtPisEntrada.Text = obj.perc_pis_entrada.ToString("N2");
            txtCstPis.Text = obj.cst_pis;
            txtCofins.Text = obj.perc_cofins.ToString("N2");
            txtCofinsEntrada.Text = obj.perc_cofins_entrada.ToString("N2");
            txtCstCofins.Text = obj.cst_cofins;
            txtCest.Text = obj.CEST;

            gridItens.DataSource = obj.tbUfs();
            gridItens.DataBind();

        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {

            cfDAO obj = (cfDAO)Session["objCf" + urlSessao()];
            obj.cf = txtNcm.Text;
            obj.descricao = txtDescricao.Text;
            Decimal.TryParse(txtMargemIva.Text, out obj.margem_iva);
            Decimal.TryParse(txtMargemIvaAjustado.Text, out obj.margem_iva_ajustado);
            Decimal.TryParse(txtImpEstadual.Text, out obj.perc_imp_estadual);
            Decimal.TryParse(txtImpFedNacional.Text, out obj.perc_imp_fed_nac);
            Decimal.TryParse(txtImpFedImportacao.Text, out obj.perc_imp_fed_importado);
            Decimal.TryParse(txtPis.Text, out obj.perc_pis);
            Decimal.TryParse(txtPisEntrada.Text, out obj.perc_pis_entrada);
            obj.cst_pis = txtCstPis.Text;
            Decimal.TryParse(txtCofins.Text, out obj.perc_cofins);
            Decimal.TryParse(txtCofinsEntrada.Text, out obj.perc_cofins_entrada);
            obj.cst_cofins = txtCstCofins.Text;
            obj.CEST = txtCest.Text;

            Session.Remove("objCf" + urlSessao());
            Session.Add("objCf" + urlSessao(), obj);
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

        protected void Img_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;


            String or = btn.ID.Substring(7);
            Session.Add("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), or);
            TxtPesquisaLista.Text = "";
            exibeLista();


        }

        protected void ImgObs_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;


            String or = btn.ID.Substring(7);
            Session.Add("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), or);

            carregarObservacoes();


        }

        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }
        protected void exibeLista()
        {
            lblErroPesquisa.Text = "";
            User usr = (User)Session["User"];
            String or = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            String sqlLista = "";


            switch (or)
            {
                case "txtCest":
                    lbllista.Text = "Escolha uma CEST";
                    sqlLista = "EXEC SP_CEST_NCM '" + txtNcm.Text.Trim() + "'";

                    break;
                case "txtCstCofins":
                    lbllista.Text = "Escolha o CST";
                    sqlLista = "Select CST=pis_cst_Saida   , Descricao from pis_cst_Saida where descricao like '%" + TxtPesquisaLista.Text + "%' or pis_cst_Saida like '%" + TxtPesquisaLista.Text + "%'";
                    break;
                case "txtCstPis":
                    lbllista.Text = "Escolha o CST";
                    sqlLista = "Select CST=pis_cst_Saida   , Descricao from pis_cst_Saida where descricao like '%" + TxtPesquisaLista.Text + "%' or pis_cst_Saida like '%" + TxtPesquisaLista.Text + "%'";
                    break;
                case "txtCodTributacao":
                    lbllista.Text = "Escolha tributação";
                    sqlLista = "Select codigo_tributacao,descricao_Tributacao from tributacao where descricao_tributacao like '%" + TxtPesquisaLista.Text + "%'";
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
                String listaAtual = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                Session.Remove("campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));

                if (listaAtual.Equals("txtCstCofins"))
                {
                    txtCstCofins.Text = ListaSelecionada(1);


                }
                else if (listaAtual.Equals("txtCstPis"))
                {
                    txtCstPis.Text = ListaSelecionada(1);


                }
                else if (listaAtual.Equals("txtCodTributacao"))
                {
                    txtCodTributacao.Text = ListaSelecionada(1);
                    txtDescTributacao.Text = ListaSelecionada(2);
                    tributacaoDAO trib = new tributacaoDAO(txtCodTributacao.Text, null);
                    txticmsInterestadual.Text = trib.Saida_ICMS.ToString("N2");
                    txticmsEstadoSimples.Text = trib.ICMS_Efetivo.ToString("N2");

                    modalDetalhes.Show();
                }
                else if (listaAtual.Equals("txtCest"))
                {
                    txtCest.Text = ListaSelecionada(1);


                }

                modalPnFundo.Hide();
            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                modalPnFundo.Show();
            }
        }

        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                cfDAO obj = (cfDAO)Session["objCf" + urlSessao()];
                obj.excluir();
                modalExluirCFOP.Hide();
                lblError.Text = "Registro Excluido com sucesso";
                limparCampos();
                Session.Remove("objAliquota" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                pesquisar(pnBtn);
            }
            catch (Exception err)
            {
                lblError.Text = "Não foi possivel Excluir o registro error:" + err.Message;
            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            modalExluirCFOP.Hide();
        }



        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            pnFundo.Visible = false;
        }
        //
        protected void btnConfirmaUF_Click(object sender, ImageClickEventArgs e)
        {
            int index = 0;
            int.TryParse(lblIndex.Text, out index);
            cfDAO obj = (cfDAO)Session["objCf" + urlSessao()];
            aliquota_imp_estadoDAO objUf = (aliquota_imp_estadoDAO)obj.arrUfs[index];
            Decimal.TryParse(txticmsInterestadual.Text, out objUf.icms_interestadual);
            Decimal.TryParse(txticmsEstadoSimples.Text, out objUf.icms_estado_simples);
            Decimal.TryParse(txtMva.Text, out objUf.mva);
            Decimal.TryParse(txtMvaSimples.Text, out objUf.mva_simples);
            Decimal.TryParse(txtMvaConsumidorFinal.Text, out objUf.mva_cons_final);
            Decimal.TryParse(txtPorcEstornoSt.Text, out objUf.porc_estorno_st);
            objUf.condicao_icms = ddlCondicoesICMS.SelectedValue;
            Decimal.TryParse(txtPorcICMS.Text, out objUf.porc_icms);
            objUf.tipo_reducao = txtTipoReducao.Text;
            Decimal.TryParse(txtPorcReducao.Text, out objUf.porc_reducao);
            objUf.tipo_reducao_simples = txtTipoReducaoSimples.Text;
            Decimal.TryParse(txtPorcReducaoSimples.Text, out objUf.porc_reducao_simples);
            objUf.texto_nf = txtTextoNf.Text;
            objUf.texto_nf_simples = txtTextoNfSimples.Text;
            objUf.cfop = txtCfop.Text;
            objUf.protocolo = txtProtocolo.Text;
            objUf.cod_tributacao = txtCodTributacao.Text;
            Decimal.TryParse(txtPorcCombatePobreza.Text, out objUf.porc_combate_pobresa);
            chkNaoAbateAliqOrigem.Checked = objUf.nao_abate_aliq_origem;

            obj.arrUfs[index] = objUf;
            Session.Remove("objCf" + urlSessao());
            Session.Add("objCf" + urlSessao(), obj);
            carregarDados();
        }

        protected void btnCancelaUF_Click(object sender, ImageClickEventArgs e)
        {
            modalDetalhes.Hide();
        }

        protected void gridItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int index = Convert.ToInt32(e.CommandArgument);

            lblIndex.Text = index.ToString();

            cfDAO obj = (cfDAO)Session["objCf" + urlSessao()];
            if (index < obj.arrUfs.Count)
            {
                aliquota_imp_estadoDAO objUf = (aliquota_imp_estadoDAO)obj.arrUfs[index];

                txtUF.Text = objUf.uf;
                txticmsInterestadual.Text = objUf.icms_interestadual.ToString("N2");
                txticmsEstadoSimples.Text = objUf.icms_estado_simples.ToString("N2");
                txtMva.Text = objUf.mva.ToString("N2");
                txtMvaSimples.Text = objUf.mva_simples.ToString("N2");
                txtPorcEstornoSt.Text = objUf.porc_estorno_st.ToString("N2");
                txtMvaConsumidorFinal.Text = objUf.mva_cons_final.ToString("N2");
                ddlCondicoesICMS.SelectedValue = objUf.condicao_icms;
                txtPorcICMS.Text = objUf.porc_icms.ToString("N2");
                txtTipoReducao.Text = objUf.tipo_reducao;
                txtPorcReducao.Text = objUf.porc_reducao.ToString("N2");
                txtTipoReducaoSimples.Text = objUf.tipo_reducao_simples;
                txtPorcReducaoSimples.Text = objUf.porc_reducao_simples.ToString("N2");
                txtTextoNf.Text = objUf.texto_nf;
                txtTextoNfSimples.Text = objUf.texto_nf_simples;
                txtCfop.Text = objUf.cfop;
                txtProtocolo.Text = objUf.protocolo;
                chkNaoAbateAliqOrigem.Checked = objUf.nao_abate_aliq_origem;
                txtCodTributacao.Text = objUf.cod_tributacao;
                txtDescTributacao.Text = objUf.descricao_tributacao.ToString();
                txtPorcCombatePobreza.Text = objUf.porc_combate_pobresa.ToString();
            }
            modalDetalhes.Show();
        }

        protected void imgBtnConfirmaObservacoes_Click(object sender, ImageClickEventArgs e)
        {
            String listaAtual = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            TextBox txt;
            if (listaAtual.Equals("TxtTextoNf"))
            {
                txt = txtTextoNf;
            }
            else
            {
                txt = txtTextoNfSimples;
            }

            foreach (GridViewRow linha in GridObservacao.Rows)
            {
                RadioButton rdo = (RadioButton)linha.FindControl("RdoListaItem");
                if (rdo.Checked)
                {
                    txt.Text = linha.Cells[1].Text;

                }

            }

            imgBtnAddObservacoes.Visible = true;
            lblObservacao.Visible = true;
            pnNovaObservacao.Visible = false;
            modalObservacoes.Hide();
            modalDetalhes.Show();
        }

        protected void imgBtnCancelarObservacoes_Click(object sender, ImageClickEventArgs e)
        {
            imgBtnAddObservacoes.Visible = true;
            lblObservacao.Visible = true;
            pnNovaObservacao.Visible = false;
            modalObservacoes.Hide();
            modalDetalhes.Show();
        }
        protected void imgConfirmaAddObservacao_Click(object sender, ImageClickEventArgs e)
        {
            if (!txtObservacaoAdd.Text.Equals(""))
            {
                if (lblcod.Text.Equals(""))
                {
                    Conexao.executarSql("insert into Obs_nota (cod,descricao)values((select isnull(MAX(cod)+1,1) from Obs_nota),'" + txtObservacaoAdd.Text + "')");
                }
                else
                {
                    Conexao.executarSql("update obs_nota set descricao='" + txtObservacaoAdd.Text + "' where cod='" + lblcod.Text + "'");
                }
            }

            pnNovaObservacao.Visible = false;
            imgBtnAddObservacoes.Visible = true;
            lblObservacao.Visible = true;

            carregarObservacoes();
        }
        protected void carregarObservacoes()
        {
            GridObservacao.DataSource = Conexao.GetTable("select cod,Observacao=SUBSTRING(descricao,0,50) from Obs_nota", null, false);
            GridObservacao.DataBind();
            modalObservacoes.Show();
        }

        protected void imgBtnAddObservacoes_Click(object sender, ImageClickEventArgs e)
        {
            pnNovaObservacao.Visible = true;
            lblcod.Text = "";
            txtObservacaoAdd.Text = "";

            txtObservacaoAdd.Focus();
            imgBtnAddObservacoes.Visible = false;
            lblObservacao.Visible = false;
            modalObservacoes.Show();
        }

        protected void GridObservacao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoListaItem");

            if (rdo == null)
            {
                return;//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            }
            string script = "SetUniqueRadioButton('GridObsLista.*GrObslistaItem',this)";
            rdo.Attributes.Add("onclick", script);
        }

        protected void GridObservacao_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            String cod = GridObservacao.Rows[index].Cells[1].Text;
            txtObservacaoAdd.Text = Conexao.retornaUmValor("select descricao from obs_nota where cod='" + cod + "'", null);
            lblcod.Text = cod;
            pnNovaObservacao.Visible = true;
            modalObservacoes.Show();

        }

        protected void btnIBPT_Click(object sender, EventArgs e)
        {

        }
    }
}