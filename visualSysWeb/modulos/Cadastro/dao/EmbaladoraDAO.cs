using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.dao
{
    public class EmbaladoraDAO
    {
        public int ID { get; set; }
        public string Descricao { get; set; }
        public string Filial { get; set; }
        public string  End_FTP { get; set; }
        public string Usr { get; set; }
        public string Senha { set; get; }
        public string Usuario { get; set; }

        public User usr = null; 
        public EmbaladoraDAO(User usr)
        {
            this.usr = usr;
            this.Filial = usr.getFilial();
        }
        public EmbaladoraDAO(int ID, User usr)
        {
            this.usr = usr;
            this.Filial = usr.getFilial();
            this.ID = ID;
            carregarDados();
        }

        private void carregarDados()
        {
            String sql = "Select * from Embaladoras where id ="+ID;
            SqlDataReader rs ;
            try
            {
                rs = Conexao.consulta(sql, usr, false);
                if(rs.Read())
                {
                    this.Descricao = rs["Descricao"].ToString();
                    this.End_FTP = rs["End_FTP"].ToString();
                    this.Usuario = rs["usuario"].ToString();
                    this.Senha = rs["Senha"].ToString();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void salvar(bool incluir)
        {
            if (incluir)
                insert();
            else
                update();

        }

        private void update()
        {
            string sql = "update Embaladoras set ";

            
            sql += "Descricao='" + Descricao + "'";
            sql += ",End_FTP='" + this.End_FTP+ "'";
            sql += ",usuario='" + this.Usuario + "'";
            sql += ",senha='" + this.Senha + "'";

            sql += " where ID =" + this.ID + " AND Filial = '" + Filial + "';";

            Conexao.executarSql(sql);

        }

        private void insert()
        {
            this.ID = Funcoes.intTry(Funcoes.sequencia("Embaladoras.id",usr));
            Funcoes.salvaProximaSequencia("Embaladoras.id", usr);

            string sql = "insert into Embaladoras ( ";
            string value = ") values( ";
            
            sql += "id";
            value += this.ID;
            
            sql += ",filial";
            value += ",'" + this.Filial + "'";

            sql += ",Descricao";
            value += ",'" + this.Descricao + "'";

            sql += ",End_FTP";
            value += ",'" + End_FTP + "'";

            sql += ",usuario";
            value += " ,'" + this.Usuario + "'";

            sql += ",senha";
            value += ",'" + Senha + "'";

            sql += value + ");";

            Conexao.executarSql(sql);




        }

        internal void excluir()
        {
            String sql = "Delete from Embaladoras where id=" + this.ID;
            Conexao.executarSql(sql);
        }
    }
}