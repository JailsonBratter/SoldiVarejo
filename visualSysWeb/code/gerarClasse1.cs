using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace visualSysWeb.code
{
    public static  class gerarClasse
    {
        

        public static void gerar(String tabela){
                StringBuilder propriedades= new StringBuilder();
                String sql = "SELECT " +
                              "     COLUNAS.NAME AS COLUNA," +
                              "     TIPOS.NAME AS TIPO," +
                              "     COLUNAS.LENGTH AS TAMANHO," +
                              "     COLUNAS.ISNULLABLE AS EH_NULO" +
                              " FROM " +
                              "     SYSOBJECTS AS TABELAS," +
                              "     SYSCOLUMNS AS COLUNAS," +
                              "     SYSTYPES   AS TIPOS" +
                              " WHERE " +
                              "     TABELAS.ID = COLUNAS.ID" +
                              "     AND COLUNAS.USERTYPE = TIPOS.USERTYPE" +
                              "     AND TABELAS.NAME = '" + tabela + "'"+
                               " ORDER BY colorder ";
               
            //Cria Arquivo
                StreamWriter valor = new StreamWriter("C:\\codigo\\"+tabela + "DAO.cs", true, Encoding.ASCII);
            //Escreve no Arquivo    
                valor.WriteLine("using System;");
                valor.WriteLine("using System.Collections.Generic;");
                valor.WriteLine("using System.Linq;");
                valor.WriteLine("using System.Web;");
                valor.WriteLine("using System.Data;");
                valor.WriteLine("using System.Data.SqlClient;");
                valor.WriteLine("namespace visualSysWeb.dao");
                valor.WriteLine("{");
                valor.WriteLine("public class "+tabela+"Dao");
                valor.WriteLine("   {");
                SqlDataReader rs =null;
                try
                {
                    rs = Conexao.consulta(sql);
                    while (rs.Read()){
                          valor.WriteLine("     public " +tipo(rs["TIPO"].ToString())+" "+rs["COLUNA"].ToString() +" { get; set; }");            
                    }
                    valor.WriteLine("   public "+tabela+"Dao(){");
                    valor.WriteLine("       String sql=\"Select * from  " + tabela + "\";");
                    valor.WriteLine("       SqlDataReader rs = Conexao.consulta(sql);");
                    
                    valor.WriteLine("       carregarDados(rs);");
                    valor.WriteLine("   }");
                    valor.WriteLine(" ");
                    valor.WriteLine("  public void carregarDados(SqlDataReader rs){");
                  
                  
                  
                    rs.Close();
                    rs = Conexao.consulta(sql);
                    while (rs.Read())
                    {
                        String strTipo = tipo(rs["TIPO"].ToString());
                        String strlinha = "";
                        switch (strTipo)
                        {
                            case "String":
                                strlinha = "      " + rs["COLUNA"].ToString() + " = rs[\"" + rs["COLUNA"].ToString() + "\"].ToString();";
                                break;
                            case "Decimal":
                                strlinha = "      " + rs["COLUNA"].ToString() + " = (Decimal)rs[\"" + rs["COLUNA"].ToString() + "\"];";
                                break;
                            case "DateTime":
                                strlinha = "      " + rs["COLUNA"].ToString() + " = DateTime.Parse(rs[\"" + rs["COLUNA"].ToString() + "\"].ToString());";
                                break;
                            case "bool":
                                strlinha = "      " + rs["COLUNA"].ToString() + " = (rs[\"" + rs["COLUNA"].ToString() + "\"].ToString().Equals(\"1\")?true:false);";
                                break;
                            case "int":
                                strlinha = "      " + rs["COLUNA"].ToString() + " = int.Parse(rs[\"" + rs["COLUNA"].ToString() + "\"].ToString());";
                                break;
                            default:
                                strlinha = ""+ rs["COLUNA"].ToString()+"tipo Não encontrado ";
                                break;

                        
                        }
                        valor.WriteLine(strlinha);
                    }

                    valor.WriteLine("       }");



                    valor.WriteLine("   }");

                    valor.WriteLine("}");
                 

                }
                catch (Exception)
                {

                }
                finally {
                    rs.Close();
                }
                valor.Close();

        }

        private static String tipo(String tipo) {
            switch (tipo)
            {
                case "varchar":
                    return "String";
                case "numeric":
                    return "Decimal";
                case "datetime":
                    return "DateTime";
                    
                case "tinyint":
                    return "bool";
                case "text":
                    return "String";
                case "int":
                    return "int";
                case "decimal":
                    return "Decimal";
                case "double":
                    return "Decimal";
                default:
                    return "TIPO NÃO ENCONTRADO!!!!!!!!!!!!!!!!!!!!!";
                
            }

        }

    }
}