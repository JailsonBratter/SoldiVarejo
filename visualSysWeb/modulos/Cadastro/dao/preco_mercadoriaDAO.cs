using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class preco_mercadoriaDAO
    {
        public String Filial { get; set; }
        public String Codigo_tabela { get; set; }
        public String PLU { get; set; }
        public String Descricao { get; set; }

        public Decimal Preco { get; set; }
        public Decimal PrecoCusto { get; set; } = 0;
        public Decimal Desconto { get; set; }
        public int tipo_arredondamento { get; set; } = 1;
        public Decimal PositivoDesconto
        {
            get
            {
                if (Desconto < 0)
                    return (Desconto * -1);
                else
                    return Desconto;
            }
        }
        public bool Acrescimo
        {
            get
            {
                if (Desconto < 0)
                {
                    return true;
                }
                else
                    return false;
            }
        }
        public Decimal Margem
        {
            get
            {

                decimal vlrMargem = 0;
                if(PrecoCusto>0 && Preco_promocao > 0)
                {
                    vlrMargem = ((Preco_promocao-PrecoCusto ) / PrecoCusto) * 100;
                }
                return Decimal.Round(vlrMargem,4);
            }
        }
        public Decimal Preco_promocao { get; set; }
        public Decimal Desconto_promocao { get; set; }
        public preco_mercadoriaDAO() { }
        public preco_mercadoriaDAO(String codTabela, String plu, User usr)
        { //colocar campo index da tabela
            String sql = "Select pm.*,m.descricao " +
                         "from  preco_mercadoria as pm " +
                         " inner join mercadoria as m on pm.plu =m.plu " +
                         "where pm.codigo_tabela ='" + codTabela + "' " +
                         "  and pm.plu ='" + plu + "' " +
                         "  and pm.filial ='" + usr.getFilial() + "'";
            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta(sql, usr, true);
                carregarDados(rs);
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
        public ArrayList ArrToString()
        {
            ArrayList item = new ArrayList();
            item.Add(Filial);
            item.Add(Codigo_tabela);
            item.Add(PLU);
            item.Add(Preco.ToString("N2"));
            item.Add(Desconto.ToString());
            item.Add(Preco_promocao.ToString("N2"));
            item.Add(Desconto_promocao.ToString("N2"));

            return item;
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
        public void carregarDados(SqlDataReader rs)
        {
            if (rs.Read())
            {
                Filial = rs["Filial"].ToString();
                Codigo_tabela = rs["Codigo_tabela"].ToString();
                PLU = rs["PLU"].ToString();
                Descricao = rs["Descricao"].ToString();
                Preco = Funcoes.decTry(rs["Preco"].ToString());
                Desconto = Funcoes.decTry(rs["Desconto"].ToString());
                Preco_promocao = Funcoes.decTry(rs["Preco_promocao"].ToString());
                Desconto_promocao = Funcoes.decTry(rs["Desconto_promocao"].ToString());
                tipo_arredondamento = Funcoes.intTry(rs["tipo_arredondamento"].ToString());
            }
        }
        private void update(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                if (Desconto == 0)
                {
                    Desconto = Funcoes.porcDesconto(this.Preco, this.Preco_promocao);
                }

                String sql = "update  preco_mercadoria set (" +
                              "Preco=" + Preco.ToString().Replace(",", ".") +
                              ",Desconto=" + Desconto.ToString().Replace(",", ".") +
                              ",Preco_promocao=" + Preco_promocao.ToString().Replace(",", ".") +
                              ",Desconto_promocao=" + Desconto_promocao.ToString().Replace(",", ".") + ")" +
                              ",tipo_arredondamento=" + tipo_arredondamento.ToString() +
                              "  where  Filial='" + Filial + "' and Codigo_tabela='" + Codigo_tabela + "' and PLU='" + PLU + "'";

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
            String sql = "delete from preco_mercadoria     where  Filial='" + Filial + "' and Codigo_tabela='" + Codigo_tabela + "' and PLU='" + PLU + "'"; ; //colocar campo index
            Conexao.executarSql(sql, conn, tran);
            return true;
        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                if(Desconto == 0)
                {
                    Desconto = Funcoes.porcDesconto(this.Preco, this.Preco_promocao);
                }

                String sql = " insert into preco_mercadoria (" +
                          "Filial," +
                          "Codigo_tabela," +
                          "PLU," +
                          "Preco," +
                          "Desconto," +
                          "Preco_promocao," +
                          "Desconto_promocao," +
                          "tipo_arredondamento"+
                     " )values (" +
                          "'" + Filial + "'" +
                          "," + "'" + Codigo_tabela + "'" +
                          "," + "'" + PLU + "'" +
                          "," + Preco.ToString().Replace(",", ".") +
                          "," + Desconto.ToString().Replace(",", ".") +
                          "," + Preco_promocao.ToString().Replace(",", ".") +
                          "," + Desconto_promocao.ToString().Replace(",", ".") +
                          "," +tipo_arredondamento+
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