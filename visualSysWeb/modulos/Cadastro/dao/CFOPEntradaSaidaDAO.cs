using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class CFOPEntradaSaidaDAO
    {
        public int id { get; set; } = 0;
        public String CFOPEntrada { get; set; }
        public String CFOPSaida { get; set; }
        public String DESCRICAO { get; set; }
        public CFOPEntradaSaidaDAO() { }
        public CFOPEntradaSaidaDAO(String id, User usr)
        { //colocar campo index da tabela
            String sql = "Select * from  CFOP_Entrada_Saida where id =" + id;
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
            if (rs.Read())
            {
                CFOPEntrada = rs["CFOP_entrada"].ToString();
                CFOPSaida = rs["CFOP_Saida"].ToString();
                DESCRICAO = rs["DESCRICAO"].ToString();
            }
        }
        private void update()
        {
            try
            {
                String sql = "update  CFOP_Entrada_Saida set " +
                              "CFOP_entrada='" + CFOPEntrada +"',"+
                              "CFOP_saida= '" + CFOPEntrada.ToString()+"'"+
                              ",DESCRICAO='" + DESCRICAO + "'" +
                    "  where id = " + id
                        ;
                Conexao.executarSql(sql);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }
        public bool salvar(bool novo)
        {
            if (novo)
            {
                insert();
            }
            else
            {
                update();
            }
            return true;
        }

        public bool excluir()
        {
            String sql = "delete from CFOP_Entrada_Saida  where id= " + id; //colocar campo index
            Conexao.executarSql(sql);
            return true;
        }

        private void insert()
        {
            try
            {
                this.id = Funcoes.intTry(Funcoes.sequencia("cfop_entrada_saida", null));
                String sql = " insert into CFOP_Entrada_Saida (" +
                          "id,"+
                          "cfop_entrada," +
                          "cfop_saida," +
                          "DESCRICAO)" +
                     " values (" +
                          id+","+
                          "'"+CFOPEntrada.ToString() +"',"+
                          "'"+CFOPSaida.ToString() +"',"+
                          "'" + DESCRICAO + "'" +
                         ")";

                Conexao.executarSql(sql);
                Funcoes.salvaProximaSequencia("cfop_entrada_saida", null);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }
    }
}