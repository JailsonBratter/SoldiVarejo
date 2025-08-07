using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace visualSysWeb.dao
{
    public class pedido_itens_tempDAO
    {
        // Propriedades
        public string Filial { get; set; }
        public string ipOrigem { get; set; }
        public string Usuario { get; set; }
        public string codigoCliente { get; set; }
        public DateTime DTCadastro { get; set; }
        public string ID { get; set; }
        public int Sequencia { get; set; }
        public string PLU { get; set; }
        public decimal Qtde { get; set; }
        public decimal Embalagem { get; set; }
        public decimal Unitario { get; set; }
        public decimal Desconto { get; set; }

        // Método para inserir um registro
        public void Insert()
        {
            using (SqlConnection conn = Conexao.novaConexao())
            {
                string query = "INSERT INTO Pedido_Itens_Temp (Filial, Usuario, IP_Origem, codigo_cliente, DTCadastro, ID, Sequencia, PLU, Qtde, Embalagem, Unitario, Desconto) " +
                               "VALUES (@Filial, @Usuario, @IPOrigem, @codigoCliente, @DTCadastro, @ID, @Sequencia, @PLU, @Qtde, @Embalagem, @Unitario, @Desconto)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Filial", Filial);
                cmd.Parameters.AddWithValue("@Usuario", Usuario);
                cmd.Parameters.AddWithValue("@IPOrigem", ipOrigem);
                cmd.Parameters.AddWithValue("@codigoCliente", codigoCliente);
                cmd.Parameters.AddWithValue("@DTCadastro", DTCadastro);
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@Sequencia", Sequencia);
                cmd.Parameters.AddWithValue("@PLU", PLU);
                cmd.Parameters.AddWithValue("@Qtde", Qtde);
                cmd.Parameters.AddWithValue("@Embalagem", Embalagem);
                cmd.Parameters.AddWithValue("@Unitario", Unitario);
                cmd.Parameters.AddWithValue("@Desconto", Desconto);

                //conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Método para atualizar um registro
        public void Update()
        {
            using (SqlConnection conn = Conexao.novaConexao())
            {
                string query = "UPDATE Pedido_Itens_Temp SET @DTCadastro = @DTCadastro, Qtde = @Qtde, Embalagem = @Embalagem, Unitario = @Unitario, Desconto = @Desconto " +
                               "WHERE Filial = @Filial AND ID = @ID AND IP_Origem = @IPOrigem AND Sequencia = @Sequencia AND PLU = @PLU";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Filial", Filial);
                cmd.Parameters.AddWithValue("@IPOrigem", ipOrigem);
                cmd.Parameters.AddWithValue("@DtCadastro", DTCadastro);
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@Sequencia", Sequencia);
                cmd.Parameters.AddWithValue("@PLU", PLU);
                cmd.Parameters.AddWithValue("@Qtde", Qtde);
                cmd.Parameters.AddWithValue("@Embalagem", Embalagem);
                cmd.Parameters.AddWithValue("@Unitario", Unitario);
                cmd.Parameters.AddWithValue("@Desconto", Desconto);

                cmd.ExecuteNonQuery();
            }
        }

        // Método para deletar um registro
        public void Delete()
        {
            using (SqlConnection conn = Conexao.novaConexao())
            {
                string query = "DELETE FROM Pedido_Itens_Temp WHERE Filial = @Filial AND ID = @ID AND IP_Origem = @IPOrigem AND Codigo_Cliente = @CodigoCliente AND Sequencia = @Sequencia AND PLU = @PLU";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Filial", Filial);
                cmd.Parameters.AddWithValue("@CodigoCliente", codigoCliente);
                cmd.Parameters.AddWithValue("@IPOrigem", ipOrigem);
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@Sequencia", Sequencia);
                cmd.Parameters.AddWithValue("@PLU", PLU);

                cmd.ExecuteNonQuery();
            }
        }
        // Método para deletar um registro
        public void DeleteTmp()
        {
            using (SqlConnection conn = Conexao.novaConexao())
            {
                string query = "DELETE FROM Pedido_Itens_Temp WHERE Filial = @Filial AND ID = @ID AND IP_Origem = @IPOrigem AND Codigo_Cliente = @CodigoCliente";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Filial", Filial);
                cmd.Parameters.AddWithValue("@IPOrigem", ipOrigem);
                cmd.Parameters.AddWithValue("@CodigoCliente", codigoCliente);
                cmd.Parameters.AddWithValue("@ID", ID);

                cmd.ExecuteNonQuery();
            }
        }

        // Método para buscar um registro
        public static List<pedido_itens_tempDAO> SelectById(string id, string codigoCliente, string ipOrigem)
        {
            List<pedido_itens_tempDAO> lista = new List<pedido_itens_tempDAO>();
            using (SqlConnection conn = Conexao.novaConexao())
            {
                string query = "SELECT * FROM Pedido_Itens_Temp WHERE ID = @ID AND IP_Origem = @IPOrigem AND Codigo_Cliente = @CodigoCliente";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@CodigoCliente", codigoCliente);
                cmd.Parameters.AddWithValue("@IPOrigem", ipOrigem);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pedido_itens_tempDAO ped =  new pedido_itens_tempDAO
                        {
                            Filial = reader["Filial"].ToString(),
                            Usuario = reader["Usuario"].ToString(),
                            DTCadastro = Convert.ToDateTime(reader["DTCadastro"]),
                            ID = reader["ID"].ToString(),
                            Sequencia = Convert.ToInt32(reader["Sequencia"]),
                            PLU = reader["PLU"].ToString(),
                            Qtde = Convert.ToDecimal(reader["Qtde"]),
                            Embalagem = Convert.ToDecimal(reader["Embalagem"]),
                            Unitario = Convert.ToDecimal(reader["Unitario"]),
                            Desconto = Convert.ToDecimal(reader["Desconto"])
                        };
                        lista.Add(ped);
                    }
                }
            }
            return lista;
        }
    }
}