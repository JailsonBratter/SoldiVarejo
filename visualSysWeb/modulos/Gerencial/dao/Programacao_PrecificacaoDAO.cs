using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Gerencial.dao
{
    public class Programacao_PrecificacaoDAO
    {
        public int id { get; set; }
        public string filial { get; set; }
        public string descricao { get; set; }
        public DateTime data_cadastro { get; set; }
        public DateTime data_inicio { get; set; }
        public string Usuario_cadastro { get; set; }
        public List<Programacao_Precificacao_ItemDAO> Itens = new List<Programacao_Precificacao_ItemDAO>();
        internal int itensFim;
        internal int qtdePorPagina=20;
        internal int itensInicio;
        internal int qtdeItens;

        public Programacao_PrecificacaoDAO()
        {
                
        }
        public Programacao_PrecificacaoDAO(int id)
        {
            this.id = id;
            String sql = "Select * from Programacao_Precificacao where id=" + id.ToString();
            SqlDataReader rs = null;

            try
            {
                rs = Conexao.consulta(sql, null,false);
                if(rs.Read())
                {
                    filial = rs["filial"].ToString();
                    descricao = rs["descricao"].ToString();
                    data_cadastro = Funcoes.dtTry(rs["data_cadastro"].ToString());
                    data_inicio = Funcoes.dtTry(rs["data_inicio"].ToString());
                    Usuario_cadastro = rs["Usuario_cadastro"].ToString();

                }
                carregarItens();

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

        private void carregarItens()
        {
            String sql = "Select ppi.*,m.descricao from Programacao_Precificacao_itens as ppi " +
                " inner join mercadoria as m on m.plu= ppi.plu" +
                
                " where id_precificacao=" + id.ToString();
            SqlDataReader rs = null;

            try
            {
                rs = Conexao.consulta(sql, null, false);
                while (rs.Read())
                {
                    Programacao_Precificacao_ItemDAO item = new Programacao_Precificacao_ItemDAO();
                    item.ordem = Itens.Count + 1;
                    item.plu = rs["plu"].ToString();
                    item.descricao = rs["descricao"].ToString();
                    item.preco = Funcoes.decTry(rs["preco"].ToString());;
                    Itens.Add(item);
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
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                if (novo)
                    insert(conn,tran);
                else
                    update(conn,tran);

                String sql = "delete from Programacao_precificacao_itens where id_precificacao =" + id+"; ";
                foreach (Programacao_Precificacao_ItemDAO item in Itens)
                {
                    sql += "insert into Programacao_precificacao_itens (id_precificacao,plu,preco) values(" +
                        id +
                        ",'" + item.plu + "'" +
                        "," +Funcoes.decimalPonto(item.preco.ToString())+
                        ");";
                }

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

        private void update(SqlConnection conn, SqlTransaction tran)
        {
            String sql = " update Programacao_Precificacao set  ";

            sql += " filial ='" + filial + "'";
            sql += " ,descricao='" + descricao + "'";
            sql += " ,data_inicio =" + Funcoes.dateSql(data_inicio);
            sql += " ,usuario_cadastro='" + Usuario_cadastro + "'";

            sql += " where id =" + id;

            Conexao.executarSql(sql, conn, tran);
        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            id = Funcoes.intTry(Funcoes.sequencia("PROG_PRECIFICACAO.ID", null,conn,tran));
            Funcoes.salvaProximaSequencia("PROG_PRECIFICACAO.ID", null, conn, tran);
            String sql = "insert into Programacao_Precificacao (";
            String values = " )values( ";

            sql += "id";
            values += id;

            sql += ",filial";
            values += ",'" + filial + "'";

            sql += ",descricao";
            values += ",'" + descricao + "'";

            sql += ",data_cadastro";
            values += "," + Funcoes.dateSql(data_cadastro) ;

            sql += ",data_inicio";
            values += "," + Funcoes.dateSql(data_inicio) ;

            sql += ",usuario_cadastro";
            values += ",'" + Usuario_cadastro+"'" ;


            sql += values + ");";
            Conexao.executarSql(sql, conn, tran);

        }

        internal void addItens(Programacao_Precificacao_ItemDAO item)
        {
           if(!Itens.Exists(a=> a.plu.Equals(item.plu))){
                item.id_precificacao = this.id;
                item.ordem = Itens.Count + 1;
                Itens.Add(item);
            }
            else
            {
                Itens.Find(x => x.plu.Equals(item.plu)).preco = item.preco;
            }
        }

        internal bool excluirItem(string plu)
        {
            Itens.RemoveAll(x => x.plu.Equals(plu));
            return true;
        }

        internal void excluir(User usr)
        {
            String sql = "update Programacao_Precificacao set excluido = 1 ,usuario_excluiu ='"+usr.getUsuario()+ "' ,data_excluiu= getdate() where id = " + id;
            Conexao.executarSql(sql);
        }
    }
}