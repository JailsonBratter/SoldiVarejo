using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using visualSysWeb.dao;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Collections;
using System.Runtime.InteropServices;
using System.Net.Mime;
using System.Web.UI.WebControls;

namespace visualSysWeb.code
{
    public class Funcoes
    {
        public static String diretorioServ = "";
        private static Hashtable parametros = new Hashtable();

        public static decimal porcDesconto(Decimal preco, Decimal desconto)
        {
            Decimal nValorDesc = (preco - desconto);
            
            if (nValorDesc != 0)
            {
                Decimal porc = (nValorDesc / preco) * 100;
                return porc ;
            }
            return 0;
        }

        public static decimal valorProduto(string plu ,decimal qtde, string cod_cliente,User usr)
        {
            decimal unitario = 0;
            decimal qtdeAtacado = Funcoes.decTry(
                        Conexao.retornaUmValor("Select qtde_atacado from mercadoria where plu ='" + plu + "'", null)
                      );
            if (qtdeAtacado > 0  && qtde >= qtdeAtacado)
            {
                unitario = Funcoes.decTry(
                    Conexao.retornaUmValor("Select preco_atacado from mercadoria where plu ='" + plu + "'", null)
                );
            }
            else
            {

                bool ativaTerceiroPreco = Conexao.retornaUmValor("Select ativa_terceiro_preco from cliente where codigo_cliente = '" + cod_cliente + "'", null).Equals("1");
                if (ativaTerceiroPreco)
                {
                    unitario = Funcoes.decTry(
                       Conexao.retornaUmValor("Select terceiro_preco from mercadoria where plu ='" + plu + "'", null)
                    );
                }
                if (unitario == 0)
                {
                    string tabDesc = Conexao.retornaUmValor("Select codigo_tabela from cliente where codigo_cliente = '" + cod_cliente + "'", null);

                    if (!tabDesc.Equals("") && !tabDesc.Equals("0"))
                    {
                        unitario = Funcoes.decTry(Conexao.retornaUmValor("Select Preco_promocao from Preco_Mercadoria where plu ='" + plu + "' and Codigo_tabela = '" + tabDesc + "'", null).ToString());
                    }

                }

                if (unitario == 0)
                {
                    unitario = Funcoes.decTry(
                        Conexao.retornaUmValor("Select CASE WHEN (convert(date,getdate()) between  Mercadoria_loja.Data_Inicio AND Mercadoria_loja.Data_Fim ) AND ISNULL(Mercadoria_Loja.Preco_Promocao,0) > 0 THEN Mercadoria_loja.Preco_Promocao ELSE Mercadoria_Loja.Preco END as [Preco]" +
                        "from mercadoria inner join mercadoria_loja on mercadoria.plu=mercadoria_loja.plu " +
                        "where mercadoria.plu ='" + plu + "' and mercadoria_loja.filial='"+usr.getFilial()+"'", null)
                   );
                }

            }
            return unitario;
        }
        public static decimal arredondar(Decimal vlr,int dec, int tipo)
        {
            ///vlr = valor a ser arredondado
            ///dec = casas decimais
            ///tipo de arredondamento 
            /// 1=sem arredondado
            /// 2=truncamento
            /// 3=arredondamento 
            /// 4=arredondamento de 0.5 
            decimal vlrFinal = 0;
            switch (tipo)
            {
                case 1:
                    vlrFinal = vlr;
                    break;
                case 2:
                    decimal step = (decimal)Math.Pow(10, dec);
                    decimal tmp = Math.Truncate(step * vlr);
                    vlrFinal =  tmp / step;
                    break;
                case 3:
                    vlrFinal = Math.Round(vlr, dec);
                    break;
                case 4:
                    int intPart = (int)vlr;
                    decimal decimalPart = vlr - intPart;
                    if (0 < decimalPart && decimalPart <= 0.5m)
                        vlrFinal = intPart + 0.5m;
                    else if (0.5m < decimalPart)
                        vlrFinal = intPart + 1;
                    else
                        vlrFinal = intPart;
                    break;

            }
            return vlrFinal;
        }
        public static void atualizaSaldoPLU(String Filial, String PLU, decimal qtdeAlterar,SqlConnection conn, SqlTransaction tran, DateTime dataMovimentacao, string tipoMovimentacao, bool inventario = false)
        {
            SqlDataReader rs = null;
            try
            {
                
                rs = Conexao.consulta(@"Select tipo.Estoque, tipo.Permite_item, MERCADORIA.PLU_Vinculado, MERCADORIA.fator_Estoque_Vinculado, isnull(ML.saldo_atual,0) saldo_atual
                                        from mercadoria INNER JOIN Mercadoria_Loja ml ON mercadoria.plu = ml.PLU
                                        inner join tipo on mercadoria.Tipo = tipo.Tipo
                                        where ML.FILIAL = '" + Filial + "' AND mercadoria.plu ='" + PLU +"'", null, false);
                if (rs.Read())
                {


                    Decimal saldoAtual = decTry(rs["saldo_atual"].ToString());
                    Decimal novoSaldo = saldoAtual + qtdeAlterar;

                    if (inventario)
                    {
                        novoSaldo = qtdeAlterar;
                    }
                    
                    //if (novoSaldo < 0)
                    //    novoSaldo = 0;

                    if (rs["Estoque"].ToString().Equals("1"))
                    {
                        String sql = "update mercadoria_loja set " +
                                "   saldo_atual =  " + decimalPonto(novoSaldo) +
                                " where  filial = '" + Filial + "' AND plu =  '" + PLU + "'";

                        Conexao.executarSql(sql, conn, tran);

                        sql = "update mercadoria set  " +
                                 "saldo_atual = (select sum(isnull(saldo_atual, 0))from mercadoria_loja b where b.plu = mercadoria.plu) " +
                              "where mercadoria.plu = '" + PLU + "'";
                        Conexao.executarSql(sql, conn, tran);
                    }
                    if (rs["Permite_item"].ToString().Equals("1"))
                    {
                        SqlDataReader rsItens = null;
                        try
                        {

                            rsItens = Conexao.consulta("Select * from item where plu = '" + PLU + "'", null, false);


                            while (rsItens.Read())
                            {
                                String sql = "";
                                //String sql = "update mercadoria_loja set " +
                                // "   saldo_atual = isnull(saldo_atual,0) + (" + decimalPonto(qtdeAlterar) + " * " + decimalPonto(decTry(rsItens["fator_conversao"].ToString())) + ") " +
                                // " where plu =  '" + rsItens["Plu_item"].ToString() + "' and filial = '" + Filial + "';";

                                //Conexao.executarSql(sql, conn, tran);

                                //sql = "update mercadoria set  " +
                                //            "saldo_atual = (select sum(isnull(saldo_atual, 0))from mercadoria_loja b where b.plu = mercadoria.plu) " +
                                //        "where mercadoria.plu = '" + rsItens["Plu_item"].ToString() + "'";
                                //Conexao.executarSql(sql, conn, tran);

                                atualizaSaldoPLU(Filial, rsItens["Plu_item"].ToString(), (qtdeAlterar * decTry(rsItens["fator_conversao"].ToString())), conn, tran, dataMovimentacao, tipoMovimentacao);
                                atualizaSaldoPLUDia(Filial, rsItens["Plu_item"].ToString(), (qtdeAlterar * decTry(rsItens["fator_conversao"].ToString())), conn, tran, tipoMovimentacao, dataMovimentacao);


                                //Inserção em outras movimentações de acordo com o item de origem
                                try
                                {
                                    using (SqlCommand cmd = new SqlCommand())
                                    {
                                        decimal qtdeAtualizar = qtdeAlterar * decTry(rsItens["fator_conversao"].ToString());
                                        cmd.Connection = conn;
                                        cmd.Transaction = tran;
                                        sql = "sp_ins_Mercadoria_Movimentacao_Relacionada ";
                                        //Adicionando parâmetros
                                        sql += " @Filial = '" + Filial + "'";
                                        sql += ", @PLU = '" + rsItens["Plu_item"].ToString() + "'";
                                        sql += ", @Data = '" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + "'";
                                        sql += ", @Qtde = " + decimalPonto(qtdeAtualizar);
                                        sql += ", @PLU_Origem = '" + PLU + "'";
                                        sql += ", @Origem = 'Movimentação com BASE na tabela ITEM. Fator: " + decimalPonto(decTry(rsItens["fator_conversao"].ToString())) + "'";
                                        cmd.CommandText = sql;
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                catch
                                {

                                }

                            }


                            String PLU_vinculado = Conexao.retornaUmValor("Select PLU_vinculado from mercadoria where plu ='" + PLU + "'", null);

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
                    }
                    if (!rs["PLU_Vinculado"].ToString().Equals(""))
                    {
                        Decimal fator = decTry(rs["fator_Estoque_Vinculado"].ToString());
                        atualizaSaldoPLU(Filial, rs["PLU_Vinculado"].ToString(), qtdeAlterar * fator, conn, tran, dataMovimentacao, tipoMovimentacao);
                        //Inserção em outras movimentações de acordo com o item de origem
                        try
                        {
                            string sql = "";
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.Transaction = tran;
                                sql = "sp_ins_Mercadoria_Movimentacao_Relacionada ";
                                //Adicionando parâmetros
                                sql += " @Filial = '" + Filial + "'";
                                sql += ", @PLU = '" + rs["PLU_Vinculado"].ToString() + "'";
                                sql += ", @Data = '" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + "'";
                                sql += ", @Qtde = " + decimalPonto(qtdeAlterar * fator);
                                sql += ", @PLU_Origem = '" + PLU + "'";
                                sql += ", @Origem = 'Movimentação com BASE no PLU VINCULADO. Fator: " + decimalPonto(fator) + "'";
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch
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
                if (rs != null)
                    rs.Close();
            }
        }
        public static dynamic leJson(String caminho)
        {
            dynamic jObj;
            using (StreamReader r = new StreamReader(diretorioServ + "/" + caminho))
            {
                string json = r.ReadToEnd();
                r.Close();
                jObj = System.Web.Helpers.Json.Decode(json);
            }
            return jObj;
        }
        public static void SetDDLValue(DropDownList ddl, string value)
        {
            ddl.ClearSelection();

            foreach (ListItem li in ddl.Items)

            {

                if (li.Value == value)
                {
                    li.Selected = true;
                    break;
                }
            }
        }

        public static string getNovoPlu(User usr, String codPlu)
        {
            return getNovoPlu(usr, codPlu, true);
        }
        public static string getNovoPlu(User usr, String codPlu, bool updateUsado)
        {

            bool dig = valorParametro("PLU_DIGITO_VERIFICADOR", usr).ToString().ToUpper().Equals("TRUE"); ;

            bool pluUsado = true;
            String pluFinal = "";
            while (pluUsado)
            {
                String sql = "select min(convert(decimal,plu)) plu from plu where usado=0 ";
                if (!codPlu.Trim().Equals(""))
                {
                    sql += " and plu > " + codPlu;

                }

                codPlu = Conexao.retornaUmValor(sql, null);



                if (!codPlu.Trim().Equals("") && dig)
                {
                    codPlu += digVerificador(codPlu);
                    String repo = Conexao.retornaUmValor("Select usado from plu where plu ='" + codPlu + "'", usr);
                    pluUsado = repo.Equals("1");
                    if (!pluUsado)
                    {
                        pluFinal = codPlu;
                    }
                }
                else
                {
                    pluFinal = codPlu;
                    pluUsado = false;
                }


                if (pluFinal.Trim().Length > 6)
                {
                    pluFinal = "";
                    throw new Exception("PLU " + codPlu + " INVÁLIDO, NÃO PODE TER MAIS QUE 6 DIG.");
                }


            }
            if (updateUsado)
            {
                if (!pluFinal.Equals(""))
                {

                    Conexao.executarSql("update plu set usado=1 where plu=" + pluFinal);
                    if (dig)
                    {
                        Conexao.executarSql("update plu set usado=1 where plu=" + pluFinal.Substring(0, codPlu.Length - 1));
                    }
                }
            }

            return pluFinal;
        }

        public static void cancelaPluUsado(String plu)
        {
            String existe = Conexao.retornaUmValor("select plu from mercadoria where plu='" + plu + "'", new User());
            if (existe.Trim().Equals(""))
            {
                Conexao.executarSql("update plu set usado=0 where plu=" + plu);
            }
        }
        public static decimal precoMargem(Decimal precoCusto, Decimal margem)
        {
            return (precoCusto + (precoCusto * margem / 100));
        }
        public static decimal valorMargem(Decimal precoCusto, Decimal precoVenda)
        {
            return ((precoVenda - precoCusto) / precoCusto) * 100;
        }
        public static void SetDDLText(DropDownList ddl, string value)
        {
            ddl.ClearSelection();

            foreach (ListItem li in ddl.Items)

            {

                if (li.Text == value)
                {
                    li.Selected = true;
                    break;
                }
            }
        }
        public static DateTime DiaUtil(DateTime data)
        {
            bool loopContinua = true;
            while (loopContinua)
            {
                loopContinua = false;
                int feriado = intTry(Conexao.retornaUmValor("Select count(*) from feriado where dia ='" + data.ToString("yyyyMMdd") + "'", null));
                if (feriado > 0)
                {
                    data = data.AddDays(1);
                    loopContinua = true;
                }

                if (data.DayOfWeek.Equals(DayOfWeek.Saturday))
                {
                    data = data.AddDays(2);
                    loopContinua = true;
                }

                if (data.DayOfWeek.Equals(DayOfWeek.Sunday))
                {
                    data = data.AddDays(1);
                    loopContinua = true;
                }

            }



            return data;
        }


        public static DateTime fatorVencimento(int dias)
        {
            return new DateTime(1997, 10, 7).AddDays(dias);
        }
        public static Decimal decTry(String vlr)
        {
            decimal vVlr = 0;
            Decimal.TryParse(vlr, out vVlr);
            return vVlr;
        }

        public static int intTry(String vlr)
        {
            int vVlr = 0;
            int.TryParse(vlr, out vVlr);
            return vVlr;
        }


        public static DateTime dtTry(String dt)
        {
            DateTime vdt = new DateTime();
            DateTime.TryParse(dt, out vdt);
            return vdt;
        }
        public static string RemoverAcentos(string texto)
        {
            if (texto == null)
                return ""
;
            string s = texto.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int k = 0; k < s.Length; k++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(s[k]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(s[k]);
                }
            }
            String ret = sb.ToString().Replace("&", "E")
                                      .Replace("'", "")
                                      .Replace("^", "")
                                      .Replace("~", "")
                                      .Replace("`", "")
                                      .Replace("@", "")
                                      .Replace("°", "")
                                      .Replace("\"", "")
                                      .Replace("$", "")
                                      .Replace("´", "")
                                      .Replace("#", "")
                                      .Replace("*", "")
                                      .Replace("¢", "")
                                      .Replace("£", "")
                                      .Replace("º", "")
                                      .Replace("ª", "")
                                      .Replace("\u000A", "") //Line Fedd (nova linha)
                                      .Replace("\u000D", "") //Carriage return (Enter)
                                      .Replace("\u0009", "") //Horizontal TAB
                                      .Replace("\u000B", "") //Vertical TAB
                                      .Replace("¨", "");


            byte[] rawBytes = System.Text.Encoding.ASCII.GetBytes(ret);

            int i = 0;
            foreach (byte item in rawBytes)
            {
                if (!hexaValido(item))
                {
                    string cr = ret.Substring(i, 1);
                    ret = ret.Replace(cr, " ");
                }
                i++;

            }
            String strFinal = "";
            bool bEspaco = valorParametro("PERMITE_DUPLO_ESP", null).ToUpper().Equals("TRUE");
            if (bEspaco)
                strFinal = ret;
            else
                strFinal = ret.Replace("  ", " ").Trim();


            return strFinal;
        }
        public static String decimalPonto(Decimal vlr)
        {
            return decimalPonto(vlr.ToString());
        }
        public static String decimalPonto(String vlr)
        {
            String milhar = ".";
            String decim = ",";

            int virg = vlr.IndexOf(",");
            int ponto = vlr.IndexOf(".");

            if ((virg >= 0 && ponto >= 0) && (virg < ponto))
            {
                milhar = ",";
                decim = ".";
            }
            else if (virg < 0 && ponto >= 0)
            {
                milhar = ",";
                decim = ".";
            }

            return vlr.Replace(milhar, "").Replace(decim, ".");
        }

        public static String dateSql(DateTime dt)
        {
            return (dt.Equals(DateTime.MinValue) ? "null" : "'" + dt.ToString("yyyy-MM-dd") + "'");
        }

        public static String dataBr(DateTime dt)
        {
            if (dt.Equals(DateTime.MinValue))
            {
                return "";
            }
            else
            {
                return dt.ToString("dd/MM/yyyy");
            }
        }
        private static bool hexaValido(byte c)
        {
            Hashtable tbHexa = new Hashtable();
            tbHexa.Add("65", "A");
            tbHexa.Add("66", "B");
            tbHexa.Add("67", "C");
            tbHexa.Add("68", "D");
            tbHexa.Add("69", "E");
            tbHexa.Add("70", "F");
            tbHexa.Add("71", "G");
            tbHexa.Add("72", "H");
            tbHexa.Add("73", "I");
            tbHexa.Add("74", "J");
            tbHexa.Add("75", "K");
            tbHexa.Add("76", "L");
            tbHexa.Add("77", "M");
            tbHexa.Add("78", "N");
            tbHexa.Add("79", "O");
            tbHexa.Add("80", "P");
            tbHexa.Add("81", "Q");
            tbHexa.Add("82", "R");
            tbHexa.Add("83", "S");
            tbHexa.Add("84", "T");
            tbHexa.Add("85", "U");
            tbHexa.Add("86", "V");
            tbHexa.Add("87", "W");
            tbHexa.Add("88", "X");
            tbHexa.Add("89", "Y");
            tbHexa.Add("90", "Z");
            tbHexa.Add("97", "a");
            tbHexa.Add("98", "b");
            tbHexa.Add("99", "c");
            tbHexa.Add("100", "d");
            tbHexa.Add("101", "e");
            tbHexa.Add("102", "f");
            tbHexa.Add("103", "g");
            tbHexa.Add("104", "h");
            tbHexa.Add("105", "i");
            tbHexa.Add("106", "j");
            tbHexa.Add("107", "k");
            tbHexa.Add("108", "l");
            tbHexa.Add("109", "m");
            tbHexa.Add("110", "n");
            tbHexa.Add("111", "o");
            tbHexa.Add("112", "p");
            tbHexa.Add("113", "q");
            tbHexa.Add("114", "r");
            tbHexa.Add("115", "s");
            tbHexa.Add("116", "t");
            tbHexa.Add("117", "u");
            tbHexa.Add("118", "v");
            tbHexa.Add("119", "w");
            tbHexa.Add("120", "x");
            tbHexa.Add("121", "y");
            tbHexa.Add("122", "z");
            tbHexa.Add("48", "0");
            tbHexa.Add("49", "1");
            tbHexa.Add("50", "2");
            tbHexa.Add("51", "3");
            tbHexa.Add("52", "4");
            tbHexa.Add("53", "5");
            tbHexa.Add("54", "6");
            tbHexa.Add("55", "7");
            tbHexa.Add("56", "8");
            tbHexa.Add("57", "9");
            tbHexa.Add("32", " ");
            tbHexa.Add("45", "-");
            tbHexa.Add("46", ".");
            tbHexa.Add("44", ",");
            tbHexa.Add("47", "/");
            tbHexa.Add("92", @"\");
            tbHexa.Add("40", "(");
            tbHexa.Add("41", ")");





            return tbHexa.ContainsKey(c.ToString());
        }

        public static bool log_sistema(User usr, String erro)
        {
            String sql = "insert into log_sistema (COD_TELA,NOME_TELA,USUARIO,NOME_USUARIO,DATA,ERRO)" +
                        "values(" +
                        "'" + usr.tela + "'," +
                        "'" + usr.nomeTela() + "'," +
                        "'" + usr.getUsuario() + "'," +
                        "'" + usr.getNome() + "'," +
                        "getDate()," +
                        "'" + erro + "')";

            Conexao.executarSql(sql);
            return true;
        }
        public static bool ValidarEAN13(string CodigoEAN13)
        {
            bool result = (CodigoEAN13.Length <= 13);
            if (result)
            {
                const string checkSum = "131313131313";

                int digito = int.Parse(CodigoEAN13[CodigoEAN13.Length - 1].ToString());
                string ean = CodigoEAN13.Substring(0, CodigoEAN13.Length - 1).PadLeft(12, '0');

                int sum = 0;
                for (int i = 0; i <= ean.Length - 1; i++)
                {
                    sum += int.Parse(ean[i].ToString()) * int.Parse(checkSum[i].ToString());
                }
                int resto = (sum % 10);
                int calculo = 0;

                if (resto == 0)
                    calculo = 0;
                else
                    calculo = 10 - resto;

                result = (digito == calculo);
            }
            else
            {
                if (CodigoEAN13.Length == 14)
                {
                    const string checkSum = "3131313131313";

                    int digito = int.Parse(CodigoEAN13[CodigoEAN13.Length - 1].ToString());
                    string ean = CodigoEAN13.Substring(0, CodigoEAN13.Length - 1).PadLeft(12, '0');

                    int sum = 0;
                    for (int i = 0; i <= ean.Length - 1; i++)
                    {
                        sum += int.Parse(ean[i].ToString()) * int.Parse(checkSum[i].ToString());
                    }
                    int resto = (sum % 10);
                    int calculo = 0;

                    if (resto == 0)
                        calculo = 0;
                    else
                        calculo = 10 - resto;

                    result = (digito == calculo);
                }
            }
            return result;
        }
        public static String digVerificador(string CodigoEAN13)
        {
            String dig = "";
            const string checkSum = "3131313131313";

            string ean = CodigoEAN13.PadLeft(13, '0');

            int sum = 0;
            for (int i = 0; i <= ean.Length - 1; i++)
            {
                sum += int.Parse(ean[i].ToString()) * int.Parse(checkSum[i].ToString());
            }
            int resto = (sum % 10);
            int calculo = 0;

            if (resto == 0)
                calculo = 0;
            else
                calculo = 10 - resto;

            dig = calculo.ToString();
            return dig;
        }


        public static String StrImpostoST(Decimal ipi, Decimal margemIva, Decimal aliquotaIva, Decimal BaseIcms, Decimal aliquotaIcms)
        {
            Decimal vIpi = (BaseIcms * ipi) / 100;
            Decimal vBaseIva = (BaseIcms + vIpi);
            Decimal pIva = ((vBaseIva * margemIva) / 100);
            vBaseIva += pIva;
            Decimal ValorIva = ((vBaseIva * aliquotaIva) / 100) - ((BaseIcms * aliquotaIcms) / 100);
            return " BC ST=" + vBaseIva.ToString("N2") + " Vlr ST=" + ValorIva.ToString("N2");
        }

        public static bool existePasta(String pasta)
        {

            try
            {
                StreamWriter ArqImprime = new StreamWriter(pasta + "\\teste.teste", false, Encoding.ASCII);
                ArqImprime.Write("testePasta");
                ArqImprime.Close();

                File.Delete(pasta + "\\teste.teste");


                return true;
            }
            catch (Exception)
            {
                return false;
            }



        }

        public static bool IsCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }


        public static bool IsCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

        //Sinto Muito Me Perdoe Agradeço Eu Te Amo.

        public static String sequencia(String tabelaCampo, User usr)
        {
            SqlDataReader rs = null;
            String strSequencia = "";

            try
            {


                rs = Conexao.consulta("select * from sequenciais where tabela_coluna ='" + tabelaCampo + "'", new User(), true);
                if (rs.Read())
                {
                    bool empresa = (bool)rs["PERMITE_POR_EMPRESA"];
                    if (!empresa)
                    {
                        strSequencia = (long.Parse(rs["SEQUENCIA"].ToString()) + 1).ToString();

                    }
                    else
                    {
                        SqlDataReader rsEmpresa = null;
                        try
                        {
                            rsEmpresa = Conexao.consulta("select SEQUENCIA from empresa_sequenciais where tabela_coluna ='" + tabelaCampo + "' and empresa=" + usr.filial.loja, new User(), true);
                            if (rsEmpresa.Read())
                            {
                                strSequencia = (long.Parse(rsEmpresa["SEQUENCIA"].ToString()) + 1).ToString();

                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        finally
                        {

                            if (rsEmpresa != null)
                                rsEmpresa.Close();
                        }

                    }
                }
                else
                {
                    String sql = "INSERT INTO [dbo].[SEQUENCIAIS] " +
                                  " ([TABELA_COLUNA] " +
                                  " ,[DESCRICAO] " +
                                  " ,[SEQUENCIA] " +
                                  " ,[TAMANHO] " +
                                  " ,[OBS1] " +
                                  " ,[OBS2] " +
                                  " ,[OBS3] " +
                                  " ,[OBS4] " +
                                  " ,[OBS5] " +
                                  " ,[OBS6] " +
                                  " ,[OBS7] " +
                                  " ,[OBS8] " +
                                  " ,[DATA_PARA_TRANSFERENCIA] " +
                                  " ,[PERMITE_POR_EMPRESA]) " +
                            " VALUES " +
                                  " ('" + tabelaCampo + "' " +
                                  " , 'SEQ " + tabelaCampo + "'" +
                                  " , '0000'" +
                                  " , 7 " +
                                  " , '' " +
                                  " , '' " +
                                  " , '' " +
                                  " , '' " +
                                  " , '' " +
                                  " , '' " +
                                  " , '' " +
                                  " , '' " +
                                  " , GETDATE() " +
                                  " , 0)";
                    if (rs != null)
                        rs.Close();
                    Conexao.executarSql(sql);
                    strSequencia = "1";
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
            return strSequencia;

        }
        public static bool salvaProximaSequencia(String tabelaCampo, User usr)
        {
            SqlDataReader rs = null;
            bool res = false;
            try
            {


                rs = Conexao.consulta("select * from sequenciais where tabela_coluna ='" + tabelaCampo + "'", new User(), true);

                if (rs.Read())
                {
                    String sql = "";
                    bool empresa = (bool)rs["PERMITE_POR_EMPRESA"];
                    if (!empresa)
                    {
                        sql = "update sequenciais set sequencia='" + (long.Parse(rs["SEQUENCIA"].ToString()) + 1).ToString() + "' where tabela_coluna='" + tabelaCampo + "'";

                    }
                    else
                    {
                        SqlDataReader rsEmpresa = null;
                        try
                        {
                            rsEmpresa = Conexao.consulta("select * from empresa_sequenciais where tabela_coluna ='" + tabelaCampo + "' and empresa=" + usr.filial.loja, new User(), true);
                            if (rsEmpresa.Read())
                            {
                                sql = "update empresa_sequenciais set sequencia='" + (long.Parse(rsEmpresa["SEQUENCIA"].ToString()) + 1).ToString() + "' where tabela_coluna='" + tabelaCampo + "' and empresa=" + usr.filial.loja;
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        finally
                        {
                            if (rsEmpresa != null)
                                rsEmpresa.Close();
                        }

                    }

                    res = Conexao.executarSql(sql);
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
            return res;
        }

        internal static DateTime proximoDiaSemana(DateTime emissao, string hora_emissao, int corte, string hora_corte)
        {

            DateTime dia = emissao;

            if (((int)emissao.DayOfWeek) == (corte - 1) && !hora_corte.Equals(""))
            {
                TimeSpan hEmissao = TimeSpan.Parse(hora_emissao);
                TimeSpan hCorte = TimeSpan.Parse(hora_corte);
                if (TimeSpan.Compare(hEmissao, hCorte) <= 0)
                    return emissao;
                else
                    dia = dia.AddDays(1);
            }


            while (((int)dia.DayOfWeek) != (corte - 1))
            {
                dia = dia.AddDays(1);
            }
            return dia;
        }

        public static String sequencia(String tabelaCampo, User usr, SqlConnection cnn, SqlTransaction tran)
        {
            SqlDataReader rs = null;
            String strSequencia = "";
            try
            {


                rs = Conexao.consulta("select * from sequenciais where tabela_coluna ='" + tabelaCampo + "'", new User(), true, cnn, tran);

                if (rs.Read())
                {
                    bool empresa = (bool)rs["PERMITE_POR_EMPRESA"];
                    if (!empresa)
                    {
                        strSequencia = (long.Parse(rs["SEQUENCIA"].ToString()) + 1).ToString();

                    }
                    else
                    {
                        SqlDataReader rsEmpresa = null;
                        try
                        {


                            rsEmpresa = Conexao.consulta("select SEQUENCIA from empresa_sequenciais where tabela_coluna ='" + tabelaCampo + "' and empresa=" + usr.filial.loja, new User(), true, cnn, tran);
                            if (rsEmpresa.Read())
                            {
                                strSequencia = (long.Parse(rsEmpresa["SEQUENCIA"].ToString()) + 1).ToString();

                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        finally
                        {
                            if (rsEmpresa != null)
                                rsEmpresa.Close();
                        }
                    }
                }
                else
                {
                    String sql = "INSERT INTO [dbo].[SEQUENCIAIS] " +
                                  " ([TABELA_COLUNA] " +
                                  " ,[DESCRICAO] " +
                                  " ,[SEQUENCIA] " +
                                  " ,[TAMANHO] " +
                                  " ,[OBS1] " +
                                  " ,[OBS2] " +
                                  " ,[OBS3] " +
                                  " ,[OBS4] " +
                                  " ,[OBS5] " +
                                  " ,[OBS6] " +
                                  " ,[OBS7] " +
                                  " ,[OBS8] " +
                                  " ,[DATA_PARA_TRANSFERENCIA] " +
                                  " ,[PERMITE_POR_EMPRESA]) " +
                            " VALUES " +
                                  " ('" + tabelaCampo + "' " +
                                  " , 'SEQ " + tabelaCampo + "'" +
                                  " , '0000'" +
                                  " , 7 " +
                                  " , '' " +
                                  " , '' " +
                                  " , '' " +
                                  " , '' " +
                                  " , '' " +
                                  " , '' " +
                                  " , '' " +
                                  " , '' " +
                                  " , GETDATE() " +
                                  " , 0)";
                    if (rs != null)
                        rs.Close();
                    Conexao.executarSql(sql, cnn, tran);
                    strSequencia = "1";
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
            return strSequencia;

        }

        public static bool salvaProximaSequencia(String tabelaCampo, User usr, SqlConnection cnn, SqlTransaction tran)
        {
            SqlDataReader rs = null;
            String sql = "";
            bool res = false;
            try
            {


                rs = Conexao.consulta("select * from sequenciais where tabela_coluna ='" + tabelaCampo + "'", new User(), true, cnn, tran);

                if (rs.Read())
                {

                    bool empresa = (bool)rs["PERMITE_POR_EMPRESA"];
                    if (!empresa)
                    {
                        sql = "update sequenciais set sequencia='" + (long.Parse(rs["SEQUENCIA"].ToString()) + 1).ToString() + "' where tabela_coluna='" + tabelaCampo + "'";

                    }
                    else
                    {
                        SqlDataReader rsEmpresa = null;
                        try
                        {


                            rsEmpresa = Conexao.consulta("select * from empresa_sequenciais where tabela_coluna ='" + tabelaCampo + "' and empresa=" + usr.filial.loja, new User(), true, cnn, tran);
                            if (rsEmpresa.Read())
                            {
                                sql = "update empresa_sequenciais set sequencia='" + (long.Parse(rsEmpresa["SEQUENCIA"].ToString()) + 1).ToString() + "' where tabela_coluna='" + tabelaCampo + "' and empresa=" + usr.filial.loja;
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        finally
                        {
                            if (rsEmpresa != null)
                                rsEmpresa.Close();
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
                if (rs != null)
                    rs.Close();
            }

            res = Conexao.executarSql(sql, cnn, tran);
            return res;
        }


        public static bool voltaUltimaSequencia(String tabelaCampo, User usr)
        {
            SqlDataReader rs = Conexao.consulta("select * from sequenciais where tabela_coluna ='" + tabelaCampo + "'", new User(), true);
            bool res = false;
            if (rs.Read())
            {
                String sql = "";
                bool empresa = (bool)rs["PERMITE_POR_EMPRESA"];
                if (!empresa)
                {
                    sql = "update sequenciais set sequencia='" + (long.Parse(rs["SEQUENCIA"].ToString()) - 1).ToString() + "' where tabela_coluna='" + tabelaCampo + "'";

                }
                else
                {
                    SqlDataReader rsEmpresa = Conexao.consulta("select * from empresa_sequenciais where tabela_coluna ='" + tabelaCampo + "' and empresa=" + usr.filial.loja, new User(), true);
                    if (rsEmpresa.Read())
                    {
                        sql = "update empresa_sequenciais set sequencia='" + (long.Parse(rsEmpresa["SEQUENCIA"].ToString()) - 1).ToString() + "' where tabela_coluna='" + tabelaCampo + "' and empresa=" + usr.filial.loja;
                    }
                    if (rsEmpresa != null)
                        rsEmpresa.Close();

                }

                res = Conexao.executarSql(sql);
            }
            if (rs != null)
                rs.Close();

            return res;
        }

        //** Função retorna a Margem a partir de parametros enviados.
        //** Custo Liquido, Preço de Venda, Aliquota de ICMS, PIS/Cofins Saída
        public static Decimal verificamargem(Decimal custoliq, Decimal venda, Decimal aliqicms, Decimal PISS)
        {
            Decimal dblmargem = 0;

            if (venda <= 0 || custoliq <= 0)
            {
                dblmargem = 0;
            }
            else
            {
                dblmargem = ((venda - custoliq) / custoliq) * 100;
                //dblmargem = (((venda * ((1 - (PISS / 100)) - (aliqicms / 100))) - custoliq) / venda) * 100;
            }
            return dblmargem;
        }

        //** Funcação para Calcular Preço de Venda de Acordo com informações passadas
        // ** Margem, Custo
        public static Decimal verificapreco(Decimal margem, Decimal custoliq)
        {
            Decimal dblPrecoVenda = 0;
            if (margem <= 0 || custoliq <= 0)
            {
                dblPrecoVenda = 0;
            }
            else
            {
                dblPrecoVenda = custoliq * (1 + (margem / 100));
            }
            return dblPrecoVenda;
        }

        public static void limpaParametros()
        {
            parametros.Clear();
        }
        public static String valorParametro(String parametro, User usr)
        {

            if (parametros.ContainsKey(parametro))
            {
                return (String)parametros[parametro];
            }
            else
            {
                String valor = Conexao.retornaUmValor("Select valor_atual from parametros where PARAMETRO='" + parametro + "'", null);
                parametros.Add(parametro, valor);
                return valor;
            }
        }

        public static bool isnumeroint(String numero)
        {
            try
            {
                int number3 = 0;
                return int.TryParse(numero, out number3);

            }
            catch (Exception)
            {
                return false;
            }

        }

        public static bool isnumero(String numero)
        {
            try
            {
                decimal number3 = 0;
                bool resultado = Decimal.TryParse(numero, out number3);
                return resultado;

            }
            catch (Exception)
            {

                return false;
            }

        }




        //Valida IE

        //[DllImport(@"C:\dll\DllInscE32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        //static extern bool ConsisteInscricaoEstadual(string Inscricao, string Estado);

        //Verificar IE / Parametro1 = Inscrição / Parametro2 = Estado
        public static bool isIE(string estado, string inscricao)
        {
            String ie = inscricao.Replace(".", "").Replace("-", "").Trim();

            return true;
            //if (!ConsisteInscricaoEstadual(ie, estado.ToUpper()))
            //    return true;
            //else
            //    return false;
        }


        //public static bool isIE(string pUF, string pInscr)
        //{

        //    bool isIE = false;

        //    string strBase;

        //    string strBase2;

        //    string strOrigem;

        //    string strDigito1;

        //    string strDigito2;

        //    int intPos;

        //    int intValor;

        //    int intSoma = 0;

        //    int intResto;

        //    int intNumero;

        //    int intPeso;

        //    int intDig;

        //    strBase = "";

        //    strBase2 = "";

        //    strOrigem = "";

        //    if ((pInscr.Trim().ToUpper() == "ISENTO"))
        //    {

        //        return true;

        //    }

        //    for (intPos = 1; intPos <= pInscr.Trim().Length; intPos++)
        //    {

        //        if ((("0123456789P".IndexOf(pInscr.Substring((intPos - 1), 1), 0, System.StringComparison.OrdinalIgnoreCase) + 1)

        //        > 0))
        //        {

        //            strOrigem = (strOrigem + pInscr.Substring((intPos - 1), 1));

        //        }

        //    }

        //    switch (pUF.ToUpper())
        //    {

        //        case "AC":

        //            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

        //            if (((strBase.Substring(0, 2) == "01")

        //            && (strBase.Substring(2, 2) != "00")))
        //            {

        //                intSoma = 0;

        //                for (intPos = 1; (intPos <= 8); intPos++)
        //                {

        //                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                    intValor = (intValor * (10 - intPos));

        //                    intSoma = (intSoma + intValor);

        //                }

        //                intResto = (intSoma % 11);

        //                strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

        //                strBase2 = (strBase.Substring(0, 8) + strDigito1);

        //                if ((strBase2 == strOrigem))
        //                {

        //                    isIE = true;

        //                }

        //            }

        //            break;

        //        case "AL":

        //            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

        //            if ((strBase.Substring(0, 2) == "24"))
        //            {

        //                intSoma = 0;

        //                for (intPos = 1; (intPos <= 8); intPos++)
        //                {

        //                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                    intValor = (intValor * (10 - intPos));

        //                    intSoma = (intSoma + intValor);

        //                }

        //                intSoma = (intSoma * 10);

        //                intResto = (intSoma % 11);

        //                strDigito1 = ((intResto == 10) ? "0" : Convert.ToString(intResto)).Substring((((intResto == 10) ? "0" : Convert.ToString(intResto)).Length - 1));

        //                strBase2 = (strBase.Substring(0, 8) + strDigito1);

        //                if ((strBase2 == strOrigem))
        //                {

        //                    isIE = true;

        //                }

        //            }

        //            break;

        //        case "AM":

        //            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

        //            intSoma = 0;

        //            for (intPos = 1; (intPos <= 8); intPos++)
        //            {

        //                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                intValor = (intValor * (10 - intPos));

        //                intSoma = (intSoma + intValor);

        //            }

        //            if ((intSoma < 11))
        //            {

        //                strDigito1 = Convert.ToString((11 - intSoma)).Substring((Convert.ToString((11 - intSoma)).Length - 1));

        //            }

        //            else
        //            {

        //                intResto = (intSoma % 11);

        //                strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

        //            }

        //            strBase2 = (strBase.Substring(0, 8) + strDigito1);

        //            if ((strBase2 == strOrigem))
        //            {

        //                isIE = true;

        //            }

        //            break;

        //        case "AP":

        //            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

        //            intPeso = 0;

        //            intDig = 0;

        //            if ((strBase.Substring(0, 2) == "03"))
        //            {

        //                intNumero = int.Parse(strBase.Substring(0, 8));

        //                if (((intNumero >= 3000001)

        //                && (intNumero <= 3017000)))
        //                {

        //                    intPeso = 5;

        //                    intDig = 0;

        //                }

        //                else if (((intNumero >= 3017001)

        //                && (intNumero <= 3019022)))
        //                {

        //                    intPeso = 9;

        //                    intDig = 1;

        //                }

        //                else if ((intNumero >= 3019023))
        //                {

        //                    intPeso = 0;

        //                    intDig = 0;

        //                }

        //                intSoma = intPeso;

        //                for (intPos = 1; (intPos <= 8); intPos++)
        //                {

        //                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                    intValor = (intValor * (10 - intPos));

        //                    intSoma = (intSoma + intValor);

        //                }

        //                intResto = (intSoma % 11);

        //                intValor = (11 - intResto);

        //                if ((intValor == 10))
        //                {

        //                    intValor = 0;

        //                }

        //                else if ((intValor == 11))
        //                {

        //                    intValor = intDig;

        //                }

        //                strDigito1 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));

        //                strBase2 = (strBase.Substring(0, 8) + strDigito1);

        //                if ((strBase2 == strOrigem))
        //                {

        //                    isIE = true;

        //                }

        //            }

        //            break;

        //        case "BA":

        //            strBase = (strOrigem.Trim() + "00000000").Substring(0, 8);

        //            if ((("0123458".IndexOf(strBase.Substring(0, 1), 0, System.StringComparison.OrdinalIgnoreCase) + 1)

        //            > 0))
        //            {

        //                intSoma = 0;

        //                for (intPos = 1; (intPos <= 6); intPos++)
        //                {

        //                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                    intValor = (intValor * (8 - intPos));

        //                    intSoma = (intSoma + intValor);

        //                }

        //                intResto = (intSoma % 10);

        //                strDigito2 = ((intResto == 0) ? "0" : Convert.ToString((10 - intResto))).Substring((((intResto == 0) ? "0" : Convert.ToString((10 - intResto))).Length - 1));

        //                strBase2 = (strBase.Substring(0, 6) + strDigito2);

        //                intSoma = 0;

        //                for (intPos = 1; (intPos <= 7); intPos++)
        //                {

        //                    intValor = int.Parse(strBase2.Substring((intPos - 1), 1));

        //                    intValor = (intValor * (9 - intPos));

        //                    intSoma = (intSoma + intValor);

        //                }

        //                intResto = (intSoma % 10);

        //                strDigito1 = ((intResto == 0) ? "0" : Convert.ToString((10 - intResto))).Substring((((intResto == 0) ? "0" : Convert.ToString((10 - intResto))).Length - 1));

        //            }

        //            else
        //            {

        //                intSoma = 0;

        //                for (intPos = 1; (intPos <= 6); intPos++)
        //                {

        //                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                    intValor = (intValor * (8 - intPos));

        //                    intSoma = (intSoma + intValor);

        //                }

        //                intResto = (intSoma % 11);

        //                strDigito2 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

        //                strBase2 = (strBase.Substring(0, 6) + strDigito2);

        //                intSoma = 0;

        //                for (intPos = 1; (intPos <= 7); intPos++)
        //                {

        //                    intValor = int.Parse(strBase2.Substring((intPos - 1), 1));

        //                    intValor = (intValor * (9 - intPos));

        //                    intSoma = (intSoma + intValor);

        //                }

        //                intResto = (intSoma % 11);

        //                strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

        //            }

        //            strBase2 = (strBase.Substring(0, 6)

        //            + (strDigito1 + strDigito2));

        //            if ((strBase2 == strOrigem))
        //            {

        //                isIE = true;

        //            }

        //            break;

        //        case "CE":

        //            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

        //            intSoma = 0;

        //            for (intPos = 1; (intPos <= 8); intPos++)
        //            {

        //                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                intValor = (intValor * (10 - intPos));

        //                intSoma = (intSoma + intValor);

        //            }

        //            intResto = (intSoma % 11);

        //            intValor = (11 - intResto);

        //            if ((intValor > 9))
        //            {

        //                intValor = 0;

        //            }

        //            strDigito1 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));

        //            strBase2 = (strBase.Substring(0, 8) + strDigito1);

        //            if ((strBase2 == strOrigem))
        //            {

        //                isIE = true;

        //            }

        //            break;

        //        case "DF":

        //            strBase = (strOrigem.Trim() + "0000000000000").Substring(0, 13);

        //            if ((strBase.Substring(0, 3) == "073"))
        //            {

        //                intSoma = 0;

        //                intPeso = 2;

        //                for (intPos = 11; (intPos <= 1); intPos = (intPos + -1))
        //                {

        //                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                    intValor = (intValor * intPeso);

        //                    intSoma = (intSoma + intValor);

        //                    intPeso = (intPeso + 1);

        //                    if ((intPeso > 9))
        //                    {

        //                        intPeso = 2;

        //                    }

        //                }

        //                intResto = (intSoma % 11);

        //                strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

        //                strBase2 = (strBase.Substring(0, 11) + strDigito1);

        //                intSoma = 0;

        //                intPeso = 2;

        //                for (intPos = 12; (intPos <= 1); intPos = (intPos + -1))
        //                {

        //                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                    intValor = (intValor * intPeso);

        //                    intSoma = (intSoma + intValor);

        //                    intPeso = (intPeso + 1);

        //                    if ((intPeso > 9))
        //                    {

        //                        intPeso = 2;

        //                    }

        //                }

        //                intResto = (intSoma % 11);

        //                strDigito2 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

        //                strBase2 = (strBase.Substring(0, 12) + strDigito2);

        //                if ((strBase2 == strOrigem))
        //                {

        //                    isIE = true;

        //                }

        //            }

        //            break;

        //        case "ES":

        //            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

        //            intSoma = 0;

        //            for (intPos = 1; (intPos <= 8); intPos++)
        //            {

        //                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                intValor = (intValor * (10 - intPos));

        //                intSoma = (intSoma + intValor);

        //            }

        //            intResto = (intSoma % 11);

        //            strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

        //            strBase2 = (strBase.Substring(0, 8) + strDigito1);

        //            if ((strBase2 == strOrigem))
        //            {

        //                isIE = true;

        //            }

        //            break;

        //        case "GO":

        //            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

        //            if ((("10,11,15".IndexOf(strBase.Substring(0, 2), 0, System.StringComparison.OrdinalIgnoreCase) + 1)

        //            > 0))
        //            {

        //                intSoma = 0;

        //                for (intPos = 1; (intPos <= 8); intPos++)
        //                {

        //                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                    intValor = (intValor * (10 - intPos));

        //                    intSoma = (intSoma + intValor);

        //                }

        //                intResto = (intSoma % 11);

        //                if ((intResto == 0))
        //                {

        //                    strDigito1 = "0";

        //                }

        //                else if ((intResto == 1))
        //                {

        //                    intNumero = int.Parse(strBase.Substring(0, 8));

        //                    strDigito1 = (((intNumero >= 10103105)

        //                    && (intNumero <= 10119997)) ? "1" : "0").Substring(((((intNumero >= 10103105)

        //                    && (intNumero <= 10119997)) ? "1" : "0").Length - 1));

        //                }

        //                else
        //                {

        //                    strDigito1 = Convert.ToString((11 - intResto)).Substring((Convert.ToString((11 - intResto)).Length - 1));

        //                }

        //                strBase2 = (strBase.Substring(0, 8) + strDigito1);

        //                if ((strBase2 == strOrigem))
        //                {

        //                    isIE = true;

        //                }

        //            }

        //            break;

        //        case "MA":

        //            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

        //            if ((strBase.Substring(0, 2) == "12"))
        //            {

        //                intSoma = 0;

        //                for (intPos = 1; (intPos <= 8); intPos++)
        //                {

        //                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                    intValor = (intValor * (10 - intPos));

        //                    intSoma = (intSoma + intValor);

        //                }

        //                intResto = (intSoma % 11);

        //                strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

        //                strBase2 = (strBase.Substring(0, 8) + strDigito1);

        //                if ((strBase2 == strOrigem))
        //                {

        //                    isIE = true;

        //                }

        //            }

        //            break;

        //        case "MT":

        //            strBase = (strOrigem.Trim() + "0000000000").Substring(0, 10);

        //            intSoma = 0;

        //            intPeso = 2;

        //            for (intPos = 10; (intPos <= 1); intPos = (intPos + -1))
        //            {

        //                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                intValor = (intValor * intPeso);

        //                intSoma = (intSoma + intValor);

        //                intPeso = (intPeso + 1);

        //                if ((intPeso > 9))
        //                {

        //                    intPeso = 2;

        //                }

        //            }

        //            intResto = (intSoma % 11);

        //            strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

        //            strBase2 = (strBase.Substring(0, 10) + strDigito1);

        //            if ((strBase2 == strOrigem))
        //            {

        //                isIE = true;

        //            }

        //            break;

        //        case "MS":

        //            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

        //            if ((strBase.Substring(0, 2) == "28"))
        //            {

        //                intSoma = 0;

        //                for (intPos = 1; (intPos <= 8); intPos++)
        //                {

        //                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                    intValor = (intValor * (10 - intPos));

        //                    intSoma = (intSoma + intValor);

        //                }

        //                intResto = (intSoma % 11);

        //                strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

        //                strBase2 = (strBase.Substring(0, 8) + strDigito1);

        //                if ((strBase2 == strOrigem))
        //                {

        //                    isIE = true;

        //                }

        //            }

        //            break;

        //        case "MG":

        //            strBase = (strOrigem.Trim() + "0000000000000").Substring(0, 13);

        //            strBase2 = (strBase.Substring(0, 3) + ("0" + strBase.Substring(3, 8)));

        //            intNumero = 2;

        //            for (intPos = 1; (intPos <= 12); intPos++)
        //            {

        //                intValor = int.Parse(strBase2.Substring((intPos - 1), 1));

        //                intNumero = ((intNumero == 2) ? 1 : 2);

        //                intValor = (intValor * intNumero);

        //                if ((intValor > 9))
        //                {

        //                    strDigito1 = string.Format("00", intValor);

        //                    intValor = (int.Parse(strDigito1.Substring(0, 1)) + int.Parse(strDigito1.Substring((strDigito1.Length - 1))));

        //                }

        //                intSoma = (intSoma + intValor);

        //            }

        //            intValor = intSoma;

        //            while ((string.Format("000", intValor).Substring((string.Format("000", intValor).Length - 1)) != "0"))
        //            {

        //                intValor = (intValor + 1);

        //                strDigito1 = string.Format("00", (intValor - intSoma)).Substring((string.Format("00", (intValor - intSoma)).Length - 1));

        //                strBase2 = (strBase.Substring(0, 11) + strDigito1);

        //                intSoma = 0;

        //                intPeso = 2;

        //                for (intPos = 12; (intPos <= 1); intPos = (intPos + -1))
        //                {

        //                    intValor = int.Parse(strBase2.Substring((intPos - 1), 1));

        //                    intValor = (intValor * intPeso);

        //                    intSoma = (intSoma + intValor);

        //                    intPeso = (intPeso + 1);

        //                    if ((intPeso > 11))
        //                    {

        //                        intPeso = 2;

        //                    }

        //                }

        //                intResto = (intSoma % 11);

        //                strDigito2 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

        //                strBase2 = (strBase2 + strDigito2);

        //                if ((strBase2 == strOrigem))
        //                {

        //                    isIE = true;

        //                }

        //            }

        //            break;

        //        case "PA":

        //            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

        //            if ((strBase.Substring(0, 2) == "15"))
        //            {

        //                intSoma = 0;

        //                for (intPos = 1; (intPos <= 8); intPos++)
        //                {

        //                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                    intValor = (intValor * (10 - intPos));

        //                    intSoma = (intSoma + intValor);

        //                }

        //                intResto = (intSoma % 11);

        //                strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

        //                strBase2 = (strBase.Substring(0, 8) + strDigito1);

        //                if ((strBase2 == strOrigem))
        //                {

        //                    isIE = true;

        //                }

        //            }

        //            break;

        //        case "PB":

        //            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

        //            intSoma = 0;

        //            for (intPos = 1; (intPos <= 8); intPos++)
        //            {

        //                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                intValor = (intValor * (10 - intPos));

        //                intSoma = (intSoma + intValor);

        //            }

        //            intResto = (intSoma % 11);

        //            intValor = (11 - intResto);

        //            if ((intValor > 9))
        //            {

        //                intValor = 0;

        //            }

        //            strDigito1 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));

        //            strBase2 = (strBase.Substring(0, 8) + strDigito1);

        //            if ((strBase2 == strOrigem))
        //            {

        //                isIE = true;

        //            }

        //            break;

        //        case "PE":

        //            strBase = (strOrigem.Trim() + "00000000000000").Substring(0, 14);

        //            intSoma = 0;

        //            intPeso = 2;

        //            for (intPos = 13; (intPos <= 1); intPos = (intPos + -1))
        //            {

        //                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                intValor = (intValor * intPeso);

        //                intSoma = (intSoma + intValor);

        //                intPeso = (intPeso + 1);

        //                if ((intPeso > 9))
        //                {

        //                    intPeso = 2;

        //                }

        //            }

        //            intResto = (intSoma % 11);

        //            intValor = (11 - intResto);

        //            if ((intValor > 9))
        //            {

        //                intValor = (intValor - 10);

        //            }

        //            strDigito1 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));

        //            strBase2 = (strBase.Substring(0, 13) + strDigito1);

        //            if ((strBase2 == strOrigem))
        //            {

        //                isIE = true;

        //            }

        //            break;

        //        case "PI":

        //            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

        //            intSoma = 0;

        //            for (intPos = 1; (intPos <= 8); intPos++)
        //            {

        //                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                intValor = (intValor * (10 - intPos));

        //                intSoma = (intSoma + intValor);

        //            }

        //            intResto = (intSoma % 11);

        //            strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

        //            strBase2 = (strBase.Substring(0, 8) + strDigito1);

        //            if ((strBase2 == strOrigem))
        //            {

        //                isIE = true;

        //            }

        //            break;

        //        case "PR":

        //            strBase = (strOrigem.Trim() + "0000000000").Substring(0, 10);

        //            intSoma = 0;

        //            intPeso = 2;

        //            for (intPos = 8; (intPos <= 1); intPos = (intPos + -1))
        //            {

        //                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                intValor = (intValor * intPeso);

        //                intSoma = (intSoma + intValor);

        //                intPeso = (intPeso + 1);

        //                if ((intPeso > 7))
        //                {

        //                    intPeso = 2;

        //                }

        //            }

        //            intResto = (intSoma % 11);

        //            strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

        //            strBase2 = (strBase.Substring(0, 8) + strDigito1);

        //            intSoma = 0;

        //            intPeso = 2;

        //            for (intPos = 9; (intPos <= 1); intPos = (intPos + -1))
        //            {

        //                intValor = int.Parse(strBase2.Substring((intPos - 1), 1));

        //                intValor = (intValor * intPeso);

        //                intSoma = (intSoma + intValor);

        //                intPeso = (intPeso + 1);

        //                if ((intPeso > 7))
        //                {

        //                    intPeso = 2;

        //                }

        //            }

        //            intResto = (intSoma % 11);

        //            strDigito2 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

        //            strBase2 = (strBase2 + strDigito2);

        //            if ((strBase2 == strOrigem))
        //            {

        //                isIE = true;

        //            }

        //            break;

        //        case "RJ":

        //            strBase = (strOrigem.Trim() + "00000000").Substring(0, 8);

        //            intSoma = 0;

        //            intPeso = 2;

        //            for (intPos = 7; (intPos <= 1); intPos = (intPos + -1))
        //            {

        //                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                intValor = (intValor * intPeso);

        //                intSoma = (intSoma + intValor);

        //                intPeso = (intPeso + 1);

        //                if ((intPeso > 7))
        //                {

        //                    intPeso = 2;

        //                }

        //            }

        //            intResto = (intSoma % 11);

        //            strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

        //            strBase2 = (strBase.Substring(0, 7) + strDigito1);

        //            if ((strBase2 == strOrigem))
        //            {

        //                isIE = true;

        //            }

        //            break;

        //        case "RN":

        //            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

        //            if ((strBase.Substring(0, 2) == "20"))
        //            {

        //                intSoma = 0;

        //                for (intPos = 1; (intPos <= 8); intPos++)
        //                {

        //                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                    intValor = (intValor * (10 - intPos));

        //                    intSoma = (intSoma + intValor);

        //                }

        //                intSoma = (intSoma * 10);

        //                intResto = (intSoma % 11);

        //                strDigito1 = ((intResto > 9) ? "0" : Convert.ToString(intResto)).Substring((((intResto > 9) ? "0" : Convert.ToString(intResto)).Length - 1));

        //                strBase2 = (strBase.Substring(0, 8) + strDigito1);

        //                if ((strBase2 == strOrigem))
        //                {

        //                    isIE = true;

        //                }

        //            }

        //            break;

        //        case "RO":

        //            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

        //            strBase2 = strBase.Substring(3, 5);

        //            intSoma = 0;

        //            for (intPos = 1; (intPos <= 5); intPos++)
        //            {

        //                intValor = int.Parse(strBase2.Substring((intPos - 1), 1));

        //                intValor = (intValor * (7 - intPos));

        //                intSoma = (intSoma + intValor);

        //            }

        //            intResto = (intSoma % 11);

        //            intValor = (11 - intResto);

        //            if ((intValor > 9))
        //            {

        //                intValor = (intValor - 10);

        //            }

        //            strDigito1 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));

        //            strBase2 = (strBase.Substring(0, 8) + strDigito1);

        //            if ((strBase2 == strOrigem))
        //            {

        //                isIE = true;

        //            }

        //            break;

        //        case "RR":

        //            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

        //            if ((strBase.Substring(0, 2) == "24"))
        //            {

        //                intSoma = 0;

        //                for (intPos = 1; (intPos <= 8); intPos++)
        //                {

        //                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                    intValor = (intValor * (10 - intPos));

        //                    intSoma = (intSoma + intValor);

        //                }

        //                intResto = (intSoma % 9);

        //                strDigito1 = Convert.ToString(intResto).Substring((Convert.ToString(intResto).Length - 1));

        //                strBase2 = (strBase.Substring(0, 8) + strDigito1);

        //                if ((strBase2 == strOrigem))
        //                {

        //                    isIE = true;

        //                }

        //            }

        //            break;

        //        case "RS":

        //            strBase = (strOrigem.Trim() + "0000000000").Substring(0, 10);

        //            intNumero = int.Parse(strBase.Substring(0, 3));

        //            if (((intNumero > 0)

        //            && (intNumero < 468)))
        //            {

        //                intSoma = 0;

        //                intPeso = 2;

        //                for (intPos = 9; (intPos <= 1); intPos = (intPos + -1))
        //                {

        //                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                    intValor = (intValor * intPeso);

        //                    intSoma = (intSoma + intValor);

        //                    intPeso = (intPeso + 1);

        //                    if ((intPeso > 9))
        //                    {

        //                        intPeso = 2;

        //                    }

        //                }

        //                intResto = (intSoma % 11);

        //                intValor = (11 - intResto);

        //                if ((intValor > 9))
        //                {

        //                    intValor = 0;

        //                }

        //                strDigito1 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));

        //                strBase2 = (strBase.Substring(0, 9) + strDigito1);

        //                if ((strBase2 == strOrigem))
        //                {

        //                    isIE = true;

        //                }

        //            }

        //            break;

        //        case "SC":

        //            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

        //            intSoma = 0;

        //            for (intPos = 1; (intPos <= 8); intPos++)
        //            {

        //                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                intValor = (intValor * (10 - intPos));

        //                intSoma = (intSoma + intValor);

        //            }

        //            intResto = (intSoma % 11);

        //            strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

        //            strBase2 = (strBase.Substring(0, 8) + strDigito1);

        //            if ((strBase2 == strOrigem))
        //            {

        //                isIE = true;

        //            }

        //            break;

        //        case "SE":

        //            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

        //            intSoma = 0;

        //            for (intPos = 1; (intPos <= 8); intPos++)
        //            {

        //                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                intValor = (intValor * (10 - intPos));

        //                intSoma = (intSoma + intValor);

        //            }

        //            intResto = (intSoma % 11);

        //            intValor = (11 - intResto);

        //            if ((intValor > 9))
        //            {

        //                intValor = 0;

        //            }

        //            strDigito1 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));

        //            strBase2 = (strBase.Substring(0, 8) + strDigito1);

        //            if ((strBase2 == strOrigem))
        //            {

        //                isIE = true;

        //            }

        //            break;

        //        case "SP":
        //            //Array que contem os pesos para o segundo digito verificador.
        //            int[] intPeso2 = new int[] {3, 2, 10, 9, 8, 7, 6, 5, 4, 3, 2};
        //            if ((strOrigem.Substring(0, 1) == "P"))
        //            {

        //                strBase = (strOrigem.Trim() + "0000000000000").Substring(0, 13);

        //                strBase2 = strBase.Substring(1, 8);

        //                intSoma = 0;

        //                intPeso = 1;

        //                for (intPos = 1; (intPos <= 8); intPos++)
        //                {

        //                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                    intValor = (intValor * intPeso);

        //                    intSoma = (intSoma + intValor);

        //                    intPeso = (intPeso + 1);

        //                    if ((intPeso == 2))
        //                    {

        //                        intPeso = 3;

        //                    }

        //                    if ((intPeso == 9))
        //                    {

        //                        intPeso = 10;

        //                    }

        //                }

        //                intResto = (intSoma % 11);

        //                strDigito1 = Convert.ToString(intResto).Substring((Convert.ToString(intResto).Length - 1));

        //                strBase2 = (strBase.Substring(0, 8)

        //                + (strDigito1 + strBase.Substring(10, 3)));

        //            }

        //            else
        //            {

        //                strBase = (strOrigem.Trim() + "000000000000").Substring(0, 12);

        //                intSoma = 0;

        //                intPeso = 1;

        //                for (intPos = 1; (intPos <= 8); intPos++)
        //                {

        //                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                    intValor = (intValor * intPeso );

        //                    intSoma = (intSoma + intValor);

        //                    intPeso = (intPeso + 1);

        //                    if ((intPeso == 2))
        //                    {

        //                        intPeso = 3;

        //                    }

        //                    if ((intPeso == 9))
        //                    {

        //                        intPeso = 10;

        //                    }

        //                }

        //                intResto = (intSoma % 11);

        //                strDigito1 = Convert.ToString(intResto).Substring((Convert.ToString(intResto).Length - 1));

        //                strBase2 = (strBase.Substring(0, 8)

        //                + (strDigito1 + strBase.Substring(9, 2)));

        //                intSoma = 0;

        //                intPeso = 2;

        //                for (intPos = 1; (intPos <= 11); intPos++)
        //                {

        //                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

        //                    intValor = (intValor * intPeso2[intPos - 1]);

        //                    intSoma = (intSoma + intValor);

        //                    /*
        //                    intPeso = (intPeso + 1);

        //                    if ((intPeso > 10))
        //                    {

        //                        intPeso = 2;

        //                    }
        //                     */

        //                }

        //                intResto = (intSoma % 11);

        //                strDigito2 = Convert.ToString(intResto).Substring((Convert.ToString(intResto).Length - 1));

        //                strBase2 = (strBase2 + strDigito2);

        //            }

        //            if ((strBase2 == strOrigem))
        //            {

        //                isIE = true;

        //            }

        //            break;

        //        case "TO":

        //            strBase = (strOrigem.Trim() + "00000000000").Substring(0, 11);

        //            if ((("01,02,03,99".IndexOf(strBase.Substring(2, 2), 0, System.StringComparison.OrdinalIgnoreCase) + 1)

        //            > 0))
        //            {

        //                strBase2 = (strBase.Substring(0, 2) + strBase.Substring(4, 6));

        //                intSoma = 0;

        //                for (intPos = 1; (intPos <= 8); intPos++)
        //                {

        //                    intValor = int.Parse(strBase2.Substring((intPos - 1), 1));

        //                    intValor = (intValor * (10 - intPos));

        //                    intSoma = (intSoma + intValor);

        //                }

        //                intResto = (intSoma % 11);

        //                strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

        //                strBase2 = (strBase.Substring(0, 10) + strDigito1);

        //                if ((strBase2 == strOrigem))
        //                {

        //                    isIE = true;

        //                }

        //            }

        //            break;

        //    }

        //    return isIE;

        //}


        public static void enviarEmailAnexos(User usr, String endereco, String endereco_cc, String assunto, String mensagem, String[] anexos)
        {
            try
            {

                if (usr.host.Equals("") || usr.email.Equals("") || usr.emailSenha.Equals(""))
                {
                    throw new Exception("Os Campos de Configuração de E-mail não estão preenchidos.");
                }

                string[] emails = endereco.Split(';');

                SmtpClient client = new SmtpClient("Servidor SMTP", int.Parse(usr.porta));
                client.Host = usr.host;

                bool ssl = Funcoes.valorParametro("EMAIL_SSL", null).ToUpper().Equals("TRUE");
                client.EnableSsl = ssl;
                client.UseDefaultCredentials = false;

                client.Credentials = new NetworkCredential(usr.email, usr.emailSenha);



                MailMessage message = new MailMessage();
                message.From = new MailAddress(usr.email);
                message.Sender = new MailAddress(usr.email);


                foreach (string item in emails)
                {
                    message.To.Add(new MailAddress(item));
                }
                if (!endereco_cc.Equals(""))
                {
                    string[] emailsCC = endereco_cc.Split(';');
                    foreach (var item in emailsCC)
                    {
                        message.CC.Add(new MailAddress(item));
                    }
                }
                message.Subject = assunto;
                message.Body = mensagem;
                message.IsBodyHtml = true;

                foreach (String item in anexos)
                {
                    String strNomeAnexo = item.Replace("/", "\\");

                    if (!item.Equals(""))
                    {
                        Attachment anexado = new Attachment(strNomeAnexo, MediaTypeNames.Application.Octet);
                        message.Attachments.Add(anexado);

                        //message.Attachments.Add(new Attachment(item));
                    }

                }

                client.Send(message);
            }
            catch (Exception Err)
            {

                throw new Exception("Erro Ao Enviar E-mail:" + Err.Message);
            }

        }
        public static string FormatCNPJ(string CNPJ)
        {
            return Convert.ToUInt64(CNPJ).ToString(@"00\.000\.000\/0000\-00");
        }
        public static string FormatCPF(string CPF)
        {
            return Convert.ToUInt64(CPF).ToString(@"000\.000\.000\-00");
        }
        public static string SemFormatacao(string Codigo)
        {
            return Codigo.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
        }
        public static void enviarEmail(User usr, String endereco, String endereco_cc, String assunto, String mensagem)
        {
            try
            {
                string[] emails = endereco.Split(';');

                if (usr.porta.Trim().Equals(""))
                {
                    throw new Exception("O Usuario não esta com as configurações de envio de E-mail!");
                }

                SmtpClient client = new SmtpClient("Servidor SMTP", int.Parse(usr.porta));
                client.Host = usr.host;
                bool ssl = Funcoes.valorParametro("EMAIL_SSL", null).ToUpper().Equals("TRUE");
                client.EnableSsl = ssl;

                client.Credentials = new NetworkCredential(usr.email, usr.emailSenha);

                MailMessage message = new MailMessage();
                message.From = new MailAddress(usr.email);
                message.Sender = new MailAddress(usr.email);
                foreach (string item in emails)
                {
                    message.To.Add(new MailAddress(item));
                }
                if (!endereco_cc.Equals(""))
                {
                    string[] emailsCC = endereco.Split(';');
                    foreach (var item in emailsCC)
                    {
                        message.CC.Add(new MailAddress(item));
                    }
                }
                message.Subject = assunto;
                message.Body = mensagem;
                message.IsBodyHtml = true;

                client.Send(message);
            }
            catch (Exception Err)
            {

                throw new Exception("Erro Ao Enviar E-mail:" + Err.Message);
            }

        }

        //Função para converter números longos
        public static long lngTry(String vlr)
        {
            long vVlr = 0;
            long.TryParse(vlr, out vVlr);
            return vVlr;
        }

        public static DateTime vencimentoDIFAL(int tipo, int qtde, bool diautil, int tipoQtde)
        {
            //** tipo 0 = dia; 1 = mes; 2 = ano; 3 = semana; 4 = quinzena; 5 = semestre
            try
            {

                DateTime dtVencimento = DateTime.Today;

                //Caso o tipo seja MÊS.
                if (tipo == 1 && qtde > 0)
                {
                    //Coloca a data para o último dia do mês anterior
                    dtVencimento = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(tipoQtde).AddDays(-1);

                    if (diautil)
                    {

                        int diasAdicionados = 0;
                        while (diasAdicionados < qtde)
                        {
                            dtVencimento = dtVencimento.AddDays(1);
                            //Verifica se é dia útil
                            if (dtVencimento.DayOfWeek != DayOfWeek.Saturday &&
                                dtVencimento.DayOfWeek != DayOfWeek.Sunday && !isFeriado(dtVencimento))
                            {
                                diasAdicionados++;
                            }
                        }
                    }
                    else
                    {
                        //Adiciona na data o número de dias (ou seja o dia do vencimento)
                        dtVencimento = dtVencimento.AddDays(qtde);
                        //Caso o vencimento seja sábado, domingo ou feriado o sistema antecipa o vencimento para o primeiro dia
                        //últis antes do feriado.
                        while (isDiaUtil(dtVencimento))
                        {
                            dtVencimento =  dtVencimento.AddDays(-1);
                        }
                    }
                }
                return dtVencimento;
            }
            catch
            {
                return DateTime.Today;
            }
        }

        internal static bool existePLU(String strPlu)
        {
            return (Funcoes.intTry(
                        Conexao.retornaUmValor("Select count(*) from mercadoria where plu ='" + strPlu + "'", null)
                        ) > 0);
        }

        //**** Checa se o dia enviado é um feriado
        public static bool isFeriado(DateTime data)
        {
            return (Funcoes.intTry(
                        Conexao.retornaUmValor("SELECT COUNT(*) FROM feriado WHERE dia ='" + data.ToString("yyyy-MM-dd") + "'", null)
                        ) > 0);
        }

        //**** Checa se o dia enviado é um feriado
        public static bool isDiaUtil(DateTime data)
        {
            if (data.DayOfWeek == DayOfWeek.Saturday ||
                data.DayOfWeek == DayOfWeek.Sunday ||
                isFeriado(data))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void atualizaSaldoPLUDia(String Filial, String PLU, decimal qtdeAlterar, SqlConnection conn, SqlTransaction tran, string tipoMovimentacao, DateTime dataMovimentacao)
        {
            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta(@"Select tipo.Estoque, tipo.Permite_item, MERCADORIA.PLU_Vinculado, MERCADORIA.fator_Estoque_Vinculado, isnull(ML.saldo_atual,0) saldo_atual
                                        from mercadoria INNER JOIN Mercadoria_Loja ml ON mercadoria.plu = ml.PLU
                                        inner join tipo on mercadoria.Tipo = tipo.Tipo
                                        where ML.FILIAL = '" + Filial + "' AND mercadoria.plu ='" + PLU + "'", null, false);
                if (rs.Read())
                {

                    if (rs["Estoque"].ToString().Equals("1"))
                    {
                        String sql = "sp_Atualizar_Saldo_Dia " +
                            "@Filial = '" + Filial + "'" +
                            ", @Data = '" + dataMovimentacao.ToString("yyyy-MM-dd") + "'" +
                            ", @PLU = '" + PLU + "'" +
                            ", @Quantidade = " + qtdeAlterar.ToString().Replace(",", ".") +
                            ", @TipoMovimentacao = '" + tipoMovimentacao + "'";

                        Conexao.executarSql(sql, conn, tran);
                    }

                    if (rs["Permite_item"].ToString().Equals("1"))
                    {
                        SqlDataReader rsItens = null;
                        try
                        {

                            rsItens = Conexao.consulta("Select * from item where plu = '" + PLU + "'", null, false);


                            while (rsItens.Read())
                            {
                                decimal qtdeAtualizar = qtdeAlterar * decTry(rsItens["fator_conversao"].ToString());

                                String sql = "sp_Atualizar_Saldo_Dia " +
                                    "@Filial = '" + Filial + "'" +
                                    ", @Data = '" + dataMovimentacao.ToString("yyyy-MM-dd") + "'" +
                                    ", @PLU = '" + rsItens["Plu_item"].ToString() + "'" +
                                    ", @Quantidade = " + decimalPonto(qtdeAtualizar) +
                                    ", @TipoMovimentacao = '" + tipoMovimentacao + "'";

                                Conexao.executarSql(sql, conn, tran);

                            }

                            String PLU_vinculado = Conexao.retornaUmValor("Select PLU_vinculado from mercadoria where plu ='" + PLU + "'", null);

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
                    }
                    if (!rs["PLU_Vinculado"].ToString().Equals(""))
                    {
                        Decimal fator = decTry(rs["fator_Estoque_Vinculado"].ToString());
                        atualizaSaldoPLUDia(Filial, rs["PLU_Vinculado"].ToString(), qtdeAlterar * fator, conn, tran, tipoMovimentacao, dataMovimentacao);
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
        public static void recalculoSaldoPLUDia(String Filial, String PLU, DateTime dataMovimentacao)
        {
            SqlConnection conn = Conexao.novaConexao();

            try
            {
                String sql = "sp_br_RecalculoSaldoDia " +
                    "@Filial = '" + Filial + "'" +
                    ", @PLU = '" + PLU + "'" +
                    ", @DATAINICIO = '" + dataMovimentacao.ToString("yyyy-MM-dd") + "'";

                Conexao.executarSql(sql);
            }
            catch (Exception e)
            {
                throw;
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
        public static Decimal ConvertstrToDecimalCulture(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return 0;

            // Remove espaços
            valor = valor.Trim();

            // Troca vírgula por ponto (para padronizar)
            valor = valor.Replace(',', '.');

            // Usa CultureInfo com ponto como separador decimal
            var culture = CultureInfo.InvariantCulture;

            // Tenta converter
            if (decimal.TryParse(valor, NumberStyles.Any, culture, out decimal resultado))
            {
                return resultado;
            }
            else
            {
                return 0;
            }
        }

        public static string DefinirDiretorio(bool dia, filialDAO filial)
        {
            try
            {
                string Diretorio = filial.diretorio_exporta;
                if (filial.diretorio_exporta.Substring(filial.diretorio_exporta.Length - 1) != "\\")
                {
                    filial.diretorio_exporta += "\\";
                }

                if (dia)
                {
                    // Ano
                    Diretorio += DateTime.Now.Year;
                    if (!Directory.Exists(Diretorio))
                        Directory.CreateDirectory(Diretorio);

                    // Mês
                    Diretorio += "\\" + String.Format("{0:00}", DateTime.Now.Month);
                    if (!Directory.Exists(Diretorio))
                        Directory.CreateDirectory(Diretorio);

                    //// Dia
                    //Diretorio += "\\" + String.Format("{0:00}", DateTime.Now.Day);
                    //if (!Directory.Exists(Diretorio))
                    //    Directory.CreateDirectory(Diretorio);
                }

                return Diretorio + (Diretorio.Substring(Diretorio.Length - 1) != "\\" ? "\\" : "");
            }
            catch (Exception err)
            {

                throw err;
            }
        }
    }

}