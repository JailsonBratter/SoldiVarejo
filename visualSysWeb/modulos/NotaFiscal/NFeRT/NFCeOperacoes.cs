
using DFe.Classes.Entidades;
using DFe.Classes.Flags;
using DFe.Utils;
using NFe.Classes;
using NFe.Classes.Informacoes.Detalhe.Tributacao;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Classes.Servicos.ConsultaCadastro;
using NFe.Classes.Servicos.Download;
using NFe.Classes.Servicos.Status;
using NFe.Classes.Servicos.Tipos;
using NFe.Servicos;
using NFe.Servicos.Retorno;
using NFe.Utils;
using NFe.Utils.Annotations;
using NFe.Utils.Assinatura;
using NFe.Utils.Enderecos;
using NFe.Utils.NFe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using visualSysWeb.dao;
using visualSysWeb.code;
using visualSysWeb.modulos.NotaFiscal.NFeRT.Cobranca;
using visualSysWeb.modulos.NotaFiscal.NFeRT.Transporte;

namespace visualSysWeb.modulos.NotaFiscal.NFeRT
{
    public class NFCeOperacoes
    {
        static string erros = "";
        public static int Contingencia = 0;
        public int sequencia_nf { get; set; }
        public int semcontingencia { get; set; }
        public static ConfiguracaoServico _configuracao;
        User usr;
        public filialDAO Loja;
        decimal vValorICMS = 0;
        public string arquivoXML = "";


