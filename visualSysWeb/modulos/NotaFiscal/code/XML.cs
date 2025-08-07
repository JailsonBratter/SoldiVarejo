using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Xml;
using System.Data.SqlClient;
using System.Data;
using visualSysWeb.dao;
using visualSysWeb.code;
using visualSysWeb.modulos.NotaFiscal.dao;

namespace XML_NFe
{
    class XML_NFE
    {
        public static string ID = "";
        public static int ide_nNF = 0;
        public static string emit_cnpj = "";
        public static string emit_xnome = "";
        public static int emit_CRT = 0;
        public static string dest_cnpj = "";
        public static string ide_sEmi = "";
        public static string ide_natOperacao = "";

        public static DateTime ide_dEmi = Convert.ToDateTime("1900-01-01 00:00:00.000");

        //Total
        public static decimal Total_ICMSTot_vBC = 0;
        public static decimal Total_ICMSTot_vICMS = 0;
        public static decimal Total_ICMSTot_vBCST = 0;
        public static decimal Total_ICMSTot_vST = 0;
        public static decimal Total_ICMSTot_vProd = 0;
        public static decimal Total_ICMSTot_vFrete = 0;
        public static decimal Total_ICMSTot_vSeg = 0;
        public static decimal Total_ICMSTot_vDesc = 0;
        public static decimal Total_ICMSTot_vII = 0;
        public static decimal Total_ICMSTot_vIPI = 0;
        public static decimal Total_ICMSTot_vPIS = 0;
        public static decimal Total_ICMSTot_vCOFINS = 0;
        public static decimal Total_ICMSTot_vOutro = 0;
        public static decimal Total_ICMSTot_vICMSDeson = 0;
        public static decimal Total_ICMSTot_vFCP = 0;
        public static decimal Total_ICMSTot_vFCPST = 0;
        public static decimal Total_ICMSTot_vFCPSTRet = 0;
        public static decimal Total_ICMSTot_vNF = 0;
        public static decimal Total_ICMSTot_vTotTrib = 0;

        //Duplicatas
        public static string cobr_dup_ndup = "";
        public static DateTime cobr_dup_dVenc = Convert.ToDateTime("1900-01-01 00:00:00.000");
        public static decimal cobr_dup_vDup = 0;

        public static string sql = "";
        //Variavel para definir se houve ou não a importação
        public static bool Inserido = false;

