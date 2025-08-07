using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace visualSysWeb.dao
{
    public class NF_Recebimento_ItensDAO
    {
        public string plu { get; set; }
        public string lote { get; set; }
        public DateTime validade { get; set; }
        public int qtdeRecebida { get; set; }
        public int qtdePrevista { get; set; }
        public int qtdeNota { get; set; }

        public NF_Recebimento_ItensDAO()
        {

        }

        public List<NF_Recebimento_ItensDAO> listaItens(SqlConnection conn, String chave)
        {
            List<NF_Recebimento_ItensDAO> lista = new List<NF_Recebimento_ItensDAO>();
            try
            {
                string sql = "SELECT plu, isnull(lote, '') AS lote, validade, ISNULL(qtde_Recebida, 0) as QtdeRecebida, ISNULL(Qtde_Prevista, 0) AS ";
                sql += " QtdePrevista, ISNULL(Qtde_Nota, 0) AS QtdeNota FROM NF_Recebimento_Itens WHERE id = '" + chave + "'";
                using (SqlCommand cmd = new SqlCommand(""))
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = System.Data.CommandType.Text;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            NF_Recebimento_ItensDAO item = new NF_Recebimento_ItensDAO();
                            item.plu = dr["PLU"].ToString();
                            item.lote = dr["Lote"].ToString();
                            item.validade = DateTime.Parse(dr["validade"].ToString());
                            item.qtdeRecebida = int.Parse(dr["QtdeRecebida"].ToString());
                            item.qtdePrevista = int.Parse(dr["QtdePrevista"].ToString());
                            item.qtdeNota = int.Parse(dr["QtdeNota"].ToString());
                            lista.Add(item);
                        }
                    }
                }
                return lista;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}