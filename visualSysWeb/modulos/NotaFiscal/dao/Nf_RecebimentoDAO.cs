using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class Nf_RecebimentoDAO
    {
        public string filial { get; set; }
        public string fornecedor { get; set; }
        public string ID { get; set; }
        public string codigo { get; set; }
        public DateTime emissao { get; set; }
        public decimal total { get; set; }
        public string usuario { get; set; }
        public int status { get; set; }
        public List<NF_Recebimento_ItensDAO> itens { get; set; }

        public Nf_RecebimentoDAO()
        {

        }

        public Nf_RecebimentoDAO(User usr, string id)
        {
            SqlConnection conn = Conexao.novaConexao();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM NF_Recebimento WHERE ID = '" + id + "'");
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();

                    filial = dr["Filial"].ToString();
                    fornecedor = dr["cliente_fornecedor"].ToString();
                    ID = id;
                    codigo = dr["codigo"].ToString();
                    emissao = DateTime.Parse(dr["emissao"].ToString());
                    usuario = dr["usuario"].ToString();
                    try
                    {
                        status = int.Parse(dr["Status"].ToString());
                    }
                    catch
                    {
                        status = 0;
                    }

                    SqlConnection connItem = Conexao.novaConexao();
                    NF_Recebimento_ItensDAO nfItem = new NF_Recebimento_ItensDAO();
                    itens = nfItem.listaItens(connItem, id);

                    if (connItem != null)
                    {
                        connItem.Close();
                        connItem.Dispose();
                        SqlConnection.ClearPool(connItem);
                    }
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