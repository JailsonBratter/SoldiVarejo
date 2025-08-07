using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using visualSysWeb.dao;
using System.Data.SqlClient;

namespace visualSysWeb.code
{
    public class Nota
    {
        public DateTime emissao { get; set; }
        public string numero { get; set; }
        public string fornecedor { get; set; }
        public decimal valorNF { get; set; }
        public string chave { get; set; }

        public List<ItensNFe> itens;

        public Nota()
        {

        }

        public bool salvar(User usr)
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction trans = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            try
            {
                string sql = "INSERT INTO NF_Recebimento (filial, cliente_fornecedor, id, codigo, emissao, total, usuario, status) VALUES (";
                sql += "'" + usr.getFilial() + "'";
                sql += ", '" + fornecedor + "'";
                sql += ", '" + chave + "'";
                sql += ", '" + numero + "'";
                sql += ", '" + emissao.ToString("yyyy-MM-dd") + "'";
                sql += ", " + valorNF.ToString().Replace(",", ".");
                sql += ", '" + usr.getUsuario().ToString() + "'";
                sql += ", 1";
                sql += ")";

                //Console.WriteLine(sql);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;
                cmd.Transaction = trans;
                cmd.ExecuteNonQuery();

                foreach (ItensNFe item in itens)
                {
                    item.salvar(usr, conn, trans, fornecedor, chave, numero);
                }

                trans.Commit();
                return true;
            }
            catch (Exception e)
            {
                trans.Rollback();
                return false;
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

        public void atualizaStatus(string chave)
        {
            SqlConnection conn = Conexao.novaConexao();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "UPDATE NF_Recebimento.Status = 2 WHERE NF_Recebimento.ID = '" + chave + "'";
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();
                }
            }
            catch
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
    }
}