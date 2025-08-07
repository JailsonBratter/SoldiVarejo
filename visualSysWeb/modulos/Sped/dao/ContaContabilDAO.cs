using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class ContaContabilDAO
    {
        public string filial { get; set; }
        public DateTime data { get; set; }
        public string strData
        {
            get
            {
                if (data.Equals(DateTime.MinValue))
                    return "";
                else
                    return data.ToString("dd/MM/yyyy");
            }
        }
        public string cod_conta { get; set; }
        public string descricao { get; set; }
        public string tipo { get; set; }
        public string strTipo
        {
            get
            {
                if (tipo != null)
                {
                    if (tipo.Equals("A"))
                    {
                        return "A-Analitica(conta)";
                    }
                    else if (tipo.Equals("S"))
                    {
                        return "S-Sintetica(grupo de contas)";
                    }
                    else
                        return tipo;
                }
                return "";
            }
        }
        public string natureza { get; set; }
        public string strNatureza
        {
            get
            {
                if (natureza != null)
                {
                    if (natureza.Equals("01"))
                    {
                        return "01-Contas de ativo";
                    }
                    else if (natureza.Equals("02"))
                    {
                        return "02-Contas de passivo";
                    }
                    else if (natureza.Equals("03"))
                    {
                        return "03-Patrimonio líquido";
                    }
                    else if (natureza.Equals("04"))
                    {
                        return "04-Contas de resultado";
                    }
                    else if (natureza.Equals("05"))
                    {
                        return "05-Contas de compensação";
                    }
                    else if (natureza.Equals("09"))
                    {
                        return "09-Outras";
                    }
                    else
                    {
                        return natureza;
                    }
                }
                return "";

            }
        }
        public string nivel { get; set; }
        public string conta_relacionada { get; set; }
        public string cnpj_estabelecimento { get; set; }
        public bool entrada { get; set; }
        public string strEntradaSaida
        {
            get
            {
                if (entrada)
                    return "Entrada";
                else
                    return "Saida";
            }
        }
        private User usr;
        public ContaContabilDAO(User usr)
        {
            this.usr = usr;
            this.filial = usr.getFilial();
        }
        public ContaContabilDAO(String cnpj, String codigo, User usr)
        {
            this.cnpj_estabelecimento = cnpj;
            this.cod_conta = codigo;
            this.usr = usr;
            this.filial = usr.getFilial();
            carregarDados();
        }

        private void carregarDados()
        {
            String sql = "Select * from Conta_Contabil where CNPJ_Estabelecimento ='" + cnpj_estabelecimento + "' and cod_conta='" + cod_conta + "'";
            SqlDataReader rs = null;

            try
            {
                rs = Conexao.consulta(sql, usr, false);
                if (rs.Read())
                {
                    data = Funcoes.dtTry(rs["data"].ToString());
                    descricao = rs["descricao"].ToString();
                    tipo = rs["tipo"].ToString();
                    natureza = rs["natureza"].ToString();
                    nivel = rs["nivel"].ToString();
                    conta_relacionada = rs["conta_relacionada"].ToString();
                    cnpj_estabelecimento = rs["cnpj_estabelecimento"].ToString();
                    entrada = rs["entrada"].ToString().Equals("1");

                }
                else
                {
                    throw new Exception("Plano de contas Não Existe!");
                }
            }
            catch (Exception err)
            {

                throw err;
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }

        }

        public void salvar(bool novo)
        {
            if (novo)
            {
                insert();
            }
            else
            {
                update();
            }

        }
        private void insert()
        {

            int cad = Funcoes.intTry(Conexao.retornaUmValor("Select count(*) from conta_contabil where Cod_Conta='" + cod_conta + "' and cnpj_estabelecimento='" + cnpj_estabelecimento + "'", usr));
            if (cad > 0)
                throw new Exception("Conta já Cadastrada!");


            String sql = "insert into Conta_Contabil (" +
                    "filial" +
                    ",data" +
                    ",Cod_Conta" +
                    ",descricao"+
                    ",tipo" +
                    ",natureza" +
                    ",nivel" +
                    ",conta_relacionada" +
                    ",cnpj_estabelecimento" +
                    ",entrada"+
                ") values(";
            sql += "'" + filial + "'";
            sql += ",'" + data.ToString("yyyy-MM-dd") + "'";
            sql += ",'" + cod_conta.ToString() + "'";
            sql += ",'" + descricao.ToString() + "'";
            sql += ",'" + tipo.ToString() + "'";
            sql += ",'" + natureza.ToString() + "'";
            sql += ",'" + nivel.ToString() + "'";
            sql += ",'" + conta_relacionada.ToString() + "'";
            sql += ",'" + cnpj_estabelecimento.ToString() + "'";
            sql += "," + (entrada? "1":"0");

            sql += ");";

            Conexao.executarSql(sql);

        }
        private void update()
        {
            String sql = "update Conta_Contabil set ";
            sql += "data=" + (data.Equals(DateTime.MinValue) ? "null" : "'" + data.ToString("yyyy-MM-dd") + "'");
            sql += ", tipo='" + tipo + "'";
            sql += ", descricao='" + descricao + "'";
            sql += ", natureza='" + natureza + "'";
            sql += ", nivel='" + nivel + "'";
            sql += ", conta_relacionada='" + conta_relacionada+ "'";
            sql += ", entrada="+(entrada ? "1" : "0");

            sql += " where filial ='" + filial + "' and cod_conta ='" + cod_conta + "' and CNPJ_Estabelecimento ='" + cnpj_estabelecimento + "'";
            Conexao.executarSql(sql);

        }
        public void excluir()
        {
            String sql = "delete from Conta_Contabil where filial ='" + filial + "' and cod_conta ='" + cod_conta + "' and CNPJ_Estabelecimento ='" + cnpj_estabelecimento + "'"  ;
            Conexao.executarSql(sql);

        }


    }
}