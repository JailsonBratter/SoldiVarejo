using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class FilialIEDAO
    {
        public string filial { get; set; }
        public string UF { get; set; }
        public string IE { get; set; }
        public int VctoTipo { get; set; }
        public int VctoQtde { get; set; }
        public bool DiasUteis { get; set; }
        public int VctoTipoQtde { get; set; }

        public static List<FilialIEDAO> buscarIE(string Filial)
        {
            List<FilialIEDAO> lista = new List<FilialIEDAO>();
            SqlConnection conn = Conexao.novaConexao();
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Filial_IE WHERE Filial = '" + Filial + "'", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            FilialIEDAO item = new FilialIEDAO();
                            item.filial = Filial;
                            item.UF = reader["UF"].ToString();
                            item.IE = reader["IE"].ToString();
                            item.VctoTipo = Funcoes.intTry(reader["Vcto_Difal_Tipo"].ToString());
                            item.VctoQtde = Funcoes.intTry(reader["Vcto_Difal_Qtde"].ToString());
                            item.DiasUteis = (Funcoes.intTry(reader["Vcto_Difal_Uteis"].ToString()) >= 1 ? true : false);
                            item.VctoTipoQtde = Funcoes.intTry(reader["Vcto_Difal_Tipo_Qtde"].ToString());
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