        public NFCeOperacoes(User user)
        {
            usr = user;
            Loja = user.filial;
            _configuracao = new ConfiguracaoServico();
            CarregarConfiguracoes();
            arquivoXML = Loja.diretorio_exporta;
        }
        public NFe.Classes.nfeProc criarNFCe(nfDAO nfDAO, string tipo_pagamento = "", bool obrigacontingencia = false)
        {
            //Numero de lote e número de NFe
            Random rnd = new Random();

            var numeroNFCe = int.Parse(nfDAO.Codigo);
            var numeroLote = rnd.Next(1, 1000);

            //Pegar todos os itens do cupom.
            nf_itemDAO cupomItem = new nf_itemDAO(usr);
            List<nf_itemDAO> itens = nfDAO.NfItens;

            //Montar DETALHES e TOTAIS do NFCe
            var dadosNFCe = montarNFe(itens, nfDAO);
            //Montar destinatario
            var dadosDest = montarDestinatario(nfDAO);
            //Montar pagamento
            var dadosPagamento = montarPagamento(nfDAO);

            //Criar objeto NFCe
            NNFe nFe = new NNFe();
            retConsStatServ StatusOK = null;
            try
            {

                StatusOK = ConsultarStatusServico();

                if (StatusOK.cStat != 107)
                {
                    throw new Exception("Sem comunicação com sefaz.");
                }
     
                if (tipo_pagamento == "")
                {
                    tipo_pagamento = "01";
                }
                string corrigecep = Loja.CEP.Replace(".", "").Replace("-", "");
                Loja.CEP =corrigecep;
                nFe.infNFe = new InfNFe
                {

                    versao = "4.00",
                    emitente = new Emitente
                    {
                        CNPJ = Loja.CNPJ.Replace(".", "").Replace("/", "".Replace("-", "")),
                        xNome = (!Loja.producaoNfe ? "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL" : Loja.Razao_Social),
                        enderEmit = new EnderEmit
                        {
                            xLgr = Loja.Endereco,
                            nro = Loja.endereco_nro,
                            xBairro = Loja.bairro,
                            cMun = Funcoes.intTry(Loja.codigo_IBGE),
                            xMun = Loja.Cidade,
                            UF = Loja.UF,
                            CEP = Loja.CEP,
                            cPais = "1058",
                            xPais = "BRASIL",
                            fone = "1150786121"
                        },
                        IE = Loja.IE,
                        CRT = Loja.CRT.ToString()
                    },
                    destinatario = dadosDest,
                    identificacao = new Identificacao
                    {
                        nNF = numeroNFCe,
                        dhEmi = DateTime.Now,
                        dhSaiEnt = DateTime.Now,
                        cMunFG = int.Parse(Loja.codigo_IBGE),
                        serie = Convert.ToInt32(nfDAO.serie),
                        tpEmis = Contingencia == 0 ? 1 : 9,
                        tpAmb = (Loja.producaoNfe ? 1 : 2),
                        tpImp = 1, //Tipo de impressão
                        tpNF = (nfDAO.Tipo_NF.Equals("1") ? 1 : 0), //0 - Entrada ; 1 - Saída
                        idDest = (dadosDest.enderDest.UF != Loja.UF ? 2 : 1), //1=Operação Interna; 2=Operação Interestadual; 3=Operação com exterior.
                        finNFe = nfDAO.finNFe, //
                        indFinal = nfDAO.indFinal,
                        indPres = nfDAO.indPres, //Indicador de presença
                        natOp = nfDAO.NtOperacao.Descricao,
                    },
                    detalhes = dadosNFCe.det,
                    total = dadosNFCe.total,
                    transporte = montarTransporte(nfDAO),
                    pag = dadosPagamento,
                    infAdic = new informacaoAdicional
                    {
                        infCpl = "Tributos Aprox." + dadosNFCe.total.ICMSTot.vTotTribFormatado + " Fonte:IBPT",

                    },
                    responsavelTecnico = new responsavelTecnico
                    {
                        CNPJ = "53969281000151",
                        xContato = "JGA Solucoes em Tecnologia LTDA",
                        email = "sistemas@jgasolucoes.tec.br",
                        fone = "1150786121"
                    }
                };
                //Regras para cobrança
                if (!nfDAO.tPag.Equals("90"))
                {
                    nFe.infNFe.cobr = montarCobranca(nfDAO);
                }
                //Regras para nota fiscal referneiciada
                if ((nFe.infNFe.identificacao.finNFe == 2 || nFe.infNFe.identificacao.finNFe == 4) && nfDAO.NfReferencias.Count <= 0)
                {
                    throw new Exception("Para NFe COMPLEMENTAR ou DEVOLUÇÃO, é obrigatório uma NFe REFERENCIADA.");
                }

                if (nFe.infNFe.identificacao.indPres == 2)
                {
                    nFe.infNFe.identificacao.indIntermed = Tipos.IndicadorIntermediador.iiSitePlataformaTerceiros; //Indicador de intermediador/marketplace
                                                         //0 = Operação sem intermediador(em site ou plataforma própria)
                }
                else
                {
                    nFe.infNFe.identificacao.indIntermedSpecified = false;
                }

                //Checar CPF
                if (nfDAO.objCliente.CNPJ.Equals(""))
                {
                    nFe.infNFe.destinatario = null;
                }
                else
                {
                    //Define valor do elemento xNome (Nome do destinatário)
                    nFe.infNFe.destinatario.xNome = (!Loja.producaoNfe ? "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL" : "NAO INFORMADO");
                    string codigoCNPJCPF = nfDAO.objCliente.CNPJ.Replace(".", "").Replace("/", "").Replace("-", "").Trim();
                    if (codigoCNPJCPF.Length > 11)
                    {
                        nFe.infNFe.destinatario.CNPJ = codigoCNPJCPF; // nfDAO.objCliente.CNPJ;
                        nFe.infNFe.destinatario.indIEDest = "9";
                    }
                    else
                    {
                        nFe.infNFe.destinatario.CPF = codigoCNPJCPF; // nfDAO.objCliente.CNPJ;
                        nFe.infNFe.destinatario.indIEDest = "9";
                    }
                }

                nFe.infNFe.Id = "NFe" + FuncoesNFe.chaveNFe(nFe.infNFe.identificacao);

                //Salvando ID no DB
                nfDAO.id = nFe.infNFe.Id.Replace("NFe", "");
                if (!nfDAO.atualizaIDNFe())
                {
                    throw new Exception("Não foi possível gravar o ID: " + nFe.infNFe.Id + " no Banco de Dados");
                }

                nFe.infNFe.identificacao.cDV = nFe.infNFe.Id.Substring(nFe.infNFe.Id.Length - 1);
                if (Contingencia == 1)
                {
                    nFe.infNFe.identificacao.dhCont = DateTime.Now;
                    nFe.infNFe.identificacao.xJust = "Falha na comunicação com sefaz";
                }

                SerializarNFCeParaXML(nFe);

                FuncoesNFe.limparerros();

                //Validar arquivo XML
                if (nFe.infNFe.total.ICMSTot.vOutro == 0)
                {
                    string diretorioSchemas = Loja.diretorio_Schemas + (Loja.diretorio_Schemas.Substring(Loja.diretorio_Schemas.Length - 1) != "\\" ? "\\" : "");
                    if (!FuncoesNFe.validarXML(arquivoXML, diretorioSchemas))
                    {
                        throw new Exception("Erro na validação do arquivo: " + FuncoesNFe.ObterErros());
                    }
                }

                //A partir deste ponto utiliza-se as DLL NFe e DFe.
                var reader = new StringReader(File.ReadAllText(arquivoXML));
                var desserializador = new XmlSerializer(typeof(NFe.Classes.NFe));
                
                List<NFe.Classes.NFe> lNFe = new List<NFe.Classes.NFe>();
                var NFeEnvio = ((NFe.Classes.NFe)desserializador.Deserialize(reader));

                if (StatusOK.cStat == 107 && Contingencia == 0)
                {
                       
                    var retorno = EnviarNFe(Convert.ToInt32(numeroLote), NFeEnvio);
                    if (retorno.Retorno.cStat == 103 || retorno.Retorno.cStat == 104 || retorno.Retorno.cStat == 100)
                    {
                        //Console.WriteLine("#NFe#" + retorno.Retorno.infRec.nRec);
                        if (retorno.Retorno.protNFe.infProt.cStat != 100)
                        {
                            throw new Exception($"NFe: {NFeEnvio.infNFe.Id.ToString()} emitida com erro:{retorno.Retorno.protNFe.infProt.xMotivo}!");
                        }

                        var procNfe = new NFe.Classes.nfeProc
                        {
                            versao = "4.00",
                            NFe = NFeEnvio,

                            protNFe = retorno.Retorno.protNFe

                        };

                        var xmlFinal = procNfe.ObterXmlString();
                        var chave = NFeEnvio.infNFe.Id.Replace("NFe", "");
                        nfDAO.id = NFeEnvio.infNFe.Id.Replace("NFe","").Replace("NFCe","");

                        //A partir deste ponto utiliza-se as DLL NFe e DFe.
                        var readerProc = new StringReader(xmlFinal);
                        var desserializadorProc = new XmlSerializer(typeof(NFe.Classes.nfeProc));
                        var NFeRetornoProc = ((NFe.Classes.nfeProc)desserializadorProc.Deserialize(readerProc));

                        //nfDAO.EditarCupom(nfDAO);
                        string arquivoAutorizado = Funcoes.DefinirDiretorio(true, Loja);
                        arquivoAutorizado += (arquivoAutorizado.Substring(arquivoAutorizado.Length - 1) != "\\" ? "\\" : "");

                        File.WriteAllText( arquivoAutorizado + $"NFe{chave}-procNFe.xml", xmlFinal);

                        //Processo para atualização de status, estoque no DB ocorre no evento abaixo.
                        try
                        {
                            if (nfDAO.numeroProtocolo.Equals(""))
                            {
                                nfDAO.numeroProtocolo = NFeRetornoProc.protNFe.infProt.nProt.ToString();
                            }
                            nfDAO.id = NFeRetornoProc.NFe.infNFe.Id.Replace("NFe", "").Replace("NFCe", "");
                            nfDAO.status = "AUTORIZADO";
                            nfDAO.Emissao = NFeRetornoProc.protNFe.infProt.dhRecbto.Date;
                            nfDAO.dataHoraLancamento = NFeRetornoProc.protNFe.infProt.dhRecbto.DateTime;
                            if (nfDAO.Data <= nfDAO.Emissao)
                            {
                                nfDAO.Data = nfDAO.Emissao;
                            }
                            //Atualiza banco de dados e movimenta estoque se a natureza solicita.

                            nfDAO.AtualizarStatus();
                        }
                        catch
                        {

                        }

                        //Incluir processo para salvar o XML
                        Documento_EletronicoDAO docE = new Documento_EletronicoDAO();
                        if (!docE.exists(nfDAO.id))
                        {
                            docE.filial = usr.getFilial();
                            docE.tipo = (nfDAO.Tipo_NF.Equals("1") ? 2 : 3);
                            docE.data = nfDAO.Emissao;
                            docE.caixa = 0;
                            docE.documento = nfDAO.Codigo.Trim();
                            docE.id_chave = nfDAO.id;
                            docE.id_chave_cancelamento = "";
                            docE.nro_serie_equipamento = "";
                            docE.operador = "0";
                            docE.cfe_xml = xmlFinal;
                            docE.cfe_xml_cancelamento = "";
                            docE.insert();
                        }


                        //MessageBox.Show($"NFe: {chave} emitida com sucesso!");

                        return NFeRetornoProc;
                    }
                    else
                    {
                        throw new Exception(retorno.Retorno.xMotivo);
                    }
                }
                else
                {
                    throw new Exception(StatusOK.xMotivo);
                }
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
        }

        private (List<Detalhamento> det, Total total) montarNFe(List<nf_itemDAO> itens, nfDAO nf)
        {
            List<Detalhamento> detalhes = new List<Detalhamento>();
            Total total = new Total();
            string CPF_CNPJ = "";
            total.ICMSTot = new ICMSTot();
            //total.IBSCBSTot = new IBSCBSTot(); //foi retirado deste ponto pq só deve ser gerado de os valores de IBS e CBS forem > 0.
            
            decimal totalIBS = 0;
            decimal totalCBS = 0;
            decimal totalIS = 0;
            decimal totalNF = 0;
            decimal totalBC = 0;

            if (itens.Count > 0)
            {
                foreach (nf_itemDAO row in itens)
                {
                    sequencia_nf++;
                    vValorICMS = 0;
                    Detalhamento detItem = new Detalhamento();
                    detItem.nItem = sequencia_nf;
                    string tiraespaco = row.Descricao .Trim();
                    row.ean.Trim();
                       
                    row.Descricao = tiraespaco;

                    //Definindo os valores do produto
                    Produto produto = new Produto
                    {
                        cProd = row.PLU,
                        cEAN = row.ean.Length > 9 ? row.ean : "SEM GTIN",
                        xProd =row.Descricao.Trim(),
                        NCM = row.NCM.ToString().Trim().PadLeft(8, '0'),
                        CFOP = int.Parse(row.codigo_operacao.ToString()),
                        uCom = row.Und,
                        vUnCom = row.Unitario,
                        qCom = Math.Round(row.Qtde * row.Embalagem, 4),
                        vProd = row.Total,
                        cEANTrib = row.ean.Length > 9 ? row.ean : "SEM GTIN",
                        qTrib = Math.Round(row.Qtde * row.Embalagem, 4),
                        uTrib = row.Und,
                        vUnTrib = row.Unitario,
                        vDesc = row.DescontoValor,
                        vOutro = row.despesas,
                        indTot = 1,
                    };
                    //CEST
                    if (!row.CEST.Equals("0"))
                    {
                        produto.CEST = row.CEST.ToString().PadLeft(7, '0');
                    }
                    //Frete
                    if (row.Frete > 0)
                    {
                        produto.vFrete = row.Frete;
                    }
                    //Desconto
                    if (row.vDesconto_valor > 0)
                    {
                        produto.vDesc = row.vDesconto_valor;
                    }
                    //Outros
                    if (row.despesas > 0)
                    {
                        produto.vOutro = row.despesas;
                    }
                    //Referenciar pedido de compra
                    if (!row.Ordem_compra.Trim().Equals(""))
                    {
                        string strOrdem = row.pedidoItemNumero.Trim();
                        if (strOrdem.Length > 15)
                        {
                            produto.xPed = strOrdem.Substring(0, 15);
                        }
                        else
                        {
                            produto.xPed = strOrdem;
                        }
                        produto.nItemPed = row.pedidoItemSequencia.Trim();
                    }
                    detItem.produto = produto;

                    #region Imposto
                    Imposto imposto = new Imposto();
                    //Valor total do imposto
                    if (row.TotTrib > 0)
                    {
                        //if (!Uteis.CRT.ToString().Equals("3"))
                        //{
                        imposto.vTotTrib = row.TotTrib; // Math.Round((produto.vProd - produto.vDesc + produto.vOutro) * (row.PercentualImposto / 100), 2);
                        //}
                    }
                    imposto.ICMS = retornaICMS(row.origem.ToString(), row.indice_St.ToString(), row.aliquota_icms, produto.vProd - produto.vDesc + produto.vOutro);
                    imposto.PIS = retonaPIS(row.CSTPIS, produto.vProd - produto.vDesc + produto.vOutro - vValorICMS);
                    imposto.COFINS = retornaCOFINS(row.CSTCOFINS, produto.vProd - produto.vDesc + produto.vOutro - vValorICMS);

                    //ICMSUFDest
                    if (!(Loja.ICMSSN.Equals("102") && Loja.CRT.ToString().Equals("1")))
                    {
                        //Não é um fornecedor e a NFe é destinada a um consumidor final e a natureza de operação permite DIFAL
                        if (!nf.DestFornecedor && nf.indFinal == 1 && nf.NtOperacao.Difal)
                        {
                            //UF de origem é diferente da UF de destino e o cliente final não é contribuinte de ICMS
                            if ((Loja.UF.Trim().ToUpper() != nf.objCliente.UF.Trim().ToUpper())) // && !cli.indIEDest.ToString().Equals("1"))
                            {
                                // Tratamento para o novo grupo  
                                // Grupo de Tributação do ICMS para a UF de destino
                                decimal vBCUFDest = 0; //Valor da BC do ICMS na UF de destino
                                decimal pFCPUFDest = 0; // Percentual do ICMS relativo ao fundo de Combate à Pobreza (FCP) da UF de destino
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

                                ClienteDAO cli = nf.objCliente; //new ClienteDAO(nf.Cliente_Fornecedor, new User());
                                aliquota_imp_estadoDAO AliqInter = new aliquota_imp_estadoDAO(cli.UF.Trim().ToUpper(), row.NCM, null);

                                vBCUFDest = row.vBaseICMS;
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
                                        pICMSInter = row.aliquota_icms;
                                        break;
                                    default:
                                        pICMSInter = row.aliquota_icms;
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
                                        vICMSUFDest = row.valor_Difal_Item; //   (((vBCUFDest - (vBCUFDest * (pICMSInter / 100))) / ((100 - pICMSUFDest) / 100)) * (pICMSUFDest / 100)) - (vBCUFDest * (pICMSInter / 100));
                                        vBCUFDest = valorBCUFDest;
                                        vICMSUFRemet = 0; // (vBCUFDest * ((pICMSUFDest - pICMSInter) / 100)) - vICMSUFDest;

                                        pFCPUFDest = AliqInter.porc_combate_pobresa;
                                        vFCPUFDest = row.vFCP;

                                    }
                                    total.ICMSTot.vFCPUFDest += vFCPUFDest;
                                    total.ICMSTot.vICMSUFDest += vICMSUFDest;
                                    total.ICMSTot.vICMSUFRemet += vICMSUFRemet;
                                }
                                imposto.ICMSUFDest = new ICMSUFDest
                                {
                                    vBCUFDest = vBCUFDest,
                                    pFCPUFDest = pFCPUFDest,
                                    pICMSUFDest = pICMSUFDest,
                                    pICMSInter = pICMSInter,
                                    pICMSInterPart = pICMSInterPart,
                                    vFCPUFDest = vFCPUFDest,
                                    vICMSUFDest = vICMSUFDest,
                                    vICMSUFRemet = vICMSUFRemet
                                };
                            }
                        }
                    }


                    //Dados para reforma tributária
                    if (Loja.CRT.ToString().Equals("3"))
                    {
                        //imposto.IS = retornaIS();
                        //totalIS =  ??
                        imposto.IBSCBS = retornaIBSCBS(detItem.produto.vProd);
                        //Total da base de cálculo
                        totalBC += Funcoes.ConvertstrToDecimalCulture(imposto.IBSCBS.gIBSCBS.vBC);
                        //Total da IBSUF
                        totalIBS += Funcoes.ConvertstrToDecimalCulture(imposto.IBSCBS.gIBSCBS.vIBS);
                        //Total da IBSMUn
                        // ??
                        //Total CBS
                        totalCBS += Funcoes.ConvertstrToDecimalCulture(imposto.IBSCBS.gIBSCBS.gCBS.vCBS);
                    }

                    detItem.Imposto = imposto;
                    #endregion
                    #region impostoDevol
                    if (row.vIPIDevol > 0)
                    {
                        impostoDevol impostoDevol = new impostoDevol();
                        impostoDevol.pDevol = row.pDevol;
                        impostoDevol.IPI = new IPIDevolvido
                        {
                            vIPIDevol = row.vIPIDevol
                        };
                    }

                    #endregion
                    if (int.Parse(row.CSTPIS.ToString()) == 1)
                    {
                        total.ICMSTot.vPIS += imposto.PIS.PISAliq.vPIS;
                        total.ICMSTot.vCOFINS += imposto.COFINS.COFINSAliq.vCOFINS;
                    }
                    var (vBC, vIcms) = RetornaBcIcms(imposto.ICMS);

                    total.ICMSTot.vBC += vBC;
                    total.ICMSTot.vICMS += vIcms;


                    total.ICMSTot.vDesc += produto.vDesc;

                    total.ICMSTot.vOutro += produto.vOutro;


                    total.ICMSTot.vProd += (produto.vProd);

                    total.ICMSTot.vNF = total.ICMSTot.vProd - total.ICMSTot.vDesc + total.ICMSTot.vOutro;

                    //if (Uteis.CRT.ToString().Equals("3"))
                    //{
                    //    //Total trib na reforma deve refletir o total do IBS + CBS
                    //    imposto.vTotTrib = (Funcoes.ConvertstrToDecimalCulture(imposto.IBSCBS.gIBSCBS.gCBS.vCBS) + Funcoes.ConvertstrToDecimalCulture(imposto.IBSCBS.gIBSCBS.gIBSUF.vIBSUF));

                    //    total.ICMSTot.vTotTrib += imposto.vTotTrib;
                    //}
                    //else
                    //{
                    total.ICMSTot.vTotTrib += imposto.vTotTrib;
                    //}

                    detalhes.Add(detItem);
                }

            }
            //A SEFAZ-SP (modelo 65 – NFC-e) não aceita o grupo total de IBS/CBS, somente o grupo por item.
            if (Loja.CRT.ToString().Equals("3") )
            {
                total.Loja = Loja;
                total.IBSCBSTot = new IBSCBSTot();
                total.IBSCBSTot.vBCIBSCBS = totalBC.ToString();

                if (totalIBS > 0 || totalCBS > 0)
                {
                    total.IBSCBSTot.gIBS = new TotgIBS();
                    total.IBSCBSTot.gIBS.gIBSUF = new TotgIBSUF();
                    total.IBSCBSTot.gIBS.gIBSUF.vIBSUF = totalIBS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
                    total.IBSCBSTot.gIBS.vIBS = totalIBS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);

                    total.IBSCBSTot.gIBS.gIBSMun = new TotgIBSMun();
                    total.IBSCBSTot.gIBS.gIBSMun.vIBSMun = (0m).ToString("F2", System.Globalization.CultureInfo.InvariantCulture);

                    total.IBSCBSTot.gCBS = new TotgCBS();
                    total.IBSCBSTot.gCBS.vCBS = totalCBS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
                }
                total._vNFTot = totalBC;
            }