        public static string Salva_XML_NFE(string Arquivo, bool salvar)
        {
            try
            {
                sql = "";
                XmlDocument docXML = new XmlDocument();
                docXML.LoadXml(Arquivo);
                XmlNodeList elemNF = docXML.GetElementsByTagName("infNFe");
                //XmlNodeList elemDET = docXML.GetElementsByTagName("det");

                ID = elemNF.Item(0).Attributes["Id"].InnerText.ToString();
                //Se existir uma NF na tabela o sistema fara a exclusão
                int nReg = Funcoes.intTry(Conexao.retornaUmValor("Select count(*) from NFe_XML where id ='" + ID + "'", null));
                if (nReg > 0)
                {
                    sql += " DELETE FROM NFe_XML WHERE ID = '" + ID + "';";
                }

                ide_nNF = Funcoes.intTry(elemNF.Item(0).ChildNodes[0]["nNF"].InnerText.ToString());
                ide_sEmi = elemNF.Item(0).ChildNodes[0]["dhEmi"].InnerText.ToString();
                ide_dEmi = Convert.ToDateTime(ide_sEmi.ToString().Substring(0, 10) + " " + ide_sEmi.ToString().Substring(11, 8) + ".000");
                ide_natOperacao = elemNF.Item(0).ChildNodes[0]["natOp"].InnerText.ToString();

                try
                {
                    emit_cnpj = elemNF.Item(0).ChildNodes[1]["CNPJ"].InnerText.ToString();
                }
                catch (Exception)
                {
                    emit_cnpj = elemNF.Item(0).ChildNodes[1]["CPF"].InnerText.ToString();
                }


                emit_xnome = Funcoes.RemoverAcentos(elemNF.Item(0).ChildNodes[1]["xNome"].InnerText.ToString());

                try
                {
                    dest_cnpj = elemNF.Item(0).ChildNodes[2]["CNPJ"].InnerText.ToString();
                }
                catch (Exception)
                {
                    dest_cnpj = elemNF.Item(0).ChildNodes[2]["CPF"].InnerText.ToString();
                }

                sql += "INSERT INTO NFe_XML (ID, ide_nNF, emit_CNPJ, emit_xNome, dest_CNPJ, ide_dEmi, ide_natOp) VALUES (";
                sql += "'" + ID + "'";
                sql += ", " + ide_nNF.ToString();
                sql += ", '" + emit_cnpj + "'";
                sql += ", '" + emit_xnome + "'";
                sql += ", '" + dest_cnpj + "'";
                sql += ", '" + ide_dEmi.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                sql += ", '" + ide_natOperacao + "'";
                sql += ")";
                //Conexao.executarSql(sql);
                //ide_nNF =Funcoes.intTry(elemNF.Item(0).ChildNodes[0]["nNF"].InnerText.ToString());
                //ide_sEmi = elemNF.Item(0).ChildNodes[0]["dhEmi"].InnerText.ToString();
                //ide_dEmi = Convert.ToDateTime(ide_sEmi.ToString().Substring(0, 10) + " " + ide_sEmi.ToString().Substring(11, 8) + ".000");
                //emit_cnpj = elemNF.Item(0).ChildNodes[1]["CNPJ"].InnerText.ToString();
                //emit_xnome = Funcoes.RemoverAcentos(elemNF.Item(0).ChildNodes[1]["xNome"].InnerText.ToString());

                emit_CRT = Funcoes.intTry(elemNF.Item(0).ChildNodes[1]["CRT"].InnerText.ToString());

                //dest_cnpj = elemNF.Item(0).ChildNodes[2]["CNPJ"].InnerText.ToString();

                sql += "INSERT INTO NFe_XML (ID, ide_nNF, emit_CNPJ, emit_xNome, emit_CRT, dest_CNPJ, ide_dEmi) VALUES (";
                sql += "'" + ID + "'";
                sql += ", " + ide_nNF.ToString();
                sql += ", '" + emit_cnpj + "'";
                sql += ", ''";
                sql += ", " + emit_CRT.ToString();
                sql += ", '" + dest_cnpj + "'";
                sql += ", '" + ide_dEmi.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                sql += ")";
                //Conexao.executarSql(sql);                //Itens
                XmlNodeList elemDET = docXML.GetElementsByTagName("det");
                for (int i = 0; i < elemDET.Count; i++)
                {
                    Item item = new Item();
                    item.ID = ID;
                    item.det_nItem = Funcoes.intTry(elemDET[i].Attributes["nItem"].InnerText.ToString());
                    item.ide_nNF = ide_nNF;
                    item.ide_dEmi = ide_dEmi;
                    item.emit_cnpj = emit_cnpj;
                    item.dest_cnpj = dest_cnpj;

                    foreach (XmlElement xnProd in elemDET.Item(i).ChildNodes)
                    {
                        if (xnProd.Name.ToString() == "prod")
                        {
                            item.det_prod_cProd = xnProd["cProd"].InnerText.ToString();
                            
                            item.det_prod_xProd = Funcoes.RemoverAcentos(xnProd["xProd"].InnerText.ToString());
                            item.det_prod_NCM = xnProd["NCM"].InnerText.ToString();
                            item.det_prod_EXTIPI = strValor(xnProd, "EXTIPI");
                            item.det_prod_genero = Funcoes.intTry(strValor(xnProd, "genero"));
                            item.det_prod_CFOP = Funcoes.intTry(strValor(xnProd, "CFOP"));
                            item.det_prod_uCOM = strValor(xnProd, "uCom"); //.InnerText.ToString(); 
                            item.det_prod_qCOM = Funcoes.decTry(strValor(xnProd, "qCom"));
                            item.det_prod_vUnCOM = Funcoes.decTry(strValor(xnProd, "vUnCom"));
                            item.det_prod_vProd = Funcoes.decTry(strValor(xnProd, "vProd"));
                            item.det_prod_cEANTrib = strValor(xnProd, "cEANTrib");
                            item.det_prod_uTrib = strValor(xnProd, "uTrib");
                            item.det_prod_qTrib = Funcoes.decTry(strValor(xnProd, "qTrib"));
                            item.det_prod_vUnTrib = Funcoes.decTry(strValor(xnProd, "vUnTrib"));
                            item.det_prod_vFrete = Funcoes.decTry(strValor(xnProd, "vFrete"));
                            item.det_prod_vOutro = Funcoes.decTry(strValor(xnProd, "vOutro"));
                            item.det_prod_vSeg = Funcoes.decTry(strValor(xnProd, "vSeg"));
                            item.det_prod_vDesc = Funcoes.decTry(strValor(xnProd, "vDesc"));
                            item.det_prod_DI = strValor(xnProd, "NCM");
                            item.det_prod_DetEspecifico = strValor(xnProd, "NCM");
                        }
                        else if (xnProd.Name.ToString() == "imposto")
                        {
                            foreach (XmlElement xnImposto in xnProd.ChildNodes)
                            {
                                switch (xnImposto.Name)
                                {
                                    case "ICMS":
                                        switch (xnImposto.ChildNodes.Item(0).Name)
                                        {
                                            case "ICMS00":
                                            case "ICMS10":
                                            case "ICMS20":
                                            case "ICMS30":
                                            case "ICMS40":
                                            case "ICMS51":
                                            case "ICMS60":
                                            case "ICMS70":
                                            case "ICMS90":
                                            case "ICMSPart":
                                            case "ICMSST":
                                                item.det_icms_orig = Funcoes.intTry(strValor(xnImposto, "orig", 1));
                                                item.det_icms_CST = strValor(xnImposto, "CST", 1);
                                                item.det_icms_modBC = Funcoes.intTry(strValor(xnImposto, "modBC", 1));
                                                item.det_icms_vBC = Funcoes.decTry(strValor(xnImposto, "vBC", 1));
                                                item.det_icms_pRedBC = Funcoes.decTry(strValor(xnImposto, "pRedBC", 1));
                                                item.det_icms_pICMS = Funcoes.decTry(strValor(xnImposto, "pICMS", 1));
                                                item.det_icms_vICMS = Funcoes.decTry(strValor(xnImposto, "vICMS", 1));
                                                item.det_icms_modBCST = Funcoes.intTry(strValor(xnImposto, "modBCST", 1));
                                                item.det_icms_pMVAST = Funcoes.decTry(strValor(xnImposto, "pMVAST", 1));
                                                item.det_icms_pRedBCST = Funcoes.decTry(strValor(xnImposto, "pRedBCST", 1));
                                                item.det_icms_vBCST = Funcoes.decTry(strValor(xnImposto, "vBCST", 1));
                                                item.det_icms_pICMSST = Funcoes.decTry(strValor(xnImposto, "pICMSST", 1));
                                                item.det_icms_vICMSST = Funcoes.decTry(strValor(xnImposto, "vICMSST", 1));
                                                item.det_icms_vBCFCPST = Funcoes.decTry(strValor(xnImposto, "vBCFCPST", 1));
                                                item.det_icms_pFCPST = Funcoes.decTry(strValor(xnImposto, "pFCPST", 1));
                                                item.det_icms_vFCPST = Funcoes.decTry(strValor(xnImposto, "vFCPST", 1));
                                                item.det_icms_vBCFCP = Funcoes.decTry(strValor(xnImposto, "vBCFCP", 1));
                                                item.det_icms_pFCP = Funcoes.decTry(strValor(xnImposto, "pFCP", 1));
                                                item.det_icms_vFCP = Funcoes.decTry(strValor(xnImposto, "vFCP", 1));

                                                break;
                                            case "ICMSSN101":
                                            case "ICMSSN102":
                                            case "ICMSSN201":
                                            case "ICMSSN202":
                                            case "ICMSSN500":
                                            case "ICMSSN900":
                                                item.det_icms_orig = Funcoes.intTry(strValor(xnImposto, "orig", 1));
                                                item.det_icms_CST = strValor(xnImposto, "CST", 1);
                                                item.det_icms_CSOSN = Funcoes.intTry(strValor(xnImposto, "CSOSN", 1));
                                                item.det_icms_modBC = Funcoes.intTry(strValor(xnImposto, "modBC", 1));
                                                item.det_icms_vBC = Funcoes.decTry(strValor(xnImposto, "vBC", 1));
                                                item.det_icms_pRedBC = Funcoes.decTry(strValor(xnImposto, "pRedBC", 1));
                                                item.det_icms_pICMS = Funcoes.decTry(strValor(xnImposto, "pICMS", 1));
                                                item.det_icms_vICMS = Funcoes.decTry(strValor(xnImposto, "vICMS", 1));
                                                item.det_icms_modBCST = Funcoes.intTry(strValor(xnImposto, "modBCST", 1));
                                                item.det_icms_pMVAST = Funcoes.decTry(strValor(xnImposto, "pMVAST", 1));
                                                item.det_icms_pRedBCST = Funcoes.decTry(strValor(xnImposto, "pRedBCST", 1));
                                                item.det_icms_vBCST = Funcoes.decTry(strValor(xnImposto, "vBCST", 1));
                                                item.det_icms_pICMSST = Funcoes.decTry(strValor(xnImposto, "pICMSST", 1));
                                                item.det_icms_vICMSST = Funcoes.decTry(strValor(xnImposto, "vICMSST", 1));
                                                item.det_icms_pCredSN = Funcoes.decTry(strValor(xnImposto, "pCredSN", 1));
                                                item.det_icms_vCredICMSSN = Funcoes.decTry(strValor(xnImposto, "vCredICMSSN", 1));
                                                item.det_icms_vBCFCPST = Funcoes.decTry(strValor(xnImposto, "vBCFCPST", 1));
                                                item.det_icms_pFCPST = Funcoes.decTry(strValor(xnImposto, "pFCPST", 1));
                                                item.det_icms_vFCPST = Funcoes.decTry(strValor(xnImposto, "vFCPST", 1));
                                                item.det_icms_vBCFCP = Funcoes.decTry(strValor(xnImposto, "vBCFCP", 1));
                                                item.det_icms_pFCP = Funcoes.decTry(strValor(xnImposto, "pFCP", 1));
                                                item.det_icms_vFCP = Funcoes.decTry(strValor(xnImposto, "vFCP", 1));
                                                break;
                                        }
                                        break;
                                    case "IPI":
                                        item.det_ipi_clEnq = strValor(xnImposto, "clEnq");
                                        item.det_ipi_CNPJProd = strValor(xnImposto, "CNPJProd");
                                        item.det_ipi_cSelo = strValor(xnImposto, "cSelo");
                                        item.det_ipi_qSelo = Funcoes.intTry(strValor(xnImposto, "qSelo"));
                                        item.det_ipi_cEnq = strValor(xnImposto, "cEnq");
                                        item.det_ipi_CST = strValor(xnImposto, "CST", 2);
                                        item.det_ipi_vBC = Funcoes.decTry(strValor(xnImposto, "vBC", 2));
                                        item.det_ipi_pIPI = Funcoes.decTry(strValor(xnImposto, "pIPI", 2));
                                        item.det_ipi_qUnid = Funcoes.decTry(strValor(xnImposto, "qUnid", 2));
                                        item.det_ipi_vUnid = Funcoes.decTry(strValor(xnImposto, "vUnid", 2));
                                        item.det_ipi_vIPI = Funcoes.decTry(strValor(xnImposto, "vIPI", 2));
                                        break;
                                    case "PIS":
                                        item.det_pis_CST = strValor(xnImposto, "CST", 1);
                                        item.det_pis_vBC = Funcoes.decTry(strValor(xnImposto, "vBC", 1));
                                        item.det_pis_pPIS = Funcoes.decTry(strValor(xnImposto, "pPIS", 1));
                                        item.det_pis_qBCProd = Funcoes.decTry(strValor(xnImposto, "qBCProd", 1));
                                        item.det_pis_vAliqProd = Funcoes.decTry(strValor(xnImposto, "vAliqProd", 1));
                                        item.det_pis_vPIS = Funcoes.decTry(strValor(xnImposto, "vPIS", 1));
                                        break;
                                    case "COFINS":
                                        item.det_cofins_CST = strValor(xnImposto, "CST", 1);
                                        item.det_cofins_vBC = Funcoes.decTry(strValor(xnImposto, "vBC", 1));
                                        item.det_cofins_pCOFINS = Funcoes.decTry(strValor(xnImposto, "pCOFINS", 1));
                                        item.det_cofins_qBCProd = Funcoes.decTry(strValor(xnImposto, "qBCProd", 1));
                                        item.det_cofins_vAliqProd = Funcoes.decTry(strValor(xnImposto, "vAliqProd", 1));
                                        item.det_cofins_vCOFINS = Funcoes.decTry(strValor(xnImposto, "vCOFINS", 1));
                                        break;
                                }
                            }
                            //Fora do laço de leitura dos impostos. Checar se há redução de base de cálculo ST
                            if (item.det_icms_CST == "70")
                            {
                                if (item.det_icms_modBCST != 5)
                                {
                                    if ((item.det_prod_vProd - item.det_prod_vDesc) > 0 && item.det_icms_pMVAST > 0)
                                    {
                                        //Calcula considerando que não há redução da base de icms st
                                        decimal baseCalculo = (item.det_prod_vProd - item.det_prod_vDesc + item.det_ipi_vIPI) * (1 + (item.det_icms_pMVAST / 100));
                                        //Caso seja diferente, mostra que há redução e por isso devemos atribuir o valor da redução de bc icms no bc icms st.
                                        if ((baseCalculo - item.det_icms_vBCST) > 0.01m || (baseCalculo - item.det_icms_vBCST) < -0.01m)
                                        {
                                            item.det_icms_pRedBCST = item.det_icms_pRedBC;
                                        }
                                        else
                                        {
                                            item.det_icms_pRedBCST = 0;
                                        }
                                    }
                                }
                            }

                        }
                    }
                    sql += item.Insert();
                }
                //Inserindo o Total
                XmlNodeList elemTOTAL = docXML.GetElementsByTagName("ICMSTot");
                Total_ICMSTot_vBC = Funcoes.decTry(strValorDBL(elemTOTAL, "vBC"));
                Total_ICMSTot_vICMS = Funcoes.decTry(strValorDBL(elemTOTAL, "vICMS"));
                Total_ICMSTot_vBCST = Funcoes.decTry(strValorDBL(elemTOTAL, "vBCST"));
                Total_ICMSTot_vST = Funcoes.decTry(strValorDBL(elemTOTAL, "vST"));
                Total_ICMSTot_vProd = Funcoes.decTry(strValorDBL(elemTOTAL, "vProd"));
                Total_ICMSTot_vFrete = Funcoes.decTry(strValorDBL(elemTOTAL, "vFrete"));
                Total_ICMSTot_vSeg = Funcoes.decTry(strValorDBL(elemTOTAL, "vSeg"));
                Total_ICMSTot_vDesc = Funcoes.decTry(strValorDBL(elemTOTAL, "vDesc"));
                Total_ICMSTot_vII = Funcoes.decTry(strValorDBL(elemTOTAL, "vII"));
                Total_ICMSTot_vIPI = Funcoes.decTry(strValorDBL(elemTOTAL, "vIPI"));
                Total_ICMSTot_vPIS = Funcoes.decTry(strValorDBL(elemTOTAL, "vPIS"));
                Total_ICMSTot_vCOFINS = Funcoes.decTry(strValorDBL(elemTOTAL, "vCOFINS"));
                Total_ICMSTot_vOutro = Funcoes.decTry(strValorDBL(elemTOTAL, "vOutro"));
                Total_ICMSTot_vICMSDeson = Funcoes.decTry(strValorDBL(elemTOTAL, "vICMSDeson"));
                Total_ICMSTot_vFCP = Funcoes.decTry(strValorDBL(elemTOTAL, "vFCP"));
                Total_ICMSTot_vFCPST = Funcoes.decTry(strValorDBL(elemTOTAL, "vFCPST"));
                Total_ICMSTot_vFCPSTRet = Funcoes.decTry(strValorDBL(elemTOTAL, "vFCPSTRet"));
                Total_ICMSTot_vNF = Funcoes.decTry(strValorDBL(elemTOTAL, "vNF"));
                Total_ICMSTot_vTotTrib = Funcoes.decTry(strValorDBL(elemTOTAL, "vTotTrib"));

                sql += " INSERT INTO NFe_XML (ID, ide_nNF, emit_CNPJ, dest_CNPJ, ide_dEmi";
                sql += ", Total_ICMSTot_vBC";
                sql += ", Total_ICMSTot_vICMS";
                sql += ", Total_ICMSTot_vBCST";
                sql += ", Total_ICMSTot_vST";
                sql += ", Total_ICMSTot_vProd";
                sql += ", Total_ICMSTot_vFrete";
                sql += ", Total_ICMSTot_vSeg";
                sql += ", Total_ICMSTot_vDesc";
                sql += ", Total_ICMSTot_vII";
                sql += ", Total_ICMSTot_vIPI";
                sql += ", Total_ICMSTot_vPIS";
                sql += ", Total_ICMSTot_vCOFINS";
                sql += ", Total_ICMSTot_vOutro";
                sql += ", Total_ICMSTot_vICMSDeson";
                sql += ", Total_ICMSTot_vFCP";
                sql += ", Total_ICMSTot_vFCPST";
                sql += ", Total_ICMSTot_vFCPSTRet";
                sql += ", Total_ICMSTot_vNF";
                sql += ", Total_ICMSTot_vTotTrib";
                sql += ") VALUES (";
                sql += "'" + ID + "'";
                sql += ", " + ide_nNF.ToString();
                sql += ", '" + emit_cnpj + "'";
                sql += ", '" + dest_cnpj + "'";
                sql += ", '" + ide_dEmi.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                sql += ", " + Total_ICMSTot_vBC.ToString().Replace(",", ".");
                sql += ", " + Total_ICMSTot_vICMS.ToString().Replace(",", ".");
                sql += ", " + Total_ICMSTot_vBCST.ToString().Replace(",", ".");
                sql += ", " + Total_ICMSTot_vST.ToString().Replace(",", ".");
                sql += ", " + Total_ICMSTot_vProd.ToString().Replace(",", ".");
                sql += ", " + Total_ICMSTot_vFrete.ToString().Replace(",", ".");
                sql += ", " + Total_ICMSTot_vSeg.ToString().Replace(",", ".");
                sql += ", " + Total_ICMSTot_vDesc.ToString().Replace(",", ".");
                sql += ", " + Total_ICMSTot_vII.ToString().Replace(",", ".");
                sql += ", " + Total_ICMSTot_vIPI.ToString().Replace(",", ".");
                sql += ", " + Total_ICMSTot_vPIS.ToString().Replace(",", ".");
                sql += ", " + Total_ICMSTot_vCOFINS.ToString().Replace(",", ".");
                sql += ", " + Total_ICMSTot_vOutro.ToString().Replace(",", ".");
                sql += ", " + Total_ICMSTot_vICMSDeson.ToString().Replace(",", ".");
                sql += ", " + Total_ICMSTot_vFCP.ToString().Replace(",", ".");
                sql += ", " + Total_ICMSTot_vFCPST.ToString().Replace(",", ".");
                sql += ", " + Total_ICMSTot_vFCPSTRet.ToString().Replace(",", ".");
                sql += ", " + Total_ICMSTot_vNF.ToString().Replace(",", ".");
                sql += ", " + Total_ICMSTot_vTotTrib.ToString().Replace(",", ".");
                sql += "); ";


                try
                {
                    XmlNodeList elemCOBR = docXML.GetElementsByTagName("dup");
                    for (int f = 0; f < elemCOBR.Count; f++)
                    {
                        cobr_dup_ndup = elemCOBR.Item(f).ChildNodes[0].InnerText.ToString();
                        cobr_dup_dVenc = Convert.ToDateTime(elemCOBR.Item(f).ChildNodes[1].InnerText.ToString());
                        cobr_dup_vDup = Funcoes.decTry(elemCOBR.Item(f).ChildNodes[2].InnerText.ToString().Replace(".", ","));

                        sql += " INSERT INTO NFe_XML (ID, ide_nNF, emit_CNPJ, dest_CNPJ, ide_dEmi";
                        sql += ", cobr_dup_ndup";
                        sql += ", cobr_dup_dVenc";
                        sql += ", cobr_dup_vDup";
                        sql += ") VALUES (";
                        sql += "'" + ID + "'";
                        sql += ", " + ide_nNF.ToString();
                        sql += ", '" + emit_cnpj + "'";
                        sql += ", '" + dest_cnpj + "'";
                        sql += ", '" + ide_dEmi.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        sql += ", '" + cobr_dup_ndup + "'";
                        sql += ", '" + cobr_dup_dVenc.ToString("yyyy-MM-dd") + "'";
                        sql += ", " + Funcoes.decimalPonto(cobr_dup_vDup.ToString());
                        sql += "); ";
                      
                    }
                    if (salvar)
                        Conexao.executarSql(sql);

                    
                }
                catch (Exception e)
                {
                    throw e;//Program.GravarLog("Importador(Classe XML) - Duplicatas", sql, e.Message.ToString());
                }
                return sql;
            }
            catch (Exception e)
            {
                throw e;

            }
        }
        private static string strValor(XmlElement valor, string Campo, int Child = 0)
        {
            string Retorno = "";
            try
            {
                if (Child == 1)
                {
                    Retorno = valor.ChildNodes[0][Campo].InnerText.ToString();
                }
                else if (Child == 2)
                {
                    bool ipi = false;
                    for (int i = 0; i < valor.ChildNodes.Count; i++)
                    {
                        if (valor.ChildNodes[i].Name.Equals("IPITrib"))
                        {
                            ipi = true;
                            if (valor.ChildNodes[i].ChildNodes.Count > 0)
                            {
                                Retorno = valor.ChildNodes[i][Campo].InnerText.ToString();
                                break;
                            }

                        }
                    }
                    if (!ipi)
                    {
                        Retorno = valor.ChildNodes[1][Campo].InnerText.ToString();
                    }
                }
                else
                {
                    Retorno = valor[Campo].InnerText.ToString();
                }
                if (Funcoes.isnumero(Retorno))
                    Retorno = Retorno.Replace(".", ",");
                return Retorno;
            }
            catch
            {
                return "";
            }
        }
        private static string strValorDBL(XmlNodeList valor, string Campo)
        {
            string Retorno = "0";
            try
            {
                Retorno = valor[0][Campo].InnerText.ToString().Replace(",","").Replace(".",",");
                return Retorno;
            }
            catch
            {
                return "0";
            }
        }
        public static NfManifestoDAO NfeManifesto(string Arquivo,User usr)
        {
            XmlDocument docXML = new XmlDocument();
            docXML.LoadXml(Arquivo);
            XmlNodeList elemNF = docXML.GetElementsByTagName("infNFe");
            String chave = elemNF.Item(0).Attributes["Id"].InnerText.ToString();
            NfManifestoDAO NF = new NfManifestoDAO(chave, usr);

            if (!NF.existeBD())
            {
                XmlNodeList elemTOTAL = docXML.GetElementsByTagName("ICMSTot");
                NF.vNF = Funcoes.decTry(strValorDBL(elemTOTAL, "vNF"));
                NF.CNPJ = elemNF.Item(0).ChildNodes[1]["CNPJ"].InnerText.ToString();
                NF.RazaoSocial = Funcoes.RemoverAcentos(elemNF.Item(0).ChildNodes[1]["xNome"].InnerText.ToString());
                NF.Emissao = Funcoes.dtTry(elemNF.Item(0).ChildNodes[0]["dhEmi"].InnerText.ToString());
                NF.naturezaOperacao = elemNF.Item(0).ChildNodes[0]["natOp"].InnerText.ToString();
                NF.Status = "CIENCIA OPERACAO";
            }

            NF.NfeXml = Arquivo;
            
            return NF;
         }
    }   
}
