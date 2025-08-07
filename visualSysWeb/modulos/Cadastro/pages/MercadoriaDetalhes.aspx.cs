using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using System.Data.SqlClient;
using System.Data;
using visualSysWeb.code;
using System.Collections;
using System.Web.Services;
using System.IO;
using visualSysWeb.modulos.Cadastro.code;
using visualSysWeb.modulos.Cadastro.dao;

namespace visualSysWeb.Cadastro
{
    public partial class MercadoriaDetalhes : visualSysWeb.code.PagePadrao
    {

        private bool usaSKU = false;
        private bool integracaoiFood = false;
        private bool integracaoMagento = false;
        private Cardapio_iFood_Produto produtoiFood;

        protected void Page_Load(object sender, EventArgs e)
        {

            User usr = (User)Session["User"];
            if (usr == null)
                return;
            this.Page.Form.Enctype = "multipart/form-data";
            MercadoriaDAO merc;


            try
            {
                if (!IsPostBack)
                {


                    Conexao.preencherDDL(ddlAgrupamento, "Select * from agrupamento_producao", "Agrupamento", "Agrupamento", usr);
                    Conexao.preencherDDL(ddlBandeja, "Select * from bandeja ORDER BY ID", "descricao", "id", usr);
                    Conexao.preencherDDL(ddlMarca, "SELECT * FROM Marca", "descricao", "id", usr);
                    divCategoria.Visible = Funcoes.valorParametro("INCLUI_CATEGORIA", null).ToUpper().Equals("TRUE");

                    //Especifico para API MAGENTO 
                    Conexao.preencherLISTCheckBox(lcAnimal, "SELECT especie, id FROM Especie", "Especie", "id", usr);
                    Conexao.preencherLISTCheckBox(lckRaca, "SELECT raca, id FROM Raca", "Raca", "id", usr);
                    Conexao.preencherLISTCheckBox(lckSabor, "SELECT descricao, id FROM PET_Sabor ORDER BY descricao", "descricao", "id", usr);
                    Conexao.preencherLISTCheckBox(lckCuidados, "SELECT descricao, id FROM PET_Cuidados", "descricao", "id", usr);
                    Conexao.preencherLISTCheckBox(lckTipoFarmaceutico, "SELECT descricao, id FROM PET_Farmacos", "descricao", "id", usr);
                    Conexao.preencherLISTCheckBox(lckTipoAcessorio, "SELECT descricao, id FROM PET_Acessorios_Tipo", "descricao", "id", usr);
                    Conexao.preencherLISTCheckBox(lckCor, "SELECT cor, id FROM Cor", "cor", "id", usr);
                }

                usaSKU = Funcoes.valorParametro("ATIVA_SKU_ECOMMERCE", usr).ToUpper().Equals("TRUE");
                integracaoMagento = Funcoes.valorParametro("ECOMMERCE_MAGENTO", usr).ToUpper().Equals("TRUE");

                if (!usaSKU)
                {
                    txtSKU.Visible = false;
                }

                if (Request.Params["PLU"] != null)
                {
                    if (!IsPostBack)
                    {

                        String plu = Request.Params["PLU"].ToString();
                        if (plu.Trim().Equals(""))
                        {
                            throw new Exception("PLU INVALIDO");
                        }


                        if (Request.Params["img"] != null)
                        {
                            merc = (MercadoriaDAO)Session["mercadoria" + urlSessao().Replace("&img=true", "")];
                            Session.Remove("mercadoria" + urlSessao().Replace("&img=true", ""));
                            if (merc != null)
                                Session.Add("mercadoria" + urlSessao(), merc);

                            status = "editar";
                            habilitarControles(true);
                            TabContainer1.ActiveTabIndex = 8;
                        }
                        else
                        {
                            merc = new MercadoriaDAO(plu, usr);
                            Session.Remove("mercadoria" + urlSessao());
                            Session.Add("mercadoria" + urlSessao(), merc);
                            status = "visualizar";

                        }


                        carregarDados();


                        //Checagem do cardapio ifood
                        integracaoiFood = Funcoes.valorParametro("INTEGRACAO_IFOOD", usr).ToUpper().Equals("TRUE");

                        if (!integracaoiFood)
                        {
                            diviFood.Visible = false;
                        }
                        else
                        {
                            int IDCardapio = int.Parse(Funcoes.valorParametro("ID_CARDAPIO_IFOOD", usr));
                            if (IDCardapio > 0)
                            {
                                String pathImagens = Server.MapPath("~/modulos/Cadastro/imgs/uploads/");
                                produtoiFood = new Cardapio_iFood_Produto(IDCardapio, merc.PLU, pathImagens);
                                if (produtoiFood.PLU != null)
                                {
                                    txtCodigoUUID.Text = produtoiFood.id;
                                    txtHrStart.Text = produtoiFood.horaInicio;
                                    txtHrEnd.Text = produtoiFood.horaFim;
                                    chkDia_1.Checked = produtoiFood.dia1;
                                    chkDia_2.Checked = produtoiFood.dia2;
                                    chkDia_3.Checked = produtoiFood.dia3;
                                    chkDia_4.Checked = produtoiFood.dia4;
                                    chkDia_5.Checked = produtoiFood.dia5;
                                    chkDia_6.Checked = produtoiFood.dia6;
                                    chkDia_7.Checked = produtoiFood.dia7;
                                    if (produtoiFood.restricaoAlimentar.Count > 0)
                                    {
                                        ddlRestricaoAlimentar.Text = produtoiFood.restricaoAlimentar[0].ToString();
                                    }
                                    ddlServe.Text = produtoiFood.serve;
                                    ddlStatus.Text = (produtoiFood.status ? 1 : 0).ToString();
                                }
                            }
                        }



                        habilitarControles(!status.Equals("visualizar"));


                    }

                }
                else
                {
                    if (!IsPostBack)
                    {


                        if (Request.Params["NOVO"] != null)
                        {
                            merc = (MercadoriaDAO)Session["mercadoriaManter"];
                            if (merc != null && merc.PLU != null && !merc.PLU.Equals(""))
                            {
                                Session.Remove("mercadoria" + urlSessao());
                                Session.Add("mercadoria" + urlSessao(), merc);

                                //Session.Remove("mercadoriaManter");
                                modalManterDados.Show();
                            }
                            else
                            {
                                merc = new MercadoriaDAO(usr);
                                txtCodTipo.Text = "PRINCIPAL";
                                txtUnidade.Text = "UN";
                                txtTecla.Text = "255";
                                chkCurvaA.Checked = true;
                                chkCurvaB.Checked = true;
                                chkCurvaC.Checked = true;

                                ddlIndEscala.SelectedItem.Value = "S";
                                Session.Remove("mercadoria" + urlSessao());
                                Session.Add("mercadoria" + urlSessao(), merc);

                            }
                            status = "incluir";

                            habilitarControles(true);
                        }
                    }
                }
                carregabtn(pnBtn);
                btnEans.Visible = true;

            }
            catch (Exception err)
            {

                msgShow("Ocorreu um erro: " + err.Message, true);

                carregabtn(pnBtn);

            }


            camposnumericos();

            camposData();

            TxtAddEan.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            ExibirImagens();

            //Checa se vai enviar imagem via FTP
            bool cardapioEnviaFTP = (Funcoes.valorParametro("CARDAPIO_ENVIA_FTP", null).ToUpper().Equals("TRUE") ? true : false);
            if (!cardapioEnviaFTP)
            {
                btnEnviarFTP.Visible = false;
            }
        }




        protected void habilitarControles(bool enable)
        {
            User usr = (User)Session["User"];

            EnabledControls(conteudo, enable);
            EnabledControls(cabecalho, enable);
            EnabledControls(PnPrecoLojaConteudo, enable);
            EnabledControls(pnTabelaPreco, enable);

            btnAplicarLojas.Visible = enable;
            divLimparCentroCusto.Visible = enable;
            if (enable)
            {

                txtvlr_energ_qtde.Enabled = !chkVlr_energetico_nao.Checked;
                txtvlr_energ_qtde_igual.Enabled = !chkVlr_energetico_nao.Checked;
                txtvlr_energ_diario.Enabled = !chkVlr_energetico_nao.Checked;
                if (chkVlr_energetico_nao.Checked)
                {
                    txtvlr_energ_qtde.Text = "";
                    txtvlr_energ_qtde_igual.Text = "";
                    txtvlr_energ_diario.Text = "";
                }

                txtcarboidratos_qtde.Enabled = !chkCarboidratos_nao.Checked;
                txtcarboidratos_vlr_diario.Enabled = !chkCarboidratos_nao.Checked;
                if (chkCarboidratos_nao.Checked)
                {
                    txtcarboidratos_qtde.Text = "";
                    txtcarboidratos_vlr_diario.Text = "";
                }

                txtproteinas_qtde.Enabled = !chkProteinas_nao.Checked;
                txtproteinas_vlr_diario.Enabled = !chkProteinas_nao.Checked;
                if (chkProteinas_nao.Checked)
                {
                    txtproteinas_qtde.Text = "";
                    txtproteinas_vlr_diario.Text = "";
                }
                txtgorduras_totais_qtde.Enabled = !chkgorduras_totais_nao.Checked;
                txtgorduras_totais_vlr_diario.Enabled = !chkgorduras_totais_nao.Checked;
                if (chkgorduras_totais_nao.Checked)
                {
                    txtgorduras_totais_qtde.Text = "";
                    txtgorduras_totais_vlr_diario.Text = "";
                }
                txtgorduras_satu_qtde.Enabled = !chkgorduras_satu_nao.Checked;
                txtgorduras_satu_vlr_diario.Enabled = !chkgorduras_satu_nao.Checked;
                if (chkgorduras_satu_nao.Checked)
                {
                    txtgorduras_satu_qtde.Text = "";
                    txtgorduras_satu_vlr_diario.Text = "";
                }

                txtgorduras_trans_qtde.Enabled = !chkgorduras_trans_nao.Checked;
                if (chkgorduras_trans_nao.Checked)
                {
                    txtgorduras_trans_qtde.Text = "";
                }

                txtfibra_alimen_qtde.Enabled = !chkfibra_alimen_nao.Checked;
                txtfibra_alimen_vlr_diario.Enabled = !chkfibra_alimen_nao.Checked;
                if (chkfibra_alimen_nao.Checked)
                {
                    txtfibra_alimen_qtde.Text = "";
                    txtfibra_alimen_vlr_diario.Text = "";
                }

                txtsodio_qtde.Enabled = !chksodio_nao.Checked;
                txtsodio_vlr_diario.Enabled = !chksodio_nao.Checked;
                if (chksodio_nao.Checked)
                {
                    txtsodio_qtde.Text = "";
                    txtsodio_vlr_diario.Text = "";
                }

                txtcolesterol_qtde.Enabled = !chkcolesterol_nao.Checked;
                txtcolesterol_vlr_diario.Enabled = !chkcolesterol_nao.Checked;
                if (chkcolesterol_nao.Checked)
                {
                    txtcolesterol_qtde.Text = "";
                    txtcolesterol_vlr_diario.Text = "";
                }

                txtcalcio_qtde.Enabled = !chkcalcio_nao.Checked;
                txtcalcio_vlr_diario.Enabled = !chkcalcio_nao.Checked;
                if (chkcalcio_nao.Checked)
                {
                    txtcalcio_qtde.Text = "";
                    txtcalcio_vlr_diario.Text = "";
                }
                txtferro_qtde.Enabled = !chkferro_nao.Checked;
                txtferro_vlr_diario.Enabled = !chkferro_nao.Checked;
                if (chkferro_nao.Checked)
                {
                    txtferro_qtde.Text = "";
                    txtferro_vlr_diario.Text = "";
                }



                if (enable && !chkPratoDia.Checked)
                {
                    chkPratoDia_1.Enabled = false;
                    chkPratoDia_2.Enabled = false;
                    chkPratoDia_3.Enabled = false;
                    chkPratoDia_4.Enabled = false;
                    chkPratoDia_5.Enabled = false;
                    chkPratoDia_6.Enabled = false;
                    chkPratoDia_7.Enabled = false;

                }
            }

            bool pesqRef = Funcoes.valorParametro("UTILIZA_PESQ_REFERENCIA", usr).ToUpper().Equals("TRUE");

            if (!pesqRef)
            {
                imgBtnUltmioFornecedor.Visible = true;
                imgRefFornecedor.Visible = true;
            }

            if (!usr.adm())
            {
                btnEans.Visible = usr.telaPermissao("Cadastro");
                TabCadastro.Visible = usr.telaPermissao("Cadastro");
                TabPreco.Visible = usr.telaPermissao("Preco");
                TabEstoque.Visible = usr.telaPermissao("Estoque");
                TabTributacao.Visible = usr.telaPermissao("Tributacao");
                TabObservacao.Visible = usr.telaPermissao("Observacao");
                TabVendasECF.Visible = usr.telaPermissao("Vendas ECF");
                TabInformacaoNutricional.Visible = usr.telaPermissao("Informacao Nutricional");
            }

            bool permiteItem = Conexao.retornaUmValor("Select permite_item from tipo where tipo ='" + txtCodTipo.Text + "'", null).Equals("1");
            if (permiteItem)
            {
                TabItens.Visible = usr.telaPermissao("Itens");
                divSaldo.Visible = !usr.telaPermissao("Itens"); ;
            }
            else
            {
                TabItens.Visible = false;
                divSaldo.Visible = true;

            }

            bool pluVinculado = Conexao.retornaUmValor("Select PLUAssociado from tipo where tipo ='" + txtCodTipo.Text + "'", null).Equals("1");
            if (pluVinculado)
            {
                divPluVinculado.Visible = true;
                txtPLUVinculado.Visible = true;
                txtDescricaoVinculado.Visible = true;
                txtFatorVinculado.Visible = true;
                txtSaldo.Visible = false;
            }
            else
            {
                txtSaldo.Visible = true;
                txtPLUVinculado.Visible = true;
                txtDescricaoVinculado.Visible = true;
                txtFatorVinculado.Visible = true;

                divPluVinculado.Visible = false;
                txtPLUVinculado.Text = "";
                txtPLUVinculado.Visible = false;
                txtDescricaoVinculado.Visible = false;
                txtFatorVinculado.Visible = false;
                txtFatorVinculado.Text = "0,000";
            }




            bool bECommerce = Funcoes.valorParametro("ATIVA_ECOMMERCE", usr).ToUpper().Equals("TRUE");


            if (bECommerce)
            {
                divTextoEcommerce.Visible = !enable;
                txtEcommercer.Visible = enable;

                txtDescricaoComercial.Enabled = enable;

                TabEcommerce.Visible = usr.telaPermissao("E-commerce");
                TabEcommerceAPI.Visible = integracaoMagento;
            }
            else
            {
                divTextoEcommerce.Visible = false;
                txtEcommercer.Visible = false;
                TabEcommerce.Visible = false;
                TabEcommerceAPI.Visible = false;
            }

            if (enable)
            {

                if (!txtPluReceita.Text.Trim().Equals(TxtCodPLU.Text.Trim()))
                {
                    EnabledControls(divItensIncluidos, false);
                    EnabledControls(divTotalisReceita, false);
                    EnabledControls(divPluReceita, false);
                    txtReceita.Enabled = false;




                }
                txtPluReceita.Enabled = true;
                txtPluReceita.BackColor = txtDescricao.BackColor;
                ddlTipoProducao.Enabled = true;
                ddlTipoProducao.BackColor = txtDescricao.BackColor;
                txtpeso_receita_unitario.Enabled = true;
                txtpeso_receita_unitario.BackColor = txtPluReceita.BackColor;
                txtDescricao.Enabled = usr.telaPermissao("Cadastro");
                txtDescResumida.Enabled = usr.telaPermissao("Cadastro");
                if (!txtDescricao.Enabled)
                {
                    txtDescricao.BackColor = txtEAN.BackColor;
                    txtDescResumida.BackColor = txtEAN.BackColor;
                }
            }
            ddlObrigatorioOrdem.Enabled = false;

            //Apresenta ou não a grid de movimentação dos últimos 10 dias.
            NaoControlaEstoque.Visible = !txtSaldo.Visible;
            Grid10Dias.Visible = txtSaldo.Visible;

        }

        protected void camposData()
        {
            ArrayList dts = new ArrayList();
            dts.Add("txtDtAlteracao");
            dts.Add("txtDtFimPromo");
            dts.Add("txtDtInicioPromo");
            dts.Add("txtDtSazoA1");
            dts.Add("txtDtSazoA2");
            dts.Add("txtDtSazoA3");
            dts.Add("txtDtSazoDe1");
            dts.Add("txtDtSazoDe2");
            dts.Add("txtDtSazoDe3");
            FormataCamposDatas(dts, conteudo);
            FormataCamposDatas(dts, cabecalho);


        }

        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }
        private void camposnumericos()
        {
            ArrayList campos = new ArrayList();
            campos.Add("txtPrecoCompra");
            campos.Add("txtPesoBruto");
            campos.Add("txtPesoLiquido");
            campos.Add("txtParcial");
            campos.Add("txtPrecoCusto");
            campos.Add("txtMargem");
            campos.Add("txtPrecoCompra");
            campos.Add("txtPrecoVenda");
            campos.Add("txtValorST");
            campos.Add("txtValorLucro");
            campos.Add("txtValorIPI");
            campos.Add("txtValorPisConfins");
            campos.Add("txtValorIcms");
            campos.Add("txtPrecoPromocao");
            campos.Add("txtPrecoCompraLoja");
            campos.Add("txtMargemLoja");
            campos.Add("txtPrecoLoja");
            campos.Add("txtPrecoCusto");
            campos.Add("txtFatorConversaoItem");
            FormataCamposNumericos(campos, conteudo);


            ArrayList camposInteiros = new ArrayList();
            camposInteiros.Add("TxtCodPLU");
            camposInteiros.Add("txtSKU");
            FormataCamposInteiros(camposInteiros, cabecalho);
        }
        protected void carregarJCS()
        {


        }


