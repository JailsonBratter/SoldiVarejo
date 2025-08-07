using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Manutencao.dao
{
    public class Soldi_Gusto_CE_DepartamentoDAO
    {

        private string _grupoGraficoAnt = "";
        private string _grupoGrafico = "";
        public string grupo_grafico
        {
            get
            {
                return _grupoGrafico;

            }
            set
            {
                _grupoGraficoAnt = _grupoGrafico;
                _grupoGrafico = value;
            }
        }
        public string descricao { get; set; }
        private string _codigo_departamentoAnt = "";
        private string _codigo_departamento = "";
        public string codigo_departamento
        {
            get
            {
                return _codigo_departamento;
            }
            set
            {
                _codigo_departamentoAnt = _codigo_departamento;
                _codigo_departamento = value;
            }
        }
        public bool Dep_ativa_CE { get; set; }


        public void salvar()
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                excluir(conn, tran);
                insert(conn, tran);

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

        }


        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            String sql = "insert into  Soldi_Gusto_CE_Departamento (";
            String values = ") values (";
            sql += "Grupo_Grafico";
            values += "'" + grupo_grafico + "'";

            sql += ",descricao";
            values += ",'" + descricao + "'";

            sql += ",Codigo_departamento";
            values += ",'" + codigo_departamento + "'";

            sql += ",Dep_Ativa_CE";
            values += "," + (Dep_ativa_CE ? "1" : "0");

            sql += values + ");";

            Conexao.executarSql(sql, conn, tran);
        }

        public void exclui()
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                excluir(conn, tran);

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

        }
        private void excluir(SqlConnection conn, SqlTransaction tran)
        {
            Conexao.executarSql("DELETE FROM  Soldi_Gusto_CE_Departamento " +
            " WHERE GRUPO_GRAFICO ='" + _grupoGraficoAnt + "' AND CODIGO_DEPARTAMENTO ='" + _codigo_departamentoAnt + "'", conn, tran);

        }
    }
}