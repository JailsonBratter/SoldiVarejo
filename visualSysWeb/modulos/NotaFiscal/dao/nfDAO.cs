using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using visualSysWeb.code;
using visualSysWeb.modulos.NotaFiscal.code;
using visualSysWeb.modulos.NotaFiscal.dao;

namespace visualSysWeb.dao
{
    public class nfDAO
    {
        #region Propriedades
        private String strCod = "";
        public String Codigo
        {
            get
            {
                return strCod;
            }
            set
            {
                try
                {
                    strCod = value;

                }
                catch (Exception)
                {

                    throw new Exception("Codigo da nota Inválido");
                }
            }

        }
        public String Cliente_Fornecedor = "";
        public int serie { get; set; }
        private ClienteDAO cli = null;

        public ClienteDAO objCliente
        {
            get
            {
                if (cli == null || !Cliente_Fornecedor.Equals(cli.Codigo_Cliente))
                {
                    cli = new ClienteDAO(Cliente_Fornecedor, usr);
                }
                return cli;
            }


        }
        public ClienteDAO objClienteNovo
        {
            get
            {
                cli = new ClienteDAO(Cliente_Fornecedor, usr);
                return cli;
            }


        }
        public fornecedorDAO objForne= null;
        public fornecedorDAO objFornecedor
        {
            get
            {
                if (!Cliente_Fornecedor.Equals(""))
                {
                    if (objForne == null || !Cliente_Fornecedor.Equals(objForne.Fornecedor))
                    {
                        objForne = new fornecedorDAO(Cliente_Fornecedor, usr);
                    }
                }
                else
                {
                    objForne = null;
                }

                return objForne;
            }
        }

        
        private String vcrt = "";

        public String crt
        {
            get
            {
                if (vcrt.Equals("") && !id.Equals(""))
                {
                    String strSql = "Select emit_CRT from NFe_XML where id ='NFE" + id + "' and emit_crt is not null";
                    vcrt = Conexao.retornaUmValor(strSql, null);
                    return vcrt;

                }
                else
                {
                    return vcrt;
                }
            }
        }

        public String Nome_Fornecedor
        {
            get
            {
                return Conexao.retornaUmValor("Select razao_social from fornecedor where fornecedor= '" + Cliente_Fornecedor + "'", usr);
            }
            set { }
        }

        public String UltimaCorrecaoRegistrada
        {
            get
            {
                return Conexao.retornaUmValor("select top 1 correcao='Correcao:'+convert(varchar(255),correcao)+' <br/>Protocolo:'+protocolo from nfe_correcao  where codigo = '" + Codigo + "' order by data desc", usr);

            }
        }

        public String Nome_cliente
        {
            get
            {
                //if ((!NtOperacao.Saida && NtOperacao.Imprime_NF) || NtOperacao.NF_devolucao)
                if (DestFornecedor)
                    return Nome_Fornecedor;
                else
                    return Conexao.retornaUmValor("Select nome_cliente from cliente where codigo_cliente= '" + Cliente_Fornecedor + "'", usr);

            }
            set { }
        }
        public String vUfcliente = "";
        public String nota_referencia = "";
        public String UfCliente
        {
            get
            {

                if (vUfcliente.Equals(""))
                {
                    //if ((!NtOperacao.Saida && NtOperacao.Imprime_NF) || NtOperacao.NF_devolucao)
                    if (DestFornecedor)
                    {
                        vUfcliente = Conexao.retornaUmValor("select top 1 uf from fornecedor where fornecedor='" + Cliente_Fornecedor + "'", null).ToUpper();
                    }
                    else
                    {
                        vUfcliente = Conexao.retornaUmValor("select top 1 uf from cliente where codigo_cliente='" + Cliente_Fornecedor + "'", null).ToUpper();
                    }
                }
                return vUfcliente;
            }
        }
        public String Tipo_NF { get; set; }
        public DateTime Data { get; set; }
        public String DataBr()
        {
            return dataBr(Data);
        }
        public User usr = new User();

        public natureza_operacaoDAO NtOperacao = new natureza_operacaoDAO();
        public Decimal Codigo_operacao
        {
            get
            {
                return NtOperacao.Codigo_operacao;
            }
            set
            {
                if (value != 0)
                {
                    NtOperacao = new natureza_operacaoDAO(value.ToString(), usr);
                    if (!NtOperacao.Saida)
                        Tipo_NF = "2";
                }
            }
        }
        public int indPres = 0;
        public int indFinal = 0;


        public Decimal Codigo_operacao1 { get; set; }
        public DateTime Emissao { get; set; }
        public String EmissaoBr()
        {
            return dataBr(Emissao);
        }
        public int finNFe = 1;
        private String strFilial = "";
        public String Filial
        {
            get
            {
                return usr.getFilial();
            }
            set
            {
                strFilial = value;
            }
        }
        public Decimal Total { get; set; }
        public Decimal valorTotalProdutos
        {
            get
            {

                TotalProdutos = 0;
                foreach (nf_itemDAO item in NfItens)
                {
                    TotalProdutos += item.vtotal_produto;
                }
                return TotalProdutos;
            }
            set { }
        }

        public Decimal vTotTrib
        {
            get
            {
                Decimal vtot = 0;
                foreach (nf_itemDAO item in NfItens)
                {
                    item.naturezaOperacao = this.NtOperacao;
                    vtot += item.TotTrib;
                }
                return vtot;
            }
        }

        private Decimal vBasePis = 0;
        private Decimal vTotalPis = 0;
        private String vCodigoNotaProdutor = "";
        public Decimal TotalPis
        {
            get
            {
                vTotalPis = 0;
                vBasePis = 0;
                foreach (nf_itemDAO item in NfItens)
                {
                    vTotalPis += item.PISV;
                    vBasePis += item.vBASEPisCofins;
                }

                return vTotalPis;
            }
            set { }

        }
        private Decimal vTotalcofins = 0;
        private Decimal vBaseCofins = 0;
        public Decimal TotalCofins
        {
            get
            {
                vTotalcofins = 0;
                vBaseCofins = 0;
                foreach (nf_itemDAO item in NfItens)
                {
                    vBaseCofins += item.vBASEPisCofins;
                    vTotalcofins += item.COFINSV;
                }
                return vTotalcofins;
            }
            set { }
        }

        public Decimal TotalProdutos { get; set; }

        public Decimal Desconto { get; set; }
        public Decimal Frete { get; set; }
        public Decimal Seguro { get; set; }
        public Decimal IPI_Nota { get; set; }
        public Decimal Outras { get; set; }
        public Decimal ICMS_Nota { get; set; }
        public bool Estado { get; set; }
        public Decimal Base_Calculo { get; set; }
        public Decimal Despesas_financeiras { get; set; }

        public String Pedido = "";
        public Decimal Base_Calc_Subst { get; set; }
        public String Observacao = "";
        public bool nf_Canc { get; set; }
        public String nome_transportadora = "";
        public String endereco_transportadora = "";
        public String municipio_transportadora = "";
        public String estado_transportadora = "";
        public String cnpjTransportadora
        {
            get
            {
                SqlDataReader rsTras = null;
                try
                {
                    rsTras = Conexao.consulta("select top 1 * from transportadora where Nome_transportadora='" + nome_transportadora + "'", null, false);
                    if (rsTras.Read())
                    {
                        String cnpj = (rsTras["cnpj"].ToString().Equals("") ? "" : rsTras["cnpj"].ToString().Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim());
                        endereco_transportadora = (rsTras["endereco"].ToString().Equals("") ? "" : rsTras["endereco"].ToString().Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim());
                        municipio_transportadora = (rsTras["cidade"].ToString().Equals("") ? "" : rsTras["cidade"].ToString().Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim());
                        estado_transportadora = (rsTras["estado"].ToString().Equals("") ? "" : rsTras["estado"].ToString().Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim());
                        return cnpj;
                    }
                    else
                    {
                        return "";

                    }




                }
                catch (Exception)
                {
                    return "";
                }
                finally
                {
                    if (rsTras != null)
                        rsTras.Close();
                }


            }
            set { }
        }
        public Decimal qtde { get; set; }
        public String especie { get; set; }
        public String marca { get; set; }
        public Decimal numero { get; set; }
        public Decimal peso_bruto { get; set; }
        public Decimal peso_liquido { get; set; }
        public String tipo_frete = "0";
        public String funcionario { get; set; }
        public String centro_custo = "";
        public Decimal encargo_financeiro { get; set; }
        public Decimal ICMS_ST { get; set; }
        public String Pedido_cliente { get; set; }
        private String vFornecedor_CNPJ = "";

        public bool notaEntradaManual = true;

