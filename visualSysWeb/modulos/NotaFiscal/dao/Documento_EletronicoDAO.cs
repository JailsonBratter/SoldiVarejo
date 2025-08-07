using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using visualSysWeb.code;
using visualSysWeb.modulos.NotaFiscal.code;
using visualSysWeb.modulos.NotaFiscal.dao;

namespace visualSysWeb.dao
{
    public class Documento_EletronicoDAO
    {
        #region Propriedades
        public string filial { get; set; }
        public int tipo { get; set; }
        public DateTime data { get; set; }
        public int caixa { get; set; }
        public string documento { get; set; }
        public string id_chave { get; set; }
        public string id_chave_cancelamento { get; set; }
        public string nro_serie_equipamento { get; set; }
        public string operador { get; set; }
        public string cfe_xml { get; set; }
        public string cfe_xml_cancelamento { get; set; }
        #endregion


        public Documento_EletronicoDAO()
        {
        }

        public bool insert()
        {
            SqlConnection conn = Conexao.novaConexao();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    string sql = "INSERT INTO Documento_Eletronico (";
                    sql += "Filial, Tipo, Data, Caixa, Documento, ID_Chave, ID_Chave_Cancelamento, Nro_Serie_Equipamento, Operador, CFe_XML, CFe_XML_Cancelamento";
                    sql += ") VALUES (";
                    sql += "'" + this.filial + "'";
                    sql += ", " + this.tipo.ToString();
                    sql += ", '" + this.data.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    sql += ", " + this.caixa.ToString(); //Caixa
                    sql += ", '" + this.documento.Trim() + "'";
                    sql += ", '" + this.id_chave + "'";
                    sql += ", '" + this.id_chave_cancelamento + "'";
                    sql += ", '" + this.nro_serie_equipamento + "'"; //Número de série
                    sql += ", '" + this.operador + "'"; //Operador
                    sql += ", '" + this.cfe_xml.Replace("'", "") + "'";
                    sql += ", '" + this.cfe_xml_cancelamento.Replace("'", "") + "'"; ;
                    sql += ")";
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                    SqlConnection.ClearPool(conn);
                }
            }
        }

        public bool exists(string chave)
        {
            try
            {
                if (Funcoes.intTry(Conexao.retornaUmValor("SELECT * FROM Documento_Eletronico WHERE ID_Chave = '" + chave + "'", null)) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}