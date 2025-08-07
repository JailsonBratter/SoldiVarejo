using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using visualSysWeb.code;
using System.Collections;

namespace visualSysWeb.dao
{
    public class OrigemFinalizadoraDAO
    {
        public DateTime emissao { get; set; }
        public string hora { get; set; }
        public int fechamento { get; set; }
        public int operador { get; set; }
        public int pdv { get; set; }
        public string cupom { get; set; }
        public int sequencia { get; set; }
        public int codFinalizadora { get; set; }
        public decimal totalFinalizadora { get; set; }

        public bool salvar(List<AlteracaoFinalizadoraDAO> finalizadorasAcerto)
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);

            decimal valorSubtrairFinalizadora = 0;

            try
            {
                string sql = "DELETE FROM Tesouraria_Detalhes WHERE ID_Fechamento = " + fechamento.ToString() + " AND";
                sql += " PDV = " + pdv.ToString() + " AND ";
                sql += " Operador = " + operador + " AND ";
                sql += " Finalizadora = " + codFinalizadora.ToString() + " AND ";
                sql += " Cupom = '" + cupom + "'";

                SqlCommand cmdDel = new SqlCommand();
                cmdDel.CommandText = sql;
                cmdDel.CommandType = CommandType.Text;
                cmdDel.Connection = conn;
                cmdDel.Transaction = tran;
                cmdDel.ExecuteNonQuery();

                int sequencia = 0;
                foreach (AlteracaoFinalizadoraDAO fin in finalizadorasAcerto)
                {
                    valorSubtrairFinalizadora += fin.valor;
                    sequencia += 100;
                    finalizadoraDAO finalizadoraCad = new finalizadoraDAO(fin.finalizadora);

                    int codigoRedeAutorizacao = 0;
                    int.TryParse(Conexao.retornaUmValor("SELECT Autorizadora.ID FROM Autorizadora WHERE Descricao = '" + fin.autorizadora + "'", null), out codigoRedeAutorizacao);

                    CartaoDAO cartaoCad = new CartaoDAO(fin.cartao, codigoRedeAutorizacao);


                    SqlCommand cmdIns = new SqlCommand();

                    //emissao, pdv, operador, finalizadora, cupom, total, id_finalizadora, id_cartao, autorizacao, id_bandeira, rede_cartao, filial , hora_Venda, vencimento, taxa, id_fechamento, sequencia

                    string sqlIns = "INSERT INTO Tesouraria_Detalhes (";
                    sqlIns += "emissao, pdv, operador, finalizadora, cupom, total, id_finalizadora, id_cartao, autorizacao";
                    sqlIns += ", id_bandeira, rede_cartao, filial , hora_Venda, vencimento, taxa, id_fechamento, sequencia";
                    sqlIns += ") VALUES (";
                    sqlIns += "'" + emissao.ToString("yyyy-MM-dd") + "'";
                    sqlIns += ", " + pdv.ToString();
                    sqlIns += ", " + operador.ToString();
                    sqlIns += ", " + finalizadoraCad.Nro_Finalizadora.ToString();
                    sqlIns += ", '" + cupom + "'";
                    sqlIns += ", " + fin.valor.ToString().Replace(",", ".");
                    sqlIns += ", '" + fin.finalizadora + "'";
                    sqlIns += ", '" + fin.cartao + "'";
                    sqlIns += ", '" + fin.codigoAutorizacao + "'";
                    sqlIns += ", '" + (cartaoCad.bandeira != null ? cartaoCad.bandeira.ToString() : "") + "'";
                    sqlIns += ", '" + (cartaoCad.rede != null ?  cartaoCad.rede.ToString() : "") + "'";
                    sqlIns += ", 'MATRIZ'";
                    sqlIns += ", '" + hora + "'";
                    sqlIns += ", '" + emissao.ToString("yyyy-MM-dd") + "'";
                    sqlIns += ", " + cartaoCad.taxa.ToString().Replace(",", ".");
                    sqlIns += ", " + fechamento.ToString();
                    sqlIns += ", " + sequencia.ToString();
                    sqlIns += ")";

                    cmdIns.CommandText = sqlIns;
                    cmdIns.CommandType = CommandType.Text;
                    cmdIns.Connection = conn;
                    cmdIns.Transaction = tran;
                    cmdIns.ExecuteNonQuery();
                }

                //Acertar tabela Tesouraria

                sql = "UPDATe Tesouraria SET Tesouraria.Total_Sistema = Tesouraria.Total_Sistema - " + valorSubtrairFinalizadora.ToString().Replace(",", ".") + " WHERE";
                sql += " ID_Fechamento = " + fechamento.ToString() + " AND";
                sql += " PDV = " + pdv.ToString() + " AND ";
                sql += " ID_Operador = " + operador + " AND ";
                sql += " Finalizadora = " + codFinalizadora.ToString();

                SqlCommand cmdUpt = new SqlCommand();
                cmdUpt.CommandText = sql;
                cmdUpt.CommandType = CommandType.Text;
                cmdUpt.Connection = conn;
                cmdUpt.Transaction = tran;
                cmdUpt.ExecuteNonQuery();



                tran.Commit();
                return true;
            }
            catch
            {
                tran.Rollback();
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

    }
}