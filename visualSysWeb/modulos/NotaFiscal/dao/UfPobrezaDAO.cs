using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class UfPobrezaDAO
    {
        public String uf { get; set; }
        public decimal porc { get; set; }
        public decimal aliq_interna = 0;
        public int calc_Fora = 0;

        public UfPobrezaDAO()
        {
        }
        public UfPobrezaDAO(String uf)
        {
            this.uf = uf;
            carregarDados();
        }

        private void carregarDados()
        {
            String sql = "Select * from uf_pobreza where uf ='" + this.uf + "'";
            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta(sql, null, false);
                if (rs.Read())
                {
                    decimal porc = 0;
                    Decimal.TryParse(rs["porc"].ToString(), out porc);
                    this.porc = porc;
                }
            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }
        }

        public void Excluir()
        {
            String sql = "DELETE FROM  uf_pobreza WHERE UF = '" + this.uf + "'";
            Conexao.executarSql(sql);
        }

        public void insert()
        {
            int qtd = 0;
            int.TryParse(Conexao.retornaUmValor("Select count(uf) from uf_pobreza where uf ='" + this.uf + "'", null), out qtd);
            if (qtd > 0)
            {
                throw new Exception("Uf Já Cadastrada!");
            }


            String sql = "insert into uf_pobreza values ('" + this.uf + "'," + Funcoes.decimalPonto(this.porc.ToString()) + ")";
            Conexao.executarSql(sql);
        }

        public void update()
        {
            String sql = "update uf_pobreza set porc =" + Funcoes.decimalPonto(this.porc.ToString()) + " where uf ='" + this.uf + "'";
            Conexao.executarSql(sql);
        }

        public void salvar(bool novo)
        {
            if (novo)
                insert();
            else
                update();

        }


        public static UfPobrezaDAO objUFPobreza(String Uf)
        {
            String sql = "Select * from Uf_pobreza where uf = '" + Uf + "'";
            SqlDataReader rs = null;
            UfPobrezaDAO uf = new UfPobrezaDAO();
            try
            {
                rs = Conexao.consulta(sql, null, false);
                if (rs.Read())
                {

                    uf.uf = Uf;
                    uf.porc = Funcoes.decTry(rs["porc"].ToString());
                    uf.aliq_interna = Funcoes.decTry(rs["aliq_interna"].ToString());
                    uf.calc_Fora = Funcoes.intTry(rs["calc_fora"].ToString());
                }

            }
            catch (Exception err)
            {


                throw err;
            }
            finally
            {
                if (rs != null)
                {
                    rs.Close();
                }
            }
            return uf;

        }
        public static List<UfPobrezaDAO> Cadastradas(String ordem)
        {

            if (ordem.Equals(""))
            {
                ordem += "uf";
            }
            List<UfPobrezaDAO> lista = new List<UfPobrezaDAO>();
            String sql = "Select * from uf_pobreza order by " + ordem;

            SqlDataReader rs = null;

            try
            {
                rs = Conexao.consulta(sql, null, false);
                while (rs.Read())
                {
                    UfPobrezaDAO uf = new UfPobrezaDAO();
                    uf.uf = rs["uf"].ToString();
                    decimal porc = 0;
                    Decimal.TryParse(rs["porc"].ToString(), out porc);
                    uf.porc = porc;
                    lista.Add(uf);
                }
            }
            catch (Exception err)
            {

                throw err;
            }
            finally
            {

            }

            return lista;
        }



    }
}