        public String Fornecedor_CNPJ
        {
            get
            {
                if (!vFornecedor_CNPJ.Equals(""))
                {
                    return vFornecedor_CNPJ;
                }

                if (!Cliente_Fornecedor.Equals(""))
                {
                    if(DestFornecedor)
                        vFornecedor_CNPJ = Conexao.retornaUmValor("Select top 1 Cnpj from fornecedor where fornecedor='" + Cliente_Fornecedor + "'", null);
                    else
                        vFornecedor_CNPJ = Conexao.retornaUmValor("Select top 1 Cnpj from cliente where codigo_cliente='" + Cliente_Fornecedor + "'", null);
                    
                    return vFornecedor_CNPJ;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                vFornecedor_CNPJ = value;
            }
        }

        public String Placa { get; set; }
        public String Endereco_Entrega { get; set; }
        public Decimal Desconto_geral = 0;
        public String nome_fantasia { get; set; }
        public bool boleto_recebido { get; set; }
        public String usuario = "";
        public String usuario_Alteracao = "";
        public bool nfe { get; set; }
        public bool XML { get; set; }
        public String id = "";
        public String numeroProtocolo = "";
        public List<nf_itemDAO> NfItens = new List<nf_itemDAO>();
        private Hashtable pluIndex = new Hashtable();
        public String status { get; set; }
        public String usuarioPrecificacao = "";
        public DateTime dataPrecificacao { get; set; }
        public String dataprecificacaoBR()
        {
            return dataBr(dataPrecificacao);
        }

        public ArrayList NfPagamentos = new ArrayList();
        private ArrayList itensExcluidos = new ArrayList();
        private ArrayList pagamentosExcluidos = new ArrayList();
        private ArrayList itensAdd = new ArrayList();
        private ArrayList pagamentosAdd = new ArrayList();
        public ArrayList Pedidos = new ArrayList();
        public ArrayList Movimentacoes = new ArrayList();
        private ArrayList MovimentacoesAdd = new ArrayList();
        private ArrayList PedidosAdd = new ArrayList();
        private ArrayList DevolucaoNFEADD = new ArrayList();
        private ArrayList DevolucaoNFe = new ArrayList();

        public List<nf_justificativa_edicaoDAO> histEdicao = new List<nf_justificativa_edicaoDAO>();
        public List<Nf_CentroCustoDAO> LsCentrosCustos = new List<Nf_CentroCustoDAO>();

        public bool precoTodasFiliais = false;
        public bool DestFornecedor = false;
        public bool Ref_ECF = false;
        public bool producao_nfe = false;

        //nfe 4.0
        public Decimal vFCP = 0;
        public Decimal vFCPST = 0;
        public String tPag = "90";
        public Decimal vIPIDevol = 0;
        public int indIEDest = 0;
        public Decimal vCredicmssn  { get; set; } = 0;


        public int indIntermed;
        public string intermedCnpj = "";
        public string idCadIntTran = "";
        public string CNPJPagamento = "";


        public string NumeroReciboSemProtocolo = "";

        public bool OrigemDevolucao = false;
        public int devolucaoNFeCodigo = 0;

        public string validacaoFiscal = "";

        public int fornecedorCRT = 0; //1-Simples Nacional; 2-Simples Naciona - excesso de sublimite da receita bruta; 3- Regime Normal

        public bool entradaDoca = false; //Se a entrada está sendo efetuada pelo sistema de recebimento (SMARTPHONE)
        public Nf_RecebimentoDAO nfRecebimentoDoca = null;

        public decimal vValorDifal = 0; //Cálculo do DIFAL

        public bool StatusConsultadoAntesDaExclusao = false;

        public NF_CTeDAO NFCTe = null;

        public DateTime dataHoraLancamento = DateTime.Parse("1900-01-01");

        #endregion Propriedades

        public nfDAO(User usr, string tipoNF)
        {
            this.usr = usr;
            this.Tipo_NF = tipoNF;
            this.serie = usr.filial.serie_nfe;
        }
        public nfDAO(String codigo, String tipoNf, String clienteFornecedor, User usr) : this(codigo, tipoNf, clienteFornecedor, usr.filial.serie_nfe, usr)
        {
        }

        public nfDAO(String codigo, String tipoNf, String clienteFornecedor, int serie, User usr)
        { //colocar campo index da tabela
            this.Codigo = codigo;
            this.Tipo_NF = tipoNf;
            this.Cliente_Fornecedor = clienteFornecedor;
            this.usr = usr;
            String sql = "Select * from  nf INNER JOIN Natureza_operacao ON NF.Codigo_operacao= Natureza_operacao.Codigo_operacao where natureza_operacao.filial='MATRIZ' and codigo ='" + Codigo + "' and ";

            if (tipoNf.Equals("1"))
                sql += " (TIPO_NF =1 OR (tipo_nf = 2 AND Imprime_NF=1))";
            else
                sql += "(tipo_nf = " + tipoNf + ")";


            sql += " and serie =" + serie;
            sql += " and replace(cliente_fornecedor,'&','E') = '" + Cliente_Fornecedor + "' and nf.filial='" + usr.getFilial() + "'";

            SqlDataReader rs = null;
            try
            {

                rs = Conexao.consulta(sql, null, true);
                carregarDados(rs);
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

        public ArrayList NfReferencias = new ArrayList();
        public ArrayList ECFReferencias = new ArrayList();
        public DataTable NotasReferencias()
        {
            ArrayList itens = new ArrayList();
            ArrayList cabecalho = new ArrayList();
            cabecalho.Add("NOTAS REFERENCIADAS");
            itens.Add(cabecalho);
            if (NfReferencias != null && NfReferencias.Count > 0)
            {
                foreach (String item in NfReferencias)
                {
                    ArrayList arrItem = new ArrayList();
                    arrItem.Add(item);
                    itens.Add(arrItem);
                }
            }
            return Conexao.GetArryTable(itens);
        }
        public void rateiaDesconto(Decimal vlrDesconto)
        {

            Decimal vDescItem = Decimal.Round((vlrDesconto / valorTotalProdutos) * 100, 2); ;
            Decimal vtotalDesc = 0;
            foreach (nf_itemDAO item in NfItens)
            {
                item.Desconto = vDescItem;
                vtotalDesc += item.DescontoValor;

            }
            if (Decimal.Round(vtotalDesc, 2) != Decimal.Round(vlrDesconto, 2))
            {
                nf_itemDAO item = (nf_itemDAO)NfItens[NfItens.Count - 1];
                Decimal vDescArrumar = (vtotalDesc - item.DescontoValor);
                Decimal vDescUlt = vlrDesconto - vDescArrumar;
                item.DescontoValor = vDescUlt;
            }

        }


        public DataTable EcfReferencias()
        {
            ArrayList itens = new ArrayList();
            ArrayList cabecalho = new ArrayList();
            cabecalho.Add("ECF");
            cabecalho.Add("COO");
            cabecalho.Add("Data");
            itens.Add(cabecalho);
            if (ECFReferencias != null && ECFReferencias.Count > 0)
            {
                foreach (ArrayList item in ECFReferencias)
                {

                    itens.Add(item);
                }
            }
            return Conexao.GetArryTable(itens);
        }
        public void addECF(String nECF, String nCOO, DateTime data)
        {
            ArrayList item = new ArrayList();
            item.Add(nECF);
            item.Add(nCOO);
            item.Add(data == null || (data.ToString("dd/MM/yyyy").Equals("01/01/0001")) ? "" : data.ToString("dd/MM/yyyy"));
            ECFReferencias.Add(item);
        }

        public void removeECF(String ECF, String nCOO)
        {
            foreach (ArrayList item in ECFReferencias)
            {
                if (item[0].Equals(ECF) && item[1].Equals(nCOO))
                    ECFReferencias.Remove(item);
                break;
            }
        }

        public DataTable PedidosImportados()
        {
            ArrayList itens = new ArrayList();
            ArrayList cabecalho = new ArrayList();
            cabecalho.Add("Codigo_pedido");
            itens.Add(cabecalho);
            if (Pedidos != null && Pedidos.Count > 0)
            {
                foreach (String item in Pedidos)
                {
                    ArrayList arrItem = new ArrayList();
                    arrItem.Add(item);
                    itens.Add(arrItem);
                }
            }
            return Conexao.GetArryTable(itens);
        }

        public DataTable nfItens()
        {
            ArrayList itens = new ArrayList();
            ArrayList cabecalho = new ArrayList();
            cabecalho.Add("Filial");
            cabecalho.Add("Codigo");
            cabecalho.Add("Cliente_Fornecedor");
            cabecalho.Add("Tipo_NF");
            cabecalho.Add("PLU");
            cabecalho.Add("Codigo_Tributacao");
            cabecalho.Add("Qtde");
            cabecalho.Add("Embalagem");
            cabecalho.Add("Unitario");
            cabecalho.Add("Desconto");
            cabecalho.Add("Total");
            cabecalho.Add("IPI");
            cabecalho.Add("Descricao");
            cabecalho.Add("IPIV");
            cabecalho.Add("ean");
            cabecalho.Add("iva");
            cabecalho.Add("base_iva");
            cabecalho.Add("margem_iva");
            cabecalho.Add("indice_St");
            cabecalho.Add("despesas");
            cabecalho.Add("CFOP");
            cabecalho.Add("CODIGO_REFERENCIA");
            cabecalho.Add("aliquota_icms");
            cabecalho.Add("aliquota_iva");
            cabecalho.Add("redutor_base");
            cabecalho.Add("codigo_operacao");
            cabecalho.Add("Frete");
            cabecalho.Add("Num_item");
            cabecalho.Add("PISV");
            cabecalho.Add("COFINSV");
            cabecalho.Add("NCM");
            cabecalho.Add("Und");
            cabecalho.Add("Artigo");
            cabecalho.Add("Peso_liquido");
            cabecalho.Add("Peso_Bruto");
            cabecalho.Add("Tipo");
            cabecalho.Add("CF");
            cabecalho.Add("CSTPIS");
            cabecalho.Add("CSTCOFINS");
            cabecalho.Add("pCredSN");
            cabecalho.Add("vCredicmssn");
            cabecalho.Add("CEST");
            cabecalho.Add("data_validade");
            cabecalho.Add("DescricaoNotaDeEntrada");
            cabecalho.Add("codigo_centro_custo");
            cabecalho.Add("descricao_centro_custo");


            itens.Add(cabecalho);
            ordenaritens();
            if (NfItens != null && NfItens.Count > 0)
            {
                foreach (nf_itemDAO item in NfItens)
                {
                    itens.Add(item.ArrToString());
                }
            }
            return Conexao.GetArryTable(itens);
        }

        public void addItem(nf_itemDAO item)
        {
            addItem(item, true);
        }
        public void addItem(nf_itemDAO item,bool calcular)
        {
            String LimiteItens = Funcoes.valorParametro("LIMITE_ITENS_NF", usr);
            if (!LimiteItens.Equals(""))
            {
                if (int.Parse(LimiteItens) <= NfItens.Count)
                    throw new Exception("NÃO É PERMITIDO INCLUIR MAIS DO QUE " + LimiteItens + " ITENS NA NOTA FISCAL !");
            }

            if (item.Num_item == 0 || item.Num_item > NfItens.Count + 1)
                item.Num_item = NfItens.Count + 1;

            if (item.PLU != null)
            {
                if (!pluIndex.ContainsKey(item.PLU))
                {
                    pluIndex.Add(item.PLU, NfItens.Count);
                }
            }
            item.naturezaOperacao = NtOperacao;
            
            item.Tipo_NF = Tipo_NF;
            item.finNFe = this.finNFe;
            item.Codigo = this.Codigo;
            item.serie = this.serie;
            item.crt = this.crt;
            item.Cliente_Fornecedor = this.Cliente_Fornecedor;
            if (DestFornecedor)
                item.objFornecedor = objFornecedor;
            item.vTotTrib = item.TotTrib;
            item.buscaCentroCusto();

            //Uso DIFAL
            try
            {
                item.ufCliente = this.cli.UF;
                item.valor_Difal_Item = 0;
                if (indFinal == 1 && !UfCliente.Equals(usr.filial.UF))
                {
                    //item.objCf = new cfDAO(item.NCM, usr);
                    //aliquota_imp_estadoDAO aliq = item.objCf.aliquotaUF(UfCliente);
                    //if (aliq != null)
                    //{
                    //    item.difalContribuinte = true;
                    //    item.vAliquota_iva = aliq.icms_interestadual;
                    //    item.vIva = item.CalculoDifal;
                    //}

                    item.valor_Difal_Item = item.CalculoDifal;
                    //if (item.aliquota_icms < item.aliquota_ICMS_Destino)
                    //{
                    //    item.valor_Difal_Item = (item.vBaseICMS * ((item.aliquota_ICMS_Destino - item.aliquota_icms) / 100));
                    //}
                    //else
                    //{
                    //    item.valor_Difal_Item = 0;
                    //}

                }
            }
            catch
            {

            }


            itensAdd.Add(item);
            NfItens.Add(item);
            recalcularCentroCusto();

            if (calcular)
                calculaTotalItens();
            

        }


        public void addItemAgrupar(nf_itemDAO item)
        {
            addItemAgrupar(item, false);
        }

        public void addItemAgrupar(nf_itemDAO item, bool round)
        {
            String LimiteItens = Funcoes.valorParametro("LIMITE_ITENS_NF", usr);
            if (!LimiteItens.Equals(""))
            {
                if (int.Parse(LimiteItens) <= NfItens.Count)
                    throw new Exception("NÃO É PERMITIDO INCLUIR MAIS DO QUE " + LimiteItens + " ITENS NA NOTA FISCAL !");
            }

            if (item.Num_item == 0 || item.Num_item > NfItens.Count + 1)
                item.Num_item = NfItens.Count + 1;

            if (pluIndex.ContainsKey(item.PLU))
            {
                int index = (int)pluIndex[item.PLU];
                nf_itemDAO nvItem = (nf_itemDAO)NfItens[index];
                nvItem.vtotal_produto += item.vtotal_produto;
                nvItem.vtotal += item.vtotal;
                nvItem.vQtde += item.Qtde;
                nvItem.vTotTrib += nvItem.vTotTrib;

                //o valor unitário, sempre deverá ser calculado para integridade do valor total do item
                Decimal vUnitario = nvItem.vtotal / nvItem.vQtde;
                nvItem.Unitario = vUnitario;

                nvItem.CalculaImpostos(round);
                //if (nvItem.Unitario != item.Unitario)
                //{
                //    //Decimal vQtdTotal = item.Qtde + nvItem.Qtde;
                //    Decimal vUnitario = nvItem.vtotal / nvItem.vQtde;
                //    nvItem.Unitario = vUnitario;
                //}




                // atualizaItem(nvItem);
            }
            else
            {
                item.buscaCentroCusto();
                agruparCentroCusto(item);
                item.vTotTrib = item.TotTrib;
                item.serie = this.serie;
                pluIndex.Add(item.PLU, NfItens.Count);
                itensAdd.Add(item);
                NfItens.Add(item);

            }
           




            //calculaTotalItens();
        }


        /*
        private bool existeItem(String plu)
        {
            int i = 0;
            if (!plu.Trim().Equals(""))
            {
                foreach (nf_itemDAO item in NfItens)
                {
                    if (item.PLU.Equals(plu))
                    {
                        i++;
                    }
                }
                if (i > 1)
                {
                    return true;
                }
            }
            return false;
        }
        */
        private void agruparCentroCusto(nf_itemDAO item)
        {
            if(!item.codigo_centro_custo.Trim().Equals(""))
            {
                if (LsCentrosCustos.Exists(c => c.Codigo_centro_custo.Equals(item.codigo_centro_custo)))
                {
                    Nf_CentroCustoDAO centro = LsCentrosCustos.Find(c => c.Codigo_centro_custo == item.codigo_centro_custo);
                    centro.Valor += item.Total;
                }
                else
                {
                    Nf_CentroCustoDAO centro = new Nf_CentroCustoDAO()
                    {
                        Filial = this.Filial,
                        Codigo = this.Codigo,
                        Codigo_centro_custo = item.codigo_centro_custo, //Faltou este
                        Descricao_centro_custo = item.descricao_centro_custo, // e este tb 
                        Cliente_fornecedor = this.Cliente_Fornecedor,
                        serie = this.serie,
                        Tipo_nf = Funcoes.intTry(this.Tipo_NF),
                        Data = this.Data,
                        Valor = item.Total
                    };
                    LsCentrosCustos.Add(centro);
                }
            }
        }
        public void recalcularCentroCusto()
        {
            LsCentrosCustos.Clear();
            foreach( nf_itemDAO item in NfItens)
            {
                agruparCentroCusto(item);
            }
            
        }
        public void calculaPorcCentroCusto()
        {
            foreach (Nf_CentroCustoDAO item in LsCentrosCustos)
            {
                item.porc = (item.Valor / Total) * 100;
            }

        }
        public void removeItem(nf_itemDAO item)
        {
            NfItens.RemoveAt(item.Num_item - 1);
            //  NfItens.Remove(item);
            itensExcluidos.Add(item);

            int exist = itemJaAdd(item);
            if (exist >= 0)
                itensAdd.RemoveAt(exist);

            ordenaritens();
            calculaTotalItens();
        }

        private int itemJaAdd(nf_itemDAO item)
        {
            int index = 0;
            foreach (nf_itemDAO i in itensAdd)
            {
                if (item.PLU.Equals(i.PLU) && item.Num_item.Equals(item.Num_item))
                    return index;
                index++;
            }
            return -1;
        }
        public void ordenaritens()
        {
            int i = 1;
            List<nf_itemDAO> newItens = new List<nf_itemDAO>();
            foreach (nf_itemDAO it in NfItens)
            {
                it.Num_item = i;
                newItens.Add(it);
                i++;

            }
            NfItens = newItens;
        }
        public nf_itemDAO item(int item)
        {
            nf_itemDAO nfitem = (nf_itemDAO)NfItens[item];
            return nfitem.copia();
        }

        public void atualizaItem(nf_itemDAO item)
        {
            item.vTotTrib = item.TotTrib;
            item.serie = this.serie;
            NfItens[item.Num_item - 1] = item;
            calculaTotalItens();
            ordenaritens();
        }
        public void atualizarSemCalculo(nf_itemDAO item)
        {
            NfItens[item.Num_item - 1] = item;
        }

        public int qtdItens()
        {
            return NfItens.Count;
        }

        public void atribuirCodOperacaoEntrada()
        {
            Decimal nCodOperacao = 0;
            String inicio = "";
            if (NtOperacao.Saida)
            {

                if (UfCliente.Equals(usr.filial.UF))
                {
                    inicio = "5";
                }
                else
                {
                    inicio = "6";
                }
            }
            else
            {
                if (UfCliente.Equals(usr.filial.UF))
                {
                    inicio = "1";
                }
                else
                {
                    inicio = "2";
                }

            }
            nCodOperacao = Decimal.Parse(inicio + Codigo_operacao.ToString().Substring(1));
            foreach (nf_itemDAO item in NfItens)
            {
                item.naturezaOperacao = NtOperacao;
                //item.codigo_operacao = nCodOperacao;
            }

        }

        public void atribuirCodOperacaoItem()
        {
            
            foreach (nf_itemDAO item in NfItens)
            {
                define_cfop(item);
            }
            calculaTotalItens();
        }

        public void calculaTotalItens()
        {

            Total = 0;
            TotalProdutos = 0;
            Decimal TotalCusto = 0;
            Base_Calc_Subst = 0;
            Base_Calculo = 0;
            ICMS_Nota = 0;
            ICMS_ST = 0;
            IPI_Nota = 0;
            vIPIDevol = 0;
            Desconto = 0;
            Despesas_financeiras = 0;
            vFCP = 0;
            vFCPST = 0;
            Frete = 0;
            vTotalPis = 0;
            vTotalcofins = 0;
            vBasePis = 0;
            vBaseCofins = 0;
            vValorDifal = 0;

            foreach (nf_itemDAO item in NfItens)
            {
                item.indFinal = indFinal;
                if (!(Tipo_NF == "2" && notaEntradaManual == false))
                {
                    if (item.indFinal.ToString().Equals("1") && UfCliente != "SP")
                        item.difalContribuinte = true;

                    item.CalculaImpostos();
                }
                //TotalProdutos += item.vtotal_produto;
                TotalProdutos += Decimal.Parse(item.vtotal_produto.ToString("N2"));
                Frete += item.Frete;
                //TotalCusto += item.TotalCusto;
                TotalCusto += Decimal.Parse(item.TotalCusto.ToString("N2"));
                //Temporariamente vai zerar a base
                if (finNFe == 2 && Tipo_NF == "2")
                {
                    Base_Calc_Subst += 0;
                    ICMS_ST += item.vIva;
                    Base_Calculo += item.vBaseICMS;
                    ICMS_Nota += item.vicms;
                    IPI_Nota += item.vIpiv;
                    vIPIDevol += item.vIPIDevol;
                    Desconto += item.vDesconto_valor;
                    Despesas_financeiras += item.despesas;
                    //vFCP += item.vFCP;
                    //vFCPST += item.vFCPST;
                    vBasePis += 0;
                    vBaseCofins +=0;
                    vTotalcofins += item.COFINSV;
                    vTotalPis += item.PISV;
                    vFCP += item.vFCP;
                    vFCPST += item.vFCPST;
                    item.ufCliente = UfCliente;
                    //Difal e FCP
                    vValorDifal += item.valor_Difal_Item;

                }
                else
                {
                    Base_Calc_Subst += item.vBaseIva;
                    ICMS_ST += item.vIva;
                    Base_Calculo += item.vBaseICMS;
                    ICMS_Nota += item.vicms;
                    IPI_Nota += item.vIpiv;
                    vIPIDevol += item.vIPIDevol;
                    Desconto += item.vDesconto_valor;
                    Despesas_financeiras += item.despesas;
                    //vFCP += item.vFCP;
                    //vFCPST += item.vFCPST;
                    vBasePis += item.vBASEPisCofins;
                    vBaseCofins += item.vBASEPisCofins;
                    vTotalcofins += item.COFINSV;
                    vTotalPis += item.PISV;
                    vFCP += item.vFCP;
                    vFCPST += item.vFCPST;
                    item.ufCliente = UfCliente;
                    //Difal e FCP
                    vValorDifal += item.valor_Difal_Item;

                }
            }


            Total = Seguro + Outras + vFCPST;// + vIPIDevol; // Jailson - 2025-06-27 14:15 IPIDevol adicionado. 

            Total += TotalCusto - Desconto_geral;
            if (vIPIDevol > 0)
            {
                Total += vIPIDevol;
            }
            recalcularCentroCusto();
        }

        public void rateiaFrete(Decimal ValorFrete)
        {
            if (ValorFrete > 0)
            {
                Decimal porcFrete = (ValorFrete / TotalProdutos) * 100;
                Decimal vTotalFreteItens = 0;

                if (porcFrete > 0)
                {
                    foreach (nf_itemDAO item in NfItens)
                    {
                        item.Frete = Decimal.Round((item.vtotal_produto * porcFrete) / 100, 2);
                        vTotalFreteItens += item.Frete;
                    }
                }

                Decimal vDifFrete = (ValorFrete - vTotalFreteItens);
                if (vDifFrete != 0)
                {
                    nf_itemDAO item = NfItens.OrderByDescending(o => o.Frete).First();
                    item.Frete += vDifFrete;
                }
            }
            else
            {
                foreach (nf_itemDAO item in NfItens)
                {
                    item.Frete = 0;
                }
            }
            
        }

        public DataTable nfPagamento()
        {
            ArrayList pagamentos = new ArrayList();
            ArrayList cabecalho = new ArrayList();
            cabecalho.Add("Vencimento");
            cabecalho.Add("Filial");
            cabecalho.Add("Codigo");
            cabecalho.Add("Cliente_Fornecedor");
            cabecalho.Add("Tipo_NF");
            cabecalho.Add("Tipo_pagamento");
            cabecalho.Add("Valor");
            cabecalho.Add("Boleto_Recebido");
            cabecalho.Add("Cod_barra");
            pagamentos.Add(cabecalho);
            if (NfItens != null && NfPagamentos.Count > 0)
            {
                foreach (nf_pagamentoDAO pg in NfPagamentos)
                {
                    pagamentos.Add(pg.ArrToString());
                }
            }
            return Conexao.GetArryTable(pagamentos);
        }


        public void addPagamento(nf_pagamentoDAO pg)
        {
            ordenarPagamentos();
            pg.Filial = Filial;
            pg.ordem = NfPagamentos.Count + 1;
            pg.serie = this.serie;
            pg.Tipo_NF = Tipo_NF;
            pg.emissao = Emissao;
            pg.entrada = Data;
            pg.centroCusto = centro_custo;
            pagamentosAdd.Add(pg);
            NfPagamentos.Add(pg);
        }


        public void removePagamento(int pg)
        {
            pagamentosExcluidos.Add(NfPagamentos[pg]);
            NfPagamentos.Remove(NfPagamentos[pg]);

        }

        public String pagamentoAvista()
        {

            if (NfPagamentos.Count > 1)
            {
                return "1";
            }
            else
            {
                if (NfPagamentos.Count == 1)
                {
                    nf_pagamentoDAO pg = (nf_pagamentoDAO)NfPagamentos[0];
                    if (pg.Tipo_pagamento.ToUpper().Equals("AVISTA"))
                    {
                        return "0";
                    }
                    else
                    {
                        return "1";
                    }
                }
                else
                {
                    return "2";
                }
            }
        }
        public Decimal Total_Pag { get; set; }
        public string Ordem_compra { get; internal set; } = "";

        public Decimal TotalPag()
        {
            Decimal total = 0;

            foreach (nf_pagamentoDAO pg in NfPagamentos)
            {
                total += pg.Valor;
            }
            return total;
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
        private void carregarHistorico()
        {
            String sql = " Select * from nf_justificativa_edicao " +
                            " where filial ='" + Filial + "'" +
                            "   and codigo_nota='" + Codigo + "'" +
                            "   and tipo_nf=" + Tipo_NF +
                            "   and cliente_fornecedor ='" + Cliente_Fornecedor + "'" +
                            "   and serie =" + serie +
                            " order by data_alteracao desc ";
            SqlDataReader rs = null;
            try
            {
                histEdicao.Clear();
                rs = Conexao.consulta(sql, null, false);
                while (rs.Read())
                {

                    nf_justificativa_edicaoDAO just = new nf_justificativa_edicaoDAO()
                    {
                        filial = Filial,
                        codigo_nota = Codigo,
                        cliente_fornecedor = Cliente_Fornecedor,
                        serie = serie,
                        tipo_nf = Funcoes.intTry(Tipo_NF),
                        data_alteracao = Funcoes.dtTry(rs["data_alteracao"].ToString()),
                        usuario = rs["usuario"].ToString(),
                        justificativa = rs["justificativa"].ToString()
                    };


                    histEdicao.Add(just);

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
        public void carregarCentrosDeCusto()
        {
            String sql = " Select nc.* , c.descricao_centro_custo from NF_CentroCusto as nc " +
                           " inner join centro_custo as c on nc.codigo_centro_custo = c.codigo_centro_custo " +
                           " where nc.filial ='" + Filial + "'" +
                           "   and nc.codigo='" + Codigo + "'" +
                           "   and nc.tipo_nf=" + Tipo_NF +
                           "   and nc.serie ="+serie+
                           "   and nc.cliente_fornecedor ='" + Cliente_Fornecedor + "'" +

                           " order by nc.codigo_centro_custo ";
            SqlDataReader rs = null;
            try
            {
                LsCentrosCustos.Clear();
                rs = Conexao.consulta(sql, null, false);
                while (rs.Read())
                {

                    Nf_CentroCustoDAO centro = new Nf_CentroCustoDAO()
                    {
                        Filial = Filial,
                        Codigo = Codigo,
                        Cliente_fornecedor = Cliente_Fornecedor,
                        Tipo_nf = Funcoes.intTry(Tipo_NF),
                        serie = Funcoes.intTry(rs["serie"].ToString()),
                        Data = Funcoes.dtTry(rs["data"].ToString()),
                        Codigo_centro_custo = rs["codigo_centro_custo"].ToString(),
                        Descricao_centro_custo = rs["descricao_centro_custo"].ToString(),
                        Valor = Funcoes.decTry(rs["valor"].ToString())
                    };


                    LsCentrosCustos.Add(centro);

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
                    Codigo = rs["Codigo"].ToString();
                    Cliente_Fornecedor = rs["Cliente_Fornecedor"].ToString();
                    serie = Funcoes.intTry(rs["serie"].ToString());
                    Tipo_NF = rs["Tipo_NF"].ToString();
                    Ordem_compra = rs["Ordem_compra"].ToString();
                    Data = (rs["Data"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data"].ToString()));
                    dataPrecificacao = (rs["Data_precificacao"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data_precificacao"].ToString()));
                    usuarioPrecificacao = rs["usuario_precificacao"].ToString();
                    Codigo_operacao = (Decimal)(rs["Codigo_operacao"].ToString().Equals("") ? new Decimal() : rs["Codigo_operacao"]);
                    Codigo_operacao1 = (Decimal)(rs["Codigo_operacao1"].ToString().Equals("") ? new Decimal() : rs["Codigo_operacao1"]);
                    Emissao = (rs["Emissao"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Emissao"].ToString()));
                    Filial = rs["Filial"].ToString();
                    Total = (Decimal)(rs["Total"].ToString().Equals("") ? new Decimal() : rs["Total"]);
                    TotalProdutos = Funcoes.decTry(rs["total_produto"].ToString());
                    Desconto = (Decimal)(rs["Desconto"].ToString().Equals("") ? new Decimal() : rs["Desconto"]);
                    Frete = (Decimal)(rs["Frete"].ToString().Equals("") ? new Decimal() : rs["Frete"]);
                    Seguro = (Decimal)(rs["Seguro"].ToString().Equals("") ? new Decimal() : rs["Seguro"]);
                    IPI_Nota = (Decimal)(rs["IPI_Nota"].ToString().Equals("") ? new Decimal() : rs["IPI_Nota"]);
                    Outras = (Decimal)(rs["Outras"].ToString().Equals("") ? new Decimal() : rs["Outras"]);
                    ICMS_Nota = (Decimal)(rs["ICMS_Nota"].ToString().Equals("") ? new Decimal() : rs["ICMS_Nota"]);
                    Estado = (rs["Estado"].ToString().Equals("1") ? true : false);
                    Base_Calculo = (Decimal)(rs["Base_Calculo"].ToString().Equals("") ? new Decimal() : rs["Base_Calculo"]);
                    Despesas_financeiras = (Decimal)(rs["Despesas_financeiras"].ToString().Equals("") ? new Decimal() : rs["Despesas_financeiras"]);
                    Pedido = rs["Pedido"].ToString();
                    Base_Calc_Subst = (Decimal)(rs["Base_Calc_Subst"].ToString().Equals("") ? new Decimal() : rs["Base_Calc_Subst"]);
                    Observacao = rs["Observacao"].ToString();
                    nf_Canc = (rs["nf_Canc"].ToString().Equals("1") ? true : false);
                    nome_transportadora = rs["nome_transportadora"].ToString();
                    qtde = (Decimal)(rs["qtde"].ToString().Equals("") ? new Decimal() : rs["qtde"]);
                    especie = rs["especie"].ToString();
                    marca = rs["marca"].ToString();
                    numero = (Decimal)(rs["numero"].ToString().Equals("") ? new Decimal() : rs["numero"]);
                    peso_bruto = (Decimal)(rs["peso_bruto"].ToString().Equals("") ? new Decimal() : rs["peso_bruto"]);
                    peso_liquido = (Decimal)(rs["peso_liquido"].ToString().Equals("") ? new Decimal() : rs["peso_liquido"]);
                    tipo_frete = rs["tipo_frete"].ToString();
                    funcionario = rs["funcionario"].ToString();
                    centro_custo = rs["centro_custo"].ToString();
                    encargo_financeiro = (Decimal)(rs["encargo_financeiro"].ToString().Equals("") ? new Decimal() : rs["encargo_financeiro"]);
                    ICMS_ST = (Decimal)(rs["ICMS_ST"].ToString().Equals("") ? new Decimal() : rs["ICMS_ST"]);
                    Pedido_cliente = rs["Pedido_cliente"].ToString();
                    Fornecedor_CNPJ = rs["Fornecedor_CNPJ"].ToString();
                    Placa = rs["Placa"].ToString();
                    Endereco_Entrega = rs["Endereco_Entrega"].ToString();
                    Desconto_geral = (Decimal)(rs["Desconto_geral"].ToString().Equals("") ? new Decimal() : rs["Desconto_geral"]);
                    nome_fantasia = rs["nome_fantasia"].ToString();
                    boleto_recebido = (rs["boleto_recebido"].ToString().Equals("1") ? true : false);
                    usuario = rs["usuario"].ToString();
                    usuario_Alteracao = rs["Usuario_Alteracao"].ToString();
                    nfe = (rs["nfe"].ToString().Equals("1") ? true : false);
                    XML = (rs["XML"].ToString().Equals("1") ? true : false);
                    DestFornecedor = (rs["Dest_Fornec"].ToString().Equals("1") ? true : false);
                    precoTodasFiliais = (rs["precoTodasFiliais"].ToString().Equals("1") ? true : false);
                    id = rs["id"].ToString();
                    status = rs["status"].ToString();
                    indPres = (rs["indPres"].ToString().Equals("") ? 2 : int.Parse(rs["indPres"].ToString()));
                    indFinal = (rs["indFinal"].ToString().Equals("") ? 0 : int.Parse(rs["indFinal"].ToString()));
                    indIntermed = Funcoes.intTry(rs["indIntermed"].ToString());
                    intermedCnpj = rs["intermedCnpj"].ToString();
                    idCadIntTran = rs["idCadIntTran"].ToString();
                    CNPJPagamento = rs["CNPJPagamento"].ToString();

                    Ref_ECF = (rs["Ref_ECF"].ToString().Equals("1") ? true : false);
                    //nota_referencia = rs["nota_referencia"].ToString();
                    vCodigoNotaProdutor = rs["CodigoNotaProdutor"].ToString();
                    numeroProtocolo = rs["numero_protocolo"].ToString();
                    int.TryParse(rs["finNfe"].ToString(), out finNFe);
                    Decimal.TryParse(rs["vFCP"].ToString(), out vFCP);
                    Decimal.TryParse(rs["vFCPST"].ToString(), out vFCPST);
                    tPag = rs["tPag"].ToString();
                    vTotalPis = Funcoes.decTry(rs["pisv"].ToString());
                    vTotalcofins = Funcoes.decTry(rs["cofinsv"].ToString());
                    vBasePis = Funcoes.decTry(rs["base_pis"].ToString());
                    vBaseCofins = Funcoes.decTry(rs["base_cofins"].ToString());
                    vCredicmssn = Funcoes.decTry(rs["vCredicmssn"].ToString());
                    validacaoFiscal = rs["Validacao_Fiscal"].ToString();

                    NfItens = nf_itemDAO.itens(Codigo, Tipo_NF, serie, Cliente_Fornecedor, NtOperacao, nf_Canc, usr);
                    NfPagamentos = nf_pagamentoDAO.pagamentos(Codigo, Tipo_NF, serie, Cliente_Fornecedor, usr);
                    carregarHistorico();
                    carregarCentrosDeCusto();
                    carregaCTe();

                    if (!Ref_ECF)
                    {
                        SqlDataReader rsRef = null;
                        try
                        {


                            rsRef = Conexao.consulta("select Id from nf_devolucao where codigo_nf='" + Codigo + "'", usr, false);
                            NfReferencias.Clear();
                            while (rsRef.Read())
                            {
                                NfReferencias.Add(rsRef["id"].ToString());
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        finally
                        {
                            if (rsRef != null)
                                rsRef.Close();
                        }
                    }
                    else
                    {
                        SqlDataReader rsRef = null;
                        try
                        {


                            rsRef = Conexao.consulta("select nECF,nCOO,data_documento from nf_devolucao where codigo_nf='" + Codigo + "'", usr, false);
                            ECFReferencias.Clear();
                            while (rsRef.Read())
                            {
                                ArrayList item = new ArrayList();
                                item.Add(rsRef["nECF"].ToString());
                                item.Add(rsRef["nCOO"].ToString());
                                item.Add(rsRef["data_documento"].ToString().Equals("") ? "" : DateTime.Parse(rsRef["data_documento"].ToString()).ToString("dd/MM/yyyy"));
                                ECFReferencias.Add(item);
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        finally
                        {
                            if (rsRef != null)
                                rsRef.Close();
                        }
                    }

                    SqlDataReader rsPedidos = null;
                    try
                    {


                        rsPedidos = Conexao.consulta("select Codigo_Pedido from Nota_Pedido where Codigo_Nota ='" + Codigo + "'", usr, false);
                        Pedidos.Clear();
                        while (rsPedidos.Read())
                        {
                            Pedidos.Add(rsPedidos["Codigo_pedido"].ToString());
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {
                        if (rsPedidos != null)
                            rsPedidos.Close();
                    }
                    SqlDataReader rsMov = null;
                    try
                    {


                        rsMov = Conexao.consulta("select Codigo_Movimentacao from nota_movimentacao where Codigo_Nota ='" + Codigo + "'", usr, false);
                        Movimentacoes.Clear();
                        while (rsMov.Read())
                        {
                            Movimentacoes.Add(rsMov["Codigo_Movimentacao"].ToString());
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {
                        if (rsMov != null)
                            rsMov.Close();
                    }


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
        private void ordenarPagamentos()
        {
            int ordem = 1;
            foreach (nf_pagamentoDAO item in NfPagamentos)
            {
                item.ordem = ordem;
                ordem++;
            }

        }


        private void update(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                if (Tipo_NF.Equals("2"))
                {
                    try
                    {
                        String strLog1 = "insert into nf_log " +
                                            "select codigo,cliente_fornecedor,tipo_nf,data,codigo_operacao,codigo_operacao1,emissao,filial,total,desconto,frete,seguro,ipi_nota,outras,icms_nota,estado,base_calculo,despesas_financeiras,pedido,base_calc_subst,observacao,nf_canc,nome_transportadora,qtde,especie,marca,numero,peso_bruto,peso_liquido,tipo_frete,funcionario,centro_custo,encargo_financeiro,icms_st,pedido_cliente,fornecedor_cnpj,placa,endereco_entrega,desconto_geral,nome_fantasia,boleto_recebido,usuario,nfe,xml,id,serie,status, '" + usr.getNome() + "',getdate() " +
                                                " from nf where codigo='" + Codigo + "' and tipo_nf = 2 and cliente_fornecedor ='" + Cliente_Fornecedor + "' and serie =" + serie;
                        Conexao.executarSql(strLog1, conn, tran);

                        String strLog2 = "insert into nf_item_log " +
                                            "select Filial,codigo,cliente_fornecedor,tipo_nf,plu,codigo_tributacao,qtde,embalagem,unitario,desconto,total,ipi,descricao,ipiv,ean,iva,base_iva,margem_iva,despesas,cfop,codigo_referencia,aliquota_icms,redutor_base,codigo_operacao,frete,num_item,pisv,cofinsv,ncm,und,artigo,peso_liquido,peso_bruto,tipo,cf,cstpis,cstcofins,pcredsn,vcredicmssn, '" + usr.getNome() + "',getdate(),aliquota_iva,indice_st,serie" +
                                            " from nf_item where codigo='" + Codigo + "' and tipo_nf = 2 and cliente_fornecedor ='" + Cliente_Fornecedor + "' and serie=" + serie;
                        Conexao.executarSql(strLog2, conn, tran);
                    }
                    catch (Exception err)
                    {
                        throw new Exception("Salvar Log:" + err.Message);
                    }



                }


                String sql = "update  nf set " +
                              "Codigo='" + Codigo + "'" +
                              ",Cliente_Fornecedor='" + Cliente_Fornecedor + "'" +
                              ",Tipo_NF=" + Tipo_NF +
                              ",Data=" + (Data.Equals("0001-01-01") ? "null" : "'" + Data.ToString("yyyy-MM-dd") + "'") +
                              ",Codigo_operacao=" + Codigo_operacao.ToString().Replace(",", ".") +
                              ",Codigo_operacao1=" + Codigo_operacao1.ToString().Replace(",", ".") +
                              ",Emissao=" + (Emissao.Equals("0001-01-01") ? "null" : "'" + Emissao.ToString("yyyy-MM-dd") + "'") +
                              ",Filial='" + Filial + "'" +
                              ",Total=" + Funcoes.decimalPonto(Total.ToString()) +
                              ",Desconto=" + Funcoes.decimalPonto(Desconto.ToString()) +
                              ",Frete=" + Funcoes.decimalPonto(Frete.ToString()) +
                              ",Seguro=" + Funcoes.decimalPonto(Seguro.ToString()) +
                              ",IPI_Nota=" + Funcoes.decimalPonto(IPI_Nota.ToString()) +
                              ",Outras=" + Funcoes.decimalPonto(Outras.ToString()) +
                              ",ICMS_Nota=" + Funcoes.decimalPonto(ICMS_Nota.ToString()) +
                              ",Estado=" + (Estado ? "1" : "0") +
                              ",Base_Calculo=" + Funcoes.decimalPonto(Base_Calculo.ToString()) +
                              ",Despesas_financeiras=" + Funcoes.decimalPonto(Despesas_financeiras.ToString()) +
                              ",Pedido='" + Pedido + "'" +
                              ",Base_Calc_Subst=" + Funcoes.decimalPonto(Base_Calc_Subst.ToString()) +
                              ",Observacao='" + Observacao + "'" +
                              ",nf_Canc=" + (nf_Canc ? "1" : "0") +
                              ",nome_transportadora='" + nome_transportadora + "'" +
                              ",qtde=" + Funcoes.decimalPonto(qtde.ToString()) +
                              ",especie='" + especie + "'" +
                              ",marca='" + marca + "'" +
                              ",numero=" + Funcoes.decimalPonto(numero.ToString()) +
                              ",peso_bruto=" + Funcoes.decimalPonto(peso_bruto.ToString()) +
                              ",peso_liquido=" + Funcoes.decimalPonto(peso_liquido.ToString()) +
                              ",tipo_frete=" + tipo_frete +
                              ",funcionario='" + funcionario + "'" +
                              ",centro_custo='" + centro_custo + "'" +
                              ",encargo_financeiro=" + Funcoes.decimalPonto(encargo_financeiro.ToString()) +
                              ",ICMS_ST=" + Funcoes.decimalPonto(ICMS_ST.ToString()) +
                              ",Pedido_cliente='" + Pedido_cliente + "'" +
                              ",Fornecedor_CNPJ='" + Fornecedor_CNPJ + "'" +
                              ",Placa='" + Placa + "'" +
                              ",Endereco_Entrega='" + Endereco_Entrega + "'" +
                              ",Desconto_geral=" + Funcoes.decimalPonto(Desconto_geral.ToString()) +
                              ",nome_fantasia='" + nome_fantasia + "'" +
                              ",boleto_recebido=" + (boleto_recebido ? "1" : "0") +
                              ",usuario='" + usuario + "'" +
                              ",nfe=" + (nfe ? "1" : "0") +
                              ",XML=" + (XML ? "1" : "0") +
                              ",id='" + id + "'" +
                              ",status='" + status + "'" +
                              ",indPres =" + indPres +
                              ",indFinal=" + indFinal +
                              ",Dest_Fornec=" + (DestFornecedor ? "1" : "0") +
                              ",Ref_ECF=" + (Ref_ECF ? "1" : "0") +
                              ",pisv=" + Funcoes.decimalPonto(vTotalPis.ToString()) +
                              ",cofinsv=" + Funcoes.decimalPonto(vTotalcofins.ToString()) +
                              ",base_pis=" + Funcoes.decimalPonto(vBasePis.ToString()) +
                              ",base_cofins=" + Funcoes.decimalPonto(vBaseCofins.ToString()) +
                              ",total_produto=" + Funcoes.decimalPonto(TotalProdutos.ToString()) +
                              ",CodigoNotaProdutor='" + vCodigoNotaProdutor + "'" +
                              ",finNFe=" + finNFe +
                              ",vFCP=" + Funcoes.decimalPonto(vFCP.ToString()) +
                              ",vFCPST=" + Funcoes.decimalPonto(vFCPST.ToString()) +
                              ",tPag='" + tPag + "'" +
                              ",serie =" + serie.ToString() +
                              ",Usuario_Alteracao ='"+usuario_Alteracao+"'"+
                              ",vCredicmssn="+Funcoes.decimalPonto(vCredicmssn.ToString())+
                              ",ordem_compra='"+Ordem_compra+"'"+
                              ",indIntermed=" + indIntermed.ToString() +
                              ",intermedCnpj='" + intermedCnpj.Trim() + "'" +
                              ",idCadIntTran='" + idCadIntTran.Trim() + "'" +
                              ",CNPJPagamento='" + CNPJPagamento + "'" +
                              ",DataHora_Lancamento = '" + dataHoraLancamento.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                    " where filial='" + Filial + "' AND  RTRIM(LTRIM(codigo)) = '" + Codigo.Trim() + "' and tipo_nf = " + Tipo_NF + " and cliente_fornecedor = '" + Cliente_Fornecedor + "'";

                Conexao.executarSql(sql, conn, tran);

                if (nf_Canc)
                {
                    if (Movimentacoes.Count > 0)
                    {
                        String strSqlMov = " update Inventario set Importado_Nota =null , Imp_Nota_emissao=null" +
                                            " where importado_nota = " + Codigo + "  and Imp_Nota_emissao ='" + Emissao.ToString("yyyy-MM-dd") + "' and filial='" + usr.getFilial() + "'";
                        Conexao.executarSql(strSqlMov, conn, tran);


                    }
                    String tipo = Tipo_NF;
                    if (NtOperacao.NF_devolucao)
                    {
                        if (Ref_ECF)
                        {
                            tipo = "3";
                        }
                        else
                        {
                            tipo = "4";
                        }
                    }
                    foreach (String pd in Pedidos)
                    {
                        Conexao.executarSql("update pedido set status=1 where pedido='" + pd + "' and tipo=" + tipo + " and filial ='" + Filial + "' and isnull(pedido_simples,0) <>1 ", conn, tran);
                    }

                    foreach (nf_pagamentoDAO pg in NfPagamentos)
                    {
                        pg.NaturezaOperacao = this.NtOperacao;
                        pg.vCodigoNotaProdutor = this.vCodigoNotaProdutor;
                        pg.serie = this.serie;
                        pg.excluir(conn, tran);
                    }

                }


                if (Tipo_NF.Equals("2"))
                {
                    if (itensExcluidos.Count > 0)
                    {
                        foreach (nf_itemDAO item in itensExcluidos)
                        {
                            item.serie = this.serie;
                            item.excluir(conn, tran, this.Data);
                        }
                    }
                }
                
                //if (itensAdd.Count > 0)
                //{
                //    foreach (nf_itemDAO item in itensAdd)
                //    {
                //        item.Codigo = this.Codigo;
                //        item.Cliente_Fornecedor = this.Cliente_Fornecedor;
                //        item.naturezaOperacao = NtOperacao;
                //        item.serie = this.serie;
                //        item.salvar(true, conn, tran);
                //    }
                //    itensAdd = new ArrayList();
                //}
                if(!nf_Canc)
                    Conexao.executarSql("delete from nf_item where codigo ='" + Codigo + "' and tipo_nf = " + Tipo_NF + " and cliente_fornecedor = '" + Cliente_Fornecedor + "'", conn, tran);


                foreach (nf_itemDAO item in NfItens)
                {
                    item.naturezaOperacao = NtOperacao;
                    if (nf_Canc)
                        item.cancela(conn, tran, Data);
                    else
                        item.salvar(true, conn, tran);

                    item.atualizarItem(conn,tran, Data);
                }







                // if (Tipo_NF.Equals("1"))

                if (Tipo_NF.Equals("2") && !NtOperacao.Imprime_NF)
                {
                    if (pagamentosExcluidos.Count > 0)
                    {
                        foreach (nf_pagamentoDAO pg in pagamentosExcluidos)
                        {
                            pg.NaturezaOperacao = this.NtOperacao;
                            pg.vCodigoNotaProdutor = this.vCodigoNotaProdutor;
                            pg.serie = this.serie;
                            pg.excluir(conn, tran);
                        }
                        pagamentosExcluidos = new ArrayList();
                    }


                    if (pagamentosAdd.Count > 0)
                    {
                        foreach (nf_pagamentoDAO pg in pagamentosAdd)
                        {
                            pg.NaturezaOperacao = this.NtOperacao;
                            pg.vCodigoNotaProdutor = this.vCodigoNotaProdutor;
                            pg.serie = this.serie;
                            pg.salvar(true, conn, tran);
                        }
                        pagamentosAdd = new ArrayList();
                    }
                }
                else
                {
                    Conexao.executarSql("delete from nf_pagamento  where filial='" + Filial + "' and codigo ='" + Codigo + "' and tipo_nf = " + Tipo_NF + " and cliente_fornecedor = '" + Cliente_Fornecedor + "' and serie =" + serie, conn, tran);
                    int ordem = 1;
                    foreach (nf_pagamentoDAO pg in NfPagamentos)
                    {
                        pg.Filial = Filial;
                        pg.Cliente_Fornecedor = Cliente_Fornecedor;
                        pg.Codigo = Codigo;
                        pg.serie = this.serie;
                        pg.Tipo_NF = Tipo_NF;
                        pg.emissao = Emissao;
                        pg.centroCusto = centro_custo;
                        pg.NaturezaOperacao = NtOperacao;
                        pg.ordem = ordem;
                        pg.boleto_recebido = this.boleto_recebido;
                        pg.vCodigoNotaProdutor = this.vCodigoNotaProdutor;
                        pg.salvar(true, conn, tran);
                        ordem++;

                    }
                }

                Conexao.executarSql("delete from nf_devolucao where codigo_nf='" + Codigo + "'", conn, tran);
                if (!nf_Canc)
                {
                    if (NfReferencias.Count > 0)
                    {
                        foreach (String item in NfReferencias)
                        {
                            Conexao.executarSql("insert into nf_devolucao(codigo_nf,id) values ('" + Codigo + "','" + item + "')", conn, tran);

                        }
                    }

                    if (ECFReferencias.Count > 0)
                    {
                        foreach (ArrayList item in ECFReferencias)
                        {
                            if (!item[0].ToString().Equals(""))
                                Conexao.executarSql("insert into nf_devolucao(codigo_nf,nECF,nCOO,data_documento) values ('" + Codigo + "','" + item[0] + "','" + item[1] + "'," + (item[2].Equals("") ? "null" : "'" + DateTime.Parse(item[2].ToString()).ToString("yyyy-MM-dd") + "')"), conn, tran);

                        }
                    }
                }

            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }
        public void importarNFeProdutor(String chave)
        {
            SqlDataReader rsTotal = null;
            SqlDataReader rsItens = null;
            SqlDataReader rsPg = null;
            SqlDataReader rsFornecedor = null;
            try
            {
                rsTotal = Conexao.consulta("Exec sp_NFe_Total 'NFE" + chave + "'", usr, true);

                rsTotal.Read();
                String Cnpj_Cpf = Conexao.retornaUmValor("select  emit_CNPJ = isnull(emit_CNPJ,emit_CPF) FROM NFe_XML where id LIKE'%NFe" + chave + "' and((emit_CNPJ is not null and emit_CNPJ <>'' ) or emit_CPF is Not null) group by emit_CNPJ,emit_CPF", usr);
                int tFornecedor = Conexao.countSql("SELECT cnpj FROM Fornecedor WHERE replace(replace(replace(CNPJ,'.',''),'/',''),'-','')='" + Cnpj_Cpf + "'", usr);

                if (tFornecedor <= 0)
                {
                    throw new Exception("Fornecedor não Cadastrado");
                }


                Emissao = DateTime.Now.Date;
                Tipo_NF = "2";
                DestFornecedor = true;
                usuario = usr.getNome();
                Data = DateTime.Now.Date;
                nome_transportadora = Conexao.retornaUmValor("Select Nome_transportadora from Transportadora where padrao = 1", null);

                rsFornecedor = Conexao.consulta("SELECT * FROM Fornecedor WHERE replace(replace(replace(CNPJ,'.',''),'/',''),'-','')='" + Cnpj_Cpf + "'", usr, true);
                if (rsFornecedor.Read())
                {
                    Fornecedor_CNPJ = rsFornecedor["CNPJ"].ToString();
                    Cliente_Fornecedor = rsFornecedor["Fornecedor"].ToString();
                    vUfcliente = rsFornecedor["UF"].ToString();
                }

                NfItens.Clear();
                String strSqlItens = "Exec sp_NFe_det 'NFE" + chave + "'";
                int tItens = Conexao.countSql(strSqlItens, usr);
                if (tItens <= 0)
                {
                    throw new Exception("NFe foi importada com problemas, favor entrar em contato com o suporte.");
                }
                rsItens = Conexao.consulta(strSqlItens, usr, true);

                while (rsItens.Read())
                {
                    if (vCodigoNotaProdutor.Equals(""))
                    {
                        vCodigoNotaProdutor = rsItens["ide_nNF"].ToString();
                    }

                    nf_itemDAO item = new nf_itemDAO(usr);
                    item.Codigo = Codigo;
                    item.Cliente_Fornecedor = Cliente_Fornecedor;
                    item.Tipo_NF = Tipo_NF;

                    String strPlu = "";
                    String strSqlplu = "";
                    if (rsItens["det_prod_cEAN"] != null && rsItens["det_prod_cEAN"].ToString().Trim().Length > 0)
                    {
                        String teste = rsItens["det_prod_cEAN"].ToString();
                        strSqlplu = "SELECT TOP 1 plu FROM EAN WHERE EAN.EAN ='" + long.Parse(rsItens["det_prod_cEAN"].ToString().Trim()).ToString() + "'";
                        strPlu = Conexao.retornaUmValor(strSqlplu, usr);
                    }
                    if (strPlu.Equals(""))
                    {

                        strSqlplu = "SELECT TOP 1 plu,embalagem FROM Fornecedor_Mercadoria WHERE RTRIM(LTRIM(Codigo_Referencia)) = '" + rsItens["det_prod_cProd"].ToString().Trim() + "'" +
                                    " AND RTRIM(LTRIM(Fornecedor)) = '" + Cliente_Fornecedor + "'";
                        SqlDataReader rsMercadoriaLoja = null;
                        try
                        {


                            rsMercadoriaLoja = Conexao.consulta(strSqlplu, usr, false);
                            if (rsMercadoriaLoja.Read())
                            {
                                strPlu = rsMercadoriaLoja["plu"].ToString();
                                item.Embalagem = (rsMercadoriaLoja["embalagem"].ToString().Equals("") ? 0 : Decimal.Parse(rsMercadoriaLoja["embalagem"].ToString()));


                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        finally
                        {
                            if (rsMercadoriaLoja != null)
                                rsMercadoriaLoja.Close();
                        }

                    }

                    //Verifica se o PLU Está cadastrado
                    if (Funcoes.intTry(
                        Conexao.retornaUmValor("Select count(*) from mercadoria where plu ='" + strPlu + "'", null)
                        ) > 0)
                    {
                        item.PLU = strPlu;

                    }
                    else
                    {
                        strPlu = "";
                    }
                    if (!strPlu.Equals(""))
                    {
                        String srtSqlMercadoria = "SELECT TOP 1 * FROM MERCADORIA WHERE PLU ='" + strPlu + "'";
                        SqlDataReader rsMercadoria = null;
                        try
                        {


                            rsMercadoria = Conexao.consulta(srtSqlMercadoria, usr, true);
                            if (rsMercadoria.Read())
                            {
                                if (item.Embalagem <= 0)
                                {
                                    item.Embalagem = (rsMercadoria["embalagem"].ToString().Equals("") ? 0 : Decimal.Parse(rsMercadoria["embalagem"].ToString()));
                                    if (item.Embalagem <= 0)
                                    {
                                        item.Embalagem = 1;
                                    }
                                }
                                item.Descricao = rsMercadoria["Descricao"].ToString();
                                item.vOrigem = int.Parse((rsMercadoria["Origem"].ToString().Equals("") ? "0" : rsMercadoria["Origem"].ToString()));
                                item.inativo = rsMercadoria["inativo"].ToString().Equals("1");
                                //nfe 4.0
                                item.indEscala = rsMercadoria["indEscala"].ToString().Equals("S");
                                item.cnpj_Fabricante = rsMercadoria["cnpj_Fabricante"].ToString();
                                item.cBenef = rsMercadoria["cBenef"].ToString();
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        finally
                        {
                            if (rsMercadoria != null)
                                rsMercadoria.Close();

                        }

                    }
                    else
                    {

                        item.Descricao = rsItens["det_prod_xProd"].ToString().Replace("&", "").Replace("#", "");
                        if (item.Embalagem <= 0)
                        {
                            item.Embalagem = 1;
                        }
                        item.indEscala = true;
                    }
                    String strSqlCST = "";
                    Decimal vRedutor = 0;
                    Decimal vRedutorST = 0;
                    item.vtotal_produto = Funcoes.decTry(rsItens["det_prod_vProd"].ToString());
                    switch (rsItens["det_icms_CST"].ToString())
                    {
                        case "60":
                        case "40":
                        case "41":
                        case "50":
                        case "51":
                            strSqlCST = "SELECT TOP 1 Codigo_Tributacao FROM Tributacao WHERE Indice_ST = '" + rsItens["det_icms_CST"].ToString() + "'";
                            break;
                        case "10":
                        case "00":
                            strSqlCST = "SELECT TOP 1 Codigo_Tributacao FROM Tributacao WHERE Indice_ST = '" + rsItens["det_icms_CST"].ToString() + "'" +
                                        " AND Entrada_ICMS =" + Decimal.Parse(rsItens["det_icms_pICMS"].ToString()).ToString("N2").Replace(",", ".");
                            break;
                        case "20":

                        case "70":
                            Decimal detIcmsVbc = (rsItens["det_icms_vBC"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_vBC"].ToString()));
                            Decimal detProdvProd = (rsItens["det_prod_vProd"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vProd"].ToString()));
                            Decimal detProdvDesc = (rsItens["det_prod_vDesc"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vDesc"].ToString()));

                            vRedutor = (100 - (detIcmsVbc / (detProdvProd - detProdvDesc) * 100));
                            if (vRedutor > 30 && vRedutor < 39)
                            {
                                vRedutor = 33.33m;
                            }
                            else if (vRedutor > 39 && vRedutor < 42)
                            {
                                vRedutor = 41.67m;
                            }
                            else if (vRedutor > 59 && vRedutor < 65)
                            {
                                vRedutor = 61.11m;
                            }
                            else if (vRedutor > 49 && vRedutor < 55)
                            {
                                vRedutor = 52.00m;
                            }

                            strSqlCST = "SELECT TOP 1 Codigo_Tributacao FROM Tributacao WHERE Indice_ST ='" + rsItens["det_icms_CST"].ToString() + "'" +
                                          " AND Entrada_ICMS =" + Decimal.Parse(rsItens["det_icms_pICMS"].ToString()).ToString("N2").Replace(",", ".") +
                                          " AND Redutor = " + vRedutor.ToString("N2").Replace(",", ".");

                            Decimal.TryParse(rsItens["det_icms_pRedBCST"].ToString(), out vRedutorST); 
                            break;
                        default:

                            if (!rsItens["det_icms_csosn"].ToString().Equals(""))
                            {
                                strSqlCST = "SELECT TOP 1 Codigo_Tributacao FROM Tributacao WHERE Indice_ST = '" + rsItens["det_icms_csosn"].ToString() + "'";
                                item.pCredSN = (rsItens["det_icms_pcredsn"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_pcredsn"].ToString()));
                                item.vCredicmssn = (rsItens["det_icms_vcredicmssn"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_vcredicmssn"].ToString()));
                                item.recalcular = false;

                            }
                            else
                            {
                                strSqlCST = "SELECT TOP 1 Codigo_Tributacao FROM Tributacao WHERE Indice_ST = '99'";
                            }
                            break;


                    }

                    String strCST = Conexao.retornaUmValor(strSqlCST, usr);
                    item.Codigo_Tributacao = (strCST.Equals("") ? 0 : Decimal.Parse(strCST));
                    item.redutor_base = vRedutor;
                    item.redutor_base_ST = vRedutorST;

                    String strCFOP = rsItens["det_prod_CFOP"].ToString();
                    strCFOP = (Decimal.Parse(strCFOP) - 4000).ToString().Substring(0, 4);
                    switch (strCFOP.Substring(1))
                    {
                        case "401":
                        case "402":
                        case "403":
                        case "404":
                        case "405":
                        case "406":
                            strCFOP = strCFOP.Substring(0, 1) + "403";
                            break;
                        case "101":
                        case "102":
                        case "104":
                        case "105":
                        case "106":
                            strCFOP = strCFOP.Substring(0, 1) + "102";
                            break;
                    }

                    item.codigo_operacao = Decimal.Parse(strCFOP);

                    item.Filial = usr.getFilial();
                    item.CODIGO_REFERENCIA = rsItens["det_prod_cProd"].ToString().Trim();
                    item.Qtde = (rsItens["det_prod_qCom"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_qCom"].ToString()));
                    item.Unitario = (rsItens["det_prod_vUnCom"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vUnCom"].ToString()));
                    item.Total = item.Total;

                    if (item.Embalagem > 1)
                    {
                        item.Unitario = (item.Unitario / item.Embalagem);
                    }

                    item.Desconto = (rsItens["det_prod_vDesc"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vDesc"].ToString()));
                    if (item.Desconto > 0)
                    {
                        item.Desconto = ((item.Desconto / Decimal.Parse(rsItens["det_prod_vProd"].ToString())) * 100);
                    }
                    item.despesas = (rsItens["det_prod_vOutro"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vOutro"].ToString()));


                    item.porcIPI = (rsItens["det_ipi_pIPI"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_ipi_pIPI"].ToString()));

                    item.vIpiv = (rsItens["det_ipi_vIPI"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_ipi_vIPI"].ToString()));

                    if ((item.porcIPI > 0) && (item.vIpiv == 0))
                    {
                        item.vIpiv = ((item.vtotal_produto - item.Desconto) * (item.porcIPI) / 100);
                    }
                    if ((item.vIpiv > 0) && (item.porcIPI == 0))
                    {
                        item.porcIPI = ((item.IPIV / (item.vtotal_produto - item.Desconto)) * 100);
                    }
                    item.vBaseICMS = Funcoes.decTry(rsItens["det_icms_vBC"].ToString());
                    item.aliquota_icms = Funcoes.decTry(rsItens["det_icms_pICMS"].ToString());
                    item.vicms = Funcoes.decTry(rsItens["det_icms_vICMS"].ToString());

                    item.vBaseIva = (rsItens["det_icms_vBCST"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_vBCST"].ToString()));
                    item.vmargemIva = (rsItens["det_icms_pMVAST"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_pMVAST"].ToString()));
                    item.vIva = (rsItens["det_icms_vICMSST"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_vICMSST"].ToString()));
                    item.vOrigem = int.Parse((rsItens["det_icms_orig"].ToString().Equals("") ? "0" : rsItens["det_icms_orig"].ToString()));

                    if (item.vIva > 0 && item.vmargemIva == 0)
                        item.vmargemIva = item.CalculoMargem_iva;


                    item.Num_item = (rsItens["det_nItem"].ToString().Equals("") ? 0 : int.Parse(rsItens["det_nItem"].ToString()));


                    item.PISV = (rsItens["det_pis_vPIS"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_pis_vPIS"].ToString()));
                    item.COFINSV = (rsItens["det_cofins_vCofins"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_cofins_vCofins"].ToString()));
                    item.NCM = (rsItens["det_prod_NCM"].ToString().Equals("") ? "" : rsItens["det_prod_NCM"].ToString());

                    item.PISp = (rsItens["det_pis_pPIS"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_pis_pPIS"].ToString()));
                    item.COFINSp = (rsItens["det_cofins_pCofins"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_cofins_pCofins"].ToString()));

                    String StrcstPis = Conexao.retornaUmValor("select CST_entrada from PISCofins_CST_DePara where CST_saida='" + rsItens["det_pis_CST"].ToString().Trim() + "'", null);
                    item.CSTPIS = (StrcstPis.Equals("") ? "99" : StrcstPis);
                    String StrcstCofins = Conexao.retornaUmValor("select CST_entrada from PISCofins_CST_DePara where CST_saida='" + rsItens["det_cofins_CST"].ToString().Trim() + "'", null);
                    item.CSTCOFINS = (StrcstCofins.Equals("") ? "99" : StrcstCofins);


                    item.Und = (rsItens["det_prod_uCOM"].ToString().Equals("") ? "" : rsItens["det_prod_uCOM"].ToString());

                    //NFE 4.0
                    try { item.indEscala = rsItens["det_prod_indEscala"].ToString().Equals("S"); } catch (Exception) { }
                    try { item.cnpj_Fabricante = rsItens["det_prod_CNPJFab"].ToString(); } catch (Exception) { }
                    try { item.cBenef = rsItens["det_prod_cBenef"].ToString(); } catch (Exception) { }
                    try { Decimal.TryParse(rsItens["det_ICMS_vBCFCP"].ToString(), out item.vBCFCP); } catch (Exception) { }
                    try { Decimal.TryParse(rsItens["det_ICMS_pFCP"].ToString(), out item.pFCP); } catch (Exception) { }
                    try { Decimal.TryParse(rsItens["det_ICMS_vFCP"].ToString(), out item.vFCP); } catch (Exception) { }
                    try { Decimal.TryParse(rsItens["det_ICMS_vBCFCPST"].ToString(), out item.vBCFCPST); } catch (Exception) { }
                    try { Decimal.TryParse(rsItens["det_ICMS_pFCPST"].ToString(), out item.pFCPST); } catch (Exception) { }
                    try { Decimal.TryParse(rsItens["det_ICMS_vFCPST"].ToString(), out item.vFCPST); } catch (Exception) { }

                    
                    if (item.PLU == null)
                        item.PLU = "";

                    addItem(item, notaEntradaManual);
                }


                recalcularCentroCusto();
                Filial = usr.getFilial();
                Total = (rsTotal["Total_ICMSTot_vNF"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vNF"].ToString()));
                Frete = (rsTotal["Total_ICMSTot_vFrete"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vFrete"].ToString()));
                Despesas_financeiras = (rsTotal["Total_ICMSTot_vOutro"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vOutro"].ToString()));
                Seguro = (rsTotal["Total_ICMSTot_vSeg"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vSeg"].ToString()));
                IPI_Nota = (rsTotal["Total_ICMSTot_vIPI"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vIPI"].ToString()));
                Base_Calculo = (rsTotal["Total_ICMSTot_vBC"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vBC"].ToString()));
                ICMS_Nota = (rsTotal["Total_ICMSTot_vICMS"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vICMS"].ToString()));
                Base_Calc_Subst = (rsTotal["Total_ICMSTot_vBCST"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vBCST"].ToString()));
                ICMS_ST = (rsTotal["Total_ICMSTot_vST"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vST"].ToString()));
                //NFE 4.0
                try { Decimal.TryParse(rsTotal["Total_ICMSTot_vFCP"].ToString(), out vFCP); } catch { }
                try { Decimal.TryParse(rsTotal["Total_ICMSTot_vFCPST"].ToString(), out vFCPST); } catch { }



                NfPagamentos.Clear();
                String sqlContasPG = "Exec sp_NFe_Cobr  'NFE" + chave + "'";
                rsPg = Conexao.consulta(sqlContasPG, usr, true);

                while (rsPg.Read())
                {
                    nf_pagamentoDAO pg = new nf_pagamentoDAO(usr);
                    pg.Vencimento = (rsPg["cobr_dup_dVenc"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rsPg["cobr_dup_dVenc"].ToString()));
                    pg.Filial = Filial;
                    pg.Codigo = Codigo;
                    pg.Cliente_Fornecedor = Cliente_Fornecedor;
                    pg.Tipo_NF = Tipo_NF;
                    pg.Tipo_pagamento = "BOLETO";
                    pg.Valor = (rsPg["cobr_dup_vDup"].ToString().Equals("") ? 0 : Decimal.Parse(rsPg["cobr_dup_vDup"].ToString()));
                    addPagamento(pg);
                }



            }
            catch (Exception err)
            {

                throw err;
            }
            finally
            {
                if (rsPg != null)
                    rsPg.Close();

                if (rsTotal != null)
                    rsTotal.Close();

                if (rsItens != null)
                    rsItens.Close();

                if (rsFornecedor != null)
                    rsFornecedor.Close();
            }
        }


        public void importarNFeDev(String chave, List<NfItemImportaDev> itensSelecionados)
        {
            SqlDataReader rsTotal = null;
            SqlDataReader rsItens = null;
            SqlDataReader rsPg = null;
            SqlDataReader rsFornecedor = null;


            bool simples = usr.filial.Reg_Federal.ToUpper().Equals("SIMPLES NACIONAL");

            try
            {
                rsTotal = Conexao.consulta("Exec sp_NFe_Total 'NFE" + chave + "'", usr, true);

                rsTotal.Read();
                String Cnpj_Cpf = Conexao.retornaUmValor("select  emit_CNPJ = isnull(emit_CNPJ,emit_CPF) FROM NFe_XML where id LIKE'%NFe" + chave + "' and((emit_CNPJ is not null and emit_CNPJ <>'' ) or emit_CPF is Not null) group by emit_CNPJ,emit_CPF", usr);
                int tFornecedor = Conexao.countSql("SELECT cnpj FROM Fornecedor WHERE replace(replace(replace(CNPJ,'.',''),'/',''),'-','')='" + Cnpj_Cpf + "'", usr);

                if (tFornecedor <= 0)
                {
                    throw new Exception("Fornecedor não Cadastrado");
                }


                Emissao = DateTime.Now.Date;
                Tipo_NF = "2";
                DestFornecedor = true;
                finNFe = 4;
                usuario = usr.getNome();
                Data = DateTime.Now.Date;
                NfReferencias.Add(chave);
                rsFornecedor = Conexao.consulta("SELECT * FROM Fornecedor WHERE replace(replace(replace(CNPJ,'.',''),'/',''),'-','')='" + Cnpj_Cpf + "'", usr, true);
                if (rsFornecedor.Read())
                {
                    Fornecedor_CNPJ = rsFornecedor["CNPJ"].ToString();
                    Cliente_Fornecedor = rsFornecedor["Fornecedor"].ToString();
                    vUfcliente = rsFornecedor["UF"].ToString();
                    centro_custo = rsFornecedor["Centro_Custo"].ToString();
                }

                //NfItens.Clear();
                String strSqlItens = "Exec sp_NFe_det 'NFE" + chave + "'";
                int tItens = Conexao.countSql(strSqlItens, usr);
                if (tItens <= 0)
                {
                    throw new Exception("NFe foi importada com problemas, favor entrar em contato com o suporte.");
                }
                rsItens = Conexao.consulta(strSqlItens, usr, true);

                while (rsItens.Read())
                {


                    nf_itemDAO item = new nf_itemDAO(usr);
                    item.Codigo = Codigo;
                    item.Cliente_Fornecedor = Cliente_Fornecedor;
                    item.Tipo_NF = Tipo_NF;

                    String strPlu = "";
                    String strSqlplu = "";
                    if (rsItens["det_prod_cEAN"] != null && !rsItens["det_prod_cEAN"].ToString().Equals("SEM GTIN") && rsItens["det_prod_cEAN"].ToString().Trim().Length > 0)
                    {
                        String teste = rsItens["det_prod_cEAN"].ToString();
                        strSqlplu = "SELECT TOP 1 plu FROM EAN WHERE EAN.EAN ='" + long.Parse(rsItens["det_prod_cEAN"].ToString().Trim()).ToString() + "'";
                        strPlu = Conexao.retornaUmValor(strSqlplu, usr);
                    }
                    if (strPlu.Equals(""))
                    {

                        strSqlplu = "SELECT TOP 1 plu,embalagem FROM Fornecedor_Mercadoria WHERE RTRIM(LTRIM(Codigo_Referencia)) = '" + rsItens["det_prod_cProd"].ToString().Trim() + "'" +
                                    " AND RTRIM(LTRIM(Fornecedor)) = '" + Cliente_Fornecedor + "'";
                        SqlDataReader rsMercadoriaLoja = null;
                        try
                        {


                            rsMercadoriaLoja = Conexao.consulta(strSqlplu, usr, false);
                            if (rsMercadoriaLoja.Read())
                            {
                                strPlu = rsMercadoriaLoja["plu"].ToString();
                                item.Embalagem = (rsMercadoriaLoja["embalagem"].ToString().Equals("") ? 0 : Decimal.Parse(rsMercadoriaLoja["embalagem"].ToString()));


                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        finally
                        {
                            if (rsMercadoriaLoja != null)
                                rsMercadoriaLoja.Close();
                        }

                    }

                    //Verifica se o PLU Está cadastrado
                    if (Funcoes.intTry(
                        Conexao.retornaUmValor("Select count(*) from mercadoria where plu ='" + strPlu + "'", null)
                        ) > 0)
                    {
                        item.PLU = strPlu;

                    }
                    else
                    {
                        strPlu = "";
                    }
                    bool containsItem = itensSelecionados.Any(i => i.plu.Equals(strPlu));
                    if (containsItem)
                    {

                        item.PLU = strPlu;
                        if (!strPlu.Equals(""))
                        {
                            String srtSqlMercadoria = "SELECT TOP 1 * FROM MERCADORIA WHERE PLU ='" + strPlu + "'";
                            SqlDataReader rsMercadoria = null;
                            try
                            {


                                rsMercadoria = Conexao.consulta(srtSqlMercadoria, usr, true);
                                if (rsMercadoria.Read())
                                {
                                    if (item.Embalagem <= 0)
                                    {
                                        item.Embalagem = (rsMercadoria["embalagem"].ToString().Equals("") ? 0 : Decimal.Parse(rsMercadoria["embalagem"].ToString()));
                                        if (item.Embalagem <= 0)
                                        {
                                            item.Embalagem = 1;
                                        }
                                    }
                                    item.Descricao = rsMercadoria["Descricao"].ToString();
                                    item.vOrigem = int.Parse((rsMercadoria["Origem"].ToString().Equals("") ? "0" : rsMercadoria["Origem"].ToString()));
                                    item.inativo = rsMercadoria["inativo"].ToString().Equals("1");
                                    //nfe 4.0
                                    item.indEscala = rsMercadoria["indEscala"].ToString().Equals("S");
                                    item.cnpj_Fabricante = rsMercadoria["cnpj_Fabricante"].ToString();
                                    item.cBenef = rsMercadoria["cBenef"].ToString();
                                 
                                }
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            finally
                            {
                                if (rsMercadoria != null)
                                    rsMercadoria.Close();
                            }
                        }
                        else
                        {

                            item.Descricao = rsItens["det_prod_xProd"].ToString().Replace("&", "").Replace("#", "");
                            if (item.Embalagem <= 0)
                            {
                                item.Embalagem = 1;
                            }
                            item.indEscala = true;
                        }
                        String strSqlCST = "";
                        Decimal vRedutor = 0;
                        Decimal vRedutorST = 0;

                        switch (rsItens["det_icms_CST"].ToString())
                        {
                            case "60":
                            case "40":
                            case "41":
                            case "50":
                            case "51":
                                strSqlCST = "SELECT TOP 1 Codigo_Tributacao FROM Tributacao WHERE Indice_ST = '" + rsItens["det_icms_CST"].ToString() + "'";
                                break;
                            case "10":
                            case "00":
                                strSqlCST = "SELECT TOP 1 Codigo_Tributacao FROM Tributacao WHERE Indice_ST = '" + rsItens["det_icms_CST"].ToString() + "'" +
                                            " AND Entrada_ICMS =" + Decimal.Parse(rsItens["det_icms_pICMS"].ToString()).ToString("N2").Replace(",", ".");
                                break;
                            case "20":

                            case "70":
                                Decimal detIcmsVbc = (rsItens["det_icms_vBC"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_vBC"].ToString()));
                                Decimal detProdvProd = (rsItens["det_prod_vProd"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vProd"].ToString()));
                                Decimal detProdvDesc = (rsItens["det_prod_vDesc"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vDesc"].ToString()));
                                vRedutor = (100 - (detIcmsVbc / (detProdvProd - detProdvDesc) * 100));
                                if (vRedutor > 30 && vRedutor < 39)
                                {
                                    vRedutor = 33.33m;
                                }
                                else if (vRedutor > 39 && vRedutor < 42)
                                {
                                    vRedutor = 41.67m;
                                }
                                else if (vRedutor > 59 && vRedutor < 65)
                                {
                                    vRedutor = 61.11m;
                                }
                                else if (vRedutor > 49 && vRedutor < 55)
                                {
                                    vRedutor = 52.00m;
                                }

                                strSqlCST = "SELECT TOP 1 Codigo_Tributacao FROM Tributacao WHERE Indice_ST ='" + rsItens["det_icms_CST"].ToString() + "'" +
                                              " AND Entrada_ICMS =" + Decimal.Parse(rsItens["det_icms_pICMS"].ToString()).ToString("N2").Replace(",", ".") +
                                              " AND Redutor = " + vRedutor.ToString("N2").Replace(",", ".");

                                Decimal.TryParse(rsItens["det_icms_pRedBCST"].ToString(), out vRedutorST);

                                item.redutor_base_ST = vRedutorST;

                                break;
                            default:

                                if (!rsItens["det_icms_csosn"].ToString().Equals(""))
                                {
                                    strSqlCST = "SELECT TOP 1 Codigo_Tributacao FROM Tributacao WHERE Indice_ST = '" + rsItens["det_icms_csosn"].ToString() + "'";
                                    item.pCredSN = (rsItens["det_icms_pcredsn"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_pcredsn"].ToString()));
                                    item.vCredicmssn = (rsItens["det_icms_vcredicmssn"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_vcredicmssn"].ToString()));
                                    item.recalcular = false;
                                    item.vIndiceSt = rsItens["det_icms_csosn"].ToString();

                                }
                                else
                                {
                                    strSqlCST = "SELECT TOP 1 Codigo_Tributacao FROM Tributacao WHERE Indice_ST = '99'";
                                }
                                break;

                        }

                        String strCST = Conexao.retornaUmValor(strSqlCST, usr);
                        item.Codigo_Tributacao = (strCST.Equals("") ? 0 : Decimal.Parse(strCST));
                        item.redutor_base = vRedutor;

                        if (!simples)
                        {
                            item.vIndiceSt = Conexao.retornaUmValor("SELECT TOP 1 Indice_ST FROM Tributacao WHERE Codigo_Tributacao = " + item.Codigo_Tributacao.ToString(), usr);
                        }
                        else
                        {
                            item.vIndiceSt = Conexao.retornaUmValor("SELECT TOP 1 CSOSN FROM Tributacao WHERE Codigo_Tributacao = " + item.Codigo_Tributacao.ToString(), usr);
                        }

                        //Quando se tratar de devolução
                        if (this.NtOperacao.NF_devolucao && item.Codigo_Tributacao == 0 && item.vIndiceSt.Equals("") )
                        {
                            try
                            {
                                item.vIndiceSt = rsItens["det_icms_CST"].ToString();
                            }
                            catch
                            {
                            }
                        }


                        item.vBaseIva = (rsItens["det_icms_vBCST"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_vBCST"].ToString()));
                        item.vmargemIva = (rsItens["det_icms_pMVAST"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_pMVAST"].ToString()));
                        item.vIva = (rsItens["det_icms_vICMSST"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_vICMSST"].ToString()));


                        define_cfop(item);

                        item.Filial = usr.getFilial();
                        item.CODIGO_REFERENCIA = rsItens["det_prod_cProd"].ToString().Trim();
                      
                        item.Qtde = (rsItens["det_prod_qCom"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_qCom"].ToString()));
                        item.Unitario = (rsItens["det_prod_vUnCom"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vUnCom"].ToString()));

                        if (item.Embalagem > 1)
                        {
                            item.Unitario = (item.Unitario / item.Embalagem);
                        }

                        item.vtotal_produto = Funcoes.decTry(rsItens["det_prod_vProd"].ToString());

                        item.Desconto = (rsItens["det_prod_vDesc"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vDesc"].ToString()));
                        if (item.Desconto > 0)
                        {
                            item.Desconto = ((item.Desconto / Decimal.Parse(rsItens["det_prod_vProd"].ToString())) * 100);
                        }
                        item.despesas = (rsItens["det_prod_vOutro"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vOutro"].ToString()));


                        item.porcIPI = (rsItens["det_ipi_pIPI"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_ipi_pIPI"].ToString()));

                        item.vIpiv = (rsItens["det_ipi_vIPI"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_ipi_vIPI"].ToString()));

                        if ((item.porcIPI > 0) && (item.vIpiv == 0))
                        {
                            item.vIpiv = ((item.vtotal_produto - item.Desconto) * (item.porcIPI) / 100);
                        }
                        if ((item.vIpiv > 0) && (item.porcIPI == 0))
                        {
                            item.porcIPI = ((item.IPIV / (item.vtotal_produto- item.Desconto)) * 100);
                        }
                        item.aliquota_icms = (rsItens["det_icms_pICMS"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_pICMS"].ToString()));

                        //** Popular de acordo com a NFe
                        item.vBaseICMS = (rsItens["det_icms_vBC"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_vBC"].ToString()));
                        item.vicms = (rsItens["det_icms_vICMS"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_vICMS"].ToString()));

                        item.vOrigem = int.Parse((rsItens["det_icms_orig"].ToString().Equals("") ? "0" : rsItens["det_icms_orig"].ToString()));

                        if (item.vIva > 0 && item.vmargemIva == 0)
                            item.vmargemIva = item.CalculoMargem_iva;

                       

                        item.Num_item = (rsItens["det_nItem"].ToString().Equals("") ? 0 : int.Parse(rsItens["det_nItem"].ToString()));

                        item.vBASEPisCofins = Funcoes.decTry(rsItens["det_pis_vBC"].ToString());

                        item.PISV = (rsItens["det_pis_vPIS"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_pis_vPIS"].ToString()));
                        item.COFINSV = (rsItens["det_cofins_vCofins"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_cofins_vCofins"].ToString()));
                        item.NCM = (rsItens["det_prod_NCM"].ToString().Equals("") ? "" : rsItens["det_prod_NCM"].ToString());

                        item.PISp = (rsItens["det_pis_pPIS"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_pis_pPIS"].ToString()));
                        item.COFINSp = (rsItens["det_cofins_pCofins"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_cofins_pCofins"].ToString()));


                        String StrcstPis = "";
                        String StrcstCofins = "";
                        if (NtOperacao.Saida)
                        {
                            StrcstPis = rsItens["det_pis_CST"].ToString().Trim();
                            StrcstCofins = rsItens["det_cofins_CST"].ToString().Trim();
                        }
                        else
                        {
                            StrcstPis = Conexao.retornaUmValor("select CST_entrada from PISCofins_CST_DePara where CST_saida='" + rsItens["det_pis_CST"].ToString().Trim() + "'", null);
                            StrcstCofins = Conexao.retornaUmValor("select CST_entrada from PISCofins_CST_DePara where CST_saida='" + rsItens["det_cofins_CST"].ToString().Trim() + "'", null);

                        }


                        item.CSTPIS = (StrcstPis.Equals("") ? "99" : StrcstPis);
                        item.CSTCOFINS = (StrcstCofins.Equals("") ? "99" : StrcstCofins);


                        item.Und = (rsItens["det_prod_uCOM"].ToString().Equals("") ? "" : rsItens["det_prod_uCOM"].ToString());

                        //NFE 4.0
                        try { item.indEscala = rsItens["det_prod_indEscala"].ToString().Equals("S"); } catch (Exception) { }
                        try { item.cnpj_Fabricante = rsItens["det_prod_CNPJFab"].ToString(); } catch (Exception) { }
                        try { item.cBenef = rsItens["det_prod_cBenef"].ToString(); } catch (Exception) { }
                        try { Decimal.TryParse(rsItens["det_ICMS_vBCFCP"].ToString(), out item.vBCFCP); } catch (Exception) { }
                        try { Decimal.TryParse(rsItens["det_ICMS_pFCP"].ToString(), out item.pFCP); } catch (Exception) { }
                        try { Decimal.TryParse(rsItens["det_ICMS_vFCP"].ToString(), out item.vFCP); } catch (Exception) { }
                        try { Decimal.TryParse(rsItens["det_ICMS_vBCFCPST"].ToString(), out item.vBCFCPST); } catch (Exception) { }
                        try { Decimal.TryParse(rsItens["det_ICMS_pFCPST"].ToString(), out item.pFCPST); } catch (Exception) { }
                        try { Decimal.TryParse(rsItens["det_ICMS_vFCPST"].ToString(), out item.vFCPST); } catch (Exception) { }

                       
                        item.Total = item.Total;

                        addItem(item);
                    }
                }

                // recalcularCentroCusto(); //Função calculaTotalItens() já faz este trabalho.
                Filial = usr.getFilial();

                //**Comentado para calcular apenas os itens que foram devolvidos. A parte abaixo comentada pq estava sempre popuçando total da NFe

                //Total = Funcoes.decTry(rsTotal["Total_ICMSTot_vNF"].ToString());

                //TotalProdutos = Funcoes.decTry(rsTotal["Total_ICMSTot_vProd"].ToString());
                //Desconto = Funcoes.decTry(rsTotal["Total_ICMSTot_vDesc"].ToString());

                //Frete = (rsTotal["Total_ICMSTot_vFrete"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vFrete"].ToString()));
                //Despesas_financeiras = (rsTotal["Total_ICMSTot_vOutro"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vOutro"].ToString()));
                //Seguro = (rsTotal["Total_ICMSTot_vSeg"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vSeg"].ToString()));
                //IPI_Nota = (rsTotal["Total_ICMSTot_vIPI"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vIPI"].ToString()));
                //Base_Calculo = (rsTotal["Total_ICMSTot_vBC"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vBC"].ToString()));
                //ICMS_Nota = (rsTotal["Total_ICMSTot_vICMS"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vICMS"].ToString()));
                //Base_Calc_Subst = (rsTotal["Total_ICMSTot_vBCST"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vBCST"].ToString()));
                //ICMS_ST = (rsTotal["Total_ICMSTot_vST"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vST"].ToString()));
                ////NFE 4.0
                //try { Decimal.TryParse(rsTotal["Total_ICMSTot_vFCP"].ToString(), out vFCP); } catch { }
                //try { Decimal.TryParse(rsTotal["Total_ICMSTot_vFCPST"].ToString(), out vFCPST); } catch { }

                //** Nota de devolução não pode gerar titulos a receber

                //NfPagamentos.Clear();
                //String sqlContasPG = "Exec sp_NFe_Cobr  'NFE" + chave + "'";
                //rsPg = Conexao.consulta(sqlContasPG, usr, true);

                //while (rsPg.Read())
                //{
                //    nf_pagamentoDAO pg = new nf_pagamentoDAO(usr);
                //    pg.Vencimento = (rsPg["cobr_dup_dVenc"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rsPg["cobr_dup_dVenc"].ToString()));
                //    pg.Filial = Filial;
                //    pg.Codigo = Codigo;
                //    pg.Cliente_Fornecedor = Cliente_Fornecedor;
                //    pg.Tipo_NF = Tipo_NF;
                //    pg.Tipo_pagamento = "BOLETO";
                //    pg.Valor = (rsPg["cobr_dup_vDup"].ToString().Equals("") ? 0 : Decimal.Parse(rsPg["cobr_dup_vDup"].ToString()));
                //    addPagamento(pg);
                //}



            }
            catch (Exception err)
            {

                throw err;
            }
            finally
            {
                if (rsPg != null)
                    rsPg.Close();

                if (rsTotal != null)
                    rsTotal.Close();

                if (rsItens != null)
                    rsItens.Close();

                if (rsFornecedor != null)
                    rsFornecedor.Close();
            }
        }

        internal fornecedorDAO novoFornecedorNfe(string chave)
        {
            SqlDataReader rs = null;
            fornecedorDAO forn = new fornecedorDAO(this.usr);
            try
            {
                String sql = @"Select	
                                pessoaFisica = case when emit_CNPJ is null then 1 else 0 end,
                                emit_CNPJ,
                                emit_CPF,
		                        emit_IE,
		                        emit_xFant,
		                        emit_xNome,
		                        emit_enderEmit_CEP,
		                        emit_enderEmit_xLgr,
		                        emit_enderEmit_nro,
		                        emit_enderEmit_xBairro,
		                        emit_enderEmit_xMun,
		                        emit_enderEmit_UF,
		                        emit_enderEmit_cMun,
		                        emit_enderEmit_fone
                            from NFe_XML
                            where ID = 'NFe" + chave+"' AND emit_xNome is not null; ";
                rs = Conexao.consulta(sql, null, false);
                if (rs.Read())
                {
                    forn.pessoa_fisica = rs["pessoaFisica"].ToString().Equals("1");
                    forn.CNPJ = (forn.pessoa_fisica? rs["emit_CPF"].ToString(): Funcoes.FormatCNPJ(rs["emit_CNPJ"].ToString()));
                    forn.IE = rs["emit_IE"].ToString();
                    forn.Razao_social = rs["emit_xNome"].ToString();
                    forn.Nome_Fantasia = rs["emit_xFant"].ToString();
                    int contFantasia = Funcoes.intTry(Conexao.retornaUmValor("Select count(*) from fornecedor where nome_fantasia like '" + forn.Nome_Fantasia + "%'", null));
                    forn.Nome_Fantasia = forn.Nome_Fantasia+ (contFantasia>0?contFantasia.ToString():"");
                    if (forn.Nome_Fantasia.Length > 19)
                    {
                        forn.Fornecedor = rs["emit_xFant"].ToString().Substring(0,17)+ (contFantasia > 0 ? contFantasia.ToString() : "");
                    }
                    else
                    {
                        forn.Fornecedor = forn.Nome_Fantasia;
                    }
                    forn.CEP = rs["emit_enderEmit_CEP"].ToString();
                    forn.Endereco = rs["emit_enderEmit_xLgr"].ToString();
                    forn.Endereco_nro = rs["emit_enderEmit_nro"].ToString();
                    forn.Bairro = rs["emit_enderEmit_xBairro"].ToString();
                    forn.Cidade = rs["emit_enderEmit_xMun"].ToString();
                    forn.UF = rs["emit_enderEmit_UF"].ToString();
                    forn.codmun = rs["emit_enderEmit_cMun"].ToString();
                    forn.telefone1 = rs["emit_enderEmit_fone"].ToString();
                }
                return forn;
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

        public void importarNFe(String chave)
        {
            SqlDataReader rsTotal = null;
            SqlDataReader rsPg = null;
            SqlDataReader rsItens = null;
            SqlDataReader rsFornecedor = null;
            try
            {
                if (!Conexao.retornaUmValor("Select * from NFe_XMLCanc where chNFe = '" + chave + "'", usr).Equals(""))
                {
                    throw new Exception("Esta nota foi cancelada junto a receita, favor verificar com o Fornecedor");
                }
                int titem = Conexao.countSql("Exec sp_NFe NFE" + chave, usr);
                if (titem > 0 && titem < 4)
                {
                    throw new Exception("NFe foi importada com problemas, favor entrar em contato com o suporte.");
                }
                if (titem <= 0)
                {
                    throw new Exception("NFe não Existe na Base de Dados. Favor Checar arquivo XML.");
                }
                int tcnpj = Conexao.countSql("Exec sp_NFe_Dest 'NFE" + chave + "'", usr);
                if (tcnpj <= 0)
                {
                    throw new Exception("O CNPJ desta NOTA não coincide com o do Estabelecimento");
                }
                SqlDataReader rscnpj = null;
                try
                {


                    rscnpj = Conexao.consulta("Exec sp_NFe_Dest 'NFE" + chave + "'", usr, false);
                    if (rscnpj.Read())
                    {
                        if (!rscnpj["FILIAL"].ToString().Equals(usr.getFilial()))
                        {
                            throw new Exception("O CNPJ desta NOTA não coincide com o da Filial");
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    if (rscnpj != null)
                    {
                        rscnpj.Close();
                    }
                }

                //Pegar CRT da entrada

                int tEmitente = 0;
                rsFornecedor = Conexao.consulta("Exec sp_NFe_ide_emit 'NFE" + chave + "'", usr, true);
                if (rsFornecedor.HasRows)
                {
                    while (rsFornecedor.Read())
                    {
                        tEmitente++;
                        fornecedorCRT = Funcoes.intTry(rsFornecedor["emit_CRT"].ToString());
                        if ( fornecedorCRT > 0)
                        {
                            break;
                        }
                    }
                }

                // int tEmitente = Conexao.countSql("Exec sp_NFe_ide_emit 'NFE" + chave + "'", usr);
                if (tEmitente <= 0)
                {
                    throw new Exception("NFe Não encontrado");
                }

                int tTotal = Conexao.countSql("Exec sp_NFe_Total 'NFE" + chave + "'", usr);
                if (tTotal <= 0)
                {
                    throw new Exception("NFe foi importada com problemas, favor entrar em contato com o suporte");
                }

                int existe = Conexao.countSql("select id from nf where id ='" + chave + "'", usr);
                if (existe > 0)
                {
                    throw new Exception("NFe Não pode ser Importada porque essa nota já existe no sistema!");
                }


                rsTotal = Conexao.consulta("Exec sp_NFe_Total 'NFE" + chave + "'", usr, true);

                rsTotal.Read();
                String Cnpj_Cpf = Conexao.retornaUmValor("select  emit_CNPJ = isnull(emit_CNPJ,emit_CPF) FROM NFe_XML where id LIKE'%NFe" + chave + "' and((emit_CNPJ is not null and emit_CNPJ <>'' ) or emit_CPF is Not null) group by emit_CNPJ,emit_CPF", usr);
                int tFornecedor = Conexao.countSql("SELECT cnpj FROM Fornecedor WHERE replace(replace(replace(CNPJ,'.',''),'/',''),'-','')='" + Cnpj_Cpf + "'", usr);

                if (tFornecedor <= 0)
                {
                    throw new Exception("Fornecedor não Cadastrado");
                }

                DestFornecedor = true;

                Codigo = rsTotal["ide_nNF"].ToString();
                Emissao = (rsTotal["ide_dEmi"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rsTotal["ide_dEmi"].ToString()));
                Tipo_NF = "2";
                serie = Funcoes.intTry(chave.Substring(22, 3));
                usuario = usr.getNome();
                Data = DateTime.Now.Date;
                rsFornecedor = Conexao.consulta("SELECT * FROM Fornecedor WHERE replace(replace(replace(CNPJ,'.',''),'/',''),'-','')='" + Cnpj_Cpf + "'", usr, true);
                if (rsFornecedor.Read())
                {
                    Fornecedor_CNPJ = rsFornecedor["CNPJ"].ToString();
                    Cliente_Fornecedor = rsFornecedor["Fornecedor"].ToString();
                }

                NfItens.Clear();
                String strSqlItens = "Exec sp_NFe_det 'NFE" + chave + "'";
                int tItens = Conexao.countSql(strSqlItens, usr);
                if (tItens <= 0)
                {
                    throw new Exception("NFe foi importada com problemas, favor entrar em contato com o suporte.");
                }
                rsItens = Conexao.consulta(strSqlItens, usr, true);

                while (rsItens.Read())
                {
                    nf_itemDAO item = new nf_itemDAO(usr);
                    item.Codigo = Codigo;
                    item.Cliente_Fornecedor = Cliente_Fornecedor;
                    item.Tipo_NF = Tipo_NF;

                    String strPlu = "";
                    String strSqlplu = "";
                    if (rsItens["det_prod_cEAN"] != null && rsItens["det_prod_cEAN"].ToString().Trim().Length > 0
                        || rsItens["det_prod_cEANTrib"] != null && rsItens["det_prod_cEANTrib"].ToString().Trim().Length > 0
                        )
                    {
                        //String teste = rsItens["det_prod_cEAN"].ToString();
                        long cEan = 0;
                        long.TryParse(rsItens["det_prod_cEANTrib"].ToString(), out cEan);
                        if (cEan == 0)
                            long.TryParse(rsItens["det_prod_cEAN"].ToString(), out cEan);


                        strSqlplu = "SELECT TOP 1 plu FROM EAN WHERE EAN.EAN ='" + cEan + "'";
                        strPlu = Conexao.retornaUmValor(strSqlplu, usr);
                        if (cEan.ToString().Trim().Length > 13)
                        {
                            long.TryParse(rsItens["det_prod_cEANTrib"].ToString(), out cEan);
                            strSqlplu = "SELECT TOP 1 plu FROM EAN WHERE EAN.EAN ='" + cEan + "'";
                            strPlu = Conexao.retornaUmValor(strSqlplu, usr);

                        }


                        item.ean = rsItens["det_prod_cEANTrib"].ToString();
                    }
                    //Verifica se o PLU Está cadastrado
                    if (Funcoes.existePLU(strPlu))
                    {
                        item.PLU = strPlu;

                    }
                    else
                    {
                        strPlu = "";
                    }

                    if (strPlu.Equals("") || item.Embalagem <= 0)
                    {

                        strSqlplu = "SELECT TOP 1 plu,embalagem FROM Fornecedor_Mercadoria WHERE RTRIM(LTRIM(Codigo_Referencia)) = '" + rsItens["det_prod_cProd"].ToString().Trim() + "'" +
                                    " AND RTRIM(LTRIM(Fornecedor)) = '" + Cliente_Fornecedor + "'";
                        SqlDataReader rsMercadoriaLoja = null;
                        try
                        {


                            rsMercadoriaLoja = Conexao.consulta(strSqlplu, usr, false);
                            if (rsMercadoriaLoja.Read())
                            {
                                if (strPlu.Equals(""))
                                    strPlu = rsMercadoriaLoja["plu"].ToString();

                                item.Embalagem = (rsMercadoriaLoja["embalagem"].ToString().Equals("") ? 0 : Decimal.Parse(rsMercadoriaLoja["embalagem"].ToString()));
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        finally
                        {
                            if (rsMercadoriaLoja != null)
                                rsMercadoriaLoja.Close();

                        }

                    }

                    //Verifica se o PLU Está cadastrado
                    if (Funcoes.intTry(
                        Conexao.retornaUmValor("Select count(*) from mercadoria where plu ='" + strPlu + "'", null)
                        ) > 0)
                    {
                        item.PLU = strPlu;

                    }
                    else
                    {
                        strPlu = "";
                    }
                    item.DescricaoNotaFiscalEntrada = rsItens["det_prod_xProd"].ToString().Replace("&", "").Replace("#", "");

                    if (!strPlu.Equals(""))
                    {

                        String srtSqlMercadoria = "SELECT TOP 1 * FROM MERCADORIA WHERE PLU ='" + strPlu + "'";
                        SqlDataReader rsMercadoria = null;
                        try
                        {
                            rsMercadoria = Conexao.consulta(srtSqlMercadoria, usr, true);
                            if (rsMercadoria.Read())
                            {
                                if (item.Embalagem <= 0)
                                {
                                    item.Embalagem = (rsMercadoria["embalagem"].ToString().Equals("") ? 0 : Decimal.Parse(rsMercadoria["embalagem"].ToString()));

                                    if (item.Embalagem <= 0)
                                    {
                                        item.Embalagem = 1;
                                    }
                                }
                                item.inativo = rsMercadoria["inativo"].ToString().Equals("1");
                                item.Descricao = rsMercadoria["Descricao"].ToString();
                            }
                            //Checa a embalagem de acordo com o recebido via APP (SMARTPHONE) tabelas NF_RECEBIMENTO, NF_RECEBIMENTO_ITENS
                            if (entradaDoca)
                            {
                                //nfRecebimentoDoca = new Nf_RecebimentoDAO(usr, chave);
                                if (nfRecebimentoDoca.itens.Count > 0)
                                {
                                    var itemDoca = nfRecebimentoDoca.itens.Find(x => x.plu == item.PLU);
                                    item.Embalagem = 1;
                                    item.Qtde = itemDoca.qtdeNota;
                                    item.Qtde_Devolver = itemDoca.qtdeNota - itemDoca.qtdeRecebida;
                                    item.Data_validade = itemDoca.validade;

                                    if (item.Qtde <= 0)
                                    {

                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        finally
                        {
                            if (rsMercadoria != null)
                                rsMercadoria.Close();

                        }

                    }
                    else
                    {
                        item.PLU = "";
                        item.Descricao = rsItens["det_prod_xProd"].ToString().Replace("&", "").Replace("#", "");
                        if (item.Embalagem <= 0)
                        {
                            item.Embalagem = 1;
                        }
                    }


                    String strSqlCST = "";
                    Decimal vRedutor = 0;
                    Decimal vRedutorST = 0;
                    item.vtotal_produto = Funcoes.decTry(rsItens["det_prod_vProd"].ToString());
                    item.indice_St = rsItens["det_icms_CST"].ToString();
                    //Alterado para popular o FRETE
                    Decimal.TryParse(rsItens["det_prod_vFrete"].ToString(), out item.Frete);
                    
                    switch (rsItens["det_icms_CST"].ToString())
                    {
                        case "60":
                        case "40":
                        case "41":
                        case "50":
                        case "51":
                        case "90":
                            strSqlCST = "SELECT TOP 1 Codigo_Tributacao FROM Tributacao WHERE Indice_ST = '" + rsItens["det_icms_CST"].ToString() + "'";
                            break;
                        case "10":
                        case "00":
                            strSqlCST = "SELECT TOP 1 Codigo_Tributacao FROM Tributacao WHERE Indice_ST = '" + rsItens["det_icms_CST"].ToString() + "'" +
                                        " AND Entrada_ICMS =" + Decimal.Parse(rsItens["det_icms_pICMS"].ToString()).ToString("N2").Replace(",", ".");
                            break;
                        case "20":
                        case "70":
                            vRedutor = Funcoes.decTry(rsItens["det_icms_pRedBC"].ToString());
                            if (vRedutor == 0)
                            {
                                Decimal detIcmsVbc = (rsItens["det_icms_vBC"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_vBC"].ToString()));
                                Decimal detProdvProd = (rsItens["det_prod_vProd"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vProd"].ToString()));
                                Decimal detProdvDesc = (rsItens["det_prod_vDesc"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vDesc"].ToString()));
                                Decimal detProdvFrete = (rsItens["det_prod_vFrete"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vFrete"].ToString()));
                                vRedutor = (100 - (detIcmsVbc / ((detProdvProd - detProdvDesc) + detProdvFrete) * 100));
                                //Caso o valor da redução retorne 0 o sistema pega o campo det_icms_pRedBC



                                //Checa qual é o redutor de base
                                if (vRedutor > 30 && vRedutor < 39)
                                {
                                    vRedutor = 33.33m;
                                }
                                else if (vRedutor > 39 && vRedutor < 42)
                                {
                                    vRedutor = 41.67m;
                                }
                                else if (vRedutor > 47 && vRedutor < 48)
                                {
                                    vRedutor = 47.37m;
                                }
                                else if (vRedutor > 59 && vRedutor < 65)
                                {
                                    vRedutor = 61.11m;
                                }
                                else if (vRedutor > 49 && vRedutor < 55)
                                {
                                    vRedutor = 52.00m;
                                }
                            }

                            strSqlCST = "SELECT TOP 1 Codigo_Tributacao FROM Tributacao WHERE Indice_ST ='" + rsItens["det_icms_CST"].ToString() + "'" +
                                          " AND Entrada_ICMS =" + Decimal.Parse(rsItens["det_icms_pICMS"].ToString()).ToString("N2").Replace(",", ".") +
                                          " AND Redutor = " + vRedutor.ToString("N2").Replace(",", ".");

                            Decimal.TryParse(rsItens["det_icms_pRedBCST"].ToString(), out vRedutorST);
                            break;
                        default:
                            //Caso a compra seja de um fornecedor do simples nacional o sistema atribui o CST 90
                            if (!rsItens["det_icms_csosn"].ToString().Equals(""))
                            {
                                item.indice_St = rsItens["det_icms_csosn"].ToString();
                                item.pCredSN = (rsItens["det_icms_pcredsn"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_pcredsn"].ToString()));

                                switch (item.indice_St)
                                {
                                    case "201":
                                    case "202":
                                    case "203":
                                    case "500":
                                        strSqlCST = "SELECT TOP 1 Codigo_Tributacao FROM Tributacao WHERE Indice_ST = '" + rsItens["det_icms_csosn"].ToString() + "'";
                                        break;
                                    default:
                                        strSqlCST = "SELECT TOP 1 Codigo_Tributacao FROM Tributacao WHERE Indice_ST = '" + rsItens["det_icms_csosn"].ToString() + "' AND Entrada_ICMS=" + item.pCredSN.ToString("N2").Replace(",", ".");
                                        break;

                                }


                                item.vCredicmssn = (rsItens["det_icms_vcredicmssn"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_vcredicmssn"].ToString()));
                                this.vCredicmssn += item.vCredicmssn;
                                item.recalcular = false;

                            }
                            else
                            {
                                strSqlCST = "SELECT TOP 1 Codigo_Tributacao FROM Tributacao WHERE Indice_ST = '99'";
                            }
                            break;


                    }
                    String strCST = Conexao.retornaUmValor(strSqlCST, usr);
                    item.Codigo_Tributacao = (strCST.Equals("") ? 0 : Decimal.Parse(strCST));
                    item.redutor_base = vRedutor;
                    item.redutor_base_ST = vRedutorST;

                    tributacaoDAO trib = item.tributacao;
                    String strCFOP = rsItens["det_prod_CFOP"].ToString();
                    String[] bonificacao = Funcoes.valorParametro("CFOPS_BONIFICACAO", usr).Split(',');
                    String iniCfop = "";
                    if (UfCliente.Equals(usr.filial.UF))
                    {
                        iniCfop = "1";
                    }
                    else
                    {
                        iniCfop = "2";
                    }
                    if (bonificacao.Length == 0 || !Array.Exists(bonificacao, (element) => element == strCFOP))
                    {
                        if (NtOperacao.cfop.ToString().Trim().Length > 0)
                        {
                            strCFOP = NtOperacao.cfop;
                        }
                        else
                        {


                            if (trib.cfop_entrada > 0)
                            {
                                strCFOP = iniCfop + trib.cfop_entrada;
                            }
                            else
                            {
                                strCFOP = iniCfop + strCFOP.Substring(1);
                            }
                        }
                        //if (strCFOP.Equals("5910") || strCFOP.Equals("6910"))
                        //{
                        //    strCFOP = iniCfop + "910";
                        //}
                        //else
                        //{
                        //    switch (item.indice_St)
                        //    {

                        //        case "30":
                        //        case "10":
                        //        case "70":
                        //        case "60":
                        //        case "201":
                        //        case "202":
                        //        case "203":
                        //        case "500":
                        //            strCFOP = iniCfop + "403";
                        //            break;
                        //        default:
                        //            strCFOP = iniCfop + "102";
                        //            break;
                        //    }

                        //}
                        //strCFOP = (Decimal.Parse(strCFOP) - 4000).ToString().Substring(0, 4);


                        //switch (strCFOP.Substring(1))
                        //{
                        //    case "401":
                        //    case "402":
                        //    case "403":
                        //    case "404":
                        //    case "405":
                        //    case "406":
                        //        strCFOP = strCFOP.Substring(0, 1) + "403";
                        //        break;
                        //    case "101":
                        //    case "102":
                        //    case "104":
                        //    case "105":
                        //    case "106":
                        //        strCFOP = strCFOP.Substring(0, 1) + "102";
                        //        break;
                        //}
                    }
                    else
                    {
                        strCFOP = iniCfop + strCFOP.Substring(1);
                    }

                    //Checagem para manter o código de acordo com o CFOP da NAtureza de operãção.



                    if (!NtOperacao.utilizaCFOP)
                    {
                        item.codigo_operacao = Decimal.Parse(strCFOP);
                    }
                    else
                    {
                        bool st = true;
                        switch (item.vIndiceSt.Trim())
                        {
                            case "00":
                            case "20":
                            case "40":
                            case "41":
                            case "50":
                            case "51":
                            case "101":
                            case "102":
                            case "103":
                                st = false;
                                break;
                            default:
                                st = true;
                                break;
                        }
                        if (NtOperacao.cfop_st.Trim().Length > 0 && st)
                        {
                            iniCfop += NtOperacao.cfop_st.Substring(1);
                        }
                        else if (NtOperacao.cfop.Trim().Length > 0)
                        {
                            iniCfop += NtOperacao.cfop.Substring(1);
                        }
                        else
                        {
                            iniCfop += NtOperacao.Codigo_operacao.ToString().Substring(1);
                        }

                        item.codigo_operacao = Decimal.Parse(iniCfop);

                    }




                    item.Filial = usr.getFilial();
                    item.CODIGO_REFERENCIA = rsItens["det_prod_cProd"].ToString().Trim();

                    //Como a entrada é sempre executada pela unidade, o sistema faz o cálculo de acordo com a entrada do produto via smartphone.
                    if (entradaDoca)
                    {
                        if (nfRecebimentoDoca.itens.Count > 0)
                        {
                            item.Unitario = (rsItens["det_prod_vProd"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vProd"].ToString()));
                            item.Unitario = (item.Unitario / (item.Qtde * item.Embalagem));
                        }
                        else
                        {

                            item.Qtde = (rsItens["det_prod_qCom"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_qCom"].ToString()));
                            item.Unitario = (rsItens["det_prod_vUnCom"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vUnCom"].ToString()));

                            if (item.Embalagem > 1)
                            {
                                item.Unitario = (item.Unitario / item.Embalagem);
                            }
                        }
                    }
                    else
                    {
                        item.Qtde = (rsItens["det_prod_qCom"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_qCom"].ToString()));
                        item.Unitario = (rsItens["det_prod_vUnCom"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vUnCom"].ToString()));

                        if (item.Embalagem > 1)
                        {
                            item.Unitario = (item.Unitario / item.Embalagem);
                        }
                    }
                    item.despesas = (rsItens["det_prod_vOutro"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vOutro"].ToString()));


                    item.porcIPI = (rsItens["det_ipi_pIPI"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_ipi_pIPI"].ToString()));

                    item.vIpiv = (rsItens["det_ipi_vIPI"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_ipi_vIPI"].ToString()));

                    if ((item.porcIPI > 0) && (item.vIpiv == 0))
                    {
                        item.vIpiv = ((item.vtotal_produto - item.Desconto) * (item.porcIPI) / 100);
                    }
                    if ((item.vIpiv > 0) && (item.porcIPI == 0))
                    {
                        item.porcIPI = ((item.IPIV / (item.vtotal_produto- item.Desconto)) * 100);
                    }


                    
                    item.vBaseICMS = Funcoes.decTry(rsItens["det_icms_vBC"].ToString());
                    item.vAliquota_icms = Funcoes.decTry(rsItens["det_icms_pICMS"].ToString());
                    item.vicms = Funcoes.decTry(rsItens["det_icms_vICMS"].ToString());
                    item.vAliquota_iva = (rsItens["det_icms_pICMSST"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_pICMSST"].ToString()));

                    item.vBaseIva = (rsItens["det_icms_vBCST"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_vBCST"].ToString()));
                    item.vmargemIva = (rsItens["det_icms_pMVAST"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_pMVAST"].ToString()));
                    item.vIva = (rsItens["det_icms_vICMSST"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_vICMSST"].ToString()));

                    item.vOrigem = int.Parse((rsItens["det_icms_orig"].ToString().Equals("") ? "0" : rsItens["det_icms_orig"].ToString()));

                    if (item.vIva > 0 && item.vmargemIva == 0)
                        item.vmargemIva = item.CalculoMargem_iva;





                    item.CEST = rsItens["det_prod_CEST"].ToString();

                    item.Num_item = (rsItens["det_nItem"].ToString().Equals("") ? 0 : int.Parse(rsItens["det_nItem"].ToString()));
                    
                    //Tratamento ESPECIAL ICMS
                    if (!NtOperacao.Incide_ICMS && !NtOperacao.cst_ICMS.Equals(""))
                    {
                        item.indice_St = NtOperacao.cst_ICMS;
                        item.vBaseICMS = 0;
                        item.vAliquota_icms = 0;
                        item.vicms = 0;
                    }


                    //Se tratando-se de simples nacional, o CST será 90
                    if (fornecedorCRT == 1)
                    {
                        item.indice_St = "90";
                    }
                    else
                    {
                        //Se existir uma configuração para CST, esta será colocada independente se há ou não incidência de ICMS
                        if (!NtOperacao.cst_ICMS.Equals(""))
                        {
                            item.indice_St = NtOperacao.cst_ICMS;
                        }
                    }

                    //PIS E COFINS
                    //Caso o regime tributário da filias seja LUCRO PRESUMIDO não há crédito de PIS E COFINS.
                    if (usr.filial.CRT.Equals("2"))
                    {

                        item.CSTPIS = "73";
                        item.PISV = 0;
                        item.PISp = 0;

                        item.CSTCOFINS = "73";
                        item.COFINSV = 0;
                        item.COFINSp = 0;
                    }
                    else
                    {
                        //Alteração para levar em conta as informações contidas na natureza de operação.
                        if (!NtOperacao.incide_PisCofins && !NtOperacao.cst_pis_cofins.Equals(""))
                        {
                            item.CSTPIS = NtOperacao.cst_pis_cofins;
                            item.CSTCOFINS = NtOperacao.cst_pis_cofins;
                            item.vBASEPisCofins = 0;
                            item.PISV = 0;
                            item.COFINSV = 0;
                            item.PISp = 0;
                            item.COFINSp = 0;
                        }
                        else
                        {
                            //Se o regime tributário da filial for LUCRO REAL e o CST de entrada for igual a 1 a minha cst é 50
                            if (usr.filial.CRT.Equals("3") && Funcoes.intTry(rsItens["det_pis_vPIS"].ToString()) > 0)
                            {
                                item.CSTPIS = "50";
                                item.CSTCOFINS = "50";
                                item.vBASEPisCofins = Funcoes.decTry(rsItens["det_pis_vBC"].ToString());
                                item.PISp = 1.65m;
                                item.COFINSp = 7.60m;
                                if (item.vBASEPisCofins > 0)
                                {
                                    item.PISV = item.vBASEPisCofins * 0.0165m;
                                    item.COFINSV = item.vBASEPisCofins * 0.076m;
                                }
                            }
                            else
                            {
                                //Tratamento especial para NFe de ENTrada
                                if (item.Tipo_NF.Equals("2") && fornecedorCRT == 1 && usr.filial.CRT.Equals("3"))
                                {
                                    //Pega o PIS de acordo com a saída da loja
                                    string cstPISSaidaLoja = Conexao.retornaUmValor("SELECT TOP 1 Mercadoria.CST_Saida FROM Mercadoria WHERE Mercadoria.PLU = '" + item.PLU + "'", usr);
                                    if (cstPISSaidaLoja.Equals("01") || cstPISSaidaLoja.Equals(""))
                                    {
                                        item.CSTPIS = "50";
                                        item.CSTCOFINS = "50";
                                        item.vBASEPisCofins = (item.vtotal_produto - item.vDesconto_valor) - item.vicms;
                                        item.PISp = 1.65m;
                                        item.COFINSp = 7.60m;
                                        if (item.vBASEPisCofins > 0)
                                        {
                                            item.PISV = item.vBASEPisCofins * 0.0165m;
                                            item.COFINSV = item.vBASEPisCofins * 0.076m;
                                        }

                                    }
                                    else
                                    {
                                        switch (cstPISSaidaLoja)
                                        {
                                            case "04":
                                            case "4":
                                                item.CSTPIS = "74";
                                                item.CSTCOFINS = "74";
                                                item.vBASEPisCofins = 0;
                                                item.PISp = 0;
                                                item.COFINSp = 0;
                                                break;
                                            case "05":
                                            case "5":
                                                item.CSTPIS = "75";
                                                item.CSTCOFINS = "75";
                                                item.vBASEPisCofins = 0;
                                                item.PISp = 0;
                                                item.COFINSp = 0;
                                                break;
                                            case "06":
                                            case "6":
                                                item.CSTPIS = "73";
                                                item.CSTCOFINS = "73";
                                                item.vBASEPisCofins = 0;
                                                item.PISp = 0;
                                                item.COFINSp = 0;
                                                break;
                                            case "07":
                                            case "7":
                                                item.CSTPIS = "71";
                                                item.CSTCOFINS = "71";
                                                item.vBASEPisCofins = 0;
                                                item.PISp = 0;
                                                item.COFINSp = 0;
                                                break;
                                            case "08":
                                            case "8":
                                                item.CSTPIS = "74";
                                                item.CSTCOFINS = "74";
                                                item.vBASEPisCofins = 0;
                                                item.PISp = 0;
                                                item.COFINSp = 0;
                                                break;
                                            case "09":
                                            case "9":
                                                item.CSTPIS = "72";
                                                item.CSTCOFINS = "72";
                                                item.vBASEPisCofins = 0;
                                                item.PISp = 0;
                                                item.COFINSp = 0;
                                                break;
                                            default:
                                                item.CSTPIS = "50";
                                                item.CSTCOFINS = "50";
                                                item.vBASEPisCofins = (item.vtotal_produto - item.vDesconto_valor) - item.vicms;
                                                item.PISp = 1.65m;
                                                item.COFINSp = 7.60m;
                                                if (item.vBASEPisCofins > 0)
                                                {
                                                    item.PISV = item.vBASEPisCofins * 0.0165m;
                                                    item.COFINSV = item.vBASEPisCofins * 0.076m;
                                                }
                                                break;
                                        }   
                                    }
                                }
                                else
                                { 
                                    if (item.Tipo_NF.Equals("2"))
                                    {
                                        if (rsItens["det_pis_CST"].ToString().Equals(""))
                                        {
                                            item.CSTPIS = "98";
                                            item.CSTCOFINS = "98";
                                        }
                                        else
                                        {
                                            item.CSTPIS = Conexao.retornaUmValor("select CST_entrada from PISCofins_CST_DePara where CST_saida='" + rsItens["det_pis_CST"].ToString().Trim() + "'", null);
                                            item.CSTCOFINS = item.CSTPIS;
                                        }
                                    }
                                    else
                                    {
                                        item.CSTPIS = (rsItens["det_pis_CST"].ToString().Equals("") ? "49" : rsItens["det_pis_CST"].ToString().Trim());
                                        item.CSTCOFINS = (rsItens["det_cofins_CST"].ToString().Equals("") ? "49" : rsItens["det_cofins_CST"].ToString().Trim());
                                    }

                                    item.vBASEPisCofins = Funcoes.decTry(rsItens["det_icms_vBC"].ToString());
                                    item.PISV = (rsItens["det_pis_vPIS"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_pis_vPIS"].ToString()));
                                    Decimal.TryParse(rsItens["det_pis_pPIS"].ToString(), out item.PISp);

                                    item.COFINSV = (rsItens["det_cofins_vCofins"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_cofins_vCofins"].ToString()));
                                    Decimal.TryParse(rsItens["det_cofins_pCofins"].ToString(), out item.COFINSp);
                                }
                            }
                        }

                    }

                    item.NCM = (rsItens["det_prod_NCM"].ToString().Equals("") ? "" : rsItens["det_prod_NCM"].ToString());

                  
                    item.DescontoValor = Funcoes.decTry(rsItens["det_prod_vDesc"].ToString());

                    item.Und = (rsItens["det_prod_uCOM"].ToString().Equals("") ? "" : rsItens["det_prod_uCOM"].ToString());


                    //NFE 4.0
                    try { item.indEscala = rsItens["det_prod_indEscala"].ToString().Equals("S"); } catch (Exception) { }
                    try { item.cnpj_Fabricante = rsItens["det_prod_CNPJFab"].ToString(); } catch (Exception) { }
                    try { item.cBenef = rsItens["det_prod_cBenef"].ToString(); } catch (Exception) { }
                    try { Decimal.TryParse(rsItens["det_ICMS_vBCFCP"].ToString(), out item.vBCFCP); } catch (Exception) { }
                    try { Decimal.TryParse(rsItens["det_ICMS_pFCP"].ToString(), out item.pFCP); } catch (Exception) { }
                    try { Decimal.TryParse(rsItens["det_ICMS_vFCP"].ToString(), out item.vFCP); } catch (Exception) { }
                    try { Decimal.TryParse(rsItens["det_ICMS_vBCFCPST"].ToString(), out item.vBCFCPST); } catch (Exception) { }
                    try { Decimal.TryParse(rsItens["det_ICMS_pFCPST"].ToString(), out item.pFCPST); } catch (Exception) { }
                    try { Decimal.TryParse(rsItens["det_ICMS_vFCPST"].ToString(), out item.vFCPST); } catch (Exception) { }

                    addItem(item,notaEntradaManual);
                }

                recalcularCentroCusto();

                Filial = usr.getFilial();
                id = chave;

                Total = Funcoes.decTry(rsTotal["Total_ICMSTot_vNF"].ToString());

                TotalProdutos = Funcoes.decTry(rsTotal["Total_ICMSTot_vProd"].ToString());
                Desconto = Funcoes.decTry(rsTotal["Total_ICMSTot_vDesc"].ToString());

                Frete = (rsTotal["Total_ICMSTot_vFrete"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vFrete"].ToString()));
                Despesas_financeiras = (rsTotal["Total_ICMSTot_vOutro"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vOutro"].ToString()));
                Seguro = (rsTotal["Total_ICMSTot_vSeg"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vSeg"].ToString()));
                IPI_Nota = (rsTotal["Total_ICMSTot_vIPI"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vIPI"].ToString()));
                Base_Calculo = (rsTotal["Total_ICMSTot_vBC"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vBC"].ToString()));
                ICMS_Nota = (rsTotal["Total_ICMSTot_vICMS"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vICMS"].ToString()));
                Base_Calc_Subst = (rsTotal["Total_ICMSTot_vBCST"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vBCST"].ToString()));
                ICMS_ST = (rsTotal["Total_ICMSTot_vST"].ToString().Equals("") ? 0 : Decimal.Parse(rsTotal["Total_ICMSTot_vST"].ToString()));
                //Alterado para o sistema totalizar de acordo com os itens.
                vBasePis = 0;
                foreach (nf_itemDAO iBCPC in NfItens)
                {
                    vBasePis += iBCPC.vBASEPisCofins;
                }
                vBaseCofins = vBasePis;

                vTotalcofins = Funcoes.decTry(rsTotal["Total_ICMSTot_vCOFINS"].ToString());
                vTotalPis = Funcoes.decTry(rsTotal["Total_ICMSTot_vPIS"].ToString());

                //NFE 4.0
                try { Decimal.TryParse(rsTotal["Total_ICMSTot_vFCP"].ToString(), out vFCP); } catch { }
                try { Decimal.TryParse(rsTotal["Total_ICMSTot_vFCPST"].ToString(), out vFCPST); } catch { }


                NfPagamentos.Clear();
                String sqlContasPG = "Exec sp_NFe_Cobr  'NFE" + chave + "'";
                rsPg = Conexao.consulta(sqlContasPG, usr, true);

                while (rsPg.Read())
                {
                    nf_pagamentoDAO pg = new nf_pagamentoDAO(usr);
                    pg.Vencimento = (rsPg["cobr_dup_dVenc"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rsPg["cobr_dup_dVenc"].ToString()));
                    pg.Filial = Filial;
                    pg.Codigo = Codigo;
                    pg.Cliente_Fornecedor = Cliente_Fornecedor;
                    pg.Tipo_NF = Tipo_NF;
                    pg.Tipo_pagamento = "BOLETO";
                    pg.Valor = (rsPg["cobr_dup_vDup"].ToString().Equals("") ? 0 : Decimal.Parse(rsPg["cobr_dup_vDup"].ToString()));
                    addPagamento(pg);
                }



            }
            catch (Exception err)
            {

                throw err;
            }
            finally
            {
                if (rsPg != null)
                    rsPg.Close();

                if (rsTotal != null)
                    rsTotal.Close();

                if (rsItens != null)
                    rsItens.Close();

                if (rsFornecedor != null)
                    rsFornecedor.Close();
            }
        }

        public void importarCupom(String numeroCupom, String caixa, DateTime dt, string codigoCliente = "")
        {
            SqlDataReader rs = null;
            try
            {


                if (!NtOperacao.NF_devolucao)
                {
                    if (Conexao.countSql("select codigo from nf where observacao like '%Cupom:" + numeroCupom + " Caixa: " + caixa + " Data:" + dt.ToString("dd/MM/yyyy") + " |%'", null) > 0)
                    {
                        throw new Exception("Cupom Já importado");
                    }

                    //verifica se o cupom já foi importado em outra nota fiscal
                    String sqlCupomTest = "select top 1  s.id_chave " +
                                   "	from saida_estoque s with (index=ix_saida_estoque) inner join mercadoria m on s.PLU=m.PLU  " +
                                   "	LEFT OUTER JOIN tributacao d ON  S.nro_ECF = d.Nro_ECF  and d.Saida_ICMS = s.Aliquota_ICMS " +
                                   "    LEFT OUTER JOIN Cliente c ON s.Codigo_Cliente = c.Codigo_Cliente "+
                                   " where s.filial='" + usr.getFilial() + "'" +
                                   " AND s.data_movimento = '" + dt.ToString("yyyy-MM-dd") + "' and s.data_cancelamento is null " +
                                   " AND s.Caixa_Saida = " + caixa + " AND Documento ='" + numeroCupom + "'";
                    if (!codigoCliente.Equals(""))
                    {
                        sqlCupomTest += " and (s.codigo_cliente = '" + codigoCliente + "'" +
                                      " or c.nome_cliente like '%" + codigoCliente + "%'" +
                                      " or replace(replace(replace(c.cnpj,'.',''),'-',''), '/','') like '" + codigoCliente.Replace(".", "").Replace("-", "").Replace("/", "") + "'" +

                            ")";
                    }
                    sqlCupomTest +=" AND d.Indice_ST in ('00','40','41','60','101','102','500') " +
                                   "  GROUP BY S.Filial , documento , c.codigo_cliente, s.plu, (vlr/Qtde) ,m.embalagem ,m.descricao ,s.aliquota_icms, d.codigo_tributacao, m.IPI, cf, m.cst_saida , und, m.Origem,m.margem_iva,isnull(m.CEST,''),s.id_chave" +
                                   " ,m.indEscala,m.cnpj_Fabricante,m.cBenef ";
                    String id_chave = Conexao.retornaUmValor(sqlCupomTest, usr);
                    if (id_chave.Replace(" ", "").Length > 0)
                    {
                        id_chave = id_chave.Replace(" ", "").Replace("CFe", "").Replace("NFe","").Trim();
                        String nSAT = id_chave.Substring(22, 9);
                        String exSat = id_chave.Substring(31, 6);
                        if (Conexao.countSql("select codigo from nf where observacao like '%Extrato N." + exSat + " Data:" + dt.ToString("dd/MM/yyyy") + " Chave:" + id_chave + " |%'", null) > 0)
                        {
                            throw new Exception("Cupom Já importado");
                        }
                    }

                }

                //Query para importar os itens do cupom
                String sqlCupom = "select  s.filial , documento , s.codigo_cliente, s.plu, SUM(Qtde)Qtde, (vlr/Qtde) vlr ,m.embalagem ,m.descricao ,s.aliquota_icms, d.codigo_tributacao, m.IPI, isnull(m.cf,'') as cf, isnull(m.cst_saida,'') as CST , isnull(m.und,'') as und,m.origem,margem_iva =isnull(m.margem_iva,0), isnull(m.CEST,'') as CEST,sum(isnull(s.Desconto,0)) as Desconto, sum(isnull(acrescimo,0)) as Acrescimo " +
                                    " ,s.id_chave " +
                                    " ,m.indEscala,m.cnpj_Fabricante,m.cBenef " +
                                    "	from saida_estoque s  with (index=ix_saida_estoque) inner join mercadoria m on s.PLU=m.PLU  " +
                                    "	LEFT OUTER JOIN tributacao d ON  S.nro_ECF = d.Nro_ECF  and d.Saida_ICMS = s.Aliquota_ICMS " +
                                    "    LEFT OUTER JOIN Cliente c ON s.Codigo_Cliente = c.Codigo_Cliente " +
                                    " where  s.filial='" + usr.getFilial() + "'" +
                                     " and s.data_movimento='" + dt.ToString("yyyy-MM-dd") + "' and s.data_cancelamento is null " +
                                     " AND s.caixa_saida = " + caixa + " AND Documento ='" + numeroCupom + "'";
                    if (!codigoCliente.Equals(""))
                    {
                        sqlCupom += " and (s.codigo_cliente = '" + codigoCliente + "'" +
                                      " or c.nome_cliente like '%" + codigoCliente + "%'" +
                                      " or replace(replace(replace(c.cnpj,'.',''),'-',''), '/','') like '" + codigoCliente.Replace(".", "").Replace("-", "").Replace("/", "") + "'" +

                            ")";
                    }
                sqlCupom += " and d.Indice_ST in ('00','40','41','60','101','102','500')  " +
                                    "  GROUP BY S.Filial , documento , s.codigo_cliente, s.plu, (vlr/Qtde) ,m.embalagem ,m.descricao ,s.aliquota_icms, d.codigo_tributacao, m.IPI, cf, m.cst_saida , und, m.Origem,m.margem_iva,isnull(m.CEST,''),s.id_chave" +
                                    " ,m.indEscala,m.cnpj_Fabricante,m.cBenef ";

                if (nome_transportadora.Equals(""))
                    nome_transportadora = Conexao.retornaUmValor("Select Nome_transportadora from Transportadora where padrao = 1", null);
                if (Conexao.countSql(sqlCupom, usr) > 0)
                {


                    bool simples = usr.filial.Reg_Federal.ToUpper().Equals("SIMPLES NACIONAL");


                    rs = Conexao.consulta(sqlCupom, this.usr, false);



                    bool SemCliente = Cliente_Fornecedor.Equals(""); 

                    Data = DateTime.Now;
                    Emissao = DateTime.Now;
                    bool adRef = false;
                    if (!Codigo_operacao.ToString().Equals("5929")
                     && !Codigo_operacao.ToString().Equals("6929")
                     && !Codigo_operacao.ToString().Equals("1202")
                     && !Codigo_operacao.ToString().Equals("2202"))
                        adRef = true;


                    while (rs.Read())
                    {

                        if (!adRef)
                        {
                            if (rs["id_chave"].ToString().Replace(" ", "").Length > 0)
                            {
                                String idChave = rs["id_chave"].ToString().Replace(" ", "").Replace("CFe", "").Replace("NFe","").Trim();
                                Ref_ECF = false;
                                if (NfReferencias.Count >= 50)
                                {
                                    NfReferencias.Clear();
                                    NfItens.Clear();
                                    pluIndex.Clear();
                                    throw new Exception("Não é permitido importar mais que 50 cupons");
                                }

                                NfReferencias.Add(idChave);
                                String nSAT = idChave.Substring(22, 9);
                                String exSat = idChave.Substring(31, 6);
                                Observacao += " Extrato N." + exSat + " Data:" + dt.ToString("dd/MM/yyyy") + " Chave:" + idChave + " |";

                            }
                            else
                            {

                                Ref_ECF = true;

                                addECF(caixa,
                                       numeroCupom,
                                        dt
                                    );
                                Observacao += "Cupom:" + numeroCupom + " Caixa: " + caixa + " Data:" + dt.ToString("dd/MM/yyyy") + " |";
                            }
                            adRef = true;
                        }

                        nf_itemDAO item = new nf_itemDAO(usr);
                        if (!rs["codigo_cliente"].ToString().Equals("") && !rs["codigo_cliente"].ToString().Equals("0"))
                        {
                            if (SemCliente)
                            {
                                SemCliente = false;
                                ClienteDAO cliente = new ClienteDAO(rs["codigo_cliente"].ToString(), usr);
                                Cliente_Fornecedor = cliente.Codigo_Cliente;
                                Fornecedor_CNPJ = cliente.CNPJ;
                                indIEDest = cliente.indIEDest;
                                if (cliente.indIEDest == 9)
                                {
                                    indFinal = 1;
                                }
                            }

                        }
                        item.Tipo_NF = "1";
                        item.PLU = rs["plu"].ToString();
                        item.Qtde = (rs["Qtde"].ToString().Equals("") ? 0 : Decimal.Parse(rs["Qtde"].ToString()));
                        item.Embalagem = 1;

                        //nfe 4.0
                        item.indEscala = rs["indEscala"].ToString().Equals("S");
                        item.cnpj_Fabricante = rs["cnpj_Fabricante"].ToString();
                        item.cBenef = rs["cBenef"].ToString();

                        item.Unitario = (rs["vlr"].ToString().Equals("") ? 0 : Decimal.Parse(rs["vlr"].ToString()));
                        item.TotalProduto(true);
                        item.Descricao = rs["descricao"].ToString();
                        item.vOrigem = int.Parse((rs["Origem"].ToString().Equals("") ? "0" : rs["Origem"].ToString()));
                        //item.Desconto =
                        Decimal vDesconto = 0;
                        Decimal.TryParse(rs["Desconto"].ToString(), out vDesconto);
                        item.DescontoValor = vDesconto;

                        Decimal.TryParse(rs["Acrescimo"].ToString(), out item.despesas);

                        item.Codigo_Tributacao = (rs["codigo_tributacao"].ToString().Equals("") ? 0 : Decimal.Parse(rs["codigo_tributacao"].ToString()));
                        if (!simples)
                        {
                            item.vIndiceSt = item.tributacao.Indice_ST;
                            item.aliquota_icms = (rs["aliquota_icms"].ToString().Equals("") ? 0 : Decimal.Parse(rs["aliquota_icms"].ToString()));
                        }
                        else
                        {

                            tributacaoDAO trib = item.tributacao;
                            item.vIndiceSt = trib.csosn.Trim();
                            switch (trib.csosn.Trim())
                            {
                                case "101": // Tributação do ICMS pelo SIMPLES NACIONAL e CSOSN=101
                                    item.aliquota_icms = 0;
                                    item.redutor_base = 0;
                                    item.vIva = 0;
                                    item.pCredSN = 0;
                                    break;
                                case "102":
                                case "103":
                                case "300":
                                case "400":
                                case "500":
                                    item.aliquota_icms = 0;
                                    item.redutor_base = 0;
                                    item.pCredSN = 0;
                                    item.vCredicmssn = 0;
                                    item.vIva = 0;
                                    break;
                                case "201":
                                case "203":
                                    item.redutor_base = trib.Redutor;
                                    item.vmargemIva = (rs["margem_iva"].ToString().Equals("") ? 0 : Decimal.Parse(rs["margem_iva"].ToString()));
                                    item.vAliquota_iva = trib.Saida_ICMS;
                                    item.pCredSN = trib.Saida_ICMS;
                                    break;
                                case "900":
                                    item.vAliquota_iva = trib.Saida_ICMS;
                                    item.redutor_base = trib.Redutor;
                                    item.vAliquota_iva = trib.Saida_ICMS;
                                    item.pCredSN = 0;
                                    break;
                            }
                        }

                        item.IPI = 0;

                        define_cfop(item);

                        item.NCM = rs["cf"].ToString();
                        item.CEST = rs["CEST"].ToString();
                        item.Und = rs["und"].ToString();
                        item.CSTPIS = rs["CST"].ToString();
                        item.CSTCOFINS = rs["CST"].ToString();

                        item.PISp = usr.filial.pis;
                        item.COFINSp = usr.filial.cofins;
                        item.CalculaImpostos();
                        item.TotalRound();
                        
                        //MercadoriaDAO m = new MercadoriaDAO(item.PLU, usr);

                        addItemAgrupar(item,true);
                    }

                    SqlDataReader rsPgPadrao = null;
                    try
                    {


                        rsPgPadrao = Conexao.consulta("Select * from tipo_pagamento where padrao = 1", null, false);
                        if (rsPgPadrao.Read())
                        {
                            if (NfPagamentos.Count > 0)
                                NfPagamentos.Clear();

                            nf_pagamentoDAO pg = new nf_pagamentoDAO(usr);
                            int dias = Funcoes.intTry(rsPgPadrao["prazo"].ToString());
                            pg.Vencimento = DateTime.Now.AddDays(dias);
                            pg.Tipo_pagamento = rsPgPadrao["tipo_pagamento"].ToString();
                            calculaTotalItens();
                            pg.Valor = Total;
                            addPagamento(pg);
                            if (pg.Tipo_pagamento.Contains("BOLETO"))
                                tPag = "15";
                        }

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {
                        if (rsPgPadrao != null)
                            rsPgPadrao.Close();
                    }

                    recalcularCentroCusto();

                }
                else
                {
                    throw new Exception("Cupom não Encontrado");
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

        public void importarCuponsVarios(List<CupomImportar> lCupons)
        {

            foreach (CupomImportar item in lCupons)
            {
                importarCupom(item.numero, item.caixa, item.dt, item.cliente);
            }


        }

        public void importarVariosPedido(List<PedidoImporta> pedidos)
        {
            foreach (PedidoImporta item in pedidos)
            {
                importarPedido(item.numeroPedido, item.tipo);
            }
        }

        public void importarPedido(String numeroPed, int tipo)
        {
            String sqlPedido = "";
            String sqlItensPedido = "";

            if ((tipo == 1) || (tipo == 3))
            {
                sqlPedido = "Select a.filial, b.cnpj, a.cliente_fornec AS cliente_fornecedor, a.desconto, a.total, a.frete, a.despesas " +
                                    " ,b.nome_cliente AS nome_cli_for, a.status, a.cfop as codigo_operacao,c.baixa_estoque,a.usuario " +
                                    ",a.idCadIntTran, a.intermedCnpj, a.indIntermed ,a.CNPJPagamento " +
                                    " from pedido a INNER JOIN cliente b ON(a.cliente_fornec = b.codigo_cliente) " +
                                    " left join natureza_operacao c on a.cfop = c.codigo_operacao " +
                                    " where tipo = " + tipo + " and pedido='" + numeroPed + "' and a.filial ='" + usr.getFilial() + "'";

            }

            else
            {
                sqlPedido = "Select a.filial,b.cnpj, a.cliente_fornec AS cliente_fornecedor,  a.desconto, a.total, a.frete,a.despesas  " +
                                    ", b.nome_fantasia AS nome_cli_for, a.status, a.cfop as codigo_operacao,c.baixa_estoque,a.usuario " +
                                    ",a.idCadIntTran, a.intermedCnpj, a.indIntermed, a.CNPJPagamento" +
                                    " from pedido a INNER JOIN fornecedor b ON(a.cliente_fornec = b.fornecedor) " +
                                    " left join natureza_operacao c on a.cfop = c.codigo_operacao " +
                                    " where tipo = " + tipo + " and pedido='" + numeroPed + "' and a.filial ='" + usr.getFilial() + "'";




            }
            SqlDataReader rsPedido = null;
            SqlDataReader rsItensPedido = null;
            try
            {


                rsPedido = Conexao.consulta(sqlPedido, usr, true);
                if (rsPedido.Read())
                {
                    bool liberaPedidoFechado = Funcoes.valorParametro("LIBERA_PEDIDO_FECHADO", usr).ToUpper().Equals("TRUE");

                    if (!liberaPedidoFechado && !rsPedido["status"].ToString().Equals("1") && !rsPedido["status"].ToString().Equals("4"))
                    {
                        throw new Exception("Pedido Cancelado ou Concluido!");
                    }





                    bool simples = usr.filial.Reg_Federal.ToUpper().Equals("SIMPLES NACIONAL");
                    Pedido = numeroPed;
                    Emissao = DateTime.Now;
                    Data = DateTime.Now;
                    Codigo = "";
                    Cliente_Fornecedor = rsPedido["cliente_fornecedor"].ToString();
                    // Desconto = (rsPedido["Desconto"].ToString().Equals("") ? 0 : Decimal.Parse(rsPedido["Desconto"].ToString()));
                    // Total = (rsPedido["total"].ToString().Equals("") ? 0 : Decimal.Parse(rsPedido["total"].ToString()));
                    usuario = rsPedido["usuario"].ToString();
                    Fornecedor_CNPJ = rsPedido["cnpj"].ToString();

                    indIntermed = Funcoes.intTry(rsPedido["indIntermed"].ToString());
                    if (indIntermed == 1)
                        indPres = 2;

                    intermedCnpj = rsPedido["intermedCnpj"].ToString();
                    idCadIntTran = rsPedido["idCadIntTran"].ToString();
                    CNPJPagamento = rsPedido["CNPJPagamento"].ToString();

                    indFinal = 1; //Consumidor Final
                    indPres = 2; //Operação não presencial, pela Internet

                    Frete = (Decimal)(rsPedido["frete"].ToString().Equals("") ? new Decimal() : rsPedido["frete"]);

                    nome_transportadora = Conexao.retornaUmValor("Select Nome_transportadora from Transportadora where padrao = 1", null);

                    //Codigo_operacao = (Decimal)(rsPedido["codigo_operacao"].ToString().Equals("") ? new Decimal() : rsPedido["codigo_operacao"]);
                    Decimal codigoOperacaoPedido = 0;

                    Decimal.TryParse(rsPedido["codigo_operacao"].ToString(), out codigoOperacaoPedido);
                    Codigo_operacao = codigoOperacaoPedido;

                    Despesas_financeiras = Funcoes.decTry(rsPedido["despesas"].ToString());

                    sqlItensPedido = "SELECT a.plu, a.ean, a.qtde, a.embalagem, a.unitario, " +
                                            "  a.desconto ,a.total, b.descricao, " + (Tipo_NF.Equals("1") ? "d.Saida_icms" : "d.entrada_icms") +
                                            " icms_efetivo, d.codigo_tributacao, b.IPI, isnull(b.cf,'') as cf," +
                                            " isnull(b.cst_" + (Tipo_NF.Equals("1") ? "Saida" : "Entrada") + ",' ') as CST , " +
                                            " isnull(b.und,'') as und, b.margem_iva,b.origem,b.inativo, ISNULL(B.CEST,'') AS CEST, " +
                                            " b.indEscala,b.cnpj_Fabricante,b.cBenef " +
                                            ", pis_perc_" + (Tipo_NF.Equals("1") ? "saida" : "entrada") + " as pis_perc , cofins_perc_" + (Tipo_NF.Equals("1") ? "saida" : "entrada") + " as cofins_perc " +
                                            ",d."+(simples?"csosn": "indice_st") +" as CST_ICMS" +
                                            ", case when isnull(b.cfop, '0') = '' then isnull(convert(varchar, d.cfop), '0') else isnull(b.cfop, '0') end as CFOP_Produto" +
                                        "  FROM pedido_itens a  " +
                                        " 	  INNER JOIN mercadoria b ON(a.plu = b.plu)" +
                                        " 	  INNER JOIN pedido c ON(a.pedido = c.pedido AND a.tipo= c.tipo  AND c.tipo = " + tipo + ") " +
                                        " LEFT OUTER JOIN tributacao d ON(b.filial = d.filial AND b.codigo_tributacao" + (Tipo_NF.Equals("1") ? "" : "_Ent") + " = d.codigo_tributacao) " +
                                        "  WHERE a.pedido ='" + numeroPed + "'";

                    int qtdeItens = Conexao.countSql(sqlItensPedido, usr);
                    Decimal vlrDespesaItem = 0;
                    Decimal vlrDespesaUltItem = 0;
                    if (qtdeItens > 0)
                    {
                        rsItensPedido = null;
                        try
                        {


                            rsItensPedido = Conexao.consulta(sqlItensPedido, usr, false);
                            if (Despesas_financeiras > 0)
                            {
                                vlrDespesaItem = Decimal.Round((Despesas_financeiras / qtdeItens), 2);
                                Decimal dif = Despesas_financeiras - (vlrDespesaItem * qtdeItens);
                                if (dif != 0)
                                    vlrDespesaUltItem = vlrDespesaItem + dif;
                            }

                            while (rsItensPedido.Read())
                            {

                                int vDecimal = 4;
                                try
                                {
                                    vDecimal = int.Parse(Funcoes.valorParametro("CASAS_DECIMAIS_NFE", usr));
                                }
                                catch (Exception)
                                {


                                }

                                nf_itemDAO item = new nf_itemDAO(usr);
                                item.Tipo_NF = Tipo_NF;
                                item.naturezaOperacao = NtOperacao;
                                item.PLU = rsItensPedido["plu"].ToString();
                                item.Qtde = (rsItensPedido["qtde"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensPedido["qtde"].ToString()));
                                item.Embalagem = (rsItensPedido["embalagem"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensPedido["embalagem"].ToString()));
                                Decimal vUnitario = (rsItensPedido["unitario"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensPedido["unitario"].ToString()));
                                Decimal vDesconto = (rsItensPedido["desconto"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensPedido["desconto"].ToString()));

                                //nfe 4.0
                                item.indEscala = rsItensPedido["indEscala"].ToString().Equals("S");
                                item.cnpj_Fabricante = rsItensPedido["cnpj_Fabricante"].ToString();
                                item.cBenef = rsItensPedido["cBenef"].ToString();

                                vUnitario = vUnitario - ((vUnitario * vDesconto) / 100);
                                item.Unitario = Decimal.Round(vUnitario, vDecimal);//(rsItensPedido["unitario"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensPedido["unitario"].ToString()));
                                item.Total = (rsItensPedido["total"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensPedido["total"].ToString()));
                                item.Descricao = rsItensPedido["descricao"].ToString();
                                item.vOrigem = int.Parse((rsItensPedido["Origem"].ToString().Equals("") ? "0" : rsItensPedido["Origem"].ToString()));

                                item.Codigo_Tributacao = (rsItensPedido["codigo_tributacao"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensPedido["codigo_tributacao"].ToString()));
                                item.indice_St = (rsItensPedido["cst_icms"].ToString().Equals("") ? "00" : rsItensPedido["cst_icms"].ToString());
                                item.inativo = rsItensPedido["inativo"].ToString().Equals("1");
                                item.despesas = vlrDespesaItem;

                                if (UfCliente.Equals(usr.filial.UF))
                                {
                                    if (!simples)
                                    {

                                        item.aliquota_icms = (rsItensPedido["icms_efetivo"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensPedido["icms_efetivo"].ToString()));
                                        item.vmargemIva = (rsItensPedido["margem_iva"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensPedido["margem_iva"].ToString()));

                                        tributacaoDAO trib = item.tributacao;
                                        item.redutor_base = trib.Redutor;
                                    }
                                    else
                                    {
                                        tributacaoDAO trib = item.tributacao;
                                        switch (trib.csosn.Trim())
                                        {
                                            case "101": // Tributação do ICMS pelo SIMPLES NACIONAL e CSOSN=101
                                                item.aliquota_icms = 0;
                                                item.redutor_base = 0;
                                                item.vIva = 0;
                                                item.pCredSN = 0;
                                                break;
                                            case "102":
                                            case "103":
                                            case "300":
                                            case "400":
                                            case "500":
                                                item.aliquota_icms = 0;
                                                item.redutor_base = 0;
                                                item.pCredSN = 0;
                                                item.vCredicmssn = 0;
                                                item.vIva = 0;
                                                break;
                                            case "201":
                                            case "203":
                                                item.redutor_base = trib.Redutor;
                                                item.vmargemIva = (rsItensPedido["margem_iva"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensPedido["margem_iva"].ToString()));
                                                item.vAliquota_iva = trib.Saida_ICMS;
                                                item.pCredSN = trib.Saida_ICMS;
                                                break;
                                            case "900":
                                                item.vAliquota_iva = trib.Saida_ICMS;
                                                item.redutor_base = trib.Redutor;
                                                item.vAliquota_iva = trib.Saida_ICMS;
                                                item.pCredSN = 0;
                                                break;
                                        }

                                    }
                                }
                                else
                                {
                                    SqlDataReader rsAliquotaEstado = null;
                                    try
                                    {
                                        //Tratamento para
                                        if (item.origem.Equals("1") || item.origem.Equals("2") || item.origem.Equals("3") || item.origem.Equals("8"))
                                        {
                                            item.aliquota_icms = 4;
                                        }
                                        else
                                        {
                                            switch (UfCliente)
                                            {
                                                case "MG":
                                                case "PR":
                                                case "RS":
                                                case "RJ":
                                                case "SC":
                                                    item.aliquota_icms = 12;
                                                    break;
                                                default:
                                                    item.aliquota_icms = 7;
                                                    break;
                                            }
                                        }
                                        item.vmargemIva = 0;
                                        item.vAliquota_iva = 0;
                                        item.vIndiceSt = "00";

                                        if (!simples)
                                        {
                                            rsAliquotaEstado = Conexao.consulta("select * from aliquota_imp_estado where uf='" + UfCliente + "' and ncm='" + rsItensPedido["cf"].ToString() + "'", null, false);


                                            //Caso não tenha aliquotas configuradas o sistema apresenta erro.
                                            if (rsAliquotaEstado.Read())
                                            {
                                                item.aliquota_ICMS_Destino = Decimal.Parse(rsAliquotaEstado["icms_interestadual"].ToString());
                                                //item.aliquota_icms = Decimal.Parse(rsAliquotaEstado["icms_interestadual"].ToString());
                                                //item.vmargemIva = Decimal.Parse(rsAliquotaEstado["iva_ajustado"].ToString());
                                                //item.vAliquota_iva = Decimal.Parse(rsAliquotaEstado["icms_estado"].ToString());
                                                //item.vIndiceSt = rsAliquotaEstado["CST"].ToString();

                                            }
                                            else
                                            {
                                                throw new Exception("Item: " + item.Num_item.ToString() + "o NCM:" + rsItensPedido["CF"].ToString() + " não configurado.");
                                            }
                                            rsAliquotaEstado.Close();
                                            

                                        }
                                        //Se for simples
                                        if (simples)
                                        {
                                            tributacaoDAO trib = item.tributacao;
                                            switch (trib.csosn.Trim())
                                            {
                                                case "101": // Tributação do ICMS pelo SIMPLES NACIONAL e CSOSN=101
                                                    item.aliquota_icms = 0;
                                                    item.redutor_base = 0;
                                                    item.vIva = 0;
                                                    item.pCredSN = 0;
                                                    break;
                                                case "102":
                                                case "103":
                                                case "300":
                                                case "400":
                                                case "500":
                                                    item.aliquota_icms = 0;
                                                    item.redutor_base = 0;
                                                    item.pCredSN = 0;
                                                    item.vCredicmssn = 0;
                                                    item.vIva = 0;
                                                    break;
                                                case "201":
                                                case "203":
                                                    item.redutor_base = trib.Redutor;
                                                    item.vmargemIva = (rsItensPedido["margem_iva"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensPedido["margem_iva"].ToString()));
                                                    item.vAliquota_iva = trib.Saida_ICMS;
                                                    item.pCredSN = trib.Saida_ICMS;
                                                    break;
                                                case "900":
                                                    item.vAliquota_iva = trib.Saida_ICMS;
                                                    item.redutor_base = trib.Redutor;
                                                    item.vAliquota_iva = trib.Saida_ICMS;
                                                    item.pCredSN = 0;
                                                    break;
                                            }

                                        }

                                    }
                                    catch (Exception)
                                    {

                                        throw;
                                    }
                                    finally
                                    {
                                        if (rsAliquotaEstado != null)
                                        {
                                            rsAliquotaEstado.Close();
                                        }
                                    }
                                }

                                item.IPI = (Tipo_NF.Equals("1") ? 0 : (rsItensPedido["ipi"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensPedido["ipi"].ToString())));
                                item.vIndiceSt = (simples?item.tributacao.csosn:item.tributacao.Indice_ST);
                                define_cfop(item);
                                
                                item.NCM = rsItensPedido["cf"].ToString();
                                item.CEST = rsItensPedido["CEST"].ToString();
                                item.Und = rsItensPedido["und"].ToString();

                                if (NtOperacao.cst_pis_cofins.Trim().Equals("") || Tipo_NF != "2")
                                {
                                    item.CSTPIS = rsItensPedido["CST"].ToString();
                                    item.CSTCOFINS = rsItensPedido["CST"].ToString();
                                }
                                else
                                {
                                    item.CSTPIS = NtOperacao.cst_pis_cofins;
                                    item.CSTCOFINS = NtOperacao.cst_pis_cofins;
                                }

                                Decimal.TryParse(rsItensPedido["pis_perc"].ToString(), out item.PISp);
                                Decimal.TryParse(rsItensPedido["cofins_perc"].ToString(), out item.COFINSp);

                                
                                if (usr.filial.CNPJ.IndexOf("02340366") >= 0)
                                {
                                    MercadoriaDAO m = new MercadoriaDAO(item.PLU, usr);
                                    item.indEscala = m.indEscala;
                                    item.cnpj_Fabricante = m.cnpjFabricante;
                                    item.cBenef = m.cBenef;
                                }
                                else
                                {
                                    item.indEscala = true;
                                    item.cnpj_Fabricante = "";
                                    item.cBenef = "";
                                }

                                item.CalculaImpostos();


                                addItemAgrupar(item);
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        finally
                        {
                            if (rsItensPedido != null)
                                rsItensPedido.Close();

                        }

                        if (vlrDespesaUltItem > 0)
                            ((nf_itemDAO)NfItens[NfItens.Count - 1]).despesas = vlrDespesaUltItem;

                        //Rotina responsável em calcular o total dos itens da NFE
                        calculaTotalItens();

                        String sqlPgPedido = "SELECT VENCIMENTO, tipo_pagamento, valor from pedido_pagamento where pedido = '" + numeroPed + "' and tipo=" + Tipo_NF;

                        if (Conexao.countSql(sqlPgPedido, usr) > 0)
                        {
                            SqlDataReader rsPgPedido = null;
                            try
                            {


                                rsPgPedido = Conexao.consulta(sqlPgPedido, usr, true);

                                while (rsPgPedido.Read())
                                {
                                    nf_pagamentoDAO pg = new nf_pagamentoDAO(usr);
                                    pg.Vencimento = (rsPgPedido["Vencimento"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rsPgPedido["Vencimento"].ToString())); ;
                                    pg.Tipo_pagamento = rsPgPedido["tipo_pagamento"].ToString();
                                    pg.Valor = (rsPgPedido["valor"].ToString().Equals("") ? 0 : Decimal.Parse(rsPgPedido["valor"].ToString()));
                                    addPagamento(pg);
                                }
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            finally
                            {
                                if (rsPgPedido != null)
                                    rsPgPedido.Close();
                            }

                        }
                        else
                        {
                            SqlDataReader rsPgPadrao = null;
                            try
                            {


                                rsPgPadrao = Conexao.consulta("Select * from tipo_pagamento where padrao = 1", null, false);
                                if (rsPgPadrao.Read())
                                {

                                    if (NfPagamentos.Count > 0)
                                        NfPagamentos.Clear();


                                    nf_pagamentoDAO pg = new nf_pagamentoDAO(usr);
                                    int dias = Funcoes.intTry(rsPgPadrao["prazo"].ToString());
                                    pg.Vencimento = DateTime.Now.AddDays(dias);
                                    pg.Tipo_pagamento = rsPgPadrao["tipo_pagamento"].ToString();
                                    calculaTotalItens();
                                    pg.Valor = Total;
                                    addPagamento(pg);
                                    if (pg.Tipo_pagamento.Contains("BOLETO"))
                                        tPag = "15";
                                }

                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            finally
                            {
                                if (rsPgPadrao != null)
                                    rsPgPadrao.Close();
                            }
                        }

                        if (NtOperacao.NF_devolucao)
                        {
                            String sqlDev = "select documento,caixa_documento,data_documento from pedido_itens " +
                                            "where pedido= " + numeroPed + " and tipo =" + tipo +
                                            "group by documento,caixa_documento,data_documento";
                            SqlDataReader rsDev = null;
                            try
                            {


                                rsDev = Conexao.consulta(sqlDev, usr, false);
                                while (rsDev.Read())
                                {
                                    if (!rsDev["documento"].ToString().Equals(""))
                                    {
                                        Ref_ECF = true;
                                        addECF(rsDev["caixa_documento"].ToString(),
                                               rsDev["documento"].ToString(),
                                                (DateTime)rsDev["data_documento"]
                                            );
                                    }

                                }
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            finally
                            {
                                if (rsDev != null)
                                    rsDev.Close();
                            }

                        }


                        PedidosAdd.Add(numeroPed);
                        Pedidos.Add(numeroPed);
                        bool fantasiaObs = Funcoes.valorParametro("NOME_FANTASIA_ENF", usr).ToUpper().Equals("TRUE");
                        if (fantasiaObs)
                        {
                            if (!Observacao.Contains(rsPedido["nome_cli_for"].ToString()))
                            {
                                Observacao += rsPedido["nome_cli_for"].ToString() + "\n";
                            }
                        }
                        recalcularCentroCusto();
                    }
                    else
                    {
                        throw new Exception("Itens do pedido não encontrados");
                    }

                    if (rsPedido != null)
                        rsPedido.Close();


                }
                else
                {
                    throw new Exception("Pedido não existe");
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rsPedido != null)
                    rsPedido.Close();

                if (rsItensPedido != null)
                    rsItensPedido.Close();

                SqlConnection.ClearAllPools();
            }

        }


        public void importarMovimentacao(String codMovimentacao)
        {
            String sqlMovimento = "";


            sqlMovimento = "SELECT	a.PLU, " +
                                   " c.Contada as qtde, " +
                                   " a.embalagem, " +
                                   " ml.preco  , " +
                                   " ml.preco_custo," +
                                   " ml.preco_compra," +
                                   " a.descricao, " +
                                   " a.indEscala,a.cnpj_Fabricante,a.cBenef, ";



            if ((NtOperacao.Saida && NtOperacao.NF_devolucao) || !NtOperacao.Saida)
            {

                sqlMovimento += " d.Entrada_icms as   icms_efetivo, ";
            }
            else
            {
                sqlMovimento += " d.Saida_icms as   icms_efetivo, ";
            }

            sqlMovimento += " d.codigo_tributacao, " +

                                   " a.IPI, " +
                                   " isnull(a.cf,'') as cf, " +
                                   " isnull(a.cst_Saida,'') as CST ," +
                                   " a.pis_perc_saida as pis_perc ," +
                                   " a.cofins_perc_saida as cofins_perc ," +
                                   " isnull(a.und,'') as und, " +
                                   " a.margem_iva, " +
                                   " a.origem, " +
                                   " a.inativo, " +
                                   " ISNULL(a.CEST,'') AS CEST " +
                             " FROM mercadoria a " +

                                   " INNER JOIN Inventario_itens c ON (a.PLU=c.PLU ) " +
                                   " inner join mercadoria_loja ml on(a.plu=ml.plu and ml.filial=c.filial)" +
                                   " LEFT OUTER JOIN tributacao d ON(";
            if ((NtOperacao.Saida && NtOperacao.NF_devolucao) || !NtOperacao.Saida)
            {
                sqlMovimento += " a.codigo_tributacao_ent ";

            }
            else
            {
                sqlMovimento += " a.codigo_tributacao ";
            }


            sqlMovimento += "=d.codigo_tributacao)  " +
                            " WHERE c.Codigo_inventario ='" + codMovimentacao + "'  and c.Filial='" + usr.getFilial() + "'";


            if (Conexao.countSql(sqlMovimento, usr) > 0)
            {
                bool simples = usr.filial.Reg_Federal.ToUpper().Equals("SIMPLES NACIONAL");
                SqlDataReader rsItenMov = null;

                try
                {



                    rsItenMov = Conexao.consulta(sqlMovimento, usr, false);
                    while (rsItenMov.Read())
                    {

                        int vDecimal = 4;
                        try
                        {


                            vDecimal = int.Parse(Funcoes.valorParametro("CASAS_DECIMAIS_NFE", usr));
                        }
                        catch (Exception)
                        {


                        }

                        nf_itemDAO item = new nf_itemDAO(usr);
                        item.naturezaOperacao = this.NtOperacao;

                        item.Tipo_NF = Tipo_NF;
                        item.PLU = rsItenMov["plu"].ToString();
                        item.Qtde = (rsItenMov["qtde"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["qtde"].ToString()));
                        item.Embalagem = 1;//(rsItenMov["embalagem"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["embalagem"].ToString()));
                        Decimal vUnitario = 0;



                        if (NtOperacao.Preco_Venda)
                        {
                            //vUnitario = //(rsItenMov["preco"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["unitario"].ToString()));
                            Decimal.TryParse(rsItenMov["preco"].ToString(), out vUnitario);
                        }
                        else
                        {
                            if (NtOperacao.NF_devolucao)
                            {
                                Decimal.TryParse(rsItenMov["preco_compra"].ToString(), out vUnitario);
                            }
                            else
                            {
                                //vUnitario = (rsItenMov["preco_custo"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["unitario"].ToString()));
                                Decimal.TryParse(rsItenMov["preco_custo"].ToString(), out vUnitario);
                            }
                        }

                        item.indEscala = rsItenMov["indEscala"].ToString().Equals("S");
                        item.cnpj_Fabricante = rsItenMov["cnpj_fabricante"].ToString();
                        item.cBenef = rsItenMov["cBenef"].ToString();


                        item.Unitario = Decimal.Round(vUnitario, vDecimal);//(rsItensPedido["unitario"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensPedido["unitario"].ToString()));

                        if (NtOperacao.Tributacao_padrao.Length > 0)
                        {
                            item.Codigo_Tributacao = Funcoes.decTry(NtOperacao.Tributacao_padrao);
                        }
                        else
                        {
                            item.Codigo_Tributacao = (rsItenMov["codigo_tributacao"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["codigo_tributacao"].ToString()));

                        }

                        item.Descricao = rsItenMov["descricao"].ToString();
                        item.vOrigem = int.Parse((rsItenMov["Origem"].ToString().Equals("") ? "0" : rsItenMov["Origem"].ToString()));


                        item.inativo = rsItenMov["inativo"].ToString().Equals("1");


                        if (UfCliente.Equals(usr.filial.UF))
                        {
                            if (!simples)
                            {

                                item.aliquota_icms = (rsItenMov["icms_efetivo"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["icms_efetivo"].ToString()));
                                item.vmargemIva = (rsItenMov["margem_iva"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["margem_iva"].ToString()));
                            }
                            else
                            {
                                tributacaoDAO trib = item.tributacao;
                                switch (trib.csosn.Trim())
                                {
                                    case "101": // Tributação do ICMS pelo SIMPLES NACIONAL e CSOSN=101
                                        item.aliquota_icms = 0;
                                        item.redutor_base = 0;
                                        item.vIva = 0;
                                        item.pCredSN = 0;
                                        break;
                                    case "102":
                                    case "103":
                                    case "300":
                                    case "400":
                                    case "500":
                                        item.aliquota_icms = 0;
                                        item.redutor_base = 0;
                                        item.pCredSN = 0;
                                        item.vCredicmssn = 0;
                                        item.vIva = 0;
                                        break;
                                    case "201":
                                    case "203":
                                        item.redutor_base = trib.Redutor;
                                        item.vmargemIva = (rsItenMov["margem_iva"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["margem_iva"].ToString()));
                                        item.vAliquota_iva = trib.Saida_ICMS;
                                        item.pCredSN = trib.Saida_ICMS;
                                        break;
                                    case "900":
                                        item.vAliquota_iva = trib.Saida_ICMS;
                                        item.redutor_base = trib.Redutor;
                                        item.vAliquota_iva = trib.Saida_ICMS;
                                        item.pCredSN = 0;
                                        break;
                                }

                            }
                        }
                        else
                        {
                            SqlDataReader rsAliquotaEstado = null;
                            try
                            {


                                rsAliquotaEstado = Conexao.consulta("select * from aliquota_imp_estado where uf='" + UfCliente + "' and ncm='" + rsItenMov["cf"].ToString() + "'", null, false);

                                if (rsAliquotaEstado.Read())
                                {
                                    item.aliquota_icms = Decimal.Parse(rsAliquotaEstado["icms_interestadual"].ToString());
                                    item.vmargemIva = Decimal.Parse(rsAliquotaEstado["iva_ajustado"].ToString());
                                    item.vAliquota_iva = Decimal.Parse(rsAliquotaEstado["icms_estado"].ToString());
                                    item.vIndiceSt = rsAliquotaEstado["CST"].ToString();

                                }
                                else
                                {
                                    if (!simples)
                                    {

                                        item.aliquota_icms = (rsItenMov["icms_efetivo"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["icms_efetivo"].ToString()));
                                        item.vmargemIva = (rsItenMov["margem_iva"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["margem_iva"].ToString()));
                                    }
                                    else
                                    {
                                        tributacaoDAO trib = item.tributacao;
                                        switch (trib.csosn.Trim())
                                        {
                                            case "101": // Tributação do ICMS pelo SIMPLES NACIONAL e CSOSN=101
                                                item.aliquota_icms = 0;
                                                item.redutor_base = 0;
                                                item.vIva = 0;
                                                item.pCredSN = 0;
                                                break;
                                            case "102":
                                            case "103":
                                            case "300":
                                            case "400":
                                            case "500":
                                                item.aliquota_icms = 0;
                                                item.redutor_base = 0;
                                                item.pCredSN = 0;
                                                item.vCredicmssn = 0;
                                                item.vIva = 0;
                                                break;
                                            case "201":
                                            case "203":
                                                item.redutor_base = trib.Redutor;
                                                item.vmargemIva = (rsItenMov["margem_iva"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["margem_iva"].ToString()));
                                                item.vAliquota_iva = trib.Saida_ICMS;
                                                item.pCredSN = trib.Saida_ICMS;
                                                break;
                                            case "900":
                                                item.vAliquota_iva = trib.Saida_ICMS;
                                                item.redutor_base = trib.Redutor;
                                                item.vAliquota_iva = trib.Saida_ICMS;
                                                item.pCredSN = 0;
                                                break;
                                        }

                                    }

                                }
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            finally
                            {
                                if (rsAliquotaEstado != null)
                                    rsAliquotaEstado.Close();
                            }
                        }

                        item.IPI = (Tipo_NF.Equals("1") ? 0 : (rsItenMov["ipi"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["ipi"].ToString())));

                        item.vIndiceSt = item.tributacao.Indice_ST;
                        define_cfop(item);
                        item.NCM = rsItenMov["cf"].ToString();
                        item.CEST = rsItenMov["CEST"].ToString();
                        item.Und = rsItenMov["und"].ToString();

                        if (NtOperacao.cst_pis_cofins.Trim().Equals(""))
                        {
                            item.CSTPIS = rsItenMov["CST"].ToString();
                            item.CSTCOFINS = rsItenMov["CST"].ToString();
                        }
                        else
                        {
                            item.CSTPIS = NtOperacao.cst_pis_cofins;
                            item.CSTCOFINS = NtOperacao.cst_pis_cofins;
                        }
                        Decimal.TryParse(rsItenMov["pis_perc"].ToString(), out item.PISp);
                        Decimal.TryParse(rsItenMov["cofins_perc"].ToString(), out item.COFINSp);

                        item.CalculaImpostos();
                        item.vtotal = item.Total;

                        addItemAgrupar(item);
                    }

                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    if (rsItenMov != null)
                        rsItenMov.Close();
                }


                //calculaTotalItens();

                /*
                String sqlPgPedido = "SELECT VENCIMENTO, tipo_pagamento, valor from pedido_pagamento where pedido = '" + numeroPed + "' and tipo=" + Tipo_NF;

                if (Conexao.countSql(sqlPgPedido, usr) > 0)
                {
                    SqlDataReader rsPgPedido = Conexao.consulta(sqlPgPedido, usr, true);

                    while (rsPgPedido.Read())
                    {
                        nf_pagamentoDAO pg = new nf_pagamentoDAO(usr);
                        pg.Vencimento = (rsPgPedido["Vencimento"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rsPgPedido["Vencimento"].ToString())); ;
                        pg.Tipo_pagamento = rsPgPedido["tipo_pagamento"].ToString();
                        pg.Valor = (rsPgPedido["valor"].ToString().Equals("") ? 0 : Decimal.Parse(rsPgPedido["valor"].ToString()));
                        addPagamento(pg);
                    }

                    if (rsPgPedido != null)
                        rsPgPedido.Close();
                }
                 */



                MovimentacoesAdd.Add(codMovimentacao);
                Movimentacoes.Add(codMovimentacao);
                recalcularCentroCusto();

            }
            else
            {
                throw new Exception("Itens da Movimentacao não encontrados");
            }
        }

        public void importaColetor(Hashtable hPlus, String strPlus)
        {


            String sqlMovimento = "SELECT	 a.PLU, " +
                                           " EAN = ISNULL((SELECT TOP 1 EAN.EAN FROM EAN WHERE EAN.PLU = a.PLU), ''), " +
                                           " a.embalagem,  " +
                                           " ml.preco  ,  " +
                                           " ml.preco_custo," +
                                           " ml.preco_compra," +
                                           " a.descricao, " +
                                           " a.indEscala,a.cnpj_Fabricante,a.cBenef, ";

            if ((NtOperacao.Saida && NtOperacao.NF_devolucao) || !NtOperacao.Saida)
            {

                sqlMovimento += " d.Entrada_icms as   icms_efetivo, ";
            }
            else
            {
                sqlMovimento += " d.Saida_icms as   icms_efetivo, ";
            }
            sqlMovimento += " d.codigo_tributacao,  " +
                                           " a.IPI,  " +
                                           " isnull(a.cf,'') as cf,  " +
                                           " isnull(a.cst_" + (Tipo_NF.Equals("1") ? "saida" : "entrada") + ",'') as CST , " +
                                           " isnull(a.pis_perc_" + (Tipo_NF.Equals("1") ? "saida" : "entrada") + ",0) as pis_perc," +
                                           " isnull(a.cofins_perc_" + (Tipo_NF.Equals("1") ? "saida" : "entrada") + ",0) as cofins_perc," +
                                           " isnull(a.und,'') as und,  " +
                                           " a.margem_iva,  " +
                                           " a.origem,  " +
                                           " a.inativo,  " +
                                           " ISNULL(a.CEST,'') AS CEST  " +
                                    " FROM mercadoria a " +
                                           " inner join Mercadoria_Loja as ml on a.PLU= ml.PLU " +
                                           " LEFT OUTER JOIN tributacao d ON(";
            if (NtOperacao.Preco_Venda)
            {

                sqlMovimento += " a.codigo_tributacao ";
            }
            else
            {
                sqlMovimento += " a.codigo_tributacao_ent ";
            }


            sqlMovimento += "=d.codigo_tributacao)  " +
                                " WHERE ML.Filial='" + usr.getFilial() + "'  AND ( ml.PLU in (" + strPlus + ")" +
                                "or ml.plu in(SELECT  EAN.PLU FROM EAN WHERE EAN.EAN IN(" + strPlus + ")))";


            if (Conexao.countSql(sqlMovimento, usr) > 0)
            {
                bool simples = usr.filial.Reg_Federal.ToUpper().Equals("SIMPLES NACIONAL");
                SqlDataReader rsItenMov = null;

                try
                {
                    rsItenMov = Conexao.consulta(sqlMovimento, usr, false);
                    while (rsItenMov.Read())
                    {

                        int vDecimal = 4;
                        try
                        {


                            vDecimal = int.Parse(Funcoes.valorParametro("CASAS_DECIMAIS_NFE", usr));
                        }
                        catch (Exception)
                        {


                        }

                        nf_itemDAO item = new nf_itemDAO(usr);
                        item.Tipo_NF = Tipo_NF;
                        item.PLU = rsItenMov["plu"].ToString();

                        Decimal vQtde = 1;
                        dynamic objItem = null;
                        if (hPlus.ContainsKey(rsItenMov["plu"].ToString()))
                        {
                            objItem =  hPlus[rsItenMov["plu"].ToString()];
                            vQtde = Funcoes.decTry(objItem.contado);
                        }
                        else
                        {
                            String strEan = rsItenMov["ean"].ToString().Trim();
                            if (!hPlus.ContainsKey(strEan))
                            {
                                SqlDataReader rsEans = null;
                                try
                                {


                                    rsEans = Conexao.consulta("Select ean from ean where plu ='" + item.PLU + "' and ean <>'" + strEan + "'", usr, false);
                                    while (rsEans.Read())
                                    {
                                        if (hPlus.ContainsKey(rsEans["ean"].ToString()))
                                        {
                                            strEan = rsEans["ean"].ToString();
                                            objItem = hPlus[strEan];
                                            vQtde = Funcoes.decTry(objItem.contado);
                                           

                                            break;
                                        }

                                    }
                                }
                                catch (Exception)
                                {

                                    throw;
                                }
                                finally
                                {
                                    if (rsEans != null)
                                        rsEans.Close();
                                }

                            }
                            else
                            {
                                if (strEan.Equals("7898049718252"))
                                {

                                }
                                //objItem = hPlus[rsItenMov[strEan].ToString()];
                                objItem = hPlus[strEan];
                                vQtde = Funcoes.decTry(objItem.contado);
                            }
                        }
                        item.Qtde = vQtde;

                        item.Embalagem = 1;//(rsItenMov["embalagem"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["embalagem"].ToString()));
                        Decimal vUnitario = 0;

                        if(objItem !=null && Funcoes.decTry(objItem.preco) >0)
                        {
                            vUnitario = Funcoes.decTry(objItem.preco);
                        }
                        else if (NtOperacao.Preco_Venda)
                        {
                            //vUnitario = //(rsItenMov["preco"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["unitario"].ToString()));
                            Decimal.TryParse(rsItenMov["preco"].ToString(), out vUnitario);
                        }
                        else
                        {
                            if (NtOperacao.NF_devolucao)
                            {
                                Decimal.TryParse(rsItenMov["preco_compra"].ToString(), out vUnitario);
                            }
                            else
                            {
                                //vUnitario = (rsItenMov["preco_custo"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["unitario"].ToString()));
                                Decimal.TryParse(rsItenMov["preco_custo"].ToString(), out vUnitario);
                            }
                        }

                        item.Unitario = Decimal.Round(vUnitario, vDecimal);//(rsItensPedido["unitario"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensPedido["unitario"].ToString()));
                        item.TotalProduto();
                        
                        item.Descricao = rsItenMov["descricao"].ToString();
                        item.vOrigem = int.Parse((rsItenMov["Origem"].ToString().Equals("") ? "0" : rsItenMov["Origem"].ToString()));
                        if (NtOperacao.Tributacao_padrao.Length > 0)
                        {
                            item.Codigo_Tributacao = Funcoes.decTry(NtOperacao.Tributacao_padrao);
                        }
                        else
                        {
                            item.Codigo_Tributacao = (rsItenMov["codigo_tributacao"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["codigo_tributacao"].ToString()));
                        }
                        item.inativo = rsItenMov["inativo"].ToString().Equals("1");



                        item.indEscala = rsItenMov["indEscala"].ToString().Equals("S");
                        item.cnpj_Fabricante = rsItenMov["cnpj_fabricante"].ToString();
                        item.cBenef = rsItenMov["cBenef"].ToString();

                        tributacaoDAO trib = item.tributacao;
                        item.vIndiceSt = trib.Indice_ST;
                        if (!simples)
                        {

                            //item.aliquota_icms = (rsItenMov["icms_efetivo"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["icms_efetivo"].ToString()));
                            //item.vmargemIva = (rsItenMov["margem_iva"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["margem_iva"].ToString()));
                            switch (trib.Indice_ST.Trim())
                            {
                                case "00":
                                    item.aliquota_icms = (rsItenMov["icms_efetivo"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["icms_efetivo"].ToString()));
                                    item.redutor_base = 0;
                                    item.vmargemIva = 0;
                                    item.vAliquota_iva = 0;
                                    item.vIva = 0;

                                    break;

                                case "30":
                                    item.redutor_base = 0;

                                    break;
                                case "20":
                                    item.aliquota_icms = (rsItenMov["icms_efetivo"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["icms_efetivo"].ToString()));

                                    item.vIva = 0;
                                    item.vmargemIva = 0;
                                    item.vAliquota_iva = 0;

                                    break;

                                case "40":
                                case "41":
                                case "50":
                                case "51":
                                case "60":
                                    item.aliquota_icms = 0;
                                    item.redutor_base = 0;
                                    item.vIva = 0;
                                    item.vmargemIva = 0;
                                    item.vAliquota_iva = 0;

                                    break;
                                case "70":
                                case "10":
                                    item.aliquota_icms = (rsItenMov["icms_efetivo"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["icms_efetivo"].ToString()));

                                    item.redutor_base = trib.Redutor;
                                    item.vmargemIva = (rsItenMov["margem_iva"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["margem_iva"].ToString()));

                                    item.vAliquota_iva = item.aliquota_icms;
                                    item.vIva = item.CalculoIva();
                                    break;
                            }


                        }
                        else
                        {

                            switch (trib.csosn.Trim())
                            {
                                case "101": // Tributação do ICMS pelo SIMPLES NACIONAL e CSOSN=101
                                    item.aliquota_icms = 0;
                                    item.redutor_base = 0;
                                    item.vIva = 0;
                                    item.pCredSN = 0;
                                    break;
                                case "102":
                                case "103":
                                case "300":
                                case "400":
                                case "500":
                                    item.aliquota_icms = 0;
                                    item.redutor_base = 0;
                                    item.pCredSN = 0;
                                    item.vCredicmssn = 0;
                                    item.vIva = 0;
                                    break;
                                case "201":
                                case "203":
                                    item.redutor_base = trib.Redutor;
                                    item.vmargemIva = (rsItenMov["margem_iva"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["margem_iva"].ToString()));
                                    item.vAliquota_iva = trib.Saida_ICMS;
                                    item.pCredSN = trib.Saida_ICMS;
                                    break;
                                case "900":
                                    item.vAliquota_iva = trib.Saida_ICMS;
                                    item.redutor_base = trib.Redutor;
                                    item.vAliquota_iva = trib.Saida_ICMS;
                                    item.pCredSN = 0;
                                    break;
                            }

                        }

                        item.IPI = (Tipo_NF.Equals("1") ? 0 : (rsItenMov["ipi"].ToString().Equals("") ? 0 : Decimal.Parse(rsItenMov["ipi"].ToString())));

                        define_cfop(item);
                        
                        item.NCM = rsItenMov["cf"].ToString();
                        item.CEST = rsItenMov["CEST"].ToString();
                        item.Und = rsItenMov["und"].ToString();
                        if (NtOperacao.cst_pis_cofins.Trim().Equals(""))
                        {
                            item.CSTPIS = rsItenMov["CST"].ToString();
                            item.CSTCOFINS = rsItenMov["CST"].ToString();
                        }
                        else
                        {
                            item.CSTPIS = NtOperacao.cst_pis_cofins;
                            item.CSTCOFINS = NtOperacao.cst_pis_cofins;
                        }
                        Decimal.TryParse(rsItenMov["pis_perc"].ToString(), out item.PISp);
                        Decimal.TryParse(rsItenMov["cofins_perc"].ToString(), out item.COFINSp);


                        item.CalculaImpostos();


                        addItem(item);
                    }
                    recalcularCentroCusto();
                }
                catch (Exception ex)
                {

                    throw;
                }
                finally
                {
                    if (rsItenMov != null)
                        rsItenMov.Close();
                }
            }
            else
            {
                throw new Exception("Itens do Arquivo não encontrados");
            }
        }

        private void define_cfop(nf_itemDAO item)
        {
            String strcfop = "";
            if (NtOperacao.Saida)
            {
                //if (objCliente.UF.Equals(usr.filial.UF)) //Objeto cliente estpa vindo vazio. Tem de levar em consideração qdo for uma NF de devolução que trata também de fornecedor.
                if (this.UfCliente.Equals(usr.filial.UF))
                {
                    strcfop = "5";
                }
                else
                {
                    strcfop = "6";
                }
            }
            else
            {
                if (objCliente.UF.Equals(usr.filial.UF))
                {
                    strcfop = "1";
                }
                else
                {
                    strcfop = "2";
                }

            }
            if (NtOperacao.utilizaCFOP)
            {
                bool st = true;
                switch (item.vIndiceSt.Trim())
                {
                    case "00":
                    case "20":
                    case "40":
                    case "41":
                    case "50":
                    case "51":
                    case "101":
                    case "102":
                    case "103":
                        st = false;
                        break;
                    default:
                        st = true;
                        break;
                }
                if (NtOperacao.cfop_st.Trim().Length > 0 && st)
                {
                    strcfop += NtOperacao.cfop_st.Substring(1);
                }
                else if (NtOperacao.cfop.Trim().Length > 0)
                {
                    strcfop += NtOperacao.cfop.Substring(1);
                }
                else
                {
                    strcfop += NtOperacao.Codigo_operacao.ToString().Substring(1);
                }

            }
            else if (NtOperacao.Saida)
            {
                
                if (NtOperacao.NF_devolucao)
                {
                    switch (item.indice_St.Trim())
                    {

                        case "30":
                        case "10":
                        case "70":
                        case "60":
                        case "201":
                        case "202":
                        case "203":
                        case "500":
                            strcfop += "411";
                            break;
                        default:
                            strcfop += "202";
                            break;
                    }
                }
                else
                {
                    int cfop_produto = Funcoes.intTry(
                            Conexao.retornaUmValor("Select cfop from mercadoria where plu ='" + item.PLU + "'", null)
                         );
                    if (cfop_produto > 0)
                    {
                        strcfop += (cfop_produto < 999 ? cfop_produto.ToString() : cfop_produto.ToString().Substring(1));
                    }
                    else if (item.tributacao.cfop > 0)
                    {
                        strcfop += item.tributacao.cfop.ToString();
                    }
                    else
                    {
                        switch (item.indice_St.Trim())
                        {
                            case "10":
                            case "70":
                            case "60":
                                strcfop += "405";
                                break;
                            default:
                                strcfop += NtOperacao.Codigo_operacao.ToString().Substring(1);
                                break;
                        }
                    }

                }

            }
            else
            {
                if (UfCliente.Equals(usr.filial.UF))
                {
                    strcfop = "1";
                }
                else
                {
                    strcfop = "2";
                }

                strcfop += NtOperacao.Codigo_operacao.ToString().Substring(1);
            }
            item.codigo_operacao = Funcoes.decTry(strcfop);
        }

        public void verificaTribItens()
        {
            bool recal = false;
            foreach (nf_itemDAO it in NfItens)
            {
                if (Tipo_NF.Equals("2") && !NtOperacao.NF_devolucao)
                {
                    if (finNFe != 2) //Se não for nota fiscal complementar
                    {
                        if (it.tributacao.ipi_EmOutrasDespesas && it.vIpiv > 0)
                        {
                            recal = true;
                            it.despesas += it.vIpiv;
                            it.vIpiv = 0;
                            it.porcIPI = 0;
                        }

                        if (it.tributacao.icmsst_emOutrasDespesas && it.vIva > 0)
                        {

                            recal = true;
                            it.despesas += it.vIva;
                            it.vIva = 0;
                            it.vmargemIva = 0;
                            it.vBaseIva = 0;
                        }
                    }
                    else
                    {
                        recal = true;
                        it.despesas += it.vIpiv;
                        it.vIpiv = 0;
                        it.porcIPI = 0;

                    }
                }
            }
            if (recal)
            {
                calculaTotalItens();
            }

        }


        public bool confirmaItens()
        {
            if (Tipo_NF.Equals("3"))
            {
                return true;
            }
            foreach (nf_itemDAO it in NfItens)
            {

                /*
                if (existeItem(it.PLU))
                {
                    throw new Exception("PLU:"+it.PLU+" Duplicado.");
                }
                */

                if (it.PLU.Trim().Equals(""))
                {
                    throw new Exception("O Item " + it.Num_item + " esta sem plu");
                }
                else if (Funcoes.intTry(
                    Conexao.retornaUmValor("Select count(*) from mercadoria where plu = '" + it.PLU + "'", null)
                    ) == 0
                    )
                {
                    throw new Exception("O Plu: " + it.PLU + " não esta cadastrado no sistema!");
                }

                //if (finNFe != 2)
                //{
                //    if (!tributacaoDAO.existe(it.Codigo_Tributacao, this.usr))
                //    {
                //        throw new Exception("O Item " + it.Num_item + " esta com uma Tributacao que não existe "); ;

                //    }
                //}

                if (!natureza_operacaoDAO.existeNaturezaOperacaocfop(it.codigo_operacao.ToString(), this.usr))
                {
                    throw new Exception("O Item " + it.Num_item + " esta com um CFOP que não existe ");

                }
                if (it.NCM.ToString().Equals("") || it.NCM.ToString().Equals("0"))
                {
                    throw new Exception("O Item " + it.Num_item + " esta com um NCM Inválido ");

                }
                if (finNFe != 2)
                {
                    if (it.CSTCOFINS.ToString().Trim().Equals(""))
                    {
                        throw new Exception("O Item " + it.Num_item + " esta com um CSTCOFINS Inválido ");
                    }

                    if (it.CSTPIS.ToString().Trim().Equals(""))
                    {
                        throw new Exception("O Item " + it.Num_item + " esta com um CSTPIS Inválido ");
                    }
                }
                //Validação dos campos de Base de Cálculo e CST
                //if ((it.indice_St.Equals("10")) || (it.indice_St.Equals("70")))
                //{
                //    if (Convert.ToDecimal(it.CalculoBase_iva.ToString().Trim()) <= 0)
                //    {
                //        throw new Exception("O Item " + it.Num_item + " não pode conter valor 0 na Margem IVA quando o CST for igual a 10 ou 70");
                //    }
                //}
                it.Ordem_compra = this.Ordem_compra;
            }
            if (finNFe == 2)
            {
                if (NfReferencias.Count <= 0)
                {
                    throw new Exception("Em uma Nota de Complementar é obrigatorio Referenciar uma nota fiscal ");
                }
            }
            if (NtOperacao.NF_devolucao)
            {
                if (NfReferencias.Count <= 0 && ECFReferencias.Count <= 0)
                {
                    throw new Exception("Em uma Nota de Devolução é obrigatorio Referenciar ao menos uma nota fiscal ou ECF");
                }
            }

            return true;
        }
        public void salvarCorrecao(String protocolo, String correcao)
        {
            Conexao.executarSql("insert into nfe_correcao (codigo,correcao,seq,protocolo,usuario,data,filial)" +
                                "values (" +
                                    "'" + Codigo + "'," +
                                    "'" + correcao.Replace("'", "") + "'," +
                                    "'" + nota_referencia + "'," +
                                    "'" + protocolo + "'," +
                                    "'" + usr.getUsuario() + "'," +
                                    " getDate()," +
                                    "'" + usr.getFilial() + "')");
            Conexao.executarSql("update nf set nota_referencia='" + nota_referencia + "' where codigo='" + Codigo + "' and tipo_nf=" + Tipo_NF + " and filial ='" + usr.getFilial() + "'");

        }



        public bool salvar(bool novo)
        {

            confirmaItens();

            verificaTribItens();

            dataHoraLancamento = DateTime.Now;

            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                if (novo)
                {
                    insert(conn, tran);
                }
                else
                {
                    update(conn, tran);
                }

                //Inclusão de NFe com pedidos.
                foreach (String pd in PedidosAdd)
                {
                    Conexao.executarSql("insert into nota_pedido (codigo_nota,codigo_pedido) values ('" + Codigo + "','" + pd + "')", conn, tran);
                    string tipo = Tipo_NF;

                    if (!Tipo_NF.Equals("3"))
                    {
                        tipo = Tipo_NF;
                    }
                    else
                    {
                        tipo = "1";
                    }
                    if (NtOperacao.NF_devolucao)
                    {
                        if (Ref_ECF)
                        {
                            tipo = "3";
                        }
                        else
                        {
                            tipo = "4";
                        }
                    }

                    bool liberaPedidoFechado = Funcoes.valorParametro("LIBERA_PEDIDO_FECHADO", usr).ToUpper().Equals("TRUE");
                    if (liberaPedidoFechado)
                    {
                        String statusPedido = Conexao.retornaUmValor("Select status from pedido where pedido='" + pd + "' and tipo=" + Tipo_NF + " and filial ='" + Filial + "'", usr, conn, tran);
                        if (statusPedido.Equals("2"))
                        {
                            Conexao.executarSql("update pedido set  " +
                                                    "obs=ISNULL(convert(varchar(max),obs),'')+' O pedido foi importado na nota-" + Codigo + " mesmo estando com o status fechado'+char(13)+char(10)+' pelo usuario=" + usr.getNome() + " no dia: '+CONVERT (VARCHAR ,getDate(),113)+char(13)+char(10) " +
                                                    " where pedido='" + pd + "' and tipo=" + tipo + " and filial ='" + Filial + "'", conn, tran);
                        }
                        else
                        {

                            Conexao.executarSql("update pedido set status=2 where pedido='" + pd + "' and tipo=" + tipo + " and filial ='" + Filial + "'", conn, tran);
                        }
                    }
                    else
                    {
                        Conexao.executarSql("update pedido set status=2 where pedido='" + pd + "' and tipo=" + tipo + " and filial ='" + Filial + "'", conn, tran);
                    }

                }
                PedidosAdd.Clear();

                foreach (String mov in MovimentacoesAdd)
                {
                    Conexao.executarSql("insert into nota_movimentacao (filial,codigo_nota,codigo_movimentacao,emissao_nota) values ('" + usr.getFilial() + "','" + Codigo + "','" + mov + "','" + Emissao.ToString("yyyy-MM-dd") + "' )", conn, tran);

                    Conexao.executarSql("update Inventario set Importado_Nota ='" + Codigo + "' , Imp_Nota_emissao='" + Emissao.ToString("yyyy-MM-dd") + "'" +
                                        " where Codigo_inventario= '" + mov + "'  and filial ='" + usr.getFilial() + "'", conn, tran);

                }

                //Finalidade da NFe não pode ser 2->NFE Complementar.
                if (finNFe != 2)
                {
                    MovimentacoesAdd.Clear();
                    recalcularCentroCusto();
                    calculaPorcCentroCusto();
                    salvarCentroCustos(conn, tran);
                }

                //Gravar dados da NFe na devolução.
                if (OrigemDevolucao && devolucaoNFeCodigo > 0)
                {
                    string sqlDevolucao = "UPDATE Devolucao_NFe SET Status = 1, EmissaoNFe = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', NumeroNFe = " + this.Codigo + ", ChaveNFe = '" + this.id + "' WHERE ";
                    sqlDevolucao += " Devolucao_NFe.Codigo = " + devolucaoNFeCodigo.ToString();
                    Conexao.executarSql(sqlDevolucao, conn, tran);
                }

                tran.Commit();


                //Recálculo é executado após salvar justamente para ter os dados gravados sem preocupação com transaction ou commit
                //Recálculo do saldo em estoque do produto
                //Se tratar de uma edição, for uma NFe de entrada, a natureza de operação movimenta estoque e a data de entrada é inferior a data atual o sistema vai
                //efetuar o recálculo de todos os produtos envolvidos.
                if (Tipo_NF.Equals("2") && NtOperacao.Baixa_estoque && Data < DateTime.Today)
                {
                    var dataProcessamento = Data.AddDays(-1);
                    foreach (var item in NfItens)
                    {
                        Funcoes.recalculoSaldoPLUDia(Filial, item.PLU, dataProcessamento);
                    }
                }

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
            return true;


        }
        public void salvarCentroCustos(SqlConnection conn, SqlTransaction tran)
        {
            String sqlDel = "delete from NF_centroCusto" +
                            " where filial = '" + Filial + "'" +
                               "   and codigo='" + Codigo + "'" +
                               "   and tipo_nf=" + Tipo_NF +
                               "   and serie =" + serie +
                               "   and cliente_fornecedor ='" + Cliente_Fornecedor + "'";
            Conexao.executarSql(sqlDel, conn, tran);

            foreach (Nf_CentroCustoDAO item in LsCentrosCustos)
            {
                item.Salvar(conn, tran);
            }
        }

        public void AtualizarStatus()
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                Conexao.executarSql("update nf set status ='" + status + "',emissao='" + Emissao.ToString("yyyy-MM-dd") + "',Emissao_hora='" + Emissao.ToString("HH:mm:ss") + "',data='" + Data.ToString("yyyy-MM-dd") + "',data_hora='" + Data.ToString("HH:mm:ss") + "',nf_canc=" + (nf_Canc ? "1" : "0") + ", numero_protocolo='" + numeroProtocolo + "' , producao_nfe=" + (producao_nfe ? "1" : "0") + " where codigo='" + Codigo + "' and filial='" + Filial + "' and cliente_fornecedor ='" + Cliente_Fornecedor + "' and tipo_nf=" + Tipo_NF);
                if (nf_Canc)
                {
                    Conexao.executarSql("delete from nf_devolucao where codigo_nf='" + Codigo + "'", conn, tran);
                }

                if (status.Equals("AUTORIZADO"))
                {
                    foreach (nf_itemDAO item in NfItens)
                    {
                        item.naturezaOperacao = NtOperacao;
                        item.AtualizaSaidaEstoque(conn, tran, DateTime.Today);
                    }
                    foreach (nf_pagamentoDAO pg in NfPagamentos)
                    {
                        pg.NaturezaOperacao = NtOperacao;
                        pg.centroCusto = centro_custo.Trim();
                        if (pg.centroCusto.Equals(""))
                        {
                            pg.centroCusto = Funcoes.valorParametro("CENTRO_CUSTO_NF_SAIDA", usr).Trim();
                        }
                        pg.atualizaFinanceiro(conn, tran);
                    }

                    //Exclusivo DIFAL
                    //Inclusão conta a pagar.
                    if (vValorDifal > 0)
                    {
                        var filialIEDestino = usr.filial.IEs.Find(x => x.UF == this.UfCliente);

                        if (Tipo_NF.Equals("1") && finNFe.ToString().Equals("1") && indFinal.ToString().Equals("1"))
                        {
                            conta_a_pagarDAO pg = new conta_a_pagarDAO(usr);
                            pg.Documento = "DIFAL-" + this.UfCliente + "-" + Codigo + "-01";
                            pg.Fornecedor = "GNRE";
                            pg.Filial = Filial;
                            pg.Codigo_Centro_Custo = "004014017";
                            pg.Valor = vValorDifal + vFCP;
                            pg.Desconto = 0;
                            pg.serie = this.serie;
                            pg.obs = "DIFAL REF. NF SAIDA" + ":" + Codigo;
                            pg.emissao = DateTime.Today;
                            pg.Vencimento = DateTime.Today;
                            if (filialIEDestino != null)
                            {
                                if (filialIEDestino.VctoQtde > 0)
                                {
                                    pg.Vencimento = Funcoes.vencimentoDIFAL(filialIEDestino.VctoTipo, filialIEDestino.VctoQtde, filialIEDestino.DiasUteis, filialIEDestino.VctoTipoQtde);
                                }
                                else
                                {
                                    pg.Vencimento = DateTime.Today;
                                }
                            }
                            pg.Tipo_Pagamento = "AVISTA";
                            pg.id_cc = "";
                            pg.Pagamento = DateTime.Today;
                            pg.Valor_Pago = vValorDifal;
                            pg.status = "1";
                            pg.entrada = DateTime.Now;
                            pg.usuario = usr.getNome();
                            pg.documento_emitido = Codigo;
                            // ALTERACÃO SOLICITADA PELO RAFAEL NO DIA 08/12/2015 
                            pg.Duplicata = false;
                            pg.BOLETO_RECEBIDO = false;
                            pg.cod_barras = "";
                            //====================================================
                            pg.salvar(true, conn, tran);
                        }
                    }

                    if (OrigemDevolucao && devolucaoNFeCodigo > 0)
                    {
                        string sqlDevolucao = "UPDATE Devolucao_NFe SET Status = 2, EmissaoNFe = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', NumeroNFe = " + this.Codigo + ", ChaveNFe = '" + this.id + "' WHERE ";
                        sqlDevolucao += " Devolucao_NFe.Codigo = " + devolucaoNFeCodigo.ToString();
                        Conexao.executarSql(sqlDevolucao, conn, tran);
                    }

                }
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

        public bool excluir()
        {
            if (!Tipo_NF.Equals("3"))
            {
                if (status != null && (status.Equals("TRANSMITIDO") || status.Equals("AUTORIZADO") || status.Equals("CANCELADA")))
                {
                    throw new Exception("Não é Possivel Cancelar a Nota, Execute o Cancelamento pela Rotina de XML");
                }
            }


            nf_Canc = true;
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {

                if (Tipo_NF.Equals("1") || NtOperacao.NF_devolucao)
                {
                    String proximoNumero = Funcoes.sequencia("NF.CODIGO", usr);

                    long dif = long.Parse(proximoNumero) - long.Parse(Codigo);
                    if (dif > 1)
                        throw new Exception("Não é Possivel Cancelar a Nota por existir uma nota emitida posterior Utilizar Cancelamento pela Rotina de XML");
                    else
                        Funcoes.voltaUltimaSequencia("NF.CODIGO", usr);



                    /*
                   // Conexao.executarSql("update nf set nf_canc=1 , status='CANCELADA' where codigo=" + Codigo + " and cliente_fornecedor='" + Cliente_Fornecedor + "' and tipo_nf= " + Tipo_NF + "and filial='" + Filial + "'", conn, tran);
                    //Conexao.executarSql("update nf_item set nf_canc=1  where codigo=" + Codigo + " and cliente_fornecedor='" + Cliente_Fornecedor + "' and tipo_nf= " + Tipo_NF + "and filial='" + Filial + "'", conn, tran);

                    foreach (nf_itemDAO item in NfItens)
                    {
                        item.naturezaOperacao = NtOperacao;
                        item.cancela(conn, tran);
                    }
                     */
                }

                foreach (nf_itemDAO item in NfItens)
                {
                    item.naturezaOperacao = NtOperacao;
                    item.excluir(conn, tran, Data);
                }

                foreach (nf_pagamentoDAO pg in NfPagamentos)
                {


                    pg.NaturezaOperacao = NtOperacao;
                    pg.excluir(conn, tran);


                }

                if (Movimentacoes.Count > 0)
                {
                    String strSqlMov = " update Inventario set Importado_Nota =null , Imp_Nota_emissao=null" +
                                        " where importado_nota = " + Codigo + "  and Imp_Nota_emissao ='" + Emissao.ToString("yyyy-MM-dd") + "' and filial='" + usr.getFilial() + "'";
                    Conexao.executarSql(strSqlMov, conn, tran);

                }


                //Gravar dados da NFe na devolução.
                if (OrigemDevolucao && devolucaoNFeCodigo > 0)
                {
                    string sqlDevolucao = "UPDATE Devolucao_NFe SET Status = 0, EmissaoNFe = NULL, NumeroNFe = NULL, ChaveNFe = '' WHERE ";
                    sqlDevolucao += " Devolucao_NFe.Codigo = " + devolucaoNFeCodigo.ToString();
                    Conexao.executarSql(sqlDevolucao, conn, tran);
                }


                Conexao.executarSql("delete from nf where codigo='" + Codigo + "' and cliente_fornecedor='" + Cliente_Fornecedor + "' and tipo_nf= " + Tipo_NF + "and filial='" + Filial + "'", conn, tran);



                //Excluir Centro de custo 

                String sqlDel = "delete from NF_centroCusto" +
                                " where filial = '" + Filial + "'" +
                                   "   and codigo='" + Codigo + "'" +
                                   "   and tipo_nf=" + Tipo_NF +
                                   "   and serie =" + serie +
                                   "   and cliente_fornecedor ='" + Cliente_Fornecedor + "'";
                Conexao.executarSql(sqlDel, conn, tran);


                //Processo para exclusão dos logs.
                try
                {
                    Conexao.executarSql("delete from NF_Log where codigo='" + Codigo + "' and cliente_fornecedor='" + Cliente_Fornecedor + "' and tipo_nf= " + Tipo_NF + "and filial='" + Filial + "'", conn, tran);
                    Conexao.executarSql("delete from NF_Item_Log where codigo='" + Codigo + "' and cliente_fornecedor='" + Cliente_Fornecedor + "' and tipo_nf= " + Tipo_NF + "and filial='" + Filial + "'", conn, tran);
                }
                catch
                {

                }


                String tipo = Tipo_NF;
                if (NtOperacao.NF_devolucao)
                {
                    if (Ref_ECF)
                    {
                        tipo = "3";
                    }
                    else
                    {
                        tipo = "4";
                    }
                }

                foreach (String pd in Pedidos)
                {
                    Conexao.executarSql("update pedido set status=1 where pedido='" + pd + "' and tipo=" + tipo + " and filial ='" + Filial + "' and isnull(pedido_simples,0) <>1 ", conn, tran);
                }
                PedidosAdd.Clear();

                Conexao.executarSql("delete from nota_pedido where codigo_nota='" + Codigo + "'", conn, tran);

                Conexao.executarSql("delete from nf_devolucao where ltrim(rtrim(codigo_nf))='" + Codigo.Trim() + "'", conn, tran);

                //Conexao.executarSql("update nf_item set nf_canc=1  where codigo=" + Codigo + " and cliente_fornecedor='" + Cliente_Fornecedor + "' and tipo_nf= " + Tipo_NF + "and filial='" + Filial + "'", conn, tran);


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
            return true;
        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                if (Tipo_NF.Equals("1") || (!NtOperacao.Saida && NtOperacao.Imprime_NF))
                {//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                    Codigo = Funcoes.sequencia("NF.CODIGO", usr);
                }
                if (Tipo_NF.Equals("3"))
                {
                    Codigo = Funcoes.sequencia("NFPEDIDO.CODIGO", usr);
                }
                if (usuario.Length > 20)
                {
                    usuario = usuario.Substring(0, 20);
                }

                String sql = " insert into nf (" +
                            "Codigo," +
                            "Cliente_Fornecedor," +
                            "Tipo_NF," +
                            "Data," +
                            "Codigo_operacao," +
                            "Codigo_operacao1," +
                            "Emissao," +
                            "Filial," +
                            "Total," +
                            "Desconto," +
                            "Frete," +
                            "Seguro," +
                            "IPI_Nota," +
                            "Outras," +
                            "ICMS_Nota," +
                            "Estado," +
                            "Base_Calculo," +
                            "Despesas_financeiras," +
                            "Pedido," +
                            "Base_Calc_Subst," +
                            "Observacao," +
                            "nf_Canc," +
                            "nome_transportadora," +
                            "qtde," +
                            "especie," +
                            "marca," +
                            "numero," +
                            "peso_bruto," +
                            "peso_liquido," +
                            "tipo_frete," +
                            "funcionario," +
                            "centro_custo," +
                            "encargo_financeiro," +
                            "ICMS_ST," +
                            "Pedido_cliente," +
                            "Fornecedor_CNPJ," +
                            "Placa," +
                            "Endereco_Entrega," +
                            "Desconto_geral," +
                            "nome_fantasia," +
                            "boleto_recebido," +
                            "usuario," +
                            "nfe," +
                            "XML," +
                            "id," +
                            "status," +
                            "indPres," +
                            "indFinal, " +
                            "nota_referencia," +
                            "Dest_Fornec," +
                            "Ref_ECF" +
                            ",pisv" +
                            ",cofinsv" +
                            ",base_pis" +
                            ",base_cofins" +
                            ",total_produto" +
                            ",CodigoNotaProdutor" +
                            ",finNFe" +
                            ",vFCP" +
                            ",vFCPST" +
                            ",tPag" +
                            ",serie" +
                            ",usuario_alteracao"+
                            ",vCredicmssn"+
                            ",ordem_compra"+
                            ",indIntermed " +
                            ",intermedCnpj " +
                            ",idCadIntTran" +
                            ",CNPJPagamento" +
                            ",Validacao_Fiscal" +
                            ", Valor_Difal" +
                            ", DataHora_Lancamento" +

                     " )values (" +
                            "'" + Codigo + "'" +
                            "," + "'" + Cliente_Fornecedor + "'" +
                            "," + Tipo_NF +
                            "," + (Data.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data.ToString("yyyy-MM-dd") + "'") +
                            "," + Funcoes.decimalPonto(Codigo_operacao.ToString()) +
                            "," + Funcoes.decimalPonto(Codigo_operacao1.ToString()) +
                            "," + (Emissao.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Emissao.ToString("yyyy-MM-dd") + "'") +
                            "," + "'" + Filial + "'" +
                            "," + Funcoes.decimalPonto(Total.ToString()) +
                            "," + Funcoes.decimalPonto(Desconto.ToString()) +
                            "," + Funcoes.decimalPonto(Frete.ToString()) +
                            "," + Funcoes.decimalPonto(Seguro.ToString()) +
                            "," + Funcoes.decimalPonto(IPI_Nota.ToString()) +
                            "," + Funcoes.decimalPonto(Outras.ToString()) +
                            "," + Funcoes.decimalPonto(ICMS_Nota.ToString()) +
                            "," + (Estado ? 1 : 0) +
                            "," + Funcoes.decimalPonto(Base_Calculo.ToString()) +
                            "," + Funcoes.decimalPonto(Despesas_financeiras.ToString()) +
                            "," + "'" + Pedido + "'" +
                            "," + Funcoes.decimalPonto(Base_Calc_Subst.ToString()) +
                            "," + "'" + Observacao + "'" +
                            "," + (nf_Canc ? 1 : 0) +
                            "," + "'" + nome_transportadora + "'" +
                            "," + Funcoes.decimalPonto(qtde.ToString()) +
                            "," + "'" + especie + "'" +
                            "," + "'" + marca + "'" +
                            "," + Funcoes.decimalPonto(numero.ToString()) +
                            "," + Funcoes.decimalPonto(peso_bruto.ToString()) +
                            "," + Funcoes.decimalPonto(peso_liquido.ToString()) +
                            "," + tipo_frete +
                            "," + "'" + funcionario + "'" +
                            "," + "'" + centro_custo.Trim() + "'" +
                            "," + Funcoes.decimalPonto(encargo_financeiro.ToString()) +
                            "," + Funcoes.decimalPonto(ICMS_ST.ToString()) +
                            "," + "'" + Pedido_cliente + "'" +
                            "," + "'" + Fornecedor_CNPJ + "'" +
                            "," + "'" + Placa + "'" +
                            "," + "'" + Endereco_Entrega + "'" +
                            "," + Funcoes.decimalPonto(Desconto_geral.ToString()) +
                            "," + "'" + nome_fantasia + "'" +
                            "," + (boleto_recebido ? 1 : 0) +
                            "," + "'" + usuario + "'" +
                            "," + (nfe ? 1 : 0) +
                            "," + (XML ? 1 : 0) +
                            "," + "'" + id + "'" +
                            ",'" + status + "'" +
                            "," + indPres +
                            "," + indFinal +
                            ",'" + nota_referencia + "'" +
                            "," + (DestFornecedor ? 1 : 0) +
                            "," + (Ref_ECF ? 1 : 0) +
                            "," + Funcoes.decimalPonto(vTotalPis.ToString()) +
                            "," + Funcoes.decimalPonto(vTotalcofins.ToString()) +
                            "," + Funcoes.decimalPonto(vBasePis.ToString()) +
                            "," + Funcoes.decimalPonto(vBaseCofins.ToString()) +
                            "," + Funcoes.decimalPonto(TotalProdutos.ToString()) +
                            ",'" + vCodigoNotaProdutor + "'" +
                            "," + finNFe +
                            "," + Funcoes.decimalPonto(vFCP.ToString()) +
                            "," + Funcoes.decimalPonto(vFCPST.ToString()) +
                            ",'" + tPag + "'" +
                            "," + serie +
                            ",'"+ usuario_Alteracao+"'"+
                            ","+Funcoes.decimalPonto(vCredicmssn.ToString())+
                            ",'"+Ordem_compra +"'"+
                            "," + indIntermed.ToString() +
                            ",'" + intermedCnpj.Trim() + "'" +
                            ",'" + idCadIntTran.Trim() + "'" +
                            ",'" + CNPJPagamento + "'" +
                            ",'" + validacaoFiscal + "'" +
                            ", " + Funcoes.decimalPonto(vValorDifal.ToString())+
                            ", '" + dataHoraLancamento.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                 ");";

                Conexao.executarSql(sql, conn, tran);

                ordenaritens();
                foreach (nf_itemDAO item in NfItens)
                {
                    item.Filial = Filial;
                    item.Codigo = Codigo;
                    item.Cliente_Fornecedor = Cliente_Fornecedor;
                    item.Tipo_NF = Tipo_NF;
                    item.naturezaOperacao = NtOperacao;
                    item.salvar(true, conn, tran);
                    item.atualizarItem(conn, tran, this.Data);

                }
                itensAdd = new ArrayList();
                itensExcluidos = new ArrayList();
                int ordem = 1;
                foreach (nf_pagamentoDAO pg in NfPagamentos)
                {
                    pg.Filial = Filial;
                    pg.Cliente_Fornecedor = Cliente_Fornecedor;
                    pg.Codigo = Codigo;
                    pg.Tipo_NF = Tipo_NF;
                    pg.emissao = Emissao;
                    pg.centroCusto = centro_custo;
                    pg.NaturezaOperacao = NtOperacao;
                    pg.ordem = ordem;
                    pg.boleto_recebido = this.boleto_recebido;
                    pg.vCodigoNotaProdutor = this.vCodigoNotaProdutor;
                    pg.salvar(true, conn, tran);
                    ordem++;
                }
                pagamentosAdd = new ArrayList();
                pagamentosExcluidos = new ArrayList();


                //if (!Pedido.Trim().Equals(""))
                //{
                //    Conexao.executarSql("update pedido set status=2 where pedido='" + Pedido + "' and tipo=" + Tipo_NF + " and filial ='" + Filial + "'",conn,tran);
                //}
                if (Tipo_NF.Equals("1") || (!NtOperacao.Saida && NtOperacao.Imprime_NF))
                {
                    Funcoes.salvaProximaSequencia("NF.CODIGO", usr);
                }
                if (Tipo_NF.Equals("3"))
                {
                    Funcoes.salvaProximaSequencia("NFPEDIDO.CODIGO", usr);
                }

                if (NfReferencias.Count > 0)
                {
                    foreach (String item in NfReferencias)
                    {
                        Conexao.executarSql("insert into nf_devolucao(codigo_nf,id, serie) values ('" + Codigo + "','" + item + "', '" + serie + "')", conn, tran);

                    }
                }

                if (ECFReferencias.Count > 0)
                {
                    foreach (ArrayList item in ECFReferencias)
                    {
                        Conexao.executarSql("insert into nf_devolucao(codigo_nf,nECF,nCOO,data_documento) values ('" + Codigo + "','" + item[0] + "','" + item[1] + "'," + (item[2].Equals("") ? "null" : "'" + DateTime.Parse(item[2].ToString()).ToString("yyyy-MM-dd") + "')"), conn, tran);

                    }
                }


            }
            catch (Exception err)
            {
                throw new Exception("Nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }

        public void importarDevolucaoNFe(String numeroDevolucao)
        {
            String sqlDevolucao = "";
            String sqlItensDevolucao = "";

            sqlDevolucao = "Select a.filial, b.cnpj, a.codigo_cliente AS cliente, a.total "+
                                    " ,b.nome_cliente AS nome, a.status, a.usuario " +
                                    " from Devolucao_NFe a INNER JOIN cliente b ON(a.Codigo_cliente = b.codigo_cliente) " +
                                    " where a.filial ='" + usr.getFilial() + "' AND a.Codigo = " + numeroDevolucao;

            SqlDataReader rsDevolucao = null;
            SqlDataReader rsItensDevolucao = null;
            try
            {


                rsDevolucao = Conexao.consulta(sqlDevolucao, usr, true);
                if (rsDevolucao.Read())
                {

                    if (!rsDevolucao["status"].ToString().Equals("0") )
                    {
                        throw new Exception("STATUS da DEVOLUÇÃO não permite emitir NFe!");
                    }

                    bool simples = usr.filial.Reg_Federal.ToUpper().Equals("SIMPLES NACIONAL");
                    Pedido = numeroDevolucao;
                    Emissao = DateTime.Now;
                    Data = DateTime.Now;
                    Codigo = "";
                    Cliente_Fornecedor = rsDevolucao["cliente"].ToString();


                    // Desconto = (rsPedido["Desconto"].ToString().Equals("") ? 0 : Decimal.Parse(rsPedido["Desconto"].ToString()));
                    // Total = (rsPedido["total"].ToString().Equals("") ? 0 : Decimal.Parse(rsPedido["total"].ToString()));
                    usuario = rsDevolucao["usuario"].ToString();
                    Fornecedor_CNPJ = rsDevolucao["cnpj"].ToString();

                    indIntermed = 0;// Operação sem intermediador
                    indPres = 1; //Operação presencial
                    finNFe = 4; //Finalidade (Devolução)


                    intermedCnpj = "";
                    idCadIntTran = "";
                    CNPJPagamento = "";


                    Frete = 0;

                    nome_transportadora = Conexao.retornaUmValor("Select Nome_transportadora from Transportadora where padrao = 1", null);

                    Codigo_operacao = decimal.Parse(Funcoes.valorParametro("NATUREZA_DEV_CLI_PDV", null));

                    Despesas_financeiras = 0;

                    sqlItensDevolucao = "SELECT a.plu, a.qtde,  a.unitario " +
                                            "   ,a.TotalItem as Total, b.descricao, d.entrada_icms" +
                                            ", d.icms_efetivo, d.codigo_tributacao, isnull(b.cf,'') as cf," +
                                            " isnull(b.cst_Entrada,' ') as CST , " +
                                            " isnull(b.und,'') as und, 0 as margem_iva, b.origem, ISNULL(B.CEST,'') AS CEST, " +
                                            " b.indEscala, b.cnpj_Fabricante, b.Inativo, b.cBenef " +
                                            ", pis_perc_entrada as pis_perc , cofins_perc_entrada as cofins_perc " +
                                            ",d." + (simples ? "csosn" : "indice_st") + " as CST_ICMS" +
                                            ", case when isnull(b.cfop, '0') = '' then isnull(convert(varchar, d.cfop), '0') else isnull(b.cfop, '0') end as CFOP_Produto" +
                                        "  FROM Devolucao_NFe_Item a  " +
                                        " 	  INNER JOIN mercadoria b ON(a.plu = b.plu)" +
                                        " LEFT OUTER JOIN tributacao d ON(b.filial = d.filial AND b.codigo_tributacao = d.codigo_tributacao) " +
                                        "  WHERE a.filial ='" + usr.getFilial() + "' AND a.Codigo = " + numeroDevolucao;

                    int qtdeItens = Conexao.countSql(sqlItensDevolucao, usr);
                    Decimal vlrDespesaItem = 0;
                    Decimal vlrDespesaUltItem = 0;
                    if (qtdeItens > 0)
                    {
                        rsItensDevolucao = null;
                        try
                        {


                            rsItensDevolucao = Conexao.consulta(sqlItensDevolucao, usr, false);
                            if (Despesas_financeiras > 0)
                            {
                                vlrDespesaItem = Decimal.Round((Despesas_financeiras / qtdeItens), 2);
                                Decimal dif = Despesas_financeiras - (vlrDespesaItem * qtdeItens);
                                if (dif != 0)
                                    vlrDespesaUltItem = vlrDespesaItem + dif;
                            }

                            while (rsItensDevolucao.Read())
                            {

                                int vDecimal = 4;
                                try
                                {
                                    vDecimal = int.Parse(Funcoes.valorParametro("CASAS_DECIMAIS_NFE", usr));
                                }
                                catch (Exception)
                                {


                                }

                                nf_itemDAO item = new nf_itemDAO(usr);
                                item.Tipo_NF = Tipo_NF;
                                item.naturezaOperacao = NtOperacao;
                                item.PLU = rsItensDevolucao["plu"].ToString();
                                item.Qtde = (rsItensDevolucao["qtde"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensDevolucao["qtde"].ToString()));
                                item.Embalagem = 1;
                                Decimal vUnitario = (rsItensDevolucao["unitario"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensDevolucao["unitario"].ToString()));
                                Decimal vDesconto = 0;

                                //nfe 4.0
                                item.indEscala = true;
                                item.cnpj_Fabricante = "";
                                item.cBenef = "";

                                vUnitario = vUnitario - ((vUnitario * vDesconto) / 100);
                                item.Unitario = Decimal.Round(vUnitario, vDecimal);//(rsItensPedido["unitario"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensPedido["unitario"].ToString()));
                                item.Total = (rsItensDevolucao["total"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensDevolucao["total"].ToString()));
                                item.Descricao = rsItensDevolucao["descricao"].ToString();
                                item.vOrigem = int.Parse((rsItensDevolucao["Origem"].ToString().Equals("") ? "0" : rsItensDevolucao["Origem"].ToString()));

                                item.Codigo_Tributacao = (rsItensDevolucao["codigo_tributacao"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensDevolucao["codigo_tributacao"].ToString()));
                                item.indice_St = (rsItensDevolucao["cst_icms"].ToString().Equals("") ? "00" : rsItensDevolucao["cst_icms"].ToString());
                                item.inativo = rsItensDevolucao["inativo"].ToString().Equals("1");
                                item.despesas = vlrDespesaItem;

                                if (UfCliente.Equals(usr.filial.UF))
                                {
                                    if (!simples)
                                    {

                                        item.aliquota_icms = (rsItensDevolucao["icms_efetivo"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensDevolucao["icms_efetivo"].ToString()));
                                        item.vmargemIva = (rsItensDevolucao["margem_iva"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensDevolucao["margem_iva"].ToString()));

                                        tributacaoDAO trib = item.tributacao;
                                        item.redutor_base = trib.Redutor;
                                    }
                                    else
                                    {
                                        tributacaoDAO trib = item.tributacao;
                                        switch (trib.csosn.Trim())
                                        {
                                            case "101": // Tributação do ICMS pelo SIMPLES NACIONAL e CSOSN=101
                                                item.aliquota_icms = 0;
                                                item.redutor_base = 0;
                                                item.vIva = 0;
                                                item.pCredSN = 0;
                                                break;
                                            case "102":
                                            case "103":
                                            case "300":
                                            case "400":
                                            case "500":
                                                item.aliquota_icms = 0;
                                                item.redutor_base = 0;
                                                item.pCredSN = 0;
                                                item.vCredicmssn = 0;
                                                item.vIva = 0;
                                                break;
                                            case "201":
                                            case "203":
                                                item.redutor_base = trib.Redutor;
                                                item.vmargemIva = (rsItensDevolucao["margem_iva"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensDevolucao["margem_iva"].ToString()));
                                                item.vAliquota_iva = trib.Saida_ICMS;
                                                item.pCredSN = trib.Saida_ICMS;
                                                break;
                                            case "900":
                                                item.vAliquota_iva = trib.Saida_ICMS;
                                                item.redutor_base = trib.Redutor;
                                                item.vAliquota_iva = trib.Saida_ICMS;
                                                item.pCredSN = 0;
                                                break;
                                        }

                                    }
                                }
                                else
                                {
                                    SqlDataReader rsAliquotaEstado = null;
                                    try
                                    {


                                        rsAliquotaEstado = Conexao.consulta("select * from aliquota_imp_estado where uf='" + UfCliente + "' and ncm='" + rsItensDevolucao["cf"].ToString() + "'", null, false);

                                        if (rsAliquotaEstado.Read())
                                        {
                                            item.aliquota_icms = Decimal.Parse(rsAliquotaEstado["icms_interestadual"].ToString());
                                            item.vmargemIva = Decimal.Parse(rsAliquotaEstado["iva_ajustado"].ToString());
                                            item.vAliquota_iva = Decimal.Parse(rsAliquotaEstado["icms_estado"].ToString());
                                            item.vIndiceSt = rsAliquotaEstado["CST"].ToString();

                                        }
                                        else
                                        {
                                            if (!simples)
                                            {

                                                item.aliquota_icms = (rsItensDevolucao["icms_efetivo"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensDevolucao["icms_efetivo"].ToString()));
                                                item.vmargemIva = (rsItensDevolucao["margem_iva"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensDevolucao["margem_iva"].ToString()));
                                            }
                                            else
                                            {
                                                tributacaoDAO trib = item.tributacao;
                                                switch (trib.csosn.Trim())
                                                {
                                                    case "101": // Tributação do ICMS pelo SIMPLES NACIONAL e CSOSN=101
                                                        item.aliquota_icms = 0;
                                                        item.redutor_base = 0;
                                                        item.vIva = 0;
                                                        item.pCredSN = 0;
                                                        break;
                                                    case "102":
                                                    case "103":
                                                    case "300":
                                                    case "400":
                                                    case "500":
                                                        item.aliquota_icms = 0;
                                                        item.redutor_base = 0;
                                                        item.pCredSN = 0;
                                                        item.vCredicmssn = 0;
                                                        item.vIva = 0;
                                                        break;
                                                    case "201":
                                                    case "203":
                                                        item.redutor_base = trib.Redutor;
                                                        item.vmargemIva = (rsItensDevolucao["margem_iva"].ToString().Equals("") ? 0 : Decimal.Parse(rsItensDevolucao["margem_iva"].ToString()));
                                                        item.vAliquota_iva = trib.Saida_ICMS;
                                                        item.pCredSN = trib.Saida_ICMS;
                                                        break;
                                                    case "900":
                                                        item.vAliquota_iva = trib.Saida_ICMS;
                                                        item.redutor_base = trib.Redutor;
                                                        item.vAliquota_iva = trib.Saida_ICMS;
                                                        item.pCredSN = 0;
                                                        break;
                                                }

                                            }

                                        }
                                    }
                                    catch (Exception)
                                    {

                                        throw;
                                    }
                                    finally
                                    {
                                        if (rsAliquotaEstado != null)
                                            rsAliquotaEstado.Close();
                                    }
                                }

                                item.IPI = 0;
                                item.vIndiceSt = (simples ? item.tributacao.csosn : item.tributacao.Indice_ST);
                                define_cfop(item);

                                item.NCM = rsItensDevolucao["cf"].ToString();
                                item.CEST = rsItensDevolucao["CEST"].ToString();
                                item.Und = rsItensDevolucao["und"].ToString();

                                if (NtOperacao.cst_pis_cofins.Trim().Equals("") || Tipo_NF != "2")
                                {
                                    item.CSTPIS = rsItensDevolucao["CST"].ToString();
                                    item.CSTCOFINS = rsItensDevolucao["CST"].ToString();
                                }
                                else
                                {
                                    item.CSTPIS = NtOperacao.cst_pis_cofins;
                                    item.CSTCOFINS = NtOperacao.cst_pis_cofins;
                                }

                                Decimal.TryParse(rsItensDevolucao["pis_perc"].ToString(), out item.PISp);
                                Decimal.TryParse(rsItensDevolucao["cofins_perc"].ToString(), out item.COFINSp);


                                MercadoriaDAO m = new MercadoriaDAO(item.PLU, usr);
                                item.indEscala = m.indEscala;
                                item.cnpj_Fabricante = m.cnpjFabricante;
                                item.cBenef = m.cBenef;

                                item.CalculaImpostos();


                                addItemAgrupar(item);
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        finally
                        {
                            if (rsItensDevolucao != null)
                                rsItensDevolucao.Close();

                        }



                        if (vlrDespesaUltItem > 0)
                            ((nf_itemDAO)NfItens[NfItens.Count - 1]).despesas = vlrDespesaUltItem;

                        //Rotina responsável em calcular o total dos itens da NFE
                        calculaTotalItens();

                        if (NtOperacao.NF_devolucao)
                        {
                            String sqlDev = "select distinct chavecfe from devolucao_nfe_item where filial ='" + usr.getFilial() + "' AND Codigo = " + numeroDevolucao;
                            SqlDataReader rsDev = null;
                            try
                            {


                                rsDev = Conexao.consulta(sqlDev, usr, false);
                                while (rsDev.Read())
                                {
                                    if (!rsDev["chavecfe"].ToString().Equals(""))
                                    {
                                        NfReferencias.Add(rsDev["chavecfe"].ToString().ToUpper().Replace("CFE","").Replace("NFe",""));
                                    }

                                }
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            finally
                            {
                                if (rsDev != null)
                                    rsDev.Close();
                            }

                        }

                        DevolucaoNFEADD.Add(numeroDevolucao);
                        DevolucaoNFe.Add(numeroDevolucao);
                        recalcularCentroCusto();
                    }
                    else
                    {
                        throw new Exception("Itens da DEVOLUÇÃO não encontrados");
                    }

                    if (rsDevolucao != null)
                        rsDevolucao.Close();


                }
                else
                {
                    throw new Exception("Devolução não existe");
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rsDevolucao != null)
                    rsDevolucao.Close();

                if (rsItensDevolucao != null)
                    rsItensDevolucao.Close();
            }

        }
        
        public void carregaCTe()
        {
            //String sql = " SELECT * FROM NF_CTE nfct" +
            //               " WHERE nfct.filial ='" + Filial + "'" +
            //               "   and nfct.Chave_NFe ='" + id + "'" ;
            //SqlDataReader rs = null;
            //try
            //{
            //    rs = Conexao.consulta(sql, null, false);
            //    while (rs.Read())
            //    {
            //        NFCTe.Filial = rs["Filial"].ToString();
            //        NFCTe.Chave_NFe = rs["Chave_NFe"].ToString();
            //        NFCTe.Situacao = rs["Situacao"].ToString();
            //        NFCTe.Fornecedor = rs["Fornecedor"].ToString();
            //        NFCTe.Serie = Int32.Parse(rs["Serie"].ToString());
            //        NFCTe.Emissao = Convert.ToDateTime(rs["Emissao"].ToString());
            //        NFCTe.Aquisicao = Convert.ToDateTime(rs["Aquisicao"].ToString());
            //        NFCTe.Chave = rs["Chave"].ToString();
            //        NFCTe.Tipo_CTe = Funcoes.intTry(rs["Tipo_CTe"].ToString());
            //        NFCTe.Chave_Substituicao = rs["Chave_Substituicao"].ToString();
            //        NFCTe.Tipo_Frete = Funcoes.intTry(rs["Tipo_Frete"].ToString());
            //        NFCTe.ICMS_Base = Funcoes.decTry(rs["ICMS_Base"].ToString());
            //        NFCTe.ICMS_Reducao = Funcoes.decTry(rs["ICMS_Reducao"].ToString());
            //        NFCTe.ICMS_Aliquota = Funcoes.decTry(rs["ICMS_Aliquota"].ToString());
            //        NFCTe.ICMS_Valor = Funcoes.decTry(rs["ICMS_Valor"].ToString());
            //        NFCTe.IBGE_Origem = Funcoes.intTry(rs["IBGE_Origem"].ToString());
            //        NFCTe.IBGE_Destino = Funcoes.intTry(rs["IBGE_Desticno"].ToString());
            //    }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            //finally
            //{
            //    if (rs != null)
            //        rs.Close();
            //}
        }

        //** Função para retornar se vai ou não haver um bloqueio de edição/exclusão da NFe de entrada ou saída que movimenta estoque.
        public bool bloqueioInventario(User usr, string PLU, DateTime dataMovimentacao)
        {
            try
            {
                string sql = "SELECT REG = count(*) FROM inventario i ";
                sql += " INNER JOIN  inventario_itens ii ON i.codigo_inventario = ii.codigo_inventario";
                sql += " INNER JOIN Tipo_movimentacao tm ON tm.Movimentacao = i.tipoMovimentacao";
                sql += " WHERE i.DataHora_Encerramento >= '" + dataMovimentacao.ToString("yyyy-MM-dd") + "'";
                sql += " AND tm.Saida = 2 and i.status = 'ENCERRADO'";
                sql += " AND ii.plu = '" + PLU +"'";
                return Funcoes.intTry(Conexao.retornaUmValor(sql, usr)) > 0 ? true : false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        //** Função para retornar se vai ou não haver um bloqueio de edição/exclusão da NFe de entrada ou saída que movimenta estoque.
        public bool bloqueioControleEstoqueDia(User usr, string PLU, DateTime dataMovimentacao)
        {
            try
            {
                string sql = "SELECT REG = count(*) FROM Mercadoria_Estoque_Dia ";
                sql += " WHERE Mercadoria_Estoque_Dia.Filial = '" + usr.getFilial() + "'";
                sql += " AND Mercadoria_Estoque_Dia.Data <= '" +  dataMovimentacao.ToString("yyyy-MM-dd") + "'";
                sql += " AND Mercadoria_Estoque_Dia.plu = '" + PLU + "'";
                return Funcoes.intTry(Conexao.retornaUmValor(sql, usr)) > 0 ? false : true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool atualizaIDNFe()
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                string sql = "UPDATE NF SET NF.ID ='" + this.id.ToString().Replace("NFe","") + "' WHERE NF.FILIAL='" + this.Filial + "' AND NF.Cliente_Fornecedor = '" + this.Cliente_Fornecedor + "' AND ";
                sql += " NF.codigo ='" + this.Codigo + "' AND NF.Tipo_NF = " + this.Tipo_NF.ToString() + " AND NF.Serie = " + this.serie.ToString();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                return true;
            }
            catch (Exception err)
            {
                tran.Rollback();
                throw err;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                    SqlConnection.ClearPool(conn);
                }
            }
        }
    }
}