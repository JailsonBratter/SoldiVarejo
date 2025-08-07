using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class Embaladora : visualSysWeb.code.PagePadrao
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
            txtID.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
        }

        protected void pesquisa()
        {
            User usr = (User)Session["User"];
            if (usr != null)
            {
                String strSql = "select * " +
                               " From Embaladoras WHERE Filial = '" + usr.filial.Filial + "' ";
                String strWhere = "";
                String strSqltotalFiltro = "select count(*) from Embaladoras WHERE Filial = '" + usr.filial.Filial + "'";
                String totalRegistro = Conexao.retornaUmValor(strSqltotalFiltro, null);

                try
                {
                    if (!txtID.Text.Equals(""))
                    {
                        strWhere = " ID = " + txtID.Text;
                    }

                    if(txtDescricao.Text.Equals(""))
                    {
                        strWhere += " Descricao like '%"+txtDescricao.Text+"%'";
                    }

                    if (strWhere.Length > 0)
                    {
                        strSql += " AND " + strWhere;
                        strSqltotalFiltro += " AND " + strWhere;
                    }

                    strSql = strSql + " ORDER BY ID desc";
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
            Response.Redirect("EmbaladoraDetalhes.aspx?novo=true");
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