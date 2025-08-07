using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using visualSysWeb.dao;

namespace visualSysWeb.code
{
    public class ItensNFe
    {
        public string plu { get; set; } = "";
        public int qtde { get; set; } = 0;
        public int emb { get; set; } = 0;
        public int qtdetotal { get; set; } = 0;
        public int qtdeRecebida { get; set; } = 0;
        public string descricao { get; set; } = "";
        public DateTime dataValidade { get; set; }

        public ItensNFe()
        {

        }

        public ItensNFe(string codigo)
        {
            string sql = "SELECT TOP 1 PLU, Descricao FROM Mercadoria WHERE ";
            if (codigo.Trim().Length < 6)
            {
                sql += " plu = '" + codigo + "'";
            }
            else
            {
                sql += " PLU IN(SELECT TOP 1 plu FROM EAN WHERE CONVERT(BIGINT, EAN) = " + long.Parse(codigo) + ")";
            }
            SqlDataReader dr = Conexao.consulta(sql, null, false);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    plu = dr["PLU"].ToString();
                    //descricao = dr["descricao"].ToString();
                }
            }

        }

        public void salvar(User usr, SqlConnection conn, SqlTransaction tran, string fornecedor, string chave, string numero)
        {
            string strTexto = "";
            try
            {
                DateTime validadeControle = DateTime.Parse("1900-01-01");
                string sql = "INSERT INTO NF_Recebimento_Itens (FILIAL, CLIENTE_FORNECEDOR, ID, CODIGO, PLU, LOTE, VALIDADE, QTDE_RECEBIDA, QTDE_PREVISTA, QTDE_NOTA) VALUES (";
                sql += "'" + usr.getFilial() + "'";
                sql += ", '" + fornecedor + "'";
                sql += ", '" + chave + "'";
                sql += ", '" + numero + "'";
                sql += ", '" + plu + "'";
                sql += ", ''";
                if (dataValidade < validadeControle)
                {
                    sql += ", '" + validadeControle.ToString("yyyy-MM-dd") + "'";
                }
                else
                {
                    sql += ", '" + dataValidade.ToString("yyyy-MM-dd") + "'";
                }
                sql += ", " + qtdeRecebida.ToString().Replace(",", ".");
                sql += ", " + qtdetotal.ToString().Replace(",", ".");
                sql += ", " + (qtde * emb).ToString().Replace(",", ".");
                sql += ")";

                Console.WriteLine(sql);

                strTexto += sql;
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = tran;
                cmd.CommandText = sql;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }

        }

    }
}