        protected void carregarDados()
        {
            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
            //lblError.Text = "";

            GridEan.DataSource = merc.ean;
            GridEan.DataBind();
            if (merc.ean != null)
            {

                foreach (DataRow item in merc.ean.Rows)
                {
                    txtEAN.Text = item[0].ToString();
                    break;
                }
            }

            TxtCodPLU.Text = merc.PLU.ToString();
            txtCodTipo.Text = merc.Tipo.ToString();
            ddlPesoVariavel.Text = (merc.Peso_Variavel.ToString().Trim().Equals("") ? "NÃO" : merc.Peso_Variavel.ToString().Trim());
            txtPortaria.Text = merc.Codigo_Portaria.ToString();
            txtCodTribSaida.Text = merc.Codigo_Tributacao.ToString();
            txtCodTribEntrada.Text = merc.Codigo_Tributacao_ent.ToString();
            txtCodDepartamento.Text = merc.Codigo_departamento.ToString();
            txtCodFamilia.Text = merc.Codigo_familia.ToString();
            txtDepartamento.Text = merc.Descricao_departamento.ToString();
            txtCodSubGrupo.Text = merc.codigo_subGrupo.ToString();
            txtSubGrupo.Text = merc.descricao_subGrupo.ToString();
            txtCodGrupo.Text = merc.codigo_Grupo.ToString();
            txtGrupo.Text = merc.descricao_Grupo.ToString();
            txtUsuario.Text = merc.usuario;
            txtUsuarioAlteracao.Text = merc.usuarioAlteracao;
            txtEcommercer.Value = merc.strEcommerce;
            divTextoEcommerce.InnerHtml = merc.strEcommerce;

            txtDescricao.Text = merc.Descricao.ToString();
            txtDescResumida.Text = merc.Descricao_resumida.ToString();
            txtFamilia.Text = merc.Descricao_familia.ToString();
            txtTecla.Text = merc.Tecla.ToString();
            txtEstoqueMinimo.Text = merc.Estoque_minimo.ToString();
            txtEmbalagem.Text = merc.Embalagem.ToString();
            txtEtiqueta.Text = merc.Etiqueta.ToString();
            txtValidade.Text = merc.Validade.ToString();
            User usr = (User)Session["User"];
            // informação de precos

            if (merc.mercadoriasLoja.Count > 0)
            {
                foreach (mercadoria_lojaDAO item in merc.mercadoriasLoja)
                {

                    if (item.Filial.Trim().Equals(usr.getFilial().Trim()))
                    {
                        txtPrecoCompra.Text = item.Preco_Compra.ToString("N2");
                        txtPrecoCusto.Text = item.Preco_Custo.ToString("N2");
                        txtMargem.Text = item.Margem.ToString("N4");
                        txtPrecoVenda.Text = item.Preco.ToString("N2");
                        txtPrecoPromocao.Text = item.Preco_Promocao.ToString("N2");
                        txtDtInicioPromo.Text = (item.Data_Inicio.ToString("dd/MM/yyyy").Equals("01/01/0001") ? "" : item.Data_Inicio.ToString("dd/MM/yyyy"));
                        txtDtFimPromo.Text = (item.Data_Fim.ToString("dd/MM/yyyy").Equals("01/01/0001") ? "" : item.Data_Fim.ToString("dd/MM/yyyy"));
                        chkPromocaoAutomatica.Checked = item.Promocao_automatica;
                        chkPromocao.Checked = item.Promocao;
                        txtQtdeAtacado.Text = item.qtde_atacado.ToString();
                        txtPrecoAtacado.Text = item.preco_atacado.ToString();
                        txtMargemAtacado.Text = item.margem_atacado.ToString();
                        break;
                    }
                }
            }
            else
            {
                txtPrecoCompra.Text = (merc.preco_compra > 0 ? merc.preco_compra.ToString("N2") : "");
                txtPrecoCusto.Text = (merc.Preco_Custo > 0 ? merc.Preco_Custo.ToString("N2") : "");
                txtMargem.Text = (merc.Margem > 0 ? merc.Margem.ToString("N4") : "");
                txtPrecoVenda.Text = (merc.Preco > 0 ? merc.Preco.ToString("N2") : "");
                txtQtdeAtacado.Text = merc.qtde_atacado.ToString();
                txtPrecoAtacado.Text = merc.preco_atacado.ToString();
                txtMargemAtacado.Text = merc.margem_atacado.ToString();

            }

            txtUndCompra.Text = merc.und_compra;
            txtDescricao_producao.Text = merc.descricao_producao;
            txtEmbalagemProducao.Text = merc.embalagem_producao.ToString();
            txtPrecoCompraProducao.Text = merc.preco_compra.ToString("N2");
            txtUndProducao_2.Text = merc.Und_producao;
            txtCustoProducao.Text = merc.custo_producao.ToString("N2");


            txtDtCadastro.Text = merc.Data_CadastroBr();
            txtDtAlteracao.Text = merc.Data_AlteracaoBr();
            txtIpi.Text = merc.IPI.ToString();

            txtUltimoFornecedor.Text = merc.ultimo_fornecedor.ToString();

            txtTeclaBalanca.Text = merc.Tecla_balanca.ToString();
            txtLocaliza.Text = merc.Localizacao.ToString();

            chkImprimeEtiqueta.Checked = merc.Imprime_etiqueta;

            txtSaldo.Text = merc.saldo_atual.ToString();
            //txtPrecoCusto.Text = merc.Preco_Custo_1.ToString("N2");
            txtReferenciaFornecedor.Text = merc.Ref_fornecedor.ToString();

            txtPesoBruto.Text = merc.peso.ToString();
            txtReceita.Text = merc.receita.ToString();


            txtQtdReceita.Text = merc.qtde_receita.ToString("N2");
            txtPluReceita.Text = merc.pluReceita;
            txtPluReceitaDescricao.Text = merc.DescResumidaReceita;

            txtUndProducao.Text = merc.Und_producao;
            chkProgSeg.Checked = merc.progSeg;
            chkProgTer.Checked = merc.progTer;
            chkProgQua.Checked = merc.progQua;
            chkProgQui.Checked = merc.progQui;
            chkProgSex.Checked = merc.progSex;
            chkProgSab.Checked = merc.progSab;
            chkProgDom.Checked = merc.progDom;
            txtCustoTotalReceita.Text = merc.CustoTotalReceita.ToString("N2");
            txtpeso_receita_unitario.Text = merc.peso_receita_unitario.ToString("N2");



            txtValorIPI.Text = merc.valor_ipi.ToString();

            txtCentroCusto.Text = merc.codigo_centro_custo.ToString();
            txtDescricaoCentroCusto.Text = merc.descricao_centro_custo;
            txtSubGrupoCentroCusto.Text = merc.codigo_subgrupo_cc;
            txtDescricaoSubGrupoCentroCusto.Text = merc.descricao_subgrupo_cc;
            txtGrupoCentroCusto.Text = merc.codigo_grupo_cc;
            txtDescricaoGrupoCentroCusto.Text = merc.descricao_grupo_cc;

            ddlVendaFracionaria.Text = merc.venda_fracionaria.ToString();

            txtClassifFiscal.Text = merc.cf.ToString();
            txtCEST.Text = merc.CEST.ToString();
            txtUnidade.Text = merc.und.ToString();




            chkCurvaA.Checked = merc.curva_a;
            chkCurvaB.Checked = merc.curva_b;
            chkCurvaC.Checked = merc.curva_c;
            chkAvisaEstoqueMinimo.Checked = merc.estoque_aviso;
            txtArtigo.Text = merc.artigo.ToString();
            txtMediaMensal.Text = merc.estoque_meses.ToString();
            txtCoberturaDias.Text = merc.cobertura.ToString();

            txtDtSazoA1.Text = merc.sazonal1Br();
            txtDtSazoDe1.Text = merc.sazonal2Br();

            txtDtSazoA2.Text = merc.sazonal3Br();
            txtDtSazoDe2.Text = merc.sazonal4Br();

            txtDtSazoA3.Text = merc.sazonal5Br();
            txtDtSazoDe3.Text = merc.sazonal6Br();
            txtPesoLiquido.Text = merc.peso_liquido.ToString();
            txtPesoBruto.Text = merc.peso_bruto.ToString();
            txtIva.Text = merc.margem_iva.ToString();
            txtMarca.Text = merc.Marca.ToString();
            //try
            //{
            //    ddlMarca.SelectedValue = merc.Marca.ToString();
            //}
            //catch
            //{
            //    ddlMarca.Text = "";
            //}
            txtCSTSaida.Text = merc.cst_saida;
            txtCSTEntrada.Text = merc.cst_entrada;
            txtCFOP.Text = merc.CFOP;

            txtUsuario.Text = merc.usuario;

            bool tabMargem = Funcoes.valorParametro("TAB_PRECO_MARG", usr).ToUpper().Equals("TRUE");
            if (tabMargem)
            {
                divPrecoTabelaMargem.Visible = true;
                divPrecoTabelaPadrao.Visible = false;
                gridTabelaPrecoMargem.DataSource = merc.arrPrecosPromocionais;
                gridTabelaPrecoMargem.DataBind();
            }
            else
            {
                divPrecoTabelaMargem.Visible = false;
                divPrecoTabelaPadrao.Visible = true;
                gridTabelaPreco.DataSource = merc.precosPromocionais();
                gridTabelaPreco.DataBind();
            }


            gridPrecoLojas.DataSource = merc.precosLojas();
            gridPrecoLojas.DataBind();


            gridItens.DataSource = merc.itensDt();
            gridItens.DataBind();

            gridObs.DataSource = merc.tbObs();
            gridObs.DataBind();

            gridImpressoras.DataSource = merc.tbImpressoras();
            gridImpressoras.DataBind();


            ddlBandeja.SelectedValue = merc.bandeja.ToString();

            txtTribEntrada.Text = merc.descricaoTributacaoEnt;
            txtTribuSaida.Text = merc.descricaoTributacao;
            chkPisConfins.Checked = merc.Incide_Pis;
            TxtCodLinha.Text = merc.linhaCodigo.ToString();
            TxtDescricaoLinha.Text = merc.linhaDescricao;
            txtCodCorLinha.Text = merc.linhaCorCodigo.ToString();
            TxtDescricaoCorLinha.Text = merc.linhaCorDescricao;
            txtValorST.Text = merc.valorSt.ToString("N4");
            txtValorLucro.Text = merc.valorLucroReal.ToString("N4");
            txtValorPisConfins.Text = merc.valorPisCofins.ToString("N4");
            txtValorIcms.Text = merc.valorIcms.ToString("N4");
            txtPrecoReferencia.Text = merc.precoReferencia.ToString("N2");
            txtFator.Text = merc.Fator_conversao.ToString("N2");

            chkAlcoolico.Checked = merc.alcoolico;

            txtPisEntrada.Text = merc.pis_perc_entrada.ToString("N2");
            txtCofinsEntrada.Text = merc.cofins_perc_entrada.ToString("N2");

            txtPisSaida.Text = merc.pis_perc_saida.ToString("N2");
            txtCofinsSaida.Text = merc.cofins_perc_saida.ToString("N2");

            txtArtigo.Text = merc.artigo;
            txtPauta.Text = merc.pauta.ToString();
            txtNExcecao.Text = merc.numero_excecao;
            txtNatReceita.Text = merc.codigo_natureza_receita;
            ddlModalidadeBaseST.SelectedValue = merc.modalidade_BCICMSST.ToString();

            txtPorcao.Text = merc.porcao.ToString("N0");
            ddlMedida.SelectedValue = (merc.porcao_medida.Equals("") ? "0" : merc.porcao_medida);
            txtPorcaoNumero.Text = merc.porcao_numero.ToString("N0");
            ddlDiv.SelectedValue = (merc.porcao_div.Equals("") ? "0" : merc.porcao_div);
            ddlDetalhe.SelectedValue = (merc.porcao_detalhe.Equals("") ? "00" : merc.porcao_detalhe.Trim().PadLeft(2, '0'));
            chkVlr_energetico_nao.Checked = merc.vlr_energ_nao;
            txtvlr_energ_qtde.Text = merc.vlr_energ_qtde.ToString("N0");
            txtvlr_energ_qtde_igual.Text = merc.vlr_energ_qtde_igual.ToString();
            txtvlr_energ_diario.Text = merc.vlr_energ_diario.ToString();
            chkCarboidratos_nao.Checked = merc.carboidratos_nao;
            txtcarboidratos_qtde.Text = merc.carboidratos_qtde.ToString("N1");
            txtcarboidratos_vlr_diario.Text = merc.carboidratos_vlr_diario.ToString();
            chkProteinas_nao.Checked = merc.proteinas_nao;
            txtproteinas_qtde.Text = merc.proteinas_qtde.ToString("N1");
            txtproteinas_vlr_diario.Text = merc.proteinas_vlr_diario.ToString();
            chkgorduras_totais_nao.Checked = merc.gorduras_totais_nao;
            txtgorduras_totais_qtde.Text = merc.gorduras_totais_qtde.ToString("N1");
            txtgorduras_totais_vlr_diario.Text = merc.gorduras_totais_vlr_diario.ToString();
            chkgorduras_satu_nao.Checked = merc.gorduras_satu_nao;
            txtgorduras_satu_qtde.Text = merc.gorduras_satu_qtde.ToString("N1");
            txtgorduras_satu_vlr_diario.Text = merc.gorduras_satu_vlr_diario.ToString();
            chkgorduras_trans_nao.Checked = merc.gorduras_trans_nao;
            txtgorduras_trans_qtde.Text = merc.gorduras_trans_qtde.ToString("N1");
            chkfibra_alimen_nao.Checked = merc.fibra_alimen_nao;
            txtfibra_alimen_qtde.Text = merc.fibra_alimen_qtde.ToString("N1");
            txtfibra_alimen_vlr_diario.Text = merc.fibra_alimen_vlr_diario.ToString();
            chksodio_nao.Checked = merc.sodio_nao;
            txtsodio_qtde.Text = merc.sodio_qtde.ToString("N1");
            txtsodio_vlr_diario.Text = merc.sodio_vlr_diario.ToString();
            txtCodCategoria.Text = merc.Categoria;
            txtDescCategoria.Text = merc.CategoriaDesc;
            txtCodSeguimento.Text = merc.Seguimento;
            txtDescSeguimento.Text = merc.SeguimentoDesc;
            txtCodSubSeguimento.Text = merc.SubSeguimento;
            txtDescSubSeguimento.Text = merc.SubSeguimentoDesc;
            txtCodGrupoCategoria.Text = merc.GrupoCategoria;
            txtDescGrupoCategoria.Text = merc.GrupoCategoriaDesc;
            txtCodSubGrupoCategoria.Text = merc.SubGrupoCategoria;
            txtDescSubGrupoCategoria.Text = merc.SubGrupoCategoriaDesc;

            chkcolesterol_nao.Checked = merc.colesterol_nao;
            txtcolesterol_qtde.Text = merc.colesterol_qtde.ToString("N1");
            txtcolesterol_vlr_diario.Text = merc.colesterol_vlr_diario.ToString();
            chkcalcio_nao.Checked = merc.calcio_nao;
            txtcalcio_qtde.Text = merc.calcio_qtde.ToString("N1");
            txtcalcio_vlr_diario.Text = merc.calcio_vlr_diario.ToString();
            chkferro_nao.Checked = merc.ferro_nao;
            txtferro_qtde.Text = merc.ferro_qtde.ToString("N1");
            txtferro_vlr_diario.Text = merc.ferro_vlr_diario.ToString();




            if (!merc.erroCarregar.Trim().Equals(""))
            {
                msgShow("Não encontrado no banco os campos:" + merc.erroCarregar, true);

            }
            ddlOrigem.SelectedValue = merc.origem.ToString();
            txtDescricaoComercial.Text = merc.Descricao_Comercial;
            txtEcommercer.Value = merc.strEcommerce;
            chkIntegraWS.Checked = merc.IntegraWS;
            chkAtivoeCommerce.Checked = merc.Ativo_Ecommerce;
            txtcBenef.Text = merc.cBenef;
            ddlIndEscala.SelectedValue = (merc.indEscala ? "S" : "N");
            txtCNPJFabricante.Text = merc.cnpjFabricante.ToString();

            chkPratoDia.Checked = merc.Prato_dia;
            chkPratoDia_1.Checked = merc.Prato_dia_1;
            chkPratoDia_2.Checked = merc.Prato_dia_2;
            chkPratoDia_3.Checked = merc.Prato_dia_3;
            chkPratoDia_4.Checked = merc.Prato_dia_4;
            chkPratoDia_5.Checked = merc.Prato_dia_5;
            chkPratoDia_6.Checked = merc.Prato_dia_6;
            chkPratoDia_7.Checked = merc.Prato_dia_7;

            //txtFilialProduzido.Text = merc.filial_produzido;
            ddlTipoProducao.Text = merc.tipo_producao;
            ddlAtivarCE.SelectedValue = merc.ativa_ce.ToString();
            txtCodDepartamentoCE.Text = merc.codigo_departamento_ce;
            txtDepartamentoCEDescricao.Text = merc.descricao_departamento_ce;
            txtPontosFidelizacao.Text = merc.pontos_fidelizacao.ToString();

            chkExcluirProximaIntegracao.Checked = merc.Excluir_proxima_integracao;
            txtIdEcommercer.Text = merc.id_Ecommercer;
            txtIDCategoriaeCommerce.Text = merc.Categoria_eCommerce;

            txtAltura.Text = merc.altura.ToString("N3");
            txtLargura.Text = merc.largura.ToString("N3");
            txtProfundidade.Text = merc.profundidade.ToString("N3");

            txtCodCategoria.Text = merc.Categoria;
            txtDescCategoria.Text = merc.CategoriaDesc;
            txtCodSeguimento.Text = merc.Seguimento;
            txtDescSeguimento.Text = merc.SeguimentoDesc;
            txtCodSubSeguimento.Text = merc.SubSeguimento;
            txtDescSubSeguimento.Text = merc.SubSeguimentoDesc;
            txtCodGrupoCategoria.Text = merc.GrupoCategoria;
            txtDescGrupoCategoria.Text = merc.GrupoCategoriaDesc;
            txtCodSubGrupoCategoria.Text = merc.SubGrupoCategoria;
            txtDescSubGrupoCategoria.Text = merc.SubGrupoCategoriaDesc;

            chkImpAux.Checked = merc.impAux;
            ChkVendaComSenha.Checked = merc.vendaComSenha;

            txtPLUVinculado.Text = merc.PLU_Vinculado;
            txtFatorVinculado.Text = merc.fatorEstoqueVinculado.ToString("N3");

            txtDescricaoWEB.Text = merc.descricaoWEB;
            txtSKU.Text = merc.SKU;

            chkConfiguravel.Checked = merc.Configuravel;

            txtMargemTerceiroPreco.Text = merc.margem_terceiro_preco.ToString();
            txtTerceiroPreco.Text = merc.terceiro_preco.ToString();

            txtCodigoProdutoANVISA.Text = merc.codigo_produto_ANVISA;
            txtMotivoIsencaoANVISA.Text = merc.motivo_isencao_ANVISA;
            txtPrecoMaximoANVISA.Text = merc.preco_maximo_ANVISA.ToString("N2");

            txtCodigoEmissaoNFe.Text = merc.codigoEmissaoNFe;

            atualizarMagentoCheckList(merc.magentoAtributos);

            ExibirImagens();
        }

