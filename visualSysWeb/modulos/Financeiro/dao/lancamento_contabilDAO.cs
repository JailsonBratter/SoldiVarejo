using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using visualSysWeb.code;
using System.Data.SqlClient;

namespace visualSysWeb.dao
{
    public class lancamento_contabilDAO
    {
        public string filial { get; set; }
        public string clienteFornecedor { get; set; }
        public string numeroDocumento { get; set; }
        public int idlancamento { get; set; }
        public DateTime data { get; set; }
        public string contaDebito { get; set; }
        public string contaCredito { get; set; }
        public string complemento { get; set; }
        public decimal valor { get; set; }

        public lancamento_contabilDAO()
        {

        }
        public lancamento_contabilDAO(string Filial, string Fornecedor, string Documento)
        {
            SqlDataReader dr = Conexao.consulta("SELECT * FROM Lancamentos_Contabeis WHERE Filial = '" + Filial + "' AND Cliente_Fornecedor = '" + Fornecedor + "' AND Numero_Documento = '" + Documento + "'", null, false);
            if (dr.Read())
            {
                this.filial = Filial;
                this.clienteFornecedor = dr["Cliente_Fornecedor"].ToString();
                this.numeroDocumento = dr["Numero_Documento"].ToString();
                this.idlancamento = int.Parse(dr["Id_Lancamento"].ToString());
                this.data = DateTime.Parse(dr["data"].ToString());
                this.contaDebito = dr["Conta_Debito"].ToString();
                this.contaCredito = dr["Conta_Credito"].ToString();
                this.complemento = dr["Complemento"].ToString();
                this.valor = Funcoes.decTry(dr["valor"].ToString());
            }

        }

        public void insert(SqlConnection conn, SqlTransaction tran)
        {
            string sql = "INSERT INTO Lancamentos_Contabeis ";
            sql += "(";
            sql += "Filial";
            sql += ", Cliente_Fornecedor";
            sql += ", Numero_Documento";
            sql += ", ID_Lancamento";
            sql += ", Data";
            sql += ", Conta_Debito";
            sql += ", Conta_Credito";
            sql += ", Complemento";
            sql += ", Valor";
            sql += ") VALUES (";
            try
            {
                sql += "'" + this.filial + "'";
                sql += ", '" + this.clienteFornecedor + "'";
                sql += ", '" + this.numeroDocumento + "'";
                sql += ", " + this.idlancamento.ToString();
                sql += ", '" + this.data.ToString("yyyy-MM-dd") + "'";
                sql += ", '" + this.contaDebito + "'";
                sql += ", '" + this.contaCredito + "'";
                sql += ", '" + this.complemento + "'";
                sql += ", " + this.valor.ToString().Replace(",", ".");
                sql += ")";

                Conexao.executarSql(sql, conn, tran);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void delete(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                string sql = "DELETE FROM Lancamentos_Contabeis WHERE";
                sql += " Filial = '" + this.filial + "'";
                sql += " AND Cliente_Fornecedor = '" + this.clienteFornecedor + "'";
                sql += " AND Numero_Documento = '" + this.numeroDocumento + "'";

                Conexao.executarSql(sql, conn, tran);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}