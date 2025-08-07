using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using visualSysWeb.dao;
using visualSysWeb.code;

namespace visualSysWeb.modulos.NotaFiscal.code
{
    public class Produto
    {
        public string filial { get; set; }
        public string plu { get; set; }
        public decimal custo1 { get; set; }
        public decimal custo { get; set; }
        public decimal preco { get; set; }

        public Produto(string filial, string plu)
        {
            SqlConnection conn = Conexao.novaConexao();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT TOP 1 filial, plu, preco_custo, preco_custo_1, preco FROM Mercadoria_Loja WHERE Filial = '" + filial + "' AND PLU = '" + plu + "'";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = conn;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            filial = dr["filial"].ToString();
                            plu = dr["plu"].ToString();
                            custo1 = Funcoes.decTry(dr["preco_custo_1"].ToString());
                            custo = Funcoes.decTry(dr["preco_custo"].ToString());
                            preco = Funcoes.decTry(dr["preco"].ToString());
                        }
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
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