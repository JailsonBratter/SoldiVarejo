using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

using visualSysWeb.dao;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Estoque.dao
{
    public class DevolucaoNFeDAO
    {
        public string filial { get; set; }
        public int codigo { get; set; }
        public int status { get; set; }
        public string codigo_cliente { get; set; }
        public DateTime data_cadastro { get; set; }
        public string horacadastro { get; set; }
        public decimal total { get; set; }
        public string origem { get; set; }
        public DateTime emissaonfe { get; set; }
        public int numeronfe { get; set; }
        public string chavenfe { get; set; }
        public string usuario { get; set; }
        public string obs { get; set; }

        public DevolucaoNFeDAO()
        {

        }

        public DevolucaoNFeDAO(int codigo)
        {
            string sql = "SELECT * FROM Devolucao_NFe DNFe WHERE DNFe.Codigo = " + codigo.ToString();
            SqlDataReader dr = Conexao.consulta(sql, null, false);

            if (dr.HasRows)
            {
                carregarDados(dr);
            }
        }

        private void carregarDados(SqlDataReader dr)
        {
            try
            {
                if (dr.Read())
                {
                    filial = dr["filial"].ToString();
                    codigo = int.Parse(dr["codigo"].ToString());
                    status = int.Parse(dr["status"].ToString());
                    codigo_cliente = dr["codigo_cliente"].ToString();
                    data_cadastro = DateTime.Parse(dr["data_cadastro"].ToString());
                    horacadastro = dr["horaCadastro"].ToString();
                    total = Funcoes.decTry(dr["Total"].ToString());
                    origem = dr["Origem"].ToString();
                    if (dr["EmissaoNFe"] == null || dr["EmissaoNFe"].ToString().Equals(""))
                    {
                        emissaonfe = DateTime.MinValue;
                    }
                    else
                    {
                        emissaonfe = DateTime.Parse(dr["EmissaoNFe"].ToString());
                    }
                    numeronfe = Funcoes.intTry(dr["NumeroNFe"].ToString());
                    chavenfe = dr["ChaveNFe"].ToString();
                    usuario = dr["Usuario"].ToString();
                    obs = dr["Obs"].ToString();
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
            }
        }
    }
}