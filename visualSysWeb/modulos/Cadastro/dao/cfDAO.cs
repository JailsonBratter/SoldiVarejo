using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace visualSysWeb.dao
{
    public class cfDAO
    {
        public String cf = "";
        public String descricao = "";
        public Decimal margem_iva = 0;
        public Decimal margem_iva_ajustado = 0;
        public Decimal perc_imp_estadual = 0;
        public Decimal perc_imp_fed_nac = 0;
        public Decimal perc_imp_fed_importado = 0;
        public Decimal perc_pis = 0;
        public Decimal perc_pis_entrada = 0;
        public String cst_pis = "";
        public Decimal perc_cofins = 0;
        public Decimal perc_cofins_entrada = 0;
        public String cst_cofins = "";
        public String CEST = "";
        private User usr = null;
        public ArrayList arrUfs = new ArrayList();


        public cfDAO(User usr)
        {
            this.usr = usr;
            carregarUFs();
        }

        public cfDAO(String ncm, User usr)
        { //colocar campo index da tabela
            this.usr = usr;
            this.cf = ncm;
            carregarDados();
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

        public DataTable tbUfs()
        {
            ArrayList itens = new ArrayList();
            ArrayList cabecalho = new ArrayList();
            cabecalho.Add("uf");
            cabecalho.Add("ncm");
            cabecalho.Add("icms_interestadual");
            cabecalho.Add("iva_ajustado");
            cabecalho.Add("icms_estado");
            cabecalho.Add("CST");
            cabecalho.Add("porc_estorno_st");
            cabecalho.Add("nao_abate_aliq_origem");
            cabecalho.Add("mva");
            cabecalho.Add("mva_cons_final");
            cabecalho.Add("condicao_icms");
            cabecalho.Add("porc_icms");
            cabecalho.Add("tipo_reducao");
            cabecalho.Add("porc_reducao");
            cabecalho.Add("texto_nf");
            cabecalho.Add("cfop");
            cabecalho.Add("porc_combate_pobresa");
            cabecalho.Add("icms_estado_simples");
            cabecalho.Add("mva_simples");
            cabecalho.Add("tipo_reducao_simples");
            cabecalho.Add("porc_reducao_simples");
            cabecalho.Add("texto_nf_simples");
            cabecalho.Add("protocolo");
            cabecalho.Add("cod_tributacao");




            itens.Add(cabecalho);
            if (arrUfs != null && arrUfs.Count > 0)
            {
                foreach (aliquota_imp_estadoDAO item in arrUfs)
                {
                    itens.Add(item.ArrToString());

                }
            }
            return Conexao.GetArryTable(itens);
        }


        private void carregarUFs()
        {
            SqlDataReader rs = null;

            try
            {
                arrUfs.Clear();

                String sql = "Select rb.uf as Uf_estado, aie.*" +
                             " from regioes_brasil as rb " +
                                        " left outer join aliquota_imp_estado as aie on rb.uf=aie.uf and aie.ncm ='" + cf + "'  and aie.filial ='" + usr.getFilial() + "'" +

                               " order by rb.uf";
                rs = Conexao.consulta(sql, usr, true);
                while (rs.Read())
                {
                    aliquota_imp_estadoDAO objUf = new aliquota_imp_estadoDAO(usr);
                    objUf.uf = rs["Uf_estado"].ToString();
                    objUf.ncm = this.cf;
                    Decimal.TryParse(rs["icms_interestadual"].ToString(), out objUf.icms_interestadual);
                    Decimal.TryParse(rs["iva_ajustado"].ToString(), out objUf.iva_ajustado);
                    Decimal.TryParse(rs["icms_estado"].ToString(), out objUf.icms_estado);
                    objUf.CST = rs["CST"].ToString();
                    Decimal.TryParse(rs["porc_estorno_st"].ToString(), out objUf.porc_estorno_st);
                    objUf.nao_abate_aliq_origem = rs["nao_abate_aliq_origem"].ToString().Equals("1");
                    Decimal.TryParse(rs["mva"].ToString(), out objUf.mva);
                    Decimal.TryParse(rs["mva_cons_final"].ToString(), out objUf.mva_cons_final);
                    objUf.condicao_icms = (rs["condicao_icms"].ToString().Equals("") ? "1" : rs["condicao_icms"].ToString());
                    Decimal.TryParse(rs["porc_icms"].ToString(), out objUf.porc_icms);
                    objUf.tipo_reducao = rs["tipo_reducao"].ToString();
                    Decimal.TryParse(rs["porc_reducao"].ToString(), out objUf.porc_reducao);
                    objUf.texto_nf = rs["texto_nf"].ToString();
                    objUf.cfop = rs["cfop"].ToString();
                    Decimal.TryParse(rs["porc_combate_pobresa"].ToString(), out objUf.porc_combate_pobresa);
                    Decimal.TryParse(rs["icms_estado_simples"].ToString(), out objUf.icms_estado_simples);
                    Decimal.TryParse(rs["mva_simples"].ToString(), out objUf.mva_simples);
                    Decimal.TryParse(rs["porc_reducao_simples"].ToString(), out objUf.porc_reducao_simples);
                    objUf.texto_nf_simples = rs["texto_nf_simples"].ToString();
                    objUf.protocolo = rs["protocolo"].ToString();
                    objUf.tipo_reducao_simples = rs["tipo_reducao_simples"].ToString();
                    objUf.cod_tributacao = rs["cod_tributacao"].ToString();

                    arrUfs.Add(objUf);

                }
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

        public void carregarDados()
        {
            String sql = "Select * from  cf where cf ='" + cf + "'";
            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta(sql, usr, true);

                if (rs.Read())
                {
                    cf = rs["cf"].ToString();
                    descricao = rs["descricao"].ToString();
                    margem_iva = (Decimal)(rs["margem_iva"].ToString().Equals("") ? new Decimal() : rs["margem_iva"]);
                    margem_iva_ajustado = (Decimal)(rs["margem_iva_ajustado"].ToString().Equals("") ? new Decimal() : rs["margem_iva_ajustado"]);
                    Decimal.TryParse(rs["perc_imp_estadual"].ToString(), out perc_imp_estadual);
                    Decimal.TryParse(rs["perc_imp_fed_Nac"].ToString(), out perc_imp_fed_nac);
                    Decimal.TryParse(rs["perc_imp_fed_importado"].ToString(), out perc_imp_fed_importado);
                    Decimal.TryParse(rs["perc_pis"].ToString(), out perc_pis);
                    Decimal.TryParse(rs["perc_pis_entrada"].ToString(), out perc_pis_entrada);
                    cst_pis = rs["cst_pis"].ToString();
                    Decimal.TryParse(rs["perc_cofins"].ToString(), out perc_cofins);
                    Decimal.TryParse(rs["perc_cofins_entrada"].ToString(), out perc_cofins_entrada);
                    cst_cofins = rs["cst_cofins"].ToString();
                    CEST = rs["CEST"].ToString();

                    carregarUFs();
                }
                else
                {
                    //cf = rs["cf"].ToString();
                    descricao = "NÃO CADASTRADO";
                    margem_iva = 0; // (Decimal)(rs["margem_iva"].ToString().Equals("") ? new Decimal() : rs["margem_iva"]);
                    margem_iva_ajustado = 0; // (Decimal)(rs["margem_iva_ajustado"].ToString().Equals("") ? new Decimal() : rs["margem_iva_ajustado"]);
                    perc_imp_estadual=0;
                    perc_imp_fed_nac = 0;
                    perc_imp_fed_importado = 0;
                    perc_pis = 0;
                    perc_pis_entrada = 0;
                    cst_pis = "70";// rs["cst_pis"].ToString();
                    perc_cofins = 0;
                    perc_cofins_entrada = 0;
                    cst_cofins = "70"; // rs["cst_cofins"].ToString();
                    CEST = ""; // rs["CEST"].ToString();

                    //throw new Exception("NCM: " + cf + " NÃO CADASTRADO NA TELA DE CLASSIFICAÇÃO FISCAL");
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
                String sql = "update  cf set " +

                              "descricao='" + descricao + "'" +
                              ",margem_iva=" + margem_iva.ToString().Replace(",", ".") +
                              ",margem_iva_ajustado=" + margem_iva_ajustado.ToString().Replace(",", ".") +
                              ",perc_imp_estadual=" + perc_imp_estadual.ToString().Replace(",", ".") +
                              ",perc_imp_fed_nac=" + perc_imp_fed_nac.ToString().Replace(",", ".") +
                              ",perc_imp_fed_importado=" + perc_imp_fed_importado.ToString().Replace(",", ".") +
                              ",perc_pis =" + perc_pis.ToString().Replace(",", ".") +
                              ",perc_pis_entrada=" + perc_pis_entrada.ToString().Replace(",", ".") +
                              ",cst_pis='" + cst_pis + "'" +
                              ",perc_cofins=" + perc_cofins.ToString().ToString().Replace(",", ".") +
                              ",perc_cofins_entrada=" + perc_cofins_entrada.ToString().Replace(",", ".") +
                              ",cst_cofins='" + cst_cofins + "'" +
                              ",CEST='" + CEST + "'" +

                    "  where cf='" + cf + "' and filial ='" + usr.getFilial() + "'";
                Conexao.executarSql(sql, conn, tran);
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


                Conexao.executarSql("Delete from aliquota_imp_estado where ncm='" + cf + "' and filial ='" + usr.getFilial() + "'", conn, tran);
                foreach (aliquota_imp_estadoDAO item in arrUfs)
                {
                    item.ncm = this.cf;
                    item.salvar(true, conn, tran);
                }



                tran.Commit();
                return true;
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
        }

        public bool excluir()
        {
            String sql = "delete from cf  where cf='" + cf + "' and filial ='" + usr.getFilial() + "'"; //colocar campo index
            Conexao.executarSql(sql);
            return true;
        }

        public aliquota_imp_estadoDAO aliquotaUF(String uf)
        {

            foreach (aliquota_imp_estadoDAO item in arrUfs)
            {
                if (item.uf.Equals(uf))
                {
                    return item;
                }
            }
            return null;
        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = " insert into cf (" +
                          "cf," +
                          "descricao," +
                          "margem_iva," +
                          "margem_iva_ajustado" +
                          ",perc_imp_estadual" +
                          ",perc_imp_fed_nac" +
                          ",perc_imp_fed_importado" +
                          ",perc_pis " +
                          ",perc_pis_entrada" +
                          ",cst_pis" +
                          ",perc_cofins" +
                          ",perc_cofins_entrada" +
                          ",cst_cofins" +
                          ",CEST" +
                          ",filial" +
                     " )values (" +
                          "'" + cf + "'" +
                          "," + "'" + descricao + "'" +
                          "," + margem_iva.ToString().Replace(",", ".") +
                          "," + margem_iva_ajustado.ToString().Replace(",", ".") +
                          "," + perc_imp_estadual.ToString().Replace(",", ".") +
                          "," + perc_imp_fed_nac.ToString().Replace(",", ".") +
                          "," + perc_imp_fed_importado.ToString().Replace(",", ".") +
                          "," + perc_pis.ToString().Replace(",", ".") +
                          "," + perc_pis_entrada.ToString().Replace(",", ".") +
                          ",'" + cst_pis + "'" +
                          "," + perc_cofins.ToString().ToString().Replace(",", ".") +
                          "," + perc_cofins_entrada.ToString().Replace(",", ".") +
                          ",'" + cst_cofins + "'" +
                          ",'" + CEST + "'" +
                          ",'" + usr.getFilial() + "'" +
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
