using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Estoque.dao
{
    public class solicitacao_producao_itensDAO
    {

        public String filial { get; set; }
        public String codigo { get; set; }
        public String plu { get; set; }
        public String ean { get; set; }
        public String ref_fornecedor { get; set; }
        public String descricao { get; set; }
        public String und { get; set; }
        public Decimal qtde { get; set; }
        public int ordem { get; set; }
        private User usr = null;
        public String obs { get; set; }
        public int tipo_producao { get; set; }

        public solicitacao_producao_itensDAO(User usr, int tipo_producao)
        {
            this.usr = usr;
            this.tipo_producao = tipo_producao;
        }
        public solicitacao_producao_itensDAO(String codigo, String plu, int tipo_producao, User usr)
        { //colocar campo index da tabela
            this.usr = usr;
            this.tipo_producao = tipo_producao;
            String sql = "Select * from  solicitacao_compra_itens " +
                        " where codigo ='" + codigo + "' " +
                            " and plu='" + plu + "' " +
                            " and filial='" + usr.getFilial() + "' " +
                            " and tipo_producao = " + tipo_producao;

            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            carregarDados(rs);
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
            try
            {
                if (rs.Read())
                {
                    filial = rs["filial"].ToString();
                    codigo = rs["codigo"].ToString();
                    plu = rs["plu"].ToString();
                    ean = rs["ean"].ToString();
                    ref_fornecedor = rs["ref_fornecedor"].ToString();
                    descricao = rs["descricao"].ToString();
                    und = rs["und"].ToString();
                    qtde = Funcoes.decTry(rs["qtde_comprar"].ToString());
                    ordem = Funcoes.intTry(rs["Ordem"].ToString());
                    obs = rs["obs"].ToString();

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
        private void update(SqlConnection conn, SqlTransaction trans)
        {
            try
            {
                String sql = "update  solicitacao_producao_itens set " +
                              "filial='" + filial + "'" +
                              ",codigo='" + codigo + "'" +
                              ",plu='" + plu + "'" +
                              ",ean='" + ean + "'" +
                              ",ref_fornecedor='" + ref_fornecedor + "'" +
                              ",descricao='" + descricao + "'" +
                              "und='" + und + "'" +
                              ",qtde=" + Funcoes.decimalPonto(qtde.ToString()) +
                              ",ordem=" + ordem +
                              ",obs = '" + obs + "'" +
                              ",tipo_producao= " + tipo_producao +

                    "  where codigo ='" + codigo + "' and plu='" + plu + "' and filial='" + filial + "'";
                Conexao.executarSql(sql, conn, trans);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }
        public bool salvar(bool novo, SqlConnection conn, SqlTransaction trans)
        {
            if (novo)
            {
                insert(conn, trans);
            }
            else
            {
                update(conn, trans);
            }
            return true;
        }

        public bool excluir()
        {
            String sql = "delete from solicitacao_producao_itens  where codigo ='" + codigo + "' and plu='" + plu + "' and filial='" + filial + "'";
            Conexao.executarSql(sql);
            return true;
        }

        private void insert(SqlConnection conn, SqlTransaction trans)
        {
            try
            {
                String sql = " insert into solicitacao_producao_itens( " +
                          "filial," +
                          "codigo," +
                          "plu," +
                          "ean," +
                          "ref_fornecedor," +
                          "descricao," +
                          "und," +
                          "qtde" +
                          ",ordem" +
                          ",obs" +
                          ",tipo_producao" +
                     ") values (" +
                          "'" + filial + "'" +
                          "," + "'" + codigo + "'" +
                          "," + "'" + plu + "'" +
                          "," + "'" + ean + "'" +
                          "," + "'" + ref_fornecedor + "'" +
                          "," + "'" + descricao + "'" +
                          ",'" + und + "'" +
                          "," + Funcoes.decimalPonto(qtde.ToString()) +
                          "," + ordem +
                          ",'" + obs + "'" +
                          "," + tipo_producao.ToString() +
                      ");";

                Conexao.executarSql(sql, conn, trans);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }
    }
}