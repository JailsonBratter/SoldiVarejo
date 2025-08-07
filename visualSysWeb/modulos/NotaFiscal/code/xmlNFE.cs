using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using visualSysWeb.dao;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml.XPath;
using visualSysWeb.code;
using System.Data.SqlClient;
using System.Collections;
using visualSysWeb.modulos.NotaFiscal.dao;
using Ionic.Zip;
using XML_NFe;

namespace visualSysWeb.modulos.NotaFiscal.code
{
    public class xmlNFE
    {

        public String strUltNSU = "";
        public String strMaxNSU = "";
        String cErro = "";
        int Retorno = 0;
        String RetornoString = "";
        String versao = "4.00";
        int tpAmb = 2;
        String CasasDecimais = "N4";
        public DateTime dtAutorizacao = new DateTime();
        User usr = new User();
        /*
        * Variaveis abaixo para uso da NFE
        * Grupo de Tributação do ICMS para a UF de destino
        */
        decimal vTotFCPUFDest = 0;
        decimal vTotICMSUFDest = 0;
        decimal vTotICMSUFRemet = 0;

        decimal vBCUFDest = 0; //Valor da BC do CIMS na UF de destino
        decimal pFCPUFDest = 0; // Percentual do ICMS relativo ao fundo de Combrate à Pobreza (FCP) da UF de destino
        decimal pICMSUFDest = 0; // Aliquota interna da UF de destino
        decimal pICMSInter = 0; /*Alíquota interestadual das UF envolvidas
                                    - 4% alíquota interestadual para produtos importados;
                                    - 7% para os Estados de origem do Sul e Sudeste (exceto ES), destinado para os Estados do Norte, Nordeste, CentroOeste e Espírito Santo;
                                    - 12% para os demais casos.*/
        decimal pICMSInterPart = 0; /*Percentual de ICMS Interestadual para a UF de destino:
                                        - 40% em 2016;
                                        - 60% em 2017;
                                        - 80% em 2018;
                                        - 100% a partir de 2019*/
        decimal vFCPUFDest = 0; //Valor do ICMS relativo ao Fundo de Combate à Pobreza (FCP) da UF de destino
        decimal vICMSUFDest = 0; //Valor do ICMS Interestadual para a UF de destino, já considerando o valor do ICMS relativo ao Fundo de Combate à Pobreza naquela UF.
        decimal vICMSUFRemet = 0;//Valor do ICMS Interestadual para a UF do remetente. Nota: A partir de 2019, este valor será zero
        private String cUF
        {
            get
            {
                if (nf != null)
                    return Conexao.retornaUmValor("select uf from unidade_federacao where sigla_uf='" + nf.usr.filial.UF + "' group by uf", new User());
                else
                    return Conexao.retornaUmValor("select uf from unidade_federacao where sigla_uf='" + usr.filial.UF + "' group by uf", new User());
            }
            set { }
        }

        private String cDV = "";
        private nfDAO nf = null;
        private bool ivaDescricao = false;
        private String DIR_PADRAO = "";

        public xmlNFE(nfDAO nf)
        {
            if (nf != null)
            {
                DIR_PADRAO = nf.usr.filial.diretorio_exporta + "/SOLDI_XMLS";
                this.nf = nf;
                nf.calculaTotalItens();

                tpAmb = (nf.usr.filial.producaoNfe ? 1 : 2);
                nf.producao_nfe = nf.usr.filial.producaoNfe;

                String vltDec = Funcoes.valorParametro("CASAS_DECIMAIS_NFE", nf.usr);
                if (!vltDec.Equals(""))
                {
                    CasasDecimais = "N" + vltDec;
                }
                else
                {
                    CasasDecimais = "N4";
                }

            }
            else
                throw new Exception("Nota Fiscal Invalida");
        }

        public xmlNFE(User usr)
        {
            this.usr = usr;
            DIR_PADRAO = usr.filial.diretorio_exporta + "/SOLDI_XMLS";
        }

        private String funGrava(String nome, String valor)
        {
            valor = valor.Replace("------", "");
            if (!valor.Trim().Equals(""))
            {
                return "<" + nome + ">" + valor + "</" + nome + ">";
            }
            else
            {
                return "<" + nome.Trim() + "/>";
            }
        }

        private String funIde()
        {
            String cUF = this.cUF;
            String cNF = nf.usr.filial.chave_XML;
            String natOp = Conexao.retornaUmValor("select descricao from natureza_operacao where codigo_operacao = '" + nf.Codigo_operacao + "'", new User()).Trim();
            //String indPag = nf.pagamentoAvista();

            String mod = "55";
            String serie = nf.usr.filial.serie_nfe.ToString();
            String nNF = long.Parse(nf.Codigo).ToString();

            String dEmi = nf.Emissao.ToString("yyyy-MM-ddT" + DateTime.Now.ToString("HH:mm") + ":sszzz");
            String dSaiEnt = nf.Data.ToString("yyyy-MM-ddT" + DateTime.Now.ToString("HH:mm") + ":sszzz");

            int tpNF = (nf.Tipo_NF.Trim().Equals("2") ? 0 : 1);
            int idDest = (nf.usr.filial.UF.Equals(nf.UfCliente) ? 1 : 2);
            String cMunFG = Conexao.retornaUmValor("Select munic from unidade_federacao where nome_munic='" + nf.usr.filial.Cidade + "' and sigla_uf='" + nf.usr.filial.UF.Trim() + "'", new User());
            int tpImp = 1;
            int tpEmis = 1;
            String cDV = this.cDV;
            int tpAmb = this.tpAmb;// Tipo de Ambiente 1-Produão; 2-Homologação
            int finNFe = nf.finNFe;//(nf.NtOperacao.NF_devolucao ? 4 : 1);

            int indFinal = nf.indFinal;
            int indPres = nf.indPres; //Indicador de presença


            int indIntermed = nf.indIntermed;   //Indicador de intermediador/marketplace
                                                //0 = Operação sem intermediador(em site ou plataforma própria)
                                                //1 = Operação em site ou plataforma de terceiros (intermediadores/ marketplace)

            int procEmi = 0;
            String verProc = "1.4.1";

            StringBuilder strIde = new StringBuilder();
            strIde.Append("<ide>");
            strIde.Append(funGrava("cUF", cUF));            // Unidade da Federação
            strIde.Append(funGrava("cNF", cNF));           // Codigo Numérico que compçoes a chaves de acesso. Número aleatório gerado pelo emitente para cada NF-e para eveitar acessos indevidos da NF-e
            strIde.Append(funGrava("natOp", natOp));       // Informar a natureza da operação de que decorrer a saída ou a entrada, tais como:
                                                           // venda, compra, transferência, devolução, importação, consignação, remessa (para fins de demonstração, de
                                                           // industrialização ou outra), conforme previsto na alínea 'i', inciso I, art. 19 do CONVÊNIO S/Nº, de 15 de dezembro de
                                                           // 1970.
                                                           //if (versao.Equals("3.10"))
                                                           //{
            //strIde.Append(funGrava("indPag", indPag));     // Indicador da forma de pagamento 0 - Pagamento a Vista; 1 - Pagamento à Prazo; 2 - Outros
                                                           //}

            strIde.Append(funGrava("mod", mod));           // Código do Modelo do Documento
            strIde.Append(funGrava("serie", serie));       // Série do Documento Fiscal
            strIde.Append(funGrava("nNF", nNF));           // Número da NF
            strIde.Append(funGrava("dhEmi", dEmi));         // Data de Emissão do Documento
            strIde.Append(funGrava("dhSaiEnt", dSaiEnt));   // Data da Saida ou Entrada

            strIde.Append(funGrava("tpNF", tpNF.ToString()));// Tipo de Operação 0 - Entrada; 1 - Saida
            strIde.Append(funGrava("idDest", idDest.ToString()));// Identificador de local de destino da operação 1=Operação interna;2=Operação interestadual;3=Operação com exterior.
            strIde.Append(funGrava("cMunFG", cMunFG));     // Codigo do Municipio de Ocorrência do Fato Gerador



            strIde.Append(funGrava("tpImp", tpImp.ToString()));  // Formato de Impressão do DANFE
            strIde.Append(funGrava("tpEmis", tpEmis.ToString()));// Tipo de Emissão da NF-e
            strIde.Append(funGrava("cDV", cDV));  // Dígito Verificador da Chave de Acesso da NF-e
            strIde.Append(funGrava("tpAmb", tpAmb.ToString()));  // Tipo de Ambiente 1-Produão; 2-Homologação
            strIde.Append(funGrava("finNFe", finNFe.ToString()));// Finalidade de Emissão da NF-e 1- NF-e normal/ 2-NF-e Complementar / 3 – NF-e de ajuste
            strIde.Append(funGrava("indFinal", indFinal.ToString()));//Indica operação com Consumidor final 0=Não;1=Consumidor final;
            strIde.Append(funGrava("indPres", indPres.ToString()));// Indicador de presença do comprador no estabelecimento comercial nomomento da operação 0=Não se aplica (por exemplo, Nota Fiscal complementar
                                                                   //                 ou de ajuste);
                                                                   //                 1=Operação presencial;
                                                                   //                 2=Operação não presencial, pela Internet;
                                                                   //                 3=Operação não presencial, Teleatendimento;
                                                                   //                 4=NFC-e em operação com entrega a domicílio;
                                                                   //                 9=Operação não presencial, outros.
                                                                   //====================================================================================================================
            if (indPres == 2)
            {
                strIde.Append(funGrava("indIntermed", indIntermed.ToString()));// indicador de intermediador/marketplace
                                                                               //  0=Operação sem intermediador (em site ou plataforma própria) 
                                                                               //  1 = Operação em site ou plataforma de terceiros(intermediadores/ marketplace)
                                                                               //====================================================================================================================
            }

            strIde.Append(funGrava("procEmi", procEmi.ToString())); // Identificador do processo de emissão da NF-e: 0 - emissão de NF-e com aplicativo do contribuinte; 1 - emissão de NF-e avulsa pelo
                                                                    // Fisco; 2 - emissão de NF-e avulsa, pelo contribuinte com seu certificado digital, através do site do Fisco;
                                                                    // 3- emissão NF-e pelo contribuinte com aplicativo fornecido pelo Fisco.
            strIde.Append(funGrava("verProc", verProc)); // Identificador da versão do processo de emissão (informar a versão do aplicativo emissor de NF-e).
            if (nf.NfReferencias.Count > 0)
            {
                foreach (String idNfRef in nf.NfReferencias)
                {

                    strIde.Append("<NFref>");
                    strIde.Append(funGrava("refNFe", idNfRef));
                    strIde.Append("</NFref>");

                }


            }
            if (nf.ECFReferencias.Count > 0)
            {
                foreach (ArrayList EcfRef in nf.ECFReferencias)
                {

                    strIde.Append("<NFref>");
                    strIde.Append("<refECF>");
                    strIde.Append(funGrava("mod", "2D"));
                    strIde.Append(funGrava("nECF", EcfRef[0].ToString()));
                    strIde.Append(funGrava("nCOO", EcfRef[1].ToString()));
                    strIde.Append("</refECF>");
                    strIde.Append("</NFref>");

                }
            }

            strIde.Append("</ide>");
            return strIde.ToString();
        }

        private String funEmit()
        {

            String strPessoa = "J";
            String CNPJ = nf.usr.filial.CNPJ.Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim();
            String CPF = "";
            String xNome = nf.usr.filial.Razao_Social.Trim();
            String xFant = nf.usr.filial.Fantasia.Trim();
            String xLgr = nf.usr.filial.Endereco.Trim();
            String nro = nf.usr.filial.endereco_nro.Trim();
            String xCpl = "";
            String xBairro = nf.usr.filial.bairro.Trim();
            String cMun = Conexao.retornaUmValor("Select munic from unidade_federacao where nome_munic='" + nf.usr.filial.Cidade.Trim() + "' and sigla_uf='" + nf.usr.filial.UF.Trim() + "'", new User()); ;
            String xMun = nf.usr.filial.Cidade.Trim();
            String UF = nf.usr.filial.UF.Trim();
            String CEP = nf.usr.filial.CEP.Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim();
            String cPais = "1058";
            String xPais = "BRASIL";
            String Fone = nf.usr.filial.fone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim();
            String IE = nf.usr.filial.IE.Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim();
            // String IEst="";
            //String IM="";
            //String CNAE="";
            String CRT = nf.usr.filial.CRT;


            StringBuilder strEmit = new StringBuilder();
            strEmit.Append("<emit>");
            if (strPessoa.Equals("J"))
                strEmit.Append(funGrava("CNPJ", CNPJ));      // CNPJ do emitente
            else
                strEmit.Append(funGrava("CPF", CPF));       // CPF do emitente

            strEmit.Append(funGrava("xNome", xNome));       // Razão Social ou Nome do emitente
            strEmit.Append(funGrava("xFant", xFant));       // Nome Fantasia
            strEmit.Append("<enderEmit>");                  // grupo do Endereço do emitente
            strEmit.Append(funGrava("xLgr", xLgr));     // Logradouro
            strEmit.Append(funGrava("nro", nro.ToString().Replace(",", ".")));       // Numero
            if (xCpl.Trim().Length > 0)
                strEmit.Append(funGrava("xCpl", xCpl)); // Complemento

            strEmit.Append(funGrava("xBairro", xBairro));// Bairro
            strEmit.Append(funGrava("cMun", cMun));     // Código do Municipio
            strEmit.Append(funGrava("xMun", xMun));     // Nome do município
            strEmit.Append(funGrava("UF", UF));         // Sigla da UF (Unidade da Federação)
            strEmit.Append(funGrava("CEP", CEP));       // CEP (Código de Endereçamento Postal)
            strEmit.Append(funGrava("cPais", cPais));   // Código do Pais
            strEmit.Append(funGrava("xPais", xPais));   // Nome do País
            strEmit.Append(funGrava("fone", Fone));     // Telefone
            strEmit.Append("</enderEmit>");
            strEmit.Append(funGrava("IE", IE));             // Incrição Estadual

            //   strEmit.Append(funGrava("IEst", IEst));    // Inscrição Estadual do Substituto Tributário
            //   strEmit.Append(funGrava("IM", IM));        // Inscrição Municipal
            //   strEmit.Append(funGrava("CNAE", CNAE));    // CNAE Fiscal

            strEmit.Append(funGrava("CRT", CRT));       // Código do Regime Tributário (1 - Simples Nacional; 2 - Simples Nacional - excesso de sublimite de receita bruta; 3 - Regime Normal)


            strEmit.Append("</emit>");
            return strEmit.ToString();

        }

        private String fun_dest()
        {
            fornecedorDAO fornecedor = null;
            ClienteDAO cliente = null;
            String strPessoa = "";

            String CNPJ = "";
            String CPF = "";

            String xNome = "";
            String xLgr = "";
            String nro = "";
            String xCpl = "";
            String xBairro = "";
            String cMun = "";
            String xMun = "";
            String UF = "";
            String CEP = "";
            String cPais = "1058";
            String xPais = "BRASIL";
            String Fone = "";
            String indIEDest = "";
            String IE = "";
            String email = "";

            //if ((!nf.NtOperacao.Saida && nf.NtOperacao.Imprime_NF) || nf.NtOperacao.NF_devolucao)
            if (nf.DestFornecedor)
            {
                fornecedor = new fornecedorDAO(nf.Cliente_Fornecedor, null);

                strPessoa = (fornecedor.pessoa_fisica ? "F" : "J");

                CNPJ = fornecedor.CNPJ.Replace(".", "").Replace("/", "").Replace("-", "").Trim();
                CPF = fornecedor.CNPJ.Replace(".", "").Replace("/", "").Replace("-", "").Trim();

                xNome = (tpAmb == 2 ? "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL" : fornecedor.Razao_social.Trim().Replace("&", "E"));
                xLgr = fornecedor.Endereco.Trim();
                nro = fornecedor.Endereco_nro.Trim();
                xCpl = "";
                xBairro = fornecedor.Bairro.Trim();
                cMun = Conexao.retornaUmValor("Select munic from unidade_federacao where nome_munic='" + fornecedor.Cidade.Trim() + "' and sigla_uf='" + fornecedor.UF.Trim() + "'", new User());
                xMun = fornecedor.Cidade.Trim();
                UF = fornecedor.UF.Trim();
                CEP = fornecedor.CEP.Replace("-", "").Trim();
                Fone = fornecedor.telefone1.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

                IE = fornecedor.IE.Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim();
                email = fornecedor.email.Trim();
                indIEDest = fornecedor.indIEDest.ToString();
            }
            else
            {

                cliente = nf.objClienteNovo;
                //indIEDest = "9";
                indIEDest = cliente.indIEDest.ToString();
                ivaDescricao = cliente.Iva_descricao;
                strPessoa = (cliente.Pessoa_Juridica ? "J" : "F");

                CNPJ = cliente.CNPJ.Replace(".", "").Replace("/", "").Replace("-", "").Trim();
                CPF = cliente.CNPJ.Replace(".", "").Replace("/", "").Replace("-", "").Trim();

                bool bNomeFantasia = Funcoes.valorParametro("NOME_FANTASIA_ENF", usr).ToUpper().Equals("TRUE");
                String strNomeCliente = (bNomeFantasia ? cliente.nome_fantasia : cliente.Nome_Cliente);

                //if (!nf.OrigemDevolucao)
                //{
                    xNome = (tpAmb == 2 ? "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL" : strNomeCliente.Trim().Replace("&", "E"));
                    xLgr = cliente.Endereco.Trim();
                    nro = cliente.endereco_nro.Trim();
                    xCpl = cliente.complemento_end.Trim();
                    xBairro = cliente.Bairro.Trim();
                    cMun = Conexao.retornaUmValor("Select munic from unidade_federacao where nome_munic='" + cliente.Cidade.Trim() + "' and sigla_uf='" + cliente.UF.Trim() + "'", new User());
                    xMun = cliente.Cidade.Trim();
                    UF = cliente.UF.ToUpper().Trim();
                    CEP = cliente.CEP.Replace("-", "").Trim();
                    cPais = "1058";
                    xPais = "BRASIL";
                    Fone = cliente.primeiroMeioComunicacao().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

                    IE = cliente.IE.Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim();
                    email = cliente.email().Trim();

                //}
                //else
                //{
                //    xNome = (tpAmb == 2 ? "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL" : strNomeCliente.Trim().Replace("&", "E"));
                //    xLgr = nf.usr.filial.Endereco;
                //    nro = nf.usr.filial.endereco_nro;
                //    xCpl = "";
                //    xBairro = nf.usr.filial.bairro;
                //    cMun = Conexao.retornaUmValor("Select munic from unidade_federacao where nome_munic='" + nf.usr.filial.Cidade.Trim() + "' and sigla_uf='" + nf.usr.filial.UF + "'", new User());
                //    xMun = nf.usr.filial.Cidade.Trim();
                //    UF = nf.usr.filial.UF.ToUpper().Trim();
                //    CEP = nf.usr.filial.CEP.Replace("-", "").Trim();
                //    cPais = "1058";
                //    xPais = "BRASIL";
                //    Fone = nf.usr.filial.telefone.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

                //    IE = cliente.IE.Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim();
                //    email = "sistemas@bratter.com.br";

                //}
            }


            String erroDestinatario = "";
            if (xLgr.Equals(""))
                erroDestinatario = "-Endereço do Destinatario não preenchido! <br/>";
            if (cMun.Equals(""))
                erroDestinatario += "-Cidade do Destinatario não preenchida ou Invalida! <br/>".ToString();
            if (UF.Equals(""))
                erroDestinatario += "-UF do Destinatario não preenchida! <br/>";

            if (CEP.Equals(""))
                erroDestinatario += "-CEP do Destinatario não preenchido! <br/> ";
            if (Fone.Equals(""))
                erroDestinatario += "-Telefone do Destinatario não preenchido! <br/>";

            if (email.Equals(""))
                erroDestinatario += "-Email do Destinatario não preenchido! <br/>";

            if (erroDestinatario.Length > 0)
                throw new Exception("===============ERROS================= <br/>" + erroDestinatario);

            StringBuilder strDest = new StringBuilder();
            strDest.Append("<dest>");
            if (strPessoa.Equals("J"))
                strDest.Append(funGrava("CNPJ", CNPJ));
            else
                strDest.Append(funGrava("CPF", CPF));


            strDest.Append(funGrava("xNome", xNome));
            strDest.Append("<enderDest>");
            strDest.Append(funGrava("xLgr", xLgr));
            if (nro.Trim().Length > 0)
                strDest.Append(funGrava("nro", nro.ToString()));
            else
                strDest.Append(funGrava("nro", "s/n"));

            if (xCpl.Trim().Length > 0)
                strDest.Append(funGrava("xCpl", xCpl));

            strDest.Append(funGrava("xBairro", xBairro));
            strDest.Append(funGrava("cMun", cMun));
            strDest.Append(funGrava("xMun", xMun));
            strDest.Append(funGrava("UF", UF));
            strDest.Append(funGrava("CEP", CEP));
            strDest.Append(funGrava("cPais", cPais));
            strDest.Append(funGrava("xPais", xPais));
            strDest.Append(funGrava("fone", Fone));
            strDest.Append("</enderDest>");
            //Caso seja uma NFe de saida
            if (nf.Tipo_NF.Equals("2"))
            {
                if (strPessoa.Equals("J"))
                {
                    if (!IE.Trim().Equals(""))
                    {
                        strDest.Append(funGrava("indIEDest", "1"));
                        strDest.Append(funGrava("IE", IE));
                    }
                    else
                    {
                        strDest.Append(funGrava("indIEDest", "2"));
                    }
                }
                else
                {
                    strDest.Append(funGrava("indIEDest", "2"));
                }
            }
            else
            {

                strDest.Append(funGrava("indIEDest", indIEDest));
                if (indIEDest.Equals("1") && !IE.Trim().Equals(""))
                {
                    strDest.Append(funGrava("IE", IE));
                }

            }

            strDest.Append(funGrava("email", email));
            strDest.Append("</dest>");

            return strDest.ToString();
        }

