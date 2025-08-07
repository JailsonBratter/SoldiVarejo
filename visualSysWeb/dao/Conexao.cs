using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Collections;

namespace visualSysWeb.dao
{
    public class Conexao
    {

        public static SqlConnection novaConexao()
        {
            SqlConnection novaCnn;
            ConnectionStringSettings strConnect;
            strConnect = ConfigurationManager.ConnectionStrings["Conexao"];
            novaCnn = new SqlConnection(strConnect.ConnectionString);
            novaCnn.Open();
            return novaCnn;
        }

        public static String nomeBanco()
        {
            ConnectionStringSettings strConnect;
            strConnect = ConfigurationManager.ConnectionStrings["Conexao"];
            String strbanco = strConnect.ConnectionString;
            int inipos = strbanco.IndexOf("Initial Catalog=") + 16;
            int tam = strbanco.IndexOf(";User") - inipos;
            strbanco = strbanco.Substring(inipos, tam);

            return strbanco;
        }

        private static String primeiratb(String sql)
        {
            try
            {

                String tabela = "";
                int ini = 0;
                int len = 0;
                ini = sql.ToLower().IndexOf("from ") + 5;
                len = sql.IndexOf(" ", ini + 1) - ini;
                if (len <= 0)
                    tabela = sql.Substring(ini);
                else
                    tabela = sql.Substring(ini, len);
                return tabela;
            }
            catch (Exception err)
            {

                throw new Exception("Não foi possivel efetuar a Consulta erro: " + err.Message);
            }

        }

        public static String[] retornaArray(String sql, int Tamfiltro)
        {
            ArrayList array = new ArrayList();

            SqlDataReader rs = consulta(sql, null, false);

            int i = 0;
            while (rs.Read() && (Tamfiltro < 4 ? (i < 100) : true))
            {
                array.Add(rs[0].ToString().Trim());
                i++;
            }
            if (rs != null)
                rs.Close();

            return (String[])array.ToArray(typeof(String));
        }

        public static int countSql(String sql, User usr, SqlConnection conn, SqlTransaction tran)
        {
            SqlDataReader rs = null;
            try
            {
                int i = 0;
                rs = consulta(sql, usr, false, conn, tran);
                while (rs.Read())
                {
                    i++;
                }
                return i;
            }
            catch (Exception)
            {

                return -1;
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }
        }


        public static int countSql(String sql, User usr)
        {
            SqlDataReader rs = null;
            try
            {
                int i = 0;
                rs = consulta(sql, usr, false);
                while (rs.Read())
                {
                    i++;
                }
                return i;
            }
            catch (Exception err)
            {

                throw err;
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }
        }

        public static SqlDataReader consulta(String sql, User usr, bool limitar, SqlConnection conn, SqlTransaction tran)
        {
            return consulta(sql, usr, limitar, conn, tran, false);

        }

        private static SqlDataReader consulta(String sql, User usr, bool limitar, SqlConnection conn, SqlTransaction tran, bool finalizaConexao)
        {
            SqlDataReader rs = null;

            SqlCommand comando = conn.CreateCommand();
            comando.CommandTimeout = 1200;
            comando.Transaction = tran;

            String tabela = primeiratb(sql);
            if (limitar)
            {
                if (sql.IndexOf("TOP") < 0)
                {
                    if (sql.ToUpper().IndexOf("DISTINCT") < 0)
                    {
                        sql = sql.Replace("Select ", "Select top 100 ");
                        sql = sql.Replace("select ", "Select Top 100 ");
                        sql = sql.Replace("SELECT ", "Select top 100 ");
                    }
                    else
                    {
                        sql = sql.Replace("Select distinct", "Select distinct top 100 ");
                        sql = sql.Replace("select distinct", "Select distinct Top 100 ");
                        sql = sql.Replace("SELECT distinct", "Select distinct top 100 ");
                        sql = sql.Replace("SELECT Distinct", "Select distinct top 100 ");
                        sql = sql.Replace("select DISTINCT", "Select distinct top 100 ");
                    }
                }
            }
            tabela = tabela.Trim();
            if (usr != null && !usr.getFilial().Equals("") && !usr.consultaTodasFiliais &&
                !tabela.ToUpper().Equals("FORNECEDOR") &&
                !tabela.ToUpper().Equals("MERCADORIA") &&
                !tabela.ToUpper().Equals("NATUREZA_OPERACAO") &&
                !tabela.ToUpper().Equals("DEPARTAMENTO") &&
                !tabela.ToUpper().Equals("SUBGRUPO") &&
                !tabela.ToUpper().Equals("GRUPO") &&
                !tabela.ToUpper().Equals("TRIBUTACAO") &&
                !tabela.ToUpper().Equals("CLIENTE") &&
                !tabela.ToUpper().Equals("CFOP") &&
                !tabela.ToUpper().Equals("FORNECEDOR_MERCADORIA") &&
                !tabela.ToUpper().Equals("EAN") &&
                !tabela.ToUpper().Equals("TIPO") &&
                !tabela.ToUpper().Equals("PRECO_MERCADORIA") &&
                !sql.ToUpper().Substring(0, 4).Equals("EXEC")
                )
            {

                if (sql.ToUpper().IndexOf("WHERE") > 0)
                {
                    comando.CommandText = sql.ToUpper().Replace("WHERE", " WHERE (" + tabela.Trim() + ".Filial='" + usr.getFilial() + "') and ");
                }
                else
                {
                    comando.CommandText = sql + " WHERE " + tabela.Trim() + ".FILIAL = '" + usr.getFilial() + "'";
                }
            }
            else
            {
                comando.CommandText = sql;
            }


            try
            {
                if (finalizaConexao)
                {
                    rs = comando.ExecuteReader(CommandBehavior.CloseConnection);
                }
                else
                {
                    rs = comando.ExecuteReader();
                }
            }
            catch (Exception)
            {
                if (finalizaConexao)
                {
                    comando.Connection.Close();
                    conn = novaConexao();
                }

                comando = conn.CreateCommand();
                comando.CommandText = sql;
                if (finalizaConexao)
                {
                    rs = comando.ExecuteReader(CommandBehavior.CloseConnection);
                }
                else
                {
                    rs = comando.ExecuteReader();
                }

            }

            return rs;
        }


