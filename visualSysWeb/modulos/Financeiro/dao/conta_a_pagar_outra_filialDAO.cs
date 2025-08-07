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
    public class conta_a_pagar_outra_filialDAO
    {
        public string filial { get; set; }
        public string fornecedor { get; set; }
        public string documento { get; set; }
        public string numero_nf { get; set; }
        public int serie { get; set; }
        public DateTime emissao { get; set; }
        public DateTime vencimento { get; set; }
        public string codigo_centro_custo { get; set; }
        public decimal valor { get; set; }
        public string usuario { get; set; }
        public int integrado { get; set; }
        public DateTime datahoraIntegracao { get; set; }

        public conta_a_pagar_outra_filialDAO()
        {

        }

        public void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = "INSERT INTO Conta_a_Pagar_Outra_Filial (" +
                    "Filial" +
                    ", Fornecedor" +
                    ", Documento" +
                    ", Numero_NF" +
                    ", Serie" +
                    ", Emissao" +
                    ", Vencimento" +
                    ", Codigo_Centro_Custo" +
                    ", Valor" +
                    ", Usuario" +
                    ", Integrado" +
                    ", DataHora_Integracao" +
                    ") VALUES (" +
                    "'" + this.filial + "'" +
                    ", '" + this.fornecedor + "'" +
                    ", '" + this.documento + "'" +
                    ", '" + this.numero_nf + "'" +
                    ", " + this.serie.ToString() +
                    ", '" + this.emissao.ToString("yyyy-MM-dd") + "'" +
                    ", '" + this.vencimento.ToString("yyyy-MM-dd") + "'" +
                    ", '" + this.codigo_centro_custo + "'" +
                    ", " + this.valor.ToString().Replace(",", ".") +
                    ", '" + this.usuario + "'" +
                    ", 0" +
                    ", '1900-01-01'"+
                    ")";
                Conexao.executarSql(sql, conn, tran);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}