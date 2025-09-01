using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class ListaProdutoPadraoDAO
    {
        public int id { get; set; }
        public string descricao { get; set; }
        public string tipo { get; set; }
        public List<ListaProdutoPadraoItensDAO> itens = new List<ListaProdutoPadraoItensDAO>();
        internal int itensFim;
        internal int qtdePorPagina;
        internal int itensInicio;
        internal int qtdeItens;

        public ListaProdutoPadraoDAO()
        {
            qtdePorPagina = Funcoes.intTry(Funcoes.valorParametro("ITENS_POR_PAG", null));
            if (qtdePorPagina == 0)
                qtdePorPagina = 100;

            itensFim = qtdePorPagina;

        }

        public ListaProdutoPadraoDAO(string tipo)
        {
            this.tipo = tipo;
            qtdePorPagina = Funcoes.intTry(Funcoes.valorParametro("ITENS_POR_PAG", null));
            if (qtdePorPagina == 0)
                qtdePorPagina = 100;

            itensFim = qtdePorPagina;
        }
        public ListaProdutoPadraoDAO(int id) //: this(tipo)
        {
            this.id = id;
            qtdePorPagina = Funcoes.intTry(Funcoes.valorParametro("ITENS_POR_PAG", null));
            if (qtdePorPagina == 0)
                qtdePorPagina = 100;

            itensFim = qtdePorPagina;


            SqlDataReader rs = null;
            SqlDataReader rsItens = null;

            try
            {
                rs = Conexao.consulta("Select * from LISTA_PADRAO WHERE ID =" + id, null, false); // +" and tipo ='"+tipo+"'", null, false);
                if (rs.Read())
                {
                    descricao = rs["descricao"].ToString();
                    tipo = rs["Tipo"].ToString();
                }

                rsItens = Conexao.consulta("Select lt.plu ,m.descricao,Ordem " +
                                            " from LISTA_PADRAO_ITENS as lt " +
                                            " inner join mercadoria as m on lt.plu = m.plu" +
                                            " WHERE lt.ID_LISTA = " + id +" order by ordem " , null, false);
                itens.Clear();
                int i = 1;
                while (rsItens.Read())
                {
                    ListaProdutoPadraoItensDAO item = new ListaProdutoPadraoItensDAO();
                    item.ordem = i;
                    item.PLU = rsItens["plu"].ToString();
                    item.descricao = rsItens["descricao"].ToString();
                    itens.Add(item);
                    i++;
                }
                qtdeItens = itens.Count;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rs != null)
                    rs.Close();
                if (rsItens != null)
                    rsItens.Close();
            }

        }
        public void salvar(bool incluir)
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                if (incluir)
                    insert(conn, tran);
                else
                    update(conn, tran);


                atualizarItens(conn, tran);
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

        private void atualizarItens(SqlConnection conn, SqlTransaction tran)
        {
            Conexao.executarSql("DELETE FROM LISTA_PADRAO_ITENS WHERE ID_LISTA=" + id, conn, tran);

            String sql = "";

            foreach (ListaProdutoPadraoItensDAO item in itens)
            {
                sql += "insert into LISTA_PADRAO_ITENS  VALUES (" + id + ",'" + item.PLU + "',"+item.ordem+");";
            }
            Conexao.executarSql(sql, conn, tran);

        }

        private void update(SqlConnection conn, SqlTransaction tran)
        {
            string sql = " UPDATE LISTA_PADRAO SET DESCRICAO ='" + descricao + "'" +
                " WHERE ID =" + id;
            Conexao.executarSql(sql, conn, tran);
        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {

            string sql = "Insert into LISTA_PADRAO VALUES ('" + descricao + "','"+tipo+"');" +
                        " select  ID= IDENT_CURRENT( 'LISTA_PADRAO' ) ";
            id = Funcoes.intTry(Conexao.retornaUmValor(sql, null, conn, tran));

        }

        internal void addItens(ListaProdutoPadraoItensDAO nitem)
        {
            if (itens.Count(i => i.PLU.Equals(nitem.PLU)) == 0)
            {
                
                nitem.ordem = itens.Count + 1;
                itens.Add(nitem);
                qtdeItens = itens.Count;
            }
        }

        public List<ListaProdutoPadraoItensDAO> GridItens()
        {
            List<ListaProdutoPadraoItensDAO> resFin = new List<ListaProdutoPadraoItensDAO>();
            for (int i = itensInicio; i < itensFim && (i < itens.Count); i++)
            {
                resFin.Add(itens[i]);
            }
            return resFin;

        }

        internal bool excluirItem(string plu)
        {
            itens.RemoveAll(i => i.PLU.Equals(plu));
            corrigirOrdem();
            return true;
        }

        private void corrigirOrdem()
        {
            int i = 1;
            foreach (ListaProdutoPadraoItensDAO item in itens)
            {
                item.ordem = i;
                i++;
            }
        }

        internal void excluir()
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                Conexao.executarSql("DELETE FROM LISTA_PADRAO_ITENS WHERE ID_LISTA=" + id, conn, tran);
                Conexao.executarSql("DELETE FROM LISTA_PADRAO WHERE ID=" + id, conn, tran);
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