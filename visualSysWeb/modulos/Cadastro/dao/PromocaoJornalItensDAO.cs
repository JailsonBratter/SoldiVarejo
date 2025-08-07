using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.dao
{
    public class PromocaoJornalItensDAO
    {
        public string Filial { get; set; }
        public int Ordem { get; set; }
        public int CodigoJornal { get; set; }
        public string Plu { get; set; }
        public string EAN { get; set; }
        public string Referencia { get; set; }
        public string Descricao { get; set; }
        public Decimal Preco { get; set; }
        public Decimal PrecoPromocao { get; set; }
        private User usr = null;
        public PromocaoJornalItensDAO(User usr)
        {
            this.usr = usr;
        }

        internal void salvar(bool novo, SqlConnection conn, SqlTransaction tran)
        {
            if (novo)
                insert(conn, tran);
            else
                update(conn, tran);
        }

        private void update(SqlConnection conn, SqlTransaction tran)
        {
            string sql = "update promocaoJornalItens set ";
            sql += "descricao ='" + this.Descricao + "',";
            sql += "preco=" + Funcoes.decimalPonto(this.Preco)+",";
            sql += "preco_promocao=" + Funcoes.decimalPonto(this.PrecoPromocao)+",";
            sql += "ordem = " + this.Ordem.ToString();
            sql += " where codigo_jornal ='" + this.CodigoJornal + "' and filial ='" + this.Filial + "' and plu ='"+this.Plu+"'";
            Conexao.executarSql(sql, conn, tran);

        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            string col = "codigo_jornal, ";
            string val = this.CodigoJornal + ",";
            col += "filial,";
            val += "'" + this.Filial + "',";
            
            col += "descricao,";
            val += "'" + Descricao + "',";
            
            col += "plu,";
            val += "'"+this.Plu + "',";
            
            col += "ordem,";
            val += this.Ordem.ToString()+",";
            
            col += "preco,";
            val += Funcoes.decimalPonto(this.Preco)+",";
            
            col += "preco_promocao";
            val += Funcoes.decimalPonto(this.PrecoPromocao);

            Conexao.executarSql("insert into promocaoJornalItens (" + col + ") values (" + val + ")", conn, tran);
        }

    }
}