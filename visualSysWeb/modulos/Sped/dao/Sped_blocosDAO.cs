using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace visualSysWeb.dao
{
    public class Sped_blocosDAO
    {
        public int id = 0;
        public String tipoArquivo = "";
        public String bloco = "";
        public String str_procedure = "";
        public int id_bloco_pai = 0;
        public User usr = new User();
        public ArrayList arrParametros = new ArrayList();
        public ArrayList arrBlocosFilhos = new ArrayList();
        public int ordem = 0;
        public bool gerarArquivo = false;
        public String campo_arquivo = "";
        public String nomeFinalArquivo = "";
        public String blocoTotaliza = "";

        public bool arquivoGrupo = false; //Bloco para agruparBlocos , não vai incluir informações no arquivo;


        public Sped_blocosDAO(User usr)
        {
            this.usr = usr;
        }
        public Sped_blocosDAO(int id, User usr)
        { //colocar campo index da tabela
            this.usr = usr;
            String sql = "Select * from  sped_blocos where id =" + id;
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
                    rs.Close();
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

        public void addParametro(Sped_blocos_parametrosDAO parametro)
        {
            parametro.id_bloco = id;
            parametro.index = arrParametros.Count;
            arrParametros.Add(parametro);
        }
        public void removeParametro(Sped_blocos_parametrosDAO parametro)
        {
            arrParametros.Remove(parametro.index);
        }

        public void addBlocoFilho(Sped_blocosDAO blocoFilho)
        {
            blocoFilho.id_bloco_pai = this.id;

            arrBlocosFilhos.Insert(blocoFilho.ordem - 1, blocoFilho);
            arrumaOrdemFilhos();

        }
        private void arrumaOrdemFilhos()
        {
            int index = 1;
            foreach (Sped_blocosDAO bFilho in arrBlocosFilhos)
            {
                bFilho.ordem = index;
                index++;
            }
        }
        public void removeBlocoFilho(Sped_blocosDAO blocoFilho)
        {
            arrBlocosFilhos.RemoveAt(blocoFilho.ordem - 1);
            arrumaOrdemFilhos();
        }


        public void carregarDados(SqlDataReader rs)
        {
            try
            {


                if (rs.Read())
                {
                    id = (rs["id"] == null ? 0 : int.Parse(rs["id"].ToString()));
                    bloco = rs["bloco"].ToString();
                    tipoArquivo = rs["tipoArquivo"].ToString();
                    str_procedure = rs["str_procedure"].ToString();
                    id_bloco_pai = (rs["id_bloco_pai"] == null ? 0 : int.Parse(rs["id_bloco_pai"].ToString()));
                    int.TryParse(rs["ordem"].ToString(), out ordem);
                    gerarArquivo = rs["gerarArquivo"].ToString().Equals("1");
                    campo_arquivo = rs["campoArquivo"].ToString();
                    blocoTotaliza = rs["blocoTotaliza"].ToString();
                    arquivoGrupo = rs["bloco_grupo"].ToString().Equals("1");
                    carregarParametros();
                    carregarBlocosFilhos();
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

        public void carregarParametros()
        {
            String sql = "Select * from sped_blocos_parametros where id_bloco=" + this.id;
            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta(sql, usr, false);
                arrParametros.Clear();
                while (rs.Read())
                {
                    Sped_blocos_parametrosDAO parametro = new Sped_blocos_parametrosDAO(usr);
                    int.TryParse(rs["id_parametro"].ToString(), out parametro.id_parametro);
                    parametro.nome_parametro = rs["nome_parametro"].ToString();
                    parametro.tipo_dados = rs["tipo_dados"].ToString();
                    parametro.campo_pai = rs["campo_pai"].ToString();
                    parametro.herda_pai = rs["herda_pai"].ToString().Equals("1");
                    addParametro(parametro);
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


        private void carregarBlocosFilhos()
        {
            String sql = "Select * from sped_blocos where id_bloco_pai=" + this.id + " order by ordem ";
            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta(sql, usr, false);
                arrBlocosFilhos.Clear();
                while (rs.Read())
                {
                    Sped_blocosDAO fBloco = new Sped_blocosDAO(usr);
                    fBloco.id = (rs["id"] == null ? 0 : int.Parse(rs["id"].ToString()));
                    fBloco.bloco = rs["bloco"].ToString();
                    fBloco.str_procedure = rs["str_procedure"].ToString();
                    fBloco.id_bloco_pai = (rs["id_bloco_pai"] == null ? 0 : int.Parse(rs["id_bloco_pai"].ToString()));
                    fBloco.tipoArquivo = rs["tipoArquivo"].ToString();
                    fBloco.gerarArquivo = rs["gerarArquivo"].ToString().Equals("1");
                    fBloco.campo_arquivo = rs["campoArquivo"].ToString();
                    fBloco.blocoTotaliza = rs["blocoTotaliza"].ToString();
                    fBloco.arquivoGrupo = rs["bloco_grupo"].ToString().Equals("1");

                    fBloco.carregarParametros();
                    fBloco.carregarBlocosFilhos();
                    arrBlocosFilhos.Add(fBloco);
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
                String sql = "update  sped_blocos set " +
                              "bloco='" + bloco + "'" +
                              ",tipoArquivo='" + tipoArquivo + "'" +
                              ",str_procedure='" + str_procedure + "'" +
                              ",id_bloco_pai=" + id_bloco_pai +
                              ",ordem =" + ordem +
                              ",gerarArquivo=" + (gerarArquivo ? "1" : "0") +
                              ",campo_arquivo='" + campo_arquivo + "'" +
                              ",blocoTotaliza='" + blocoTotaliza + "'" +
                    "  where id = " + this.id;
                Conexao.executarSql(sql, conn, tran);

                salvarParametros(conn, tran);
            }
            catch (Exception err)
            {
                throw new Exception("Nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }
        public bool salvar(bool novo)
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                if (novo)
                {
                    insert(conn, tran);
                }
                else
                {
                    update(conn, tran);
                }

                tran.Commit();
            }
            catch (Exception err)
            {
                tran.Rollback();
                throw err;
            }
            finally
            {

                if (conn != null)
                    conn.Close();
            }
            return true;

        }

        public bool excluir()
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {

                int blocoFilhos = 0;
                int.TryParse(Conexao.retornaUmValor("Select count(*) from sped_blocos where id_bloco_pai=" + this.id, null), out blocoFilhos);
                if (blocoFilhos > 0)
                    throw new Exception("Não é possivel Excluir um Bloco que contenha Blocos Filhos!");


                String sql = "delete from sped_blocos  where id= " + this.id;
                Conexao.executarSql(sql, conn, tran);

                String sqlParametros = "delete from sped_blocos_parametros where id_bloco =" + this.id;
                Conexao.executarSql(sqlParametros, conn, tran);

                tran.Commit();
            }
            catch (Exception err)
            {
                tran.Rollback();
                throw err;
            }
            finally
            {

                if (conn != null)
                    conn.Close();
            }
            return true;
        }



        private void salvarParametros(SqlConnection conn, SqlTransaction tran)
        {

            String sqlParametros = "delete from sped_blocos_parametros where id_bloco =" + this.id;
            Conexao.executarSql(sqlParametros, conn, tran);

            foreach (Sped_blocos_parametrosDAO param in arrParametros)
            {
                param.id_bloco = this.id;
                param.salvar(true, conn, tran);
            }

        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {

                int.TryParse(Conexao.retornaUmValor("Select isnull(MAX(ID),0)+1 from sped_blocos", null), out id);

                String sql = " insert into sped_blocos (" +
                          "id," +
                          "bloco," +
                          "tipoArquivo" +
                          "str_procedure," +
                          "id_bloco_pai" +
                          ",ordem" +
                          ",gerarArquivo" +
                          ",campo_arquivo" +
                          ",blocoTotaliza" +
                     ") values (" +
                          id +
                          ",'" + bloco + "'" +
                          ",'" + tipoArquivo + "'" +
                          ",'" + str_procedure + "'" +
                          "," + id_bloco_pai +
                          "," + ordem +
                          "," + (gerarArquivo ? "1" : "0") +
                          ",'" + campo_arquivo + "'" +
                          ",'" + blocoTotaliza + "'" +
                         ");";


                salvarParametros(conn, tran);
                Conexao.executarSql(sql, conn, tran);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }
    }
}