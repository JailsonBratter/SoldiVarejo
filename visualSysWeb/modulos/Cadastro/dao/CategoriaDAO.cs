using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.dao
{
    public class CategoriaDAO
    {
        public String codigo_departamento {get;set;}
        public String codigo { get; set; }
        public String descricao { get; set; }


        public void salvar(bool novo)
        {
            if (novo)
                insert();
            else
                update();
        }

        private void update()
        {
            String sql = "update categorias set descricao='" + descricao + "'" +
                " where codigo ='" + codigo + "'" +
                " and codigo_departamento = '" + codigo_departamento + "'";
            Conexao.executarSql(sql);
        }

        public void delete()
        {
            int count = Funcoes.intTry(Conexao.retornaUmValor("Select count(*) from categorias " +
                "where codigo_departamento ='" + codigo + "'", null));

            if (count > 0)
                throw new Exception("Não é Possivel Excluir, Categoria com Sub Categoria cadastradas! ");

            String sql = "delete from categorias "+
                " where codigo ='" + codigo + "'" +
                " and codigo_departamento = '" + codigo_departamento + "'";
            Conexao.executarSql(sql);
        }

        private void insert()
        {
            String sql = "insert into categorias (codigo_departamento , codigo, descricao) " +
                         " values('"+codigo_departamento+"'" +
                         ",'"+codigo+"'" +
                         ",'"+descricao+"')";
            Conexao.executarSql(sql);
        }
    }
}