        private String fun_prod()
        {
            StringBuilder strProd = new StringBuilder();

            foreach (nf_itemDAO item in nf.NfItens)
            {

                //Se o código de emissão NFe está populado, será ele que vai compor o XML.
                String cProd = (item.codigoEmissaoNFe.Equals("") ? item.PLU.Trim() : item.codigoEmissaoNFe.Trim());
                String cEAN = (item.ean.Trim().Length == 0 ? "SEM GTIN" : item.ean.Trim()); 
                String xProd = Funcoes.RemoverAcentos(item.Descricao.Trim());
                if (ivaDescricao)
                {
                    MercadoriaDAO merc = new MercadoriaDAO(item.PLU, nf.usr);
                    Decimal Ipi = merc.IPI;
                    Decimal vmargIva = merc.margem_iva;
                    Decimal aliquota = merc.tributacaoEntrada.Entrada_ICMS; //item.aliquota_icms;
                    Decimal bcIcms = merc.preco_compra * (item.Qtde * item.Embalagem);


                    xProd += " " + Funcoes.StrImpostoST(Ipi, vmargIva, aliquota, bcIcms, aliquota);

                }

                String NCM = item.NCM.ToString().Trim().Replace(".", "");
                // String EXTIPI = "00";//?
                decimal CFOP = item.codigo_operacao;
                String uCom = item.Und;
                decimal QCom = (item.Qtde * item.Embalagem);
                decimal vUnCom = item.Unitario;
                decimal vProd = item.vtotal_produto;
                String cEANTrib = cEAN;
                String uTrib = item.Und;
                decimal qTrib = (item.Qtde * item.Embalagem);
                decimal vUnTrib = item.Unitario;
                decimal vFrete = item.Frete;
                decimal vSeg = 0;
                decimal vDesc = item.DescontoValor;
                decimal vOutro = item.despesas;
                decimal vTotTrib = item.TotTrib;

                byte indTot = 1;

                //NFE 4.0
                String indEscala = (item.indEscala ? "S" : "N");
                String CNPJFab = item.cnpj_Fabricante;
                String cBenef = item.cBenef;

                //**  Medicamentos
                string cCodigoProdutoANVISA = item.codigo_produto_ANVISA;
                string cMotivoIsencaoANVISA = item.motivo_isencao_ANVISA;
                decimal vPrecoMaximoANVISA = item.preco_Maximo_ANVISA;

                strProd.Append("<det nItem=\"" + item.Num_item + "\">");

                strProd.Append("<prod>");
                strProd.Append(funGrava("cProd", cProd));
                strProd.Append(funGrava("cEAN", cEAN));
                strProd.Append(funGrava("xProd", xProd));
                strProd.Append(funGrava("NCM", NCM));
                if (!item.CEST.Trim().Equals(""))
                {
                    strProd.Append(funGrava("CEST", item.CEST.Replace(".", "").PadLeft(7, '0')));
                    // nfe 4.0
                    strProd.Append(funGrava("indEscala", indEscala));
                    if (!CNPJFab.Trim().Equals(""))
                    {
                        strProd.Append(funGrava("CNPJFab", CNPJFab));
                    }
                    if (!cBenef.Trim().Equals(""))
                    {
                        strProd.Append(funGrava("cBenef", cBenef));
                    }

                }
                strProd.Append(funGrava("CFOP", Funcoes.decimalPonto(CFOP.ToString())));
                strProd.Append(funGrava("uCom", uCom));
                strProd.Append(funGrava("qCom", Funcoes.decimalPonto(QCom.ToString(CasasDecimais))));
                strProd.Append(funGrava("vUnCom", Funcoes.decimalPonto(vUnCom.ToString(CasasDecimais))));

                strProd.Append(funGrava("vProd", Funcoes.decimalPonto(vProd.ToString("N2"))));
                strProd.Append(funGrava("cEANTrib", cEANTrib));
                strProd.Append(funGrava("uTrib", uTrib));
                strProd.Append(funGrava("qTrib", Funcoes.decimalPonto(qTrib.ToString(CasasDecimais))));
                strProd.Append(funGrava("vUnTrib", Funcoes.decimalPonto(vUnTrib.ToString(CasasDecimais))));
                if (vFrete > 0)
                {
                    strProd.Append(funGrava("vFrete", Funcoes.decimalPonto(vFrete.ToString())));

                }

                if (vSeg > 0)
                    strProd.Append(funGrava("vSeg", Funcoes.decimalPonto(vSeg.ToString())));

                if (vDesc > 0)
                    strProd.Append(funGrava("vDesc", Funcoes.decimalPonto(vDesc.ToString("N2"))));

                if (vOutro > 0)
                    strProd.Append(funGrava("vOutro", Funcoes.decimalPonto(vOutro.ToString("N2"))));

                strProd.Append(funGrava("indTot", indTot.ToString()));

                //Número do pedido de compra
                if (!item.Ordem_compra.Trim().Equals("") || !item.pedidoItemNumero.Trim().Equals(""))
                {
                    String strOrdem = "";

                    if (!item.pedidoItemNumero.Trim().Equals(""))
                    {
                        strOrdem = item.pedidoItemNumero.Trim();
                        if (strOrdem.Length > 15)
                        {
                            strOrdem = strOrdem.Substring(0, 15);
                        }
                    }
                    else
                    {
                        strOrdem = item.Ordem_compra.Trim();
                        if (strOrdem.Length > 15)
                        {
                            strOrdem = strOrdem.Substring(0, 15);
                        }
                    }
                    strProd.Append(funGrava("xPed", strOrdem));
                }

                //Sequencia do item no pedido
                if (!item.pedidoItemSequencia.Trim().Equals(""))
                {
                    strProd.Append(funGrava("nItemPed", item.pedidoItemSequencia.Trim()));
                }

                if (!item.codigo_produto_ANVISA.Trim().Equals(""))
                {
                    strProd.Append("<med>");
                    strProd.Append(funGrava("cProdANVISA", cCodigoProdutoANVISA.ToString()));
                    if (cCodigoProdutoANVISA.Equals("ISENTO"))
                    {
                        strProd.Append(funGrava("xMotivoIsencao", cMotivoIsencaoANVISA.ToString()));
                    }
                    strProd.Append(funGrava("vPMC", Funcoes.decimalPonto(vPrecoMaximoANVISA.ToString("N2"))));
                    strProd.Append("</med>");
                }

                strProd.Append("</prod>");
                strProd.Append("<imposto>");

                if (vTotTrib > 0)
                {
                    strProd.Append(funGrava("vTotTrib", Funcoes.decimalPonto(vTotTrib.ToString("N2"))));
                }
                ClienteDAO cli = nf.objCliente;
                nf.indIEDest = cli.indIEDest;

                strProd.Append(fun_ICMS(item));
                if (item.vIpiv > 0)
                {
                    strProd.Append(fun_IPI(item));
                }



                strProd.Append(fun_PIS(item));
                strProd.Append(fun_COFINS(item));



                //Checa se será necessário incluir os campos para o grupo ICMSUFDest
                if (!(nf.usr.filial.ICMSSN.Equals("102") && nf.usr.filial.CRT.ToString().Equals("1")))
                {
                    if (!nf.DestFornecedor && nf.indFinal == 1 && nf.NtOperacao.Difal )
                    {

                        //UF de origem é diferente da UF de destino e o cliente final não é contribuinte de ICMS
                        if ((nf.usr.filial.UF.Trim().ToUpper() != cli.UF.Trim().ToUpper())) // && !cli.indIEDest.ToString().Equals("1"))
                        {
                            strProd.Append(fun_ICMSUFDest(item));
                        }
                    }
                }
                strProd.Append("</imposto>");
                if (item.vIPIDevol > 0)
                {
                    if (item.pDevol <= 0)
                    {
                        throw new Exception("Porcentagem de Devolucao não preenchida");

                    }

                    strProd.Append("<impostoDevol>");
                    strProd.Append("<pDevol>" + Funcoes.decimalPonto(item.pDevol.ToString()) + "</pDevol>");
                    strProd.Append("<IPI>");
                    strProd.Append("<vIPIDevol>" + Funcoes.decimalPonto(item.vIPIDevol.ToString()) + "</vIPIDevol>");
                    strProd.Append("</IPI>");
                    strProd.Append("</impostoDevol>");
                }



                strProd.Append("</det>");
            }

            return strProd.ToString();



        }

