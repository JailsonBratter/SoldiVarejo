using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.NotaFiscal.code
{
    public class NfItemImportaDev
    {
        public string plu { get; set; }
        public string descricao { get; set; }
        public string qtde { get; set; }
        public string unitario { get; set; }
        public int item { get; set; }
        public string total { get; set; }



        public static List<NfItemImportaDev> pesquisa(String chave, User usr)
        {
            List<NfItemImportaDev> list = new List<NfItemImportaDev>();
            SqlDataReader rsItens = null;
            try
            {
                String strSqlItens = "Exec sp_NFe_det 'NFE" + chave + "'";
                rsItens = Conexao.consulta(strSqlItens, null, false);
                string Cliente_Fornecedor = destinatario(chave);

                while (rsItens.Read())
                {
                    NfItemImportaDev item = new NfItemImportaDev();
                    String strPlu = "";
                    String strSqlplu = "";
                    if (rsItens["det_prod_cEAN"] != null && rsItens["det_prod_cEAN"].ToString().Trim().Length > 0)
                    {
                        //String teste = rsItens["det_prod_cEAN"].ToString();
                        long cEan = 0;
                        long.TryParse(rsItens["det_prod_cEAN"].ToString(), out cEan);
                        strSqlplu = "SELECT TOP 1 plu FROM EAN WHERE EAN.EAN ='" + cEan + "'";
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
                    item.plu = strPlu;
                    item.descricao = Funcoes.RemoverAcentos(rsItens["det_prod_xProd"].ToString());
                    item.qtde = Funcoes.decTry(rsItens["det_prod_qCom"].ToString()).ToString("N2");
                    item.unitario = Funcoes.decTry(rsItens["det_prod_vUnCom"].ToString()).ToString("N2");
                    item.total = Funcoes.decTry(rsItens["det_prod_vProd"].ToString()).ToString("N2");
                    item.item = Funcoes.intTry(rsItens["det_nItem"].ToString());
                    list.Add(item);
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rsItens != null)
                    rsItens.Close();
            }
            if (list.Count == 0)
                throw new Exception("Sem Itens");

            return list;
        }

        private static string destinatario(string chave)
        {
            String dest = "";
            String Cnpj_Cpf = Conexao.retornaUmValor("select  emit_CNPJ = isnull(emit_CNPJ,emit_CPF) FROM NFe_XML where id LIKE'%NFe" + chave + "' and((emit_CNPJ is not null and emit_CNPJ <>'' ) or emit_CPF is Not null) group by emit_CNPJ,emit_CPF", null);
            SqlDataReader rsFornecedor = null;
            try
            {
                rsFornecedor = Conexao.consulta("SELECT * FROM Fornecedor WHERE replace(replace(replace(CNPJ,'.',''),'/',''),'-','')='" + Cnpj_Cpf + "'", null, false);
                if (rsFornecedor.Read())
                {
                    dest = rsFornecedor["Fornecedor"].ToString();
                }
                else
                {
                    throw new Exception("Fornecedor não encontrado");
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rsFornecedor != null)
                    rsFornecedor.Close();
            }


            return dest;
        }
    }
}