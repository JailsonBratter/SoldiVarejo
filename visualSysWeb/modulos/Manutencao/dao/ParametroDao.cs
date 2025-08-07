using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.dao;
using System.Data;

namespace visualSysWeb.modulos.Manutencao.dao
{
    public class ParametroDao
    {
        public String Parametro { get; set; }
        public String Valor_Default { get; set; }
        public String Valor_Atual { get; set; }
        public String Descricao { get; set; }

        public String Tipo_dado { get; set; }
        public bool Permite_por_empresa { get; set; }

        public ParametroDao()
        {

        }

        public ParametroDao(String Parametro)
        {
            this.Parametro = Parametro;
            String sql = "Select * from Parametros where parametro ='" + Parametro + "'";
            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta(sql, null,false);
                if(rs.Read())
                {
                    Valor_Default = rs["Valor_Default"].ToString();
                    Valor_Atual = rs["Valor_Atual"].ToString();
                    Descricao = rs["DESC_PARAMETRO"].ToString();
                    Permite_por_empresa = rs["PERMITE_POR_EMPRESA"].ToString().Equals("1");
                    Tipo_dado = rs["TIPO_DADO"].ToString();
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

        public void salvar()
        {
            String sql = "update parametros set ";

            sql += " Valor_Atual= '" + Valor_Atual + "'";

            sql += " Where PARAMETRO ='" + Parametro + "'";

            Conexao.executarSql(sql);
        }

        public DataTable dtParametros(string[] parametros)
        {
            string sql = "SELECT Parametro, ISNULL(VALOR_ATUAL, '') AS ValorAtual, ISNULL(Desc_Parametro, '') AS Descritivo FROM Parametros WHERE Parametros.Parametro IN(";
            foreach (String arr in parametros)
            {
                if (!sql.Substring(sql.Length - 1, 1).Equals("("))
                {
                    sql += ", ";
                }
                sql += "'" + arr + "'";

            }
            sql += ") ORDER BY 1";

            DataTable dtParametros = null;

            try
            {
                dtParametros = Conexao.GetTable(sql, null, false);
                return dtParametros;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dtParametros != null)
                    dtParametros.Dispose();
            }
        }
    }
}