        private String fun_ICMS(nf_itemDAO item)
        {
            String orig = item.origem;
            String CST = item.indice_St.Trim();

            byte modBC = 3;
            decimal vBC = item.vBaseICMS;
            decimal pICMS = item.aliquota_icms;
            decimal vICMS = item.vicms;
            decimal modBCST = Decimal.Parse(Conexao.retornaUmValor("select isnull(Modalidade_BCICMSST,3) from mercadoria where plu='" + item.PLU + "'", null));
            decimal pMVAST = item.vmargemIva;
            decimal pRedBCST = item.redutor_base;
            decimal vBCST = item.vBaseIva;
            decimal pICMSST = item.aliquota_iva;
            decimal vICMSST = item.vIva;
            decimal pCredSN = item.pCredSN;
            decimal vCredICMSSN = item.vCredicmssn;

            //NF 4.0
            decimal vBCFCP = item.vBCFCP;
            decimal pFCP = item.pFCP;
            decimal vFCP = item.vFCP;
            decimal vBCFCPST = item.vBCFCPST;
            decimal pFCPST = item.pFCPST;
            decimal vFCPST = item.vFCPST;

            StringBuilder strICMS = new StringBuilder();

            strICMS.Append("<ICMS>");
            if (!nf.usr.filial.Reg_Federal.ToUpper().Equals("SIMPLES NACIONAL"))
            {
                //cASO SEJA UMA NFE COMPLEMENTAR
                if (nf.finNFe == 2 && nf.Tipo_NF == "2" && pICMS > 0)
                {
                    CST = "00";
                }

                //Incluso condição para, caso o CST seja 40, 41 ou 50 o sistema assume o 40 para todos
                if ((CST == "40") || (CST == "41") || (CST == "50"))
                {
                    strICMS.Append("<ICMS" + "40" + ">");
                }
                else
                {
                    strICMS.Append("<ICMS" + CST + ">");
                }

            }
            else
            {
                if ((CST == "101"))
                {
                    strICMS.Append("<ICMSSN101>");
                }
                else if ((CST == "102") || (CST == "103") || (CST == "300") || (CST == "400"))
                {
                    strICMS.Append("<ICMSSN102>");
                }
                else if ((CST == "201"))
                {
                    strICMS.Append("<ICMSSN201>");
                }
                else if ((CST == "202") || (CST == "203"))
                {
                    strICMS.Append("<ICMSSN202>");

                }
                else if ((CST == "500"))
                {
                    strICMS.Append("<ICMSSN500>");
                }
                else if (CST == "900")
                {
                    strICMS.Append("<ICMSSN900>");
                }
                else
                {
                    strICMS.Append("<ICMSSN" + CST + ">");
                }
            }
            switch (CST)
            {
                case "00":
                    strICMS.Append(funGrava("orig", orig));
                    strICMS.Append(funGrava("CST", CST));
                    strICMS.Append(funGrava("modBC", modBC.ToString()));//(0-Margem Valor Agregado %; 1-Pauta(Valor); 2-Preço Tabelado Máx (valor); 3-Valor da Operação)
                    strICMS.Append(funGrava("vBC", Funcoes.decimalPonto(vBC.ToString("N2"))));
                    strICMS.Append(funGrava("pICMS", Funcoes.decimalPonto(pICMS.ToString("N2"))));
                    strICMS.Append(funGrava("vICMS", Funcoes.decimalPonto(vICMS.ToString("N2"))));
                    if (!nf.DestFornecedor && nf.indFinal != 1 && nf.indIEDest != 9 && nf.UfCliente.Equals(usr.filial.UF))
                    {
                        //NFE 4.0
                        if (pFCP > 0)
                            strICMS.Append(funGrava("pFCP", Funcoes.decimalPonto(pFCP.ToString("N2"))));

                        if (vFCP > 0)
                            strICMS.Append(funGrava("vFCP", Funcoes.decimalPonto(vFCP.ToString("N2"))));
                    }
                    break;
                case "10":
                    if (vBCST <= 0)
                        throw new Exception("Base ST com Valor Incorreto");

                    strICMS.Append(funGrava("orig", orig));
                    strICMS.Append(funGrava("CST", CST));
                    strICMS.Append(funGrava("modBC", modBC.ToString())); //(0-Margem Valor Agregado %; 1-Pauta(Valor); 2-Preço Tabelado Máx (valor); 3-Valor da Operação)
                    strICMS.Append(funGrava("vBC", Funcoes.decimalPonto(vBC.ToString("N2"))));
                    strICMS.Append(funGrava("pICMS", Funcoes.decimalPonto(pICMS.ToString("N2"))));
                    strICMS.Append(funGrava("vICMS", Funcoes.decimalPonto(vICMS.ToString("N2"))));

                    //NFE 4.0
                    if (!nf.DestFornecedor && nf.indFinal != 1 && nf.indIEDest != 9 && nf.UfCliente.Equals(usr.filial.UF))
                    {
                        if (vBCFCP > 0)
                            strICMS.Append(funGrava("vBCFCP", Funcoes.decimalPonto(vBCFCP.ToString("N2"))));

                        if (pFCP > 0)
                            strICMS.Append(funGrava("pFCP", Funcoes.decimalPonto(pFCP.ToString("N2"))));

                        if (vFCP > 0)
                            strICMS.Append(funGrava("vFCP", Funcoes.decimalPonto(vFCP.ToString("N2"))));
                    }
                    strICMS.Append(funGrava("modBCST", Funcoes.decimalPonto(modBCST.ToString()))); // (0-Preço Tabelado ou máximo; 1-Lista Negativa(valor); 2-Lista Positiva (valor); 3-Lista Neutra (valor); 4-Margem Valor Agregado(%); 5-Pauta (valor)
                    strICMS.Append(funGrava("pMVAST", Funcoes.decimalPonto(pMVAST.ToString("N2"))));
                    if (pRedBCST > 0)
                    {
                        strICMS.Append(funGrava("pRedBCST", Funcoes.decimalPonto(pRedBCST.ToString("N3"))));
                    }
                    strICMS.Append(funGrava("vBCST", Funcoes.decimalPonto(vBCST.ToString("N2"))));
                    strICMS.Append(funGrava("pICMSST", Funcoes.decimalPonto(pICMSST.ToString("N2"))));
                    strICMS.Append(funGrava("vICMSST", Funcoes.decimalPonto(vICMSST.ToString("N2"))));


                    //NFE 4.0
                    if (vBCFCPST > 0)
                        strICMS.Append(funGrava("vBCFCPST", Funcoes.decimalPonto(vBCFCPST.ToString("N2"))));

                    if (pFCPST > 0)
                        strICMS.Append(funGrava("pFCPST", Funcoes.decimalPonto(pFCPST.ToString("N2"))));

                    if (vFCPST > 0)
                        strICMS.Append(funGrava("vFCPST", Funcoes.decimalPonto(vFCPST.ToString("N2"))));



                    break;
                case "20":
                    if (pRedBCST <= 0)
                        throw new Exception("Redutor de Base com Valor Incorreto");
                    strICMS.Append(funGrava("orig", orig));
                    strICMS.Append(funGrava("CST", CST));
                    strICMS.Append(funGrava("modBC", modBC.ToString())); //(0-Margem Valor Agregado %; 1-Pauta(Valor); 2-Preço Tabelado Máx (valor); 3-Valor da Operação)
                    strICMS.Append(funGrava("pRedBC", Funcoes.decimalPonto(pRedBCST.ToString("N2"))));
                    strICMS.Append(funGrava("vBC", Funcoes.decimalPonto(vBC.ToString("N2"))));
                    strICMS.Append(funGrava("pICMS", Funcoes.decimalPonto(pICMS.ToString("N2"))));
                    strICMS.Append(funGrava("vICMS", Funcoes.decimalPonto(vICMS.ToString("N2"))));
                    if (!nf.DestFornecedor && nf.indFinal != 1 && nf.indIEDest != 9 && nf.UfCliente.Equals(usr.filial.UF))
                    {
                        //NFE 4.0
                        if (vBCFCP > 0)
                            strICMS.Append(funGrava("vBCFCP", Funcoes.decimalPonto(vBCFCP.ToString("N2"))));

                        if (pFCP > 0)
                            strICMS.Append(funGrava("pFCP", Funcoes.decimalPonto(pFCP.ToString("N2"))));

                        if (vFCP > 0)
                            strICMS.Append(funGrava("vFCP", Funcoes.decimalPonto(vFCP.ToString("N2"))));
                    }
                    break;
                case "30": // Isenta ou não tributada e com cobraça do ICMS por substituição tributária
                    if (vBCST <= 0)
                        throw new Exception("Base ST com Valor Incorreto");


                    strICMS.Append(funGrava("orig", orig));
                    strICMS.Append(funGrava("CST", CST));
                    strICMS.Append(funGrava("modBC", modBC.ToString())); //(0-Margem Valor Agregado %; 1-Pauta(Valor); 2-Preço Tabelado Máx (valor); 3-Valor da Operação)
                    strICMS.Append(funGrava("pMVAST", Funcoes.decimalPonto(pMVAST.ToString("N2"))));
                    strICMS.Append(funGrava("pRedBCST", Funcoes.decimalPonto(pRedBCST.ToString("N2"))));
                    strICMS.Append(funGrava("vBCST", Funcoes.decimalPonto(vBCST.ToString("N2"))));
                    strICMS.Append(funGrava("pICMSST", Funcoes.decimalPonto(pICMSST.ToString("N2"))));
                    strICMS.Append(funGrava("vICMSST", Funcoes.decimalPonto(vICMSST.ToString("N2"))));
                    //NFE 4.0
                    if (vBCFCPST > 0)
                        strICMS.Append(funGrava("vBCFCPST", Funcoes.decimalPonto(vBCFCPST.ToString("N2"))));

                    if (pFCPST > 0)
                        strICMS.Append(funGrava("pFCPST", Funcoes.decimalPonto(pFCPST.ToString("N2"))));

                    if (vFCPST > 0)
                        strICMS.Append(funGrava("vFCPST", Funcoes.decimalPonto(vFCPST.ToString("N2"))));
                    break;
                case "40": // Isenta
                    if (nf.usr.filial.Reg_Federal.ToUpper().Equals("SIMPLES NACIONAL"))
                    {
                        strICMS.Append(funGrava("orig", orig));
                        strICMS.Append(funGrava("CSOSN", "103"));
                    }
                    else
                    {
                        strICMS.Append(funGrava("orig", orig));
                        strICMS.Append(funGrava("CST", CST));
                    }
                    break;
                case "41": // Não tributada

                    strICMS.Append(funGrava("orig", orig));
                    strICMS.Append(funGrava("CST", CST));

                    break;
                case "50": // Suspensão
                    strICMS.Append(funGrava("orig", orig));
                    strICMS.Append(funGrava("CST", CST));
                    break;
                case "51": // Diferimento. A exigência do preenchimento das informações do ICMS diferido fica à critério de cada UF.
                    strICMS.Append(funGrava("orig", orig));
                    strICMS.Append(funGrava("CST", CST));
                    strICMS.Append(funGrava("modBC", modBC.ToString())); //(0-Margem Valor Agregado %; 1-Pauta(Valor); 2-Preço Tabelado Máx (valor); 3-Valor da Operação)
                    if (vBC > 0)
                    {
                        strICMS.Append(funGrava("pRedBC", Funcoes.decimalPonto(pRedBCST.ToString("N2"))));
                        strICMS.Append(funGrava("vBC", Funcoes.decimalPonto(vBC.ToString("N2"))));
                        strICMS.Append(funGrava("pICMS", Funcoes.decimalPonto(pICMS.ToString("N2"))));
                        strICMS.Append(funGrava("vICMS", Funcoes.decimalPonto(vICMS.ToString("N2"))));
                    }

                    //NFE 4.0
                    if (!nf.DestFornecedor && nf.indFinal != 1 && nf.indIEDest != 9 && nf.UfCliente.Equals(usr.filial.UF))
                    {
                        if (vBCFCP > 0)
                            strICMS.Append(funGrava("vBCFCP", Funcoes.decimalPonto(vBCFCP.ToString("N2"))));

                        if (pFCP > 0)
                            strICMS.Append(funGrava("pFCP", Funcoes.decimalPonto(pFCP.ToString("N2"))));

                        if (vFCP > 0)
                            strICMS.Append(funGrava("vFCP", Funcoes.decimalPonto(vFCP.ToString("N2"))));
                    }
                    break;
                case "60": // ICMS cobrado anteriormente por substituição tributária
                    strICMS.Append(funGrava("orig", orig));
                    strICMS.Append(funGrava("CST", CST));
                    strICMS.Append(funGrava("vBCSTRet", "0"));
                    //NFe 4.0

                    strICMS.Append(funGrava("pST", "0.0000"));
                    strICMS.Append(funGrava("vICMSSubstituto", "0.00"));
                    strICMS.Append(funGrava("vICMSSTRet", "0"));
                    break;
                case "70": // Tributado pelo ICMS com redução de base de cálculo e cobrança do ICMS por substituição tributária ICMS por substituição tributária
                    if (vBCST <= 0)
                        throw new Exception("Base ST com Valor Incorreto");

                    if (pRedBCST <= 0)
                        throw new Exception("Redutor de Base com Valor Incorreto");

                    strICMS.Append(funGrava("orig", orig));
                    strICMS.Append(funGrava("CST", CST));
                    strICMS.Append(funGrava("modBC", modBC.ToString())); //(0-Margem Valor Agregado %; 1-Pauta(Valor); 2-Preço Tabelado Máx (valor); 3-Valor da Operação)
                    strICMS.Append(funGrava("pRedBC", Funcoes.decimalPonto(pRedBCST.ToString("N2"))));
                    strICMS.Append(funGrava("vBC", Funcoes.decimalPonto(vBC.ToString("N2"))));
                    strICMS.Append(funGrava("pICMS", Funcoes.decimalPonto(pICMS.ToString("N2"))));
                    strICMS.Append(funGrava("vICMS", Funcoes.decimalPonto(vICMS.ToString("N2"))));
                    if (!nf.DestFornecedor && nf.indFinal != 1 && nf.indIEDest != 9 && nf.UfCliente.Equals(usr.filial.UF))
                    {
                        //NFE 4.0
                        if (vBCFCP > 0)
                            strICMS.Append(funGrava("vBCFCP", Funcoes.decimalPonto(vBCFCP.ToString("N2"))));

                        if (pFCP > 0)
                            strICMS.Append(funGrava("pFCP", Funcoes.decimalPonto(pFCP.ToString("N2"))));

                        if (vFCP > 0)
                            strICMS.Append(funGrava("vFCP", Funcoes.decimalPonto(vFCP.ToString("N2"))));
                    }

                    strICMS.Append(funGrava("modBCST", modBCST.ToString())); // (0-Preço Tabelado ou máximo; 1-Lista Negativa(valor); 2-Lista Positiva (valor); 3-Lista Neutra (valor); 4-Margem Valor Agregado(%); 5-Pauta (valor)
                    strICMS.Append(funGrava("pMVAST", Funcoes.decimalPonto(pMVAST.ToString("N2"))));
                    strICMS.Append(funGrava("pRedBCST", Funcoes.decimalPonto(pRedBCST.ToString("N2"))));
                    strICMS.Append(funGrava("vBCST", Funcoes.decimalPonto(vBCST.ToString("N2"))));
                    strICMS.Append(funGrava("pICMSST", Funcoes.decimalPonto(pICMSST.ToString("N2"))));
                    strICMS.Append(funGrava("vICMSST", Funcoes.decimalPonto(vICMSST.ToString("N2"))));
                    //NFE 4.0
                    if (vBCFCPST > 0)
                        strICMS.Append(funGrava("vBCFCPST", Funcoes.decimalPonto(vBCFCPST.ToString("N2"))));

                    if (pFCPST > 0)
                        strICMS.Append(funGrava("pFCPST", Funcoes.decimalPonto(pFCPST.ToString("N2"))));

                    if (vFCP > 0)
                        strICMS.Append(funGrava("vFCPST", Funcoes.decimalPonto(vFCPST.ToString("N2"))));
                    break;

                case "90": // Outros
                    strICMS.Append(funGrava("orig", orig));
                    strICMS.Append(funGrava("CST", CST));
                    strICMS.Append(funGrava("modBC", modBC.ToString())); //(0-Margem Valor Agregado %; 1-Pauta(Valor); 2-Preço Tabelado Máx (valor); 3-Valor da Operação)
                    strICMS.Append(funGrava("vBC", Funcoes.decimalPonto(vBC.ToString("N2"))));
                    strICMS.Append(funGrava("pRedBC", Funcoes.decimalPonto(pRedBCST.ToString("N2"))));
                    strICMS.Append(funGrava("pICMS", Funcoes.decimalPonto(pICMS.ToString("N2"))));
                    strICMS.Append(funGrava("vICMS", Funcoes.decimalPonto(vICMS.ToString("N2"))));
                    //NFE 4.0
                    if (!nf.DestFornecedor && nf.indFinal != 1 && nf.indIEDest != 9 && nf.UfCliente.Equals(usr.filial.UF))
                    {
                        if (vBCFCP > 0)
                            strICMS.Append(funGrava("vBCFCP", Funcoes.decimalPonto(vBCFCP.ToString("N2"))));

                        if (pFCP > 0)
                            strICMS.Append(funGrava("pFCP", Funcoes.decimalPonto(pFCP.ToString("N2"))));

                        if (vFCP > 0)
                            strICMS.Append(funGrava("vFCP", Funcoes.decimalPonto(vFCP.ToString("N2"))));
                    }
                    strICMS.Append(funGrava("modBCST", modBCST.ToString())); // (0-Preço Tabelado ou máximo; 1-Lista Negativa(valor); 2-Lista Positiva (valor); 3-Lista Neutra (valor); 4-Margem Valor Agregado(%); 5-Pauta (valor)
                    strICMS.Append(funGrava("pMVAST", Funcoes.decimalPonto(pMVAST.ToString("N2"))));
                    strICMS.Append(funGrava("pRedBCST", Funcoes.decimalPonto(pRedBCST.ToString("N2"))));
                    strICMS.Append(funGrava("vBCST", Funcoes.decimalPonto(vBCST.ToString("N2"))));
                    strICMS.Append(funGrava("pICMSST", Funcoes.decimalPonto(pICMSST.ToString("N2"))));
                    strICMS.Append(funGrava("vICMSST", Funcoes.decimalPonto(vICMSST.ToString("N2"))));

                    //NFE 4.0
                    if (vBCFCPST > 0)
                        strICMS.Append(funGrava("vBCFCPST", Funcoes.decimalPonto(vBCFCPST.ToString("N2"))));

                    if (pFCPST > 0)
                        strICMS.Append(funGrava("pFCPST", Funcoes.decimalPonto(pFCPST.ToString("N2"))));

                    if (vFCPST > 0)
                        strICMS.Append(funGrava("vFCPST", Funcoes.decimalPonto(vFCPST.ToString("N2"))));

                    break;

                case "101": // Tributação do ICMS pelo SIMPLES NACIONAL e CSOSN=101
                    strICMS.Append(funGrava("orig", orig));
                    strICMS.Append(funGrava("CSOSN", CST));
                    strICMS.Append(funGrava("pCredSN", Funcoes.decimalPonto(pCredSN.ToString("N2")))); // porcentagem credito sobre o total do produto
                    strICMS.Append(funGrava("vCredICMSSN", Funcoes.decimalPonto(vCredICMSSN.ToString("N2")))); // valor de credito 
                    break;
                case "102":
                case "103":
                case "300":
                case "400": // Tributação do ICMS pelo SIMPLES NACIONAL 
                    strICMS.Append(funGrava("orig", orig));
                    strICMS.Append(funGrava("CSOSN", CST));
                    break;
                case "201":
                    strICMS.Append(funGrava("orig", orig));
                    strICMS.Append(funGrava("CSOSN", CST));
                    strICMS.Append(funGrava("modBCST", modBCST.ToString())); // (0-Preço Tabelado ou máximo; 1-Lista Negativa(valor); 2-Lista Positiva (valor); 3-Lista Neutra (valor); 4-Margem Valor Agregado(%); 5-Pauta (valor)
                    strICMS.Append(funGrava("pMVAST", Funcoes.decimalPonto(pMVAST.ToString("N2"))));
                    strICMS.Append(funGrava("pRedBCST", Funcoes.decimalPonto(pRedBCST.ToString("N2"))));
                    strICMS.Append(funGrava("vBCST", Funcoes.decimalPonto(vBCST.ToString("N2"))));
                    strICMS.Append(funGrava("pICMSST", Funcoes.decimalPonto(pICMSST.ToString("N2"))));
                    strICMS.Append(funGrava("vICMSST", Funcoes.decimalPonto(vICMSST.ToString("N2"))));
                    strICMS.Append(funGrava("pCredSN", Funcoes.decimalPonto(pCredSN.ToString("N2")))); // porcentagem credito sobre o total do produto
                    strICMS.Append(funGrava("vCredICMSSN", Funcoes.decimalPonto(vCredICMSSN.ToString("N2")))); // valor de credito 
                    if (vBCFCPST > 0)
                        strICMS.Append(funGrava("vBCFCPST", Funcoes.decimalPonto(vBCFCPST.ToString("N2"))));

                    if (pFCPST > 0)
                        strICMS.Append(funGrava("pFCPST", Funcoes.decimalPonto(pFCPST.ToString("N2"))));

                    if (vFCPST > 0)
                        strICMS.Append(funGrava("vFCPST", Funcoes.decimalPonto(vFCPST.ToString("N2"))));


                    break;
                case "202":
                case "203":
                    strICMS.Append(funGrava("orig", orig));
                    strICMS.Append(funGrava("CSOSN", CST));
                    strICMS.Append(funGrava("modBCST", modBCST.ToString())); // (0-Preço Tabelado ou máximo; 1-Lista Negativa(valor); 2-Lista Positiva (valor); 3-Lista Neutra (valor); 4-Margem Valor Agregado(%); 5-Pauta (valor)
                    strICMS.Append(funGrava("pMVAST", Funcoes.decimalPonto(pMVAST.ToString("N2"))));
                    strICMS.Append(funGrava("pRedBCST", Funcoes.decimalPonto(pRedBCST.ToString("N2"))));
                    strICMS.Append(funGrava("vBCST", Funcoes.decimalPonto(vBCST.ToString("N2"))));
                    strICMS.Append(funGrava("pICMSST", Funcoes.decimalPonto(pICMSST.ToString("N2"))));
                    strICMS.Append(funGrava("vICMSST", Funcoes.decimalPonto(vICMSST.ToString("N2"))));
                    //NFE 4.0
                    if (vBCFCPST > 0)
                        strICMS.Append(funGrava("vBCFCPST", Funcoes.decimalPonto(vBCFCPST.ToString("N2"))));

                    if (pFCPST > 0)
                        strICMS.Append(funGrava("pFCPST", Funcoes.decimalPonto(pFCPST.ToString("N2"))));

                    if (vFCPST > 0)
                        strICMS.Append(funGrava("vFCPST", Funcoes.decimalPonto(vFCPST.ToString("N2"))));

                    break;
                case "500":
                    strICMS.Append(funGrava("orig", orig));
                    strICMS.Append(funGrava("CSOSN", CST));
                    strICMS.Append(funGrava("vBCSTRet", Funcoes.decimalPonto(vBCST.ToString("N2"))));
                    //NFe 4.0
                    strICMS.Append(funGrava("pST", "0.00"));
                    strICMS.Append(funGrava("vICMSSubstituto", "0.00"));
                    strICMS.Append(funGrava("vICMSSTRet", Funcoes.decimalPonto(vICMSST.ToString("N2"))));

                    break;
                case "900":
                    strICMS.Append(funGrava("orig", orig));
                    strICMS.Append(funGrava("CSOSN", CST));
                    strICMS.Append(funGrava("modBC", modBC.ToString())); //(0-Margem Valor Agregado %; 1-Pauta(Valor); 2-Preço Tabelado Máx (valor); 3-Valor da Operação)
                    strICMS.Append(funGrava("vBC", Funcoes.decimalPonto(vBC.ToString("N2"))));
                    strICMS.Append(funGrava("pRedBC", Funcoes.decimalPonto(pRedBCST.ToString("N2"))));
                    strICMS.Append(funGrava("pICMS", Funcoes.decimalPonto(pICMS.ToString("N2"))));
                    strICMS.Append(funGrava("vICMS", Funcoes.decimalPonto(vICMS.ToString("N2"))));
                    strICMS.Append(funGrava("modBCST", modBCST.ToString())); // (0-Preço Tabelado ou máximo; 1-Lista Negativa(valor); 2-Lista Positiva (valor); 3-Lista Neutra (valor); 4-Margem Valor Agregado(%); 5-Pauta (valor)
                    strICMS.Append(funGrava("pMVAST", Funcoes.decimalPonto(pMVAST.ToString("N2"))));
                    strICMS.Append(funGrava("pRedBCST", Funcoes.decimalPonto(pRedBCST.ToString("N2"))));
                    strICMS.Append(funGrava("vBCST", Funcoes.decimalPonto(vBCST.ToString("N2"))));
                    strICMS.Append(funGrava("pICMSST", Funcoes.decimalPonto(pICMSST.ToString("N2"))));
                    strICMS.Append(funGrava("vICMSST", Funcoes.decimalPonto(vICMSST.ToString("N2"))));
                    //NFE 4.0
                    if (vBCFCPST > 0)
                        strICMS.Append(funGrava("vBCFCPST", Funcoes.decimalPonto(vBCFCPST.ToString("N2"))));

                    if (pFCPST > 0)
                        strICMS.Append(funGrava("pFCPST", Funcoes.decimalPonto(pFCPST.ToString("N2"))));

                    if (vFCPST > 0)
                        strICMS.Append(funGrava("vFCPST", Funcoes.decimalPonto(vFCPST.ToString("N2"))));

                    strICMS.Append(funGrava("pCredSN", Funcoes.decimalPonto(pCredSN.ToString("N2")))); // porcentagem credito sobre o total do produto
                    strICMS.Append(funGrava("vCredICMSSN", Funcoes.decimalPonto(vCredICMSSN.ToString("N2")))); // valor de credito 
                    break;

            }

            if (!nf.usr.filial.Reg_Federal.ToUpper().Equals("SIMPLES NACIONAL"))
            {
                if ((CST == "40") || (CST == "41") || (CST == "50"))
                {
                    strICMS.Append("</ICMS40>");
                }
                else
                {
                    strICMS.Append("</ICMS" + CST + ">");
                }
            }
            else
            {
                if ((CST == "101"))
                {
                    strICMS.Append("</ICMSSN101>");
                }
                else if ((CST == "102") || (CST == "103") || (CST == "300") || (CST == "400"))
                {
                    strICMS.Append("</ICMSSN102>");
                }
                else if ((CST == "201"))
                {
                    strICMS.Append("</ICMSSN201>");
                }
                else if ((CST == "202") || (CST == "203"))
                {
                    strICMS.Append("</ICMSSN202>");
                }
                else if ((CST == "500"))
                {
                    strICMS.Append("</ICMSSN500>");
                }
                else if (CST == "900")
                {
                    strICMS.Append("</ICMSSN900>");
                }
                else
                {

                    strICMS.Append("</ICMSSN" + CST + ">");
                }
            }

            strICMS.Append("</ICMS>");

            return strICMS.ToString();
        }


        private String fun_IPI(nf_itemDAO item)
        {

            String CST = item.CSTPIS;
            decimal vBC = item.vBaseICMS;
            decimal pIPI = item.IPI;
            decimal vIPI = item.IPIV;
            StringBuilder strIPI = new StringBuilder();
            strIPI.Append("<IPI>");
            strIPI.Append(funGrava("cEnq", "999"));
            if (vIPI > 0)
            {
                strIPI.Append("<IPITrib>");
                strIPI.Append(funGrava("CST", "50"));
                strIPI.Append(funGrava("vBC", Funcoes.decimalPonto(vBC.ToString("N2"))));
                strIPI.Append(funGrava("pIPI", Funcoes.decimalPonto(pIPI.ToString("N2"))));
                strIPI.Append(funGrava("vIPI", Funcoes.decimalPonto(vIPI.ToString("N2"))));
                strIPI.Append("</IPITrib>");
            }
            else
            {
                strIPI.Append("<IPINT>");
                strIPI.Append(funGrava("CST", "52"));
                strIPI.Append("</IPINT>");
            }
            strIPI.Append("</IPI>");
            return strIPI.ToString();
        }

        private String fun_PIS(nf_itemDAO item)
        {
            String CST = item.CSTPIS.Trim().PadLeft(2, '0');
            decimal vBC = item.vBASEPisCofins;
            decimal pPIS = item.PISp;
            decimal vPIS = item.PISV;

            StringBuilder strPIS = new StringBuilder();
            strPIS.Append("<PIS>");
            if (CST.Equals("01") || CST.Equals("02"))
            {
                strPIS.Append("<PISAliq>");
                strPIS.Append(funGrava("CST", CST));
                strPIS.Append(funGrava("vBC", Funcoes.decimalPonto(vBC.ToString("N2"))));
                strPIS.Append(funGrava("pPIS", Funcoes.decimalPonto(pPIS.ToString("N2"))));
                strPIS.Append(funGrava("vPIS", Funcoes.decimalPonto(vPIS.ToString("N2"))));
                strPIS.Append("</PISAliq>");
            }
            else if (CST.Equals("04") || CST.Equals("05") || CST.Equals("06") || CST.Equals("07") || CST.Equals("08") || CST.Equals("09"))
            {
                strPIS.Append("<PISNT>");
                strPIS.Append(funGrava("CST", CST));
                strPIS.Append("</PISNT>");
            }
            else if (CST.Equals("99"))
            {
                strPIS.Append("<PISOutr>");
                strPIS.Append(funGrava("CST", CST));
                strPIS.Append(funGrava("vBC", "0"));
                strPIS.Append(funGrava("pPIS", "0"));
                strPIS.Append(funGrava("vPIS", "0"));

                strPIS.Append("</PISOutr>");

            }

            else //if (nf.Tipo_NF.Equals("2"))
            {
                strPIS.Append("<PISOutr>");
                strPIS.Append(funGrava("CST", CST));
                strPIS.Append(funGrava("vBC", Funcoes.decimalPonto(vBC.ToString("N2"))));
                strPIS.Append(funGrava("pPIS", Funcoes.decimalPonto(pPIS.ToString("N2"))));
                strPIS.Append(funGrava("vPIS", Funcoes.decimalPonto(vPIS.ToString("N2"))));
                strPIS.Append("</PISOutr>");

            }

            strPIS.Append("</PIS>");


            return strPIS.ToString();

        }

        //Sinto Muito Me Perdoe Agradeço Eu Te Amo.

        private String fun_COFINS(nf_itemDAO item)
        {
            String CST = item.CSTCOFINS.PadLeft(2, '0');
            decimal vBC = item.vBASEPisCofins;
            decimal pCOFINS = item.COFINSp;
            decimal vCOFINS = item.COFINSV;

            StringBuilder strCOFINS = new StringBuilder();
            strCOFINS.Append("<COFINS>");

            if (CST.Equals("01") || CST.Equals("02"))
            {
                strCOFINS.Append("<COFINSAliq>");
                strCOFINS.Append(funGrava("CST", CST));
                strCOFINS.Append(funGrava("vBC", Funcoes.decimalPonto(vBC.ToString("N2"))));
                strCOFINS.Append(funGrava("pCOFINS", Funcoes.decimalPonto(pCOFINS.ToString("N2"))));
                strCOFINS.Append(funGrava("vCOFINS", Funcoes.decimalPonto(vCOFINS.ToString("N2"))));
                strCOFINS.Append("</COFINSAliq>");
            }
            else if (CST.Equals("04") || CST.Equals("05") || CST.Equals("06") || CST.Equals("07") || CST.Equals("08") || CST.Equals("09"))
            {
                strCOFINS.Append("<COFINSNT>");
                strCOFINS.Append(funGrava("CST", CST));
                strCOFINS.Append("</COFINSNT>");
            }
            else if (CST.Equals("99"))
            {
                strCOFINS.Append("<COFINSOutr>");
                strCOFINS.Append(funGrava("CST", CST));
                strCOFINS.Append(funGrava("vBC", "0"));
                strCOFINS.Append(funGrava("pCOFINS", "0"));
                strCOFINS.Append(funGrava("vCOFINS", "0"));
                strCOFINS.Append("</COFINSOutr>");

            }

            else // if (nf.Tipo_NF.Equals("2"))
            {
                strCOFINS.Append("<COFINSOutr>");
                strCOFINS.Append(funGrava("CST", CST));
                strCOFINS.Append(funGrava("vBC", Funcoes.decimalPonto(vBC.ToString("N2"))));
                strCOFINS.Append(funGrava("pCOFINS", Funcoes.decimalPonto(pCOFINS.ToString("N2"))));
                strCOFINS.Append(funGrava("vCOFINS", Funcoes.decimalPonto(vCOFINS.ToString("N2"))));
                strCOFINS.Append("</COFINSOutr>");

            }
            strCOFINS.Append("</COFINS>");
            return strCOFINS.ToString();
        }

