using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class TabelaPrecoDAO
    {
        public string codigo_tabela { get; set; }
        public string filial { get; set; }
        public decimal nro_tabela  { get; set; }
        public decimal porc { get; set; }
        public decimal porcPositivo { get
            {
                if (porc < 0)
                    return (porc * -1);
                else
                    return porc;
            } }
        private User usr = null;
        public bool Acrescimo
        {
            get
            {
                if (porc < 0)
                {
                    return true;
                }
                else
                    return false;
            }
        }
        public List<preco_mercadoriaDAO> produtos = new List<preco_mercadoriaDAO>();

        public TabelaPrecoDAO()
        {
        }
        public TabelaPrecoDAO(string codigo_tabela , User usr)
        {
            this.usr = usr;
            this.filial = usr.getFilial();
            this.codigo_tabela = codigo_tabela;
            carregarDados();
            carregarProdutos();
        }
        public TabelaPrecoDAO(User usr)
        {
            this.usr = usr;
            this.filial = usr.getFilial();
        }
        public void carregarDados()
        {
            string sql = "Select * " +
                "   from Tabela_preco  " +
                " where filial = '" + this.usr.getFilial() + "' " +
                "   and codigo_tabela ='" + this.codigo_tabela + "'";
            SqlDataReader rs = null;
            produtos.Clear();
            try
            {
                rs = Conexao.consulta(sql, usr, false);
                if (rs.Read())
                {
                    nro_tabela = Funcoes.decTry(rs["nro_tabela"].ToString());
                    porc = Funcoes.decTry(rs["porc"].ToString());
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
        public void carregarProdutos()
        {
            string sql = "Select pm.*,m.descricao,m.preco_custo " +
                "   from Preco_Mercadoria  as pm " +
                "   inner join mercadoria as m on pm.plu = m.plu " +
                " where pm.filial = '"+this.usr.getFilial()+"' " +
                "   and pm.codigo_tabela ='"+this.codigo_tabela+"'";
            SqlDataReader rs = null;
            produtos.Clear();
            try
            {
                rs = Conexao.consulta(sql, usr, false);
                while (rs.Read())
                {
                    preco_mercadoriaDAO pm = new preco_mercadoriaDAO()
                    {
                        Filial = rs["filial"].ToString(),
                        Codigo_tabela = this.codigo_tabela,
                        PLU = rs["plu"].ToString(),
                        Descricao = rs["descricao"].ToString(),
                        Preco = Funcoes.decTry(rs["preco"].ToString()),
                        Desconto = Funcoes.decTry(rs["desconto"].ToString()),
                        Preco_promocao = Funcoes.decTry(rs["preco_promocao"].ToString()),
                        tipo_arredondamento = Funcoes.intTry(rs["tipo_arredondamento"].ToString()),
                        PrecoCusto = Funcoes.decTry(rs["preco_custo"].ToString())
                        
                    };
                    produtos.Add(pm);
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

        public void salvar(bool novo)
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                if (novo)
                { 
                    insert(conn,tran);
                }
                else
                {
                    update(conn,tran);
                }

                Conexao.executarSql("delete from Preco_Mercadoria " +
                                    " where filial='" + this.filial + "' " +
                                    "   and codigo_tabela ='" + this.codigo_tabela + "'", conn, tran);
                foreach(preco_mercadoriaDAO prod in produtos)
                {
                    prod.Codigo_tabela = this.codigo_tabela;
                    prod.Filial = this.filial;
                    prod.salvar(true, conn, tran);
                }
                tran.Commit();
            }
            catch (Exception err)
            {
                tran.Rollback();
                throw err;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        public void insert(SqlConnection conn ,SqlTransaction tran)
        {
            string col = "codigo_tabela";
            string val = "'"+this.codigo_tabela+"'";

            col += ",filial";
            val += ",'" + this.filial + "'";

            col += ",nro_tabela";
            val += "," + Funcoes.decimalPonto(this.nro_tabela.ToString());

            col += ",porc";
            val += "," + Funcoes.decimalPonto(this.porc.ToString());

            Conexao.executarSql("insert into tabela_preco ("+col+") values ("+val+");",conn,tran);
        }

        public void update(SqlConnection conn , SqlTransaction tran) 
        {
            string cols = "nro_tabela = " + Funcoes.decimalPonto(this.nro_tabela);
            cols += ",porc = " + Funcoes.decimalPonto(this.porc);

            Conexao.executarSql("update tabela_preco set "+cols + 
                " where codigo_tabela = '" + this.codigo_tabela + "' and filial ='" + this.filial + "'", conn,tran);
        }

        public void excluir()
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
              
                Conexao.executarSql("delete from Preco_Mercadoria " +
                                    " where filial='" + this.filial + "' " +
                                    "   and codigo_tabela ='" + this.codigo_tabela + "'", conn, tran);

                Conexao.executarSql("delete from tabela_preco " +
                                   " where filial='" + this.filial + "' " +
                                   "   and codigo_tabela ='" + this.codigo_tabela + "'", conn, tran);
                tran.Commit();
            }
            catch (Exception err)
            {
                tran.Rollback();
                throw err;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }
    }
}