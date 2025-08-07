using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace visualSysWeb.dao
{
    public class aliquota_imp_estadoDAO
    {
        public String uf = "";
        public String ncm = "";
        public Decimal icms_interestadual = 0;
        public Decimal iva_ajustado = 0;
        public Decimal icms_estado = 0;
        public String CST = "";

        //Novos Campos
        public Decimal porc_estorno_st = 0;
        public bool nao_abate_aliq_origem = false;
        public Decimal mva = 0;
        public Decimal mva_cons_final = 0;
        public String condicao_icms = "1";
        public Decimal porc_icms = 0;
        public String tipo_reducao = "";
        public Decimal porc_reducao = 0;
        public String texto_nf = "";
        public String cfop = "";
        public Decimal porc_combate_pobresa = 0;
        public Decimal icms_estado_simples = 0;
        public Decimal mva_simples = 0;
        public String tipo_reducao_simples = "";
        public Decimal porc_reducao_simples = 0;
        public String texto_nf_simples = "";
        public String protocolo = "";
        public tributacaoDAO trib = null;
        public String descricao_tributacao
        {
            get
            {
                if (trib != null)
                    try
                    {
                        return (trib.Descricao_Tributacao == null ? "" : trib.Descricao_Tributacao);
                    }
                    catch
                    {
                        return "";
                    }
                else
                    return "";
            }
        }

        public String cod_tributacao
        {
            get
            {
                if (trib != null)
                    return trib.Codigo_Tributacao.ToString();
                else
                    return "0";
            }
            set
            {
                if (!value.Equals(""))
                {
                    trib = new tributacaoDAO(value, null);
                }


            }
        }
        User usr = null;

        public aliquota_imp_estadoDAO(User usr)
        {
            this.usr = usr;
        }

        public aliquota_imp_estadoDAO(String uf, String ncm, User usr)
        { //colocar campo index da tabela
            this.usr = usr;
            String sql = "Select * from  aliquota_imp_estado where uf ='" + uf + "' and ncm='" + ncm + "' and filial ='MATRIZ'";
            SqlDataReader rs = Conexao.consulta(sql, null, true);
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

        public ArrayList ArrToString()
        {

            ArrayList item = new ArrayList();
            item.Add(uf);
            item.Add(ncm);
            item.Add(icms_interestadual.ToString("N2"));
            item.Add(iva_ajustado.ToString("N2"));
            item.Add(icms_estado.ToString("N2"));
            item.Add(CST);
            item.Add(porc_estorno_st.ToString("N2"));
            item.Add((nao_abate_aliq_origem ? "1" : "0"));
            item.Add(mva.ToString("N2"));
            item.Add(mva_cons_final.ToString("N2"));
            item.Add(condicao_icms);
            item.Add(porc_icms.ToString("N2"));
            item.Add(tipo_reducao);
            item.Add(porc_reducao.ToString("N2"));
            item.Add(texto_nf);
            item.Add(cfop.ToString());
            item.Add(porc_combate_pobresa.ToString("N2"));
            item.Add(icms_estado_simples.ToString("N2"));
            item.Add(mva_simples.ToString("N2"));
            item.Add(tipo_reducao_simples);
            item.Add(porc_reducao_simples.ToString("N2"));
            item.Add(texto_nf_simples);
            item.Add(protocolo);
            item.Add(cod_tributacao);

            return item;

        }

        public void carregarDados(SqlDataReader rs)
        {
            try
            {


                if (rs.Read())
                {
                    uf = rs["uf"].ToString();
                    ncm = rs["ncm"].ToString();
                    icms_interestadual = (Decimal)(rs["icms_interestadual"].ToString().Equals("") ? new Decimal() : rs["icms_interestadual"]);
                    iva_ajustado = (Decimal)(rs["iva_ajustado"].ToString().Equals("") ? new Decimal() : rs["iva_ajustado"]);
                    icms_estado = (Decimal)(rs["icms_estado"].ToString().Equals("") ? new Decimal() : rs["icms_estado"]);

                    CST = rs["CST"].ToString();


                    //Novos Campos

                    Decimal.TryParse(rs["porc_estorno_st"].ToString(), out porc_estorno_st);
                    nao_abate_aliq_origem = rs["nao_abate_aliq_origem"].ToString().Equals("1");
                    Decimal.TryParse(rs["mva"].ToString(), out mva);
                    Decimal.TryParse(rs["mva_cons_final"].ToString(), out mva_cons_final);
                    condicao_icms = rs["condicao_icms"].ToString();
                    Decimal.TryParse(rs["porc_icms"].ToString(), out porc_icms);
                    tipo_reducao = rs["tipo_reducao"].ToString();
                    Decimal.TryParse(rs["porc_reducao"].ToString(), out porc_reducao);
                    texto_nf = rs["texto_nf"].ToString();
                    cfop = rs["cfop"].ToString();
                    Decimal.TryParse(rs["porc_combate_pobresa"].ToString(), out porc_combate_pobresa);
                    Decimal.TryParse(rs["icms_estado_simples"].ToString(), out icms_estado_simples);
                    Decimal.TryParse(rs["mva_simples"].ToString(), out mva_simples);
                    tipo_reducao_simples = rs["tipo_reducao_simples"].ToString();
                    Decimal.TryParse(rs["porc_reducao_simples"].ToString(), out porc_reducao_simples);
                    texto_nf_simples = rs["texto_nf_simples"].ToString();
                    protocolo = rs["protocolo"].ToString();


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
                String sql = "update  aliquota_imp_estado set " +
                              "uf='" + uf + "'" +
                              ",ncm='" + ncm + "'" +
                              ",icms_interestadual=" + icms_interestadual.ToString().Replace(",", ".") +
                              ",iva_ajustado=" + iva_ajustado.ToString().Replace(",", ".") +
                              ",icms_estado=" + icms_estado.ToString().Replace(",", ".") +
                              ",CST ='" + CST + "'" +

                               //novos campos 
                               ",porc_estorno_st= " + porc_estorno_st.ToString().Replace(",", ".") +
                               ",nao_abate_aliq_origem = " + (nao_abate_aliq_origem ? "1" : "0") +
                               ",mva = " + mva.ToString().Replace(",", ".") +
                               ",mva_cons_final=" + mva_cons_final.ToString().Replace(",", ".") +
                               ",condicao_icms = '" + condicao_icms + "'" +
                               ",porc_icms=" + porc_icms.ToString().Replace(",", ".") +
                               ",tipo_reducao = '" + tipo_reducao + "'" +
                               ",porc_reducao= " + porc_reducao.ToString().Replace(",", ".") +
                               ",texto_nf = '" + texto_nf + "'" +
                               ",cfop = '" + cfop + "'" +
                               ",porc_combate_pobresa=" + porc_combate_pobresa.ToString().Replace(",", ".") +
                               ",icms_estado_simples= " + icms_estado_simples.ToString().Replace(",", ".") +
                               ",mva_simples= " + mva_simples.ToString().Replace(",", ".") +
                               ",tipo_reducao_simples = '" + tipo_reducao_simples + "'" +
                               ",porc_reducao_simples= " + porc_reducao_simples.ToString().Replace(",", ".") +
                               ",texto_nf_simples = '" + texto_nf_simples + "'" +
                               ",protocolo = '" + protocolo + "'" +
                               ",codigo_tributacao =" + cod_tributacao +

                    "  where uf='" + uf + "' and ncm='" + ncm + "' and filial ='" + usr.getFilial() + "'"
                        ;
                Conexao.executarSql(sql, conn, tran);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }
        public bool salvar(bool novo, SqlConnection conn, SqlTransaction tran)
        {
            if (novo)
            {
                insert(conn, tran);
            }
            else
            {
                update(conn, tran);
            }
            return true;
        }

        public bool excluir()
        {
            String sql = "delete from aliquota_imp_estado  where uf= '" + uf + "' and ncm ='" + ncm + "'"; //colocar campo index
            Conexao.executarSql(sql);
            return true;
        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = " insert into aliquota_imp_estado (" +
                          "uf," +
                          "ncm," +
                          "icms_interestadual," +
                          "iva_ajustado," +
                          "icms_estado," +
                          "CST" +
                          ",porc_estorno_st" +
                          ",nao_abate_aliq_origem " +
                          ",mva " +
                          ",mva_cons_final" +
                          ",condicao_icms " +
                          ",porc_icms" +
                          ",tipo_reducao " +
                          ",porc_reducao " +
                          ",texto_nf " +
                          ",cfop " +
                          ",porc_combate_pobresa" +
                          ",icms_estado_simples" +
                          ",mva_simples " +
                          ",tipo_reducao_simples " +
                          ",porc_reducao_simples " +
                          ",texto_nf_simples " +
                          ",protocolo " +
                          ",cod_tributacao" +
                          ",filial" +

                     " )values (" +
                        "'" + uf + "'" +
                        "," + "'" + ncm + "'" +
                        "," + icms_interestadual.ToString().Replace(",", ".") +
                        "," + iva_ajustado.ToString().Replace(",", ".") +
                        "," + icms_estado.ToString().Replace(",", ".") +
                        ",'" + CST + "'" +

                        //novos Campos
                        "," + porc_estorno_st.ToString().Replace(",", ".") +
                        "," + (nao_abate_aliq_origem ? "1" : "0") +
                        "," + mva.ToString().Replace(",", ".") +
                        "," + mva_cons_final.ToString().Replace(",", ".") +
                        ",'" + condicao_icms + "'" +
                        "," + porc_icms.ToString().Replace(",", ".") +
                        ",'" + tipo_reducao + "'" +
                        "," + porc_reducao.ToString().Replace(",", ".") +
                        ",'" + texto_nf + "'" +
                        ",'" + cfop + "'" +
                        "," + porc_combate_pobresa.ToString().Replace(",", ".") +
                        "," + icms_estado_simples.ToString().Replace(",", ".") +
                        "," + mva_simples.ToString().Replace(",", ".") +
                        ",'" + tipo_reducao_simples + "'" +
                        "," + porc_reducao_simples.ToString().Replace(",", ".") +
                        ",'" + texto_nf_simples + "'" +
                        ",'" + protocolo + "'" +
                        "," + cod_tributacao +
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
