using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class nf_itemDAO
    {
        public String Filial { get; set; }
        public String Codigo { get; set; }
        public String Cliente_Fornecedor { get; set; }
        public String Tipo_NF { get; set; }
        public String PLU { get; set; }
        public bool recalcular = true;
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
        private Decimal vQtde = 0;
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
                    Unitario = (TotalProduto / (value * Embalagem));
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
                        Unitario = (TotalProduto / (Qtde * value));
                    }
                }
                vEmbalagem = value;
            }
        }
        public Decimal Unitario = 0;
        public Decimal Desconto = 0;
        public Decimal DescontoValor
        {
            get
            {
                if (Desconto == 0)
                    return 0;
                else
                    return (TotalProduto * Desconto) / 100;
            }
            set
            {
                if (value <= 0)
                    Desconto = 0;
                else
                    Desconto = (value / TotalProduto) * 100;
            }
        }
        public Decimal vTotTrib = 0;
        public Decimal TotTrib
        {
            get
            {
                Decimal vTot = 0;
                switch (codigo_operacao.ToString())
                {
                    case "5102":
                    case "5405":
                    case "5402":
                    case "5403":
                    case "6102":
                    case "6405":
                    case "6401":
                    case "6403":
                        String strValor = Conexao.retornaUmValor("Select ISNULL(aliquota_imposto,0) from Imposto_Nota where NCM = '" + NCM + "'", null);
                        if (!strValor.Equals(""))
                        {
                            Decimal porcTrib = Decimal.Parse(strValor);
                            vTotTrib = Decimal.Round((vtotal * porcTrib) / 100, 2);
                            vTot = vTotTrib;
                        }
                        break;
                }
                return vTot;

            }
        }
        public Decimal TotalProduto
        {
            get
            {
                return (Qtde * vEmbalagem) * Unitario;
            }
        }
        public Decimal vtotal = 0;
        public Decimal Total
        {
            get
            {

                if (vtotal == 0)
                {
                    return (TotalProduto - DescontoValor) + despesas + vIpiv + vIva;
                }
                else
                {
                    return vtotal;
                }
            }
            set
            {
                vtotal = value;
            }
        }
        public Decimal TotalCustoUnitario
        {
            get
            {
                if (Total != 0)
                {
                    return (((TotalProduto - DescontoValor) + despesas + vIpiv + vIva) / (Qtde * Embalagem));
                }
                else
                    return 0;
            }
        }
        public Decimal TotalCusto
        {
            get
            {
                return (TotalProduto - DescontoValor) + despesas + IPIV + vIva;
            }
            set { }
        }
        public Decimal porcIPI = 0;
        public Decimal IPI
        {
            get
            {
                return porcIPI;
            }
            set
            {
                Decimal vIpi = 0;
                if (value != 0)
                {
                    //(item.Total * (item.porcIPI) / 100)
                    vIpi = ((TotalProduto - DescontoValor) * value) / 100;
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
                if (vIpiv == 0 && porcIPI > 0)
                    vIpiv = ((TotalProduto - Desconto) * porcIPI) / 100;
                return vIpiv;
            }
            set
            {

                Decimal pIPI = 0;
                if (value != 0)
                {
                    pIPI = (value / (TotalProduto - DescontoValor)) * 100;
                }
                if (!pIPI.Equals(porcIPI))
                {
                    porcIPI = pIPI;

                }
                vIpiv = value;


            }
        }
        private String strDescricao = "";
        public String Descricao
        {
            get
            {
                if (!PLU.Equals("") && strDescricao.Equals(""))
                {
                    strDescricao = Conexao.retornaUmValor("Select descricao from mercadoria where plu='" + PLU + "'", new User()).Replace("'", "");

                }

                return strDescricao;
            }
            set
            {
                if (value.Length > 40)
                    strDescricao = value.Substring(0, 40).Replace("'", "");
                else
                    strDescricao = value.Replace("'", "");
            }
        }
        private String strEan = "";
        public String ean
        {
            get
            {
                if (!PLU.Equals(""))
                    return Conexao.retornaUmValor("Select ean from ean where plu='" + PLU + "'", new User());
                else
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
                if (vmargemIva == 0 && vIva == 0)
                    return 0;
                else if (vAliquota_iva == 0)
                    return aliquota_icms;
                else
                    return vAliquota_iva;

            }
        }
        public Decimal CalculoIva
        {
            get
            {

                if (aliquota_iva != 0 && baseICMS != 0)
                {

                    vPIva = CalculoPIva;
                    vBaseIva = CalculoBase_iva;

                    Decimal vcontiva = ((vBaseIva * aliquota_iva) / 100) - ((baseICMS * aliquota_icms) / 100);
                    if (vcontiva < 0)
                        return 0;
                    else
                    {
                        vIva = vcontiva;
                        return vcontiva;
                    }
                }
                else
                    return 0;
            }
            /*
            set
            {
                vIva = value;
                if (recalcular)
                {
                    if (value == 0)
                    {
                        if (margem_iva != 0)
                            margem_iva = 0;
                    }
                    else
                    {

                        Decimal vMargem = (value / base_iva) * 100;
                        if (margem_iva != vMargem)
                            margem_iva = vMargem;
                    }
                }

            }
            */
        }
        public Decimal vBaseIva = 0;
        public Decimal CalculoBase_iva
        { // base icms st ?
            get
            {

                if (vmargemIva != 0)
                {
                    vBaseIva = ((((TotalProduto - DescontoValor) + IPIV + despesas)) + vPIva);
                    return vBaseIva;
                }
                else
                {
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

                Decimal vBase = ((TotalProduto - DescontoValor) + IPIV + despesas);
                if (vmargemIva <= 0)
                    return 0;
                else
                {
                    vPIva = (vBase * vmargemIva) / 100;
                    return vPIva;
                }
            }
        }
        public Decimal vmargemIva = 0;
        public Decimal CalculoMargem_iva
        {
            get
            {
                if (vIva > 0 && baseICMS > 0)
                {
                    Decimal valoricmsiva = (vIva + ((baseICMS * aliquota_icms) / 100));
                    Decimal valorBaseIva = (valoricmsiva / aliquota_iva) * 100;
                    Decimal valorProduto = ((TotalProduto - DescontoValor) + IPIV + despesas);
                    Decimal valorPorcIva = valorBaseIva - valorProduto;
                    vmargemIva = (valorPorcIva / valorProduto) * 100;
                    return vmargemIva;
                }
                else
                {
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
        public Decimal despesas { get; set; }
        public Decimal CFOP { get; set; }
        public String CODIGO_REFERENCIA { get; set; }
        public Decimal aliquota_icms = 0;
        public Decimal baseICMS
        {
            get
            {
                if (aliquota_icms > 0)
                {
                    Decimal vBase = (TotalProduto - DescontoValor);
                    if (vBase == 0)
                        return 0;
                    else if (redutor_base != 0)
                        return (vBase - ((vBase * redutor_base) / 100)) + despesas;
                    else
                        return vBase + despesas;
                }
                else
                {
                    return 0;
                }
            }
        }
        public Decimal valorIcms
        {
            get
            {
                if (baseICMS == 0 || aliquota_icms == 0)
                    return 0;
                else
                {
                    Decimal vicms = (baseICMS * aliquota_icms) / 100;
                    return vicms;
                }
            }
        }
        public Decimal redutor_base { get; set; }
        public Decimal codigo_operacao { get; set; }
        public Decimal Frete = 0;
        public int Num_item { get; set; }
        private Decimal vPBasePisCofins = -1;
        public Decimal vBASEPisCofins
        {
            get
            {
                if (vPBasePisCofins == -1)
                {
                    vPBasePisCofins = TotalProduto - DescontoValor;
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
        public String NCM = "";
        public String Und { get; set; }
        public String Artigo { get; set; }
        public Decimal Peso_liquido { get; set; }
        public Decimal Peso_Bruto { get; set; }
        public String Tipo { get; set; }
        public String CF { get; set; }
        private String vCSTPIS = "";
        public String CSTPIS
        {
            get
            {
                return vCSTPIS;
            }
            set
            {

                vCSTPIS = value;
                if (!vCSTPIS.Trim().Equals(""))
                {

                    int cst = int.Parse(vCSTPIS);
                    if (Tipo_NF.Equals("2"))
                    {


                        if (cst >= 50 && cst < 60)
                        {
                            PISp = 1.65m;
                            PISV = (PISp * (TotalProduto - DescontoValor)) / 100;
                        }
                        else if (cst >= 60 && cst <= 66)
                        {
                            PISp = 0.65m;
                            PISV = (PISp * (TotalProduto - DescontoValor)) / 100;
                        }
                        else
                        {
                            PISV = 0;
                        }
                    }
                    else
                    {


                        if (cst == 1 || cst == 2)
                        {
                            PISp = usr.filial.pis;// 0.65m; //1.65M;
                            PISV = (PISp * (TotalProduto - DescontoValor)) / 100;

                        }
                        else
                        {
                            PISp = 0;
                            PISV = 0;
                        }
                    }
                }
            }

        }
        private String vCSTCOFINS = "";
        public String CSTCOFINS
        {

            get
            {
                return vCSTCOFINS;
            }
            set
            {
                vCSTCOFINS = value;
                if (!vCSTCOFINS.Trim().Equals(""))
                {
                    int cst = int.Parse(vCSTCOFINS);
                    if (Tipo_NF.Equals("2"))
                    {


                        if (cst >= 50 && cst < 60)
                        {
                            COFINSp = 7.60m;
                            COFINSV = (COFINSp * (TotalProduto - DescontoValor)) / 100;
                        }
                        else if (cst >= 60 && cst <= 70)
                        {
                            COFINSp = 3.00m;
                            COFINSV = (COFINSp * (TotalProduto - DescontoValor)) / 100;
                        }
                        else
                        {
                            COFINSp = 0;
                            COFINSV = 0;
                        }
                    }
                    else
                    {

                        if (cst == 1 || cst == 2)
                        {
                            COFINSp = usr.filial.cofins; //3.00m;//7.60m;
                            COFINSV = (COFINSp * (TotalProduto - DescontoValor)) / 100;
                        }
                        else
                        {
                            COFINSp = 0;
                            COFINSV = 0;
                        }
                    }
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
                    return 0;
                else
                {
                    Decimal vBase = (TotalProduto - DescontoValor);
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


        public nf_itemDAO(User usr)
        {
            this.usr = usr;
            Filial = usr.getFilial();
        }

        public nf_itemDAO copia()
        {

            return (nf_itemDAO)this.MemberwiseClone();
        }

        public static ArrayList itens(String codigo, String tipoNf, String clienteFornecedor, bool nf_canc, User usr)
        {
            ArrayList ArrItens = new ArrayList();
            String sql = "Select * from  nf_item where codigo ='" + codigo + "' and tipo_nf = " + tipoNf + " and cliente_fornecedor = '" + clienteFornecedor + "' and isnull(nf_canc,0)=" + (nf_canc ? '1' : '0');
            SqlDataReader rs = Conexao.consulta(sql, usr, false);
            while (rs.Read())
            {
                nf_itemDAO nfItem = new nf_itemDAO(usr);
                nfItem.Filial = rs["Filial"].ToString();
                nfItem.Codigo = rs["Codigo"].ToString();
                nfItem.Cliente_Fornecedor = rs["Cliente_Fornecedor"].ToString();
                nfItem.Tipo_NF = rs["Tipo_NF"].ToString();
                nfItem.PLU = rs["PLU"].ToString();
                nfItem.Codigo_Tributacao = (Decimal)(rs["Codigo_Tributacao"].ToString().Equals("") ? new Decimal() : rs["Codigo_Tributacao"]);
                nfItem.Qtde = (Decimal)(rs["Qtde"].ToString().Equals("") ? new Decimal() : rs["Qtde"]);
                nfItem.Embalagem = (Decimal)(rs["Embalagem"].ToString().Equals("") ? new Decimal() : rs["Embalagem"]);
                nfItem.Unitario = (Decimal)(rs["Unitario"].ToString().Equals("") ? new Decimal() : rs["Unitario"]);
                nfItem.Desconto = (Decimal)(rs["Desconto"].ToString().Equals("") ? new Decimal() : rs["Desconto"]);
                nfItem.Total = (Decimal)(rs["Total"].ToString().Equals("") ? new Decimal() : rs["Total"]);
                nfItem.IPI = (Decimal)(rs["IPI"].ToString().Equals("") ? new Decimal() : rs["IPI"]);
                nfItem.IPIV = (Decimal)(rs["IPIV"].ToString().Equals("") ? new Decimal() : rs["IPIV"]);
                nfItem.ean = rs["ean"].ToString();
                nfItem.aliquota_icms = (Decimal)(rs["aliquota_icms"].ToString().Equals("") ? new Decimal() : rs["aliquota_icms"]);
                nfItem.vAliquota_iva = (Decimal)(rs["aliquota_iva"].ToString().Equals("") ? new Decimal() : rs["aliquota_iva"]);
                nfItem.vIndiceSt = rs["indice_st"].ToString();
                nfItem.vBaseIva = (Decimal)(rs["base_iva"].ToString().Equals("") ? new Decimal() : rs["base_iva"]);
                nfItem.vIva = (Decimal)(rs["iva"].ToString().Equals("") ? new Decimal() : rs["iva"]);
                nfItem.vmargemIva = (Decimal)(rs["margem_iva"].ToString().Equals("") ? new Decimal() : rs["margem_iva"]);
                nfItem.despesas = (Decimal)(rs["despesas"].ToString().Equals("") ? new Decimal() : rs["despesas"]);
                nfItem.CFOP = (Decimal)(rs["CFOP"].ToString().Equals("") ? new Decimal() : rs["CFOP"]);
                nfItem.CODIGO_REFERENCIA = rs["CODIGO_REFERENCIA"].ToString();
                nfItem.redutor_base = (Decimal)(rs["redutor_base"].ToString().Equals("") ? new Decimal() : rs["redutor_base"]);
                nfItem.codigo_operacao = (Decimal)(rs["codigo_operacao"].ToString().Equals("") ? new Decimal() : rs["codigo_operacao"]);
                nfItem.Frete = (Decimal)(rs["Frete"].ToString().Equals("") ? new Decimal() : rs["Frete"]);
                nfItem.Num_item = (rs["Num_item"] == null ? 0 : int.Parse(rs["Num_item"].ToString()));
                nfItem.PISV = (rs["PISV"].ToString().Equals("") ? new Decimal() : Decimal.Parse(rs["PISV"].ToString()));
                nfItem.COFINSV = (rs["COFINSV"].ToString().Equals("") ? new Decimal() : Decimal.Parse(rs["COFINSV"].ToString()));
                nfItem.NCM = (rs["NCM"] == null ? "null" : rs["NCM"].ToString());
                nfItem.Und = rs["Und"].ToString();
                nfItem.Artigo = rs["Artigo"].ToString();
                nfItem.Peso_liquido = (Decimal)(rs["Peso_liquido"].ToString().Equals("") ? new Decimal() : rs["Peso_liquido"]);
                nfItem.Peso_Bruto = (Decimal)(rs["Peso_Bruto"].ToString().Equals("") ? new Decimal() : rs["Peso_Bruto"]);
                nfItem.Tipo = rs["Tipo"].ToString();
                nfItem.CF = rs["CF"].ToString();
                nfItem.CSTPIS = rs["CSTPIS"].ToString();
                nfItem.CSTCOFINS = rs["CSTCOFINS"].ToString();

                nfItem.pCredSN = (Decimal)(rs["pCredSN"].ToString().Equals("") ? new Decimal() : rs["pCredSN"]);
                nfItem.vCredicmssn = (Decimal)(rs["vCredicmssn"].ToString().Equals("") ? new Decimal() : rs["vCredicmssn"]);
                nfItem.vOrigem = int.Parse((rs["origem"].ToString().Equals("") ? "-1" : rs["origem"].ToString()));
                nfItem.inativo = (rs["inativo"].ToString().Equals("1") ? true : false);
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
            item.Add(TotalProduto.ToString("N4"));
            item.Add(IPI.ToString("N4"));
            item.Add(Descricao);
            item.Add(IPIV.ToString("N4"));
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
            if (rs.Read())
            {
                Filial = rs["Filial"].ToString();
                Codigo = rs["Codigo"].ToString();
                Cliente_Fornecedor = rs["Cliente_Fornecedor"].ToString();
                Tipo_NF = rs["Tipo_NF"].ToString();
                PLU = rs["PLU"].ToString();
                Codigo_Tributacao = (Decimal)(rs["Codigo_Tributacao"].ToString().Equals("") ? new Decimal() : rs["Codigo_Tributacao"]);
                Qtde = (Decimal)(rs["Qtde"].ToString().Equals("") ? new Decimal() : rs["Qtde"]);
                Embalagem = (Decimal)(rs["Embalagem"].ToString().Equals("") ? new Decimal() : rs["Embalagem"]);
                Unitario = (Decimal)(rs["Unitario"].ToString().Equals("") ? new Decimal() : rs["Unitario"]);
                Desconto = (Decimal)(rs["Desconto"].ToString().Equals("") ? new Decimal() : rs["Desconto"]);
                Total = (Decimal)(rs["Total"].ToString().Equals("") ? new Decimal() : rs["Total"]);
                IPI = (Decimal)(rs["IPI"].ToString().Equals("") ? new Decimal() : rs["IPI"]);
                // Descricao = rs["Descricao"].ToString();
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
                inativo = (rs["origem"].ToString().Equals("1") ? true : false);
            }

            if (rs != null)
                rs.Close();
        }
        private void update(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                if (Tipo_NF.Equals("2"))  //descarta ultima alteração
                {
                    if (naturezaOperacao.Baixa_estoque)
                    {
                        try
                        {
                            Decimal vLogQtde = 0;
                            Decimal vLogEmbalagen = 0;
                            SqlDataReader rsUltAlteracao = Conexao.consulta("select  top 1 qtde,embalagem ,data_alteracao from nf_item_log where plu='" + PLU + "' and codigo='" + Codigo + "' and filial='" + Filial + "' and  tipo_nf=2 and Num_item="+Num_item+" order by data_alteracao desc", null, false, conn, tran);
                            if (rsUltAlteracao.Read())
                            {
                                vLogQtde = (rsUltAlteracao["qtde"].ToString().Equals("") ? 0 : Decimal.Parse(rsUltAlteracao["qtde"].ToString()));
                                vLogEmbalagen = (rsUltAlteracao["embalagem"].ToString().Equals("") ? 0 : Decimal.Parse(rsUltAlteracao["embalagem"].ToString())); ;
                            }
                            if (rsUltAlteracao != null)
                                rsUltAlteracao.Close();


                            String SqlEstoque = "update mercadoria_Loja set  saldo_atual = (isnull(saldo_atual,0) -" + (vLogQtde * vLogEmbalagen).ToString().Replace(',', '.') + ") where plu='" + PLU + "' and filial='" + Filial + "'";
                            Conexao.executarSql(SqlEstoque, conn, tran);


                        }
                        catch (Exception err)
                        {

                            throw new Exception("retorno valor alteracao mercadoria_loja : Detalhe:" + err.Message);
                        }
                    }

                }

                String sql = "update  nf_item set " +

                              "Codigo_Tributacao=" + Codigo_Tributacao.ToString().Replace(",", ".") +
                              ",Qtde=" + Qtde.ToString().Replace(".", "").Replace(",", ".") +
                              ",Embalagem=" + Embalagem.ToString().Replace(".", "").Replace(",", ".") +
                              ",Unitario=" + Unitario.ToString("N2").Replace(".", "").Replace(",", ".") +
                              ",Desconto=" + Desconto.ToString("N2").Replace(".", "").Replace(",", ".") +
                              ",Total=" + TotalProduto.ToString("N2").Replace(".", "").Replace(",", ".") +
                              ",IPI=" + IPI.ToString().Replace(".", "").Replace(",", ".") +
                              ",Descricao='" + Descricao + "'" +
                              ",IPIV=" + IPIV.ToString().Replace(".", "").Replace(",", ".") +
                              ",ean='" + ean + "'" +
                              ",iva=" + vIva.ToString().Replace(".", "").Replace(",", ".") +
                              ",base_iva=" + vBaseIva.ToString().Replace(".", "").Replace(",", ".") +
                              ",margem_iva=" + vmargemIva.ToString().Replace(".", "").Replace(",", ".") +
                              ",despesas=" + despesas.ToString().Replace(".", "").Replace(",", ".") +
                              ",CFOP=" + CFOP.ToString().Replace(".", "").Replace(",", ".") +
                              ",CODIGO_REFERENCIA='" + CODIGO_REFERENCIA + "'" +
                              ",aliquota_icms=" + aliquota_icms.ToString().Replace(".", "").Replace(",", ".") +
                              ",aliquota_iva=" + aliquota_iva.ToString().Replace(".", "").Replace(",", ".") +
                              ",indice_st='" + indice_St + "'" +
                              ",redutor_base=" + redutor_base.ToString().Replace(".", "").Replace(",", ".") +
                              ",codigo_operacao=" + codigo_operacao.ToString().Replace(".", "").Replace(",", ".") +
                              ",Frete=" + Frete.ToString().Replace(".", "").Replace(",", ".") +
                              ",Num_item=" + Num_item +
                              ",PISV=" + PISV.ToString("N4").Replace(".", "").Replace(",", ".") +
                              ",COFINSV=" + COFINSV.ToString("N4").Replace(".", "").Replace(",", ".") +
                              ",NCM='" + NCM + "'" +
                              ",Und='" + Und + "'" +
                              ",Artigo='" + Artigo + "'" +
                              ",Peso_liquido=" + Peso_liquido.ToString().Replace(",", ".") +
                              ",Peso_Bruto=" + Peso_Bruto.ToString().Replace(",", ".") +
                              ",Tipo='" + Tipo + "'" +
                              ",CF='" + CF + "'" +
                              ",CSTPIS='" + CSTPIS + "'" +
                              ",CSTCOFINS='" + CSTCOFINS + "'" +
                              ",pCredSN=" + pCredSN.ToString().Replace(",", ".") +
                              ",vCredicmssn=" + vCredicmssn.ToString().Replace(",", ".") +
                              ",origem =" + origem.ToString() +
                              ",inativo=" + (inativo ? "1" : "0") +
                              ",base_pis = " + vBASEPisCofins.ToString().Replace(".", "").Replace(",", ".") +
                              ",base_cofins=" + vBASEPisCofins.ToString().Replace(".", "").Replace(",", ".") +
                              ",aliquota_pis=" + PISp.ToString().Replace(".", "").Replace(",", ".") +
                              ",aliquota_cofins=" + COFINSp.ToString().Replace(".", "").Replace(",", ".") +
                              ",cst_icms='" + indice_St + "'" +
                              ",base_icms=" + baseICMS.ToString().Replace(".", "").Replace(",", ".") +
                              ",icmsv=" + valorIcms.ToString().Replace(".", "").Replace(",", ".") +
                    "  where Filial='" + Filial + "' and codigo =" + Codigo + " and tipo_nf = " + Tipo_NF + " and cliente_fornecedor = '" + Cliente_Fornecedor + "' and plu='" + PLU + "' and  Num_item = " + Num_item;

                Conexao.executarSql(sql, conn, tran);

                if (Tipo_NF.Equals("2"))
                {

                    if (naturezaOperacao.Baixa_estoque)
                    {
                        try
                        {

                            String SqlEstoque = "update mercadoria_Loja set  saldo_atual = (ISNULL(saldo_atual,0) +" + (Qtde * Embalagem).ToString().Replace(".", "").Replace(',', '.') + ") where plu='" + PLU + "' and filial='" + Filial + "'";
                            Conexao.executarSql(SqlEstoque, conn, tran);
                            String SqlMercadoria = " update mercadoria  set  saldo_atual =(select sum(isnull(saldo_atual,0))from mercadoria_loja b where b.plu='" + PLU + "') where plu='" + PLU + "'";
                            Conexao.executarSql(SqlMercadoria, conn, tran);
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
                            String SqlCusto = "update mercadoria set   preco_custo = " + (Total / (Qtde * Embalagem)).ToString().Replace(".", "").Replace(',', '.') + " , preco_compra=" + (TotalProduto / (Qtde * Embalagem)).ToString().Replace(".", "").Replace(',', '.') + ", cf='" + NCM + "'   where plu='" + PLU + "' ";
                            Conexao.executarSql(SqlCusto, conn, tran);

                            String SqlCustoLoja = "update mercadoria_loja set PRECO_CUSTO_2 = PRECO_CUSTO_1, Preco_Custo_1 = Preco_Custo,  preco_custo = " + (Total / (Qtde * Embalagem)).ToString().Replace(".", "").Replace(',', '.') + " , preco_compra=" + (TotalProduto / (Qtde * Embalagem)).ToString().Replace(".", "").Replace(',', '.') + ", data_alteracao='" + DateTime.Now.ToString("yyyy-MM-dd") + "'  where plu='" + PLU + "' and filial='" + Filial + "'";
                            Conexao.executarSql(SqlCustoLoja, conn, tran);

                        }
                        catch (Exception err)
                        {
                            throw new Exception("Atualização do preco_custo,Preco_compra, margem_iva, NCM,tributacao da Tabela mercadoria-" + err);

                        }

                    }
                    String atualiza = Funcoes.valorParametro("ATUALIZA_TRIBUTACAO", usr);
                    if (atualiza.Equals(".T."))
                    {
                        String SqlCusto = "update mercadoria set  margem_iva = " + vmargemIva.ToString("N2").Replace(".", "").Replace(",", ".") + ",ipi=" + porcIPI.ToString().Replace(".", "").Replace(",", ".") + " ,codigo_tributacao_ent=" + Codigo_Tributacao + "   where plu='" + PLU + "' and filial='" + Filial + "'";
                        Conexao.executarSql(SqlCusto, conn, tran);

                    }

                }
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }

        public void AtualizaSaidaEstoque(SqlConnection conn, SqlTransaction tran)
        {
            if (naturezaOperacao != null && naturezaOperacao.Baixa_estoque)
            {
                try
                {
                    if (Tipo_NF.Equals("1"))
                    {
                        String SqlEstoque = " update mercadoria_loja set  saldo_atual = (isnull(saldo_atual,0) -" + (Qtde * Embalagem).ToString().Replace(',', '.') + ") where plu='" + PLU + "' and filial='" + Filial + "'";
                        Conexao.executarSql(SqlEstoque, conn, tran);
                    }
                    else
                    {
                        String SqlEstoque = " update mercadoria_loja set  saldo_atual = (isnull(saldo_atual,0) +" + (Qtde * Embalagem).ToString().Replace(',', '.') + ") where plu='" + PLU + "' and filial='" + Filial + "'";
                        Conexao.executarSql(SqlEstoque, conn, tran);
                    }
                    String SqlMercadoria = " update mercadoria  set  saldo_atual =(select sum(isnull(saldo_atual,0))from mercadoria_loja b where b.plu='" + PLU + "') where plu='" + PLU + "'";
                    Conexao.executarSql(SqlMercadoria, conn, tran);

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

        public bool excluir(SqlConnection conn, SqlTransaction tran)
        {
            if (naturezaOperacao != null && naturezaOperacao.Baixa_estoque)
            {
                if (Tipo_NF.Equals("2"))
                {
                    try
                    {

                        String SqlEstoque = "update mercadoria_Loja set  saldo_atual = (isnull(saldo_atual,0) -" + (Qtde * Embalagem).ToString().Replace(',', '.') + ") where plu='" + PLU + "' and filial='" + Filial + "'";
                        Conexao.executarSql(SqlEstoque);
                        String SqlMercadoria = " update mercadoria  set  saldo_atual =(select sum(isnull(saldo_atual,0))from mercadoria_loja b where b.plu='" + PLU + "') where plu='" + PLU + "'";
                        Conexao.executarSql(SqlMercadoria, conn, tran);
                    }
                    catch (Exception err)
                    {

                        throw new Exception("Atualização estoque Excluir Item  mercadoria_loja , mercadoria : Erro Detalhe:" + err.Message);
                    }
                }
            }
            String sql = "delete from nf_item where codigo ='" + Codigo + "' and tipo_nf = " + Tipo_NF + " and cliente_fornecedor = '" + Cliente_Fornecedor + "' and plu='" + PLU + "'"; //colocar campo index
            Conexao.executarSql(sql, conn, tran);
            return true;
        }
        public bool cancela(SqlConnection conn, SqlTransaction tran)
        {

            if (naturezaOperacao != null && naturezaOperacao.Baixa_estoque)
            {
                if (Tipo_NF.Equals("2"))
                {
                    try
                    {

                        String SqlEstoque = "update mercadoria_Loja set  saldo_atual = (isnull(saldo_atual,0) -" + (Qtde * Embalagem).ToString().Replace(',', '.') + ") where plu='" + PLU + "' and filial='" + Filial + "'";
                        Conexao.executarSql(SqlEstoque);
                        String SqlMercadoria = " update mercadoria  set  saldo_atual =(select sum(isnull(saldo_atual,0))from mercadoria_loja b where b.plu='" + PLU + "') where plu='" + PLU + "'";
                        Conexao.executarSql(SqlMercadoria, conn, tran);
                    }
                    catch (Exception err)
                    {

                        throw new Exception("Atualização estoque Cancelamento  mercadoria_loja , mercadoria : Erro Detalhe:" + err.Message);
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
                     " )values (" +
                          "'" + Filial + "'" +
                          "," + "'" + Codigo + "'" +
                          "," + "'" + Cliente_Fornecedor + "'" +
                          "," + Tipo_NF +
                          "," + "'" + PLU + "'" +
                          "," + Codigo_Tributacao.ToString().Replace(".", "").Replace(",", ".") +
                          "," + Qtde.ToString().Replace(".", "").Replace(",", ".") +
                          "," + Embalagem.ToString().Replace(".", "").Replace(",", ".") +
                          "," + Unitario.ToString("N2").Replace(".", "").Replace(",", ".") +
                          "," + Desconto.ToString("N2").Replace(".", "").Replace(",", ".") +
                          "," + TotalProduto.ToString("N2").Replace(".", "").Replace(",", ".") +
                          "," + IPI.ToString().Replace(".", "").Replace(",", ".") +
                          "," + "'" + Descricao + "'" +
                          "," + IPIV.ToString().Replace(".", "").Replace(",", ".") +
                          "," + "'" + ean + "'" +
                          "," + vIva.ToString().Replace(".", "").Replace(",", ".") +
                          "," + vBaseIva.ToString().Replace(".", "").Replace(",", ".") +
                          "," + vmargemIva.ToString().Replace(".", "").Replace(",", ".") +
                          "," + despesas.ToString().Replace(".", "").Replace(",", ".") +
                          "," + CFOP.ToString().Replace(".", "").Replace(",", ".") +
                          "," + "'" + CODIGO_REFERENCIA + "'" +
                          "," + aliquota_icms.ToString().Replace(".", "").Replace(",", ".") +
                          "," + aliquota_iva.ToString().Replace(".", "").Replace(",", ".") +
                          ",'" + indice_St + "'" +
                          "," + redutor_base.ToString().Replace(".", "").Replace(",", ".") +
                          "," + codigo_operacao.ToString().Replace(".", "").Replace(",", ".") +
                          "," + Frete.ToString().Replace(".", "").Replace(",", ".") +
                          "," + Num_item +
                          "," + PISV.ToString().Replace(".", "").Replace(",", ".") +
                          "," + COFINSV.ToString().Replace(".", "").Replace(",", ".") +
                          ",'" + NCM.Replace(".", "") + "'" +
                          "," + "'" + Und + "'" +
                          "," + "'" + Artigo + "'" +
                          "," + Peso_liquido.ToString().Replace(".", "").Replace(",", ".") +
                          "," + Peso_Bruto.ToString().Replace(".", "").Replace(",", ".") +
                          "," + "'" + Tipo + "'" +
                          "," + "'" + CF + "'" +
                          "," + "'" + CSTPIS + "'" +
                          "," + "'" + CSTCOFINS + "'" +
                          "," + pCredSN.ToString().Replace(".", "").Replace(",", ".") +
                          "," + vCredicmssn.ToString().Replace(".", "").Replace(",", ".") +
                          "," + origem +
                          "," + (inativo ? "1" : "0") +
                          "," + vBASEPisCofins.ToString().Replace(".", "").Replace(",", ".") +
                          "," + vBASEPisCofins.ToString().Replace(".", "").Replace(",", ".") +
                          "," + PISp.ToString().Replace(".", "").Replace(",", ".") +
                          "," + COFINSp.ToString().Replace(".", "").Replace(",", ".") +
                          ",'" + indice_St + "'" +
                          "," + baseICMS.ToString().Replace(".", "").Replace(",", ".") +
                          "," + valorIcms.ToString().Replace(".", "").Replace(",", ".") +
                         ");";

                Conexao.executarSql(sql, conn, tran);
                if (Tipo_NF.Equals("2") && !naturezaOperacao.Imprime_NF)
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

                        sqlIMercFornc = "insert into fornecedor_mercadoria (filial,fornecedor,plu,descricao,Data,preco_compra,ean,codigo_referencia,preco_custo,embalagem)" +
                               " values ('" + usr.getFilial() + "','" + Cliente_Fornecedor + "','" + PLU + "','" + (Descricao.Length > 40 ? Descricao.Substring(0, 40) : Descricao) + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                             Unitario.ToString().Replace(',', '.') + ",'','" + CODIGO_REFERENCIA + "'," + Total.ToString().Replace(',', '.') + "," + Embalagem.ToString().Replace(",", ".") + ")";
                        Conexao.executarSql(sqlIMercFornc, conn, tran);

                        Conexao.executarSql("update mercadoria set ultimo_fornecedor= '" + Cliente_Fornecedor + "' where plu ='" + PLU + "'", conn, tran);

                    }
                    catch (Exception err)
                    {

                        throw new Exception("codigo de referencia no fornecedor:" + err.Message + "<\\br> Sql1:" + sqlprodutofornecedor + " <\\br>Sql2:" + sqlIMercFornc);
                    }

                    if (naturezaOperacao.Baixa_estoque)
                    {
                        try
                        {
                            String SqlEstoque = "update mercadoria_Loja set  saldo_atual = (isnull(saldo_atual,0) +" + (Qtde * Embalagem).ToString().Replace(',', '.') + ") where plu='" + PLU + "' and filial='" + Filial + "'";
                            Conexao.executarSql(SqlEstoque, conn, tran);
                            String SqlMercadoria = " update mercadoria  set  saldo_atual =(select sum(isnull(saldo_atual,0))from mercadoria_loja b where b.plu='" + PLU + "') where plu='" + PLU + "'";
                            Conexao.executarSql(SqlMercadoria, conn, tran);
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
                            String SqlCusto = "update mercadoria set  preco_custo = " + TotalCustoUnitario.ToString().Replace(',', '.') + " , preco_compra=" + (TotalProduto / (Qtde * Embalagem)).ToString().Replace(',', '.') + ", cf='" + NCM + "'  where plu='" + PLU + "' and filial='" + Filial + "'";
                            Conexao.executarSql(SqlCusto, conn, tran);

                            String SqlCustoLoja = "update mercadoria_loja set PRECO_CUSTO_2 = PRECO_CUSTO_1, Preco_Custo_1 = Preco_Custo,  preco_custo = " + TotalCustoUnitario.ToString().Replace(',', '.') + " , preco_compra=" + (TotalProduto / (Qtde * Embalagem)).ToString().Replace(',', '.') + ", data_alteracao='" + DateTime.Now.ToString("yyyy-MM-dd") + "'  where plu='" + PLU + "' and filial='" + Filial + "'";
                            Conexao.executarSql(SqlCustoLoja, conn, tran);

                        }
                        catch (Exception err)
                        {
                            throw new Exception("Atualização do preco_custo,Preco_compra, margem_iva, NCM,tributacao da Tabela mercadoria-" + err);

                        }

                    }

                    String vlr = Funcoes.valorParametro("ATUALIZA_TRIBUTACAO", usr);
                    if (vlr.Equals(".T.") || vlr.ToUpper().Equals("TRUE"))
                    {
                        String SqlCusto = "update mercadoria set  margem_iva = " + vmargemIva.ToString("N2").Replace(",", ".") + ",  codigo_tributacao_ent=" + Codigo_Tributacao + " ,IPI=" + IPI.ToString("N2").Replace(",", ".") + "  where plu='" + PLU + "' ";
                        Conexao.executarSql(SqlCusto, conn, tran);

                    }

                }





            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }
    }
}/* 
/*================================Metodos tela de Pesquisa==========================================
using System.Data; 
using visualSysWeb.dao;
           :visualSysWeb.code.PagePadrao     //inicio da classe 
{ 
                  static DataTable tb;
                  static String sqlGrid = ""select * from nf_item";//colocar os campos no select que ser?o apresentados na tela
                  protected void Page_Load(object sender, EventArgs e)
                  {
                     if (!IsPostBack)
                     {   
                       User usr = (User)Session["User"];
                       tb = Conexao.GetTable(sqlGrid ,usr); 
                       gridPesquisa.DataSource = tb;
                       gridPesquisa.DataBind();
                       Lblindex.Text = "1/" + gridPesquisa.PageCount;
                      }
                      pesquisar(pnBtn);
                  }
                  
                  protected override void btnIncluir_Click(object sender, EventArgs e)
                  {
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ nf_itemDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
                  }
                  
                  protected override void btnPesquisar_Click(object sender, EventArgs e)
                  {
                      String sql = "";
                      if (!txtPESQ1.Text.Equals("")) //colocar nome do campo de pesquisa
                      {
                          sql = " campoPesquisa1 like '" + txtPESQ1.Text + "%'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
                      }
                      if (!txtPESQ2.Text.Equals("")) //colocar nome do campo de pesquisa2
                      {
                          if (!sql.Equals(""))
                          {
                              sql += " and ";     
                          }
                         sql += "campoPesquisa2 = '" + txtPESQ2.Text + "'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
                      }
                         try
                         {//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                            User usr = (User)Session["User"];
                            if (!sql.Equals(""))
                            {
                               tb = Conexao.GetTable(sqlGrid+" where "+sql, usr);
                             }
                             else
                             {
                               tb = Conexao.GetTable(sqlGrid, usr);
                              }
                               gridPesquisa.DataSource = tb;
                               gridPesquisa.DataBind();
                               lblPesquisaErro.Text = "";
                               Lblindex.Text = "1/" + gridPesquisa.PageCount;
                        }catch (Exception err)
                         {
                                      lblPesquisaErro.Text = err.Message;
                         }
                  }
                  protected override void btnEditar_Click(object sender, EventArgs e){}
                  protected override void btnExcluir_Click(object sender, EventArgs e) {}
                  protected override void btnConfirmar_Click(object sender, EventArgs e){}
                  protected override void btnCancelar_Click(object sender, EventArgs e){}   
                  
                  
                  protected void gridPesquisa_PageIndexChanging(object sender, GridViewPageEventArgs e)
                  {
                    gridPesquisa.DataSource = tb;
                    gridPesquisa.PageIndex = e.NewPageIndex;
                    Lblindex.Text = (e.NewPageIndex+1)+"/" + gridPesquisa.PageCount;
                    gridPesquisa.DataBind();
                  }
                 protected override bool campoObrigatorio(Control campo)
                 { 
                       return false;
                 }
                 
                 protected override bool campoDesabilitado(Control campo)
                 {
                       return false;
                 }
                 
*/

/*================================html tela de Pesquisa==========================================
                  
   <center><h1>nf_item</h1></center>
    <hr />              
       <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">               
       </asp:Panel>           
       <br />           
       <div class="filter" id="filtrosPesq" runat="server" visible="false">           
         <table>           
           <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>           
            <tr>           
                <td>           
                <p>CAMPO DE PESQUISA 1</p>   
                <asp:TextBox ID="txtPESQ1" runat="server" ></asp:TextBox></asp:TextBox>  
                </td>  
                <td>  
                   <p>CAMPO DE PESQUISA 2</p>  
                   <asp:TextBox ID="txtPESQ2" runat="server" > </asp:TextBox>
                </td>  
            </tr>      
                  
                  
                  
         </table>           
        </div>            
        <div class="gridTable">          
            <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False"           
                 AllowPaging="True" 
                 PageSize="20"  
                 onpageindexchanging="gridPesquisa_PageIndexChanging" CellPadding="37"  
                 ForeColor="#333333" GridLines="None"  
                 > 
                 <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" /> 
                 <Columns> 
                    <asp:HyperLinkField DataTextField="Filial" Text="Filial" Visible="true" 
                    HeaderText="Filial" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Codigo" Text="Codigo" Visible="true" 
                    HeaderText="Codigo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Cliente_Fornecedor" Text="Cliente_Fornecedor" Visible="true" 
                    HeaderText="Cliente_Fornecedor" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Tipo_NF" Text="Tipo_NF" Visible="true" 
                    HeaderText="Tipo_NF" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="PLU" Text="PLU" Visible="true" 
                    HeaderText="PLU" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Codigo_Tributacao" Text="Codigo_Tributacao" Visible="true" 
                    HeaderText="Codigo_Tributacao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Qtde" Text="Qtde" Visible="true" 
                    HeaderText="Qtde" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Embalagem" Text="Embalagem" Visible="true" 
                    HeaderText="Embalagem" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Unitario" Text="Unitario" Visible="true" 
                    HeaderText="Unitario" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Desconto" Text="Desconto" Visible="true" 
                    HeaderText="Desconto" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Total" Text="Total" Visible="true" 
                    HeaderText="Total" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="IPI" Text="IPI" Visible="true" 
                    HeaderText="IPI" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Descricao" Text="Descricao" Visible="true" 
                    HeaderText="Descricao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="IPIV" Text="IPIV" Visible="true" 
                    HeaderText="IPIV" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="ean" Text="ean" Visible="true" 
                    HeaderText="ean" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="iva" Text="iva" Visible="true" 
                    HeaderText="iva" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="base_iva" Text="base_iva" Visible="true" 
                    HeaderText="base_iva" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="margem_iva" Text="margem_iva" Visible="true" 
                    HeaderText="margem_iva" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="despesas" Text="despesas" Visible="true" 
                    HeaderText="despesas" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="CFOP" Text="CFOP" Visible="true" 
                    HeaderText="CFOP" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="CODIGO_REFERENCIA" Text="CODIGO_REFERENCIA" Visible="true" 
                    HeaderText="CODIGO_REFERENCIA" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="aliquota_icms" Text="aliquota_icms" Visible="true" 
                    HeaderText="aliquota_icms" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="redutor_base" Text="redutor_base" Visible="true" 
                    HeaderText="redutor_base" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="codigo_operacao" Text="codigo_operacao" Visible="true" 
                    HeaderText="codigo_operacao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Frete" Text="Frete" Visible="true" 
                    HeaderText="Frete" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Num_item" Text="Num_item" Visible="true" 
                    HeaderText="Num_item" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="PISV" Text="PISV" Visible="true" 
                    HeaderText="PISV" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="COFINSV" Text="COFINSV" Visible="true" 
                    HeaderText="COFINSV" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="NCM" Text="NCM" Visible="true" 
                    HeaderText="NCM" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Und" Text="Und" Visible="true" 
                    HeaderText="Und" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Artigo" Text="Artigo" Visible="true" 
                    HeaderText="Artigo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Peso_liquido" Text="Peso_liquido" Visible="true" 
                    HeaderText="Peso_liquido" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Peso_Bruto" Text="Peso_Bruto" Visible="true" 
                    HeaderText="Peso_Bruto" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Tipo" Text="Tipo" Visible="true" 
                    HeaderText="Tipo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="CF" Text="CF" Visible="true" 
                    HeaderText="CF" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="CSTPIS" Text="CSTPIS" Visible="true" 
                    HeaderText="CSTPIS" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="CSTCOFINS" Text="CSTCOFINS" Visible="true" 
                    HeaderText="CSTCOFINS" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  


                 </Columns> 
                 <EditRowStyle BackColor="#999999" /> 
                 <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" /> 
                 <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" /> 
                 <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" /> 
                 <RowStyle BackColor="#F7F6F3" ForeColor="#333333" /> 
                 <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" /> 
                 <SortedAscendingCellStyle BackColor="#E9E7E2" /> 
                 <SortedAscendingHeaderStyle BackColor="#506C8C" /> 
                 <SortedDescendingCellStyle BackColor="#FFFDF8" /> 
                  <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
           </asp:GridView>           
           <br />       
           <center><asp:Label ID="Lblindex" runat="server" Text="1/.."></asp:Label></center>       
        </div>          
                  
*/
/*================================Metodos tela detalhes==========================================
using System.Data; 
using visualSysWeb.dao;
using System.Data.SqlClient;
                 : visualSysWeb.code.PagePadrao
  {
                 protected static nf_itemDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new nf_itemDAO();
                      tabMenu.Items[MultiView1.ActiveViewIndex].Selected = true;
                      if (Request.Params["novo"] != null) 
                      {
                        status = "incluir";
                                         EnabledControls(conteudo, true);
                                         EnabledControls(cabecalho, true);
                      }
                      else
                      {
                           if (Request.Params["campoIndex"] != null)  // colocar o campo index da tabela
                           {
                              try
                              {
                                   if (!IsPostBack)
                                   {
                                        String index = Request.Params["campoIndex"].ToString();// colocar o campo index da tabela
                                        status = "visualizar";
                                        obj = new nf_itemDAO(index,usr);
                                        carregarDados();
                                    }
                                    if (status.Equals("visualizar"))
                                    {
                                         EnabledControls(conteudo, false);
                                         EnabledControls(cabecalho, false);
                                    }else{
                                         EnabledControls(conteudo, true);
                                         EnabledControls(cabecalho, true);
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
                 
                 private void limparCampos(){
                    LimparCampos(cabecalho);          
                    LimparCampos(conteudo);             
                 }
                 
                 protected bool validaCamposObrigatorios() {
                    if (validaCampos(cabecalho) && validaCampos(conteudo))
                             return true;
                    else
                             return false;
                 }
                 
                 protected override bool campoObrigatorio(Control campo)
                 {// colocar os nomes dos campos obrigarios no Array
                     String[] campos = { "", 
                                    "", 
                                    "", 
                                    "" 
                                     };
                       return existeNoArray(campos, campo.ID+"");
                 }
                 
                 protected override bool campoDesabilitado(Control campo)
                 {// colocar os nomes dos campos Desabilitados no Array
                     String[] campos = { "", 
                                    "", 
                                    "", 
                                    "" 
                                     };
                       return existeNoArray(campos, campo.ID+"");
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
                 Response.Redirect("nomepaginapesquisa.aspx"); //colocar o endereco da tela de pesquisa
                 }
                  
                 protected override void btnExcluir_Click(object sender, EventArgs e)
                 {
                     pnConfima.Visible = true;
                  }
                  
                  protected override void btnConfirmar_Click(object sender, EventArgs e)
                  {
                     try
                     {
                       if (validaCamposObrigatorios())
                       {
                  
                             carregarDadosObj();
                             obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                             lblError.Text = "Salvo com Sucesso";
                             lblError.ForeColor = System.Drawing.Color.Blue;
                             EnabledControls(cabecalho, false);
                             EnabledControls(conteudo, false);
                             visualizar(pnBtn);
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
                      Response.Redirect("nomepaginapesquisa.aspx");//colocar endereco pagina de pesquisa
                  }
                  protected void tabMenu_MenuItemClick(object sender, MenuEventArgs e)
                  {
                      switch (e.Item.Value)
                      {
                          case "tab1":
                          MultiView1.ActiveViewIndex = 0;
                          break;
                       }
                   }
                    //--Atualizar DaoForm 
      private void carregarDados()
      {
                                         txtFilial.Text=obj.Filial.ToString();
                                         txtCodigo.Text=obj.Codigo.ToString();
                                         txtCliente_Fornecedor.Text=obj.Cliente_Fornecedor.ToString();
                                         chkTipo_NF.Checked =obj.Tipo_NF;
                                         txtPLU.Text=obj.PLU.ToString();
                                         txtCodigo_Tributacao.Text=string.Format("{0:0,0.00}",obj.Codigo_Tributacao);
                                         txtQtde.Text=string.Format("{0:0,0.00}",obj.Qtde);
                                         txtEmbalagem.Text=string.Format("{0:0,0.00}",obj.Embalagem);
                                         txtUnitario.Text=string.Format("{0:0,0.00}",obj.Unitario);
                                         txtDesconto.Text=string.Format("{0:0,0.00}",obj.Desconto);
                                         txtTotal.Text=string.Format("{0:0,0.00}",obj.Total);
                                         txtIPI.Text=string.Format("{0:0,0.00}",obj.IPI);
                                         txtDescricao.Text=obj.Descricao.ToString();
                                         txtIPIV.Text=string.Format("{0:0,0.00}",obj.IPIV);
                                         txtean.Text=obj.ean.ToString();
                                         txtiva.Text=string.Format("{0:0,0.00}",obj.iva);
                                         txtbase_iva.Text=string.Format("{0:0,0.00}",obj.base_iva);
                                         txtmargem_iva.Text=string.Format("{0:0,0.00}",obj.margem_iva);
                                         txtdespesas.Text=string.Format("{0:0,0.00}",obj.despesas);
                                         txtCFOP.Text=string.Format("{0:0,0.00}",obj.CFOP);
                                         txtCODIGO_REFERENCIA.Text=obj.CODIGO_REFERENCIA.ToString();
                                         txtaliquota_icms.Text=string.Format("{0:0,0.00}",obj.aliquota_icms);
                                         txtredutor_base.Text=string.Format("{0:0,0.00}",obj.redutor_base);
                                         txtcodigo_operacao.Text=string.Format("{0:0,0.00}",obj.codigo_operacao);
                                         txtFrete.Text=string.Format("{0:0,0.00}",obj.Frete);
                                         txtNum_item.Text=obj.Num_item.ToString();
                                         txtPISV.Text=string.Format("{0:0,0.00}",obj.PISV);
                                         txtCOFINSV.Text=string.Format("{0:0,0.00}",obj.COFINSV);
                                         txtNCM.Text=obj.NCM.ToString();
                                         txtUnd.Text=obj.Und.ToString();
                                         txtArtigo.Text=obj.Artigo.ToString();
                                         txtPeso_liquido.Text=string.Format("{0:0,0.00}",obj.Peso_liquido);
                                         txtPeso_Bruto.Text=string.Format("{0:0,0.00}",obj.Peso_Bruto);
                                         txtTipo.Text=obj.Tipo.ToString();
                                         txtCF.Text=obj.CF.ToString();
                                         txtCSTPIS.Text=obj.CSTPIS.ToString();
                                         txtCSTCOFINS.Text=obj.CSTCOFINS.ToString();
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.Filial=txtFilial.Text;
                                         obj.Codigo=txtCodigo.Text;
                                         obj.Cliente_Fornecedor=txtCliente_Fornecedor.Text;
                                         obj.Tipo_NF=chkTipo_NF.Checked ;
                                         obj.PLU=txtPLU.Text;
                                         obj.Codigo_Tributacao=Decimal.Parse(txtCodigo_Tributacao.Text);
                                         obj.Qtde=Decimal.Parse(txtQtde.Text);
                                         obj.Embalagem=Decimal.Parse(txtEmbalagem.Text);
                                         obj.Unitario=Decimal.Parse(txtUnitario.Text);
                                         obj.Desconto=Decimal.Parse(txtDesconto.Text);
                                         obj.Total=Decimal.Parse(txtTotal.Text);
                                         obj.IPI=Decimal.Parse(txtIPI.Text);
                                         obj.Descricao=txtDescricao.Text;
                                         obj.IPIV=Decimal.Parse(txtIPIV.Text);
                                         obj.ean=txtean.Text;
                                         obj.iva=Decimal.Parse(txtiva.Text);
                                         obj.base_iva=Decimal.Parse(txtbase_iva.Text);
                                         obj.margem_iva=Decimal.Parse(txtmargem_iva.Text);
                                         obj.despesas=Decimal.Parse(txtdespesas.Text);
                                         obj.CFOP=Decimal.Parse(txtCFOP.Text);
                                         obj.CODIGO_REFERENCIA=txtCODIGO_REFERENCIA.Text;
                                         obj.aliquota_icms=Decimal.Parse(txtaliquota_icms.Text);
                                         obj.redutor_base=Decimal.Parse(txtredutor_base.Text);
                                         obj.codigo_operacao=Decimal.Parse(txtcodigo_operacao.Text);
                                         obj.Frete=Decimal.Parse(txtFrete.Text);
                                         obj.Num_item=int.Parse(txtNum_item.Text);
                                         obj.PISV=Decimal.Parse(txtPISV.Text);
                                         obj.COFINSV=Decimal.Parse(txtCOFINSV.Text);
                                         obj.NCM=int.Parse(txtNCM.Text);
                                         obj.Und=txtUnd.Text;
                                         obj.Artigo=txtArtigo.Text;
                                         obj.Peso_liquido=Decimal.Parse(txtPeso_liquido.Text);
                                         obj.Peso_Bruto=Decimal.Parse(txtPeso_Bruto.Text);
                                         obj.Tipo=txtTipo.Text;
                                         obj.CF=txtCF.Text;
                                         obj.CSTPIS=txtCSTPIS.Text;
                                         obj.CSTCOFINS=txtCSTCOFINS.Text;
   }

                  
                  protected void lista_click(object sender, ImageClickEventArgs e)
                  {
                      ImageButton btn = (ImageButton)sender;
                      pnFundo.Visible = true;
                      chkLista.Items.Clear();
                      String sqlLista = "";
                  
                      switch (btn.ID)
                      {
                          case "idBotao":
                              sqlLista = "Query de pesquisa com no minimo 2campos";
                              lbllista.Text = "Pagamentos";
                              camporeceber = "txtPagamento";
                              break;
                      }
                      User usr = (User)Session["User"];
                      SqlDataReader lista = Conexao.consulta(sqlLista, usr);
                  
                      while (lista.Read())
                      {
                          ListItem item = new ListItem();
                          item.Value = lista[0].ToString();
                          item.Text = lista[1].ToString();
                          chkLista.Items.Add(item);
                       }
                  }
                  
                  protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
                  {
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
                               lblError.Text = "N?o foi possivel Excluir o registro error:" +err.Message;
                         }
                  }
                  
                  protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
                  {
                      pnConfima.Visible = false;
                  }
                  
                  protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
                  {
                      TextBox txt = (TextBox)conteudo.FindControl(camporeceber);
                      txt.Text = "";
                      for (int i = 0; i < chkLista.Items.Count; i++)
                      {
                          if (chkLista.Items[i].Selected)
                          {
                              txt.Text += chkLista.Items[i].Value;
                         }
                     }
                     pnFundo.Visible = false;
                  }
                  
                  protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
                  {
                      pnFundo.Visible = false;
                  }
                  
                  
                  
/*================================HTML Pagina Detalhes==========================================
<div class="cabMenu">                  
       <center> <h1>Detalhes do nf_item</h1></center>                  
</div>                  
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">                  
    </asp:Panel>                  
    <br />              
     <asp:Label ID="lblError" runat="server" Text="" ForeColor=Red></asp:Label>              
                  
    <div id="cabecalho" runat="server" class="frame" >               
     <!--Coloque aqui os campos do cabe?alho    -->         
        <table>              
              <tr>    
                  <td></td>
              </tr>    
        </table>          
    </div>              
<div class="opcoes">                  
    <asp:Menu ID="tabMenu" runat="server" Orientation="Horizontal"               
                 OnMenuItemClick="tabMenu_MenuItemClick" Visible="true" > 
                  
       <Items>              
           <asp:MenuItem Text="Primeira Tab" Value="tab1" />         
       </Items>             
       <StaticMenuStyle CssClass="tab" />              
       <StaticMenuItemStyle CssClass="item" />             
       <staticselectedstyle backcolor="Beige" ForeColor="#465c71" />            
    </asp:Menu>              
</div>                  
                  
<div id="conteudo" runat="server" class="conteudo" enableviewstate="false">                  
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">              
       <asp:View ID="view1" runat="server" >              
           <table>              
                <tr>    
/*--Campos Form
                                      <td >                   <p>Filial</p>
                   <asp:TextBox ID="txtFilial" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Codigo</p>
                   <asp:TextBox ID="txtCodigo" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Cliente_Fornecedor</p>
                   <asp:TextBox ID="txtCliente_Fornecedor" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Tipo_NF</p>
                   <td><asp:CheckBox ID="chkTipo_NF" runat="server" Text="Tipo_NF"/>
                   </td>

                                      <td >                   <p>PLU</p>
                   <asp:TextBox ID="txtPLU" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Codigo_Tributacao</p>
                   <asp:TextBox ID="txtCodigo_Tributacao" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Qtde</p>
                   <asp:TextBox ID="txtQtde" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Embalagem</p>
                   <asp:TextBox ID="txtEmbalagem" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Unitario</p>
                   <asp:TextBox ID="txtUnitario" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Desconto</p>
                   <asp:TextBox ID="txtDesconto" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Total</p>
                   <asp:TextBox ID="txtTotal" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>IPI</p>
                   <asp:TextBox ID="txtIPI" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Descricao</p>
                   <asp:TextBox ID="txtDescricao" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>IPIV</p>
                   <asp:TextBox ID="txtIPIV" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>ean</p>
                   <asp:TextBox ID="txtean" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>iva</p>
                   <asp:TextBox ID="txtiva" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>base_iva</p>
                   <asp:TextBox ID="txtbase_iva" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>margem_iva</p>
                   <asp:TextBox ID="txtmargem_iva" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>despesas</p>
                   <asp:TextBox ID="txtdespesas" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>CFOP</p>
                   <asp:TextBox ID="txtCFOP" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>CODIGO_REFERENCIA</p>
                   <asp:TextBox ID="txtCODIGO_REFERENCIA" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>aliquota_icms</p>
                   <asp:TextBox ID="txtaliquota_icms" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>redutor_base</p>
                   <asp:TextBox ID="txtredutor_base" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>codigo_operacao</p>
                   <asp:TextBox ID="txtcodigo_operacao" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Frete</p>
                   <asp:TextBox ID="txtFrete" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Num_item</p>
                   <asp:TextBox ID="txtNum_item" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>PISV</p>
                   <td><asp:CheckBox ID="chkPISV" runat="server" Text="PISV"/>
                   </td>

                                      <td >                   <p>COFINSV</p>
                   <td><asp:CheckBox ID="chkCOFINSV" runat="server" Text="COFINSV"/>
                   </td>

                                      <td >                   <p>NCM</p>
                   <asp:TextBox ID="txtNCM" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Und</p>
                   <asp:TextBox ID="txtUnd" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Artigo</p>
                   <asp:TextBox ID="txtArtigo" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Peso_liquido</p>
                   <asp:TextBox ID="txtPeso_liquido" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Peso_Bruto</p>
                   <asp:TextBox ID="txtPeso_Bruto" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Tipo</p>
                   <asp:TextBox ID="txtTipo" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>CF</p>
                   <asp:TextBox ID="txtCF" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>CSTPIS</p>
                   <asp:TextBox ID="txtCSTPIS" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>CSTCOFINS</p>
                   <asp:TextBox ID="txtCSTCOFINS" runat="server" ></asp:TextBox>
                   </td>


                </tr>    
           </table>          
       </asp:View>              
    </asp:MultiView>                
</div>                  
        <asp:Panel ID="pnFundo" runat="server" CssClass="fundo" Visible =false>          
              <asp:Label ID="lbllista" runat="server" Text="Label" CssClass="cabMenu"></asp:Label>           
                    <table class="frame">
                       <tr>
                           <td>          
                             <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"                    <td>       
                              Width="25px" onclick="btnConfirmaLista_Click"   />           
                              <asp:Label ID="Label4" runat="server" Text="Seleciona" ></asp:Label>          
                           </td>           
                           <td>          
                                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png" 
                                       Width="25px" onclick="btnCancelaLista_Click"  />                   
                                    <asp:Label ID="Label5" runat="server" Text="Cancela" ></asp:Label>     
                           </td>           
                       </tr>
                     </table>
                  
                      <asp:Panel ID="Panel1" runat="server" CssClass="lista" >   
                             <asp:RadioButtonList ID="chkLista" runat="server" Height=50 Width=200>
                             </asp:RadioButtonList>
                      </asp:Panel>
         </asp:Panel>      
                  
         <asp:Panel ID="pnConfima" runat="server" CssClass="fundo" Visible =false>         
           <asp:Label ID="Label1" runat="server" Text="Confirma Exclus?o" CssClass="cabMenu"></asp:Label>         
             <table class="frame">          
                  <tr>     
                      <td>             
                             <asp:ImageButton ID="btnConfirmaExclusao" runat="server" ImageUrl="~/img/confirm.png" 
                                     Width="25px" onclick="btnConfirmaExclusao_Click"  /> 
                                     <asp:Label ID="Label2" runat="server" Text="Confirma" ></asp:Label>
                      </td>
                      <td>
                                    <asp:ImageButton ID="btnCancelaExclusao" runat="server" ImageUrl="~/img/cancel.png" 
                                     Width="25px" onclick="btnCancelaExclusao_Click"  /> 
                                     <asp:Label ID="Label3" runat="server" Text="Cancela" ></asp:Label>
                      </td>
                  </tr>
              </table>     
         </asp:Panel>         
*/

