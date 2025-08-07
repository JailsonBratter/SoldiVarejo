using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class PDV : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pesquisa();
            }
            pesquisar(pnBtn);
            camposnumericos();
        }



        private void camposnumericos()
        {
            txtPDV.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
        }

        protected void pesquisa()
        {
            User usr = (User)Session["User"];
            if (usr != null)
            {
                String strSql = "select Filial,"+
                                       " Modelo,"+
                                       " FabEqu,"+
                                       " Caixa AS PDV,"+
                                       " SAT = CASE WHEN SAT = 1 THEN 'SIM' ELSE 'NAO' END,"+
		                               " Tipo_carga = CASE WHEN ATIVA_LINK_SERVER = 1 THEN 'LINK SERVER' ELSE 'ARQUIVO' END,"+
		                               " Endereco = CASE WHEN ATIVA_LINK_SERVER = 1 THEN Link_server ELSE Diretorio_Carga END,"+
		                               " DATA_ULT_ATUALIZACAO = CONVERT(VARCHAR, DATA_ULT_ATUALIZACAO, 103)"+
                               " From Controle_Filial_PDV WHERE Filial = '" + usr.filial.Filial + "'";
                String strWhere = "";
                String strSqltotalFiltro = "select count(*) from Controle_Filial_PDV WHERE Filial = '" + usr.filial.Filial + "'";
                String totalRegistro = Conexao.retornaUmValor(strSqltotalFiltro, null);

                try
                {
                    if (!txtPDV.Text.Equals(""))
                    {
                        strWhere = " CAIXA = " + txtPDV.Text;
                    }

                    if (strWhere.Length > 0)
                    {
                        strSql += " AND " + strWhere;
                        strSqltotalFiltro += " AND " + strWhere;
                    }

                    strSql = strSql + "ORDER BY Caixa";
                    String totalFiltro = Conexao.retornaUmValor(strSqltotalFiltro, usr);
                    lblRegistros.Text = totalFiltro + " Registros de  " + totalRegistro + " Cadastrados ";
                    gridPesquisa.DataSource = Conexao.GetTable(strSql, usr, true);
                    gridPesquisa.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Não Encontrado:" + ex.Message);
                }
            }
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("PDVDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisa();
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
    }
}