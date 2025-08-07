using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Sped.code
{
    public class PlanoContasFiltros
    {
        public String dtDe="";
        public String dtAte="";
        public String cod_conta="";
        public String descricao = "";
        public String cnpj_Estabelecimento = "";
        public String tipo = "";
        public String natureza="";
        public String entrada="";
        public int qtdFiltros = 0;
        public int qtdeCadastro = 0;

        public List<ContaContabilDAO> pesquisa( User usr)
        {
            String sql = " Select * from Conta_Contabil ";
            String where = "";
            if (cod_conta.Trim().Length > 0)
            {
                where += " cod_conta = '" + cod_conta + "'";
            }
            if (descricao.Trim().Length > 0)
            {

                if (where.Length > 0)
                    where += " and ";

                where += " descricao like '%" + descricao + "%'";
            }
            if (cnpj_Estabelecimento.Trim().Length > 0)
            {

                if (where.Length > 0)
                    where += " and ";

                where += " replace(replace(replace(cnpj_Estabelecimento,'.',''),'/',''),'-','') like '%" + cnpj_Estabelecimento.Replace(".","").Replace("/","").Replace("-","") + "%'";
            }

            if (tipo.Trim().Length > 0)
            {
                if (where.Length > 0)
                    where += " and ";

                where += " tipo = '" + tipo+"'";
            }

            if (natureza.Trim().Length > 0)
            {
                if (where.Length > 0)
                    where += " and ";

                where += " natureza = '" + natureza + "'";
            }
            if (entrada.Trim().Length > 0)
            {
                if (where.Length > 0)
                    where += " and ";

                where += "isnull(entrada,0)=" + entrada;
            }

            if (dtDe.Trim().Length > 0 || dtAte.Trim().Length > 0)
            {
                DateTime de = Funcoes.dtTry(dtDe);
                DateTime ate = Funcoes.dtTry(dtAte);

                if (where.Length > 0)
                    where += " and ";

                where += "(data ";
                if (dtDe.Trim().Length > 0 && dtAte.Trim().Length > 0)
                {
                    where += " between '" + de.ToString("yyyy-MM-dd") + "' and '" + ate.ToString("yyyy-MM-dd") + "'";
                }
                else if (dtDe.Trim().Length > 0)
                {
                    where += " >= '" + de.ToString("yyyy-MM-dd") + "'";
                }
                else
                {
                    where += " <= '" + ate.ToString("yyyy-MM-dd") + "'";
                }
                where += " )";
            }

            if (where.Length > 0)
            {
                sql += " where " + where;
            }


            SqlDataReader rs = null;
            List<ContaContabilDAO> list = new List<ContaContabilDAO>();
            try
            {
                rs = Conexao.consulta(sql, usr, false);
                while (rs.Read())
                {
                    ContaContabilDAO plano = new ContaContabilDAO(usr);

                    plano.data = Funcoes.dtTry(rs["data"].ToString());
                    plano.cod_conta = rs["cod_conta"].ToString();
                    plano.descricao = rs["descricao"].ToString();
                    plano.tipo = rs["tipo"].ToString();
                    plano.natureza = rs["natureza"].ToString();
                    plano.nivel = rs["nivel"].ToString();
                    plano.conta_relacionada = rs["conta_relacionada"].ToString();
                    plano.cnpj_estabelecimento = rs["cnpj_estabelecimento"].ToString();
                    plano.entrada = rs["entrada"].ToString().Equals("1");
                    list.Add(plano);
                }

            }
            catch (Exception err)
            {

                throw err;
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }

            qtdFiltros = list.Count;
            qtdeCadastro = Funcoes.intTry(Conexao.retornaUmValor("Select Count(*) from Conta_Contabil", usr));
            if (list.Count == 0)
            {
                ContaContabilDAO pl  = new ContaContabilDAO(usr);
                pl.cod_conta = "---";
                pl.descricao = "---";
                pl.tipo = "---";
                pl.natureza = "---";
                pl.nivel = "---";
                list.Add(pl);
            }
                

            return list;

        }
    }
}