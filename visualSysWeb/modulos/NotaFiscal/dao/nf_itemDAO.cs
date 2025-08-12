using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using visualSysWeb.code;
using visualSysWeb.modulos.NotaFiscal.code;

namespace visualSysWeb.dao
{
    public class nf_itemDAO
    {
        //Campos para novo cadastro Entrada
        public String Codigo_tributacao_novo { get; set; } = "";
        public String Descricao_tributacao_novo { get; set; } = "";
        public String cstPisCofins_novo { get; set; } = "";
        public String pis_novo { get; set; } = "";
        public String cofins_novo { get; set; } = "";

        public String Codigo_departamento_novo { get; set; } = "";
        public String Descricao_departamento_novo { get; set; } = "";
        public String Codigo_subGrupo_novo { get; set; } = "";
        public String Descricao_subGrupo_novo { get; set; } = "";
        public String Codigo_grupo_novo { get; set; } = "";
        public String Descricao_grupo_novo { get; set; } = "";
        public String tipo_novo { get; set; } = "";

        public String Filial { get; set; }
        public String Codigo { get; set; }
        public String crt { get; set; }
        public int serie { get; set; }
        public String Ordem_compra { get; set; } = "";
        public String Cliente_Fornecedor { get; set; }
        public fornecedorDAO objFornecedor { get; set; }
        public String Tipo_NF { get; set; }
        public String PLU { get; set; }
        public bool recalcular = true;
        public int finNFe = 1;
        public DateTime Data_validade = new DateTime();
        public string codigo_centro_custo { get; set; } = "";
        public string descricao_centro_custo { get; set; } = "";

        //Nota Técnica 2021.004v1.21 - Elemento MED NCM´S 3001, 3002, 3003, 3004, 3005 e 3006
        public string codigo_produto_ANVISA = "";
        public string motivo_isencao_ANVISA = "";
        public decimal preco_Maximo_ANVISA = 0;

        public string codigoEmissaoNFe = "";
        public string pedidoItemNumero = "";
        public string pedidoItemSequencia = "";

        //Valor do DIFAL do item

        public Decimal valor_Difal_Item { get; set; }
        public Decimal aliquota_ICMS_Destino = 0;

        //Recebimento pelo Smartphone/navegador/
        public decimal Qtde_Devolver = 0;

        public String Data_validadeBr
        {
            get
            {
                return dataBr(Data_validade);
            }
        }
        private tributacaoDAO trib = null;
        public tributacaoDAO tributacao
        {
            get
            {
                if ((!StrCodTributacao.Equals("") && trib == null) || !StrCodTributacao.Equals(trib.Codigo_Tributacao.ToString()))
                {
                    trib = new tributacaoDAO(StrCodTributacao, usr);

                }
                return trib;
            }
            set { }
        }
        public String vIndiceSt = "";
        public String indice_St
        {
            get
            {
                if (finNFe == 2)
                {
                    return vIndiceSt;
                }
                if (!vIndiceSt.Equals(""))
                {
                    return vIndiceSt;
                }
                else
                {
                    if (!usr.filial.Reg_Federal.ToUpper().Equals("SIMPLES NACIONAL"))
                    {
                        if (tributacao.Indice_ST == null)
                            return "";
                        else
                            return tributacao.Indice_ST;
                    }
                    else
                    {
                        if (tributacao.csosn == null)
                            return "";
                        else
                            return tributacao.csosn;

                    }
                }
            }
            set
            {
                vIndiceSt = value;
            }
        }
        private String StrCodTributacao = "";
        public Decimal Codigo_Tributacao
        {
            get
            {
                if (!StrCodTributacao.Equals(""))
                    return Decimal.Parse(StrCodTributacao);
                else
                    return 0;
            }
            set
            {
                StrCodTributacao = value.ToString();
            }
        }
        public Decimal vQtde = 0;
        public Decimal Qtde
        {
            get
            {
                return vQtde;
            }
            set
            {
                if (vtotal != 0)
                {
                    Unitario = (vtotal_produto / (value * Embalagem));
                }
                vQtde = value;
            }
        }
        private Decimal vEmbalagem = 0;
        public Decimal Embalagem
        {
            get
            {
                return vEmbalagem;
            }
            set
            {
                if (value != 0)
                {

                    if (vtotal != 0)
                    {
                        Unitario = (vtotal_produto / (Qtde * value));
                    }
                }
                vEmbalagem = value;
            }
        }
        public Decimal Unitario = 0;
        public Decimal Desconto = 0;
        public Decimal vDesconto_valor { get; set; } = 0;

        public Decimal DescontoValor
        {
            get
            {
                if (Desconto == 0)
                {
                    vDesconto_valor = 0;
                    return 0;
                }
                else
                    vDesconto_valor = (vtotal_produto * Desconto) / 100;

                return vDesconto_valor;
            }
            set
            {
                if (value <= 0)
                {
                    vDesconto_valor = 0;
                    Desconto = 0;
                }
                else
                {
                    vDesconto_valor = value;
                    Desconto = (value / vtotal_produto) * 100;
                }

            }
        }
        public Decimal vTotTrib = 0;
        public Decimal TotTrib
        {
            get
            {
                Decimal vTot = 0;

                if (naturezaOperacao != null && naturezaOperacao.Saida)
                {
                    String strValor = Conexao.retornaUmValor("Select ISNULL(aliquota_imposto,0) from Imposto_Nota where NCM = '" + NCM.Replace(".", "").Trim() + "'", null);
                    if (!strValor.Equals(""))
                    {
                        Decimal porcTrib = Decimal.Parse(strValor);
                        vTotTrib = Decimal.Round((Total * porcTrib) / 100, 2);
                        vTot = vTotTrib;
                    }

                }
                return vTot;

            }
        }
        public Decimal vtotal_produto { get; set; }
        public Decimal TotalProduto()
        {
            return TotalProduto(false);
        }
        public Decimal TotalProduto(bool round)
        {
            if (round)
                vtotal_produto = Decimal.Round((Qtde * vEmbalagem) * Unitario, 2);
            else
                vtotal_produto = (Qtde * vEmbalagem) * Unitario;
            return vtotal_produto;
        }
        public Decimal vtotal = 0;
        public Decimal Total
        {
            get
            {


                vtotal = (vtotal_produto - DescontoValor) + despesas + Frete + (naturezaOperacao != null && !naturezaOperacao.Incide_IPI ? 0 : vIpiv) + vIva;
                return vtotal;

            }
            set
            {
                vtotal = value;
            }
        }
        public Decimal TotalRound()
        {
            vtotal = Decimal.Round((vtotal_produto - DescontoValor) + despesas + Frete + (naturezaOperacao != null && !naturezaOperacao.Incide_IPI ? 0 : vIpiv) + vIva, 2);
            return vtotal;
        }
        public Decimal TotalCustoUnitario
        {
            get
            {
                if (Total != 0)
                {
                    return (((vtotal_produto - DescontoValor) + despesas + vFCPST + Frete + (naturezaOperacao != null && !naturezaOperacao.Incide_IPI ? 0 : vIpiv) + vIva) / (Qtde * Embalagem));
                }
                else
                    return 0;
            }
        }
        public Decimal TotalCusto
        {
            get
            {
                return (vtotal_produto - DescontoValor) + despesas + vIpiv + vIva + Frete;
            }
            set { }
        }
        public Decimal porcIPI = 0;
        public Decimal IPI
        {
            get
            {
                if (naturezaOperacao != null && !naturezaOperacao.Incide_IPI)
                {
                    porcIPI = 0;
                    return 0;
                }

                return porcIPI;
            }
            set
            {
                Decimal vIpi = 0;
                if (value != 0)
                {
                    //(item.Total * (item.porcIPI) / 100)
                    vIpi = ((vtotal_produto - DescontoValor) * value) / 100;
                }
                if (!vIpi.Equals(vIpiv))
                {
                    vIpiv = vIpi;
                }

                porcIPI = value;

            }
        }
        public Decimal vIpiv = 0;
        public Decimal IPIV
        {
            get
            {
                if (finNFe == 2)
                {
                    return vIpiv;
                }
                if (naturezaOperacao != null && !naturezaOperacao.Incide_IPI)
                {
                    vIpiv = 0;
                    return 0;
                }

                baseIpi = (vtotal_produto - DescontoValor);
                if (baseIpi > 0 && porcIPI > 0)
                {
                    vIpiv = ((baseIpi) * porcIPI) / 100;
                }
                else
                {
                    vIpiv = 0;
                }

                return vIpiv;
            }
            set
            {
                if (naturezaOperacao != null && naturezaOperacao.Incide_IPI)
                {
                    Decimal pIPI = 0;
                    if (value != 0)
                    {
                        baseIpi = (vtotal_produto - DescontoValor);
                        if (value > 0 && baseIpi > 0)
                        {
                            pIPI = (value / (baseIpi)) * 100;
                        }
                        else
                        {
                            pIPI = 0;
                        }
                    }
                    if (!pIPI.Equals(porcIPI))
                    {
                        porcIPI = pIPI;

                    }
                    vIpiv = value;
                }

            }
        }

        public Decimal baseIpi = 0;
        public Decimal calculoIpi()
        {
            baseIpi = (vtotal_produto - DescontoValor);
            vIpiv = IPIV;
            return baseIpi;
        }
        private String strDescricao = "";
        public String DescricaoNotaFiscalEntrada { get; set; } = "";
        public String Descricao
        {
            get
            {
                if (finNFe != 2)
                {
                    if (!PLU.Equals("") && strDescricao.Equals(""))
                    {
                        strDescricao = Conexao.retornaUmValor("Select descricao from mercadoria where plu='" + PLU + "'", new User()).Replace("'", "");

                    }
                }

                return strDescricao;
            }
            set
            {
                if (value.Length > 200)
                    strDescricao = Funcoes.RemoverAcentos(value.Substring(0, 200).Replace("'", ""));
                else
                    strDescricao = Funcoes.RemoverAcentos(value.Replace("'", ""));
            }
        }
        private String strEan = "";
        public String ean
        {
            get
            {
                if (!PLU.Equals("") && strEan.Equals(""))
                    strEan = Conexao.retornaUmValor("Select ean from ean where plu='" + PLU + "'", new User());



                return strEan;
            }
            set
            {
                strEan = value;
            }
        }
        public Decimal vIva = 0;
        public Decimal vAliquota_iva = 0;

