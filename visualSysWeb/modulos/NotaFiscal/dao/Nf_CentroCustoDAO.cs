using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.NotaFiscal.dao
{
    public class Nf_CentroCustoDAO
    {
        public string Filial { get; set; }
        public string Codigo { get; set; }
        public string Cliente_fornecedor { get; set; }
        public int Tipo_nf { get; set; }
        public int serie { get; set; }
        public DateTime Data { get; set; }
        public string Codigo_centro_custo { get; set; }
        public string Descricao_centro_custo { get; set; }
        public decimal Valor { get; set; }
        public decimal porc { get; set; } = 0;


        public void Salvar(SqlConnection conn, SqlTransaction tran)
        {
            string col = "filial,";
            string val = "'"+Filial+"',";
            
            col += "codigo,";
            val += "'"+Codigo+"',";
            
            col += "cliente_fornecedor,";
            val += "'"+Cliente_fornecedor+"',";
            
            col += "tipo_nf,";
            val += Tipo_nf+",";
            
            col += "serie,";
            val += serie+",";

            col += "data,";
            val += Funcoes.dateSql(Data)+",";
            
            col += "codigo_centro_custo,";
            val += "'"+Codigo_centro_custo+"',";
            
            col += "valor,";
            val += Funcoes.decimalPonto(Valor)+",";

            col += "porc";
            val += Funcoes.decimalPonto(porc);


            Conexao.executarSql("insert into NF_CentroCusto (" + col + ") values(" + val+");", conn, tran);
        }
    }
}