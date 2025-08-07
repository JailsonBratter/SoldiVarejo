using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class CartaoDAO
    {

        public string bandeira { get; set; }
        public string rede { get; set; }
        public decimal taxa { get; set; }
        public int dias { get; set; }
        
        public CartaoDAO(string idCartao)
        {
            try
            {
                using (SqlConnection conn = Conexao.novaConexao())
                {
                    //conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM Cartao WHERE id_Cartao = '" + idCartao + "'", conn))
                    {
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            bandeira = dr["id_bandeira"].ToString();
                            rede = dr["id_rede"].ToString();
                            taxa = Funcoes.decTry(dr["taxa"].ToString());
                            dias = Funcoes.intTry(dr["dias"].ToString());
                        }
                    }
                }
            }
            catch
            {

            }
        }
        public CartaoDAO(string idCartao, int idRede)
        {
            try
            {
                using (SqlConnection conn = Conexao.novaConexao())
                {
                    //conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM Cartao WHERE id_Cartao = '" + idCartao + "' AND id_Rede = " + idRede.ToString(), conn))
                    {
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            bandeira = dr["id_bandeira"].ToString();
                            rede = dr["id_rede"].ToString();
                            taxa = Funcoes.decTry(dr["taxa"].ToString());
                            dias = Funcoes.intTry(dr["dias"].ToString());
                        }
                    }
                }
            }
            catch
            {

            }
        }
    }
}