        public Decimal aliquota_iva
        {

            get
            {
                if (finNFe == 2)
                {
                    return vAliquota_iva;
                }
                if (tributacao != null && (trib.Incide_ICM_Subistituicao || (vmargemIva > 0 && finNFe == 4)))
                {
                    if (vmargemIva == 0 && vIva == 0)
                        return 0;
                    else if (vAliquota_iva == 0)
                        return aliquota_icms;
                    else
                        return vAliquota_iva;
                }
                else
                {
                    vAliquota_iva = 0;
                    return 0;
                }

            }
        }
        public Decimal CalculoIva()
        {
            //Checa se há calculo do DIFAL PERMITIDO
            if (this.naturezaOperacao != null)
            {
                if (this.naturezaOperacao.Difal)
                {
                    if (difalContribuinte)
                    {
                        valor_Difal_Item = CalculoDifal;
                    }
                }
            }

            if (finNFe == 2)
            {
                if (Tipo_NF.Equals("2"))
                {
                    vIva = 0;
                }
                return vIva;
            }
            if (finNFe ==2 && Tipo_NF.Equals("2") && indFinal == 1)
            {
                vPIva = 0;
                vBaseIva = 0;
                vIva = 0;
                return 0;

            }
            else
            {
                if (tributacao != null && (trib.Incide_ICM_Subistituicao || (vmargemIva > 0 && finNFe == 4)))
                {
                    if (aliquota_iva != 0 && vBaseICMS != 0)
                    {

                        vPIva = CalculoPIva;
                        vBaseIva = CalculoBase_iva;

                        Decimal vcontiva = ((vBaseIva * aliquota_iva) / 100) - ((vBaseICMS * aliquota_icms) / 100);
                        if (vcontiva < 0)
                        {
                            vIva = 0;
                            return 0;
                        }
                        else
                        {
                            vIva = vcontiva;
                            return vcontiva;
                        }
                    }
                    else
                    {
                        vPIva = 0;
                        vBaseIva = 0;
                        vIva = 0;
                        return 0;
                    }

                }
                else
                {
                    vPIva = 0;
                    vBaseIva = 0;
                    vIva = 0;
                    return 0;
                }

            }
        }
        public Decimal vBaseIva = 0;
        public Decimal CalculoBase_iva
        { // base icms st ?
            get
            {
                if (finNFe == 2)
                {
                    return vBaseIva;
                }
                if (tributacao != null && (trib.Incide_ICM_Subistituicao || (vmargemIva > 0 && finNFe == 4)))
                {
                    if (vmargemIva != 0)
                    {
                        //Alteração efetuada em 08/10/2015 - Jailson
                        //Caso a operação seja SAIDA (CFOP > 4999)
                        //o sistema considera o redutor para calculo da Base
                        if (codigo_operacao > 4999 || (Tipo_NF.Equals("2") && redutor_base_ST > 0))  //Alterado pois o redutor não influencia na base de calculo do IVA
                        {
                            vBaseIva = (((((vtotal_produto - DescontoValor) - (((vtotal_produto - DescontoValor) * redutor_base_ST) / 100)) + IPIV + despesas + (indFinal == 1 ? Frete : 0))) + vPIva);
                        }
                        else
                        {
                            vBaseIva = ((((vtotal_produto - DescontoValor) + IPIV + despesas + (indFinal == 1 ? Frete : 0))) + vPIva);
                        }
                        return vBaseIva;
                    }
                    else
                    {
                        vBaseIva = 0;
                        return 0;
                    }
                }
                else
                {
                    vBaseIva = 0;
                    return 0;
                }


            }
            /*
              set
              {
                  vBaseIva = value;
                  if (recalcular)
                  {
                      if (value == 0 || baseICMS == 0)
                          iva = 0;
                      else
                      {
                          if ((((value * aliquota_iva) / 100) - (baseICMS * aliquota_icms) / 100) != iva)
                              iva = ((value * aliquota_iva) / 100) - ((baseICMS * aliquota_icms) / 100);
                      }
                  }
              }
             */
        }
        private Decimal vPIva = 0;
        private Decimal CalculoPIva
        {
            get
            {

                if (finNFe == 2)
                {
                    return vPIva;
                }
                if (tributacao != null && (trib.Incide_ICM_Subistituicao || (vmargemIva > 0 && finNFe == 4)))
                {
                    Decimal vBase;
                    //Alteração efetuada em 08/10/2015 - Jailson
                    //Caso a operação seja SAIDA (CFOP > 4999)
                    //o sistema considera o redutor para calculo da Base
                    if (codigo_operacao > 4999 || redutor_base_ST > 0)
                    {
                        vBase = (((vtotal_produto - DescontoValor) - (((vtotal_produto - DescontoValor) * redutor_base_ST) / 100)) + IPIV + despesas + (indFinal == 1 ? Frete : 0));
                    }
                    else
                    {
                        vBase = ((vtotal_produto - DescontoValor) + IPIV + despesas + (indFinal == 1 ? Frete : 0));
                    }

                    if (vmargemIva <= 0)
                    {
                        vPIva = 0;
                        return 0;
                    }
                    else
                    {
                        vPIva = (vBase * vmargemIva) / 100;
                        return vPIva;
                    }
                }
                else
                {
                    vPIva = 0;
                    return 0;
                }
            }
        }
        public Decimal vmargemIva = 0;
        public Decimal CalculoMargem_iva
        {
            get
            {
                if (finNFe == 2)
                {
                    return vmargemIva;
                }
                if (tributacao != null && trib.Incide_ICM_Subistituicao)
                {
                    if (vIva > 0 && vBaseICMS > 0)
                    {
                        if (vBaseIva > 0)
                        {
                            Decimal valorBaseIva = vBaseIva;
                            Decimal valorProduto = ((vtotal_produto - DescontoValor) + vIpiv + despesas + (indFinal == 1 ? Frete : 0));
                            Decimal valorPorcIva = valorBaseIva - valorProduto;
                            vmargemIva = (valorPorcIva / valorProduto) * 100;
                        }
                        else
                        {
                            Decimal valoricmsiva = (vIva + ((vBaseICMS * aliquota_icms) / 100));
                            Decimal valorBaseIva = (valoricmsiva / aliquota_iva) * 100;
                            Decimal valorProduto = ((vtotal_produto - DescontoValor) + vIpiv + despesas + (indFinal == 1 ? Frete : 0));
                            Decimal valorPorcIva = valorBaseIva - valorProduto;
                            vmargemIva = (valorPorcIva / valorProduto) * 100;
                        }
                        return vmargemIva;
                    }
                    else
                    {
                        vmargemIva = 0;
                        return 0;
                    }
                }
                else
                {
                    vmargemIva = 0;
                    return 0;
                }
            }
            /*
            set
            {
                vmargemIva = value;
                if (recalcular)
                {

                    if (value == 0 || baseICMS == 0)
                    {
                        if (iva != 0)
                        {
                            iva = 0;
                        }
                    }
                    else
                    {
                        Decimal vBase = ((TotalProduto - DescontoValor) + IPIV + despesas);
                        Decimal vPIva = (vBase * value) / 100;
                        if (!vPIva.Equals(this.vPIva))
                        {
                            this.vPIva = vPIva;
                            Decimal vIva = ((base_iva * aliquota_iva) / 100) - ((baseICMS * aliquota_icms) / 100);
                            if (this.vIva != vIva)
                            {
                                this.vIva = vIva;
                            }
                        }
                    }
                }
             
            }
             * * */
        } //perguntar jailson
        public Decimal despesas = 0;
        public Decimal CFOP { get; set; }
        public String CODIGO_REFERENCIA { get; set; }
        public Decimal vAliquota_icms = 0;

        public Decimal aliquota_icms
        {

            get
            {
                if (finNFe == 2)
                {
                    return vAliquota_icms;
                }
                if (naturezaOperacao != null && !naturezaOperacao.Incide_ICMS)
                {
                    return 0;
                }
                else
                {
                    return vAliquota_icms;
                }
            }
            set
            {
                vAliquota_icms = value;
            }
        }
        public Decimal vBaseICMS = 0;
        public Decimal baseICMS()
        {
            if (finNFe == 2)
            {
                return vBaseICMS;
            }
            if ((naturezaOperacao != null && !naturezaOperacao.Incide_ICMS) || (crt != null && crt.Equals("1")))
            {
                vBaseICMS = 0;
                return 0;
            }

            if (aliquota_icms > 0)
            {
                vBaseICMS = (vtotal_produto - DescontoValor) + (indFinal == 1 ? Frete : 0);
                if (objFornecedor != null)
                {
                    if (objFornecedor.vIpi_base)
                    {
                        vBaseICMS += IPIV;
                    }

                }

                if (vBaseICMS > 0 && redutor_base != 0)
                {
                    vBaseICMS = (vBaseICMS - decimal.Round(((vBaseICMS * redutor_base) / 100), 4));
                    //vBaseICMS = (vBaseICMS - decimal.Round(((vPBasePisCofins * redutor_base) / 100), 4));
                }

                if (objFornecedor != null)
                {
                    if (objFornecedor.vDespesas_base)
                    {
                        vBaseICMS += despesas;
                    }
                }
                return vBaseICMS;
            }
            else
            {
                vBaseICMS = 0;
                return 0;
            }
        }
        public Decimal vicms = 0;
        public Decimal valorIcms()
        {
            if (finNFe == 2)
            {
                return vicms;
            }
            if ((naturezaOperacao != null && !naturezaOperacao.Incide_ICMS) || (crt != null && crt.Equals("1")))
            {
                vicms = 0;
                return 0;
            }
            if (vBaseICMS == 0 || aliquota_icms == 0)
            {
                vicms = 0;
                return 0;
            }
            else
            {
                vicms = (vBaseICMS * aliquota_icms) / 100;
                return vicms;
            }
        }
        public Decimal redutor_base = 0;
        public Decimal redutor_base_ST = 0;
        public Decimal codigo_operacao = 0;
        public Decimal Frete = 0;

