using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.NotaFiscal.dao
{
    public class nf_justificativa_edicaoDAO
    {
        public string filial { get; set; } = "";
        public string usuario { get; set; } = "";
        public string codigo_nota { get; set; } = "";
        public string cliente_fornecedor { get; set; } = "";
        public int serie { get; set; } = 0;
        public int tipo_nf { get; set; } = 0;
        public DateTime data_alteracao { get; set; }
        public string justificativa { get; set; } = "";


        public void salvar()
        {
            String sql = "insert into nf_justificativa_edicao (";
            String values = " )values(";


            sql += "filial";
            values += "'" + filial + "'";

            sql += ",usuario";
            values += ",'" + usuario + "'";

            sql += ",codigo_nota";
            values += ",'" + codigo_nota + "'";
            
            sql += ",tipo_nf";
            values += ","+tipo_nf;

            sql += ",data_alteracao";
            values += ",'" + data_alteracao.ToString("yyyy-MM-dd HH:mm:ss")+"'";

            sql += ",justificativa";
            values += ",'" + justificativa+"'";

            sql += ",serie";
            values += ",'" + serie+"'";

            sql += ",cliente_fornecedor";
            values += ",'" + cliente_fornecedor + "'";

            sql += values + ")";
            Conexao.executarSql(sql);

        }
    }
}