            return (detalhes, total);
        }

        private void CarregarConfiguracoes()
        {
            #region Set file config

            ConfiguracaoServico.Instancia.Certificado.TipoCertificado = (Loja.tipo_certificado.Equals("A1") ? TipoCertificado.A1Arquivo : TipoCertificado.A3);
            ConfiguracaoServico.Instancia.Certificado.Arquivo = Loja.certificado_arquivo;
            ConfiguracaoServico.Instancia.Certificado.Senha = Loja.certificado_senha;
            ConfiguracaoServico.Instancia.DiretorioSalvarXml = Loja.diretorio_exporta;
            //-------------------
            Estado uf;
            Enum.TryParse(Loja.UF, out uf);
            ConfiguracaoServico.Instancia.cUF = uf;
            //-------------------
            ConfiguracaoServico.Instancia.DiretorioSchemas = Loja.diretorio_Schemas;
            ModeloDocumento mod = ModeloDocumento.NFe;
            //Enum.TryParse(Properties.Settings.Default.modelo_documento, out mod);
            ConfiguracaoServico.Instancia.ModeloDocumento = mod;
            //-------------------
            ConfiguracaoServico.Instancia.SalvarXmlServicos = false;
            //-------------------
            int time;
            int.TryParse("30000", out time);
            ConfiguracaoServico.Instancia.TimeOut = time;
            //-------------------
            TipoAmbiente tamb = (Loja.producaoNfe ? TipoAmbiente.Producao : TipoAmbiente.Homologacao);
            ConfiguracaoServico.Instancia.tpAmb = tamb;
            //-------------------
            TipoEmissao temiss = TipoEmissao.teNormal;
            ConfiguracaoServico.Instancia.tpEmis = temiss;
            //-------------------------------------------------------------------------------

            //var versaoNFe = Properties.Settings.Default.versao_NFe;

            //ConfiguracaoServico.Instancia.ProtocoloDeSeguranca = System.Net.SecurityProtocolType.Tls;
            ConfiguracaoServico.Instancia.ProtocoloDeSeguranca = System.Net.SecurityProtocolType.Tls12;
            ConfiguracaoServico.Instancia.VersaoNfceAministracaoCSC = VersaoServico.Versao400;
            ConfiguracaoServico.Instancia.VersaoNFeAutorizacao = VersaoServico.Versao400;
            ConfiguracaoServico.Instancia.VersaoNfeConsultaCadastro = VersaoServico.Versao400;
            ConfiguracaoServico.Instancia.VersaoNfeConsultaDest = VersaoServico.Versao400;
            ConfiguracaoServico.Instancia.VersaoNfeConsultaProtocolo = VersaoServico.Versao400;
            ConfiguracaoServico.Instancia.VersaoNFeDistribuicaoDFe = VersaoServico.Versao400;
            ConfiguracaoServico.Instancia.VersaoNfeDownloadNF = VersaoServico.Versao400;
            ConfiguracaoServico.Instancia.VersaoNfeInutilizacao = VersaoServico.Versao400;
            ConfiguracaoServico.Instancia.VersaoNfeRecepcao = VersaoServico.Versao400;
            ConfiguracaoServico.Instancia.VersaoNFeRetAutorizacao = VersaoServico.Versao400;
            ConfiguracaoServico.Instancia.VersaoNfeRetRecepcao = VersaoServico.Versao400;
            ConfiguracaoServico.Instancia.VersaoNfeStatusServico = VersaoServico.Versao400;
            ConfiguracaoServico.Instancia.VersaoRecepcaoEventoCceCancelamento = VersaoServico.Versao400;

            _configuracao = ConfiguracaoServico.Instancia;

            #endregion

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public retConsStatServ ConsultarStatusServico()
        {
            try
            {
                var servicoNFe = new ServicosNFe(_configuracao); // ConfiguracaoServico.Instancia);
                return servicoNFe.NfeStatusServico().Retorno;
            }
            catch (Exception ex)
            {
                retConsStatServ ret = new retConsStatServ()
                {
                    cStat = 500,
                    xMotivo = ex.Message,
                };
                return ret;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numLote"></param>
        /// <param name="nfe"></param>
        /// <returns></returns>
        public RetornoNFeAutorizacao EnviarNFe(Int32 numLote, NFe.Classes.NFe nfe)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            nfe.Assina(); //não precisa validar aqui, pois o lote será validado em ServicosNFe.NFeAutorizacao
            var servicoNFe = new ServicosNFe(_configuracao); // ConfiguracaoServico.Instancia);
            return servicoNFe.NFeAutorizacao(numLote, IndicadorSincronizacao.Sincrono, new List<NFe.Classes.NFe> { nfe });
        }

        /// <summary>
        ///     Consulta a situação cadastral, com base na UF/Documento
        ///     <para>O documento pode ser: CPF ou CNPJ. O serviço avaliará o tamanho da string passada e determinará se a coonsulta será por CPF ou por CNPJ</para>
        /// </summary>
        /// <param name="uf">Sigla da UF consultada, informar 'SU' para SUFRAMA.</param>
        /// <param name="tipoDocumento">Tipo do documento</param>
        /// <param name="documento">CPF ou CNPJ</param>
        /// <returns>Retorna um objeto da classe RetornoNfeConsultaCadastro com o retorno do serviço NfeConsultaCadastro</returns>
        public RetornoNfeConsultaCadastro ConsultaCadastro(string uf, ConsultaCadastroTipoDocumento tipoDocumento,
            string documento)
        {
            var servicoNFe = new ServicosNFe(_configuracao); // ConfiguracaoServico.Instancia);
            return servicoNFe.NfeConsultaCadastro(uf, tipoDocumento, documento);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recibo"></param>
        /// <returns></returns>
        public RetornoNFeRetAutorizacao ConsultarReciboDeEnvio(string recibo)
        {
            var servicoNFe = new ServicosNFe(_configuracao);
            return servicoNFe.NFeRetAutorizacao(recibo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnpjEmitente"></param>
        /// <param name="numeroLote"></param>
        /// <param name="sequenciaEvento"></param>
        /// <param name="chaveAcesso"></param>
        /// <param name="protocolo"></param>
        /// <param name="justificativa"></param>
        /// <returns></returns>
        public RetornoRecepcaoEvento CancelarNFe(string cnpjEmitente, int numeroLote, short sequenciaEvento, string chaveAcesso,
            string protocolo, string justificativa)
        {
            var servicoNFe = new ServicosNFe(_configuracao);
            return servicoNFe.RecepcaoEventoCancelamento(numeroLote, sequenciaEvento, protocolo, chaveAcesso, justificativa, cnpjEmitente);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ano"></param>
        /// <param name="cnpj"></param>
        /// <param name="justificativa"></param>
        /// <param name="numeroInicial"></param>
        /// <param name="numeroFinal"></param>
        /// <param name="serie"></param>
        /// <returns></returns>
        public RetornoNfeInutilizacao InutilizarNumeracao(int ano, string cnpj, string justificativa,
            int numeroInicial, int numeroFinal, int serie)
        {
            var servicoNFe = new ServicosNFe(_configuracao);
            return servicoNFe.NfeInutilizacao(cnpj, Convert.ToInt16(ano.ToString().Substring(2, 2)), _configuracao.ModeloDocumento, Convert.ToInt16(serie), Convert.ToInt32(numeroInicial), Convert.ToInt32(numeroFinal), justificativa);
        }
        private ICMS retornaICMS(string origem, string cst, decimal aliquota = 0, decimal bc = 0, decimal reducaoBC = 0)
        {
            ICMS iCMS = new ICMS();
            if (Loja.CRT.Equals("1") || Loja.CRT.Equals("2")) //Se o regime tributário for SIMPLES NACIONAL ou SIMPLES NACIONAL COM EXCESSO DE RECEITA
            {
                //De acordo com o CST será aplicado o CSOSN
                switch (cst)
                {
                    case "500":
                        ICMSSN500 iCMSSN500 = new ICMSSN500
                        {
                            CSOSN = "500",
                            orig = origem
                        };
                        iCMS.ICMSSN500 = iCMSSN500;
                        break;
                    case "101":
                        ICMSSN101 iCMSSN101 = new ICMSSN101
                        {
                            CSOSN = "101",
                            orig = origem,
                            pCredSN = aliquota,
                            vCredICMSSN = Math.Round(bc * (aliquota / 100), 2)
                        };
                        iCMS.ICMSSN101 = iCMSSN101;
                        break;
                    case "102":
                    case "103":
                    case "300":
                    case "400":
                        ICMSSN102 iCMSSN102 = new ICMSSN102
                        {
                            CSOSN = "102",
                            orig = origem
                        };
                        iCMS.ICMSSN102 = iCMSSN102;
                        break;
                    default:
                        ICMSSN900 iCMSSN900 = new ICMSSN900
                        {
                            CSOSN = "900",
                            orig = origem
                        };
                        iCMS.ICMSSN900 = iCMSSN900;
                        break;

                }
            }
            else //Lucro Real ou Lucro presumido
            {
                switch (cst)
                {
                    case "60": //Tributação ICMS cobrado anteriormente por substituição tributária
                        ICMS60 iCMS60 = new ICMS60
                        {
                            orig = origem,
                            CST = cst
                        };
                        iCMS.ICMS60 = iCMS60;
                        break;
                    case "00": //Tributada integralmente
                        ICMS00 iCMS00 = new ICMS00
                        {
                            orig = origem,
                            CST = cst,
                            modBC = 3,
                            vBC = bc,
                            pICMS = aliquota,
                            vICMS = Math.Round(bc * (aliquota / 100), 2)
                        };
                        vValorICMS = iCMS00.vICMS; //Pegar o valor do ICMS para cálculo da BC do PIS e do COFINS
                        iCMS.ICMS00 = iCMS00;
                        break;
                    case "20": //Tributação com redução de base de cálculo
                        ICMS20 iCMS20 = new ICMS20
                        {
                            orig = origem,
                            CST = cst,
                            modBC = 3,
                            pRedBC = reducaoBC,
                            vBC = Math.Round(bc * ((100 - reducaoBC) / 100), 2),
                            pICMS = aliquota,
                            vICMS = Math.Round((bc * ((100 - reducaoBC) / 100)) * (aliquota / 100), 2)
                        };
                        vValorICMS = iCMS20.vICMS; //Pegar o valor do ICMS para cálculo da BC do PIS e do COFINS
                        iCMS.ICMS20 = iCMS20;
                        break;
                    case "40": //Isento
                    case "41": //Não tributado
                    case "50": //Suspenso
                        ICMS40 iCMS40 = new ICMS40
                        {
                            orig = origem,
                            CST = cst
                        };
                        iCMS.ICMS40 = iCMS40;
                        break;
                    case "90": //Outros
                        ICMS90 iCMS90 = new ICMS90
                        {
                            orig = origem,
                            CST = cst
                        };
                        iCMS.ICMS90 = iCMS90;
                        break;
                }
            }
            return iCMS;
        }
        private PIS retonaPIS(string cst, decimal bc = 0)
        {
            if (cst == "49")
            {
                cst = "08";
            }
            PIS pIS = new PIS();
            switch (cst)
            {
                case "01":

                    PISAliq aliq = new PISAliq
                    {
                        CST = "01",
                        vBC = bc,
                        pPIS = Loja.pis,
                        vPIS = Math.Round((bc * Math.Round(Loja.pis / 100, 4)), 2)
                    };
                    pIS.PISAliq = aliq;
                    break;
                case "04":
                case "05":
                case "06":
                case "07":
                case "08":
                case "09":
                    PISNT pISNT = new PISNT
                    {
                        CST = cst
                    };
                    pIS.PISNT = pISNT;
                    break;
                default:
                    PISOutr pISOUTR = new PISOutr
                    {
                        CST = cst

                    };
                    pIS.PISOutr = pISOUTR;
                    break;


            }
            return pIS;
        }
        private COFINS retornaCOFINS(string cst, decimal bc = 0)
        {
            COFINS pCofins = new COFINS();
            if (cst == "49")
            {
                cst = "08";
            }
            switch (cst)
            {
                case "01":
                    COFINSAliq aliq = new COFINSAliq
                    {
                        CST = "01",
                        vBC = bc,
                        pCOFINS = Loja.cofins,
                        vCOFINS = Math.Round((bc * Math.Round(Loja.cofins / 100, 4)), 2)//Math.Round((bc * 0.076m), 2)
                    };
                    pCofins.COFINSAliq = aliq;
                    break;
                case "04":
                case "05":
                case "06":
                case "07":
                case "08":
                case "09":

                    COFINSNT pCOFINSNT = new COFINSNT
                    {
                        CST = cst
                    };
                    pCofins.COFINSNT = pCOFINSNT;
                    break;
                default:
                    COFINSOutr cOFINSOutr = new COFINSOutr
                    {
                        CST = cst
                    };
                    pCofins.COFINSOutr = cOFINSOutr;
                    break;
            }
            return pCofins;
        }
        private IS retornaIS()
        {
            IS iS = new IS();
            iS.CSTIS = "000";
            iS.cClassTribIS = "000001";
            iS._vBCIS = 0m;
            iS._uTrib = "UN";
            return iS;
        }
        private IBSCBS retornaIBSCBS(decimal vBC = 0)
        {
            IBSCBS iIBSCBS = new IBSCBS();
            iIBSCBS.CST = "000";
            iIBSCBS.cClassTrib = "000001";
            iIBSCBS.gIBSCBS = new gIBSCBS();
            iIBSCBS.gIBSCBS._vBC = vBC;
            //Gerando o gIBSUF
            gIBSUF giBSUF = new gIBSUF();
            giBSUF._pIBSUF = 0.1m;
            giBSUF._vIBSUF = (vBC * 0.001m);
            //Gerando IBSMun
            gIBSMun giBSMun = new gIBSMun();
            giBSMun._pIBSMun = (0.0m);
            giBSMun._vIBSMun = (0.0m);
            //Gerando o gCBS
            gCBS gcBS = new gCBS();
            gcBS._pCBS = (0.9m);
            gcBS._vCBS = (vBC * 0.009m);

            iIBSCBS.gIBSCBS._vIBS = giBSUF._vIBSUF + giBSMun._vIBSMun;


            iIBSCBS.gIBSCBS.gIBSUF = giBSUF;
            iIBSCBS.gIBSCBS.gIBSMun = giBSMun;
            iIBSCBS.gIBSCBS.gCBS = gcBS;


            return iIBSCBS;
        }

        public (decimal vBC, decimal vIcms) RetornaBcIcms(ICMS imposto)
        {
            if (imposto.ICMS00 != null)
            {
                return (imposto.ICMS00.vBC, imposto.ICMS00.vICMS);
            }
            else if (imposto.ICMS20 != null)
            {
                return (imposto.ICMS20.vBC, imposto.ICMS20.vICMS);
            }
            else if (imposto.ICMS90 != null)
            {
                return (imposto.ICMS90.vBC, imposto.ICMS90.vICMS);
            }
            else if (imposto.ICMSSN101 != null)
            {
                return (0, imposto.ICMSSN101.vCredICMSSN);
            }
            else if (imposto.ICMSSN900 != null)
            {
                return (imposto.ICMSSN900.vBC, imposto.ICMSSN900.vICMS);
            }
            else
            {
                return (0, 0);
            }
        }
        void SerializarNFCeParaXML(NNFe nfe)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(NNFe));
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "http://www.portalfiscal.inf.br/nfe");
                arquivoXML = Funcoes.DefinirDiretorio(false, Loja);

                arquivoXML += (arquivoXML.Substring(arquivoXML.Length -1) == "\\" ? "" : "\\") + nfe.infNFe.Id + ".xml";

                string xml;

                var settings = new XmlWriterSettings
                {
                    Indent = false,
                    NewLineHandling = NewLineHandling.None,
                    Encoding = new UTF8Encoding(false), // sem BOM
                    OmitXmlDeclaration = false
                };

                using (var memoryStream = new MemoryStream())
                {
                    using (var xmlWriter = XmlWriter.Create(memoryStream, settings))
                    {
                        serializer.Serialize(xmlWriter, nfe, ns);
                    }

                    memoryStream.Position = 0; // volta ao início para leitura

                    using (var reader = new StreamReader(memoryStream, Encoding.UTF8))
                    {
                        xml = reader.ReadToEnd(); // agora funciona sem erro
                    }
                }


                var XML = new XmlDocument();
                if (nfe.infNFe.identificacao.tpEmis == 9)
                    XML.PreserveWhitespace = true;
                XML.LoadXml(xml);

                //Salvar o XML no arquivo, também sem quebras de linhas
                using (var writer = XmlWriter.Create(arquivoXML, settings))
                {
                    XML.Save(writer);
                }

                //Assina o arquivo
                Certificado.AssinarXML(XML, nfe.infNFe.identificacao.tpEmis, Loja);
                XML.Save(arquivoXML);

                if (File.Exists(arquivoXML))
                {
                    //throw new Exception("SEREALIZARNFCEPARAXML(). Após assinatura. O arquivo existe");
                }
                else
                {
                    throw new Exception("SEREALIZARNFCEPARAXML(). Após assinatura. O arquivo NÃO existe");
                }
                //*** FOI MOVIDO PARA PARTE DE CIMA
                ////Salvar o XML no arquivo, também sem quebras de linhas
                //using (var writer = XmlWriter.Create(arquivoXML, settings))
                //{
                //    XML.Save(writer);
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao serializar: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine("Inner: " + ex.InnerException.Message);
            }
        }
        //Novo processo para tentar gerar a contingência.
        public static string SerializeToXML<T>(T obj)
        {
            var serializer = new XmlSerializer(typeof(T));
            var settings = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(false),
                Indent = false,
                OmitXmlDeclaration = false,
                NewLineHandling = NewLineHandling.None
            };

            using (var stringWrite = new utf8StringWrite())
            using (var xmlWriter = XmlWriter.Create(stringWrite, settings))
            {
                serializer.Serialize(xmlWriter, obj);
                return stringWrite.ToString();
            }
        }

        public static string AssinarXml(string xml, string tagAssinatura, X509Certificate2 cert)
        {
            // Carrega o XML como XElement
            var xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.LoadXml(xml); // necessário para usar SignedXml

            var signedXml = new SignedXml(xmlDoc);
            signedXml.SigningKey = cert.GetRSAPrivateKey();

            // Referência à tag <infNFe Id="NFe...">
            var infNFe = (XmlElement)xmlDoc.GetElementsByTagName(tagAssinatura)[0];
            var reference = new Reference();
            reference.Uri = "#" + infNFe.GetAttribute("Id");

            var env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);

            signedXml.AddReference(reference);

            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(cert));
            signedXml.KeyInfo = keyInfo;

            signedXml.ComputeSignature();

            var signatureXml = signedXml.GetXml();

            // Inserir a assinatura depois de <infNFe>
            infNFe.ParentNode.InsertAfter(xmlDoc.ImportNode(signatureXml, true), infNFe);

            using (var sw = new StringWriter())
            using (var xw = XmlWriter.Create(sw, new XmlWriterSettings { Encoding = Encoding.UTF8, OmitXmlDeclaration = false }))
            {
                xmlDoc.Save(xw);
                return sw.ToString();
            }
        }

        public Destinatario montarDestinatario(nfDAO nf)
        {
            Destinatario dest = new Destinatario();
            EnderDest enderDest = new EnderDest();
            string strPessoa = "";
            string codigoCNPJCPF = "";
            if (nf.DestFornecedor)
            {
                var fornecedor = new fornecedorDAO(nf.Cliente_Fornecedor, null);

                strPessoa = (fornecedor.pessoa_fisica ? "F" : "J");

                codigoCNPJCPF = fornecedor.CNPJ.Replace(".", "").Replace("/", "").Replace("-", "").Trim();
                if (codigoCNPJCPF.Length > 11)
                {
                    dest.CNPJ = codigoCNPJCPF;
                }
                else
                {
                    dest.CPF = codigoCNPJCPF;
                }

                dest.xNome = fornecedor.Razao_social.Trim().Replace("&", "E");
                enderDest.xLgr = fornecedor.Endereco.Trim();
                enderDest.nro = fornecedor.Endereco_nro.Trim();
                //enderDest.xCpl = "";
                enderDest.xBairro = fornecedor.Bairro.Trim();
                enderDest.cMun = int.Parse(Conexao.retornaUmValor("Select munic from unidade_federacao where nome_munic='" + fornecedor.Cidade.Trim() + "' and sigla_uf='" + fornecedor.UF.Trim() + "'", new User()));
                enderDest.xMun = fornecedor.Cidade.Trim();
                enderDest.UF = fornecedor.UF.Trim();
                enderDest.CEP = fornecedor.CEP.Replace("-", "").Trim();
                enderDest.fone = fornecedor.telefone1.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

                dest.IE = fornecedor.IE.Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim();
                dest.email = fornecedor.email.Trim();
                dest.indIEDest = fornecedor.indIEDest.ToString();
            }
            else
            {

                var cliente = nf.objClienteNovo;
                //indIEDest = "9";
                dest.indIEDest = cliente.indIEDest.ToString();
                //ivaDescricao = cliente.Iva_descricao;
                strPessoa = (cliente.Pessoa_Juridica ? "J" : "F");

                codigoCNPJCPF = cliente.CNPJ.Replace(".", "").Replace("/", "").Replace("-", "").Trim();
                if (codigoCNPJCPF.Length > 11)
                {
                    dest.CNPJ = codigoCNPJCPF;
                }
                else
                {
                    dest.CPF = codigoCNPJCPF;
                }

                bool bNomeFantasia = Funcoes.valorParametro("NOME_FANTASIA_ENF", new User()).ToUpper().Equals("TRUE");
                String strNomeCliente = (bNomeFantasia ? cliente.nome_fantasia : cliente.Nome_Cliente).Trim();

                dest.xNome = strNomeCliente.Trim().Replace("&", "E");
                enderDest.xLgr = cliente.Endereco.Trim();
                enderDest.nro = cliente.endereco_nro.Trim();
                //enderDest.xCpl = cliente.complemento_end.Trim();
                enderDest.xBairro = cliente.Bairro.Trim();
                enderDest.cMun = int.Parse(Conexao.retornaUmValor("Select munic from unidade_federacao where nome_munic='" + cliente.Cidade.Trim() + "' and sigla_uf='" + cliente.UF.Trim() + "'", new User()));
                enderDest.xMun = cliente.Cidade.Trim();
                enderDest.UF = cliente.UF.ToUpper().Trim();
                enderDest.CEP = cliente.CEP.Replace("-", "").Trim();
                enderDest.cPais = "1058";
                enderDest.xPais = "BRASIL";
                enderDest.fone = cliente.primeiroMeioComunicacao().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                if (strPessoa.Equals("F"))
                {
                    dest.IE = null;
                    if (enderDest.UF != Loja.UF) //Caso a emissão seja para outro estado
                    {
                        dest.indIEDest = "9"; //Não contribuinte, que pode ou não possuir  IE
                    }
                    else
                    {
                        dest.indIEDest = "1";
                    }
                }
                else
                {
                    dest.IE = cliente.IE.Replace(",", "").Replace(".", "").Replace("-", "").Replace("-", "").Replace("/", "").Trim();
                }
                dest.email = cliente.email().Trim();

            }

            dest.enderDest = enderDest;

            String erroDestinatario = "";
            if (enderDest.xLgr.Equals(""))
                erroDestinatario = "-Endereço do Destinatario não preenchido! <br/>";
            if (enderDest.cMun.Equals(""))
                erroDestinatario += "-Cidade do Destinatario não preenchida ou Invalida! <br/>".ToString();
            if (enderDest.UF.Equals(""))
                erroDestinatario += "-UF do Destinatario não preenchida! <br/>";

            if (enderDest.CEP.Equals(""))
                erroDestinatario += "-CEP do Destinatario não preenchido! <br/> ";
            if (enderDest.fone.Equals(""))
                erroDestinatario += "-Telefone do Destinatario não preenchido! <br/>";

            if (dest.email.Equals(""))
                erroDestinatario += "-Email do Destinatario não preenchido! <br/>";

            if (erroDestinatario.Length > 0)
                throw new Exception("===============ERROS================= <br/>" + erroDestinatario);

            return dest;
        }

        public cobr montarCobranca(nfDAO nf)
        {
            cobr cobr = new cobr();
            cobr.fat = new fat
            {
                nFat = nf.Codigo.Trim(),
                vOrig = nf.Total,
                vDesc = 0,
                vLiq = nf.Total
            };
            if (nf.tPag.Equals("14") || nf.tPag.Equals("15"))
            {
                cobr.dup = new List<dup>();
                int i = 1;
                foreach (nf_pagamentoDAO pg in nf.NfPagamentos)
                {
                    dup dupl = new dup();
                    dupl.nDup = i.ToString().PadLeft(3, '0');
                    dupl.dVenc = pg.Vencimento;
                    dupl.vDup = pg.Valor;
                    cobr.dup.Add(dupl);
                    i++;
                }
            }
            return cobr;
        }
        public Pagamento montarPagamento(nfDAO nf)
        {
            Pagamento pgto = new Pagamento();
            pgto.DetPag = new List<DetPag>(); //Matriz de detalhamento de pagamento
            
            DetPag detPag = new DetPag(); //Detalhamento para cada pagamento
            string indPag = nf.pagamentoAvista(); //Definição se o pgto é avista ou a prazo.
            if (nf.NfPagamentos.Count > 0)
            {
                detPag.indPag = int.Parse(indPag);
            }
            detPag.tPag = nf.tPag;
            detPag.vPag = (nf.tPag == "90" ? 0 : nf.TotalPag());

            //Caso o pagamento seja 03-Cartão de C´redito; 04-Cartão de Débito ou 17-Pagamento Instantâeneo (PIX)
            if (nf.tPag.Equals("03") || nf.tPag.Equals("04") || nf.tPag.Equals("17"))
            {
                detPag.Card = new Card
                {
                    tpIntegra = "2",
                    CNPJ = nf.CNPJPagamento
                };
            }
            pgto.DetPag.Add(detPag);
            return pgto;
        }
        public transp montarTransporte(nfDAO nf)
        {
            transp transp = new transp();
            if (nf.tipo_frete.Equals("9"))
            {
                transp.modFrete = 9;
            }
            else
            {
                transp.modFrete = int.Parse(nf.tipo_frete);
                transp.transporta = new transporta();
                if (nf.cnpjTransportadora.Trim().Length == 14)
                {
                    transp.transporta.CNPJ = nf.cnpjTransportadora.Trim();
                }
                else
                {
                    transp.transporta.CPF = nf.cnpjTransportadora.Trim();
                }
                transp.transporta.xNome = nf.nome_transportadora.Trim();
                //Endereço
                if (!nf.endereco_transportadora.Equals(""))
                {
                    transp.transporta.xEnder = nf.endereco_transportadora.Trim();
                }
                //Municipio
                if (!nf.municipio_transportadora.Trim().Equals(""))
                {
                    transp.transporta.xMun = nf.municipio_transportadora.Trim();
                }
                //UF
                if (!nf.estado_transportadora.Trim().Equals(""))
                {
                    transp.transporta.UF = nf.estado_transportadora.Trim().ToUpper();
                }
                transp.vol = new List<vol>();
                vol volumes = new vol();

                volumes.qVol = Funcoes.intTry(nf.qtde.ToString());
                volumes.esp = nf.especie;
                volumes.pesoL = nf.peso_liquido;
                volumes.pesoB = nf.peso_bruto;
                transp.vol.Add(volumes);
            }
            return transp;
        }
    }


    //Classe exclusiva para contingência
    public class utf8StringWrite : StringWriter
    {
        public override Encoding Encoding => new UTF8Encoding(false);
    }
}
