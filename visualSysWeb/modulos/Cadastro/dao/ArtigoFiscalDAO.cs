using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using visualSysWeb.dao;
using System.Data.SqlClient;

namespace visualSysWeb.dao
{
    public class ArtigoFiscalDAO
    {
        public Decimal ArtigoFiscal { get; set; }
        public String Descricao { get; set; }
        public ArtigoFiscalDAO() { }
        public ArtigoFiscalDAO(String campoIndex, User usr)
        { //colocar campo index da tabela
            String sql = "Select * from  Artigo_Fiscal where Artigo =" + campoIndex;
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
                ArtigoFiscal = (Decimal)(rs["Artigo"].ToString().Equals("") ? new Decimal() : rs["Artigo"]);
                Descricao = rs["Descricao"].ToString();
            }
        }
        private void update()
        {
            try
            {
                String sql = "UPDATE  Artigo_Fiscal SET " +
                              "Artigo =" + ArtigoFiscal.ToString().Replace(",", ".") +
                              ", Descricao='" + Descricao + "'" +
                    "  WHERE Artigo = " + ArtigoFiscal
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
            String sql = "DELETE FROM Artigo_Fiscal  where cfop= " + ArtigoFiscal; //colocar campo index
            Conexao.executarSql(sql);
            return true;
        }

        private void insert()
        {
            try
            {
                String sql = " insert into Artigo_Fiscal (" +
                          "Artigo," +
                          "Descricao," +
                     " values (" +
                          ArtigoFiscal.ToString().Replace(",", ".") +
                          "," + "'" + Descricao + "'" +
                         ")";

                Conexao.executarSql(sql);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }
    }
}
