using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class MercadoriaDAO
    {
        public User usr = null;
        public DataTable ean { get; set; }
        public String eanPrimeiro
        {
            get
            {
                if (ean != null)
                {

                    foreach (DataRow item in ean.Rows)
                    {
                        return item[0].ToString();

                    }
                }
                return "";
            }
        }
        public String Categoria { get; set; }
        public String CategoriaDesc { get; set; }
        public String Seguimento { get; set; }
        public String SeguimentoDesc { get; set; }
        public String SubSeguimento { get; set; }
        public String SubSeguimentoDesc { get; set; }
        public String GrupoCategoria { get; set; }
        public String GrupoCategoriaDesc { get; set; }
        public String SubGrupoCategoria { get; set; }
        public String SubGrupoCategoriaDesc { get; set; }
        public String Filial { get; set; }
        public String PLU = "";
        public String Tipo = "";
        public String Peso_Variavel = "";
        public String Codigo_Portaria = "";
        public int bandeja { get; set; }

        public tributacaoDAO tributacaoSaida = null;

        public Decimal qtde_atacado = 0;
        public Decimal margem_atacado = 0;
        public Decimal preco_atacado = 0;
        public Decimal margem_terceiro_preco { get; set; } = 0;
        public Decimal terceiro_preco { get; set; } = 0;
        public String strEcommerce = "<H1>SEM DESCRIÇÃO</H1>";
        public String informacoes_extras = "";
        public string urlImage { get; set; } = "";

        public Decimal Codigo_Tributacao
        {
            get
            {
                if (tributacaoSaida == null)
                {
                    return 0;
                }
                else if (tributacaoSaida.Codigo_Tributacao == 0)
                {
                    return 0;
                }
                else
                {
                    return tributacaoSaida.Codigo_Tributacao;
                }

            }
            set
            {
                tributacaoSaida = new tributacaoDAO(value.ToString(), new User());
            }
        }
        public String descricaoTributacao
        {
            get
            {
                if (tributacaoSaida == null)
                {
                    return "";
                }
                else if (tributacaoSaida.Codigo_Tributacao == 0)
                {
                    return "";
                }
                else
                {
                    return tributacaoSaida.Descricao_Tributacao;
                }

            }
            set
            {

            }
        }

        public tributacaoDAO tributacaoEntrada = null;
        public Decimal Codigo_Tributacao_ent
        {
            get
            {
                if (tributacaoEntrada == null)
                {
                    return 0;
                }
                else if (tributacaoEntrada.Codigo_Tributacao == 0)
                {
                    return 0;
                }
                else
                {
                    return tributacaoEntrada.Codigo_Tributacao;
                }

            }
            set
            {
                tributacaoEntrada = new tributacaoDAO(value.ToString(), new User());
            }
        }
        public String descricaoTributacaoEnt
        {
            get
            {
                if (tributacaoEntrada == null)
                {
                    return "";
                }
                else if (tributacaoEntrada.Codigo_Tributacao == 0)
                {
                    return "";
                }
                else
                {
                    return tributacaoEntrada.Descricao_Tributacao;
                }

            }
            set
            {

            }
        }

        private DepartamentoDAO departamento;
        public String Codigo_departamento
        {
            get
            {
                if (departamento == null || departamento.Codigo_departamento == null)
                    return "";
                else
                    return departamento.Codigo_departamento;
            }
            set
            {
                try
                {
                    if (value.Equals(""))
                    {
                        departamento = null;
                    }
                    else
                    {
                        departamento = new DepartamentoDAO(value);
                    }
                }
                catch (Exception)
                {

                    throw new Exception("Codigo de Departamento invalido");
                }


            }
        }

        public String Descricao_departamento
        {
            get
            {
                if (departamento == null || departamento.Descricao_departamento == null)
                    return "";
                else
                    return departamento.Descricao_departamento;
            }
            set { }
        }
        public String Codigo_familia = "";
        private SubGrupoDAO subGrupo;
        public String codigo_subGrupo
        {
            get
            {
                if (subGrupo == null)
                    return "";
                else
                    return subGrupo.Codigo_SubGrupo;
            }
            set
            {
                try
                {
                    if (!value.Equals(""))
                        subGrupo = new SubGrupoDAO(value);
                }
                catch (Exception)
                {

                    throw new Exception("Codigo de SubGrupo invalido");
                }


            }

        }
        public String descricao_subGrupo
        {
            get
            {
                if (subGrupo == null)
                    return "";
                else
                    return subGrupo.Descricao_SubGrupo;
            }
            set { }
        }

        private grupoDAO grupo;
        public String codigo_Grupo
        {
            get
            {
                if (grupo == null)
                    return "";
                else
                    return grupo.Codigo_Grupo;
            }
            set
            {
                try
                {
                    if (!value.Equals(""))
                        grupo = new grupoDAO(value);
                }
                catch (Exception)
                {

                    throw new Exception("Codigo de Grupo invalido");
                }


            }

        }
        public String descricao_Grupo
        {
            get
            {
                if (grupo == null)
                    return "";
                else
                    return grupo.Descricao_Grupo;
            }
            set { }
        }

        public int ativa_ce { get; set; }
        private DepartamentoDAO departamento_ce;
        public string codigo_departamento_ce
        {
            get
            {
                if (departamento_ce == null)
                    return "";
                else
                    return departamento_ce.Codigo_departamento;
            }
            set
            {
                if (!value.Equals(""))
                    departamento_ce = new DepartamentoDAO(value);
                else
                    departamento_ce = null;
            }
        }
        public string descricao_departamento_ce
        {
            get
            {
                if (departamento_ce != null)
                    return departamento_ce.Descricao_departamento;
                else
                    return "";
            }
            set { }
        }
        public String Descricao = "";
        public String Descricao_resumida = "";
        public String Descricao_Comercial = "";

        public String Descricao_familia = "";
        public Decimal Tecla = 0;
        public Decimal Margem = 0;
        public Decimal Estoque_minimo = 0;
        public Decimal Etiqueta = 0;
        public Decimal Validade = 0;
        public Decimal Preco = 0;
        public Decimal Preco_promocao = 0;
        public DateTime data_inicio = new DateTime();

        public DateTime data_fim = new DateTime();

        public bool Promocao_automatica = false;
        public bool Promocao = false;
        public Decimal Preco_Custo = 0;
        public DateTime Data_Cadastro = new DateTime();
        public String Data_CadastroBr()
        {
            return Data_Cadastro.ToString("dd/MM/yyyy");
        }
        public DateTime Data_Alteracao = new DateTime();
        public String Data_AlteracaoBr()
        {
            return Data_Alteracao.ToString("dd/MM/yyyy");
        }
        public Decimal IPI = 0;
        public bool Incide_Pis = false;
        public Decimal Embalagem = 0;
        public String ultimo_fornecedor = "";
        public Decimal Fator_conversao = 0;
        public Decimal Tecla_balanca = 0;
        public String Localizacao = "";
        public int Inativo = 0;
        public bool Imprime_etiqueta { get; set; }
        public bool Estado_Mercadoria { get; set; }
        public Decimal saldo_atual { get; set; }
        public Decimal Preco_Custo_1 { get; set; }
        public Decimal Preco_Custo_2 { get; set; }
        public String Ref_fornecedor = "";
        public DateTime data_inventario { get; set; }
        public String data_inventarioBr()
        {
            return data_inventario.ToString("dd/MM/yyyy");
        }
        public Decimal saldo_inicial { get; set; }
        public Decimal peso { get; set; }
        private String _receita = "";
        public String receita
        {
            get
            {
                if (_receita.Equals("") && !pluReceita.Equals(PLU))
                    receitaPluProducao();


                return _receita;
            }
            set
            {
                _receita = value;
            }
        }


        public void receitaPluProducao()
        {
            SqlDataReader rs = null;

            try
            {
                rs = Conexao.consulta("Select qtde_receita, receita,filial_produzido from mercadoria where plu ='" + pluReceita + "'", null, false);
                if (rs.Read())
                {
                    _receita = rs["receita"].ToString();
                    qtde_receita = Funcoes.decTry(rs["qtde_receita"].ToString());
                    filial_produzido = rs["filial_produzido"].ToString();
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
        public Decimal preco_compra { get; set; }
        public Decimal frete { get; set; }
        public Decimal seguro { get; set; }
        public Decimal outras_despesas { get; set; }
        public Decimal valor_ipi { get; set; }
        private String _codigo_centro_custo = "";
        public String codigo_centro_custo
        {
            get
            {
                return _codigo_centro_custo;
            }
            set
            {
                _codigo_centro_custo = value;
                String sql = " Select c.descricao_centro_custo " +
                                     ",sg.codigo_subgrupo" +
                                     ",sg.descricao_subgrupo" +
                                     ",g.codigo_grupo" +
                                     ",g.descricao_grupo" +
                               " from centro_custo as c inner join subgrupo_cc as sg on c.codigo_subgrupo = sg.codigo_subgrupo " +
                               "     inner join grupo_cc as g on sg.codigo_grupo = g.codigo_grupo " +
                               " where c.codigo_centro_custo = '" + _codigo_centro_custo + "'";
                SqlDataReader rs = null;

                try
                {
                    rs = Conexao.consulta(sql, null, false);
                    if (rs.Read())
                    {
                        descricao_centro_custo = rs["descricao_centro_custo"].ToString();
                        codigo_subgrupo_cc = rs["codigo_subgrupo"].ToString();
                        descricao_subgrupo_cc = rs["descricao_subgrupo"].ToString();
                        codigo_grupo_cc = rs["codigo_grupo"].ToString();
                        descricao_grupo_cc = rs["descricao_grupo"].ToString();
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
        }
        public String descricao_centro_custo { get; set; } = "";
        public String codigo_grupo_cc { get; set; } = "";
        public String descricao_grupo_cc { get; set; } = "";
        public String codigo_subgrupo_cc { get; set; } = "";
        public String descricao_subgrupo_cc { get; set; } = "";

        public String venda_fracionaria { get; set; } = "";
        public Decimal pis { get; set; }
        public Decimal cofins { get; set; }
        public String cf { get; set; }
        public int CEST = 0;
        public String und { get; set; } = "";
        public bool gera_inativo { get; set; }
        public String inventario { get; set; }
        public String tipo_produto_origem { get; set; } = "";
        public String tipo_produto_destino { get; set; } = "";
        public bool curva_a { get; set; }
        public bool curva_b { get; set; }
        public bool curva_c { get; set; }
        public bool estoque_aviso { get; set; }
        public String artigo { get; set; } = "";
        public int estoque_margem { get; set; }
        public int estoque_meses { get; set; }
        public int cobertura { get; set; }
        public DateTime sazonal1 { get; set; }
        public String Und_producao { get; set; } = "";

        public List<preco_mercadoriaDAO> arrPrecosPromocionais = new List<preco_mercadoriaDAO>();

        public ArrayList arrItens = new ArrayList();
        private ArrayList arrItensExcluidos = new ArrayList();
        private ArrayList arrItensAdd = new ArrayList();

        public itemDAO pegarItem(int index)
        {
            itemDAO item = (itemDAO)arrItens[index];
            return item;
        }
        public String pluReceita { get; set; }
        private String _descResumidaReceita = "";
        public String DescResumidaReceita
        {
            get
            {
                if (_descResumidaReceita.Equals(""))
                    _descResumidaReceita = Conexao.retornaUmValor("Select Descricao_resumida from mercadoria where plu ='" + pluReceita + "'", null);

                return _descResumidaReceita;
            }
            set
            {
                _descResumidaReceita = value;
            }
        }
        public bool progSeg = false;
        public bool progTer = false;
        public bool progQua = false;
        public bool progQui = false;
        public bool progSex = false;
        public bool progSab = false;
        public bool progDom = false;
        public Decimal qtde_receita { get; set; }

        public Decimal CustoTotalReceita
        {
            get
            {
                Decimal totalCusto = 0;
                foreach (itemDAO item in arrItens)
                {
                    totalCusto += item.Custo_Unitario;
                }
                return totalCusto;
            }
        }

        public Decimal custo_producao
        {
            get
            {
                if (preco_compra > 0 && embalagem_producao > 0)
                    return preco_compra / embalagem_producao;
                else
                    return 0;
            }
        }
        public Decimal peso_receita_unitario { get; set; }

        public String filial_produzido { get; set; }
        public String tipo_producao { get; set; }


        public ArrayList arrImpressoras = new ArrayList();


        public Decimal VlrPisCofins { get; set; }

        public Decimal valorSt { get; set; }
        public Decimal valorIcms { get; set; }
        public Decimal valorPisCofins { get; set; }
        public Decimal valorIpi { get; set; }
        public Decimal valorLucroReal { get; set; }
        public bool Prato_dia = false;
        public bool Prato_dia_1 = false;
        public bool Prato_dia_2 = false;
        public bool Prato_dia_3 = false;
        public bool Prato_dia_4 = false;
        public bool Prato_dia_5 = false;
        public bool Prato_dia_6 = false;
        public bool Prato_dia_7 = false;

        public String sazonal1Br()
        {
            return sazonal1.ToString("dd/MM/yyyy");
        }
        public DateTime sazonal2 { get; set; }
        public String sazonal2Br()
        {
            return sazonal2.ToString("dd/MM/yyyy");
        }
        public DateTime sazonal3 { get; set; }
        public String sazonal3Br()
        {
            return sazonal3.ToString("dd/MM/yyyy");
        }
        public DateTime sazonal4 { get; set; }
        public String sazonal4Br()
        {
            return sazonal4.ToString("dd/MM/yyyy");
        }
        public DateTime sazonal5 { get; set; }
        public String sazonal5Br()
        {
            return sazonal4.ToString("dd/MM/yyyy");
        }
        public DateTime sazonal6 { get; set; }
        public String sazonal6Br()
        {
            return sazonal6.ToString("dd/MM/yyyy");
        }
        public Decimal peso_liquido { get; set; }
        public Decimal peso_bruto { get; set; }
        public Decimal margem_iva { get; set; }
        public String Marca { get; set; }

        public String mercadoria { get; set; }
        public String cst_entrada { get; set; }
        public String cst_saida { get; set; }

        public linhaDAO linha = null;
        public int linhaCodigo
        {
            get
            {
                if (linha == null)
                {
                    return 0;
                }
                else
                {
                    return linha.codigo_linha;
                }
            }
            set
            {
                linha = new linhaDAO(value.ToString(), new User());
            }

        }
        public String linhaDescricao
        {
            get
            {
                if (linha == null)
                {
                    return "";
                }
                else
                {
                    return linha.descricao_linha;
                }
            }
            set
            {

            }

        }

        public cor_linhaDAO linhaCor = null;
        public int linhaCorCodigo
        {
            get
            {
                if (linhaCor == null)
                {
                    return 0;
                }
                else
                {
                    return linhaCor.codigo_cor;
                }
            }
            set
            {
                linhaCor = new cor_linhaDAO(value.ToString(), linhaCodigo.ToString(), new User());
            }
        }

        public String linhaCorDescricao
        {

            get
            {
                if (linhaCor == null)
                {
                    return "";
                }
                else
                {
                    return linhaCor.descricao_cor;
                }
            }
            set
            {
            }
        }

        public string CFOP { get; internal set; }
        public string und_compra { get; internal set; }
        public string descricao_producao { get; internal set; }
        public int embalagem_producao { get; internal set; }
        public string Agrupamento_producao { get; internal set; }
        public int pontos_fidelizacao { get; internal set; }
        public bool Excluir_proxima_integracao { get; internal set; }

        public Decimal precoReferencia = 0;

        public String erroCarregar = "";

        public ArrayList mercadoriasLoja = new ArrayList();


        public ArrayList arrMercadoriaObs = new ArrayList();



        public Decimal pauta = 0;
        public String numero_excecao = "";
        public String codigo_natureza_receita = "";
        public Decimal pis_perc_entrada = 0;
        public Decimal pis_perc_saida = 0;
        public Decimal cofins_perc_entrada = 0;
        public Decimal cofins_perc_saida = 0;
        public int modalidade_BCICMSST = -1;
        public DataTable historioEntrada()
        {

            String sqlHistorico = "SELECT 	nf.Filial, " +
                                    " Data = CONVERT(VARCHAR(10),NF.Data,103), " +
                                    " Fornecedor = NF.Cliente_Fornecedor, " +
                                    " Qtde = (NF_Item.Qtde * NF_Item.Embalagem), " +
                                    " Preco = CONVERT(DECIMAL(10,2),NF_Item.Unitario), " +
                                    " IPI = Convert(Decimal(10,2),NF_Item.IPI), IVA = Convert(Decimal(10,2),ISNULL((NF_Item.IVA / NF_Item.Qtde),0)), " +
                                    " Documento = RTRIM(LTRIM(NF.Codigo)), " +
                                    " CFOP = Case When NF_Item.Codigo_Operacao IS NULL Then NF.Codigo_Operacao Else NF_Item.Codigo_Operacao End , " +
                                    " Natureza = RTrim(Ltrim(Natureza_Operacao.Descricao)), " +
                                    " nf.Usuario " +
                                    ",nf.serie" +
                                    " FROM NF INNER JOIN NF_Item ON NF.Filial = NF_Item.Filial AND NF.Cliente_Fornecedor = NF_Item.Cliente_Fornecedor " +
                                    " AND NF.Codigo = NF_Item.Codigo AND nf.serie = NF_Item.serie AND  nf.tipo_nf = NF_Item.Tipo_NF " +
                                    " Left Outer Join Natureza_Operacao on  Natureza_Operacao.Filial = NF.Filial AND Natureza_Operacao.Codigo_Operacao = NF.Codigo_Operacao " +
                                    " WHERE NF.Tipo_NF = '2' AND PLU = '" + PLU + "' " +
                                    " ORDER BY convert(varchar,NF.Data,102) Desc";
            return Conexao.GetTable(sqlHistorico, null, true);

        }

        public DataTable historioSaida //Incluso em 23/07/2025 - Jailson
        {
            get
            {
                String sqlHistorico = "SELECT 	nf.Filial, " +
                                        " Emissao = CONVERT(VARCHAR(10),NF.EMISSAO,103), " +
                                        " Cliente = NF.Cliente_Fornecedor, " +
                                        " Qtde = (NF_Item.Qtde * NF_Item.Embalagem), " +
                                        " Preco = CONVERT(DECIMAL(10,2),NF_Item.Unitario), " +
                                        " IPI = Convert(Decimal(10,2),NF_Item.IPI), IVA = Convert(Decimal(10,2),ISNULL((NF_Item.IVA / NF_Item.Qtde),0)), " +
                                        " Documento = RTRIM(LTRIM(NF.Codigo)), " +
                                        " CFOP = Case When NF_Item.Codigo_Operacao IS NULL Then NF.Codigo_Operacao Else NF_Item.Codigo_Operacao End , " +
                                        " Natureza = RTrim(Ltrim(Natureza_Operacao.Descricao)), " +
                                        " nf.Usuario " +
                                        ",nf.serie" +
                                        ", Estoque = CASE WHEN Natureza_Operacao.Baixa_Estoque = 1 THEN 'SIM' ELSE 'NAO' END" +
                                        " FROM NF WITH (INDEX=IX_NF_01) INNER JOIN NF_Item WITH (INDEX=IX_NF_ITEM_02) ON NF.Filial = NF_Item.Filial AND NF.Cliente_Fornecedor = NF_Item.Cliente_Fornecedor " +
                                        " AND NF.Codigo = NF_Item.Codigo  AND nf.serie = NF_Item.serie AND  nf.tipo_nf = NF_Item.Tipo_NF " +
                                        " Left Outer Join Natureza_Operacao on  Natureza_Operacao.Filial = NF.Filial AND Natureza_Operacao.Codigo_Operacao =  NF.Codigo_Operacao " +
                                        " WHERE NF.Tipo_NF = '1' AND PLU = '" + PLU + "' " +
                                        " AND NF.Status IN('AUTORIZADO', 'AUTORIZADA')" +
                                        " ORDER BY convert(varchar,NF.EMISSAO,102) Desc";
                return Conexao.GetTable(sqlHistorico, null, true);


            }
        }
        public DataTable historicoEstoqueDia
        {
            get
            {
                String sqlHistorico = "SELECT TOP 10 CONVERT(VARCHAR, Data, 103) As Data, Qtde_Inicial, Entrada_NFe, Entrada_Outras, Saida_NFe, Saida_Outras, Saida_Cupom, Saldo" +
                                        " FROM Mercadoria_Estoque_Dia " +
                                        " WHERE Mercadoria_Estoque_Dia.PLU = '" + PLU + "'" +
                                        " ORDER BY Mercadoria_Estoque_Dia.DATA DESC ";
                return Conexao.GetTable(sqlHistorico, null, true);
            }
        }

        public int origem = 0;
        public bool alcoolico = false;
        public String usuario = "";

        public Decimal porcao = 0;
        public String porcao_medida = "";
        public Decimal porcao_numero = 0;
        public String porcao_div = "";
        public String porcao_detalhe = "";

        public bool vlr_energ_nao = false;
        public Decimal vlr_energ_qtde = 0;
        public Decimal vlr_energ_qtde_igual = 0;
        public Decimal vlr_energ_diario = 0;

        public bool carboidratos_nao = false;
        public Decimal carboidratos_qtde = 0;
        public Decimal carboidratos_vlr_diario = 0;

        public bool proteinas_nao = false;
        public Decimal proteinas_qtde = 0;
        public Decimal proteinas_vlr_diario = 0;

        public bool gorduras_totais_nao = false;
        public Decimal gorduras_totais_qtde = 0;
        public Decimal gorduras_totais_vlr_diario = 0;

        public bool gorduras_satu_nao = false;
        public Decimal gorduras_satu_qtde = 0;
        public Decimal gorduras_satu_vlr_diario = 0;

        public bool gorduras_trans_nao = false;
        public Decimal gorduras_trans_qtde = 0;

        public bool fibra_alimen_nao = false;
        public Decimal fibra_alimen_qtde = 0;
        public Decimal fibra_alimen_vlr_diario = 0;

        public bool sodio_nao = false;
        public Decimal sodio_qtde = 0;
        public Decimal sodio_vlr_diario = 0;

        //01/10/2024 - Novas colunas
        public bool colesterol_nao = false;
        public Decimal colesterol_qtde = 0;
        public Decimal colesterol_vlr_diario = 0;
        public bool calcio_nao = false;
        public Decimal calcio_qtde = 0;
        public Decimal calcio_vlr_diario = 0;
        public bool ferro_nao = false;
        public Decimal ferro_qtde = 0;
        public Decimal ferro_vlr_diario = 0;
        //01/10/2024 - Fim


        public bool IntegraWS = false;
        public bool Ativo_Ecommerce = false;


        //nfe 4.0
        public bool indEscala = false; // indicador de Escala Relevante 
        public String cnpjFabricante = "";
        public String cBenef = "";


        public string id_Ecommercer = "";
        public string Categoria_eCommerce = "";
        public decimal altura = 0;
        public decimal largura = 0;
        public decimal profundidade = 0;
        public string descricaoWEB = "";
        public string SKU = "";

        //Determina impressão de todos o cupom caso o produto selecionado seja vendido
        public bool impAux = false;
        //Solicita senha de autorização para venda do produto
        public bool vendaComSenha = false;

        public string PLU_Vinculado = "";
        public decimal fatorEstoqueVinculado = 0;

        internal string usuarioAlteracao;

        public bool Configuravel = false; //Utilizado para integração Magento/eCommerce. Este marcador tem como finalidade definir se o produto ao ser enviado ao eCommerce poderá ser configurável na plataforma.

        public string codigo_produto_ANVISA = "";
        public string motivo_isencao_ANVISA = "";
        public Decimal preco_maximo_ANVISA = 0;

        public string codigoEmissaoNFe = "";
        public mercadoria_AtributosMagentoDAO magentoAtributos = null;


        public MercadoriaDAO(String plu, User usr)
        {
            this.usr = usr;
            String sql = "Select c.ean,b.codigo_subgrupo, b.descricao_subgrupo,b.codigo_grupo ,b.descricao_grupo, a.* " +
                                              ",Cat.descricao as Categoria_Descricao " +
                                              ",Seg.descricao as Seguimento_Descricao " +
                                              ",SubSeg.descricao as SubSeguimento_Descricao " +
                                              ",GCat.descricao as GrupoCategoria_Descricao " +
                                              ",SGCat.descricao as SubGrupoCategoria_Descricao " +

                          " from  Mercadoria a left join W_BR_CADASTRO_DEPARTAMENTO b on (a.codigo_departamento= b.codigo_Departamento and a.filial=b.filial)" +
                                             " left join ean c on (a.plu = c.plu and a.filial= c.filial	)" +
                                             " left join categorias as  Cat on a.Categoria = cat.codigo " +
                                             " left join categorias as Seg on a.Seguimento = Seg.codigo " +
                                             " left join categorias as SubSeg on a.SubSeguimento = SubSeg.codigo " +
                                             " left join categorias as GCat on a.GrupoCategoria = GCat.codigo " +
                                             " left join categorias as SGCat on a.SubGrupoCategoria = SGCat.codigo" +
                           " where a.plu ='" + plu + "'";
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            carregarDados(rs);
        }

        public MercadoriaDAO(User usr)
        {
            this.usr = usr;
        }

        private void carregarPrecoLoja()
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                carregarPrecoLoja(conn, tran);
                tran.Commit();
            }
            catch (Exception err)
            {
                tran.Rollback();
                throw err;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        private void carregarPrecoLoja(SqlConnection conn, SqlTransaction tran)
        {
            String sqlfiliais = "select filial from filial";
            String sqlMercFilias = "select * from mercadoria_loja where plu='" + PLU + "'";
            int contFiliais = Conexao.countSql(sqlfiliais, new User());
            int contMerc = Conexao.countSql(sqlMercFilias, new User());

            if (contFiliais == contMerc)
            {
                mercadoriasLoja.Clear();
                SqlDataReader rsMercs = null;
                try
                {


                    rsMercs = Conexao.consulta(sqlMercFilias, new User(), false);
                    while (rsMercs.Read())
                    {

                        mercadoria_lojaDAO mercLoja = new mercadoria_lojaDAO(this.usr);
                        mercLoja.PLU = this.PLU;
                        mercLoja.Filial = rsMercs["filial"].ToString();
                        mercLoja.Tipo = rsMercs["Tipo"].ToString();
                        mercLoja.Saldo_Atual = (Decimal)(rsMercs["Saldo_Atual"].ToString().Equals("") ? new Decimal() : rsMercs["Saldo_Atual"]);
                        mercLoja.Preco_Compra = (Decimal)(rsMercs["Preco_Compra"].ToString().Equals("") ? new Decimal() : rsMercs["Preco_Compra"]);
                        mercLoja.Preco_Custo = (Decimal)(rsMercs["Preco_Custo"].ToString().Equals("") ? new Decimal() : rsMercs["Preco_Custo"]);
                        mercLoja.Margem = (Decimal)(rsMercs["Margem"].ToString().Equals("") ? new Decimal() : rsMercs["Margem"]);
                        mercLoja.Preco = (Decimal)(rsMercs["Preco"].ToString().Equals("") ? new Decimal() : rsMercs["Preco"]);
                        mercLoja.Preco_Promocao = (Decimal)(rsMercs["Preco_Promocao"].ToString().Equals("") ? new Decimal() : rsMercs["Preco_Promocao"]);
                        mercLoja.Margem_Promocao = (Decimal)(rsMercs["Margem_Promocao"].ToString().Equals("") ? new Decimal() : rsMercs["Margem_Promocao"]);
                        mercLoja.Data_Inicio = (rsMercs["Data_Inicio"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rsMercs["Data_Inicio"].ToString()));
                        mercLoja.Data_Fim = (rsMercs["Data_Fim"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rsMercs["Data_Fim"].ToString()));
                        mercLoja.Preco_Custo_1 = (Decimal)(rsMercs["Preco_Custo_1"].ToString().Equals("") ? new Decimal() : rsMercs["Preco_Custo_1"]);
                        mercLoja.Preco_Custo_2 = (Decimal)(rsMercs["Preco_Custo_2"].ToString().Equals("") ? new Decimal() : rsMercs["Preco_Custo_2"]);
                        mercLoja.Estoque_Minimo = (Decimal)(rsMercs["Estoque_Minimo"].ToString().Equals("") ? new Decimal() : Decimal.Parse(rsMercs["Estoque_Minimo"].ToString()));
                        mercLoja.Cobertura = (rsMercs["Cobertura"].ToString().Equals("") ? 0 : int.Parse(rsMercs["Cobertura"].ToString()));
                        mercLoja.Ultima_Entrada = (rsMercs["Ultima_Entrada"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rsMercs["Ultima_Entrada"].ToString()));
                        mercLoja.Data_Inventario = (rsMercs["Data_Inventario"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rsMercs["Data_Inventario"].ToString()));
                        mercLoja.Promocao = (rsMercs["Promocao"].ToString().Equals("1") ? true : false);
                        mercLoja.Promocao_automatica = (rsMercs["Promocao_automatica"].ToString().Equals("1") ? true : false);
                        mercLoja.descricao = rsMercs["descricao"].ToString();
                        mercLoja.sugerido = (Decimal)(rsMercs["sugerido"].ToString().Equals("") ? new Decimal() : rsMercs["sugerido"]);
                        mercLoja.descricao_resumida = rsMercs["descricao_resumida"].ToString();
                        mercLoja.ingredientes = rsMercs["ingredientes"].ToString();
                        mercLoja.marca = rsMercs["marca"].ToString();
                        mercLoja.validade = (Decimal)(rsMercs["validade"].ToString().Equals("") ? new Decimal() : rsMercs["validade"]);
                        mercLoja.codigo_familia = Codigo_familia;

                        Decimal.TryParse(rsMercs["qtde_atacado"].ToString(), out mercLoja.qtde_atacado); // a Função tryParse converte de texto para numero sem gerar um erro caso o valor não seja valido colocando como padrão 0;
                        Decimal.TryParse(rsMercs["margem_atacado"].ToString(), out mercLoja.margem_atacado);
                        Decimal.TryParse(rsMercs["preco_atacado"].ToString(), out mercLoja.preco_atacado);
                        mercLoja.id_contrato = rsMercs["id_contrato"].ToString();

                        mercadoriasLoja.Add(mercLoja);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    if (rsMercs != null)
                        rsMercs.Close();
                }
            }
            else
            {

                SqlDataReader rsFiliais = null;
                try
                {


                    rsFiliais = Conexao.consulta(sqlfiliais, new User(), false);


                    while (rsFiliais.Read())
                    {
                        bool incluir = false;
                        if (contMerc <= 0)
                        {
                            incluir = true;
                        }
                        else
                        {
                            int exiteFilial = Conexao.countSql("select filial from mercadoria_loja where plu='" + PLU + "' and filial ='" + rsFiliais["Filial"].ToString() + "'", new User());
                            if (exiteFilial <= 0)
                                incluir = true;
                        }
                        if (incluir)
                        {

                            mercadoria_lojaDAO mercLoja = new mercadoria_lojaDAO(this.usr);
                            mercLoja.PLU = this.PLU;
                            mercLoja.Filial = rsFiliais["filial"].ToString();
                            mercLoja.Tipo = Tipo;
                            mercLoja.Saldo_Atual = 0;
                            mercLoja.Preco_Compra = this.preco_compra;
                            mercLoja.Preco_Custo = this.Preco_Custo;
                            mercLoja.Margem = this.Margem;
                            mercLoja.Preco = this.Preco;
                            mercLoja.Preco_Promocao = this.Preco_promocao;
                            mercLoja.Margem_Promocao = this.Margem;
                            mercLoja.Data_Inicio = data_inicio;
                            mercLoja.Data_Fim = data_fim;
                            mercLoja.Preco_Custo_1 = 0;
                            mercLoja.Preco_Custo_2 = 0;
                            mercLoja.Estoque_Minimo = 0;
                            mercLoja.Cobertura = 0;
                            mercLoja.Ultima_Entrada = DateTime.Now;
                            mercLoja.Data_Inventario = data_inventario;
                            mercLoja.Promocao = Promocao;
                            mercLoja.Promocao_automatica = Promocao_automatica;
                            mercLoja.descricao = Descricao;
                            mercLoja.sugerido = 0;
                            mercLoja.descricao_resumida = Descricao_resumida;
                            mercLoja.ingredientes = "";
                            mercLoja.marca = Marca;
                            mercLoja.validade = Validade;
                            mercLoja.codigo_familia = Codigo_familia;
                            mercLoja.qtde_atacado = this.qtde_atacado;
                            mercLoja.margem_atacado = this.margem_atacado;
                            mercLoja.preco_atacado = this.preco_atacado;

                            mercLoja.salvar(true, conn, tran);



                            mercadoriasLoja.Add(mercLoja);

                        }

                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    if (rsFiliais != null)
                    {
                        rsFiliais.Close();
                    }
                }

            }
        }

        public DataTable precosLojas()
        {

            ArrayList itens = new ArrayList();
            ArrayList cabecalho = new ArrayList();
            cabecalho.Add("Filial");
            cabecalho.Add("PLU");
            cabecalho.Add("Tipo");
            cabecalho.Add("Saldo_Atual");
            cabecalho.Add("Preco_Compra");
            cabecalho.Add("Preco_Custo");
            cabecalho.Add("Margem");
            cabecalho.Add("Preco");
            cabecalho.Add("Preco_Promocao");
            cabecalho.Add("Margem_Promocao");
            cabecalho.Add("Data_Inicio");
            cabecalho.Add("Data_Fim");
            cabecalho.Add("Preco_Custo_1");
            cabecalho.Add("Preco_Custo_2");
            cabecalho.Add("Estoque_Minimo");
            cabecalho.Add("Cobertura");
            cabecalho.Add("Ultima_entrada");
            cabecalho.Add("Data_Inventario");
            cabecalho.Add("Promocao");
            cabecalho.Add("Promocao_automatica");
            cabecalho.Add("Descricao");
            cabecalho.Add("sugerido");
            cabecalho.Add("Descricao_resumida");
            cabecalho.Add("ingredientes");
            cabecalho.Add("marca");
            cabecalho.Add("validade");

            cabecalho.Add("qtde_atacado");
            cabecalho.Add("margem_atacado");
            cabecalho.Add("preco_atacado");
            cabecalho.Add("id_contrato");

            itens.Add(cabecalho);
            if (mercadoriasLoja != null && mercadoriasLoja.Count > 0)
            {
                foreach (mercadoria_lojaDAO item in mercadoriasLoja)
                {
                    itens.Add(item.ArrToString());
                }
            }
            return Conexao.GetArryTable(itens);

        }

        public mercadoria_lojaDAO precoLojaObj(String strFilial)
        {
            foreach (mercadoria_lojaDAO item in mercadoriasLoja)
            {
                if (item.Filial.Equals(strFilial))
                    return item;
            }
            return null;
        }

        public void precoLojaObjAtualiza(mercadoria_lojaDAO lj)
        {
            lj.codigo_familia = Codigo_familia;
            int i = 0;
            foreach (mercadoria_lojaDAO item in mercadoriasLoja)
            {
                if (item.Equals(lj))
                    break;

                i++;
            }
            mercadoriasLoja[i] = lj;
        }

        public DataTable precosPromocionais()
        {

            ArrayList itens = new ArrayList();
            ArrayList cabecalho = new ArrayList();
            cabecalho.Add("Filial");
            cabecalho.Add("Codigo_tabela");
            cabecalho.Add("PLU");
            cabecalho.Add("Preco");
            cabecalho.Add("Desconto");
            cabecalho.Add("Preco_promocao");
            cabecalho.Add("Desconto_promocao");
            itens.Add(cabecalho);
            if (arrPrecosPromocionais != null && arrPrecosPromocionais.Count > 0)
            {
                foreach (preco_mercadoriaDAO item in arrPrecosPromocionais)
                {
                    itens.Add(item.ArrToString());
                }
            }
            return Conexao.GetArryTable(itens);

        }

        public void atualizaPrecoTabelas(Decimal vPreco)
        {
            foreach (preco_mercadoriaDAO item in arrPrecosPromocionais)
            {
                item.Preco = vPreco;
                item.Preco_promocao = vPreco - ((vPreco * item.Desconto) / 100);
            }
        }

        public void limparPrecoPromocao()
        {
            arrPrecosPromocionais.Clear();
        }

        public void addPrecoPromocao(preco_mercadoriaDAO preco)
        {
            arrPrecosPromocionais.Add(preco);
        }

        public void removePrecoPromocao(int preco)
        {
            arrPrecosPromocionais.RemoveAt(preco);
        }

        public bool ordemObsAnterioExist(int ordem)
        {
            if (ordem == 0)
                return true;
            int antOrdem = ordem - 1;
            foreach (mercadoria_obsDAO item in arrMercadoriaObs)
            {
                if (item.obrigatorioOrdem == antOrdem)
                    return true;
            }
            return false;
        }


        public void addObs(String obs, String pluAdc, bool obrigatorio, int obrigatorioOrdem, string tipoCardapio)
        {
            if (ordemObsAnterioExist(obrigatorioOrdem))
            {

                mercadoria_obsDAO mercObs = new mercadoria_obsDAO();
                mercObs.filial = "MATRIZ";
                mercObs.plu = this.PLU;
                mercObs.plu_item_adc = pluAdc;
                mercObs.obs = obs;
                mercObs.obrigatorio = obrigatorio;
                mercObs.obrigatorioOrdem = obrigatorioOrdem;
                mercObs.tipoCardapio = tipoCardapio;
                arrMercadoriaObs.Add(mercObs);

            }
            else
            {
                throw new Exception("Não é possivel pular ordem de Obrigatoriedade");
            }
        }
        public void removeObs(String obs, String pluAdc)
        {
            foreach (mercadoria_obsDAO item in arrMercadoriaObs)
            {
                if (item.obs.Equals(obs) & item.plu_item_adc.Equals(pluAdc))
                {
                    arrMercadoriaObs.Remove(item);

                    break;
                }
            }
        }

        public bool obsExist(String obs)
        {
            foreach (mercadoria_obsDAO item in arrMercadoriaObs)
            {
                if (item.obs.Equals(obs))
                {
                    return true;
                }
            }
            return false;
        }

        public DataTable tbObs()
        {
            ArrayList itens = new ArrayList();
            ArrayList cabecalho = new ArrayList();
            cabecalho.Add("obs");
            cabecalho.Add("plu_item_adc");
            cabecalho.Add("Obrigatorio");
            cabecalho.Add("ObrigatorioOrdem");
            cabecalho.Add("tipoCardapio");

            itens.Add(cabecalho);
            if (arrMercadoriaObs != null && arrMercadoriaObs.Count > 0)
            {
                foreach (mercadoria_obsDAO item in arrMercadoriaObs)
                {
                    itens.Add(item.ArrToString());
                }
            }
            return Conexao.GetArryTable(itens);


        }



        public void carregaObs()
        {
            String sqlObs = "Select * from mercadoria_obs where plu='" + PLU + "'";
            SqlDataReader rsObs = null;
            try
            {


                rsObs = Conexao.consulta(sqlObs, null, false);
                while (rsObs.Read())
                {
                    mercadoria_obsDAO obs = new mercadoria_obsDAO();
                    obs.filial = "MATRIZ";
                    obs.plu = PLU;
                    obs.obs = rsObs["obs"].ToString();
                    obs.plu_item_adc = rsObs["plu_item_adc"].ToString();
                    obs.obrigatorio = rsObs["Obrigatorio"].ToString().Equals("1");
                    obs.obrigatorioOrdem = Funcoes.intTry(rsObs["obrigatorioOrdem"].ToString());
                    obs.tipoCardapio = rsObs["tipo"].ToString();
                    arrMercadoriaObs.Add(obs);

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rsObs != null)
                    rsObs.Close();
            }

        }

        public preco_mercadoriaDAO precoPromocial(String CodTabela)
        {

            foreach (preco_mercadoriaDAO prc in arrPrecosPromocionais)
            {
                if (prc.Codigo_tabela.Equals(CodTabela))
                {

                    return prc;
                }

            }
            return null;
        }

        public void addItem(itemDAO item)
        {
            removeItem(item.Plu_item);
            item.Filial = this.Filial;
            item.PLU = this.PLU;

            arrItens.Add(item);
        }

        public void removeItem(String pluItem)
        {
            foreach (itemDAO item in arrItens)
            {
                if (item.Plu_item.Equals(pluItem))
                {
                    arrItens.Remove(item);
                    break;

                }
            }


        }

        public DataTable itensDt()
        {

            ArrayList itens = new ArrayList();
            ArrayList cabecalho = new ArrayList();
            cabecalho.Add("Filial");
            cabecalho.Add("PLU");
            cabecalho.Add("Plu_item");
            cabecalho.Add("Descricao");
            cabecalho.Add("preco_custo");
            cabecalho.Add("Fator_conversao");
            cabecalho.Add("Preco_compra");
            cabecalho.Add("Und");
            cabecalho.Add("Custo_Unitario");
            cabecalho.Add("Qtde");

            itens.Add(cabecalho);
            if (arrItens != null && arrItens.Count > 0)
            {
                foreach (itemDAO item in arrItens)
                {
                    itens.Add(item.ArrToString());
                }
            }
            return Conexao.GetArryTable(itens);

        }



        public void addEan(String CodEan)
        {
            try
            {
                if (Conexao.retornaUmValor("select ean from ean where ean='" + long.Parse(CodEan).ToString() + "'", usr).Equals(""))
                {
                    if (ean == null)
                    {
                        ean = Conexao.GetTable("select  '' ean", usr, false);
                        ean.Rows.Remove(ean.Rows[0]);
                    }
                    DataRow rw = this.ean.NewRow();
                    rw[0] = CodEan;
                    ean.Rows.Add(rw);
                }
                else
                {
                    throw new Exception("Ean já cadastrado");
                }

            }
            catch (Exception err)
            {

                throw new Exception("Não foi possivel incluir o Ean: Erro " + err.Message);
            }
        }


        public void carregarPrecosPromocionais()
        {
            String sqlpreco = "SELECT * " +
                                       " FROM dbo.Preco_Mercadoria Preco_mercadoria " +
                                       " WHERE  Preco_mercadoria.filial = '" + Filial + "' AND  Preco_mercadoria.plu = '" + PLU + "'";
            SqlDataReader rs = null;
            try
            {

                rs = Conexao.consulta(sqlpreco, new User(), true);

                while (rs.Read())
                {
                    preco_mercadoriaDAO preco = new preco_mercadoriaDAO();
                    preco.Filial = Filial;
                    preco.Codigo_tabela = rs["codigo_tabela"].ToString();
                    preco.PLU = PLU;
                    preco.Preco = (Decimal)(rs["Preco"].ToString().Equals("") ? new Decimal() : rs["Preco"]);
                    preco.PrecoCusto = Preco_Custo;
                    preco.Desconto = (Decimal)(rs["Desconto"].ToString().Equals("") ? new Decimal() : rs["Desconto"]);
                    preco.Preco_promocao = (Decimal)(rs["preco_promocao"].ToString().Equals("") ? new Decimal() : rs["preco_promocao"]);
                    preco.Desconto_promocao = (Decimal)(rs["Desconto_promocao"].ToString().Equals("") ? new Decimal() : rs["Desconto_promocao"]);
                    arrPrecosPromocionais.Add(preco);
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

        public void carregarItens()
        {

            if (pluReceita.Equals(""))
                pluReceita = PLU;

            String sqlitens = "SELECT Item.Plu_item, Mercadoria.Descricao , Mercadoria.Preco_Custo , Mercadoria.preco_compra, Item.fator_conversao, Und = isnull(Mercadoria.und_producao, Mercadoria.und), item.qtde " +
                                           " FROM  ( dbo.Item Item  LEFT OUTER JOIN dbo.Mercadoria Mercadoria  ON  Item.Filial = Mercadoria.Filial AND  Item.Plu_item = Mercadoria.PLU) " +
                                           " WHERE  Item.Filial = '" + Filial + "' AND  Item.PLU = '" + pluReceita + "'";

            SqlDataReader rs = null;
            try
            {
                arrItens.Clear();

                rs = Conexao.consulta(sqlitens, new User(), false);
                while (rs.Read())
                {
                    itemDAO it = new itemDAO();
                    it.Filial = Filial;
                    it.PLU = PLU;
                    it.Plu_item = rs["plu_item"].ToString();
                    it.Descricao = rs["descricao"].ToString();
                    it.Preco_custo = Funcoes.decTry(rs["preco_custo"].ToString());

                    it.Fator_conversao = Funcoes.decTry(rs["fator_conversao"].ToString());
                    it.Preco_compra = Funcoes.decTry(rs["preco_custo"].ToString());
                    it.Und = rs["Und"].ToString();
                    it.Qtde = Funcoes.decTry(rs["qtde"].ToString());
                    arrItens.Add(it);
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


        public void carregarProgramacao()
        {

            String sqlitens = "Select prog_seg" +
                             ",prog_ter" +
                             ",prog_qua" +
                             ",prog_qui" +
                             ",prog_sex" +
                             ",prog_sab" +
                             ",prog_dom" +
                        " from mercadoria where plu = '" + pluReceita + "'";
            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta(sqlitens, null, false);
                if (rs.Read())
                {
                    progSeg = rs["prog_seg"].ToString().Equals("1");
                    progTer = rs["prog_ter"].ToString().Equals("1");
                    progQua = rs["prog_qua"].ToString().Equals("1");
                    progQui = rs["prog_qui"].ToString().Equals("1");
                    progSex = rs["prog_sex"].ToString().Equals("1");
                    progSab = rs["prog_sab"].ToString().Equals("1");
                    progDom = rs["prog_dom"].ToString().Equals("1");

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


        public void carregarDados(SqlDataReader rs)
        {
            try
            {

                if (rs.Read())
                {
                    erroCarregar = "";
                    Filial = rs["Filial"].ToString();
                    PLU = rs["PLU"].ToString();
                    Tipo = rs["Tipo"].ToString();
                    Peso_Variavel = rs["Peso_Variavel"].ToString();
                    Codigo_Portaria = rs["Codigo_Portaria"].ToString();
                    Codigo_Tributacao = (Decimal)(rs["Codigo_Tributacao"].ToString().Equals("") ? new Decimal() : rs["Codigo_Tributacao"]);
                    Codigo_Tributacao_ent = (Decimal)(rs["Codigo_Tributacao_ent"].ToString().Equals("") ? new Decimal() : rs["Codigo_Tributacao_ent"]);
                    Codigo_departamento = rs["Codigo_departamento"].ToString();
                    Codigo_familia = rs["Codigo_familia"].ToString();
                    Descricao_departamento = rs["Descricao_departamento"].ToString();
                    Descricao = rs["Descricao"].ToString().Replace("'", "");
                    Descricao_resumida = rs["Descricao_resumida"].ToString().Replace("'", "");
                    Descricao_familia = rs["Descricao_familia"].ToString();
                    Tecla = (Decimal)(rs["Tecla"].ToString().Equals("") ? new Decimal() : rs["Tecla"]);
                    Margem = (Decimal)(rs["Margem"].ToString().Equals("") ? new Decimal() : rs["Margem"]);
                    Estoque_minimo = (Decimal)(rs["Estoque_minimo"].ToString().Equals("") ? new Decimal() : rs["Estoque_minimo"]);
                    Etiqueta = (Decimal)(rs["Etiqueta"].ToString().Equals("") ? new Decimal() : rs["Etiqueta"]);
                    Validade = (Decimal)(rs["Validade"].ToString().Equals("") ? new Decimal() : rs["Validade"]);
                    Preco = (Decimal)(rs["Preco"].ToString().Equals("") ? new Decimal() : rs["Preco"]);
                    Preco_promocao = (Decimal)(rs["Preco_promocao"].ToString().Equals("") ? new Decimal() : rs["Preco_promocao"]);

                    data_inicio = (rs["data_inicio"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["data_inicio"].ToString()));
                    data_fim = (rs["data_fim"].ToString().Equals("") ? data_fim : DateTime.Parse(rs["data_fim"].ToString()));
                    Promocao_automatica = (rs["Promocao_automatica"].ToString().Equals("1") ? true : false);
                    Promocao = (rs["Promocao"].ToString().Equals("1") ? true : false);
                    Preco_Custo = (Decimal)(rs["Preco_Custo"].ToString().Equals("") ? new Decimal() : rs["Preco_Custo"]);
                    Data_Cadastro = (rs["Data_Cadastro"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data_Cadastro"].ToString()));
                    Data_Alteracao = (rs["Data_Alteracao"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data_Alteracao"].ToString()));
                    IPI = (Decimal)(rs["IPI"].ToString().Equals("") ? new Decimal() : rs["IPI"]);
                    Incide_Pis = (rs["Incide_Pis"].ToString().Equals("1") ? true : false);
                    Embalagem = (Decimal)(rs["Embalagem"].ToString().Equals("") ? new Decimal() : rs["Embalagem"]);
                    ultimo_fornecedor = rs["ultimo_fornecedor"].ToString();
                    Fator_conversao = (Decimal)(rs["Fator_conversao"].ToString().Equals("") ? new Decimal() : rs["Fator_conversao"]);
                    Tecla_balanca = (Decimal)(rs["Tecla_balanca"].ToString().Equals("") ? new Decimal() : rs["Tecla_balanca"]);
                    Localizacao = rs["Localizacao"].ToString();
                    Inativo = Funcoes.intTry(rs["Inativo"].ToString());
                    Imprime_etiqueta = (rs["Imprime_etiqueta"].ToString().Equals("1") ? true : false);
                    Estado_Mercadoria = (rs["Estado_Mercadoria"].ToString().Equals("1") ? true : false);
                    saldo_atual = (Decimal)(rs["saldo_atual"].ToString().Equals("") ? new Decimal() : rs["saldo_atual"]);

                    Preco_Custo_1 = (Decimal)(rs["Preco_Custo_1"].ToString().Equals("") ? new Decimal() : rs["Preco_Custo_1"]);
                    Preco_Custo_2 = (Decimal)(rs["Preco_Custo_2"].ToString().Equals("") ? new Decimal() : rs["Preco_Custo_2"]);
                    Ref_fornecedor = rs["Ref_fornecedor"].ToString();
                    data_inventario = (rs["data_inventario"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["data_inventario"].ToString()));
                    saldo_inicial = (Decimal)(rs["saldo_inicial"].ToString().Equals("") ? new Decimal() : rs["saldo_inicial"]);
                    peso = (Decimal)(rs["peso"].ToString().Equals("") ? new Decimal() : rs["peso"]);
                    receita = rs["receita"].ToString(); //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                    qtde_receita = (Decimal)(rs["qtde_receita"].ToString().Equals("") ? new Decimal() : rs["qtde_receita"]);
                    preco_compra = (Decimal)(rs["preco_compra"].ToString().Equals("") ? new Decimal() : rs["preco_compra"]);
                    frete = (Decimal)(rs["frete"].ToString().Equals("") ? new Decimal() : rs["frete"]);
                    seguro = (Decimal)(rs["seguro"].ToString().Equals("") ? new Decimal() : rs["seguro"]);
                    outras_despesas = (Decimal)(rs["outras_despesas"].ToString().Equals("") ? new Decimal() : rs["outras_despesas"]);
                    valor_ipi = (Decimal)(rs["valor_ipi"].ToString().Equals("") ? new Decimal() : rs["valor_ipi"]);
                    codigo_centro_custo = rs["codigo_centro_custo"].ToString();
                    venda_fracionaria = rs["venda_fracionaria"].ToString();
                    //try{pis = (Decimal)(rs["pis"].ToString().Equals("") ? new Decimal() : rs["pis"]);}catch (Exception){erroCarregar +="pis,"; }
                    //try { cofins = (Decimal)(rs["cofins"].ToString().Equals("") ? new Decimal() : rs["cofins"]); }catch (Exception) { erroCarregar += "cofins,"; }
                    cf = rs["cf"].ToString();
                    int.TryParse(rs["CEST"].ToString(), out CEST);
                    und = rs["und"].ToString();
                    gera_inativo = (rs["gera_inativo"].ToString().Equals("1") ? true : false);
                    //try { inventario = rs["inventario"].ToString(); }catch (Exception) { erroCarregar += "inventario,"; }
                    tipo_produto_origem = rs["tipo_produto_origem"].ToString();
                    tipo_produto_destino = rs["tipo_produto_destino"].ToString();
                    curva_a = (rs["curva_a"].ToString().Equals("1") ? true : false);
                    curva_b = (rs["curva_b"].ToString().Equals("1") ? true : false);
                    curva_c = (rs["curva_c"].ToString().Equals("1") ? true : false);
                    estoque_aviso = (rs["estoque_aviso"].ToString().Equals("1") ? true : false);
                    artigo = rs["artigo"].ToString();
                    estoque_margem = (rs["estoque_margem"].ToString().Equals("") ? 0 : int.Parse(rs["estoque_margem"].ToString()));
                    estoque_meses = (rs["estoque_meses"].ToString().Equals("") ? 0 : int.Parse(rs["estoque_meses"].ToString()));
                    cobertura = (rs["cobertura"].ToString().Equals("") ? 0 : int.Parse(rs["cobertura"].ToString()));
                    sazonal1 = (rs["sazonal1"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["sazonal1"].ToString()));
                    sazonal2 = (rs["sazonal2"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["sazonal2"].ToString()));
                    sazonal3 = (rs["sazonal3"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["sazonal3"].ToString()));
                    sazonal4 = (rs["sazonal4"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["sazonal4"].ToString()));
                    sazonal5 = (rs["sazonal5"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["sazonal5"].ToString()));
                    sazonal6 = (rs["sazonal6"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["sazonal6"].ToString()));
                    peso_liquido = (Decimal)(rs["peso_liquido"].ToString().Equals("") ? new Decimal() : rs["peso_liquido"]);
                    peso_bruto = (Decimal)(rs["peso_bruto"].ToString().Equals("") ? new Decimal() : rs["peso_bruto"]);
                    margem_iva = (Decimal)(rs["margem_iva"].ToString().Equals("") ? new Decimal() : rs["margem_iva"]);
                    Marca = rs["Marca"].ToString();

                    //try { mercadoria = rs["mercadoria"].ToString(); }catch (Exception) { erroCarregar += "Mercadoria,"; }


                    codigo_subGrupo = rs["codigo_subGrupo"].ToString();
                    descricao_subGrupo = rs["descricao_subGrupo"].ToString();
                    codigo_Grupo = rs["codigo_grupo"].ToString();
                    descricao_Grupo = rs["descricao_grupo"].ToString();


                    pluReceita = rs["plu_receita"].ToString();
                    Und_producao = rs["und_producao"].ToString();

                    progSeg = rs["prog_seg"].ToString().Equals("1");
                    progTer = rs["prog_ter"].ToString().Equals("1");
                    progQua = rs["prog_qua"].ToString().Equals("1");
                    progQui = rs["prog_qui"].ToString().Equals("1");
                    progSex = rs["prog_sex"].ToString().Equals("1");
                    progSab = rs["prog_sab"].ToString().Equals("1");
                    progDom = rs["prog_dom"].ToString().Equals("1");

                    peso_receita_unitario = Funcoes.decTry(rs["peso_receita_unitario"].ToString());

                    carregarPrecosPromocionais();

                    carregarItens();

                    carregaObs();

                    descricaoTributacao = Conexao.retornaUmValor("select Descricao_tributacao from tributacao where codigo_tributacao=" + Codigo_Tributacao, usr);
                    descricaoTributacaoEnt = Conexao.retornaUmValor("select Descricao_tributacao from tributacao where codigo_tributacao=" + Codigo_Tributacao_ent, usr);
                    ean = Conexao.GetTable("select ean  from ean where plu=" + PLU, null, false);
                    cst_entrada = rs["cst_entrada"].ToString();
                    cst_saida = rs["cst_saida"].ToString();
                    CFOP = rs["cfop"].ToString();

                    linhaCodigo = (rs["cod_linha"].ToString().Equals("") ? 0 : int.Parse(rs["cod_linha"].ToString()));
                    linhaCorCodigo = (rs["cod_cor_linha"].ToString().Equals("") ? 0 : int.Parse(rs["cod_cor_linha"].ToString()));
                    precoReferencia = (Decimal)(rs["Preco_Referencia"].ToString().Equals("") ? new Decimal() : rs["Preco_Referencia"]);

                    pauta = (rs["pauta"].ToString().Equals("") ? 0 : Decimal.Parse(rs["pauta"].ToString()));
                    numero_excecao = rs["numero_excecao"].ToString();
                    codigo_natureza_receita = rs["codigo_natureza_receita"].ToString();

                    pis_perc_entrada = (rs["pis_perc_entrada"].ToString().Equals("") ? 0 : Decimal.Parse(rs["pis_perc_entrada"].ToString()));
                    pis_perc_saida = (rs["pis_perc_saida"].ToString().Equals("") ? 0 : Decimal.Parse(rs["pis_perc_saida"].ToString()));
                    cofins_perc_entrada = (rs["cofins_perc_entrada"].ToString().Equals("") ? 0 : Decimal.Parse(rs["cofins_perc_entrada"].ToString()));
                    cofins_perc_saida = (rs["cofins_perc_saida"].ToString().Equals("") ? 0 : Decimal.Parse(rs["cofins_perc_saida"].ToString()));
                    modalidade_BCICMSST = (rs["modalidade_BCICMSST"].ToString().Equals("") ? -1 : int.Parse(rs["modalidade_BCICMSST"].ToString())); ;
                    origem = (rs["origem"].ToString().Equals("") ? 0 : int.Parse(rs["origem"].ToString())); ;
                    alcoolico = (rs["alcoolico"].ToString().Equals("1") ? true : false);
                    usuario = rs["usuario"].ToString();
                    usuarioAlteracao = rs["usuarioAlteracao"].ToString();

                    Decimal.TryParse(rs["qtde_atacado"].ToString(), out qtde_atacado);
                    Decimal.TryParse(rs["margem_atacado"].ToString(), out margem_atacado);
                    Decimal.TryParse(rs["preco_atacado"].ToString(), out preco_atacado);

                    Decimal.TryParse(rs["porcao"].ToString(), out porcao);
                    porcao_medida = rs["porcao_medida"].ToString();
                    Decimal.TryParse(rs["porcao_numero"].ToString(), out porcao_numero);
                    porcao_div = rs["porcao_div"].ToString();
                    porcao_detalhe = rs["porcao_detalhe"].ToString();

                    vlr_energ_nao = rs["vlr_energ_nao"].ToString().Equals("1");
                    Decimal.TryParse(rs["vlr_energ_Qtde"].ToString(), out vlr_energ_qtde);
                    Decimal.TryParse(rs["vlr_energ_qtde_igual"].ToString(), out vlr_energ_qtde_igual);
                    Decimal.TryParse(rs["vlr_energ_diario"].ToString(), out vlr_energ_diario);

                    carboidratos_nao = rs["carboidratos_nao"].ToString().Equals("1");
                    Decimal.TryParse(rs["carboidratos_qtde"].ToString(), out carboidratos_qtde);
                    Decimal.TryParse(rs["carboidratos_vlr_diario"].ToString(), out carboidratos_vlr_diario);

                    proteinas_nao = rs["proteinas_nao"].ToString().Equals("1");
                    Decimal.TryParse(rs["proteinas_qtde"].ToString(), out proteinas_qtde);
                    Decimal.TryParse(rs["proteinas_vlr_diario"].ToString(), out proteinas_vlr_diario);

                    gorduras_totais_nao = rs["gorduras_totais_nao"].ToString().Equals("1");
                    Decimal.TryParse(rs["gorduras_totais_qtde"].ToString(), out gorduras_totais_qtde);
                    Decimal.TryParse(rs["gorduras_totais_vlr_diario"].ToString(), out gorduras_totais_vlr_diario);

                    gorduras_satu_nao = rs["gorduras_satu_nao"].ToString().Equals("1");
                    Decimal.TryParse(rs["gorduras_satu_qtde"].ToString(), out gorduras_satu_qtde);
                    Decimal.TryParse(rs["gorduras_satu_vlr_diario"].ToString(), out gorduras_satu_vlr_diario);

                    gorduras_trans_nao = rs["gorduras_trans_nao"].ToString().Equals("1");
                    Decimal.TryParse(rs["gorduras_trans_qtde"].ToString(), out gorduras_trans_qtde);

                    fibra_alimen_nao = rs["fibra_alimen_nao"].ToString().Equals("1");
                    Decimal.TryParse(rs["fibra_alimen_qtde"].ToString(), out fibra_alimen_qtde);
                    Decimal.TryParse(rs["fibra_alimen_vlr_diario"].ToString(), out fibra_alimen_vlr_diario);

                    sodio_nao = rs["sodio_nao"].ToString().Equals("1");
                    Decimal.TryParse(rs["sodio_qtde"].ToString(), out sodio_qtde);
                    Decimal.TryParse(rs["sodio_vlr_diario"].ToString(), out sodio_vlr_diario);

                    colesterol_nao = rs["colesterol_nao"].ToString().Equals("1");
                    Decimal.TryParse(rs["colesterol_qtde"].ToString(), out colesterol_qtde);
                    Decimal.TryParse(rs["colesterol_vlr_diario"].ToString(), out colesterol_vlr_diario);

                    sodio_nao = rs["calcio_nao"].ToString().Equals("1");
                    Decimal.TryParse(rs["calcio_qtde"].ToString(), out calcio_qtde);
                    Decimal.TryParse(rs["calcio_vlr_diario"].ToString(), out calcio_vlr_diario);

                    sodio_nao = rs["ferro_nao"].ToString().Equals("1");
                    Decimal.TryParse(rs["ferro_qtde"].ToString(), out ferro_qtde);
                    Decimal.TryParse(rs["ferro_vlr_diario"].ToString(), out ferro_vlr_diario);

                    strEcommerce = rs["texto_ecommerce"].ToString();

                    Descricao_Comercial = rs["Descricao_comercial"].ToString();

                    IntegraWS = rs["IntegraWS"].ToString().Equals("1");

                    Ativo_Ecommerce = rs["Ativo_Ecommerce"].ToString().Equals("1");

                    urlImage = rs["url_img"].ToString();

                    bool pesqRef = Funcoes.valorParametro("UTILIZA_PESQ_REFERENCIA", usr).ToUpper().Equals("TRUE");
                    if (!pesqRef)
                    {
                        if (Ref_fornecedor.Equals(""))
                        {
                            Ref_fornecedor = Conexao.retornaUmValor("Select  codigo_referencia from Fornecedor_Mercadoria where plu ='" + PLU + "' " +
                                                                              "  order by convert(varchar,data,102) desc ", null);
                        }
                    }


                    //NFE 4.0
                    indEscala = rs["indEscala"].ToString().Equals("S");
                    cnpjFabricante = rs["cnpj_Fabricante"].ToString();
                    cBenef = rs["cBenef"].ToString();
                    Prato_dia = rs["prato_dia"].ToString().Equals("1");
                    Prato_dia_1 = rs["prato_dia_1"].ToString().Equals("1");
                    Prato_dia_2 = rs["prato_dia_2"].ToString().Equals("1");
                    Prato_dia_3 = rs["prato_dia_3"].ToString().Equals("1");
                    Prato_dia_4 = rs["prato_dia_4"].ToString().Equals("1");
                    Prato_dia_5 = rs["prato_dia_5"].ToString().Equals("1");
                    Prato_dia_6 = rs["prato_dia_6"].ToString().Equals("1");
                    Prato_dia_7 = rs["prato_dia_7"].ToString().Equals("1");
                    ativa_ce = Funcoes.intTry(rs["Ativa_ce"].ToString());
                    codigo_departamento_ce = rs["Departamento_CE"].ToString();

                    pontos_fidelizacao = Funcoes.intTry(rs["Pontos_fidelizacao"].ToString());

                    Excluir_proxima_integracao = rs["Excluir_proxima_integracao"].ToString().Equals("1");
                    id_Ecommercer = rs["id_Ecommercer"].ToString();
                    bandeja = Funcoes.intTry(rs["bandeja"].ToString());
                    Categoria_eCommerce = rs["Categoria_eCommerce"].ToString();
                    Decimal.TryParse(rs["altura"].ToString(), out altura);
                    Decimal.TryParse(rs["largura"].ToString(), out largura);
                    Decimal.TryParse(rs["profundidade"].ToString(), out profundidade);
                    descricaoWEB = rs["Descricao_WEB"].ToString();
                    SKU = rs["SKU"].ToString();

                    calculaValoresLucro();
                    carregarPrecoLoja();

                    carregarImpressoras();
                    Categoria = rs["Categoria"].ToString();
                    CategoriaDesc = rs["Categoria_Descricao"].ToString();
                    Seguimento = rs["Seguimento"].ToString();
                    SeguimentoDesc = rs["Seguimento_Descricao"].ToString();
                    SubSeguimento = rs["SubSeguimento"].ToString();
                    SubSeguimentoDesc = rs["SubSeguimento_Descricao"].ToString();
                    GrupoCategoria = rs["GrupoCategoria"].ToString();
                    GrupoCategoriaDesc = rs["GrupoCategoria_Descricao"].ToString();
                    SubGrupoCategoria = rs["SubGrupoCategoria"].ToString();
                    SubGrupoCategoriaDesc = rs["SubGrupoCategoria_Descricao"].ToString();

                    impAux = (Funcoes.intTry(rs["impAux"].ToString()) > 0 ? true : false);
                    vendaComSenha = (Funcoes.intTry(rs["Venda_Com_Senha"].ToString()) > 0 ? true : false);

                    PLU_Vinculado = rs["PLU_Vinculado"].ToString();
                    Decimal.TryParse(rs["Fator_Estoque_Vinculado"].ToString(), out fatorEstoqueVinculado);

                    Configuravel = (Funcoes.intTry(rs["Configuravel"].ToString()) > 0 ? true : false);
                    margem_terceiro_preco = Funcoes.decTry(rs["margem_terceiro_preco"].ToString());
                    terceiro_preco = Funcoes.decTry(rs["terceiro_preco"].ToString());

                    codigo_produto_ANVISA = rs["Codigo_Produto_ANVISA"].ToString();
                    motivo_isencao_ANVISA = rs["Motivo_Isencao_ANVISA"].ToString();
                    preco_maximo_ANVISA = Funcoes.decTry(rs["Preco_Maximo_ANVISA"].ToString());
                    codigoEmissaoNFe = rs["Codigo_Emissao_NFe"].ToString();

                    carregarMagentoAtributos();
                }
            }
            catch (Exception err)
            {

                throw new Exception("CarregarDados Mercadorias: " + err.Message);
            }
            finally
            {
                if (rs != null)
                {
                    rs.Close();
                }
            }
        }

        private void calculaValoresLucro()
        {
            decimal VPisConfins = 0;
            decimal piscofins = 0;
            decimal valorCusto = 0;

            decimal custoLiquido = 0;
            decimal vendaLiquida = 0;
            if (Incide_Pis)
            {
                piscofins = 9.25m;
                VPisConfins = Preco * (piscofins / 100);
            }


            if (preco_compra > 0)
                valorCusto = preco_compra;
            else
                valorCusto = Preco_Custo;
            switch (tributacaoSaida.Indice_ST)
            {
                case "70":
                case "10":
                    valorSt = ((((valorCusto * (1 - (tributacaoEntrada.Redutor / 100))) + (valorCusto * (IPI / 100))) * (1 + (margem_iva / 100))) * (tributacaoSaida.Saida_ICMS / 100)) - (valorCusto * ((tributacaoEntrada.Entrada_ICMS / 100) * (1 - (tributacaoEntrada.Redutor / 100))));
                    valorIcms = 0;
                    valorPisCofins = (VPisConfins - (valorCusto * (piscofins / 100)));
                    valorIpi = (valorCusto * (IPI / 100));
                    custoLiquido = valorCusto + (valorCusto * (IPI / 100)) + valorSt - (valorCusto * (piscofins / 100));
                    vendaLiquida = Preco - valorPisCofins;
                    break;
                case "60":
                    valorSt = 0;
                    valorIcms = 0;
                    valorPisCofins = (VPisConfins - (valorCusto * (piscofins / 100)));
                    valorIpi = (valorCusto * (IPI / 100));
                    custoLiquido = valorCusto + (valorCusto * (IPI / 100)) + valorSt - (valorCusto * (piscofins / 100));
                    vendaLiquida = Preco - valorPisCofins;

                    break;
                default:

                    Decimal vIcmsEntrada = (valorCusto * (1 - (tributacaoEntrada.Redutor / 100))) * (tributacaoEntrada.Entrada_ICMS / 100);
                    Decimal vIcmsSaida = (Preco * (1 - (tributacaoEntrada.Redutor / 100))) * (tributacaoSaida.Saida_ICMS / 100);
                    valorSt = 0;
                    valorIcms = vIcmsSaida - vIcmsEntrada;
                    valorPisCofins = (VPisConfins - (valorCusto * (piscofins / 100)));
                    valorIpi = (valorCusto * (IPI / 100));
                    custoLiquido = valorCusto - vIcmsEntrada + (valorCusto * (IPI / 100)) - (valorCusto * (piscofins / 100));
                    vendaLiquida = Preco - valorPisCofins - vIcmsSaida;

                    break;
            }

            valorLucroReal = vendaLiquida - custoLiquido;

        }

        public void salvar(bool novo)
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                this.salvar(novo, conn, tran);
                tran.Commit();
            }
            catch (Exception err)
            {
                tran.Rollback();
                throw err;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

        }
        public void salvar(bool novo, SqlConnection conn, SqlTransaction tran)
        {
            Estado_Mercadoria = true;
            if (novo)
            {
                carregarPrecoLoja(conn, tran);
                insert(conn, tran);
            }
            else
            {
                update(conn, tran);
            }

            atualizarEans(conn, tran);
            atualizarTabelapreco(conn, tran);
            atualizarPrecoLojas(conn, tran);
            atualizarItens(conn, tran);
            atualizarObs(conn, tran);
            atualizarImpressoras(conn, tran);
            
            atualizarAtributos(conn, tran);

        }

        private void update(SqlConnection conn, SqlTransaction tran)
        {
            bool ImprimeEtiquetaItens = false;
            String sqlultimaDescPreco = "select preco,descricao,Preco_promocao,Promocao,isnull((Select Imprimir_Etiqueta_itens from familia where codigo_familia=mercadoria.codigo_familia),0)as Imprimir_Etiqueta_itens from mercadoria where plu='" + PLU + "'";
            SqlDataReader rs = Conexao.consulta(sqlultimaDescPreco, null, false, conn, tran);
            if (rs.Read())
            {
                Decimal vprecoAntigo = (rs["preco"].ToString().Equals("") ? 0 : Decimal.Parse(rs["preco"].ToString()));
                Decimal vprecoPromocao = (rs["Preco_promocao"].ToString().Equals("") ? 0 : Decimal.Parse(rs["Preco_promocao"].ToString()));
                bool vPromocao = (rs["promocao"].ToString().Equals("1") ? true : false);
                if (Codigo_familia.Trim().Equals(""))
                {
                    if (!rs["Descricao"].ToString().Equals(Descricao) || !Preco.Equals(vprecoAntigo) || !Preco_promocao.Equals(vprecoPromocao) || (Promocao != vPromocao))
                        Imprime_etiqueta = true;
                }
                else
                {
                    if (!Preco.Equals(vprecoAntigo) || !Preco_promocao.Equals(vprecoPromocao) || (Promocao != vPromocao))
                    {
                        Imprime_etiqueta = true;

                        String IpEtiquetas = rs["Imprimir_Etiqueta_itens"].ToString();
                        ImprimeEtiquetaItens = IpEtiquetas.Equals("1");
                    }
                }
            }

            if (rs != null)
                rs.Close();



            String sql = "update  Mercadoria set " +
                   "Filial='MATRIZ'," +
                   "PLU='" + PLU + "'," +
                   "Tipo='" + Tipo + "'," +
                   "Peso_Variavel='" + Peso_Variavel + "'," +
                   "Codigo_Portaria='" + Codigo_Portaria + "'," +
                   "Codigo_Tributacao=" + Codigo_Tributacao.ToString() + "," +
                   "Codigo_Tributacao_ent=" + Codigo_Tributacao_ent.ToString() + "," +
                   "Codigo_departamento='" + Codigo_departamento + "'," +
                   "Codigo_familia='" + Codigo_familia + "'," +
                   "Descricao_departamento='" + Descricao_departamento + "'," +
                   "Descricao='" + Funcoes.RemoverAcentos(Descricao.Replace("'", "")) + "'," +
                   "Descricao_resumida='" + Funcoes.RemoverAcentos(Descricao_resumida.Replace("'", "")) + "'," +
                   "Descricao_familia='" + Descricao_familia + "'," +
                   "Tecla=" + Tecla.ToString() + "," +
                   "Margem=" + Margem.ToString().Replace(",", ".") + "," +
                   "Estoque_minimo=" + Estoque_minimo.ToString().Replace(",", ".") + "," +
                   "Etiqueta=" + Etiqueta.ToString().Replace(",", ".") + "," +
                   "Validade=" + Validade.ToString().Replace(",", ".") + "," +
                   "Preco=" + Preco.ToString().Replace(",", ".") + "," +
                   "Preco_promocao=" + Preco_promocao.ToString().Replace(",", ".") + "," +
                   "Promocao_automatica=" + (Promocao_automatica ? 1 : 0) + "," +
                   "Promocao=" + (Promocao ? 1 : 0) + "," +
                   "Preco_Custo=" + Preco_Custo.ToString().Replace(",", ".") + "," +
                   "Data_Cadastro=" + (Data_Cadastro.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Cadastro.ToString("yyyy-MM-dd") + "'") + "," +
                   "Data_inicio=" + (data_inicio.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_inicio.ToString("yyyy-MM-dd") + "'") + "," +
                   "Data_fim=" + (data_fim.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_fim.ToString("yyyy-MM-dd") + "'") + "," +

                   "Data_Alteracao= getdate()," +// '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                   "IPI=" + IPI.ToString().Replace(",", ".") + "," +
                   "Incide_Pis=" + (Incide_Pis ? 1 : 0) + "," +
                   "Embalagem=" + Embalagem.ToString().Replace(",", ".") + "," +
                   "ultimo_fornecedor='" + ultimo_fornecedor + "'," +
                   "Fator_conversao=" + Fator_conversao.ToString().Replace(",", ".") + "," +
                   "Tecla_balanca=" + Tecla_balanca.ToString().Replace(",", ".") + "," +
                   "Localizacao='" + Localizacao + "'," +
                   "Inativo=" + (Inativo == 3 ? 0 : Inativo).ToString() + "," +
                   "Imprime_etiqueta=" + ((Imprime_etiqueta && Codigo_familia.Trim().Equals("")) || ImprimeEtiquetaItens ? "1" : "0") + "," +
                   "Estado_Mercadoria=1," +
                   "saldo_atual=" + saldo_atual.ToString().Replace(",", ".") + "," +
                   "Preco_Custo_1=" + Preco_Custo_1.ToString().Replace(",", ".") + "," +
                   "Preco_Custo_2=" + Preco_Custo_2.ToString().Replace(",", ".") + "," +
                   "Ref_fornecedor='" + Ref_fornecedor + "'," +
                   "data_inventario=" + (data_inventario.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_inventario.ToString("yyyy-MM-dd") + "'") + "," +
                   "saldo_inicial=" + saldo_inicial.ToString().Replace(",", ".") + "," +
                   "peso=" + peso.ToString().Replace(",", ".") + "," +
                   "receita='" + receita + "'," +
                   "qtde_receita=" + qtde_receita.ToString().Replace(",", ".") + "," +
                   "preco_compra=" + preco_compra.ToString().Replace(",", ".") + "," +
                   "frete=" + frete.ToString().Replace(",", ".") + "," +
                   "seguro=" + seguro.ToString().Replace(",", ".") + "," +
                   "outras_despesas=" + outras_despesas.ToString().Replace(",", ".") + "," +
                   "valor_ipi=" + valor_ipi.ToString().Replace(",", ".") + "," +
                   "codigo_centro_custo='" + codigo_centro_custo + "'," +
                   "venda_fracionaria='" + venda_fracionaria + "'," +
                   //"pis=" +  pis.ToString().Replace(",",".") + "," +
                   //"cofins=" + cofins.ToString().Replace(",",".") + "," +
                   "cf='" + cf + "'," +
                   "CEST=" + CEST.ToString() + "," +
                   "und='" + und + "'," +
                   "gera_inativo=" + (gera_inativo ? 1 : 0) + "," +
                   //"inventario='" + inventario + "'," +
                   "tipo_produto_origem='" + tipo_produto_origem + "'," +
                   "tipo_produto_destino='" + tipo_produto_destino + "'," +
                   "curva_a=" + (curva_a ? 1 : 0) + "," +
                   "curva_b=" + (curva_b ? 1 : 0) + "," +
                   "curva_c=" + (curva_c ? 1 : 0) + "," +
                   "estoque_aviso=" + (estoque_aviso ? 1 : 0) + "," +
                   "artigo='" + artigo + "'," +
                   "estoque_margem=" + estoque_margem + "," +
                   "estoque_meses=" + estoque_meses + "," +
                   "cobertura=" + cobertura + "," +
                   "sazonal1=" + (sazonal1.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + sazonal1.ToString("yyyy-MM-dd") + "'") + "," +
                   "sazonal2=" + (sazonal2.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + sazonal2.ToString("yyyy-MM-dd") + "'") + "," +
                   "sazonal3=" + (sazonal3.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + sazonal3.ToString("yyyy-MM-dd") + "'") + "," +
                   "sazonal4=" + (sazonal4.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + sazonal4.ToString("yyyy-MM-dd") + "'") + "," +
                   "sazonal5=" + (sazonal5.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + sazonal5.ToString("yyyy-MM-dd") + "'") + "," +
                   "sazonal6=" + (sazonal6.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + sazonal6.ToString("yyyy-MM-dd") + "'") + "," +
                   "peso_liquido=" + peso_liquido.ToString().Replace(",", ".") + "," +
                   "peso_bruto=" + peso_bruto.ToString().Replace(",", ".") + "," +
                   "margem_iva=" + margem_iva.ToString().Replace(",", ".") + "," +
                   "cst_entrada='" + cst_entrada.ToString() + "'," +
                   "cst_saida='" + cst_saida + "'," +
                   "Marca='" + Marca + "'," +
                   "cod_linha=" + linhaCodigo + "," +
                   "cod_cor_linha=" + linhaCorCodigo + "," +
                   "preco_referencia=" + precoReferencia.ToString().Replace(",", ".") + "," +
                   "origem=" + origem + "," +
                   "alcoolico=" + (alcoolico ? "1" : "0") + "," +
                   "pis_perc_entrada=" + pis_perc_entrada.ToString("N2").Replace(",", ".") + "," +
                   "cofins_perc_entrada=" + cofins_perc_entrada.ToString("N2").Replace(",", ".") + "," +
                   "pis_perc_saida=" + pis_perc_saida.ToString("N2").Replace(",", ".") + "," +
                   "cofins_perc_saida=" + cofins_perc_saida.ToString("N2").Replace(",", ".") + "," +
                   "pauta=" + pauta.ToString("N2").Replace(",", ".") + "," +
                   "numero_excecao='" + numero_excecao + "'," +
                   "codigo_natureza_receita='" + codigo_natureza_receita + "'," +
                   "modalidade_BCICMSST=" + (modalidade_BCICMSST == -1 ? "null" : modalidade_BCICMSST.ToString()) +
                   ",usuario='" + usuario + "' " +
                   ",qtde_atacado=" + qtde_atacado.ToString().Replace(",", ".") +
                   ",margem_atacado=" + margem_atacado.ToString().Replace(",", ".") +
                   ",preco_atacado=" + preco_atacado.ToString().Replace(",", ".") +
                   ",margem_terceiro_preco=" + Funcoes.decimalPonto(margem_terceiro_preco) +
                   ",terceiro_preco=" + Funcoes.decimalPonto(terceiro_preco) +

                   ",porcao=" + porcao.ToString().Replace(",", ".") +
                   ",porcao_medida='" + porcao_medida + "'" +
                   ",porcao_numero=" + porcao_numero.ToString().Replace(",", ".") +
                   ",porcao_div='" + porcao_div + "'" +
                   ",porcao_detalhe='" + porcao_detalhe + "'" +
                   ",vlr_energ_nao=" + (vlr_energ_nao ? "1" : "0") +
                   ",vlr_energ_qtde=" + vlr_energ_qtde.ToString().Replace(",", ".") +
                   ",vlr_energ_qtde_igual=" + vlr_energ_qtde_igual.ToString().Replace(",", ".") +
                   ",vlr_energ_diario=" + vlr_energ_diario.ToString().Replace(",", ".") +
                   ",carboidratos_nao=" + (carboidratos_nao ? "1" : "0") +
                   ",carboidratos_qtde=" + carboidratos_qtde.ToString().Replace(",", ".") +
                   ",carboidratos_vlr_diario=" + carboidratos_vlr_diario.ToString().Replace(",", ".") +
                   ",proteinas_nao=" + (proteinas_nao ? "1" : "0") +
                   ",proteinas_qtde=" + proteinas_qtde.ToString().Replace(",", ".") +
                   ",proteinas_vlr_diario=" + proteinas_vlr_diario.ToString().Replace(",", ".") +
                   ",gorduras_totais_nao=" + (gorduras_totais_nao ? "1" : "0") +
                   ",gorduras_totais_qtde=" + gorduras_totais_qtde.ToString().Replace(",", ".") +
                   ",gorduras_totais_vlr_diario=" + gorduras_totais_vlr_diario.ToString().Replace(",", ".") +
                   ",gorduras_satu_nao=" + (gorduras_satu_nao ? "1" : "0") +
                   ",gorduras_satu_qtde=" + gorduras_satu_qtde.ToString().Replace(",", ".") +
                   ",gorduras_satu_vlr_diario=" + gorduras_satu_vlr_diario.ToString().Replace(",", ".") +
                   ",gorduras_trans_nao=" + (gorduras_trans_nao ? "1" : "0") +
                   ",gorduras_trans_qtde=" + gorduras_trans_qtde.ToString().Replace(",", ".") +
                   ",fibra_alimen_nao=" + (fibra_alimen_nao ? "1" : "0") +
                   ",fibra_alimen_qtde=" + fibra_alimen_qtde.ToString().Replace(",", ".") +
                   ",fibra_alimen_vlr_diario=" + fibra_alimen_vlr_diario.ToString().Replace(",", ".") +
                   ",sodio_nao=" + (sodio_nao ? "1" : "0") +
                   ",sodio_qtde=" + sodio_qtde.ToString().Replace(",", ".") +
                   ",sodio_vlr_diario=" + sodio_vlr_diario.ToString().Replace(",", ".") +
                   ",texto_ecommerce='" + strEcommerce + "'" +
                   ",descricao_comercial='" + Descricao_Comercial + "'" +
                   ",IntegraWS=" + (IntegraWS ? "1" : "0") +
                   ",Ativo_Ecommerce=" + (Ativo_Ecommerce ? "1" : "0") +
                   ",indEscala='" + (indEscala ? "S" : "N") + "'" +
                   ",cnpj_Fabricante='" + cnpjFabricante + "'" +
                   ",cBenef='" + cBenef + "'" +
                   ",prato_dia=" + (Prato_dia ? "1" : "0") +
                   ",prato_dia_1=" + (Prato_dia_1 ? "1" : "0") +
                   ",prato_dia_2=" + (Prato_dia_2 ? "1" : "0") +
                   ",prato_dia_3=" + (Prato_dia_3 ? "1" : "0") +
                   ",prato_dia_4=" + (Prato_dia_4 ? "1" : "0") +
                   ",prato_dia_5=" + (Prato_dia_5 ? "1" : "0") +
                   ",prato_dia_6=" + (Prato_dia_6 ? "1" : "0") +
                   ",prato_dia_7=" + (Prato_dia_7 ? "1" : "0") +
                   ",informacoes_extras='" + informacoes_extras + "'" +
                   ",plu_receita='" + pluReceita + "'" +
                   ",und_producao='" + Und_producao + "'" +
                   ",prog_seg=" + (progSeg ? "1" : "0") +
                   ",prog_ter=" + (progTer ? "1" : "0") +
                   ",prog_qua=" + (progQua ? "1" : "0") +
                   ",prog_qui=" + (progQui ? "1" : "0") +
                   ",prog_sex=" + (progSex ? "1" : "0") +
                   ",prog_sab=" + (progSab ? "1" : "0") +
                   ",prog_dom=" + (progDom ? "1" : "0") +
                   ",peso_receita_unitario=" + Funcoes.decimalPonto(peso_receita_unitario.ToString()) +
                    ",filial_produzido='" + filial_produzido + "'" +
                   ",tipo_producao='" + tipo_producao + "'" +
                   ",Ativa_ce=" + ativa_ce.ToString() +
                   ",Departamento_CE= '" + codigo_departamento_ce + "'" +
                   ",cfop='" + CFOP + "'" +
                   ",und_compra='" + und_compra + "'" +
                   ",descricao_producao='" + descricao_producao + "'" +
                   ",embalagem_producao=" + embalagem_producao +
                   ",custo_producao=" + Funcoes.decimalPonto(custo_producao.ToString()) +
                   ",Pontos_fidelizacao =" + pontos_fidelizacao.ToString() +
                   ",Excluir_proxima_integracao=" + (Excluir_proxima_integracao ? "1" : "0") +
                   ",id_ecommercer='" + id_Ecommercer + "'" +
                   ",bandeja=" + bandeja.ToString() +
                   ",Categoria='" + Categoria + "'" +
                   ",Seguimento='" + Seguimento + "'" +
                   ",SubSeguimento='" + SubSeguimento + "'" +
                   ",GrupoCategoria='" + GrupoCategoria + "'" +
                   ",SubGrupoCategoria='" + SubGrupoCategoria + "'" +
                   ",UsuarioAlteracao='" + usuarioAlteracao + "'" +
                   ",Categoria_eCommerce='" + Categoria_eCommerce + "'" +
                   ", Altura = " + Funcoes.decimalPonto(altura.ToString()) +
                   ", Largura = " + Funcoes.decimalPonto(largura.ToString()) +
                   ", profundidade = " + Funcoes.decimalPonto(profundidade.ToString()) +
                   ", ImpAux = " + (impAux ? "1" : "0") +
                   ", Venda_Com_Senha = " + (vendaComSenha ? "1" : "0") +
                   ", PLU_Vinculado = '" + PLU_Vinculado + "'" +
                   ", Fator_Estoque_Vinculado = " + Funcoes.decimalPonto(fatorEstoqueVinculado.ToString()) +
                   ", Descricao_WEB  = '" + descricaoWEB + "'" +
                   ", SKU = '" + SKU + "'" +
                   ", Configuravel = " + (Configuravel ? "1" : "0") +
                   ", Codigo_Produto_ANVISA = '" + codigo_produto_ANVISA.Trim() + "'" +
                   ", Motivo_Isencao_ANVISA = '" + motivo_isencao_ANVISA.Trim() + "'" +
                   ", Preco_Maximo_ANVISA = " + Funcoes.decimalPonto(preco_maximo_ANVISA.ToString()) +
                   ", Codigo_Emissao_NFe = '" + codigoEmissaoNFe.Trim() + "'" +
                   ",colesterol_nao=" + (colesterol_nao ? "1" : "0") +
                   ",colesterol_qtde=" + colesterol_qtde.ToString().Replace(",", ".") +
                   ",colesterol_vlr_diario=" + colesterol_vlr_diario.ToString().Replace(",", ".") +
                   ",calcio_nao=" + (calcio_nao ? "1" : "0") +
                   ",calcio_qtde=" + calcio_qtde.ToString().Replace(",", ".") +
                   ",calcio_vlr_diario=" + calcio_vlr_diario.ToString().Replace(",", ".") +
                   ",ferro_nao=" + (ferro_nao ? "1" : "0") +
                   ",ferro_qtde=" + ferro_qtde.ToString().Replace(",", ".") +
                   ",ferro_vlr_diario=" + ferro_vlr_diario.ToString().Replace(",", ".") +
             " where plu='" + PLU + "'";

            Conexao.executarSql(sql, conn, tran);
            if (!usr.filial.inibe_marcacao_familia)
            {
                if (Codigo_familia.Trim().Length > 0 && Funcoes.isnumeroint(Codigo_familia) && int.Parse(Codigo_familia) > 0)
                {
                    SqlDataReader rsFmLOjas = null;
                    ArrayList sqlsFiliais = new ArrayList();
                    try
                    {


                        rsFmLOjas = Conexao.consulta("select PLU,isnull(Preco_Custo,0)preco_custo from mercadoria where Codigo_familia='" + Codigo_familia + "' AND PLU <> '" + PLU + "'", null, false, conn, tran);

                        while (rsFmLOjas.Read())
                        {
                            Decimal fmCustoLoja = Decimal.Parse(rsFmLOjas["preco_custo"].ToString());
                            Decimal fmMargemLoja = Funcoes.verificamargem(fmCustoLoja, Preco, 0, 0);


                            sql = "update  Mercadoria set " +
                                " Margem =" + fmMargemLoja.ToString().Replace(",", ".") +
                                ", Preco =" + Preco.ToString().Replace(",", ".") +
                                ", Etiqueta = 1" +
                                ", Imprime_etiqueta=" + (ImprimeEtiquetaItens ? "1" : "0") +
                                ", Estado_Mercadoria = 1" +
                                ",Preco_promocao=" + Preco_promocao.ToString().Replace(",", ".") +
                                ",Promocao_automatica=" + (Promocao_automatica ? 1 : 0) +
                                ",Promocao=" + (Promocao ? 1 : 0) +
                                ",Data_inicio=" + (data_inicio.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_inicio.ToString("yyyy-MM-dd") + "'") +
                                ",Data_fim=" + (data_fim.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_fim.ToString("yyyy-MM-dd") + "'") +
                                ",qtde_atacado=" + qtde_atacado.ToString().Replace(",", ".") +
                                ",margem_atacado=" + margem_atacado.ToString().Replace(",", ".") +
                                ",preco_atacado=" + preco_atacado.ToString().Replace(",", ".") +
                                ",Data_Alteracao = getDate() " + //'" + DateTime.Today.ToString("yyyy-MM-dd") + "'" +
                            "  where  PLU= '" + rsFmLOjas["plu"].ToString() + "'";
                            sqlsFiliais.Add(sql);
                            ////Conexao.executarSql(sql, conn, tran);
                            String sqlTb = "update Preco_Mercadoria " +
                                   "set preco=" + Funcoes.decimalPonto(Preco.ToString()) + ", preco_promocao =case when desconto=0 then " + Funcoes.decimalPonto(Preco.ToString()) + "  else " + Funcoes.decimalPonto(Preco.ToString()) + "- ((" + Funcoes.decimalPonto(Preco.ToString()) + " * desconto)/100) end " +
                                   " where plu ='" + rsFmLOjas["plu"].ToString() + "'";

                            sqlsFiliais.Add(sqlTb);
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {
                        if (rsFmLOjas != null)
                            rsFmLOjas.Close();

                    }

                    if (sqlsFiliais.Count > 0)
                    {
                        foreach (String item in sqlsFiliais)
                        {
                            Conexao.executarSql(item, conn, tran);
                        }

                    }

                    sql = " update familia set preco=" + Preco.ToString().Replace(",", ".") + ",imprime_etiqueta=" + (Imprime_etiqueta && !ImprimeEtiquetaItens ? "1" : "0") + " where codigo_familia='" + Codigo_familia + "'";
                    Conexao.executarSql(sql, conn, tran);


                }
            }


        }

        public void inativar()
        {

            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {

                String sql = "update mercadoria set inativo =1 where plu='" + PLU + "'";
                Conexao.executarSql(sql, conn, tran);

                sql = " delete from fornecedor_mercadoria where plu ='" + PLU + "'";
                Conexao.executarSql(sql, conn, tran);
                tran.Commit();
            }
            catch (Exception err)
            {
                tran.Rollback();
                throw err;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }


        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            Data_Cadastro = DateTime.Now;
            usuario = usr.getUsuario();
            usuarioAlteracao = usr.getUsuario();
            String sql = "insert into mercadoria (Filial," +
            "PLU," +
            "Tipo," +
            "Peso_Variavel," +
            "Codigo_Portaria," +
            "Codigo_Tributacao," +
            "Codigo_Tributacao_ent," +
            "Codigo_departamento," +
            "Codigo_familia," +
            "Descricao_departamento," +
            "Descricao," +
            "Descricao_resumida," +
            "Descricao_familia," +
            "Tecla," +
            "Margem," +
            "Estoque_minimo," +
            "Etiqueta," +
            "Validade," +
            "Preco," +
            "Preco_promocao," +
            "data_inicio," +
            "data_fim," +
            "Promocao_automatica," +
            "Promocao," +
            "Preco_Custo," +
            "Data_Cadastro," +
            "Data_Alteracao," +
            "IPI," +
            "Incide_Pis," +
            "Embalagem," +
            "ultimo_fornecedor," +
            "Fator_conversao," +
            "Tecla_balanca," +
            "Localizacao," +
            "Inativo," +
            "Imprime_etiqueta," +
            "Estado_Mercadoria," +
            "saldo_atual," +
            "Preco_Custo_1," +
            "Preco_Custo_2," +
            "Ref_fornecedor," +
            "data_inventario," +
            "saldo_inicial," +
            "peso," +
            "receita," +
            "qtde_receita," +
            "preco_compra," +
            "frete," +
            "seguro," +
            "outras_despesas," +
            "valor_ipi," +
            "codigo_centro_custo," +
            "venda_fracionaria," +
            "cf," +
            "CEST," +
            "und," +
            "gera_inativo," +
            //  "inventario," +
            "tipo_produto_origem," +
            "tipo_produto_destino," +
            "curva_a," +
            "curva_b," +
            "curva_c," +
            "estoque_aviso," +
            "artigo," +
            "estoque_margem," +
            "estoque_meses," +
            "cobertura," +
            "sazonal1," +
            "sazonal2," +
            "sazonal3," +
            "sazonal4," +
            "sazonal5," +
            "sazonal6," +
            "peso_liquido," +
            "peso_bruto," +
            "margem_iva," +
            "Marca," +
            "cst_entrada," +
            "cst_saida," +
            "cod_linha," +
            "cod_cor_linha," +
            "preco_referencia," +
            "origem," +
            "alcoolico," +
            "pis_perc_entrada," +
            "cofins_perc_entrada," +
            "pis_perc_saida," +
            "cofins_perc_saida," +
            "pauta," +
            "numero_excecao," +
            "codigo_natureza_receita," +
            "modalidade_BCICMSST," +
            "usuario" +
            ",qtde_atacado" +
            ",margem_atacado" +
            ",preco_atacado" +
            ",porcao" +
            ",porcao_medida" +
            ",porcao_numero" +
            ",porcao_div" +
            ",porcao_detalhe" +
            ",vlr_energ_nao" +
            ",vlr_energ_qtde" +
            ",vlr_energ_qtde_igual" +
            ",vlr_energ_diario" +
            ",carboidratos_nao" +
            ",carboidratos_qtde" +
            ",carboidratos_vlr_diario" +
            ",proteinas_nao" +
            ",proteinas_qtde" +
            ",proteinas_vlr_diario" +
            ",gorduras_totais_nao" +
            ",gorduras_totais_qtde" +
            ",gorduras_totais_vlr_diario" +
            ",gorduras_satu_nao" +
            ",gorduras_satu_qtde" +
            ",gorduras_satu_vlr_diario" +
            ",gorduras_trans_nao" +
            ",gorduras_trans_qtde" +
            ",fibra_alimen_nao" +
            ",fibra_alimen_qtde" +
            ",fibra_alimen_vlr_diario" +
            ",sodio_nao" +
            ",sodio_qtde" +
            ",sodio_vlr_diario" +
            ",texto_ecommerce" +
            ",Descricao_Comercial" +
            ",IntegraWS" +
            ",Ativo_Ecommerce" +
            ",indEscala" +
            ",cnpj_Fabricante" +
            ",cBenef" +
            ",prato_dia" +
            ",prato_dia_1" +
            ",prato_dia_2" +
            ",prato_dia_3" +
            ",prato_dia_4" +
            ",prato_dia_5" +
            ",prato_dia_6" +
            ",prato_dia_7" +
            ",informacoes_extras" +
            ",plu_receita" +
            ",und_producao" +
            ",prog_seg" +
            ",prog_ter" +
            ",prog_qua" +
            ",prog_qui" +
            ",prog_sex" +
            ",prog_sab" +
            ",prog_dom" +
            ",peso_receita_unitario" +
            ",filial_produzido" +
            ",tipo_producao" +
            ",Ativa_CE" +
            ",Departamento_CE" +
            ",cfop" +
            ",und_compra" +
            ",descricao_producao" +
            ",embalagem_producao" +
            ",custo_producao" +
            ",Pontos_fidelizacao" +
            ",excluir_proxima_integracao" +
            ",id_ecommercer" +
            ",bandeja" +
            ",Categoria" +
            ",Seguimento" +
            ",SubSeguimento" +
            ",GrupoCategoria" +
            ",SubGrupoCategoria" +
            ",UsuarioAlteracao" +
            ",Categoria_eCommerce" +
            ", Altura" +
            ", Largura" +
            ", Profundidade" +
            ", ImpAux" +
            ", Venda_Com_Senha" +
            ", PLU_Vinculado" +
            ", Fator_Estoque_Vinculado" +
            ", Descricao_WEB" +
            ", SKU" +
            ", Configuravel" +
            ", margem_terceiro_preco" +
            ", terceiro_preco" +
            ", Codigo_Produto_ANVISA" +
            ", Motivo_Isencao_ANVISA" +
            ", Preco_Maximo_ANVISA" +
            ", Codigo_Emissao_NFe" +
            ",colesterol_nao" +
            ",colesterol_qtde" +
            ",colesterol_vlr_diario" +
            ",calcio_nao" +
            ",calcio_qtde" +
            ",calcio_vlr_diario" +
            ",ferro_nao" +
            ",ferro_qtde" +
            ",ferro_vlr_diario" +
            ")";
            String strValues = " Values (" +
              "'MATRIZ'," +
              "'" + PLU + "'," +
              "'" + Tipo + "'," +
              "'" + Peso_Variavel + "'," +
              "'" + Codigo_Portaria + "'," +
              Codigo_Tributacao.ToString().Replace(",", ".") + "," +
              Codigo_Tributacao_ent.ToString().Replace(",", ".") + "," +
              "'" + Codigo_departamento + "'," +
              "'" + Codigo_familia + "'," +
              "'" + Descricao_departamento + "'," +
              "'" + Funcoes.RemoverAcentos(Descricao.Replace("'", "")) + "'," +
              "'" + Funcoes.RemoverAcentos(Descricao_resumida.Replace("'", "")) + "'," +
              "'" + Descricao_familia + "'," +
              Tecla.ToString().Replace(",", ".") + "," +
              Margem.ToString().Replace(",", ".") + "," +
              Estoque_minimo.ToString().Replace(",", ".") + "," +
              Etiqueta.ToString().Replace(",", ".") + "," +
              Validade.ToString().Replace(",", ".") + "," +
              Preco.ToString().Replace(",", ".") + "," +
              Preco_promocao.ToString().Replace(",", ".") + "," +
              (data_inicio.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_inicio.ToString("yyyy-MM-dd") + "'") + "," +
              (data_fim.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_fim.ToString("yyyy-MM-dd") + "'") + "," +
              (Promocao_automatica ? 1 : 0) + "," +
              (Promocao ? 1 : 0) + "," +
              Preco_Custo.ToString().Replace(",", ".") + "," +
              (Data_Cadastro.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Cadastro.ToString("yyyy-MM-dd") + "'") + "," +
              //(Data_Alteracao.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Alteracao.ToString("yyyy-MM-dd") + "'") + "," +
              "getdate()," +
              IPI.ToString().Replace(",", ".") + "," +
              (Incide_Pis ? "1" : "0") + "," +
              Embalagem.ToString().Replace(",", ".") + "," +
              "'" + ultimo_fornecedor + "'," +
              Fator_conversao.ToString().Replace(",", ".") + "," +
              Tecla_balanca.ToString().Replace(",", ".") + "," +
              "'" + Localizacao + "'," +
              Inativo.ToString() + "," +
              "1," + //imprime Etiqueta
              "1," + //estado mercadoria
              saldo_atual.ToString().Replace(",", ".") + "," +
              Preco_Custo_1.ToString().Replace(",", ".") + "," +
              Preco_Custo_2.ToString().Replace(",", ".") + "," +
              "'" + Ref_fornecedor + "'," +
              (data_inventario.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_inventario.ToString("yyyy-MM-dd") + "'") + "," +
              saldo_inicial.ToString().Replace(",", ".") + "," +
              peso.ToString().Replace(",", ".") + "," +
              "'" + _receita + "'," +
              qtde_receita.ToString().Replace(",", ".") + "," +
              preco_compra.ToString().Replace(",", ".") + "," +
              frete.ToString().Replace(",", ".") + "," +
              seguro.ToString().Replace(",", ".") + "," +
              outras_despesas.ToString().Replace(",", ".") + "," +
              valor_ipi.ToString().Replace(",", ".") + "," +
              "'" + codigo_centro_custo + "'," +
              "'" + venda_fracionaria + "'," +


              "'" + cf + "'," +
                CEST.ToString() + "," +
              "'" + und + "'," +
              (gera_inativo ? 1 : 0) + "," +
              //   "'" + inventario + "'," +
              "'" + tipo_produto_origem + "'," +
              "'" + tipo_produto_destino + "'," +
              (curva_a ? 1 : 0) + "," +
              (curva_b ? 1 : 0) + "," +
              (curva_c ? 1 : 0) + "," +
              (estoque_aviso ? 1 : 0) + "," +
              "'" + artigo + "'," +
              estoque_margem + "," +
              estoque_meses + "," +
              cobertura + "," +
              (sazonal1.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + sazonal1.ToString("yyyy-MM-dd") + "'") + "," +
              (sazonal2.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + sazonal2.ToString("yyyy-MM-dd") + "'") + "," +
              (sazonal3.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + sazonal3.ToString("yyyy-MM-dd") + "'") + "," +
              (sazonal4.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + sazonal4.ToString("yyyy-MM-dd") + "'") + "," +
              (sazonal5.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + sazonal5.ToString("yyyy-MM-dd") + "'") + "," +
              (sazonal6.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + sazonal6.ToString("yyyy-MM-dd") + "'") + "," +

              peso_liquido.ToString().Replace(",", ".") + "," +
              peso_bruto.ToString().Replace(",", ".") + "," +
              margem_iva.ToString().Replace(",", ".") + "," +
              "'" + Marca + "'," +
              "'" + cst_entrada + "'," +
              "'" + cst_saida + "'," +
              linhaCodigo + "," +
              linhaCorCodigo + "," +
              precoReferencia.ToString().Replace(",", ".") + "," +
              origem + "," +
              (alcoolico ? "1" : "0") + "," +
              pis_perc_entrada.ToString("N2").Replace(",", ".") + "," +
              cofins_perc_entrada.ToString("N2").Replace(",", ".") + "," +
              pis_perc_saida.ToString("N2").Replace(",", ".") + "," +
              cofins_perc_saida.ToString("N2").Replace(",", ".") + "," +
              pauta.ToString("N2").Replace(",", ".") + "," +
              "'" + numero_excecao + "'," +
              "'" + codigo_natureza_receita + "'," +
              (modalidade_BCICMSST == -1 ? "null" : modalidade_BCICMSST.ToString()) + "," +
              "'" + usuario + "'" +
              "," + qtde_atacado.ToString().Replace(",", ".") +
              "," + margem_atacado.ToString().Replace(",", ".") +
              "," + preco_atacado.ToString().Replace(",", ".") +

              "," + porcao.ToString().Replace(",", ".") +
               ",'" + porcao_medida + "'" +
               "," + porcao_numero.ToString().Replace(",", ".") +
               ",'" + porcao_div + "'" +
               ",'" + porcao_detalhe + "'" +

               "," + (vlr_energ_nao ? "1" : "0") +
               "," + vlr_energ_qtde.ToString().Replace(",", ".") +
               "," + vlr_energ_qtde_igual.ToString().Replace(",", ".") +
               "," + vlr_energ_diario.ToString().Replace(",", ".") +

               "," + (carboidratos_nao ? "1" : "0") +
               "," + carboidratos_qtde.ToString().Replace(",", ".") +
               "," + carboidratos_vlr_diario.ToString().Replace(",", ".") +
               "," + (proteinas_nao ? "1" : "0") +
               "," + proteinas_qtde.ToString().Replace(",", ".") +
               "," + proteinas_vlr_diario.ToString().Replace(",", ".") +
               "," + (gorduras_totais_nao ? "1" : "0") +
               "," + gorduras_totais_qtde.ToString().Replace(",", ".") +
               "," + gorduras_totais_vlr_diario.ToString().Replace(",", ".") +
               "," + (gorduras_satu_nao ? "1" : "0") +
               "," + gorduras_satu_qtde.ToString().Replace(",", ".") +
               "," + gorduras_satu_vlr_diario.ToString().Replace(",", ".") +
               "," + (gorduras_trans_nao ? "1" : "0") +
               "," + gorduras_trans_qtde.ToString().Replace(",", ".") +
               "," + (fibra_alimen_nao ? "1" : "0") +
               "," + fibra_alimen_qtde.ToString().Replace(",", ".") +
               "," + fibra_alimen_vlr_diario.ToString().Replace(",", ".") +
               "," + (sodio_nao ? "1" : "0") +
               "," + sodio_qtde.ToString().Replace(",", ".") +
               "," + sodio_vlr_diario.ToString().Replace(",", ".") +
               ",'" + strEcommerce + "'" +
               ",'" + Descricao_Comercial + "'" +
               "," + (IntegraWS ? "1" : "0") +
               "," + (Ativo_Ecommerce ? "1" : "0") +
                ",'" + (indEscala ? "S" : "N") + "'" +
               ",'" + cnpjFabricante + "'" +
               ",'" + cBenef + "'" +
               "," + (Prato_dia ? "1" : "0") +
               "," + (Prato_dia_1 ? "1" : "0") +
               "," + (Prato_dia_2 ? "1" : "0") +
               "," + (Prato_dia_3 ? "1" : "0") +
               "," + (Prato_dia_4 ? "1" : "0") +
               "," + (Prato_dia_5 ? "1" : "0") +
               "," + (Prato_dia_6 ? "1" : "0") +
               "," + (Prato_dia_7 ? "1" : "0") +
                 ",'" + informacoes_extras + "'" +
               ",'" + pluReceita + "'" +
               ",'" + Und_producao + "'" +
               "," + (progSeg ? "1" : "0") +
               "," + (progTer ? "1" : "0") +
               "," + (progQua ? "1" : "0") +
               "," + (progQui ? "1" : "0") +
               "," + (progSex ? "1" : "0") +
               "," + (progSab ? "1" : "0") +
               "," + (progDom ? "1" : "0") +
               "," + Funcoes.decimalPonto(peso_receita_unitario.ToString()) +
               ",'" + filial_produzido + "'" +
               ",'" + tipo_producao + "'" +
               "," + ativa_ce.ToString() +
               ",'" + codigo_departamento_ce + "'" +
               ",'" + CFOP + "'" +
               ",'" + Und_producao + "'" +
               ",'" + descricao_producao + "'" +
               "," + embalagem_producao.ToString() +
               "," + Funcoes.decimalPonto(custo_producao.ToString()) +
               "," + pontos_fidelizacao.ToString() +
               "," + (Excluir_proxima_integracao ? "1" : "0") +
               ",'" + id_Ecommercer + "'" +
               "," + bandeja.ToString() +
               ",'" + Categoria + "'" +
               ",'" + Seguimento + "'" +
               ",'" + SubSeguimento + "'" +
               ",'" + GrupoCategoria + "'" +
               ",'" + SubGrupoCategoria + "'" +
               ",'" + usuarioAlteracao + "'" +
               ",'" + Categoria_eCommerce + "'" +
               ", " + Funcoes.decimalPonto(altura.ToString()) +
               ", " + Funcoes.decimalPonto(largura.ToString()) +
               ", " + Funcoes.decimalPonto(profundidade.ToString()) +
               "," + (impAux ? "1" : "0") +
               "," + (vendaComSenha ? "1" : "0") +
               ", '" + PLU_Vinculado + "'" +
               ", " + Funcoes.decimalPonto(fatorEstoqueVinculado.ToString()) +
               ", '" + descricaoWEB + "'" +
               ", '" + SKU + "'" +
               ", " + (Configuravel ? "1" : "0") +
               ", " + Funcoes.decimalPonto(margem_terceiro_preco) +
               ", " + Funcoes.decimalPonto(terceiro_preco) +
               ", '" + codigo_produto_ANVISA + "'" +
               ", '" + motivo_isencao_ANVISA + "'" +
               ", " + Funcoes.decimalPonto(preco_maximo_ANVISA.ToString()) +
               ", '" + codigoEmissaoNFe + "'" +
               "," + (colesterol_nao ? "1" : "0") +
               "," + colesterol_qtde.ToString().Replace(",", ".") +
               "," + colesterol_vlr_diario.ToString().Replace(",", ".") +
               "," + (calcio_nao ? "1" : "0") +
               "," + calcio_qtde.ToString().Replace(",", ".") +
               "," + calcio_vlr_diario.ToString().Replace(",", ".") +
               "," + (ferro_nao ? "1" : "0") +
               "," + ferro_qtde.ToString().Replace(",", ".") +
               "," + ferro_vlr_diario.ToString().Replace(",", ".") +
             ")";
            Conexao.executarSql(sql + strValues, conn, tran);
            Conexao.executarSql("update plu set usado=1 where plu = '" + PLU + "'", conn, tran);

        }


        private void atualizarEans(SqlConnection conn, SqlTransaction tran)
        {
            Conexao.executarSql("delete from ean where plu=" + PLU, conn, tran);
            if (ean != null)
            {
                foreach (DataRow item in ean.Rows)
                {
                    if (!item[0].ToString().Equals("------"))
                    {
                        Conexao.executarSql("insert into ean (PLU,ean,filial) values('" + PLU + "','" + item[0].ToString() + "','" + usr.getFilial() + "')", conn, tran);
                    }
                }
            }

        }
        private void atualizarTabelapreco(SqlConnection conn, SqlTransaction tran)
        {
            Conexao.executarSql("delete from Preco_Mercadoria where plu=" + PLU + " and filial='" + Filial + "'", conn, tran);
            if (ean != null)
            {
                foreach (preco_mercadoriaDAO preco in arrPrecosPromocionais)
                {
                    preco.PLU = PLU;
                    preco.Filial = Filial;
                    preco.salvar(true, conn, tran);

                }
            }

        }

        private void atualizarPrecoLojas(SqlConnection conn, SqlTransaction tran)
        {
            /*
            String sqlLoja = "update mercadoria_loja set preco =" + Preco.ToString().Replace(",", ".") +
                                ", preco_custo=" + Preco_Custo.ToString().Replace(",", ".") +
                                ", margem=" + Margem.ToString().Replace(",", ".") +
                                ", Preco_promocao=" + Preco_promocao.ToString().Replace(",", ".") +
                                ", Promocao_automatica=" + (Promocao_automatica ? 1 : 0) +
                                ", Promocao=" + (Promocao ? 1 : 0) +
                                ", data_inicio=" + (data_inicio.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_inicio.ToString("yyyy-MM-dd") + "'") +
                                ", data_fim=" + (data_fim.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_fim.ToString("yyyy-MM-dd") + "'") +

                                " where plu ='" + PLU + "' and Filial='" + usr.getFilial() + "'";

            //Conexao.executarSql(sqlLoja);
            //carregarPrecoLoja();
            */

            if (mercadoriasLoja != null)
            {
                foreach (mercadoria_lojaDAO item in mercadoriasLoja)
                {
                    if (item.Filial.Trim().Equals(usr.getFilial().Trim()))
                    {
                        item.Preco = this.Preco;
                        item.Preco_Custo = this.Preco_Custo;
                        item.Margem = this.Margem;
                        item.Preco_Promocao = this.Preco_promocao;
                        item.Promocao_automatica = this.Promocao_automatica;
                        item.Promocao = this.Promocao;
                        item.Data_Inicio = this.data_inicio;
                        item.Data_Fim = this.data_fim;
                        item.qtde_atacado = this.qtde_atacado;
                        item.margem_atacado = this.margem_atacado;
                        item.preco_atacado = this.preco_atacado;

                    }
                    item.codigo_familia = Codigo_familia;
                    item.salvar(false, conn, tran);
                }


            }
        }
        private void atualizarItens(SqlConnection conn, SqlTransaction tran)
        {


            Conexao.executarSql("Delete from item where plu='" + PLU + "'", conn, tran);
            foreach (itemDAO item in arrItens)
            {
                item.PLU = this.PLU;
                item.salvar(true, conn, tran);
            }


        }
        private void atualizarObs(SqlConnection conn, SqlTransaction tran)
        {
            Conexao.executarSql("delete from mercadoria_obs where plu='" + this.PLU + "'", conn, tran);
            if (arrMercadoriaObs.Count > 0)
            {
                foreach (mercadoria_obsDAO obs in arrMercadoriaObs)
                {
                    obs.plu = this.PLU;
                    obs.salvar(true, conn, tran);

                }
            }
        }

        private void carregarImpressoras()
        {
            String sql = "  Select filial.Loja, filial.Filial , si.impressora_remota, si.Descricao, sli.Observacao " +
                           " from Spool_Loja_Impressoras as sli " +
                               " inner join filial  on sli.Loja  = Filial.loja " +
                               " inner join Spool_impressoras as si on sli.Impressora_Remota = si.impressora_remota " +
                          "  where sli.plu ='" + PLU + "'";
            SqlDataReader rs = null;

            try
            {
                rs = Conexao.consulta(sql, null, false);
                arrImpressoras.Clear();
                while (rs.Read())
                {
                    ArrayList item = new ArrayList();
                    item.Add(rs["loja"].ToString());
                    item.Add(rs["filial"].ToString());
                    item.Add(rs["impressora_remota"].ToString());
                    item.Add(rs["descricao"].ToString());
                    item.Add(rs["observacao"].ToString());
                    arrImpressoras.Add(item);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public DataTable tbImpressoras()
        {
            ArrayList arrTb = new ArrayList();
            ArrayList cabecalho = new ArrayList();
            cabecalho.Add("LOJA");
            cabecalho.Add("FILIAL");
            cabecalho.Add("IMPRESSORA_REMOTA");
            cabecalho.Add("DESCRICAO");
            cabecalho.Add("OBSERVACAO");

            arrTb.Add(cabecalho);

            foreach (ArrayList item in arrImpressoras)
            {
                arrTb.Add(item);
            }

            return Conexao.GetArryTable(arrTb);

        }


        private void atualizarImpressoras(SqlConnection conn, SqlTransaction tran)
        {
            Conexao.executarSql("delete from Spool_Loja_Impressoras where plu='" + PLU + "'", conn, tran);

            if (arrImpressoras.Count > 0)
            {
                if (ean != null)
                {
                    foreach (ArrayList item in arrImpressoras)
                    {
                        String strSql = "insert into Spool_loja_impressoras (loja,plu, impressora_remota,observacao)" +
                                    " values(" + item[0].ToString() + ",'" + PLU + "'," + item[2].ToString() + ",'" + item[4].ToString() + "')";

                        Conexao.executarSql(strSql, conn, tran);

                    }
                }
            }

        }


        public static Decimal valorPisEntrada(String plu)
        {
            Decimal vlr = 0;
            String strVlr = Conexao.retornaUmValor("Select pis_perc_entrada from mercadoria where plu ='" + plu + "'", null);
            Decimal.TryParse(strVlr, out vlr);

            return vlr;
        }

        public static Decimal valorPisSaida(String plu)
        {
            Decimal vlr = 0;
            String strVlr = Conexao.retornaUmValor("Select pis_perc_saida from mercadoria where plu ='" + plu + "'", null);
            Decimal.TryParse(strVlr, out vlr);

            return vlr;
        }

        public static Decimal valorCofinsEntrada(String plu)
        {
            Decimal vlr = 0;
            String strVlr = Conexao.retornaUmValor("Select cofins_perc_entrada from mercadoria where plu ='" + plu + "'", null);
            Decimal.TryParse(strVlr, out vlr);

            return vlr;
        }

        public static Decimal valorCofinsSaida(String plu)
        {
            Decimal vlr = 0;
            String strVlr = Conexao.retornaUmValor("Select cofins_perc_saida from mercadoria where plu ='" + plu + "'", null);
            Decimal.TryParse(strVlr, out vlr);

            return vlr;
        }

        public static Decimal CSTPisCofinsEntrada(String plu)
        {
            Decimal vlr = 0;
            String strVlr = Conexao.retornaUmValor("Select cst_entrada from mercadoria where plu ='" + plu + "'", null);
            Decimal.TryParse(strVlr, out vlr);

            return vlr;
        }

        public static Decimal CSTPisCofinsSaida(String plu)
        {
            Decimal vlr = 0;
            String strVlr = Conexao.retornaUmValor("Select cst_saida from mercadoria where plu ='" + plu + "'", null);
            Decimal.TryParse(strVlr, out vlr);

            return vlr;
        }

        private void carregarMagentoAtributos()
        {
            if (magentoAtributos == null)
            {
                magentoAtributos = new mercadoria_AtributosMagentoDAO(this.PLU, usr);
            }

        }

        private void atualizarAtributos(SqlConnection conn, SqlTransaction tran)
        {
            Conexao.executarSql("DELETE FROM  Mercadoria_Magento_Atributos WHERE plu='" + PLU + "'", conn, tran);
            if (magentoAtributos != null)
            {
                magentoAtributos.salvar(true, conn, tran);
            }
        }

    }
    public class MercadoriaTributario
    {
        public string CSTPIS = "";
        public string CST = "";
        public MercadoriaTributario(string plu)
        {
            using (SqlConnection conn = Conexao.novaConexao())
            {
                string sql = "SELECT TOP 1 ISNULL(CST_Entrada,'') AS CSTPIS, ISNULL(CONVERT(VARCHAR(1), m.Origem) + t.Indice_ST,'') AS CST FROM ";
                sql += "Mercadoria M INNER JOIN Tributacao T on t.Codigo_Tributacao = m.codigo_tributacao_ent WHERE PLU = '" + plu + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        CSTPIS = reader["cstpis"].ToString();
                        CST = reader["cst"].ToString();
                    }
                }
            }
        }
    }
}
