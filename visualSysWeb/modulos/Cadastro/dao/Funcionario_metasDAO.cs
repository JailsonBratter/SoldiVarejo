using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Cadastro.dao
{
    public class Funcionario_metasDAO
    {
        public string Filial { get; set; }
        public string Codigo_funcionario { get; set; }
        public string Codigo_departamento { get; set; }
        public string Descricao_departamento { get; set; }
        public decimal Meta { get; set; }
        public String sql()
        {
            String strSql = "insert into Funcionario_metas (";
            String values = ") Values(";

            strSql += "Filial";
            values += "'"+Filial+"'";
            strSql += ",Codigo_funcionario";
            values += ",'"+Codigo_funcionario+"'";
            strSql += ",Codigo_departamento";
            values += ",'"+Codigo_departamento+"'";
            strSql += ",Meta";
            values += ","+Funcoes.decimalPonto(Meta.ToString());
            
            strSql += values + ");";
            return strSql;
        }
    }
}