        internal void buscaCentroCusto()
        {
            if (codigo_centro_custo.Equals("") && !PLU.Equals(""))
            {

                SqlDataReader rs = null;
                try
                {
                    rs = Conexao.consulta("Select m.codigo_centro_custo, c.descricao_centro_custo " +
                                        " from mercadoria as m inner join centro_custo as c on m.codigo_centro_custo = c.codigo_centro_custo " +
                                        " where m.plu='" + PLU + "'", usr, false);
                    if (rs.Read())
                    {
                        codigo_centro_custo = rs["codigo_centro_custo"].ToString();
                        descricao_centro_custo = rs["descricao_centro_custo"].ToString();
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

        public int Num_item { get; set; }
        private Decimal vPBasePisCofins = -1;
        public Decimal vBASEPisCofins
        {
            get
            {
                if (vPBasePisCofins <= 0)
                {
                    //Qdo existir o Frete, o valor deverá compor a base de cálculo.
                    //** a partir de 01/05/2023 o valor do ICMS deverá ser excluído da base de pis e cofins
                    //vPBasePisCofins = vtotal_produto - DescontoValor  + (indFinal == 1 ? Frete : 0);
                    vPBasePisCofins = (vtotal_produto - DescontoValor) - vicms; // + (indFinal == 1 ? Frete : 0); Retirada do Frete na BASE
                }
                else
                {
                    decimal valorICMSFrete = 0;
                    if (indFinal ==1 && Frete > 0 && vicms > 0)
                    {
                        valorICMSFrete = Math.Round(Frete * (aliquota_icms / 100), 2); 
                    }
                    vPBasePisCofins = (vtotal_produto - DescontoValor) - vicms + valorICMSFrete; // + (indFinal == 1 ? Frete : 0);
                }
                return vPBasePisCofins;
            }
            set
            {
                vPBasePisCofins = value;
            }
        }
        public Decimal PISp = 0;
        public Decimal PISV = 0;
        public Decimal COFINSp = 0;
        public Decimal COFINSV = 0;
        public String NCM { get; set; } = "";
        public String CEST = "";
        public String Und { get; set; }
        public String Artigo { get; set; }
        public Decimal Peso_liquido { get; set; }
        public Decimal Peso_Bruto { get; set; }
        public String Tipo { get; set; }
        public cfDAO objCf = null;
        public String CF
        {
            get
            {
                if (objCf == null)
                {
                    return "";
                }
                else
                {
                    return objCf.cf;
                }
            }
            set
            {
                objCf = new cfDAO(value, usr);
            }
        }
        private String vCST_IPI = "";
        public String CST_IPI
        {
            get
            {
                if (naturezaOperacao != null || naturezaOperacao.Incide_IPI)
                {
                    //if (vCST_IPI.Equals(""))&& !naturezaOperacao.cenq_ipi.Equals(""))
                    //{
                    //    String sqlCSTipi = "";
                    //    if (Tipo_NF.Equals("1"))
                    //    {
                    //        sqlCSTipi = "Select CST_SAIDA from cst_cenq WHERE codigo='" + naturezaOperacao.cenq_ipi + "'";
                    //    }
                    //    else
                    //    {
                    //        sqlCSTipi = "Select CST_ENTRADA from cst_cenq WHERE codigo='" + naturezaOperacao.cenq_ipi + "'";
                    //    }
                    //    vCST_IPI = Conexao.retornaUmValor(sqlCSTipi, null);
                    //}

                    return vCST_IPI;
                }
                else
                {
                    if (vCST_IPI.Equals(""))
                    {
                        vCST_IPI = "52";
                    }
                    return vCST_IPI;
                }
            }
            set
            {
                vCST_IPI = value;
            }
        }
        private String vCSTPIS = "";
        public String CSTPIS
        {
            get
            {
                if (naturezaOperacao == null || naturezaOperacao.incide_PisCofins)
                {
                    return vCSTPIS;
                }
                else
                {
                    if (naturezaOperacao.Saida)
                    {
                        return "08";
                    }
                    else
                    {
                        return "74";
                    }
                }
            }
            set
            {
                vCSTPIS = value;

            }

        }
        private String vCSTCOFINS = "";
        public String CSTCOFINS
        {

            get
            {
                if (naturezaOperacao == null || naturezaOperacao.incide_PisCofins)
                {
                    return vCSTCOFINS;
                }
                else
                {
                    if (naturezaOperacao.Saida)
                    {
                        return "08";
                    }
                    else
                    {
                        return "74";
                    }
                }
            }
            set
            {
                if (naturezaOperacao == null || naturezaOperacao.incide_PisCofins)
                {
                    vCSTCOFINS = value;
                }
                else
                {
                    vCSTCOFINS = "99";
                }


            }
        }
        public User usr = new User();

        public Decimal pCredSN = 0;
        public Decimal vCredicmssn = 0;
        public Decimal vlrcredIcmssn
        {
            get
            {
                if (pCredSN == 0)
                {
                    vCredicmssn = 0;
                    return 0;
                }
                else
                {
                    Decimal vBase = (vtotal_produto - DescontoValor);
                    vCredicmssn = (vBase * pCredSN) / 100;
                    return vCredicmssn;
                }
            }
            set
            {
                vCredicmssn = 0;
            }

        }




        public natureza_operacaoDAO naturezaOperacao { get; set; }

        public int vOrigem = -1;
        public bool inativo
        {
            get
            {
                return Conexao.retornaUmValor("select inativo from mercadoria where plu='" + PLU + "'", null).Equals("1");
            }
            set { }
        }
        public String origem
        {
            get
            {
                if (vOrigem == -1)
                    return Conexao.retornaUmValor("select isnull(origem,0) from mercadoria where plu='" + PLU + "'", null);
                else
                    return vOrigem.ToString();
            }

            set { }
        }


        //Nfe 4.0
        public bool indEscala = false;
        public string cnpj_Fabricante = "";
        public string cBenef = "";
        public Decimal vBCFCP = 0;
        public Decimal pFCP = 0;
        public Decimal vFCP = 0;
        public Decimal vBCFCPST = 0;
        public Decimal pFCPST = 0;
        public Decimal vFCPST = 0;
        public Decimal vIPIDevol = 0;
        public Decimal pDevol = 0;

        internal int indFinal;

        //Exclusivo para inclusão do DIFAL
        public bool difalContribuinte = false;
        public String ufCliente = "";
        public Decimal vDifal = 0;

        public Decimal CalculoDifal
        {
            get
            {
                UfPobrezaDAO uf = UfPobrezaDAO.objUFPobreza(ufCliente);
                aliquota_imp_estadoDAO impEstado = new aliquota_imp_estadoDAO(ufCliente, NCM, null);
                //vAliquota_iva = (uf.aliq_interna - vAliquota_icms);
                vAliquota_iva = (aliquota_ICMS_Destino - vAliquota_icms);
                decimal vAliqDifal = (vAliquota_iva / 100);
                if (uf.calc_Fora == 1)
                {
                    vBaseIva = vBaseICMS; // TotalProduto(true);
                    vIva = (vBaseIva * vAliqDifal);
                }
                else if (uf.calc_Fora == 0)
                {
                    //Decimal vIcms = (TotalProduto(true) * (vAliquota_icms / 100));
                    Decimal vIcms = (vBaseICMS * (vAliquota_icms / 100));
                    vBaseIva = (vBaseICMS - vIcms) / (1 - ((aliquota_ICMS_Destino + impEstado.porc_combate_pobresa) / 100));
                    vIva = (vBaseIva * (aliquota_ICMS_Destino / 100)) - vIcms;
                    
                    //Valor do DIFAL
                    vDifal = Math.Round(vIva, 2);
                }
                else
                {
                    if (uf.uf.Equals("GO"))
                    {
                        vBaseIva = (vBaseICMS) / (1 - (aliquota_ICMS_Destino / 100));
                    }
                    else if (uf.uf.Equals("SE"))
                    {
                        vBaseIva = (vBaseICMS) / (1 - vAliqDifal);
                    }
                    else if (uf.uf.Equals("AL"))
                    {
                        vBaseIva = (vBaseICMS / (1 - ((uf.aliq_interna + uf.porc) / 100)));
                    }
                    vIva = (vBaseIva * vAliqDifal);

                }

                //Neste ponto calculamos o Fundo de combate a pobreza
                calcularFCPST(ufCliente, vBaseIva);

                return Math.Round(vIva,2);
            }
        }




        public nf_itemDAO(User usr)
        {
            this.usr = usr;
            Filial = usr.getFilial();
        }

        public nf_itemDAO copia()
        {

            return (nf_itemDAO)this.MemberwiseClone();
        }


        public void calculaPisCofins()
        {
            if (naturezaOperacao != null)
            {
                if (vCSTPIS.Equals("") && !naturezaOperacao.cst_pis_cofins.Trim().Equals(""))
                {
                    vCSTPIS = naturezaOperacao.cst_pis_cofins;
                    vCSTCOFINS = naturezaOperacao.cst_pis_cofins;
                }
                if (naturezaOperacao.incide_PisCofins)
                {
                    if (Tipo_NF.Equals("1"))
                    {
                        int cst = Funcoes.intTry(vCSTCOFINS);
                        if (cst == 1 || cst == 2)
                        {
                            if (PISp <= 0)
                            {
                                PISp = usr.filial.pis;
                            }
                            if (COFINSp <= 0)
                            {
                                COFINSp = usr.filial.cofins;
                            }
                        }
                        else
                        {
                            PISp = 0;
                            COFINSp = 0;
                        }
                    }
                    else
                    {
                        if (PISp <= 0)
                        {
                            PISp = MercadoriaDAO.valorPisEntrada(this.PLU);
                        }
                        if (COFINSp <= 0)
                        {
                            COFINSp = MercadoriaDAO.valorCofinsEntrada(this.PLU);
                        }

                    }

                    //Checa se a BASE do PIS/COFINS é superior ao valor do produto
                    if (vBASEPisCofins > vtotal_produto)
                    {
                        vBASEPisCofins = vtotal_produto;
                    }

                    if (PISp > 0)
                    {
                        PISV = Decimal.Round((vBASEPisCofins * PISp) / 100, 2);
                    }
                    else
                    {
                        PISV = 0;
                    }
                    if (COFINSp > 0)
                    {
                        COFINSV = Decimal.Round((vBASEPisCofins * COFINSp) / 100, 2);
                    }
                    else
                    {
                        COFINSV = 0;
                    }
                }
                else
                {
                    PISp = 0;
                    COFINSp = 0;
                    PISV = 0;
                    COFINSV = 0;
                }
            }

        }


        public static List<nf_itemDAO> itens(String codigo, String tipoNf, int serie, String clienteFornecedor, natureza_operacaoDAO ntOperacao, bool nf_canc, User usr)
        {
            List<nf_itemDAO> ArrItens = new List<nf_itemDAO>();
            String sql = "Select *, c.descricao_centro_custo, ISNULL(me.Codigo_Emissao_NFe,'') AS Codigo_Emissao_NF from nf_item " +
                        " left join centro_custo as c on nf_item.codigo_centro_custo = c.codigo_centro_custo " +
                        " inner join Mercadoria me on me.plu = nf_item.plu " +
                         "where codigo ='" + codigo + "' " +
                         "  and tipo_nf = " + tipoNf +
                         "  and cliente_fornecedor = '" + clienteFornecedor + "' " +
                         "  and isnull(nf_canc,0)=" + (nf_canc ? '1' : '0') + " " +
                         "  and serie =" + serie.ToString() +
                         " order by Num_item ";
            SqlDataReader rs = Conexao.consulta(sql, usr, false);
            while (rs.Read())
            {
                nf_itemDAO nfItem = new nf_itemDAO(usr);
                nfItem.naturezaOperacao = ntOperacao;
                nfItem.Filial = rs["Filial"].ToString();
                nfItem.Codigo = rs["Codigo"].ToString();
                nfItem.Cliente_Fornecedor = rs["Cliente_Fornecedor"].ToString();
                nfItem.serie = Funcoes.intTry(rs["serie"].ToString());
                nfItem.Ordem_compra = rs["ordem_compra"].ToString();
                nfItem.Tipo_NF = rs["Tipo_NF"].ToString();
                nfItem.PLU = rs["PLU"].ToString();
                nfItem.Descricao = rs["Descricao"].ToString();
                nfItem.Codigo_Tributacao = (Decimal)(rs["Codigo_Tributacao"].ToString().Equals("") ? new Decimal() : rs["Codigo_Tributacao"]);
                nfItem.Qtde = (Decimal)(rs["Qtde"].ToString().Equals("") ? new Decimal() : rs["Qtde"]);
                nfItem.Embalagem = (Decimal)(rs["Embalagem"].ToString().Equals("") ? new Decimal() : rs["Embalagem"]);
                nfItem.Unitario = (Decimal)(rs["Unitario"].ToString().Equals("") ? new Decimal() : rs["Unitario"]);
                nfItem.Desconto = (Decimal)(rs["Desconto"].ToString().Equals("") ? new Decimal() : rs["Desconto"]);
                nfItem.vtotal_produto = Funcoes.decTry(rs["total_produto"].ToString());
                nfItem.Total = (Decimal)(rs["Total"].ToString().Equals("") ? new Decimal() : rs["Total"]);
                Decimal.TryParse(rs["base_ipi"].ToString(), out nfItem.baseIpi);
                nfItem.porcIPI = (Decimal)(rs["IPI"].ToString().Equals("") ? new Decimal() : rs["IPI"]);
                nfItem.vIpiv = (Decimal)(rs["IPIV"].ToString().Equals("") ? new Decimal() : rs["IPIV"]);
                nfItem.ean = rs["ean"].ToString();
                nfItem.aliquota_icms = (Decimal)(rs["aliquota_icms"].ToString().Equals("") ? new Decimal() : rs["aliquota_icms"]);
                nfItem.vAliquota_iva = (Decimal)(rs["aliquota_iva"].ToString().Equals("") ? new Decimal() : rs["aliquota_iva"]);
                nfItem.vIndiceSt = rs["indice_st"].ToString();
                nfItem.CFOP = (Decimal)(rs["CFOP"].ToString().Equals("") ? new Decimal() : rs["CFOP"]);
                nfItem.vBaseIva = (Decimal)(rs["base_iva"].ToString().Equals("") ? new Decimal() : rs["base_iva"]);
                nfItem.vIva = (Decimal)(rs["iva"].ToString().Equals("") ? new Decimal() : rs["iva"]);
                nfItem.vmargemIva = (Decimal)(rs["margem_iva"].ToString().Equals("") ? new Decimal() : rs["margem_iva"]);
                nfItem.despesas = (Decimal)(rs["despesas"].ToString().Equals("") ? new Decimal() : rs["despesas"]);
                nfItem.CODIGO_REFERENCIA = rs["CODIGO_REFERENCIA"].ToString();
                nfItem.redutor_base = (Decimal)(rs["redutor_base"].ToString().Equals("") ? new Decimal() : rs["redutor_base"]);
                nfItem.codigo_operacao = (Decimal)(rs["codigo_operacao"].ToString().Equals("") ? new Decimal() : rs["codigo_operacao"]);
                nfItem.Frete = (Decimal)(rs["Frete"].ToString().Equals("") ? new Decimal() : rs["Frete"]);
                nfItem.Num_item = (rs["Num_item"] == null ? 0 : int.Parse(rs["Num_item"].ToString()));

                //centro de custo
                nfItem.codigo_centro_custo = rs["codigo_centro_custo"].ToString();
                nfItem.descricao_centro_custo = rs["descricao_centro_custo"].ToString();

                Decimal.TryParse(rs["aliquota_pis"].ToString(), out nfItem.PISp);
                Decimal.TryParse(rs["aliquota_cofins"].ToString(), out nfItem.COFINSp);

                nfItem.PISV = (rs["PISV"].ToString().Equals("") ? new Decimal() : Decimal.Parse(rs["PISV"].ToString()));
                nfItem.COFINSV = (rs["COFINSV"].ToString().Equals("") ? new Decimal() : Decimal.Parse(rs["COFINSV"].ToString()));
                nfItem.NCM = (rs["NCM"] == null ? "null" : rs["NCM"].ToString());
                nfItem.CEST = (rs["CEST"].ToString());
                nfItem.Und = rs["Und"].ToString();
                nfItem.Artigo = rs["Artigo"].ToString();
                nfItem.Peso_liquido = (Decimal)(rs["Peso_liquido"].ToString().Equals("") ? new Decimal() : rs["Peso_liquido"]);
                nfItem.Peso_Bruto = (Decimal)(rs["Peso_Bruto"].ToString().Equals("") ? new Decimal() : rs["Peso_Bruto"]);
                nfItem.Tipo = rs["Tipo"].ToString();
                nfItem.CF = nfItem.NCM; //( rs["CF"].ToString();
                nfItem.CSTPIS = rs["CSTPIS"].ToString();
                nfItem.CSTCOFINS = rs["CSTCOFINS"].ToString();

                nfItem.pCredSN = (Decimal)(rs["pCredSN"].ToString().Equals("") ? new Decimal() : rs["pCredSN"]);
                nfItem.vCredicmssn = (Decimal)(rs["vCredicmssn"].ToString().Equals("") ? new Decimal() : rs["vCredicmssn"]);
                nfItem.vOrigem = int.Parse((rs["origem"].ToString().Equals("") ? "-1" : rs["origem"].ToString()));
                nfItem.inativo = (rs["inativo"].ToString().Equals("1") ? true : false);
                int.TryParse(rs["finNfe"].ToString(), out nfItem.finNFe);
                Decimal.TryParse(rs["base_icms"].ToString(), out nfItem.vBaseICMS);
                Decimal.TryParse(rs["icmsv"].ToString(), out nfItem.vicms);
                nfItem.vTotTrib = nfItem.TotTrib;

                // nfe 4.0
                nfItem.indEscala = rs["indEscala"].ToString().Equals("S");
                nfItem.cnpj_Fabricante = rs["cnpj_fabricante"].ToString();
                nfItem.cBenef = rs["cBenef"].ToString();
                Decimal.TryParse(rs["vBCFCP"].ToString(), out nfItem.vBCFCP);
                Decimal.TryParse(rs["pFCP"].ToString(), out nfItem.pFCP);
                Decimal.TryParse(rs["vFCP"].ToString(), out nfItem.vFCP);
                Decimal.TryParse(rs["vBCFCPST"].ToString(), out nfItem.vBCFCPST);
                Decimal.TryParse(rs["pFCPST"].ToString(), out nfItem.pFCPST);
                Decimal.TryParse(rs["vFCPST"].ToString(), out nfItem.vFCPST);
                Decimal.TryParse(rs["vIPIDevol"].ToString(), out nfItem.vIPIDevol);
                Decimal.TryParse(rs["pDevol"].ToString(), out nfItem.pDevol);
                nfItem.Data_validade = Funcoes.dtTry(rs["Data_Validade"].ToString());
                if (nfItem.Tipo_NF.Equals("2"))
                {
                    String sqlItem = "Select top 1 descricao from fornecedor_mercadoria " +
                        "where codigo_referencia = '" + nfItem.CODIGO_REFERENCIA + "' and fornecedor = '" + clienteFornecedor + "'";
                    nfItem.DescricaoNotaFiscalEntrada = Conexao.retornaUmValor(sqlItem, null);

                }
                nfItem.codigo_produto_ANVISA = rs["Codigo_Produto_ANVISA"].ToString();
                nfItem.motivo_isencao_ANVISA = rs["Motivo_Isencao_ANVISA"].ToString();
                Decimal.TryParse(rs["Preco_Maximo_ANVISA"].ToString(), out nfItem.preco_Maximo_ANVISA);
                Decimal.TryParse(rs["Redutor_Base_IVA"].ToString(), out nfItem.redutor_base_ST);
                nfItem.codigoEmissaoNFe = rs["Codigo_Emissao_NF"].ToString();
                nfItem.pedidoItemNumero = rs["Pedido_Numero"].ToString();
                nfItem.pedidoItemSequencia = rs["Pedido_Sequencia"].ToString();
                nfItem.aliquota_ICMS_Destino = Funcoes.decTry(rs["Aliquota_ICMS_Destino"].ToString());
                ArrItens.Add(nfItem);

            }

            if (rs != null)
                rs.Close();
            return ArrItens;
        }

        public ArrayList ArrToString()
        {
            ArrayList item = new ArrayList();
            item.Add(Filial);
            item.Add(Codigo);
            item.Add(Cliente_Fornecedor);
            item.Add(Tipo_NF);
            item.Add(PLU);
            item.Add(Codigo_Tributacao.ToString());
            item.Add(Qtde.ToString("N3"));
            item.Add(Embalagem.ToString("N3"));
            item.Add(Unitario.ToString("N4"));
            item.Add(Desconto.ToString("N4"));
            item.Add(vtotal_produto.ToString("N4"));
            item.Add(porcIPI.ToString("N4"));
            item.Add(Descricao);
            item.Add(vIpiv.ToString("N4"));
            item.Add(ean);
            item.Add(vIva.ToString("N4"));
            item.Add(vBaseIva.ToString("N4"));
            item.Add(vmargemIva.ToString("N4"));
            item.Add(indice_St.ToString());
            item.Add(despesas.ToString("N4"));
            item.Add(CFOP.ToString());
            item.Add(CODIGO_REFERENCIA);
            item.Add(aliquota_icms.ToString("N4"));
            item.Add(aliquota_iva.ToString("N4"));
            item.Add(redutor_base.ToString("N4"));
            item.Add(codigo_operacao.ToString());
            item.Add(Frete.ToString("N4"));
            item.Add(Num_item.ToString());
            item.Add(PISV.ToString("N4"));
            item.Add(COFINSV.ToString("N4"));
            item.Add(NCM.ToString());
            item.Add(Und);
            item.Add(Artigo);
            item.Add(Peso_liquido.ToString());
            item.Add(Peso_Bruto.ToString());
            item.Add(Tipo);
            item.Add(CF);
            item.Add(CSTPIS);
            item.Add(CSTCOFINS);
            item.Add(pCredSN.ToString("N2"));
            item.Add(vCredicmssn.ToString("N2"));
            item.Add(CEST.ToString().Trim());
            item.Add(Data_validadeBr);
            item.Add(DescricaoNotaFiscalEntrada);
            item.Add(codigo_centro_custo.ToString());
            item.Add(descricao_centro_custo).ToString();
            return item;
        }

        public nf_itemDAO(String plu, String codigo, String tipoNf, String clienteFornecedor, User usr)
        {
            this.usr = usr;
            this.PLU = plu;
            this.Codigo = codigo;
            this.Tipo_NF = tipoNf;
            this.Filial = usr.getFilial();
            this.Cliente_Fornecedor = clienteFornecedor;
            String sql = "Select * from  nf_item where plu=" + PLU + " and  codigo ='" + Codigo + "' and tipo_nf = " + Tipo_NF + " and cliente_fornecedor = '" + Cliente_Fornecedor + "'";
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            carregarDados(rs);
        }

        private String dataBr(DateTime dt)
        {
            if (dt.ToString("dd/MM/yyyy").Equals("01/01/0001"))
            {
                return "";
            }
            else
            {
                return dt.ToString("dd/MM/yyyy");
            }
        }
        public void carregarDados(SqlDataReader rs)
        {
            try
            {


                if (rs.Read())
                {
                    Filial = rs["Filial"].ToString();
                    Codigo = rs["Codigo"].ToString();
                    Cliente_Fornecedor = rs["Cliente_Fornecedor"].ToString();
                    Tipo_NF = rs["Tipo_NF"].ToString();
                    Ordem_compra = rs["Ordem_compra"].ToString();
                    PLU = rs["PLU"].ToString();
                    Codigo_Tributacao = (Decimal)(rs["Codigo_Tributacao"].ToString().Equals("") ? new Decimal() : rs["Codigo_Tributacao"]);
                    codigo_centro_custo = rs["codigo_centro_custo"].ToString();
                    Qtde = (Decimal)(rs["Qtde"].ToString().Equals("") ? new Decimal() : rs["Qtde"]);
                    Embalagem = (Decimal)(rs["Embalagem"].ToString().Equals("") ? new Decimal() : rs["Embalagem"]);
                    Unitario = (Decimal)(rs["Unitario"].ToString().Equals("") ? new Decimal() : rs["Unitario"]);
                    Desconto = (Decimal)(rs["Desconto"].ToString().Equals("") ? new Decimal() : rs["Desconto"]);
                    Total = (Decimal)(rs["Total"].ToString().Equals("") ? new Decimal() : rs["Total"]);
                    IPI = (Decimal)(rs["IPI"].ToString().Equals("") ? new Decimal() : rs["IPI"]);
                    Descricao = rs["Descricao"].ToString();
                    IPIV = (Decimal)(rs["IPIV"].ToString().Equals("") ? new Decimal() : rs["IPIV"]);
                    ean = rs["ean"].ToString();
                    vmargemIva = (Decimal)(rs["margem_iva"].ToString().Equals("") ? new Decimal() : rs["margem_iva"]);
                    vBaseIva = (Decimal)(rs["base_iva"].ToString().Equals("") ? new Decimal() : rs["base_iva"]);
                    vIva = (Decimal)(rs["iva"].ToString().Equals("") ? new Decimal() : rs["iva"]);
                    indice_St = rs["indice_st"].ToString();
                    despesas = (Decimal)(rs["despesas"].ToString().Equals("") ? new Decimal() : rs["despesas"]);
                    CFOP = (Decimal)(rs["CFOP"].ToString().Equals("") ? new Decimal() : rs["CFOP"]);
                    CODIGO_REFERENCIA = rs["CODIGO_REFERENCIA"].ToString();
                    aliquota_icms = (Decimal)(rs["aliquota_icms"].ToString().Equals("") ? new Decimal() : rs["aliquota_icms"]);
                    redutor_base = (Decimal)(rs["redutor_base"].ToString().Equals("") ? new Decimal() : rs["redutor_base"]);
                    codigo_operacao = (Decimal)(rs["codigo_operacao"].ToString().Equals("") ? new Decimal() : rs["codigo_operacao"]);
                    Frete = (Decimal)(rs["Frete"].ToString().Equals("") ? new Decimal() : rs["Frete"]);
                    Num_item = (rs["Num_item"] == null ? 0 : int.Parse(rs["Num_item"].ToString()));
                    PISV = (rs["PISV"].ToString().Equals("") ? new Decimal() : Decimal.Parse(rs["PISV"].ToString()));
                    COFINSV = (rs["COFINSV"].ToString().Equals("") ? new Decimal() : Decimal.Parse(rs["COFINSV"].ToString()));
                    NCM = (rs["NCM"] == null ? "null" : rs["NCM"].ToString());
                    CEST = rs["CEST"].ToString();
                    Und = rs["Und"].ToString();
                    Artigo = rs["Artigo"].ToString();
                    Peso_liquido = (Decimal)(rs["Peso_liquido"].ToString().Equals("") ? new Decimal() : rs["Peso_liquido"]);
                    Peso_Bruto = (Decimal)(rs["Peso_Bruto"].ToString().Equals("") ? new Decimal() : rs["Peso_Bruto"]);
                    Tipo = rs["Tipo"].ToString();
                    CF = rs["CF"].ToString();
                    CSTPIS = rs["CSTPIS"].ToString();
                    CSTCOFINS = rs["CSTCOFINS"].ToString();

                    pCredSN = (Decimal)(rs["pCredSN"].ToString().Equals("") ? new Decimal() : rs["pCredSN"]);
                    vCredicmssn = (Decimal)(rs["vCredicmssn"].ToString().Equals("") ? new Decimal() : rs["vCredicmssn"]);
                    vOrigem = int.Parse(rs["origem"].ToString().Equals("") ? "-1" : rs["origem"].ToString());
                    inativo = (rs["inativo"].ToString().Equals("1") ? true : false);
                    int.TryParse(rs["finNfe"].ToString(), out finNFe);

                    indEscala = rs["indEscala"].ToString().Equals("S");
                    cnpj_Fabricante = rs["cnpj_fabricante"].ToString();
                    cBenef = rs["cBenef"].ToString();
                    Decimal.TryParse(rs["vBCFCP"].ToString(), out vBCFCP);
                    Decimal.TryParse(rs["pFCP"].ToString(), out pFCP);
                    Decimal.TryParse(rs["vFCP"].ToString(), out vFCP);
                    Decimal.TryParse(rs["vBCFCPST"].ToString(), out vBCFCP);
                    Decimal.TryParse(rs["pFCPST"].ToString(), out pFCPST);
                    Decimal.TryParse(rs["vFCPST"].ToString(), out vFCPST);
                    Data_validade = Funcoes.dtTry(rs["Data_Validade"].ToString());
                    Decimal.TryParse(rs["Redutor_Base_ST"].ToString(), out redutor_base_ST);
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
        public void CalculaImpostos()
        {
            CalculaImpostos(false);
        }
        internal void CalculaImpostos(bool round)
        {
            TotalProduto(round);
            calculoIpi();
            calculaPisCofins();
            //Evitar o cálculo de impostos dos itens quando se tratar de uma NFe de importação na entrada.

            baseICMS();
            valorIcms();
            CalculoIva();
            //calculaPisCofins();
        }

        //Incluido para DIFAL e FCPST
        public void calcularFCPST(String UfCliente, decimal baseFCP = 0)
        {
            //Decimal pFCP = 0;

            //Decimal.TryParse(Conexao.retornaUmValor("Select porc from uf_pobreza where uf ='" + UfCliente + "'", null), out pFCP);
            aliquota_imp_estadoDAO DadosEstado = new aliquota_imp_estadoDAO(ufCliente, NCM, null);


            if (DadosEstado.porc_combate_pobresa > 0)
            {
                if (baseFCP > 0)
                {
                    //this.vBCFCPST = this.vBaseIva;
                    //this.pFCPST = pFCP;
                    //this.vFCP = 
                    //this.vFCPST = (this.vBCFCPST * this.pFCPST) / 100;
                    this.pFCP = DadosEstado.porc_combate_pobresa;
                    this.vBCFCP = baseFCP;
                    //this.pFCP = pFCP;
                    this.vFCP = (this.vBCFCP * this.pFCP) / 100;
                }
                else
                {
                    this.vBCFCP = this.vBaseICMS;
                    this.pFCP = pFCP;
                    this.vFCP = (this.vBCFCP * pFCP) / 100;
                }
            }
            else
            {
                this.pFCP = 0;
                this.vBCFCP = 0;
                this.pFCP = 0;
                this.vFCP = 0;
            }
        }

        public void atualizarItem(SqlConnection conn, SqlTransaction tran, DateTime dataMovimento)
        {
            Produto produto = new Produto(this.Filial, this.PLU);

            if (Tipo_NF.Equals("2") && !naturezaOperacao.Imprime_NF)  //descarta ultima alteração
            {

                String sqlprodutofornecedor = "";
                String sqlIMercFornc = "";
                try
                {

                    sqlprodutofornecedor = "select * from fornecedor_mercadoria where codigo_referencia='" + CODIGO_REFERENCIA + "' and fornecedor='" + Cliente_Fornecedor + "'";
                    if (Conexao.countSql(sqlprodutofornecedor, usr, conn, tran) > 0)
                    {
                        Conexao.executarSql("delete from fornecedor_mercadoria where codigo_referencia='" + CODIGO_REFERENCIA + "' and fornecedor='" + Cliente_Fornecedor + "'", conn, tran);
                    }


                    String strDescSalvar = DescricaoNotaFiscalEntrada;
                    if (strDescSalvar.Length == 0)
                        strDescSalvar = Descricao;

                    sqlIMercFornc = "insert into fornecedor_mercadoria (filial,fornecedor,plu,descricao,Data,preco_compra,ean,codigo_referencia,preco_custo,embalagem,importado_nf)" +
                           " values ('" + usr.getFilial() + "','" + Cliente_Fornecedor + "','" + PLU + "','" + (strDescSalvar.Length > 40 ? strDescSalvar.Substring(0, 40) : strDescSalvar) + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                         Funcoes.decimalPonto(Unitario.ToString()) + ",'" + ean + "','" + CODIGO_REFERENCIA + "'," + Funcoes.decimalPonto((Total / (Qtde * Embalagem)).ToString()) + "," + Funcoes.decimalPonto(Embalagem.ToString()) + ",1)";
                    Conexao.executarSql(sqlIMercFornc, conn, tran);
                    bool bAtualizaFornec = Funcoes.valorParametro("NAO_ATUALIZA_FORNECEDOR", usr).Equals("TRUE");

                    if (!bAtualizaFornec)
                    {
                        Conexao.executarSql("update mercadoria set ultimo_fornecedor= '" + Cliente_Fornecedor + "', Ref_fornecedor='" + CODIGO_REFERENCIA + "'  where plu ='" + PLU + "'", conn, tran);

                    }

                }
                catch (Exception err)
                {

                    throw new Exception("codigo de referencia no fornecedor:" + err.Message + "<\\br> Sql1:" + sqlprodutofornecedor + " <\\br>Sql2:" + sqlIMercFornc);
                }

                if (naturezaOperacao.Baixa_estoque)
                {
                    try
                    {
                        Decimal vLogQtde = 0;
                        Decimal vLogEmbalagen = 0;
                        SqlDataReader rsUltAlteracao = Conexao.consulta("select  top 1 qtde,embalagem ,data_alteracao from nf_item_log where plu='" + PLU + "' and codigo='" + Codigo + "' and filial='" + Filial + "' and  tipo_nf=2 and Num_item=" + Num_item + " order by data_alteracao desc", null, false, conn, tran);
                        if (rsUltAlteracao.Read())
                        {
                            vLogQtde = (rsUltAlteracao["qtde"].ToString().Equals("") ? 0 : Decimal.Parse(rsUltAlteracao["qtde"].ToString()));
                            vLogEmbalagen = (rsUltAlteracao["embalagem"].ToString().Equals("") ? 0 : Decimal.Parse(rsUltAlteracao["embalagem"].ToString()));
                        }
                        if (rsUltAlteracao != null)
                            rsUltAlteracao.Close();

                        //volta o estoque 
                        Funcoes.atualizaSaldoPLU(Filial, PLU, -1 * (vLogQtde * vLogEmbalagen), conn, tran, dataMovimento, "EN");
                        //String SqlEstoque = "update mercadoria_Loja set  saldo_atual = (isnull(saldo_atual,0) -" + Funcoes.decimalPonto((vLogQtde * vLogEmbalagen).ToString()) + ") where plu='" + PLU + "' and filial='" + Filial + "'";
                        //Conexao.executarSql(SqlEstoque, conn, tran);

                        //Atualiza saldo do dia.
                        Funcoes.atualizaSaldoPLUDia(Filial, PLU, -1 * (vLogQtde * vLogEmbalagen), conn, tran, "EN", dataMovimento);


                    }
                    catch (Exception err)
                    {

                        throw new Exception("retorno valor alteracao mercadoria_loja : Detalhe:" + err.Message);
                    }

                    try
                    {
                        Funcoes.atualizaSaldoPLU(Filial, PLU, (Qtde * Embalagem), conn, tran, dataMovimento, "EN");
                        //String SqlEstoque = "update mercadoria_Loja set  saldo_atual = (ISNULL(saldo_atual,0) +" + Funcoes.decimalPonto((Qtde * Embalagem).ToString()) + ") where plu='" + PLU + "' and filial='" + Filial + "'";
                        //Conexao.executarSql(SqlEstoque, conn, tran);
                        //String SqlMercadoria = " update mercadoria  set  saldo_atual =(select sum(isnull(saldo_atual,0))from mercadoria_loja b where b.plu='" + PLU + "') where plu='" + PLU + "'";
                        //Conexao.executarSql(SqlMercadoria, conn, tran);
                        Funcoes.atualizaSaldoPLUDia(Filial, PLU, (Qtde * Embalagem), conn, tran, "EN", dataMovimento);
                    }
                    catch (Exception err)
                    {

                        throw new Exception("Atualização estoque mercadoria_loja , mercadoria : Erro Detalhe:" + err.Message);
                    }
                }
                if (naturezaOperacao.Gera_custo)
                {
                    try
                    {
                        String SqlCusto = "update mercadoria set   preco_custo = " + Funcoes.decimalPonto(TotalCustoUnitario.ToString()) + " , preco_compra=" + Funcoes.decimalPonto((vtotal_produto / (Qtde * Embalagem)).ToString()) + ", cf='" + NCM + "'  where plu='" + PLU + "' ";
                        Conexao.executarSql(SqlCusto, conn, tran);

                        if (!CEST.Equals(""))
                        {
                            String SqlCEST = "update mercadoria set   CEST='" + CEST + "'  where plu='" + PLU + "' ";
                            Conexao.executarSql(SqlCusto, conn, tran);
                        }




                        String SqlCustoLoja = "update mercadoria_loja set PRECO_CUSTO_2 = PRECO_CUSTO_1, Preco_Custo_1 = Preco_Custo,  preco_custo = " + Funcoes.decimalPonto(TotalCustoUnitario.ToString()) + " , preco_compra=" + Funcoes.decimalPonto((vtotal_produto / (Qtde * Embalagem)).ToString()) + ", data_alteracao='" + DateTime.Now.ToString("yyyy-MM-dd") + "'  where plu='" + PLU + "' and filial='" + Filial + "'";
                        Conexao.executarSql(SqlCustoLoja, conn, tran);

                        //Checa se a natureza de operação não precifica
                        //bool PrecificacaoDesativada = (Funcoes.valorParametro("PRECIFIC_NF_OFF", usr).Equals("TRUE") ? true : false);
                        //if (!naturezaOperacao.Precificacao)
                        //{
                            try
                            {
                                SqlCustoLoja = "UPDATE Mercadoria_loja SET Mercadoria_loja.Margem = CONVERT(DECIMAL(12,4), ((Mercadoria_loja.Preco - Mercadoria_loja.Preco_Custo) / Mercadoria_loja.Preco_Custo) * 100)   WHERE Mercadoria_loja.plu='" + PLU + "' AND Mercadoria_loja.filial='" + Filial + "'";
                                Conexao.executarSql(SqlCustoLoja, conn, tran);

                                SqlCustoLoja = "UPDATE Mercadoria SET Mercadoria.Margem = CONVERT(DECIMAL(12,4), ((Mercadoria.Preco - Mercadoria.Preco_Custo) / Mercadoria.Preco_Custo) * 100)   WHERE Mercadoria.plu='" + PLU + "' AND Mercadoria.filial='" + Filial + "'";
                                Conexao.executarSql(SqlCustoLoja, conn, tran);

                                //Insere em LOG_PRECO
                                SqlCustoLoja = "INSERT INTO Log_Preco (Filial, PLU, Descricao, Data, Usuario, Preco_Old, Preco_New, Custo_Old, Custo_New) VALUES (";
                                SqlCustoLoja += "'" + Filial + "', '" + PLU + "', 'Entrada NFe', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + usr.getUsuario() + "'";
                                SqlCustoLoja += ", " + produto.preco.ToString().Replace(",", ".");
                                SqlCustoLoja += ", " + produto.preco.ToString().Replace(",", ".");
                                SqlCustoLoja += ", " + produto.custo.ToString().Replace(",", ".");
                                SqlCustoLoja += ", " + (this.TotalCusto / (this.Qtde * this.Embalagem)).ToString().Replace(",", ".");
                                SqlCustoLoja += ")";
                                Conexao.executarSql(SqlCustoLoja, conn, tran);
                            }
                            catch
                            {

                            }
                        //}


                    }
                    catch (Exception err)
                    {
                        throw new Exception("Atualização do preco_custo,Preco_compra, margem_iva, NCM,tributacao da Tabela mercadoria-" + err);

                    }

                }
                String atualiza = Funcoes.valorParametro("ATUALIZA_TRIBUTACAO", usr);
                if (atualiza.Equals(".T.") || atualiza.ToUpper().Equals("TRUE"))
                {
                    /* De -> Para
                        1	50
                        2	50
                        3	50
                        4	74
                        5	75
                        6	73
                        7	71
                        8	74
                        9	72
                        49	98
                        Alteração só é executada qdo atualização estiver com CST de SAÍDA. 
                    */
                    switch (Funcoes.intTry(vCSTCOFINS))
                    {
                        case 1:
                        case 2:
                        case 3:
                            vCSTCOFINS = "50";
                            break;
                        case 4:
                            vCSTCOFINS = "74";
                            break;
                        case 5:
                            vCSTCOFINS = "75";
                            break;
                        case 6:
                            vCSTCOFINS = "73";
                            break;
                        case 7:
                            vCSTCOFINS = "71";
                            break;
                        case 8:
                            vCSTCOFINS = "74";
                            break;
                        case 9:
                            vCSTCOFINS = "72"; 
                            break;
                    }


                    String SqlCusto = "update mercadoria set  CST_Entrada = " + vCSTCOFINS + ", margem_iva = " + Funcoes.decimalPonto(vmargemIva.ToString("N2")) + ",ipi=" + Funcoes.decimalPonto(porcIPI.ToString()) + " ,codigo_tributacao_ent=" + Codigo_Tributacao + ", cf='" + NCM + "', CEST='" + CEST + "'    where plu='" + PLU + "' and filial='" + Filial + "'";
                    Conexao.executarSql(SqlCusto, conn, tran);

                }

            }
            else
            {

                try
                {
                    if (!NCM.Equals("") || !CEST.Equals(""))
                    {
                        String SqlCusto = "update mercadoria set   cf='" + NCM + "', CEST='" + CEST + "'  where plu='" + PLU + "' ";
                        Conexao.executarSql(SqlCusto, conn, tran);
                    }
                }
                catch (Exception)
                {

                    throw new Exception("Atualizar Ncm ou CEST Tabela Mercadoria");
                }



            }

        }


        private void update(SqlConnection conn, SqlTransaction tran)
        {
            try
            {


                String sql = "update  nf_item set " +
                              "plu='" + PLU + "'" +
                              ",Codigo_Tributacao=" + Funcoes.decimalPonto(Codigo_Tributacao.ToString()) +
                              ",Qtde=" + Funcoes.decimalPonto(Qtde.ToString()) +
                              ",Embalagem=" + Funcoes.decimalPonto(Embalagem.ToString()) +
                              ",Unitario=" + Funcoes.decimalPonto(Unitario.ToString("N4")) +
                              ",Desconto=" + Funcoes.decimalPonto(Desconto.ToString("N4")) +
                              ",Total=" + Funcoes.decimalPonto(vtotal_produto.ToString("N2")) +
                              ",IPI=" + Funcoes.decimalPonto(IPI.ToString()) +
                              ",Descricao='" + Descricao + "'" +
                              ",IPIV=" + Funcoes.decimalPonto(IPIV.ToString()) +
                              ",ean='" + ean + "'" +
                              ",iva=" + Funcoes.decimalPonto(vIva.ToString()) +
                              ",base_iva=" + Funcoes.decimalPonto(vBaseIva.ToString()) +
                              ",margem_iva=" + Funcoes.decimalPonto(vmargemIva.ToString()) +
                              ",despesas=" + Funcoes.decimalPonto(despesas.ToString()) +
                              ",CFOP=" + Funcoes.decimalPonto(CFOP.ToString()) +
                              ",CODIGO_REFERENCIA='" + CODIGO_REFERENCIA + "'" +
                              ",aliquota_icms=" + Funcoes.decimalPonto(aliquota_icms.ToString()) +
                              ",aliquota_iva=" + Funcoes.decimalPonto(aliquota_iva.ToString()) +
                              ",indice_st='" + indice_St + "'" +
                              ",redutor_base=" + Funcoes.decimalPonto(redutor_base.ToString()) +
                              ",codigo_operacao=" + Funcoes.decimalPonto(codigo_operacao.ToString()) +
                              ",Frete=" + Funcoes.decimalPonto(Frete.ToString()) +
                              ",Num_item=" + Num_item +
                              ",PISV=" + Funcoes.decimalPonto(PISV.ToString("N4")) +
                              ",COFINSV=" + Funcoes.decimalPonto(COFINSV.ToString("N4")) +
                              ",NCM='" + NCM + "'" +
                              ",CEST='" + CEST + "'" +
                              ",Und='" + Und + "'" +
                              ",Artigo='" + Artigo + "'" +
                              ",Peso_liquido=" + Funcoes.decimalPonto(Peso_liquido.ToString()) +
                              ",Peso_Bruto=" + Funcoes.decimalPonto(Peso_Bruto.ToString()) +
                              ",Tipo='" + Tipo + "'" +
                              ",CF='" + CF + "'" +
                              ",CSTPIS='" + CSTPIS + "'" +
                              ",CSTCOFINS='" + CSTCOFINS + "'" +
                              ",pCredSN=" + Funcoes.decimalPonto(pCredSN.ToString()) +
                              ",vCredicmssn=" + Funcoes.decimalPonto(vCredicmssn.ToString()) +
                              ",origem =" + origem.ToString() +
                              ",inativo=" + (inativo ? "1" : "0") +
                              ",base_pis = " + Funcoes.decimalPonto(vPBasePisCofins.ToString()) +
                              ",base_cofins=" + Funcoes.decimalPonto(vPBasePisCofins.ToString()) +
                              ",aliquota_pis=" + Funcoes.decimalPonto(PISp.ToString()) +
                              ",aliquota_cofins=" + Funcoes.decimalPonto(COFINSp.ToString()) +
                              ",cst_icms='" + indice_St + "'" +
                              ",base_icms= abs(" + Funcoes.decimalPonto(vBaseICMS.ToString()) + ")" +
                              ",icmsv= abs(" + Funcoes.decimalPonto(vicms.ToString()) + ")" +
                              ",CST_IPI='" + CST_IPI + "'" +
                              ",base_ipi=" + Funcoes.decimalPonto(baseIpi.ToString()) +
                              ",vTotTrib=" + Funcoes.decimalPonto(vTotTrib.ToString()) +
                              ",finNFe=" + finNFe +
                              ",total_faturado=" + Funcoes.decimalPonto(Total.ToString()) +
                              ",indEscala='" + (indEscala ? "S" : "N") + "'" +
                              ",cnpj_Fabricante='" + cnpj_Fabricante + "'" +
                              ",cBenef='" + cBenef + "'" +
                              ",vBCFCP=" + Funcoes.decimalPonto(vBCFCP.ToString()) +
                              ",pFCP=" + Funcoes.decimalPonto(pFCP.ToString()) +
                              ",vFCP=" + Funcoes.decimalPonto(vFCP.ToString()) +
                              ",vBCFCPST=" + Funcoes.decimalPonto(vBCFCPST.ToString()) +
                              ",pFCPST=" + Funcoes.decimalPonto(pFCPST.ToString()) +
                              ",vFCPST=" + Funcoes.decimalPonto(vFCPST.ToString()) +
                              ",vIPIDevol=" + Funcoes.decimalPonto(vIPIDevol.ToString()) +
                              ",pDevol=" + Funcoes.decimalPonto(pDevol.ToString()) +
                              ",data_validade=" + (Data_validade.Equals(DateTime.MinValue) ? "null" : "'" + Data_validade.ToString("yyyy-MM-dd") + "'") +
                              ",serie=" + serie.ToString() +
                              ",total_produto=" + Funcoes.decimalPonto(vtotal_produto.ToString()) +
                              ",codigo_centro_custo='" + codigo_centro_custo + "'" +
                              ",ordem_compra ='" + Ordem_compra + "'" +
                              ", Codigo_Produto_ANVISA = '" + codigo_produto_ANVISA + "'" +
                              ", Motivo_Isencao_ANVISA = '" + motivo_isencao_ANVISA + "'" +
                              ", Preco_Maximo_ANVISA = " + Funcoes.decimalPonto(preco_Maximo_ANVISA.ToString()) +
                              ", redutor_base_IVA = " + Funcoes.decimalPonto(redutor_base_ST.ToString()) +
                              ", Codigo_Emisao_NFe = '" + codigoEmissaoNFe + "'" +
                              ", Pedido_Numero = '" + pedidoItemNumero + "'" +
                              ", Pedido_Sequencia = '" + pedidoItemSequencia + "'" +
                              ", Aliquota_ICMS_Destino = " + Funcoes.decimalPonto(aliquota_ICMS_Destino.ToString()) +
                              ", Valor_Difal = " + Funcoes.decimalPonto(valor_Difal_Item.ToString()) +
                       "  where Filial='" + Filial + "' and codigo ='" + Codigo + "' and tipo_nf = " + Tipo_NF + " and cliente_fornecedor = '" + Cliente_Fornecedor + "'  and  Num_item = " + Num_item;

                Conexao.executarSql(sql, conn, tran);




            }
            catch (Exception err)
            {

                throw new Exception("update: Erro Detalhe:" + err.Message);
            }
        }

        public void AtualizaSaidaEstoque(SqlConnection conn, SqlTransaction tran, DateTime dataMovimentacao)
        {
            if (naturezaOperacao != null)
            {

                try
                {
                    if (Tipo_NF.Equals("1") || Tipo_NF.Equals("3"))
                    {
                        if (naturezaOperacao.Baixa_estoque)
                        {
                            Funcoes.atualizaSaldoPLU(Filial, PLU, -(Qtde * Embalagem), conn, tran, dataMovimentacao,"EN");
                            //String SqlEstoque = " update mercadoria_loja set  saldo_atual = (isnull(saldo_atual,0) -" + Funcoes.decimalPonto((Qtde * Embalagem).ToString()) + ") where plu='" + PLU + "' and filial='" + Filial + "'";
                            //Conexao.executarSql(SqlEstoque, conn, tran);
                            //String SqlMercadoria = " update mercadoria  set  saldo_atual =(select sum(isnull(saldo_atual,0))from mercadoria_loja b where b.plu='" + PLU + "') where plu='" + PLU + "'";
                            //Conexao.executarSql(SqlMercadoria, conn, tran);
                            Funcoes.atualizaSaldoPLUDia(Filial, PLU, (Qtde * Embalagem), conn, tran, "SN", dataMovimentacao);
                        }
                    }
                    else
                    {
                        if (naturezaOperacao.Baixa_estoque)
                        {
                            Funcoes.atualizaSaldoPLU(Filial, PLU, ((Qtde * Embalagem)), conn, tran, dataMovimentacao, "EN");
                            //String SqlEstoque = " update mercadoria_loja set  saldo_atual = (isnull(saldo_atual,0) +" + Funcoes.decimalPonto((Qtde * Embalagem).ToString()) + ") where plu='" + PLU + "' and filial='" + Filial + "'";
                            //Conexao.executarSql(SqlEstoque, conn, tran);
                            //String SqlMercadoria = " update mercadoria  set  saldo_atual =(select sum(isnull(saldo_atual,0))from mercadoria_loja b where b.plu='" + PLU + "') where plu='" + PLU + "'";
                            //Conexao.executarSql(SqlMercadoria, conn, tran);
                            Funcoes.atualizaSaldoPLUDia(Filial, PLU, (Qtde * Embalagem), conn, tran, "EN", dataMovimentacao);
                        }
                        if (naturezaOperacao.Gera_custo)
                        {

                            String SqlCusto = "update mercadoria set  preco_custo = " + Funcoes.decimalPonto(TotalCustoUnitario.ToString()) + " , preco_compra=" + Funcoes.decimalPonto((vtotal_produto / (Qtde * Embalagem)).ToString()) + ", cf='" + NCM + "'  where plu='" + PLU + "' and filial='" + Filial + "'";
                            Conexao.executarSql(SqlCusto, conn, tran);

                            String SqlCustoLoja = "update mercadoria_loja set PRECO_CUSTO_2 = PRECO_CUSTO_1, Preco_Custo_1 = Preco_Custo,  preco_custo = " + Funcoes.decimalPonto(TotalCustoUnitario.ToString()) + " , preco_compra=" + Funcoes.decimalPonto((vtotal_produto / (Qtde * Embalagem)).ToString()) + ", data_alteracao='" + DateTime.Now.ToString("yyyy-MM-dd") + "'  where plu='" + PLU + "' and filial='" + Filial + "'";
                            Conexao.executarSql(SqlCustoLoja, conn, tran);

                        }

                    }


                }
                catch (Exception err)
                {

                    throw new Exception("Atualização estoque mercadoria_loja , mercadoria : Erro Detalhe:" + err.Message);
                }

            }


        }

        public bool salvar(bool novo, SqlConnection conn, SqlTransaction tran)
        {

            if (novo)
            {
                insert(conn, tran);
            }
            else
            {
                update(conn, tran);
            }
            return true;
        }

        public bool excluir(SqlConnection conn, SqlTransaction tran, DateTime dataMovimentacao)
        {
            if (naturezaOperacao != null && naturezaOperacao.Baixa_estoque)
            {
                if (Tipo_NF.Equals("2"))
                {
                    try
                    {
                        Funcoes.atualizaSaldoPLU(Filial, PLU, -1 * (Qtde * Embalagem), conn, tran, dataMovimentacao, "EN");

                        //String SqlEstoque = "update mercadoria_Loja set  saldo_atual = (isnull(saldo_atual,0) -" + Funcoes.decimalPonto((Qtde * Embalagem).ToString()) + ") where plu='" + PLU + "' and filial='" + Filial + "'";
                        //Conexao.executarSql(SqlEstoque, conn, tran);
                        //String SqlMercadoria = " update mercadoria  set  saldo_atual =(select sum(isnull(saldo_atual,0))from mercadoria_loja b where b.plu='" + PLU + "') where plu='" + PLU + "'";
                        //Conexao.executarSql(SqlMercadoria, conn, tran);
                        Funcoes.atualizaSaldoPLUDia(Filial, PLU, -1 * (Qtde * Embalagem), conn, tran, "EN", dataMovimentacao);
                    }
                    catch (Exception err)
                    {

                        throw new Exception("Atualização estoque Excluir Item  mercadoria_loja , mercadoria : Erro Detalhe:" + err.Message);
                    }
                }

                if (Tipo_NF.Equals("3"))
                {
                    try
                    {
                        Funcoes.atualizaSaldoPLU(Filial, PLU, (Qtde * Embalagem), conn, tran,  dataMovimentacao, "SN");

                        //String SqlEstoque = "update mercadoria_Loja set  saldo_atual = (isnull(saldo_atual,0) +" + Funcoes.decimalPonto((Qtde * Embalagem).ToString()) + ") where plu='" + PLU + "' and filial='" + Filial + "'";
                        //Conexao.executarSql(SqlEstoque, conn, tran);
                        //String SqlMercadoria = " update mercadoria  set  saldo_atual =(select sum(isnull(saldo_atual,0))from mercadoria_loja b where b.plu='" + PLU + "') where plu='" + PLU + "'";
                        //Conexao.executarSql(SqlMercadoria, conn, tran);
                        Funcoes.atualizaSaldoPLUDia(Filial, PLU, -1 * (Qtde * Embalagem), conn, tran, "SN", dataMovimentacao);
                    }
                    catch (Exception err)
                    {

                        throw new Exception("Atualização estoque Excluir Item  mercadoria_loja , mercadoria : Erro Detalhe:" + err.Message);
                    }
                }
            }
            if (!Tipo_NF.Equals("3"))
            {
                String sql = "delete from nf_item where codigo ='" + Codigo + "' and tipo_nf = " + Tipo_NF + " and cliente_fornecedor = '" + Cliente_Fornecedor + "' and plu='" + PLU + "'"; //colocar campo index

                Conexao.executarSql(sql, conn, tran);
            }
            return true;
        }
        public bool cancela(SqlConnection conn, SqlTransaction tran, DateTime dataMovimento)
        {

            if (naturezaOperacao != null && naturezaOperacao.Baixa_estoque)
            {
                if (Tipo_NF.Equals("2"))
                {
                    try
                    {
                        Funcoes.atualizaSaldoPLU(Filial, PLU, -1 * (Qtde * Embalagem), conn, tran, dataMovimento, "EN");

                        //String SqlEstoque = "update mercadoria_Loja set  saldo_atual = (isnull(saldo_atual,0) -" + Funcoes.decimalPonto((Qtde * Embalagem).ToString()) + ") where plu='" + PLU + "' and filial='" + Filial + "'";
                        //Conexao.executarSql(SqlEstoque);
                        //String SqlMercadoria = " update mercadoria  set  saldo_atual =(select sum(isnull(saldo_atual,0))from mercadoria_loja b where b.plu='" + PLU + "') where plu='" + PLU + "'";
                        //Conexao.executarSql(SqlMercadoria, conn, tran);
                        Funcoes.atualizaSaldoPLUDia(Filial, PLU, -1 * (Qtde * Embalagem), conn, tran, "EN", dataMovimento);
                    }
                    catch (Exception err)
                    {

                        throw new Exception("Atualização estoque Cancelamento  mercadoria_loja , mercadoria : Erro Detalhe:" + err.Message);
                    }
                }
                else
                {
                    try
                    {
                        Funcoes.atualizaSaldoPLU(Filial, PLU, (Qtde * Embalagem), conn, tran, dataMovimento, "SN");

                        //String SqlEstoque = "update mercadoria_Loja set  saldo_atual = (isnull(saldo_atual,0) +" + Funcoes.decimalPonto((Qtde * Embalagem).ToString()) + ") where plu='" + PLU + "' and filial='" + Filial + "'";
                        //Conexao.executarSql(SqlEstoque);
                        //String SqlMercadoria = " update mercadoria  set  saldo_atual =(select sum(isnull(saldo_atual,0))from mercadoria_loja b where b.plu='" + PLU + "') where plu='" + PLU + "'";
                        //Conexao.executarSql(SqlMercadoria, conn, tran);
                        Funcoes.atualizaSaldoPLUDia(Filial, PLU, -1 * (Qtde * Embalagem), conn, tran, "SN", dataMovimento);
                    }
                    catch (Exception err)
                    {

                        throw new Exception("Atualização Saida estoque Cancelamento  mercadoria_loja , mercadoria : Erro Detalhe:" + err.Message);
                    }
                }
            }
            Conexao.executarSql("update nf_item set nf_canc=1  where plu ='" + PLU + "' and codigo=" + Codigo + " and cliente_fornecedor='" + Cliente_Fornecedor + "' and tipo_nf= " + Tipo_NF + "and filial='" + Filial + "'", conn, tran);





            return true;
        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = " insert into nf_item (" +
                          "Filial," +
                          "Codigo," +
                          "Cliente_Fornecedor," +
                          "Tipo_NF," +
                          "PLU," +
                          "Codigo_Tributacao," +
                          "Qtde," +
                          "Embalagem," +
                          "Unitario," +
                          "Desconto," +
                          "Total," +
                          "IPI," +
                          "Descricao," +
                          "IPIV," +
                          "ean," +
                          "iva," +
                          "base_iva," +
                          "margem_iva," +
                          "despesas," +
                          "CFOP," +
                          "CODIGO_REFERENCIA," +
                          "aliquota_icms," +
                          "aliquota_iva," +
                          "indice_st," +
                          "redutor_base," +
                          "codigo_operacao," +
                          "Frete," +
                          "Num_item," +
                          "PISV," +
                          "COFINSV," +
                          "NCM," +
                          "CEST," +
                          "Und," +
                          "Artigo," +
                          "Peso_liquido," +
                          "Peso_Bruto," +
                          "Tipo," +
                          "CF," +
                          "CSTPIS," +
                          "CSTCOFINS," +
                          "pCredSN," +
                          "vCredicmssn," +
                          "origem," +
                          "inativo" +
                          ",base_pis " +
                          ",base_cofins" +
                          ",aliquota_pis" +
                          ",aliquota_cofins" +
                          ",cst_icms" +
                          ",base_icms" +
                          ",icmsv" +
                          ",CST_IPI" +
                          ",Base_ipi" +
                          ",vTotTrib" +
                          ",finNFe" +
                          ",total_faturado" +
                          ",indEscala" +
                          ",cnpj_Fabricante" +
                          ",cBenef" +
                          ",vBCFCP" +
                          ",pFCP" +
                          ",vFCP" +
                          ",vBCFCPST" +
                          ",pFCPST" +
                          ",vFCPST" +
                          ",pDevol" +
                          ",Data_validade" +
                          ",serie" +
                          ",total_produto" +
                          ",desconto_valor" +
                          ",codigo_centro_custo" +
                          ",ordem_compra" +
                          ", Codigo_Produto_ANVISA" +
                          ", Motivo_Isencao_ANVISA" +
                          ", Preco_Maximo_ANVISA" +
                          ", Redutor_Base_IVA" +
                          ", Codigo_Emissao_NFe" +
                          ", Pedido_Numero " + 
                          ", Pedido_Sequencia" +
                          ", Aliquota_ICMS_Destino" +
                          ", Valor_Difal"+
                          ", Qtde_DevolVer"+
                     " )values (" +
                            "'" + Filial + "'" +
                            "," + "'" + Codigo + "'" +
                            "," + "'" + Cliente_Fornecedor + "'" +
                            "," + Tipo_NF +
                            "," + "'" + PLU + "'" +
                            "," + Funcoes.decimalPonto(Codigo_Tributacao.ToString()) +
                            "," + Funcoes.decimalPonto(Qtde.ToString()) +
                            "," + Funcoes.decimalPonto(Embalagem.ToString()) +
                            "," + Funcoes.decimalPonto(Unitario.ToString("N4")) +
                            "," + Funcoes.decimalPonto(Desconto.ToString("N4")) +
                            "," + Funcoes.decimalPonto(vtotal_produto.ToString("N2")) +
                            "," + Funcoes.decimalPonto(porcIPI.ToString()) +
                            "," + "'" + Descricao + "'" +
                            "," + Funcoes.decimalPonto(vIpiv.ToString()) +
                            "," + "'" + ean + "'" +
                            "," + Funcoes.decimalPonto(vIva.ToString()) +
                            "," + Funcoes.decimalPonto(vBaseIva.ToString()) +
                            "," + Funcoes.decimalPonto(vmargemIva.ToString()) +
                            "," + Funcoes.decimalPonto(despesas.ToString()) +
                            "," + Funcoes.decimalPonto(CFOP.ToString()) +
                            "," + "'" + CODIGO_REFERENCIA + "'" +
                            "," + Funcoes.decimalPonto(aliquota_icms.ToString()) +
                            "," + Funcoes.decimalPonto(aliquota_iva.ToString()) +
                            ",'" + indice_St + "'" +
                            "," + Funcoes.decimalPonto(redutor_base.ToString()) +
                            "," + Funcoes.decimalPonto(codigo_operacao.ToString()) +
                            "," + Funcoes.decimalPonto(Frete.ToString()) +
                            "," + Num_item +
                            "," + Funcoes.decimalPonto(PISV.ToString()) +
                            "," + Funcoes.decimalPonto(COFINSV.ToString()) +
                            ",'" + NCM.Replace(".", "") + "'" +
                            ",'" + CEST.Replace(".", "") + "'" +
                            "," + "'" + Und + "'" +
                            "," + "'" + Artigo + "'" +
                            "," + Funcoes.decimalPonto(Peso_liquido.ToString()) +
                            "," + Funcoes.decimalPonto(Peso_Bruto.ToString()) +
                            "," + "'" + Tipo + "'" +
                            "," + "'" + CF + "'" +
                            "," + "'" + CSTPIS + "'" +
                            "," + "'" + CSTCOFINS + "'" +
                            "," + Funcoes.decimalPonto(pCredSN.ToString()) +
                            "," + Funcoes.decimalPonto(vCredicmssn.ToString()) +
                            "," + origem +
                            "," + (inativo ? "1" : "0") +
                            "," + Funcoes.decimalPonto(vPBasePisCofins.ToString()) +
                            "," + Funcoes.decimalPonto(vPBasePisCofins.ToString()) +
                            "," + Funcoes.decimalPonto(PISp.ToString()) +
                            "," + Funcoes.decimalPonto(COFINSp.ToString()) +
                            ",'" + indice_St + "'" +
                            ",abs(" + Funcoes.decimalPonto(vBaseICMS.ToString()) + ")" +
                            ",abs(" + Funcoes.decimalPonto(vicms.ToString()) + ")" +
                            ",'" + CST_IPI + "'" +
                            "," + Funcoes.decimalPonto(baseIpi.ToString()) +
                            "," + Funcoes.decimalPonto(vTotTrib.ToString()) +
                            "," + finNFe +
                            "," + Funcoes.decimalPonto(Total.ToString()) +

                            ",'" + (indEscala ? "S" : "N") + "'" +
                            ",'" + cnpj_Fabricante + "'" +
                            ",'" + cBenef + "'" +
                            "," + Funcoes.decimalPonto(vBCFCP.ToString()) +
                            "," + Funcoes.decimalPonto(pFCP.ToString()) +
                            "," + Funcoes.decimalPonto(vFCP.ToString()) +
                            "," + Funcoes.decimalPonto(vBCFCPST.ToString()) +
                            "," + Funcoes.decimalPonto(pFCPST.ToString()) +
                            "," + Funcoes.decimalPonto(vFCPST.ToString()) +
                            "," + Funcoes.decimalPonto(pDevol.ToString()) +
                            "," + (Data_validade.Equals(DateTime.MinValue) ? "null" : "'" + Data_validade.ToString("yyyy-MM-dd") + "'") +
                            "," + serie.ToString() +
                            "," + Funcoes.decimalPonto(vtotal_produto.ToString()) +
                            "," + Funcoes.decimalPonto(vDesconto_valor.ToString()) +
                            ",'" + codigo_centro_custo + "'" +
                            ",'" + Ordem_compra + "'" +
                            ", '" + codigo_produto_ANVISA + "'" +
                            ", '" + motivo_isencao_ANVISA + "'" +
                            ", " + Funcoes.decimalPonto(preco_Maximo_ANVISA.ToString()) +
                            ", " + Funcoes.decimalPonto(redutor_base_ST.ToString()) +
                            ", '" + codigoEmissaoNFe + "'" +
                            ", '" + pedidoItemNumero + "'" +
                            ", '" + pedidoItemSequencia + "'" +
                            ", " + Funcoes.decimalPonto(aliquota_ICMS_Destino.ToString()) +
                            ", " + Funcoes.decimalPonto(valor_Difal_Item.ToString()) +
                            ", " + Funcoes.decimalPonto(Qtde_Devolver.ToString()) +
                          ");";

                Conexao.executarSql(sql, conn, tran);





            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }
    }
}