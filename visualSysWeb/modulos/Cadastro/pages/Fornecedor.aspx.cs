using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using visualSysWeb.modulos.Cadastro.code;
using visualSysWeb.code;

namespace visualSysWeb.Cadastro
{
    public partial class Fornecedor : visualSysWeb.code.PagePadrao
    {
        static DataTable tb;
        static String UltimaOrdem = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FiltroFornecedor filtro = (FiltroFornecedor)Session["filtroFornecedor"];
                if (filtro != null)
                {
                    txtCNPJ.Value = filtro.cnpj;
                    txtFornecedor.Value = filtro.fornecedor;
                    ddlTipoFornecedor.Text = filtro.tipo;
                    txtContaContabil.Value = filtro.contaContabil;

                }

            }
            if (!IsPostBack)
            {

                pesquisar();
            }
            pesquisar(pnBtn);
            camposnumericos();
        }

        private void camposnumericos()
        {
            txtCNPJ.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
        }

        protected void gridFornecedor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridFornecedor.DataSource = tb;
            gridFornecedor.PageIndex = e.NewPageIndex;
            gridFornecedor.DataBind();
        }




        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/Cadastro/pages/FornecedorDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void pesquisar()
        {

            FiltroFornecedor filtro = new FiltroFornecedor();
            filtro.cnpj = txtCNPJ.Value;
            filtro.fornecedor = txtFornecedor.Value;
            filtro.tipo = ddlTipoFornecedor.Text;
            filtro.contaContabil = txtContaContabil.Value;
            Session.Remove("filtroFornecedo");
            Session.Add("filtroFornecedor", filtro);

            String sql = "select fornecedor" +
                        ",strfornecedor=replace(fornecedor,'&','|E')" +
                        ",Razao_social,CNPJ " +
                        ",TIPO_FORNECEDOR = case when tipo_fornecedor = 1 then 'ADMINISTRATIVO' ELSE 'COMERCIAL' END "+
                        ",Conta_Contabil = ISNULL(Conta_Contabil_Credito, '') " +
                        " from fornecedor ";
            String sqlWhere = "";
            String strSqltotalFiltro = "select count(*)  from fornecedor ";
            String totalRegistro = Conexao.retornaUmValor(strSqltotalFiltro, null);

            if (!txtFornecedor.Value.Equals(""))
            {
                sqlWhere = " (fornecedor like '%" + txtFornecedor.Value + "%' or razao_social like '%" + txtFornecedor.Value + "%') ";

            }

            if (!txtCNPJ.Value.Equals(""))
            {
                if (sqlWhere.Length > 0)
                    sqlWhere += " and ";

                sqlWhere += " replace(replace(replace(cnpj,'.',''),'-',''),'/','') like '%" + txtCNPJ.Value.Replace(".", "").Replace("-", "").Replace("/", "") + "%'";
            }

            if (!ddlTipoFornecedor.Text.Equals("TODOS"))
            {
                if (sqlWhere.Length > 0)
                    sqlWhere += " and ";

                sqlWhere += " isnull(tipo_fornecedor,0) =" + (ddlTipoFornecedor.Text.Equals("ADMINISTRATIVO")?"1":"0");
            }

            if (!txtContaContabil.Value.Equals(""))
            {
                if (sqlWhere.Length > 0)
                    sqlWhere += " and ";

                sqlWhere += " ISNULL(conta_contabil_credito, '') = '" + txtContaContabil.Value.ToString() + "'";
            }


            try
            {

                User usr = (User)Session["User"];
                strSqltotalFiltro = "select count(*)  from (";
                if (!sqlWhere.Equals(""))
                {
                    tb = Conexao.GetTable(sql + " Where " + sqlWhere, usr, false);
                    strSqltotalFiltro += sql + " Where " + sqlWhere;
                }
                else//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                {
                    tb = Conexao.GetTable(sql, usr, true);
                    strSqltotalFiltro += sql.Replace("select","select top 100 ") ;
                }
                strSqltotalFiltro += ") as  a ";
                gridFornecedor.DataSource = tb;
                gridFornecedor.DataBind();
                lblPesquisaErro.Text = "";
                String totalFiltro = Conexao.retornaUmValor(strSqltotalFiltro, usr);
                lblRegistros.Text = totalFiltro + " Registros de  " + totalRegistro + " Cadastrados ";
            }
            catch (Exception err)
            {

                lblPesquisaErro.Text = err.Message;
            }
        }
        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisar();
            HyperLink meuLink = (HyperLink)gridFornecedor.Rows[0].Cells[0].Controls[0];
            if (gridFornecedor.Rows.Count == 1 && !meuLink.Text.Equals("------"))
            {
                Response.Redirect("FornecedorDetalhes.aspx?fornecedor=" + meuLink.Text);
            }
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
        protected override bool campoObrigatorio(Control campo)
        {
            String[] campos = { "", 
                           "", 
                           "",
                           "",
                           ""
                          };

            for (int i = 0; i < campos.Length; i++)
            {
                if (campo.ClientID.Equals(campos[i]))
                {

                    return true;
                }
            }
            return false;

        }




        protected override bool campoDesabilitado(Control campo)
        {
            String[] campos = { "", 
                           ""
                          };

            for (int i = 0; i < campos.Length; i++)
            {
                if (campo.ClientID.Equals(campos[i]))
                {

                    return true;
                }
            }
            return false;
        }

        protected void btnTransmitir_Click(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];

            if (Funcoes.valorParametro("INTEGRA_WS", usr).Equals("RAKUTEN"))
            {
                DataTable dtWS = Conexao.GetTable("select * from fornecedor", usr, false);
                if (dtWS.Rows.Count > 2)
                {
                    foreach (DataRow row in dtWS.Rows)
                    {
                        fornecedorDAO fornKCWS = new fornecedorDAO(row[0].ToString(), usr);
                        //KCWSFabricante forn = new KCWSFabricante(fornKCWS, "Salvar",usr);
                    }
                }
            }
        }

        protected void gridFornecedor_Sorting(object sender, GridViewSortEventArgs e)
        {
            //   pesquisar(e.SortExpression);
            String ordem = e.SortExpression;
            if (ordem.Equals(UltimaOrdem))
            {
                if (ordem.IndexOf("Desc") < 0)
                    ordem += " Desc";

                UltimaOrdem = "";
            }
            else
            {
                UltimaOrdem = ordem;
            }

            if (!ordem.Equals(" Desc"))
            {
                DataView dv = tb.DefaultView;
                dv.Sort = ordem;
                tb = dv.ToTable();
            }
            gridFornecedor.DataSource = tb;
            gridFornecedor.DataBind();

        }
    }
}