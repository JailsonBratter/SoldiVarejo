using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Estoque.dao
{
    public class solicitacao_producaoDAO
    {
        public String filial { get; set; }
        public String codigo { get; set; }
        public String descricao { get; set; }
        public DateTime data_cadastro = new DateTime();
        public String data_cadastroBr()
        {
            return dataBr(data_cadastro);
        }

        public String usuario_cadastro { get; set; }
        public String status { get; set; }

        public List<solicitacao_producao_itensDAO> arrItens = new List<solicitacao_producao_itensDAO>();
        public int gridInicio = 0;
        public int gridFim = 100;
        public User usr;
        public bool itensDigitados = false;
        public int qtdeItensDigitados
        {
            get
            {
                int qtde = 0;
                foreach (solicitacao_producao_itensDAO item in arrItens)
                {
                    if (item.qtde > 0)
                        qtde++;
                }
                return qtde;
            }
        }
        public int tipoProducao { get; set; }

        public solicitacao_producao_itensDAO item(String plu)
        {
            foreach (solicitacao_producao_itensDAO item in arrItens)
            {
                if (plu.Equals(item.plu))
                    return item;
            }
            return null;
        }


        public solicitacao_producaoDAO(User usr, int tipo_producao)
        {
            this.usr = usr;
            this.filial = usr.getFilial();
            this.tipoProducao = tipo_producao;
        }
        public solicitacao_producaoDAO(String strCodigo, int tipo_producao, User usr)
        { //colocar campo index da tabela
            this.usr = usr;
            this.filial = usr.getFilial();
            this.tipoProducao = tipo_producao;
            String sql = "Select * from  solicitacao_producao where codigo ='" + strCodigo + "' and filial='" + usr.getFilial() + "' and isnull(tipo_producao,0)=" + tipo_producao;
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


        public bool itemIncluido(String plu)
        {
            bool oItem = false;
            foreach (solicitacao_producao_itensDAO item in arrItens)
            {
                if (item.plu.Equals(plu))
                {
                    oItem = true;
                    break;
                }
            }
            return oItem;
        }

        public void excluirItem(String plu)
        {
            foreach (solicitacao_producao_itensDAO item in arrItens)
            {
                if (item.plu.Equals(plu))
                {
                    arrItens.Remove(item);
                    break;
                }
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
                    descricao = rs["descricao"].ToString();
                    DateTime.TryParse(rs["data_cadastro"].ToString(), out data_cadastro);
                    usuario_cadastro = rs["usuario_cadastro"].ToString();
                    status = rs["status"].ToString();

                }

                carregarItens();
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




        private void carregarItens()
        {
            arrItens.Clear();
            SqlDataReader rs = null;
            try
            {


                String sql = "Select * from  solicitacao_producao_itens " +
                            " where codigo ='" + codigo + "'  " +
                            "   and filial='" + filial + "' " +
                            "   and isnull(tipo_producao,0)=" + tipoProducao +
                            " order by ordem";
                rs = Conexao.consulta(sql, null, false);
                while (rs.Read())
                {
                    solicitacao_producao_itensDAO item = new solicitacao_producao_itensDAO(usr, tipoProducao);
                    item.filial = rs["filial"].ToString();
                    item.codigo = rs["codigo"].ToString();
                    item.plu = rs["plu"].ToString();
                    item.ean = rs["ean"].ToString();
                    item.ref_fornecedor = rs["ref_fornecedor"].ToString();
                    item.descricao = rs["descricao"].ToString();

                    item.und = rs["und"].ToString();

                    item.qtde = Funcoes.decTry(rs["qtde"].ToString());
                    item.ordem = Funcoes.intTry(rs["ordem"].ToString());
                    item.obs = rs["obs"].ToString();
                    arrItens.Add(item);
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



        public bool existItemZerado()
        {
            foreach (solicitacao_producao_itensDAO item in arrItens)
            {
                if (item.qtde <= 0)
                {
                    return true;
                }
            }
            return false;
        }


        public void ordemItens()
        {
            int i = 1;
            foreach (solicitacao_producao_itensDAO item in arrItens)
            {
                item.ordem = i;
                i++;
            }
        }

        public List<solicitacao_producao_itensDAO> Itens
        {
            get
            {
                List<solicitacao_producao_itensDAO> conteudo = new List<solicitacao_producao_itensDAO>();

                if (arrItens.Count > 0)
                {

                    for (int i = gridInicio; (i < gridFim) && (i < arrItens.Count); i++)
                    {
                        solicitacao_producao_itensDAO item = arrItens[i];
                        item.ordem = i + 1;


                        if (itensDigitados)
                        {
                            if (item.qtde > 0)
                                conteudo.Add(item);
                        }
                        else
                        {
                            conteudo.Add(item);
                        }
                    }
                }
                else
                {
                    conteudo.Add(new solicitacao_producao_itensDAO(null, tipoProducao));
                }
                return conteudo;
            }
        }




        private void update(SqlConnection conn, SqlTransaction trans)
        {
            try
            {
                String sql = "update  solicitacao_producao set " +
                              "descricao='" + descricao + "'" +
                              ",data_cadastro=" + (data_cadastro.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_cadastro.ToString("yyyy-MM-dd") + "'") +
                              ",usuario_cadastro='" + usuario_cadastro + "'" +
                              ",status='" + status + "'" +
                    "  where codigo='" + codigo + "' and filial='" + filial + "' and tipo_producao=" + tipoProducao;
                Conexao.executarSql(sql, conn, trans);

                Conexao.executarSql("delete from solicitacao_producao_itens " +
                                   " where codigo ='" + codigo + "'  " +
                                   "   and filial='" + filial + "' " +
                                   "   and isnull(tipo_producao,0)=" + tipoProducao, conn, trans);
                foreach (solicitacao_producao_itensDAO item in arrItens)
                {

                    item.codigo = codigo;
                    item.filial = filial;
                    item.salvar(true, conn, trans);
                }


            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
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
            String sql = "delete from solicitacao_producao  where campoIndex= "; //colocar campo index
            Conexao.executarSql(sql);
            return true;
        }

        private void insert(SqlConnection conn, SqlTransaction trans)
        {
            try
            {
                String sequencia = "";
                if (tipoProducao == 0)
                {
                    sequencia = "SOLICITACAO_PRODUCAO.CODIGO";

                }
                else
                {
                    sequencia = "SOLICITACAO_ENCOMENDA.CODIGO";
                }
                codigo = Funcoes.sequencia(sequencia, usr, conn, trans);
                Funcoes.salvaProximaSequencia(sequencia, usr, conn, trans);
                filial = usr.getFilial();

                String sql = " insert into solicitacao_producao (" +
                          "filial," +
                          "codigo," +
                          "descricao," +
                          "data_cadastro," +
                          "usuario_cadastro," +
                          "status" +
                          ",tipo_producao" +
                     ") values (" +
                          "'" + filial + "'" +
                          ",'" + codigo + "'" +
                          ",'" + descricao + "'" +
                          "," + (data_cadastro.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_cadastro.ToString("yyyy-MM-dd") + "'") +
                          ",'" + usuario_cadastro + "'" +
                          ",'" + status + "'" +
                          "," + tipoProducao.ToString("1") +
                         ");";

                Conexao.executarSql(sql, conn, trans);

                foreach (solicitacao_producao_itensDAO item in arrItens)
                {
                    item.codigo = codigo;
                    item.filial = filial;
                    item.salvar(true, conn, trans);
                }




            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }
    }
}