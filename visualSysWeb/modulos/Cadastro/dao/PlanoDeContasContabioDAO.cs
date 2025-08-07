using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.dao
{
    public class PlanoDeContasContabilDAO
    {
        public String id { get; set; }
        public String codigoPlanoPai {get;set;}
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
            String sql = "update Plano_de_contas_contabil set descricao='" + descricao + "'" +
                " where id='"+id+"'";
            Conexao.executarSql(sql);
        }

        public void delete()
        {
            int count = Funcoes.intTry(Conexao.retornaUmValor("Select count(*) from Plano_de_contas_contabil " +
                "where codigo_plano_pai ='" + codigo + "'", null));

            if (count > 0)
                throw new Exception("Não é Possivel Excluir, Plano com Sub Planos cadastrados! ");

            String sql = "delete from Plano_de_contas_contabil " +
                " where id='" + id + "'"; ;
            Conexao.executarSql(sql);
        }

        private void insert()
        {
            String sql = "insert into Plano_de_contas_contabil (id,codigo_plano_pai , codigo, descricao) " +
                         " values(" +
                         "'" +id+"'"+
                         ",'"+codigoPlanoPai+"'" +
                         ",'"+codigo+"'" +
                         ",'"+descricao+"')";
            Conexao.executarSql(sql);
        }
    }
}