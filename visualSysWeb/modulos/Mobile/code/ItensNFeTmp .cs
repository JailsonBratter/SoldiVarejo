using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using visualSysWeb.dao;

namespace visualSysWeb.code
{
    public class ItensNFeTmp
    {
        public string plu { get; set; } = "";
        public int qtde { get; set; } = 0;
        public DateTime dataValidade { get; set; }

        public List<ItensNFeTmp> itens;

        public ItensNFeTmp()
        {

        }

        public ItensNFeTmp(string chave)
        {
            itens = new List<ItensNFeTmp>();
            SqlConnection conn = Conexao.novaConexao();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT * FROM NF_Recebimento_Itens_Temp WHERE ID = '" + chave + "'";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = conn;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ItensNFeTmp item = new ItensNFeTmp();
                            item.plu = dr["PLU"].ToString();
                            item.qtde = Funcoes.intTry(dr["qtde"].ToString());
                            try
                            {
                                item.dataValidade = DateTime.Parse(dr["Validade"].ToString());
                            }
                            catch
                            {
                                item.dataValidade = DateTime.Parse("1900-01-01");
                            }
                            itens.Add(item);
                        }
                    }
                }
            }
            catch (Exception e)
            {

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

        public void salvar(string chave)
        {
            try
            {
                Conexao.executarSqlCmd("DELETE FROM NF_Recebimento_Itens_Temp WHERE ID = '" + chave + "' AND PLU = '" + plu + "'");

                string sql = "INSERT INTO NF_Recebimento_Itens_Temp (ID, PLU, VALIDADE, QTDE) VALUES (";
                sql += "'" + chave + "'";
                sql += ", '" + plu + "'";
                sql += ", '" + dataValidade.ToString("yyyy-MM-dd") + "'";
                sql += ", " + qtde.ToString().Replace(",", ".");
                sql += ")";

                Conexao.executarSqlCmd(sql);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void excluir(string chave)
        {
            try
            {
                Conexao.executarSqlCmd("DELETE FROM NF_Recebimento_Itens_Temp WHERE ID = '" + chave + "'");
            }
            catch 
            {
            }
        }

    }
}