using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.dao
{
    public class PromocaoDAO
    {
        public int Codigo { get; set; }
        public int Tipo { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public string Descricao { get; set; }
        public decimal Param_Base { get; set; }
        public decimal Param_Brinde { get; set; }
        private User usr = null;
        public List<Promocao_itensDAO> itensBase = new List<Promocao_itensDAO>();
        public List<Promocao_itensDAO> itensBrinde = new List<Promocao_itensDAO>();

        public PromocaoDAO(User usr)
        {
            this.usr = usr;
        }
        public PromocaoDAO(int codigo, User usr)
        {
            this.usr = usr;
            this.Codigo = codigo;
            CarregarDados();
        }

        private void CarregarDados()
        {
            String sql = "Select * from Promocao where codigo =" + Codigo;
            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta(sql, null, false);
                if (rs.Read())
                {
                    Tipo = Funcoes.intTry(rs["Tipo"].ToString());
                    Inicio = Funcoes.dtTry(rs["Inicio"].ToString());
                    Fim = Funcoes.dtTry(rs["Fim"].ToString());
                    Descricao = rs["Descricao"].ToString();
                    Param_Base = Funcoes.decTry(rs["Param_Base"].ToString());
                    Param_Brinde = Funcoes.decTry(rs["Param_Brinde"].ToString());
                    itensBase = carregarItens("promocao_base");
                    itensBrinde = carregarItens("promocao_brinde");

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }
        }



        private List<Promocao_itensDAO> carregarItens(String tabela)
        {

            String sql = "Select pb.*,m.descricao from " + tabela + " as pb inner join mercadoria as m on pb.plu = m.plu where codigo_promo =" + Codigo;
            SqlDataReader rs = null;
            List<Promocao_itensDAO> itens = new List<Promocao_itensDAO>();
            try
            {
                rs = Conexao.consulta(sql, null, false);
                while (rs.Read())
                {
                    Promocao_itensDAO item = new Promocao_itensDAO()
                    {
                        Codigo_promo = Codigo,
                        Plu = Funcoes.intTry(rs["plu"].ToString()),
                        Descricao = rs["descricao"].ToString()
                    };

                    itens.Add(item);

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }

            return itens;
        }


        public bool Salvar(bool novo)
        {

            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {


                if (novo)
                {
                    insert(conn, tran);

                }
                else
                {
                    update(conn, tran);

                }

                salvarItens(conn, tran);

                tran.Commit();
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();

            }
            return true;
        }

        private void salvarItens(SqlConnection conn, SqlTransaction tran)
        {
            string sql = " delete from Promocao_base where codigo_promo =" + Codigo+"; ";
            sql += " delete from Promocao_brinde where codigo_promo =" + Codigo+ "; ";

            foreach (Promocao_itensDAO item in itensBase)
            {
                sql += " insert into promocao_base (codigo_promo, plu) " +
                    "values (" + Codigo + "," + item.Plu + "); ";
            }

            foreach (Promocao_itensDAO item in itensBrinde)
            {
                sql += " insert into promocao_brinde (codigo_promo, plu) " +
                    "values (" + Codigo + "," + item.Plu + "); ";

            }


            Conexao.executarSql(sql, conn, tran);



        }

        private void update(SqlConnection conn, SqlTransaction tran)
        {
            String sql = " update promocao set ";
            sql += "tipo=" + Tipo.ToString();
            sql += ",inicio='" + Inicio.ToString("yyyy-MM-dd")+"'";
            sql += ",fim ='" + Fim.ToString("yyyy-MM-dd") + "'";
            sql += ",descricao='" + Descricao + "'";
            sql += ",Param_base =" + Funcoes.decimalPonto(Param_Base.ToString());
            sql += ",Param_brinde =" + Funcoes.decimalPonto(Param_Brinde.ToString());
            sql += " where  codigo = " + Codigo + "; ";

            Conexao.executarSql(sql, conn, tran);

        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {

            String sql = " insert into promocao (";
            String values = ") values (";

           

            sql += "Tipo";
            values +=""+ Tipo;

            sql += ",Inicio";
            values +=",'"+ Inicio.ToString("yyyy-MM-dd")+"'";

            sql += ",Fim";
            values +=",'"+ Fim.ToString("yyyy-MM-dd")+"'";

            sql += ",Descricao";
            values +=",'"+ Descricao+"'";

            sql += ",Param_Base";
            values +=","+ Funcoes.decimalPonto(Param_Base.ToString());

            sql += ",Param_Brinde";
            values +=","+ Funcoes.decimalPonto(Param_Brinde.ToString());

            sql += values + "); " +
                " select IDENT_CURRENT( 'promocao' ) ";
            Codigo =Funcoes.intTry( Conexao.retornaUmValor(sql,null, conn, tran));


        }

        internal void validaItensPromocao()
        {
            String plus = "";
            String sql = "";

            foreach (Promocao_itensDAO item in itensBase)
            {
                if (plus.Length > 0)
                    plus += ",";

                plus += item.Plu;
            }

            foreach (Promocao_itensDAO item in itensBrinde)
            {
                if (plus.Length > 0)
                    plus += ",";

                plus += item.Plu;
            }

            if (plus.Length > 0)
            {


                String where = " where promocao.codigo <> " + Codigo + " and plu in (" + plus + ") and('" + Inicio.ToString("yyyy-MM-dd") + "' between promocao.Inicio and promocao.fim or ";
                where += " '" + Fim.ToString("yyyy-MM-dd") + "' between  promocao.Inicio and promocao.fim" +
                          " or ('" + Inicio.ToString("yyyy-MM-dd") + "' <= Promocao.Inicio and '" + Fim.ToString("yyyy-MM-dd") + "'>= promocao.fim )" +
              ")";

                sql = "Select plu, codigo_promo from Promocao_brinde as i ";
                sql += "inner join Promocao on i.Codigo_promo = Promocao.Codigo";
                sql += where;
                sql += " union all";
                sql += " Select plu, codigo_promo from Promocao_base as i";
                sql += " inner join Promocao on i.Codigo_promo = Promocao.Codigo";
                sql += where;
            }
            else
            {
                sql += "Select codigo  from promocao "+
                    " where tipo = 1 and codigo <> "+Codigo+"  and ('" + Inicio.ToString("yyyy-MM-dd") + "' between promocao.Inicio and promocao.fim or "+
                    " '" + Fim.ToString("yyyy-MM-dd") + "' between  promocao.Inicio and promocao.fim" +
                    " or ('" + Inicio.ToString("yyyy-MM-dd") + "' <= Promocao.Inicio and '" + Fim.ToString("yyyy-MM-dd") + "'>= promocao.fim ))" ;

            }
            String erros = "";
            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta(sql, null, false);
                while(rs.Read())
                {
                    if(plus.Length>0)
                        erros += " Plu:" + rs["plu"] + " Promocao n." + rs["codigo_promo"].ToString()+"<br>";
                    else
                        erros += " Promocao n." + rs["codigo"].ToString() + "<br>";
                }

                if (erros.Length > 0)
                    throw new Exception("Já existe uma promoção cadastrada no periodo selecionado  <br>" + erros);

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }
           
        }

        internal void excluir()
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {

                String sql = "Delete from promocao where codigo =" + Codigo+";";
                sql += "delete from promocao_base where codigo_promo = " + Codigo + ";";
                sql += "delete from promocao_brinde where codigo_promo = " + Codigo + ";";

                Conexao.executarSql(sql, conn, tran);

                
                tran.Commit();
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();

            }
          

        }
    }


}