        private String fun_ICMSUFDest(nf_itemDAO item)
        {
            // Tratamento para o novo grupo  
            // Grupo de Tributação do ICMS para a UF de destino
            vBCUFDest = 0; //Valor da BC do ICMS na UF de destino
            pFCPUFDest = 0; // Percentual do ICMS relativo ao fundo de Combate à Pobreza (FCP) da UF de destino
            pICMSUFDest = 0; // Aliquota interna da UF de destino
            pICMSInter = 0; /*Alíquota interestadual das UF envolvidas
                                - 4% alíquota interestadual para produtos importados;
                                - 7% para os Estados de origem do Sul e Sudeste (exceto ES), destinado para os Estados do Norte, Nordeste, CentroOeste e Espírito Santo;
                                - 12% para os demais casos.*/
            pICMSInterPart = 0; /*Percentual de ICMS Interestadual para a UF de destino:
                                    - 40% em 2016;
                                    - 60% em 2017;
                                    - 80% em 2018;
                                    - 100% a partir de 2019*/
            vFCPUFDest = 0; //Valor do ICMS relativo ao Fundo de Combate à Pobreza (FCP) da UF de destino
            vICMSUFDest = 0; //Valor do ICMS Interestadual para a UF de destino, já considerando o valor do ICMS relativo ao Fundo de Combate à Pobreza naquela UF.
            vICMSUFRemet = 0;//Valor do ICMS Interestadual para a UF do remetente. Nota: A partir de 2019, este valor será zero

            StringBuilder strICMSUFDest = new StringBuilder();
            strICMSUFDest.Append("<ICMSUFDest>");

            ClienteDAO cli = nf.objCliente; //new ClienteDAO(nf.Cliente_Fornecedor, new User());
            aliquota_imp_estadoDAO AliqInter = new aliquota_imp_estadoDAO(cli.UF.Trim().ToUpper(), item.NCM, null);

            vBCUFDest = item.vBaseICMS;
            //pICMSUFDest = item.aliquota_ICMS_Destino;
            pICMSUFDest = AliqInter.icms_interestadual;
            //Aliquota interestadual
            switch (cli.UF.Trim().ToUpper())
            {
                case "RS":
                case "PR":
                case "SC":
                case "MG":
                case "RJ":
                    pICMSInter = item.aliquota_icms;
                    break;
                default:
                    pICMSInter = item.aliquota_icms;
                    break;
            }
            //Percentual de participação
            switch (DateTime.Today.Year)
            {
                case 2016:
                    pICMSInterPart = 40;
                    break;
                case 2017:
                    pICMSInterPart = 60;
                    break;
                case 2018:
                    pICMSInterPart = 80;
                    break;
                default:
                    pICMSInterPart = 100;
                    break;
            }
            vFCPUFDest = 0;
            if (pICMSUFDest <= pICMSInter)
            {
                vICMSUFDest = 0;
                vICMSUFRemet = 0;
            }
            else
            {

                UfPobrezaDAO uf = UfPobrezaDAO.objUFPobreza(cli.UF.Trim().ToUpper());
                if (uf.calc_Fora == 1)
                {
                    vICMSUFDest = (vBCUFDest * ((pICMSUFDest - pICMSInter) / 100));
                }
                else //(uf.calc_Fora == 0)
                {
                    //vICMSUFDest = (vBCUFDest * ((pICMSUFDest - pICMSInter) / 100)) * (pICMSInterPart / 100);
                    decimal valorBCUFDest = ((vBCUFDest - (vBCUFDest * (pICMSInter / 100))) / ((100 - pICMSUFDest - AliqInter.porc_combate_pobresa) / 100));
                    vICMSUFDest = item.valor_Difal_Item; //   (((vBCUFDest - (vBCUFDest * (pICMSInter / 100))) / ((100 - pICMSUFDest) / 100)) * (pICMSUFDest / 100)) - (vBCUFDest * (pICMSInter / 100));
                    vBCUFDest =  valorBCUFDest;
                    vICMSUFRemet = 0; // (vBCUFDest * ((pICMSUFDest - pICMSInter) / 100)) - vICMSUFDest;

                    pFCPUFDest = AliqInter.porc_combate_pobresa;
                    vFCPUFDest = item.vFCP;

                }
                vTotFCPUFDest += vFCPUFDest;
                vTotICMSUFDest = vTotICMSUFDest + vICMSUFDest;
                vTotICMSUFRemet =  vTotICMSUFRemet + vICMSUFRemet;
            }
            if (nf.usr.filial.CRT.ToString().Equals("1"))
            {

                strICMSUFDest.Append(funGrava("vBCUFDest", Funcoes.decimalPonto(0.ToString("N2"))));
                strICMSUFDest.Append(funGrava("pICMSUFDest", Funcoes.decimalPonto(0.ToString("N2"))));
                strICMSUFDest.Append(funGrava("pICMSInter", Funcoes.decimalPonto(pICMSInter.ToString("N2"))));
                strICMSUFDest.Append(funGrava("pICMSInterPart", Funcoes.decimalPonto(100.ToString("N2"))));
                strICMSUFDest.Append(funGrava("vICMSUFDest", Funcoes.decimalPonto(0.ToString("N2"))));
                strICMSUFDest.Append(funGrava("vICMSUFRemet", Funcoes.decimalPonto(0.ToString("N2"))));
            }
            else
            {
                strICMSUFDest.Append(funGrava("vBCUFDest", Funcoes.decimalPonto(vBCUFDest.ToString("N2"))));
                strICMSUFDest.Append(funGrava("pFCPUFDest", Funcoes.decimalPonto(pFCPUFDest.ToString("N2"))));
                strICMSUFDest.Append(funGrava("pICMSUFDest", Funcoes.decimalPonto(pICMSUFDest.ToString("N2"))));
                strICMSUFDest.Append(funGrava("pICMSInter", Funcoes.decimalPonto(pICMSInter.ToString("N2"))));
                strICMSUFDest.Append(funGrava("pICMSInterPart", Funcoes.decimalPonto(pICMSInterPart.ToString("N2"))));
                strICMSUFDest.Append(funGrava("vFCPUFDest", Funcoes.decimalPonto(vFCPUFDest.ToString("N2"))));
                strICMSUFDest.Append(funGrava("vICMSUFDest", Funcoes.decimalPonto(vICMSUFDest.ToString("N2"))));
                strICMSUFDest.Append(funGrava("vICMSUFRemet", Funcoes.decimalPonto(vICMSUFRemet.ToString("N2"))));
            }
            strICMSUFDest.Append("</ICMSUFDest>");

            return strICMSUFDest.ToString();
        }
        private String fun_Total()
        {
            decimal vBC = nf.Base_Calculo;
            decimal vICMS = nf.ICMS_Nota;
            decimal vBCST = nf.Base_Calc_Subst;
            decimal vST = nf.ICMS_ST;
            //decimal vProd = nf.valorTotalProdutos;
            decimal vProd = nf.TotalProdutos;
            decimal vFrete = nf.Frete;
            decimal vSeg = nf.Seguro;
            decimal vDesc = nf.Desconto;
            decimal vIPI = nf.IPI_Nota;
            decimal vPIS = nf.TotalPis;
            decimal vCOFINS = nf.TotalCofins;
            decimal vOutro = nf.Despesas_financeiras;
            decimal vNF = nf.Total;
            decimal vTotTrib = nf.vTotTrib;
            decimal vFCP = nf.vFCP;
            decimal vFCPST = nf.vFCPST;
            decimal vIPIDevol = nf.vIPIDevol;

            StringBuilder strTOTAL = new StringBuilder();
            strTOTAL.Append("<total>");
            strTOTAL.Append("<ICMSTot>");



            strTOTAL.Append(funGrava("vBC", Funcoes.decimalPonto(vBC.ToString("N2"))));
            strTOTAL.Append(funGrava("vICMS", Funcoes.decimalPonto(vICMS.ToString("N2"))));


            strTOTAL.Append(funGrava("vICMSDeson", "0"));

            //Checa se será necessário incluir os campos para o grupo ICMSUFDest


            if (!(nf.usr.filial.ICMSSN.ToString().Equals("102") && nf.usr.filial.CRT.ToString().Equals("1")))
            {
                if (!nf.DestFornecedor && nf.indFinal == 1)
                {
                    ClienteDAO cli = nf.objCliente;
                    if (nf.usr.filial.UF.Trim().ToUpper() != cli.UF.Trim().ToUpper())
                    {
                        strTOTAL.Append(funGrava("vFCPUFDest", Funcoes.decimalPonto(vTotFCPUFDest.ToString("N2"))));
                        strTOTAL.Append(funGrava("vICMSUFDest", Funcoes.decimalPonto(vTotICMSUFDest.ToString("N2"))));
                        strTOTAL.Append(funGrava("vICMSUFRemet", Funcoes.decimalPonto(vTotICMSUFRemet.ToString("N2"))));
                    }
                }
            }

            if (!nf.DestFornecedor && nf.indFinal != 1 && nf.indIEDest != 9 && nf.UfCliente.Equals(usr.filial.UF))
            {
                //nfe 4.0
                strTOTAL.Append(funGrava("vFCP", Funcoes.decimalPonto(vFCP.ToString("N2"))));
            }
            else
            {
                strTOTAL.Append(funGrava("vFCP", "0.00"));
            }

            strTOTAL.Append(funGrava("vBCST", Funcoes.decimalPonto(vBCST.ToString("N2"))));
            strTOTAL.Append(funGrava("vST", Funcoes.decimalPonto(vST.ToString("N2"))));
            // nfe 4.0
            strTOTAL.Append(funGrava("vFCPST", Funcoes.decimalPonto(vFCPST.ToString("N2"))));
            strTOTAL.Append(funGrava("vFCPSTRet", "0.00"));

            strTOTAL.Append(funGrava("vProd", Funcoes.decimalPonto(vProd.ToString("N2"))));
            strTOTAL.Append(funGrava("vFrete", Funcoes.decimalPonto(vFrete.ToString("N2"))));
            strTOTAL.Append(funGrava("vSeg", Funcoes.decimalPonto(vSeg.ToString("N2"))));
            if (nf.producao_nfe)
                strTOTAL.Append(funGrava("vDesc", Funcoes.decimalPonto(vDesc.ToString("N2"))));
            else
                strTOTAL.Append(funGrava("vDesc", Funcoes.decimalPonto("0.01")));

            strTOTAL.Append("<vII>0.00</vII>");
            strTOTAL.Append(funGrava("vIPI", Funcoes.decimalPonto(vIPI.ToString("N2"))));

            //NFe 4.0
            strTOTAL.Append(funGrava("vIPIDevol", Funcoes.decimalPonto(vIPIDevol.ToString())));

            strTOTAL.Append(funGrava("vPIS", Funcoes.decimalPonto(vPIS.ToString("N2"))));
            strTOTAL.Append(funGrava("vCOFINS", Funcoes.decimalPonto(vCOFINS.ToString("N2"))));
            strTOTAL.Append(funGrava("vOutro", Funcoes.decimalPonto(vOutro.ToString("N2"))));
            if (nf.producao_nfe)
                strTOTAL.Append(funGrava("vNF", Funcoes.decimalPonto(vNF.ToString("N2"))));
            else
                strTOTAL.Append(funGrava("vNF", Funcoes.decimalPonto((vNF - 0.01m).ToString("N2"))));
            if (vTotTrib > 0)
                strTOTAL.Append(funGrava("vTotTrib", Funcoes.decimalPonto(vTotTrib.ToString("N2"))));
            strTOTAL.Append("</ICMSTot>");

            strTOTAL.Append("</total>");
            return strTOTAL.ToString();
        }

        private String fun_TRANSP()
        {

            String modFrete = nf.tipo_frete;



            String CNPJ = nf.cnpjTransportadora;

            String xNome = nf.nome_transportadora.Trim();
            String IE = "";
            String xEnder = nf.endereco_transportadora;
            String xMun = nf.municipio_transportadora;
            String UF = nf.estado_transportadora;
            String qVol = Funcoes.decimalPonto(nf.qtde.ToString("N0"));
            String esp = nf.especie;
            decimal pesoL = nf.peso_liquido;
            decimal pesoB = nf.peso_bruto;

            StringBuilder strTransp = new StringBuilder();

            if (nf.tipo_frete.Equals("9"))
            {
                strTransp.Append("<transp>");
                strTransp.Append("<modFrete>" + modFrete + "</modFrete>");
                strTransp.Append("</transp>");
            }
            else
            {
                strTransp.Append("<transp>");
                strTransp.Append("<modFrete>" + modFrete + "</modFrete>");
                strTransp.Append("<transporta>");
                if (!CNPJ.Equals(""))
                {
                    if (CNPJ.Trim().Length == 14)
                        strTransp.Append(funGrava("CNPJ", CNPJ));
                    else
                        strTransp.Append(funGrava("CPF", CNPJ));

                }
                strTransp.Append(funGrava("xNome", xNome));
                // strTransp.Append(funGrava("IE", IE));
                if (!xEnder.Equals(""))
                    strTransp.Append(funGrava("xEnder", xEnder));

                if (!xMun.Equals(""))
                    strTransp.Append(funGrava("xMun", xMun));

                if (!UF.Equals(""))
                    strTransp.Append(funGrava("UF", UF));

                strTransp.Append("</transporta>");

                strTransp.Append("<vol>");
                strTransp.Append(funGrava("qVol", qVol.ToString()));
                strTransp.Append(funGrava("esp", esp));
                strTransp.Append(funGrava("pesoL", Funcoes.decimalPonto(pesoL.ToString("N3"))));
                strTransp.Append(funGrava("pesoB", Funcoes.decimalPonto(pesoB.ToString("N3"))));
                strTransp.Append("</vol>");
                strTransp.Append("</transp>");
            }
            return strTransp.ToString();

        }

        private String funPagamenos()
        {
            StringBuilder StrPagamentos = new StringBuilder();

            if (nf.tPag.Equals("90"))
            {
                StrPagamentos.Append("<cobr/>");
            }
            else
            {
                StrPagamentos.Append("<cobr>");
                StrPagamentos.Append("<fat>");
                StrPagamentos.Append(funGrava("nFat", nf.Codigo.ToString()));
                StrPagamentos.Append(funGrava("vOrig", Funcoes.decimalPonto((nf.Total).ToString("N2"))));
                //StrPagamentos.Append("<vDesc>0.00</vDesc>");
                StrPagamentos.Append(funGrava("vLiq", Funcoes.decimalPonto(nf.Total.ToString("N2"))));
                StrPagamentos.Append("</fat>");


                String tPg = nf.tPag;

                if (tPg.Equals("14") || tPg.Equals("15"))
                {
                    int i = 1;
                    foreach (nf_pagamentoDAO pg in nf.NfPagamentos)
                    {
                        StrPagamentos.Append("<dup>");
                        StrPagamentos.Append(funGrava("nDup", i.ToString().PadLeft(3, '0')));
                        StrPagamentos.Append(funGrava("dVenc", pg.Vencimento.ToString("yyyy-MM-dd")));
                        StrPagamentos.Append(funGrava("vDup", Funcoes.decimalPonto(pg.Valor.ToString("N2"))));
                        StrPagamentos.Append("</dup>");
                        i++;
                    }
                }
                StrPagamentos.Append("</cobr>");
            }


            return StrPagamentos.ToString();
        }


        public String funChave()
        {

            String UF = this.cUF;
            if (UF.Trim().Equals(""))
            {
                throw new Exception("Uf não configurada na Tabela Filial");
            }
            String strAnoMes = nf.Emissao.ToString("yyMM");
            String CNPJ = nf.usr.filial.CNPJ.Trim().Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "");
            if (CNPJ.Trim().Equals(""))
            {
                throw new Exception("CNPJ não configurado na Tabela Filial");
            }
            String strModelo = "55";
            int strSerie = nf.usr.filial.serie_nfe;

            long nNF = long.Parse(nf.Codigo);

            byte tpEmi = 1;
            long cNF = long.Parse(nf.usr.filial.chave_XML);

            int nFor = 0;
            int nPeso = 0;
            int nSoma = 0;
            int nResto = 0;
            int nDV = 0;

            StringBuilder strChave = new StringBuilder();
            strChave.Append(UF);
            strChave.Append(strAnoMes);
            strChave.Append(CNPJ);
            strChave.Append(strModelo);
            strChave.Append(strSerie.ToString("000"));
            strChave.Append(nNF.ToString("000000000"));
            strChave.Append(tpEmi.ToString("0"));
            strChave.Append(cNF.ToString("00000000"));

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

            this.cDV = nDV.ToString();
            strChave.Append(nDV.ToString().Trim());
            nf.id = strChave.ToString();
            Conexao.executarSql("update nf set id ='" + strChave.ToString() + "' where codigo='" + nf.Codigo + "' and Cliente_Fornecedor='" + nf.Cliente_Fornecedor + "' and FILIAL='" + nf.Filial + "'");
            return strChave.ToString();
        }

        public void gravarArquivo(bool arValidar)
        {

            StringBuilder xml = new StringBuilder();
            xml.Append("<?xml version=\"1.0\"?>");
            if (!nf.usr.filial.tipo_certificado.Equals("A3"))
            {
                xml.Append("<enviNFe xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"" + versao + "\">");
                xml.Append("<idLote>" + DateTime.Now.ToString("yyyyMMdd") + "</idLote>");
                xml.Append("<indSinc>0</indSinc>");
            }
            xml.Append("<NFe xmlns=\"http://www.portalfiscal.inf.br/nfe\">");
            xml.Append("<infNFe Id=\"NFe" + funChave() + "\" versao=\"" + versao + "\">");
            xml.Append(funIde());
            xml.Append(funEmit());
            xml.Append(fun_dest());

            //Alteração para inserir a tag AUTXML que permite atores (cnpj, cpf) a obterem acesso ao XML
            string strAutObterXML = Funcoes.valorParametro("AUT_OBTER_XML", null).Trim();
            if (!strAutObterXML.Equals(""))
            {
                xml.Append("<autXML><CNPJ>" + strAutObterXML + "</CNPJ></autXML>");
            }

            xml.Append(fun_prod());
            xml.Append(fun_Total());
            xml.Append(fun_TRANSP());


            xml.Append(funPagamenos());


            //01 = Dinheiro 
            //02 = Cheque 
            //03 = Cartão de Crédito 
            //04 = Cartão de Débito 
            //05 = Crédito Loja 
            //10 = Vale Alimentação 
            //11 = Vale Refeição 
            //12 = Vale Presente 
            //13 = Vale Combustível 
            //14 = Duplicata Mercantil 
            //15 = Boleto Bancário 
            //90 = Sem pagamento 
            //99 = Outros

            //Nfe 4.0
            String tPg = nf.tPag;
            String indPag = nf.pagamentoAvista();
            xml.Append("<pag>");
            xml.Append("<detPag>");
            if (nf.NfPagamentos.Count > 0)
                xml.Append(funGrava("indPag", indPag));     // Indicador da forma de pagamento 0 - Pagamento a Vista; 1 - Pagamento à Prazo; 2 - Outros
            xml.Append(funGrava("tPag", tPg));
            xml.Append(funGrava("vPag", tPg.Equals("90") ? "0.00" : Funcoes.decimalPonto(nf.TotalPag().ToString("N2"))));
            if (!nf.CNPJPagamento.Trim().Equals(""))
            {
                xml.Append("<card>");
                xml.Append(funGrava("tpIntegra", "2"));
                xml.Append(funGrava("CNPJ", nf.CNPJPagamento));
                xml.Append("</card>");
            }
            xml.Append("</detPag>");

            xml.Append("</pag>");

            //Se for operação não presencial pela internet (indPress=2) em site de terceiros(intermediadores) (indIntermed==1)
            if (nf.indIntermed == 1)
            {
                xml.Append("<infIntermed>");
                xml.Append(funGrava("CNPJ", nf.intermedCnpj));
                xml.Append(funGrava("idCadIntTran", nf.idCadIntTran));
                xml.Append("</infIntermed>");

            }
            if (nf.Pedidos.Count > 0)
            {
                foreach (String item in nf.Pedidos)
                {
                    nf.Observacao += "Pedido:" + item + ";";
                }

            }


            if (!nf.Observacao.Trim().Equals(""))
            {
                xml.Append("<infAdic>");
                xml.Append("<infCpl>");
                xml.Append(Funcoes.RemoverAcentos(nf.Observacao).Trim().Replace("\n", ";"));
                xml.Append("</infCpl>");
                xml.Append("</infAdic>");
            }
            xml.Append("</infNFe>");
            xml.Append("</NFe>");

            if (!nf.usr.filial.tipo_certificado.Equals("A3"))
            {
                xml.Append("</enviNFe>");
            }

            if (!Directory.Exists(DIR_PADRAO))
            {
                Directory.CreateDirectory(DIR_PADRAO);
            }


            StreamWriter ArqXml = new StreamWriter(DIR_PADRAO + "\\" + funChave() + ".xml", false, Encoding.ASCII);
            ArqXml.Write(xml.ToString());
            ArqXml.Close();

            //Voltar aqui para validar no futuro.
            if (false)
            {
                if (!nf.usr.filial.tipo_certificado.Equals("A3"))
                {
                    Assinar(nf.usr.filial.diretorio_exporta + "/" + funChave() + ".xml", "NFe", "infNFe");
                }
                Validar(DIR_PADRAO + "/" + funChave() + ".xml");

            }

        }