        public static SqlDataReader consulta(String sql, User usr, bool limitar)
        {
            SqlConnection conn = novaConexao();
            SqlTransaction tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);

            return consulta(sql, usr, limitar, conn, tran, true);

        }


        public static bool executarSqlCmd(String sql)
        {


            SqlCommand comando = new SqlCommand();
            comando.CommandTimeout = 1200;
            comando.Connection = novaConexao();
            try
            {
                comando.CommandText = sql;
                comando.ExecuteNonQuery();
                return true;
            }
            catch (Exception err)
            {
                if (err.Message.IndexOf("Violation of PRIMARY KEY") >= 0)
                    throw new Exception("Registro Dublicado");
                else
                {
                    throw err;
                }
            }
            finally
            {
                if (comando.Connection.State == ConnectionState.Open)
                {
                    comando.Dispose();
                    comando.Connection.Close();
                    SqlConnection.ClearPool(comando.Connection);
                }
            }


        }

        public static bool executarSql(String sql)
        {


            SqlCommand comando = new SqlCommand();
            comando.CommandTimeout = 1200;
            SqlDataReader rs = null;
            try
            {
                comando.Connection = novaConexao();
                comando.CommandText = sql;

                rs = comando.ExecuteReader(CommandBehavior.CloseConnection);
                return true;
            }
            catch (Exception err)
            {
                if (err.Message.IndexOf("Violation of PRIMARY KEY") >= 0)
                    throw new Exception("Registro Dublicado");
                else
                {
                    throw err;
                }
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }


        }

        public static bool executarSql(String sql, SqlConnection conn, SqlTransaction transaciton)
        {
            try
            {
                SqlCommand comando = conn.CreateCommand();
                comando.CommandTimeout = 1200;
                comando.CommandText = sql;
                comando.Transaction = transaciton;

                comando.ExecuteNonQuery();
                return true;
            }
            catch (Exception err)
            {
                if (err.Message.IndexOf("Violation of PRIMARY KEY") >= 0)
                    throw new Exception("Registro Dublicado");
                else
                {
                    throw err;
                }
            }


        }




        public static void preencherDDL(DropDownList ddl, String sql, String text, String value, User usr)
        {
            //SqlDataReader rs = consulta(sql, usr, false);

            ddl.DataTextField = text;
            ddl.DataValueField = value;
            ddl.DataSource = GetTable(sql, usr, false);//rs;
            ddl.DataBind();

            //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
        }

        public static void preencherDDL1Branco(DropDownList ddl, String sql, String text, String value, User usr)
        {
            ddl.Items.Clear();
            ddl.Items.Add("");
            SqlDataReader lista = Conexao.consulta(sql, usr, false);
            while (lista.Read())
            {
                ListItem item = new ListItem(lista[text].ToString(), lista[value].ToString());
                ddl.Items.Add(item);
            }

            if (lista != null)
                lista.Close();
            //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
        }

        public static void preencherDDL(DropDownList ddl, String sql)
        {
            ddl.Items.Clear();
            ddl.Items.Add("");
            SqlDataReader lista = Conexao.consulta(sql, null, false);
            while (lista.Read())
            {
                ddl.Items.Add(lista[0].ToString());
            }

            if (lista != null)
                lista.Close();

        }

        public static void preencherLISTCheckBox(CheckBoxList ddl, String sql, String text, String value, User usr)
        {
            ddl.DataTextField = text;
            ddl.DataValueField = value;
            ddl.DataSource = GetTable(sql, usr, false);//rs;
            ddl.DataBind();

            //ddl.Items.Clear();
            //ddl.Items.Add("");
            //SqlDataReader lista = Conexao.consulta(sql, null, false);
            //while (lista.Read())
            //{
            //    ddl.Items.Add(lista[0].ToString());
            //}

            //if (lista != null)
            //    lista.Close();

        }

