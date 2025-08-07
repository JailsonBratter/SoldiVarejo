using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class ClienteObservacao_veterinarioDAO
    {
        public String Usuario { get; set; }
        public DateTime Data { get; set; }
        public String Nome_pet { get; set; }
        public String Codigo_cliente { get; set; }
        public String Observacao { get; set; }
        public String Filial { get; set; }
        public String Pedido { get; set; }

        internal void salvar( SqlConnection conn, SqlTransaction tran)
        {
            String sql = "insert into Observacao_veterinario (usuario,data,Nome_pet,codigo_cliente,Observacao,Filial, Pedido) values(";
            sql += "'" + Usuario + "'";
             sql +=","+ "'"+ Data.ToString("yyyy-MM-dd HH:mm")+"'" ;
             sql += ",'" + Nome_pet + "'";
             sql += ",'" + Codigo_cliente + "'";
             sql += ",'" + Observacao + "'";
             sql += ",'" + Filial + "'";
            sql += ",'" + Pedido+ "')";

            Conexao.executarSql(sql, conn, tran);
        }
    }
}