        private void reader_ValidationEventHandler(object sender, ValidationEventArgs e)
        {

            this.cErro += "Linha: " + e.Exception.LineNumber + " Coluna: " + e.Exception.LinePosition + " Erro: " + e.Exception.Message + "<br />";

        }
        private void Validar(string cRotaArqXML)
        {
            bool lArqXML = File.Exists(cRotaArqXML);
            String caminhoDoSchema = nf.usr.filial.caminhoServidor + "modulos\\NotaFiscal\\pages\\schemas\\enviNFe_v" + versao + ".xsd";
            bool lArqXSD = File.Exists(caminhoDoSchema);
            bool temXSD = true;

            String RetornoString = "";

            if (lArqXML && lArqXSD)
            {
                XmlReader xmlReader = null;

                try
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.ValidationType = ValidationType.Schema;

                    XmlSchemaSet schemas = new XmlSchemaSet();
                    settings.Schemas = schemas;


                    schemas.Add("http://www.portalfiscal.inf.br/nfe", caminhoDoSchema);
                    schemas.Add("http://www.portalfiscal.inf.br/nfe", nf.usr.filial.caminhoServidor + "modulos\\NotaFiscal\\pages\\schemas\\leiauteNFe_v" + versao + ".xsd");
                    //schemas.Add("http://www.portalfiscal.inf.br/nfe", nf.usr.filial.caminhoServidor + "modulos\\NotaFiscal\\pages\\schemas\\tiposBasico_v" + versao + ".xsd");


                    settings.ValidationEventHandler += new ValidationEventHandler(reader_ValidationEventHandler);

                    xmlReader = XmlReader.Create(cRotaArqXML, settings);

                    this.cErro = "";
                    try
                    {
                        while (xmlReader.Read())
                        {
                            // Console.WriteLine(xmlReader.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        this.cErro = ex.Message;
                    }

                    xmlReader.Close();
                }
                catch (Exception ex)
                {
                    if (xmlReader != null)
                        xmlReader.Close();

                    throw ex;
                }

                this.Retorno = 0;
                this.RetornoString = "";
                if (this.cErro != "")
                {
                    this.Retorno = 1;
                    this.RetornoString = "Início da validação...<br /> Versao = " + versao + " <br />";
                    this.RetornoString += "Arquivo XML: " + cRotaArqXML + "<br />";
                    this.RetornoString += "Arquivo SCHEMA: " + caminhoDoSchema + "<br /><br />";
                    this.RetornoString += this.cErro;
                    this.RetornoString += "<br />...Final da validação";
                    throw new Exception(this.RetornoString);
                }

            }
            else
            {
                if (lArqXML == false)
                {
                    Retorno = 2;
                    RetornoString = "Arquivo XML não foi encontrato";
                }
                else if (lArqXSD == false && temXSD)
                {
                    Retorno = 3;
                    RetornoString = "Arquivo XSD (schema) não foi encontrado em " + caminhoDoSchema;

                }
                throw new Exception(RetornoString);
            }
        }


        private X509Certificate2 ExtraiCertificado()
        {
            User usr = (nf.usr != null ? nf.usr : this.usr);
            string certificadoArquivo = usr.filial.certificado;
            string certificadoSenha = usr.filial.certificado_senha.Trim();
            try
            {
                X509Certificate2 cert = null;
                if (usr.filial.tipo_certificado.Equals("A1") || usr.filial.tipo_certificado.Equals(""))
                {
                    cert = new X509Certificate2(certificadoArquivo, certificadoSenha, X509KeyStorageFlags.MachineKeySet);
                }
                else
                {
                    X509Certificate2Collection lcerts;
                    X509Store lStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                    lStore.Open(OpenFlags.ReadOnly);
                    lcerts = lStore.Certificates;
                    String cnpj = nf.usr.filial.CNPJ;
                    foreach (X509Certificate2 certificado in lcerts)
                    {
                        String nome = certificado.GetName();
                        // certificado.PrivateKey = AsymmetricAlgorithm.Create(
                        if (nome.IndexOf(cnpj) > 0)
                        {
                            cert = certificado;
                            break;
                        }

                    }

                }
                if (cert != null)
                    return cert;
                else
                    throw new Exception("Não foi possível encontrar o certificado Digital");
            }
            catch (Exception err)
            {
                throw new Exception("Não foi possível processar a Assinatura Digital   Error:" + err.Message);
            }
        }

