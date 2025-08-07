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
    public class itemDAO
    {
        public String Filial { get; set; }
        public String PLU { get; set; }
        public String Plu_item { get; set; }
        public String Descricao { get; set; }
        public Decimal Preco_custo
        {
            get
            {
                if (Preco_compra > 0 && Fator_conversao > 0)
                {
                    return Preco_compra / Fator_conversao;
                }
                else
                {
                    return 0;
                }
            }
            set { }
        }
        public Decimal Custo_Unitario
        {
            get
            {
                if (Preco_custo > 0 && Qtde > 0)
                {
                    return (Preco_custo * Qtde);
                }
                else
                    return 0;
            }
            set { }
        }
        public Decimal Fator_conversao { get; set; }
        public Decimal Preco_compra { get; set; }
        public Decimal Qtde { get; set; }
        public String Und { get; set; }
        private User usr = null;


        public itemDAO() { }
        public itemDAO(String plu, String pluItem, User usr)
        { //colocar campo index da tabela
            this.usr = usr;
            this.PLU = plu;
            this.Plu_item = pluItem;
            carregarDados();
        }

        public ArrayList ArrToString()
        {
            ArrayList item = new ArrayList();
            item.Add(Filial);
            item.Add(PLU);
            item.Add(Plu_item);
            item.Add(Descricao);
            item.Add(Preco_custo.ToString("N2"));
            item.Add(Fator_conversao.ToString());
            item.Add(Preco_compra.ToString("N2"));
            item.Add(Und);
            item.Add(Custo_Unitario.ToString("N2"));
            item.Add(Qtde.ToString("N2"));

            return item;
        }

        public void carregarDados()
        {
            String sql = "Select item.*,Und = isnull(m.und_producao, m.und),m.preco_compra from  item " +
                 " inner join mercadoria as m on item.plu = m.plu where item.plu ='" + PLU + "' and item.plu_item='" + Plu_item + "'";
            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta(sql, usr, true);

                if (rs.Read())
                {
                    Filial = rs["Filial"].ToString();
                    PLU = rs["PLU"].ToString();
                    Plu_item = rs["Plu_item"].ToString();
                    Fator_conversao = Funcoes.decTry(rs["fator_conversao"].ToString());
                    Preco_compra = Funcoes.decTry(rs["preco_compra"].ToString());
                    Und = rs["Und"].ToString();
                    Qtde = Funcoes.decTry(rs["Qtde"].ToString());


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
        private void update(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = "update  item set " +
                              "Plu_item='" + Plu_item + "'" +
                              ",fator_conversao=" + Fator_conversao.ToString().Replace(",", ".") +
                    "  where Filial='" + Filial + "' and PLU='" + PLU + "' and Plu_item='" + Plu_item + "'"
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
            String sql = "delete from item  where Filial='" + Filial + "' and PLU='" + PLU + "' and Plu_item='" + Plu_item + "'";
            Conexao.executarSql(sql, conn, tran);
            return true;
        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = " insert into item (" +
                          "Filial," +
                          "PLU," +
                          "Plu_item," +
                          "fator_conversao" +
                          ",qtde" +
                     " )values (" +
                          "'" + Filial + "'" +
                          "," + "'" + PLU + "'" +
                          "," + "'" + Plu_item + "'" +
                          "," + Fator_conversao.ToString().Replace(",", ".") +
                          "," + Funcoes.decimalPonto(Qtde.ToString()) +
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