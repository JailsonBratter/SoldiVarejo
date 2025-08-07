using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class PromocaoJornal : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pesquisa();
            }
            pesquisar(pnBtn);
        }
                
        protected void pesquisa()
        {
            User usr = (User)Session["User"];
            if (usr != null)
            {
                String strSql = "select filial, status, codigo, descricao, inicio, fim" +
                                ", Data_Inicio = CONVERT(VARCHAR, inicio, 103)" +
                                ", Data_Fim = CONVERT(VARCHAR, fim, 103)" +
                                " From PromocaoJornal WHERE Filial = '" + usr.filial.Filial + "'";
                String strWhere = "";
                String strSqltotalFiltro = "select count(*) from PromocaoJornal WHERE Filial = '" + usr.filial.Filial + "'";
                String totalRegistro = Conexao.retornaUmValor(strSqltotalFiltro, null);

                try
                {
                    if (!txtStatus.Text.Equals(""))
                    {
                        strWhere += " AND  Status = " + txtStatus.Text;
                    }
                    if (!txtCodigo.Text.Equals(""))
                    {
                        strWhere += " AND  Codigo = " + txtCodigo.Text;
                    }
                    if (!txtDescricao.Text.Trim().Equals(""))
                    {
                        strWhere += " AND  Descricao like '%" + txtDescricao.Text+"%'";
                    }
                    if(!txtDataInicio.Text.Trim().Equals("") && !txtDataFim.Text.Trim().Equals(""))
                    {
                        string inicio = Funcoes.dtTry(txtDataInicio.Text).ToString("yyyy-MM-dd");
                        string fim = Funcoes.dtTry(txtDataFim.Text).ToString("yyyy-MM-dd");
                        strWhere += " AND ('" + inicio + "' between inicio and fim " +
                            " OR '" + fim + "' between inicio and fim  " +
                            " OR Inicio between '"+inicio+"' and '"+fim+"'"+
                            " OR Fim between '"+inicio+"' and '"+fim+"')" ;
                    }

                    if (strWhere.Length > 0)
                    {
                        strSql += strWhere;
                        strSqltotalFiltro +=  strWhere;
                    }

                    strSql = strSql + " ORDER BY Descricao";
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
            Response.Redirect("PromocaoJornalDetalhes.aspx?novo=true");
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