        public void Assinar(string arqXMLAssinar, string tagAssinatura, string tagAtributoId)
        {
            X509Certificate2 x509Cert = ExtraiCertificado();

            if (String.IsNullOrEmpty(tagAssinatura))
                return;

            StreamReader SR = null;

            try
            {
                //Abrir o arquivo XML a ser assinado e ler o seu conteúdo
                SR = File.OpenText(arqXMLAssinar);
                string xmlString = SR.ReadToEnd();
                SR.Close();
                SR = null;

                try
                {
                    // Create a new XML document.
                    XmlDocument doc = new XmlDocument();

                    // Format the document to ignore white spaces.
                    doc.PreserveWhitespace = false;

                    // Load the passed XML file using it’s name.
                    try
                    {
                        doc.LoadXml(xmlString);

                        if (doc.GetElementsByTagName(tagAssinatura).Count == 0)
                        {
                            throw new Exception("A tag de assinatura " + tagAssinatura.Trim() + " não existe no XML. (Código do Erro: 5)");
                        }
                        else if (doc.GetElementsByTagName(tagAtributoId).Count == 0)
                        {
                            throw new Exception("A tag de assinatura " + tagAtributoId.Trim() + " não existe no XML. (Código do Erro: 4)");
                        }
                        // Existe mais de uma tag a ser assinada
                        else
                        {
                            try
                            {
                                XmlDocument XMLDoc;

                                XmlNodeList lists = doc.GetElementsByTagName(tagAssinatura);
                                foreach (XmlNode nodes in lists)
                                {
                                    foreach (XmlNode childNodes in nodes.ChildNodes)
                                    {
                                        if (!childNodes.Name.Equals(tagAtributoId))
                                            continue;

                                        if (childNodes.NextSibling != null && childNodes.NextSibling.Name.Equals("Signature"))
                                            continue;

                                        // Create a reference to be signed
                                        Reference reference = new Reference();
                                        reference.Uri = "";

                                        // pega o uri que deve ser assinada                                       
                                        XmlElement childElemen = (XmlElement)childNodes;
                                        if (childElemen.GetAttributeNode("Id") != null)
                                        {
                                            reference.Uri = "#" + childElemen.GetAttributeNode("Id").Value;
                                        }
                                        else if (childElemen.GetAttributeNode("id") != null)
                                        {
                                            reference.Uri = "#" + childElemen.GetAttributeNode("id").Value;
                                        }
                                        /*
                                        XmlAttributeCollection _Uri = childElemen.GetElementsByTagName(tagAtributoId).Item(0).Attributes;

                                        if (_Uri.Count > 0)
                                            foreach (XmlAttribute _atributo in _Uri)
                                            {
                                                if (_atributo.Name == "Id" || _atributo.Name == "id")
                                                {
                                                    reference.Uri = "#" + _atributo.InnerText;
                                                }
                                            }
                                        */

                                        // Create a SignedXml object.
                                        SignedXml signedXml = new SignedXml(doc);

                                        // Add the key to the SignedXml document
                                        signedXml.SigningKey = x509Cert.PrivateKey;

                                        // Add an enveloped transformation to the reference.
                                        XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
                                        reference.AddTransform(env);

                                        XmlDsigC14NTransform c14 = new XmlDsigC14NTransform();
                                        reference.AddTransform(c14);

                                        // Add the reference to the SignedXml object.
                                        signedXml.AddReference(reference);

                                        // Create a new KeyInfo object
                                        KeyInfo keyInfo = new KeyInfo();

                                        // Load the certificate into a KeyInfoX509Data object
                                        // and add it to the KeyInfo object.
                                        keyInfo.AddClause(new KeyInfoX509Data(x509Cert));

                                        // Add the KeyInfo object to the SignedXml object.
                                        signedXml.KeyInfo = keyInfo;
                                        signedXml.ComputeSignature();

                                        // Get the XML representation of the signature and save
                                        // it to an XmlElement object.
                                        XmlElement xmlDigitalSignature = signedXml.GetXml();

                                        // Gravar o elemento no documento XML
                                        nodes.AppendChild(doc.ImportNode(xmlDigitalSignature, true));
                                    }
                                }

                                XMLDoc = new XmlDocument();
                                XMLDoc.PreserveWhitespace = false;
                                XMLDoc = doc;

                                // Atualizar a string do XML já assinada
                                string StringXMLAssinado = XMLDoc.OuterXml;

                                // Gravar o XML Assinado no HD
                                StreamWriter SW_2 = File.CreateText(arqXMLAssinar);
                                SW_2.Write(StringXMLAssinado);
                                SW_2.Close();
                            }
                            catch (Exception caught)
                            {
                                throw (caught);
                            }
                        }
                    }
                    catch (Exception caught)
                    {
                        throw (caught);
                    }
                }
                catch (Exception caught)
                {
                    throw (caught);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (SR != null)
                    SR.Close();
            }
        }


        public XmlDocument Assinar(XmlDocument doc, string tagAssinatura, string tagAtributoId)
        {
            X509Certificate2 x509Cert = ExtraiCertificado();

            if (String.IsNullOrEmpty(tagAssinatura))
                throw new Exception("Tag não informada");

            StreamReader SR = null;

            try
            {
                //Abrir o arquivo XML a ser assinado e ler o seu conteúdo


                try
                {

                    // Format the document to ignore white spaces.
                    doc.PreserveWhitespace = false;

                    // Load the passed XML file using it’s name.
                    try
                    {


                        if (doc.GetElementsByTagName(tagAssinatura).Count == 0)
                        {
                            throw new Exception("A tag de assinatura " + tagAssinatura.Trim() + " não existe no XML. (Código do Erro: 5)");
                        }
                        else if (doc.GetElementsByTagName(tagAtributoId).Count == 0)
                        {
                            throw new Exception("A tag de assinatura " + tagAtributoId.Trim() + " não existe no XML. (Código do Erro: 4)");
                        }
                        // Existe mais de uma tag a ser assinada
                        else
                        {
                            try
                            {


                                XmlNodeList lists = doc.GetElementsByTagName(tagAssinatura);
                                foreach (XmlNode nodes in lists)
                                {
                                    foreach (XmlNode childNodes in nodes.ChildNodes)
                                    {
                                        if (!childNodes.Name.Equals(tagAtributoId))
                                            continue;

                                        if (childNodes.NextSibling != null && childNodes.NextSibling.Name.Equals("Signature"))
                                            continue;

                                        // Create a reference to be signed
                                        Reference reference = new Reference();
                                        reference.Uri = "";

                                        // pega o uri que deve ser assinada                                       
                                        XmlElement childElemen = (XmlElement)childNodes;
                                        if (childElemen.GetAttributeNode("Id") != null)
                                        {
                                            reference.Uri = "#" + childElemen.GetAttributeNode("Id").Value;
                                        }
                                        else if (childElemen.GetAttributeNode("id") != null)
                                        {
                                            reference.Uri = "#" + childElemen.GetAttributeNode("id").Value;
                                        }
                                        /*
                                        XmlAttributeCollection _Uri = childElemen.GetElementsByTagName(tagAtributoId).Item(0).Attributes;

                                        if (_Uri.Count > 0)
                                            foreach (XmlAttribute _atributo in _Uri)
                                            {
                                                if (_atributo.Name == "Id" || _atributo.Name == "id")
                                                {
                                                    reference.Uri = "#" + _atributo.InnerText;
                                                }
                                            }
                                        */

                                        // Create a SignedXml object.
                                        SignedXml signedXml = new SignedXml(doc);

                                        // Add the key to the SignedXml document
                                        signedXml.SigningKey = x509Cert.PrivateKey;

                                        // Add an enveloped transformation to the reference.
                                        XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
                                        reference.AddTransform(env);

                                        XmlDsigC14NTransform c14 = new XmlDsigC14NTransform();
                                        reference.AddTransform(c14);

                                        // Add the reference to the SignedXml object.
                                        signedXml.AddReference(reference);

                                        // Create a new KeyInfo object
                                        KeyInfo keyInfo = new KeyInfo();

                                        // Load the certificate into a KeyInfoX509Data object
                                        // and add it to the KeyInfo object.
                                        keyInfo.AddClause(new KeyInfoX509Data(x509Cert));

                                        // Add the KeyInfo object to the SignedXml object.
                                        signedXml.KeyInfo = keyInfo;
                                        signedXml.ComputeSignature();

                                        // Get the XML representation of the signature and save
                                        // it to an XmlElement object.
                                        XmlElement xmlDigitalSignature = signedXml.GetXml();

                                        // Gravar o elemento no documento XML
                                        nodes.AppendChild(doc.ImportNode(xmlDigitalSignature, true));
                                    }
                                }
                                return doc;

                                // Atualizar a string do XML já assinada
                            }
                            catch (Exception caught)
                            {
                                throw (caught);
                            }
                        }
                    }
                    catch (Exception caught)
                    {
                        throw (caught);
                    }
                }
                catch (Exception caught)
                {
                    throw (caught);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (SR != null)
                    SR.Close();
            }
        }

        public void transmitir()
        {
            StreamReader SR = null;
            XmlDocument doc = null;
            StreamWriter SW_2 = null;
            try
            {

                String id = funChave();
                SR = File.OpenText(DIR_PADRAO + "/" + id + ".xml");
                string xmlString = SR.ReadToEnd();
                // Create a new XML document.
                doc = new XmlDocument();

                // Format the document to ignore white spaces.
                doc.PreserveWhitespace = false;

                // Load the passed XML file using it’s name.

                doc.LoadXml(xmlString);

                XmlNode xmlResc = null;
                if (!nf.usr.filial.tipo_certificado.Equals("A3"))
                {

                    if (tpAmb == 2)
                    {
                        if (versao.Equals("2.0"))
                        {

                            HomologacaoNfeRecepcao2.NfeRecepcao2 servNf = new HomologacaoNfeRecepcao2.NfeRecepcao2();
                            servNf.ClientCertificates.Add(ExtraiCertificado());
                            HomologacaoNfeRecepcao2.nfeCabecMsg cabRet = new HomologacaoNfeRecepcao2.nfeCabecMsg();

                            cabRet.versaoDados = versao;
                            cabRet.cUF = "35";
                            servNf.nfeCabecMsgValue = cabRet;

                            xmlResc = servNf.nfeRecepcaoLote2(doc);
                        }
                        else if (versao.Equals("3.10"))
                        {
                            Homologacao3NfeAutorizacao.NfeAutorizacao servNF = new Homologacao3NfeAutorizacao.NfeAutorizacao();
                            servNF.ClientCertificates.Add(ExtraiCertificado());
                            Homologacao3NfeAutorizacao.nfeCabecMsg cabRet = new Homologacao3NfeAutorizacao.nfeCabecMsg();
                            cabRet.versaoDados = versao;
                            cabRet.cUF = "35";
                            servNF.nfeCabecMsgValue = cabRet;

                            xmlResc = servNF.nfeAutorizacaoLote(doc);
                        }
                    }
                    else
                    {
                        if (versao.Equals("2.0"))
                        {

                            NfeRecepcao2.NfeRecepcao2 servNf = new NfeRecepcao2.NfeRecepcao2();
                            servNf.ClientCertificates.Add(ExtraiCertificado());
                            NfeRecepcao2.nfeCabecMsg cabRet = new NfeRecepcao2.nfeCabecMsg();

                            cabRet.versaoDados = versao;
                            cabRet.cUF = "35";
                            servNf.nfeCabecMsgValue = cabRet;

                            xmlResc = servNf.nfeRecepcaoLote2(doc);
                        }
                        else if (versao.Equals("3.10"))
                        {
                            Producao3NfAutorizacao.NfeAutorizacao servNF = new Producao3NfAutorizacao.NfeAutorizacao();
                            servNF.ClientCertificates.Add(ExtraiCertificado());
                            Producao3NfAutorizacao.nfeCabecMsg cabRet = new Producao3NfAutorizacao.nfeCabecMsg();
                            cabRet.versaoDados = versao;
                            cabRet.cUF = "35";
                            servNF.nfeCabecMsgValue = cabRet;

                            xmlResc = servNF.nfeAutorizacaoLote(doc);
                        }
                    }

                    string StringXMLResposta = xmlResc.OuterXml;


                    SW_2 = File.CreateText(nf.usr.filial.diretorio_exporta + "\\retorno\\" + id + "Resp-Rec.xml");
                    SW_2.Write(StringXMLResposta);

                    SW_2.Close();
                }
                else
                {

                    SW_2 = File.CreateText(nf.usr.filial.diretorio_exporta + "\\Envio\\" + id + "-nfe.xml");
                    SW_2.Write(xmlString);

                    SW_2.Close();

                }


            }
            catch (Exception err)
            {

                throw err;
            }
            finally
            {

                if (SR != null)
                    SR.Close();


                if (SW_2 != null)
                    SW_2.Close();

            }

        }



        /*
        private HomoNfeRecepcao2.nfeCabecMsg CriaCabecalho(string uf)
        {
            try
            {
                // Instanciando objeto nfeCabecMsg 
                HomoNfeRecepcao2.nfeCabecMsg cabecalho = new HomoNfeRecepcao2.nfeCabecMsg();

                // Preenchendo objeto
                cabecalho.cUF = (uf.ToUpper().Equals("SP")) ? "35" : string.Empty;
                cabecalho.versaoDados = versao;

                // Retornando objeto
                return cabecalho;
            }
            catch (Exception be)
            {
                throw new Exception("CriaCabecalho");
            }

        }

      */

        public void abortarTransmissaoA3()
        {
            if (nf.usr.filial.tipo_certificado.Equals("A3"))
            {
                String id = funChave();
                File.Delete(nf.usr.filial.diretorio_exporta + "\\Envio\\" + id + "-nfe.xml");
            }
        }

        public void SituacaoTransmicao()
        {
            XmlNode xmlRespo = null;
            if (!nf.usr.filial.tipo_certificado.Equals("A3"))
            {

                if (tpAmb == 2)
                {
                    if (versao.Equals("2.0"))
                    {

                        HomologacaoNfeRetRecepcao2.NfeRetRecepcao2 sw = new HomologacaoNfeRetRecepcao2.NfeRetRecepcao2();
                        sw.ClientCertificates.Add(ExtraiCertificado());
                        XmlDocument conXml = xmlConsulta();
                        HomologacaoNfeRetRecepcao2.nfeCabecMsg cabRet = new HomologacaoNfeRetRecepcao2.nfeCabecMsg();
                        cabRet.versaoDados = versao;
                        cabRet.cUF = "35";
                        sw.nfeCabecMsgValue = cabRet;
                        xmlRespo = sw.nfeRetRecepcao2(conXml);
                    }
                    else if (versao.Equals("3.10"))
                    {
                        Homologacao3NFeRetAutorizacao.NfeRetAutorizacao sw = new Homologacao3NFeRetAutorizacao.NfeRetAutorizacao();
                        sw.ClientCertificates.Add(ExtraiCertificado());
                        XmlDocument conXml = xmlConsulta();
                        Homologacao3NFeRetAutorizacao.nfeCabecMsg cabRet = new Homologacao3NFeRetAutorizacao.nfeCabecMsg();
                        cabRet.versaoDados = versao;
                        cabRet.cUF = "35";
                        sw.nfeCabecMsgValue = cabRet;
                        xmlRespo = sw.nfeRetAutorizacaoLote(conXml);


                    }
                }
                else
                {
                    if (versao.Equals("2.0"))
                    {
                        NfeRetRecepcao2.NfeRetRecepcao2 sw = new NfeRetRecepcao2.NfeRetRecepcao2();
                        sw.ClientCertificates.Add(ExtraiCertificado());
                        XmlDocument conXml = xmlConsulta();
                        NfeRetRecepcao2.nfeCabecMsg cabRet = new NfeRetRecepcao2.nfeCabecMsg();
                        cabRet.versaoDados = versao;
                        cabRet.cUF = "35";
                        sw.nfeCabecMsgValue = cabRet;
                        xmlRespo = sw.nfeRetRecepcao2(conXml);
                    }
                    else if (versao.Equals("3.10"))
                    {
                        Producao3NfeRetAutorizacao.NfeRetAutorizacao sw = new Producao3NfeRetAutorizacao.NfeRetAutorizacao();
                        sw.ClientCertificates.Add(ExtraiCertificado());
                        XmlDocument conXml = xmlConsulta();
                        Producao3NfeRetAutorizacao.nfeCabecMsg cabRet = new Producao3NfeRetAutorizacao.nfeCabecMsg();
                        cabRet.versaoDados = versao;
                        cabRet.cUF = "35";
                        sw.nfeCabecMsgValue = cabRet;
                        xmlRespo = sw.nfeRetAutorizacaoLote(conXml);
                    }

                }
                string StringXMLResp = xmlRespo.OuterXml;


                StreamWriter SW_2 = File.CreateText(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "Resp-Situacao.xml");
                SW_2.Write(StringXMLResp);
                SW_2.Close();
            }
        }

        public void CancelarNFE(String justificativa)
        {
            XmlNode xmlRespo = null;
            if (!nf.usr.filial.tipo_certificado.Equals("A3"))
            {
                if (tpAmb == 2)
                {
                    if (versao.Equals("2.0"))
                    {
                        HomologacaoRecepcaoEvento.RecepcaoEvento sw = new HomologacaoRecepcaoEvento.RecepcaoEvento();
                        sw.ClientCertificates.Add(ExtraiCertificado());
                        XmlDocument conXml = xmlCancelar(justificativa);
                        HomologacaoRecepcaoEvento.nfeCabecMsg cabRet = new HomologacaoRecepcaoEvento.nfeCabecMsg();
                        cabRet.versaoDados = "1.00";
                        cabRet.cUF = "35";
                        sw.nfeCabecMsgValue = cabRet;
                        xmlRespo = sw.nfeRecepcaoEvento(conXml);
                    }
                    else if (versao.Equals("3.10"))
                    {
                        Homologacao3RecepcaoEvento.RecepcaoEvento sw = new Homologacao3RecepcaoEvento.RecepcaoEvento();
                        sw.ClientCertificates.Add(ExtraiCertificado());
                        XmlDocument conXml = xmlCancelar(justificativa);
                        Homologacao3RecepcaoEvento.nfeCabecMsg cabRet = new Homologacao3RecepcaoEvento.nfeCabecMsg();
                        cabRet.versaoDados = "1.00";
                        cabRet.cUF = "35";
                        sw.nfeCabecMsgValue = cabRet;
                        xmlRespo = sw.nfeRecepcaoEvento(conXml);
                    }



                }
                else
                {
                    if (versao.Equals("2.0"))
                    {
                        RecepcaoEvento.RecepcaoEvento sw = new RecepcaoEvento.RecepcaoEvento();
                        sw.ClientCertificates.Add(ExtraiCertificado());
                        XmlDocument conXml = xmlCancelar(justificativa);
                        RecepcaoEvento.nfeCabecMsg cabRet = new RecepcaoEvento.nfeCabecMsg();
                        cabRet.versaoDados = "1.00";
                        cabRet.cUF = "35";
                        sw.nfeCabecMsgValue = cabRet;
                        xmlRespo = sw.nfeRecepcaoEvento(conXml);
                    }
                    else if (versao.Equals("3.10"))
                    {
                        Producao3RecepcaoEvento.RecepcaoEvento sw = new Producao3RecepcaoEvento.RecepcaoEvento();
                        sw.ClientCertificates.Add(ExtraiCertificado());
                        XmlDocument conXml = xmlCancelar(justificativa);
                        Producao3RecepcaoEvento.nfeCabecMsg cabRet = new Producao3RecepcaoEvento.nfeCabecMsg();
                        cabRet.versaoDados = "1.00";
                        cabRet.cUF = "35";
                        sw.nfeCabecMsgValue = cabRet;
                        xmlRespo = sw.nfeRecepcaoEvento(conXml);
                    }
                }
                string StringXMLResp = xmlRespo.OuterXml;


                StreamWriter SW_2 = File.CreateText(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "Resp-Cancelamento.xml");
                SW_2.Write(StringXMLResp);
                SW_2.Close();
            }
            else
            {
                StreamWriter SW_2 = File.CreateText(nf.usr.filial.diretorio_exporta + "/envio/" + funChave() + "-env-canc.xml");
                SW_2.Write(xmlCancelar(justificativa).OuterXml);
                SW_2.Close();


            }

        }

        public void CorrecaoNFE(String Correcao, String nCorrecao)
        {
            XmlNode xmlRespo = null;


            if (!nf.usr.filial.tipo_certificado.Equals("A3"))
            {
                if (tpAmb == 2)
                {
                    Homologacao3RecepcaoEvento.RecepcaoEvento sw = new Homologacao3RecepcaoEvento.RecepcaoEvento();
                    sw.ClientCertificates.Add(ExtraiCertificado());
                    XmlDocument conXml = xmlCorrecao(Correcao, nCorrecao);
                    Homologacao3RecepcaoEvento.nfeCabecMsg cabRet = new Homologacao3RecepcaoEvento.nfeCabecMsg();
                    cabRet.versaoDados = "1.00";
                    cabRet.cUF = "35";
                    sw.nfeCabecMsgValue = cabRet;
                    xmlRespo = sw.nfeRecepcaoEvento(conXml);



                }
                else
                {

                    Producao3RecepcaoEvento.RecepcaoEvento sw = new Producao3RecepcaoEvento.RecepcaoEvento();
                    sw.ClientCertificates.Add(ExtraiCertificado());
                    XmlDocument conXml = xmlCorrecao(Correcao, nCorrecao);
                    Producao3RecepcaoEvento.nfeCabecMsg cabRet = new Producao3RecepcaoEvento.nfeCabecMsg();
                    cabRet.versaoDados = "1.00";
                    cabRet.cUF = "35";
                    sw.nfeCabecMsgValue = cabRet;
                    xmlRespo = sw.nfeRecepcaoEvento(conXml);

                }
                string StringXMLResp = xmlRespo.OuterXml;


                StreamWriter SW_2 = File.CreateText(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "-" + nCorrecao.PadLeft(2, '0') + "Resp-Correcao.xml");
                SW_2.Write(StringXMLResp);
                SW_2.Close();
            }
            else
            {
                StreamWriter SW_2 = File.CreateText(nf.usr.filial.diretorio_exporta + "/envio/" + funChave() + "-" + nCorrecao.PadLeft(2, '0') + "-env-cce.xml");
                SW_2.Write(xmlCorrecao(Correcao, nCorrecao).OuterXml);
                SW_2.Close();


            }

        }

        private String numeroReciboEnvio()
        {
            StreamReader SR = null;
            String recibo = "";
            string xmlString = "";
            XmlDocument doc = null;

            try
            {


                if (nf.usr.filial.tipo_certificado.Equals("A3"))
                {
                    SR = File.OpenText(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "-num-lot.xml");
                    xmlString = SR.ReadToEnd();

                    doc = new XmlDocument();
                    doc.PreserveWhitespace = false;
                    doc.LoadXml(xmlString);
                    String numLote = doc.GetElementsByTagName("NumeroLoteGerado").Item(0).InnerText;

                    SR.Close();
                    SR = File.OpenText(nf.usr.filial.diretorio_exporta + "/retorno/" + numLote.PadLeft(15, '0') + "-rec.xml");


                }
                else
                {
                    SR = File.OpenText(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "Resp-Rec.xml");

                }

                xmlString = SR.ReadToEnd();

                doc = new XmlDocument();
                doc.PreserveWhitespace = false;
                doc.LoadXml(xmlString);
                recibo = doc.GetElementsByTagName("nRec").Item(0).InnerText;

                return recibo;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (SR != null)
                    SR.Close();

            }
        }

        public void limparErros()
        {
            String end = nf.usr.filial.diretorio_exporta + "/erro/";
            if (Directory.Exists(end))
            {
                string[] _files = Directory.GetFiles(end, "*", SearchOption.AllDirectories);

                foreach (string _file in _files)
                {
                    File.Delete(_file);
                }
            }
        }



        public String respostaEnvio()
        {

            String resposta = "";
            StreamReader SR = null;
            try
            {
                if (nf.usr.filial.tipo_certificado.Equals("A3"))
                {
                    try
                    {

                        SR = File.OpenText(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "-num-lot.xml");
                        string xmlString = SR.ReadToEnd();

                        XmlDocument doc = new XmlDocument();
                        doc.PreserveWhitespace = false;
                        doc.LoadXml(xmlString);
                        String numLote = doc.GetElementsByTagName("NumeroLoteGerado").Item(0).InnerText;

                        SR.Close();
                        // String caminho = nf.usr.filial.diretorio_exporta + "/retorno/" + numLote.PadLeft(1, '0') + "-rec.xml";
                        try
                        {
                            //Faz checagem neste ponto caso não tenha gerado o arquivo contendo o número do lote
                            if (File.Exists(nf.usr.filial.diretorio_exporta + "/retorno/" + numLote.PadLeft(15, '0') + "-rec.xml"))
                            {
                                SR = File.OpenText(nf.usr.filial.diretorio_exporta + "/retorno/" + numLote.PadLeft(15, '0') + "-rec.xml");
                            }
                            else
                            {
                                string[] lista_arquivos = Directory.GetFiles(nf.usr.filial.diretorio_exporta + "/retorno/", "???????????????-pro-rec.XML");
                                foreach (string item in lista_arquivos)
                                {
                                    StreamReader re = File.OpenText(item);
                                    string inputConteudo = re.ReadToEnd();
                                    if (inputConteudo.IndexOf(funChave()) > 1)
                                    {

                                        doc = new XmlDocument();
                                        doc.PreserveWhitespace = false;
                                        doc.LoadXml(inputConteudo);

                                        resposta = doc.GetElementsByTagName("xMotivo").Item(1).InnerText + "<br/>";
                                        if (resposta.IndexOf("Rejeição") >= 0)
                                        {
                                            re.Close();
                                            re.Dispose();
                                            File.Delete(item);
                                            throw new Exception(resposta);
                                        }
                                        else if (resposta.IndexOf("Autorizado") >= 0)
                                        {
                                            //Rotina que define se houve ou não autorização de uso direto sem validação.
                                            resposta = "Autorizado Uso" + "<br/>";
                                            resposta += "Recibo:" + doc.GetElementsByTagName("nRec").Item(0).InnerText + "<br/>";
                                            resposta += "Resposta :" + doc.GetElementsByTagName("xMotivo").Item(0).InnerText + "<br/>";
                                            resposta += "Protocolo :" + doc.GetElementsByTagName("nProt").Item(0).InnerText + " " + doc.GetElementsByTagName("dhRecbto").Item(0).InnerText;

                                            re.Close();
                                            re.Dispose();
                                            File.Delete(item);

                                            if (File.Exists(DIR_PADRAO + "\\" + nf.id + ".xml"))
                                            {
                                                File.Delete(DIR_PADRAO + "\\" + nf.id + ".xml");
                                            }

                                            return resposta;
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            re.Close();
                                            re.Dispose();
                                            File.Delete(item);
                                        }
                                        catch
                                        {

                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception)
                        {
                            SR = new StreamReader(nf.usr.filial.diretorio_exporta + "/retorno/" + numLote.PadLeft(15, '0') + "-rec.err", Encoding.Default);
                            //File.OpenText(nf.usr.filial.diretorio_exporta + "/retorno/" + numLote.PadLeft(15, '0') + "-rec.err");

                            String msgErro = SR.ReadToEnd().Replace("\r\n", "<br/>");
                            SR.Close();
                            File.Delete(nf.usr.filial.diretorio_exporta + "/erro/" + numLote.PadLeft(15, '0') + "-rec.err");
                            File.Move(nf.usr.filial.diretorio_exporta + "/retorno/" + numLote.PadLeft(15, '0') + "-rec.err", nf.usr.filial.diretorio_exporta + "/erro/" + numLote.PadLeft(15, '0') + "-rec.err");

                            throw new Exception("Erro Transmissao" + msgErro);

                        }
                        xmlString = SR.ReadToEnd();

                        doc = new XmlDocument();
                        doc.PreserveWhitespace = false;
                        doc.LoadXml(xmlString);


                        // nf.numeroProtocolo = doc.GetElementsByTagName("nRec").Item(0).InnerText;
                        //resposta = "Recibo:" + nf.numeroRecibo + "<br/>";
                        resposta = "Recibo:" + doc.GetElementsByTagName("nRec").Item(0).InnerText + "<br/>";
                        resposta += "Resposta :" + doc.GetElementsByTagName("xMotivo").Item(0).InnerText + "<br/>";

                        if (File.Exists(DIR_PADRAO + "\\" + nf.id + ".xml"))
                        {
                            File.Delete(DIR_PADRAO + "\\" + nf.id + ".xml");
                        }

                        return resposta;
                    }
                    catch (Exception err)
                    {
                        if (err.Message.IndexOf("Erro Transmissao") >= 0)
                            throw err;

                        if (SR != null)
                            SR.Close();

                        SR = new StreamReader(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "-nfe.err", Encoding.Default);
                        String msgErro = SR.ReadToEnd().Replace("\r\n", "<br/>");
                        SR.Close();
                        File.Delete(nf.usr.filial.diretorio_exporta + "/erro/" + funChave() + "-nfe.err");
                        File.Move(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "-nfe.err", nf.usr.filial.diretorio_exporta + "/erro/" + funChave() + "-nfe.err");

                        throw new Exception("Erro Transmissao" + msgErro);

                    }
                }
                else
                {
                    try
                    {


                        SR = File.OpenText(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "Resp-Rec.xml");
                        string xmlString = SR.ReadToEnd();

                        XmlDocument doc = new XmlDocument();
                        doc.PreserveWhitespace = false;
                        doc.LoadXml(xmlString);
                        resposta = "Recibo:" + doc.GetElementsByTagName("nRec").Item(0).InnerText + "<br/>";
                        resposta += "Resposta :" + doc.GetElementsByTagName("xMotivo").Item(0).InnerText + "<br/>";

                        return resposta;
                    }
                    catch (Exception err1)
                    {

                        throw new Exception("Erro Transmissao " + err1.Message);
                    }


                }
            }
            catch (Exception err2)
            {
                //int i = err.Message.IndexOf("Erro Transmissao");
                if (err2.Message.IndexOf("Erro Transmissao") >= 0)
                {
                    throw new Exception(err2.Message);
                }
                else
                {
                    if (resposta.IndexOf("Rejeição") >= 0)
                    {
                        throw new Exception(resposta);
                    }
                    return "Sem Resposta";
                }
            }
            finally
            {
                if (SR != null)
                    SR.Close();

            }

        }

        public String respostaCancelamento()
        {
            try
            {


                String resposta = "";
                XPathDocument doc = null;
                if (!nf.usr.filial.tipo_certificado.Equals("A3"))
                {
                    doc = new XPathDocument(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "Resp-Cancelamento.xml");
                }
                else
                {
                    System.Threading.Thread.Sleep(5000);
                    doc = new XPathDocument(nf.usr.filial.diretorio_exporta + "/Enviado/Autorizados/" + nf.Data.ToString("yyyyMM") + "/" + funChave() + "_110111_01-procEventoNFe.xml");
                }
                //doc.PreserveWhitespace = false;
                //doc.LoadXml(xmlString);


                XPathNavigator rs = doc.CreateNavigator();
                rs.MoveToRoot();
                rs.MoveToFirstChild();
                rs.MoveToFirstChild();
                bool prot = moverNo(rs, "retEvento");

                if (prot)
                {
                    String status = "";
                    rs.MoveToFirstChild();
                    rs.MoveToFirstChild();

                    moverNo(rs, "cStat");
                    status = rs.Value;
                    resposta = "Status:" + rs.Value + "<br/>";


                    moverNo(rs, "xMotivo");
                    resposta += "Resposta :" + rs.Value + "<br/>";


                    moverNo(rs, "chNFe");
                    resposta += "ID :" + rs.Value + "<br/>";

                    //moverNo(rs, "xEvento");
                    //resposta += "Situação :" + rs.Value + "<br/>";

                    moverNo(rs, "dhRegEvento");
                    resposta += "Data :" + DateTime.Parse(rs.Value).ToString() + "<br/>";

                    moverNo(rs, "nProt");
                    resposta += "Protocolo :" + rs.Value + "<br/>";


                    if (!status.Equals("135") && !status.Equals("155"))
                    {
                        throw new Exception("Erro-Cancelar:" + resposta);
                    }


                }
                else
                {
                    rs.MoveToRoot();
                    rs.MoveToFirstChild();

                    moverNo(rs, "cStat");
                    resposta = "Status:" + rs.Value + "<br/>";

                    moverNo(rs, "xMotivo");
                    resposta += "Resposta :" + rs.Value + "<br/>";
                    throw new Exception("Erro-Cancelar:" + resposta);
                }

                return resposta;
            }
            catch (Exception err)
            {
                if (err.Message.IndexOf("Erro-Cancelar:") >= 0)
                    throw err;
                StreamReader SR = null;
                try
                {
                    SR = new StreamReader(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "-ret-env-canc.err", Encoding.Default);
                }
                catch (Exception)
                {
                    if (SR != null)
                        SR.Close();

                    SR = new StreamReader(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "-ped-can.err", Encoding.Default);

                }

                String erro = SR.ReadToEnd().Replace("\r\n", "<br/>");
                if (SR != null)
                    SR.Close();
                throw new Exception("Erro-Cancelar" + erro);

            }
        }

        public String respostaCartaCorrecao(String ncorrecao, String correcao)
        {
            try
            {

                nf.nota_referencia = ncorrecao;
                String resposta = "";
                XPathDocument doc = null;
                if (!nf.usr.filial.tipo_certificado.Equals("A3"))
                {
                    doc = new XPathDocument(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "-" + ncorrecao.PadLeft(2, '0') + "Resp-Correcao.xml");

                }
                else
                {
                    System.Threading.Thread.Sleep(5000);
                    doc = new XPathDocument(nf.usr.filial.diretorio_exporta + "/Enviado/Autorizados/" + nf.Data.ToString("yyyyMM") + "/" + funChave() + "_" + ncorrecao.PadLeft(2, '0') + "-procEventoNFe.xml");
                }
                //doc.PreserveWhitespace = false;
                //doc.LoadXml(xmlString);


                XPathNavigator rs = doc.CreateNavigator();
                rs.MoveToRoot();
                rs.MoveToFirstChild();
                rs.MoveToFirstChild();
                bool prot = moverNo(rs, "retEvento");

                String retEvento = "<retEvento xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"" + versao + " \">" + rs.InnerXml + "</retEvento>";

                if (prot)
                {
                    String status = "";
                    rs.MoveToFirstChild();
                    rs.MoveToFirstChild();

                    moverNo(rs, "cStat");
                    status = rs.Value;
                    resposta = "Status:" + rs.Value + "<br/>";


                    moverNo(rs, "xMotivo");
                    resposta += "Resposta :" + rs.Value + "<br/>";


                    moverNo(rs, "chNFe");
                    resposta += "ID :" + rs.Value + "<br/>";

                    //moverNo(rs, "xEvento");
                    //resposta += "Situação :" + rs.Value + "<br/>";

                    moverNo(rs, "dhRegEvento");
                    resposta += "Data :" + DateTime.Parse(rs.Value).ToString() + "<br/>";

                    moverNo(rs, "nProt");
                    resposta += "Protocolo :" + rs.Value + "<br/>";


                    if (!status.Equals("135") && !status.Equals("155") && !status.Equals("128"))
                    {
                        throw new Exception("Erro-Correcao:" + resposta);
                    }
                    else
                    {
                        if (!nf.usr.filial.tipo_certificado.Equals("A3"))
                        {
                            renomearXml(retEvento, true, ncorrecao);
                        }

                    }



                }
                else
                {
                    rs.MoveToRoot();
                    rs.MoveToFirstChild();

                    moverNo(rs, "cStat");
                    resposta = "Status:" + rs.Value + "<br/>";

                    moverNo(rs, "xMotivo");
                    resposta += "Resposta :" + rs.Value + "<br/>";
                    throw new Exception("Erro-Correcao:" + resposta);
                }

                nf.salvarCorrecao(resposta.Substring(resposta.IndexOf("Protocolo :") + 11, 15), correcao);


                return resposta;
            }
            catch (Exception err)
            {
                if (err.Message.IndexOf("Erro-Correcao:") >= 0)
                    throw err;
                StreamReader SR = null;
                try
                {
                    SR = new StreamReader(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "-" + ncorrecao.PadLeft(2, '0') + "-ret-env-cce.xml", Encoding.Default);
                }
                catch (Exception)
                {
                    if (SR != null)
                        SR.Close();

                    SR = new StreamReader(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "-" + ncorrecao.PadLeft(2, '0') + "-ret-env-cce.err", Encoding.Default);

                }

                String erro = SR.ReadToEnd().Replace("\r\n", "<br/>");
                if (SR != null)
                    SR.Close();
                throw new Exception("Erro-Correcao" + erro);

            }
        }

        public String respostaConsulta()
        {
            String resposta = "";
            String nProtocolo = "";
            String dtProtocolo = "";

            String numRecibo = numeroReciboEnvio();
            XPathDocument doc = null;
            if (nf.usr.filial.tipo_certificado.Equals("A3"))
            {
                int contVerifica = 0;
                while (contVerifica < 10) //antes era 50. 2022.09.02
                {
                    try
                    {
                        System.Threading.Thread.Sleep(1000);
                        doc = new XPathDocument(nf.usr.filial.diretorio_exporta + "/Retorno/" + numRecibo + "-pro-rec.xml");
                        break;
                    }
                    catch (Exception)
                    {
                        contVerifica++;

                    }
                }
            }
            else
            {
                doc = new XPathDocument(nf.usr.filial.diretorio_exporta + "/Retorno/" + funChave() + "Resp-Situacao.xml");
            }
            //doc.PreserveWhitespace = false;
            //doc.LoadXml(xmlString);


            XPathNavigator rs = doc.CreateNavigator();
            rs.MoveToRoot();
            rs.MoveToFirstChild();
            rs.MoveToFirstChild();
            bool prot = moverNo(rs, "protNFe");
            String protNfe = "<protNFe versao=\"" + versao + "\">" + rs.InnerXml + "</protNFe>";

            if (prot)
            {
                String status = "";
                rs.MoveToFirstChild();
                rs.MoveToFirstChild();

                moverNo(rs, "dhRecbto");
                DateTime dtRec;
                DateTime.TryParse(rs.Value, out dtRec);
                dtProtocolo = dtRec.ToString("dd/MM/yyyy HH:mm:ss");
                dtAutorizacao = dtRec;

                moverNo(rs, "nProt");
                nProtocolo = rs.Value;

                nProtocolo += " " + dtProtocolo;


                moverNo(rs, "cStat");
                status = rs.Value;
                resposta = "Status:" + rs.Value + "<br/>";

                moverNo(rs, "xMotivo");
                resposta += "Resposta :" + rs.Value + "<br/>";

                resposta += "ID=" + funChave();







                resposta += "Protocolo :" + nProtocolo + "<br/>";


                if (!status.Equals("100"))
                {
                    //File.Delete(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "-num-lot.xml");
                    File.Move(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "-num-lot.xml", nf.usr.filial.diretorio_exporta + "/Erro/" + funChave() + "-num-lot.xml");

                    throw new Exception(resposta);
                }
                else
                {
                    nf.numeroProtocolo = nProtocolo;
                    if (!nf.usr.filial.tipo_certificado.Equals("A3"))
                    {
                        renomearXml(protNfe, false, "");
                    }

                }

            }
            else
            {
                rs.MoveToRoot();
                rs.MoveToFirstChild();

                moverNo(rs, "cStat");
                resposta = "Status:" + rs.Value + "<br/>";

                moverNo(rs, "xMotivo");
                resposta += "Resposta :" + rs.Value + "<br/>";
                throw new Exception(resposta);
            }

            return resposta;

        }

        public String situacaoNotafiscal()
        {
            String strResposta = "";
            if (!nf.usr.filial.tipo_certificado.Equals("A3"))
            {

            }
            else
            {
                StreamWriter SW_2 = File.CreateText(nf.usr.filial.diretorio_exporta + "/envio/" + funChave() + "-ped-sit.xml");
                SW_2.Write(xmlSituacaoNota().OuterXml);
                SW_2.Close();

            }
            StreamReader SR = null;
            XPathNavigator rs = null;

            bool espRetorn = true;
            int cTentativa = 0;
            while (espRetorn)
            {
                System.Threading.Thread.Sleep(5000);

                XPathDocument doc = null;

                try
                {
                    doc = new XPathDocument(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "-sit.xml");
                    rs = doc.CreateNavigator();
                    espRetorn = false;
                }
                catch (Exception)
                {
                    try
                    {

                        System.Threading.Thread.Sleep(5000);
                        SR = new StreamReader(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "-sit.err", Encoding.Default);
                        String error = SR.ReadToEnd();
                        if (SR != null)
                            SR.Close();
                        espRetorn = false;
                        throw new Exception("Erro:" + error);
                    }
                    catch (Exception err)
                    {
                        if (err.Message.Contains("Erro:"))
                        {
                            espRetorn = false;
                            throw err;
                        }

                    }
                }
                if (cTentativa > 5)
                {
                    espRetorn = false;
                    throw new Exception("Sem Resposta");
                }
                cTentativa++;
            }

            rs.MoveToRoot();
            rs.MoveToFirstChild();
            rs.MoveToFirstChild();
            String strDt = "";
            String nProtocolo = "";
            String strStatus = "";
            moverNo(rs, "cStat");
            strStatus = rs.Value;
            strResposta = "Status:" + rs.Value + "<br/>";

            if (!strStatus.Equals("217"))
            {
                moverNo(rs, "protNFe");
                rs.MoveToFirstChild();
                rs.MoveToFirstChild();

                moverNo(rs, "dhRecbto");
                strDt = rs.Value;


                moverNo(rs, "nProt");
                nProtocolo = rs.Value;

            }

            moverNo(rs, "xMotivo");
            strResposta += "Resposta :" + rs.Value + "<br/>";

            if (!nProtocolo.Equals(""))
            {
                strResposta += "Protocolo:" + nProtocolo + " " + strDt;
            }

            dtAutorizacao = Funcoes.dtTry(strDt);

            return strResposta;



        }


        private XmlDocument xmlSituacaoNota()
        {
            XmlDocument xml = new XmlDocument();
            StringBuilder xmlConteudo = new StringBuilder();



            xmlConteudo.Append("<?xml version=\"1.0\"?>");
            xmlConteudo.Append("<consSitNFe xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao= \"4.00\">");
            xmlConteudo.Append(funGrava("tpAmb", tpAmb.ToString()));
            xmlConteudo.Append(funGrava("xServ", "CONSULTAR"));
            xmlConteudo.Append(funGrava("chNFe", nf.id));
            xmlConteudo.Append("</consSitNFe>");
            xml.LoadXml(xmlConteudo.ToString());

            if (!nf.usr.filial.tipo_certificado.Equals("A3"))
            {
                xml = Assinar(xml, "consSitNFe", "chNFe");
            }


            return xml;

        }

        public void renomearXml(String protNfe, bool correcao, string nCorrecao)
        {
            if (!correcao)
            {
                StreamReader SR = File.OpenText(nf.usr.filial.diretorio_exporta + "/" + funChave() + ".xml");
                string xmlString = SR.ReadToEnd();
                xmlString = xmlString.Substring(0, xmlString.IndexOf("<idLote>")) + xmlString.Substring(xmlString.IndexOf("</idLote>") + 9);

                xmlString = (xmlString.Replace("</enviNFe>", "") + protNfe + "</nfeProc>").Replace("enviNFe", "nfeProc").Replace("<indSinc>0</indSinc>", "");

                SR.Close();

                StreamWriter ArqXml = new StreamWriter(nf.usr.filial.diretorio_exporta + "/" + funChave() + ".xml", false, Encoding.ASCII);
                ArqXml.Write(xmlString);
                ArqXml.Close();

            }
            else
            {
                //nf.usr.filial.diretorio_exporta + "/" + funChave() + "-"+nCorrecao.PadLeft(2, '0')+"-correcao.xml"
                StreamReader SR = File.OpenText(nf.usr.filial.diretorio_exporta + "/" + funChave() + "-" + nCorrecao.PadLeft(2, '0') + "-correcao.xml");
                string xmlString = SR.ReadToEnd();
                xmlString = xmlString.Substring(0, xmlString.IndexOf("<idLote>")) + xmlString.Substring(xmlString.IndexOf("</idLote>") + 9);

                xmlString = (xmlString.Replace("</envEvento>", "") + protNfe + "</procEventoNFe>").Replace("envEvento", "procEventoNFe");

                SR.Close();

                StreamWriter ArqXml = new StreamWriter(nf.usr.filial.diretorio_exporta + "/" + funChave() + "-" + nCorrecao.PadLeft(2, '0') + "-correcao.xml", false, Encoding.ASCII);
                ArqXml.Write(xmlString);
                ArqXml.Close();
            }
        }

        private bool moverNo(XPathNavigator rs, String noxml)
        {
            while (rs.MoveToNext())
            {
                if (rs.Name.Equals(noxml))
                    return true;
            }
            return false;
        }

        private XmlDocument xmlConsulta()
        {
            XmlDocument xml = new XmlDocument();
            StringBuilder xmlConteudo = new StringBuilder();
            xmlConteudo.Append("<?xml version=\"1.0\"?>");
            xmlConteudo.Append("<consReciNFe xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao= \"" + versao + "\">");
            if (tpAmb == 2)
            {
                xmlConteudo.Append("<tpAmb>2</tpAmb>");
            }
            else
            {
                xmlConteudo.Append("<tpAmb>1</tpAmb>");
            }
            xmlConteudo.Append("<nRec>" + numeroReciboEnvio() + "</nRec>");

            xmlConteudo.Append("</consReciNFe>");
            xml.LoadXml(xmlConteudo.ToString());
            /*
            StreamWriter SW_2 = File.CreateText(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "consulta.xml");
            SW_2.Write(xmlConteudo.ToString());
            SW_2.Close();
             */
            return xml;

        }
        private XmlDocument xmlCancelar(String justificativa)
        {
            XmlDocument xml = new XmlDocument();
            StringBuilder xmlConteudo = new StringBuilder();



            xmlConteudo.Append("<?xml version=\"1.0\"?>");
            xmlConteudo.Append("<envEvento xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao= \"1.00\">");
            xmlConteudo.Append("<idLote>" + DateTime.Now.ToString("yyyyMMdd") + "</idLote>");
            xmlConteudo.Append("<evento xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao= \"1.00\">");
            xmlConteudo.Append("<infEvento Id=\"ID110111" + nf.id + "01\">");
            xmlConteudo.Append(funGrava("cOrgao", "35"));
            xmlConteudo.Append(funGrava("tpAmb", tpAmb.ToString()));
            xmlConteudo.Append(funGrava("CNPJ", nf.usr.filial.CNPJ.Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim()));
            xmlConteudo.Append(funGrava("chNFe", nf.id));
            xmlConteudo.Append(funGrava("dhEvento", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz")));
            xmlConteudo.Append(funGrava("tpEvento", "110111"));
            xmlConteudo.Append(funGrava("nSeqEvento", "1"));
            xmlConteudo.Append(funGrava("verEvento", "1.00"));
            xmlConteudo.Append("<detEvento versao=\"1.00\">");
            xmlConteudo.Append(funGrava("descEvento", "Cancelamento"));
            xmlConteudo.Append(funGrava("nProt", numeroProtocolo()));
            xmlConteudo.Append(funGrava("xJust", justificativa.Trim()));
            xmlConteudo.Append("</detEvento>");
            xmlConteudo.Append("</infEvento>");
            xmlConteudo.Append("</evento>");

            xmlConteudo.Append("</envEvento>");
            xml.LoadXml(xmlConteudo.ToString());
            if (!nf.usr.filial.tipo_certificado.Equals("A3"))
            {
                xml = Assinar(xml, "evento", "infEvento");
            }

            /*
        StreamWriter SW_2 = null;
        if (!nf.usr.filial.tipo_certificado.Equals("A3"))
        {
            SW_2 = File.CreateText(nf.usr.filial.diretorio_exporta + "/retorno/" + funChave() + "EnviarTeste-Cancelamento.xml");
        }
        else
        {
            SW_2 = File.CreateText(nf.usr.filial.diretorio_exporta + "/envio/" + funChave() + "-ped-can.xml");
        }
            SW_2.Write(xml.OuterXml);
        SW_2.Close();
        */

            return xml;

        }

        private XmlDocument xmlCorrecao(String correcao, String nCorrecao)
        {
            XmlDocument xml = new XmlDocument();
            StringBuilder xmlConteudo = new StringBuilder();

            xmlConteudo.Append("<?xml version=\"1.0\"?>");
            xmlConteudo.Append("<envEvento  versao= \"1.00\" xmlns=\"http://www.portalfiscal.inf.br/nfe\">");
            xmlConteudo.Append("<idLote>000000000000001</idLote>");
            xmlConteudo.Append("<evento versao= \"1.00\" xmlns=\"http://www.portalfiscal.inf.br/nfe\" >");
            xmlConteudo.Append("<infEvento Id=\"ID110110" + nf.id + nCorrecao.PadLeft(2, '0') + "\">");
            xmlConteudo.Append(funGrava("cOrgao", "35"));
            xmlConteudo.Append(funGrava("tpAmb", tpAmb.ToString()));
            xmlConteudo.Append(funGrava("CNPJ", nf.usr.filial.CNPJ.Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim()));
            xmlConteudo.Append(funGrava("chNFe", nf.id));
            xmlConteudo.Append(funGrava("dhEvento", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz")));
            xmlConteudo.Append(funGrava("tpEvento", "110110"));
            xmlConteudo.Append(funGrava("nSeqEvento", nCorrecao));
            xmlConteudo.Append(funGrava("verEvento", "1.00"));
            xmlConteudo.Append("<detEvento versao=\"1.00\">");
            xmlConteudo.Append(funGrava("descEvento", "Carta de Correcao"));
            xmlConteudo.Append(funGrava("xCorrecao", Funcoes.RemoverAcentos(correcao.Trim().Replace("\n", ";"))));
            xmlConteudo.Append(funGrava("xCondUso", "A Carta de Correcao e disciplinada pelo paragrafo 1o-A do art. 7o do Convenio S/N, de 15 de dezembro de 1970 e pode ser utilizada para regularizacao de erro ocorrido na emissao de documento fiscal, desde que o erro nao esteja relacionado com: I - as variaveis que determinam o valor do imposto tais como: base de calculo, aliquota, diferenca de preco, quantidade, valor da operacao ou da prestacao; II - a correcao de dados cadastrais que implique mudanca do remetente ou do destinatario; III - a data de emissao ou de saida."));
            xmlConteudo.Append("</detEvento>");
            xmlConteudo.Append("</infEvento>");
            xmlConteudo.Append("</evento>");

            xmlConteudo.Append("</envEvento>");
            xml.LoadXml(xmlConteudo.ToString());
            if (!nf.usr.filial.tipo_certificado.Equals("A3"))
            {
                xml = Assinar(xml, "evento", "infEvento");
            }

            StreamWriter ArqXml = new StreamWriter(nf.usr.filial.diretorio_exporta + "/" + funChave() + "-" + nCorrecao.PadLeft(2, '0') + "-correcao.xml", false, Encoding.ASCII);
            ArqXml.Write(xml.InnerXml);
            ArqXml.Close();

            return xml;

        }

        private String numeroProtocolo()
        {

            StreamReader SR = null;
            if (!nf.usr.filial.tipo_certificado.Equals("A3"))
            {
                SR = File.OpenText(nf.usr.filial.diretorio_exporta + "/" + funChave() + ".xml");
            }
            else
            {
                SR = File.OpenText(nf.usr.filial.diretorio_exporta + "/Enviado/Autorizados/" + nf.Data.ToString("yyyyMM") + "/" + funChave() + "-procNFe.xml");
            }

            string xmlString = SR.ReadToEnd();

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            doc.LoadXml(xmlString);
            String Protocolo = doc.GetElementsByTagName("nProt").Item(0).InnerText;
            if (SR != null)
                SR.Close();

            return Protocolo;
        }

        private XmlDocument xmlInutilizar(String id, String strcUf, String strCnpj, String serie, String nInicio, String nFim, String justificativa, User usr)
        {
            XmlDocument xml = new XmlDocument();
            StringBuilder xmlConteudo = new StringBuilder();
            id = "ID" + id;

            xmlConteudo.Append("<?xml version=\"1.0\"?>");
            xmlConteudo.Append("<inutNFe  xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"4.00\">");
            xmlConteudo.Append("<infInut Id=\"" + id + "\">");
            xmlConteudo.Append(funGrava("tpAmb", (usr.filial.producaoNfe ? "1" : "2")));
            xmlConteudo.Append(funGrava("xServ", "INUTILIZAR"));
            xmlConteudo.Append(funGrava("cUF", strcUf));
            xmlConteudo.Append(funGrava("ano", DateTime.Now.ToString("yy")));
            xmlConteudo.Append(funGrava("CNPJ", strCnpj));
            xmlConteudo.Append(funGrava("mod", "55"));
            xmlConteudo.Append(funGrava("serie", serie));
            xmlConteudo.Append(funGrava("nNFIni", nInicio.Trim()));
            xmlConteudo.Append(funGrava("nNFFin", nFim.Trim()));
            xmlConteudo.Append(funGrava("xJust", Funcoes.RemoverAcentos(justificativa.Trim())));
            xmlConteudo.Append("</infInut>");
            xmlConteudo.Append("</inutNFe >");
            xml.LoadXml(xmlConteudo.ToString());
            if (!usr.filial.tipo_certificado.Equals("A3"))
            {
                xml = Assinar(xml, "inutNFe", "infInut");
            }


            return xml;

        }


        public void inutilizarNumero(String serie, String nInicio, String nFim, String justificativa, User usr)
        {
            XmlNode xmlRespo = null;
            String strcUf = cUF;
            String strCnpj = usr.filial.CNPJ.Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim();
            String id = strcUf +
                        DateTime.Now.ToString("yy") +
                        strCnpj +
                        "55" +
                        serie.Trim().PadLeft(3, '0') +
                        nInicio.Trim().PadLeft(9, '0') +
                        nFim.Trim().PadLeft(9, '0')
                        ;

            if (!usr.filial.tipo_certificado.Equals("A3"))
            {
                if (tpAmb == 2)
                {
                    homologacao3NfeInutilizacao.NfeInutilizacao2 sw = new homologacao3NfeInutilizacao.NfeInutilizacao2();
                    sw.ClientCertificates.Add(ExtraiCertificado());
                    XmlDocument conXml = xmlInutilizar(id, strcUf, strCnpj, serie, nInicio, nFim, justificativa, usr);
                    homologacao3NfeInutilizacao.nfeCabecMsg cabRet = new homologacao3NfeInutilizacao.nfeCabecMsg();
                    cabRet.versaoDados = "4.00";
                    cabRet.cUF = "35";
                    sw.nfeCabecMsgValue = cabRet;
                    xmlRespo = sw.nfeInutilizacaoNF2(conXml);
                }
                else
                {
                    Producao3NfeInutilizacao.NfeInutilizacao2 sw = new Producao3NfeInutilizacao.NfeInutilizacao2();
                    sw.ClientCertificates.Add(ExtraiCertificado());
                    XmlDocument conXml = xmlInutilizar(id, strcUf, strCnpj, serie, nInicio, nFim, justificativa, usr);
                    Producao3NfeInutilizacao.nfeCabecMsg cabRet = new Producao3NfeInutilizacao.nfeCabecMsg();
                    cabRet.versaoDados = "4.00";
                    cabRet.cUF = "35";
                    sw.nfeCabecMsgValue = cabRet;
                    xmlRespo = sw.nfeInutilizacaoNF2(conXml);
                }

                string StringXMLResp = xmlRespo.OuterXml;


                StreamWriter SW_2 = File.CreateText(nf.usr.filial.diretorio_exporta + "/retorno/" + serie + nInicio + nFim + "Resp-Inutilizacao.xml");
                SW_2.Write(StringXMLResp);
                SW_2.Close();

            }
            else
            {
                StreamWriter SW_2 = File.CreateText(usr.filial.diretorio_exporta + "/envio/" + id + "-ped-inu.xml");
                SW_2.Write(xmlInutilizar(id, strcUf, strCnpj, serie, nInicio, nFim, justificativa, usr).OuterXml);
                SW_2.Close();
            }

        }
        public String respostaInutilizacao(String serie, String nInicio, String nFim, DateTime data, bool encerra)
        {
            String strcUf = cUF;
            String strCnpj = usr.filial.CNPJ.Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim();
            String id = strcUf +
                   DateTime.Now.ToString("yy") +
                   strCnpj +
                   "55" +
                   serie.Trim().PadLeft(3, '0') +
                   nInicio.Trim().PadLeft(9, '0') +
                   nFim.Trim().PadLeft(9, '0')
                   ;
            try
            {


                String resposta = "";
                XPathDocument doc = null;
                if (!usr.filial.tipo_certificado.Equals("A3"))
                {
                    doc = new XPathDocument(usr.filial.diretorio_exporta + "/retorno/" + serie + nInicio + nFim + "Resp-Inutilizacao.xml");
                }
                else
                {
                    System.Threading.Thread.Sleep(5000);
                    doc = new XPathDocument(usr.filial.diretorio_exporta + "/retorno/" + id + "-inu.xml");
                }
                //doc.PreserveWhitespace = false;
                //doc.LoadXml(xmlString);


                XPathNavigator rs = doc.CreateNavigator();
                rs.MoveToRoot();
                rs.MoveToFirstChild();
                rs.MoveToFirstChild();

                bool prot = (rs.Name.Equals("infInut"));

                if (prot)
                {
                    String status = "";
                    rs.MoveToFirstChild();

                    moverNo(rs, "cStat");
                    status = rs.Value;
                    resposta = "Status:" + rs.Value + "<br/>";


                    moverNo(rs, "xMotivo");
                    resposta += "Resposta :" + rs.Value + "<br/>";


                    moverNo(rs, "dhRecbto");
                    resposta += "Data :" + DateTime.Parse(rs.Value).ToString() + "<br/>";

                    moverNo(rs, "nProt");
                    resposta += "Protocolo :" + rs.Value + "<br/>";


                    if (!status.Equals("102"))
                    {
                        throw new Exception("Erro-Inutilizar:" + resposta);
                    }


                }
                else
                {
                    rs.MoveToRoot();
                    rs.MoveToFirstChild();

                    moverNo(rs, "cStat");
                    resposta = "Status:" + rs.Value + "<br/>";

                    moverNo(rs, "xMotivo");
                    resposta += "Resposta :" + rs.Value + "<br/>";
                    throw new Exception("Erro-Inutilizar:" + resposta);
                }

                return resposta;
            }
            catch (Exception err)
            {
                if (err.Message.IndexOf("Erro-Inutilizar:") >= 0)
                    throw err;

                StreamReader SR = null;
                try
                {

                    try
                    {


                        try
                        {

                            SR = new StreamReader(usr.filial.diretorio_exporta + "/retorno/" + id + "-inu.xml", Encoding.Default);
                        }
                        catch (Exception)
                        {
                            try
                            {

                                SR = null;
                                SR = new StreamReader(usr.filial.diretorio_exporta + "/retorno/" + id + "-inu.err", Encoding.Default);

                            }
                            catch (Exception)
                            {

                                SR = null;
                                SR = new StreamReader(usr.filial.diretorio_exporta + "/retorno/" + id + "-ped-inu.err", Encoding.Default);

                            }
                        }

                        String erro = SR.ReadToEnd().Replace("\r\n", "<br/>");
                        throw new Exception("Erro-Inutilizar:" + erro);
                    }
                    catch (Exception erro)
                    {
                        if (erro.Message.IndexOf("Erro-Inutilizar:") >= 0)
                            throw erro;
                        if (encerra)
                        {
                            File.Delete(usr.filial.diretorio_exporta + "/envio/" + id + "-ped-inu.xml");
                        }
                        throw new Exception("Sem Resposta do Servidor!");
                    }
                }
                catch (Exception err2)
                {
                    throw err2;
                }
                finally
                {
                    if (SR != null)
                        SR.Close();

                }
            }
        }





        public void AtualiarXmlsManifesto()
        {


            try
            {


                if (!Directory.Exists(usr.filial.diretorio_exporta + "/retorno/dfe"))
                {
                    Directory.CreateDirectory(usr.filial.diretorio_exporta + "/retorno/dfe");
                }

                DirectoryInfo Dir = new DirectoryInfo(usr.filial.diretorio_exporta + "/retorno/dfe");

                FileInfo[] Files = Dir.GetFiles("*-nfe.xml");

                foreach (FileInfo File in Files)
                {
                    XmlDocument docXML = new XmlDocument();
                    XmlTextReader xmlR = new XmlTextReader(File.FullName) { Namespaces = false };
                    docXML.Load(xmlR);
                    NfManifestoDAO item = new NfManifestoDAO(usr);
                    item.NSU = File.FullName.Replace("-nfe.xml", "").Substring(File.FullName.IndexOf('-') + 1);
                    item.Chave = vlrNo(docXML, "/resNFe/chNFe");
                    item.CNPJ = vlrNo(docXML, "/resNFe/CNPJ");
                    item.RazaoSocial = vlrNo(docXML, "/resNFe/xNome");
                    item.vNF = Funcoes.decTry(vlrNo(docXML, "/resNFe/vNF").Replace(".", ","));
                    item.Emissao = Funcoes.dtTry(vlrNo(docXML, "/resNFe/dhEmi"));
                    item.NfeXml = DowLoadXML(item.Chave);
                    int cSitConf = Funcoes.intTry(vlrNo(docXML, "/resNFe/cSitConf"));
                    switch (cSitConf)
                    {
                        case 0:
                            item.Status = "NAO MANIFESTADO";
                            break;
                        case 1:
                            item.Status = "CONFIRMADO OPERACAO";
                            break;
                        case 2:
                            item.Status = "DESCONHECIMENTO DA OPERACAO";
                            break;
                        case 3:
                            item.Status = "OPERACAO NAO REALIZADA";
                            break;
                        case 4:
                            item.Status = "CIENCIA OPERACAO";
                            break;
                    }

                    item.salvar(!item.existeBD());

                    xmlR.Close();
                }
                System.Threading.Thread.Sleep(5000);

                importarNFe();
            }
            catch (Exception)
            {
                throw;
            }


        }
        public void importarNFe()
        {
            DirectoryInfo Dir = new DirectoryInfo(usr.filial.diretorio_exporta + "/retorno/dfe");

            if (!Directory.Exists(usr.filial.diretorio_exporta + "/retorno/NFE_Baixadas"))
            {
                Directory.CreateDirectory(usr.filial.diretorio_exporta + "/retorno/NFE_Baixadas");
            }
            FileInfo[] FilesXmlBaixados = Dir.GetFiles("*-procNFe.xml");
            foreach (FileInfo Arq in FilesXmlBaixados)
            {
                try
                {
                    String id = Arq.Name.ToUpper().Replace("-PROCNFE.XML", "");
                    String xml = DowLoadXML(id).Replace("'", "");
                    NfManifestoDAO NF = XML_NFE.NfeManifesto(xml, usr);

                    NF.salvar(!NF.existeBD());

                    XML_NFE.Salva_XML_NFE(xml, true);
                    if (!File.Exists(usr.filial.diretorio_exporta + "/retorno/NFE_Baixadas/" + Arq.Name))
                    {
                        File.Move(Arq.FullName, usr.filial.diretorio_exporta + "/retorno/NFE_Baixadas/" + Arq.Name);
                    }
                    else
                    {
                        File.Delete(Arq.FullName);
                    }

                }
                catch (Exception err)
                {
                    if (!Directory.Exists(usr.filial.diretorio_exporta + "/retorno/NFE_erro_importadas"))
                    {
                        Directory.CreateDirectory(usr.filial.diretorio_exporta + "/retorno/NFE_erro_importadas");
                    }
                    if (!File.Exists(usr.filial.diretorio_exporta + "/retorno/NFE_erro_importadas/" + Arq.Name))
                    {
                        File.Move(Arq.FullName, usr.filial.diretorio_exporta + "/retorno/NFE_erro_importadas/" + Arq.Name);
                    }
                    StreamWriter sw = new StreamWriter(usr.filial.diretorio_exporta + "/retorno/NFE_erro_importadas/" + Arq.Name + "_log_erro.txt");
                    sw.WriteLine(err.Message);
                    sw.Close();
                }
            }
        }
        public void verificarXmlsBaixados(String idStr)
        {
            DirectoryInfo Dir = new DirectoryInfo(usr.filial.diretorio_exporta + "/retorno/NFE_Baixadas");

            FileInfo[] FilesXmlBaixados = Dir.GetFiles("*" + idStr + "-procNFe.xml");
            String sql = "";
            foreach (FileInfo Arq in FilesXmlBaixados)
            {

                String id = Arq.Name.ToUpper().Replace("-PROCNFE.XML", "");
                String xml = System.IO.File.ReadAllText(Arq.Name);
                sql += " UPDATE Nf_manifestar SET nfeXml = '" + xml + "' where chave ='" + id + "'; ";


                sql += XML_NFE.Salva_XML_NFE(xml, false);
            }
            Conexao.executarSql(sql);
        }


        private String vlrNo(XmlNode no, String nome)
        {
            try
            {
                if (no.SelectSingleNode(nome) != null)
                    return no.SelectSingleNode(nome).InnerText.Trim().Replace("'", "");
                else
                    return "";
            }
            catch (Exception)
            {
                return "";
            }


        }


        private String DowLoadXML(String id)
        {
            String nome = usr.filial.diretorio_exporta + "/retorno/dfe/" + id + "-procNFe.xml";
            if (File.Exists(nome))
            {

                String strDoc = System.IO.File.ReadAllText(nome);


                return strDoc;
            }
            else
                return "";
        }
        public XmlDocument xmlManifestar(NfManifestoDAO nfe, String operacao)
        {
            String tpEvento = tpEventoCod(operacao);
            String descEvento = "";

            if (operacao.Equals("CONFIRMACAO OPERACAO"))
            {
                descEvento = "Confirmacao da Operacao";
            }
            else if (operacao.Equals("CIENCIA OPERACAO"))
            {
                descEvento = "Ciencia da Operacao";
            }
            else if (operacao.Equals("DESCONHECIMENTO OPERACAO"))
            {
                descEvento = "Desconhecimento da Operacao";
            }
            else if (operacao.Equals("OPERACAO NAO REALIZADA"))
            {
                descEvento = "Operacao nao Realizada";
            }

            XmlDocument xml = new XmlDocument();
            StringBuilder xmlConteudo = new StringBuilder();
            xmlConteudo.Append("<envEvento  xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"1.00\">");
            xmlConteudo.Append("<idLote>000000000000001</idLote>");

            xmlConteudo.Append("<evento xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"1.00\">");
            xmlConteudo.Append("<infEvento Id=\"ID" + tpEvento + nfe.Chave + "01\">");
            xmlConteudo.Append("<cOrgao>91</cOrgao>");
            xmlConteudo.Append(funGrava("tpAmb", (usr.filial.producaoNfe ? "1" : "2")));
            xmlConteudo.Append(funGrava("CNPJ", usr.filial.CNPJ.Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim()));
            xmlConteudo.Append(funGrava("chNFe", nfe.Chave));
            xmlConteudo.Append(funGrava("dhEvento", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz")));
            xmlConteudo.Append(funGrava("tpEvento", tpEvento));
            xmlConteudo.Append(funGrava("nSeqEvento", "1"));
            xmlConteudo.Append(funGrava("verEvento", "1.00"));
            xmlConteudo.Append("<detEvento versao=\"1.00\">");
            xmlConteudo.Append(funGrava("descEvento", descEvento));
            xmlConteudo.Append("</detEvento>");
            xmlConteudo.Append("</infEvento>");
            xmlConteudo.Append("</evento>");

            xmlConteudo.Append("</envEvento>");
            xml.LoadXml(xmlConteudo.ToString());
            return xml;
        }

        public XmlDocument xmlConsultaNotaManifestada(NfManifestoDAO nfe)
        {
            XmlDocument xml = new XmlDocument();
            StringBuilder xmlConteudo = new StringBuilder();
            xmlConteudo.Append("<distDFeInt xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"1.35\">");
            xmlConteudo.Append(funGrava("tpAmb", (usr.filial.producaoNfe ? "1" : "2")));
            xmlConteudo.Append("<cUFAutor>35</cUFAutor>");
            xmlConteudo.Append(funGrava("CNPJ", usr.filial.CNPJ.Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim()));

            xmlConteudo.Append("<distNSU>");
            xmlConteudo.Append(funGrava("ultNSU", nfe.NSU));
            xmlConteudo.Append("</distNSU>");
            xmlConteudo.Append("</distDFeInt>");
            xml.LoadXml(xmlConteudo.ToString());
            return xml;
        }


        public void manifestarNfe(List<NfManifestoDAO> LisNfe, String operacao)
        {

            String tpEvento = tpEventoCod(operacao);

            foreach (NfManifestoDAO nfe in LisNfe)
            {
                StreamWriter SW_2 = File.CreateText(usr.filial.diretorio_exporta + "/envio/" + tpEvento + "_" + nfe.Chave + "-ped-eve.xml");
                SW_2.Write(xmlManifestar(nfe, operacao).OuterXml);
                SW_2.Close();
            }


        }

        public void consultaNotasManifestadas(NfManifestoDAO nfe)
        {
            System.Threading.Thread.Sleep(5000);
            StreamWriter SW_2 = File.CreateText(usr.filial.diretorio_exporta + "/envio/" + usr.filial.CNPJ.Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim() + "-con-dist-dfe.xml");
            SW_2.Write(xmlConsultaNotaManifestada(nfe).OuterXml);
            SW_2.Close();

        }


        public void consultaNotaDFE(string chave)
        {

            StreamWriter SW_2 = File.CreateText(usr.filial.diretorio_exporta + "/envio/" + usr.filial.CNPJ.Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim() + "-con-dist-dfe.xml");
            SW_2.Write(xmlConsultaDFe(chave).OuterXml);
            SW_2.Close();
        }

        public String retornoConsultaNotaManifestada()
        {
            String resposta = "";
            String nome = usr.filial.diretorio_exporta + "/retorno/" + usr.filial.CNPJ + "-dist-dfe.xml";
            if (File.Exists(nome))
            {
                XmlDocument docXML = new XmlDocument();
                XmlTextReader xmlR = new XmlTextReader(nome) { Namespaces = false };
                docXML.Load(xmlR);
                resposta = vlrNo(docXML, "retDistDFeInt/xMotivo");
                strUltNSU = vlrNo(docXML, "retDistDFeInt/ultNSU");
                strMaxNSU = vlrNo(docXML, "retDistDFeInt/maxNSU");
                string cStat = vlrNo(docXML, "retDistDFeInt/cStat");

                if (!cStat.Equals("138"))
                {

                }

                if (xmlR != null)
                    xmlR.Close();
                Conexao.executarSql("insert into manifesto_consultas values ('" + usr.getFilial() + "','" + resposta + "',getdate(),'" + strUltNSU + "','" + strMaxNSU + "',0,'" + cStat + "');");

                if (resposta.Contains("Rejeicao: Consumo Indevido"))
                {
                    limparArquivo(nome);
                }

                DirectoryInfo Dir = new DirectoryInfo(usr.filial.diretorio_exporta + "/retorno/dfe");
                FileInfo[] Files = Dir.GetFiles("*-nfe.xml");
                FileInfo[] FilesProc = Dir.GetFiles("*-procNFe.xml");

                if (Files.Length > 0 || FilesProc.Length > 0)
                {
                    AtualiarXmlsManifesto();
                    resposta = Files.Length + FilesProc.Length + " Atualizados";

                    limparArquivo(nome);

                }
                else
                {
                    resposta = "Nenhum documento localizado";
                }

            }
            return resposta;

        }
        private void limparArquivo(String nome)
        {
            String dirLog = usr.filial.diretorio_exporta + "/retorno/dfeLog";
            if (!Directory.Exists(dirLog))
            {
                Directory.CreateDirectory(dirLog);
            }
            String novoCaminho = dirLog + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + usr.filial.CNPJ + "-dist-dfe.xml";
            File.Copy(nome, novoCaminho);
            File.Delete(nome);
        }

        public void retornoEventoManifestados(List<NfManifestoDAO> listNfe, String operacao)
        {
            String tpEvento = tpEventoCod(operacao);
            String nome = "";
            foreach (NfManifestoDAO nfe in listNfe)
            {
                nome = usr.filial.diretorio_exporta + "/retorno/" + tpEvento + "_" + nfe.Chave;
                if (File.Exists(nome + "-eve.xml"))
                {
                    XmlDocument docXML = new XmlDocument();
                    XmlTextReader xmlR = new XmlTextReader(nome + "-eve.xml") { Namespaces = false };
                    docXML.Load(xmlR);
                    String cStat = vlrNo(docXML, "/retEnvEvento/retEvento/infEvento/cStat");
                    String xMotivo = vlrNo(docXML, "/retEnvEvento/retEvento/infEvento/xMotivo");
                    String xEvento = vlrNo(docXML, "/retEnvEvento/retEvento/infEvento/xEvento");
                    if (cStat.Equals("135") || cStat.Equals("136"))
                    {
                        nfe.retornoManifestacao = "Evento Registrado: " + xEvento;
                        if (xEvento.Equals("Confirmacao da Operacao"))
                        {
                            nfe.Status = "CONFIRMADO OPERACAO";
                        }
                        else if (xEvento.Equals("Ciencia da Operacao"))
                        {
                            nfe.Status = "CIENCIA OPERACAO";
                        }
                        else if (xEvento.Equals("Desconhecimento da Operacao"))
                        {
                            nfe.Status = "DESCONHECIMENTO DA OPERACAO";
                        }
                        else if (xEvento.Equals("Operacao nao Realizada"))
                        {
                            nfe.Status = "OPERACAO NAO REALIZADA";
                        }
                        try
                        {
                            nfe.salvar(false);
                        }
                        catch (Exception err)
                        {

                            nfe.retornoManifestacao += err.Message;

                        }

                    }

                    else
                    {
                        nfe.retornoManifestacao = "ERRO:" + xMotivo;
                    }

                    if (xmlR != null)
                    {
                        xmlR.Close();
                    }
                }
                else
                {
                    if (File.Exists(nome + "-eve.err"))
                    {
                        StreamReader SR = new StreamReader(nome + "-eve.err", Encoding.Default);
                        String error = SR.ReadToEnd();
                        if (SR != null)
                            SR.Close();
                        nfe.retornoManifestacao = "ERRO:" + error;
                        throw new Exception("ERRO:" + error);
                    }
                    else
                    {
                        nfe.retornoManifestacao = "SEM RETORNO";
                    }
                }
            }
        }
        private string tpEventoCod(String operacao)
        {
            String tpEvento = "";

            if (operacao.Equals("CONFIRMACAO OPERACAO"))
            {
                tpEvento = "210200";

            }
            else if (operacao.Equals("CIENCIA OPERACAO"))
            {
                tpEvento = "210210";
            }
            else if (operacao.Equals("DESCONHECIMENTO OPERACAO"))
            {
                tpEvento = "210220";

            }
            else if (operacao.Equals("OPERACAO NAO REALIZADA"))
            {
                tpEvento = "210240";

            }
            return tpEvento;
        }
        public static void criarPastaDowload(List<NfManifestoDAO> listNfe, String enderecoPasta)
        {
            if (!Directory.Exists(enderecoPasta))
                Directory.CreateDirectory(enderecoPasta);

            foreach (NfManifestoDAO nf in listNfe)
            {
                StreamWriter SW_2 = File.CreateText(enderecoPasta + "/" + nf.Chave + "-procNFe.xml");
                SW_2.Write(nf.NfeXml);
                SW_2.Close();

            }
            using (ZipFile zip = new ZipFile())
            {

                // percorre todos os arquivos da lista
                // se o item é uma pasta
                if (Directory.Exists(enderecoPasta))
                {
                    try
                    {
                        // Adiciona a pasta no arquivo zip com o nome da pasta 
                        zip.AddDirectory(enderecoPasta, new DirectoryInfo(enderecoPasta).Name);


                    }
                    catch
                    {
                        throw;
                    }
                }

                // Salva o arquivo zip para o destino
                try
                {
                    zip.Save(enderecoPasta + ".zip");

                    Directory.Delete(enderecoPasta, true);


                }
                catch
                {
                    throw;
                }
            }




        }

        public String ultimoNSU()
        {
            if (strUltNSU.Equals("") || strUltNSU.Equals("000000000000000"))
            {
                return Conexao.retornaUmValor("Select ISNULL(max(ultNSU),'000000000000000') NSU from manifesto_consultas where Filial ='" + usr.getFilial() + "'", null);
            }
            else
            {
                return strUltNSU;
            }

        }

        public bool permitirNovaConsultaManifesto()
        {
            SqlDataReader rs = null;
            rs = Conexao.consulta("Select count(*) as qtde, esperar " +
                                  "from manifesto_consultas where data_hora >= DATEADD(mi, -5, GETDATE()) group by esperar;", usr, false);
            try
            {
                if (rs.Read())
                {
                    if (Funcoes.intTry(rs["qtde"].ToString()) > 10 || rs["esperar"].ToString().Equals("1"))
                    {
                        Conexao.executarSql("update manifesto_consultas set esperar = 1 where data_hora >= DATEADD (mi,-10,GETDATE())");
                        throw new Exception("O Limite de consulta foi atingido espere por 5 min para efetuar uma nova consulta! ");
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
            try
            {

                rs = Conexao.consulta("Select convert(varchar,DATEADD (hh,1,data_hora),108)  as hora from manifesto_consultas  where (ultNSU = maxNSU or cStat in ('137','656'))and data_hora >= DATEADD (hh,-1,GETDATE()) group by data_hora", usr, false);
                if (rs.Read())
                {
                    throw new Exception("Nenhum documento para baixar <br> Faça uma nova consulta após as " + rs["hora"].ToString());
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
            return true;
        }

        public XmlDocument xmlConsultaDFe(String chave)
        {

            XmlDocument xml = new XmlDocument();
            StringBuilder xmlConteudo = new StringBuilder();
            xmlConteudo.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            xmlConteudo.Append("<distDFeInt xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"1.01\">");
            xmlConteudo.Append(funGrava("tpAmb", (usr.filial.producaoNfe ? "1" : "2")));
            xmlConteudo.Append("<cUFAutor>35</cUFAutor>");
            xmlConteudo.Append(funGrava("CNPJ", usr.filial.CNPJ.Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim()));
            //Novo conteudo
            //xmlConteudo.Append("<distNSU>");
            //xmlConteudo.Append(funGrava("ultNSU", ultimoNSU().ToString()));
            //xmlConteudo.Append("</distNSU>");
            //xmlConteudo.Append("<consNSU>");
            //xmlConteudo.Append(funGrava("NSU", "000000000000000"));
            //xmlConteudo.Append("</consNSU>");

            xmlConteudo.Append("<consChNFe>");
            xmlConteudo.Append(funGrava("chNFe", chave));
            xmlConteudo.Append("</consChNFe >");
            xmlConteudo.Append("</distDFeInt>");
            xml.LoadXml(xmlConteudo.ToString());
            return xml;
        }
    }
}


