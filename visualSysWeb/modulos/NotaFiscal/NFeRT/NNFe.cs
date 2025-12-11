using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using NFe.Classes;
using NFe.Utils;
using System.Globalization;
using NFe.Utils.Excecoes;
using visualSysWeb.dao;
using visualSysWeb.code;
using visualSysWeb.modulos.NotaFiscal.NFeRT.Tipos;
using visualSysWeb.modulos.NotaFiscal.NFeRT.Cobranca;
using visualSysWeb.modulos.NotaFiscal.NFeRT.Transporte;

namespace visualSysWeb.modulos.NotaFiscal.NFeRT
{

    [XmlRoot("NFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    //[XmlRoot("NFe")]
    public class NNFe
    {
        [XmlElement("infNFe")]
        public InfNFe infNFe { get; set; }

    }
    #region INFORMAÇÕES NFe (infNFe)
    public class InfNFe
    {
        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlAttribute("versao")]
        public string versao { get; set; }

        [XmlElement("ide")]
        public Identificacao identificacao { get; set; }

        [XmlElement("emit")]
        public Emitente emitente { get; set; }

        [XmlElement("dest")]
        public Destinatario destinatario { get; set; }

        [XmlElement("det")]
        public List<Detalhamento> detalhes { get; set; }

        [XmlElement("total")]
        public Total total { get; set; }

        [XmlElement("transp")]
        public transp transporte { get; set; }
        /// <summary>
        ///     Y01 - Grupo Cobrança
        /// </summary>
        [XmlElement("cobr")]
        public cobr cobr { get; set; }

        [XmlElement("pag")]
        public Pagamento pag { get; set; }

        [XmlElement("infAdic")]
        public informacaoAdicional infAdic { get; set; }

        [XmlElement("infRespTec")]
        public responsavelTecnico responsavelTecnico { get; set; }
    }

    #region IDENTIFICAÇÃO (ide)
    public class Identificacao
    {
        public string cUF { get; set; } = "35";
        public string cNF { get; set; } = "98765432";
        public string natOp { get; set; } = "VENDA CONSUMIDOR FINAL";
        public int mod { get; set; } = 55; //Modelo
        public int serie { get; set; }
        public int nNF { get; set; }
        [XmlIgnore]
        public DateTime dhEmi { get; set; }
        [XmlIgnore]
        public DateTime dhSaiEnt { get; set; }

        [XmlElement("dhEmi")]
        public string dhEmiFormatado
        {
            get => dhEmi.ToString("yyyy-MM-dd'T'HH:mm:sszzz");
            set => dhEmi = DateTime.Parse(value);
        }
        [XmlElement("dhSaiEnt")]
        public string dhSaiEntFormatado
        {
            get => dhSaiEnt.ToString("yyyy-MM-dd'T'HH:mm:sszzz");
            set => dhSaiEnt = DateTime.Parse(value);
        }
        public int tpNF { get; set; } //0=Entrada;1=Saída
        public int idDest { get; set; } = 1; //1=Operação interna;2=Operação interestadual;3=Operação com o exterior
        public int cMunFG { get; set; } //Código do município no IBGE
        public int tpImp { get; set; } = 4; //DANFE NFC-E ou 5=Quando for mensagem eletrônica (email)
        public int tpEmis { get; set; }  //1=Emissão normal; 9=Emissão em contingência
        public string cDV { get; set; }
        public int tpAmb { get; set; } = 2; //Identificação do ambiente - 1=Produção; 2=Homologação
        public int finNFe { get; set; } = 1; //1=NFe-NFCe normal.
        public int indFinal { get; set; } = 1; //Consumidor final
        public int indPres { get; set; } = 1; //Operação presencial
        [XmlElement("indIntermed")]
        public IndicadorIntermediador? indIntermed { get; set; } //Não há intermediador
        [XmlIgnore]
        public bool indIntermedSpecified { get; set; }

        public int procEmi { get; set; } = 0; //Emissão de NF-e/NFC-e com aplicativo do contribuinte
        public string verProc { get; set; } = "NF-e 1.0";
        //[XmlElement("dhCont", Order = 20)]
        //public string DhContFormatado
        //{
        //    get => dhCont?.ToString("yyyy-MM-ddTHH:mm:sszzz");
        //    set => dhCont = DateTime.Parse(value);
        //}
        [XmlIgnore]
        public DateTime? dhCont { get; set; }  // Agora nullable

        [XmlElement("dhCont")]
        public string dhContFormatado
        {
            get => dhCont?.ToString("yyyy-MM-dd'T'HH:mm:sszzz");
            set => dhCont = !string.IsNullOrEmpty(value) ? DateTime.Parse(value) : (DateTime?)null;
        }
        public string xJust { get; set; }

        /// <summary>
        ///     BA01 - Informação de Documentos Fiscais referenciados
        /// </summary>
        [XmlElement("NFref")]
        public List<NFref> NFref { get; set; }

    }
    //Nota Fiscal Referenciada
    public class NFref
    {
        /// <summary>
        ///     BA02 - Chave de acesso da NF-e referenciada
        /// </summary>
        public string refNFe { get; set; }

        /// <summary>
        ///     BA10 - Informações da NF de produtor rural referenciada
        /// </summary>
        public refNFP refNFP { get; set; }

        /// <summary>
        ///     BA19 - Chave de acesso do CT-e referenciado
        /// </summary>
        public string refCTe { get; set; }

    }
    //Nota Fiscal Produtor Rural referenciada
    public class refNFP
    {
        /// <summary>
        ///     BA11 - Código da UF do emitente
        /// </summary>
        public Estado cUF { get; set; }

        /// <summary>
        ///     BA12 - Ano e Mês de emissão da NF-e
        /// </summary>
        public string AAMM { get; set; }

        /// <summary>
        ///     BA13 - CNPJ do emitente
        /// </summary>
        public string CNPJ { get; set; }

        /// <summary>
        ///     BA14 - CPF do emitente
        /// </summary>
        public string CPF { get; set; }

        /// <summary>
        ///     BA15 - IE do emitente
        /// </summary>
        public string IE { get; set; }

        /// <summary>
        ///     BA16 - Modelo do Documento Fiscal
        /// </summary>
        public string mod { get; set; }

        /// <summary>
        ///     BA17 - Série do Documento Fiscal
        /// </summary>
        public int serie { get; set; }

        /// <summary>
        ///     BA18 - Número do Documento Fiscal
        /// </summary>
        public int nNF { get; set; }
    }
    #endregion
    #region EMITENTE (emit)
    public class Emitente
    {
        public string CNPJ { get; set; }
        public string xNome { get; set; }
        public string xFant { get; set; }
        public EnderEmit enderEmit { get; set; }
        public string IE { get; set; }
        public string CRT { get; set; }
    }
    public class EnderEmit
    {
        public string xLgr { get; set; }
        public string nro { get; set; }
        public string xBairro { get; set; }
        public int cMun { get; set; }
        public string xMun { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public string cPais { get; set; }
        public string xPais { get; set; }
        public string fone { get; set; }
    }
    #endregion
    #region DESTINATARIO (dest)
    public class Destinatario
    {
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string xNome { get; set; }
        public EnderDest enderDest { get; set; }
        public string indIEDest { get; set; }
        public string IE { get; set; }
        public string email { get; set; }

    }
    public class EnderDest
    {
        public string xLgr { get; set; }
        public string nro { get; set; }
        public string xBairro { get; set; }
        public int cMun { get; set; }
        public string xMun { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public string cPais { get; set; }
        public string xPais { get; set; }
        public string fone { get; set; }
    }
    #endregion
    #region DETALHAMENTO (det)
    public class Detalhamento
    {
        [XmlAttribute("nItem")]
        public int nItem { get; set; }

        [XmlElement("prod")]
        public Produto produto { get; set; }
        [XmlElement("imposto")]
        public Imposto Imposto { get; set; }
        [XmlElement("impostoDevol")]
        public impostoDevol ImpostoDevol { get; set; }
    }

    public class Produto
    {
        public string cProd { get; set; }        // Código do produto
        public string cEAN { get; set; }         // Código de barras (GTIN)
        public string xProd { get; set; }        // Descrição do produto
        public string NCM { get; set; }          // Código NCM
        public string CEST { get; set; }         // Código CEST (se aplicável)
        public int CFOP { get; set; }         // Código CFOP
        public string uCom { get; set; }         // Unidade comercial
        [XmlIgnore]
        public decimal qCom { get; set; } // Quantidade comercial
        [XmlElement("qCom")]
        public string qComFormatado
        {
            get => qCom.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => qCom = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vUnCom { get; set; } // Valor unitário comercial
        [XmlElement("vUnCom")]
        public string vUnComFormatado
        {
            get => vUnCom.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => vUnCom = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vProd { get; set; } // Valor total bruto do produto
        [XmlElement("vProd")]
        public string vProdFormatado
        {
            get => vProd.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vProd = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }
        public string cEANTrib { get; set; }     // Código de barras tributável (GTIN)
        public string uTrib { get; set; }        // Unidade tributável

        [XmlIgnore]
        public decimal qTrib { get; set; } // Quantidade tributável
        [XmlElement("qTrib")]
        public string qTribFormatado
        {
            get => qTrib.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => qTrib = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vUnTrib { get; set; } // Valor unitário tributável
        [XmlElement("vUnTrib")]
        public string vUnTribFormatado
        {
            get => vUnTrib.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => vUnTrib = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vDesc { get; set; } // Valor do desconto
        [XmlElement("vDesc")]
        public string vDescFormatado
        {
            get => (vDesc > 0 ? vDesc.ToString("F2", System.Globalization.CultureInfo.InvariantCulture) : null);
            set => vDesc = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        public int indTot { get; set; }         // Indica se compõe total da NF (0=Não, 1=Sim)
        [XmlIgnore]
        public decimal vOutro { get; set; } // Valor do desconto
        [XmlElement("vOutro")]
        public string vOutroFormatado
        {
            get => (vOutro > 0 ? vOutro.ToString("F2", System.Globalization.CultureInfo.InvariantCulture) : null);
            set => vOutro = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }
        [XmlIgnore]
        public decimal vFrete { get; set; } // Valor do desconto
        [XmlElement("vFrete")]
        public string vFreteFormatado
        {
            get => (vFrete > 0 ? vOutro.ToString("F2", System.Globalization.CultureInfo.InvariantCulture) : null);
            set => vFrete = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }
        public string xPed { get; set; }
        public string nItemPed { get; set; }
    }
    #region IMPOSTO
    public class Imposto
    {
        [XmlIgnore]
        public decimal vTotTrib { get; set; }
        [XmlElement("vTotTrib")]
        public string vTotTribFormatado
        {
            get => vTotTrib.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vTotTrib = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }
        [XmlElement("ICMS")]
        public ICMS ICMS { get; set; }
        [XmlElement("PIS")]
        public PIS PIS { get; set; }
        [XmlElement("COFINS")]
        public COFINS COFINS { get; set; }
        [XmlElement("ICMSUFDest")]
        public ICMSUFDest ICMSUFDest { get; set; } //Informação do Imposto devolvido
        [XmlElement("IS")]
        public IS IS { get; set; } //Informação do Imposto Seletivo
        [XmlElement("IBSCBS")]
        public IBSCBS IBSCBS { get; set; } //Informação do Imposto Sobre Bens e Serviços e o Imposto Sobre Contribuições

    }
    #region ICMS
    public class ICMS
    {
        [XmlElement("ICMS00")]
        public ICMS00 ICMS00 { get; set; }
        [XmlElement("ICMS10")]
        public ICMS10 ICMS10 { get; set; }
        [XmlElement("ICMS20")]
        public ICMS20 ICMS20 { get; set; }
        [XmlElement("ICMS30")]
        public ICMS30 ICMS30 { get; set; }
        [XmlElement("ICMS40")]
        public ICMS40 ICMS40 { get; set; }
        [XmlElement("ICMS51")]
        public ICMS51 ICMS51 { get; set; }
        [XmlElement("ICMS60")]
        public ICMS60 ICMS60 { get; set; }
        [XmlElement("ICMS70")]
        public ICMS70 ICMS70 { get; set; }
        [XmlElement("ICMS90")]
        public ICMS90 ICMS90 { get; set; }
        [XmlElement("ICMSPart")]
        public ICMSPart ICMSPart { get; set; }
        [XmlElement("ICMSSN101")]
        public ICMSSN101 ICMSSN101 { get; set; }
        [XmlElement("ICMSSN102")]
        public ICMSSN102 ICMSSN102 { get; set; }
        [XmlElement("ICMSSN201")]
        public ICMSSN201 ICMSSN201 { get; set; }
        [XmlElement("ICMSSN202")]
        public ICMSSN202 ICMSSN202 { get; set; }
        [XmlElement("ICMSSN500")]
        public ICMSSN500 ICMSSN500 { get; set; }
        [XmlElement("ICMSSN900")]
        public ICMSSN900 ICMSSN900 { get; set; }
    }

    public class ICMS00
    {
        public string orig { get; set; }
        public string CST { get; set; }
        public int modBC { get; set; }
        [XmlIgnore]
        public decimal vBC { get; set; }
        [XmlElement("vBC")]
        public string vBCFormatado
        {
            get => vBC.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBC = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pICMS { get; set; }
        [XmlElement("pICMS")]
        public string pICMSFormatado
        {
            get => pICMS.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pICMS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vICMS { get; set; }
        [XmlElement("vICMS")]
        public string vICMSFormatado
        {
            get => vICMS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    public class ICMS10
    {
        public string orig { get; set; }
        public string CST { get; set; }
        public int modBC { get; set; }
        public int modBCST { get; set; }
        [XmlIgnore]
        public decimal vBC { get; set; }
        [XmlElement("vBC")]
        public string vBCFormatado
        {
            get => vBC.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBC = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pICMS { get; set; }
        [XmlElement("pICMS")]
        public string pICMSFormatado
        {
            get => pICMS.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pICMS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vICMS { get; set; }
        [XmlElement("vICMS")]
        public string vICMSFormatado
        {
            get => vICMS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pMVAST { get; set; }
        [XmlElement("pMVAST")]
        public string pMVASTFormatado
        {
            get => pMVAST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pMVAST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pRedBCST { get; set; }
        [XmlElement("pRedBCST")]
        public string pRedBCSTFormatado
        {
            get => pRedBCST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pRedBCST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vBCST { get; set; }
        [XmlElement("vBCST")]
        public string vBCSTFormatado
        {
            get => vBCST.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBCST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pICMSST { get; set; }
        [XmlElement("pICMSST")]
        public string pICMSSTFormatado
        {
            get => pICMSST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pICMSST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vICMSST { get; set; }
        [XmlElement("vICMSST")]
        public string vICMSSTFormatado
        {
            get => vICMSST.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMSST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    public class ICMS20
    {
        public string orig { get; set; }
        public string CST { get; set; }
        public int modBC { get; set; }
        [XmlIgnore]
        public decimal pRedBC { get; set; }
        [XmlElement("pRedBC")]
        public string pRedBCFormatado
        {
            get => pRedBC.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pRedBC = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vBC { get; set; }
        [XmlElement("vBC")]
        public string vBCFormatado
        {
            get => vBC.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBC = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pICMS { get; set; }
        [XmlElement("pICMS")]
        public string pICMSFormatado
        {
            get => pICMS.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pICMS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vICMS { get; set; }
        [XmlElement("vICMS")]
        public string vICMSFormatado
        {
            get => vICMS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    public class ICMS30
    {
        public string orig { get; set; }
        public string CST { get; set; }
        public int modBCST { get; set; }
        [XmlIgnore]
        public decimal pMVAST { get; set; }
        [XmlElement("pMVAST")]
        public string pMVASTFormatado
        {
            get => pMVAST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pMVAST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pRedBCST { get; set; }
        [XmlElement("pRedBCST")]
        public string pRedBCSTFormatado
        {
            get => pRedBCST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pRedBCST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vBCST { get; set; }
        [XmlElement("vBCST")]
        public string vBCSTFormatado
        {
            get => vBCST.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBCST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pICMSST { get; set; }
        [XmlElement("pICMSST")]
        public string pICMSSTFormatado
        {
            get => pICMSST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pICMSST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vICMSST { get; set; }
        [XmlElement("vICMSST")]
        public string vICMSSTFormatado
        {
            get => vICMSST.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMSST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    public class ICMS40
    {
        public string orig { get; set; }
        public string CST { get; set; }
    }

    public class ICMS51
    {
        public string orig { get; set; }
        public string CST { get; set; }
        [XmlIgnore]
        public decimal pRedBC { get; set; }
        [XmlElement("pRedBC")]
        public string pRedBCFormatado
        {
            get => pRedBC.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pRedBC = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vBC { get; set; }
        [XmlElement("vBC")]
        public string vBCFormatado
        {
            get => vBC.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBC = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pICMS { get; set; }
        [XmlElement("pICMS")]
        public string pICMSFormatado
        {
            get => pICMS.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pICMS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vICMSOp { get; set; }
        [XmlElement("vICMSOp")]
        public string vICMSOpFormatado
        {
            get => vICMSOp.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMSOp = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pDif { get; set; }
        [XmlElement("pDif")]
        public string pDifFormatado
        {
            get => pDif.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pDif = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vICMSDif { get; set; }
        [XmlElement("vICMSDif")]
        public string vICMSDifFormatado
        {
            get => vICMSDif.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMSDif = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vICMS { get; set; }
        [XmlElement("vICMS")]
        public string vICMSFormatado
        {
            get => vICMS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    public class ICMS60
    {
        public string orig { get; set; }
        public string CST { get; set; }
    }

    public class ICMS70
    {
        public string orig { get; set; }
        public string CST { get; set; }
        public int modBC { get; set; }
        public int modBCST { get; set; }
        [XmlIgnore]
        public decimal pRedBC { get; set; }
        [XmlElement("pRedBC")]
        public string pRedBCFormatado
        {
            get => pRedBC.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pRedBC = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vBC { get; set; }
        [XmlElement("vBC")]
        public string vBCFormatado
        {
            get => vBC.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBC = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pICMS { get; set; }
        [XmlElement("pICMS")]
        public string pICMSFormatado
        {
            get => pICMS.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pICMS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vICMS { get; set; }
        [XmlElement("vICMS")]
        public string vICMSFormatado
        {
            get => vICMS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pMVAST { get; set; }
        [XmlElement("pMVAST")]
        public string pMVASTFormatado
        {
            get => pMVAST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pMVAST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pRedBCST { get; set; }
        [XmlElement("pRedBCST")]
        public string pRedBCSTFormatado
        {
            get => pRedBCST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pRedBCST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vBCST { get; set; }
        [XmlElement("vBCST")]
        public string vBCSTFormatado
        {
            get => vBCST.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBCST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pICMSST { get; set; }
        [XmlElement("pICMSST")]
        public string pICMSSTFormatado
        {
            get => pICMSST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pICMSST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vICMSST { get; set; }
        [XmlElement("vICMSST")]
        public string vICMSSTFormatado
        {
            get => vICMSST.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMSST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

    }

    public class ICMS90
    {
        public string orig { get; set; }
        public string CST { get; set; }
        public int modBC { get; set; }
        public int modBCST { get; set; }
        [XmlIgnore]
        public decimal pRedBC { get; set; }
        [XmlElement("pRedBC")]
        public string pRedBCFormatado
        {
            get => pRedBC.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pRedBC = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vBC { get; set; }
        [XmlElement("vBC")]
        public string vBCFormatado
        {
            get => vBC.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBC = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pICMS { get; set; }
        [XmlElement("pICMS")]
        public string pICMSFormatado
        {
            get => pICMS.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pICMS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vICMS { get; set; }
        [XmlElement("vICMS")]
        public string vICMSFormatado
        {
            get => vICMS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pMVAST { get; set; }
        [XmlElement("pMVAST")]
        public string pMVASTFormatado
        {
            get => pMVAST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pMVAST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pRedBCST { get; set; }
        [XmlElement("pRedBCST")]
        public string pRedBCSTFormatado
        {
            get => pRedBCST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pRedBCST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vBCST { get; set; }
        [XmlElement("vBCST")]
        public string vBCSTFormatado
        {
            get => vBCST.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBCST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pICMSST { get; set; }
        [XmlElement("pICMSST")]
        public string pICMSSTFormatado
        {
            get => pICMSST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pICMSST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vICMSST { get; set; }
        [XmlElement("vICMSST")]
        public string vICMSSTFormatado
        {
            get => vICMSST.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMSST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

    }

    public class ICMSPart
    {
        public string orig { get; set; }
        public string CST { get; set; }
        public int modBC { get; set; }
        public int modBCST { get; set; }
        [XmlIgnore]
        public decimal pRedBC { get; set; }
        [XmlElement("pRedBC")]
        public string pRedBCFormatado
        {
            get => pRedBC.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pRedBC = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vBC { get; set; }
        [XmlElement("vBC")]
        public string vBCFormatado
        {
            get => vBC.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBC = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pICMS { get; set; }
        [XmlElement("pICMS")]
        public string pICMSFormatado
        {
            get => pICMS.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pICMS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vICMS { get; set; }
        [XmlElement("vICMS")]
        public string vICMSFormatado
        {
            get => vICMS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pMVAST { get; set; }
        [XmlElement("pMVAST")]
        public string pMVASTFormatado
        {
            get => pMVAST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pMVAST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pRedBCST { get; set; }
        [XmlElement("pRedBCST")]
        public string pRedBCSTFormatado
        {
            get => pRedBCST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pRedBCST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vBCST { get; set; }
        [XmlElement("vBCST")]
        public string vBCSTFormatado
        {
            get => vBCST.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBCST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pICMSST { get; set; }
        [XmlElement("pICMSST")]
        public string pICMSSTFormatado
        {
            get => pICMSST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pICMSST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vICMSST { get; set; }
        [XmlElement("vICMSST")]
        public string vICMSSTFormatado
        {
            get => vICMSST.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMSST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pBCOp { get; set; }
        [XmlElement("pBCOp")]
        public string pBCOpFormatado
        {
            get => pBCOp.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pBCOp = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal UFST { get; set; }
        [XmlElement("UFST")]
        public string UFSTFormatado
        {
            get => UFST.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => UFST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

    }

    public class ICMSSN101
    {
        public string orig { get; set; }
        public string CSOSN { get; set; }
        [XmlIgnore]
        public decimal pCredSN { get; set; }
        [XmlElement("pCredSN")]
        public string pCredSNFormatado
        {
            get => pCredSN.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pCredSN = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vCredICMSSN { get; set; }
        [XmlElement("vCredICMSSN")]
        public string vCredICMSSNFormatado
        {
            get => vCredICMSSN.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vCredICMSSN = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    public class ICMSSN102
    {
        public string orig { get; set; }
        public string CSOSN { get; set; }
    }

    public class ICMSSN201
    {
        public string orig { get; set; }
        public string CSOSN { get; set; }
        public int modBCST { get; set; }
        [XmlIgnore]
        public decimal pMVAST { get; set; }
        [XmlElement("pMVAST")]
        public string pMVASTFormatado
        {
            get => pMVAST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pMVAST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pRedBCST { get; set; }
        [XmlElement("pRedBCST")]
        public string pRedBCSTFormatado
        {
            get => pRedBCST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pRedBCST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vBCST { get; set; }
        [XmlElement("vBCST")]
        public string vBCSTFormatado
        {
            get => vBCST.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBCST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pICMSST { get; set; }
        [XmlElement("pICMSST")]
        public string pICMSSTFormatado
        {
            get => pICMSST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pICMSST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vICMSST { get; set; }
        [XmlElement("vICMSST")]
        public string vICMSSTFormatado
        {
            get => vICMSST.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMSST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pCredSN { get; set; }
        [XmlElement("pCredSN")]
        public string pCredSNFormatado
        {
            get => pCredSN.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pCredSN = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vCredICMSSN { get; set; }
        [XmlElement("vCredICMSSN")]
        public string vCredICMSSNFormatado
        {
            get => vCredICMSSN.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vCredICMSSN = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

    }

    public class ICMSSN202
    {
        public string orig { get; set; }
        public string CSOSN { get; set; }
        public int modBCST { get; set; }
        [XmlIgnore]
        public decimal pMVAST { get; set; }
        [XmlElement("pMVAST")]
        public string pMVASTFormatado
        {
            get => pMVAST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pMVAST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pRedBCST { get; set; }
        [XmlElement("pRedBCST")]
        public string pRedBCSTFormatado
        {
            get => pRedBCST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pRedBCST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vBCST { get; set; }
        [XmlElement("vBCST")]
        public string vBCSTFormatado
        {
            get => vBCST.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBCST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pICMSST { get; set; }
        [XmlElement("pICMSST")]
        public string pICMSSTFormatado
        {
            get => pICMSST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pICMSST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vICMSST { get; set; }
        [XmlElement("vICMSST")]
        public string vICMSSTFormatado
        {
            get => vICMSST.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMSST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

    }

    public class ICMSSN500
    {
        public string orig { get; set; }
        public string CSOSN { get; set; }
    }

    public class ICMSSN900
    {
        public string orig { get; set; }
        public string CSOSN { get; set; }
        public int modBC { get; set; }
        public int modBCST { get; set; }
        [XmlIgnore]
        public decimal pRedBC { get; set; }
        [XmlElement("pRedBC")]
        public string pRedBCFormatado
        {
            get => pRedBC.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pRedBC = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vBC { get; set; }
        [XmlElement("vBC")]
        public string vBCFormatado
        {
            get => vBC.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBC = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pICMS { get; set; }
        [XmlElement("pICMS")]
        public string pICMSFormatado
        {
            get => pICMS.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pICMS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vICMS { get; set; }
        [XmlElement("vICMS")]
        public string vICMSFormatado
        {
            get => vICMS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pMVAST { get; set; }
        [XmlElement("pMVAST")]
        public string pMVASTFormatado
        {
            get => pMVAST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pMVAST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pRedBCST { get; set; }
        [XmlElement("pRedBCST")]
        public string pRedBCSTFormatado
        {
            get => pRedBCST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pRedBCST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vBCST { get; set; }
        [XmlElement("vBCST")]
        public string vBCSTFormatado
        {
            get => vBCST.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBCST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pICMSST { get; set; }
        [XmlElement("pICMSST")]
        public string pICMSSTFormatado
        {
            get => pICMSST.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pICMSST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vICMSST { get; set; }
        [XmlElement("vICMSST")]
        public string vICMSSTFormatado
        {
            get => vICMSST.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMSST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pCredSN { get; set; }
        [XmlElement("pCredSN")]
        public string pCredSNFormatado
        {
            get => pCredSN.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pCredSN = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vCredICMSSN { get; set; }
        [XmlElement("vCredICMSSN")]
        public string vCredICMSSNFormatado
        {
            get => vCredICMSSN.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vCredICMSSN = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

    }
    #endregion

    #region PIS
    public class PIS
    {
        [XmlElement("PISAliq")]
        public PISAliq PISAliq { get; set; }
        [XmlElement("PISQtde")]
        public PISQtde PISQtde { get; set; }
        [XmlElement("PISNT")]
        public PISNT PISNT { get; set; }
        [XmlElement("PISOutr")]
        public PISOutr PISOutr { get; set; }
    }

    public class PISAliq
    {
        public string CST { get; set; }
        [XmlIgnore]
        public decimal vBC { get; set; }
        [XmlElement("vBC")]
        public string vBCFormatado
        {
            get => vBC.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBC = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pPIS { get; set; }
        [XmlElement("pPIS")]
        public string pPISFormatado
        {
            get => pPIS.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pPIS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vPIS { get; set; }
        [XmlElement("vPIS")]
        public string vPISFormatado
        {
            get => vPIS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vPIS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

    }

    public class PISQtde
    {
        public string CST { get; set; }
        [XmlIgnore]
        public decimal qBCProd { get; set; }
        [XmlElement("qBCProd")]
        public string qBCProdFormatado
        {
            get => qBCProd.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => qBCProd = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vAliqProd { get; set; }
        [XmlElement("vAliqProd")]
        public string vAliqProdFormatado
        {
            get => vAliqProd.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vAliqProd = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vPIS { get; set; }
        [XmlElement("vPIS")]
        public string vPISFormatado
        {
            get => vPIS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vPIS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

    }

    public class PISNT
    {
        public string CST { get; set; }
    }

    public class PISOutr
    {
        public string CST { get; set; }
        
        [XmlIgnore]
        public decimal vBC { get; set; }
        [XmlElement("vBC")]
        public string vBCFormatado
        {
            get => vBC.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBC = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pPIS { get; set; }
        [XmlElement("pPIS")]
        public string pPISFormatado
        {
            get => pPIS.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pPIS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal qBCProd { get; set; }
        [XmlElement("qBCProd")]
        public string qBCProdFormatado
        {
            get => qBCProd.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => qBCProd = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vAliqProd { get; set; }
        [XmlElement("vAliqProd")]
        public string vAliqProdFormatado
        {
            get => vAliqProd.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vAliqProd = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vPIS { get; set; }
        [XmlElement("vPIS")]
        public string vPISFormatado
        {
            get => vPIS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vPIS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }
        

    }
    #endregion

    #region COFINS
    public class COFINS
    {
        [XmlElement("COFINSAliq")]
        public COFINSAliq COFINSAliq { get; set; }
        [XmlElement("COFINSQtde")]
        public COFINSQtde COFINSQtde { get; set; }
        [XmlElement("COFINSNT")]
        public COFINSNT COFINSNT { get; set; }
        [XmlElement("COFINSOutr")]
        public COFINSOutr COFINSOutr { get; set; }
    }

    public class COFINSAliq
    {
        public string CST { get; set; }
        [XmlIgnore]
        public decimal vBC { get; set; }
        [XmlElement("vBC")]
        public string vBCFormatado
        {
            get => vBC.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBC = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal pCOFINS { get; set; }
        [XmlElement("pCOFINS")]
        public string pCOFINSFormatado
        {
            get => pCOFINS.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            set => pCOFINS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vCOFINS { get; set; }
        [XmlElement("vCOFINS")]
        public string vCOFINSFormatado
        {
            get => vCOFINS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vCOFINS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

    }

    public class COFINSQtde
    {
        public string CST { get; set; }
        [XmlIgnore]
        public decimal qBCProd { get; set; }
        [XmlElement("qBCProd")]
        public string qBCProdFormatado
        {
            get => qBCProd.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => qBCProd = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vAliqProd { get; set; }
        [XmlElement("vAliqProd")]
        public string vAliqProdFormatado
        {
            get => vAliqProd.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vAliqProd = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vCOFINS { get; set; }
        [XmlElement("vCOFINS")]
        public string vCOFINSFormatado
        {
            get => vCOFINS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vCOFINS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

    }

    public class COFINSNT
    {
        public string CST { get; set; }
    }

    public class COFINSOutr
    {
        public string CST { get; set; }
       
       [XmlIgnore]

       public decimal vBC { get; set; }
       [XmlElement("vBC")]
       public string vBCFormatado
       {
           get => vBC.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
           set => vBC = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
       }

       [XmlIgnore]
       public decimal pCOFINS { get; set; }
       [XmlElement("pCOFINS")]
       public string pCOFINSFormatado
       {
           get => pCOFINS.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
           set => pCOFINS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
       }

       [XmlIgnore]
       public decimal qBCProd { get; set; }
       [XmlElement("qBCProd")]
       public string qBCProdFormatado
       {
           get => qBCProd.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
           set => qBCProd = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
       }

       [XmlIgnore]
       public decimal vAliqProd { get; set; }
       [XmlElement("vAliqProd")]
       public string vAliqProdFormatado
       {
           get => vAliqProd.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
           set => vAliqProd = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
       }

       [XmlIgnore]
       public decimal vCOFINS { get; set; }
       [XmlElement("vCOFINS")]
       public string vCOFINSFormatado
       {
           get => vCOFINS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
           set => vCOFINS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
       }
       

    }
    #endregion
    #endregion
    #endregion
    #region TOTAL (total)
    public class Total
    {
        [XmlIgnore]
        public decimal _vNFTot;

        [XmlIgnore]
        public filialDAO Loja;

        [XmlElement("ICMSTot")]
        public ICMSTot ICMSTot { get; set; }


        public IBSCBSTot IBSCBSTot { get; set; }
     
        public string vNFTot
        {
            get => (Loja.CRT.ToString().Equals("3") ? _vNFTot.ToString("F2", System.Globalization.CultureInfo.InvariantCulture) : null);
            set => _vNFTot = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

    }

    public class ICMSTot
    {
        [XmlIgnore]
        public decimal vBC { get; set; } = 0.00m;
        [XmlElement("vBC")]
        public string vBCFormatado
        {
            get => vBC.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBC = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }
        [XmlIgnore]
        public decimal vICMS { get; set; } = 0.00m;
        [XmlElement("vICMS")]
        public string vICMSFormatado
        {
            get => vICMS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vICMSDeson { get; set; } = 0m;
        [XmlElement("vICMSDeson")]
        public string vICMSDesonFormatado
        {
            get => vICMSDeson.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMSDeson = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        //**
        // Inicio Apenas qdo houver DIFAL
        //

        [XmlIgnore]
        public decimal vFCPUFDest { get; set; } = 0m;
        [XmlElement("vFCPUFDest")]
        public string vFCPUFDestFormatado
        {
            get => vFCPUFDest.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vFCPUFDest = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }
        [XmlIgnore]
        public bool vFCPUFDestSpecified { get ; set; }

        [XmlIgnore]
        public decimal vICMSUFDest { get; set; } = 0m;
        [XmlElement("vICMSUFDest")]
        public string vICMSUFDestFormatado
        {
            get => vICMSUFDest.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMSUFDest = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }
        [XmlIgnore]
        public bool vICMSUFDestSpecified { get; set; }

        [XmlIgnore]
        public decimal vICMSUFRemet { get; set; } = 0m;
        [XmlElement("vICMSUFRemet")]
        public string vICMSUFRemetFormatado
        {
            get => vICMSUFRemet.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vICMSUFRemet = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }
        [XmlIgnore]
        public bool vICMSUFRemetSpecified { get; set; }
        
        //** Término DIFAL


        [XmlIgnore]
        public decimal vFCP { get; set; } = 0m;
        [XmlElement("vFCP")]
        public string vFCPFormatado
        {
            get => vFCP.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vFCP = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vBCST { get; set; } = 0m;
        [XmlElement("vBCST")]
        public string vBCSTFormatado
        {
            get => vBCST.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vBCST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vST { get; set; } = 0m;
        [XmlElement("vST")]
        public string vSTFormatado
        {
            get => vST.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vFCPST { get; set; } = 0m;
        [XmlElement("vFCPST")]
        public string vFCPSTFormatado
        {
            get => vFCPST.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vFCPST = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vFCPSTRet { get; set; } = 0m;
        [XmlElement("vFCPSTRet")]
        public string vFCPSTRetFormatado
        {
            get => vFCPSTRet.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vFCPSTRet = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vProd { get; set; } = 0m;
        [XmlElement("vProd")]
        public string vProdFormatado
        {
            get => vProd.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vProd = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vFrete { get; set; } = 0m;
        [XmlElement("vFrete")]
        public string vFreteFormatado
        {
            get => vFrete.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vFrete = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vSeg { get; set; } = 0m;
        [XmlElement("vSeg")]
        public string vSegFormatado
        {
            get => vSeg.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vSeg = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vDesc { get; set; } = 0m;
        [XmlElement("vDesc")]
        public string vDescFormatado
        {
            get => vDesc.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vDesc = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vII { get; set; } = 0m;
        [XmlElement("vII")]
        public string vIIFormatado
        {
            get => vII.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vII = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vIPI { get; set; } = 0m;
        [XmlElement("vIPI")]
        public string vIPIFormatado
        {
            get => vIPI.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vIPI = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vIPIDevol { get; set; } = 0m;
        [XmlElement("vIPIDevol")]
        public string vIPIDevolFormatado
        {
            get => vIPIDevol.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vIPIDevol = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vPIS { get; set; } = 0m;
        [XmlElement("vPIS")]
        public string vPISFormatado
        {
            get => vPIS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vPIS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vCOFINS { get; set; } = 0m;
        [XmlElement("vCOFINS")]
        public string vCOFINSFormatado
        {
            get => vCOFINS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vCOFINS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vOutro { get; set; } = 0m;
        [XmlElement("vOutro")]
        public string vOutroFormatado
        {
            get => vOutro.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vOutro = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vNF { get; set; } = 0m;
        [XmlElement("vNF")]
        public string vNFFormatado
        {
            get => vNF.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vNF = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public decimal vTotTrib { get; set; } = 0m;
        [XmlElement("vTotTrib")]
        public string vTotTribFormatado
        {
            get => vTotTrib.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vTotTrib = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
    #endregion
    #region PAGAMENTO (pag)
    public class Pagamento
    {
        [XmlElement("detPag")]
        public List<DetPag> DetPag { get; set; }
        //[XmlIgnore]
        //public decimal? vTroco { get; set; }
        //[XmlElement("vTroco")]
        //public string vTrocoFormatado
        //{
        //    get => vTroco.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
        //    set => vTroco = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        //}
        //[XmlIgnore]
        //public bool vTrocoSpecified
        //{
        //    get { return vTroco.HasValue; }
        //}
    }

    public class DetPag
    {
        public int? indPag { get; set; } // Indicador da forma de pagamento (0=pagamento à vista; 1=pagamento a prazo)
        [XmlIgnore]
        public bool indPagSpecified
        {
            get { return indPag.HasValue; }
        }
        public string tPag { get; set; }   // Meio de pagamento (01=Dinheiro, 02=Cheque, etc.)
        [XmlIgnore]
        public decimal vPag { get; set; }

        [XmlElement("vPag")]
        public string vPagFormatado
        {
            get => vPag.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            set => vPag = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }
        [XmlElement("card")]
        public Card Card { get; set; }     // Informações do cartão (opcional)
    }

    public class Card
    {
        public string tpIntegra { get; set; }  // Tipo de integração (1=Integrado, 2=Não integrado)
        public string CNPJ { get; set; }       // CNPJ da credenciadora
        public string tBand { get; set; }      // Bandeira do cartão
        public string cAut { get; set; }       // Código de autorização da operação
    }
    #endregion
    #region INFORMAÇÕES ADICIONAIS (infAdic)
    public class informacaoAdicional
    {
        public string infCpl { get; set; }
        public string xJust { get; set; }
    }
    #endregion
    #region INFORMAÇÕES RESPONSÁVEL TÉCNICO (infRespTec)
    public class responsavelTecnico
    {
        public string CNPJ { get; set; }           // CNPJ do responsável técnico
        public string xContato { get; set; }       // Nome da pessoa para contato
        public string email { get; set; }          // E-mail de contato
        public string fone { get; set; }           // Telefone de contato
        public string idCSRT { get; set; }         // Identificador do CSRT (se utilizado)
        public string hashCSRT { get; set; }       // Hash do CSRT (se utilizado)
    }
    #endregion
    #endregion
  
    public class FuncoesNFe
    {
        //** Funções privadas
        public static string erros = "";
        public static string chaveNFe(Identificacao ide)
        {
            string idChave = "";
            int nFor = 0;
            int nPeso = 0;
            int nSoma = 0;
            int nResto = 0;
            int nDV = 0;
            string cDV = "";
            try
            {
                StringBuilder strChave = new StringBuilder();
                strChave.Append(ide.cUF);
                strChave.Append(DateTime.Now.ToString("yyyyMM").Substring(2));
                strChave.Append("02340366000216".Replace(".", "").Replace("-", "").Replace("/", ""));
                strChave.Append(ide.mod);
                strChave.Append(ide.serie.ToString("000"));
                strChave.Append(ide.nNF.ToString("000000000"));
                strChave.Append(ide.tpEmis.ToString("0"));
                strChave.Append(ide.cNF);

                nPeso = 4;
                int cont = strChave.Length;

                for (nFor = 0; nFor < 43; nFor++)
                {
                    nSoma += (int.Parse(strChave.ToString().Substring(nFor, 1)) * nPeso);
                    if (nPeso == 2)
                        nPeso = 9;
                    else
                        nPeso -= 1;

                }
                nResto = nSoma % 11;

                if (nResto == 1 || nResto == 0)
                    nDV = 0;
                else
                    nDV = 11 - nResto;

                cDV = nDV.ToString();
                strChave.Append(nDV.ToString().Trim());

                return strChave.ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static bool validarXML(string caminhoXML, string caminhoSchemas)
        {
            var falhas = new StringBuilder();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;

            var schemas = new XmlSchemaSet();
            schemas.XmlResolver = new XmlUrlResolver();

            settings.Schemas = schemas;
            schemas.Add("http://www.portalfiscal.inf.br/nfe", caminhoSchemas + "nfe_v4.00.xsd");

            settings.ValidationEventHandler += delegate (object sender, ValidationEventArgs args)
            {
                falhas.AppendLine($"[{args.Severity}] - {args.Message} {args.Exception?.Message} na linha {args.Exception.LineNumber} posição {args.Exception.LinePosition} em {args.Exception.SourceUri}".ToString());
            };

            // cria um leitor para validação
            var validator = XmlReader.Create(caminhoXML, settings);
            try
            {
                // Faz a leitura de todos os dados XML
                while (validator.Read())
                {
                }
            }
            catch
            {
            }
            finally
            {
                validator.Close();
            }

            if (falhas.Length > 0)
            {
                throw new ValidacaoSchemaException($"Ocorreu o seguinte erro durante a validação XML: {Environment.NewLine}{falhas}","");
            }

            return true;

            //settings.Schemas.Add("http://www.portalfiscal.inf.br/nfe", Path.Combine(caminhoSchemas, "nfe_v4.00.xsd"));
            //settings.ValidationEventHandler += new ValidationEventHandler(ValidaEvento);

            //using (XmlReader reader = XmlReader.Create(caminhoXML, settings))
            //{
            //    try
            //    {
            //        while (reader.Read()) { }
            //        return string.IsNullOrEmpty(erros);
            //    }
            //    catch (Exception ex)
            //    {
            //        erros += ex.Message;
            //        return false;
            //    }
            //}
        }
        private static void ValidaEvento(object sender, ValidationEventArgs e)
        {
            erros += $"[Erro] {e.Message}\n";
        }


        public static string ObterErros()
        {
            
            return erros;
            
        }
        public static string ObterStringDeHex(string hex)
        {
            if (string.IsNullOrEmpty(hex))
                return string.Empty;

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return Encoding.UTF8.GetString(bytes);
        }
        public static string limparerros()
        {
            erros = "";
            return erros;
        }
    }

}

