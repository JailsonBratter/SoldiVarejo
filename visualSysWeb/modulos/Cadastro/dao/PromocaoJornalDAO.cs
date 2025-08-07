using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.dao
{
    public class PromocaoJornalDAO
    {
        public string  Filial { get; set; }
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public List<PromocaoJornalItensDAO> Itens = new List<PromocaoJornalItensDAO>();
        private User usr { get; set; }
        public int gridInicio { get; internal set; }
        public int gridFim { get; internal set; }
        public string status { get; set; }

        public PromocaoJornalDAO(User usr) {
            this.usr = usr;
        }
        public PromocaoJornalDAO(int codigo, User usr)
        {
            this.usr = usr;
            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta("Select * from promocaoJornal where codigo =" + codigo.ToString(), usr,false);
                if (rs.Read())
                {
                    Filial = usr.getFilial();
                    Codigo = codigo;
                    Descricao = rs["Descricao"].ToString();
                    Inicio = Funcoes.dtTry(rs["inicio"].ToString());
                    Fim = Funcoes.dtTry(rs["fim"].ToString());
                    status = rs["Status"].ToString();
                    carregarItens();
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

        private void carregarItens()
        {
            String sql = "Select * from promocaoJornalItens where codigo_jornal = "+this.Codigo;
            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta(sql, this.usr,false);
                while (rs.Read())
                {
                    PromocaoJornalItensDAO item = new PromocaoJornalItensDAO(this.usr)
                    {
                        Filial = rs["Filial"].ToString(),
                        Ordem = Funcoes.intTry(rs["Ordem"].ToString()),
                        CodigoJornal = this.Codigo,
                        Plu = rs["PLU"].ToString(),
                        Descricao = rs["Descricao"].ToString(),
                        Preco = Funcoes.decTry(rs["Preco"].ToString()),
                        PrecoPromocao = Funcoes.decTry(rs["preco_promocao"].ToString())

                    };
                    addItens(item);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void salvar(bool novo)
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                if (novo)
                    insert(conn,tran);
                else
                    update(conn,tran);

                Conexao.executarSql("delete from promocaoJornalItens where codigo_jornal =" + this.Codigo + " and filial ='" + this.Filial + "'", conn, tran);
                foreach (PromocaoJornalItensDAO item in this.Itens)
                {
                    item.CodigoJornal = this.Codigo;
                    item.Filial = this.Filial;
                    item.salvar(true, conn, tran);
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


        private void update(SqlConnection conn, SqlTransaction tran)
        {
            string sql = "update promocaoJornal set ";
            sql += "descricao ='" + this.Descricao + "',";
            sql += "inicio=" + Funcoes.dateSql(this.Inicio)+","; 
            sql += "fim=" + Funcoes.dateSql(this.Fim) + ",";
            sql += "status= '" + this.status + "'"; 
            sql += " where codigo ='"+this.Codigo+"' and filial ='"+this.Filial+"'";
            Conexao.executarSql(sql,conn, tran);

            if (this.Inicio <= DateTime.Now.Date && this.status.Equals("ATIVO"))
            {
                //Altera mercadoria_loja
                sql = "UPDATE mercadoria_loja SET";
                sql += " mercadoria_loja.preco_promocao = promocaoJornalItens.preco_promocao";
                sql += ", mercadoria_loja.data_inicio = promocaoJornal.inicio";
                sql += ", mercadoria_loja.data_fim = promocaoJornal.fim";
                sql += ", mercadoria_loja.Promocao = 1";
                sql += ", mercadoria_loja.data_alteracao = GETDATE()";
                sql += " FROM promocaoJornal INNER JOIN promocaoJornalItens ON promocaoJornal.codigo = promocaoJornalItens.codigo_jornal";
                sql += " WHERE promocaoJornal.filial = '" + this.Filial + "'";
                sql += " AND promocaoJornal.codigo = " + this.Codigo;
                sql += " AND mercadoria_loja.Filial = promocaoJornal.Filial";
                sql += " AND mercadoria_loja.PLU = promocaoJornalItens.plu";
                Conexao.executarSql(sql, conn, tran);

                //Altera Estado da mercadoria
                sql = "UPDATE mercadoria SET";
                sql += " mercadoria.preco_promocao = promocaoJornalItens.preco_promocao";
                sql += ", mercadoria.data_inicio = promocaoJornal.inicio";
                sql += ", mercadoria.data_fim = promocaoJornal.fim";
                sql += ", mercadoria.Promocao = 1";
                sql += ", mercadoria.data_alteracao = GETDATE()";
                sql += ", mercadoria.estado_mercadoria = 1";
                sql += " FROM promocaoJornal INNER JOIN promocaoJornalItens ON promocaoJornal.codigo = promocaoJornalItens.codigo_jornal";
                sql += " WHERE promocaoJornal.filial = '" + this.Filial + "'";
                sql += " AND promocaoJornal.codigo = " + this.Codigo;
                sql += " AND mercadoria.PLU = promocaoJornalItens.plu";

                Conexao.executarSql(sql, conn, tran);

            }

        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            this.Codigo =Funcoes.intTry( Funcoes.sequencia("PROMOCAO_JORNAL", this.usr));
            Funcoes.salvaProximaSequencia("PROMOCAO_JORNAL", this.usr);
            string col = "codigo, ";
            string val = this.Codigo + ",";
            
            col += "filial,";
            val += "'" + this.Filial+ "',";
            
            col += "descricao,";
            val += "'" + Descricao + "',";
            
            col += "inicio,";
            val +=  Funcoes.dateSql(this.Inicio)+ ",";

            col += "fim,";
            val += Funcoes.dateSql(this.Fim)  + "," ;
            col += "status";
            val += "'" + status + "'";


            Conexao.executarSql("insert into promocaoJornal (" + col + ") values (" + val + ")",conn,tran);
        }

        internal void excluir()
        {
            throw new NotImplementedException();
        }

        internal void addItens(PromocaoJornalItensDAO item)
        {
            item.CodigoJornal = this.Codigo;
            item.Ordem = Itens.Count + 1;
            Itens.Add(item);
        }

    }
}