        public static DataTable GetTable(String sql, User usr, bool limitar)
        {
            SqlDataReader sqlReader = null;
            try
            {


                DataTable outputTable = new DataTable();
                DataColumn dcColumn;
                DataRow drRow;




                sqlReader = consulta(sql, usr, limitar);

                DataTable schemaTable = sqlReader.GetSchemaTable();


                for (int i = 0; i < schemaTable.Rows.Count; i++)
                {
                    dcColumn = new DataColumn();
                    if (!outputTable.Columns.Contains(schemaTable.Rows[i]["ColumnName"].ToString()))
                    {
                        dcColumn.ColumnName = schemaTable.Rows[i]["ColumnName"].ToString();
                        dcColumn.Unique = Convert.ToBoolean(schemaTable.Rows[i]["IsUnique"]);
                        dcColumn.AllowDBNull = Convert.ToBoolean(schemaTable.Rows[i]["AllowDBNull"]);
                        dcColumn.ReadOnly = Convert.ToBoolean(schemaTable.Rows[i]["IsReadOnly"]);
                        if (sqlReader.HasRows)
                        {
                            dcColumn.DataType = Type.GetType(schemaTable.Rows[i]["DataType"].ToString());


                        }
                        outputTable.Columns.Add(dcColumn);
                    }
                }

                while (sqlReader.Read())
                {

                    drRow = outputTable.NewRow();
                    for (int i = 0; i < sqlReader.FieldCount; i++)
                    {
                        // String str = drRow[i].GetType().ToString();
                        if (drRow[i].GetType() != null && drRow[i].GetType().Equals(Type.GetType("")))
                        {
                            drRow[i] = sqlReader.GetValue(i).ToString().Replace("/", "");
                        }
                        else
                        {
                            try
                            {

                                drRow[i] = sqlReader.GetValue(i);

                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }
                    }
                    outputTable.Rows.Add(drRow);
                }

                if (outputTable.Rows.Count < 1)
                {
                    drRow = outputTable.NewRow();
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        drRow[i] = "------";
                    }


                    outputTable.Rows.Add(drRow);
                }

               
                return outputTable;
            }
            catch (Exception errTable)
            {

                return new DataTable();

            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }

        }

        public static DataTable GetArryTable(ArrayList arr)
        {
            ArrayList cab = (ArrayList)arr[0];


            DataTable outputTable = new DataTable();
            DataColumn dcColumn;
            DataRow drRow;


            for (int i = 0; i < cab.Count; i++)
            {
                dcColumn = new DataColumn();
                if (!outputTable.Columns.Contains(cab[i].ToString()))
                {
                    dcColumn.DataType = Type.GetType("System.String");
                    dcColumn.ColumnName = cab[i].ToString();
                    dcColumn.Unique = false;
                    dcColumn.AllowDBNull = false;
                    dcColumn.ReadOnly = true;
                    dcColumn.AllowDBNull = true;

                    outputTable.Columns.Add(dcColumn);
                }
            }

            for (int i = 1; i < arr.Count; i++)
            {

                drRow = outputTable.NewRow();
                ArrayList rw = (ArrayList)arr[i];
                int j = 0;
                foreach (String item in rw)
                {
                    drRow[j] = item;
                    j++;
                }
                outputTable.Rows.Add(drRow);
            }

            if (outputTable.Rows.Count < 1)
            {
                drRow = outputTable.NewRow();
                for (int i = 0; i < cab.Count; i++)
                {
                    drRow[i] = "------";
                }


                outputTable.Rows.Add(drRow);
            }


            return outputTable;
        }


        public static String retornaUmValor(String sql, User usr)
        {
            SqlDataReader rs = null;
            try
            {
                rs = consulta(sql, usr, false);
                if (rs.Read())
                {
                    return rs[0].ToString();
                }
                else
                {
                    return "";
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rs != null)
                {
                    rs.Close();
                }
            }
        }

        public static Hashtable TbValores(String sql, User usr)
        {
            Hashtable tb = new Hashtable();
            SqlDataReader rs = null;

            try
            {
                rs = Conexao.consulta(sql, usr, false);
                while (rs.Read())
                {
                    tb.Add(rs[0].ToString(), rs[1].ToString());
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

            return tb;
        }

        public static String retornaUmValor(String sql, User usr, SqlConnection conn, SqlTransaction tran)
        {
            SqlDataReader rs = null;
            try
            {
                rs = consulta(sql, usr, false, conn, tran);
                if (rs.Read())
                {
                    return rs[0].ToString();
                }
                else
                {
                    return "";
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rs != null)
                {
                    rs.Close();
                }
            }

        }
    }




}