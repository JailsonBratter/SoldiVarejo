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
    public class User
    {
        private int id = 0;
        private String nome = "";
        private String usuario = "";
        private String senha = "";
        public filialDAO filial = new filialDAO();
        private ArrayList telas= new ArrayList();
        public String tela = "";
        private bool admin = false;
        public bool consultaTodasFiliais = false;
        public String host = "";
        public String porta = "";
        public String email = "";
        public String emailSenha = "";
        public String codigo_funcionario = "";
        public int id_operador = 0;
        public string FilialFinanceiro { get; set; }
        public string grupoClientes { get; set; }
        
        public User(String usuario,String senha,String filial) {
            this.usuario = usuario;
            this.senha = senha;
            this.filial = new filialDAO (filial);
            SqlDataReader rs = null;
            String erro = "Usuario ou Senha inválidos";
            try
            {
                try
                {                
                    rs = Conexao.consulta("Select id,nome ,senha, adm,filial,host,porta,email,emailSenha,codigo_funcionario,id_operador, Filial_Financeiro, Grupo_Clientes from usuarios_web where usuario='" + usuario + "' and filial='" + filial + "'", new User(), true);
                }
                catch (Exception)
                {
                    erro = "Erro ao Consultar o Banco Verifique as ultimas Atualizações";
                    throw;
                }
                if (rs.Read())
                {
                    this.id = int.Parse(rs["id"].ToString());

                    if (confirmaSenha(senha))
                    {
                        this.nome = rs["nome"].ToString();
                        this.admin = (rs["adm"].ToString().Equals("1") ? true : false);
                        this.host= rs["host"].ToString();
                        this.porta= rs["porta"].ToString();
                        this.email = rs["email"].ToString();
                        this.emailSenha = rs["emailSenha"].ToString();
                        this.codigo_funcionario = rs["codigo_funcionario"].ToString();
                        this.id_operador= (rs["id_operador"].ToString().Equals("")?0:int.Parse(rs["id_operador"].ToString()));
                        this.FilialFinanceiro = rs["Filial_Financeiro"].ToString();
                        try
                        {
                            this.grupoClientes = rs["Grupo_Clientes"].ToString();
                        }
                        catch
                        {
                            this.grupoClientes = "";
                        }
                        carregarTelas();
                        
                    }
                    else {
                        erro = " - Senha incorreta";
                        throw new Exception();
                    }
                }
                else {
                    erro = " - Usuario não encontrado";
                    throw new Exception();
                }
            }
            catch (Exception)
            {

               throw new Exception(erro);
            }
            finally {
                if (rs != null)
                {
                    rs.Close();
                }
            }
        }
        private bool confirmaSenha(String senha)
        {
            bool logar = false;
            logar = Conexao.retornaUmValor("select pwdCompare( '" + senha + "',senha,0) from usuarios_web where id=" + this.id, null).Equals("1");
            return logar;
        }

        public void carregarTelas() {
            SqlDataReader rs = null;
            try
            {


                rs = Conexao.consulta("select * from usuarios_web_telas where  usuario =" + id, this, true);
                telas.Clear();
                while (rs.Read())
                {
                    Tela t = new Tela(this);
                    t.cod = rs["cod"].ToString();
                    t.nome_Tela = rs["nome_tela"].ToString();
                    t.incluir = (rs["incluir"].ToString().Equals("1") ? true : false);
                    t.editar = (rs["editar"].ToString().Equals("1") ? true : false);
                    t.excluir = (rs["excluir"].ToString().Equals("1") ? true : false);
                    t.visualizar = (rs["visualizar"].ToString().Equals("1") ? true : false);
                    t.adm = (rs["adm"].ToString().Equals("1") ? true : false);
                    t.tela_inicial = (rs["tela_inicial"].ToString().Equals("1") ? true : false);
                    t.carregaPermissao();

                    telas.Add(t);
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
        
        public User(int id) { 
        
        }

        public User(String usuario) { 
        
        }

        public User() { 
        
        }
        public String getNome(){
            return nome.ToUpper();
        }

        public void trocarSenha(String senha, String novaSenha, String ConfirmaSenha)
        {

            if (confirmaSenha(senha))
            {
                if (novaSenha.Equals(ConfirmaSenha))
                {

                    Conexao.executarSql("update usuarios_web set senha=pwdEncrypt('" + novaSenha + "') where id=" + id);
                }
                else
                {
                    throw new Exception("Nova Senha e Confirmação não São iguais");
                }
            }
            else
            {
                throw new Exception("Senha incorreta");
            }
        }

        public bool acesso(String tela) {
            if(admin){
                return true;
            }
            foreach (Tela t in telas)
            {
             if(t.cod.Equals(tela))
             {
                if (t.visualizar || t.adm)
                    return true;
                else
                    return false;
             }
            }
            return false;

        }


        public bool telaPermissao(String item)
        {
            if (admin)
            {
                return true;
            }

            Tela t = telaAtual();


            if (t != null)
            {
                if (t.adm)
                {
                    return true;
                }
                return t.acesso(item);
            }

            return false;
        }
        public Tela telaAtual()
        {
            foreach (Tela t in telas)
            {
                if (t.cod.Equals(this.tela))
                {
                    return t;
                }
            }
            return null;
        }


        public bool incluir(String tela)
        {
            if (admin)
            {
                return true;
            }
            foreach (Tela t in telas)
            {
                if (t.cod.Equals(tela) )
                {
                    if (t.incluir || t.adm)
                        return true;
                    else
                        return false;
                }
            }
            return false;
            //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
        }

        public bool editar(String tela)
        {
            if (admin)
            {
                return true;
            }
            foreach (Tela t in telas)
            {
                if (t.cod.Equals(tela) )
                {
                    if (t.editar|| t.adm)
                        return true;
                    else
                        return false;
                }
            }
            return false;

        }

        public bool excluir(String tela)
        {
            if (admin)
            {
                return true;
            }
            foreach (Tela t in telas)
            {
                if (t.cod.Equals(tela) )
                {
                    if (t.excluir || t.adm)
                        return true;
                    else
                        return false;
                }
            }
            return false;

        }
        public bool admGeral()
        {
            return admin;
        }

        public bool adm()
        {
            return adm(this.tela);
        }

        public bool adm(String tela)
        {
            if (admin)
            {
                return true;
            }
            
            foreach (Tela t in telas)
            {
                if (t.cod.Equals(tela) )
                {
                    if (t.adm)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        public bool inicial(String tela)
        {
            if (admin)
            {
                return true;
            }
            foreach (Tela t in telas)
            {
                if (t.cod.Equals(tela) )
                {
                    if (t.tela_inicial)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        
        public String getUsuario() {
            return usuario.ToUpper(); ;
        }

        public String getFilial() {
          
                return filial.Filial;

        }
        public int getId() {
            return id;
        }

        public String nomeTela()
        {
            foreach (Tela item in telas)
            {
                if (item.cod.Equals(this.tela))
                {
                    return item.nome_Tela;
                    
                }
            }
            return "";
        }


    }
}