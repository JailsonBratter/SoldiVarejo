using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.NotaFiscal.dao
{
    public class NfManifestoDAO
    {
        public String Filial { get; set; }
        private String _Nsu = "";
        public String NSU
        {
            get
            {
                if (_Nsu.Equals(""))
                    return "000000000000000";
                else
                    return _Nsu;
            }
            set
            {
                _Nsu = value;
            }
        }
        public Decimal vNF { get; set; }
        public String CNPJ { get; set; }
        public String RazaoSocial { get; set; }
        public String Chave { get; set; }
        public DateTime Emissao { get; set; }
        public String Status { get; set; }
        public String Xml
        {
            get
            {
                return (NfeXml != null && NfeXml.Length > 0 ? "SIM" : "NAO");
            }
            set { }
        }
        public String NfeXml { get; set; }
        public bool Lancada { get; set; }
        public string NotaLancada { get { return (Lancada ? "SIM" : "NAO"); } set {} }
        

        public String retornoManifestacao = "";
        private User usr = null;
        public string situacaoNFe { get; set; }
        public string naturezaOperacao { get; set; }

        public NfManifestoDAO(User usr)
        {
            this.usr = usr;
            this.Filial = usr.getFilial();
        }
        public NfManifestoDAO(String chave , User usr)
        {
            this.usr = usr;
            this.Filial = usr.getFilial();
            this.Chave = chave.Replace("NFe","");

            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta("select * from Nf_manifestar where chave='" + this.Chave + "'", usr, false);
                if (rs.Read())
                {
                    NSU = rs["NSU"].ToString();
                    vNF = Funcoes.decTry(rs["vNF"].ToString());
                    CNPJ = rs["CNPJ"].ToString();
                    RazaoSocial = rs["RazaoSocial"].ToString();
                    Emissao = Funcoes.dtTry(rs["Emissao"].ToString());
                    Status = rs["status"].ToString();
                    NfeXml = rs["nfeXML"].ToString();
                    naturezaOperacao = rs["Natureza_Operacao"].ToString();

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally{
                if (rs != null)
                    rs.Close();
            }

        }

        public void salvar(bool novo)
        {
            if (novo)
                insert();
            else
                update();
        }

        private void update()
        {
            String sql = "Update Nf_manifestar set " +
                "NSU='"+NSU+"'";
            if (NfeXml.Length > 0)
            {
                sql += ",nfeXml='" + NfeXml.Replace("'", "") + "'";
            }
            
            if (Status != null)
                sql += ",Status='" + Status + "'";
            sql += " Where CHAVE='" + Chave.Replace("NFe", "") + "'" +
                 " AND FILIAL='" + Filial + "'";
            Conexao.executarSql(sql);
        }

        public bool existeBD()
        {
            int qtd = Funcoes.intTry(Conexao.retornaUmValor("Select COUNT(*) from Nf_manifestar " +
                " Where CHAVE='" + Chave.Replace("NFe", "") + "'", usr));
            return (qtd > 0);

        }

        private void insert()
        {
            String sql = "insert into Nf_manifestar (" +
                "Filial" +
                ",NSU" +
                ",vNf" +
                ",CNPJ" +
                ",RazaoSocial" +
                ",Chave" +
                ",Emissao" +
                ",status" +
                ",nfeXML" +
                ",Natureza_Operacao"+
              ") Values(" +
                  "'" + Filial + "'" +
                  ",'" + NSU + "'" +
                  "," + Funcoes.decimalPonto(vNF.ToString()) +
                  ",'" + CNPJ + "'" +
                  ",'" + RazaoSocial + "'" +
                  ",'" + Chave.Replace("NFe","") + "'" +
                  "," + (Emissao.Equals(DateTime.MinValue) ? "null" : "'" + Emissao.ToString("yyyy-MM-dd") + "'") +
                  ",'" + (Status == null ? "NAO MANIFESTADO" : Status) + "'" +
                  ",'" + NfeXml.Replace("'", "") + "'" +
                  ", '" + naturezaOperacao + "'"+ 
              ");";
            Conexao.executarSql(sql);

        }
        public static List<NfManifestoDAO> notasManifestos(String chave,String razaoSocial , String tipoData, DateTime dtDe, DateTime dtAte, String status,String lancado, User usr)
        {
            List<NfManifestoDAO> lista = new List<NfManifestoDAO>();
            SqlDataReader rs = null;

            try
            {
                String sql = " Select *,lancado=(Select count(*) from nf where id =Nf_manifestar.Chave)" +
                    ", CASE WHEN isnull(Situacao, 0) = 1 THEN '' " +
                    " WHEN isnull(Situacao, 0) = 3 THEN 'CANCELADA' " +
                    " ELSE 'NÃO RETORNADO' END AS SituacaoNFe, ISNULL(Natureza_Operacao, '') As Natureza FROM Nf_manifestar  " +
                    "WHERE  nf_manifestar.filial='"+usr.getFilial()+"'  and ";
                if (chave.Length > 0)
                    sql += "chave='" + chave.Replace("NFe", "") + "'";
                else
                {
                    if (tipoData.Equals("ENTRADA"))
                    {
                        sql += " nf_manifestar.Chave in (Select nf.id from nf where nf.data between '" + dtDe.ToString("yyyy-MM-dd") + "' and '" + dtAte.ToString("yyyy-MM-dd") + "')"; 
                    }
                    else
                    {
                        sql += "emissao between '" + dtDe.ToString("yyyy-MM-dd") + "' and '" + dtAte.ToString("yyyy-MM-dd") + "'";
                    }

                    if (!status.Equals("TODOS"))
                        sql += " and status ='" + status + "'";
                }

                if(razaoSocial.Length>0)
                    sql+=" and razaoSocial like '%"+razaoSocial+"%'";


                if(lancado != "TODOS")
                {
                    sql += " and (Select count(*) from nf where id =Nf_manifestar.Chave)  ="+(lancado.Equals("SIM") ? "1" : "0");
                }
                sql += " order by RazaoSocial ";

                rs = Conexao.consulta(sql, usr, false);
                while (rs.Read())
                {

                    NfManifestoDAO item = new NfManifestoDAO(usr);
                    item.NSU = rs["NSU"].ToString();
                    item.Chave = rs["Chave"].ToString();
                    item.CNPJ = rs["CNPJ"].ToString();
                    item.RazaoSocial = rs["RazaoSocial"].ToString();
                    item.vNF = Funcoes.decTry(rs["vNf"].ToString());
                    item.Emissao = Funcoes.dtTry(rs["Emissao"].ToString());
                    item.NfeXml = rs["nfeXml"].ToString();
                    item.Status = rs["status"].ToString();
                    item.Lancada = (Funcoes.intTry(rs["lancado"].ToString()) > 0);
                    item.situacaoNFe = rs["SituacaoNFe"].ToString();
                    item.naturezaOperacao = rs["Natureza"].ToString();
                    lista.Add(item);

                }

                if (lista.Count == 0)
                {
                    NfManifestoDAO item = new NfManifestoDAO(usr);
                    lista.Add(item);
                }
                lista = lista.OrderBy(o => o.RazaoSocial).ToList();
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

            return lista;
        }
    }
}