using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace visualSysWeb.dao
{
    public class cor_linhaDAO
    {
        public int codigo_cor { get; set; }
        public Decimal codigo_linha { get; set; }
        public String descricao_cor { get; set; }
        public cor_linhaDAO() { }
        public cor_linhaDAO(String codigoCor, String codigoLinha, User usr)
        { //colocar campo index da tabela
            String sql = "Select * from  cor_linha where codigo_cor =" + codigoCor + " and codigo_linha=" + codigoLinha;
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            carregarDados(rs);
        }
        public ArrayList ArrToString()
        {
            ArrayList cor = new ArrayList();
            cor.Add(codigo_cor.ToString());
            cor.Add(codigo_linha.ToString());
            cor.Add(descricao_cor);

            return cor;
        }
        private String dataBr(DateTime dt)
        {
            if (dt.ToString("dd/MM/yyyy").Equals("01/01/0001"))
            {
                return "";
            }
            else
            {
                return dt.ToString("dd/MM/yyyy");
            }
        }

        public static ArrayList arrayCoresLinha(int codLinha)
        {
            ArrayList coresLinha = new ArrayList();
            String sql = "Select codigo_cor from cor_linha where codigo_linha=" + codLinha;

            SqlDataReader rs = Conexao.consulta(sql, new User(), true);

            while (rs.Read())
            {
                cor_linhaDAO cor = new cor_linhaDAO(rs["codigo_cor"].ToString(), codLinha.ToString(), new User());
                coresLinha.Add(cor);
            }

            if (rs != null)
                rs.Close();
            return coresLinha;
        }
        public void carregarDados(SqlDataReader rs)
        {
            if (rs.Read())
            {
                codigo_cor = (rs["codigo_cor"] == null ? 0 : int.Parse(rs["codigo_cor"].ToString()));
                codigo_linha = (Decimal)(rs["codigo_linha"].ToString().Equals("") ? new Decimal() : rs["codigo_linha"]);
                descricao_cor = rs["descricao_cor"].ToString();
            }

            if (rs != null)
                rs.Close();
        }
        private void update(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = "update  cor_linha set " +
                              "codigo_linha=" + codigo_linha.ToString().Replace(",", ".") +
                              ",descricao_cor='" + descricao_cor + "'" +
                    "  where codigo_cor=" + codigo_cor + " and codigo_linha = " + codigo_linha
                        ;
                Conexao.executarSql(sql, conn, tran);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }
        public bool salvar(bool novo, SqlConnection conn, SqlTransaction tran)
        {
            if (novo)
            {
                insert(conn, tran);
            }
            else
            {
                update(conn, tran);
            }
            return true;
        }

        public bool excluir(SqlConnection conn, SqlTransaction tran)
        {
            String sql = "delete from cor_linha  where codigo_cor= " + codigo_cor + " and codigo_linha = " + codigo_linha; //colocar campo index
            Conexao.executarSql(sql, conn, tran);
            return true;
        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = " insert into cor_linha (" +
                          "codigo_cor," +
                          "codigo_linha," +
                          "descricao_cor" +
                     " )values( " +
                          codigo_cor +
                          "," + codigo_linha.ToString().Replace(",", ".") +
                          "," + "'" + descricao_cor + "'" +
                         ");";

                Conexao.executarSql(sql, conn, tran);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }
    }
}