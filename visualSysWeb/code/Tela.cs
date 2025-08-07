using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using visualSysWeb.dao;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace visualSysWeb.code
{
    public class ItemPermissao
    {
        public String cod = "";
        public int usuario = 0;
        public String item = "";
        public bool acesso = false;
    }


    public class Tela
    {
        public String cod = "";
        public String nome_Tela = "";
        //public String pagina = "";
        public bool incluir = false;
        public bool editar = false;
        public bool excluir = false;
        public bool visualizar = false;
        public bool adm = false;
        public bool tela_inicial = false;
        public int usuario = 0;
        public ArrayList arrPermissao = new ArrayList();

        public Tela(User usr)
        {
            usuario = usr.getId();

        }
        public void carregaPermissao()
        {
            arrPermissao.Clear();
            SqlDataReader rs = null;
            try
            {


                rs = Conexao.consulta("Select * from usuarios_web_telas_permissoes where  cod='" + cod + "' and usuario=" + usuario, null, false);

                while (rs.Read())
                {
                    ItemPermissao i = new ItemPermissao();
                    i.cod = this.cod;
                    i.usuario = this.usuario;
                    i.item = rs["item"].ToString();
                    i.acesso = rs["acesso"].ToString().Equals("1");

                    arrPermissao.Add(i);
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
        public bool acesso(String item)
        {
            foreach (ItemPermissao i in arrPermissao)
            {
                if (i.item.ToUpper().Equals(item.ToUpper()))
                {
                    return i.acesso;
                }
            }
            return false;
        }



    }
}





















//Sinto Muito Me Perdoe Agradeço Eu Te Amo.