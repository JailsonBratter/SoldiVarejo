using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using System.Data.SqlClient;
using System.Data;
using visualSysWeb.code;
using System.Collections;
using System.Web.Services;

namespace visualSysWeb.modulos.Relatorios.pages
{
    public partial class RelComercial : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                User usr = (User)Session["User"];
                status = "sopesquisa";
                //carregarDados();
            }

            carregabtn(pnBtn);
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            DataTable Tb01 = new DataTable();
            DataTable Tb02 = new DataTable();
            DataTable Tb03 = new DataTable();

            User usr = (User)Session["User"];
            string sql = "";
            string strWhere = "";

            if (!txtDescricao.Text.Equals(""))
            {
                strWhere = strWhere + " AND Descricao LIKE '%" + txtDescricao.Text + "%'";
            }

            if (!txtDescricaoGrupo.Text.Equals(""))
            {
                strWhere = strWhere + " AND Grupo_Descricao LIKE '%" + txtDescricaoGrupo.Text + "%'";
            }

            if (!txtDescricaoSubGrupo.Text.Equals(""))
            {
                strWhere = strWhere + " AND SubGrupo_Descricao LIKE '%" + txtDescricaoSubGrupo.Text + "%'";
            }

            if (!txtDescricaoDepto.Text.Equals(""))
            {
                strWhere = strWhere + " AND Departamento_Descricao LIKE '%" + txtDescricaoDepto.Text + "%'";
            }

            for (int i = -12; i != 1; i++)
            {
                sql = sql + "SELECT ANOMES = SUBSTRING(CONVERT(VARCHAR, DATEADD(MONTH, " + 
                    i.ToString() + ", GETDATE()), 102), 1,7), " +
                    "Qtde = SUM(VQMES" + ((i * -1) + 1).ToString("00") + "), " +
                    "Vlr = SUM(VVMES" + ((i * -1) + 1).ToString("00") + "), " +
                    "Lucro = SUM(VVMES" + ((i * -1) + 1).ToString("00") + " - (VQMES" + ((i * -1) + 1).ToString("00") + " * CUSTOMES" + ((i * -1) + 1).ToString("00") + ")) " +
                   // "PrcMD = CONVERT(NUMERIC(12,2), SUM(VQMES" + ((i * -1) + 1).ToString("00") + ")/SUM(VVMES" + ((i * -1) + 1).ToString("00") + "))" +
                    " FROM Acumulado_Geral WHERE Filial = '" + usr.filial.Filial + "'" + strWhere;

                if (i < 0)
                {
                    sql = sql + " UNION ALL ";
                }
            }

            sql = sql + " ORDER BY 1 DESC";

            Tb01 = Conexao.GetTable(sql, null, false);

            sql = "SELECT TOP 30 PLU, DESCRICAO, DEPARTAMENTO, DEPARTAMENTO_DESCRICAO," ;
            sql = sql + " QTDE = SUM(VQMES13 +VQMES12 + VQMES11 + VQMES10 + VQMES09 + VQMES08 + VQMES07 + VQMES06 + VQMES05 + VQMES04 + VQMES03 + VQMES02 + VQMES01)," ;
            sql = sql + " VLR = SUM(VVMES13 + VVMES12 + VVMES11 + VVMES10 + VVMES09 + VVMES08 + VVMES07 + VVMES06 + VVMES05 + VVMES04 + VVMES03 + VVMES02 + VVMES01)," ;
            sql = sql + " PrcMD = CONVERT(NUMERIC(12,2), 0)";
            sql = sql + " FROM Acumulado_Geral WHERE Filial = '" + usr.filial.Filial + "'" + strWhere;
            sql = sql + " GROUP BY PLU, DESCRICAO, DEPARTAMENTO, DEPARTAMENTO_dESCRICAO";
            sql = sql + " ORDER BY 6 DESC";

            Tb02 = Conexao.GetTable(sql, null, false);

            sql = "SELECT GRUPO_DESCRICAO,";
            sql = sql + " VLR = SUM(VVMES13 + VVMES12 + VVMES11 + VVMES10 + VVMES09 + VVMES08 + VVMES07 + VVMES06 + VVMES05 + VVMES04 + VVMES03 + VVMES02 + VVMES01)";
            sql = sql + " FROM Acumulado_Geral WHERE Filial = '" + usr.filial.Filial + "'" + strWhere;
            sql = sql + " GROUP BY GRUPO_DESCRICAO";

            Tb03 = Conexao.GetTable(sql, null, false);
            
            GridVendas.DataSource = Tb01;
            GridVendas.DataBind();

            Grafico01.DataSource = Tb01;
            Grafico01.DataBind();

            Grid30Mais.DataSource = Tb02;
            Grid30Mais.DataBind();

            Grafico02.DataSource = Tb03;
            Grafico02.DataBind();

            //Grid30Mais.DataSource = Tb01;
            //Grid30Mais.DataBind();
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override bool campoDesabilitado(Control campo)
        {
            throw new NotImplementedException();
        }

        protected override bool campoObrigatorio(Control campo)
        {
            throw new NotImplementedException();
        }

        protected void Chart1_Load(object sender, EventArgs e)
        {

        }

    }
}