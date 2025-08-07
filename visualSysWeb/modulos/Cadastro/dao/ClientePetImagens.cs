using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.dao
{
    public class ClientePetImagens
    {
        public String Imagem { get; set; }
        public String url { get; set; }
        public string Codigo_Cliente { get; internal set; }
        public string Codigo_pet { get; internal set; }

        internal void Salvar(SqlConnection conn, SqlTransaction tran)
        {
            String sql = "insert into cliente_pet_Imagem(" +
                                "codigo_cliente" +
                                ",codigo_pet" +
                                ",Imagem" +
                                ",urlstr" +
                            ") " +
                         "Values (" +
                                "'"+Codigo_Cliente+"'" +
                                ",'"+Codigo_pet+"'" +
                                ",'"+Imagem+"'" +
                                ",'"+url+"'" +
                            ")";
            Conexao.executarSql(sql,conn ,tran);
        }
    }
}