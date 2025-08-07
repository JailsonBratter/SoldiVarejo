using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Collections;
using visualSysWeb.code;
using visualSysWeb.modulos.NotaFiscal.code;

namespace visualSysWeb.dao
{
    public class NF_CTeDAO
    {
        #region Propriedades
        public string Filial { get; set; }
        public string Chave_NFe { get; set; }
        public string Situacao { get; set; }
        public string Fornecedor { get; set; }
        public string Numero_Documento { get; set; }
        public int Serie { get; set; }
        public DateTime Emissao { get; set; }
        public DateTime Aquisicao { get; set; }
        public string Chave { get; set; }
        public int Tipo_CTe { get; set; }
        public string Chave_Substituicao { get; set; }
        public int Tipo_Frete { get; set; }
        public decimal ICMS_Base { get; set; }
        public decimal ICMS_Reducao { get; set; }
        public decimal ICMS_Aliquota { get; set; }
        public decimal ICMS_Valor { get; set; }
        public decimal Valor_Documento { get; set; }
        public decimal Valor_Desconto { get; set; }
        public int IBGE_Origem { get; set; }
        public int IBGE_Destino { get; set; }
        public DateTime Boleto_Vencimento {get;set;}
        #endregion

        public NF_CTeDAO()
        {

        }

        public NF_CTeDAO(string chave, User usr)
        {
            SqlConnection conn = Conexao.novaConexao();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM NF_CTe WHERE Chave = '" + chave + "'";
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        Filial = dr["Filial"].ToString();
                        Chave_NFe = dr["Chave_NFe"].ToString();
                        Situacao = dr["Situacao"].ToString();
                        Fornecedor = dr["Fornecedor"].ToString();
                        Numero_Documento = dr["Numero_Documento"].ToString();
                        Serie = Funcoes.intTry(dr["Serie"].ToString());
                        Emissao = DateTime.Parse(dr["Emissao"].ToString());
                        Aquisicao = DateTime.Parse(dr["Aquisicao"].ToString());
                        Chave = dr["Chave"].ToString();
                        Tipo_CTe = Funcoes.intTry(dr["Tipo_CTe"].ToString());
                        Chave_Substituicao = dr["Chave_Substituicao"].ToString();
                        Tipo_Frete = Funcoes.intTry(dr["Tipo_Frete"].ToString());
                        ICMS_Base = Funcoes.decTry(dr["ICMS_Base"].ToString());
                        ICMS_Reducao = Funcoes.decTry(dr["ICMS_Reducao"].ToString());
                        ICMS_Aliquota = Funcoes.decTry(dr["ICMS_Aliquota"].ToString());
                        ICMS_Valor = Funcoes.decTry(dr["ICMS_Valor"].ToString());
                        Valor_Documento = Funcoes.decTry(dr["Valor_Documento"].ToString());
                        Valor_Desconto = Funcoes.decTry(dr["Valor_Desconto"].ToString());
                        IBGE_Origem = Funcoes.intTry(dr["IBGE_Origem"].ToString());
                        IBGE_Destino = Funcoes.intTry(dr["IBGE_Destino"].ToString());
                    }
                }
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

        public bool salvar (SqlConnection conn, SqlTransaction transaction)
        {
            string sql = "";
            try
            {
                // Comando INSERT SQL
                sql = @"INSERT INTO nf_cte (Filial, Chave_NFe, Situacao, Fornecedor, Numero_Documento, Serie, Emissao, Aquisicao, Chave, Tipo_CTe, Chave_Substituicao, Tipo_Frete, ICMS_Base, ICMS_Reducao, ICMS_Aliquota, ICMS_Valor, Valor_Documento, Valor_Desconto, IBGE_Origem, IBGE_Destino, Boleto_Vencimento) 
                                VALUES (@Filial, @ChaveNFe, @Situacao, @Fornecedor, @Numero_Documento, @Serie, @Emissao, @Aquisicao, @Chave, @Tipo_CTe, @Chave_Substituicao, @Tipo_Frete, @ICMS_Base, @ICMS_Reducao, @ICMS_Aliquota, @ICMS_Valor, @Valor_Documento, @Valor_Desconto, @IBGE_Origem, @IBGE_Destino, @Boleto_Vencimento)";

                // Criando o comando SQL
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    // Adicionando parâmetros
                    command.Transaction = transaction;
                    command.Parameters.AddWithValue("@Filial", Filial);
                    command.Parameters.AddWithValue("@ChaveNFe", Chave_NFe);
                    command.Parameters.AddWithValue("@Situacao", Situacao);
                    command.Parameters.AddWithValue("@Fornecedor", Fornecedor);
                    command.Parameters.AddWithValue("@Numero_Documento", Numero_Documento);
                    command.Parameters.AddWithValue("@Serie", Serie);
                    command.Parameters.AddWithValue("@Emissao", Emissao);
                    command.Parameters.AddWithValue("@Aquisicao", Aquisicao);
                    command.Parameters.AddWithValue("@Chave", Chave);
                    command.Parameters.AddWithValue("@Tipo_CTe", Tipo_CTe);
                    command.Parameters.AddWithValue("@Chave_Substituicao", Chave_Substituicao);
                    command.Parameters.AddWithValue("@Tipo_Frete", Tipo_Frete);
                    command.Parameters.AddWithValue("@ICMS_Base", ICMS_Base);
                    command.Parameters.AddWithValue("@ICMS_Reducao", ICMS_Reducao);
                    command.Parameters.AddWithValue("@ICMS_Aliquota", ICMS_Aliquota);
                    command.Parameters.AddWithValue("@ICMS_Valor", ICMS_Valor);
                    command.Parameters.AddWithValue("@Valor_Documento", Valor_Documento);
                    command.Parameters.AddWithValue("@Valor_Desconto", Valor_Desconto);
                    command.Parameters.AddWithValue("@IBGE_Origem", IBGE_Origem);
                    command.Parameters.AddWithValue("@IBGE_Destino", IBGE_Destino);
                    command.Parameters.AddWithValue("@Boleto_Vencimento", Boleto_Vencimento);

                    // Executando o comando
                    int rowsAffected = command.ExecuteNonQuery();
                    //Console.WriteLine($"Linhas afetadas: {rowsAffected}");
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}