        protected void ExibirImagens()
        {
            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
            if (merc == null)
                return;

            btnUpload.Visible = !status.Equals("visualizar");
            int qtdeImgs = Funcoes.intTry(Conexao.retornaUmValor("Select count(*)-1 from mercadoria_media where plu = '" + merc.PLU + "'", null));
            String sql = "Select * from mercadoria_media where plu = '" + merc.PLU + "' order by ordem";
            divImagens.Controls.Clear();
            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta(sql, null, false);
                while (rs.Read())
                {
                    Panel pn = new Panel()
                    {
                        ID = "pnImagem_" + rs["ORDEM"].ToString(),
                        CssClass = "pnImgEcommerce"
                    };
                    ImageButton img = new ImageButton()
                    {
                        ID = "img_" + rs["ORDEM"].ToString(),
                        ImageUrl = "data:image/" + rs["TIPO"].ToString() + ";base64," + rs["BASE"].ToString(),
                        CssClass = "ImgEcommerce"
                    };

                    img.Click += imgAbrir_Click;
                    pn.Controls.Add(img);
                    if (status != "visualizar")
                    {


                        Panel pn2 = new Panel()
                        {
                            ID = "pnAcao_" + rs["ORDEM"].ToString(),
                            CssClass = "pnAcaoOrdem"

                        };
                        if (!rs["ORDEM"].ToString().Equals("0"))
                        {
                            ImageButton imgUp = new ImageButton()
                            {
                                ID = "imgUp_" + rs["ORDEM"].ToString(),
                                ImageUrl = "../imgs/OrdemUp.png",
                                CssClass = "ImgBtnAcao"
                            };
                            imgUp.Click += imgOrdemUp_Click;
                            pn2.Controls.Add(imgUp);
                        }
                        if (qtdeImgs > Funcoes.intTry(rs["ORDEM"].ToString()))
                        {
                            ImageButton imgDown = new ImageButton()
                            {
                                ID = "imgDown_" + rs["ORDEM"].ToString(),
                                ImageUrl = "../imgs/OrdemDown.png",
                                CssClass = "ImgBtnAcao"
                            };
                            imgDown.Click += imgOrdemDown_Click;
                            pn2.Controls.Add(imgDown);
                        }

                        ImageButton imgDelete = new ImageButton()
                        {
                            ID = "imgDelete_" + rs["ORDEM"].ToString(),
                            ImageUrl = "~/img/cancel.png",
                            CssClass = "ImgBtnAcao"
                        };
                        imgDelete.Click += imgDelete_Click;
                        pn2.Controls.Add(imgDelete);
                        pn.Controls.Add(pn2);

                    }
                    divImagens.Controls.Add(pn);


                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rs != null)
                    rs.Close();

            }
        }
        //protected void ExibirImagens()
        //{
        //    MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" +urlSessao()];
        //    if (merc == null)
        //        return;
        //    String path = Server.MapPath("~/modulos/Cadastro/imgs/uploads/" + merc.PLU + "/");
        //    if (Directory.Exists(@path))
        //    {
        //        DirectoryInfo Dir = new DirectoryInfo(@path);
        //        FileInfo[] Files = Dir.GetFiles("*").OrderByDescending(f => f.CreationTime).ToArray();
        //        divImagens.Controls.Clear();
        //        foreach (FileInfo File in Files)
        //        {
        //            Panel pn = new Panel()
        //            {
        //                ID = "pn" + File.Name
        //            };

        //            ImageButton img = new ImageButton()
        //            {
        //                ID = File.Name,
        //                ImageUrl = "~/modulos/Cadastro/imgs/uploads/" + merc.PLU + "/" + File.Name,
        //                CssClass= "ImgEcommerce"
        //            };
        //            img.Click += imgAbrir_Click;
        //            if (merc.urlImage == img.ImageUrl)
        //            {
        //                Image imgDefault = new Image()
        //                {
        //                    ID = "imgDefault_" + File.Name,
        //                    ImageUrl = "../imgs/defaultSelect.png",
        //                    CssClass= "btnDefault"
        //                };
        //                pn.Controls.Add(imgDefault);
        //            }
        //            else
        //            {
        //                ImageButton imgDefault = new ImageButton()
        //                {
        //                    ID = "imgP_" + img.ImageUrl,
        //                    ImageUrl = "../imgs/default.png",
        //                    CssClass = "btnDefault"
        //                };
        //                imgDefault.Click += definirDefault_Click;
        //                pn.Controls.Add(imgDefault);
        //            }
        //            Label lbl = new Label()
        //            {
        //                ID="lblP"+File.Name,
        //                Text="Padrão"
        //            };
        //            pn.Controls.Add(lbl);
        //            pn.Controls.Add(img);
        //            divImagens.Controls.Add(pn);

        //        }
        //    }
        //}
        protected void definirDefault_Click(object sender, EventArgs e)
        {
            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];

            ImageButton img = (ImageButton)sender;
            merc.urlImage = img.ID.Replace("imgP_", "");
            Conexao.executarSql(@"update mercadoria set url_img ='" + merc.urlImage + "' where plu='" + merc.PLU + "' ");
            ExibirImagens();
        }
        protected void imgAbrir_Click(object sender, EventArgs e)
        {
            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];

            ImageButton img = (ImageButton)sender;
            string ordem = img.ID.Replace("img_", "");
            RedirectNovaAba("ImageBase64.aspx?plu=" + merc.PLU + "&ordem=" + ordem);

        }
        protected void mudarOrdem(string PLU, int OrdAnt, int OrdNova)
        {
            try
            {
                String sql = "update mercadoria_media set ordem = 99" + OrdNova + " where plu = '" + PLU + "' and ordem =" + OrdNova + "; " +
                 "update mercadoria_media set ordem = " + OrdNova + " where plu = '" + PLU + "' and ordem = " + OrdAnt + ";" +
                 "update mercadoria_media set ordem = " + OrdAnt + " where plu = '" + PLU + "' and ordem = 99" + OrdNova + ";";
                Conexao.executarSql(sql);
            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
            }
        }
        protected void imgOrdemUp_Click(object sender, EventArgs e)
        {
            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];

            ImageButton img = (ImageButton)sender;
            int OrdAnt = Funcoes.intTry(img.ID.Replace("imgUp_", ""));
            int OrdNova = OrdAnt - 1;
            this.mudarOrdem(merc.PLU, OrdAnt, OrdNova);
            ExibirImagens();
            img.Focus();
        }
        protected void imgOrdemDown_Click(object sender, EventArgs e)
        {
            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];

            ImageButton img = (ImageButton)sender;
            int OrdAnt = Funcoes.intTry(img.ID.Replace("imgDown_", ""));
            int OrdNova = OrdAnt + 1;
            this.mudarOrdem(merc.PLU, OrdAnt, OrdNova);
            ExibirImagens();
            img.Focus();
        }
        protected void imgDelete_Click(object sender, EventArgs e)
        {
            ImageButton img = (ImageButton)sender;
            lblImagemExcluir.Text = img.ID.Replace("imgDelete_", "");
            Image imgExclui = (Image)divImagens.FindControl("img_" + lblImagemExcluir.Text);
            imgConfirmaExcluir.ImageUrl = imgExclui.ImageUrl;
            Session.Remove("dubloClick");
            modalConfirmaExcluirImagem.Show();
        }

        protected void Grid_DataBound(object sender, EventArgs e)
        {

            Decimal vTotalQtde = 0;
            Decimal vTotalVlr = 0;

            foreach (GridViewRow linha in GridVendas.Rows)
            {
                vTotalQtde += Decimal.Parse(linha.Cells[1].Text);

                vTotalVlr += Decimal.Parse(linha.Cells[2].Text);
            }

            GridViewRow footer = GridVendas.FooterRow;
            if (footer != null)
            {
                footer.Cells[0].Text = "TOTAIS";
                footer.Cells[0].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[1].Text = vTotalQtde.ToString("N2");
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Right;

                footer.Cells[2].Text = vTotalVlr.ToString("N2");
                footer.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                if (vTotalQtde > 0 && vTotalVlr > 0)
                {
                    footer.Cells[3].Text = (vTotalVlr / vTotalQtde).ToString("N2");
                    footer.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                }
            }

        }

        protected void gridVendasDia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // cores para aprovação
                if (e.Row.Cells[0].Text.Substring(0, 3) == "Dom")
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }
            }

        }

        protected void GridVD_DataBound(object sender, EventArgs e)
        {

            Decimal vTotalQtde = 0;
            Decimal vTotalVlr = 0;

            foreach (GridViewRow linha in GridVendasDia.Rows)
            {
                if (isnumero(linha.Cells[1].Text))
                {
                    vTotalQtde += Decimal.Parse(linha.Cells[1].Text);

                    vTotalVlr += Decimal.Parse(linha.Cells[2].Text);
                }
            }

            GridViewRow footer = GridVendasDia.FooterRow;
            if (footer != null)
            {
                footer.Cells[0].Text = "TOTAIS";
                footer.Cells[0].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[1].Text = vTotalQtde.ToString("N2");
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Right;

                footer.Cells[2].Text = vTotalVlr.ToString("N2");
                footer.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                if (vTotalQtde > 0 && vTotalVlr > 0)
                {
                    footer.Cells[3].Text = (vTotalVlr / vTotalQtde).ToString("N2");
                    footer.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                }
            }

        }

        private void atualizarObj()
        {
            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
            User usr = (User)Session["user"];
            merc.Filial = usr.getFilial();
            merc.usuario = txtUsuario.Text;
            merc.usuarioAlteracao = usr.getNome();
            merc.PLU = TxtCodPLU.Text;
            merc.Tipo = txtCodTipo.Text;
            merc.Peso_Variavel = ddlPesoVariavel.Text;
            merc.Codigo_Portaria = txtPortaria.Text;
            merc.Codigo_Tributacao = Decimal.Parse(txtCodTribSaida.Text.Equals("") ? "0.0" : txtCodTribSaida.Text);
            merc.Codigo_Tributacao_ent = Decimal.Parse(txtCodTribEntrada.Text.Equals("") ? "0.0" : txtCodTribEntrada.Text);
            merc.codigo_Grupo = txtCodGrupo.Text;
            merc.codigo_subGrupo = txtCodSubGrupo.Text;
            merc.Codigo_departamento = txtCodDepartamento.Text;
            merc.Codigo_familia = txtCodFamilia.Text.Trim();
            merc.Descricao_departamento = txtDepartamento.Text;
            merc.Descricao = Funcoes.RemoverAcentos(txtDescricao.Text);
            merc.Descricao_resumida = Funcoes.RemoverAcentos(txtDescResumida.Text);
            merc.Descricao_familia = txtFamilia.Text;
            merc.Tecla = Decimal.Parse(txtTecla.Text.Equals("") ? "0.0" : txtTecla.Text);
            merc.Margem = Decimal.Parse(txtMargem.Text.Equals("") ? "0.0" : txtMargem.Text);
            merc.Estoque_minimo = Decimal.Parse(txtEstoqueMinimo.Text.Equals("") ? "0.0" : txtEstoqueMinimo.Text);
            merc.Embalagem = Decimal.Parse(txtEmbalagem.Text.Equals("") ? "0.0" : txtEmbalagem.Text);
            merc.Etiqueta = Decimal.Parse(txtEtiqueta.Text.Equals("") ? "0.0" : txtEtiqueta.Text);
            merc.Validade = Decimal.Parse(txtValidade.Text.Equals("") ? "0.0" : txtValidade.Text);
            Decimal novoPreco = Decimal.Parse(txtPrecoVenda.Text.Equals("") ? "0.0" : txtPrecoVenda.Text);
            if (!novoPreco.Equals(merc.Preco))
            {
                merc.Preco = novoPreco;
                merc.Imprime_etiqueta = true;
            }
            merc.Preco_promocao = Decimal.Parse(txtPrecoPromocao.Text.Equals("") ? "0.0" : txtPrecoPromocao.Text);
            merc.data_inicio = (txtDtInicioPromo.Text.Equals("") ? new DateTime() : DateTime.Parse(txtDtInicioPromo.Text));
            merc.data_fim = (txtDtFimPromo.Text.Equals("") ? new DateTime() : DateTime.Parse(txtDtFimPromo.Text));
            merc.Promocao_automatica = chkPromocaoAutomatica.Checked;
            merc.Promocao = chkPromocao.Checked;
            merc.Preco_Custo = Decimal.Parse(txtPrecoCusto.Text.Equals("") ? "0.0" : txtPrecoCusto.Text);

            Decimal.TryParse(txtQtdeAtacado.Text, out merc.qtde_atacado);
            Decimal.TryParse(txtMargemAtacado.Text, out merc.margem_atacado);
            Decimal.TryParse(txtPrecoAtacado.Text, out merc.preco_atacado);

            merc.Data_Alteracao = DateTime.Now;
            merc.IPI = Decimal.Parse(txtIpi.Text.Equals("") ? "0.0" : txtIpi.Text);
            merc.ultimo_fornecedor = txtUltimoFornecedor.Text;

            merc.Tecla_balanca = Decimal.Parse(txtTeclaBalanca.Text.Equals("") ? "0.0" : txtTeclaBalanca.Text);
            merc.Localizacao = txtLocaliza.Text;

            merc.Imprime_etiqueta = chkImprimeEtiqueta.Checked;

            merc.saldo_atual = Decimal.Parse(txtSaldo.Text.Equals("") ? "0.0" : txtSaldo.Text);
            merc.Fator_conversao = Decimal.Parse(txtFator.Text.Equals("") ? "0.0" : txtFator.Text);
            merc.Ref_fornecedor = txtReferenciaFornecedor.Text;
            merc.peso = Decimal.Parse(txtPesoBruto.Text.Equals("") ? "0.0" : txtPesoBruto.Text);
            merc.receita = txtReceita.Text;
            merc.qtde_receita = Decimal.Parse(txtQtdReceita.Text.Equals("") ? "0.0" : txtQtdReceita.Text);
            merc.Und_producao = txtUndProducao.Text;
            merc.pluReceita = txtPluReceita.Text;
            merc.progSeg = chkProgSeg.Checked;
            merc.progTer = chkProgTer.Checked;
            merc.progQua = chkProgQua.Checked;
            merc.progQui = chkProgQui.Checked;
            merc.progSex = chkProgSex.Checked;
            merc.progSab = chkProgSab.Checked;
            merc.progDom = chkProgDom.Checked;

            merc.peso_receita_unitario = Funcoes.decTry(txtpeso_receita_unitario.Text);
            //merc.filial_produzido = txtFilialProduzido.Text;
            merc.tipo_producao = ddlTipoProducao.Text;



            merc.preco_compra = Decimal.Parse(txtPrecoCompra.Text.Equals("") ? "0.0" : txtPrecoCompra.Text);

            merc.valor_ipi = Decimal.Parse(txtValorIPI.Text.Equals("") ? "0.0" : txtValorIPI.Text);
            merc.codigo_centro_custo = txtCentroCusto.Text;
            merc.venda_fracionaria = ddlVendaFracionaria.Text;

            merc.cf = txtClassifFiscal.Text;
            int.TryParse(txtCEST.Text, out merc.CEST);

            merc.und = txtUnidade.Text;

            merc.curva_a = chkCurvaA.Checked;
            merc.curva_b = chkCurvaB.Checked;
            merc.curva_c = chkCurvaC.Checked;
            merc.estoque_aviso = chkAvisaEstoqueMinimo.Checked;
            merc.artigo = txtArtigo.Text;
            merc.cobertura = (txtCoberturaDias.Text.Trim().Equals("") ? 0 : int.Parse(txtCoberturaDias.Text));
            merc.sazonal1 = (txtDtSazoA1.Text.Equals("") ? new DateTime() : DateTime.Parse(txtDtSazoA1.Text));
            merc.sazonal2 = (txtDtSazoDe1.Text.Equals("") ? new DateTime() : DateTime.Parse(txtDtSazoDe1.Text));
            merc.sazonal3 = (txtDtSazoA2.Text.Equals("") ? new DateTime() : DateTime.Parse(txtDtSazoA2.Text));
            merc.sazonal4 = (txtDtSazoDe2.Text.Equals("") ? new DateTime() : DateTime.Parse(txtDtSazoDe2.Text));
            merc.sazonal5 = (txtDtSazoA3.Text.Equals("") ? new DateTime() : DateTime.Parse(txtDtSazoA3.Text));
            merc.sazonal6 = (txtDtSazoDe3.Text.Equals("") ? new DateTime() : DateTime.Parse(txtDtSazoDe3.Text));
            merc.peso_liquido = Decimal.Parse(txtPesoLiquido.Text.Equals("") ? "0.0" : txtPesoLiquido.Text);
            merc.peso_bruto = Decimal.Parse(txtPesoBruto.Text.Equals("") ? "0.0" : txtPesoBruto.Text);
            merc.Marca = txtMarca.Text;
            merc.Incide_Pis = chkPisConfins.Checked;
            merc.cst_entrada = txtCSTEntrada.Text;
            merc.cst_saida = txtCSTSaida.Text;
            merc.CFOP = txtCFOP.Text;

            merc.margem_iva = Decimal.Parse(txtIva.Text.Equals("") ? "0.0" : txtIva.Text);

            merc.linhaCodigo = (TxtCodLinha.Text.Trim().Equals("") ? 0 : int.Parse(TxtCodLinha.Text));
            merc.linhaCorCodigo = (txtCodCorLinha.Text.Trim().Equals("") ? 0 : int.Parse(txtCodCorLinha.Text));
            merc.precoReferencia = Decimal.Parse(txtPrecoReferencia.Text.Equals("") ? "0.0" : txtPrecoReferencia.Text);
            merc.origem = int.Parse(ddlOrigem.SelectedValue);
            merc.alcoolico = chkAlcoolico.Checked;

            merc.pis_perc_entrada = Decimal.Parse(txtPisEntrada.Text.Equals("") ? "0.0" : txtPisEntrada.Text);
            merc.cofins_perc_entrada = Decimal.Parse(txtCofinsEntrada.Text.Equals("") ? "0.0" : txtCofinsEntrada.Text);

            merc.pis_perc_saida = Decimal.Parse(txtPisSaida.Text.Equals("") ? "0.0" : txtPisSaida.Text);
            merc.cofins_perc_saida = Decimal.Parse(txtCofinsSaida.Text.Equals("") ? "0.0" : txtCofinsSaida.Text);

            merc.artigo = txtArtigo.Text;
            merc.pauta = Decimal.Parse(txtPauta.Text.Equals("") ? "0.0" : txtPauta.Text);
            merc.numero_excecao = txtNExcecao.Text;
            merc.codigo_natureza_receita = txtNatReceita.Text;
            merc.modalidade_BCICMSST = int.Parse(ddlModalidadeBaseST.SelectedValue);


            Decimal.TryParse(txtPorcao.Text, out merc.porcao);
            merc.porcao_medida = ddlMedida.SelectedValue;
            Decimal.TryParse(txtPorcaoNumero.Text, out merc.porcao_numero);
            merc.porcao_div = ddlDiv.SelectedValue;
            merc.porcao_detalhe = ddlDetalhe.SelectedValue;

            merc.vlr_energ_nao = chkVlr_energetico_nao.Checked;
            Decimal.TryParse(txtvlr_energ_qtde.Text, out merc.vlr_energ_qtde);
            Decimal.TryParse(txtvlr_energ_qtde_igual.Text, out merc.vlr_energ_qtde_igual);
            Decimal.TryParse(txtvlr_energ_diario.Text, out merc.vlr_energ_diario);
            merc.carboidratos_nao = chkCarboidratos_nao.Checked;
            Decimal.TryParse(txtcarboidratos_qtde.Text, out merc.carboidratos_qtde);
            Decimal.TryParse(txtcarboidratos_vlr_diario.Text, out merc.carboidratos_vlr_diario);

            merc.proteinas_nao = chkProteinas_nao.Checked;
            Decimal.TryParse(txtproteinas_qtde.Text, out merc.proteinas_qtde);
            Decimal.TryParse(txtproteinas_vlr_diario.Text, out merc.proteinas_vlr_diario);

            merc.gorduras_totais_nao = chkgorduras_totais_nao.Checked;
            Decimal.TryParse(txtgorduras_totais_qtde.Text, out merc.gorduras_totais_qtde);
            Decimal.TryParse(txtgorduras_totais_vlr_diario.Text, out merc.gorduras_totais_vlr_diario);

            merc.gorduras_satu_nao = chkgorduras_satu_nao.Checked;
            Decimal.TryParse(txtgorduras_satu_qtde.Text, out merc.gorduras_satu_qtde);
            Decimal.TryParse(txtgorduras_satu_vlr_diario.Text, out merc.gorduras_satu_vlr_diario);

            merc.gorduras_trans_nao = chkgorduras_trans_nao.Checked;
            Decimal.TryParse(txtgorduras_trans_qtde.Text, out merc.gorduras_trans_qtde);

            merc.fibra_alimen_nao = chkfibra_alimen_nao.Checked;
            Decimal.TryParse(txtfibra_alimen_qtde.Text, out merc.fibra_alimen_qtde);
            Decimal.TryParse(txtfibra_alimen_vlr_diario.Text, out merc.fibra_alimen_vlr_diario);

            merc.sodio_nao = chksodio_nao.Checked;
            Decimal.TryParse(txtsodio_qtde.Text, out merc.sodio_qtde);
            decimal.TryParse(txtsodio_vlr_diario.Text, out merc.sodio_vlr_diario);

            merc.strEcommerce = txtEcommercer.Value;// divTextoEcommerce.InnerHtml;
            merc.Descricao_Comercial = txtDescricaoComercial.Text;
            merc.Ativo_Ecommerce = chkAtivoeCommerce.Checked;
            merc.IntegraWS = chkIntegraWS.Checked;


            merc.cBenef = txtcBenef.Text;
            merc.indEscala = ddlIndEscala.SelectedItem.Value.Equals("S");
            merc.cnpjFabricante = txtCNPJFabricante.Text;

            merc.Prato_dia = chkPratoDia.Checked;
            merc.Prato_dia_1 = chkPratoDia_1.Checked;
            merc.Prato_dia_2 = chkPratoDia_2.Checked;
            merc.Prato_dia_3 = chkPratoDia_3.Checked;
            merc.Prato_dia_4 = chkPratoDia_4.Checked;
            merc.Prato_dia_5 = chkPratoDia_5.Checked;
            merc.Prato_dia_6 = chkPratoDia_6.Checked;
            merc.Prato_dia_7 = chkPratoDia_7.Checked;

            merc.ativa_ce = Funcoes.intTry(ddlAtivarCE.SelectedItem.Value);
            merc.codigo_departamento_ce = txtCodDepartamentoCE.Text;


            merc.descricao_producao = txtDescricao_producao.Text;
            merc.und_compra = txtUndCompra.Text;
            merc.embalagem_producao = Funcoes.intTry(txtEmbalagemProducao.Text);
            merc.pontos_fidelizacao = Funcoes.intTry(txtPontosFidelizacao.Text);


            merc.Excluir_proxima_integracao = chkExcluirProximaIntegracao.Checked;
            merc.id_Ecommercer = txtIdEcommercer.Text;
            merc.Categoria_eCommerce = txtIDCategoriaeCommerce.Text;
            Decimal.TryParse(txtAltura.Text, out merc.altura);
            Decimal.TryParse(txtLargura.Text, out merc.largura);
            Decimal.TryParse(txtProfundidade.Text, out merc.profundidade);

            merc.bandeja = Funcoes.intTry(ddlBandeja.SelectedValue);
            merc.Categoria = txtCodCategoria.Text;
            merc.CategoriaDesc = txtDescCategoria.Text;
            merc.Seguimento = txtCodSeguimento.Text;
            merc.SeguimentoDesc = txtDescSeguimento.Text;
            merc.SubSeguimento = txtCodSubSeguimento.Text;
            merc.SubSeguimentoDesc = txtDescSubSeguimento.Text;
            merc.GrupoCategoria = txtCodGrupoCategoria.Text;
            merc.GrupoCategoriaDesc = txtDescGrupoCategoria.Text;
            merc.SubGrupoCategoria = txtCodSubGrupoCategoria.Text;
            merc.SubGrupoCategoriaDesc = txtDescSubGrupoCategoria.Text;

            merc.impAux = chkImpAux.Checked;
            merc.vendaComSenha = ChkVendaComSenha.Checked;

            merc.PLU_Vinculado = txtPLUVinculado.Text;
            Decimal.TryParse(txtFatorVinculado.Text, out merc.fatorEstoqueVinculado);

            merc.descricaoWEB = txtDescricaoWEB.Text;
            merc.SKU = txtSKU.Text;

            merc.Configuravel = chkConfiguravel.Checked;

            merc.margem_terceiro_preco = Funcoes.decTry(txtMargemTerceiroPreco.Text);
            merc.terceiro_preco = Funcoes.decTry(txtTerceiroPreco.Text);

            merc.codigo_produto_ANVISA = txtCodigoProdutoANVISA.Text;
            merc.motivo_isencao_ANVISA = txtMotivoIsencaoANVISA.Text;
            merc.preco_maximo_ANVISA = Funcoes.decTry(txtPrecoMaximoANVISA.Text);

            merc.codigoEmissaoNFe = txtCodigoEmissaoNFe.Text;

            //Atualizar objetos magento atributos
            merc.magentoAtributos = atualizarObjMagentoAtributos(merc.magentoAtributos);

            merc.colesterol_nao = chkcolesterol_nao.Checked;
            Decimal.TryParse(txtcolesterol_qtde.Text, out merc.colesterol_qtde);
            Decimal.TryParse(txtcolesterol_vlr_diario.Text, out merc.colesterol_vlr_diario);
            merc.calcio_nao = chkcalcio_nao.Checked;
            Decimal.TryParse(txtcalcio_qtde.Text, out merc.calcio_qtde);
            Decimal.TryParse(txtcalcio_vlr_diario.Text, out merc.calcio_vlr_diario);
            merc.ferro_nao = chkferro_nao.Checked;
            Decimal.TryParse(txtferro_qtde.Text, out merc.ferro_qtde);
            Decimal.TryParse(txtferro_vlr_diario.Text, out merc.ferro_vlr_diario);


            Session.Remove("mercadoria" + urlSessao());
            Session.Add("mercadoria" + urlSessao(), merc);

        }

        protected void imgPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/modulos/Cadastro/pages/mercadoria.aspx");
        }


        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
            if (merc != null)
            {
                Session.Remove("mercadoriaManter");
                Session.Add("mercadoriaManter", merc);
                Session.Remove("mercadoria" + urlSessao());

            }
            Response.Redirect("~/modulos/Cadastro/pages/mercadoriaDetalhes.aspx?novo=true");

        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            editar(pnBtn);
            carregarDados();
            habilitarControles(true);
            lblError.Text = "";
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/Cadastro/pages/mercadoria.aspx");
            pesquisar(pnBtn);
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            modalInativarMercadoria.Show();
        }
        protected bool validaCamposObrigatorios()
        {

            if (validaCampos(cabecalho) && validaCampos(conteudo))
            {
                if (!txtPesoBruto.Text.Equals("") || !txtPesoLiquido.Text.Equals(""))
                {
                    Decimal pesoBruto = 0;
                    Decimal pesoLiquido = 0;
                    Decimal.TryParse(txtPesoBruto.Text, out pesoBruto);
                    Decimal.TryParse(txtPesoLiquido.Text, out pesoLiquido);

                    if (pesoLiquido > pesoBruto)
                    {
                        txtPesoLiquido.BackColor = System.Drawing.Color.Red;
                        throw new Exception("Peso Liquido não pode ser Maior que o Peso Bruto!");
                    }

                }

                if (ddlIndEscala.SelectedItem.Value.Equals("N") && txtCNPJFabricante.Text.Trim().Equals(""))
                {
                    txtCNPJFabricante.BackColor = System.Drawing.Color.Red;
                    TabContainer1.ActiveTabIndex = 4;
                    throw new Exception("Produtos com Escala 'NAO' Relavante é Obrigatorio Preencher o CNPJ do Fabricante!");
                }


                User usr = (User)Session["User"];

                if (usr.filial.CRT.Equals("2"))
                {
                    Decimal cstPisCofins = Funcoes.decTry(txtCSTEntrada.Text);
                    String erro = "";
                    if (cstPisCofins < 70 || cstPisCofins > 98)
                    {
                        txtCSTEntrada.BackColor = System.Drawing.Color.Red;

                        erro = "CST PIS/Cofins não permitida para Empresa do Lucro presumido!<br>";
                    }

                    Decimal vlrPis = Funcoes.decTry(txtPisEntrada.Text);
                    Decimal vlrCofins = Funcoes.decTry(txtCofinsEntrada.Text);

                    if (vlrPis > 0)
                    {
                        txtPisEntrada.BackColor = System.Drawing.Color.Red;
                        erro += "Valor de Pis não permitido para Empresa do Lucro Presumido!<br>";
                    }
                    if (vlrCofins > 0)
                    {
                        txtCofinsEntrada.BackColor = System.Drawing.Color.Red;
                        erro += "Valor de Cofins não permitido para Empresa do Lucro Presumido!<br>";
                    }
                    if (erro.Length > 0)
                    {
                        TabContainer1.ActiveTabIndex = 4;
                        throw new Exception(erro);
                    }
                }
                //if (!txtFilialProduzido.Text.Equals(""))
                //{
                //    int qtd = Funcoes.intTry(Conexao.retornaUmValor("Select count(*) " +
                //                                                    " from filial " +
                //                                                    " where filial='" + txtFilialProduzido.Text + "' " +
                //                                                    " and produtora =1", null));
                //    if (qtd == 0)
                //    {
                //        txtFilialProduzido.BackColor = System.Drawing.Color.Red;
                //        throw new Exception("Filial Selecionada não é uma Filial Produtora !");
                //    }

                //}

                MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];

                foreach (mercadoria_obsDAO item in merc.arrMercadoriaObs)
                {
                    if (!merc.ordemObsAnterioExist(item.obrigatorioOrdem))
                    {
                        throw new Exception("Observações não podem pular ordem de Obrigatoriedade!");
                    }
                }

                if (ddlAtivarCE.SelectedItem.Value.Equals("2"))
                {
                    if (txtCodDepartamentoCE.Text.Equals(""))
                    {
                        txtCodDepartamentoCE.BackColor = System.Drawing.Color.Red;
                        throw new Exception("Departamento CE Obrigatorio para produtos com <br>CE CARGA E TELA Ativado! ");
                    }
                }
                if (txtCodTipo.Text.ToUpper().Equals("RELACIONADO"))
                {
                    if ((txtPLUVinculado.Text.Equals("") || Funcoes.decTry(txtFatorVinculado.Text) <= 0))
                    {
                        txtPLUVinculado.BackColor = System.Drawing.Color.Red;
                        txtFator.BackColor = System.Drawing.Color.Red;
                        throw new Exception("Para este tipo de produto, é obrigatório vincular outro produto! ");

                    }

                }


                return true;
            }
            else
                return false;

            //bool existe = (int.Parse(Conexao.retornaUmValor("Select COUNT(*) from PIS_CST_entrada where PIS_CST_entrada='" + txtCSTEntrada.Text + "'", null)) >= 1);
            //if (!existe)
            //{
            //    throw new Exception("CST de PIS de Entrada não existe");
            //}
            //bool existeSaida = (int.Parse(Conexao.retornaUmValor("Select COUNT(*) from PIS_CST_saida where PIS_CST_saida='" + txtCSTSaida.Text + "'", null)) >= 1);
            //if (!existeSaida)
            //{
            //    throw new Exception("CST de PIS de Saida não existe");
            //}

        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {

                User usr = (User)Session["User"];
                bool consCst = Funcoes.valorParametro("VALIDAR_CST_SAIDA", usr).ToUpper().Equals("TRUE");
                String strCargaTipo = "";
                strCargaTipo = Conexao.retornaUmValor("Select Gera_carga from tipo where tipo ='" + txtCodTipo.Text + "'", usr);
                Session.Remove("TipoCarga");
                Session.Add("TipoCarga", strCargaTipo);


                String erroTrib = "";
                txtPisSaida.BackColor = System.Drawing.Color.White;
                txtCofinsSaida.BackColor = System.Drawing.Color.White;
                txtNatReceita.BackColor = System.Drawing.Color.White;
                txtClassifFiscal.BackColor = System.Drawing.Color.White;
                txtCSTSaida.BackColor = System.Drawing.Color.White;
                TxtCodPLU.BackColor = System.Drawing.Color.White;
                txtCodTribSaida.BackColor = System.Drawing.Color.White;

                txtCSTSaida.Text = txtCSTSaida.Text.Trim().PadLeft(2, '0');

                if (TxtCodPLU.Text.Trim().Length > 6)
                {
                    TxtCodPLU.BackColor = System.Drawing.Color.Red;

                    throw new Exception("PLU INVÁLIDO, NÃO PODE TER MAIS QUE 6 DIG.");
                }

                Decimal codTrib = 0;
                Decimal.TryParse(txtCodTribSaida.Text, out codTrib);

                String strTrib = Conexao.retornaUmValor("Select Codigo_Tributacao from Tributacao where codigo_tributacao =" + codTrib.ToString(), usr);
                if (strTrib.Equals(""))
                {
                    txtCodTribSaida.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Codigo Tributação inválido");
                }



                if (consCst)
                {

                    //Validar se o código digitado está cadastrado no DB
                    strTrib = Conexao.retornaUmValor("Select cst from pis_cst_Saida where CST =" + txtCSTSaida.Text.ToString(), usr);
                    if (strTrib.Equals(""))
                    {
                        txtCSTSaida.BackColor = System.Drawing.Color.Red;
                        throw new Exception("Codigo CST Saída PIS/Cofins é inválido.");
                    }

                    if (txtCSTSaida.Text.Length < 2)
                    {
                        erroTrib = "CST de Saida Inválida <br>";
                        txtCSTSaida.BackColor = System.Drawing.Color.Red;
                    }

                    if (
                        txtCSTSaida.Text.Equals("01") ||
                        txtCSTSaida.Text.Equals("02")
                        )
                    {
                        if (
                            (txtPisSaida.Text.Equals("") || txtCofinsSaida.Text.Equals("")) || // se estiver em branco
                            (Decimal.Parse(txtPisSaida.Text) <= 0 || Decimal.Parse(txtCofinsSaida.Text) <= 0) // ou for menor ou igual a Zero
                            )
                        {
                            txtPisSaida.BackColor = System.Drawing.Color.Red;
                            txtCofinsSaida.BackColor = System.Drawing.Color.Red;
                            erroTrib = "CST de Saida Exige que os campos de PIS% e COFINS% sejam preenchidos com valores maiores que 0 <br>";
                        }
                        if (!txtNatReceita.Text.Equals(""))
                        {
                            txtNatReceita.BackColor = System.Drawing.Color.Red;
                            erroTrib += "CST de Saida Exige que o campo  Nat. Receita Esteja em branco!  ";

                        }
                    }
                    if (
                        txtCSTSaida.Text.Equals("04") ||
                        txtCSTSaida.Text.Equals("05") ||
                        txtCSTSaida.Text.Equals("06") ||
                        txtCSTSaida.Text.Equals("07") ||
                        txtCSTSaida.Text.Equals("08") ||
                        txtCSTSaida.Text.Equals("09")
                        )
                    {

                        if (
                            ((!txtPisSaida.Text.Equals("") && Decimal.Parse(txtPisSaida.Text) != 0) ||
                            (!txtCofinsSaida.Text.Equals("") && Decimal.Parse(txtCofinsSaida.Text) != 0)) // ou for Diferente a Zero
                            )
                        {
                            txtPisSaida.BackColor = System.Drawing.Color.Red;
                            txtCofinsSaida.BackColor = System.Drawing.Color.Red;
                            erroTrib = "CST de Saida Exige que os campos de PIS% e COFINS% sejam preenchidos com valor 0 <br>";
                        }


                        if (txtNatReceita.Text.Equals(""))
                        {
                            txtNatReceita.BackColor = System.Drawing.Color.Red;
                            erroTrib += "CST de Saida Exige que o campo  Nat. Receita seja preenchido!  <br> ";

                        }



                    }
                    if ((txtClassifFiscal.Text.Length != 8) || (Decimal.Parse(txtClassifFiscal.Text) < (Decimal)999.999))
                    {
                        txtClassifFiscal.BackColor = System.Drawing.Color.Red;
                        erroTrib += "Codigo NCM Inválido!  <br> ";
                    }



                }
                if (!erroTrib.Equals(""))
                {
                    lblErroTrib.Text = erroTrib;
                    lblErroTrib.ForeColor = System.Drawing.Color.Red;
                    modalErroTrib.Show();
                }
                else if (txtCodFamilia.Text.Trim().Equals("") || usr.filial.inibe_marcacao_familia)
                {
                    SalvarMercadoria();
                }
                else
                {
                    MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
                    Decimal novopreco = Decimal.Parse(txtPrecoVenda.Text);
                    Decimal novoPrecoPromocao = 0;
                    if (isnumero(txtPrecoPromocao.Text))
                    {
                        novoPrecoPromocao = Decimal.Parse(txtPrecoPromocao.Text);
                    }
                    if (!merc.Preco.Equals(novopreco) || !merc.Preco_promocao.Equals(novoPrecoPromocao))
                    {
                        GridItensFamilia.DataSource = Conexao.GetTable("select plu,descricao from mercadoria where codigo_familia='" + txtCodFamilia.Text + "'", null, false);
                        GridItensFamilia.DataBind();
                        modalConfirmaFamilia.Show();
                    }
                    else
                    {
                        SalvarMercadoria();
                    }
                }
            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
            }

        }
        protected void btnConfirmaFamilia_Click(object sender, EventArgs e)
        {
            SalvarMercadoria();
            modalConfirmaFamilia.Hide();
        }
        protected void btnCancelarFamilia_Click(object sender, EventArgs e)
        {
            modalConfirmaFamilia.Hide();
        }



        protected void
            SalvarMercadoria()
        {
            try
            {
                if (validaCamposObrigatorios())
                {
                    atualizarObj();
                    MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
                    merc.salvar(status.Equals("incluir"));

                    visualizar(pnBtn);
                    carregarDados();
                    habilitarControles(false);


                    btnEans.Visible = true;

                    msgShow("Salvo com sucesso!!", false);

                    //Exclusivo RAKUTEN
                    User usr = (User)Session["User"];
                    if (Funcoes.valorParametro("INTEGRA_WS", usr).Equals("RAKUTEN"))
                    {
                        if (merc.IntegraWS)
                        {
                            try
                            {
                                KCWSFabricante fb = new KCWSFabricante(merc.Marca, "Salvar");
                                KCWSProduto KCProd = new KCWSProduto(merc, "Salvar", usr);
                                KCWSProdutoEstoque KCProdEstoque = new KCWSProdutoEstoque("Salvar", merc.PLU, (int)merc.saldo_atual, usr);


                                KCWSProdutoCategoria prodCat = new KCWSProdutoCategoria(usr);
                                if (!merc.Codigo_departamento.Substring(0, 3).Equals("099"))
                                {
                                    prodCat.Excluir(merc.PLU, "099999999");
                                    prodCat.Excluir(merc.PLU, "099999");
                                    prodCat.Excluir(merc.PLU, "99");
                                }


                                prodCat.Salvar(merc.PLU, merc.Codigo_departamento);
                                prodCat.Salvar(merc.PLU, merc.Codigo_departamento.Substring(0, 6));
                                prodCat.Salvar(merc.PLU, merc.Codigo_departamento.Substring(0, 3));
                            }
                            catch (Exception err)
                            {
                                throw new Exception("Não foi possivel a integração com Servidor Web,  erro:" + err.Message);
                            }
                        }
                    }
                }
                else
                {
                    msgShow("Campo Obrigatorio não preenchido", true);

                }

            }
            catch (Exception err)
            {
                msgShow("Não foi possivel salvar o registro:" + err.Message, true);

            }
            ExibirImagens();
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            if (status.Equals("incluir") && !TxtCodPLU.Text.Trim().Equals(""))
            {
                Funcoes.cancelaPluUsado(TxtCodPLU.Text);
                //String existe = Conexao.retornaUmValor("select plu from mercadoria where plu='" + TxtCodPLU.Text + "'", new User());
                //if (existe.Trim().Equals(""))
                //{
                //    Conexao.executarSql("update plu set usado=0 where plu=" +);
                //}
            }

            Session.Remove("mercadoria" + urlSessao());
            Response.Redirect("~/modulos/Cadastro/pages/mercadoria.aspx");
        }
        protected override bool campoObrigatorio(Control campo)
        {
            String[] campos = { "TxtCodPLU",            //00
                                "txtDescricao",         //01
                                "txtCodGrupo",          //02
                                "txtCodSubGrupo",       //03
                                "txtCodDepartamento",   //04
                                "txtCodTribSaida",      //05
                                "txtCodTipo",           //06
                                "txtUnidade",           //07
                                "txtCSTEntrada",        //08
                                "txtCSTSaida",          //09
                                "txtPrecoVenda"         //10
                                ,"txtClassifFiscal"     //11
                                 ,(txtTipo.Text.Equals("PRODUCAO")?"ddlTipoProducao":"")
                          };
            User usr = (User)Session["User"];

            String strCargaTipo = "";
            strCargaTipo = (String)Session["TipoCarga"];
            if (strCargaTipo == null)
            {
                strCargaTipo = Conexao.retornaUmValor("Select Gera_carga from tipo where tipo ='" + txtCodTipo.Text + "'", usr);
                Session.Add("TipoCarga", strCargaTipo);
            }



            if (!strCargaTipo.Equals("1"))
            {
                campos[10] = ""; // Se não For carga O preço de Venda não é obrigatorio.
            }

            for (int i = 0; i < campos.Length; i++)
            {
                if (campo.ID.Equals(campos[i]))
                {

                    return true;
                }
            }
            return false;

        }




        protected override bool campoDesabilitado(Control campo)
        {
            try
            {


                if (campo == null || campo.ID == null)
                {
                    return false;
                }



                if (!status.Equals("incluir") && (campo.ID.Equals("TxtCodPLU") || campo.ID.Equals("ImgPlu") || campo.ID.Equals("txtIdEcommercer") || campo.ID.Equals("txtSKU")))
                {
                    return true;
                }

                if (!status.Equals("visualizar") && (campo.Parent.ID != null) && campo.Parent.ID.Equals("pnPrecoPromocao") && !campo.ID.Equals("chkPromocao") && !campo.ID.Equals("txtPontosFidelizacao"))
                {
                    return !chkPromocao.Checked;
                }
                imgDtSazoDe1.Visible = false;
                imgDtSazoDe2.Visible = false;
                imgDtSazoDe3.Visible = false;
                imgDtSazoA1.Visible = false;
                imgDtSazoA2.Visible = false;
                imgDtSazoA3.Visible = false;


                switch (campo.ID)
                {
                    case "txtEAN":
                    case "txtDtCadastro":
                    case "txtDtAlteracao":
                    case "txtPrecoCompra":
                    case "txtValorST":
                    case "txtValorLucro":
                    case "txtValorPisConfins":
                    case "txtValorIcms":
                    case "txtGrupo":
                    case "txtSubGrupo":
                    case "txtDepartamento":
                    case "txtFamilia":
                    case "TxtDescricaoLinha":
                    case "TxtDescricaoCorLinha":
                    case "txtFilial":
                    case "txtTipo":
                    case "txtSaldoAtualLoja":
                    case "txtCusto1Loja":
                    case "txtCusto2Loja":
                    case "txtEstoqueMinimoLoja":
                    case "txtCoberturaLoja":
                    case "txtUltimaEntradaLoja":
                    case "txtDataInventarioLoja":
                    case "txtMarcaLoja":
                    case "txtValidadeLoja":
                    case "txtIngredientesLoja":
                    case "txtDescricaoItem":
                    case "txtPrecoCustoItem":
                    case "txtSaldo":
                    case "txtUsuario":
                    case "txtLocaliza":
                    case "txtDtSazoA1":
                    case "txtDtSazoA2":
                    case "txtDtSazoA3":
                    case "txtDtSazoDe1":
                    case "txtDtSazoDe2":
                    case "txtDtSazoDe3":
                    case "txtImpressoraDescricao":
                    case "txtDescricaoGrupoCentroCusto":
                    case "txtDescricaoSubGrupoCentroCusto":
                    case "txtDescricaoCentroCusto":
                    case "txtLojaFilial":
                    case "txtPluReceitaDescricao":
                    case "txtCustoTotalReceita":
                    case "txtUndProducaoItem":
                    case "txtPrecoCompraItem":
                    case "txtTipoPrecoCompra":
                    case "txtCodTipoPrecoCompra":
                    case "txtDepartamentoCEDescricao":
                    case "txtPrecoCompraProducao":
                    case "txtCustoProducao":
                    case "txtDescCategoria":
                    case "txtDescSeguimento":
                    case "txtDescSubSeguimento":
                    case "txtDescGrupoCategoria":
                    case "txtDescSubGrupoCategoria":
                    case "txtUsuarioAlteracao":
                    case "txtDescricaoVinculado":
                    case "txtTbPreco":

                        return true;
                    case "txtReferenciaFornecedor":
                        User usr = (User)Session["User"];
                        bool pesqRef = Funcoes.valorParametro("UTILIZA_PESQ_REFERENCIA", usr).ToUpper().Equals("TRUE");
                        if (!pesqRef)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    default:
                        return false;
                }





            }
            catch (Exception)
            {

                return false;
            }
        }

        protected void ImgPlu_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                User usr = (User)Session["User"];
                TxtCodPLU.Text = Funcoes.getNovoPlu(usr, TxtCodPLU.Text);

                MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
                merc.PLU = TxtCodPLU.Text;
            }
            catch (Exception err)
            {
                msgShow(err.Message, true);


            }
        }

        private void preencherDepartamentos()
        {
            bool novaPesquis = false;
            User usr = (User)Session["User"];

            if (txtCodSubGrupo.Text.Equals(""))
            {
                String strQtdSub = Conexao.retornaUmValor("Select count(*) from subgrupo where codigo_grupo='" + txtCodGrupo.Text + "'", usr);
                if (strQtdSub.Equals("1"))
                {
                    SqlDataReader rsSub = null;
                    try
                    {
                        rsSub = Conexao.consulta("Select codigo_subgrupo,descricao_subgrupo from subgrupo where codigo_grupo='" + txtCodGrupo.Text + "'", usr, false);
                        if (rsSub.Read())
                        {
                            txtCodSubGrupo.Text = rsSub["codigo_subgrupo"].ToString();
                            txtSubGrupo.Text = rsSub["descricao_subgrupo"].ToString();

                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {
                        if (rsSub != null)
                            rsSub.Close();
                    }

                }
                else
                {
                    Session.Remove("campoLista" + urlSessao());
                    Session.Add("campoLista" + urlSessao(), "TxtSubGrupo");
                    novaPesquis = true;
                }
            }

            if (!novaPesquis && txtCodDepartamento.Text.Equals(""))
            {
                String strQtdSub = Conexao.retornaUmValor("Select count(*) from departamento where codigo_subgrupo='" + txtCodSubGrupo.Text + "'", usr);
                if (strQtdSub.Equals("1"))
                {
                    SqlDataReader rsSub = null;
                    try
                    {
                        rsSub = Conexao.consulta("Select codigo_departamento,descricao_departamento from departamento where codigo_subgrupo='" + txtCodSubGrupo.Text + "'", usr, false);
                        if (rsSub.Read())
                        {
                            txtCodDepartamento.Text = rsSub["codigo_departamento"].ToString();
                            txtDepartamento.Text = rsSub["descricao_departamento"].ToString();

                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {
                        if (rsSub != null)
                            rsSub.Close();
                    }

                }
                else
                {
                    Session.Remove("campoLista" + urlSessao());
                    Session.Add("campoLista" + urlSessao(), "TxtDepartamento");
                    novaPesquis = true;
                }
            }


            if (novaPesquis)
            {
                exibeLista();
            }
        }


        protected void btnFechar_Click(object sender, ImageClickEventArgs e)
        {
            String selecionado = ListaSelecionada(1);

            if (!selecionado.Equals("") && !selecionado.Equals("------"))
            {
                String listaAtual = (String)Session["campoLista" + urlSessao()];

                if (listaAtual.Equals("TxtGrupo"))
                {
                    txtCodGrupo.Text = ListaSelecionada(1);
                    txtGrupo.Text = ListaSelecionada(2);
                    txtCodSubGrupo.Text = "";
                    txtSubGrupo.Text = "";
                    txtCodDepartamento.Text = "";
                    txtDepartamento.Text = "";
                    preencherDepartamentos();
                    limparCategoria(0);
                }
                else if (listaAtual.Equals("TxtSubGrupo"))
                {
                    txtCodSubGrupo.Text = ListaSelecionada(1);
                    txtSubGrupo.Text = ListaSelecionada(2);
                    txtCodDepartamento.Text = "";
                    txtDepartamento.Text = "";
                    preencherDepartamentos();
                    limparCategoria(0);
                }
                else if (listaAtual.Equals("TxtDepartamento"))
                {
                    txtCodDepartamento.Text = ListaSelecionada(1);
                    txtDepartamento.Text = ListaSelecionada(2);
                    limparCategoria(0);
                }
                else if (listaAtual.Equals("TxtFamilia"))
                {
                    txtCodFamilia.Text = ListaSelecionada(1);
                    txtFamilia.Text = ListaSelecionada(2);

                }
                else if (listaAtual.Equals("txtCodDepartamentoCE"))
                {
                    txtCodDepartamentoCE.Text = ListaSelecionada(1);
                    txtDepartamentoCEDescricao.Text = ListaSelecionada(2);
                }
                else if (listaAtual.Equals("TxtTribuSaida"))
                {
                    txtCodTribSaida.Text = ListaSelecionada(1);
                    txtTribuSaida.Text = ListaSelecionada(2);

                }
                else if (listaAtual.Equals("TxtTribEntrada"))
                {
                    txtCodTribEntrada.Text = ListaSelecionada(1);
                    txtTribEntrada.Text = ListaSelecionada(2);

                }
                else if (listaAtual.Equals("TxtTipo"))
                {
                    txtCodTipo.Text = ListaSelecionada(1);
                    TabItens.Visible = ListaSelecionada(2).Equals("SIM");

                    divPluVinculado.Visible = ListaSelecionada(3).Equals("SIM");
                    txtPLUVinculado.Visible = ListaSelecionada(3).Equals("SIM");
                    txtDescricaoVinculado.Visible = ListaSelecionada(3).Equals("SIM");
                    txtFatorVinculado.Visible = ListaSelecionada(3).Equals("SIM");
                    txtSaldo.Visible = !ListaSelecionada(6).Equals("SIM");

                    if (!ListaSelecionada(3).Equals("SIM"))
                    {
                        txtPLUVinculado.Text = "";
                        txtFatorVinculado.Text = "0,000";
                        txtDescricaoVinculado.Text = "";
                    }

                }
                else if (listaAtual.Equals("TxtUnidade"))
                {
                    txtUnidade.Text = ListaSelecionada(1);

                }

                else if (listaAtual.Equals("txtCSTEntrada"))
                {
                    txtCSTEntrada.Text = ListaSelecionada(1);
                }
                else if (listaAtual.Equals("txtCSTSaida"))
                {
                    txtCSTSaida.Text = ListaSelecionada(1);
                }
                else if (listaAtual.Equals("txtCodLinha"))
                {
                    TxtCodLinha.Text = ListaSelecionada(1);
                    TxtDescricaoLinha.Text = ListaSelecionada(2);
                }
                else if (listaAtual.Equals("txtCodCorLinha"))
                {
                    txtCodCorLinha.Text = ListaSelecionada(1);
                    TxtDescricaoCorLinha.Text = ListaSelecionada(2);
                }
                else if (listaAtual.Equals("txtCodTbPreco"))
                {
                    txtCodTbPreco.Text = ListaSelecionada(1);
                    Decimal vPreco = Decimal.Parse(txtPrecoVenda.Text);
                    String strDesc = ListaSelecionada(2);
                    Decimal vDesc = (isnumero(strDesc) ? Decimal.Parse(strDesc) : 0);
                    Decimal vPrecotb = (vPreco - ((vPreco * vDesc) / 100));
                    txtTbPreco.Text = vPreco.ToString("N2");
                    txtTbPrecoPromocao.Text = vPrecotb.ToString("N2");
                    txtTbPrecoDesconto.Text = vDesc.ToString("N2");


                    modalTbpreco.Show();
                }
                else if (listaAtual.Equals("txtCodTbMargem"))
                {
                    txtCodTbMargem.Text = ListaSelecionada(1);
                    Decimal vCusto = Funcoes.decTry(txtPrecoCusto.Text);
                    Decimal vPreco = Funcoes.decTry(txtPrecoVenda.Text);
                    Decimal vDesc = Funcoes.decTry(ListaSelecionada(2));
                    Decimal vPrecotb = (vPreco - ((vPreco * vDesc) / 100));
                    if (vCusto > 0)
                    {
                        txtPrecoVendaTabPreco.Text = vPrecotb.ToString("N2");
                        Decimal margem = ((vPrecotb - vCusto) / vCusto) * 100;
                        txtMargTabPreco.Text = margem.ToString("N4");

                    }

                    modalTbpreco.Show();
                }
                else if (listaAtual.Equals("txtClassifFiscal"))
                {
                    txtClassifFiscal.Text = ListaSelecionada(1).PadLeft(8, '0');
                }
                else if (listaAtual.Equals("txtPluItem"))
                {
                    txtPluItem.Text = ListaSelecionada(1);
                    txtDescricaoItem.Text = ListaSelecionada(2);
                    txtPrecoCompraItem.Text = ListaSelecionada(3);
                    txtUndProducaoItem.Text = ListaSelecionada(4);
                    txtFatorConversaoItem.Text = "1";
                    txtFatorConversaoItem.Attributes.Add("onfocus", "this.select();");
                    txtFatorConversaoItem.Focus();

                }
                else if (listaAtual.Equals("txtPluItemAdc"))
                {
                    txtPluItemAdc.Text = ListaSelecionada(1);
                }
                else if (listaAtual.Equals("txtCEST"))
                {
                    txtCEST.Text = ListaSelecionada(1);
                }
                else if (listaAtual.Equals("imgBtnLojaImp"))
                {
                    txtLojaImp.Text = ListaSelecionada(1);
                    txtLojaFilial.Text = ListaSelecionada(2);
                }
                else if (listaAtual.Equals("imgBtnImpressora"))
                {
                    txtImpressora.Text = ListaSelecionada(1);
                    txtImpressoraDescricao.Text = ListaSelecionada(2);
                }
                else if (listaAtual.Equals("imgObsImpressora"))
                {
                    txtObsImpressora.Text = ListaSelecionada(1);
                }
                else if (listaAtual.Equals("txtGrupoCentroCusto"))
                {
                    txtGrupoCentroCusto.Text = selecionado;
                    txtDescricaoGrupoCentroCusto.Text = ListaSelecionada(2);
                    txtSubGrupoCentroCusto.Text =
                    txtDescricaoSubGrupoCentroCusto.Text = "";
                    txtCentroCusto.Text = "";
                    txtDescricaoCentroCusto.Text = "";
                }
                else if (listaAtual.Equals("txtSubGrupoCentroCusto"))
                {
                    txtGrupoCentroCusto.Text = Funcoes.intTry(selecionado.Substring(0, 3)).ToString();
                    txtDescricaoGrupoCentroCusto.Text = ListaSelecionada(3);
                    txtSubGrupoCentroCusto.Text = selecionado;
                    txtDescricaoSubGrupoCentroCusto.Text = ListaSelecionada(2);
                    txtCentroCusto.Text = "";
                    txtDescricaoCentroCusto.Text = "";
                }
                else if (listaAtual.Equals("TxtCentroCusto"))
                {
                    txtGrupoCentroCusto.Text = Funcoes.intTry(selecionado.Substring(0, 3)).ToString();
                    txtDescricaoGrupoCentroCusto.Text = ListaSelecionada(4);
                    txtSubGrupoCentroCusto.Text = selecionado.Substring(0, 6);
                    txtDescricaoSubGrupoCentroCusto.Text = ListaSelecionada(3);
                    txtCentroCusto.Text = selecionado;
                    txtDescricaoCentroCusto.Text = ListaSelecionada(2);

                }
                //else if (listaAtual.Equals("txtFilialProduzido"))
                //{
                //    txtFilialProduzido.Text = ListaSelecionada(1);

                //}
                else if (listaAtual.Equals("txtCFOP"))
                {
                    txtCFOP.Text = ListaSelecionada(1);
                }
                else if (listaAtual.Equals("txtUndProducao2"))
                {
                    txtUndProducao_2.Text = ListaSelecionada(1);
                }
                else if (listaAtual.Equals("txtUndCompra"))
                {
                    txtUndCompra.Text = ListaSelecionada(1);
                }
                else if (listaAtual.Equals("Categoria"))
                {
                    limparCategoria(1);
                    txtCodCategoria.Text = ListaSelecionada(1);
                    txtDescCategoria.Text = ListaSelecionada(5);
                }
                else if (listaAtual.Equals("Seguimento"))
                {

                    limparCategoria(2);
                    txtCodSeguimento.Text = ListaSelecionada(1);
                    txtDescSeguimento.Text = ListaSelecionada(4);
                }
                else if (listaAtual.Equals("SubSeguimento"))
                {

                    limparCategoria(3);
                    txtCodSubSeguimento.Text = ListaSelecionada(1);
                    txtDescSubSeguimento.Text = ListaSelecionada(4);
                }
                else if (listaAtual.Equals("GrupoCategoria"))
                {

                    limparCategoria(4);
                    txtCodGrupoCategoria.Text = ListaSelecionada(1);
                    txtDescGrupoCategoria.Text = ListaSelecionada(5);
                }
                else if (listaAtual.Equals("SubGrupoCategoria"))
                {

                    limparCategoria(5);
                    txtCodSubGrupoCategoria.Text = ListaSelecionada(1);
                    txtDescSubGrupoCategoria.Text = ListaSelecionada(6);
                }



                modalPnFundo.Hide();
            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                modalPnFundo.Show();
            }
        }

        private void limparCategoria(int nPontos)
        {
            if (nPontos == 1)
            {
                txtCodCategoria.Text = "";
                txtDescCategoria.Text = "";
            }
            if (nPontos <= 2)
            {
                txtCodSeguimento.Text = "";
                txtDescSeguimento.Text = "";

            }
            if (nPontos <= 3)
            {
                txtCodSubSeguimento.Text = "";
                txtDescSubSeguimento.Text = "";
            }
            if (nPontos <= 4)
            {
                txtCodGrupoCategoria.Text = "";
                txtDescGrupoCategoria.Text = "";
            }
            if (nPontos <= 5)
            {
                txtCodSubGrupoCategoria.Text = "";
                txtDescSubGrupoCategoria.Text = "";
            }
        }

        protected void ddlAgrupamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
            merc.filial_produzido = "";
            merc.Agrupamento_producao = ddlAgrupamento.SelectedItem.Value;
            //txtFilialProduzido.Text = merc.filial_produzido;

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
        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalPnFundo.Hide();
            String listaAtual = (String)Session["campoLista" + urlSessao()];
            Session.Remove("campoLista" + urlSessao());
            if (listaAtual.Equals("txtCodTbPreco"))
                modalTbpreco.Show();


        }

        protected void Img_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;


            String or = btn.ID.Substring(7);
            Session.Remove("campoLista" + urlSessao());
            Session.Add("campoLista" + urlSessao(), or);
            TxtPesquisaLista.Text = "";
            exibeLista();


        }

        protected void exibeLista()
        {
            lblErroPesquisa.Text = "";
            User usr = (User)Session["User"];
            String or = (String)Session["campoLista" + urlSessao()];
            String sqlLista = "";
            bool query = true;


            switch (or)
            {
                case "txtFilial":
                    lbllista.Text = "Escolha uma Filial";
                    sqlLista = "select Filial,Filial from filial where filial like '%" + TxtPesquisaLista.Text + "%'";

                    break;
                case "TxtGrupo":
                    lbllista.Text = "Escolha o Grupo";
                    sqlLista = "select codigo_grupo,descricao_grupo from grupo where descricao_grupo like '%" + TxtPesquisaLista.Text + "%'";
                    break;
                case "TxtSubGrupo":
                    lbllista.Text = "Escolha o SubGrupo";
                    sqlLista = "select codigo_Subgrupo,descricao_Subgrupo from subgrupo  where descricao_Subgrupo like '%" + TxtPesquisaLista.Text + "%'" + (txtCodGrupo.Text.Equals("") ? "" : "and codigo_grupo =" + txtCodGrupo.Text);
                    break;
                case "TxtDepartamento":
                case "txtCodDepartamentoCE":
                    lbllista.Text = "Escolha o Departamento";
                    sqlLista = "Select codigo_departamento,descricao_departamento from departamento " +
                        "where descricao_departamento like '%" + TxtPesquisaLista.Text + "%'";
                    if (!or.Equals("txtCodDepartamentoCE"))
                        sqlLista += (txtCodSubGrupo.Text.Equals("") ? "" : " and codigo_subgrupo =" + txtCodSubGrupo.Text);
                    else
                        sqlLista += " and isnull(cardapio,0) = 1";
                    break;
                case "TxtAddEanCodDepartamento":
                    lbllista.Text = "Escolha o Departamento";
                    sqlLista = "Select codigo_departamento,descricao_departamento from departamento where descricao_departamento like '%" + TxtPesquisaLista.Text + "%'";
                    break;

                case "TxtFamilia":
                    lbllista.Text = "Escolha a Familia";
                    sqlLista = "select codigo_familia,descricao_familia from familia where descricao_familia like '%" + TxtPesquisaLista.Text + "%'";
                    break;

                case "TxtTribuSaida":
                case "TxtTribEntrada":
                    lbllista.Text = "Escolha a Tributação";
                    sqlLista = "Select codigo_tributacao,descricao_Tributacao from tributacao where descricao_tributacao like '%" + TxtPesquisaLista.Text + "%'";
                    break;


                case "TxtTipo":
                    lbllista.Text = "Escolha o Tipo";
                    sqlLista = "select tipo, " +
                                   "[Permite Item]=case when ISNULL(permite_item,0)=1 then 'SIM' ELSE 'NAO'END, " +
                                   "[PLU Vinculado]=case when ISNULL(PLUAssociado,0)=1 then 'SIM' ELSE 'NAO'END,  " +
                                   "[Gera Carga] = case when ISNULL(gera_carga, 0) = 1 then 'SIM' ELSE 'NAO' END,  " +
                                   "[Compra] = case when ISNULL(compra,0)=1 then 'SIM' ELSE 'NAO'END, " +
                                   "[Estoque] = case when ISNULL(Estoque,0)=1 then 'SIM' ELSE 'NAO' END " +
                               " from tipo where tipo  like '%" + TxtPesquisaLista.Text + "%'";
                    break;
                case "TxtUnidade":
                case "txtUndCompra":
                case "txtUndProducao2":
                    lbllista.Text = "Escolha a Unidade";
                    sqlLista = "select Und,Descricao from unidade where descricao  like '%" + TxtPesquisaLista.Text + "%'";
                    break;

                case "txtCSTEntrada":
                    lbllista.Text = "Escolha o CST";
                    sqlLista = "Select CST= PIS_CST_entrada , Descricao from pis_cst_entrada where ";
                    if (usr.filial.CRT.Equals("2"))
                    {
                        sqlLista += " (PIS_CST_entrada between '70' and '99') and";
                    }
                    sqlLista += " (descricao like '%" + TxtPesquisaLista.Text + "%' or PIS_CST_entrada like '%" + TxtPesquisaLista.Text + "%')";
                    break;
                case "txtCSTSaida":
                    lbllista.Text = "Escolha o CST";
                    sqlLista = "Select CST=pis_cst_Saida   , Descricao from pis_cst_Saida where descricao like '%" + TxtPesquisaLista.Text + "%' or pis_cst_Saida like '%" + TxtPesquisaLista.Text + "%'";
                    break;
                case "txtCodLinha":
                    lbllista.Text = "Escolha a Linha";
                    sqlLista = "select codigo_linha codigo, descricao_linha Descrição from linha where codigo_linha like '%" + TxtPesquisaLista.Text + "%' or descricao_linha like '%" + TxtPesquisaLista.Text + "%'";
                    break;
                case "txtCodCorLinha":
                    lbllista.Text = "Escolha a Cor";
                    sqlLista = "select codigo_cor Codigo,descricao_cor Descrição from cor_linha where (codigo_cor like '%" + TxtPesquisaLista.Text + "%' or descricao_cor like '%" + TxtPesquisaLista.Text + "%')  and codigo_linha = " + TxtCodLinha.Text;
                    break;
                case "txtClassifFiscal":
                    lbllista.Text = "Escolha a Classificação Fiscal (NCM)";
                    sqlLista = "select  cf as NCM,descricao from cf where cf like '%" + TxtPesquisaLista.Text + "%' or descricao like '%" + TxtPesquisaLista.Text + "%'";
                    break;
                case "txtCodTbPreco":
                case "txtCodTbMargem":
                    lbllista.Text = "Escolha uma Tabela de Preços";
                    sqlLista = "select Codigo_tabela Codigo,isnull(porc,0) Desconto from tabela_preco  where codigo_tabela like '%" + TxtPesquisaLista.Text + "%' or porc like '%" + TxtPesquisaLista.Text + "%'";

                    break;
                case "txtPluItem":
                case "txtPluItemAdc":
                    lbllista.Text = "Escolha um Produto";
                    sqlLista = "select PLU,DESCRICAO,PRECO_CUSTO,UND=Und_producao,  [CUSTO PRODUCAO] = isnull(custo_producao,0) from MERCADORIA  where PLU like '%" + TxtPesquisaLista.Text + "%' or DESCRICAO like '%" + TxtPesquisaLista.Text + "%'";

                    break;
                case "txtCEST":
                    lbllista.Text = "Escolha uma CEST";
                    sqlLista = "EXEC SP_CEST_NCM '" + txtClassifFiscal.Text.Trim() + "'";
                    break;
                case "imgBtnLojaImp":
                    lbllista.Text = "Escolha a Loja";
                    sqlLista = "Select loja , filial from filial  where filial like '%" + TxtPesquisaLista.Text + "%'";
                    GridLista.DataSource = Conexao.GetTable(sqlLista, null, false);
                    query = false;
                    break;
                case "imgBtnImpressora":
                    lbllista.Text = "Escolha a Impressora";
                    sqlLista = "Select Impressora= impressora_remota, Descricao from Spool_impressoras " +
                                               " where ativo = 1 and Descricao  like '%" + TxtPesquisaLista.Text + "%'";
                    break;
                case "imgObsImpressora":
                    lbllista.Text = "Escolha a Observacao";
                    MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
                    query = false;
                    GridLista.DataSource = merc.tbObs();
                    break;

                case "txtGrupoCentroCusto":
                    lbllista.Text = "Escolha o Grupo";
                    sqlLista = "select codigo_grupo as codigo,descricao_grupo as Descricao from grupo_cc where descricao_grupo like '%" + TxtPesquisaLista.Text + "%'";
                    break;
                case "txtSubGrupoCentroCusto":
                    lbllista.Text = "Escolha o SubGrupo";
                    sqlLista = "select sg.codigo_Subgrupo as codigo ,sg.descricao_Subgrupo as Descricao,g.descricao_grupo as Grupo from subgrupo_cc as sg  inner join grupo_cc as g on sg.codigo_grupo = g.codigo_grupo " +
                        " where descricao_Subgrupo like '%" + TxtPesquisaLista.Text + "%'" + (txtGrupoCentroCusto.Text.Equals("") ? "" : "and g.codigo_grupo =" + txtGrupoCentroCusto.Text);
                    break;
                case "TxtCentroCusto":
                    lbllista.Text = "Escolha o Centro de custo";
                    sqlLista = "Select c.codigo_centro_custo codigo ,c.descricao_centro_custo Descrição , sg.descricao_subgrupo AS SubGrupo , g.descricao_grupo as Grupo " +
                        " from centro_custo  as c inner join subgrupo_cc as sg on c.codigo_subgrupo = sg.codigo_subgrupo " +
                        "    inner join grupo_cc as g on sg.codigo_grupo = g.codigo_grupo" +
                        " where (descricao_centro_custo  like '%" + TxtPesquisaLista.Text + "%'" +
                        "   or c.codigo_centro_custo like '%" + TxtPesquisaLista.Text + "%')";

                    if (txtGrupoCentroCusto.Text.Trim().Length > 0)
                        sqlLista += " and substring(c.codigo_centro_custo,1,3) = '" + txtGrupoCentroCusto.Text.PadLeft(3, '0') + "'";

                    if (txtSubGrupoCentroCusto.Text.Trim().Length > 0)
                        sqlLista += " and substring(c.codigo_centro_custo,1,6) = '" + txtSubGrupoCentroCusto.Text.PadLeft(6, '0') + "'";

                    break;
                case "txtFilialProduzido":
                    lbllista.Text = "Escolha a Filial";
                    sqlLista = "Select Filial  from filial where isnull(produtora,0)=1";
                    usr.consultaTodasFiliais = true;
                    break;
                case "txtCFOP":
                    lbllista.Text = "Escolha um Codigo de operação";
                    sqlLista = "Select  * from cfop where (cfop like '%" + TxtPesquisaLista.Text + "%' or Descricao like '%" + TxtPesquisaLista.Text + "%') and tipo =1";

                    break;
                case "Categoria":
                    lbllista.Text = "Escolha a Categoria";
                    sqlLista = "select a.codigo " +
                                     " ,Dep.Descricao_grupo as Grupo " +
                                     " ,Dep.descricao_subgrupo as SubGrupo " +
                                     " ,Dep.descricao_departamento as Departamento " +
                                     " ,a.descricao as Categoria " +
                               " from Categorias as a " +
                               "    left join W_BR_CADASTRO_DEPARTAMENTO as Dep on a.codigo_departamento = dep.Codigo_departamento " +
                              " where len(a.codigo)-len(replace(a.codigo ,'.','')) = 1 ";
                    if (txtCodDepartamento.Text.Trim().Length > 0)
                        sqlLista += " and a.codigo_departamento ='" + txtCodDepartamento.Text + "'";
                    break;

                case "Seguimento":
                    lbllista.Text = "Escolha o Seguimento";
                    sqlLista = "select a.codigo " +
                                     " ,Dep.Descricao_departamento as Departamento" +
                                     " ,Cat.descricao as Categoria " +
                                     " ,a.descricao as Seguimento " +
                               " from Categorias as a " +
                                   " left join categorias as Cat on a.codigo_departamento = cat.codigo " +
                                   " left join  W_BR_CADASTRO_DEPARTAMENTO as Dep on Cat.codigo_departamento = dep.Codigo_departamento" +
                               " where len(a.codigo)-len(replace(a.codigo ,'.','')) = 2 ";
                    if (txtCodCategoria.Text.Trim().Length > 0)
                        sqlLista += " and a.codigo_departamento ='" + txtCodCategoria.Text + "'";
                    break;

                case "SubSeguimento":
                    lbllista.Text = "Escolha o SubSeguimento";
                    sqlLista = "select a.codigo " +
                                      ",Cat.descricao as Categoria " +
                                      ",Seg.descricao as Seguimento " +
                                      ",a.descricao as SubSeguimento " +
                                " from Categorias as a " +
                                  "  Left join categorias as Seg on a.codigo_departamento = seg.codigo" +
                                   " left join categorias as Cat on Seg.codigo_departamento = cat.codigo " +
                        " where len(a.codigo)-len(replace(a.codigo ,'.','')) = 3 ";
                    if (txtCodSeguimento.Text.Trim().Length > 0)
                        sqlLista += " and a.codigo_departamento ='" + txtCodSeguimento.Text + "'";
                    break;

                case "GrupoCategoria":
                    lbllista.Text = "Escolha o Grupo";
                    sqlLista = "select  a.codigo " +
                                      ",Cat.descricao as Categoria " +
                                      ",Seg.descricao as Seguimento " +
                                      ",SubSeg.descricao as SubSeguimento " +
                                      ",a.descricao as Grupo " +
                                " from Categorias as a " +
                                 "   Left join categorias as SubSeg on a.codigo_departamento = SubSeg.codigo " +
                                 "   Left join categorias as Seg on SubSeg.codigo_departamento = seg.codigo " +
                                 "   left join categorias as Cat on Seg.codigo_departamento = cat.codigo " +
                                 "where len(a.codigo)-len(replace(a.codigo ,'.','')) = 4 ";


                    if (txtCodSubSeguimento.Text.Trim().Length > 0)
                        sqlLista += " and a.codigo_departamento ='" + txtCodSubSeguimento.Text + "'";
                    break;
                case "SubGrupoCategoria":
                    lbllista.Text = "Escolha o SubGrupo";
                    sqlLista = "Select " +
                                    " a.codigo " +
                                    " ,Cat.descricao as Categoria " +
                                    " ,Seg.descricao as Seguimento " +
                                    " ,SubSeg.descricao as SubSeguimento " +
                                    " ,GCat.descricao as Grupo " +
                                    " ,a.descricao as SubGrupo " +
                                " from Categorias as a " +
                                " left join categorias as GCat on a.codigo_departamento = GCat.CODIGO " +
                                " Left join categorias as SubSeg on GCat.codigo_departamento = SubSeg.codigo" +
                                " Left join categorias as Seg on SubSeg.codigo_departamento = seg.codigo  " +
                                " left join categorias as Cat on Seg.codigo_departamento = cat.codigo " +
                               " where len(a.codigo)-len(replace(a.codigo, '.', '')) = 5";
                    if (txtCodGrupoCategoria.Text.Trim().Length > 0)
                        sqlLista += " and a.codigo_departamento ='" + txtCodGrupoCategoria.Text + "'";
                    break;
                case "txtArtigo":
                    lbllista.Text = "Escolha um Artigo";
                    sqlLista = "SELECT Artigo, Descricao FROM Artigo_Fiscal ORDER BY Artigo";
                    break;

            }


            if (query)
            {
                GridLista.DataSource = Conexao.GetTable(sqlLista, usr, true);
            }

            GridLista.DataBind();

            if (GridLista.Rows.Count == 1)
            {
                if (!GridLista.Rows[0].Cells[1].Text.Equals("------"))
                {
                    RadioButton rdo = (RadioButton)GridLista.Rows[0].FindControl("RdoListaItem");
                    rdo.Checked = true;
                }
            }
            TxtPesquisaLista.Attributes.Add("onfocus", "this.select();");
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

        protected void btnEans_Click(object sender, ImageClickEventArgs e)
        {
            modalEan.Show();
            if (status.Equals("visualizar"))
            {
                habilitarControles(false);
                btnEans.Visible = true;
            }

            lblTituloEan.Text = "EANS";
            lblTituloEan.ForeColor = System.Drawing.Color.DarkBlue;
            btnAddEan.Visible = !status.Equals("visualizar");
            lblEanAdd.Visible = !status.Equals("visualizar");
            lblEanTitulo.Visible = !status.Equals("visualizar");
            TxtAddEan.Visible = !status.Equals("visualizar");
            EnabledControls(GridEan, !status.Equals("visualizar"));
        }

        protected void btnCancelarEan_Click(object sender, ImageClickEventArgs e)
        {
            modalEan.Hide();
            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
            if (merc.ean != null)
            {
                if (merc.ean.Rows.Count > 0)
                {
                    foreach (DataRow item in merc.ean.Rows)
                    {
                        txtEAN.Text = item[0].ToString();
                        break;
                    }
                }
                else
                {
                    txtEAN.Text = "";
                }
            }
            if (status.Equals("visualizar"))
            {
                habilitarControles(false);
                btnEans.Visible = true;
            }
        }

        protected void GridEan_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (status.Equals("incluir") || status.Equals("editar"))
            {
                MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
                DataRow rw = merc.ean.Rows[Convert.ToInt32(e.CommandArgument)];
                merc.ean.Rows.Remove(rw);
                GridEan.DataSource = merc.ean;
                GridEan.DataBind();
                Session.Remove("mercadoria" + urlSessao());
                Session.Add("mercadoria" + urlSessao(), merc);
            }
            modalEan.Show();
        }

        private void addEan()
        {
            lblTituloEan.Text = "EANS";
            lblTituloEan.ForeColor = System.Drawing.Color.DarkBlue;
            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
            merc.addEan(TxtAddEan.Text);
            GridEan.DataSource = merc.ean;
            GridEan.DataBind();
            TxtAddEan.Text = "";

            Session.Remove("mercadoria" + urlSessao());
            Session.Add("mercadoria" + urlSessao(), merc);
        }
        protected void btnAddEan_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (TxtAddEan.Text.Trim().Length >= 7)
                {
                    if (Funcoes.ValidarEAN13(TxtAddEan.Text))
                    {

                        addEan();
                    }
                    else
                    {
                        lblErroEan.Text = TxtAddEan.Text;
                        modalErroEan.Show();
                    }
                }
                else
                {
                    lblTituloEan.Text = "EAN INVALIDO";
                    lblTituloEan.ForeColor = System.Drawing.Color.Red;

                }
                modalEan.Show();
            }
            catch (Exception err)
            {
                lblTituloEan.Text = err.Message;
                lblTituloEan.ForeColor = System.Drawing.Color.Red;
                modalEan.Show();
            }
        }



        protected void txtMargem_TextChanged(object sender, EventArgs e)
        {
            if (!txtMargem.Text.Equals(""))
            {
                Decimal margem = Decimal.Parse(txtMargem.Text);
                Decimal precoCusto = Decimal.Parse((txtPrecoCusto.Text.Equals("") ? "0" : txtPrecoCusto.Text));
                txtMargem.Text = margem.ToString("");
                if (margem != 0 && precoCusto != 0)
                {
                    txtPrecoVenda.Text = (precoCusto + (precoCusto * margem / 100)).ToString();
                }
            }
            formataPrecos();
            txtPrecoVenda.Attributes.Add("onfocus", "this.select();");
            txtPrecoVenda.Focus();
            atualizarTBPreco();
        }

        protected void txtMargemAtacado_TextChanged(object sender, EventArgs e)
        {
            if (!txtMargemAtacado.Text.Equals(""))
            {
                Decimal margem = 0;
                Decimal.TryParse(txtMargemAtacado.Text, out margem);
                Decimal precoCusto = 0;
                Decimal.TryParse(txtPrecoCusto.Text, out precoCusto);

                if (margem != 0 && precoCusto != 0)
                {
                    txtPrecoAtacado.Text = (precoCusto + (precoCusto * margem / 100)).ToString();
                }
            }
            formataPrecos();
            txtPrecoAtacado.Attributes.Add("onfocus", "this.select();");
            txtPrecoAtacado.Focus();
        }

        protected void txtCodTipo_TextChanged(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            bool permiteItem = Conexao.retornaUmValor("Select permite_item from tipo where tipo ='" + txtCodTipo.Text + "'", null).Equals("1");
            if (permiteItem)
            {
                TabItens.Visible = usr.telaPermissao("Itens");
            }

            bool pluVinculado = Conexao.retornaUmValor("Select PLUAssociado from tipo where tipo ='" + txtCodTipo.Text + "'", null).Equals("1");
            if (pluVinculado)
            {
                divPluVinculado.Visible = false;
                txtPLUVinculado.Enabled = false;
                txtPLUVinculado.Text = "";
                txtFatorVinculado.Enabled = false;
                txtFatorVinculado.Text = "0,000";
            }
            else
            {
                divPluVinculado.Visible = true;
                txtPLUVinculado.Enabled = true;
                txtFatorVinculado.Enabled = true;
            }

        }
        protected void txtPrecoVenda_TextChanged(object sender, EventArgs e)
        {
            if (!txtPrecoVenda.Text.Equals(""))
            {
                Decimal precoVenda = Decimal.Parse((txtPrecoVenda.Text.Equals("") ? "0" : txtPrecoVenda.Text));
                Decimal precoCusto = Decimal.Parse((txtPrecoCusto.Text.Equals("") ? "0" : txtPrecoCusto.Text));
                if (precoVenda != 0 && precoCusto != 0)
                {
                    Decimal margem = ((precoVenda - precoCusto) / precoCusto) * 100;
                    txtMargem.Text = margem.ToString();
                }
                atualizarTBPreco();

            }
            formataPrecos();
            txtValorIPI.Attributes.Add("onfocus", "this.select();");
            txtValorIPI.Focus();
        }

        protected void atualizarTBPreco()
        {
            try
            {


                Decimal precoVenda = Funcoes.decTry(txtPrecoVenda.Text);


                MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
                merc.atualizaPrecoTabelas(precoVenda);

                gridTabelaPreco.DataSource = merc.precosPromocionais();
                gridTabelaPreco.DataBind();


                Session.Remove("mercadoria" + urlSessao());
                Session.Add("mercadoria" + urlSessao(), merc);
            }
            catch (Exception err)
            {

                msgShow(err.Message, true);
            }

        }


        protected void txtPrecoAtacado_TextChanged(object sender, EventArgs e)
        {
            if (!txtPrecoAtacado.Text.Equals(""))
            {
                Decimal precoAtacado = 0;
                Decimal precoCusto = 0;
                Decimal.TryParse(txtPrecoCusto.Text, out precoCusto);
                Decimal.TryParse(txtPrecoAtacado.Text, out precoAtacado);

                if (precoAtacado != 0 && precoCusto != 0)
                {
                    Decimal margem = ((precoAtacado - precoCusto) / precoCusto) * 100;
                    txtMargemAtacado.Text = margem.ToString();
                }
            }
            formataPrecos();
        }

        protected void txtPrecoCusto_TextChanged(object sender, EventArgs e)
        {
            if (!txtPrecoCusto.Text.Equals(""))
            {
                User usr = (User)Session["User"];
                bool pVenda = Funcoes.valorParametro("NAO_CALCULA_PRECO_VENDA", usr).ToUpper().Equals("TRUE");
                if (pVenda)
                {
                    Decimal precoVenda = 0;
                    Decimal.TryParse(txtPrecoVenda.Text, out precoVenda);
                    Decimal precoCusto = 0;
                    Decimal.TryParse(txtPrecoCusto.Text, out precoCusto);
                    if (precoVenda != 0 && precoCusto != 0)
                    {
                        Decimal margem = ((precoVenda - precoCusto) / precoCusto) * 100;
                        txtMargem.Text = margem.ToString();
                    }
                }
                else
                {
                    Decimal precoCusto = Decimal.Parse((txtPrecoCusto.Text.Equals("") ? "0" : txtPrecoCusto.Text));
                    Decimal margem = Decimal.Parse((txtMargem.Text.Equals("") ? "0" : txtMargem.Text));
                    if (margem != 0 && precoCusto != 0)
                        txtPrecoVenda.Text = (precoCusto + (precoCusto * margem / 100)).ToString();

                    Decimal precoAtacado = 0;
                    Decimal.TryParse(txtPrecoAtacado.Text, out precoAtacado);
                    if (precoAtacado != 0 && precoCusto != 0)
                    {
                        txtMargemAtacado.Text = (((precoAtacado - precoCusto) / precoCusto) * 100).ToString("N4");
                    }
                    txtMargemTerceiroPreco_TextChanged(sender, e);
                    atualizarTBPreco();
                }
            }


            formataPrecos();
            txtMargem.Attributes.Add("onfocus", "this.select();");
            txtMargem.Focus();
        }

        private void formataPrecos()
        {
            Decimal precoVenda = Decimal.Parse(txtPrecoVenda.Text.Trim().Equals("") ? "0" : txtPrecoVenda.Text);
            Decimal precoCusto = Decimal.Parse(txtPrecoCusto.Text.Trim().Equals("") ? "0" : txtPrecoCusto.Text);
            Decimal margem = Decimal.Parse(txtMargem.Text.Trim().Equals("") ? "0" : txtMargem.Text);
            Decimal precoAtacado = 0;
            Decimal margemAtacado = 0;
            Decimal.TryParse(txtPrecoAtacado.Text, out precoAtacado);
            Decimal.TryParse(txtMargemAtacado.Text, out margemAtacado);

            txtPrecoCusto.Text = precoCusto.ToString("N2");
            txtPrecoVenda.Text = precoVenda.ToString("N2");
            txtMargem.Text = margem.ToString("N4");
            txtPrecoAtacado.Text = precoAtacado.ToString("N2");
            txtMargemAtacado.Text = margemAtacado.ToString("N4");

        }

        protected void chkPromocao_CheckedChanged(object sender, EventArgs e)
        {
            EnabledControls(pnPrecoPromocao, chkPromocao.Checked);
            chkPromocao.Enabled = true;
            if (!chkPromocao.Checked)
            {
                txtDtInicioPromo.Text = "";
                txtDtFimPromo.Text = "";
                txtPrecoPromocao.Text = "0,00";
                chkPromocaoAutomatica.Checked = false;
            }
            else
            {
                txtDtInicioPromo.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtDtFimPromo.Text = DateTime.Now.ToString("dd/MM/yyyy");

            }
        }

        protected void btnConfirmaManter_Click(object sender, ImageClickEventArgs e)
        {

            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];

            merc.PLU = "";
            merc.saldo_atual = 0;
            merc.Data_Cadastro = DateTime.Now;
            merc.mercadoriasLoja = new ArrayList();
            //merc.limparPrecoPromocao();
            merc.Promocao = false;
            merc.Promocao_automatica = false;
            merc.Preco_promocao = 0;
            merc.data_inicio = new DateTime();
            merc.data_fim = new DateTime();
            merc.ean = null;

            if (!chkManterPrecos.Checked)
            {
                merc.preco_compra = -1;
                merc.Preco_Custo = -1;
                merc.Margem = -1;
                merc.Preco = -1;

                merc.qtde_atacado = 0;
                merc.margem_atacado = 0;
                merc.preco_atacado = 0;
            }

            //Integração WEB
            //Sistema está preservando informações que geram problemas, por este motivo, estes campos foram zerados.
            merc.id_Ecommercer = "";
            merc.SKU = "";
            merc.IntegraWS = false;
            merc.Ativo_Ecommerce = false;

            Session.Remove("mercadoria" + urlSessao());
            Session.Add("mercadoria" + urlSessao(), merc);
            carregarDados();
            modalManterDados.Hide();

        }

        protected void btnCancelaManter_Click(object sender, ImageClickEventArgs e)
        {
            modalManterDados.Hide();
            User usr = (User)Session["User"];
            MercadoriaDAO merc = new MercadoriaDAO(usr);
            txtCodTipo.Text = "PRINCIPAL";
            txtUnidade.Text = "UN";
            txtTecla.Text = "255";
            Session.Remove("mercadoria" + urlSessao());
            Session.Add("mercadoria" + urlSessao(), merc);
        }
        protected void btnCancelaHistoricoPreco_Click(object sender, ImageClickEventArgs e)
        {
            modalHistoricoPreco.Hide();
        }

        protected void btnCancelaKardex_Click(object sender, ImageClickEventArgs e)
        {
            modalKardex.Hide();
        }
        protected void txtCodGrupo_TextChanged(object sender, EventArgs e)
        {
            if (txtCodGrupo.Text.Trim().Equals(""))
            {
                txtGrupo.Text = "";
            }
            else
            {
                try
                {
                    MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
                    merc.codigo_Grupo = txtCodGrupo.Text;
                    txtGrupo.Text = (merc.descricao_Grupo == null ? "Codigo Grupo errado" : merc.descricao_Grupo);
                    txtGrupo.BackColor = (merc.descricao_Grupo == null ? System.Drawing.Color.Red : System.Drawing.Color.FromArgb(0xDCDCDC));
                    txtCodSubGrupo.Attributes.Add("onfocus", "this.select();");
                    txtCodSubGrupo.Focus();

                }
                catch (Exception)
                {
                    txtGrupo.Text = "Codigo Grupo errado";
                }

            }
        }

        protected void txtCodSubGrupo_TextChanged(object sender, EventArgs e)
        {

            if (txtCodSubGrupo.Text.Trim().Equals(""))
            {
                txtSubGrupo.Text = "";
            }
            else
            {
                try
                {
                    MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
                    merc.codigo_subGrupo = txtCodSubGrupo.Text;
                    txtSubGrupo.Text = (merc.descricao_subGrupo == null ? " Codigo Grupo errado " : merc.descricao_subGrupo);
                    txtSubGrupo.BackColor = (merc.descricao_subGrupo == null ? System.Drawing.Color.Red : System.Drawing.Color.FromArgb(0xDCDCDC));
                    txtCodDepartamento.Attributes.Add("onfocus", "this.select();");
                    txtCodDepartamento.Focus();

                }
                catch (Exception)
                {
                    txtSubGrupo.Text = "Codigo SubGrupo errado";
                }

            }
        }



        protected void GridLista_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        protected void txtCodDepartamento_TextChanged(object sender, EventArgs e)
        {
            if (txtCodDepartamento.Text.Trim().Equals(""))
            {
                txtDepartamento.Text = "";
            }
            else
            {
                try
                {
                    MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
                    merc.Codigo_departamento = txtCodDepartamento.Text;
                    txtDepartamento.Text = (merc.Descricao_departamento == null ? " Codigo Departamento errado " : merc.Descricao_departamento);
                    txtDepartamento.BackColor = (merc.Descricao_departamento == null ? System.Drawing.Color.Red : System.Drawing.Color.FromArgb(0xDCDCDC));
                    txtCodFamilia.Attributes.Add("onfocus", "this.select();");
                    txtCodFamilia.Focus();

                }
                catch (Exception)
                {
                    txtDepartamento.Text = "Codigo Departamento errado";

                }
            }
        }

        protected void btnAddTbPreco_Click(object sender, ImageClickEventArgs e)
        {
            User usr = (User)Session["User"];
            Session.Remove("preco_promocional" + urlSessao());
            bool tabMargem = Funcoes.valorParametro("TAB_PRECO_MARG", usr).ToUpper().Equals("TRUE");
            lblTituloTabelaPreco.Visible = !tabMargem;
            divExcluirTabelaPreco.Visible = false;
            atualizarObj();
            if (tabMargem)
            {
                txtPrecoTabCompra.Text = txtPrecoCompra.Text;
                txtPrecoTabCusto.Text = txtPrecoCusto.Text;
                txtPrecoTabMargem.Text = txtMargem.Text;
                txtPrecoTabVenda.Text = txtPrecoVenda.Text;
                txtPrecoTabCompra.BackColor = txtPrecoCompra.BackColor;
                txtPrecoTabCusto.BackColor = txtPrecoCompra.BackColor;
                txtPrecoTabMargem.BackColor = txtPrecoCompra.BackColor;
                txtPrecoTabVenda.BackColor = txtPrecoCompra.BackColor;
            }

            modalTbpreco.Show();
        }


        protected void btnConfirmaTbPreco_Click(object sender, ImageClickEventArgs e)
        {
            User usr = (User)Session["user"];
            bool novo = false;
            preco_mercadoriaDAO preco = (preco_mercadoriaDAO)Session["preco_promocional" + urlSessao()];

            if (preco == null)
            {
                novo = true;
                preco = new preco_mercadoriaDAO();
            }

            bool tabMargem = Funcoes.valorParametro("TAB_PRECO_MARG", usr).ToUpper().Equals("TRUE");
            if (tabMargem)
            {
                preco.Codigo_tabela = txtCodTbMargem.Text;
                preco.Preco = Funcoes.decTry(txtPrecoTabVenda.Text);
                preco.Preco_promocao = Funcoes.decTry(txtPrecoVendaTabPreco.Text);
                preco.PrecoCusto = Funcoes.decTry(txtPrecoTabCusto.Text);

                //Decimal vDesc = preco.Preco_promocao - preco.Preco;
                Decimal vDesc = preco.Preco - preco.Preco_promocao;

                preco.Desconto = (vDesc / preco.Preco) * 100;

            }
            else
            {
                preco.Codigo_tabela = txtCodTbPreco.Text;
                preco.Preco = (txtTbPreco.Text.Equals("") ? 0 : Decimal.Parse(txtTbPreco.Text));
                preco.Desconto = (txtTbPrecoDesconto.Text.Equals("") ? 0 : Decimal.Parse(txtTbPrecoDesconto.Text));
                preco.Preco_promocao = (txtTbPrecoPromocao.Text.Equals("") ? 0 : Decimal.Parse(txtTbPrecoPromocao.Text));
            }
            if (novo)
            {
                MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
                merc.addPrecoPromocao(preco);
                Session.Remove("mercadoria" + urlSessao());
                Session.Add("mercadoria" + urlSessao(), merc);

            }

            modalTbpreco.Hide();
            carregarDados();

        }


        protected void btnCancelaTbPreco_Click(object sender, ImageClickEventArgs e)
        {
            modalTbpreco.Hide();
        }

        protected void gridTabelaPreco_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (status.Equals("incluir") || status.Equals("editar"))
            {
                divExcluirTabelaPreco.Visible = true;
                MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
                if (merc.arrPrecosPromocionais.Count == 0)
                {
                    return;
                }
                preco_mercadoriaDAO preco = merc.arrPrecosPromocionais[Convert.ToInt32(e.CommandArgument)];
                User usr = (User)Session["User"];
                Session.Remove("preco_promocional" + urlSessao());
                Session.Add("preco_promocional" + urlSessao(), preco);
                bool tabMargem = Funcoes.valorParametro("TAB_PRECO_MARG", usr).ToUpper().Equals("TRUE");
                lblTituloTabelaPreco.Visible = !tabMargem;
                if (tabMargem)
                {
                    txtPrecoTabCompra.Text = txtPrecoCompra.Text;
                    txtPrecoTabCusto.Text = txtPrecoCusto.Text;
                    txtPrecoTabMargem.Text = txtMargem.Text;
                    txtPrecoTabVenda.Text = txtPrecoVenda.Text;

                    txtCodTbMargem.Text = preco.Codigo_tabela;
                    txtPrecoTabVenda.Text = preco.Preco.ToString();
                    txtPrecoVendaTabPreco.Text = preco.Preco_promocao.ToString();
                    txtPrecoTabCusto.Text = preco.PrecoCusto.ToString();
                    txtMargTabPreco.Text = preco.Margem.ToString();
                    txtPrecoTabCompra.BackColor = txtPrecoCompra.BackColor;
                    txtPrecoTabCusto.BackColor = txtPrecoCompra.BackColor;
                    txtPrecoTabMargem.BackColor = txtPrecoCompra.BackColor;
                    txtPrecoTabVenda.BackColor = txtPrecoCompra.BackColor;
                }
                else
                {
                    txtCodTbPreco.Text = preco.Codigo_tabela.ToString();
                    txtTbPreco.Text = preco.Preco.ToString();
                    txtTbPrecoDesconto.Text = preco.Desconto.ToString();
                    txtTbPrecoPromocao.Text = preco.Preco_promocao.ToString();
                }
                modalTbpreco.Show();
            }

        }
        protected void gridPrecoLojas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
            mercadoria_lojaDAO loja = merc.precoLojaObj(gridPrecoLojas.Rows[index].Cells[1].Text);
            Session.Remove("objLoja" + urlSessao());
            Session.Add("objLoja" + urlSessao(), loja);

            carredarDadosLoja();
            modalPrecoLoja.Show();

        }
        protected void gridObs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (!gridObs.Rows[index].Cells[1].Text.Equals("------"))
            {

                MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
                String obs = gridObs.Rows[index].Cells[1].Text;
                String pluAdc = gridObs.Rows[index].Cells[2].Text.Replace("&nbsp;", "");

                merc.removeObs(obs, pluAdc);

                Session.Remove("mercadoria" + urlSessao());
                Session.Add("mercadoria" + urlSessao(), merc);
                carregarDados();

            }
        }


        protected void carredarDadosLoja()
        {
            mercadoria_lojaDAO loja = (mercadoria_lojaDAO)Session["objLoja" + urlSessao()];
            txtFilial.Text = loja.Filial;
            txtTipo.Text = loja.Tipo;
            txtPrecoCustoLoja.Text = loja.Preco_Custo.ToString();
            txtPrecoCompraLoja.Text = loja.Preco_Compra.ToString();
            txtMargemLoja.Text = loja.Margem.ToString();
            txtPrecoLoja.Text = loja.Preco.ToString();
            txtSaldoAtualLoja.Text = loja.Saldo_Atual.ToString();
            txtCusto1Loja.Text = loja.Preco_Custo_1.ToString();
            txtCusto2Loja.Text = loja.Preco_Custo_2.ToString();
            txtEstoqueMinimoLoja.Text = loja.Estoque_Minimo.ToString();
            txtCoberturaLoja.Text = loja.Cobertura.ToString();
            txtUltimaEntradaLoja.Text = loja.Ultima_Entrada.ToString("dd/MM/yyyy");
            txtMarcaLoja.Text = loja.marca;
            txtValidadeLoja.Text = loja.validade.ToString();
            chkPromocaoLoja.Checked = loja.Promocao;
            chkPromocaoAutomaticaLoja.Checked = loja.Promocao_automatica;
            txtPrecoPromocaoLoja.Text = loja.Preco_Promocao.ToString();
            txtMargemPromocaoLoja.Text = loja.Margem_Promocao.ToString();
            txtDataInicioLoja.Text = loja.Data_Inicio.ToString("dd/MM/yyyy");
            txtDataFimLoja.Text = loja.Data_Fim.ToString("dd/MM/yyyy");
            txtQtdeAtacadoLoja.Text = loja.qtde_atacado.ToString("N2");
            txtMargemAtacadoLoja.Text = loja.margem_atacado.ToString("N4");
            txtPrecoAtacadoLoja.Text = loja.preco_atacado.ToString("N2");
            txtIngredientesLoja.Text = loja.ingredientes;


        }
        protected void carregarDadosObjLoja()
        {
            mercadoria_lojaDAO loja = (mercadoria_lojaDAO)Session["objLoja" + urlSessao()];
            loja.Preco_Custo = (txtPrecoCustoLoja.Text.Trim().Equals("") ? 0 : Decimal.Parse(txtPrecoCustoLoja.Text));
            loja.Margem = (txtMargemLoja.Text.Trim().Equals("") ? 0 : Decimal.Parse(txtMargemLoja.Text));
            loja.Preco = (txtPrecoLoja.Text.Trim().Equals("") ? 0 : Decimal.Parse(txtPrecoLoja.Text));
            loja.Promocao = chkPromocaoLoja.Checked;
            loja.Promocao_automatica = chkPromocaoAutomaticaLoja.Checked;
            loja.Data_Inicio = (txtDataInicioLoja.Text.Equals("") ? new DateTime() : DateTime.Parse(txtDataInicioLoja.Text));
            loja.Data_Fim = (txtDataFimLoja.Text.Equals("") ? new DateTime() : DateTime.Parse(txtDataFimLoja.Text));

            Decimal.TryParse(txtQtdeAtacadoLoja.Text, out loja.qtde_atacado);
            Decimal.TryParse(txtMargemAtacadoLoja.Text, out loja.margem_atacado);
            Decimal.TryParse(txtPrecoAtacadoLoja.Text, out loja.preco_atacado);


            Session.Remove("objLoja" + urlSessao());
            Session.Add("objLoja" + urlSessao(), loja);
        }


        protected void btnConfirmaPrecoLoja_Click(object sender, ImageClickEventArgs e)
        {

            carregarDadosObjLoja();

            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
            mercadoria_lojaDAO loja = (mercadoria_lojaDAO)Session["objLoja" + urlSessao()];
            merc.precoLojaObjAtualiza(loja);

            Session.Remove("objLoja" + urlSessao());
            Session.Remove("mercadoria" + urlSessao());
            Session.Add("mercadoria" + urlSessao(), merc);

            gridPrecoLojas.DataSource = merc.precosLojas();
            gridPrecoLojas.DataBind();
            modalPrecoLoja.Hide();
        }

        protected void btnCancelaPrecoLoja_Click(object sender, ImageClickEventArgs e)
        {
            modalPrecoLoja.Hide();
        }


        protected void txtPrecoCustoLoja_TextChanged(object sender, EventArgs e)
        {
            if (!txtPrecoCusto.Text.Equals(""))
            {
                Decimal precoCusto = Decimal.Parse((txtPrecoCustoLoja.Text.Equals("") ? "0" : txtPrecoCustoLoja.Text));
                Decimal margem = Decimal.Parse((txtMargemLoja.Text.Equals("") ? "0" : txtMargemLoja.Text));
                if (margem != 0 && precoCusto != 0)
                    txtPrecoLoja.Text = (precoCusto + (precoCusto * margem / 100)).ToString();

            }
            txtMargemLoja.Attributes.Add("onfocus", "this.select();");
            txtMargemLoja.Focus();
            modalPrecoLoja.Show();
        }
        protected void txtMargemLoja_TextChanged(object sender, EventArgs e)
        {
            if (!txtMargemLoja.Text.Equals(""))
            {
                Decimal margem = Decimal.Parse(txtMargemLoja.Text);
                Decimal precoCusto = Decimal.Parse((txtPrecoCustoLoja.Text.Equals("") ? "0" : txtPrecoCustoLoja.Text));
                txtMargem.Text = margem.ToString("");
                if (margem != 0 && precoCusto != 0)
                {
                    txtPrecoLoja.Text = (precoCusto + (precoCusto * margem / 100)).ToString();
                }
            }
            txtPrecoLoja.Attributes.Add("onfocus", "this.select();");
            txtPrecoLoja.Focus();
            modalPrecoLoja.Show();
        }
        protected void txtPrecoLoja_TextChanged(object sender, EventArgs e)
        {
            if (!txtPrecoLoja.Text.Equals(""))
            {
                Decimal precoVenda = Decimal.Parse((txtPrecoLoja.Text.Equals("") ? "0" : txtPrecoLoja.Text));
                Decimal precoCusto = Decimal.Parse((txtPrecoCustoLoja.Text.Equals("") ? "0" : txtPrecoCustoLoja.Text));
                if (precoVenda != 0 && precoCusto != 0)
                {
                    Decimal margem = ((precoVenda - precoCusto) / precoCusto) * 100;
                    txtMargemLoja.Text = margem.ToString();
                }
            }
            modalPrecoLoja.Show();

        }

        protected void limparAddItem()
        {
            txtPluItem.Text = "";
            txtDescricaoItem.Text = "";
            //txtPrecoCustoItem.Text = "";
            txtFatorConversaoItem.Text = "";


        }
        protected void btnAddItem_Click(object sender, ImageClickEventArgs e)
        {
            limparAddItem();

            Session.Add("campoLista" + urlSessao(), "txtPluItem");
            exibeLista();

            //modaladdItens.Show();
        }
        protected void btnConfirmaAddItem_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (txtPluItem.Text.Equals(""))
                {
                    Session.Add("campoLista" + urlSessao(), "txtPluItem");

                    exibeLista();
                    return;
                }
                else if (txtFatorConversaoItem.Text.Trim().Equals(""))
                {
                    String sql = "Select descricao, preco_custo, und_producao from mercadoria where plu ='" + txtPluItem.Text.Trim() + "'";
                    SqlDataReader rs = null;

                    try
                    {
                        rs = Conexao.consulta(sql, null, false);
                        if (rs.Read())
                        {
                            txtDescricaoItem.Text = rs["descricao"].ToString();
                            txtPrecoCompraItem.Text = Funcoes.decTry(rs["preco_custo"].ToString()).ToString("N2");
                            txtUndProducaoItem.Text = rs["und_producao"].ToString();
                            txtFatorConversaoItem.Text = "1";
                            txtFatorConversaoItem.Focus();
                            txtFatorConversaoItem.Attributes.Add("onfocusin", " select();");

                        }
                        else
                        {
                            txtPluItem.Focus();
                            lblError.Text = "Item não encontrado";
                        }
                    }
                    catch (Exception err)
                    {

                        lblError.Text = err.Message;
                    }
                    finally
                    {
                        if (rs != null)
                            rs.Close();
                    }

                    return;
                }
                else if (txtQtdeItem.Text.Equals(""))
                {
                    txtQtdeItem.Focus();
                }
                else
                {


                    MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];

                    itemDAO it = new itemDAO();
                    it.Plu_item = txtPluItem.Text;
                    it.Descricao = txtDescricaoItem.Text;
                    it.Preco_compra = Funcoes.decTry(txtPrecoCompraItem.Text);
                    it.Und = txtUndProducaoItem.Text;
                    it.Fator_conversao = Funcoes.decTry(txtFatorConversaoItem.Text);
                    it.Qtde = Funcoes.decTry(txtQtdeItem.Text);
                    merc.addItem(it);

                    gridItens.DataSource = merc.itensDt();
                    gridItens.DataBind();

                    txtCustoTotalReceita.Text = merc.CustoTotalReceita.ToString("N2");
                    txtPluItem.Text = "";
                    txtDescricaoItem.Text = "";
                    txtPrecoCompraItem.Text = "";
                    txtUndProducaoItem.Text = "";
                    txtFatorConversaoItem.Text = "";
                    txtQtdeItem.Text = "";
                    txtPluItem.Focus();
                    Session.Remove("mercadoria" + urlSessao());
                    Session.Add("mercadoria" + urlSessao(), merc);
                }

            }
            catch (Exception err)
            {
                lblError.Text = "Erro:" + err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
                //modaladdItens.Show();
            }
        }

        protected void gridItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (status.Equals("editar") || status.Equals("incluir"))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
                if (e.CommandName.Equals("Excluir"))
                {
                    lblPluItemExcluir.Text = gridItens.Rows[index].Cells[2].Text;
                    modalConfirmaExcluirItem.Show();

                }
                else
                {
                    itemDAO item = merc.pegarItem(index);
                    txtPluItem.Text = item.Plu_item;
                    txtDescricaoItem.Text = item.Descricao;
                    txtFatorConversaoItem.Text = item.Fator_conversao.ToString("N2");
                    txtPrecoCompraItem.Text = item.Preco_compra.ToString("N2");
                    txtQtdeItem.Text = item.Qtde.ToString("N2");
                    txtUndProducaoItem.Text = item.Und;
                    txtQtdeItem.Focus();
                    txtQtdeItem.Attributes.Add("onfocusin", " select();");

                }
            }
        }


        protected void btnConfirmaInativar_Click(object sender, ImageClickEventArgs e)
        {

            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
            merc.inativar();
            modalInativarMercadoria.Hide();
            User usr = (User)Session["User"];
            if (Funcoes.valorParametro("INTEGRA_WS", usr).Equals("RAKUTEN"))
            {
                if (merc.IntegraWS)
                {
                    try
                    {
                        KCWSProduto KCProd = new KCWSProduto(merc, "Excluir", usr);

                    }
                    catch (Exception err)
                    {
                        throw new Exception("Não foi possivel Excluir no Servidor Web,  erro:" + err.Message);
                    }
                }
            }

            msgShow("Mercadoria Inativada", false);
            LimparCampos(conteudo);
            LimparCampos(cabecalho);
        }


        protected void btnCancelarInativar_Click(object sender, ImageClickEventArgs e)
        {
            modalInativarMercadoria.Hide();
        }

        protected void btnHistorioPreco_Click(object sender, EventArgs e)
        {

            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
            
            String sql = "Select  Filial , Origem = Descricao, convert ( varchar ,Log_Preco.data,103) + ' ' + CONVERT(VARCHAR, log_Preco.Data, 108) AS  DATA, Usuario, ISNULL(custo_old, 0) as Custo_Ant, Mrg_Ant = CONVERT(DECIMAL(12,4), CASE WHEN ISNULL(CUSTO_old, 0) > 0 AND ISNULL(PRECO_old, 0) > 0 THEN  (((Preco_old - Custo_old) / Custo_old) * 100) ELSE 0 END), preco_old Vda_Ant, ISNULL(Custo_New, 0) as Custo_Atu,  Mrg_Atu = CONVERT(DECIMAL(12,4), CASE WHEN ISNULL(CUSTO_New, 0) > 0 AND ISNULL(PRECO_New, 0) > 0 THEN  (((Preco_New - Custo_New) / Custo_New) * 100) ELSE 0 END), "+
            "preco_new  Vda_Atu FROM log_preco WHERE plu='" + merc.PLU + "'  order by Log_Preco.data desc";// ;
            GridHistoricoPreco.DataSource = Conexao.GetTable(sql, null, false);
            GridHistoricoPreco.DataBind();

            for (int i=0; i < GridHistoricoPreco.Rows.Count; i++)
            {
                if (!GridHistoricoPreco.Rows[i].Cells[0].Text.ToString().Equals("----"))
                {
                    GridHistoricoPreco.Rows[i].Cells[4].HorizontalAlign = HorizontalAlign.Right;
                    GridHistoricoPreco.Rows[i].Cells[5].HorizontalAlign = HorizontalAlign.Right;
                    GridHistoricoPreco.Rows[i].Cells[6].HorizontalAlign = HorizontalAlign.Right;
                    GridHistoricoPreco.Rows[i].Cells[7].HorizontalAlign = HorizontalAlign.Right;
                    GridHistoricoPreco.Rows[i].Cells[8].HorizontalAlign = HorizontalAlign.Right;
                    GridHistoricoPreco.Rows[i].Cells[9].HorizontalAlign = HorizontalAlign.Right;
                }
            }


            modalHistoricoPreco.Show();
        }
        
        protected void btnKardex_Click(object sender, EventArgs e)
        {
            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
            String sql = "sp_br_cons_saldokardexplu '" + merc.PLU.Trim() + "'";
            GridKardex.DataSource = Conexao.GetTable(sql, null, false);
            GridKardex.DataBind();
            modalKardex.Show();
        }

        protected void btnAplicarLojas_Click(object sender, EventArgs e)
        {
            atualizarObj();
            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];

            foreach (mercadoria_lojaDAO lj in merc.mercadoriasLoja)
            {
                //lj.Preco_Compra = merc.preco_compra;
                lj.Preco_Custo = merc.Preco_Custo;
                lj.Margem = Funcoes.verificamargem(lj.Preco_Custo, merc.Preco, 0, 0);
                lj.Preco = merc.Preco;
                lj.Promocao = merc.Promocao;
                lj.Promocao_automatica = merc.Promocao_automatica;
                lj.Preco_Promocao = merc.Preco_promocao;
                lj.Data_Inicio = merc.data_inicio;
                lj.Data_Fim = merc.data_fim;
                lj.qtde_atacado = merc.qtde_atacado;
                lj.margem_atacado = Funcoes.verificamargem(lj.Preco_Custo, merc.preco_atacado, 0, 0);
                lj.preco_atacado = merc.preco_atacado;
            }
            Session.Remove("mercadoria" + urlSessao());
            Session.Add("mercadoria" + urlSessao(), merc);
            carregarDados();


        }

        protected void txtAddEAN_TextChanged(object sender, EventArgs e)
        {
            btnAddEan_Click(new object(), new ImageClickEventArgs(0, 0));
            modalEan.Show();
        }

        protected void imgAddObservacao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {


                txtAddObs.BackColor = System.Drawing.Color.White;
                if (!txtAddObs.Text.Equals(""))
                {
                    atualizarObj();
                    MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
                    int obrigatorioOrdem = Funcoes.intTry(ddlObrigatorioOrdem.Text);
                    merc.addObs(txtAddObs.Text, txtPluItemAdc.Text, chkObsObrigatorio.Checked, obrigatorioOrdem, ddlTipoObs.SelectedItem.Value);

                    Session.Remove("mercadoria" + urlSessao());
                    Session.Add("mercadoria" + urlSessao(), merc);
                    gridObs.DataSource = merc.tbObs();
                    gridObs.DataBind();

                    txtAddObs.Text = "";
                    txtPluItemAdc.Text = "";
                    chkObsObrigatorio.Checked = false;
                    ddlObrigatorioOrdem.Enabled = false;
                    ddlObrigatorioOrdem.Text = "0";


                }
                else
                {
                    txtAddObs.BackColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception err)
            {

                msgShow(err.Message, true);
            }
        }

        protected void txtCodFamilia_TextChanged(object sender, EventArgs e)
        {

            if (txtCodFamilia.Text.Trim().Equals(""))
            {
                txtFamilia.Text = "";
            }
            else
            {
                try
                {
                    txtFamilia.Text = Conexao.retornaUmValor("select top 1 descricao_familia from familia where Codigo_familia = " + txtCodFamilia.Text, null);
                }
                catch (Exception)
                {
                    txtFamilia.Text = "";
                    txtFamilia.BackColor = System.Drawing.Color.Red;
                }

            }

        }



        protected void ImgRefFornecedor_Click1(object sender, ImageClickEventArgs e)
        {

            User usr = (User)Session["user"];


            bool bPesq = Funcoes.valorParametro("UTILIZA_PESQ_REFERENCIA", usr).ToUpper().Equals("TRUE");



            string sql = "";


            if (bPesq)
            {
                string strSeqRef = "";

                DataTable dtRef = new DataTable();



                if (!txtReferenciaFornecedor.Text.Trim().Equals("") && txtReferenciaFornecedor.Text.Trim().Length == 3)
                {

                    sql = " EXEC sp_Ret_Seq_Ref_Fornecedor '" + txtReferenciaFornecedor.Text.Trim() + "'";
                    strSeqRef = Conexao.retornaUmValor(sql, usr);


                }


                //            strSeqRef = dtRef.Rows[0]["SEQ"].ToString();



                if (strSeqRef.ToString().Trim().Length > 3)
                {

                    strSeqRef = strSeqRef.ToString().Trim().PadLeft(4, '0');

                }

                else
                {

                    strSeqRef = strSeqRef.ToString().Trim().PadLeft(3, '0');

                }



                txtReferenciaFornecedor.Text = txtReferenciaFornecedor.Text.Trim() + strSeqRef;

                MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];

                merc.Ref_fornecedor = txtReferenciaFornecedor.Text;

            }
            else
            {
                if (!TxtCodPLU.Text.Equals(""))
                {
                    sql = "Select Referencia= codigo_referencia, Fornecedor  from Fornecedor_Mercadoria where plu ='" + TxtCodPLU.Text + "'";
                    gridDetalhesReferencia.DataSource = Conexao.GetTable(sql, usr, false);
                    gridDetalhesReferencia.DataBind();
                    modalDetalhesReferencia.Show();

                }
                else
                {
                    msgShow("Sem codigo de PLU para referencia.", true);
                }
            }

        }
        protected void btnConfirmaTributacao_Click(object sender, ImageClickEventArgs e)
        {
            SalvarMercadoria();
            modalErroTrib.Hide();
        }
        protected void btnCancelarTributacao_Click(object sender, ImageClickEventArgs e)
        {
            modalErroTrib.Hide();
        }

        protected void txtMargemAtacadoLoja_TextChanged(object sender, EventArgs e)
        {
            if (!txtMargemAtacadoLoja.Text.Equals(""))
            {
                Decimal margem = 0;
                Decimal.TryParse(txtMargemAtacadoLoja.Text, out margem);
                Decimal precoCusto = 0;
                Decimal.TryParse(txtPrecoCustoLoja.Text, out precoCusto);

                if (margem != 0 && precoCusto != 0)
                {
                    txtPrecoAtacadoLoja.Text = (precoCusto + (precoCusto * margem / 100)).ToString();
                }
            }
            modalPrecoLoja.Show();
        }

        protected void txtPrecoAtacadoLoja_TextChanged(object sender, EventArgs e)
        {
            if (!txtPrecoAtacadoLoja.Text.Equals(""))
            {
                Decimal precoAtacado = 0;
                Decimal precoCusto = 0;
                Decimal.TryParse(txtPrecoCustoLoja.Text, out precoCusto);
                Decimal.TryParse(txtPrecoAtacadoLoja.Text, out precoAtacado);

                if (precoAtacado != 0 && precoCusto != 0)
                {
                    Decimal margem = ((precoAtacado - precoCusto) / precoCusto) * 100;
                    txtMargemAtacadoLoja.Text = margem.ToString();
                }
            }
            modalPrecoLoja.Show();
        }

        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.ID.Equals("chkVlr_energetico_nao"))
            {
                txtvlr_energ_qtde.Enabled = !chk.Checked;
                txtvlr_energ_qtde_igual.Enabled = !chk.Checked;
                txtvlr_energ_diario.Enabled = !chk.Checked;
                if (chk.Checked)
                {
                    txtvlr_energ_qtde.Text = "";
                    txtvlr_energ_qtde_igual.Text = "";
                    txtvlr_energ_diario.Text = "";

                    txtvlr_energ_qtde.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                    txtvlr_energ_qtde_igual.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                    txtvlr_energ_diario.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);

                }
                else
                {
                    txtvlr_energ_qtde.Attributes.Add("onfocus", "this.select();");
                    txtvlr_energ_qtde.Focus();
                    txtvlr_energ_qtde.BackColor = System.Drawing.Color.White;
                    txtvlr_energ_qtde_igual.BackColor = System.Drawing.Color.White;
                    txtvlr_energ_diario.BackColor = System.Drawing.Color.White;
                }
            }
            else if (chk.ID.Equals("chkCarboidratos_nao"))
            {
                txtcarboidratos_qtde.Enabled = !chk.Checked;
                txtcarboidratos_vlr_diario.Enabled = !chk.Checked;
                if (chk.Checked)
                {
                    txtcarboidratos_qtde.Text = "";
                    txtcarboidratos_vlr_diario.Text = "";
                    txtcarboidratos_qtde.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                    txtcarboidratos_vlr_diario.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                }
                else
                {
                    txtcarboidratos_qtde.Attributes.Add("onfocus", "this.select();");
                    txtcarboidratos_qtde.Focus();
                    txtcarboidratos_qtde.BackColor = System.Drawing.Color.White;
                    txtcarboidratos_vlr_diario.BackColor = System.Drawing.Color.White;
                }
            }
            if (chk.ID.Equals("chkProteinas_nao"))
            {
                txtproteinas_qtde.Enabled = !chk.Checked;
                txtproteinas_vlr_diario.Enabled = !chk.Checked;
                if (chk.Checked)
                {
                    txtproteinas_qtde.Text = "";
                    txtproteinas_vlr_diario.Text = "";
                    txtproteinas_qtde.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                    txtproteinas_vlr_diario.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                }
                else
                {
                    txtproteinas_qtde.Attributes.Add("onfocus", "this.select();");
                    txtproteinas_qtde.Focus();

                    txtproteinas_qtde.BackColor = System.Drawing.Color.White;
                    txtproteinas_vlr_diario.BackColor = System.Drawing.Color.White;
                }
            }
            if (chk.ID.Equals("chkgorduras_totais_nao"))
            {
                txtgorduras_totais_qtde.Enabled = !chk.Checked;
                txtgorduras_totais_vlr_diario.Enabled = !chk.Checked;
                if (chk.Checked)
                {
                    txtgorduras_totais_qtde.Text = "";
                    txtgorduras_totais_vlr_diario.Text = "";
                    txtgorduras_totais_qtde.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                    txtgorduras_totais_vlr_diario.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                }
                else
                {
                    txtgorduras_totais_qtde.Attributes.Add("onfocus", "this.select();");
                    txtgorduras_totais_qtde.Focus();
                    txtgorduras_totais_qtde.BackColor = System.Drawing.Color.White;
                    txtgorduras_totais_vlr_diario.BackColor = System.Drawing.Color.White;
                }
            }
            if (chk.ID.Equals("chkgorduras_satu_nao"))
            {
                txtgorduras_satu_qtde.Enabled = !chk.Checked;
                txtgorduras_satu_vlr_diario.Enabled = !chk.Checked;
                if (chk.Checked)
                {
                    txtgorduras_satu_qtde.Text = "";
                    txtgorduras_satu_vlr_diario.Text = "";

                    txtgorduras_satu_qtde.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                    txtgorduras_satu_vlr_diario.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                }
                else
                {
                    txtgorduras_satu_qtde.Attributes.Add("onfocus", "this.select();");
                    txtgorduras_satu_qtde.Focus();

                    txtgorduras_satu_qtde.BackColor = System.Drawing.Color.White;
                    txtgorduras_satu_vlr_diario.BackColor = System.Drawing.Color.White;
                }

            }
            if (chk.ID.Equals("chkgorduras_trans_nao"))
            {
                txtgorduras_trans_qtde.Enabled = !chk.Checked;
                if (chk.Checked)
                {
                    txtgorduras_trans_qtde.Text = "";

                    txtgorduras_trans_qtde.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                }
                else
                {
                    txtgorduras_trans_qtde.Attributes.Add("onfocus", "this.select();");
                    txtgorduras_trans_qtde.Focus();
                    txtgorduras_trans_qtde.BackColor = System.Drawing.Color.White;
                }
            }
            if (chk.ID.Equals("chkfibra_alimen_nao"))
            {
                txtfibra_alimen_qtde.Enabled = !chk.Checked;
                txtfibra_alimen_vlr_diario.Enabled = !chk.Checked;
                if (chk.Checked)
                {
                    txtfibra_alimen_qtde.Text = "";
                    txtfibra_alimen_vlr_diario.Text = "";

                    txtfibra_alimen_qtde.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                    txtfibra_alimen_vlr_diario.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                }
                else
                {
                    txtfibra_alimen_qtde.Attributes.Add("onfocus", "this.select();");
                    txtfibra_alimen_qtde.Focus();

                    txtfibra_alimen_qtde.BackColor = System.Drawing.Color.White;
                    txtfibra_alimen_vlr_diario.BackColor = System.Drawing.Color.White;
                }

            }
            if (chk.ID.Equals("chksodio_nao"))
            {
                txtsodio_qtde.Enabled = !chk.Checked;
                txtsodio_vlr_diario.Enabled = !chk.Checked;
                if (chk.Checked)
                {
                    txtsodio_qtde.Text = "";
                    txtsodio_vlr_diario.Text = "";


                    txtsodio_qtde.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                    txtsodio_vlr_diario.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                }
                else
                {
                    txtsodio_qtde.Attributes.Add("onfocus", "this.select();");
                    txtsodio_qtde.Focus();

                    txtsodio_qtde.BackColor = System.Drawing.Color.White;
                    txtsodio_vlr_diario.BackColor = System.Drawing.Color.White;
                }
            }
            if (chk.ID.Equals("chkcolesterol_nao"))
            {
                txtcolesterol_qtde.Enabled = !chk.Checked;
                txtcolesterol_vlr_diario.Enabled = !chk.Checked;
                if (chk.Checked)
                {
                    txtcolesterol_qtde.Text = "";
                    txtcolesterol_qtde.Text = "";
                    txtcolesterol_qtde.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                    txtcolesterol_vlr_diario.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                }
                else
                {
                    txtcolesterol_qtde.Attributes.Add("onfocus", "this.select();");
                    txtcolesterol_vlr_diario.Focus();
                    txtcolesterol_qtde.BackColor = System.Drawing.Color.White;
                    txtcolesterol_vlr_diario.BackColor = System.Drawing.Color.White;
                }
            }
            if (chk.ID.Equals("chkcalcio_nao"))
            {
                txtcalcio_qtde.Enabled = !chk.Checked;
                txtcalcio_vlr_diario.Enabled = !chk.Checked;
                if (chk.Checked)
                {
                    txtcalcio_qtde.Text = "";
                    txtcalcio_vlr_diario.Text = "";
                    txtcalcio_qtde.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                    txtcalcio_vlr_diario.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                }
                else
                {
                    txtcalcio_qtde.Attributes.Add("onfocus", "this.select();");
                    txtcalcio_vlr_diario.Focus();
                    txtcalcio_qtde.BackColor = System.Drawing.Color.White;
                    txtcalcio_vlr_diario.BackColor = System.Drawing.Color.White;
                }
            }
            if (chk.ID.Equals("chkferro_nao"))
            {
                txtferro_qtde.Enabled = !chk.Checked;
                txtferro_vlr_diario.Enabled = !chk.Checked;
                if (chk.Checked)
                {
                    txtferro_qtde.Text = "";
                    txtferro_vlr_diario.Text = "";
                    txtferro_qtde.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                    txtferro_vlr_diario.BackColor = System.Drawing.Color.FromArgb(0xDCDCDC);
                }
                else
                {
                    txtferro_qtde.Attributes.Add("onfocus", "this.select();");
                    txtferro_vlr_diario.Focus();
                    txtferro_qtde.BackColor = System.Drawing.Color.White;
                    txtferro_vlr_diario.BackColor = System.Drawing.Color.White;
                }
            }
        }

        protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            if (!TxtCodPLU.Text.Equals(""))
            {
                int tab = TabContainer1.ActiveTabIndex;

                MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];

                if (tab == 6)
                {

                    DataTable TbG01 = new DataTable();
                    DataTable TbG02 = new DataTable();

                    TbG01 = Conexao.GetTable("exec sp_br_Mercadoria_Acum '" + TxtCodPLU.Text.Trim() + "' ,'" + usr.getFilial() + "'", null, false);
                    TbG02 = Conexao.GetTable("exec sp_br_Mercadoria_Acum_Dia '" + TxtCodPLU.Text.Trim() + "' ,'" + usr.getFilial() + "'", null, false);

                    GridVendas.DataSource = TbG01;
                    GridVendas.DataBind();

                    GridVendasDia.DataSource = TbG02;
                    GridVendasDia.DataBind();

                    Grafico01.DataSource = TbG01;
                    Grafico01.DataBind();

                    Grafico02.DataSource = TbG02;
                    Grafico02.DataBind();
                }
                else if (tab == 3)
                {
                    Grid10Dias.DataSource = merc.historicoEstoqueDia;
                    Grid10Dias.DataBind();

                    Gridhistorico.DataSource = merc.historioEntrada();
                    Gridhistorico.DataBind();

                    GridhistoricoSaida.DataSource = merc.historioSaida;
                    GridhistoricoSaida.DataBind();
                }
                else if (tab == 8)
                {
                    if (status == "incluir")
                    {
                        btnUpload.Enabled = false;
                    }
                    else
                    {
                        btnUpload.Enabled = true;
                    }
                }
            }
            if (!status.Equals("visualizar"))
                btnEans.Visible = usr.telaPermissao("Cadastro");
        }

        protected void imgBtnConfirmaErroEan_Click(object sender, ImageClickEventArgs e)
        {
            addEan();
            modalErroEan.Hide();
            modalEan.Show();
        }
        protected void imgBtnCancelaErroEan_Click(object sender, ImageClickEventArgs e)
        {
            modalErroEan.Hide();
            modalEan.Show();
        }

        protected void imgBtnAddImpressora_Click(object sender, EventArgs e)
        {
            try
            {
                atualizarObj();
                MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];


                int exist = 0;
                if (txtLojaImp.Text.Equals(""))
                {
                    txtLojaImp.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Loja não preenchida");
                }
                else
                {
                    int.TryParse(Conexao.retornaUmValor("Select count (*) from filial where loja=" + txtLojaImp.Text, null), out exist);
                    if (exist <= 0)
                    {
                        txtLojaImp.BackColor = System.Drawing.Color.Red;
                        throw new Exception("Loja não Existe");
                    }
                }
                if (txtImpressora.Text.Equals(""))
                {
                    txtImpressora.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Impressora não preenchida");

                }
                else
                {
                    int.TryParse(Conexao.retornaUmValor("Select count (*) from Spool_impressoras where impressora_remota=" + txtImpressora.Text, null), out exist);
                    if (exist <= 0)
                    {
                        txtImpressora.BackColor = System.Drawing.Color.Red;
                        throw new Exception("Impressora não Existe");
                    }
                }


                if (!txtObsImpressora.Text.Equals(""))
                {
                    String obs = txtObsImpressora.Text.Trim();
                    if (!merc.obsExist(obs))
                    {
                        txtObsImpressora.BackColor = System.Drawing.Color.Red;
                        throw new Exception("Observação Não Existe");
                    }


                }

                foreach (ArrayList item in merc.arrImpressoras)
                {
                    if (item[0].ToString().Equals(txtLojaImp.Text) &&
                       item[2].ToString().Equals(txtImpressora.Text) &&
                       item[4].ToString().Equals(txtObsImpressora.Text.Trim()))
                    {
                        throw new Exception("Impressora já foi incluida");
                    }

                }

                ArrayList imp = new ArrayList();
                imp.Add(txtLojaImp.Text);
                imp.Add(txtLojaFilial.Text);
                imp.Add(txtImpressora.Text);
                imp.Add(txtImpressoraDescricao.Text);
                imp.Add(txtObsImpressora.Text);

                merc.arrImpressoras.Add(imp);

                txtLojaImp.Text = "";
                txtLojaFilial.Text = "";
                txtImpressora.Text = "";
                txtImpressoraDescricao.Text = "";
                txtObsImpressora.Text = "";

                Session.Remove("mercadoria" + urlSessao());
                Session.Add("mercadoria" + urlSessao(), merc);

                carregarDados();

            }
            catch (Exception err)
            {
                msgShow(err.Message, true);

            }

        }
        protected void txtLojaImp_TextChange(object sender, EventArgs e)
        {
            if (!txtLojaImp.Text.Trim().Equals(""))
            {
                txtLojaFilial.Text = Conexao.retornaUmValor("Select Filial from filial where loja =" + txtLojaImp.Text + "", null);
                txtImpressora.Attributes.Add("onfocus", "this.select();");
                txtImpressora.Focus();


            }
            else
            {
                txtLojaFilial.Text = "";
            }

        }

        protected void txtImpressora_TextChange(object sender, EventArgs e)
        {

            if (!txtImpressora.Text.Trim().Equals(""))
            {
                txtImpressoraDescricao.Text = Conexao.retornaUmValor("Select Descricao from Spool_impressoras where impressora_remota =" + txtImpressora.Text + "", null);
                txtObsImpressora.Attributes.Add("onfocus", "this.select();");
                txtObsImpressora.Focus();
            }
            else
            {
                txtImpressoraDescricao.Text = "";
            }
        }
        protected void gridImpressora_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
            int index = Convert.ToInt32(e.CommandArgument);
            if (!gridImpressoras.Rows[index].Cells[1].Text.Equals("------"))
            {
                merc.arrImpressoras.RemoveAt(index);
            }

            Session.Remove("mercadoria" + urlSessao());
            Session.Add("mercadoria" + urlSessao(), merc);

            carregarDados();



        }

        protected void imgBtnUltmioFornecedor_Click(object sender, EventArgs e)
        {
            if (!TxtCodPLU.Text.Equals(""))
            {
                User usr = (User)Session["User"];
                String sql = "Select Fornecedor, Data= convert(varchar,max(data),103)   from Fornecedor_Mercadoria where plu ='" + TxtCodPLU.Text + "' group by fornecedor";
                gridDetalhesReferencia.DataSource = Conexao.GetTable(sql, usr, false);
                gridDetalhesReferencia.DataBind();
                modalDetalhesReferencia.Show();

            }
            else
            {
                msgShow("Sem codigo de PLU para referencia.", true);
            }
        }
        protected void imgBtnFecharReferencia_Click(object sender, EventArgs e)
        {
            modalDetalhesReferencia.Hide();
        }

        protected void btnOkError_Click(object sender, EventArgs e)
        {
            modalError.Hide();
        }

        protected void msgShow(String mensagem, bool erro)
        {
            lblErroPanel.Text = mensagem;
            if (erro)
            {
                lblErroPanel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblErroPanel.ForeColor = System.Drawing.Color.Blue;
            }
            btnOkError.Focus();
            modalError.Show();
        }


        protected void imgBtnLimparCentroCusto_Click(object sender, ImageClickEventArgs e)
        {
            txtGrupoCentroCusto.Text = "";
            txtDescricaoGrupoCentroCusto.Text = "";
            txtSubGrupoCentroCusto.Text = "";
            txtDescricaoSubGrupoCentroCusto.Text = "";
            txtCentroCusto.Text = "";
            txtDescricaoCentroCusto.Text = "";

        }

        protected void txtPluReceita_TextChanged(object sender, EventArgs e)
        {
            AtualizarReceita();

        }


        protected void AtualizarReceita()
        {
            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
            if (txtPluReceita.Text.Equals(""))
                txtPluReceita.Text = TxtCodPLU.Text;

            merc.pluReceita = txtPluReceita.Text;
            merc.DescResumidaReceita = "";
            txtPluReceitaDescricao.Text = merc.DescResumidaReceita;

            merc.carregarItens();
            merc.carregarProgramacao();
            chkProgSeg.Checked = merc.progSeg;
            chkProgTer.Checked = merc.progTer;
            chkProgQua.Checked = merc.progQua;
            chkProgQui.Checked = merc.progQui;
            chkProgSex.Checked = merc.progSex;
            chkProgSab.Checked = merc.progSab;
            chkProgDom.Checked = merc.progDom;


            gridItens.DataSource = merc.itensDt();
            gridItens.DataBind();
            merc.receitaPluProducao();
            txtReceita.Text = merc.receita;
            txtCustoTotalReceita.Text = merc.CustoTotalReceita.ToString("N2");
            txtQtdReceita.Text = merc.qtde_receita.ToString("N2");
            //txtFilialProduzido.Text = merc.filial_produzido;
            bool editarReceita = true;
            if (!txtPluReceita.Text.Trim().Equals(TxtCodPLU.Text.Trim()))
            {
                editarReceita = false;
            }
            else
            {
                editarReceita = true;
            }

            EnabledControls(divItensIncluidos, editarReceita);


            EnabledControls(divTotalisReceita, editarReceita);
            EnabledControls(divPluReceita, editarReceita);
            txtPluReceita.Enabled = true;
            txtPluReceita.BackColor = txtDescricao.BackColor;
            ddlTipoProducao.Enabled = true;
            ddlTipoProducao.BackColor = txtDescricao.BackColor;
            txtpeso_receita_unitario.Enabled = true;
            txtpeso_receita_unitario.BackColor = txtPluReceita.BackColor;
            txtReceita.Enabled = editarReceita;
            txtpeso_receita_unitario.BackColor = txtPluReceita.BackColor;



        }

        protected void chkObsObrigatorio_CheckedChanged(object sender, EventArgs e)
        {
            ddlObrigatorioOrdem.Enabled = chkObsObrigatorio.Checked;
            if (!ddlObrigatorioOrdem.Enabled)
                ddlObrigatorioOrdem.Text = "0";
        }

        protected void imgBtnConfirmaExcluirItem_Click(object sender, ImageClickEventArgs e)
        {
            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
            merc.removeItem(lblPluItemExcluir.Text);
            Session.Remove("mercadoria" + urlSessao());
            Session.Add("mercadoria" + urlSessao(), merc);
            gridItens.DataSource = merc.itensDt();
            gridItens.DataBind();
            modalConfirmaExcluirItem.Hide();
        }

        protected void imgBrnCancelaExcluirItem_Click(object sender, ImageClickEventArgs e)
        {
            modalConfirmaExcluirItem.Hide();
        }

        protected void txtCodDepartamentoCE_TextChanged(object sender, EventArgs e)
        {
            if (txtCodDepartamentoCE.Text.Equals(""))
            {
                txtDepartamentoCEDescricao.Text = "";
            }
            else
            {
                MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
                merc.codigo_departamento_ce = txtCodDepartamentoCE.Text;
                txtDepartamentoCEDescricao.Text = merc.descricao_departamento_ce;
                txtReferenciaFornecedor.Focus();
            }

        }

        protected void txtEmbalagemProducao_TextChanged(object sender, EventArgs e)
        {
            Decimal PrecoCompra = Funcoes.decTry(txtPrecoCompraProducao.Text);
            Decimal Emb = Funcoes.decTry(txtEmbalagemProducao.Text);
            if (PrecoCompra > 0 && Emb > 0)
                txtCustoProducao.Text = (PrecoCompra / Emb).ToString("N2");
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (status != "incluir")
            {
                atualizarObj();
                Response.Redirect("MercadoriaUpload.aspx?plu=" + TxtCodPLU.Text);
            }
            else
            {
                msgShow("Salve o produto antes de carregar a imagem.", true);
            }
        }

        protected void txtPLUVinculado_TextChanged(object sender, EventArgs e)
        {
            if (!txtPLUVinculado.Text.Equals(""))
            {
                MercadoriaDAO mercVinculado = new MercadoriaDAO(txtPLUVinculado.Text, null);
                txtDescricaoVinculado.Text = mercVinculado.Descricao;
                txtFatorVinculado.Focus();
            }
            else
            {
                txtDescricaoVinculado.Text = "";
            }

        }

        protected void proximoSKU()
        {
            try
            {

                String codSKU = "";
                User usr = (User)Session["user"];

                bool skuUsado = true;
                while (skuUsado)
                {
                    String sql = "select min(convert(decimal,SKU)) SKU FROM SKU WHERE usado=0 ";
                    if (!txtSKU.Text.Trim().Equals(""))
                    {
                        sql += " and SKU > " + txtSKU.Text;

                    }

                    txtSKU.Text = Conexao.retornaUmValor(sql, null);

                    if (!txtSKU.Text.Trim().Equals(""))
                    {
                        codSKU = txtSKU.Text;
                        String repo = Conexao.retornaUmValor("Select usado from sku where sku ='" + codSKU + "'", usr);
                        skuUsado = repo.Equals("1");
                        if (!skuUsado)
                        {
                            txtSKU.Text = codSKU;
                            skuUsado = false;
                        }
                    }
                    else
                    {
                        skuUsado = false;
                    }

                }

                if (!txtSKU.Text.Equals(""))
                {
                    Conexao.executarSql("update sku set usado=1 where sku=" + txtSKU.Text);
                }

                MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
                merc.SKU = txtSKU.Text;
            }
            catch (Exception err)
            {
                msgShow(err.Message, true);

            }
        }

        protected void chkIntegraWS_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIntegraWS.Checked && txtSKU.Text.Equals(""))
            {
                proximoSKU();
            }
        }


        protected void ImgBtnEnviarAPI_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void txtMargTabPreco_TextChanged(object sender, EventArgs e)
        {

            Decimal margem = Funcoes.decTry(txtMargTabPreco.Text);
            Decimal precoCusto = Funcoes.decTry(txtPrecoTabCusto.Text);
            if (margem != 0 && precoCusto != 0)
            {
                txtPrecoVendaTabPreco.Text = (precoCusto + (precoCusto * margem / 100)).ToString("N2");
            }
            txtPrecoVendaTabPreco.Focus();
            modalTbpreco.Show();
        }

        protected void txtPrecoVendaTabPreco_TextChanged(object sender, EventArgs e)
        {
            Decimal preco = Funcoes.decTry(txtPrecoVendaTabPreco.Text);
            Decimal precoCusto = Funcoes.decTry(txtPrecoTabCusto.Text);

            if (precoCusto != 0 && precoCusto != 0)
            {
                Decimal margem = ((preco - precoCusto) / precoCusto) * 100;
                txtMargTabPreco.Text = margem.ToString("N4");
            }
            txtPrecoVendaTabPreco.Focus();
            modalTbpreco.Show();
        }

        protected void ImgBtnExcluirTbPreco_Click(object sender, ImageClickEventArgs e)
        {
            preco_mercadoriaDAO preco = (preco_mercadoriaDAO)Session["preco_promocional" + urlSessao()];
            MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
            merc.arrPrecosPromocionais.Remove(preco);
            carregarDados();
            modalTbpreco.Hide();
        }

        protected void ImgBtnConfirmaExluirImagem_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //Evitar duplo click
                string dubloClick = (string)Session["dubloClick"];

                if (dubloClick == null)
                {
                    Session.Add("dubloClick", "trava");
                }
                else
                {
                    return;
                }

                int ordem = Funcoes.intTry(lblImagemExcluir.Text);
                MercadoriaDAO merc = (MercadoriaDAO)Session["mercadoria" + urlSessao()];
                string sql = "delete from mercadoria_media where plu ='" + merc.PLU + "' and ordem =" + ordem + ";" +
                             "update mercadoria_media set ordem = ordem-1 where plu ='" + merc.PLU + "' and ordem >" + ordem + ";";
                Conexao.executarSql(sql);
                modalConfirmaExcluirImagem.Hide();
                ExibirImagens();
            }
            catch (Exception err)
            {

                msgShow(err.Message, true);
            }

        }

        protected void imgBtnCancelaExcluirImagem_Click(object sender, ImageClickEventArgs e)
        {
            modalConfirmaExcluirImagem.Hide();
        }

        protected void txtMargemTerceiroPreco_TextChanged(object sender, EventArgs e)
        {
            Decimal vlrCusto = Funcoes.decTry(txtPrecoCusto.Text);
            Decimal marg = Funcoes.decTry(txtMargemTerceiroPreco.Text);
            txtTerceiroPreco.Text = Funcoes.precoMargem(vlrCusto, marg).ToString("N2");
            txtMargemTerceiroPreco.Text = marg.ToString("N4");
        }

        protected void txtTerceiroPreco_TextChanged(object sender, EventArgs e)
        {
            Decimal vlrCusto = Funcoes.decTry(txtPrecoCusto.Text);
            Decimal vlrTerceioPreco = Funcoes.decTry(txtTerceiroPreco.Text);
            txtMargemTerceiroPreco.Text = Funcoes.valorMargem(vlrCusto, vlrTerceioPreco).ToString("N4");
            txtTerceiroPreco.Text = vlrTerceioPreco.ToString("N2");
        }

        protected mercadoria_AtributosMagentoDAO atualizarObjMagentoAtributos(mercadoria_AtributosMagentoDAO attributtes)
        {
            mercadoria_AtributosMagentoDAO magentoAttributes = attributtes;
            try
            {
                magentoAttributes.tipoProduto = int.Parse(dllTipoProduto.SelectedValue);
                magentoAttributes.marca = int.Parse(ddlMarca.SelectedValue);

                magentoAttributes.especie = lerChkList(lcAnimal);
                magentoAttributes.racas = lerChkList(lckRaca);
                magentoAttributes.porte = lerChkList(lckPorte);
                magentoAttributes.idade = lerChkList(lckIdade);
                magentoAttributes.sabor = lerChkList(lckSabor);
                magentoAttributes.cuidados = lerChkList(lckCuidados);
                magentoAttributes.tipoPetiscos = lerChkList(lckTipoPetisco);
                magentoAttributes.odor = lerChkList(lckOdor);
                magentoAttributes.tipoAreia = lerChkList(lckAreia);
                magentoAttributes.tipoHigienico = lerChkList(lckTipoHigienico);
                magentoAttributes.farmacos = lerChkList(lckTipoFarmaceutico);
                magentoAttributes.tipoAcessorios = lerChkList(lckTipoAcessorio);
                magentoAttributes.cor = lerChkList(lckCor);
                magentoAttributes.qtdeUnidade = txtQtdeUnidadesProduto.Text;
                magentoAttributes.pesoGramas = Funcoes.decTry(txtPesoGramas.Text);
                magentoAttributes.dosagemRecomendada = txtDosagemRecomendada.Text;

                magentoAttributes.outlet = int.Parse(ddlOutlet.SelectedValue);

                return magentoAttributes;
            }
            catch
            {
                return null;
            }
        }

        protected void atualizarMagentoCheckList(mercadoria_AtributosMagentoDAO attributtes)
        {
            try
            {
                dllTipoProduto.SelectedValue = attributtes.tipoProduto.ToString();
                ddlMarca.SelectedValue = attributtes.marca.ToString();

                atualizarCheckBoxList(lcAnimal, attributtes.especie  );
                atualizarCheckBoxList(lckRaca, attributtes.racas );
                atualizarCheckBoxList(lckPorte, attributtes.porte  );
                atualizarCheckBoxList(lckIdade, attributtes.idade  );
                atualizarCheckBoxList(lckSabor, attributtes.sabor  );
                atualizarCheckBoxList(lckCuidados, attributtes.cuidados  );
                atualizarCheckBoxList(lckTipoPetisco, attributtes.tipoPetiscos  );
                atualizarCheckBoxList(lckOdor, attributtes.odor  );
                atualizarCheckBoxList(lckAreia, attributtes.tipoAreia  );
                atualizarCheckBoxList(lckTipoHigienico, attributtes.tipoHigienico  );
                atualizarCheckBoxList(lckTipoFarmaceutico, attributtes.farmacos  );
                atualizarCheckBoxList(lckTipoAcessorio, attributtes.tipoAcessorios  );
                atualizarCheckBoxList(lckCor, attributtes.cor  );

                txtQtdeUnidadesProduto.Text = attributtes.qtdeUnidade;
                txtPesoGramas.Text = attributtes.pesoGramas.ToString("N3");
                txtDosagemRecomendada.Text = attributtes.dosagemRecomendada;

                ddlOutlet.SelectedValue = attributtes.outlet.ToString();
            }
            catch
            {
            }

        }

        //Atualizar dados do obj com conteudos dos chkboxlist
        protected string lerChkList(CheckBoxList listaCHK)
        {
            try
            {
                string chkRetorno = "";
                foreach (ListItem item in listaCHK.Items)
                {
                    if (item.Selected)
                    {
                        chkRetorno += item.Value + ",";
                    }
                }
                
                if (chkRetorno.Length > 1)
                {
                    string trataRetorno = chkRetorno.Substring(0, chkRetorno.Length - 1);
                    chkRetorno = trataRetorno;
                }

                return chkRetorno;
            }
            catch
            {
                return "";
            }
        }
        //Atualizar dados do obj com conteudos dos chkboxlist
        protected void atualizarCheckBoxList(CheckBoxList listaCHK, string valor)
        {
            try
            {
                //Zerar objeto
                foreach (ListItem item in listaCHK.Items)
                {
                    item.Selected = false;
                }

                var arrValor = valor.Split(',');
                //Atribuir selecionado ou não
                foreach (string arr in arrValor)
                {
                    foreach (ListItem item in listaCHK.Items)
                    {
                        if (item.Value == arr)
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        protected void btnEnviarFTP_Click(object sender, EventArgs e)
        {
            
            String dadosConexao = Funcoes.valorParametro("CARDAPIO_CONEC_FTP", null);

            try
            {
                if (!dadosConexao.Equals(""))
                {
                    if (Funcoes.intTry(dadosConexao.Substring(0, 1)) <= 0)
                    {
                        throw new Exception("Dados de conexão com o servidor são inválidos.");
                    }
                }
            }
            catch (Exception er)
            {
                throw er;
            }

            var arrDadosConexao = dadosConexao.Split('|');

            string base64 = Conexao.retornaUmValor("SELECT TOP 1 base FROM mercadoria_media WHERE plu='" + TxtCodPLU.Text + "' ORDER BY Ordem", null);
            string tipo = Conexao.retornaUmValor("SELECT tipo FROM mercadoria_media WHERE plu='" + TxtCodPLU.Text + "' ORDER BY Ordem", null);
            string pathImagens = Server.MapPath("~/modulos/Cadastro/imgs/uploads/") + TxtCodPLU.Text.Trim() + ".jpg";
            byte[] imageBytes = Convert.FromBase64String(base64);
            File.WriteAllBytes(pathImagens, imageBytes);

            if (File.Exists(pathImagens))
            {
                FTPEnviarArquivo ftp = new FTPEnviarArquivo();
                ftp.localFilePath = @pathImagens;
                ftp.ftpServer = arrDadosConexao[0];

                ftp.username = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(arrDadosConexao[3])); //    "bratter";
                ftp.password = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(arrDadosConexao[4])); //"L6RbepbfVbT%TynUWf3P@";
                ftp.remotePath = arrDadosConexao[2]; //@"/html/villagrano/appM/static/img/";
                ftp.porta = int.Parse(arrDadosConexao[1]);
                //ftp.ftpServer = "ftp://186.209.225.57";
                //ftp.username = "jgasolucoes";
                //ftp.password = "";
                //ftp.remotePath = @"/www";

                //if (ftp.EnviarArquivoFluentFTP() != "200")
                //if (ftp.EnviarArquivoWEBClient() != "200")
                //if (ftp.EnviarArquivo() != "200")
                //{

                //}
                ftp.EnviarArquivoWinSCP(); // != "200")
            }

        }


    }
}