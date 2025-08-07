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
    public class mercadoria_obsDAO
    {
        public String filial { get; set; }
        public String plu { get; set; }
        public String obs { get; set; }
        public String plu_item_adc { get; set; }
        public bool obrigatorio = false;
        public int obrigatorioOrdem { get; set; }
        public string tipoCardapio { get; set; }
        public mercadoria_obsDAO() { }
        public mercadoria_obsDAO(String plu, String obs, User usr)
        { //colocar campo index da tabela
            String sql = "Select * from  mercadoria_obs where plu ='" + plu + "' and obs='" + obs + "'";
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
                {
                    rs.Close();
                }
            }
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
                    plu = rs["plu"].ToString();
                    obs = rs["obs"].ToString();
                    plu_item_adc = rs["plu_item_adc"].ToString();
                    obrigatorio = rs["obrigatorio"].ToString().Equals("1");
                    obrigatorioOrdem = Funcoes.intTry(rs["ObrigatorioOrdem"].ToString());
                    tipoCardapio = rs["tipo"].ToString();

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

        public ArrayList ArrToString()
        {
            ArrayList list = new ArrayList();
            list.Add(obs.ToString());
            list.Add(plu_item_adc.ToString());
            list.Add((obrigatorio ? "SIM" : "NAO"));
            list.Add(obrigatorioOrdem.ToString());
            list.Add(tipoCardapio);
            return list;

        }
        private void update(SqlConnection conn, SqlTransaction tran)
        {

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
            String sql = "delete from mercadoria_obs  where plu= " + plu + " and filial='" + filial + "' and obs='" + obs + "'"; //colocar campo index
            Conexao.executarSql(sql, conn, tran);
            return true;
        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = " insert into mercadoria_obs (" +
                          "filial," +
                          "plu," +
                          "obs," +
                          "plu_item_adc" +
                          ",obrigatorio" +
                          ",obrigatorioOrdem" +
                          ",tipo"+
                     " )values( " +
                          "'" + filial + "'" +
                          "," + "'" + plu + "'" +
                          "," + "'" + obs + "'" +
                          "," + "'" + plu_item_adc + "'" +
                          "," + (obrigatorio ? "1" : "0") +
                          "," + obrigatorioOrdem +
                          ",'"+tipoCardapio+"'"+
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